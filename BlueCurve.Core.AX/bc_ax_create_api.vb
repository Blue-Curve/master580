Option Strict Off
Imports System
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Collections
Imports Microsoft.Win32
Imports Microsoft.VisualBasic
Imports System.Threading
Imports System.Diagnostics
Imports BlueCurve.create.am
Imports BlueCurve.core.OM
Imports BlueCurve.core.CS
Imports BlueCurve.core.AS
Imports BlueCurve.Process.AM
Imports System.Windows.Forms



REM ==========================================
REM Bluecurve Limited 2005
REM Module:       BC Create Active X API
REM Type:         Application Object
REM Description:  Class that gets converted
REM               via TlbEXP to provide Active X api
REM               into 4.6i framework
REM Version:      1
REM Change history
REM ==========================================
REM Workflow Events Handler
#Region "changes"
'Changes:
'Tracker                 Initials                   Date                      Synopsis
'FIL 4913                PR                         8/1/2014                  Quick Submit
'FIL 8027                PR                         8/1/2014                  Disable Save
#End Region

Public Class bc_ax_create_api
    REM class to contain all methods Active-X can invoke
    REM these call into lower layers of the framework.

    Public Sub New()
        REM read config file into memory
        REM PR remobed this May 2013 so system can be passed through
        REM Dim bc_central_settings As New bc_cs_central_settings(True)

    End Sub
    REM FIL 5.3

    Public Sub bc_ax_api_build_parameters(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal refresh_type As Integer = 1, Optional ByVal brecreate_metadata As Boolean = False, Optional ByVal evaluate_last_update_date As Boolean = False, _
       Optional ByVal username As String = "", Optional ByVal password As String = "", Optional ByRef locked As Boolean = False, Optional ByRef locked_password As String = "", Optional ByVal system As String = "")
        Dim bc_central_settings As New bc_cs_central_settings(True, system)


        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_build_parameters", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")
        Try
            ' if use r name and password passed in then set
            If Trim(username) <> "" Then
                bc_cs_central_settings.user_name = username
                bc_cs_central_settings.user_password = password
            End If

            Dim bc_am_load As New bc_am_load("Create", True, True)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                Dim ocommentary As New bc_cs_activity_log("bc_ax_api_build_parameters", "new", CStr(bc_cs_activity_codes.COMMENTARY), "Cannot Refresh as System working Locally!")
                Dim omessage = New bc_cs_message("Blue Curve Create", "Cannot Set Build Parameters system is offline!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim obc As New bc_am_at_build_params(show_form, ao_object, ao_type, refresh_type, True, brecreate_metadata)


            End If
            If locked = True Then
                locked_password = bc_am_load_objects.obc_prefs.document_protection_password
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_build_parameters", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_build_parameters", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub

    REM =======
    REM api call to enable ad hoc sql to be executed from VBA
    Public Function bc_ax_api_execute_sql(ByVal sql As String, ByVal ao_object As Object, ByVal ao_type As String) As Object
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_execute_sql", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
             Dim bc_am_load As New bc_am_load("Create", True, True)
            'If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
            Dim osql As New bc_am_sql(sql, ao_object, ao_type)
            bc_ax_api_execute_sql = osql.execute
            'Else
            'bc_ax_api_execute_sql = Nothing
            'End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_execute_sql", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Return Nothing
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_execute_sql", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Function
    Public Sub bc_ax_api_submit(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal submit_components As Boolean = True, Optional ByVal brecreate_metadata As Boolean = False, Optional ByVal scannedList(,) As String = Nothing)
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_submit", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try


            'Dim osubmit As New bc_am_at_submit(show_form, ao_object, ao_type, submit_components, False, True, False, 0, brecreate_metadata)
            Dim bc_am_load As New bc_am_load("Create with Offline Working")
         
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                REM ASYNC BACKED OUT
                'Dim osubmit As New bc_am_at_submit(show_form, ao_object, ao_type, submit_components, False, True, False, 0, brecreate_metadata, scannedList)

                Dim osubmit As New bc_am_at_submit(ao_object, ao_type, brecreate_metadata, scannedList)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_submit", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_submit", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    REM JIRA IMSR-4913 
    REM need a toolbar option for this call as well
    Public Sub bc_ax_api_quick_submit(ByVal mode As Integer, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal brecreate_metadata As Boolean = False)
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "quick_submit", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_am_load As New bc_am_load("Create with Offline Working")
            Dim osubmit As New bc_am_at_submit
            osubmit.quick_submit(mode, ao_object, ao_type, brecreate_metadata)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "quick_submit", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "quick_submit", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    Public Sub bc_ax_api_show_sdc_udc(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal show_mode As Boolean, Optional ByVal sdc_colour_index As Long = 0, Optional ByVal udc_colour_index As Long = 0, Optional ByVal show_sdc As Boolean = True, Optional ByVal show_udc As Boolean = True, Optional ByVal brecreate_metadata As Boolean = False)
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_show_sdc_udc", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")
        Try
            Dim oshow_sdc_udc As New bc_am_show_sdc_udc(show_form, ao_object, ao_type, show_mode, show_sdc, show_udc, brecreate_metadata, sdc_colour_index, udc_colour_index)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_show_sdc_udc", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_show_sdc_udc", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    'Public Sub bc_ax_api_refresh(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
    '    Dim bc_cs_central_settings As New bc_cs_central_settings(True)
    '    Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_refresh", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

    '    Try
    '        Dim bc_am_load As New bc_am_load("Create")
    '        If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
    '            Dim orefresh As New bc_am_at_refresh(show_form, ao_object, ao_type)
    '        End If
    '    Catch ex As Exception
    '        Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_refresh", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        slog = New bc_cs_activity_log("bc_create_ax_api", "bc_refresh", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
    '    End Try
    'End Sub
    Public Sub bc_ax_api_prod_tools_structure_wizard(ByVal ao_object As Object)
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "prod_tools_structure_wizard", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim f As New bc_am_page_structure
            Dim c As New Cbc_am_page_structure
            If c.load_data(f, ao_object) = True Then
                f.ShowDialog()
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "prod_tools_structure_wizard", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "prod_tools_structure_wizard", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    Public Sub bc_ax_api_prod_tools_table_wizard(ByVal ao_object As Object)
        Dim bc_central_settings As New bc_cs_central_settings(True)

        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "prod_tools_table_wizard", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim f As New bc_am_table_wizard
            Dim c As New Cbc_am_table_wizard
            If c.load_data(f, ao_object) = True Then
                f.ShowDialog()
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "prod_tools_table_wizard", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "prod_tools_stable_wizard", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    Public Sub bc_ax_api_prod_tools_side_comment(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal submit_components As Boolean = True, Optional ByVal brecreate_metadata As Boolean = False, Optional ByVal scannedList(,) As String = Nothing)
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "prod_tools_side_comment(", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            If ao_type <> bc_ao_at_object.WORD_DOC Then
                Dim omessage As New bc_cs_message("Blue Curve Create", "Not Yet Implemented for this mime type.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

            Dim bc_am_load As New bc_am_load("Create with Offline Working")

            Dim odoc As bc_ao_at_object
            odoc = New bc_ao_word(ao_object)

            Dim err_tx As String
            err_tx = odoc.insert_side_comment
            If err_tx <> "" Then
                Dim omsg = New bc_cs_message("Blue Curve", err_tx, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "prod_tools_side_comment(", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "prod_tools_side_comment(", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub

    Public Function bc_ax_api_componetize(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal doc_id As String) As Boolean
        Dim blnResult As Boolean
        Dim osubmit As bc_am_at_componentize
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_componetize", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_am_load As New bc_am_load("Create")
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                osubmit = New bc_am_at_componentize
                blnResult = osubmit.Componentise(show_form, ao_object, ao_type, doc_id)
            End If
        Catch ex As Exception
            blnResult = False
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_componetize", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_ax_api_componetize = blnResult
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_componetize", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Function
    Public Sub bc_ax_api_component_refresh(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal refresh_type As Integer = 1, Optional ByVal brecreate_metadata As Boolean = False, Optional ByVal evaluate_last_update_date As Boolean = False, _
        Optional ByVal username As String = "", Optional ByVal password As String = "", Optional ByRef locked As Boolean = False, Optional ByRef locked_password As String = "", Optional system As String = "")
        Dim bc_central_settings As New bc_cs_central_settings(True, system)


        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_component_refresh", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")
        Try
            ' if user name and password passed in then set
            If Trim(username) <> "" Then
                bc_cs_central_settings.user_name = username
                bc_cs_central_settings.user_password = password
            End If

            Dim bc_am_load As New bc_am_load("Create", True, True)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                Dim ocommentary As New bc_cs_activity_log("bc_am_at_refresh", "new", CStr(bc_cs_activity_codes.COMMENTARY), "Cannot Refresh as System working Locally!")
                Dim omessage = New bc_cs_message("Blue Curve Create", "Cannot Refresh as system is offline!", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim orefresh As New bc_am_at_component_refresh(show_form, ao_object, ao_type, refresh_type, True, brecreate_metadata, evaluate_last_update_date, locked)
            End If
            If locked = True Then
                locked_password = bc_am_load_objects.obc_prefs.document_protection_password
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_component_refresh", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_component_refresh", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub

    Public Sub bc_ax_api_new_document(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal refresh_type As Integer = 1, Optional ByVal brecreate_metadata As Boolean = False, Optional ByVal evaluate_last_update_date As Boolean = False, _
            Optional ByVal username As String = "", Optional ByVal password As String = "", Optional ByRef locked As Boolean = False, Optional ByRef locked_password As String = "", Optional ByVal system As String = "")

        Dim bc_central_settings As New bc_cs_central_settings(True, system)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_new_document", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        'Dim gpub_type_id As Integer
        'Dim entity_id As Long
        'Dim params As bc_om_parameters
        'Dim sub_entity_id As Long
        'Dim build_mode As Integer
        'Dim compositePubType As Integer
        'Dim clonePubType As Boolean

        Try
            ' if user name and password passed in then set
            If Trim(username) <> "" Then
                bc_cs_central_settings.user_name = username
                bc_cs_central_settings.user_password = password
            End If

            'Dim bc_am_load As New bc_am_load("Create", True, True)
            'PR changes this aug 2013
            Dim bc_am_load As New bc_am_load("Create with Offline Working")
            If bc_am_load.bcancelselected = True Then
                Exit Sub
            End If

            If bc_am_load.bdataloaded = False Then
                Throw New SystemLoadfailedException
            End If

            MessageFilter.Register()

            Dim fxwizard As New bc_dx_am_create_wizard_frm
            fxwizard.build_mode = True
            'fxwizard.gfrom_desktop = True
            fxwizard.bc_am_create_wizard_frm_load()

            If fxwizard.sel_pub_type_Id <> 0 Then
                If fxwizard.is_clone = True Then
                    Dim bic As New bc_am_clone_document
                    fxwizard.WindowState = Windows.Forms.FormWindowState.Minimized
                    If bic.invoke_clone(fxwizard.sel_pub_type_Id, fxwizard.sel_entity_Id1, fxwizard.sel_entity_id2) = False Then
                        fxwizard.WindowState = Windows.Forms.FormWindowState.Normal
                    End If
                ElseIf fxwizard.is_composite = True Then
                    Dim vcb As New bc_dx_adv_composite_build
                    Dim ccb As New Cbc_dx_adv_composite_build(vcb)
                    If ccb.load_data(fxwizard.sel_pub_type_Id, fxwizard.sel_entity_Id1) = True Then
                        vcb.ShowDialog()
                    End If
                Else
                    Dim build_document As New bc_am_at_build_document
                    build_document.launch_product(fxwizard.sel_pub_type_Id, fxwizard.sel_entity_Id1, Nothing, fxwizard.sel_entity_id2)
                End If

                MessageFilter.Revoke()
                Exit Sub
            End If


            'Dim fwizard_main As New bc_am_at_wizard_main
            'fwizard_main.from_create = False
            'fwizard_main.from_workflow = True
            'fwizard_main.ShowDialog()
            'gpub_type_id = fwizard_main.gpub_type_id
            'entity_id = fwizard_main.entity_id
            'params = fwizard_main.params
            'sub_entity_id = fwizard_main.sub_entity_id
            'build_mode = fwizard_main.buildMode
            'compositePubType = fwizard_main.compositePubType
            'clonePubType = fwizard_main.clonePubType
            'fwizard_main = Nothing

            'REM launch
            'Dim build_document As New bc_am_at_build_document
            'If build_mode = 2 Or (build_mode = 1 And compositePubType = 0) Then
            '    If build_document.launch_product(gpub_type_id, entity_id, params, sub_entity_id) = False Then
            '        Throw New SystemLoadfailedException
            '    End If
            'End If

            ''Dim obc_pub_types As New bc_om_pub_types
            'obc_pub_types.tmode = bc_cs_soap_base_class.tREAD
            'obc_pub_types.process_object()

            REM invoke coposite
            'If (build_mode = 1 Or build_mode = 20) And compositePubType <> 0 Then
            '    build_document.invoke_composite(0, compositePubType)
            'End If

            'REM invoke clone
            'If build_mode = 0 And clonePubType = True Then
            '    build_document.invoke_clone(gpub_type_id, 0, 0)
            'End If


            'MessageFilter.Revoke()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_component_refresh", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_component_refresh", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub

    REM FIL JUlY 2013

    'Public Sub bc_ax_api_change_component_params(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal brecreate_metadata As Boolean = False, Optional ByVal bhide_refresh As Boolean = False, Optional ByRef locked As Boolean = False, Optional ByRef locked_password As String = "", Optional ByVal username As String = "", Optional ByVal password As String = "", Optional ByVal system As String = "", Optional ByVal enable_cancel As Boolean = True, Optional ByVal enable_refresh As Boolean = True, Optional ByVal enable_highlight As Boolean = False)
    REM FIL IMSR-8027 
    Public Function bc_ax_api_change_component_params(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal brecreate_metadata As Boolean = False, Optional ByVal bhide_refresh As Boolean = False, Optional ByRef locked As Boolean = False, Optional ByRef locked_password As String = "", Optional ByVal username As String = "", Optional ByVal password As String = "", Optional ByVal system As String = "", Optional ByVal enable_cancel As Boolean = True, Optional ByVal enable_refresh As Boolean = True, Optional ByVal enable_highlight As Boolean = False, Optional ByVal disable_refresh As Boolean = False, Optional ByVal disable_contributor As Boolean = False, Optional ByVal enable_save As Boolean = True) As Integer

        Dim bc_central_settings As New bc_cs_central_settings(True, system)


        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_change_component_params", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            If Trim(username) <> "" Then
                bc_cs_central_settings.user_name = username
                bc_cs_central_settings.user_password = password
            End If

            Dim bc_am_load As New bc_am_load("Create")
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                Dim ocommentary As New bc_cs_activity_log("bc_am_at_refresh", "new", CStr(bc_cs_activity_codes.COMMENTARY), "Cannot Refresh as System working Locally!")
                Dim omessage = New bc_cs_message("Blue Curve Create", "Cannot Ammend Component as system is offline!", bc_cs_message.MESSAGE)
                bc_cs_central_settings.progress_bar.unload()
                Exit Function
            End If
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim exit_code As Integer
                Dim orefresh As New bc_am_at_change_component_params(show_form, ao_object, ao_type, brecreate_metadata, bhide_refresh, locked, enable_cancel, enable_refresh, enable_highlight, disable_refresh, disable_contributor, enable_save, exit_code)
                Return exit_code
            End If
            If locked = True Then
                locked_password = bc_am_load_objects.obc_prefs.document_protection_password
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_change_component_params", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_change_component_params", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Function
    Public Sub bc_ax_api_edit_text_components(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_edit_text_components", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_am_load As New bc_am_load("Create")
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim oedit_text_components As New bc_am_edit_text_components(ao_object, ao_type)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_edit_text_components", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_edit_text_components", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    REM synchonize active X api call
    Public Sub bc_ax_api_synchronize()
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_execute_sql", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_am_load As New bc_am_load("Create")
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim bc_cs_central_settings As New bc_cs_central_settings(True)
                Dim osync As New bc_am_synchronize
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_execute_sql", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_execute_sql", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    REM returns setting using lowest level xml tag
    Public Function bc_ax_api_get_central_setting(ByVal setting As String) As String
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_get_central_settings", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            bc_ax_api_get_central_setting = "Invalid Setting"
            Dim bc_cs_central_settings As New bc_cs_central_settings(True)
            If setting = "local_repos_path" Then
                bc_ax_api_get_central_setting = bc_cs_central_settings.local_repos_path
            End If
            If setting = "local_template_path" Then
                bc_ax_api_get_central_setting = bc_cs_central_settings.local_template_path
            End If
            If setting = "bc_user_dir" Then
                bc_ax_api_get_central_setting = bc_cs_central_settings.get_user_dir
            End If
            If setting = "bc_user_id" Then
                bc_ax_api_get_central_setting = bc_cs_central_settings.logged_on_user_id
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_get_central_settings", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Return ""
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_get_central_settings", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Function
    Public Function bc_ax_style_componetize(ByVal ao_object As Object, ByVal ao_type As String, ByRef ocomponents As Object, ByVal componetize_styles As Object) As Object
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_style_componetize", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")
        Try
            'Dim bc_am_load As New bc_am_load("Create")
            bc_ax_style_componetize = False
            'If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
            Dim comp As New bc_am_style_componetize
            bc_ax_style_componetize = comp.componetize_from_ax(ao_object, ao_type, ocomponents, componetize_styles)
            'End If
        Catch ex As Exception
            bc_ax_style_componetize = Nothing
            Dim comm As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_style_componetize", CStr(bc_cs_activity_codes.COMMENTARY), "Failed: " + ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_style_componetize", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Function
    Public Function bc_ax_html_componetize(ByVal ao_object As Object, ByVal ao_type As String, ByRef ocomponents As Object, ByVal componetize_styles As Object) As Boolean
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_html_componetize", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")
        Try
            'Dim bc_am_load As New bc_am_load("Create")
            bc_ax_html_componetize = False
            'If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
            Dim comp As New bc_am_html_componetize
            bc_ax_html_componetize = comp.componetize_from_ax(ao_object, ao_type, ocomponents, componetize_styles)

            'End If
        Catch ex As Exception
            bc_ax_html_componetize = Nothing
            Dim comm As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_html_componetize", CStr(bc_cs_activity_codes.COMMENTARY), "Failed: " + ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_html_componetize", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Function
    Public Sub bc_ax_api_translate_document(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String)
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_translate_documents", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_am_load As New bc_am_load("Create")
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim otranslate As New bc_am_translate_document
                otranslate.translate(show_form, ao_object, ao_type)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_translate_documents", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax__api_translate_documents", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    'Public Sub bc_ax_api_email_copy(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal embargo_level As Integer)
    '    Dim bc_central_settings As New bc_cs_central_settings(True)
    '    Dim slog As New bc_cs_activity_log("bc_ax_api_email_copy", "bc_ax_api_translate_documents", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")
    '    Try
    '        Dim bc_am_load As New bc_am_load("Create")
    '        If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
    '            Dim oemailcopy As New bc_am_email_copy
    '            oemailcopy.email_copy(show_form, ao_object, ao_type, embargo_level)
    '        End If
    '    Catch ex As Exception
    '        Dim oerr As New bc_cs_error_log("bc_ax_api_email_copy", "bc_ax_api_translate_documents", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        slog = New bc_cs_activity_log("bc_ax_api_email_copy", "bc_ax__api_translate_documents", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
    '    End Try
    'End Sub
    Public Sub bc_ax_api_insert_component(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal component_id As Long)
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_insert_component", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_am_load As New bc_am_load("Create")
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then

                Dim ocomp As New bc_am_insert_components(show_form, ao_object, ao_type, component_id)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_insert_component", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_insert_component", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub

    Public Sub bc_ax_api_dx_insert_component(ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal component_id As Long = 0, Optional ByVal enable_class As Boolean = True, Optional ByVal enable_entity As Boolean = True)
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_dx_insert_component", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_am_load As New bc_am_load("Create")
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim fdxic As New bc_am_dx_insert_component()
                Dim cdxic As New Cbc_am_dx_insert_component(fdxic, ao_type, ao_object, enable_class, enable_entity)
                If cdxic.load(component_id) = True Then
                    fdxic.ShowDialog()
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_dx_insert_component", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_dx_insert_component", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub

    Public Sub bc_ax_api_manage_supporting_documents(ByVal ao_object As Object, ByVal ao_type As String)
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_manage_supporting_documents", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_am_load As New bc_am_load("Create")
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim supportingDocsMgr As New bc_am_at_supporting_docs_mgmt
                If supportingDocsMgr.Initialize(ao_object, ao_type) Then supportingDocsMgr.Show()
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_manage_supporting_documents", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_manage_supporting_documents", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Sub
    Public Sub bc_ax_api_edit_chart(ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal brecreate_metadata As Boolean = False)
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_edit_chart", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Try
            Dim bc_am_load As New bc_am_load("Create")
            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim orefresh As New bc_am_at_edit_chart(ao_object, ao_type, brecreate_metadata)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_edit_chart", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_edit_chart", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    Public Function bc_ax_insert_landscape_page(ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal template_dir As Boolean = False) As String
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_insert_landscape_page(", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")
        bc_ax_insert_landscape_page = ""
        Try
            If template_dir = True Then
                bc_ax_insert_landscape_page = bc_cs_central_settings.local_template_path
                Exit Function
            End If
            Dim olandscape As New bc_am_landscape_page(ao_object, ao_type)
            olandscape.insert()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_insert_landscape_page(", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_insert_landscape_page(", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try

    End Function
    Public Sub bc_ax_maintain_user_defined_componenet(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal brecreate_metadata As Boolean = False)
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_maintain_user_defined_componenet", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")
        Try
            Dim ocomp As New bc_am_udcs(show_form, ao_object, ao_type, brecreate_metadata)
            ocomp.create_component()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_maintain_user_defined_componenet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_maintain_user_defined_componenet", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    Public Sub bc_ax_insert_user_defined_componenet(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal brecreate_metadata As Boolean = False)
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "insert", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")
        Try
            REM FIL MAY 2013

            Dim bc_am_load As New bc_am_load("Create", True, True)

            Dim ocomp As New bc_am_udcs(show_form, ao_object, ao_type, brecreate_metadata)

            ocomp.insert_component()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "insert", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "insert", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    Public Sub bc_ax_delete_user_defined_componenet(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, Optional ByVal brecreate_metadata As Boolean = False)
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "insert", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")
        Try
            Dim ocomp As New bc_am_udcs(show_form, ao_object, ao_type, brecreate_metadata)
            ocomp.delete_component_from_document()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "insert", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "insert", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Sub
    Public Function bc_ax_api_scan_document(ByVal listType As String, ByVal ao_object As Object, ByVal ao_type As String) As String(,)
        Dim bc_central_settings As New bc_cs_central_settings(True)
        Dim slog As New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_scan_document", CStr(bc_cs_activity_codes.TRACE_ENTRY), "")

        Dim scanResults As ArrayList = Nothing
        Dim results(2, 0) As String

        Try
            Dim bc_am_load As New bc_am_load("Create")

            If bc_am_load.bdataloaded = True And bc_am_load.bdeploy = False Then
                Dim scanDoc As New bc_am_scan_document
                scanDoc.scan_document(listType, ao_object, ao_type, scanResults)
            End If

            If Not scanResults Is Nothing AndAlso scanResults.Count > 0 Then
                For i = 0 To scanResults.Count - 1
                    ReDim Preserve results(2, i)
                    results(0, i) = CType(scanResults(i), String(,))(0, 0)
                    results(1, i) = CType(scanResults(i), String(,))(1, 0)
                    results(2, i) = CType(scanResults(i), String(,))(2, 0)
                Next
            Else
                results(0, 0) = ""
                results(1, 0) = ""
                results(2, 0) = ""
            End If

            Return results

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_create_ax_api", "bc_ax_api_scan_document", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Return results
        Finally
            slog = New bc_cs_activity_log("bc_create_ax_api", "bc_ax_api_scan_document", CStr(bc_cs_activity_codes.TRACE_EXIT), "")
        End Try
    End Function

End Class

Public Class bc_ax_bc_about
    Public Sub New()

    End Sub
    Public Sub show_about_box()
        Dim omessage As New bc_cs_message("", "", bc_cs_message.MESSAGE, False, True)
    End Sub

End Class
