Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM

Public Class bc_dx_step_attribute
    Implements Ibc_dx_step_attribute
    Public Event save_data(steps As List(Of bc_om_step_attribute)) Implements Ibc_dx_step_attribute.save_data
    Dim _steps As List(Of bc_om_step_attribute)

    Public Function load_view(caption As String, steps As List(Of bc_om_step_attribute)) As Boolean Implements Ibc_dx_step_attribute.load_view

        Try
            _steps = steps
            Me.Text = "Step attributes for  " + caption
            load_Steps()
            Return True
        Catch ex As Exception
            Dim oerr = New bc_cs_error_log("bc_dx_step_attribute", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Sub load_Steps(Optional sel As Integer = -1)
        Try
            uxsteps.BeginUpdate()
            uxsteps.ClearNodes()


            For i = 0 To _steps.Count - 1
                uxsteps.Nodes.Add()
                uxsteps.Nodes(uxsteps.Nodes.Count - 1).SetValue(0, _steps(i).value)
                uxsteps.Nodes(uxsteps.Nodes.Count - 1).SetValue(1, _steps(i).date_from.ToLocalTime)
                uxsteps.Nodes(uxsteps.Nodes.Count - 1).SetValue(2, Format(_steps(i).date_to.ToLocalTime, "dd-MMM-yyyy"))
            Next
            set_in_range()
            If sel > -1 Then
                uxsteps.Nodes(sel).Selected = True
            End If

        Catch ex As Exception
            Dim oerr = New bc_cs_error_log("bc_dx_step_attribute", "load_Steps", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            uxsteps.EndUpdate()
        End Try
    End Sub
    Sub value_changed(sender As Object, e As Object) Handles RepositoryItemTextEdit1.EditValueChanged
        Try
            Dim selidx As Integer
            selidx = get_selected_index()
            Dim s As DevExpress.XtraEditors.TextEdit
            s = DirectCast(sender, DevExpress.XtraEditors.TextEdit)

            _steps(selidx).value = s.EditValue
        Catch ex As Exception
            Dim oerr = New bc_cs_error_log("bc_dx_step_attribute", "value_changed", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Sub date_from_changed(sender As Object, e As Object) Handles RepositoryItemDateEdit4.EditValueChanged
        Try
            Dim selidx As Integer
            selidx = get_selected_index()
            Dim s As DevExpress.XtraEditors.DateEdit
            s = DirectCast(sender, DevExpress.XtraEditors.DateEdit)
            Dim df As New Date
            df = s.EditValue
            df = df.ToUniversalTime

            If _steps.Count = 1 Then
                _steps(selidx).date_from = df

            ElseIf selidx = 0 And _steps.Count > 1 Then
                If df <= _steps(1).date_from Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Date From: " + Format(df.ToLocalTime, "dd-MMM-yyyy") + " must be greater than " + Format(_steps(1).date_from.ToLocalTime, "dd-MMM-yyyy"), bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    uxsteps.Nodes(0).SetValue(1, s.OldEditValue)
                    Exit Sub
                Else
                    uxsteps.Nodes(selidx).SetValue(1, Format(s.EditValue, "dd-MMM-yyyy"))
                    uxsteps.Nodes(selidx + 1).SetValue(2, Format(s.EditValue, "dd-MMM-yyyy"))
                    _steps(selidx).date_from = df
                    _steps(selidx + 1).date_to = df
                End If
            ElseIf _steps.Count > 1 AndAlso selidx = _steps.Count - 1 Then
                If df >= _steps(_steps.Count - 2).date_from Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Date From: " + Format(df.ToLocalTime, "dd-MMM-yyyy") + " must be less than " + Format(_steps(_steps.Count - 2).date_from.ToLocalTime, "dd-MMM-yyyy"), bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    uxsteps.Nodes(selidx).SetValue(1, s.OldEditValue)
                    Exit Sub
                Else
                    _steps(selidx).date_from = df
                End If
            Else

                If df >= _steps(selidx - 1).date_from Or df <= _steps(selidx + 1).date_from Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Date From: " + Format(df.ToLocalTime, "dd-MMM-yyyy") + " must be between than " + Format(_steps(selidx + 1).date_from.ToLocalTime, "dd-MMM-yyyy") + " and " + Format(_steps(selidx - 1).date_from.ToLocalTime, "dd-MMM-yyyy"), bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    uxsteps.Nodes(selidx).SetValue(1, s.OldEditValue)
                    Exit Sub
                Else
                    uxsteps.Nodes(selidx).SetValue(1, Format(s.EditValue, "dd-MMM-yyyy"))
                    uxsteps.Nodes(selidx + 1).SetValue(2, Format(s.EditValue, "dd-MMM-yyyy"))
                    _steps(selidx).date_from = df
                    _steps(selidx + 1).date_to = df

                End If
            End If
            set_in_range()



        Catch ex As Exception
            Dim oerr = New bc_cs_error_log("bc_dx_step_attribute", "date_from_changed", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try



    End Sub

    Function get_selected_index() As Integer
        For i = 0 To uxsteps.Nodes.Count - 1
            If uxsteps.FocusedNode.GetValue(1) = uxsteps.Nodes(i).GetValue(1) Then
                get_selected_index = i
                Exit Function
            End If
        Next
    End Function



    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub bclose_Click(sender As Object, e As EventArgs) Handles bclose.Click
        RaiseEvent save_data(_steps)


        Me.Hide()
    End Sub

    Private Sub uxsteps_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs) Handles uxsteps.FocusedNodeChanged
        bdel.Enabled = False

        If uxsteps.FocusedNode Is Nothing = False Then
            bdel.Enabled = True
        End If

    End Sub

  

    Private Sub bdel_Click(sender As Object, e As EventArgs) Handles bdel.Click
        delete()
    End Sub

    Private Sub delete()
        Try
            For i = 0 To _steps.Count - 1

                If uxsteps.Nodes(i).GetValue(1) = uxsteps.FocusedNode.GetValue(1) Then
                    If i = 0 AndAlso _steps.Count > 1 Then
                        _steps(i + 1).date_to = New Date(9999, 9, 9)
                    ElseIf i < _steps.Count - 1 Then
                        _steps(i + 1).date_to = _steps(i - 1).date_from
                    End If
                    _steps.RemoveAt(i)
                    load_Steps()
                    Exit For
                End If
            Next


        Catch ex As Exception
            Dim oerr = New bc_cs_error_log("bc_dx_step_attribute", "delete step", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Sub set_in_range()
        Dim da As New Date()
        da = Now
        da = da.ToUniversalTime
        For i = 0 To _steps.Count - 1
            uxsteps.Nodes(i).StateImageIndex = -1
            If da >= _steps(i).date_from And da < _steps(i).date_to Then
                uxsteps.Nodes(i).StateImageIndex = 0
            End If

        Next
    End Sub
    Private Sub DateEdit1_EditValueChanged(sender As Object, e As EventArgs) Handles DateEdit1.EditValueChanged
        add()
    End Sub

    Sub add()
        Try
            If DateEdit1.EditValue Is Nothing Then
                Exit Sub
            End If
            Dim us As New bc_om_step_attribute
            us.value = "Enter Value here"
            us.date_from = DateEdit1.EditValue
            us.date_from = us.date_from.ToUniversalTime



            REM no nodes so set to current
            If _steps.Count = 0 Then
                us.date_to = New Date(9999, 9, 9)
                _steps.Add(us)
                load_Steps()
            Else
                REM chek date from doesnt alreay exist
                For i = 0 To _steps.Count - 1
                    If _steps(i).date_from = us.date_from Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Date From: " + Format(_steps(i).date_from.ToLocalTime, "dd-MMM-yyyy") + " Already Exists", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                Next
                Dim idx As Integer
                For i = 0 To _steps.Count - 1
                    REM insert at correct location
                    If us.date_from > _steps(0).date_from Then
                        us.date_to = New Date(9999, 9, 9)
                        _steps(0).date_to = us.date_from
                        _steps.Insert(0, us)
                        idx = 0
                    ElseIf i < _steps.Count - 1 AndAlso (us.date_from < _steps(i).date_from And us.date_from > _steps(i + 1).date_from) Then
                        us.date_to = _steps(i).date_from
                        _steps(i + 1).date_to = us.date_from
                        _steps.Insert(i + 1, us)
                        idx = i + 1
                        Exit For
                    ElseIf i = _steps.Count - 1 AndAlso us.date_from < _steps(i).date_from Then
                        us.date_to = _steps(i).date_from
                        _steps.Add(us)
                        idx = _steps.Count - 1

                    End If
                Next
                load_Steps(idx)
            End If

        Catch ex As Exception
            Dim oerr = New bc_cs_error_log("bc_dx_step_attribute", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            DateEdit1.EditValue = Nothing
        End Try
    End Sub
End Class
Public Class Cbc_dx_step_attribute
    WithEvents _view As Ibc_dx_step_attribute
    Public _steps As New List(Of bc_om_step_attribute)
    Public bsave As Boolean = False
    Public Function load_data(view As Ibc_dx_step_attribute, caption As String, steps As List(Of bc_om_step_attribute))
        load_data = False

        Try
            _view = view
            Dim _steps As New List(Of bc_om_step_attribute)
            For i = 0 To steps.Count - 1
                _steps.Add(steps(i))
            Next

            Return _view.load_view(caption, _steps)
            load_data = True
        Catch ex As Exception
            Dim oerr = New bc_cs_error_log("Cbc_dx_step_attribute", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Sub save(steps As List(Of bc_om_step_attribute)) Handles _view.save_data
        Try
            For i = 0 To steps.Count - 1
                _steps.Add(steps(i))
            Next
            bsave = True
        Catch ex As Exception
            Dim oerr = New bc_cs_error_log("Cbc_dx_step_attribute", "save", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
Public Interface Ibc_dx_step_attribute
    Function load_view(caption As String, steps As List(Of bc_om_step_attribute)) As Boolean
    Event save_data(steps As List(Of bc_om_step_attribute))
End Interface