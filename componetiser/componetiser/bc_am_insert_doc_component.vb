Imports System.IO
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS

Public Class bc_am_insert_doc_component

    Public oworddocument As Object
    Public componetize_styles As New bc_om_componetize_styles_for_template
    Public lcomponents As New bc_om_document_components
    Public view_ids As New ArrayList
    Public rtf_flag As Boolean = True
    Public headers As Boolean = True
    Public search_mode As Boolean = False
    Public docs As New bc_om_component_documents
    Public loading As Boolean = False
    REM current document
    Public ldoc As New bc_om_document

    'Public Class bc_am_componetize_Styles
    '    Public styles As New ArrayList
    'End Class

    Private Sub Form1_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oword As Object
        oword = GetObject(, "word.application")
        Me.oworddocument = oword.activedocument
        set_up_styles()
        Dim bcs As New bc_cs_central_settings(True)
        set_up_lists()
    End Sub
    Public Sub set_up_styles()
        componetize_styles.styles.Clear()
        Dim bcs As New bc_cs_central_settings(True)
        componetize_styles.template_id = 1

        componetize_styles.db_read()

        Dim ostyle As New bc_om_componetize_style
        Exit Sub
        ''ostyle.header_style = "ABG_FP_First_Heading"
        ''ostyle.body_style = "Headline"
        ''componetize_styles.styles.Add(ostyle)
        ''ostyle = New bc_am_componetize_style
        ''ostyle.header_style = "ABG_FP_Second_Heading"
        ''ostyle.body_style = "Pg 1 Bullet List"
        ''componetize_styles.styles.Add(ostyle)
        ''ostyle = New bc_am_componetize_style
        ''ostyle.header_style = "ABG_FP_Graph_Title"
        ''ostyle.body_style = "Pg 1 Bullet List"
        ''componetize_styles.styles.Add(ostyle)
        ''ostyle = New bc_am_componetize_style
        ''ostyle.header_style = "ColumnHeading1"
        ''ostyle.body_style = "ColumnText"
        ''componetize_styles.styles.Add(ostyle)
        ''ostyle = New bc_am_componetize_style
        ''ostyle.header_style = "ColumnHeading2"
        ''ostyle.body_style = "ColumnText"
        ''componetize_styles.styles.Add(ostyle)
        'ostyle = New bc_om_componetize_style
        'ostyle.header_style = "Bullets level 1"
        ''ostyle.body_style = "Bullets level 2"
        'componetize_styles.styles.Add(ostyle)
        'ostyle.header_style = "Header"
        'ostyle.body_style.Add("Table Grid")
        'componetize_styles.styles.Add(ostyle)
        'ostyle = New bc_om_componetize_style

        'ostyle.header_style = "Footer"
        'ostyle.cell_only = True
        'componetize_styles.styles.Add(ostyle)
        'ostyle = New bc_om_componetize_style
        'ostyle.header_style = "Price"
        'ostyle.cell_only = True
        'componetize_styles.styles.Add(ostyle)
        'ostyle = New bc_om_componetize_style
        'ostyle.header_style = "Table Heading"
        'ostyle.body_style.Add("Table Grid")
        'ostyle.body_style.Add("Table Source")
        'componetize_styles.styles.Add(ostyle)

        'ostyle = New bc_om_componetize_style
        'ostyle.header_style = "Heading 1"
        'ostyle.body_style.Add("Normal")
        'ostyle.body_style.Add("Table Grid")

        'componetize_styles.styles.Add(ostyle)
        'ostyle = New bc_om_componetize_style
        'ostyle.header_style = "Heading 2"
        'ostyle.body_style.Add("Normal")
        'componetize_styles.styles.Add(ostyle)
        'ostyle = New bc_om_componetize_style
        'ostyle.header_style = "Section"
        'ostyle.cell_only = True
        'ostyle.body_style.Add("Table Grid")
        'componetize_styles.styles.Add(ostyle)



        'ostyle = New bc_om_componetize_style
        'ostyle.header_style = "SB_Tbl_Header_Title_Left"
        'componetize_styles.styles.Add(ostyle)

        'ostyle = New bc_om_componetize_style
        'ostyle.header_style = "SB_FP_Doc_Date"
        'componetize_styles.styles.Add(ostyle)
    End Sub
    Private Sub set_header_view()
        If oworddocument.ActiveWindow.View.SplitSpecial = 0 Then
            oworddocument.ActiveWindow.ActivePane.View.Type = 3
        Else
            oworddocument.ActiveWindow.View.Type = 3
        End If
        If oworddocument.ActiveWindow.View.SplitSpecial <> 0 Then
            oworddocument.ActiveWindow.Panes(2).Close()
        End If
        If oworddocument.ActiveWindow.ActivePane.View.Type = 1 Or oworddocument.ActiveWindow. _
            ActivePane.View.Type = 2 Then
            oworddocument.ActiveWindow.ActivePane.View.Type = 3
        End If
        oworddocument.ActiveWindow.ActivePane.View.SeekView = 9
    End Sub

    Public Sub componetize_headers_footers(ByRef ocomponents As bc_om_document_components)
        REM for each header footre
        Dim ocomponent As bc_om_style_component
        Dim body_style As New ArrayList
        Dim odd_even As Boolean
        Dim first_page As Boolean
        Dim tx As String

        REM for all sections
        For i = 1 To oworddocument.Sections.Count
            odd_even = False
            first_page = False

            REM for each header footer
            REM if first page is different
            If oworddocument.Sections(i).PageSetup.DifferentFirstPageHeaderFooter = True Then
                first_page = True
                If (i > 1 And oworddocument.Sections(i).Headers(2).LinkToPrevious = False) Or i = 1 Then
                    For j = 1 To oworddocument.Sections(i).Headers(2).Range.Paragraphs.Count
                        oworddocument.Sections(i).Headers(2).Range.Paragraphs(j).Range.select()
                        parse_paragraph(ocomponents, ocomponent, body_style, True, "Section " + CStr(i) + "-First Page Header")
                    Next
                End If
            End If
            REM first page footer
            If oworddocument.Sections(i).PageSetup.DifferentFirstPageHeaderFooter = True Then
                odd_even = True
                If (i > 1 And oworddocument.Sections(i).Footers(2).LinkToPrevious = False) Or i = 1 Then
                    For j = 1 To oworddocument.Sections(i).Footers(2).Range.Paragraphs.Count
                        oworddocument.Sections(i).Footers(2).Range.Paragraphs(j).Range.select()
                        parse_paragraph(ocomponents, ocomponent, body_style, True, "Section " + CStr(i) + "-First Page Footer")
                    Next
                End If
            End If

            REM even header
            If oworddocument.Sections(i).PageSetup.OddAndEvenPagesHeaderFooter = True Then
                If (i > 1 And oworddocument.Sections(i).Headers(3).LinkToPrevious = False) Or i = 1 Then
                    For j = 1 To oworddocument.Sections(i).Headers(3).Range.Paragraphs.Count
                        oworddocument.Sections(i).Headers(3).Range.Paragraphs(j).Range.select()
                        parse_paragraph(ocomponents, ocomponent, body_style, True, "Section " + CStr(i) + "-Even Page Header")

                    Next
                End If
            End If


            REM even footer
            If oworddocument.Sections(i).PageSetup.OddAndEvenPagesHeaderFooter = True Then
                If (i > 1 And oworddocument.Sections(i).Footers(3).LinkToPrevious = False) Or i = 1 Then
                    For j = 1 To oworddocument.Sections(i).Footers(3).Range.Paragraphs.Count
                        oworddocument.Sections(i).Footers(3).Range.Paragraphs(j).Range.select()
                        parse_paragraph(ocomponents, ocomponent, body_style, True, "Section " + CStr(i) + "-Even Page Footer")
                    Next
                End If
            End If
            REM primary
            If (i > 1 And oworddocument.Sections(i).Headers(1).LinkToPrevious = False) Or i = 1 Then
                For j = 1 To oworddocument.Sections(i).Headers(1).Range.Paragraphs.Count
                    oworddocument.Sections(i).Headers(1).Range.Paragraphs(j).Range.select()
                    tx = "Primary Header"
                    If odd_even = True Then
                        tx = "Odd Page Header"
                    End If
                    parse_paragraph(ocomponents, ocomponent, body_style, True, "Section " + CStr(i) + "-" + tx)

                Next

            End If
            REM primary footer
            If (i > 1 And oworddocument.Sections(i).Footers(1).LinkToPrevious = False) Or i = 1 Then
                For j = 1 To oworddocument.Sections(i).Footers(1).Range.Paragraphs.Count
                    oworddocument.Sections(i).Footers(1).Range.Paragraphs(j).Range.select()
                    tx = "Primary Footer"
                    If odd_even = True Then
                        tx = "Odd Page Footer"
                    End If
                    parse_paragraph(ocomponents, ocomponent, body_style, True, "Section " + CStr(i) + "-" + tx)
                Next
            End If
        Next
        REM txt boxed in header/footer
        Try
            set_header_view()
            For j = 1 To oworddocument.Sections(1).Headers(1).shapes.count
                oworddocument.Sections(1).Headers(1).shapes(j).select()
                If oworddocument.application.Selection.StoryType = 5 Then
                    For k = 1 To oworddocument.application.Selection.paragraphs.count
                        oworddocument.application.Selection.paragraphs(k).range.select()
                        tx = "Primary Header"
                        If odd_even = True Then
                            tx = "Odd Page Header"
                        End If
                        parse_paragraph(ocomponents, ocomponent, body_style, True, "Text boxs in Headers/Footers")
                        oworddocument.Sections(1).Headers(1).shapes(j).select()
                    Next
                End If
            Next
        Catch ex As Exception
            MsgBox(ex.Message)

        End Try
    End Sub
    Public Sub componetize(ByRef ocomponents As bc_om_document_components)
        oworddocument.application.displayalerts = False

        Dim ocomponent As bc_om_style_component
        Dim co As Integer
        co = oworddocument.paragraphs.count
        Dim body_style As New ArrayList
        For i = 1 To oworddocument.paragraphs.count
            oworddocument.paragraphs(i).range.select()
            parse_paragraph(ocomponents, ocomponent, body_style)
            lstatus.Text = "parsing paragraph " + CStr(i) + " of " + CStr(co)
            lstatus.Refresh()
        Next
        oworddocument.application.displayalerts = True
        lstatus.Text = ""

    End Sub
    Public Sub parse_paragraph(ByRef ocomponents As bc_om_document_components, ByRef ocomponent As bc_om_style_component, ByRef body_style As ArrayList, Optional ByVal header As Boolean = False, Optional ByVal header_location As String = "")
        Dim opara As bc_om_style_component_paragraph
        For j = 0 To Me.componetize_styles.styles.Count - 1
            Try
                If oworddocument.application.selection.Style.namelocal = Me.componetize_styles.styles(j).header_style Then
                    If oworddocument.application.selection.range.text.length > 1 Then
                        ocomponent = New bc_om_style_component
                        ocomponent.title = oworddocument.application.selection.range.text
                        If rtf_flag = True Then
                            Dim ortf As bc_ao_word
                            ortf.worddocument = oworddocument

                            ocomponent.rtf = ortf.get_selection_in_rtf
                        Else
                            ocomponent.rtf = ""
                        End If
                        ocomponent.title = ocomponent.title.Substring(0, ocomponent.title.Length - 1)
                        If Asc(ocomponent.title.Substring(0, 1)) = 12 Or Asc(ocomponent.title.Substring(0, 1)) = 21 Then
                            ocomponent.title = ocomponent.title.Substring(1, ocomponent.title.Length - 1)
                        End If
                        If Asc(ocomponent.title.Substring(ocomponent.title.Length - 1, 1)) = 13 Then
                            ocomponent.title = ocomponent.title.Substring(0, ocomponent.title.Length - 1)
                        End If
                        ocomponent.style = Me.componetize_styles.styles(j).header_style
                        If header = False Then
                            ocomponent.page_no = oworddocument.application.selection.Range.Information(3)
                        Else
                            ocomponent.header = True
                            ocomponent.page_no = header_location

                        End If
                        ocomponent.table = False
                        ocomponent.chart_in_table = False
                        ocomponent.chart = False
                        If oworddocument.application.selection.Range.tables.count > 0 Then
                            ocomponent.table = True
                        ElseIf oworddocument.Application.Selection.Range.InlineShapes.Count = 1 Then
                            ocomponent.chart = True
                        End If

                        If Trim(ocomponent.title) <> "" Then
                            If ocomponent.table = True Then
                                REM compontize entire table at a later stage
                                ocomponent.table = True

                                REM now see if there is a chart in the table
                                If oworddocument.application.selection.Range.tables(1).range.InlineShapes.Count > 0 Then
                                    ocomponent.chart_in_table = True
                                End If
                                If rtf_flag = True Then
                                    Dim ortf As New bc_ao_word
                                    ortf.worddocument = oworddocument


                                    If Me.componetize_styles.styles(j).cell_only = False Then
                                        oworddocument.application.selection.Range.tables(1).range.select()
                                        ocomponent.rtf = ortf.get_selection_in_rtf
                                    Else
                                        ocomponent.rtf = ortf.get_selection_in_rtf
                                        ocomponent.table = False
                                    End If
                                    REM reset selection to paragraph
                                End If
                                body_style = Me.componetize_styles.styles(j).body_style
                                ocomponents.components.Add(ocomponent)
                            ElseIf ocomponent.chart = False Then
                                body_style = Me.componetize_styles.styles(j).body_style
                                ocomponents.components.Add(ocomponent)
                            End If
                            REM chart
                        End If
                        REM free flow chart with heading style around it
                        If ocomponent.chart Then
                            ocomponent.title = "Chart"
                            ocomponent.chart = True
                            If rtf_flag = True Then
                                Dim ortf As New bc_ao_word
                                ortf.worddocument = oworddocument

                                ocomponent.rtf = ortf.get_selection_in_rtf
                                ocomponents.components.Add(ocomponent)
                            End If
                        End If

                    End If
                    Exit Sub
                End If

            Catch ex As Exception
                MsgBox(ex.Message)


            End Try
        Next
        If body_style.Count > 0 Then
            REM if in a table ignore
            'If oworddocument.application.selection.range.tables.count = 0 Then
            Try
                For i = 0 To body_style.Count - 1
                    If oworddocument.application.selection.Style.namelocal = body_style(i) Then
                        REM if in a table but paragraph is not dont do anything
                        If (ocomponent.table = True And oworddocument.Application.Selection.Range.tables.count = 1) Or (ocomponent.table = False) Then
                            If oworddocument.Application.Selection.Range.InlineShapes.Count = 0 Then
                                If oworddocument.application.selection.range.text.length > 1 Then
                                    opara = New bc_om_style_component_paragraph
                                    opara.text = oworddocument.application.selection.range.text
                                    If Asc(opara.text) <> 13 Then
                                        If rtf_flag = True Then

                                            Try
                                                Dim ortf As New bc_ao_word
                                                ortf.worddocument = oworddocument


                                                oworddocument.application.displayalerts = False
                                                opara.rtf = ortf.get_selection_in_rtf
                                            Catch
                                                opara.rtf = ""
                                            End Try
                                        Else
                                            opara.rtf = ""
                                        End If
                                        opara.text = opara.text.Substring(0, opara.text.Length - 1)
                                        opara.text = RTrim(opara.text)
                                        If Trim(opara.text) <> "" Then

                                            If Asc(opara.text.Substring(0, 1)) = 12 Or Asc(opara.text.Substring(0, 1)) = 13 Then
                                                opara.text = opara.text.Substring(1, opara.text.Length - 1)
                                            End If
                                            If Asc(opara.text.Substring(opara.text.Length - 1, 1)) = 13 Then
                                                opara.text = opara.text.Substring(0, opara.text.Length - 1)
                                            End If
                                            opara.style = body_style(i)
                                            opara.page_no = oworddocument.application.selection.Range.Information(3)
                                            ocomponent.paragraphs.Add(opara)
                                        End If
                                    End If
                                End If

                            ElseIf oworddocument.Application.Selection.Range.InlineShapes.Count = 1 Then
                                REM chart in body style
                                opara = New bc_om_style_component_paragraph
                                opara.style = body_style(i)
                                opara.page_no = oworddocument.application.selection.Range.Information(3)
                                opara.text = "Chart"
                                opara.chart = True
                                If rtf_flag = True Then
                                    Dim ortf As New bc_ao_word
                                    ortf.worddocument = oworddocument

                                    opara.rtf = ortf.get_selection_in_rtf
                                    ocomponent.paragraphs.Add(opara)
                                End If

                            End If
                        End If
                    End If
                Next
            Catch

            End Try
            'End If
        End If
    End Sub

    Private Sub update_type_list(ByVal tx As String)
        For i = 1 To Me.citemtype.Items.Count - 1
            If Me.citemtype.Items(i) = tx Then
                Exit Sub
            End If
        Next
        Me.citemtype.Items.Add(tx)
    End Sub
    Private Sub update_style_list(ByVal tx As String)
        For i = 1 To Me.Cstyle.Items.Count - 1
            If Me.Cstyle.Items(i) = tx Then
                Exit Sub
            End If
        Next
        Me.Cstyle.Items.Add(tx)
    End Sub
    Private Function body_search(ByVal component As bc_om_style_component, ByVal str As String) As Boolean
        Dim i As Integer
        body_search = False
        For i = 0 To component.paragraphs.Count - 1
            component.paragraphs(i).search = False
            If Trim(str) <> "" Then
                If InStr(UCase(component.paragraphs(i).text), UCase(str)) > 0 Then
                    body_search = True
                    component.paragraphs(i).search = True
                End If
            End If
        Next
    End Function
    Private Sub write_out_components(ByVal ocomponents As bc_om_document_components, Optional ByVal update_filters As Boolean = False, Optional ByVal search As String = "")
        If search_mode = True Then
            Exit Sub
        End If
        Me.tcomponents.Enabled = False
        Me.Cursor = Cursors.WaitCursor
        Me.bcopyselection.Enabled = False
        Me.pviewtype.Enabled = False
        Me.papplytype.Enabled = False
        view_ids.Clear()
        Dim pn As String = "0"
        Dim n As Integer
        Me.tcomponents.Nodes.Clear()
        Me.rtfpreview.Text = ""
        Me.rtfpreview.Rtf = ""
        Dim show As Boolean
        Try
            For i = 0 To ocomponents.components.Count - 1
                show = False
                If ((Me.Cpage.Text = "All Header Only" And ocomponents.components(i).header = True) Or (Me.Cpage.Text = "All Body Only" And ocomponents.components(i).header = False) Or (Me.Cpage.Text = "All" Or Me.Cpage.Text = "Page: " + CStr(ocomponents.components(i).page_no)) Or Me.Cpage.Text = CStr(ocomponents.components(i).page_no)) And (search = "" Or InStr(UCase(ocomponents.components(i).title), UCase(search)) > 0 Or body_search(ocomponents.components(i), search) = True) And ((Me.citemtype.Text = "All" Or (Me.citemtype.Text = "Table" And ocomponents.components(i).table = True) Or (Me.citemtype.Text = "Chart in Table" And ocomponents.components(i).chart_in_table = True) Or (Me.citemtype.Text = "Text" And ocomponents.components(i).table = False) Or (Me.citemtype.Text = "Chart" And ocomponents.components(i).chart = True)) And ((Me.Cstyle.Text = "All") Or Me.Cstyle.Text = ocomponents.components(i).style)) Then
                    If (CStr(pn) <> ocomponents.components(i).page_no And (Me.Cpage.Text = "All" Or Me.Cpage.Text = "All Header Only" Or Me.Cpage.Text = "All Body Only")) Or (CStr(pn) <> ocomponents.components(i).page_no And CStr(ocomponents.components(i).page_no) = Me.Cpage.Text Or "Page: " + CStr(ocomponents.components(i).page_no) = Me.Cpage.Text) Then
                        If IsNumeric(CStr(ocomponents.components(i).page_no)) Then
                            Me.tcomponents.Nodes.Add("Page: " + CStr(ocomponents.components(i).page_no))
                        Else
                            Me.tcomponents.Nodes.Add(CStr(ocomponents.components(i).page_no))
                        End If
                        If update_filters = True Then
                            If IsNumeric(CStr(ocomponents.components(i).page_no)) Then
                                Me.Cpage.Items.Add("Page: " + CStr(ocomponents.components(i).page_no))
                            Else
                                Me.Cpage.Items.Add(ocomponents.components(i).page_no)
                            End If
                        End If
                    End If
                    show = True
                    pn = ocomponents.components(i).page_no
                    If ocomponents.components(i).chart = True Then
                        If update_filters = True Then
                            update_type_list("Chart")
                        End If
                        If InStr(ocomponents.components(i).title, search) > 0 And search <> "" Then
                            Me.tcomponents.Nodes(Me.tcomponents.Nodes.Count - 1).Nodes.Add(CStr(ocomponents.components(i).title), CStr(ocomponents.components(i).title), 1)
                        Else
                            Me.tcomponents.Nodes(Me.tcomponents.Nodes.Count - 1).Nodes.Add(CStr(ocomponents.components(i).title), CStr(ocomponents.components(i).title), 6)
                        End If
                        view_ids.Add(i)
                    ElseIf ocomponents.components(i).table = True Then
                        If update_filters = True Then
                            update_type_list("Table")
                        End If
                        If ocomponents.components(i).chart_in_table = True Then
                            update_type_list("Chart in Table")
                        End If
                        If InStr(ocomponents.components(i).title, search) > 0 And search <> "" Then
                            Me.tcomponents.Nodes(Me.tcomponents.Nodes.Count - 1).Nodes.Add(CStr(ocomponents.components(i).title), CStr(ocomponents.components(i).title), 1)
                        Else
                            Me.tcomponents.Nodes(Me.tcomponents.Nodes.Count - 1).Nodes.Add(CStr(ocomponents.components(i).title), CStr(ocomponents.components(i).title), 5)
                        End If
                        view_ids.Add(i)
                    Else
                        If update_filters = True Then
                            update_type_list("Text")
                        End If
                        If InStr(ocomponents.components(i).title, search) > 0 And search <> "" Then

                            Me.tcomponents.Nodes(Me.tcomponents.Nodes.Count - 1).Nodes.Add(CStr(ocomponents.components(i).title) + " [" + ocomponents.components(i).style + "]", CStr(ocomponents.components(i).title) + " [" + ocomponents.components(i).style + "]", 1)
                        Else
                            If Me.Cstyle.SelectedIndex = 0 Then
                                Me.tcomponents.Nodes(Me.tcomponents.Nodes.Count - 1).Nodes.Add(CStr(ocomponents.components(i).title) + " [" + ocomponents.components(i).style + "]", CStr(ocomponents.components(i).title) + " [" + ocomponents.components(i).style + "]", 4)
                            Else
                                Me.tcomponents.Nodes(Me.tcomponents.Nodes.Count - 1).Nodes.Add(CStr(ocomponents.components(i).title), CStr(ocomponents.components(i).title), 4)
                            End If
                        End If
                        view_ids.Add(i)

                    End If
                    update_style_list(ocomponents.components(i).style)
                    If show = True Then
                        Dim ignore_para As Boolean = False
                        n = Me.tcomponents.Nodes(Me.tcomponents.Nodes.Count - 1).Nodes.Count
                        REM if one chart in a paragrpah make this heading a chart
                        'If ocomponents.components(i).paragraphs.count = 1 Then
                        '    If ocomponents.components(i).paragraphs(0).chart = True Then
                        '        ocomponents.components(i).chart = True
                        '        ocomponents.components(i).rtf = ocomponents.components(i).paragraphs(0).rtf
                        '        Me.tcomponents.Nodes(Me.tcomponents.Nodes.Count - 1).Nodes(n - 1).ImageIndex = 6
                        '        ignore_para = True
                        '    End If
                        'End If
                        If ignore_para = False Then
                            For j = 0 To ocomponents.components(i).paragraphs.count - 1
                                Try
                                    If ocomponents.components(i).paragraphs(j).search = True Then
                                        Me.tcomponents.Nodes(Me.tcomponents.Nodes.Count - 1).Nodes(n - 1).Nodes.Add(ocomponents.components(i).paragraphs(j).text, ocomponents.components(i).paragraphs(j).text, 1)
                                        Me.tcomponents.Nodes(Me.tcomponents.Nodes.Count - 1).Nodes(n - 1).Expand()
                                    Else
                                        If ocomponents.components(i).paragraphs(j).chart = True Then
                                            Me.tcomponents.Nodes(Me.tcomponents.Nodes.Count - 1).Nodes(n - 1).Nodes.Add(ocomponents.components(i).paragraphs(j).text, ocomponents.components(i).paragraphs(j).text, 6)
                                        Else
                                            Me.tcomponents.Nodes(Me.tcomponents.Nodes.Count - 1).Nodes(n - 1).Nodes.Add(ocomponents.components(i).paragraphs(j).text, ocomponents.components(i).paragraphs(j).text, 3)
                                        End If
                                    End If
                                Catch

                                End Try
                            Next
                        End If
                    End If
                End If


            Next
            For i = 0 To Me.tcomponents.Nodes.Count - 1
                Me.tcomponents.Nodes(i).Expand()
            Next
            If Me.tcomponents.Nodes.Count = 0 Then
                Me.tcomponents.Nodes.Add("No results for search", "No results for search", 2)
            End If
            Me.tcomponents.Enabled = True
        Catch ex As Exception
            MsgBox(ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        set_up_styles()
        Me.lcomponents.components.Clear()
        oworddocument.application.screenupdating = True
        If Me.headers = True Then
            Me.lstatus.Text = "Parsing Headers...."
            Me.lstatus.Refresh()
            componetize_headers_footers(lcomponents)
        End If
        componetize(lcomponents)

        componetize_items_in_text_boxes(lcomponents)

        Dim bcs As New bc_cs_central_settings(True)
        lcomponents.doc_id = 100008
        lcomponents.db_write()
    End Sub

    REM deal with stuff in text boxes
    Public Sub componetize_items_in_text_boxes(ByRef ocomponents As bc_om_document_components)
        Dim ocomponent As bc_om_style_component
        Dim body_style As New ArrayList
        For i = 1 To oworddocument.Shapes.Count
            lstatus.Text = "parsing text box " + CStr(i) + " of " + CStr(oworddocument.Shapes.Count)
            lstatus.Refresh()
            Try
                oworddocument.Shapes(i).Select()
                If oworddocument.application.Selection.StoryType = 5 Then
                    For j = 1 To oworddocument.application.Selection.paragraphs.count
                        Try
                            oworddocument.application.Selection.paragraphs(j).range.select()
                            Me.parse_paragraph(ocomponents, ocomponent, body_style)
                            oworddocument.Shapes(i).Select()
                        Catch

                        End Try
                    Next
                End If
            Catch ex As Exception
                MsgBox(ex.Message)

            End Try

        Next
        lstatus.Text = ""
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.write_out_components(lcomponents)

    End Sub

    Private Sub Cpage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cpage.SelectedIndexChanged
        Me.write_out_components(lcomponents)
        Me.Tsearch.Text = ""
    End Sub

    Private Sub citemtype_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles citemtype.SelectedIndexChanged
        Me.write_out_components(lcomponents)
        Me.Tsearch.Text = ""
    End Sub

    Private Sub Cstyle_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cstyle.SelectedIndexChanged
        Me.write_out_components(lcomponents)
        Me.Tsearch.Text = ""
    End Sub

    Private Sub rbody_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Tsearch.Clear()
    End Sub

    Private Sub rtitle_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Tsearch.Clear()
    End Sub

    Private Sub Tsearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tsearch.TextChanged
        Me.bsearch.Enabled = False
        Me.ralldocs.Enabled = False
        Me.Rhighlight.Enabled = False

        If Trim(Me.Tsearch.Text) <> "" Then
            Me.bsearch.Enabled = True
            Me.ralldocs.Enabled = True
            If Me.ldocs.SelectedItems.Count = 1 Then
                Me.Rhighlight.Enabled = True
            End If
        End If
    End Sub
    Private Sub reset_filter()
        Try
            Me.Cstage.SelectedIndex = 0
            Me.Cpubtype.SelectedIndex = 0
            Me.Cauthor.SelectedIndex = 0
            Me.Centity.SelectedIndex = 0
            Me.Cpage.SelectedIndex = 0
            Me.Cstyle.SelectedIndex = 0
            Me.citemtype.SelectedIndex = 0
        Catch ex As Exception


        End Try
    End Sub
    Private Sub clear_search_flag()
        For i = 0 To docs.documents.Count - 1
            docs.documents(i).search = False
        Next
    End Sub
    Private Sub bsearch_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsearch.Click
        Me.Cursor = Cursors.WaitCursor

        Try
            If ralldocs.Checked = True Then
                Me.Pfilter.Enabled = False
                reset_filter()
                Me.ldocs.Items.Clear()
                Me.ldocs.Refresh()
                Me.tcomponents.Nodes.Clear()
                Me.rtfpreview.Text = ""
                Me.rtfpreview.Rtf = ""
                Dim osearchdocs As New bc_om_component_search
                osearchdocs.search_text = Me.Tsearch.Text
                REM search all docs in lis
                For i = 0 To docs.documents.Count - 1
                    osearchdocs.search_in_doc_ids.Add(docs.documents(i).doc_Id)
                Next
                osearchdocs.db_read()
                clear_search_flag()
                REM now set document list with search results
                For i = 0 To docs.documents.Count - 1
                    For j = 0 To osearchdocs.result_doc_ids.Count - 1
                        If docs.documents(i).doc_id = osearchdocs.result_doc_ids(j) Then
                            docs.documents(i).search = True
                            Exit For
                        End If
                    Next
                Next
                Me.load_list()
            Else
                search_mode = True
                Me.Cpage.SelectedIndex = 0
                Me.citemtype.SelectedIndex = 0
                Me.Cstyle.SelectedIndex = 0
                search_mode = False
                Me.write_out_components(lcomponents, False, Me.Tsearch.Text)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bsearch", "Click", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bclear.Click
        clear()
    End Sub
    Private Sub clear()
        Me.Cursor = Cursors.WaitCursor
        Dim sdoc_id As String = ""
        If Me.ldocs.SelectedItems.Count = 1 Then
            sdoc_id = Me.ldocs.SelectedItems(0).SubItems(6).Text
        End If

        Try
            Me.Cpage.SelectedIndex = 0
            Me.citemtype.SelectedIndex = 0
            Me.Cstyle.SelectedIndex = 0
        Catch
        End Try
        Me.rtfpreview.Text = ""
        Me.rtfpreview.Rtf = ""
        Me.Tsearch.Text = ""
        Me.pviewtype.Enabled = False
        Me.clear_search_flag()
        Me.load_list()
        If IsNumeric(sdoc_id) Then
            For i = 0 To Me.ldocs.Items.Count - 1
                If Me.ldocs.Items(i).SubItems(6).Text = sdoc_id Then
                    Me.ldocs.Items(i).Selected = True
                    Exit For
                End If
            Next
        End If
        'Me.write_out_components(lcomponents, False)
        Me.Cursor = Cursors.Default

    End Sub

    Private Sub tcomponents_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles tcomponents.AfterSelect
        preview()
    End Sub
    Private Sub preview(Optional ByVal from_wrap_check As Boolean = False, Optional ByVal from_tree As Boolean = True)
        Dim ptx As String
        Dim rtftx As String
        Dim idx As String
        Dim i As Integer
        Dim ncount As Integer = 0
        Me.rtfpreview.Text = ""
        Me.binsert.Enabled = False
        Me.bcopyselection.Enabled = False
        Dim tx As String
        Try
            idx = get_index_of_highlighted_component()
            REM if table only supported in rtf
            If from_tree = True Then
                Me.rplain.Enabled = True
                Me.rstyle.Enabled = True
                Me.bshowplain.Enabled = True
                If from_wrap_check = False Then
                    Me.cwrap1.Checked = True
                End If
            End If

            Try
                tx = Me.tcomponents.SelectedNode.Parent.Parent.Text
                REM paragraph
                ptx = ptx + Me.lcomponents.components(Me.view_ids(idx)).paragraphs(Me.tcomponents.SelectedNode.Index).Text + vbCrLf + vbCrLf
                rtftx = rtftx + Me.lcomponents.components(Me.view_ids(idx)).paragraphs(Me.tcomponents.SelectedNode.Index).rtf
                If rtftx <> "" Then
                    rtftx = rtftx.Substring(0, rtftx.Length - 2)

                End If
                If from_tree = True Then
                    Me.bshowplain.Enabled = False

                    If Me.lcomponents.components(Me.view_ids(idx)).paragraphs(Me.tcomponents.SelectedNode.Index).chart = False Then
                        Me.bshowplain.Enabled = True
                        If rtftx = "" Then
                            Me.bshowplain.Checked = True
                        End If
                    Else
                        Me.bshowRtf.Checked = True

                    End If
                End If
            Catch

                REM title
                If from_tree = True Then
                    If Me.lcomponents.components(Me.view_ids(idx)).table = True Or Me.lcomponents.components(Me.view_ids(idx)).chart = True Then
                        Me.rplain.Enabled = False
                        Me.rstyle.Enabled = False
                        Me.bshowplain.Enabled = False
                        Me.rrtf.Checked = True
                        Me.bshowRtf.Checked = True
                        If from_wrap_check = False Then
                            Me.cwrap1.Checked = False
                        End If
                    End If
                End If

                ptx = Me.lcomponents.components(Me.view_ids(idx)).title
                rtftx = Me.lcomponents.components(Me.view_ids(idx)).rtf
                If Me.lcomponents.components(Me.view_ids(idx)).chart = False And Me.lcomponents.components(Me.view_ids(idx)).table = False Then
                    If rtftx <> "" Then
                        rtftx = rtftx.Substring(0, rtftx.Length - 2)
                    End If
                    ptx = ptx + vbCrLf + vbCrLf
                    For i = 0 To Me.lcomponents.components(Me.view_ids(idx)).paragraphs.Count - 1
                        ptx = ptx + Me.lcomponents.components(Me.view_ids(idx)).paragraphs(i).Text + vbCrLf + vbCrLf
                        rtftx = rtftx + Me.lcomponents.components(Me.view_ids(idx)).paragraphs(i).rtf
                        If rtftx <> "" Then
                            rtftx = rtftx.Substring(0, rtftx.Length - 2)
                        End If
                    Next
                End If
            End Try
            If from_tree = True Then
                Me.bshowRtf.Enabled = False
                Me.rrtf.Enabled = False
                Me.rstyle.Checked = True

                If rtftx.Length > 10 Then
                    Me.bshowRtf.Enabled = True
                    Me.rrtf.Enabled = True
                    Me.rrtf.Checked = True
                End If
            End If
            If Me.bshowRtf.Checked = True And rtftx.Length > 10 Then
                Me.rtfpreview.Rtf = rtftx
                If from_tree = True Then
                    Me.bshowRtf.Enabled = True
                    Me.bshowRtf.Checked = True
                End If
            Else
                Me.rtfpreview.Text = ptx
                If from_tree = True Then
                    Me.bshowplain.Checked = True
                End If

            End If
            Me.binsert.Enabled = True
            Me.pviewtype.Enabled = True
            Me.papplytype.Enabled = True
        Catch
            Me.pviewtype.Enabled = False
            Me.papplytype.Enabled = False
        End Try
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Hide()
    End Sub

    Private Sub Button2_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles binsert.Click
        insert_component()
        Me.binsert.Enabled = False
    End Sub
    Private Function get_index_of_highlighted_component() As Integer
        REM need to work out which component we are on
        Dim idx_count As Integer = 0
        For i = 0 To Me.tcomponents.Nodes.Count - 1
            For j = 0 To Me.tcomponents.Nodes(i).Nodes.Count - 1
                If Me.tcomponents.Nodes(i).Nodes(j).Text = Me.tcomponents.SelectedNode.Text Or Me.tcomponents.Nodes(i).Nodes(j).Text = Me.tcomponents.SelectedNode.Parent.Text Then
                    get_index_of_highlighted_component = idx_count
                    Exit Function

                End If
                idx_count = idx_count + 1
            Next
        Next
    End Function
    Private Sub insert_selection()
        Dim ortf As New bc_ao_word
        ortf.worddocument = oworddocument


        If Me.bshowRtf.Checked = True Then
            ortf.set_selection_from_rtf(Me.rtfpreview.SelectedRtf)
        Else
            ortf.set_selection_from_rtf(Me.rtfpreview.SelectedText)
        End If
        Me.bcopyselection.Enabled = False

    End Sub
    Private Sub insert_component()
        Dim style_err As Boolean = False
        If rrtf.Checked = True Then
            Dim ortf As New bc_ao_word
            ortf.worddocument = oworddocument

            ortf.set_selection_from_rtf(Me.rtfpreview.Rtf)
        End If
        If rplain.Checked = True Or rstyle.Checked = True Then
            Dim ptx As String
            Dim idx As Integer = -1

            REM get component from list check if in  title or body
            Try
                ptx = Me.tcomponents.SelectedNode.Parent.Parent.Text
                REM body
                ptx = Me.tcomponents.SelectedNode.Parent.Text
                idx = Me.tcomponents.SelectedNode.Index
            Catch
                REM title
                ptx = Me.tcomponents.SelectedNode.Text
            End Try
            For i = 1 To Me.Cstyle.Items.Count - 1
                If InStr(ptx, "[" + Me.Cstyle.Items(i) + "]") > 0 Then
                    ptx = ptx.Replace("[" + Me.Cstyle.Items(i) + "]", "")
                    Exit For
                End If
            Next
            For i = 0 To lcomponents.components.Count - 1
                If Trim(lcomponents.components(i).title) = Trim(ptx) Then
                    If idx = -1 Then
                        Me.oworddocument.application.selection.text = lcomponents.components(i).title
                        Try
                            If rstyle.Checked = True Then
                                Me.oworddocument.application.selection.style = lcomponents.components(i).style
                            End If
                        Catch
                            style_err = True
                        End Try
                    End If
                    For j = 0 To lcomponents.components(i).paragraphs.count - 1
                        If idx = -1 Or idx = j Then
                            If idx = -1 Then
                                Me.oworddocument.application.selection.paragraphs.add()
                                Me.oworddocument.application.Selection.MoveDown(4, 1)
                            End If
                            Me.oworddocument.application.Selection.Text = lcomponents.components(i).paragraphs(j).text
                            Try
                                If rstyle.Checked = True Then
                                    Me.oworddocument.application.selection.style = lcomponents.components(i).paragraphs(j).style
                                End If
                            Catch
                                style_err = True
                            End Try
                        End If
                    Next
                    Exit For
                End If
            Next

        End If

    End Sub
    
   

    Private Sub bshowRtf_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bshowRtf.CheckedChanged
        If Me.bshowRtf.Checked = True Then
            preview(False, False)
        End If
    End Sub

    Private Sub bshowplain_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bshowplain.CheckedChanged
        If Me.bshowplain.Checked = True Then
            preview(False, False)
        End If
    End Sub

    Private Sub Form1_MdiChildActivate(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.MdiChildActivate

    End Sub

    Private Sub rtfpreview_SelectionChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles rtfpreview.SelectionChanged
        Me.bcopyselection.Enabled = True
    End Sub

    Private Sub rtfpreview_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rtfpreview.TextChanged

    End Sub

    Private Sub cwrap1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cwrap1.CheckedChanged
        If Me.cwrap1.Checked = True Then
            Me.rtfpreview.WordWrap = True
        Else
            Me.rtfpreview.WordWrap = False
        End If
        preview(True)
    End Sub

    Private Sub bcopyselection_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcopyselection.Click
        insert_selection()
    End Sub

    Public Sub load_docs_db()
        If loading = True Then
            Exit Sub
        End If
        docs.date_from = Me.Dfrom.Value
        docs.date_to = Me.Dto.Value
        docs.packet_code = "process"
        docs.user_id = 1
        If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
            docs.db_read()
        Else
            docs.tmode = bc_cs_soap_base_class.tREAD
            docs.transmit_to_server_and_receive(docs, True)
        End If


        load_dynamic_lists()
        load_list()
    End Sub

    Private Sub Centity_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Centity.SelectedIndexChanged
        If Centity.SelectedIndex > -1 Then
            load_list()
        End If
    End Sub

    Private Sub Dto_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Dto.ValueChanged

        load_docs_db()

    End Sub



    Private Sub document_selected()
        Try
            If Me.ldocs.SelectedItems.Count = 0 Then
                Exit Sub
            End If
            Me.Cursor = Cursors.WaitCursor
            lcomponents.components.Clear()
            lcomponents.doc_id = CInt(Me.ldocs.SelectedItems(0).SubItems(6).Text)
            Dim sx As String
            sx = Me.Tsearch.Text
            search_mode = True
            Me.Cpage.Items.Clear()
            Me.Cpage.Items.Add("All")
            Me.Cpage.Items.Add("All Body Only")
            Me.Cpage.Items.Add("All Header Only")
            Me.Cpage.SelectedIndex = 0
            Me.citemtype.Items.Clear()
            Me.citemtype.Items.Add("All")
            Me.citemtype.SelectedIndex = 0
            Me.Cstyle.Items.Clear()
            Me.Cstyle.Items.Add("All")
            Me.Cstyle.SelectedIndex = 0
            Me.Pfilter.Enabled = True
            If Me.Tsearch.Text <> "" Then
                Me.Rhighlight.Enabled = True
            Else
                Me.Rhighlight.Enabled = False
            End If
            Me.Rhighlight.Checked = True
            Me.Tsearch.Text = sx
            lcomponents.db_read()
            search_mode = False
            write_out_components(lcomponents, True, sx)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Insert Component", "document_selected", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub ldocs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ldocs.SelectedIndexChanged
        Me.document_selected()
    End Sub
    Private Sub clear_comps()
        Me.Pfilter.Enabled = False
        Me.tcomponents.Nodes.Clear()
    End Sub
    Private Sub set_up_lists()
        Try
            clear_comps()
            Dim tp As TimeSpan
            tp = New TimeSpan(28, 0, 0, 0, 0)
            Dim da As DateTime
            da = Now
            loading = True
            Me.Dfrom.Value = da.Subtract(tp)
            loading = False
            Me.Dto.Value = Now
            loading = False
            REM set defaults for doc
            'Try
            '    If ldoc.pub_type_name = "" Then
            '        Me.cctype.SelectedIndex = 0
            '    Else
            '        Me.cctype.Text = ldoc.pub_type_name
            '    End If
            'Catch
            '    Me.cctype.SelectedIndex = 0
            'End Try
            'Dim i As Integer
            'For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
            '    If bc_am_load_objects.obc_entities.entity(i).id = ldoc.entity_id Then
            '        Me.Centity.Text = bc_am_load_objects.obc_entities.entity(i).name
            '        Exit For
            '    End If
            'Next
            'For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
            '    If bc_am_load_objects.obc_users.user(i).id = ldoc.originating_author Then
            '        Me.Cauthor.Text = bc_am_load_objects.obc_users.user(i).first_name + " " + bc_am_load_objects.obc_users.user(i).surname
            '        Exit For
            '    End If
            'Next
            Me.Cstage.Text = "All"
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ins_adhoc_component", "set_up_lists", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            loading = False
        End Try


    End Sub
    Private Sub load_dynamic_lists()
        Try
            clear_comps()
            Dim op As String
            Dim oa As String
            Dim oe As String
            Dim os As String
            op = Me.Cpubtype.Text
            oa = Me.Cauthor.Text
            oe = Me.Centity.Text
            os = Me.Cstage.Text

            Me.Cpubtype.Items.Clear()
            Me.Cpubtype.Items.Add("All")
            Me.Cstage.Items.Clear()
            Me.Cstage.Items.Add("All")
            Me.Centity.Items.Clear()
            Me.Centity.Items.Add("All")
            Me.Cauthor.Items.Clear()
            Me.Cauthor.Items.Add("All")
            Dim i, j As Integer
            Dim found As Boolean
            For i = 0 To docs.documents.Count - 1
                found = False
                For j = 1 To Me.Cpubtype.Items.Count - 1
                    If CStr(Me.Cpubtype.Items(j)) = CStr(docs.documents(i).pub_type_name) Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    Me.Cpubtype.Items.Add(CStr(docs.documents(i).pub_type_name))
                End If
                found = False
                For j = 1 To Me.Cstage.Items.Count - 1
                    If CStr(Me.Cstage.Items(j)) = CStr(docs.documents(i).stage) Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    Me.Cstage.Items.Add(CStr(docs.documents(i).stage))
                End If
                found = False
                For j = 1 To Me.Centity.Items.Count - 1
                    If CStr(Me.Centity.Items(j)) = CStr(docs.documents(i).entity) Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    Me.Centity.Items.Add(docs.documents(i).entity)
                End If
                found = False
                For j = 1 To Me.Cauthor.Items.Count - 1
                    If CStr(Me.Cauthor.Items(j)) = CStr(docs.documents(i).author) Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    Me.Cauthor.Items.Add(docs.documents(i).author)
                End If

            Next

            Try
                Me.Cpubtype.Text = op
                If Me.Cpubtype.SelectedIndex = -1 Then
                    Me.Cpubtype.SelectedIndex = 0
                End If
            Catch ex As Exception
                Me.Cpubtype.SelectedIndex = 0
            End Try
            Try
                Me.Centity.Text = oe
                If Me.Centity.SelectedIndex = -1 Then
                    Me.Centity.SelectedIndex = 0
                End If
            Catch ex As Exception
                Me.Centity.SelectedIndex = 0
            End Try
            Try
                Me.Cauthor.Text = oa
                If Me.Cauthor.SelectedIndex = -1 Then
                    Me.Cauthor.SelectedIndex = 0
                End If
            Catch ex As Exception
                Me.Cauthor.SelectedIndex = 0

            End Try
            Try
                Me.Cstage.Text = os
                If Me.Cstage.SelectedIndex = -1 Then
                    Me.Cstage.SelectedIndex = 0
                End If
            Catch ex As Exception
                Me.Cstage.Text = "All"

            End Try


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ins_adhoc_component", "load_dynamic_lists", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            loading = False
        End Try
    End Sub
    Private Sub load_list()
        Dim i As Integer
        Dim lvew As ListViewItem
        Dim k As bc_om_document
        clear_comps()
        Me.ldocs.Items.Clear()
        Dim f_all As Boolean
        Dim f_p As Boolean
        Dim f_s As Boolean
        Dim f_e As Boolean
        Dim f_a As Boolean


        For i = 0 To docs.documents.Count - 1
            f_all = False
            f_p = False
            f_s = False
            f_e = False
            f_a = False


            lvew = New ListViewItem(Format(docs.documents(i).doc_date, "dd-MMM-yyyy HH:mm"))
            Me.ldocs.SmallImageList = Me.ImageList1

            If docs.documents(i).search = True Then
                lvew.ImageIndex = 1
            Else
                lvew.ImageIndex = 3
            End If
            lvew.SubItems.Add(CStr(docs.documents(i).title))
            lvew.SubItems.Add(CStr(docs.documents(i).pub_type_name))
            lvew.SubItems.Add(CStr(docs.documents(i).entity))
            lvew.SubItems.Add(CStr(docs.documents(i).author))
            lvew.SubItems.Add(CStr(docs.documents(i).stage))
            lvew.SubItems.Add(CStr(docs.documents(i).doc_id))
            If Me.Cpubtype.Text = "All" And Me.Cauthor.Text = "All" And Me.Centity.Text = "All" And Me.Cstage.Text = "All" Then
                f_all = True
            End If
            If Me.Cpubtype.Text = docs.documents(i).pub_type_name Or Me.Cpubtype.Text = "All" Then
                f_p = True
            End If
            If Me.Cstage.Text = docs.documents(i).stage Or Me.Cstage.Text = "All" Then
                f_s = True
            End If
            If Me.Centity.Text = docs.documents(i).entity Or Me.Centity.Text = "All" Then
                f_e = True
            End If
            If Me.Cauthor.Text = docs.documents(i).author Or Me.Cauthor.Text = "All" Then
                f_a = True
            End If
            If (f_all = True Or (f_p = True And f_s = True And f_e = True And f_a = True)) And (Me.Tsearch.Text = "" Or (Me.Tsearch.Text <> "" And docs.documents(i).search = True)) Then
                If docs.documents(i).doc_id <> ldoc.id Then
                    Me.ldocs.Items.Add(lvew)
                End If
            End If
        Next
    End Sub

    Private Sub Cstage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cstage.SelectedIndexChanged
        If Cstage.SelectedIndex > -1 Then
            load_list()
        End If
    End Sub

    Private Sub Cauthor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cauthor.SelectedIndexChanged
        If Cauthor.SelectedIndex > -1 Then
            load_list()
        End If
    End Sub

    Private Sub Cpubtype_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cpubtype.SelectedIndexChanged
        If Cpubtype.SelectedIndex > -1 Then
            load_list()
        End If
    End Sub

    Private Sub Dfrom_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Dfrom.ValueChanged
        load_docs_db()
    End Sub

    Private Sub ralldocs_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ralldocs.CheckedChanged

    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class