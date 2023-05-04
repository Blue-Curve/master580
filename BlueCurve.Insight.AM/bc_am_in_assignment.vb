Imports System.Windows.Forms.Screen
Imports System.io
Imports Microsoft.Win32
Imports System.Windows.forms
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Public Class bc_am_in_assignment
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents uxContributingClasses As System.Windows.Forms.GroupBox
    Friend WithEvents Lcclass As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader12 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader14 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader17 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader13 As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxTemplateAssignment As System.Windows.Forms.GroupBox
    Friend WithEvents tasstmp As System.Windows.Forms.TabControl
    Friend WithEvents lasstmp As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader15 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader16 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxImageList As System.Windows.Forms.ImageList
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_assignment))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.uxContributingClasses = New System.Windows.Forms.GroupBox()
        Me.Lcclass = New System.Windows.Forms.ListView()
        Me.ColumnHeader12 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader14 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader17 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader13 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader1 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader2 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.uxTemplateAssignment = New System.Windows.Forms.GroupBox()
        Me.tasstmp = New System.Windows.Forms.TabControl()
        Me.lasstmp = New System.Windows.Forms.ListView()
        Me.ColumnHeader15 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.ColumnHeader16 = CType(New System.Windows.Forms.ColumnHeader(), System.Windows.Forms.ColumnHeader)
        Me.uxImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.uxContributingClasses.SuspendLayout()
        Me.uxTemplateAssignment.SuspendLayout()
        Me.SuspendLayout()
        '
        'uxContributingClasses
        '
        Me.uxContributingClasses.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxContributingClasses.Controls.Add(Me.Lcclass)
        Me.uxContributingClasses.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxContributingClasses.Location = New System.Drawing.Point(8, 8)
        Me.uxContributingClasses.Name = "uxContributingClasses"
        Me.uxContributingClasses.Size = New System.Drawing.Size(700, 120)
        Me.uxContributingClasses.TabIndex = 49
        Me.uxContributingClasses.TabStop = False
        Me.uxContributingClasses.Text = "Contributing Classes"
        '
        'Lcclass
        '
        Me.Lcclass.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Lcclass.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader12, Me.ColumnHeader14, Me.ColumnHeader17, Me.ColumnHeader13, Me.ColumnHeader1, Me.ColumnHeader2})
        Me.Lcclass.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Lcclass.FullRowSelect = True
        Me.Lcclass.HideSelection = False
        Me.Lcclass.Location = New System.Drawing.Point(6, 24)
        Me.Lcclass.MultiSelect = False
        Me.Lcclass.Name = "Lcclass"
        Me.Lcclass.Size = New System.Drawing.Size(684, 90)
        Me.Lcclass.TabIndex = 52
        Me.Lcclass.UseCompatibleStateImageBehavior = False
        Me.Lcclass.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader12
        '
        Me.ColumnHeader12.Text = "Parent Class"
        Me.ColumnHeader12.Width = 156
        '
        'ColumnHeader14
        '
        Me.ColumnHeader14.Text = "Schema"
        Me.ColumnHeader14.Width = 126
        '
        'ColumnHeader17
        '
        Me.ColumnHeader17.Text = "Granularity"
        Me.ColumnHeader17.Width = 120
        '
        'ColumnHeader13
        '
        Me.ColumnHeader13.Text = "Child Class"
        Me.ColumnHeader13.Width = 110
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Primary Pref"
        Me.ColumnHeader1.Width = 132
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Secondary Pref"
        Me.ColumnHeader2.Width = 175
        '
        'uxTemplateAssignment
        '
        Me.uxTemplateAssignment.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxTemplateAssignment.Controls.Add(Me.tasstmp)
        Me.uxTemplateAssignment.Controls.Add(Me.lasstmp)
        Me.uxTemplateAssignment.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxTemplateAssignment.Location = New System.Drawing.Point(8, 144)
        Me.uxTemplateAssignment.Name = "uxTemplateAssignment"
        Me.uxTemplateAssignment.Size = New System.Drawing.Size(700, 232)
        Me.uxTemplateAssignment.TabIndex = 50
        Me.uxTemplateAssignment.TabStop = False
        Me.uxTemplateAssignment.Text = "Template Assignment"
        '
        'tasstmp
        '
        Me.tasstmp.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tasstmp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tasstmp.Location = New System.Drawing.Point(8, 20)
        Me.tasstmp.Name = "tasstmp"
        Me.tasstmp.SelectedIndex = 0
        Me.tasstmp.Size = New System.Drawing.Size(684, 24)
        Me.tasstmp.TabIndex = 30
        '
        'lasstmp
        '
        Me.lasstmp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
            Or System.Windows.Forms.AnchorStyles.Left) _
            Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lasstmp.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader15, Me.ColumnHeader16})
        Me.lasstmp.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lasstmp.FullRowSelect = True
        Me.lasstmp.HideSelection = False
        Me.lasstmp.Location = New System.Drawing.Point(8, 46)
        Me.lasstmp.MultiSelect = False
        Me.lasstmp.Name = "lasstmp"
        Me.lasstmp.Size = New System.Drawing.Size(684, 178)
        Me.lasstmp.SmallImageList = Me.uxImageList
        Me.lasstmp.TabIndex = 29
        Me.lasstmp.UseCompatibleStateImageBehavior = False
        Me.lasstmp.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader15
        '
        Me.ColumnHeader15.Text = "Entity"
        Me.ColumnHeader15.Width = 203
        '
        'ColumnHeader16
        '
        Me.ColumnHeader16.Text = "Template"
        Me.ColumnHeader16.Width = 193
        '
        'uxImageList
        '
        Me.uxImageList.ImageStream = CType(resources.GetObject("uxImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.uxImageList.Images.SetKeyName(0, "")
        '
        'bc_am_in_assignment
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(716, 384)
        Me.ControlBox = False
        Me.Controls.Add(Me.uxTemplateAssignment)
        Me.Controls.Add(Me.uxContributingClasses)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_in_assignment"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.uxContributingClasses.ResumeLayout(False)
        Me.uxTemplateAssignment.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public tk_main As Object
    'Public Sub set_change()
    '    tk_main.set_change("Assignment")
    'End Sub
    Public loading_from_top As Boolean = False

    Private Sub Lcclass_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lcclass.SelectedIndexChanged
        Try
            Me.Cursor = Cursors.WaitCursor


            loading_from_top = True


            If Me.Lcclass.SelectedItems.Count = 0 Then
                Me.tk_main.set_button_disabled(1)
                Me.tk_main.set_button_disabled(3)
                Me.tk_main.set_button_disabled(4)
                Me.uxTemplateAssignment.Enabled = False
                Exit Sub
            End If
            Me.tk_main.set_button_enabled(1)
            Me.tk_main.set_button_disabled(3)
            Me.tk_main.set_button_enabled(4)
            Me.uxTemplateAssignment.Enabled = True
            Dim i As Integer
            REM populate granularity list
            Me.lasstmp.Items.Clear()
            Me.uxTemplateAssignment.Text = "Template Assignment for Class: " + Me.Lcclass.SelectedItems(0).SubItems(0).Text + ", Schema " + Me.Lcclass.SelectedItems(0).SubItems(1).Text
            Dim tp As TabPage

            For i = 0 To tasstmp.TabPages.Count - 1
                tasstmp.TabPages.RemoveAt(0)
            Next

            tp = New TabPage(CStr(Me.Lcclass.SelectedItems(0).SubItems(0).Text))
            Me.tasstmp.TabPages.Add(tp)
            If Me.Lcclass.SelectedItems(0).SubItems(3).Text <> "None" Then
                tp = New TabPage(CStr(Me.Lcclass.SelectedItems(0).SubItems(3).Text))
                Me.tasstmp.TabPages.Add(tp)
            End If
            Me.lasstmp.Columns(0).Text = Me.Lcclass.SelectedItems(0).SubItems(2).Text
            Me.uxTemplateAssignment.Enabled = True
            REM load template assignments
            If Me.tasstmp.TabPages.Count = 1 Then
                load_template_assignments(True, False, True)
            Else
                load_template_assignments(True, True, True)
            End If
            If Me.Lcclass.SelectedItems(0).SubItems(2).Text() = "None" Then
                Me.ToolTip1.SetToolTip(Me.lasstmp, "Assign Template that all  " + Me.Lcclass.SelectedItems(0).SubItems(0).Text() + "(s) will use")
            Else
                Me.ToolTip1.SetToolTip(Me.lasstmp, "Assign Template based upon the " + Me.Lcclass.SelectedItems(0).SubItems(2).Text() + " the " + Me.Lcclass.SelectedItems(0).SubItems(0).Text() + " is in.")
            End If

        Catch ex As Exception
        Finally

            loading_from_top = False
            Me.Cursor = Cursors.Default

        End Try
    End Sub
    Public Sub load_template_assignments(ByVal primary_flag As Boolean, ByVal child_entity As Boolean, reload_from_db As Boolean)
        Dim i, j As Integer
        Dim st As Integer
        Dim co As Integer
        Dim found As Boolean
        Try

            If reload_from_db = True Then
                bc_am_in_context.insight_items.contributing_classes(Me.Lcclass.SelectedItems(0).Index).entity_template_assigments.clear()
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    bc_am_in_context.insight_items.contributing_classes(Me.Lcclass.SelectedItems(0).Index).db_read()
                Else
                    bc_am_in_context.insight_items.contributing_classes(Me.Lcclass.SelectedItems(0).Index).tmode = bc_cs_soap_base_class.tREAD
                    If bc_am_in_context.insight_items.contributing_classes(Me.Lcclass.SelectedItems(0).Index).transmit_to_server_and_receive(bc_am_in_context.insight_items.contributing_classes(Me.Lcclass.SelectedItems(0).Index), True) = False Then
                        Exit Sub
                    End If
                End If
            End If

            Me.Cursor = Cursors.WaitCursor

            Me.lasstmp.BeginUpdate()
            Me.lasstmp.Items.Clear()

            If primary_flag = True Then
                st = 0
                If child_entity = True Then
                    co = bc_am_in_context.insight_items.contributing_classes(Me.Lcclass.SelectedItems(0).Index).entity_template_assigments.Count / 2
                Else
                    co = bc_am_in_context.insight_items.contributing_classes(Me.Lcclass.SelectedItems(0).Index).entity_template_assigments.Count
                End If
            Else
                st = bc_am_in_context.insight_items.contributing_classes(Me.Lcclass.SelectedItems(0).Index).entity_template_assigments.Count / 2
                co = bc_am_in_context.insight_items.contributing_classes(Me.Lcclass.SelectedItems(0).Index).entity_template_assigments.Count
            End If
            Dim lvw As ListViewItem
            Me.lasstmp.Items.Clear()


            For i = st To co - 1
                found = False
                lvw = New ListViewItem(CStr(bc_am_in_context.insight_items.contributing_classes(Me.Lcclass.SelectedItems(0).Index).entity_template_assigments(i).entity_name), 0)
                If bc_am_in_context.insight_items.contributing_classes(Me.Lcclass.SelectedItems(0).Index).entity_template_assigments(i).template_id = 0 Then
                    lvw.SubItems.Add("not set")
                Else
                    For j = 0 To bc_am_in_context.asheet.logical_templates_ids.Count - 1
                        If bc_am_in_context.asheet.logical_templates_ids(j) = bc_am_in_context.insight_items.contributing_classes(Me.Lcclass.SelectedItems(0).Index).entity_template_assigments(i).template_id Then
                            lvw.SubItems.Add(bc_am_in_context.asheet.logical_templates_names(j))
                            found = True
                            Exit For
                        End If
                    Next
                    If found = False Then
                        lvw.SubItems.Add("not set")
                    End If
                End If

                Me.lasstmp.Items.Add(lvw)
            Next

        Catch

        Finally
            Me.lasstmp.EndUpdate()
            Me.Cursor = Cursors.Default

        End Try
    End Sub


    Public Sub load_contributing_classes()
        Dim i, j As Integer
        Dim pclass_name As String = ""
        Dim cclass_name As String
        Dim pgran_name As String = ""
        Me.Lcclass.Items.Clear()
        Dim lvew As New ListViewItem
        Me.uxTemplateAssignment.Enabled = False
        For i = 0 To bc_am_in_context.insight_items.contributing_classes.Count - 1
            cclass_name = "None"
            For j = 0 To bc_am_in_context.insight_items.class_ids.Count - 1
                If bc_am_in_context.insight_items.class_ids(j) = bc_am_in_context.insight_items.contributing_classes(i).parent_class_id Then
                    pclass_name = bc_am_in_context.insight_items.class_names(j)
                End If
                If bc_am_in_context.insight_items.class_ids(j) = bc_am_in_context.insight_items.contributing_classes(i).child_class_id Then
                    If bc_am_in_context.insight_items.class_ids(j) <> 0 Then
                        cclass_name = bc_am_in_context.insight_items.class_names(j)
                    End If
                End If
                If bc_am_in_context.insight_items.contributing_classes(i).granularity_class_id = 0 Then
                    pgran_name = "None"
                Else
                    If bc_am_in_context.insight_items.class_ids(j) = bc_am_in_context.insight_items.contributing_classes(i).granularity_class_id Then
                        pgran_name = bc_am_in_context.insight_items.class_names(j)
                    End If
                End If
            Next
            lvew = New ListViewItem(pclass_name)
            For j = 0 To bc_am_in_context.insight_items.schema_ids.Count - 1
                If bc_am_in_context.insight_items.schema_ids(j) = bc_am_in_context.insight_items.contributing_classes(i).schema_id Then
                    lvew.SubItems.Add(bc_am_in_context.insight_items.schema_names(j))
                    Exit For
                End If
            Next
            lvew.SubItems.Add(pgran_name)
            lvew.SubItems.Add(cclass_name)
            Dim found As Boolean
            found = False
            For j = 0 To bc_am_in_context.insight_items.pref_types.Count - 1
                If bc_am_in_context.insight_items.pref_types(j).id = bc_am_in_context.insight_items.contributing_classes(i).primary_apref_type Then
                    lvew.SubItems.Add(bc_am_in_context.insight_items.pref_types(j).name)
                    found = True
                    Exit For
                End If
            Next
            If found = False Then
                lvew.SubItems.Add("None")
            End If
            found = False
            For j = 0 To bc_am_in_context.insight_items.pref_types.Count - 1
                If bc_am_in_context.insight_items.pref_types(j).id = bc_am_in_context.insight_items.contributing_classes(i).secondary_apref_type Then
                    lvew.SubItems.Add(bc_am_in_context.insight_items.pref_types(j).name)
                    found = True
                    Exit For
                End If
            Next
            If found = False Then
                lvew.SubItems.Add("None")
            End If
            Me.Lcclass.Items.Add(lvew)
        Next
        Me.lasstmp.Items.Clear()

    End Sub


    Private Sub lasstmp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lasstmp.SelectedIndexChanged
        If lasstmp.SelectedItems.Count = 0 Then
            Me.tk_main.set_button_disabled(3)
        Else
            Me.tk_main.set_button_enabled(3)
        End If
        If lasstmp.SelectedItems.Count > 0 Then
            Me.Cursor = Cursors.WaitCursor
            tk_main.get_ass()
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub tasstmp_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tasstmp.SelectedIndexChanged
        Try
            If loading_from_top = True Then
                Exit Sub
            End If
            Me.tk_main.set_button_disabled(3)
            Select Case Me.tasstmp.SelectedIndex
                Case 0
                    Me.uxTemplateAssignment.Text = "Template Assignment for Parent Class: " + Me.Lcclass.SelectedItems(0).SubItems(0).Text + ", Schema " + Me.Lcclass.SelectedItems(0).SubItems(1).Text
                    If Me.tasstmp.TabPages.Count = 1 Then
                        Me.load_template_assignments(True, False, False)
                    Else
                        Me.load_template_assignments(True, True, False)
                    End If
                    If Me.Lcclass.SelectedItems(0).SubItems(3).Text() = "None" Then
                        Me.ToolTip1.SetToolTip(Me.lasstmp, "Assign Template that all  " + Me.Lcclass.SelectedItems(0).SubItems(0).Text() + "(s) will use")
                    Else
                        Me.ToolTip1.SetToolTip(Me.lasstmp, "Assign Template based upon the " + Me.Lcclass.SelectedItems(0).SubItems(2).Text() + " the " + Me.Lcclass.SelectedItems(0).SubItems(0).Text() + " is in.")
                    End If


                Case 1
                    Me.uxTemplateAssignment.Text = "Template Assignment for Child Class: " + Me.Lcclass.SelectedItems(0).SubItems(3).Text + ", Schema " + Me.Lcclass.SelectedItems(0).SubItems(1).Text
                    Me.load_template_assignments(False, True, False)
                    If Me.Lcclass.SelectedItems(0).SubItems(3).Text() = "None" Then
                        Me.ToolTip1.SetToolTip(Me.lasstmp, "Assign Template that all  " + Me.Lcclass.SelectedItems(0).SubItems(3).Text() + "(s) will use")
                    Else
                        Me.ToolTip1.SetToolTip(Me.lasstmp, "Assign Template based upon the " + Me.Lcclass.SelectedItems(0).SubItems(2).Text() + " the " + Me.Lcclass.SelectedItems(0).SubItems(3).Text() + " is in.")
                    End If


            End Select
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub
    'Public Sub upload_cont(Optional ByVal show_message As Boolean = True)
    '    Try
    '        Dim i As Integer
    '        Me.Cursor = Cursors.WaitCursor

    '        For i = 0 To bc_am_in_context.insight_items.contributing_classes.Count - 1
    '            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
    '                bc_am_in_context.insight_items.contributing_classes(i).db_write()
    '            Else
    '                bc_am_in_context.insight_items.contributing_classes(i).tmode = bc_cs_soap_base_class.tWRITE
    '                bc_am_in_context.insight_items.contributing_classes(i).no_send_back = True
    '                bc_am_in_context.insight_items.contributing_classes(i).transmit_to_server_and_receive(Nothing, True)
    '            End If
    '            Me.Lcclass.SelectedItems.Clear()
    '            Me.lasstmp.Items.Clear()
    '            Me.uxTemplateAssignment.Enabled = False
    '        Next
    '    Catch ex As Exception
    '        Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "upload_cont", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally

    '        Me.Cursor = Cursors.Default
    '    End Try
    'End Sub

   
    Private Sub bc_am_in_assignment_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub
End Class

