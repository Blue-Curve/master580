Imports BlueCurve.Core.CS
Imports System.Collections
Imports System.IO
Imports System.Data.OleDb
Imports System.Data.sqlclient
Imports System.Data
Public Class ComponentiserConstants

    'Declare constants for componentiser
    Public Shared TableTextConverToRTF As Boolean
    Public Shared MainTextConverToRTF As Boolean
    Public Shared TextConverToRTF As Boolean
    Public Shared ShowErrorMessage As Boolean
    Public Shared LogActivityFile As Boolean = False
    Public Shared CaptureHeightWidthInfoofTable As Boolean = False
    Public Shared TableHasMergedCells As Boolean
    Public Shared TableHasImages As Boolean
    Public Shared PubTypeId As String
    Public Shared CentralRepository As String
    Public Shared CaptureTableCellRGBValue As Boolean = False
    Public Shared RedR As Integer = 0
    Public Shared RedG As Integer = 0
    Public Shared RedB As Integer = 0
    Public Shared GreenR As Integer = 0
    Public Shared GreenG As Integer = 0
    Public Shared GreenB As Integer = 0

    Public Shared Function RemoveSpecialChars(ByVal strValue As String) As String
        Dim i As Integer
        If Not strValue Is Nothing AndAlso strValue.Length > 0 Then
            Dim strReplacedValue As String = strValue.Trim
            Try
                For i = 1 To 255
                    If Len(strValue) = 0 Then
                        Return ""
                    End If
                    If i < 32 Or i > 192 Or i = 133 Or i = 135 Or (i >= 139 And i <= 141) Or i = 150 Or i = 151 Or _
                                   (i >= 154 And i <= 158) Or i = 168 Or i = 171 Or i = 172 Or i = 177 Or _
                                         i = 182 Or i = 187 Then
                        If i <> 199 Or i <> 200 Then
                            strReplacedValue = Replace(strReplacedValue, Chr(i).ToString, "", 1, , CompareMethod.Binary)
                        End If
                    End If
                Next
                Return strReplacedValue
            Catch ex As Exception
                If ComponentiserConstants.ShowErrorMessage Then
                    Dim db_err As New bc_cs_error_log("bc_components_dal", "RemoveSpecialChars ", bc_cs_error_codes.USER_DEFINED, ex.Message)
                Else
                    Dim otrace As New bc_cs_activity_log("bc_components_dal", "RemoveSpecialChars", bc_cs_activity_codes.COMMENTARY, ex.Message)
                End If
                Return ""
            End Try
        Else
            Return ""
        End If
    End Function
    Public Shared Sub Set_Central_repository()
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("Componentiser Constants", "set_central_repository", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim sqlcn As New SqlConnection
        Dim sp_name As String
        Dim gbc_db As bc_cs_db_services
        sp_name = "dbo.bcc_pub_type_repository"
        gbc_db = New bc_cs_db_services(False)
        sqlcn.ConnectionString = gbc_db.GetConnectionString(Nothing)
        Try
            '   Dim ocommentary = New bc_cs_activity_log("bc_am_component", "executesp_for_table_component", bc_cs_activity_codes.COMMENTARY, "Attempting to execute stored procedure: " + CStr(sp_name) + " for entity_id:" + Locator + " for entity_id:" + CStr(Rowno) + " for entity_id:" + CStr(Colno))
            sqlcn.Open()
            Dim cmd As New SqlCommand(sp_name, sqlcn)
            cmd.CommandType = Data.CommandType.StoredProcedure
            cmd.Parameters.AddWithValue("@pub_type_id", ComponentiserConstants.PubTypeId)
            ComponentiserConstants.CentralRepository = cmd.ExecuteScalar()
        Catch ex As Exception
            ComponentiserConstants.CentralRepository = ""
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("Componentiser Constants", "set_central_repository", bc_cs_error_codes.DB_ERR, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("Componentiser Constants", "set_central_repository", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("Componentiser Constants", "set_central_repository", bc_cs_activity_codes.TRACE_EXIT, "")
            If Not (sqlcn Is Nothing) Then
                sqlcn.Close()
            End If
        End Try
    End Sub
End Class

