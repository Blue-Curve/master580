
Imports System.Exception
Imports System.IO
Imports System.Diagnostics



Module bc_lo_program

    '''<summary>
    ''' Module:         BlueCurveLoader
    ''' Type:           lo
    ''' Desciption:     Start up
    ''' Version:        1.0
    ''' Change history:
    ''' </summary>


    Public _accLog As ActivityLogger
    Public ServiceLoadingNow As Boolean = False

    <MTAThread()> _
    Public Sub Main()

        Dim args As String = ""

        For Each argument As String In My.Application.CommandLineArgs
            args += argument
        Next

        If args = "" Then
            'Run as a Service

            'ServiceBase.Run(new MessageService());
            Dim ServicesToRun() As System.ServiceProcess.ServiceBase
            ServicesToRun = New System.ServiceProcess.ServiceBase() {New BlueCurveLoader}
            System.ServiceProcess.ServiceBase.Run(ServicesToRun)

            '   ServicesToRun = New System.ServiceProcess.ServiceBase () {New Service1, New MySecondUserService}

        ElseIf args = "-win" Then
            'Run as windows application
            Dim loadfactory As New bc_lo_loadfactory("M")

            _accLog = New ActivityLogger
            _accLog.WriteToActivityLog("Manual Synchronisation started", "Manual Synchronisation")
            _accLog = Nothing

            '**** Testing
            'loadfactory.Testing()
            '**** Testing

            loadfactory.CheckAndLoad()

            loadfactory = Nothing

            _accLog = New ActivityLogger
            _accLog.WriteToActivityLog("Manual Synchronisation ended", "Manual Synchronisation")
            _accLog = Nothing

        End If

    End Sub

End Module
