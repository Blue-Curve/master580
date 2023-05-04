Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Windows.Forms
Imports System.Drawing
Imports System.Xml
Imports System.Threading
#Region "Changes"
REM -------------------------------------------------------------------------------------------------------------------
REM Changes:
REM Tracker                 Initials                   Date                      Synopsis
REM FIL JIRA 8306          PR                         3/01/2014                  Dual target entities
#End Region
Public Class bc_am_in_universe_builder

    Public tk_main As Object
    Public Const AGGREGATION_CLASS_TYPE As Integer = 2
    Public Shared AGGREGATION_CLASS_ID As Long
    Friend lAggregationLists As List(Of bc_om_entity_class)
    Friend entities As New bc_om_entities
    Friend classes As New bc_om_entity_classes
    Private boolAggregationChanged As Boolean = False
    Private boolLoading As Boolean = False
    Private bocAggregationUniverse As bc_om_entity_class
    Private wmMode As WriteMode
    Private lAggregations As New List(Of bc_om_entity)
    Private new_load As Boolean = False
    REM Friend boeAggregation As bc_om_entity
    Friend btnNew, btnDelete, btnSave, btnCancel, btnCalculations, btnactive As ToolBarButton

    Private Const FORM_NAME As String = "bc_am_in_universe_builder"

    Public lvwColumnSorter As ListViewColumnSorter

    Private Enum WriteMode
        INSERT = 0
        UPDATE = 1
    End Enum

    Private Class bc_am_in_listviewitem
        Inherits ListViewItem
        Public boe As bc_om_entity
        Public Sub New(ByRef boe As bc_om_entity)
            MyBase.New(boe.name)
            Me.boe = boe
        End Sub

        Protected Overrides Sub Finalize()
            MyBase.Finalize()
        End Sub
    End Class

    Private Property AggregationLoading() As Boolean
        Get
            Return boolLoading
        End Get
        Set(ByVal value As Boolean)
            boolLoading = value
        End Set
    End Property

    Private Property AggregationChanged() As Boolean
        Get
            Return boolAggregationChanged
        End Get
        Set(ByVal value As Boolean)
            boolAggregationChanged = value
        End Set
    End Property

    Private Sub clearControls()
        AggregationLoading = True
        clearControls(uxAggregations)
        Dim i As Integer = 0

        For i = uxUCPanel.Controls.Count - 1 To 0 Step -1
            If Not uxUCPanel.Controls(i) Is Bc_am_in_ub_listentity1 Then
                uxUCPanel.Controls.RemoveAt(i)
            Else
                Dim uxbaiul As bc_am_in_ub_listentity
                uxbaiul = uxUCPanel.Controls(i)
                uxbaiul.uxAdd.Visible = True
                uxbaiul.cbAggregationLists.SelectedIndex = -1
                uxbaiul.cbEntities.SelectedIndex = -1
                uxbaiul.cbOperator.SelectedIndex = -1
                uxbaiul.cbClasses.SelectedIndex = -1
                uxbaiul.txtParen1.Text = ""
                uxbaiul.txtParen2.Text = ""
                'uxbaiul.txtParen3.Text = ""

            End If
        Next

        Me.uxFormula.Text = ""
        Me.cbType.Enabled = False
        Me.cbSourceClass.Enabled = False
        Me.cbSourceClass.SelectedIndex = -1
        Me.cbTargetClass.Enabled = False
        Me.cbStartYear.Enabled = False
        Me.cbNumberOfYears.Enabled = False
        Me.cbCurrency.Enabled = False
        Me.cbExchangeRate.Enabled = False
        Me.cbMonthEnd.Enabled = False
        lvResults.Items.Clear()
        Me.cbStartYear.SelectedIndex = 0
        Me.cbMonthEnd.Text = "Dec"


        AggregationLoading = False
    End Sub

    Private Sub clearControls(ByVal c As Control)
        If TypeOf c Is bc_am_calc_search Then
            Exit Sub
        End If
        If TypeOf c Is TextBox Then
            c.Text = ""
        ElseIf TypeOf c Is ComboBox Then
            CType(c, ComboBox).SelectedItem = Nothing
        End If
        For Each cChild As Control In c.Controls
            clearControls(cChild)
        Next
    End Sub

    Private Sub attachHandlers(ByVal c As Control)
        If c.Parent Is pPreview Then
            Exit Sub
        End If
        If TypeOf c Is bc_am_calc_search Then
            Exit Sub
        End If
        If TypeOf c Is ListView Then
            Exit Sub
        End If
        If TypeOf c Is TextBox Then
            AddHandler c.TextChanged, AddressOf aggregationChanged_handler
        ElseIf TypeOf c Is ComboBox Then
            AddHandler CType(c, ComboBox).SelectedIndexChanged, AddressOf aggregationChanged_handler
        ElseIf TypeOf c Is CheckBox Then
            AddHandler CType(c, CheckBox).CheckedChanged, AddressOf aggregationChanged_handler
        End If
        For Each cChild As Control In c.Controls
            attachHandlers(cChild)
        Next
    End Sub

    Friend Sub loadAggregationLists()
        If lAggregationLists Is Nothing Then
            lAggregationLists = New List(Of bc_om_entity_class)
            For Each boc As bc_om_entity_class In classes.classes
                'If boc.class_name.ToLower.IndexOf("list") > -1 Then
                '    lAggregationLists.Add(boc)
                'End If
                If boc.class_type_Id = 3 Then
                    lAggregationLists.Add(boc)
                End If
            Next
        End If
    End Sub

    Private Sub loadAttributes(ByRef boc As bc_om_entity_class, ByRef boe As bc_om_entity)
        boe.attribute_values.Clear()
        Dim oatt As bc_om_attribute_value
        For j = 0 To boc.attributes.Count - 1
            For i = 0 To classes.attribute_pool.Count - 1
                If classes.attribute_pool(i).attribute_id = boc.attributes(j).attribute_id Then
                    oatt = New bc_om_attribute_value
                    oatt.attribute_Id = classes.attribute_pool(i).attribute_id
                    oatt.submission_code = classes.attribute_pool(i).submission_code
                    oatt.nullable = classes.attribute_pool(i).nullable
                    oatt.show_workflow = classes.attribute_pool(i).show_workflow
                    boe.attribute_values.Add(oatt)
                End If
            Next
        Next
    End Sub

    Private Sub loadCalculations()
        If Not boeAggregation Is Nothing Then
            cbPreviewCalculation.Items.Clear()
            bc_am_calculation_values.AggregationId = boeAggregation.id
            Dim obc_calcs As bc_om_calculations = bc_am_calculation_values.getCalcs()
            For Each bac As bc_om_calculation In obc_calcs.calculations
                cbPreviewCalculation.Items.Add(bac)
            Next
        End If
    End Sub

    Private Sub bc_am_in_universe_builder_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

        Dim slog = New bc_cs_activity_log(FORM_NAME, "bc_am_in_universe_builder_Load", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Application.DoEvents()
            createProgressBar("Loading data...")
            new_load = True
            REM PR only load the parts if classes relevant to aggregations
            classes.no_attributes = True

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                classes.db_read()
            Else
                classes.tmode = bc_cs_soap_base_class.tREAD
                If classes.transmit_to_server_and_receive(classes, True) = False Then
                    Exit Sub
                End If
            End If

            Dim boc As bc_om_entity_class
            AGGREGATION_CLASS_ID = 0
            For Each boc In classes.classes
                If boc.class_type_Id = AGGREGATION_CLASS_TYPE Then
                    AGGREGATION_CLASS_ID = boc.class_id

                    bocAggregationUniverse = boc
                    Exit For
                End If
            Next

            incrementProgressBar(25)

            If AGGREGATION_CLASS_ID = 0 Then
                Dim oerr As New bc_cs_error_log("bc_am_in_universeBuilder", "Load", bc_cs_error_codes.USER_DEFINED, "Cannot find Aggregation Class of type 2.")
                Exit Sub
            End If
            populateComboBox("Currency", cbCurrency)
            populateComboBox("Type", cbType)
            populateComboBox("Exchange Rate Method", cbExchangeRate)
            populateComboBox("Month End", cbMonthEnd)
            populateComboBox("Number of Years", cbNumberOfYears)
            populateComboBox("Start Year", cbStartYear)
          

            incrementProgressBar(50)
            entities.get_inactive = True
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                entities.db_read()
            Else
                entities.tmode = bc_cs_soap_base_class.tREAD
                If entities.transmit_to_server_and_receive(entities, True) = False Then
                    Exit Sub
                End If
            End If

            Dim bocBlank As New bc_om_entity_class
            bocBlank.class_name = ""

            cbSourceClass.Items.Add(bocBlank)

            For Each boc2 In classes.classes
                'cbSourceClass.Items.Add(boc2)
                If boc2.class_type_id = 1 Or boc2.class_type_id = 4 Then
                    cbSourceClass.Items.Add(boc2)
                End If
            Next

            incrementProgressBar(75)

            loadAggregations()

            REM PR this is overkill!!! can get controbutors from returned preview list
            'Dim boef As New bc_om_ef_functions
            'If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            '    boef.db_read()
            'Else
            '    boef.tmode = bc_cs_soap_base_class.tREAD
            '    If boef.transmit_to_server_and_receive(boef, True) = False Then
            '        Exit Sub
            '    End If
            'End If
            'Dim i As Integer = 0
            'Dim l As New List(Of KeyValuePair(Of String, String))
            'While i < boef.contributor_ids.Count And i < boef.contributor_names.Count
            '    Dim kvp As New KeyValuePair(Of String, String)(boef.contributor_names(i), boef.contributor_ids(i))
            '    l.Add(kvp)
            '    i += 1
            'End While
            'cbPreviewContributor.DataSource = l
            'cbPreviewContributor.DisplayMember = "Key"
            'cbPreviewContributor.ValueMember = "Value"

            incrementProgressBar(100)

            loadAggregationLists()

            Bc_am_in_ub_listentity1.loadLists(lAggregationLists)
            AddHandler Bc_am_in_ub_listentity1.itemchanged, AddressOf Bc_am_in_ub_listentity1_itemchanged

            clearComboBoxes()

            new_load = False

            If lvAggregations.Items.Count > 0 Then
                lvAggregations.Items(0).Selected = True
            End If

            attachHandlers(Me)

            unloadProgressBar()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "bc_am_in_universe_builder_Load", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(FORM_NAME, "bc_am_in_universe_builder_Load", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub loadAggregations(Optional ByVal entity_id As Integer = 0)
        Dim lv As ListViewItem
        uxDetails.Enabled = False
        lvAggregations.Items.Clear()

        If lAggregations.Count = 0 Then
            For i = 0 To entities.entity.Count - 1
                If entities.entity(i).class_id = AGGREGATION_CLASS_ID Then
                    lAggregations.Add(entities.entity(i))
                    lv = New ListViewItem(CStr(entities.entity(i).name), 0)
                    If entities.entity(i).inactive = True Then
                        lv.SubItems.Add("no")
                    Else
                        lv.SubItems.Add("yes")
                    End If
                    Bc_am_calc_search1.complete_list.Add(entities.entity(i).name)
                    lvAggregations.Items.Add(lv)
                End If
            Next
        Else
            For i = 0 To entities.entity.Count - 1
                If entities.entity(i).class_id = AGGREGATION_CLASS_ID Then
                    lv = New ListViewItem(CStr(entities.entity(i).name), 0)
                    If entities.entity(i).inactive = True Then
                        lv.SubItems.Add("no")
                    Else
                        lv.SubItems.Add("yes")
                    End If
                    lvAggregations.Items.Add(lv)
                End If
            Next
        End If
    End Sub
    Public Sub load_aggregation()
        Try

            For i = 0 To lAggregations.Count - 1
                If lAggregations(i).name = Me.lvAggregations.SelectedItems(0).Text Then
                    lAggregations(i).boolLoadAggregationPreview = False
                    loadAttributes(bocAggregationUniverse, lAggregations(i))

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        lAggregations(i).db_read()
                    Else
                        lAggregations(i).tmode = bc_cs_soap_base_class.tREAD
                        If lAggregations(i).transmit_to_server_and_receive(lAggregations(i), True) = False Then
                            Exit Sub
                        End If
                    End If
                    Me.boeAggregation = lAggregations(i)

                    Exit For
                End If
            Next
        Catch ex As Exception

        End Try
        'For Each boe As bc_om_entity In lAggregations
        '    If Not boe.deleted Then
        '        If boe.name = Me.lvAggregations.SelectedItems(0).Text Then
        '            boeAggregation = boe
        '        End If
        '        loadAttributes(bocAggregationUniverse, boe)

        '        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
        '            boe.db_read()
        '        Else
        '            boe.tmode = bc_cs_soap_base_class.tREAD
        '            If boe.transmit_to_server_and_receive(boe, True) = False Then
        '                Exit Sub
        '            End If
        '        End If

        '        Dim bail As New bc_am_in_listviewitem(boe)
        '        bail.boe = boe
        '        lvAggregations.Items.Add(bail)
        '    End If
        'Next

    End Sub

    Private Sub populateComboBox(ByVal attribute_name As String, ByVal cb As ComboBox)
        Dim l As New List(Of KeyValuePair(Of String, String))
        For Each attribute As bc_om_attribute In classes.attribute_pool
            If attribute.name = attribute_name Then
                Dim i As Integer = 0
                If attribute.lookup_keys.Count > 0 Then
                    While i < attribute.lookup_values.Count And i < attribute.lookup_keys.Count
                        Dim kvp As New KeyValuePair(Of String, String)(attribute.lookup_keys(i), attribute.lookup_values(i))
                        l.Add(kvp)
                        i += 1
                    End While
                Else
                    While i < attribute.lookup_values.Count
                        Dim kvp As New KeyValuePair(Of String, String)(attribute.lookup_values(i), attribute.lookup_values(i))
                        l.Add(kvp)
                        i += 1
                    End While
                End If
            End If
        Next
        cb.DataSource = l
        cb.DisplayMember = "Value"
        cb.ValueMember = "Key"
        cb.SelectedIndex = -1

    End Sub

    Private Sub displayCalculationEditor()
        Dim baice As New bc_am_in_calculation_editor
        baice.Visible = False
        baice.Visible = True
        Me.ParentForm.ShowDialog(baice)
    End Sub


    Private Function addNewListEntity(ByRef baiul As bc_am_in_ub_listentity) As bc_am_in_ub_listentity

        Dim slog = New bc_cs_activity_log(FORM_NAME, "addNewListEntity", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Dim intY As Integer = -1
            Dim intOffset As Integer = 32
            For Each c As Control In uxUCPanel.Controls
                If c.Equals(baiul) Then
                    intY = c.Location.Y
                    Exit For
                End If
            Next

            Dim newBaiul As New bc_am_in_ub_listentity
            newBaiul.Anchor = AnchorStyles.Top + AnchorStyles.Left + AnchorStyles.Right
            newBaiul.Size = New Size(uxUCPanel.Controls(uxUCPanel.Controls.Count - 1).Size.Width, baiul.Size.Height)
        
            newBaiul.Visible = False
            REM 

            newBaiul.loadLists(lAggregationLists)
            newBaiul.Location = New Point(baiul.Location.X, intY + intOffset)
            newBaiul.Anchor = AnchorStyles.Top + AnchorStyles.Left + AnchorStyles.Right

            uxUCPanel.Controls.Add(newBaiul)

            AddHandler newBaiul.AddNewItem, AddressOf Bc_am_in_ub_listentity1_AddNewLevel
            AddHandler newBaiul.RemoveItem, AddressOf Bc_am_in_ub_listentity1_RemoveLevel
            AddHandler newBaiul.itemchanged, AddressOf Bc_am_in_ub_listentity1_itemchanged

            baiul.uxAdd.Visible = False
            baiul.uxRemove.Visible = False

            newBaiul.Visible = True

            Return newBaiul

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "addNewListEntity", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Return Nothing
        Finally
            slog = New bc_cs_activity_log(FORM_NAME, "addNewListEntity", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Sub Bc_am_in_ub_listentity1_AddNewLevel(ByRef baiul As bc_am_in_ub_listentity) Handles Bc_am_in_ub_listentity1.AddNewItem

        Dim slog = New bc_cs_activity_log(FORM_NAME, "Bc_am_in_ub_listentity1_AddNewItem", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            addNewListEntity(baiul)
            setChanged(True)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "Bc_am_in_ub_listentity1_AddNewLevel", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(FORM_NAME, "Bc_am_in_ub_listentity1_AddNewLevel", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub Bc_am_in_ub_listentity1_itemchanged(ByRef baiul As bc_am_in_ub_listentity) Handles Bc_am_in_ub_listentity1.AddNewItem

        Dim slog = New bc_cs_activity_log(FORM_NAME, "Bc_am_in_ub_listentity1_itemchanged", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            setChanged(True)
            'preview_composition()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "Bc_am_in_ub_listentity1_itemchanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(FORM_NAME, "Bc_am_in_ub_listentity1_itemchanged", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Function preview_composition() As Boolean
        Dim err As String
        preview_composition = False
        err = Me.check_composition
        If err <> "" Then
            Me.uxFormula.Text = err
        Else
            If check_brackets() = 0 Then
                preview_composition = True
                Dim le As bc_am_in_ub_listentity
                Dim tx As String = "Aggregate " + Me.cbSourceClass.Text + "(s) in: "
                tx = tx + vbCrLf
                For i = 0 To uxUCPanel.Controls.Count - 1
                    le = uxUCPanel.Controls(i)
                    tx = tx + le.txtParen1.Text + " " + le.cbClasses.Text + ": " + le.cbEntities.Text + " " + le.txtParen2.Text + " " + le.cbOperator.Text
                    tx = tx + vbCrLf
                Next
                Me.uxFormula.Text = tx
            Else
                Me.uxFormula.Text = "Incomplete Compoistion: Parentheses mismatch"
            End If
        End If
    End Function
    Public Function check_brackets() As Integer
        check_brackets = False

        check_brackets = 0
        Dim le As bc_am_in_ub_listentity
        For i = 0 To uxUCPanel.Controls.Count - 1
            le = uxUCPanel.Controls(i)
            check_brackets = check_brackets + Len(LTrim(RTrim(le.txtParen1.Text)))
            check_brackets = check_brackets - Len(LTrim(RTrim(le.txtParen2.Text)))
        Next

    End Function

    Private Sub displayAggregation()
        Try
            If Not boeAggregation Is Nothing Then
                loadAggregationDetails()
            End If
        Catch ex As Exception
            MsgBox("Fail.")
            'fix
        End Try
    End Sub

    Private Sub lvAggregations_ColumnClick(ByVal sender As Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles lvAggregations.ColumnClick

        ' Determine if the clicked column is already the column that is 
        ' being sorted.
        If (e.Column = lvwColumnSorter.SortColumn) Then
            ' Reverse the current sort direction for this column.
            If (lvwColumnSorter.Order = SortOrder.Ascending) Then
                lvwColumnSorter.Order = SortOrder.Descending
            Else
                lvwColumnSorter.Order = SortOrder.Ascending
            End If
        Else
            ' Set the column number that is to be sorted; default to ascending.
            lvwColumnSorter.SortColumn = e.Column
            lvwColumnSorter.Order = SortOrder.Ascending
        End If

        ' Perform the sort with these new sort options.
        lvAggregations.Sort()

    End Sub

    

    Private Sub lvAggregations_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvAggregations.SelectedIndexChanged
        If lvAggregations.SelectedItems.Count = 0 Then
            Me.btnCancel.Enabled = False
            Me.btnDelete.Enabled = False
            Me.btnCalculations.Enabled = False
            Me.btnactive.Enabled = False
            Me.btnNew.Enabled = True
            Me.btnSave.Enabled = False
            Me.txtName.Enabled = False
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor
        Me.clear_preview()


        load_aggregation()
        displayAggregation()
        btnCalculations.Enabled = True
        btnDelete.Enabled = False
        btnactive.Enabled = True
        btnactive.Text = "Deactivate"
        btnactive.ImageIndex = 13
        If boeAggregation.inactive = True Then
            btnactive.Text = "Activate"
            btnactive.ImageIndex = 1
            btnDelete.Enabled = True
        End If
        Me.cbType.Enabled = True
        Me.txtName.Enabled = True
      
     


        Me.Cursor = Cursors.Default


    End Sub

    Private Sub setCBClass(ByRef cb As ComboBox, ByVal class_id As String)
        For Each boc As bc_om_entity_class In cb.Items
            If boc.class_id.ToString() = class_id Then
                cb.SelectedItem = boc
                Exit Sub
            End If
        Next
    End Sub

    Private Function saveAggregationDetails() As Boolean

        Dim slog = New bc_cs_activity_log(FORM_NAME, "saveAggregationDetails", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            saveAggregationDetails = True
            boeAggregation.name = txtName.Text
            wmMode = WriteMode.UPDATE

            If Me.lvAggregations.SelectedItems.Count = 0 Then
                wmMode = WriteMode.INSERT
                REM check doesnt already exists
                For i = 0 To Me.lvAggregations.Items.Count - 1
                    REM FIL MAY2 2013
                    If Trim(UCase(Me.lvAggregations.Items(i).Text)) = Trim(UCase(boeAggregation.name)) Then
                        saveAggregationDetails = False
                        Dim omsg As New bc_cs_message("Blue Curve", "Aggregation: " + boeAggregation.name + " already exists please enter another name", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Me.txtName.Select()
                        Exit Function
                    End If
                Next
            Else
                Dim otx As String
                otx = Me.lvAggregations.SelectedItems(0).Text

                For i = 0 To Me.lvAggregations.Items.Count - 1
                    If UCase(Me.lvAggregations.Items(i).Text) = UCase(boeAggregation.name) And UCase(Me.lvAggregations.Items(i).Text) <> UCase(otx) Then
                        saveAggregationDetails = False
                        Dim omsg As New bc_cs_message("Blue Curve", "Aggregation: " + boeAggregation.name + " already exists please enter another name", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Me.txtName.Select()
                        Exit Function
                    End If
                Next
            End If

            For Each boav As bc_om_attribute_value In boeAggregation.attribute_values

                For i = 0 To classes.attribute_pool.Count - 1
                    If classes.attribute_pool(i).attribute_id = boav.attribute_Id Then
                        REM JIRA 
                       
                        If classes.attribute_pool(i).name = "Dual Target Class" Then
                            setAttributeFromClass(cbtargetclass2, boav)
                        End If

                        REM END JIRA
                        If classes.attribute_pool(i).name = "Source Class" Then
                            setAttributeFromClass(cbSourceClass, boav)
                        End If
                        If classes.attribute_pool(i).name = "Target Class" Then
                            setAttributeFromClass(cbTargetClass, boav)
                        End If

                        If classes.attribute_pool(i).name = "Currency" Then
                            setAttributeFromKVP(cbCurrency, boav)
                        End If

                        If classes.attribute_pool(i).name = "Type" Then
                            setAttributeFromKVP(cbType, boav)
                        End If
                        If classes.attribute_pool(i).name = "Month End" Then
                            setAttributeFromKVP(cbMonthEnd, boav)
                        End If
                        If classes.attribute_pool(i).name = "Start Year" Then
                            setAttributeFromKVP(cbStartYear, boav)
                        End If
                        If classes.attribute_pool(i).name = "Number of Years" Then
                            setAttributeFromKVP(cbNumberOfYears, boav)
                        End If
                        If classes.attribute_pool(i).name = "Exchange Rate Method" Then
                            setAttributeFromKVP(cbExchangeRate, boav)
                        End If

                        If classes.attribute_pool(i).name = "List Formula XML" Then
                            Dim strOutput As String = "<list_entities>"
                            For Each baiul As bc_am_in_ub_listentity In uxUCPanel.Controls
                                strOutput = strOutput & baiul.toXml
                            Next
                            strOutput = strOutput & "</list_entities>"
                            boav.value = strOutput
                            boav.value_changed = True
                        End If
                        If classes.attribute_pool(i).name = "List Formula" Then
                            Dim strOutput As String = ""
                            For Each baiul As bc_am_in_ub_listentity In uxUCPanel.Controls
                                strOutput = strOutput & baiul.ToString
                            Next
                            boav.value = strOutput
                            boav.value_changed = True
                        End If


                    End If
                Next
            Next

            If wmMode = WriteMode.UPDATE Then

                boeAggregation.write_mode = bc_om_entity.ATTRIBUTES
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    boeAggregation.db_write()
                Else
                    boeAggregation.tmode = bc_cs_soap_base_class.tWRITE
                    If boeAggregation.transmit_to_server_and_receive(boeAggregation, True) = False Then
                        Exit Function
                    End If
                End If


                boeAggregation.write_mode = bc_om_entity.UPDATE
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    boeAggregation.db_write()
                Else
                  
                    boeAggregation.tmode = bc_cs_soap_base_class.tWRITE
                    If boeAggregation.transmit_to_server_and_receive(boeAggregation, True) = False Then
                        Throw New ApplicationException("Saving Aggregation Failed")
                    End If
                    For i = 0 To entities.entity.Count - 1
                        If entities.entity(i).id = boeAggregation.id Then
                             entities.entity(i).name = boeAggregation.name
                            Exit For
                        End If
                    Next
                End If

            ElseIf wmMode = WriteMode.INSERT Then

                boeAggregation.write_mode = bc_om_entity.INSERT
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    boeAggregation.db_write()
                Else
                    boeAggregation.tmode = bc_cs_soap_base_class.tWRITE
                    If boeAggregation.transmit_to_server_and_receive(boeAggregation, True) = False Then
                        Throw New ApplicationException("Saving Aggregation Failed")
                    End If
                End If

                For Each boav As bc_om_attribute_value In boeAggregation.attribute_values
                    boav.entity_id = boeAggregation.id
                Next

                boeAggregation.write_mode = bc_om_entity.ATTRIBUTES
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    boeAggregation.db_write()
                Else
                    boeAggregation.tmode = bc_cs_soap_base_class.tWRITE
                    If boeAggregation.transmit_to_server_and_receive(boeAggregation, True) = False Then
                        Throw New ApplicationException("Saving Aggregation Failed")
                    End If
                End If
                entities.entity.Add(boeAggregation)
                lAggregations.Add(boeAggregation)
                Bc_am_calc_search1.complete_list.Add(boeAggregation.name)

            End If
            'Dim active_changed As Boolean = False

            'If Me.Chkactive.Checked = True And boeAggregation.inactive = True Then
            '    boeAggregation.inactive = False

            '    active_changed = True
            '    boeAggregation.write_mode = bc_om_entity.SET_ACTIVE
            'ElseIf Me.Chkactive.Checked = False And boeAggregation.inactive = False Then
            '    boeAggregation.inactive = True
            '    active_changed = True
            '    boeAggregation.write_mode = bc_om_entity.SET_INACTIVE
            'End If

            'If active_changed = True Then
            '    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            '        boeAggregation.db_write()
            '    Else
            '        boeAggregation.tmode = bc_cs_soap_base_class.tWRITE
            '        If boeAggregation.transmit_to_server_and_receive(boeAggregation, True) = False Then
            '            Throw New ApplicationException("Saving Aggregation Failed")
            '        End If
            '    End If

            'End If

            saveAggregationDetails = True
            'setChanged(False)



        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "saveAggregationDetails", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(FORM_NAME, "saveAggregationDetails", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Sub setAttributeFromClass(ByRef cb As ComboBox, ByRef boav As bc_om_attribute_value)
        If Not cb.SelectedItem Is Nothing Then
            boav.value = CType(cb.SelectedItem, bc_om_entity_class).class_id
        Else
            boav.value = Nothing
        End If
        boav.value_changed = True
    End Sub

    Private Sub setAttributeFromKVP(ByRef cb As ComboBox, ByRef boav As bc_om_attribute_value)
        If Not cb.SelectedItem Is Nothing Then
            boav.value = CType(cb.SelectedItem, KeyValuePair(Of String, String)).Key
        Else
            boav.value = Nothing
        End If
        boav.value_changed = True
    End Sub

    Private Sub loadAggregationDetails()

        Dim slog = New bc_cs_activity_log(FORM_NAME, "loadAggregationDetails", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            wmMode = WriteMode.UPDATE

            AggregationLoading = True

            Me.uxDetails.Enabled = True

            clearComboBoxes()

            lvResults.Items.Clear()

            txtName.Text = boeAggregation.name
            Dim i As Integer

            For Each boav As bc_om_attribute_value In boeAggregation.attribute_values

                For i = 0 To classes.attribute_pool.Count - 1
                    If classes.attribute_pool(i).attribute_id = boav.attribute_Id Then
                        If classes.attribute_pool(i).name = "Type" Then
                            setComboboxFromKVP(cbType, boav)
                        End If
                        If classes.attribute_pool(i).name = "Start Year" Then
                            setComboboxFromKVP(cbStartYear, boav)
                        End If
                    End If
                Next
            Next
            For Each boav As bc_om_attribute_value In boeAggregation.attribute_values

                For i = 0 To classes.attribute_pool.Count - 1
                    If classes.attribute_pool(i).attribute_id = boav.attribute_Id Then
                        If classes.attribute_pool(i).name = "Source Class" Then
                            setCBClass(cbSourceClass, boav.value)
                        End If
                        If classes.attribute_pool(i).name = "Target Class" Then
                            setCBClass(cbTargetClass, boav.value)
                        End If

                        If classes.attribute_pool(i).name = "Dual Target Class" Then
                            setCBClass(cbtargetclass2, boav.value)
                        End If
                        If classes.attribute_pool(i).name = "Currency" Then
                            setComboboxFromKVP(cbCurrency, boav)
                        End If
                       
                        If classes.attribute_pool(i).name = "Month End" Then
                            setComboboxFromKVP(cbMonthEnd, boav)
                        End If

                        If classes.attribute_pool(i).name = "Number of Years" Then
                            setComboboxFromKVP(cbNumberOfYears, boav)
                        End If
                        If classes.attribute_pool(i).name = "Exchange Rate Method" Then
                            setComboboxFromKVP(cbExchangeRate, boav)
                        End If
                    End If
                Next
            Next



            For i = 0 To classes.attribute_pool.Count - 1
                For Each boav As bc_om_attribute_value In boeAggregation.attribute_values
                    If classes.attribute_pool(i).attribute_id = boav.attribute_Id Then
                        If classes.attribute_pool(i).name = "List Formula XML" Then
                            loadListComposition(boav.value)
                            Exit For
                        End If
                    End If
                Next
            Next
           

            bc_am_calculation_values.AggregationId = boeAggregation.id

            'loadCalculations()

            enableDelete(True)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "loadAggregationDetails", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(FORM_NAME, "loadAggregationDetails", bc_cs_activity_codes.TRACE_EXIT, "")
            AggregationLoading = False
        End Try

    End Sub

    Private Sub loadListComposition(ByVal strComposition As String)

        If Not strComposition Is Nothing AndAlso Not strComposition = "" Then



            Dim i As Integer = 0

            For i = uxUCPanel.Controls.Count - 1 To 0 Step -1
                If Not uxUCPanel.Controls(i) Is Bc_am_in_ub_listentity1 Then
                    uxUCPanel.Controls.RemoveAt(i)
                Else
                    Dim uxbaiul As bc_am_in_ub_listentity
                    uxbaiul = uxUCPanel.Controls(i)
                    uxbaiul.uxAdd.Visible = True
                End If
            Next

            Dim xdComposition As New XmlDocument
            xdComposition.LoadXml(strComposition)

            i = 0
            Dim lastListEntity As bc_am_in_ub_listentity = Nothing

            For Each le As XmlNode In xdComposition.GetElementsByTagName("list_entity")
                If i = 0 Then
                    lastListEntity = Bc_am_in_ub_listentity1
                ElseIf Not lastListEntity Is Nothing Then
                    lastListEntity = addNewListEntity(lastListEntity)
                End If
                lastListEntity.loadXml(le.OuterXml)
                i += 1
            Next

        End If

    End Sub

    Private Sub setComboboxFromKVP(ByRef cb As ComboBox, ByRef boav As bc_om_attribute_value)
        For Each kvp As KeyValuePair(Of String, String) In cb.Items
            If kvp.Key = boav.value Then
                cb.SelectedItem = kvp
            End If
        Next
    End Sub

    Friend Sub incrementProgressBar(ByVal decPercent)
        bc_cs_central_settings.progress_bar.increment(decPercent)
    End Sub

    Friend Sub unloadProgressBar()

        ParentForm.Refresh()

        If Not bc_cs_central_settings.progress_bar Is Nothing Then
            bc_cs_central_settings.progress_bar.unload()
        End If

    End Sub

    Friend Sub createProgressBar(ByVal strMessage As String)

        bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve Insight Toolkit", strMessage, 4, False, True)
        bc_cs_central_settings.progress_bar.SetTopmost()
        bc_cs_central_settings.progress_bar.increment(1D)

    End Sub

    Private Sub aggregationChanged_handler(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim slog = New bc_cs_activity_log(FORM_NAME, "aggregationChanged_handler", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            setChanged(True)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "aggregationChanged_handler", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(FORM_NAME, "aggregationChanged_handler", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Friend Sub cancelClick()
        Dim slog = New bc_cs_activity_log(FORM_NAME, "cancelClick", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            If wmMode = WriteMode.UPDATE Then
                displayAggregation()
            ElseIf wmMode = WriteMode.INSERT Then
                clearControls()
            End If
            setChanged(False)
            Me.btnSave.Enabled = False
            Me.btnNew.Enabled = True
            Me.btnCancel.Enabled = False
            Me.lvAggregations.Enabled = True
            Me.Bc_am_calc_search1.Enabled = True
            Me.btnactive.Enabled = False
            If Me.lvAggregations.SelectedItems.Count > 0 Then
                Me.btnCalculations.Enabled = True
                Me.btnDelete.Enabled = True
                Me.btnactive.Enabled = True
            Else
                Me.cbType.Enabled = False
                Me.txtName.Enabled = False
                Me.uxFormula.Text = ""
            End If
            check_preview_enable()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "cancelClick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(FORM_NAME, "cancelClick", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub enableDelete(ByVal b As Boolean)
        btnDelete.Enabled = b
        btnCalculations.Enabled = b
    End Sub
    Private Sub check_preview_enable()
        Me.pPreview.Enabled = False

        If Me.lvAggregations.SelectedItems.Count > 0 And Me.btnNew.Enabled = True Then
            Me.pPreview.Enabled = True
        End If
    End Sub
    Public from_search As Boolean = False
    Private Sub setChanged(ByVal boolChanged As Boolean)
        If from_search = True Then
            Exit Sub
        End If

        If AggregationLoading = False Then
            Me.lvAggregations.Enabled = False
            Me.Bc_am_calc_search1.Enabled = False




        End If
        'Me.cbTargetClass.Enabled = True
        btnSave.Enabled = False
        btnCalculations.Enabled = False
        btnDelete.Enabled = False
        Me.btnactive.Enabled = False



        If Me.cbType.SelectedIndex = 0 Then
            Me.cbTargetClass.SelectedIndex = -1
            Me.cbTargetClass.Enabled = False
            Me.cbtargetclass2.SelectedIndex = -1
            Me.cbtargetclass2.Enabled = False
        End If
        If Me.cbType.SelectedIndex = 1 And Me.cbSourceClass.SelectedIndex > -1 Then
            Me.cbTargetClass.Enabled = True
        End If
        Me.cbNumberOfYears.Enabled = False
        If Me.cbStartYear.SelectedIndex > 0 Then
            Me.cbNumberOfYears.Enabled = True
        End If
        Dim complete As Boolean
        complete = False
        If Me.cbType.SelectedIndex <> -1 And Me.cbSourceClass.SelectedIndex <> -1 And Me.cbExchangeRate.SelectedIndex <> -1 And Me.cbCurrency.SelectedIndex <> -1 And Me.cbMonthEnd.SelectedIndex <> -1 And Me.cbStartYear.SelectedIndex <> -1 Then
            complete = True
            If Me.cbType.SelectedIndex = 1 And Me.cbTargetClass.SelectedIndex = -1 Then
                complete = False
            End If
            If Me.cbStartYear.SelectedIndex > 0 And Me.cbNumberOfYears.SelectedIndex = -1 Then
                complete = False
            End If
        End If
        If Me.cbType.SelectedIndex = 0 And complete = True Then
            complete = preview_composition()
        End If

        

        btnSave.Enabled = False
        If complete = True And Not AggregationLoading Then
            btnSave.Enabled = True
        End If

        If Not AggregationLoading Then
            '    AggregationChanged = boolChanged
            '    btnSave.Enabled = boolChanged
            btnCancel.Enabled = True
            btnNew.Enabled = False

            '    btnNew.Enabled = Not boolChanged
            '    lvAggregations.Enabled = Not boolChanged
            '    enableDelete(Not boolChanged)
        End If
        check_preview_enable()
    End Sub



    Private Sub clearComboBoxes()
        cbExchangeRate.SelectedItem = Nothing
        cbTargetClass.SelectedItem = Nothing
        cbSourceClass.SelectedItem = Nothing
        cbStartYear.SelectedItem = Nothing
        cbType.SelectedItem = Nothing
        cbMonthEnd.SelectedItem = Nothing
        cbNumberOfYears.SelectedItem = Nothing
        cbCurrency.SelectedItem = Nothing
        Me.cbStartYear.SelectedIndex = 0
        Me.cbMonthEnd.Text = "Dec"
    End Sub

    Friend Sub saveClick()

        Dim slog = New bc_cs_activity_log(FORM_NAME, "saveClick", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            If Me.cbType.SelectedIndex = 0 Then
                If preview_composition() = False Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Compistion Errors please check compostion", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            End If

            If saveAggregationDetails() = True Then
                Me.Bc_am_calc_search1.tsearch.Text = ""
                loadAggregations(boeAggregation.id)
                For i = 0 To Me.lvAggregations.Items.Count - 1
                    If UCase(Me.lvAggregations.Items(i).Text) = UCase(boeAggregation.name) Then
                        Me.lvAggregations.Items(i).Selected = True
                        Exit For
                    End If
                Next
                Me.btnSave.Enabled = False
                Me.btnCancel.Enabled = False
                Me.btnNew.Enabled = True
                Me.lvAggregations.Enabled = True
                Me.Bc_am_calc_search1.Enabled = True
                Me.uxFormula.Text = ""
                check_preview_enable()
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "saveClick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(FORM_NAME, "saveClick", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub newClick()

        Dim slog = New bc_cs_activity_log(FORM_NAME, "newClick", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            If Not AggregationChanged Then
                boeAggregation = New bc_om_entity()
                boeAggregation.class_id = AGGREGATION_CLASS_ID
                loadAttributes(bocAggregationUniverse, boeAggregation)
                wmMode = WriteMode.INSERT
                uxDetails.Enabled = True
                clearControls()
                txtName.Select()
                Me.lvAggregations.SelectedItems.Clear()
                Me.btnCancel.Enabled = True
                Me.btnactive.Enabled = False
                Me.btnNew.Enabled = False
                Me.btnCalculations.Enabled = False
                Me.btnDelete.Enabled = False
                Me.Bc_am_calc_search1.Enabled = False

                Me.lvAggregations.Enabled = False
                Me.cbType.Enabled = True
                Me.txtName.Enabled = True
                Me.uxFormula.Text = ""
              
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "newClick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(FORM_NAME, "newClick", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Function check_composition() As String
        Dim le, ple As bc_am_in_ub_listentity
        ple = Nothing
        check_composition = ""

        For i = 0 To uxUCPanel.Controls.Count - 1
            le = uxUCPanel.Controls(i)
            If i > 0 Then
                ple = uxUCPanel.Controls(i - 1)
            End If
            REM check lists
            If le.all_lists_selected = False Then
                check_composition = "Incomplete Composition Line: " + CStr(i + 1)
                'Dim omsg As New bc_cs_message("Blue Curve", "Incomplete Composition Line: " + CStr(i + 1) + " Please make sure all list boxes are have items selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Me.btnSave.Enabled = False
                Exit Function
            End If
            REM
            REM check if operand selected then remove it if last item

            REM now check if previous operator set
            If i > 0 Then
                If ple.operator_selected = False Then
                    check_composition = "Incomplete Composition Line: " + CStr(i) + " Operator Must be selected."
                    Me.btnSave.Enabled = False
                    REM ple.operator_highlighted()
                    'Dim omsg As New bc_cs_message("Blue Curve", "Incomplete Composition Line: " + CStr(i) + " Operator Must be selected.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

                    Exit Function
                End If
            End If
            If i = uxUCPanel.Controls.Count - 1 Then
                If le.operator_selected = True Then
                    check_composition = "Incomplete Composition Line: " + CStr(i + 1) + " Operator Cannot be selected."
                End If
            End If
        Next

    End Function

    Private Sub deleteAggregation()
        Try
            boeAggregation.write_mode = bc_om_entity.DELETE
            boeAggregation.tmode = bc_cs_soap_base_class.tWRITE

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                boeAggregation.db_write()
            Else
                If boeAggregation.transmit_to_server_and_receive(boeAggregation, True) = False Then
                    Exit Sub
                End If
            End If
            If boeAggregation.deleted Then
                For i = 0 To lAggregations.Count - 1
                    If lAggregations(i).name = Me.lvAggregations.SelectedItems(0).Text Then
                        For j = 0 To entities.entity.Count - 1
                            If entities.entity(j).id = lAggregations(i).id Then
                                entities.entity.RemoveAt(j)
                                Exit For
                            End If
                        Next
                        lAggregations.RemoveAt(i)
                        Exit For
                    End If
                Next
            End If
            clearControls()
            loadAggregations()
        Catch ex As Exception
            Dim s As String = ""
        End Try
    End Sub
    Private Sub activeAggregation(ByVal active As Boolean)
        Try
            If active = True Then
                boeAggregation.write_mode = bc_om_entity.SET_ACTIVE
            Else
                boeAggregation.write_mode = bc_om_entity.SET_INACTIVE
            End If
            boeAggregation.tmode = bc_cs_soap_base_class.tWRITE

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                boeAggregation.db_write()
            Else
                If boeAggregation.transmit_to_server_and_receive(boeAggregation, True) = False Then
                    Exit Sub
                End If
            End If
            boeAggregation.inactive = Not active
            Dim idx As Integer
            idx = Me.lvAggregations.SelectedItems(0).Index


            For i = 0 To lAggregations.Count - 1
                If lAggregations(i).name = Me.lvAggregations.SelectedItems(0).Text Then
                    For j = 0 To entities.entity.Count - 1
                        If entities.entity(j).id = lAggregations(i).id Then
                            lAggregations(i).inactive = Not active
                            entities.entity(j).inactive = Not active
                            Exit For
                        End If
                    Next
                    Exit For
                End If
            Next

            Me.lvAggregations.SelectedItems.Clear()
            Me.lvAggregations.Items(idx).Selected = True
            If Me.lvAggregations.Items(idx).SubItems(1).Text = "yes" Then
                Me.lvAggregations.Items(idx).SubItems(1).Text = "no"
            Else
                Me.lvAggregations.Items(idx).SubItems(1).Text = "yes"
            End If


            'clearControls()
            'loadAggregations()
        Catch ex As Exception
            Dim s As String = ""
        End Try
    End Sub

    Friend Sub activateClick(ByVal activate As Boolean)

        Dim slog = New bc_cs_activity_log(FORM_NAME, "activateClick", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            activeAggregation(activate)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "activateClick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(FORM_NAME, "activateClick", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Friend Sub deleteClick()

        Dim slog = New bc_cs_activity_log(FORM_NAME, "deleteClick", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            deleteAggregation()
            Me.uxFormula.Text = ""
            Me.txtName.Enabled = False

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "deleteClick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(FORM_NAME, "deleteClick", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public boeAggregation As New bc_om_entity
    Public Class bc_am_preview_result
        Public entity_id As Long
        Public da As DateTime
        Public results As Object
    End Class
    Public preview_results As New ArrayList


    'Public Sub async_results_returned(ByVal results As Object) Handles boeAggregation.async_preview_results_om_entity

    '    Dim pr As New bc_am_preview_result
    '    Try
    '        MsgBox("AA")
    '        MsgBox("Preview Complete: for " + CStr(results(0, 0)))
    '        For i = 0 To preview_results.Count - 1
    '            If preview_results(i).entity_id = results(0, 0) Then
    '                preview_results.RemoveAt(i)
    '                Exit For
    '            End If
    '        Next
    '        pr.entity_id = results(0, 0)
    '        pr.da = Now
    '        pr.results = results
    '        preview_results.Add(pr)
    '    Catch

    '    End Try
    'End Sub
    Public waiting_result As Boolean = False

   

    Private Sub btnPreview_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnPreview.Click

        Dim slog = New bc_cs_activity_log(FORM_NAME, "btnPreview_Click", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
          
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            Me.pPreview.Enabled = False
            Me.clear_preview()

            Me.lvResults.Items.Clear()
            Me.Refresh()

            boeAggregation.boolLoadAggregationPreview = True
            boeAggregation.brunnow = False

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                boeAggregation.db_read()
                
            Else
                boeAggregation.tmode = bc_cs_soap_base_class.tREAD
                If boeAggregation.transmit_to_server_and_receive(boeAggregation, True) = False Then
                    Exit Sub
                End If
            End If
            boeAggregation.boolLoadAggregationPreview = False
        
            loadPreview_first()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "btnPreview_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.pPreview.Enabled = True
            boeAggregation.boolLoadAggregationPreview = False
            slog = New bc_cs_activity_log(FORM_NAME, "btnPreview_Click", bc_cs_activity_codes.TRACE_EXIT, "")
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try

    End Sub
    Public gpreview_results As Object
    Public preview_loading As Boolean = False
    Private Sub loadPreview_first()
        Dim j As Integer
        preview_loading = True
        lvResults.Items.Clear()
        Me.cbPreviewContributor.Items.Clear()
        Me.cbPreviewPublish.Items.Clear()
        Me.cbPreviewCalculation.Items.Clear()
        Me.uxPeriod.Items.Clear()
        Me.uxYear.Items.Clear()
        Me.uxEntity.Items.Clear()

        Me.cbPreviewContributor.Items.Add("All")
        Me.cbPreviewPublish.Items.Add("All")
        Me.cbPreviewCalculation.Items.Add("All")
        Me.uxPeriod.Items.Add("All")
        Me.uxYear.Items.Add("All")
        Me.uxEntity.Items.Add("All")

        Me.cbPreviewContributor.SelectedIndex = 0
        Me.cbPreviewPublish.SelectedIndex = 0
        Me.cbPreviewCalculation.SelectedIndex = 0
        Me.uxPeriod.SelectedIndex = 0
        Me.uxYear.SelectedIndex = 0
        Me.uxEntity.SelectedIndex = 0

        Me.cbPreviewContributor.Enabled = False
        Me.uxEntity.Enabled = False
        Me.cbPreviewCalculation.Enabled = False
        Me.uxYear.Enabled = False
        Me.uxPeriod.Enabled = False
        Me.cbPreviewPublish.Enabled = False

        Me.Cdps.Items.Clear()
        For i = 0 To 8
            Me.Cdps.Items.Add(CStr(i))
        Next
        Me.Cdps.SelectedIndex = 2
        Me.Cmult.Items.Clear()
        Me.Cmult.Items.Add("Unit")
        Me.Cmult.Items.Add("m")
        Me.Cmult.Items.Add("100's")
        Me.Cmult.Items.Add("1000's")
        Me.Cmult.Items.Add("%")
        Me.Cmult.SelectedIndex = 0







        gpreview_results = boeAggregation.oPreview
        If IsArray(gpreview_results) Then
            If UBound(gpreview_results, 1) = 1 Then
                Dim omsg As New bc_cs_message("Blue Curve", CStr(gpreview_results(0, 0)), bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Else

                For i As Integer = 0 To gpreview_results.GetUpperBound(1)

                    'If (cbPreviewContributor.SelectedItem Is Nothing OrElse CType(cbPreviewContributor.SelectedItem, KeyValuePair(Of String, String)).Value = vres(4, i)) _
                    '    And (cbPreviewCalculation.SelectedItem Is Nothing OrElse CType(cbPreviewCalculation.SelectedItem, bc_om_calculation).id = vres(12, i)) Then

                    Dim str(6) As String
                    'Dim itm As ListViewItem
                    If Not gpreview_results(6, i) Is DBNull.Value Then
                        str(0) = gpreview_results(6, i)
                    End If
                    If Not gpreview_results(1, i) Is DBNull.Value Then
                        str(1) = gpreview_results(1, i)
                    End If
                    If Not gpreview_results(11, i) Is DBNull.Value Then
                        str(2) = gpreview_results(11, i)
                    End If
                    If Not gpreview_results(10, i) Is DBNull.Value Then
                        str(3) = gpreview_results(10, i)
                    End If
                    If Not gpreview_results(16, i) Is DBNull.Value Then
                        str(4) = gpreview_results(16, i)
                    End If

                    If Not gpreview_results(17, i) Is DBNull.Value Then
                        str(6) = gpreview_results(17, i)
                    End If
                    If Not gpreview_results(7, i) Is DBNull.Value Then
                        If gpreview_results(7, i) = 1 Then
                            str(5) = "Draft"
                        ElseIf gpreview_results(7, i) = 8 Then
                            str(5) = "Publish"
                        End If
                    End If

                    REM load filters
                    REM year
                    Dim found As Boolean
                    found = False
                    For j = 0 To Me.uxYear.Items.Count - 1
                        If Me.uxYear.Items(j) = str(1) Then
                            found = True
                        End If
                    Next
                    If found = False Then
                        Me.uxYear.Items.Add(str(1))
                    End If
                    REM period
                    found = False
                    For j = 0 To Me.uxPeriod.Items.Count - 1
                        If Me.uxPeriod.Items(j) = str(2) Then
                            found = True
                        End If
                    Next
                    If found = False Then
                        Me.uxPeriod.Items.Add(str(2))
                    End If
                    REM pitem
                    found = False
                    For j = 0 To Me.cbPreviewCalculation.Items.Count - 1
                        If Me.cbPreviewCalculation.Items(j) = str(4) Then
                            found = True
                        End If
                    Next
                    If found = False Then
                        Me.cbPreviewCalculation.Items.Add(str(4))
                    End If
                    REM stage
                    found = False
                    For j = 0 To Me.cbPreviewPublish.Items.Count - 1
                        If Me.cbPreviewPublish.Items(j) = str(5) Then
                            found = True
                        End If
                    Next
                    If found = False Then
                        Me.cbPreviewPublish.Items.Add(str(5))
                    End If
                    REM contributor
                    found = False
                    For j = 0 To Me.cbPreviewContributor.Items.Count - 1
                        If Me.cbPreviewContributor.Items(j) = str(6) Then
                            found = True
                        End If
                    Next
                    If found = False Then
                        Me.cbPreviewContributor.Items.Add(str(6))
                    End If
                    REM Entity
                    found = False
                    For j = 0 To Me.uxEntity.Items.Count - 1
                        If Me.uxEntity.Items(j) = str(3) Then
                            found = True
                        End If
                    Next
                    If found = False Then
                        Me.uxEntity.Items.Add(str(3))
                    End If
                Next
            End If
            If Me.cbPreviewPublish.Items.Count = 2 Then
                Me.cbPreviewPublish.SelectedIndex = 1
            Else
                Me.cbPreviewPublish.Enabled = True
            End If

            If Me.cbPreviewCalculation.Items.Count = 2 Then
                Me.cbPreviewCalculation.SelectedIndex = 1
            Else
                Me.cbPreviewCalculation.Enabled = True
            End If
            If Me.cbPreviewContributor.Items.Count = 2 Then
                Me.cbPreviewContributor.SelectedIndex = 1
            Else
                Me.cbPreviewContributor.Enabled = True
            End If
            If Me.uxEntity.Items.Count = 2 Then
                Me.uxEntity.SelectedIndex = 1
            Else
                Me.uxEntity.Enabled = True
            End If
            If Me.cbPreviewCalculation.Items.Count = 2 Then
                Me.cbPreviewCalculation.SelectedIndex = 1
            Else
                Me.cbPreviewCalculation.Enabled = True
            End If
            If Me.uxYear.Items.Count = 2 Then
                Me.uxYear.SelectedIndex = 1
            Else
                Me.uxYear.Enabled = True
            End If
            If Me.uxPeriod.Items.Count = 2 Then
                Me.uxPeriod.SelectedIndex = 1
            Else
                Me.uxPeriod.Enabled = True
            End If
            Me.Cdps.Enabled = True
            Me.Cmult.Enabled = True

            preview_loading = False
            loadPreview()
        End If
        preview_loading = False
    End Sub
    Private Sub loadPreview()

        Try
            If preview_loading = True Then
                Exit Sub
            End If
            Dim itm As ListViewItem
            Dim str(9) As String
            Dim mult(5) As Decimal
            Dim res As Decimal

            mult(0) = 1
            mult(1) = 1000000
            mult(2) = 100
            mult(3) = 1000
            mult(4) = 0.01

            lvResults.Items.Clear()


            If IsArray(gpreview_results) Then

                For i As Integer = 0 To gpreview_results.GetUpperBound(1)
                    If Not gpreview_results(6, i) Is DBNull.Value Then
                        Try
                            res = CDec(gpreview_results(6, i))
                            res = res / mult(Me.Cmult.SelectedIndex)

                            res = Decimal.Round(res, Me.Cdps.SelectedIndex)

                            str(0) = CStr(res)

                        Catch
                            str(0) = gpreview_results(6, i)
                        End Try

                    End If
                    If Not gpreview_results(1, i) Is DBNull.Value Then
                        str(1) = gpreview_results(1, i)
                    End If
                    If Not gpreview_results(11, i) Is DBNull.Value Then
                        str(2) = gpreview_results(11, i)
                    End If
                    If Not gpreview_results(10, i) Is DBNull.Value Then
                        str(3) = gpreview_results(10, i)
                    End If
                    If Not gpreview_results(16, i) Is DBNull.Value Then
                        str(4) = gpreview_results(16, i)
                    End If
                    If Not gpreview_results(17, i) Is DBNull.Value Then
                        str(6) = gpreview_results(17, i)
                    End If
                    If Not gpreview_results(7, i) Is DBNull.Value Then
                        If gpreview_results(7, i) = 1 Then
                            str(5) = "Draft"
                        ElseIf gpreview_results(7, i) = 8 Then
                            str(5) = "Publish"
                        End If
                    End If

                    str(7) = gpreview_results(13, i)
                    str(8) = gpreview_results(14, i)
                    str(9) = gpreview_results(15, i)


                    If (Me.cbPreviewCalculation.SelectedIndex = 0 Or Me.cbPreviewCalculation.Text = str(4)) _
                    And (Me.uxYear.SelectedIndex = 0 Or Me.uxYear.Text = str(1)) _
                    And (Me.uxPeriod.SelectedIndex = 0 Or Me.uxPeriod.Text = str(2)) _
                    And (Me.uxEntity.SelectedIndex = 0 Or Me.uxEntity.Text = str(3)) _
                    And (Me.cbPreviewPublish.SelectedIndex = 0 Or Me.cbPreviewPublish.Text = str(5)) _
                    And (Me.cbPreviewContributor.SelectedIndex = 0 Or Me.cbPreviewContributor.Text = str(6)) Then
                        itm = New ListViewItem(str)
                        lvResults.Items.Add(itm)
                    End If
                Next
            End If
        Catch

        End Try

    End Sub
    Private Sub clear_preview()
        Me.cbPreviewCalculation.SelectedIndex = -1
        Me.cbPreviewPublish.SelectedIndex = -1
        Me.uxYear.SelectedIndex = -1
        Me.uxPeriod.SelectedIndex = -1
        Me.lvResults.Items.Clear()
        Me.uxEntity.SelectedIndex = -1
        Me.cbPreviewContributor.SelectedIndex = -1



        Me.cbPreviewCalculation.Enabled = False
        Me.cbPreviewPublish.Enabled = False
        Me.uxYear.Enabled = False
        Me.uxPeriod.Enabled = False
        Me.uxEntity.Enabled = False
        Me.cbPreviewCalculation.Enabled = False
        Me.cbPreviewContributor.Enabled = False
        Me.Cdps.SelectedIndex = -1
        Me.Cmult.SelectedIndex = -1

        Me.Cdps.Enabled = False
        Me.Cmult.Enabled = False


    End Sub
    Private Sub cbPreviewCalculation_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPreviewCalculation.SelectedIndexChanged
        loadPreview()
    End Sub

    Private Sub cbxPreviewPublish_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        loadPreview()
    End Sub

    Private Sub Bc_am_in_ub_listentity1_RemoveLevel(ByRef baiul As bc_am_in_ub_listentity) Handles Bc_am_in_ub_listentity1.RemoveItem

        Dim slog = New bc_cs_activity_log(FORM_NAME, "Bc_am_in_ub_listentity1_RemoveLevel", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            uxUCPanel.Controls.Remove(baiul)
            Dim prevBaiul As bc_am_in_ub_listentity
            prevBaiul = uxUCPanel.Controls(uxUCPanel.Controls.Count - 1)
            prevBaiul.uxAdd.Visible = True
            If uxUCPanel.Controls.Count > 1 Then
                prevBaiul.uxRemove.Visible = True
            End If
            setChanged(True)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "Bc_am_in_ub_listentity1_RemoveLevel", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(FORM_NAME, "Bc_am_in_ub_listentity1_RemoveLevel", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub


    Private Sub txtName_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtName.TextChanged
        If Trim(Me.txtName.Text) <> "" Then
            Me.cbType.Enabled = True

        End If
    End Sub
    Private Sub Bc_am_in_ub_listentity1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub propogate_up_from_class(ByVal class_id As Long, ByVal first As Boolean)
        Dim found As Boolean

        For i = 0 To classes.classes.Count - 1
            If classes.classes(i).class_id = class_id And (classes.classes(i).class_type_id = 1 Or classes.classes(i).class_type_id = 4) Then
                found = False
                If first = False Then
                    If classes.classes(i).class_name <> "ROOT" Then
                        For k = 0 To Me.cbTargetClass.Items.Count - 1
                            If Me.cbTargetClass.Items(k).class_name = classes.classes(i).class_name Then
                                found = True
                                Exit For
                            End If
                        Next
                        If found = False Then
                            Me.cbTargetClass.Items.Add(classes.classes(i))
                            Me.cbtargetclass2.Items.Add(classes.classes(i))
                        End If
                    End If
                End If
                For j = 0 To classes.classes(i).parent_links.count - 1
                    propogate_up_from_class(classes.classes(i).parent_links(j).parent_class_id, False)
                Next
            End If
        Next

    End Sub

    Private Sub cbType_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbType.SelectedIndexChanged

        If new_load = True Then
            Exit Sub
        End If
        Me.cbSourceClass.SelectedIndex = -1

        If Me.cbType.SelectedIndex > -1 Then
            Me.cbType.Enabled = True
            Me.cbTargetClass.Enabled = False
            Me.cbSourceClass.Enabled = True
            Me.cbStartYear.Enabled = True
            Me.cbNumberOfYears.Enabled = False
            Me.cbCurrency.Enabled = True
            Me.cbExchangeRate.Enabled = True
            Me.cbMonthEnd.Enabled = True
            Me.tcAggregation.TabPages(0).Show()
            Me.tcAggregation.TabPages(0).Select()
            If Me.cbType.SelectedIndex = 1 Then
                'Me.cbSourceClass.Enabled = True
                'Me.cbSourceClass.SelectedIndex = -1
                Me.tcAggregation.TabPages(0).Hide()
                Me.tcAggregation.TabPages(1).Select()
            End If
        End If

        If Me.cbType.SelectedIndex = 1 Then
            If tcAggregation.TabPages.Contains(tpComposition) Then
                tcAggregation.TabPages.Remove(tpComposition)
            End If
        ElseIf Not tcAggregation.TabPages.Contains(tpComposition) Then
            tcAggregation.TabPages.Add(tpComposition)
            tpComposition.Parent = tcAggregation
            For i = 0 To classes.attribute_pool.Count - 1
                For Each boav As bc_om_attribute_value In boeAggregation.attribute_values
                    If classes.attribute_pool(i).attribute_id = boav.attribute_Id Then
                        If classes.attribute_pool(i).name = "List Formula XML" Then
                            loadListComposition(boav.value)
                            Exit For
                        End If
                    End If
                Next
            Next
        End If

        tcAggregation.TabPages.Remove(tpPreview)
        tcAggregation.TabPages.Add(tpPreview)
    End Sub

    Private Sub cbStartYear_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbStartYear.SelectedIndexChanged
        Me.cbNumberOfYears.Enabled = False
        If Me.cbStartYear.SelectedIndex > 0 Then
            Me.cbNumberOfYears.Enabled = True
        Else
            Me.cbNumberOfYears.SelectedIndex = -1
        End If
    End Sub

    Private Sub cbSourceClass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbSourceClass.SelectedIndexChanged
        Dim slog = New bc_cs_activity_log(FORM_NAME, "cbTargetClass_SelectedIndexChanged", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Me.cbTargetClass.Enabled = False
            Me.cbTargetClass.SelectedIndex = -1
            If cbType.SelectedIndex = 1 Then
                REM get progated list from targe class
                Me.cbTargetClass.Enabled = True
                Me.cbTargetClass.SelectedIndex = -1
                Me.cbTargetClass.Items.Clear()
                Me.cbtargetclass2.SelectedIndex = -1
                Me.cbtargetclass2.Items.Clear()
                
                For i = 0 To classes.classes.Count - 1
                    If classes.classes(i).class_name = cbSourceClass.Text And (classes.classes(i).class_type_id = 1 Or classes.classes(i).class_type_id = 4) Then
                        propogate_up_from_class(classes.classes(i).class_id, True)
                        Exit For
                    End If
                Next
            End If



            'If Not cbSourceClass.SelectedItem Is Nothing AndAlso Not cbTargetClass.SelectedItem.ToString = "" Then
            '    If tcAggregation.TabPages.Contains(tpComposition) Then
            '        tcAggregation.TabPages.Remove(tpComposition)
            '    End If
            'ElseIf Not tcAggregation.TabPages.Contains(tpComposition) Then
            '    tcAggregation.TabPages.Add(tpComposition)
            '    tpComposition.Parent = tcAggregation
            '    For i = 0 To classes.attribute_pool.Count - 1
            '        For Each boav As bc_om_attribute_value In boeAggregation.attribute_values
            '            If classes.attribute_pool(i).attribute_id = boav.attribute_Id Then
            '                If classes.attribute_pool(i).name = "List Formula XML" Then
            '                    loadListComposition(boav.value)
            '                    Exit For
            '                End If
            '            End If
            '        Next
            '    Next
            'End If

            'tcAggregation.TabPages.Remove(tpPreview)
            'tcAggregation.TabPages.Add(tpPreview)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "cbSourceClass_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log(FORM_NAME, "cbSourceClass_SelectedIndexChanged", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub cbCurrency_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbCurrency.SelectedIndexChanged

    End Sub




    Private Sub Bc_am_in_ub_listentity1_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles Bc_am_in_ub_listentity1.LostFocus

    End Sub

    Private Sub Bc_am_in_ub_listentity1_Load_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bc_am_in_ub_listentity1.Load
        Bc_am_in_ub_listentity1.uxRemove.Visible = False
    End Sub

    Private Sub uxEntity_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxEntity.SelectedIndexChanged
        loadPreview()

    End Sub

    Private Sub uxYear_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxYear.SelectedIndexChanged
        loadPreview()
    End Sub

    Private Sub cbPreviewPublish_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPreviewPublish.SelectedIndexChanged
        loadPreview()
    End Sub

    Private Sub uxPeriod_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxPeriod.SelectedIndexChanged
        loadPreview()
    End Sub

    Private Sub cbPreviewContributor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbPreviewContributor.SelectedIndexChanged
        loadPreview()
    End Sub

    Private Sub Cdps_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cdps.SelectedIndexChanged
        loadPreview()
    End Sub

    Private Sub Cmult_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cmult.SelectedIndexChanged
        loadPreview()
    End Sub

    Private Sub uxAudit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAudit.Click

        Dim DetailUniverse As String
        Dim DetailBatch As String

        Dim slog = New bc_cs_activity_log("bc_am_in_settings", "uxAudit_Click", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Me.Cursor = Cursors.WaitCursor

            'If Me.lvResults.Items.Count > 0 Then
            DetailUniverse = Me.txtName.Text
            DetailBatch = "09-09-9999"
            Dim oAuditDetails As New bc_am_audit_details
            oAuditDetails.DetailLogUniverse = DetailUniverse
            oAuditDetails.DetailLogBatch = DetailBatch
            oAuditDetails.ShowDialog()
            'End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_settings", "uxAudit_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_in_settings", "uxAudit_Click", bc_cs_activity_codes.TRACE_EXIT, "")
            Me.Cursor = Cursors.Default
        End Try


    End Sub

    Private Sub lvResults_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvResults.SelectedIndexChanged

    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim slog As New bc_cs_activity_log(FORM_NAME, "btnrun_Click", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim omsg As New bc_cs_message("Blue Curve", "This action will run the aggregation universe asynchronously. Progress can be checked via the audit log. Are you sure you wish to continue?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Exit Sub
            End If


            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False

            boeAggregation.boolLoadAggregationPreview = False
            boeAggregation.brunnow = True

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                boeAggregation.db_read()

            Else
                boeAggregation.tmode = bc_cs_soap_base_class.tREAD
                boeAggregation.async = True
                If boeAggregation.transmit_to_server_and_receive(boeAggregation, True) = False Then
                    Exit Sub
                End If
                boeAggregation.async = False
            End If
            boeAggregation.brunnow = False

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log(FORM_NAME, "btnrun_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            boeAggregation.boolLoadAggregationPreview = False
            slog = New bc_cs_activity_log(FORM_NAME, "btnrun_Click", bc_cs_activity_codes.TRACE_EXIT, "")
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub aggregation_results_ready(ByVal results As ArrayList) Handles Bc_am_calc_search1.results_ready
        lvAggregations.Items.Clear()
        Dim lv As ListViewItem
        For j = 0 To results.Count - 1
            For i = 0 To lAggregations.Count - 1
                If lAggregations(i).name = results(j) Then
                    lv = New ListViewItem(CStr(lAggregations(i).name), 0)
                    If entities.entity(i).inactive = True Then
                        lv.SubItems.Add("no")
                    Else
                        lv.SubItems.Add("yes")
                    End If
                    lvAggregations.Items.Add(lv)
                    Exit For
                End If
            Next
        Next
        from_search = True
        clearControls()
        Me.btnCancel.Enabled = False
        Me.btnNew.Enabled = True
        Me.btnactive.Enabled = False
        Me.btnDelete.Enabled = False
        Me.btnCalculations.Enabled = False
        Me.txtName.Enabled = False
        from_search = False
    End Sub

    Public Sub New()

        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        DoubleBuffered = True

        ' Create an instance of a ListView column sorter and assign it 
        ' to the ListView control.
        lvwColumnSorter = New ListViewColumnSorter
        lvAggregations.ListViewItemSorter = lvwColumnSorter

    End Sub

    Private Sub uxDetails_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDetails.Enter

    End Sub
    Public loading As Boolean = False
   

    Private Sub cbTargetClass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbTargetClass.SelectedIndexChanged
        Me.cbtargetclass2.Enabled = False
        Me.cbtargetclass2.SelectedIndex = -1
        If cbTargetClass.SelectedIndex > -1 Then
            Me.cbtargetclass2.Enabled = True
        End If

    End Sub
End Class