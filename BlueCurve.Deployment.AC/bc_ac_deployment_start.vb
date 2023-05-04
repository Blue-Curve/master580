Imports BlueCurve.Core.CS
Imports BlueCurve.Deployment.AM
Imports BlueCurve.Core.AS
Imports System.Exception
Imports System.IO
Imports System.Runtime.Serialization
Imports System.Diagnostics
Imports System.Runtime.InteropServices

Public Class bc_ac_deployment_start

    Dim intErrorCode As Int32

    Public Property ErrorCode() As Int32
        Get
            Return intErrorCode
        End Get
        Set(ByVal intErrorCode As Int32)
            Me.intErrorCode = intErrorCode
        End Set
    End Property

    Public Sub New()

        Try

            bc_cs_central_settings.CommandLine = True

            Dim bcAmLoad As New bc_am_load("Deployment")

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

                If My.Application.CommandLineArgs.Count = 2 Or My.Application.CommandLineArgs.Count = 3 Then

                    Dim intOffset As Integer = My.Application.CommandLineArgs.Count - 1

                    Dim controller As New bc_am_deployment(My.Application.CommandLineArgs(My.Application.CommandLineArgs.Count - intOffset))

                    Console.WriteLine()
                    Console.WriteLine("Operations completed.")

                    If My.Application.CommandLineArgs(My.Application.CommandLineArgs.Count - 1) = "w" Then
                        Console.Read()
                    End If

                    ErrorCode = controller.ErrorCode

                Else
                    ErrorCode = ErrorCodes.UNKNOWN
                    Console.WriteLine("Error: Invalid arguments. Please try again.")
                End If

            End If

        Catch ex As Exception
            If ErrorCode = ErrorCodes.NONE Then
                ErrorCode = ErrorCodes.UNKNOWN
            End If
            Dim message As New bc_cs_message("Deployment - Blue Curve", "System Failed To Load. Contact System Administrator!", bc_cs_message.MESSAGE)
        End Try

    End Sub

End Class
