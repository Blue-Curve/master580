Imports System.IO
Imports System.Text
Imports System.Diagnostics

<CLSCompliant(True)> _
Public Class ErrorLogger

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     Loader error log
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>


    Dim strdate As String
    Dim ostrdate As String

    Public Sub New()

        'default constructor

    End Sub
    Public Sub WriteToErrorLog(ByVal msg As String, ByVal innerMsg As String, ByVal stkTrace As String, ByVal title As String)

        'check and make the directory if necessary; this is set to look in 

        If Not System.IO.Directory.Exists("C:\bluecurve\" & "Errors\") Then
            System.IO.Directory.CreateDirectory("C:\bluecurve\" & "Errors\")
        End If

        'check the file
        strdate = DateAndTime.DateString

        Dim checkfs As FileStream = New FileStream("C:\bluecurve\" & "Errors\loadererrlog" & strdate & ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite)
        Dim checks As StreamWriter = New StreamWriter(checkfs)
        checks.Close()
        checkfs.Close()

        'log it
        Dim logfs As FileStream = New FileStream("C:\bluecurve\" & "Errors\loadererrlog" & strdate & ".txt", FileMode.Append, FileAccess.Write)
        Dim logs As StreamWriter = New StreamWriter(logfs)
        logs.Write("Date/Time: " & DateTime.Now.ToString() & vbCrLf)
        logs.Write("Title: " & title & vbCrLf)
        logs.Write("Message: " & msg & vbCrLf)
        logs.Write("Inner: " & innerMsg & vbCrLf)
        logs.Write("StackTrace: " & stkTrace & vbCrLf)
        logs.Write("================================================" & vbCrLf)
        logs.Close()
        logfs.Close()

    End Sub

End Class


<CLSCompliant(True)> _
Public Class ActivityLogger

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     Loader log
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>


    Dim strdate As String
    Dim ostrdate As String

    Public Sub New()

        'default constructor


    End Sub

    Public Sub WriteToActivityLog(ByVal msg As String, ByVal title As String)

        'check and make the directory if necessary; this is set to look in 

        If Not System.IO.Directory.Exists("C:\bluecurve\" & "Log\") Then
            System.IO.Directory.CreateDirectory("C:\bluecurve\" & "Log\")
        End If

        'check the file
        strdate = DateAndTime.DateString

        Dim checkfs As FileStream = New FileStream("C:\bluecurve\" & "Log\loaderactivitylog" & strdate & ".txt", FileMode.OpenOrCreate, FileAccess.ReadWrite)
        Dim checks As StreamWriter = New StreamWriter(checkfs)
        checks.Close()
        checkfs.Close()

        'log it
        Dim logfs As FileStream = New FileStream("C:\bluecurve\" & "Log\loaderactivitylog" & strdate & ".txt", FileMode.Append, FileAccess.Write)
        Dim logs1 As StreamWriter = New StreamWriter(logfs)

        logs1.Write("Date/Time: " & DateTime.Now.ToString() & " ")
        logs1.Write("Title: " & title & " ")
        logs1.Write("Message: " & msg & vbCrLf)
        logs1.Close()
        logfs.Close()

    End Sub

End Class
