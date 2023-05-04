Imports BlueCurve.Core.CS
Imports BlueCurve.BluePrint.AM
Imports BlueCurve.Core.as
Imports System.Exception
Imports System.io
Imports System.Runtime.Serialization
Imports System.Diagnostics

'==========================================
' Bluecurve Limited 2008
' Module:         BluePrint
' Type:           AP
' Desciption:     Assemble Main Entry Point
' Public Methods: Main
'                 
' Version:        1.0
' Change history:
'
'==========================================
Module process_main
    Public Sub main()

        ' if a copy is running then don't start another
        If check_prev_instance() = True Then
            System.Windows.Forms.Application.Exit()
        Else
            Dim obc_ap_assemble_start = New bc_ap_BluePrint_start
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

'==========================================
' Bluecurve Limited 2008
' Module:         BluePrint
' Type:           AP
' Desciption:     BluePrint initial class
' Public Methods: New
'                 
' Version:        1.0
' Change history:
'
'==========================================

Public Class bc_ap_BluePrint_start

    Public Sub New()

        Try
            Dim bcAmLoad As New bc_am_load("BluePrint")

            If bcAmLoad.bcancelselected = True Then
                Exit Sub
            End If

            If bcAmLoad.bdataloaded = False Then
                Throw New SystemLoadfailedException
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                Dim message As New bc_cs_message("BluePrint - Blue Curve", "No Network Detected BluePrint Cannot be Loaded", bc_cs_message.MESSAGE)
            Else
                bc_cs_central_settings.version = System.Windows.Forms.Application.ProductVersion
                ' initialise System

                'added to handle Office 2010 startup changes
                MessageFilter.Register()

                Dim view As New bc_am_bp_main
                Dim controller As New bc_am_blueprint(view)
                controller.Show()

                'added to handle Office 2010 startup changes
                MessageFilter.Revoke()

            End If

        Catch ex As Exception
            Dim message As New bc_cs_message("BluePrint - Blue Curve", "System Failed To Load. Contact System Administrator!", bc_cs_message.MESSAGE)
        End Try

    End Sub

End Class







