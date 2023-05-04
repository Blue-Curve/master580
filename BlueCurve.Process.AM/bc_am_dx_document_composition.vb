Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports DevExpress.XtraTreeList
Imports System.Windows.Forms

Public Class bc_am_dx_document_composition
    Implements Ibc_am_document_composition
    Dim _comps As bc_om_document_components
    Dim _doc_id As Long
    Dim _filename As String
    Dim _selected_comps As bc_om_document_components
    Private _word As New bc_ao_word
    Public Event close1() Implements Ibc_am_document_composition.close
    Dim _new_comp As Boolean = False
    Class sel_comp
        Public na As String
        Public ty As String
        Public pg As String
        Public co As String
        Public pa As String
        Public add_text As String
        Public display_ord As Integer
        Public table_locator As String

    End Class
    Dim _sel_comps As New List(Of sel_comp)
    Private Sub get_Image(ByVal table_locator As String)
        Try
            Dim fn As String
            fn = bc_cs_central_settings.local_repos_path + "img_" + CStr(_doc_id) + "_" + table_locator + ".png"
            Dim fs As New bc_cs_file_transfer_services
            If fs.check_document_exists(fn) = True Then
                set_preview(fn)
                Exit Sub
            End If
            If _word.get_table_as_image_from_bm(table_locator, fn, _doc_id) = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Failed to Generate Image for Table", bc_cs_message.MESSAGE, False, False, "yes", "no", True)
                Exit Sub
            End If
            set_preview(fn)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("ctrll_bc_am_document_composition", "get_image", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    REM html compontizer and powerpoint
    Public Sub load_list_new(title As String, doc_id As Long, comps As Core.OM.bc_om_document_components, filename As String, Optional reload As Boolean = False, Optional read_only As Boolean = False) Implements Ibc_am_document_composition.load_list_new
        Try
            _new_comp = True
            Dim tln As Nodes.TreeListNode = Nothing
            Dim osel As sel_comp

            Dim sel_items As New List(Of ListViewItem)
            Dim sel_order As New List(Of Integer)
            Dim dcomps As New List(Of sel_comp)

            Me.Text = "Blue Curve - Document Composition: " + title

            If read_only = True Then
                Me.Text = Me.Text + " (read only)"
                breset.Visible = False
                bupdate.Visible = False
                bok.Visible = False
                bup.Visible = False
                bdn.Visible = False
                bcancel.Text = "Close"
                bcancel.Location = bok.Location
                Me.tinfo.Enabled = False
            End If

            If Not IsNothing(comps) Then
                _comps = comps
            End If
            Me.uxcomps.BeginUpdate()


            Me.uxcomps.Nodes.Clear()
            Dim n As DevExpress.XtraTreeList.Nodes.TreeListNode
            Me.bok.Enabled = False
            _filename = filename
            _doc_id = doc_id

            For i = 0 To comps.components.Count - 1
                If comps.components(i).display_order > 0 And reload = False Then
                    osel = New sel_comp
                    osel.na = comps.components(i).text
                    If comps.components(i).bimage = True Then
                        osel.na = "Image: Lead"
                    End If
                    If comps.components(i).bimage = True Then
                        osel.ty = "Image: Lead"
                    End If
                    If comps.components(i).btable = True Then
                        If comps.components(i).chart_in_table = True Then
                            osel.ty = "Image in Table: Lead"
                        Else
                            osel.ty = "Table: Lead"
                        End If
                    ElseIf comps.components(i).bimage Then
                        osel.ty = "Image: Lead"
                    Else
                        osel.ty = "Text: Lead"
                    End If
                    osel.pg = comps.components(i).page_no
                    osel.co = comps.components(i).component_id
                    osel.pa = 0
                    osel.add_text = comps.components(i).description
                    osel.display_ord = comps.components(i).display_order
                    dcomps.Add(osel)
                End If

                If Me.chkshowtext.Checked = True Or comps.components(i).bimage = True Or comps.components(i).btable = True Then
                    If comps.components(i).bimage = True Then
                        n = uxcomps.AppendNode(New Object() {"Image: Sub"}, tln)
                    Else
                        n = uxcomps.AppendNode(New Object() {comps.components(i).text}, tln)
                    End If
                    If comps.components(i).text_box_id = 0 Then
                        If comps.components(i).bimage = True Then
                            n.SetValue(1, "Image: Lead")
                            n.StateImageIndex = 3
                        End If

                        If comps.components(i).btable = True Then
                            If comps.components(i).chart_in_table = True Then
                                n.SetValue(1, "Image in Table: Lead")
                                n.StateImageIndex = 6
                            Else
                                n.SetValue(1, "Table: Lead")
                                n.StateImageIndex = 5
                            End If
                        ElseIf comps.components(i).bimage Then
                            n.SetValue(1, "Image: Lead")
                            n.StateImageIndex = 3
                        Else
                            n.SetValue(1, "Text: Lead")
                            n.StateImageIndex = 3
                        End If
                        n.SetValue(2, CStr(comps.components(i).page_no))

                    Else
                        If comps.components(i).bimage = True Then
                            n.SetValue(1, "Image (Frame)")
                            n.StateImageIndex = 4
                        End If

                        If comps.components(i).btable = True Then
                            If comps.components(i).chart_in_table = True Then
                                n.SetValue(1, "Image in Table (Frame)")
                                n.StateImageIndex = 4
                            Else
                                n.SetValue(1, "Table (Frame)")
                                n.StateImageIndex = 5
                            End If
                        ElseIf comps.components(i).bimage Then
                            n.SetValue(1, "Image (Frame)")
                            n.StateImageIndex = 4
                            n.StateImageIndex = 4
                        Else
                            n.SetValue(1, "Text (Frame)")
                            n.StateImageIndex = 3
                        End If
                        n.SetValue(2, CStr(comps.components(i).page_no) + " : " + CStr(comps.components(i).text_box_id))
                    End If

                    n.SetValue(3, CStr(comps.components(i).component_id))
                    n.SetValue(4, "0")
                    n.SetValue(6, CStr(comps.components(i).style))

                End If
            Next


            If reload = False Then
                For i = 0 To dcomps.Count - 1
                    For j = 0 To dcomps.Count - 1
                        If dcomps(j).display_ord - 1 = i Then
                            osel = New sel_comp
                            osel = dcomps(j)
                            _sel_comps.Add(osel)
                            Exit For
                        End If
                    Next
                Next

                load_sel_comps()
            End If

            If Me.uxcomps.Nodes.Count > 0 Then
                Me.uxcomps.FocusedNode = Nothing
                Me.uxcomps.FocusedNode = Me.uxcomps.Nodes(0)
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_document_compoisition", "load_list_new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.uxcomps.EndUpdate()
        End Try
    End Sub


    Public Sub load_list(title As String, doc_id As Long, comps As Core.OM.bc_om_document_components, filename As String, Optional reload As Boolean = False, Optional read_only As Boolean = False) Implements Ibc_am_document_composition.load_list
        Try
            Dim tln As Nodes.TreeListNode = Nothing
            Dim osel As sel_comp

            Dim sel_items As New List(Of ListViewItem)
            Dim sel_order As New List(Of Integer)
            Dim dcomps As New List(Of sel_comp)

            Me.Text = "Blue Curve - Document Composition: " + title

            If read_only = True Then
                Me.Text = Me.Text + " (read only)"
                breset.Visible = False
                bupdate.Visible = False
                bok.Visible = False
                bup.Visible = False
                bdn.Visible = False
                bcancel.Text = "Close"
                bcancel.Location = bok.Location
                Me.tinfo.Enabled = False
            End If

            If Not IsNothing(comps) Then
                _comps = comps
            End If
            Me.uxcomps.BeginUpdate()


            Me.uxcomps.Nodes.Clear()
            Dim n As DevExpress.XtraTreeList.Nodes.TreeListNode
            Me.bok.Enabled = False
            _filename = filename
            _doc_id = doc_id

            For i = 0 To comps.components.Count - 1
                If comps.components(i).display_order > 0 And reload = False Then
                    osel = New sel_comp
                    osel.na = comps.components(i).text
                    If comps.components(i).bimage = True Then
                        osel.na = "Chart: Lead"
                    End If
                    If comps.components(i).bimage = True Then
                        osel.ty = "Chart: Lead"
                    End If
                    If comps.components(i).btable = True Then
                        If comps.components(i).chart_in_table = True Then
                            osel.ty = "Chart in Table: Lead"
                        Else
                            osel.ty = "Table: Lead"
                        End If
                    ElseIf comps.components(i).bimage Then
                        osel.ty = "Chart: Lead"
                    Else
                        osel.ty = "Text: Lead"
                    End If
                    osel.pg = comps.components(i).page_no
                    osel.co = comps.components(i).component_id
                    osel.pa = 0
                    osel.add_text = comps.components(i).description
                    osel.display_ord = comps.components(i).display_order
                    dcomps.Add(osel)
                End If
                If Me.chkshowtext.Checked = True Or comps.components(i).bimage = True Or comps.components(i).btable = True Then
                    If comps.components(i).bimage = True Then
                        n = uxcomps.AppendNode(New Object() {"Chart: Sub"}, tln)
                    Else
                        n = uxcomps.AppendNode(New Object() {comps.components(i).text}, tln)
                    End If
                    If comps.components(i).text_box_id = 0 Then
                        If comps.components(i).bimage = True Then
                            n.SetValue(1, "Chart: Lead")
                            n.StateImageIndex = 4
                        End If

                        If comps.components(i).btable = True Then
                            If comps.components(i).chart_in_table = True Then
                                n.SetValue(1, "Chart in Table: Lead")
                                n.StateImageIndex = 6
                            Else
                                n.SetValue(1, "Table: Lead")
                                n.StateImageIndex = 5
                            End If
                        ElseIf comps.components(i).bimage Then
                            n.SetValue(1, "Chart: Lead")
                            n.StateImageIndex = 4
                        Else
                            n.SetValue(1, "Text: Lead")
                            n.StateImageIndex = 3
                        End If
                        n.SetValue(2, CStr(comps.components(i).page_no))

                    Else
                        If comps.components(i).bimage = True Then
                            n.SetValue(1, "Chart (Frame)")
                            n.StateImageIndex = 4
                        End If

                        If comps.components(i).btable = True Then
                            If comps.components(i).chart_in_table = True Then
                                n.SetValue(1, "Chart in Table (Frame)")
                                n.StateImageIndex = 4
                            Else
                                n.SetValue(1, "Table (Frame)")
                                n.StateImageIndex = 5
                            End If
                        ElseIf comps.components(i).bimage Then
                            n.SetValue(1, "Chart (Frame)")
                            n.StateImageIndex = 4
                            n.StateImageIndex = 4
                        Else
                            n.SetValue(1, "Text (Frame)")
                            n.StateImageIndex = 3
                        End If
                        n.SetValue(2, CStr(comps.components(i).page_no) + " : " + CStr(comps.components(i).text_box_id))
                    End If

                    n.SetValue(3, CStr(comps.components(i).component_id))
                    n.SetValue(4, "0")
                    n.SetValue(6, CStr(comps.components(i).style))



                End If
                For j = 0 To comps.components(i).paragraphs.Count - 1

                    If comps.components(i).paragraphs(j).display_order > 0 And reload = False Then

                        osel = New sel_comp
                        osel.na = comps.components(i).paragraphs(j).text
                        If comps.components(i).paragraphs(j).bimage = True Then
                            osel.na = "Chart: Sub"
                        End If
                        If comps.components(i).paragraphs(j).bimage = True Then
                            osel.ty = "Chart: Sub"
                        End If
                        If comps.components(i).paragraphs(j).btable = True Then
                            If comps.components(i).paragraphs(j).chart_in_table = True Then
                                osel.ty = "Chart in Table: Sub"
                            Else
                                osel.ty = "Table: Sub"
                            End If
                        ElseIf comps.components(i).paragraphs(j).bimage Then
                            osel.ty = "Chart: Sub"
                        Else
                            osel.ty = "Text: Sub"

                        End If
                        osel.pg = comps.components(i).paragraphs(j).page_no
                        osel.co = comps.components(i).component_id
                        osel.pa = comps.components(i).paragraphs(j).ord
                        osel.add_text = comps.components(i).paragraphs(j).description
                        osel.display_ord = comps.components(i).paragraphs(j).display_order
                        dcomps.Add(osel)

                    End If

                    If Me.chkshowtext.Checked = True Or comps.components(i).paragraphs(j).bimage = True Or comps.components(i).paragraphs(j).btable = True Then

                        If comps.components(i).paragraphs(j).bimage = True Then
                            n = uxcomps.AppendNode(New Object() {"Chart: Sub"}, tln)
                        Else
                            n = uxcomps.AppendNode(New Object() {comps.components(i).paragraphs(j).text}, tln)
                        End If
                        If comps.components(i).paragraphs(j).bimage = True Then
                            n.SetValue(1, "Chart: Sub")
                            n.StateImageIndex = 4
                        End If
                        If comps.components(i).paragraphs(j).btable = True Then
                            If comps.components(i).paragraphs(j).chart_in_table = True Then
                                n.SetValue(1, "Chart in Table: Sub")
                                n.StateImageIndex = 6
                            Else
                                n.SetValue(1, "Table: Sub")
                                n.StateImageIndex = 5
                            End If
                        ElseIf comps.components(i).paragraphs(j).bimage Then
                            n.SetValue(1, "Chart: Sub")
                            n.StateImageIndex = 4
                        Else
                            n.SetValue(1, "Text: Sub")
                            n.StateImageIndex = 3
                        End If

                        n.SetValue(2, CStr(comps.components(i).paragraphs(j).page_no))
                        n.SetValue(3, CStr(comps.components(i).component_id))
                        n.SetValue(4, CStr(comps.components(i).paragraphs(j).ord))
                        n.SetValue(5, CStr(comps.components(i).paragraphs(j).table_locator))
                        n.SetValue(6, CStr(comps.components(i).paragraphs(j).style))
                    End If
                Next
            Next


            If reload = False Then
                If comps.table_on_demand = True Then
                    _word.open_new_instance(_filename, False)
                End If

                For i = 0 To dcomps.Count - 1
                    For j = 0 To dcomps.Count - 1
                        If dcomps(j).display_ord - 1 = i Then
                            osel = New sel_comp
                            osel = dcomps(j)
                            _sel_comps.Add(osel)
                            Exit For
                        End If
                    Next
                Next

                load_sel_comps()
            End If
            If Me.uxcomps.Nodes.Count > 0 Then
                Me.uxcomps.FocusedNode = Nothing
                Me.uxcomps.FocusedNode = Me.uxcomps.Nodes(0)
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_document_compoisition", "load_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.uxcomps.EndUpdate()
        End Try
    End Sub

    Public Event reload(bwithtext As Boolean) Implements Ibc_am_document_composition.reload

    Public Event save(comps As Core.OM.bc_om_components_to_display) Implements Ibc_am_document_composition.save
    Private Function check_exit() As Boolean
        check_exit = True

        If Me.breset.Enabled = True Then
            Dim omsg As New bc_cs_message("Blue Curve", "You have unsaved additional text please update or reset first", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            check_exit = False
        End If
       
    End Function
    Public Sub set_preview(fn As String) Implements Ibc_am_document_composition.set_preview

        Me.ipreview.Visible = True
        ipreview.Image = System.Drawing.Image.FromFile(fn)

    End Sub

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        If check_exit() = False Then
            Exit Sub
        End If


        RaiseEvent close1()
        Me.Hide()
    End Sub
    Private Sub close_doc()
        If IsNothing(_word) = False And _comps.table_on_demand = True Then
            _word.close(False)
            _word.quit()
            Dim fs As New bc_cs_file_transfer_services
            fs.delete_file(_filename, Nothing, False)
        End If
    End Sub

    Private Sub bok_Click(sender As Object, e As EventArgs) Handles bok.Click
        Cursor = Cursors.WaitCursor


        Try
            If check_exit() = False Then
                Exit Sub
            End If



            Dim cdisps As New bc_om_components_to_display
            Dim cdisp As bc_om_component_to_display

            For j = 0 To Me._sel_comps.Count - 1
                cdisp = New bc_om_component_to_display
                cdisp.component_id = Me._sel_comps(j).co
                cdisp.ord = Me._sel_comps(j).pa
                cdisp.display_order = j + 1
                cdisp.desc = Me._sel_comps(j).add_text

                If Me._sel_comps(j).table_locator <> "" Then
                    cdisp.image_name = "img_" + CStr(_comps.doc_id) + "_" + Me._sel_comps(j).table_locator + ".png"
                    Dim fs As New bc_cs_file_transfer_services
                    If fs.check_document_exists(bc_cs_central_settings.local_repos_path + cdisp.image_name) = True Then
                        fs.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + cdisp.image_name, cdisp.bimage, Nothing, True)
                        fs.delete_file(bc_cs_central_settings.local_repos_path + cdisp.image_name)
                    End If
                End If
                cdisps.components.Add(cdisp)
            Next

            RaiseEvent save(cdisps)
            Me.Hide()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("save", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor = Cursors.Default
        End Try


    End Sub

    Private Sub chkshowtext_CheckedChanged(sender As Object, e As EventArgs) Handles chkshowtext.CheckedChanged
        Cursor = Cursors.WaitCursor
        RaiseEvent reload(Me.chkshowtext.Checked)
        Cursor = Cursors.Default
    End Sub

    Private Sub load_preview_new(Optional sel As Boolean = False)
        Me.Cursor = Cursors.WaitCursor
        Try
            Me.ipreview.Visible = False
            Me.rtfpreview.Visible = True

            If Me.uxcomps.Nodes.Count > 0 Then
                Me.rtfpreview.RtfText = ""
                Me.rtfpreview.HtmlText = ""


                For i = 0 To _comps.components.Count - 1
                    If (sel = false andalso _comps.components(i).component_id = Me.uxcomps.Selection.Item(0).GetValue(3)) or (sel = true andalso _comps.components(i).component_id = Me.uxselcomps.Selection.Item(0).GetValue(3))
                        'If _comps.components(i).component_id = Me.uxcomps.Selection.Item(0).GetValue(3) Then
                        If _comps.components(i).bimage = True Then
                            Me.rtfpreview.HtmlText = "<head></head><body><img src ='http://" + bc_cs_central_settings.check_components_image_url + "/components/" + CStr(_doc_id) + "/" + _comps.components(i).text + "' > "
                        ElseIf _comps.components(i).is_html Then
                            Me.rtfpreview.HtmlText = "<head><style>" + _comps.components(i).css + "</style></head><body>" + _comps.components(i).html + "</body"
                        Else
                            Me.rtfpreview.Text = _comps.components(i).text
                        End If
                    End If
                Next
            End If

        Catch ex As Exception

            'Dim oerr As New bc_cs_error_log("ListView1", "ListView1_SelectedIndexChanged_1", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default

        End Try
    End Sub

    Private Sub uxcomps_FocusedNodeChanged(sender As Object, e As FocusedNodeChangedEventArgs) Handles uxcomps.FocusedNodeChanged
        If _new_comp = True Then
            load_preview_new()
            Return
        End If

        Me.Cursor = Cursors.WaitCursor
        Try

            If Me.uxcomps.Nodes.Count > 0 Then
                Me.rtfpreview.RtfText = ""


                For i = 0 To _comps.components.Count - 1
                    If _comps.components(i).component_id = Me.uxcomps.Selection.Item(0).GetValue(3) Then
                        If Me.uxcomps.Selection.Item(0).GetValue(4) = "0" Then
                            If _comps.components(i).ortf <> "" Then
                                Me.rtfpreview.RtfText = _comps.components(i).ortf
                            Else
                                Me.rtfpreview.Text = _comps.components(i).text
                            End If
                            Exit Sub
                        Else
                            For j = 0 To _comps.components(i).paragraphs.Count - 1

                                If _comps.components(i).paragraphs(j).ord = Me.uxcomps.Selection.Item(0).GetValue(4) Then
                                    Me.rtfpreview.Visible = True
                                    Me.ipreview.Visible = False

                                    If _comps.components(i).paragraphs(j).ortf <> "" Then
                                        Me.rtfpreview.RtfText = _comps.components(i).paragraphs(j).ortf
                                    Else
                                        Me.rtfpreview.Text = _comps.components(i).paragraphs(j).text
                                    End If
                                    If _comps.components(i).paragraphs(j).btable = True Then
                                        If _comps.components(i).paragraphs(j).table_locator <> "" Then
                                            Me.rtfpreview.Visible = False
                                            Me.ipreview.Visible = True
                                            get_Image(_comps.components(i).paragraphs(j).table_locator)
                                        End If
                                    End If
                                    Exit Sub
                                End If
                            Next
                        End If
                    End If
                Next
            End If
        Catch ex As Exception

            'Dim oerr As New bc_cs_error_log("ListView1", "ListView1_SelectedIndexChanged_1", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default

        End Try
    End Sub
    Sub load_sel_comps()
        Me.tinfo.Enabled = False
        Me.uxselcomps.BeginUpdate()


        Dim tln As Nodes.TreeListNode = Nothing
        Dim n As DevExpress.XtraTreeList.Nodes.TreeListNode
        Me.uxselcomps.Nodes.Clear()

        For i = 0 To _sel_comps.Count - 1

            n = uxselcomps.AppendNode(New Object() {_sel_comps(i).na}, tln)
            n.SetValue(1, _sel_comps(i).ty)
            n.SetValue(2, _sel_comps(i).pg)
            n.SetValue(3, _sel_comps(i).co)
            n.SetValue(4, _sel_comps(i).pa)
            n.SetValue(5, _sel_comps(i).table_locator)

            If _sel_comps(i).ty.Substring(0, 4) = "Text" Then
                n.StateImageIndex = 3
            ElseIf _sel_comps(i).ty.Substring(0, 5) = "Chart" And InStr(_sel_comps(i).ty, "Table") = 0 Then
                n.StateImageIndex = 4
            ElseIf _sel_comps(i).ty.Substring(0, 5) = "Image" And InStr(_sel_comps(i).ty, "Table") = 0 Then
                n.StateImageIndex = 3
            ElseIf _sel_comps(i).ty.Substring(0, 5) = "Table" Then
                n.StateImageIndex = 5
            ElseIf _sel_comps(i).ty.Substring(0, 14) = "Chart in Table" Then
                n.StateImageIndex = 6
            ElseIf _sel_comps(i).ty.Substring(0, 14) = "Image in Table" Then
                n.StateImageIndex = 6
            End If

        Next
        Me.bok.Enabled = True
        Me.uxselcomps.EndUpdate()

        If Me.uxselcomps.Nodes.Count > 0 Then
            Me.uxselcomps.FocusedNode = Nothing
            Me.uxselcomps.FocusedNode = Me.uxselcomps.Nodes(0)
            Me.tinfo.Enabled = False
            If Me.bok.Visible = True Then
                Me.tinfo.Enabled = True
            End If
        Else
            Me.rtfpreview.RtfText = ""
            Me.tinfo.Text = ""
            Me.tinfo.Enabled = False
        End If




    End Sub

    Private Sub bc_am_dx_document_composition_DoubleClick(sender As Object, e As EventArgs) Handles uxcomps.DoubleClick

        Try
            Me.Cursor = Cursors.WaitCursor
            If Me.bok.Visible = False Then
                Exit Sub
            End If
            If Me.uxselcomps.Nodes.Count = 10 Then
                Dim omsg As New bc_cs_message("Blue Curve", "Maximum number of components selected (10)", bc_cs_message.MESSAGE, False, False, "yes", "no", True)
                Exit Sub
            End If

            Dim lvew As New ListViewItem


            For i = 0 To Me.uxselcomps.Nodes.Count - 1
                If Me.uxselcomps.Nodes(i).GetValue(3) = Me.uxcomps.Selection.Item(0).GetValue(3) And Me.uxselcomps.Nodes(i).GetValue(4) = Me.uxcomps.Selection.Item(0).GetValue(4) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Component Already Selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If
            Next

            Dim sco As New sel_comp
            sco.na = Me.uxcomps.Selection.Item(0).GetValue(0)
            sco.ty = Me.uxcomps.Selection.Item(0).GetValue(1)
            sco.pg = Me.uxcomps.Selection.Item(0).GetValue(2)
            sco.co = Me.uxcomps.Selection.Item(0).GetValue(3)
            sco.pa = Me.uxcomps.Selection.Item(0).GetValue(4)
            sco.table_locator = Me.uxcomps.Selection.Item(0).GetValue(5)

            _sel_comps.Add(sco)
            load_sel_comps()
            Me.uxselcomps.FocusedNode = Me.uxselcomps.Nodes(Me.uxselcomps.Nodes.Count - 1)




        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("uxselcomps", "Doubleclick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub bc_am_dx_document_composition_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If check_exit() = False Then
            e.Cancel = True
            Exit Sub
        End If
        close_doc()

    End Sub

    Private Sub uxselcomps_Click(sender As Object, e As EventArgs) Handles uxselcomps.Click
        get_sel(Nothing)
    End Sub

    Private Sub uxselcomps_DoubleClick(sender As Object, e As EventArgs) Handles uxselcomps.DoubleClick
        Try
            If Me.bok.Visible = False Then
                Exit Sub
            End If

            For i = 0 To Me.uxselcomps.Nodes.Count - 1
                If Me.uxselcomps.Nodes(i).Id = Me.uxselcomps.Selection.Item(0).Id Then
                    Me._sel_comps.RemoveAt(i)
                    Exit For
                End If
            Next
            load_sel_comps()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("uxselcomps", "Doubleclick", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub

    Private Sub uxselcomps_FocusedNodeChanged(sender As Object, e As FocusedNodeChangedEventArgs) Handles uxselcomps.FocusedNodeChanged
        get_sel(e)
    End Sub
    Private Sub get_sel(e As FocusedNodeChangedEventArgs)
        Try
            Cursor = Cursors.WaitCursor


            Me.tinfo.Enabled = True
            Me.breset.Enabled = False
            Me.bupdate.Enabled = False
            Dim idx As Integer = -1
            If Not IsNothing(e) AndAlso IsNothing(e.Node) Then
                Exit Sub
            End If

            For i = 0 To Me.uxselcomps.Nodes.Count - 1
                If Me.uxselcomps.Nodes(i).GetValue(3) = Me.uxselcomps.Selection.Item(0).GetValue(3) And Me.uxselcomps.Nodes(i).GetValue(4) = Me.uxselcomps.Selection.Item(0).GetValue(4) Then
                    idx = i
                    Exit For
                End If
            Next

            If idx = -1 Then
                Exit Sub
            End If
            Me.tinfo.Text = Me._sel_comps(idx).add_text

            If Me.bok.Visible = False Then
                Me.tinfo.Enabled = False
            End If

            If _new_comp = True Then
                load_preview_new(True)
                Return
            End If

            For i = 0 To _comps.components.Count - 1
                If _comps.components(i).component_id = Me.uxselcomps.Selection.Item(0).GetValue(3) Then
                    If Me.uxcomps.Selection.Item(0).GetValue(4) = "0" Then
                        If _comps.components(i).ortf <> "" Then
                            Me.rtfpreview.RtfText = _comps.components(i).ortf
                        Else
                            Me.rtfpreview.Text = _comps.components(i).text
                        End If
                        Exit Sub
                    Else
                        For j = 0 To _comps.components(i).paragraphs.Count - 1

                            If _comps.components(i).paragraphs(j).ord = Me.uxselcomps.Selection.Item(0).GetValue(4) Then
                                Me.rtfpreview.Visible = True
                                Me.ipreview.Visible = False

                                If _comps.components(i).paragraphs(j).ortf <> "" Then
                                    Me.rtfpreview.RtfText = _comps.components(i).paragraphs(j).ortf
                                Else
                                    Me.rtfpreview.Text = _comps.components(i).paragraphs(j).text
                                End If
                                If _comps.components(i).paragraphs(j).btable = True Then
                                    If _comps.components(i).paragraphs(j).table_locator <> "" Then
                                        Me.rtfpreview.Visible = False
                                        Me.ipreview.Visible = True
                                        get_Image(_comps.components(i).paragraphs(j).table_locator)
                                    End If
                                End If
                                Exit Sub
                            End If
                        Next
                    End If
                End If
            Next


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("uxselcomps", "FocusedNodeChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub bup_Click(sender As Object, e As EventArgs) Handles bup.Click

        Try
            If Me.uxselcomps.Nodes.Count = 0 Then
                Exit Sub
            End If
            Dim tln As Nodes.TreeListNode = Nothing
            Dim n As DevExpress.XtraTreeList.Nodes.TreeListNode

            Dim idx As Integer
            For i = 0 To Me.uxselcomps.Nodes.Count - 1
                If Me.uxselcomps.Nodes(i).GetValue(3) = Me.uxselcomps.Selection.Item(0).GetValue(3) And Me.uxselcomps.Nodes(i).GetValue(4) = Me.uxselcomps.Selection.Item(0).GetValue(4) Then
                    idx = i
                    Exit For
                End If
            Next

            If idx = 0 Then
                Exit Sub
            End If

            Dim sco As New sel_comp
            sco = _sel_comps(idx)
            _sel_comps.RemoveAt(idx)
            _sel_comps.Insert(idx - 1, sco)
            load_sel_comps()
            Me.uxselcomps.FocusedNode = Me.uxselcomps.Nodes(idx - 1)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Button3_Click", "Button3_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)


        End Try
    End Sub
    Private Sub bdn_Click(sender As Object, e As EventArgs) Handles bdn.Click

        Try
            If Me.uxselcomps.Nodes.Count = 0 Then
                Exit Sub
            End If
            Dim tln As Nodes.TreeListNode = Nothing
            Dim n As DevExpress.XtraTreeList.Nodes.TreeListNode

            Dim idx As Integer
            For i = 0 To Me.uxselcomps.Nodes.Count - 1
                If Me.uxselcomps.Nodes(i).GetValue(3) = Me.uxselcomps.Selection.Item(0).GetValue(3) And Me.uxselcomps.Nodes(i).GetValue(4) = Me.uxselcomps.Selection.Item(0).GetValue(4) Then
                    idx = i
                    Exit For
                End If
            Next

            If idx = Me.uxselcomps.Nodes.Count - 1 Then
                Exit Sub
            End If

            Dim sco As New sel_comp
            sco = _sel_comps(idx)
            _sel_comps.RemoveAt(idx)
            _sel_comps.Insert(idx + 1, sco)
            load_sel_comps()
            Me.uxselcomps.FocusedNode = Me.uxselcomps.Nodes(idx + 1)





        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Button3_Click", "Button3_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)


        End Try
    End Sub

    Private Sub tinfo_EditValueChanged(sender As Object, e As EventArgs) Handles tinfo.EditValueChanged
        Try
            If Me.uxselcomps.Nodes.Count = 0 Then
                Exit Sub
            End If
            Me.breset.Enabled = False
            Me.bupdate.Enabled = False

            Dim idx As Integer
            For i = 0 To Me.uxselcomps.Nodes.Count - 1
                If Me.uxselcomps.Nodes(i).GetValue(3) = Me.uxselcomps.Selection.Item(0).GetValue(3) And Me.uxselcomps.Nodes(i).GetValue(4) = Me.uxselcomps.Selection.Item(0).GetValue(4) Then
                    idx = i
                    Exit For
                End If
            Next
            If Me.tinfo.Text <> Me._sel_comps(idx).add_text Then
                Me.breset.Enabled = True
                Me.bupdate.Enabled = True
                Me.uxselcomps.Enabled = False
                Me.uxcomps.Enabled = False
                Me.bup.Enabled = False
                Me.bdn.Enabled = False
                Me.chkshowtext.Enabled = False

            End If

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Button3_Click", "tinfo_EditValueChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)


        End Try

    End Sub

    Private Sub bupdate_Click(sender As Object, e As EventArgs) Handles bupdate.Click
        Dim idx As Integer
        For i = 0 To Me.uxselcomps.Nodes.Count - 1
            If Me.uxselcomps.Nodes(i).GetValue(3) = Me.uxselcomps.Selection.Item(0).GetValue(3) And Me.uxselcomps.Nodes(i).GetValue(4) = Me.uxselcomps.Selection.Item(0).GetValue(4) Then
                idx = i
                Exit For
            End If
        Next
        Me._sel_comps(idx).add_text = Me.tinfo.Text
        Me.bupdate.Enabled = False
        Me.breset.Enabled = False
        Me.uxselcomps.Enabled = True
        Me.uxcomps.Enabled = True
        Me.bup.Enabled = True
        Me.bdn.Enabled = True
        Me.chkshowtext.Enabled = True
        Me.bok.Enabled = True
    End Sub
    Private Sub breset_Click(sender As Object, e As EventArgs) Handles breset.Click
        Dim idx As Integer
        For i = 0 To Me.uxselcomps.Nodes.Count - 1
            If Me.uxselcomps.Nodes(i).GetValue(3) = Me.uxselcomps.Selection.Item(0).GetValue(3) And Me.uxselcomps.Nodes(i).GetValue(4) = Me.uxselcomps.Selection.Item(0).GetValue(4) Then
                idx = i
                Exit For
            End If
        Next
        Me.tinfo.Text = Me._sel_comps(idx).add_text
        Me.bupdate.Enabled = False
        Me.breset.Enabled = False
        Me.uxselcomps.Enabled = True
        Me.uxcomps.Enabled = True
        Me.bup.Enabled = True
        Me.bdn.Enabled = True
        Me.chkshowtext.Enabled = True
        Me.bok.Enabled = True


    End Sub
End Class
REM MVP
Public Interface Ibc_am_document_composition
    Sub load_list(title As String, ByVal doc_id As Long, ByVal comps As bc_om_document_components, ByVal filename As String, Optional reload As Boolean = False, Optional read_only As Boolean = False)
    Sub load_list_new(title As String, ByVal doc_id As Long, ByVal comps As bc_om_document_components, ByVal filename As String, Optional reload As Boolean = False, Optional read_only As Boolean = False)
    Sub set_preview(ByVal fn As String)
    Event save(ByVal comps As bc_om_components_to_display)
    Event close()
    Event reload(ByVal bwithtext As Boolean)

End Interface

Public Class ctrll_bc_am_document_composition
    Private WithEvents _view As Ibc_am_document_composition
    Private _doc_id As Long
    Private _adoc As Object
    Private _filename As String
    Private _title As String
    Private _read_only As Boolean
    Private _table_on_demand As Boolean

    Public Sub New(ByVal doc_id As Long, ByVal view As Ibc_am_document_composition)
        _doc_id = doc_id
        _view = view
    End Sub
    Public Function load_data(title As String, Optional ByVal bwithtext As Boolean = False, Optional reload As Boolean = False, Optional read_only As Boolean = False) As Boolean
        Dim comps As New bc_om_document_components
        comps.doc_id = _doc_id
        comps.table_charts_only = Not bwithtext

        _title = title
        _read_only = read_only


        load_data = False
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            comps.db_read()
        Else
            comps.tmode = bc_cs_soap_base_class.tREAD
            If comps.transmit_to_server_and_receive(comps, True) = False Then
                Exit Function
            End If
        End If
        If comps.components.Count = 0 Then
                Exit Function
        End If
        _table_on_demand = comps.table_on_demand

        REM save document
        If reload = False AndAlso IsNothing(comps.byteDoc) = False Then
            _filename = bc_cs_central_settings.local_repos_path + CStr(_doc_id) + "_ctmp.docx"
            Dim fs As New bc_cs_file_transfer_services
            If fs.write_bytestream_to_document(_filename, comps.byteDoc, Nothing, True) = False Then
                Exit Function
            End If
        Else
            _filename = ""
        End If
        load_data = True
        If comps.table_on_demand = True Then
            _view.load_list(title, _doc_id, comps, _filename, reload, read_only)
        Else

            _view.load_list_new(title, _doc_id, comps, _filename, reload, read_only)
        End If
        
    End Function
    Public Sub save_composition(ByVal comps As bc_om_components_to_display) Handles _view.save
        comps.doc_id = _doc_id
        If _table_on_demand = False Then
            comps.generate_template = True
        End If
        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
            comps.db_write()
        Else
            comps.tmode = bc_cs_soap_base_class.tWRITE
            If comps.transmit_to_server_and_receive(comps, True) = False Then
                Exit Sub
            End If
        End If

    End Sub


    Public Sub reload(ByVal bwithtext As Boolean) Handles _view.reload
        load_data(_title, bwithtext, True, _read_only)

    End Sub

End Class