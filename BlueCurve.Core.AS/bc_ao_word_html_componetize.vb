Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Windows.Forms
Imports System.Threading
Imports Microsoft.VisualBasic.Devices
Module hmclipboard
    <DllImport("user32.dll", SetLastError:=True, CharSet:=CharSet.Unicode)> _
    Private Function GetClipboardData(ByVal uFormat As UInteger) As IntPtr
    End Function

    <DllImport("user32.dll", SetLastError:=True)> _
    Private Function OpenClipboard(ByVal hWndNewOwner As IntPtr) As Boolean
    End Function

    <DllImport("user32.dll", SetLastError:=True)> _
    Private Function CloseClipboard() As Boolean

    End Function
    <DllImport("user32.dll", SetLastError:=True)> _
    Private Function RegisterClipboardFormatA(ByVal lpszFormat As String) As UInteger
    End Function
    <DllImport("user32.dll", SetLastError:=True)> _
    Private Function IsClipboardFormatAvailable(ByVal format As UInteger) As Boolean
    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Private Function GlobalLock(ByVal hMem As IntPtr) As IntPtr

    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Private Function GlobalSize(ByVal hMem As IntPtr) As UInteger

    End Function

    <DllImport("kernel32.dll", SetLastError:=True)> _
    Private Function GlobalUnlock(ByVal hMem As IntPtr) As IntPtr
    End Function
    Public Function GetClipboard() As String
        Dim CF_HTML As UInteger = RegisterClipboardFormatA("HTML Format")
        If (IsClipboardFormatAvailable(CF_HTML)) Then
            If (OpenClipboard(Nothing)) Then
                Dim hGMem As IntPtr = GetClipboardData(CF_HTML)
                Dim pMFP = GlobalLock(hGMem)
                Dim Len As UInteger = GlobalSize(hGMem)
                Dim bytes(Len) As Byte
                Marshal.Copy(pMFP, bytes, 0, Len)
                Dim strMFP As String = System.Text.Encoding.UTF8.GetString(bytes)

                GlobalUnlock(hGMem)
                CloseClipboard()
                Return strMFP
            End If
        End If
        GetClipboard = Nothing
    End Function
    'Sub Main()
    '    Dim html As String = GetClipboard()
    '    If html Is Nothing Then
    '        Console.WriteLine("Nothing in clippy!")
    '    Else
    '        Console.WriteLine(html)
    '    End If
    '    Console.ReadLine()
    'End Sub
End Module

Public Class bc_ao_word_html_componetize
    Public worddocument As Object
    Public componetize_error_text As String
    Public overall_failure As Boolean = False

    Public Sub New(ao_object As Object)
        worddocument = ao_object

    End Sub

    Public Function get_property_value(ByVal property_name As String) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_html_componeizer", "get_property_value", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            get_property_value = worddocument.customdocumentproperties(property_name).value()
        Catch ex As Exception
            get_property_value = "Error"
            Dim ocommentary As New bc_cs_activity_log("bc_ao_html_componeizer", "get_property_value", bc_cs_activity_codes.COMMENTARY, "Failed to set property: " + property_name, Nothing)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_html_componeizer", "get_property_value", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Function bc_ax_html_componetize(ByRef ocomponents As bc_om_document_components, ByVal componetize_styles As bc_om_html_componetize_settings_for_template) As Boolean
        REM pass through to VBA
        Try
            bc_ax_html_componetize = worddocument.bc_ax_html_componetize(ocomponents, componetize_styles)
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "bc_ax_style_componetize", bc_cs_activity_codes.COMMENTARY, ex.Message, Nothing)
            bc_ax_html_componetize = False
        End Try

    End Function
    Public Function componetize(ByRef ofcomponents As bc_om_document_components, ByVal componetize_settings As bc_om_html_componetize_settings_for_template) As Boolean
        Try
            Dim ocomm As New bc_cs_activity_log("bc_ao_html_componeizer", "componetize", bc_cs_activity_codes.TRACE_ENTRY, "")

            Dim ocomponent As bc_om_style_component = Nothing



            Dim p As Object
            ofcomponents = New bc_om_document_components

            'Dim table_start As Boolean = False
            Dim working_table_id As Integer = 1

            REM parse paragrahs
            Dim cp As New bc_dx_ao_component_swap

            Dim thtml As String = ""
            Try

                worddocument.bookmarks("bctmptable").delete()
            Catch

            End Try



            If componetize_settings.s_search_main_body = True Then

                ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Starting Main Body Paragraphs Count: " + CStr(worddocument.paragraphs.count))
                'Dim allparas As Object
                'allparas = worddocument.paragraphs
                'For Each p In allparas
                For i = 1 To worddocument.paragraphs.count


                    ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "paragraph: " + CStr(i))

                    Try


                        REM TBD images in body
                        p = worddocument.paragraphs(i).range

                        If componetize_settings.s_pages_up_to > 0 AndAlso p.Information(3) > componetize_settings.s_pages_up_to Then
                            Exit For

                        End If

                        If p.tables.count = 0 Then
                            'table_start = False
                            ocomponent = New bc_om_style_component
                            ocomponent.page_no = p.Information(3)


                            If componetize_settings.s_pages_up_to > 0 AndAlso ocomponent.page_no > componetize_settings.s_pages_up_to Then
                                Continue For
                            End If

                            If p.inlineshapes.count = 1 And p.text.length <= 2 And componetize_settings.s_images = True Then
                                Try
                                    ocomponent.style = p.Style.namelocal
                                Catch
                                    ocomponent.style = "could not set namelocal"
                                End Try
                                ocomponent.image = get_image_from_inline_shape(p.inlineshapes(1))
                                If IsNothing(ocomponent.image) Then
                                    ocomm = New bc_cs_activity_log("bc_ao_word_html_componetize", "componetize", bc_cs_activity_codes.COMMENTARY, "Nothing Inline shape paragraph: " + CStr(i))
                                    overall_failure = True
                                End If
                                ocomponent.bimage = True
                                ocomponent.text = "image"
                                ofcomponents.components.Add(ocomponent)
                            ElseIf componetize_settings.s_text = True And p.text.length > 2 Then

                                If componetize_settings.s_text_as_html_body = True And p.inlineshapes.count = 0 Then
                                    paragraph_as_html(cp, p, thtml)
                                    ocomponent.text = tidy_up_text(p.text)
                                    Try
                                        ocomponent.style = p.Style.namelocal
                                    Catch
                                        ocomponent.style = "could not read namelocal"
                                    End Try
                                    Dim pst, ptb As String
                                    ocomponent.is_html = True

                                    Dim s, e As Integer
                                    s = InStr(thtml, "<style")
                                    e = InStr(thtml, "</style>")
                                    pst = thtml.Substring(s - 1, e - s + 8)
                                    ocomponent.css = pst
                                    s = InStr(thtml, "<body")
                                    e = InStr(thtml, "</body>")
                                    ptb = thtml.Substring(s + 5, e - s - 6)
                                    ocomponent.html = ptb
                                    ofcomponents.components.Add(ocomponent)
                                Else
                                    REM if shaperanges anchored to paragrpah 
                                    REM take as text
                                    Try
                                        ocomponent.style = p.Style.namelocal
                                    Catch
                                        ocomponent.style = "could not read namelocal"
                                    End Try
                                    ocomponent.text = tidy_up_text(p.text)
                                    If p.Hyperlinks.Count > 0 Then
                                        ocomponent.text = parse_hyperlinks(p, ocomponent.text, i)
                                    End If
                                    ofcomponents.components.Add(ocomponent)
                                End If
                            End If


                            'ElseIf p.tables.count = 1 And table_start = False Then
                        ElseIf p.tables.count = 1 AndAlso has_table_bookmark(p.tables(1).range) = False Then

                            p.tables(1).range.bookmarks.add("bctmptable")
                            REM image in table

                            If componetize_settings.s_tables = True Then

                                ocomponent = New bc_om_style_component
                                ocomponent.page_no = p.Information(3)
                                Try
                                    ocomponent.style = p.tables(1).Cell(1, 1).Range.Style.NameLocal
                                Catch
                                    ocomponent.style = "could not read namelocal"
                                End Try
                                REM take width
                                Try
                                    ocomponent.width = p.Tables(1).Range.Cells(1).Width

                                Catch ex As Exception
                                    ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Couldnt get table width:" + CStr(i) + ": " + ex.Message)


                                End Try

                                Try
                                    ocomponent.location = p.Tables(1).Range.Rows.HorizontalPosition()

                                Catch ex As Exception
                                    ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Couldnt get table postion:" + CStr(i) + ": " + ex.Message)

                                End Try

                                ocomponent.text = p.tables(1).Cell(1, 1).Range.text
                                ocomponent.btable = True
                                If p.tables(1).Range.InlineShapes.Count > 0 Then
                                    ocomponent.chart_in_table = True
                                End If
                                ofcomponents.components.Add(ocomponent)
                            End If
                            i = i + p.Tables(1).Range.Paragraphs.Count - 1

                        End If

                    Catch ex As Exception
                        ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Couldnt compontize paragraph: " + CStr(i) + ": " + ex.Message)
                    End Try
                Next
                ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "End Pargraphs Count")

                REM parse tables
                Dim alltables As Object
                Dim rtable As List(Of bc_om_style_component_table_cell)

                alltables = worddocument.tables
                Dim tcount As Integer
                Dim rtablesall As New List(Of List(Of bc_om_style_component_table_cell))
                Dim rtablesallhtml As New List(Of String)
                Dim rtablesallcell As New List(Of List(Of bc_om_style_component_table_cell))
                tcount = 0

                If componetize_settings.s_tables = True Then
                    ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Start Tables Count: " + CStr(worddocument.tables.count))
                    For Each t In alltables

                        If componetize_settings.s_pages_up_to > 0 AndAlso t.range.Information(3) > componetize_settings.s_pages_up_to Then
                            Exit For

                        End If

                        ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Table: " + CStr(tcount + 1))

                        'ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "table: " + CStr(tcount))
                        tcount = tcount + 1
                        REM if image in table parse as table cells not html
                        If t.Range.InlineShapes.Count > 0 Or componetize_settings.s_table_as_html_body = False Then
                            rtable = New List(Of bc_om_style_component_table_cell)
                            If parse_table(tcount, t.range, rtable, componetize_settings.s_images) = True Then
                                rtablesallcell.Add(rtable)
                            Else
                                rtable.Clear()
                                rtablesallcell.Add(rtable)
                            End If
                        Else
                            ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Table: " + CStr(tcount + 1) + " as HTML")
                            If table_as_html(tcount, cp, t.range, thtml) = True Then
                                rtablesallhtml.Add(thtml)
                            Else
                                rtablesallhtml.Add("")
                            End If
                        End If
                    Next
                    ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "End Tables")

                    REM assign tables to correct component
                    Dim htcount, ttcount As Integer
                    htcount = 0
                    ttcount = 0
                    For i = 0 To ofcomponents.components.Count - 1
                        If ofcomponents.components(i).btable = True Then

                            If ofcomponents.components(i).chart_in_table = False And componetize_settings.s_table_as_html_body = True Then
                                Try
                                    Dim tx, st, tb As String
                                    tx = rtablesallhtml(htcount)
                                    Dim s, e As Integer
                                    s = InStr(tx, "<style")
                                    e = InStr(tx, "</style>")
                                    st = tx.Substring(s - 1, e - s + 8)
                                    ofcomponents.components(i).is_html = True

                                    ofcomponents.components(i).css = st
                                    REM PR handke tables in table
                                    s = InStr(tx, "<table")
                                    REM e = InStr(tx, "</table>")
                                    e = InStrRev(tx, "</table>")

                                    tb = tx.Substring(s - 1, e - s + 8)
                                    ofcomponents.components(i).html = tb
                                    htcount = htcount + 1
                                Catch ex As Exception
                                    ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Failed to assign html table: " + CStr(htcount) + " : " + ex.Message)
                                End Try
                            Else
                                ofcomponents.components(i).table_cells = rtablesallcell(ttcount)
                                ttcount = ttcount + 1
                            End If
                        End If
                    Next
                    ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "End Table Assinment To Component.")
                End If
                ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "End Main Body.")
            End If

            If componetize_settings.s_search_text_boxes Then
                ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Start Text Boxes.")

                Dim i As Integer = 1
                Dim width As String
                For Each shp In worddocument.Shapes


                    If shp.Type = 17 Then
                        If componetize_settings.s_pages_up_to > 0 AndAlso shp.TextFrame.TextRange.Information(3) > componetize_settings.s_pages_up_to Then
                            Continue For
                        End If
                        width = ""
                        For j = 1 To shp.TextFrame.TextRange.Paragraphs.count
                            Try
                                width = shp.width
                            Catch ex As Exception
                                Dim ocomms As New bc_cs_activity_log("bc_ao_word_html_componetize", "componetize", bc_cs_activity_codes.COMMENTARY, "cant get width: " + ex.Message)
                            End Try
                            parse_paragraph_in_text_box(shp.TextFrame.TextRange.Paragraphs(j).range, i, j, ofcomponents, componetize_settings, cp, width)
                            'If componetize_error = True Then
                            '    componetize_error_text = componetize_error_text + " shape: " + CStr(j) + " paragraph: " + CStr(i)
                            '    Exit Function
                            'End If
                        Next
                        i = i + 1
                    End If
                Next shp

                ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "End Text Boxes.")
            End If


            If overall_failure = False Then


                componetize = True
            End If


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_word", "componetize", bc_cs_error_codes.USER_DEFINED, ex.Message)
            componetize = False
        Finally

            Dim ocomm As New bc_cs_activity_log("bc_ao_html_componeizer", "componetize", bc_cs_activity_codes.TRACE_EXIT, "")

            


        End Try
    End Function
    Function has_table_bookmark(p As Object) As Boolean

        Dim bm As String
        has_table_bookmark = False
        For i = 1 To p.bookmarks.count
            bm = p.bookmarks(i).name
            If bm = "bctmptable" Then
                has_table_bookmark = True
                Exit For
            End If
        Next

    End Function

    Function table_as_html(tid As Integer, cp As bc_dx_ao_component_swap, table As Object, ByRef html As String)
        Try
            table_as_html = False
            cp.RichEditControl1.ResetText()
            table_as_html = ""
            table.select()
            table.copy()
            cp.RichEditControl1.Paste()
            html = cp.RichEditControl1.HtmlText
            table_as_html = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_word", "table_as_html", bc_cs_error_codes.USER_DEFINED, "Failed to get table as html: " + CStr(tid) + " :" + ex.Message)
        End Try
    End Function
    Function paragraph_as_html(cp As bc_dx_ao_component_swap, para As Object, ByRef html As String)
        Try
            cp.RichEditControl1.ResetText()
            paragraph_as_html = ""

            para.select()
            para.copy()
            cp.RichEditControl1.Paste()


            html = cp.RichEditControl1.HtmlText

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_word", "table_as_html", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Private Function parse_hyperlinks(para As Object, tx As String, paragraph_number As Integer) As String
        Dim itx As String
        itx = tx
        Try
            Dim hwords(100) As Integer
            Dim hwc As Integer

            Dim cc As Integer
            cc = 1
            hwc = 0

            For i = 1 To para.Words.Count
                Try

                    If para.Words(i).Fields(1).Type = 88 Then
                        hwc = hwc + 1
                        hwords(hwc) = cc
                    End If
                    cc = cc + para.Words(i).text.length
                Catch
                    cc = cc + para.Words(i).text.length
                End Try
            Next

            Dim td, hy, nt As String
            Dim hco As Integer
            hco = para.Hyperlinks.Count
            For i = 1 To para.Hyperlinks.Count
                td = para.Hyperlinks(hco - i + 1).TextToDisplay
                hy = para.Hyperlinks(hco - i + 1).Address
                nt = "<a href='" + hy + "' class='whyread_link'>" + td + "</a>"
                Dim st As String
                st = tx.Substring(0, hwords(hco - i + 1) - 1)
                tx = st + Replace(tx, td, nt, hwords(hco - i + 1), 1)
            Next

        Catch ex As Exception
            parse_hyperlinks = itx
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_hyperlinks", bc_cs_activity_codes.COMMENTARY, "Cant parse hyperlink: " + CStr(paragraph_number) + ":" + itx + ": " + ex.Message)
        Finally
            parse_hyperlinks = tx
        End Try
    End Function
    Private Function tidy_up_text(ByVal text As String) As String
        text = text.Substring(0, text.Length - 1)
        text = RTrim(text)
        If Trim(text) <> "" Then
            If Asc(text.Substring(0, 1)) = 12 Or Asc(text.Substring(0, 1)) = 13 Then
                text = text.Substring(1, text.Length - 1)
            End If
            If Asc(text.Substring(text.Length - 1, 1)) = 13 Then
                text = text.Substring(0, text.Length - 1)
            End If
        End If
        tidy_up_text = text

    End Function

    Function parse_table(tcount As Integer, table As Object, ByRef rtable As List(Of bc_om_style_component_table_cell), ByVal bimages As Boolean) As Boolean
        Try
            parse_table = False
            rtable = New List(Of bc_om_style_component_table_cell)

            Dim cell As bc_om_style_component_table_cell
            Dim k As Integer
            For i = 1 To table.cells.count
                For k = 1 To table.cells(i).range.paragraphs.count
                    cell = New bc_om_style_component_table_cell
                    cell.row = table.cells(i).rowindex
                    cell.col = table.cells(i).columnindex
                    cell.paragraph = k
                    cell.text = table.cells(i).range.paragraphs(k).range.text
                    'cell.text = table.cells(i).range.text
                    If Asc(cell.text) <> 13 Then
                        cell.text = tidy_up_text(cell.text)
                    Else
                        cell.text = ""
                    End If
                    Try
                        cell.style = CStr(table.cells(i).range.paragraphs(k).range.style.namelocal)
                    Catch
                        cell.style = "could not read namelocal"
                    End Try
                    If table.cells(i).range.paragraphs(k).range.inlineshapes.count > 0 And bimages = True Then

                        REM TBD turn this back on later
                        cell.bimage = True
                        cell.image = get_image_from_inline_shape(table.cells(i).range.paragraphs(k).range.inlineshapes(1))
                        If IsNothing(cell.image) Then
                            Dim ocomm As New bc_cs_activity_log("bc_ao_word_html_componetize", "componetize", bc_cs_activity_codes.COMMENTARY, "Nothing Inline shape table cell table: " + CStr(tcount) + "cell: " + CStr(i))
                            overall_failure = True
                            Exit Function
                        End If
                        cell.text = "Chart"
                    End If
                    rtable.Add(cell)
                Next
            Next
            parse_table = True
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_table", bc_cs_activity_codes.COMMENTARY, "Cant parse table " + CStr(tcount) + ": " + ex.Message)
        End Try
    End Function
    Function CopyFromClipboardInlineShape(gimage As Object) As Byte()
        Try
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "CopyFromClipboardInlineShape", bc_cs_activity_codes.TRACE_ENTRY, "")

            Dim sfn As String
            Dim k As New Random

            k.ToString()


            sfn = k.Next.ToString + Now.Millisecond.ToString + Now.Second.ToString + Now.Minute.ToString + Now.Hour.ToString + Now.Day.ToString + Now.Month.ToString + Now.Year.ToString

            sfn = bc_cs_central_settings.local_repos_path + sfn + ".png"
            Dim retry As Integer = 0
            Dim bh As Byte()
            While retry < 10
                Try
                    gimage.select()

                    worddocument.application.selection.copyaspicture()
                    Dim computer As New Computer()
                    If (IsNothing(computer.Clipboard.GetDataObject()) = False) Then
                        Dim data As System.Windows.Forms.IDataObject
                        data = computer.Clipboard.GetDataObject()
                        If data.GetDataPresent(System.Windows.Forms.DataFormats.Bitmap) = True Then
                            Dim image As System.Drawing.Image
                            image = data.GetData(System.Windows.Forms.DataFormats.Bitmap, True)
                            image.Save(sfn, (System.Drawing.Imaging.ImageFormat.Png))
                            Dim fs As New bc_cs_file_transfer_services


                            fs.write_document_to_bytestream(sfn, bh, Nothing)
                            fs.delete_file(sfn)

                            CopyFromClipboardInlineShape = bh
                            retry = 10
                            Exit While
                        Else
                            Thread.Sleep(100)
                            retry = retry + 1
                            ocomm = New bc_cs_activity_log("bc_ao_word_html_componetize", "CopyFromClipboardInlineShape", bc_cs_activity_codes.COMMENTARY, "Nothing in Clipboard retry1:" + CStr(retry))
                        End If
                    Else
                        Thread.Sleep(100)
                        retry = retry + 1
                        ocomm = New bc_cs_activity_log("bc_ao_word_html_componetize", "CopyFromClipboardInlineShape", bc_cs_activity_codes.COMMENTARY, "Nothing in Clipboard retry2:" + CStr(retry))
                    End If

                Catch ex As Exception
                    Thread.Sleep(100)
                    retry = retry + 1
                    ocomm = New bc_cs_activity_log("bc_ao_word_html_componetize", "CopyFromClipboardInlineShape", bc_cs_activity_codes.COMMENTARY, "retry error:" + CStr(retry) + " : " + ex.Message)
                End Try
            End While

            If IsNothing(bh) Then
                ocomm = New bc_cs_activity_log("bc_ao_word_html_componetize", "CopyFromClipboardInlineShape", bc_cs_activity_codes.COMMENTARY, "Nothing in Clipboard  failed after retrys")
            End If


        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "componetize_error: CopyFromClipboardInlineShape", bc_cs_activity_codes.COMMENTARY, ex.Message)
            Return Nothing
        Finally
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "CopyFromClipboardInlineShape", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try

    End Function
    Function get_image_from_inline_shape(ByVal ils As Object) As Byte()
        Return CopyFromClipboardInlineShape(ils)
        Exit Function
        Try
            Dim ocommm As New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "entry")

            Dim data As IDataObject
            Dim bmp As System.Drawing.Bitmap
            Dim fs As New bc_cs_file_transfer_services
            Dim fn As String = bc_cs_central_settings.local_repos_path + "tmpimage.png"
            Dim success As Boolean = False
            Dim retry As Integer = 0
            Dim bytedoc As Byte() = Nothing


            While Not success And retry < 20
                Try

                    retry = retry + 1
                    Try
                        Clipboard.Clear()
                    Catch ex As Exception
                        ocommm = New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "clipboard error:" + ex.Message)

                    End Try



                    ils.range.copyaspicture()
                    Dim ocomm As New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "copy")

                    data = Clipboard.GetDataObject()
                    ocomm = New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "clipboard")

                    bmp = CType(data.GetData(GetType(System.Drawing.Bitmap)), System.Drawing.Bitmap)
                    ocomm = New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "getData")

                    bmp.Save(fn, System.Drawing.Imaging.ImageFormat.Png)
                    ocomm = New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "save")

                    fs.write_document_to_bytestream(fn, bytedoc, Nothing, True)
                    ocomm = New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "write")

                    fs.delete_file(fn)
                    ocomm = New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "delete")

                    success = True


                Catch ex As Exception
                    Dim ocomm As New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "Failed to get image: " + ex.Message + " path: " + fn)
                    Thread.CurrentThread.Sleep(500)
                End Try
            End While

            Return bytedoc
        Catch ex As Exception

            'componetize_error = True
            'componetize_error_text = "get_image_from_inline_shape: " + ex.Message
            Return Nothing
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "componetize_error: get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try
    End Function
    'Function get_image_from_inline_shape(ByVal ils As Object) As Byte()

    '    Try
    '        Dim ocommm As New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "entry")

    '        Dim data As IDataObject
    '        Dim bmp As System.Drawing.Bitmap
    '        Dim fs As New bc_cs_file_transfer_services
    '        Dim fn As String = bc_cs_central_settings.local_repos_path + "tmpimage.png"
    '        Dim success As Boolean = False
    '        Dim retry As Integer = 0
    '        Dim bytedoc As Byte() = Nothing


    '        While Not success And retry < 20
    '            Try

    '                retry = retry + 1
    '                Try
    '                    Clipboard.Clear()
    '                Catch ex As Exception
    '                    ocommm = New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "clipboard error:" + ex.Message)

    '                End Try



    '                ils.range.copyaspicture()
    '                Dim ocomm As New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "copy")

    '                data = Clipboard.GetDataObject()
    '                ocomm = New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "clipboard")

    '                bmp = CType(data.GetData(GetType(System.Drawing.Bitmap)), System.Drawing.Bitmap)
    '                ocomm = New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "getData")

    '                bmp.Save(fn, System.Drawing.Imaging.ImageFormat.Png)
    '                ocomm = New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "save")

    '                fs.write_document_to_bytestream(fn, bytedoc, Nothing, True)
    '                ocomm = New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "write")

    '                fs.delete_file(fn)
    '                ocomm = New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "delete")

    '                success = True


    '            Catch ex As Exception
    '                Dim ocomm As New bc_cs_activity_log("bc_ao_word_html_componetize", "get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, "Failed to get image: " + ex.Message + " path: " + fn)
    '                Thread.CurrentThread.Sleep(500)
    '            End Try
    '        End While

    '        Return bytedoc
    '    Catch ex As Exception

    '        'componetize_error = True
    '        'componetize_error_text = "get_image_from_inline_shape: " + ex.Message
    '        Return Nothing
    '        Dim ocomm As New bc_cs_activity_log("bc_ao_word", "componetize_error: get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, ex.Message)
    '    End Try
    'End Function

    Private Function parse_paragraph_in_text_box(p As Object, ByVal shape_id As Integer, ByRef paragraph_number As Integer, ByRef ofcomponents As bc_om_document_components, ByVal componetize_settings As bc_om_html_componetize_settings_for_template, cp As bc_dx_ao_component_swap, width As String) As Boolean
        Try
            REM take everything as lead
            Dim ocomponent = New bc_om_style_component
            parse_paragraph_in_text_box = False
            ocomponent.text_box_id = shape_id
            ocomponent.width = width
            ocomponent.page_no = p.Information(3)
            Dim thtml As String = ""
            Dim rtable As List(Of bc_om_style_component_table_cell)
            If p.tables.count = 0 Then
                If p.inlineshapes.count = 1 And p.text.length <= 2 And componetize_settings.s_images = True Then
                    Try
                        ocomponent.style = p.Style.namelocal
                    Catch
                        ocomponent.style = "could not set namelocal"
                    End Try
                    ocomponent.image = get_image_from_inline_shape(p.inlineshapes(1))
                    If IsNothing(ocomponent.image) Then
                        Dim ocomm As New bc_cs_activity_log("bc_ao_word_html_componetize", "componetize", bc_cs_activity_codes.COMMENTARY, "Nothing Inline shape paragraph in text box: " + CStr(shape_id) + "paragraph: " + CStr(paragraph_number))
                        overall_failure = True
                        Exit Function
                    End If
                    ocomponent.bimage = True
                    ocomponent.text = "image"
                    ofcomponents.components.Add(ocomponent)
                ElseIf componetize_settings.s_text = True And p.text.length > 2 Then
                    If componetize_settings.s_text_as_html_text_boxes = True And p.inlineshapes.count = 0 Then
                        paragraph_as_html(cp, p, thtml)
                        Try
                            ocomponent.style = p.Style.namelocal
                        Catch
                            ocomponent.style = "could not set namelocal"
                        End Try
                        ocomponent.text = tidy_up_text(p.text)
                        Dim pst, ptb As String
                        ocomponent.is_html = True

                        Dim s, e As Integer
                        s = InStr(thtml, "<style")
                        e = InStr(thtml, "</style>")
                        pst = thtml.Substring(s - 1, e - s + 8)
                        ocomponent.css = pst
                        s = InStr(thtml, "<body")
                        e = InStr(thtml, "</body>")
                        ptb = thtml.Substring(s + 5, e - s - 6)
                        ocomponent.html = ptb
                        ofcomponents.components.Add(ocomponent)
                    Else
                        REM if shaperanges anchored to paragrpah 
                        REM take as text
                        Try
                            ocomponent.style = p.Style.namelocal
                        Catch
                            ocomponent.style = "could not set namelocal"
                        End Try
                        ocomponent.text = tidy_up_text(p.text)
                        If p.Hyperlinks.Count > 0 Then
                            ocomponent.text = parse_hyperlinks(p, ocomponent.text, shape_id)
                        End If
                        ofcomponents.components.Add(ocomponent)
                    End If
                End If


            ElseIf p.tables.count = 1 Then
                REM image in table
                If componetize_settings.s_tables = True Then
                    Try
                        ocomponent.style = p.tables(1).Cell(1, 1).Range.Style.NameLocal
                    Catch
                        ocomponent.style = "could not set namelocal"
                    End Try
                    ocomponent.text = p.tables(1).Cell(1, 1).Range.text
                    ocomponent.btable = True
                    If p.tables(1).Range.InlineShapes.Count > 0 Then
                        ocomponent.chart_in_table = True
                    End If

                    If p.tables(1).range.InlineShapes.Count > 0 Or componetize_settings.s_table_as_html_text_boxes = False Then
                        rtable = New List(Of bc_om_style_component_table_cell)
                        If parse_table(0, p.tables(1).range, rtable, componetize_settings.s_images) = True Then
                            ocomponent.table_cells = rtable
                        Else
                            Dim ocomm As New bc_cs_activity_log("bc_ao_word_html_componetize", "componetize", bc_cs_activity_codes.COMMENTARY, "Parse table failed " + CStr(shape_id) + "paragraph: " + CStr(paragraph_number))

                            overall_failure = True
                        End If
                    Else
                        If table_as_html(0, cp, p.tables(1).range, thtml) = True Then
                            Dim tx, st, tb As String
                            tx = thtml
                            Dim s, e As Integer
                            s = InStr(tx, "<style")
                            e = InStr(tx, "</style>")

                            e = InStrRev(tx, "</style>")
                            st = tx.Substring(s - 1, e - s + 8)
                            ocomponent.is_html = True

                            ocomponent.css = st
                            s = InStr(tx, "<table")
                            REM PR look for last table
                            'e = InStr(tx, "</table>")
                            e = InStrRev(tx, "</table>")
                            tb = tx.Substring(s - 1, e - s + 8)
                            ocomponent.html = tb
                        End If
                    End If


                    ofcomponents.components.Add(ocomponent)
                End If
                paragraph_number = paragraph_number + p.Tables(1).Range.Paragraphs.Count - 1

            End If
            parse_paragraph_in_text_box = True
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Couldnt compontize paragraph: " + CStr(paragraph_number) + ": in shape: " + CStr(shape_id) + ": " + ex.Message)
        End Try



    End Function

End Class
