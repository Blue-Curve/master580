Imports BlueCurve.Core.CS
Imports BlueCurve.Process.AM
Imports BlueCurve.Core.AS
Imports System.Exception
Imports System.io
Imports System.Runtime.Serialization
Imports System.Diagnostics

Module process_main
    Public Sub main()

        REM if copy running dont start another
        If check_prev_instance() = True Then
            System.Windows.Forms.Application.Exit()
        Else
            Dim obc_ap_process_start = New bc_ap_process_start
        End If

    End Sub
    Private Function check_prev_instance() As Boolean

        check_prev_instance = False

        Dim bcs As New bc_cs_central_settings(True)

        If bc_cs_central_settings.multiple_instance = True Then
            Exit Function
        End If

        Dim aModuleName As String = Diagnostics.Process.GetCurrentProcess.MainModule.ModuleName

        Dim aProcName As String = System.IO.Path.GetFileNameWithoutExtension(aModuleName)

        If System.Diagnostics.Process.GetProcessesByName(aProcName).Length > 1 Then
            check_prev_instance = True
            System.Windows.Forms.Application.Exit()
        End If

    End Function

End Module
Public Class bc_ap_process_start

    Public Sub New()

        Try
            REM use same load as create as create can be invoked 
            REM from workflow
           
            Dim bc_am_load As New bc_am_load("Create")

            If bc_am_load.bcancelselected = True Then

                Exit Sub
            End If

            If bc_am_load.bdataloaded = False Then

                Throw New SystemLoadfailedException
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                Dim omessage As New bc_cs_message("Blue Curve - Process", "No Network Detected Workflow Cannot be Loaded", bc_cs_message.MESSAGE)
            Else
                REM system cant work offline so exit
                bc_cs_central_settings.version = System.Windows.Forms.Application.ProductVersion
                REM initialise system
                Dim owf As New bc_am_process
                owf.Load()
            End If

        Catch ex As Exception
            Dim msg = New bc_cs_message("Blue Curve Process", "System Failed To Load. Contact System Administrator!", bc_cs_message.MESSAGE,False,False, "Yes","No",True)
        End Try

    End Sub

End Class
