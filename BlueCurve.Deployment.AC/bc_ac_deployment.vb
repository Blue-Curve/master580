Imports BlueCurve.Core.CS
'==========================================
' Bluecurve Limited 2012
' Module:         Deployment
' Type:           AP
' Desciption:     Assemble Main Entry Point
' Public Methods: Main
'                 
' Version:        1.0
' Change history:
'
'==========================================
Module bc_ac_deployment

    Function Main() As Int32
        bc_cs_central_settings.ApplicationName = bc_cs_central_settings.DEPLOYMENT_MANAGER_APPLICATION_NAME
        Dim bads As New bc_ac_deployment_start

        REM Console.WriteLine(bads.ErrorCode.ToString)

        Return bads.ErrorCode
    End Function

End Module
