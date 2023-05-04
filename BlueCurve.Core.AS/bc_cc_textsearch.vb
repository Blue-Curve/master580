Imports BlueCurve.Core.OM
Imports BlueCurve.Core.CS
Imports System.Windows.Forms

REM EFG JULY 2013
Public Class BlueCurve_TextSearch

    Dim Updateing As Boolean = False
    Dim SearchTextValue As String
    Dim SearchControlValue As String
    Dim ExcludeControlValue As String
    Dim AttributeSearchValue As Integer = 2
    Dim SearchClassValue As Integer
    Dim SearchAttributeValue As String = ""
    Dim SearchonAttributeName As String
    Dim SearchonAttributeId As String
    Dim UserEntitiesOnly As Boolean = False
    Dim BuildEntitiesOnly As Boolean = False
    Dim FireSearchEventOnly As Boolean = False
    Dim TimerOff As Boolean = False
    Dim GetInactive As Boolean = False
    Public AttributePool As New ArrayList
    Public ShownEntityList As New ArrayList
    Dim search_attributes As New bc_om_search_attributes
    Dim FilterContolItems As New ArrayList
    Dim bshowinactive As Boolean = True
    Public inactive As Boolean = True
    Public filter_attribute_id As Long = 0
    Public Event FireSearch(ByVal sender As Object, ByVal e As EventArgs)
    Public Event AttributeChanged(ByVal sender As Object, ByVal e As EventArgs)

    Sub New()
        InitializeComponent()
    End Sub

    Public Property SearchText() As String
        Get
            Return SearchTextValue
        End Get
        Set(ByVal value As String)
            SearchTextValue = value
            uxTextFilter.Text = value
        End Set
    End Property

    Public Property SearchControl() As String
        Get
            Return SearchControlValue
        End Get
        Set(ByVal value As String)
            SearchControlValue = value
        End Set
    End Property
    Public Property ExcludeControl() As String
        Get
            Return ExcludeControlValue
        End Get
        Set(ByVal value As String)
            ExcludeControlValue = value
        End Set
    End Property
    Public Property showinactive() As Integer
        Get
            Return bshowinactive
        End Get
        Set(ByVal value As Integer)
            bshowinactive = value
        End Set
    End Property
    Public Property SearchClass() As Integer
        Get
            Return SearchClassValue
        End Get
        Set(ByVal value As Integer)
            SearchClassValue = value
        End Set
    End Property

    Public Property SearchAttributeList() As String
        Get
            Return SearchAttributeValue
        End Get
        Set(ByVal value As String)
            SearchAttributeValue = value
        End Set
    End Property

    Public Property SearchAttributes() As Integer

        REM 0  Entity Text Search
        REM 1  Attribute Search with dropdown of atrribute type
        REM 2  Silent Attribute Search & Text Search
        REM 3  Text search only 
        Get
            Return AttributeSearchValue
        End Get

        Set(ByVal value As Integer)
            AttributeSearchValue = value

            If value = 1 Then
                Me.uxAttribSelect.Visible = True
                uxTextFilter.Left = uxAttribSelect.Width + 40
                uxTextFilter.Width = Me.Width - uxAttribSelect.Width + 40
            End If

            If value = 0 Or value = 2 Or value = 3 Then
                Me.uxAttribSelect.Visible = False
                uxTextFilter.Left = 40
                uxTextFilter.Width = Me.Width - 40
            End If
        End Set

    End Property


    Public Property SearchCurrentAttribute() As String
        Get
            Return SearchonAttributeName
        End Get
        Set(ByVal value As String)
            SearchonAttributeName = value

            For j = 0 To Me.uxAttribSelect.Items.Count - 1
                If Me.uxAttribSelect.Items(j) = value Then
                    Me.uxAttribSelect.SelectedIndex = j
                End If
            Next

        End Set
    End Property


    Public Property SearchUserEntitiesOnly() As Boolean
        Get
            Return UserEntitiesOnly
        End Get
        Set(ByVal value As Boolean)
            UserEntitiesOnly = value

            Updateing = True
            Me.uxTextFilter.Text = ""
            Me.SearchText = ""
            Updateing = False

        End Set
    End Property

    Public Property SearchFireEventOnly() As Boolean
        Get
            Return FireSearchEventOnly
        End Get
        Set(ByVal value As Boolean)
            FireSearchEventOnly = value
        End Set
    End Property

    Public Property SearchBuildEntitiesOnly() As Boolean
        Get
            Return BuildEntitiesOnly
        End Get
        Set(ByVal value As Boolean)
            BuildEntitiesOnly = value
        End Set
    End Property
    Public Property SearchTimerOff() As Boolean
        Get
            Return TimerOff
        End Get
        Set(ByVal value As Boolean)
            TimerOff = value
        End Set
    End Property

    Public Property SearchGetInactive() As Boolean
        Get
            Return GetInactive
        End Get
        Set(ByVal value As Boolean)
            GetInactive = value
        End Set
    End Property

    Private Sub TextSearch_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize

        If Me.SearchAttributes = 1 Then
            uxTextFilter.Left = uxAttribSelect.Width + 40
            uxTextFilter.Width = Me.Width - uxAttribSelect.Width + 40
        Else
            uxTextFilter.Left = 40
            uxTextFilter.Width = Me.Width - 40
        End If
    End Sub

    Private Sub uxTextFilter_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles uxTextFilter.KeyPress

        If (Asc(e.KeyChar) = 13) Then
            SearchTimer.Stop()
            SearchText = uxTextFilter.Text
            RunSearch()
            'uxTextFilter.Text = SearchText
            e.Handled = True
        End If
    End Sub

    Private Sub uxTextFilter_LostFocus(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxTextFilter.LostFocus

        If uxTextFilter.Text <> "" And Updateing = False And SearchTimer.Enabled = True Then
            SearchTimer.Stop()
            SearchText = uxTextFilter.Text
            RunSearch()
        End If
    End Sub

    Private Sub TextFilter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTextFilter.TextChanged
        If Updateing = False And TimerOff = False Then
            SearchTimer.Stop()
            SearchTimer.Start()
        End If
    End Sub

    Private Sub AttribSelect_Resize(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxAttribSelect.Resize
        'TextFilter.Left = Me.Width + 40
    End Sub

    Private Sub SearchTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SearchTimer.Tick
        SearchTimer.Stop()
        SearchText = uxTextFilter.Text
        RunSearch(False)
    End Sub

    Public Sub SearchSetup(Optional ByVal RebuildEntities As Boolean = True)

        REM Setup the control 
        REM This must be called in the load event of the host form.

        Try

            Dim entityClass As New bc_om_entity_class
            Dim entity_list As New bc_om_entities
            Dim oattribute As bc_om_search_attribute

            Updateing = True

            If SearchAttributes = 3 Then
                SearchAttributes = 3
            Else
                REM If no class set use Instrument
                If SearchClassValue = 0 Then
                    Me.SearchClassValue = 5
                    entity_list = bc_am_load_objects.obc_entities
                    For i = 0 To entity_list.entity.Count() - 1
                        If InStr(UCase(entity_list.entity(i).class_name.ToString), "INSTRUMENT") <> 0 Then
                            Me.SearchClassValue = entity_list.entity(i).class_id
                            Exit For
                        End If
                    Next
                End If

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then

                    search_attributes.db_read()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    search_attributes.tmode = bc_cs_soap_base_class.tREAD
                    If search_attributes.transmit_to_server_and_receive(search_attributes, True) = False Then
                        Exit Sub
                    End If
                Else
                    Exit Sub
                End If

                If search_attributes.search_attributes.Count = 0 Then
                    If Not bc_cs_central_settings.search_attributes Is Nothing Then
                        If bc_cs_central_settings.search_attributes = True Then
                            SearchAttributes = 1
                        Else
                            SearchAttributes = 0
                        End If
                        SearchAttributes = bc_cs_central_settings.search_attributes
                    End If
                    If Not bc_cs_central_settings.search_attributes_list Is Nothing Then
                        SearchAttributeList = bc_cs_central_settings.search_attributes_list
                    End If
                Else
                    SearchAttributes = 2
                    SearchAttributeList = ""
                    For Each oattribute In search_attributes.search_attributes
                        If SearchClassValue = oattribute.class_id Then
                            If SearchAttributeList <> "" Then
                                SearchAttributeList = SearchAttributeList + ","
                            End If
                            SearchAttributeList = SearchAttributeList + oattribute.attribute_Id.ToString
                        End If
                    Next
                End If

                REM Set up objects

                REM entities
                'bc_am_load_objects.obc_entities.search_attributes.search_attributes = Me.SearchAttributeValue
                'bc_am_load_objects.obc_entities.search_attributes.search_class = Me.SearchClass
                'If RebuildEntities = True Then
                '    bc_am_load_objects.obc_entities.search_attributes.search_values.Clear()
                '    EntityRefresh()
                'End If

                'REM entities
                'If RebuildEntities = True Then
                '    bc_am_load_objects.obc_entities.search_attributes.search_attributes = Me.SearchAttributeValue
                '    bc_am_load_objects.obc_entities.search_attributes.search_class = Me.SearchClass
                '    bc_am_load_objects.obc_entities.search_attributes.search_values.Clear()
                '    EntityRefresh()
                'Else
                '    bc_am_load_objects.obc_entities.search_attributes.search_attributes = Me.SearchAttributeValue
                '    bc_am_load_objects.obc_entities.search_attributes.search_class = Me.SearchClass
                '    bc_am_load_objects.obc_entities.search_attributes.search_values.Clear()
                '    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                '        bc_am_load_objects.obc_entities.search_attributes.db_read()
                '    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                '        bc_am_load_objects.obc_entities.search_attributes.tmode = bc_cs_soap_base_class.tREAD
                '        If bc_am_load_objects.obc_entities.search_attributes.transmit_to_server_and_receive(bc_am_load_objects.obc_entities.search_attributes, True) = False Then
                '            Exit Sub
                '        End If
                '    End If
                'End If

                If Me.SearchUserEntitiesOnly = True Then
                    REM preferances
                    'bc_am_load_objects.obc_prefs = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PREFS_FILENAME)
                    bc_am_load_objects.obc_prefs = bc_am_load_objects.obc_prefs.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.PREFS_FILENAME)
                End If


                REM If attribute search on build list of attributes for class
                If SearchAttributes = 1 Then
                    AttributeControlBuild(True)
                End If

            End If

        Catch ex As Exception

        Finally
            Updateing = False
        End Try

    End Sub
    Public Sub AttributeControlBuild(ByVal buildPool As Boolean)

        Dim entityClass As New bc_om_entity_class

        entityClass.class_id = Me.SearchClassValue
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            entityClass.db_read()
        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
            entityClass.tmode = bc_cs_soap_base_class.tREAD
            If entityClass.transmit_to_server_and_receive(entityClass, True) = False Then
                Exit Sub
            End If
        End If

        Me.uxAttribSelect.Items.Clear()

        If buildPool = True Then
            Me.AttributePool.Clear()
        End If
        Me.uxAttribSelect.Items.Add("Name")

        If Me.SearchAttributeList <> "" Then
            Dim oatt As bc_om_class_attribute
            For i = 0 To entityClass.attributes.Count - 1
                oatt = entityClass.attributes(i)
                If InStr(Me.SearchAttributeList, oatt.attribute_id.ToString) > 0 Then
                    Me.uxAttribSelect.Items.Add(oatt.attribute_name)
                    If buildPool = True Then
                        Me.AttributePool.Add(oatt)
                    End If
                End If
            Next
        End If

        Me.SearchonAttributeName = "Name"
        Me.SearchonAttributeId = ""
        'Me.uxAttribSelect.SelectedIndex = 0

    End Sub
    Public Sub AttributeValueListBuild()

        REM Build Attribute List
        Try

            Dim SearchEntity As bc_om_entity
            Dim EntityClass As New bc_om_entity_class
            Dim SearchAttribute As bc_om_attribute_value = Nothing
            Dim entity_list As New bc_om_entities

            entity_list = bc_am_load_objects.obc_entities
            For i = 0 To entity_list.entity.Count() - 1
                SearchEntity = entity_list.entity(i)
                If SearchEntity.class_id = SearchClass Then
                    For j = 0 To Me.AttributePool.Count - 1
                        SearchAttribute = New bc_om_attribute_value
                        SearchAttribute.entity_id = entity_list.entity(i).id
                        SearchAttribute.attribute_Id = Me.AttributePool(j).attribute_Id
                        SearchAttribute.submission_code = 1

                        SearchAttribute.tmode = bc_cs_soap_base_class.tREAD

                        If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                            SearchAttribute.db_read()
                        ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                            SearchAttribute.tmode = bc_cs_soap_base_class.tREAD
                            If SearchAttribute.transmit_to_server_and_receive(SearchAttribute, True) = False Then
                                Exit Sub
                            End If
                        Else
                            Dim omsg As New bc_cs_message("Blue Curve", "Cannot connect to " + bc_cs_central_settings.soap_server + " application cannot be launched.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Sub
                        End If

                        entity_list.entity(i).attribute_values.add(SearchAttribute)
                    Next
                    Application.DoEvents()

                End If
            Next

        Catch ex As Exception

        End Try

    End Sub

    Public Sub SearchRefresh(Optional ByVal RebuildEntities As Boolean = False, Optional do_search_entities As Boolean = True)

        SearchTimer.Stop()
        RunSearch(RebuildEntities, do_search_entities)

    End Sub

    Public Sub RunSearch(Optional ByVal RebuildEntities As Boolean = False, Optional do_search_attributes As Boolean = True)

        Try

            Dim filterControl As Object = Nothing
            Dim excludeControl As Object = Nothing
            Dim searchEntity As bc_om_entity
            Dim entityClass As New bc_om_entity_class
            Dim searchAttribute As bc_om_attribute_value = Nothing
            Dim entity_list As New bc_om_entities
            Dim attribOk As Boolean
            Dim oent As bc_om_search_attribute_for_entity

            Dim lvew As ListViewItem = Nothing
            Dim data_icon As Integer = 3
            Dim inactive_icon As Integer = 4
            Dim EntityAdded As Boolean = False

            Dim tValues(0) As bc_om_search_attribute_for_entity
            Dim iee As Integer = 0
            Dim match As Boolean = False
            Updateing = True




            If Me.Enabled = False Then
                Exit Sub
            End If



            If SearchClass <= 0 And SearchAttributes <> 3 Then
                Exit Sub
            End If



            If FireSearchEventOnly Then
                Exit Sub
            End If


            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.WaitCursor

            If RebuildEntities = True Then
                EntityRefresh(do_search_attributes)
            End If

            'entity_list = bc_am_load_objects.obc_entities

            REM
            'entity_list.search_attributes = bc_am_load_objects.obc_entities.search_attributes

            entity_list.entity.Clear()



            REM filter inactive active
            If filter_attribute_id = 0 Then

                For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                    If bc_am_load_objects.obc_entities.entity(i).class_id = SearchClass Then
                        If inactive = True Then
                            entity_list.entity.Add(bc_am_load_objects.obc_entities.entity(i))
                        ElseIf bc_am_load_objects.obc_entities.entity(i).inactive = False Then
                            entity_list.entity.Add(bc_am_load_objects.obc_entities.entity(i))
                        End If
                    End If
                Next
            Else
                For i = 0 To bc_am_load_objects.obc_entities.filter_attributes.Count - 1
                    With bc_am_load_objects.obc_entities.filter_attributes(i)
                        If .attribute_id = filter_attribute_id AndAlso .class_id = SearchClass Then
                            If inactive = True Then
                                entity_list.entity.Add(bc_am_load_objects.obc_entities.filter_attributes(i))
                            ElseIf bc_am_load_objects.obc_entities.filter_attributes(i).inactive = False Then
                                entity_list.entity.Add(bc_am_load_objects.obc_entities.filter_attributes(i))

                            End If
                        End If
                    End With

                Next
            End If


            REM extended filter
            If SearchUserEntitiesOnly = False Then

                If bc_cs_central_settings.alt_entity_for_build = True And InStr(Diagnostics.Process.GetCurrentProcess.MainModule.ModuleName, "CommonPlatform") = 0 Then
                    For i = 0 To bc_am_load_objects.obc_entities.alternate_entity_list.entity.Count - 1
                        entity_list.entity.Add(bc_am_load_objects.obc_entities.alternate_entity_list.entity(i))
                    Next
                End If
            Else

                If bc_cs_central_settings.alt_entity_for_apref = True And InStr(Diagnostics.Process.GetCurrentProcess.MainModule.ModuleName, "CommonPlatform") = 0 Then
                    For i = 0 To bc_am_load_objects.obc_entities.alternate_entity_list.entity.Count - 1
                        For j = 0 To bc_am_load_objects.obc_prefs.pref.Count - 1
                            If bc_am_load_objects.obc_prefs.pref(i).class_id = SearchClass AndAlso bc_am_load_objects.obc_prefs.pref(i).entity_id = bc_am_load_objects.obc_entities.alternate_entity_list.entity(i).id Then
                                entity_list.entity.Add(bc_am_load_objects.obc_entities.alternate_entity_list.entity(i))
                                Exit For
                            End If
                        Next
                    Next
                Else

                    entity_list.entity.Clear()
                    Dim ooent As bc_om_entity
                    For i = 0 To bc_am_load_objects.obc_prefs.pref.Count - 1
                        If bc_am_load_objects.obc_prefs.pref(i).class_id = SearchClass Then
                            ooent = New bc_om_entity
                            ooent.id = bc_am_load_objects.obc_prefs.pref(i).entity_id
                            ooent.name = bc_am_load_objects.obc_prefs.pref(i).entity_name
                            ooent.class_id = SearchClass
                            ooent.inactive = bc_am_load_objects.obc_prefs.pref(i).inactive
                            'ooent.show_mode = bc_am_load_objects.obc_prefs.pref(i).show_mode
                            entity_list.entity.Add(ooent)
                        End If

                    Next
                End If
            End If

            '-----------------------------
            REM Simple Text search
            '-----------------------------
            If Me.SearchAttributes = 3 Then

                filterControl = GetControlFromName(Me.Parent, Me.SearchControl)
                filterControl.BeginUpdate()

                For x = 0 To FilterContolItems.Count - 1
                    REM Listview
                    If TypeOf filterControl Is ListView Then
                        lvew = New ListViewItem(FilterContolItems(x).ToString, data_icon)
                        filterControl.Items.Add(lvew)
                    Else
                        filterControl.Items.Add(FilterContolItems(x).ToString)
                        filterControl.Sorted = True
                    End If
                Next
                FilterContolItems.Clear()

                Dim i As Integer = 0
                Do While i <= filterControl.Items.count - 1
                    If Len(Me.SearchText) = 0 Or InStr(UCase(filterControl.Items(i)), UCase(Me.SearchText)) <> 0 Then
                        i = i + 1
                    Else
                        FilterContolItems.Add(filterControl.Items(i).ToString)
                        filterControl.Items.remove(filterControl.Items(i))
                    End If
                Loop
                filterControl.EndUpdate()
            End If

            '--------------------------------
            REM Mode 0 Old style Text search
            '--------------------------------
            If (Me.SearchAttributes = 0 And bc_am_load_objects.obc_entities.entity.Count() > 0) _
                Or (Me.SearchAttributes = 1 And Me.SearchonAttributeName = "Name") Then

                ShownEntityList.Clear()
                If Me.SearchControl <> "" Then
                    If BuildEntitiesOnly = False Then
                        filterControl = GetControlFromName(Me.Parent, Me.SearchControl)
                        filterControl.BeginUpdate()
                        filterControl.Items.clear()
                    End If
                End If

                For i = 0 To entity_list.entity.Count() - 1
                    If Len(Me.SearchText) = 0 Or InStr(UCase(entity_list.entity(i).name), UCase(Me.SearchText)) <> 0 Then

                        If entity_list.entity(i).class_id = SearchClassValue Then

                            If Me.SearchUserEntitiesOnly = False Then

                                If bc_am_load_objects.obc_entities.entity(i).show_mode = 0 Then

                                    REM Listview
                                    If TypeOf filterControl Is ListView Then

                                        If BuildEntitiesOnly = False Then

                                            If entity_list.entity(i).inactive = False Then
                                                lvew = New ListViewItem(CStr(entity_list.entity(i).name), data_icon)
                                            Else
                                                lvew = New ListViewItem(CStr(entity_list.entity(i).name + " (inactive)"), inactive_icon)
                                            End If
                                            filterControl.Items.Add(lvew)
                                        End If

                                        ShownEntityList.Add(entity_list.entity(i).id)
                                    Else
                                        If Me.SearchControl <> "" Then
                                            If BuildEntitiesOnly = False Then
                                                filterControl.Items.Add(entity_list.entity(i).name + " " + bc_am_load_objects.obc_entities.entity(i).show_text)
                                            End If
                                        End If
                                        ShownEntityList.Add(entity_list.entity(i).id)
                                    End If

                                End If

                            Else
                                'For j = 0 To bc_am_load_objects.obc_prefs.pref.Count - 1
                                'If bc_am_load_objects.obc_prefs.pref(j).entity_id = entity_list.entity(i).id Then

                                REM Listview
                                If TypeOf filterControl Is ListView Then

                                    If BuildEntitiesOnly = False Then
                                        If entity_list.entity(i).inactive = False Then
                                            lvew = New ListViewItem(CStr(entity_list.entity(i).name), data_icon)
                                        Else
                                            lvew = New ListViewItem(CStr(entity_list.entity(i).name + " (inactive)"), inactive_icon)
                                        End If
                                        filterControl.Items.Add(lvew)
                                    End If
                                    ShownEntityList.Add(entity_list.entity(i).id)
                                Else

                                    If bc_am_load_objects.obc_entities.entity(i).show_mode = 0 Then
                                        If Me.SearchControl <> "" Then
                                            If BuildEntitiesOnly = False Then
                                                filterControl.Items.Add(entity_list.entity(i).name)
                                            End If
                                        End If
                                        ShownEntityList.Add(entity_list.entity(i).id)
                                    ElseIf bc_am_load_objects.obc_entities.entity(i).show_mode = 1 Then
                                        If Me.SearchControl <> "" Then
                                            If BuildEntitiesOnly = False Then
                                                filterControl.Items.Add(entity_list.entity(i).name + " " + entity_list.entity(i).show_text)
                                            End If
                                        End If
                                        ShownEntityList.Add(entity_list.entity(i).id)
                                    End If

                                End If
                                'End If
                                'Next
                            End If
                        End If
                    End If
                Next
                If Me.SearchControl <> "" Then
                    filterControl.EndUpdate()
                End If
            End If

            '--------------------------------
            REM Old style attribute search
            '--------------------------------
            If Me.SearchAttributes = 1 And Me.SearchonAttributeName <> "Name" Then
                ShownEntityList.Clear()
                If Me.SearchControl <> "" Then
                    If BuildEntitiesOnly = False Then
                        filterControl = GetControlFromName(Me.Parent, Me.SearchControl)
                        filterControl.Items.clear()
                        filterControl.BeginUpdate()
                    End If
                End If
                For i = 0 To entity_list.entity.Count() - 1
                    searchEntity = entity_list.entity(i)
                    REM search for value
                    If searchEntity.class_id = SearchClass Then

                        attribOk = False
                        If Len(Me.SearchText) = 0 Then
                            attribOk = True
                        Else

                            'For Each oent In entity_list.search_attributes.search_values
                            '    If oent.attribute_Id = SearchonAttributeId _
                            '        And oent.entity_Id = searchEntity.id _
                            '        And oent.class_id = searchEntity.class_id Then
                            '        If InStr(UCase(oent.value), UCase(Me.SearchText)) <> 0 Then
                            '            attribOk = True
                            '            Exit For
                            '        End If
                            '    End If
                            'Next

                        End If

                        If attribOk = True Then
                            If Me.SearchUserEntitiesOnly = False Then
                                If bc_am_load_objects.obc_entities.entity(i).show_mode = 0 Then

                                    REM Listview
                                    If TypeOf filterControl Is ListView Then
                                        If BuildEntitiesOnly = False Then
                                            If entity_list.entity(i).inactive = False Then
                                                lvew = New ListViewItem(CStr(entity_list.entity(i).name), data_icon)
                                            Else
                                                lvew = New ListViewItem(CStr(entity_list.entity(i).name + " (inactive)"), inactive_icon)
                                            End If
                                            filterControl.Items.Add(lvew)
                                        End If
                                        ShownEntityList.Add(entity_list.entity(i).id)
                                    Else
                                        If Me.SearchControl <> "" Then
                                            If BuildEntitiesOnly = False Then
                                                filterControl.Items.Add(entity_list.entity(i).name + " " + bc_am_load_objects.obc_entities.entity(i).show_text)
                                            End If
                                        End If
                                        ShownEntityList.Add(entity_list.entity(i).id)
                                    End If

                                End If
                            Else
                                'For k = 0 To bc_am_load_objects.obc_prefs.pref.Count - 1
                                'If bc_am_load_objects.obc_prefs.pref(k).entity_id = entity_list.entity(i).id Then

                                REM Listview
                                If TypeOf filterControl Is ListView Then
                                    If BuildEntitiesOnly = False Then
                                        If entity_list.entity(i).inactive = False Then
                                            lvew = New ListViewItem(CStr(entity_list.entity(i).name), data_icon)
                                        Else
                                            lvew = New ListViewItem(CStr(entity_list.entity(i).name + " (inactive)"), inactive_icon)
                                        End If
                                        filterControl.Items.Add(lvew)
                                    End If
                                    ShownEntityList.Add(entity_list.entity(i).id)
                                Else

                                    If bc_am_load_objects.obc_entities.entity(i).show_mode = 0 Then
                                        If Me.SearchControl <> "" Then
                                            If BuildEntitiesOnly = False Then
                                                filterControl.Items.Add(entity_list.entity(i).name)
                                            End If
                                        End If
                                        ShownEntityList.Add(entity_list.entity(i).id)
                                    ElseIf bc_am_load_objects.obc_entities.entity(i).show_mode = 1 Then
                                        If Me.SearchControl <> "" Then
                                            If BuildEntitiesOnly = False Then
                                                filterControl.Items.Add(entity_list.entity(i).name + " " + entity_list.entity(i).show_text)
                                            End If
                                        End If
                                        ShownEntityList.Add(entity_list.entity(i).id)
                                    End If

                                End If
                                'End If
                                'Next
                            End If
                        End If
                    End If
                Next
                If Me.SearchControl <> "" Then
                    filterControl.EndUpdate()
                End If
            End If

            '--------------------------------
            'Combind Text & Attributesearch
            'Steve Wooderson 16/04/2008
            'PR rationalised this Jul 2013
            '---------------------------------
            If Me.SearchAttributes = 2 Then
                If Me.SearchControl <> "" Then
                    If BuildEntitiesOnly = False Then
                        filterControl = GetControlFromName(Me.Parent, Me.SearchControl)
                        excludeControl = GetControlFromName(Me.Parent, Me.ExcludeControl)
                        filterControl.BeginUpdate()
                        filterControl.Items.clear()
                    End If
                End If

                ShownEntityList.Clear()

                ' First Stage of Attribute Search
                ' Build list of all items that match search
                'PR XXXX
                If Me.SearchText <> "" Then
                    REM June 2017 real time search
                    Dim search_results As New bc_om_real_time_search
                    search_results.class_id = SearchClassValue
                    search_results.search_text = UCase(Me.SearchText)
                    search_results.mine = False
                    search_results.inactive = inactive
                    search_results.filter_attribute_id = 0
                    search_results.results_as_ids = True


                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        search_results.db_read()
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        search_results.tmode = bc_cs_soap_base_class.tREAD
                        search_results.transmit_to_server_and_receive(search_results, False)
                    End If
                    For i = 0 To search_results.resultsids.Count - 1
                        ReDim Preserve tValues(iee)
                        oent = New bc_om_search_attribute_for_entity
                        oent.entity_Id = search_results.resultsids(i)
                        oent.value = Me.SearchText
                        tValues(iee) = oent
                        iee = iee + 1
                    Next




                    'For Each oent In entity_list.search_attributes.search_values
                    '    If InStr(UCase(oent.value), UCase(Me.SearchText)) <> 0 Then
                    '        ReDim Preserve tValues(iee)
                    '        tValues(iee) = oent
                    '        iee = iee + 1
                    '    End If
                    'Next
                End If


                For i = 0 To entity_list.entity.Count() - 1
                    match = False
                    If entity_list.entity(i).class_id <> SearchClassValue Then
                        Continue For
                    End If

                    searchEntity = entity_list.entity(i)
                    EntityAdded = False

                    REM Check Text
                    If Len(Me.SearchText) = 0 Or InStr(UCase(entity_list.entity(i).name), UCase(Me.SearchText)) <> 0 Then
                        match = True
                    Else
                        searchEntity = entity_list.entity(i)
                        attribOk = False
                        For x = 1 To tValues.Count
                            If Not IsNothing(tValues(x - 1)) Then
                                If tValues(x - 1).entity_Id = searchEntity.id Then
                                    If InStr(UCase(tValues(x - 1).value), UCase(Me.SearchText)) <> 0 Then
                                        attribOk = True
                                        match = True
                                    End If
                                End If
                            End If
                        Next
                    End If
                    REM PR added this JULY 2013 not tos hhow selected values

                    If Not IsNothing(excludeControl) Then
                        If TypeOf excludeControl Is ListBox Then
                            For m = 0 To excludeControl.items.count - 1
                                If excludeControl.items(m) = entity_list.entity(i).name Then
                                    match = False
                                End If

                            Next
                        End If

                    End If
                    If match = False Then
                        Continue For
                    End If
                    'If Me.SearchUserEntitiesOnly = False Then

                    REM Listview
                    If TypeOf filterControl Is ListView Then
                        If BuildEntitiesOnly = False Then
                            If entity_list.entity(i).inactive = False Then
                                lvew = New ListViewItem(CStr(entity_list.entity(i).name), data_icon)
                            Else
                                If bshowinactive = True Then
                                    lvew = New ListViewItem(CStr(entity_list.entity(i).name + " (inactive)"), inactive_icon)
                                End If
                            End If
                            filterControl.Items.Add(lvew)
                        End If
                        ShownEntityList.Add(entity_list.entity(i).id)
                        EntityAdded = True
                        Continue For
                    Else
                        If BuildEntitiesOnly = False Then
                            If Me.SearchControl <> "" Then
                                If entity_list.entity(i).inactive = False Then
                                    filterControl.Items.Add(entity_list.entity(i).name)
                                Else
                                    If bshowinactive = True Then
                                        filterControl.Items.Add(entity_list.entity(i).name + " (inactive)")
                                    End If
                                End If

                            End If
                        End If
                        ShownEntityList.Add(entity_list.entity(i).id)
                        EntityAdded = True
                        Continue For
                    End If
                Next

                If Me.SearchControl <> "" Then
                    If BuildEntitiesOnly = False Then
                        filterControl.EndUpdate()
                    End If
                End If
            End If

        Catch ex As Exception
            Updateing = False

        Finally
            RaiseEvent FireSearch(Me, EventArgs.Empty)
            Updateing = False
            System.Windows.Forms.Cursor.Current = System.Windows.Forms.Cursors.Default
        End Try
    End Sub

    Function GetControlFromName(ByRef containerObj As Object, ByVal name As String) As Control
        Try
            Dim tempCtrl As Control

            For Each tempCtrl In containerObj.Controls
                If tempCtrl.Name.ToUpper.Trim = name.ToUpper.Trim Then
                    Return tempCtrl
                End If
            Next tempCtrl
            Return Nothing

        Catch ex As Exception
            Return Nothing
        End Try
    End Function


    Private Sub AttribSelect_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAttribSelect.SelectedIndexChanged

        Try

            Cursor = Cursors.WaitCursor

            If Me.uxAttribSelect.SelectedIndex = -1 Then
                Exit Sub
            End If

            SearchonAttributeName = "Name"
            SearchonAttributeId = ""

            If Updateing = False Then

                For i = 0 To AttributePool.Count - 1
                    If AttributePool(i).attribute_name = Me.uxAttribSelect.Text Then

                        'REM Rebuild attributes for new class
                        'If SearchonAttributeName <> Name And SearchonAttributeName <> AttributePool(i).attribute_name Then
                        '    AttributeValueListBuild()
                        'End If

                        SearchonAttributeName = AttributePool(i).attribute_name
                        SearchonAttributeId = (AttributePool(i).attribute_id)
                    End If
                Next

                uxTextFilter.Text = ""

                RunSearch()
                RaiseEvent AttributeChanged(Me, EventArgs.Empty)

            End If


        Catch ex As Exception

        Finally
            Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub EntityRefresh(Optional do_search_attributes As Boolean = True)
        REM Rebuild entities
        If Me.SearchGetInactive = True Or Len(SearchAttributeList) > 0 Then
            bc_am_load_objects.obc_entities.entity.Clear()
            If Me.SearchGetInactive = True Then
                bc_am_load_objects.obc_entities.get_inactive = False
            End If
            REM read in data
            'bc_am_load_objects.obc_entities.search_attributes.search_attributes = Me.SearchAttributeValue
            'bc_am_load_objects.obc_entities.search_attributes.search_class = Me.SearchClass

            REM 5.7
            bc_am_load_objects.obc_entities.entity.Clear()
            bc_am_load_objects.obc_entities.disclosure_entities.Clear()
            Dim tsa As New ArrayList


            'For i = 0 To bc_am_load_objects.obc_entities.search_attributes.search_values.Count - 1
            '    tsa.Add(bc_am_load_objects.obc_entities.search_attributes.search_values(i))
            'Next
            'bc_am_load_objects.obc_entities.search_attributes.search_values.Clear()

            bc_am_load_objects.obc_entities.do_search_attributes = do_search_attributes
            REM  End 5.7

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                bc_am_load_objects.obc_entities.get_inactive = True
                bc_am_load_objects.obc_entities.db_read()

                REM test bc_am_load_objects.obc_entities.search_attributes.db_read()

            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                bc_am_load_objects.obc_entities.get_inactive = True
                bc_am_load_objects.obc_entities.tmode = bc_om_entities.tREAD
                bc_am_load_objects.obc_entities.transmit_to_server_and_receive(bc_am_load_objects.obc_entities, True)
            End If
            'bc_am_load_objects.obc_entities.search_attributes.search_values = tsa
        Else
            bc_am_load_objects.obc_entities = bc_am_load_objects.obc_entities.read_data_from_file(bc_cs_central_settings.local_template_path + bc_am_load_objects.ENTITIES_FILENAME)
        End If

    End Sub


    Private Sub BlueCurve_TextSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class
