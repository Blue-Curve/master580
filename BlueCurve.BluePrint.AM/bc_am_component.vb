Imports BlueCurve.Core.CS
Imports BlueCurve.Core.AS
Imports BlueCurve.Core.OM
Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Windows.Forms
'==========================================
' Bluecurve Limited 2008
' Module:         BluePrint
' Type:           AM
' Desciption:     component form controller
' Public Methods: Show
'                 Delete
'  
' Version:        1.0
' Change history:
'
'==========================================
Friend Class bc_am_component

    Private view As bc_am_bp_component
    Private originalDesc As String

    Friend Sub New(Optional ByVal view As bc_am_bp_component = Nothing)

        If Not view Is Nothing Then
            view.Controller = Me
            Me.view = view
        End If

    End Sub

    Friend Overloads Function Show(ByRef componentName As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_bp_component", "Show", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            populateTypes()
            populateRefreshTypes()
            populateExternalComponents()

            ' split tooltips
            Dim tt As String = view.uxToolTip.GetToolTip(view.uxSPHelp)
            If Not tt = Nothing Then
                If tt.Length > 75 Then view.uxToolTip.SetToolTip(view.uxSPHelp, SplitToolTip(tt))
            End If

            tt = view.uxToolTip.GetToolTip(view.uxTypeNameHelp)
            If Not tt = Nothing Then
                If tt.Length > 75 Then view.uxToolTip.SetToolTip(view.uxTypeNameHelp, SplitToolTip(tt))
            End If

            tt = view.uxToolTip.GetToolTip(view.uxFmtFileHelp)
            If Not tt = Nothing Then
                If tt.Length > 75 Then view.uxToolTip.SetToolTip(view.uxFmtFileHelp, SplitToolTip(tt))
            End If

            tt = view.uxToolTip.GetToolTip(view.uxWebServiceNameHelp)
            If Not tt = Nothing Then
                If tt.Length > 75 Then view.uxToolTip.SetToolTip(view.uxWebServiceNameHelp, SplitToolTip(tt))
            End If

            tt = view.uxToolTip.GetToolTip(view.uxCacheHelp)
            If Not tt = Nothing Then
                If tt.Length > 75 Then view.uxToolTip.SetToolTip(view.uxCacheHelp, SplitToolTip(tt))
            End If


            'reset
            originalDesc = ""
            view.uxOK.Enabled = False

            view.uxOK.Text = "Add"

            If view.ShowDialog() = DialogResult.OK Then

                Dim newIndex As Integer
                Dim cachingLevel As Integer

                ' set the caching level
                Select Case True
                    Case view.uxNoCache.Checked
                        cachingLevel = 0
                    Case view.uxCacheWebServiceNameOnly.Checked
                        cachingLevel = 1
                    Case view.uxCacheWebServiceNameAndParams.Checked
                        cachingLevel = 2
                End Select

                If view.uxExternalComponentName.SelectedIndex < 1 Then
                    newIndex = bc_am_load_objects.obc_templates.component_types.add(view.uxDescription.Text, _
                                                                                    view.uxType.SelectedItem.type_id, _
                                                                                    view.uxStoredProcName.Text, view.uxTypeName.Text, _
                                                                                    view.uxRefreshType.SelectedItem.id, view.uxWebServiceName.Text, _
                                                                                    0, _
                                                                                    view.uxFormatFile.Text, cachingLevel)
                Else
                    newIndex = bc_am_load_objects.obc_templates.component_types.add(view.uxDescription.Text, _
                                                                                    view.uxType.SelectedItem.type_id, _
                                                                                    view.uxStoredProcName.Text, view.uxTypeName.Text, _
                                                                                    view.uxRefreshType.SelectedItem.id, view.uxWebServiceName.Text, _
                                                                                    view.uxExternalComponentName.SelectedItem.id, _
                                                                                    view.uxFormatFile.Text, cachingLevel)
                End If

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    ' write component definition via soap
                    log = New bc_cs_activity_log("bc_am_template", "Show", bc_cs_activity_codes.COMMENTARY, "Updating Component via SOAP")
                    bc_am_load_objects.obc_templates.component_types.component_types(newIndex).tmode = bc_am_load_objects.obc_templates.component_types.component_types(newIndex).tWRITE
                    bc_am_load_objects.obc_templates.component_types.component_types(newIndex).transmit_to_server_and_receive(bc_am_load_objects.obc_templates.component_types.component_types(newIndex), True)
                Else
                    ' write component definition via ado
                    log = New bc_cs_activity_log("bc_am_template", "Show", bc_cs_activity_codes.COMMENTARY, "Updating Component via ADO")
                    bc_am_load_objects.obc_templates.component_types.component_types(newIndex).db_write()
                End If

                If bc_am_load_objects.obc_templates.component_types.component_types(newIndex).component_id = -1 Then
                    bc_am_load_objects.obc_templates.component_types.Remove(newIndex)
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Updating Component Failed!", bc_cs_message.MESSAGE)
                    Exit Try
                End If

                ' update component id
                Dim comp As bc_om_component_type
                comp = bc_am_load_objects.obc_templates.component_types.component_types(newIndex)
                bc_am_load_objects.obc_templates.component_types.Remove(newIndex)
                bc_am_load_objects.obc_templates.component_types.add(comp.component_name, comp.mode, comp.sp_name, _
                                                                       comp.insert_object, comp.refresh_type, _
                                                                       comp.web_service_name, comp.external_id, _
                                                                       comp.format_file, cachingLevel, comp.component_id)
                componentName = view.uxDescription.Text
                Show = True

            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_component", "Show", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_component", "Show", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Overloads Function Clone(ByVal index As Integer, ByRef componentName As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_bp_component", "Clone", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Dim viewClone As New bc_am_bp_component_clone
            Dim componentClone As New bc_am_clone(viewClone)

            Clone = componentClone.ShowComponent(bc_am_load_objects.obc_templates.component_types.component_types(index).component_id)

            If Clone = True Then
                Dim newIndex As Integer
                newIndex = bc_am_load_objects.obc_templates.component_types.add(viewClone.uxComponentName.Text, _
                                                                                    bc_am_load_objects.obc_templates.component_types.component_types(index).mode, _
                                                                                    bc_am_load_objects.obc_templates.component_types.component_types(index).sp_name, _
                                                                                    bc_am_load_objects.obc_templates.component_types.component_types(index).insert_object, _
                                                                                    bc_am_load_objects.obc_templates.component_types.component_types(index).refresh_type, _
                                                                                    bc_am_load_objects.obc_templates.component_types.component_types(index).web_service_name, _
                                                                                    0, _
                                                                                    bc_am_load_objects.obc_templates.component_types.component_types(index).format_file, _
                                                                                    bc_am_load_objects.obc_templates.component_types.component_types(index).cache_level)

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    ' write component definition via soap
                    log = New bc_cs_activity_log("bc_am_template", "Show", bc_cs_activity_codes.COMMENTARY, "Updating Component via SOAP")
                    bc_am_load_objects.obc_templates.component_types.component_types(newIndex).tmode = bc_am_load_objects.obc_templates.component_types.component_types(newIndex).tWRITE
                    bc_am_load_objects.obc_templates.component_types.component_types(newIndex).transmit_to_server_and_receive(bc_am_load_objects.obc_templates.component_types.component_types(newIndex), True)
                Else
                    ' write component definition via ado
                    log = New bc_cs_activity_log("bc_am_template", "Show", bc_cs_activity_codes.COMMENTARY, "Updating Component via ADO")
                    bc_am_load_objects.obc_templates.component_types.component_types(newIndex).db_write()
                End If

                If bc_am_load_objects.obc_templates.component_types.component_types(newIndex).component_id = -1 Then
                    bc_am_load_objects.obc_templates.component_types.Remove(newIndex)
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Updating Component Failed!", bc_cs_message.MESSAGE)
                    Exit Try
                End If

                ' update component id
                Dim comp As bc_om_component_type
                comp = bc_am_load_objects.obc_templates.component_types.component_types(newIndex)
                bc_am_load_objects.obc_templates.component_types.Remove(newIndex)
                bc_am_load_objects.obc_templates.component_types.add(comp.component_name, comp.mode, comp.sp_name, _
                                                                       comp.insert_object, comp.refresh_type, _
                                                                       comp.web_service_name, comp.external_id, _
                                                                       comp.format_file, comp.cache_level, comp.component_id)
                componentName = comp.component_name
                Clone = True

            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_component", "Show", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_component", "Show", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function


    Friend Overloads Function Show(ByVal index As Integer, ByRef componentName As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_bp_component", "Show", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            populateTypes()
            populateRefreshTypes()
            populateExternalComponents()

            ' split tooltips
            Dim tt As String = view.uxToolTip.GetToolTip(view.uxSPHelp)
            If Not tt = Nothing Then
                If tt.Length > 75 Then view.uxToolTip.SetToolTip(view.uxSPHelp, SplitToolTip(tt))
            End If

            tt = view.uxToolTip.GetToolTip(view.uxTypeNameHelp)
            If Not tt = Nothing Then
                If tt.Length > 75 Then view.uxToolTip.SetToolTip(view.uxTypeNameHelp, SplitToolTip(tt))
            End If

            tt = view.uxToolTip.GetToolTip(view.uxFmtFileHelp)
            If Not tt = Nothing Then
                If tt.Length > 75 Then view.uxToolTip.SetToolTip(view.uxFmtFileHelp, SplitToolTip(tt))
            End If

            tt = view.uxToolTip.GetToolTip(view.uxWebServiceNameHelp)
            If Not tt = Nothing Then
                If tt.Length > 75 Then view.uxToolTip.SetToolTip(view.uxWebServiceNameHelp, SplitToolTip(tt))
            End If

            tt = view.uxToolTip.GetToolTip(view.uxCacheHelp)
            If Not tt = Nothing Then
                If tt.Length > 75 Then view.uxToolTip.SetToolTip(view.uxCacheHelp, SplitToolTip(tt))
            End If

            ' disable type
            view.uxType.Enabled = False

            ' set the form title
            view.Text = bc_am_load_objects.obc_templates.component_types.component_types(index).component_name

            Dim i As Integer

            ' load details
            For i = 0 To bc_am_load_objects.obc_templates.component_types.addable_types.Count - 1
                If bc_am_load_objects.obc_templates.component_types.addable_types(i).type_id = bc_am_load_objects.obc_templates.component_types.component_types(index).mode Then
                    view.uxType.SelectedItem = bc_am_load_objects.obc_templates.component_types.addable_types(i)
                    Exit For
                End If
            Next

            ' update the form values
            view.uxTypeName.Text = bc_am_load_objects.obc_templates.component_types.component_types(index).insert_object
            view.uxDescription.Text = bc_am_load_objects.obc_templates.component_types.component_types(index).component_name
            view.uxStoredProcName.Text = bc_am_load_objects.obc_templates.component_types.component_types(index).sp_name

            For i = 0 To bc_am_load_objects.obc_templates.component_types.refresh_types.Count - 1
                If bc_am_load_objects.obc_templates.component_types.refresh_types(i).id = bc_am_load_objects.obc_templates.component_types.component_types(index).refresh_type Then
                    view.uxRefreshType.SelectedItem = bc_am_load_objects.obc_templates.component_types.refresh_types(i)
                    Exit For
                End If
            Next
            view.uxWebServiceName.Text = bc_am_load_objects.obc_templates.component_types.component_types(index).web_service_name
            For i = 0 To bc_am_load_objects.obc_templates.component_types.external_component_types.Count - 1
                If bc_am_load_objects.obc_templates.component_types.external_component_types(i).id = bc_am_load_objects.obc_templates.component_types.component_types(index).external_id Then
                    view.uxExternalComponentName.SelectedItem = bc_am_load_objects.obc_templates.component_types.external_component_types(i)
                    Exit For
                End If
            Next
            view.uxFormatFile.Text = bc_am_load_objects.obc_templates.component_types.component_types(index).format_file
            Select Case bc_am_load_objects.obc_templates.component_types.component_types(index).cache_level
                Case 0
                    view.uxNoCache.Checked = True
                Case 1
                    view.uxCacheWebServiceNameOnly.Checked = True
                Case 2
                    view.uxCacheWebServiceNameAndParams.Checked = True
            End Select

            ' store for validation purposes
            originalDesc = view.uxDescription.Text

            view.uxOK.Text = "Update"

            If view.ShowDialog() = DialogResult.OK Then

                Dim tmp As Integer
                Dim comp As Integer
                Dim subComp As Integer

                'check to see if component is in use
                For tmp = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                    For comp = 0 To bc_am_load_objects.obc_templates.template(tmp).components.component.count - 1
                        For subComp = 0 To bc_am_load_objects.obc_templates.template(tmp).components.component(comp).sub_components.sub_component.count - 1
                            If bc_am_load_objects.obc_templates.template(tmp).components.component(comp).sub_components.sub_component(subComp).type = _
                                bc_am_load_objects.obc_templates.component_types.component_types(index).component_id Then
                                If MessageBox.Show("The component is currently in use." & vbCrLf & _
                                                    "Changing this component will affect multiple templates." & vbCrLf & _
                                                    "Are you sure?", "Update", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                                    bc_am_load_objects.obc_templates.component_types.component_types(index).insert_object = view.uxTypeName.Text
                                    bc_am_load_objects.obc_templates.component_types.component_types(index).component_name = view.uxDescription.Text
                                    bc_am_load_objects.obc_templates.component_types.component_types(index).sp_name = view.uxStoredProcName.Text
                                    bc_am_load_objects.obc_templates.component_types.component_types(index).refresh_type = view.uxRefreshType.SelectedItem.id
                                    bc_am_load_objects.obc_templates.component_types.component_types(index).web_service_name = view.uxWebServiceName.Text
                                    If view.uxExternalComponentName.SelectedIndex < 1 Then
                                        bc_am_load_objects.obc_templates.component_types.component_types(index).external_id = 0
                                    Else
                                        bc_am_load_objects.obc_templates.component_types.component_types(index).external_id = view.uxExternalComponentName.SelectedItem.id
                                    End If
                                    bc_am_load_objects.obc_templates.component_types.component_types(index).format_file = view.uxFormatFile.Text
                                    Select Case True
                                        Case view.uxNoCache.Checked
                                            bc_am_load_objects.obc_templates.component_types.component_types(index).cache_level = 0
                                        Case view.uxCacheWebServiceNameOnly.Checked
                                            bc_am_load_objects.obc_templates.component_types.component_types(index).cache_level = 1
                                        Case view.uxCacheWebServiceNameAndParams.Checked
                                            bc_am_load_objects.obc_templates.component_types.component_types(index).cache_level = 2
                                    End Select

                                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                                        ' write template definition via soap
                                        log = New bc_cs_activity_log("bc_am_template", "Show", bc_cs_activity_codes.COMMENTARY, "Updating Component via SOAP")
                                        bc_am_load_objects.obc_templates.component_types.component_types(index).tmode = bc_am_load_objects.obc_templates.component_types.component_types(index).tWRITE
                                        bc_am_load_objects.obc_templates.component_types.component_types(index).transmit_to_server_and_receive(bc_am_load_objects.obc_templates.component_types.component_types(index), True)
                                    Else
                                        ' write template definition via ado
                                        log = New bc_cs_activity_log("bc_am_template", "Show", bc_cs_activity_codes.COMMENTARY, "Updating Component via ADO")
                                        bc_am_load_objects.obc_templates.component_types.component_types(index).db_write()
                                    End If

                                    componentName = view.uxDescription.Text
                                    Show = True
                                End If
                                Exit Try
                            End If
                        Next
                    Next
                Next

                ' if the component is not in use then just update
                bc_am_load_objects.obc_templates.component_types.component_types(index).insert_object = view.uxTypeName.Text
                bc_am_load_objects.obc_templates.component_types.component_types(index).component_name = view.uxDescription.Text
                bc_am_load_objects.obc_templates.component_types.component_types(index).sp_name = view.uxStoredProcName.Text
                bc_am_load_objects.obc_templates.component_types.component_types(index).refresh_type = view.uxRefreshType.SelectedItem.id
                bc_am_load_objects.obc_templates.component_types.component_types(index).web_service_name = view.uxWebServiceName.Text
                If view.uxExternalComponentName.SelectedIndex < 1 Then
                    bc_am_load_objects.obc_templates.component_types.component_types(index).external_id = 0
                Else
                    bc_am_load_objects.obc_templates.component_types.component_types(index).external_id = view.uxExternalComponentName.SelectedItem.id
                End If
                bc_am_load_objects.obc_templates.component_types.component_types(index).format_file = view.uxFormatFile.Text
                Select Case True
                    Case view.uxNoCache.Checked
                        bc_am_load_objects.obc_templates.component_types.component_types(index).cache_level = 0
                    Case view.uxCacheWebServiceNameOnly.Checked
                        bc_am_load_objects.obc_templates.component_types.component_types(index).cache_level = 1
                    Case view.uxCacheWebServiceNameAndParams.Checked
                        bc_am_load_objects.obc_templates.component_types.component_types(index).cache_level = 2
                End Select

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    ' write template definition via soap
                    log = New bc_cs_activity_log("bc_am_template", "Show", bc_cs_activity_codes.COMMENTARY, "Updating Component via SOAP")
                    bc_am_load_objects.obc_templates.component_types.component_types(index).tmode = bc_am_load_objects.obc_templates.component_types.component_types(index).tWRITE
                    bc_am_load_objects.obc_templates.component_types.component_types(index).transmit_to_server_and_receive(bc_am_load_objects.obc_templates.component_types.component_types(index), True)
                Else
                    ' write template definition via ado
                    log = New bc_cs_activity_log("bc_am_template", "Show", bc_cs_activity_codes.COMMENTARY, "Updating Component via ADO")
                    bc_am_load_objects.obc_templates.component_types.component_types(index).db_write()
                End If

                componentName = view.uxDescription.Text
                Show = True

            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_component", "Show", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_component", "Show", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Function Delete(ByVal index As Integer) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_bp_component", "Delete", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim tmp As Integer
            Dim comp As Integer
            Dim subComp As Integer

            Dim templatesInUse As New ArrayList

            'check that component is not currently used
            For tmp = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                For comp = 0 To bc_am_load_objects.obc_templates.template(tmp).components.component.count - 1
                    For subComp = 0 To bc_am_load_objects.obc_templates.template(tmp).components.component(comp).sub_components.sub_component.count - 1
                        If bc_am_load_objects.obc_templates.template(tmp).components.component(comp).sub_components.sub_component(subComp).type = _
                            bc_am_load_objects.obc_templates.component_types.component_types(index).component_id Then
                            templatesInUse.Add(bc_am_load_objects.obc_templates.template(tmp).name)
                        End If
                    Next
                Next
            Next

            If templatesInUse.Count > 0 Then
                Dim sb As New StringBuilder
                Dim i As Integer

                sb.Append("The component is in use by the following templates and cannot be deleted.")
                sb.Append(Environment.NewLine)
                sb.Append(Environment.NewLine)
                For i = 0 To templatesInUse.Count - 1
                    sb.Append(templatesInUse(i))
                    sb.Append(Environment.NewLine)
                Next i
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", sb.ToString(), bc_cs_message.MESSAGE)
                Exit Try
            End If

            ' mark component for deletion
            bc_am_load_objects.obc_templates.component_types.component_types(index).delete = True

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                ' delete component type via soap
                log = New bc_cs_activity_log("bc_am_template", "Delete", bc_cs_activity_codes.COMMENTARY, "Deleting Component via SOAP")
                bc_am_load_objects.obc_templates.component_types.component_types(index).tmode = bc_am_load_objects.obc_templates.component_types.component_types(index).tWRITE
                bc_am_load_objects.obc_templates.component_types.component_types(index).transmit_to_server_and_receive(bc_am_load_objects.obc_templates.component_types.component_types(index), True)
            Else
                ' delete component type via ado
                log = New bc_cs_activity_log("bc_am_template", "Delete", bc_cs_activity_codes.COMMENTARY, "Deleting Component via ADO")
                bc_am_load_objects.obc_templates.component_types.component_types(index).db_write()
            End If

            ' remove from memory
            bc_am_load_objects.obc_templates.component_types.Remove(index)

            Delete = True

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_component", "Delete", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_component", "Delete", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Sub populateTypes()

        Dim log = New bc_cs_activity_log("bc_am_bp_component", "populateTypes", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer

            For i = 0 To bc_am_load_objects.obc_templates.component_types.addable_types.Count - 1
                view.uxType.Items.Add(bc_am_load_objects.obc_templates.component_types.addable_types(i))
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_component", "populateTypes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_component", "populateTypes", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub populateRefreshTypes()

        Dim log = New bc_cs_activity_log("bc_am_bp_component", "populateRefreshTypes", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer

            For i = 0 To bc_am_load_objects.obc_templates.component_types.refresh_types.Count - 1
                view.uxRefreshType.Items.Add(bc_am_load_objects.obc_templates.component_types.refresh_types(i))
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_component", "populateRefreshTypes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_component", "populateRefreshTypes", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub populateExternalComponents()

        Dim log = New bc_cs_activity_log("bc_am_bp_component", "populateExternalComponents", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer

            view.uxExternalComponentName.Items.Add("")

            For i = 0 To bc_am_load_objects.obc_templates.component_types.external_component_types.Count - 1
                view.uxExternalComponentName.Items.Add(bc_am_load_objects.obc_templates.component_types.external_component_types(i))
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_bp_component", "populateExternalComponents", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_bp_component", "populateExternalComponents", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub


    Private Function SplitToolTip(ByVal strOrig As String) As String

        Dim strArray As String()
        Dim SPACE As String = " "
        Dim CR As String = vbCrLf
        Dim strOneWord As String
        Dim strBuilder As String = ""
        Dim strReturn As String = ""
        strArray = strOrig.Split(SPACE)
        For Each strOneWord In strArray
            strBuilder = strBuilder & strOneWord & SPACE
            If Len(strBuilder) > 70 Then
                strReturn = strReturn & strBuilder & CR
                strBuilder = ""
            End If
        Next
        If Len(strBuilder) < 8 Then strReturn = strReturn.Substring(0, _
                                                    strReturn.Length - 2)
        Return strReturn & strBuilder

    End Function

    Private Function componentExists(ByVal componentName As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_component", "componentExists", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer

            For i = 0 To bc_am_load_objects.obc_templates.component_types.component_types.Count - 1
                If LCase(bc_am_load_objects.obc_templates.component_types.component_types(i).component_name) = LCase(componentName) Then
                    componentExists = True
                    Exit For
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_component", "componentExists", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_component", "componentExists", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Function CloneComponent(ByVal componentId As Integer) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_component", "CloneComponent", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim viewClone As New bc_am_bp_component_clone
            Dim componentClone As New bc_am_clone(viewClone)

            CloneComponent = componentClone.ShowComponent(componentId)


        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_component", "CloneComponent", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_component", "CloneComponent", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function


    Friend Function Validate(ByVal checkAlreadyExists As Boolean) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_component", "Validate", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            If view.uxType.Text.Contains("Single Cell") Or view.uxType.Text = "Dynamic" Or view.uxType.SelectedIndex = 0 Then
                view.uxTypeName.Enabled = False
            Else
                view.uxTypeName.Enabled = True
            End If

            If view.uxType.Text.Contains("Read/Write Single Cell") Then
                view.uxStoredProcName.Enabled = False
            Else
                view.uxStoredProcName.Enabled = True
            End If

            If (Trim(view.uxTypeName.Text) = "" And view.uxType.Text <> "Single Cell") Or _
                Trim(view.uxDescription.Text) = "" Or _
                Trim(view.uxStoredProcName.Text) = "" Or _
                view.uxType.SelectedIndex = -1 Or _
                view.uxRefreshType.SelectedIndex = -1 Then
                Validate = False
                view.uxOK.Enabled = False
            Else
                Validate = True
                view.uxOK.Enabled = True
            End If

            ' if external component is specified then ensure all fields are entered
            If (Trim(view.uxWebServiceName.Text) <> "" And (view.uxExternalComponentName.SelectedIndex < 1 Or Trim(view.uxFormatFile.Text) = "")) Or _
                (view.uxExternalComponentName.SelectedIndex > 0 And (Trim(view.uxWebServiceName.Text) = "" Or Trim(view.uxFormatFile.Text) = "")) Or _
                (Trim(view.uxFormatFile.Text) <> "" And (view.uxExternalComponentName.SelectedIndex < 1 Or Trim(view.uxWebServiceName.Text) = "")) Then
                Validate = False
                view.uxOK.Enabled = False
            End If

            If checkAlreadyExists Then
                'check component does not exist already
                If LCase(originalDesc) <> LCase(view.uxDescription.Text) AndAlso componentExists(view.uxDescription.Text) Then
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Component already exists.", bc_cs_message.MESSAGE)
                    Validate = False
                    Exit Try
                End If
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_component", "Validate", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_component", "Validate", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

End Class

'==========================================
' Bluecurve Limited 2008
' Module:         BluePrint
' Type:           AM
' Desciption:     component parameters form
'                 controller
' Public Methods: Show
'                 Save
'                 ChangeOrder
'                 Validate
'                 Reset
'                 LoadContext
'                 LoadDefaultValues
'                 PopulateDetails
'                 ActionToolBar
'
' Version:        1.0
' Change history:
'
'==========================================
Friend Class bc_am_parameters

    Private view As bc_am_bp_parameters
    Private selectedComponent As Integer
    Private results As Object
    Private lookupResults As ArrayList
    Private is_system_set As ArrayList

    Private saveType As eSaveType
    Private Enum eSaveType As Integer
        eAdd = 1
        eEdit = 2
        eOrder = 3
    End Enum

    Private Const create_new_icon As Integer = 0
    Private Const amend_icon As Integer = 1
    Private Const delete_icon As Integer = 2
    Private Const publicationTypes_icon As Integer = 3
    Private Const save_order_icon As Integer = 4
    Private Const refresh_icon As Integer = 7
    Private Const parameter_icon As Integer = 8

    Friend Sub New(ByVal view As bc_am_bp_parameters)

        view.Controller = Me
        Me.view = view

    End Sub

    Friend Function Show(ByVal index As Integer) As Boolean

        selectedComponent = index

        populateAvailableParameters()

        populateCurrentParameters()

        view.Text = bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).component_name

        view.ShowDialog()

        Return True

    End Function

    Private Sub populateAvailableParameters()

        Dim log = New bc_cs_activity_log("bc_am_parameters", "populateAvailableParameters", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer
            Dim bissystemset As Boolean = False

            Dim osql As New bc_om_sql("exec bcc_core_bp_get_parameter_pool")

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success = True Then

                view.uxAvailableParameters.Items.Clear()

                results = osql.results

                lookupResults = New ArrayList
                is_system_set = New ArrayList



                For i = 0 To UBound(results, 2)
                    view.uxAvailableParameters.Items.Add(results(1, i))
                    bissystemset = False
                    If results(3, i) = 7 And results(6, i) = 2 Then

                        bissystemset = True
                    End If

                    is_system_set.Add(bissystemset)
                    osql = New bc_om_sql(results(5, i))

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        osql.tmode = bc_cs_soap_base_class.tREAD
                        osql.transmit_to_server_and_receive(osql, True)
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        osql.db_read()
                    End If

                    'save look up results for later use
                    If osql.success Then
                        lookupResults.Add(osql.results)
                    Else
                        Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Loading available parameters failed!", bc_cs_message.MESSAGE)
                        Exit For
                    End If
                Next
            Else
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Loading available parameters failed!", bc_cs_message.MESSAGE)
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_parameters", "populateAvailableParameters", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_parameters", "populateAvailableParameters", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub populateCurrentParameters()

        Dim log = New bc_cs_activity_log("bc_am_parameters", "populateCurrentParameters", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer
            Dim lvi As ListViewItem

            view.uxCurrentParameters.Items.Clear()

            For i = 0 To bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).parameters.bc_om_component_parameters.count - 1
                lvi = view.uxCurrentParameters.Items.Add(bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).parameters.bc_om_component_parameters(i).name, parameter_icon)
                lvi.Tag = i
            Next

            Reset()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_parameters", "populateCurrentParameters", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_parameters", "populateCurrentParameters", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub ChangeOrder(ByVal moveUp As Boolean)

        Dim log = New bc_cs_activity_log("bc_am_parameters", "ChangeOrder", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim lvi As ListViewItem
            Dim i As Integer

            ' change tghe order of the parameters
            If moveUp Then
                lvi = view.uxCurrentParameters.SelectedItems(0)
                i = lvi.Index
                If lvi.Index > 0 Then
                    view.uxCurrentParameters.SelectedItems(0).Remove()
                    view.uxCurrentParameters.Items.Insert(i - 1, lvi)
                End If
            Else
                lvi = view.uxCurrentParameters.SelectedItems(0)
                i = lvi.Index
                If lvi.Index < view.uxCurrentParameters.Items.Count - 1 Then
                    view.uxCurrentParameters.SelectedItems(0).Remove()
                    view.uxCurrentParameters.Items.Insert(i + 1, lvi)
                End If
            End If

            view.uxAdd.Enabled = False
            view.uxEdit.Enabled = False
            view.uxDelete.Enabled = False
            view.uxSaveOrder.Enabled = True
            view.uxRefresh.Enabled = True

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_parameters", "ChangeOrder", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_parameters", "ChangeOrder", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Function Validate(Optional ByVal saveParameter As Boolean = False) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_parameters", "Validate", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim lvi As ListViewItem

            Validate = True

            ' ensure the same unit cannot be added twice to the same composite.
            If view.uxAvailableParameters.Enabled = True And saveParameter Then
                For Each lvi In view.uxCurrentParameters.Items
                    If bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).parameters.bc_om_component_parameters(lvi.Tag).name = results(1, view.uxAvailableParameters.SelectedIndex) Then
                        Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "This parameter is already added to the component", bc_cs_message.MESSAGE)
                        Validate = False
                    End If
                Next
            End If

            If view.uxDefaultValue.SelectedIndex = 0 And Not saveParameter Then
                Validate = False
            End If

            If view.uxAvailableParameters.SelectedIndex = -1 And Not saveParameter Then
                Validate = False
            End If

            If Validate Then
                view.uxSave.Enabled = True
            Else
                view.uxSave.Enabled = False
            End If

        Catch ex As Exception
            Validate = False
            Dim errLog As New bc_cs_error_log("bc_am_parameters", "Validate", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_parameters", "Validate", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Sub Reset()

        Dim log = New bc_cs_activity_log("bc_am_parameters", "Reset", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            ' reset the form to initial state
            view.uxAdd.Enabled = True
            view.uxParameters.Enabled = True
            view.uxDefinition.Enabled = False
            view.uxAvailableParameters.SelectedIndex = -1
            view.uxDefaultValue.SelectedIndex = -1
            view.uxDelete.Enabled = False
            view.uxEdit.Enabled = False
            view.uxSaveOrder.Enabled = False
            view.uxRefresh.Enabled = False
            view.uxUp.Enabled = False
            view.uxDown.Enabled = False
            'view.uxAdvanced.Enabled = False
            If view.uxCurrentParameters.SelectedItems.Count = 1 Then
                view.uxCurrentParameters.SelectedItems(0).Selected = False
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_parameters", "Reset", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_parameters", "Reset", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub PopulateDetails()

        Dim log = New bc_cs_activity_log("bc_am_parameters", "PopulateDetails", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            view.uxAvailableParameters.Text = bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).parameters.bc_om_component_parameters(view.uxCurrentParameters.SelectedItems(0).Tag).name
            If bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).parameters.bc_om_component_parameters(view.uxCurrentParameters.SelectedItems(0).Tag).system_defined = 1 Then
                view.uxDefaultValue.SelectedIndex = 1
            ElseIf bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).parameters.bc_om_component_parameters(view.uxCurrentParameters.SelectedItems(0).Tag).system_defined = 2 Then

                view.uxDefaultValue.SelectedIndex = 2

            Else
                If bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).parameters.bc_om_component_parameters(view.uxCurrentParameters.SelectedItems(0).Tag).lookup_values_ids.count > 0 Then
                    view.uxDefaultValue.SelectedValue = bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).parameters.bc_om_component_parameters(view.uxCurrentParameters.SelectedItems(0).Tag).default_value_id
                Else
                    view.uxDefaultValue.SelectedValue = bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).parameters.bc_om_component_parameters(view.uxCurrentParameters.SelectedItems(0).Tag).default_value
                End If
            End If

            If bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).parameters.bc_om_component_parameters(view.uxCurrentParameters.SelectedItems(0).Tag).disabled_in_doc = 0 Then
                view.uxChangeInDoc.Checked = True
            Else
                view.uxChangeInDoc.Checked = False
            End If

            REM FIL JULY 2013
            If bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).parameters.bc_om_component_parameters(view.uxCurrentParameters.SelectedItems(0).Tag).mandatory = 0 Then
                view.Chkman.Checked = False
            Else
                view.Chkman.Checked = True
            End If

            view.uxEdit.Enabled = True
            view.uxDelete.Enabled = True
            view.uxUp.Enabled = True
            view.uxDown.Enabled = True

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_parameters", "PopulateDetails", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_parameters", "PopulateDetails", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub ActionToolbar(ByVal btn As ToolBarButton)

        Dim log = New bc_cs_activity_log("bc_am_parameters", "ActionToolbar", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Select Case btn.ImageIndex
                Case create_new_icon
                    view.uxDefinition.Enabled = True
                    view.uxAvailableParameters.Enabled = True
                    view.uxAvailableParameters.SelectedIndex = -1
                    view.uxDefaultValue.SelectedIndex = -1
                    view.uxParameters.Enabled = False
                    view.uxSave.Enabled = False
                    saveType = eSaveType.eAdd
                Case amend_icon
                    view.uxDefinition.Enabled = True
                    view.uxParameters.Enabled = False
                    view.uxAvailableParameters.Enabled = False
                    saveType = eSaveType.eEdit
                Case delete_icon
                    deleteParameter()
                Case save_order_icon
                    saveType = eSaveType.eOrder
                    Save()
                Case refresh_icon
                    populateCurrentParameters()
            End Select

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_parameters", "ActionToolbar", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_parameters", "ActionToolbar", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Function Save() As Boolean

        Dim log = New bc_cs_activity_log("bc_am_parameters", "Save", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim sql As New StringBuilder

            If Validate(True) Then

                view.Cursor = Cursors.WaitCursor

                Select Case saveType
                    Case eSaveType.eAdd
                        Dim lookupSQL As String
                        lookupSQL = results(5, view.uxAvailableParameters.SelectedIndex)

                        With sql
                            .Append("exec dbo.bcc_core_bp_insert_component_parameter ")
                            .Append(bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).component_id) ' component type id
                            .Append(",'")
                            .Append(results(1, view.uxAvailableParameters.SelectedIndex)) ' name
                            .Append("',")
                            .Append(results(2, view.uxAvailableParameters.SelectedIndex)) ' order
                            .Append(",")
                            .Append(results(3, view.uxAvailableParameters.SelectedIndex)) ' data type
                            .Append(",")
                            .Append(results(4, view.uxAvailableParameters.SelectedIndex)) ' class id
                            .Append(",'")
                            .Append(lookupSQL.Replace("'", "''")) ' look up sql
                            .Append("',")
                            If view.uxDefaultValue.SelectedIndex = 1 Then
                                .Append("1") ' system defined
                            ElseIf view.uxDefaultValue.Text = "System Set" Then

                                .Append("2") ' system set
                            Else
                                .Append("0") ' not system defined
                            End If
                            .Append(",'")
                            If view.uxDefaultValue.SelectedValue <> "0" Then
                                .Append(view.uxDefaultValue.SelectedValue) ' default value
                            Else
                                .Append("0") ' default value
                            End If
                            .Append("',")
                            .Append(IIf(view.uxChangeInDoc.Checked, 0, 1))

                            .Append(",")
                            .Append(IIf(view.Chkman.Checked, 1, 0))
                        End With

                        Dim osql As New bc_om_sql(sql.ToString)
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                            osql.tmode = bc_cs_soap_base_class.tREAD
                            osql.transmit_to_server_and_receive(osql, True)
                        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            osql.db_read()
                        End If

                        If osql.success = True Then
                            Dim compTypeID As Integer
                            ' store current component ID
                            compTypeID = bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).component_id
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                                Dim ocommentary = New bc_cs_activity_log("bc_am_bp_parameters", "Save", bc_cs_activity_codes.COMMENTARY, "Loading Component Types via SOAP")
                                bc_am_load_objects.obc_templates.component_types.tmode = bc_cs_soap_base_class.tREAD
                                bc_am_load_objects.obc_templates.component_types.transmit_to_server_and_receive(bc_am_load_objects.obc_templates.component_types, True)
                            Else
                                Dim ocommentary = New bc_cs_activity_log("bc_am_bp_parameters", "Save", bc_cs_activity_codes.COMMENTARY, "Loading Component Types via Database")
                                bc_am_load_objects.obc_templates.component_types.db_read()
                            End If
                            'reset selected component variable
                            Dim i As Integer
                            For i = 0 To bc_am_load_objects.obc_templates.component_types.component_types.Count
                                If bc_am_load_objects.obc_templates.component_types.component_types(i).component_id = compTypeID Then
                                    selectedComponent = i
                                    Exit For
                                End If
                            Next
                            populateCurrentParameters()
                            Save = True
                        Else
                            Save = False
                            Dim errLog As New bc_cs_error_log("bc_am_composite", "Save", bc_cs_error_codes.USER_DEFINED, "Failed to save parameter")
                            Exit Try
                        End If

                    Case eSaveType.eEdit
                        With sql
                            .Append("exec dbo.bcc_core_bp_update_component_parameter ")
                            .Append(bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).component_id) ' component type id
                            .Append(",'")
                            .Append(results(1, view.uxAvailableParameters.SelectedIndex)) ' name
                            .Append("','")
                            If view.uxDefaultValue.SelectedValue <> "0" Then
                                .Append(view.uxDefaultValue.SelectedValue) ' default value
                            Else
                                .Append("0") ' default value
                            End If
                            .Append("',")
                            If view.uxDefaultValue.SelectedIndex = 1 Then
                                .Append("1") ' system defined
                            ElseIf view.uxDefaultValue.Text = "System Set" Then

                                .Append("2") ' system set
                            Else
                                .Append("0") ' not system defined
                            End If
                            .Append(",")
                            .Append(IIf(view.uxChangeInDoc.Checked, 0, 1))

                            .Append(",")
                            .Append(IIf(view.Chkman.Checked, 1, 0))
                        End With

                        Dim osql As New bc_om_sql(sql.ToString)
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                            osql.tmode = bc_cs_soap_base_class.tREAD
                            osql.transmit_to_server_and_receive(osql, True)
                        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            osql.db_read()
                        End If

                        If osql.success = True Then
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                                Dim ocommentary = New bc_cs_activity_log("bc_am_bp_parameters", "Save", bc_cs_activity_codes.COMMENTARY, "Loading Component Types via SOAP")
                                bc_am_load_objects.obc_templates.component_types.tmode = bc_cs_soap_base_class.tREAD
                                bc_am_load_objects.obc_templates.component_types.transmit_to_server_and_receive(bc_am_load_objects.obc_templates.component_types, True)
                            Else
                                Dim ocommentary = New bc_cs_activity_log("bc_am_bp_parameters", "Save", bc_cs_activity_codes.COMMENTARY, "Loading Component Types via Database")
                                bc_am_load_objects.obc_templates.component_types.db_read()
                            End If
                            populateCurrentParameters()
                            Save = True
                        Else
                            Save = False
                            Dim errLog As New bc_cs_error_log("bc_am_composite", "Save", bc_cs_error_codes.USER_DEFINED, "Failed to save composite")
                            Exit Try
                        End If
                    Case eSaveType.eOrder
                        Dim lvi As ListViewItem
                        Dim params As String = ""
                        With sql
                            .Append("exec dbo.bcc_core_bp_update_parameter_order ")
                            .Append(bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).component_id) ' component type id
                            .Append(",'")
                            For Each lvi In view.uxCurrentParameters.Items
                                params = String.Concat(params, bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).parameters.bc_om_component_parameters(lvi.Tag).name, "|")
                            Next
                            .Append(Left(params, Len(params) - 1))
                            .Append("'")
                        End With

                        Dim osql As New bc_om_sql(sql.ToString)
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                            osql.tmode = bc_cs_soap_base_class.tREAD
                            osql.transmit_to_server_and_receive(osql, True)
                        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            osql.db_read()
                        End If

                        If osql.success = True Then
                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                                Dim ocommentary = New bc_cs_activity_log("bc_am_bp_parameters", "Save", bc_cs_activity_codes.COMMENTARY, "Loading Component Types via SOAP")
                                bc_am_load_objects.obc_templates.component_types.tmode = bc_cs_soap_base_class.tREAD
                                bc_am_load_objects.obc_templates.component_types.transmit_to_server_and_receive(bc_am_load_objects.obc_templates.component_types, True)
                            Else
                                Dim ocommentary = New bc_cs_activity_log("bc_am_bp_parameters", "Save", bc_cs_activity_codes.COMMENTARY, "Loading Component Types via Database")
                                bc_am_load_objects.obc_templates.component_types.db_read()
                            End If
                            populateCurrentParameters()
                            Save = True
                        Else
                            Save = False
                            Dim errLog As New bc_cs_error_log("bc_am_composite", "Save", bc_cs_error_codes.USER_DEFINED, "Failed to save composite")
                            Exit Try
                        End If
                End Select
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_parameters", "Save", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default
            log = New bc_cs_activity_log("bc_am_parameters", "Save", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Sub LoadDefaultValues()

        Dim log = New bc_cs_activity_log("bc_am_parameters", "LoadDefaultValues", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim results As Object
            Dim i As Integer

            If view.uxAvailableParameters.SelectedIndex = -1 Then
                Exit Sub
            End If

            results = lookupResults(view.uxAvailableParameters.SelectedIndex)

            Dim cbh As New ComboBoxHelper

            cbh.Add("0", "System Defined")

            If is_system_set(view.uxAvailableParameters.SelectedIndex) Then
                cbh.Add("0", "System Set")
            End If


            If Not results Is Nothing Then
                For i = 0 To UBound(results, 2)
                    cbh.Add(results(0, i).ToString(), results(1, i))
                Next
            End If

            view.uxDefaultValue.DataSource = cbh
            view.uxDefaultValue.DisplayMember = "Name"
            view.uxDefaultValue.ValueMember = "Code"

            view.uxDefaultValue.SelectedIndex = 0

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_parameters", "LoadDefaultValues", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            view.Cursor = Cursors.Default
            log = New bc_cs_activity_log("bc_am_parameters", "LoadDefaultValues", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub deleteParameter()

        Dim log = New bc_cs_activity_log("bc_am_parameters", "deleteParameter", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            If MessageBox.Show("Are you sure you wish to delete the selected parameter?", "Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                Dim sql As New StringBuilder

                Cursor.Current = Cursors.WaitCursor

                With sql
                    .Append("exec bcc_core_bp_delete_component_parameter ")
                    .Append(bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).component_id)
                    .Append(",'")
                    .Append(bc_am_load_objects.obc_templates.component_types.component_types(selectedComponent).parameters.bc_om_component_parameters(view.uxCurrentParameters.SelectedItems(0).Tag).name)
                    .Append("'")
                End With

                Dim osql As New bc_om_sql(sql.ToString)

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success = True Then
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        Dim ocommentary = New bc_cs_activity_log("bc_am_bp_parameters", "deleteParameter", bc_cs_activity_codes.COMMENTARY, "Loading Component Types via SOAP")
                        bc_am_load_objects.obc_templates.component_types.tmode = bc_cs_soap_base_class.tREAD
                        bc_am_load_objects.obc_templates.component_types.transmit_to_server_and_receive(bc_am_load_objects.obc_templates.component_types, True)
                    Else
                        Dim ocommentary = New bc_cs_activity_log("bc_am_bp_parameters", "deleteParameter", bc_cs_activity_codes.COMMENTARY, "Loading Component Types via Database")
                        bc_am_load_objects.obc_templates.component_types.db_read()
                    End If
                    populateCurrentParameters()
                Else
                    Dim errLog As New bc_cs_error_log("bc_am_bp_parameters", "deleteParameter", bc_cs_error_codes.USER_DEFINED, "Failed to delete parameter")
                    Exit Try
                End If
                ' reset the display
                Reset()
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_parameters", "deleteParameter", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_parameters", "deleteParameter", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

End Class

'==========================================
' Bluecurve Limited 2008
' Module:         BluePrint
' Type:           AM
' Desciption:     component context form
'                 controller
' Public Methods: Show
'                 Update
'                 PopulateDataModelContextConfiguration
'                 CloneDataModel
'                 DeleteContext
'                 MoveDataItems
'                 PopulateFromSelectedDataItem
'                 Save
'                 Validate
'                 UpdateToSelectedDataItem
'  
' Version:        1.0
' Change history:
'
'==========================================

Friend Class bc_am_context

    Private view As bc_am_bp_context
    Private labelView As bc_am_bp_add_label

    Private contextConfig As New DataItems
    Private stylesCombo As ComboBoxHelper

    Private saveType As eSaveType

    Private Enum eSaveType As Integer
        eAdd = 1
        eEdit = 2
    End Enum

    Friend updateSelectedItem As Boolean = True
    Private updated As Boolean = False

    Private contextCount As Integer = -1

    Private defaultFormatMaskIndex As Integer = 0

    Private Enum eContextAttributes As Integer
        eLabelCode = 1
        eScaleSymbol = 2
        eScaleFactor = 3
        eRow = 5
        eColumn = 6
        eDataStyle = 7
        eLabelStyle = 8
        eHorizontalFlag = 9
        eNumberOfYears = 10
        eYearRow = 11
        eYearColumn = 12
        eYearStyle = 13
        eFormatMask = 14
        eLinkCode = 19
        eTitle = 25
        eSource = 26
        eNumberOfActualYears = 27
        eTitleStyle = 28
        eSourceStyle = 29
        eYearToText = 30
        eYearToStyle = 31
        eEstimateIdentifier = 32
        eActualIdentifier = 33
        ePeriodFormat = 34
        ePeriodFormatStyle = 35
        eDisplayPeriod = 36
        ePeriodRow = 37
        ePeriodColumn = 38
    End Enum

    Private Const blank_entry = "<Blank Entry>"
    Private blank_entry_id As Integer = -1
    Private blank_entry_value As Integer = -1

    Friend Sub New(Optional ByVal view As bc_am_bp_context = Nothing)

        If Not view Is Nothing Then
            view.Controller = Me
            Me.view = view
        End If

    End Sub

    Friend Function Show(ByRef name As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_context", "Show", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            updated = False

            Dim styles As New bc_ao_word
            stylesCombo = New ComboBoxHelper

            saveType = eSaveType.eAdd

            ' disable clone as not available when adding a new context
            view.uxClone.Enabled = False
            'view.uxImport.Enabled = False
            'view.uxExport.Enabled = False
            view.uxDelete.Enabled = False
            view.uxRemove.Enabled = False
            view.uxAdd.Enabled = False
            view.uxDataItemSettings.Enabled = False

            'initalise the form
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Loading Data Definition...", 10, False, True)


            styles.get_styles_from_auto_text(stylesCombo)
            ' close word if no documents open
            If styles.document_count = 0 Then
                styles.quit()
            End If
            bc_cs_central_settings.progress_bar.increment("Loading Data Definition...")
            buildLookUpLists()
            bc_cs_central_settings.progress_bar.increment("Loading Data Definition...")

            loadDataModels()

            bc_cs_central_settings.progress_bar.unload()
            Application.DoEvents()
            view.ShowDialog()

            If updated Then
                name = view.uxContextName.Text
            End If
            If contextCount = 0 Then
                DeleteContext(view.Tag, True, False)
            End If

            Return updated

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "Show", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            log = New bc_cs_activity_log("bc_am_context", "Show", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Function Update(ByRef contextName As String, ByVal contextID As Integer) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_context", "Update", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim styles As New bc_ao_word
            stylesCombo = New ComboBoxHelper
            saveType = eSaveType.eEdit

            view.Tag = contextID

            view.Text = contextName

            view.uxDelete.Enabled = True

            view.uxDataItemSettings.Enabled = False

            ' initialise the form
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Loading Data Definition...", 10, False, True)

            styles.get_styles_from_auto_text(stylesCombo)
            ' close word if no documents open
            If styles.document_count = 0 Then
                styles.quit()
            End If

            bc_cs_central_settings.progress_bar.increment("Loading Data Definition...")

            buildLookUpLists()
            bc_cs_central_settings.progress_bar.increment("Loading Data Definition...")

            loadDataModels()
            bc_cs_central_settings.progress_bar.increment("Loading Data Definition...")

            loadContextConfiguration(contextID)
            bc_cs_central_settings.progress_bar.increment("Loading Data Definition...")

            view.uxContextName.Text = contextName
            'view.uxContextName.Enabled = False
            bc_cs_central_settings.progress_bar.increment("Loading Data Definition...")

            ' select the first datamodel in the list
            If view.uxDataModel.Items.Count > 0 Then
                view.uxDataModel.SelectedIndex = 0
                PopulateDataModelContextConfiguration()
            End If

            bc_cs_central_settings.progress_bar.unload()

            If view.Visible = False Then
                view.ShowDialog()
            End If

            If contextCount = 0 Then
                DeleteContext(view.Tag, True, False)
            End If

            Return updated

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "Update", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            log = New bc_cs_activity_log("bc_am_context", "Update", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Sub loadDataModels()

        Dim log = New bc_cs_activity_log("bc_am_context", "loadDataModels", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim osql As New bc_om_sql("exec bcc_core_bp_get_financial_templates")

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success = True Then
                Dim cbh As New ComboBoxHelper
                Dim i As Integer

                cbh.RemoveAt(0)

                For i = 0 To UBound(osql.results, 2)
                    cbh.Add(CType(osql.results(0, i), Integer), osql.results(1, i))
                Next

                view.uxDataModel.DataSource = cbh
                view.uxDataModel.DisplayMember = "Name"
                view.uxDataModel.ValueMember = "ID"

                If view.uxDataModel.Items.Count > 0 Then
                    view.uxDataModel.SelectedIndex = 0
                End If

            Else
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Loading data models failed.", bc_cs_message.MESSAGE)
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "loadDataModels", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "loadDataModels", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub


    Private Function contextExists(ByVal contextName As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_context", "contextExists", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer

            For i = 0 To bc_am_load_objects.obc_templates.component_types.component_types.Count - 1
                If LCase(bc_am_load_objects.obc_templates.component_types.component_types(i).component_name) = LCase(contextName) Then
                    contextExists = True
                    Exit For
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "contextExists", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "contextExists", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Function

    Private Sub populateAvailableDataItems()

        Dim log = New bc_cs_activity_log("bc_am_context", "populateAvailableDataItems", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim osql As bc_om_sql

            If InStr(view.uxDataModel.Text, " ") > 0 Then
                osql = New bc_om_sql(String.Concat("exec bcc_core_bp_get_all_data_items '", _
                                                            Left(view.uxDataModel.Text, InStr(view.uxDataModel.Text, " ") - 1), "'"))
            Else
                osql = New bc_om_sql(String.Concat("exec bcc_core_bp_get_all_data_items '", _
                                                                            view.uxDataModel.Text, "'"))
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success = True Then
                Dim di As New CustomHashTable
                Dim i As Integer

                view.uxAvailableDataItems.Items.Clear()
                view.uxSelectedDataItems.Items.Clear()

                ' add a default blank entry
                di.Add(blank_entry_id, blank_entry_value)
                di.Add(0, blank_entry)
                view.uxAvailableDataItems.Items.Add(di)

                di = New CustomHashTable

                ' store item information in hashtable before populating.
                For i = 0 To UBound(osql.results, 2)
                    di.Add(-1, CType(osql.results(0, i), Integer))
                    di.Add(0, osql.results(1, i))
                    di.Add(eContextAttributes.eLabelCode, osql.results(2, i)) ' label_code
                    di.Add(eContextAttributes.eScaleFactor, osql.results(3, i)) ' scale factor
                    di.Add(eContextAttributes.eLinkCode, osql.results(4, i)) ' link code 
                    di.Add(eContextAttributes.eFormatMask, osql.results(5, i)) ' format mask
                    view.uxAvailableDataItems.Items.Add(di)
                    di = New CustomHashTable
                Next

            Else
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Loading data items failed.", bc_cs_message.MESSAGE)
            End If

            If view.uxAvailableDataItems.Items.Count > 0 Then
                view.uxAvailableDataItems.SelectedIndex = 0
                view.uxAdd.Enabled = True
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "populateAvailableDataItems", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "populateAvailableDataItems", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub loadContextConfiguration(ByVal contextID As Integer)

        Dim log = New bc_cs_activity_log("bc_am_context", "loadContextConfiguration", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim osql As New bc_om_sql(String.Concat("exec bcc_core_bp_get_context_configuration ", contextID))

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success = True Then
                ' store context config for later use
                contextConfig = New DataItems
                Dim i As Integer
                If Not osql.results Is Nothing Then
                    For i = 0 To UBound(osql.results, 2)
                        contextConfig.Add(osql.results(0, i), IIf(Trim(osql.results(1, i)) = "", blank_entry, osql.results(1, i)), osql.results(2, i), osql.results(3, i), osql.results(4, i))
                        If osql.results(0, i) < 0 And osql.results(0, i) <= blank_entry_value Then
                            blank_entry_value = osql.results(0, i) - 1
                        End If
                    Next
                End If
            Else
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Loading context failed.", bc_cs_message.MESSAGE)
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "loadContextConfiguration", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "loadContextConfiguration", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub buildLookUpLists(Optional ByVal labelsOnly As Boolean = False)

        Dim log = New bc_cs_activity_log("bc_am_context", "buildLookUpLists", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim cbh As New ComboBoxHelper
            Dim osql As bc_om_sql

            If Not labelsOnly Then

                'Data Item Direction
                cbh.Add(0, "Horizontal")
                cbh.Add(1, "Vertical")
                view.uxDataItemDirection.DataSource = cbh
                view.uxDataItemDirection.DisplayMember = "Name"
                view.uxDataItemDirection.ValueMember = "ID"
                view.uxDataItemDirection.SelectedIndex = 0

                'Scale Factors
                osql = New bc_om_sql("exec bcc_core_bp_get_scale_factors")

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success = True Then
                    Dim i As Integer
                    cbh = New ComboBoxHelper
                    For i = 0 To UBound(osql.results, 2)
                        cbh.Add(CType(osql.results(0, i), Integer), osql.results(1, i))

                    Next
                    view.uxScaleFactor.DataSource = cbh
                    view.uxScaleFactor.DisplayMember = "Name"
                    view.uxScaleFactor.ValueMember = "ID"
                    view.uxScaleFactor.SelectedIndex = 0
                Else
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Loading scale factors failed.", bc_cs_message.MESSAGE)
                End If

                'Scale Symbols
                osql = New bc_om_sql("exec bcc_core_bp_get_scale_symbols")

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success = True Then
                    Dim i As Integer

                    cbh = New ComboBoxHelper
                    cbh.RemoveAt(0)
                    cbh.Add(0, "(Custom)")

                    For i = 0 To UBound(osql.results, 2)
                        cbh.Add(CType(osql.results(0, i), Integer), osql.results(1, i))
                    Next
                    view.uxScaleSymbol.DataSource = cbh
                    view.uxScaleSymbol.DisplayMember = "Name"
                    view.uxScaleSymbol.ValueMember = "ID"
                    view.uxScaleSymbol.SelectedIndex = 0
                Else
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Loading scale symbols failed.", bc_cs_message.MESSAGE)
                End If

                'Format Masks
                osql = New bc_om_sql("exec bcc_core_bp_get_format_masks")

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                defaultFormatMaskIndex = 0

                If osql.success = True Then
                    Dim i As Integer

                    cbh = New ComboBoxHelper
                    For i = 0 To UBound(osql.results, 2)
                        cbh.Add(CType(osql.results(0, i), Integer), osql.results(1, i))
                        If osql.results(2, i) = True Then ' default value
                            defaultFormatMaskIndex = i + 1
                        End If
                    Next
                    view.uxFormatMask.DataSource = cbh
                    view.uxFormatMask.DisplayMember = "Name"
                    view.uxFormatMask.ValueMember = "ID"
                    view.uxFormatMask.SelectedIndex = 0
                Else
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Loading format masks failed.", bc_cs_message.MESSAGE)
                End If

                'Styles - currently only supports getting styles from autotext            

                view.uxDataStyle.BindingContext = New BindingContext
                view.uxDataStyle.DataSource = stylesCombo
                view.uxDataStyle.DisplayMember = "Name"
                view.uxDataStyle.ValueMember = "ID"
                view.uxDataStyle.SelectedIndex = 0

                view.uxYearStyle.BindingContext = New BindingContext
                view.uxYearStyle.DataSource = stylesCombo
                view.uxYearStyle.DisplayMember = "Name"
                view.uxYearStyle.ValueMember = "ID"
                view.uxYearStyle.SelectedIndex = 0

                view.uxLabelStyle.BindingContext = New BindingContext
                view.uxLabelStyle.DataSource = stylesCombo
                view.uxLabelStyle.DisplayMember = "Name"
                view.uxLabelStyle.ValueMember = "ID"
                view.uxLabelStyle.SelectedIndex = 0

                view.uxTitleStyle.BindingContext = New BindingContext
                view.uxTitleStyle.DataSource = stylesCombo
                view.uxTitleStyle.DisplayMember = "Name"
                view.uxTitleStyle.ValueMember = "ID"
                view.uxTitleStyle.SelectedIndex = 0

                view.uxSourceStyle.BindingContext = New BindingContext
                view.uxSourceStyle.DataSource = stylesCombo
                view.uxSourceStyle.DisplayMember = "Name"
                view.uxSourceStyle.ValueMember = "ID"
                view.uxSourceStyle.SelectedIndex = 0

                view.uxYearToStyle.BindingContext = New BindingContext
                view.uxYearToStyle.DataSource = stylesCombo
                view.uxYearToStyle.DisplayMember = "Name"
                view.uxYearToStyle.ValueMember = "ID"
                view.uxYearToStyle.SelectedIndex = 0

                view.uxPeriodFormatStyle.BindingContext = New BindingContext
                view.uxPeriodFormatStyle.DataSource = stylesCombo
                view.uxPeriodFormatStyle.DisplayMember = "Name"
                view.uxPeriodFormatStyle.ValueMember = "ID"
                view.uxPeriodFormatStyle.SelectedIndex = 0

                'Period Formats
                osql = New bc_om_sql("exec bc_core_bp_get_period_formats")

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success = True Then
                    Dim i As Integer

                    cbh = New ComboBoxHelper

                    For i = 0 To UBound(osql.results, 2)
                        cbh.Add(CType(osql.results(0, i), Integer), osql.results(1, i))
                    Next
                    view.uxPeriodFormat.DataSource = cbh
                    view.uxPeriodFormat.DisplayMember = "Name"
                    view.uxPeriodFormat.ValueMember = "ID"
                    view.uxPeriodFormat.SelectedIndex = 0
                Else
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Loading period formats failed.", bc_cs_message.MESSAGE)
                End If
            End If

            'labels
            osql = New bc_om_sql("exec bcc_core_bp_get_label_values")

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success = True Then
                Dim i As Integer

                cbh = New ComboBoxHelper
                For i = 0 To UBound(osql.results, 2)
                    cbh.Add(CType(osql.results(0, i), String), osql.results(1, i))
                Next
                view.uxLabel.DataSource = cbh
                view.uxLabel.DisplayMember = "Name"
                view.uxLabel.ValueMember = "Code"
                view.uxLabel.SelectedIndex = 0
            Else
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Loading label values failed.", bc_cs_message.MESSAGE)
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "buildLookUpLists", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "buildLookUpLists", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub PopulateDataModelContextConfiguration()

        Dim log = New bc_cs_activity_log("bc_am_context", "PopulateDataModelContextConfiguration", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Cursor.Current = Cursors.WaitCursor

            Dim i As Integer
            Dim di As DataItems.DataItem
            Dim attributes As New CustomHashTable

            clearTableSettings()
            clearDataItemSettings(False)
            populateAvailableDataItems()

            For Each di In contextConfig
                If di.FinancialModelID = view.uxDataModel.SelectedValue Then ' check we have the correct datamodel
                    Select Case di.AttributeID
                        Case eContextAttributes.eHorizontalFlag ' horizontal flag
                            view.uxDataItemDirection.SelectedValue = CType(di.AttributeValue, Integer)
                        Case eContextAttributes.eNumberOfYears  ' no. of years
                            view.uxNumberOfYears.Text = di.AttributeValue
                        Case eContextAttributes.eYearRow ' year row
                            view.uxYearStartRow.Text = di.AttributeValue
                        Case eContextAttributes.eYearColumn   ' year column
                            view.uxYearStartColumn.Text = di.AttributeValue
                        Case eContextAttributes.eYearStyle  ' year style
                            view.uxYearStyle.Text = di.AttributeValue
                        Case eContextAttributes.eTitle
                            view.uxTitle.Text = di.AttributeValue
                        Case eContextAttributes.eSource
                            view.uxSource.Text = di.AttributeValue
                        Case eContextAttributes.eNumberOfActualYears
                            view.uxNumberOfActualYears.Text = di.AttributeValue
                        Case eContextAttributes.eTitleStyle
                            view.uxTitleStyle.Text = di.AttributeValue
                        Case eContextAttributes.eSourceStyle
                            view.uxSourceStyle.Text = di.AttributeValue
                        Case eContextAttributes.eYearToText
                            view.uxYearToText.Text = di.AttributeValue
                        Case eContextAttributes.eYearToStyle
                            view.uxYearToStyle.Text = di.AttributeValue
                        Case eContextAttributes.eEstimateIdentifier
                            view.uxEstimateIdentifier.Text = di.AttributeValue
                        Case eContextAttributes.eActualIdentifier
                            view.uxActualIdentifier.Text = di.AttributeValue
                        Case eContextAttributes.ePeriodFormat
                            view.uxPeriodFormat.SelectedValue = CType(di.AttributeValue, Integer)
                        Case eContextAttributes.ePeriodFormatStyle
                            view.uxPeriodFormatStyle.Text = di.AttributeValue
                        Case eContextAttributes.eDisplayPeriod
                            view.uxDisplayPeriodLabels.Checked = IIf(di.AttributeValue = 1, True, False)
                        Case eContextAttributes.ePeriodRow
                            view.uxPeriodStartRow.Text = di.AttributeValue
                        Case eContextAttributes.ePeriodColumn
                            view.uxPeriodStartCol.Text = di.AttributeValue
                    End Select

                    If di.AttributeID = 1 Then
                        attributes.Add(-1, di.RowID)
                        attributes.Add(0, di.Description)
                    End If
                    attributes.Add(CType(di.AttributeID, eContextAttributes), di.AttributeValue)

                    ' if last attribute then add the data item into the selected list
                    If di.AttributeID = 38 Then
                        Dim newIndex As Integer
                        newIndex = view.uxSelectedDataItems.Items.Add(attributes)
                        Dim index As Integer = -1
                        For i = -1 To view.uxAvailableDataItems.Items.Count
                            index = view.uxAvailableDataItems.FindStringExact(di.Description, index)
                            If index = -1 OrElse view.uxSelectedDataItems.Items.Item(newIndex)(-1) = view.uxAvailableDataItems.Items.Item(index)(-1) Then
                                Exit For
                            End If
                            i = i + 1
                        Next
                        If index <> -1 AndAlso view.uxAvailableDataItems.Items.Item(index)(blank_entry_id) <> blank_entry_value Then
                            view.uxSelectedDataItems.Items.Item(newIndex)(eContextAttributes.eLinkCode) = _
                                view.uxAvailableDataItems.Items.Item(index)(eContextAttributes.eLinkCode)
                            view.uxAvailableDataItems.Items.RemoveAt(index)
                        End If
                        attributes = New CustomHashTable
                    End If
                End If
            Next

            If view.uxSelectedDataItems.Items.Count > 0 Then
                view.uxSelectedDataItems.SelectedIndex = 0
                view.uxRemove.Enabled = True
                view.uxDataItemSettings.Enabled = True
                view.uxSelDataItems.Enabled = True
                view.uxDelete.Enabled = True
            Else
                view.uxRemove.Enabled = False
                view.uxDataItemSettings.Enabled = False
                view.uxSelDataItems.Enabled = False
                view.uxDelete.Enabled = False
            End If

            view.uxSave.Enabled = False

            If view.uxAvailableDataItems.Items.Count > 0 Then
                view.uxAvailableDataItems.SelectedIndex = 0
                view.uxAdd.Enabled = True
            End If

        Catch ex As Exception
            Cursor.Current = Cursors.Default
            Dim errLog As New bc_cs_error_log("bc_am_context", "PopulateDataModelContextConfiguration", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_context", "PopulateDataModelContextConfiguration", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub MoveDataItems(ByVal add As Boolean)

        Dim log = New bc_cs_activity_log("bc_am_context", "MoveDataItems", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim availableIndex As Integer
        Dim selectableIndex As Integer

        Try
            ' add or remove the selected data item
            If add Then
                If view.uxAvailableDataItems.SelectedItem(blank_entry_id) < 0 Then
                    Dim di = New CustomHashTable
                    di.Add(blank_entry_id, blank_entry_value)
                    di.Add(0, blank_entry)
                    Dim i As Integer
                    i = view.uxSelectedDataItems.Items.Add(di)
                    view.uxSelectedDataItems.SelectedItem = view.uxSelectedDataItems.Items.Item(i)
                    blank_entry_value = blank_entry_value - 1
                    view.uxAvailableDataItems.SelectedItem(blank_entry_id) = blank_entry_value
                Else
                    view.uxSelectedDataItems.Items.Add(view.uxAvailableDataItems.SelectedItem)
                    view.uxSelectedDataItems.SelectedItem = view.uxAvailableDataItems.SelectedItem
                End If

                ReOrderSelectedDataItems()
                PopulateFromSelectedDataItem()
                availableIndex = view.uxAvailableDataItems.SelectedIndex

                ' do no remove the blank entry
                If view.uxAvailableDataItems.SelectedItem(blank_entry_id) > 0 Then
                    view.uxAvailableDataItems.Items.Remove(view.uxAvailableDataItems.SelectedItem)
                End If
                If view.uxAvailableDataItems.Items.Count > 0 Then
                    Try
                        view.uxAvailableDataItems.SelectedIndex = availableIndex
                    Catch
                        view.uxAvailableDataItems.SelectedIndex = availableIndex - 1
                    End Try
                    view.uxAdd.Enabled = True
                    view.uxSelDataItems.Enabled = True
                    view.uxDataItemSettings.Enabled = True
                Else
                    view.uxAdd.Enabled = False
                    view.uxSelDataItems.Enabled = False
                    view.uxDataItemSettings.Enabled = False
                End If
            Else
                UpdateToSelectedDataItem()
                updateSelectedItem = False
                clearDataItemSettings(False)

                ' do not add the blank entry as it will already exist
                If view.uxSelectedDataItems.SelectedItem(blank_entry_id) < 0 Then
                    blank_entry_value = blank_entry_value + 1
                    Dim index As Integer
                    index = view.uxAvailableDataItems.FindStringExact(blank_entry)
                    view.uxAvailableDataItems.Items.Item(index)(blank_entry_id) = blank_entry_value
                Else
                    view.uxAvailableDataItems.Items.Add(view.uxSelectedDataItems.SelectedItem)
                End If

                selectableIndex = view.uxSelectedDataItems.SelectedIndex
                view.uxAvailableDataItems.SelectedItem = view.uxSelectedDataItems.SelectedItem
                view.uxSelectedDataItems.Items.Remove(view.uxSelectedDataItems.SelectedItem)
                ReOrderSelectedDataItems()

                If view.uxSelectedDataItems.Items.Count > 0 Then
                    Try
                        view.uxSelectedDataItems.SelectedIndex = selectableIndex
                    Catch
                        view.uxSelectedDataItems.SelectedIndex = selectableIndex - 1
                    End Try
                    'view.uxSelectedDataItems.SelectedIndex = 0
                    view.uxRemove.Enabled = True
                    view.uxDataItemSettings.Enabled = True
                    view.uxSelDataItems.Enabled = True
                Else
                    view.uxRemove.Enabled = False
                    view.uxDataItemSettings.Enabled = False
                    view.uxSelDataItems.Enabled = False
                End If
            End If

            Validate(False)

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "MoveDataItems", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            updateSelectedItem = True
            log = New bc_cs_activity_log("bc_am_context", "MoveDataItems", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Friend Sub UpdateToSelectedDataItem(Optional ByVal performValidate As Boolean = True)
        Dim log = New bc_cs_activity_log("bc_am_context", "UpdateToSelectedDataItem", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            ' populate the hashtable with the selected data items attributes
            If view.Visible And updateSelectedItem Then
                view.uxSelectedDataItems.SelectedItem(eContextAttributes.eLabelCode) = view.uxLabel.SelectedValue
                If view.uxScaleSymbol.SelectedIndex = 0 Then
                    view.uxSelectedDataItems.SelectedItem(eContextAttributes.eScaleSymbol) = view.uxSSCustom.Text
                Else
                    view.uxSelectedDataItems.SelectedItem(eContextAttributes.eScaleSymbol) = view.uxScaleSymbol.Text
                End If
                view.uxSelectedDataItems.SelectedItem(eContextAttributes.eScaleFactor) = view.uxScaleFactor.Text
                view.uxSelectedDataItems.SelectedItem(eContextAttributes.eRow) = view.uxDataStartRow.Text
                view.uxSelectedDataItems.SelectedItem(eContextAttributes.eColumn) = view.uxDataStartColumn.Text
                view.uxSelectedDataItems.SelectedItem(eContextAttributes.eDataStyle) = view.uxDataStyle.Text
                view.uxSelectedDataItems.SelectedItem(eContextAttributes.eLabelStyle) = view.uxLabelStyle.Text
                view.uxSelectedDataItems.SelectedItem(eContextAttributes.eFormatMask) = view.uxFormatMask.Text
                If performValidate Then
                    Validate(False)
                End If
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "UpdateToSelectedDataItem", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "UpdateToSelectedDataItem", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Friend Sub PopulateFromSelectedDataItem()
        Dim log = New bc_cs_activity_log("bc_am_context", "PopulateFromSelectedDataItem", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            updateSelectedItem = False

            view.uxDataItemSettings.Enabled = True

            clearDataItemSettings(True)

            ' populate the form with the selected data item values
            If Not view.uxSelectedDataItems.SelectedItem(eContextAttributes.eLabelCode) Is Nothing AndAlso _
                    Trim(view.uxSelectedDataItems.SelectedItem(eContextAttributes.eLabelCode)) <> "" Then
                view.uxLabel.SelectedValue = view.uxSelectedDataItems.SelectedItem(eContextAttributes.eLabelCode)
            End If
            If Not view.uxSelectedDataItems.SelectedItem(eContextAttributes.eScaleSymbol) Is Nothing AndAlso _
                    Trim(view.uxSelectedDataItems.SelectedItem(eContextAttributes.eScaleSymbol)) <> "" Then
                view.uxScaleSymbol.Text = view.uxSelectedDataItems.SelectedItem(eContextAttributes.eScaleSymbol)
                If view.uxScaleSymbol.SelectedIndex = 0 Then
                    view.uxSSCustom.Text = view.uxSelectedDataItems.SelectedItem(eContextAttributes.eScaleSymbol)
                End If
            End If
            If Not view.uxSelectedDataItems.SelectedItem(eContextAttributes.eScaleFactor) Is Nothing AndAlso _
                    Trim(view.uxSelectedDataItems.SelectedItem(eContextAttributes.eScaleFactor)) <> "" Then
                view.uxScaleFactor.Text = view.uxSelectedDataItems.SelectedItem(eContextAttributes.eScaleFactor)
            End If
            If Not view.uxSelectedDataItems.SelectedItem(eContextAttributes.eRow) Is Nothing AndAlso _
                    Trim(view.uxSelectedDataItems.SelectedItem(eContextAttributes.eRow)) <> "" Then
                view.uxDataStartRow.Text = view.uxSelectedDataItems.SelectedItem(eContextAttributes.eRow)
            End If
            If Not view.uxSelectedDataItems.SelectedItem(eContextAttributes.eColumn) Is Nothing AndAlso _
                    Trim(view.uxSelectedDataItems.SelectedItem(eContextAttributes.eColumn)) <> "" Then
                view.uxDataStartColumn.Text = view.uxSelectedDataItems.SelectedItem(eContextAttributes.eColumn)
            End If
            If Not view.uxSelectedDataItems.SelectedItem(eContextAttributes.eDataStyle) Is Nothing AndAlso _
                    Trim(view.uxSelectedDataItems.SelectedItem(eContextAttributes.eDataStyle)) <> "" Then
                view.uxDataStyle.Text = view.uxSelectedDataItems.SelectedItem(eContextAttributes.eDataStyle)
            End If
            If Not view.uxSelectedDataItems.SelectedItem(eContextAttributes.eLabelStyle) Is Nothing AndAlso _
                    Trim(view.uxSelectedDataItems.SelectedItem(eContextAttributes.eLabelStyle)) <> "" Then
                view.uxLabelStyle.Text = view.uxSelectedDataItems.SelectedItem(eContextAttributes.eLabelStyle)
            End If
            If Not view.uxSelectedDataItems.SelectedItem(eContextAttributes.eFormatMask) Is Nothing AndAlso _
                    Trim(view.uxSelectedDataItems.SelectedItem(eContextAttributes.eFormatMask)) <> "" Then
                view.uxFormatMask.Text = view.uxSelectedDataItems.SelectedItem(eContextAttributes.eFormatMask)
            End If

            view.uxRemove.Enabled = True

            If view.uxSelectedDataItems.SelectedIndex = 0 Then
                view.uxMoveUp.Enabled = False
            Else
                view.uxMoveUp.Enabled = True
            End If

            If view.uxSelectedDataItems.SelectedIndex = view.uxSelectedDataItems.Items.Count - 1 Then
                view.uxMoveDown.Enabled = False
            Else
                view.uxMoveDown.Enabled = True
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "PopulateFromSelectedDataItem", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            updateSelectedItem = True
            log = New bc_cs_activity_log("bc_am_context", "PopulateFromSelectedDataItem", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Private Sub clearDataItemSettings(ByVal setDefaults As Boolean)

        Dim log = New bc_cs_activity_log("bc_am_context", "clearDataItemSettings", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            view.uxLabel.SelectedIndex = 0
            If view.uxScaleSymbol.Enabled Then
                view.uxScaleSymbol.SelectedIndex = 0
                view.uxSSCustom.Enabled = True
                view.uxSSCustom.Text = ""
            End If
            If view.uxScaleFactor.Enabled Then
                view.uxScaleFactor.SelectedIndex = 0
            End If
            If view.uxDataItemDirection.Text = "Vertical" Then
                view.uxDataStartRow.Text = ""
                If view.uxDataStartColumn.Enabled Then
                    view.uxDataStartColumn.Text = "1"
                End If
            Else
                view.uxDataStartColumn.Text = ""
                If view.uxDataStartRow.Enabled Then
                    view.uxDataStartRow.Text = "1"
                End If
            End If
            If (Not setDefaults) Or (setDefaults _
                                    AndAlso Not view.uxSelectedDataItems.SelectedItem(eContextAttributes.eDataStyle) Is Nothing _
                                    AndAlso view.uxSelectedDataItems.SelectedItem(eContextAttributes.eDataStyle) <> "") Then
                view.uxDataStyle.SelectedIndex = 0
            Else
                view.uxSelectedDataItems.SelectedItem(eContextAttributes.eDataStyle) = view.uxDataStyle.Text
            End If
            If (Not setDefaults) Or (setDefaults _
                                    AndAlso Not view.uxSelectedDataItems.SelectedItem(eContextAttributes.eLabelStyle) Is Nothing _
                                    AndAlso view.uxSelectedDataItems.SelectedItem(eContextAttributes.eLabelStyle) <> "") Then
                view.uxLabelStyle.SelectedIndex = 0
            Else
                view.uxSelectedDataItems.SelectedItem(eContextAttributes.eLabelStyle) = view.uxLabelStyle.Text
            End If
            If view.uxFormatMask.Enabled Then
                view.uxFormatMask.SelectedIndex = defaultFormatMaskIndex
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "clearDataItemSettings", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "clearDataItemSettings", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub clearTableSettings()

        Dim log = New bc_cs_activity_log("bc_am_context", "clearTableSettings", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            view.uxDataItemDirection.SelectedIndex = 0
            view.uxYearStyle.SelectedIndex = 0
            view.uxYearStartRow.Text = ""
            view.uxYearStartColumn.Text = ""
            view.uxNumberOfYears.Text = ""
            view.uxNumberOfActualYears.Text = ""
            view.uxTitle.Text = ""
            view.uxTitleStyle.SelectedIndex = 0
            view.uxSource.Text = ""
            view.uxSourceStyle.SelectedIndex = 0
            view.uxYearToText.Text = ""
            view.uxYearToStyle.SelectedIndex = 0
            view.uxEstimateIdentifier.Text = ""
            view.uxActualIdentifier.Text = ""
            view.uxPeriodFormat.SelectedIndex = 0
            view.uxPeriodFormatStyle.SelectedIndex = 0
            view.uxDisplayPeriodLabels.Checked = False
            view.uxPeriodStartRow.Text = ""
            view.uxPeriodStartCol.Text = ""

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "clearTableSettings", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "clearTableSettings", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub Save()

        Dim log = New bc_cs_activity_log("bc_am_context", "Save", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim osql As bc_om_sql = Nothing
            Dim sql As New StringBuilder
            Dim attributes As CustomHashTable
            Dim attributeValues As String = ""
            Dim rowIDs As String = ""
            Dim i As Integer
            Dim j As Integer

            If Not Validate(True) Then
                Exit Try
            End If

            If saveType = eSaveType.eAdd Or view.uxContextName.Text <> view.Text Then
                osql = New bc_om_sql(String.Concat("bcc_core_bp_context_exists '", view.uxContextName.Text, "'"))
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success = True Then
                    If osql.results(0, 0) > 0 Then
                        Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Context already exists.", bc_cs_message.MESSAGE)
                        Exit Try
                    End If
                Else
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Context already exists.", bc_cs_message.MESSAGE)
                    Exit Try
                End If
            End If

            'loop through all the selected items and build up delimited strings
            'all strings delimited by pipe along with the attribute id/value delimeted by "!"
            For i = 0 To view.uxSelectedDataItems.Items.Count - 1
                attributes = view.uxSelectedDataItems.Items(i)
                For j = 1 To 38
                    If (j > 8 And j < 14) Or j > 24 Then
                        Select Case j
                            Case eContextAttributes.eHorizontalFlag
                                attributeValues = String.Concat(attributeValues, j, "!", view.uxDataItemDirection.SelectedValue)
                            Case eContextAttributes.eNumberOfYears
                                attributeValues = String.Concat(attributeValues, j, "!", view.uxNumberOfYears.Text)
                            Case eContextAttributes.eYearRow
                                attributeValues = String.Concat(attributeValues, j, "!", view.uxYearStartRow.Text)
                            Case eContextAttributes.eYearColumn
                                attributeValues = String.Concat(attributeValues, j, "!", view.uxYearStartColumn.Text)
                            Case eContextAttributes.eYearStyle
                                attributeValues = String.Concat(attributeValues, j, "!", view.uxYearStyle.Text)
                            Case eContextAttributes.eTitle
                                attributeValues = String.Concat(attributeValues, j, "!", Replace(view.uxTitle.Text, "'", "''"))
                            Case eContextAttributes.eSource
                                attributeValues = String.Concat(attributeValues, j, "!", Replace(view.uxSource.Text, "'", "''"))
                            Case eContextAttributes.eNumberOfActualYears
                                attributeValues = String.Concat(attributeValues, j, "!", view.uxNumberOfActualYears.Text)
                            Case eContextAttributes.eTitleStyle
                                attributeValues = String.Concat(attributeValues, j, "!", Replace(view.uxTitleStyle.Text, "'", "''"))
                            Case eContextAttributes.eSourceStyle
                                attributeValues = String.Concat(attributeValues, j, "!", Replace(view.uxSourceStyle.Text, "'", "''"))
                            Case eContextAttributes.eYearToText
                                attributeValues = String.Concat(attributeValues, j, "!", Replace(view.uxYearToText.Text, "'", "''"))
                            Case eContextAttributes.eYearToStyle
                                attributeValues = String.Concat(attributeValues, j, "!", Replace(view.uxYearToStyle.Text, "'", "''"))
                            Case eContextAttributes.eEstimateIdentifier
                                attributeValues = String.Concat(attributeValues, j, "!", Replace(view.uxEstimateIdentifier.Text, "'", "''"))
                            Case eContextAttributes.eActualIdentifier
                                attributeValues = String.Concat(attributeValues, j, "!", Replace(view.uxActualIdentifier.Text, "'", "''"))
                            Case eContextAttributes.ePeriodFormat
                                attributeValues = String.Concat(attributeValues, j, "!", Replace(view.uxPeriodFormat.SelectedValue, "'", "''"))
                            Case eContextAttributes.ePeriodFormatStyle
                                attributeValues = String.Concat(attributeValues, j, "!", Replace(view.uxPeriodFormatStyle.Text, "'", "''"))
                            Case eContextAttributes.eDisplayPeriod
                                attributeValues = String.Concat(attributeValues, j, "!", IIf(view.uxDisplayPeriodLabels.Checked, 1, 0))
                            Case eContextAttributes.ePeriodRow
                                attributeValues = String.Concat(attributeValues, j, "!", view.uxPeriodStartRow.Text)
                            Case eContextAttributes.ePeriodColumn
                                attributeValues = String.Concat(attributeValues, j, "!", view.uxPeriodStartCol.Text)

                        End Select
                        attributeValues = String.Concat(attributeValues, "|")
                    Else
                        If j <> 4 And j < 15 Then  ' ignore certain attributes
                            attributeValues = String.Concat(attributeValues, j, "!", attributes(CType(j, eContextAttributes)))
                            attributeValues = String.Concat(attributeValues, "|")
                        End If
                    End If
                Next
                rowIDs = String.Concat(rowIDs, attributes(-1), "|")
            Next

            ' either insert or update
            Select Case saveType
                Case eSaveType.eAdd
                    With sql
                        .Append("exec bcc_core_bp_insert_context_configuration ")
                        .Append("'")
                        .Append(Trim(Replace(view.uxContextName.Text, "'", "''")))
                        .Append("',")
                        .Append(view.uxDataModel.SelectedValue)
                        .Append(",'")
                        .Append(Left(rowIDs, Len(rowIDs) - 1))
                        .Append("','")
                        .Append(Left(attributeValues, Len(attributeValues) - 1))
                        .Append("'")
                    End With

                    osql = New bc_om_sql(sql.ToString)
                Case eSaveType.eEdit
                    With sql
                        .Append("exec bcc_core_bp_update_context_configuration ")
                        .Append(view.Tag)
                        .Append(",")
                        .Append(view.uxDataModel.SelectedValue)
                        .Append(",'")
                        .Append(Left(rowIDs, Len(rowIDs) - 1))
                        .Append("','")
                        .Append(Left(attributeValues, Len(attributeValues) - 1))
                        .Append("','")
                        .Append(Trim(Replace(view.uxContextName.Text, "'", "''")))
                        .Append("'")
                    End With

                    osql = New bc_om_sql(sql.ToString)
            End Select

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success = True Then
                view.uxSave.Enabled = False
                view.uxDelete.Enabled = True
                view.uxClone.Enabled = True
                updated = True
                If saveType = eSaveType.eAdd Then
                    ' store the new context id for later use
                    view.Tag = osql.results(0, 0)
                    'view.uxContextName.Enabled = False
                    'set the save type to update for any further commits
                    saveType = eSaveType.eEdit
                    view.Text = view.uxContextName.Text
                End If
                'reload the data into memory
                loadContextConfiguration(view.Tag)
            Else
                Dim errLog As New bc_cs_error_log("bc_am_context", "Save", bc_cs_error_codes.USER_DEFINED, "Failed to save context.")
                Exit Try
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "Save", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "Save", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub DeleteContext(ByVal contextId As Integer, ByVal rootContext As Boolean, ByVal deleteAllDataModels As Boolean)

        Dim log = New bc_cs_activity_log("bc_am_context", "DeleteContext", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim osql As bc_om_sql

            If rootContext Then
                If deleteAllDataModels Then
                    osql = New bc_om_sql(String.Concat("exec bcc_core_bp_delete_context ", contextId, ", 1"))
                Else
                    osql = New bc_om_sql(String.Concat("exec bcc_core_bp_delete_context ", contextId, ", 0"))
                End If


                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success = True Then

                Else
                    Dim errLog As New bc_cs_error_log("bc_am_context", "Delete", bc_cs_error_codes.USER_DEFINED, "Failed to delete context.")
                    Exit Try
                End If
            Else
                If MessageBox.Show("Are you sure you wish to delete this data model?", "Delete Data Model", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    osql = New bc_om_sql(String.Concat("exec bcc_core_bp_delete_context_configuration ", contextId, ",", view.uxDataModel.SelectedValue))

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        osql.tmode = bc_cs_soap_base_class.tREAD
                        osql.transmit_to_server_and_receive(osql, True)
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        osql.db_read()
                    End If

                    If osql.success = True Then
                        clearTableSettings()
                        clearDataItemSettings(False)
                        view.uxSelectedDataItems.Items.Clear()
                        view.uxSave.Enabled = False
                        view.uxDelete.Enabled = False
                        updated = True
                        contextCount = osql.results(0, 0)
                        loadContextConfiguration(view.Tag)
                    Else
                        Dim errLog As New bc_cs_error_log("bc_am_context", "Delete", bc_cs_error_codes.USER_DEFINED, "Failed to delete context.")
                        Exit Try
                    End If
                End If
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "DeleteContext", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "DeleteContext", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub CloneDataModel()

        Dim log = New bc_cs_activity_log("bc_am_context", "CloneDataModel", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim viewClone As New bc_am_bp_data_model_clone
            Dim dataModelClone As New bc_am_clone(viewClone)
            Dim dataModelID As Integer

            dataModelID = dataModelClone.Show(view.uxDataModel.Text, view.uxDataModel.SelectedValue, _
                                                CType(view.uxDataModel.DataSource, ComboBoxHelper), view.Tag)

            If dataModelID <> -1 Then
                loadContextConfiguration(view.Tag)
                view.uxDataModel.SelectedValue = dataModelID
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "CloneDataModel", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "CloneDataModel", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Function CloneContext(ByVal contextId As Integer) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_context", "CloneContext", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim viewClone As New bc_am_bp_context_clone
            Dim contextClone As New bc_am_clone(viewClone)

            CloneContext = contextClone.Show(contextId)

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "CloneContext", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "CloneContext", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function


    Friend Function Validate(ByVal saveContext As Boolean) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_context", "Validate", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer
            Dim di As CustomHashTable

            Validate = True
            view.uxSave.Enabled = False

            If Trim(view.uxNumberOfYears.Text) = "" Or _
                Trim(view.uxNumberOfActualYears.Text) = "" Or _
                Trim(view.uxYearStartRow.Text) = "" Or _
                Trim(view.uxYearStartColumn.Text) = "" Or _
                Trim(view.uxContextName.Text) = "" Then
                Validate = False
                Exit Try
            End If

            If view.uxDisplayPeriodLabels.Checked Then
                If Trim(view.uxPeriodStartRow.Text) = "" Or _
                    Trim(view.uxPeriodStartCol.Text) = "" Or _
                    view.uxPeriodFormatStyle.SelectedIndex = 0 Then
                    Validate = False
                End If
            End If

            If view.uxDataItemDirection.SelectedIndex = 0 Or view.uxYearStyle.SelectedIndex = 0 Or _
                view.uxPeriodFormat.SelectedIndex = 0 Then
                Validate = False
                Exit Try
            End If

            If view.uxSelectedDataItems.Items.Count = 0 Then
                Validate = False
                Exit Try
            End If

            If saveContext Then
                For i = 0 To view.uxSelectedDataItems.Items.Count - 1
                    di = view.uxSelectedDataItems.Items(i)
                    If di(blank_entry_id) > 0 Then ' do not validate blank entries
                        If Trim(di(eContextAttributes.eLabelCode)) = "" Or _
                            Trim(di(eContextAttributes.eScaleFactor)) = "" Or _
                            Trim(di(eContextAttributes.eRow)) = "" Or Trim(di(eContextAttributes.eColumn)) = "" Or _
                            Trim(di(eContextAttributes.eDataStyle)) = "" Or Trim(di(eContextAttributes.eLabelStyle)) = "" Or _
                            Trim(di(eContextAttributes.eFormatMask)) = "" Then
                            If MessageBox.Show(String.Concat("Context Configuration is in-complete.  This will cause errors for document generation.", _
                                                                vbCrLf, "Are you sure you wish to continue"), _
                                                                "Save Context", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) = DialogResult.Yes Then
                                Validate = True
                            Else
                                Validate = False
                            End If
                            view.uxSave.Enabled = True
                            view.uxClone.Enabled = False
                            Exit Try
                        End If
                    End If
                Next
            End If

            If Validate = True Then
                view.uxSave.Enabled = True
                view.uxClone.Enabled = False
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "Validate", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "Validate", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Sub MoveSelectedDataItem(ByVal up As Boolean, Optional ByVal index As Integer = -1)

        Dim log = New bc_cs_activity_log("bc_am_context", "MoveSelectedDataItem", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim di As CustomHashTable

            If up Then
                di = view.uxSelectedDataItems.SelectedItem
                If index = -1 Then
                    index = view.uxSelectedDataItems.SelectedIndex - 1
                End If
            Else
                di = view.uxSelectedDataItems.SelectedItem
                If index = -1 Then
                    index = view.uxSelectedDataItems.SelectedIndex + 1
                End If
            End If

            If index = -1 Or index > view.uxSelectedDataItems.Items.Count - 1 Then
                If view.uxDataItemDirection.Text = "Vertical" Then
                    MessageBox.Show("Invalid Start Row, order not changed", "Data Definition", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    MessageBox.Show("Invalid Start Column, order not changed", "Data Definition", MessageBoxButtons.OK, MessageBoxIcon.Error)
                End If
                ReOrderSelectedDataItems()
                PopulateFromSelectedDataItem()
            Else
                view.uxSelectedDataItems.Items.RemoveAt(view.uxSelectedDataItems.SelectedIndex)
                view.uxSelectedDataItems.Items.Insert(index, di)
                ReOrderSelectedDataItems()
                view.uxSelectedDataItems.SelectedItem = view.uxSelectedDataItems.Items(index)
                Validate(False)
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "MoveSelectedDataItem", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "MoveSelectedDataItem", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Sub

    Private Sub ReOrderSelectedDataItems()

        Dim log = New bc_cs_activity_log("bc_am_context", "ReOrderSelectedDataItems", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer

            For i = 0 To view.uxSelectedDataItems.Items.Count - 1
                If view.uxDataItemDirection.Text = "Vertical" Then
                    view.uxSelectedDataItems.Items(i)(eContextAttributes.eRow) = i + 1
                Else
                    view.uxSelectedDataItems.Items(i)(eContextAttributes.eColumn) = i + 1
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "ReOrderSelectedDataItems", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "ReOrderSelectedDataItems", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub Export(ByVal contextId As Integer)

        Dim log = New bc_cs_activity_log("bc_am_context", "Export", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            ' set title and default path
            view.uxSaveFileDlg.Title = "Export"
            view.uxSaveFileDlg.InitialDirectory = bc_cs_central_settings.local_template_path
            view.uxSaveFileDlg.FileName = ""
            view.uxSaveFileDlg.Filter = "XML Files (*.xml)|*.xml"
            If view.uxSaveFileDlg.ShowDialog() = DialogResult.OK Then
                Dim osql As New bc_om_sql(String.Concat("bcc_core_bp_export_context ", contextId))

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success Then
                    Dim sw As StreamWriter
                    sw = File.CreateText(String.Concat(Environment.CurrentDirectory, "\", _
                                                Mid(view.uxSaveFileDlg.FileName, InStrRev(view.uxSaveFileDlg.FileName, "\") + 1)))
                    sw.Write(osql.results(0, 0))
                    sw.Close()
                Else
                    Dim errLog As New bc_cs_error_log("bc_am_context", "Export", bc_cs_error_codes.USER_DEFINED, "Data Definition Export Failed")
                    Exit Try
                End If
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Data Definition Export Completed.", bc_cs_message.MESSAGE)
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "Export", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "Export", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub Import()

        Dim log = New bc_cs_activity_log("bc_am_context", "Import", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            ' set title and default path
            view.uxOpenFileDlg.Title = "Import"
            view.uxOpenFileDlg.InitialDirectory = bc_cs_central_settings.local_template_path
            view.uxOpenFileDlg.FileName = ""
            view.uxOpenFileDlg.Filter = "XML Files (*.xml)|*.xml"
            If view.uxOpenFileDlg.ShowDialog() = DialogResult.OK Then

                Dim xmlDoc As New XmlDocument

                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Importing...", 10, False, True)
                Cursor.Current = Cursors.WaitCursor

                xmlDoc.Load(String.Concat(Environment.CurrentDirectory, "\", Mid(view.uxOpenFileDlg.FileName, InStrRev(view.uxOpenFileDlg.FileName, "\") + 1)))

                Dim osql As New bc_om_sql(String.Concat("bcc_core_bp_import_context 0, ", "'", Replace(xmlDoc.OuterXml, "'", "''"), "'"))

                bc_cs_central_settings.progress_bar.increment("Importing...")

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                bc_cs_central_settings.progress_bar.increment("Importing...")

                If osql.success Then
                    If osql.results(0, 0) = -2 Then ' invalid format import file
                        Dim omsg As New bc_cs_message("BluePrint - Blue Curve", "Import file is an invalid format.", bc_cs_message.MESSAGE)
                        Exit Try
                    End If
                    If osql.results(0, 0) = -1 Then ' template already exists
                        If MessageBox.Show("Data Definition already exists.  Do you wish to overwrite?", "Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                            osql = New bc_om_sql(String.Concat("bcc_core_bp_import_context 1, ", "'", Replace(xmlDoc.OuterXml, "'", "''"), "'"))

                            bc_cs_central_settings.progress_bar.increment("Importing...")

                            ' import again specifying overwrite

                            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                                osql.tmode = bc_cs_soap_base_class.tREAD
                                osql.transmit_to_server_and_receive(osql, True)
                            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                osql.db_read()
                            End If
                            If Not osql.success Then
                                Dim errLog As New bc_cs_error_log("bc_am_context", "Import", bc_cs_error_codes.USER_DEFINED, "Context Import Failed")
                                Exit Try
                            End If
                        Else
                            Exit Try
                        End If
                    End If

                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Data Defintion Import Completed.", bc_cs_message.MESSAGE)
                Else
                    Dim errLog As New bc_cs_error_log("bc_am_context", "Import", bc_cs_error_codes.USER_DEFINED, "Context Import Failed")
                End If
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "Import", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_context", "Import", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub AddNewLabel(Optional ByVal LabelDescription As String = "", Optional ByVal LabelValue As String = "")

        Dim log = New bc_cs_activity_log("bc_am_context", "AddNewLabel", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            labelView = New bc_am_bp_add_label
            labelView.Controller = Me

            labelView.uxOK.Enabled = False

            If Not LabelDescription = "" Then
                labelView.uxLabelDescription.Text = LabelDescription
            End If

            If Not LabelValue = "" Then
                labelView.uxLabelValue.Text = LabelValue
            End If

            If labelView.ShowDialog() = DialogResult.OK Then

                Dim sql As New StringBuilder

                With sql
                    .Append("bcc_core_bp_add_label 'at.create.") ' prefix with at.create
                    .Append(labelView.uxLabelCode.Text)
                    .Append("', '")
                    .Append(Replace(labelView.uxLabelDescription.Text, "'", "''"))
                    .Append("', '")
                    .Append(Replace(labelView.uxLabelValue.Text, "'", "''"))
                    .Append("'")
                End With

                Dim osql As New bc_om_sql(sql.ToString)

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success Then
                    If Not osql.results Is Nothing AndAlso osql.results(0, 0) = -1 Then
                        Dim omessage As New bc_cs_message("Add Label", "Label already exists.", bc_cs_message.MESSAGE)
                        AddNewLabel(labelView.uxLabelDescription.Text, labelView.uxLabelValue.Text)
                        Exit Try
                    Else
                        'refresh label list
                        buildLookUpLists(True)
                        view.uxLabel.Text = labelView.uxLabelValue.Text
                    End If
                Else
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Failed to create new label.", bc_cs_message.MESSAGE)
                    Exit Try
                End If
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "AddNewLabel", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "AddNewLabel", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub EditLabel()

        Dim log = New bc_cs_activity_log("bc_am_context", "EditLabel", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            labelView = New bc_am_bp_add_label
            labelView.Controller = Me

            If view.uxLabel.SelectedIndex = 0 Then ' no label selected
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "No label selected.", bc_cs_message.MESSAGE)
                Exit Try
            End If

            'If InStr(view.uxLabel.SelectedValue, "at.create") = 0 Then
            '    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Cannot edit a system label.  Click Yes if you wish to create a new label based on the system label.", bc_cs_message.MESSAGE, True, False, "Yes", "No", False)
            '    If omessage.cancel_selected = False Then
            '        AddNewLabel(view.uxLabel.SelectedValue.Text, view.uxLabel.Text)
            '        Exit Try
            '    Else
            '        Exit Try
            '    End If
            'End If

            Dim sql As New StringBuilder
            With sql
                .Append("bcc_core_bp_get_label '")
                .Append(view.uxLabel.SelectedValue)
                .Append("'")
            End With

            Dim osql As New bc_om_sql(sql.ToString)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success Then
                'Sam 2.1.12 Edit System Label
                If InStr(view.uxLabel.SelectedValue, "at.create") = 0 Then
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Cannot edit a system label." + vbCrLf + "Click 'Yes' if you wish to create a new label based on the system label.", bc_cs_message.MESSAGE, True, False, "Yes", "No", False)
                    If omessage.cancel_selected = False Then
                        AddNewLabel(osql.results(0, 0), osql.results(1, 0))
                        Exit Try
                    Else
                        Exit Try
                    End If
                End If

                labelView.uxLabelCode.Text = view.uxLabel.SelectedValue
                labelView.uxLabelCode.Enabled = False
                labelView.uxLabelDescription.Text = osql.results(0, 0)
                labelView.uxLabelValue.Text = osql.results(1, 0)
                labelView.uxOK.Enabled = False
            Else
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Failed to load label.", bc_cs_message.MESSAGE)
                Exit Try
            End If

            If labelView.ShowDialog() = DialogResult.OK Then

                sql = New StringBuilder

                With sql
                    .Append("bcc_core_bp_edit_label '")
                    .Append(labelView.uxLabelCode.Text)
                    .Append("', '")
                    .Append(labelView.uxLabelDescription.Text)
                    .Append("', '")
                    .Append(labelView.uxLabelValue.Text)
                    .Append("'")
                End With

                osql = New bc_om_sql(sql.ToString)

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success Then
                    'refresh label list
                    buildLookUpLists(True)
                    view.uxLabel.Text = labelView.uxLabelValue.Text
                Else
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Failed to edit label.", bc_cs_message.MESSAGE)
                    Exit Try
                End If
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "EditLabel", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "EditLabel", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub ValidateLabel()

        Dim log = New bc_cs_activity_log("bc_am_context", "ValidateLabel", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            If Trim(labelView.uxLabelCode.Text) = "" Or _
                Trim(labelView.uxLabelDescription.Text) = "" Or _
                Trim(labelView.uxLabelValue.Text) = "" Then
                labelView.uxOK.Enabled = False
            Else
                labelView.uxOK.Enabled = True
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "ValidateLabel", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "ValidateLabel", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Sub

    Friend Sub PopulateLinkCode()

        Dim log = New bc_cs_activity_log("bc_am_context", "PopulateLinkCode", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            If Not view.uxAvailableDataItems.SelectedItem Is Nothing Then
                view.uxAvailLinkCode.Text = view.uxAvailableDataItems.SelectedItem(eContextAttributes.eLinkCode)
            End If

            If Not view.uxSelectedDataItems.SelectedItem Is Nothing Then
                view.uxSelLinkCode.Text = view.uxSelectedDataItems.SelectedItem(eContextAttributes.eLinkCode)
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_context", "PopulateLinkCode", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_context", "PopulateLinkCode", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Class DataItems
        Inherits CollectionBase

        Public Sub New()

        End Sub

        Default Public ReadOnly Property item(ByVal index As Integer) As DataItem
            Get
                Return CType(Me.List(index), DataItem)
            End Get
        End Property

        Public Function Add(ByVal rowID As Integer, ByVal description As String) As Integer

            Dim di As New DataItem

            With di
                .RowID = rowID
                .Description = description
            End With

            Return list.Add(di)

        End Function

        Public Function Add(ByVal rowID As Integer, ByVal description As String, ByVal attributeID As Integer, _
                        ByVal attributeValue As String, ByVal financialModelID As Integer) As Integer

            Dim di As New DataItem

            With di
                .RowID = rowID
                .Description = description
                .AttributeID = attributeID
                .AttributeValue = attributeValue
                .FinancialModelID = financialModelID
            End With

            Return list.Add(di)

        End Function

        Public Class DataItem

            Private _rowID As Integer
            Private _description As String
            Private _attributeID As Integer
            Private _attributeValue As String
            Private _financialModelID As Integer

            Public Sub New()

            End Sub

            Public Property RowID() As Integer
                Get
                    RowID = _rowID
                End Get
                Set(ByVal Value As Integer)
                    _rowID = Value
                End Set
            End Property

            Public Property Description() As String
                Get
                    Description = _description
                End Get
                Set(ByVal Value As String)
                    _description = Value
                End Set
            End Property

            Public Property AttributeID() As Integer
                Get
                    AttributeID = _attributeID
                End Get
                Set(ByVal Value As Integer)
                    _attributeID = Value
                End Set
            End Property

            Public Property AttributeValue() As String
                Get
                    AttributeValue = _attributeValue
                End Get
                Set(ByVal Value As String)
                    _attributeValue = Value
                End Set
            End Property

            Public Property FinancialModelID() As Integer
                Get
                    FinancialModelID = _financialModelID
                End Get
                Set(ByVal Value As Integer)
                    _financialModelID = Value
                End Set
            End Property

            Public Overrides Function ToString() As String

                Return Description

            End Function

        End Class
    End Class

    Private Class CustomHashTable
        Inherits Hashtable

        Public Overrides Function ToString() As String
            Return Me(0)
        End Function
    End Class

End Class

'==========================================
' Bluecurve Limited 2008
' Module:         BluePrint
' Type:           AM
' Desciption:     data model clone form
'                 controller
' Public Methods: Show
'  
' Version:        1.0
' Change history:
'
'==========================================

Friend Class bc_am_clone

    Private view As bc_am_bp_data_model_clone
    Private viewContextClone As bc_am_bp_context_clone
    Private viewComponentClone As bc_am_bp_component_clone

    Friend Sub New(ByVal view As bc_am_bp_data_model_clone)

        view.Controller = Me
        Me.view = view

    End Sub

    Friend Sub New(ByVal view As bc_am_bp_context_clone)

        view.Controller = Me
        Me.viewContextClone = view

    End Sub


    Friend Sub New(ByVal view As bc_am_bp_component_clone)

        view.Controller = Me
        Me.viewComponentClone = view

    End Sub


    'Sam 2.1.3 Cloning Components
    Friend Function Show(ByVal DataModel As String, ByVal currentFinancialModelID As Integer, _
                            ByVal cbh As ComboBoxHelper, ByVal contextID As Integer) As Integer

        Dim log = New bc_cs_activity_log("bc_am_clone", "show", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            populateDataModels(currentFinancialModelID, cbh)
            view.Text = DataModel
            view.Tag = contextID
            If view.ShowDialog = DialogResult.OK Then
                'If MessageBox.Show("This will overwrite any existing configuration for this Data Model.  Are you sure?", "Clone", _
                '                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                If MessageBox.Show("You are about to clone data model """ + view.Text + """ to data model """ + view.uxDataModels.SelectedItems(0).Text + """.  Are you sure?", "Clone", _
                                    MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                    Show = CloneModel(currentFinancialModelID)
                Else
                    Show = -1
                End If
            Else
                Show = -1
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_clone", "show", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_clone", "show", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Function Show(ByVal contextId As Integer) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_clone", "show", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim osql As bc_om_sql

            If viewContextClone.ShowDialog = DialogResult.OK Then
                osql = New bc_om_sql(String.Concat("bcc_core_bp_context_exists '", viewContextClone.uxContextName.Text, "'"))
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success = True Then
                    If osql.results(0, 0) > 0 Then
                        Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Context already exists.", bc_cs_message.MESSAGE)
                        Exit Try
                    End If
                Else
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Cloning context failed.", bc_cs_message.MESSAGE)
                    Exit Try
                End If

                Show = CloneContext(contextId)
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_clone", "show", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_clone", "show", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    'Sam 2.1.3 Cloning Components
    Friend Function ShowComponent(ByVal componentId As Integer) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_clone", "show", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim osql As bc_om_sql

            If viewComponentClone.ShowDialog = DialogResult.OK Then
                osql = New bc_om_sql(String.Concat("bcc_core_bp_component_exists '", viewComponentClone.uxComponentName.Text, "'"))
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success = True Then
                    If osql.results(0, 0) > 0 Then
                        Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Component already exists.", bc_cs_message.MESSAGE)
                        Exit Try
                    End If
                Else
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Cloning Component failed.", bc_cs_message.MESSAGE)
                    Exit Try
                End If

                ShowComponent = True 'CloneComponent(componentId)
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_clone", "show", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_clone", "show", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function


    Private Function CloneModel(ByVal currentFinancialModelID As Integer) As Integer

        Dim log = New bc_cs_activity_log("bc_am_clone", "CloneModel", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim sql As New StringBuilder

            With sql
                .Append("exec bcc_core_bp_clone_context_configuration ")
                .Append(view.Tag) ' context id
                .Append(",")
                .Append(currentFinancialModelID)
                .Append(",")
                .Append(view.uxDataModels.SelectedItems(0).Tag)
            End With

            Dim osql As New bc_om_sql(sql.ToString)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success = True Then
                CloneModel = view.uxDataModels.SelectedItems(0).Tag
            Else
                Dim errLog As New bc_cs_error_log("bc_am_clone", "CloneModel", bc_cs_error_codes.USER_DEFINED, "Failed to clone model.")
                CloneModel = -1
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_clone", "CloneModel", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_clone", "CloneModel", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Sub populateDataModels(ByVal currentFinancialModelID As Integer, ByVal cbh As ComboBoxHelper)

        Dim log = New bc_cs_activity_log("bc_am_clone", "populateDataModels", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer
            Dim lvi As ListViewItem

            For i = 0 To cbh.Count - 1
                If cbh(i).ID <> currentFinancialModelID And cbh(i).ID <> -1 Then
                    lvi = view.uxDataModels.Items.Add(cbh(i).Name)
                    lvi.Tag = cbh(i).ID
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_clone", "populateDataModels", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_clone", "populateDataModels", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Function CloneContext(ByVal contextID As Integer) As Integer

        Dim log = New bc_cs_activity_log("bc_am_clone", "CloneContext", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim sql As New StringBuilder

            With sql
                .Append("exec bcc_core_bp_clone_context ")
                .Append(contextID)
                .Append(",'")
                .Append(Replace(viewContextClone.uxContextName.Text, "'", "''"))
                .Append("'")
            End With

            Dim osql As New bc_om_sql(sql.ToString)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success = True Then
                CloneContext = True
            Else
                Dim errLog As New bc_cs_error_log("bc_am_clone", "CloneContext", bc_cs_error_codes.USER_DEFINED, "Failed to clone context.")
                CloneContext = False
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_clone", "CloneContext", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_clone", "CloneContext", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    'Private Function CloneComponent(ByVal componentID As Integer) As Integer

    '    Dim log = New bc_cs_activity_log("bc_am_clone", "CloneComponent", bc_cs_activity_codes.TRACE_ENTRY, "")
    '    Try

    '        Dim sql As New StringBuilder

    '        With sql
    '            .Append("exec bcc_core_bp_clone_component ")
    '            .Append(componentID)
    '            .Append(",'")
    '            .Append(Replace(viewComponentClone.uxComponentName.Text, "'", "''"))
    '            .Append("'")
    '        End With

    '        Dim osql As New bc_om_sql(sql.ToString)

    '        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
    '            osql.tmode = bc_cs_soap_base_class.tREAD
    '            osql.transmit_to_server_and_receive(osql, True)
    '        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
    '            osql.db_read()
    '        End If

    '        If osql.success = True Then
    '            CloneComponent = True
    '        Else
    '            Dim errLog As New bc_cs_error_log("bc_am_clone", "CloneComponent", bc_cs_error_codes.USER_DEFINED, "Failed to clone component.")
    '            CloneComponent = False
    '        End If

    '    Catch ex As Exception
    '        Dim errLog As New bc_cs_error_log("bc_am_clone", "CloneComponent", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        log = New bc_cs_activity_log("bc_am_clone", "CloneComponent", bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try

    'End Function


End Class

