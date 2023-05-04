
Imports BlueCurve.Core.CS

Imports BlueCurve.Core.OM
Public Class bc_am_cp_distribution
    Implements Ibc_am_cp_distribution
    Dim _cf As bc_om_pt_dist_config
    Public Function load_view(cf As bc_om_pt_dist_config, pub_type_name As String) Implements Ibc_am_cp_distribution.load_view
        Dim found = False
        REM channels
        Try
            _cf = cf
            load_view = True
            'lsel.SortOrder = Windows.Forms.SortOrder.None

            Me.Text = "Distribution Configuration for: " + pub_type_name
            Me.cproducttype.Properties.Items.Clear()

            For i = 0 To _cf.all_product_types.Count - 1
                Me.cproducttype.Properties.Items.Add(_cf.all_product_types(i).name)
                If _cf.sel_product_type = _cf.all_product_types(i).id Then
                    Me.cproducttype.SelectedIndex = i
                End If
            Next

            If _cf.all_products.Count = 0 Then
                Me.XtraTabPage3.PageEnabled = False
            Else

                For i = 0 To _cf.all_products.Count - 1
                    found = False
                    For j = 0 To _cf.sel_products.Count - 1
                        If _cf.all_products(i).id = _cf.sel_products(j) Then
                            Me.chkmodules.Items.Add(_cf.all_products(i).name, True)
                            found = True
                            Exit For
                        End If
                    Next
                    If found = False Then
                        Me.chkmodules.Items.Add(_cf.all_products(i).name, False)
                    End If
                Next
            End If

            If _cf.automatic = True Then
                Me.rauto.SelectedIndex = 0
            Else
                Me.rauto.SelectedIndex = 1
            End If
            Me.cmaxnum.Properties.Items.Add("No Restriction")
            For i = 1 To 20
                Me.cmaxnum.Properties.Items.Add(CStr(i))
            Next
            Me.cmaxnum.SelectedIndex = 0

            Me.cdepclass.Properties.Items.Add("None")

            Me.cdepclass.SelectedIndex = 0

            For i = 0 To _cf.all_channels.Count - 1

                found = False
                For j = 0 To _cf.sel_channels.Count - 1
                    If _cf.all_channels(i).channel_id = _cf.sel_channels(j) Then
                        Me.chkchannels.Items.Add(_cf.all_channels(i).channel_name, True)
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    Me.chkchannels.Items.Add(_cf.all_channels(i).channel_name, False)
                End If
            Next

            REM templates

            Me.cbodytemplate.Properties.Items.Add("None")
            Me.cbodytemplate.SelectedIndex = 0
            For i = 0 To _cf.email_templates.Count - 1
                Me.cbodytemplate.Properties.Items.Add(_cf.email_templates(i).html_filename)
                If _cf.email_templates(i).email_template_id = _cf.sel_body_email_template Then
                    Me.cbodytemplate.SelectedIndex = i + 1
                End If
            Next
            REM extended taxonomy

            load_tax()
            load_attributes()
            load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_cp_distribution", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Sub load_attributes()

        Dx_uc_attributes1.attributes = _cf.attributes.attributes
        Dim av As bc_om_attribute_value
        Dx_uc_attributes1.values.Clear()
        For i = 0 To Dx_uc_attributes1.attributes.Count - 1
            av = New bc_om_attribute_value
            av.value = Dx_uc_attributes1.attributes(i).default_value
            Dx_uc_attributes1.values.Add(av)
        Next
        Dx_uc_attributes1.load_data()
    End Sub

    '    Try
    '        Dim tag As String = ""
    '        Me.uxatt.BeginUpdate()
    '        For i = 0 To _cf.attributes.attributes.Count - 1
    '            uxatt.Nodes.Add()
    '            uxatt.Nodes(i).SetValue(0, _cf.attributes.attributes(i).name)

    '            Me.uxatt.Nodes(i).Tag = _cf.attributes.attributes(i).attribute_id
    '            If i = 0 Then
    '                tag = Me.uxatt.Nodes(i).Tag
    '            End If
    '            If _cf.attributes.attributes(i).is_lookup = False Then
    '                If _cf.attributes.attributes(i).type_id = 1 Or _cf.attributes.attributes(i).type_id = 2 Or _cf.attributes.attributes(i).type_id = 5 Then
    '                    Me.uxatt.Nodes(i).SetValue(1, _cf.attributes.attributes(i).default_value)
    '                ElseIf _cf.attributes.attributes(i).type_id = 3 Then
    '                    If _cf.attributes.attributes(i).default_value = "1" Then
    '                        Me.uxatt.Nodes(i).SetValue(1, "true")
    '                    Else
    '                        _cf.attributes.attributes(i).default_value = "0"
    '                        Me.uxatt.Nodes(i).SetValue(1, "false")
    '                    End If
    '                End If
    '            Else
    '                For j = 0 To _cf.attributes.attributes(i).lookup_keys.Count - 1
    '                    If IsNumeric(_cf.attributes.attributes(i).default_value) Then
    '                        If _cf.attributes.attributes(i).lookup_keys(j) = _cf.attributes.attributes(i).default_value Then
    '                            Me.uxatt.Nodes(i).SetValue(1, _cf.attributes.attributes(i).lookup_values(j))
    '                            Exit For
    '                        End If
    '                    End If
    '                Next
    '            End If

    '        Next

    '        If tag <> "" Then
    '            set_list(tag)
    '        End If

    '    Catch ex As Exception
    '        Dim oerr As New bc_cs_error_log("bc_om_cp_distribution", "load_attributes", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        Me.uxatt.EndUpdate()
    '    End Try
    'End Sub
    Sub reset_class_details()
        Me.chkdefault.Checked = False
        Me.chkmandatory.Checked = False
        Me.cdepclass.Properties.Items.Clear()
        Me.cdepclass.Properties.Items.Add("None")
        Me.cdepclass.SelectedIndex = 0
        Me.cmaxnum.SelectedIndex = 0
    End Sub

    Event save(cf As bc_om_pt_dist_config) Implements Ibc_am_cp_distribution.save



    Private Sub load_tax()
        Try
            Dim found As Boolean
            Me.lall.Items.Clear()
            Me.lsel.Items.Clear()
            Me.ptaxsettings.Enabled = False

            For i = 0 To _cf.all_classes.Count - 1

                found = False
                For j = 0 To _cf.sel_classes.Count - 1

                    If _cf.all_classes(i).class_id = _cf.sel_classes(j).class_id Then

                        'Me.lsel.Items.Add(_cf.all_classes(i).class_name)

                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    Me.lall.Items.Add(_cf.all_classes(i).class_name)
                End If
            Next
            For j = 0 To _cf.sel_classes.Count - 1
                For i = 0 To _cf.all_classes.Count - 1
                    If _cf.all_classes(i).class_id = _cf.sel_classes(j).class_id Then
                        Me.lsel.Items.Add(_cf.all_classes(i).class_name)
                        Exit For
                    End If
                Next
            Next
            Me.bup.Enabled = False
            Me.bdn.Enabled = False
            Me.lsel.SelectedIndex = -1
            If Me.lsel.Items.Count > 0 Then
                Me.lsel.SelectedIndex = 0
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_cp_distribution", "load_tax", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub


    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub



    Private Sub bsave_Click(sender As Object, e As EventArgs) Handles bsave.Click
        Try

            If cproducttype.SelectedIndex = -1 Then
                Dim omsg As New bc_cs_message("Blue Curve", "Product Type must be selected.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            _cf.sel_product_type = _cf.all_product_types(Me.cproducttype.SelectedIndex).id

            _cf.sel_products.Clear()
            For i = 0 To Me.chkmodules.Items.Count - 1
                If Me.chkmodules.Items(i).CheckState = Windows.Forms.CheckState.Checked Then
                    _cf.sel_products.Add(_cf.all_products(i).id)
                End If
            Next

            _cf.sel_channels.Clear()
            For i = 0 To Me.chkchannels.Items.Count - 1
                If Me.chkchannels.Items(i).CheckState = Windows.Forms.CheckState.Checked Then
                    _cf.sel_channels.Add(_cf.all_channels(i).channel_id)
                End If
            Next
            _cf.sel_body_email_template = 0
            _cf.sel_title_email_template = 0

            If Me.cbodytemplate.SelectedIndex > 0 Then
                _cf.sel_body_email_template = _cf.email_templates(Me.cbodytemplate.SelectedIndex - 1).email_template_id
            End If
            _cf.automatic = False
            If Me.rauto.SelectedIndex = 0 Then
                _cf.automatic = True
            End If
            REM check attributes
            Dim attribute_errors As String = ""
            For i = 0 To _cf.attributes.attributes.Count - 1
                _cf.attributes.attributes(i).default_value = Dx_uc_attributes1.values(i).value

                If _cf.attributes.attributes(i).type_id = 5 AndAlso _cf.attributes.attributes(i).default_value <> "" AndAlso IsDate(_cf.attributes.attributes(i).default_value) = False Then
                    attribute_errors = attribute_errors + _cf.attributes.attributes(i).name + ": must be a date" + vbCrLf
                End If
                If _cf.attributes.attributes(i).type_id = 2 AndAlso _cf.attributes.attributes(i).default_value <> "" AndAlso IsNumeric(_cf.attributes.attributes(i).default_value) = False Then
                    attribute_errors = attribute_errors + _cf.attributes.attributes(i).name + ": must be a number" + vbCrLf
                End If
                If _cf.attributes.attributes(i).nullable = False AndAlso _cf.attributes.attributes(i).default_value = "" Then
                    attribute_errors = attribute_errors + _cf.attributes.attributes(i).name + ": must be entered" + vbCrLf
                End If
            Next
            If attribute_errors <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "Attribute(s) " + vbCrLf + attribute_errors, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If


            Me.Cursor = Windows.Forms.Cursors.WaitCursor
            RaiseEvent save(_cf)
            Me.Hide()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_pt_dist_config", "save", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub lsel_DoubleClick(sender As Object, e As EventArgs) Handles lsel.DoubleClick

        Me._cf.sel_classes.RemoveAt(lsel.SelectedIndex)
        load_tax()
    End Sub

    Private Sub lsel_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lsel.SelectedIndexChanged
        Me.bup.Enabled = False
        Me.bdn.Enabled = False

        If lsel.SelectedIndex = -1 Then
            Exit Sub
        End If

        If lsel.SelectedIndex > 0 Then
            Me.bup.Enabled = True
        End If
        If lsel.SelectedIndex < Me.lsel.Items.Count - 1 Then
            Me.bdn.Enabled = True
        End If

        chkmandatory.Checked = _cf.sel_classes(Me.lsel.SelectedIndex).mandatory
        cmaxnum.SelectedIndex = _cf.sel_classes(Me.lsel.SelectedIndex).max_num

        chkdefault.Checked = _cf.sel_classes(Me.lsel.SelectedIndex).has_default_value

        Me.cdepclass.Properties.Items.Clear()
        Me.cdepclass.Properties.Items.Add("None")
        Me.cdepclass.SelectedIndex = 0




        For i = 0 To lsel.Items.Count - 1
            If lsel.Items(i) <> Me.lsel.Text Then
                Me.cdepclass.Properties.Items.Add(lsel.Items(i))
            End If
        Next

        For j = 0 To _cf.all_classes.Count - 1
            If _cf.all_classes(j).class_id = _cf.sel_classes(Me.lsel.SelectedIndex).dependent_class_id Then
                For k = 0 To Me.cdepclass.Properties.Items.Count - 1
                    If Me.cdepclass.Properties.Items(k) = _cf.all_classes(j).class_name Then
                        Me.cdepclass.SelectedIndex = k
                        Exit For
                    End If
                Next
            End If
        Next





        Me.ptaxsettings.Enabled = True
    End Sub

    Private Sub chkmandatory_CheckedChanged(sender As Object, e As EventArgs) Handles chkmandatory.CheckedChanged

        _cf.sel_classes(Me.lsel.SelectedIndex).mandatory = chkmandatory.Checked
    End Sub

    Private Sub chkdefault_CheckedChanged(sender As Object, e As EventArgs) Handles chkdefault.CheckedChanged
        If Me.lsel.SelectedIndex = -1 Then
            Exit Sub
        End If
        _cf.sel_classes(Me.lsel.SelectedIndex).has_default_value = chkdefault.Checked
    End Sub

    Private Sub cdepclass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cdepclass.SelectedIndexChanged
        If Me.lsel.SelectedIndex = -1 Then
            Exit Sub
        End If
        'If Me.cdepclass.SelectedIndex = 0 Then
        '    _cf.sel_classes(Me.lsel.SelectedIndex).dependent_class_id = 0
        'Else
        For i = 0 To _cf.all_classes.Count - 1
            If _cf.all_classes(i).class_name = Me.cdepclass.Text Then

                _cf.sel_classes(Me.lsel.SelectedIndex).dependent_class_id = _cf.all_classes(i).class_id
                Exit For
            End If
        Next
        'End If
    End Sub

    Private Sub lall_DoubleClick(sender As Object, e As EventArgs) Handles lall.DoubleClick
        Dim ns As New bc_om_pt_dist_config.bc_om_extended_taxonomy
        For i = 0 To _cf.all_classes.Count - 1
            If _cf.all_classes(i).class_name = Me.lall.Text Then
                ns.class_id = _cf.all_classes(i).class_id
                ns.class_name = _cf.all_classes(i).class_name
                _cf.sel_classes.Add(ns)
                Exit For
            End If
        Next
        load_tax()
    End Sub

    Private Sub lsel_SelectedIndexChanged_1(sender As Object, e As EventArgs) Handles lsel.SelectedIndexChanged

    End Sub

    Private Sub bup_Click(sender As Object, e As EventArgs) Handles bup.Click
        Dim tselclass As New bc_om_pt_dist_config.bc_om_extended_taxonomy
        Dim s As Integer
        s = Me.lsel.SelectedIndex
        tselclass = _cf.sel_classes(s)
        _cf.sel_classes.RemoveAt(s)
        _cf.sel_classes.Insert(s - 1, tselclass)

        load_tax()

        Me.lsel.SelectedIndex = s - 1
    End Sub
    Private Sub bdn_Click(sender As Object, e As EventArgs) Handles bdn.Click
        Dim tselclass As New bc_om_pt_dist_config.bc_om_extended_taxonomy
        Dim s As Integer
        s = Me.lsel.SelectedIndex
        tselclass = _cf.sel_classes(s)
        _cf.sel_classes.RemoveAt(s)
        _cf.sel_classes.Insert(s + 1, tselclass)

        load_tax()
        Me.lsel.SelectedIndex = s + 1
    End Sub

    Private Sub cmaxnum_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cmaxnum.SelectedIndexChanged
        If Me.lsel.SelectedIndex = -1 Or cmaxnum.SelectedIndex = -1 Then
            Exit Sub
        End If
        _cf.sel_classes(Me.lsel.SelectedIndex).max_num = cmaxnum.SelectedIndex
    End Sub



    'Private Sub uxatt_FocusedNodeChanged(sender As Object, e As DevExpress.XtraTreeList.FocusedNodeChangedEventArgs)
    'set_list(e.Node.Tag)
    'Me.RepositoryItemComboBox1.Items.Clear()
    'For i = 0 To _cf.attributes.attributes.Count - 1
    '    If _cf.attributes.attributes(i).attribute_id = e.Node.Tag Then
    '        For j = 0 To _cf.attributes.attributes(i).lookup_values.Count - 1
    '            Me.RepositoryItemComboBox1.Items.Add(_cf.attributes.attributes(i).lookup_values(j))
    '        Next
    '        For j = 0 To _cf.attributes.attributes(i).lookup_keys.Count - 1
    '            If IsNumeric(_cf.attributes.attributes(i).default_value) Then
    '                If _cf.attributes.attributes(i).lookup_keys(j) = _cf.attributes.attributes(i).default_value Then
    '                    Me.uxatt.Nodes(i).SetValue(1, _cf.attributes.attributes(i).lookup_values(j))
    '                    Exit For
    '                End If
    '            End If
    '        Next
    '        Exit For
    '    End If
    'Next
    'End Sub
    'Private Sub set_list(tag As String)
    '    Me.RepositoryItemComboBox1.Items.Clear()
    '    '1:      Text
    '    '3:      bool()
    '    '0:      mum()
    '    '  2 date
    '    Me.RepositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.DisableTextEditor



    '    For i = 0 To _cf.attributes.attributes.Count - 1
    '        If _cf.attributes.attributes(i).attribute_id = tag Then

    '            REM non lookup
    '            If _cf.attributes.attributes(i).is_lookup = False Then
    '                If _cf.attributes.attributes(i).type_id = 1 Or _cf.attributes.attributes(i).type_id = 2 Or _cf.attributes.attributes(i).type_id = 5 Then
    '                    Me.RepositoryItemComboBox1.TextEditStyle = DevExpress.XtraEditors.Controls.TextEditStyles.Standard
    '                    Me.uxatt.Nodes(i).SetValue(1, _cf.attributes.attributes(i).default_value)
    '                ElseIf _cf.attributes.attributes(i).type_id = 3 Then
    '                    Me.RepositoryItemComboBox1.Items.Add("true")
    '                    Me.RepositoryItemComboBox1.Items.Add("false")
    '                    If _cf.attributes.attributes(i).default_value = 1 Then
    '                        Me.uxatt.Nodes(i).SetValue(1, "true")
    '                    Else
    '                        Me.uxatt.Nodes(i).SetValue(1, "false")
    '                    End If
    '                End If

    '            Else

    '                For j = 0 To _cf.attributes.attributes(i).lookup_values.Count - 1
    '                    Me.RepositoryItemComboBox1.Items.Add(_cf.attributes.attributes(i).lookup_values(j))
    '                Next
    '                For j = 0 To _cf.attributes.attributes(i).lookup_keys.Count - 1
    '                    If IsNumeric(_cf.attributes.attributes(i).default_value) Then
    '                        If _cf.attributes.attributes(i).lookup_keys(j) = _cf.attributes.attributes(i).default_value Then
    '                            Me.uxatt.Nodes(i).SetValue(1, _cf.attributes.attributes(i).lookup_values(j))
    '                            Exit For
    '                        End If
    '                    End If
    '                Next
    '            End If
    '            Exit For
    '        End If
    '    Next
    'End Sub

    'Private Sub RepositoryItemComboBox1_SelectedIndexChanged(sender As Object, e As EventArgs)


    '    Dim s As DevExpress.XtraEditors.ComboBoxEdit
    '    s = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
    '    Dim ev As String
    '    ev = s.EditValue
    '    For i = 0 To _cf.attributes.attributes.Count - 1
    '        If _cf.attributes.attributes(i).attribute_id = uxatt.Selection(0).Tag Then




    '            REM store key
    '            For j = 0 To _cf.attributes.attributes(i).lookup_values.Count - 1
    '                If _cf.attributes.attributes(i).lookup_values(j) = ev Then
    '                    _cf.attributes.attributes(i).default_value = _cf.attributes.attributes(i).lookup_keys(j)
    '                    Exit For
    '                End If
    '            Next
    '            Exit For
    '        End If
    '    Next
    'End Sub

    'Private Sub RepositoryItemComboBox1_SelectedValueChanged(sender As Object, e As EventArgs)
    '    REM validate data type if  number or date
    '    Dim s As DevExpress.XtraEditors.ComboBoxEdit
    '    s = DirectCast(sender, DevExpress.XtraEditors.ComboBoxEdit)
    '    Dim ev As String
    '    ev = s.EditValue
    '    For i = 0 To _cf.attributes.attributes.Count - 1
    '        If _cf.attributes.attributes(i).attribute_id = uxatt.Selection(0).Tag Then
    '            If _cf.attributes.attributes(i).is_lookup = False Then
    '                If _cf.attributes.attributes(i).type_id = 3 Then
    '                    _cf.attributes.attributes(i).default_value = 0
    '                    If ev = "true" Then
    '                        _cf.attributes.attributes(i).default_value = 1
    '                    End If
    '                Else
    '                    _cf.attributes.attributes(i).default_value = ev
    '                End If
    '            End If
    '            Exit For
    '        End If
    '    Next
    'End Sub

    Private Sub PanelControl3_Paint(sender As Object, e As Windows.Forms.PaintEventArgs) Handles PanelControl3.Paint

    End Sub

    Private Sub Dx_uc_attributes1_fire_warnings(warnings As List(Of String)) Handles Dx_uc_attributes1.fire_warnings
        'MsgBox(warnings(0))
    End Sub
End Class
Public Class Cbc_am_cp_distribution
    Dim _pub_type_name As String
    Dim _pub_type_id As Long
    WithEvents _view As Ibc_am_cp_distribution
    Public Sub New(view As Ibc_am_cp_distribution, pub_type_id As Long, pub_type_name As String)
        _view = view
        _pub_type_id = pub_type_id

        _pub_type_name = pub_type_name

    End Sub

    Public Function load_data() As Boolean
        Try
            Dim cf As New bc_om_pt_dist_config
            cf.pub_type_id = _pub_type_id

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                cf.db_read()

            Else
                cf.tmode = bc_cs_soap_base_class.tREAD
                If cf.transmit_to_server_and_receive(cf, True) = False Then
                    Exit Function
                End If
            End If
            load_data = _view.load_view(cf, _pub_type_name)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_om_pt_dist_config", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Public Sub save(cf As bc_om_pt_dist_config) Handles _view.save
        Try
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                cf.db_write()
            Else
                cf.tmode = bc_cs_soap_base_class.tWRITE
                If cf.transmit_to_server_and_receive(cf, True) = False Then
                    Exit Sub
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_om_pt_dist_config", "save", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

End Class

Public Interface Ibc_am_cp_distribution
    Function load_view(cf As bc_om_pt_dist_config, pub_type_name As String)
    Event save(cf As bc_om_pt_dist_config)
End Interface
