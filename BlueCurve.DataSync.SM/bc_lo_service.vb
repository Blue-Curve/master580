Imports System.Timers


Public Class BlueCurveLoader

    <TimersDescriptionAttribute("TimerIntervalElapsed")> _
    Public Event Elapsed As ElapsedEventHandler

    Dim loaderTimer As System.Timers.Timer


    Public Sub New()

        MyBase.New()
        InitializeComponent()

        loaderTimer = New System.Timers.Timer()
        loaderTimer.Interval = My.Settings.TimerDelay

        ' Hook up the Elapsed event for the timer.
        AddHandler loaderTimer.Elapsed, AddressOf OnTimedEvent

    End Sub


    Protected Overrides Sub OnStart(ByVal args() As String)
        ' Add code here to start your service. This method should set things
        ' in motion so your service can do its work.

        _accLog = New ActivityLogger
        _accLog.WriteToActivityLog("Synchronisation Service Started", "Synchronisation Service")
        _accLog = Nothing

        loaderTimer.Enabled = True

    End Sub

    Protected Overrides Sub OnStop()
        ' Add code here to perform any tear-down necessary to stop your service.

        If ServiceLoadingNow = False Then

            loaderTimer.Enabled = False

            _accLog = New ActivityLogger
            _accLog.WriteToActivityLog("Synchronisation Service Stoped", "Synchronisation Service")
            _accLog = Nothing

        End If

    End Sub

    Protected Overrides Sub OnPause()
        loaderTimer.Enabled = False

        _accLog = New ActivityLogger
        _accLog.WriteToActivityLog("Synchronisation Service Paused", "Synchronisation Service")
        _accLog = Nothing

    End Sub

    Protected Overrides Sub OnContinue()

        loaderTimer.Enabled = True

        _accLog = New ActivityLogger
        _accLog.WriteToActivityLog("Synchronisation Service Resumed", "Synchronisation Service")
        _accLog = Nothing

    End Sub


    Private Sub OnTimedEvent(ByVal source As Object, ByVal e As ElapsedEventArgs)


        If ServiceLoadingNow = False Then

            ServiceLoadingNow = True

            Dim loadfactory As New bc_lo_loadfactory("S")

            '**** Testing
            'Me.EventLog.WriteEntry("Test Testing")
            'loadfactory.Testing()
            '**** Testing

            
            loadfactory.CheckAndLoad()
            loadfactory = Nothing

            ServiceLoadingNow = False

        End If

    End Sub


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub EventLog1_EntryWritten(ByVal sender As System.Object, ByVal e As System.Diagnostics.EntryWrittenEventArgs) Handles EventLog1.EntryWritten

    End Sub
End Class
