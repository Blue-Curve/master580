'Imports BlueCurve.Core.CS
'Imports BlueCurve.Core.OM
'Imports System.Windows.Forms
'Imports BlueCurve.Core.AS
'Imports System.Collections
'Imports System.Threading
'Imports System.Windows.Forms.Screen
'Imports System.IO
'Imports System.Text
'Imports System.Text.RegularExpressions



'Public Class bc_am_document_composition
'    Implements Ibc_am_document_composition
'    Dim _comps As bc_om_document_components
'    Dim _selected_comps As bc_om_document_components
'    Dim _doc_id As Long
'    Private _word As New bc_ao_word

'    Dim _filename As String
'    Private Sub get_Image(ByVal table_locator As String)
'        Try
'            Dim fn As String
'            fn = bc_cs_central_settings.local_repos_path + "img_" + CStr(_doc_id) + "_" + table_locator + ".png"
'            Dim fs As New bc_cs_file_transfer_services
'            If fs.check_document_exists(fn) = True Then
'                set_preview(fn)
'                Exit Sub
'            End If
'            If _word.get_table_as_image_from_bm(table_locator, fn, _doc_id) = False Then
'                Dim omsg As New bc_cs_message("Blue Curve", "Failed to Generate Image for Table", bc_cs_message.MESSAGE, False, False, "yes", "no", True)
'                Exit Sub
'            End If
'            set_preview(fn)

'        Catch ex As Exception
'            Dim oerr As New bc_cs_error_log("ctrll_bc_am_document_composition", "get_image", bc_cs_error_codes.USER_DEFINED, ex.Message)
'        End Try

'    End Sub

'    Public Sub load_list(ByVal doc_id As Long, ByVal comps As bc_om_document_components, ByVal filename As String, Optional reload As Boolean = False) Implements Ibc_am_document_composition.load_list
'        Try
'            Dim sel_items As New List(Of ListViewItem)
'            Dim sel_order As New List(Of Integer)
'            If Not IsNothing(comps) Then
'                _comps = comps
'            End If
'            Dim lvw As ListViewItem
'            Dim do_not_show As Boolean
'            Me.ListView1.Items.Clear()
'            Me.ListView2.Items.Clear()

'            _filename = filename

'            _doc_id = doc_id

'            For i = 0 To comps.components.Count - 1
'                do_not_show = False
'                lvw = New ListViewItem(Trim(comps.components(i).text))
'                If comps.components(i).text_box_id = 0 Then
'                    If comps.components(i).bimage = True Then
'                        lvw = New ListViewItem("Chart: Lead")
'                    End If

'                    If comps.components(i).btable = True Then
'                        If comps.components(i).chart_in_table = True Then
'                            lvw.SubItems.Add("Chart in Table: Lead")
'                        Else
'                            lvw.SubItems.Add("Table: Lead")
'                        End If
'                    ElseIf comps.components(i).bimage Then
'                        lvw.SubItems.Add("Chart: Lead")
'                    Else
'                        If Me.chkshowtext.Checked = False Then
'                            do_not_show = True
'                        End If
'                        lvw.SubItems.Add("Text: Lead")
'                    End If
'                    lvw.SubItems.Add(CStr(comps.components(i).page_no))

'                Else
'                    If comps.components(i).bimage = True Then
'                        lvw = New ListViewItem("Chart (Frame)")
'                    End If

'                    If comps.components(i).btable = True Then
'                        If comps.components(i).chart_in_table = True Then
'                            lvw.SubItems.Add("Chart in Table (Frame)")
'                        Else
'                            lvw.SubItems.Add("Table (Frame)")
'                        End If
'                    ElseIf comps.components(i).bimage Then
'                        lvw.SubItems.Add("Chart (Frame)")
'                    Else
'                        If Me.chkshowtext.Checked = False Then
'                            do_not_show = True
'                        End If
'                        lvw.SubItems.Add("Text (Frame)")
'                    End If
'                    lvw.SubItems.Add(CStr(comps.components(i).page_no) + " : " + CStr(comps.components(i).text_box_id))

'                End If


'                lvw.SubItems.Add(comps.components(i).component_id)
'                lvw.SubItems.Add("0")

'                If comps.components(i).display_order > 0 Then
'                    sel_items.Add(lvw)
'                    sel_order.Add(comps.components(i).display_order)
'                End If
'                If do_not_show = False Then
'                    Me.ListView1.Items.Add(lvw)
'                End If

'                For j = 0 To comps.components(i).paragraphs.Count - 1
'                    do_not_show = False
'                    lvw = New ListViewItem(comps.components(i).paragraphs(j).text)
'                    If comps.components(i).paragraphs(j).bimage = True Then
'                        lvw = New ListViewItem("Chart: Sub")
'                    End If
'                    If comps.components(i).paragraphs(j).btable = True Then
'                        If comps.components(i).paragraphs(j).chart_in_table = True Then
'                            lvw.SubItems.Add("Chart in Table: Sub")
'                        Else
'                            lvw.SubItems.Add("Table: Sub")
'                        End If
'                    ElseIf comps.components(i).paragraphs(j).bimage Then
'                        lvw.SubItems.Add("Chart: Sub")
'                    Else
'                        If Me.chkshowtext.Checked = False Then
'                            do_not_show = True
'                        End If
'                        lvw.SubItems.Add("Text: Sub")
'                    End If
'                    lvw.SubItems.Add(CStr(comps.components(i).paragraphs(j).page_no))

'                    lvw.SubItems.Add(comps.components(i).component_id)
'                    lvw.SubItems.Add(comps.components(i).paragraphs(j).ord)
'                    If do_not_show = False Then
'                        Me.ListView1.Items.Add(lvw)
'                    End If

'                    If comps.components(i).paragraphs(j).display_order > 0 Then
'                        sel_items.Add(lvw)
'                        sel_order.Add(comps.components(i).paragraphs(j).display_order)
'                    End If
'                Next
'            Next

'            REM load selected items
'            For i = 1 To sel_items.Count
'                For j = 1 To sel_items.Count
'                    If sel_order(j - 1) = i Then
'                        lvw = New ListViewItem(sel_items(j - 1).Text)
'                        lvw.SubItems.Add(sel_items(j - 1).SubItems(1))
'                        lvw.SubItems.Add(sel_items(j - 1).SubItems(2))
'                        lvw.SubItems.Add(sel_items(j - 1).SubItems(3))
'                        lvw.SubItems.Add(sel_items(j - 1).SubItems(4))
'                        Me.ListView2.Items.Add(lvw)
'                        Exit For
'                    End If
'                Next
'            Next
'            If reload = False Then
'                _word.open_new_instance(_filename, False)
'            End If




'        Catch ex As Exception
'            Dim oerr As New bc_cs_error_log("bc_am_document_compoisition", "load_list", bc_cs_error_codes.USER_DEFINED, ex.Message)


'        End Try
'    End Sub

'    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
'        If check_exit() = False Then
'            Exit Sub
'        End If
'        close_doc()

'        RaiseEvent xclose()
'        Me.Hide()
'    End Sub
'    Public Sub set_preview(ByVal fn As String) Implements Ibc_am_document_composition.set_preview

'        REM TBD

'        Me.ipreview.Visible = True



'        ipreview.Image = System.Drawing.Image.FromFile(fn)




'    End Sub
'    Private Sub close_doc()
'        If IsNothing(_word) = False Then
'            _word.close(False)
'            _word.quit()
'            Dim fs As New bc_cs_file_transfer_services
'            fs.delete_file(_filename, Nothing, False)
'        End If
'    End Sub
'    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
'        Try
'            Dim cdisps As New bc_om_components_to_display
'            Dim cdisp As bc_om_component_to_display


'            close_doc()

'            For j = 0 To Me.ListView2.Items.Count - 1
'                cdisp = New bc_om_component_to_display
'                cdisp.component_id = Me.ListView2.Items(j).SubItems(3).Text
'                cdisp.ord = Me.ListView2.Items(j).SubItems(4).Text
'                cdisp.display_order = j + 1
'                For i = 0 To _comps.components.Count - 1
'                    If _comps.components(i).component_id = Me.ListView2.Items(j).SubItems(3).Text And Me.ListView2.Items(j).SubItems(4).Text = 0 Then
'                        cdisp.desc = _comps.components(i).description
'                        Exit For
'                    ElseIf Me.ListView2.Items(j).SubItems(3).Text <> "0" Then
'                        For k = 0 To _comps.components(i).paragraphs.Count - 1
'                            If _comps.components(i).component_id = Me.ListView2.Items(j).SubItems(3).Text And _
'                            _comps.components(i).paragraphs(k).ord = Me.ListView2.Items(j).SubItems(4).Text Then
'                                cdisp.desc = _comps.components(i).paragraphs(k).description
'                                If _comps.components(i).paragraphs(k).table_locator <> "" Then
'                                    cdisp.image_name = "img_" + CStr(_comps.doc_id) + "_" + _comps.components(i).paragraphs(k).table_locator + ".png"
'                                    Dim fs As New bc_cs_file_transfer_services
'                                    If fs.check_document_exists(bc_cs_central_settings.local_repos_path + cdisp.image_name) = True Then
'                                        fs.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + cdisp.image_name, cdisp.bimage, Nothing, True)
'                                        fs.delete_file(bc_cs_central_settings.local_repos_path + cdisp.image_name)
'                                    End If
'                                End If
'                                Exit For
'                            End If
'                        Next
'                    End If
'                Next
'                cdisps.components.Add(cdisp)
'            Next
'            Me.Hide()
'            ipreview.Image = Nothing




'            RaiseEvent save(cdisps)
'            Me.Hide()
'        Catch ex As Exception
'            'Dim oerr As New bc_cs_error_log("save", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)


'        End Try
'    End Sub

'    Public Event save(ByVal comps As bc_om_components_to_display) Implements Ibc_am_document_composition.save
'    Public Event xclose() Implements Ibc_am_document_composition.close
'    Public Event reload(ByVal bwithtext As Boolean) Implements Ibc_am_document_composition.reload

'    Private Sub Label4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label4.Click

'    End Sub

'    Private Sub bc_am_document_composition_FormClosing(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles Me.FormClosing
'        If check_exit() = False Then
'            e.Cancel = True

'        End If
'    End Sub

'    Private Sub bc_am_document_composition_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

'    End Sub

'    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.DoubleClick

'        Try
'            If Me.Button1.Visible = False Then
'                Exit Sub

'            End If
'            If Me.ListView2.Items.Count = 10 Then
'                Dim omsg As New bc_cs_message("Blue Curve", "Maximum number of components selected (10)", bc_cs_message.MESSAGE, False, False, "yes", "no", True)
'                Exit Sub
'            End If

'            Dim lvew As New ListViewItem
'            If Me.ListView1.SelectedItems.Count <> 1 Then
'                Exit Sub
'            End If

'            For i = 0 To Me.ListView2.Items.Count - 1
'                If Me.ListView2.Items(i).SubItems(3).Text = Me.ListView1.SelectedItems(0).SubItems(3).Text And Me.ListView2.Items(i).SubItems(4).Text = Me.ListView1.SelectedItems(0).SubItems(4).Text Then
'                    Dim omsg As New bc_cs_message("Blue Curve", "Component Already Selected", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
'                    Exit Sub
'                End If
'            Next

'            If Me.ListView1.SelectedItems.Count = 1 Then

'                For i = 0 To _comps.components.Count - 1
'                    If _comps.components(i).component_id = Me.ListView1.SelectedItems(0).SubItems(3).Text Then
'                        If Me.ListView1.SelectedItems(0).SubItems(4).Text = "0" Then
'                            lvew = New ListViewItem(Me.ListView1.SelectedItems(0).Text)
'                            lvew.SubItems.Add(Me.ListView1.SelectedItems(0).SubItems(1).Text)
'                            lvew.SubItems.Add(Me.ListView1.SelectedItems(0).SubItems(2).Text)
'                            lvew.SubItems.Add(Me.ListView1.SelectedItems(0).SubItems(3).Text)
'                            lvew.SubItems.Add(Me.ListView1.SelectedItems(0).SubItems(4).Text)
'                            Me.ListView2.Items.Add(lvew)
'                            Exit Sub
'                        Else
'                            For j = 0 To _comps.components(i).paragraphs.Count - 1
'                                If _comps.components(i).paragraphs(j).ord = Me.ListView1.SelectedItems(0).SubItems(4).Text Then
'                                    lvew = New ListViewItem(Me.ListView1.SelectedItems(0).Text)
'                                    lvew.SubItems.Add(Me.ListView1.SelectedItems(0).SubItems(1).Text)
'                                    lvew.SubItems.Add(Me.ListView1.SelectedItems(0).SubItems(2).Text)
'                                    lvew.SubItems.Add(Me.ListView1.SelectedItems(0).SubItems(3).Text)
'                                    lvew.SubItems.Add(Me.ListView1.SelectedItems(0).SubItems(4).Text)
'                                    Me.ListView2.Items.Add(lvew)
'                                    Exit Sub
'                                End If
'                            Next
'                        End If
'                    End If
'                Next
'            End If

'        Catch ex As Exception
'            Dim oerr As New bc_cs_error_log("ListView1", "Doubleclick", bc_cs_error_codes.USER_DEFINED, ex.Message)


'        End Try
'    End Sub

'    Private Sub Label5_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label5.Click

'    End Sub

'    Private Sub ListView1_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
'        Me.Cursor = Cursors.WaitCursor
'        Try

'            If Me.ListView1.SelectedItems.Count = 1 Then
'                Me.rtfpreview.Clear()

'                For i = 0 To _comps.components.Count - 1
'                    If _comps.components(i).component_id = Me.ListView1.SelectedItems(0).SubItems(3).Text Then
'                        If Me.ListView1.SelectedItems(0).SubItems(4).Text = "0" Then
'                            If _comps.components(i).ortf <> "" Then
'                                Me.rtfpreview.Rtf = _comps.components(i).ortf
'                            Else
'                                Me.rtfpreview.Text = _comps.components(i).text

'                            End If
'                            Exit Sub
'                        Else
'                            For j = 0 To _comps.components(i).paragraphs.Count - 1

'                                If _comps.components(i).paragraphs(j).ord = Me.ListView1.SelectedItems(0).SubItems(4).Text Then
'                                    Me.rtfpreview.Visible = True
'                                    Me.ipreview.Visible = False

'                                    If _comps.components(i).paragraphs(j).ortf <> "" Then
'                                        Me.rtfpreview.Rtf = _comps.components(i).paragraphs(j).ortf
'                                    Else
'                                        Me.rtfpreview.Text = _comps.components(i).paragraphs(j).text
'                                    End If
'                                    If _comps.components(i).paragraphs(j).btable = True Then
'                                        If _comps.components(i).paragraphs(j).table_locator <> "" Then
'                                            Me.rtfpreview.Visible = False
'                                            Me.ipreview.Visible = True
'                                            get_Image(_comps.components(i).paragraphs(j).table_locator)
'                                        End If
'                                    End If
'                                    Exit Sub
'                                End If
'                            Next
'                        End If
'                    End If
'                Next
'            End If
'        Catch ex As Exception

'            'Dim oerr As New bc_cs_error_log("ListView1", "ListView1_SelectedIndexChanged_1", bc_cs_error_codes.USER_DEFINED, ex.Message)

'        Finally
'            Me.Cursor = Cursors.Default

'        End Try
'    End Sub

'    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bup.Click
'        If Me.ListView2.SelectedItems(0).Index = 0 Then
'            Exit Sub

'        End If

'        Try
'            If Me.ListView2.SelectedItems.Count = 1 Then
'                Dim idx As Integer
'                idx = Me.ListView2.SelectedItems(0).Index
'                Dim lvw As New ListViewItem(Me.ListView2.SelectedItems(0).Text)
'                lvw.SubItems.Add(Me.ListView2.SelectedItems(0).SubItems(1).Text)
'                lvw.SubItems.Add(Me.ListView2.SelectedItems(0).SubItems(2).Text)
'                lvw.SubItems.Add(Me.ListView2.SelectedItems(0).SubItems(3).Text)
'                lvw.SubItems.Add(Me.ListView2.SelectedItems(0).SubItems(4).Text)
'                Me.ListView2.Items.Insert(Me.ListView2.SelectedItems(0).Index - 1, lvw)
'                Me.ListView2.Items.RemoveAt(Me.ListView2.SelectedItems(0).Index)
'                Me.ListView2.Items(idx - 1).Selected = True
'            End If
'            If Me.ListView2.SelectedItems(0).Index > 0 Then
'                Me.bup.Enabled = True
'                load_desc()
'            End If

'            If Me.ListView2.SelectedItems(0).Index < Me.ListView2.Items.Count - 1 Then
'                Me.bdn.Enabled = True
'                load_desc()
'            End If
'        Catch ex As Exception
'            Dim oerr As New bc_cs_error_log("Button3_Click", "Button3_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)


'        End Try
'    End Sub

'    Private Sub ListView2_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles ListView2.DoubleClick
'        If Me.Button1.Visible = False Then
'            Exit Sub

'        End If
'        If Me.ListView2.SelectedItems.Count = 1 Then
'            Me.ListView2.Items.RemoveAt(Me.ListView2.SelectedItems(0).Index)
'            Me.tinfo.Text = ""
'            Me.tinfo.Enabled = False

'        End If
'    End Sub

'    Private Sub ListView2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView2.SelectedIndexChanged

'        Me.Cursor = Cursors.WaitCursor


'        Try
'            Me.bup.Enabled = False
'            Me.bdn.Enabled = False
'            Me.tinfo.Text = ""
'            If Me.ListView2.SelectedItems.Count = 0 Then
'                Me.tinfo.Enabled = False
'                Exit Sub
'            End If
'            Me.tinfo.Enabled = True
'            If Me.ListView2.SelectedItems(0).Index > 0 Then
'                Me.bup.Enabled = True

'            End If

'            If Me.ListView2.SelectedItems(0).Index < Me.ListView2.Items.Count - 1 Then
'                Me.bdn.Enabled = True

'            End If
'            load_desc()
'            Me.rtfpreview.Clear()

'            For i = 0 To _comps.components.Count - 1
'                If _comps.components(i).component_id = Me.ListView2.SelectedItems(0).SubItems(3).Text Then
'                    If Me.ListView2.SelectedItems(0).SubItems(4).Text = "0" Then
'                        If _comps.components(i).ortf = "" Then
'                            Me.rtfpreview.Text = _comps.components(i).text
'                        Else
'                            Me.rtfpreview.Rtf = _comps.components(i).ortf
'                        End If
'                        Exit Sub
'                    Else
'                        For j = 0 To _comps.components(i).paragraphs.Count - 1
'                            If _comps.components(i).paragraphs(j).ord = Me.ListView2.SelectedItems(0).SubItems(4).Text Then
'                                If _comps.components(i).paragraphs(j).ortf = "" Then
'                                    Me.rtfpreview.Text = _comps.components(i).paragraphs(j).text
'                                Else
'                                    Me.rtfpreview.Rtf = _comps.components(i).paragraphs(j).ortf
'                                End If
'                                If _comps.components(i).paragraphs(j).btable = True Then
'                                    If _comps.components(i).paragraphs(j).table_locator <> "" Then
'                                        Me.rtfpreview.Visible = False
'                                        Me.ipreview.Visible = True
'                                        get_Image(_comps.components(i).paragraphs(j).table_locator)
'                                    End If
'                                End If
'                                Exit Sub
'                            End If
'                        Next
'                    End If
'                End If
'            Next

'        Catch ex As Exception

'            'Dim oerr As New bc_cs_error_log("ListView2", "ListView1_SelectedIndexChanged_1", bc_cs_error_codes.USER_DEFINED, ex.Message)

'        Finally
'            Me.Cursor = Cursors.Default

'        End Try

'    End Sub
'    Dim current_desc As String
'    Private Sub load_desc()
'        Me.tinfo.Text = ""
'        For i = 0 To _comps.components.Count - 1
'            If _comps.components(i).component_id = Me.ListView2.SelectedItems(0).SubItems(3).Text And Me.ListView2.SelectedItems(0).SubItems(4).Text = 0 Then
'                current_desc = _comps.components(i).description
'                Me.tinfo.Text = _comps.components(i).description
'                Me.tinfo.Enabled = True
'                Exit Sub
'            ElseIf Me.ListView2.SelectedItems(0).SubItems(3).Text <> "0" Then
'                For j = 0 To _comps.components(i).paragraphs.Count - 1
'                    If _comps.components(i).component_id = Me.ListView2.SelectedItems(0).SubItems(3).Text And _
'                    _comps.components(i).paragraphs(j).ord = Me.ListView2.SelectedItems(0).SubItems(4).Text Then
'                        current_desc = _comps.components(i).paragraphs(j).description

'                        Me.tinfo.Text = _comps.components(i).paragraphs(j).description
'                        Me.tinfo.Enabled = True
'                        Exit Sub
'                    End If
'                Next
'            End If
'        Next

'    End Sub

'    Private Sub bdn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdn.Click
'        If Me.ListView2.SelectedItems(0).Index = Me.ListView2.Items.Count - 1 Then
'            Exit Sub
'        End If
'        Try
'            If Me.ListView2.SelectedItems.Count = 1 Then
'                Dim idx As Integer
'                idx = Me.ListView2.SelectedItems(0).Index
'                Dim lvw As New ListViewItem(Me.ListView2.SelectedItems(0).Text)
'                lvw.SubItems.Add(Me.ListView2.SelectedItems(0).SubItems(1).Text)
'                lvw.SubItems.Add(Me.ListView2.SelectedItems(0).SubItems(2).Text)
'                lvw.SubItems.Add(Me.ListView2.SelectedItems(0).SubItems(3).Text)
'                lvw.SubItems.Add(Me.ListView2.SelectedItems(0).SubItems(4).Text)
'                Me.ListView2.Items.RemoveAt(Me.ListView2.SelectedItems(0).Index)
'                Me.ListView2.Items.Insert(idx + 1, lvw)
'                Me.ListView2.Items(idx + 1).Selected = True
'            End If
'            If Me.ListView2.SelectedItems(0).Index > 0 Then
'                Me.bup.Enabled = True
'                load_desc()
'            End If

'            If Me.ListView2.SelectedItems(0).Index < Me.ListView2.Items.Count - 1 Then
'                Me.bdn.Enabled = True
'                load_desc()
'            End If
'        Catch

'        End Try

'    End Sub

'    Private Sub Button3_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
'        If Me.ListView2.SelectedItems.Count = 0 Then
'            Exit Sub
'        End If
'        For i = 0 To _comps.components.Count - 1
'            If _comps.components(i).component_id = Me.ListView2.SelectedItems(0).SubItems(3).Text And Me.ListView2.SelectedItems(0).SubItems(4).Text = 0 Then
'                _comps.components(i).description = Me.tinfo.Text
'                Exit For
'            ElseIf Me.ListView2.SelectedItems(0).SubItems(3).Text <> "0" Then
'                For j = 0 To _comps.components(i).paragraphs.Count - 1
'                    If _comps.components(i).component_id = Me.ListView2.SelectedItems(0).SubItems(3).Text And _
'                    _comps.components(i).paragraphs(j).ord = Me.ListView2.SelectedItems(0).SubItems(4).Text Then
'                        _comps.components(i).paragraphs(j).description = Me.tinfo.Text
'                        Exit For
'                    End If
'                Next
'            End If
'        Next
'        Me.bup.Enabled = True
'        Me.bdn.Enabled = True
'        Me.Button1.Enabled = True
'        Me.ListView1.Enabled = True
'        Me.ListView2.Enabled = True
'        Me.Button3.Enabled = False
'        Me.Button4.Enabled = False
'    End Sub

'    Private Sub chkshowtext_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles chkshowtext.CheckedChanged
'        RaiseEvent reload(Me.chkshowtext.Checked)

'    End Sub

'    Private Sub tinfo_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tinfo.TextChanged
'        If Me.tinfo.Text = "" Or Me.tinfo.Text = current_desc Then
'            Me.Button3.Enabled = False
'            Me.Button4.Enabled = False
'            Me.ListView2.Enabled = True
'            Me.ListView1.Enabled = True
'            Me.Button1.Enabled = True
'            Me.bup.Enabled = False
'            Me.bdn.Enabled = False
'            If Me.ListView2.SelectedItems(0).Index > 0 Then
'                Me.bup.Enabled = True
'            End If

'            If Me.ListView2.SelectedItems(0).Index < Me.ListView2.Items.Count - 1 Then
'                Me.bdn.Enabled = True
'            End If
'        Else
'            Me.ListView2.Enabled = False
'            Me.ListView1.Enabled = False
'            Me.Button3.Enabled = True
'            Me.Button4.Enabled = True
'            Me.Button1.Enabled = False
'            Me.bup.Enabled = False
'            Me.bdn.Enabled = False
'        End If

'    End Sub
'    Private Function check_exit() As Boolean
'        check_exit = True
'        If Me.Button3.Enabled = True Then
'            Dim omsg As New bc_cs_message("Blue Curve", "You have unsaved additional text please update or reset first", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
'            check_exit = False
'        End If

'    End Function
'    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
'        Me.tinfo.Text = current_desc
'        Me.bup.Enabled = True
'        Me.bdn.Enabled = True
'        Me.Button1.Enabled = True
'        Me.ListView1.Enabled = True
'        Me.ListView2.Enabled = True
'        Me.Button3.Enabled = False
'        Me.Button4.Enabled = False
'        Me.tinfo.Text = current_desc

'    End Sub

'    Private Sub Label1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Label1.Click

'    End Sub
'End Class

