Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM

Public Class bc_am_edit_attributes
    Implements Ibc_am_edit_attributes
    Dim _atts As bc_om_attributes_for_doc
    Event save(update_attributes As bc_om_set_attribute_values) Implements Ibc_am_edit_attributes.save
    Dim loading As Boolean
    Dim attribute_id As Long
    Dim attribute_name As String
    Dim autoset As Boolean = False
    Dim _save_msg As String = ""
    Dim pmeth As String
    Dim prisk As String
    Dim sel_column As Integer
    Dim rec_change As Boolean = False
    Private Sub uxparent_FocusedColumnChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedColumnChangedEventArgs) Handles uxparent.FocusedColumnChanged
        Try
            sel_column = e.Column.VisibleIndex
        Catch

        End Try

    End Sub


    


    Private Sub uxparent_NodeCellStyle(sender As Object, e As DevExpress.XtraTreeList.GetCustomNodeCellStyleEventArgs) Handles uxparent.NodeCellStyle
        For i = 0 To e.Node.Nodes.Count - 1
            If (e.Node.Nodes(i).GetValue(2) <> e.Node.Nodes(i).GetValue(6)) Or (e.Node.Nodes(i).GetValue(13) <> e.Node.Nodes(i).GetValue(15)) Or (e.Node.Nodes(i).GetValue(14) <> e.Node.Nodes(i).GetValue(16)) Then
                e.Appearance.BackColor = Drawing.Color.LightYellow
                e.Appearance.ForeColor = Drawing.Color.Black
               

                Exit For
            End If
        Next
        REM parent node
        If (e.Node.GetValue(2) <> e.Node.GetValue(6)) Or (e.Node.GetValue(13) <> e.Node.GetValue(15)) Or (e.Node.GetValue(14) <> e.Node.GetValue(16)) Then
            e.Appearance.BackColor = Drawing.Color.Orange
            e.Appearance.ForeColor = Drawing.Color.Black
            If (e.Node.GetValue(2) <> e.Node.GetValue(6)) AndAlso e.Column.VisibleIndex = 2 Then
                e.Appearance.BackColor = Drawing.Color.Red
            End If
            If (e.Node.GetValue(13) <> e.Node.GetValue(15)) AndAlso e.Column.VisibleIndex = 3 Then
                e.Appearance.BackColor = Drawing.Color.Red
            End If
            If (e.Node.GetValue(14) <> e.Node.GetValue(16)) AndAlso e.Column.VisibleIndex = 4 Then
                e.Appearance.BackColor = Drawing.Color.Red
            End If
        End If

    End Sub






    Public Function load_view(atts As bc_om_attributes_for_doc, title As String, save_msg As String) As Boolean Implements Ibc_am_edit_attributes.load_view
        Try

            loading = True
            _atts = atts
            _save_msg = save_msg
            uxparent.Nodes.Clear()
            uxparent.BeginUnboundLoad()
            Me.uxparent.Columns(2).OptionsColumn.AllowEdit = False
            Me.Text = "Entity Attributes for: " + title
            'Dim published_value As String
            REM this will come from a list box eventually
            If _atts.parent_entities.Count = 0 Then
                Exit Function
            End If
            attribute_id = _atts.parent_entities(0).attributes(0).attribute_id
            attribute_name = _atts.parent_entities(0).attributes(0).attribute_name
            Me.uxparent.Columns(2).Caption = "Draft: " + attribute_name
            Me.uxparent.Columns(6).Caption = "Published: " + attribute_name

            Dim nc As Integer = 0
            For i = 0 To _atts.parent_entities.Count - 1
                nc = 0
                With _atts.parent_entities(i).entity
                    uxparent.Nodes.Add()
                    'uxparent.Nodes(i).SetValue(0, .name + " [" + .class_name + "]")


                    uxparent.Nodes(i).SetValue(0, .name)
                    uxparent.Nodes(i).SetValue(5, .class_name)
                    uxparent.Nodes(i).Tag = .entity_id
                    uxparent.Nodes(i).StateImageIndex = 3
                    For j = 0 To _atts.parent_entities(i).attributes.Count - 1
                        If attribute_id = _atts.parent_entities(i).attributes(j).attribute_id Then
                            If _atts.parent_entities(i).attributes(j).is_lookup = True Then
                                uxparent.Nodes(i).SetValue(2, _atts.parent_entities(i).attributes(j).dval_str)
                                uxparent.Nodes(i).SetValue(6, _atts.parent_entities(i).attributes(j).pval_str)
                                uxparent.Nodes(i).SetValue(13, _atts.parent_entities(i).attributes(j).meth)
                                uxparent.Nodes(i).SetValue(14, _atts.parent_entities(i).attributes(j).risk)
                                uxparent.Nodes(i).SetValue(15, _atts.parent_entities(i).attributes(j).pmeth)
                                uxparent.Nodes(i).SetValue(16, _atts.parent_entities(i).attributes(j).prisk)
                                For m = 0 To _atts.lookup_lists.Count - 1
                                    If _atts.lookup_lists(m).attribute_id = _atts.parent_entities(i).attributes(j).attribute_id Then
                                        If _atts.lookup_lists(m).lookup_keys.Count > 0 Then
                                            For p = 0 To _atts.lookup_lists(m).lookup_keys.Count - 1
                                                If _atts.lookup_lists(m).lookup_keys(p) = _atts.parent_entities(i).attributes(j).value Then
                                                    uxparent.Nodes(i).SetValue(2, _atts.lookup_lists(m).lookup_values(p))
                                                End If
                                                'If _atts.parent_entities(i).attributes(j).workflowed = True Then
                                                If _atts.lookup_lists(m).lookup_keys(p) = _atts.parent_entities(i).attributes(j).published_value Then
                                                    uxparent.Nodes(i).SetValue(6, _atts.lookup_lists(m).lookup_values(p))
                                                End If
                                                'Else
                                                'uxparent.Nodes(i).SetValue(6, "na")
                                                'End If
                                            Next
                                            If uxparent.Nodes(i).GetValue(2) = "" Then

                                                Me.btnsubmit.Enabled = True
                                                uxparent.Nodes(i).StateImageIndex = 2
                                                Dim clevel As Boolean = False
                                                REM if any of its instruments are not blank and not parent level set to child level
                                                For r = 0 To _atts.parent_entities(i).child_entities.Count - 1
                                                    For s = 0 To _atts.parent_entities(i).child_entities(r).attributes.Count - 1
                                                        If _atts.parent_entities(i).child_entities(r).attributes(s).attribute_id = atts.parent_entities(i).attributes(j).attribute_id Then

                                                            For p = 0 To _atts.lookup_lists(m).lookup_keys.Count - 1
                                                                If _atts.lookup_lists(m).lookup_keys(p) = _atts.parent_entities(i).child_entities(r).attributes(s).value Then
                                                                    'If _atts.lookup_lists(m).lookup_values(p) <> _atts.parent_entities(i).child_entities(r).entity.class_name + " Level" Then
                                                                    '    Me.btnsubmit.Enabled = True
                                                                    '    _atts.parent_entities(i).attributes(j).changed = True
                                                                    '    uxparent.Nodes(i).SetValue(2, _atts.parent_entities(i).child_entities(r).entity.class_name + " Level")
                                                                    '    uxparent.Nodes(i).StateImageIndex = 4

                                                                    'End If
                                                                    For q = 0 To _atts.rec_classes.Count - 1
                                                                        If _atts.lookup_lists(m).lookup_values(p) <> _atts.rec_classes(p).rec_name Then
                                                                            Me.btnsubmit.Enabled = True
                                                                            _atts.parent_entities(i).attributes(j).changed = True
                                                                            For qq = 0 To _atts.rec_classes.Count - 1
                                                                                If _atts.rec_classes(qq).class_name = _atts.parent_entities(i).child_entities(r).entity.class_name Then
                                                                                    uxparent.Nodes(i).SetValue(2, _atts.rec_classes(qq).rec_name)
                                                                                    uxparent.Nodes(i).StateImageIndex = 4

                                                                                    Exit For
                                                                                End If
                                                                            Next
                                                                            Exit For
                                                                        End If
                                                                    Next

                                                                    Exit For
                                                                End If

                                                            Next

                                                            Exit For
                                                        End If

                                                    Next
                                                Next

                                                'If uxparent.Nodes(i).GetValue(1) <> uxparent.Nodes(i).Nodes(k + nc).GetValue(5) + " Level " Then
                                                '    Me.btnsubmit.Enabled = True
                                                '    _atts.parent_entities(i).child_entities(k)c(j).changed = True
                                                '    uxparent.Nodes(i).Nodes(k + nc).SetValue(2, uxparent.Nodes(i).GetValue(5) + " Level")
                                                '    uxparent.Nodes(i).Nodes(k + nc).StateImageIndex = 4
                                                'End If
                                            End If
                                        Else
                                            uxparent.Nodes(i).SetValue(2, _atts.parent_entities(i).attributes(j).value)
                                            'If _atts.parent_entities(i).attributes(j).workflowed = True Then
                                            uxparent.Nodes(i).SetValue(6, _atts.parent_entities(i).attributes(j).published_value)
                                            'End If
                                        End If
                                        Exit For
                                    End If

                                Next
                            Else
                                uxparent.Nodes(i).SetValue(2, _atts.parent_entities(i).attributes(j).value)
                            End If
                            If Year(_atts.parent_entities(i).attributes(j).date_changed) <> "1900" Then
                                uxparent.Nodes(i).SetValue(3, Format(_atts.parent_entities(i).attributes(j).date_changed.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                            End If
                            If _atts.parent_entities(i).attributes(j).user_changed <> "unknown" Then
                                uxparent.Nodes(i).SetValue(4, _atts.parent_entities(i).attributes(j).user_changed)
                            End If
                            REM workflowed attributes
                            'If _atts.parent_entities(i).attributes(j).workflowed = True Then
                            If Year(_atts.parent_entities(i).attributes(j).published_date_changed) <> "1900" Then
                                uxparent.Nodes(i).SetValue(7, Format(_atts.parent_entities(i).attributes(j).published_date_changed.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                            End If
                            If _atts.parent_entities(i).attributes(j).user_changed <> "unknown" Then
                                uxparent.Nodes(i).SetValue(8, _atts.parent_entities(i).attributes(j).published_user_changed)
                            End If
                            'End If
                            uxparent.Nodes(i).SetValue(12, _atts.parent_entities(i).extended_name4)

                            Exit For
                        End If
                    Next

                    For k = 0 To _atts.parent_entities(i).child_entities.Count - 1
                        uxparent.Nodes(i).Nodes.Add()
                        uxparent.Nodes(i).Expanded = False

                        'uxparent.Nodes(i).Nodes(k + nc).SetValue(0, _atts.parent_entities(i).child_entities(k).entity.name + " [" + _atts.parent_entities(i).child_entities(k).entity.class_name + "]")
                        uxparent.Nodes(i).Nodes(k + nc).SetValue(0, _atts.parent_entities(i).child_entities(k).entity.name)
                        uxparent.Nodes(i).Nodes(k + nc).SetValue(5, _atts.parent_entities(i).child_entities(k).entity.class_name)
                        uxparent.Nodes(i).Nodes(k + nc).StateImageIndex = 3
                        uxparent.Nodes(i).Nodes(k + nc).Tag = _atts.parent_entities(i).child_entities(k).entity.entity_id
                        If IsNumeric(_atts.parent_entities(i).child_entities(k).extended_name1) Then
                            Dim en As Double
                            en = atts.parent_entities(i).child_entities(k).extended_name1
                            uxparent.Nodes(i).Nodes(k + nc).SetValue(9, en)
                        End If
                        If IsDate(_atts.parent_entities(i).child_entities(k).extended_name2) Then
                            Dim eda As Date
                            eda = _atts.parent_entities(i).child_entities(k).extended_name2
                            uxparent.Nodes(i).Nodes(k + nc).SetValue(10, eda)
                            'Else
                            '    uxparent.Nodes(i).Nodes(k + nc).SetValue(10, _atts.parent_entities(i).child_entities(k).extended_name2)
                        End If
                        uxparent.Nodes(i).Nodes(k + nc).SetValue(11, _atts.parent_entities(i).child_entities(k).extended_name3)


                        For j = 0 To _atts.parent_entities(i).child_entities(k).attributes.Count - 1
                            If _atts.parent_entities(i).child_entities(k).attributes(j).attribute_id = attribute_id Then
                                REM check if lookup KVP
                                If _atts.parent_entities(i).child_entities(k).attributes(j).is_lookup = True Then
                                    uxparent.Nodes(i).Nodes(k + nc).SetValue(2, _atts.parent_entities(i).child_entities(k).attributes(j).dval_str)
                                    uxparent.Nodes(i).Nodes(k + nc).SetValue(6, _atts.parent_entities(i).child_entities(k).attributes(j).pval_str)
                                    uxparent.Nodes(i).Nodes(k + nc).SetValue(13, _atts.parent_entities(i).child_entities(k).attributes(j).meth)
                                    uxparent.Nodes(i).Nodes(k + nc).SetValue(14, _atts.parent_entities(i).child_entities(k).attributes(j).risk)
                                    uxparent.Nodes(i).Nodes(k + nc).SetValue(15, _atts.parent_entities(i).child_entities(k).attributes(j).pmeth)
                                    uxparent.Nodes(i).Nodes(k + nc).SetValue(16, _atts.parent_entities(i).child_entities(k).attributes(j).prisk)
                                    For m = 0 To _atts.lookup_lists.Count - 1
                                        If _atts.lookup_lists(m).attribute_id = _atts.parent_entities(i).child_entities(k).attributes(j).attribute_id Then
                                            If _atts.lookup_lists(m).lookup_keys.Count > 0 Then
                                                For p = 0 To _atts.lookup_lists(m).lookup_keys.Count - 1
                                                    If _atts.lookup_lists(m).lookup_keys(p) = _atts.parent_entities(i).child_entities(k).attributes(j).value Then
                                                        uxparent.Nodes(i).Nodes(k + nc).SetValue(2, _atts.lookup_lists(m).lookup_values(p))
                                                    End If
                                                    'If _atts.parent_entities(i).child_entities(k).attributes(j).workflowed = True Then
                                                    If _atts.lookup_lists(m).lookup_keys(p) = _atts.parent_entities(i).child_entities(k).attributes(j).published_value Then
                                                        uxparent.Nodes(i).Nodes(k + nc).SetValue(6, _atts.lookup_lists(m).lookup_values(p))
                                                    End If
                                                    'Else
                                                    'uxparent.Nodes(i).Nodes(k + nc).SetValue(6, "na")
                                                    'End If
                                                Next
                                                If uxparent.Nodes(i).Nodes(k + nc).GetValue(2) = "" Then
                                                    'If uxparent.Nodes(i).GetValue(2) <> uxparent.Nodes(i).Nodes(k + nc).GetValue(5) + " Level" Then
                                                    '    Me.btnsubmit.Enabled = True
                                                    '    _atts.parent_entities(i).child_entities(k).attributes(j).changed = True
                                                    '    uxparent.Nodes(i).Nodes(k + nc).SetValue(2, uxparent.Nodes(i).GetValue(5) + " Level")
                                                    '    uxparent.Nodes(i).Nodes(k + nc).StateImageIndex = 4
                                                    'Else
                                                    '    uxparent.Nodes(i).Nodes(k + nc).StateImageIndex = 2
                                                    'End If
                                                    uxparent.Nodes(i).Nodes(k + nc).StateImageIndex = 2
                                                    For q = 0 To _atts.rec_classes.Count - 1
                                                        If _atts.rec_classes(q).rec_name <> uxparent.Nodes(i).Nodes(k + nc).GetValue(5) Then

                                                            _atts.parent_entities(i).child_entities(k).attributes(j).changed = True
                                                            For qq = 0 To _atts.rec_classes.Count - 1
                                                                If _atts.rec_classes(q).class_name = uxparent.Nodes(i).GetValue(5) Then
                                                                    uxparent.Nodes(i).Nodes(k + nc).SetValue(2, _atts.rec_classes(q).rec_name)
                                                                    Exit For
                                                                End If
                                                            Next

                                                            uxparent.Nodes(i).Nodes(k + nc).StateImageIndex = 4
                                                            Exit For
                                                        End If
                                                    Next

                                                End If

                                            Else
                                                uxparent.Nodes(i).Nodes(k + nc).SetValue(2, _atts.parent_entities(i).child_entities(k).attributes(j).value)
                                            End If
                                            Exit For
                                        End If

                                    Next
                                Else
                                    uxparent.Nodes(i).Nodes(k + nc).SetValue(2, _atts.parent_entities(i).child_entities(k).attributes(j).value)
                                    If _atts.parent_entities(i).child_entities(k).attributes(j).workflowed = True Then
                                        uxparent.Nodes(i).Nodes(k + nc).SetValue(6, _atts.parent_entities(i).child_entities(k).attributes(j).published_value)
                                    End If
                                End If


                                If Year(_atts.parent_entities(i).child_entities(k).attributes(j).date_changed) <> "1900" Then
                                    uxparent.Nodes(i).Nodes(k + nc).SetValue(3, Format(_atts.parent_entities(i).child_entities(k).attributes(j).date_changed.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                                End If
                                If _atts.parent_entities(i).child_entities(k).attributes(j).user_changed <> "unknown" Then
                                    uxparent.Nodes(i).Nodes(k + nc).SetValue(4, _atts.parent_entities(i).child_entities(k).attributes(j).user_changed)
                                End If

                                If _atts.parent_entities(i).child_entities(k).attributes(j).workflowed = True Then
                                    If Year(_atts.parent_entities(i).child_entities(k).attributes(j).published_date_changed) <> "1900" Then
                                        uxparent.Nodes(i).Nodes(k + nc).SetValue(7, Format(_atts.parent_entities(i).child_entities(k).attributes(j).published_date_changed.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
                                    End If
                                    If _atts.parent_entities(i).child_entities(k).attributes(j).user_changed <> "unknown" Then
                                        uxparent.Nodes(i).Nodes(k + nc).SetValue(8, _atts.parent_entities(i).child_entities(k).attributes(j).published_user_changed)
                                    End If
                                End If


                                Exit For
                            End If
                        Next

                    Next

                End With
            Next

            'uxparent.ExpandAll()



            load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_edit_attributes", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            uxparent.EndUnboundLoad()
            loading = False
        End Try

    End Function
    Private Function is_kvp(val As String, attribute_id As Long)
        is_kvp = False
        For k = 0 To _atts.lookup_lists.Count - 1
            If _atts.lookup_lists(k).attribute_id = attribute_id AndAlso _atts.lookup_lists(k).lookup_keys.Count > 0 Then
                For l = 0 To _atts.lookup_lists(k).lookup_values.Count - 1
                    If _atts.lookup_lists(k).lookup_values(l) = val Then
                        is_kvp = True
                        Exit Function

                    End If
                Next
            End If
        Next

    End Function

    'Public Function load_view(atts As bc_om_attributes_for_doc, title As String) As Boolean Implements Ibc_am_edit_attributes.load_view
    '    Try
    '        loading = True
    '        _atts = atts
    '        uxparent.Nodes.Clear()
    '        uxparent.BeginUnboundLoad()
    '        Me.uxparent.Columns(2).OptionsColumn.AllowEdit = False
    '        Me.Text = "Entity Attributes for: " + title
    '        Dim published_value As String
    '        Dim nc As Integer = 0
    '        For i = 0 To _atts.parent_entities.Count - 1
    '            nc = 0
    '            With _atts.parent_entities(i).entity
    '                uxparent.Nodes.Add()
    '                uxparent.Nodes(i).SetValue(0, .name + " [" + .class_name + "]")
    '                uxparent.Nodes(i).SetValue(5, .class_name)
    '                uxparent.Nodes(i).Tag = .entity_id
    '                uxparent.Nodes(i).StateImageIndex = 3
    '                For j = 0 To _atts.parent_entities(i).attributes.Count - 1
    '                    uxparent.Nodes(i).Nodes.Add()
    '                    uxparent.Nodes(i).Nodes(j).SetValue(0, _atts.parent_entities(i).attributes(j).attribute_name)
    '                    REM check if lookup KVP
    '                    If _atts.parent_entities(i).attributes(j).is_lookup = True Then
    '                        For m = 0 To _atts.lookup_lists.Count - 1
    '                            If _atts.lookup_lists(m).attribute_id = _atts.parent_entities(i).attributes(j).attribute_id Then
    '                                If _atts.lookup_lists(m).lookup_keys.Count > 0 Then
    '                                    For p = 0 To _atts.lookup_lists(m).lookup_keys.Count - 1
    '                                        If _atts.lookup_lists(m).lookup_keys(p) = _atts.parent_entities(i).attributes(j).value Then
    '                                            uxparent.Nodes(i).Nodes(j).SetValue(2, _atts.lookup_lists(m).lookup_values(p))
    '                                        End If
    '                                        If _atts.parent_entities(i).attributes(j).workflowed = True Then
    '                                            If _atts.lookup_lists(m).lookup_keys(p) = _atts.parent_entities(i).attributes(j).published_value Then
    '                                                uxparent.Nodes(i).Nodes(j).SetValue(6, _atts.lookup_lists(m).lookup_values(p))
    '                                            End If
    '                                        Else
    '                                            uxparent.Nodes(i).Nodes(j).SetValue(6, "na")
    '                                        End If
    '                                    Next
    '                                Else
    '                                    uxparent.Nodes(i).Nodes(j).SetValue(2, _atts.parent_entities(i).attributes(j).value)
    '                                    If _atts.parent_entities(i).attributes(j).workflowed = True Then
    '                                        uxparent.Nodes(i).Nodes(j).SetValue(6, _atts.parent_entities(i).attributes(j).published_value)
    '                                    End If
    '                                End If
    '                                Exit For
    '                            End If

    '                        Next
    '                    Else
    '                        uxparent.Nodes(i).Nodes(j).SetValue(2, _atts.parent_entities(i).attributes(j).value)
    '                    End If
    '                    If Year(_atts.parent_entities(i).attributes(j).date_changed) <> "1900" Then
    '                        uxparent.Nodes(i).Nodes(j).SetValue(3, Format(_atts.parent_entities(i).attributes(j).date_changed.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
    '                    End If
    '                    If _atts.parent_entities(i).attributes(j).user_changed <> "unknown" Then
    '                        uxparent.Nodes(i).Nodes(j).SetValue(4, _atts.parent_entities(i).attributes(j).user_changed)
    '                    End If
    '                    REM workflowed attributes
    '                    If _atts.parent_entities(i).attributes(j).workflowed = True Then
    '                        If Year(_atts.parent_entities(i).attributes(j).published_date_changed) <> "1900" Then
    '                            uxparent.Nodes(i).Nodes(j).SetValue(7, Format(_atts.parent_entities(i).attributes(j).published_date_changed.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
    '                        End If
    '                        If _atts.parent_entities(i).attributes(j).user_changed <> "unknown" Then
    '                            uxparent.Nodes(i).Nodes(j).SetValue(8, _atts.parent_entities(i).attributes(j).published_user_changed)
    '                        End If
    '                    End If

    '                    uxparent.Nodes(i).Nodes(j).StateImageIndex = 1
    '                    uxparent.Nodes(i).Nodes(j).Tag = _atts.parent_entities(i).attributes(j).attribute_id

    '                    nc = nc + 1
    '                Next
    '                For k = 0 To _atts.parent_entities(i).child_entities.Count - 1
    '                    uxparent.Nodes(i).Nodes.Add()
    '                    uxparent.Nodes(i).Nodes(k + nc).SetValue(0, _atts.parent_entities(i).child_entities(k).entity.name + " [" + _atts.parent_entities(i).child_entities(k).entity.class_name + "]")
    '                    uxparent.Nodes(i).Nodes(k + nc).SetValue(5, _atts.parent_entities(i).child_entities(k).entity.class_name)
    '                    uxparent.Nodes(i).Nodes(k + nc).StateImageIndex = 3
    '                    uxparent.Nodes(i).Nodes(k + nc).Tag = _atts.parent_entities(i).child_entities(k).entity.entity_id
    '                    For j = 0 To _atts.parent_entities(i).child_entities(k).attributes.Count - 1
    '                        uxparent.Nodes(i).Nodes(k + nc).Nodes.Add()
    '                        uxparent.Nodes(i).Nodes(k + nc).Nodes(j).SetValue(0, _atts.parent_entities(i).child_entities(k).attributes(j).attribute_name)

    '                        REM check if lookup KVP
    '                        If _atts.parent_entities(i).child_entities(k).attributes(j).is_lookup = True Then
    '                            For m = 0 To _atts.lookup_lists.Count - 1
    '                                If _atts.lookup_lists(m).attribute_id = _atts.parent_entities(i).child_entities(k).attributes(j).attribute_id Then
    '                                    If _atts.lookup_lists(m).lookup_keys.Count > 0 Then
    '                                        For p = 0 To _atts.lookup_lists(m).lookup_keys.Count - 1
    '                                            If _atts.lookup_lists(m).lookup_keys(p) = _atts.parent_entities(i).child_entities(k).attributes(j).value Then
    '                                                uxparent.Nodes(i).Nodes(k + nc).Nodes(j).SetValue(2, _atts.lookup_lists(m).lookup_values(p))
    '                                            End If
    '                                            If _atts.parent_entities(i).child_entities(k).attributes(j).workflowed = True Then
    '                                                If _atts.lookup_lists(m).lookup_keys(p) = _atts.parent_entities(i).child_entities(k).attributes(j).published_value Then
    '                                                    uxparent.Nodes(i).Nodes(k + nc).Nodes(j).SetValue(6, _atts.lookup_lists(m).lookup_values(p))
    '                                                End If
    '                                            Else
    '                                                uxparent.Nodes(i).Nodes(k + nc).Nodes(j).SetValue(6, "na")
    '                                            End If


    '                                        Next
    '                                    Else
    '                                        uxparent.Nodes(i).Nodes(k + nc).Nodes(j).SetValue(2, _atts.parent_entities(i).child_entities(k).attributes(j).value)
    '                                    End If
    '                                    Exit For
    '                                End If

    '                            Next
    '                        Else
    '                            uxparent.Nodes(i).Nodes(k + nc).Nodes(j).SetValue(2, _atts.parent_entities(i).child_entities(k).attributes(j).value)
    '                            If _atts.parent_entities(i).child_entities(k).attributes(j).workflowed = True Then
    '                                uxparent.Nodes(i).Nodes(k + nc).Nodes(j).SetValue(6, _atts.parent_entities(i).child_entities(k).attributes(j).published_value)
    '                            End If
    '                        End If


    '                        If Year(_atts.parent_entities(i).child_entities(k).attributes(j).date_changed) <> "1900" Then
    '                            uxparent.Nodes(i).Nodes(k + nc).Nodes(j).SetValue(3, Format(_atts.parent_entities(i).child_entities(k).attributes(j).date_changed.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
    '                        End If
    '                        If _atts.parent_entities(i).child_entities(k).attributes(j).user_changed <> "unknown" Then
    '                            uxparent.Nodes(i).Nodes(k + nc).Nodes(j).SetValue(4, _atts.parent_entities(i).child_entities(k).attributes(j).user_changed)
    '                        End If

    '                        If _atts.parent_entities(i).child_entities(k).attributes(j).workflowed = True Then
    '                            If Year(_atts.parent_entities(i).child_entities(k).attributes(j).published_date_changed) <> "1900" Then
    '                                uxparent.Nodes(i).Nodes(k + nc).Nodes(j).SetValue(7, Format(_atts.parent_entities(i).child_entities(k).attributes(j).published_date_changed.ToLocalTime, "dd MMM yyyy HH:mm:ss"))
    '                            End If
    '                            If _atts.parent_entities(i).child_entities(k).attributes(j).user_changed <> "unknown" Then
    '                                uxparent.Nodes(i).Nodes(k + nc).Nodes(j).SetValue(8, _atts.parent_entities(i).child_entities(k).attributes(j).published_user_changed)
    '                            End If
    '                        End If

    '                        uxparent.Nodes(i).Nodes(k + nc).Nodes(j).StateImageIndex = 1
    '                        uxparent.Nodes(i).Nodes(k + nc).Nodes(j).Tag = _atts.parent_entities(i).child_entities(k).attributes(j).attribute_id
    '                    Next

    '                Next

    '            End With
    '        Next
    '        uxparent.EndUnboundLoad()
    '        uxparent.ExpandAll()



    '        load_view = True
    '    Catch ex As Exception
    '        Dim oerr As New bc_cs_error_log("bc_am_edit_attributes", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        loading = False
    '    End Try

    'End Function

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub


    Private Sub uxparent_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxparent.FocusedNodeChanged
        Try



            Me.uxparent.Columns(2).OptionsColumn.AllowEdit = True
            'If e.Node.StateImageIndex = 3 Then
            '    Exit Sub
            'End If
            Dim exclude_name As String = ""

            Try
                exclude_name = e.Node.GetValue(5)

            Catch
            End Try


            Me.RepositoryItemComboBox1.Items.Clear()

            For i = 0 To _atts.parent_entities.Count - 1

                For m = 0 To _atts.parent_entities(i).attributes.Count - 1
                    With _atts.parent_entities(i).attributes(m)
                        If .attribute_id = attribute_id Then
                            set_input(.data_type, .is_lookup, .attribute_id, exclude_name, _atts.parent_entities(i).entity.class_name)
                            Exit Sub
                        End If
                    End With
                Next

                For k = 0 To _atts.parent_entities(i).child_entities.Count - 1
                    For m = 0 To _atts.parent_entities(i).child_entities(k).attributes.Count - 1
                        With _atts.parent_entities(i).child_entities(k).attributes(m)
                            If .attribute_id = attribute_id Then
                                set_input(.data_type, .is_lookup, .attribute_id, _atts.parent_entities(i).child_entities(k).entity.class_name, _atts.parent_entities(i).entity.class_name)
                                Exit Sub
                            End If

                        End With
                    Next
                Next
            Next



        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_edit_attributes", "uxparent_FocusedNodeChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub

    'Private Sub uxparent_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxparent.FocusedNodeChanged
    '    Try

    '        Me.uxparent.Columns(2).OptionsColumn.AllowEdit = True
    '        If e.Node.StateImageIndex = 3 Then
    '            Exit Sub
    '        End If
    '        Dim exclude_name As String = ""

    '        Try
    '            exclude_name = e.Node.ParentNode.GetValue(5)

    '        Catch
    '        End Try


    '        Me.RepositoryItemComboBox1.Items.Clear()

    '        For i = 0 To _atts.parent_entities.Count - 1

    '            For m = 0 To _atts.parent_entities(i).attributes.Count - 1
    '                With _atts.parent_entities(i).attributes(m)
    '                    If .attribute_id = e.Node.Tag Then
    '                        set_input(.data_type, .is_lookup, .attribute_id, exclude_name, _atts.parent_entities(i).entity.class_name)
    '                        Exit Sub
    '                    End If
    '                End With
    '            Next

    '            For k = 0 To _atts.parent_entities(i).child_entities.Count - 1
    '                For m = 0 To _atts.parent_entities(i).child_entities(k).attributes.Count - 1
    '                    With _atts.parent_entities(i).child_entities(k).attributes(m)
    '                        If .attribute_id = e.Node.Tag Then
    '                            set_input(.data_type, .is_lookup, .attribute_id, _atts.parent_entities(i).child_entities(k).entity.class_name, _atts.parent_entities(i).entity.class_name)
    '                            Exit Sub
    '                        End If

    '                    End With
    '                Next
    '            Next
    '        Next


    '        'MsgBox(linked_attribute)
    '        'REM if linked attribute and set to parent disable column
    '        'If linked_attribute > 0 Then
    '        '    Try
    '        '        If Me.uxparent.Selection(0).GetValue(2) = Me.uxparent.Selection(0).ParentNode.ParentNode.GetValue(5) + " Level" Then
    '        '            Me.uxparent.Columns(2).OptionsColumn.AllowEdit = False

    '        '        End If
    '        '    Catch

    '        '    End Try
    '        'End If

    '    Catch ex As Exception
    '        Dim oerr As New bc_cs_error_log("bc_am_edit_attributes", "uxparent_FocusedNodeChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

    '    End Try
    'End Sub
    Dim linked_attribute As Long = 0



    Private Sub set_linked_attribute(value As String)
        Try

            Dim entity_id As Long
            Dim class_name As String
            Dim parent As Boolean = False
            entity_id = Me.uxparent.Selection(0).Tag
            class_name = Me.uxparent.Selection(0).GetValue(5)

            Dim reset_child As Boolean = False
            Dim reset_parent As Boolean = False
            REM parent and selection is child level clear child level otherwise set child level to parent level
            For i = 0 To _atts.parent_entities.Count - 1
                If _atts.parent_entities(i).entity.entity_id = entity_id Then
                    parent = True
                    For j = 0 To _atts.parent_entities(i).child_entities.Count - 1

                        'If value = _atts.parent_entities(i).child_entities(j).entity.class_name + " Level" Then
                        '    REM child level selected so reset these values
                        '    reset_child = True
                        'End If
                        For k = 0 To _atts.rec_classes.Count - 1
                            If _atts.rec_classes(k).class_name = _atts.parent_entities(i).child_entities(j).entity.class_name Then
                                If value = _atts.rec_classes(k).rec_name Then
                                    reset_child = True
                                    Exit For
                                End If

                            End If
                        Next
                    Next
                    Exit For
                End If
                If parent = True Then
                    Exit For
                End If
                For k = 0 To _atts.parent_entities(i).child_entities.Count - 1
                    If _atts.parent_entities(i).child_entities(k).entity.entity_id = entity_id Then
                        For j = 0 To _atts.parent_entities(i).child_entities.Count - 1
                            'If value = _atts.parent_entities(i).entity.class_name + " Level" Then
                            '    REM child level selected so reset these values
                            '    reset_parent = True
                            '    Exit For
                            'End If
                            For l = 0 To _atts.rec_classes.Count - 1
                                If _atts.rec_classes(l).class_name = _atts.parent_entities(i).entity.class_name Then
                                    If value = _atts.rec_classes(l).rec_name Then
                                        reset_parent = True
                                        Exit For
                                    End If

                                End If
                            Next
                        Next

                        Exit For
                    End If
                Next
            Next

            Dim omsg As String = ""
            Dim pmsg As String = ""
            If parent = True Then
                If reset_child = True Then
                    REM reset linked attribute for all child entities
                    REM if publis rec exists set this else flag as need setting
                    For i = 0 To uxparent.Nodes.Count - 1
                        If uxparent.Nodes(i).Tag = entity_id Then
                            For j = 0 To uxparent.Nodes(i).Nodes.Count - 1
                                If attribute_id = linked_attribute Then

                                    If (uxparent.Nodes(i).Nodes(j).GetValue(6) = "No Recommendation") Then
                                        pmsg = pmsg + vbCrLf + uxparent.Nodes(i).Nodes(j).GetValue(0)

                                    ElseIf (uxparent.Nodes(i).Nodes(j).GetValue(6) = "" Or is_kvp(uxparent.Nodes(i).Nodes(j).GetValue(6), attribute_id) = False) Or uxparent.Nodes(i).Nodes(j).GetValue(6) = "Recommendation Moved to Company Level" Then
                                        omsg = omsg + vbCrLf + uxparent.Nodes(i).Nodes(j).GetValue(0)

                                    Else
                                        pmsg = pmsg + vbCrLf + uxparent.Nodes(i).Nodes(j).GetValue(0)
                                    End If
                                End If
                            Next
                            Dim dmsg As bc_cs_message
                            If omsg <> "" Then
                                dmsg = New bc_cs_message("Blue Curve", "Please set the " + attribute_name + "(s) for " + omsg, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            End If
                            If pmsg <> "" Then
                                dmsg = New bc_cs_message("Blue Curve", "Published " + attribute_name + "(s) set for " + pmsg, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            End If
                            For j = 0 To uxparent.Nodes(i).Nodes.Count - 1
                                If attribute_id = linked_attribute Then
                                    If (uxparent.Nodes(i).Nodes(j).GetValue(6) = "No Recommendation") Then
                                        uxparent.Nodes(i).Nodes(j).SetValue(2, uxparent.Nodes(i).Nodes(j).GetValue(6))
                                        uxparent.Nodes(i).Nodes(j).StateImageIndex = 4
                                        set_changed(uxparent.Nodes(i).Nodes(j).Tag, linked_attribute)
                                    ElseIf (uxparent.Nodes(i).Nodes(j).GetValue(6) <> "" And is_kvp(uxparent.Nodes(i).Nodes(j).GetValue(6), attribute_id)) And uxparent.Nodes(i).Nodes(j).GetValue(6) <> "Recommendation Moved to Company Level" Then
                                        uxparent.Nodes(i).Nodes(j).SetValue(2, uxparent.Nodes(i).Nodes(j).GetValue(6))
                                        uxparent.Nodes(i).Nodes(j).StateImageIndex = 4
                                        set_changed(uxparent.Nodes(i).Nodes(j).Tag, linked_attribute)
                                    Else
                                        uxparent.Nodes(i).Nodes(j).SetValue(2, Nothing)
                                        omsg = omsg + vbCrLf + uxparent.Nodes(i).Nodes(j).GetValue(0)
                                        uxparent.Nodes(i).Nodes(j).StateImageIndex = 2
                                    End If

                                        uxparent.Nodes(i).ExpandAll()
                                    End If
                            Next




                            Exit For
                        End If
                    Next
                Else
                    REM set all children to parent level
                    For i = 0 To uxparent.Nodes.Count - 1
                        If uxparent.Nodes(i).Tag = entity_id Then
                            For j = 0 To uxparent.Nodes(i).Nodes.Count - 1
                                If attribute_id = linked_attribute Then
                                    'If uxparent.Nodes(i).Nodes(j).GetValue(2) <> class_name + " Level" Then
                                    '    omsg = omsg + vbCrLf + uxparent.Nodes(i).Nodes(j).GetValue(0)
                                    'End If
                                    For k = 0 To _atts.rec_classes.Count - 1
                                        If uxparent.Nodes(i).Nodes(j).GetValue(2) <> _atts.rec_classes(k).rec_name AndAlso uxparent.Nodes(i).GetValue(5) = _atts.rec_classes(k).class_name Then
                                            omsg = omsg + vbCrLf + uxparent.Nodes(i).Nodes(j).GetValue(0)

                                            Exit For
                                        End If
                                    Next


                                End If
                            Next
                            If omsg <> "" Then
                                For k = 0 To _atts.rec_classes.Count - 1
                                    If _atts.rec_classes(k).class_name = class_name Then
                                        Dim dmsg As New bc_cs_message("Blue Curve", attribute_name + "(s) for " + omsg + vbCrLf + "have been set to " + _atts.rec_classes(k).rec_name, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                        Exit For
                                    End If
                                Next

                                'Dim dmsg As New bc_cs_message("Blue Curve", attribute_name + "(s) for " + omsg + vbCrLf + "have been set to " + class_name + " Level", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            End If

                            For j = 0 To uxparent.Nodes(i).Nodes.Count - 1
                                If attribute_id = linked_attribute Then
                                    For k = 0 To _atts.rec_classes.Count - 1
                                        If uxparent.Nodes(i).Nodes(j).GetValue(2) <> _atts.rec_classes(k).rec_name AndAlso _atts.rec_classes(k).class_name = class_name Then
                                            uxparent.Nodes(i).Nodes(j).SetValue(2, _atts.rec_classes(k).rec_name)
                                            uxparent.Nodes(i).Nodes(j).StateImageIndex = 4
                                            set_changed(uxparent.Nodes(i).Nodes(j).Tag, linked_attribute)
                                            uxparent.Nodes(i).Expanded = False
                                            Exit For
                                        End If
                                    Next
                                    'If uxparent.Nodes(i).Nodes(j).GetValue(2) <> class_name + " Level" Then
                                    '    uxparent.Nodes(i).Nodes(j).SetValue(2, class_name + " Level")
                                    '    uxparent.Nodes(i).Nodes(j).StateImageIndex = 4
                                    '    set_changed(uxparent.Nodes(i).Nodes(j).Tag, linked_attribute)
                                    '    uxparent.Nodes(i).Expanded = False
                                    'End If
                                End If
                            Next

                            Exit For
                        End If
                    Next
                End If


            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_edit_attributes", "set_linked_attribute", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Private Sub set_changed(entity_id As Long, attribute_id As Long)
        Try
            For i = 0 To _atts.parent_entities.Count - 1
                If _atts.parent_entities(i).entity.entity_id = entity_id Then
                    For j = 0 To _atts.parent_entities(i).attributes.Count - 1
                        If _atts.parent_entities(i).attributes(j).attribute_id = attribute_id Then
                            _atts.parent_entities(i).attributes(j).changed = True
                            Exit Sub
                        End If
                    Next
                End If
                For p = 0 To _atts.parent_entities(i).child_entities.Count - 1
                    If _atts.parent_entities(i).child_entities(p).entity.entity_id = entity_id Then
                        For j = 0 To _atts.parent_entities(i).child_entities(p).attributes.Count - 1
                            If _atts.parent_entities(i).child_entities(p).attributes(j).attribute_id = attribute_id Then
                                _atts.parent_entities(i).child_entities(p).attributes(j).changed = True
                                Exit Sub
                            End If
                        Next
                    End If

                Next
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_edit_attributes", "set_changed", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub
    Private Sub set_input(data_type As bc_om_attributes_for_doc.ATT_TYPE, is_lookup As Boolean, attribute_id As Integer, exclude_value As String, exclude_value2 As String)
        Try

            linked_attribute = 0
            Me.RepositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
            If data_type = bc_om_attributes_for_doc.ATT_TYPE.BOOL Then
                Me.RepositoryItemComboBox1.Items.Add("True")
                Me.RepositoryItemComboBox1.Items.Add("False")
                Me.RepositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor


            ElseIf is_lookup = True Then
                Dim exclude_values As New List(Of String)


                REM wf may 2017
                'For i = 0 To _atts.rec_classes.Count - 1
                '    If _atts.rec_classes(i).class_name = exclude_value Then
                '        exclude_value = _atts.rec_classes(i).rec_name
                '    End If
                '    If _atts.rec_classes(i).class_name = exclude_value2 Then
                '        exclude_value2 = _atts.rec_classes(i).rec_name
                '    End If
                'Next

                For i = 0 To _atts.rec_classes.Count - 1
                    If _atts.rec_classes(i).class_name = exclude_value Or _atts.rec_classes(i).class_name = exclude_value2 Then
                        If _atts.rec_classes(i).display = False Then

                            exclude_values.Add(_atts.rec_classes(i).rec_name)
                        End If
                    End If
                Next
                Dim found As Boolean = False
                'exclude_value = exclude_value + " Level"
                'exclude_value2 = exclude_value2 + " Level"
                Me.RepositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
                For i = 0 To _atts.lookup_lists.Count - 1
                    If _atts.lookup_lists(i).attribute_id = attribute_id Then

                        Me.uxparent.Columns(2).OptionsColumn.AllowEdit = True
                        For j = 0 To _atts.lookup_lists(i).lookup_values.Count - 1
                            found = False
                            For k = 0 To exclude_values.Count - 1
                                If exclude_values(k) = _atts.lookup_lists(i).lookup_values(j) Then
                                    found = True
                                    Exit For
                                End If

                            Next

                            'If exclude_value <> _atts.lookup_lists(i).lookup_values(j) And exclude_value2 <> _atts.lookup_lists(i).lookup_values(j) Then
                            If found = False Then
                                Me.RepositoryItemComboBox1.Items.Add(_atts.lookup_lists(i).lookup_values(j))
                            Else
                                linked_attribute = attribute_id
                            End If
                            If linked_attribute > 0 Then
                                If exclude_value2 = Me.uxparent.Selection(0).GetValue(2) Then
                                    Me.uxparent.Columns(2).OptionsColumn.AllowEdit = False
                                End If
                            End If
                        Next
                        Exit For
                    End If
                Next
            End If



        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_edit_attributes", "set_input", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub



    Private Sub value_changed(sender As Object, e As System.EventArgs) Handles RepositoryItemComboBox1.EditValueChanged

        Try
            Me.btnsubmit.Enabled = True
            Me.uxparent.Selection(0).StateImageIndex = 4
            rec_change = True
            Dim s As DevExpress.XtraEditors.ComboBoxEdit
            s = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
            Dim ev As String
            ev = s.EditValue

            For i = 0 To _atts.parent_entities.Count - 1
                If _atts.parent_entities(i).entity.entity_id = Me.uxparent.Selection(0).Tag Then
                    For j = 0 To _atts.parent_entities(i).attributes.Count - 1
                        If _atts.parent_entities(i).attributes(j).attribute_id = attribute_id Then
                            _atts.parent_entities(i).attributes(j).changed = True
                            If linked_attribute <> 0 Then
                                set_linked_attribute(ev)
                            End If
                            Exit Sub
                        End If
                    Next
                    Exit Sub
                End If



                For k = 0 To _atts.parent_entities(i).child_entities.Count - 1
                    If _atts.parent_entities(i).child_entities(k).entity.entity_id = Me.uxparent.Selection(0).Tag Then
                        For j = 0 To _atts.parent_entities(i).child_entities(k).attributes.Count - 1
                            If _atts.parent_entities(i).child_entities(k).attributes(j).attribute_id = attribute_id Then
                                _atts.parent_entities(i).child_entities(k).attributes(j).changed = True
                                If linked_attribute <> 0 Then
                                    set_linked_attribute(ev)
                                End If
                                Exit Sub
                            End If
                        Next
                        Exit Sub
                    End If
                Next
            Next


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_edit_attributes", "value_changed", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub
    Private Function get_value_from_node_tree(entity_id As String, attribute_id As String, parent As Boolean, type As Integer) As String
        Try
            get_value_from_node_tree = ""
            For i = 0 To uxparent.Nodes.Count
                If parent = True Then
                    If uxparent.Nodes(i).Tag = entity_id Then
                        If type = 0 Then
                            get_value_from_node_tree = uxparent.Nodes(i).GetValue(2)
                        ElseIf type = 1 Then
                            get_value_from_node_tree = uxparent.Nodes(i).GetValue(13)
                        Else
                            get_value_from_node_tree = uxparent.Nodes(i).GetValue(14)
                        End If
                        Exit Function
                    End If
                Else
                    For j = 0 To uxparent.Nodes(i).Nodes.Count - 1
                        If uxparent.Nodes(i).Nodes(j).Tag = entity_id Then
                            If type = 0 Then
                                get_value_from_node_tree = uxparent.Nodes(i).Nodes(j).GetValue(2)
                            ElseIf type = 1 Then
                                get_value_from_node_tree = uxparent.Nodes(i).Nodes(j).GetValue(13)
                            ElseIf type = 2 Then
                                get_value_from_node_tree = uxparent.Nodes(i).Nodes(j).GetValue(14)
                            End If
                            Exit Function
                        End If

                    Next

                End If

            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_edit_attributes", "get_value_from_node_tree", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function
    Private Sub btnsubmit_Click(sender As Object, e As EventArgs) Handles btnsubmit.Click
        Try
            REM check no blanks
            Dim man_str As String = ""
            For i = 0 To Me.uxparent.Nodes.Count - 1

                If Me.uxparent.Nodes(i).GetValue(2) = "" Then
                    Me.uxparent.Nodes(i).StateImageIndex = 2
                    man_str = man_str + vbCrLf + Me.uxparent.Nodes(i).GetValue(0) + " for " + Me.uxparent.Nodes(i).GetValue(0)
                End If
                For j = 0 To Me.uxparent.Nodes(i).Nodes.Count - 1
                    If Me.uxparent.Nodes(i).Nodes(j).GetValue(2) = "" Then
                        Me.uxparent.Nodes(i).Nodes(j).StateImageIndex = 2
                        man_str = man_str + vbCrLf + Me.uxparent.Nodes(i).Nodes(j).GetValue(0) + " for " + Me.uxparent.Nodes(i).GetValue(0)
                    End If
                Next
            Next
            If man_str <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "The following items must be selected: " + man_str, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If


            Dim update_attributes As New bc_om_set_attribute_values
            Dim ua As bc_om_set_attribute_value

            REM check mandatory items entered
            For i = 0 To Me.uxparent.Nodes.Count - 1
                If Me.uxparent.Nodes(i).StateImageIndex = 2 Then
                    man_str = man_str + vbCrLf + Me.uxparent.Nodes(i).GetValue(0) + " for " + Me.uxparent.Nodes(i).GetValue(0)
                End If
                For j = 0 To Me.uxparent.Nodes(i).Nodes.Count - 1
                    If Me.uxparent.Nodes(i).Nodes(j).StateImageIndex = 2 Then
                        man_str = man_str + vbCrLf + Me.uxparent.Nodes(i).Nodes(j).GetValue(0) + " for " + Me.uxparent.Nodes(i).GetValue(0)
                    End If
                Next
            Next
            If man_str <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "The following items must be selected: " + man_str, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

            REM get changed values
            For i = 0 To _atts.parent_entities.Count - 1
                For j = 0 To _atts.parent_entities(i).attributes.Count - 1
                    If _atts.parent_entities(i).attributes(j).changed = True Then
                        ua = New bc_om_set_attribute_value
                        ua.entity_id = _atts.parent_entities(i).entity.entity_id
                        ua.attribute_id = _atts.parent_entities(i).attributes(j).attribute_id
                        ua.submission_code = _atts.parent_entities(i).attributes(j).submission_code
                        ua.value = get_value_from_node_tree(ua.entity_id, ua.attribute_id, True, 0)
                        ua.meth = get_value_from_node_tree(ua.entity_id, ua.attribute_id, True, 1)
                        ua.risk = get_value_from_node_tree(ua.entity_id, ua.attribute_id, True, 2)

                        REM if KVP assign key
                        If _atts.parent_entities(i).attributes(j).is_lookup = True Then
                            For k = 0 To _atts.lookup_lists.Count - 1
                                If _atts.lookup_lists(k).attribute_id = ua.attribute_id AndAlso _atts.lookup_lists(k).lookup_keys.Count > 0 Then
                                    For l = 0 To _atts.lookup_lists(k).lookup_values.Count - 1
                                        If _atts.lookup_lists(k).lookup_values(l) = ua.value Then
                                            ua.value = _atts.lookup_lists(k).lookup_keys(l)

                                            Exit For
                                        End If
                                    Next
                                    Exit For
                                End If
                            Next
                        End If
                        update_attributes.attributes.Add(ua)
                    End If
                Next
                For k = 0 To _atts.parent_entities(i).child_entities.Count - 1
                    For j = 0 To _atts.parent_entities(i).child_entities(k).attributes.Count - 1
                        If _atts.parent_entities(i).child_entities(k).attributes(j).changed = True Then

                            ua = New bc_om_set_attribute_value
                            ua.entity_id = _atts.parent_entities(i).child_entities(k).entity.entity_id
                            ua.attribute_id = _atts.parent_entities(i).child_entities(k).attributes(j).attribute_id
                            ua.submission_code = _atts.parent_entities(i).child_entities(k).attributes(j).submission_code
                            ua.value = get_value_from_node_tree(ua.entity_id, ua.attribute_id, False, 0)
                            ua.meth = get_value_from_node_tree(ua.entity_id, ua.attribute_id, False, 1)
                            ua.risk = get_value_from_node_tree(ua.entity_id, ua.attribute_id, False, 2)



                            REM if KVP assign key
                            If _atts.parent_entities(i).child_entities(k).attributes(j).is_lookup = True Then
                                For m = 0 To _atts.lookup_lists.Count - 1
                                    If _atts.lookup_lists(m).attribute_id = ua.attribute_id AndAlso _atts.lookup_lists(m).lookup_keys.Count > 0 Then
                                        For l = 0 To _atts.lookup_lists(m).lookup_values.Count - 1
                                            If _atts.lookup_lists(m).lookup_values(l) = ua.value Then
                                                ua.value = _atts.lookup_lists(m).lookup_keys(l)
                                                Exit For
                                            End If
                                        Next
                                        Exit For
                                    End If
                                Next
                            End If
                            update_attributes.attributes.Add(ua)
                        End If
                    Next
                Next
            Next
            If _save_msg <> "" And rec_change = True Then
                Dim omsgs As New bc_cs_message("Blue Curve", _save_msg, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                If omsgs.cancel_selected = False Then
                    Exit Sub
                End If
            End If
            RaiseEvent save(update_attributes)
            Me.Hide()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_edit_attributes", "save", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub








    Private Sub PopupContainerControl1_VisibleChanged(sender As Object, e As EventArgs) Handles PopupContainerControl1.VisibleChanged
        Try
            If sel_column = 3 Then
                Me.MemoEdit1.Text = uxparent.Selection(0).GetValue(13)
                Me.ptitle.Text = uxparent.Columns(13).GetTextCaption
            Else
                Me.MemoEdit1.Text = uxparent.Selection(0).GetValue(14)
                Me.ptitle.Text = uxparent.Columns(14).GetTextCaption
            End If
        Catch

        End Try
    End Sub

    Private Sub pcancel_Click(sender As Object, e As EventArgs)


    End Sub

    Private Sub MemoEdit1_EditValueChanged(sender As Object, e As EventArgs) Handles MemoEdit1.EditValueChanged
        Me.lpnum.Text = CStr(Me.MemoEdit1.Text.Length) + " of 1000"
        Me.psave.Enabled = True
    End Sub

    Private Sub psave_Click(sender As Object, e As EventArgs) Handles psave.Click
        If sel_column = 3 Then
            uxparent.Selection(0).SetValue(13, Me.MemoEdit1.Text)
        Else
            uxparent.Selection(0).SetValue(14, Me.MemoEdit1.Text)
        End If
        Dim w As Double
        w = Me.Width
        Me.Width = w - 1
        Me.Width = w
        Me.psave.Enabled = False
        If Me.WindowState = Windows.Forms.FormWindowState.Maximized Then
            Me.WindowState = Windows.Forms.FormWindowState.Normal
            Me.WindowState = Windows.Forms.FormWindowState.Maximized

        End If
        REM now mark as changed
        Me.btnsubmit.Enabled = True
        Me.uxparent.Selection(0).StateImageIndex = 4

        For i = 0 To _atts.parent_entities.Count - 1
            If _atts.parent_entities(i).entity.entity_id = Me.uxparent.Selection(0).Tag Then
                For j = 0 To _atts.parent_entities(i).attributes.Count - 1
                    _atts.parent_entities(i).attributes(j).changed = True
                    Exit Sub
                Next
                Exit Sub
            End If



            For k = 0 To _atts.parent_entities(i).child_entities.Count - 1
                If _atts.parent_entities(i).child_entities(k).entity.entity_id = Me.uxparent.Selection(0).Tag Then
                    For j = 0 To _atts.parent_entities(i).child_entities(k).attributes.Count - 1
                        _atts.parent_entities(i).child_entities(k).attributes(j).changed = True
                        Exit Sub
                    Next
                    Exit Sub
                End If
            Next
        Next

    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Dim w As Double
        w = Me.Width
        Me.Width = w - 1
        Me.Width = w
        Me.psave.Enabled = False
        If Me.WindowState = Windows.Forms.FormWindowState.Maximized Then
            Me.WindowState = Windows.Forms.FormWindowState.Normal
            Me.WindowState = Windows.Forms.FormWindowState.Maximized

        End If
    End Sub

    Private Sub btnsubmit_TextChanged(sender As Object, e As EventArgs) Handles btnsubmit.TextChanged

    End Sub
End Class
Public Class Cbc_am_edit_attributes
    WithEvents _view As Ibc_am_edit_attributes
    Dim _doc As bc_om_document
    Dim _atts_for_doc As New bc_om_attributes_for_doc

    Public Function load_data(view As bc_am_edit_attributes, doc As bc_om_document)

        load_data = False
        Try

            _view = view
            _doc = doc
            _atts_for_doc.doc = _doc
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                _atts_for_doc.db_read()
            Else
                _atts_for_doc.tmode = bc_cs_soap_base_class.tREAD
                If _atts_for_doc.transmit_to_server_and_receive(_atts_for_doc, True) = False Then
                    Exit Function
                End If
            End If
            load_data = _view.load_view(_atts_for_doc, doc.title, _atts_for_doc.save_msg)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_edit_attributes", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Private Sub save(update_attribute As bc_om_set_attribute_values) Handles _view.save

        Try
            update_attribute.doc_id = _doc.id
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                update_attribute.db_write()
            Else
                update_attribute.tmode = bc_cs_soap_base_class.tWRITE
                If update_attribute.transmit_to_server_and_receive(update_attribute, True) Then
                    Exit Sub
                End If


            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_edit_attributes", "save", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub


End Class
Public Interface Ibc_am_edit_attributes
    Function load_view(atts As bc_om_attributes_for_doc, title As String, save_msg As String) As Boolean
    Event save(update_attributes As bc_om_set_attribute_values)
End Interface
