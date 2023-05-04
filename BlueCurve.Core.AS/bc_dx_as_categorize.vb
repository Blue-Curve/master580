Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Create.AM
Imports System.Windows.Forms

Public Class bc_dx_as_categorize
    Private authors_order As New List(Of author_order)
    Private Class author_order
        Public id As Long
        Public name As String

    End Class
    Private loading As Boolean = True
    Private _taxonomies As New List(Of bc_as_cat_taxonomy)
    Private entity_list As New bc_om_entities

    Public Class bc_as_cat_taxonomy
        Public Sub New(ByVal name As String, ByVal type As Integer, ByVal class_id As Long)
            Me.name = name
            Me.type = type
            Me.class_id = class_id
        End Sub
        Public name As String
        Public type As Integer REM 0 author 1 class 2 disclosures
        Public Class_id As Long
        Public items As New List(Of bc_as_cat_taxononmy_item)
        'Public selected_items As New List(Of bc_as_cat_taxononmy_sel_item)
        Public Class bc_as_cat_taxononmy_item
            Public display_name As String
            Public id As Long
            Public selected As Boolean
            Public searched As Boolean
            Public search_names As New List(Of String)
            Public search_name As String
            Public Sub New(ByVal display_name As String, ByVal id As Long, ByVal selected As Boolean)
                Me.display_name = display_name
                Me.id = id
                Me.selected = selected
            End Sub
        End Class
        Public Class bc_as_cat_taxononmy_sel_item
            Public display_name As String
            Public id As Long
        End Class
    End Class
    Public _ok_selected As Boolean = False
    Public _doc As bc_om_document
    Private _show_local_submit As Boolean = False
    Private _show_attach_doc As Boolean = False
    Private _pub_types As List(Of String)
    Private _file As Boolean
    Private _enable_lead_entity As Boolean = False
    Private _show_ext As Boolean = False
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
                Me.uxtime.Enabled = False
                Me.uxputypes.Enabled = False
                Dim dp As System.Drawing.Point
                dp.X = Me.uxall.Location.X
                dp.Y = Me.uxsel.Location.Y
                Me.uxsel.Location = dp
                Me.uxsel.Width = Me.uxtaxonomy.Width
                Me.uxsel.Height = Me.uxsel.Height + Me.tsearch.Height

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
    'Public WriteOnly Property show_attach_doc()
    '    Set(ByVal value)
    '        _show_attach_doc = value
    '    End Set
    'End Property
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
        _doc.disclosures.Clear()

        For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
            If bc_am_load_objects.obc_pub_types.pubtype(i).name = Me.uxputypes.Text Then
                Me._doc.pub_type_id = bc_am_load_objects.obc_pub_types.pubtype(i).id
                If _doc.language_id = 0 Then
                    _doc.language_id = bc_am_load_objects.obc_pub_types.pubtype(i).language
                End If
                Exit For
            End If

        Next

        For i = 0 To authors_order.Count - 1
            user = New bc_om_user
            user.id = authors_order(i).id
            _doc.authors.Add(user)
        Next

        If _doc.authors.Count = 0 Then
            Dim omsg As New bc_cs_message("Blue Curve", "At least 1 author must be selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            Exit Sub
        End If


        Dim has_lead_entity As Boolean = False
        For i = 1 To _taxonomies.Count - 1
            For j = 0 To _taxonomies(i).items.Count - 1

                If _taxonomies(i).items(j).selected = True Then
                    Select Case _taxonomies(i).type
                        Case 0
                            user = New bc_om_user
                            user.id = _taxonomies(i).items(j).id
                            _doc.authors.Add(user)
                        Case 2
                            tax = New bc_om_taxonomy
                            tax.entity_id = _taxonomies(i).items(j).id
                            _doc.disclosures.Add(tax)
                        Case 1
                            has_lead_entity = True
                            tax = New bc_om_taxonomy
                            tax.entity_id = _taxonomies(i).items(j).id
                            _doc.taxonomy.Add(tax)
                            If _taxonomies(i).items(j).display_name = Me.uxlead.Text Then
                                _doc.entity_id = _taxonomies(i).items(j).id
                            End If
                        Case 3
                            tax = New bc_om_taxonomy
                            tax.entity_id = _taxonomies(i).items(j).id
                            _doc.taxonomy.Add(tax)

                    End Select
                ElseIf _taxonomies(i).type = 1 Then
                    If _taxonomies(i).items(j).display_name = Me.uxlead.Text Then
                        _doc.entity_id = _taxonomies(i).items(j).id
                    End If
                End If
            Next
        Next

        If has_lead_entity = False Then
            For i = 1 To _taxonomies.Count - 1
                If _taxonomies(i).type = 1 Then
                    Dim omsg As New bc_cs_message("Blue Curve", "At least 1 " + _taxonomies(i).name + " must be selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                    Exit For
                End If
            Next
        End If

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
            If fs.write_document_to_bytestream(filename, _doc.byteDoc, Nothing, False) = False Then
                Dim omessage As New bc_cs_message("Blue Curve", "File: " + filename + " cannot be accessed please make sure you dont have it open.", bc_cs_message.MESSAGE, False, False, "Yes", "Ok", True)
                Me._ok_selected = False
                Exit Sub
            End If

        End If
        Me._ok_selected = True

        Me.Hide()
    End Sub
    Public Sub load_data()
        Me.searchtimer.Stop()
        Me.lsearchtimer.Stop()

        Me.uxtitle.Text = Me._doc.title
        Me.uxsubtitle.Text = Me._doc.sub_title
        Me.uxsummary.Text = Me._doc.summary
        Me.disable_search = True

        If bc_cs_central_settings.alt_entity_for_submit = True Then
            entity_list = bc_am_load_objects.obc_entities.alternate_entity_list
        Else
            entity_list = bc_am_load_objects.obc_entities
        End If

        If Not IsNothing(_pub_types) Then
            If _pub_types.Count = 1 Then
                Me.uxputypes.Text = _pub_types(0)
                'Me.uxputypes.SelectedIndex = 0
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
        If CStr(_doc.doc_date) = "00:00:00" Then
            _doc.doc_date = Now
        End If
        Me.uxdate.DateTime = _doc.doc_date
        Me.uxtime.Time = _doc.doc_date

        load_taxonomy_data(_doc.pub_type_id)
        display_taxonomy()
        Me.display_data_for_taxonomy_page(0)

        Me.chkurgent.Checked = _doc.urgent_flag

        If Me._show_ext = True Then
            Me.pext.Visible = True

        End If

        Me.disable_search = False
    End Sub
    Private Sub load_taxonomy_data(ByVal pub_type_id As Long, Optional ByVal pub_type_change As Boolean = False)
        Dim lead_class_id As Long
        REM taxonomy first tab is always the author
        Try
            Me.disable_search = True


            If pub_type_change = True Then
                For i = 0 To _taxonomies.Count - 1
                    _taxonomies.RemoveAt(0)
                Next
                Me.Llead.Text = "Lead"
                Me.uxlead.Properties.Items.Clear()
                Me.uxlead.Text = ""
                Me.uxlead.Enabled = False
                Me.lsearch.Visible = False
                Me.llsearch.Visible = False
                Me.llsearch.Text = ""

            End If
            Dim otax As bc_as_cat_taxonomy

            otax = New bc_as_cat_taxonomy("Author", 0, 0)
            _taxonomies.Add(otax)
            REM see if document has lead entity
            Dim ou As author_order
            Me.authors_order.Clear()
            For j = 0 To _doc.authors.Count - 1
                ou = New author_order
                ou.id = _doc.authors(j).id
                'ou.name = _doc.authors(j).first_name + " " + _doc.authors(j).surname
                Me.authors_order.Add(ou)
            Next

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
                        otax = New bc_as_cat_taxonomy("Lead Entity", 1, lead_class_id)
                        _taxonomies.Add(otax)
                    End If
                End If
            End If
            If bc_am_load_objects.obc_entities.disclosure_entities.Count > 0 Then
                otax = New bc_as_cat_taxonomy("Disclosures", 2, 0)
                _taxonomies.Add(otax)
            End If

            If pub_type_id > 0 Then
                For j = 0 To pt.taxonomy.Count - 1
                    otax = New bc_as_cat_taxonomy(pt.taxonomy(j), 3, 0)
                    _taxonomies.Add(otax)
                Next
            End If
            Dim item As bc_as_cat_taxonomy.bc_as_cat_taxononmy_item = Nothing
            Dim found As Boolean = False
            REM load user taxonomy
            For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                found = False
                With bc_am_load_objects.obc_users.user(i)
                    For j = 0 To _doc.authors.Count - 1
                        If _doc.authors(j).id = .id Then
                            item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(.first_name + " " + .surname, .id, True)
                            found = True
                            Continue For
                        End If
                    Next
                    If found = False Then
                        item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(.first_name + " " + .surname, .id, False)
                    End If
                    _taxonomies(0).items.Add(item)
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
                            _taxonomies(j).items.Add(item)
                        End With
                    Next
                    Exit For
                End If
            Next


            Dim sel As Boolean
            Dim efound As Boolean = False
            For i = 0 To entity_list.entity.Count - 1
                efound = False
                With entity_list.entity(i)
                    For j = 1 To _taxonomies.Count - 1
                        sel = False
                        For k = 0 To _doc.taxonomy.Count - 1
                            If _doc.taxonomy(k).entity_Id = .id Then
                                sel = True
                                Exit For
                            End If
                        Next
                        Select Case _taxonomies(j).type
                            Case 1
                                If .class_id = lead_class_id Then
                                    _taxonomies(j).Class_id = .class_id
                                    _taxonomies(j).name = .class_name
                                    Me.Llead.Text = "Lead " + .class_name
                                    If _enable_lead_entity = True Then
                                        Me.uxlead.Enabled = True
                                        Me.lsearch.Visible = True
                                        Me.llsearch.Visible = True


                                    End If
                                    item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(.name, .id, sel)
                                    _taxonomies(j).items.Add(item)
                                    Me.uxlead.Properties.Items.Add(.name)

                                    If _doc.entity_id = .id Then
                                        Me.uxlead.SelectedIndex = _taxonomies(j).items.Count - 1
                                    End If
                                End If

                            Case 3
                                If .class_name = _taxonomies(j).name Then
                                    _taxonomies(j).Class_id = .class_id
                                    item = New bc_as_cat_taxonomy.bc_as_cat_taxononmy_item(.name, .id, sel)
                                    _taxonomies(j).items.Add(item)
                                End If
                        End Select
                    Next
                End With
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_as_categorize", "load_taxonomy_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.disable_search = False
        End Try
    End Sub
    Private Sub display_taxonomy()
        REM taxonomy
        REM always have one page
        For i = 1 To uxtaxonomy.TabPages.Count - 1
            uxtaxonomy.TabPages.RemoveAt(1)
        Next
        If _taxonomies.Count > 0 Then
            uxtaxonomy.TabPages(0).Text = _taxonomies(0).name
            For i = 1 To _taxonomies.Count - 1
                uxtaxonomy.TabPages.Add(_taxonomies(i).name)
            Next
        End If

    End Sub
    Private Sub display_data_for_taxonomy_page(ByVal page As Integer)
        Try
            Me.Cursor = Windows.Forms.Cursors.WaitCursor
            Me.uxsel.Items.Clear()
            Me.uxall.Items.Clear()
            Dim bsearch As Boolean = False
            If Me.tsearch.Text <> "" Then
                bsearch = True
            End If

            For i = 0 To _taxonomies(page).items.Count - 1
                If _taxonomies(page).items(i).selected = True Then
                    REM authors need to be order so deal with differently
                    If _taxonomies(page).name <> "Author" Then
                        Me.uxsel.Items.Add(_taxonomies(page).items(i).display_name)
                    Else
                        For j = 0 To Me.authors_order.Count - 1
                            If Me.authors_order(j).id = _taxonomies(page).items(i).id Then
                                Me.authors_order(j).name = _taxonomies(page).items(i).display_name
                                Exit For
                            End If
                        Next
                    End If
                ElseIf _taxonomies(page).items(i).selected = False And (bsearch = False Or _taxonomies(page).items(i).searched = True) Then
                    Me.uxall.Items.Add(_taxonomies(page).items(i).display_name)
                End If
            Next
            If _taxonomies(page).name = "Author" Then
                For i = 0 To authors_order.Count - 1
                    Me.uxsel.Items.Add(authors_order(i).name)
                Next
                Me.check_up_down_buttons()

            End If
        Catch

        Finally
            Me.Cursor = Windows.Forms.Cursors.Default
        End Try

    End Sub


    Private disable_search As Boolean = False
    Private Sub uxtaxonomy_SelectedPageChanged(ByVal sender As Object, ByVal e As DevExpress.XtraTab.TabPageChangedEventArgs) Handles uxtaxonomy.SelectedPageChanged
        disable_search = True

        If Me.tsearch.Text = "" Then
            Me.display_data_for_taxonomy_page(uxtaxonomy.SelectedTabPageIndex)
        Else
            Me.tsearch.Text = ""
        End If

        disable_search = False
        If uxtaxonomy.SelectedTabPageIndex = 0 Then
            check_up_down_buttons()
        End If
    End Sub



    Private Sub bc_as_categorize_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        loading = True
        load_data()
        loading = False
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
                If _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).name <> "Author" Then
                    For i = 0 To Me.entity_list.search_attributes.search_values.Count - 1
                        If Me.entity_list.search_attributes.search_values(i).class_id = _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).Class_id AndAlso InStr(UCase(Me.entity_list.search_attributes.search_values(i).value), UCase(tx)) > 0 Then
                            found_entities.Add(Me.entity_list.search_attributes.search_values(i).entity_id)
                        End If
                    Next
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




            display_data_for_taxonomy_page(Me.uxtaxonomy.SelectedTabPageIndex)




        Catch ex As Exception

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
                For i = 0 To Me.entity_list.search_attributes.search_values.Count - 1
                    If Me.entity_list.search_attributes.search_values(i).class_id = _taxonomies(1).Class_id AndAlso InStr(UCase(Me.entity_list.search_attributes.search_values(i).value), UCase(tx)) > 0 Then
                        found_entities.Add(Me.entity_list.search_attributes.search_values(i).entity_id)
                    End If
                Next
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
        del_item()
    End Sub

    Private Sub sel_item()

        For i = 0 To _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items.Count - 1
            If _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).display_name = Me.uxall.SelectedItems(0) Then
                _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).selected = True
                If _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).name = "Author" Then
                    Dim ou As New author_order
                    ou.id = _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).id
                    authors_order.Add(ou)

                End If
                display_data_for_taxonomy_page(Me.uxtaxonomy.SelectedTabPageIndex)
                Exit Sub
            End If
        Next
        display_data_for_taxonomy_page(Me.uxtaxonomy.SelectedTabPageIndex)
    End Sub


    Private Sub del_item()
        For i = 0 To _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items.Count - 1
            If _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).display_name = Me.uxsel.SelectedItems(0) Then
                _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).items(i).selected = False
                If _taxonomies(Me.uxtaxonomy.SelectedTabPageIndex).name = "Author" Then
                    authors_order.RemoveAt(Me.uxsel.SelectedIndex)
                End If
                display_data_for_taxonomy_page(Me.uxtaxonomy.SelectedTabPageIndex)
                Exit Sub
            End If
        Next

    End Sub

    Private Sub btnattach_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Dim odialog As New OpenFileDialog
        Dim extensionsize As Integer
        odialog.Title = "Import Document"
        odialog.ShowDialog()
        Me.uxattach.Text = odialog.FileName

        If Len(Me.uxtitle.Text) = 0 Then
            extensionsize = (Len(odialog.SafeFileName) - (InStrRev(odialog.SafeFileName, ".") - 1))
            Me.uxtitle.Text = Microsoft.VisualBasic.Left(odialog.SafeFileName, Len(odialog.SafeFileName) - extensionsize)
        End If
    End Sub

    Private Sub uxputypes_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxputypes.SelectedIndexChanged
        If loading = True Then
            Exit Sub
        End If
        Try
            Me.Cursor = Cursors.WaitCursor


            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).name = Me.uxputypes.Text Then
                    load_taxonomy_data(bc_am_load_objects.obc_pub_types.pubtype(i).id, True)
                    display_taxonomy()
                    Me.display_data_for_taxonomy_page(0)
                    Exit For
                End If
            Next
        Catch ex As Exception

        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub
    Private Sub uxsel_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxsel.SelectedIndexChanged
        check_up_down_buttons()
    End Sub
    Private Sub check_up_down_buttons()
        Me.bdn.Enabled = False
        If Me.uxtaxonomy.SelectedTabPageIndex = 0 And Me.uxsel.SelectedIndex < Me.uxsel.Items.Count - 1 Then
            Me.bdn.Enabled = True
        End If
        Me.bup.Enabled = False
        If Me.uxtaxonomy.SelectedTabPageIndex = 0 And Me.uxsel.SelectedIndex > 0 Then
            Me.bup.Enabled = True
        End If
    End Sub


    Private Sub bup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bup.Click
        Dim tu As author_order
        tu = authors_order(Me.uxsel.SelectedIndex)

        authors_order.RemoveAt(Me.uxsel.SelectedIndex)
        authors_order.Insert(Me.uxsel.SelectedIndex - 1, tu)
        Me.display_data_for_taxonomy_page(0)


    End Sub
    Private Sub bdn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdn.Click
        Dim tu As author_order
        tu = authors_order(Me.uxsel.SelectedIndex)

        authors_order.RemoveAt(Me.uxsel.SelectedIndex)
        authors_order.Insert(Me.uxsel.SelectedIndex + 1, tu)
        Me.display_data_for_taxonomy_page(0)
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
        odialog.ShowDialog()
        Me.uxattach.Text = odialog.FileName
        If Len(Me.uxattach.Text) > 2 Then
            If Len(Me.uxtitle.Text) = 0 Then
                extensionsize = (Len(odialog.SafeFileName) - (InStrRev(odialog.SafeFileName, ".") - 1))
                Me.uxtitle.Text = Microsoft.VisualBasic.Left(odialog.SafeFileName, Len(odialog.SafeFileName) - extensionsize)
            End If
        End If
    End Sub

    Private Sub uxlead_SelectedIndexChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxlead.SelectedIndexChanged
        If Me.uxlead.Enabled = True Then
            For i = 0 To _taxonomies(1).items.Count - 1
                If _taxonomies(1).items(i).display_name = Me.uxlead.Text Then
                    _taxonomies(1).items(i).selected = True
                    Me.uxtaxonomy.SelectedTabPageIndex = 1
                    Me.display_data_for_taxonomy_page(1)

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
        Dim vfgc As New bc_am_extended_class
        Dim cfgc As New cttrl_bc_am_extended_class(vfgc, ldoc)
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
End Class