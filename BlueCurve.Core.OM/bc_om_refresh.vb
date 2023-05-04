Imports BlueCurve.Core.CS
Imports System.IO
Imports System.Xml.Serialization
Imports System.Xml
Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.Collections
Imports BlueCurve.Core.CS.WCF.Proxy


'Imports System.Xml.Linq
'Imports System.Text
'Imports System.Text.RegularExpressions

REM ===============================================
REM Bluecurve Limited 2005
REM Module:       Create Document Object Model
REM Type:         Object Model
REM Description:  Refresh Object
REM Version:      1
REM Change history
REM ===============================================
Friend Class bc_om_cached_component
    Public method As String
    Public contributor_id As String
    Public external_id As Long
    Public entity_id As Long
    Public parameters As Object
    Public external_xml As String
End Class
<Serializable()> Public Class bc_om_parameter_lookup
    Inherits bc_cs_soap_base_class
    Public lookup_sql As String
    Public entity_id As Long
    Public lookup_vals As New ArrayList
    Public lookup_vals_ids As New ArrayList
    Public doc_id As String
    Public type_id As Integer
    Public context_id As Long
    Public context_items As New bc_om_parameter_context_items
    Public dependent_parameter_values As New List(Of String)
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Select Case Me.tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
        End Select
    End Sub
    REM FIL AUG 2013
    Public Sub db_read()
        Dim gdb As New bc_om_component_parameters_db
        Dim sql As String
        Dim res As Object
        Dim i As Integer
        lookup_vals.Clear()

        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If

        sql = "exec " + lookup_sql + " " + CStr(entity_id)

        res = gdb.get_lookup_values(sql, dependent_parameter_values, certificate, doc_id)
        If IsArray(res) Then
            For i = 0 To UBound(res, 2)
                If UBound(res, 1) = 1 Then
                    lookup_vals.Add(res(0, i))
                Else
                    lookup_vals_ids.Add(res(1, i))
                    lookup_vals.Add(res(0, i))
                End If
            Next
        End If
        If type_id = 9 Then
            Me.context_items.context_id = context_id
            Me.context_items.entity_id = entity_id
            Me.context_items.doc_id = doc_id
            Me.context_items.certificate = certificate

            Me.context_items.db_read()
        End If
    End Sub
End Class
REM FIL OCT 2013
Public Delegate Function refreshMethodCaller(ByVal callDuration As Integer, _
    <Out()> ByRef threadId As Integer) As String
<Serializable()> Public Class bc_om_refresh_components
    Inherits bc_cs_soap_base_class
    Public workflow_state As Long
    Public accounting_standard As Long
    'Public data_at_date As String = "9-9-9999"
    Public data_at_date As Date = "9-9-9999"
    Public upload_component_values As Boolean = False
    Public doc_id As String
    Public language_id As Long
    Public pub_type_id As Long
    Public refresh_components As New List(Of bc_om_refresh_component)
    Public index_components As New bc_om_indexs
    Public indexs_only As Boolean = False
    Public refresh_type As Integer = 1
    'Public new_components As New ArrayList
    Public assoc_entities As New ArrayList
    Public calculating As Boolean
    Public success As Boolean
    Public embargo_level As Integer = 0
    Public external_data_sources As New ArrayList
    Public Const tREAD_UPDATE_DATES = 4
    REM selection parameters
    Public oselection_parameters As New selection_parameters
    REM  extrnal proxies
    Public Shared loaded_external_proxies As New ArrayList
    Public show_sdcs As Boolean = False
    Public show_udcs As Boolean = False
    Public single_select_mode As Boolean = False
    Public component_documents As New ArrayList
    REM FIL JUNE 2013
    Public partial_component_refresh As Boolean = False
    Public partial_component_sp_list As String = ""
    Public include_component_types As New ArrayList
    Public stage_change_error_text As String
    Public from_stage_change As Boolean = False
    <NonSerialized()> Public err_text As String
    <NonSerialized()> Dim ocached_components As ArrayList
    <NonSerialized()> Public component_to_refresh_count = 0
    <Serializable()> Public Class selection_parameters
        Public value As String
        Public row As Integer
        Public col As Integer
    End Class
    Public Sub New()
        calculating = 0
        Me.packet_code = "create"
        success = True
    End Sub
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_components", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
                If certificate.server_errors.Count > 0 AndAlso from_stage_change = True Then
                    Me.stage_change_error_text = certificate.server_errors(0)
                    certificate.server_errors.Clear()
                    certificate.error_state = False
                End If
            End If
            If Me.tmode = bc_om_refresh_components.tREAD_UPDATE_DATES Then
                Me.get_data_update_date()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_refresh_components", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_refresh_components", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub


    REM this is used for defined components
    Public Function db_write_components(ByVal doc_id As Long, ByVal stage_name As String, ByRef certificate As bc_cs_security.certificate, Optional ByRef db As bc_cs_db_services = Nothing)
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_components", "db_write_components", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        db_write_components = True
        Try
            For i = 0 To refresh_components.Count - 1
                refresh_components(i).db_write_component(doc_id, certificate, db)
                If refresh_components(i).err_text <> "" Then
                    err_text = refresh_components(i).err_text
                    db_write_components = False
                    err_text = err_text + ", locator: " + refresh_components(i).locator
                    Exit Function
                End If
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_refresh_components", "db_write_components", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_refresh_components", "db_write_components", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Shadows Sub db_write(ByVal doc_id As Long, ByVal stage_name As String, ByRef certificate As bc_cs_security.certificate, Optional ByRef db As bc_cs_db_services = Nothing)

        Dim otrace As New bc_cs_activity_log("bc_om_refresh_components", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim i As Integer
            For i = 0 To refresh_components.Count - 1
                'refresh_components(i).db_write(doc_id, certificate, db)
                If refresh_components(i).err_text <> "" Then
                    err_text = refresh_components(i).err_text
                    Exit Sub
                End If
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_refresh_components", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_refresh_components", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    Public Delegate Sub parallelrefreshMethodCaller()
    Public Delegate Sub parallelrefreshbatchCaller(ByVal batch_id As Integer)
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_components", "db_read: parallel mode: " + CStr(bc_cs_central_settings.parallel_refresh_mode), bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim gdb As New bc_om_refresh_components_db

        Try
            Dim ocomm As bc_cs_activity_log
            Me.success = True
            REM clear out cache
            ocached_components = New ArrayList
            REM first check entity not being calculated
            If Me.refresh_components.Count > 0 Then
                Me.calculating = gdb.is_calculating(Me.refresh_components(0).entity_id)
            End If
            If Me.calculating = True Then
                Exit Sub
            End If
            If Me.embargo_level > 0 Then
                gdb.set_embargo_level(doc_id, Me.embargo_level, certificate)
            End If
            If Me.component_documents.Count > 0 Then
                gdb.set_component_documents(doc_id, Me.component_documents, MyBase.certificate)
            End If
            Dim i As Integer
            Dim disclosures_loaded As Boolean = False

            REM FIL JUNE 2013
            REM partial component refresh event
            If Me.partial_component_refresh = True Then

                Dim res As Object
                ocomm = New bc_cs_activity_log("bc_om_refresh_components", "db_read", bc_cs_activity_codes.COMMENTARY, "Partial Componet Refresh Selected", MyBase.certificate)
                res = gdb.read_components_to_refresh(Me.partial_component_sp_list, MyBase.certificate)
                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        Me.include_component_types.Add(res(0, i))
                        ocomm = New bc_cs_activity_log("bc_om_refresh_components", "db_read", bc_cs_activity_codes.COMMENTARY, "Partial Componet Refresh required for component type: " + CStr(res(0, i)), MyBase.certificate)
                    Next
                End If
                For i = 0 To refresh_components.Count - 1
                    refresh_components(i).no_refresh = 1
                    For j = 0 To Me.include_component_types.Count - 1
                        If refresh_components(i).type = Me.include_component_types(j) Then
                            ocomm = New bc_cs_activity_log("bc_om_refresh_components", "db_read", bc_cs_activity_codes.COMMENTARY, "Partial Componet Refresh enabled for locator: " + refresh_components(i).locator, MyBase.certificate)
                            refresh_components(i).no_refresh = 0
                        End If
                    Next
                Next
            End If


            REM load index components first
            If indexs_only = True Then
                index_components.db_write(doc_id, certificate)
                REM disable displays that aren't indexs
                For i = 0 To refresh_components.Count - 1
                    If refresh_components(i).mode <> 6 And refresh_components(i).mode <> 26 Then
                        refresh_components(i).no_refresh = 1
                    Else
                        refresh_components(i).no_refresh = 0
                    End If
                Next
            Else
                REM update assoc doc entity as may of changed in submit screen prior to submission
                If IsNumeric(doc_id) = True Then
                    gdb.delete_assoc_entity(doc_id, True, MyBase.certificate)
                    For i = 0 To assoc_entities.Count - 1
                        gdb.add_assoc_entity(doc_id, assoc_entities(i).entity_id, True, MyBase.certificate)
                    Next
                Else
                    gdb.delete_assoc_entity(doc_id, False, MyBase.certificate)
                    For i = 0 To assoc_entities.Count - 1
                        gdb.add_assoc_entity(doc_id, assoc_entities(i).entity_id, False, MyBase.certificate)
                    Next
                End If
                For i = 0 To refresh_components.Count - 1
                    'refresh_components(i).no_refresh = 0
                    If refresh_components(i).mode = 6 Or refresh_components(i).mode = 26 Then
                        refresh_components(i).no_refresh = 1
                    End If
                Next
            End If
            Dim has_external_calls As Boolean = False
            For i = 0 To refresh_components.Count - 1
                REM if refresh component is not correct tyoe disable
                If refresh_components(i).refresh_type <> Me.refresh_type Then
                    REM Or refresh_components(i).no_refresh = 1 Then
                    Dim ocommentary = New bc_cs_activity_log("bc_om_refresh", "db_read", bc_cs_activity_codes.COMMENTARY, "Refresh Disabled for:" + refresh_components(i).locator + " as not in refresh type: " + CStr(Me.refresh_type), MyBase.certificate)
                    refresh_components(i).no_refresh = 1
                ElseIf refresh_components(i).no_refresh = 1 Then
                    Dim ocommentary = New bc_cs_activity_log("bc_om_refresh", "db_read", bc_cs_activity_codes.COMMENTARY, "Refresh Disabled for:" + refresh_components(i).locator + CStr(Me.refresh_type), MyBase.certificate)
                Else
                    refresh_components(i).no_refresh = 0
                    Dim ocommentary = New bc_cs_activity_log("bc_om_refresh", "db_read", bc_cs_activity_codes.COMMENTARY, "Refresh Enabled for:" + refresh_components(i).locator, MyBase.certificate)
                End If
                If bc_cs_central_settings.parallel_refresh_mode <> 2 Then
                    If refresh_components(i).disabled = 0 And refresh_components(i).no_refresh = 0 Then
                        has_external_calls = True
                        'refresh_components(i).db_read(Me.pub_type_id, Me.accounting_standard, Me.workflow_state, Me.language_id, Me.data_at_date, Me.doc_id, Me.oselection_parameters.row, Me.oselection_parameters.col, False, Me.assoc_entities, bc_om_refresh_components.loaded_external_proxies, ocached_components, MyBase.certificate, 0)
                        'Me.success = refresh_components(i).success
                        'If Me.success = False Then
                        '    Exit Sub
                        'End If
                    End If
                End If
            Next

            REM allow up to 64 threads
            REM so chunk up the components into batches
            REM ========================================
            If bc_cs_central_settings.parallel_refresh_mode >= 2 Then
                Dim first As Integer = -1
                Dim batch As Integer = 0
                component_to_refresh_count = 0
                For i = 0 To refresh_components.Count - 1
                    If refresh_components(i).disabled = 0 And refresh_components(i).no_refresh = 0 Then
                        refresh_components(i).parallel_batch = batch
                        If first = -1 Then
                            first = i
                        End If
                        component_to_refresh_count = component_to_refresh_count + 1
                        batch = batch + 1
                        If batch Mod 64 = 0 Then
                            batch = 0
                        End If
                    Else
                        refresh_components(i).parallel_batch = -1
                    End If
                Next
                ocomm = New bc_cs_activity_log("bc_om_refresh_components", "db_read:Parallel Mode", bc_cs_activity_codes.COMMENTARY, "Number to Refresh: " + CStr(component_to_refresh_count), certificate)
                ' if 1 component only run in sequential mode
                If component_to_refresh_count = 1 Then
                    ocomm = New bc_cs_activity_log("bc_om_refresh_components", "db_read:Parallel Mode", bc_cs_activity_codes.COMMENTARY, "Run in Sequential mode as only 1 component" + CStr(component_to_refresh_count), certificate)
                    refresh_components(first).db_read(Me.pub_type_id, Me.accounting_standard, Me.workflow_state, Me.language_id, Me.data_at_date, Me.doc_id, Me.oselection_parameters.row, Me.oselection_parameters.col, False, Me.assoc_entities, bc_om_refresh_components.loaded_external_proxies, ocached_components, MyBase.certificate, 0)
                    stage_change_error_text = refresh_components(first).stage_change_error_text
                    Me.success = refresh_components(first).success
                    If Me.success = False Then
                        Exit Sub
                    End If
                Else
                    Dim caller As parallelrefreshMethodCaller
                    ' Create the delegate. 
                    If bc_cs_central_settings.parallel_refresh_mode = 2 Then
                        caller = New parallelrefreshMethodCaller(AddressOf parallelrefreshmethod_connectionpool)
                    Else
                        caller = New parallelrefreshMethodCaller(AddressOf parallelrefreshmethod_no_connection_pool)
                    End If
                    ' Initiate the asynchronous call. 

                    Dim result As IAsyncResult = caller.BeginInvoke(Nothing, Nothing)
                    result.AsyncWaitHandle.WaitOne()
                    result.AsyncWaitHandle.Close()
                End If
            End If


            If bc_cs_central_settings.parallel_refresh_mode < 2 Then
                REM FIL OCT 2013 sync web service refresh
                REM =========================================
                Dim result As IAsyncResult = Nothing
                Dim caller As refreshMethodCaller = Nothing
                Dim threadId As Integer

                If bc_cs_central_settings.parallel_refresh_mode = 1 And has_external_calls = True Then
                    ' Create the delegate. 
                    caller = New refreshMethodCaller(AddressOf asyncrefresh)
                    ' Initiate the asynchronous call. 
                    result = caller.BeginInvoke(1000, threadId, Nothing, Nothing)
                    ' Perform additional processing here and then 
                End If

                For i = 0 To refresh_components.Count - 1
                    If refresh_components(i).disabled = 0 And refresh_components(i).no_refresh = 0 And (refresh_components(i).web_service_name = "" Or bc_cs_central_settings.parallel_refresh_mode = 0) Then
                        refresh_components(i).db_read(Me.pub_type_id, Me.accounting_standard, Me.workflow_state, Me.language_id, Me.data_at_date, Me.doc_id, Me.oselection_parameters.row, Me.oselection_parameters.col, disclosures_loaded, Me.assoc_entities, bc_om_refresh_components.loaded_external_proxies, ocached_components, MyBase.certificate, 0)
                        Me.success = refresh_components(i).success
                        stage_change_error_text = refresh_components(i).stage_change_error_text

                        If Me.success = False Then
                            Me.err_text = refresh_components(i).err_text
                            Exit Sub
                        End If
                    End If
                Next

                If bc_cs_central_settings.parallel_refresh_mode = 1 And has_external_calls = True Then
                    ' wait for the WaitHandle to be signaled.
                    result.AsyncWaitHandle.WaitOne()
                    ' Call EndInvoke to retrieve the results. 
                    Dim returnValue As String = caller.EndInvoke(threadId, result)
                    ' Close the wait handle.
                    result.AsyncWaitHandle.Close()
                End If
            End If

            REM ==================================

            REM read back disclosures in case some are set on refresh
            Me.assoc_entities.Clear()
            Dim vres As Object
            Dim otaxonomy As bc_om_taxonomy
            If IsNumeric(Me.doc_id) = True Then
                vres = gdb.get_assoc_entities(Me.doc_id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        otaxonomy = New bc_om_taxonomy
                        otaxonomy.entity_id = vres(0, i)
                        Me.assoc_entities.Add(otaxonomy)
                    Next
                End If
            Else
                vres = gdb.get_tmp_assoc_entities(Me.doc_id, MyBase.certificate)
                If IsArray(vres) Then
                    For i = 0 To UBound(vres, 2)
                        otaxonomy = New bc_om_taxonomy
                        otaxonomy.entity_id = vres(0, i)
                        Me.assoc_entities.Add(otaxonomy)
                    Next
                End If
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_refresh_components", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            If Me.embargo_level > 0 Then
                gdb.clear_embargo_level(doc_id, certificate)
            End If
            otrace = New bc_cs_activity_log("bc_om_refresh_components", "db_read: parallel mode: " + CStr(bc_cs_central_settings.parallel_refresh_mode), bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub parallelrefreshmethod_no_connection_pool()
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_components", "parallelrefreshmethod_no_connection_pool", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try

            Dim cthreads As New List(Of parallelrefreshbatchCaller)
            Dim threads_to_run As Integer
            If component_to_refresh_count > 64 Then
                threads_to_run = 63
            Else
                threads_to_run = component_to_refresh_count
            End If

            Dim ocomm As New bc_cs_activity_log("parallelrefreshmethod", "db_read:Parallel Mode", bc_cs_activity_codes.COMMENTARY, "Number of threads: " + CStr(threads_to_run), certificate)

            'Dim caller As parallelrefreshbatchCaller
            Dim handle(threads_to_run) As WaitHandle



            Dim t As Thread
            Dim allthreads As New List(Of Thread)
            completed_thread_count = 0
            For i = 0 To threads_to_run
                otrace = New bc_cs_activity_log("bc_om_refresh_components", "parallelrefreshmethod starting thread:" + CStr(i), bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
                t = New Thread(AddressOf Me.parallelrefresh)
                t.Start(i)
                allthreads.Add(t)
            Next
            REM wait until all the threads have stopped
            While completed_thread_count < threads_to_run
                Thread.CurrentThread.Sleep(1000)
            End While

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_refresh_components", "parallelrefreshmethod_no_connection_pool", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Me.success = False
        Finally
            otrace = New bc_cs_activity_log("bc_om_refresh_components", "parallelrefreshmethod_no_connection_pool", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try

    End Sub
    Public Sub parallelrefreshmethod_connectionpool()
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_components", "parallelrefreshmethod_connectionpool", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try

            Dim cthreads As New List(Of parallelrefreshbatchCaller)
            Dim threads_to_run As Integer
            If component_to_refresh_count > 64 Then
                threads_to_run = 63
            Else
                threads_to_run = component_to_refresh_count
            End If

            Dim ocomm As New bc_cs_activity_log("parallelrefreshmethod", "db_read:Parallel Mode", bc_cs_activity_codes.COMMENTARY, "Number of threads: " + CStr(threads_to_run), certificate)

            Dim minWorker, minIOC As Integer
            Dim newminthreads As Integer
            ' Get the current settings.
            ThreadPool.GetMinThreads(minWorker, minIOC)

            ' Change the minimum number of worker threads to four, but 
            ' keep the old setting for minimum asynchronous I/O  
            ' completion threads. 
            If bc_cs_central_settings.num_min_idle_thread = 0 Then
                newminthreads = threads_to_run
            Else
                newminthreads = bc_cs_central_settings.num_min_idle_thread
            End If
            If ThreadPool.SetMinThreads(newminthreads, minIOC) Then
                otrace = New bc_cs_activity_log("bc_om_refresh_components", "parallelrefreshmethod_connectionpool", bc_cs_activity_codes.TRACE_ENTRY, "Sucessfully set minimum threads to: " + CStr(newminthreads), certificate)
            Else
                otrace = New bc_cs_activity_log("bc_om_refresh_components", "parallelrefreshmethod_connectionpool", bc_cs_activity_codes.TRACE_ENTRY, "Failed set minimum threads to: " + CStr(newminthreads), certificate)
            End If

            Dim caller As parallelrefreshbatchCaller
            Dim handle(threads_to_run) As WaitHandle

            'Dim t As Thread
            'Dim allthreads As New List(Of Thread)
            'completed_thread_count = 0
            For i = 0 To threads_to_run
                caller = New parallelrefreshbatchCaller(AddressOf parallelrefresh)
                otrace = New bc_cs_activity_log("bc_om_refresh_components", "parallelrefreshmethod starting thread:" + CStr(i), bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
                Dim result As IAsyncResult = caller.BeginInvoke(i, Nothing, Nothing)
                handle(i) = result.AsyncWaitHandle
            Next

            WaitHandle.WaitAll(handle)

            ThreadPool.SetMinThreads(minWorker, minIOC)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_refresh_components", "parallelrefreshmethod_connectionpool", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Me.success = False
        Finally
            otrace = New bc_cs_activity_log("bc_om_refresh_components", "parallelrefreshmethod_connectionpool", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try

    End Sub
    Public completed_thread_count As Integer = 0
    Public Sub parallelrefresh(ByVal batch_id As Integer)
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_components", "parallelrefresh batch: " + CStr(batch_id), bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try

            Dim i As Integer
            For i = 0 To Me.refresh_components.Count - 1
                If Me.refresh_components(i).parallel_batch = batch_id Then
                    refresh_components(i).db_read(Me.pub_type_id, Me.accounting_standard, Me.workflow_state, Me.language_id, Me.data_at_date, Me.doc_id, Me.oselection_parameters.row, Me.oselection_parameters.col, False, Me.assoc_entities, bc_om_refresh_components.loaded_external_proxies, ocached_components, MyBase.certificate, batch_id)
                    stage_change_error_text = refresh_components(i).stage_change_error_text

                    Me.success = refresh_components(i).success
                    If Me.success = False Then
                        Exit Sub
                    End If
                End If
            Next

        Catch ex As Exception
            otrace = New bc_cs_activity_log("bc_om_refresh_components", "parallelrefresh exit batch: " + CStr(batch_id) + ex.Message, bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_refresh_components", "parallelrefresh exit batch: " + CStr(batch_id), bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            completed_thread_count = completed_thread_count + 1
        End Try

    End Sub


    Public Function asyncrefresh(ByVal callDuration As Integer, _
           <Out()> ByRef threadId As Integer) As String
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_components", "asyncrefresh", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        asyncrefresh = False
        Try

            For i = 0 To refresh_components.Count - 1
                If refresh_components(i).disabled = 0 And refresh_components(i).no_refresh = 0 And refresh_components(i).web_service_name <> "" Then
                    refresh_components(i).db_read(Me.pub_type_id, Me.accounting_standard, Me.workflow_state, Me.language_id, Me.data_at_date, Me.doc_id, Me.oselection_parameters.row, Me.oselection_parameters.col, False, Me.assoc_entities, bc_om_refresh_components.loaded_external_proxies, ocached_components, MyBase.certificate, 1)
                    stage_change_error_text = refresh_components(i).stage_change_error_text

                    Me.success = refresh_components(i).success
                    If Me.success = False Then
                        Exit Function
                    End If
                End If
            Next
            asyncrefresh = True
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_refresh_components", "asyncrefresh", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            Me.success = False
        Finally
            otrace = New bc_cs_activity_log("bc_om_refresh_components", "asyncrefresh", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Function

    REM read component metadata
    Public Sub db_read_metadata(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate)
        Try
            Dim otrace As New bc_cs_activity_log("bc_om_refresh_components", "db_read_metadata", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
            Dim i, j As Integer
            MyBase.certificate = certificate
            Dim obc_om_refresh_component As New bc_om_refresh_component
            obc_om_refresh_component.certificate = MyBase.certificate
            Dim orefresh_components As Object
            Dim db_params As New bc_om_component_parameters_db
            Dim vparams As Object
            Dim oparam As bc_om_component_parameter
            Dim gbc As New bc_om_refresh_components_db
            orefresh_components = gbc.get_document_refresh_components(doc_id, MyBase.certificate)
            For i = 0 To UBound(orefresh_components, 2)
                obc_om_refresh_component = New bc_om_refresh_component
                obc_om_refresh_component.certificate = MyBase.certificate
                obc_om_refresh_component.entity_id = orefresh_components(0, i)
                obc_om_refresh_component.locator = orefresh_components(1, i)
                obc_om_refresh_component.contributor_id = orefresh_components(2, i)
                obc_om_refresh_component.last_refresh_date = orefresh_components(3, i)
                obc_om_refresh_component.last_update_date = orefresh_components(4, i)
                obc_om_refresh_component.disabled = orefresh_components(5, i)
                obc_om_refresh_component.type = orefresh_components(6, i)
                obc_om_refresh_component.name = orefresh_components(7, i)
                obc_om_refresh_component.mode = orefresh_components(8, i)
                obc_om_refresh_component.mode_param1 = orefresh_components(9, i)
                obc_om_refresh_component.mode_param2 = orefresh_components(10, i)
                obc_om_refresh_component.refresh_type = orefresh_components(11, i)
                obc_om_refresh_component.web_service_name = orefresh_components(12, i)
                obc_om_refresh_component.format_file = orefresh_components(13, i)
                obc_om_refresh_component.external_id = orefresh_components(14, i)
                obc_om_refresh_component.cache_level = orefresh_components(15, i)
                REM UDCs only
                obc_om_refresh_component.author_id = orefresh_components(16, i)
                obc_om_refresh_component.mode_param3 = orefresh_components(17, i)


                Dim dependent_parameter_values As New List(Of String)

                vparams = db_params.get_component_document_parameters(doc_id, obc_om_refresh_component.locator, certificate)
                If IsArray(vparams) Then
                    If UBound(vparams, 2) >= 0 Then

                        For j = 0 To UBound(vparams, 2)
                            If vparams(2, j) = 10 Then
                                dependent_parameter_values.Add(vparams(6, j))
                            End If
                        Next


                        For j = 0 To UBound(vparams, 2)
                            oparam = New bc_om_component_parameter
                            oparam.certificate = MyBase.certificate
                            oparam.name = vparams(0, j)
                            oparam.order = vparams(1, j)
                            oparam.datatype = vparams(2, j)
                            oparam.class_id = vparams(3, j)
                            oparam.lookup_sql = vparams(4, j)
                            oparam.system_defined = vparams(5, j)
                            oparam.default_value = vparams(6, j)
                            oparam.original_system_defined = vparams(7, j)
                            oparam.disabled_in_doc = vparams(8, j)
                            oparam.mandatory = vparams(9, j)
                            oparam.locator = obc_om_refresh_component.locator
                            oparam.type_id = obc_om_refresh_component.type
                            If oparam.datatype = 15 Or oparam.datatype = 16 Or oparam.datatype = 5 Or oparam.datatype = 6 Or oparam.datatype = 7 Or oparam.datatype = 8 Or oparam.datatype = 9 Or oparam.datatype = 10 Or oparam.datatype = 11 Then
                                REM lookup value
                                If oparam.datatype <> 9 Then
                                    oparam.db_read(obc_om_refresh_component.entity_id, dependent_parameter_values, doc_id)
                                Else
                                    oparam.db_read(obc_om_refresh_component.entity_id, dependent_parameter_values, doc_id, obc_om_refresh_component.parameters.component_template_parameters(0).default_value_id)
                                End If
                            End If

                            obc_om_refresh_component.parameters.component_template_parameters.Add(oparam)
                        Next
                    End If
                End If
                Me.refresh_components.Add(obc_om_refresh_component)
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_refresh_components", "db_read_metadata", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            Dim otrace = New bc_cs_activity_log("bc_om_refresh_components", "db_read_metadata", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    'Public Overloads Overrides Function call_web_service() As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_refresh_components", "call_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    If IsNumeric(bc_cs_central_settings.timeout) Then
    '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout, MyBase.certificate)
    '    End If


    '    call_web_service = webservice.LoadRefreshData(MyBase.write_xml_to_string)
    '    otrace = New bc_cs_activity_log("bc_om_refresh_components", "call_web_service", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    'End Function
    'Public Overrides Function write_xml_via_soap_client_request() As Object


    'End Function
    Public Sub get_data_update_date()
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_components", "get_date_update_date", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim i As Integer
            For i = 0 To Me.refresh_components.Count - 1
                If single_select_mode = False Or (single_select_mode = True And Me.refresh_components(i).single_selected = True) Then
                    Me.refresh_components(i).certificate = MyBase.certificate
                    Me.refresh_components(i).get_data_update_date(Me.workflow_state, Me.accounting_standard, Me.doc_id)
                    If single_select_mode = True And Me.refresh_components(i).single_selected = True Then
                        Me.refresh_components(i).db_read(doc_id)
                    End If
                End If
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_refresh_components", "get_date_update_date", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_refresh_components", "get_date_update_date", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
End Class
REM Cowan Oct 2012
<Serializable()> Public Class bc_om_binary_file_in_cell
    Public objbyte As Byte()
    Public row As Integer
    Public col As Integer
    Public ext As String
End Class
<Serializable()> Public Class bc_om_refresh_component
    Inherits bc_om_sub_component
    REM cowan oct 2012
    REM holds array bc_om_binary_file_in_cell
    Public binary_files As New ArrayList
    Public from_web As Boolean = False
    REM component entity id
    Public entity_id As Long
    Public parent_entity_id As Long
    REM text based document locator
    Public locator As String
    Public locator_description As String
    REM data set are values single cell component
    Public last_refresh_date As Date = "9-9-9999"
    REM last update date
    Public last_update_date As Date = "9-9-9999"
    REM if refresh value is a single value or array
    Public refresh_value As Object
    REM if refresh value is an image as a bytestream
    Public objbyte As Byte()
    REM mark it as not requiring a refresh
    Public no_refresh As Integer = 0
    REM mark it as disabled from refresh
    Public disabled As Integer = 0
    REM restriction mode 0 none 1 do not show 2 show text 3 dont show estimates
    Public restriction_mode As Integer
    REM
    Public restriction_text As String
    Public success As Boolean
    <NonSerialized()> Public err_text As String
    REM tempraily here will end upin base class
    REM markup 
    Public original_markup_colour_index As Long = 0
    REM for UDCS
    Public author_id As Long
    Public single_selected As Boolean = False
    Public tx As String
    Public compressed_html As Byte()
    Public sub_doc_id As Long = 0

    REM FIL 9411
    Public doc_id As String = ""

    REM FIL FEB 2014 single doc parameter screen
    Public doc_parameter_screen_order As Integer = -1
    Public stage_change_error_text As String
    REM ========================================

    REM SIM MAY 2013
    Public images As New ArrayList
    Public Const tREAD_SYSTEM_DEFINED_VALUE As Integer = 2000
    REM ============
    <NonSerialized()> Public parallel_batch As Integer = -1

    Public Sub New()
        success = True
    End Sub
    REM FIL AUG 2013
    Public Overrides Sub process_object()
        If Me.tmode = bc_om_refresh_component.tREAD_SYSTEM_DEFINED_VALUE Then
            Me.read_system_defined_value()
        End If
    End Sub

    REM FIL JUNE 2013
    Public Sub read_system_defined_value()
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_component", "read_system_defined_value", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            For i = 0 To MyBase.parameters.component_template_parameters.Count - 1
                MyBase.parameters.component_template_parameters(i).certificate = MyBase.certificate

                MyBase.parameters.component_template_parameters(i).read_system_defined_value(entity_id, doc_id, MyBase.certificate)
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_refresh_component", "read_system_defined_value", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_refresh_component", "read_system_defined_value", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try

    End Sub

    Private Function write_params_to_xml(ByVal pub_type_id As Long, ByVal acc_id As Long, ByVal stage_id As Long, ByVal language_id As Long, ByVal data_at_date As Date, ByVal doc_id As String, ByVal row As Long, ByVal col As Long) As String
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_components", "write_params_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim xml As String = ""

        Try
            Dim i As Integer
            REM FIL AUG 2013
            If bc_cs_central_settings.server_flag = 0 Then
                certificate.user_id = bc_cs_central_settings.logged_on_user_id
            End If
            xml = "<bc_parameters><core>"
            xml = xml + "<user_id>" + CStr(certificate.user_id) + "</user_id>"
            xml = xml + "<entity_id>" + CStr(entity_id) + "</entity_id>"
            xml = xml + "<pub_type_id>" + CStr(pub_type_id) + "</pub_type_id>"
            xml = xml + "<acc_id>" + CStr(acc_id) + "</acc_id>"
            xml = xml + "<stage_id>" + CStr(stage_id) + "</stage_id>"
            xml = xml + "<language_id>" + CStr(language_id) + "</language_id>"
            xml = xml + "<data_at_date>" + CStr(data_at_date) + "</data_at_date>"
            xml = xml + "<doc_id>" + CStr(doc_id) + "</doc_id>"
            xml = xml + "<row>" + CStr(row) + "</row>"
            xml = xml + "<col>" + CStr(col) + "</col>"
            xml = xml + "<contributor_id>" + CStr(contributor_id) + "</contributor_id>"
            xml = xml + "<external_id>" + CStr(external_id) + "</external_id>"
            xml = xml + "</core>"
            xml = xml + "<custom>"
            For i = 0 To parameters.component_template_parameters.Count - 1
                With parameters.component_template_parameters(i)
                    xml = xml + "<bc_custom_parameter><name>" + .name + "</name>"
                    If .system_defined = True Then
                        xml = xml + "<value></value>"
                    Else
                        If .default_value_id <> "" Then
                            xml = xml + "<value>" + .default_value_id + "</value>"
                        Else
                            xml = xml + "<value>" + .default_value + "</value>"
                        End If
                    End If
                    xml = xml + "</bc_custom_parameter>"
                End With
            Next
            xml = xml + "</custom>"
            xml = xml + "</bc_parameters>"
            Dim ocomm As New bc_cs_activity_log("bc_om_refresh_component", "write_params_to_xml", bc_cs_activity_codes.COMMENTARY, "Param string: " + xml, MyBase.certificate)

        Catch

        Finally
            otrace = New bc_cs_activity_log("bc_om_refresh_components", "write_params_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
            write_params_to_xml = xml
        End Try


    End Function
    REM PR/SW WCF external read AUGUST 2016
    Private Function wcf_external_read(ByVal pub_type_id As Long, ByVal acc_id As Long, ByVal stage_id As Long, ByVal language_id As Long, ByVal data_at_date As Date, ByVal doc_id As String, ByVal row As Long, ByVal col As Long, ByRef disclosures_loaded As Boolean, ByRef new_components As ArrayList, ByVal assoc_entities As ArrayList, ByVal cached As Integer, ByRef cached_components As Object, ByRef loaded_external_proxies As ArrayList, ByVal certificate As bc_cs_security.certificate) As Boolean
        Try
            REM call c# proxy
            Dim result As Object
            Dim wcf_proxy As New bc_wcf_methods

            Dim k As New bc_cs_security.certificate


            result = wcf_proxy.call_method(external_id, doc_id, certificate)

            If wcf_proxy.ErrorText <> "" Then
                stage_change_error_text = wcf_proxy.ErrorText
                certificate.server_errors.Clear()
                certificate.error_state = False
                Exit Function
            End If
            Me.success = False


            If certificate.error_state = True Then
                stage_change_error_text = "Error calling external service: " + certificate.server_errors(0)
                certificate.server_errors.Clear()
                certificate.error_state = False
                Exit Function
            End If

            REM write result to database
            Dim gdb As New bc_om_refresh_components_db
            gdb.write_wcf_response(doc_id, MyBase.type, result, certificate)

            If certificate.error_state = True Then
                stage_change_error_text = "Error writing external service response: " + certificate.server_errors(0)
                certificate.server_errors.Clear()
                certificate.error_state = False
                Exit Function
            End If

            REM mow call internal refresh
            Dim twebservice_name As String
            twebservice_name = Me.web_service_name
            Me.web_service_name = ""
            'now carry out an internal component call
            db_read(pub_type_id, acc_id, stage_id, language_id, data_at_date, doc_id, row, col, disclosures_loaded, assoc_entities, loaded_external_proxies, cached_components, certificate)
            Me.web_service_name = twebservice_name
            Me.success = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_refresh_component", " wcf_external_read", bc_cs_error_codes.USER_DEFINED, Me.err_text + ": " + ex.Message, certificate)
        End Try
    End Function
    REM PR april 2010 to allow external data via webservices
    Public Function external_read(ByVal pub_type_id As Long, ByVal acc_id As Long, ByVal stage_id As Long, ByVal language_id As Long, ByVal data_at_date As Date, ByVal doc_id As String, ByVal row As Long, ByVal col As Long, ByRef disclosures_loaded As Boolean, ByRef new_components As ArrayList, ByVal assoc_entities As ArrayList, ByVal cached As Integer, ByRef cached_components As Object, ByRef loaded_external_proxies As ArrayList, ByVal certificate As bc_cs_security.certificate) As Object
        external_read = Nothing

        Try
            If MyBase.web_service_name = "WCF" Then
                Return wcf_external_read(pub_type_id, acc_id, stage_id, language_id, data_at_date, doc_id, row, col, disclosures_loaded, new_components, assoc_entities, cached, cached_components, loaded_external_proxies, certificate)

            End If
            REM tmp setting
            Dim ocomm As bc_cs_activity_log
            Dim res As Object
            Dim oxml As String = ""
            Dim loaded As Boolean = False

            success = True
            Me.objbyte = Nothing
            REM check if data is alread cached
            If bc_cs_central_settings.server_flag = 0 Then
                MyBase.certificate.user_id = bc_cs_central_settings.logged_on_user_id
                MyBase.certificate.os_name = bc_cs_central_settings.GetLoginName
                MyBase.certificate.name = bc_cs_central_settings.logged_on_user_name
            End If
            If cached = -1 Then
                ocomm = New bc_cs_activity_log("bc_om_refresh_component", "external_read", bc_cs_activity_codes.COMMENTARY, "Getting data from webservice for locator:" + Me.locator, MyBase.certificate)
                REM new dynamic method
                Dim oconn As New bc_cs_third_party_web_service
                res = oconn.call_web_service_dynamic(MyBase.mode_param1, MyBase.web_service_name, external_id, entity_id, pub_type_id, acc_id, stage_id, language_id, data_at_date, doc_id, row, col, contributor_id, parameters, False, MyBase.certificate, Me.write_params_to_xml(pub_type_id, acc_id, stage_id, language_id, data_at_date, doc_id, row, col))
                If oconn.err = True Then
                    Me.objbyte = Nothing
                    success = False
                    Me.err_text = oconn.error_text
                    external_read = Nothing
                    Exit Function
                End If

                If IsNothing(res) = False Then
                    If LCase(res.GetType.ToString) = "system.string" Then
                        oxml = CStr(res)
                    ElseIf LCase(res.GetType.ToString) = "system.byte[]" Then
                        Me.objbyte = res
                        external_read = Nothing
                        Exit Function
                    ElseIf LCase(res.GetType.ToString) = "system.boolean" Then
                        Try
                            If res = True Then
                                oxml = "true"
                            Else
                                oxml = "false"
                            End If
                        Catch
                            Me.err_text = "invalid boolean type returned from webservice: " + CStr(res.GetType.ToString)
                            success = False
                            external_read = Nothing
                            Exit Function
                        End Try
                    Else
                        Me.err_text = "invalid data type returned from webservice: " + CStr(res.GetType.ToString)
                        success = False
                        external_read = Nothing
                        Exit Function
                    End If
                Else
                    Me.err_text = "nothing returned from webservice"
                    success = False
                End If

                REM if component requires caching add to caching table
                If Me.cache_level > 0 Then
                    Dim cache_component As New bc_om_cached_component
                    cache_component.method = MyBase.web_service_name + Me.mode_param1
                    cache_component.parameters = Me.parameters
                    cache_component.contributor_id = Me.contributor_id
                    cache_component.external_id = Me.external_id
                    cache_component.entity_id = Me.entity_id
                    cache_component.external_xml = oxml
                    cached_components.add(cache_component)
                    ocomm = New bc_cs_activity_log("bc_om_refresh_component", "external_read", bc_cs_activity_codes.COMMENTARY, "Adding data for locator: " + Me.locator + " to cache", MyBase.certificate)
                End If
            ElseIf cached > -1 Then
                REM get data from the cache
                ocomm = New bc_cs_activity_log("bc_om_refresh_component", "external_read", bc_cs_activity_codes.COMMENTARY, "Getting data from cache for locator:" + Me.locator, MyBase.certificate)
                oxml = cached_components(cached).external_xml
            End If

            REM now we need to translate xml
            Dim fs As New bc_cs_file_transfer_services
            Dim err_text As String
            REM add parameter to external data and wrap external data
            oxml = "<all_data><external_data>" + oxml + "</external_data>" + Me.write_params_to_xml(pub_type_id, acc_id, stage_id, language_id, data_at_date, doc_id, row, col) + "</all_data>"
            If bc_cs_central_settings.log_external_data = 1 Then
                ocomm = New bc_cs_activity_log("bc_om_refresh_component", "external_read", bc_cs_activity_codes.COMMENTARY, "Data Before Transformtaion: " + oxml, MyBase.certificate)
            End If
            err_text = translate_xml(oxml)
            If err_text <> "" Then
                success = False
                external_read = Nothing
                Exit Function
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_refresh_component", "external_read", bc_cs_error_codes.USER_DEFINED, Me.err_text + ": " + ex.Message, certificate)
            external_read = Nothing
        Finally

        End Try

    End Function
    Private Function translate_xml(ByVal xml_native As String) As String
        '        REM mode 1 single cell, 2 table, 3 chart, 4 db image, 5 fs image, 6 index, 7 document insertion from db, 12, file system non entity image
        '        REM 1 <value>
        '        REM 2,6 <value><row><col><style>
        '        REM 3 <value><row><col>
        '        REM 4,5,7,12 <value> blob
        Dim xmldoc As New XmlDocument
        Dim xmlNReader As XmlNodeReader
        Dim xsltran As New System.Xml.Xsl.XslCompiledTransform
        Dim outdata As New Object
        Dim xslsettings As New System.Xml.Xsl.XsltSettings(True, False)
        Try
            REM firstly try and load xslt cannot proceed without correct formed xslt
            Try
                If Me.format_file = "" Then
                    err_text = "No xslt file for component: " + Me.locator
                    Exit Function
                End If
                If bc_cs_central_settings.log_external_data = 1 Then
                    Dim ocomm As New bc_cs_activity_log("bc_om_refresh_component", "translate_xml", bc_cs_activity_codes.COMMENTARY, "Attempting to load XSLT file: " + Me.format_file, MyBase.certificate)
                End If

                xsltran.Load(bc_cs_central_settings.central_template_path + Me.format_file, xslsettings, New XmlUrlResolver)
            Catch ex As Exception
                err_text = "Error with xslt file: " + Me.format_file + ":" + ex.Message
                Exit Function
            End Try
            Try
                xml_native = xml_native.Replace("<?xml version=""1.0"" encoding=""utf-16""?>", "")
                xmldoc.LoadXml(xml_native)
            Catch ex As Exception
                err_text = "error loading native xml: " + ex.Message
                Exit Function
            End Try
            REM now apply the transformation to the xml
            Dim memStream As New MemoryStream
            Dim xmlsettings As New XmlWriterSettings
            xmlsettings.Encoding = New System.Text.UTF8Encoding(False)
            Dim strWriter As New StringWriter
            Dim outDoc = XmlWriter.Create(memStream, xmlsettings)
            Try
                xmlNReader = New XmlNodeReader(xmldoc)
                xsltran.Transform(xmlNReader, outDoc)
                outDoc.Flush()
                outDoc.Close()
                If bc_cs_central_settings.log_external_data = 1 Then
                    Dim ocomm As New bc_cs_activity_log("bc_om_refresh_component", "tranlate_xml", bc_cs_activity_codes.COMMENTARY, "XML after transformation: " + System.Text.Encoding.UTF8.GetString(memStream.ToArray), MyBase.certificate)
                End If
            Catch ex As Exception
                err_text = "unable to transform xml: " + ex.Message
                Exit Function
            End Try
            REM now load the transformed xml
            Dim outxmldoc As New XmlDocument
            Try
                outxmldoc.LoadXml(System.Text.Encoding.UTF8.GetString(memStream.ToArray))
            Catch ex As Exception
                err_text = "unable to load transformed xml from webservice: " + ex.Message
                Exit Function
            End Try
            REM now parse the output into object
            Dim myXmlNodeList As Xml.XmlNodeList
            Dim myXmlNode As Xml.XmlNode
            Dim i As Integer

            myXmlNodeList = outxmldoc.SelectNodes("/data/success")
            If myXmlNodeList.Count = 1 Then
                If myXmlNodeList(0).InnerXml = 1 Then
                    err_text = "error with returned xml"
                    myXmlNodeList = outxmldoc.SelectNodes("/data/error_text")
                    If myXmlNodeList.Count = 1 Then
                        Me.err_text = myXmlNodeList(0).InnerXml
                    End If
                    Exit Function
                End If
            End If
            myXmlNodeList = outxmldoc.SelectNodes("/data/element/value")
            Select Case Me.mode
                Case 1, 2, 3, 5, 12
                    i = 0
                    ReDim outdata(4, myXmlNodeList.Count - 1)
                    For Each myXmlNode In myXmlNodeList
                        Dim ts As String
                        ts = myXmlNode.InnerXml
                        ts = ts.Replace("&amp;", "&")
                        ts = ts.Replace("&gt;", ">")
                        ts = ts.Replace("&lt;", "<")
                        outdata(0, i) = ts
                        REM get rid of special chars

                        i = i + 1
                    Next
                    i = 0
                    myXmlNodeList = outxmldoc.SelectNodes("/data/element/row")
                    For Each myXmlNode In myXmlNodeList
                        outdata(1, i) = myXmlNode.InnerXml
                        i = i + 1
                    Next
                    i = 0
                    myXmlNodeList = outxmldoc.SelectNodes("/data/element/col")
                    For Each myXmlNode In myXmlNodeList
                        outdata(2, i) = myXmlNode.InnerXml
                        i = i + 1
                    Next
                    i = 0
                    myXmlNodeList = outxmldoc.SelectNodes("/data/element/style")
                    For Each myXmlNode In myXmlNodeList
                        outdata(3, i) = myXmlNode.InnerXml
                        i = i + 1
                    Next
                    Me.refresh_value = outdata
                    err_text = ""
                Case 4, 7
                    err_text = "data must be returned as a bytestream from webservice directly"
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_refresh_component", "translate_xml", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally


            translate_xml = err_text
        End Try
    End Function
    Private Function is_cached(ByVal cached_components As ArrayList) As Integer
        REM determines if components data is already cached
        Dim i, j As Integer
        is_cached = -1
        If Me.cache_level = 1 Then
            REM method contribuor_Id and  external id only
            For i = 0 To cached_components.Count - 1
                If Me.web_service_name + Me.mode_param1 = cached_components(i).method And Me.external_id = cached_components(i).external_id And Me.contributor_id = cached_components(i).contributor_id And Me.entity_id = cached_components(i).entity_id Then
                    is_cached = i
                    If cache_level = 1 Then
                        Exit Function
                    End If
                End If
            Next
        ElseIf Me.cache_level = 2 Then
            For i = 0 To cached_components.Count - 1
                If Me.web_service_name + Me.mode_param1 = cached_components(i).method And Me.external_id = cached_components(i).external_id And Me.contributor_id = cached_components(i).contributor_id And Me.entity_id = cached_components(i).entity_id Then
                    If parameters.component_template_parameters.Count = 0 Then
                        is_cached = i
                        Exit Function
                    End If
                    REM now check parameter counts are the same
                    If parameters.component_template_parameters.Count = cached_components(i).parameters.component_template_parameters.Count Then
                        REM now check parameter values are the same
                        For j = 0 To parameters.component_template_parameters.Count - 1
                            If parameters.component_template_parameters(j).default_value <> cached_components(i).parameters.component_template_parameters(j).default_value Then
                                Exit For
                            End If
                            If j = parameters.component_template_parameters.Count - 1 Then
                                is_cached = i
                                Exit Function
                            End If
                        Next
                    End If
                End If
            Next
        End If
    End Function
    Public Shadows Sub db_read(ByVal doc_id As String)
        Dim db As New bc_om_refresh_components_db
        Select Case MyBase.mode
            Case 8, 9, 88, 888, 80, 880, 8880
                REM editable SDC
                REM UDC
                Dim ocommentary As New bc_cs_activity_log("bc_om_refresh_component", "db_read", bc_cs_activity_codes.COMMENTARY, "retreiving data for SDC:" + CStr(Me.type), certificate)
                Me.refresh_value = db.get_value_for_udc(doc_id, Me.type, Me.entity_id, MyBase.certificate)
            Case 100
                REM UDC
                Dim ocommentary As New bc_cs_activity_log("bc_om_refresh_component", "db_read", bc_cs_activity_codes.COMMENTARY, "retreiving data for UDC:" + CStr(Me.type), certificate)
                Me.refresh_value = db.get_value_for_sdc(Me.type, MyBase.certificate)
        End Select

    End Sub
    Public Shadows Sub db_read(ByVal pub_type_id As Long, ByVal acc_id As Long, ByVal stage_id As Long, ByVal language_id As Long, ByVal data_at_date As Date, ByVal doc_id As String, ByVal row As Long, ByVal col As Long, ByRef disclosures_loaded As Boolean, ByVal assoc_entities As ArrayList, ByRef loaded_external_proxies As ArrayList, ByRef cached_components As ArrayList, ByVal certificate As bc_cs_security.certificate, Optional ByVal thread As Integer = 0)
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_component", "db_read component: " + CStr(MyBase.type) + ", thread: " + CStr(thread), bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim db As New bc_om_refresh_components_db
        Dim ocommentary As bc_cs_activity_log
        Dim i, j As Integer
        Dim oassoc As Object = Nothing
        Dim cached As Integer = 0
        REM Cowan Oct 2012#

        Me.binary_files = New ArrayList

        MyBase.certificate = certificate
        Try
            If MyBase.contributor_id = 0 Then
                MyBase.contributor_id = 1
            End If
            Me.err_text = ""
            Me.new_components.Clear()
            get_restrictions(MyBase.type)
            Select Case MyBase.mode
                Case 1000, 1001
                    REM dynamic components
                    Me.refresh_value = db.get_value_for_component(MyBase.type, entity_id, acc_id, stage_id, pub_type_id, MyBase.contributor_id, language_id, data_at_date, doc_id, MyBase.parameters, MyBase.mode_param1, row, col, parent_entity_id, certificate, from_web)
                    REM now evaluate all the new components
                    build_new_components(Me.refresh_value, Me.locator, pub_type_id, acc_id, stage_id, language_id, data_at_date, doc_id, row, col, disclosures_loaded, assoc_entities, loaded_external_proxies, cached_components, certificate)

                Case 8, 9, 88, 888, 80, 880, 8880
                    REM SDC edit componet 8 text 9 rtf
                    ocommentary = New bc_cs_activity_log("bc_om_refresh_component", "db_read", bc_cs_activity_codes.COMMENTARY, "retreiving data for SDC:" + CStr(Me.type) + "type: " + CStr(Me.mode), certificate)
                    Me.refresh_value = db.get_value_for_udc(doc_id, Me.type, Me.entity_id, MyBase.certificate)
                Case 100
                    REM UDC
                    ocommentary = New bc_cs_activity_log("bc_om_refresh_component", "db_read", bc_cs_activity_codes.COMMENTARY, "retreiving data for UDC:" + CStr(Me.type), certificate)
                    Me.refresh_value = db.get_value_for_sdc(Me.type, MyBase.certificate)
                    REM 10 behaves as a 1 with write down
                Case 1, 2, 3, 10, 11, 21, 26, 31, 111, 110, 210, 260
                    REM hook into comply system if disclosure types to set text elements
                    If Me.refresh_type = 2 And disclosures_loaded = False Then
                        REM if common platform is not set then assume it is not being used for disclosures
                        If bc_cs_central_settings.common_platform <> "" Then
                            ocommentary = New bc_cs_activity_log("bc_om_refresh_component", "db_read", bc_cs_activity_codes.COMMENTARY, "Integrating into Comply", certificate)
                            Me.success = retrieve_disclosure_text(doc_id)
                            If Me.success = False Then
                                Exit Sub
                            End If
                        Else
                            Me.success = True
                        End If
                        disclosures_loaded = True
                    End If
                    If Me.web_service_name <> "" Then
                        Me.refresh_value = Nothing
                        If cache_level = 0 Then
                            REM always request again
                            cached = -1
                        Else
                            REM see if already cached
                            cached = is_cached(cached_components)
                        End If
                        Me.external_read(pub_type_id, acc_id, stage_id, language_id, data_at_date, doc_id, row, col, disclosures_loaded, new_components, assoc_entities, cached, cached_components, loaded_external_proxies, certificate)
                        If success = False Then
                            Dim oerr As New bc_cs_error_log("bc_om_refresh_component", "db_read: mode " + CStr(type) + ", thread " + CStr(thread), bc_cs_error_codes.USER_DEFINED, "Failed to get external data for component at locator: " + Me.locator + ": " + Me.err_text, MyBase.certificate)
                        End If
                        If stage_change_error_text <> "" Then
                            success = False
                        Else
                            success = True
                        End If
                    Else
                        REM single cell, standard display tables
                        If sub_doc_id <> 0 Then
                            Me.refresh_value = db.get_value_for_component(MyBase.type, entity_id, acc_id, stage_id, pub_type_id, MyBase.contributor_id, language_id, data_at_date, sub_doc_id, MyBase.parameters, MyBase.mode_param1, row, col, parent_entity_id, certificate, from_web)
                        Else
                            Me.refresh_value = db.get_value_for_component(MyBase.type, entity_id, acc_id, stage_id, pub_type_id, MyBase.contributor_id, language_id, data_at_date, doc_id, MyBase.parameters, MyBase.mode_param1, row, col, parent_entity_id, certificate, from_web)
                        End If

                        If Me.mode = 2 Or Me.mode = 21 And Not IsNothing(refresh_value) Then
                            If IsArray(refresh_value) Then
                                For i = 0 To UBound(refresh_value, 2)
                                    If Len(refresh_value(0, i)) > 7 Then
                                        If Left(refresh_value(0, i), 7) = "[image]" Then

                                            Dim fn As String
                                            fn = refresh_value(0, i)
                                            fn = Replace(fn, "[image]", "")
                                            fn = Replace(fn, "<repos>", bc_cs_central_settings.central_repos_path) + "." + refresh_value(3, i)

                                            Dim oimage As New bc_om_binary_file_in_cell
                                            Dim fs As New bc_cs_file_transfer_services
                                            If fs.write_document_to_bytestream(fn, oimage.objbyte, certificate, False) = False Then
                                                Dim oocomm As New bc_cs_activity_log("bc_om_refresh_component", "db_read", bc_cs_activity_codes.COMMENTARY, "failed to load file: " + fn, certificate)
                                            Else
                                                Dim ocomm As New bc_cs_activity_log("bc_om_refresh_component", "db_read", bc_cs_activity_codes.COMMENTARY, "successfully loaded file: " + fn, certificate)
                                                oimage.row = refresh_value(1, i)
                                                oimage.col = refresh_value(2, i)
                                                oimage.ext = refresh_value(3, i)
                                                Me.binary_files.Add(oimage)
                                            End If
                                        End If
                                    End If
                                Next
                            End If
                        End If
                    End If
                    If Me.web_service_name = "" Then
                        REM if parameter is a type 9 then update context items
                        For j = 0 To Me.parameters.component_template_parameters.Count - 1
                            With Me.parameters.component_template_parameters(j)
                                If .datatype = 9 Then
                                    Dim oparam_values As New bc_om_parameter_lookup
                                    oparam_values.type_id = .datatype
                                    oparam_values.lookup_sql = .lookup_sql
                                    oparam_values.entity_id = Me.entity_id
                                    oparam_values.doc_id = doc_id
                                    oparam_values.certificate = certificate

                                    REM assune context_id is parameter 1
                                    oparam_values.context_id = Me.parameters.component_template_parameters(0).default_value_id
                                    oparam_values.db_read()
                                    .lookup_values.Clear()
                                    .lookup_values = oparam_values.lookup_vals
                                    .lookup_values_ids = oparam_values.lookup_vals_ids
                                    .context_items = oparam_values.context_items
                                End If
                            End With
                        Next
                    End If
                    REM core change nov 2009 if number of companies is above a specified amount
                    REM specify an alternate insert file
                    Dim vres As Object
                    Dim use_alternate_list As Boolean = False
                    Dim no_master As Boolean = False
                    Dim max_number_default = 0
                    Dim alternate_chart As String = ""


                    REM specifies whetehr master entity is treated seperatle
                    If (MyBase.mode = 3) And Me.refresh_type = 2 Then
                        vres = db.get_extended_disclosure_params(MyBase.certificate)
                        If IsArray(vres) Then
                            Try
                                no_master = CBool(vres(0, 0))
                                max_number_default = vres(1, 0)
                                alternate_chart = vres(2, 0)
                                use_alternate_list = vres(3, 0)
                            Catch
                                If vres(0, 0) <> "Error" Then
                                    no_master = vres(0, 0)
                                    max_number_default = vres(1, 0)
                                    alternate_chart = vres(2, 0)
                                    use_alternate_list = vres(3, 0)
                                End If
                            End Try
                        End If
                        If Me.entity_id = 0 Or no_master = True Then
                            Me.refresh_value = Nothing
                        End If
                        Try
                            If use_alternate_list = False Then
                                oassoc = db.get_assoc_entities(doc_id, MyBase.certificate)
                            Else
                                oassoc = db.get_alternate_assoc_entities(doc_id, MyBase.certificate)
                            End If
                        Catch

                        End Try


                        REM core change nov 2009 if number of companies is above a specified amount
                        REM specify an alternate insert file
                        If IsArray(oassoc) Then
                            For i = 0 To UBound(oassoc, 2)
                                If oassoc(0, i) <> Me.entity_id Or no_master = True Then
                                    ocommentary = New bc_cs_activity_log("bc_om_refresh_component", "db_read", bc_cs_activity_codes.COMMENTARY, "Generating Supplementary Display: " + CStr(Me.type) + " for entity id:" + CStr(oassoc(0, i)), MyBase.certificate)
                                    Dim orefresh As New bc_om_refresh_component
                                    orefresh.certificate = MyBase.certificate
                                    orefresh.locator = ""
                                    orefresh.entity_id = oassoc(0, i)
                                    orefresh.parameters = Me.parameters
                                    orefresh.name = Me.name
                                    orefresh.mode = Me.mode
                                    orefresh.web_service_name = Me.web_service_name
                                    orefresh.format_file = Me.format_file
                                    orefresh.external_id = Me.external_id
                                    orefresh.cache_level = 0
                                    orefresh.mode_param1 = Me.mode_param1
                                    If MyBase.mode = 3 Then
                                        If max_number_default > 0 And alternate_chart <> "" And max_number_default <= UBound(oassoc, 2) Then
                                            orefresh.mode_param2 = alternate_chart
                                        Else
                                            orefresh.mode_param2 = Me.mode_param2
                                        End If
                                    End If
                                    orefresh.type = Me.type
                                    orefresh.contributor_id = Me.contributor_id
                                    orefresh.disabled = False
                                    If orefresh.web_service_name <> "" Then
                                        orefresh.refresh_value = Nothing
                                        REM always request again
                                        cached = -1
                                        orefresh.external_read(pub_type_id, acc_id, stage_id, language_id, data_at_date, doc_id, row, col, disclosures_loaded, new_components, assoc_entities, cached, cached_components, loaded_external_proxies, certificate)
                                        If success = False Then
                                            Dim oerr As New bc_cs_error_log("bc_om_refresh_component", "db_read", bc_cs_error_codes.USER_DEFINED, "Failed to get external data for component at locator: " + Me.locator + ": " + Me.err_text, MyBase.certificate)
                                        End If

                                        success = True
                                    Else
                                        If MyBase.mode = 3 Then
                                            orefresh.refresh_value = db.get_value_for_component(MyBase.type, CLng(oassoc(0, i)), acc_id, stage_id, pub_type_id, MyBase.contributor_id, language_id, data_at_date, doc_id, MyBase.parameters, MyBase.mode_param1, row, col, parent_entity_id, certificate, from_web)
                                        Else
                                            Dim vArrtribute As Object
                                            Dim vImageName As String = ""
                                            Dim fn As String
                                            Dim sfn As New bc_cs_file_transfer_services
                                            vArrtribute = db.get_component_image(entity_id, Me.mode_param3, MyBase.certificate)
                                            If IsArray(vArrtribute) Then
                                                vImageName = CStr(vArrtribute(0, 0))
                                            End If
                                            fn = bc_cs_central_settings.external_files_path + Me.mode_param1 + "\" + vImageName + CStr(Me.mode_param2)
                                            If sfn.check_document_exists(fn) Then
                                                sfn.write_document_to_bytestream(fn, Me.objbyte, MyBase.certificate)
                                            End If

                                        End If
                                    End If
                                    Me.new_components.Add(orefresh)
                                    REM if master entity class doesnt match associated entity class flag
                                    REM to clear master data
                                End If
                            Next
                        End If
                    End If


                Case 4, 7
                    REM database image
                    If Me.web_service_name <> "" Then
                        Me.refresh_value = Nothing
                        Me.objbyte = Nothing
                        If cache_level = 0 Then
                            REM always request again
                            cached = -1
                        Else
                            REM see if already cached
                            cached = is_cached(cached_components)
                        End If
                        Me.external_read(pub_type_id, acc_id, stage_id, language_id, data_at_date, doc_id, row, col, disclosures_loaded, new_components, assoc_entities, cached, cached_components, loaded_external_proxies, certificate)
                        If success = False Then
                            Dim oerr As New bc_cs_error_log("bc_om_refresh_component", "db_read", bc_cs_error_codes.USER_DEFINED, "Failed to get external data for component at locator: " + Me.locator + ": " + Me.err_text, MyBase.certificate)
                        End If
                        success = True
                        Exit Sub
                    Else
                        Dim oimage As Object
                        oimage = db.get_image(Me.mode_param1, entity_id, MyBase.certificate)
                        If IsArray(oimage) Then
                            If UBound(oimage, 2) > -1 Then
                                Me.objbyte = oimage(0, 0)
                            End If
                        End If
                    End If
                Case 5
                    Dim fn As String = ""
                    If Me.web_service_name <> "" Then
                        Me.refresh_value = Nothing
                        If cache_level = 0 Then
                            REM always request again
                            cached = -1
                        Else
                            REM see if already cached
                            cached = is_cached(cached_components)
                        End If
                        Me.external_read(pub_type_id, acc_id, stage_id, language_id, data_at_date, doc_id, row, col, disclosures_loaded, new_components, assoc_entities, cached, cached_components, loaded_external_proxies, certificate)

                        Try
                            fn = Me.refresh_value(0, 0)
                        Catch
                            success = False
                            Me.err_text = "could not resolve a filename refresh_value not string"
                        End Try
                        If success = False Then
                            Dim oerr As New bc_cs_error_log("bc_om_refresh_component", "db_read", bc_cs_error_codes.USER_DEFINED, "Failed to get external data for component at locator: " + Me.locator + ": " + Me.err_text, MyBase.certificate)
                        End If
                        success = True
                        Me.refresh_value = Nothing
                    Else
                        REM file system image entity based 
                        REM images must be stored in dirctory below network repos
                        REM using param 1 as directory and param 2 as mime type.
                        REM first try with entity id
                        fn = bc_cs_central_settings.external_files_path + Me.mode_param1 + "\" + CStr(entity_id) + CStr(Me.mode_param2)
                    End If
                    Dim sfn As New bc_cs_file_transfer_services
                    If sfn.check_document_exists(fn) Then
                        sfn.write_document_to_bytestream(fn, Me.objbyte, MyBase.certificate)
                    ElseIf Me.web_service_name = "" Then

                        'Dim vticker As Object
                        'Dim ticker As String = ""
                        'REM now try with Ticker
                        'vticker = db.get_ticker(entity_id, MyBase.certificate)
                        'If IsArray(vticker) Then
                        '    ticker = CStr(vticker(0, 0))
                        'End If
                        'fn = bc_cs_central_settings.central_repos_path + Me.mode_param1 + "\" + ticker + CStr(Me.mode_param2)
                        'If sfn.check_document_exists(fn) Then
                        '    sfn.write_document_to_bytestream(fn, Me.objbyte, MyBase.certificate)
                        'End If

                        REM now try finding the component by arrtribute id
                        Dim vArrtribute As Object
                        Dim vImageName As String = ""
                        Dim params As New List(Of String)
                        For j = 0 To Me.parameters.component_template_parameters.Count - 1
                            params.Add(Me.parameters.component_template_parameters(j).default_value_id)
                        Next
                        vArrtribute = db.get_component_image_bytype(entity_id, pub_type_id, doc_id, Me.type, params, MyBase.certificate)

                        Try
                            If IsArray(vArrtribute) Then
                                vImageName = CStr(vArrtribute(0, 0))
                            End If

                            fn = bc_cs_central_settings.external_files_path + Me.mode_param1 + "\" + vImageName + CStr(Me.mode_param2)
                            If sfn.check_document_exists(fn) Then
                                sfn.write_document_to_bytestream(fn, Me.objbyte, MyBase.certificate)
                            End If
                        Catch
                            vImageName = ""
                        End Try

                    End If

                    REM 5 refresh_type = 2
                    Dim vres As Object
                    Dim use_alternate_list As Boolean = False
                    Dim no_master As Boolean = False
                    Dim max_number_default = 0
                    Dim alternate_chart As String = ""
                    If (MyBase.mode = 5) And Me.refresh_type = 2 Then
                        vres = db.get_extended_disclosure_params(MyBase.certificate)
                        If IsArray(vres) Then
                            Try
                                no_master = CBool(vres(0, 0))
                                max_number_default = vres(1, 0)
                                alternate_chart = vres(2, 0)
                                use_alternate_list = vres(3, 0)
                            Catch
                                If vres(0, 0) <> "Error" Then
                                    no_master = vres(0, 0)
                                    max_number_default = vres(1, 0)
                                    alternate_chart = vres(2, 0)
                                    use_alternate_list = vres(3, 0)
                                End If
                            End Try
                        End If
                        If Me.entity_id = 0 Or no_master = True Then
                            Me.objbyte = Nothing
                        End If
                        If use_alternate_list = False Then
                            oassoc = db.get_assoc_entities(doc_id, MyBase.certificate)
                        Else
                            oassoc = db.get_alternate_assoc_entities(doc_id, MyBase.certificate)
                        End If

                        If IsArray(oassoc) Then
                            'new_components.Clear()
                            For i = 0 To UBound(oassoc, 2)
                                If oassoc(0, i) <> Me.entity_id Or no_master = True Then
                                    ocommentary = New bc_cs_activity_log("bc_om_refresh_component", "db_read", bc_cs_activity_codes.COMMENTARY, "Generating Supplementary Display: " + CStr(Me.type) + " for entity id:" + CStr(oassoc(0, i)), MyBase.certificate)
                                    Dim orefresh As New bc_om_refresh_component
                                    orefresh.certificate = MyBase.certificate
                                    orefresh.locator = ""
                                    orefresh.entity_id = oassoc(0, i)
                                    orefresh.parameters = Me.parameters
                                    orefresh.name = Me.name
                                    orefresh.mode = Me.mode
                                    orefresh.web_service_name = Me.web_service_name
                                    orefresh.format_file = Me.format_file
                                    orefresh.external_id = Me.external_id
                                    orefresh.cache_level = 0
                                    orefresh.mode_param1 = Me.mode_param1
                                    orefresh.mode_param2 = Me.mode_param2

                                    orefresh.type = Me.type
                                    orefresh.contributor_id = Me.contributor_id
                                    orefresh.disabled = False
                                    orefresh.mode_param3 = Me.mode_param3

                                    If Me.mode_param3 <> "0" And (max_number_default = 0 Or Me.mode_param2 <> ".txt" Or (max_number_default >= UBound(oassoc, 2) And Me.mode_param2 = ".txt")) Then
                                        Dim vArrtribute As Object
                                        Dim vImageName As String = ""
                                        'vArrtribute = db.get_component_image(orefresh.entity_id, Me.mode_param3, MyBase.certificate)
                                        Dim params As New List(Of String)
                                        For j = 0 To Me.parameters.component_template_parameters.Count - 1
                                            params.Add(Me.parameters.component_template_parameters(j).default_value_id)
                                        Next
                                        vArrtribute = db.get_component_image_bytype(orefresh.entity_id, pub_type_id, Me.type, Me.doc_id, params, MyBase.certificate)

                                        If IsArray(vArrtribute) And vArrtribute.Length > 0 Then
                                            vImageName = CStr(vArrtribute(0, 0))
                                            fn = bc_cs_central_settings.external_files_path + Me.mode_param1 + "\" + vImageName + CStr(Me.mode_param2)
                                            If sfn.check_document_exists(fn) Then
                                                orefresh.objbyte = Nothing
                                                sfn.write_document_to_bytestream(fn, orefresh.objbyte, MyBase.certificate)
                                                Me.new_components.Add(orefresh)
                                            Else
                                                orefresh.objbyte = Nothing
                                                'Dim encoding As New System.Text.UTF8Encoding()
                                                'orefresh.objbyte = encoding.GetBytes(" ")
                                                Me.new_components.Add(orefresh)
                                            End If
                                        End If
                                    End If
                                End If
                            Next
                        End If
                    End If




                Case 12
                    Dim fn As String
                    If Me.web_service_name <> "" Then
                        Me.refresh_value = Nothing
                        If cache_level = 0 Then
                            REM always request again
                            cached = -1
                        Else
                            REM see if already cached
                            cached = is_cached(cached_components)
                        End If
                        Me.external_read(pub_type_id, acc_id, stage_id, language_id, data_at_date, doc_id, row, col, disclosures_loaded, new_components, assoc_entities, cached, cached_components, loaded_external_proxies, certificate)

                        Try
                            fn = Me.refresh_value(0, 0)
                        Catch
                            success = False
                            Me.err_text = "could not resolve a filename refresh_value not string"
                        End Try
                        If success = False Then
                            Dim oerr As New bc_cs_error_log("bc_om_refresh_component", "db_read", bc_cs_error_codes.USER_DEFINED, "Failed to get external data for component at locator: " + Me.locator + ": " + Me.err_text, MyBase.certificate)
                        End If
                        success = True
                        Me.refresh_value = Nothing

                    Else
                        fn = bc_cs_central_settings.external_files_path + Me.mode_param1 + "\" + CStr(Me.mode_param2)
                        Dim sfn As New bc_cs_file_transfer_services
                        If sfn.check_document_exists(fn) Then
                            sfn.write_document_to_bytestream(fn, Me.objbyte, MyBase.certificate)
                        End If
                    End If
                Case 6
                    If Me.web_service_name <> "" Then
                        Dim oerr As New bc_cs_error_log("bc_om_refresh_component", "db_read", bc_cs_error_codes.USER_DEFINED, "Index table supported from external web service for locator:" + Me.locator, MyBase.certificate)
                    Else
                        REM index table display
                        Me.refresh_value = db.get_value_for_component(MyBase.type, entity_id, acc_id, stage_id, pub_type_id, MyBase.contributor_id, language_id, data_at_date, doc_id, MyBase.parameters, MyBase.mode_param1, row, col, parent_entity_id, MyBase.certificate, from_web)
                    End If
            End Select
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_refresh_component", "db_read: async: " + CStr(thread), bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_refresh_component", "db_read component: " + CStr(MyBase.type) + ", thread: " + CStr(thread), bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Private Sub get_restrictions(ByVal type As Long)
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_component", "get_restrictions", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM firstly get restrictions
            Dim db As New bc_om_refresh_components_db

            Me.restriction_mode = 0
            Me.restriction_text = ""
            Dim vres As Object
            vres = db.get_restrictions_for_component(Me.entity_id, type, MyBase.certificate)
            If IsArray(vres) Then
                Me.restriction_mode = CInt(vres(0, 0))
                Me.restriction_text = CStr(vres(1, 0))
            End If

        Catch ex As Exception
            'Dim db_err As New bc_cs_error_log("bc_om_refresh_component", "get_restrictions", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_refresh_component", "get_restrictions", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    Private Sub build_new_components(ByVal components As Object, ByVal master_locator As String, ByVal pub_type_id As Long, ByVal acc_id As Long, ByVal stage_id As Long, ByVal language_id As Long, ByVal data_at_date As Date, ByVal doc_id As String, ByVal row As Long, ByVal col As Long, ByVal disclosures_loaded As Boolean, ByVal assoc_entities As ArrayList, ByVal loaded_external_proxies As ArrayList, ByVal cached_components As ArrayList, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_component", "build_new_components", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try

            Dim orefresh As bc_om_refresh_component
            If IsArray(components) Then
                For i = 0 To UBound(components, 2)
                    orefresh = New bc_om_refresh_component
                    orefresh.certificate = MyBase.certificate
                    orefresh.mode = components(0, i)
                    orefresh.sub_doc_id = components(1, i)
                    orefresh.entity_id = components(2, i)
                    orefresh.mode_param1 = components(3, i)
                    orefresh.mode_param2 = components(4, i)
                    orefresh.name = components(5, i)
                    orefresh.type = components(6, i)
                    orefresh.mode_param3 = components(7, i)
                    orefresh.locator = master_locator
                    orefresh.contributor_id = Me.contributor_id
                    orefresh.disabled = False
                    orefresh.refresh_type = 1
                    'orefresh.parameters = Me.parameters
                    'orefresh.web_service_name = Me.web_service_name
                    'orefresh.format_file = Me.format_file
                    'orefresh.external_id = Me.external_id
                    'orefresh.cache_level = 0
                    orefresh.db_read(pub_type_id, acc_id, stage_id, language_id, data_at_date, doc_id, row, col, disclosures_loaded, assoc_entities, loaded_external_proxies, cached_components, certificate)
                    Me.new_components.Add(orefresh)
                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_refresh_component", "build_new_components", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_refresh_component", "build_new_components", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    Private Function retrieve_disclosure_text(ByVal doc_id As String) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_component", "retrieve_disclosure_text", bc_cs_activity_codes.TRACE_ENTRY, CStr(doc_id), certificate)
        Try
            retrieve_disclosure_text = True
            Dim xml_text As String
            Dim txml As String
            Dim disclosure_text As String = ""
            Dim vxpaths As Object
            Dim i As Integer
            Dim analyst_text As String = ""
            Dim ocommentary As bc_cs_activity_log
            Dim dswebservices As New CommonPlatformWebService.CMDSoapService
            dswebservices.Timeout = bc_cs_central_settings.timeout
            ocommentary = New bc_cs_activity_log("bc_om_refresh_component", "retrieve_disclosure_text", bc_cs_activity_codes.COMMENTARY, bc_cs_central_settings.common_platform, MyBase.certificate)
            dswebservices.Url = bc_cs_central_settings.common_platform

            ocommentary = New bc_cs_activity_log("bc_om_refresh_component", "retrieve_disclosure_text", bc_cs_activity_codes.COMMENTARY, "attempting to Connect to Disclosures at URI: " + bc_cs_central_settings.common_platform + " for doc_id:" + CStr(doc_id), MyBase.certificate)
            xml_text = dswebservices.disclosures(doc_id)
            ocommentary = New bc_cs_activity_log("bc_om_refresh_component", "retrieve_disclosure_text", bc_cs_activity_codes.COMMENTARY, "Disclosures Text Returned: " + xml_text, MyBase.certificate)
            REM now parse out disclosure text
            Dim xmlload As New Xml.XmlDocument
            Dim myXmlNodeList As Xml.XmlNodeList


            xmlload.LoadXml(xml_text)
            myXmlNodeList = xmlload.SelectNodes("/disclosures/text")
            If myXmlNodeList.Count > 0 Then
                disclosure_text = xmlload.SelectSingleNode("/disclosures/text").InnerXml()
            End If
            REM analyst disclosures
            myXmlNodeList = xmlload.SelectNodes("/disclosures/disclosure[class_name='net.bluecurve.core.cmd.disclosure.AllAnalystDisclosure']/text")
            If myXmlNodeList.Count > 0 Then
                analyst_text = xmlload.SelectSingleNode("/disclosures/disclosure[class_name='net.bluecurve.core.cmd.disclosure.AllAnalystDisclosure']/text").InnerXml()
            End If

            Dim s As New bc_cs_string_services(disclosure_text)
            Dim a As New bc_cs_string_services(analyst_text)
            REM store in database
            Dim db As New bc_om_refresh_components_db
            db.write_disclosure_text(doc_id, s.delimit_apostrophies, a.delimit_apostrophies, MyBase.certificate)
            REM more specific disclsoures that are configured on the database
            vxpaths = db.get_xpaths()
            Try
                If IsArray(vxpaths) Then
                    REM delete existing text for doc
                    db.delete_comply_data_for_doc(doc_id)
                    REM TBD
                    For i = 0 To UBound(vxpaths, 2)
                        REM extract text using xpath
                        myXmlNodeList = xmlload.SelectNodes(CStr(vxpaths(1, i)))
                        txml = ""
                        If myXmlNodeList.Count > 0 Then
                            txml = xmlload.SelectSingleNode(CStr(vxpaths(1, i))).InnerXml()
                            db.write_comply_text_for_document(doc_id, CStr(vxpaths(0, i)), txml, MyBase.certificate)
                        Else
                            retrieve_disclosure_text = False
                            ocommentary = New bc_cs_activity_log("bc_om_refresh_component", "retrieve_disclsoure_text", bc_cs_activity_codes.COMMENTARY, "Xpath not found: " + CStr(vxpaths(1, i)))
                        End If
                        REM also extract entity ids

                        Dim node, nnode, name As String
                        node = Left(vxpaths(1, i), Len(vxpaths(1, i)) - 5)
                        nnode = node + "/entity/name"

                        myXmlNodeList = xmlload.SelectNodes(nnode)

                        If myXmlNodeList.Count > 0 Then
                            name = xmlload.SelectSingleNode(nnode).InnerXml()

                            node = node + "/entity/entity_id"
                            Dim nnodelist As Xml.XmlNodeList
                            nnodelist = xmlload.SelectNodes(node)
                            Dim j As Integer
                            Dim eid As String
                            For j = 0 To nnodelist.Count - 1
                                eid = nnodelist(j).InnerXml
                                db.write_comply_text_for_entity(doc_id, txml, eid, MyBase.certificate)
                            Next
                        End If
                    Next
                End If
            Catch
                ocommentary = New bc_cs_activity_log("bc_om_refresh_component", "retrieve_disclsoure_text", bc_cs_activity_codes.COMMENTARY, "Comply data structures not set up")
            End Try


        Catch ex As Exception
            retrieve_disclosure_text = False
            Dim db_err As New bc_cs_error_log("bc_om_refresh_component", "retrieve_disclosure_text", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_refresh_component", "retrieve_disclosure_text", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    'Public Sub db_write_values(ByVal doc_id As Long, ByVal accounting_standard As Long, ByVal workflow_stage As Long, ByRef certificate As bc_cs_security.certificate)
    '    Dim otrace As New bc_cs_activity_log("bc_om_refresh_component", "db_write_values", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    Try
    '        Dim db As New bc_om_refresh_components_db
    '        db.write_component_value(Me.entity_id, MyBase.type, Me.value, doc_id, accounting_standard, workflow_stage, Me.contributor_id, Me.locator, certificate)
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_om_refresh_component", "db_write_values", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

    '    Finally
    '        otrace = New bc_cs_activity_log("bc_om_refresh_component", "db_write_values", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try

    'End Sub
    Public Function db_write_component(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate, Optional ByRef tdb As bc_cs_db_services = Nothing) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_component", "db_write_components", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            db_write_component = True
            Dim db As New bc_om_refresh_components_db
            REM write down if mode 8-9 SDC 10 tyoe 1 write down
            If Me.mode = 210 Or Me.mode = 310 Or Me.mode = 260 Or Me.mode = 110 Or Me.mode = 80 Or Me.mode = 880 Or Me.mode = 8880 Or Me.mode = 8 Or Me.mode = 88 Or Me.mode = 888 Or Me.mode = 9 Or Me.mode = 10 Or Me.mode = 11 Or Me.mode = 21 Or Me.mode = 26 Or Me.mode = 111 Then
                Dim res As Integer
                If bc_cs_central_settings.server_flag = 0 Then
                    certificate = New bc_cs_security.certificate
                    certificate.user_id = bc_cs_central_settings.logged_on_user_id
                End If
                'Dim occomm As New bc_cs_activity_log("aa", "bb", bc_cs_activity_codes.COMMENTARY, certificate.user_id, certificate)
                'occomm = New bc_cs_activity_log("aa", "bb", bc_cs_activity_codes.COMMENTARY, MyBase.certificate.user_id, certificate)

                res = db.update_sdc(Me.locator, Me.type, Me.value, doc_id, certificate.user_id, Me.entity_id, Me.tx, Me.objbyte, Me.compressed_html, certificate, tdb)
                If db.err_text <> "" Then
                    err_text = db.err_text
                    Exit Function
                End If
                REM SIM MAY 2013
                REM images in the write down component range
                Dim fn As String
                Dim fs As New bc_cs_file_transfer_services
                If Me.images.Count > 0 Then
                    db.bc_core_delete_component_images(locator, doc_id, MyBase.certificate, tdb)
                    If db.err_text <> "" Then
                        err_text = db.err_text
                        Exit Function
                    End If
                End If

                For i = 0 To images.Count - 1
                    fn = bc_cs_central_settings.central_repos_path + "component images\" + Me.locator + "_" + CStr(doc_id) + "_" + CStr(i + 1) + ".png"
                    'fs.write_bytestream_to_document(fn, images(i), certificate)
                    If fs.write_bytestream_to_document(fn, images(i), certificate, False) = False Then
                        err_text = fs.err_text
                        Exit Function
                    End If
                    db.write_component_image(Me.type, locator, doc_id, fn, i + 1, MyBase.certificate, tdb)
                    If db.err_text <> "" Then
                        err_text = db.err_text
                        Exit Function
                    End If
                Next
                images.Clear()

                REM =====
                If db.err_text <> "" Then
                    err_text = db.err_text
                    Exit Function
                End If
                If res = 0 Then
                    Dim ocomm As New bc_cs_activity_log("bc_om_refresh_component", "db_write", bc_cs_activity_codes.COMMENTARY, "SDC: " + CStr(Me.type) + " updated.")
                ElseIf res = 1 Then
                    Dim ocomm As New bc_cs_activity_log("bc_om_refresh_component", "db_write", bc_cs_activity_codes.COMMENTARY, "SDC: " + CStr(Me.type) + " not updated as no chaange")
                End If
                Me.objbyte = Nothing
                Me.compressed_html = Nothing
            End If
            REM write down if a udc
            If Me.mode = 100 Then
                REM udc will only be updated if owned by current user
                REM and it has changed, only rtf and doc_id and date will be updated.
                Dim res As Integer
                res = db.update_udc(Me.locator, Me.type, Me.value, doc_id, "", 0, 0, 0, 0, MyBase.certificate, tdb)
                If db.err_text <> "" Then
                    err_text = db.err_text
                    Exit Function
                End If
                If res = 0 Then
                    Dim ocomm As New bc_cs_activity_log("bc_om_refresh_component", "db_write", bc_cs_activity_codes.COMMENTARY, "UDC: " + CStr(Me.type) + " updated.")
                ElseIf res = 1 Then
                    Dim ocomm As New bc_cs_activity_log("bc_om_refresh_component", "db_write", bc_cs_activity_codes.COMMENTARY, "UDC: " + CStr(Me.type) + " not updated as no chaange")
                ElseIf res = 2 Then
                    Dim ocomm As New bc_cs_activity_log("bc_om_refresh_component", "db_write", bc_cs_activity_codes.COMMENTARY, "UDC: " + CStr(Me.type) + " not updated as owned by different user")
                End If
            End If
        Catch ex As Exception
            db_write_component = False
        Finally
            If err_text <> "" Then
                db_write_component = False
            End If
            otrace = New bc_cs_activity_log("bc_om_refresh_component", "db_write_components", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function

    Public Sub get_data_update_date(ByVal stage_id, ByVal acc_id, ByVal doc_id)
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_component", "get_data_update_date", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim da As Date
            Dim db As New bc_om_refresh_components_db
            REM if last update set and greater than db date then use this
            da = db.get_component_update_date(Me.mode, Me.entity_id, Me.contributor_id, Me.type, stage_id, acc_id, doc_id, MyBase.certificate)
            If da = "9-9-9999" Then
                Me.last_update_date = da
                Exit Sub
            End If
            Me.last_update_date = da
            'If Me.last_update_date = "9-9-9999" Then
            'Exit Sub
            'End If
            'If da > Me.last_update_date Then
            'Me.last_update_date = da
            'End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_refresh_component", "get_data_update_date", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_refresh_component", "get_data_update_date", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
<Serializable()> Public Class bc_om_parameter_context_items
    Inherits bc_cs_soap_base_class

    <XmlArrayItem(GetType(bc_om_parameter_context_item))> Public table_items As New List(Of bc_om_parameter_context_item)
    Public context_id As Long
    Public entity_id As Long
    Public doc_id As String
    Public system_defined As Integer

    Public Sub New()

    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_parameter_context_items", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim vres As Object
            Dim i As Integer
            Dim gdb As New bc_om_refresh_components_db
            Dim pci As bc_om_parameter_context_item
            Me.table_items.Clear()
            vres = gdb.read_context_items(entity_id, context_id, doc_id, MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    pci = New bc_om_parameter_context_item
                    pci.orig_item_id = vres(0, i)
                    pci.row = vres(1, i)
                    pci.col = vres(2, i)
                    pci.orig_item_name = vres(3, i)
                    pci.system_defined = vres(4, i)
                    If pci.system_defined = 0 Then
                        pci.new_item_id = vres(6, i)
                        pci.new_item_name = vres(5, i)
                    End If
                    table_items.Add(pci)
                Next
            End If
        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_om_parameter_context_items", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_parameter_context_items", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_parameter_context_item
    Public orig_item_id As Long
    Public orig_item_name As String
    Public new_item_id As Long
    Public new_item_name As String
    Public system_defined As Integer
    Public row As Integer
    Public col As Integer

    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class bc_om_indexs
    Inherits bc_cs_soap_base_class
    Public index_components As New ArrayList
    Public Sub New()

    End Sub
    Public Shadows Sub db_write(ByVal doc_id As String, ByVal certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_om_indexes", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim i As Integer
            Dim db As New bc_om_refresh_components_db
            db.delete_index_components(doc_id, certificate)
            For i = 0 To index_components.Count - 1
                index_components(i).db_write(doc_id, certificate)
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_indexes", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_indexes", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_index_component
    Inherits bc_cs_soap_base_class
    Public index_id As Long
    Public page As Integer
    Public item As String
    Public Sub New()

    End Sub
    Public Sub New(ByVal index_id, ByVal page, ByVal item)
        Me.index_id = index_id
        Me.page = page
        Me.item = item
    End Sub
    Public Shadows Sub db_write(ByVal doc_id As String, ByVal certificate As bc_cs_security.certificate)
        Dim db As New bc_om_refresh_components_db
        Dim fs As New bc_cs_string_services(item)
        item = fs.delimit_apostrophies()
        db.write_index_component(doc_id, index_id, page, item, MyBase.certificate)
    End Sub
End Class
REM =========================================================================
REM Database interaction layer
REM =========================================================================
Public Class bc_om_refresh_components_db
    Private gbc_db As New bc_cs_db_services(False)
    Public err_text As String = ""
    Public Sub write_wcf_response(doc_id As String, type As Integer, result As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim str As New bc_cs_string_services(result)
        result = str.delimit_apostrophies

        sql = "exec dbo.bc_core_write_wcf_response '" + CStr(doc_id) + "'," + CStr(type) + ",'" + result + "'"
        gbc_db.executesql(sql, certificate)
    End Sub
    REM FIL JUNE 2013
    Public Function read_components_to_refresh(ByVal sp As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo." + sp

        read_components_to_refresh = gbc_db.executesql(sql, certificate)
    End Function

    Public Function get_value_for_component(ByVal id As Long, ByVal entity_id As Long, ByVal acc_id As Long, ByVal stage_id As Long, ByVal pub_type_id As Long, ByVal contributor_id As Long, ByVal language_id As Long, ByVal data_at_date As Date, ByVal doc_id As String, ByVal parameters As Object, ByVal sp_name As String, ByVal row As Integer, ByVal col As Integer, ByVal parent_entity_id As Long, ByRef certificate As bc_cs_security.certificate, from_web As Boolean) As Object
        REM get stored procedure to call
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_component_db", "get_value_for_component: type: ", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM exceute stored procedure
            If from_web = False Then
                Dim ocommentary = New bc_cs_activity_log("bc_om_refresh_components_db", "get_value_for_component non web: type: ", bc_cs_activity_codes.COMMENTARY, "Attempting to get value using stored procedure:" + sp_name, certificate)

                get_value_for_component = gbc_db.executesp_for_component(sp_name, entity_id, pub_type_id, language_id, contributor_id, stage_id, acc_id, data_at_date, doc_id, parameters, row, col, parent_entity_id, certificate)
            Else
                Dim ocommentary = New bc_cs_activity_log("bc_om_refresh_components_db", "get_value_for_component for web : type: ", bc_cs_activity_codes.COMMENTARY, "Attempting to get value using stored procedure:" + sp_name, certificate)

                get_value_for_component = gbc_db.executesp_for_component_from_web(sp_name, entity_id, pub_type_id, language_id, contributor_id, stage_id, acc_id, data_at_date, doc_id, parameters, row, col, parent_entity_id, certificate)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_refresh_components", "get_value_for_component", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            get_value_for_component = Nothing
        Finally
            '   gbc_db.close_connection()
            otrace = New bc_cs_activity_log("bc_om_refresh_components", "get_value_for_component", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function get_value_for_udc(ByVal doc_id As String, ByVal id As Long, ByVal entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        REM get stored procedure to call
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_component_db", "get_value_for_udc", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM exceute stored procedure
            If bc_cs_central_settings.server_flag = 0 Then
                certificate.user_id = bc_cs_central_settings.logged_on_user_id
            End If


            Dim sql As String
            sql = "exec dbo.bc_core_get_sdc_value '" + CStr(doc_id) + "'," + CStr(id) + "," + CStr(entity_id) + "," + CStr(certificate.user_id)
            get_value_for_udc = gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_refresh_components", "get_value_for_udc", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            get_value_for_udc = Nothing
        Finally
            '   gbc_db.close_connection()
            otrace = New bc_cs_activity_log("bc_om_refresh_components", "get_value_for_udc", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function get_value_for_sdc(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        REM get stored procedure to call
        Dim otrace As New bc_cs_activity_log("bc_om_refresh_component_db", "get_value_for_sdc", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM exceute stored procedure
            Dim sql As String
            sql = "select rtf from bc_core_user_def_comp where udc_id=" + CStr(id) + " and [current]=1"
            get_value_for_sdc = gbc_db.executesql(sql, certificate)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_refresh_components", "get_value_for_sdc", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            get_value_for_sdc = Nothing
        Finally
            '   gbc_db.close_connection()
            otrace = New bc_cs_activity_log("bc_om_refresh_components", "get_value_for_sdc", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Sub write_component_value(ByVal entity_id As Long, ByVal component_id As Long, ByVal value As String, ByVal doc_id As Long, ByVal acc_id As Long, ByVal workflow_id As Long, ByVal contributor_id As Long, ByVal locator As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bcc_core_insert_document_component " + CStr(doc_id) + "," + CStr(entity_id) + "," + CStr(component_id) + ",'" + value + "'," + CStr(locator) + "," + CStr(workflow_id) + "," + CStr(contributor_id)
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function get_image(ByVal attribute_name As String, ByVal entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim eid As Object
        If entity_id = 0 Then
            sql = "select entity_id from entity_tbl where name = 'No Entity'"
            eid = gbc_db.executesql(sql, certificate)
            If IsArray(eid) Then
                Try
                    entity_id = CLng(eid(0, 0))
                Catch ex As Exception
                    Dim commentary As New bc_cs_activity_log("bc_om_refresh_components_db", "get_image", bc_cs_activity_codes.COMMENTARY, "No Entity not set up")
                    entity_id = 0
                End Try
            End If

        End If
        sql = "select item_value from entity_item_value_binary_d e, attribute_tbl a where a.label_code='" + attribute_name + "' and a.attribute_id=e.attribute_id and entity_id=" + CStr(entity_id)
        get_image = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_ticker(ByVal entity_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select attrib_value from entity_attribute_value_tbl e, attribute_tbl a where e.attribute_id=a.attribute_id and (name = 'Ticker' or name = 'TeleKurs') and entity_id=" + CStr(entity_id)
        get_ticker = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_component_image(ByVal entity_id As Long, ByVal attribute_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select attrib_value from entity_attribute_value_tbl e where e.attribute_id =" + CStr(attribute_id) + " and entity_id=" + CStr(entity_id)
        get_component_image = gbc_db.executesql(sql, certificate)
    End Function

    'Public Function get_component_image_bytype(ByVal entity_id As Long, ByVal pub_type_id As Long, ByVal type_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
    '    Dim sql As String
    '    Dim vres As Object

    '    sql = "exec dbo.bcc_core_get_component_value " + CStr(entity_id) + "," + CStr(pub_type_id) + "," + CStr(type_id)
    '    vres = gbc_db.executesql(sql, certificate)

    '    get_component_image_bytype = vres
    'End Function
    Public Function get_component_image_bytype(ByVal entity_id As Long, ByVal pub_type_id As Long, doc_id As String, ByVal type_id As Long, ByVal params As List(Of String), ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim vres As Object
        Dim user_id As Long
        If bc_cs_central_settings.server_flag = 0 Then
            user_id = bc_cs_central_settings.logged_on_user_id
        Else
            user_id = certificate.user_id
        End If

        sql = "exec dbo.bcc_core_get_component_value " + CStr(entity_id) + "," + CStr(pub_type_id) + "," + CStr(type_id) + ", '" + CStr(doc_id) + "', " + CStr(user_id)
        For i = 0 To params.Count - 1
            sql = sql + ",'" + params(i) + "'"
        Next
        vres = gbc_db.executesql(sql, certificate)

        get_component_image_bytype = vres
    End Function



    'Public Sub write_component_properties(ByVal doc_id As Long, ByVal entity_id As Long, ByVal locator As String, ByVal last_refresh_date As Date, ByVal last_update_date As Date, ByVal disabled As String, ByVal contributor_id As Long, ByVal type_id As Long, ByVal sub_doc_id As Long, ByRef certificate As bc_cs_security.certificate, ByRef db As bc_cs_db_services)
    '    Dim sql As String
    '    Dim ocomm As New bc_cs_activity_log("JJJ", "KKK", bc_cs_activity_codes.COMMENTARY, last_refresh_date, certificate)
    '    Try



    '        If last_refresh_date <> "9-9-9999" Then
    '            ocomm = New bc_cs_activity_log("YES", "KKK", bc_cs_activity_codes.COMMENTARY, last_refresh_date, certificate)

    '            last_refresh_date = last_refresh_date.ToUniversalTime
    '        End If
    '        If last_update_date <> "9-9-9999" Then
    '            last_update_date = last_update_date.ToUniversalTime
    '        End If
    '        Dim rda As New bc_om_date(last_refresh_date)
    '        Dim uda As New bc_om_date(last_update_date)
    '        sql = "exec dbo.bc_core_write_component_properties " + CStr(doc_id) + "," + CStr(entity_id) + ",'" + CStr(locator) + "','" + rda.write_data_to_xml(certificate) + "','" + uda.write_data_to_xml(certificate) + "','" + disabled + "'," + CStr(contributor_id) + "," + CStr(type_id) + "," + CStr(sub_doc_id)
    '        db.executesql_trans(sql, certificate, False)
    '    Catch ex As Exception
    '        Dim oerr As New bc_cs_error_log("bc_om_refresh_components_db", "write_component_properties", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)


    '    End Try

    'End Sub
    Public Function get_component_update_date(ByVal mode As Integer, ByVal entity_id As Long, ByVal contributor_id As Long, ByVal type_id As Long, ByVal stage_id As Long, ByVal acc_id As Long, ByVal doc_id As String, ByRef certificate As bc_cs_security.certificate) As Date
        Dim sql As String
        Dim res As Object
        If mode = 100 Then
            REM UDC
            sql = "select update_date from bc_core_user_def_comp where udc_id=" + CStr(type_id) + " and [current]=1"
        ElseIf mode = 8 Or mode = 88 Or mode = 888 Or mode = 9 Or mode = 80 Or mode = 880 Or mode = 8880 Then
            REM FIL AUG 2013
            If bc_cs_central_settings.server_flag = 0 Then
                certificate.user_id = bc_cs_central_settings.logged_on_user_id
            End If
            sql = "exec dbo.bc_core_get_sdc_last_update '" + CStr(doc_id) + "'," + CStr(entity_id) + "," + CStr(type_id) + "," + CStr(certificate.user_id)
        Else
            REM SDC
            sql = "exec bcc_core_component_update_date " + CStr(entity_id) + "," + CStr(contributor_id) + "," + CStr(type_id) + "," + CStr(stage_id) + "," + CStr(acc_id)
        End If
        res = gbc_db.executesql(sql, certificate)
        If IsArray(res) Then
            If UBound(res, 2) > -1 Then
                get_component_update_date = res(0, 0)
            Else
                get_component_update_date = "9-9-9999"
            End If
        End If
    End Function
    Public Function get_restrictions_for_component(ByVal entity_id As Long, ByVal component_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bcc_core_res_component_refresh " + CStr(entity_id) + "," + CStr(component_id)
        get_restrictions_for_component = gbc_db.executesql_show_no_error(sql)
    End Function
    Public Function get_extended_disclosure_params(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select no_master,max_number_default,alternate_chart,use_alternate_list from bcc_core_extended_disclosures"
        get_extended_disclosure_params = gbc_db.executesql_show_no_error(sql)
    End Function
    Public Function get_alternate_assoc_entities(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bcc_core_disclosure_entities " + CStr(doc_id)
        get_alternate_assoc_entities = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_assoc_entities(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select a.entity_id, e.class_id from assoc_doc_entity_tbl a, entity_tbl e, disclosure_entities d where a.entity_id=e.entity_id and doc_id=" + CStr(doc_id) + "  and d.entity_id = a.entity_id order by e.name asc"
        get_assoc_entities = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_tmp_assoc_entities(ByVal doc_id As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select a.entity_id, e.class_id from tmp_assoc_doc_entity_tbl a, entity_tbl e, disclosure_entities d where a.entity_id=e.entity_id and doc_id='" + CStr(doc_id) + "'  and d.entity_id = a.entity_id order by e.name asc"
        get_tmp_assoc_entities = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_document_refresh_components(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        'sql = "select a.entity_id, a.locator_id, a.contributor_id,a.last_refresh_date,a.last_update_date,a.disabled,type_id, b.at_component_description,b.at_type_addable, coalesce(b.at_sp_name,''), coalesce(b.chart_name,''),b.refresh_type,coalesce(web_service_name,''), coalesce(format_file,''), coalesce(external_id,0),coalesce(caching_level,0) from at_document_components a, at_component_types b where a.type_id = b.at_component_type_id and doc_id=" + CStr(doc_id)
        sql = "select a.entity_id, a.locator_id, a.contributor_id,a.last_refresh_date,a.last_update_date,a.disabled,type_id, b.at_component_description as name,b.at_type_addable, coalesce(b.at_sp_name,''), coalesce(b.chart_name,''),b.refresh_type,coalesce(web_service_name,''), coalesce(format_file,''), coalesce(external_id,0),coalesce(caching_level,0),0,isnull(b.attribute_id,'0'), coalesce(sub_doc_id,0) from at_document_components a, at_component_types b where a.type_id = b.at_component_type_id and doc_id=" + CStr(doc_id) + _
              " and PATINDEX('%udc%',locator_id) = 0" + _
              " union" + _
              " select a.entity_id, a.locator_id, a.contributor_id,a.last_refresh_date,b.update_date,a.disabled,type_id, b.title as name,100, '', '',1,'','',0,0,coalesce(b.author_id,0),0,0 from at_document_components a, bc_core_user_def_comp b where a.type_id = b.udc_id and a.doc_id=" + CStr(doc_id) + _
              " and [current]=1 and PATINDEX('%udc%',locator_id) <> 0 order by at_type_addable, name asc"
        get_document_refresh_components = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_context_items(ByVal entity_id, ByVal context_id, ByVal doc_id, ByVal certificate) As Object
        Dim sql As String
        sql = "exec dbo.bcc_core_get_items_for_context " + CStr(entity_id) + "," + CStr(context_id) + ",'" + CStr(doc_id) + "'"
        read_context_items = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub delete_assoc_entity(ByVal doc_id As String, ByVal int As Boolean, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        If int = True Then
            sql = "delete from assoc_doc_entity_tbl where doc_id=" + CStr(doc_id)
        Else
            sql = "delete from tmp_assoc_doc_entity_tbl where doc_id='" + CStr(doc_id) + "'"
        End If
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub add_assoc_entity(ByVal doc_id As String, ByVal entity_id As Long, ByVal int As Boolean, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        If int = True Then
            REM SR - Column names included in the insert statement
            sql = "insert into assoc_doc_entity_tbl (entity_id, doc_id, system_set) values(" + CStr(entity_id) + "," + CStr(doc_id) + ",0)"
        Else
            sql = "insert into tmp_assoc_doc_entity_tbl (entity_id, doc_id, system_set) values(" + CStr(entity_id) + ",'" + CStr(doc_id) + "',0)"
        End If
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub delete_index_components(ByVal doc_id As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from bcc_document_indexs where doc_id='" + CStr(doc_id) + "'"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub write_index_component(ByVal doc_id As String, ByVal index As String, ByVal page As String, ByVal item As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "insert into bcc_document_indexs (doc_id, [index], page, item) values('" + CStr(doc_id) + "'," + CStr(index) + "," + CStr(page) + ",'" + item + "')"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub write_disclosure_text(ByVal doc_id As String, ByVal text As String, ByVal atext As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from bcc_core_disc_text where doc_id='" + doc_id + "'"
        gbc_db.executesql(sql, certificate)
        REM SR - Column names included in the insert statement
        sql = "insert into bcc_core_disc_text (doc_id, [text], analyst_text) values('" + doc_id + "','" + text + "','" + atext + "')"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub set_component_documents(ByVal doc_id As String, ByVal component_documents As ArrayList, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from bc_core_tmp_doc_constituents where master_doc_id='" + doc_id + "'"
        gbc_db.executesql(sql, certificate)
        For i = 0 To component_documents.Count - 1
            REM SR - Column names included in the insert statement
            sql = "insert into bc_core_tmp_doc_constituents (master_doc_id, conts_doc_id, ord) values('" + CStr(doc_id) + "'," + CStr(component_documents(i)) + "," + CStr(i) + ")"
            gbc_db.executesql(sql, certificate)
        Next

    End Sub
    Public Function get_entity_class_for_entity(ByVal entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Long
        Dim sql As String
        Dim oclass As Object
        get_entity_class_for_entity = 0
        sql = "select class_id from entity_tbl where entity_id=" + CStr(entity_id)

        oclass = gbc_db.executesql(sql, certificate)
        If IsArray(oclass) Then
            If UBound(oclass, 2) > -1 Then
                get_entity_class_for_entity = oclass(0, 0)
            End If
        End If
    End Function
    Public Function is_calculating(ByVal entity_id As Long) As Boolean
        Dim sql As String
        is_calculating = False
        sql = "exec dbo.bcc_core_is_calculating  " + CStr(entity_id)
        Dim val As Object
        val = gbc_db.executesql_show_no_error(sql)
        Try
            If IsArray(val) Then
                If val(0, 0) = 1 Then
                    is_calculating = True
                End If
            End If
        Catch

        End Try
    End Function
    Public Function get_xpaths() As Object
        Dim sql As String
        sql = "select name,xpath from bcc_core_comply_xpath"
        REM use no error as not all clients will have this
        get_xpaths = gbc_db.executesql_show_no_error(sql)
    End Function
    Public Sub set_embargo_level(ByVal doc_id As String, ByVal level As Integer, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        REM SR - "bcc_core_set_doc_embargo" is a redundant table
        sql = "insert into bcc_core_set_doc_embargo values ('" + CStr(doc_id + "'," + CStr(level) + ")")
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub clear_embargo_level(ByVal doc_id As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "delete from bcc_core_set_doc_embargo where doc_id='" + CStr(doc_id) + "'"
        gbc_db.executesql(sql, certificate)
    End Sub
    REM SIM MAY 2013
    Public Sub bc_core_delete_component_images(ByVal locator As String, ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate, Optional ByRef db As bc_cs_db_services = Nothing)
        Dim sql As String
        sql = "exec dbo.bc_core_delete_component_images '" + locator + "'," + CStr(doc_id)
        If IsNothing(db) = True Then
            gbc_db.executesql(sql, certificate)
        Else
            Dim res As Object
            res = db.executesql_trans(sql, certificate, True)
            If db.success = False Then
                err_text = CStr(res(0, 0))
                Exit Sub
            End If
        End If


    End Sub
    Public Sub write_component_image(ByVal component_type As Integer, ByVal locator As String, ByVal doc_id As Long, ByVal fn As String, ByVal ord As Integer, ByRef certificate As bc_cs_security.certificate, Optional ByRef db As bc_cs_db_services = Nothing)
        Dim sql As String
        sql = "exec dbo.bc_core_write_component_image " + CStr(component_type) + ",'" + locator + "'," + CStr(doc_id) + ",'" + fn + "'," + CStr(ord)
        If IsNothing(db) = True Then
            gbc_db.executesql(sql, certificate)
        Else
            Dim res As Object
            res = db.executesql_trans(sql, certificate, True)
            If db.success = False Then
                err_text = CStr(res(0, 0))
                Exit Sub
            End If
        End If

    End Sub
    REM =================================
    Public Function update_udc(ByVal locator As String, ByVal type As Long, ByVal value As String, ByVal doc_id As Long, Optional ByVal title As String = "", Optional ByVal ipf As Integer = 0, Optional ByVal pub_type_id As Long = 0, Optional ByVal author_id As Long = 0, Optional ByVal entity_id As Integer = 0, Optional ByRef certificate As bc_cs_security.certificate = Nothing, Optional ByRef db As bc_cs_db_services = Nothing) As Integer
        Try
            Dim user_id As Long
            If bc_cs_central_settings.server_flag = 0 Then
                user_id = bc_cs_central_settings.logged_on_user_id
            Else
                user_id = certificate.user_id
            End If

            Dim ostr As New bc_cs_string_services(value)
            value = ostr.delimit_apostrophies
            Dim res As Object
            If IsNothing(db) = True Then
                res = gbc_db.executesql("exec dbo.bc_core_update_udc '" + locator + "'," + CStr(user_id) + "," + CStr(type) + "," + CStr(doc_id) + ",'" + value + "','" + title + "'," + CStr(ipf) + "," + CStr(pub_type_id) + "," + CStr(entity_id), certificate)
            Else
                res = db.executesql_trans("exec dbo.bc_core_update_udc '" + locator + "'," + CStr(user_id) + "," + CStr(type) + "," + CStr(doc_id) + ",'" + value + "','" + title + "'," + CStr(ipf) + "," + CStr(pub_type_id) + "," + CStr(entity_id), certificate, True)
                If db.success = False Then
                    err_text = CStr(res(0, 0))
                    Exit Function
                End If

            End If
            REM 0 suceess 1 no change 2 no access
            If IsArray(res) Then
                If UBound(res, 2) >= 0 Then
                    update_udc = res(0, 0)
                End If
            End If
        Catch ex As Exception
            update_udc = -1
            Dim oerr As New bc_cs_error_log("bc_om_refresh_components_db", "update_udc", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Function
    REM mode 8 or 9 8 rtf 9 text
    REM FIL October 2013
    Public Function update_sdc(ByVal locator As String, ByVal type As Long, ByVal value As String, ByVal doc_id As Long, ByVal author_id As Long, ByVal entity_id As Integer, ByVal tx As String, ByVal objbyte As Byte(), ByVal objbytehtml As Byte(), Optional ByRef certificate As bc_cs_security.certificate = Nothing, Optional ByRef db As bc_cs_db_services = Nothing) As Integer
        Try

            Dim user_id As Long
            Dim html As String = ""
            If bc_cs_central_settings.server_flag = 0 Then
                user_id = bc_cs_central_settings.logged_on_user_id
                certificate = New bc_cs_security.certificate
            Else
                user_id = certificate.user_id
            End If




            REM if objbyte not null then decompress into value
            If Not IsNothing(objbyte) Then
                Dim bcs As New bc_cs_security
                value = bcs.decompress_xml_winzip(objbyte, certificate)
                If value = "" Then
                    Me.err_text = "failed to decompress rtf"
                    Exit Function
                End If
            End If
            If Not IsNothing(objbytehtml) Then
                Dim bcs As New bc_cs_security
                html = bcs.decompress_xml_winzip(objbytehtml, certificate)
                If html = "" Then
                    Me.err_text = "failed to decompress html"
                    Exit Function
                End If
            End If
            If tx = Nothing And value = Nothing And html = Nothing Then
                update_sdc = 0
                Exit Function
            End If
            Dim ostr As New bc_cs_string_services(value)
            value = ostr.delimit_apostrophies
            ostr = New bc_cs_string_services(tx)
            tx = ostr.delimit_apostrophies
            ostr = New bc_cs_string_services(html)
            html = ostr.delimit_apostrophies

            Dim res As Object
            Dim ocomm As New bc_cs_activity_log("bc_om_refresh_components_db", "update_sdc", bc_cs_activity_codes.COMMENTARY, "calling exec dbo.bc_core_update_sdc for user: " + CStr(user_id) + " locator: " + CStr(locator) + tx, certificate)
            If IsNothing(db) = True Then
                res = gbc_db.executesql_show_no_error("exec dbo.bc_core_update_sdc '" + locator + "'," + CStr(user_id) + "," + CStr(type) + "," + CStr(doc_id) + ",N'" + value + "'," + CStr(entity_id) + ",N'" + tx + "',N'" + html + "'")
            Else
                res = db.executesql_trans("exec dbo.bc_core_update_sdc '" + locator + "'," + CStr(user_id) + "," + CStr(type) + "," + CStr(doc_id) + ",N'" + value + "'," + CStr(entity_id) + ",N'" + tx + "',N'" + html + "'", certificate, True, True)
                If db.success = False Then
                    err_text = CStr(res(0, 0))
                    Exit Function
                End If
            End If
            REM 0 suceess 1 no change 2 no access
            If IsArray(res) Then
                If UBound(res, 2) >= 0 Then
                    update_sdc = res(0, 0)
                End If
            End If
        Catch ex As Exception
            update_sdc = -1
            err_text = "bc_om_refresh_components_db: update_sdc " + ex.Message

            'Dim oerr As New bc_cs_error_log("bc_om_refresh_components_db", "update_sdc", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Function

    Public Sub delete_comply_data_for_doc(ByVal doc_id As Long)
        Dim sql As String
        sql = "delete from bcc_core_comply_xpath_value_for_doc where doc_id=" + CStr(doc_id)
        gbc_db.executesql_show_no_error(sql)
        sql = "delete from bcc_core_comply_xpath_value_for_entity where doc_id=" + CStr(doc_id)
        gbc_db.executesql_show_no_error(sql)
    End Sub

    Public Sub write_comply_text_for_document(ByVal doc_id As Long, ByVal name As String, ByVal rtext As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim fs As New bc_cs_string_services(rtext)
        rtext = fs.delimit_apostrophies()
        sql = "insert into bcc_core_comply_xpath_value_for_doc(doc_id,name,value) values(" + CStr(doc_id) + ",'" + name + "','" + rtext + "')"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Sub write_comply_text_for_entity(ByVal doc_id As Long, ByVal tx As String, ByVal entity_id As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim fs As New bc_cs_string_services(tx)
        tx = fs.delimit_apostrophies()
        sql = "insert into bcc_core_comply_xpath_value_for_entity(doc_id,entity_id,tx) values(" + CStr(doc_id) + ",'" + CStr(entity_id) + "','" + tx + "')"
        'gbc_db.executesql_show_no_error(sql)
        gbc_db.executesql(sql, certificate)
    End Sub
End Class
REM ==================================================
REM Bluecurve Limited 2005
REM Module:       Standard Display Template Parameters
REM Type:         Object Model
REM Description:  Holds paramter details for an instance 
REM               of a display in a template located by
REM               the component id.
REM Version:      1
REM Change history
REM ==================================================
<Serializable()> Public Class bc_om_component_parameters
    Inherits bc_cs_soap_base_class
    Public component_id As Long
    Public bc_om_component_parameters As New List(Of bc_om_component_parameter)
    Public Sub New()

    End Sub

    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_component_parameters", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim i, j As Integer
            bc_om_component_parameters.Clear()
            'Dim otemplate_parameter As bc_om_standard_display_template_parameter
            Dim db_params As New bc_om_component_parameters_db
            Dim vparams As Object
            REM get non-specific Standard Display Parameters first
            vparams = db_params.get_parameters_for_component(Me.component_id, certificate)
            If IsArray(vparams) Then
                For i = 0 To UBound(vparams, 2)
                    Dim oparam As New bc_om_component_parameter
                    oparam.name = vparams(0, i)
                    oparam.order = vparams(1, i)
                    oparam.datatype = vparams(2, i)
                    oparam.class_id = vparams(3, i)
                    oparam.lookup_sql = CStr(vparams(4, i))
                    oparam.default_value = CStr(vparams(6, i))

                    oparam.disabled_in_doc = CStr(vparams(7, i))
                    Try
                        oparam.mandatory = CStr(vparams(8, i))
                    Catch

                    End Try

                    'End Try

                    If oparam.datatype = 15 Or oparam.datatype = 16 Or oparam.datatype = 5 Or oparam.datatype = 6 Or oparam.datatype = 7 Or oparam.datatype = 8 Then
                        REM lookup value
                        Dim vvalues As Object
                        If oparam.datatype = 7 Then
                            vvalues = db_params.get_lookup_values("exec " + CStr(vparams(4, i) + " 0"), Nothing, certificate)
                        Else
                            vvalues = db_params.get_lookup_values(CStr(vparams(4, i)), Nothing, certificate)
                        End If
                        If IsArray(vvalues) Then
                            For j = 0 To UBound(vvalues, 2)
                                If UBound(vvalues, 1) = 1 Then
                                    oparam.lookup_values.Add(CStr(vvalues(0, j)))
                                Else
                                    If oparam.datatype <> 15 And oparam.datatype <> 16 Then
                                        If CStr(vvalues(0, j)) = oparam.default_value Then
                                            oparam.default_value_id = CStr(vvalues(0, j))
                                            oparam.default_value = CStr(vvalues(1, j))
                                        End If
                                    End If
                                    REM uses id as well
                                    oparam.lookup_values_ids.Add(CStr(vvalues(1, j)))
                                    oparam.lookup_values.Add(CStr(vvalues(0, j)))
                                End If
                            Next
                        End If

                    End If

                    oparam.system_defined = vparams(5, i)
                    oparam.original_system_defined = vparams(5, i)

                    bc_om_component_parameters.Add(oparam)

                Next
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_component_parameters", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_component_parameters", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
End Class
REM ===============================================
REM Bluecurve Limited 2006
REM Module:       Component Paramter
REM Type:         Object Model
REM Description:  Refresh Object
REM Version:      1
REM Change history
REM ===============================================
<Serializable()> Public Class bc_om_component_parameter
    Inherits bc_cs_soap_base_class

    REM parameter name
    Public name As String
    REM parameter order
    Public order As Integer
    REM 0 string 1 boolean 2 numeric 3 date 4 class 5 lookup sql
    Public datatype As Integer
    REM if datatype 4 class id
    Public class_id As Long
    REM if datatype 5 lookup sql
    Public lookup_sql As String
    REM system defined
    Public system_defined As Integer
    REM component was originally system defined
    Public original_system_defined As Integer
    REM default_value if not system defined
    Public default_value As String
    Public default_value_id As String = ""
    REM lookyp values for type 5
    Public lookup_values As New ArrayList
    Public lookup_values_ids As New ArrayList
    Public context_items As New bc_om_parameter_context_items
    Public disabled_in_doc As Integer
    Public author_id As Long
    REM FIL AUG 2013
    Public mandatory As Boolean = False
    Public dependent_parameter_values As New List(Of String)
    Public type_id As Long
    Public locator As String
    Public list_key_ids As New List(Of Long)


    Public Sub New()

    End Sub
    REM FIL JUNE 2013
    Public Sub read_system_defined_value(ByVal entity_id As Long, ByVal doc_id As String, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_om_component_parameter", "read_system_defined_value", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            If system_defined = 2 And datatype = 7 Then
                Dim gdb As New bc_om_component_parameters_db
                Dim vres As Object
                default_value = ""
                default_value_id = 0
                vres = gdb.read_system_defined_value(entity_id, name, doc_id, certificate)
                If IsArray(vres) Then
                    default_value = CStr(vres(0, 0))
                    default_value_id = CStr(vres(1, 0))
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_component_parameter", "read_system_defined_value", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_component_parameter", "read_system_defined_value", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        End Try
    End Sub
    Public Sub db_read(ByVal entity_id As Long, ByVal dependent_parameter_values As List(Of String), Optional ByVal doc_id As Long = 0, Optional ByVal context_id As Long = 0)
        REM if lookup value get list of lookup values
        Dim gdb As New bc_om_component_parameters_db
        Dim i As Integer
        Dim res As Object
        Me.default_value_id = ""

        If Me.datatype = 15 Or Me.datatype = 16 Or Me.datatype = 5 Or Me.datatype = 6 Or Me.datatype = 7 Or Me.datatype = 8 Or Me.datatype = 9 Or Me.datatype = 10 Or Me.datatype = 11 Then
            If Me.datatype = 16 Or Me.datatype = 7 Or Me.datatype = 9 Or Me.datatype = 10 Then
                res = gdb.get_lookup_values("exec " + Me.lookup_sql + " " + CStr(entity_id), dependent_parameter_values, certificate)
            Else
                res = gdb.get_lookup_values(Me.lookup_sql, Nothing, certificate)
            End If
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    If UBound(res, 1) = 1 Then
                        lookup_values.Add(CStr(res(0, i)))
                    Else
                        REM uses id as well
                        lookup_values_ids.Add(CStr(res(1, i)))
                        lookup_values.Add(CStr(res(0, i)))
                        If Me.datatype <> 15 And Me.datatype <> 16 Then
                            If CStr(res(0, i)) = Me.default_value Then
                                Me.default_value_id = CStr(res(0, i))
                                Me.default_value = CStr(res(1, i))
                            End If
                        Else
                            Me.default_value_id = Me.default_value
                        End If
                    End If
                Next
            End If
        End If
        If Me.datatype = 9 Then
            REM load array of items
            Dim oci As New bc_om_parameter_context_items
            oci.entity_id = entity_id
            oci.doc_id = doc_id
            oci.context_id = context_id
            oci.db_read()
            context_items = oci
        End If

        If doc_id <> 0 Then
            If Me.datatype = 15 Or Me.datatype = 16 Then
                res = Nothing
                Me.list_key_ids.Clear()
                res = gdb.get_List_values(doc_id, Me.type_id, Me.order, Me.locator, certificate)
                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        Me.list_key_ids.Add(res(0, i))
                    Next
                End If
            End If
        End If


    End Sub

    Public Sub db_write_document_parameter(ByVal doc_id As String, ByVal locator As String, ByVal type_id As Long, ByRef certificate As bc_cs_security.certificate, ByRef db As bc_cs_db_services)
        Dim gdb As New bc_om_component_parameters_db
        If Me.default_value_id <> "" Then
            gdb.write_document_parameter(doc_id, locator, Me.order, Me.system_defined, Me.default_value_id, type_id, certificate, db)
        Else
            gdb.write_document_parameter(doc_id, locator, Me.order, Me.system_defined, Me.default_value, type_id, certificate, db)
        End If
    End Sub

End Class
REM ===============================================
REM Bluecurve Limited 2006
REM Module:       Template Paramter
REM Type:         Object Model
REM Description:  Refresh Object
REM Version:      1
REM Change history
REM ===============================================
<Serializable()> Public Class bc_om_component_template_parameters
    Public component_template_parameters As New List(Of bc_om_component_parameter)

    <NonSerialized()> Public comp_id As Long
    <NonSerialized()> Public type_id As Long
    <NonSerialized()> Public row_id As Long
    <NonSerialized()> Public col_id As Long
    Public Sub New()

    End Sub
    Public Sub New(ByVal comp_id As Long, ByVal type_id As Long, ByVal row_id As Long, ByVal col_id As Long, ByRef certificate As bc_cs_security.certificate)
        Me.comp_id = comp_id
        Me.type_id = type_id
        Me.row_id = row_id
        Me.col_id = col_id
    End Sub


    Public Sub db_read(ByRef certificate As bc_cs_security.certificate)
        Dim db_params As New bc_om_component_parameters_db
        Dim vparams As Object
        Dim oparam As bc_om_component_parameter
        Dim i As Integer
        vparams = db_params.get_component_template_parameters(type_id, comp_id, row_id, col_id, certificate)
        If IsArray(vparams) Then
            If UBound(vparams, 2) >= 0 Then
                For i = 0 To UBound(vparams, 2)
                    oparam = New bc_om_component_parameter
                    oparam.name = vparams(0, i)
                    oparam.order = vparams(1, i)
                    oparam.datatype = vparams(2, i)
                    oparam.class_id = vparams(3, i)
                    oparam.lookup_sql = vparams(4, i)
                    oparam.system_defined = vparams(5, i)
                    oparam.default_value = vparams(6, i)
                    oparam.original_system_defined = vparams(7, i)
                    oparam.disabled_in_doc = vparams(8, i)
                    oparam.mandatory = vparams(9, i)
                    If oparam.datatype = 15 Or oparam.datatype = 16 Or oparam.datatype = 5 Or oparam.datatype = 6 Or oparam.datatype = 8 Or oparam.datatype = 9 Or oparam.datatype = 10 Or oparam.datatype = 11 Then
                        REM lookup value
                        oparam.db_read(0, Nothing)
                    End If

                    component_template_parameters.Add(oparam)
                Next
            End If
        End If
    End Sub
    'Public Sub db_write_document_parameters(ByVal doc_id, ByVal locator, ByVal type_id)
    '    Dim i As Integer
    '    Dim db As New bc_om_component_parameters_db
    '    db.delete_document_parameters_for_locator(doc_id, locator)
    '    For i = 0 To Me.component_template_parameters.Count - 1
    '        Me.component_template_parameters(i).write_document_parameter(doc_id, locator, type_id)
    '    Next
    'End Sub
End Class
REM =========================================================================
REM Database interaction layer
REM =========================================================================
Public Class bc_om_component_parameters_db
    Private gbc_db As New bc_cs_db_services(False)
    REM FIL JUNE 2013
    Public Function read_system_defined_value(ByVal entity_id As Long, ByVal name As String, ByVal doc_id As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        If bc_cs_central_settings.server_flag = 0 Then
            certificate.user_id = bc_cs_central_settings.logged_on_user_id
        End If
        sql = "exec dbo.bc_core_read_system_defined_value '" + name + "'," + CStr(entity_id) + "," + CStr(certificate.user_id) + ",'" + doc_id + "'"
        read_system_defined_value = gbc_db.executesql(sql, certificate)
    End Function
    REM FIL AUG 2013
    Public Function get_component_template_parameters(ByVal type_id As Long, ByVal comp_id As Long, ByVal row_id As Long, ByVal col_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        REM get stored procedure to call
        Dim otrace As New bc_cs_activity_log("bc_om_standard_display_parameters_db", "get_parameters_for_display_in_template", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim sql As String
            Dim vres As Object
            sql = "select a.name, a.ord,a.datatype, a.class_id, a.lookup_sql, b.system_defined,b.default_value, a.system_defined, coalesce(a.disabled_in_doc,0),coalesce(a.mandatory,0) from at_component_parameters a, at_component_template_parameters b" + _
            " where(a.type_id = b.type_id And a.ord = b.ord And a.type_id = " + CStr(type_id) + " And b.comp_id = " + CStr(comp_id) + " And b.row_id = " + CStr(row_id) + " and b.col_id=" + CStr(col_id) + ") order by a.ord asc"

            vres = gbc_db.executesql_show_no_error(sql)
            If IsArray(vres) Then
                If UBound(vres, 2) > -1 Then
                    If vres(0, 0) = "Error" Then
                        sql = "select a.name, a.ord,a.datatype, a.class_id, a.lookup_sql, b.system_defined,b.default_value, a.system_defined,0 from at_component_parameters a, at_component_template_parameters b" + _
                         " where(a.type_id = b.type_id And a.ord = b.ord And a.type_id = " + CStr(type_id) + " And b.comp_id = " + CStr(comp_id) + " And b.row_id = " + CStr(row_id) + " and b.col_id=" + CStr(col_id) + ") order by a.ord asc"
                        vres = gbc_db.executesql(sql, certificate)
                    End If
                End If
            End If
            get_component_template_parameters = vres
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_standard_display_parameters_db", "get_parameters_for_display_in_template", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            get_component_template_parameters = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_om_standard_display_parameters_db", "get_parameters_for_display_in_template", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function get_component_document_parameters(ByVal doc_id As Long, ByVal locator_id As String, ByRef certificate As bc_cs_security.certificate) As Object
        REM get stored procedure to call
        'Dim otrace As New bc_cs_activity_log("bc_om_standard_display_parameters_db", "get_parameters_for_display_in_template", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim sql As String
            Dim vres As Object
            sql = "select a.name, a.ord,a.datatype, a.class_id, a.lookup_sql, b.system_defined,b.default_value, a.system_defined, coalesce(a.disabled_in_doc,0), coalesce(a.mandatory,0) from at_component_parameters a, at_component_document_parameters b" + _
            " where(a.type_id = b.type_id And a.ord = b.ord And b.doc_id=" + CStr(doc_id) + " and b.locator ='" + locator_id + "') order by a.ord asc"
            vres = gbc_db.executesql_show_no_error(sql)
            If IsArray(vres) Then
                If UBound(vres, 2) > -1 Then
                    If vres(0, 0) = "Error" Then
                        sql = "select a.name, a.ord,a.datatype, a.class_id, a.lookup_sql, b.system_defined,b.default_value, a.system_defined,0 from at_component_parameters a, at_component_document_parameters b" + _
                             " where(a.type_id = b.type_id And a.ord = b.ord And b.doc_id=" + CStr(doc_id) + " and b.locator ='" + locator_id + "') order by a.ord asc"
                        vres = gbc_db.executesql_no_log(sql)
                    End If
                End If
            End If
            get_component_document_parameters = vres
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_standard_display_parameters_db", "get_parameters_for_display_in_template", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            get_component_document_parameters = Nothing
        Finally
            'otrace = New bc_cs_activity_log("bc_om_standard_display_parameters_db", "get_parameters_for_display_in_template", bc_cs_activity_codes.TRACE_EXIT, "",certificate)
        End Try
    End Function
    REM FIL AUG 2013
    Public Function get_List_values(ByVal doc_id As Long, ByVal type_id As Integer, ByVal order As Integer, ByVal locator As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_get_param_list_vals " + CStr(doc_id) + "," + CStr(type_id) + "," + CStr(order) + ",'" + locator + "'"
        get_List_values = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_lookup_values(ByVal sql As String, ByVal additional_params As List(Of String), ByRef certificate As bc_cs_security.certificate, Optional ByVal doc_id As String = "") As Object
        Try
            Dim res As Object

            If bc_cs_central_settings.server_flag = 0 Then
                certificate.user_id = bc_cs_central_settings.logged_on_user_id
            End If
            Dim nsql As String = sql
            If doc_id = "" Then
                nsql = nsql + "," + CStr(certificate.user_id)
            Else
                nsql = nsql + "," + CStr(certificate.user_id) + ",'" + CStr(doc_id) + "'"

            End If
            If IsNothing(additional_params) = False Then

                For i = 0 To additional_params.Count - 1
                    nsql = nsql + ", '" + additional_params(i) + "'"
                Next
            End If

            Dim ocomm As New bc_cs_activity_log("bc_om_component_parameters_db", "get_lookyp_values", bc_cs_activity_codes.COMMENTARY, "running lookup SQL: " + nsql, certificate)

            res = gbc_db.executesql_show_no_error(nsql)

            If IsArray(res) Then

                If UBound(res, 2) > -1 Then
                    If CStr(res(0, 0)) = "Error" Then

                        sql = sql
                        res = gbc_db.executesql(sql, certificate)
                    End If
                End If

            End If
            get_lookup_values = res
        Catch ex As Exception
            get_lookup_values = Nothing
            Dim oerr As New bc_cs_error_log("bc_om_component_parameters_db", "get_lookup_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Public Sub delete_document_parameters_for_locator(ByVal doc_id, ByVal locator)
        Dim sql As String
        sql = "delete from at_component_document_parameters where doc_id=" + CStr(doc_id) + " and locator='" + locator + "'"
        gbc_db.executesql_no_log(sql)
    End Sub
    Public Sub write_document_parameter(ByVal doc_id As Long, ByVal locator As String, ByVal order As Long, ByVal system_defined As String, ByVal default_value As String, ByVal type_id As Long, ByRef certificate As bc_cs_security.certificate,ByRef db As bc_cs_db_services)
        Dim sql As String
        Dim fs As New bc_cs_string_services(default_value)
        default_value = fs.delimit_apostrophies

        If IsNothing(db) = True Then
            sql = "insert into at_component_document_parameters (doc_id, locator, ord,system_defined,default_value, type_id) values(" + CStr(doc_id) + ",'" + locator + "'," + CStr(order) + "," + CStr(system_defined) + ",'" + CStr(default_value) + "'," + CStr(type_id) + ")"
            gbc_db.executesql(sql, certificate)
        Else
            sql = "exec dbo.bc_core_write_document_parameter " + CStr(doc_id) + ",'" + CStr(locator) + "'," + CStr(order) + "," + CStr(system_defined) + ",'" + CStr(default_value) + "'," + CStr(type_id)
            db.executesql_trans(sql, certificate, True)
        End If
    End Sub
    Public Function get_parameters_for_component(ByVal component_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim vres As Object
        REM get newer version with disabled_in_doc
        sql = "select name, ord, datatype,class_id, lookup_sql, system_defined, default_value, coalesce(disabled_in_doc,0), coalesce(mandatory,0) from at_component_parameters where type_id =" + CStr(component_id) + " order by ord asc"
        vres = gbc_db.executesql_show_no_error(sql)
        If IsArray(vres) Then
            If UBound(vres, 2) > -1 Then
                If vres(0, 0) = "Error" Then
                    sql = "select name, ord, datatype,class_id, lookup_sql, system_defined, default_value,0 from at_component_parameters where type_id =" + CStr(component_id) + " order by ord asc"
                    vres = gbc_db.executesql(sql, certificate)
                End If
            End If
        End If
        get_parameters_for_component = vres
    End Function

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_undistribute
    Inherits bc_cs_soap_base_class

    Private doc_id As String
    Private db As bc_cs_db_services
    Public err_text As String
    Private alFiles As New ArrayList
    Public Sub New(ByVal doc_id As String,ByRef db As bc_cs_db_services)
        Me.doc_id = doc_id
        Me.db = db
        err_text = ""
    End Sub

    Public Function get_documents(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        'Dim gbc_db As New bc_cs_db_services
        sql = "BC_core_get_distributed_files_by_doc_id " & doc_id
        get_documents = db.executesql_trans(sql, certificate, True)
    End Function

    Public Function remove_docs(ByRef files_to_commit As ArrayList,ByRef certificate As bc_cs_security.certificate) As Boolean
        remove_docs = True
        Dim res As Object = get_documents(doc_id, certificate)
        If db.success = False Then
            err_text = "Failed to execute BC_core_get_distributed_files_by_doc_id " & doc_id
            remove_docs = False
            Exit Function
        End If
        If IsArray(res) Then
            Dim xdFiles As New XmlDocument
            Try
                Dim xstr As String = ""
                For i = 0 To UBound(res, 2)
                    xstr = xstr + CStr(res(0, i))
                Next

                xdFiles.LoadXml(xstr)
                For Each xdFile As XmlNode In xdFiles.GetElementsByTagName("bc_core_container_controlfile")
                    If Not xdFile.Attributes("controlfile_name") Is Nothing AndAlso Not xdFile.Attributes("controlfile_directory") Is Nothing Then
                        Dim strName, strDirectory, strFile As String
                        strName = xdFile.Attributes("controlfile_name").Value
                        strDirectory = xdFile.Attributes("controlfile_directory").Value
                        If Not strDirectory.Chars(strDirectory.Length - 1) = "\" Then
                            strDirectory = strDirectory & "\"
                        End If
                        strFile = strDirectory & strName
                        If File.Exists(strFile) Then
                            Dim oc As New bc_om_files_to_commit
                            oc.mode = bc_om_files_to_commit.DELETE
                            oc.target_file = strFile
                            files_to_commit.Add(oc)
                        End If
                    End If
                Next
            Catch ex As Exception
                'Dim db_err As New bc_cs_error_log("bc_om_undistribute", "remove_docs", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
                err_text = "Failed to get documents to undistribute" + ex.Message
                remove_docs = False
            End Try
        Else
            remove_docs = False
            err_text = "Failed to get documents to undistribute"
            'Dim db_err As New bc_cs_error_log("bc_om_undistribute", "process_object", bc_cs_error_codes.USER_DEFINED, "Distributed files failed to load.", certificate)
            Exit Function
        End If
    End Function

End Class
'<Serializable()> Public Class bc_om_conflict_check
'    Inherits bc_cs_soap_base_class
'    Private doc_id As String
'    Public err_txt As String
'    Public has_conflicts As Boolean
'    Public conflict_stage_id As Long
'    Public conflict_stage_name As String
'    Public with_email As Boolean
'    Public cdoc As bc_om_document
'    Public err As Boolean
'    Public Sub New()

'    End Sub
'    Public Sub New(ByVal doc_id As String, ByVal with_email As Boolean)
'        Me.doc_id = doc_id
'        Me.with_email = with_email
'    End Sub
'    Public Overrides Sub process_object()
'        Dim otrace As New bc_cs_activity_log("bc_om_conflict_check", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
'        Try
'            REM this is always specific to object
'            If Me.tmode = bc_cs_soap_base_class.tREAD Then
'                db_read()
'            End If
'        Catch ex As Exception
'            Dim db_err As New bc_cs_error_log("bc_om_conflict_check", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
'        Finally
'            otrace = New bc_cs_activity_log("bc_om_conflict_check", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
'        End Try
'    End Sub
'    Public Sub db_read()
'        retrieve_conflict_text()
'    End Sub
'    Private Sub retrieve_conflict_text()
'        Dim otrace As New bc_cs_activity_log("bc_om_conflict_check", "retrieve_conflict_text", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
'        Try
'            err = False
'            REM writes conflicts for docment to database and also attachs them to object model
'            Dim xml_text As String
'            Dim i As Integer
'            Dim ocommentary As bc_cs_activity_log
'            Dim dswebservices As New CommonPlatformWebService.CMDSoapService
'            ocommentary = New bc_cs_activity_log("bc_om_refresh_component", "retrieve_disclosure_text", bc_cs_activity_codes.COMMENTARY, bc_cs_central_settings.common_platform, MyBase.certificate)
'            dswebservices.Url = bc_cs_central_settings.common_platform

'            ocommentary = New bc_cs_activity_log("bc_om_refresh_component", "retrieve_disclosure_text", bc_cs_activity_codes.COMMENTARY, "attempting to Connect to Disclosures at URI: " + bc_cs_central_settings.common_platform + "for doc_id:" + CStr(doc_id), MyBase.certificate)
'            xml_text = dswebservices.conflicts(doc_id)
'            ocommentary = New bc_cs_activity_log("bc_om_refresh_component", "retrieve_disclosure_text", bc_cs_activity_codes.COMMENTARY, "Disclosures Text Returned: " + xml_text, MyBase.certificate)
'            REM now parse out disclosure text
'            Dim xmlload As New Xml.XmlDocument
'            Dim myXmlNodeList As Xml.XmlNodeList

'            xmlload.LoadXml(xml_text)
'            Dim db As New bc_om_conflict_check_db
'            REM delete exiting text
'            db.delete_comply_data_for_doc(Me.doc_id)
'            REM see  if any conlfocys
'            myXmlNodeList = xmlload.SelectNodes("/disclosures/disclosure/entity/name")
'            Dim ds As String
'            If myXmlNodeList.Count > 0 Then
'                ds = myXmlNodeList(0).InnerXml
'                If UCase(ds) = "NONE" Then
'                    has_conflicts = False
'                    Exit Sub
'                End If
'            End If

'            myXmlNodeList = xmlload.SelectNodes("/disclosures/disclosure/text")
'            has_conflicts = False
'            Dim filename As String = ""
'            REM generate conflic file if conflicts
'            Dim fn As StreamWriter = Nothing
'            If myXmlNodeList.Count > 0 Then
'                has_conflicts = True
'                filename = bc_cs_central_settings.central_repos_path + CStr(doc_id) + "conflicts.txt"
'                fn = New StreamWriter(filename, False)
'                fn.WriteLine("Conflict Report: " + Format(Now, "dd-MMM-yyyy"))
'                fn.WriteLine("=============================")
'                fn.WriteLine(" ")
'            End If
'            REM get stage to move
'            REM write new conflict text 
'            Dim s As bc_cs_string_services
'            For i = 0 To myXmlNodeList.Count - 1
'                s = New bc_cs_string_services(myXmlNodeList(i).InnerXml)
'                fn.WriteLine(CStr(i + 1) + ": " + myXmlNodeList(i).InnerXml)
'                db.write_comply_text_for_document(doc_id, "Conflict:" + CStr(i + 1), s.delimit_apostrophies(), MyBase.certificate)
'            Next
'            If myXmlNodeList.Count > 0 Then
'                fn.Close()
'                REM attach this as a support document
'                'Dim cdoc As New bc_om_document
'                'cdoc.certificate = MyBase.certificate
'                'REM get master document
'                'cdoc.register_only = True
'                'cdoc.btake_revision = False
'                'cdoc.bcheck_out = True
'                'cdoc.id = Me.doc_id
'                ''Dim tdoc_id As Long
'                ''tdoc_id = Me.doc_id
'                'cdoc.db_read()
'                'cdoc.id = tdoc_id
'                'ocommentary = New bc_cs_activity_log("bc_om_refresh_component", "retrieve_disclosure_text", bc_cs_activity_codes.COMMENTARY, "DOC ID: " + CStr(doc_id), MyBase.certificate)
'                'cdoc.master_flag = True
'                'cdoc.history.Clear()

'                REM now attach support document
'                Dim support_doc As New bc_om_document
'                support_doc.certificate = MyBase.certificate
'                support_doc.id = 0
'                support_doc.master_flag = False
'                support_doc.pub_type_id = cdoc.pub_type_id
'                support_doc.pub_type_name = cdoc.pub_type_name
'                support_doc.originating_author = cdoc.originating_author
'                support_doc.bus_area = cdoc.bus_area
'                support_doc.checked_out_user = 0
'                support_doc.doc_date = Now
'                support_doc.entity_id = cdoc.entity_id
'                support_doc.originating_author = cdoc.originating_author
'                support_doc.title = "Conflict Report " + Format(Now, "dd-MMM-yyyy")
'                support_doc.extension = ".txt"
'                support_doc.bwith_document = True
'                cdoc.bimport_support_only = True
'                Dim fs As New bc_cs_file_transfer_services
'                fs.write_document_to_bytestream(filename, support_doc.byteDoc, MyBase.certificate)
'                cdoc.support_documents.document.Clear()
'                cdoc.support_documents.document.Add(support_doc)
'                cdoc.history.Clear()
'                cdoc.bcheck_out = False
'                cdoc.bwith_document = False
'                cdoc.certificate = MyBase.certificate
'                cdoc.db_write(MyBase.certificate)
'                fs.delete_file(filename)
'            End If
'            REM email notify piece


'            Dim vstage As Object
'            vstage = db.get_conflict_stage(MyBase.certificate)
'            If IsArray(vstage) Then
'                Try
'                    Me.conflict_stage_id = CLng(vstage(0, 0))
'                    Me.conflict_stage_name = CStr(vstage(1, 0))
'                Catch ex As Exception
'                    ocommentary = New bc_cs_activity_log("bc_om_conflict_check", "retrieve_conflict_text", bc_cs_activity_codes.COMMENTARY, "Could not get conflict stages", MyBase.certificate)
'                    err = True
'                End Try
'            End If
'            REM email notify piece
'            If Me.with_email = True Then
'                ocommentary = New bc_cs_activity_log("bc_om_conflict_check", "retrieve_conflict_text", bc_cs_activity_codes.COMMENTARY, "Conflict Email Requested", MyBase.certificate)
'                db.email_notify(doc_id, Me.conflict_stage_id, 0, CStr(doc_id) + ".txt", MyBase.certificate)
'            End If



'        Catch ex As Exception
'            err = True
'            err_txt = ex.Message

'            'Dim db_err As New bc_cs_error_log("bc_om_conflict_check", "retrieve_conflict_text", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
'        Finally
'            otrace = New bc_cs_activity_log("bc_om_conflict_check", "retrieve_conflict_text", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
'        End Try

'    End Sub






'Public Overloads Overrides Function call_web_service() As String
'    Dim otrace As New bc_cs_activity_log("bc_om_conflict_check", "call_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
'    If IsNumeric(bc_cs_central_settings.timeout) Then
'        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
'        Dim ocommentary As New bc_cs_activity_log("bc_om_conflict_check", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
'    End If
'    Dim s As String

'    s = write_xml_to_string()
'    call_web_service = webservice.LoadConflicts(s)
'    otrace = New bc_cs_activity_log("bc_om_conflict_check", "call_web_service", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
'End Function

'End Class
Public Class bc_om_conflict_check_db
    Private gbc_db As New bc_cs_db_services(False)
    Public Sub delete_comply_data_for_doc(ByVal doc_id As Long)
        Dim sql As String
        sql = "delete from bcc_core_comply_xpath_value_for_doc where doc_id=" + CStr(doc_id) + " and name like 'Conflict%'"
        gbc_db.executesql_show_no_error(sql)
    End Sub

    Public Sub write_comply_text_for_document(ByVal doc_id As Long, ByVal name As String, ByVal rtext As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim fs As New bc_cs_string_services(rtext)
        rtext = fs.delimit_apostrophies()
        sql = "insert into bcc_core_comply_xpath_value_for_doc(doc_id,name,value) values(" + CStr(doc_id) + ",'" + name + "','" + rtext + "')"
        gbc_db.executesql(sql, certificate)
    End Sub
    Public Function get_conflict_stage(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bcc_core_wf_conflict_stage"
        get_conflict_stage = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub email_notify(ByVal doc_id As Long, ByVal stage_id As Long, ByVal mode As Integer, ByVal filename As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bcc_core_wf_email_notify '" + CStr(doc_id) + "','" + CStr(stage_id) + "','" + CStr(mode) + "','" + filename + "'"
        gbc_db.executesql(sql, certificate)
    End Sub
End Class



