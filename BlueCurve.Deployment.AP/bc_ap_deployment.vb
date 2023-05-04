Imports BlueCurve.Core.CS
Imports BlueCurve.Core.as
Imports System.Exception
Imports System.io
Imports System.Runtime.Serialization
Imports System.Diagnostics
Imports System.Runtime.InteropServices
'==========================================
' Bluecurve Limited 2012
' Module:         Deployment
' Type:           AP
' Desciption:     Assemble Main Entry Point
' Public Methods: Main
'                 
' Version:        1.0
' Change history:
'
'==========================================
Module bc_ap_deployment

    Public Sub main()

        bc_cs_central_settings.ApplicationName = bc_cs_central_settings.DEPLOYMENT_MANAGER_APPLICATION_NAME

        ' if a copy is running then don't start another
        If check_prev_instance() = True Then
            System.Windows.Forms.Application.Exit()
        Else
            Dim obc_ap_assemble_start = New bc_ap_Deployment_start
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

        If Process.GetProcessesByName(aProcName).Length > 1 Then
            check_prev_instance = True
        End If
    End Function

End Module

