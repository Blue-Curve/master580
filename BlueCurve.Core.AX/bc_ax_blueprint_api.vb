Imports BlueCurve.Core.CS
Imports BlueCurve.Core.AS
Imports BlueCurve.blueprint.am
'==========================================
' Bluecurve Limited 2008
' Module:         BluePrint
' Type:           AX
' Desciption:     API to support toolbar
'                 for use in Word / 
'                 Powerpoint etc.
' Public Methods: 
'  
' Version:        1.0
' Change history:
'
'==========================================
Public Class bc_ax_blueprint_api

    Public Sub New()
        REM read config file into memory
        Dim bc_central_settings As New bc_cs_central_settings(True)

    End Sub

    Public Sub bc_ax_api_show_all_markup(ByVal filePathName As String, ByVal ao_object As Object)

        Dim slog As New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_show_all_markup", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("BluePrint API")

            If Not bc_am_load.abortload Then
                Dim template As New bc_am_template(filePathName)
                template.ShowAllMarkup(ao_object)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_blueprint_api", "bc_ax_api_show_all_markup", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_show_all_markup", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Sub

    Public Function bc_ax_api_package_for_use(ByVal filePathName As String, ByVal ao_object As Object) As Boolean

        Dim slog As New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_package_for_use", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("BluePrint API")

            If Not bc_am_load.abortload Then
                Dim template As New bc_am_template(filePathName)
                If template.PackageForUse(ao_object) Then
                    Return True
                End If
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_blueprint_api", "bc_ax_api_package_for_use", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_package_for_use", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Function

    Public Sub bc_ax_api_advanced(ByVal filePathName As String, ByVal ao_object As Object)

        Dim slog As New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_advanced", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("BluePrint API")

            If Not bc_am_load.abortload Then
                Dim tempComp As New bc_am_template(filePathName)
                tempComp.Advanced(ao_object)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_blueprint_api", "bc_ax_api_advanced", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_advanced", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Sub

    Public Sub bc_ax_api_delete_sub_component(ByVal filePathName As String, ByVal ao_object As Object)

        Dim slog As New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_delete_sub_component", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("BluePrint API")

            If Not bc_am_load.abortload Then
                Dim tempComp As New bc_am_template(filePathName)
                tempComp.DeleteSubComponent(ao_object)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_blueprint_api", "bc_ax_api_delete_sub_component", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_delete_sub_component", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Sub

    Public Sub bc_ax_api_add_update_sub_component(ByVal filePathName As String, ByVal ao_object As Object)

        Dim slog As New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_add_sub_component", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("BluePrint API")

            If Not bc_am_load.abortload Then
                Dim tempComp As New bc_am_template(filePathName)
                tempComp.AddUpdateSubComponent(ao_object)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_blueprint_api", "bc_ax_api_add_sub_component", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_add_sub_component", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Sub

    Public Sub bc_ax_api_register_new_table(ByVal filePathName As String, ByVal ao_object As Object)

        Dim slog As New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_register_new_table", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("BluePrint API")

            If Not bc_am_load.abortload Then
                Dim tempComp As New bc_am_template(filePathName)
                tempComp.RegisterNewTable(ao_object)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_blueprint_api", "bc_ax_api_register_new_table", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_register_new_table", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Sub

    Public Sub bc_ax_api_insert_registered_table(ByVal filePathName As String, ByVal ao_object As Object)

        Dim slog As New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_insert_registered_table", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("BluePrint API")

            If Not bc_am_load.abortload Then
                Dim tempComp As New bc_am_template(filePathName)
                tempComp.InsertRegisteredTable(ao_object)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_blueprint_api", "bc_ax_api_insert_registered_table", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_insert_registered_table", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Sub

    Public Sub bc_ax_api_delete_registered_table(ByVal filePathName As String, ByVal ao_object As Object)

        Dim slog As New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_delete_registered_table", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("BluePrint API")

            If Not bc_am_load.abortload Then
                Dim tempComp As New bc_am_template(filePathName)
                tempComp.DeleteRegisteredTable(ao_object)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_blueprint_api", "bc_ax_api_delete_registered_table", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_delete_registered_table", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Sub

    Public Sub bc_ax_api_update_registered_table(ByVal filePathName As String, ByVal ao_object As Object)

        Dim slog As New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_register_new_table", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_central_settings As New bc_cs_central_settings(True)
            Dim bc_am_load As New bc_am_load("BluePrint API")

            If Not bc_am_load.abortload Then
                Dim tempComp As New bc_am_template(filePathName)
                tempComp.UpdateRegisteredTable(ao_object)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ax_blueprint_api", "bc_ax_api_register_new_table", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ax_blueprint_api", "bc_ax_api_register_new_table", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Sub

End Class
