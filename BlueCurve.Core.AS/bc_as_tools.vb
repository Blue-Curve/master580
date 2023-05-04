Imports System.IO
Imports System.Xml
Imports System.Windows.Forms
Imports BlueCurve.Core.AS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.CS

Public Module bc_as_tools

    Public Function QueryDatabase(ByVal strSqlCommand As String, Optional ByVal boolIsXml As Boolean = False) As bc_om_sql

        Dim METHOD_NAME As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        Dim log = New bc_cs_activity_log("bc_am_dd_tools", METHOD_NAME, bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim osql As New bc_om_sql(strSqlCommand)
        osql.IsXml = boolIsXml

        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
            osql.tmode = bc_cs_soap_base_class.tREAD
            osql.transmit_to_server_and_receive(osql, True)
        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            osql.db_read()
        End If

        log = New bc_cs_activity_log("bc_am_dd_tools", METHOD_NAME, bc_cs_activity_codes.TRACE_EXIT, "")

        Return osql

    End Function

    Public Sub SetLockMessageDisplayed()
        REM FIL Drop 5
        REM this nees to handle different authentification methods as well else it can error
        Dim osql As bc_om_sql = QueryDatabase("exec bc_core_set_lock_message_displayed """ & bc_cs_central_settings.logged_on_user_name & """", True)
        REM END FIL DROP 5
    End Sub

    Public Function GetLockMessage() As String
        Try
            Dim osql As bc_om_sql = QueryDatabase("exec bc_core_get_lock_message """ & bc_cs_central_settings.logged_on_user_name & """", True)
            If osql.success Then
                Dim xdSystemStatus As New XmlDocument
                xdSystemStatus.LoadXml(osql.results(0, 0))
                If Not xdSystemStatus.ChildNodes(0).Attributes("system_lockdown") Is Nothing AndAlso xdSystemStatus.ChildNodes(0).Attributes("system_lockdown").Value = 1 Then
                    Dim strLockMessage As String = xdSystemStatus.ChildNodes(0).Attributes("system_lockdown_message").Value
                    If strLockMessage Is Nothing Then
                        strLockMessage = ""
                    Else
                        strLockMessage = strLockMessage & ControlChars.Lf
                    End If
                    Return strLockMessage & "The system will be locked at " & CType(xdSystemStatus.ChildNodes(0).Attributes("system_lockdown_datetime").Value, DateTime).ToString("h:mm tt") & "."
                Else
                    'Error
                    Return ""
                End If
            Else
                'Error
                Return ""
            End If
        Catch

        End Try

    End Function

    Public Function GetDatabaseStatus() As Boolean
        REM PR changes this June 2012 so it is backwardly compatible
      
        Dim osql As bc_om_sql = QueryDatabase("exec bc_core_get_system_status " + CStr(bc_cs_central_settings.logged_on_user_id), True)
        If osql.success Then
            Dim xdSystemStatus As New XmlDocument
            xdSystemStatus.LoadXml(osql.results(0, 0))
            If Not xdSystemStatus.ChildNodes(0).Attributes("system_lockdown") Is Nothing Then
                Return CType(xdSystemStatus.ChildNodes(0).Attributes("system_lockdown").Value, Boolean)
            Else
                'Error
                Return False
            End If
        Else
            'Error
            Return False
        End If

    End Function

End Module

'Public Class BCListViewItemSorter

'    'List view Column Sorter 
'    'Steve Wooderson 21/02/2013

'    Implements IComparer

'    Private sort_column As Integer
'    Private sort_numeric As Boolean = False
'    Private sort_desending As Boolean = False

'    Public Property column() As Integer
'        Get
'            Return sort_column
'        End Get
'        Set(ByVal value As Integer)
'            sort_column = value
'        End Set
'    End Property

'    Public Property Numeric() As Boolean
'        Get
'            Return sort_numeric
'        End Get
'        Set(ByVal value As Boolean)
'            sort_numeric = value
'        End Set
'    End Property

'    Public Property Desending() As Boolean
'        Get
'            Return sort_desending
'        End Get
'        Set(ByVal value As Boolean)
'            sort_desending = value
'        End Set
'    End Property


'    Public Sub New(ByVal columnIndex As Integer)
'        column = columnIndex
'    End Sub

'    Public Function Compare(ByVal x As Object, ByVal y As Object) As Integer Implements System.Collections.IComparer.Compare

'        Dim listx, listy As ListViewItem

'        If Desending Then
'            listy = CType(x, ListViewItem)
'            listx = CType(y, ListViewItem)

'        Else
'            listx = CType(x, ListViewItem)
'            listy = CType(y, ListViewItem)
'        End If

'        If Numeric Then

'            Dim valx, valy As Double
'            valx = Val(listx.SubItems(column).Text)
'            valy = Val(listy.SubItems(column).Text)

'            Return Decimal.Compare(valx, valy)

'        Else
'            Return String.Compare(listx.SubItems(column).Text, listy.SubItems(column).Text)
'        End If

'    End Function
'End Class