Imports BlueCurve.Core.CS
Imports BlueCurve.Deployment.AM
Imports BlueCurve.Core.AS
Imports System.Exception
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Diagnostics
Imports System.Runtime.InteropServices
'==========================================
' Bluecurve Limited 2012
' Module:         Deployment
' Type:           AP
' Desciption:     Deployment initial class
' Public Methods: New
'                 
' Version:        1.0
' Change history:
'
'==========================================

Public Class bc_ap_Deployment_start

    Public Sub New()

        Try

            Dim bcAmLoad As New bc_am_load(bc_cs_central_settings.DEPLOYMENT_MANAGER_APPLICATION_NAME)

            If bcAmLoad.bcancelselected = True Then
                Exit Sub
            End If

            If bcAmLoad.bdataloaded = False Then
                Throw New SystemLoadfailedException
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                Dim message As New bc_cs_message("Deployment - Blue Curve", "No Network Detected Deployment Cannot be Loaded", bc_cs_message.MESSAGE)
            Else
                bc_cs_central_settings.version = System.Windows.Forms.Application.ProductVersion
                ' initialise System

                Dim view As New bc_am_dd_main
                Dim controller As New bc_am_deployment(view)
                controller.Show()

            End If

        Catch ex As Exception
            Dim message As New bc_cs_message("Deployment - Blue Curve", "System Failed To Load. Contact System Administrator!", bc_cs_message.MESSAGE)
        End Try

    End Sub

End Class