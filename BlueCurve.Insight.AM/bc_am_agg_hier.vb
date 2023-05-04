Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM


Public Class bc_am_agg_hier
    Implements Ibc_am_agg_hier
    Dim _entities As bc_om_entities
    Dim _classes As bc_om_entity_classes
    Dim _ohier As bc_om_agg_hier
    Public Sub load_data(ByVal ohier As bc_om_agg_hier, ByVal classes As bc_om_entity_classes, ByVal entities As bc_om_entities) Implements Ibc_am_agg_hier.load_data

        _entities = entities
        _classes = classes
        _ohier = ohier
        Me.lta.Items.Clear()
        'Me.lth.Items.Clear()
        'Me.dth.Items.Clear()
        Me.Text = "Hierarchy for Universe " + ohier.universe_name
        Me.at.Text = "Aggregate to Target Entities of Classes"
        lt.Text = ohier.target_class_name + "  to " + ohier.source_class_name
        cdual.Text = ohier.dual_class_name + "  to " + ohier.source_class_name

      



        Dim found As Boolean
        Dim exists As Boolean
        For i = 0 To ohier.target_prop.Count - 1
            found = False
            Me.ctarget.Items.Add(ohier.target_prop(i).class_name)
            For j = 0 To ohier.values.Count - 1
                exists = False
                For k = 0 To Me.lta.Items.Count - 1
                    If Me.lta.Items(k) = ohier.target_prop(i).class_name Then
                        exists = True
                        Exit For
                    End If
                Next
                If ohier.values(j).dual_class_id = 0 And ohier.values(j).target_class_id = ohier.target_prop(i).class_id Then
                    found = True
                    Exit For
                End If
            Next
            If exists = False Then
                If found = True Then
                    Me.lta.Items.Add(ohier.target_prop(i).class_name, True)
                Else
                    Me.lta.Items.Add(ohier.target_prop(i).class_name)
                End If
            End If
        Next
        For i = 0 To ohier.dual_prop.Count - 1
            found = False
            Me.ccdual.Items.Add(ohier.dual_prop(i).class_name)
            For j = 0 To ohier.values.Count - 1
                exists = False
                For k = 0 To Me.lta.Items.Count - 1
                    If Me.lta.Items(k) = ohier.dual_prop(i).class_name Then
                        exists = True
                        Exit For
                    End If
                Next

                If ohier.values(j).dual_class_id = 0 And ohier.values(j).target_class_id = ohier.dual_prop(i).class_id Then
                    found = True
                    Exit For
                End If
            Next
            If exists = False Then
                If found = True Then
                    Me.lta.Items.Add(ohier.dual_prop(i).class_name, True)
                Else
                    Me.lta.Items.Add(ohier.dual_prop(i).class_name)
                End If
            End If

        Next

        For i = 0 To classes.classes.Count - 1
            If classes.classes(i).class_type_id = 1 Then
                Me.cclass.Items.Add(classes.classes(i).class_name)
            End If
        Next

        If ohier.values.Count > 0 Then
            If ohier.values(0).ball = True Then
                Me.chkall.Checked = True
                Me.cclass.Text = ohier.values(0).all_class_name
                Me.Centity.Text = ohier.values(0).all_entity_name
            End If

            If ohier.values(0).target_class_id = -1 Then
                Exit Sub
            End If
        End If


        For i = 0 To ohier.values.Count - 1
            If ohier.values(i).tdname <> "" Then
                Me.lduals.Items.Add(ohier.values(i).tdname)
            End If
        Next

       

       
     

    End Sub

    Sub check_save()
        Me.bsave.Enabled = True
        If Me.chkall.Checked = True And (Me.cclass.SelectedIndex = -1 Or Me.Centity.SelectedIndex = -1) Then
            Me.bsave.Enabled = False
        End If
        'If (Me.lth.CheckedItems.Count > 0 And Me.dth.CheckedItems.Count = 0) Or (Me.lth.CheckedItems.Count = 0 And Me.dth.CheckedItems.Count > 0) Then
        '    Me.bsave.Enabled = False
        'End If
    End Sub

    Event save(ByVal ohier As bc_om_agg_hier) Implements Ibc_am_agg_hier.save

    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsave.Click
        Try
            Me.Cursor = Windows.Forms.Cursors.WaitCursor
            _ohier.values.Clear()
            Dim value As bc_om_agg_hier_value
            Dim tx As String



            For i = 0 To Me.ctarget.Items.Count - 1
                For j = 0 To Me.ccdual.Items.Count - 1
                    tx = Me.ctarget.Items(i) + " and " + Me.ccdual.Items(j)
                    For k = 0 To Me.lduals.Items.Count - 1
                        If Me.lduals.Items(k) = tx Then
                            value = New bc_om_agg_hier_value
                            For l = 0 To _classes.classes.Count - 1
                                If _classes.classes(l).class_name = Me.ctarget.Items(i) Then
                                    value.target_class_id = _classes.classes(l).class_Id
                                End If
                                If _classes.classes(l).class_name = Me.ccdual.Items(j) Then
                                    value.dual_class_id = _classes.classes(l).class_Id
                                End If
                            Next
                            _ohier.values.Add(value)
                            Exit For
                        End If
                    Next
                Next
            Next

            For i = 0 To Me.lta.CheckedItems.Count - 1
                For l = 0 To _classes.classes.Count - 1
                    If Me.lta.CheckedItems(i) = _classes.classes(l).class_name Then
                        value = New bc_om_agg_hier_value
                        value.target_class_id = _classes.classes(l).class_Id
                        value.dual_class_id = 0
                        _ohier.values.Add(value)

                        Exit For
                    End If
                Next
            Next
            _ohier.all_entity = 0

            If Me.chkall.Checked = True Then
                For i = 0 To _entities.entity.Count - 1
                    If _entities.entity(i).class_name = Me.cclass.Text And _entities.entity(i).name = Me.Centity.Text Then
                        _ohier.all_entity = _entities.entity(i).id
                        Exit For
                    End If
                Next
            End If

           

            RaiseEvent save(_ohier)
            Me.Cursor = Windows.Forms.Cursors.Default

            Me.Hide()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bsave", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub chkall_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkall.CheckedChanged

        Me.cclass.Enabled = False
        Me.Centity.Enabled = False
        Me.Centity.SelectedIndex = -1
        Me.cclass.SelectedIndex = -1
        If Me.chkall.Checked = True Then
            Me.cclass.Enabled = True
            Me.Centity.Enabled = True
            Me.cclass.Text = _ohier.all_class_name
            Me.Centity.Items.Clear()

            For i = 0 To _entities.entity.Count - 1
                If _entities.entity(i).class_name = _ohier.all_class_name Then
                    Me.Centity.Items.Add(_entities.entity(i).name)
                End If
            Next
            Me.Centity.Text = _ohier.all_entity_name
        End If

        check_save()
    End Sub

    Private Sub cclass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cclass.SelectedIndexChanged
        Me.Centity.Items.Clear()

        If Me.cclass.SelectedIndex > -1 Then
            For i = 0 To _entities.entity.Count - 1
                If _entities.entity(i).class_name = Me.cclass.Text Then
                    Me.Centity.Items.Add(_entities.entity(i).name)
                End If
            Next
        End If
        check_save()
    End Sub

    Private Sub Centity_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Centity.SelectedIndexChanged
        check_save()
    End Sub


    Private Sub lth_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        check_save()
    End Sub

    Private Sub dth_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        check_save()
    End Sub

    Private Sub lta_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lta.SelectedIndexChanged
        check_save()
    End Sub

    Private Sub csource_CheckedChanged(sender As Object, e As EventArgs)
        check_save()
    End Sub

   

    Private Sub ccdual_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ccdual.SelectedIndexChanged
        Me.badd.Enabled = False
        If Me.ctarget.SelectedIndex > -1 And Me.ccdual.SelectedIndex > -1 Then
            Me.badd.Enabled = True
        End If
    End Sub
    Private Sub ctarget_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ctarget.SelectedIndexChanged
        Me.badd.Enabled = False
        If Me.ctarget.SelectedIndex > -1 And Me.ccdual.SelectedIndex > -1 Then
            Me.badd.Enabled = True
        End If
    End Sub

    Private Sub badd_Click(sender As Object, e As EventArgs) Handles badd.Click
        Dim tx As String
        tx = Me.ctarget.Text + " and " + Me.ccdual.Text
        Dim found As Boolean
        found = False
        For i = 0 To Me.lduals.Items.Count - 1
            If tx = Me.lduals.Items(i) Then
                found = True
            End If
        Next
        If found = False Then
            Me.lduals.Items.Add(tx)
            check_save()
        End If
    End Sub

    Private Sub lduals_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lduals.DoubleClick
        If Me.lduals.SelectedIndex > -1 Then
            Me.lduals.Items.RemoveAt(Me.lduals.SelectedIndex)
            check_save()
        End If

    End Sub
End Class

Public Class Cbc_am_agg_hier
    Dim WithEvents _view As Ibc_am_agg_hier

    Public Sub New(ByVal view As Ibc_am_agg_hier)
        _view = view
    End Sub
    Public Sub save(ByVal ohier As bc_om_agg_hier) Handles _view.save
        Try
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ohier.db_write()
            Else
                ohier.tmode = bc_cs_soap_base_class.tWRITE
                If ohier.transmit_to_server_and_receive(ohier, True) = False Then
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_agg_hier", "save", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Public Function load_hier_for_universe(ByVal universe_id As String, ByVal universe_name As String, ByVal classes As bc_om_entity_classes, ByVal entities As bc_om_entities, ByVal target_class As String, ByVal dual_class As String, ByVal source_class As String)
        Try
            load_hier_for_universe = False
            Dim ohier As New bc_om_agg_hier
            ohier.universe_id = universe_id
            ohier.target_class_name = target_class
            ohier.dual_class_name = dual_class
            ohier.source_class_name = source_class
            ohier.universe_name = universe_name

            For i = 0 To classes.classes.Count - 1
                If classes.classes(i).class_name = target_class Then
                    ohier.target_class_id = classes.classes(i).class_id
                End If
                If classes.classes(i).class_name = dual_class Then
                    ohier.dual_class_id = classes.classes(i).class_id
                End If
                If classes.classes(i).class_name = source_class Then
                    ohier.source_class_id = classes.classes(i).class_id
                End If
            Next
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                ohier.db_read()
            Else
                ohier.tmode = bc_cs_soap_base_class.tREAD
                If ohier.transmit_to_server_and_receive(ohier, True) = False Then
                    Exit Function
                End If
            End If

            _view.load_data(ohier, classes, entities)

            load_hier_for_universe = True
        Catch ex As Exception
            load_hier_for_universe = False
            Dim oerr As New bc_cs_error_log("Cbc_am_agg_hier", "load_hier_for_universe", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
End Class

Public Interface Ibc_am_agg_hier
    Event save(ByVal ohier As bc_om_agg_hier)
    Sub load_data(ByVal ohier As bc_om_agg_hier, ByVal classes As bc_om_entity_classes, ByVal entities As bc_om_entities)
End Interface