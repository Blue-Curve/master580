Imports BlueCurve.Core.CS
Imports System.IO
Imports System.Xml.Serialization
REM ===============================================
REM Bluecurve Limited 2005
REM Module:       Insight Excel Submission Object 
REM                Model
REM Type:         Object Model
REM Description: 
REM Components, 
REM sub Components
REM Version:      1
REM Change history
REM ===============================================
REM class to hold propogation of Excel submission through class
REM hierarchies
REM FIL FEB 2012
#Region "changes"
'Changes:
'Tracker                 Initials                   Date                      Synopsis
'FIL 8306                PR                         8/1/2014                  Dual Target class
'FIL 7964                PR                         8/1/2014                  extended timeout class
#End Region
REM FIL 5.5
<Serializable()> Public Class bc_om_rtd_functions
    Inherits bc_cs_soap_base_class
    Public functions As New List(Of bc_om_rtd_function)
    Public lookups As New List(Of bc_om_param_lookup)
    Public default_values As New List(Of bc_om_param_lookup)
    Public classes As New bc_om_entity_classes
    Public entities As New bc_om_entities
    Public item_names As New List(Of String)

    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_in_excel_submission_db
            Dim res As Object
            Dim mfunction As bc_om_rtd_function
            res = gdb.get_ef_macros(certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)

                    mfunction = New bc_om_rtd_function
                    mfunction.certificate = MyBase.certificate
                    mfunction.name = res(0, i)
                    mfunction.helptext = res(1, i)

                    mfunction.id = res(3, i)
                    mfunction.display_name = res(4, i)

                    mfunction.db_read()
                    If res(5, i) <> 0 Then
                        'If mfunction.display_name <> "" Then
                        functions.Add(mfunction)
                    End If


                Next
            End If
            Dim db As New bc_cs_db_services


            Dim oparam As New bc_om_rtd_function_param
            res = db.executesql("exec dbo.bc_core_get_rtd_excel_params_lookups", certificate)
            Dim ares As Object
            Dim lu As bc_om_param_lookup
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    ares = db.executesql(res(0, i), certificate)
                    If IsArray(ares) Then
                        For j = 0 To UBound(ares, 2)
                            lu = New bc_om_param_lookup
                            lu.lookupsql = res(0, i)
                            lu.value = ares(0, j)
                            lookups.Add(lu)
                        Next
                    End If
                Next
            End If

            res = db.executesql("exec dbo.bc_core_get_rtd_excel_params_defaults", certificate)
            
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    ares = db.executesql(res(0, i), certificate)
                    If IsArray(ares) Then
                        For j = 0 To UBound(ares, 2)
                            lu = New bc_om_param_lookup
                            lu.lookupsql = res(0, i)
                            lu.value = ares(0, j)
                            default_values.Add(lu)
                        Next
                    End If
                Next
            End If

            classes.get_inactive = False
            classes.class_only = True

            classes.db_read()
            entities.get_inactive = False
            entities.db_read()
            res = gdb.get_ef_items(0, 0, 0, 1, "", MyBase.certificate)
            Me.item_names.Clear()

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    Me.item_names.Add(res(0, i))
                Next
            End If




        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_rtd_functions", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub

    <Serializable()> Public Class bc_om_rtd_function
        Inherits bc_cs_soap_base_class
        Public id As Integer
        Public name As String
        Public display_name As String
        Public helptext As String
        Public params As New List(Of bc_om_rtd_function_param)
        Public dimensions As New List(Of bc_om_ef_dimen)

        Public Sub db_read()
            Dim db As New bc_cs_db_services

            Dim res As Object
            Dim oparam As New bc_om_rtd_function_param
            res = db.executesql("exec dbo.bc_core_get_rtd_excel_params " + CStr(id), certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    oparam = New bc_om_rtd_function_param
                    oparam.certificate = MyBase.certificate
                    oparam.name = res(0, i)
                    oparam.default_value = res(1, i)
                    oparam.lookupsql = res(2, i)
                    oparam.dependent_on_item = res(3, i)

                    oparam.entity_search = res(4, i)
                    oparam.item_search = res(5, i)
                    oparam.class_search = res(6, i)
                    oparam.delimit_value = res(7, i)
                    oparam.list_index = res(8, i)
                    oparam.is_universe = res(9, i)

                    Me.params.Add(oparam)
                Next
            End If

            Dim odim As bc_om_ef_dimen
            Dim gdb As New bc_in_excel_submission_db
            res = gdb.get_ef_macro_dims(id, MyBase.certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    odim = New bc_om_ef_dimen
                    odim.name = res(0, i)
                    odim.ord = res(1, i)
                    dimensions.Add(odim)
                Next
            End If

        End Sub
    End Class
    <Serializable()> Public Class bc_om_param_lookup
        Public lookupsql As String
        Public value As String


    End Class
    <Serializable()> Public Class bc_om_rtd_function_param
        Inherits bc_cs_soap_base_class
        Public name As String
        Public default_value As String
        Public lookupsql As String
        Public dependent_on_item As String
        Public entity_search As Boolean
        Public item_search As Boolean
        Public class_search As Boolean
        Public delimit_value As Boolean = False
        Public list_index As Boolean = False
        Public is_universe As Boolean = False
        Public Overrides Sub process_object()
            Select Case tmode
                Case bc_cs_soap_base_class.tREAD
                    db_read()
            End Select
        End Sub
        Public Sub db_read()

        End Sub
    End Class
End Class

<Serializable> Public Class bc_om_contributor_waterfall
    Inherits bc_cs_soap_base_class

    Public universe_id As Long
    Public alternates As New List(Of bc_om_contributor_waterfall_alt)
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
            Dim res, ares As Object
            Dim alts As bc_om_contributor_waterfall_alt
            res = gdb.executesql("exec dbo.bc_core_aggs_get_waterfall_contributors " + CStr(universe_id), certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    alts = New bc_om_contributor_waterfall_alt
                    alts.contributor_id = res(0, i)
                    alts.alt_contributor_id = res(1, i)
                    ares = gdb.executesql("exec dbo.bc_core_aggs_get_waterfall_contributors_rules " + CStr(universe_id) + "," + CStr(alts.contributor_id) + "," + CStr(alts.alt_contributor_id), certificate)
                    If IsArray(ares) Then
                        For j = 0 To UBound(ares, 2)
                            alts.rules.Add(ares(0, j))
                        Next
                    End If
                    alternates.Add(alts)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_contributor_waterfall", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
    Public Sub db_write()
        Try
            Dim gdb As New bc_cs_db_services

            gdb.executesql("exec dbo.bc_core_aggs_waterfall_delete " + CStr(universe_id), certificate)

            For i = 0 To alternates.Count - 1
                gdb.executesql("exec dbo.bc_core_aggs_waterfall_write_contributors " + CStr(universe_id) + "," + CStr(alternates(i).contributor_id) + "," + CStr(alternates(i).alt_contributor_id) + "," + CStr(i + 1), certificate)
                For j = 0 To alternates(i).rules.Count - 1
                    gdb.executesql("exec dbo.bc_core_aggs_waterfall_write_contributor_rules " + CStr(universe_id) + "," + CStr(alternates(i).contributor_id) + "," + CStr(alternates(i).alt_contributor_id) + "," + CStr(alternates(i).rules(j)) + "," + CStr(i + 1), certificate)
                Next
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_contributor_waterfall", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class
<Serializable> Public Class bc_om_contributor_waterfall_alt
    Public contributor_id As Long
    Public alt_contributor_id As Long
    Public rules As New List(Of Integer)
End Class
<Serializable()> Public Class bc_om_agg_hier
    Inherits bc_cs_soap_base_class
    Public target_class_name As String
    Public dual_class_name As String
    Public target_class_id As Long
    Public dual_class_id As Long
    Public source_class_name As String
    Public source_class_id As String
    Public universe_id As Long
    Public universe_name As String
    Public all As Boolean
    Public all_class_name As String
    Public all_class As Long
    Public all_entity_name As String
    Public all_entity As Long
    Public store_source As Boolean = False
    Public consolidate_calenderized As Boolean = False
    Public target_prop As New List(Of bc_om_agg_hier_item)
    Public dual_prop As New List(Of bc_om_agg_hier_item)
    Public values As New List(Of bc_om_agg_hier_value)



    Public Overrides Sub process_object()
        Select Case MyBase.tmode

            Case bc_cs_soap_base_class.tREAD
                db_read()
            Case bc_cs_soap_base_class.tWRITE
                db_write()

        End Select
    End Sub

    Public Sub db_read()

        Try
            Dim db As New bc_om_universe_excl_db
            Dim res As Object
            res = db.read_hier_for_agg(universe_id, target_class_id, dual_class_id, source_class_id, certificate)
            Dim hitem As bc_om_agg_hier_item
            Dim hvalue As bc_om_agg_hier_value

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    hitem = New bc_om_agg_hier_item
                    hitem.class_id = res(0, i)
                    hitem.class_name = res(1, i)
                    If res(2, i) = 1 Then
                        Me.target_prop.Add(hitem)
                    Else
                        Me.dual_prop.Add(hitem)
                    End If
                    Me.all_class = res(3, i)
                    Me.all_entity = res(4, i)
                    Me.all_class_name = res(5, i)
                    Me.all_entity_name = res(6, i)
                Next
            End If
            res = db.read_values_for_hier(universe_id, target_class_id, dual_class_id, source_class_id, certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    hvalue = New bc_om_agg_hier_value
                    hvalue.target_class_id = res(0, i)
                    hvalue.dual_class_id = res(1, i)
                    hvalue.name = res(2, i)
                    hvalue.tname = res(3, i)
                    hvalue.tdname = res(4, i)
                    hvalue.all_class_Id = res(5, i)
                    hvalue.all_entity_Id = res(6, i)
                    hvalue.all_class_name = res(7, i)
                    hvalue.all_entity_name = res(8, i)
                    hvalue.ball = res(9, i)
                    Me.values.Add(hvalue)
                Next
            End If



        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_agg_hier", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
    Public Sub db_write()

        Try
            Dim db As New bc_om_universe_excl_db

            If Me.all_entity = 0 Then
                db.write_aggregation_hierarchy(universe_id, 0, 0, certificate)
            Else
                db.write_aggregation_hierarchy(universe_id, 1, Me.all_entity, certificate)
            End If
            For i = 0 To Me.values.Count - 1
                db.write_hier_value(universe_id, Me.values(i).target_class_id, Me.values(i).dual_class_id, certificate)
            Next



        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_agg_hier", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_agg_hier_item
    Public class_name As String
    Public class_id As Long
    Public list As Integer

End Class
<Serializable()> Public Class bc_om_agg_hier_value
    Public target_class_id As Long
    Public dual_class_id As Long
    Public name As String
    Public tname As String
    Public tdname As String
    Public all_class_Id As Long
    Public all_entity_Id As Long
    Public all_class_name As String
    Public all_entity_name As String
    Public ball As Boolean

End Class

<Serializable()> Public Class bc_om_agg_entity
    Public id As Long
    Public name As String
    Public type As Integer
    Public pentity As Long
    Public dentity As Long
    Public pentityname As String
    Public dentityname As String
End Class
REM FIL 5.2 August 2014
<Serializable()> Public Class bc_om_item_curr_types
    Inherits bc_cs_soap_base_class
    Public types As New List(Of bc_om_item_curr_type)

End Class
<Serializable()> Public Class bc_om_item_curr_type
    Public item_id As Long
    Public types_id As Integer
End Class
<Serializable()> Public Class bc_om_att_excl_types
    Public types As New List(Of bc_om_item_curr_type)


End Class
<Serializable()> Public Class bc_om_att_excl_type
    Public id As Integer
    Public name As String
End Class
<Serializable()> Public Class bc_om_universes_excl
    Inherits bc_cs_soap_base_class
    Public universes As New List(Of bc_om_universe_excl)
    Public Overrides Sub process_object()
        Select Case MyBase.tmode

            Case bc_cs_soap_base_class.tWRITE
                db_write()

        End Select
    End Sub
    Public Sub db_write()

    End Sub

End Class
<Serializable()> Public Class bc_om_universe_excls
    Inherits bc_cs_soap_base_class
    Public universes As New List(Of bc_om_universe_excl)
    Public Overrides Sub process_object()
        Select Case MyBase.tmode

            Case bc_cs_soap_base_class.tWRITE
                db_write()

        End Select
    End Sub
    Public Sub db_write()
        Try
            For i = 0 To universes.Count - 1
                universes(i).db_write()
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_universe_excls", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class

<Serializable()> Public Class bc_om_alt_contributor
    Public primary_contributor As Long
    Public alternate_contributor As Long
End Class

<Serializable()> Public Class bc_om_universe_excl
    Inherits bc_cs_soap_base_class

    Public universe_id As Long
    Public attribute_excls As New List(Of bc_om_attribute_excl)
    Public schema_excls As New List(Of bc_om_schema_excl)
    Public attribute_excls_types As New List(Of bc_om_att_excl_type)
    Public sec_attribute_excls_types As New List(Of bc_om_att_excl_type)
    Public sec_attribute_excls As New List(Of bc_om_attribute_excl)
    Public altcontributors As New List(Of bc_om_alt_contributor)
    Public contributors As New List(Of bc_om_contributor)
    Public aggregate_contributors As New List(Of bc_om_contributor)

    Public ignore_include_in_aggregation_flag As Boolean = False
    Public Overrides Sub process_object()
        Select Case MyBase.tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
            Case bc_cs_soap_base_class.tWRITE
                db_write()

        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim db As New bc_om_universe_excl_db
            Dim res As Object
            Dim oatt As bc_om_attribute_excl
            Dim osch As bc_om_schema_excl
            Dim ott As bc_om_att_excl_type
            Me.attribute_excls_types.Clear()
            Me.attribute_excls.Clear()
            Me.schema_excls.Clear()

            res = db.get_attribute_excl_types(MyBase.certificate)

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    ott = New bc_om_att_excl_type
                    ott.id = res(0, i)
                    ott.name = res(1, i)
                    Me.attribute_excls_types.Add(ott)
                Next
            End If

            res = db.get_attribute_excl(Me.universe_id, MyBase.certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    oatt = New bc_om_attribute_excl
                    oatt.id = res(0, i)
                    oatt.name = res(1, i)
                    Me.attribute_excls.Add(oatt)
                Next
            End If
            res = db.get_schema_excl(Me.universe_id, MyBase.certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    osch = New bc_om_schema_excl
                    osch.entity_id = res(0, i)
                    osch.name = res(1, i)
                    osch.class_name = res(2, i)
                    Me.schema_excls.Add(osch)
                Next
            End If
            Me.sec_attribute_excls_types.Clear()
            res = db.get_sec_attribute_excl_types(MyBase.certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    ott = New bc_om_att_excl_type
                    ott.id = res(0, i)
                    ott.name = res(1, i)
                    Me.sec_attribute_excls_types.Add(ott)
                Next
            End If
            Me.sec_attribute_excls.Clear()
            res = db.get_sec_attribute_excl(Me.universe_id, MyBase.certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    oatt = New bc_om_attribute_excl
                    oatt.id = res(0, i)
                    oatt.name = res(1, i)
                    Me.sec_attribute_excls.Add(oatt)
                Next
            End If
            res = db.get_contributors(MyBase.certificate)
            Me.contributors.Clear()
            Dim oc As bc_om_contributor
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    oc = New bc_om_contributor(res(0, i), res(1, i))
                    Me.contributors.Add(oc)
                Next
            End If
            Me.altcontributors.Clear()
            Dim ac As bc_om_alt_contributor
            res = db.get_alt_contributors(Me.universe_id, MyBase.certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    ac = New bc_om_alt_contributor
                    ac.primary_contributor = res(0, i)
                    ac.alternate_contributor = res(1, i)
                    Me.altcontributors.Add(ac)
                Next
            End If
            res = db.get_ignore_inc_flag(Me.universe_id, MyBase.certificate)
            If IsArray(res) Then
                If UBound(res, 2) = 0 Then
                    Me.ignore_include_in_aggregation_flag = res(0, 0)
                End If
            End If
            Me.aggregate_contributors.Clear()

            res = db.get_aggregated_contributors(Me.universe_id, MyBase.certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    oc = New bc_om_contributor(res(0, i), res(1, i))
                    Me.aggregate_contributors.Add(oc)
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_universe_excl", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
    Public Sub db_write()
        Try
            Dim db As New bc_om_universe_excl_db
            db.remove_exclusions(Me.universe_id, certificate)
            For i = 0 To Me.attribute_excls.Count - 1
                db.add_attribute_excl(Me.universe_id, Me.attribute_excls(i).id, certificate)
            Next
            For i = 0 To Me.schema_excls.Count - 1
                db.add_schema_excl(Me.universe_id, Me.schema_excls(i).entity_id, certificate)
            Next
            For i = 0 To Me.altcontributors.Count - 1
                db.add_alt_contributor(Me.universe_id, Me.altcontributors(i).primary_contributor, Me.altcontributors(i).alternate_contributor, certificate)
            Next
            For i = 0 To Me.sec_attribute_excls.Count - 1
                db.add_sec_attribute_excl(Me.universe_id, Me.sec_attribute_excls(i).id, certificate)
            Next
            db.remove_agg_contributors(Me.universe_id, certificate)
            For i = 0 To Me.aggregate_contributors.Count - 1
                db.add_agg_contributor(Me.universe_id, Me.aggregate_contributors(i).id, certificate)
            Next

            db.update_ignore_inc_flag(Me.universe_id, Me.ignore_include_in_aggregation_flag, certificate)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_universe_excl", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class
Public Class bc_om_universe_excl_db
    Private gbc_db As New bc_cs_db_services
    Friend Sub update_ignore_inc_flag(universe_id As Long, ignore_include_in_aggregation_flag As Boolean, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_update_ignore_inc_flag " + CStr(universe_id) + "," + CStr(ignore_include_in_aggregation_flag)
        gbc_db.executesql(sql, certificate)

    End Sub
    Friend Function get_ignore_inc_flag(universe_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_ignore_inc_flag " + CStr(universe_id)
        get_ignore_inc_flag = gbc_db.executesql(sql, certificate)

    End Function
    Friend Sub remove_agg_contributors(universe_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_del_aggregated_contributors " + CStr(universe_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Friend Sub add_agg_contributor(universe_id As Long, contributor_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_add_aggregated_contributor " + CStr(universe_id) + "," + CStr(contributor_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Friend Sub add_alt_contributor(universe_id As Long, primary_contributor As Long, alternate_contributor As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_write_alt_contributor " + CStr(universe_id) + "," + CStr(primary_contributor) + "," + CStr(alternate_contributor)
        gbc_db.executesql(sql, certificate)
    End Sub
    Friend Function get_aggregated_contributors(universe_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_aggregated_contributors " + CStr(universe_id)
        get_aggregated_contributors = gbc_db.executesql(sql, certificate)
    End Function
    Friend Function get_contributors(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_contributors "
        get_contributors = gbc_db.executesql(sql, certificate)
    End Function
    Friend Function get_alt_contributors(ByVal universe_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_alt_contributors " + CStr(universe_id)
        get_alt_contributors = gbc_db.executesql(sql, certificate)
    End Function
    Friend Sub write_aggregation_hierarchy(ByVal universe_id As Long, ByVal all As Boolean, ByVal all_entity_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_write_aggregate_hier " + CStr(universe_id) + ",'" + CStr(all) + "'," + CStr(all_entity_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Friend Sub write_aggregation_hierarchy_classes(ByVal universe_id As Long, ByVal mode As Integer, ByVal class_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_write_aggregate_hier_class " + CStr(universe_id) + ",'" + CStr(mode) + "'," + CStr(class_id)
        gbc_db.executesql(sql, certificate)
    End Sub


    Friend Function read_hier_for_agg(ByVal universe_id As Long, ByVal target_class_id As Long, ByVal dual_class_id As Long, ByVal source_class As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_aggregate_hier " + CStr(universe_id) + "," + CStr(target_class_id) + "," + CStr(dual_class_id) + "," + CStr(source_class)
        read_hier_for_agg = gbc_db.executesql(sql, certificate)
    End Function
    Friend Function read_values_for_hier(ByVal universe_id As Long, target_class_id As Long, dual_class_id As Long, source_class_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_hier_for_universe " + CStr(universe_id) + "," + CStr(target_class_id) + "," + CStr(dual_class_id) + "," + CStr(source_class_id)
        read_values_for_hier = gbc_db.executesql(sql, certificate)
    End Function
    Friend Sub write_hier_value(ByVal universe_id As Long, target As Long, dual As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_write_hier_value " + CStr(universe_id) + "," + CStr(target) + "," + CStr(dual)
        gbc_db.executesql(sql, certificate)
    End Sub
    Friend Function get_attribute_excl(ByVal universe_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_aggregate_excl " + CStr(universe_id)
        get_attribute_excl = gbc_db.executesql(sql, certificate)
    End Function
    Friend Function get_sec_attribute_excl(ByVal universe_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_sec_aggregate_excl " + CStr(universe_id)
        get_sec_attribute_excl = gbc_db.executesql(sql, certificate)
    End Function
    Friend Function get_schema_excl(ByVal universe_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_schema_excl " + CStr(universe_id)
        get_schema_excl = gbc_db.executesql(sql, certificate)
    End Function
    Friend Function get_attribute_excl_types(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_aggregate_exclusion_types"
        get_attribute_excl_types = gbc_db.executesql(sql, certificate)
    End Function
    Friend Function get_sec_attribute_excl_types(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_sec_aggregate_exclusion_types"
        get_sec_attribute_excl_types = gbc_db.executesql(sql, certificate)
    End Function
    Friend Sub remove_exclusions(ByVal universe_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_remove_attribute_exclusions " + CStr(universe_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Friend Sub add_attribute_excl(ByVal universe_id As Long, ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_add_attribute_exclusion " + CStr(universe_id) + "," + CStr(id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Friend Sub add_sec_attribute_excl(ByVal universe_id As Long, ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_add_sec_attribute_exclusion " + CStr(universe_id) + "," + CStr(id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Friend Sub add_schema_excl(ByVal universe_id As Long, ByVal entity_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_add_schema_exclusion " + CStr(universe_id) + "," + CStr(entity_id)
        gbc_db.executesql(sql, certificate)
    End Sub
End Class
<Serializable()> Public Class bc_om_attribute_excl
    Public id As Long
    Public name As String
End Class
<Serializable()> Public Class bc_om_schema_excl
    Public entity_id As Long
    Public name As String
    Public class_name As String
End Class
REM ==================

<Serializable()> Public Class bc_om_calc_types
    Public name As String
    Public type As Integer
    Public aggregate As Integer
End Class
<Serializable()> Public Class bc_om_in_submission_security
    Inherits bc_cs_soap_base_class
    Public schema_id As Long
    Public class_id As Long
    Public entity_id As Long
    Public approval_type As Integer
    REM 0 none
    REM 1 normal
    REM 2 proxy
    Public proxy_user_ids As New ArrayList
    Public proxy_user_names As New ArrayList
    Public Sub New()
        Me.approval_type = 0
    End Sub
    Public Overrides Sub process_object()
        Select Case MyBase.tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Dim vres As Object
        Dim gdb As New bc_om_in_submission_security_db
        vres = gdb.get_submission_security(Me.schema_id, Me.class_id, Me.entity_id, MyBase.certificate)
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
    Private Class bc_om_in_submission_security_db
        Private gbc_db As New bc_cs_db_services
        Public Function get_submission_security(ByVal schema_id As Long, ByVal class_id As Long, ByVal entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
            Try
                Dim user_id As Long
                If bc_cs_central_settings.server_flag = 0 Then
                    user_id = bc_cs_central_settings.logged_on_user_id
                Else
                    user_id = certificate.user_id
                End If
                Dim sql As String
                sql = "exec dbo.bc_core_insight_sub_sec " + CStr(schema_id) + "," + CStr(class_id) + "," + CStr(entity_id) + "," + CStr(user_id)
                get_submission_security = gbc_db.executesql(sql, certificate)

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_in_submission_security_db", " get_submission_security", bc_cs_error_codes.USER_DEFINED, ex.Message)
                get_submission_security = Nothing
            End Try

        End Function

        Public Sub New()

        End Sub
    End Class
End Class

<Serializable()> Public Class bc_om_pt_predefined_extract
    Inherits bc_cs_soap_base_class
    Public sp_name As String
    Public format_filename As String
    Public entity_name As String
    Public entity_list As New ArrayList
    Public parent_entity_list As New ArrayList
    Public class_name As String
    Public parent_entity_name As String
    Public parent_class_name As String
    Public stage As String
    Public schema As String
    Public contributor As String
    Public date_from As Date
    Public date_to As Date
    Public start_year As Integer
    Public end_year As Integer
    Public periods As New ArrayList
    Public items As New ArrayList
    Public currency As String
    Public working_days As Boolean
    Public results As New Object
    Public year_end_date As New DateTime
    Public param1 As String
    Public id As String

    Public Overrides Sub process_object()
        Select Case MyBase.tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
        End Select
    End Sub

    Public Sub db_read()
        Try
            Dim gdb As New bc_om_excel_data_elements_db
            results = gdb.run_predefined_extract(Me.sp_name, Me.write_data_to_xml(MyBase.certificate), MyBase.certificate)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_pt_predefined_extract", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Overrides Function write_data_to_xml(ByRef certificate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_om_insight_sheet", "write_data_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim cbc_data_services As New bc_cs_data_services
        Dim ocommentary As New bc_cs_activity_log("bc_om_insight_sheet", "write_data_to_xml", bc_cs_activity_codes.COMMENTARY, "Attempting to write XML string")

        write_data_to_xml = soap_serialize_object_to_xml(Me, Me.certificate)

        otrace = New bc_cs_activity_log("bc_om_insight_sheet", "write_data_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    End Function
    'Public Function read_data_from_xml(ByVal xml As String, ByRef certificate As bc_cs_security.certificate) As Object
    '    Dim otrace As New bc_cs_activity_log("bc_om_insight_sheet", "read_data_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    Dim cbc_data_services As New bc_cs_data_services
    '    Dim ocommentary As New bc_cs_activity_log("bc_om_insight_sheet", "read_data_to_xml", bc_cs_activity_codes.COMMENTARY, "Attempting to write XML string")

    '    read_data_from_xml = soap_deserialize_xml_to_object(xml, Me.certificate)

    '    otrace = New bc_cs_activity_log("bc_om_insight_sheet", "read_data_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    'End Function
    Private Function soap_serialize_object_to_xml(ByVal o As Object, ByRef certificate As bc_cs_security.certificate, Optional ByVal packet_code As String = "") As String

        Dim otrace As New bc_cs_activity_log("bc_cs_excel_data_elements", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Dim bc_cs_central_settings As New bc_cs_central_settings
        Dim by() As Byte

        Try
            Using ms As New MemoryStream

                Dim extratype(0) As Type
                extratype(0) = GetType(bc_om_entity)

                Dim sf As New XmlSerializer(o.GetType, extratype)

                sf.Serialize(ms, o)

                by = ms.ToArray()

                ms.Close()

                'xml serialization does not always encode apostrophe's.  This causes an issue when passing to SQL Server
                soap_serialize_object_to_xml = Replace(System.Text.Encoding.UTF8.GetString(by), "'", "&#39;")

            End Using

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_excel_data_elements", "soap_serialize_object_to_xml", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            soap_serialize_object_to_xml = ""
        Finally
            otrace = New bc_cs_activity_log("bc_cs_excel_data_elements", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    '    Public Function soap_deserialize_xml_to_object(ByVal strdata As String, ByRef certificate As bc_cs_security.certificate, Optional ByVal packet_code As String = "") As Object

    '        Dim otrace As New bc_cs_activity_log("bc_om_pt_predefined_extract", "soap_deserialize_xml_to_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

    '        Dim bc_cs_central_settings As New bc_cs_central_settings
    '        Dim by() As Byte

    '        Try
    '            MsgBox("a")
    '            MsgBox(strdata)


    '            by = System.Text.Encoding.UTF8.GetBytes(strdata)



    '            Using ms As New MemoryStream
    '                Dim extratype(0) As Type
    '                extratype(0) = GetType(bc_om_entity)
    '                MsgBox("1")
    '                Dim sf As New XmlSerializer(Me.GetType, extratype)
    '                MsgBox("2")

    '                MsgBox(by.Length)

    '                ms.Write(by, 0, by.Length)
    '                MsgBox("3")
    '                soap_deserialize_xml_to_object = sf.Deserialize(ms)
    '            End Using


    '        Catch ex As Exception
    '            Dim db_err As New bc_cs_error_log("bc_om_pt_predefined_extract", "soap_deserialize_xml_to_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '            soap_deserialize_xml_to_object = Nothing
    '        Finally
    '            otrace = New bc_cs_activity_log("bc_om_pt_predefined_extracts", "soap_deserialize_xml_to_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '        End Try

    '    End Function

    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class bc_om_excel_data_element_errors
    Public errors As New ArrayList
End Class
<Serializable()> Public Class bc_om_excel_data_element_error
    Public worksheet As String
    Public row As Integer
    Public col As Integer
    Public err_tx As String
    Public type As String = "Cell"

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_excel_data_elements
    Inherits bc_cs_soap_base_class
    Public elements As New ArrayList
    Public draft As Boolean
    Public publish As Boolean
    Public io_type As String
    Public class_name As String
    Public worksheet As String
    Public template As String
    Public date_to As Date
    Public calc As Integer = 0
    Public errors As New bc_om_excel_data_element_errors
    Public Overrides Sub process_object()
        Select Case MyBase.tmode
            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_write()
        Dim otrace As New bc_cs_activity_log("bc_om_excel_data_elements", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim gdb As New bc_om_excel_data_elements_db
            Dim res As Object
            res = gdb.write_elements(Me.write_data_to_xml(MyBase.certificate), draft, publish, io_type, class_name, template, date_to, calc, MyBase.certificate)
            errors.errors.Clear()
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    Dim oerr As New bc_om_excel_data_element_error
                    oerr.worksheet = worksheet
                    oerr.row = res(0, i)
                    oerr.col = res(1, i)
                    oerr.err_tx = res(2, i)
                    oerr.type = res(3, i)
                    errors.errors.Add(oerr)
                Next
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_excel_data_elements", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_excel_data_elements", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Overrides Function write_data_to_xml(ByRef certificate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_om_insight_sheet", "write_data_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim cbc_data_services As New bc_cs_data_services
        Dim ocommentary As New bc_cs_activity_log("bc_om_insight_sheet", "write_data_to_xml", bc_cs_activity_codes.COMMENTARY, "Attempting to write XML string")

        write_data_to_xml = soap_serialize_object_to_xml(Me, Me.certificate)

        otrace = New bc_cs_activity_log("bc_om_insight_sheet", "write_data_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    End Function
    Private Function soap_serialize_object_to_xml(ByVal o As Object, ByRef certificate As bc_cs_security.certificate, Optional ByVal packet_code As String = "") As String

        Dim otrace As New bc_cs_activity_log("bc_cs_excel_data_elements", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Dim bc_cs_central_settings As New bc_cs_central_settings
        Dim by() As Byte

        Try
            Using ms As New MemoryStream

                Dim extratype(0) As Type
                extratype(0) = GetType(bc_om_excel_data_element)

                Dim sf As New XmlSerializer(o.GetType, extratype)

                sf.Serialize(ms, o)

                by = ms.ToArray()

                ms.Close()

                'xml serialization does not always encode apostrophe's.  This causes an issue when passing to SQL Server
                soap_serialize_object_to_xml = Replace(System.Text.Encoding.UTF8.GetString(by), "'", "&#39;")

            End Using

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_excel_data_elements", "soap_serialize_object_to_xml", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            soap_serialize_object_to_xml = ""
        Finally
            otrace = New bc_cs_activity_log("bc_cs_excel_data_elements", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function


    Public Sub New()

    End Sub
End Class
Public Class bc_om_excel_data_elements_db
    Private gbc_db As New bc_cs_db_services
    Public Function run_predefined_extract(ByVal sp_name As String, ByVal extract As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo." + sp_name + " '" + extract + "'"
        REM FIL DEC 2013 JIRA 7964
        run_predefined_extract = gbc_db.executesql_extended_timeout(sql, certificate)
    End Function
    Public Function write_elements(ByVal elements As String, ByVal draft As Boolean, ByVal publish As Boolean, ByVal io_type As String, ByVal class_name As String, ByVal template As String, ByVal date_to As Date, ByVal calc As Integer, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim str As New bc_cs_string_services(class_name)
        class_name = str.delimit_apostrophies
        str = New bc_cs_string_services(template)
        template = str.delimit_apostrophies
        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo.bc_core_excel_io_write_data '" + elements + "','" + CStr(draft) + "','" + CStr(publish) + "','" + io_type + "','" + class_name + "','" + template + "','" + Format(date_to, "yyyyMMdd") + "'," + CStr(calc) + "," + CStr(certificate.user_id)
        write_elements = gbc_db.executesql(sql, certificate)
    End Function

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_excel_data_element
    Public entity_key As String
    Public entity_key_name As String
    Public entity_row As Integer
    Public entity_col As Integer
    Public item_key As String
    Public value As String
    Public submission_code As Integer
    Public contributor As String
    Public date_from As Date = "1-1-1900"
    Public copy_to_draft As Integer = 0
    Public year As Integer
    Public period As String
    Public estimate As Boolean
    Public period_end_date As Date
    Public period_end_item_key As String
    Public period_end_item_row As Integer
    Public row As Integer
    Public col As Integer
    Public period_row As Integer


    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class bc_om_portoflio_names
    Inherits bc_cs_soap_base_class
    Public names As New ArrayList
    Public user_id As Long

    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Dim gdb As New bc_in_excel_submission_db
        Dim i As Integer
        Dim vres As Object
        vres = gdb.read_portfolios_for_user(Me.user_id, MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                Me.names.Add(vres(0, i))
            Next
        End If

    End Sub
End Class
<Serializable()> Public Class bc_om_portfolio_universe_set
    Inherits bc_cs_soap_base_class
    Public entities As New ArrayList
    Public assoc_classes As New ArrayList
    Public schema_id As Long
    Public item_names As New ArrayList
    Public item_type As New ArrayList
    Public factor As New ArrayList
    Public monetary As New ArrayList
    Public symbol As New ArrayList
    Public iclass As New ArrayList
    Public class_dup As New ArrayList
    Public type_dup As New ArrayList
    Public aggregation As Boolean = False
    Public dual_entity As Boolean = False
    Public target_class As String
    Public target_dual_class As String


    Public Overrides Sub process_object()
        Select Case MyBase.tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim vres As Object
            Dim i As Integer
            Dim xml_entities As String
            REM form xml

            xml_entities = "<entities>"
            If Me.aggregation = False Then
                For i = 0 To entities.Count - 1
                    xml_entities = xml_entities + "<entity ID=""" + CStr(entities(i)) + """ class=""" + CStr(assoc_classes(i)) + """></entity>"
                Next
            Else
                For i = 0 To entities.Count - 1
                    xml_entities = xml_entities + "<entity ID=""" + CStr(entities(i)) + """ class=""Aggregation Universe""></entity>"
                Next

            End If

            xml_entities = xml_entities + "</entities>"
            xml_entities = xml_entities.Replace("&", "amp;")

            Dim st As New bc_cs_string_services(xml_entities)
            xml_entities = st.delimit_apostrophies

            Dim gdb As New bc_in_excel_submission_db
            vres = gdb.read_universe_set(xml_entities, Me.schema_id, Me.aggregation, Me.dual_entity, Me.target_class, Me.target_dual_class, MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    Me.item_names.Add(vres(0, i))
                    Me.item_type.Add(vres(1, i))
                    Me.factor.Add(vres(2, i))
                    Me.monetary.Add(vres(3, i))
                    Me.symbol.Add(vres(4, i))
                    Me.iclass.Add(vres(5, i))
                    Me.class_dup.Add(vres(6, i))
                    Me.type_dup.Add(vres(7, i))
                Next
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_portfolio_universe_set", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_portoflio_client_settings
    Inherits bc_cs_soap_base_class
    Public logged_on_user_id As Long
    REM 0 all 1 draft 2 publish
    Public stages As Integer = 0
    REM 0 draft 1 publish
    Public default_stage As Integer = 0
    REM 0 yes 1 no
    Public global_use As Integer = 0

    Public Overrides Sub process_object()
        Select Case MyBase.tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Dim gdb As New bc_in_excel_submission_db
        Dim vres As Object
        vres = gdb.get_pt_user_settings(logged_on_user_id, MyBase.certificate)
        If IsArray(vres) Then
            If UBound(vres, 2) > -1 Then
                Me.stages = vres(0, 0)
                Me.default_stage = vres(1, 0)
                Me.global_use = vres(2, 0)
            End If
        End If
    End Sub


End Class
<Serializable()> Public Class bc_om_ef_item_sets
    Inherits bc_cs_soap_base_class
    Public user_id As Long
    Public class_name As String
    Public item_sets As New ArrayList
    Public dual_class_name As String
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_in_excel_submission_db
            Dim vres As Object
            Dim oitem_set As bc_om_ef_item_set
            Dim i As Integer
            item_sets.Clear()
            vres = gdb.get_item_sets(user_id, class_name, dual_class_name, MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    oitem_set = New bc_om_ef_item_set
                    oitem_set.user_id = user_id
                    oitem_set.class_name = class_name
                    oitem_set.set_id = vres(0, i)
                    oitem_set.name = vres(1, i)
                    oitem_set.db_read()
                    item_sets.Add(oitem_set)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_ef_item_sets", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally

        End Try

    End Sub
End Class
<Serializable()> Public Class bc_om_ef_item_set
    Inherits bc_cs_soap_base_class
    Public set_id As Long
    Public user_id As Long
    Public name As String
    Public class_name As String
    Public delete_flag As Boolean = False
    Public items As New ArrayList
    Public types As New ArrayList
    Public factors As New ArrayList
    Public monatarys As New ArrayList
    Public symbols As New ArrayList
    Public assoc_classes As New ArrayList
    Public dual_class As String

    Public Overrides Sub process_object()
        Select Case tmode
            Case tWRITE
                db_write()

        End Select
    End Sub

    Public Sub New()

    End Sub
    Public Sub db_write()
        Try
            Dim gdb As New bc_in_excel_submission_db
            Dim i As Integer
            If Me.delete_flag = True Then
                gdb.delete_item_set(Me.set_id, MyBase.certificate)
            Else
                Me.set_id = gdb.add_item_set(user_id, name, class_name, dual_class, MyBase.certificate)
                For i = 0 To Me.items.Count - 1
                    gdb.add_item_to_set(set_id, items(i), types(i), factors(i), monatarys(i), symbols(i), assoc_classes(i), i, dual_class, MyBase.certificate)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_ef_item_set", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally

        End Try

    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_in_excel_submission_db
            Dim vres As Object
            Dim i As Integer

            vres = gdb.get_items_for_set(set_id, MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    Me.items.Add(vres(0, i))
                    Me.types.Add(vres(1, i))
                    Me.factors.Add(vres(2, i))
                    Me.monatarys.Add(vres(3, i))
                    Me.symbols.Add(vres(4, i))
                    Me.assoc_classes.Add(vres(5, i))
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_ef_item_set", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally

        End Try

    End Sub

End Class

<Serializable()> Public Class bc_om_ef_entity_sets
    Inherits bc_cs_soap_base_class
    Public user_id As Long
    Public class_name As String
    Public entity_sets As New ArrayList
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_in_excel_submission_db
            Dim vres As Object
            Dim oentity_set As bc_om_ef_entity_set
            Dim i As Integer
            entity_sets.Clear()
            vres = gdb.get_entity_sets(user_id, class_name, MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    oentity_set = New bc_om_ef_entity_set
                    oentity_set.user_id = user_id
                    oentity_set.class_name = class_name
                    oentity_set.set_id = vres(0, i)
                    oentity_set.name = vres(1, i)
                    oentity_set.db_read()
                    entity_sets.Add(oentity_set)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_ef_entity_sets", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally

        End Try

    End Sub
End Class



<Serializable()> Public Class bc_om_ef_entity_set
    Inherits bc_cs_soap_base_class
    Public set_id As Long
    Public user_id As Long
    Public name As String
    Public class_name As String
    Public delete_flag As Boolean = False
    Public entities As New ArrayList

    Public Overrides Sub process_object()
        Select Case tmode
            Case tWRITE
                db_write()

        End Select
    End Sub

    Public Sub New()

    End Sub
    Public Sub db_write()
        Try
            Dim gdb As New bc_in_excel_submission_db
            Dim i As Integer
            If Me.delete_flag = True Then
                gdb.delete_entity_set(Me.set_id, MyBase.certificate)
            Else

                Me.set_id = gdb.add_entity_set(user_id, name, class_name, MyBase.certificate)
                For i = 0 To Me.entities.Count - 1
                    gdb.add_entity_to_set(set_id, entities(i), i, MyBase.certificate)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_ef_entity_set", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally

        End Try

    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_in_excel_submission_db
            Dim vres As Object
            Dim i As Integer

            vres = gdb.get_entities_for_set(set_id, MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    Me.entities.Add(vres(0, i))
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_ef_entity_set", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally

        End Try

    End Sub

End Class
<Serializable()> Public Class bc_om_ef_portolios
    Inherits bc_cs_soap_base_class
    Public user_id As Long
    Public portfolios As New ArrayList
    Public predefined As Boolean = False

    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Select Case tmode
            Case tREAD
                db_read()



        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_in_excel_submission_db
            Dim vres, wres As Object
            Dim i, j As Integer
            Dim oportfolio As bc_om_ef_portolio
            portfolios.Clear()
            If predefined = False Then
                vres = gdb.get_user_defined_portfolios(Me.user_id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        oportfolio = New bc_om_ef_portolio
                        oportfolio.portfolio_id = vres(0, i)
                        oportfolio.user_id = vres(1, i)
                        oportfolio.public_flag = vres(2, i)
                        oportfolio.create_date = vres(3, i)
                        oportfolio.title = vres(4, i)
                        oportfolio.function_name = vres(5, i)
                        oportfolio.class_name = vres(6, i)
                        oportfolio.schema_name = vres(7, i)
                        oportfolio.universe_flag = vres(8, i)
                        oportfolio.entity_set_name = vres(9, i)
                        oportfolio.entity_prop_class_name = vres(10, i)
                        oportfolio.entity_prop_entity_name = vres(11, i)
                        oportfolio.stage = vres(12, i)
                        oportfolio.contributor = vres(13, i)
                        oportfolio.date_at = vres(14, i)
                        oportfolio.start_year = vres(15, i)
                        oportfolio.end_year = vres(16, i)
                        oportfolio.convert_type = vres(17, i)
                        oportfolio.currency = vres(18, i)
                        oportfolio.user_name = vres(19, i)
                        oportfolio.show_e_a = vres(20, i)
                        oportfolio.format_type = vres(21, i)
                        oportfolio.precision = vres(22, i)
                        oportfolio.date_from = vres(23, i)
                        oportfolio.table_title = vres(24, i)
                        oportfolio.sub_title = vres(25, i)
                        oportfolio.source = vres(26, i)
                        oportfolio.associated_class = vres(27, i)
                        oportfolio.sec_entity_prop_class_name = vres(28, i)
                        oportfolio.sec_entity_prop_entity_name = vres(29, i)
                        oportfolio.dual_entity = vres(30, i)
                        oportfolio.data_type = vres(31, i)
                        oportfolio.exch_type = vres(32, i)

                        wres = gdb.get_entities_for_portfolio(oportfolio.portfolio_id, MyBase.certificate)
                        For j = 0 To UBound(wres, 2)
                            oportfolio.entities.Add(wres(0, j))
                            If oportfolio.dual_entity <> "" Then
                                oportfolio.dual_entities.Add(wres(1, j))
                            End If
                        Next

                        wres = gdb.get_items_for_portfolio(oportfolio.portfolio_id, MyBase.certificate)
                        For j = 0 To UBound(wres, 2)
                            oportfolio.items.Add(wres(0, j))
                            oportfolio.item_types.Add(wres(1, j))
                            oportfolio.item_factor.Add(wres(2, j))
                            oportfolio.item_monatary.Add(wres(3, j))
                            oportfolio.item_symbol.Add(wres(4, j))
                            oportfolio.item_assoc.Add(wres(5, j))
                        Next
                        wres = gdb.get_periods_for_portfolio(oportfolio.portfolio_id, MyBase.certificate)
                        For j = 0 To UBound(wres, 2)
                            oportfolio.periods.Add(wres(0, j))
                        Next



                        Me.portfolios.Add(oportfolio)
                    Next
                End If
            Else
                REM predefined portfolios
                vres = gdb.get_predefined_portfolios(Me.user_id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        oportfolio = New bc_om_ef_portolio
                        oportfolio.portfolio_id = vres(0, i)
                        oportfolio.title = vres(1, i)
                        oportfolio.function_name = vres(2, i)
                        oportfolio.class_name = vres(3, i)
                        oportfolio.schema_name = vres(4, i)
                        oportfolio.universe_flag = vres(5, i)
                        oportfolio.entity_set_name = vres(6, i)
                        oportfolio.entity_prop_class_name = vres(7, i)
                        oportfolio.entity_prop_entity_name = vres(8, i)
                        oportfolio.stage = vres(9, i)
                        oportfolio.contributor = vres(10, i)
                        oportfolio.date_at = vres(11, i)
                        oportfolio.start_year = vres(12, i)
                        oportfolio.end_year = vres(13, i)
                        oportfolio.convert_type = vres(14, i)
                        oportfolio.currency = vres(15, i)
                        oportfolio.format_type = vres(16, i)
                        oportfolio.precision = vres(17, i)
                        oportfolio.date_from = vres(18, i)
                        oportfolio.associated_class = vres(19, i)
                        oportfolio.sec_entity_prop_class_name = vres(20, i)
                        oportfolio.sec_entity_prop_entity_name = vres(21, i)
                        oportfolio.format_filename = vres(22, i)
                        oportfolio.sp_name = vres(23, i)
                        oportfolio.num_entities_select = vres(24, i)
                        oportfolio.num_items_select = vres(25, i)
                        oportfolio.enable_stage = vres(26, i)
                        oportfolio.enable_contributor = vres(27, i)
                        oportfolio.enable_years_periods = vres(28, i)
                        oportfolio.enable_date_at = vres(29, i)
                        oportfolio.enable_date_range = vres(30, i)
                        oportfolio.item_set = vres(31, i)
                        oportfolio.enable_items = vres(32, i)
                        oportfolio.show_date_type = vres(33, i)
                        oportfolio.show_year_end_date = vres(34, i)
                        oportfolio.show_currency = vres(35, i)
                        oportfolio.folder_name = vres(36, i)
                        oportfolio.sub_folder_name = vres(37, i)


                        wres = gdb.get_entities_for_portfolio(oportfolio.portfolio_id, MyBase.certificate)
                        For j = 0 To UBound(wres, 2)
                            oportfolio.entities.Add(wres(0, j))
                        Next
                        wres = gdb.get_items_for_portfolio(oportfolio.portfolio_id, MyBase.certificate)
                        For j = 0 To UBound(wres, 2)
                            oportfolio.items.Add(wres(0, j))
                            oportfolio.item_types.Add(wres(1, j))
                            oportfolio.item_factor.Add(wres(2, j))
                            oportfolio.item_monatary.Add(wres(3, j))
                            oportfolio.item_symbol.Add(wres(4, j))
                            oportfolio.item_assoc.Add(wres(5, j))
                        Next
                        wres = gdb.get_periods_for_portfolio(oportfolio.portfolio_id, MyBase.certificate)
                        For j = 0 To UBound(wres, 2)
                            oportfolio.periods.Add(wres(0, j))
                        Next
                        wres = gdb.get_param1_list_for_portfolio(oportfolio.portfolio_id, MyBase.certificate)
                        For j = 0 To UBound(wres, 2)
                            oportfolio.param1_list.Add(wres(0, j))
                            oportfolio.param1_name = (wres(1, j))
                        Next
                        Me.portfolios.Add(oportfolio)
                    Next
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_ef_portfolios", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_ef_portolio
    Inherits bc_cs_soap_base_class
    Public delete_only As Boolean = False
    Public user_id As String
    Public user_name As String
    Public portfolio_id As Long
    Public public_flag As Boolean
    Public create_date As Date
    Public title As String
    Public function_name As String
    Public class_name As String
    Public schema_name As String
    Public universe_flag As Integer
    Public show_e_a As Boolean
    Public format_type As Integer
    Public precision As Integer
    REM 0 selected, 1 saved set 2 class entity, 3 all
    Public entity_set_name As String
    Public entity_prop_class_name As String
    Public entity_prop_entity_name As String
    Public sec_entity_prop_class_name As String
    Public sec_entity_prop_entity_name As String
    Public entities As New ArrayList
    Public dual_entities As New ArrayList
    Public item_types As New ArrayList
    Public item_factor As New ArrayList
    Public item_monatary As New ArrayList
    Public item_symbol As New ArrayList
    Public item_assoc As New ArrayList
    Public items As New ArrayList
    Public stage As String
    Public contributor As String
    Public date_from As Date
    Public date_at As Date
    Public start_year As String
    Public end_year As String
    Public periods As New ArrayList
    Public output_type As Integer
    REM 0 cursor 1 worksheet
    Public convert_type As Integer
    REM 0 none 1 show item 2 convert all
    Public currency As String
    Public table_title As String
    Public sub_title As String
    Public source As String
    Public associated_class As String
    REM predefined extracts July 2010
    Public predefined_flag As Boolean
    Public format_filename As String
    Public sp_name As String
    REM 0 as many as you like else amount
    Public num_entities_select
    Public num_items_select
    Public item_selection_list As New ArrayList
    Public enable_stage As Boolean
    Public enable_contributor As Boolean
    Public enable_years_periods As Boolean
    Public enable_date_at As Boolean
    Public enable_date_range As Boolean
    Public item_set As String
    Public enable_items As Boolean
    Public show_date_type As Boolean = False
    Public show_year_end_date As Boolean
    Public show_currency As Boolean = False
    Public param1_list As New ArrayList
    Public param1_name As String
    Public dual_entity As String
    Public data_type As Integer
    Public folder_name As String
    Public sub_folder_name As String
    Public exch_type As String
    REM ----------------------------------
    Public Sub New()


    End Sub
    Public Overrides Sub process_object()
        Select Case tmode
            Case tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_write()
        Dim otrace As New bc_cs_activity_log("bc_om_ef_portfolio", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim gdb As New bc_in_excel_submission_db
            Dim i As Integer
            If Me.public_flag = True Then
                Me.public_flag = 1
            Else
                Me.public_flag = 0
            End If
            If Me.public_flag = True Then
                Me.public_flag = 1
            Else
                Me.public_flag = 0
            End If
            If portfolio_id > 0 Then
                gdb.delete_portfolio(portfolio_id, MyBase.certificate)
            End If
            If delete_only = True Then
                gdb.delete_portfolio(portfolio_id, MyBase.certificate)
                Exit Sub
            End If

            portfolio_id = 0

            portfolio_id = gdb.add_porfolio(Me.user_id, Me.public_flag, Me.title, Me.function_name, Me.class_name, Me.schema_name, Me.universe_flag, Me.entity_set_name, Me.entity_prop_class_name, Me.entity_prop_entity_name, Me.stage, Me.contributor, Me.date_at, Me.start_year, Me.end_year, Me.convert_type, Me.currency, Me.show_e_a, Me.format_type, Me.precision, Me.date_from, Me.table_title, Me.sub_title, Me.source, Me.associated_class, Me.sec_entity_prop_class_name, Me.sec_entity_prop_entity_name, Me.data_type, Me.dual_entity, Me.exch_type, MyBase.certificate)

            REM FIL MAY 2013
            Dim entity_found As Boolean = False
            If portfolio_id > 0 Then
                If Me.data_type = 0 Then
                    For i = 0 To Me.entities.Count - 1
                        If i > 0 Then
                            For j = 0 To i - 1
                                If Me.entities(i) = Me.entities(j) Then
                                    entity_found = True
                                    Exit For
                                End If
                            Next
                        End If
                        If entity_found = False Then
                            gdb.add_portfolio_entities(portfolio_id, Me.entities(i), CStr(i), "", MyBase.certificate)
                        End If
                    Next
                Else

                    For i = 0 To Me.entities.Count - 1
                        If dual_entity = "" Then
                            gdb.add_portfolio_entities(portfolio_id, Me.entities(i), CStr(i), "", MyBase.certificate)
                        Else
                            gdb.add_portfolio_entities(portfolio_id, Me.entities(i), CStr(i), Me.dual_entities(i), MyBase.certificate)
                        End If
                    Next
                End If
                For i = 0 To Me.items.Count - 1
                    gdb.add_portfolio_items(portfolio_id, Me.items(i), i, Me.item_types(i), Me.item_factor(i), Me.item_monatary(i), Me.item_symbol(i), Me.item_assoc(i), MyBase.certificate)
                Next
                For i = 0 To Me.periods.Count - 1
                    gdb.add_portfolio_periods(portfolio_id, Me.periods(i), MyBase.certificate)
                Next

            End If
        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_om_ef_portfolio", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_ef_portfolio", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try



    End Sub

End Class
<Serializable()> Public Class bc_om_ef_functions
    Inherits bc_cs_soap_base_class

    Public macros As New ArrayList
    Public stage_ids As New ArrayList
    Public stage_names As New ArrayList
    Public class_ids As New ArrayList
    Public class_names As New ArrayList
    Public entities As New ArrayList
    Public contributor_ids As New ArrayList
    Public contributor_names As New ArrayList
    Public period_ids As New ArrayList
    Public period_names As New ArrayList
    Public item_types As New ArrayList
    Public item_names As New ArrayList
    Public item_factors As New ArrayList
    Public item_monatarys As New ArrayList
    Public item_symbols As New ArrayList
    Public template_ids As New ArrayList
    Public template_names As New ArrayList
    Public currency_codes As New ArrayList
    Public schema_ids As New ArrayList
    Public schema_names As New ArrayList
    Public class_links As New ArrayList
    Public chart_tools As New ArrayList
    Private chart_method As String
    Private chart_passrangeasparam As Boolean
    Private chart_active As Boolean
    Public universe As String = ""
    Public aggs As New List(Of bc_om_agg_entity)

    Public Property ChartMethod() As String
        Get
            Dim i As Integer
            For i = 0 To chart_tools.Count - 1
                If chart_tools(i).tool_active = True Then
                    chart_method = chart_tools(i).tool_method_call
                End If
            Next
            ChartMethod = chart_method
        End Get
        Set(ByVal value As String)
            chart_method = value
        End Set
    End Property

    Public Property ChartPassRangeAsParam() As Boolean
        Get
            Dim i As Integer
            For i = 0 To chart_tools.Count - 1
                If chart_tools(i).tool_active = True Then
                    ChartPassRangeAsParam = chart_tools(i).tool_passrange
                End If
            Next
            ChartPassRangeAsParam = chart_passrangeasparam
        End Get
        Set(ByVal value As Boolean)
            chart_passrangeasparam = value
        End Set
    End Property

    Public Property ChartActive() As Boolean
        Get
            Dim i As Integer
            ChartActive = False
            For i = 0 To chart_tools.Count - 1
                If chart_tools(i).tool_active = True Then
                    ChartActive = chart_tools(i).tool_active
                End If
            Next
            ChartActive = chart_active
        End Get
        Set(ByVal value As Boolean)
            chart_active = value
        End Set
    End Property


    Public Overrides Sub process_object()
        Select Case Me.tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim i As Integer
            Dim vres As Object
            Dim omacro As bc_om_ef_function

            Me.macros.Clear()

            Dim gdb As New bc_in_excel_submission_db
            vres = gdb.get_ef_macros(MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    omacro = New bc_om_ef_function
                    omacro.certificate = MyBase.certificate
                    omacro.name = vres(0, i)
                    omacro.helptext = vres(1, i)
                    omacro.category = vres(2, i)
                    omacro.id = vres(3, i)
                    omacro.display_name = vres(4, i)
                    omacro.show_in_wizard = vres(5, i)
                    omacro.db_read()
                    Me.macros.Add(omacro)
                Next
            End If
            vres = gdb.get_ef_Stages(MyBase.certificate)
            Me.stage_ids.Clear()
            Me.stage_names.Clear()
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    Me.stage_ids.Add(vres(0, i))
                    Me.stage_names.Add(vres(1, i))
                Next
            End If
            vres = gdb.get_ef_classes(MyBase.certificate)
            Me.class_ids.Clear()
            Me.class_names.Clear()
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    Me.class_ids.Add(vres(0, i))
                    Me.class_names.Add(vres(1, i))
                Next
            End If
            vres = gdb.get_ef_entities(MyBase.certificate)
            Me.entities.Clear()
            Dim oent As bc_om_entity
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    oent = New bc_om_entity
                    oent.id = (vres(0, i))
                    oent.name = vres(1, i)
                    oent.class_id = vres(2, i)
                    Me.entities.Add(oent)
                Next
            End If
            vres = gdb.get_ef_contributors(MyBase.certificate)
            Me.contributor_ids.Clear()
            Me.contributor_names.Clear()
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    Me.contributor_ids.Add(vres(0, i))
                    Me.contributor_names.Add(vres(1, i))
                Next
            End If
            Me.currency_codes.Clear()
            vres = gdb.get_ef_currency_codes(MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    Me.currency_codes.Add(vres(0, i))
                Next
            End If

            vres = gdb.get_ef_templates(MyBase.certificate)
            Me.template_ids.Clear()
            Me.template_names.Clear()
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    Me.template_ids.Add(vres(0, i))
                    Me.template_names.Add(vres(1, i))
                Next
            End If
            vres = gdb.get_ef_periods(MyBase.certificate)
            Me.period_ids.Clear()
            Me.period_names.Clear()
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    Me.period_ids.Add(vres(0, i))
                    Me.period_names.Add(vres(1, i))
                Next
            End If
            vres = gdb.get_ef_items(0, 0, 0, 1, universe, MyBase.certificate)
            Me.item_types.Clear()
            Me.item_names.Clear()
            Me.item_factors.Clear()
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    Me.item_names.Add(vres(0, i))
                    Me.item_types.Add(vres(1, i))
                    Me.item_factors.Add(vres(2, i))
                    Me.item_monatarys.Add(vres(3, i))
                    Me.item_symbols.Add(vres(4, i))
                Next
            End If
            Me.schema_ids.Clear()
            Me.schema_names.Clear()
            vres = gdb.get_ef_schemas(MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    Me.schema_ids.Add(vres(0, i))
                    Me.schema_names.Add(vres(1, i))
                Next
            End If
            Me.class_links.Clear()
            Dim cl As bc_om_class_link
            vres = gdb.get_class_links(MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    cl = New bc_om_class_link
                    cl.schema_id = vres(0, i)
                    cl.parent_class_id = vres(1, i)
                    cl.child_class_id = vres(2, i)
                    Me.class_links.Add(cl)
                Next
            End If

            Me.chart_tools.Clear()
            Dim chart_tool As bc_om_class_chart_tools
            vres = gdb.get_ef_chart_tool(MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    chart_tool = New bc_om_class_chart_tools
                    chart_tool.tool_name = vres(0, i)
                    chart_tool.tool_method_call = vres(1, i)
                    chart_tool.tool_passrange = vres(2, i)
                    chart_tool.tool_active = vres(3, i)
                    Me.chart_tools.Add(chart_tool)
                Next
            End If
            vres = gdb.get_universes(MyBase.certificate)
            Dim u As bc_om_agg_entity
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    u = New bc_om_agg_entity
                    u.id = vres(0, i)
                    u.name = vres(1, i)
                    u.type = vres(2, i)
                    u.pentity = vres(3, i)
                    u.dentity = vres(4, i)
                    u.pentityname = vres(5, i)
                    u.dentityname = vres(6, i)
                    Me.aggs.Add(u)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_ef_help", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Public Sub New()

    End Sub
End Class
REM holds all template for a collection of entities
<Serializable()> Public Class bc_om_ef_templates
    Inherits bc_cs_soap_base_class
    Public entity_ids As New ArrayList
    Public class_names As New ArrayList
    Public entity_template_ids As New ArrayList
    Public template_class_names As New ArrayList
    Public schema_id As Long
    Public template_ids As New ArrayList
    Public context_names As New ArrayList
    Public context_templates As New ArrayList
    Public section_names As New ArrayList
    Public section_templates As New ArrayList
    Public Overrides Sub process_object()
        Select Case Me.tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            Dim i, j As Integer
            Dim bcs As New bc_cs_db_services
            Dim gdb As New bc_in_excel_submission_db
            Dim vres As Object
            Dim res As Object

            Dim tid As Long
            Dim found As Boolean
            Me.template_ids.Clear()
            Me.template_class_names.Clear()
            Me.context_names.Clear()
            Me.context_templates.Clear()
            Me.section_names.Clear()
            Me.section_templates.Clear()
            Me.entity_template_ids.Clear()
            For i = 0 To Me.entity_ids.Count - 1
                res = bcs.executesp_for_logical_template(entity_ids(i), Me.schema_id, MyBase.certificate)
                If IsArray(res) Then
                    tid = res(0, 0)
                End If
                Me.entity_template_ids.Add(tid)
                REM if not already exists add to list
                found = False
                For j = 0 To template_ids.Count - 1
                    If template_ids(j) = tid Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    Me.template_ids.Add(tid)
                    Me.template_class_names.Add(class_names(i))
                End If
            Next
            REM now get contexts for each template
            For i = 0 To Me.template_ids.Count - 1
                vres = gdb.get_contexts_for_template(Me.template_ids(i), MyBase.certificate)
                If IsArray(vres) Then
                    For j = 0 To UBound(vres, 2)
                        Me.context_templates.Add(Me.template_ids(i))
                        Me.context_names.Add(vres(1, j))
                    Next
                End If
                vres = gdb.get_sections_for_template(Me.template_ids(i), MyBase.certificate)
                If IsArray(vres) Then
                    For j = 0 To UBound(vres, 2)
                        Me.section_templates.Add(Me.template_ids(i))
                        Me.section_names.Add(vres(0, j))
                    Next
                End If

            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_ef_templates", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)


        Finally

        End Try
    End Sub

End Class

<Serializable()> Public Class bc_om_unqiue_entity_template_ids
    Inherits bc_cs_soap_base_class
    Public entity_ids As New ArrayList
    Public schema_id As Long
    Public use_entity_ids As New ArrayList
    Public use_template_ids As New ArrayList
    Public Overrides Sub process_object()
        Select Case MyBase.tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Try
            REM get union template ids used by entity list
            Dim res As Object
            Dim found As Boolean
            Dim tid As Long
            Dim i, j As Integer

            Dim bcs As New bc_cs_db_services
            use_entity_ids.Clear()
            use_template_ids.Clear()
            tid = 0
            For i = 0 To entity_ids.Count - 1
                res = bcs.executesp_for_logical_template(entity_ids(i), Me.schema_id, MyBase.certificate)
                If IsArray(res) Then
                    tid = res(0, 0)
                    REM check template_id not already used
                    found = False
                    For j = 0 To use_template_ids.Count - 1
                        If use_template_ids(j) = tid Then
                            found = True
                            Exit For
                        End If
                    Next
                    If found = False Then
                        Me.use_template_ids.Add(tid)
                        Me.use_entity_ids.Add(entity_ids(i))
                    End If
                End If
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_all_ef_items", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_ef_items
    Inherits bc_cs_soap_base_class
    Public item_names As New ArrayList
    Public item_types As New ArrayList
    Public item_factors As New ArrayList
    Public item_monatarys As New ArrayList
    Public item_symbols As New ArrayList
    Public template_id As Long
    Public entity_id As Long
    Public schema_id As Long
    Public universe As String = ""
    Public Sub New(ByVal entity_id As Long)
        Me.entity_id = entity_id
    End Sub
    Public Overrides Sub process_object()
        Select Case Me.tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Dim i As Integer
        Dim gdb As New bc_in_excel_submission_db
        Dim vres As Object
        vres = gdb.get_ef_items(Me.entity_id, 0, 0, Me.schema_id, universe, MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                Me.item_names.Add(vres(0, i))
                Me.item_types.Add(vres(1, i))
                Me.item_factors.Add(vres(2, i))
                Me.item_monatarys.Add(vres(3, i))
                Me.item_symbols.Add(vres(4, i))
                Me.template_id = vres(5, i)
            Next
        End If
    End Sub
End Class
<Serializable()> Public Class bc_om_ef_function
    Inherits bc_cs_soap_base_class
    Public id As Long
    Public name As String
    Public display_name As String
    Public helptext As String
    Public category As String
    Public show_in_wizard As Integer
    Public params As New ArrayList
    Public dims As New ArrayList

    Public Sub db_read()
        Dim i As Integer
        Dim vres As Object
        Dim oparam As bc_om_ef_param
        Dim odim As bc_om_ef_dimen
        Me.params.Clear()
        Me.dims.Clear()
        Dim gdb As New bc_in_excel_submission_db
        vres = gdb.get_ef_macro_params(Me.id, MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                oparam = New bc_om_ef_param
                oparam.certificate = MyBase.certificate
                oparam.name = vres(0, i)
                oparam.opt = vres(1, i)
                oparam.has_default = vres(2, i)
                oparam.default_value = vres(3, i)
                oparam.display_name = vres(4, i)
                oparam.lookup_Sql = vres(5, i)
                oparam.db_read()
                Me.params.Add(oparam)

            Next
        End If
        vres = gdb.get_ef_macro_dims(Me.id, MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                odim = New bc_om_ef_dimen
                odim.name = vres(0, i)
                odim.ord = vres(1, i)
                Me.dims.Add(odim)
            Next
        End If
    End Sub

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_ef_param
    Inherits bc_cs_soap_base_class
    Public name As String
    Public opt As Boolean
    Public has_default As Boolean
    Public default_value As String
    Public display_name As String
    Public lookup_Sql As String
    Public lookup_vals As New ArrayList

    Public Sub New()

    End Sub
    Public Sub db_read()
        Dim i As Integer
        If lookup_Sql = "" Then
            Exit Sub
        End If
        Me.lookup_vals.Clear()
        Dim gdb As New bc_in_excel_submission_db
        Dim vres As Object
        vres = gdb.get_ef_param_lookup(lookup_Sql, MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                Me.lookup_vals.Add(vres(0, i))
            Next
        End If

    End Sub
End Class

<Serializable()> Public Class bc_om_ef_dimen
    Public name As String
    Public ord As Integer

    Public Sub New()

    End Sub

End Class
<Serializable()> Public Class bc_om_insight_submission_entity_links
    Inherits bc_cs_soap_base_class
    Public bc_om_insight_submission_entity_links As New ArrayList
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_submission_entity_links", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_submission_entity_links", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_submission_entity_links", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_submission_entity_links", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try

            Dim db_insight As New bc_in_excel_submission_db
            Dim vlinks As Object
            Dim i As Integer
            bc_om_insight_submission_entity_links.Clear()
            vlinks = db_insight.get_insight_entity_class_submission(MyBase.certificate)
            If IsArray(vlinks) Then
                For i = 0 To UBound(vlinks, 2)
                    Dim obc_om_insight_submission_entity_link As New bc_om_insight_submission_entity_link
                    obc_om_insight_submission_entity_link.parent_entity_class_id = vlinks(0, i)
                    obc_om_insight_submission_entity_link.child_entity_class_id = vlinks(1, i)
                    obc_om_insight_submission_entity_link.parent_class_name = vlinks(2, i)
                    obc_om_insight_submission_entity_link.schema_id = vlinks(3, i)
                    obc_om_insight_submission_entity_link.schema_name = vlinks(4, i)
                    bc_om_insight_submission_entity_links.Add(obc_om_insight_submission_entity_link)
                Next
            End If
            REM now read in contributors for user


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_submission_entity_links", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_submission_entity_links", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

    'Public Overloads Overrides Function call_web_service() As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_insight_submission_entity_links", "call_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    If IsNumeric(bc_cs_central_settings.timeout) Then
    '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout, MyBase.certificate)
    '     End If

    '    call_web_service = webservice.LoadInsightLinks
    '   otrace = New bc_cs_activity_log("bc_om_insight_submission_entity_links", "call_web_service", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    'End Function

    ' Public Overrides Function write_xml_via_soap_client_request() As Object

    'End Function


End Class
<Serializable()> Public Class bc_om_insight_submission_entity_link
    Inherits bc_cs_soap_base_class
    Public parent_entity_class_id As Long
    Public child_entity_class_id As Long
    Public parent_class_name As String
    Public schema_id As Long
    Public schema_name As String
    Public Sub New()

    End Sub
End Class
REM collection class
<Serializable()> Public Class bc_om_insight_contribution_for_entities
    Inherits bc_cs_soap_base_class
    Public mine As Boolean
    Public bc_om_insight_contribution_for_entity As New ArrayList
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_contribution_for_entities", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
            If Me.tmode = bc_cs_soap_base_class.tREAD_ALL_CHECKED_OUT Then
                Me.db_read_checked_out_to_me()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_contribution_for_entities", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_contribution_for_entities", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_contribution_for_entities", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim vworkbooks As Object
            Dim i As Integer
            Dim db_insight As New bc_in_excel_submission_db
            Dim insight_contribution As bc_om_insight_contribution_for_entity

            vworkbooks = db_insight.retrieve_workbooks(Me.mine, False, certificate)
            If IsArray(vworkbooks) Then
                For i = 0 To UBound(vworkbooks, 2)
                    insight_contribution = New bc_om_insight_contribution_for_entity
                    insight_contribution.workbook_name = vworkbooks(0, i)
                    insight_contribution.lead_entity_id = vworkbooks(1, i)
                    insight_contribution.lead_class_id = vworkbooks(2, i)
                    insight_contribution.contributor_id = vworkbooks(3, i)
                    insight_contribution.author_id = vworkbooks(4, i)
                    insight_contribution.last_submission_date = vworkbooks(5, i)
                    insight_contribution.checked_out_user_id = vworkbooks(6, i)
                    insight_contribution.schema_name = vworkbooks(7, i)
                    Me.bc_om_insight_contribution_for_entity.Add(insight_contribution)
                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_contribution_for_entities", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_contribution_for_entities", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    Public Sub db_read_checked_out_to_me()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_contribution_for_entities", "db_read_checked_out_to_me", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim vworkbooks As Object
            Dim i As Integer
            Dim db_insight As New bc_in_excel_submission_db
            Dim insight_contribution As bc_om_insight_contribution_for_entity
            vworkbooks = db_insight.retrieve_workbooks(False, True, certificate)
            If IsArray(vworkbooks) Then
                For i = 0 To UBound(vworkbooks, 2)
                    insight_contribution = New bc_om_insight_contribution_for_entity
                    insight_contribution.workbook_name = vworkbooks(0, i)
                    insight_contribution.lead_entity_id = vworkbooks(1, i)
                    insight_contribution.lead_class_id = vworkbooks(2, i)
                    insight_contribution.contributor_id = vworkbooks(3, i)
                    insight_contribution.author_id = vworkbooks(4, i)
                    insight_contribution.last_submission_date = vworkbooks(5, i)
                    insight_contribution.checked_out_user_id = vworkbooks(6, i)
                    insight_contribution.schema_name = vworkbooks(7, i)
                    Me.bc_om_insight_contribution_for_entity.Add(insight_contribution)
                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_contribution_for_entities", "db_read_checked_out_to_me", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_contribution_for_entities", "db_read_checked_out_to_me", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    'Public Overloads Overrides Function call_web_service(ByVal s As String) As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "call_web_service (s)", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    If IsNumeric(bc_cs_central_settings.timeout) Then
    '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout, MyBase.certificate)
    '    End If

    '    call_web_service = webservice.InsightWorkBookList(s)
    '    otrace = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "call_web_service (s)", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    'End Function

End Class

REM class that sets paramters for contribution then retrieves
REM information
<Serializable()> Public Class bc_om_insight_user_access
    Public role_id As Long
    Public role_name As String
    Public stage_name As String
    Public stage_id As Integer
    Public draft_default As Boolean

    Public Sub New()

    End Sub
End Class
REM FIL JULY 2012
<Serializable()> Public Class bc_om_insight_contribution_for_entity
    Inherits bc_cs_soap_base_class
    Public lead_class_id As Long
    Public lead_entity_id As Long
    Public lead_class_name As String
    Public lead_entity_name As String
    Public contributor_id As Long
    Public contributor_name As String

    Public workflow_stage As Long
    Public accounting_standard As Long

    Public insight_sheets As New bc_om_insight_sheets
    REM workbook metadata
    Public workbook_name As String
    Public author_name As String
    Public author_id As String

    Public surrogate_author_id As Long



    Public last_submission_date As Date
    Public schema_name As String
    Public date_from As String
    REM document storage
    Public doc_byte As Byte()
    REM checked out user
    Public checked_out_user_id As String
    REM
    Public load_from_files As Boolean = False
    Public controlled_submission As Integer = 0
    Public bc_om_result_types As New bc_om_result_types
    Public accounting_standards As New bc_om_accounting_standards
    Public ignore_checkin As Boolean
    Public read_values As Boolean = False
    Public validate_only = False
    Public no_validate = False
    Public Const tREAD_VALUES = 3
    Public no_template As Boolean = False

    Public Sub New(ByVal class_id, ByVal class_name, ByVal entity_id, ByVal entity_name, ByVal contributor_id, ByVal contributor_name)
        Me.lead_class_id = class_id
        Me.lead_class_name = class_name
        Me.lead_entity_id = entity_id
        Me.lead_entity_name = entity_name
        Me.contributor_id = contributor_id
        Me.contributor_name = contributor_name
        Me.controlled_submission = 0
        REM temporary set
        Me.accounting_standard = 1
        Me.ignore_checkin = False
    End Sub
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
            If Me.tmode = bc_cs_soap_base_class.tWRITE Then
                Me.no_send_back = True
                db_write()
            End If
            If Me.tmode = bc_om_insight_contribution_for_entity.tREAD_VALUES Then
                db_read_values(MyBase.certificate)
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_contribution_for_entity", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "Logged on as: " + MyBase.certificate.user_id, certificate)
        Try
            REM  if populate from files flag then read these otherwise from database
            MyBase.certificate = certificate
            If read_values = False Then
                If Me.load_from_files = True Then
                    Dim o As New bc_om_insight_contribution_for_entity
                    o.certificate = MyBase.certificate
                    o.contributor_id = Me.contributor_id
                    o.accounting_standard = Me.accounting_standard
                    o.schema_name = Me.schema_name
                    o.lead_entity_id = Me.lead_entity_id
                    o.lead_entity_name = Me.lead_entity_name
                    o.lead_class_id = Me.lead_class_id
                    o.author_id = Me.author_id
                    o.author_name = Me.author_name
                    o.contributor_name = Me.contributor_name
                    o.lead_class_name = Me.lead_class_name
                    o.bc_om_result_types = Me.bc_om_result_types
                    o = o.populate_from_files(MyBase.certificate)
                    REM assign across
                    Me.insight_sheets = o.insight_sheets
                    Me.doc_byte = o.doc_byte
                    Me.accounting_standard = o.accounting_standard
                    Me.schema_name = o.schema_name
                    Me.lead_entity_id = o.lead_entity_id
                    Me.lead_entity_name = o.lead_entity_name
                    Me.lead_class_id = o.lead_class_id
                    Me.author_id = o.author_id
                    Me.author_name = o.author_name
                    Me.contributor_name = o.contributor_name
                    Me.lead_class_name = o.lead_class_name
                    Me.bc_om_result_types = o.bc_om_result_types
                Else
                    REM evaluate logical template to use based upon entity_id
                    Dim db_insight As New bc_in_excel_submission_db
                    insight_sheets = New bc_om_insight_sheets(contributor_id)
                    insight_sheets.certificate = MyBase.certificate
                    insight_sheets.certificate = MyBase.certificate
                    insight_sheets.db_read(lead_class_id, lead_class_name, lead_entity_id, lead_entity_name)
                    REM read in result types this is needed for validation purposes
                    no_template = False

                    If insight_sheets.no_template = True Then
                        no_template = True
                        Exit Sub
                    End If
                    bc_om_result_types.db_read()
                    accounting_standards.db_read()
                End If
            Else
                REM read actually data values into object model
                db_read_values(MyBase.certificate)
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_contribution_for_entity", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Function populate_from_files(ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "populate_from_files", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim o As Object
            Dim luser As String
            If bc_cs_central_settings.server_flag = 0 Then
                luser = bc_cs_central_settings.logged_on_user_id
            Else
                luser = certificate.user_id
            End If
            Dim db_insight As New bc_in_excel_submission_db

            Dim fn = "insight_" + CStr(Me.lead_entity_id) + "_" + CStr(Me.contributor_id) + ".xls"
            Dim mfn = "insight_" + CStr(Me.lead_entity_id) + "_" + CStr(Me.contributor_id) + ".dat"
            Dim fs As New bc_cs_file_transfer_services
            If fs.check_document_exists(bc_cs_central_settings.central_repos_path + mfn, MyBase.certificate) Then
                o = read_data_from_file(bc_cs_central_settings.central_repos_path + mfn, certificate, False)
                Try
                    Dim ts As String
                    ts = o.lead_entity_name
                    If fs.check_document_exists(bc_cs_central_settings.central_repos_path + fn, MyBase.certificate) Then
                        fs.write_document_to_bytestream(bc_cs_central_settings.central_repos_path + fn, o.doc_byte, MyBase.certificate)
                        REM if mesata file doesnt exist recreate
                        REM checkout workbook
                        db_insight.check_out_workbook(Me.lead_entity_id, Me.contributor_id, luser, MyBase.certificate)
                    End If
                    REM file doesnt exist at all so recreate from database
                    populate_from_files = o
                Catch
                    REM failed so recreate from database
                    Me.db_read()
                    If fs.check_document_exists(bc_cs_central_settings.central_repos_path + fn, MyBase.certificate) Then
                        fs.write_document_to_bytestream(bc_cs_central_settings.central_repos_path + fn, Me.doc_byte, MyBase.certificate)
                        REM checkout workbook
                        db_insight.check_out_workbook(Me.lead_entity_id, Me.contributor_id, luser, MyBase.certificate)
                    End If
                    populate_from_files = Me
                End Try
            Else
                REM recreate metadata
                Me.db_read()
                If fs.check_document_exists(bc_cs_central_settings.central_repos_path + fn, MyBase.certificate) Then
                    fs.write_document_to_bytestream(bc_cs_central_settings.central_repos_path + fn, Me.doc_byte, MyBase.certificate)
                    REM checkout workbook
                    db_insight.check_out_workbook(Me.lead_entity_id, Me.contributor_id, luser, MyBase.certificate)
                End If
                populate_from_files = Me
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_contribution_for_entity", "populate_from_files", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            populate_from_files = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "populate_from_files", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    REM FIL JUNE 2014 improved auditiing
    Public Function db_write() As String
        Dim otrace As New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim i As Integer
            Dim ocommentary As bc_cs_activity_log
            Dim gdb As New bc_in_excel_submission_db
            Dim excelexstension As String
            REM FIL JUNE 2014 auditting fix
            Dim audit_id As Long
            Dim audit_date As Date
            REM if workbook is checked out flag this
            REM later on need to implememt this for soap mode and pass
            REM message back to clien
            If validate_only = False Then

                If Me.author_id = "Force Check In" Then
                    gdb.force_check_in(lead_entity_id, contributor_id, MyBase.certificate)
                    Return ""
                End If
                If bc_cs_central_settings.server_flag = 0 Then
                    If Me.ignore_checkin = False Then
                        db_write = gdb.workbook_cannot_be_submitted(lead_entity_id, contributor_id, MyBase.certificate)
                        If db_write <> "" Then
                            ocommentary = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_write", bc_cs_activity_codes.COMMENTARY, "Workbook cant be submitted as checked out elsewhere", MyBase.certificate)
                            Exit Function
                        End If
                    End If
                End If



                Dim res As Object
                Try
                    res = gdb.get_audit_id(1, certificate)
                    If IsArray(res) Then
                        audit_id = res(0, 0)
                        audit_date = res(1, 0)
                        db_write = ""

                    Else
                        Dim oerr As New bc_cs_error_log("bc_om_insight_contribution_for_entity", "db_write", bc_cs_error_codes.USER_DEFINED, "Failed to get audit id", certificate)
                        db_write = ""
                        Exit Function
                    End If
                Catch ex As Exception
                    Dim oerr As New bc_cs_error_log("bc_om_insight_contribution_for_entity", "db_write", bc_cs_error_codes.USER_DEFINED, "Failed to get audit id: " + ex.Message, certificate)
                    db_write = ""
                    Exit Function
                End Try
            End If



            Dim ofs As New bc_cs_string_services(workbook_name)
            REM removed sept 2007 this is done via events now
            REM gdb.set_workflow_stage(lead_entity_id, workflow_stage, ofs.delimit_apostrophies, contributor_id, certificate)
            REM get date from here so all submissions are consistant
            REM changed Jan 2011 to run in a db tranaction
            Dim db As New bc_cs_db_services
            db.open_conn(certificate)
            Dim success As Boolean = True
            db.success = True
            REM submit each sheet if one or more error rollback transaction and report error
            For i = 0 To Me.insight_sheets.bc_om_insight_sheets.Count - 1
                With Me.insight_sheets.bc_om_insight_sheets(i)
                    .db_write_via_xml(db, Me.contributor_id, Me.workflow_stage, Me.accounting_standard, audit_date, audit_id, Me.author_id, validate_only, no_validate, MyBase.certificate)
                    success = db.success
                End With
            Next
            If validate_only = True Then
                db.close_conn(False, certificate)
                db_write = ""
                Me.no_send_back = False
                Exit Function
            End If

            If success = True And validate_only = False Then
                ocommentary = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_write", bc_cs_activity_codes.COMMENTARY, "Commencing Database Calculations.", MyBase.certificate)
                Dim wtx As Object
                wtx = gdb.run_calculations(db, lead_entity_id, Me.contributor_id, Me.author_id, audit_date, audit_id, certificate)
                success = db.success
                If success = False Then
                    Dim calc_warn As New bc_om_insight_sheet.warning
                    calc_warn.type = "Calc"
                    Try
                        calc_warn.tx = wtx(0, 0)
                    Catch
                        calc_warn.tx = "calculations failed"
                    End Try
                    calc_warn.row = 0
                    calc_warn.column = 0
                    Me.insight_sheets.bc_om_insight_sheets(0).warnings.add(calc_warn)

                End If
            End If
            If success = True Then
                Me.no_send_back = False
                REM FIL FEB 2013 now audit the submssion
                gdb.audit_submission(db, audit_date, audit_id, Me.author_id, Me.lead_entity_id, 1, Me.contributor_id, Me.surrogate_author_id, MyBase.certificate)
                If workflow_stage <> 1 And db.success = True Then
                    gdb.audit_submission(db, audit_date, audit_id, Me.author_id, Me.lead_entity_id, Me.workflow_stage, Me.contributor_id, Me.surrogate_author_id, MyBase.certificate)
                End If
                If db.success = True Then
                    db.close_conn(True, certificate)
                Else
                    Me.no_send_back = False
                    ocommentary = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_write", bc_cs_activity_codes.COMMENTARY, "Part of insight submission had database errored so submission rolled back", MyBase.certificate)
                    db.close_conn(False, certificate)
                End If

                REM now mark the end of the audit
                gdb.set_audit_end_date(audit_id, certificate)
            Else
                Me.no_send_back = False
                ocommentary = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_write", bc_cs_activity_codes.COMMENTARY, "Part of insight submission had database errored so submission rolled back", MyBase.certificate)
                db.close_conn(False, certificate)
            End If
            REM write down excel workbook to file system
            If Me.controlled_submission = 1 And success = True Then
                gdb.register_workbook(lead_entity_id, lead_class_id, contributor_id, workbook_name, author_id, doc_byte, Me.checked_out_user_id, MyBase.certificate)
                Dim fs = New bc_cs_file_transfer_services
                REM if document is to be checked in write down document to central repository
                ocommentary = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_write", bc_cs_activity_codes.COMMENTARY, "Excel Wqorkbook Being Checked In", MyBase.certificate)


                REM SW cope with office versions
                If bc_cs_central_settings.userOfficeStatus = 2 Then
                    excelexstension = ".xlsx"
                Else
                    excelexstension = ".xls"
                End If

                fs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + "insight_" + CStr(Me.lead_entity_id) + "_" + CStr(Me.contributor_id) + excelexstension, Me.doc_byte, MyBase.certificate)
                If Me.checked_out_user_id = "0" Then
                    clear_cell_values()
                    Me.write_data_to_file(bc_cs_central_settings.central_repos_path + "insight_" + CStr(Me.lead_entity_id) + "_" + CStr(Me.contributor_id) + ".dat")
                End If
            End If

            db_write = ""
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_contribution_for_entity", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            db_write = ""
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Sub db_read_values(ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_read_values", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim i As Integer
            Dim gdb As New bc_in_excel_submission_db
            REM if workbook is checked out flag this
            REM get date from here so all submissions are consistant
            For i = 0 To Me.insight_sheets.bc_om_insight_sheets.Count - 1
                With Me.insight_sheets.bc_om_insight_sheets(i)
                    .db_read_values(Me.contributor_id, Me.workflow_stage, Me.accounting_standard, date_from)
                End With
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_contribution_for_entity", "db_read_values", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_read_values", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Sub clear_cell_values()

        Dim slog = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "clear_cell_values", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim temp_values_list As New ArrayList
            Dim i, j, k, l As Integer
            For i = 0 To Me.insight_sheets.bc_om_insight_sheets.Count - 1
                With Me.insight_sheets.bc_om_insight_sheets(i)
                    For j = 0 To .bc_om_insightsections.count - 1
                        With .bc_om_insightsections(j)
                            For k = 0 To .rows.count - 1
                                REM if no change validation remove all
                                If .rows(k).change_validation_flag = False Then
                                    .rows(k).values_list.clear()
                                Else
                                    REM set values to not current
                                    temp_values_list.Clear()
                                    For l = 0 To .rows(k).values_list.count - 1
                                        REM set current values to not current and assgn to temporay list
                                        If .rows(k).values_list(l).current = True Then
                                            .rows(k).values_list(l).current = False
                                            temp_values_list.Add(.rows(k).values_list(l))
                                        End If
                                    Next
                                    REM remove values then assign only current ones
                                    .rows(k).values_list.clear()
                                    For l = 0 To temp_values_list.Count - 1
                                        Dim ocommentary As New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_write", bc_cs_activity_codes.COMMENTARY, "Assigning temporary values back to values list")
                                        .rows(k).values_list.add(temp_values_list(l))
                                    Next
                                End If
                            Next
                        End With
                    Next
                    For j = 0 To .bc_om_insightsections_static.count - 1
                        With .bc_om_insightsections_static(j)
                            For k = 0 To .rows.count - 1
                                If .rows(k).change_validation_flag = False Then
                                    .rows(k).values_list.clear()
                                Else
                                    REM update state of last but one value
                                    For l = 0 To .rows(k).values_list.count - 1
                                        .rows(k).values_list(l).current = False
                                    Next
                                    REM remove older values
                                    For l = 0 To .rows(k).values_list.count - 2
                                        .rows(k).values_list.removeat(0)
                                    Next
                                End If
                            Next
                        End With
                    Next
                End With
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_contribution_for_entity", "clear_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            slog = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "clear_cell_values", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    'Public Overloads Overrides Function call_web_service(ByVal s As String) As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "call_web_service (s)", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    If IsNumeric(bc_cs_central_settings.timeout) Then
    '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
    '    End If

    '    call_web_service = webservice.LoadInsightForEntity(s)
    '    otrace = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "call_web_service (s)", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    'End Function
    'Public Function call_web_service_uploadinsight(ByVal s As String) As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "call_web_service_upload_insight", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    Dim asyncstate As Object
    '    Dim ar As IAsyncResult
    '    Try
    '        'ar = webservice.BeginUploadInsight(s, Nothing, Nothing)
    '        If IsNumeric(bc_cs_central_settings.timeout) Then
    '            webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '            Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
    '        End If

    '        webservice.UploadInsight(s)
    '    Catch ex As Exception
    '        MsgBox(ex.Message)
    '    End Try
    '    otrace = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "call_web_service_upload_insight", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    'End Function
    'Public Overrides Function write_xml_via_soap_client_request() As Object
    '    Dim otrace As New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "write_xml_via_soap_client_request", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
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
    '        MyBase.certificate.user_id = bc_cs_central_settings.logged_on_user_id
    '        os = write_xml_to_string()

    '        s = call_web_service_uploadinsight(os)
    '        If Left(s, 25) = "Webservice Authentication" Then
    '            ocommentary = New bc_cs_activity_log("bc_om_document", "write_xml_via_soap_client_request", bc_cs_activity_codes.COMMENTARY, "Authentication Failed for Web Service UploadDocument! User:" + bc_cs_accountinfoheader.os_logon_name)
    '            Dim omessage As New bc_cs_message("Blue Curve Create", "Authentication Failed for Web Service LoadUsers! User:" + bc_cs_accountinfoheader.os_logon_name, bc_cs_message.MESSAGE)
    '            write_xml_via_soap_client_request = Nothing
    '        End If
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_om_insight_contribution_for_entity", "write_xml_via_soap_client_request", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '    Finally
    '        otrace = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "write_xml_via_soap_client_request", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try

    'End Function

    'Public Overloads Overrides Function read_xml_from_soap_server_reponse(ByVal s As String, Optional ByRef certificate As cs.bc_cs_security.certificate = Nothing, Optional ByVal not_encrypted As Boolean = False) As Object

    'End Function
End Class
REM holds information for each sheet
<Serializable()> Public Class bc_om_insight_sheets
    Inherits bc_cs_soap_base_class
    Public bc_om_insight_sheets As New ArrayList
    Public no_template As Boolean = False
    <NonSerialized()> Private contributor_id As Long
    Public Sub New()

    End Sub
    Public Sub New(ByVal contributor_id As Long)
        Me.contributor_id = contributor_id
        bc_om_insight_sheet.sheet_count = 1
    End Sub
    Public Shadows Sub db_read(ByVal parent_class_id As Long, ByVal parent_class_name As String, ByVal parent_entity_id As Long, ByVal parent_entity_name As String)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_sheets", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim i As Integer
            Dim vsheets As Object
            Dim osheet As bc_om_insight_sheet
            Dim db_insight As New bc_in_excel_submission_db
            MyBase.certificate = certificate
            REM instantiate parent sheet as always exists
            Dim parent_sheet As New bc_om_insight_sheet(parent_class_id, parent_class_name, parent_entity_id, parent_entity_name, contributor_id)
            parent_sheet.certificate = MyBase.certificate
            parent_sheet.db_read()
            If parent_sheet.no_template = True Then
                no_template = True
                Exit Sub
            End If
            bc_om_insight_sheets.Add(parent_sheet)
            REM evaluate child sheets if any
            Dim ocommentary As New bc_cs_activity_log("bc_om_insight_sheets", "db_read", bc_cs_activity_codes.COMMENTARY, "Adding Object Sheet for Entity: " + CStr(parent_entity_id), MyBase.certificate)
            vsheets = db_insight.get_all_contributing_child_entities(parent_entity_id, parent_class_id, contributor_id, MyBase.certificate)

            REM see if there is a prmary entity
            Dim vprimary As Object
            Dim pentity As Long = 0
            vprimary = db_insight.get_primary_entity(parent_entity_id, parent_class_id, contributor_id, MyBase.certificate)
            If IsArray(vprimary) Then
                If UBound(vprimary, 2) > -1 Then
                    pentity = vprimary(0, 0)
                    ocommentary = New bc_cs_activity_log("bc_om_insight_sheets", "db_read", bc_cs_activity_codes.COMMENTARY, "Primary Entity found:" + CStr(pentity), MyBase.certificate)
                End If
            End If

            If IsArray(vsheets) Then
                REM assign primary first
                For i = 0 To UBound(vsheets, 2)
                    If pentity = vsheets(0, i) Then
                        ocommentary = New bc_cs_activity_log("bc_om_insight_sheets", "db_read", bc_cs_activity_codes.COMMENTARY, "Adding Object Sheet for Entity (primary): " + CStr(vsheets(0, i)), MyBase.certificate)
                        osheet = New bc_om_insight_sheet(CLng(vsheets(2, i)), vsheets(3, i), CLng(vsheets(0, i)), vsheets(1, i), contributor_id)
                        osheet.certificate = MyBase.certificate
                        osheet.db_read()
                        If osheet.no_template = True Then
                            no_template = True
                            Exit Sub
                        End If
                        If db_insight.is_entity_inactive(CLng(vsheets(0, i)), MyBase.certificate) = False Then
                            bc_om_insight_sheets.Add(osheet)
                        End If
                        Exit For
                    End If
                Next
                For i = 0 To UBound(vsheets, 2)
                    If pentity <> vsheets(0, i) Then
                        ocommentary = New bc_cs_activity_log("bc_om_insight_sheets", "db_read", bc_cs_activity_codes.COMMENTARY, "Adding Object Sheet for Entity: " + CStr(vsheets(0, i)), MyBase.certificate)
                        osheet = New bc_om_insight_sheet(CLng(vsheets(2, i)), vsheets(3, i), CLng(vsheets(0, i)), vsheets(1, i), contributor_id)
                        osheet.certificate = MyBase.certificate
                        osheet.db_read()
                        If osheet.no_template = True Then
                            no_template = True
                            Exit Sub
                        End If
                        If db_insight.is_entity_inactive(CLng(vsheets(0, i)), MyBase.certificate) = False Then
                            bc_om_insight_sheets.Add(osheet)
                        End If
                    End If
                Next
            End If
            bc_om_insight_sheet.sheet_count = 1
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_sheets", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_sheets", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class
REM holds information per sheet
<Serializable()> Public Class bc_om_insight_sheet
    Inherits bc_cs_soap_base_class
    Public sheet_name As String
    Public entity_id As Long
    Public entity_name As String
    REM used for multiple entities of same class
    Public link_order_code As String
    Public class_id As Long
    Public class_name As String
    Public logical_template_id As Long
    Public logical_template_name As String
    Public logical_templates_ids As New ArrayList
    Public logical_templates_names As New ArrayList
    Public validations_ids As New ArrayList
    Public validations_names As New ArrayList
    REM period sections
    Public bc_om_insightsections As New ArrayList
    REM static sections
    Public bc_om_insightsections_static As New ArrayList
    REM custom section period
    Public bc_om_period_section_custom As New bc_om_insight_section
    REM custom section static
    Public bc_om_static_section_custom As New bc_om_insight_section
    Public Shared sheet_count As Integer = 1
    Public Shared db_errors As Object
    Public no_template As Boolean = False
    Public warnings As New ArrayList
    Public is_warning As Boolean = False
    <Serializable()> Public Class warning
        Public type As String
        Public tx As String
        Public row As Integer
        Public column As Integer
    End Class
    <NonSerialized()> Private contributor_id As Long
    Public Sub New()

    End Sub
    REM constructor wheb logical template is known
    Public Sub New(ByVal logical_template_id As Long)
        Me.logical_template_id = logical_template_id
    End Sub
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_sheet", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_sheet", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_sheet", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    REM constructor when logical template isnt known
    Public Sub New(ByVal class_id As Long, ByVal class_name As String, ByVal entity_id As Long, ByVal entity_name As String, ByVal contributor_id As Long)
        Me.entity_id = entity_id
        Me.entity_name = entity_name
        Me.class_id = class_id
        Me.class_name = class_name
        Me.contributor_id = contributor_id
        Me.logical_template_id = 0

        sheet_name = "BCSHEET" + CStr(sheet_count)
        sheet_count = sheet_count + 1
    End Sub
    Public Sub db_write_via_xml(ByRef db As bc_cs_db_services, ByVal contributor_id As String, ByVal workflow_stage As String, ByVal accounting_standard As String, ByVal audit_date As Date, ByVal audit_id As Long, ByVal author_id As Long, ByVal validate_only As Boolean, no_validate As Boolean, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_sheet", "db_write_via_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim gdb As New bc_in_excel_submission_db
            Dim ret_warnings As Object
            MyBase.certificate = certificate
            warnings.Clear()
            Dim owarn As warning

            If bc_cs_central_settings.server_flag = 0 Then
                certificate.user_id = bc_cs_central_settings.logged_on_user_id
            End If

            ret_warnings = gdb.write_data_via_xml_trans(db, Me.write_data_to_xml(certificate), contributor_id, workflow_stage, accounting_standard, entity_id, audit_date, audit_id, author_id, validate_only, no_validate, certificate)
            If IsArray(ret_warnings) Then
                For i = 0 To UBound(ret_warnings, 2)
                    Dim psuccess As Boolean
                    psuccess = db.success
                    db.success = False
                    owarn = New warning
                    owarn.type = ret_warnings(0, i)
                    If owarn.type = "Warning" Then
                        is_warning = True
                        'If psuccess = True Then
                        '    db.success = True
                        'End If
                        Dim ocomm As New bc_cs_activity_log("bc_om_insight_sheet", "db_write_via_xml", bc_cs_activity_codes.COMMENTARY, "Insight Submisison warning on sheet: " + sheet_name + ": type:" + owarn.type + ", message: " + owarn.tx + " row: " + CStr(owarn.row) + " col: " + CStr(owarn.column), certificate)
                    Else
                        Dim ocomm As New bc_cs_activity_log("bc_om_insight_sheet", "db_write_via_xml", bc_cs_activity_codes.COMMENTARY, "Insight Submisison failed on sheet: " + sheet_name + ": type:" + owarn.type + ", message: " + owarn.tx + " row: " + CStr(owarn.row) + " col: " + CStr(owarn.column), certificate)
                    End If
                    owarn.tx = ret_warnings(1, i)
                    owarn.row = ret_warnings(2, i)
                    owarn.column = ret_warnings(3, i)
                    'Dim ocomm As New bc_cs_activity_log("bc_om_insight_sheet", "db_write_via_xml", bc_cs_activity_codes.COMMENTARY, "Insight Submisison failed on sheet: " + sheet_name + ": type:" + owarn.type + ", message: " + owarn.tx + " row: " + CStr(owarn.row) + " col: " + CStr(owarn.column), certificate)
                    warnings.Add(owarn)
                Next
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_sheet", "db_write_via_xml", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_sheet", "db_write_via_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Shadows Sub db_write(ByVal contributor_id, ByVal workflow_stage, ByVal accounting_standard, ByVal date_from)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_sheet", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim i As Integer
        Try
            For i = 0 To Me.bc_om_insightsections.Count - 1
                With Me.bc_om_insightsections(i)
                    .db_write(contributor_id, workflow_stage, accounting_standard, Me.entity_id, date_from)
                End With
            Next
            For i = 0 To Me.bc_om_insightsections_static.Count - 1
                With Me.bc_om_insightsections_static(i)
                    .db_write(contributor_id, workflow_stage, accounting_standard, Me.entity_id, date_from)
                End With
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_sheet", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_sheet", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    Public Shadows Sub db_read_values(ByVal contributor_id, ByVal workflow_stage, ByVal accounting_standard, ByVal date_from)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_sheet", "db_read_values", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim i As Integer
        Try
            For i = 0 To Me.bc_om_insightsections.Count - 1
                With Me.bc_om_insightsections(i)
                    .db_read_values(contributor_id, workflow_stage, accounting_standard, Me.entity_id, date_from)
                End With
            Next
            For i = 0 To Me.bc_om_insightsections_static.Count - 1
                With Me.bc_om_insightsections_static(i)
                    .db_read_values(contributor_id, workflow_stage, accounting_standard, Me.entity_id, date_from)
                End With
            Next


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_sheet", "db_read_values", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_sheet", "db_read_values", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    'Public Shadows Function write_xml_to_soap_server_reponse(ByVal s As String) As String
    '    Dim cs As New bc_cs_central_settings(True)
    '    Dim otrace As New bc_cs_activity_log("bc_om_insight_sheet", "write_xml_to_soap_server_reponse", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)

    '    Try
    '        REM initialse database
    '        Dim db As New bc_cs_db_services(False)
    '        Dim odoc As New bc_om_insight_sheet

    '        odoc.certificate = MyBase.certificate

    '        Dim oxml As New bc_cs_xml_services
    '        REM decrypt
    '        Dim osec As New bc_cs_security
    '        s = osec.decrypt_xml(s, certificate)
    '        odoc = oxml.soap_deserialize_string_to_object(s, Me.certificate)
    '        REM product response and return
    '        'Me.id = CLng(doc_id)
    '        odoc.db_read()
    '        REM write object to string
    '        write_xml_to_soap_server_reponse = osec.encrypt_xml(odoc.write_xml_to_string, certificate), certificate)
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_om__insight_sheet", "write_xml_to_soap_server_reponse", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '    Finally
    '        otrace = New bc_cs_activity_log("bc_om__insight_sheet", "write_xml_to_soap_server_reponse", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try

    'End Function

    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_sheet", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim db_insight As New bc_in_excel_submission_db
            Dim vsections As Object
            Dim i As Integer
            Dim vlink_code As Object
            Dim vltemplates As Object
            Dim vlvalidations As Object
            REM firstly get logical template_id
            Dim vlogical_template As Object
            Dim ocommentary As bc_cs_activity_log
            MyBase.certificate = certificate
            REM return list of all logical templates only
            If Me.logical_template_id = -99 Then
                Me.logical_templates_ids.Clear()
                Me.logical_templates_names.Clear()
                vltemplates = db_insight.get_all_logical_templates(MyBase.certificate)
                If IsArray(vltemplates) Then
                    For i = 0 To UBound(vltemplates, 2)
                        Me.logical_templates_ids.Add(vltemplates(0, i))
                        Me.logical_templates_names.Add(vltemplates(1, i))
                    Next
                End If
                Exit Sub
            End If
            Me.validations_ids.Clear()
            Me.validations_names.Clear()
            vlvalidations = db_insight.get_validation_types(MyBase.certificate)
            If IsArray(vlvalidations) Then
                For i = 0 To UBound(vlvalidations, 2)
                    Me.validations_ids.Add(vlvalidations(0, i))
                    Me.validations_names.Add(vlvalidations(1, i))
                Next
            End If
            REM get link_code
            If Me.logical_template_id = 0 Then
                link_order_code = 0
                vlink_code = db_insight.get_link_code(entity_id, MyBase.certificate)
                If IsArray(vlink_code) Then
                    If UBound(vlink_code, 2) > -1 Then
                        If IsNumeric(CStr(vlink_code(0, 0))) Then
                            link_order_code = CStr(vlink_code(0, 0))
                            ocommentary = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_read", bc_cs_activity_codes.COMMENTARY, "Setting Link Order code to : " + CStr(link_order_code) + " for entity: " + CStr(entity_id), MyBase.certificate)
                        Else
                            ocommentary = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_read", bc_cs_activity_codes.COMMENTARY, "Non Numeric Link Order: " + CStr(link_order_code) + " for entity: " + CStr(entity_id), MyBase.certificate)
                        End If
                    End If
                End If
            End If

            REM default logical template to 0
            If Me.logical_template_id = 0 Then
                logical_template_id = -1
                vlogical_template = db_insight.get_logical_template(Me.entity_id, Me.contributor_id, MyBase.certificate)
                If IsArray(vlogical_template) Then
                    logical_template_id = CLng(vlogical_template(0, 0))
                    ocommentary = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_read", bc_cs_activity_codes.COMMENTARY, "Logical Template Being Used: " + CStr(logical_template_id) + " for sheet: " + sheet_name, MyBase.certificate)
                End If

                If logical_template_id = 0 Then
                    no_template = True
                    ocommentary = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "db_read", bc_cs_activity_codes.COMMENTARY, "Logical Template Not Found for Sheet: " + sheet_name, MyBase.certificate)
                    Exit Sub
                End If
            Else
                Dim vname As Object
                vname = db_insight.get_template_name(Me.logical_template_id, MyBase.certificate)
                If IsArray(vname) Then
                    Me.logical_template_name = vname(0, 0)
                End If

            End If
            REM period sections for logical template
            vsections = db_insight.get_section_for_template(logical_template_id, 0, MyBase.certificate)
            If IsArray(vsections) Then
                For i = 0 To UBound(vsections, 2)
                    ocommentary = New bc_cs_activity_log("bc_om_insight_sheet", "db_read", bc_cs_activity_codes.COMMENTARY, "Adding Section: " + CStr(vsections(1, i)) + " for Class:" + CStr(class_id) + ", Contributor: " + CStr(contributor_id) + " for template: " + CStr(logical_template_id), MyBase.certificate)
                    Dim osections As New bc_om_insight_section(CLng(vsections(0, i)), CStr(vsections(1, i)))
                    osections.certificate = MyBase.certificate

                    osections.db_read(logical_template_id, "0", entity_id, link_order_code)
                    bc_om_insightsections.Add(osections)
                Next
            End If
            REM get custom period sections
            bc_om_period_section_custom.db_read_custom(logical_template_id, 0, entity_id, link_order_code, certificate)
            If bc_om_period_section_custom.rows.Count > 0 Then
                ocommentary = New bc_cs_activity_log("bc_om_insight_sheet", "db_read", bc_cs_activity_codes.COMMENTARY, "Adding Custom Period Section for entity " + CStr(entity_id), MyBase.certificate)
                bc_om_period_section_custom.section_id = 0
                bc_om_period_section_custom.name = "Custom"
                bc_om_insightsections.Add(bc_om_period_section_custom)
            End If

            REM Static
            vsections = db_insight.get_section_for_template(logical_template_id, 1, MyBase.certificate)
            If IsArray(vsections) Then
                For i = 0 To UBound(vsections, 2)
                    ocommentary = New bc_cs_activity_log("bc_om_insight_sheet", "db_read", bc_cs_activity_codes.COMMENTARY, "Adding Static Section: " + CStr(vsections(1, i)) + " for Class:" + CStr(class_id) + ", Contributor: " + CStr(contributor_id) + " for template: " + CStr(logical_template_id), MyBase.certificate)
                    Dim osections As New bc_om_insight_section(CLng(vsections(0, i)), CStr(vsections(1, i)))
                    osections.certificate = MyBase.certificate
                    osections.db_read(logical_template_id, "1", entity_id, link_order_code)
                    bc_om_insightsections_static.Add(osections)
                Next
            End If
            REM get custom static sections
            bc_om_static_section_custom.db_read_custom(logical_template_id, 1, entity_id, link_order_code, certificate)
            If bc_om_static_section_custom.rows.Count > 0 Then
                ocommentary = New bc_cs_activity_log("bc_om_insight_sheet", "db_read", bc_cs_activity_codes.COMMENTARY, "Adding Custom Static Section for entity " + CStr(entity_id), MyBase.certificate)
                bc_om_static_section_custom.section_id = 0
                bc_om_static_section_custom.name = "Custom"
                bc_om_insightsections_static.Add(bc_om_static_section_custom)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_sheet", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_sheet", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    REM writes down the custom sections to the database
    Public Sub db_write_custom_sections()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_sheet", "db_write_custom_sections", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim gbc As New bc_in_excel_submission_db
            gbc.reset_custom_items(Me.entity_id, Me.logical_template_id, MyBase.certificate)
            REM period
            bc_om_period_section_custom.db_write_custom_section(Me.logical_template_id, Me.entity_id)
            REM Statics
            bc_om_static_section_custom.db_write_custom_section(Me.logical_template_id, Me.entity_id)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_sheet", "db_write_custom_sections", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_sheet", "db_write_custom_sections", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    'Public Overloads Overrides Function call_web_service(ByVal s As String) As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_insight_sheet", "aacall_web_service (s)", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    If IsNumeric(bc_cs_central_settings.timeout) Then
    '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
    '    End If
    '    call_web_service = webservice.DownLoadInsightSheet(s)

    '    otrace = New bc_cs_activity_log("bc_om_insight_sheet", "call_web_service (s)", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    'End Function
    'Public Overrides Function write_xml_via_soap_client_request() As Object
    '    Dim otrace As New bc_cs_activity_log("bc_om_insight_sheet", "write_xml_via_soap_client_request", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    Try
    '        Dim ocommentary As bc_cs_activity_log

    '        Dim s, os As String
    '        Dim bc_cs_accountinfoheader As New localhost.bc_cs_accountinfoheader
    '        webservice = New localhost.Service1
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
    '        Dim bcs As New bc_cs_security
    '        os = bcs.encrypt_xml(os, Nothing)
    '        s = call_web_service(os)
    '        If Left(s, 25) = "Webservice Authentication" Then
    '            ocommentary = New bc_cs_activity_log("bc_om_insight_sheet", "write_xml_via_soap_client_request", bc_cs_activity_codes.COMMENTARY, "Authentication Failed for Web Service UploadDocument! User:" + bc_cs_accountinfoheader.os_logon_name)
    '            Dim omessage As New bc_cs_message("Blue Curve Create", "Authentication Failed for Web Service LoadUsers! User:" + bc_cs_accountinfoheader.os_logon_name, bc_cs_message.MESSAGE)
    '            write_xml_via_soap_client_request = Nothing
    '        End If
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_om_insight_sheet", "write_xml_via_soap_client_request", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '    Finally
    '        otrace = New bc_cs_activity_log("bc_om_insight_sheet", "write_xml_via_soap_client_request", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try

    'End Function
    'Public Overloads Overrides Function read_xml_from_soap_server_reponse(ByVal s As String, Optional ByRef certificate As bc_cs_security.certificate = Nothing, Optional ByVal not_encrypted As Boolean = False) As Object
    '    Dim otrace As New bc_cs_activity_log("bc_om_insight_sheet", "read_xml_from_soap_server_reponse", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    Dim o As New Object
    '    Try
    '        REM initiliase central utilities
    '        Dim cs As New bc_cs_central_settings(True)
    '        REM initialse database
    '        Dim db As New bc_cs_db_services(False)
    '        Dim bc_cs_security As New bc_cs_security
    '        o = Me.read_xml_from_string(bc_cs_security.decrypt_xml(s, MyBase.certificate))
    '        'o.db_write_custom_sections()
    '        o.db_read()
    '        read_xml_from_soap_server_reponse = o
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_om_insight_sheet", "read_xml_from_soap_server_reponse", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '    Finally
    '        otrace = New bc_cs_activity_log("bc_om_insight_sheet", "read_xml_from_soap_server_reponse", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try
    'End Function

    'Public Overloads Overrides Function call_web_service() As String

    'End Function

    Public Overrides Function write_data_to_xml(ByRef certifiate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_om_insight_sheet", "write_data_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim cbc_data_services As New bc_cs_data_services
        Dim ocommentary As New bc_cs_activity_log("bc_om_insight_sheet", "write_data_to_xml", bc_cs_activity_codes.COMMENTARY, "Attempting to write XML string")

        write_data_to_xml = soap_serialize_object_to_xml(Me, Me.certificate)

        otrace = New bc_cs_activity_log("bc_om_insight_sheet", "write_data_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    End Function

    Private Function soap_serialize_object_to_xml(ByVal o As Object, ByRef certificate As bc_cs_security.certificate, Optional ByVal packet_code As String = "") As String

        Dim otrace As New bc_cs_activity_log("bc_cs_data_services", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Dim bc_cs_central_settings As New bc_cs_central_settings
        Dim by() As Byte

        Try
            Using ms As New MemoryStream

                Dim extratype(2) As Type

                extratype(0) = GetType(bc_om_insight_row)
                extratype(1) = GetType(bc_om_cell_validation)
                extratype(2) = GetType(bc_om_insight_rows_cell_value)

                Dim sf As New XmlSerializer(o.GetType, extratype)

                sf.Serialize(ms, o)

                by = ms.ToArray()

                ms.Close()

                'xml serialization does not always encode apostrophe's.  This causes an issue when passing to SQL Server
                soap_serialize_object_to_xml = Replace(System.Text.Encoding.UTF8.GetString(by), "'", "&#39;")

            End Using

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_cs_data_services", "soap_serialize_object_to_xml", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            soap_serialize_object_to_xml = ""
        Finally
            otrace = New bc_cs_activity_log("bc_cs_data_services", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function

End Class
<Serializable()> Public Class bc_om_insight_section
    Inherits bc_cs_soap_base_class
    Public section_id As Long
    Public name As String
    Public rows As New ArrayList
    Public Sub New()

    End Sub
    Public Sub New(ByVal id, ByVal name)
        Me.section_id = id
        Me.name = name
    End Sub
    Public Shadows Sub db_read(ByVal template_id As Long, ByVal static_flag As String, ByVal entity_id As Long, ByVal link_order_code As Long)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_section", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim db_insight As New bc_in_excel_submission_db
            Dim vrows As Object
            Dim i As Integer
            Dim orow As bc_om_insight_row
            MyBase.certificate = certificate
            vrows = db_insight.get_rows_for_section(section_id, template_id, MyBase.certificate)
            If IsArray(vrows) Then
                i = 0
                While i < UBound(vrows, 2)
                    orow = New bc_om_insight_row(CStr(vrows(1, i)), CStr(vrows(1, i + 1)), CStr(vrows(1, i + 2)), CStr(vrows(1, i + 3)), CStr(vrows(1, i + 4)), CStr(vrows(1, i + 5)), CStr(vrows(1, i + 6)), static_flag, CStr(vrows(1, i + 7)), CStr(vrows(1, i + 8)), CStr(vrows(1, i + 9)))
                    orow.certificate = MyBase.certificate
                    orow.row_id = vrows(0, i)
                    Dim ocommentary As New bc_cs_activity_log("bc_om_insight_section", "db_read", bc_cs_activity_codes.COMMENTARY, "Adding row: " + CStr(vrows(0, i)), certificate)
                    i = i + 10
                    orow.db_read(template_id, entity_id, link_order_code, MyBase.certificate)
                    rows.Add(orow)
                End While
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_section", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_section", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Shadows Sub db_read_custom(ByVal template_id As Long, ByVal static_flag As String, ByVal entity_id As Long, ByVal link_order_code As Long, ByVal certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_section", "db_read_custom", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim db_insight As New bc_in_excel_submission_db
            Dim vrows As Object
            Dim i As Integer
            Dim orow As bc_om_insight_row
            MyBase.certificate = certificate
            vrows = db_insight.get_custom_rows(template_id, entity_id, static_flag, MyBase.certificate)
            If IsArray(vrows) Then
                i = 0
                While i < UBound(vrows, 2)
                    orow = New bc_om_insight_row(CStr(vrows(1, i)), CStr(vrows(1, i + 1)), CStr(vrows(1, i + 2)), CStr(vrows(1, i + 3)), CStr(vrows(1, i + 4)), CStr(vrows(1, i + 5)), CStr(vrows(1, i + 6)), static_flag, CStr(vrows(1, i + 7)), CStr(vrows(1, i + 8)), CStr(vrows(1, i + 9)))
                    orow.certificate = MyBase.certificate
                    orow.row_id = vrows(0, i)
                    Dim ocommentary As New bc_cs_activity_log("bc_om_insight_section", "db_read_custom", bc_cs_activity_codes.COMMENTARY, "Adding row: " + CStr(vrows(0, i)), certificate)
                    i = i + 10
                    orow.db_read(template_id, entity_id, link_order_code, MyBase.certificate)
                    rows.Add(orow)
                End While
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_section", "db_read_custom", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_section", "db_read_custom", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Shadows Sub db_write(ByVal contributor_id, ByVal workflow_stage, ByVal accounting_standard, ByVal entity_id, ByVal date_from)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_section", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim i As Integer
        Try
            For i = 0 To Me.rows.Count - 1
                With Me.rows(i)
                    .db_write(contributor_id, workflow_stage, accounting_standard, entity_id, date_from)
                End With
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_section", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_section", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    Public Sub db_read_values(ByVal contributor_id, ByVal workflow_stage, ByVal accounting_standard, ByVal entity_id, ByVal date_from)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_section", "db_read_value", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim i As Integer
        Dim tmp As Integer
        Try
            For i = 0 To Me.rows.Count - 1
                With Me.rows(i)
                    Try
                        tmp = CInt(.datatype)
                        If .datatype <> "" Or .datatype = 6 Then
                            .db_read_value(contributor_id, workflow_stage, accounting_standard, entity_id, date_from)
                        End If
                    Catch
                        Dim ocommentary As New bc_cs_activity_log("bc_om_insight_section", "db_read_value", bc_cs_activity_codes.COMMENTARY, "No Datatype set:" + .label_code, certificate)
                    End Try
                End With
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_section", "db_read_value", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_section", "db_read_value", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    Public Sub db_write_custom_section(ByVal logical_template_id As Long, ByVal entity_id As Long)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_section", "db_write_custom_section", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim i As Integer
            For i = 0 To rows.Count - 1
                rows(i).db_write_custom_row(logical_template_id, entity_id, i)
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_section", "db_write_custom_section", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_section", "db_write_custom_section", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_result_types
    Inherits bc_cs_soap_base_class
    Public bc_om_result_types As New ArrayList
    Public Sub New()

    End Sub
    Public Shadows Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_result_types", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim vresults As Object
        Dim i As Integer
        Dim oresult_type As Object
        Try

            Dim gbc As New bc_in_excel_submission_db
            vresults = gbc.get_result_types(MyBase.certificate)
            If IsArray(vresults) Then
                For i = 0 To UBound(vresults, 2)
                    oresult_type = New bc_om_result_type
                    oresult_type.period_id = vresults(0, i)
                    oresult_type.description = vresults(1, i)
                    bc_om_result_types.Add(oresult_type)
                Next
            End If

        Catch ex As Exception

        Finally
            otrace = New bc_cs_activity_log("bc_om_result_types", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
End Class
<Serializable()> Public Class bc_om_accounting_standards
    Inherits bc_cs_soap_base_class
    Public bc_om_accounting_standards As New ArrayList
    Public Sub New()

    End Sub
    Public Shadows Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_accounting_standards", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim vresults As Object
        Dim i As Integer
        Dim oaccounting_standard As Object
        Try

            Dim gbc As New bc_in_excel_submission_db
            vresults = gbc.get_accounting_standards(MyBase.certificate)
            If IsArray(vresults) Then
                For i = 0 To UBound(vresults, 2)
                    oaccounting_standard = New bc_om_accounting_standard
                    oaccounting_standard.id = vresults(0, i)
                    oaccounting_standard.name = vresults(1, i)
                    oaccounting_standard.desc = vresults(2, i)
                    oaccounting_standard.active = vresults(3, i)
                    bc_om_accounting_standards.Add(oaccounting_standard)
                Next
            End If

        Catch ex As Exception

        Finally
            otrace = New bc_cs_activity_log("bc_om_accounting_standards", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
End Class
<Serializable()> Public Class bc_om_accounting_standard
    Inherits bc_cs_soap_base_class
    Public id As Long
    Public name As String
    Public desc As String
    Public active As Integer
    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class bc_om_result_type
    Inherits bc_cs_soap_base_class
    Public Period_id As Long
    Public Description As String
    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class bc_om_insight_row
    Inherits bc_cs_soap_base_class
    REM insight row id
    Public row_id As Long
    REM mapping code for linking purposes
    Public mapping_code As String
    REM display label string
    Public label As String
    Public label_code As String
    REM display scale symbol
    Public scale_symbol As String
    REM scale factor
    Public scale_factor As String
    REM display order
    Public order As String
    REM flexible label flag
    Public flexible_label_flag As String
    REM flexible label value
    Public flexible_label_value As New ArrayList
    REM datatype
    REM datatype 6 is output only value
    Public datatype As String
    REM label link formular
    Public label_formula As String
    REM link code
    Public link_code As String
    REM Static_flag as string
    Public static_flag As String
    REM storage method
    Public submission_code As String
    REM used for repeating attributes
    Public repeating_count As String
    REM link found flag used in linking to see if link code has already been matched
    Public link_found As Boolean
    REM row number inserted in excel
    Public excel_row As Integer
    REM change validation flag
    Public change_validation_flag As Boolean
    REM look up sql
    Public lookup_sql As String
    REM values
    Public values_list As New ArrayList
    REM moneytary
    Public monetary As Integer
    Public price_related As Integer
    Public validations As New bc_om_cell_validations
    REM FIL 5.7
    Public warnings As New bc_om_cell_validations
    REM values these are the values returned back from insight contribution
    REM for the row
    Public Sub New()

    End Sub
    Public Sub New(ByVal label_code, ByVal scale_symbol, ByVal scale_factor, ByVal order, ByVal flexible_label_flag, ByVal link_code, ByVal data_type, ByVal static_flag, ByVal submission_code, ByVal repeating_count, ByVal lookup_sql)
        Me.label_code = label_code
        Me.scale_symbol = scale_symbol
        Me.scale_factor = scale_factor
        Me.order = order
        Me.flexible_label_flag = flexible_label_flag
        Me.link_code = link_code
        Me.datatype = data_type
        Me.static_flag = static_flag
        Me.submission_code = submission_code
        If Me.static_flag = 0 Then
            Me.submission_code = 0
        End If
        Me.repeating_count = repeating_count
        REM reset flag that indicates row has a change
        Me.lookup_sql = lookup_sql

    End Sub
    REM get label for code
    Public Shadows Sub db_read(ByVal template_id As Integer, ByVal entity_id As Long, ByVal link_order_code As Long, ByVal certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_row", "db_read", bc_cs_activity_codes.TRACE_ENTRY, certificate.user_id, certificate)
        Try
            Dim db_insight As New bc_in_excel_submission_db
            Dim vlabel As Object
            Dim i As Integer
            If Me.flexible_label_flag = True Then
                Me.flexible_label_value.Clear()
                For i = 0 To Me.repeating_count
                    vlabel = db_insight.get_label_for_flexible_code(entity_id, Me.row_id, i, certificate)
                    If IsArray(vlabel) Then
                        If UBound(vlabel, 2) > -1 Then
                            Try
                                Me.label = vlabel(0, 0)
                                Me.flexible_label_value.Add(vlabel(0, 0))
                            Catch

                                Me.flexible_label_value.Add("")
                            End Try
                        Else
                            vlabel = db_insight.get_label_for_code(Me.label_code, Me.datatype, Me.lookup_sql, MyBase.certificate)
                            Me.label = vlabel
                            Me.flexible_label_value.Add(vlabel)
                        End If
                    Else
                        vlabel = db_insight.get_label_for_code(Me.label_code, Me.datatype, Me.lookup_sql, MyBase.certificate)
                        Me.label = vlabel
                        Me.flexible_label_value.Add(vlabel)
                    End If
                Next
            Else
                vlabel = db_insight.get_label_for_code(Me.label_code, Me.datatype, Me.lookup_sql, MyBase.certificate)
                Me.label = vlabel
            End If
            REM link code
            If link_order_code > 0 And IsNumeric(link_order_code) Then
                Me.link_code = Me.link_code + CStr(link_order_code)
            End If
            Me.change_validation_flag = False
            validations.db_read(False, row_id, template_id, Me.change_validation_flag, lookup_sql, MyBase.certificate)
            If Me.change_validation_flag = True Then
                Dim ocommentary As New bc_cs_activity_log("bc_om_insight_row", "db_read", bc_cs_activity_codes.COMMENTARY, "Change value flag set for row: " + CStr(row_id), certificate)
            End If
            warnings.db_read(True, row_id, template_id, Me.change_validation_flag, lookup_sql, MyBase.certificate)


            REM monetary
            Dim vres As Object
            Me.monetary = 0
            vres = db_insight.get_monetary_type(Me.row_id, Me.static_flag, MyBase.certificate)
            If IsArray(vres) Then
                If UBound(vres, 2) > -1 Then
                    Me.monetary = vres(0, 0)
                End If
            End If
            Me.price_related = 0


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_row", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_row", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Shadows Sub db_write_config()

    End Sub
    Public Shadows Sub db_write(ByVal contributor_id, ByVal workflow_stage, ByVal accounting_standard, ByVal entity_id, ByVal date_from)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_row", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim i As Integer
        Dim gbc_db As New bc_in_excel_submission_db

        Try
            REM if flexible write down label
            If Me.flexible_label_flag = True Then
                For i = 0 To flexible_label_value.Count - 1
                    gbc_db.insert_flexible_label(flexible_label_value.Item(i), Me.row_id, entity_id, contributor_id, i, certificate)
                Next
            End If
            For i = 0 To Me.values_list.Count - 1
                With Me.values_list(i)
                    If Not IsNumeric(Me.scale_factor) Then
                        Me.scale_factor = 1
                    End If
                    .db_write(contributor_id, workflow_stage, accounting_standard, entity_id, Me.row_id, Me.label_code, Me.scale_factor, CStr(date_from), submission_code)
                End With
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_row", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_row", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    Public Shadows Sub db_read_value(ByVal contributor_id, ByVal workflow_stage, ByVal accounting_standard, ByVal entity_id, ByVal date_from)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_row", "db_read_value", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim i As Integer
        Dim gbc_db As New bc_in_excel_submission_db

        Try
            REM only do for outout type value
            If Me.datatype = 6 Then

                For i = 0 To Me.values_list.Count - 1
                    With Me.values_list(i)
                        If Me.values_list(i).current = True Then
                            If Not IsNumeric(Me.scale_factor) Then
                                Me.scale_factor = 1
                            End If
                            REM looksq holds contributor id for this
                            .db_read(lookup_sql, workflow_stage, accounting_standard, entity_id, Me.row_id, Me.label_code, Me.scale_factor, CStr(date_from), submission_code)
                        End If
                    End With
                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_row", "db_read_value", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_row", "db_read_value", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    Public Sub db_write_custom_row(ByVal logical_template_id As Long, ByVal entity_id As Long, ByVal ord As Long)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_row", "db_write_custom_row", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim db_insight As New bc_in_excel_submission_db
            db_insight.create_custom_items(logical_template_id, entity_id, Me.label, Me.static_flag, Me.datatype, Me.scale_symbol, Me.scale_factor, Me.link_code, Me.submission_code, Me.repeating_count, Me.flexible_label_flag, ord, certificate)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_sheet", "db_write_custom_row", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_row", "db_write_custom_row", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
End Class
<Serializable()> Public Class bc_om_insight_rows_cell_value
    Inherits bc_cs_soap_base_class
    Public year As String
    Public period_id As String
    Public year_formula As String
    Public period_id_formula As String
    Public e_a_formula As String
    Public accounting_standard As String
    Public acc_formula As String
    Public value As String
    Public row As String
    Public column As String
    Public order As String
    Public comment As String
    Public formula As String
    Public label_formula As String
    Public current As Boolean = True
    Public previous As String
    Public enable_disable_formula As String

    REM used for db xml
    Public row_id As Long
    Public scalefactor As Double
    Public contributor_code As Integer
    Public flexible_label_value As String
    Public Sub New()

    End Sub
    Public Sub New(ByVal row_id, ByVal scalefactor, ByVal contributor_code, ByVal flexible_label)
        Me.row_id = row_id
        Try
            If IsNumeric(scalefactor) Then
                Me.scalefactor = scalefactor
            Else
                Me.scalefactor = 1
            End If
            Me.contributor_code = contributor_code
            Me.flexible_label_value = flexible_label
        Catch ex As Exception

            Dim db_err As New bc_cs_error_log("bc_om_insight_row_cell_value", "new", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        End Try
    End Sub
    Public Shadows Sub db_write(ByVal contributor_id As Long, ByVal workflow_stage As Long, ByVal accounting_standard As Long, ByVal entity_id As Long, ByVal row_id As Long, ByVal label_code As String, ByVal scale_factor As Double, ByVal date_from As String, ByVal dimension_id As Integer)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_row_cell_value", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim gbc As New bc_in_excel_submission_db
            REM only write down current value
            If Me.current = True Then
                If dimension_id = 0 Then
                    gbc.insert_row(contributor_id, accounting_standard, workflow_stage, entity_id, year, period_id, value, date_from, row_id, scale_factor, comment, certificate)
                Else
                    gbc.insert_static_row(contributor_id, accounting_standard, workflow_stage, entity_id, value, date_from, scale_factor, row_id, dimension_id, order, comment, certificate)
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_row_cell_value", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_row_cell_value", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    REM reads actually data value back into object model
    REM uselful for supplying calc values back to excel
    Public Shadows Sub db_read(ByVal contributor_id As Long, ByVal workflow_stage As Long, ByVal accounting_standard As Long, ByVal entity_id As Long, ByVal row_id As Long, ByVal label_code As String, ByVal scale_factor As Double, ByVal date_from As String, ByVal dimension_id As Integer)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_row_cell_value", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim gbc As New bc_in_excel_submission_db
            Me.value = "nm"
            If dimension_id = 0 Then
                Me.value = gbc.retrieve_value(contributor_id, accounting_standard, workflow_stage, entity_id, year, period_id, value, date_from, row_id, scale_factor, comment, certificate)
            Else
                Me.value = gbc.retrieve_static(contributor_id, accounting_standard, workflow_stage, entity_id, value, date_from, scale_factor, row_id, dimension_id, order, comment, certificate)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_row_cell_value", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_row_cell_value", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
End Class
REM EVR JUNE 2013
Public Class bc_om_list_boxs

End Class
Public Class bc_om_list_box
    Public sheetno As Integer
    Public row As Integer
    Public column As Integer
    Public count As Integer
    Public values As New ArrayList
End Class
<Serializable()> Public Class bc_om_cell_validations
    Inherits bc_cs_soap_base_class
    Public bc_om_cell_validation As New ArrayList
    Public Sub New()
    End Sub
    Public Shadows Sub db_read(warnings As Boolean, ByVal row_id As Long, ByVal template_id As Integer, ByRef change_validation_flag As Boolean, ByVal sql As String, ByRef certificate As bc_cs_security.certificate)
        Dim vvalidation As New Object
        Dim gbc As New bc_in_excel_submission_db
        Dim res As Object
        Dim i, j As Integer
        Dim ocell_validation As bc_om_cell_validation
        MyBase.certificate = certificate
        If warnings = True Then
            vvalidation = gbc.get_warnings_for_row(row_id, template_id, MyBase.certificate)
            If IsArray(vvalidation) Then
                For i = 0 To UBound(vvalidation, 2)
                    ocell_validation = New bc_om_cell_validation(vvalidation(0, i), vvalidation(1, i), vvalidation(2, i), vvalidation(3, i), vvalidation(4, i), certificate)
                    ocell_validation.valid_values_list.Clear()
                    bc_om_cell_validation.Add(ocell_validation)
                Next
            End If
        Else
            vvalidation = gbc.get_validations_for_row(row_id, template_id, MyBase.certificate)
            If IsArray(vvalidation) Then
                For i = 0 To UBound(vvalidation, 2)
                    ocell_validation = New bc_om_cell_validation(vvalidation(0, i), vvalidation(1, i), vvalidation(2, i), vvalidation(3, i), vvalidation(4, i), certificate)
                    ocell_validation.valid_values_list.Clear()

                    REM validation
                    If CInt(vvalidation(0, i) = 7) Then
                        REM look up validation
                        res = gbc.get_lookup_validations_for_row(sql, MyBase.certificate)
                        If Not IsNothing(res) Then
                            If UBound(res, 2) > -1 Then
                                For j = 0 To UBound(res, 2)
                                    ocell_validation.valid_values_list.Add(CStr(res(0, j)))
                                    'If j > 0 Then
                                    '    ocell_validation.valid_values_list = ocell_validation.valid_values_list + "," + CStr(res(0, j))
                                    'Else
                                    '    ocell_validation.valid_values_list = CStr(res(0, j))
                                    'End If
                                Next
                            End If
                        End If
                    End If
                    bc_om_cell_validation.Add(ocell_validation)
                    If CInt(vvalidation(2, i) = 1) Or CInt(vvalidation(2, i) = 2) Then
                        change_validation_flag = True
                    End If
                Next
            End If
        End If
    End Sub
End Class
<Serializable()> Public Class bc_om_cell_validation
    Public validation_id As Integer
    Public comment_req As Integer
    Public mode As Integer
    Public value1 As Double
    Public value2 As Double
    Public valid_values_list As New ArrayList
    Public Sub New(ByVal validation_id As Integer, ByVal comment_req As Integer, ByVal mode As Integer, ByVal value1 As Double, ByVal value2 As Double, ByRef certificate As bc_cs_security.certificate)
        Me.validation_id = validation_id
        Me.comment_req = comment_req
        Me.mode = mode
        Me.value1 = value1
        Me.value2 = value2
    End Sub
    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_cell_errors
    Public bc_om_cell_errors As New ArrayList
    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_cell_error
    Public msg As String
    Public row As Integer
    Public col As Integer
    Public sheetname As String
    Public comment_req As Integer
    Public Sub New(ByVal msg, ByVal row, ByVal col, ByVal sheetname, ByVal comment_req)
        Me.msg = msg
        Me.row = row
        Me.col = col
        Me.sheetname = sheetname
        Me.comment_req = comment_req
    End Sub
End Class
<Serializable()> Public Class bc_om_insight_retrieve_values
    Inherits bc_cs_soap_base_class
    Public entity_id As New ArrayList
    Public stage_id As Long
    Public contributor_id As Long
    Public accounting_id As Long
    Public values As New ArrayList
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_retrieve_values", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_retrieve_values", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_retrieve_values", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_retrieve_values", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim i As Integer
            For i = 0 To values.Count - 1
                values(i).certificate = MyBase.certificate
                values(i).db_read(Me.entity_id, Me.stage_id, Me.contributor_id, Me.accounting_id)
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_retrieve_values", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_retrieve_values", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    'Public Overloads Overrides Function call_web_service(ByVal s As String) As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "call_web_service (s)", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    If IsNumeric(bc_cs_central_settings.timeout) Then
    '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
    '    End If

    '    call_web_service = webservice.LoadDataFromInsight(s)
    '    otrace = New bc_cs_activity_log("bc_om_insight_contribution_for_entity", "call_web_service (s)", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    'End Function
End Class
<Serializable()> Public Class bc_om_insight_retrieve_value
    Inherits bc_cs_soap_base_class
    Public attribute_code As String
    Public submission_code As Long
    Public value As New ArrayList
    Public value_entity As New ArrayList
    Public sheet As String
    Public row As Integer
    Public col As Integer
    Public dimension As String
    Public contributor_id As String
    Public scale_factor As Double
    Public order As Integer
    Public Sub New()

    End Sub
    Public Shadows Sub db_read(ByVal entity_ids As ArrayList, ByVal stage_id As Long, ByVal contributor_id As Long, ByVal accounting_id As Long)
        Dim otrace As New bc_cs_activity_log("bc_om_insight_retrieve_value", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim i As Integer
            Dim gdb As New bc_in_excel_submission_db

            If Me.contributor_id <> "" Then
                contributor_id = Me.contributor_id
            End If
            For i = 0 To entity_ids.Count - 1
                value.Add(gdb.get_item(attribute_code, submission_code, entity_ids(i), stage_id, contributor_id, accounting_id, dimension, order, certificate))
                value_entity.Add(entity_ids(i))
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_retrieve_value", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_retrieve_value", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

End Class
<Serializable()> Public Class bc_om_insight_template_config
    Inherits bc_cs_soap_base_class
    Public logical_template_name As String
    Public logical_template_id As Long
    Public context_id As Long

    Public rows As New ArrayList
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_template_config", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_template_config", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_template_config", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Function db_read() As Long
        Dim otrace As New bc_cs_activity_log("bc_om_insight_template_config", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim gdb As New bc_in_excel_submission_db
            Dim i As Integer
            Dim vtid As Object
            Dim ltemplate_name As String
            Dim fs As New bc_cs_string_services(Me.logical_template_name)

            ltemplate_name = fs.delimit_apostrophies()
            REM template deletion 
            If Me.logical_template_name = "DELETE" Then
                gdb.delete_logical_template(Me.logical_template_id, 1, MyBase.certificate)
                Exit Function
            End If
            REM delete data for this template context combination
            If Me.logical_template_id = 0 Then
                vtid = gdb.get_template_id(ltemplate_name, MyBase.certificate)
                If IsArray(vtid) Then
                    If UBound(vtid, 2) >= 0 Then
                        Me.logical_template_id = CInt(vtid(0, 0))
                        If Me.logical_template_id > 0 Then
                            REM duplicate decteced so flag this and quit
                            Me.logical_template_id = -1
                            Exit Function
                        End If
                    End If
                End If
            End If

            REM delete data for this template context combination
            gdb.delete_logical_template_config(Me.logical_template_id, Me.context_id, MyBase.certificate)
            For i = 0 To rows.Count - 1
                rows(i).db_write(ltemplate_name, Me.context_id, MyBase.certificate)
            Next
            REM now extract new template id
            vtid = gdb.get_template_id(ltemplate_name, MyBase.certificate)
            If IsArray(vtid) Then
                If UBound(vtid, 2) >= 0 Then

                    Me.logical_template_id = CInt(vtid(0, 0))
                Else
                    Me.logical_template_id = -99
                End If
            Else
                Me.logical_template_id = -99
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_template_config", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_template_config", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Overrides Function write_data_to_xml(ByRef certificate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_om_insight_template_config", "write_data_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim cbc_data_services As New bc_cs_data_services
        Dim ocommentary As New bc_cs_activity_log("bc_om_insight_template_config", "write_data_to_xml", bc_cs_activity_codes.COMMENTARY, "Attempting to write XML string")

        write_data_to_xml = soap_serialize_object_to_xml(Me, Me.certificate)

        otrace = New bc_cs_activity_log("bc_om_insight_template_confi", "write_data_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    End Function
    Private Function soap_serialize_object_to_xml(ByVal o As Object, ByRef certificate As bc_cs_security.certificate, Optional ByVal packet_code As String = "") As String

        Dim otrace As New bc_cs_activity_log("bc_om_insight_template_config", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Dim bc_cs_central_settings As New bc_cs_central_settings
        Dim by() As Byte

        Try
            Using ms As New MemoryStream

                Dim extratype(0) As Type

                extratype(0) = GetType(bc_om_insight_row_config)

                Dim sf As New XmlSerializer(o.GetType, extratype)

                sf.Serialize(ms, o)

                by = ms.ToArray()

                ms.Close()

                'xml serialization does not always encode apostrophe's.  This causes an issue when passing to SQL Server
                soap_serialize_object_to_xml = Replace(System.Text.Encoding.UTF8.GetString(by), "'", "&#39;")

            End Using

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_template_config", "soap_serialize_object_to_xml", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            soap_serialize_object_to_xml = ""
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_template_config", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    'Public Overloads Overrides Function call_web_service(ByVal s As String) As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_insight_template_config", "aacall_web_service (s)", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    If IsNumeric(bc_cs_central_settings.timeout) Then
    '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
    '    End If
    '    call_web_service = webservice.UpLoadInsightSheet(s)

    '    otrace = New bc_cs_activity_log("bc_om_insight_template_config", "call_web_service (s)", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    'End Function
    'Public Shadows Function write_xml_to_soap_server_reponse(ByVal s As String) As String
    '    Dim cs As New bc_cs_central_settings(True)
    '    Dim otrace As New bc_cs_activity_log("bc_om_insight_template_config", "write_xml_to_soap_server_reponse", bc_cs_activity_codes.TRACE_ENTRY, "", MyBase.certificate)

    '    Try
    '        REM initialse database
    '        Dim db As New bc_cs_db_services(False)
    '        Dim odoc As New bc_om_insight_template_config

    '        odoc.certificate = MyBase.certificate

    '        Dim oxml As New bc_cs_xml_services
    '        REM decrypt
    '        Dim osec As New bc_cs_security
    '        s = osec.decrypt_xml(s, certificate)
    '        odoc = oxml.soap_deserialize_string_to_object(s, Me.certificate)
    '        REM product response and return
    '        'Me.id = CLng(doc_id)
    '        odoc.db_read(Me.certificate)
    '        REM write object to string
    '        write_xml_to_soap_server_reponse = osec.encrypt_xml(odoc.write_xml_to_string, certificate)
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_om__insight_template_config", "write_xml_to_soap_server_reponse", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '    Finally
    '        otrace = New bc_cs_activity_log("bc_om__insight_template_config", "write_xml_to_soap_server_reponse", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try

    'End Function
End Class

<Serializable()> Public Class bc_om_insight_row_config
    Inherits bc_cs_soap_base_class
    Public row_id As Long
    Public template_name As String
    Public context_id As Long
    Public section_name As String
    Public static_flag As Integer
    Public label As String
    Public scale_symbol As String
    Public scale_factor As String
    Public order As Integer
    Public flexible_label_flag As Integer
    Public link_code As String
    Public data_type As Integer
    Public submission_code As Integer
    Public repeating_count As Integer
    Public lookup_sql As String
    Public monetary As Integer
    Public price_related As Integer
    Public validations As New ArrayList
    Public warnings As New ArrayList
    Public Sub New()

    End Sub

    Public Shadows Sub db_write(ByVal logical_template_name As String, ByVal context_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim gdb As New bc_in_excel_submission_db
        Dim i As Integer
        Dim strval As String
        Dim swarnings As String = ""
        strval = ""
        For i = 0 To validations.Count - 1
            strval = strval + "," + CStr(validations(i))
        Next
        For i = 0 To warnings.Count - 1
            swarnings = swarnings + "," + CStr(warnings(i))
        Next
        Dim fs As bc_cs_string_services
        fs = New bc_cs_string_services(Me.section_name)
        Me.section_name = fs.delimit_apostrophies
        fs = New bc_cs_string_services(Me.template_name)
        Me.template_name = fs.delimit_apostrophies
        fs = New bc_cs_string_services(Me.label)
        Me.label = fs.delimit_apostrophies
        fs = New bc_cs_string_services(Me.scale_symbol)
        Me.scale_symbol = fs.delimit_apostrophies
        fs = New bc_cs_string_services(logical_template_name)
        logical_template_name = fs.delimit_apostrophies
        fs = New bc_cs_string_services(lookup_sql)
        lookup_sql = fs.delimit_apostrophies

        gdb.insert_row_config(logical_template_name, context_id, Me.section_name, Me.static_flag, Me.label, Me.scale_symbol, Me.scale_factor, Me.order, Me.flexible_label_flag, Me.link_code, Me.data_type, Me.submission_code, Me.repeating_count, Me.lookup_sql, strval, row_id, Me.monetary, Me.price_related, swarnings, certificate)
    End Sub
End Class
REM =============================================================
REM class for database ineraction for bc_at_template object model
Public Class bc_in_excel_submission_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub
    REM FIL JUNE 2014 audit fix
    Public Function get_audit_id(ByVal source_id As Integer, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_write_new_data_input_audit " + CStr(source_id)
        get_audit_id = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub set_audit_end_date(ByVal audit_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_set_data_input_end_date " + CStr(audit_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    REM =====================================
    Public Function get_calc_types(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select calculation_type,type,is_agg,ord from bc_core_calc_type_order order by ord asc"
        get_calc_types = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_reserved_words(ByRef certificate As bc_cs_security.certificate)
        read_reserved_words = gbc_db.executesql("exec dbo.bc_core_get_reserved_words", certificate)
    End Function
    Public Function read_aggregation_types(ByRef certificate As bc_cs_security.certificate)
        read_aggregation_types = gbc_db.executesql("exec dbo.bc_core_get_aggregation_types", certificate)
    End Function

    Public Function is_sp(ByVal formula As String, ByRef certificate As bc_cs_security.certificate) As Boolean
        Dim sql As String
        is_sp = False
        sql = "SELECT * FROM sys.objects WHERE name='" + formula + "' AND type in (N'P', N'PC')"
        Dim vres As Object = gbc_db.executesql(sql, certificate)
        If UBound(vres, 2) >= 0 Then
            is_sp = True
        End If
    End Function

    Public Function verifyCalculation(ByVal strFormula As String, ByVal boolIsAgg As Boolean, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        Dim isAgg As String = "0"
        If boolIsAgg Then
            isAgg = "1"
        End If
        sql = "exec dbo.bc_core_calc_test_prelim  '" & Replace(strFormula, "'", "''") & "', " & isAgg
        Dim vres As Object = gbc_db.executesql(sql, certificate)
        If IsArray(vres) Then
            verifyCalculation = vres(0, 0)
        Else
            verifyCalculation = 0
        End If
    End Function
    Public Sub register_workbook(ByVal master_entity_id As Long, ByVal class_id As Long, ByVal contributor_id As Long, ByVal workbook_name As String, ByVal author_id As Long, ByRef workbook As Byte(), ByVal checked_out_user As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim exists As Object
        REM check workbook isnt rehistered keyed on master_enttity_id and contributor_id
        sql = "select master_entity_id from bc_insight_submission_workbooks where master_entity_id=" + CStr(master_entity_id) + " and contributor_id=" + CStr(contributor_id)
        exists = gbc_db.executesql(sql, certificate)
        Dim ostring As New bc_cs_string_services(workbook_name)
        workbook_name = ostring.delimit_apostrophies
        If IsArray(exists) Then
            If UBound(exists, 2) > -1 Then
                sql = "update bc_insight_submission_workbooks set author_id = '" + CStr(author_id) + "', workbook_name='" + workbook_name + "', submission_date=getdate(), user_id=" + CStr(checked_out_user) + " where master_entity_id=" + CStr(master_entity_id) + " and contributor_id=" + CStr(contributor_id)
                gbc_db.executesql(sql, certificate)
                gbc_db.executesp_for_workbook_submission("dbo.bc_store_images", CStr(master_entity_id), CStr(contributor_id), workbook, 0, certificate)
            Else
                sql = "insert into bc_insight_submission_workbooks(master_entity_id,class_id, contributor_id,workbook_name,author_id, submission_date, user_id) values(" + CStr(master_entity_id) + "," + CStr(class_id) + "," + CStr(contributor_id) + ",'" + workbook_name + "'," + CStr(author_id) + ", getdate()," + CStr(checked_out_user) + ")"
                gbc_db.executesql(sql, certificate)
                gbc_db.executesp_for_workbook_submission("dbo.bc_store_images", CStr(master_entity_id), CStr(contributor_id), workbook, 0, certificate)
            End If
        Else
            REM insert
            sql = "insert into bc_insight_submission_workbooks(master_entity_id,class_id, contributor_id,workbook_name,author_id, submission_date) values(" + CStr(master_entity_id) + "," + CStr(class_id) + "," + CStr(contributor_id) + ",'" + workbook_name + "'," + CStr(author_id) + ", getdate())"
            gbc_db.executesql(sql, certificate)
            gbc_db.executesp_for_workbook_submission("dbo.bc_store_images", CStr(master_entity_id), CStr(contributor_id), workbook, 0, certificate)
        End If
    End Sub
    REM gets all submission classes and their child links
    Public Function get_insight_entity_class_submission(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim luser As String
        If bc_cs_central_settings.server_flag = 0 Then
            luser = bc_cs_central_settings.logged_on_user_id
        Else
            luser = certificate.user_id

        End If
        sql = "select distinct i.parent_entity_class_id,i.child_entity_class_id,e.class_name, s.schema_id, s.schema_name from " + _
              " schema_tbl s, insight_entity_class_submission i, entity_class_tbl e, bus_area_schema_link b, person_bus_area_tbl p where " + _
              " i.contributor_id=s.schema_id and " + _
              " b.schema_id=i.contributor_id and " + _
              " b.schema_id=i.contributor_id and " + _
              " p.bus_area_id = b.bus_area_id and " + _
              " p.user_id ='" + CStr(luser) + "' and " + _
              " parent_entity_class_id = class_Id order by parent_entity_class_id asc"
        get_insight_entity_class_submission = gbc_db.executesql(sql, certificate)
    End Function
    Public Function test_calculation(ByVal formula As String, ByVal is_agg As Boolean, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        sql = "exec dbo.bcc_core_calc_test '" + formula + "'," + CStr(is_agg)
        test_calculation = gbc_db.test_sql(sql, certificate)
    End Function


    REM gets all child entities that can be contributed for the
    REM selected master parent class needs to propogate in the future
    Public Function get_all_contributing_child_entities(ByVal entity_id As Long, ByVal class_id As Long, ByVal contributor_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim i As Integer
        Dim child_class_id As Object

        get_all_contributing_child_entities = Nothing

        REM see what the child class 
        sql = "select child_entity_class_id from insight_entity_class_submission where parent_entity_class_id=" + CStr(class_id) + " and contributor_id=" + CStr(contributor_id)
        child_class_id = gbc_db.executesql(sql, certificate)
        REM is exists get entities for this class for the parent
        If IsArray(child_class_id) Then
            For i = 0 To UBound(child_class_id, 2)
                sql = "select entity_id, name, entity_tbl.class_id, class_name from entity_tbl, entity_link_tbl, entity_class_tbl where parent_entity_id = " + CStr(entity_id) + " And child_entity_id = entity_id And entity_tbl.class_id =" + CStr(child_class_id(0, i)) + " and entity_tbl.class_id = entity_class_tbl.class_id and coalesce(entity_tbl.inactive,0)=0 and coalesce(entity_tbl.deleted,0) = 0 and schema_id=" + CStr(contributor_id) + " order by entity_link_tbl.child_parent_rating asc"
                get_all_contributing_child_entities = gbc_db.executesql(sql, certificate)
            Next
        End If
    End Function
    Public Function is_entity_inactive(ByVal entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Boolean
        Dim sql As String
        Dim res As Object
        is_entity_inactive = False
        sql = "select count(*) from entity_attribute_value_tbl v, attribute_tbl a where a.label_code = 'attribute.entity.inactive' and a.attribute_id=v.attribute_id and v.attrib_value = '1' and v.entity_id=" + CStr(entity_id)
        res = gbc_db.executesql(sql, certificate)
        If IsArray(res) Then
            If res(0, 0) > 0 Then
                is_entity_inactive = True
                Exit Function
            End If
        End If
        sql = "select count(*) from entity_attribute_value_tbl v, attribute_tbl a where a.label_code = 'attribute.inc_build' and a.attribute_id=v.attribute_id and v.attrib_value = 'False' and v.entity_id=" + CStr(entity_id)

        res = gbc_db.executesql(sql, certificate)
        If IsArray(res) Then
            If res(0, 0) > 0 Then
                is_entity_inactive = True
                Exit Function
            End If
        End If

    End Function
    Public Function get_primary_entity(ByVal entity_id As Long, ByVal class_id As Long, ByVal contributor_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim child_class_id As Object
        REM see what the child class 
        sql = "select child_entity_class_id from insight_entity_class_submission where parent_entity_class_id=" + CStr(class_id) + " and contributor_id=" + CStr(contributor_id)
        child_class_id = gbc_db.executesql(sql, certificate)
        If IsArray(child_class_id) Then
            sql = "select e.entity_id from entity_tbl e,  " + _
                  "entity_attribute_value_tbl a,entity_attribute_value_tbl b, " + _
                  "entity_link_tbl l, entity_class_tbl c where  " + _
                  "l.parent_entity_id =" + CStr(entity_id) + _
                  "and l.child_entity_id = e.entity_id And e.class_id =" + CStr(child_class_id(0, 0)) + " and e.class_id = " + _
                  "c.class_id and " + _
                  "a.entity_id=l.parent_entity_id and " + _
                  "a.attribute_id=9 and " + _
                  "b.attribute_id= 25 and " + _
                  "e.entity_id=b.entity_id and " + _
                  "a.attrib_value = b.attrib_value and coalesce(e.inactive,0)=0 and coalesce(e.deleted,0)=0"
        End If
        get_primary_entity = gbc_db.executesql(sql, certificate)


    End Function
    Public Function get_logical_template(ByVal entity_id As Long, ByVal schema_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        get_logical_template = gbc_db.executesp_for_logical_template(entity_id, schema_id, certificate)
    End Function
    REM gets sections for template, contributor and class
    Public Function get_section_for_template(ByVal template_id As Long, ByVal static_flag As Integer, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select section_tbl.section_id,description from section_tbl, template_section_tbl where section_tbl.section_id=template_section_tbl.section_id and template_id=" + CStr(template_id) + " And context_id = 1 and static_flag=" + CStr(static_flag) + " order by display_order asc"
        get_section_for_template = gbc_db.executesql(sql, certificate)
    End Function
    REM get result types
    Public Function get_result_types(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select period_id, description from period_type_tbl"
        get_result_types = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_schemas(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select schema_id, schema_name from schema_tbl where coalesce(inactive,0)=0 and coalesce(deleted,0)= 0 order by schema_name asc"
        read_schemas = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_classes(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select class_id, class_name from entity_class_tbl where coalesce(inactive,0)=0 and coalesce(deleted,0)= 0 order by class_name asc"
        read_classes = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_entitites_for_class(ByVal granularity_class_id, ByVal certificate) As Object
        Dim sql As String
        sql = "select entity_id, name from entity_tbl where class_id=" + CStr(granularity_class_id) + " and coalesce(entity_tbl.inactive,0)=0 and coalesce(entity_tbl.deleted,0)=0order by name asc"
        get_entitites_for_class = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub force_check_in(ByVal lead_entity_id As Long, ByVal contributor_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "update bc_insight_submission_workbooks set user_id=0 where master_entity_id = " + CStr(lead_entity_id) + " And contributor_id = " + CStr(contributor_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function read_sections(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select description from section_tbl order by description asc"
        read_sections = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_class_links(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select schema_id, parent_class_id,child_class_id from class_link_tbl order by schema_id asc"
        get_class_links = gbc_db.executesql(sql, certificate)
    End Function
    'Public Sub delete_template_assignment(ByVal class_id As Long, ByVal schema_id As Long, ByRef certificate As bc_cs_security.certificate)
    '    Dim sql As String
    '    sql = "delete from bcc_core_insight_temp_assign where class_id=" + CStr(class_id) + " and schema_id=" + CStr(schema_id)
    '    gbc_db.executesql(sql, certificate)
    'End Sub
    REM FIL FEB 2013
    Public Sub insert_template_assignment(ByVal xml_assignments As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim fs As New bc_cs_string_services(xml_assignments)
        xml_assignments = fs.delimit_apostrophies
        sql = "exec dbo.bc_core_add_cont_assign '" + CStr(xml_assignments) + "'"
        Dim db As New bc_cs_db_services
        db.open_conn(certificate)
        Dim success As Boolean = True
        db.success = True
        db.executesql_trans(sql, certificate)
        db.close_conn(db.success, certificate)

    End Sub
    Public Sub delete_template_assignment(ByVal xml_assignments As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim fs As New bc_cs_string_services(xml_assignments)
        xml_assignments = fs.delimit_apostrophies
        sql = "exec dbo.bc_core_del_cont_assign '" + CStr(xml_assignments) + "'"
        Dim db As New bc_cs_db_services
        db.open_conn(certificate)
        Dim success As Boolean = True
        db.success = True
        db.executesql_trans(sql, certificate)
        db.close_conn(db.success, certificate)

    End Sub

    'Public Sub insert_template_assignment(ByVal class_id As Long, ByVal schema_id As Long, ByVal entity_id As Long, ByVal template_id As Long, ByRef certificate As bc_cs_security.certificate)
    '    Dim sql As String
    '    sql = "insert into bcc_core_insight_temp_assign (class_id, schema_id, entity_id, template_id) values(" + CStr(class_id) + "," + CStr(schema_id) + "," + CStr(entity_id) + "," + CStr(template_id) + ")"
    '    gbc_db.executesql(sql, certificate)
    'End Sub

    Public Function get_accounting_standards(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select acc_stnd_id , name,""desc"",active from account_standard_tbl order by acc_stnd_id  asc"
        get_accounting_standards = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_template_name(ByVal logical_template_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select label_value from template_tbl t, label_value_tbl l where template_id=" + CStr(logical_template_id) + " and l.label_code=t.label_code and language_code ='en'"
        get_template_name = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_fixed_operands(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bcc_core_in_fixed_ops"
        read_fixed_operands = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_all_aggregations(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select entity_id, name from ENTITY_TBL where class_id in (select class_id from entity_class_tbl where class_name = 'Aggregation Universe') and (deleted = 0 or deleted is null)"
        get_all_aggregations = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_all_logical_templates(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select template_id, label_value from template_tbl t, label_value_tbl l where t.label_code=l.label_code and language_code ='en' order by template_id desc"
        get_all_logical_templates = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_calcs_for_aggregation(ByVal aggregation_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select calculation_id, calc_order, calculation_name,calculation_type,result_row_id, coalesce(operand_1_id,0),coalesce(operand_2_id,0),coalesce(operand_3_id,0),coalesce(operand_4_id,0),coalesce(operand_5_id,0),coalesce(operand_6_id,0),coalesce(operand_7_id,0),coalesce(operand_8_id,0),formula,num_years, calc_when,min_val,max_val,coalesce(min_txt,''),coalesce(max_txt,''),coalesce(is_extract,1), coalesce(inactive,0),coalesce(na_text,''),coalesce(contributor1_id,0) ,coalesce(contributor2_id,0), coalesce(interval_type,0),  coalesce(interval,0), coalesce(weighting_attribute,0),0,0,12 , coalesce(standard_deviation,0.0) from insight_calculations_tbl where agg_universe_id=" + CStr(aggregation_id) + " order by calculation_name, calc_order asc"
        get_calcs_for_aggregation = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_calcs_for_template(ByVal template_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select calculation_id, calc_order, calculation_name,calculation_type,result_row_id, coalesce(operand_1_id,0),coalesce(operand_2_id,0),coalesce(operand_3_id,0),coalesce(operand_4_id,0),coalesce(operand_5_id,0),coalesce(operand_6_id,0),coalesce(operand_7_id,0),coalesce(operand_8_id,0),formula,num_years, calc_when,min_val,max_val,coalesce(min_txt,''),coalesce(max_txt,''),coalesce(is_extract,1), coalesce(inactive,0),coalesce(na_text,''),coalesce(contributor1_id,0) ,coalesce(contributor2_id,0), coalesce(interval_type,0),  coalesce(interval,0),0,coalesce(exch_rate_method,0), coalesce(calenderized,0),coalesce(calenderized_month_end,12) , 0  from insight_calculations_tbl where template_id=" + CStr(template_id) + " order by calculation_name, calc_order asc"
        get_calcs_for_template = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_formula_for_row(ByVal row_id As Long, ByVal template_id As Nullable(Of Long), ByVal agg_universe_id As Nullable(Of Long), ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim strTemplateId, strAggregationId As String
        If Not template_id Is Nothing Then
            strTemplateId = template_id
        Else
            strTemplateId = "null"
        End If
        If Not agg_universe_id Is Nothing Then
            strAggregationId = agg_universe_id
        Else
            strAggregationId = "null"
        End If
        sql = "exec dbo.bcc_core_calc_form " + CStr(row_id) + "," + strTemplateId + ",'',0," + strAggregationId
        get_formula_for_row = gbc_db.executesql(sql, certificate)
    End Function

    REM rows for section
    Public Function get_rows_for_section(ByVal section_id As Long, ByVal template_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim res As Object

        REM later on this needs to have added section_row_tbl.template_Id = template_id
        REM but cant be dont until all clients section_row_tbl are updated with correct
        REM template id.
        REM first see if sp equivalent exists if so this
        res = gbc_db.executesql_show_no_error("exec dbo.bcc_core_in_section_rows " + CStr(template_id) + "," + CStr(section_id))
        If IsArray(res) Then
            If UBound(res, 2) > -1 Then
                If CStr(res(0, 0)) = "Error" Then

                    Dim ocommentary As New bc_cs_activity_log("bc_in_excel_submission_db", "get_rows_for_section", bc_cs_activity_codes.COMMENTARY, "SP: dbo.bcc_core_in_section_rows not installed so using core logic", certificate)
                Else
                    get_rows_for_section = res
                    Exit Function
                End If
            End If
        End If
        '" section_row_tbl.section_id = template_section_tbl.section_id and section_row_tbl.context_id=1 and section_row_tbl.template_id = 1 INNER JOIN " + _
        sql = "SELECT rct.row_id, rct.value " + _
            " FROM  row_context_tbl rct inner join " + _
            " section_row_tbl ON rct.row_id = section_row_tbl.row_id INNER JOIN " + _
            " template_section_tbl ON rct.template_id = template_section_tbl.template_id AND " + _
            " section_row_tbl.section_id = template_section_tbl.section_id and section_row_tbl.context_id=1 and section_row_tbl.template_id = 1 INNER JOIN " + _
            " template_tbl ON rct.template_id = template_tbl.template_id AND template_section_tbl.template_id = template_tbl.template_id INNER JOIN " + _
            " row_context_tbl rct_1 ON rct.row_id = rct_1.row_id AND " + _
            " template_tbl.template_id = rct_1.template_id INNER JOIN " + _
            " insight_attributes_tbl insight_attributes_tbl_1 ON rct_1.insight_attribute_id = insight_attributes_tbl_1.attribute_id " + _
            " WHERE (template_tbl.template_id = " + CStr(template_id) + _
            " AND (rct.context_id = 1) " + _
            " AND (rct_1.context_id = 1) " + _
            " AND (template_section_tbl.context_id = 1) " + _
            " AND (insight_attributes_tbl_1.label_code = 'fd.iattributes.order') " + _
            " AND (section_row_tbl.section_id) = " + CStr(section_id) + _
            " ) ORDER BY  convert(integer, rct_1.value), rct.insight_attribute_id"
        get_rows_for_section = gbc_db.executesql(sql, certificate)


    End Function
    Public Function get_custom_rows(ByVal template_id As Long, ByVal entity_id As Long, ByVal static_flag As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "SELECT rct.row_id, rct.value " + _
                   " FROM  row_context_entity_tbl rct inner join " + _
                   " row_context_entity_tbl rct_1 ON rct.row_id = rct_1.row_id inner join " + _
                   " insight_attributes_tbl insight_attributes_tbl_1 ON rct_1.insight_attribute_id = insight_attributes_tbl_1.attribute_id  " + _
                   " WHERE(rct.context_id = 1) " + _
                   " AND (rct_1.context_id = 1) " + _
                   " AND (rct.template_id=" + CStr(template_id) + ")" + _
                   " AND (rct_1.template_id=" + CStr(template_id) + ")" + _
                   " AND (rct.entity_id=" + CStr(entity_id) + ")" + _
                   " AND (rct_1.entity_id=" + CStr(entity_id) + ")" + _
                   " AND (insight_attributes_tbl_1.label_code = 'fd.iattributes.order')" + _
                   " AND (rct.static_flag=" + CStr(static_flag) + ")" + _
                   " AND (rct_1.static_flag=" + CStr(static_flag) + ")" + _
                   " ORDER BY  convert(integer, rct_1.value), rct.insight_attribute_id"

        get_custom_rows = gbc_db.executesql(sql, certificate)

    End Function
    Public Function get_label_for_code(ByVal label_code As String, ByVal datatype As Long, ByVal contributor_id As String, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        Dim luserid As String
        Dim lusername As String
        Dim olabel As Object
        Dim label As String = ""
        If bc_cs_central_settings.server_flag = 0 Then
            luserid = bc_cs_central_settings.logged_on_user_id
            lusername = bc_cs_central_settings.logged_on_user_name
        Else
            luserid = certificate.user_id
            lusername = certificate.name
        End If
        sql = "select l.label_value from label_value_tbl l,user_table u, language_table t where " + _
          " u.language_id=t.language_id  and " + _
          " t.language_code = l.language_code and " + _
          " l.label_code = '" + label_code + "'  and u.user_id=" + CStr(luserid)
        olabel = gbc_db.executesql(sql, certificate)

        If IsArray(olabel) Then
            If Not IsNothing(olabel(0, 0)) Then
                label = olabel(0, 0)
            End If
        End If
        REM if datatyoe is 6 output add contributor id
        If CStr(datatype) = "6" Then
            Try
                Dim tcid As Long
                Dim ocid As Object
                tcid = CInt(contributor_id)
                sql = "select contributor_name from contributor_tbl where contributor_id=" + CStr(contributor_id)
                ocid = gbc_db.executesql(sql, certificate)
                If IsArray(ocid) Then
                    If IsArray(ocid) Then
                        'label = label + " (" + CStr(ocid(0, 0)) + ")"
                    End If
                End If

            Catch
            End Try
        End If
        get_label_for_code = label
    End Function
    REM seals previous data set based on entity, workflow stage, contributor and accounting standard so new data can 
    REM be entered
    Public Sub seal_previous_data_set(ByVal date_to As String, ByVal entity_id As Long, ByVal cid As Long, ByVal acc_id As Long, ByVal st_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "update entity_period_tbl set date_to ='" + CStr(date_to) + "'  where entity_id=" + CStr(entity_id) + " and contributor_id=" + CStr(cid) + " and acc_standard=" + CStr(acc_id) + " And workflow_stage = " + CStr(st_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    REM get validations for row
    Public Function get_validations_for_row(ByVal row_id As Long, ByVal template_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select validation_id,comment_required, mode, value1, value2 from insight_cell_validation_for_row,insight_cell_validation_types  where template_id=" + CStr(template_id) + " and row_id=" + CStr(row_id) + " and validation_id=validation_type_id"
        get_validations_for_row = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_warnings_for_row(ByVal row_id As Long, ByVal template_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select validation_id,comment_required, mode, value1, value2 from insight_cell_warnings_for_row,insight_cell_validation_types  where template_id=" + CStr(template_id) + " and row_id=" + CStr(row_id) + " and validation_id=validation_type_id"
        get_warnings_for_row = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub insert_row(ByVal cid As Long, ByVal acc As Long, ByVal ws As Long, ByVal entity_Id As Long, ByVal year As String, ByVal period_name As String, ByVal value As String, ByVal date_from As String, ByVal row_id As Long, ByVal scale_factor As String, ByVal comment As String, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_in_excel_submission_db", "insert_row", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim sql As String
            Dim ea_flag = 0
            Dim vperiod As Object
            Dim period_id As Integer
            Dim ostring As bc_cs_string_services
            If Right(year, 1) = "E" Then
                ea_flag = 1
            End If
            year = Left(year, 4)
            REM convert value
            If IsNumeric(value) Then
                If IsNumeric(scale_factor) Then
                    value = value * scale_factor
                End If
                ostring = New bc_cs_string_services(value)
                value = ostring.delimit_apostrophies
            End If
            ostring = New bc_cs_string_services(comment)
            comment = ostring.delimit_apostrophies
            REM get period id
            sql = "select period_id from period_type_tbl where description='" + period_name + "'"
            vperiod = gbc_db.executesql(sql, certificate)
            If IsArray(vperiod) Then
                period_id = vperiod(0, 0)
            End If
            If value <> "" Then
                sql = "insert into entity_period_tbl(entity_id,year,period_id,date_from,date_to,e_a_flag, contributor_id,row_id, value,workflow_stage,acc_standard, comment) values (" + CStr(entity_Id) + "," + CStr(year) + "," + CStr(period_id) + ",'" + CStr(date_from) + "','9-9-9999'," + CStr(ea_flag) + "," + CStr(cid) + "," + CStr(row_id) + ",'" + CStr(value) + "'," + CStr(ws) + "," + CStr(acc) + ",'" + comment + "')"
            Else
                sql = "insert into entity_period_tbl(entity_id,year,period_id,date_from,date_to,e_a_flag, contributor_id,row_id, value,workflow_stage,acc_standard, comment) values (" + CStr(entity_Id) + "," + CStr(year) + "," + CStr(period_id) + ",'" + CStr(date_from) + "','9-9-9999'," + CStr(ea_flag) + "," + CStr(cid) + "," + CStr(row_id) + ",null," + CStr(ws) + "," + CStr(acc) + ",'" + comment + "')"
            End If
            gbc_db.executesql(sql, certificate)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_in_excel_submission_db", "insert_row", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_in_excel_submission_db", "insert_row", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Function retrieve_value(ByVal cid As Long, ByVal acc As Long, ByVal ws As Long, ByVal entity_Id As Long, ByVal year As String, ByVal period_name As String, ByVal value As String, ByVal date_from As String, ByVal row_id As Long, ByVal scale_factor As String, ByVal comment As String, ByRef certificate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_in_excel_submission_db", "retrieve_value", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim sql As String
            Dim ea_flag = 0
            Dim vperiod As Object
            Dim period_id As Integer
            Dim tcid
            REM check contributor is numeric
            Try
                tcid = CInt(cid)
            Catch ex As Exception
                Dim ocommentary = New bc_cs_activity_log("bc_in_excel_submission_db", "retrieve_static", bc_cs_activity_codes.COMMENTARY, "Contributor id for row:" + CStr(row_id) = " is not numeric", certificate)
                Return ""
            End Try
            value = "nm"
            REM get period id
            period_id = 1
            sql = "select period_id from period_type_tbl where description='" + period_name + "'"
            vperiod = gbc_db.executesql(sql, certificate)
            If IsArray(vperiod) Then
                period_id = vperiod(0, 0)
            End If
            If Right(year, 1) = "E" Then
                ea_flag = 1
            End If
            year = Left(year, 4)
            Dim ovalue As Object
            sql = "select coalesce(value,'') from entity_period_tbl where date_to='9-9-9999' and row_id=" + CStr(row_id) + " and period_id=" + CStr(period_id) + " and contributor_id=" + CStr(cid) + " and workflow_stage=" + CStr(ws) + " and acc_standard=" + CStr(acc) + " and year=" + CStr(year) + " and e_a_flag=" + CStr(ea_flag) + " and entity_id=" + CStr(entity_Id)
            ovalue = gbc_db.executesql(sql, certificate)
            If IsArray(ovalue) Then
                If UBound(ovalue, 2) >= 0 Then
                    value = ovalue(0, 0)
                End If
            End If
            REM convert value
            If IsNumeric(value) Then
                If IsNumeric(scale_factor) Then
                    value = value / scale_factor
                End If
            End If
            retrieve_value = value
        Catch ex As Exception
            REM EVR JUNE 2013
            'Dim db_err As New bc_cs_error_log("bc_in_excel_submission_db", "retrieve_value", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Dim ocomm As New bc_cs_activity_log("bc_in_excel_submission_db", "retrieve_value", bc_cs_activity_codes.COMMENTARY, "error: " + ex.Message, certificate)
            retrieve_value = ""
        Finally
            otrace = New bc_cs_activity_log("bc_in_excel_submission_db", "retrieve_value", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function retrieve_static(ByVal contributor_id As Long, ByVal accounting_standard As Long, ByVal workflow_stage As Long, ByVal entity_id As Long, ByVal value As String, ByVal date_from As String, ByVal scale_factor As String, ByVal row_id As Long, ByVal dimension_id As Long, ByVal order As Long, ByVal comment As String, ByRef certificate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_in_excel_submission_db", "retrieve_static", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim sql As String = ""
            Dim ea_flag = 0
            Dim ovalue As Object
            Dim tcid As Long
            REM check contributor is numeric
            Try
                tcid = CInt(contributor_id)
            Catch ex As Exception
                Dim ocommentary = New bc_cs_activity_log("bc_in_excel_submission_db", "retrieve_value", bc_cs_activity_codes.COMMENTARY, "Contributor id for row:" + CStr(row_id) = " is not numeric", certificate)
                Return ""
            End Try
            REM get period id
            If dimension_id = 1 Then
                sql = "select coalesce(attrib_value,'') from entity_attribute_value_tbl where attribute_id=" + CStr(row_id) + " and workflow_stage=" + CStr(workflow_stage) + " and contributor_id=" + CStr(contributor_id) + " And acc_standard=" + CStr(accounting_standard) + " and entity_id=" + CStr(entity_id)
            ElseIf dimension_id = 2 Then
                sql = "select coalesce(value,'') from entity_attribute_values_time_series where attribute_id=" + CStr(row_id) + " and workflow_stage=" + CStr(workflow_stage) + " and contributor_id=" + CStr(contributor_id) + " And  acc_standard=" + CStr(accounting_standard) + " and entity_id=" + CStr(entity_id) + " and date_to='9-9-9999'"
            Else
                Dim ocommentary As New bc_cs_activity_log("bc_in_excel_submission_db", "retrieve_static", bc_cs_activity_codes.COMMENTARY, "Dimension: " + CStr(dimension_id) + " not supported for extraction")
            End If
            value = ""
            ovalue = gbc_db.executesql(sql, certificate)
            If IsArray(ovalue) Then
                If UBound(ovalue, 2) >= 0 Then
                    value = CStr(ovalue(0, 0))
                End If
            End If
            REM convert value
            If IsNumeric(value) Then
                If IsNumeric(scale_factor) Then
                    value = value / scale_factor
                End If
            End If
            retrieve_static = value

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_in_excel_submission_db", "retrieve_static", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            retrieve_static = ""
        Finally
            otrace = New bc_cs_activity_log("bc_in_excel_submission_db", "retrieve_static", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Sub insert_static_row(ByVal cid As Long, ByVal acc As Long, ByVal ws As Long, ByVal entity_Id As Long, ByVal value As String, ByVal date_from As String, ByVal scale_factor As String, ByVal attribute_id As Integer, ByVal dimension_id As Integer, ByVal order As Integer, ByVal comment As String, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_in_excel_submission_db", "insert_row", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim sql As String = ""
            Dim ocommentary As bc_cs_activity_log
            Dim ostring As bc_cs_string_services
            REM convert value
            Dim aa As New ArrayList

            If IsNumeric(value) Then
                If IsNumeric(scale_factor) Then
                    value = value * scale_factor
                End If
            Else
                ostring = New bc_cs_string_services(value)
                value = ostring.delimit_apostrophies
            End If
            ostring = New bc_cs_string_services(comment)
            comment = ostring.delimit_apostrophies
            REM deduce which table to update and update
            Select Case dimension_id
                Case 0
                    REM entity_period_tbl
                    ocommentary = New bc_cs_activity_log("bc_in_excel_submission_db", "insert_static_row", bc_cs_activity_codes.COMMENTARY, "Dimension Code not Valid for attribute_id: " + CStr(cid))
                Case 1
                    REM entity_attribute_value_tbl
                    sql = "delete from entity_attribute_value_tbl where attribute_id=" + CStr(attribute_id) + " and entity_id=" + CStr(entity_Id)
                    gbc_db.executesql(sql, certificate)
                    sql = "insert into entity_attribute_value_tbl(attribute_id, entity_id,attrib_value, contributor_id, acc_standard, workflow_stage, comment) values(" + CStr(attribute_id) + "," + CStr(entity_Id) + ",'" + value + "'," + CStr(cid) + "," + CStr(acc) + "," + CStr(ws) + ",'" + comment + "')"
                Case 2
                    REM entity_attribute_values_time_series
                    sql = "update entity_attribute_values_time_series set date_to='" + date_from + "' where entity_id=" + CStr(entity_Id) + " and attribute_id=" + CStr(attribute_id) + " and date_to='9-9-9999'"
                    gbc_db.executesql(sql, certificate)
                    sql = "insert into entity_attribute_values_time_series(entity_id, attribute_id,date_from,date_to,value, contributor_id, acc_standard, workflow_stage, comment) values(" + CStr(entity_Id) + "," + CStr(attribute_id) + ",'" + date_from + "','9-9-9999','" + value + "'," + CStr(cid) + "," + CStr(acc) + "," + CStr(ws) + ",'" + comment + "')"
                Case 3
                    REM entity_attribute_repeating_value_tbl
                    sql = "delete from entity_attribute_repeating_value_tbl where attribute_id=" + CStr(attribute_id) + " and entity_id=" + CStr(entity_Id) + " and attrib_order=" + CStr(order)
                    gbc_db.executesql(sql, certificate)
                    sql = "insert into entity_attribute_repeating_value_tbl(attribute_id, entity_id,attrib_value, attrib_order, contributor_id, acc_standard, workflow_stage, comment) values(" + CStr(attribute_id) + "," + CStr(entity_Id) + ",'" + value + "'," + CStr(order) + "," + CStr(cid) + "," + CStr(acc) + "," + CStr(ws) + ",'" + comment + "')"
                Case 4
                    REM entity_link_attribute_value_tbl 
                Case 5
                    REM entity_link_attribute_values_time_series_tbl 
            End Select
            gbc_db.executesql(sql, certificate)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_in_excel_submission_db", "insert_row", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_in_excel_submission_db", "insert_row", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    Public Sub set_workflow_stage(ByVal entity_Id As Long, ByVal stage_id As Integer, ByVal title As String, ByVal contributor_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_in_excel_submission_db", "set_workflow_stage", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim sql As String
            Dim vcont_id As Object
            Dim cont_id As Long
            Dim luserid As String
            Dim lusername As String
            If bc_cs_central_settings.server_flag = 0 Then
                luserid = bc_cs_central_settings.logged_on_user_id
                lusername = bc_cs_central_settings.logged_on_user_name
            Else
                luserid = certificate.user_id
                lusername = certificate.name
            End If

            REM see if a container exists for entity/contributor
            sql = "select cont_id from container_entity_tbl where entity_id=" + CStr(entity_Id) + " and contributor_id=" + CStr(contributor_id)
            vcont_id = gbc_db.executesql(sql, certificate)
            If IsArray(vcont_id) Then
                If UBound(vcont_id, 2) > -1 Then
                    cont_id = vcont_id(0, 0)
                Else
                    sql = "insert into container_tbl (stage_id, pub_type_id, bus_area_id, user_id, title, max_stage_no, create_date, target_date, priority, note) values(1,1000,1,2,'" + title + "',null,getdate(),getdate()," + CStr(luserid) + ",'')"
                    gbc_db.executesql(sql, certificate)
                    sql = "select max(container_id) from container_tbl"
                    vcont_id = gbc_db.executesql(sql, certificate)
                    If UBound(vcont_id, 2) > -1 Then
                        If IsNumeric(vcont_id(0, 0)) Then
                            cont_id = vcont_id(0, 0)
                        Else
                            cont_id = 1
                        End If
                    Else
                        cont_id = 1
                    End If
                    sql = "insert into container_entity_tbl (cont_id, contributor_id, entity_id) values(" + CStr(cont_id) + "," + CStr(contributor_id) + "," + CStr(entity_Id) + ")"
                    gbc_db.executesql(sql, certificate)
                End If
            End If
            REM assign stage
            sql = "update container_tbl set stage_id=" + CStr(stage_id) + " where container_id=" + CStr(cont_id)
            gbc_db.executesql(sql, certificate)
            sql = "insert into history_tbl (CONTAINER_ID, HISTORY_DATE, DESCRIPTION, HISTORY_COMMENT, HISTORY_CODE) values(" + CStr(cont_id) + ",getdate(),'Container Reset to Draft due to Insight Contributor by: " + bc_cs_central_settings.logged_on_user_name + "','','A')"
            gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_in_excel_submission_db", "set_workflow_stage", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_in_excel_submission_db", "set_workflow_stage", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    Public Function run_calculations(ByRef db As bc_cs_db_services, ByVal entity_Id As Long, ByVal contributor_id As Long, ByVal author_id As Long, ByVal audit_date As DateTime, ByVal audit_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_in_excel_submission_db", "run_calculations", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        run_calculations = ""

        Try
            Dim sql As String

            sql = "exec insight_calculate_entity " + CStr(entity_Id) + "," + CStr(contributor_id) + ",0,0,1,1,''," + CStr(author_id) + ",'" + Format(audit_date, "dd-MMM-yyyy HH:mm:ss") + "'," + CStr(audit_id)

            run_calculations = db.executesql_trans(sql, certificate, True)

        Catch ex As Exception
            db.success = False
            Dim db_err As New bc_cs_error_log("bc_in_excel_submission_db", "run_calculations", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_in_excel_submission_db", "run_calculations", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function retrieve_workbooks(ByVal mine As Boolean, ByVal checked_out_to_me As Boolean, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_in_excel_submission_db", "retrieve_all_workbooks", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim sql As String
            Dim luser As String
            If bc_cs_central_settings.server_flag = 0 Then
                luser = bc_cs_central_settings.logged_on_user_id
            Else
                luser = certificate.user_id
            End If

            If mine = False And checked_out_to_me = False Then
                sql = "select distinct workbook_name, master_entity_id, class_id, contributor_id,author_id,submission_date, " + _
                      " coalesce(w.user_id,0), schema_name s from bc_insight_submission_workbooks w, person_bus_area_tbl p, bus_area_schema_link b, schema_tbl s " + _
                      " where p.user_id = '" + CStr(luser) + "'" + _
                      " and b.schema_id = w.contributor_id" + _
                      " and p.bus_area_id = b.bus_area_id" + _
                      " and s.schema_id = b.schema_id" + _
                      " order by workbook_name asc"
            ElseIf mine = True And checked_out_to_me = False Then
                sql = "select distinct workbook_name, master_entity_id, class_id, contributor_id,author_id,submission_date, " + _
                      " coalesce(w.user_id,0), schema_name s from bc_insight_submission_workbooks w, person_bus_area_tbl p, bus_area_schema_link b,apref_entity_tbl a, schema_tbl s " + _
                      " where p.user_id = '" + CStr(luser) + "'" + _
                      " and b.schema_id =w.contributor_id " + _
                      " and p.bus_area_id = b.bus_area_id " + _
                      " and a.user_id = p.user_id " + _
                      " and w.master_entity_id = a.entity_id " + _
                      " and s.schema_id = b.schema_id" + _
                      " order by workbook_name asc"

            Else
                REM checked out to me
                sql = "select distinct workbook_name, master_entity_id, class_id, contributor_id,author_id,submission_date, " + _
                    " coalesce(w.user_id,0), schema_name s from bc_insight_submission_workbooks w, person_bus_area_tbl p, bus_area_schema_link b, schema_tbl s " + _
                    " where p.user_id = '" + CStr(luser) + "'" + _
                    " and b.schema_id = w.contributor_id" + _
                    " and p.bus_area_id = b.bus_area_id" + _
                    " and s.schema_id = b.schema_id" + _
                    " and w.user_id = " + CStr(luser) + _
                    " order by workbook_name asc"
            End If
            retrieve_workbooks = gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_in_excel_submission_db", "retrieve_all_workbooks", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            retrieve_workbooks = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_in_excel_submission_db", "retrieve_all_workbooks", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    Public Function get_intermediate_sheets(ByVal lead_entity_id As Long, ByVal schema_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_in_excel_submission_db", "get_intermediate_sheets", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim sql As String
            sql = "exec dbo.bcc_core_get_int_sheets '" + CStr(lead_entity_id) + "','" + CStr(schema_id) + "'"
            get_intermediate_sheets = gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_in_excel_submission_db", "get_intermediate_sheets", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            get_intermediate_sheets = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_in_excel_submission_db", "get_intermediate_sheets", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function get_all_intermediate_sheets(ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_in_excel_submission_db", "get_all_intermediate_sheets", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim sql As String
            sql = "select template_id, filename,name from bcc_core_interm_sheets order by template_id asc"
            get_all_intermediate_sheets = gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_in_excel_submission_db", "get_all_intermediate_sheets", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            get_all_intermediate_sheets = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_in_excel_submission_db", "get_all_intermediate_sheets", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Sub update_intermediate_sheet(ByVal id As Long, ByVal name As String, ByVal filename As String, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_in_excel_submission_db", "get_all_intermediate_sheets", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim sql As String
            sql = "delete from bcc_core_interm_sheets  where template_id=" + CStr(id)
            gbc_db.executesql(sql, certificate)
            If name <> "DELETE" Then
                If id > 0 Then
                    sql = "insert into bcc_core_interm_sheets (template_id, ord, name, filename) values(" + CStr(id) + ",1,'" + CStr(name) + "','" + CStr(filename) + "')"
                    gbc_db.executesql(sql, certificate)
                Else
                    sql = "delete from bcc_core_interm_sheets  where template_id=(select max(template_id) + 1 from template_tbl)"
                    gbc_db.executesql(sql, certificate)
                    sql = "insert into bcc_core_interm_sheets (template_id, ord, name, filename) select max(template_id) + 1 ,1,'" + CStr(name) + "','" + CStr(filename) + "' from template_tbl"
                    gbc_db.executesql(sql, certificate)
                End If

            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_in_excel_submission_db", "get_all_intermediate_sheets", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_in_excel_submission_db", "get_all_intermediate_sheets", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Sub check_out_workbook(ByVal entity_id As Long, ByVal contributor_id As Long, ByVal logged_on_user_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "update bc_insight_submission_workbooks set user_id=" + CStr(logged_on_user_id) + "  where master_entity_id=" + CStr(entity_id) + " and contributor_id=" + CStr(contributor_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function read_items(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_read_rows_for_calcs"
        'sql = "select row_id, description, coalesce(c.currency_type,1) from row_tbl r left outer join bc_core_item_currency c on c.item_id=r.row_id order by description asc"
        read_items = gbc_db.executesql(sql, certificate)
    End Function
    REM FIL 5.3
    Public Function read_rows(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_read_rows"
        REM sql = "select row_id, description, coalesce(default_scale_factor,1), coalesce(default_scale_symbol,''),coalesce(monetary,0),coalesce(price_related,0) from row_tbl order by description asc"
        read_rows = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_attributes(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_read_attributes"
        REM sql = "select attribute_id, label_value, a.label_code,coalesce(monetary,0),coalesce(price_related,0)  from attribute_tbl a, label_value_tbl l where  a.label_code = l.label_code and language_code='en' and country_code='GB' order by label_value asc"
        read_attributes = gbc_db.executesql(sql, certificate)
    End Function
    REM ====
    Public Sub delete_sub_security(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from bcc_core_financial_wf_role"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub insert_sub_security(ByVal role_name As String, ByVal stage_name As String, ByVal default_stage As Boolean, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim ostage As Object
        Dim orole As Object
        Dim stage_id As Long
        Dim role_id As Long
        sql = "select stage_id from stage_tbl where stage_name='" + CStr(stage_name) + "'"
        ostage = gbc_db.executesql(sql, certificate)
        If IsArray(ostage) Then
            stage_id = ostage(0, 0)
        End If
        sql = "select role_id from role_tbl where role_name='" + CStr(role_name) + "'"
        orole = gbc_db.executesql(sql, certificate)
        If IsArray(ostage) Then
            role_id = orole(0, 0)
        End If

        sql = "insert into bcc_core_financial_wf_role (role_id, stage_id, default_flag) values(" + CStr(role_id) + "," + CStr(stage_id) + ",'" + CStr(default_stage) + "')"
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Function read_insight_security(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select r.role_id,role_name, s.stage_name, b.stage_id, default_flag from bcc_core_financial_wf_role b inner join role_tbl r on r.role_id=b.role_id inner join stage_tbl s on s.stage_id=b.stage_id order by role_name, s.stage_id asc"
        read_insight_security = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_contexts_for_template(ByVal template_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select distinct(c.context_id),description from context_tbl c inner join row_context_tbl r on r.context_id=c.context_id and r.template_id=" + CStr(template_id) + " and c.context_id >=4 order by description asc"
        get_contexts_for_template = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_sections_for_template(ByVal template_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select description from template_section_tbl t inner join section_tbl s on s.section_id=t.section_id where template_id = " + CStr(template_id) + " order by display_order asc"
        get_sections_for_template = gbc_db.executesql(sql, certificate)
    End Function

    Public Function add_porfolio(ByVal user_id As Long, ByVal public_flag As Integer, ByVal title As String, ByVal function_name As String, ByVal class_name As String, ByVal schema_name As String, ByVal universe_flag As Integer, ByVal entity_set_name As String, ByVal entity_prop_class_name As String, ByVal entity_prop_entity_name As String, ByVal stage As String, ByVal contributor As String, ByVal date_at As Date, ByVal start_year As String, ByVal end_year As String, ByVal convert_type As Integer, ByVal currency As String, ByVal show_e_a As Integer, ByVal format_type As Long, ByVal precision As Long, ByVal date_from As Date, ByVal table_title As String, ByVal sub_title As String, ByVal source As String, ByVal associated_class As String, ByVal sec_entity_prop_class_name As String, ByVal sec_entity_prop_entity_name As String, ByVal dual_type As Integer, ByVal dual_entity As String, ByVal exch_type As String, ByRef certificate As bc_cs_security.certificate) As Long
        Dim sql As String
        Dim vres As Object
        Dim fs As bc_cs_string_services
        fs = New bc_cs_string_services(title)
        title = fs.delimit_apostrophies
        fs = New bc_cs_string_services(entity_set_name)
        entity_set_name = fs.delimit_apostrophies
        fs = New bc_cs_string_services(class_name)
        class_name = fs.delimit_apostrophies
        fs = New bc_cs_string_services(table_title)
        table_title = fs.delimit_apostrophies
        fs = New bc_cs_string_services(sub_title)
        sub_title = fs.delimit_apostrophies
        fs = New bc_cs_string_services(source)
        source = fs.delimit_apostrophies
        fs = New bc_cs_string_services(associated_class)
        associated_class = fs.delimit_apostrophies
        fs = New bc_cs_string_services(sec_entity_prop_class_name)
        sec_entity_prop_class_name = fs.delimit_apostrophies
        fs = New bc_cs_string_services(entity_prop_class_name)
        entity_prop_class_name = fs.delimit_apostrophies
        fs = New bc_cs_string_services(sec_entity_prop_entity_name)
        sec_entity_prop_entity_name = fs.delimit_apostrophies
        fs = New bc_cs_string_services(entity_prop_entity_name)
        entity_prop_entity_name = fs.delimit_apostrophies
        fs = New bc_cs_string_services(dual_entity)
        dual_entity = fs.delimit_apostrophies

        sql = "insert into dbo.bc_core_pt_portfolios(portfolio_id,user_id, public_flag,create_date,title,function_name,class,[schema],universe_flag,entity_set_name,entity_prop_class_name,entity_prop_entity_name,stage,contributor,date_at,start_year,end_year,convert_type,currency,show_e_a,format_type,precision,date_from,table_title,sub_title,source,associated_class,sec_entity_prop_class_name,sec_entity_prop_entity_name,data_type,dual_class,exch_type) select(coalesce(max(portfolio_id) ,0) + 1)," + CStr(user_id) + "," + CStr(public_flag) + ",getdate(),'" + title + "','" + function_name + "','" + class_name + "','" + schema_name + "'," + CStr(universe_flag) + ",'" + entity_set_name + "','" + entity_prop_class_name + "','" + entity_prop_entity_name + "','" + stage + "','" + contributor + "','" + CStr(date_at) + "','" + start_year + "','" + end_year + "'," + CStr(convert_type) + ",'" + currency + "'," + CStr(show_e_a) + "," + CStr(format_type) + "," + CStr(precision) + ",'" + CStr(date_from) + "','" + table_title + "','" + sub_title + "','" + source + "','" + associated_class + "','" + sec_entity_prop_class_name + "','" + sec_entity_prop_entity_name + "'," + CStr(dual_type) + ",'" + CStr(dual_entity) + "','" + exch_type + "' from bc_core_pt_portfolios"
        gbc_db.executesql(sql, certificate)
        sql = "select max(portfolio_id) from dbo.bc_core_pt_portfolios"
        vres = gbc_db.executesql(sql, certificate)
        If IsArray(vres) Then
            add_porfolio = vres(0, 0)
        End If
    End Function
    Public Function get_monetary_type(ByVal row_id As Long, ByVal static_flag As Boolean, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select currency_type from bc_core_item_currency where item_id=" + CStr(row_id)
        get_monetary_type = gbc_db.executesql(sql, certificate)
    End Function
    'Public Function is_item_price_related(ByVal row_id As Long, ByVal static_flag As Boolean, ByRef certificate As bc_cs_security.certificate)
    '    Dim sql As String
    '    If static_flag = False Then
    '        sql = "select coalesce(price_related,0) from row_tbl where row_id=" + CStr(row_id)
    '    Else
    '        sql = "select coalesce(price_related,0) from attribute_tbl where attribute_id=" + CStr(row_id)
    '    End If
    '    is_item_price_related = gbc_db.executesql(sql, certificate)
    'End Function
    Public Sub add_portfolio_entities(ByVal portfolio_id As Long, ByVal entity As String, ByVal ord As String, ByVal dual_entity As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim fs As New bc_cs_string_services(entity)
        entity = fs.delimit_apostrophies
        fs = New bc_cs_string_services(dual_entity)
        dual_entity = fs.delimit_apostrophies
        sql = "insert into dbo.bc_core_pt_portfolio_entities (portfolio_id, entity, ord, dual_entity) values(" + CStr(portfolio_id) + ",'" + CStr(entity) + "'," + CStr(ord) + ",'" + dual_entity + "')"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub add_portfolio_periods(ByVal portfolio_id As Long, ByVal period As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "insert into dbo.bc_core_pt_portfolio_periods (portfolio_id, period) values(" + CStr(portfolio_id) + ",'" + CStr(period) + "')"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub add_portfolio_items(ByVal portfolio_id As Long, ByVal item As String, ByVal ord As Integer, ByVal type As String, ByVal factor As String, ByVal monatary As Integer, ByVal symbol As String, ByVal iclass As String, ByRef certificate As bc_cs_security.certificate)
        Try
            Dim sql As String
            Dim fs As New bc_cs_string_services(item)
            item = fs.delimit_apostrophies
            fs = New bc_cs_string_services(symbol)
            symbol = fs.delimit_apostrophies
            fs = New bc_cs_string_services(iclass)
            iclass = fs.delimit_apostrophies

            sql = "insert into dbo.bc_core_pt_portfolio_items (portfolio_id, item, [order], type, factor, monatary, symbol, class) values(" + CStr(portfolio_id) + ",'" + CStr(item) + "'," + CStr(ord) + ",'" + type + "','" + factor + "'," + CStr(monatary) + ",'" + symbol + "','" + CStr(iclass) + "')"
            gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_in_excel_submission_db", "add_potfolio_items", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        End Try
    End Sub
    Public Function get_user_defined_portfolios(ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String

        sql = "select portfolio_id,p.user_id, public_flag, create_date, p.title, function_name,class,[schema],universe_flag,entity_set_name, entity_prop_class_name,entity_prop_entity_name,stage,contributor,date_at,start_year,end_year,convert_type,currency, coalesce(first_name + ' ' + surname,'unknown'),show_e_a, format_type,precision, date_from, table_title, sub_title, source, associated_class,coalesce(sec_entity_prop_class_name,'')  ,coalesce(sec_entity_prop_entity_name,''), coalesce(dual_class,''), coalesce(data_type,0), coalesce(exch_type,'Current')  from dbo.bc_core_pt_portfolios p left outer join user_table u on u.user_id=p.user_id where (p.user_id=" + CStr(user_id) + "  and public_flag=0) or public_flag =1 and coalesce(predefined,0) = 0 order by title asc, create_date desc"
        get_user_defined_portfolios = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_predefined_portfolios(ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String

        sql = "exec dbo.bc_core_get_pd_extracts " + CStr(user_id)
        'sql = "select  portfolio_id, title, function_name,class,[schema],universe_flag, entity_set_name,entity_prop_class_name, entity_prop_entity_name,stage,contributor,date_at,start_year,end_year,convert_type,currency,format_type,[precision], date_from, associated_class,sec_entity_prop_class_name, sec_entity_prop_entity_name,coalesce(format_file,''),coalesce(sp_name,''),coalesce(num_entities_select,0),coalesce(num_item_select,0), enable_stage,enable_contributor,enable_years_periods,enable_date_at,enable_date_range,coalesce(item_set,''), coalesce(enable_items,0),coalesce(show_date_type,0),coalesce(show_year_end_date,0), coalesce(enable_currency,0), coalesce(folder_name, 'General'), coalesce(sub_folder_name, '')  from dbo.bc_core_pt_portfolios where coalesce(predefined,0)=1 order by coalesce(folder_name, 'General') asc, coalesce(sub_folder_name, '') asc, title asc"
        get_predefined_portfolios = gbc_db.executesql(sql, certificate)
    End Function

    Public Sub delete_portfolio(ByVal portfolio_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from bc_core_pt_portfolio_items where portfolio_id=" + CStr(portfolio_id)
        gbc_db.executesql(sql, certificate)
        sql = "delete from bc_core_pt_portfolio_entities where portfolio_id=" + CStr(portfolio_id)
        gbc_db.executesql(sql, certificate)
        sql = "delete from bc_core_pt_portfolio_periods where portfolio_id=" + CStr(portfolio_id)
        gbc_db.executesql(sql, certificate)
        sql = "delete from bc_core_pt_portfolios  where portfolio_id=" + CStr(portfolio_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function get_entities_for_portfolio(ByVal portfolio_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        REM FIL MAY 2013
        sql = "exec dbo.bc_core_get_entities_for_portfolio " + CStr(portfolio_id)
        'sql = "select  entity,dual_entity from bc_core_pt_portfolio_entities where portfolio_id=" + CStr(portfolio_id) + " order by ord asc"
        get_entities_for_portfolio = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_periods_for_portfolio(ByVal portfolio_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select period from bc_core_pt_portfolio_periods where portfolio_id=" + CStr(portfolio_id) + " order by period asc"
        get_periods_for_portfolio = gbc_db.executesql(sql, certificate)
    End Function

    Public Function get_param1_list_for_portfolio(ByVal portfolio_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select item, list_name,[default] from bc_core_pt_param_items  where portfolio_id=" + CStr(portfolio_id) + " and list_id=1 order by [default] desc, item asc"
        get_param1_list_for_portfolio = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_items_for_portfolio(ByVal portfolio_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select item, type,factor,monatary,symbol,class from bc_core_pt_portfolio_items where portfolio_id=" + CStr(portfolio_id) + " order by [order] asc"
        get_items_for_portfolio = gbc_db.executesql(sql, certificate)
    End Function


    Public Sub delete_entity_set(ByVal set_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from dbo.bc_core_pt_entity_set where set_id=" + CStr(set_id)
        gbc_db.executesql(sql, certificate)
        sql = "delete from dbo.bc_core_pt_user_entity_set where set_id=" + CStr(set_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub delete_item_set(ByVal set_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from dbo.bc_core_pt_item_set where set_id=" + CStr(set_id)
        gbc_db.executesql(sql, certificate)
        sql = "delete from dbo.bc_core_pt_user_item_set where set_id=" + CStr(set_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function read_portfolios_for_user(ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select b.title from bc_core_pt_portfolios b inner join user_table u on u.user_id=b.user_id and u.user_id=" + CStr(user_id) + " and public_flag = 0 union select d.title from bc_core_pt_portfolios d where public_flag = 1 and coalesce(predefined,0)=0"
        read_portfolios_for_user = gbc_db.executesql(sql, certificate)
    End Function
    REM FIL JULY 2012
    Public Function load_pref_types(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select preference_type_id ,preference_type_name from bc_core_cp_preference_types order by default_type desc, preference_type_name asc"
        load_pref_types = gbc_db.executesql(sql, certificate)
    End Function
    Public Function add_entity_set(ByVal user_id As Long, ByVal name As String, ByVal class_name As String, ByRef certificate As bc_cs_security.certificate) As Long
        Dim sql As String
        Dim vres As Object
        add_entity_set = 0
        Dim fs As bc_cs_string_services
        fs = New bc_cs_string_services(name)
        name = fs.delimit_apostrophies
        fs = New bc_cs_string_services(class_name)
        class_name = fs.delimit_apostrophies
        sql = "insert into dbo.bc_core_pt_user_entity_set (set_id, user_id, class, name) select(coalesce(max(set_id),0)+1)," + CStr(user_id) + ",'" + class_name + "','" + name + "' from bc_core_pt_user_entity_set"
        gbc_db.executesql(sql, certificate)
        sql = "select max(set_id) from dbo.bc_core_pt_user_entity_set"
        vres = gbc_db.executesql(sql, certificate)
        If IsArray(vres) Then
            add_entity_set = vres(0, 0)
        End If
    End Function
    Public Function add_item_set(ByVal user_id As Long, ByVal name As String, ByVal class_name As String, ByVal dual_class As String, ByRef certificate As bc_cs_security.certificate) As Long
        Dim sql As String
        Dim vres As Object
        add_item_set = 0
        Dim fs As bc_cs_string_services
        fs = New bc_cs_string_services(name)
        name = fs.delimit_apostrophies
        fs = New bc_cs_string_services(class_name)
        class_name = fs.delimit_apostrophies
        sql = "insert into dbo.bc_core_pt_user_item_set (set_id, user_id, class, name,dual_class) select(coalesce(max(set_id),0)+1)," + CStr(user_id) + ",'" + class_name + "','" + name + "','" + dual_class + "' from bc_core_pt_user_item_set"
        gbc_db.executesql(sql, certificate)
        sql = "select max(set_id) from dbo.bc_core_pt_user_item_set"
        vres = gbc_db.executesql(sql, certificate)
        If IsArray(vres) Then
            add_item_set = vres(0, 0)
        End If
    End Function
    Public Sub add_entity_to_set(ByVal set_id As Long, ByVal entity As String, ByVal ord As Integer, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim fs As bc_cs_string_services
        fs = New bc_cs_string_services(entity)
        entity = fs.delimit_apostrophies
        sql = "insert into dbo.bc_core_pt_entity_set (set_id, entity, ord) values(" + CStr(set_id) + ",'" + entity + "'," + CStr(ord) + ")"
        gbc_db.executesql(sql, certificate)
    End Sub
    REM FIL JIRA 6835
    Public Function get_target_class_id(ByVal intAggregationId As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select t.attrib_value from entity_attribute_value_tbl t " + _
              "inner join attribute_tbl a on a.name='Target Class' Or  a.name='Dual Target Class' " + _
              "where(t.entity_id = " + CStr(intAggregationId) + " And a.attribute_id = t.attribute_id) order by a.attribute_id asc"
        get_target_class_id = gbc_db.executesql(sql, certificate)
    End Function
    REM FIL JIRA 6835
    Public Function read_all_agg_results_rows(ByVal target_class_id As Long, ByVal dual_target_class_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_read_all_agg_results_rows " + CStr(target_class_id) + "," + CStr(dual_target_class_id)
        read_all_agg_results_rows = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub add_item_to_set(ByVal set_id As Long, ByVal item As String, ByVal type As String, ByVal factor As String, ByVal monatary As Integer, ByVal symbol As String, ByVal iclass As String, ByVal ord As Integer, ByVal dual_class As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim fs As bc_cs_string_services
        fs = New bc_cs_string_services(item)
        item = fs.delimit_apostrophies
        fs = New bc_cs_string_services(iclass)
        iclass = fs.delimit_apostrophies
        sql = "insert into dbo.bc_core_pt_item_set (set_id, item, type, factor, monatary, symbol, class, ord,dual_class) values(" + CStr(set_id) + ",'" + item + "','" + type + "','" + factor + "'," + CStr(monatary) + ",'" + symbol + "','" + iclass + "'," + CStr(ord) + ",'" + dual_class + "')"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function get_pt_user_settings(ByVal logged_on_user_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select stages,def_stage, global from bc_core_pt_role_security s inner join user_table u on u.role_id=s.role_id and u.user_id=" + CStr(logged_on_user_id)
        get_pt_user_settings = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub create_custom_items(ByVal template_id As Long, ByVal entity_id As Long, ByVal label As String, ByVal static_flag As String, ByVal data_type As Integer, ByVal scale_symbol As String, ByVal scale_factor As String, ByVal link_code As String, ByVal submission_code As Integer, ByVal repeating_count As Integer, ByVal flexible_label_flag As String, ByVal ord As Integer, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_in_excel_submission_db", "create_custom_items", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim sql As String
            Dim res As Object
            Dim label_code As String
            Dim id As Long
            Dim ocommentary As bc_cs_activity_log
            Dim ostr As New bc_cs_string_services(label)
            label = ostr.delimit_apostrophies
            ostr = New bc_cs_string_services(scale_symbol)
            scale_symbol = ostr.delimit_apostrophies
            REM get next ID
            sql = "select max(row_id) from row_tbl"
            res = gbc_db.executesql(sql, certificate)
            If UBound(res, 2) > -1 Then
                id = res(0, 0) + 1
            End If
            sql = "select max(attribute_id) from attribute_tbl"
            res = gbc_db.executesql(sql, certificate)
            If UBound(res, 2) > -1 Then
                If res(0, 0) + 1 > id Then
                    id = res(0, 0) + 1
                End If
            End If
            REM first assign the label code
            ocommentary = New bc_cs_activity_log("bc_in_excel_submission_db", "create_custom_items", bc_cs_activity_codes.COMMENTARY, "Checking for exisitance of label: " + label)
            sql = "select label_code from label_value_tbl where label_value = '" + label + "'"
            res = gbc_db.executesql(sql, certificate)
            If UBound(res, 2) > -1 Then
                label_code = res(0, 0)
                ocommentary = New bc_cs_activity_log("bc_in_excel_submission_db", "create_custom_items", bc_cs_activity_codes.COMMENTARY, "Label: " + label + " exists with code: " + CStr(label_code))
            Else
                label_code = Format(Now, "ddMMyyhhmmss")
                label_code = label_code + CStr(id)

                REM if no label code for label generate new label
                If static_flag = 0 Then
                    label_code = "fd.rows.custom" + label_code
                Else
                    label_code = "attribute.custom" + label_code
                End If
                REM see of this code exists
                REM insert label code
                ocommentary = New bc_cs_activity_log("bc_in_excel_submission_db", "create_custom_items", bc_cs_activity_codes.COMMENTARY, "Creating albe code records for label_code: " + CStr(label_code))
                sql = "insert into label_tbl(label_code, label_desc) values('" + label_code + "','" + label + "')"
                gbc_db.executesql(sql, certificate)
                sql = "insert into label_value_tbl(label_code, language_code, country_code,label_value) values('" + label_code + "','en','GB','" + label + "')"
                gbc_db.executesql(sql, certificate)
            End If
            ocommentary = New bc_cs_activity_log("bc_in_excel_submission_db", "create_custom_items", bc_cs_activity_codes.COMMENTARY, "Checking for row_id for label code: " + CStr(label_code))
            sql = "select row_id from row_tbl where default_label_code='" + label_code + "'"
            res = gbc_db.executesql(sql, certificate)
            If UBound(res, 2) = -1 Then
                ocommentary = New bc_cs_activity_log("bc_in_excel_submission_db", "create_custom_items", bc_cs_activity_codes.COMMENTARY, "Checking for attribute_id for label code: " + CStr(label_code))
                sql = "select attribute_id from attribute_tbl where label_code='" + label_code + "'"
                res = gbc_db.executesql(sql, certificate)
                If UBound(res, 2) > -1 Then
                    id = res(0, 0)
                    ocommentary = New bc_cs_activity_log("bc_in_excel_submission_db", "create_custom_items", bc_cs_activity_codes.COMMENTARY, "Attribute id: " + CStr(id) + " found for label code: " + CStr(label_code))
                Else
                    ocommentary = New bc_cs_activity_log("bc_in_excel_submission_db", "create_custom_items", bc_cs_activity_codes.COMMENTARY, "No attribute or row id found so using new ID: " + CStr(id))
                    If static_flag = False Then
                        REM insert row
                        If scale_factor = "" Or Not IsNumeric(scale_factor) Then
                            scale_factor = 1
                        End If
                        sql = "insert into row_tbl(row_id, description, default_scale_factor, default_scale_symbol,default_label_code) values(" + CStr(id) + ",'" + label + "','" + scale_factor + "','" + scale_symbol + "','" + label_code + "')"
                        gbc_db.executesql(sql, certificate)
                    Else
                        REM insert attribute
                        sql = "insert into attribute_tbl(attribute_id, type_id, is_lookup,label_code, name, nullable) values(" + CStr(id) + ",1,0,'" + label_code + "','" + label + "',1)"
                        gbc_db.executesql(sql, certificate)
                    End If
                End If
            Else
                id = res(0, 0)
                ocommentary = New bc_cs_activity_log("bc_in_excel_submission_db", "create_custom_items", bc_cs_activity_codes.COMMENTARY, "Row id: " + CStr(id) + " found for label code: " + CStr(label_code))
            End If
            REM insert insight attributes for template/entity
            REM label code 1 
            sql = "insert into  row_context_entity_tbl(row_id, context_id, insight_attribute_id,value, template_id,entity_id,static_flag) values(" + CStr(id) + ",1,1,'" + label_code + "'," + CStr(template_id) + "," + CStr(entity_id) + "," + CStr(static_flag) + ")"
            gbc_db.executesql(sql, certificate)
            REM scale symbol 2
            sql = "insert into  row_context_entity_tbl(row_id, context_id, insight_attribute_id,value, template_id,entity_id,static_flag) values(" + CStr(id) + ",1,2,'" + scale_symbol + "'," + CStr(template_id) + "," + CStr(entity_id) + "," + CStr(static_flag) + ")"
            gbc_db.executesql(sql, certificate)
            REM scale factor 3
            sql = "insert into  row_context_entity_tbl(row_id, context_id, insight_attribute_id,value, template_id,entity_id,static_flag) values(" + CStr(id) + ",1,3,'" + scale_factor + "'," + CStr(template_id) + "," + CStr(entity_id) + "," + CStr(static_flag) + ")"
            gbc_db.executesql(sql, certificate)
            REM order 4
            sql = "insert into  row_context_entity_tbl(row_id, context_id, insight_attribute_id,value, template_id,entity_id,static_flag) values(" + CStr(id) + ",1,4,'" + CStr(ord) + "'," + CStr(template_id) + "," + CStr(entity_id) + "," + CStr(static_flag) + ")"
            gbc_db.executesql(sql, certificate)
            REM flexible label flag 18
            sql = "insert into  row_context_entity_tbl(row_id, context_id, insight_attribute_id,value, template_id,entity_id,static_flag) values(" + CStr(id) + ",1,18,'" + CStr(flexible_label_flag) + "'," + CStr(template_id) + "," + CStr(entity_id) + "," + CStr(static_flag) + ")"
            gbc_db.executesql(sql, certificate)
            REM link code 19
            sql = "insert into  row_context_entity_tbl(row_id, context_id, insight_attribute_id,value, template_id,entity_id,static_flag) values(" + CStr(id) + ",1,19,'" + CStr(link_code) + "'," + CStr(template_id) + "," + CStr(entity_id) + "," + CStr(static_flag) + ")"
            gbc_db.executesql(sql, certificate)
            REM data type 20
            sql = "insert into  row_context_entity_tbl(row_id, context_id, insight_attribute_id,value, template_id,entity_id,static_flag) values(" + CStr(id) + ",1,20,'" + CStr(data_type) + "'," + CStr(template_id) + "," + CStr(entity_id) + "," + CStr(static_flag) + ")"
            gbc_db.executesql(sql, certificate)
            REM submission code 21
            sql = "insert into  row_context_entity_tbl(row_id, context_id, insight_attribute_id,value, template_id,entity_id,static_flag) values(" + CStr(id) + ",1,21,'" + CStr(submission_code) + "'," + CStr(template_id) + "," + CStr(entity_id) + "," + CStr(static_flag) + ")"
            gbc_db.executesql(sql, certificate)
            REM repeating count 22
            sql = "insert into  row_context_entity_tbl(row_id, context_id, insight_attribute_id,value, template_id,entity_id,static_flag) values(" + CStr(id) + ",1,22,'" + CStr(repeating_count) + "'," + CStr(template_id) + "," + CStr(entity_id) + "," + CStr(static_flag) + ")"
            gbc_db.executesql(sql, certificate)
            REM lookup sql
            sql = "insert into  row_context_entity_tbl(row_id, context_id, insight_attribute_id,value, template_id,entity_id,static_flag) values(" + CStr(id) + ",1,24,''," + CStr(template_id) + "," + CStr(entity_id) + "," + CStr(static_flag) + ")"
            gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_in_excel_submission_db", "create_custom_items", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_in_excel_submission_db", "create_custom_items", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    Public Sub reset_custom_items(ByVal entity_id As Long, ByVal template_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from row_context_entity_tbl where entity_id=" + CStr(entity_id) + " and template_id=" + CStr(template_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function get_label_for_flexible_code(ByVal entity_id As Long, ByVal row_id As Long, ByVal ord As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("bc_in_excel_submission_db", "get_label_for_flexible_code", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim sql As String
        Try
            sql = "select label_value from entity_custom_financials_tbl, label_value_tbl where label_code=value and entity_id=" + CStr(entity_id) + " and row_id=" + CStr(row_id) + " and context_id=1 and insight_attribute_id=17 and ord=" + CStr(ord) + " and language_code='en' and country_code ='GB'"
            get_label_for_flexible_code = gbc_db.executesql(sql, certificate)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_in_excel_submission_db", "get_label_for_flexible_code", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            get_label_for_flexible_code = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_in_excel_submission_db", "get_label_for_flexible_code", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Sub insert_flexible_label(ByVal label As String, ByVal row_id As Long, ByVal entity_id As Long, ByVal contributor_id As Long, ByVal ord As Long, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_in_excel_submission_db", "insert_flexible_label", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim res As Object
        Try
            Dim sql As String
            Dim label_code As String
            Dim ostr As New bc_cs_string_services(label)
            label = ostr.delimit_apostrophies
            sql = "select label_code from label_tbl where label_desc='" + label + "'"
            res = gbc_db.executesql(sql, certificate)
            If UBound(res, 2) > -1 Then
                label_code = res(0, 0)
            Else
                label_code = "fd.rows." + Format(Now, "ddMMyyhhmmss")
                label_code = label_code + CStr(row_id) + CStr(ord)
                REM new label code
                REM see if label code exists
                sql = "insert into label_tbl(label_code,label_desc) values('" + label_code + "','" + label + "')"
                gbc_db.executesql(sql, certificate)
                sql = "insert into label_value_tbl(label_code,language_code, country_code,label_value) values('" + label_code + "','en','GB','" + label + "')"
                gbc_db.executesql(sql, certificate)
            End If
            sql = "delete from entity_custom_financials_tbl where entity_id=" + CStr(entity_id) + " and row_id=" + CStr(row_id) + " and context_id=1 and insight_attribute_id=17 and ord =" + CStr(ord)
            gbc_db.executesql(sql, certificate)
            sql = "insert into entity_custom_financials_tbl(entity_id,row_id, context_id, insight_attribute_id, value, contributor_id, ord) values(" + CStr(entity_id) + "," + CStr(row_id) + ",1,17,'" + label_code + "'," + CStr(contributor_id) + "," + CStr(ord) + ")"
            gbc_db.executesql(sql, certificate)


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_in_excel_submission_db", "insert_flexible_label", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_in_excel_submission_db", "insert_flexible_label", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Function get_link_code(ByVal entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select attrib_value from  entity_attribute_value_tbl e, attribute_tbl a where e.attribute_id=a.attribute_id and a.label_code='attribute.share.linkorder' And entity_id = " + CStr(entity_id)
        get_link_code = gbc_db.executesql(sql, certificate)
    End Function
    Public Function workbook_cannot_be_submitted(ByVal entity_id As Long, ByVal contributor_id As Long, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        Dim res As Object
        Dim luser As String
        If bc_cs_central_settings.server_flag = 0 Then
            luser = bc_cs_central_settings.logged_on_user_id
        Else
            luser = certificate.user_id
        End If

        workbook_cannot_be_submitted = ""
        sql = "select first_name + ' ' + surname, u.user_id from user_table u, bc_insight_submission_workbooks  w where u.user_id=w.user_id and w.user_id <> 0 and master_entity_id=" + CStr(entity_id) + " and contributor_id=" + CStr(contributor_id)
        res = gbc_db.executesql(sql, certificate)
        If Not IsNothing(res) Then
            If UBound(res, 2) > -1 Then
                If CStr(res(1, 0)) <> CStr(luser) Then
                    workbook_cannot_be_submitted = "Workbook Checked out to: " + CStr(res(0, 0)) + " cannot be submitted!"
                    Exit Function
                End If
            Else
                sql = "select user_id from  bc_insight_submission_workbooks  where master_entity_id=" + CStr(entity_id) + " and contributor_id=" + CStr(contributor_id)
                res = gbc_db.executesql(sql, certificate)
                If Not IsNothing(res) Then
                    If UBound(res, 2) > -1 Then
                        If res(1, 0) = 0 Then
                            workbook_cannot_be_submitted = "Workbook is already Checked in cannot be submitted!"
                            Exit Function
                        End If
                    End If
                End If
            End If
        End If
    End Function
    Public Function get_lookup_validations_for_row(ByVal sql As String, ByRef certificate As bc_cs_security.certificate)
        get_lookup_validations_for_row = gbc_db.executesql_show_no_error(sql)
    End Function
    Public Function write_data_via_xml(ByVal xmltext As String, ByVal contributor_id As Long, ByVal workflow_stage As Long, ByVal accounting_standard As Long, ByVal entity_id As Long, ByVal date_from As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec insight_write_data '" + xmltext + "'," + CStr(contributor_id) + "," + CStr(workflow_stage) + "," + CStr(accounting_standard) + "," + CStr(entity_id) + ",'" + CStr(date_from) + "'"
        write_data_via_xml = gbc_db.executesql(sql, certificate)
    End Function
    REM FIL FEB 2013
    Public Function audit_submission(ByRef db As bc_cs_db_services, ByVal audit_date As Date, ByVal audit_Id As Long, ByVal author_id As Long, ByVal lead_entity_id As Long, ByVal workflow_stage As Long, ByVal contributor_id As Long, ByVal surrogate_author_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_in_submission_audit_write  " + CStr(author_id) + "," + CStr(lead_entity_id) + "," + CStr(workflow_stage) + "," + CStr(contributor_id) + "," + CStr(surrogate_author_id) + ",'" + Format(audit_date, "dd-MMM-yyyy HH:mm:ss") + "'," + CStr(audit_Id)
        audit_submission = db.executesql_trans(sql, certificate)
    End Function


    Public Function write_data_via_xml_trans(ByRef db As bc_cs_db_services, ByVal xmltext As String, ByVal contributor_id As Long, ByVal workflow_stage As Long, ByVal accounting_standard As Long, ByVal entity_id As Long, ByVal audit_date As Date, ByVal audit_id As Long, ByVal author_id As Long, ByVal validate_only As Boolean, ByVal no_validate As Boolean, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec insight_write_data '" + xmltext + "'," + CStr(contributor_id) + "," + CStr(workflow_stage) + "," + CStr(accounting_standard) + "," + CStr(entity_id) + ",'" + Format(audit_date, "dd-MMM-yyyy HH:mm:ss") + "'," + CStr(author_id) + ",0," + CStr(audit_id) + "," + CStr(validate_only) + "," + CStr(no_validate)
        write_data_via_xml_trans = db.executesql_trans(sql, certificate)
    End Function
    Public Sub register_insight_config_files(ByVal format_filename As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bcc_core_in_register_config '" + format_filename + "'"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub delete_logical_template_config(ByVal logical_template_id As Long, ByVal context_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bcc_core_ic_reset_template " + CStr(logical_template_id) + " , " + CStr(context_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function insert_row_config(ByVal logical_template_name As String, ByVal context_id As Long, ByVal section_name As String, ByVal static_flag As Integer, ByVal label As String, ByVal scale_symbol As String, ByVal scale_factor As String, ByVal order As Long, ByVal flexible_label_flag As Boolean, ByVal link_code As String, ByVal data_type As Long, ByVal submission_code As Long, ByVal repeating_count As Long, ByVal lookup_sql As String, ByVal strval As String, ByVal row_id As Long, ByVal monetary As Integer, ByVal price_related As Integer, swarnings As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bcc_core_insight_config '" + logical_template_name + "' , " + " '" + section_name + "' , " + CStr(static_flag) + " , '" + label + "' , '" + scale_symbol + "' , " + CStr(scale_factor) + " , " + CStr(order) + " , " + CStr(flexible_label_flag) + " , '" + CStr(link_code) + "' , " + CStr(data_type) + " , " + CStr(submission_code) + " , " + CStr(repeating_count) + " , '" + lookup_sql + "','" + strval + " '," + CStr(row_id) + "," + CStr(monetary) + "," + CStr(price_related) + ",'" + swarnings + " '"
        insert_row_config = gbc_db.executesql(sql, certificate)
    End Function

    Public Function get_template_assigment(ByVal entity_id As Long, ByVal class_id As Long, ByVal schema_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select template_id from bcc_core_insight_temp_assign where entity_id=" + CStr(entity_id) + " and schema_id=" + CStr(schema_id) + " and class_id=" + CStr(class_id)
        get_template_assigment = gbc_db.executesql(sql, certificate)
    End Function

    Public Function get_contribution_assign(ByVal gid As Long, ByVal class_id As Long, ByVal schema_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_contribution_assign " + CStr(gid) + "," + CStr(class_id) + "," + CStr(schema_id)
        get_contribution_assign = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_item(ByVal attribute_code As String, ByVal submission_code As Integer, ByVal entity_Id As Long, ByVal stage_id As Long, ByVal contributor_id As Long, ByVal accounting_id As Long, ByVal dimension As String, ByVal order As Integer, ByRef certificate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_in_excel_submission_db", "get_item", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim class_id As String
        Dim new_entity_id As String
        Dim new_attribute_code As String
        Dim new_entity_class As String
        Dim new_entity_name As String
        Dim priority As String
        Try
            Dim sql As String = ""
            Dim res As Object
            If InStr(attribute_code, "[entity=", CompareMethod.Text) > 0 Then
                REM now get entity id 
                new_entity_name = Right(attribute_code, Len(attribute_code) - (InStr(attribute_code, "[", CompareMethod.Text) + 7))
                new_entity_name = Left(new_entity_name, Len(new_entity_name) - 1)
                new_attribute_code = Left(attribute_code, InStr(attribute_code, "[entity=", CompareMethod.Text) - 1)
                attribute_code = new_attribute_code
                sql = "select entity_id from entity_tbl where name='" + new_entity_name + "'"
                res = gbc_db.executesql(sql, certificate)
                If Not IsNothing(res) Then
                    If UBound(res, 2) > -1 Then
                        entity_Id = res(0, 0)
                        REM propogate entity until get a match then take value  
                    End If
                End If
            Else
                REM first see if propogated attribute is required
                If InStr(attribute_code, "[", CompareMethod.Text) > 0 Then
                    REM now get entity id 
                    new_attribute_code = Right(attribute_code, Len(attribute_code) - InStr(attribute_code, "[", CompareMethod.Text))
                    new_attribute_code = Left(new_attribute_code, Len(new_attribute_code) - 1)
                    new_entity_class = Left(attribute_code, InStr(attribute_code, "[", CompareMethod.Text) - 1)
                    attribute_code = new_attribute_code
                    sql = "select class_id from entity_class_tbl where class_name='" + new_entity_class + "'"
                    res = gbc_db.executesql(sql, certificate)
                    If Not IsNothing(res) Then
                        If UBound(res, 2) > -1 Then
                            class_id = res(0, 0)
                            REM propogate entity until get a match then take value  
                            entity_Id = get_entity_id_for_class(entity_Id, CStr(class_id), certificate)
                        End If
                    End If
                End If
            End If
            REM now see if specific entity attribute is required

            If submission_code = 1 Then
                sql = "select rtrim(ltrim(attrib_value)) from entity_attribute_value_tbl e, attribute_tbl a where e.attribute_id=a.attribute_id and entity_id=" + CStr(entity_Id) + " and contributor_id=" + CStr(contributor_id) + " and workflow_stage=" + CStr(stage_id) + " and a.label_code='" + attribute_code + "'"
            End If
            If submission_code = 2 Then
                If dimension = "Date" Then
                    sql = "select date_from from entity_attribute_values_time_series e, attribute_tbl a where e.attribute_id=a.attribute_id and entity_id=" + CStr(entity_Id) + " and contributor_id=" + CStr(contributor_id) + " and workflow_stage=" + CStr(stage_id) + " and date_to='9-9-9999'" + " and a.label_code='" + attribute_code + "'"
                Else
                    sql = "select rtrim(ltrim(value)) from entity_attribute_values_time_series e, attribute_tbl a where e.attribute_id=a.attribute_id and entity_id=" + CStr(entity_Id) + " and contributor_id=" + CStr(contributor_id) + " and workflow_stage=" + CStr(stage_id) + " and date_to='9-9-9999'" + " and a.label_code='" + attribute_code + "'"
                End If
            End If
            If submission_code = 3 Then
                sql = "select rtrim(ltrim(attrib_value)) from entity_attribute_repeating_value_tbl e, attribute_tbl a where e.attribute_id=a.attribute_id and entity_id=" + CStr(entity_Id) + " and contributor_id=" + CStr(contributor_id) + " and workflow_stage=" + CStr(stage_id) + " and a.label_code='" + attribute_code + "' and attrib_order =" + CStr(order)
            End If
            REM entity propogation
            If submission_code = 10 Then
                REM set class_id for value
                sql = "select class_id from entity_class_tbl where class_name='" + attribute_code + "'"
                res = gbc_db.executesql(sql, certificate)
                If Not IsNothing(res) Then
                    If UBound(res, 2) > -1 Then
                        class_id = res(0, 0)
                        REM propogate entity until get a match then take value  
                        new_entity_id = get_entity_id_for_class(entity_Id, CStr(class_id), certificate)
                        sql = "select rtrim(ltrim(name)) from entity_tbl where entity_id=" + CStr(new_entity_id)
                    End If
                End If
            End If
            REM analysts
            If submission_code = 11 Then
                priority = Right(attribute_code, 1)
                If IsNumeric(priority) Then
                    If dimension = "name" Then
                        sql = "select first_name + ' ' + surname from user_table u, apref_entity_tbl a where u.user_id = a.user_id And entity_Id =" + CStr(entity_Id) + " And rating=" + CStr(priority)
                    ElseIf dimension = "email" Then
                        sql = "select coalesce(email,'') from user_table u, apref_entity_tbl a where u.user_id = a.user_id And entity_Id =" + CStr(entity_Id) + " And rating=" + CStr(priority)
                    ElseIf dimension = "tel" Then
                        sql = "select coalesce(telephone,'') from user_table u, apref_entity_tbl a where u.user_id = a.user_id And entity_Id =" + CStr(entity_Id) + " And rating=" + CStr(priority)
                    ElseIf dimension = "mobile" Then
                        sql = "select coalesce(mobile_telephone,'') from user_table u, apref_entity_tbl a where u.user_id = a.user_id And entity_Id =" + CStr(entity_Id) + " And rating=" + CStr(priority)
                    End If
                End If
            End If
            res = gbc_db.executesql(sql, certificate)
            If Not IsNothing(res) Then
                If UBound(res, 2) > -1 Then
                    get_item = res(0, 0)
                Else
                    get_item = ""
                End If
            Else
                get_item = ""
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_in_excel_submission_db", "get_item", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            get_item = ""
        Finally
            otrace = New bc_cs_activity_log("bc_in_excel_submission_db", "get_item", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Private Function get_entity_id_for_class(ByVal entity_id As String, ByVal class_id As String, ByRef certificate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_in_excel_submission_db", "get_entity_id_for_class", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim sql As String
            Dim res As Object
            Dim ret_entity_id As Long
            Dim i As Integer
            get_entity_id_for_class = 0
            sql = "select parent_entity_id, class_id from entity_link_tbl l, entity_tbl e where l.parent_entity_id=e.entity_id and l.child_entity_id=" + CStr(entity_id) + " and coaleasce(e.inactive,0)=0 and coalesce(e.deleted,0)=0"
            res = gbc_db.executesql(sql, certificate)
            If Not IsNothing(res) Then
                If UBound(res, 2) > -1 Then
                    If IsNumeric(res(i, 0)) Then
                        For i = 0 To UBound(res, 2)
                            If CStr(res(1, i)) = class_id Then
                                get_entity_id_for_class = CStr(res(0, i))
                                Exit Function
                            Else
                                REM propogate parent
                                ret_entity_id = get_entity_id_for_class(CStr(res(0, i)), class_id, certificate)
                                If ret_entity_id > 0 Then
                                    get_entity_id_for_class = ret_entity_id
                                    Exit Function
                                End If
                            End If
                        Next
                    End If
                End If
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_in_excel_submission_db", "get_entity_id_for_class", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            get_entity_id_for_class = 0
        Finally
            otrace = New bc_cs_activity_log("bc_in_excel_submission_db", "get_entity_id_for_class", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function get_template_id(ByVal logical_template_name As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select template_id from template_tbl where description='" + logical_template_name + "'"
        get_template_id = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_validation_types(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select validation_type_id, description from insight_cell_validation_types"
        get_validation_types = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub insert_validation_for_row(ByVal row_id As Long, ByVal validation_id As Long, ByVal logical_template_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "insert into insight_cell_validation_for_row (row_id, template_id, validation_id) values(" + CStr(row_id) + "," + CStr(logical_template_id) + "," + CStr(validation_id) + ")"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function get_contributing_classes(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select parent_entity_class_id, child_entity_class_id, contributor_id, granularity_class_id,coalesce(primary_pref_type_id,0), coalesce(surrogate_pref_type_id,0) from insight_entity_class_submission"
        get_contributing_classes = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_universe_set(ByVal xml_entities As String, ByVal schema_id As Long, ByVal aggregation As Boolean, ByVal dual_entity As Boolean, ByVal target_class As String, ByVal target_dual_class As String, ByVal certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim fs As New bc_cs_string_services(target_class)
        target_class = fs.delimit_apostrophies
        fs = New bc_cs_string_services(target_dual_class)
        target_dual_class = fs.delimit_apostrophies

        sql = "exec dbo.bc_core_pt_item_list '" + xml_entities + "'," + CStr(schema_id) + ",'" + CStr(aggregation) + "','" + CStr(dual_entity) + "','" + target_class + "','" + target_dual_class + "'"
        read_universe_set = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_all_contributors(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select contributor_id, contributor_name from contributor_tbl order by contributor_name asc"
        read_all_contributors = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_lib_calcs(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select calculation_name, calculation_type, num_operands, formula, num_years,calc_when,coalesce(agg_only,0),coalesce(contributor1_id,0),coalesce(contributor2_id,0), coalesce(interval_type,0), coalesce(interval,0) from dbo.insight_calculations_library order by calculation_name asc"
        read_lib_calcs = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub write_lib_calc(name As String, years As Integer, c1 As Long, c2 As Long, ct As Integer, ci As Integer, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim fs As New bc_cs_string_services(name)
        name = fs.delimit_apostrophies
        sql = "exec dbo.bc_core_add_lib_calc '" + name + "', " + CStr(years) + "," + CStr(c1) + "," + CStr(c2) + "," + CStr(ct) + "," + CStr(ci)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub delete_lib_calc(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_del_lib_calcs"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function get_ef_macros(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select name, help_text,category,id,display_name,show_in_wizard from dbo.bcc_core_ef_macros order by display_name asc"
        get_ef_macros = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_ef_macro_dims(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select dimension, ord from bcc_core_ef_macro_dims where macro_id=" + CStr(id) + " order by ord asc"
        get_ef_macro_dims = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_ef_Stages(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select stage_id, stage_name from stage_tbl where stage_name in ('Draft','Publish') order by stage_name asc"
        get_ef_Stages = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_ef_periods(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select period_id,name from period_type_tbl order by full_year_flag desc"
        get_ef_periods = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_ef_templates(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select template_id, description from template_tbl order by description asc"
        get_ef_templates = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_ef_currency_codes(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select attrib_value from entity_attribute_value_tbl a inner join entity_tbl e on e.entity_id=a.entity_id and coalesce(e.inactive,0)=0 and coalesce(e.deleted,0) = 0 inner join attribute_tbl l on a.attribute_id=l.attribute_id and l.label_code='attribute.core.currency_code' inner join entity_class_tbl c on c.class_id=e.class_id and c.class_name='Currency' order by a.attrib_value asc"
        get_ef_currency_codes = gbc_db.executesql(sql, certificate)
    End Function

    Public Function get_ef_classes(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select class_id, class_name from entity_class_tbl where coalesce(inactive,0)=0 and coalesce(deleted,0)=0 and coalesce(class_type_id,1) < 3 or  coalesce(class_type_id,1)=4 order by class_name asc"
        get_ef_classes = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_ef_entities(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select entity_id, name, class_id from entity_tbl  where coalesce(inactive,0)=0 and coalesce(deleted,0)=0 order by name asc"
        get_ef_entities = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_ef_macro_params(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select param_name,optional,has_default,default_value,display_name,coalesce(lookup_sql,'') from bcc_core_ef_macro_params where macro_id=" + CStr(id) + " order by ord asc"
        get_ef_macro_params = gbc_db.executesql(sql, certificate)
    End Function
    REM FIL Feb 2013
    Public Function get_ef_schemas(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select schema_id, schema_name from schema_tbl where coalesce(inactive,0)=0 and coalesce(deleted,0)=0 and coalesce(core,0)=0 order by internal desc,schema_name asc"
        get_ef_schemas = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_ef_class_links(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select schema_id, parent_class_id, child_class_id from class_link_tbl order by schema_id asc"
        get_ef_class_links = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_ef_items(ByVal entity_id As Long, ByVal context_id As Long, ByVal section_id As Long, ByVal schema_id As Long, universe As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bcc_core_ef_item_lib " + CStr(entity_id) + "," + CStr(context_id) + "," + CStr(section_id) + "," + CStr(schema_id) + ",1,0,1,'" + universe + "'"
        get_ef_items = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_ef_contributors(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select contributor_id, contributor_name from contributor_tbl order by  contributor_id asc"
        get_ef_contributors = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_entity_sets(ByVal user_id As Long, ByVal class_name As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim fs As New bc_cs_string_services(class_name)
        class_name = fs.delimit_apostrophies
        Dim sql As String
        sql = "select set_id,name from dbo.bc_core_pt_user_entity_set where user_id=" + CStr(user_id) + " and class='" + class_name + "' order by name asc"
        get_entity_sets = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_item_sets(ByVal user_id As Long, ByVal class_name As String, ByVal dual_class_name As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim fs As New bc_cs_string_services(class_name)
        class_name = fs.delimit_apostrophies
        Dim sql As String
        If dual_class_name = "" Then
            sql = "select set_id,name from dbo.bc_core_pt_user_item_set where user_id=" + CStr(user_id) + " and class='" + class_name + "' and coalesce(dual_class,'') = '' order by name asc"
        Else
            sql = "select set_id,name from dbo.bc_core_pt_user_item_set where user_id=" + CStr(user_id) + " and class='" + class_name + "' and dual_class='" + dual_class_name + "' order by name asc"
        End If
        get_item_sets = gbc_db.executesql(sql, certificate)
    End Function

    Public Function get_entities_for_set(ByVal set_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select entity from dbo.bc_core_pt_entity_set where set_id=" + CStr(set_id) + " order by ord asc"
        get_entities_for_set = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_items_for_set(ByVal set_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select item,type,factor,monatary,symbol,class from dbo.bc_core_pt_item_set where set_id=" + CStr(set_id) + " order by ord asc"
        get_items_for_set = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_ef_param_lookup(ByVal sql As String, ByRef certificate As bc_cs_security.certificate) As Object
        get_ef_param_lookup = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_ef_chart_tool(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select name,method_call, pass_range_as_param,active from bc_core_pt_chart_tool"
        get_ef_chart_tool = gbc_db.executesql(sql, certificate)
    End Function

    Public Function get_universes(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_universes"
        get_universes = gbc_db.executesql(sql, certificate)
    End Function

    Public Function load_roles(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select role_name from role_tbl order by role_name asc"
        load_roles = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub delete_contributing_class(ByVal schema_id As Long, ByVal class_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from insight_entity_class_submission where contributor_id=" + CStr(schema_id) + " and parent_entity_class_id=" + CStr(class_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub delete_contributing_classes(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from insight_entity_class_submission"
        gbc_db.executesql(sql, certificate)
        sql = "delete from bcc_core_insight_temp_assign"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub add_contributing_classes(ByVal schema_id As Long, ByVal parent_entity_id As Long, ByVal child_entity_id As Long, ByVal granularity_class_id As Long, ByVal primary_apref_type As Long, ByVal secondary_apref_type As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "insert into insight_entity_class_submission (parent_entity_class_id, child_entity_class_id, contributor_id, granularity_class_id, primary_pref_type_id, surrogate_pref_type_id) values(" + CStr(parent_entity_id) + "," + CStr(child_entity_id) + "," + CStr(schema_id) + "," + CStr(granularity_class_id) + "," + CStr(primary_apref_type) + "," + CStr(secondary_apref_type) + ")"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub delete_logical_template(ByVal template_id As Long, ByVal context_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        REM firstly delete assignements
        sql = "delete from bcc_core_insight_temp_assign where template_id=" + CStr(template_id)
        gbc_db.executesql(sql, certificate)
        sql = "exec dbo.bcc_core_ic_delete_template " + CStr(template_id) + "," + CStr(context_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function undo_checkout(ByVal entity_id As Long, ByVal contributor_id As Long, ByVal user_id As Long, ByRef certificate As bc_cs_security.certificate) As Boolean
        undo_checkout = False
        REM can only undo checkout if person doing it has it checked out
        Dim sql As String
        Dim vuser As Object
        sql = "select count(*) from bc_insight_submission_workbooks where master_entity_id=" + CStr(entity_id) And contributor_id = " + cstr(contributor_id) and user_id=" + CStr(user_id)
        vuser = gbc_db.executesql(sql, certificate)
        If IsArray(vuser) Then
            sql = "update bc_insight_workbooks_submission set user_id=0 where master_entity_id=" + CStr(entity_id) And contributor_id = " + cstr(contributor_id) and user_id=" + CStr(user_id)
            gbc_db.executesql(sql, certificate)
            undo_checkout = True
        End If
    End Function
    Public Sub write_calculation(ByVal id As Long, ByVal result_row_id As Long, ByVal name As String, ByVal type As String, ByVal intTemplateId As Nullable(Of Long), ByVal intAggregationId As Nullable(Of Long), ByVal op1 As Long, ByVal op2 As Long, ByVal op3 As Long, ByVal op4 As Long, ByVal op5 As Long, ByVal op6 As Long, ByVal op7 As Long, ByVal op8 As Long, ByVal formula As String, ByVal calc_when As Long, ByVal delete_flag As Integer, ByVal num_years As Integer, ByVal period_name As String, ByVal monetary As Integer, ByVal monetary2 As Integer, ByVal min_tx As String, ByVal max_tx As String, ByVal min_val As String, ByVal max_val As String, ByVal na_tx As String, ByVal is_extract As Boolean, ByVal in_active As Boolean, ByVal exch_rate_method As Integer, ByVal contributor1_id As Long, ByVal contributor2_id As Long, interval_type As Integer, interval As Integer, weighting_Attribte As Long, cal_operands As Boolean, cal_month_end As Integer, standard_deviation As Double, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim strTemplateId, strAggregationId As String
        If Not intTemplateId Is Nothing Then
            strTemplateId = intTemplateId
        Else
            strTemplateId = "null"
        End If
        If Not intAggregationId Is Nothing Then
            strAggregationId = intAggregationId
        Else
            strAggregationId = "null"
        End If

        sql = "exec dbo.bcc_core_calc_write " + CStr(id) + "," + CStr(result_row_id) + ",'" + CStr(name) + "','" + CStr(type) + "'," + CStr(strTemplateId) + "," + CStr(op1) + "," + CStr(op2) + "," + CStr(op3) + "," + CStr(op4) + "," + CStr(op5) + "," + CStr(op6) + "," + CStr(op7) + "," + CStr(op8) + ",'" + formula + "'," + CStr(calc_when) + "," + CStr(delete_flag) + "," + CStr(num_years) + ",'" + period_name + "'," + CStr(monetary) + "," + CStr(monetary2) + "," + CStr(calc_when) + ",'" + min_tx + "','" + max_tx + "','" + CStr(min_val) + "','" + CStr(max_val) + "','" + na_tx + "'," + CStr(is_extract) + "," + CStr(in_active) + "," + CStr(exch_rate_method) + "," + CStr(strAggregationId) + "," + CStr(contributor1_id) + "," + CStr(contributor2_id) + "," + CStr(interval_type) + "," + CStr(interval) + "," + CStr(weighting_Attribte) + ",'" + CStr(cal_operands) + "'," + CStr(cal_month_end) + "," + CStr(standard_deviation)
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Function get_agg_audit_details(ByVal universe_name As String, ByVal batch As String, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_aggregation_logging '" + universe_name + "','" + batch + "'"
        get_agg_audit_details = gbc_db.executesql(sql, certificate)
    End Function


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
<Serializable()> Public Class bc_om_calculations
    Inherits bc_cs_soap_base_class

    Public calculations As New List(Of bc_om_calculation)

    Public templates As New ArrayList
    Public alAggregations As New ArrayList

    Public intTemplateID As Nullable(Of Long)
    Public intAggregationId As Nullable(Of Long)

    Public items As New List(Of bc_om_calc_item)
    Public fixed_operands As New List(Of bc_om_calc_item)
    Public aggregation_results As New ArrayList
    Public calc_types As New ArrayList
    Public aggregation_types As New ArrayList
    Public reserved_words As New ArrayList

    <Serializable()> Public Class bc_om_aggregation_result_row
        Public agg_id As Long
        Public agg_name As String
        Public result As Long
        Public result_name As String
        Public static_flag As Integer

        Public Sub New()

        End Sub
    End Class


    <Serializable()> Public Class bc_om_calc_item
        Implements IComparable
        Public item_id As Long
        Public name As String
        Public price_sensitive As Integer
        Public static_flag As Boolean = False
        Public curr_type As Integer


        Public Overrides Function ToString() As String
            Return name
        End Function

        Public Function CompareTo(ByVal obj As Object) As Integer Implements System.IComparable.CompareTo
            If name.Length > obj.ToString.Length Then
                Return -1
            ElseIf name.Length < obj.ToString.Length Then
                Return 1
            Else
                Return 0
            End If
        End Function

    End Class
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_calculations", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
            If Me.tmode = bc_cs_soap_base_class.tWRITE Then
                db_write()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_calculations", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message & ControlChars.Lf & ex.StackTrace, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_calculations", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim vres As Object
        Dim vresf As Object
        Dim i As Integer
        Dim oitem As bc_om_calc_item
        Dim otem As bc_om_template
        Dim oe As bc_om_entity
        Dim ocalc As bc_om_calculation
        Me.items.Clear()
        Me.templates.Clear()
        Me.alAggregations.Clear()
        Dim gdb As New bc_in_excel_submission_db

        REM read templates
        vres = gdb.get_all_logical_templates((MyBase.certificate))
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                otem = New bc_om_template
                otem.id = vres(0, i)
                otem.name = vres(1, i)
                Me.templates.Add(otem)
            Next
        End If

        REM read aggregations
        vres = gdb.get_all_aggregations((MyBase.certificate))
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                oe = New bc_om_entity
                oe.id = vres(0, i)
                oe.name = vres(1, i)
                Me.alAggregations.Add(oe)
            Next
        End If

        REM read items config items
        Me.items.Clear()
        vres = gdb.read_items(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                oitem = New bc_om_calc_item
                oitem.item_id = vres(0, i)
                oitem.name = CStr(vres(1, i))
                oitem.curr_type = CStr(vres(2, i))
                oitem.static_flag = False
                oitem.price_sensitive = 0
                Me.items.Add(oitem)
            Next
        End If
        vres = gdb.read_attributes(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                oitem = New bc_om_calc_item
                oitem.item_id = vres(0, i)
                oitem.name = CStr(vres(1, i))
                oitem.curr_type = CStr(vres(3, i))
                oitem.static_flag = True
                oitem.price_sensitive = 0
                Me.items.Add(oitem)
            Next
        End If
        REM fixed operands
        vres = gdb.read_fixed_operands(MyBase.certificate)
        Me.fixed_operands.Clear()
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                oitem = New bc_om_calc_item
                oitem.item_id = 0
                oitem.name = CStr(vres(0, i))
                oitem.price_sensitive = vres(1, i)

                Me.fixed_operands.Add(oitem)
            Next
        End If
        REM read calculations for template
        Me.calculations.Clear()
        Me.aggregation_results.Clear()
        If Not intTemplateID Is Nothing Then
            vres = gdb.get_calcs_for_template(Me.intTemplateID, MyBase.certificate)
        ElseIf Not intAggregationId Is Nothing Then
            REM  read all aggregation results for later valdity checking
            REM get taregt class of current aggregation
            Dim target_class_id As Long
            REM JIRA FIL 8306
            Dim dual_target_class_id As Long
            vres = gdb.get_target_class_id(intAggregationId, MyBase.certificate)
            If IsArray(vres) Then
                Try
                    target_class_id = CLng(vres(0, 0))
                    If IsNumeric(vres(0, 1)) = True Then
                        dual_target_class_id = CLng(vres(0, 1))
                    End If

                    REM JIRA FIL 8306
                    vres = gdb.read_all_agg_results_rows(target_class_id, dual_target_class_id, MyBase.certificate)
                    Dim ar As bc_om_aggregation_result_row
                    If IsArray(vres) Then
                        For i = 0 To UBound(vres, 2)
                            ar = New bc_om_aggregation_result_row
                            ar.agg_id = vres(0, i)
                            ar.agg_name = vres(1, i)
                            ar.result = vres(2, i)
                            ar.result_name = vres(3, i)
                            ar.static_flag = vres(4, i)
                            Me.aggregation_results.Add(ar)
                        Next
                    End If
                Catch

                End Try
            End If

            vres = gdb.get_calcs_for_aggregation(Me.intAggregationId, MyBase.certificate)


        End If
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                ocalc = New bc_om_calculation
                ocalc.id = vres(0, i)
                ocalc.ord = vres(1, i)
                ocalc.name = vres(2, i)
                ocalc.type = vres(3, i)
                ocalc.result_row_id = vres(4, i)
                ocalc.formula = vres(13, i)
                ocalc.calc_when = vres(15, i)
                ocalc.op1 = vres(5, i)
                ocalc.op2 = vres(6, i)
                ocalc.op3 = vres(7, i)
                ocalc.op4 = vres(8, i)
                ocalc.op5 = vres(9, i)
                ocalc.op6 = vres(10, i)
                ocalc.op7 = vres(11, i)
                ocalc.op8 = vres(12, i)
                ocalc.num_years = vres(14, i)
                ocalc.is_extract = 1

                ocalc.formula = Replace(ocalc.formula, "cast(isnull(value_1,0) as decimal(38,5))", "isnull(value_1,0)")
                ocalc.formula = Replace(ocalc.formula, "cast(isnull(value_2,0) as decimal(38,5))", "isnull(value_2,0)")
                ocalc.formula = Replace(ocalc.formula, "cast(isnull(value_3,0) as decimal(38,5))", "isnull(value_3,0)")
                ocalc.formula = Replace(ocalc.formula, "cast(isnull(value_4,0) as decimal(38,5))", "isnull(value_4,0)")
                ocalc.formula = Replace(ocalc.formula, "cast(isnull(value_5,0) as decimal(38,5))", "isnull(value_5,0)")
                ocalc.formula = Replace(ocalc.formula, "cast(isnull(value_6,0) as decimal(38,5))", "isnull(value_6,0)")
                ocalc.formula = Replace(ocalc.formula, "cast(isnull(value_7,0) as decimal(38,5))", "isnull(value_7,0)")
                ocalc.formula = Replace(ocalc.formula, "cast(isnull(value_8,0) as decimal(38,5))", "isnull(value_8,0)")


                ocalc.formula = Replace(ocalc.formula, "isnull(value_1,0)", "value_1")
                ocalc.formula = Replace(ocalc.formula, "isnull(value_2,0)", "value_2")
                ocalc.formula = Replace(ocalc.formula, "isnull(value_3,0)", "value_3")
                ocalc.formula = Replace(ocalc.formula, "isnull(value_4,0)", "value_4")
                ocalc.formula = Replace(ocalc.formula, "isnull(value_5,0)", "value_5")
                ocalc.formula = Replace(ocalc.formula, "isnull(value_6,0)", "value_6")
                ocalc.formula = Replace(ocalc.formula, "isnull(value_7,0)", "value_7")
                ocalc.formula = Replace(ocalc.formula, "isnull(value_8,0)", "value_8")


                Try
                    ocalc.min_val = vres(16, i)
                Catch

                End Try
                Try
                    ocalc.max_val = vres(17, i)
                Catch

                End Try
                ocalc.min_tx = vres(18, i)
                ocalc.max_tx = vres(19, i)
                ocalc.is_extract = vres(20, i)
                ocalc.inactive = vres(21, i)
                ocalc.na_tx = vres(22, i)
                ocalc.contributor1_id = vres(23, i)
                ocalc.contributor2_id = vres(24, i)
                ocalc.interval_type = vres(25, i)
                ocalc.interval = vres(26, i)
                ocalc.weighting_attribute = vres(27, i)
                ocalc.exch_rate_method = vres(28, i)
                ocalc.cal_operands = vres(29, i)
                ocalc.cal_month_end = vres(30, i)

                Try
                    ocalc.standard_deviation = vres(31, i)
                Catch

                End Try
                REM not yet implemeted
                REM ocalc.exch_rate_method()


                REM FIL JUNE 2012
                vresf = gdb.get_formula_for_row(ocalc.id, Me.intTemplateID, Me.intAggregationId, MyBase.certificate)
                If IsArray(vresf) Then
                    If UBound(vresf, 2) > -1 Then
                        ocalc.display_formula = CStr(vresf(0, 0))
                    End If
                End If

                Me.calculations.Add(ocalc)
            Next
        End If
        Dim ocalc_type As bc_om_calc_types
        calc_types.Clear()
        vres = gdb.get_calc_types(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                ocalc_type = New bc_om_calc_types
                ocalc_type.name = vres(0, i)
                ocalc_type.type = vres(1, i)
                ocalc_type.aggregate = vres(2, i)
                Me.calc_types.Add(ocalc_type)
            Next
        End If
        vres = gdb.read_reserved_words(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                Me.reserved_words.Add(vres(0, i))

            Next
        End If
        vres = gdb.read_aggregation_types(MyBase.certificate)
        Me.aggregation_types.Clear()
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                Me.aggregation_types.Add(vres(0, i))

            Next
        End If

    End Sub
    Public Sub db_write()
        Dim i As Integer
        For i = 0 To Me.calculations.Count - 1
            Me.calculations(i).db_write()
        Next
    End Sub
End Class
<Serializable()> Public Class bc_om_calculation
    Inherits bc_cs_soap_base_class
    Public result_row_id As Long
    Public id As Long
    Public ord As Long
    Public name As String
    Public period_name As String
    Public type As String
    Public intTemplateId As Nullable(Of Long)
    Public intAggregationId As Nullable(Of Long)
    Public op1 As Long
    Public op2 As Long
    Public op3 As Long
    Public op4 As Long
    Public op5 As Long
    Public op6 As Long
    Public op7 As Long
    Public op8 As Long
    Public formula As String
    Public display_formula As String
    Public calc_when As Integer
    Public delete_flag As Integer
    Public num_years As Integer
    Public formula_error As String = ""
    Public monetary As Integer = 0
    Public monetary2 As Integer = 0
    Public boolVerifyCalculation As Boolean = False
    Public strVerified As String
    REM PR extended what we save
    Public is_extract As Boolean
    Public inactive As Boolean
    Public min_tx As String
    Public max_tx As String
    Public na_tx As String
    Public min_val As String
    Public max_val As String
    Public exch_rate_method As Integer
    Public contributor1_id As Integer
    Public contributor2_id As Integer
    Public interval_type As Integer = 0
    Public interval As Integer = 0
    Public weighting_attribute As Integer = 0
    Public cal_operands As Boolean = False
    Public cal_month_end As Integer
    Public VALIDATE As Boolean = False
    Public standard_deviation As Double

    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_calculation", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tWRITE Then
                db_write()
                'ElseIf Me.tmode = bc_cs_soap_base_class.tREAD AndAlso boolVerifyCalculation Then
                '    strVerified = verifyCalculation()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_calculation", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message & ControlChars.Lf & ex.StackTrace, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_calculation", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Function verifyCalculation() As String
        Dim gdb As New bc_in_excel_submission_db
        Dim boolIsAgg As Boolean = type.ToLower.IndexOf("aggreg") > -1
        verifyCalculation = gdb.verifyCalculation(formula, boolIsAgg, MyBase.certificate)
    End Function
    Public Function db_write() As Long

        REM test calculation first
        Dim gdb As New bc_in_excel_submission_db
        Dim ostr As bc_cs_string_services
        ostr = New bc_cs_string_services(Me.name)
        Me.name = ostr.delimit_apostrophies
        ostr = New bc_cs_string_services(Me.period_name)
        Me.period_name = ostr.delimit_apostrophies
        Dim err As String
        If VALIDATE = True Then
            If Me.delete_flag = 0 Then
                REM check if an SP first for non aggs only
                If IsNothing(Me.intTemplateId) = False Then
                    If gdb.is_sp(Me.formula, MyBase.certificate) = True Then
                        Me.formula_error = ""
                        Exit Function
                    End If
                End If
                err = "1"
                REM FIL MAy 2013
                'err = verifyCalculation()
                If err = "1" Then
                    If IsNothing(Me.intTemplateId) = False Or (Len(Me.type) > 14 AndAlso Left(Me.type, 15) = "aggregate style") Then
                        Me.formula_error = gdb.test_calculation(Me.formula, False, MyBase.certificate)
                    Else
                        Me.formula_error = gdb.test_calculation(Me.formula, True, MyBase.certificate)
                    End If
                    If Me.formula_error = "" Then
                        err = verifyCalculation()
                        If err <> "1" Then
                            Me.formula_error = err
                        End If
                    End If
                Else
                    Me.formula_error = err
                End If
            End If
        Else
            gdb.write_calculation(Me.id, Me.result_row_id, Me.name, Me.type, Me.intTemplateId, Me.intAggregationId, Me.op1, Me.op2, Me.op3, Me.op4, Me.op5, Me.op6, Me.op7, Me.op8, Me.formula, Me.calc_when, Me.delete_flag, Me.num_years, Me.period_name, Me.monetary, Me.monetary2, Me.min_tx, Me.max_tx, Me.min_val, Me.max_val, Me.na_tx, Me.is_extract, Me.inactive, Me.exch_rate_method, Me.contributor1_id, Me.contributor2_id, Me.interval_type, Me.interval, Me.weighting_attribute, Me.cal_operands, Me.cal_month_end, Me.standard_deviation, MyBase.certificate)
        End If

    End Function

    Public Overrides Function ToString() As String
        Return name
    End Function

End Class
<Serializable()> Public Class bc_om_all_intermediate_sheets
    Inherits bc_cs_soap_base_class
    Public insert_sheets As New ArrayList

    REM

    Public Sub New()

    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_all_intermediate_sheets", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim gdb As New bc_in_excel_submission_db
            Dim res As Object
            Dim i As Integer
            Dim osheet As bc_om_intermediate_sheet
            Dim fs As New bc_cs_file_transfer_services
            Me.insert_sheets.Clear()

            res = gdb.get_all_intermediate_sheets(MyBase.certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    If fs.check_document_exists(bc_cs_central_settings.central_template_path + CStr(res(1, i)), certificate) Then
                        osheet = New bc_om_intermediate_sheet
                        osheet.id = res(0, i)

                        osheet.file_name = res(1, i)
                        osheet.name = res(2, i)
                        'fs.write_document_to_bytestream(bc_cs_central_settings.central_repos_path + CStr(res(1, i)), osheet.sheet, MyBase.certificate)
                        Me.insert_sheets.Add(osheet)
                    Else
                        Dim ocommentary As New bc_cs_activity_log("bc_om_all_intermediate_sheets", "db_read", bc_cs_activity_codes.COMMENTARY, "Interediate sheet does not exist on server: " + CStr(res(1, 0)), MyBase.certificate)
                    End If
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_all_intermediate_sheets", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_all_intermediate_sheets", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
End Class
<Serializable()> Public Class bc_om_intermediate_sheets
    Inherits bc_cs_soap_base_class
    Public lead_entity_id As Long
    Public schema_id As Long
    Public err_str As String
    Public insert_sheets As New ArrayList

    REM

    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Select Case MyBase.tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Dim gdb As New bc_in_excel_submission_db
        Dim osheet As bc_om_intermediate_sheet
        Dim fs As New bc_cs_file_transfer_services
        Dim vres As Object
        Dim i As Integer
        Me.insert_sheets.Clear()
        err_str = ""
        vres = gdb.get_intermediate_sheets(Me.lead_entity_id, Me.schema_id, MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                If fs.check_document_exists(bc_cs_central_settings.central_template_path + CStr(vres(1, 0)), MyBase.certificate) = True Then
                    osheet = New bc_om_intermediate_sheet
                    osheet.name = vres(0, i)
                    osheet.file_name = vres(1, i)
                    If fs.write_document_to_bytestream(bc_cs_central_settings.central_template_path + CStr(vres(1, i)), osheet.sheet, MyBase.certificate, False) = True Then
                        Me.insert_sheets.Add(osheet)
                    Else
                        Me.err_str = "Insert file: " + CStr(vres(1, i)) + " cannot be read on  server"
                        Me.insert_sheets.Clear()
                        Exit For
                    End If
                Else
                    Me.insert_sheets.Clear()
                    Me.err_str = "Insert file: " + CStr(vres(1, i)) + " not found on server"
                    Exit For
                End If
            Next
        End If
    End Sub
End Class
<Serializable()> Public Class bc_om_intermediate_sheet
    Inherits bc_cs_soap_base_class
    Public id As Long
    Public name As String
    Public file_name As String
    Public sheet As Byte()
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Select Case MyBase.tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_read()
        REM read in file from file system
        Dim fs As New bc_cs_file_transfer_services
        If fs.check_document_exists(bc_cs_central_settings.central_template_path + file_name) Then
            fs.write_document_to_bytestream(bc_cs_central_settings.central_template_path + file_name, Me.sheet, certificate)
        End If
    End Sub
    Public Sub db_write()
        REM read in file from file system
        If Me.name <> "DELETE" Then
            Dim fs As New bc_cs_file_transfer_services
            fs.write_bytestream_to_document(bc_cs_central_settings.central_template_path + file_name, Me.sheet, certificate)
        End If
        Dim gdb As New bc_in_excel_submission_db
        gdb.update_intermediate_sheet(Me.id, Me.name, Me.file_name, certificate)
    End Sub
End Class

<Serializable()> Public Class bc_om_insight_items
    Inherits bc_cs_soap_base_class
    Public insight_items As New ArrayList
    Public sections As New ArrayList
    Public schema_ids As New ArrayList
    Public schema_names As New ArrayList
    Public class_ids As New ArrayList
    Public class_names As New ArrayList
    Public contributing_classes As New ArrayList
    Public class_links As New ArrayList
    Public result_types As New bc_om_result_types
    Public acc_standards As New bc_om_accounting_standards
    Public contributor_ids As New ArrayList
    Public contributor_names As New ArrayList
    Public libray_calcs As New bc_om_libary_calculations
    Public intermediate_sheets As New bc_om_all_intermediate_sheets
    Public user_security As New ArrayList
    Public roles As New ArrayList
    Public write_mode As Integer = 0
    Public items_only As Boolean = False
    Public pref_types As New ArrayList
    Public agg_audit_headers As New ArrayList

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_items", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
            If Me.tmode = bc_cs_soap_base_class.tWRITE Then
                db_write()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_items", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_items", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim vres As Object
        Dim i As Integer
        Dim oitem As bc_om_insight_item
        Dim gdb As New bc_in_excel_submission_db
        insight_items.Clear()
        vres = gdb.read_rows(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                oitem = New bc_om_insight_item
                oitem.item_id = vres(0, i)
                oitem.desc = vres(1, i)
                oitem.default_scale_factor = vres(2, i)
                oitem.default_scale_symbol = vres(3, i)
                oitem.monetary = vres(4, i)
                oitem.price_related = vres(5, i)
                oitem.row_flag = 1
                insight_items.Add(oitem)
            Next
        End If

        vres = gdb.read_attributes(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                oitem = New bc_om_insight_item
                oitem.item_id = vres(0, i)
                oitem.desc = vres(1, i)
                oitem.label_code = vres(2, i)
                oitem.default_scale_factor = "1"
                oitem.default_scale_symbol = ""
                oitem.row_flag = 0
                oitem.monetary = vres(3, i)
                oitem.price_related = vres(4, i)
                insight_items.Add(oitem)
            Next
        End If
        If items_only = True Then
            Exit Sub
        End If
        vres = gdb.read_sections(MyBase.certificate)
        Me.sections.Clear()

        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                Me.sections.Add(CStr(vres(0, i)))
            Next
        End If
        Me.schema_ids.Clear()
        Me.schema_names.Clear()

        vres = gdb.read_schemas(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                Me.schema_ids.Add(vres(0, i))
                Me.schema_names.Add(vres(1, i))
            Next
        End If
        Me.class_ids.Clear()
        Me.class_names.Clear()
        vres = gdb.read_classes(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                Me.class_ids.Add(vres(0, i))
                Me.class_names.Add(vres(1, i))
            Next
        End If
        Me.contributing_classes.Clear()

        Dim occ As bc_om_contributing_class
        vres = gdb.get_contributing_classes(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                occ = New bc_om_contributing_class
                occ.parent_class_id = vres(0, i)
                occ.child_class_id = vres(1, i)
                occ.schema_id = vres(2, i)
                occ.granularity_class_id = vres(3, i)
                occ.primary_apref_type = vres(4, i)
                occ.secondary_apref_type = vres(5, i)

                'occ.db_read()
                Me.contributing_classes.Add(occ)
            Next
        End If
        Dim oentity_class_link As bc_om_class_link
        vres = gdb.get_class_links(MyBase.certificate)
        Me.class_links.Clear()
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                oentity_class_link = New bc_om_class_link
                oentity_class_link.schema_id = vres(0, i)
                oentity_class_link.parent_class_id = vres(1, i)
                oentity_class_link.child_class_id = vres(2, i)
                Me.class_links.Add(oentity_class_link)
            Next
        End If
        REM contributors

        vres = gdb.read_all_contributors(MyBase.certificate)
        Me.contributor_ids.Clear()
        Me.contributor_names.Clear()
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                Me.contributor_ids.Add(vres(0, i))
                Me.contributor_names.Add(vres(1, i))
            Next
        End If
        result_types.certificate = MyBase.certificate
        result_types.db_read()
        acc_standards.certificate = MyBase.certificate
        acc_standards.db_read()
        libray_calcs.db_read()
        intermediate_sheets.db_read()
        REM submission for roles
        vres = gdb.read_insight_security(certificate)
        user_security.Clear()
        Dim osec As bc_om_insight_user_access
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                osec = New bc_om_insight_user_access
                osec.role_id = vres(0, i)
                osec.role_name = vres(1, i)
                osec.stage_name = vres(2, i)
                osec.stage_id = vres(3, i)
                osec.draft_default = vres(4, i)
                user_security.Add(osec)
            Next
        End If
        Me.roles.Clear()
        vres = gdb.load_roles(certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                Me.roles.Add(vres(0, i))
            Next
        End If
        Dim opreftype As bc_om_users.bc_om_preference_type

        Me.pref_types.Clear()
        vres = gdb.load_pref_types(certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                opreftype = New bc_om_users.bc_om_preference_type
                opreftype.id = vres(0, i)
                opreftype.name = vres(1, i)
                Me.pref_types.Add(opreftype)
            Next
        End If

        REM Aggregation audit header rows 
        REM FIL 5.5 turn this off for performance
        'vres = gdb.get_agg_audit_details("HeadersOnly", "0", certificate)
        'agg_audit_headers.Clear()
        'Dim oaggheasers As bc_om_agg_audit_headers
        'If IsArray(vres) Then
        '    For i = 0 To UBound(vres, 2)
        '        oaggheasers = New bc_om_agg_audit_headers
        '        oaggheasers.universe_id = vres(0, i)
        '        oaggheasers.universe_name = vres(1, i)
        '        oaggheasers.batch_date = vres(2, i)
        '        oaggheasers.start_date = vres(3, i)
        '        oaggheasers.finnish_date = vres(4, i)
        '        oaggheasers.elapsed10ths = vres(5, i)
        '        oaggheasers.calculations = vres(6, i)
        '        oaggheasers.successtext = vres(7, i)
        '        oaggheasers.warnings = vres(8, i)
        '        agg_audit_headers.Add(oaggheasers)
        '    Next
        'End If

    End Sub

    Public Sub db_write()
        REM contributing classes
        Dim gdb As New bc_in_excel_submission_db
        Dim i As Integer
        Select Case Me.write_mode
            Case 0
                REM contributing classes
                gdb.delete_contributing_classes(MyBase.certificate)
                For i = 0 To Me.contributing_classes.Count - 1
                    gdb.add_contributing_classes(Me.contributing_classes(i).schema_id, Me.contributing_classes(i).parent_class_id, Me.contributing_classes(i).child_class_id, Me.contributing_classes(i).granularity_class_id, Me.contributing_classes(i).primary_apref_type, Me.contributing_classes(i).secondary_apref_type, MyBase.certificate)
                    Me.contributing_classes(i).db_write()
                Next
            Case 1
                gdb.delete_sub_security(MyBase.certificate)
                For i = 0 To Me.user_security.Count - 1
                    If Me.user_security(i).draft_default = True Then
                        gdb.insert_sub_security(Me.user_security(i).role_name, Me.user_security(i).stage_name, 1, MyBase.certificate)
                    Else
                        gdb.insert_sub_security(Me.user_security(i).role_name, Me.user_security(i).stage_name, 0, MyBase.certificate)
                    End If
                Next

        End Select
    End Sub
    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_insight_item
    Public item_id As Long
    Public desc As String
    Public label_code As String
    Public row_flag As Integer
    Public default_scale_factor As String
    Public default_scale_symbol As String
    Public monetary As Integer
    Public price_related As Integer

    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class bc_om_calculation_lib
    Inherits bc_om_calculation
    Public num_operand As Integer
    Public agg_only As Boolean
    'Public contributor1_id As Integer
    'Public contributor2_id As Integer
    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_libary_calculations
    Inherits bc_cs_soap_base_class

    Public libray_calculations As New ArrayList
    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select
    End Sub

    Public Sub db_read()
        Dim gdb As New bc_in_excel_submission_db
        Dim vres As Object
        Dim i As Integer
        Dim ocalc As bc_om_calculation_lib

        Me.libray_calculations.Clear()

        vres = gdb.read_lib_calcs(MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                ocalc = New bc_om_calculation_lib
                ocalc.name = vres(0, i)
                ocalc.type = vres(1, i)
                ocalc.num_operand = vres(2, i)
                ocalc.formula = vres(3, i)
                ocalc.num_years = vres(4, i)
                ocalc.calc_when = vres(5, i)
                ocalc.agg_only = vres(6, i)
                ocalc.contributor1_id = vres(7, i)
                ocalc.contributor2_id = vres(8, i)
                ocalc.interval_type = vres(9, i)
                ocalc.interval = vres(10, i)
                Me.libray_calculations.Add(ocalc)
            Next
        End If
    End Sub
    Public Sub db_write()
        Dim gdb As New bc_in_excel_submission_db
        gdb.delete_lib_calc(certificate)

        For i = 0 To Me.libray_calculations.Count - 1
            With Me.libray_calculations(i)
                gdb.write_lib_calc(.name, .num_years, .contributor1_id, .contributor2_id, .interval_type, .interval, certificate)
            End With
        Next
    End Sub

End Class
REM FIL JULY 2012
<Serializable()> Public Class bc_om_contributing_class
    Inherits bc_cs_soap_base_class
    Public parent_class_id As Long
    Public child_class_id As Long
    Public schema_id As Long
    Public granularity_class_id As Long
    Public primary_apref_type As Long
    Public secondary_apref_type As Long
    Public entity_template_assigments As New ArrayList
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_contributing_class", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
            If Me.tmode = bc_cs_soap_base_class.tWRITE Then
                db_write()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_contributing_class", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_contributing_class", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Try
            Dim gdb As New bc_in_excel_submission_db
            Dim vres As Object
            Dim vent As Object
            Dim oeta As bc_om_entity_template_assigments
            Dim i, j As Integer
            REM parent entity
            entity_template_assigments.Clear()

            If Me.granularity_class_id = 0 Then
                oeta = New bc_om_entity_template_assigments
                oeta.class_id = parent_class_id
                oeta.entity_id = 0
                oeta.entity_name = "All"
                oeta.template_id = 0
                vres = gdb.get_template_assigment(0, parent_class_id, schema_id, certificate)
                If IsArray(vres) Then
                    For j = 0 To UBound(vres, 2)
                        oeta.template_id = vres(0, j)
                    Next
                End If
                entity_template_assigments.Add(oeta)
                If Me.child_class_id <> 0 Then
                    oeta = New bc_om_entity_template_assigments
                    oeta.class_id = child_class_id
                    oeta.entity_id = 0
                    oeta.entity_name = "All"
                    oeta.template_id = 0
                    vres = gdb.get_template_assigment(0, child_class_id, schema_id, certificate)
                    If IsArray(vres) Then
                        For j = 0 To UBound(vres, 2)
                            oeta.template_id = vres(0, j)
                        Next
                    End If
                    entity_template_assigments.Add(oeta)
                End If
            Else
                REM PPPPPPPP
                vent = gdb.get_contribution_assign(granularity_class_id, parent_class_id, schema_id, certificate)
                If IsArray(vent) Then
                    For i = 0 To UBound(vent, 2)
                        oeta = New bc_om_entity_template_assigments
                        oeta.class_id = parent_class_id
                        oeta.entity_id = CStr(vent(0, i))
                        oeta.entity_name = CStr(vent(1, i))
                        oeta.template_id = CStr(vent(2, i))
                        entity_template_assigments.Add(oeta)
                    Next
                End If

                REM chid entity
                If Me.child_class_id <> 0 Then
                    vent = gdb.get_contribution_assign(granularity_class_id, child_class_id, schema_id, certificate)
                    If IsArray(vent) Then
                        For i = 0 To UBound(vent, 2)
                            oeta = New bc_om_entity_template_assigments
                            oeta.class_id = child_class_id
                            oeta.entity_id = CStr(vent(0, i))
                            oeta.entity_name = CStr(vent(1, i))
                            oeta.template_id = CStr(vent(2, i))
                            entity_template_assigments.Add(oeta)
                        Next
                    End If
                End If

            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_contributing_class", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally

        End Try
    End Sub
    REM FIL FEB 2013
    Public Enum EWRITE_MODE
        INSERT = 0
        DELETE = 1
    End Enum
    Public write_mode As EWRITE_MODE = EWRITE_MODE.INSERT

    Public Sub db_write()
        Dim gdb As New bc_in_excel_submission_db
        Select Case write_mode

            Case EWRITE_MODE.INSERT
                gdb.insert_template_assignment(Me.write_data_to_xml(MyBase.certificate), MyBase.certificate)
            Case EWRITE_MODE.DELETE
                gdb.delete_template_assignment(Me.write_data_to_xml(MyBase.certificate), MyBase.certificate)
        End Select
    End Sub
    Public Overrides Function write_data_to_xml(ByRef certifiate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_contributing_class", "write_data_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim cbc_data_services As New bc_cs_data_services
        Dim ocommentary As New bc_cs_activity_log("bc_om_contributing_class", "write_data_to_xml", bc_cs_activity_codes.COMMENTARY, "Attempting to write XML string")

        write_data_to_xml = soap_serialize_object_to_xml(Me, Me.certificate)

        otrace = New bc_cs_activity_log("bc_om_contributing_class", "write_data_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    End Function

    Private Function soap_serialize_object_to_xml(ByVal o As Object, ByRef certificate As bc_cs_security.certificate, Optional ByVal packet_code As String = "") As String

        Dim otrace As New bc_cs_activity_log("bc_om_contributing_class", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Dim bc_cs_central_settings As New bc_cs_central_settings
        Dim by() As Byte

        Try
            Using ms As New MemoryStream

                Dim extratype(0) As Type

                extratype(0) = GetType(bc_om_entity_template_assigments)
                REM extratype(1) = GetType(bc_om_cell_validation)
                REM extratype(2) = GetType(bc_om_insight_rows_cell_value)

                Dim sf As New XmlSerializer(o.GetType, extratype)

                sf.Serialize(ms, o)

                by = ms.ToArray()

                ms.Close()

                'xml serialization does not always encode apostrophe's.  This causes an issue when passing to SQL Server
                soap_serialize_object_to_xml = Replace(System.Text.Encoding.UTF8.GetString(by), "'", "&#39;")

            End Using

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_contributing_class", "soap_serialize_object_to_xml", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            soap_serialize_object_to_xml = ""
        Finally
            otrace = New bc_cs_activity_log("bc_om_contributing_class", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function


    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_class_link
    Public schema_id As Long
    Public parent_class_id As Long
    Public child_class_id As Long

    Public Sub New()

    End Sub
End Class
REM PR FIL 5.6
<Serializable()> Public Class bc_om_entity_template_assigments
    Inherits bc_cs_soap_base_class

    Public class_id As Long
    Public entity_id As Long
    Public entity_name As String
    Public template_id As Long
    Public schema_Id As Long
    Public granularity_class_id As Long

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
            Dim res As Object
            template_id = 0
            Dim gdb As New bc_om_entity_template_assigments_db
            res = gdb.read_ass(Me.class_id, Me.schema_Id, Me.entity_id, certificate)
            If IsArray(res) Then
                If UBound(res, 2) = 0 Then
                    template_id = res(0, 0)
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_entity_template_assigments", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
    Public Sub db_write()
        Try
            Dim gdb As New bc_om_entity_template_assigments_db
            gdb.write_ass(Me.class_id, Me.schema_Id, Me.entity_id, Me.template_id, Me.granularity_class_id, certificate)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_entity_template_assigments", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub

    Class bc_om_entity_template_assigments_db
        Dim db As New bc_cs_db_services
        Public Sub write_ass(class_id As Long, schema_id As Long, entity_id As Long, template_id As Long, granularity_class_id As Long, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            sql = "dbo.bc_core_write_temp_assign " + CStr(class_id) + "," + CStr(schema_id) + "," + CStr(entity_id) + "," + CStr(template_id) + "," + CStr(granularity_class_id)
            db.executesql(sql, certificate)
        End Sub
        Public Function read_ass(class_id As Long, schema_id As Long, entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
            Dim sql As String
            sql = "dbo.bc_core_read_temp_assign " + CStr(class_id) + "," + CStr(schema_id) + "," + CStr(entity_id)
            read_ass = db.executesql(sql, certificate)
        End Function
    End Class
End Class
<Serializable()> Public Class bc_om_insight_config_files
    Inherits bc_cs_soap_base_class
    Public xml_file As Byte()
    Public xml_file_exists As Boolean
    Public format_file As Byte()
    Public format_file_name As String
    Public Overrides Sub process_object()
        Select Case Me.tmode
            Case tREAD
                db_read()
            Case tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_write()
        REM copy files into central repository
        Dim gdb As New bc_in_excel_submission_db
        gdb.register_insight_config_files(Me.format_file_name, MyBase.certificate)
        Dim fs As New bc_cs_file_transfer_services
        fs.write_bytestream_to_document(bc_cs_central_settings.central_template_path + "bc_am_insight_config.xml", xml_file, MyBase.certificate)
        If Me.format_file_name <> "" And Me.format_file_name <> "ignore" Then
            fs.write_bytestream_to_document(bc_cs_central_settings.central_template_path + format_file_name, format_file, MyBase.certificate)
        End If
        REM register templates in database and set sync
    End Sub
    Public Sub db_read()
        REM copy files from central repository
        Dim fs As New bc_cs_file_transfer_services
        REM config file
        xml_file_exists = False
        If fs.check_document_exists(bc_cs_central_settings.central_template_path + "bc_am_insight_config.xml", certificate) Then
            fs.write_document_to_bytestream(bc_cs_central_settings.central_template_path + "bc_am_insight_config.xml", xml_file, MyBase.certificate)
            xml_file_exists = True
        End If
        REM format file TBD
        REM get format filename from db
    End Sub

End Class
<Serializable()> Public Class bc_om_class_chart_tools
    Public tool_name As String
    Public tool_method_call As String
    Public tool_passrange As Boolean
    Public tool_active As Boolean

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_agg_audit_headers_List
    Inherits bc_cs_soap_base_class
    Public headers As New List(Of bc_om_agg_audit_headers)
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_aggregation_log_headers", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_aggregation_log_headers", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_aggregation_log_headers", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim vres As Object
        Dim i As Integer
        Dim gdb As New bc_in_excel_submission_db

        REM Aggregation audit header rows 
        REM FIL 5.5 
        vres = gdb.get_agg_audit_details("HeadersOnly", "0", certificate)
        headers.Clear()
        Dim oaggheasers As bc_om_agg_audit_headers
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                oaggheasers = New bc_om_agg_audit_headers
                oaggheasers.universe_id = vres(0, i)
                oaggheasers.universe_name = vres(1, i)
                oaggheasers.batch_date = vres(2, i)
                oaggheasers.start_date = vres(3, i)
                oaggheasers.finnish_date = vres(4, i)
                oaggheasers.elapsed10ths = vres(5, i)
                oaggheasers.calculations = vres(6, i)
                oaggheasers.successtext = vres(7, i)
                oaggheasers.warnings = vres(8, i)
                headers.Add(oaggheasers)
            Next
        End If

    End Sub
End Class

<Serializable()> Public Class bc_om_agg_audit_headers
    Inherits bc_cs_soap_base_class

    Public universe_id As Long
    Public universe_name As String
    Public batch_date As String
    Public start_date As Date
    Public finnish_date As Date
    Public elapsed10ths As Long
    Public calculations As Long
    Public successtext As String
    Public warnings As Long

    Public Sub New()

    End Sub


End Class

<Serializable()> Public Class bc_om_agg_audit_details
    Public universe_id As Long
    Public batch_date As String
    Public log_date As Date
    Public log_comment As String
    Public log_error As String
    Public type_name As String
    Public elapsed10ths As Long
    Public successtext As String
    Public warnings As Long

    Public Sub New()

    End Sub



End Class

<Serializable()> Public Class bc_om_insight_aggregation_log_deatils
    Inherits bc_cs_soap_base_class

    Public detail_universe As String
    Public detail_batch As String
    Public agg_audit_details As New ArrayList

    Public Sub New(ByVal loguniverse As String, ByVal logbatch As String)

        detail_universe = loguniverse
        detail_batch = logbatch

    End Sub

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_insight_aggregation_log_deatils", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
            If Me.tmode = bc_cs_soap_base_class.tWRITE Then
                db_write()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_insight_aggregation_log_deatils", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_insight_aggregation_log_deatils", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Sub db_read()
        Dim vres As Object
        Dim i As Integer
        Dim gdb As New bc_in_excel_submission_db

        REM Aggregation audit detail rows 
        vres = gdb.get_agg_audit_details(detail_universe, detail_batch, certificate)
        agg_audit_details.Clear()
        Dim oaggheasers As bc_om_agg_audit_details
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                oaggheasers = New bc_om_agg_audit_details
                oaggheasers.universe_id = vres(0, i)
                oaggheasers.batch_date = vres(1, i)
                oaggheasers.log_date = vres(2, i)
                oaggheasers.log_comment = vres(3, i)
                oaggheasers.log_error = vres(4, i)
                oaggheasers.type_name = vres(5, i)
                oaggheasers.elapsed10ths = vres(6, i)
                oaggheasers.successtext = vres(7, i)
                oaggheasers.warnings = vres(8, i)
                agg_audit_details.Add(oaggheasers)
            Next
        End If

    End Sub

    Public Sub db_write()

    End Sub

End Class
