Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports BlueCurve.Create.AM
Imports System.Exception
Imports System.IO
Imports System.Diagnostics
Imports System.Threading
Imports System.Runtime.InteropServices
Imports System.Windows.Forms



REM ==========================================
REM Bluecurve Limited 2005
REM Module:       Author Tool
REM Type:         AP
REM Desciption:   Author Tool Main Entry Point
REM Methods:      main
REM               bc_ap_author_tool_start
REM Version:      1.0
REM Change history
REM ==========================================
Module author_tool_main
    Public Sub main()
        REM if copy running dont start another
        If check_prev_instance() = True Then
            System.Windows.Forms.Application.Exit()
        Else

            Dim obc_ap_author_tool_start = New bc_ap_author_tool_start
        End If

    End Sub
    Private Function check_prev_instance() As Boolean
        Dim bcs As New bc_cs_central_settings(True)
        If bc_cs_central_settings.multiple_instance = True Then
            Exit Function
        End If
        check_prev_instance = False
        Dim aModuleName As String = Diagnostics.Process.GetCurrentProcess.MainModule.ModuleName

        Dim aProcName As String = System.IO.Path.GetFileNameWithoutExtension(aModuleName)


        If Process.GetProcessesByName(aProcName).Length > 1 Then
            check_prev_instance = True
            System.Windows.Forms.Application.Exit()
        End If
    End Function


End Module
Public Class bc_ap_author_tool_start

    Public Sub New()
        Dim message As bc_cs_message

        Try
            bc_cs_central_settings.version = System.Windows.Forms.Application.ProductVersion
            REM initialise system

            Dim obcload As New bc_am_load("Create with Offline Working")
            If obcload.bcancelselected = True Then
                Exit Sub
            End If
            If obcload.bdataloaded = False And obcload.bdeploy = False Then
                Throw New SystemLoadfailedException
            Else
                If obcload.bdeploy = False Then
                    dothetask()
                End If
            End If

        Catch ex As Exception
            Dim msg = New bc_cs_message("Blue Curve Process", ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

        End Try

    End Sub
    Public Sub dothetask()
        'added to handle Office 2010 startup changes
        MessageFilter.Register()

        'Dim fwizard_main As New bc_am_at_wizard_main
        'fwizard_main.ShowDialog()
        'fwizard_main = Nothing
       
        Dim fxwizard As New bc_dx_am_create_wizard_frm
        fxwizard.build_mode = False
        fxwizard.gfrom_desktop = True
        fxwizard.TopLevel = True
        fxwizard.TopMost = True
        fxwizard.WindowState = FormWindowState.Normal
        'Dim oapi As New API
        'API.SetWindowPos(fxwizard.Handle.ToInt32, API.HWND_TOPMOST, 10, 10, 1, 1, 1)
        'fxwizard.ShowDialog()

        fxwizard.bc_am_create_wizard_frm_load()
       

        'added to handle Office 2010 startup changes
        MessageFilter.Revoke()

    End Sub
End Class

'added to handle Office 2010 startup changes
<ComImport(), Guid("00000016-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IOleMessageFilter
    <PreserveSig()> Function HandleInComingCall(ByVal dwCallType As Integer, ByVal hTaskCaller As IntPtr, ByVal dwTickCount As Integer, ByVal lpInterfaceInfo As IntPtr) As Integer
    <PreserveSig()> Function RetryRejectedCall(ByVal hTaskCallee As IntPtr, ByVal dwTickCount As Integer, ByVal dwRejectType As Integer) As Integer
    <PreserveSig()> Function MessagePending(ByVal hTaskCallee As IntPtr, ByVal dwTickCount As Integer, ByVal dwPendingType As Integer) As Integer
End Interface

Public Class MessageFilter
    Implements IOleMessageFilter
    <DllImport("Ole32.dll")> _
    Private Shared Function CoRegisterMessageFilter(ByVal newFilter As IOleMessageFilter, ByRef oldFilter As IOleMessageFilter) As Integer
    End Function
    Public Shared Sub Register()
        Dim newFilter As IOleMessageFilter = New MessageFilter()
        Dim oldFilter As IOleMessageFilter = Nothing
        CoRegisterMessageFilter(newFilter, oldFilter)
    End Sub
    Public Shared Sub Revoke()
        Dim oldFilter As IOleMessageFilter = Nothing
        CoRegisterMessageFilter(Nothing, oldFilter)
    End Sub
    Public Function HandleInComingCall(ByVal dwCallType As Integer, ByVal hTaskCaller As System.IntPtr, ByVal dwTickCount As Integer, ByVal lpInterfaceInfo As System.IntPtr) As Integer Implements IOleMessageFilter.HandleInComingCall
        Return 0
    End Function
    Public Function MessagePending(ByVal hTaskCallee As System.IntPtr, ByVal dwTickCount As Integer, ByVal dwPendingType As Integer) As Integer Implements IOleMessageFilter.MessagePending
        Return 2
    End Function
    Public Function RetryRejectedCall(ByVal hTaskCallee As System.IntPtr, ByVal dwTickCount As Integer, ByVal dwRejectType As Integer) As Integer Implements IOleMessageFilter.RetryRejectedCall
        If (dwRejectType = 2) Then
            Return 99
            'Value >=0 and <100: the call is to be retried immediately
            'Value >=100: COM will wait for this many milliseconds and then retry the call
            'Value -1: the call should be canceled. COM the Returns RPC_E_CALL_REJECTED for the original method call
        Else
            Return -1
        End If
    End Function
End Class
