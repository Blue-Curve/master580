Imports BlueCurve.Core.CS

<Serializable>Public Class bc_om_universes_preview
    Inherits bc_cs_soap_base_class
    Public luniverses = New List(Of bc_om_universe_preview)
    Public entities = New List(Of entity)
    Public universe_id As Long
    Public Overrides Sub process_object()
        db_read()
    End Sub


    Public Sub db_read()
        Try

            Dim gdb = New bc_cs_db_services
            Dim res As Object

            Dim universe As bc_om_universe_preview
            Dim entity As entity
            Dim i As Integer
            res = gdb.executesql("exec dbo.bc_core_aggs_services_preview_get_entities", certificate)

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    entity = New entity
                    entity.entity_id = res(0, i)
                    entity.name = res(1, i)
                    entity.class_id = res(2, i)
                    entities.Add(entity)
                Next
            End If
            res = gdb.executesql("exec dbo.bc_core_aggs_services_preview_get_universes", certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    universe = New bc_om_universe_preview
                    universe.universe_id = res(0, i)
                    universe.universe_name = res(1, i)
                    If (universe_id = 0 Or (universe_id = universe.universe_id)) Then
                        universe.db_read(certificate)
                        luniverses.Add(universe)
                    End If
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("universes", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

End Class

<Serializable> Public Class bc_om_universe_preview

    Public universe_id As Long
    Public universe_name As String
    Public target_classs = New List(Of target_class)
    Public years = New List(Of Integer)
    Public contributors = New List(Of contributor)
    Public metrics = New List(Of metric)
    Public exch_rate_method As Integer
    Public calc_types = New List(Of String)

    Public Sub db_read(ByRef certificate As bc_cs_security.certificate)
        Try
            Dim gdb As New bc_cs_db_services
            Dim target_class As target_class
            Dim contributor As contributor
            Dim metric As metric

            Dim res As Object
            res = gdb.executesql("exec dbo.bc_core_aggs_services_preview_get_universe_target " + CStr(universe_id), certificate)
            Dim i As Integer

            For i = 0 To UBound(res, 2)
                target_class = New target_class
                target_class.class_id = res(0, i)
                target_class.class_name = res(1, i)
                target_class.dual_class_id = res(2, i)
                target_class.dual_class_name = res(3, i)
                target_classs.Add(target_class)
            Next

            res = gdb.executesql("exec dbo.bc_core_aggs_services_preview_get_contributors " + CStr(universe_id), certificate)
            For i = 0 To UBound(res, 2)
                contributor = New contributor()
                contributor.contributor_id = res(0, i)
                contributor.contributor_name = res(1, i)
                contributors.Add(contributor)
            Next

            res = gdb.executesql("exec dbo.bc_core_aggs_services_preview_get_years " + CStr(universe_id), certificate)
            For i = 0 To UBound(res, 2)
                years.Add(res(0, i))
            Next

            res = gdb.executesql("exec dbo.bc_core_aggs_services_preview_get_metrics " + CStr(universe_id), certificate)
            For i = 0 To UBound(res, 2)
                metric = New metric
                metric.metric_id = res(0, i)
                metric.metric_name = res(1, i)
                metric.num_years = res(2, i)
                metric.contributor2 = res(3, i)
                metric.formula = res(4, i)
                metrics.Add(metric)
            Next

            res = gdb.executesql("exec dbo.bc_core_aggs_services_preview_get_exch_rate_method " + CStr(universe_id), certificate)
            For i = 0 To UBound(res, 2)
                exch_rate_method = res(0, 0)
            Next

            res = gdb.executesql(" dbo.bc_core_aggs_services_preview_get_calc_types " + CStr(universe_id), certificate)
            For i = 0 To UBound(res, 2)
                calc_types.Add(res(0, i))
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_universe_preview", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
<Serializable> Public Class metric
    Public metric_id As Long
    Public metric_name As String
    Public num_years As Integer
    Public contributor2 As Integer
    Public formula As String
End Class
<Serializable> Public Class contributor
    Public contributor_id As Long
    Public contributor_name As String
End Class
<Serializable> Public Class target_class
    Public class_id As Long
    Public class_name As String
    Public dual_class_id As Long
    Public dual_class_name As String
End Class
<Serializable> Public Class entity
    Public entity_id As Long
    Public class_id As Long
    Public name As String
End Class
