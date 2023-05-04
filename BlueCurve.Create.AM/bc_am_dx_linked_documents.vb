Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.IO

Imports System.Windows.Forms
Imports DevExpress.XtraTreeList


Public Class bc_am_dx_linked_documents
    Implements Ibc_am_linked_documents
    Public Event save(ByVal docs As List(Of bc_om_document)) Implements Ibc_am_linked_documents.save
    Public Event search(search_params As bc_om_doc_search_params) Implements Ibc_am_linked_documents.search

    Dim _sarch_docs As List(Of bc_om_documents)
    Dim _ctrll As Cbc_am_linked_documents
    'Dim _sel_docs As New List(Of bc_om_document)
    Dim _search_docs As List(Of bc_om_document)
    Dim _select_params As bc_om_doc_select_params
    Dim _num_sel As Integer
    Dim _doc As bc_om_document


    Private Sub bsave_Click(sender As Object, e As EventArgs) Handles bsave.Click
        Dim docs As New List(Of bc_om_document)
        Dim doc As bc_om_document
        Dim mandatory As Boolean = False
        REM check if linked doc must be attached
      




        For i = 0 To Me.lvsel.Nodes.Count - 1
            doc = New bc_om_document
            doc.id = Me.lvsel.Nodes(i).Tag
            doc.title = Me.lvsel.Nodes(i).GetDisplayText(0)
            doc.doc_date = Me.lvsel.Nodes(i).GetDisplayText(1)
            doc.pub_type_name = Me.lvsel.Nodes(i).GetDisplayText(2)
            doc.filename = Me.lvsel.Nodes(i).GetDisplayText(3)
            doc.original_extension = Me.lvsel.Nodes(i).GetDisplayText(4)
            doc.extension = Me.lvsel.Nodes(i).GetDisplayText(5)
            docs.Add(doc)
        Next
        RaiseEvent save(docs)
        Me.Hide()
    End Sub
    Dim loading As Boolean
    Public Sub load_data(ByVal num_sel As Integer, ByVal doc As bc_om_document, ByVal select_params As bc_om_doc_select_params, analytic_mode As Boolean) Implements Ibc_am_linked_documents.load_data
        Try
            If analytic_mode = True Then
                Me.Text = "Blue Curve - Analytic Documents"
            End If
            loading = True
            _doc = doc
            _select_params = select_params
            If analytic_mode = False Then
                If doc.id = 0 And _doc.linked_docs.Count = 0 Then
                    _doc.linked_docs = select_params.def_docs
                End If
                If Not IsNothing(_doc.linked_docs) Then
                    load_sel_docs()
                End If

                _num_sel = num_sel
            Else
                If doc.id = 0 And _doc.analytic_docs.Count = 0 Then
                    _doc.linked_docs = select_params.def_docs
                End If
                If Not IsNothing(_doc.analytic_docs) Then
                    load_sel_analytic_docs()
                End If

                _num_sel = num_sel

            End If
            Me.cbotype.Properties.Items.Clear()
            Me.cbotype.Properties.Items.Add("All")
            Dim sel_idx As Integer = -1
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                Me.cbotype.Properties.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = _doc.pub_type_id Then
                    sel_idx = i
                End If
            Next
            Me.cbotype.SelectedIndex = -1
            Me.cbotype.SelectedIndex = 0


            Me.cboauthor.Properties.Items.Clear()
            Me.cboauthor.Properties.Items.Add("All")
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                With bc_am_load_objects.obc_users.user(i)
                    Me.cboauthor.Properties.Items.Add(.first_name + " " + .surname)
                    'If .id = _doc.originating_author Then
                    '    sel_idx = i + 1
                    'End If
                End With
            Next
            'Me.cboauthor.SelectedIndex = sel_idx
            Me.cboauthor.SelectedIndex = 0
            Me.cbopropclass.Properties.Items.Clear()

            Me.cboclass.Properties.Items.Clear()
            Dim class_id As Long
            class_id = bc_am_load_objects.obc_pub_types.pubtype(sel_idx).child_category
            For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                With bc_am_load_objects.obc_entities.entity(i)
                    If .class_id = class_id Then
                        Me.cboclass.Properties.Items.Add(.class_name)
                        Me.cboclass.SelectedIndex = 0
                        Exit For
                    End If
                End With
            Next
            For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                With bc_am_load_objects.obc_entities.entity(i)
                    If _doc.entity_id = .id Then
                        Me.cboentity.Text = .name
                    End If
                End With
            Next
            For i = 0 To _select_params.prop_class.Count - 1
                Me.cbopropclass.Properties.Items.Add(_select_params.prop_class(i).class_name)
            Next
            Me.cbopropclass.SelectedIndex = 0
            Me.cbopropentity.Text = _select_params.def_prop_entity.name


            Me.dfrom.DateTime = DateAdd(DateInterval.Month, -9, Now)
            Me.dto.DateTime = Now
            Me.RadioGroup1.SelectedIndex = 0
            Me.cbopropentity.Enabled = False
            Me.cbopropclass.Enabled = False
            loading = False

            If Me.cboclass.Properties.Items.Count = 0 Then
                Me.cboclass.Visible = False
                Me.cboentity.Visible = False
                Me.cbopropclass.Visible = False
                Me.cbopropentity.Visible = False
                Me.RadioGroup1.Visible = False
                Me.Ltype.Visible = True
                Me.lauthor.Visible = True
                Me.cbotype.Visible = True
                Me.cboauthor.Visible = True
            End If

            set_description()


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_linked_documents", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Sub load_sel_analytic_docs()
        Try
            If IsNothing(_doc.analytic_docs) Then
                Exit Sub
            End If
            Me.lvsel.BeginUpdate()
            Dim tln As Nodes.TreeListNode = Nothing
            Dim n As DevExpress.XtraTreeList.Nodes.TreeListNode
            Me.lvsel.Nodes.Clear()


            For i = 0 To _doc.analytic_docs.Count - 1
                n = lvsel.AppendNode(New Object() {_doc.analytic_docs(i).title}, tln)
                n.Tag = CStr(Format(_doc.analytic_docs(i).id))
                n.SetValue(1, Format(_doc.analytic_docs(i).doc_date.ToLocalTime, "dd-MMM-yyyy HH:mm"))
                n.SetValue(2, _doc.analytic_docs(i).pub_type_name)
                n.SetValue(3, _doc.analytic_docs(i).filename)
                n.SetValue(4, _doc.analytic_docs(i).original_extension)
                n.SetValue(5, _doc.analytic_docs(i).extension)

            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_linked_documents", "load_sel_analytic_docs", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.lvsel.EndUpdate()
        End Try
    End Sub
    Sub load_sel_docs()
        Try
            If IsNothing(_doc.linked_docs) Then
                Exit Sub
            End If
            Me.lvsel.BeginUpdate()
            Dim tln As Nodes.TreeListNode = Nothing
            Dim n As DevExpress.XtraTreeList.Nodes.TreeListNode
            Me.lvsel.Nodes.Clear()


            For i = 0 To _doc.linked_docs.Count - 1
                n = lvsel.AppendNode(New Object() {_doc.linked_docs(i).title}, tln)
                n.Tag = CStr(Format(_doc.linked_docs(i).id))
                n.SetValue(1, Format(_doc.linked_docs(i).doc_date.ToLocalTime, "dd-MMM-yyyy HH:mm"))
                n.SetValue(2, _doc.linked_docs(i).pub_type_name)
                n.SetValue(3, _doc.linked_docs(i).filename)
                n.SetValue(4, _doc.linked_docs(i).original_extension)
                n.SetValue(5, _doc.linked_docs(i).extension)

            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_linked_documents", "load_sel_docs", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.lvsel.EndUpdate()
        End Try
    End Sub
    Public Sub load_search_results(search_docs As List(Of bc_om_document)) Implements Ibc_am_linked_documents.load_search_results
        Try
            Dim tln As Nodes.TreeListNode = Nothing
            Dim n As DevExpress.XtraTreeList.Nodes.TreeListNode

            Me.lvall.BeginUpdate()

            Dim tlns As New List(Of Nodes.TreeListNode)
            lres.Text = "Search Results (" + CStr(search_docs.Count) + ")"
            lres.Update()

            Me.lvall.Nodes.Clear()
            Dim da As DateTime

            For i = 0 To search_docs.Count - 1
                da = Format(search_docs(i).doc_date.ToLocalTime, "dd-MMM-yyyy HH:mm")
                n = lvall.AppendNode(New Object() {search_docs(i).title, da, search_docs(i).pub_type_name, search_docs(i).filename, search_docs(i).original_extension, search_docs(i).extension}, tln)
                n.Tag = CStr(Format(search_docs(i).id))
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_linked_documents", "load_search_results", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.lvall.EndUpdate()
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs)
        run_search()
    End Sub
    Private Sub run_search()

        Me.Cursor = Cursors.WaitCursor
      

        Try
            Dim params As New bc_om_doc_search_params
            params.date_from = Me.dfrom.DateTime
            params.date_to = Me.dto.DateTime
            If Me.cbotype.SelectedIndex > 0 Then
                params.type_id = bc_am_load_objects.obc_pub_types.pubtype(Me.cbotype.SelectedIndex - 1).id
            End If
            If Me.cboauthor.SelectedIndex > 0 Then
                params.author_id = bc_am_load_objects.obc_users.user(Me.cboauthor.SelectedIndex - 1).id
            End If
            params.entity_id = 0
            params.prop_entity_id = 0
            If RadioGroup1.SelectedIndex = 0 Then
                For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                    With bc_am_load_objects.obc_entities.entity(i)
                        If .class_name = Me.cboclass.Text AndAlso .name = Me.cboentity.Text Then
                            params.entity_id = .id
                            Exit For
                        End If
                    End With
                Next
            Else
                For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                    With bc_am_load_objects.obc_entities.entity(i)
                        If .class_name = Me.cbopropclass.Text AndAlso .name = Me.cbopropentity.Text Then
                            params.prop_entity_id = .id
                            Exit For
                        End If
                    End With
                Next
            End If

            RaiseEvent search(params)

            Me.Cursor = Windows.Forms.Cursors.WaitCursor

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_linked_documents", "run_search", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub






    Private Sub lvall_DoubleClick(sender As Object, e As EventArgs) Handles lvall.DoubleClick
        If _num_sel <> 0 AndAlso Me.lvsel.Nodes.Count = _num_sel Then
            Dim omsg As New bc_cs_message("Blue Curve", "Maximum number of documents selected.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If

        For i = 0 To Me.lvsel.Nodes.Count - 1
            If Me.lvsel.Nodes(i).Tag = Me.lvall.Selection(0).Tag Then
                Dim omsg As New bc_cs_message("Blue Curve", "Document already selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

        Next

        Dim tln As Nodes.TreeListNode = Nothing
        Dim n As DevExpress.XtraTreeList.Nodes.TreeListNode

        n = lvsel.AppendNode(New Object() {Me.lvall.Selection(0).GetValue(0)}, tln)
        n.Tag = Me.lvall.Selection(0).Tag
        n.SetValue(1, Me.lvall.Selection(0).GetValue(1))
        n.SetValue(2, Me.lvall.Selection(0).GetValue(2))
        n.SetValue(3, Me.lvall.Selection(0).GetValue(3))
        n.SetValue(4, Me.lvall.Selection(0).GetValue(4))
        n.SetValue(5, Me.lvall.Selection(0).GetValue(5))
    End Sub

   

    Private Sub cboclass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboclass.SelectedIndexChanged
        If Me.cboclass.SelectedIndex = -1 Then
            Exit Sub
        End If


        Me.cboentity.Properties.Items.Clear()
        Me.cboentity.Properties.Items.Add("All")
        For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
            With bc_am_load_objects.obc_entities.entity(i)
                If .class_name = Me.cboclass.Text Then
                    Me.cboentity.Properties.Items.Add(.name)
                End If
            End With
        Next
        set_description()
    End Sub
    Private Sub cbopropclass_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbopropclass.SelectedIndexChanged
        If Me.cbopropclass.SelectedIndex = -1 Then
            Exit Sub
        End If

        Me.cbopropentity.Properties.Items.Clear()
        For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
            With bc_am_load_objects.obc_entities.entity(i)
                If .class_name = Me.cbopropclass.Text Then
                    Me.cbopropentity.Properties.Items.Add(.name)
                End If
            End With
        Next

    End Sub

    Private Sub cbotype_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbotype.SelectedIndexChanged
        If Me.cbotype.SelectedIndex = -1 Or loading = True Then
            Exit Sub
        End If
        Dim class_id As Long


        If Me.cbotype.SelectedIndex > 0 Then
            Me.cboclass.Properties.Items.Clear()
            class_id = bc_am_load_objects.obc_pub_types.pubtype(Me.cbotype.SelectedIndex - 1).child_category
            For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                With bc_am_load_objects.obc_entities.entity(i)
                    If .class_id = class_id Then
                        Me.cboclass.Properties.Items.Add(.class_name)
                        Me.cboclass.SelectedIndex = 0
                        Me.cboentity.SelectedIndex = -1
                        Exit Sub
                    End If
                End With
            Next
        Else
            Dim prev_class As String = ""
            Me.cboclass.Properties.Items.Clear()
            For i = 0 To _select_params.sel_class.Count - 1
                Me.cboclass.Properties.Items.Add(_select_params.sel_class(i).class_name)
            Next

        End If
        set_description()

    End Sub
    Sub set_description()
        If loading = True Then
            Exit Sub
        End If
        Dim desc As String
        desc = "All Documents dated between " + Format(Me.dfrom.DateTime, "dd-MMM-yyyy") + " to " + Format(Me.dto.DateTime, "dd-MMM-yyyy") + " "


        If Me.cbotype.SelectedIndex > 0 Then
            desc = desc + "of type " + Me.cbotype.Text + " "
        Else
            desc = desc + "of all types "
        End If

        If Me.cboauthor.SelectedIndex > 0 Then
            desc = desc + "by author " + Me.cboauthor.Text + " "
        Else
            desc = desc + "by all authors "
        End If
        If Me.RadioGroup1.SelectedIndex = 0 Then
            If Me.cboentity.SelectedIndex > 0 Then
                desc = desc + "for " + Me.cboclass.Text + ": " + Me.cboentity.Text + " "
            ElseIf Me.cboentity.SelectedIndex = 0 Then
                desc = desc + "for all " + Me.cboclass.Text + " "
            End If
        Else
            If Me.cbopropclass.SelectedIndex > -1 And Me.cbopropentity.SelectedIndex > -1 Then
                desc = desc + " in " + Me.cbopropclass.Text + ": " + Me.cbopropentity.Text
            End If
        End If
        Me.ldesc.Text = desc
        run_search()
    End Sub

    Private Sub cboauthor_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboauthor.SelectedIndexChanged
        set_description()
    End Sub

    Private Sub cboentity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cboentity.SelectedIndexChanged
        If cboentity.SelectedIndex > -1 Then
            set_description()
        End If
    End Sub

    Private Sub cbopropentity_SelectedIndexChanged(sender As Object, e As EventArgs) Handles cbopropentity.SelectedIndexChanged
        If cbopropentity.SelectedIndex > -1 Then
            set_description()
        End If

    End Sub

    Private Sub dfrom_ValueChanged(sender As Object, e As EventArgs) Handles dfrom.DateTimeChanged
        set_description()
    End Sub

    Private Sub dto_ValueChanged(sender As Object, e As EventArgs) Handles dto.DateTimeChanged
        set_description()
    End Sub

    Private Sub Button2_Click(ByVal sender As Object, ByVal e As EventArgs)
        Me.cbotype.SelectedIndex = -1
        Me.cboauthor.SelectedIndex = -1
        Me.cboclass.SelectedIndex = -1
        Me.cbopropclass.SelectedIndex = -1
        Me.cboentity.Properties.Items.Clear()
        run_search()
        set_description()
    End Sub

    Private Sub bc_am_linked_documents_Resize(sender As Object, e As EventArgs) Handles Me.Resize
        Dim width As Double
        width = Me.lvall.Width
        Me.lvall.Columns(0).Width = width * 0.35
        Me.lvall.Columns(1).Width = width * 0.2
        Me.lvall.Columns(2).Width = width * 0.15
        Me.lvall.Columns(3).Width = width * 0.15
        Me.lvall.Columns(4).Width = width * 0.15


    End Sub

    Private Sub lvsel_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lvsel.DoubleClick
        For i = 0 To Me.lvsel.Nodes.Count
            If Me.lvsel.Nodes(i).Tag = Me.lvsel.Selection(0).Tag Then
                Me.lvsel.Nodes.RemoveAt(i)
                Exit Sub
            End If
        Next
    End Sub

    Private Sub lvsel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lvsel.FocusedNodeChanged

        If Me.lvsel.Selection.Count = 0 Then
            Exit Sub
        End If

        Me.uxMoveUp.Enabled = False
        Me.uxMoveDown.Enabled = False

        For i = 0 To Me.lvsel.Nodes.Count
            If Me.lvsel.Nodes(i).Tag = Me.lvsel.Selection(0).Tag Then
                If i > 0 Then
                    Me.uxMoveUp.Enabled = True
                End If
                If i < Me.lvsel.Nodes.Count - 1 Then
                    Me.uxMoveDown.Enabled = True
                End If
                Exit Sub
            End If
        Next

    End Sub

    Private Sub uxMoveUp_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxMoveUp.Click
        Try
            Dim vals(7) As String

            lvsel.BeginUpdate()

            For i = 0 To Me.lvsel.Nodes.Count - 1
                If Me.lvsel.Nodes(i).Tag = Me.lvsel.Selection(0).Tag Then
                    vals(0) = Me.lvsel.Nodes(i).GetValue(0)
                    vals(1) = Me.lvsel.Nodes(i).GetValue(1)
                    vals(2) = Me.lvsel.Nodes(i).GetValue(2)
                    vals(3) = Me.lvsel.Nodes(i).GetValue(3)
                    vals(4) = Me.lvsel.Nodes(i).GetValue(4)
                    vals(5) = Me.lvsel.Nodes(i).Tag
                    vals(6) = Me.lvsel.Nodes(i).GetValue(5)
                    Me.lvsel.Nodes(i).SetValue(0, Me.lvsel.Nodes(i - 1).GetValue(0))
                    Me.lvsel.Nodes(i).SetValue(1, Me.lvsel.Nodes(i - 1).GetValue(1))
                    Me.lvsel.Nodes(i).SetValue(2, Me.lvsel.Nodes(i - 1).GetValue(2))
                    Me.lvsel.Nodes(i).SetValue(3, Me.lvsel.Nodes(i - 1).GetValue(3))
                    Me.lvsel.Nodes(i).SetValue(4, Me.lvsel.Nodes(i - 1).GetValue(4))
                    Me.lvsel.Nodes(i).SetValue(5, Me.lvsel.Nodes(i - 1).GetValue(5))
                    Me.lvsel.Nodes(i).Tag = Me.lvsel.Nodes(i - 1).Tag
                    Me.lvsel.Nodes(i - 1).SetValue(0, vals(0))
                    Me.lvsel.Nodes(i - 1).SetValue(1, vals(1))
                    Me.lvsel.Nodes(i - 1).SetValue(2, vals(2))
                    Me.lvsel.Nodes(i - 1).SetValue(3, vals(3))
                    Me.lvsel.Nodes(i - 1).SetValue(4, vals(4))
                    Me.lvsel.Nodes(i - 1).Tag = vals(5)
                    Me.lvsel.Nodes(i - 1).SetValue(5, vals(6))
                    Me.lvsel.SetFocusedNode(Me.lvsel.Nodes(i - 1))
                    Me.lvsel.Focus()
                    Exit Sub
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("uxMoveUp", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            lvsel.EndUpdate()
        End Try
    End Sub

    Private Sub uxMoveDown_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxMoveDown.Click
        Try
            Dim vals(7) As String
            lvsel.BeginUpdate()
            For i = 0 To Me.lvsel.Nodes.Count - 1
                If Me.lvsel.Nodes(i).Tag = Me.lvsel.Selection(0).Tag Then
                    vals(0) = Me.lvsel.Nodes(i).GetValue(0)
                    vals(1) = Me.lvsel.Nodes(i).GetValue(1)
                    vals(2) = Me.lvsel.Nodes(i).GetValue(2)
                    vals(3) = Me.lvsel.Nodes(i).GetValue(3)
                    vals(4) = Me.lvsel.Nodes(i).GetValue(4)
                    vals(5) = Me.lvsel.Nodes(i).Tag
                    vals(6) = Me.lvsel.Nodes(i).GetValue(5)
                    Me.lvsel.Nodes(i).SetValue(0, Me.lvsel.Nodes(i + 1).GetValue(0))
                    Me.lvsel.Nodes(i).SetValue(1, Me.lvsel.Nodes(i + 1).GetValue(1))
                    Me.lvsel.Nodes(i).SetValue(2, Me.lvsel.Nodes(i + 1).GetValue(2))
                    Me.lvsel.Nodes(i).SetValue(3, Me.lvsel.Nodes(i + 1).GetValue(3))
                    Me.lvsel.Nodes(i).SetValue(4, Me.lvsel.Nodes(i + 1).GetValue(4))
                    Me.lvsel.Nodes(i).SetValue(5, Me.lvsel.Nodes(i + 1).GetValue(5))
                    Me.lvsel.Nodes(i).Tag = Me.lvsel.Nodes(i + 1).Tag
                    Me.lvsel.Nodes(i + 1).SetValue(0, vals(0))
                    Me.lvsel.Nodes(i + 1).SetValue(1, vals(1))
                    Me.lvsel.Nodes(i + 1).SetValue(2, vals(2))
                    Me.lvsel.Nodes(i + 1).SetValue(3, vals(3))
                    Me.lvsel.Nodes(i + 1).SetValue(4, vals(4))
                    Me.lvsel.Nodes(i + 1).Tag = vals(5)
                    Me.lvsel.Nodes(i + 1).SetValue(5, vals(6))
                    Me.lvsel.SetFocusedNode(Me.lvsel.Nodes(i + 1))
                    Me.lvsel.Focus()
                    Exit Sub
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("uxMoveDown", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            lvsel.EndUpdate()
        End Try
    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles RadioGroup1.SelectedIndexChanged

        If RadioGroup1.SelectedIndex = 1 Then

            Me.cboentity.Enabled = False
            Me.cboclass.Enabled = False
            Me.cbopropclass.Enabled = True
            Me.cbopropentity.Enabled = True
            If Me.cbopropentity.SelectedIndex > -1 Then
                set_description()
            End If
        Else
            Me.cboentity.Enabled = True
            Me.cboclass.Enabled = True
            Me.cbopropclass.Enabled = False
            Me.cbopropentity.Enabled = False
            If Me.cboentity.SelectedIndex > -1 Then
                set_description()
            End If

        End If
       

    End Sub

   
  
  
    Private Sub bc_am_dx_linked_documents_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub
End Class
Public Class Cbc_am_linked_documents
    Public bsave As Boolean = False
    Public linked_docs As New List(Of bc_om_document)
    Dim _doc As bc_om_document
    Dim _num_sel As Integer = 0
    Dim _analytic_mode As Boolean = False
    WithEvents _view As Ibc_am_linked_documents
    Public Sub New(ByVal view As Ibc_am_linked_documents, ByVal doc As bc_om_document, ByVal num_sel As Integer, analytic_mode As Boolean)
        _view = view
        _doc = doc
        _num_sel = num_sel
        _analytic_mode = analytic_mode
    End Sub
    Public Function load_data() As Boolean
        Try
            Dim select_params As New bc_om_doc_select_params
            select_params.doc = _doc
            select_params.pub_type_Id = _doc.pub_type_id

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                select_params.db_read()
            Else
                select_params.tmode = bc_cs_soap_base_class.tREAD
                If select_params.transmit_to_server_and_receive(select_params, True) = False Then
                    Exit Function
                End If
            End If

            _view.load_data(_num_sel, _doc, select_params, _analytic_mode)
            load_data = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_linked_documents", "search", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Public Sub save(ByVal docs As List(Of bc_om_document)) Handles _view.save
        linked_docs.Clear()
        For i = 0 To docs.Count - 1
            linked_docs.Add(docs(i))
        Next
        bsave = True
    End Sub
    Public Sub search(search_params As bc_om_doc_search_params) Handles _view.search
        Try
            search_params.analytic_mode = _analytic_mode
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                search_params.db_read()
            Else
                search_params.tmode = bc_cs_soap_base_class.tREAD
                If search_params.transmit_to_server_and_receive(search_params, True) = False Then
                    Exit Sub
                End If
            End If
            _view.load_search_results(search_params.search_results)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_linked_documents", "search", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
Public Interface Ibc_am_linked_documents
    Event save(ByVal docs As List(Of bc_om_document))
    Event search(search_params As bc_om_doc_search_params)
    Sub load_data(ByVal num_sel As Integer, ByVal doc As bc_om_document, ByVal select_params As bc_om_doc_select_params, analytic_mode As Boolean)
    Sub load_search_results(search_docs As List(Of bc_om_document))
End Interface



'Public Class Cbc_am_linked_documents
'    Public bsave As Boolean = False
'    Public linked_docs As New List(Of bc_om_document)
'    Dim _doc As bc_om_document
'    Dim _num_sel As Integer = 0

'    WithEvents _view As Ibc_am_linked_documents
'    Public Sub New(ByVal view As Ibc_am_linked_documents, ByVal doc As bc_om_document, ByVal num_sel As Integer)
'        _view = view
'        _doc = doc
'        _num_sel = num_sel
'    End Sub
'    Public Function load_data() As Boolean
'        Try
'            Dim select_params As New bc_om_doc_select_params
'            select_params.doc = _doc
'            select_params.pub_type_id = _doc.pub_type_id

'            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'                select_params.db_read()
'            Else
'                select_params.tmode = bc_cs_soap_base_class.tREAD
'                If select_params.transmit_to_server_and_receive(select_params, True) = False Then
'                    Exit Function
'                End If
'            End If

'            _view.load_data(_num_sel, _doc, select_params)
'            load_data = True
'        Catch ex As Exception
'            Dim oerr As New bc_cs_error_log("Cbc_am_linked_documents", "search", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        End Try
'    End Function
'    Public Sub save(ByVal docs As List(Of bc_om_document)) Handles _view.save
'        linked_docs.Clear()
'        For i = 0 To docs.Count - 1
'            linked_docs.Add(docs(i))
'        Next
'        bsave = True
'    End Sub
'    Public Sub search(search_params As bc_om_doc_search_params) Handles _view.search
'        Try

'            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'                search_params.db_read()
'            Else
'                search_params.tmode = bc_cs_soap_base_class.tREAD
'                If search_params.transmit_to_server_and_receive(search_params, True) = False Then
'                    Exit Sub
'                End If
'            End If
'            _view.load_search_results(search_params.search_results)
'        Catch ex As Exception
'            Dim oerr As New bc_cs_error_log("Cbc_am_linked_documents", "search", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        End Try
'    End Sub
'End Class
'Public Interface Ibc_am_linked_documents
'    Event save(ByVal docs As List(Of bc_om_document))
'    Event search(search_params As bc_om_doc_search_params)
'    Sub load_data(ByVal num_sel As Integer, ByVal doc As bc_om_document, ByVal select_params As bc_om_doc_select_params)
'    Sub load_search_results(search_docs As List(Of bc_om_document))
'End Interface


