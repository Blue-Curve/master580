Option Strict On
Imports System
Imports System.Runtime.InteropServices
Imports System.IO
Imports Microsoft.Win32
Imports Microsoft.VisualBasic
Imports System.Threading
Imports System.Diagnostics
Imports BlueCurve.Core.CS
Public Class ProcessList
    Public Function GetFileNames(ByVal strInstance As String) As String
        Dim strFileNames As String = ""
        Dim strFileName As String
        Dim inti As Integer
        Try
            Dim localByName As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcessesByName(strInstance)
            For inti = 0 To localByName.Length - 1
                strFileName = Replace(localByName(inti).MainWindowTitle(), "Microsoft Excel -", "")
                strFileNames = strFileNames & strFileName & ","
            Next
            'remove the trailing comma
            strFileNames = Left(strFileNames, strFileNames.Length - 1)
        Catch ex As Exception
            strFileNames = ""
            Dim slog As New bc_cs_activity_log("blucurve.application.ax.Configure :error in ", "GetUserProfileAppKeyValue .", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
        Finally
            GetFileNames = strFileNames
        End Try
    End Function
    Public Function GetInstanceCount(ByVal strInstance As String) As Integer
        Dim intCount As Integer
        Try
            Dim localByName As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcessesByName(strInstance)
            intCount = localByName.Length
        Catch ex As Exception
            intCount = 0
            Dim slog As New bc_cs_activity_log("blucurve.application.ax.Configure :error in ", "GetUserProfileAppKeyValue .", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
        Finally
            GetInstanceCount = intCount
        End Try
    End Function
End Class
