Imports BlueCurve.Core.CS
Imports BlueCurve.CommonPlatform.AM
Imports BlueCurve.Core.AS
Imports System.Threading
Module commonplatform_main
    Public Sub main()
        REM if copy running dont start another
        If check_prev_instance() = True Then
            System.Windows.Forms.Application.Exit()
        Else
            Dim obc_ap_commonplatform_start = New bc_ap_commonplatform_start
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
            System.Windows.Forms.Application.Exit()
        End If
    End Function


End Module

Public Class bc_ap_commonplatform_start

    Public Sub New()

        Dim message As bc_cs_message

        Try

            Dim bcAmLoad As New bc_am_load("Insight Config")

            If bcAmLoad.bcancelselected = True Then
                Exit Sub
            End If

            If bcAmLoad.bdataloaded = False Then
                Throw New SystemLoadfailedException
            End If

            bc_cs_central_settings.version = System.Windows.Forms.Application.ProductVersion

            Dim cp As New bc_am_cp_container

            cp.ShowDialog()

        Catch ex As Exception
            message = New bc_cs_message("Blue Curve Common Platform", ex.Message, bc_cs_message.MESSAGE)
        End Try

    End Sub

End Class
