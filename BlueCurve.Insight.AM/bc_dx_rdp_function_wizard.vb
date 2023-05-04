Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Threading


Public Class bc_dx_rdp_function_wizard
    Implements Ibc_dx_rdp_function_wizard
    Dim _efs As bc_om_rtd_functions
    Dim _rtd_mode As Boolean
    'WithEvents params As New List(Of dx_rtd_param)
    Dim is_list_function As Boolean = False
    Dim is_universe As Boolean = False
    Public Event write_formula(formula As String, params As List(Of String), dimensions As List(Of String), is_list_function As Boolean, number_in_list As Integer, _rtd_mode As Boolean) Implements Ibc_dx_rdp_function_wizard.write_formula
    Public Event set_address(param_name As String) Implements Ibc_dx_rdp_function_wizard.set_address
    Public Sub New()

        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Public Function load_view(efs As bc_om_rtd_functions, existingformula As String, rtd_mode As Boolean) As Boolean Implements Ibc_dx_rdp_function_wizard.load_view
        _rtd_mode = rtd_mode
        If _rtd_mode = True Then
            Me.Text = "Blue Curve - Excel RTD function wizard"
        Else
            Me.Text = "Blue Curve - Excel function wizard"
            Me.lrow.Visible = False
            Me.crow.Visible = False
        End If
        _efs = efs
        For i = 0 To _efs.functions.Count - 1
            cfunctions.Properties.Items.Add(_efs.functions(i).display_name)
        Next
        If existingformula <> "" Then
            If _rtd_mode = True Then
                If set_up_existing_formula_rtd(existingformula) = False Then
                    Exit Function
                End If
            Else
                If set_up_existing_formula_batch(existingformula) = False Then
                    Exit Function
                End If
            End If
        End If
        crow.Properties.Items.Clear()
        crow.Properties.Items.Add("1")
        crow.Properties.Items.Add("2")
        For i = 1 To 20000
            If i Mod 10 = 0 Then
                crow.Properties.Items.Add(i)
            End If
        Next
        crow.SelectedIndex = 6

        load_view = True
    End Function
    Public Sub set_status(text As String) Implements Ibc_dx_rdp_function_wizard.set_status



        lstatus.Text = text
    End Sub
    Private Function set_up_existing_formula_rtd(existingformula As String) As Boolean
        Try


            Dim func As String
            func = existingformula.Substring(19, existingformula.Length - 19)

            func = func.Substring(0, InStr(func, """") - 1)

            Dim found As Boolean = False
            For i = 0 To _efs.functions.Count - 1
                If _efs.functions(i).name = func Then
                    cfunctions.Text = _efs.functions(i).display_name
                    found = True
                    Exit For
                End If
            Next
            If found = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Cannot parse exiting formula " + existingformula, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            Dim param_vals As New List(Of String)
            Dim rest As String

            rest = Replace(existingformula, """", "")
            rest = Replace(rest, "=RTD(bluecurvertd." + func + ",,", "")
            While InStr(rest, ",") > 0

                param_vals.Add(rest.Substring(0, InStr(rest, ",") - 1))
                rest = rest.Substring(InStr(rest, ","), rest.Length - InStr(rest, ","))
            End While
            If rest.Length > 1 AndAlso rest.Substring(rest.Length - 1, 1) = ")" Then
                rest = rest.Substring(0, rest.Length - 1)
                param_vals.Add(rest)
            End If

            Dim x As dx_rtd_param
            Dim co As Integer = 0

            For i = 0 To Controls.Count - 1
                If Controls(i).GetType.ToString = "BlueCurve.Insight.AM.dx_rtd_param" Then
                    x = DirectCast(Controls(i), dx_rtd_param)

                    If x.delimit_value = True Then
                        x.cvalue.Text = """" + param_vals(co) + """"
                    Else
                        x.cvalue.Text = param_vals(co)
                    End If
                    co = co + 1
                End If
            Next
            REM if more params than controls then these are dimensions
            If co < param_vals.Count Then
                For i = 0 To chkdim.Items.Count - 1
                    chkdim.Items(i).CheckState = Windows.Forms.CheckState.Unchecked
                    If chkdim.Items(i).Value = param_vals(param_vals.Count - 1) Then
                        chkdim.Items(i).CheckState = Windows.Forms.CheckState.Checked
                    End If
                Next
            End If

            set_up_existing_formula_rtd = True

        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Cannot parse exiting formula " + existingformula + vbCr + vbCr + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        End Try
    End Function

    Private Function set_up_existing_formula_batch(existingformula As String) As Boolean
        Try


            Dim func As String
            func = existingformula.Substring(1, existingformula.Length - 1)

            func = func.Substring(0, InStr(func, "(") - 1)

            Dim found As Boolean = False
            For i = 0 To _efs.functions.Count - 1
                If _efs.functions(i).name = func Then
                    cfunctions.Text = _efs.functions(i).display_name
                    found = True
                    Exit For
                End If
            Next

            If found = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Cannot parse exiting formula " + existingformula, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            Dim param_vals As New List(Of String)
            Dim rest As String


            rest = Replace(existingformula, """", "")
            rest = Replace(rest, "=" + func + "(", "")



            While InStr(rest, ",") > 0
                param_vals.Add(rest.Substring(0, InStr(rest, ",") - 1))
                rest = rest.Substring(InStr(rest, ","), rest.Length - InStr(rest, ","))
            End While

            If rest.Length > 0 AndAlso rest.Substring(rest.Length - 1, 1) = ")" Then
                rest = rest.Substring(0, rest.Length - 1)


                param_vals.Add(rest)
            End If

            Dim x As dx_rtd_param
            Dim co As Integer = 0

            For i = 0 To Controls.Count - 1
                If Controls(i).GetType.ToString = "BlueCurve.Insight.AM.dx_rtd_param" Then
                    x = DirectCast(Controls(i), dx_rtd_param)

                    If x.delimit_value = True Then
                        x.cvalue.Text = """" + param_vals(co) + """"
                    Else
                        x.cvalue.Text = param_vals(co)
                    End If
                    co = co + 1
                End If
            Next
            REM if more params than controls then these are dimensions

           

            If co < param_vals.Count Then
                Dim dims As New List(Of String)
                For i = 0 To chkdim.Items.Count - 1
                    dims.Add(chkdim.Items(i).Value)
                Next
                chkdim.Items.Clear()

                For j = co To param_vals.Count - 1
                    chkdim.Items.Add(param_vals(j), True)
                Next
                Dim dfound As Boolean = False
                For i = 0 To dims.Count - 1
                    dfound = False
                    For j = 0 To chkdim.Items.Count - 1
                        If chkdim.Items(j).Value = dims(i) Then
                            dfound = True
                            Exit For
                        End If
                    Next
                    If dfound = False Then
                        chkdim.Items.Add(dims(i), False)
                    End If
                Next
            End If

            set_up_existing_formula_batch = True

        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Cannot parse exiting formula " + existingformula + vbCr + vbCr + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        End Try
    End Function

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Close()

    End Sub
    
    Private Sub bok_Click(sender As Object, e As EventArgs) Handles bok.Click
        Try
            Cursor = Windows.Forms.Cursors.WaitCursor



            Dim params As New List(Of String)
            Dim x As dx_rtd_param
            For i = 0 To Controls.Count - 1
                If Controls(i).GetType.ToString = "BlueCurve.Insight.AM.dx_rtd_param" Then
                    x = DirectCast(Controls(i), dx_rtd_param)
                    params.Add(x.cvalue.Text)
                End If
            Next
            Dim dimensions As New List(Of String)
            For i = 0 To chkdim.Items.Count - 1
                If chkdim.Items(i).CheckState = Windows.Forms.CheckState.Checked Then
                    dimensions.Add(chkdim.Items(i).Value)
                End If
            Next
            If chkdim.Items.Count > 0 AndAlso dimensions.Count = 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "please select at least 1 dimension", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            Dim number_in_list As Integer
            number_in_list = 1
            If Me.crow.Enabled = True Then
                number_in_list = CInt(crow.Text)
            End If

            RaiseEvent write_formula(_efs.functions(cfunctions.SelectedIndex).name, params, dimensions, is_list_function, number_in_list, _rtd_mode)
            If chkopen.Checked = False Then
                Close()
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bok", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor = Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub cfunctions_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cfunctions.SelectedIndexChanged
        Try
            bok.Enabled = False
            is_list_function = False
            is_universe = False
            crow.Enabled = False
            lrow.Enabled = False
            search_lists.Clear()

            If cfunctions.SelectedIndex = -1 Then
                Exit Sub

            End If
            'params.Clear()
            Dim i As Integer
            While i < Controls.Count

                If Controls(i).GetType.ToString = "BlueCurve.Insight.AM.dx_rtd_param" Then

                    Controls.RemoveAt(i)
                    i = i - 1
                End If
                i = i + 1
            End While

            Dim dp As System.Drawing.Point
            dp.X = cfunctions.Location.X
            dp.Y = cfunctions.Location.Y + 30

            Me.Height = 237


            Dim param As dx_rtd_param
            For i = 0 To _efs.functions(cfunctions.SelectedIndex).params.Count - 1
                param = New dx_rtd_param
                param.lname.Text = _efs.functions(cfunctions.SelectedIndex).params(i).name
                param.Visible = True
                param.Location = dp
                param.Width = cfunctions.Width

                dp.Y = dp.Y + param.Height
                Me.Height = Me.Height + param.Height
                If _efs.functions(cfunctions.SelectedIndex).params(i).is_universe = True Then
                    param.is_universe = True
                    is_universe = True
                End If

                If _efs.functions(cfunctions.SelectedIndex).params(i).class_search = True Then
                    For j = 0 To _efs.classes.classes.Count - 1
                        param.cvalue.Properties.Items.Add(_efs.classes.classes(j).class_name)
                    Next
                    param.cvalue.Properties.Sorted = True


                ElseIf _efs.functions(cfunctions.SelectedIndex).params(i).entity_search = True Then
                    param.cvalue.Properties.AutoComplete = False
                    Dim ent_list As New List(Of bc_om_entity)
                    search_lists.Add(ent_list)
                    param.list_id = search_lists.Count - 1

                ElseIf _efs.functions(cfunctions.SelectedIndex).params(i).item_search = True Then
                    For j = 0 To _efs.item_names.Count - 1
                        param.cvalue.Properties.Items.Add(_efs.item_names(j))
                    Next
                ElseIf _efs.functions(cfunctions.SelectedIndex).params(i).list_index Then
                    is_list_function = True
                    Me.Height = Me.Height - param.Height
                    crow.Enabled = True
                    lrow.Enabled = True
                    Continue For
                ElseIf _efs.functions(cfunctions.SelectedIndex).params(i).lookupsql <> "" Then
                    For j = 0 To _efs.lookups.Count - 1
                        If _efs.lookups(j).lookupsql = _efs.functions(cfunctions.SelectedIndex).params(i).lookupsql Then
                            param.cvalue.Properties.Items.Add(_efs.lookups(j).value)
                        End If
                    Next
                End If


                Dim default_found As Boolean = False
                If (_efs.functions(cfunctions.SelectedIndex).params(i).default_value <> "") Then
                    For j = 0 To _efs.default_values.Count - 1
                        If _efs.default_values(j).lookupsql = _efs.functions(cfunctions.SelectedIndex).params(i).default_value Then
                            param.cvalue.Text = _efs.default_values(j).value
                            default_found = True
                            Exit For
                        End If
                    Next
                    If default_found = False Then
                        param.cvalue.Text = _efs.functions(cfunctions.SelectedIndex).params(i).default_value
                    End If
                End If

                param.delimit_value = _efs.functions(cfunctions.SelectedIndex).params(i).delimit_value

                AddHandler param.selbutton, AddressOf param_sel_button
                AddHandler param.listchanged, AddressOf param_list_changed
                AddHandler param.stextchanged, AddressOf param_text_changed
                'params.Add(param)
                Controls.Add(param)
            Next
            REM diemnsions
            Me.chkdim.Items.Clear()

            For i = 0 To _efs.functions(cfunctions.SelectedIndex).dimensions.Count - 1
                If _efs.functions(cfunctions.SelectedIndex).dimensions(i).ord = 1 Then
                    Me.chkdim.Items.Add(_efs.functions(cfunctions.SelectedIndex).dimensions(i).name, True)
                Else
                    Me.chkdim.Items.Add(_efs.functions(cfunctions.SelectedIndex).dimensions(i).name, False)
                End If
            Next


            bok.Enabled = True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("cfunctions", "selectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub param_sel_button(ByRef param As dx_rtd_param)

        RaiseEvent set_address(param.lname.Text)
    End Sub
    'Dim search_results As bc_om_real_time_search
    'Dim entities_of_class As New List(Of bc_om_entity)
    Dim search_lists As New List(Of List(Of bc_om_entity))

    Private Sub param_list_changed(ByRef param As dx_rtd_param)

        Try
            Cursor = Windows.Forms.Cursors.WaitCursor


            REM see if any params are dependent on this
            For i = 0 To _efs.functions(cfunctions.SelectedIndex).params.Count - 1
                If _efs.functions(cfunctions.SelectedIndex).params(i).dependent_on_item = param.lname.Text Then

                    Dim x As dx_rtd_param
                    For j = 0 To Controls.Count - 1
                        If Controls(j).GetType.ToString = "BlueCurve.Insight.AM.dx_rtd_param" Then
                            x = DirectCast(Controls(j), dx_rtd_param)
                            If x.lname.Text = _efs.functions(cfunctions.SelectedIndex).params(i).name Then
                                x.cvalue.Properties.Items.Clear()
                                Exit For
                            End If
                        End If
                    Next


                    If _efs.functions(cfunctions.SelectedIndex).params(i).entity_search = True And param.cvalue.SelectedIndex > -1 Then

                        For k = 0 To _efs.classes.classes.Count - 1
                            If _efs.classes.classes(k).class_name = param.cvalue.Text Then

                                x.class_id = _efs.classes.classes(k).class_id
                                Dim querya = From en In _efs.entities.entity
                                Where en.class_id = _efs.classes.classes(k).class_id
                                Select New bc_om_entity With {
                                .id = en.id,
                                .name = en.name}
                                search_lists(x.list_id) = querya.ToList
                                load_all_entities_of_class(x)

                                Exit For
                            End If
                        Next


                    ElseIf _efs.functions(cfunctions.SelectedIndex).params(i).item_search = True Then

                        If param.cvalue.SelectedIndex > -1 Then

                            Dim na As String
                            Dim cid As Long
                            cid = param.class_id

                            na = param.cvalue.Text

                            Dim oitems As bc_om_ef_items
                            Dim cent As New List(Of bc_om_entity)
                            Dim filter_list As Boolean = False
                            If is_universe = True Then
                                oitems = New bc_om_ef_items(0)
                                oitems.universe = na
                            Else
                                Dim querya = From en In _efs.entities.entity
                                Where en.class_id = cid And en.name = na
                                Select New bc_om_entity With {
                                         .id = en.id,
                                         .name = en.name}
                                cent = querya.ToList()
                                If (cent.Count = 1 AndAlso cent(0).id <> 0) Then
                                    oitems = New bc_om_ef_items(cent(0).id)
                                    filter_list = True
                                End If
                            End If
                            If is_universe = True Or filter_list = True Then

                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    oitems.db_read()
                                Else
                                    oitems.tmode = bc_cs_soap_base_class.tREAD
                                    If oitems.transmit_to_server_and_receive(oitems, True) = False Then
                                        Exit Sub
                                    End If
                                End If

                                For j = 0 To oitems.item_names.Count - 1
                                    x.cvalue.Properties.Items.Add(oitems.item_names(j))
                                Next

                            Else
                                For j = 0 To _efs.item_names.Count - 1
                                    x.cvalue.Properties.Items.Add(_efs.item_names(j))
                                Next
                            End If
                        Else
                                For j = 0 To _efs.item_names.Count - 1
                                    x.cvalue.Properties.Items.Add(_efs.item_names(j))
                                Next
                        End If
                    End If
                End If

            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_rdp_function_wizard", "param_list_changed", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor = Windows.Forms.Cursors.Default


        End Try
    End Sub
    Sub load_all_entities_of_class(entity_param As dx_rtd_param)
        entity_param.cvalue.Text = ""
        entity_param.cvalue.Properties.Items.Clear()
        For k = 0 To search_lists(entity_param.list_id).Count - 1
            entity_param.cvalue.Properties.Items.Add(search_lists(entity_param.list_id)(k).name)
        Next

    End Sub
    Dim search_param As New dx_rtd_param

    Private Sub param_text_changed(ByRef param As dx_rtd_param)

        For i = 0 To _efs.functions(cfunctions.SelectedIndex).params.Count - 1
            If _efs.functions(cfunctions.SelectedIndex).params(i).name = param.lname.Text AndAlso _efs.functions(cfunctions.SelectedIndex).params(i).entity_search = True Then
                REM entity search
                Timer1.Stop()
                search_param = param
                Timer1.Start()
            End If
        Next
    End Sub
    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Try
            REM run entity search
            Cursor = Windows.Forms.Cursors.WaitCursor

            Timer1.Stop()


            If search_param.cvalue.Text = "" Then
                load_all_entities_of_class(search_param)
                Exit Sub
            Else
                Dim search_results = New bc_om_real_time_search
                search_results.class_id = search_param.class_id

                search_results.search_text = ""
                search_results.mine = False
                search_results.inactive = False
                search_results.filter_attribute_id = 0
                search_results.results_as_ids = True

                search_results.search_text = search_param.cvalue.Text

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    search_results.db_read()
                Else
                    search_results.tmode = bc_cs_soap_base_class.tREAD
                    search_results.transmit_to_server_and_receive(search_results, True)
                End If

                Dim found_entities As New List(Of Long)
                For i = 0 To search_results.resultsids.Count - 1
                    found_entities.Add(search_results.resultsids(i))
                Next

                search_param.cvalue.Text = ""
                search_param.cvalue.Properties.Items.Clear()


                For i = 0 To search_lists(search_param.list_id).Count -1
                    If InStr(UCase(search_lists(search_param.list_id)(i).name), UCase(search_results.search_text)) > 0 Then
                        search_param.cvalue.Properties.Items.Add(search_lists(search_param.list_id)(i).name)
                    Else
                        For j = 0 To found_entities.Count - 1
                            If found_entities(j) = search_lists(search_param.list_id)(i).id Then
                                search_param.cvalue.Properties.Items.Add(search_lists(search_param.list_id)(i).name)
                                Exit For
                            End If
                        Next
                    End If
                Next

                If search_param.cvalue.Properties.Items.Count = 0 Then
                    search_param.cvalue.Text = search_results.search_text
                ElseIf search_param.cvalue.Properties.Items.Count = 1 Then
                    search_param.cvalue.SelectedIndex = 0
                Else
                    search_param.cvalue.ShowPopup()
                End If
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_rdp_function_wizard", "param_list_changed", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor = Windows.Forms.Cursors.Default


        End Try
    End Sub

    Public Sub write_address(param_name As String, address As String) Implements Ibc_dx_rdp_function_wizard.write_address


        For i = 0 To Controls.Count - 1
            If Controls(i).GetType.ToString = "BlueCurve.Insight.AM.dx_rtd_param" Then

                If Controls(i).Controls(1).Text = param_name Then

                    Controls(i).Controls(2).Text = address
                    Exit For
                End If

            End If

        Next
    End Sub


    Private Sub chkdim_SelectedIndexChanged(sender As Object, e As EventArgs) Handles chkdim.SelectedIndexChanged

        bup.Enabled = False
        bdn.Enabled = False

        If chkdim.SelectedIndex > 0 Then
            bup.Enabled = True
        End If
        If chkdim.SelectedIndex < chkdim.ItemCount - 1 Then
            bdn.Enabled = True
        End If
    End Sub

    Private Sub bup_Click(sender As Object, e As EventArgs) Handles bup.Click
        Dim vals As New List(Of String)
        Dim checked As New List(Of Boolean)
        Dim sel As Integer
        sel = chkdim.SelectedIndex


        For i = 0 To chkdim.Items.Count - 1
            vals.Add(chkdim.Items(i).Value)
            If chkdim.Items(i).CheckState = Windows.Forms.CheckState.Checked Then
                checked.Add(True)
            Else
                checked.Add(False)
            End If
        Next
        chkdim.Items.Clear()
        For i = 0 To vals.Count - 1
            If i = sel - 1 Then
                If checked(i + 1) = True Then
                    chkdim.Items.Add(vals(i + 1), True)
                Else
                    chkdim.Items.Add(vals(i + 1), False)
                End If

            ElseIf i = sel Then
                If checked(i - 1) = True Then
                    chkdim.Items.Add(vals(i - 1), True)
                Else
                    chkdim.Items.Add(vals(i - 1), False)
                End If
            Else
                If checked(i) = True Then
                    chkdim.Items.Add(vals(i), True)
                Else
                    chkdim.Items.Add(vals(i), False)

                End If
            End If
        Next
        chkdim.SelectedIndex = -1
        chkdim.SelectedIndex = sel - 1

    End Sub
    Private Sub bdn_Click(sender As Object, e As EventArgs) Handles bdn.Click
        Dim vals As New List(Of String)
        Dim checked As New List(Of Boolean)
        Dim sel As Integer
        sel = chkdim.SelectedIndex


        For i = 0 To chkdim.Items.Count - 1
            vals.Add(chkdim.Items(i).Value)
            If chkdim.Items(i).CheckState = Windows.Forms.CheckState.Checked Then
                checked.Add(True)
            Else
                checked.Add(False)
            End If
        Next
        chkdim.Items.Clear()
        For i = 0 To vals.Count - 1
            If i = sel Then
                If checked(i + 1) = True Then
                    chkdim.Items.Add(vals(i + 1), True)
                Else
                    chkdim.Items.Add(vals(i + 1), False)
                End If

            ElseIf i = sel + 1 Then
                If checked(i - 1) = True Then
                    chkdim.Items.Add(vals(i - 1), True)
                Else
                    chkdim.Items.Add(vals(i - 1), False)
                End If
            Else
                If checked(i) = True Then
                    chkdim.Items.Add(vals(i), True)
                Else
                    chkdim.Items.Add(vals(i), False)

                End If
            End If
        Next
        chkdim.SelectedIndex = -1
        chkdim.SelectedIndex = sel + 1

    End Sub
End Class
Public Class Cbc_dx_rdp_function_wizard
    WithEvents _view As Ibc_dx_rdp_function_wizard
    Dim _excelapp As Object
    Dim formulacell As Object
    Dim existingformula As String = ""
    Shared efs As New bc_om_rtd_functions
    Shared already_loaded As Boolean = False
    Dim _rtd_mode As Boolean

    Public Function load_data(view As Ibc_dx_rdp_function_wizard, excelapp As Object, rtd_mode As Boolean)
        Try

            _view = view
            _excelapp = excelapp
            _rtd_mode = rtd_mode
            formulacell = _excelapp.activecell
            load_data = False

            If is_bc_function_in_cell() = 2 Then
                Dim omsg As New bc_cs_message("Blue Curve", "Cell has a non Blue Curve RTD function in it please use another cell or remove this function.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If

            'formulacell = _oexcel.exceldoc.activecell
            If already_loaded = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    efs.db_read()
                Else
                    efs.tmode = bc_cs_soap_base_class.tREAD
                    If efs.transmit_to_server_and_receive(efs, True) = False Then
                        Exit Function
                    End If
                End If
            End If
            Dim success As Boolean
            success = view.load_view(efs, existingformula, rtd_mode)
            If success = True Then
                already_loaded = True
            End If
            Return success
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_rdp_function_wizard", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function
    Public Sub write_formula(formula As String, params As List(Of String), dimensions As List(Of String), is_list_function As Boolean, number_in_list As Integer, _rtd_mode As Boolean) Handles _view.write_formula
        Dim calc_type As Integer

        Try
            calc_type = _excelapp.calculation
            _excelapp.screenupdating = False

            _excelapp.calculation = -4135
            Dim fn As String
            If _rtd_mode = True Then
                fn = "=RTD(""bluecurvertd." + formula + ""","""""
                For i = 0 To params.Count - 1
                    If LTrim(RTrim(params(i)) = "") Then
                        fn = fn + ","
                    ElseIf is_formula(params(i)) = False Then
                        If Len(params(i)) > 2 AndAlso params(i).Substring(0, 1) = """" AndAlso params(i).Substring(params(i).Length - 1, 1) = """" Then
                            fn = fn + "," + params(i)
                        Else
                            fn = fn + ",""" + params(i) + """"
                        End If
                    Else
                        fn = fn + "," + params(i)
                    End If

                Next
            Else
                fn = "=" + formula + "("
                For i = 0 To params.Count - 1
                    If LTrim(RTrim(params(i)) = "") Then
                        If i > 0 Then
                            fn = fn + ","
                        End If
                    ElseIf is_formula(params(i)) = False Then
                        If i = 0 Then
                            If Len(params(i)) > 2 AndAlso params(i).Substring(0, 1) = """" AndAlso params(i).Substring(params(i).Length - 1, 1) = """" Then
                                fn = fn + params(i)
                            Else
                                fn = fn + """" + params(i) + """"
                            End If

                        Else
                            If Len(params(i)) > 2 AndAlso params(i).Substring(0, 1) = """" AndAlso params(i).Substring(params(i).Length - 1, 1) = """" Then
                                fn = fn + "," + params(i)
                            Else
                                fn = fn + ",""" + params(i) + """"
                            End If
                        End If
                    Else
                        If i = 0 Then
                            fn = fn + params(i)
                        Else
                            fn = fn + "," + params(i)
                        End If
                    End If

                Next
            End If


            
            If _rtd_mode = True Then
                REM now do for each dimension
                If is_list_function = False Then
                    If dimensions.Count = 0 Then
                        fn = fn + ")"
                        formulacell.formula = fn
                    Else
                        Dim dfn As String
                        For i = 0 To dimensions.Count - 1
                            dfn = fn + ",""" + CStr(dimensions(i)) + """)"
                            _excelapp.ActiveSheet.Cells(formulacell.Offset(0, 0).Row, formulacell.Offset(0, 0).Column + i).formula = dfn
                        Next
                    End If
                Else
                    'If dimensions.Count = 0 Then
                    '    fn = fn + ",""""1"""")"
                    '    formulacell.formula = fn
                    'Else
                    Dim dfn As String
                    '_excelapp.ActiveSheet.Cells(formulacell.Offset(0, 0).Row, formulacell.Offset(0, 0).Column + dimensions.Count).value = 1
                    Dim index_address As String
                    'Dim start_address
                    'start_address = _excelapp.ActiveSheet.Cells(formulacell.Offset(0, 0).Row, formulacell.Offset(0, 0).Column + dimensions.Count).address
                    Dim perc As Integer
                    For l = 0 To number_in_list - 1
                        perc = ((l + 1) / number_in_list) * 100
                        _view.set_status("setting " + CStr(perc) + "%...")
                        If l = 0 Then
                            _excelapp.ActiveSheet.Cells(formulacell.Offset(0, 0).Row, formulacell.Offset(0, 0).Column + dimensions.Count).value = 1

                        Else
                            _excelapp.ActiveSheet.Cells(formulacell.Offset(0, 0).Row + l, formulacell.Offset(0, 0).Column + dimensions.Count).formula = "=" + index_address + " + " + CStr(1)
                        End If
                        index_address = _excelapp.ActiveSheet.Cells(formulacell.Offset(0, 0).Row + l, formulacell.Offset(0, 0).Column + dimensions.Count).address
                        index_address = Replace(index_address, "$", "")
                        For i = 0 To dimensions.Count - 1
                            dfn = fn + ",""" + CStr(dimensions(i)) + """," + index_address + ")"

                            _excelapp.ActiveSheet.Cells(formulacell.Offset(0, 0).Row + l, formulacell.Offset(0, 0).Column + i).formula = dfn
                        Next
                        If (l = 0) Then
                            Thread.Sleep(2000)
                        End If


                    Next
                    'End If

                End If
            Else
                For i = 0 To dimensions.Count - 1
                    If i = 0 Then
                        fn = fn + ",""" + CStr(dimensions(i))
                    Else
                        fn = fn + "," + CStr(dimensions(i))
                    End If
                Next
                If dimensions.Count > 0 Then
                    fn = fn + """)"
                Else
                    fn = fn + ")"
                End If


                _excelapp.ActiveSheet.Cells(formulacell.Offset(0, 0).Row, formulacell.Offset(0, 0).Column).formula = fn

            End If
        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Cannot write formula: " + formula + Asc(13) + Asc(13) + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Finally
            _view.set_status("")

            _excelapp.calculation = calc_type

            _excelapp.visible = True
        End Try

    End Sub
    Public Sub set_address(param_name As String) Handles _view.set_address
        Try
            _view.write_address(param_name, _excelapp.activecell.address)
        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Cannot get cell address: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        End Try
    End Sub

    Private Function is_bc_function_in_cell() As Integer
        Try

            existingformula = formulacell.formula
            If _rtd_mode = True Then
                If InStr(existingformula, "=RTD(""bluecurvertd") > 0 Then
                    is_bc_function_in_cell = 1
                ElseIf existingformula <> "" Then
                    is_bc_function_in_cell = 2
                Else
                    is_bc_function_in_cell = 0
                End If
            Else
                If InStr(existingformula, "=bc_") > 0 Then
                    is_bc_function_in_cell = 1
                ElseIf existingformula <> "" Then
                    is_bc_function_in_cell = 2
                Else
                    is_bc_function_in_cell = 0
                End If


            End If
        Catch
            is_bc_function_in_cell = 0
        Finally

        End Try
    End Function



    Private Function is_formula(ByVal fm As String) As Boolean
        Try
            Dim i As Integer
            Dim now_numeric As Boolean = False
            Dim alphacount As Integer = 0
            is_formula = False
            REM if has a $ it is a formula
            If fm.Length < 2 Then
                Exit Function
            End If
            'Dim k As Integer
            If InStr(fm, "$") > 0 Then
                is_formula = True
                Exit Function
            End If

            REM if has alpha chracter followed by numeric it is as well
            REM first character must be alpha
            REM FIL MAY 2013 only allow up 2 to alphas before a numeric else any word
            REM that ends in a numeric will pass
            If (Asc(fm.Substring(0, 1)) >= Asc("a") And Asc(fm.Substring(0, 1)) <= Asc("z")) Or (Asc(fm.Substring(0, 1)) >= Asc("A") And Asc(fm.Substring(0, 1)) <= Asc("Z")) Then
                alphacount = 1
                For i = 1 To fm.Length - 1
                    If (Asc(fm.Substring(i, 1)) >= Asc("a") And Asc(fm.Substring(i, 1)) <= Asc("z")) Or (Asc(fm.Substring(i, 1)) >= Asc("A") And Asc(fm.Substring(i, 1)) <= Asc("Z")) Then
                        alphacount = alphacount + 1
                        If alphacount > 2 Then
                            Exit Function
                        End If
                        'k = 4
                        If now_numeric = True Then
                            Exit Function
                        End If
                    ElseIf IsNumeric(fm.Substring(i, 1)) = True Then
                        REM must now be numeric 
                        now_numeric = True
                        If i = fm.Length - 1 Then
                            is_formula = True
                            Exit Function
                        End If
                    Else
                        Exit Function
                    End If
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ef_wizard", "is_formula", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            REM FIL JULY 2013 
            REM double check valid formula by pasting in excel cell
            If is_formula = True Then
                If test_valid_excel_address(fm) = False Then
                    is_formula = False
                End If
            End If
        End Try
    End Function
    Private Function test_valid_excel_address(ByVal fm As String) As Boolean
        Dim ofm As String
        ofm = formulacell.formula
        fm = "=" + fm
        test_valid_excel_address = True
        Try
            formulacell.formula = fm
        Catch ex As Exception
            test_valid_excel_address = False
        Finally
            formulacell.formula = ofm
        End Try
    End Function
End Class
Public Interface Ibc_dx_rdp_function_wizard
    Function load_view(efs As bc_om_rtd_functions, existingformula As String, rtd_mode As Boolean) As Boolean
    Event write_formula(formula As String, params As List(Of String), dimensions As List(Of String), is_list_function As Boolean, number_in_list As Integer, _rtd_mode As Boolean)
    Event set_address(param_name As String)
    Sub write_address(param_name As String, address As String)
    Sub set_status(text As String)
End Interface