Imports System
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Collections
Imports Microsoft.Win32
Imports Microsoft.VisualBasic
Imports System.Threading
Imports System.Diagnostics
Imports BlueCurve.Insight.AM
Imports BlueCurve.core.OM
Imports BlueCurve.core.CS
Imports BlueCurve.core.AS
REM ==========================================
REM Bluecurve Limited 2005
REM Module:       BC Insight Active X API
REM Type:         Application Object
REM Description:  Class that gets converted
REM               via TlbEXP to provide Active X api
REM               into 4.6i framework
REM Version:      1
REM Change history
REM ==========================================

Public Class bc_ax_excel_functions
    Public bc_am_excel_functions As bc_am_excel_functions
    Public application As Object
    Private authenticated As Boolean = False
    Public Shared auto_ready = True

    Public Sub New()

    End Sub
    Public Sub load_rtd_function_wizard(ByVal excelapp As Object)
        Try
            If excelapp.activecell Is Nothing Then
                Dim omsg As New bc_cs_message("Blue Curve", "No cell selected!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Else
                Dim bcs As New bc_cs_central_settings(True)
                Dim bc_load As New bc_am_load("Excel Functions", True)

                Dim f As New bc_dx_rdp_function_wizard
                Dim c As New Cbc_dx_rdp_function_wizard
                If c.load_data(f, excelapp, True) = True Then
                    f.TopMost = True
                    f.Show()
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_rtd_excel_functions", "load_rtd_function_wizard", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub sign_out()
        Dim cl As New Cbc_dx_full_logon
        cl.remove_credentials_file()
    End Sub
    Public Sub load_batch_function_wizard(ByVal excelapp As Object)
        Try
            If excelapp.activecell Is Nothing Then
                Dim omsg As New bc_cs_message("Blue Curve", "No cell selected!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Else
                Dim bcs As New bc_cs_central_settings(True)
                Dim bc_load As New bc_am_load("Excel Functions", True)

                Dim f As New bc_dx_rdp_function_wizard
                Dim c As New Cbc_dx_rdp_function_wizard
                If c.load_data(f, excelapp, False) = True Then
                    f.TopMost = True
                    f.Show()
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_rtd_excel_functions", "load_rtd_function_wizard", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Public Sub connect()
        Dim slog As New bc_cs_activity_log("bc_ax_excel_functions", "bc_ax_api_build_sheets", CStr(CStr(bc_cs_activity_codes.TRACE_ENTRY)), "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            'ensure any other progress bar is unloaded before Excel Functions connect
            bc_cs_central_settings.progress_bar.unload()
            Dim bc_am_load As New bc_am_load("Excel Functions", True)

            If bc_am_load.bdataloaded = False Then
                authenticated = False
                Exit Sub
            Else
                authenticated = True
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                Dim omsg As New bc_cs_message("Blue Curve", "Aithentication failed for User", bc_cs_message.MESSAGE, False, False, "Ok", "Cancel", True)
                Exit Sub
            End If
            bc_am_excel_functions = New bc_am_excel_functions

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_excel_functions", "connect", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_excel_functions", "connect", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    Public Function get_connection_parameter(ByVal oparam As String) As String
        get_connection_parameter = bc_am_excel_functions.get_connection_parameters(oparam)
    End Function
    Public Function get_batch_mode() As Integer

        get_batch_mode = bc_am_excel_functions.get_batch_mode()
    End Function
    Public Function get_auto_ready() As Integer

        get_auto_ready = bc_am_excel_functions.auto_ready
    End Function
    Public Function execute_batch_mode_changes(ByVal excelapp As Object, ByVal calc_mode As Integer) As Boolean

        execute_batch_mode_changes = bc_am_excel_functions.execute_batch_mode_changes(excelapp, calc_mode)

    End Function
    Public Function execute_all(ByVal excelapp As Object) As Boolean
        execute_all = bc_am_excel_functions.execute_all(excelapp)


    End Function
    Public Sub set_batch_mode(ByVal batch_mode As Integer)
        bc_am_excel_functions.set_batch_mode(batch_mode)
    End Sub

    Public Function execute_selection(ByVal excelapp As Object, ByVal calc_mode As Integer) As Boolean
        execute_selection = bc_am_excel_functions.execute_selection(excelapp, calc_mode)
    End Function
    Public Function execute_function(ByVal excelapp As Object, ByVal str As String) As String
        If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
            execute_function = bc_am_excel_functions.execute_function(excelapp, str)
        Else
            execute_function = "connection failed"
        End If
    End Function
    Public Sub initialise_batch_mode()
        If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
            bc_am_excel_functions.initialise_batch_mode()
        End If
    End Sub

    Public Sub clear_batch_mode()
        If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
            bc_am_excel_functions.clear_batch_mode()
        End If
    End Sub

    Public Function execute(ByVal sql As String) As Object
        Try

            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            bc_am_excel_functions = New bc_am_excel_functions
            execute = bc_am_excel_functions.execute(sql)
        Catch ex As Exception
            MsgBox(ex.Message)
            execute = Nothing
        Finally
        End Try
    End Function
    Public Sub load_function_wizard(ByVal application As Object, Optional ByVal function_name As String = "")
        Try
            If application.activecell Is Nothing Then
                Throw New ApplicationException
            End If
            'Dim bcs As New bc_cs_central_settings(True)
            Dim ohelp As New bc_am_ef_functions
            ohelp.application = application
            ohelp.load(False, function_name)

        Catch ex As Exception
            Dim sMessage As String

            If application.activecell Is Nothing Then
                sMessage = "A WorkSheet should be open"
            Else
                sMessage = ex.Message
            End If
            Dim oerr As New bc_cs_error_log("bc_ax_excel_functions", "load_help", bc_cs_error_codes.USER_DEFINED, sMessage)

        End Try
    End Sub

    Public Function bc_ax_api_refresh_pd_extract(ByVal application As Object) As Long
        Dim slog As New bc_cs_activity_log("bc_ax_excel_functions", "bc_ax_api_refresh_pd_extract", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Dim oef As New bc_am_ef_functions


            oef.application = application
            oef.refresh_pd_extract()
            bc_ax_api_refresh_pd_extract = oef.portfolio_Id

        Catch ex As Exception
            bc_ax_api_refresh_pd_extract = 0
            Dim oerr As New bc_cs_error_log("bc_ax_excel_functionsi", "bc_ax_api_refresh_pd_extract", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            slog = New bc_cs_activity_log("bc_ax_excel_functions", "bc_ax_api_refresh_pd_extract", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Function
    Public Function load_extract_wizard(ByVal application As Object, Optional ByRef start_row As Integer = 1, Optional ByRef start_col As Integer = 1, Optional ByRef end_row As Integer = 1, Optional ByRef end_col As Integer = 1, Optional ByRef chart_data As Boolean = False, Optional ByRef chart_method_call As String = "", Optional ByRef chart_range As String = "", Optional ByRef chart_passRangeAsParam As Boolean = False, Optional ByRef chart_sheet_name As String = "") As Long
        Try
            load_extract_wizard = 0
            If application.activecell Is Nothing Then
                Throw New ApplicationException
            End If

            bc_am_excel_functions.bulk_write_to_excel = True

            'Dim bcs As New bc_cs_central_settings(True)
            Dim oef As New bc_am_ef_functions
            oef.application = application
            oef.load(True)

            start_row = oef.start_row
            start_col = oef.start_col
            end_row = oef.end_row
            end_col = oef.end_col

            chart_method_call = oef.chart_method
            chart_passRangeAsParam = oef.chart_passrangeasparam
            chart_data = oef.chart_data
            chart_range = oef.dataRange
            chart_sheet_name = oef.chart_sheet_name
            load_extract_wizard = oef.portfolio_Id

        Catch ex As Exception
            Dim sMessage As String

            If application.activecell Is Nothing Then
                Dim omessage As bc_cs_message
                omessage = New bc_cs_message("Blue Curve insight", "Workbook must be Open", bc_cs_message.MESSAGE)
            Else
                sMessage = ex.Message
                Dim oerr As New bc_cs_error_log("bc_ax_excel_functions", "load_help", bc_cs_error_codes.USER_DEFINED, sMessage)
            End If
        Finally
            bc_am_excel_functions.bulk_write_to_excel = False
        End Try
    End Function

    Public Sub after_format(ByVal application As Object, ByVal singlecell As Object, ByVal activesheet As Object, ByVal activecell As Object, ByVal str As String, ByVal dimension As String, Optional ByVal multiple As Boolean = True, Optional ByVal batch As Boolean = False, Optional ByVal function_count As Integer = 1, Optional ByVal datatype As Integer = 1)
        Dim oafter_fomat As New bc_am_ef_after_format(application, singlecell, activesheet, activecell, str, dimension, multiple, batch, function_count, datatype)
    End Sub

    Public Function bc_format(ByVal value As String, Optional ByVal format_type As String = "decimal places", Optional ByVal places As String = "2", Optional ByVal scale_factor As String = "1", Optional ByVal item As String = "") As String
        bc_format = bc_am_excel_functions.bc_format(value, format_type, places, scale_factor, item)
    End Function

    Public Property isauthenticated() As Boolean
        Get
            Return authenticated
        End Get
        Set(ByVal value As Boolean)
            authenticated = value
        End Set

    End Property

End Class
Public Class bc_ax_insight_api
    REM class to contain all methods Active-X can invoke
    REM these call into lower layers of the framework.
    Public io_errors As New bc_om_excel_data_element_errors
    Public Sub New()

    End Sub

    Public Sub bc_ax_api_insert_intermediate_sheets(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_insert_intermediate_sheets", CStr(CStr(bc_cs_activity_codes.TRACE_ENTRY)), "")

        Try
            bc_am_excel_functions.bulk_write_to_excel = True
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("Insight")
            Dim bc_am_insight_parameters As New bc_am_insight_parameters
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim oi_sheets As New bc_am_in_insert_intermediate_sheets(show_form, ao_object, ao_type)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", "bc_ax_api_build_sheets", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_am_excel_functions.bulk_write_to_excel = False
            slog = New bc_cs_activity_log("bc_ax_insight_api", "bc_ax_api_build_sheets", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Sub
    Public Function bc_ax_api_excel_io(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal template_name As String = "", Optional ByVal batch_mode As Boolean = False, Optional ByVal first_batch As Boolean = False, Optional ByVal calc As Boolean = False) As Boolean
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_excel_io", CStr(CStr(bc_cs_activity_codes.TRACE_ENTRY)), "")

        Try
            bc_ax_api_excel_io = False
            If batch_mode = False Or (batch_mode = True And first_batch = True) Then
                io_errors.errors.Clear()
                Dim bc_cs_central_settings As New bc_cs_central_settings(True)
                Dim bc_am_load As New bc_am_load("Insight")
                Dim bc_am_insight_parameters As New bc_am_insight_parameters
                If (bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False) Then
                    Dim oio As New bc_am_excel_data_io(ao_object, ao_type)
                    If show_form = True Then
                        oio.show_template_list(io_errors)
                        bc_ax_api_excel_io = True
                    Else
                        bc_ax_api_excel_io = oio.run_template(template_name, io_errors, batch_mode, calc)
                    End If
                Else
                    bc_ax_api_excel_io = False
                End If
            Else
                Dim oio As New bc_am_excel_data_io(ao_object, ao_type)
                bc_ax_api_excel_io = oio.run_template(template_name, io_errors, batch_mode, calc)
            End If
        Catch ex As Exception
            bc_ax_api_excel_io = False
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", "bc_ax_api_excel_io", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_insight_api", "bc_ax_api_excel_io", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Function
    Public Function bc_ax_api_show_excel_io_errors(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String) As Boolean
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", " bc_ax_api_show_excel_io_errors", CStr(CStr(bc_cs_activity_codes.TRACE_ENTRY)), "")

        Try
            Dim oio As New bc_am_excel_data_io(ao_object, ao_type)
            oio.show_errors(Me.io_errors, False)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", " bc_ax_api_show_excel_io_errors", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_insight_api", " bc_ax_api_show_excel_io_errors", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Function
    Public Sub bc_ax_api_build_sheets(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal link_now As Boolean)
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_build_sheets", CStr(CStr(bc_cs_activity_codes.TRACE_ENTRY)), "")

        Try
            bc_am_excel_functions.bulk_write_to_excel = True

            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("Insight")
            Dim bc_am_insight_parameters As New bc_am_insight_parameters
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim obuild_sheets As New bc_am_in_build(True, ao_object, ao_type, link_now)
            End If

            Try
                ao_object.bc_after_build()
            Catch
                slog = New bc_cs_activity_log("bc_ax_insight_api", "bc_ax_api_build_sheets", CStr(bc_cs_activity_codes.COMMENTARY), "no bc_after_build")
            End Try

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", "bc_ax_api_build_sheets", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_am_excel_functions.bulk_write_to_excel = False
            slog = New bc_cs_activity_log("bc_ax_insight_api", "bc_ax_api_build_sheets", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Sub
    Public Sub bc_ax_api_calculations(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_calculations", CStr(CStr(bc_cs_activity_codes.TRACE_ENTRY)), "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("Insight")
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
                    Dim ocalcs As New bc_am_calculations
                    ocalcs.ShowDialog()
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", "bc_ax_api_calculations", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_insight_api", "bc_ax_api_calculations", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Sub
    Public Sub bc_ax_api_link(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_link", CStr(CStr(bc_cs_activity_codes.TRACE_ENTRY)), "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("Insight")
            Dim bc_am_insight_parameters As New bc_am_insight_parameters
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim olink As New bc_am_in_link_only(True, ao_object, ao_type)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", "bc_ax_api_link", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_insight_api", "bc_ax_api_link", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Sub
    REM self heal call
    Public Sub bc_ax_api_rebuild(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal link_now As Boolean)
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_rebuild", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            bc_am_excel_functions.bulk_write_to_excel = True
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("Insight")
            Dim bc_am_insight_parameters As New bc_am_insight_parameters
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim orebuild_sheets As New bc_am_in_rebuild(False, ao_object, ao_type, link_now)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", "bc_ax_api_rebuild", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_am_excel_functions.bulk_write_to_excel = False
            slog = New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_rebuild", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Sub
    REM FIL AUGUST 2012
    Public Sub bc_ax_api_clone(ByVal ao_object As Object, ByVal ao_type As String)
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", " bc_ax_api_clone", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            bc_am_excel_functions.bulk_write_to_excel = True
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("Insight")
            Dim bc_am_insight_parameters As New bc_am_insight_parameters
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim oclone_workbook As New bc_am_in_clone(ao_object, ao_type)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", "bc_ax_api_clone", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_am_excel_functions.bulk_write_to_excel = False
            slog = New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_clone", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Sub
    REM AUTO AUG 2013



    Public Function bc_ax_api_upload_model(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal pub_type_id As Long, ByVal title As String) As Boolean
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_submit", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("Insight")
            Dim bc_am_insight_parameters As New bc_am_insight_parameters
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim osubmit_model As New bc_am_in_submit_model
                osubmit_model.submit_model(show_form, ao_object, ao_type, pub_type_id, title)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", "bc_ax_api_submit", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax__insight_api", "bc_ax_api_submit", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Function

    Public Function bc_ax_api_submit(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal validate_only As Boolean) As Boolean
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_submit", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            REM check authentication and set logged on user
            Dim bc_am_load As New bc_am_load("Insight")
            Dim bc_am_insight_parameters As New bc_am_insight_parameters
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim osubmit As New bc_am_in_submit(show_form, ao_object, ao_type, validate_only)
                bc_ax_api_submit = osubmit.close_flag
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", "bc_ax_api_submit", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax__insight_api", "bc_ax_api_submit", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Function
    'Public Function bc_ax_api_force_check_in(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String) As Boolean
    '    Dim slog As New bc_cs_activity_log("bc_ax_api_force_check_in", "bc_ax_api_submit", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

    '    Try
    '        Dim bc_cs_central_settings As New bc_cs_central_settings(True)
    '        REM check authentication and set logged on user
    '        Dim bc_am_load As New bc_am_load("Insight")
    '        Dim bc_am_insight_parameters As New bc_am_insight_parameters
    '        If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
    '            Dim ofci As New bc_am_force_check_in(show_form, ao_object, ao_type)
    '            bc_ax_api_force_check_in = ofci.close_flag
    '        End If

    '    Catch ex As Exception
    '        Dim oerr As New bc_cs_error_log("bc_ax_api_force_check_in", "bc_ax_api_submit", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        slog = New bc_cs_activity_log("bc_ax_api_force_check_in", "bc_ax_api_submit", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
    '    End Try
    'End Function
    Public Sub bc_ax_api_open(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_open", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            REM check authentication and set logged on user
            Dim bc_am_load As New bc_am_load("Insight")
            Dim bc_am_insight_parameters As New bc_am_insight_parameters
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim open As New bc_am_in_open(show_form, ao_object, ao_type)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", "bc_ax_api_open", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax__insight_api", "bc_ax_api_open", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    Public Sub bc_ax_api_custom_rows(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_custom_rows", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            REM check authentication and set logged on user
            Dim bc_am_load As New bc_am_load("Insight")
            Dim bc_am_insight_parameters As New bc_am_insight_parameters
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim open As New bc_am_in_custom_items(show_form, ao_object, ao_type)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", "bc_ax_api_custom_rows", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax__insight_api", "bc_ax_api_custom_rows", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    Public Sub bc_ax_api_retrieve_items(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_retrieve_items", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            bc_am_excel_functions.bulk_write_to_excel = True
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            REM check authentication and set logged on user
            Dim bc_am_load As New bc_am_load("Insight")
            Dim bc_am_insight_parameters As New bc_am_insight_parameters
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim open As New bc_am_in_retrieve_data(show_form, ao_object, ao_type)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", "bc_ax_api_retrieve_items", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_am_excel_functions.bulk_write_to_excel = False
            slog = New bc_cs_activity_log("bc_ax__insight_api", "bc_ax_api_retrieve_items", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    Public Function bc_ax_api_retrieve_all_values(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal validate_only As Boolean) As Boolean
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_retrieve_all_values", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            bc_am_excel_functions.bulk_write_to_excel = True
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            REM check authentication and set logged on user
            Dim bc_am_load As New bc_am_load("Insight")
            Dim bc_am_insight_parameters As New bc_am_insight_parameters
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                REM structured retrieval
                Dim oretrive As New bc_am_in_download_values(show_form, ao_object, ao_type, validate_only)
                REM ad-hoc retrieval
                Dim open As New bc_am_in_retrieve_data(show_form, ao_object, ao_type)

            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", "bc_ax_api_retrieve_all_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_am_excel_functions.bulk_write_to_excel = False
            slog = New bc_cs_activity_log("bc_ax__insight_api", "bc_ax_api_retrieve_all_values", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Function

    Public Function bc_ax_api_build_corp_action(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal validate_only As Boolean) As Boolean
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_build_corp_action", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim omessage As bc_cs_message
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)

            REM check authentication and set logged on user
            Dim bc_am_load As New bc_am_load("Insight")

            If bc_cs_central_settings.selected_conn_method = "local" Then
                omessage = New bc_cs_message("Blue Curve", "This function can not run offline!", bc_cs_message.MESSAGE)
                Exit Function
            End If

            Dim bc_am_insight_parameters As New bc_am_insight_parameters
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                'Dim osubmit As New bc_am_corp_action_frm(show_form, ao_object, ao_type, validate_only)
                Dim osubmit As New bc_am_corp_build(show_form, ao_object, ao_type, validate_only)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", "bc_ax_api_build_corp_action", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_insight_api", "bc_ax_api_build_corp_action", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Function

    Public Function bc_ax_api_submit_corp_action(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal validate_only As Boolean) As Boolean
        Dim slog As New bc_cs_activity_log("bc_insight_ax_api", "bc_ax_api_submit_corp_action", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim omessage As bc_cs_message
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)

            REM check authentication and set logged on user
            Dim bc_am_load As New bc_am_load("Insight")

            If bc_cs_central_settings.selected_conn_method = "local" Then
                omessage = New bc_cs_message("Blue Curve", "This function can not run offline!", bc_cs_message.MESSAGE)
                Exit Function
            End If

            Dim bc_am_insight_parameters As New bc_am_insight_parameters
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                'Dim osubmit As New bc_am_corp_submit_frm(show_form, ao_object, ao_type, validate_only)
                Dim osubmit As New bc_am_corp_submit(show_form, ao_object, ao_type, validate_only)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_api", "bc_ax_api_submit_corp_action", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_insight_api", "bc_ax_api_submit_corp_action", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Function

End Class
Public Class bc_ax_insight_configuration_api
    Public Function bc_ax_api_download_sheet(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal sheet_id As Integer, ByVal logical_template_id As Long, ByVal context_id As Long) As Boolean
        Dim slog As New bc_cs_activity_log("bc_ax_insight_configuration_api", "bc_ax_api_download_sheet", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            REM check authentication and set logged on user
            Dim bc_am_load As New bc_am_load("Insight Config")
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                REM structured retrieval
                Dim oupload As New bc_am_insight_configuration(ao_object, ao_type)

                bc_ax_api_download_sheet = oupload.download_to_excel(sheet_id, logical_template_id, context_id)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_configuration_api", "bc_ax_api_download_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_insight_configuration_api", "bc_ax_api_download_sheet", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Function
    Public Function bc_ax_api_add_new_template(ByVal ao_object As Object, ByVal ao_type As String, ByVal from_template As Integer) As Boolean
        Dim slog As New bc_cs_activity_log("bc_ax_insight_configuration_api", "bc_ax_api_download_sheet", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            REM check authentication and set logged on user
            Dim bc_am_load As New bc_am_load("Insight Config")
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                REM structured retrieval
                Dim oupload As New bc_am_insight_configuration(ao_object, ao_type)
                bc_ax_api_add_new_template = oupload.add_new_template(from_template)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_configuration_api", "bc_ax_api_download_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_insight_configuration_api", "bc_ax_api_download_sheet", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Function
    Public Function bc_ax_api_download_all(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal context_id As Long) As Boolean
        Dim slog As New bc_cs_activity_log("bc_ax_insight_configuration_api", "bc_ax_api_download_all", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            REM check authentication and set logged on user
            Dim bc_am_load As New bc_am_load("Insight Config")
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                REM structured retrieval
                Dim oupload As New bc_am_insight_configuration(ao_object, ao_type)
                bc_ax_api_download_all = oupload.download_all_to_excel(context_id)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_configuration_api", "bc_ax_api_download_all", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_insight_configuration_api", "bc_ax_api_download_all", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Function
    Public Function bc_ax_api_upload_sheet(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal sheet_id As Integer) As Boolean
        Dim slog As New bc_cs_activity_log("bc_ax_insight_configuration_api", "bc_ax_api_upload_sheet", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            REM check authentication and set logged on user
            Dim bc_am_load As New bc_am_load("Insight Config")
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                REM structured retrieval
                Dim odownload As New bc_am_insight_configuration(ao_object, ao_type)
                bc_ax_api_upload_sheet = odownload.upload_from_excel(sheet_id)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_configuration_api", "bc_ax_api_upload_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_insight_configuration_api", "bc_ax_api_upload_sheet", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Function
    Public Function download_from_excel(ByVal sheet_id As Integer) As Boolean

    End Function
    Public Function bc_ax_api_delete_sheet(ByVal ao_object As Object, ByVal ao_type As String, ByVal sheet_id As Integer) As Boolean
        Dim slog As New bc_cs_activity_log("bc_ax_insight_configuration_api", "bc_ax_api_download_sheet", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            REM check authentication and set logged on user
            Dim bc_am_load As New bc_am_load("Insight Config")
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                REM structured retrieval
                Dim oupload As New bc_am_insight_configuration(ao_object, ao_type)
                REM now reload all
                bc_ax_api_delete_sheet = oupload.delete_logical_template(sheet_id)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_configuration_api", "bc_ax_api_download_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_insight_configuration_api", "bc_ax_api_download_sheet", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Function
    Public Shared obc_in_config As bc_am_in_context

    REM application based config tool
    Public Sub bc_ax_insight_config_app(ByVal ao_object As Object, ByVal ao_app As Object)
        Dim slog As New bc_cs_activity_log("bc_ax_insight_configuration_api", "bc_ax_insight_config_app", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim obc_in_config As bc_am_in_context
            If bc_am_in_context.loaded = False Then
                Dim bc_cs_central_settings As New bc_cs_central_settings(True)
                REM check authentication and set logged on user
                Dim bc_am_load As New bc_am_load("Insight Config")
                If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                    If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
                        obc_in_config = New bc_am_in_context
                        obc_in_config.oexcel = ao_object
                        obc_in_config.oexcelapp = ao_app
                        obc_in_config.Show()
                    End If
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_insight_configuration_api", "bc_ax_insight_config_app", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_insight_configuration_api", "bc_ax_insight_config_app", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub

    

End Class
Public Class bc_ax_api_insight_changes
    Public Sub bc_ax_api_changes()
        Dim och As New bc_am_in_changes
    End Sub
End Class
