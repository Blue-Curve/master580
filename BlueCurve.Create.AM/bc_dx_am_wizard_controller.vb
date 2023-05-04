Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS

Public Class bc_dx_am_wizard_controller
    Private view As bc_dx_am_wizard
    Private sel_pub_type As bc_om_pub_type
    Private lead_entity As Long
    Private secondary_entity As Long
    Private Shared entity_list As bc_om_entities
    Private lallmine As Boolean
    Private sallmine As Boolean
    Private current_class As Long
    Public Enum entry_point
        OPEN_NEW = 0
        PUB_TYPE = 1
    End Enum
    Public Enum wizard_mode
        MAIN = 0
        PUB_TYPE = 1
        LEAD_ENTITY = 2
        SECONDARY_ENTITY = 3
        COMPOSITE = 4
        LEAD_COMPOSITE = 5
    End Enum
    Private current_mode As wizard_mode
    Private prev_mode As wizard_mode

    Private _entry_point As entry_point
    Public Property set_entry_point()
        Get
            Return _entry_point
        End Get
        Set(ByVal value)
            _entry_point = value
        End Set
    End Property
    Public Sub New()
        view = New bc_dx_am_wizard
        view.controller = Me

        If IsNothing(entity_list) Then
            entity_list = New bc_om_entities
            If bc_cs_central_settings.alt_entity_for_build = True Then
                For i = 0 To bc_am_load_objects.obc_entities.alternate_entity_list.entity.Count - 1
                    If bc_am_load_objects.obc_entities.alternate_entity_list.entity(i).class_id = 23 Or bc_am_load_objects.obc_entities.alternate_entity_list.entity(i).class_id = 4 Then
                        entity_list.entity.Add(bc_am_load_objects.obc_entities.entity(i))
                    End If
                Next

            Else
                For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                    If bc_am_load_objects.obc_entities.entity(i).class_id = 23 Or bc_am_load_objects.obc_entities.entity(i).class_id = 4 Then
                        entity_list.entity.Add(bc_am_load_objects.obc_entities.entity(i))
                    End If
                Next
            End If
        End If
        'If bc_cs_central_settings.alt_entity_for_build = True Then
        '    entity_list = bc_am_load_objects.obc_entities.alternate_entity_list
        'Else
        '    entity_list = bc_am_load_objects.obc_entities
        'End If

    End Sub
    Public Sub show()
        If _entry_point = entry_point.PUB_TYPE Then
            If load_pub_type() = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "User doesnt have access to any publications", bc_cs_message.MESSAGE, False, "Yes", "No", True)
                Exit Sub
            End If
           
        End If
        view.ShowDialog()
    End Sub
    Friend Sub button_Press(ByVal tag As String)
        Dim new_mode As wizard_mode
        If tag = "Close" Then
            view.Hide()
            Exit Sub
        End If
        If tag = "Ok" Then
            view.Hide()
            REM build_document()
            Exit Sub
        End If
        view.uxallmine.Visible = False
        view.SimpleButton1.Visible = False
        view.SimpleButton2.Visible = False
        view.SimpleButton3.Visible = False
        view.SimpleButton4.Visible = False


        Select Case current_mode
            Case wizard_mode.PUB_TYPE
                lead_entity = 0
                secondary_entity = 0
                sallmine = -1
                lallmine = -1
                If tag = "Next" Then
                    new_mode = wizard_mode.LEAD_ENTITY
                    load_lead_entity()
                ElseIf tag = "Composite" Then
                    new_mode = wizard_mode.COMPOSITE
                    load_composite()
                ElseIf tag = "Back" Then
                    new_mode = wizard_mode.MAIN
                End If
            Case wizard_mode.LEAD_ENTITY
                secondary_entity = 0
                If tag = "Next" Then
                    new_mode = wizard_mode.SECONDARY_ENTITY
                    set_lead_entity()
                    load_secondary_entity()


                ElseIf tag = "Composite" Then
                    new_mode = wizard_mode.COMPOSITE
                    load_composite()
                ElseIf tag = "Back" Then
                    new_mode = wizard_mode.PUB_TYPE
                    load_pub_type()
                End If
            Case wizard_mode.SECONDARY_ENTITY
                If tag = "Composite" Then
                    new_mode = wizard_mode.COMPOSITE
                    load_composite()
                ElseIf tag = "Back" Then
                    new_mode = wizard_mode.LEAD_ENTITY
                    load_lead_entity()
                End If

            Case wizard_mode.COMPOSITE

                If tag = "Back" Then
                    Select Case prev_mode
                        Case wizard_mode.PUB_TYPE
                            new_mode = wizard_mode.PUB_TYPE
                            load_pub_type()

                        Case wizard_mode.LEAD_ENTITY
                            new_mode = wizard_mode.LEAD_ENTITY
                            load_lead_entity()
                        Case wizard_mode.SECONDARY_ENTITY

                            new_mode = wizard_mode.SECONDARY_ENTITY
                            load_secondary_entity()

                    End Select

                End If
        End Select

        If tag = "Close" Then
            view.Hide()
        End If
        prev_mode = current_mode
        current_mode = new_mode
    End Sub
    Private Sub set_pub_type()
        For i = 0 To bc_dx_am_wizard_controller.entity_list.entity.Count - 1
            If bc_dx_am_wizard_controller.entity_list.entity(i).class_id = sel_pub_type.child_category AndAlso bc_dx_am_wizard_controller.entity_list.entity(i).name = view.uxpubtypelist.Text Then
                Me.lead_entity = bc_dx_am_wizard_controller.entity_list.entity(i)
                Exit For
            End If
        Next
    End Sub
    Private Sub set_lead_entity()
        For i = 0 To bc_dx_am_wizard_controller.entity_list.entity.Count - 1
            If bc_dx_am_wizard_controller.entity_list.entity(i).class_id = sel_pub_type.child_category AndAlso bc_dx_am_wizard_controller.entity_list.entity(i).name = view.uxpubtypelist.Text Then
                Me.lead_entity = bc_dx_am_wizard_controller.entity_list.entity(i).id
                Exit For
            End If
        Next
        lallmine = 0
        If view.uxallmine.SelectedIndex = 1 Then
            lallmine = 1
        End If
    End Sub
    Private Sub set_secondary_entity()
        For i = 0 To bc_dx_am_wizard_controller.entity_list.entity.Count - 1
            If bc_dx_am_wizard_controller.entity_list.entity(i).class_id = sel_pub_type.child_category AndAlso bc_dx_am_wizard_controller.entity_list.entity(i).name = view.uxpubtypelist.Text Then
                Me.secondary_entity = bc_dx_am_wizard_controller.entity_list.entity(i).id
                Exit For
            End If
        Next
        sallmine = 0
        If view.uxallmine.SelectedIndex = 1 Then
            sallmine = 1
        End If
    End Sub
    Private sloading As Boolean = False
    Friend Sub toggle_all_mine(ByVal i As Integer)
        load_entities_for_class(current_class, i)
    End Sub
    Private Function set_all_mine(ByVal mode As Integer) As Integer
        view.uxallmine.Visible = False
        sloading = True
        Dim idx As Integer
        Dim mload As Boolean = False

        If bc_cs_central_settings.show_all_entities = True AndAlso bc_cs_central_settings.my_entities_default = False Then

            view.uxallmine.Visible = True
            idx = view.uxallmine.SelectedIndex

            If idx <> 1 Then
                view.uxallmine.SelectedIndex = 1
            Else
                mload = True
            End If


        ElseIf bc_cs_central_settings.show_all_entities = True AndAlso bc_cs_central_settings.my_entities_default = True Then
            view.uxallmine.Visible = True
            idx = view.uxallmine.SelectedIndex

            If idx <> 0 Then
                view.uxallmine.SelectedIndex = 0
            Else
                mload = True
            End If
        Else
            mload = True

        End If


        If mload = True Then
            Select Case mode
                Case 0
                    load_entities_for_class(sel_pub_type.child_category)
                Case 1
                    load_entities_for_class(sel_pub_type.sub_entity_class)
            End Select
        End If

    End Function
    Private Sub load_lead_entity()
        view.SimpleButton2.Visible = True
        view.SimpleButton2.Text = "Back"
        view.SimpleButton2.Tag = "Back"

        view.SimpleButton3.Visible = True
        view.SimpleButton3.Text = "Close"
        view.SimpleButton3.Tag = "Close"
        If sel_pub_type.sub_entity_class = 0 And sel_pub_type.composite = False Then
            view.SimpleButton1.Visible = True
            view.SimpleButton1.Text = "Ok"
            view.SimpleButton1.Tag = "Ok"
        Else
            view.SimpleButton1.Visible = True
            view.SimpleButton1.Text = "Next"
            view.SimpleButton1.Tag = "Next"
        End If
        current_class = sel_pub_type.child_category
        set_all_mine(0)
        'load_entities_for_class(sel_pub_type.child_category)
    End Sub
    Private tloading As Boolean = False
    Public Sub load_entities_for_class(ByVal class_id As Long, Optional ByVal ballmine As Integer = 2)
        Dim mylist As Integer


        If ballmine = 0 Then
            mylist = 0
        Else
            mylist = 1
        End If



        view.uxpubtypelist.Items.Clear()
        view.uxdesc.Visible = False
        Dim found As Boolean = False
        Dim idx As Integer = 0

        If mylist = 0 Then
            For i = 0 To bc_am_load_objects.obc_prefs.pref.Count - 1

                If bc_am_load_objects.obc_prefs.pref(i).class_id = class_id Then
                    If found = False Then
                        found = True
                        view.ltitle.Text = "Please select " + bc_am_load_objects.obc_prefs.pref(i).class_name
                    End If
                    view.uxpubtypelist.Items.Add(bc_am_load_objects.obc_prefs.pref(i).entity_name)
                    If lead_entity > 0 AndAlso lead_entity = bc_dx_am_wizard_controller.entity_list.entity(i).id Then
                        idx = view.uxpubtypelist.Items.Count - 1
                    End If
                End If
            Next
        Else

            For i = 0 To bc_dx_am_wizard_controller.entity_list.entity.Count - 1
                If bc_dx_am_wizard_controller.entity_list.entity(i).class_id = class_id Then
                    If found = False Then
                        found = True
                        view.ltitle.Text = "Please select " + bc_dx_am_wizard_controller.entity_list.entity(i).class_name
                    End If
                    view.uxpubtypelist.Items.Add(bc_dx_am_wizard_controller.entity_list.entity(i).name)
                    If lead_entity > 0 AndAlso lead_entity = bc_dx_am_wizard_controller.entity_list.entity(i).id Then
                        idx = view.uxpubtypelist.Items.Count - 1
                    End If
                End If
            Next
        End If
        If view.uxpubtypelist.Items.Count > 0 Then
            view.uxpubtypelist.SelectedIndex = idx
        End If

    End Sub
    Private Sub load_secondary_entity()

        Dim mylist As Boolean = False
         view.SimpleButton2.Visible = True
        view.SimpleButton2.Text = "Back"
        view.SimpleButton2.Tag = "Back"

        view.SimpleButton3.Visible = True
        view.SimpleButton3.Text = "Close"
        view.SimpleButton3.Tag = "Close"
        If sel_pub_type.composite = False Then
            view.SimpleButton1.Visible = True
            view.SimpleButton1.Text = "Ok"
            view.SimpleButton1.Tag = "Ok"
        Else
            view.SimpleButton1.Visible = True
            view.SimpleButton1.Text = "Next"
            view.SimpleButton1.Tag = "Composite"
        End If
        current_class = sel_pub_type.sub_entity_class
        set_all_mine(1)
        'load_entities_for_class(sel_pub_type.sub_entity_class)
    End Sub
    Private Sub load_composite()
        view.SimpleButton1.Visible = True
        view.SimpleButton1.Tag = "Ok"
        view.SimpleButton1.Text = "Ok"
        view.SimpleButton1.Enabled = True
        view.SimpleButton2.Visible = True
        view.SimpleButton2.Tag = "Back"
        view.SimpleButton2.Text = "Back"
        view.SimpleButton2.Enabled = True
        view.SimpleButton3.Visible = True
        view.SimpleButton3.Tag = "Close"
        view.SimpleButton3.Text = "Close"
        view.SimpleButton3.Enabled = True
        view.uxpubtypelist.Items.Clear()
        view.ltitle.Text = "Composite"

    End Sub

    Friend Function pub_type_selected(ByVal pub_type_name As String)
        For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1



            If bc_am_load_objects.obc_pub_types.pubtype(i).name = pub_type_name Then
                With bc_am_load_objects.obc_pub_types.pubtype(i)
                    view.uxdesc.Text = .description
                    sel_pub_type = bc_am_load_objects.obc_pub_types.pubtype(i)

                    If .child_category = 0 And .composite = False Then
                        view.SimpleButton1.Visible = True
                        view.SimpleButton1.Tag = "Ok"
                        view.SimpleButton1.Text = "Ok"
                        view.SimpleButton1.Enabled = True
                    Else
                        view.SimpleButton1.Visible = True
                        view.SimpleButton1.Tag = "Next"
                        view.SimpleButton1.Text = "Next"
                        view.SimpleButton1.Enabled = True
                    End If

                End With

                Exit For
            End If
        Next
        pub_type_selected = True
    End Function


    Private Function load_pub_type() As Boolean
        Try
            view.ltitle.Text = "Please select a Publication"
            If _entry_point = entry_point.PUB_TYPE Then
                view.uxppubtype.Visible = True
                view.SimpleButton1.Visible = True
                view.SimpleButton2.Visible = True
                view.SimpleButton1.Tag = "Next"
                view.SimpleButton1.Text = "Next"
                view.SimpleButton1.Enabled = False
                view.SimpleButton2.Tag = "Close"
                view.SimpleButton2.Text = "Close"
                view.SimpleButton2.Enabled = True
            End If
            REM load create pub types
            view.uxdesc.Text = ""
            view.uxpubtypelist.Items.Clear()
            load_pub_type = False
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                REM filter pub types for users bus area
                For j = 0 To bc_am_load_objects.obc_prefs.bus_areas.Count - 1
                    If bc_am_load_objects.obc_prefs.bus_areas(j) = bc_am_load_objects.obc_pub_types.pubtype(i).bus_area_id And bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_only = False Then
                        If bc_am_load_objects.obc_pub_types.pubtype(i).show_in_wizard = True Then
                            view.uxpubtypelist.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
                            If Not IsNothing(sel_pub_type) AndAlso sel_pub_type.id = bc_am_load_objects.obc_pub_types.pubtype(i).id Then
                                view.uxpubtypelist.SelectedIndex = view.uxpubtypelist.Items.Count - 1
                            End If
                            load_pub_type = True
                        End If
                    End If
                Next
            Next

            view.uxdesc.Visible = True
            current_mode = wizard_mode.PUB_TYPE



        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_am_wizard_controller", "load_pub_type", bc_cs_error_codes.USER_DEFINED, ex.Message)
            load_pub_type = False
        End Try

    End Function
End Class
