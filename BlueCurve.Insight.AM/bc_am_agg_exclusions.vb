Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Windows.Forms


REM view
Public Class bc_am_agg_exclusions
    Implements Ibc_am_agg_exclusions_controller
    Dim _excl As bc_om_universe_excl
    Dim _classes As bc_om_entity_classes
    Dim _entities As bc_om_entities
    Dim etx, ctx, detx, dctx As String
    Dim agg_class_id As Long
    Dim type As Integer

    Public Event cancel() Implements Ibc_am_agg_exclusions_controller.cancel
    Public Event save(ByVal excl As bc_om_universe_excl) Implements Ibc_am_agg_exclusions_controller.save
    Public Event assign(ByVal excl As bc_om_universe_excls) Implements Ibc_am_agg_exclusions_controller.assign

    Private agg_universes_ids As New List(Of bc_om_entity)
    Public loading As Boolean
    Public Sub load_data(ByVal excl As bc_om_universe_excl, ByVal universe_name As String, ByVal classes As bc_om_entity_classes, ByVal entities As bc_om_entities) Implements Ibc_am_agg_exclusions_controller.load_data
        Try
            loading = True
            _excl = excl
            _classes = classes
            _entities = entities

            Dim found As Boolean
            Me.Cclass.Items.Clear()
            Me.lall.Items.Clear()
            'Me.cp.Items.Clear()
            'Me.cs.Items.Clear()
            'Me.Lvwalt.Items.Clear()
            'Me.Chksec.Items.Clear()

            agg_universes_ids.Clear()

            REM standing data
            For i = 0 To _classes.classes.Count - 1
                If _classes.classes(i).class_type_id = 1 Then
                    Me.Cclass.Items.Add(_classes.classes(i).class_name)
                ElseIf _classes.classes(i).class_name = "Aggregation Universe" Then
                    agg_class_id = _classes.classes(i).class_id
                End If

            Next

            load_copy_universes()


            'Me.CheckedListBox2.Items.Clear()
            Me.Text = "Exclusions for universe " + universe_name
            For i = 0 To excl.attribute_excls_types.Count - 1
                found = False
                For j = 0 To excl.attribute_excls.Count - 1
                    If excl.attribute_excls_types(i).id = excl.attribute_excls(j).id Then
                        found = True
                        Exit For
                    End If
                Next
                'If found = True Then
                '    Me.CheckedListBox2.Items.Add(excl.attribute_excls_types(i).name, True)
                'Else
                '    Me.CheckedListBox2.Items.Add(excl.attribute_excls_types(i).name, False)
                'End If
            Next

            Me.tschemaexclusions.Nodes.Clear()
            Dim pclass As String = ""
            For i = 0 To excl.schema_excls.Count - 1
                If pclass <> excl.schema_excls(i).class_name Then
                    Me.tschemaexclusions.Nodes.Add(excl.schema_excls(i).class_name, excl.schema_excls(i).class_name, 2)
                End If
                Me.tschemaexclusions.Nodes(Me.tschemaexclusions.Nodes.Count - 1).Nodes.Add(_excl.schema_excls(i).name)
                pclass = excl.schema_excls(i).class_name
            Next
            Me.tschemaexclusions.ExpandAll()
            load_inc()

            REM alternate contributor
            'Me.cs.Enabled = False
            'Me.cp.Items.Clear()
            'For i = 0 To excl.contributors.Count - 1
            '    Me.cp.Items.Add(excl.contributors(i).name)
            'Next
            REM contributors for aggregation
            Me.chkcont.Items.Clear()
            For i = 0 To excl.contributors.Count - 1
                found = False
                For j = 0 To excl.aggregate_contributors.Count - 1
                    If excl.contributors(i).id = excl.aggregate_contributors(j).id Then
                        found = True
                        Exit For
                    End If
                Next
                If found = True Then
                    Me.chkcont.Items.Add(excl.contributors(i).name, True)
                Else
                    Me.chkcont.Items.Add(excl.contributors(i).name, False)
                End If
            Next
            REM c



            Dim lvw As ListViewItem = Nothing
            'Me.Lvwalt.Items.Clear()
            For i = 0 To excl.altcontributors.Count - 1
                For j = 0 To excl.contributors.Count - 1
                    If excl.altcontributors(i).primary_contributor = excl.contributors(j).id Then
                        lvw = New ListViewItem(excl.contributors(j).name)
                        Exit For
                    End If
                Next
                For j = 0 To excl.contributors.Count - 1
                    If excl.altcontributors(i).alternate_contributor = excl.contributors(j).id Then
                        lvw.SubItems.Add(excl.contributors(j).name)
                        Exit For
                    End If
                Next
                'Me.Lvwalt.Items.Add(lvw)
            Next

            'Me.Chksec.Items.Clear()
            For i = 0 To excl.sec_attribute_excls_types.Count - 1
                found = False
                For j = 0 To excl.sec_attribute_excls.Count - 1
                    If excl.sec_attribute_excls(j).id = excl.sec_attribute_excls_types(i).id Then
                        'Me.Chksec.Items.Add(excl.sec_attribute_excls_types(i).name, True)
                        found = True
                        Exit For
                    End If
                Next
                'If found = False Then
                '    Me.Chksec.Items.Add(excl.sec_attribute_excls_types(i).name, False)
                'End If
            Next

            Me.chkiia.Checked = _excl.ignore_include_in_aggregation_flag




            check_attributes_set()
            loading = False
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub load_copy_universes()
        Me.cuniverses.Items.Clear()
        agg_universes_ids.Clear()
        For i = 0 To _entities.entity.Count - 1
            If agg_class_id = _entities.entity(i).class_id And _entities.entity(i).id <> _excl.universe_id Then
                If Me.binactive.Checked = True Or (Me.binactive.Checked = False And _entities.entity(i).inactive = False) Then
                    Me.cuniverses.Items.Add(_entities.entity(i).name)
                    agg_universes_ids.Add(_entities.entity(i))
                End If
            End If
        Next
    End Sub

    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click
        Dim omsg As New bc_cs_message("Blue Curve", "Changes will be lost are you sure you wish to clear?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected Then
            Exit Sub
        End If

        RaiseEvent cancel()
        edit_mode(False)
        load_inc()
    End Sub
    Sub edit_mode(ByVal edit As Boolean)
        If edit = True Then
            Me.bsave.Enabled = True
            Me.bcancel.Enabled = True
            Me.Passign.Enabled = False
            Me.Panel3.Enabled = False
            Me.TreeView1.Nodes.Clear()
            Me.TreeView2.Nodes.Clear()
            Me.ListBox1.Items.Clear()
            Me.luinc.Text = ""
        Else
            Me.bsave.Enabled = False
            Me.bcancel.Enabled = False
            Me.Passign.Enabled = True
            Me.Panel3.Enabled = True
        End If
    End Sub

   

    Private Sub CheckedListBox2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        edit_mode(True)
        check_attributes_set()

    End Sub
    Sub check_attributes_set()
        'If Me.CheckedListBox2.CheckedItems.Count = 0 Then
        '    Me.Lvwalt.Items.Clear()
        '    Me.Lvwalt.Enabled = False
        '    Me.Chksec.Enabled = False
        '    Me.cp.SelectedIndex = -1
        '    Me.cs.SelectedIndex = -1

        '    Me.Chksec.Items.Clear()
        '    For i = 0 To _excl.sec_attribute_excls_types.Count - 1
        '        Me.Chksec.Items.Add(_excl.sec_attribute_excls_types(i).name, False)
        '    Next
        '    Me.cp.Enabled = False
        '    Me.cs.Enabled = False
        'Else
        '    Me.cp.Enabled = True
        '    Me.cs.Enabled = True
        '    If Me.Lvwalt.Items.Count > 0 Then
        '        Me.Lvwalt.Enabled = True
        '        Me.Chksec.Enabled = True
        '    Else
        '        Me.Chksec.Items.Clear()
        '        For i = 0 To _excl.sec_attribute_excls_types.Count - 1
        '            Me.Chksec.Items.Add(_excl.sec_attribute_excls_types(i).name, False)
        '        Next
        '        Me.Chksec.Enabled = False
        '    End If
        'End If
    End Sub

    Private Sub bsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsave.Click
        Try
            Me.Cursor = Windows.Forms.Cursors.WaitCursor

            If Me.chkcont.CheckedItems.Count = 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "Universe must aggregate at least 1 contributor!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

            edit_mode(False)

            REM read data into object
            Dim excl As New bc_om_universe_excl
            Dim aexcl As bc_om_attribute_excl
            Dim ascha As bc_om_schema_excl

            excl.ignore_include_in_aggregation_flag = chkiia.Checked


            excl.universe_id = _excl.universe_id
            For i = 0 To _excl.attribute_excls_types.Count - 1
                'For j = 0 To Me.CheckedListBox2.CheckedItems.Count - 1
                '    If Me.CheckedListBox2.CheckedItems(j) = _excl.attribute_excls_types(i).name Then
                '        aexcl = New bc_om_attribute_excl
                '        aexcl.id = _excl.attribute_excls_types(i).id
                '        excl.attribute_excls.Add(aexcl)
                '    End If
                'Next
            Next

            Dim sentities As New List(Of bc_om_entity)
            Dim sentity As bc_om_entity

            For i = 0 To Me.tschemaexclusions.Nodes.Count - 1
                For j = 0 To Me.tschemaexclusions.Nodes(i).Nodes.Count - 1
                    sentity = New bc_om_entity
                    sentity.class_name = Me.tschemaexclusions.Nodes(i).Text
                    sentity.name = Me.tschemaexclusions.Nodes(i).Nodes(j).Text
                    sentities.Add(sentity)
                Next
            Next

            For i = 0 To Me._entities.entity.Count - 1
                For j = 0 To sentities.Count - 1
                    If Me._entities.entity(i).class_name = sentities(j).class_name And Me._entities.entity(i).name = sentities(j).name Then
                        ascha = New bc_om_schema_excl
                        ascha.entity_id = Me._entities.entity(i).id
                        excl.schema_excls.Add(ascha)
                    End If
                Next
            Next

            REM alternate contributor and exclusions
            Dim opa As bc_om_alt_contributor

            'For i = 0 To Me.Lvwalt.Items.Count - 1
            '    opa = New bc_om_alt_contributor
            '    For j = 0 To _excl.contributors.Count - 1
            '        If _excl.contributors(j).name = Me.Lvwalt.Items(i).Text Then
            '            opa.primary_contributor = _excl.contributors(j).id
            '        End If
            '        If _excl.contributors(j).name = Me.Lvwalt.Items(i).SubItems(1).Text Then
            '            opa.alternate_contributor = _excl.contributors(j).id
            '        End If
            '    Next
            '    excl.altcontributors.Add(opa)
            'Next

            'For i = 0 To Me.Chksec.CheckedItems.Count - 1
            '    For j = 0 To _excl.sec_attribute_excls_types.Count - 1
            '        If Me.Chksec.CheckedItems(i) = _excl.sec_attribute_excls_types(j).name Then
            '            aexcl = New bc_om_attribute_excl
            '            aexcl.id = _excl.sec_attribute_excls_types(j).id
            '            excl.sec_attribute_excls.Add(aexcl)
            '        End If
            '    Next
            'Next
            Dim ac As bc_om_contributor
            excl.aggregate_contributors.Clear()

            For i = 0 To Me.chkcont.CheckedItems.Count - 1
                For j = 0 To _excl.contributors.Count - 1
                    If Me.chkcont.CheckedItems(i) = _excl.contributors(j).name Then
                        ac = New bc_om_contributor(_excl.contributors(j).id, _excl.contributors(j).name)
                        excl.aggregate_contributors.Add(ac)
                        Exit For
                    End If
                Next
            Next

            RaiseEvent save(excl)
            edit_mode(False)
            load_inc()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions", "bsave_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Windows.Forms.Cursors.Default

        End Try

    End Sub
    Private Sub Cclass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cclass.SelectedIndexChanged
        Me.tsearch.Text = ""

        set_entities()
    End Sub
    Private Sub set_entities()
        Try

            Me.Cursor = Windows.Forms.Cursors.WaitCursor
            Me.lall.Items.Clear()
            Dim inc_entities As New List(Of String)
            If Cclass.SelectedIndex = 0 Then
                Exit Sub
            End If
            For i = 0 To Me.tschemaexclusions.Nodes.Count - 1
                If Me.tschemaexclusions.Nodes(i).Text = Me.Cclass.Text Then
                    For j = 0 To Me.tschemaexclusions.Nodes(i).Nodes.Count - 1
                        inc_entities.Add(Me.tschemaexclusions.Nodes(i).Nodes(j).Text)
                    Next
                End If
            Next
            Dim found As Boolean = False
            For i = 0 To _entities.entity.Count - 1
                found = False
                If _entities.entity(i).class_name = Me.Cclass.Text Then
                    For j = 0 To inc_entities.Count - 1
                        If inc_entities(j) = _entities.entity(i).name Then
                            found = True
                            Exit For
                        End If
                    Next
                    If found = False Then
                        If Trim(Me.tsearch.Text) <> "" Then
                            If InStr(UCase(_entities.entity(i).name), UCase(Me.tsearch.Text)) > 0 Then
                                Me.lall.Items.Add(_entities.entity(i).name)
                            End If
                        Else
                            Me.lall.Items.Add(_entities.entity(i).name)
                        End If
                    End If
                End If
            Next


            Me.Cursor = Windows.Forms.Cursors.Default
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions", "set_entities", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub tschemaexclusions_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tschemaexclusions.AfterSelect
        Me.Cclass.Text = Me.tschemaexclusions.SelectedNode.Text
    End Sub

    Private Sub lall_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lall.DoubleClick
        set_schema_exclusion()
    End Sub

   
    Private Sub badd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        set_schema_exclusion()
    End Sub
    Private Sub set_schema_exclusion()
        Try
            Dim found As Boolean = False
            For i = 0 To Me.tschemaexclusions.Nodes.Count - 1
                If Me.tschemaexclusions.Nodes(i).Text = Me.Cclass.Text Then
                    Me.tschemaexclusions.Nodes(i).Nodes.Add(CStr(Me.lall.SelectedItem))
                    found = True
                    Exit For
                End If
            Next
            If found = False Then
                Me.tschemaexclusions.Nodes.Add(Me.Cclass.Text, Me.Cclass.Text, 2)
                Me.tschemaexclusions.Nodes(Me.tschemaexclusions.Nodes.Count - 1).Nodes.Add(Me.lall.SelectedItem)
                Me.tschemaexclusions.Nodes(Me.tschemaexclusions.Nodes.Count - 1).Expand()
            End If
            edit_mode(True)

            set_entities()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions", "set_schema_exclusion", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try
    End Sub
    Private Sub remove_schema_exclusion(ByVal cclass As String, ByVal entity As String)
        Try

            Dim found As Boolean = False
            For i = 0 To Me.tschemaexclusions.Nodes.Count - 1
                If Me.tschemaexclusions.Nodes(i).Text = cclass Then
                    For j = 0 To Me.tschemaexclusions.Nodes(i).Nodes.Count - 1
                        If Me.tschemaexclusions.Nodes(i).Nodes(j).Text = entity Then
                            Me.tschemaexclusions.Nodes(i).Nodes(j).Remove()
                            If Me.tschemaexclusions.Nodes(i).Nodes.Count = 0 Then
                                Me.tschemaexclusions.Nodes(i).Remove()
                            End If
                            edit_mode(True)
                            set_entities()
                            Exit Sub
                        End If
                    Next
                End If
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions", "remove_schema_exclusion", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try
    End Sub
    


    Private Sub tschemaexclusions_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles tschemaexclusions.DoubleClick
        Dim cclass As String
        Try
            cclass = tschemaexclusions.SelectedNode.Parent.Text
            remove_schema_exclusion(cclass, tschemaexclusions.SelectedNode.Text)
        Catch ex As Exception
            Exit Sub
        End Try
    End Sub

    
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Me.Hide()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bclear.Click
       
        Dim omsg As New bc_cs_message("Blue Curve", "Changes will be lost are you sure you wish to clear?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected Then
            Exit Sub
        End If
        RaiseEvent cancel()
        edit_mode(False)
        Me.bclear.Enabled = False
        Me.bass.Enabled = False
    End Sub

    Private Sub cuniverses_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cuniverses.SelectedIndexChanged
        If Me.cuniverses.SelectedItems.Count <> 1 Then
            Exit Sub
        End If
        Me.bclear.Enabled = True
        Me.bass.Enabled = True


    End Sub

    Private Sub bass_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bass.Click
        Try

            Me.Cursor = Windows.Forms.Cursors.WaitCursor
            Dim omsg As New bc_cs_message("Blue Curve", "Existing Exclusions Will be overwritten for selected universes. Proceed?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected Then
                Exit Sub
            End If
            Dim excls As New bc_om_universe_excls
            Dim excl As bc_om_universe_excl
            For i = 0 To Me.cuniverses.CheckedItems.Count - 1

                For k = 0 To agg_universes_ids.Count - 1
                    If Me.cuniverses.CheckedItems(i) = agg_universes_ids(k).name Then
                        excl = New bc_om_universe_excl
                        excl.universe_id = agg_universes_ids(k).id
                        excl.schema_excls = _excl.schema_excls
                        excl.attribute_excls = _excl.attribute_excls
                        excl.altcontributors = _excl.altcontributors
                        excl.sec_attribute_excls = _excl.sec_attribute_excls
                        excl.ignore_include_in_aggregation_flag = _excl.ignore_include_in_aggregation_flag
                        excl.aggregate_contributors = _excl.aggregate_contributors
                        excls.universes.Add(excl)
                        Exit For
                    End If
                Next
            Next

            RaiseEvent assign(excls)
            Me.bclear.Enabled = False
            Me.bass.Enabled = False

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions", "bass_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            Me.Cursor = Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub load_inc()
        Try
            Me.Cursor = Windows.Forms.Cursors.WaitCursor



            Dim sql As String
            Dim bcs As New bc_cs_central_settings(True)
            Dim db As bc_om_sql



            Me.TreeView1.Nodes.Clear()


            sql = "exec dbo.bc_core_aggs_inclusion_view " + CStr(_excl.universe_id)

            db = New bc_om_sql(sql)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                db.db_read()
            Else
                db.tmode = bc_cs_soap_base_class.tREAD
                If db.transmit_to_server_and_receive(db, True) = False Then
                    Exit Sub
                End If
            End If
            Dim ptx As String = ""
            If Not IsNothing(db.results) Then
                For i = 0 To UBound(db.results, 2)
                    If i = 0 Or ptx <> db.results(2, i) Then
                        Me.TreeView1.Nodes.Add(db.results(2, i), db.results(2, i), 2)
                    End If
                    ptx = db.results(2, i)
                    Me.TreeView1.Nodes(Me.TreeView1.Nodes.Count - 1).Nodes.Add(db.results(3, i))
                Next
                If db.results(5, 0) = 1 Then
                    Me.buni.Enabled = True
                    type = 1
                End If
            End If
            sql = "exec dbo.bc_core_aggs_inclusion_view  " + CStr(_excl.universe_id) + ",1"
            db = New bc_om_sql(sql)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                db.db_read()
            Else
                db.tmode = bc_cs_soap_base_class.tREAD
                If db.transmit_to_server_and_receive(db, True) = False Then
                    Exit Sub
                End If

            End If
            If Not IsNothing(db.results) Then
                For i = 0 To UBound(db.results, 2)
                    If i = 0 Then
                        Me.TreeView2.Nodes.Add(db.results(2, i), db.results(2, i), 2)
                    End If
                    Me.TreeView2.Nodes(Me.TreeView2.Nodes.Count - 1).Nodes.Add(db.results(3, i))
                Next
            Else
                Me.TreeView2.Visible = False
                Me.TreeView1.Height = 137 * 2
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions", "inc_click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub TreeView1_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView1.AfterSelect

        Try

            Dim db As bc_om_sql
            Dim sql As String
            Me.ListBox1.Items.Clear()

            Me.luinc.Text = ""
            If type <> 1 Then
                Me.buni.Enabled = False
            End If

            If e.Node.ImageIndex = 2 Then

                Exit Sub
            End If

            Dim head As String
            Dim tx As String
            Try
                tx = e.Node.Parent.Text
                e.Node.Nodes.Clear()
            Catch
                Exit Sub
            End Try

            ctx = ""
            etx = ""

            Try
                tx = e.Node.Parent.Parent.Text
            Catch
                Try
                    tx = e.Node.Parent.Text
                    ctx = e.Node.Parent.Text
                    etx = e.Node.Text
                    If Me.TreeView2.Visible = False Then
                        Me.buni.Enabled = True
                    ElseIf dctx <> "" Then
                         Me.buni.Enabled = True
                    End If

                Catch

                End Try
            End Try



            Me.Cursor = Windows.Forms.Cursors.WaitCursor

            head = ""
            sql = "exec dbo.bc_core_aggs_inclusion_view  " + CStr(_excl.universe_id) + ",0,'" + e.Node.Parent.Text + "','" + e.Node.Text + "'"

            db = New bc_om_sql(sql)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                db.db_read()
            Else
                db.tmode = bc_cs_soap_base_class.tREAD
                If db.transmit_to_server_and_receive(db, True) = False Then
                    Exit Sub
                End If
            End If

            If Not IsNothing(db.results) Then

               
                If Not IsNothing(db.results) Then
                    For i = 0 To UBound(db.results, 2)
                        If i = 0 Or head <> db.results(2, i) Then
                            e.Node.Nodes.Add(db.results(2, i), db.results(2, i), 2)
                        End If
                        head = db.results(2, i)
                        e.Node.Nodes(e.Node.Nodes.Count - 1).Nodes.Add(db.results(3, i))
                    Next
                End If

            End If

            sql = "exec dbo.bc_core_aggs_inclusion_view  " + CStr(_excl.universe_id) + ",0,'" + e.Node.Parent.Text + "','" + e.Node.Text + "',1"

            db = New bc_om_sql(sql)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                db.db_read()
            Else
                db.tmode = bc_cs_soap_base_class.tREAD
                If db.transmit_to_server_and_receive(db, True) = False Then
                    Exit Sub
                End If
            End If

            If Not IsNothing(db.results) Then
                Me.luinc.Text = "(" + CStr(UBound(db.results, 2) + 1) + ") " + CStr(db.results(2, 0)) + "s's for " + e.Node.Parent.Text + " : " + e.Node.Text + " Included"
                For i = 0 To UBound(db.results, 2)
                    Me.ListBox1.Items.Add(db.results(3, i))

                Next
            End If
           



        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions", "after_select", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try
    End Sub

    
    Private Sub lall_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lall.SelectedIndexChanged

    End Sub

    Private Sub buni_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles buni.Click
        Try
            Me.Cursor = Windows.Forms.Cursors.WaitCursor



            Dim sql As String
            Dim bcs As New bc_cs_central_settings(True)
            Dim db As bc_om_sql



            Me.ListBox1.Items.Clear()



            sql = "exec dbo.bc_core_view_entities_for_aggregation " + CStr(_excl.universe_id) + ",'" + ctx + "','" + etx + "'" + ",'" + dctx + "','" + detx + "'"

            db = New bc_om_sql(sql)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                db.db_read()
            Else
                db.tmode = bc_cs_soap_base_class.tREAD
                If db.transmit_to_server_and_receive(db, True) = False Then
                    Exit Sub
                End If
            End If
            Dim ptx As String = ""
            If Not IsNothing(db.results) Then
                For i = 0 To UBound(db.results, 2)
                    Me.ListBox1.Items.Add(db.results(0, i))
                Next
               
            End If
            If Not IsNothing(db.results) Then
                If type = 1 Then
                    Me.luinc.Text = CStr(UBound(db.results, 2) + 1) + " Total Universe"
                ElseIf dctx = "" Then
                    Me.luinc.Text = CStr(UBound(db.results, 2) + 1) + " " + ctx + " - " + etx + " Universe"
                Else
                    Me.luinc.Text = CStr(UBound(db.results, 2) + 1) + " " + ctx + " - " + etx + " and " + dctx + " - " + detx + " Universe"
                End If
            Else
                If type = 1 Then
                    Me.luinc.Text = "0 Total Universe"
                ElseIf dctx = "" Then
                    Me.luinc.Text = "0 " + ctx + " - " + etx + " Universe"
                Else
                    Me.luinc.Text = "0 " + ctx + " - " + etx + " and " + dctx + " - " + detx + " Universe"
                End If
            End If



        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions", "buni_click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try
       
    End Sub

    Private Sub TreeView2_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles TreeView2.AfterSelect
        Try

            Dim db As bc_om_sql
            Dim sql As String
            Me.ListBox1.Items.Clear()

            Me.luinc.Text = ""
            If type <> 1 Then
                Me.buni.Enabled = False
            End If

            If e.Node.ImageIndex = 2 Then

                Exit Sub
            End If

            Dim head As String
            Dim tx As String
            Try
                tx = e.Node.Parent.Text
                e.Node.Nodes.Clear()
            Catch
                Exit Sub
            End Try

            dctx = ""
            detx = ""

            Try
                tx = e.Node.Parent.Parent.Text
            Catch
                Try
                    tx = e.Node.Parent.Text
                    dctx = e.Node.Parent.Text
                    detx = e.Node.Text
                    If ctx <> "" Then
                        Me.buni.Enabled = True
                    End If

                Catch

                End Try
            End Try



            Me.Cursor = Windows.Forms.Cursors.WaitCursor

            head = ""
            sql = "exec dbo.bc_core_aggs_inclusion_view  " + CStr(_excl.universe_id) + ",0,'" + e.Node.Parent.Text + "','" + e.Node.Text + "'"

            db = New bc_om_sql(sql)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                db.db_read()
            Else
                db.tmode = bc_cs_soap_base_class.tREAD
                If db.transmit_to_server_and_receive(db, True) = False Then
                    Exit Sub
                End If
            End If

            If Not IsNothing(db.results) Then


                If Not IsNothing(db.results) Then
                    For i = 0 To UBound(db.results, 2)
                        If i = 0 Or head <> db.results(2, i) Then
                            e.Node.Nodes.Add(db.results(2, i), db.results(2, i), 2)
                        End If
                        head = db.results(2, i)
                        e.Node.Nodes(e.Node.Nodes.Count - 1).Nodes.Add(db.results(3, i))
                    Next
                End If

            End If

            sql = "exec dbo.bc_core_aggs_inclusion_view  " + CStr(_excl.universe_id) + ",0,'" + e.Node.Parent.Text + "','" + e.Node.Text + "',1"

            db = New bc_om_sql(sql)
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                db.db_read()
            Else
                db.tmode = bc_cs_soap_base_class.tREAD
                If db.transmit_to_server_and_receive(db, True) = False Then
                    Exit Sub
                End If
            End If

            If Not IsNothing(db.results) Then
                Me.luinc.Text = "(" + CStr(UBound(db.results, 2) + 1) + ") " + CStr(db.results(2, 0)) + "s's for " + e.Node.Parent.Text + " : " + e.Node.Text + " Included"
                For i = 0 To UBound(db.results, 2)
                    Me.ListBox1.Items.Add(db.results(3, i))

                Next
            End If




        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions", "after_select2", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub binactive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binactive.CheckedChanged
        load_copy_universes()
    End Sub

    Private Sub tsearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsearch.TextChanged
        Timer1.Start()
    End Sub


    Private Sub Timer1_Tick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        Me.Cursor = Windows.Forms.Cursors.WaitCursor


        set_entities()
        Me.Cursor = Windows.Forms.Cursors.Default

    End Sub

    Private Sub cp_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            'Me.cs.Items.Clear()
            'If Me.cp.SelectedIndex = -1 Then
            '    Exit Sub
            'End If
            'Me.cs.Enabled = True

            'For i = 0 To _excl.contributors.Count - 1
            '    If _excl.contributors(i).name <> Me.cp.Text Then
            '        Me.cs.Items.Add(_excl.contributors(i).name)
            '    End If
            'Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("cp", "SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub

    Private Sub cs_SelectedIndexChanged(sender As Object, e As EventArgs)
        Try
            'If Me.cs.SelectedIndex = -1 Then
            '    Exit Sub
            'End If
            'For i = 0 To Me.Lvwalt.Items.Count - 1
            '    If Me.cp.Text = Me.Lvwalt.Items(i).Text And Me.cs.Text = Me.Lvwalt.Items(i).SubItems(1).Text Then
            '        Dim omsg As New bc_cs_message("Blue Curve", "Combination already set", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            '        Me.cs.SelectedIndex = -1
            '        Exit Sub
            '    End If
            'Next

            'Dim lvw As New ListViewItem(Me.cp.Text)
            'lvw.SubItems.Add(Me.cs.Text)
            'Me.Lvwalt.Items.Add(lvw)
            'Me.cs.SelectedIndex = -1

            edit_mode(True)
            check_attributes_set()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("cs", "SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub

    Private Sub Lvwalt_SelectedIndexChanged(sender As Object, e As EventArgs)
        'Me.Lvwalt.Items.RemoveAt(Me.Lvwalt.SelectedItems(0).Index)
        edit_mode(True)
        check_attributes_set()
    End Sub


    Private Sub Lvwalt_SelectedIndexChanged_1(sender As Object, e As EventArgs)

    End Sub

    Private Sub Chksec_SelectedIndexChanged(sender As Object, e As EventArgs)
        edit_mode(True)

    End Sub

    Private Sub chkiia_CheckedChanged(sender As Object, e As EventArgs) Handles chkiia.CheckedChanged
        If loading = True Then
            Exit Sub
        End If
        edit_mode(True)
    End Sub

    Private Sub bc_am_agg_exclusions_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub chkcont_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkcont.SelectedIndexChanged
        edit_mode(True)
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim fwaterfall As New bc_dx_waterfall
        Dim cwaterfall As New Cbc_dx_waterfall
        If cwaterfall.load_data(fwaterfall, _excl.universe_id, _excl) Then
            fwaterfall.ShowDialog()
        End If

    End Sub
End Class
REM controller
Public Class bc_am_agg_exclusions_controller
    Private WithEvents _view As Ibc_am_agg_exclusions_controller
    Private _universe_id As Long
    Private _universe_name As String
    Private _classes As New bc_om_entity_classes
    Private _entities As New bc_om_entities

    Private _excl As bc_om_universe_excl
    Public Sub New(ByVal view As Ibc_am_agg_exclusions_controller)
        _view = view
    End Sub
    Public Function load_exclusions_for_universe(ByVal universe_id As Long, ByVal universe_name As String, ByVal classes As bc_om_entity_classes, ByVal entities As bc_om_entities) As Boolean
        Try
            _universe_id = universe_id
            _classes = classes
            _entities = entities
            _excl = New bc_om_universe_excl
            _excl.universe_id = _universe_id
            _universe_name = universe_name

            load_from_database()

            _view.load_data(_excl, _universe_name, _classes, _entities)
            load_exclusions_for_universe = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions_controller", "load_exclusions_for_universe", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Private Sub load_from_database()
        Try
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _excl.db_read()
            Else
                _excl.tmode = bc_cs_soap_base_class.tREAD
                If _excl.transmit_to_server_and_receive(_excl, True) = False Then
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions_controller", "load_from_database", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub cancel() Handles _view.cancel
        Try
            load_from_database()
            _view.load_data(_excl, _universe_name, _classes, _entities)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions_controller", "cancel", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub save(ByVal excl As bc_om_universe_excl) Handles _view.save
        Try
            save_to_database(excl)
            load_from_database()
            _view.load_data(_excl, _universe_name, _classes, _entities)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions_controller", "save", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub assign(ByVal excls As bc_om_universe_excls) Handles _view.assign
        Try
            save_assignments_to_database(excls)
            load_from_database()
            _view.load_data(_excl, _universe_name, _classes, _entities)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions_controller", "assign", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub save_to_database(ByVal excl As bc_om_universe_excl)
        Try


            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                excl.db_write()
            Else
                excl.tmode = bc_cs_soap_base_class.tWRITE
                If excl.transmit_to_server_and_receive(excl, True) = False Then
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions_controller", "save_to_database", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Sub save_assignments_to_database(ByVal excls As bc_om_universe_excls)
        Try


            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                excls.db_write()
            Else
                excls.tmode = bc_cs_soap_base_class.tWRITE
                If excls.transmit_to_server_and_receive(excls, True) = False Then
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_agg_exclusions_controller", "save_to_database", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

End Class
REM view interface
Public Interface Ibc_am_agg_exclusions_controller
    Sub load_data(ByVal excl As bc_om_universe_excl, ByVal universe_name As String, ByVal classes As bc_om_entity_classes, ByVal entities As bc_om_entities)
    Event cancel()
    Event save(ByVal excl As bc_om_universe_excl)
    Event assign(ByVal excls As bc_om_universe_excls)
End Interface