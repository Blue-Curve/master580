
Imports System.Windows.Forms
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM

Public Class bc_am_audit_details

    Private DetailUniverse As String
    Private DetailBatch As String
    Private DetailElapsed As String
    Private DetailResult As String

    Public Property DetailLogUniverse() As String
        Get
            DetailLogUniverse = DetailUniverse
        End Get
        Set(ByVal value As String)
            DetailUniverse = value
        End Set
    End Property

    Public Property DetailLogBatch() As String
        Get
            DetailLogBatch = DetailBatch
        End Get
        Set(ByVal value As String)
            DetailBatch = value
        End Set
    End Property

    Public Property DetailLogElapsed() As String
        Get
            DetailLogElapsed = DetailElapsed
        End Get
        Set(ByVal value As String)
            DetailElapsed = value
        End Set
    End Property

    Public Property DetailLogResult() As String
        Get
            DetailLogResult = DetailResult
        End Get
        Set(ByVal value As String)
            DetailResult = value
        End Set
    End Property

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Private agg_audit_details As bc_om_insight_aggregation_log_deatils

    Private Sub bc_am_audit_details_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        load_audit_details()
    End Sub
    Private Sub load_audit_details()

        Dim slog = New bc_cs_activity_log("bc_am_in_audit_details", "bc_am_audit_details_Load", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Me.Cursor = Cursors.WaitCursor

            'Load the datas
            agg_audit_details = New bc_om_insight_aggregation_log_deatils(DetailUniverse, DetailBatch)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                agg_audit_details.db_read()
            Else
                agg_audit_details.tmode = bc_cs_soap_base_class.tREAD
                If agg_audit_details.transmit_to_server_and_receive(agg_audit_details, True) = False Then
                    Exit Sub
                End If
            End If

            uxUniverse.Text = DetailUniverse
            If agg_audit_details.agg_audit_details.Count > 1 Then
                'load textboxes
                uxBatch.Text = agg_audit_details.agg_audit_details(0).batch_date
                uxElapsed.Text = agg_audit_details.agg_audit_details(0).elapsed10ths / 10
                If agg_audit_details.agg_audit_details(0).warnings = 1 Then
                    uxResult.Text = agg_audit_details.agg_audit_details(0).successtext + " with " + CStr(agg_audit_details.agg_audit_details(0).warnings) + " warning"
                ElseIf agg_audit_details.agg_audit_details(0).warnings > 1 Then
                    uxResult.Text = agg_audit_details.agg_audit_details(0).successtext + " with " + CStr(agg_audit_details.agg_audit_details(0).warnings) + " warnings"
                Else
                    uxResult.Text = agg_audit_details.agg_audit_details(0).successtext
                End If
            End If

            'Load listview
            Me.uxListAggAuditDetails.Items.Clear()

            'insert into bc_core_aggs_log_type values(0,'Error')
            'insert into bc_core_aggs_log_type values(1,'Trace')
            'insert into bc_core_aggs_log_type values(2,'Info')
            'insert into bc_core_aggs_log_type values(3,'Warning')
            'insert into bc_core_aggs_log_type values(4,'Universe List')
            'insert into bc_core_aggs_log_type values(5,'Excluded List')
            'insert into bc_core_aggs_log_type values(6,'Number Aggregated')
            'insert into bc_core_aggs_log_type values(7,'Number Excluded')
            'insert into bc_core_aggs_log_type values(8,'Calculation Name')
            'insert into bc_core_aggs_log_type values(9,'Volume')

            load_audit_list(True)

            'For j = 0 To agg_audit_details.agg_audit_details.Count - 1
            '    lvw = New ListViewItem(CStr(agg_audit_details.agg_audit_details(j).log_date))
            '    'lvw.SubItems.Add(agg_audit_details.agg_audit_details(j).batch_date)
            '    lvw.SubItems.Add(agg_audit_details.agg_audit_details(j).type_name)
            '    lvw.SubItems.Add(agg_audit_details.agg_audit_details(j).log_comment)
            '    lvw.SubItems.Add(agg_audit_details.agg_audit_details(j).log_error)
            '    If Me.RadioButton2.Checked = True Then
            '        Me.uxListAggAuditDetails.Items.Add(lvw)
            '    ElseIf Me.RadioButton1.Checked = True And agg_audit_details.agg_audit_details(j).type_name <> "Trace" Then
            '        Me.uxListAggAuditDetails.Items.Add(lvw)
            '    ElseIf Me.RadioButton2.Checked = True And agg_audit_details.agg_audit_details(j).type_name = Me.Cfilter.Text Then
            '        Me.uxListAggAuditDetails.Items.Add(lvw)
            '    End If
            'Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_audit_details_Load", "uxExport_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_audit_details_Load", "uxExport_Click", bc_cs_activity_codes.TRACE_EXIT, "")
            Me.Cursor = Cursors.Default
        End Try

    End Sub
    Private Sub load_audit_list(Optional ByVal first As Boolean = False)

        If agg_audit_details Is Nothing Then
            Exit Sub
        End If
        Dim lvw As ListViewItem
        Dim found As Boolean
        Me.uxListAggAuditDetails.Items.Clear()
        If first = True Then
            Me.Cfilter.Items.Clear()
        End If
        For j = 0 To agg_audit_details.agg_audit_details.Count - 1
            lvw = New ListViewItem(CStr(agg_audit_details.agg_audit_details(j).log_date))
            'lvw.SubItems.Add(agg_audit_details.agg_audit_details(j).batch_date)
            lvw.SubItems.Add(agg_audit_details.agg_audit_details(j).type_name)
            lvw.SubItems.Add(agg_audit_details.agg_audit_details(j).log_comment)
            lvw.SubItems.Add(agg_audit_details.agg_audit_details(j).log_error)
            If Me.RadioButton2.Checked = True Then
                Me.uxListAggAuditDetails.Items.Add(lvw)
            ElseIf Me.RadioButton1.Checked = True And agg_audit_details.agg_audit_details(j).type_name <> "Trace" Then
                Me.uxListAggAuditDetails.Items.Add(lvw)
            ElseIf Me.RadioButton3.Checked = True And agg_audit_details.agg_audit_details(j).type_name = Me.Cfilter.Text Then
                Me.uxListAggAuditDetails.Items.Add(lvw)
            End If
            found = False
            If first = True Then
                For i = 0 To Me.Cfilter.Items.Count - 1
                    If Me.Cfilter.Items(i) = agg_audit_details.agg_audit_details(j).type_name Then
                        found = True
                        Exit For
                    End If
                Next

                If found = False Then
                    Me.Cfilter.Items.Add(agg_audit_details.agg_audit_details(j).type_name)
                End If
            End If
        Next
    End Sub


    Private Sub uxListAggAuditDetails_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub uxExport_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxExport.Click

        Dim oxlApp As Object 'Excel.Application4.        
        Dim oxlOutWBook As Object 'Excel.Workbook5.     


        Dim slog = New bc_cs_activity_log("bc_am_in_audit_details", "uxExport_Click", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Me.Cursor = Cursors.WaitCursor
            oxlApp = bc_ao_in_excel.CreateNewExcelInstance
            oxlOutWBook = oxlApp.Workbooks.Add
            Dim oexcel As New bc_ao_in_excel(oxlApp)

            'Open the Blue Curve addins 
            'Steve wooderson 31/03/2014
            'oexcel.open_addins(oxlApp)

            Dim values(uxListAggAuditDetails.Items.Count, uxListAggAuditDetails.Items(0).SubItems.Count - 1) As String
            For i = 0 To uxListAggAuditDetails.Columns.Count() - 1
                values(0, i) = uxListAggAuditDetails.Columns(i).Text
            Next
            For i = 0 To uxListAggAuditDetails.Items.Count - 1
                For j = 0 To uxListAggAuditDetails.Items(i).SubItems.Count - 1
                    values(i + 1, j) = uxListAggAuditDetails.Items(i).SubItems(j).Text
                Next j
            Next i

            oexcel.bc_array_excel_export(values)
            oxlApp.application.visible = True

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_audit_details", "uxExport_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_audit_details", "uxExport_Click", bc_cs_activity_codes.TRACE_EXIT, "")
            Me.Cursor = Cursors.Default
        End Try


    End Sub

    Private Sub uxOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxOk.Click
        Me.Close()
    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub

    Private Sub uxListAggAuditDetails_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxListAggAuditDetails.SelectedIndexChanged

    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton1.CheckedChanged
        If Me.RadioButton1.Checked = True Then
            load_audit_list()
            Me.Cfilter.Enabled = False
            Me.Cfilter.SelectedIndex = -1

        End If

    End Sub

    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton2.CheckedChanged
        If Me.RadioButton2.Checked = True Then
            Me.Cfilter.Enabled = False
            Me.Cfilter.SelectedIndex = -1

            load_audit_list()

        End If

    End Sub

    Private Sub Cfilter_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cfilter.SelectedIndexChanged
        If Me.Cfilter.SelectedIndex <> -1 Then
            load_audit_list()
        End If
    End Sub

    Private Sub RadioButton3_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RadioButton3.CheckedChanged
        If Me.RadioButton3.Checked = True Then
            Me.uxListAggAuditDetails.Items.Clear()
            Me.Cfilter.Enabled = True
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        load_audit_details()
    End Sub
End Class