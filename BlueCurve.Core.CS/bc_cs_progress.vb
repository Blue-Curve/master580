Imports System.Threading
REM ==========================================
REM Bluecurve Limited 2005
REM Module:       Progress Bar Service
REM Description:  New
REM               unload                          
REM Type:         CS
REM               Read from file
REM Version:      1
REM Change history
REM ==========================================
Public Class bc_cs_progress
    Dim time As Integer
    Dim fpbar As New bc_cs_progress_bar
    Dim t As Thread
    Dim show_commentary As Boolean
    Dim inc_flag As Boolean
    
    REM global commentary area set by activity log holds
    Public Shared caption As String
    REM current commentary
    Public Shared commentary As String
    REM suspend variable called if error handler shows an error message
    Public Shared suspend As Boolean
    Public i As Integer
    Private Const MAX_LENGTH As Integer = 368

    REM takes as input the caption name, task name and the number of seconds the
    REM task progress is being tracked for is anticipated to take
    REM show commentray determines whether to show the current commentary set
    REM by the activity log
    Public Sub New()
        fpbar.Label2.Width = 0
    End Sub
    Public Sub New(ByVal strcaption As String, ByVal strtask As String, ByVal itime As Integer, ByVal bshowcommentary As Boolean, ByVal inc_flag As Boolean)
        If Not bc_cs_central_settings.CommandLine Then
            fpbar.Text = strcaption
            fpbar.task.Text = strtask
            show_commentary = bshowcommentary
            suspend = False
            time = itime
            Me.inc_flag = inc_flag
            fpbar.ProgressBar1.Value = 1

            'If bshowcommentary = False Then
            'fpbar.Height = 134
            fpbar.Label2.Width = 0
            'fpbar.PictureBox1.Top = -5
            'Else
            commentary = "initiating process..."
            'fpbar.Height = 168
            'fpbar.PictureBox1.Top = 32
            'End If
            If inc_flag = False Then
                t = New Thread(New ThreadStart(AddressOf dotask))
                t.Start()
            Else
                fpbar.Show()
                increment(strtask)
            End If


        Else
            Console.WriteLine(strcaption)
        End If
    End Sub
    Public Sub change_caption(ByVal caption As String)
        If Not bc_cs_central_settings.CommandLine Then
            fpbar.task.Text = caption
            fpbar.Refresh()
        Else
            Console.WriteLine(caption)
        End If

    End Sub
    Public Sub increment(ByVal caption As String)
        Try
            If Not bc_cs_central_settings.CommandLine Then
                fpbar.Label2.Width = fpbar.Label2.Width + 51
                fpbar.task.Text = caption

                fpbar.Refresh()
            Else
                Console.WriteLine(caption)
            End If

        Catch ex As Exception
            'MsgBox("progress bar: " + ex.Message)
        End Try
    End Sub
    'allred
    Public Sub increment(ByVal percent As Decimal)
        Try
            If Not bc_cs_central_settings.CommandLine Then
                fpbar.Label2.Width = System.Math.Round(MAX_LENGTH * (percent / 100))
                fpbar.Refresh()
            Else
                Console.WriteLine(percent.ToString() & "% done.")
            End If
        Catch ex As Exception
            MsgBox("progress bar: " + ex.Message)
        End Try
    End Sub
    Public Sub increment(ByVal caption As String, ByVal percent As Integer)

        Try
            If Not bc_cs_central_settings.CommandLine Then
                fpbar.Label2.Width = System.Math.Round(MAX_LENGTH * (percent / 100))
                fpbar.task.Text = caption
                fpbar.Refresh()
            Else
                Console.WriteLine(caption & "  " & percent.ToString() & "% done.")
            End If
        Catch ex As Exception
            MsgBox("progress bar: " + ex.Message)
        End Try

    End Sub

    Public Sub complete()
        Try
            If Not bc_cs_central_settings.CommandLine Then
                fpbar.Label2.Width = MAX_LENGTH
                fpbar.Refresh()
            End If
        Catch ex As Exception
            MsgBox("progress bar: " + ex.Message)
        End Try
    End Sub

    Public Sub refresh()
        If Not bc_cs_central_settings.CommandLine Then
            fpbar.Refresh()
        End If
    End Sub
    REM unloads progress bar and aborts thread
    Public Sub unload()
        Try

            If Not bc_cs_central_settings.CommandLine Then
                If fpbar.Visible = False Then
                    Exit Sub
                End If
                i = 0
                If inc_flag = False Then
                    t.Abort()
                End If

                fpbar.Hide()
                fpbar = Nothing
            Else
                'ditto, not sure if a message should be displayed
            End If
        Catch

        End Try
    End Sub
    Public Sub show()
        If Not bc_cs_central_settings.CommandLine Then
            fpbar.Visible = True
        End If
    End Sub
    Public Sub hide()
        If Not bc_cs_central_settings.CommandLine Then
            Try
                fpbar.Visible = False
            Catch

            End Try
        End If
    End Sub
    REM thread to update progress bar
    Private Sub dotask()
        If Not bc_cs_central_settings.CommandLine Then
            fpbar.Show()
        End If
        i = 1
        While i >= 1
            If suspend = True Then
                Me.unload()
            Else
                fpbar.Show()
                REM progress bar increments every 100* time milliseconds
                REM by a scale of 10
                If show_commentary = True Then
                    'fpbar.Commentary.Text = commentary
                    'fpbar.Text = caption
                End If
                'fpbar.ProgressBar1.Increment(10)
                If fpbar.Label2.Width = 357 Then
                    fpbar.Label2.Width = 0
                End If
                fpbar.Label2.Width = fpbar.Label2.Width + 51
                fpbar.Refresh()
                Thread.Sleep(100 * time)
            End If
        End While
    End Sub
    Public Sub SetTopmost()
        If Not bc_cs_central_settings.CommandLine AndAlso Not fpbar Is Nothing Then
            fpbar.TopMost = True
        End If
    End Sub
End Class

