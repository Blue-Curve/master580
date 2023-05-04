Public Class bc_am_pr_splash_screen
    Sub New()


        InitializeComponent()


    End Sub

    Public Overrides Sub ProcessCommand(ByVal cmd As System.Enum, ByVal arg As Object)
    End Sub

    Public Enum SplashScreenCommand
        updatecaptions = 1
    End Enum


    
End Class
