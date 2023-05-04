Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS

Public Class bc_dx_insight_build
    Implements Ibc_dx_insight_build

    Dim entities As New List(Of bc_om_entity)
    Dim pref_entities As New List(Of bc_om_entity)
    Public Sub New()

        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Event execute(lead_entity_id As Long, schema_id As Long, class_id As Long, entity_name As String, class_name As String, schema_name As String) Implements Ibc_dx_insight_build.execute

    Public Function load_view(title As String, button_text As String) As Boolean Implements Ibc_dx_insight_build.load_view
        load_view = False
        Try
            Me.Text = title
            Me.bok.Text = button_text
            Select Case True
                Case bc_am_insight_link_names.show_all_entities And Not bc_am_insight_link_names.my_entities_default
                    Me.rallmine.Visible = True
                    Me.rallmine.SelectedIndex = 0
                Case bc_am_insight_link_names.show_all_entities And bc_am_insight_link_names.my_entities_default
                    Me.rallmine.Visible = True
                    Me.rallmine.SelectedIndex = 1
                Case Not bc_am_insight_link_names.show_all_entities
                    Me.rallmine.Visible = False
                    Me.rallmine.SelectedIndex = 1
            End Select

            Me.uxschema.Properties.Items.Clear()
            For i = 0 To bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links.Count - 1
                If i > 0 Then
                    If bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).schema_id <> bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i - 1).schema_id Then
                        Me.uxschema.Properties.Items.Add(bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).schema_name)
                    End If
                Else
                    Me.uxschema.Properties.Items.Add(bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).schema_name)
                End If
            Next
            If Me.uxschema.Properties.Items.Count = 0 Then
                Dim omessage As New bc_cs_message("Blue Curve - insight", "User not synchronized or not enough access rights!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            ElseIf Me.uxschema.Properties.Items.Count = 1 Then
                Me.uxschema.SelectedIndex = 0
                Me.uxschema.Enabled = False
            Else
                Me.uxschema.SelectedIndex = 0
            End If




            load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_insight_build", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Function

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub uxschema_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxschema.SelectedIndexChanged
        If Me.uxschema.SelectedIndex = -1 Then
            Exit Sub
        End If
        Dim i As Integer
        REM populate entity class's
        Me.uxclass.Properties.Items.Clear()
        For i = 0 To bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links.Count - 1
            If bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).schema_name = Me.uxschema.Text Then
                Me.uxclass.Properties.Items.Add(bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).parent_class_name)
            End If
        Next
        If Me.uxclass.Properties.Items.Count = 1 Then
            Me.uxclass.SelectedIndex = 0
            Me.uxclass.Enabled = False

        End If
        check_ok()
    End Sub
    Private class_id As Long
    Private Sub uxclass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxclass.SelectedIndexChanged
        Try
            Me.lentity.Items.Clear()
            If Me.uxclass.SelectedIndex = -1 Then
                Exit Sub

            End If
            Me.tsearch.Text = ""

            For i = 0 To bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links.Count - 1
                If bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).schema_name = Me.uxschema.Text Then
                    For j = 0 To bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links.Count - 1
                        If bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).parent_class_name = Me.uxclass.Text Then
                            class_id = bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).parent_entity_class_id
                            Exit For
                        End If
                    Next
                End If
            Next
            REM set up netities for class
            Dim querya = From en In bc_am_load_objects.obc_entities.entity
                              Where en.class_id = class_id
             Select New bc_om_entity With {
             .id = en.id,
             .name = en.name}
            entities = querya.ToList

            Dim queryp = From en In entities
                                       Join a In bc_am_load_objects.obc_prefs.pref On en.id Equals a.entity_id
                      Select New bc_om_entity With {
                      .id = en.id,
                      .name = en.name}

            pref_entities = queryp.ToList


            run_search()
            check_ok()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("uxclass", "selectedindexchanged", bc_cs_error_codes.USER_DEFINED,ex.Message)

        End Try
    End Sub

    Private Sub tsearch_EditValueChanged(sender As Object, e As EventArgs) Handles tsearch.EditValueChanged
        Timer1.Start()
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Timer1.Stop()
        run_search()

    End Sub
    Sub run_search()
        Try
            Me.Cursor = Windows.Forms.Cursors.WaitCursor
            Me.lentity.BeginUpdate()
            Me.lentity.Items.Clear()
            REM extended search for entities
            Dim found_entities As New List(Of Long)
            Dim tx As String = Me.tsearch.Text

            If tx <> "" Then

                REM real time extended search
                Dim search_results As New bc_om_real_time_search
                search_results.class_id = class_id
                search_results.search_text = tx
                search_results.mine = False
                search_results.inactive = False
                search_results.filter_attribute_id = 0
                search_results.results_as_ids = True
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    search_results.db_read()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    search_results.tmode = bc_cs_soap_base_class.tREAD
                    search_results.transmit_to_server_and_receive(search_results, False)
                End If
                For i = 0 To search_results.resultsids.Count - 1
                    found_entities.Add(search_results.resultsids(i))
                Next


            End If



            If Me.rallmine.SelectedIndex = 1 Then
                For i = 0 To pref_entities.Count - 1
                    If tx <> "" Then
                        If InStr(UCase(pref_entities(i).name), UCase(tx)) > 0 Then
                            Me.lentity.Items.Add(pref_entities(i).name)
                        Else

                            For j = 0 To found_entities.Count - 1
                                If found_entities(j) = pref_entities(i).id Then
                                    Me.lentity.Items.Add(pref_entities(i).name)
                                    Exit For
                                End If
                            Next
                        End If
                    Else
                        Me.lentity.Items.Add(pref_entities(i).name)
                    End If
                Next

            Else
                For i = 0 To entities.Count - 1
                    If tx <> "" Then
                        If InStr(UCase(entities(i).name), UCase(tx)) > 0 Then
                            Me.lentity.Items.Add(entities(i).name)
                        Else

                            For j = 0 To found_entities.Count - 1
                                If found_entities(j) = entities(i).id Then
                                    Me.lentity.Items.Add(entities(i).name)
                                    Exit For
                                End If
                            Next
                        End If
                    Else
                        Me.lentity.Items.Add(entities(i).name)
                    End If
                Next
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_insight_build", "run_search", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.lentity.EndUpdate()
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try


    End Sub

    Private Sub pclear_Click(sender As Object, e As EventArgs) Handles pclear.Click
        Me.tsearch.Text = ""

    End Sub


    Private Sub rallmine_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rallmine.SelectedIndexChanged
        run_search()
    End Sub

    Sub check_ok()
        Me.bok.Enabled = False
        If Me.uxschema.SelectedIndex > -1 AndAlso Me.uxclass.SelectedIndex > -1 AndAlso Me.lentity.SelectedIndex > -1 Then
            Me.bok.Enabled = True
        End If
    End Sub

    Private Sub bok_Click(sender As Object, e As EventArgs) Handles bok.Click
        complete()
    End Sub
    Private Sub complete()
        Try
            Dim entity_id As Long
            Dim schema_id As Long
            Me.Cursor = Windows.Forms.Cursors.WaitCursor

            For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                If bc_am_load_objects.obc_entities.entity(i).class_id = class_id AndAlso bc_am_load_objects.obc_entities.entity(i).name = Me.lentity.Text Then
                    entity_id = bc_am_load_objects.obc_entities.entity(i).id
                    Exit For
                End If
            Next
            For i = 0 To bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links.Count - 1
                If bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).schema_name = Me.uxschema.Text Then
                    schema_id = bc_am_load_objects.obc_om_insight_submission_entity_links.bc_om_insight_submission_entity_links(i).schema_id
                    Exit For
                End If
            Next



            RaiseEvent execute(entity_id, schema_id, class_id, Me.lentity.Text, Me.uxclass.Text, Me.uxschema.Text)
            Me.Hide()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_insight_build", "complete", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            Me.Cursor = Windows.Forms.Cursors.Default
        End Try
    End Sub

    Private Sub lentity_DoubleClick(sender As Object, e As EventArgs) Handles lentity.DoubleClick
        complete()

    End Sub

    Private Sub lentity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lentity.SelectedIndexChanged
        check_ok()

    End Sub

  
End Class
Public Class Cbc_dx_insight_build
    WithEvents _view As Ibc_dx_insight_build
    Public lead_entity_id As Long = 0
    Public schema_id As Long = 0
    Public class_id As Long = 0
    Public entity_name As String
    Public class_name As String
    Public schema_name As String

    Public Function load_data(view As Ibc_dx_insight_build, title As String, button_text As String)
        load_data = False
        Try
            _view = view
            load_data = _view.load_view(title, button_text)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_insight_build", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function
    Public Sub execute(llead_entity_id As Long, lschema_id As Long, lclass_id As Long, lentity_name As String, lclass_name As String, lschema_name As String) Handles _view.execute
        lead_entity_id = llead_entity_id
        schema_id = lschema_id
        class_id = lclass_id
        entity_name = lentity_name
        class_name = lclass_name
        schema_name = lschema_name

    End Sub

End Class
Public Interface Ibc_dx_insight_build
    Function load_view(title As String, button_text As String) As Boolean
    Event execute(lead_entity_id As Long, schema_id As Long, class_id As Long, entity_name As String, class_name As String, schema_name As String)
End Interface
