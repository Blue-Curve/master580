Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS


Imports DevExpress.XtraTreeList
Imports System.Drawing
Imports System.Windows.Forms

Public Class bc_dx_as_categorize
    'Private authors_order As New List(Of author_order)
    'Private Class author_order
    '    Public id As Long
    '    Public name As String

    'End Class
    'llldd
    Private loading As Boolean = True
    Private _taxonomies As New List(Of bc_as_cat_taxonomy)
    Private entity_list As New bc_om_entities
    Public enable_lead As Boolean = False
    Public master_pub_type_id As Long
    Private pdf_count As Integer = 0

    Public Sub New()
        Try
            DevExpress.Skins.SkinManager.EnableFormSkins()
            DevExpress.UserSkins.BonusSkins.Register()
            DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")
            ' This call is required by the Windows Form Designer.
            InitializeComponent()
        Catch

        End Try
        ' Add any initialization after the InitializeComponent() call.

    End Sub



    Public _ok_selected As Boolean = False
    Public _doc As bc_om_document
    Private _show_local_submit As Boolean = False
    Private _show_attach_doc As Boolean = False
    Private _pub_types As List(Of String)
    Private _file As Boolean
    Private _enable_lead_entity As Boolean = False
    Private _show_ext As Boolean = False
    Private _behalf_of_authors_ids As New ArrayList
    Private _behalf_of_authors_names As New ArrayList
    Private _create_mode As Boolean = False
    Public ao_object As bc_ao_at_object
    Private scanned As Boolean = False
    Public attachment As Boolean = False
    Public regular_report_mode As Boolean = False
    Public regular_report_title As String
    Public regular_report_bglobal As Integer
    Private _import_master_mode As Boolean = False
    Public import_regular_report As Boolean = False
    Public import_support_document = False
    Public support_doc_cat = False
    Public Property document() As bc_om_document
        Get
            document = _doc
        End Get
        Set(ByVal value As bc_om_document)
            _doc = value
        End Set
    End Property
    Public Property taxonomies() As List(Of bc_as_cat_taxonomy)
        Get
            taxonomies = _taxonomies
        End Get
        Set(ByVal value As List(Of bc_as_cat_taxonomy))
            _taxonomies = value
        End Set
    End Property
    Public Property create_mode() As Boolean
        Get
            create_mode = _create_mode
        End Get
        Set(ByVal value As Boolean)
            _create_mode = value
        End Set
    End Property

    Public Property import_master_mode() As Boolean
        Get
            import_master_mode = _import_master_mode
        End Get
        Set(ByVal value As Boolean)
            _import_master_mode = value
        End Set
    End Property

    Public ReadOnly Property ok_selected() As Boolean
        Get
            ok_selected = _ok_selected
        End Get

    End Property

    Public ReadOnly Property filename() As String
        Get
            filename = Me.uxattach.Text
        End Get

    End Property
    Public WriteOnly Property show_stage_change()
        Set(ByVal value)
            'Me.pstagechange.Visible = value
            If value = True Then
                'Me.Height = Me.Height + Me.pstagechange.Height + Me.psupportdocs.Height + 20
            End If
        End Set
    End Property
    Public WriteOnly Property read_only()
        Set(ByVal value)
            If value = True Then
                Me.bok.Text = "Close"
                Me.bcancel.Visible = False
                Me.uxpages.Enabled = False
                Me.uxdate.Enabled = False
                Me.uxtitle.Enabled = False
                Me.uxsubtitle.Enabled = False
                Me.uxtitle.Enabled = False
                Me.uxsummary.Enabled = False
                Me.uxall.Visible = False
                Me.bup.Visible = False
                Me.bdn.Visible = False
                Me.tsearch.Visible = False
                Me.pclear.Visible = False

                Me.uxtime.Enabled = False
                Me.uxputypes.Enabled = False
                Dim dp As System.Drawing.Point
                dp.X = Me.uxall.Location.X
                dp.Y = Me.uxsel.Location.Y
                Me.uxsel.Location = dp
                Me.uxsel.Width = Me.uxtaxonomy.Width
                Me.uxsel.Height = Me.uxsel.Height + Me.tsearch.Height
                Me.pext.Visible = False
                Me.PictureEdit2.Visible = False
                Me.lsearch.Visible = False
                Me.blinks.Visible = False
                Me.balinks.Visible = False
            End If
        End Set
    End Property
    Public WriteOnly Property show_ext()
        Set(ByVal value)
            Me._show_ext = value
        End Set
    End Property
    Public WriteOnly Property enable_lead_entity()
        Set(ByVal value)

            Me._enable_lead_entity = value
        End Set
    End Property
    Public WriteOnly Property file()
        Set(ByVal value)
            If value = True Then
                Me.uxattach.Visible = True
                Me.btnattach.Visible = True
                Me.lmdoc.Visible = True

                _file = value
            End If
        End Set
    End Property
    Public WriteOnly Property set_pub_types()
        Set(ByVal value)
            _pub_types = value
        End Set
    End Property
    Public WriteOnly Property show_local_submit()
        Set(ByVal value)
            _show_local_submit = value
        End Set
    End Property

    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click

        Me.Hide()

    End Sub
    Public WriteOnly Property enable_pub_types()
        Set(ByVal value)
            Me.uxputypes.Properties.ReadOnly = value
        End Set
    End Property
    Public WriteOnly Property caption()
        Set(ByVal value)
            Me.Text = value
        End Set
    End Property
    Public WriteOnly Property ok_button_caption()
        Set(ByVal value)
            Me.bok.Text = value
        End Set
    End Property


    Private Sub bok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bok.Click
        Try

           

                Dim mandatory_ld As Boolean = False
                If support_doc_cat = False AndAlso Me.show_linked_docs = True Then
                    For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                        If bc_am_load_objects.obc_pub_types.pubtype(i).id = _doc.pub_type_id Then
                            If bc_am_load_objects.obc_pub_types.pubtype(i).mandatory_linked_doc = True Then

                                mandatory_ld = True
                            End If
                            Exit For
                        End If
                    Next

                    If mandatory_ld = True And _doc.linked_docs.Count = 0 Then
                        Dim omsg As New bc_cs_message("Blue Curve", "At least one linked research note (PDF) must be selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                    If mandatory_ld = True And pdf_count = 0 Then
                        Dim omsg As New bc_cs_message("Blue Curve", "At least one linked research note (PDF) must be selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                End If

                If Me.show_comments = True AndAlso Me.uxComment.Text <> "" Then
                    _doc.stage_change_comment = Me.uxComment.Text
                    If uxnextuser.SelectedIndex > -1 Then
                        _doc.assigned_user = sta.users(uxnextuser.SelectedIndex).id
                    End If

                    If Me.Chkcompletion.Checked = True Then
                        Dim da As Date
                        da = Me.DateEdit1.DateTime.Date
                        da = da.AddHours(Me.TimeEdit1.Time.Hour)
                        da = da.AddMinutes(Me.TimeEdit1.Time.Minute)
                        If da <= Now Then
                            Dim omsg As New bc_cs_message("Blue Curve", "Completion Date Cannot Be in the Past for Stage Change Comment", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                            Exit Sub
                        End If
                        _doc.stage_expire_date = da.ToUniversalTime
                    End If
                End If
                Me.Cursor = Cursors.WaitCursor

                If Me.bok.Text = "Close" Then
                    Me.Hide()
                    Exit Sub
                End If
                Me.instate = False
                If Me.chkinstateactions.Visible = True Then
                    Me.instate = True
                End If

                If Me.uxputypes.Text = "" Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Publication must be selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                If Trim(Me.uxtitle.Text) = "" Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Title must be entered", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                If _file = True And Trim(Me.uxattach.Text) = "" Then
                    Dim omsg As New bc_cs_message("Blue Curve", "File must be attached", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
                If Me.uxlead.Enabled = True And Me.uxlead.SelectedIndex = -1 Then
                    Dim omsg As New bc_cs_message("Blue Curve", Me.Llead.Text + " must be selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If

                If Me.regular_report_mode = True AndAlso Trim(Me.uxregularreportname.Text) = "" Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Regular Report Name  must be entered", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If

                _doc.urgent_flag = False
                If Me.chkurgent.Checked = True Then
                    If _doc.urgent_text = "" Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Document is urgent but no urgent text added please select details", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                    _doc.urgent_flag = True
                Else
                    _doc.urgent_text = ""
                    _doc.oec.Lists.Clear()
                End If

                If Me.support_doc_mandatory = True And _doc.support_documents.Count = 0 Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Support Document Must Be Attached", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If

                If Me.regular_report_mode = True Then
                    Me.regular_report_title = Me.uxregularreportname.Text
                    'If Me.rregularreporttype.SelectedIndex = 0 Then
                    '    Me.regular_report_bglobal = 1
                    'Else
                    '    Me.regular_report_bglobal = 0
                    'End If
                End If

                Dim user As bc_om_user = Nothing
                Dim tax As bc_om_taxonomy = Nothing

                Me._doc.title = Me.uxtitle.Text
                Me._doc.sub_title = Me.uxsubtitle.Text
                Me._doc.summary = Me.uxsummary.Text

                _doc.urgent_flag = Me.chkurgent.Checked
                _doc.doc_date = Me.uxdate.DateTime.Date
                _doc.doc_date = _doc.doc_date.AddHours(Me.uxtime.Time.Hour)
                _doc.doc_date = _doc.doc_date.AddMinutes(Me.uxtime.Time.Minute)
                _doc.doc_date = _doc.doc_date.ToUniversalTime

                If Me.uxpages.SelectedIndex > -1 Then
                    _doc.pages = Me.uxpages.SelectedIndex + 1
                End If

                _doc.authors.Clear()
            _doc.taxonomy.Clear()

            If bc_am_load_objects.obc_entities.disclosure_entities.Count > 0 And _doc.allow_disclosures = True Then

                _doc.disclosures.Clear()
            End If



            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).name = Me.uxputypes.Text Then
                    Me._doc.pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(i).id
                    If _doc.language_id = 0 Then
                        _doc.language_id = bc_am_load_objects.obc_pub_types.pubtype(i).language
                    End If
                    Exit For
                End If

            Next

            'For i = 0 To authors_order.Count - 1
            '    user = New bc_om_user
            '    user.id = authors_order(i).id
            '    _doc.authors.Add(user)
            'Next

            'If _doc.authors.Count = 0 Then
            '    Dim omsg As New bc_cs_message("Blue Curve", "At least 1 author must be selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            '    Exit Sub
            'End If


            Dim has_lead_entity As Boolean = False
            Dim tmp_analyst As List(Of bc_as_cat_taxonomy.bc_as_cat_taxononmy_item)
            Dim tmp_entity As List(Of bc_as_cat_taxonomy.bc_as_cat_taxononmy_item)
            Dim tmp_item As bc_as_cat_taxonomy.bc_as_cat_taxononmy_item
            tmp_entity = New List(Of bc_as_cat_taxonomy.bc_as_cat_taxononmy_item)
            tmp_analyst = New List(Of bc_as_cat_taxonomy.bc_as_cat_taxononmy_item)
            For i = 0 To _taxonomies.Count - 1
                _taxonomies(i).num_set = 0
            Next

            For i = 0 To _taxonomies.Count - 1
                For j = 0 To _taxonomies(i).items.Count - 1
                    If _taxonomies(i).items(j).selected = True Then
                        Select Case _taxonomies(i).type
                            Case 0
                                tmp_item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(_taxonomies(i).items(j).display_name, _taxonomies(i).items(j).id, True)
                                tmp_item.order = _taxonomies(i).items(j).order
                                tmp_analyst.Add(tmp_item)
                                'user = New bc_om_user
                                'user.id = _taxonomies(i).items(j).id
                                '_doc.authors.Add(user)
                            Case 2
                                tax = New bc_om_taxonomy
                                tax.entity_id = _taxonomies(i).items(j).id
                                _doc.disclosures.Add(tax)
                            Case 1
                                has_lead_entity = True
                                tmp_item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(_taxonomies(i).items(j).display_name, _taxonomies(i).items(j).id, True)
                                tmp_item.order = _taxonomies(i).items(j).order

                                tmp_entity.Add(tmp_item)


                                If _taxonomies(i).items(j).display_name = Me.uxlead.Text Then
                                    _doc.entity_id = _taxonomies(i).items(j).id
                                End If
                            Case 3
                                tmp_item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(_taxonomies(i).items(j).display_name, _taxonomies(i).items(j).id, True)
                                tmp_item.order = _taxonomies(i).items(j).order
                                tmp_entity.Add(tmp_item)
                                _taxonomies(i).num_set = _taxonomies(i).num_set + 1


                        End Select
                    ElseIf _taxonomies(i).type = 1 Then
                        If _taxonomies(i).items(j).display_name = Me.uxlead.Text Then
                            _doc.entity_id = _taxonomies(i).items(j).id
                        End If
                    End If
                Next
            Next

            For i = 0 To tmp_analyst.Count - 1
                For j = 0 To tmp_analyst.Count - 1
                    If tmp_analyst(j).order = i + 1 Then
                        user = New bc_om_user
                        user.id = tmp_analyst(j).id
                        _doc.authors.Add(user)
                        Exit For
                    End If
                Next
            Next
            Dim found As Boolean
            For i = 0 To tmp_entity.Count - 1
                For j = 0 To tmp_entity.Count - 1
                    If tmp_entity(j).order = i + 1 Then


                        found = False
                        tax = New bc_om_taxonomy
                        tax.entity_id = tmp_entity(j).id
                        For m = 0 To _doc.taxonomy.Count - 1
                            If _doc.taxonomy(m).entity_id = tax.entity_id Then
                                found = True
                                Exit For
                            End If
                        Next
                        If found = False Then
                            _doc.taxonomy.Add(tax)
                        End If
                    End If
                Next
            Next


            If _doc.authors.Count = 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "At least 1 author must be selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

            If has_lead_entity = False Then
                For i = 1 To _taxonomies.Count - 1
                    If _taxonomies(i).type = 1 Then
                        Dim omsg As New bc_cs_message("Blue Curve", "At least 1 " + _taxonomies(i).name + " must be selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                        Exit For
                    End If
                Next
            End If

            For i = 1 To _taxonomies.Count - 1
                If _taxonomies(i).type = 3 Then
                    If taxonomies(i).mandatory = True AndAlso taxonomies(i).num_set = 0 Then
                        If taxonomies(i).max_number = 1 Then
                            Dim omsg As New bc_cs_message("Blue Curve", _taxonomies(i).name + " must be selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Else
                            Dim omsg As New bc_cs_message("Blue Curve", "At least 1 " + _taxonomies(i).name + " must be selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        End If
                        Exit Sub
                        Exit For
                    End If
                    If taxonomies(i).max_number > 0 AndAlso taxonomies(i).num_set > taxonomies(i).max_number Then
                        If taxonomies(i).max_number = 1 Then
                            Dim omsg As New bc_cs_message("Blue Curve", "Only " + CStr(taxonomies(i).max_number) + " " + _taxonomies(i).name + " can be selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Else
                            Dim omsg As New bc_cs_message("Blue Curve", "Only " + CStr(taxonomies(i).max_number) + " " + _taxonomies(i).name + "(s) can be selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        End If
                        Exit Sub
                        Exit For
                    End If
                End If
            Next






            If _file = True Then
                Dim filename As String
                Dim extensionsize As Integer
                filename = Me.uxattach.Text
                REM SW cope with office versions
                extensionsize = (Len(filename) - (InStrRev(filename, ".") - 1))
                _doc.extension = filename.Substring(InStrRev(filename, ".") - 1, extensionsize)
                _doc.register_only = False
                _doc.filename = filename
                _doc.bwith_document = True
                Dim fs As New bc_cs_file_transfer_services


                If bc_am_load_objects.obc_pub_types.process_switches.import_if_open = True Then

                    If fs.write_document_to_bytestream_doc_open(filename, _doc.byteDoc, Nothing, True) = False Then
                        Dim omessage As New bc_cs_message("Blue Curve", "File: " + filename + " cannot be accessed.", bc_cs_message.MESSAGE, False, False, "Yes", "Ok", True)
                        Me._ok_selected = False
                        Exit Sub
                    End If

                Else
                    If fs.write_document_to_bytestream(filename, _doc.byteDoc, Nothing, False) = False Then
                        Dim omessage As New bc_cs_message("Blue Curve", "File: " + filename + " cannot be accessed please make sure you dont have it open.", bc_cs_message.MESSAGE, False, False, "Yes", "Ok", True)
                        Me._ok_selected = False
                        Exit Sub
                    End If
                End If
            End If

            If Me.Cbehalf.Visible = True Then
                _doc.originating_author = _behalf_of_authors_ids(Me.Cbehalf.SelectedIndex)
            End If



            If _create_mode = True Then

                REM if publish check if mandatory support doc attached
                If Me.uxstage.Text = "Publish" Then
                    For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                        If bc_am_load_objects.obc_pub_types.pubtype(i).id = _doc.pub_type_id Then
                            If bc_am_load_objects.obc_pub_types.pubtype(i).mandatory_default_support_doc = True Then
                                For j = 0 To _doc.support_documents.Count - 1
                                    If _doc.support_documents(j).pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(i).default_support_pub_type Then
                                        found = True
                                        Exit For
                                    End If
                                Next
                                If found = False Then
                                    Dim omsg As New bc_cs_message
                                    For j = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                                        If bc_am_load_objects.obc_pub_types.pubtype(j).id = bc_am_load_objects.obc_pub_types.pubtype(i).default_support_pub_type Then
                                            Dim omsgg As New bc_cs_message("Blue Curve", "Support Document: " + bc_am_load_objects.obc_pub_types.pubtype(j).name + " not attached prior to Publish. Do you wish to Continue?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                                            If omsgg.cancel_selected = True Then
                                                Exit Sub
                                            End If
                                            Exit For
                                        End If
                                    Next
                                End If
                            End If
                        End If
                    Next
                End If

                REM validate first FIL 5.3
                If Me.Cbehalf.Visible = True And Me.Cbehalf.SelectedIndex = -1 Then
                    Dim om As New bc_cs_message("Blue Curve", "Behalf of Author must be selected!", bc_cs_message.MESSAGE)
                    Exit Sub
                End If


                'If Me.Cbehalf.Visible = True Then

                '    _doc.originating_author = behalf_of_authors_ids(Me.Cbehalf.SelectedIndex)
                'End If
            End If



            _doc.urgent_flag = False

            If Me.chkurgent.Checked = True Then
                If _doc.urgent_text = "" Then
                    Dim bc_cs_message As New bc_cs_message("Blue Curve", "Document is important but no important text added please select details", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                _doc.urgent_flag = True
            Else
                _doc.urgent_text = ""
                _doc.oec.Lists.Clear()
            End If


            _doc.action_Ids.Clear()

            REM only update if workflow has changed
            If uxstage.SelectedIndex > 0 Then
                REM hold original stage
                _doc.original_stage = _doc.stage
                _doc.original_stage_name = _doc.stage_name
                _doc.stage = _doc.workflow_stages.stages(uxstage.SelectedIndex).stage_id
                _doc.stage_name = _doc.workflow_stages.stages(uxstage.SelectedIndex).stage_name
                For i = 0 To _doc.workflow_stages.stages(uxstage.SelectedIndex).action_ids.Count - 1
                    _doc.action_Ids.Add(_doc.workflow_stages.stages(uxstage.SelectedIndex).action_ids(i))
                Next

            End If

            _doc.custom_title = Me.tcustom.Text



            Me._ok_selected = True

            Me.Hide()


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_submit_frm", "submit_click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try

        REM CCC



    End Sub
    Public instate As Boolean = False
    Public show_urgent_flag As Boolean = False
    Public show_doc_scan As Boolean = False
    Public show_support_docs As Boolean = False
    Public show_linked_docs As Boolean = False
    Public num_analytic_docs As Integer = 0
    Public show_analytic_docs As Boolean = False
    Public linked_docs_count As Integer = 3
    Public support_doc_title As String = ""
    Public support_doc_filter As String = ""
    Public support_doc_mandatory As Boolean
    Public support_doc_maximum As Integer = 0
    Public prefix_text As String = ""
    Public show_comments As Boolean = False
    Public default_support_pub_type_id As Long
    Public max_title_length As Integer = 0
    Public max_sub_title_length As Integer = 0
    Public custom_field_name As String = ""
    Public custom_field_length As Integer = 0
    Public custom_field_default_value As String = ""
    Public html_editor_length As Integer = 0
    Public html_editor_default_font As String = ""
    Public html_editor_default_font_size As Double
    Public html_editor_only_bold_italic_underline As Boolean
    Public master_doc_filter As String = ""

    Public disable_title_st_summary As Boolean = False
    Public Sub load_data()
        Try

            If bc_am_load_objects.obc_pub_types.process_switches.sizeable_cat_screen = False Then
                Me.FormBorderStyle = Windows.Forms.FormBorderStyle.Fixed3D
                Me.MaximizeBox = False
            End If

            Me.searchtimer.Stop()
            Me.lsearchtimer.Stop()

            If bc_am_load_objects.obc_pub_types.process_switches.html_summary = True Then
                Me.uxsummary.Properties.ReadOnly = True
                Me.lsummary.Text = "Summary (double click to edit)"
            End If

            REM elimate pub types not i user business area
            Dim i As Integer = 0
            Dim pfound As Boolean = False
            While i < _pub_types.Count
                pfound = False
                For j = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(j).name = _pub_types(i) Then
                        For k = 0 To bc_am_load_objects.obc_prefs.bus_areas.Count - 1
                            If bc_am_load_objects.obc_prefs.bus_areas(k) = bc_am_load_objects.obc_pub_types.pubtype(j).bus_area_id Then
                                pfound = True
                                Exit For
                            End If
                        Next
                        If pfound = True Then
                            Exit For
                        End If
                    End If
                Next

                If pfound = False Then
                    _pub_types.RemoveAt(i)
                    i = i - 1

                End If
                i = i + 1
            End While
            If (_doc.pub_type_id = 0) Then
                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(i).Id = master_pub_type_id Then
                        If bc_am_load_objects.obc_pub_types.pubtype(i).hide_submission_date = True Then

                            Me.lsd.Visible = False
                            Me.uxdate.Visible = False
                            Me.uxtime.Visible = False
                        Else
                            Me.lsd.Visible = True
                            Me.uxdate.Visible = True
                            Me.uxtime.Visible = True
                        End If
                    End If
                Next
            End If
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).Id = _doc.pub_type_id Then

                    show_urgent_flag = bc_am_load_objects.obc_pub_types.pubtype(i).show_urgent_flag
                    show_doc_scan = bc_am_load_objects.obc_pub_types.pubtype(i).show_doc_scan
                    show_support_docs = bc_am_load_objects.obc_pub_types.pubtype(i).show_support_docs
                    show_linked_docs = bc_am_load_objects.obc_pub_types.pubtype(i).show_linked_docs
                    show_analytic_docs = bc_am_load_objects.obc_pub_types.pubtype(i).show_analytic_docs
                    num_analytic_docs = bc_am_load_objects.obc_pub_types.pubtype(i).num_analytic_docs
                    linked_docs_count = bc_am_load_objects.obc_pub_types.pubtype(i).linked_docs_count
                    support_doc_title = bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_title
                    support_doc_filter = bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_filter
                    support_doc_mandatory = bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_mandatory
                    support_doc_maximum = bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_maximum
                    prefix_text = bc_am_load_objects.obc_pub_types.pubtype(i).prefix_text
                    show_comments = bc_am_load_objects.obc_pub_types.pubtype(i).show_comments
                    max_title_length = bc_am_load_objects.obc_pub_types.pubtype(i).max_title_length
                    max_sub_title_length = bc_am_load_objects.obc_pub_types.pubtype(i).max_sub_title_length

                    custom_field_name = bc_am_load_objects.obc_pub_types.pubtype(i).custom_field_name
                    custom_field_length = bc_am_load_objects.obc_pub_types.pubtype(i).custom_field_length
                    custom_field_default_value = bc_am_load_objects.obc_pub_types.pubtype(i).custom_field_default_value

                    html_editor_length = bc_am_load_objects.obc_pub_types.pubtype(i).html_editor_length
                    html_editor_default_font = bc_am_load_objects.obc_pub_types.pubtype(i).html_editor_default_font
                    html_editor_default_font_size = bc_am_load_objects.obc_pub_types.pubtype(i).html_editor_default_font_size
                    html_editor_only_bold_italic_underline = bc_am_load_objects.obc_pub_types.pubtype(i).html_editor_only_bold_italic_underline
                    master_doc_filter = bc_am_load_objects.obc_pub_types.pubtype(i).master_doc_filter

                    disable_title_st_summary = bc_am_load_objects.obc_pub_types.pubtype(i).disable_title_st_summary

                    If disable_title_st_summary = True Then
                        disable_tss()
                      
                    End If

                    set_field_constraints()
                    If bc_am_load_objects.obc_pub_types.pubtype(i).hide_submission_date = True Then

                        Me.lsd.Visible = False
                        Me.uxdate.Visible = False
                        Me.uxtime.Visible = False
                    Else
                        Me.lsd.Visible = True
                        Me.uxdate.Visible = True
                        Me.uxtime.Visible = True
                    End If

                    Exit For
                End If
            Next

            If import_support_document = True Then
                Me.show_analytic_docs = False
                Me.show_linked_docs = False

            End If

            If regular_report_mode = True Then
                show_support_docs = False
                show_linked_docs = False
                show_comments = False
                show_analytic_docs = False
            End If

            If attachment = True Then
                If import_master_mode = False Then
                    show_linked_docs = False
                    show_analytic_docs = False
                End If
                show_comments = False
            End If

            'If import_master_mode = False Then
            '    If show_comments = False Then
            '        Me.tsuppdocs.TabPages.RemoveAt(2)
            '    End If
            '    If show_linked_docs = False Then
            '        Me.tsuppdocs.TabPages.RemoveAt(1)
            '    End If
            '    If show_support_docs = False Then
            '        Me.tsuppdocs.TabPages.RemoveAt(0)
            '    End If
            'End If
            If show_doc_scan = True And Me.uxtitle.Enabled = True Then
                Me.rallscan.Visible = True
                If create_mode = False Then
                    Me.rallscan.Properties.Items(2).Enabled = False
                    Me.rallscan.Visible = True
                End If
            End If

           
            Me.psupport.Visible = True
            Me.tsuppdocs.Visible = False

            If show_analytic_docs = False Then
                Me.tsuppdocs.TabPages(3).PageVisible = False
            End If
            If show_comments = False Then
                Me.tsuppdocs.TabPages(2).PageVisible = False
            End If
            If show_linked_docs = False Then
                Me.tsuppdocs.TabPages(1).PageVisible = False
            End If
            If show_support_docs = False Then
                Me.tsuppdocs.TabPages(0).PageVisible = False
            End If

            If Me.import_master_mode = True And _doc.pub_type_id <> 0 Then

                set_pt_specifics(_doc.pub_type_id)

            ElseIf Me.create_mode = False Then

                Me.pworkflow.Visible = False
                Me.tsuppdocs.TabPages(2).PageVisible = False
                Me.tsuppdocs.TabPages(0).PageVisible = False
                If show_linked_docs = True Or show_analytic_docs = True Then
                    Me.tsuppdocs.Visible = True
                    Me.Height = 714
                Else
                    Me.Height = 480
                End If

            Else

                Me.pworkflow.Visible = True
                If show_support_docs = True Or show_linked_docs = True Or show_comments = True Or show_analytic_docs = True Then
                    Me.tsuppdocs.Visible = True
                    Me.Height = 714

                    'If show_support_docs = False Then
                    '    Me.tsuppdocs.TabPages.RemoveAt(0)
                    'ElseIf show_linked_docs = False Then
                    '    Me.tsuppdocs.TabPages.RemoveAt(1)

                    'End If
                    If show_support_docs = True Then
                        If support_doc_title <> "" Then
                            Me.tsuppdocs.TabPages(0).Text = support_doc_title
                        End If
                    End If
                    If (show_linked_docs = True Or show_analytic_docs = True) And show_support_docs = True Then
                        Me.tsuppdocs.TabPages(1).Focus()
                        Me.tsuppdocs.TabPages(1).Select()
                    End If

                Else
                    Me.Height = 518
                End If
            End If

            Me.uxtitle.Text = Me._doc.title
            Me.uxsubtitle.Text = Me._doc.sub_title
            Me.uxsummary.Text = Me._doc.summary
            Me.disable_search = True
            Me.tcustom.Text = Me._doc.custom_title

            If bc_cs_central_settings.alt_entity_for_submit = True Then
                entity_list = bc_am_load_objects.obc_entities.alternate_entity_list
            Else
                entity_list = bc_am_load_objects.obc_entities
            End If




            If Not IsNothing(_pub_types) Then
                If _pub_types.Count = 1 Then

                    If Me.Text = "Blue Curve Process - Import Master Document" Or Me.Text = "Blue Curve Process - Register Master Document" Then
                        Me.uxputypes.Properties.Items.Add(_pub_types(0))
                        Me.uxputypes.SelectedIndex = -1
                        Me.uxputypes.Properties.ReadOnly = False
                    Else
                        Me.uxputypes.Text = _pub_types(0)
                    End If
                Else
                    Me.uxputypes.Properties.ReadOnly = False
                    For i = 0 To _pub_types.Count - 1
                        Me.uxputypes.Properties.Items.Add(_pub_types(i))
                        If _pub_types(i) = _doc.pub_type_name Then
                            Me.uxputypes.SelectedIndex = i
                        End If
                    Next

                End If
            End If

            For i = 1 To 1000
                Me.uxpages.Properties.Items.Add(CStr(i))
            Next

            If _doc.pages > 0 Then
                Me.uxpages.SelectedIndex = _doc.pages - 1
            End If
            If bc_cs_central_settings.deny_backdated_submission Then
                If _doc.doc_date < Now Then
                    _doc.doc_date = Now
                End If
            End If

            If _doc.doc_date = "1-Jan-0001" Then
                _doc.doc_date = Now
            End If
            Me.uxdate.DateTime = _doc.doc_date.ToLocalTime
            Me.uxtime.Time = _doc.doc_date.ToLocalTime

            load_taxonomy_data(_doc.pub_type_id)

            display_taxonomy()

            Me.display_data_for_taxonomy_page(0, False, False)

            Me.chkurgent.Checked = _doc.urgent_flag

            If Me._show_ext = True And _doc.checked_out_user = 0 And Me.bok.Text.ToLower <> "close" Then
                REM behalf of analyst

                If show_urgent_flag = False Then
                    Me.chkurgent.Visible = False
                    Me.bdetails.Visible = False
                Else
                    Me.pext.Visible = True
                End If

                If _doc.behalf_of_author_id > 0 And _doc.behalf_of_author_id = bc_cs_central_settings.logged_on_user_id Then
                    Dim sec As New bc_om_doc_security
                    sec.entity_id = _doc.entity_id
                    sec.pub_type_id = _doc.pub_type_id
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        sec.db_read()
                    Else
                        sec.tmode = bc_cs_soap_base_class.tREAD
                        sec.transmit_to_server_and_receive(sec, False)
                    End If
                    _behalf_of_authors_ids = sec.proxy_user_ids
                    _behalf_of_authors_names = sec.proxy_user_names
                    If _behalf_of_authors_names.Count > 1 Then
                        Me.Cbehalf.Visible = True
                        Me.lbehalf.Visible = True
                        Me.pext.Visible = True

                        For i = 0 To _behalf_of_authors_names.Count - 1
                            Me.Cbehalf.Properties.Items.Add(_behalf_of_authors_names(i))
                            If _behalf_of_authors_ids(i) = _doc.originating_author Then
                                Me.Cbehalf.SelectedIndex = i
                            End If
                        Next
                    End If
                End If

            End If


            If _create_mode Then
                REM set workflow stage
                Dim idx As Integer = 0
                Me.uxstage.Properties.Items.Clear()
                For i = 0 To _doc.workflow_stages.stages.Count - 1
                    REM stage routes
                    Me.uxstage.Properties.Items.Add(_doc.workflow_stages.stages(i).stage_name)
                    If _doc.workflow_stages.stages(i).stage_id = _doc.stage Then
                        idx = i
                    End If
                Next

                Me.uxstage.SelectedIndex = idx
                REM default save
                If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
                    Me.rcentral.SelectedIndex = 0
                Else
                    Me.rcentral.SelectedIndex = 1
                    Me.rcentral.Properties.Items(0).Enabled = False
                End If


                cspub.Properties.Items.Clear()
                Me.uxputypes.Enabled = False
                Me.PictureEdit2.Visible = False

                REM FIL JIRA 6835 set default sipport doc
                Dim default_support_pub_type_id As Long
                default_support_pub_type_id = 0
                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(i).id = _doc.pub_type_id Then
                        default_support_pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(i).default_support_pub_type
                        Exit For
                    End If
                Next
                Dim dpt As Integer
                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    cspub.Properties.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
                    REM FIL JIRA 6835 set default sipport doc
                    If default_support_pub_type_id <> 0 Then
                        If bc_am_load_objects.obc_pub_types.pubtype(i).id = default_support_pub_type_id Then
                            dpt = i
                        End If
                    Else
                        If bc_am_load_objects.obc_pub_types.pubtype(i).id = _doc.pub_type_id Then
                            dpt = i
                        End If
                    End If
                Next
                cspub.SelectedIndex = dpt


                If show_urgent_flag = True Then
                    Me.pext.Visible = True
                    Me.lbehalf.Visible = False
                    Me.Cbehalf.Visible = False
                Else
                    Me.pext.Visible = False
                End If
                load_support_docs()
            End If

            If show_linked_docs = True Then

                If _doc.id = 0 Then
                    _doc.linked_docs.Clear()

                    Dim select_params As New bc_om_doc_select_params
                    select_params.doc = _doc
                    select_params.pub_type_id = _doc.pub_type_id
                    select_params.analytic_mode = False
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        select_params.db_read()
                    Else
                        select_params.tmode = bc_cs_soap_base_class.tREAD
                        If select_params.transmit_to_server_and_receive(select_params, True) = False Then
                            Exit Sub
                        End If
                    End If
                    For i = 0 To select_params.def_docs.Count - 1
                        _doc.linked_docs.Add(select_params.def_docs(i))
                    Next
                End If
                load_linked_documents()

            End If
            If show_analytic_docs = True Then

                If _doc.id = 0 Then
                    _doc.analytic_docs.Clear()

                    Dim select_params As New bc_om_doc_select_params
                    select_params.doc = _doc
                    select_params.pub_type_id = _doc.pub_type_id
                    select_params.analytic_mode = True
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        select_params.db_read()
                    Else
                        select_params.tmode = bc_cs_soap_base_class.tREAD
                        If select_params.transmit_to_server_and_receive(select_params, True) = False Then
                            Exit Sub
                        End If
                    End If
                    For i = 0 To select_params.def_docs.Count - 1
                        _doc.analytic_docs.Add(select_params.def_docs(i))
                    Next
                End If
                load_analytic_documents()

            End If
            Me.disable_search = False
        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_dx_as_categorize", "load_data", bc_cs_error_codes.DB_ERR, ex.Message)

        End Try
    End Sub
    Private Sub load_support_docs()
        Try

            Dim tln As Nodes.TreeListNode = Nothing
            Dim bcs As New bc_cs_icon_services
            Dim image As Bitmap
            'Dim filter_attribute_id As Integer
            For i = 0 To Me.mimeimages.Images.Count - 1
                Me.mimeimages.Images.RemoveAt(0)
            Next

            Me.uxsupportdocs.Nodes.Clear()
            For i = 0 To _doc.support_documents.Count - 1
                With _doc.support_documents(i)
                    If _doc.support_documents(i).marked_for_delete = False Then
                        image = Nothing
                        image = bcs.get_icon_for_file_type(_doc.support_documents(i).extension)
                        If Not IsNothing(image) Then
                            Me.mimeimages.Images.Add(image)
                        End If

                        If .id = 0 Then
                            uxsupportdocs.AppendNode(New Object() {.pub_type_name}, tln).Tag = CStr(.title)
                        Else
                            uxsupportdocs.AppendNode(New Object() {.pub_type_name}, tln).Tag = CStr(.id)
                        End If
                        uxsupportdocs.Nodes(uxsupportdocs.Nodes.Count - 1).SetValue(1, .title)
                        uxsupportdocs.Nodes(uxsupportdocs.Nodes.Count - 1).SetValue(2, .filename)
                        If Not IsNothing(image) Then
                            uxsupportdocs.Nodes(uxsupportdocs.Nodes.Count - 1).StateImageIndex = Me.mimeimages.Images.Count - 1
                        End If
                    End If
                End With
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_at_categorize", "load_support_docs", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    Sub disable_tss()
        Me.uxtitle.Enabled = False
        Me.uxsubtitle.Enabled = False
        Me.uxsummary.Enabled = False
        If bc_am_load_objects.obc_pub_types.process_switches.html_summary = True Then
            Me.uxsummary.Properties.ReadOnly = True
            Me.lsummary.Text = ""
        End If
    End Sub

    Dim alt_list_class As New List(Of String)
    Dim alt_list_attribute As New List(Of String)
    Dim hold_disclosures As New ArrayList
    Private Sub load_taxonomy_data(ByVal pub_type_id As Long, Optional ByVal pub_type_change As Boolean = False)

        Dim lead_class_id As Long
        REM taxonomy first tab is always the author
        Try
            Me.disable_search = True
            Me.alt_list_class.Clear()
            Me.alt_list_attribute.Clear()


            Dim filter_attribute_id As Long = 0



            Dim bus_area_id As Long
            REM custom lists wf may 2017
            If bc_am_load_objects.obc_entities.filter_attributes_types.Count > 0 Then
                For k = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(k).id = pub_type_id Then
                        bus_area_id = bc_am_load_objects.obc_pub_types.pubtype(k).bus_area_id
                        Exit For
                    End If
                Next
                For k = 0 To bc_am_load_objects.obc_entities.filter_attributes_types.Count - 1
                    If bc_am_load_objects.obc_entities.filter_attributes_types(k).bus_area_id = bus_area_id And bc_am_load_objects.obc_entities.filter_attributes_types(k).use_in_submit = True Then
                        alt_list_class.Add(bc_am_load_objects.obc_entities.filter_attributes_types(k).class_id)
                        alt_list_attribute.Add(bc_am_load_objects.obc_entities.filter_attributes_types(k).attribute_id)

                    End If
                Next
            End If

            If pub_type_change = True Then

                For i = 0 To _taxonomies.Count - 1
                    _taxonomies.RemoveAt(0)
                Next
                Me.Llead.Text = "Lead"
                Me.uxlead.Properties.Items.Clear()
                Me.uxlead.Text = ""
                Me.uxlead.Enabled = False
                'Me.lsearch.Enabled = False
                'Me.tsearch.Enabled = False
                'Me.pclear.Enabled = False


                Me.llsearch.Visible = False
                Me.PictureEdit2.Visible = False

                Me.llsearch.Text = ""

            End If
            Dim otax As bc_as_cat_taxonomy

            otax = New bc_as_cat_taxonomy("Authors(*)", 0, 0, True, 0)
            _taxonomies.Add(otax)

            Dim pt As bc_om_pub_type = Nothing
            If pub_type_id > 0 Then





                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(i).id = pub_type_id Then
                        pt = bc_am_load_objects.obc_pub_types.pubtype(i)
                        Exit For
                    End If
                Next
                If pub_type_id > 0 Then
                    If pt.child_category > 0 Then
                        lead_class_id = pt.child_category
                        otax = New bc_as_cat_taxonomy("Lead Entity", 1, lead_class_id, True, 0)
                        _taxonomies.Add(otax)
                    End If
                End If
            End If

            If bc_am_load_objects.obc_entities.disclosure_entities.Count > 0 And _doc.allow_disclosures = True Then
                otax = New bc_as_cat_taxonomy("Disclosures", 2, 0, False, 0)
                _taxonomies.Add(otax)
           
            End If

            REM get user rold
            Dim role_id As Long
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                If bc_am_load_objects.obc_users.user(i).id = bc_cs_central_settings.logged_on_user_id Then
                    role_id = bc_am_load_objects.obc_users.user(i).role_id
                    Exit For
                End If
            Next


            If pub_type_id > 0 Then
                For j = 0 To pt.taxonomy.Count - 1
                
                    If pt.taxonomy(j).include_roles.Count = 0 Then
                      
                        otax = New bc_as_cat_taxonomy(pt.taxonomy(j).class_name, 3, pt.taxonomy(j).class_id, pt.taxonomy(j).mandatory, pt.taxonomy(j).max_number)
                        For m = 0 To pt.taxonomy(j).dependent_class_list.Count - 1
                            otax.dependent_class.Add(pt.taxonomy(j).dependent_class_list(m))
                        Next

                        _taxonomies.Add(otax)
                    Else

                        For k = 0 To pt.taxonomy(j).include_roles.Count - 1
                            If pt.taxonomy(j).include_roles(k) = role_id Then
                                otax = New bc_as_cat_taxonomy(pt.taxonomy(j).class_name, 3, pt.taxonomy(j).class_id, pt.taxonomy(j).mandatory, pt.taxonomy(j).max_number)
                                For m = 0 To pt.taxonomy(j).dependent_class_list.Count - 1
                                    otax.dependent_class.Add(pt.taxonomy(j).dependent_class_list(m))
                                Next
                                _taxonomies.Add(otax)
                                Exit For
                            End If
                        Next
                    End If


                Next
              
            End If
           

            Dim item As bc_as_cat_taxonomy.bc_as_cat_taxononmy_item = Nothing
            Dim found As Boolean = False

            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                found = False
                With bc_am_load_objects.obc_users.user(i)
                    For j = 0 To _doc.authors.Count - 1
                        If _doc.authors(j).id = .id Then
                            found = True

                            If bc_am_load_objects.obc_pub_types.process_switches.surname_first = True Then
                                item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(.surname + ", " + .first_name, .id, True)
                            Else
                                item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(.first_name + " " + .surname, .id, True)
                            End If

                            item.order = j + 1
                            item.mine = True
                            _taxonomies(0).items.Add(item)
                            Exit For
                        End If
                    Next
                    If found = False Then
                        If bc_am_load_objects.obc_pub_types.process_switches.surname_first = True Then
                            item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(.surname + ", " + .first_name, .id, False)
                        Else
                            item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(.first_name + " " + .surname, .id, False)
                        End If
                        _taxonomies(0).items.Add(item)
                    End If

                End With
            Next



            REM load disclosure taxonomy
            For j = 1 To _taxonomies.Count - 1
                If _taxonomies(j).type = 2 Then
                    For i = 0 To bc_am_load_objects.obc_entities.disclosure_entities.Count - 1
                        found = False
                        With bc_am_load_objects.obc_entities.disclosure_entities(i)
                            For k = 0 To _doc.disclosures.Count - 1
                                If _doc.disclosures(k).entity_id = .id Then
                                    _taxonomies(j).Class_id = .class_id
                                    item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(.name, .id, True)
                                    found = True
                                    Continue For
                                End If
                            Next
                            If found = False Then
                                item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(.name, .id, False)
                            End If
                            REM see if is a pref
                            For m = 0 To bc_am_load_objects.obc_prefs.pref.Count - 1
                                If bc_am_load_objects.obc_prefs.pref(m).entity_id = .id Then
                                    item.mine = True
                                    Exit For
                                End If
                            Next
                            _taxonomies(j).items.Add(item)
                        End With
                    Next
                    Exit For
                End If
            Next

            REM if disclosure class not set set now
            For j = 1 To _taxonomies.Count - 1
                If _taxonomies(j).type = 2 And _taxonomies(j).Class_id = 0 Then
                    If bc_am_load_objects.obc_entities.disclosure_entities.Count > 0 Then
                        taxonomies(j).Class_id = bc_am_load_objects.obc_entities.disclosure_entities(0).class_id
                    End If
                    Exit For
                End If
            Next

            Dim lead_name As String = ""
            Dim sel As Boolean
            Dim efound As Boolean = False
            Dim ord As Integer
            Dim new_class As Boolean = False
            Dim ec As Integer = 0
            Dim nec As Integer = 0
            Dim pclass_id As Long

            For i = 0 To entity_list.entity.Count - 1
                With entity_list.entity(i)
                    If pclass_id = 0 Or pclass_id <> .class_id Then
                        new_class = True
                        ord = 0
                        nec = ec
                    Else
                        new_class = False
                    End If
                    efound = False

                    sel = False
                    For k = 0 To _doc.taxonomy.Count - 1
                        If _doc.taxonomy(k).entity_Id = .id Then
                            ord = (k + 1) - nec
                            ec = ec + 1
                            sel = True
                            Exit For
                        End If
                    Next

                    For j = 1 To _taxonomies.Count - 1

                        Select Case _taxonomies(j).type
                            Case 1
                                If .class_id = lead_class_id Then
                                    filter_attribute_id = 0


                                    REM see if filtered list is used
                                    For f = 0 To alt_list_class.Count - 1
                                        If alt_list_class(f) = .class_id Then
                                            filter_attribute_id = alt_list_attribute(f)
                                            Exit For
                                        End If
                                    Next

                                    If filter_attribute_id = 0 Or bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                                        _taxonomies(j).Class_id = .class_id
                                        _taxonomies(j).name = .class_name
                                        Me.Llead.Text = "Lead " + .class_name
                                        If _enable_lead_entity = True Then
                                            Me.uxlead.Enabled = True
                                            Me.lsearch.Visible = True
                                            Me.llsearch.Visible = True
                                            Me.PictureEdit2.Visible = True
                                        End If

                                        item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(.name, .id, sel)
                                        If sel = True Then

                                            item.order = ord
                                        End If
                                        If Me.show_doc_scan = True Then
                                            For m = 0 To bc_am_load_objects.obc_prefs.pref.Count - 1
                                                If bc_am_load_objects.obc_prefs.pref(m).entity_id = .id Then
                                                    item.mine = True
                                                    Exit For
                                                End If
                                            Next
                                        End If


                                        _taxonomies(j).items.Add(item)
                                        Me.uxlead.Properties.Items.Add(.name)

                                        If _doc.entity_id = .id Then
                                            lead_name = .name
                                        End If
                                    End If
                                End If
                            Case 3



                                If .class_name = _taxonomies(j).name Then
                                    filter_attribute_id = 0



                                    REM see if filtered list is used
                                    For f = 0 To alt_list_class.Count - 1
                                        If alt_list_class(f) = .class_id Then
                                            filter_attribute_id = alt_list_attribute(f)
                                            Exit For
                                        End If
                                    Next

                                 
                                    _taxonomies(j).Class_id = .class_id

                                    If filter_attribute_id = 0 Or bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                                        item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(.name, .id, sel)
                                        If sel = True Then
                                            item.order = ord
                                        End If
                                        If Me.show_doc_scan = True Then
                                            For m = 0 To bc_am_load_objects.obc_prefs.pref.Count - 1
                                                If bc_am_load_objects.obc_prefs.pref(m).entity_id = .id Then
                                                    item.mine = True
                                                    Exit For
                                                End If
                                            Next
                                        End If
                                        _taxonomies(j).items.Add(item)

                                    End If
                                End If
                        End Select
                    Next
                    pclass_id = .class_id


                End With

            Next

            REM dependent lists
            If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
                For i = 0 To _taxonomies.Count - 1
                    If _taxonomies(i).dependent_class.Count = 1 Then
                        REM get dependent list from server
                        reset_page_for_dependent_taxonomy(_taxonomies(i).dependent_class(0), _taxonomies(i).Class_id, i)
                    End If
                Next
            End If
            REM filtered lists
            If pub_type_id > 0 AndAlso bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
                Dim fentities As List(Of bc_om_entity)
                Dim classes_loaded As New List(Of Long)
                Dim attributes_of_class As New List(Of cloti)
                Dim afound As Boolean
                REM now do filtered lists
                For i = 0 To alt_list_class.Count - 1

                    For j = 0 To _taxonomies.Count - 1
                        If alt_list_class(i) = _taxonomies(j).Class_id Then
                            afound = False
                            ord = 0
                            If _taxonomies(j).type = 2 Then
                                _taxonomies(j).items.Clear()
                            End If
                            Dim ofatt As New bc_om_filter_attribute
                            For k = 0 To classes_loaded.Count - 1
                                If classes_loaded(k) = _taxonomies(j).Class_id Then
                                    fentities = attributes_of_class(k).cloti
                                    afound = True
                                    Exit For
                                End If
                            Next

                            If afound = False Then

                                ofatt = New bc_om_filter_attribute
                                ofatt.attribute_id = alt_list_attribute(i)
                                ofatt.class_id = _taxonomies(j).Class_id

                                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                                    ofatt.db_read()
                                Else
                                    ofatt.tmode = bc_cs_soap_base_class.tREAD
                                    If ofatt.transmit_to_server_and_receive(ofatt, True) = False Then
                                        Exit Sub
                                    End If
                                End If
                                classes_loaded.Add(ofatt.class_id)
                                Dim oti As bc_om_entity
                                Dim loti = New List(Of bc_om_entity)
                                For m = 0 To ofatt.results.Count - 1
                                    oti = New bc_om_entity
                                    oti.id = ofatt.results(m)
                                    loti.Add(oti)
                                Next


                                Dim fss = From t In entity_list.entity
                                          Join p In loti On p.id Equals t.id
                                         Select t
                                fentities = fss.ToList
                                Dim clo As New cloti
                                clo.cloti = fentities
                                attributes_of_class.Add(clo)
                            End If
                            ord = 0

                            Dim class_start As Integer = 0

                            REM get taxonmy for this class only
                            Dim lltax As New List(Of bc_om_entity)
                            Dim lltaxe As bc_om_entity
                            For y = 0 To _doc.taxonomy.Count - 1
                                lltaxe = New bc_om_entity
                                lltaxe.id = _doc.taxonomy(y).entity_id
                                lltax.Add(lltaxe)
                            Next
                            Dim ltax = From t In lltax
                                         Join p In fentities On p.id Equals t.id
                                        Select t
                            Dim fltax As List(Of bc_om_entity)
                            fltax = ltax.ToList


                            For k = 0 To fentities.Count - 1
                                sel = False

                                If _taxonomies(j).type = 2 Then
                                    For l = 0 To _doc.disclosures.Count - 1
                                        If _doc.disclosures(l).entity_Id = fentities(k).id Then
                                            ord = ord + 1
                                            sel = True
                                            Exit For
                                        End If
                                    Next
                                Else
                                    For l = 0 To fltax.Count - 1
                                        If fltax(l).id = fentities(k).id Then
                                            ord = l + 1
                                            sel = True
                                            Exit For
                                        End If

                                    Next
                                End If
                                item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(fentities(k).name, fentities(k).id, sel)

                                If sel = True Then
                                    item.order = ord
                                End If
                                _taxonomies(j).items.Add(item)

                                If _taxonomies(j).Class_id = lead_class_id Then
                                    Me.Llead.Text = "Lead " + fentities(0).class_name
                                    If _enable_lead_entity = True Then
                                        Me.uxlead.Enabled = True
                                        Me.lsearch.Visible = True
                                        Me.llsearch.Visible = True
                                        Me.PictureEdit2.Visible = True
                                    End If

                                    Me.uxlead.Properties.Items.Add(fentities(k).name)

                                    If _doc.entity_id = fentities(k).id Then
                                        lead_name = fentities(k).name
                                    End If

                                End If

                            Next
                            If Me.show_doc_scan = True Then
                                set_mine_list(j)
                            End If
                        End If
                    Next
                Next
            End If


            If lead_name <> "" Then
                For i = 0 To Me.uxlead.Properties.Items.Count - 1
                    If Me.uxlead.Properties.Items(i).ToString = lead_name Then
                        Me.uxlead.SelectedIndex = i
                        Exit For
                    End If
                Next
            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_as_categorize", "load_taxonomy_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.disable_search = False
        End Try
    End Sub

    Private Sub set_dynamic_mine()

        If Me.uxtaxonomy.SelectedTabPageIndex <> 0 Then
            Exit Sub
        End If
        If alt_list_class.Count = 0 Then
            Exit Sub
        End If
        For i = 0 To alt_list_class.Count - 1
            For j = 0 To _taxonomies.Count - 1
                If _taxonomies(j).Class_id = alt_list_class(i) Then
                    set_mine_list(j)
                End If

            Next
        Next
    End Sub
    Private Sub set_mine_list(page As Integer)

        REM get prefs for all classified users
        Dim prefs As New bc_om_get_prefs_for_users

        prefs.class_id = _taxonomies(page).Class_id

        For a = 0 To _taxonomies(0).items.Count - 1
            If _taxonomies(0).items(a).selected = True Then
                prefs.users.Add(_taxonomies(0).items(a).id)
            End If
        Next
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            prefs.db_read()
        Else
            prefs.tmode = bc_cs_soap_base_class.tREAD
            If prefs.transmit_to_server_and_receive(prefs, True) = False Then
                Exit Sub
            End If
        End If

        For i = 0 To _taxonomies(page).items.Count - 1
            _taxonomies(page).items(i).mine = False
            For m = 0 To prefs.prefs.Count - 1
                If prefs.prefs(m).entity_id = _taxonomies(page).items(i).id Then
                    _taxonomies(page).items(i).mine = True
                    Exit For
                End If
            Next
        Next


    End Sub
    Private Sub display_taxonomy()
        REM taxonomy
        REM always have one page
        'uxtaxonomy.TabPages(0).Text = "Authors(*)"
        For i = 1 To uxtaxonomy.TabPages.Count - 1
            uxtaxonomy.TabPages.RemoveAt(1)
        Next
        If _taxonomies.Count > 0 Then
            uxtaxonomy.TabPages(0).Text = _taxonomies(0).name
            For i = 1 To _taxonomies.Count - 1
                If _taxonomies(i).max_number = 0 Then
                    If _taxonomies(i).mandatory = True AndAlso _taxonomies(i).dependent_class.Count > 0 Then
                        uxtaxonomy.TabPages.Add(_taxonomies(i).name + "(*~)")
                    ElseIf _taxonomies(i).mandatory = True Then
                        uxtaxonomy.TabPages.Add(_taxonomies(i).name + "(*)")
                    ElseIf _taxonomies(i).dependent_class.Count > 0 Then
                        uxtaxonomy.TabPages.Add(_taxonomies(i).name + "(~)")
                    Else
                        uxtaxonomy.TabPages.Add(_taxonomies(i).name)
                    End If
                Else
                    If _taxonomies(i).mandatory = True AndAlso _taxonomies(i).dependent_class.Count > 0 Then
                        uxtaxonomy.TabPages.Add(_taxonomies(i).name + "(*~" + CStr(_taxonomies(i).max_number) + ")")
                    ElseIf _taxonomies(i).mandatory = True Then
                        uxtaxonomy.TabPages.Add(_taxonomies(i).name + "(*" + CStr(_taxonomies(i).max_number) + ")")
                    ElseIf _taxonomies(i).dependent_class.Count > 0 Then
                        uxtaxonomy.TabPages.Add(_taxonomies(i).name + "(~" + CStr(_taxonomies(i).max_number) + ")")
                    Else
                        uxtaxonomy.TabPages.Add(_taxonomies(i).name + "(" + CStr(_taxonomies(i).max_number) + ")")
                    End If

                End If
            Next
        End If

    End Sub
    Private Class ord_item
        Public name As String
        Public order As Integer

        Public Sub New(ByVal iname As String, ByVal iorder As Integer)
            name = iname
            order = iorder
        End Sub
    End Class

    Dim explicit_names As New List(Of String)
    Dim bdt As bc_om_display_taxonomy

    Private Sub display_data_for_taxonomy_page(ByVal page As Integer, from_rb As Boolean, from_tab As Boolean, Optional ByVal idx As Integer = 0)
        Try


            Me.Cursor = Windows.Forms.Cursors.WaitCursor
            Me.bup.Visible = True
            Me.bdn.Visible = True
            explicit_names.Clear()

            If page > 0 AndAlso from_tab = True AndAlso bc_am_load_objects.obc_pub_types.process_switches.show_implicit_tagging = True Then


                Me.bup.Visible = False
                Me.bdn.Visible = False
                bdt = New bc_om_display_taxonomy

               
                
                bdt.single_class_id = _taxonomies(page).Class_id
                Dim tdoc As New bc_om_document
                Dim tttax As bc_om_taxonomy

                For i = 0 To _taxonomies.Count - 1
                    If _taxonomies(i).Class_id > 0 Then
                        For j = 0 To _taxonomies(i).items.Count - 1
                            If _taxonomies(i).items(j).selected = True Then
                                tttax = New bc_om_taxonomy
                                tttax.entity_id = _taxonomies(i).items(j).id
                                tdoc.taxonomy.Add(tttax)
                            End If
                        Next
                    End If
                Next
                bdt.read_mode = 1
                bdt.doc = tdoc
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    bdt.db_read_single_class()
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    bdt.tmode = bc_cs_soap_base_class.tREAD
                    bdt.transmit_to_server_and_receive(bdt, True)
                End If

            End If


            Dim ord_items As New List(Of ord_item)
            Dim ord_item As ord_item

            Me.uxsel.Items.Clear()
            Me.uxall.Items.Clear()
            Me.uxall.BeginUpdate()
            Me.uxsel.BeginUpdate()
            Me.rallscan.Properties.Items(0).Description = "All"
            If Me.rallscan.Properties.Items(2).Enabled = False Then
                Dim dp As System.Drawing.Point
                dp.X = Me.uxsel.Location.X
                dp.Y = Me.rallscan.Location.Y
                Me.rallscan.Width = Me.uxsel.Width
                Me.rallscan.Location = dp

            End If


            REM custom lists wf may 2017
            For i = 0 To bc_am_load_objects.obc_entities.filter_attributes_types.Count - 1
                If bc_am_load_objects.obc_entities.filter_attributes_types(i).class_id = _taxonomies(page).Class_id And bc_am_load_objects.obc_entities.filter_attributes_types(i).use_in_submit = True Then
                    Dim bus_area_id As Integer
                    For k = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                        If bc_am_load_objects.obc_pub_types.pubtype(k).id = _doc.pub_type_id Then
                            bus_area_id = bc_am_load_objects.obc_pub_types.pubtype(k).bus_area_id
                            Exit For
                        End If
                    Next

                    If bc_am_load_objects.obc_entities.filter_attributes_types(i).bus_area_id = bus_area_id Then
                        Me.rallscan.Properties.Items(0).Description = "All " + bc_am_load_objects.obc_entities.filter_attributes_types(i).display_name
                        'If from_rb = False Then
                        '    If bc_am_load_objects.obc_entities.filter_attributes_types(i).mine = True Then
                        '        Me.rallscan.SelectedIndex = 1
                        '    Else
                        '        Me.rallscan.SelectedIndex = 0
                        '    End If
                        'End If
                        Exit For
                    End If
                End If
            Next




            If bc_am_load_objects.obc_pub_types.process_switches.surname_first = True Then
                If page = 0 Then
                    Me.uxall.SortOrder = SortOrder.Ascending
                Else
                    Me.uxall.SortOrder = SortOrder.None
                End If
            End If

            Dim bsearch As Boolean = False
            If Me.tsearch.Text <> "" Then
                bsearch = True
            End If



            For i = 0 To _taxonomies(page).items.Count - 1
                If _taxonomies(page).items(i).selected = True Then
                    If _taxonomies(page).type = 2 Then
                        Me.uxsel.Items.Add(_taxonomies(page).items(i).display_name)
                        explicit_names.Add(_taxonomies(page).items(i).display_name)
                    Else
                        ord_item = New ord_item(_taxonomies(page).items(i).display_name, _taxonomies(page).items(i).order)
                        ord_items.Add(ord_item)
                    End If
                ElseIf _taxonomies(page).items(i).selected = False And (bsearch = False Or _taxonomies(page).items(i).searched = True) And (Me.rallscan.SelectedIndex = 0 Or Me.rallscan.SelectedIndex = -1) Then
                    Me.uxall.Items.Add(_taxonomies(page).items(i).display_name)
                ElseIf _taxonomies(page).items(i).selected = False And (bsearch = False Or _taxonomies(page).items(i).searched = True) And Me.rallscan.SelectedIndex = 1 And _taxonomies(page).items(i).mine = True Then
                    Me.uxall.Items.Add(_taxonomies(page).items(i).display_name)
                ElseIf _taxonomies(page).items(i).selected = False And (bsearch = False Or _taxonomies(page).items(i).searched = True) And Me.rallscan.SelectedIndex = 2 And _taxonomies(page).items(i).scanned = True Then
                    Me.uxall.Items.Add(_taxonomies(page).items(i).display_name)

                End If
            Next
            REM put seleced list in correct order
            If _taxonomies(page).type <> 2 Then
                For i = 0 To ord_items.Count - 1
                    For j = 0 To ord_items.Count - 1
                        If ord_items(j).order = i + 1 Then
                            Me.uxsel.Items.Add(ord_items(j).name)
                            explicit_names.Add(ord_items(j).name)
                        End If
                    Next

                Next
            End If

            If page > 0 AndAlso Not IsNothing(bdt) AndAlso bc_am_load_objects.obc_pub_types.process_switches.show_implicit_tagging = True Then
                If page > 0 Then
                    Me.bup.Visible = False
                    Me.bdn.Visible = False
                End If
                For i = 0 To bdt.implicit_tags.Count - 1

                    Dim found As Boolean = False
                    For j = 0 To explicit_names.Count - 1
                        If explicit_names(j) = bdt.implicit_tags(i).name Then
                            found = True
                            Exit For
                        End If

                    Next
                    If found = False Then
                        Me.uxsel.Items.Add("*" + CStr(bdt.implicit_tags(i).name))
                    End If
                Next
            End If

            If idx <> 0 Then
                Me.uxsel.SelectedIndex = idx
            End If

            Me.check_up_down_buttons()


        Catch ex As Exception
            'MsgBox(ex.Message)



        Finally
            Me.uxall.EndUpdate()
            Me.uxsel.EndUpdate()
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try

    End Sub


    Private disable_search As Boolean = False

    Private Sub uxtaxonomy_LocationChanged(sender As Object, e As EventArgs) Handles uxtaxonomy.LocationChanged

    End Sub
    Private Sub dependent_parameters(page As Integer)
        Try
            REM dependent parameters#
            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                Exit Sub
            End If

            REM see if other pages depend on this page
            Dim this_class_id As Long
            Dim sdep As String = ""
            this_class_id = _taxonomies(page).Class_id
            For i = 0 To _taxonomies.Count - 1
                If _taxonomies(i).dependent_class.Count = 1 Then
                    If this_class_id = _taxonomies(i).dependent_class(0) Then
                        If sdep = "" Then
                            sdep = _taxonomies(i).name
                        Else
                            sdep = sdep + " and " + _taxonomies(i).name
                        End If
                        reset_page_for_dependent_taxonomy(this_class_id, _taxonomies(i).Class_id, i)
                    End If
                End If
            Next
            If sdep <> "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "You have changed " + _taxonomies(page).name + " which " + sdep + " depends on  please check and reassign " + sdep, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_as_categorize", "dependent_parameters", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Private Sub reset_page_for_dependent_taxonomy(source_class_id As Long, target_class_id As Long, targetpage As Integer)
        Try

            Dim odep As New bc_om_dependent_taxonomy
            odep.target_class_id = target_class_id
            Dim selitems As New List(Of bc_as_cat_taxonomy.bc_as_cat_taxononmy_item)
            Dim bc As bc_as_cat_taxonomy.bc_as_cat_taxononmy_item





            For j = 0 To _taxonomies(targetpage).items.Count - 1
                If _taxonomies(targetpage).items(j).selected = True Then
                    bc = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(_taxonomies(targetpage).items(j).display_name, _taxonomies(targetpage).items(j).id, True)
                    bc.order = _taxonomies(targetpage).items(j).order
                    selitems.Add(bc)
                End If
            Next
            REM now order selected items
            Dim fss = From t In selitems Select t Order By t.order
            selitems = fss.ToList
            _taxonomies(targetpage).items.Clear()

            For i = 0 To _taxonomies.Count - 1
                If _taxonomies(i).Class_id = source_class_id Then
                    For j = 0 To _taxonomies(i).items.Count - 1
                        If _taxonomies(i).items(j).selected = True Then
                            odep.dependent_entities.Add(_taxonomies(i).items(j).id)
                        End If
                    Next
                End If
            Next

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                odep.db_read()
            Else
                odep.tmode = bc_cs_soap_base_class.tREAD
                If odep.transmit_to_server_and_receive(odep, True) = False Then
                    Exit Sub
                End If
            End If
            _taxonomies(targetpage).items = odep.results

            REM reset preious selected taht are still valid
            Dim ord As Integer
            ord = 1
            For i = 0 To selitems.Count - 1
                For j = 0 To _taxonomies(targetpage).items.Count - 1
                    If selitems(i).id = _taxonomies(targetpage).items(j).id Then
                        _taxonomies(targetpage).items(j).selected = True
                        _taxonomies(targetpage).items(j).order = ord
                        ord = ord + 1
                        Exit For
                    End If
                Next
            Next


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_as_categorize", "reset_page_for_dependent_taxonomy", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try


    End Sub
    Private Sub uxtaxonomy_SelectedPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles uxtaxonomy.SelectedPageChanged
        Try
            disable_search = True
            'dependent_parameters(uxtaxonomy.SelectedTabPageIndex)

            If Me.tsearch.Text = "" Then
                For i = 0 To bc_am_load_objects.obc_entities.filter_attributes_types.Count - 1
                    If bc_am_load_objects.obc_entities.filter_attributes_types(i).class_id = _taxonomies(uxtaxonomy.SelectedTabPageIndex).Class_id And bc_am_load_objects.obc_entities.filter_attributes_types(i).use_in_submit = True Then

                        Dim bus_area_id As Integer
                        For k = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                            If bc_am_load_objects.obc_pub_types.pubtype(k).id = _doc.pub_type_id Then
                                bus_area_id = bc_am_load_objects.obc_pub_types.pubtype(k).bus_area_id
                                Exit For
                            End If
                        Next

                        If bc_am_load_objects.obc_entities.filter_attributes_types(i).bus_area_id = bus_area_id Then
                            Me.rallscan.Properties.Items(0).Description = "All " + bc_am_load_objects.obc_entities.filter_attributes_types(i).display_name
                            If bc_am_load_objects.obc_entities.filter_attributes_types(i).mine = True Then
                                Me.rallscan.SelectedIndex = 1
                            Else
                                Me.rallscan.SelectedIndex = 0
                            End If

                            Me.display_data_for_taxonomy_page(uxtaxonomy.SelectedTabPageIndex, False, True)
                            Exit Sub
                        End If
                    End If
                Next

                If Me.rallscan.SelectedIndex <> 0 Then
                    Me.rallscan.SelectedIndex = 0
                Else
                    Me.display_data_for_taxonomy_page(uxtaxonomy.SelectedTabPageIndex, False, True)
                End If
            Else
                Me.tsearch.Text = ""
            End If

            disable_search = False
            If uxtaxonomy.SelectedTabPageIndex = 0 Then
                check_up_down_buttons()
            End If
        Catch ex As Exception
            MsgBox(ex.Message)

        Finally
            disable_search = False
        End Try

    End Sub



    Private Sub bc_as_categorize_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        Try
            loading = True


            load_data()

            If create_mode = True AndAlso _doc.id = 0 Then
                load_regular_reports()
            End If

            loading = False
            If Me.uxputypes.Enabled = True Then
                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(i).name = Me.uxputypes.Text Then

                        'load_taxonomy_data(bc_am_load_objects.obc_pub_types.pubtype(i).id, True)
                        display_taxonomy()
                        Me.display_data_for_taxonomy_page(0, False, False)
                        Exit For
                    End If
                Next

            End If

            If enable_lead = True Then
                Me.uxlead.Enabled = True

            End If

            If regular_report_mode = True Then
                Me.uxpages.Visible = False
                Me.uxdate.Visible = False
                Me.LabelControl5.Visible = False
                Me.lsd.Visible = False
                Me.uxtime.Visible = False
                Me.pregularreport.Visible = True
                Me.uxregularreportname.Text = regular_report_title
                'If regular_report_bglobal = False Then
                '    Me.rregularreporttype.SelectedIndex = 1
                'End If

            End If
            If import_master_mode = True And import_regular_report = False Then

                enable_disable_controls(False)
            End If
            If Me.uxputypes.Text = "" Then

                Me.uxdate.Visible = False
                Me.uxtime.Visible = False
                Me.lsd.Visible = False

            End If
        Catch

        End Try
    End Sub
    Sub enable_disable_controls(enable As Boolean)
        Me.uxtitle.Enabled = enable
        Me.uxsubtitle.Enabled = enable
        Me.uxsummary.Enabled = enable
        Me.bok.Enabled = enable
        Me.uxall.Enabled = enable
        Me.uxsel.Enabled = enable
        Me.btnattach.Enabled = enable

        Me.lsearch.Enabled = enable
        Me.tsearch.Enabled = enable
        Me.pclear.Enabled = enable
    End Sub
    Private updating As Boolean
    Private lupdating As Boolean

    Private Sub tsearch_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsearch.EditValueChanged
        If updating = False And loading = False Then
            searchtimer.Stop()
            searchtimer.Start()
        End If
    End Sub
    Private Sub lSearchTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lsearchtimer.Tick
        lsearchtimer.Stop()
        lrunsearch(llsearch.Text)
    End Sub
    Private Sub SearchTimer_Tick(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles searchtimer.Tick
        searchtimer.Stop()
        runsearch(tsearch.Text)
    End Sub
    Private Sub runsearch(ByVal tx As String)
        Try

            If disable_search = True Then
                Exit Sub
            End If

            Me.Cursor = Cursors.WaitCursor


            updating = True
           
            REM extended search for entities
            Dim found_entities As New List(Of Long)
            If tx <> "" Then
                If _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).name <> "Authors(*)" Then
                    REM real time extended search
                    Dim search_results As New bc_om_real_time_search

                    search_results.class_id = _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).Class_id

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

                    REM end real time extended search
                    'For i = 0 To Me.entity_list.search_attributes.search_values.Count - 1
                    '    If Me.entity_list.search_attributes.search_values(i).class_id = _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).Class_id AndAlso InStr(UCase(Me.entity_list.search_attributes.search_values(i).value), UCase(tx)) > 0 Then
                    '        found_entities.Add(Me.entity_list.search_attributes.search_values(i).entity_id)
                    '    End If
                    'Next
                End If
            End If


            For i = 0 To _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items.Count - 1
                If tx <> "" Then
                    _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).searched = False
                    If InStr(UCase(_taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).display_name), UCase(tx)) > 0 And _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).selected = False Then
                        _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).searched = True
                    Else
                        REM try extended search o attributes
                        For j = 0 To found_entities.Count - 1
                            If found_entities(j) = _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).id Then
                                _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).searched = True
                                Exit For
                            End If

                        Next
                    End If
                Else
                    _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).searched = False
                End If
            Next




            display_data_for_taxonomy_page(Me.uxtaxonomy.SelectedTabPageIndex, True, False)




        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Cursor = Cursors.Default

            updating = False
        End Try
    End Sub

    Private Sub lrunsearch(ByVal tx As String)
        Try
            If disable_search = True Then
                Exit Sub
            End If

            Me.Cursor = Cursors.WaitCursor


            lupdating = True

            REM extended search for entities

            Dim found_entities As New List(Of Long)
            If tx <> "" Then
                Dim search_results As New bc_om_real_time_search
                search_results.class_id = _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).Class_id
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
                'For i = 0 To Me.entity_list.search_attributes.search_values.Count - 1
                '    If Me.entity_list.search_attributes.search_values(i).class_id = _taxonomies(1).Class_id AndAlso InStr(UCase(Me.entity_list.search_attributes.search_values(i).value), UCase(tx)) > 0 Then
                '        found_entities.Add(Me.entity_list.search_attributes.search_values(i).entity_id)
                '    End If
                'Next
            End If
            Me.uxlead.Properties.Items.Clear()
            For i = 0 To _taxonomies(1).items.Count - 1
                If tx <> "" Then
                    If InStr(UCase(_taxonomies(1).items(i).display_name), UCase(tx)) > 0 Then
                        Me.uxlead.Properties.Items.Add(_taxonomies(1).items(i).display_name)
                    Else
                        REM try extended search o attributes
                        For j = 0 To found_entities.Count - 1
                            If found_entities(j) = _taxonomies(1).items(i).id Then
                                Me.uxlead.Properties.Items.Add(_taxonomies(1).items(i).display_name)
                                Exit For
                            End If

                        Next
                    End If
                Else
                    Me.uxlead.Properties.Items.Add(_taxonomies(1).items(i).display_name)
                End If
            Next


        Catch ex As Exception

        Finally
            Me.Cursor = Cursors.Default

            lupdating = False
        End Try
    End Sub

    Private Sub uxall_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxall.DoubleClick

        sel_item()
    End Sub
    Private Sub uxsel_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxsel.DoubleClick
        If Me.uxall.Visible = False Then
            Exit Sub
        End If
        If uxsel.SelectedItem(0).ToString.Substring(0, 1) = "*" Then
            Dim omsg As New bc_cs_message("Blue Curve", "Cannot remove implicit tagging", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Else
            del_item()
        End If
    End Sub

    Private Sub sel_item()

        If _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).max_number > 0 AndAlso Me.uxsel.Items.Count = _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).max_number Then
            If taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).max_number = 1 Then
                Dim omsg As New bc_cs_message("Blue Curve", "Only " + CStr(taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).max_number) + " " + _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).name + " can be selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Only " + CStr(taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).max_number) + " " + _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).name + "(s) can be selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If

            Exit Sub
        End If
        Dim max_ord As Integer
        For i = 0 To Me.uxsel.Items.Count - 1
            If Me.uxsel.Items(i).ToString.Substring(0, 1) <> "*" Then
                max_ord = max_ord + 1
            Else
                Exit For
            End If
        Next


        For i = 0 To _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items.Count - 1
            If _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).display_name = Me.uxall.SelectedItems(0) Then
                _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).selected = True
                REM _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).order = Me.uxsel.Items.Count + 1
                _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).order = max_ord + 1

                REM if class is disclosre class add this to disclosure as well
                If bc_am_load_objects.obc_pub_types.process_switches.auto_assign_disclosures = True Then
                    For j = 0 To _taxonomies.Count - 1
                        If _taxonomies(j).Class_id = _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).Class_id And
                            _taxonomies(j).name = "Disclosures" And _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).name <> "Disclosures" Then
                            For k = 0 To _taxonomies(j).items.Count - 1
                                If _taxonomies(j).items(i).id = _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).id Then
                                    REM turn this off for wells fargo
                                    _taxonomies(j).items(i).selected = True
                                    Exit For
                                End If
                            Next
                            Exit For
                        End If
                    Next
                End If
                display_data_for_taxonomy_page(Me.uxtaxonomy.SelectedTabPageIndex, True, False)
                set_dynamic_mine()
                dependent_parameters(Me.uxtaxonomy.SelectedTabPageIndex)
                Exit Sub
            End If
        Next
        'display_data_for_taxonomy_page(Me.uxtaxonomy.SelectedTabPageIndex, True)



    End Sub


    Private Sub del_item()

        Dim sel As Integer
        sel = 0
        If _taxonomies(uxtaxonomy.SelectedTabPageIndex).type = 2 Then
            For i = 0 To _taxonomies(uxtaxonomy.SelectedTabPageIndex).items.Count - 1
                If _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).display_name = Me.uxsel.Text Then
                    _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order = 0
                    _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).selected = False
                End If
            Next
        Else
            For i = 0 To _taxonomies(uxtaxonomy.SelectedTabPageIndex).items.Count - 1
                If _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).selected = True And _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order = Me.uxsel.SelectedIndex + 1 Then
                    _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order = 0
                    _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).selected = False
                    If bc_am_load_objects.obc_pub_types.process_switches.auto_assign_disclosures = True Then
                        For j = 0 To _taxonomies.Count - 1
                            If _taxonomies(j).Class_id = _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).Class_id And
                                _taxonomies(j).name = "Disclosures" And _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).name <> "Disclosures" Then
                                For k = 0 To _taxonomies(j).items.Count - 1
                                    If _taxonomies(j).items(i).id = _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).id Then
                                        REM turn this off for wells fargo
                                        _taxonomies(j).items(i).selected = False
                                        Exit For
                                    End If
                                Next
                                Exit For
                            End If
                        Next
                    End If


                ElseIf _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).selected = True And _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order > Me.uxsel.SelectedIndex Then
                    _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order = _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order - 1
                End If

            Next
        End If
        Me.display_data_for_taxonomy_page(uxtaxonomy.SelectedTabPageIndex, True, True)
        set_dynamic_mine()

        dependent_parameters(Me.uxtaxonomy.SelectedTabPageIndex)
    End Sub

    Private Sub btnattach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)

        Try
            Dim odialog As New OpenFileDialog
            Dim extensionsize As Integer
            odialog.Title = "Import Document"
            odialog.ShowDialog()
            Me.uxattach.Text = odialog.FileName

            If Len(Me.uxtitle.Text) = 0 Then

                extensionsize = (Len(odialog.SafeFileName) - (InStrRev(odialog.SafeFileName, ".") - 1))
                Me.uxtitle.Text = Microsoft.VisualBasic.Left(odialog.SafeFileName, Len(odialog.SafeFileName) - extensionsize)
            End If
        Catch

        End Try

    End Sub

    Private Sub uxputypes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxputypes.SelectedIndexChanged
        If loading = True Then
            Exit Sub
        End If
        Try
            Me.Cursor = Cursors.WaitCursor

            Dim pt_id As Long
            If uxputypes.SelectedIndex > -1 Then
                enable_disable_controls(True)
            End If

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).name = Me.uxputypes.Text Then
                    pt_id = bc_am_load_objects.obc_pub_types.pubtype(i).id
                    _doc.pub_type_id = pt_id
                    show_urgent_flag = bc_am_load_objects.obc_pub_types.pubtype(i).show_urgent_flag
                    show_doc_scan = bc_am_load_objects.obc_pub_types.pubtype(i).show_doc_scan
                    show_support_docs = bc_am_load_objects.obc_pub_types.pubtype(i).show_support_docs
                    show_linked_docs = bc_am_load_objects.obc_pub_types.pubtype(i).show_linked_docs
                    show_analytic_docs = bc_am_load_objects.obc_pub_types.pubtype(i).show_analytic_docs
                    num_analytic_docs = bc_am_load_objects.obc_pub_types.pubtype(i).num_analytic_docs
                    linked_docs_count = bc_am_load_objects.obc_pub_types.pubtype(i).linked_docs_count
                    support_doc_title = bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_title
                    support_doc_filter = bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_filter
                    support_doc_mandatory = bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_mandatory
                    support_doc_maximum = bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_maximum
                    prefix_text = bc_am_load_objects.obc_pub_types.pubtype(i).prefix_text
                    default_support_pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(i).default_support_pub_type
                    max_title_length = bc_am_load_objects.obc_pub_types.pubtype(i).max_title_length
                    max_sub_title_length = bc_am_load_objects.obc_pub_types.pubtype(i).max_sub_title_length

                    custom_field_name = bc_am_load_objects.obc_pub_types.pubtype(i).custom_field_name
                    custom_field_length = bc_am_load_objects.obc_pub_types.pubtype(i).custom_field_length
                    custom_field_default_value = bc_am_load_objects.obc_pub_types.pubtype(i).custom_field_default_value

                    html_editor_length = bc_am_load_objects.obc_pub_types.pubtype(i).html_editor_length
                    html_editor_default_font = bc_am_load_objects.obc_pub_types.pubtype(i).html_editor_default_font
                    html_editor_default_font_size = bc_am_load_objects.obc_pub_types.pubtype(i).html_editor_default_font_size
                    html_editor_only_bold_italic_underline = bc_am_load_objects.obc_pub_types.pubtype(i).html_editor_only_bold_italic_underline
                    master_doc_filter = bc_am_load_objects.obc_pub_types.pubtype(i).master_doc_filter

                    disable_title_st_summary = bc_am_load_objects.obc_pub_types.pubtype(i).disable_title_st_summary

                    If disable_title_st_summary = True Then
                        Me.uxtitle.Enabled = False
                        Me.uxsubtitle.Enabled = False
                        Me.uxsummary.Enabled = False
                    End If



                    If bc_am_load_objects.obc_pub_types.pubtype(i).hide_submission_date = True Then
                        Me.lsd.Visible = False
                        Me.uxdate.Visible = False
                        Me.uxtime.Visible = False
                    Else
                        Me.lsd.Visible = True
                        Me.uxdate.Visible = True
                        Me.uxtime.Visible = True
                    End If



                    Exit For
                End If
            Next
            Me.rallscan.Visible = False
            If show_doc_scan = True Then
                Me.rallscan.Visible = True
                If create_mode = False Then
                    Me.rallscan.Properties.Items(2).Enabled = False
                    Me.rallscan.Visible = True
                End If
            End If


            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).name = Me.uxputypes.Text Then
                    load_taxonomy_data(bc_am_load_objects.obc_pub_types.pubtype(i).id, True)
                    display_taxonomy()
                    Me.display_data_for_taxonomy_page(0, False, False)
                    Exit For
                End If
            Next
            If regular_report_mode = True Then
                set_field_constraints()

            End If
            If import_master_mode = True Then
                set_pt_specifics(pt_id)
                If show_linked_docs = True Then
                    If _doc.id = 0 Then
                        _doc.linked_docs.Clear()
                        Dim select_params As New bc_om_doc_select_params
                        select_params.doc = _doc
                        select_params.pub_type_id = _doc.pub_type_id
                        select_params.analytic_mode = False
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            select_params.db_read()
                        Else
                            select_params.tmode = bc_cs_soap_base_class.tREAD
                            If select_params.transmit_to_server_and_receive(select_params, True) = False Then
                                Exit Sub
                            End If
                        End If
                        For i = 0 To select_params.def_docs.Count - 1
                            _doc.linked_docs.Add(select_params.def_docs(i))
                        Next
                    End If
                    load_linked_documents()
                End If
                If show_analytic_docs = True Then
                    If _doc.id = 0 Then
                        _doc.analytic_docs.Clear()
                        Dim select_params As New bc_om_doc_select_params
                        select_params.doc = _doc
                        select_params.pub_type_id = _doc.pub_type_id
                        select_params.analytic_mode = True
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            select_params.db_read()
                        Else
                            select_params.tmode = bc_cs_soap_base_class.tREAD
                            If select_params.transmit_to_server_and_receive(select_params, True) = False Then
                                Exit Sub
                            End If
                        End If
                        For i = 0 To select_params.def_docs.Count - 1
                            _doc.analytic_docs.Add(select_params.def_docs(i))
                        Next
                    End If
                    load_analytic_documents()
                End If

            End If
        Catch ex As Exception
            MsgBox(ex.Message)

        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub
    Public Sub set_pt_specifics(pt_id As Long)
        Try

            Me.tsuppdocs.TabPages(0).PageVisible = True
            Me.tsuppdocs.TabPages(1).PageVisible = True
            Me.tsuppdocs.TabPages(2).PageVisible = True
            Me.tsuppdocs.TabPages(3).PageVisible = True
            Me.tsuppdocs.Visible = False
            If import_master_mode = True Then

                Me.pworkflow.Visible = False
                If show_support_docs = True Or show_linked_docs = True Or show_comments = True Or show_analytic_docs = True Then
                    Me.tsuppdocs.Visible = True
                    Me.Height = 714

                    If show_support_docs = True Then
                        If support_doc_title <> "" Then
                            Me.tsuppdocs.TabPages(0).Text = support_doc_title
                        End If
                    End If
                    If (show_linked_docs = True Or show_analytic_docs = True) And show_support_docs = True Then
                        Me.tsuppdocs.TabPages(1).Focus()
                        Me.tsuppdocs.TabPages(1).Select()
                    End If

                Else
                    Me.Height = 518
                End If
                If show_analytic_docs = False Then
                    Me.tsuppdocs.TabPages(3).PageVisible = False
                End If
                If show_comments = False Then
                    Me.tsuppdocs.TabPages(2).PageVisible = False
                End If
                If show_linked_docs = False Then
                    Me.tsuppdocs.TabPages(1).PageVisible = False
                End If
                If show_support_docs = False Then
                    Me.tsuppdocs.TabPages(0).PageVisible = False
                End If
                
                If show_support_docs = True Then
                    cspub.Properties.Items.Clear()
                    Dim dpt As Integer
                    For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                        cspub.Properties.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).name)
                        REM FIL JIRA 6835 set default sipport doc
                        If default_support_pub_type_id <> 0 Then
                            If bc_am_load_objects.obc_pub_types.pubtype(i).id = default_support_pub_type_id Then
                                dpt = i
                            End If
                        Else
                            If bc_am_load_objects.obc_pub_types.pubtype(i).id = pt_id Then
                                dpt = i
                            End If
                        End If
                    Next
                    If dpt > -1 Then
                        cspub.SelectedIndex = dpt
                    End If

                End If
                If show_linked_docs = True Then

                    If _doc.id = 0 Then

                        Dim select_params As New bc_om_doc_select_params
                        select_params.doc = _doc
                        select_params.pub_type_id = _doc.pub_type_id
                        select_params.analytic_mode = False
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            select_params.db_read()
                        Else
                            select_params.tmode = bc_cs_soap_base_class.tREAD
                            If select_params.transmit_to_server_and_receive(select_params, True) = False Then
                                Exit Sub
                            End If
                        End If
                        For i = 0 To select_params.def_docs.Count - 1
                            _doc.linked_docs.Add(select_params.def_docs(i))
                        Next
                    End If
                    load_linked_documents()
                End If
                If show_analytic_docs = True Then

                    If _doc.id = 0 Then

                        Dim select_params As New bc_om_doc_select_params
                        select_params.doc = _doc
                        select_params.pub_type_id = _doc.pub_type_id
                        select_params.analytic_mode = True
                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            select_params.db_read()
                        Else
                            select_params.tmode = bc_cs_soap_base_class.tREAD
                            If select_params.transmit_to_server_and_receive(select_params, True) = False Then
                                Exit Sub
                            End If
                        End If
                        For i = 0 To select_params.def_docs.Count - 1
                            _doc.analytic_docs.Add(select_params.def_docs(i))
                        Next
                    End If
                    load_analytic_documents()
                End If
                set_field_constraints()

            End If

        Catch ex As Exception


        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub
    Sub set_field_constraints()

        If support_doc_cat = True Then
            Exit Sub
        End If


        If max_title_length > 0 Then
            Me.uxtitle.Text = ""
            Me.uxtitle.Properties.MaxLength = max_title_length
        Else
            Me.uxtitle.Properties.MaxLength = 250
        End If

        If max_sub_title_length > 0 Then
            Me.uxsubtitle.Text = ""
            Me.uxsubtitle.Properties.MaxLength = max_sub_title_length
        Else
            max_sub_title_length = 250
        End If
        Me.lcustom.Text = ""
        Me.lcustom.Visible = False
        Me.tcustom.Text = ""
        Me.tcustom.Visible = False

        Me.uxsummary.Height = 58

        If custom_field_name <> "" Then
            Me.uxsummary.Height = 20
            Me.lcustom.Text = custom_field_name
            Me.lcustom.Visible = True
            Me.tcustom.Visible = True

            If custom_field_length > 0 Then
                Me.tcustom.Properties.MaxLength = custom_field_length
            Else
                Me.tcustom.Properties.MaxLength = 250
            End If
            Me.tcustom.Text = custom_field_default_value
        End If
    End Sub
    Private Sub uxsel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxsel.SelectedIndexChanged
        check_up_down_buttons()
    End Sub
    Private Sub check_up_down_buttons()
        Me.bdn.Enabled = False
        Me.bup.Enabled = False
        If _taxonomies(uxtaxonomy.SelectedTabPageIndex).type = 2 Then
            Exit Sub
        End If
        If Me.uxsel.SelectedIndex < Me.uxsel.Items.Count - 1 Then
            Me.bdn.Enabled = True
        End If

        If Me.uxsel.SelectedIndex > 0 Then
            Me.bup.Enabled = True
        End If
    End Sub


    Private Sub bup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bup.Click
        Dim sel As Integer
        sel = 0
        For i = 0 To _taxonomies(uxtaxonomy.SelectedTabPageIndex).items.Count - 1
            If _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).selected = True And _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order = Me.uxsel.SelectedIndex + 1 Then
                _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order = _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order - 1
            ElseIf _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).selected = True And _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order = Me.uxsel.SelectedIndex Then
                _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order = _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order + 1
            End If

        Next
        Me.display_data_for_taxonomy_page(uxtaxonomy.SelectedTabPageIndex, True, Me.uxsel.SelectedIndex - 1)
    End Sub
    Private Sub bdn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdn.Click
        Dim sel As Integer
        sel = 0
        For i = 0 To _taxonomies(uxtaxonomy.SelectedTabPageIndex).items.Count - 1
            If _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).selected = True And _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order = Me.uxsel.SelectedIndex + 1 Then
                _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order = _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order + 1
            ElseIf _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).selected = True And _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order = Me.uxsel.SelectedIndex + 2 Then
                _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order = _taxonomies(uxtaxonomy.SelectedTabPageIndex).items(i).order - 1
            End If

        Next
        Me.display_data_for_taxonomy_page(uxtaxonomy.SelectedTabPageIndex, True, Me.uxsel.SelectedIndex + 1)
    End Sub

    Private Sub uxComment_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub uxcheckout_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxcheckout.CheckedChanged

    End Sub

    Private Sub uxkeepopen_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxkeepopen.CheckedChanged
        If uxcheckout.Checked = True Then
            uxcheckout.Enabled = False
            uxcheckout.Checked = True
        Else
            uxcheckout.Enabled = True
        End If
    End Sub






    Private Sub btnattach_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnattach.Click
        Dim odialog As New OpenFileDialog
        Dim extensionsize As Integer
        odialog.Title = "Import Document"
        Try
            If import_support_document = True Then
                support_doc_filter = ""
                For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                    If bc_am_load_objects.obc_pub_types.pubtype(i).name = Me.uxputypes.Text Then
                        support_doc_filter = bc_am_load_objects.obc_pub_types.pubtype(i).support_doc_filter

                    End If
                Next
                odialog.Filter = support_doc_filter
            Else
                odialog.Filter = master_doc_filter
            End If

            odialog.ShowDialog()
            Me.uxattach.Text = odialog.FileName
            If Len(Me.uxattach.Text) > 2 Then
                If Len(Me.uxtitle.Text) = 0 Then
                    extensionsize = (Len(odialog.SafeFileName) - (InStrRev(odialog.SafeFileName, ".") - 1))
                    REM WF dont prerppulate title
                    If bc_am_load_objects.obc_pub_types.process_switches.set_def_master_support_title = True Then
                        Me.uxtitle.Text = Microsoft.VisualBasic.Left(odialog.SafeFileName, Len(odialog.SafeFileName) - extensionsize)
                    End If

                End If
            End If
        Catch ex As Exception
            'MsgBox(ex.Message)


        End Try
    End Sub

    Private Sub uxlead_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxlead.SelectedIndexChanged
        If Me.uxlead.Enabled = True Then
            For i = 0 To _taxonomies(1).items.Count - 1
                If _taxonomies(1).items(i).display_name = Me.uxlead.Text Then
                    _taxonomies(1).items(i).selected = True
                    _taxonomies(1).items(i).order = 1
                    Me.uxtaxonomy.SelectedTabPageIndex = 1
                    Me.display_data_for_taxonomy_page(1, True, False)

                    REM if class is disclosre class add this to disclosure as well
                    If bc_am_load_objects.obc_pub_types.process_switches.auto_assign_disclosures = True Then
                        For j = 0 To _taxonomies.Count - 1
                            If _taxonomies(j).Class_id = _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).Class_id And
                                _taxonomies(j).name = "Disclosures" And _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).name <> "Disclosures" Then
                                For k = 0 To _taxonomies(j).items.Count - 1
                                    If _taxonomies(j).items(i).id = _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).id Then

                                        _taxonomies(j).items(i).selected = True
                                        Exit For
                                    End If
                                Next
                                Exit For
                            End If
                        Next
                    End If


                    Exit For
                End If

            Next
        End If
    End Sub

    Private Sub llsearch_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles llsearch.EditValueChanged
        If lupdating = False And loading = False Then
            lsearchtimer.Stop()
            lsearchtimer.Start()
        End If
    End Sub

    Private Sub bdetails_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdetails.Click
        Dim vfgc As New bc_dx_extended_class
        Dim cfgc As New cttrl_bc_am_extended_class(vfgc, _doc)
        cfgc.load_data()
        vfgc.TopMost = True
        vfgc.ShowDialog()
    End Sub

    Private Sub chkurgent_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkurgent.CheckedChanged
        If Me.chkurgent.Checked = True Then
            Me.bdetails.Enabled = True
        Else
            Me.bdetails.Enabled = False
        End If


    End Sub

    Private Sub uxall_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxall.SelectedIndexChanged

    End Sub

    Private Sub uxAddSupportDocument_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAddSupportDocument.Click
        Try
            Dim sc As Integer = 0
            For i = 0 To _doc.support_documents.Count - 1
                If (_doc.support_documents(i).marked_for_delete = False) Then
                    sc = sc + 1
                End If
            Next

            If support_doc_maximum <> 0 And sc >= support_doc_maximum Then
                Dim omsg As New bc_cs_message("Blue Curve", "You can only attach a maximum of " + CStr(support_doc_maximum) + " support documents", bc_cs_message.MESSAGE, False, False, "Ok", "Cancel", True)
                Exit Sub
            End If

            REM =============
            Dim odialog As New OpenFileDialog
            odialog.Title = "Import Document"
            If support_doc_filter <> "" Then
                odialog.Filter = support_doc_filter
            End If

            odialog.ShowDialog()

            If odialog.FileName = "" Then
                Exit Sub
            End If

            Dim sdoc As New bc_om_document
            sdoc.id = 0
            sdoc.master_flag = False

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).name = Me.cspub.Text Then
                    sdoc.pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(i).id
                    Exit For
                End If
            Next

            sdoc.pub_type_name = Me.cspub.Text
            sdoc.originating_author = _doc.originating_author
            sdoc.bus_area = _doc.bus_area
            sdoc.checked_out_user = 0
            sdoc.stage = _doc.stage

            sdoc.entity_id = _doc.entity_id
            sdoc.originating_author = _doc.originating_author

            Dim a As bc_om_user
            Dim t As bc_om_taxonomy
            For i = 0 To _doc.authors.Count - 1
                a = New bc_om_user
                a.id = _doc.authors(i).id
                sdoc.authors.Add(a)
            Next

            For i = 0 To _doc.taxonomy.Count - 1
                t = New bc_om_taxonomy
                t.entity_id = _doc.taxonomy(i).entity_id
                t.name = _doc.taxonomy(i).name
                t.class_id = _doc.taxonomy(i).class_id
                t.class_name = _doc.taxonomy(i).class_name
                sdoc.taxonomy.Add(t)
            Next
            For i = 0 To _doc.disclosures.Count - 1
                t = New bc_om_taxonomy
                t.entity_id = _doc.disclosures(i).entity_id
                t.name = _doc.disclosures(i).name
                t.class_id = _doc.disclosures(i).class_id
                t.class_name = _doc.disclosures(i).class_name
                sdoc.disclosures.Add(t)
            Next


            sdoc.workflow_stages = _doc.workflow_stages

            sdoc.sub_title = Me.uxtitle.Text
            sdoc.doc_date = Now.ToUniversalTime
            Dim filename As String
            filename = odialog.FileName

            sdoc.extension = filename.Substring(InStrRev(filename, ".") - 1, filename.Length - InStrRev(filename, ".") + 1)
            sdoc.register_only = False
            sdoc.filename = filename
            sdoc.bwith_document = True

            sdoc.title = Microsoft.VisualBasic.Left(odialog.SafeFileName, Len(odialog.SafeFileName) - Len(sdoc.extension))

            Dim fs As New bc_cs_file_transfer_services
            REM FIL FEB 2013
            If fs.write_document_to_bytestream(filename, sdoc.byteDoc, Nothing, True) = False Then
                'Dim omessage As New bc_cs_message("Blue Curve", "File: " + filename + " can't be accessed", bc_cs_message.MESSAGE)
                Exit Sub
            End If
            _doc.support_documents.Add(sdoc)

            load_support_docs()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bsnew", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

    Private Sub uxDeleteSupportDoc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDeleteSupportDoc.CheckedChanged
        Dim did As String
        did = Me.uxsupportdocs.Selection.Item(0).Tag



        Dim idx As Integer = -1
        For i = 0 To _doc.support_documents.Count - 1
            If (IsNumeric(did) = True AndAlso _doc.support_documents(i).id = did) Or (_doc.support_documents(i).title = did And _doc.support_documents(i).marked_for_delete = False) Then
                If _doc.support_documents(i).support_doc_display_only = False Then
                    _doc.support_documents.RemoveAt(i)
                Else
                    If _doc.support_documents(i).checked_out_user <> 0 Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Document is checked out cannot delete", bc_cs_message.MESSAGE, "false", False, "Yes", "No", True)
                    Else
                        _doc.support_documents(i).marked_for_delete = True
                    End If
                End If
                Exit For
            End If
        Next
        uxDeleteSupportDoc.Enabled = False
        uxcatsupportdoc.Enabled = False

        Me.load_support_docs()
    End Sub

    Private Sub uxsupportdocs_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxsupportdocs.Click
        Me.uxDeleteSupportDoc.Enabled = False
        Me.uxcatsupportdoc.Enabled = False
        If uxsupportdocs.Selection.Count = 1 Then
            Me.uxDeleteSupportDoc.Enabled = True
            Me.uxcatsupportdoc.Enabled = True
        End If
    End Sub


    Private Sub uxcatsupportdoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxcatsupportdoc.Click
        Try
            Me.Cursor = Cursors.WaitCursor


            Dim ocat As New bc_dx_as_categorize
            ocat.attachment = True
            ocat.support_doc_cat = True
            Dim idx As String
            idx = Me.uxsupportdocs.Selection.Item(0).Tag
            ocat.Text = "Blue Curve - Support Document Categorisation"
            For i = 0 To _doc.support_documents.Count - 1
                If (IsNumeric(idx) = True AndAlso _doc.support_documents(i).id = idx) Or (_doc.support_documents(i).title = idx And _doc.support_documents(i).marked_for_delete = False) Then
                    ocat.document = _doc.support_documents(i)

                    'ocat.doc_date = ocat.doc_date.ToLocalTime
                    ocat.show_stage_change = False
                    ocat.show_local_submit = False

                    ocat.enable_pub_types = True
                    ocat.enable_lead_entity = False
                    ocat.caption = "Blue Curve Process - Update Support Document Metadata: " + _doc.support_documents(i).title

                    ocat.ok_button_caption = "Update"

                    Dim pts As New List(Of String)
                    For j = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                        pts.Add(bc_am_load_objects.obc_pub_types.pubtype(j).name)
                    Next
                    ocat.set_pub_types = pts
                    ocat.Focus()
                    Me.Cursor = Cursors.Default

                    ocat.ShowDialog()

                    If ocat.ok_selected = False Then
                        Exit Sub
                    End If
                    _doc.support_documents(i).metadata_changed = True

                    For j = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                        If ocat.uxputypes.Text = bc_am_load_objects.obc_pub_types.pubtype(j).name Then
                            _doc.support_documents(i).pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(j).id()
                            _doc.support_documents(i).pub_type_name = bc_am_load_objects.obc_pub_types.pubtype(j).name
                            Exit For
                        End If
                    Next
                    Me.load_support_docs()

                    For j = 0 To Me.uxsupportdocs.Nodes.Count - 1
                        If Me.uxsupportdocs.Nodes(j).Tag = idx Then
                            Me.uxsupportdocs.Nodes(j).Selected = True
                            Exit For
                        End If
                    Next
                    Exit For
                End If
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("uxcatsupportdoc", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            Me.Cursor = Cursors.Default
        End Try
    End Sub



    Private Sub uxcatsupportdoc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxcatsupportdoc.CheckedChanged

    End Sub

    Private Sub rcentral_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rcentral.SelectedIndexChanged
        If rcentral.SelectedIndex = 1 Then
            Me.uxstage.Enabled = False
        Else
            Me.uxstage.Enabled = True
        End If

    End Sub
    Dim sta As bc_om_next_stage_users
    Private Sub uxstage_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxstage.SelectedIndexChanged
        Me.pinstageactions.Visible = False
        Me.tcomment.PageEnabled = False
        _doc.stage_change_comment = ""
        _doc.stage_expire_date = "09/09/9999"
        Me.Chkcompletion.Checked = False
        Me.uxComment.Text = ""

        If _doc.stage_name = uxstage.Text And uxstage.Visible = True Then
            load_instate_actions()
        Else
            If Me.show_comments = True Then
                Me.tcomment.PageEnabled = True
                Me.tsuppdocs.SelectedTabPage = Me.tcomment
                Me.DateEdit1.DateTime = Now
                Me.TimeEdit1.Time = Now
                sta = New bc_om_next_stage_users
                sta.doc_id = _doc.id
                sta.pub_type_id = _doc.pub_type_id
                sta.next_stage = _doc.workflow_stages.stages(uxstage.SelectedIndex).stage_id

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    sta.db_read()
                Else
                    sta.tmode = bc_cs_soap_base_class.tREAD
                    If sta.transmit_to_server_and_receive(sta, True) = False Then
                        Exit Sub
                    End If
                End If
                If sta.users.Count > 0 Then
                    lnextuser.Visible = True
                    uxnextuser.Visible = True
                    For j = 0 To sta.users.Count - 1
                        uxnextuser.Properties.Items.Add(sta.users(j).user_name)
                    Next
                End If

            End If
        End If
    End Sub
    Sub load_instate_actions()
        Try
            Me.chkinstateactions.Items.Clear()
            Me.pinstageactions.Visible = False
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = _doc.pub_type_id Then
                    If bc_am_load_objects.obc_pub_types.pubtype(i).in_stage_actions.count > 0 Then
                        For j = 0 To bc_am_load_objects.obc_pub_types.pubtype(i).in_stage_actions.count - 1
                            If bc_am_load_objects.obc_pub_types.pubtype(i).in_stage_actions(j).stage_Id = _doc.stage Then
                                Me.pinstageactions.Visible = True
                                For k = 0 To bc_am_load_objects.obc_pub_types.pubtype(i).in_stage_actions(j).actions.count - 1
                                    If bc_am_load_objects.obc_pub_types.pubtype(i).in_stage_actions(j).actions(k).mandatory = True Then
                                        Me.chkinstateactions.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).in_stage_actions(j).actions(k).display_name, True)
                                    Else
                                        Me.chkinstateactions.Items.Add(bc_am_load_objects.obc_pub_types.pubtype(i).in_stage_actions(j).actions(k).display_name, False)
                                    End If
                                Next
                            End If
                        Next
                    End If
                    Exit For
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_at_submit_frm", "load_instate_actions", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles blinks.Click
        Try
            Me.Cursor = Cursors.WaitCursor


            Dim fld As New bc_am_dx_linked_documents
            Dim cld As New Cbc_am_linked_documents(fld, _doc, Me.linked_docs_count, False)




            If cld.load_data = True Then
                fld.TopMost = True
                fld.ShowDialog()
                If cld.bsave = True Then
                    Me.pdf_count = 0
                    _doc.linked_docs.Clear()
                    For i = 0 To cld.linked_docs.Count - 1
                        _doc.linked_docs.Add(cld.linked_docs(i))
                    Next
                    load_linked_documents()
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_at_submit_frm", "load_instate_actions", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Sub load_linked_documents()

        Me.lsdocs.Text = ""
        pdf_count = 0
        Dim tx As String = ""
        For i = 0 To _doc.linked_docs.Count - 1
            If (_doc.linked_docs(i).extension = ".pdf") Then
                pdf_count = pdf_count + 1
            End If

            tx = tx + "Date: " + Format(_doc.linked_docs(i).doc_date, "dd-MMM-yyyy") + "  Type: " + _doc.linked_docs(i).pub_type_name + vbCrLf
            tx = tx + "  Title: " + _doc.linked_docs(i).title + vbCrLf
            If _doc.linked_docs(i).sub_title <> "" Then
                tx = tx + "Sub Title: " + _doc.linked_docs(i).sub_title + vbCrLf + vbCrLf
            Else
                tx = tx + vbCrLf
            End If
        Next
        Me.lsdocs.Text = tx

    End Sub
    Sub load_analytic_documents()
        Me.asdocs.Text = ""
        'pdf_count = 0
        Dim tx As String = ""
        For i = 0 To _doc.analytic_docs.Count - 1
            'If (_doc.analytic_docs(i).extension = ".pdf") Then
            '    pdf_count = pdf_count + 1
            'End If

            tx = tx + "Date: " + Format(_doc.analytic_docs(i).doc_date, "dd-MMM-yyyy") + "  Type: " + _doc.analytic_docs(i).pub_type_name + vbCrLf
            tx = tx + "  Title: " + _doc.analytic_docs(i).title + vbCrLf
            If _doc.analytic_docs(i).sub_title <> "" Then
                tx = tx + "Sub Title: " + _doc.analytic_docs(i).sub_title + vbCrLf + vbCrLf
            Else
                tx = tx + vbCrLf
            End If
        Next
        Me.asdocs.Text = tx

    End Sub

    Private Sub rallscan_SelectedIndexChanged(sender As Object, e As EventArgs) Handles rallscan.SelectedIndexChanged
        Try
            Me.cet.Visible = False
            Me.Cursor = Cursors.WaitCursor

            If Me.rallscan.SelectedIndex = 2 Then
                Me.cet.Visible = True
                If scanned = True Then
                    Exit Sub
                End If

                ao_object.scan_document_for_entities(_taxonomies, Me.cet.Checked)
            End If
            Me.display_data_for_taxonomy_page(uxtaxonomy.SelectedTabPageIndex, True, True)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("rallscan", "selectedindexchanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.display_data_for_taxonomy_page(uxtaxonomy.SelectedTabPageIndex, True, False)
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub uxtaxonomy_Click(sender As Object, e As EventArgs) Handles uxtaxonomy.Click

    End Sub

    Private Sub uxtaxonomy_SizeChanged(sender As Object, e As EventArgs) Handles uxtaxonomy.SizeChanged

    End Sub

    Private Sub cet_CheckedChanged(sender As Object, e As EventArgs) Handles cet.CheckedChanged

        Me.Cursor = Cursors.WaitCursor
        ao_object.scan_document_for_entities(_taxonomies, Me.cet.Checked)
        scanned = True
        Me.display_data_for_taxonomy_page(uxtaxonomy.SelectedTabPageIndex, True, False)
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub Chkcompletion_CheckedChanged(sender As Object, e As EventArgs) Handles Chkcompletion.CheckedChanged
        Me.DateEdit1.Enabled = False
        Me.TimeEdit1.Enabled = False
        If Me.Chkcompletion.Checked = True Then
            Me.DateEdit1.Enabled = True
            Me.TimeEdit1.Enabled = True
        End If
    End Sub

    Private Sub uxsummary_EditValueChanged(sender As Object, e As EventArgs) Handles uxsummary.DoubleClick
        Try

            If bc_am_load_objects.obc_pub_types.process_switches.html_summary = False Or disable_title_st_summary = True Then
                Exit Sub
            End If

            Dim f As New bc_dx_html_summary
            Dim c As New Cbc_dx_html_summary(f)
            Me.Cursor = Cursors.WaitCursor


            If c.load_data(_doc.summary, _doc.summary_rtf, _doc.summary_html, html_editor_length, html_editor_default_font, html_editor_default_font_size, html_editor_only_bold_italic_underline) = True Then
                Me.Cursor = Cursors.Default
                f.ShowDialog()
                If c.bsave = True Then
                    _doc.summary = c.text
                    Me.uxsummary.Text = _doc.summary
                    _doc.summary_rtf = c.rtf
                    _doc.summary_html = c.html
                End If
            End If
        Catch ex As Exception
            Dim err As New bc_cs_error_log("uxsummary", "DoubleClick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub


    Private Sub uxsummary_EditValueChanged_1(sender As Object, e As EventArgs) Handles uxsummary.EditValueChanged

    End Sub

    Private Sub uxtime_EditValueChanged(sender As Object, e As EventArgs)

    End Sub

    Private Sub lsd_Click(sender As Object, e As EventArgs)

    End Sub

    Private Sub lsearch_EditValueChanged(sender As Object, e As EventArgs) Handles lsearch.EditValueChanged

    End Sub

    Private Sub pclear_EditValueChanged(sender As Object, e As EventArgs) Handles pclear.Click
        Me.Cursor = Cursors.WaitCursor
        Me.tsearch.Text = ""
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub uxtaxonomy_RightToLeftChanged(sender As Object, e As EventArgs) Handles uxtaxonomy.RightToLeftChanged

    End Sub
    Dim grr As New bc_om_regular_reports
    Private Sub load_regular_reports()
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
            Exit Sub
        End If
        Dim rr As New bc_om_regular_reports
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            rr.db_read()
        Else
            rr.tmode = bc_cs_soap_base_class.tREAD
            If rr.transmit_to_server_and_receive(rr, True) = False Then
                Exit Sub
            End If
        End If
        crr.Properties.Items.Add(Me.uxputypes.Text)
        For i = 0 To rr.regular_reports.Count - 1
            If rr.regular_reports(i).doc.pub_type_id = _doc.pub_type_id Then
                grr.regular_reports.Add(rr.regular_reports(i))
                crr.Properties.Items.Add(rr.regular_reports(i).name)

            End If
        Next
        If grr.regular_reports.Count > 0 Then
            crr.Visible = True
            Me.LabelControl4.Text = "Select Regular Report"
            crr.SelectedIndex = 0
        End If

    End Sub

    Private Sub crr_SelectedIndexChanged(sender As Object, e As EventArgs) Handles crr.SelectedIndexChanged
        Try
            If Me.crr.SelectedIndex = 0 Then
                Exit Sub
            End If
            Dim omsg As New bc_cs_message("Blue Curve", "Choosing a regular report will remove some existing classification values proceed?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
            If omsg.cancel_selected = True Then
                Me.crr.SelectedIndex = 0
                Exit Sub
            End If

            Me.uxsel.Items.Clear()
            Me.uxtitle.Text = grr.regular_reports(Me.crr.SelectedIndex - 1).doc.title
            Me.uxsubtitle.Text = grr.regular_reports(Me.crr.SelectedIndex - 1).doc.sub_title
            Me.uxsummary.Text = grr.regular_reports(Me.crr.SelectedIndex - 1).doc.summary
            Me.tcustom.Text = grr.regular_reports(Me.crr.SelectedIndex - 1).doc.custom_title


            Dim ord As Integer
            For i = 0 To _taxonomies.Count - 1
                ord = 1
                For j = 0 To _taxonomies(i).items.Count - 1
                    _taxonomies(i).items(j).selected = False
                    _taxonomies(i).items(j).order = 0
                    
                    If _taxonomies(i).type = 0 Then
                        For k = 0 To grr.regular_reports(Me.crr.SelectedIndex - 1).doc.authors.Count - 1
                            If _taxonomies(i).items(j).id = grr.regular_reports(Me.crr.SelectedIndex - 1).doc.authors(k).id Then
                                _taxonomies(i).items(j).selected = True
                                _taxonomies(i).items(j).order = ord
                                Exit For
                            End If
                        Next
                    ElseIf _taxonomies(i).type = 2 Then
                        For k = 0 To grr.regular_reports(Me.crr.SelectedIndex - 1).doc.disclosures.Count - 1
                            If _taxonomies(i).items(j).id = grr.regular_reports(Me.crr.SelectedIndex - 1).doc.disclosures(k).entity_id Then
                                _taxonomies(i).items(j).selected = True
                                _taxonomies(i).items(j).order = ord
                                Exit For
                            End If
                        Next
                    Else
                        For k = 0 To grr.regular_reports(Me.crr.SelectedIndex - 1).doc.taxonomy.Count - 1
                            If _taxonomies(i).items(j).id = grr.regular_reports(Me.crr.SelectedIndex - 1).doc.taxonomy(k).entity_id Then
                                _taxonomies(i).items(j).selected = True
                                _taxonomies(i).items(j).order = ord
                                ord = ord + 1
                                Exit For
                            End If
                        Next
                    End If


                Next
            Next
            If bc_cs_central_settings.selected_conn_method <> bc_cs_central_settings.LOCAL Then
                For i = 0 To _taxonomies.Count - 1
                    If _taxonomies(i).dependent_class.Count = 1 Then
                        ord = 1
                        REM get dependent list from server
                        reset_page_for_dependent_taxonomy(_taxonomies(i).dependent_class(0), _taxonomies(i).Class_id, i)
                        For j = 0 To _taxonomies(i).items.Count - 1
                            For k = 0 To grr.regular_reports(Me.crr.SelectedIndex - 1).doc.taxonomy.Count - 1
                                If _taxonomies(i).items(j).id = grr.regular_reports(Me.crr.SelectedIndex - 1).doc.taxonomy(k).entity_id Then
                                    _taxonomies(i).items(j).selected = True
                                    _taxonomies(i).items(j).order = ord
                                    ord = ord + 1
                                    Exit For
                                End If
                            Next
                        Next
                    End If
                Next
            End If

            Me.uxtaxonomy.SelectedTabPageIndex = 0
            Me.display_data_for_taxonomy_page(0, False, False)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Blue Curve", "crr_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub

    Private Sub uxtitle_EditValueChanged(sender As Object, e As EventArgs) Handles uxtitle.EditValueChanged

    End Sub

    Private Sub lmdoc_Click(sender As Object, e As EventArgs) Handles lmdoc.Click

    End Sub

    Private Sub balinks_Click(sender As Object, e As EventArgs) Handles balinks.Click
        Try
            Me.Cursor = Cursors.WaitCursor


            Dim fld As New bc_am_dx_linked_documents
            Dim cld As New Cbc_am_linked_documents(fld, _doc, Me.num_analytic_docs, True)




            If cld.load_data = True Then
                fld.TopMost = True
                fld.ShowDialog()
                If cld.bsave = True Then
                    'Me.pdf_count = 0
                    _doc.analytic_docs.Clear()
                    For i = 0 To cld.linked_docs.Count - 1
                        _doc.analytic_docs.Add(cld.linked_docs(i))
                    Next
                    load_analytic_documents()
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_at_submit_frm", "balinks_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
End Class
Class cloti
    Public cloti As New List(Of bc_om_entity)
End Class