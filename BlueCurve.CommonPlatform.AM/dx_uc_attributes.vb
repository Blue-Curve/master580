Imports System.Threading

Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM

Public Class dx_uc_attributes
    Public attributes As New List(Of bc_om_attribute)
    Public values As New List(Of bc_om_attribute_value)
    Public show_workflow As Boolean = False
    Public show_errors_box As Boolean = True
    Public errors As String
    Event fire_warnings(warnings As List(Of String))
    Event attribute_value_changed(attval As bc_om_attribute_value)
    Event attribute_selection_changed(att As bc_om_attribute)
    Event publish_attribute(attval As bc_om_attribute_value)

    Public delayed_input As Boolean = False

    Public Sub load_data()

        Try


            If show_workflow = False Then
                uxatt.Columns(3).Visible = False
                uxatt.Columns(4).Visible = False
            End If
            If show_errors_box = False Then
                Me.lerrors.Visible = False
                Me.uxatt.Height = Me.Height - (Me.uxatt.Location.Y * 2)
            End If
            load_attributes()
            check_warnings(-1)
            uxatt.Columns(1).Width = 20

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_uc_attributes", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub
    Public Sub clear_values()
        For i = 0 To uxatt.Nodes.Count - 1
            uxatt.Nodes(i).SetValue(2, "")
            uxatt.Nodes(i).SetValue(4, "")

        Next
    End Sub
    Public Sub disable_edit()


        uxatt.Columns(2).OptionsColumn.AllowEdit = False
        If show_workflow = True Then
            uxatt.Columns(3).Visible = False
        End If
    End Sub
    Public Sub enable_edit()
        uxatt.Columns(2).OptionsColumn.AllowEdit = True
        If show_workflow = True Then
            uxatt.Columns(3).Visible = True
        End If
    End Sub
    Sub load_attributes()

        Try
            Dim tag As String = ""
            Me.uxatt.BeginUpdate()


            Me.uxatt.Nodes.Clear()


            For i = 0 To attributes.Count - 1

                uxatt.Nodes.Add()

                uxatt.Nodes(i).SetValue(0, attributes(i).name)

                Me.uxatt.Nodes(i).Tag = attributes(i).attribute_id
                If i = 0 Then
                    tag = Me.uxatt.Nodes(i).Tag
                End If
                If attributes(i).is_lookup = False Then
                    If attributes(i).type_id = 1 Or attributes(i).type_id = 2 Or attributes(i).type_id = 5 Then
                        Me.uxatt.Nodes(i).SetValue(2, values(i).value)
                        If attributes(i).type_id = 1 Then
                            Me.uxatt.Nodes(i).StateImageIndex = 1
                        ElseIf attributes(i).type_id = 2 Then
                            Me.uxatt.Nodes(i).StateImageIndex = 5
                        Else
                            Me.uxatt.Nodes(i).StateImageIndex = 3
                        End If
                    ElseIf attributes(i).type_id = 3 Then
                        Me.uxatt.Nodes(i).StateImageIndex = 2

                        If values(i).value = "1" Then
                            Me.uxatt.Nodes(i).SetValue(2, "true")
                        Else

                            values(i).value = "0"
                            Me.uxatt.Nodes(i).SetValue(2, "false")
                        End If
                    End If
                Else
                    For j = 0 To attributes(i).lookup_keys.Count - 1
                        'If IsNumeric(values(i).value) Then
                        If CStr(attributes(i).lookup_keys(j)) = CStr(values(i).value) Then
                            Me.uxatt.Nodes(i).SetValue(2, attributes(i).lookup_values(j))
                            Exit For
                        End If
                        'End If
                    Next
                    Me.uxatt.Nodes(i).StateImageIndex = 4
                End If
                If attributes(i).nullable = False Then
                    Me.uxatt.Nodes(i).SetValue(1, "0")
                End If
                
                Me.uxatt.Nodes(i).SetValue(3, -1)
                If attributes(i).show_workflow = 1 Then
                    Me.uxatt.Nodes(i).SetValue(3, "1")

                    If attributes(i).is_lookup = True Then
                        For j = 0 To attributes(i).lookup_keys.Count - 1
                            If CStr(attributes(i).lookup_keys(j)) = CStr(values(i).published_value) Then
                                Me.uxatt.Nodes(i).SetValue(4, attributes(i).lookup_values(j))
                                Exit For
                            End If
                        Next
                    ElseIf attributes(i).type_id = 3 Then

                        If values(i).published_value = "1" Then
                            Me.uxatt.Nodes(i).SetValue(4, "true")
                        Else
                            Me.uxatt.Nodes(i).SetValue(4, "false")
                        End If
                    Else
                        Me.uxatt.Nodes(i).SetValue(4, values(i).published_value)
                    End If
                End If

                If attributes(i).persmission = 1 Then
                    Me.uxatt.Nodes(i).StateImageIndex = 9
                End If
            Next

            If tag <> "" Then
                set_list(tag)
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_uc_attributes", "load_attributes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.uxatt.EndUpdate()
        End Try
    End Sub
    Private Sub set_list(tag As String)
        Try


            Me.RepositoryItemComboBox1.Items.Clear()
            Me.RepositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor
            For i = 0 To attributes.Count - 1
                If attributes(i).attribute_id = tag Then
                    RaiseEvent attribute_selection_changed(attributes(i))
                    If attributes(i).persmission = 1 Then
                        Exit Sub
                    Else
                        REM non lookup
                        If attributes(i).is_lookup = False Then

                            If attributes(i).type_id = 1 Or attributes(i).type_id = 2 Or attributes(i).type_id = 5 Then
                                Me.RepositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard

                                Me.uxatt.Nodes(i).SetValue(2, values(i).value)
                            ElseIf attributes(i).type_id = 3 Then

                                Me.RepositoryItemComboBox1.Items.Add("true")
                                Me.RepositoryItemComboBox1.Items.Add("false")

                                If values(i).value = 1 Then
                                    Me.uxatt.Nodes(i).SetValue(2, "true")
                                Else
                                    Me.uxatt.Nodes(i).SetValue(2, "false")
                                End If
                            End If

                        Else

                            For j = 0 To attributes(i).lookup_values.Count - 1
                                Me.RepositoryItemComboBox1.Items.Add(attributes(i).lookup_values(j))
                            Next
                            For j = 0 To attributes(i).lookup_keys.Count - 1
                                'If IsNumeric(attributes(i).default_value) Then
                                If CStr(attributes(i).lookup_keys(j)) = CStr(values(i).value) Then
                                    Me.uxatt.Nodes(i).SetValue(2, attributes(i).lookup_values(j))
                                    Exit For
                                End If
                                'End If
                            Next
                        End If
                    End If
                    Exit For
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_uc_attributes", "set_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub

    

    Private Sub uxatt_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxatt.FocusedNodeChanged
        If Me.uxatt.Selection.Count = 0 Then
            RaiseEvent attribute_selection_changed(Nothing)
            Exit Sub
        End If

        set_list(e.Node.Tag)





    End Sub

    Class gedv
        Public value As String
        Public idx As Integer
    End Class
    Dim gev As New List(Of gedv)

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick

        Timer1.Stop()
        Dim j As Integer
        j = 0
        While j <= gev.Count - 1
            values(gev(j).idx).value = gev(j).value
            check_warnings(gev(j).idx)
            gev.RemoveAt(j)
        End While

    End Sub
    'Private Sub check_can_publish(i As Integer)
    '    RepositoryItemCheckEdit1.ReadOnly = True


    '    If attributes(i).show_workflow = 1 AndAlso values(i).value <> values(i).published_value Then

    '        RepositoryItemCheckEdit1.ReadOnly



    '    End If
    'End Sub
    Private Sub RepositoryItemComboBox1_SelectedValueChanged(sender As Object, e As EventArgs) Handles RepositoryItemComboBox1.EditValueChanged
        'REM validate data type if  number or date
        'Timer1.Stop()
        'Timer1.Start()
        Cursor = Windows.Forms.Cursors.WaitCursor
        Try
            Dim s As DevExpress.XtraEditors.ComboBoxEdit
            s = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
            Dim nev, oev As String

            oev = s.OldEditValue
            nev = s.EditValue

            Dim attribute_index As Integer = -1


            For i = 0 To attributes.Count - 1
                REM if date invoke popup

                If attributes(i).attribute_id = uxatt.Selection(0).Tag AndAlso attributes(i).type_id = 5 Then


                    attribute_index = i
                    Dim fpu As New bc_dx_att_popup1
                    Dim cpu As New Cbc_dx_att_popup
                    Dim da As DateTime
                    Try
                        da = oev
                    Catch ex As Exception
                        da = DateAndTime.Now

                    End Try
                    If cpu.load_data(fpu, 5, 0, "", da) = True Then
                        fpu.ShowDialog()
                        If cpu.bsave = False Then
                            uxatt.Selection(0).SetValue(2, oev)
                        Else
                            values(i).value_changed = True

                            values(i).value = Format(cpu._dval, "dd MMM yyyy")
                            uxatt.Selection(0).SetValue(2, values(i).value)
                            uxatt.Nodes(i).StateImageIndex = 6
                            check_warnings(i)

                        End If
                    End If

                    Exit Sub
                End If
                REM if memo invoke popup
                If attributes(i).attribute_id = uxatt.Selection(0).Tag AndAlso attributes(i).type_id = 1 AndAlso attributes(i).repeats = 1 Then
                    attribute_index = i
                    Dim fpu As New bc_dx_att_popup1
                    Dim cpu As New Cbc_dx_att_popup
                    If cpu.load_data(fpu, 1, attributes(i).length, oev, "1-1-1900") = True Then
                        fpu.ShowDialog()
                        If cpu.bsave = False Then
                            uxatt.Selection(0).SetValue(2, oev)
                            Exit Sub
                        Else
                            values(i).value_changed = True
                            uxatt.Selection(0).SetValue(2, cpu._tval)
                            values(i).value = cpu._tval
                            uxatt.Nodes(i).StateImageIndex = 6
                            check_warnings(i)

                        End If
                    End If

                    Exit Sub

                    REM step attribute TBD
                ElseIf attributes(i).attribute_id = uxatt.Selection(0).Tag AndAlso attributes(i).type_id = 1 AndAlso attributes(i).repeats = 2 Then

                    attribute_index = i
                    Dim fsa = New bc_dx_step_attribute
                    Dim csa = New Cbc_dx_step_attribute
                    If csa.load_data(fsa, attributes(i).name, values(i).steps) = True Then
                        fsa.ShowDialog()

                        If csa.bsave = False Then
                            uxatt.Selection(0).SetValue(2, oev)
                            Exit Sub
                        Else

                            values(i).value_changed = True
                            For t = 0 To csa._steps.Count - 1
                                With csa._steps(t)
                                    If Now.ToUniversalTime >= .date_from And Now.ToUniversalTime < .date_to Then
                                        nev = .value
                                        uxatt.Selection(0).SetValue(2, .value)
                                        Exit For
                                    End If
                                End With
                            Next


                            values(i).steps = csa._steps
                            uxatt.Nodes(i).StateImageIndex = 6
                            check_warnings(i)
                        End If
                    End If
                End If
            Next

            'ev = s.EditValue
            For i = 0 To attributes.Count - 1
                If attributes(i).attribute_id = uxatt.Selection(0).Tag Then
                    attribute_index = i
                    values(i).value_changed = True
                    uxatt.Nodes(i).StateImageIndex = 6
                    If attributes(i).is_lookup = False Then
                        If attributes(i).type_id = 3 Then
                            values(i).value = 0
                            If nev = "true" Then
                                values(i).value = 1
                            End If
                        Else

                            If delayed_input = False Then
                                values(i).value = nev
                            Else
                                Timer1.Stop()
                                Timer1.Start()
                                Dim lgev = New gedv

                                For j = 0 To gev.Count - 1
                                    If gev(j).idx = i Then
                                        gev.RemoveAt(j)
                                        Exit For
                                    End If
                                Next
                                lgev.value = nev
                                lgev.idx = i
                                gev.Add(lgev)
                                Exit Sub
                            End If
                        End If
                    Else
                        REM store key
                        For j = 0 To attributes(i).lookup_values.Count - 1
                            If attributes(i).lookup_values(j) = nev Then
                                values(i).value = attributes(i).lookup_keys(j)
                                Exit For
                            End If
                        Next
                    End If
                    Exit For
                End If
            Next

            check_warnings(attribute_index)

        Catch ex As Exception
            MsgBox(ex.Message)

        Finally
            Cursor = Windows.Forms.Cursors.Default
        End Try


    End Sub


    Sub check_warnings(attribute_index As Integer)
        Try
            lerrors.Text = ""
            errors = ""
            Dim warnings As New List(Of String)



            For i = 0 To attributes.Count - 1
               


                If attributes(i).type_id = 2 And values(i).value <> "" And IsNumeric(values(i).value) = False Then
                    errors = errors + attributes(i).name + " must be numeric" + vbCrLf
                    warnings.Add(attributes(i).name + " must be numeric")
                    uxatt.Nodes(i).StateImageIndex = 7

                End If
                If attributes(i).type_id = 5 And values(i).value <> "" And IsDate(values(i).value) = False Then
                    errors = errors + attributes(i).name + " must be a date" + vbCrLf
                    warnings.Add(attributes(i).name + " must be a date")
                    uxatt.Nodes(i).StateImageIndex = 7
                End If
                If attributes(i).nullable = False And values(i).value = "" Then
                    errors = errors + attributes(i).name + " must be entered" + vbCrLf
                    warnings.Add(attributes(i).name + " must  be entered")
                    uxatt.Nodes(i).StateImageIndex = 7
                End If
            Next
            lerrors.Text = errors
            If warnings.Count > 0 Then
                RaiseEvent fire_warnings(warnings)
            End If
            If uxatt.Nodes.Count > 0 Then
                If attribute_index > -1 And uxatt.FocusedNode.StateImageIndex <> 7 Then
                    values(attribute_index).submission_code = attributes(attribute_index).submission_code
                    values(attribute_index).attribute_Id = attributes(attribute_index).attribute_id
                    values(attribute_index).show_workflow = attributes(attribute_index).show_workflow
                    RaiseEvent attribute_value_changed(values(attribute_index))
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_uc_attributes", "check warnings", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub



   
    Private Sub RepositoryItemImageComboBox3_Click(sender As Object, e As EventArgs) Handles RepositoryItemImageComboBox3.Click
        If uxatt.FocusedNode.GetValue(3) = "1" Then
            For i = 0 To attributes.Count - 1
                If attributes(i).attribute_id = uxatt.FocusedNode.Tag Then
                    If values(i).value <> values(i).published_value AndAlso Timer1.Enabled = False AndAlso uxatt.FocusedNode.StateImageIndex <> 7 Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Do you wish to publish the value for " + uxatt.FocusedNode.GetValue(2) + " for " + attributes(i).name, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                        If omsg.cancel_selected = True Then
                            Exit Sub
                        End If
                        values(i).publish_draft_value = True
                        uxatt.Selection(0).SetValue(4, uxatt.Selection(0).GetValue(2))

                        REM store key
                        If attributes(i).is_lookup = True Then
                            For j = 0 To attributes(i).lookup_values.Count - 1
                                If attributes(i).lookup_values(j) = uxatt.Selection(0).GetValue(2) Then
                                    values(i).published_value = attributes(i).lookup_keys(j)
                                    Exit For
                                End If
                            Next
                        ElseIf attributes(i).type_id = 3 Then
                            If uxatt.Selection(0).GetValue(2) = "true" Then
                                values(i).published_value = 1
                            Else
                                values(i).published_value = 0
                            End If
                        Else
                            values(i).published_value = uxatt.Selection(0).GetValue(2)
                        End If

                        values(i).submission_code = attributes(i).submission_code
                        values(i).attribute_Id = attributes(i).attribute_id
                        values(i).show_workflow = attributes(i).show_workflow
                        RaiseEvent publish_attribute(values(i))
                        values(i).publish_draft_value = False
                        Exit For
                    End If
                    End If
            Next
        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
