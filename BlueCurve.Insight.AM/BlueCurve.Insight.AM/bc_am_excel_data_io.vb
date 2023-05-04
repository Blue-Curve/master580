Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Create.AM
Imports System.Xml
Imports System.Windows.Forms
Public Class bc_am_excel_data_io
    Public ao_object As Object
    Public ao_type As String
    Public ao_excel As bc_ao_in_excel
    Public templates As New bc_in_excel_io_templates


    Public Sub New(ByVal ao_object As Object, ByVal ao_type As String)
        Me.ao_object = ao_object
        Me.ao_type = ao_type
    End Sub
    REM interface where template is passed in
    Public Sub show_errors(ByVal oerrors As bc_om_excel_data_element_errors, Optional ByVal showAffectedRowCount As Boolean = True)
        Try
            REM firstly check success count
            Dim ins_count As Integer = 0
            For i = 0 To oerrors.errors.Count - 1
                If oerrors.errors(i).type = "success" Then
                    ins_count = ins_count + oerrors.errors(i).col
                End If
            Next
            ao_excel = New bc_ao_in_excel(ao_object)
            Dim ofrm As New bc_am_excel_data_load
            ofrm.pwarnings.Visible = True
            ofrm.ptemplate.Visible = False
            Dim lvw As ListViewItem
            For i = 0 To oerrors.errors.Count - 1
                If oerrors.errors(i).type <> "success" Then
                    lvw = New ListViewItem(CStr(oerrors.errors(i).Worksheet))
                    lvw.SubItems.Add(CStr(oerrors.errors(i).type))
                    If oerrors.errors(i).row = 0 Then
                        lvw.SubItems.Add("n/a")
                        lvw.SubItems.Add("n/a")
                    Else
                        lvw.SubItems.Add(CStr(oerrors.errors(i).row))
                        lvw.SubItems.Add(CStr(oerrors.errors(i).col))
                    End If
                    lvw.SubItems.Add(oerrors.errors(i).err_tx)
                    ofrm.Lvwarnings.Items.Add(lvw)
                End If
            Next
            If ofrm.Lvwarnings.Items.Count > 0 Then
                ofrm.ao_excel = Me.ao_excel
                Dim oapi As New API
                'API.SetWindowPos(ofrm.Handle.ToInt32, API.HWND_TOPMOST, 10, 10, 1, 1, 1)
                ofrm.Show()
                ofrm.TopMost = True
            Else
                Dim ocomm As New bc_cs_activity_log("bc_am_excel_data_io", "show_errors", bc_cs_activity_codes.COMMENTARY, "Data upload successful: " + CStr(ins_count) + " rows inserted")
                If showAffectedRowCount Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Data upload successful: " + CStr(ins_count) + " rows inserted", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Else
                    Dim omsg As New bc_cs_message("Blue Curve", "Data upload successful", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                End If
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_excel_data_io", "show_errors", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Public Function run_template(ByVal name As String, ByRef errors As bc_om_excel_data_element_errors, Optional ByVal batch_mode As Boolean = False, Optional ByVal calc As Boolean = False) As Boolean
        Try
            run_template = False
            If UCase(ao_type) <> UCase(bc_ao_in_object.EXCEL_DOC) Then
                Dim omessage As New bc_cs_message("Blue Curve insight", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            If templates.load = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Failed to load or read config file please check activity file.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            If templates.templates.Count = 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "No i/o templates configured in config file", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            Dim found As Boolean = False
            For i = 0 To templates.templates.Count - 1
                If UCase(templates.templates(i).name) = UCase(name) Then
                    If calc = False Then
                        templates.templates(i).calc = 0
                    End If
                    execute_template(templates.templates(i), errors, batch_mode)
                    found = True
                    run_template = True
                    Exit For
                End If
            Next
            If found = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "No template found with name: " + name, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_excel_data_io", "run_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    REM interface where template is selected
    Public Sub show_template_list(byref errors As bc_om_excel_data_element_errors)
        Try
            If UCase(ao_type) <> UCase(bc_ao_in_object.EXCEL_DOC) Then
                Dim omessage As New bc_cs_message("Blue Curve insight", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            If templates.load = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Failed to load or read config file please check activity file.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            If templates.templates.Count = 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "No i/o templates configured in config file", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            Dim ofrm As New bc_am_excel_data_load
            ofrm.Width = 333
            ofrm.Height = 244
            ofrm.ptemplate.Visible = True
            ofrm.pwarnings.Visible = False
            For i = 0 To templates.templates.Count - 1
                ofrm.Ltemplate.Items.Add(templates.templates(i).name)
            Next
            ofrm.templates = templates
            ofrm.ShowDialog()
            If ofrm.cancel_selected = True Then
                Exit Sub
            End If
            If ofrm.Ltemplate.SelectedIndex > -1 Then
                execute_template(templates.templates(ofrm.Ltemplate.SelectedIndex), errors)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_excel_data_io", "show_template_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Function execute_template(ByVal template As bc_in_excel_io_template, Optional ByRef oerrors As bc_om_excel_data_element_errors = Nothing, Optional ByVal batch_mode As Boolean = False) As Boolean
        Try
            ao_excel = New bc_ao_in_excel(ao_object)
            REM check if input or output
            Dim prop_entity_name As String = ""
            REM firstly get entity of propgating class
            Select Case template.read_entity_mode
                Case "entity name"
                    prop_entity_name = template.entity_name
                Case "row col"
                    prop_entity_name = ao_excel.get_cell_value(template.entity_name_row_id, template.entity_name_col_id)
                Case "sheet name"
                    prop_entity_name = ao_excel.get_sheet_name_from_tab
                Case Else
                    prop_entity_name = "na"
            End Select
            If template.io = "input" Then
                If prop_entity_name = "" Then
                    Dim omsg As New bc_cs_message("Blue Curve", "No propogating entity name", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
                Dim errors As New bc_om_excel_data_element_errors
                Dim oelements As New bc_om_excel_data_elements
                template.entity_name = prop_entity_name
                ao_excel.read_io_data(template, oelements, errors)

                If errors.errors.Count = 0 Then
                    REM if time series reverse the order

                    If template.io_mode = "time series" Or template.io_mode = "period time series" Then
                        Dim telements As New ArrayList
                        For i = 0 To oelements.elements.Count - 1
                            telements.Add(oelements.elements(i))
                        Next
                        oelements.elements.Clear()

                        For i = 0 To telements.Count - 1
                            oelements.elements.Add(telements(telements.Count - 1 - i))
                        Next

                    End If

                    oelements.calc = template.calc
                    oelements.draft = template.draft
                    oelements.publish = template.publish
                    oelements.class_name = template.class_name
                    oelements.io_type = template.io_mode
                    oelements.worksheet = ao_excel.get_sheet_name_from_tab
                    oelements.template = template.name
                    oelements.errors.errors.Clear()
                    If template.date_to_row = 0 Or template.date_to_col = 0 Then
                        oelements.date_to = "9-9-9999"
                    Else
                        Try
                            oelements.date_to = ao_excel.get_cell_value(template.date_to_row, template.date_to_col)
                        Catch
                            oelements.date_to = "9-9-9999"
                        End Try
                    End If

                    If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                        oelements.db_write()
                    ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                        oelements.tmode = bc_cs_soap_base_class.tWRITE
                        If oelements.transmit_to_server_and_receive(oelements, True) = False Then
                            Exit Function
                        End If
                    Else
                        Exit Function
                    End If
                    'oelements.db_write()

                End If
                    REM add errors to glonal object
                    For i = 0 To errors.errors.Count - 1
                        oerrors.errors.Add(errors.errors(i))
                    Next
                    For i = 0 To oelements.errors.errors.Count - 1
                        oerrors.errors.Add(oelements.errors.errors(i))
                    Next
                    If batch_mode = False Then
                        show_errors(oerrors)
                    End If
                Else
                    REM need to set up values for data extarction depending upon io_mode etc
                    REM the back end will use excel functions to actually get the data
                    If template.io_mode = "propogating" Then
                        If prop_entity_name = "" Then
                            Dim omsg As New bc_cs_message("Blue Curve", "No propogating entity name", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Function
                        End If
                        Dim oef As New bc_io_excel_functions
                        Dim res As String = ""
                        REM first see if all entities of a class
                        If template.propogating_class = template.class_name Then
                            res = oef.bc_get_entities_for_class(template.class_name)
                        Else
                            res = oef.load_propogating_entities(5006, template.propogating_class, prop_entity_name, template.class_name, 1)
                            If res = "" Then
                                res = oef.load_propogating_entities(5005, template.propogating_class, prop_entity_name, template.class_name, 1)
                            End If
                        End If
                        If res.Length > 4 Then
                            If res.Substring(0, 4) = "err:" Then
                                Dim omsg As New bc_cs_message("Blue Curve", "Error retrieving propgating entity list: " + res, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                                Exit Function
                            End If
                        End If
                        If res.Length > 0 Then
                            REM now need to create elemenst from result set
                            REM parse the results and excuete item values
                            Dim out_elements As bc_io_output_elements
                            out_elements = parse_results(res, template)
                            REM write values into excel
                            ao_excel.write_io_data(out_elements)
                        Else
                            Dim osg As New bc_cs_message("Blue Curve", "No propogating entities retrieved", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Function
                        End If
                ElseIf template.io_mode = "current" Then
                    If prop_entity_name = "" Then
                        Dim omsg As New bc_cs_message("Blue Curve", "No entity name", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Function
                    End If
                    Dim out_elements As New bc_io_output_elements
                    Dim out_element As New bc_io_output_element
                    out_element.value = prop_entity_name
                    out_element.col = 0
                    out_elements.elements.Add(out_element)
                    retrieve_item_values(out_elements, template.entity_name_row_id, template)
                    REM write values into excel
                    ao_excel.write_io_data(out_elements)

                ElseIf template.io_mode = "period time series" Then
                    If prop_entity_name = "" Then
                        Dim omsg As New bc_cs_message("Blue Curve", "No entity name", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Function
                    End If
                    Dim out_elements As New bc_io_output_elements
                    Dim out_element As New bc_io_output_element

                    Dim oef As New bc_io_excel_functions
                    Dim res As String
                    Dim stage As String
                    Dim lrc As Integer
                    Dim lcol As Integer = 0


                    If template.publish = True Then
                        stage = "publish"
                    Else
                        stage = "draft"
                    End If
                    Dim date_from As String = "1-1-1900"
                    Dim date_to As String = "9-9-9999"

                    Dim val As String
                    If template.date_from_col <> 0 And template.date_from_row <> 0 Then
                        val = ao_excel.get_cell_value(template.date_from_row, template.date_from_col)
                        date_from = val
                        If IsDate(val) Then
                            date_from = Format("dd-mmm-yyyy", date_from)
                        Else
                            Dim omsg As New bc_cs_message("bc_am_excel_data_io", "Incorrect date format: " + date_from, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Function
                        End If
                    End If
                    If template.date_to_col <> 0 And template.date_to_row <> 0 Then
                        val = ao_excel.get_cell_value(template.date_to_row, template.date_to_col)
                        date_to = val
                        If IsDate(val) Then
                            date_to = Format("dd-mmm-yyyy", date_to)
                        Else
                            Dim omsg As New bc_cs_message("bc_am_excel_data_io", "Incorrect date format: " + date_to, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Function
                        End If
                    End If


                    REM Get Period headers from database
                    Dim periodheaders As New bc_om_period_headers
                    periodheaders.entityname = prop_entity_name
                    If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                        periodheaders.db_read()
                    ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                        periodheaders.tmode = bc_cs_soap_base_class.tREAD
                        If periodheaders.transmit_to_server_and_receive(periodheaders, True) = False Then
                            Exit Function
                        End If
                    Else
                        Exit Function
                    End If

                    'out_element.value = prop_entity_name
                    'out_element.col = 0
                    'out_elements.elements.Add(out_element)

                    REM write headers into excel
                    'ao_excel.exceldoc.ActiveSheet.Next.Select()
                    ao_excel.write_io_periodheaders(periodheaders, prop_entity_name, template.items(0).item_identifier)

                    lrc = 0
                    lcol = 0
                    Dim prev_year As Integer = 0
                    Dim prev_period As String = ""
                    REM write data into excel
                    For i = 0 To periodheaders.periodheadings.Count - 1

                        res = oef.bc_get_historic_financial_data(template.class_name, prop_entity_name, template.items(0).item_identifier, stage, template.items(0).contributor, date_from, date_to, "", periodheaders.periodheadings(i).periodyear, periodheaders.periodheadings(i).periodname, "e_a_flag,date_from,value")
                        Dim ostr As String
                        Dim dim_toggle As Integer
                        Dim ignore As Boolean = False
                        dim_toggle = 1
                        ostr = res
                        Dim row As Integer
                        row = template.items(0).start_row
                        out_elements.elements.Clear()
                        out_element = Nothing
                        Dim date_toggle As Boolean = True
                        If InStr(ostr, "err:") > 0 Then
                            out_element = New bc_io_output_element
                            out_element.value = ostr
                            out_element.row = row
                            out_element.col = template.items(0).start_col + lcol

                            out_elements.elements.Add(out_element)
                        Else
                            While InStr(ostr, ";") > 0

                                out_element = New bc_io_output_element
                                out_element.value = ostr.Substring(0, InStr(ostr, ";") - 1)
                                out_element.row = row
                                If dim_toggle = 1 Then
                                    ignore = False
                                    If ((out_element.value = "1" And periodheaders.periodheadings(i).periodeaflag = "A") Or (out_element.value = "0" And periodheaders.periodheadings(i).periodeaflag = "E")) Then
                                        ignore = True
                                    End If
                                    dim_toggle = 2
                                ElseIf dim_toggle = 2 Then
                                    out_element.col = (template.items(0).start_col + lcol) - 1
                                    dim_toggle = 3

                                ElseIf dim_toggle = 3 Then
                                    out_element.col = template.items(0).start_col + (lcol)
                                    dim_toggle = 1
                                    row = row + 1
                                End If

                                If (dim_toggle = 3 Or (dim_toggle = 1 And template.items(0).date_col + lcol > 0) And ignore = False) Then
                                    out_elements.elements.Add(out_element)
                                End If
                                REM now go through items
                                ostr = Right(ostr, Len(ostr) - InStr(ostr, ";"))
                            End While
                        End If
                        ao_excel.write_io_data(out_elements, True)

                        'retrieve_item_values(out_elements, template.entity_name_row_id, template)
                        'ao_excel.write_io_data(out_elements)
                        lcol = lcol + 2
                        prev_year = periodheaders.periodheadings(i).periodyear
                        prev_period = periodheaders.periodheadings(i).periodname
                    Next

                ElseIf template.io_mode = "time series" Then
                    If prop_entity_name = "" Then
                        Dim omsg As New bc_cs_message("Blue Curve", "No entity name", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Function
                    End If
                    Dim oef As New bc_io_excel_functions
                    Dim res As String
                    Dim stage As String
                    If template.publish = True Then
                        stage = "publish"
                    Else
                        stage = "draft"
                    End If
                    Dim date_from As String = "1-1-1900"
                    Dim date_to As String = "9-9-9999"

                    Dim val As String
                    If template.date_from_col <> 0 And template.date_from_row <> 0 Then
                        val = ao_excel.get_cell_value(template.date_from_row, template.date_from_col)
                        date_from = val
                        If IsDate(val) Then
                            date_from = Format("dd-mmm-yyyy", date_from)
                        Else
                            Dim omsg As New bc_cs_message("bc_am_excel_data_io", "Incorrect date format: " + date_from, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Function
                        End If
                    End If
                    If template.date_to_col <> 0 And template.date_to_row <> 0 Then
                        val = ao_excel.get_cell_value(template.date_to_row, template.date_to_col)
                        date_to = val
                        If IsDate(val) Then
                            date_to = Format("dd-mmm-yyyy", date_to)
                        Else
                            Dim omsg As New bc_cs_message("bc_am_excel_data_io", "Incorrect date format: " + date_to, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Function
                        End If
                    End If

                    res = oef.bc_get_historic_financial_data(template.class_name, prop_entity_name, template.items(0).item_identifier, stage, template.items(0).contributor, date_from, date_to, "", "", "", "date_from,value")
                    Dim ostr As String
                    ostr = res
                    Dim row As Integer
                    row = template.items(0).start_row
                    Dim out_elements As New bc_io_output_elements
                    Dim out_element As bc_io_output_element
                    Dim date_toggle As Boolean = True
                    If InStr(ostr, "err:") > 0 Then
                        out_element = New bc_io_output_element
                        out_element.value = ostr
                        out_element.row = row
                        out_element.col = template.items(0).start_col
                        out_elements.elements.Add(out_element)
                    Else
                        While InStr(ostr, ";") > 0
                            out_element = New bc_io_output_element
                            out_element.value = ostr.Substring(0, InStr(ostr, ";") - 1)
                            out_element.row = row
                            If date_toggle = True Then
                                out_element.col = template.items(0).date_col
                                date_toggle = False
                            Else
                                out_element.col = template.items(0).start_col
                                date_toggle = True
                                row = row + 1
                            End If
                            If date_toggle = True Or (date_toggle = False And template.items(0).date_col > 0) Then
                                out_elements.elements.Add(out_element)
                            End If
                            REM now go through items
                            ostr = Right(ostr, Len(ostr) - InStr(ostr, ";"))
                        End While
                        'If template.items(0).date_col > 0 Then
                        '    out_element = New bc_io_output_element
                        '    out_element.value = template.list_end_delimiter
                        '    out_element.row = row
                        '    out_element.col = template.items(0).date_col
                        '    out_elements.elements.Add(out_element)
                        'End If
                        'out_element = New bc_io_output_element
                        'out_element.value = template.list_end_delimiter
                        'out_element.row = row
                        'out_element.col = template.items(0).start_col
                        'out_elements.elements.Add(out_element)
                        REM write values into excel
                    End If
                    ao_excel.write_io_data(out_elements, True)

                End If
            End If
        Catch ex As Exception

            Dim oerr As New bc_cs_error_log("bc_am_excel_data_io", "execute_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Private Function parse_results(ByVal res As String, ByVal template As bc_in_excel_io_template) As bc_io_output_elements
        Dim out_elements As New bc_io_output_elements
        Try
            Dim out_element As bc_io_output_element
            Dim ostr As String
            Dim row As Integer
            row = template.list_start_row - 1
            ostr = res

            While InStr(ostr, ";") > 0
                out_element = New bc_io_output_element
                out_element.value = ostr.Substring(0, InStr(ostr, ";") - 1)
                If check_if_in_filter(out_element.value, template) = True Then
                    row = row + 1
                    out_element.row = row
                    out_element.col = template.list_start_col
                    out_elements.elements.Add(out_element)
                    REM now go through items
                    retrieve_item_values(out_elements, row, template)
                End If
                ostr = Right(ostr, Len(ostr) - InStr(ostr, ";"))
            End While
            If ostr <> "" Then
                out_element = New bc_io_output_element
                out_element.value = ostr
                If check_if_in_filter(out_element.value, template) = True Then
                    row = row + 1
                    out_element.row = row
                    out_element.col = template.list_start_col
                    out_elements.elements.Add(out_element)
                    REM now go through items
                    retrieve_item_values(out_elements, row, template)
                End If
            End If
            REM now add end delimiter
            out_element = New bc_io_output_element
            out_element.value = template.list_end_delimiter
            out_element.row = row + 1
            out_element.col = template.list_start_col
            out_elements.elements.Add(out_element)
            parse_results = out_elements
        Catch ex As Exception
            out_elements.elements.Clear()
            parse_results = out_elements
            Dim oerr As New bc_cs_error_log("bc_am_excel_data_io", "parse_results", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function

    Private Function check_if_in_filter(ByRef entity_name As String, ByVal template As bc_in_excel_io_template) As Boolean
        Try
            Dim oef As New bc_io_excel_functions

            check_if_in_filter = True
            Dim stage As String
            Dim filter_value As String
            If template.publish = True Then
                stage = "publish"
            Else
                stage = "draft"
            End If
            For i = 0 To template.items.Count - 1
                filter_value = ""
                If template.items(i).type = "value" Then
                    check_if_in_filter = True
                    filter_value = oef.bc_get_financial_data(template.class_name, entity_name, template.items(i).item_identifier, stage, template.items(i).contributor)
                    If Trim(UCase(filter_value) <> Trim(UCase(template.items(i).value))) Then
                        check_if_in_filter = False
                    Else
                        Exit For
                    End If
                End If
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_excel_data_io", "retrieve_item_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Private Function retrieve_item_values(ByRef out_elements As bc_io_output_elements, ByVal row As Integer, ByVal template As bc_in_excel_io_template) As Boolean
        Try
            retrieve_item_values = False
            Dim oef As New bc_io_excel_functions
            Dim out_element As New bc_io_output_element

            Dim entity_name As String
            Dim stage As String
            If template.publish = True Then
                stage = "publish"
            Else
                stage = "draft"
            End If
            entity_name = out_elements.elements(out_elements.elements.Count - 1).value
            For i = 0 To template.items.Count - 1
                out_element = New bc_io_output_element
                If template.items(i).type = "class" Then
                    out_element.value = oef.load_propogating_entities(5006, template.class_name, entity_name, template.items(i).item_identifier, 1)
                    If out_element.value = "" Then
                        out_element.value = oef.load_propogating_entities(5005, template.class_name, entity_name, template.items(i).item_identifier, 1)
                    End If

                ElseIf template.items(i).type = "item" Then
                    out_element.value = oef.bc_get_financial_data(template.class_name, entity_name, template.items(i).item_identifier, stage, template.items(i).contributor)
                End If
                out_element.row = template.items(i).start_row()
                If out_element.row = 0 Then
                    out_element.row = row
                End If
                out_element.col = template.items(i).start_col
                out_elements.elements.Add(out_element)
            Next
            retrieve_item_values = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_excel_data_io", "retrieve_item_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Private Class bc_io_excel_functions
        Public Function load_propogating_entities(ByVal ty As Integer, ByVal pclass As String, ByVal entity As String, ByVal child_class As String, Optional ByVal schema_id As String = "1") As String
            load_propogating_entities = ""
            Try
                Dim str As String
                str = "<excel_function>" + vbCr + _
                "<type>" + CStr(ty) + "</type>" + vbCr + _
                "<class_id>" + pclass + "</class_id>" + vbCr + _
                "<entity_id>" + entity + "</entity_id>" + vbCr + _
                "<ass_class_id>" + child_class + "</ass_class_id>" + vbCr + _
                "<schema_id>" + schema_id + "</schema_id>" + vbCr + _
                "<priority>all</priority>" + vbCr + _
                "<dimensions>name</dimensions>" + vbCr + _
                "</excel_function>"
                Dim oef As New bc_am_excel_functions
                load_propogating_entities = oef.execute_function(str)
            Catch ex As Exception
                load_propogating_entities = "err:" + ex.Message
                Dim oerr As New bc_cs_error_log("bc_io_excel_functions", "load_propogating_entities", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try
        End Function
        Public Function bc_get_financial_data(ByVal class_name As String, ByVal entity As String, ByVal item As String, ByVal stage As String, ByVal contributor As String, Optional ByVal date_at As String = "9-9-9999", Optional ByVal year As String = "", Optional ByVal period As String = "", Optional ByVal dimensions As String = "value") As String
            bc_get_financial_data = ""
            Try
                Dim str As String
                str = "<excel_function>" + vbCr + _
                      "<type>5002</type>" + vbCr + _
                      "<class_id>" + CStr(class_name) + "</class_id>" + vbCr + _
                      "<entity_id>" + CStr(entity) + "</entity_id>" + vbCr + _
                      "<item_id>" + CStr(item) + "</item_id>" + vbCr + _
                      "<stage_id>" + CStr(stage) + "</stage_id>" + vbCr + _
                      "<contributor_id>" + CStr(contributor) + "</contributor_id>" + vbCr + _
                      "<date_at>" + CStr(date_at) + "</date_at>" + vbCr + _
                      "<year>" + CStr(year) + "</year>" + vbCr + _
                      "<period>" + CStr(period) + "</period>" + vbCr + _
                      "<dimensions>" + CStr(dimensions) + "</dimensions>" + vbCr + _
                      "</excel_function>"
                Dim oef As New bc_am_excel_functions
                bc_get_financial_data = oef.execute_function(str)
            Catch ex As Exception
                bc_get_financial_data = "err:" + ex.Message
                Dim oerr As New bc_cs_error_log("bc_io_excel_functions", "bc_get_financial_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try
        End Function
        Public Function bc_get_historic_financial_data(ByVal class_name As String, ByVal entity As String, ByVal item As String, ByVal stage As String, ByVal contributor As String, Optional ByVal date_from As String = "1-1-1990", Optional ByVal date_to As String = "9-9-9999", Optional ByVal date_at As String = "9-9-9999", Optional ByVal year As String = "", Optional ByVal period As String = "", Optional ByVal dimensions As String = "value") As String
            bc_get_historic_financial_data = ""
            Try
                Dim str As String
                str = "<excel_function>" + vbCr + _
                    "<type>5003</type>" + vbCr + _
                    "<class_id>" + CStr(class_name) + "</class_id>" + vbCr + _
                    "<entity_id>" + CStr(entity) + "</entity_id>" + vbCr + _
                    "<item_id>" + CStr(item) + "</item_id>" + vbCr + _
                    "<stage_id>" + CStr(stage) + "</stage_id>" + vbCr + _
                    "<contributor_id>" + CStr(contributor) + "</contributor_id>" + vbCr + _
                    "<date_from>" + date_from + "</date_from>" + vbCr + _
                    "<date_to>" + date_to + "</date_to>" + vbCr + _
                    "<year>" + CStr(year) + "</year>" + vbCr + _
                    "<period>" + CStr(period) + "</period>" + vbCr + _
                    "<dimensions>" + CStr(dimensions) + "</dimensions>" + vbCr + _
                    "</excel_function>"
                Dim oef As New bc_am_excel_functions
                bc_get_historic_financial_data = oef.execute_function(str)
            Catch ex As Exception
                bc_get_historic_financial_data = "err:" + ex.Message
                Dim oerr As New bc_cs_error_log("bc_io_excel_functions", "bc_get_historic_financial_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try
        End Function


        Public Function bc_get_entities_for_class(ByVal class_name As String, Optional ByVal dimensions As String = "name") As Object
            Try
                Dim str As String
                str = "<excel_function>" + vbCr + _
                "<type>5001</type>" + vbCr + _
                "<class_id>" + CStr(class_name) + "</class_id>" + vbCr + _
                "<dimensions>" + CStr(dimensions) + "</dimensions>" + vbCr + _
                "</excel_function>"
                Dim oef As New bc_am_excel_functions
                bc_get_entities_for_class = oef.execute_function(str)
            Catch ex As Exception
                bc_get_entities_for_class = "err:" + ex.Message
                Dim oerr As New bc_cs_error_log("bc_io_excel_functions", "bc_get_historic_financial_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try
        End Function

    End Class
End Class
Public Class bc_in_excel_io_templates
    Public templates As New ArrayList
    Const INSIGHT_CONFIG_FILE As String = "bc_am_insight_config.xml"
    Public Function load() As Boolean
        Try
            load = False
            templates.Clear()
            REM attempt to open config file
            Dim myXmlNode As Xml.XmlNodeList
            Dim myXmlitemsNode As Xml.XmlNodeList
            Dim xmlload As New Xml.XmlDocument
            REM read in link codes
            xmlload.Load(bc_cs_central_settings.local_template_path + INSIGHT_CONFIG_FILE)

            myXmlNode = xmlload.SelectNodes("/insight_settings/excel_io/template")
            For i = 0 To myXmlNode.Count - 1
                Dim otemplate As New bc_in_excel_io_template
                Try
                    otemplate.io = myXmlNode(i).SelectSingleNode("io").InnerXml
                    If otemplate.io <> "input" And otemplate.io <> "output" And otemplate.io <> "predefined" Then
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "invalid /io for template: " + CStr(i + 1))
                        Exit Function
                    End If
                Catch
                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "invalid /io for template: " + CStr(i + 1))
                    Exit Function
                End Try
                If otemplate.io = "predefined" Then
                    MsgBox("TBD")
                    Exit Function
                End If
                REM io mode
                Try
                    otemplate.io_mode = myXmlNode(i).SelectSingleNode("io_mode").InnerXml
                    If otemplate.io = "output" Then
                        If otemplate.io_mode <> "current" And otemplate.io_mode <> "period time series" And otemplate.io_mode <> "time series" And otemplate.io_mode <> "propogating" Then
                            Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no /io_mode valid values are current, time series or propogating for template:" + CStr(i + 1))
                            Exit Function
                        End If
                    End If
                Catch
                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no /io_mode valid values are current, time series or propogating for template:" + CStr(i + 1))
                    Exit Function
                End Try
                Try
                    otemplate.calc = 0
                    otemplate.calc = myXmlNode(i).SelectSingleNode("calculate").InnerXml
                    If otemplate.calc <> 0 And otemplate.calc <> 1 And otemplate.calc <> 2 Then
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "invalid /calculation for template: " + CStr(i + 1) + " valid values are 0,1 or 2")
                        Exit Function
                    End If
                Catch
                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no /calculation for template: " + CStr(i + 1))
                End Try
                Try
                    otemplate.name = myXmlNode(i).SelectSingleNode("name").InnerXml
                Catch
                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no /name for template: " + CStr(i + 1))
                    Exit Function
                End Try
                Try
                    otemplate.class_name = myXmlNode(i).SelectSingleNode("class_name").InnerXml
                Catch
                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no /class_name for template: " + CStr(i + 1))
                    Exit Function
                End Try
                Try
                    otemplate.entity_identifier = myXmlNode(i).SelectSingleNode("entity_identifier").InnerXml
                Catch
                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no /entity_identifier for template: " + CStr(i + 1))
                    Exit Function
                End Try
                Try
                    otemplate.read_entity_mode = myXmlNode(i).SelectSingleNode("read_entity_mode").InnerXml
                    If otemplate.read_entity_mode <> "none" And otemplate.read_entity_mode <> "from list" And otemplate.read_entity_mode <> "entity name" And otemplate.read_entity_mode <> "row col" And otemplate.read_entity_mode <> "sheet name" Then
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "read_entity_mode valid values are from none,list,entity name,row col, or sheet name  for  template:" + CStr(i + 1))
                        Exit Function
                    End If
                Catch
                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no /read_entity_mode  for output for  template: " + CStr(i + 1))
                    Exit Function
                End Try
                If (otemplate.io = "input" And otemplate.io_mode = "current" And otemplate.read_entity_mode = "from list") Or (otemplate.io = "output" And otemplate.io_mode = "propogating") Then
                    Try
                        otemplate.list_start_col = myXmlNode(i).SelectSingleNode("list_start_col").InnerXml
                    Catch
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no numeric /entity_list_start_col for template: " + CStr(i + 1))
                        Exit Function
                    End Try
                    Try
                        otemplate.list_start_row = myXmlNode(i).SelectSingleNode("list_start_row").InnerXml
                    Catch
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no numeric /entity_list_start_row for template: " + CStr(i + 1))
                        Exit Function
                    End Try
                End If
                If otemplate.io = "input" Or otemplate.io_mode <> "current" Then
                    Try
                        otemplate.list_end_delimiter = myXmlNode(i).SelectSingleNode("list_end_delimiter").InnerXml
                    Catch
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no /delimiter found so blank will be used for template: " + CStr(i + 1))
                        otemplate.list_end_delimiter = ""
                    End Try
                End If
                Try
                    otemplate.draft = CBool(myXmlNode(i).SelectSingleNode("draft").InnerXml)
                Catch ex As Exception
                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /draft found so false will be used for template: " + CStr(i + 1))
                    otemplate.draft = False
                End Try
                Try
                    otemplate.publish = CBool(myXmlNode(i).SelectSingleNode("publish").InnerXml)
                Catch ex As Exception
                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /pubish found so true will be used for template: " + CStr(i + 1))
                    otemplate.publish = True
                End Try
                REM 

                If otemplate.io_mode = "period time series" Then

                    REM Period Data Input
                    Try
                        otemplate.offset_identifier = myXmlNode(i).SelectSingleNode("offset_identifier").InnerXml
                    Catch
                        otemplate.offset_identifier = ""
                    End Try

                    Try
                        otemplate.offset_col = CInt(myXmlNode(i).SelectSingleNode("offset_col").InnerXml)
                    Catch
                        otemplate.offset_col = 0
                    End Try

                    Try
                        otemplate.offset_row_max = CInt(myXmlNode(i).SelectSingleNode("offset_row_max").InnerXml)
                    Catch
                        otemplate.offset_row_max = 0
                    End Try

                End If

                If otemplate.io_mode <> "period time series" Then

                    Try
                        otemplate.date_from_row = CInt(myXmlNode(i).SelectSingleNode("date_from_row").InnerXml)
                    Catch ex As Exception
                        otemplate.date_from_row = 0
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /date_from_row for template: " + CStr(i + 1))
                    End Try

                    Try
                        otemplate.date_from_col = CInt(myXmlNode(i).SelectSingleNode("date_from_col").InnerXml)
                    Catch ex As Exception
                        otemplate.date_from_col = 0
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /date_from_col for template: " + CStr(i + 1))
                    End Try

                    Try
                        otemplate.date_to_row = CInt(myXmlNode(i).SelectSingleNode("date_to_row").InnerXml)
                    Catch ex As Exception
                        otemplate.date_to_row = 0
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /date_to_row for template: " + CStr(i + 1))
                    End Try

                End If

                
                Try
                    otemplate.date_to_col = CInt(myXmlNode(i).SelectSingleNode("date_to_col").InnerXml)
                Catch ex As Exception
                    otemplate.date_to_col = 0
                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /date_to_col for template: " + CStr(i + 1))
                End Try

                REM now do items if none then error
                myXmlitemsNode = myXmlNode(i).SelectNodes("item")
                If myXmlitemsNode.Count = 0 Then
                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no items defined for template: " + CStr(i + 1))
                    Exit Function
                End If
                If otemplate.io = "output" And otemplate.io_mode = "time series" And myXmlitemsNode.Count > 1 Then
                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "only one item can be defined for output time series for template: " + CStr(i + 1))
                    Exit Function
                End If
                If otemplate.io = "input" And otemplate.io_mode = "time series" And myXmlitemsNode.Count > 1 Then
                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "only one item can be defined for input time series for template: " + CStr(i + 1))
                    Exit Function
                End If
                REM parameters for propogating mode only
                If otemplate.io = "output" And otemplate.io_mode = "propogating" Then
                    Try
                        otemplate.propogating_class = myXmlNode(i).SelectSingleNode("propogating_class").InnerXml
                    Catch
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no /parent_class  for output-propogating  for template: " + CStr(i + 1))
                        Exit Function
                    End Try
                End If


                If otemplate.read_entity_mode = "entity name" Then
                    Try
                        otemplate.entity_name = myXmlNode(i).SelectSingleNode("entity_name").InnerXml
                    Catch
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no /entity name  for output/read entity mode:entity_name for  template:" + CStr(i + 1))
                        Exit Function
                    End Try
                End If
                If otemplate.read_entity_mode = "row col" Then
                    Try
                        otemplate.entity_name_row_id = myXmlNode(i).SelectSingleNode("entity_name_row_id").InnerXml
                    Catch
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no /entity_name_row_id for output/read entity mode:row col for  template:" + CStr(i + 1))
                        Exit Function
                    End Try
                    Try
                        otemplate.entity_name_col_id = myXmlNode(i).SelectSingleNode("entity_name_col_id").InnerXml
                    Catch
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no /entity_name_col_id for output/read entity mode:row col for  template:" + CStr(i + 1))
                        Exit Function
                    End Try

                End If


                For j = 0 To myXmlitemsNode.Count - 1
                    Dim oitem As New bc_in_excel_io_item

                    Try
                        oitem.type = myXmlitemsNode(j).SelectSingleNode("type").InnerXml
                        If oitem.type <> "item" And oitem.type <> "class" And oitem.type <> "value" Then
                            Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/item not found valid values are item,class,value in template:" + CStr(i + 1) + " item: " + CStr(j + 1))
                            Exit Function
                        End If
                    Catch
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/item not found so item used in template:" + CStr(i + 1) + " item: " + CStr(j + 1))

                    End Try
                    Try
                        oitem.item_identifier = myXmlitemsNode(j).SelectSingleNode("item_identifier").InnerXml
                    Catch
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/item_identifier not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1))
                        Exit Function
                    End Try
                    Try
                        oitem.item_display_name = myXmlitemsNode(j).SelectSingleNode("item_display_name").InnerXml
                    Catch
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/item_display_name not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1))
                        Exit Function
                    End Try
                    If oitem.type <> "value" Then
                        Try
                            oitem.start_row = myXmlitemsNode(j).SelectSingleNode("start_row").InnerXml
                        Catch
                            Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/start_row not found in template: " + CStr(i + 1) + " item: " + CStr(j + 1) + " so using entity Start row")
                            oitem.start_row = otemplate.entity_name_row_id
                        End Try
                        Try
                            oitem.start_col = myXmlitemsNode(j).SelectSingleNode("start_col").InnerXml
                        Catch
                            Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/start_col not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1))
                            Exit Function
                        End Try
                        Try
                            oitem.datatype = myXmlitemsNode(j).SelectSingleNode("datatype").InnerXml
                        Catch
                            oitem.datatype = 2
                            Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/datatype not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1) + " so string used")
                        End Try
                        Try
                            oitem.mandatory = CBool(myXmlitemsNode(j).SelectSingleNode("mandatory").InnerXml)
                        Catch
                            oitem.mandatory = False
                            Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/mandatory not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1) + " so false used")
                        End Try
                    Else
                        Try
                            oitem.value = myXmlitemsNode(j).SelectSingleNode("value").InnerXml
                        Catch
                            Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/value not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1))
                            Exit Function
                        End Try

                    End If

                    Try
                        oitem.submission_code = CInt(myXmlitemsNode(j).SelectSingleNode("submission_code").InnerXml)
                    Catch
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/submission_code not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1))
                        Exit Function
                    End Try

                    REM period data
                    If oitem.submission_code = 0 Then

                        If (otemplate.io = "input") Then

                            REM XXXX

                            If otemplate.io_mode <> "period time series" Then

                                Try
                                    oitem.year_header_row = CInt(myXmlitemsNode(j).SelectSingleNode("year_header_row").InnerXml)
                                Catch
                                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/year_header_row not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1))
                                    Exit Function
                                End Try
                                Try
                                    oitem.period_header_row = CInt(myXmlitemsNode(j).SelectSingleNode("period_header_row").InnerXml)
                                Catch
                                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/period_header_row  not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1))
                                    Exit Function
                                End Try
                                Try
                                    oitem.ea_header_row = CInt(myXmlitemsNode(j).SelectSingleNode("ea_header_row").InnerXml)
                                Catch
                                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/ea_header_row not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1))
                                    Exit Function
                                End Try
                                Try
                                    oitem.item_col_delimiter = myXmlitemsNode(j).SelectSingleNode("item_col_delimiter").InnerXml
                                Catch
                                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/item_col_delimiter not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1))
                                    Exit Function
                                End Try

                                Try
                                    oitem.period_end_item_row = CInt(myXmlitemsNode(j).SelectSingleNode("period_end_item_row").InnerXml)
                                    Try
                                        oitem.period_end_item_key = myXmlitemsNode(j).SelectSingleNode("period_end_item_key").InnerXml
                                    Catch
                                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/period_end_item_key  in template:" + CStr(i + 1) + " item: " + CStr(j + 1))
                                    End Try
                                Catch
                                    Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/period_end_item_row in template:" + CStr(i + 1) + " item: " + CStr(j + 1))
                                End Try
                            End If

                        End If

                        If (otemplate.io = "output") Then

                            REM Period Data Output
                            Try
                                oitem.start_year = CInt(myXmlitemsNode(j).SelectSingleNode("start_year").InnerXml)
                            Catch
                                Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid start_year not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1))
                            End Try
                            Try
                                oitem.end_year = CInt(myXmlitemsNode(j).SelectSingleNode("end_year").InnerXml)
                            Catch
                                Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid end_year not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1))
                            End Try
                            Try
                                oitem.periods = CInt(myXmlitemsNode(j).SelectSingleNode("periods").InnerXml)
                            Catch
                                Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid periods not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1))
                            End Try

                        End If

                    End If

                    Try
                        oitem.contributor = CInt(myXmlitemsNode(j).SelectSingleNode("contributor").InnerXml)
                    Catch
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/contributor not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1) + " so 1 used")
                        oitem.contributor = 1
                    End Try
                    Try
                        oitem.date_col = CInt(myXmlitemsNode(j).SelectSingleNode("date_col").InnerXml)
                    Catch
                        Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/date_col not found in template:" + CStr(i + 1) + " item: " + CStr(j + 1) + " so 0 indicates use system date.")
                        oitem.date_col = 0
                    End Try


                    If otemplate.io_mode <> "period time series" Then
                        Try
                            oitem.copy_to_draft = myXmlitemsNode(j).SelectSingleNode("copy_to_draft").InnerXml
                            If oitem.copy_to_draft <> 0 Or oitem.copy_to_draft <> 1 Then
                                Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "invalid /item/copy_to_draft valid avlues are 0 or 1 found in template: " + CStr(i + 1) + " item: " + CStr(j + 1) + " so using value 0")
                            End If
                        Catch
                            Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, "no valid /item/copy_to_draft  found in template: " + CStr(i + 1) + " item: " + CStr(j + 1) + " so using value 0")
                        End Try
                    End If



                    otemplate.items.Add(oitem)
                Next
                templates.Add(otemplate)
            Next
            load = True
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_in_excel_io_templates", "load", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try
    End Function

    Public Sub New()

    End Sub
End Class
Public Class bc_in_excel_io_template
    Public name As String
    REM input/output
    Public io As String
    REM input and output parameters
    Public class_name As String
    Public entity_identifier As String
    Public list_start_col As Integer
    Public list_start_row As Integer
    Public draft As Boolean
    Public publish As Boolean
    Public list_end_delimiter = ""
    Public items As New ArrayList
    REM extraction parameters
    REM if this is set entity identifier is read from worksheet name
    REM else it is read from entity_identifier
    REM if this is set then will retrieve for entities that propogate to this class
    Public io_mode As String
    REM current (single entity)
    REM time series (single entity)
    REM propogating (multiple)
    REM only set i mode 2
    Public propogating_class As String
    Public read_entity_mode As String
    REM   entity name
    REM   row col
    REM   sheet name
    Public entity_name_row_id As Integer
    Public entity_name_col_id As Integer
    Public entity_name As String
    Public calc As Integer = 0
    REM -----------------------------------------------------------------------------
    Public date_from_row As Integer = 0
    Public date_from_col As Integer = 0
    Public date_to_row As Integer = 0
    Public date_to_col As Integer = 0
    Public offset_identifier As String
    Public offset_row_max As Integer
    Public offset_col As Integer

    Public Sub New()

    End Sub
End Class
Friend Class bc_in_excel_io_item
    Public type As String = "item"
    Public start_row As Integer
    Public start_col As Integer
    Public date_col As Integer = 0
    Public item_identifier As String
    Public item_display_name As String
    Public datatype As Integer
    Public mandatory As Boolean
    Public submission_code As Integer
    Public contributor As Integer
    Public copy_to_draft As Integer = 0
    Public year_header_row As Integer
    Public ea_header_row As Integer
    Public period_header_row As Integer
    Public period_end_item_row As Integer
    Public item_col_delimiter As String
    Public period_end_item_key As String
    Public value As String
    Public start_year As String
    Public end_year As String
    Public periods As String
    

    Public Sub New()

    End Sub
End Class
Public Class bc_io_output_elements
    Public elements As New ArrayList
End Class
Public Class bc_io_output_element
    Public value As String
    Public row As Integer
    Public col As Integer

    Public Sub New()

    End Sub
End Class

