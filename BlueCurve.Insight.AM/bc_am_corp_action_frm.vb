Imports System.Xml
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS

Public Class bc_am_corp_action_frm

    Dim ClassId As Long
    Dim bc_cs_central_settings As New bc_cs_central_settings(True)
    Dim EventList As New bc_om_corp_events()
    Dim EntityList As New bc_om_entities()

    Public SelectedAction As bc_om_corp_event
    Public SelectedEntity As bc_om_entity
    Public SelectedEntityList As New ArrayList
    Public AoExcel As bc_ao_in_excel
    Public BuildAction As Boolean = False
    Public OFunctions = New bc_om_ef_functions
    Public UseClassIds As New ArrayList
    Public UseClassTypes As New ArrayList
    Public PrimaryText As String
    Public PrimaryFocus As Integer
    Public DeletionClass As Integer

    Private TabPageAdjustment As TabPage
    Private TabpageDelete As TabPage


    Public Sub New(ByVal aoObject As Object)

        Dim slog = New bc_cs_activity_log("bc_am_corp_action_frm", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As New bc_cs_activity_log("bc_am_corp_action_frm", "new", bc_cs_activity_codes.COMMENTARY, "Load corporate action build form")

        Try

            AoExcel = New bc_ao_in_excel(aoObject)
            Me.InitializeComponent()

            REM set up tab control
            Me.TabPageAdjustment = Me.uxTabCorpActions.TabPages(1)
            Me.TabpageDelete = Me.uxTabCorpActions.TabPages(2)
            Me.uxTabCorpActions.TabPages.RemoveAt(2)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_corp_action_frm", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub

    Public Sub LoadEntities()

        Try

            Me.txtStock.Text = ""
            uxListStock.BeginUpdate()
            Me.uxListStock.Items.Clear()
            For i = 0 To OFunctions.entities.Count - 1
                If Len(Me.BlueCurve_TextSearch1.SearchText) = 0 Or (Me.BlueCurve_TextSearch1.SearchCurrentAttribute = "Name" And InStr(UCase(OFunctions.entities(i).name), UCase(Me.BlueCurve_TextSearch1.SearchText)) <> 0) Then
                    If OFunctions.entities(i).class_id = PrimaryFocus Then
                        Me.uxListStock.Items.Add(OFunctions.entities(i).name)
                    End If
                End If
            Next
            uxListStock.EndUpdate()
            If Me.uxListStock.Items.Count > 0 Then
                Me.uxListStock.SelectedIndex = 0
            Else
                Me.uxListStockSelected.Items.Clear()
                BuildEntityList()
            End If

            'If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
            '    entity_list.db_read()
            'ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
            '    entity_list.tmode = bc_cs_soap_base_class.tREAD
            '    If entity_list.transmit_to_server_and_receive(entity_list, True) = False Then
            '        Exit Sub
            '    End If
            'Else
            '    Dim omsg As New bc_cs_message("Blue Curve", "Cannot connect to " + bc_cs_central_settings.soap_server + " application cannot be launched.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            '    Exit Sub
            'End If

            ''entity_list = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + "bc_entities.dat")

            'For i = 0 To entity_list.entity.Count - 1
            '    'For i = 0 To obc_objects.obc_entities.entity.Count - 1
            '    'If entity_list.entity(i).class_id = class_id Then
            '    If entity_list.entity(i).class_name = "Instrument" Then
            '        Me.liststock.Items.Add(entity_list.entity(i).name)
            '        Me.liststock.SelectedIndex = 0
            '    End If
            'Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_corp_action_frm", "LoadEntities", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub

    Public Sub LoadCorpEvents()

        Try

            If EventList.ActionType.Count = 0 Then
                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                    EventList.db_read()
                ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                    EventList.tmode = bc_cs_soap_base_class.tREAD
                    If EventList.transmit_to_server_and_receive(EventList, True) = False Then
                        Exit Sub
                    End If
                Else
                    Dim omsg As New bc_cs_message("Blue Curve", "Cannot connect to " + bc_cs_central_settings.soap_server + " application cannot be launched.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            End If

            Me.uxActionStock.BeginUpdate()
            Me.uxActionStock.Items.Clear()
            For i = 0 To EventList.ActionType.Count - 1
                If EventList.ActionType(i).classid = PrimaryFocus Then
                    Me.uxActionStock.Items.Add(EventList.ActionType(i).description)
                End If
            Next
            Me.uxActionStock.EndUpdate()

            If Me.uxActionStock.Items.Count > 0 Then
                Me.uxActionStock.SelectedIndex = 0
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_corp_action_frm", "LoadCorpEvents", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub

    Public Sub UpdateAdjustmentDisplay()

        Dim slog As New bc_cs_activity_log("bc_am_corp_action_frm", "UpdateAdjustmentDisplay", bc_cs_activity_codes.TRACE_ENTRY, "")

        If IsNothing(SelectedAction) Then Exit Sub

        ' Set up the controls to use
        Me.uxExDate.Enabled = False
        Me.uxNumber.Enabled = False
        Me.uxMoney.Enabled = False
        Me.uxRatioB.Enabled = False

        Try

            For i = 0 To SelectedAction.DataInputs.input.Count - 1

                If SelectedAction.DataInputs.input(i).inputcode = "date" Then
                    Me.uxLblDate.Text = SelectedAction.DataInputs.input(i).inputtype
                    Me.uxExDate.Enabled = True
                End If

                If SelectedAction.DataInputs.input(i).inputcode = "numb" Then
                    Me.uxNumber.Enabled = True
                    Me.uxLlblNumber.Text = SelectedAction.DataInputs.input(i).inputtype
                End If

                If SelectedAction.DataInputs.input(i).inputcode = "mony" Then
                    Me.uxMoney.Enabled = True
                    Me.uxLlblMoney.Text = SelectedAction.DataInputs.input(i).inputtype
                End If

                If SelectedAction.DataInputs.input(i).inputcode = "rati" Then
                    Me.uxRatioB.Enabled = True
                    Me.uxLblRatio.Text = SelectedAction.DataInputs.input(i).inputtype
                End If

            Next

            If Me.uxNumber.Enabled = False Then Me.uxNumber.Text = ""
            If Me.uxMoney.Enabled = False Then Me.uxMoney.Text = ""
            If Me.uxRatioB.Enabled = False Then Me.uxRatioB.Text = ""

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_corp_action_frm", "UpdateAdjustmentDiplay", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

    Public Sub UpdateDeleteDisplay()
        REM get the required inputs from the user to do a deletion 
        Dim slog As New bc_cs_activity_log("bc_am_corp_action_frm", "UpdateDeleteDisplay", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Me.uxCfType2.Items.Clear()
            Me.uxCfType2.Sorted = False

            If Me.uxCfType.Text = "Market" Then
                Me.uxCfType2.Items.Add("Instrument")
            Else
                Me.uxCfType2.Items.Add(Me.uxCfType.Text)
            End If
            Me.uxCfType2.SelectedIndex = 0
            Me.uxDateFrom.Value = DateValue(Now)
            Me.uxDateTo.Value = DateValue(Now)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_corp_action_frm", "UpdateDeleteDisplay", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub


    Private Sub bc_am_corp_action_frm_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

        Dim classes As New bc_om_entity_classes

        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            OFunctions.db_read()
        Else
            OFunctions.tmode = bc_cs_soap_base_class.tREAD
            If OFunctions.transmit_to_server_and_receive(OFunctions, True) = False Then
                Exit Sub
            End If
        End If

        BlueCurve_TextSearch1.SearchSetup()

        LoadTypes()

        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            classes.db_read()
        Else
            classes.tmode = bc_cs_soap_base_class.tREAD
            If classes.transmit_to_server_and_receive(classes, True) = False Then
                Exit Sub
            End If
        End If

        REM classes.db_read()
        For i = 0 To classes.classes.Count - 1
            If classes.classes(i).class_name = PrimaryText Then
                PrimaryFocus = classes.classes(i).class_id
            End If
        Next i
        classes = Nothing

        'Me.caDate.Value = DateAdd(DateInterval.Day, -1, DateValue(Now))
        'Me.caDate.MaxDate = DateAdd(DateInterval.Day, +1, DateValue(Now))
        Me.uxExDate.Value = DateValue(Now)

        LoadClasses()
        LoadEntities()
        LoadCorpEvents()
        UpdateAdjustmentDisplay()

    End Sub

    Public Sub BuildCorpActionWorkbook(ByVal corpAction As bc_om_corp_action)

        Dim test As String

        Try
            'Write data to Excel
            test = AoExcel.get_sheet_name()

            AoExcel.BuildCorpAction(corpAction)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_corp_action_frm", "BuildCorpActionWorkbook", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

    Private Sub uxTabAdjustment_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxTabAdjustment.Enter

        UpdateAdjustmentDisplay()
        Me.bleftgrey.Visible = True
        Me.Btnback.Visible = True
        btnforwardgrey.Visible = False
        btnnext.Visible = False
        bok.Visible = True
        bokdisabled.Visible = True

    End Sub
    Private Sub uxTaAction_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxTaAction.Enter

        Me.bleftgrey.Visible = False
        Me.Btnback.Visible = False
        btnforwardgrey.Visible = True
        btnnext.Visible = True
        bok.Visible = False
        bokdisabled.Visible = False

    End Sub

    Private Sub uxTabDelete_Enter(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxTabDelete.Enter

        Me.bleftgrey.Visible = True
        Me.Btnback.Visible = True
        btnforwardgrey.Visible = False
        btnnext.Visible = False
        bok.Visible = True
        bokdisabled.Visible = True

    End Sub

    Private Sub uxNumber_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxNumber.KeyPress
        If Asc(e.KeyChar) < 48 Or Asc(e.KeyChar) > 57 Then
            e.Handled = True
        End If
        If (Asc(e.KeyChar) = 8) Then
            e.Handled = False
        End If

    End Sub

    Private Sub uxNumber_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxNumber.LostFocus
        uxNumber.Text = (CLng(Replace(uxNumber.Text, ",", ""))).ToString("#,###")
    End Sub

    Private Sub uxNumber_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxNumber.TextChanged

        'caNumber.Text = (CLng(Replace(caNumber.Text, ",", ""))).ToString("#,###")

    End Sub

    Private Sub uxRatioB_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxRatioB.KeyPress
        If (Asc(e.KeyChar) < 48) _
               Or (Asc(e.KeyChar) > 57) Then
            e.Handled = True
        End If
        If Asc(e.KeyChar) = 8 Then
            e.Handled = False
        End If
    End Sub

    Private Sub uxMoney_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxMoney.KeyPress

        Select Case Asc(e.KeyChar)
            Case 8, 45, 46, 48 To 57    '/ BackSpace, -, period and 0-9 permitted only.
                e.Handled = False
            Case Else
                e.Handled = True
        End Select
    End Sub

    Private Sub bok_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bok.MouseDown
        Me.bok.Visible = False
    End Sub

    Private Sub bok_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bok.MouseUp

        Dim workNumber As String
        Dim workMoney As String
        Dim workRatio As String
        Dim workExdate As Date = Nothing

        workNumber = Me.uxNumber.Text
        workMoney = Me.uxMoney.Text
        workRatio = Me.uxRatioB.Text

        If workNumber = "" Then
            workNumber = "0"
        End If
        If workMoney = "" Then
            workMoney = "0"
        End If
        If workRatio = "" Then
            workRatio = "0"
        End If

        If Me.uxExDate.Enabled = True Then
            workExdate = Me.uxExDate.Value
        End If

        Try

            Me.bok.Visible = True
            Me.Refresh()
            Me.Cursor = Cursors.WaitCursor

            REM Validate
            If Me.SelectedAction.EventType = "DELETE" Then

                REM Deletion Validation

                If Me.uxListStockSelected.Items.Count < 1 Then
                    Dim bc_cs_message As New bc_cs_message("Blue Curve Insight", "Please select an Entity.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

                If Me.uxDateFrom.Checked = False Then
                    Dim bc_cs_message As New bc_cs_message("Blue Curve Insight", "A From Date must be selected.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

                If Me.uxDateTo.Checked = False Then
                    Dim bc_cs_message As New bc_cs_message("Blue Curve Insight", "A To Date must be selected.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

                If Me.uxDateFrom.Value > Me.uxDateTo.Value Then
                    Dim bc_cs_message As New bc_cs_message("Blue Curve Insight", "Date From can not be older than date To.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

            Else

                REM Adjustment Validation
                If Me.uxListStockSelected.Items.Count < 1 Then
                    Dim bc_cs_message As New bc_cs_message("Blue Curve Insight", "Please select an Entity.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

                If Me.uxActionStock.SelectedIndex = -1 Then
                    Dim bc_cs_message As New bc_cs_message("Blue Curve Insight", "Please select an Action Type.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

                If Me.uxNumber.Enabled = True And Val(workNumber) = 0 Then
                    Dim bc_cs_message As New bc_cs_message("Blue Curve Insight", uxLlblNumber.Text & " can not be 0.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

                If Me.uxExDate.Enabled = True And Me.uxExDate.Checked = False Then
                    Dim bc_cs_message As New bc_cs_message("Blue Curve Insight", "An Ex-Date must be selected.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If

                If Me.uxRatioB.Enabled = True And Val(workRatio) = 0 Then
                    Dim bc_cs_message As New bc_cs_message("Blue Curve Insight", uxLblRatio.Text & " can not be 0.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                If Me.uxMoney.Enabled = True And Val(workMoney) = 0 Then
                    Dim bc_cs_message As New bc_cs_message("Blue Curve Insight", uxLlblMoney.Text & " can not be 0.", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
            End If

            REM Do the work
            SelectedEntity = SelectedEntityList.Item(0)
            Dim corp_action As New bc_om_corp_action(Me.PrimaryFocus, Me.SelectedEntity.id, Me.SelectedAction.Id, CLng(workNumber), workExdate, CDbl(workMoney), CInt(workRatio))
            BuildAction = True
            Me.Close()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_corp_action_frm", "bok_MouseUp", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default

        End Try

    End Sub

    Private Sub bcancel_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bcancel.MouseDown
        Me.bcancel.Visible = False
    End Sub

    Private Sub bcancel_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bcancel.MouseUp
        Dim ocommentary As New bc_cs_activity_log("bc_am_corp_action_frm", "bcancel_MouseUp", bc_cs_activity_codes.COMMENTARY, "Cancel corporate action")
        Me.Close()
    End Sub

    Private Sub btnforwardgrey_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnforwardgrey.MouseDown
        Me.btnforwardgrey.Visible = False
    End Sub

    Private Sub btnforwardgrey_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles btnforwardgrey.MouseUp
        Me.btnforwardgrey.Visible = True

        If SelectedAction.EventType = "DELETE" Then
            Me.uxTabCorpActions.SelectedTab = uxTabDelete
        Else
            Me.uxTabCorpActions.SelectedTab = uxTabAdjustment
        End If

    End Sub

    Private Sub bleftgrey_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bleftgrey.MouseDown
        Me.bleftgrey.Visible = False
    End Sub

    Private Sub bleftgrey_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles bleftgrey.MouseUp
        Me.bleftgrey.Visible = True
        Me.uxTabCorpActions.SelectedTab = uxTaAction

    End Sub


    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub BuildEntityList()

        Dim EntityDescription As String

        Me.SelectedEntityList.Clear()

        For x = 1 To uxListStockSelected.Items.Count
            EntityDescription = uxListStockSelected.Items(x - 1)

            For i = 0 To OFunctions.entities.Count - 1
                If OFunctions.entities(i).class_id = PrimaryFocus Then
                    If Trim(OFunctions.entities(i).name) = Trim(EntityDescription) Then

                        SelectedEntity = OFunctions.entities(i)
                        Me.SelectedEntityList.Add(SelectedEntity)

                        Exit For
                    End If
                End If
            Next
        Next

    End Sub

    Private Sub LoadClasses()
        Dim j, i As Integer

        Dim del_list As New ArrayList
        del_list.Clear()

        Me.UseClassIds.Clear()
        Me.UseClassTypes.Clear()
        PropUp(PrimaryFocus, 1, UseClassIds, UseClassTypes)

        Me.uxCfClass.Items.Clear()
        Me.uxCfEntity.Items.Clear()
        Me.uxCfClass.Items.Add("None")
        Me.uxCfClass.Sorted = False
        Me.uxCfClass.SelectedIndex = 0

        REM remove duplicates
        For i = 0 To UseClassIds.Count - 1
            del_list.Add(0)
            For j = 0 To UseClassIds.Count - 1
                If j > i Then
                    If UseClassIds(j) = UseClassIds(i) Then
                        del_list(i) = 1
                    End If
                End If
            Next
        Next

        For i = 0 To UseClassIds.Count - 1
            For j = 0 To OFunctions.class_ids.Count - 1
                If del_list(i) = 0 And OFunctions.class_names(j) <> "ROOT" Then

                    If OFunctions.class_ids(j) = UseClassIds(i) Then
                        Me.uxCfClass.Items.Add(OFunctions.class_names(j))
                    End If

                End If
            Next
        Next

    End Sub


    Private Sub LoadTypes()


        'EFG ver
        Me.uxCfType.Items.Clear()
        Me.uxCfType.Sorted = False
        Me.uxCfType.Items.Add("Instrument")
        Me.uxCfType.SelectedIndex = 0
        PrimaryText = "Instrument"
        Me.uxCfType.Items.Add("Issuer")
        Me.uxCfType.Items.Add("Index")
        Me.uxCfType.Items.Add("Market")

        'Me.uxCfType.Items.Clear()
        'Me.uxCfType.Sorted = False

        'Dim e As New bc_am_excel_functions
        'Dim o As Object = e.execute("bc_corp_action_get_classes")

        'Dim xd As XmlDocument = Nothing
        'If o(0, 0) <> "" Then
        '    xd = New XmlDocument
        '    xd.PreserveWhitespace = True
        '    xd.LoadXml(o(0, 0))
        'End If

        'Dim intSelected As Integer = 0
        'For Each xn As XmlNode In xd.GetElementsByTagName("entity_class_tbl")
        '    If Not xn.Attributes()("class_name") Is Nothing Then
        '        Me.uxCfType.Items.Add(xn.Attributes()("class_name").Value)
        '    End If
        '    If Not xn.Attributes()("selected") Is Nothing AndAlso xn.Attributes()("selected").Value = 1 AndAlso Not xn.Attributes()("class_name") Is Nothing Then
        '        PrimaryText = xn.Attributes()("class_name").Value
        '        Me.uxCfType.SelectedIndex = intSelected
        '    End If
        '    intSelected += 1
        'Next

    End Sub

    Private Sub PropUp(ByVal hostClassId As Long, ByVal schemaId As Long, ByRef useClassIds As ArrayList, ByRef useClassTypes As ArrayList)
        Dim i As Integer
        For i = 0 To OFunctions.class_links.Count - 1
            If OFunctions.class_links(i).child_class_id = hostClassId And OFunctions.class_links(i).schema_id = schemaId Then
                useClassIds.Add(OFunctions.class_links(i).parent_class_id)
                useClassTypes.Add("Parent")
                PropUp(OFunctions.class_links(i).parent_class_id, schemaId, useClassIds, useClassTypes)
            End If
        Next
    End Sub

    Private Function LoadPropogatingEntities(ByVal ty As Integer, ByVal pClass As String, ByVal entity As String, ByVal childClass As String) As String
        Dim str As String

        str = "<excel_function>" + vbCr + _
        "<type>" + CStr(ty) + "</type>" + vbCr + _
        "<class_id>" + pClass + "</class_id>" + vbCr + _
        "<entity_id>" + entity + "</entity_id>" + vbCr + _
        "<ass_class_id>" + childClass + "</ass_class_id>" + vbCr + _
        "<schema_id>" + "Internal Schema" + "</schema_id>" + vbCr + _
        "<dimensions>name</dimensions>" + vbCr + _
        "</excel_function>"

        Dim oef As New bc_am_excel_functions
        LoadPropogatingEntities = oef.execute_function(str)

    End Function

    Private Sub uxMoney_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxMoney.LostFocus
        'caMoney.Text = (CLng(Replace(caMoney.Text, ",", ""))).ToString("#,###.##")
    End Sub

    Private Sub uxRatioB_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxRatioB.LostFocus
        'caRatioB.Text = (CLng(Replace(caRatioB.Text, ",", ""))).ToString("#,###")
    End Sub

    Private Sub uxSearchTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSearchTimer.Tick
        uxSearchTimer.Stop()
        LoadEntities()
    End Sub


    Private Sub uxCfType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxCfType.SelectedIndexChanged

        Try
            Me.Cursor = Cursors.WaitCursor

            Dim classes As New bc_om_entity_classes

            PrimaryText = uxCfType.Text
            uxLEntity.Text = LTrim(uxCfType.Text) + ":"
            Application.DoEvents()

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                classes.db_read()
            Else
                classes.tmode = bc_cs_soap_base_class.tREAD
                If classes.transmit_to_server_and_receive(classes, True) = False Then
                    Exit Sub
                End If
            End If

            REM classes.db_read()

            For i = 0 To classes.classes.Count - 1
                If classes.classes(i).class_name = PrimaryText Then
                    PrimaryFocus = classes.classes(i).class_id
                End If
            Next i
            classes = Nothing

            LoadCorpEvents()
            LoadClasses()

            If BlueCurve_TextSearch1.SearchClass <> PrimaryFocus Then
                BlueCurve_TextSearch1.SearchClass = PrimaryFocus
                BlueCurve_TextSearch1.SearchSetup()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_corp_action_frm", "Cftype_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub


    Private Sub uxCfType2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxCfType2.SelectedIndexChanged

        Try
            Me.Cursor = Cursors.WaitCursor

            Dim classes As New bc_om_entity_classes

            If uxCfType2.Text = "All" Then
                DeletionClass = -99
            Else

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    classes.db_read()
                Else
                    classes.tmode = bc_cs_soap_base_class.tREAD
                    If classes.transmit_to_server_and_receive(classes, True) = False Then
                        Exit Sub
                    End If
                End If
                REM classes.db_read()

                For i = 0 To classes.classes.Count - 1
                    If classes.classes(i).class_name = uxCfType2.Text Then
                        DeletionClass = classes.classes(i).class_id
                    End If
                Next i
                classes = Nothing
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_corp_action_frm", "Cftype2_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub uxDateFrom_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDateFrom.ValueChanged

        If uxDateFrom.Checked = True And uxDateTo.Checked = False Then
            uxDateTo.Value = uxDateFrom.Value
            uxDateTo.Checked = False
        End If

    End Sub

    Private Sub uxEntityAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxEntityAdd.Click
        Dim AddItem As Boolean = True

        If Me.uxListStock.SelectedIndex <> -1 Then

            For i = 0 To OFunctions.entities.Count - 1
                If OFunctions.entities(i).class_id = PrimaryFocus Then
                    If Trim(OFunctions.entities(i).name) = Trim(Me.uxListStock.Items(Me.uxListStock.SelectedIndex)) Then

                        For j = 1 To Me.uxListStockSelected.Items.Count
                            If Trim(Me.uxListStockSelected.Items(j - 1)) = Trim(OFunctions.entities(i).name) Then
                                AddItem = False
                            End If
                        Next

                        If AddItem = True Then
                            Me.uxListStockSelected.Items.Add(OFunctions.entities(i).name)
                            BuildEntityList()
                        End If

                        Exit For
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub uxActionStock_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxActionStock.SelectedIndexChanged

        If Me.uxActionStock.SelectedIndex <> -1 Then

            For i = 0 To EventList.ActionType.Count - 1
                If EventList.ActionType(i).classid = PrimaryFocus Then
                    If EventList.ActionType(i).description = Me.uxActionStock.Items(Me.uxActionStock.SelectedIndex) Then
                        Me.txtCorpAction.Text = EventList.ActionType(i).description
                        SelectedAction = EventList.ActionType(i)
                        Exit For
                    End If
                End If
            Next

            'Me.txtCorpAction.Text = event_list.actiontype(Me.actionstock.SelectedIndex).description
            'SelectedAction = event_list.actiontype(Me.actionstock.SelectedIndex)

            If SelectedAction.EventType = "DELETE" Then
                Me.uxListStockSelected.Items.Clear()
                BuildEntityList()
                Me.txtStock.Text = ""
                Me.uxTabCorpActions.TabPages.RemoveAt(1)
                Me.uxTabCorpActions.TabPages.AddRange(New TabPage() {TabpageDelete})
                UpdateDeleteDisplay()
                Me.uxEntityAdd.Enabled = True
                Me.uxEntityAdd.Visible = True
                Me.uxEntityDelete.Enabled = True
                Me.uxEntityDelete.Visible = True
                Me.uxListStockSelected.Visible = True
                Me.uxListStock.Size = New System.Drawing.Size(210, 108)
            Else

                If Me.uxListStock.SelectedIndex <> -1 Then
                    Me.uxListStockSelected.Items.Clear()
                    Me.uxListStockSelected.Items.Add(Me.uxListStock.Items(Me.uxListStock.SelectedIndex))
                    BuildEntityList()
                    Me.txtStock.Text = Me.uxListStock.Items(Me.uxListStock.SelectedIndex)
                End If

                Me.uxTabCorpActions.TabPages.RemoveAt(1)
                Me.uxTabCorpActions.TabPages.AddRange(New TabPage() {TabPageAdjustment})
                UpdateAdjustmentDisplay()
                Me.uxEntityAdd.Enabled = False
                Me.uxEntityDelete.Enabled = False
                Me.uxEntityAdd.Visible = False
                Me.uxEntityDelete.Visible = False
                Me.uxListStockSelected.Visible = False
                Me.uxListStock.Size = New System.Drawing.Size(454, 108)
            End If

        End If

    End Sub

    Private Sub uxCfClass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxCfClass.SelectedIndexChanged
        Try

            If Me.uxCfClass.SelectedIndex = -1 Then
                Exit Sub
            End If
            Me.Cursor = Cursors.WaitCursor

            If Me.uxCfClass.SelectedIndex <> 0 Then
                BlueCurve_TextSearch1.SearchCurrentAttribute = "Name"
            End If

            Dim i, j As Integer
            Me.uxCfEntity.Items.Clear()
            Me.uxCfEntity.Enabled = False

            If Me.uxCfClass.SelectedIndex = 0 Then
                LoadEntities()
                'Me.liststock2.Items.Clear()
                'Me.liststock2.Text = ""
                'For i = 0 To ofunctions.entities.Count - 1
                '    If ofunctions.entities(i).class_id = ofunctions.class_ids(0) Then
                '        Me.liststock2.Items.Add(ofunctions.entities(i).name)
                '    End If
                'Next
                Exit Sub
            End If
            For i = 0 To OFunctions.class_names.Count - 1
                If OFunctions.class_names(i) = Me.uxCfClass.Text Then
                    For j = 0 To OFunctions.entities.Count - 1
                        Me.uxCfEntity.Enabled = True
                        If OFunctions.entities(j).class_id = OFunctions.class_ids(i) Then
                            Me.uxCfEntity.Items.Add(OFunctions.entities(j).name)
                        End If
                    Next
                    If uxCfEntity.Items.Count > 0 Then
                        uxCfEntity.SelectedIndex = 0
                    End If
                    Exit For
                End If
            Next
        Catch

        Finally

            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub uxCfEntity_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxCfEntity.SelectedIndexChanged

        Try
            Me.Cursor = Cursors.WaitCursor

            REM primary filter
            Dim i, j As Integer
            Dim list1 As New ArrayList
            Dim outstr As String
            Dim istr As String
            Dim ty As String = ""
            For i = 0 To OFunctions.class_ids.Count - 1
                For j = 0 To Me.UseClassIds.Count - 1
                    If Me.UseClassIds(j) = OFunctions.class_ids(i) And OFunctions.class_names(i) = Me.uxCfClass.Text Then
                        If Me.UseClassTypes(j) = "Parent" Then
                            ty = 5006
                        Else
                            ty = 5005
                        End If
                        Exit For
                    End If
                Next
            Next
            outstr = LoadPropogatingEntities(ty, Me.uxCfClass.Text, Me.uxCfEntity.Text, PrimaryText)
            If outstr <> "" And InStr(outstr, ";") = 0 Then
                list1.Add(outstr)
            Else
                REM chop it up
                While InStr(outstr, ";") > 0
                    istr = outstr.Substring(0, InStr(outstr, ";") - 1)
                    list1.Add(istr)
                    outstr = outstr.Substring(InStr(outstr, ";"), Len(outstr) - InStr(outstr, ";"))
                End While
            End If

            Me.txtStock.Text = ""
            Me.uxListStock.Items.Clear()
            Me.uxListStock.Text = ""
            'Me.SelectedEntity = Nothing
            For i = 0 To list1.Count - 1
                If Len(Me.BlueCurve_TextSearch1.SearchText) = 0 Or (Me.BlueCurve_TextSearch1.SearchCurrentAttribute = "Name" And InStr(UCase(list1(i)), UCase(Me.BlueCurve_TextSearch1.SearchText)) <> 0) Then
                    Me.uxListStock.Items.Add(list1(i))
                End If
            Next
            If Me.uxListStock.Items.Count() > 0 Then
                Me.uxListStock.SelectedIndex = 0
            End If

        Catch

        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub uxListStock_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxListStock.DoubleClick
        Dim AddItem As Boolean = True

        If Me.uxListStock.SelectedIndex <> -1 Then

            For i = 0 To OFunctions.entities.Count - 1
                If OFunctions.entities(i).class_id = PrimaryFocus Then
                    If OFunctions.entities(i).name = Me.uxListStock.Items(Me.uxListStock.SelectedIndex) Then

                        For j = 1 To Me.uxListStockSelected.Items.Count
                            If Me.uxListStockSelected.Items(j - 1) = OFunctions.entities(i).name Then
                                AddItem = False
                            End If
                        Next

                        If AddItem = True Then
                            Me.uxListStockSelected.Items.Add(OFunctions.entities(i).name)
                            BuildEntityList()
                        End If

                        Exit For
                    End If
                End If
            Next
        End If
    End Sub

    Private Sub uxListStock_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxListStock.SelectedIndexChanged

        If IsNothing(SelectedAction) Then Exit Sub

        If Me.uxListStock.SelectedIndex <> -1 Then

            For i = 0 To OFunctions.entities.Count - 1
                If OFunctions.entities(i).class_id = PrimaryFocus Then
                    If Trim(OFunctions.entities(i).name) = Trim(Me.uxListStock.Items(Me.uxListStock.SelectedIndex)) Then
                        If SelectedAction.EventType <> "DELETE" Then
                            Me.uxListStockSelected.Items.Clear()
                            Me.uxListStockSelected.Items.Add(OFunctions.entities(i).name)
                            BuildEntityList()
                            Me.txtStock.Text = OFunctions.entities(i).name
                        End If

                        Exit For

                    End If
                End If
            Next

        End If
    End Sub

    Private Sub uxTFilter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        uxSearchTimer.Stop()
        uxSearchTimer.Start()
        ' LoadEntities()
    End Sub

    Private Sub uxEntityDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxEntityDelete.Click

        If Me.uxListStockSelected.SelectedIndex <> -1 Then
            Me.uxListStockSelected.Items.RemoveAt(Me.uxListStockSelected.SelectedIndex)
            BuildEntityList()
        End If

    End Sub

    Private Sub uxListStockSelected_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxListStockSelected.DoubleClick
        If Me.uxListStockSelected.SelectedIndex <> -1 Then
            Me.uxListStockSelected.Items.RemoveAt(Me.uxListStockSelected.SelectedIndex)
            BuildEntityList()
        End If
    End Sub

    Private Sub BlueCurve_TextSearch1_AttributeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles BlueCurve_TextSearch1.AttributeChanged
        If BlueCurve_TextSearch1.SearchCurrentAttribute <> "Name" Then
            uxCfClass.SelectedIndex = 0
        End If
    End Sub

    Private Sub BlueCurve_TextSearch1_FireSearch(ByVal sender As Object, ByVal e As System.EventArgs) Handles BlueCurve_TextSearch1.FireSearch

        If Me.uxListStock.Items.Count > 0 Then
            Me.uxListStock.SelectedIndex = 0
        Else
            Me.uxListStockSelected.Items.Clear()
            BuildEntityList()
        End If

    End Sub

    Private Sub BlueCurve_TextSearch1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles BlueCurve_TextSearch1.Load

    End Sub

    Private Sub GroupBox3_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles GroupBox3.Enter

    End Sub

    Private Sub txtStock_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtStock.TextChanged

    End Sub

    Private Sub uxRatioB_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxRatioB.TextChanged

    End Sub

    Private Sub uxMoney_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxMoney.TextChanged

    End Sub

    Private Sub btnforwardgrey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnforwardgrey.Click

    End Sub
End Class


