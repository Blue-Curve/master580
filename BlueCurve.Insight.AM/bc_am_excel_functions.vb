Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Create.AM
Imports BlueCurve.Core.AS
Imports System.IO
Imports System.Threading
Imports System.Windows.Forms
Imports System.Diagnostics
Imports System.Collections
Imports System.Runtime.InteropServices
Imports System.Text


Public Class bc_am_excel_functions


    Class cell_function
        Public formula As String
        Public res As String
        Public row As Integer
        Public col As Integer
        Public ws As Integer
        Public state As Integer
    End Class
    Dim connection_mode As String
    Dim connection_state As Boolean
    Dim connection_string As String
    Public function_count As Integer
    Public Shared batch_mode As Integer
    Public Shared bulk_write_to_excel As Boolean = False
    Public Shared auto_ready As Boolean = True
    REM Private recalc_cell_count As Integer
    Private recalc_cells_list As New ArrayList
    Private Class recalc_cell
        Public row As Integer
        Public col As Integer
        Public ws As Integer
    End Class
    REM recalc_cells(3, 100000) As Integer

    Public Sub New()
        batch_mode = 4
        Me.connection_state = True
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
            Me.connection_string = "connection error."
            Me.connection_state = False
        End If
    End Sub
    Public Sub set_parameters()

    End Sub

    Public Sub set_batch_mode(ByVal lbatch_mode As Integer)
        batch_mode = lbatch_mode
        REM recalc_cell_count = 0
        recalc_cells_list.Clear()

    End Sub

    Public Function get_batch_mode() As Integer
        get_batch_mode = batch_mode
    End Function
    Public Function get_connection_parameters(ByVal param As String) As String
        If LCase(param) = "user" Then
            get_connection_parameters = bc_cs_central_settings.logged_on_user_name
        ElseIf LCase(param) = "server" Then
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                get_connection_parameters = bc_cs_central_settings.soap_server
            Else
                get_connection_parameters = bc_cs_central_settings.servername + ":" + bc_cs_central_settings.dbname
            End If
        ElseIf LCase(param) = "connection" Then
            get_connection_parameters = bc_cs_central_settings.selected_conn_method
        Else
            get_connection_parameters = "invalid parameter"
        End If
    End Function
    Public batch_efs As New ArrayList
    Public batch_efs_res As New ArrayList
    Public rem_batch_efs_res As New ArrayList


    Public Sub initialise_batch_mode()
        function_count = 0
        batch_efs.Clear()
        batch_efs_res.Clear()
        rem_batch_efs_res.Clear()
    End Sub

    Public Sub clear_batch_mode()

        batch_efs.Clear()
        batch_efs_res.Clear()
        rem_batch_efs_res.Clear()

        batch_efs = Nothing
        batch_efs = Nothing
        rem_batch_efs_res = Nothing

        batch_mode = Nothing
        bulk_write_to_excel = Nothing
        auto_ready = Nothing

        Thread.CurrentThread.Sleep(100)
        GC.Collect()
        Application.DoEvents()

    End Sub


    Public Function execute_all(ByVal excelapp As Object) As Boolean
        Dim ocomm As New bc_cs_activity_log("bc_am_excel_functions", "execute_all", bc_cs_activity_codes.COMMENTARY, "Execute All started")
        Try
            If bc_am_excel_functions.bulk_write_to_excel = True Then
                Exit Function
            End If
            excelapp.screenupdating = False
            excelapp.statusbar = "Evaluating all BC Functions..."
            execute_all = True

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                If bc_am_insight_formats.number_per_batch = 1 Then
                    excelapp.CalculateFull()
                    Exit Function
                End If
            End If


            'If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
            initialise_batch_mode()



            batch_mode = 1
            excelapp.CalculateFull()


            execute_batch()
            batch_mode = 2

            ocomm = New bc_cs_activity_log("bc_am_excel_functions", "execute_all", bc_cs_activity_codes.COMMENTARY, "Starting writing out results")
            excelapp.CalculateFull()
            ocomm = New bc_cs_activity_log("bc_am_excel_functions", "execute_all", bc_cs_activity_codes.COMMENTARY, "Completed writing out results")

            REM manual list dependencies
            If excelapp.Calculation = -4135 Then
                If bc_am_insight_formats.update_lists = 1 Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Do you want to Update List Dependencies?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                    If omsg.cancel_selected = False Then
                        ocomm = New bc_cs_activity_log("bc_am_excel_functions", "execute_all", bc_cs_activity_codes.COMMENTARY, "Starting other updates")
                        batch_mode = 4
                        excelapp.Calculate()
                        Me.execute_batch_mode_changes(excelapp, excelapp.calculation)
                    End If
                End If
            End If
            'Else
            'excelapp.CalculateFull()
            'End If
        Catch ex As Exception
            execute_all = False
            Dim oerr As New bc_cs_error_log("bc_am_excel_functions", "execute_all", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            excelapp.statusbar = Nothing
            If bc_am_excel_functions.bulk_write_to_excel = False Then
                excelapp.screenupdating = True
            End If
            ocomm = New bc_cs_activity_log("bc_am_excel_functions", "execute_all", bc_cs_activity_codes.COMMENTARY, "Execute All completed")
            batch_mode = 4

        End Try
    End Function
    Public Function execute_selection(ByVal excelapp As Object, ByVal calc_mode As Integer) As Boolean
        Try
            excelapp.ScreenUpdating = False
            execute_selection = True
            Dim sr, sc, er, ec As Integer
            sr = excelapp.Selection.Cells(1).Row
            sc = excelapp.Selection.Cells(1).Column
            er = excelapp.Selection.Cells(excelapp.Selection.Cells.Count).Row
            ec = excelapp.Selection.Cells(excelapp.Selection.Cells.Count).Column

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                If bc_am_insight_formats.number_per_batch = 1 Then
                    excelapp.Range(excelapp.Cells(sr, sc), excelapp.Cells(er, ec)).Calculate()
                    Exit Function
                End If
            End If


            'If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
            excelapp.statusbar = "Executing selected BC Functions"
            excelapp.Calculation = -4135
            initialise_batch_mode()
            batch_mode = 1


            excelapp.Range(excelapp.Cells(sr, sc), excelapp.Cells(er, ec)).Calculate()

            execute_batch()

            batch_mode = 2
            excelapp.Range(excelapp.Cells(sr, sc), excelapp.Cells(er, ec)).Calculate()
            'excelapp.batch_mode = 4
            If calc_mode = -4105 Or calc_mode = 2 Then
                REM recalc_cell_count = 0
                recalc_cells_list.Clear()
                batch_mode = 5
                excelapp.Calculation = calc_mode
            End If

            'Else
            'excelapp.Range(excelapp.Cells(sr, sc), excelapp.Cells(er, ec)).Calculate()
            'End If
        Catch ex As Exception
            execute_selection = False
            Dim oerr As New bc_cs_error_log("bc_am_excel_functions", "execute_selection", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            batch_mode = 4
            excelapp.statusbar = Nothing
            excelapp.ScreenUpdating = True
        End Try

    End Function

    Public Function execute_batch_mode_changes(ByVal excelapp As Object, ByVal calc_mode As Integer) As Boolean
        Dim pscreenupdating As Boolean

        Try
            pscreenupdating = excelapp.screenupdating
            auto_ready = False
            If bc_am_excel_functions.bulk_write_to_excel = True Then

                Exit Function
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                If bc_am_insight_formats.number_per_batch = 1 Then
                    Exit Function
                End If
            End If
            ' If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
            excelapp.screenupdating = False
            excelapp.statusbar = "Evaluating Changed BC Functions..."
            If batch_mode <> 4 Then
                Exit Function
            End If
            execute_batch_mode_changes = True
            Dim i As Integer
            'If recalc_cell_count = 0 Then
            '    Exit Function
            'End If

            If recalc_cells_list.Count = 0 Then
                Exit Function
            End If

            'PR MAY 2018 do all to slove sheet change issues with / frmulas
            If calc_mode = -4105 Or calc_mode = 2 Then
                execute_all(excelapp)
                Exit Function
            End If

            initialise_batch_mode()
            excelapp.Calculation = -4135

            'modFunctions.batch_mode = 3
            'Application.Calculate
            batch_mode = 1

            For i = 0 To recalc_cells_list.Count - 1
                excelapp.worksheets(recalc_cells_list(i).ws).Cells(recalc_cells_list(i).row, recalc_cells_list(i).col).Calculate()
            Next
            'For i = 0 To recalc_cell_count - 1
            '    excelapp.worksheets(recalc_cells(2, i)).Cells(recalc_cells(0, i), recalc_cells(1, i)).Calculate()
            'Next

            execute_batch()
            batch_mode = 2

            'For i = 0 To recalc_cell_count - 1
            'excelapp.worksheets(recalc_cells(2, i)).Cells(recalc_cells(0, i), recalc_cells(1, i)).Calculate()
            'Next

            For i = 0 To recalc_cells_list.Count - 1
                excelapp.worksheets(recalc_cells_list(i).ws).Cells(recalc_cells_list(i).row, recalc_cells_list(i).col).Calculate()
            Next

            batch_mode = 0
            REM  recalc_cell_count = 0
            recalc_cells_list.Clear()

            If calc_mode = -4105 Or calc_mode = 2 Then
                
                batch_mode = 5
                excelapp.Calculation = calc_mode

            End If

            'End If
        Catch ex As Exception
            execute_batch_mode_changes = False
            Dim oerr As New bc_cs_error_log("bc_am_excel_functions", "execute_batch_mode_changes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            If bc_am_excel_functions.bulk_write_to_excel = False Then
                excelapp.screenupdating = pscreenupdating
            End If
            excelapp.statusbar = Nothing
            batch_mode = 4
            recalc_cells_list.Clear()
            auto_ready = True

            REM recalc_cell_count = 0
            Thread.CurrentThread.Sleep(100)
            Dim ocomm As New bc_cs_activity_log("hhh", "nnn", bc_cs_activity_codes.COMMENTARY, CStr(auto_ready))

            '-- steve 31/12/2016 Clear Memory
            batch_mode = Nothing
            bulk_write_to_excel = Nothing
            auto_ready = Nothing
            Application.DoEvents()

        End Try
    End Function
    Private Class executed_functions
        Public row As Integer
        Public col As Integer
        Public ord As Integer
        Public val As String
    End Class

    Public Function execute_batch() As Boolean

        Dim oef As New bc_om_excel_functions
        Dim ocomm As bc_cs_activity_log
        Dim num_batches As Integer = 1
        Dim dnum_batches As Double
        Dim ores As cell_function
        REM PR
        Dim ic As New bc_am_insight_formats

        Try


            execute_batch = True


            For i = 0 To batch_efs.Count - 1
                oef.batch_efs.Add(batch_efs(i).formula)
                oef.batch_results.Add("")
                oef.batch_results_list.Add("")
            Next


            If bc_am_insight_formats.number_per_batch = 0 Then
                ocomm = New bc_cs_activity_log("bc_am_excel_functions", "execute_batch", bc_cs_activity_codes.COMMENTARY, "Executing: " + CStr(batch_efs.Count) + " functions in batch mode in 1 batch")
                oef.number_per_batch = 0
            Else
                dnum_batches = batch_efs.Count / bc_am_insight_formats.number_per_batch
                num_batches = dnum_batches + 0.5
                ocomm = New bc_cs_activity_log("bc_am_excel_functions", "execute_batch", bc_cs_activity_codes.COMMENTARY, "Executing: " + CStr(batch_efs.Count) + " functions in  batch mode in " + CStr(num_batches) + " batches")
                oef.number_per_batch = bc_am_insight_formats.number_per_batch
            End If
            REM FIL 5.5
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                oef.db_batch_mode = bc_am_insight_formats.db_batch_mode
                ocomm = New bc_cs_activity_log("bc_am_excel_functions", "execute_batch", bc_cs_activity_codes.COMMENTARY, "BATCH MODE " + CStr(bc_am_insight_formats.db_batch_mode))

                If bc_am_insight_formats.db_batch_mode = 0 Then

                    REM run in single mode
                    oef.tmode = bc_cs_soap_base_class.tREAD
                    For i = 0 To num_batches - 1
                        oef.batch_count = i + 1
                        ocomm = New bc_cs_activity_log("bc_am_excel_functions", "execute_batch", bc_cs_activity_codes.COMMENTARY, "Running batch: " + CStr(i + 1) + " of: " + CStr(num_batches))
                        If oef.transmit_to_server_and_receive(oef, True) = True And execute_batch = True Then
                            execute_batch = True
                        Else
                            execute_batch = False
                            Exit Function
                        End If
                    Next
                    'Dim ores As cell_function
                    ores = Nothing

                   

                    For i = 0 To batch_efs.Count - 1
                        batch_efs(i).res = oef.batch_results(i)
                        ores = New cell_function
                        ores.row = batch_efs(i).row
                        ores.ws = batch_efs(i).ws
                        ores.col = batch_efs(i).col
                        ores.res = batch_efs(i).res
                        batch_efs_res.Add(ores)
                    Next


                Else

                    oef.batch_count = 0
                    oef.tmode = bc_cs_soap_base_class.tREAD
                    REM FIL 5.5 all done on database now
                    REM load function definitions to database
                    oef.load_efs_flag = True
                    For i = 0 To num_batches - 1
                        oef.batch_count = i + 1
                        ocomm = New bc_cs_activity_log("bc_am_excel_functions", "execute_batch", bc_cs_activity_codes.COMMENTARY, "Loading batch: " + CStr(i + 1) + " of: " + CStr(num_batches))
                        If oef.transmit_to_server_and_receive(oef, True) = True And execute_batch = True Then
                            execute_batch = True
                        Else
                            execute_batch = False
                            Exit Function
                        End If
                    Next
                    oef.load_efs_flag = False
                    REM =====
                    oef.batch_count = 0

                    For i = 0 To num_batches - 1
                        oef.batch_count = i + 1
                        ocomm = New bc_cs_activity_log("bc_am_excel_functions", "execute_batch", bc_cs_activity_codes.COMMENTARY, "Running batch: " + CStr(i + 1) + " of: " + CStr(num_batches))
                        If oef.transmit_to_server_and_receive(oef, True) = True And execute_batch = True Then
                            execute_batch = True
                        Else
                            execute_batch = False
                            Exit Function
                        End If
                    Next

                    REM FIL 5.5 get results back from database
                    oef.tmode = bc_cs_soap_base_class.tREAD
                    REM FIL 5.5 all done on database now
                    REM load function defintions to database
                    oef.get_results = True
                    oef.batch_count = 0

                    For i = 0 To num_batches - 1
                        ocomm = New bc_cs_activity_log("bc_am_excel_functions", "execute_batch", bc_cs_activity_codes.COMMENTARY, "Retrieving batch: " + CStr(i + 1) + " of: " + CStr(num_batches))
                        oef.batch_count = i + 1
                        If oef.transmit_to_server_and_receive(oef, True) = True And execute_batch = True Then
                            execute_batch = True
                        Else
                            execute_batch = False
                            Exit Function
                        End If
                    Next

                    oef.get_results = False
                    REM =======

                    'Dim ores As cell_function
                    ores = Nothing

                    For i = 0 To batch_efs.Count - 1
                        batch_efs(i).res = oef.batch_results(i)
                        ores = New cell_function
                        ores.row = batch_efs(i).row
                        ores.ws = batch_efs(i).ws
                        ores.col = batch_efs(i).col
                        ores.res = batch_efs(i).res

                        batch_efs_res.Add(ores)
                    Next
                End If
            Else

                REM PG batch all calls on db
                REM Dim omsg As New bc_cs_message("Blue Curve", "Batch mode only implemented for SOAP", bc_cs_message.MESSAGE)
                oef.batch_count = 0
                REM FIL 5.5 all done on database now
                REM load function definitions to database
                oef.load_efs_flag = True
                For i = 0 To num_batches - 1
                    oef.batch_count = i + 1
                    ocomm = New bc_cs_activity_log("bc_am_excel_functions", "execute_batch", bc_cs_activity_codes.COMMENTARY, "Loading batch: " + CStr(i + 1) + " of: " + CStr(num_batches))
                    oef.db_read_batch()
                    execute_batch = True
                Next
                oef.load_efs_flag = False
                REM =====
                oef.batch_count = 0

                For i = 0 To num_batches - 1
                    oef.batch_count = i + 1
                    ocomm = New bc_cs_activity_log("bc_am_excel_functions", "execute_batch", bc_cs_activity_codes.COMMENTARY, "Running batch: " + CStr(i + 1) + " of: " + CStr(num_batches))
                    oef.db_read_batch()
                    execute_batch = True
                Next

                REM FIL 5.5 get results back from database
                oef.tmode = bc_cs_soap_base_class.tREAD
                REM FIL 5.5 all done on database now
                REM load function defintions to database
                oef.get_results = True
                oef.batch_count = 0

                For i = 0 To num_batches - 1
                    ocomm = New bc_cs_activity_log("bc_am_excel_functions", "execute_batch", bc_cs_activity_codes.COMMENTARY, "Retrieving batch: " + CStr(i + 1) + " of: " + CStr(num_batches))
                    oef.batch_count = i + 1
                    oef.db_read_batch()
                    execute_batch = True
                Next

                oef.get_results = False
                REM =======

                'Dim ores As cell_function
                ores = Nothing
                For i = 0 To batch_efs.Count - 1
                    batch_efs(i).res = oef.batch_results(i)
                    ores = New cell_function
                    ores.row = batch_efs(i).row
                    ores.ws = batch_efs(i).ws
                    ores.col = batch_efs(i).col
                    ores.res = batch_efs(i).res

                    batch_efs_res.Add(ores)
                Next
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_excel_functions", "execute_batch", bc_cs_error_codes.USER_DEFINED, ex.Message)


        Finally

            '-- steve 31/12/2016 Clear Memory
            ocomm = Nothing
            num_batches = Nothing
            dnum_batches = Nothing
            ic = Nothing
            ores = Nothing
            oef = Nothing
            Application.DoEvents()

        End Try


    End Function
    Public Function execute_function(ByVal excelapp As Object, ByVal str As String) As String
        REM load dont do an authenticate load as this is to slow
        execute_function = ""
        Dim cf As cell_function = Nothing
        Dim oef As New bc_om_excel_functions

        Try

            If Me.connection_state = False Then
                execute_function = Me.connection_string
            Else
                Dim s As New bc_cs_string_services(str)
                str = s.delimit_apostrophies
                str = str.Replace("&", "amp;")

                'Dim osql As New bc_om_sql("exec dbo.bcc_core_v5_excel_function '" + str + "','" + CStr(bc_cs_central_settings.logged_on_user_id) + "'")
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    'Dim ic As New bc_am_insight_formats

                    If bc_am_insight_formats.number_per_batch = 1 Then
                        Dim osql As New bc_om_sql("exec dbo.bcc_core_v5_excel_function '" + str + "','" + CStr(bc_cs_central_settings.logged_on_user_id) + "'")
                        osql.db_read()
                        If IsArray(osql.results) Then
                            execute_function = osql.results(0, 0)
                            Exit Function
                        End If
                    End If
                End If

                ''Dim oef As New bc_om_excel_functions


                Select Case batch_mode

                    Case 0
                        Dim ls As String = bc_cs_central_settings.activity_file
                        bc_cs_central_settings.activity_file = "off"
                        Dim ocomm As New bc_cs_activity_log("bc_am_excel_functions", "execute_functions", bc_cs_activity_codes.COMMENTARY, "Executing in batch mode 0")
                        REM no batch

                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then


                            If oef.execute_function(str) = False Then
                                Dim ocommentary As New bc_cs_activity_log("bc_am_excel_functions", "execute_function", bc_cs_activity_codes.COMMENTARY, "Network Error Executing Excel Function:" + str, Nothing)
                                execute_function = "Network Error"
                            Else
                                If Left(oef.result, 9) = "DB Error:" Then
                                    execute_function = "DB Err"
                                    Dim ocommentary As New bc_cs_activity_log("bc_am_excel_functions", "execute_function", bc_cs_activity_codes.COMMENTARY, "Database Error:" + oef.result, Nothing)
                                Else
                                    execute_function = oef.result
                                End If
                            End If
                            bc_cs_central_settings.activity_file = ls
                        Else
                            Dim osql As New bc_om_sql("exec dbo.bcc_core_v5_excel_function '" + str + "','" + CStr(bc_cs_central_settings.logged_on_user_id) + "'")
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                osql.db_read()
                                If IsArray(osql.results) Then
                                    execute_function = osql.results(0, 0)
                                End If
                            End If
                        End If
                        batch_mode = 4
                    Case 1

                        REM set batch
                        cf = New cell_function
                        cf.formula = str
                        cf.row = excelapp.Caller.Offset(0, 0).Row
                        cf.col = excelapp.Caller.Offset(0, 0).column
                        cf.state = 0
                        cf.ws = excelapp.Caller.Offset(0, 0).worksheet.index


                        Me.batch_efs.Add(cf)
                        execute_function = "eval[" + CStr(Me.function_count) + "]"
                        Me.function_count = Me.function_count + 1
                    Case 2


                        execute_function = ""
                        REM rectrieve batch result
                        For i = 0 To Me.batch_efs.Count - 1
                            If Me.batch_efs(i).ws = excelapp.Caller.Offset(0, 0).worksheet.index And Me.batch_efs(i).row = excelapp.Caller.Offset(0, 0).Row And Me.batch_efs(i).col = excelapp.Caller.Offset(0, 0).column Then
                                execute_function = Me.batch_efs(i).res


                                Me.batch_efs.RemoveAt(i)



                                Exit For
                            End If
                        Next

                    Case 4

                        REM add cell that need recaulating to list
                        REM only do if not a duplicate
                        Dim found As Boolean
                        Dim row, col, ws As Integer
                        row = excelapp.Caller.Offset(0, 0).Row
                        col = excelapp.Caller.Offset(0, 0).column
                        ws = excelapp.Caller.Offset(0, 0).worksheet.index

                        found = False
                        REM if already exists remove original and add at end
                        For i = 0 To recalc_cells_list.Count - 1
                            If recalc_cells_list(i).row = row And recalc_cells_list(i).col = col And row And recalc_cells_list(i).ws = ws Then
                                recalc_cells_list.RemoveAt(i)
                                Exit For
                            End If
                        Next
                        Dim rc As New recalc_cell
                        rc.row = row
                        rc.col = col
                        rc.ws = ws
                        recalc_cells_list.Add(rc)

                        REM For i = 0 To recalc_cell_count - 1
                        REM If recalc_cells(0, i) = row And recalc_cells(1, i) = col And row And recalc_cells(2, i) = ws Then
                        REM found = True
                        REM Exit For
                        REM End If
                        REM Next
                        REM If found = False Then
                        REM recalc_cells(0, recalc_cell_count) = row
                        REM recalc_cells(1, recalc_cell_count) = col
                        REM recalc_cells(2, recalc_cell_count) = ws
                        REM recalc_cell_count = recalc_cell_count + 1
                        REM End If

                        execute_function = excelapp.worksheets(ws).cells(excelapp.Caller.Offset(0, 0).Row, excelapp.Caller.Offset(0, 0).column).text

                    Case 5


                        Dim found As Boolean = False
                        For i = 0 To Me.batch_efs_res.Count - 1
                            If Me.batch_efs_res(i).ws = excelapp.Caller.Offset(0, 0).worksheet.index And Me.batch_efs_res(i).row = excelapp.Caller.Offset(0, 0).Row And Me.batch_efs_res(i).col = excelapp.Caller.Offset(0, 0).column Then
                                execute_function = Me.batch_efs_res(i).res



                                REM set batch
                                cf = New cell_function
                                cf.formula = str
                                cf.row = Me.batch_efs_res(i).row
                                cf.col = Me.batch_efs_res(i).col
                                cf.state = 0
                                cf.ws = Me.batch_efs_res(i).ws
                                cf.res = Me.batch_efs_res(i).res
                                Me.rem_batch_efs_res.Add(cf)
                                Me.batch_efs_res.RemoveAt(i)
                                found = True
                                Exit Function
                            End If
                        Next
                        If found = False Then
                            'For i = 0 To Me.rem_batch_efs_res.Count - 1
                            '    If Me.rem_batch_efs_res(i).ws = excelapp.Caller.Offset(0, 0).worksheet.index And Me.rem_batch_efs_res(i).row = excelapp.Caller.Offset(0, 0).Row And Me.rem_batch_efs_res(i).col = excelapp.Caller.Offset(0, 0).column Then
                            '        execute_function = Me.rem_batch_efs_res(i).res
                            '        'Me.rem_batch_efs_res.RemoveAt(i)
                            '        If excelapp.Caller.Offset(0, 0).Row = 16 Then
                            '            Dim ocomm As New bc_cs_activity_log("zzz", "aaa", bc_cs_activity_codes.COMMENTARY, CStr(excelapp.Caller.Offset(0, 0).Row) + ":" + CStr(excelapp.Caller.Offset(0, 0).column) + ": " + Me.rem_batch_efs_res(i).res, Nothing)
                            '        End If
                            '        Exit For
                            '    End If
                            'Next
                            For i = 0 To Me.rem_batch_efs_res.Count - 1
                                If Me.rem_batch_efs_res(Me.rem_batch_efs_res.Count - 1 - i).ws = excelapp.Caller.Offset(0, 0).worksheet.index And Me.rem_batch_efs_res(Me.rem_batch_efs_res.Count - 1 - i).row = excelapp.Caller.Offset(0, 0).Row And Me.rem_batch_efs_res(Me.rem_batch_efs_res.Count - 1 - i).col = excelapp.Caller.Offset(0, 0).column Then
                                    execute_function = Me.rem_batch_efs_res(Me.rem_batch_efs_res.Count - 1 - i).res


                                    rem_batch_efs_res.RemoveAt(Me.rem_batch_efs_res.Count - 1 - i)

                                    Exit For
                                End If
                            Next


                        End If
                End Select
            End If

            'Steve
            'oef = Nothing

            'End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_excel_functions", "execute_function", bc_cs_error_codes.USER_DEFINED, ex.Message + " batch mode: " + batch_mode)
        End Try

    End Function
    Public Function execute_function(ByVal str As String) As String
        REM load dont do an authenticate load as this is to slow
        execute_function = ""
        Try

            If Me.connection_state = False Then
                execute_function = Me.connection_string
            Else
                Dim s As New bc_cs_string_services(str)
                str = s.delimit_apostrophies
                str = str.Replace("&", "amp;")

                Dim osql As New bc_om_sql("exec dbo.bcc_core_v5_excel_function '" + str + "','" + CStr(bc_cs_central_settings.logged_on_user_id) + "'")
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                    If IsArray(osql.results) Then
                        execute_function = osql.results(0, 0)
                    End If

                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then


                    Dim oef As New bc_om_excel_functions

                    REM no batch
                    If oef.execute_function(str) = False Then
                        Dim ocommentary As New bc_cs_activity_log("bc_am_excel_functions", "execute_function", bc_cs_activity_codes.COMMENTARY, "Network Error Executing Excel Function:" + str, Nothing)
                        execute_function = "Network Error"
                    Else
                        If Left(oef.result, 9) = "DB Error:" Then
                            execute_function = "DB Err"
                            Dim ocommentary As New bc_cs_activity_log("bc_am_excel_functions", "execute_function", bc_cs_activity_codes.COMMENTARY, "Database Error:" + oef.result, Nothing)
                        Else
                            execute_function = oef.result
                        End If
                    End If
                End If

            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_excel_functions", "execute_function", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
    Public Function execute(ByVal sql As String) As Object
        Dim slog = New bc_cs_activity_log("bc_am_sql", "execute", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim ocommentary As bc_cs_activity_log

            Dim osql As New bc_om_sql(sql)

            If bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Executing SQL via SOAP.")
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
                Return osql.results
            ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Executing SQL via ADO direct.")
                osql.db_read()
                Return osql.results
            ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.LOCAL Then
                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Cannot execute SQL as Connected Locally Only!")
                Return Nothing
            Else
                ocommentary = New bc_cs_activity_log("bc_am_at_refresh", "new", bc_cs_activity_codes.COMMENTARY, "Connection Method: " + bc_cs_central_settings.connection_method + " not supported!")
                Return Nothing
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_sql", "execute", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Return Nothing
        Finally
            slog = New bc_cs_activity_log("bc_am_sql", "execute", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Function bc_format(ByVal value As String, Optional ByVal format_type As String = "decimal places", Optional ByVal places As String = "2", Optional ByVal scale_factor As String = "1", Optional ByVal item As String = "") As String

        If IsNumeric(value) <> True Then
            bc_format = value
            Exit Function
        End If
        If format_type <> "decimal places" And format_type <> "significant figures" And format_type <> "dps" And format_type <> "sfs" Then
            bc_format = "err: valid format types are decimal places, significant figures"
            Exit Function
        End If
        If IsNumeric(places) <> True Then
            bc_format = "err: places must be numeric"
            Exit Function
        End If
        If IsNumeric(scale_factor) = True Then
            value = value / scale_factor
        End If
        If format_type = "decimal places" Or format_type = "dps" Then
            value = Math.Round(CDbl(value), CInt(places))

        Else
            value = sigfigs(value, places)
        End If
        bc_format = value
    End Function
    Private Function sigfigs(ByVal value As String, ByVal sfs As String) As String
        Try
            Dim dp As Integer
            Dim mult As Double
            Dim tvalue As Double
            Dim f As Double
            Dim minus As Boolean
            minus = False
            If Left(value, 1) = "-" Then
                value = Right(value, Len(value) - 1)
                minus = True
            End If
            If sfs = "0" Then
                sigfigs = 0
                Exit Function
            End If

            Dim i As Integer
            dp = InStr(1, value, ".")

            If dp > 0 Then
                mult = Len(value) - dp
                mult = Math.Pow(10, mult)
                value = value * mult
            End If
            If Len(value) >= sfs Then
                f = Len(value) - sfs
                f = Math.Pow(10, f)
                tvalue = value / f
                tvalue = Math.Round(tvalue, 0)
                sigfigs = CStr(tvalue)
                For i = 0 To Len(value) - sfs - 1
                    sigfigs = sigfigs + "0"
                Next
            Else
                sigfigs = value
                For i = Len(value) To sfs
                    sigfigs = sigfigs + "0"
                Next
            End If
            If dp > 0 Then
                sigfigs = Left(sigfigs, dp - 1) + "." + Right(sigfigs, Len(sigfigs) - dp + 1)
                For i = 0 To Len(sigfigs) - 1
                    If Right(sigfigs, 1) = "0" Then
                        sigfigs = Left(sigfigs, Len(sigfigs) - 1)
                    End If
                Next
                If Right(sigfigs, 1) = "." Then
                    sigfigs = Left(sigfigs, Len(sigfigs) - 1)
                End If
            End If
            If minus = True Then
                sigfigs = "-" + sigfigs
            End If
        Catch
            sigfigs = value
        End Try
    End Function
End Class
Public Class bc_am_ef_after_format
    Public application As Object
    Public singlecell As Object
    Public activecell As Object
    Public activesheet As Object
    Public dimensions As String
    Public multiple As String
    Public datatype As Integer
    Public str As String
    'Public Shared inprogress As Integer = 0
    'Public Shared function_count As Integer = 0
    Public batch As Boolean = False
    Public function_count As Integer
    'Public Shared total_count As Integer = 0
    Public Sub New(ByVal application As Object, ByVal singlecell As Object, ByVal activesheet As Object, ByVal activecell As Object, ByVal str As String, ByVal dimensions As String, ByVal multiple As Boolean, ByVal batch As Boolean, ByVal function_count As Integer, Optional ByVal datatype As Integer = 0)
        REM dont do a single cell result that hasnt errored.
        Try
            Dim tthread As New System.Threading.Thread(AddressOf do_format)
            Me.datatype = datatype
            Me.str = str
            Me.batch = batch
            Me.application = application
            Me.activecell = activecell
            Me.activesheet = activesheet
            Me.dimensions = dimensions
            Me.multiple = multiple
            Me.function_count = function_count
            Me.singlecell = singlecell

            tthread.Start()
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_am_ef_after_format", "new", bc_cs_activity_codes.COMMENTARY, "a1:" + ex.Message)

        End Try


    End Sub

    Private Sub do_format()

        Try
            Dim ftext As String
            'inprogress = inprogress + 1

            Try
                ftext = "Evaluating BC excel function(s)..."
                If Me.batch = False Then
                    application.statusbar = ftext
                Else
                    ftext = "Evaluating BC excel function: " + CStr(function_count)
                    application.statusbar = ftext
                End If
            Catch ex As Exception

                Dim ocomm As New bc_cs_activity_log("bc_am_ef_after_format", "do_format", bc_cs_activity_codes.COMMENTARY, "111:" + ex.Message)

            End Try

            format_output(str, activesheet, activecell, dimensions, multiple, datatype)

        Catch ex As Exception
            MsgBox(ex.Message)

        Finally
            Try
                application.statusbar = False
            Catch
            End Try


            application = Nothing
            'activecell = Nothing
            activesheet = Nothing
            singlecell = Nothing
            GC.Collect()
        End Try


    End Sub
    Private Sub format_output(ByVal list As String, ByVal activesheet As Object, ByVal activecell As Object, ByVal dimensions As String, Optional ByVal multiple As Boolean = True, Optional ByVal datatype As Integer = 0)

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim ocomm As New bc_cs_activity_log("bc_am_ef_after_format", "format_output", bc_cs_activity_codes.COMMENTARY, "Entering format1")



            Dim r, ir, sr, i As Integer
            Dim c, ic, sc As Integer
            Dim dc As Integer
            Dim k As String
            Dim num_dimensions As Integer

            REM PR loop until rpc server is available
            Dim icount As Integer = 0

            Dim err As Boolean
            err = True


            While err = True
                Try
                    c = activecell.Column
                    r = activecell.Row
                    Thread.Sleep(100)
                    err = False
                Catch ex As Exception
                    icount = icount + 1
                    ocomm = New bc_cs_activity_log("bc_am_ef_after_format", "format_output", bc_cs_activity_codes.COMMENTARY, "acticecell: " + ex.Message)
                    If icount = 100 Then
                        Exit While
                    End If
                End Try
            End While

            REM if error write comment
            Try
                If batch = False Then
                    sc = singlecell.column
                    sr = singlecell.row
                    If sc = c And sr = r Then
                        activesheet.Cells(r, c).select()
                        activesheet.application.selection.ClearComments()
                        If datatype = 3 Then
                            activesheet.cells(r, c).numberformat = "dd/mm/yyyy"
                        End If
                    End If
                End If
            Catch ex As Exception

            End Try
            If InStr(str, ";") = 0 And InStr(str, "err:") = 0 And multiple = False Then
                ocomm = New bc_cs_activity_log("bc_am_ef_after_format", "format_output", bc_cs_activity_codes.COMMENTARY, "No furthur processing")
                REM no furthur processing required
                Exit Sub
            End If

            If list = "" Or Left(list, 4) = "err:" Then

                num_dimensions = evaluate_num_dimensions(dimensions)
                For i = c + 1 To c + num_dimensions
                    If batch = False Then
                        activesheet.cells(r, i).value = ""
                    End If
                Next
            End If

            If Left(list, 4) = "err:" Then
                Try
                    If batch = False Then
                        If sc = c And sr = r Then
                            activesheet.Cells(r, c).select()
                            activesheet.application.selection.addcomment(list)
                        End If
                    End If
                Catch ex As Exception

                End Try
            Else

                num_dimensions = evaluate_num_dimensions(dimensions)
                dc = 0
                k = InStr(1, list, ";")
                If k = 0 And multiple = False Then
                    Exit Sub
                End If
                ir = r
                ic = c
                list = bc_asp_get_list_truncate(list)
                Dim ostr As String
                While list <> ""
                    k = InStr(1, list, ";")
                    If k > 0 Then
                        If num_dimensions > dc Then
                            dc = dc + 1
                        Else
                            r = r + 1
                            dc = 0
                        End If
                        If CStr(activesheet.Cells(r, c + dc).value) <> CStr(Left(list, k - 1)) Then
                            ostr = Left(list, k - 1)
                            If IsNumeric(ostr) = True Then
                                activesheet.Cells(r, c + dc) = CDbl(ostr)
                            ElseIf IsDate(ostr) = True Then
                                activesheet.Cells(r, c + dc) = CDate(ostr)
                            Else
                                activesheet.Cells(r, c + dc) = ostr
                            End If
                        End If
                    Else
                        If CStr(activesheet.Cells(r, c + dc + 1).value) <> CStr(list) Then
                            REM ostr = Left(list, k - 1)
                            If IsNumeric(list) = True Then
                                activesheet.Cells(r, c + dc + 1) = CDbl(list)
                            ElseIf IsDate(list) = True Then
                                activesheet.Cells(r, c + dc + 1) = CDate(list)
                            Else
                                activesheet.Cells(r, c + dc + 1) = list
                            End If
                        End If
                    End If
                    list = bc_asp_get_list_truncate(list)
                End While



            End If

            REM clear remaining items
            If multiple = True Then
                activesheet.Cells(r + 1, c).value = "END"
                For i = c + 1 To c + num_dimensions
                    activesheet.cells(r + 1, i).value = ""
                Next
                Dim start_r As Integer = 0
                Dim start_c As Integer = 0
                start_r = r + 2
                start_c = c
                Try
                    While CStr(activesheet.cells(start_r, start_c).value) <> "" And CStr(activesheet.cells(start_r, start_c).value) <> "END"
                        activesheet.cells(start_r, start_c).value = ""
                        For i = start_c + 1 To start_c + num_dimensions
                            activesheet.cells(start_r, i).value = ""
                        Next
                        start_r = start_r + 1
                    End While
                    If activesheet.cells(start_r, start_c).value = "END" Then
                        activesheet.cells(start_r, start_c).value = ""
                        For i = start_c + 1 To start_c + num_dimensions
                            activesheet.cells(start_r, i).value = ""
                        Next
                    End If
                Catch ex As Exception

                End Try
            End If


        Catch ex As Exception



        Finally

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Sub

    Private Function evaluate_num_dimensions(ByVal dimensions As String)
        Dim r As Integer
        r = 0
        If InStr(1, dimensions, ",") = 0 Then
            Return 0
        End If
        While dimensions <> ""
            r = r + 1
            dimensions = bc_asp_get_list_truncate(dimensions, ",")
        End While
        Return r - 1
    End Function
    Public Function bc_asp_get_list_truncate(ByVal list As String, Optional ByVal seperator As String = ";") As String
        Dim k As String
        k = InStr(1, list, seperator)
        If k > 0 Then
            bc_asp_get_list_truncate = Right(list, Len(list) - k)
        Else
            bc_asp_get_list_truncate = ""
        End If
    End Function

End Class
Public Class bc_am_ef_functions
    Public application As Object
    Public Shared excel_functions As New ArrayList
    Public Shared stage_ids As New ArrayList
    Public Shared stage_names As New ArrayList
    Public Shared class_ids As New ArrayList
    Public Shared class_names As New ArrayList
    Public Shared entities As New ArrayList
    Public Shared contributor_ids As New ArrayList
    Public Shared contributor_names As New ArrayList
    Public Shared period_ids As New ArrayList
    Public Shared period_names As New ArrayList
    Public Shared item_types As New ArrayList
    Public Shared item_names As New ArrayList
    Public Shared item_factors As New ArrayList
    Public Shared item_monatarys As New ArrayList
    Public Shared item_symbols As New ArrayList
    Public Shared template_ids As New ArrayList
    Public Shared template_names As New ArrayList
    Public Shared currency_codes As New ArrayList
    Public Shared schema_ids As New ArrayList
    Public Shared schema_names As New ArrayList
    Public Shared class_links As New ArrayList
    Public Shared chart_tools As New ArrayList
    Public Shared aggs As New List(Of bc_om_agg_entity)

    Public Shared ousersettings As New bc_om_portoflio_client_settings

    Public chart_method As String
    Public chart_passrangeasparam As Boolean
    Public chart_active As Boolean
    Public chart_data As Boolean
    Public dataRange As String
    Public chart_sheet_name As String

    Public start_row As Integer
    Public start_col As Integer
    Public end_row As Integer
    Public end_col As Integer
    Public bc_am_excel As Object
    Public portfolio_Id As Long = 0
    Dim off As bc_am_portfolio_wizard
    Private Class formulas_header
        Public title As String
        Public stage As String
        Public contributor As String
        Public date_at As DateTime
        Public date_from As DateTime
        Public date_to As DateTime
        Public entity_class As String
        Public row_offset As Integer
        Public col_offset As Integer
        Public item_row_offset As Integer
        Public year_row_offset As Integer
        Public period_row_offset As Integer
        Public currency_code As String
        Public prop As String
        Public exchange_method As String
        Public universe As String

        Public Sub New()

        End Sub
    End Class
    Private Class formula_entity
        Public header As formulas_header
        Public name As String
        Public associated_name As String
        Public row_offset As Integer
        Public col_offset As Integer

        Public Sub New()

        End Sub
    End Class
    Private Class formula
        Public formula_entity As New formula_entity
        Public row_offset As Integer
        Public col_offset As Integer
        Public item As String
        Public year As String
        Public period As String
        Public formula As String
        Public item_type As String
        Public item_factor As String
        Public item_monatary As Boolean
        Public item_symbol As String
        Public first As Boolean = True
        Public assoc_entity As Boolean = False

        Public Sub New()

        End Sub
    End Class
    Private Sub deduce_child_entities()
        If off.clink.SelectedIndex > -1 Then
            REM for each selected entity deduce child entity



        End If
    End Sub
    Public Sub New()
        Try

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                bc_am_load_objects.obc_om_insight_submission_entity_links.db_read()
            Else
                bc_am_load_objects.obc_om_insight_submission_entity_links.tmode = bc_cs_soap_base_class.tREAD
                bc_am_load_objects.obc_om_insight_submission_entity_links.transmit_to_server_and_receive(bc_am_load_objects.obc_om_insight_submission_entity_links, True)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ef_functions", "New", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub refresh_pd_extract()
        Try
            refresh_extract()


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ef_functions", "refresh_extractt", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub
    Private Sub set_parameter_property(ByVal xml As String)
        Try

            Dim na As String
            Dim i As Integer
            i = 1
            While i <= application.ActiveWorkbook.ActiveSheet.CustomProperties.Count
                na = application.ActiveWorkbook.ActiveSheet.CustomProperties(i).name
                If Len(na) > 9 AndAlso na.Substring(0, 9) = "BCEXTRACT" Then
                    application.ActiveWorkbook.ActiveSheet.CustomProperties(i).Delete()
                    i = i - 1
                End If
            End While




            Dim pname As String
            Dim co As Integer
            co = xml.Length / 250
            For i = 0 To co
                pname = "BCEXTRACT_" + CStr(i)

                If xml.Length > 249 Then
                    application.ActiveWorkbook.ActiveSheet.CustomProperties.Add(pname, xml.Substring(0, 250))
                    xml = xml.Substring(250, xml.Length - 250)
                Else
                    application.ActiveWorkbook.ActiveSheet.CustomProperties.Add(pname, xml)
                    Exit Sub
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_portfolio_wizard", "set_parameter_property", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Function read_parameter_property_new() As String
        Dim sname, pname As String
        Dim i As Integer
        Try
           
            Dim exists As Boolean = True
            read_parameter_property_new = ""
            sname = application.ActiveWorkbook.ActiveSheet.name
            sname = sname.Replace(" ", "_")
            i = 0
            While exists = True And i < 1000
                pname = "BCEXTRACT_" + CStr(i)
                Try
                    For j = 1 To application.ActiveWorkbook.ActiveSheet.CustomProperties.count

                        If application.ActiveWorkbook.ActiveSheet.CustomProperties(j).name = pname Then
                            read_parameter_property_new = read_parameter_property_new + application.ActiveWorkbook.ActiveSheet.CustomProperties(j).value
                            Exit For
                        End If
                        If i = application.ActiveWorkbook.ActiveSheet.CustomProperties.count Then
                            exists = False
                        End If
                    Next
                    i = i + 1
                Catch ex As Exception
                    exists = False
                End Try
            End While

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ef_functions", "read_parameter_property_new", bc_cs_error_codes.USER_DEFINED, ex.Message)
            read_parameter_property_new = ""
        End Try
    End Function


    Private Function read_parameter_property() As String
        Dim sname, pname As String
        Dim i As Integer
        Try

            Dim exists As Boolean = True
            read_parameter_property = ""
            sname = application.ActiveWorkbook.ActiveSheet.name
            sname = sname.Replace(" ", "_")

            While exists = True
                pname = "rnet_extact" + sname + "_" + CStr(i)
                Try
                    read_parameter_property = read_parameter_property + application.ActiveWorkbook.CustomDocumentProperties(pname).value
                    i = i + 1
                Catch
                    exists = False
                End Try
            End While


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ef_functions", "read_parameter_property", bc_cs_error_codes.USER_DEFINED, ex.Message)
            read_parameter_property = ""
        End Try
    End Function
    Private Function refresh_extract() As Boolean
        refresh_extract = False

        Dim xmlload As New System.Xml.XmlDocument
        Dim pxml As String = ""
        REM new way
        Try
            pxml = read_parameter_property_new()
            xmlload.LoadXml(pxml)
        Catch



            REM legacy way
            Try

                xmlload.LoadXml(read_parameter_property)
            Catch ex As Exception
                REM 
                Dim omsg As New bc_cs_message("Blue Curve", "Cannot refresh extract." + vbCrLf + "Please check if this is a predefined data extract or redo from wizard.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End Try
        End Try

        Try
            Me.portfolio_Id = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/id").InnerXml()
        Catch

        End Try

        REM reconstruct the object
        Dim opt As New bc_om_pt_predefined_extract
        Dim format_filename As String = ""

        Dim myXmlNodeList As Xml.XmlNodeList
        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/sp_name")
        If myXmlNodeList.Count > 0 Then
            opt.sp_name = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/sp_name").InnerXml()
        Else
            Dim omsg As New bc_cs_message("Blue Curve", "Cannot refresh extract." + vbCrLf + "Please check if this is a predefined data extract or redo from wizard.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Function
        End If

        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/format_filename")
        If myXmlNodeList.Count > 0 Then
            format_filename = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/format_filename").InnerXml()
        End If

        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/entity_name")
        If myXmlNodeList.Count > 0 Then
            opt.entity_name = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/entity_name").InnerXml()
            opt.entity_name = opt.entity_name.Replace("&amp;", "&")

        End If

        Dim name As New List(Of String)
        Dim ids As New List(Of String)
        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/entity_list/anyType/name")

        For Each myxmlnode In myXmlNodeList
            Dim na As String
            na = myxmlnode.InnerXml
            na = na.Replace("&amp;", "&")
            'opt.entity_list.Add(ent)
            name.Add(na)
        Next
        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/entity_list/anyType/id")

        For Each myxmlnode In myXmlNodeList
            ids.Add(myxmlnode.InnerXml)
        Next

        For i = 0 To name.Count - 1
            Dim ent As New bc_om_entity
            ent.name = name(i)
            ent.id = ids(i)
            opt.entity_list.Add(ent)
        Next

        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/date_from")
        If myXmlNodeList.Count > 0 Then
            opt.date_from = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/date_from").InnerXml()
        End If

        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/date_to")
        If myXmlNodeList.Count > 0 Then
            opt.date_to = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/date_to").InnerXml()
        End If

        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/year_end_date")
        If myXmlNodeList.Count > 0 Then
            opt.year_end_date = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/year_end_date").InnerXml()
        End If
        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/start_year")
        If myXmlNodeList.Count > 0 Then
            opt.start_year = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/start_year").InnerXml()
        End If
        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/end_year")
        If myXmlNodeList.Count > 0 Then
            opt.end_year = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/end_year").InnerXml()
        End If

        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/stage")
        If myXmlNodeList.Count > 0 Then
            opt.stage = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/stage").InnerXml()
        End If
        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/schema")
        If myXmlNodeList.Count > 0 Then
            opt.schema = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/schema").InnerXml()
        End If
        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/contributor")
        If myXmlNodeList.Count > 0 Then
            opt.contributor = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/contributor").InnerXml()
        End If
        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/working_days")
        If myXmlNodeList.Count > 0 Then
            opt.working_days = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/working_days").InnerXml()
        End If
        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/param1")
        If myXmlNodeList.Count > 0 Then
            opt.param1 = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/param1").InnerXml()
        End If
        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/class_name")
        If myXmlNodeList.Count > 0 Then
            opt.class_name = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/class_name").InnerXml()
        End If
        myXmlNodeList = xmlload.SelectNodes("/bc_om_pt_predefined_extract/parent_class_name")
        If myXmlNodeList.Count > 0 Then
            opt.parent_class_name = xmlload.SelectSingleNode("/bc_om_pt_predefined_extract/parent_class_name").InnerXml()
        End If

        Dim xml As String
        xml = opt.write_data_to_xml(Nothing)



        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            opt.db_read()
        Else
            opt.tmode = bc_cs_soap_base_class.tREAD
            If opt.transmit_to_server_and_receive(opt, True) = False Then
                Exit Function
            End If
        End If
        If IsArray(opt.results) Then
            output_custom_extract_results_to_excel(application, opt.results, format_filename, xml)
        Else
            Dim omsg As New bc_cs_message("Blue Curve", "No results for predefiend extract", bc_cs_message.MESSAGE, False, False, "yes", "no", True)
        End If

        set_parameter_property(pxml)


    End Function
    Private Function output_custom_extract_results_to_excel(ByVal excelapp As Object, ByVal results As Object, ByVal format_filename As String, Optional ByVal xml As String = "") As Boolean
        Dim calc_state As Long

        Try

            Dim fs As New bc_cs_file_transfer_services
            Dim oexcel As New bc_ao_in_excel(excelapp)
            calc_state = excelapp.calculation
            excelapp.calculation = -4135
            If format_filename <> "" Then
                If Me.check_format_file_exists(format_filename) = True Then
                    REM load it
                    oexcel.disable_application_alerts()
                    REM FIL 5.2 August 2014
                    Dim one_sheet As Boolean
                    one_sheet = False
                    If excelapp.ActiveWorkbook.Worksheets.Count = 1 Then

                        excelapp.ActiveWorkbook.Sheets.Add()
                        excelapp.ActiveWorkbook.Worksheets(1).name = "tmpone"
                        excelapp.ActiveWorkbook.Worksheets(2).Activate()
                        one_sheet = True
                    End If

                    excelapp.activeworkbook.activesheet.delete()
                    oexcel.insert_sheet(format_filename, bc_cs_central_settings.local_template_path + format_filename)
                    If one_sheet = True Then

                        excelapp.ActiveWorkbook.Worksheets("tmpone").delete()
                    End If
                    REM END FIL
                Else
                    Dim omsg As New bc_cs_message("Blue Curve", "Format filename " + format_filename + "doesnt exist", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
            End If
            oexcel.screen_updating(True)
            excelapp.screenupdating = False


            Dim str As String
            Dim url As String

            For i = 0 To UBound(results, 2)

                str = CStr(results(0, i))
                If str.Length > 12 Then
                    If str.Substring(0, 10) = "[hyperlink" Then

                        url = str.Substring(10, InStr(str, "]") - 11)
                        excelapp.activeworkbook.activesheet.cells(results(1, i), results(2, i)).select()

                        Try
                            excelapp.activeworkbook.ActiveSheet.Hyperlinks.Add(Anchor:=excelapp.activeworkbook.activesheet.cells(results(1, i), results(2, i)), Address:=url, TextToDisplay:="aa")
                            results(0, i) = str.Substring(InStr(str, "]"), str.Length - InStr(str, "]"))

                        Catch ex As Exception
                            Dim ocomm As New bc_cs_activity_log("bc_am_portfolio_wizard", "output_custom_extract_results_to_excel", bc_cs_activity_codes.COMMENTARY, "cant set hyperlink: " + ex.Message)
                        End Try
                    End If
                End If

                If IsNumeric(results(0, i)) = True Then
                    excelapp.activeworkbook.activesheet.cells(results(1, i), results(2, i)).value = CDbl(results(0, i))
                ElseIf IsDate(results(0, i)) = True Then
                    excelapp.activeworkbook.activesheet.cells(results(1, i), results(2, i)).value = CDate(results(0, i))
                Else
                    excelapp.activeworkbook.activesheet.cells(results(1, i), results(2, i)).value = results(0, i)
                End If
                Try
                    excelapp.activeworkbook.activesheet.cells(results(1, i), results(2, i)).style = results(3, i)
                Catch

                End Try
            Next



            output_custom_extract_results_to_excel = True
            oexcel.enable_application_alerts()


        Catch ex As Exception
            output_custom_extract_results_to_excel = False
            Dim oerr As New bc_cs_error_log("bc_am_portfolio_wizard", " output_custom_extract_results_to_excel", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            excelapp.calculation = calc_state
            excelapp.screenupdating = True

        End Try
    End Function
    Public Sub load(Optional ByVal extract_wizard As Boolean = False, Optional ByVal function_name As String = "")
        Dim i As Integer
        Dim ofunctions As New bc_om_ef_functions
        Try
            REM dont do this if already loaded
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                If extract_wizard = False Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Cannot use BC functions as system is either off line or you are not an authenticated user", bc_cs_message.MESSAGE)
                Else
                    Dim omsg As New bc_cs_message("Blue Curve", "Cannot use Data Extraction Tool as system is either off line or you are not an authenticated user", bc_cs_message.MESSAGE)
                End If
                Exit Sub
            End If
            If excel_functions.Count = 0 Then
                excel_functions.Clear()
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ofunctions.db_read()
                Else
                    ofunctions.tmode = bc_cs_soap_base_class.tREAD
                    If ofunctions.transmit_to_server_and_receive(ofunctions, True) = False Then

                        Exit Sub
                    End If
                End If
                If ofunctions.macros.Count = 0 Then
                    Exit Sub
                End If
                excel_functions = ofunctions.macros
                stage_ids = ofunctions.stage_ids
                stage_names = ofunctions.stage_names
                class_ids = ofunctions.class_ids
                class_names = ofunctions.class_names
                entities.Clear()
                entities = ofunctions.entities
                contributor_ids = ofunctions.contributor_ids
                contributor_names = ofunctions.contributor_names
                period_ids = ofunctions.period_ids
                period_names = ofunctions.period_names
                item_types = ofunctions.item_types
                item_factors = ofunctions.item_factors
                item_monatarys = ofunctions.item_monatarys
                item_symbols = ofunctions.item_symbols
                item_names = ofunctions.item_names
                template_ids = ofunctions.template_ids
                template_names = ofunctions.template_names
                currency_codes = ofunctions.currency_codes
                schema_ids = ofunctions.schema_ids
                schema_names = ofunctions.schema_names
                class_links = ofunctions.class_links
                chart_tools = ofunctions.chart_tools
                aggs = ofunctions.aggs
                Try
                    For i = 0 To ofunctions.macros.Count - 1
                        application.MacroOptions(Macro:=ofunctions.macros(i).name, Description:=ofunctions.macros(i).helptext, Category:=ofunctions.macros(i).category)
                    Next
                Catch
                End Try

            End If
            If extract_wizard = False Then
                REM PR July 2010
                ousersettings.logged_on_user_id = bc_cs_central_settings.logged_on_user_id
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ousersettings.db_read()
                Else
                    ousersettings.tmode = bc_cs_soap_base_class.tREAD
                    If ousersettings.transmit_to_server_and_receive(ousersettings, True) = False Then
                        Exit Sub
                    End If
                End If
                REM =====================
                Dim off As New bc_am_ef_wizard
                off.activecell = application.activecell
                Dim oapi As New API
                'API.SetWindowPos(off.Handle.ToInt32, API.HWND_TOPMOST, 10, 10, 1, 1, 1)
                off.Function_name = function_name
                'off.Show()
                off.Show()
                off.TopMost = True
            Else
                REM load user settingsmsgbox
                ousersettings.logged_on_user_id = bc_cs_central_settings.logged_on_user_id
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    ousersettings.db_read()
                Else
                    ousersettings.tmode = bc_cs_soap_base_class.tREAD
                    If ousersettings.transmit_to_server_and_receive(ousersettings, True) = False Then
                        Exit Sub
                    End If
                End If
                off = New bc_am_portfolio_wizard
                off.excelapp = application
                Dim oapi As New API
                'API.SetWindowPos(off.Handle.ToInt32, API.HWND_TOPMOST, 10, 10, 1, 1, 1)
                off.TopMost = True
                off.ShowDialog()
                Me.portfolio_Id = off.selected_portfolio_id
                If off.success = True Then
                    Me.portfolio_Id = 0
                    chart_method = off.chart_method
                    chart_passrangeasparam = off.chart_passrangeasparam
                    chart_active = off.chart_active
                    chart_data = off.chart_data
                    If off.rbase.Checked = True Then
                        build_portfolio()
                    Else
                        build_portfolio_aggregation()
                    End If
                    If off.Rnosave.Checked = False Then
                        If save_portfolio(off.rglobalsave.Checked) = False Then
                            Exit Sub
                        End If
                    End If
                End If
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ef_help", "set_help", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub
    Private Function save_portfolio(ByVal globalPF As Boolean) As Boolean
        Try
            Dim i As Integer
            save_portfolio = True


            Dim oportfolio As New bc_om_ef_portolio
            If globalPF = True Then
                oportfolio.public_flag = True
            Else
                oportfolio.public_flag = False
            End If
            If off.rnew.Checked = True Then
                oportfolio.portfolio_id = 0
            Else
                oportfolio.portfolio_id = off.selected_portfolio_id

            End If
            oportfolio.user_id = bc_cs_central_settings.logged_on_user_id
            oportfolio.function_name = off.cfunction.Text
            oportfolio.title = off.tname.Text
            oportfolio.schema_name = off.cschema.Text
            oportfolio.class_name = off.cclass.Text
            oportfolio.table_title = off.ttitle.Text
            oportfolio.sub_title = off.tsubtitle.Text
            oportfolio.source = off.tsource.Text
            oportfolio.associated_class = ""
            oportfolio.dual_entity = ""

            If off.rbase.Checked = True Then
                oportfolio.data_type = 0
                oportfolio.dual_entity = ""
            Else
                oportfolio.data_type = 1
                oportfolio.exch_type = off.cexchmethod.Text
                If off.clink.SelectedIndex > 0 Then
                    oportfolio.dual_entity = off.clink.Text
                End If
            End If


            If off.cand.Checked = True And off.cclass3.SelectedIndex > -1 And off.centity2.SelectedIndex > -1 Then
                oportfolio.sec_entity_prop_class_name = off.cclass3.Text
                oportfolio.sec_entity_prop_entity_name = off.centity2.Text
            End If
            If off.clink.SelectedIndex > 0 Then
                oportfolio.associated_class = off.clink.Text
            End If
            If off.c_show_ea.Checked = True Then
                oportfolio.show_e_a = True
            Else
                oportfolio.show_e_a = False
            End If
            oportfolio.format_type = 0
            oportfolio.precision = 0
            If off.cformat.Checked = True Then
                oportfolio.format_type = off.cprectype.SelectedIndex + 1
                If off.cprec.SelectedIndex > -1 Then
                    oportfolio.precision = CInt(off.cprec.Text)
                End If
            End If
            If off.rchoose.Checked = True Then
                oportfolio.universe_flag = 0
            ElseIf off.rentity_set.Checked = True Then
                oportfolio.universe_flag = 1
                oportfolio.entity_set_name = off.centityset.Text
            ElseIf off.rclass.Checked = True Then
                oportfolio.universe_flag = 2
                oportfolio.entity_prop_class_name = off.cclass2.Text
                oportfolio.entity_prop_entity_name = off.centity.Text
            ElseIf off.rall.Checked = True Then
                oportfolio.universe_flag = 3
            End If
            If oportfolio.dual_entity = "" Then
                REM now assign entities
                For i = 0 To off.lselentity.Items.Count - 1
                    oportfolio.entities.Add(off.lselentity.Items(i))
                Next
            Else
                For i = 0 To off.lvwdual.Items.Count - 1
                    oportfolio.entities.Add(off.lvwdual.Items(i).Text)
                    oportfolio.dual_entities.Add(off.lvwdual.Items(i).SubItems(1).Text)
                Next
            End If

            REM items
            For i = 0 To off.lselitem.Items.Count - 1
                oportfolio.items.Add(off.lselitem.Items(i))
                oportfolio.item_types.Add(off.item_sel_type(i))
                oportfolio.item_factor.Add(off.item_sel_factor(i))
                oportfolio.item_monatary.Add(off.item_sel_monatary(i))
                oportfolio.item_symbol.Add(off.item_sel_symbol(i))
                oportfolio.item_assoc.Add(off.item_sel_assoc_class(i))
            Next
            REM criteria
            oportfolio.stage = off.cstage.Text
            oportfolio.contributor = off.Ccont.Text
            If off.rnow.Checked = True Then
                oportfolio.date_at = "9-9-9999"
            Else
                oportfolio.date_at = off.DateTimePicker1.Value
            End If
            If off.rold.Checked = True Then
                oportfolio.date_from = "1-1-1900"
            Else
                oportfolio.date_from = off.DateTimePicker2.Value
            End If
            oportfolio.start_year = off.Cyearstart.Text
            oportfolio.end_year = off.cyearend.Text
            For i = 0 To off.lperiods.Items.Count - 1
                oportfolio.periods.Add(off.lperiods.Items(i))
            Next
            If off.rcurrnone.Checked = True Then
                oportfolio.convert_type = 1

            ElseIf off.rcurrshow.Checked = True Then
                oportfolio.convert_type = 0

            ElseIf off.rcurrconv.Checked = True Then
                oportfolio.convert_type = 2
                oportfolio.currency = off.ccurr.Text
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oportfolio.db_write()
            Else
                oportfolio.tmode = bc_cs_soap_base_class.tWRITE
                oportfolio.transmit_to_server_and_receive(oportfolio, True)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_portfolio_main", "save_portfolio", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally

        End Try

    End Function
    Public Function check_formats_exists() As Boolean
        Dim i As Integer
        check_formats_exists = False
        For i = 1 To application.ActiveWorkbook.Styles.Count()
            If application.ActiveWorkbook.Styles(i).Name = "bc_pt_title" Then
                check_formats_exists = True
                Exit Function
            End If
        Next
    End Function
    Public Function check_format_file_exists(ByVal fn As String) As Boolean

        Dim fs As New bc_cs_file_transfer_services
        check_format_file_exists = False
        If fs.check_document_exists(bc_cs_central_settings.local_template_path + fn) Then
            check_format_file_exists = True
        End If
    End Function
    Public Function excel_address(ByVal row As Integer, ByVal col As Integer, Optional ByVal abs_row As Boolean = False, Optional ByVal abs_col As Boolean = False) As String
        Dim j As Double
        Dim k As Double
        Dim l As Integer

        excel_address = ""

        j = col
        k = j Mod 26
        If k = 0 Then
            k = 26
        End If
        REM FIL JULY 2012
        l = j / 26 - 0.51
        If l > 0 Then
            excel_address = Chr(l + 64)
        End If
        If abs_row = False And abs_col = False Then
            excel_address = excel_address + Chr(k + 64) + CStr(row)
        ElseIf abs_row = True And abs_col = True Then
            excel_address = "$" + excel_address + Chr(k + 64) + "$" + CStr(row)
        ElseIf abs_row = True And abs_col = False Then
            excel_address = excel_address + Chr(k + 64) + "$" + CStr(row)
        Else
            excel_address = "$" + excel_address + Chr(k + 64) + CStr(row)
        End If
    End Function
    Private Function build_portfolio() As Boolean
        REM first get all paramters constant across data set

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim otrace As New bc_cs_activity_log("bc_am_portfolio_wizard", "build_portfolio", bc_cs_activity_codes.TRACE_ENTRY, "")

            If CInt(off.cyearend.Text) < CInt(off.Cyearstart.Text) Then
                off.cyearend.Text = off.Cyearstart.Text
            End If

            'application.Cursor = Cursors.WaitCursor
            build_portfolio = True

            Dim i, j, k, l As Integer
            start_row = 1
            start_col = 1
            Dim fh As New formulas_header
            Dim entity_formulas As New ArrayList
            Dim formulas As New ArrayList
            Dim item_col As Integer
            Dim multiple_entity As Boolean = True
            REM check workbook is opne
            Try
                i = application.activeworkbook.worksheets.count
            Catch
                Dim omsg As New bc_cs_message("Blue Curve", "Please open a workbook  in Excel and run portfolio again", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End Try
            REM if only one entity selected and more than one item display data vertically
            If off.lselentity.Items.Count = 1 Or off.cfunction.SelectedIndex = 1 Then
                multiple_entity = False
            End If


            If off.rcell.Checked = True Then
                start_row = application.activecell.row
                start_col = application.activecell.column
            ElseIf off.rsheet.Checked = True Then
                application.activeworkbook.worksheets.add()
                start_row = 1
                start_col = 1
            End If
            application.visible = True
            application.statusbar = "Generating Extract..."
            Dim period As String = False
            fh.stage = off.cstage.Text


            fh.contributor = off.Ccont.Text

            fh.row_offset = start_row + 1
            fh.item_row_offset = start_row + 4
            fh.title = off.tname.Text

            If off.rnow.Checked = True Then
                fh.date_at = "9-9-9999"
            Else
                fh.date_at = off.DateTimePicker1.Value
            End If
            fh.entity_class = off.cclass.Text
            If off.rclass.Checked = True Then
                If InStr(off.rclass.Text, "used") = 0 Then
                    fh.prop = "in " + off.cclass2.Text + " " + off.centity.Text
                Else
                    fh.prop = "used by " + off.cclass2.Text + " " + off.centity.Text
                End If
            End If

            If off.ccurr.SelectedIndex > -1 And off.rcurrconv.Checked = True Then
                fh.currency_code = off.ccurr.Text
            End If
            REM now get entity list
            REM if portfolio as child entities as well deduce what these are
            deduce_child_entities()
            Dim oent_formula As formula_entity
            If off.clink.SelectedIndex > 0 Then
                off.get_propogating_entities()
                If off.propentitymaster.Count > 1 Then
                    multiple_entity = True
                End If

                off.lselentity.Items.Clear()
                For i = 0 To off.propentitymaster.Count - 1
                    off.lselentity.Items.Add(off.propentitymaster(i))
                Next

            End If
            For i = 0 To off.lselentity.Items.Count - 1
                Dim ocomm As New bc_cs_activity_log("bc_am_portfolio_wizard", "build_portfolio", bc_cs_activity_codes.COMMENTARY, "Creating formula for entity: " + CStr(i + 1) + " of " + CStr(off.lselentity.Items.Count))
                oent_formula = New formula_entity
                oent_formula.row_offset = start_row + 11 + i
                oent_formula.col_offset = start_col
                oent_formula.header = fh
                oent_formula.name = off.lselentity.Items(i)

                If off.clink.SelectedIndex > 0 Then
                    oent_formula.associated_name = off.propentityslave(i)
                End If
                entity_formulas.Add(oent_formula)
                REM now build cells that hold formula
                REM item
                If multiple_entity = True Then
                    item_col = oent_formula.col_offset + 2
                Else
                    item_col = oent_formula.col_offset + 3
                End If

                For j = 0 To off.lselitem.Items.Count - 1
                    REM if item is period related
                    If off.item_sel_type(j) = "period" Then
                        If multiple_entity = False Then
                            item_col = start_col + 7
                        End If
                        Dim oform As formula = Nothing

                        If item_col > 255 Then
                            off.WindowState = FormWindowState.Minimized
                            Dim omsg As New bc_cs_message("Blue Curve", "Excel column limit exeeded (255), please go back and choose less items years or periods", bc_cs_message.MESSAGE)
                            off.WindowState = FormWindowState.Normal
                            off.twizard.SelectedIndex = 3
                            off.tprogress.Abort()
                            off.ppropress.Visible = False
                            build_portfolio = False
                            Exit Function
                        End If

                        For k = CInt(off.Cyearstart.Text) To CInt(off.cyearend.Text)
                            For l = 0 To off.lperiods.Items.Count - 1
                                oform = New formula
                                oform.formula_entity = oent_formula
                                oform.item_type = "item"
                                If k > CInt(off.Cyearstart.Text) Then
                                    oform.first = False
                                End If
                                If l > 0 Then
                                    oform.first = False
                                End If
                                oform.item = off.lselitem.Items(j)
                                oform.item_factor = off.item_sel_factor(j)
                                oform.item_monatary = off.item_sel_monatary(j)
                                oform.item_symbol = off.item_sel_symbol(j)
                                oform.year = k
                                oform.assoc_entity = False
                                If off.item_sel_assoc_class(j) = off.clink.Text Then
                                    oform.assoc_entity = True
                                End If


                                oform.period = off.lperiods.Items(l)
                                oform.row_offset = oent_formula.row_offset
                                oform.col_offset = item_col
                                If multiple_entity = True Then
                                    If off.rcurrconv.Checked = True Then
                                        currency_convert(oform)
                                    Else
                                        build_function_multiple(oform)
                                    End If
                                Else
                                    oform.row_offset = start_row + 9 + j
                                    oform.col_offset = item_col
                                    If off.rcurrconv.Checked = True Then
                                        currency_convert(oform, True, False)
                                    Else
                                        build_function_single(oform)
                                    End If
                                End If
                                oform.formula = oform.formula.Replace(",)", ")")
                                If off.cformat.Checked = True Then

                                    REM wrap function aound format
                                    oform.formula = oform.formula.Substring(1, oform.formula.Length - 1)
                                    If multiple_entity = True Then
                                        oform.formula = "=bc_format(" + oform.formula + "," + excel_address(start_row + 2, oform.col_offset, True, False) + "," + excel_address(start_row + 1, oform.col_offset, True, False) + "," + excel_address(start_row + 3, oform.col_offset, True, False) + "," + excel_address(start_row + 9, oform.col_offset, True, False) + ")"
                                    Else
                                        oform.formula = "=bc_format(" + oform.formula + "," + excel_address(oform.row_offset, start_col + 1, False, True) + "," + excel_address(oform.row_offset, start_col, False, True) + "," + excel_address(oform.row_offset, start_col + 2, False, True) + "," + excel_address(oform.row_offset, start_col + 3, False, True) + ")"
                                    End If
                                End If
                                formulas.Add(oform)
                                If off.cformat.Checked = True Then
                                    REM add scale symbol formula
                                    If multiple_entity = True Then
                                        Dim oform1 As New formula
                                        oform1.formula_entity = oent_formula
                                        oform1.item = ""
                                        oform1.item_type = "symbol"
                                        oform1.row_offset = start_row + 10
                                        oform1.col_offset = item_col
                                        oform1.formula = "=bc_get_scale_symbol(" + excel_address(start_row + 9, item_col) + "," + excel_address(start_row + 3, item_col) + ")"
                                        formulas.Add(oform1)
                                    End If
                                End If
                                If off.c_show_ea.Checked = True And multiple_entity = False Then
                                    Dim oform1 As New formula
                                    oform1.formula_entity = oent_formula
                                    oform1.item_type = "e_a"
                                    oform1.item = "E/A"
                                    oform1.assoc_entity = False
                                    If off.item_sel_assoc_class(j) = off.clink.Text Then
                                        oform1.assoc_entity = True
                                    End If

                                    oform1.year = ""
                                    oform1.period = ""
                                    oform1.row_offset = start_row + 8
                                    oform1.formula_entity.row_offset = start_row + 9 + j
                                    oform1.col_offset = item_col
                                    build_function_single(oform1, True, False, False, True)
                                    oform1.formula = oform1.formula.Replace(",)", ",""e_a_flag"")")
                                    oform1.formula = oform1.formula.Replace("=bc", "=IF(bc")
                                    oform1.formula = oform1.formula.Replace(")", ")=0,""A"",""E"")")
                                    formulas.Add(oform1)

                                End If
                                item_col = item_col + 1

                                If off.c_show_ea.Checked = True And multiple_entity = True Then

                                    REM add e_a-after item
                                    Dim oform1 As New formula
                                    oform1.formula_entity = oent_formula
                                    oform1.item_type = "e_a"
                                    oform1.item = "E/A"
                                    oform1.assoc_entity = False
                                    If off.item_sel_assoc_class(j) = off.clink.Text Then
                                        oform1.assoc_entity = True
                                    End If

                                    oform1.year = ""
                                    oform1.period = ""
                                    oform1.row_offset = oent_formula.row_offset
                                    oform1.col_offset = item_col
                                    item_col = item_col + 1
                                    build_function_multiple(oform1, True, False, False, True)
                                    oform1.formula = oform1.formula.Replace(",)", ",""e_a_flag"")")
                                    oform1.formula = oform1.formula.Replace("=bc", "=IF(bc")
                                    REM ING JUNE 2012
                                    oform1.formula = oform1.formula.Replace(")", ")=0,""A"",""E"")")
                                    REM --------------------
                                    formulas.Add(oform1)
                                End If



                            Next
                        Next
                        If off.rcurrnone.Checked = True And oform.item_monatary = True Then
                            REM add currency after item
                            oform = New formula
                            oform.formula_entity = oent_formula
                            oform.item = "Currency"
                            oform.item_type = "curr"
                            oform.assoc_entity = False
                            If off.item_sel_assoc_class(j) = off.clink.Text Then
                                oform.assoc_entity = True
                            End If

                            If multiple_entity = True Then
                                oform.row_offset = oent_formula.row_offset
                                oform.col_offset = item_col
                                build_function_multiple(oform, True, True, True, off.c_show_ea.Checked)
                            Else
                                oform.formula_entity.header.col_offset = 3 + start_col
                                oform.row_offset = start_row + 9 + j
                                oform.col_offset = start_col + 5
                                build_function_single(oform, True, True, True, off.c_show_ea.Checked)
                            End If

                            oform.formula = oform.formula.Replace(")", """item_currency"")")
                            item_col = item_col + 1
                            formulas.Add(oform)
                        End If
                        REM add scale symbol formula
                        If multiple_entity = False And off.cformat.Checked = True Then
                            Dim oform1 As New formula
                            oform1.formula_entity = oent_formula
                            oform1.item = ""
                            oform1.item_type = "symbol"
                            oform1.row_offset = start_row + 9 + j
                            oform1.col_offset = start_col + 4
                            oform1.formula = "=bc_get_scale_symbol(" + excel_address(start_row + 9 + j, start_col + 3) + "," + excel_address(start_row + 9 + j, start_col + 2)
                            formulas.Add(oform1)
                        End If
                    Else
                        Dim oform As New formula
                        oform.formula_entity = oent_formula
                        oform.item = off.lselitem.Items(j)
                        oform.item_type = "item"
                        oform.item_factor = off.item_sel_factor(j)
                        oform.item_monatary = off.item_sel_monatary(j)
                        oform.item_symbol = off.item_sel_symbol(j)
                        oform.row_offset = oent_formula.row_offset
                        oform.col_offset = item_col
                        oform.assoc_entity = False
                        If off.item_sel_assoc_class(j) = off.clink.Text Then
                            oform.assoc_entity = True
                        End If


                        If multiple_entity = True Then
                            If off.rcurrconv.Checked = True Then
                                currency_convert(oform)
                            Else
                                build_function_multiple(oform)
                            End If
                        Else
                            oform.row_offset = start_row + 9 + j
                            oform.col_offset = start_col + 6
                            If off.rcurrconv.Checked = True Then
                                currency_convert(oform, True, False)
                            Else
                                build_function_single(oform)
                            End If
                        End If
                        oform.formula = oform.formula.Replace(",)", ")")
                        If off.cformat.Checked = True Then
                            REM wrap function aound format
                            oform.formula = oform.formula.Substring(1, oform.formula.Length - 1)
                            If multiple_entity = True Then
                                oform.formula = "=bc_format(" + oform.formula + "," + excel_address(start_row + 2, oform.col_offset, True, False) + "," + excel_address(start_row + 1, oform.col_offset, True, False) + "," + excel_address(start_row + 3, oform.col_offset, True, False) + "," + excel_address(start_row + 9, oform.col_offset, True, False) + ")"
                            Else
                                oform.formula = "=bc_format(" + oform.formula + "," + excel_address(oform.row_offset, start_col + 1, False, True) + "," + excel_address(oform.row_offset, start_col, False, True) + "," + excel_address(oform.row_offset, start_col + 2, False, True) + "," + excel_address(oform.row_offset, start_col + 9, False, True) + ")"
                            End If
                        End If
                        formulas.Add(oform)
                        If off.cformat.Checked = True And multiple_entity = True Then
                            REM add scale symbol formula
                            Dim oform1 As New formula
                            oform1.formula_entity = oent_formula
                            oform1.item = ""
                            oform1.item_type = "symbol"
                            oform1.row_offset = start_row + 10
                            oform1.col_offset = item_col
                            oform1.formula = "=bc_get_scale_symbol(" + excel_address(start_row + 9, item_col) + "," + excel_address(start_row + 3, item_col) + ")"
                            formulas.Add(oform1)
                        End If
                        item_col = item_col + 1

                        If off.rcurrnone.Checked = True And oform.item_monatary = True Then
                            REM add currency after item
                            oform = New formula
                            oform.formula_entity = oent_formula
                            oform.item = "Currency"
                            oform.item_type = "curr"
                            oform.assoc_entity = False
                            If off.item_sel_assoc_class(j) = off.clink.Text Then
                                oform.assoc_entity = True
                            End If

                            If multiple_entity = True Then
                                oform.row_offset = oent_formula.row_offset
                                oform.col_offset = item_col
                                build_function_multiple(oform, True, True, True)
                            Else
                                oform.formula_entity.header.col_offset = 2 + start_col
                                oform.row_offset = start_row + 9 + j
                                oform.col_offset = start_col + 5
                                build_function_single(oform, True, True, True, off.c_show_ea.Checked)
                            End If
                            oform.formula = oform.formula.Replace(")", """item_currency"")")
                            item_col = item_col + 1
                            formulas.Add(oform)
                        End If
                        If off.cformat.Checked = True Then
                            REM add scale symbol formula
                            If multiple_entity = False Then
                                Dim oform1 As New formula
                                oform1.formula_entity = oent_formula
                                oform1.item = ""
                                oform1.item_type = "symbol"
                                oform1.row_offset = start_row + 9 + j
                                oform1.col_offset = start_col + 4
                                oform1.formula = "=bc_get_scale_symbol(" + excel_address(start_row + 9 + j, start_col + 3) + "," + excel_address(start_row + 9 + j, start_col + 2) + ")"
                                formulas.Add(oform1)
                            End If
                        End If
                    End If
                Next
            Next

            end_row = 1
            end_col = 1
            For i = 0 To formulas.Count - 1
                If formulas(i).row_offset > end_row Then
                    end_row = formulas(i).row_offset
                End If
                If formulas(i).col_offset > end_col Then
                    end_col = formulas(i).col_offset
                End If
            Next
            build_portfolio = output_to_excel(start_row, start_col, fh, entity_formulas, formulas, multiple_entity)



        Catch ex As Exception
            build_portfolio = False
            Dim oerr As New bc_cs_error_log("bc_am_ef_functions", "build_portfolio", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            Dim otrace As New bc_cs_activity_log("bc_am_ef_functions", "build_portfolio", bc_cs_activity_codes.TRACE_EXIT, "")

            off.Cursor = Cursors.Default
        End Try

    End Function

    Private Function build_portfolio_aggregation() As Boolean
        REM first get all paramters constant across data set

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim otrace As New bc_cs_activity_log("bc_am_portfolio_wizard", "build_portfolio_aggregation", bc_cs_activity_codes.TRACE_ENTRY, "")

            If CInt(off.cyearend.Text) < CInt(off.Cyearstart.Text) Then
                off.cyearend.Text = off.Cyearstart.Text
            End If

            'application.Cursor = Cursors.WaitCursor
            build_portfolio_aggregation = True

            Dim i, j, k, l As Integer
            start_row = 1
            start_col = 1
            Dim fh As New formulas_header
            Dim entity_formulas As New ArrayList
            Dim formulas As New ArrayList
            Dim item_col As Integer
            Dim multiple_entity As Boolean = True
            REM check workbook is opne
            Try
                i = application.activeworkbook.worksheets.count
            Catch
                Dim omsg As New bc_cs_message("Blue Curve", "Please open a workbook  in Excel and run portfolio again", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End Try
            REM if only one entity selected and more than one item display data vertically
            If off.lselentity.Items.Count = 1 Or off.cfunction.SelectedIndex = 1 Or off.lvwdual.Items.Count = 1 Then
                multiple_entity = False
            End If


            If off.rcell.Checked = True Then
                start_row = application.activecell.row
                start_col = application.activecell.column
            ElseIf off.rsheet.Checked = True Then
                application.activeworkbook.worksheets.add()
                start_row = 1
                start_col = 1
            End If
            application.visible = True
            application.statusbar = "Generating Extract..."
            Dim period As String = False
            fh.stage = off.cstage.Text

            fh.contributor = off.Ccont.Text

            fh.row_offset = start_row + 1
            fh.item_row_offset = start_row + 4
            fh.title = off.tname.Text
            fh.exchange_method = off.cexchmethod.Text
            fh.universe = off.cschema.Text

            If off.rnow.Checked = True Then
                fh.date_at = "9-9-9999"
            Else
                fh.date_at = off.DateTimePicker1.Value
            End If
            fh.entity_class = off.cclass.Text
            If off.rclass.Checked = True Then
                If InStr(off.rclass.Text, "used") = 0 Then
                    fh.prop = "in " + off.cclass2.Text + " " + off.centity.Text
                Else
                    fh.prop = "used by " + off.cclass2.Text + " " + off.centity.Text
                End If
            End If

            If off.ccurr.SelectedIndex > -1 And off.rcurrconv.Checked = True Then
                fh.currency_code = off.ccurr.Text
            End If
            REM now get entity list
            REM if portfolio as child entities as well deduce what these are
            deduce_child_entities()
            Dim oent_formula As formula_entity
            If off.clink.SelectedIndex > 0 Then
                If off.lvwdual.Items.Count > 1 Then
                    multiple_entity = True
                End If

            End If
            Dim ents As New List(Of String)
            Dim dents As New List(Of String)
            If off.clink.SelectedIndex > 0 Then
                For i = 0 To off.lvwdual.Items.Count - 1
                    ents.Add(off.lvwdual.Items(i).Text)
                    dents.Add(off.lvwdual.Items(i).SubItems(1).Text)
                Next
            Else
                For i = 0 To off.lselentity.Items.Count - 1
                    ents.Add(off.lselentity.Items(i))
                Next
            End If

            For i = 0 To ents.Count - 1
                Dim ocomm As New bc_cs_activity_log("bc_am_portfolio_wizard", "build_portfolio", bc_cs_activity_codes.COMMENTARY, "Creating formula for entity: " + CStr(i + 1) + " of " + CStr(off.lselentity.Items.Count))
                oent_formula = New formula_entity
                oent_formula.row_offset = start_row + 11 + i
                oent_formula.col_offset = start_col
                oent_formula.header = fh
                'oent_formula.name = off.lselentity.Items(i)
                oent_formula.name = ents(i)

                If off.clink.SelectedIndex > 0 Then
                    oent_formula.associated_name = dents(i)


                End If
                entity_formulas.Add(oent_formula)
                REM now build cells that hold formula
                REM item
                If multiple_entity = True Then
                    item_col = oent_formula.col_offset + 2
                Else
                    item_col = oent_formula.col_offset + 3
                End If

                For j = 0 To off.lselitem.Items.Count - 1
                    REM if item is period related
                    If off.item_sel_type(j) = "period" Then
                        If multiple_entity = False Then
                            item_col = start_col + 7
                        End If
                        Dim oform As formula = Nothing

                        If item_col > 255 Then
                            off.WindowState = FormWindowState.Minimized
                            Dim omsg As New bc_cs_message("Blue Curve", "Excel column limit exeeded (255), please go back and choose less items years or periods", bc_cs_message.MESSAGE)
                            off.WindowState = FormWindowState.Normal
                            off.twizard.SelectedIndex = 3
                            off.tprogress.Abort()
                            off.ppropress.Visible = False
                            build_portfolio_aggregation = False
                            Exit Function
                        End If

                        For k = CInt(off.Cyearstart.Text) To CInt(off.cyearend.Text)
                            For l = 0 To off.lperiods.Items.Count - 1
                                oform = New formula
                                oform.formula_entity = oent_formula
                                oform.item_type = "item"
                                If k > CInt(off.Cyearstart.Text) Then
                                    oform.first = False
                                End If
                                If l > 0 Then
                                    oform.first = False
                                End If
                                oform.item = off.lselitem.Items(j)
                                oform.item_factor = off.item_sel_factor(j)
                                oform.item_monatary = off.item_sel_monatary(j)
                                oform.item_symbol = off.item_sel_symbol(j)
                                oform.year = k
                                oform.assoc_entity = False
                                If off.clink.SelectedIndex > 0 Then

                                    oform.assoc_entity = True
                                End If


                                oform.period = off.lperiods.Items(l)
                                oform.row_offset = oent_formula.row_offset
                                oform.col_offset = item_col
                                If multiple_entity = True Then
                                    If off.rcurrconv.Checked = True Then
                                        currency_convert(oform)
                                    Else
                                        build_function_multiple_aggregation(oform)
                                    End If
                                Else
                                    oform.row_offset = start_row + 9 + j
                                    oform.col_offset = item_col
                                    If off.rcurrconv.Checked = True Then
                                        currency_convert(oform, True, False)
                                    Else
                                        build_function_single_aggregation(oform)
                                    End If
                                End If
                                oform.formula = oform.formula.Replace(",)", ")")
                                If off.cformat.Checked = True Then

                                    REM wrap function aound format
                                    oform.formula = oform.formula.Substring(1, oform.formula.Length - 1)
                                    If multiple_entity = True Then
                                        oform.formula = "=bc_format(" + oform.formula + "," + excel_address(start_row + 2, oform.col_offset, True, False) + "," + excel_address(start_row + 1, oform.col_offset, True, False) + "," + excel_address(start_row + 3, oform.col_offset, True, False) + "," + excel_address(start_row + 9, oform.col_offset, True, False) + ")"
                                    Else
                                        oform.formula = "=bc_format(" + oform.formula + "," + excel_address(oform.row_offset, start_col + 1, False, True) + "," + excel_address(oform.row_offset, start_col, False, True) + "," + excel_address(oform.row_offset, start_col + 2, False, True) + "," + excel_address(oform.row_offset, start_col + 3, False, True) + ")"
                                    End If
                                End If
                                formulas.Add(oform)
                                If off.cformat.Checked = True Then
                                    REM add scale symbol formula
                                    If multiple_entity = True Then
                                        Dim oform1 As New formula
                                        oform1.formula_entity = oent_formula
                                        oform1.item = ""
                                        oform1.item_type = "symbol"
                                        oform1.row_offset = start_row + 10
                                        oform1.col_offset = item_col
                                        oform1.formula = "=bc_get_scale_symbol(" + excel_address(start_row + 9, item_col) + "," + excel_address(start_row + 3, item_col) + ")"
                                        formulas.Add(oform1)
                                    End If
                                End If
                                If off.c_show_ea.Checked = True And multiple_entity = False Then
                                    Dim oform1 As New formula
                                    oform1.formula_entity = oent_formula
                                    oform1.item_type = "e_a"
                                    oform1.item = "E/A"
                                    oform1.assoc_entity = False
                                    If off.item_sel_assoc_class(j) = off.clink.Text Then
                                        oform1.assoc_entity = True
                                    End If

                                    oform1.year = ""
                                    oform1.period = ""
                                    oform1.row_offset = start_row + 8
                                    oform1.formula_entity.row_offset = start_row + 9 + j
                                    oform1.col_offset = item_col
                                    build_function_single_aggregation(oform1, True, False, False, True)
                                    oform1.formula = oform1.formula.Replace(",)", ",""e_a_flag"")")
                                    oform1.formula = oform1.formula.Replace("=bc", "=IF(bc")
                                    oform1.formula = oform1.formula.Replace(")", ")=0,""A"",""E"")")
                                    formulas.Add(oform1)

                                End If
                                item_col = item_col + 1

                                If off.c_show_ea.Checked = True And multiple_entity = True Then

                                    REM add e_a-after item
                                    Dim oform1 As New formula
                                    oform1.formula_entity = oent_formula
                                    oform1.item_type = "e_a"
                                    oform1.item = "E/A"
                                    oform1.assoc_entity = False
                                    If off.item_sel_assoc_class(j) = off.clink.Text Then
                                        oform1.assoc_entity = True
                                    End If

                                    oform1.year = ""
                                    oform1.period = ""
                                    oform1.row_offset = oent_formula.row_offset
                                    oform1.col_offset = item_col
                                    item_col = item_col + 1
                                    build_function_multiple_aggregation(oform1, True, False, False, True)
                                    oform1.formula = oform1.formula.Replace(",)", ",""e_a_flag"")")
                                    oform1.formula = oform1.formula.Replace("=bc", "=IF(bc")
                                    REM ING JUNE 2012
                                    oform1.formula = oform1.formula.Replace(")", ")=0,""A"",""E"")")
                                    REM --------------------
                                    formulas.Add(oform1)
                                End If



                            Next
                        Next
                        If off.rcurrnone.Checked = True And oform.item_monatary = True Then
                            REM add currency after item
                            oform = New formula
                            oform.formula_entity = oent_formula
                            oform.item = "Currency"
                            oform.item_type = "curr"
                            oform.assoc_entity = False
                            'If off.item_sel_assoc_class(j) = off.clink.Text Then
                            '    oform.assoc_entity = True
                            'End If
                            If off.clink.SelectedIndex > 0 Then

                                oform.assoc_entity = True
                            End If
                            If multiple_entity = True Then
                                oform.row_offset = oent_formula.row_offset
                                oform.col_offset = item_col
                                build_function_multiple_aggregation(oform, True, True, True, off.c_show_ea.Checked)
                            Else
                                oform.formula_entity.header.col_offset = 3 + start_col
                                oform.row_offset = start_row + 9 + j
                                oform.col_offset = start_col + 5
                                build_function_single_aggregation(oform, True, True, True, off.c_show_ea.Checked)
                            End If

                            oform.formula = oform.formula.Replace(")", """item_currency"")")
                            item_col = item_col + 1
                            formulas.Add(oform)
                        End If
                        REM add scale symbol formula
                        If multiple_entity = False And off.cformat.Checked = True Then
                            Dim oform1 As New formula
                            oform1.formula_entity = oent_formula
                            oform1.item = ""
                            oform1.item_type = "symbol"
                            oform1.row_offset = start_row + 9 + j
                            oform1.col_offset = start_col + 4
                            oform1.formula = "=bc_get_scale_symbol(" + excel_address(start_row + 9 + j, start_col + 3) + "," + excel_address(start_row + 9 + j, start_col + 2)
                            formulas.Add(oform1)
                        End If
                    Else
                        Dim oform As New formula
                        oform.formula_entity = oent_formula
                        oform.item = off.lselitem.Items(j)
                        oform.item_type = "item"
                        oform.item_factor = off.item_sel_factor(j)
                        oform.item_monatary = off.item_sel_monatary(j)
                        oform.item_symbol = off.item_sel_symbol(j)
                        oform.row_offset = oent_formula.row_offset
                        oform.col_offset = item_col
                        oform.assoc_entity = False
                        'If off.item_sel_assoc_class(j) = off.clink.Text Then
                        '    oform.assoc_entity = True
                        'End If
                        If off.clink.SelectedIndex > 0 Then

                            oform.assoc_entity = True
                        End If

                        If multiple_entity = True Then
                            If off.rcurrconv.Checked = True Then
                                currency_convert(oform)
                            Else
                                build_function_multiple_aggregation(oform)
                            End If
                        Else
                            oform.row_offset = start_row + 9 + j
                            oform.col_offset = start_col + 6
                            If off.rcurrconv.Checked = True Then
                                currency_convert(oform, True, False)
                            Else
                                build_function_single_aggregation(oform)
                            End If
                        End If
                        oform.formula = oform.formula.Replace(",)", ")")
                        If off.cformat.Checked = True Then
                            REM wrap function aound format
                            oform.formula = oform.formula.Substring(1, oform.formula.Length - 1)
                            If multiple_entity = True Then
                                oform.formula = "=bc_format(" + oform.formula + "," + excel_address(start_row + 2, oform.col_offset, True, False) + "," + excel_address(start_row + 1, oform.col_offset, True, False) + "," + excel_address(start_row + 3, oform.col_offset, True, False) + "," + excel_address(start_row + 9, oform.col_offset, True, False) + ")"
                            Else
                                oform.formula = "=bc_format(" + oform.formula + "," + excel_address(oform.row_offset, start_col + 1, False, True) + "," + excel_address(oform.row_offset, start_col, False, True) + "," + excel_address(oform.row_offset, start_col + 2, False, True) + "," + excel_address(oform.row_offset, start_col + 9, False, True) + ")"
                            End If
                        End If
                        formulas.Add(oform)
                        If off.cformat.Checked = True And multiple_entity = True Then
                            REM add scale symbol formula
                            Dim oform1 As New formula
                            oform1.formula_entity = oent_formula
                            oform1.item = ""
                            oform1.item_type = "symbol"
                            oform1.row_offset = start_row + 10
                            oform1.col_offset = item_col
                            oform1.formula = "=bc_get_scale_symbol(" + excel_address(start_row + 9, item_col) + "," + excel_address(start_row + 3, item_col) + ")"
                            formulas.Add(oform1)
                        End If
                        item_col = item_col + 1

                        If off.rcurrnone.Checked = True And oform.item_monatary = True Then
                            REM add currency after item
                            oform = New formula
                            oform.formula_entity = oent_formula
                            oform.item = "Currency"
                            oform.item_type = "curr"
                            oform.assoc_entity = False
                            'If off.item_sel_assoc_class(j) = off.clink.Text Then
                            '    oform.assoc_entity = True
                            'End If
                            If off.clink.SelectedIndex > 0 Then
                                oform.assoc_entity = True
                            End If
                            If multiple_entity = True Then
                                oform.row_offset = oent_formula.row_offset
                                oform.col_offset = item_col
                                build_function_multiple_aggregation(oform, True, True, True)
                            Else
                                oform.formula_entity.header.col_offset = 2 + start_col
                                oform.row_offset = start_row + 9 + j
                                oform.col_offset = start_col + 5
                                build_function_single_aggregation(oform, True, True, True, off.c_show_ea.Checked)
                            End If
                            oform.formula = oform.formula.Replace(")", """item_currency"")")
                            item_col = item_col + 1
                            formulas.Add(oform)
                        End If
                        If off.cformat.Checked = True Then
                            REM add scale symbol formula
                            If multiple_entity = False Then
                                Dim oform1 As New formula
                                oform1.formula_entity = oent_formula
                                oform1.item = ""
                                oform1.item_type = "symbol"
                                oform1.row_offset = start_row + 9 + j
                                oform1.col_offset = start_col + 4
                                oform1.formula = "=bc_get_scale_symbol(" + excel_address(start_row + 9 + j, start_col + 3) + "," + excel_address(start_row + 9 + j, start_col + 2) + ")"
                                formulas.Add(oform1)
                            End If
                        End If
                    End If
                Next
            Next

            end_row = 1
            end_col = 1
            For i = 0 To formulas.Count - 1
                If formulas(i).row_offset > end_row Then
                    end_row = formulas(i).row_offset
                End If
                If formulas(i).col_offset > end_col Then
                    end_col = formulas(i).col_offset
                End If
            Next

            If off.lvwdual.Items.Count > 0 Then
                build_portfolio_aggregation = output_to_excel(start_row, start_col, fh, entity_formulas, formulas, multiple_entity, False, 0, True, off.clink.Text, off.lvwdual.Items(0).SubItems(1).Text)
            Else
                build_portfolio_aggregation = output_to_excel(start_row, start_col, fh, entity_formulas, formulas, multiple_entity, False, 0, True)
            End If


        Catch ex As Exception
            build_portfolio_aggregation = False
            Dim oerr As New bc_cs_error_log("bc_am_ef_functions", "build_portfolio_aggregation", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            Dim otrace As New bc_cs_activity_log("bc_am_ef_functions", "build_portfolio_aggregation", bc_cs_activity_codes.TRACE_EXIT, "")

            off.Cursor = Cursors.Default
        End Try

    End Function
    Public Function output_to_excel(ByVal start_row As Integer, ByVal start_col As Integer, ByVal fh As Object, ByVal entity_formulas As ArrayList, ByVal formulas As ArrayList, Optional ByVal multiple_entity As Boolean = True, Optional ByVal time_series As Boolean = False, Optional ByVal tk_count As Integer = 0, Optional ByVal aggregation As Boolean = False, Optional ByVal dual_class As String = "", Optional ByVal dual_entity As String = "") As Boolean
        Dim calc_state As Integer
        Dim style_error As String = ""
        Dim den_str As String = ""
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        'Dim sheet_exists As Boolean
        Dim source_sheet_name As String
        Dim formula As String
        Dim maxrow As Integer, maxcol As Integer
        Dim rc As Integer
        Dim yeartext As String
        Dim chart_active_sheet As String

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim format_filename As String = "bc_pt_default.xls"
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                format_filename = "bc_pt_default.xlsx"
            End If
            Dim activesheet As String
            Dim i As Integer

            calc_state = application.application.calculation
            application.application.calculation = -4135
            application.application.screenupdating = False
            output_to_excel = True
            Dim oexcel As New bc_ao_in_excel(application)
            REM check if formats are already in excel workbook
            If check_formats_exists() = False Then
                If off.check_format_file_exists(format_filename) = True Then
                    REM load it
                    activesheet = application.application.activeworkbook.activesheet.name
                    oexcel.disable_application_alerts()
                    oexcel.insert_sheet("bc_pt_formats", bc_cs_central_settings.local_template_path + format_filename)
                    oexcel.enable_application_alerts()
                    application.application.activeworkbook.activesheet.visible = False
                    REM swicth back activesheet
                    For i = 1 To application.application.activeworkbook.worksheets.count
                        If application.application.activeworkbook.worksheets(i).name = activesheet Then
                            application.application.activeworkbook.worksheets(i).select()
                            Exit For
                        End If
                    Next
                Else
                    Dim omsg As New bc_cs_message("Blue Curve", "Format filename " + format_filename + "doesnt exist", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
            End If



            Dim yr_str As String = ""
            Dim pe_str As String = ""
            Dim en_str As String = ""
            For i = Year(Now) - 15 To Year(Now) + 15
                If i = Year(Now) - 15 Then
                    yr_str = CStr(i)
                Else
                    yr_str = yr_str + "," + CStr(i)
                End If
            Next
            For i = 0 To bc_am_ef_functions.period_names.Count - 1
                If i = 0 Then
                    pe_str = bc_am_ef_functions.period_names(0)
                Else
                    pe_str = pe_str + "," + bc_am_ef_functions.period_names(i)
                End If
            Next
            If multiple_entity = False Then
                For i = 0 To bc_am_ef_functions.entities.Count - 1
                    If bc_am_ef_functions.entities(i).class_id = bc_am_ef_functions.class_ids(off.cclass.SelectedIndex) Then
                        'off.lallentity.Items.Add(bc_am_ef_functions.entities(i).name)
                        If i = 0 Then
                            en_str = bc_am_ef_functions.entities(i).name
                        Else
                            en_str = en_str + "," + bc_am_ef_functions.entities(i).name
                        End If
                    End If
                Next
                den_str = ""
                REM XXX
                If dual_entity <> "" Then
                    For i = 0 To bc_am_ef_functions.entities.Count - 1
                        If bc_am_ef_functions.entities(i).class_id = bc_am_ef_functions.class_ids(off.clink.SelectedIndex) Then
                            If den_str = "" Then
                                den_str = bc_am_ef_functions.entities(i).name
                            Else
                                den_str = den_str + "," + bc_am_ef_functions.entities(i).name
                            End If
                        End If
                    Next
                End If

            End If



            REM set whole area into header colour
            Dim startcell As String
            Dim endcell As String
            Dim starttablecell As String
            Dim endtablecell As String = ""
            startcell = excel_address(start_row, start_col)
            If multiple_entity = True Then
                starttablecell = excel_address(start_row + 7, start_col)
            Else
                starttablecell = excel_address(start_row + 3, start_col + 3)
            End If
            Dim limit_exc As Boolean = False
            If multiple_entity = True Then
                If formulas(formulas.Count - 1).col_offset > 255 Then
                    limit_exc = True
                End If
            ElseIf multiple_entity = False And time_series = False Then
                If off.Cyearstart.Enabled = True Then
                    If start_col + 6 + (((CInt(off.cyearend.Text) - CInt(off.Cyearstart.Text) + 1) * off.lperiods.Items.Count)) > 255 Then
                        limit_exc = True
                    End If
                End If
            Else
                If formulas(formulas.Count - 1).col_offset > 255 Then
                    limit_exc = True
                End If
            End If
            If limit_exc = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "Excel column limit exeeded (255), please go back and choose less items years or periods", bc_cs_message.MESSAGE)
                output_to_excel = False
                Exit Function
            End If


            If multiple_entity = True Then

                endcell = excel_address(start_row + 6, formulas(formulas.Count - 1).col_offset)
                If off.tsource.Text = "" Then
                    endtablecell = excel_address(start_row + 10 + off.lselentity.Items.Count, formulas(formulas.Count - 1).col_offset)
                Else
                    endtablecell = excel_address(start_row + 11 + off.lselentity.Items.Count, formulas(formulas.Count - 1).col_offset)
                End If
            ElseIf multiple_entity = False And time_series = False Then
                If off.Cyearstart.Enabled = True Then
                    endcell = excel_address(start_row + 8 + off.lselitem.Items.Count + 1, start_col + 6 + (((CInt(off.cyearend.Text) - CInt(off.Cyearstart.Text) + 1) * off.lperiods.Items.Count)))
                    endtablecell = excel_address(start_row + 8 + off.lselitem.Items.Count + 1, start_col + 6 + (((CInt(off.cyearend.Text) - CInt(off.Cyearstart.Text) + 1) * off.lperiods.Items.Count)))
                Else
                    endcell = excel_address(start_row + 8 + off.lselitem.Items.Count + 1, start_col + 6)
                    endtablecell = excel_address(start_row + 8 + off.lselitem.Items.Count + 1, start_col + 6)
                End If
            Else
                endcell = excel_address(start_row + 6 + tk_count + 3, formulas(formulas.Count - 1).col_offset)
            End If
            REM header background
            Try
                application.Application.ActiveSheet.Range(startcell + ":" + endcell).style = "bc_pt_bg"
            Catch
                style_error = " bc_pt_bg"
            End Try
            REM table background
            Try
                application.Application.ActiveSheet.Range(starttablecell + ":" + endtablecell).style = "bc_pt_table_bg"
            Catch
                style_error = " bc_pt_table_bg"
            End Try

            application.application.activeworkbook.activesheet.cells(start_row, start_col) = "Blue Curve Extract"
            application.application.activeworkbook.activesheet.cells(start_row, start_col).font.bold = True
            application.application.activeworkbook.activesheet.cells(start_row, start_col + 1) = fh.title

            start_row = start_row + 1


            REM write out static headers
            REM workflow stage class
            If time_series = True Then
                application.application.activeworkbook.activesheet.cells(start_row + 7, start_col) = "Date"
                application.application.activeworkbook.activesheet.cells(start_row + 7, start_col).font.bold = True

                If off.rcurrnone.Checked = True Then
                    application.application.activeworkbook.activesheet.cells(start_row + 7, start_col + 2) = "Currency"
                    application.application.activeworkbook.activesheet.cells(start_row + 7, start_col + 2).font.bold = True

                End If
            End If

            REM workflow stage
            application.application.activeworkbook.activesheet.cells(start_row, start_col) = "Stage"
            application.application.activeworkbook.activesheet.cells(start_row, start_col).HorizontalAlignment = -4152


            application.application.activeworkbook.activesheet.cells(start_row, start_col).font.bold = True

            Dim ltx As String = ""
            For i = 0 To bc_am_ef_functions.stage_names.Count - 1
                If i = 0 Then
                    ltx = bc_am_ef_functions.stage_names(i)
                Else
                    ltx = ltx + "," + bc_am_ef_functions.stage_names(i)
                End If

            Next
            If bc_am_ef_functions.ousersettings.stages <> 0 Then
                off.cstage.Enabled = False
                If bc_am_ef_functions.ousersettings.stages = 1 Then
                    ltx = "Draft"
                Else
                    ltx = "Publish"
                End If
            End If

            With application.application.activeworkbook.activesheet.cells(start_row, start_col + 1).Validation()
                .Delete()
                .Add(Type:=3, AlertStyle:=1, Operator:= _
                1, Formula1:=ltx)
                .IgnoreBlank = True
                .InCellDropdown = True
                .InputTitle = ""
                .ErrorTitle = "Input Error"
                .InputMessage = ""
                .ErrorMessage = "Invalid Workflow Stage"
                .ShowInput = True
                .ShowError = True
            End With
            application.application.activeworkbook.activesheet.cells(start_row, start_col + 1) = fh.stage
            Try
                application.application.activeworkbook.activesheet.cells(start_row, start_col + 1).style = "bc_pt_filter"
            Catch
                style_error = style_error + "bc_pt_filter"
            End Try

            REM exchange method
            If fh.exchange_method <> "" Then

                application.application.activeworkbook.activesheet.cells(5, 1) = "Exchange Method"
                application.application.activeworkbook.activesheet.cells(5, 1).HorizontalAlignment = -4152


                application.application.activeworkbook.activesheet.cells(5, 1).font.bold = True


                With application.application.activeworkbook.activesheet.cells(5, 2).Validation()
                    .Delete()
                    .Add(Type:=3, AlertStyle:=1, Operator:= _
                    1, Formula1:="Current, Average, End")
                    .IgnoreBlank = True
                    .InCellDropdown = True
                    .InputTitle = ""
                    .ErrorTitle = "Input Error"
                    .InputMessage = ""
                    .ErrorMessage = "Invalid Exchange Method Stage"
                    .ShowInput = True
                    .ShowError = True
                End With
                application.application.activeworkbook.activesheet.cells(5, 2) = fh.exchange_method
                Try
                    application.application.activeworkbook.activesheet.cells(5, 2).style = "bc_pt_filter"
                Catch
                    style_error = style_error + "bc_pt_filter"
                End Try
            End If


            If aggregation = True Then
                application.application.activeworkbook.activesheet.cells(6, 1) = "Aggregation Universe"
                application.application.activeworkbook.activesheet.cells(6, 1).font.bold = True
                application.application.activeworkbook.activesheet.cells(6, 2) = fh.universe
                application.application.activeworkbook.activesheet.cells(6, 2).style = "bc_pt_filter"
            End If



            REM class

            If multiple_entity = True Then
                If aggregation = True Then
                    application.application.activeworkbook.activesheet.cells(7, 1) = fh.entity_class
                    application.application.activeworkbook.activesheet.cells(7, 1).font.bold = True
                Else
                    application.application.activeworkbook.activesheet.cells(fh.item_row_offset + 1, start_col) = fh.entity_class
                    application.application.activeworkbook.activesheet.cells(fh.item_row_offset + 1, start_col).font.bold = True
                End If


                If aggregation = False Then
                    application.application.activeworkbook.activesheet.cells(fh.item_row_offset + 2, start_col) = fh.prop
                    application.application.activeworkbook.activesheet.cells(fh.item_row_offset + 2, start_col).font.bold = True

                End If

                If off.clink.SelectedIndex > 0 Then
                    If aggregation = False Then
                        application.application.activeworkbook.activesheet.cells(fh.item_row_offset + 1, start_col + 1) = off.clink.Text
                        application.application.activeworkbook.activesheet.cells(fh.item_row_offset + 1, start_col + 1).font.bold = True
                    Else
                        application.application.activeworkbook.activesheet.cells(fh.item_row_offset + 2, start_col + 1) = off.clink.Text
                        application.application.activeworkbook.activesheet.cells(fh.item_row_offset + 2, start_col + 1).font.bold = True

                    End If
                End If

                REM title sub title
                If off.ttitle.Text <> "" Then
                    application.application.activeworkbook.activesheet.cells(fh.item_row_offset + 3, start_col) = off.ttitle.Text
                    Try
                        application.application.activeworkbook.activesheet.cells(fh.item_row_offset + 3, start_col).style = "bc_pt_title"
                    Catch
                        style_error = style_error + " bc_pt_title"
                    End Try
                End If
                If off.tsubtitle.Text <> "" Then
                    application.application.activeworkbook.activesheet.cells(fh.item_row_offset + 4, start_col) = off.tsubtitle.Text
                    Try
                        application.application.activeworkbook.activesheet.cells(fh.item_row_offset + 4, start_col).style = "bc_pt_subtitle"
                    Catch
                        style_error = style_error + " bc_pt_subtitle"
                    End Try
                End If
                If off.tsource.Text <> "" Then
                    application.application.activeworkbook.activesheet.cells(start_row + 11 + off.lselentity.Items.Count - 1, start_col) = off.tsource.Text
                    Try
                        application.application.activeworkbook.activesheet.cells(start_row + 11 + off.lselentity.Items.Count - 1, start_col).style = "bc_pt_source"
                    Catch
                        style_error = style_error + " bc_pt_title"
                    End Try
                End If

            Else
                If off.ttitle.Text <> "" Then
                    application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 3) = off.ttitle.Text
                    Try
                        application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 3).style = "bc_pt_title"
                    Catch
                        style_error = style_error + " bc_pt_title"
                    End Try
                End If
                If off.tsubtitle.Text <> "" Then
                    application.application.activeworkbook.activesheet.cells(start_row + 3, start_col + 3) = off.tsubtitle.Text
                    Try
                        application.application.activeworkbook.activesheet.cells(start_row + 3, start_col + 3).style = "bc_pt_subtitle"
                    Catch
                        style_error = style_error + " bc_pt_subtitle"
                    End Try
                End If
                If off.tsource.Text <> "" Then
                    application.application.activeworkbook.activesheet.cells(start_row + 9 + off.lselitem.Items.Count - 1, start_col + 3) = off.tsource.Text
                    Try
                        application.application.activeworkbook.activesheet.cells(start_row + 9 + off.lselitem.Items.Count - 1, start_col + 3).style = "bc_pt_source"
                    Catch
                        style_error = style_error + " bc_pt_title"
                    End Try
                End If
                If aggregation = True Then
                    application.application.activeworkbook.activesheet.cells(start_row + 5, start_col) = fh.entity_class
                    application.application.activeworkbook.activesheet.cells(start_row + 5, start_col).font.bold = True
                    application.application.activeworkbook.activesheet.cells(start_row + 5, start_col).HorizontalAlignment = -4152
                Else

                    application.application.activeworkbook.activesheet.cells(start_row + 4, start_col) = fh.entity_class
                    application.application.activeworkbook.activesheet.cells(start_row + 4, start_col).font.bold = True
                    application.application.activeworkbook.activesheet.cells(start_row + 4, start_col).HorizontalAlignment = -4152
                End If
                application.application.activeworkbook.activesheet.cells(start_row + 4, start_col).HorizontalAlignment = -4152
                If off.clink.SelectedIndex > 0 Then
                    If aggregation = True Then
                        application.application.activeworkbook.activesheet.cells(start_row + 6, start_col) = off.clink.Text
                        application.application.activeworkbook.activesheet.cells(start_row + 6, start_col).font.bold = True
                        application.application.activeworkbook.activesheet.cells(start_row + 6, start_col).HorizontalAlignment = -4152
                    Else
                        application.application.activeworkbook.activesheet.cells(start_row + 5, start_col) = off.clink.Text
                        application.application.activeworkbook.activesheet.cells(start_row + 5, start_col).font.bold = True
                        application.application.activeworkbook.activesheet.cells(start_row + 5, start_col).HorizontalAlignment = -4152
                    End If
                    REM evaluate potentials
                    Dim j As Integer

                    If aggregation = False Then
                        Dim fm As String = ""
                        For i = 0 To bc_am_ef_functions.class_ids.Count - 1
                            For j = 0 To off.use_class_ids.Count - 1
                                If off.use_class_ids(j) = bc_am_ef_functions.class_ids(i) And bc_am_ef_functions.class_names(i) = off.clink.Text Then
                                    If off.use_class_types(j) = "Parent" Then
                                        fm = "=bc_get_parent_associations(" + excel_address(start_row + 4, start_col) + "," + excel_address(start_row + 4, start_col + 1) + "," + excel_address(start_row + 5, start_col) + ",""" + off.cschema.Text + """,""all"",""name"")"
                                    Else
                                        fm = "=bc_get_child_associations(" + excel_address(start_row + 4, start_col) + "," + excel_address(start_row + 4, start_col + 1) + "," + excel_address(start_row + 5, start_col) + ",""" + off.cschema.Text + """,""all"",""name"")"
                                    End If
                                    Exit For
                                End If
                            Next
                        Next
                        application.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 1).formula = fm
                    Else
                        application.application.activeworkbook.activesheet.cells(start_row + 6, start_col + 1) = dual_entity
                        Try
                            application.application.activeworkbook.activesheet.cells(start_row + 6, start_col + 1).style = "bc_pt_filter"
                        Catch
                            style_error = style_error + "bc_pt_filter"
                        End Try
                        Try

                            With application.application.activeworkbook.activesheet.cells(start_row + 6, start_col + 1).Validation()
                                .Delete()
                                .Add(Type:=3, AlertStyle:=1, Operator:= _
                                1, Formula1:=den_str)
                                .IgnoreBlank = True
                                .InCellDropdown = True
                                .InputTitle = ""
                                .ErrorTitle = ""
                                .InputMessage = ""
                                .ErrorMessage = ""
                                .ShowInput = True
                                .ShowError = True
                            End With
                        Catch ex As Exception



                        End Try

                    End If
                End If
            End If
            REM contributor
            application.application.activeworkbook.activesheet.cells(start_row + 1, start_col) = "Contributor"
            application.application.activeworkbook.activesheet.cells(start_row + 1, start_col).HorizontalAlignment = -4152
            application.application.activeworkbook.activesheet.cells(start_row + 1, start_col).font.bold = True
            For i = 0 To bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links.Count - 1
                For j = 0 To bc_am_ef_functions.contributor_ids.Count - 1
                    If bc_am_ef_functions.contributor_ids(j) = bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).schema_id Then
                        If i = 0 Then
                            ltx = bc_am_ef_functions.contributor_names(j)
                        Else
                            ltx = ltx + "," + bc_am_ef_functions.contributor_names(j)
                        End If
                        Exit For
                    End If
                Next
            Next
            If ltx = "" Then
                For i = 0 To bc_am_ef_functions.contributor_names.Count - 1
                    If i = 0 Then
                        ltx = bc_am_ef_functions.contributor_names(i)
                    Else
                        ltx = ltx + "," + bc_am_ef_functions.contributor_names(i)
                    End If
                Next
            End If

            With application.application.activeworkbook.activesheet.cells(start_row + 1, start_col + 1).Validation()
                .Delete()
                .Add(Type:=3, AlertStyle:=1, Operator:= _
                1, Formula1:=ltx)
                .IgnoreBlank = True
                .InCellDropdown = True
                .InputTitle = ""
                .ErrorTitle = ""
                .InputMessage = ""
                .ErrorMessage = ""
                .ShowInput = True
                .ShowError = True
            End With
            Try
                application.application.activeworkbook.activesheet.cells(start_row + 1, start_col + 1).style = "bc_pt_filter"
            Catch
                style_error = style_error + "bc_pt_filter"
            End Try
            application.application.activeworkbook.activesheet.cells(start_row + 1, start_col + 1) = fh.contributor

            REM dateat
            Try
                With application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1).Validation()

                    .Delete()
                    .Add(Type:=4, AlertStyle:=1, Operator:= _
                    1, Formula1:="1/1/1900", Formula2:="9/9/9999")
                    .IgnoreBlank = True
                    .InCellDropdown = True
                    .InputTitle = ""
                    .ErrorTitle = "Input Error"
                    .InputMessage = ""
                    .ErrorMessage = "Value must be date"
                    .ShowInput = True
                    .ShowError = True
                End With
            Catch

            End Try
            If time_series = False Then
                application.application.activeworkbook.activesheet.cells(start_row + 2, start_col) = "Date at"
            Else
                application.application.activeworkbook.activesheet.cells(start_row + 2, start_col) = "Date Range"
            End If
            application.application.activeworkbook.activesheet.cells(start_row + 2, start_col).HorizontalAlignment = -4152

            application.application.activeworkbook.activesheet.cells(start_row + 2, start_col).font.bold = True
            If time_series = False Then
                application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1) = Format(fh.date_at, "dd-MMM-yyyy")
                Try
                    application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1).style = "bc_pt_filter"
                Catch
                    style_error = style_error + " bc_pt_filter"
                End Try
                Try
                    With application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1).Validation()

                        .Delete()
                        .Add(Type:=4, AlertStyle:=1, Operator:= _
                        1, Formula1:="1/1/1900", Formula2:="9/9/9999")
                        .IgnoreBlank = True
                        .InCellDropdown = True
                        .InputTitle = ""
                        .ErrorTitle = "Input Error"
                        .InputMessage = ""
                        .ErrorMessage = "Value must be date"
                        .ShowInput = True
                        .ShowError = True
                    End With
                Catch

                End Try
                application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1) = Format(fh.date_at, "dd-MMM-yyyy")
            Else
                'application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1) = Format(fh.date_from, "dd-MMM-yyyy")
                'Try
                '    application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1).style = "bc_pt_filter"
                'Catch
                '    style_error = style_error + " bc_pt_filter"
                'End Try
                'application.application.activeworkbook.activesheet.cells(start_row + 1, start_col + 2) = Format(fh.date_at, "dd-MMM-yyyy")


                'Try
                '    application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 2).style = "bc_pt_filter"
                'Catch
                '    style_error = style_error + " bc_pt_filter"
                'End Try
                'Try
                '    With application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 2).Validation()

                '        .Delete()
                '        .Add(Type:=4, AlertStyle:=1, Operator:= _
                '        1, Formula1:="1/1/1900", Formula2:="9/9/9999")
                '        .IgnoreBlank = True
                '        .InCellDropdown = True
                '        .InputTitle = ""
                '        .ErrorTitle = "Input Error"
                '        .InputMessage = ""
                '        .ErrorMessage = "Value must be date"
                '        .ShowInput = True
                '        .ShowError = True
                '    End With

                application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1) = Format(fh.date_from, "dd-MMM-yyyy")
                Try
                    application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 1).style = "bc_pt_filter"
                Catch
                    style_error = style_error + " bc_pt_filter"
                End Try
                application.application.activeworkbook.activesheet.cells(start_row + 1, start_col + 2) = Format(fh.date_at, "dd-MMM-yyyy")


                Try
                    application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 2).style = "bc_pt_filter"
                Catch
                    style_error = style_error + " bc_pt_filter"
                End Try
                Try
                    With application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 2).Validation()

                        .Delete()
                        .Add(Type:=4, AlertStyle:=1, Operator:= _
                        1, Formula1:="1/1/1900", Formula2:="9/9/9999")
                        .IgnoreBlank = True
                        .InCellDropdown = True
                        .InputTitle = ""
                        .ErrorTitle = "Input Error"
                        .InputMessage = ""
                        .ErrorMessage = "Value must be date"
                        .ShowInput = True
                        .ShowError = True
                    End With

                Catch

                End Try
                application.application.activeworkbook.activesheet.cells(start_row + 2, start_col + 2) = Format(fh.date_at, "dd-MMM-yyyy")

            End If

            If off.rcurrconv.Checked = True Then
                application.application.activeworkbook.activesheet.cells(start_row + 3, start_col) = "Currency"
                application.application.activeworkbook.activesheet.cells(start_row + 3, start_col).HorizontalAlignment = -4152

                application.application.activeworkbook.activesheet.cells(start_row + 3, start_col).font.bold = True
                For i = 0 To bc_am_ef_functions.currency_codes.Count - 1
                    If i = 0 Then
                        ltx = bc_am_ef_functions.currency_codes(i)
                    Else
                        ltx = ltx + "," + bc_am_ef_functions.currency_codes(i)
                    End If

                Next
                With application.application.activeworkbook.activesheet.cells(start_row + 3, start_col + 1).Validation()
                    .Delete()
                    .Add(Type:=3, AlertStyle:=1, Operator:= _
                    1, Formula1:=ltx)
                    .IgnoreBlank = True
                    .InCellDropdown = True
                    .InputTitle = ""
                    .ErrorTitle = ""
                    .InputMessage = ""
                    .ErrorMessage = ""
                    .ShowInput = True
                    .ShowError = True
                End With
                application.application.activeworkbook.activesheet.cells(start_row + 3, start_col + 1) = fh.currency_code
                Try
                    application.application.activeworkbook.activesheet.cells(start_row + 3, start_col + 1).style = "bc_pt_filter"
                Catch
                    style_error = style_error + "bc_pt_filter"
                End Try

            End If

            REM table starts here
            REM write out entities

            If multiple_entity = True Then
                For i = 0 To entity_formulas.Count - 1
                    application.application.activeworkbook.activesheet.cells(entity_formulas(i).row_offset, entity_formulas(i).col_offset) = entity_formulas(i).name

                    Try
                        application.application.activeworkbook.activesheet.cells(entity_formulas(i).row_offset, entity_formulas(i).col_offset).style = "bc_pt_title_left"
                    Catch
                        style_error = style_error + "bc_pt_title_left"
                    End Try
                    If off.clink.SelectedIndex > 0 Then

                        application.application.activeworkbook.activesheet.cells(entity_formulas(i).row_offset, entity_formulas(i).col_offset + 1) = entity_formulas(i).associated_name
                        Try
                            application.application.activeworkbook.activesheet.cells(entity_formulas(i).row_offset, entity_formulas(i).col_offset + 1).style = "bc_pt_title_left"
                        Catch
                            style_error = style_error + "bc_pt_title_left"
                        End Try
                    End If

                Next
            Else

                Try
                    If aggregation = False Then

                        With application.application.activeworkbook.activesheet.cells(start_row + 4, start_col + 1).Validation()
                            .Delete()
                            .Add(Type:=3, AlertStyle:=1, Operator:= _
                            1, Formula1:=en_str)
                            .IgnoreBlank = True
                            .InCellDropdown = True
                            .InputTitle = ""
                            .ErrorTitle = ""
                            .InputMessage = ""
                            .ErrorMessage = ""
                            .ShowInput = True
                            .ShowError = True
                        End With
                    Else

                        With application.application.activeworkbook.activesheet.cells(7, start_col + 1).Validation()
                            .Delete()
                            .Add(Type:=3, AlertStyle:=1, Operator:= _
                            1, Formula1:=en_str)
                            .IgnoreBlank = True
                            .InCellDropdown = True
                            .InputTitle = ""
                            .ErrorTitle = ""
                            .InputMessage = ""
                            .ErrorMessage = ""
                            .ShowInput = True
                            .ShowError = True
                        End With

                    End If
                Catch ex As Exception
                End Try
                If aggregation = False Then
                    application.application.activeworkbook.activesheet.cells(start_row + 4, start_col + 1) = entity_formulas(0).name
                    Try
                        application.application.activeworkbook.activesheet.cells(start_row + 4, start_col + 1).style = "bc_pt_filter"
                    Catch
                        style_error = style_error + "bc_pt_filter"
                    End Try
                Else
                    application.application.activeworkbook.activesheet.cells(7, start_col + 1) = entity_formulas(0).name
                    Try
                        application.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 1).style = "bc_pt_filter"
                    Catch
                        style_error = style_error + "bc_pt_filter"
                    End Try
                End If



                application.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 6) = "Static"
                Try
                    application.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 6).style = "bc_pt_title_top"
                Catch
                    style_error = style_error + "bc_pt_title_top"
                End Try
                If off.rcurrnone.Checked = True Then
                    application.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 5) = "Currency"
                    Try
                        application.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 5).style = "bc_pt_title_top"
                    Catch
                        style_error = style_error + "bc_pt_title_top"
                    End Try
                End If
                If off.cformat.Checked = True Then
                    If time_series = False Then
                        application.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 4) = "Symbol"
                        Try
                            application.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 4).style = "bc_pt_title_top"
                        Catch
                            style_error = style_error + "bc_pt_title_top"
                        End Try
                        application.application.activeworkbook.activesheet.cells(start_row + 7, start_col + 0) = "Precision"
                        application.application.activeworkbook.activesheet.cells(start_row + 7, start_col + 1) = "Type"
                        application.application.activeworkbook.activesheet.cells(start_row + 7, start_col + 2) = "Factor"
                    End If
                End If
            End If
            application.Calculation = -4135

            Dim max_col As Integer
            If multiple_entity = True Or time_series = True Then
                For i = 0 To formulas.Count - 1
                    If time_series = False Or (time_series = True And i > 0) Then
                        If (time_series = False Or formulas(i).item_type = "item") And formulas(i).item_type <> "symbol" Then
                            application.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 5, formulas(i).col_offset) = formulas(i).item
                            Try
                                application.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 5, formulas(i).col_offset).style = "bc_pt_title_top"
                            Catch
                                style_error = style_error + "bc_pt_title_top"
                            End Try
                        End If
                    End If
                    REM if precsion requested and value then set up
                    If off.cformat.Checked = True Then
                        If formulas(i).item_type = "item" Then
                            If time_series = False Or (time_series = True And i > 0) Then

                                If off.cprectype.SelectedIndex = 0 Then
                                    application.application.activeworkbook.activesheet.cells(start_row + 1, formulas(i).col_offset) = "dps"
                                Else
                                    application.application.activeworkbook.activesheet.cells(start_row + 1, formulas(i).col_offset) = "sfs"
                                End If
                                With application.application.activeworkbook.activesheet.cells(start_row + 1, formulas(i).col_offset).Validation()
                                    .Delete()
                                    .Add(Type:=3, AlertStyle:=1, Operator:= _
                                    1, Formula1:="dps,sfs")
                                    .IgnoreBlank = True
                                    .InCellDropdown = True
                                    .InputTitle = ""
                                    .ErrorTitle = "Error"
                                    .InputMessage = "Select decimal places or sig figs"
                                    .ErrorMessage = "Please select dps or sfs"
                                    .ShowInput = True
                                    .ShowError = True
                                End With
                                application.application.activeworkbook.activesheet.cells(start_row - 1, formulas(i).col_offset) = "Precision"
                                application.application.activeworkbook.activesheet.cells(start_row - 1, formulas(i).col_offset).HorizontalAlignment = -4152
                                application.application.activeworkbook.activesheet.cells(start_row, formulas(i).col_offset) = off.cprec.Text
                                With application.application.activeworkbook.activesheet.cells(start_row, formulas(i).col_offset).Validation()
                                    .Delete()
                                    .Add(Type:=3, AlertStyle:=1, Operator:= _
                                    1, Formula1:="0,1,2,3,4,5,6,7,8,9,10")
                                    .IgnoreBlank = True
                                    .InCellDropdown = True
                                    .InputTitle = ""
                                    .ErrorTitle = ""
                                    .InputMessage = "Select number of decimal places or sig figs"
                                    .ErrorMessage = ""
                                    .ShowInput = True
                                    .ShowError = True
                                End With
                                Try
                                    application.application.activeworkbook.activesheet.cells(start_row, formulas(i).col_offset).style = "bc_pt_filter"
                                Catch
                                    style_error = style_error + " bc_pt_filter"
                                End Try
                                Try
                                    application.application.activeworkbook.activesheet.cells(start_row + 1, formulas(i).col_offset).style = "bc_pt_filter"
                                Catch
                                    style_error = style_error + " bc_pt_filter"
                                End Try
                                With application.application.activeworkbook.activesheet.cells(start_row + 2, formulas(i).col_offset).Validation()
                                    .Delete()
                                    .Add(Type:=3, AlertStyle:=1, Operator:= _
                                    1, Formula1:="1,10,100,1000,1000000,0.01")
                                    .IgnoreBlank = True
                                    .InCellDropdown = True
                                    .InputTitle = ""
                                    .ErrorTitle = ""
                                    .InputMessage = "Select scale factor"
                                    .ErrorMessage = ""
                                    .ShowInput = True
                                    .ShowError = True
                                End With
                                application.application.activeworkbook.activesheet.cells(start_row + 2, formulas(i).col_offset) = formulas(i).item_factor
                                Try
                                    application.application.activeworkbook.activesheet.cells(start_row + 2, formulas(i).col_offset).style = "bc_pt_filter"
                                Catch
                                    style_error = style_error + "bc_pt_filter"
                                End Try

                            End If
                        End If
                    End If



                    If formulas(i).year <> "" Then
                        REM year list
                        With application.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 3, formulas(i).col_offset).Validation()
                            .Delete()
                            .Add(Type:=3, AlertStyle:=1, Operator:= _
                            1, Formula1:=yr_str)
                            .IgnoreBlank = True
                            .InCellDropdown = True
                            .InputTitle = ""
                            .ErrorTitle = ""
                            .InputMessage = ""
                            .ErrorMessage = ""
                            .ShowInput = True
                            .ShowError = True
                        End With

                        application.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 3, formulas(i).col_offset) = formulas(i).year
                        Try
                            application.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 3, formulas(i).col_offset).style = "bc_pt_year"
                        Catch
                            style_error = style_error + "bc_pt_year"
                        End Try
                        If formulas(i).period <> "" Then
                            REM period list
                            With application.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 4, formulas(i).col_offset).Validation()
                                .Delete()
                                .Add(Type:=3, AlertStyle:=1, Operator:= _
                                1, Formula1:=pe_str)
                                .IgnoreBlank = True
                                .InCellDropdown = True
                                .InputTitle = ""
                                .ErrorTitle = ""
                                .InputMessage = ""
                                .ErrorMessage = ""
                                .ShowInput = True
                                .ShowError = True
                            End With
                        End If
                        application.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 4, formulas(i).col_offset) = formulas(i).period
                        Try
                            application.application.activeworkbook.activesheet.cells(formulas(i).formula_entity.header.item_row_offset + 4, formulas(i).col_offset).style = "bc_pt_period"
                        Catch
                            style_error = style_error + " " + "bc_pt_period"

                        End Try
                    End If
                    application.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).formula = formulas(i).formula
                    Try
                        If formulas(i).item_type = "item" Then
                            application.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_data"
                            If formulas(i).item_symbol = "Date" Then
                                application.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).numberformat = "dd/mm/yyyy"
                            End If
                        ElseIf formulas(i).item_type = "curr" Then
                            application.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_currency"
                        ElseIf formulas(i).item_type = "symbol" Then
                            application.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_symbol"
                        Else
                            application.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_ea"
                        End If
                    Catch ex As Exception
                        style_error = style_error + "bc_pt_data or bc_pt_currency or bt_pt_ea"
                    End Try
                    'With application.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset)
                    '    .style = "bc_pt_data"
                    'End With
                    max_col = formulas(i).col_offset

                Next
            Else
                rc = 0
                rc = rc + 3
                Dim j As Integer
                REM write out year and periods
                If off.Cyearstart.Enabled = True Then
                    rc = rc + 1
                    For i = CInt(off.Cyearstart.Text) To CInt(off.cyearend.Text)
                        For j = 0 To off.lperiods.Items.Count - 1
                            With application.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 3 + rc).Validation()
                                .Delete()
                                .Add(Type:=3, AlertStyle:=1, Operator:= _
                                1, Formula1:=yr_str)
                                .IgnoreBlank = True
                                .InCellDropdown = True
                                .InputTitle = ""
                                .ErrorTitle = ""
                                .InputMessage = ""
                                .ErrorMessage = ""
                                .ShowInput = True
                                .ShowError = True
                            End With
                            With application.application.activeworkbook.activesheet.cells(start_row + 6, start_col + 3 + rc).Validation()
                                .Delete()
                                .Add(Type:=3, AlertStyle:=1, Operator:= _
                                1, Formula1:=pe_str)
                                .IgnoreBlank = True
                                .InCellDropdown = True
                                .InputTitle = ""
                                .ErrorTitle = ""
                                .InputMessage = ""
                                .ErrorMessage = ""
                                .ShowInput = True
                                .ShowError = True
                            End With

                            application.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 3 + rc) = i
                            application.application.activeworkbook.activesheet.cells(start_row + 6, start_col + 3 + rc) = off.lperiods.Items(j)
                            Try
                                application.application.activeworkbook.activesheet.cells(start_row + 5, start_col + 3 + rc).style = "bc_pt_year"
                            Catch
                                style_error = style_error + " bc_pt_year"
                            End Try
                            Try
                                application.application.activeworkbook.activesheet.cells(start_row + 6, start_col + 3 + rc).style = "bc_pt_period"
                            Catch
                                style_error = style_error + " bc_pt_period"
                            End Try

                            rc = rc + 1
                        Next
                    Next
                End If
                rc = 0
                For i = 0 To formulas.Count - 1
                    If formulas(i).item_type = "item" And formulas(i).first = True Then
                        If time_series = True And i = 0 Then
                            application.application.activeworkbook.activesheet.cells(start_row + 8, start_col) = "Date"
                            application.application.activeworkbook.activesheet.cells(start_row + 8, start_col + 2) = formulas(i).item
                            application.application.activeworkbook.activesheet.cells(start_row + 8, start_col).font.bold = True
                            application.application.activeworkbook.activesheet.cells(start_row + 8, start_col + 2).font.bold = True
                        Else
                            application.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 3) = formulas(i).item
                            Try
                                application.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 3).style = "bc_pt_title_left"
                            Catch
                                style_error = style_error + " bc_pt_title_left"
                            End Try
                        End If

                        If off.cformat.Checked = True Then

                            With application.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 1).Validation()
                                .Delete()
                                .Add(Type:=3, AlertStyle:=1, Operator:= _
                                1, Formula1:="dps,sfs")
                                .IgnoreBlank = True
                                .InCellDropdown = True
                                .InputTitle = ""
                                .ErrorTitle = "Error"
                                .InputMessage = ""
                                .ErrorMessage = "Please select dps or sfs"
                                .ShowInput = True
                                .ShowError = True
                            End With
                            If off.cprectype.SelectedIndex = 0 Then
                                application.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 1) = "dps"
                            Else
                                application.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 1) = "sfs"
                            End If
                            application.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col) = off.cprec.Text
                            Try
                                application.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col).style = "bc_pt_filter"
                            Catch
                                style_error = style_error + " bc_pt_filter"
                            End Try
                            With application.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col).Validation()
                                .Delete()
                                .Add(Type:=3, AlertStyle:=1, Operator:= _
                                1, Formula1:="0,1,2,3,4,5,6,7,8,9,10")
                                .IgnoreBlank = True
                                .InCellDropdown = True
                                .InputTitle = ""
                                .ErrorTitle = ""
                                .InputMessage = ""
                                .ErrorMessage = ""
                                .ShowInput = True
                                .ShowError = True
                            End With
                            Try
                                application.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 2).style = "bc_pt_filter"
                            Catch
                                style_error = style_error + " bc_pt_filter"
                            End Try
                            application.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 2) = formulas(i).item_factor
                            Try
                                application.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 2).style = "bc_pt_filter"
                            Catch
                                style_error = style_error + " bc_pt_filter"
                            End Try
                            With application.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 2).Validation()
                                .Delete()
                                .Add(Type:=3, AlertStyle:=1, Operator:= _
                                1, Formula1:="1,10,100,1000,1000000,0.01")
                                .IgnoreBlank = True
                                .InCellDropdown = True
                                .InputTitle = ""
                                .ErrorTitle = ""
                                .InputMessage = ""
                                .ErrorMessage = ""
                                .ShowInput = True
                                .ShowError = True
                            End With
                            Try
                                application.application.activeworkbook.activesheet.cells(start_row + 8 + rc, start_col + 1).style = "bc_pt_filter"
                            Catch
                                style_error = style_error + " bc_pt_filter"
                            End Try

                        End If
                        rc = rc + 1
                    End If


                    Try
                        application.application.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).formula = formulas(i).formula
                    Catch

                    End Try

                    Try
                        If formulas(i).item_type = "item" Then
                            application.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_data"
                            If formulas(i).item_symbol = "Date" Then
                                application.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).numberformat = "dd/mm/yyyy"
                            End If
                        ElseIf formulas(i).item_type = "curr" Then
                            application.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_currency"
                        ElseIf formulas(i).item_type = "symbol" Then
                            application.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_symbol"
                        Else
                            application.application.activeworkbook.activesheet.cells(formulas(i).row_offset, formulas(i).col_offset).style = "bc_pt_ea"
                        End If
                    Catch ex As Exception
                        style_error = style_error + "bc_pt_data or bc_pt_currency or bt_pt_ea"
                    End Try
                    max_col = formulas(i).col_offset
                Next
            End If

            REM autosize used columns
            For i = start_col To max_col
                application.activeworkbook.activesheet.Columns(i).Select()
                application.selection.Columns.AutoFit()
            Next
            REM remove style sheet
            For i = 1 To application.application.activeworkbook.worksheets.count
                If application.application.activeworkbook.worksheets(i).name = "BC Portfolio Style Sheet" Then
                    oexcel.disable_application_alerts()
                    application.application.activeworkbook.worksheets(i).delete()
                    oexcel.enable_application_alerts()
                    Exit For
                End If
            Next
            application.application.activeworkbook.activesheet.cells(start_row - 1, start_col + 1).select()

            If time_series = True Then
                For i = 0 To tk_count - 1
                    Try
                        application.application.activeworkbook.activesheet.cells(start_row + 9 + i, start_col).style = "bc_pt_data"
                    Catch
                        style_error = style_error + " bc_pt_data"
                    End Try
                Next
            End If

            REM Steve Wooderson 18/01/2013 DA5
            REM Auto Chart Production
            If off.chart_data = True Then

                chart_active_sheet = application.ActiveWorkbook.ActiveSheet.name
                source_sheet_name = application.activesheet.name
                application.ActiveWorkbook.Worksheets.add()
                chart_sheet_name = application.activesheet.name

                If multiple_entity = True Then
                    REM Multiple_entity
                    REM entity names
                    For i = 0 To entity_formulas.Count - 1
                        If off.chart_focus = "Primary" Then
                            formula = "=" & source_sheet_name & "!" & CStr(ConvertColToLetter(entity_formulas(i).col_offset)) & entity_formulas(i).row_offset
                            application.application.activeworkbook.activesheet.cells(entity_formulas(i).row_offset - 9, entity_formulas(i).col_offset).formula = formula
                        Else
                            If off.clink.SelectedIndex > 0 Then
                                formula = "=" & source_sheet_name & "!" & CStr(ConvertColToLetter(entity_formulas(i).col_offset + 1)) & entity_formulas(i).row_offset
                                application.application.activeworkbook.activesheet.cells(entity_formulas(i).row_offset - 9, entity_formulas(i).col_offset).formula = formula
                            End If
                        End If
                    Next

                    rc = 1
                    For i = 0 To formulas.Count - 1

                        If formulas(i).col_offset = 3 Then
                            rc = 1
                        End If

                        If formulas(i).item_type = "e_a" And formulas(i).first = True Then
                            rc = rc + 1
                        End If

                        If formulas(i).year <> "" Then
                            application.application.activeworkbook.activesheet.cells(1, formulas(i).col_offset - rc) = formulas(i).year
                        End If

                        If (formulas(i).item_type = "item") And formulas(i).item_type <> "symbol" Then
                            application.application.activeworkbook.activesheet.cells(2, formulas(i).col_offset - rc) = formulas(i).item
                        End If

                        If formulas(i).item_type = "item" Then
                            formula = "=IF(ISERROR(VALUE(" & source_sheet_name & "!" & CStr(ConvertColToLetter(formulas(i).col_offset) & formulas(i).row_offset) & "))"
                            formula = formula + "," & source_sheet_name & "!" & CStr(ConvertColToLetter(formulas(i).col_offset) & formulas(i).row_offset)
                            formula = formula + ",VALUE(" & source_sheet_name & "!" & CStr(ConvertColToLetter(formulas(i).col_offset) & formulas(i).row_offset) & "))"
                            application.application.activeworkbook.activesheet.cells(formulas(i).row_offset - 9, formulas(i).col_offset - rc).formula = formula

                            If formulas(i).row_offset - 9 > maxrow Then
                                maxrow = formulas(i).row_offset - 9
                            End If
                            If formulas(i).col_offset - rc > maxcol Then
                                maxcol = formulas(i).col_offset - rc
                            End If
                        End If

                    Next

                Else
                    REM Single Entity
                    REM write out year
                    rc = 1
                    If off.Cyearstart.Enabled = True Then
                        rc = rc + 1
                        For i = CInt(off.Cyearstart.Text) To CInt(off.cyearend.Text)

                            For j = 0 To off.lperiods.Items.Count - 1
                                application.application.activeworkbook.activesheet.cells(1, rc) = i
                                Try
                                    application.application.activeworkbook.activesheet.cells(1, rc).style = "bc_pt_year"
                                Catch
                                    style_error = style_error + " bc_pt_year"
                                End Try
                                rc = rc + 1
                            Next j
                        Next
                    End If

                    REM set up chart data
                    For i = 0 To formulas.Count - 1

                        If formulas(i).item_type = "e_a" And formulas(i).first = True Then
                            yeartext = ""
                            yeartext = "=" & source_sheet_name & "!" & CStr(ConvertColToLetter(formulas(i).col_offset) & formulas(i).row_offset - 2)
                            yeartext = yeartext + " & " + source_sheet_name & "!" & CStr(ConvertColToLetter(formulas(i).col_offset) & formulas(i).row_offset)
                            application.application.activeworkbook.activesheet.cells(1, formulas(i).col_offset - 6).formula = yeartext
                        End If

                        If formulas(i).item_type = "item" Then

                            If formulas(i).first = True Then

                                formula = "=" & source_sheet_name & "!" & CStr(ConvertColToLetter(start_col + 3) & formulas(i).row_offset)
                                application.application.activeworkbook.activesheet.cells(formulas(i).row_offset - 8, 1).formula = formula

                                formula = "=IF(ISERROR(VALUE(" & source_sheet_name & "!" & CStr(ConvertColToLetter(formulas(i).col_offset) & formulas(i).row_offset) & "))"
                                formula = formula + "," & source_sheet_name & "!" & CStr(ConvertColToLetter(formulas(i).col_offset) & formulas(i).row_offset)
                                formula = formula + ",VALUE(" & source_sheet_name & "!" & CStr(ConvertColToLetter(formulas(i).col_offset) & formulas(i).row_offset) & "))"

                                application.application.activeworkbook.activesheet.cells(formulas(i).row_offset - 8, formulas(i).col_offset - 6).formula = formula
                            Else

                                formula = "=IF(ISERROR(VALUE(" & source_sheet_name & "!" & CStr(ConvertColToLetter(formulas(i).col_offset) & formulas(i).row_offset) & "))"
                                formula = formula + "," & source_sheet_name & "!" & CStr(ConvertColToLetter(formulas(i).col_offset) & formulas(i).row_offset)
                                formula = formula + ",VALUE(" & source_sheet_name & "!" & CStr(ConvertColToLetter(formulas(i).col_offset) & formulas(i).row_offset) & "))"

                                application.application.activeworkbook.activesheet.cells(formulas(i).row_offset - 8, formulas(i).col_offset - 6).formula = formula

                            End If

                            If formulas(i).row_offset - 8 > maxrow Then
                                maxrow = formulas(i).row_offset - 8
                            End If
                            If formulas(i).col_offset - 6 > maxcol Then
                                maxcol = formulas(i).col_offset - 6
                            End If
                        End If
                    Next
                End If

                REM Set the range object
                dataRange = "A1:" + CStr(ConvertColToLetter(maxcol) & maxrow)
                application.ActiveWorkbook.sheets(chart_active_sheet).select()
            End If


            'For i = 1 To application.Application.AddIns2.Count
            '    If application.Application.AddIns2(i).Name = "BC Excel Functions.xla" Then

            '        Try
            '            Try
            '                application.application.activeworkbook.VBProject.References.AddFromFile(application.Application.AddIns2(i).FullName)
            '            Catch

            '            End Try
            '            Application.DoEvents()
            '            application.Application.Run("ASPXLDBFunctions.ThisWorkbook.run_after_extract")
            '        Catch
            '            Dim ocomm As New bc_cs_message("Blue Curve", "Failed to find run_after_extract batch mode failed", bc_cs_message.MESSAGE, False, False, "Ok", "Cancel", True)
            '        End Try
            '            Exit For
            '    End If
            'Next

            'application.Application.Calculation = calc_state

        Catch ex As Exception
            MsgBox(ex.Message)

            output_to_excel = False
            off.WindowState = FormWindowState.Minimized
            Dim omsg As New bc_cs_message("Blue Curve", "Error building extract in Excel into current worksheet. Please try again using new worksheet option.", bc_cs_message.MESSAGE, False, False, "OK", "Cancel", True)
            Dim ocomm As New bc_cs_activity_log("ba_am_portfolio_wizard", "output_to_excel", bc_cs_activity_codes.COMMENTARY, ex.Message)
            off.rsheet.Checked = True
            off.tprogress.Abort()
            off.ppropress.Visible = False
            off.WindowState = FormWindowState.Normal

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            If style_error <> "" And output_to_excel = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "Warnings some styles do not exist Extract may not look as required. Please check style sheet.", bc_cs_message.MESSAGE, False, False, "OK", "Cancel", True)
                Dim ocomm As New bc_cs_activity_log("ba_am_portfolio_wizard", "output_to_excel", bc_cs_activity_codes.COMMENTARY, style_error)

            End If
            application.statusbar = False
            If output_to_excel = True Then
                application = Nothing
            End If
            off.Cursor = Cursors.Default
        End Try
    End Function

    Private Function ConvertColToLetter(ByVal icol As Integer) As String

        Dim iAlpha As Integer
        Dim iRemainder As Integer

        ConvertColToLetter = ""

        iAlpha = Int(icol / 27)
        iRemainder = icol - (iAlpha * 26)
        If iAlpha > 0 Then
            ConvertColToLetter = Chr(iAlpha + 64)
        End If
        If iRemainder > 0 Then
            ConvertColToLetter = ConvertColToLetter & Chr(iRemainder + 64)
        End If

    End Function

    Private Sub currency_convert(ByRef oform As formula, Optional ByVal linked_header_values As Boolean = True, Optional ByVal multiple_entity As Boolean = True, Optional ByVal time_series As Boolean = False)
        Dim fn As String
        Dim cfn As String
        REM get base formula
        If off.rbase.Checked = True Then

            If multiple_entity = True Then
                build_function_multiple(oform, linked_header_values)
            Else
                build_function_single(oform, linked_header_values, False, False, False, time_series, False)
            End If
            fn = oform.formula
            REM now make currency formula
            If multiple_entity = True Then
                cfn = build_function_multiple(oform, linked_header_values, True)
            Else
                cfn = build_function_single(oform, linked_header_values, True, False, False, time_series, False)
            End If
            fn = fn.Substring(1, fn.Length - 1)
            cfn = cfn.Replace("""VALUE""", fn)
            oform.formula = cfn
        Else

            If multiple_entity = True Then
                build_function_multiple_aggregation(oform, linked_header_values)
            Else
                build_function_single_aggregation(oform, linked_header_values, False, False, False, time_series, False)
            End If
            fn = oform.formula
            REM now make currency formula
            If multiple_entity = True Then
                cfn = build_function_multiple_aggregation(oform, linked_header_values, True)
            Else
                cfn = build_function_single_aggregation(oform, linked_header_values, True, False, False, time_series, False)
            End If
            fn = fn.Substring(1, fn.Length - 1)
            cfn = cfn.Replace("""VALUE""", fn)
            oform.formula = cfn
        End If

    End Sub
    Public Function is_formula(ByVal fm As String) As Boolean
        Try
            Dim i As Integer
            Dim now_numeric As Boolean = False
            is_formula = False
            REM if has a $ it is a formula
            If fm.Length < 2 Then
                Exit Function
            End If
            Dim k As Integer
            If InStr(fm, "$") > 0 Then
                is_formula = True
                Exit Function
            End If
            REM if has alpha chracter followed by numeric it is as well
            REM first character must be alpha
            If (Asc(fm.Substring(0, 1)) >= Asc("a") And Asc(fm.Substring(0, 1)) <= Asc("z")) Or (Asc(fm.Substring(0, 1)) >= Asc("A") And Asc(fm.Substring(0, 1)) <= Asc("Z")) Then
                For i = 1 To fm.Length - 1
                    If (Asc(fm.Substring(i, 1)) >= Asc("a") And Asc(fm.Substring(i, 1)) <= Asc("z")) Or (Asc(fm.Substring(i, 1)) >= Asc("A") And Asc(fm.Substring(i, 1)) <= Asc("Z")) Then
                        k = 4
                        If now_numeric = True Then
                            Exit Function
                        End If
                    ElseIf IsNumeric(fm.Substring(i, 1)) = True Then
                        REM must now be numeric 
                        now_numeric = True
                        If i = fm.Length - 1 Then
                            is_formula = True
                            Exit Function
                        End If
                    Else
                        Exit Function
                    End If
                Next
            End If
            If off.lselitem.Items.Count = 1 Then
                off.titemset.Enabled = False
                off.titemset.Text = ""

            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ef_wizard", "is_formula", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function

    Private Function build_function_multiple(ByRef oform As formula, Optional ByVal linked_header_values As Boolean = True, Optional ByVal currency_conv As Boolean = False, Optional ByVal item_currency As Boolean = False, Optional ByVal e_a As Boolean = False) As String
        Dim i, j As Integer
        Dim fm As String
        Dim dims As String
        Dim ti As String
        Dim sti As String
        Dim eti As String
        eti = ""
        sti = ""
        fm = ""
        dims = ""
        For i = 0 To bc_am_ef_functions.excel_functions.Count - 1
            If (bc_am_ef_functions.excel_functions(i).display_name = off.cfunction.Text And currency_conv = False) Or (currency_conv = True And bc_am_ef_functions.excel_functions(i).display_name = "currency convert" And item_currency = False) Or (item_currency = True And bc_am_ef_functions.excel_functions(i).display_name = "currency convert" And currency_conv = True) Then
                fm = "=" + bc_am_ef_functions.excel_functions(i).name + "("
                For j = 0 To bc_am_ef_functions.excel_functions(i).params.count - 1
                    ti = ""
                    sti = ""
                    eti = ""

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "context" Then
                        ti = off.ccontext.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "section" Then
                        ti = off.csection.Text
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "class" Then
                        If oform.assoc_entity = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 1, oform.formula_entity.col_offset, True, True)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 1, oform.formula_entity.col_offset + 1, True, True)
                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "entity" Then
                        If oform.assoc_entity = False Then
                            ti = excel_address(oform.row_offset, oform.formula_entity.col_offset, False, True)
                        Else
                            ti = excel_address(oform.row_offset, oform.formula_entity.col_offset + 1, False, True)
                        End If
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "item" Then
                        If item_currency = False And e_a = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 5, oform.col_offset, True, False)
                        ElseIf item_currency = False Or e_a = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 5, oform.col_offset - 1, True, False)
                        ElseIf item_currency = True And e_a = True Then

                            ti = excel_address(oform.formula_entity.header.item_row_offset + 5, oform.col_offset - 2, True, False)
                        End If
                    End If
                    REM cusbstiute value
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "gen1" Then
                        If currency_conv = True And item_currency = False Then
                            ti = "VALUE"
                        ElseIf item_currency = True And e_a = False Then

                            ti = excel_address(oform.formula_entity.row_offset, oform.col_offset - 1)
                        Else
                            ti = excel_address(oform.formula_entity.row_offset, oform.col_offset - 2)


                        End If
                    End If
                    REM currency code convert
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "gen2" Then
                        If currency_conv = True And item_currency = False Then
                            If linked_header_values = True Then
                                ti = excel_address(oform.formula_entity.header.row_offset + 3, oform.formula_entity.col_offset + 1)
                            Else
                                ti = oform.formula_entity.header.currency_code
                            End If
                        Else
                            REM item currency
                            REM just use a randon one
                            ti = off.ccurr.Items(0)
                        End If
                    End If


                    If bc_am_ef_functions.excel_functions(i).params(j).name = "contributor" Then
                        If linked_header_values = True Then
                            ti = excel_address(oform.formula_entity.header.row_offset + 1, oform.formula_entity.col_offset + 1, True, True)
                        Else
                            ti = oform.formula_entity.header.contributor
                        End If

                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "stage" Then
                        If linked_header_values = True Then
                            ti = excel_address(oform.formula_entity.header.row_offset, oform.formula_entity.col_offset + 1, True, True)
                        Else
                            ti = oform.formula_entity.header.stage
                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "year" Then
                        If e_a = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 3, oform.col_offset, True, False)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 3, oform.col_offset - 1, True, False)

                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "period" Then
                        If e_a = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 4, oform.col_offset, True, False)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 4, oform.col_offset - 1, True, False)

                        End If
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "date_at" Then
                        If linked_header_values = True Then
                            ti = excel_address(oform.formula_entity.header.row_offset + 2, oform.formula_entity.col_offset + 1, True, True)
                        Else
                            ti = oform.formula_entity.header.date_at
                        End If
                    End If
                    'If bc_am_ef_functions.excel_functions(i).params(j).name = "date_from" Then
                    '    If off.chkdate_from.Checked = True Then
                    '        ti = "1-1-1900"
                    '    Else
                    '        ti = CStr(off.tdate_from.Text)
                    '    End If
                    'End If
                    If sti <> "" Then
                        fm = fm + sti
                    Else
                        If is_formula(ti) = False Then
                            If ti = "" Then
                                fm = fm + ","
                            Else
                                fm = fm + """" + ti + ""","
                            End If
                        Else
                            If ti = "" Then
                                fm = fm + ","
                            Else
                                fm = fm + ti + ","
                            End If
                        End If
                    End If
                Next
                '        Exit For
            End If
        Next

        fm = fm + ")"
        oform.formula = fm
        build_function_multiple = fm
    End Function
    Private Function build_function_single(ByRef oform As formula, Optional ByVal linked_header_values As Boolean = True, Optional ByVal currency_conv As Boolean = False, Optional ByVal item_currency As Boolean = False, Optional ByVal e_a As Boolean = False, Optional ByVal time_series As Boolean = False, Optional ByVal time_series_first As Boolean = False) As String
        Dim i, j As Integer
        Dim fm As String
        Dim dims As String
        Dim ti As String
        Dim sti As String
        Dim eti As String
        eti = ""
        sti = ""
        fm = ""
        dims = ""
        For i = 0 To bc_am_ef_functions.excel_functions.Count - 1
            If (bc_am_ef_functions.excel_functions(i).display_name = off.cfunction.Text And currency_conv = False) Or (currency_conv = True And bc_am_ef_functions.excel_functions(i).display_name = "currency convert" And item_currency = False) Or (item_currency = True And bc_am_ef_functions.excel_functions(i).display_name = "currency convert" And currency_conv = True) Then
                fm = "=" + bc_am_ef_functions.excel_functions(i).name + "("
                For j = 0 To bc_am_ef_functions.excel_functions(i).params.count - 1
                    ti = ""

                    sti = ""
                    eti = ""

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "context" Then
                        ti = off.ccontext.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "section" Then
                        ti = off.csection.Text
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "class" Then
                        If oform.assoc_entity = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 1, oform.formula_entity.col_offset, True, True)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.formula_entity.col_offset, True, True)
                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "entity" Then
                        If oform.assoc_entity = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 1, oform.formula_entity.col_offset + 1, True, True)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.formula_entity.col_offset + 1, True, True)
                        End If
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "item" Then
                        If time_series = False Then
                            If e_a = False Or item_currency = True Then
                                ti = excel_address(oform.row_offset, oform.formula_entity.col_offset + 3, False, True)
                            Else
                                ti = excel_address(oform.formula_entity.row_offset, oform.formula_entity.col_offset + 3)
                            End If
                        Else
                            If time_series_first = True Then
                                ti = excel_address(oform.row_offset - 5, oform.formula_entity.col_offset + 3)
                            Else
                                If item_currency = False Then
                                    ti = excel_address(oform.formula_entity.row_offset - 7, oform.col_offset)
                                Else
                                    ti = excel_address(oform.formula_entity.row_offset - 6, oform.col_offset)
                                End If
                            End If
                        End If
                    End If
                    REM cusbstiute value
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "gen1" Then
                        If currency_conv = True And item_currency = False Then
                            ti = "VALUE"
                        Else
                            If time_series = False Then
                                ti = excel_address(oform.row_offset, oform.formula_entity.header.col_offset + 4)
                            Else
                                ti = excel_address(oform.row_offset + 1, oform.col_offset)
                            End If
                        End If
                    End If
                    REM currency code convert
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "gen2" Then
                        If currency_conv = True And item_currency = False Then
                            ti = excel_address(oform.formula_entity.header.row_offset + 3, oform.formula_entity.col_offset + 1)
                        Else
                            REM item currency
                            REM just use a randon one
                            ti = off.ccurr.Items(0)
                        End If
                    End If


                    If bc_am_ef_functions.excel_functions(i).params(j).name = "contributor" Then
                        ti = excel_address(oform.formula_entity.header.row_offset + 1, oform.formula_entity.col_offset + 1, True, True)
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "stage" Then
                        ti = excel_address(oform.formula_entity.header.row_offset, oform.formula_entity.col_offset + 1, True, True)
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "year" Then
                        If time_series = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.col_offset, True, False)
                        Else

                            ti = excel_address(oform.formula_entity.header.item_row_offset + 1, oform.col_offset)
                        End If
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "period" Then
                        If time_series = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 3, oform.col_offset, True, False)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.col_offset)
                        End If
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "date_at" Then
                        If time_series = False Then
                            ti = excel_address(oform.formula_entity.header.row_offset + 2, oform.formula_entity.col_offset + 1, True, True)
                        Else
                            If time_series_first = True Then
                                ti = excel_address(oform.formula_entity.header.row_offset + 2, oform.formula_entity.col_offset + 2)
                            Else
                                If bc_am_ef_functions.excel_functions(i).display_name = "currency convert" Then
                                    REM always currency convert at current exhange rate for time series
                                    ti = "9-9-9999"
                                Else
                                    ti = excel_address(oform.row_offset, oform.formula_entity.col_offset)
                                End If
                            End If
                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "date_from" Then
                        ti = excel_address(oform.formula_entity.header.row_offset + 2, oform.formula_entity.col_offset + 1)
                    End If
                    If sti <> "" Then
                        fm = fm + sti
                    Else
                        If is_formula(ti) = False Then
                            If ti = "" Then
                                fm = fm + ","
                            Else
                                fm = fm + """" + ti + ""","
                            End If
                        Else
                            If ti = "" Then
                                fm = fm + ","
                            Else
                                fm = fm + ti + ","
                            End If
                        End If
                    End If
                Next
                '        Exit For
            End If
        Next

        fm = fm + ")"
        oform.formula = fm
        build_function_single = fm
    End Function
    Private Function build_function_single_aggregation(ByRef oform As formula, Optional ByVal linked_header_values As Boolean = True, Optional ByVal currency_conv As Boolean = False, Optional ByVal item_currency As Boolean = False, Optional ByVal e_a As Boolean = False, Optional ByVal time_series As Boolean = False, Optional ByVal time_series_first As Boolean = False) As String
        Dim i, j As Integer
        Dim fm As String
        Dim dims As String
        Dim ti As String
        Dim sti As String
        Dim eti As String
        eti = ""
        sti = ""
        fm = ""
        dims = ""

        For i = 0 To bc_am_ef_functions.excel_functions.Count - 1
            If (bc_am_ef_functions.excel_functions(i).display_name = "get aggregated financial data" And currency_conv = False) Or (currency_conv = True And bc_am_ef_functions.excel_functions(i).display_name = "currency convert" And item_currency = False) Or (item_currency = True And bc_am_ef_functions.excel_functions(i).display_name = "currency convert" And currency_conv = True) Then
                fm = "=" + bc_am_ef_functions.excel_functions(i).name + "("
                For j = 0 To bc_am_ef_functions.excel_functions(i).params.count - 1
                    ti = ""

                    sti = ""
                    eti = ""

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "context" Then
                        ti = off.ccontext.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "section" Then
                        ti = off.csection.Text
                    End If
                    REM dual entities
                    If oform.assoc_entity = True Then

                        If bc_am_ef_functions.excel_functions(i).params(j).name = "lclass" Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 3, oform.formula_entity.col_offset, True, True)
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "lentity" Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 3, oform.formula_entity.col_offset + 1, True, True)
                        End If
                    End If
                    REM ===



                    If bc_am_ef_functions.excel_functions(i).params(j).name = "class" Then
                        If oform.assoc_entity = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.formula_entity.col_offset, True, True)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.formula_entity.col_offset, True, True)
                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "entity" Then
                        If oform.assoc_entity = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.formula_entity.col_offset + 1, True, True)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.formula_entity.col_offset + 1, True, True)
                        End If
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "item" Then
                        If time_series = False Then
                            If e_a = False Or item_currency = True Then
                                ti = excel_address(oform.row_offset, oform.formula_entity.col_offset + 3, False, True)
                            Else
                                ti = excel_address(oform.formula_entity.row_offset, oform.formula_entity.col_offset + 3)
                            End If
                        Else
                            If time_series_first = True Then
                                ti = excel_address(oform.row_offset - 5, oform.formula_entity.col_offset + 3)
                            Else
                                If item_currency = False Then
                                    ti = excel_address(oform.formula_entity.row_offset - 7, oform.col_offset)
                                Else
                                    ti = excel_address(oform.formula_entity.row_offset - 6, oform.col_offset)
                                End If
                            End If
                        End If
                    End If
                    REM cusbstiute value
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "gen1" Then
                        If currency_conv = True And item_currency = False Then
                            ti = "VALUE"
                        Else
                            If time_series = False Then
                                ti = excel_address(5, 2)
                            Else
                                ti = excel_address(5, 2)
                            End If
                        End If
                    End If
                    REM aggregation universe
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "gen2" Then
                        ti = excel_address(6, 2)

                    End If


                    If bc_am_ef_functions.excel_functions(i).params(j).name = "contributor" Then
                        ti = excel_address(oform.formula_entity.header.row_offset + 1, oform.formula_entity.col_offset + 1, True, True)
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "stage" Then
                        ti = excel_address(oform.formula_entity.header.row_offset, oform.formula_entity.col_offset + 1, True, True)
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "year" Then
                        If time_series = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.col_offset, True, False)
                        Else

                            ti = excel_address(oform.formula_entity.header.item_row_offset + 1, oform.col_offset)
                        End If
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "period" Then
                        If time_series = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 3, oform.col_offset, True, False)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.col_offset)
                        End If
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "date_at" Then
                        If time_series = False Then
                            ti = excel_address(oform.formula_entity.header.row_offset + 2, oform.formula_entity.col_offset + 1, True, True)
                        Else
                            If time_series_first = True Then
                                ti = excel_address(oform.formula_entity.header.row_offset + 2, oform.formula_entity.col_offset + 2)
                            Else
                                If bc_am_ef_functions.excel_functions(i).display_name = "currency convert" Then
                                    REM always currency convert at current exhange rate for time series
                                    ti = "9-9-9999"
                                Else
                                    ti = excel_address(oform.row_offset, oform.formula_entity.col_offset)
                                End If
                            End If
                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "date_from" Then
                        ti = excel_address(oform.formula_entity.header.row_offset + 2, oform.formula_entity.col_offset + 1)
                    End If
                    If sti <> "" Then
                        fm = fm + sti
                    Else
                        If is_formula(ti) = False Then
                            If ti = "" Then
                                fm = fm + ","
                            Else
                                fm = fm + """" + ti + ""","
                            End If
                        Else
                            If ti = "" Then
                                fm = fm + ","
                            Else
                                fm = fm + ti + ","
                            End If
                        End If
                    End If
                Next
                '        Exit For
            End If
        Next

        fm = fm + ")"
        oform.formula = fm
        build_function_single_aggregation = fm
    End Function

    Private Function build_function_multiple_aggregation(ByRef oform As formula, Optional ByVal linked_header_values As Boolean = True, Optional ByVal currency_conv As Boolean = False, Optional ByVal item_currency As Boolean = False, Optional ByVal e_a As Boolean = False) As String
        Dim i, j As Integer
        Dim fm As String
        Dim dims As String
        Dim ti As String
        Dim sti As String
        Dim eti As String
        eti = ""
        sti = ""
        fm = ""
        dims = ""
        For i = 0 To bc_am_ef_functions.excel_functions.Count - 1
            If (bc_am_ef_functions.excel_functions(i).display_name = "get aggregated financial data" And currency_conv = False) Or (currency_conv = True And bc_am_ef_functions.excel_functions(i).display_name = "currency convert" And item_currency = False) Or (item_currency = True And bc_am_ef_functions.excel_functions(i).display_name = "currency convert" And currency_conv = True) Then
                fm = "=" + bc_am_ef_functions.excel_functions(i).name + "("
                For j = 0 To bc_am_ef_functions.excel_functions(i).params.count - 1
                    ti = ""
                    sti = ""
                    eti = ""

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "context" Then
                        ti = off.ccontext.Text
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "section" Then
                        ti = off.csection.Text
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "class" Then
                        If oform.assoc_entity = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.formula_entity.col_offset, True, True)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.formula_entity.col_offset, True, True)
                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "entity" Then
                        If oform.assoc_entity = False Then
                            ti = excel_address(oform.row_offset, oform.formula_entity.col_offset, False, True)
                        Else
                            ti = excel_address(oform.row_offset, oform.formula_entity.col_offset, False, True)
                        End If
                    End If

                    REM dual entities
                    If oform.assoc_entity = True Then

                        If bc_am_ef_functions.excel_functions(i).params(j).name = "lclass" Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 2, oform.formula_entity.col_offset + 1, True, True)
                        End If
                        If bc_am_ef_functions.excel_functions(i).params(j).name = "lentity" Then
                            ti = excel_address(oform.row_offset, oform.formula_entity.col_offset + 1, True, True)
                        End If
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "item" Then
                        If item_currency = False And e_a = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 5, oform.col_offset, True, False)
                        ElseIf item_currency = False Or e_a = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 5, oform.col_offset - 1, True, False)
                        ElseIf item_currency = True And e_a = True Then

                            ti = excel_address(oform.formula_entity.header.item_row_offset + 5, oform.col_offset - 2, True, False)
                        End If
                    End If
                    REM cusbstiute value
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "gen1" Then
                        If currency_conv = True And item_currency = False Then
                            ti = "VALUE"
                        ElseIf item_currency = True And e_a = False Then

                            ti = excel_address(5, 2)
                        Else
                            ti = excel_address(5, 2)


                        End If
                    End If
                    REM currency code convert
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "gen2" Then
                        ti = excel_address(6, 2)

                    End If


                    If bc_am_ef_functions.excel_functions(i).params(j).name = "contributor" Then
                        If linked_header_values = True Then
                            ti = excel_address(oform.formula_entity.header.row_offset + 1, oform.formula_entity.col_offset + 1, True, True)
                        Else
                            ti = oform.formula_entity.header.contributor
                        End If

                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "stage" Then
                        If linked_header_values = True Then
                            ti = excel_address(oform.formula_entity.header.row_offset, oform.formula_entity.col_offset + 1, True, True)
                        Else
                            ti = oform.formula_entity.header.stage
                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "year" Then
                        If e_a = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 3, oform.col_offset, True, False)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 3, oform.col_offset - 1, True, False)

                        End If
                    End If
                    If bc_am_ef_functions.excel_functions(i).params(j).name = "period" Then
                        If e_a = False Then
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 4, oform.col_offset, True, False)
                        Else
                            ti = excel_address(oform.formula_entity.header.item_row_offset + 4, oform.col_offset - 1, True, False)

                        End If
                    End If

                    If bc_am_ef_functions.excel_functions(i).params(j).name = "date_at" Then
                        If linked_header_values = True Then
                            ti = excel_address(oform.formula_entity.header.row_offset + 2, oform.formula_entity.col_offset + 1, True, True)
                        Else
                            ti = oform.formula_entity.header.date_at
                        End If
                    End If
                    'If bc_am_ef_functions.excel_functions(i).params(j).name = "date_from" Then
                    '    If off.chkdate_from.Checked = True Then
                    '        ti = "1-1-1900"
                    '    Else
                    '        ti = CStr(off.tdate_from.Text)
                    '    End If
                    'End If
                    If sti <> "" Then
                        fm = fm + sti
                    Else
                        If is_formula(ti) = False Then
                            If ti = "" Then
                                fm = fm + ","
                            Else
                                fm = fm + """" + ti + ""","
                            End If
                        Else
                            If ti = "" Then
                                fm = fm + ","
                            Else
                                fm = fm + ti + ","
                            End If
                        End If
                    End If
                Next
                '        Exit For
            End If
        Next

        fm = fm + ")"
        oform.formula = fm
        build_function_multiple_aggregation = fm
    End Function
End Class
'Public Class API
'    Public Const SWP_NOMOVE As Integer = 2
'    Public Const SWP_NOSIZE As Integer = 1
'    Public Const HWND_TOPMOST As Integer = -1
'    Public Const HWND_NOTOPMOST As Integer = -2

'    <DllImport("user32")> Public Shared Function _
'        SetWindowPos(ByVal hwnd As Integer, _
'        ByVal hWndInsertAfter As Integer, _
'        ByVal x As Integer, _
'        ByVal y As Integer, _
'        ByVal cy As Long, _
'        ByVal cx As Integer, _
'       ByVal wFlags As Integer) As Integer
'    End Function
'End Class


