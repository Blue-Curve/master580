Imports BlueCurve.Core.cs
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports System.Threading
Imports System.Xml
Imports System.Xml.Linq
Imports System.Text
Imports System.Text.RegularExpressions
Imports System.Runtime.InteropServices
REM ING JUNE 2012
REM ==========================================
REM Blue Curve Limited 2005
REM Module:       AT/BP Application Object Specific
REM Type:         Application Object
REM Description:  Word
REM               Powerpoint
REM Version:      1
REM Change history
REM ==========================================
REM builds a word document

Module mclipboard
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
'End Module
Public Class bc_ao_word
    Inherits bc_ao_at_object

    Private Declare Function OpenClipboard Lib "user32" (ByVal hwnd As Long) As Long
    Private Declare Function RegisterClipboardFormat Lib "user32" Alias _
        "RegisterClipboardFormatA" (ByVal lpString As String) As Long
    Private Declare Function EmptyClipboard Lib "user32" () As Long
    Private Declare Function CloseClipboard Lib "user32" () As Long
    Private Declare Function SetClipboardData Lib "user32" ( _
        ByVal wFormat As Long, ByVal hMem As Long) As Long
    Private Declare Function GlobalAlloc Lib "kernel32" (ByVal wFlags As Long, _
        ByVal dwBytes As Long) As Long
    Private Declare Sub CopyMemory Lib "kernel32" Alias "RtlMoveMemory" ( _
        ByVal Destination As Long, ByVal Source As Object, ByVal Length As Long)
    Private Declare Function GlobalUnlock Lib "kernel32" (ByVal hMem As Long) As Long
    Private Declare Function GlobalLock Lib "kernel32" (ByVal hMem As Long) As Long
    Private Declare Function GlobalFree Lib "kernel32" Alias "GlobalFree" ( _
         ByVal hMem As Long) As Long

    Private Const GMEM_DDESHARE = &H2000
    Private Const GMEM_MOVEABLE = &H2

    Public Const TABLE = 1
    Public Const PROP = 28
    Public Const TEXT = 2
    Public Const BOOKMARK = 27
    Private trackchangesstate As Boolean
    REM word instance
    Private word As Object
    REM excel
    Private excel As Object
    REM word document instance
    Public worddocument As Object
    ' is word aready open
    Private wordAlreadyOpen As Boolean
    REM constructor if document open

    Public Sub New(ByRef oworddocument As Object)
        worddocument = oworddocument
        word = worddocument.application
    End Sub
    REM constructor if document is not open
    Public Sub New()

    End Sub
   

    


    Public Overrides Function rendertoPDF_With_office(fn As String) As String
        Try

            Dim paramExportFormat As Integer
            paramExportFormat = 17 'WdExportFormat.wdExportFormatPDF
            Dim paramOpenAfterExport As Boolean = False
            Dim paramExportOptimizeFor As Integer
            paramExportOptimizeFor = 0 'WdExportOptimizeFor.wdExportOptimizeForPrint
            Dim paramExportRange As Integer
            paramExportRange = 0 'WdExportRange.wdExportAllDocument
            Dim paramStartPage As Int32 = 0
            Dim paramEndPage As Int32 = 0
            Dim paramExportItem As Integer
            paramExportItem = 0 'WdExportItem.wdExportDocumentContent
            Dim paramIncludeDocProps As Boolean = True
            Dim paramKeepIRM As Boolean = True
            Dim paramCreateBookmarks As Integer
            paramCreateBookmarks = 1 'WdExportCreateBookmarks.wdExportCreateWordBookmarks

            Dim paramDocStructureTags As Boolean = False
            Dim paramBitmapMissingFonts As Boolean = True
            Dim paramUseISO19005_1 As Boolean = False


            Try
                worddocument.ExportAsFixedFormat(fn, paramExportFormat, paramOpenAfterExport, _
                    paramExportOptimizeFor, paramExportRange, paramStartPage, paramEndPage, paramExportItem, paramIncludeDocProps, paramKeepIRM, _
                    paramCreateBookmarks, paramDocStructureTags, paramBitmapMissingFonts, paramUseISO19005_1)
            Catch ex As Exception
                rendertoPDF_With_office = ex.Message
                Exit Function
            End Try

            rendertoPDF_With_office = ""
        Catch ex As Exception
            rendertoPDF_With_office = ex.Message
            Dim ocommentary = New bc_cs_activity_log("bc_ao_word", "rendertoPDF_with_office", bc_cs_activity_codes.COMMENTARY, ex.Message)

        End Try

    End Function

    REM strips out parts if rtf not wanted
    Public Overrides Function check_if_visible()
        check_if_visible = word.visible
    End Function
    Private Function format_rtf(ByVal str As String) As String
        Dim stripoutvalues As New ArrayList
        stripoutvalues.Add("themedata")
        stripoutvalues.Add("colorschememapping")
        stripoutvalues.Add("datastore")
        stripoutvalues.Add("svb")
        stripoutvalues.Add("blipuid")
        Dim i, j, k As Integer
        Dim st, en As String

        For k = 0 To stripoutvalues.Count - 1
            i = InStr(str, "{\*\" + stripoutvalues(k))
            If i > 0 Then
                st = str.Substring(0, i - 1)
                j = InStr(i + 1, str, "}")
                en = str.Substring(j, Len(str) - j)
                str = st + en
            End If
        Next
        format_rtf = str
    End Function

    REM July 2010 new routines fir user defined components
    Public rtf_tx As String
    Public Overrides Sub enable_screen_updating()

        Try
            worddocument.application.screenupdating = True
        Catch
            word.screenupdating = True
        End Try
    End Sub
    Public Overrides Sub disable_screen_updating()
        worddocument.application.screenupdating = False
    End Sub

    Public Overrides Sub set_activedocument(ByVal name As String)
        REM pr fix active document can get changed on refresh so reset
        For i = 1 To worddocument.application.documents.count
            If worddocument.application.documents(i).name = name Then
                worddocument.application.documents(i).select()
                Exit For
            End If
        Next
    End Sub
    Public Overrides Function get_activedocument()
        REM pr fix active document can get changed on refresh so reset
        get_activedocument = worddocument.application.activedocument.name
    End Function

    Public Overrides Sub set_background_colour_for_locator(ByVal locator As String, ByVal colour_index As Long, ByVal type As Integer)
        Dim slog As New bc_cs_activity_log("bc_ao_word", "set_background_colour_for_locator", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            If colour_index = 0 Then
                Exit Sub
            End If

            If worddocument.Bookmarks.exists(locator) Then
                Select Case type
                    Case 1, 11, 3, 4, 5, 8, 9, 12, 100, 31
                        worddocument.Bookmarks(locator).Range.Paragraphs.Shading.BackgroundPatternColor = colour_index
                        REM FIL OCT 2013
                        worddocument.Bookmarks(locator).Range.Shading.BackgroundPatternColor = colour_index
                    Case 2, 21
                        worddocument.Bookmarks(locator).Range.Tables(1).Shading.BackgroundPatternColor = colour_index
                End Select

            End If
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "set_background_color_for_locator", bc_cs_activity_codes.COMMENTARY, "Unable to set colour index:" + CStr(colour_index) + "for locator: " + CStr(locator))
        Finally
            slog = New bc_cs_activity_log("bc_ao_word", "set_background_colour_for_locator", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try

    End Sub

    Public Overrides Function insert_side_comment() As String
        Try
            Dim tx As String
            Dim side_comment_style As String
            Dim crange As Object
            crange = word.selection.range

            'checks
            'check style exists

            side_comment_style = ""

            side_comment_style = get_property_value("xrnet_style_sidecomment")


            If side_comment_style = "Error" Then
                insert_side_comment = "Side Comment Style not set cant find property or value for xrnet_style_sidecomment"
                Exit Function
            End If

            'check frame style
            If worddocument.Styles(side_comment_style).Frame.WidthRule = 0 Then
                insert_side_comment = "Please update Side Comment style before inserting a Side Comment"
                Exit Function
            End If
            'check insertion point

            ' Check cursor position
            If crange.Information(12) = True Then
                insert_side_comment = "You are trying to put a figure inside of table. Please move the insertion point elsewhere."
                Exit Function
            End If

            If crange.Information(28) = True Then
                insert_side_comment = "You are in the header/footer of the document. Please move the insertion point elsewhere."
                Exit Function
            End If


            'Check if you are inside of frame, this will only recognise a selected frame
            If crange.Frames.Count > 0 Then
                insert_side_comment = "You are inside of a frame. Please move the insertion point elsewhere."
                Exit Function
            End If

            ' sometimes a shape will remain selected
            If crange.ShapeRange.Count > 0 Then
                insert_side_comment = "You are inside of a shape. Please move the insertion point elsewhere."
                Exit Function
            End If

            If crange.paragraphs.count > 1 Then
                insert_side_comment = "You can only highlight up to one paragraph. Please move the insertion point elsewhere."
                Exit Function

            End If

            Dim lseltext As String
            If Len(crange.text) <> 0 Then
                lseltext = Right(crange.text, 1)
                If Asc(lseltext) = 13 Then word.selection.MoveLeft(Count:=1, Extend:=1)
            End If



            ' end checks


            tx = crange.Text
            If tx = "" Then
                tx = "Type your comment"
            Else
                word.selection.MoveRight(Unit:=1, Count:=1)
                word.selection.range.Bookmarks.Add("_BC_TMP_SC")
            End If
            Dim oSCFrame As Object
            oSCFrame = worddocument.Frames.Add(Range:=crange)
            oSCFrame.Borders.OutsideLineStyle = 0
            If tx = "Type your comment" Then
                oSCFrame.Range.InsertAfter(tx)
                oSCFrame.Range.style = side_comment_style
            Else
                crange.Style = "Margin Text"
            End If
            

            If worddocument.Bookmarks.Exists("_BC_TMP_SC") Then
                If tx <> "Type your comment" Then
                    worddocument.Bookmarks("_BC_TMP_SC").Range.Text = tx
                End If
                worddocument.Bookmarks("_BC_TMP_SC").Delete()
            End If
            insert_side_comment = ""
        Catch ex As Exception
            insert_side_comment = "Cannot Insert Side Comment Please Change Your Selection."

        End Try
    End Function


    Public Overrides Function get_background_color_for_locator(ByVal locator As String) As Long
        Try
            get_background_color_for_locator = 0

            If worddocument.Bookmarks.exists(locator) Then
                get_background_color_for_locator = worddocument.Bookmarks(locator).Range.Paragraphs.Shading.BackgroundPatternColor
                REM FIL OCT 2013
                If get_background_color_for_locator = 0 Then
                    get_background_color_for_locator = worddocument.Bookmarks(locator).Range.Shading.BackgroundPatternColor
                End If
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_word", "get_background_color_for_locator", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try
    End Function
    Public Function save_to_mht(ByVal fn As String) As Boolean
        Try
            save_to_mht = False
            Try
                REM 2010
                worddocument.SaveAs2(fn, FileFormat:= _
                     9, LockComments:=False, Password:="", AddToRecentFiles:= _
                        True, WritePassword:="", ReadOnlyRecommended:=False, EmbedTrueTypeFonts:= _
                        False, SaveNativePictureFormat:=False, SaveFormsData:=False, _
                        SaveAsAOCELetter:=False)
            Catch
                REM 2007
                worddocument.SaveAs(fn, FileFormat:= _
                     9, LockComments:=False, Password:="", AddToRecentFiles:= _
                        True, WritePassword:="", ReadOnlyRecommended:=False, EmbedTrueTypeFonts:= _
                        False, SaveNativePictureFormat:=False, SaveFormsData:=False, _
                        SaveAsAOCELetter:=False)
            End Try
            save_to_mht = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_word", "save_to_mht", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
           
    Public Overrides Function get_selection_in_rtf(Optional ByVal locator As String = "") As String
        get_selection_in_rtf = ""
        Dim data As IDataObject = Nothing
        Try


            clipboard.SetText(" ")


            If locator <> "" And worddocument.bookmarks.exists(locator) Then
                worddocument.bookmarks(locator).range.copyaspicture()
            ElseIf locator <> "" Then
                Exit Function
            End If

            Application.DoEvents()
            data = clipboard.GetDataObject()

            If Not data Is Nothing AndAlso _
            data.GetDataPresent("Rich Text Format") Then
                get_selection_in_rtf = data.GetData("Rich Text Format", True)
                get_selection_in_rtf = get_selection_in_rtf.Trim
            End If

        Catch ex As Exception
            get_selection_in_rtf = ""
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_from_rtf", bc_cs_activity_codes.COMMENTARY, ex.Message)

            'Dim oerr As New bc_cs_error_log("bc_ao_word", "get_selection_from_rtf", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            If Not data Is Nothing Then
                data = Nothing
            End If
            clipboard.SetText(" ")
        End Try
    End Function
    REM SIM MAY 2013
    Public Overrides Function get_images_in_selection(ByVal locator As String, Optional ByVal bwithtable As Boolean = False) As ArrayList
        Dim images As New ArrayList

        Try
            Dim data As IDataObject
            Dim bmp As System.Drawing.Bitmap
            Dim fs As New bc_cs_file_transfer_services
            Dim fn As String

            fn = bc_cs_central_settings.local_repos_path + "tmpimage.png"
            Dim bytedoc As Byte() = Nothing
            Dim orange, prange As Object
            If worddocument.bookmarks.exists(locator) Then
                If bwithtable = True Then
                    orange = worddocument.bookmarks(locator).range.tables(1).range
                Else
                    orange = worddocument.bookmarks(locator).range
                End If
                For i = 1 To orange.inlineshapes.count
                    prange = orange.inlineshapes(i).range
                    prange.copyaspicture()
                    data = clipboard.GetDataObject()
                    bmp = CType(data.GetData(GetType(System.Drawing.Bitmap)), System.Drawing.Bitmap)
                    bmp.Save(fn, System.Drawing.Imaging.ImageFormat.Png)
                    fs.write_document_to_bytestream(fn, bytedoc, Nothing, True)
                    fs.delete_file(fn)
                    images.Add(bytedoc)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_word", " get_images_in_selection:" + locator, bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            get_images_in_selection = images
        End Try
    End Function
    REM for style componrizer
    Public Sub get_selection_in_rtf_and_html_compressed(ByRef compressed_rtf As Byte(), ByRef compressed_html As Byte(), ByVal btake_rtf As Boolean, ByVal btake_html As Boolean, ByVal btable As Boolean, ByVal paragraph_number As Integer, Optional ByVal shape_Id As Integer = 0)
        Dim data As IDataObject = Nothing

        Dim rtf As String
        Dim html As String
        Dim retry As Integer = 3
        Dim i As Integer = 0

        Try

            While i < retry
                Try


                    If shape_Id = 0 Then
                        If btable = True Then
                            worddocument.paragraphs(paragraph_number).range.tables(1).range.copyaspicture()
                        Else
                            worddocument.paragraphs(paragraph_number).range.copy()
                        End If
                    Else
                        If btable = True Then
                            worddocument.shapes(shape_Id).paragraphs(paragraph_number).range.tables(1).range.copyaspicture()
                        Else
                            worddocument.shapes(shape_Id).paragraphs(paragraph_number).range.copy()
                        End If

                    End If

                    Application.DoEvents()
                    data = Clipboard.GetDataObject()

                    If Not data Is Nothing AndAlso _
                    data.GetDataPresent("Rich Text Format") Then
                        rtf = data.GetData("Rich Text Format", True)
                 
                        Dim bcs As New bc_cs_security
                        Try
                            If btake_rtf = True Then
                                compressed_rtf = bcs.compress_xml_winzip(rtf, Nothing)
                            End If
                            If btake_html = True Then
                                Try
                                    html = mclipboard.GetClipboard
                                Catch ex As Exception
                                    html = data.GetData(System.Windows.Forms.DataFormats.Html)
                                End Try
                                compressed_html = bcs.compress_xml_winzip(html, Nothing)
                            End If

                            If ((btake_rtf = True And IsNothing(compressed_rtf) = False) And (btake_html = True And IsNothing(compressed_html) = False)) Or _
                                ((btake_rtf = True And IsNothing(compressed_rtf) = False) And btake_html = False) Or _
                                ((btake_html = True And IsNothing(compressed_html) = False) And btake_rtf = False) Then
                                Exit Sub
                            End If
                        Catch ex As Exception
                            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_in_rtf_and_html_compressed", bc_cs_activity_codes.COMMENTARY, "Failed to extact component in HTML: " + ex.Message)
                        End Try

                    Else
                        Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_in_rtf_and_html_compressed", bc_cs_activity_codes.COMMENTARY, "Data Nothing or no RTF")
                    End If


                Catch ex As Exception
                    Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_in_rtf_and_html_compressed", bc_cs_activity_codes.COMMENTARY, ex.Message)

                End Try
                Dim occomm As New bc_cs_activity_log("bc_ao_word", "get_selection_in_rtf_and_html_compressed", bc_cs_activity_codes.COMMENTARY, "Retrying " + CStr(i + 1))
                i = i + 1
            End While
            'Dim oerr As New bc_cs_error_log("bc_ao_word", "get_selection_from_rtf", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Catch ex As Exception
            compressed_rtf = Nothing
            compressed_html = Nothing


            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_in_rtf_and_html_compressed", bc_cs_activity_codes.COMMENTARY, ex.Message)

        Finally
            If Not data Is Nothing Then
                data = Nothing
            End If
        End Try
    End Sub
    
    Public Sub get_selection_in_rtf_and_html_compressed_in_text_box(obj As Object, ByRef compressed_rtf As Byte(), ByRef compressed_html As Byte(), ByVal btake_rtf As Boolean, ByVal btake_html As Boolean, ByVal btable As Boolean, ByVal paragraph_number As Integer, Optional ByVal shape_Id As Integer = 0)
        Dim data As IDataObject = Nothing

        Dim rtf As String
        Dim html As String
        Dim retry As Integer = 3
        Dim i As Integer = 0

        Try

            While i < retry
                Try


                    If shape_Id = 0 Then
                        If btable = True Then
                            obj.Range.tables(1).range.copyaspicture()
                        Else
                            obj.Range.range.copy()
                        End If
                    Else
                        If btable = True Then
                            obj.Range.tables(1).range.copyaspicture()
                        Else
                            obj.Range.copy()
                        End If

                    End If

                    Application.DoEvents()
                    data = Clipboard.GetDataObject()

                    If Not data Is Nothing AndAlso _
                    data.GetDataPresent("Rich Text Format") Then
                        rtf = data.GetData("Rich Text Format", True)
                        Dim bcs As New bc_cs_security
                        Try
                            If btake_rtf = True Then

                                compressed_rtf = bcs.compress_xml_winzip(rtf, Nothing)
                            End If
                            If btake_html = True Then
                                Try
                                    html = mclipboard.GetClipboard
                                Catch ex As Exception
                                    html = data.GetData(System.Windows.Forms.DataFormats.Html)
                                End Try
                                compressed_html = bcs.compress_xml_winzip(html, Nothing)
                            End If

                            If ((btake_rtf = True And IsNothing(compressed_rtf) = False) And (btake_html = True And IsNothing(compressed_html) = False)) Or _
                                ((btake_rtf = True And IsNothing(compressed_rtf) = False) And btake_html = False) Or _
                                ((btake_html = True And IsNothing(compressed_html) = False) And btake_rtf = False) Then
                                Exit Sub
                            End If
                        Catch ex As Exception
                            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_in_rtf_and_html_compressed_in_text_boxs", bc_cs_activity_codes.COMMENTARY, "Failed to extact component in HTML: " + ex.Message)
                        End Try

                    Else
                        Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_in_rtf_and_html_compressed_in_text_boxs", bc_cs_activity_codes.COMMENTARY, "Data Nothing or no RTF")
                    End If


                Catch ex As Exception
                    Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_in_rtf_and_html_compressed_in_text_boxs", bc_cs_activity_codes.COMMENTARY, ex.Message)

                End Try
                Dim occomm As New bc_cs_activity_log("bc_ao_word", "get_selection_in_rtf_and_html_compressed_in_text_boxs", bc_cs_activity_codes.COMMENTARY, "Retrying " + CStr(i + 1))
                i = i + 1
            End While
            'Dim oerr As New bc_cs_error_log("bc_ao_word", "get_selection_from_rtf", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Catch ex As Exception
            compressed_rtf = Nothing
            compressed_html = Nothing


            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_in_rtf_and_html_compressed_in_text_boxs", bc_cs_activity_codes.COMMENTARY, ex.Message)

        Finally
            If Not data Is Nothing Then
                data = Nothing
            End If
        End Try
    End Sub
    Public Overrides Function get_selection_in_rtf_compressed(ByRef success As Boolean, Optional ByVal locator As String = "", Optional ByVal btable As Boolean = False, Optional ByRef compressed_html As Byte() = Nothing, Optional ByVal comp_type As Long = 0, Optional ByVal mode As Integer = 0) As Byte()
        get_selection_in_rtf_compressed = Nothing
        success = False
        Dim data As IDataObject = Nothing

        Dim rtf As String
        Dim html As String
        Dim retry As Integer = 10
        Dim i As Integer = 0

        Try
            While i < retry
                Try

                    Clipboard.SetText(" ")

                    If locator <> "" And worddocument.bookmarks.exists(locator) Then
                        If btable = False Then
                            If comp_type = 111 Then
                                worddocument.bookmarks(locator).range.copyaspicture()
                                Dim oRange As Object
                                oRange = worddocument.bookmarks(locator).range
                                If oRange.start <= oRange.end - 1 Then
                                    oRange.end = oRange.end - 1
                                    oRange.copyaspicture()
                                End If
                            Else
                                worddocument.bookmarks(locator).range.copyaspicture()
                            End If
                        Else
                            Try
                                worddocument.bookmarks(locator).range.tables(1).range.copyaspicture()
                            Catch ex As Exception
                                Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_from_rtf_compressed", bc_cs_activity_codes.COMMENTARY, "Expected a table but none existed: " + locator + ":" + ex.Message)
                            End Try
                        End If
                    ElseIf locator <> "" Then
                        Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_in_rtf_compressed", bc_cs_activity_codes.COMMENTARY, "Locator not found: " + locator)
                        Exit Function
                    End If

                    Dim bcs As New bc_cs_security

                    If mode <> 888 Then
                        Application.DoEvents()
                        data = Clipboard.GetDataObject()
                        If Not data Is Nothing AndAlso _
                        data.GetDataPresent("Rich Text Format") Then
                            rtf = data.GetData("Rich Text Format", True)
                            Try
                                get_selection_in_rtf_compressed = bcs.compress_xml_winzip(rtf, Nothing)
                                success = True
                            Catch ex As Exception
                                Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_from_rtf_compressed", bc_cs_activity_codes.COMMENTARY, "Failed to get rtf: " + locator + ":" + ex.Message)
                            End Try
                        End If
                    Else
                        success = True
                    End If

                    If success = True AndAlso mode <> 88 Then
                        success = False
                        Try
                            html = mclipboard.GetClipboard
                        Catch ex As Exception
                            html = data.GetData(System.Windows.Forms.DataFormats.Html)
                        End Try
                        Try
                            compressed_html = bcs.compress_xml_winzip(html, Nothing)
                            success = True
                            Exit While
                        Catch ex As Exception
                            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_from_rtf_compressed", bc_cs_activity_codes.COMMENTARY, "Failed to get html: " + locator + ":" + ex.Message)
                        End Try
                    Else
                        Exit While
                    End If

                Catch ex As Exception
                    Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_from_rtf_compressed", bc_cs_activity_codes.COMMENTARY, ex.Message)

                End Try
                Dim occomm As New bc_cs_activity_log("bc_ao_word", "get_selection_from_rtf_compressed", bc_cs_activity_codes.COMMENTARY, "Retrying " + CStr(i + 1))
                i = i + 1
            End While
            'Dim oerr As New bc_cs_error_log("bc_ao_word", "get_selection_from_rtf", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_from_rtf_compressed", bc_cs_activity_codes.COMMENTARY, ex.Message)


        Finally
            If Not data Is Nothing Then
                data = Nothing
            End If
            Clipboard.SetText(" ")
        End Try
    End Function

    Public Function dx_paragraph_as_html_rtf(para As Object, brtf As Boolean, bhtml As Boolean, ByRef rc As bc_om_refresh_component) As Boolean
        dx_paragraph_as_html_rtf = False

        Try

            Dim cp As New bc_dx_ao_component_swap
            Dim bcs As New bc_cs_security
            cp.RichEditControl1.ResetText()

            Try
                para.copy()
            Catch ex As Exception
                Try
                    worddocument.Bookmarks(rc.locator).Delete()
                    worddocument.Bookmarks.Add(rc.locator, para)
                    para.copy()
                Catch exx As Exception
                    Dim oerr As New bc_cs_error_log("bc_ao_word", "dx_paragraph_as_html_rtf", bc_cs_error_codes.USER_DEFINED, CStr(rc.type) + " copy : " + ex.Message)
                    Exit Function
                End Try
            End Try


            cp.RichEditControl1.Paste()
            Dim html, rtf As String
            If bhtml = True Then

                html = cp.RichEditControl1.HtmlText

                rc.compressed_html = bcs.compress_xml_winzip(html, Nothing)


            End If
            If brtf = True Then

                rtf = cp.RichEditControl1.RtfText
                rc.objbyte = bcs.compress_xml_winzip(rtf, Nothing)



            End If


            dx_paragraph_as_html_rtf = True
        Catch ex As Exception

            Dim oerr As New bc_cs_error_log("bc_ao_word", "dx_paragraph_as_html_rtf", bc_cs_error_codes.USER_DEFINED, CStr(rc.type) + ": " + ex.Message)
        End Try
    End Function
    Public Function get_selection_from_dev_expresss_converter(ByRef rc As bc_om_refresh_component) As Boolean
        get_selection_from_dev_expresss_converter = False

        Dim data As IDataObject = Nothing

        Dim rtf As String = ""
        Dim html As String = ""
        Dim retry As Integer = 10
        Dim i As Integer = 0

        Try
            Dim orange As Object
            REM check locator exists TBD 
            If worddocument.bookmarks.exists(rc.locator) = False Then
                Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_from_dev_expresss_converter", bc_cs_activity_codes.COMMENTARY, "Locator not found: " + rc.locator)

                Exit Function
            End If
            REM get text value
            rc.tx = CStr(get_value_for_locator(rc.locator))

            Dim bhtml As Boolean = False
            Dim brtf As Boolean = False
            Select Case rc.mode
                Case 80, 110
                    REM rtf html
                    brtf = True
                    bhtml = True
                    orange = worddocument.bookmarks(rc.locator).range()
                Case 880
                    REM html 
                    brtf = True
                    orange = worddocument.bookmarks(rc.locator).range()
                Case 8880
                    REM html 
                    bhtml = True
                    orange = worddocument.bookmarks(rc.locator).range()
                Case 210, 260
                    brtf = True
                    bhtml = True
                    orange = worddocument.bookmarks(rc.locator).range.tables(1).range()
            End Select
            REM html

            If dx_paragraph_as_html_rtf(orange, brtf, bhtml, rc) = True Then
                get_selection_from_dev_expresss_converter = True
            End If

        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_selection_from_dev_express_converter", bc_cs_activity_codes.COMMENTARY, ex.Message)

        End Try
    End Function


    Public Overloads Function set_selection_from_rtf(ByVal bmrange As Object, ByVal rtf As String) As Boolean
        Try

            Dim ocomm As bc_cs_activity_log
            Dim clipboard_success As Boolean = False
            Dim data As New DataObject
            Dim retry As Integer = 0
            set_selection_from_rtf = False
            While retry < 10 And clipboard_success = False
                Try
                    Clipboard.SetText(" ")
                    Application.DoEvents()
                    clipboard_success = True
                Catch ex As Exception
                    ocomm = New bc_cs_activity_log("bc_ao_word", "set_selection_from_rtf", bc_cs_activity_codes.COMMENTARY, "clipboared clear failed retry: " + CStr(retry) + ":" + ex.Message)
                    retry = retry + 1
                End Try
            End While
            If clipboard_success = False Then
                ocomm = New bc_cs_activity_log("bc_ao_word", "set_selection_from_rtf", bc_cs_activity_codes.COMMENTARY, "Clipboard failed to cleared")
            End If
            retry = 0
            While set_selection_from_rtf = False And retry < 10
                Try
                    Application.DoEvents()
                    data.SetData("Rich Text Format", rtf)
                    Application.DoEvents()
                    Clipboard.SetDataObject(data)
                    Try
                        bmrange.Paste()
                        set_selection_from_rtf = True
                    Catch ex As Exception
                        bmrange.pasteasnestedtable()
                        set_selection_from_rtf = True
                    End Try
                Catch ex As Exception
                    ocomm = New bc_cs_activity_log("bc_ao_word", "set_selection_from_rtf", bc_cs_activity_codes.COMMENTARY, "rtf failed retry: " + CStr(retry) + ":" + ex.Message)
                    retry = retry + 1
                End Try
            End While
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "New2 set_selection_from_rtf", bc_cs_activity_codes.COMMENTARY, ex.Message)
            set_selection_from_rtf = False
            'Dim oerr As New bc_cs_error_log("bc_ao_word", "set_selection_from_rtf", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function

    Public Overrides Function set_selection_from_rtf(ByVal bm As String, ByVal rtf As String, ByVal refresh_components As bc_om_refresh_components) As Boolean
        Try
            Dim data As New DataObject
            data.SetData("Rich Text Format", rtf)
            Application.DoEvents()
            clipboard.SetDataObject(data)
            Dim bmrange As Object
            bmrange = worddocument.Bookmarks(bm).Range
            data.SetData("Rich Text Format", rtf)
            clipboard.SetDataObject(data)
            Try
                bmrange.paste()
            Catch ex As Exception
                bmrange.pasteasnestedtable()
            End Try
            worddocument.Bookmarks.Add(Name:=bm, Range:=bmrange)
            Me.remove_unused_udc_sdc_locators(refresh_components)
            set_selection_from_rtf = True
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "set_selection_from_rtf", bc_cs_activity_codes.COMMENTARY, ex.Message)
            set_selection_from_rtf = False
            'Dim oerr As New bc_cs_error_log("bc_ao_word", "set_selection_from_rtf", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Public Overrides Sub udi_delete_bookmark(ByVal bm As String)
        Try
            worddocument.Bookmarks(bm).Delete()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_word", "udi_delete_bookmark", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Overrides Sub udi_delete_bookmark_and_range(ByVal bm As String)
        Try
            worddocument.Bookmarks(bm).range.delete()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_word", "udi_delete_bookmark", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Public Overrides Function udi_get_selected_udi_id() As Long
        udi_get_selected_udi_id = 0
        For i = 1 To worddocument.application.selection.range.bookmarks.count - 0
            If worddocument.application.selection.range.bookmarks(i).name.length > 8 Then
                If worddocument.application.selection.range.bookmarks(i).name.substring(0, 8) = "rnet_udc" Then
                    If worddocument.application.selection.bookmarks(i).name.substring(0, 8) = "rnet_udc" Then
                        udi_get_selected_udi_id = CLng(worddocument.application.selection.bookmarks(i).name.substring(9, worddocument.application.selection.bookmarks(i).name.length - 9))
                    End If
                    worddocument.application.selection.range.bookmarks(i).range.select()
                    Exit Function
                    Exit For
                End If
            End If
        Next
    End Function
    Public Overrides Function check_for_sdc() As Boolean
        check_for_sdc = False
        For i = 1 To worddocument.application.selection.range.bookmarks.count
            If worddocument.application.selection.range.bookmarks(i).name.length > 8 Then
                If InStr(worddocument.application.selection.range.bookmarks(i).name, "_at") > 0 Then
                    check_for_sdc = True
                    Exit Function
                    Exit For
                End If
            End If
        Next
    End Function
    Public Overrides Function check_for_udc() As Boolean
        check_for_udc = False
        For i = 1 To worddocument.application.selection.range.bookmarks.count
            If worddocument.application.selection.range.bookmarks(i).name.length > 8 Then
                If InStr(worddocument.application.selection.range.bookmarks(i).name, "_udc") > 0 Then
                    check_for_udc = True
                    Exit Function
                    Exit For
                End If
            End If
        Next
    End Function
    Public Overrides Sub udi_highlight(ByVal bm)
        worddocument.Bookmarks(bm).Range.Select()
    End Sub
    Public Overrides Function udi_get_selected_text() As String
        udi_get_selected_text = worddocument.application.selection.paragraphs(1).range.text()
    End Function
    Public Overrides Function udi_get_first_paragraph() As String
        udi_get_first_paragraph = worddocument.application.selection.paragraphs(1).range.text
    End Function
    Public Overrides Function udi_get_number_of_paragraphs() As String
        udi_get_number_of_paragraphs = worddocument.application.selection.paragraphs.count
    End Function
    Public Overrides Function udi_set_locator(ByVal udi_id As Long, ByVal entity_id As Long, Optional ByVal new_sel As Boolean = False) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "udi_set_locator", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim bm As String = ""
        Dim i As Integer = 1
        Try
            If entity_id = 0 Then
                bm = "rnet" + CStr(worddocument.Bookmarks.count + i) + "_udc" + CStr(udi_id) + "_en" + CStr(Format(Now, "hhmmss"))
            Else
                bm = "rnet" + CStr(worddocument.bookmarks.count + 1) + "_udc" + CStr(udi_id) + "_en" + CStr(entity_id)
            End If
            Dim err = True
            While worddocument.bookmarks.exists(bm) = True
                i = i + 1
                If entity_id = 0 Then
                    bm = "rnet" + CStr(worddocument.Bookmarks.count + i) + "_udc" + CStr(udi_id) + "_en" + CStr(Format(Now, "hhmmss"))
                Else
                    bm = "rnet" + CStr(worddocument.bookmarks.count + 1) + "_udc" + CStr(udi_id) + "_en" + CStr(entity_id)
                End If
            End While
            worddocument.Application.Selection.Bookmarks.Add(bm)

        Catch ex As Exception
            Dim omessage As New bc_cs_message("Blue Curve", "Cant add component at selected location", bc_cs_message.MESSAGE)
            bm = ""
        Finally
            udi_set_locator = bm
            otrace = New bc_cs_activity_log("bc_ao_word", "udi_set_locator", bc_cs_activity_codes.TRACE_ENTRY, "")
        End Try

    End Function
    Public Overrides Sub udi_set_bookmark(ByVal bm As String)
        worddocument.application.selection.range.bookmarks.add(bm)
    End Sub
    Public Overrides Sub udi_set_bookmark_around_paragraphs(ByVal bm As String, ByVal paras As Integer)
        Try
            REM now make sure bookmark goes around insertion
            worddocument.bookmarks(bm).range.select()
            Dim arange As Object

            arange = worddocument.Range( _
                Start:=worddocument.Paragraphs(udi_GetParNum() + 1).Range.Start, _
                End:=worddocument.Paragraphs(udi_GetParNum() + paras + 1).Range.End)

            arange.Bookmarks.Add(bm)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_word", "udi_set_bookmark_around_paragraphs", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub
    Private Function udi_GetParNum() As Integer
        Dim rParagraphs As Object
        Dim CurPos As Long
        Try
            'r.Select
            CurPos = worddocument.Bookmarks("\startOfSel").Start
            rParagraphs = worddocument.Range(Start:=0, End:=CurPos)
            udi_GetParNum = rParagraphs.Paragraphs.Count
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_word", "udi_GetParNum", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function
    Public Overrides Function udi_component_exists(ByVal name As String) As Integer
        Try
            udi_component_exists = 0
            If worddocument.bookmarks.exists(name) Then
                udi_component_exists = worddocument.Bookmarks(name).Range.Information(3)
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_word", "udi_component_exists", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function
    REM End July 2010
    REM routine to open a word document 
    Private Sub invoke_word_new()
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "invoke_word_new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            word = CreateObject("word.application")
            wordAlreadyOpen = False

            REM SW cope with office versions
            bc_cs_central_settings.user_office_version = Replace(word.Version, ".0", "")

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "invoke_word_new", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "invoke_word_new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Private Sub invoke_word()
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "invoke_word", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            word = GetObject(, "word.application")
            wordAlreadyOpen = True

            REM SW cope with office versions
            bc_cs_central_settings.user_office_version = Replace(word.Version, ".0", "")

        Catch
            Try
                word = CreateObject("word.application")
                wordAlreadyOpen = False

                REM SW cope with office versions
                bc_cs_central_settings.user_office_version = Replace(word.Version, ".0", "")

            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_ao_word", "invoke_word", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try

        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "invoke_word", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Overrides Sub update_refresh_status_bar(ByVal caption As String, ByVal item As Integer, ByVal total_items As Integer)

        If total_items = 0 Then
            worddocument.application.statusbar = caption
        Else
            worddocument.application.statusbar = caption + CStr(item) + " of " + CStr(total_items)
        End If
    End Sub
    Public Overrides Sub lock_document()
        Try
            'worddocument.customdocumentproperties("xrnet_locked") = 1
            If worddocument.protectiontype = 2 Then
                Exit Sub
            End If
            Try
                worddocument.protect(2, True, bc_am_load_objects.obc_prefs.document_protection_password)
            Catch
                worddocument.protect(2)
            End Try

            'worddocument.FormFields(1).Select()
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "lock_document", bc_cs_activity_codes.COMMENTARY, "Warning: cant lock document: " + ex.Message)

        End Try

    End Sub
    Public Overrides Sub unlock_document()
        Try
            'worddocument.customdocumentproperties("xrnet_locked") = 0
            If worddocument.protectiontype = -1 Then
                Exit Sub
            End If
            Try
                worddocument.unprotect(bc_am_load_objects.obc_prefs.document_protection_password)
            Catch
                worddocument.unprotect()
            End Try
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "lock_document", bc_cs_activity_codes.COMMENTARY, "Warning: cant unlock document: " + ex.Message)
        End Try
    End Sub


    Public Overrides Sub protect_all_document()
        Try
            'Fully lock the document
            If worddocument.protectiontype <> -1 Then
                Exit Sub
            End If
            Try
                worddocument.protect(3, True, bc_am_load_objects.obc_prefs.document_protection_password)
            Catch
                worddocument.protect(3)
            End Try

        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "protect_all_document", bc_cs_activity_codes.COMMENTARY, "Warning: cant lock document: " + ex.Message)

        End Try

    End Sub


    Public Overrides Sub convert_to_rtf(ByVal fn As String)
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "convert_to_rtf", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            REM save word document as rtf
            worddocument.SaveAs(FileName:=fn, FileFormat:=100, LockComments _
                :=False, Password:="", AddToRecentFiles:=True, WritePassword:="", _
                ReadOnlyRecommended:=False, EmbedTrueTypeFonts:=False, _
                SaveNativePictureFormat:=False, SaveFormsData:=False, SaveAsAOCELetter:= _
                False)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "convert_to_rtf", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "convert_to_rtf", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Overrides Function get_object() As Object
        get_object = Me.worddocument
    End Function
    Public Overrides Function get_property_value(ByVal property_name As String) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "get_property_value", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            get_property_value = worddocument.customdocumentproperties(property_name).value()
        Catch ex As Exception
            get_property_value = "Error"
            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "get_property_value", bc_cs_activity_codes.COMMENTARY, "Failed to set property: " + property_name, Nothing)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "get_property_value", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Overrides Function get_locator_user_friendly_name(ByVal locator As String) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "get_locator_user_friendly_name", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "open", bc_cs_activity_codes.COMMENTARY, "Locator Name: " + locator)
            get_locator_user_friendly_name = "Page:"
            Exit Function
            If worddocument.bookmarks.exists(locator) Then
                worddocument.Bookmarks(locator).Range.select()
                get_locator_user_friendly_name = "Page: " + CStr(worddocument.Application.Selection.Information(3))
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "get_locator_user_friendly_name", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_locator_user_friendly_name = ""
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "get_locator_user_friendly_name", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Overrides Sub invisible()
        Try
            word.application.visible = False
        Catch
            Try
                worddocument.application.visible = False
            Catch
            End Try
        End Try

    End Sub
    Public Overrides Sub quit_if_no_docs()
        If word.documents.count = 0 Then
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
            GC.WaitForPendingFinalizers()
            Me.quit()
            Marshal.FinalReleaseComObject(word)
            word = Nothing
        Else
            word.visible = True
        End If
    End Sub
    Public Overrides Sub abort_doc()
        save()
        word.displayalerts = False
        close()
        visible()
    End Sub
    Public Overloads Overrides Sub visible()
        REM worddocument.application.visible = True


        If word.documents.count = 0 And wordAlreadyOpen = False Then

            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
            GC.WaitForPendingFinalizers()
            Me.quit()
            Marshal.FinalReleaseComObject(word)
            word = Nothing
        Else
            word.visible = True
        End If
    End Sub
    Public Overrides Function get_selection_values(ByRef row, ByRef col) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "get_selection_values", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim tx As String
            row = 0
            col = 0

            Try
                row = worddocument.Application.Selection.Information(14)
                col = worddocument.Application.Selection.Information(17)
                tx = worddocument.Application.Selection.tables(1).cell(row, col).range.text
                If Len(tx) > 2 Then
                    tx = Left(tx, Len(tx) - 2)
                Else
                    tx = ""
                End If
                get_selection_values = tx
            Catch ex As Exception
                Dim ocommentray As New bc_cs_activity_log("bc_ao_word", "get_selection_values", bc_cs_activity_codes.COMMENTARY, "Error getting selection parameters: " + ex.Message)
                get_selection_values = ""
            End Try

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "get_selection_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_selection_values = ""
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "get_selection_values", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Sub open_new_instance(ByVal strfilename As String, ByVal visible As Boolean)
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "open", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            word = CreateObject("word.application")
            wordAlreadyOpen = False

            REM SW cope with office versions
            bc_cs_central_settings.user_office_version = word.Version

            If word.WindowState <> 2 Then
                word.visible = False
            End If


            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "new", bc_cs_activity_codes.COMMENTARY, "Attempting to Open Document: " + strfilename)
            REM open locally
            worddocument = word.documents.open(strfilename)

            REM make application visible
            word.visible = visible
            REM set title
            If bc_am_load_objects.obc_current_document.title <> "" Then
                worddocument.activewindow.caption = bc_am_load_objects.obc_current_document.title
            End If

            worddocument.Application.WindowState = 1
            worddocument.activate()
            docobject = worddocument
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "open", bc_cs_error_codes.USER_DEFINED, ex.Message)
            word.visible = True

        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "open", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try
    End Sub
    Function copyTableAsImage(ByVal table_locator As String, ByVal fn As String, ByVal doc_id As Long) As Boolean
        ' *** Copies all of the tables in the document and pastes them as image files ***
        '
        Dim canvas As Object = Nothing

        Try
            Dim tbl As Object
            Dim tblWidth As Integer
            Dim tblHeight As Integer
            Dim tblShape As Object
            Dim tblShapeWidth As Integer
            Dim cropRight As Integer


            ' Create a blank page to be used as the canvas
            canvas = word.application.Documents.Add

            ' Set the selection to the beginning of the document
            word.ActiveWindow.Selection.GoTo(What:=0, Which:=1)


            tbl = worddocument.bookmarks(table_locator).range.tables(1)

            If tbl.Rows.Count > 1 Then

                tbl.Select()

                ' Setup the canvas page to paste tables into so that original document is not modified
                If SetPageOrientation(canvas) = False Then
                    canvas.Close(SaveChanges:=0)
                    copyTableAsImage = False
                    Exit Function
                End If


                worddocument.application.Selection.Copy()

                ' Paste as HTML to preserve formatting
                canvas.Content.PasteSpecial(DataType:=10)

                ' Get the width of the table from the first row
                tblWidth = canvas.Tables(1).Cell(1, 1).Width

                ' ** Delete the header row from the table
                'canvas.Tables(1).Cell(1, 1).range.delete()

                canvas.Tables(1).Range.Cells(1).Select()
                word.Selection.Rows.Delete()


                'With canvas.Tables(1)
                '    Call DeleteHeaderRow(canvas)
                'End With
                canvas.Tables(1).Select()
                word.Selection.Cut()
                tblShape = canvas.Tables.Add(Range:=canvas.Range(0, 0), NumRows:=1, NumColumns:=1, DefaultTableBehavior:=0)

                ' Position the table in the top-right corner
                With tblShape.Rows
                    .RelativeVerticalPosition = 0
                    .HorizontalPosition = 0
                    .RelativeHorizontalPosition = 0
                    .VerticalPosition = 0
                End With

                ' Initialise the table height/width
                With tblShape
                    .Rows(1).HeightRule = 1
                    .Rows(1).Height = 5
                    .Columns(1).Width = tblWidth
                    .TopPadding = 2 ' New...
                    .LeftPadding = 2 ' New...
                    .RightPadding = 0 ' New...
                    .BottomPadding = 0 ' New...

                End With

                ' Paste table into container table
                tblShape.Rows(1).Cells(1).Range.PasteSpecial(DataType:=10)

                ' Get the width of the first table row to get the entire table width
                tblHeight = tblShape.Rows(1).Height

                tblShape.Select()
                canvas.application.Selection.Cut()

                ' Paste as Image
                canvas.Content.PasteSpecial(DataType:=9)


                ' New...
                ' Only crop if necesary
                tblShapeWidth = canvas.Shapes(1).Width
                If tblShapeWidth > tblWidth Then
                    cropRight = tblShapeWidth - tblWidth
                Else
                    cropRight = 0
                End If


                ' Resize the image
                canvas.Shapes(1).ConvertToInlineShape()

                With canvas.InlineShapes(1)
                    .Width = tblWidth
                    .Height = tblHeight
                    .ScaleWidth = 100
                    .ScaleHeight = 100
                    .PictureFormat.cropRight = cropRight - 20 ' New
                    '.PictureFormat.Crop.ShapeWidth = tblWidth + 20
                End With

                Dim bmp As System.Drawing.Bitmap
                Dim success As Boolean = False

                Dim bytedoc As Byte() = Nothing

                Try
                    Dim data As IDataObject
                    canvas.InlineShapes(1).range.copyaspicture()
                    data = Clipboard.GetDataObject()
                    bmp = CType(data.GetData(GetType(System.Drawing.Bitmap)), System.Drawing.Bitmap)
                    bmp.Save(fn, System.Drawing.Imaging.ImageFormat.Png)
                Catch ex As Exception
                    Dim ocomm As New bc_cs_activity_log("bc_ao_word", "CopytableAsImageSave", bc_cs_activity_codes.COMMENTARY, ex.Message)
                    copyTableAsImage = False
                End Try

            End If

            ' Close the canvas once images have been rendered
            canvas.Close(SaveChanges:=0)
            Dim fs As New bc_cs_file_transfer_services

            copyTableAsImage = True

        Catch ex As Exception
            If Not IsNothing(canvas) Then
                canvas.Close(SaveChanges:=0)
            End If
            copyTableAsImage = False
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "CopytableAsImage", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try

    End Function
    Function SetPageOrientation(ByRef newPage As Object) As Boolean
        ' *** Creates a canvas page and sets the orientation based on the page containing ***
        ' *** the table which will be converted to an image ***'
        '
        Try
            Dim CurrentOrientation As Integer
            Dim CurrentSection As Integer


            ' Get the current section number
            CurrentSection = word.ActiveWindow.Selection.Information(2)

            ' Get the orientation of the current page containing the table
            CurrentOrientation = worddocument.Sections(CurrentSection).PageSetup.Orientation

            ' Set the orientation of the canvas page
            newPage.Sections(1).PageSetup.Orientation = CurrentOrientation
            'newPage.PrintOut

            ' Set the canvas page to PrintView so that the image is not distorted
            newPage.Windows(1).View.Type = 3

            ' Set the margin and indentation of the new page
            newPage.PageSetup.LeftMargin = 10
            newPage.PageSetup.RightMargin = 10
            newPage.PageSetup.TopMargin = 5
            newPage.PageSetup.BottomMargin = 5
            newPage.Paragraphs.LeftIndent = 10
            newPage.Paragraphs.RightIndent = 10
            SetPageOrientation = True
        Catch ex As Exception
            SetPageOrientation = False
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "SetPageOrientation", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try

    End Function
    Public Function get_table_as_image_from_bm(ByVal table_locator As String, ByVal fn As String, ByVal doc_id As Long) As Boolean

        Try

            get_table_as_image_from_bm = copyTableAsImage(table_locator, fn, doc_id)

        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_table_as_image", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try
    End Function
    Public Overrides Sub set_parameter(ByVal prop As String, ByVal value As String)
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "set_parameter", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "set_parameter", bc_cs_activity_codes.COMMENTARY, "Attempting to set property: " + prop + " with value: " + value)
            worddocument.customdocumentproperties(prop).value = value
        Catch ex As Exception
            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "set_parameter", bc_cs_activity_codes.COMMENTARY, "Failed to set property: " + prop + " with value: " + value)

        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "set_parameter", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Sub highlight_locator(ByVal locator As String, ByVal mode As Integer)
        Try
            If worddocument.bookmarks.exists(locator) Then
                If mode = 2 Or mode = 21 Or mode = 6 Then
                    If worddocument.bookmarks(locator).range.tables.count > 0 Then
                        worddocument.bookmarks(locator).range.tables(1).select()
                    Else
                        worddocument.bookmarks(locator).range.select()
                    End If
                Else
                    worddocument.bookmarks(locator).range.select()
                End If
            End If
        Catch ex As Exception
            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "set_parameter", bc_cs_activity_codes.COMMENTARY, "Failed to set highlight locator: " + locator)
        End Try
    End Sub
    Public Overrides Sub activate()
        Try
            Dim i As Integer
            Dim s As String

            word.Application.Activate()

            For i = 1 To word.Documents.Count
                If word.Documents(i).Name = worddocument.name Then
                    s = worddocument.name
                    Try
                        REM SW cope with office versions
                        If bc_cs_central_settings.userOfficeStatus = 3 Then
                            'handle 2007 compatibility mode
                            worddocument.application.windows(String.Concat(s, " [Compatibility Mode]")).activate()
                        Else
                            worddocument.application.windows(s).activate()
                        End If
                    Catch
                        ' ignore error as only trying to activate window
                    End Try
                    Exit For
                End If
            Next
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "activate", bc_cs_activity_codes.COMMENTARY, "error: " + ex.Message)
        End Try
    End Sub
    Public Overrides Sub get_index_components(ByRef indexs As bc_om_indexs)
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "get_index_components", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim index_component As bc_om_index_component

        Dim i As Integer
        Dim bm As String

        Try
            indexs.index_components.Clear()
            REM parse all bookmarks looking for indexs the assign to object
            For i = 1 To worddocument.bookmarks.count
                bm = worddocument.bookmarks(i).name
                If Len(bm) > 8 Then
                    If Left(bm, 8) = "rnet_ind" Then
                        index_component = New bc_om_index_component
                        REM index id
                        If IsNumeric(bm.Substring(8, 1)) Then
                            index_component.index_id = bm.Substring(8, 1)
                            REM page
                            worddocument.Bookmarks(bm).Range.select()
                            index_component.page = worddocument.Application.Selection.Information(3)
                            REM item
                            index_component.item = worddocument.Bookmarks(bm).Range.text()
                            If Len(index_component.item) > 2 Then
                                index_component.item = Left(index_component.item, Len(index_component.item) - 2)
                            End If
                            indexs.index_components.Add(index_component)
                        End If
                    End If
                End If
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "get_index_components", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "get_index_components", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Overrides Sub caption(ByVal caption As String)
        worddocument.activewindow.caption = caption
    End Sub



    Public Overrides Sub open(ByVal strfilename As String, ByVal visible As Boolean, Optional ByVal new_instance As Boolean = False)
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "open", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            If new_instance = True Then
                invoke_word_new()
            Else
                invoke_word()
            End If

            If word.WindowState <> 2 Then
                word.visible = False
            End If

            Dim attached_template As String
            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "new", bc_cs_activity_codes.COMMENTARY, "Attempting to Open Document: " + strfilename)
            REM open locally


            worddocument = word.documents.open(strfilename)

            attached_template = get_property_value("xrnet_attached_template")

            If attached_template <> "Error" Then
                If attached_template <> "" Then
                    Try
                        ocommentary = New bc_cs_activity_log("bc_ao_word", "open", bc_cs_activity_codes.COMMENTARY, "Attempting to attach template: " + bc_cs_central_settings.local_template_path + attached_template)
                        REM FIL Match 2014 XP windows 7 only reattach if not attchaed
                        If worddocument.AttachedTemplate.name <> attached_template Then
                            worddocument.attachedtemplate = bc_cs_central_settings.local_template_path + attached_template
                        End If
                        REM END
                    Catch ex As Exception

                        ocommentary = New bc_cs_activity_log("bc_ao_word", "open", bc_cs_activity_codes.COMMENTARY, "Failed ro attach template: " + bc_cs_central_settings.local_template_path + attached_template + ": " + ex.Message)
                    End Try
                End If
            End If

            REM make application visible
            word.visible = visible
            REM set title
            If bc_am_load_objects.obc_current_document.title <> "" Then
                worddocument.activewindow.caption = bc_am_load_objects.obc_current_document.title
            End If

            worddocument.Application.WindowState = 1
            worddocument.activate()

            docobject = worddocument
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "open", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Try
                word.visible = True
            Catch exs As Exception
                Dim ocomm As New bc_cs_activity_log("bc_ao_word", "open", bc_cs_error_codes.USER_DEFINED, "failed to make word visible: " + ex.Message)
            End Try

        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "open", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try
    End Sub
    Public Overrides Sub open_read_only(ByVal strfilename As String, ByVal name As String)
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "open_read_only", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            invoke_word()
            REM open locally
            worddocument = word.documents.open(strfilename)
            REM make application visible
            word.visible = True
            REM set title
            worddocument.activewindow.caption = name
            Me.protect_all_document()

            'worddocument.Application.WindowState = 1
            'worddocument.activate()
            'docobject = worddocument
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "open_read_only", bc_cs_error_codes.USER_DEFINED, ex.Message)
            word.visible = True
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "open_read_only", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    REM number of pages
    Public Overrides Function number_of_pages() As Integer
        REM wdNumberOfPagesInDocument is 4
        number_of_pages = worddocument.Range.Information(4)
    End Function
    Public Overrides Sub set_doc_id(ByVal doc_id As String)
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "set_doc_id", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            worddocument.customdocumentproperties("rnet_doc_id").value = CStr(doc_id)
        Catch ex As Exception
            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "set_doc_id", bc_cs_activity_codes.COMMENTARY, "Failed to find property: rnet_doc_id")
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "set_doc_id", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Overrides Function get_doc_id()
        Try
            get_doc_id = worddocument.customdocumentproperties("rnet_doc_id").value
        Catch ex As Exception
            get_doc_id = "NONE"
            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "get_doc_id", bc_cs_activity_codes.COMMENTARY, "Property rnet_doc_id not set in Document.")
        End Try
    End Function
    Public Overrides Function get_document_title() As String
        Try
            get_document_title = ""
            get_document_title = worddocument.customdocumentproperties("xrnet_title").value()
        Catch ex As Exception
            REM jsut log title property error to activity file
            get_document_title = ""
            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "get_document_title", bc_cs_activity_codes.COMMENTARY, "Property xrnet_title not set in Document.")
        End Try
    End Function
    Public Overrides Function get_locator_for_display(ByVal atBookmarkOnly As Boolean) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "get_locator_for_display", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i, r, c As Integer
            Dim found As Integer
            Dim na As String

            get_locator_for_display = ""
            r = worddocument.Application.Selection.Information(13)
            c = worddocument.Application.Selection.Information(17)

            If atBookmarkOnly = True Then
                If r = -1 And c = -1 Then
                    found = False
                    REM picture
                    If worddocument.Application.Selection.ShapeRange.Count > 0 Then
                        na = worddocument.Application.Selection.ShapeRange(1).Name
                        If worddocument.Application.Selection.Tables.Count > 0 Then
                            For r = 1 To worddocument.Application.Selection.Tables(1).Rows.Count - 1
                                For c = 1 To worddocument.Application.Selection.Tables(1).Columns.Count - 1
                                    Try
                                        If worddocument.Application.Selection.Tables(1).Cell(r, c).Range.ShapeRange.Count = 1 Then
                                            If na = worddocument.Application.Selection.Tables(1).Cell(r, c).Range.ShapeRange(1).Name Then
                                                For i = 1 To worddocument.Application.Selection.Tables(1).Cell(r, c).Range.Bookmarks.Count
                                                    If InStr(worddocument.Application.Selection.Tables(1).Cell(r, c).Range.Bookmarks(i).name, "rnet") > 0 Then
                                                        found = True
                                                        Exit For
                                                    End If
                                                    If found = True Then
                                                        Exit For
                                                    End If
                                                Next
                                            End If
                                        End If
                                        If found = True Then
                                            Exit For
                                        End If
                                    Catch

                                    End Try
                                Next
                                If found = True Then
                                    Dim comm As New bc_cs_activity_log("bc_ao_word", "get_locator_for_display", bc_cs_activity_codes.COMMENTARY, "Locator found as row: " + CStr(r) + " col: " + CStr(c))
                                    Exit For
                                End If
                            Next
                        End If
                    End If
                End If
            End If

            REM Blueprintz

            If Not atBookmarkOnly AndAlso worddocument.Application.Selection.Tables.Count > 0 Then
                If worddocument.Application.Selection.Tables(1).Cell(1, 1).Range.Bookmarks.Count > 1 Then
                    For i = 1 To worddocument.Application.Selection.Tables(1).Cell(1, 1).Range.Bookmarks.Count
                        get_locator_for_display = worddocument.Application.Selection.Tables(1).Cell(1, 1).Range.Bookmarks(i).Name
                        If InStr(1, get_locator_for_display, "_r1c1") > 0 Then
                            Exit For
                        End If
                    Next
                End If
            End If

            If atBookmarkOnly = True Then
                found = False
                na = ""
                REM first see if bookmark exists in selected cell
                If worddocument.Application.Selection.Tables.Count > 0 Then
                    Try
                        For i = 1 To worddocument.Application.Selection.Tables(1).Cell(r, c).Range.Bookmarks.Count
                            na = worddocument.Application.Selection.Tables(1).Cell(r, c).Range.Bookmarks(i).Name
                            If InStr(1, na, "_at") > 1 Then
                                If worddocument.Bookmarks(na).Range.Information(17) = c Then
                                    found = True
                                    Exit For
                                End If
                            End If
                        Next
                    Catch
                        'if merged cells are present try cell(1,1)
                    End Try
                End If
                REM if not look in cell(1,1)
                If found = False Then
                    If worddocument.Application.Selection.Tables.Count > 0 Then
                        For i = 1 To worddocument.Application.Selection.Tables(1).Cell(1, 1).Range.Bookmarks.Count
                            na = worddocument.Application.Selection.Tables(1).Cell(1, 1).Range.Bookmarks(i).Name
                            If InStr(1, na, "_at") > 1 Then
                                found = True
                                Exit For
                            End If
                        Next
                    End If
                End If
                REM now look around selection
                If found = False Then
                    For i = 1 To worddocument.application.selection.range.bookmarks.count - 0
                        If worddocument.application.selection.range.bookmarks(i).name.length > 8 Then
                            If InStr(worddocument.application.selection.range.bookmarks(i).name, "_udc") > 0 Then
                                get_locator_for_display = worddocument.application.selection.range.bookmarks(i).name

                                found = True
                                Exit Function
                                Exit For
                            End If
                        End If
                    Next
                End If

                If found = False Then
                    get_locator_for_display = ""
                Else
                    get_locator_for_display = na
                    Dim comm As New bc_cs_activity_log("bc_ao_word", "get_locator_for_display", bc_cs_activity_codes.COMMENTARY, "Locator found: " + na)
                End If

            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "get_locator_for_display", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_locator_for_display = ""
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "get_locator_for_display", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Public Sub remove_unused_udc_sdc_locators(ByVal refresh_components As bc_om_refresh_components)
        Dim i As Integer = 0

        Try
            Dim found As Boolean = False
            Dim bm_count As Integer
            Dim no_inc As Boolean = False
            bm_count = worddocument.bookmarks.count - 1
            While i < bm_count + 1
                found = False
                no_inc = False
                If InStr(worddocument.bookmarks(i + 1).name, "_udc") > 0 Or InStr(worddocument.bookmarks(i + 1).name, "_at") > 0 Then
                    For j = 0 To refresh_components.refresh_components.Count - 1
                        If refresh_components.refresh_components(j).locator = worddocument.bookmarks(i + 1).name Then
                            found = True
                            Exit For
                        End If
                    Next
                    If found = False Then
                        Dim ocomm As New bc_cs_activity_log("bc_ao_word", "remove_unused_udc_sdc_locators", bc_cs_activity_codes.COMMENTARY, "Removing unused bookmark: " + worddocument.bookmarks(i + 1).name)
                        worddocument.bookmarks(i + 1).delete()
                        no_inc = True
                        'bm_count = worddocument.bookmarks.count - 1
                    End If
                End If
                If no_inc = False Then
                    i = i + 1
                End If
            End While

        Catch ex As Exception
            'Dim db_err As New bc_cs_error_log("bc_ao_word", "remove_unused_udc_locators", bc_cs_error_codes.USER_DEFINED, ex.Message + ": " + CStr(i))
        End Try
    End Sub

    Public Overloads Sub visible(ByVal visible As Boolean)
        If visible = True Then
            If worddocument.application.visible = False Then
                worddocument.application.visible = True
            End If
        Else
            worddocument.application.visible = False
        End If
    End Sub
    Public Sub insert_file(ByVal filename As String, Optional ByVal new_page As Boolean = True, Optional ByVal link_to_previous As Boolean = True, Optional ByVal bookmark_name As String = "")
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "insert_file", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            If Trim(bookmark_name) = "" Then
                bookmark_name = "\endofdoc"
                worddocument.Bookmarks(bookmark_name).Range.Select()

            ElseIf Trim(bookmark_name) = "rpcstart" Then
                worddocument.Bookmarks(bookmark_name).Range.Select()
                worddocument.Bookmarks(bookmark_name).Delete()
                worddocument.application.Selection.InsertBreak(2)
                worddocument.application.Selection.InsertFile(filename:=filename, Range:="", ConfirmConversions:=False, Link:=False, Attachment:=False)
                Exit Try
            ElseIf Trim(bookmark_name) = "rpccont" Then
                worddocument.Bookmarks(bookmark_name).Range.Select()
                worddocument.Bookmarks(bookmark_name).Delete()
                worddocument.application.Selection.InsertFile(filename:=filename, Range:="", ConfirmConversions:=False, Link:=False, Attachment:=False)
                Exit Try
            Else
                If worddocument.Bookmarks.Exists(bookmark_name) Then
                    worddocument.Bookmarks(bookmark_name).Range.Select()
                    worddocument.Bookmarks(bookmark_name).Delete()
                    worddocument.application.Selection.InsertFile(filename:=filename, Range:="", ConfirmConversions:=False, Link:=False, Attachment:=False)
                    Exit Try
                End If
            End If

            If new_page = True Then
                worddocument.application.Selection.InsertBreak(2)
                If link_to_previous = False Then
                    worddocument.application.ActiveWindow.ActivePane.View.SeekView = 9
                    worddocument.application.Selection.headerfooter.linktoprevious = False
                    worddocument.application.ActiveWindow.ActivePane.View.Type = 3
                    If worddocument.Bookmarks.Exists(bookmark_name) Then
                        worddocument.Bookmarks(bookmark_name).Range.Select()
                    End If
                End If
            End If

            worddocument.application.Selection.InsertFile(filename:=filename, Range:="", ConfirmConversions:=False, Link:=False, Attachment:=False)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "insert_file", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "insert_file", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides Sub disable_track_changes()
        Try
            Me.trackchangesstate = worddocument.trackrevisions
            worddocument.trackrevisions = False
        Catch ex As Exception
            REM juut log sub title property error to activity log
            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "disable_track_changes", bc_cs_activity_codes.COMMENTARY, "Property xrnet_subtitle not set in Document.")
        End Try

    End Sub
    Public Overrides Sub enable_track_changes()
        Try
            worddocument.trackrevisions = Me.trackchangesstate
        Catch ex As Exception
            REM juut log sub title property error to activity log
            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "enable_track_changes", bc_cs_activity_codes.COMMENTARY, "Property xrnet_subtitle not set in Document.")
        End Try

    End Sub

    Public Overrides Function get_document_subtitle() As String
        Try
            get_document_subtitle = worddocument.customdocumentproperties("xrnet_sub_title").value
        Catch ex As Exception
            REM juut log sub title property error to activity log
            get_document_subtitle = ""
            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "get_document_subtitle", bc_cs_activity_codes.COMMENTARY, "Property xrnet_subtitle not set in Document.")
        End Try
    End Function
    Public Overrides Function get_document_summary() As String
        Dim prop_name As String = ""
        get_document_summary = ""

        Try
            Dim i As Integer
            i = 1
            While i > 0
                prop_name = "xrnet_summary" + CStr(i)
                If i > 1 Then
                    'get_document_summary = get_document_summary + " "
                    get_document_summary = get_document_summary
                End If
                get_document_summary = get_document_summary + worddocument.customdocumentproperties(prop_name).value
                i = i + 1
            End While
        Catch ex As Exception
            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "get_document_summary", bc_cs_activity_codes.COMMENTARY, "Property " + prop_name + " not set in Document.")
            'get_document_summary = ""
        Finally

        End Try
    End Function
    REM constructor to open a template
    Public Sub New(ByRef otemplate As bc_om_template, ByRef oproperties As bc_om_doc_properties, ByVal bvisible As Boolean, ByVal bastemplate As Boolean)
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings

            Dim i As Integer = 1

            bsuccess = False

            ogtemplate = otemplate
            ogproperties = oproperties
            bc_am_load_objects.obc_current_document.filename = MyBase.generate_local_filename_prefix
            REM get a handle on word or startup word
            invoke_word()

            If word.WindowState <> 2 Then
                word.visible = False
            End If

            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "new", bc_cs_activity_codes.COMMENTARY, "Attempting to create Document from template: " + bc_cs_central_settings.local_template_path + otemplate.filename)
            If bastemplate = True Then
                worddocument = word.documents.open(bc_cs_central_settings.local_template_path + otemplate.filename)
            Else
                worddocument = word.documents.add(bc_cs_central_settings.local_template_path + otemplate.filename)
            End If

            ' OFFICE 2010 asynchronous startup issues
            While worddocument Is Nothing And i < 16 ' max time allowed (15 seconds) to get a word document
                ocommentary = New bc_cs_activity_log("bc_ao_word", "new", bc_cs_activity_codes.COMMENTARY, "Failed to create Document from template retrying for 15 seconds: " + bc_cs_central_settings.local_template_path + otemplate.filename)
                If bastemplate = True Then
                    worddocument = word.documents.open(bc_cs_central_settings.local_template_path + otemplate.filename)
                Else
                    worddocument = word.documents.add(bc_cs_central_settings.local_template_path + otemplate.filename)
                End If
                If worddocument Is Nothing Then
                    Thread.Sleep(1000)
                    i = i + 1
                Else
                    Exit While
                End If
                Application.DoEvents()
            End While
            If i = 16 Then
                Dim omessage As New bc_cs_message("Blue Curve create", "Could not create a new word document", bc_cs_message.MESSAGE)
                word.visible = True
                Exit Try
            End If

            REM Steve Wooderson 07/03/2012 Fix problem with minamized documents
            worddocument.ActiveWindow.Visible = False
            REM worddocument.application.visible = False

            REM set docfilename temporaily to this new document
            ocommentary = New bc_cs_activity_log("bc_ao_word", "new", bc_cs_activity_codes.COMMENTARY, "Document created from template: " + bc_cs_central_settings.local_template_path + otemplate.filename)
            bsuccess = True
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
            word.visible = True
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    REM shows word document
    Public Overrides Sub show()
        Try
            If Not IsNothing(word) Then
                word.visible = True
            End If
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "show", bc_cs_activity_codes.COMMENTARY, "error: " + ex.Message)
        End Try
    End Sub
    'bc_ax_style_componetize(ByVal ao_object As Object, ByVal ao_type As String, ByRef ocomponents As bc_om_document_components, ByVal componetize_styles As bc_om_componetize_styles_for_template) As Object

    Public Function bc_ax_style_componetize(ByRef ocomponents As bc_om_document_components, ByVal componetize_styles As bc_om_componetize_styles_for_template) As Boolean
        REM pass through to VBA
        Try
            bc_ax_style_componetize = worddocument.bc_ax_style_componetize(ocomponents, componetize_styles)
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "bc_ax_style_componetize", bc_cs_activity_codes.COMMENTARY, ex.Message, Nothing)
            bc_ax_style_componetize = False
        End Try

    End Function

    Public componetize_error As Boolean = False
    Public componetize_error_text As String = ""
    Public Overrides Function componetize(ByRef ocomponents As bc_om_document_components, ByVal componetize_styles As bc_om_componetize_styles_for_template) As Boolean
        Try
            worddocument.application.displayalerts = False
            componetize_error = False
            componetize_error_text = ""
            Dim ocomponent As bc_om_style_component = Nothing
            Dim co As Integer
            co = worddocument.paragraphs.count
            REM do main body first
            Dim body_style As New ArrayList
            Dim para As Integer = worddocument.paragraphs.count
            Dim i As Integer
            Dim ocell As bc_om_style_component_table_cell


            If componetize_styles.table_on_demand = True Then
                REM main document body
                For i = 1 To worddocument.paragraphs.count
                    Try
                        parse_paragraph(i, ocomponents, componetize_styles, ocomponent, body_style, componetize_styles.rtf, componetize_styles.html)
                        If worddocument.paragraphs(i).Range.Information(3) > 2 Then
                            Exit For
                        End If
                    Catch ex As Exception
                        Dim ocomm As New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Couldnt compontize paragraph: " + CStr(i) + ": " + ex.Message)
                    End Try
                Next

                REM now do tables

                Dim opara As bc_om_style_component_paragraph
                Dim bm As String
                Dim table_count As Integer
                table_count = worddocument.tables.count
                For i = 1 To worddocument.tables.count
                    If worddocument.tables(i).range.cells.count > 1 Then
                        Try
                            opara = New bc_om_style_component_paragraph
                            opara.btable = True
                            REM if the bookmark exists on table leave
                            bm = bookmark_already_in_table(i)
                            REM otherwise generate a new bookmark
                            If bm = "" Then
                                bm = generate_new_bookmark(i)
                                worddocument.tables(i).range.bookmarks.add(bm)
                            End If
                            opara.table_locator = bm
                            opara.page_no = worddocument.tables(i).Range.Information(3)
                            opara.style = worddocument.tables(i).range.cells(1).range.paragraphs(1).range.style.namelocal

                            opara.table_cells = New List(Of bc_om_style_component_table_cell)
                            opara.text = worddocument.tables(i).range.cells(1).range.paragraphs(1).range.text
                            REM ===
                            REM take text for first cell
                            ocell = New bc_om_style_component_table_cell
                            ocell.row = 1
                            ocell.col = 1
                            ocell.paragraph = 1
                            ocell.bimage = False
                            ocell.text = opara.text
                            If Asc(ocell.text) <> 13 Then
                                ocell.text = tidy_up_text(ocell.text)
                                opara.text = tidy_up_text(opara.text)


                            Else
                                ocell.text = ""
                            End If
                            opara.table_cells.Add(ocell)
                            REM rest take as image
                            ocell = New bc_om_style_component_table_cell
                            ocell.row = 2
                            ocell.col = 1
                            ocell.paragraph = 1

                            ocell.style = CStr(worddocument.tables(i).range.cells(2).range.paragraphs(1).range.style.namelocal)
                            ocell.text = "no image available"

                            opara.table_cells.Add(ocell)
                            ocomponents.components(ocomponents.components.Count - 1).paragraphs.Add(opara)
                        Catch ex As Exception
                            Dim ocom = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Table: " + CStr(i) + " Failed " + ex.Message)
                        End Try
                        Dim ocomm As New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Table: " + CStr(i) + " of " + CStr(table_count))


                    End If
                Next

            Else
                REM main document body
                For i = 1 To worddocument.paragraphs.count
                    Try
                        parse_paragraph(i, ocomponents, componetize_styles, ocomponent, body_style, componetize_styles.rtf, componetize_styles.html)
                        If componetize_error = True Then
                            componetize_error_text = componetize_error_text + " paragraph: " + CStr(i)
                            Exit Function
                        End If
                    Catch ex As Exception
                        Dim ocomm As New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Couldnt compontize paragraph: " + CStr(i) + ": " + ex.Message)
                    End Try
                Next



                'REM check if text boxes need to be componetized
                If componetize_styles.search_text_boxes = True Then
                    Dim ocomm As New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Compontising Text Boxes")
                    i = 1
                    For Each shp In worddocument.Shapes
                        If shp.Type = 17 Then
                            For j = 1 To shp.TextFrame.TextRange.Paragraphs.count

                                parse_paragraph_in_text_box(shp.TextFrame.TextRange.Paragraphs(j), i, j, ocomponents, componetize_styles, ocomponent, body_style, componetize_styles.rtf, componetize_styles.html)
                                If componetize_error = True Then
                                    componetize_error_text = componetize_error_text + " shape: " + CStr(j) + " paragraph: " + CStr(i)
                                    Exit Function
                                End If
                            Next
                            i = i + 1
                        End If
                    Next shp

                End If
            End If
            'REM check if header/footer need to be componetized
            'If componetize_styles.search_headers = True Then

            'End If

            worddocument.application.displayalerts = True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_word", "componetize", bc_cs_error_codes.USER_DEFINED, ex.Message)
            componetize = False
        Finally
            If componetize_error = True Then
                componetize = False
                Dim ocomm = New bc_cs_activity_log("bc_ao_word", "componetize", bc_cs_activity_codes.COMMENTARY, "Style componetize error: " + componetize_error_text)
            Else
                componetize = True
            End If
        End Try
    End Function



    Public Function generate_new_bookmark(ByVal start_id As Integer) As String
        Try
            While worddocument.bookmarks.exists("bc_comp_t" + CStr(start_id))
                start_id = start_id + 1
            End While
            generate_new_bookmark = "bc_comp_t" + CStr(start_id)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_word", "generate_new_bookmark", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Public Function bookmark_already_in_table(ByVal table_id As Integer) As String
        Try
            Dim na As String
            bookmark_already_in_table = ""
            For i = 1 To worddocument.Tables(table_id).Range.Bookmarks.Count
                na = worddocument.Tables(table_id).Range.Bookmarks(i).Name
                If Len(na) > 9 Then
                    If Left(na, 9) = "bc_comp_t" Then
                        bookmark_already_in_table = na
                        Exit Function
                    End If
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_word", "bookmark_already_in_table", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    'Private Sub parse_paragraph(ByRef paragraph_number As Integer, ByRef ocomponents As bc_om_document_components, ByVal componetize_styles As bc_om_componetize_styles_for_template, ByRef ocomponent As bc_om_style_component, ByRef body_style As ArrayList, Optional ByVal header As Boolean = False, Optional ByVal header_location As String = "")
    '    Try
    '        Dim opara As bc_om_style_component_paragraph
    '        REM
    '        If worddocument.paragraphs(paragraph_number).Range.tables.count > 0 And componetize_styles.table_as_image = True Then
    '            Exit Sub
    '        End If
    '        REM
    '        If worddocument.paragraphs(paragraph_number).Range.tables.count > 0 And componetize_styles.tables = False Then
    '            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_paragraph", bc_cs_activity_codes.COMMENTARY, "Paragraph in a table and tables suppressed: " + CStr(paragraph_number) + ": " + worddocument.paragraphs(paragraph_number).Range.text)
    '            Exit Sub
    '        End If
    '        If worddocument.paragraphs(paragraph_number).Range.InlineShapes.Count = 1 And componetize_styles.images = False Then
    '            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_paragraph", bc_cs_activity_codes.COMMENTARY, "Paragraph in a chart and chart suppressed: " + CStr(paragraph_number) + ": " + worddocument.paragraphs(paragraph_number).Range.text)
    '            Exit Sub
    '        End If


    '        Dim style As String
    '        style = worddocument.paragraphs(paragraph_number).Range.Style.namelocal
    '        If componetize_styles.all_styles = True Then
    '            Dim st As New bc_om_componetize_style
    '            st.header_style = "All"
    '            componetize_styles.styles.Add(st)
    '        End If

    '        For j = 0 To componetize_styles.styles.Count - 1
    '            If style = componetize_styles.styles(j).header_style Or componetize_styles.all_styles = True Then
    '                ocomponent = New bc_om_style_component
    '                ocomponent.page_no = worddocument.paragraphs(paragraph_number).Range.Information(3)
    '                ocomponent.style = worddocument.paragraphs(paragraph_number).Range.Style.namelocal

    '                If worddocument.paragraphs(paragraph_number).Range.tables.count = 1 Then
    '                    ocomponent.btable = True
    '                    parse_table(worddocument, paragraph_number, ocomponent, componetize_styles.images, componetize_styles.table_as_image)
    '                ElseIf worddocument.paragraphs(paragraph_number).Range.InlineShapes.Count > 0 Then
    '                    ocomponent.bimage = True
    '                    ocomponent.text = "Chart"
    '                    ocomponent.image = get_image_from_inline_shape(worddocument.paragraphs(paragraph_number).Range.InlineShapes(1))
    '                Else
    '                    If componetize_styles.all_styles = False Or Trim(worddocument.paragraphs(paragraph_number).Range.text.length) > 1 Then
    '                        REM  and here
    '                        ocomponent.text = worddocument.paragraphs(paragraph_number).Range.text
    '                        ocomponent.text = tidy_up_text(ocomponent.text)
    '                        'JL: Parse Hyperlinks
    '                        'Wrap links in an anchor tag
    '                        Dim td, hy, nt As String
    '                        For i = 1 To worddocument.paragraphs(paragraph_number).Range.Hyperlinks.Count
    '                            td = worddocument.paragraphs(paragraph_number).Range.Hyperlinks(i).TextToDisplay
    '                            hy = worddocument.paragraphs(paragraph_number).Range.Hyperlinks(i).Address
    '                            nt = "<a href='" + hy + "' class='whyread_link'>" + td + "</a>"
    '                            ocomponent.text = Replace(ocomponent.text, td, nt)
    '                        Next
    '                    Else
    '                        Exit Sub
    '                    End If
    '                End If
    '                If componetize_styles.rtf = True Or componetize_styles.html = True Then
    '                    get_selection_in_rtf_and_html_compressed(ocomponent.rtf, ocomponent.compressed_html, componetize_styles.rtf, componetize_styles.html, ocomponent.btable, paragraph_number)
    '                ElseIf componetize_styles.preview_rtf = True And (ocomponent.bimage = True Or ocomponent.btable = True) Then
    '                    get_selection_in_rtf_and_html_compressed(ocomponent.rtf, ocomponent.compressed_html, True, False, ocomponent.btable, paragraph_number)
    '                End If
    '                If ocomponent.btable = True Then
    '                    paragraph_number = paragraph_number + worddocument.paragraphs(paragraph_number).range.tables(1).range.paragraphs.count() - 1
    '                End If
    '                ocomponents.components.Add(ocomponent)
    '                Exit Sub
    '            End If
    '        Next
    '        REM now do body
    '        If IsNothing(ocomponent) Or componetize_styles.all_styles = True Then
    '            Exit Sub
    '        End If
    '        If componetize_styles.header_styles_only = True Then
    '            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_paragraph", bc_cs_activity_codes.COMMENTARY, "Paragraph is not a header style and header styles suppressed: " + CStr(paragraph_number) + ": " + worddocument.paragraphs(paragraph_number).Range.text)
    '            Exit Sub
    '        End If
    '        opara = New bc_om_style_component_paragraph
    '        opara.style = worddocument.paragraphs(paragraph_number).Range.Style.namelocal

    '        opara.page_no = worddocument.paragraphs(paragraph_number).Range.Information(3)
    '        If worddocument.paragraphs(paragraph_number).Range.tables.count = 1 Then
    '            opara.btable = True
    '            parse_table(worddocument, paragraph_number, opara, componetize_styles.images, componetize_styles.table_as_image)

    '        ElseIf worddocument.paragraphs(paragraph_number).Range.InlineShapes.Count > 0 Then
    '            opara.bimage = True
    '            opara.text = "Chart"
    '            opara.image = get_image_from_inline_shape(worddocument.paragraphs(paragraph_number).Range.InlineShapes(1))

    '        Else
    '            If worddocument.paragraphs(paragraph_number).Range.text.length > 1 Then
    '                REM  and here
    '                opara.text = worddocument.paragraphs(paragraph_number).Range.text
    '                If Asc(opara.text) <> 13 Then
    '                    opara.text = tidy_up_text(opara.text)
    '                Else
    '                    opara.text = ""
    '                End If
    '                'JL: Parse Hyperlinks
    '                'Wrap links in an anchor tag
    '                Dim td, hy, nt As String
    '                For i = 1 To worddocument.paragraphs(paragraph_number).Range.Hyperlinks.Count
    '                    td = worddocument.paragraphs(paragraph_number).Range.Hyperlinks(i).TextToDisplay
    '                    hy = worddocument.paragraphs(paragraph_number).Range.Hyperlinks(i).Address
    '                    nt = "<a href='" + hy + "' class='whyread_link'>" + td + "</a>"
    '                    opara.text = Replace(opara.text, td, nt)
    '                Next


    '            End If
    '        End If
    '        If opara.text <> "" Then
    '            If componetize_styles.rtf = True Or componetize_styles.html = True Then
    '                get_selection_in_rtf_and_html_compressed(opara.rtf, opara.compressed_html, componetize_styles.rtf, componetize_styles.html, opara.btable, paragraph_number)
    '            ElseIf componetize_styles.preview_rtf = True And (opara.bimage = True Or opara.btable = True) Then
    '                get_selection_in_rtf_and_html_compressed(opara.rtf, opara.compressed_html, True, False, opara.btable, paragraph_number)

    '            End If
    '            If opara.btable = True Then
    '                paragraph_number = paragraph_number + worddocument.paragraphs(paragraph_number).range.tables(1).range.paragraphs.count - 1
    '            End If
    '            ocomponent.paragraphs.Add(opara)
    '        End If

    '    Catch ex As Exception
    '        Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_paragraph", bc_cs_activity_codes.COMMENTARY, "Cant parse paragraph: " + CStr(paragraph_number) + ": " + ex.Message)
    '    End Try
    'End Sub

    Private Sub parse_paragraph(ByRef paragraph_number As Integer, ByRef ocomponents As bc_om_document_components, ByVal componetize_styles As bc_om_componetize_styles_for_template, ByRef ocomponent As bc_om_style_component, ByRef body_style As ArrayList, Optional ByVal header As Boolean = False, Optional ByVal header_location As String = "")
        Try
            Dim opara As bc_om_style_component_paragraph
            REM
            If worddocument.paragraphs(paragraph_number).Range.tables.count > 0 And componetize_styles.table_on_demand = True Then
                Exit Sub
            End If
            REM
            If worddocument.paragraphs(paragraph_number).Range.tables.count > 0 And componetize_styles.tables = False Then
                Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_paragraph", bc_cs_activity_codes.COMMENTARY, "Paragraph in a table and tables suppressed: " + CStr(paragraph_number) + ": " + worddocument.paragraphs(paragraph_number).Range.text)
                Exit Sub
            End If
            If worddocument.paragraphs(paragraph_number).Range.InlineShapes.Count = 1 And componetize_styles.images = False Then
                Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_paragraph", bc_cs_activity_codes.COMMENTARY, "Paragraph in a chart and chart suppressed: " + CStr(paragraph_number) + ": " + worddocument.paragraphs(paragraph_number).Range.text)
                Exit Sub
            End If


            Dim style As String

            style = worddocument.paragraphs(paragraph_number).Range.Style.namelocal
            If componetize_styles.all_styles = True Then
                Dim st As New bc_om_componetize_style
                st.header_style = "All"
                componetize_styles.styles.Add(st)
            End If

            For j = 0 To componetize_styles.styles.Count - 1
                If style = componetize_styles.styles(j).header_style Or componetize_styles.all_styles = True Then
                    ocomponent = New bc_om_style_component
                    ocomponent.page_no = worddocument.paragraphs(paragraph_number).Range.Information(3)
                    ocomponent.style = worddocument.paragraphs(paragraph_number).Range.Style.namelocal

                    If worddocument.paragraphs(paragraph_number).Range.tables.count = 1 Then
                        ocomponent.btable = True
                        parse_table(worddocument, paragraph_number, ocomponent, componetize_styles.images, componetize_styles.table_as_image)

                        If componetize_styles.table_as_image = True And worddocument.paragraphs(paragraph_number).Range.tables(1).rows.count > 1 Then
                            REM take table as image as well
                            REM put a switch in for this later
                            Dim bytedoc As Byte() = Nothing
                            If copycompleTableAsImage(paragraph_number, bytedoc, False) = True Then
                                ocomponent.image = bytedoc
                                ocomponent.bimage = True
                                ocomponent.text = "Table"
                            End If
                        End If

                    ElseIf worddocument.paragraphs(paragraph_number).Range.InlineShapes.Count > 0 Then
                        ocomponent.bimage = True
                        ocomponent.text = "Chart"
                        ocomponent.image = get_image_from_inline_shape(worddocument.paragraphs(paragraph_number).Range.InlineShapes(1))
                    Else
                        If componetize_styles.all_styles = False Or Trim(worddocument.paragraphs(paragraph_number).Range.text.length) > 1 Then
                            REM  and here
                            ocomponent.text = worddocument.paragraphs(paragraph_number).Range.text
                            ocomponent.text = tidy_up_text(ocomponent.text)

                            If worddocument.paragraphs(paragraph_number).Range.Hyperlinks.Count > 0 Then
                                ocomponent.text = parse_hyperlinks_in_range(ocomponent.text, worddocument.paragraphs(paragraph_number).Range)
                            End If

                        Else
                            Exit Sub
                        End If
                    End If
                    If componetize_styles.rtf = True Or componetize_styles.html = True Then
                        get_selection_in_rtf_and_html_compressed(ocomponent.rtf, ocomponent.compressed_html, componetize_styles.rtf, componetize_styles.html, ocomponent.btable, paragraph_number)
                    ElseIf componetize_styles.preview_rtf = True And (ocomponent.bimage = True Or ocomponent.btable = True) Then
                        get_selection_in_rtf_and_html_compressed(ocomponent.rtf, ocomponent.compressed_html, True, False, ocomponent.btable, paragraph_number)
                    End If
                    If ocomponent.btable = True Then
                        paragraph_number = paragraph_number + worddocument.paragraphs(paragraph_number).range.tables(1).range.paragraphs.count() - 1
                    End If
                    ocomponents.components.Add(ocomponent)
                    Exit Sub
                End If
            Next
            REM now do body
            If IsNothing(ocomponent) Or componetize_styles.all_styles = True Then
                Exit Sub
            End If
            If componetize_styles.header_styles_only = True Then
                Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_paragraph", bc_cs_activity_codes.COMMENTARY, "Paragraph is not a header style and header styles suppressed: " + CStr(paragraph_number) + ": " + worddocument.paragraphs(paragraph_number).Range.text)
                Exit Sub
            End If
            opara = New bc_om_style_component_paragraph
            opara.style = worddocument.paragraphs(paragraph_number).Range.Style.namelocal

            opara.page_no = worddocument.paragraphs(paragraph_number).Range.Information(3)
            If worddocument.paragraphs(paragraph_number).Range.tables.count = 1 Then
                opara.btable = True
                parse_table(worddocument, paragraph_number, opara, componetize_styles.images, componetize_styles.table_as_image)
                If componetize_styles.table_as_image = True Then
                    REM take table as image as well
                    REM put a switch in for this later
                    Dim bytedoc As Byte() = Nothing
                    If copycompleTableAsImage(paragraph_number, bytedoc, False) = True And worddocument.paragraphs(paragraph_number).Range.tables(1).rows.count > 1 Then
                        opara.image = bytedoc
                        opara.bimage = True
                        opara.text = "Table"
                    End If
                End If
            ElseIf worddocument.paragraphs(paragraph_number).Range.InlineShapes.Count > 0 Then
                opara.bimage = True
                opara.text = "Chart"
                opara.image = get_image_from_inline_shape(worddocument.paragraphs(paragraph_number).Range.InlineShapes(1))

            Else
                If worddocument.paragraphs(paragraph_number).Range.text.length > 1 Then
                    REM  and here
                    opara.text = worddocument.paragraphs(paragraph_number).Range.text
                    If Asc(opara.text) <> 13 Then
                        opara.text = tidy_up_text(opara.text)
                    Else
                        opara.text = ""
                    End If

                    If worddocument.paragraphs(paragraph_number).Range.Hyperlinks.Count > 0 Then
                        opara.text = parse_hyperlinks_in_range(opara.text, worddocument.paragraphs(paragraph_number).Range)
                    End If
                    'For i = 1 To worddocument.paragraphs(paragraph_number).Range.Hyperlinks.Count
                    '    td = worddocument.paragraphs(paragraph_number).Range.Hyperlinks(i).TextToDisplay
                    '    hy = worddocument.paragraphs(paragraph_number).Range.Hyperlinks(i).Address
                    '    nt = "<a href='" + hy + "' class='whyread_link'>" + td + "</a>"
                    '    'opara.text = Replace(opara.text, td, nt)
                    '    opara.text = Replace(opara.text, td, nt, 1, 1)

                    'Next


                End If
            End If
            If opara.text <> "" Then
                If componetize_styles.rtf = True Or componetize_styles.html = True Then
                    get_selection_in_rtf_and_html_compressed(opara.rtf, opara.compressed_html, componetize_styles.rtf, componetize_styles.html, opara.btable, paragraph_number)
                ElseIf componetize_styles.preview_rtf = True And (opara.bimage = True Or opara.btable = True) Then
                    get_selection_in_rtf_and_html_compressed(opara.rtf, opara.compressed_html, True, False, opara.btable, paragraph_number)

                End If
                If opara.btable = True Then
                    paragraph_number = paragraph_number + worddocument.paragraphs(paragraph_number).range.tables(1).range.paragraphs.count - 1
                End If
                ocomponent.paragraphs.Add(opara)
            End If

        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_paragraph", bc_cs_activity_codes.COMMENTARY, "Cant parse paragraph: " + CStr(paragraph_number) + ": " + ex.Message)
        End Try
    End Sub

    Private Function parse_hyperlinks_in_range(tx As String, range As Object) As String
        Dim itx As String
        itx = tx
        Try

            Dim hwords(100) As Integer
            Dim hwc As Integer

            Dim cc As Integer
            cc = 1
            hwc = 0
            If range.Hyperlinks.Count > 0 Then

                For i = 1 To range.Words.Count
                    Try

                        If range.Words(i).Fields(1).Type = 88 Then
                            hwc = hwc + 1
                            hwords(hwc) = cc
                        End If
                        cc = cc + range.Words(i).text.length
                    Catch
                        cc = cc + range.Words(i).text.length
                    End Try
                Next

                Dim td, hy, nt As String
                Dim hco As Integer
                hco = range.Hyperlinks.Count
                For i = 1 To range.Hyperlinks.Count

                    td = range.Hyperlinks(hco - i + 1).range.text
                    hy = range.Hyperlinks(hco - i + 1).Address
                    nt = "<a href='" + hy + "' class='whyread_link'>" + td + "</a>"
                    Dim st As String
                    st = tx.Substring(0, hwords(hco - i + 1) - 1)
                    tx = st + Replace(tx, td, nt, hwords(hco - i + 1), 1)
                
                Next
            End If
        Catch ex As Exception
            parse_hyperlinks_in_range = itx

            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_hyperlinks", bc_cs_activity_codes.COMMENTARY, "Cant parse hyperlink: " + itx + ": " + ex.Message)
        Finally
            parse_hyperlinks_in_range = tx
        End Try
    End Function

    'Private Function parse_hyperlinks(tx As String, paragraph_number As Integer) As String
    '    Dim itx As String
    '    itx = tx
    '    Try
    '        Dim hwords(100) As Integer
    '        Dim hwc As Integer

    '        Dim cc As Integer
    '        cc = 1
    '        hwc = 0
    '        If worddocument.paragraphs(paragraph_number).Range.Hyperlinks.Count > 0 Then
    '            For i = 1 To worddocument.paragraphs(paragraph_number).Range.Words.Count
    '                Try

    '                    If worddocument.paragraphs(paragraph_number).Range.Words(i).Fields(1).Type = 88 Then
    '                        hwc = hwc + 1
    '                        hwords(hwc) = cc
    '                    End If
    '                    cc = cc + worddocument.paragraphs(paragraph_number).Range.Words(i).text.length
    '                Catch
    '                    cc = cc + worddocument.paragraphs(paragraph_number).Range.Words(i).text.length
    '                End Try
    '            Next

    '            Dim td, hy, nt As String
    '            Dim hco As Integer
    '            hco = worddocument.paragraphs(paragraph_number).Range.Hyperlinks.Count
    '            For i = 1 To worddocument.paragraphs(paragraph_number).Range.Hyperlinks.Count
    '                td = worddocument.paragraphs(paragraph_number).Range.Hyperlinks(hco - i + 1).range.text
    '                hy = worddocument.paragraphs(paragraph_number).Range.Hyperlinks(hco - i + 1).Address
    '                nt = "<a href='" + hy + "' class='whyread_link'>" + td + "</a>"
    '                Dim st As String
    '                st = tx.Substring(0, hwords(hco - i + 1) - 1)
    '                tx = st + Replace(tx, td, nt, hwords(hco - i + 1), 1)
    '            Next
    '        End If
    '    Catch ex As Exception
    '        parse_hyperlinks = itx

    '        Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_hyperlinks", bc_cs_activity_codes.COMMENTARY, "Cant parse hyperlink: " + CStr(paragraph_number) + ":" + itx + ": " + ex.Message)
    '    Finally
    '        parse_hyperlinks = tx
    '    End Try
    'End Function


    Private Sub parse_paragraph_in_text_box(obj As Object, ByVal shape_id As Integer, ByRef paragraph_number As Integer, ByRef ocomponents As bc_om_document_components, ByVal componetize_styles As bc_om_componetize_styles_for_template, ByRef ocomponent As bc_om_style_component, ByRef body_style As ArrayList, Optional ByVal header As Boolean = False, Optional ByVal header_location As String = "")
        Try
            REM take everything as lead
            Dim opara As bc_om_style_component_paragraph
            If obj.Range.tables.count > 0 And componetize_styles.tables = False Then
                Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_paragraph_in_text_box", bc_cs_activity_codes.COMMENTARY, "Paragraph in a table and tables suppressed: " + CStr(shape_id) + ":" + CStr(paragraph_number) + ": " + worddocument.paragraphs(paragraph_number).Range.text)
                Exit Sub
            End If
            If obj.Range.InlineShapes.Count = 1 And componetize_styles.images = False Then
                Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_paragraph_in_text_box", bc_cs_activity_codes.COMMENTARY, "Paragraph in a chart and chart suppressed: " + CStr(shape_id) + ":" + CStr(paragraph_number) + ": " + worddocument.paragraphs(paragraph_number).Range.text)
                Exit Sub
            End If
            Dim style As String
            'If componetize_styles.all_styles = True Then
            '    Dim st As New bc_om_componetize_style
            '    st.header_style = "All"
            '    componetize_styles.styles.Add(st)
            'End If
            style = obj.Range.Style.namelocal
            'For j = 0 To componetize_styles.styles.Count - 1
            '    If style = componetize_styles.styles(j).header_style Or componetize_styles.all_styles = True Then
            ocomponent = New bc_om_style_component
            ocomponent.page_no = obj.Range.Information(3)
            ocomponent.style = style

            If obj.Range.tables.count = 1 Then
                ocomponent.btable = True
                parse_table_in_shape(obj, ocomponent, componetize_styles.images)
            ElseIf obj.Range.InlineShapes.Count = 1 Then
                ocomponent.bimage = True
                ocomponent.text = "Chart"
                ocomponent.image = get_image_from_inline_shape(obj.Range.InlineShapes(1))
            Else
                If componetize_styles.all_styles = False Or Trim(obj.Range.text.length) > 1 Then
                    ocomponent.text = obj.Range.text
                    ocomponent.text = tidy_up_text(ocomponent.text)
                    If ocomponent.text = "" Then
                        Exit Sub
                    End If
                    If obj.Range.Hyperlinks.Count > 0 Then
                        ocomponent.text = parse_hyperlinks_in_range(ocomponent.text, obj.Range)
                    End If
                Else
                    Exit Sub
                End If
            End If
            If componetize_styles.rtf = True Or componetize_styles.html = True Then
                get_selection_in_rtf_and_html_compressed_in_text_box(obj, ocomponent.rtf, ocomponent.compressed_html, componetize_styles.rtf, componetize_styles.html, ocomponent.btable, paragraph_number, shape_id)
            ElseIf componetize_styles.preview_rtf = True And (ocomponent.bimage = True Or ocomponent.btable = True) Then

                get_selection_in_rtf_and_html_compressed_in_text_box(obj, ocomponent.rtf, ocomponent.compressed_html, True, False, ocomponent.btable, paragraph_number, shape_id)
            End If

            If ocomponent.btable = True Then
                paragraph_number = paragraph_number + obj.Range.tables(1).range.paragraphs.count - 1
            End If
            ocomponent.text_box_id = shape_id
            ocomponents.components.Add(ocomponent)
            Exit Sub
            '    End If
            ''Next
            'REM now do body
            'If IsNothing(ocomponent) Or componetize_styles.all_styles = True Then
            '    Exit Sub
            'End If
            'If componetize_styles.header_styles_only = True Then
            '    Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_paragraph_in_text_box", bc_cs_activity_codes.COMMENTARY, "Paragraph is not a header style and header styles suppressed: " + CStr(shape_id) + ":" + CStr(paragraph_number) + ": " + worddocument.paragraphs(paragraph_number).Range.text)
            '    Exit Sub
            'End If
            'opara = New bc_om_style_component_paragraph
            'opara.style = worddocument.application.selection.paragraphs(paragraph_number).Range.Style.namelocal

            'opara.page_no = worddocument.application.selection.paragraphs(paragraph_number).Range.Information(3)
            'If worddocument.application.selection.paragraphs(paragraph_number).Range.tables.count = 1 Then
            '    opara.btable = True
            '    parse_table_in_shape(worddocument, paragraph_number, shape_id, opara, componetize_styles.images)

            'ElseIf worddocument.application.selection.paragraphs(paragraph_number).Range.InlineShapes.Count = 1 Then
            '    opara.bimage = True
            '    opara.text = "Chart"
            '    opara.image = get_image_from_inline_shape(worddocument.application.selection.paragraphs(paragraph_number).Range.InlineShapes(1))

            'Else
            '    If worddocument.application.selection.paragraphs(paragraph_number).Range.text.length > 1 Then
            '        opara.text = worddocument.application.selection.paragraphs(paragraph_number).Range.text
            '        If Asc(opara.text) <> 13 Then
            '            opara.text = tidy_up_text(opara.text)
            '        Else
            '            opara.text = ""
            '        End If
            '    End If
            'End If
            'If opara.text <> "" Then
            '    If componetize_styles.rtf = True Or componetize_styles.html = True Then
            '        REM urmm
            '        get_selection_in_rtf_and_html_compressed_in_text_box(opara.rtf, opara.compressed_html, componetize_styles.rtf, componetize_styles.html, opara.btable, paragraph_number, shape_id)
            '        REM 
            '    End If
            '    If opara.btable = True Then
            '        paragraph_number = paragraph_number + worddocument.application.selection.range.tables(1).range.paragraphs.count - 1
            '    End If
            '    ocomponent.paragraphs.Add(opara)
            'End If

        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_paragraph_in_text_boxs", bc_cs_activity_codes.COMMENTARY, "Cant parse paragraph: " + CStr(shape_id) + ":" + CStr(paragraph_number) + ": " + ex.Message)
        End Try
    End Sub

    REM NEW
    Function copycompleTableAsImage(ByVal paragraph As Integer, ByRef bytedoc As Byte(), ByVal take_title As Boolean) As Boolean
        ' *** Copies all of the tables in the document and pastes them as image files ***
        '
        Dim canvas As Object = Nothing

        Try
            Dim tbl As Object
            Dim tblWidth As Integer
            Dim tblHeight As Integer
            Dim tblShape As Object
            Dim tblShapeWidth As Integer
            Dim cropRight As Integer


            ' Create a blank page to be used as the canvas
            canvas = word.application.Documents.Add

            ' Set the selection to the beginning of the document
            word.ActiveWindow.Selection.GoTo(What:=0, Which:=1)


            tbl = worddocument.paragraphs(paragraph).range.tables(1)


            tbl.Select()

            ' Setup the canvas page to paste tables into so that original document is not modified
            If SetPageOrientation(canvas) = False Then
                canvas.Close(SaveChanges:=0)
                copycompleTableAsImage = False
                Dim ocomm As New bc_cs_activity_log("bc_ao_word", "CopytableAsImageSave", bc_cs_activity_codes.COMMENTARY, "SetPageOrientation failed: " + CStr(paragraph))
                copycompleTableAsImage = False
                Exit Function
            End If

            Dim psuccess As Boolean = False
            While psuccess = False
                Try
                    worddocument.application.Selection.Copy()


                    ' Paste as HTML to preserve formatting
                    canvas.Content.PasteSpecial(DataType:=10)
                    psuccess = True
                Catch
                    Thread.CurrentThread.Sleep(10)
                    Dim ocomm As New bc_cs_activity_log("bc_ao_word", "CopytableAsImageSave", bc_cs_activity_codes.COMMENTARY, "Retry copy for para: " + CStr(paragraph))

                End Try
            End While
          
            ' Get the width of the table from the first row
            tblWidth = canvas.Tables(1).Cell(1, 1).Width

            If take_title = False Then
                canvas.Tables(1).Range.Cells(1).Select()
                word.Selection.Rows.Delete()
            End If

            canvas.Tables(1).Select()
            word.Selection.Cut()
            tblShape = canvas.Tables.Add(Range:=canvas.Range(0, 0), NumRows:=1, NumColumns:=1, DefaultTableBehavior:=0)

            ' Position the table in the top-right corner
            With tblShape.Rows
                .RelativeVerticalPosition = 0
                .HorizontalPosition = 0
                .RelativeHorizontalPosition = 0
                .VerticalPosition = 0
            End With

            ' Initialise the table height/width
            With tblShape
                .Rows(1).HeightRule = 1
                .Rows(1).Height = 5
                .Columns(1).Width = tblWidth
                .TopPadding = 2 ' New...
                .LeftPadding = 2 ' New...
                .RightPadding = 0 ' New...
                .BottomPadding = 0 ' New...

            End With


            ' Paste table into container table
            tblShape.Rows(1).Cells(1).Range.PasteSpecial(DataType:=10)

            ' Get the width of the first table row to get the entire table width
            tblHeight = tblShape.Rows(1).Height

            tblShape.Select()
            canvas.application.Selection.Cut()

            ' Paste as Image
            canvas.Content.PasteSpecial(DataType:=9)


            ' New...
            ' Only crop if necesary
            tblShapeWidth = canvas.Shapes(1).Width
            If tblShapeWidth > tblWidth Then
                cropRight = tblShapeWidth - tblWidth
            Else
                cropRight = 0
            End If


            ' Resize the image
            canvas.Shapes(1).ConvertToInlineShape()

            With canvas.InlineShapes(1)
                .Width = tblWidth
                .Height = tblHeight
                .ScaleWidth = 100
                .ScaleHeight = 100
                .PictureFormat.cropRight = cropRight - 20 ' New
                '.PictureFormat.Crop.ShapeWidth = tblWidth + 20
            End With

            Dim bmp As System.Drawing.Bitmap
            Dim success As Boolean = False



            Try
                Dim data As IDataObject
                Dim fn As String
                fn = bc_cs_central_settings.local_repos_path + "tmp" + CStr(paragraph) + ".png"
                psuccess = False
                While psuccess = False
                    Try
                        canvas.InlineShapes(1).range.copyaspicture()
                        data = Clipboard.GetDataObject()
                        psuccess = True
                    Catch
                        Thread.CurrentThread.Sleep(10)
                        Dim ocomm As New bc_cs_activity_log("bc_ao_word", "CopytableAsImageSave", bc_cs_activity_codes.COMMENTARY, "Retry data for para: " + CStr(paragraph))
                    End Try
                End While
                bmp = CType(data.GetData(GetType(System.Drawing.Bitmap)), System.Drawing.Bitmap)
                bmp.Save(fn, System.Drawing.Imaging.ImageFormat.Png)
                Dim fss As New bc_cs_file_transfer_services
                fss.write_document_to_bytestream(fn, bytedoc, Nothing)

                fss.delete_file(fn)

            Catch ex As Exception
                Dim ocomm As New bc_cs_activity_log("bc_ao_word", "CopytableAsImageSave a:", bc_cs_activity_codes.COMMENTARY, ex.Message)
                copycompleTableAsImage = False
                componetize_error = True
                componetize_error_text = "Failed to copy table as image"
            End Try
            ' Close the canvas once images have been rendered
            canvas.Close(SaveChanges:=0)
            Dim fs As New bc_cs_file_transfer_services

            copycompleTableAsImage = True

        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "CopycompletetableAsImage b:", bc_cs_activity_codes.COMMENTARY, ex.Message)
            If Not IsNothing(canvas) Then
                canvas.Close(SaveChanges:=0)
            End If
            copycompleTableAsImage = False
            componetize_error = True
            componetize_error_text = "Failed to copy table as image"


        End Try

    End Function



    Private Function get_table_as_image(ByVal paragraph_number As Integer) As Byte()

        Try
            'Dim word As Object
            Dim width As Double
            'Dim tword As Objects
            Dim tdoc As Object
            Dim rows As Integer


            width = worddocument.paragraphs(paragraph_number).Range.Tables(1).Cell(1, 1).width
            Dim r As Object
            r = worddocument.paragraphs(paragraph_number).Range.Tables(1).Range
            r.Start = worddocument.paragraphs(paragraph_number).Range.Tables(1).Rows(2).Range.Start
            rows = worddocument.paragraphs(paragraph_number).Range.Tables(1).rows.count
            r.End = worddocument.paragraphs(paragraph_number).Range.Tables(1).Rows(rows).Range.end

            r.copyaspicture()

            tdoc = worddocument.application.Documents.Add(Template:="Normal", NewTemplate:=False, DocumentType:=0)

            tdoc.range.PasteSpecial(Link:=False, DataType:=0, Placement _
            :=0, DisplayAsIcon:=False)
            tdoc.paragraphs(1).range.inlineshapes(1).width = width

            tdoc.paragraphs(1).range.inlineshapes(1).range.copyaspicture()
            tdoc.range.PasteSpecial(Link:=False, DataType:=9, Placement _
           :=0, DisplayAsIcon:=False)

            get_table_as_image = get_image_from_inline_shape(tdoc.paragraphs(1).range.inlineshapes(1))

            'tdoc.close()
            'word.activedocument.bookmarks("tmp").range.width = width

            '   word.activedocument.bookmarks("tmp").range.PasteSpecial(Link:=False, DataType:=9, _
            'Placement:=2, DisplayAsIcon:=False)
        Catch ex As Exception
            get_table_as_image = Nothing
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "get_table_as_image", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try
    End Function


    Function get_image_from_inline_shape(ByVal ils As Object) As Byte()

        Try
            REM PR NOV 2018 
            Dim oh As New bc_ao_word_html_componetize(worddocument)
            Return oh.CopyFromClipboardInlineShape(ils)
            Exit Function
            REM
            Dim data As IDataObject
            Dim bmp As System.Drawing.Bitmap
            Dim fs As New bc_cs_file_transfer_services
            Dim fn As String = bc_cs_central_settings.local_repos_path + "tmpimage.png"
            Dim success As Boolean = False

            Dim bytedoc As Byte() = Nothing

            While Not success
                Try
                    ils.range.copyaspicture()
                    data = Clipboard.GetDataObject()
                    bmp = CType(data.GetData(GetType(System.Drawing.Bitmap)), System.Drawing.Bitmap)
                    bmp.Save(fn, System.Drawing.Imaging.ImageFormat.Png)
                    fs.write_document_to_bytestream(fn, bytedoc, Nothing, True)
                    fs.delete_file(fn)
                    success = True

                Catch
                    Thread.CurrentThread.Sleep(100)
                End Try
            End While

            Return bytedoc
        Catch ex As Exception
            componetize_error = True
            componetize_error_text = "get_image_from_inline_shape: " + ex.Message
            Return Nothing
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "componetize_error: get_image_from_inline_shape", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try
    End Function





    REM =======

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



    Sub parse_table(ByVal adoc As Object, ByVal paragraph_number As Integer, ByRef opara As bc_om_style_component_base_class, ByVal bimages As Boolean, ByVal table_as_image As Boolean)
        Try


            'If table_as_image = True And adoc.paragraphs(paragraph_number).range.tables(1).range.cells.count > 1 Then
            '    Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_table", bc_cs_activity_codes.COMMENTARY, "Starting table:" + CStr(paragraph_number))
            '    opara.btable = True
            '    opara.style = worddocument.paragraphs(paragraph_number).Range.Style.namelocal
            '    opara.table_cells = New List(Of bc_om_style_component_table_cell)
            '    opara.text = "Table Image"
            '    REM ===
            '    Dim ocell As bc_om_style_component_table_cell
            '    REM take text for first cell
            '    ocell = New bc_om_style_component_table_cell
            '    ocell.row = adoc.paragraphs(paragraph_number).range.tables(1).range.cells(1).rowindex
            '    ocell.col = adoc.paragraphs(paragraph_number).range.tables(1).range.cells(1).columnindex
            '    ocell.paragraph = 1
            '    ocell.bimage = False
            '    ocell.text = adoc.paragraphs(paragraph_number).range.tables(1).range.cells(1).range.paragraphs(1).range.text
            '    If Asc(ocell.text) <> 13 Then
            '        ocell.text = tidy_up_text(ocell.text)
            '    Else
            '        ocell.text = ""
            '    End If
            '    opara.table_cells.Add(ocell)


            '    ocell.style = CStr(adoc.paragraphs(paragraph_number).range.tables(1).range.cells(2).range.paragraphs(1).range.style.namelocal)
            '    opara.table_cells.Add(ocell)
            '    REM rest take as image
            '    ocell = New bc_om_style_component_table_cell
            '    ocell.row = adoc.paragraphs(paragraph_number).range.tables(1).range.cells(2).rowindex
            '    ocell.col = adoc.paragraphs(paragraph_number).range.tables(1).range.cells(2).columnindex
            '    ocell.paragraph = 1
            '    ocell.bimage = True
            '    ocell.style = CStr(adoc.paragraphs(paragraph_number).range.tables(1).range.cells(2).range.paragraphs(1).range.style.namelocal)
            '    ocell.image = get_table_as_image(paragraph_number)
            '    If IsNothing(ocell.image) = True Then
            '        Dim comm As New bc_cs_activity_log("bc_ao_word", "parse_table", bc_cs_activity_codes.COMMENTARY, "Failed to get image for table: " + opara.text)
            '        ocell.bimage = False
            '        ocell.text = "no image available"
            '    End If
            '    opara.table_cells.Add(ocell)
            '    ocomm = New bc_cs_activity_log("bc_ao_word", "parse_table", bc_cs_activity_codes.COMMENTARY, "End table:" + CStr(paragraph_number))


            'Else
            opara.text = worddocument.paragraphs(paragraph_number).Range.text
            If Asc(opara.text) <> 13 Then
                opara.text = tidy_up_text(opara.text)
            Else
                opara.text = "Table"
            End If
            opara.style = worddocument.paragraphs(paragraph_number).Range.Style.namelocal
            opara.table_cells = New List(Of bc_om_style_component_table_cell)

            Dim cell As bc_om_style_component_table_cell

            For i = 1 To adoc.paragraphs(paragraph_number).range.tables(1).range.cells.count
                For k = 1 To adoc.paragraphs(paragraph_number).range.tables(1).range.cells(i).range.paragraphs.count
                    cell = New bc_om_style_component_table_cell
                    cell.row = adoc.paragraphs(paragraph_number).range.tables(1).range.cells(i).rowindex
                    cell.col = adoc.paragraphs(paragraph_number).range.tables(1).range.cells(i).columnindex
                    cell.paragraph = k

                    cell.text = adoc.paragraphs(paragraph_number).range.tables(1).range.cells(i).range.paragraphs(k).range.text
                    If Asc(cell.text) <> 13 Then
                        cell.text = tidy_up_text(cell.text)
                    Else
                        cell.text = ""
                    End If

                    If cell.text <> "" Then
                        Dim td, hy, nt As String
                        Try
                            If adoc.paragraphs(paragraph_number).range.tables(1).range.cells(i).range.paragraphs(k).range.Hyperlinks.Count > 0 Then
                                cell.text = parse_hyperlinks_in_range(adoc.paragraphs(paragraph_number).range.tables(1).range.cells(i).range.paragraphs(k).range.text, adoc.paragraphs(paragraph_number).range.tables(1).range.cells(i).range.paragraphs(k).range)
                            End If
                        Catch ex As Exception
                            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_table", bc_cs_activity_codes.COMMENTARY, "couldnt get hyperlinks  shape:paragraph: " + CStr(paragraph_number) + ": " + ex.Message)
                        End Try
                    End If


                    Try
                        cell.style = CStr(adoc.paragraphs(paragraph_number).range.tables(1).range.cells(i).range.paragraphs(k).range.style.namelocal)
                    Catch
                        cell.style = "style error"
                    End Try
                    If adoc.paragraphs(paragraph_number).range.tables(1).range.cells(i).range.paragraphs(k).range.inlineshapes.count > 0 And bimages = True Then

                        cell.bimage = True
                        cell.image = get_image_from_inline_shape(adoc.paragraphs(paragraph_number).range.tables(1).range.cells(i).range.paragraphs(k).range.inlineshapes(1))

                        cell.text = "Chart"
                    End If
                    opara.table_cells.Add(cell)
                Next
            Next
            'End If
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_table", bc_cs_activity_codes.COMMENTARY, "Cant parse shape:paragraph: " + CStr(paragraph_number) + ": " + ex.Message)
        End Try
    End Sub

    Sub parse_table_in_shape(obj As Object, ByRef opara As bc_om_style_component_base_class, ByVal bimages As Boolean)
        Try
            opara.btable = True

            opara.text = obj.Range.text
            If Asc(opara.text) <> 13 Then
                opara.text = tidy_up_text(opara.text)
            Else
                opara.text = "Table"
            End If
            opara.style = obj.range.Style.namelocal
            opara.table_cells = New List(Of bc_om_style_component_table_cell)

            Dim cell As bc_om_style_component_table_cell
            For i = 1 To obj.Range.tables(1).range.cells.count
                For k = 1 To obj.Range.tables(1).range.cells(i).range.paragraphs.count
                    cell = New bc_om_style_component_table_cell
                    cell.row = obj.Range.tables(1).range.cells(i).rowindex
                    cell.col = obj.Range.tables(1).range.cells(i).columnindex
                    cell.paragraph = k

                    cell.text = obj.Range.tables(1).range.cells(i).range.paragraphs(k).range.text
                    If Asc(cell.text) <> 13 Then
                        cell.text = tidy_up_text(cell.text)
                    Else
                        cell.text = ""
                    End If
                    Try
                        cell.style = CStr(obj.Range.tables(1).range.cells(i).range.paragraphs(k).range.style.namelocal)
                    Catch
                        cell.style = "style error"
                    End Try
                    If obj.Range.tables(1).range.cells(i).range.paragraphs(k).range.inlineshapes.count = 1 And bimages = True Then
                        cell.bimage = True
                        cell.image = get_image_from_inline_shape(obj.Range.tables(1).range.cells(i).range.paragraphs(k).range.inlineshapes(1))
                        cell.text = "Chart"
                    End If
                    opara.table_cells.Add(cell)
                Next
            Next

        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "parse_table_in_shapes", bc_cs_activity_codes.COMMENTARY, "Cant parse shape:paragraph: " + ex.Message)
        End Try
    End Sub
    REM saves current document
    Public Overrides Sub save()

        worddocument.save()
    End Sub
    Public Overrides Function save_return_error()
        Try
            worddocument.save()
            save_return_error = ""
        Catch ex As Exception
            save_return_error = ex.Message
        End Try
    End Function
    REM saves document to filename
    Public Overrides Function saveas_return_error(ByVal filename As String) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "saveas", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim officeFormat As Integer = 0
        Dim officeExstention As String = ""

        Try
            REM SW cope with office versions
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                officeFormat = 12
                officeExstention = ".docx"
            Else
                officeFormat = 0
                officeExstention = ".doc"
            End If

            worddocument.saveas(bc_cs_central_settings.local_repos_path + filename + officeExstention, FileFormat:=officeFormat, addtorecentfiles:=False)
            saveas_return_error = ""

        Catch ex As Exception
            'Dim db_err As New bc_cs_error_log("bc_ao_word", "saveas", bc_cs_error_codes.USER_DEFINED, ex.Message)
            saveas_return_error = ex.Message

        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "saveas", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Overrides Function get_extension() As String
        Dim na As String
        na = worddocument.Name
        na = Right(na, Len(na) - InStrRev(na, ".") + 1)
        get_extension = na

    End Function

    Public Overrides Function saveas(ByVal filename As String) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "saveas", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim officeFormat As Integer = 0
        Dim officeExstention As String = ""

        Try
            REM SW cope with office versions
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                officeFormat = 12
                officeExstention = ".docx"
            Else
                officeFormat = 0
                officeExstention = ".doc"
            End If
            'If bc_cs_central_settings.office_version < 12 And bc_cs_central_settings.user_office_version >= 12 Then
            '    officeFormat = 0
            '    officeExstention = ".doc"
            'ElseIf bc_cs_central_settings.office_version < 12 Then
            '    officeFormat = 0
            '    officeExstention = ".doc"
            'ElseIf bc_cs_central_settings.office_version >= 12 Then
            '    officeFormat = 12
            '    officeExstention = ".docx"
            'End If

            saveas = "Failed"
            worddocument.saveas(bc_cs_central_settings.local_repos_path + filename + officeExstention, FileFormat:=officeFormat, addtorecentfiles:=False)
            saveas = officeExstention

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "saveas", bc_cs_error_codes.USER_DEFINED, ex.Message)
            saveas = ""
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "saveas", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function


    Public Overrides Function tmp_saveas(ByVal filename As String) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "saveas", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim officeFormat As Integer = 0
        Dim officeExstention As String = ""

        Try
            REM SW cope with office versions
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                officeFormat = 12
                officeExstention = ".docx"
            Else
                officeFormat = 0
                officeExstention = ".doc"
            End If
            'If bc_cs_central_settings.office_version < 12 And bc_cs_central_settings.user_office_version >= 12 Then
            '    officeFormat = 0
            '    officeExstention = ".doc"
            'ElseIf bc_cs_central_settings.office_version < 12 Then
            '    officeFormat = 0
            '    officeExstention = ".doc"
            'ElseIf bc_cs_central_settings.office_version >= 12 Then
            '    officeFormat = 12
            '    officeExstention = ".docx"
            'End If

            tmp_saveas = "Failed"
            worddocument.saveas(filename:=bc_cs_central_settings.local_repos_path + filename + officeExstention, FileFormat:=officeFormat, addtorecentfiles:=False)
            tmp_saveas = officeExstention

        Catch ex As Exception
            Dim db_err As New bc_cs_activity_log("bc_ao_word", "tmp_saveas", bc_cs_activity_codes.COMMENTARY, "cannot save to bc_tmp.doc file may already be open")
            tmp_saveas = ""
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "tmp_saveas", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    REM closes current document
    Public Overrides Sub close(Optional ByVal keep_invisible As Boolean = False, Optional ByVal from_create As Boolean = False)
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "close", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
            GC.WaitForPendingFinalizers()

            worddocument.close(0)
            REM Process Async
            'If word.documents.count = 0 And from_create = False Then

            '    word.quit()
            '    GC.Collect()
            'End If
            Marshal.FinalReleaseComObject(worddocument)

            worddocument = Nothing

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "close", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "close", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    REM quits word application
    Public Overrides Sub quit()
        If Not wordAlreadyOpen And Not word Is Nothing Then
            word.application.quit()
        End If
    End Sub
    Public Overrides Function get_name() As String
        get_name = worddocument.name
    End Function
    Public Overrides Function get_fullname() As String
        get_fullname = worddocument.fullname
    End Function
    REM build document
    Public Overrides Sub build(ByVal template_id As Long)
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "build", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As bc_cs_activity_log
        Dim i As Integer

        Try
            REM set and assign master properties for portability
            Try
                worddocument.CustomDocumentProperties.add("rnet_workflow_refresh", False, 4, "")
                worddocument.CustomDocumentProperties.add("rnet_data_at_data", False, 4, "")
                worddocument.CustomDocumentProperties.add("rnet_refresh_lang_id", False, 4, "")
                worddocument.CustomDocumentProperties.add("rnet_secondary_entity", False, 4, "")

                worddocument.CustomDocumentProperties("rnet_workflow_refresh") = CStr(bc_am_load_objects.obc_current_document.refresh_components.workflow_state)
                worddocument.CustomDocumentProperties("rnet_refresh_lang_id") = bc_am_load_objects.obc_current_document.language_id
                worddocument.CustomDocumentProperties("rnet_data_at_data") = CStr(bc_am_load_objects.obc_current_document.refresh_components.data_at_date)
                worddocument.CustomDocumentProperties.add("rnet_secondary_entity_id", False, 4, "")
                If bc_am_load_objects.obc_current_document.taxonomy.Count > 1 Then
                    For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                        If bc_am_load_objects.obc_entities.entity(i).id = bc_am_load_objects.obc_current_document.taxonomy(1).entity_id Then
                            worddocument.CustomDocumentProperties("rnet_secondary_entity") = bc_am_load_objects.obc_entities.entity(i).name
                            worddocument.CustomDocumentProperties("rnet_secondary_entity_id") = bc_am_load_objects.obc_entities.entity(i).id
                            Exit For
                        End If
                    Next
                End If

            Catch ex As Exception
                ocommentary = New bc_cs_activity_log("bc_ao_word", "build", bc_cs_activity_codes.COMMENTARY, "Error Setting PortabiltyProperties:" + ex.Message)
            End Try

            REM set properties
            ocommentary = New bc_cs_activity_log("bc_ao_word", "build", bc_cs_activity_codes.COMMENTARY, "Setting Document Properties")
            MyBase.build(template_id)
            For i = 0 To ogproperties.doc_property.Count - 1
                With ogproperties.doc_property(i)
                    ocommentary = New bc_cs_activity_log("bc_ao_word", "build", bc_cs_activity_codes.COMMENTARY, "Attempting to set property: " + .name)
                    'Rama Comments : Added try block without catch as there is unspecified error
                    Try
                        If .name = "rnet_entity_id" Then
                            .type = 1
                        End If
                        Select Case .type
                            Case 1
                                worddocument.CustomDocumentProperties.Add(name:=CStr(.name), LinkToContent:=False, Type:=4, value:="")
                            Case 2
                                worddocument.CustomDocumentProperties.Add(name:=CStr(.name), LinkToContent:=False, Type:=2, value:=True)
                            Case 3
                                worddocument.CustomDocumentProperties.Add(name:=CStr(.name), LinkToContent:=False, Type:=1, value:=0)
                            Case 4
                                worddocument.CustomDocumentProperties.Add(name:=CStr(.name), LinkToContent:=False, Type:=3, value:="")
                        End Select
                    Catch
                        ' Dont catch the error as it will not set other properties.
                    End Try

                End With
            Next
            REM set mandatory metadata property values
            ocommentary = New bc_cs_activity_log("bc_ao_word", "build", bc_cs_activity_codes.COMMENTARY, "Attempting to set mandatory property values.")
            REM entity_id

            worddocument.CustomDocumentProperties("rnet_entity_id").value = CStr(bc_am_load_objects.obc_current_document.entity_id)

            REM analyst_id
            worddocument.CustomDocumentProperties("rnet_analyst_id").value = CStr(bc_am_load_objects.obc_current_document.originating_author)
            REM pub_type_id
            worddocument.CustomDocumentProperties("rnet_pub_type_id").value = CStr(bc_am_load_objects.obc_current_document.pub_type_id)
            REM language_id
            'worddocument.CustomDocumentProperties("rnet_language_id").value = cstr(bc_am_load_objects.obc_current_document.language_id)
            REM FIL June 2013
            REM set this here now as if n document changes happen it doesnt stick
            REM PR JULY 2014
            worddocument.CustomDocumentProperties("rnet_template_id").value = template_id
            worddocument.CustomDocumentProperties("rnet_doc_id").value = CStr(bc_am_load_objects.obc_current_document.filename)

            Try
                For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                    If bc_am_load_objects.obc_users.user(i).id = bc_cs_central_settings.logged_on_user_id Then
                        worddocument.customdocumentproperties("rnet_user_role_name").value = bc_am_load_objects.obc_users.user(i).role
                        Exit For
                    End If
                Next
            Catch
                ocommentary = New bc_cs_activity_log("bc_ao_word", "build", bc_cs_activity_codes.COMMENTARY, "Property: rnet_user_role_name not set in document")
            End Try

            REM template components
            ocommentary = New bc_cs_activity_log("bc_ao_word", "build", bc_cs_activity_codes.COMMENTARY, "Attempting to set Components.")
            With ogtemplate.components
                For i = 0 To .component.Count - 1
                    With .component(i)
                        Select Case .type
                            Case TABLE
                                REM place autotext
                                ocommentary = New bc_cs_activity_log("bc_ao_word", "build", bc_cs_activity_codes.COMMENTARY, "Setting Table: " + .autotext_name + " at bookmark: " + .bookmark_name)
                                If insert_autotext(.autotext_name, .bookmark_name) = bc_cs_error_codes.RETURN_ERROR Then
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "build", bc_cs_activity_codes.COMMENTARY, "Insert Autotext failed to find autotext: " + .autotext_name + " or bookmark: " + .bookmark_name)
                                Else
                                    REM sub components and component parameters
                                    If insert_sub_components(.r1c1_ref, .sub_components) = bc_cs_error_codes.RETURN_ERROR Then
                                        ocommentary = New bc_cs_activity_log("bc_ao_word", "build", bc_cs_activity_codes.COMMENTARY, "Insert Sub Component failed to find bookmark: " + .r1c1_ref + " in autotext: " + .autotext_name)
                                    End If
                                End If

                            Case PROP
                                ocommentary = New bc_cs_activity_log("bc_ao_word", "build", bc_cs_activity_codes.COMMENTARY, "Setting Property: " + .bookmark_name + " with value: " + .autotext_name)
                                worddocument.customdocumentproperties(.bookmark_name).value = CStr(.autotext_name)
                        End Select

                    End With
                Next
            End With

            REM finally save document metadata for this new document to disk
            MyBase.build(template_id)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "build", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            REM housekeep here
            otrace = New bc_cs_activity_log("bc_ao_word", "build", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    REM inserts an autotext table into document 
    REM at specified bookmark
    Public Overrides Sub initialize()
        Dim ocommentary As bc_cs_activity_log
        REM call initialisation function
        Try
            ocommentary = New bc_cs_activity_log("bc_ao_word", "initialize", bc_cs_activity_codes.COMMENTARY, "Attempting to call DOCINIT on new")

            bc_cs_central_settings.progress_bar.unload()

            If Trim(bc_cs_central_settings.user_name) = "" Then
                REM PR May 2013 nee dto pass system through as well
                Try
                    worddocument.DOCINIT("NEW", bc_cs_central_settings.selected_system)
                Catch
                    worddocument.DOCINIT("NEW")
                End Try
            Else
                Try
                    worddocument.DOCINIT("NEW", bc_cs_central_settings.user_name, bc_cs_central_settings.user_password, bc_cs_central_settings.selected_system)

                Catch
                    worddocument.DOCINIT("NEW", bc_cs_central_settings.user_name, bc_cs_central_settings.user_password)

                End Try
            End If

        Catch ex As Exception
            ocommentary = New bc_cs_activity_log("bc_ao_word", "initialize", bc_cs_activity_codes.COMMENTARY, String.Concat("No DOCINIT in Document ", ex.Message, " ", ex.InnerException))
        End Try
    End Sub
    Public Overrides Sub after_open()
        Dim ocommentary As bc_cs_activity_log
        REM call initialisation function
        Try
            ocommentary = New bc_cs_activity_log("bc_ao_word", "after_open", bc_cs_activity_codes.COMMENTARY, "Attempting to call DOCOPEN on open")
            worddocument.DOCOPEN()
        Catch ex As Exception
            ocommentary = New bc_cs_activity_log("bc_ao_word", "after_open", bc_cs_activity_codes.COMMENTARY, String.Concat("No DOCOPEN in Document ", ex.Message, " ", ex.InnerException))
        End Try
    End Sub


    Private Function insert_autotext(ByVal strautotextname As String, ByVal strbookmarklocation As String) As Integer
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "insert_autotext", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim i As Integer
        Dim ocommentary As bc_cs_activity_log
        Dim found As Boolean
        found = False
        insert_autotext = 0
        Try
            If worddocument.Bookmarks.Exists(strbookmarklocation) Then
                worddocument.Bookmarks(strbookmarklocation).range.Select()
                With worddocument.Application
                    For i = 1 To .Templates.Count
                        If LCase(.Templates(i).name) = "autotext.dot" Or LCase(.Templates(i).name) = "autotext.dotm" Then
                            found = True
                            Try
                                Dim rng As Object
                                rng = worddocument.application.selection.range
                                .Templates(i).AutoTextEntries(strautotextname).Insert(where:=rng, RichText:=True)
                            Catch ex As Exception
                                ocommentary = New bc_cs_activity_log("bc_ao_word", "insert_autotext", bc_cs_activity_codes.COMMENTARY, "Error with Autotext Element:" + strautotextname + " :" + ex.Message)
                                insert_autotext = bc_cs_error_codes.RETURN_ERROR
                            End Try
                            Exit For
                        End If
                    Next
                    If found = False Then
                        ocommentary = New bc_cs_activity_log("bc_ao_word", "insert_autotext", bc_cs_activity_codes.COMMENTARY, "Failed to find Autotext Library")
                        insert_autotext = bc_cs_error_codes.RETURN_ERROR
                    End If
                End With
            Else
                ocommentary = New bc_cs_activity_log("bc_ao_word", "insert_autotext", bc_cs_activity_codes.COMMENTARY, "Failed to find Bookmark: " + strbookmarklocation)
                insert_autotext = bc_cs_error_codes.RETURN_ERROR
            End If
        Catch ex As Exception

        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "insert_autotext", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Private Function insert_sub_components(ByVal strr1c1_ref As String, ByVal osub_components As bc_om_sub_components) As Integer
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "insert_sub_components", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim i, j As Integer
        Dim orange As Object
        Dim bm_name As String
        Dim pprop_name As String

        Dim ocommentary As bc_cs_activity_log

        Try

            If worddocument.Bookmarks.exists(strr1c1_ref) Then
                For i = 0 To osub_components.sub_component.Count - 1
                    With osub_components.sub_component(i)
                        REM select component
                        Try
                            orange = worddocument.Application.Selection.Tables(1).Cell(.row, .col).range
                        Catch
                            ocommentary = New bc_cs_activity_log("bc_ao_word", "insert_sub_components", bc_cs_activity_codes.COMMENTARY, "Rol/ Col:" + CStr(.row) + "," + CStr(.col) + " does not exist in table.")
                            Exit Function
                        End Try
                        REM set style
                        Try
                            orange.style = .style
                        Catch
                            ocommentary = New bc_cs_activity_log("bc_ao_word", "insert_sub_components", bc_cs_activity_codes.COMMENTARY, "Style:" + CStr(.style) + " does not exist in document.")
                        End Try
                        If .type = TEXT Then
                            orange.text = .value
                        ElseIf (.type = BOOKMARK) Then
                            orange.bookmarks.add(.value + "_en" + CStr(bc_am_load_objects.obc_current_document.entity_id), orange)
                        Else
                            REM if entity_id is 0 replace with a random number so display can be used
                            REM in composites
                            If bc_am_load_objects.obc_current_document.entity_id = 0 Then
                                bm_name = "rnet" + CStr(worddocument.Bookmarks.count) + "_at" + CStr(.type) + "_en" + CStr(Format(Now, "hhmmss"))
                            Else
                                bm_name = "rnet" + CStr(worddocument.Bookmarks.count) + "_at" + CStr(.type) + "_en" + CStr(bc_am_load_objects.obc_current_document.entity_id)
                            End If
                            ocommentary = New bc_cs_activity_log("bc_ao_word", "insert_sub_components", bc_cs_activity_codes.COMMENTARY, "Setting Bookmark:" + bm_name + " in style: " + .style + " at position(" + CStr(.row) + "," + CStr(.col))
                            orange.bookmarks.add(bm_name, orange)
                            REM set paramters as properties for portability
                            Try
                                REM contributor id
                                pprop_name = bm_name + "_contributor"
                                worddocument.CustomDocumentProperties.add(pprop_name, False, 4, "")
                                worddocument.CustomDocumentProperties(pprop_name) = osub_components.sub_component(i).contributor_id
                                REM disabled set to enabled on build
                                pprop_name = bm_name + "_disabled"
                                worddocument.CustomDocumentProperties.add(pprop_name, False, 4, "")
                                worddocument.CustomDocumentProperties(pprop_name) = "enabled"
                                If osub_components.sub_component(i).parameters.component_template_parameters.count > 0 Then
                                    For j = 0 To osub_components.sub_component(i).parameters.component_template_parameters.count - 1
                                        With osub_components.sub_component(i).parameters.component_template_parameters(j)
                                            pprop_name = bm_name + "_p" + CStr(j)
                                            worddocument.CustomDocumentProperties.add(pprop_name, False, 4, "")
                                            If .system_defined = 0 Then
                                                If IsNumeric(.default_value_id) Then
                                                    worddocument.CustomDocumentProperties(pprop_name) = CStr(.default_value_id)
                                                Else
                                                    worddocument.CustomDocumentProperties(pprop_name) = CStr(.default_value)
                                                End If
                                            Else
                                                worddocument.CustomDocumentProperties(pprop_name) = "system_defined"
                                            End If
                                        End With
                                    Next
                                End If
                            Catch

                            End Try

                            REM see if parameters exists for sub component 
                            REM if so add component to document metadata
                            MyBase.set_component_in_document_metadata(osub_components.sub_component(i), bm_name, bc_am_load_objects.obc_current_document.entity_id, bc_am_load_objects.obc_current_document.taxonomy)
                        End If
                    End With
                Next

            Else
                insert_sub_components = bc_cs_error_codes.RETURN_ERROR
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "insert_sub_components", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "insert_sub_components", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Overrides Sub update_parameter_values_to_document(ByVal refresh_components As bc_om_refresh_components, ByRef ldoc As bc_om_document)
        Dim i, j As Integer
        Dim bm_name As String
        Dim otrace As New bc_cs_activity_log("bc_ao_word", " update_parameter_values_to_document", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Try
                worddocument.CustomDocumentProperties("rnet_workflow_refresh").value = refresh_components.workflow_state
                worddocument.CustomDocumentProperties("rnet_refresh_lang_id").value = ldoc.language_id
                worddocument.CustomDocumentProperties("rnet_data_at_data").value = refresh_components.data_at_date
            Catch
                Dim ocomm As New bc_cs_activity_log("bc_ao_word", " update_parameter_values_to_document", bc_cs_activity_codes.COMMENTARY, "Portabilty not installed")
            End Try

            For i = 0 To refresh_components.refresh_components.Count - 1
                With refresh_components.refresh_components(i)
                    REM contributor_id
                    bm_name = CStr(.locator) + "_contributor"
                    worddocument.CustomDocumentProperties(bm_name) = .contributor_id
                    REM enabled
                    bm_name = CStr(.locator) + "_disabled"
                    If .disabled = 0 Then
                        worddocument.CustomDocumentProperties(bm_name) = "enabled"
                    Else
                        worddocument.CustomDocumentProperties(bm_name) = "disabled"
                    End If

                    For j = 0 To .parameters.component_template_parameters.Count - 1
                        bm_name = CStr(.locator) + "_p" + CStr(j)
                        Try
                            If .parameters.component_template_parameters(j).system_defined = 0 Then
                                If IsNumeric(.parameters.component_template_parameters(j).default_value_id) Then
                                    worddocument.CustomDocumentProperties(bm_name) = CStr(.parameters.component_template_parameters(j).default_value_id)
                                Else
                                    worddocument.CustomDocumentProperties(bm_name) = CStr(.parameters.component_template_parameters(j).default_value)
                                End If
                            Else
                                worddocument.CustomDocumentProperties(bm_name) = "system_defined"
                            End If
                        Catch

                        End Try
                    Next
                End With
            Next
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", " update_parameter_values_to_document", bc_cs_activity_codes.COMMENTARY, "Portabilty not installed or portabilty error")
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", " update_parameter_values_to_document", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Sub update_parameter_values_from_document(ByRef refresh_components As bc_om_refresh_components, ByRef ldoc As bc_om_document)
        Dim i, j, k As Integer
        Dim bm_name As String
        Dim found As Boolean = False
        Dim val As String
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "update_parameter_values_from_document", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            REM financial workflow stage
            REM data at date
            REM language id
            Try
                refresh_components.workflow_state = worddocument.CustomDocumentProperties("rnet_workflow_refresh").value
                refresh_components.language_id = worddocument.CustomDocumentProperties("rnet_refresh_lang_id").value
                ldoc.language_id = refresh_components.language_id

                refresh_components.data_at_date = worddocument.CustomDocumentProperties("rnet_data_at_data").value
            Catch
                Dim omsg As New bc_cs_message("Blue Curve", "Error reading portability properties", bc_cs_message.MESSAGE)
            End Try

            For i = 0 To refresh_components.refresh_components.Count - 1
                With refresh_components.refresh_components(i)
                    REM contributor_id
                    bm_name = CStr(.locator) + "_contributor"
                    val = Me.get_property_value(bm_name)
                    .contributor_id = val
                    REM enabled
                    bm_name = CStr(.locator) + "_disabled"
                    val = Me.get_property_value(bm_name)
                    If val <> "enabled" Then
                        .disabled = 1
                    End If
                    For j = 0 To .parameters.component_template_parameters.Count - 1
                        bm_name = CStr(.locator) + "_p" + CStr(j)
                        val = Me.get_property_value(bm_name)
                        If val <> "Error" Then
                            If val = "system_defined" Then
                                .parameters.component_template_parameters(j).default_value = ""
                                .parameters.component_template_parameters(j).system_defined = 1
                            Else
                                .parameters.component_template_parameters(j).system_defined = 0
                                If .parameters.component_template_parameters(j).lookup_values_ids.count > 0 Then
                                    .parameters.component_template_parameters(j).default_value_id = val
                                    For k = 0 To .parameters.component_template_parameters(j).lookup_values.count - 1
                                        If .parameters.component_template_parameters(j).lookup_values(k) = val Then
                                            .parameters.component_template_parameters(j).default_value = .parameters.component_template_parameters(j).lookup_values_ids(k)
                                            found = True
                                            Exit For
                                        End If
                                    Next
                                    If found = False Then
                                        .parameters.component_template_parameters(j).default_value = val
                                    End If
                                Else
                                    .parameters.component_template_parameters(j).default_value = val
                                End If
                            End If
                        End If
                    Next
                End With
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", " update_parameter_values_from_document", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", " update_parameter_values_from_document", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides Function bookmark_count() As Integer
        bookmark_count = worddocument.bookmarks.count
    End Function

    Overrides Function get_component_markup(ByVal with_value As Boolean) As bc_am_ao_locators
        REM parse bookmarks in document looking for automated components and
        REM store in array
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "get_component_markup", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim i As Integer
            Dim bm As String
            Dim locators As New bc_am_ao_locators
            Dim locator As bc_am_ao_locator
            Dim value As String
            Dim ocommentary As bc_cs_activity_log
            Dim s As bc_cs_string_services

            For i = 1 To worddocument.bookmarks.count
                Me.update_refresh_status_bar("Extracting Refresh Component: ", i, worddocument.bookmarks.count)
                bm = worddocument.bookmarks(i).name
                If InStr(bm, "rnet", CompareMethod.Text) = 1 Then
                    value = ""
                    If with_value = True And InStr(bm, "at") > 0 Then
                        Try
                            value = worddocument.bookmarks(i).range.text
                            value = Left(value, Len(value) - 2)
                            s = New bc_cs_string_services(value)
                            value = s.xml_compliant()
                            ocommentary = New bc_cs_activity_log("bc_ao_word", "get_component_markup", bc_cs_activity_codes.COMMENTARY, "Text for Bookmark:" + bm + " is: " + value)
                        Catch
                            ocommentary = New bc_cs_activity_log("bc_ao_word", "get_component_markup", bc_cs_activity_codes.COMMENTARY, "No Text for Bookmark:" + bm)
                        End Try
                    End If
                    locator = New bc_am_ao_locator(worddocument.bookmarks(i).name, value)
                    locator.name = get_locator_user_friendly_name(worddocument.bookmarks(i).name)
                    locators.locators.Add(locator)
                End If
            Next
            get_component_markup = locators

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "get_component_markup", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_component_markup = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "get_component_markup", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    REM autonomous oct 2012
    Public Overrides Function get_value_for_locator(ByVal bm As String) As String
        Try
            Dim ocomm = New bc_cs_activity_log("bc_ao_word", "get_value_for_locator", bc_cs_activity_codes.COMMENTARY, "start: " + bm)
            get_value_for_locator = ""
            Dim val As String
            If worddocument.bookmarks.exists(bm) Then
                val = worddocument.bookmarks(bm).range.text
                If Len(val) > 2 Then
                    get_value_for_locator = val
                End If
            End If
        Catch
            get_value_for_locator = ""
        Finally
            Dim ocomm = New bc_cs_activity_log("bc_ao_word", "get_value_for_locator", bc_cs_activity_codes.COMMENTARY, "end: " + bm)

        End Try

    End Function
    Public Sub set_value_for_locator(ByVal bm As String, ByVal val As String)
        Try
            Dim pro As Integer
            If worddocument.bookmarks.exists(bm) Then
                pro = worddocument.ProtectionType
                If pro = 2 Then
                    Try
                        worddocument.unprotect()
                    Catch

                    End Try
                End If
                worddocument.bookmarks(bm).range.text = val
                worddocument.application.screenrefresh()
                If pro = 2 Then
                    Try
                        worddocument.Protect(2)
                    Catch

                    End Try
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "set_value_for_locator", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub
    Public Overrides Function set_next_selection(ByVal bm As String, Optional ByVal page_break As Boolean = True) As Boolean
        Try
            worddocument.application.selection.movedown(unit:=5, count:=1)
            worddocument.application.selection.Range.Font.Size = 1
            If page_break = True Then
                worddocument.application.selection.insertbreak(Type:=7)
                worddocument.application.selection.Range.Font.Size = 1
            End If
            set_next_selection = True
        Catch ex As Exception
            set_next_selection = False
        End Try

    End Function
    Public Overrides Sub remove_invalid_bookmarks(ByVal orefresh As bc_om_refresh_components)
        REM remove any BC bookmarks in document but not in list
        Try
            Dim i As Integer
            i = 1
            Dim found As Boolean = False
            While i <= worddocument.bookmarks.count
                found = False
                If Len(worddocument.bookmarks(i).name) > 13 And Left(worddocument.bookmarks(i).name, 13) = "rnet_breakref" Then
                    found = False
                ElseIf Len(worddocument.bookmarks(i).name) > 10 And Left(worddocument.bookmarks(i).name, 10) = "rnet_break" Then
                    found = False

                ElseIf Len(worddocument.bookmarks(i).name) > 8 Then
                    If Left(worddocument.bookmarks(i).name, 4) = "rnet" And Left(worddocument.bookmarks(i).name, 8) <> "rnet_ind" Then
                        For j = 0 To orefresh.refresh_components.Count - 1
                            If orefresh.refresh_components(j).locator = worddocument.bookmarks(i).name Then
                                found = True
                                Exit For
                            End If
                        Next
                        If found = False Then
                            REM FIL DEBUG MAY 2013
                            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "remove_invalid_bookmarks", bc_cs_activity_codes.COMMENTARY, "Removing bookmark:" + worddocument.bookmarks(i).name + " as not in metadata file for doc: " + CStr(worddocument.name))
                            REM ==== 
                            worddocument.bookmarks(i).delete()
                            i = i - 1
                        End If
                    End If
                End If

                i = i + 1
            End While

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_word", "remove_invalid_bookmarks", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally

        End Try
    End Sub



    Public Overrides Function set_locator_for_component(ByVal component_id As Long, ByVal entity_id As Long, ByVal component_type As Integer, Optional ByVal bwith_table As Boolean = True) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "set_locator_for_component", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim bm As String
        bm = ""
        Try
            If entity_id = 0 Then
                bm = "rnet" + CStr(worddocument.Bookmarks.count) + "_at" + CStr(component_id) + "_en" + CStr(Format(Now, "hhmmss"))
            Else
                bm = "rnet" + CStr(worddocument.bookmarks.count + 1) + "_at" + CStr(component_id) + "_en" + CStr(entity_id)
                REM if it exists increase count
                Dim i As Integer
                i = worddocument.bookmarks.count + 1
                While worddocument.bookmarks.exists(bm) = True
                    i = i + 1
                    bm = "rnet" + CStr(worddocument.bookmarks.count + 1 + i) + "_at" + CStr(component_id) + "_en" + CStr(entity_id)
                End While
            End If
            If bwith_table = True Then
                Try
                    worddocument.Tables.Add(Range:=worddocument.application.Selection.Range, NumRows:=2, NumColumns:=1, DefaultTableBehavior:=1, AutoFitBehavior:=0)
                Catch
                    worddocument.Application.Selection.Bookmarks.Add(bm)
                End Try
            Else
                Select Case component_type
                    Case 1, 3, 4, 5, 8, 9, 12, 31
                        If worddocument.Application.selection.tables.count = 0 Then

                            Try
                                worddocument.Tables.Add(Range:=worddocument.application.Selection.Range, NumRows:=1, NumColumns:=1, DefaultTableBehavior:=1, AutoFitBehavior:=0)
                            Catch ex As Exception

                                worddocument.Application.Selection.Tables(1).Cell(1, 1).Range.Bookmarks.Add(bm)
                                worddocument.Application.Selection.Tables(1).Borders.OutsideLineStyle = 0
                            End Try
                        Else

                            Try
                                Dim r, c As Integer
                                r = worddocument.Application.Selection.Information(13)
                                c = worddocument.Application.Selection.Information(16)
                                worddocument.Application.Selection.Tables(1).Cell(r, c).Range.Bookmarks.Add(bm)
                            Catch
                                Dim omsg As New bc_cs_message("Blue Curve", "Failed to insert component this selection point please move cussor to new insertion point", bc_cs_message.MESSAGE)
                            End Try
                        End If
                        REM if table put table in first if table already there then disallow
                    Case 2, 7
                        If worddocument.Application.selection.tables.count > 0 Then
                            Dim omsg As New bc_cs_message("Blue Curve", "Cannot insert a new table in an existing table please move cursor to new insertion point", bc_cs_message.MESSAGE)
                            bm = ""
                        Else
                            Try
                                worddocument.Tables.Add(Range:=worddocument.application.Selection.Range, NumRows:=2, NumColumns:=1, DefaultTableBehavior:=1, AutoFitBehavior:=0)
                            Catch
                                worddocument.Application.Selection.Bookmarks.Add(bm)
                            End Try
                        End If
                End Select
            End If
        Catch ex As Exception
            Dim omessage As New bc_cs_message("Blue Curve", "Cant add component at selected location", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            bm = ""
        Finally
            set_locator_for_component = bm
            otrace = New bc_cs_activity_log("bc_ao_word", "set_locator_for_component", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    'Public Overrides Function set_locator_for_component(ByVal component_id As Long, ByVal entity_id As Long) As String
    '    Dim otrace As New bc_cs_activity_log("bc_ao_word", "set_locator_for_component", bc_cs_activity_codes.TRACE_ENTRY, "")
    '    Dim bm As String = ""
    '    Try
    '        Dim exists As Boolean = True
    '        Dim i As Integer = 1
    '        While exists = True
    '            If entity_id = 0 Then
    '                bm = "rnet" + CStr(worddocument.Bookmarks.count + i) + "_at" + CStr(component_id) + "_en" + CStr(Format(Now, "hhmmss"))
    '            Else
    '                bm = "rnet" + CStr(worddocument.bookmarks.count + i) + "_at" + CStr(component_id) + "_en" + CStr(entity_id)
    '            End If
    '            If worddocument.bookmarks.exists(bm) = False Then
    '                exists = False
    '            End If
    '            i = i + 1
    '        End While
    '        Try
    '            worddocument.Tables.Add(Range:=worddocument.application.Selection.Range, NumRows:=2, NumColumns:=1, DefaultTableBehavior:=1, AutoFitBehavior:=0)
    '        Catch
    '            worddocument.Application.Selection.Bookmarks.Add(bm)
    '        End Try
    '    Catch ex As Exception
    '        Dim omessage As New bc_cs_message("Blue Curve", "Cant add component at selected location", bc_cs_message.MESSAGE)
    '        bm = ""
    '    Finally
    '        set_locator_for_component = bm
    '        otrace = New bc_cs_activity_log("bc_ao_word", "set_locator_for_component", bc_cs_activity_codes.TRACE_ENTRY, "")
    '    End Try

    'End Function
    Public Overrides Function check_udc_in_document(ByVal udc_id As Long) As Boolean
        check_udc_in_document = False
        For i = 0 To worddocument.bookmarks.count - 1
            If InStr(worddocument.bookmarks(i + 1).name, "udc" + CStr(udc_id)) > 0 Then
                check_udc_in_document = True
                Exit Function
            End If
        Next
    End Function
    Public Overrides Function check_sdc_in_document(ByVal sdc_id As Long, ByVal type_id As Long, ByVal entity_id As Long) As Boolean
        check_sdc_in_document = False
        For i = 0 To worddocument.bookmarks.count - 1
            If InStr(worddocument.bookmarks(i + 1).name, "at" + CStr(sdc_id) + "_en" + CStr(entity_id)) > 0 Then
                check_sdc_in_document = True
                Exit Function
            End If
        Next
    End Function
    Overrides Sub set_component_markup(ByRef orefresh_components As bc_om_refresh_components, Optional ByVal show_form As Boolean = True, Optional ByVal editChart As Boolean = False)
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.TRACE_ENTRY, "")
        'Dim i, j, k As Integer
        Dim i As Integer
        'Dim placeholder_found As Boolean
        Dim placeholder As String = ""
        Dim officeExstention As String = ""
        Dim extensionsize As Integer = 0

        'Const chart_placeholder_count As Integer = 20

        Try

            worddocument.application.screenupdating = False
            MyBase.set_excel_running_flag()

            Me.disable_track_changes()

            REM take out tables that have been broken

            REM loop through list and set value at component
            Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Setting Component Values into word")
            For i = 0 To orefresh_components.refresh_components.Count - 1
                If orefresh_components.refresh_components(i).no_refresh = 0 And orefresh_components.refresh_components(i).disabled = 0 Then
                    If show_form = True Then
                        bc_cs_central_settings.progress_bar.change_caption("Refreshing item " + CStr(i + 1) + " of " + CStr(orefresh_components.refresh_components.Count))
                    End If
                    ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Refreshing Locator:" + orefresh_components.refresh_components(i).locator + " modes:" + CStr(orefresh_components.refresh_components(i).mode))
                    Me.update_refresh_status_bar("Refreshing: ", i + 1, orefresh_components.refresh_components.Count)
                    If Not IsNothing(orefresh_components.refresh_components(i).refresh_value) Then
                        REM cowan oct 2012
                        If orefresh_components.refresh_components(i).mode = 1 Or orefresh_components.refresh_components(i).mode = 10 Or orefresh_components.refresh_components(i).mode = 11 Or orefresh_components.refresh_components(i).mode = 111 Or orefresh_components.refresh_components(i).mode = 110 Then
                            If UBound(orefresh_components.refresh_components(i).refresh_value, 2) > -1 Then
                                Dim rtf As String
                                rtf = CStr(orefresh_components.refresh_components(i).refresh_value(0, 0))
                                If Len(rtf) > 5 Then
                                    If Left(rtf, 5) = "[rtf]" Then
                                        rtf = Right(rtf, Len(rtf) - 5)
                                        If set_selection_from_rtf(worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range, rtf) = False Then
                                            orefresh_components.success = False
                                            ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "failed to set from rtf 1,11,111:" + orefresh_components.refresh_components(i).locator)
                                        End If
                                    Else
                                        ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Setting Bookmark with text: " + orefresh_components.refresh_components(i).locator)
                                        worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.text = orefresh_components.refresh_components(i).refresh_value(0, 0)
                                    End If
                                Else
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Setting Bookmark:" + orefresh_components.refresh_components(i).locator + " with value:" + orefresh_components.refresh_components(i).refresh_value(0, 0))
                                    worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.text = orefresh_components.refresh_components(i).refresh_value(0, 0)
                                End If
                                REM make it work 
                                If orefresh_components.refresh_components(i).mode = 111 Then

                                    orefresh_components.refresh_components(i).disabled = 1
                                End If
                            End If
                            REM editable SDCS rtf
                        ElseIf orefresh_components.refresh_components(i).mode = 8 Or orefresh_components.refresh_components(i).mode = 88 Or orefresh_components.refresh_components(i).mode = 80 Or orefresh_components.refresh_components(i).mode = 880 Then
                            ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Setting Bookmark from rtf: " + orefresh_components.refresh_components(i).locator)
                            worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.text = ""

                            If set_selection_from_rtf(orefresh_components.refresh_components(i).locator, orefresh_components.refresh_components(i).refresh_value(0, 0), orefresh_components) = False Then
                                orefresh_components.success = False
                                ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Failed to set  Bookmark:" + orefresh_components.refresh_components(i).locator + " with rtf")
                                'worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.text = orefresh_components.refresh_components(i).refresh_value(0, 0)
                            End If
                            REM editable SDCs text
                        ElseIf orefresh_components.refresh_components(i).mode = 9 Then
                            ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Setting Bookmark:" + orefresh_components.refresh_components(i).locator + " with value:" + orefresh_components.refresh_components(i).refresh_value(0, 0))
                            worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.text = orefresh_components.refresh_components(i).refresh_value(0, 0)
                            REM user defined components
                        ElseIf orefresh_components.refresh_components(i).mode = 100 Then
                            REM worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.select()
                            ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Setting Bookmark from rtf: " + orefresh_components.refresh_components(i).locator)
                            If set_selection_from_rtf(orefresh_components.refresh_components(i).locator, orefresh_components.refresh_components(i).refresh_value(0, 0), orefresh_components) = False Then
                                orefresh_components.success = False
                                ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Setting Bookmark:" + orefresh_components.refresh_components(i).locator + " with value:" + orefresh_components.refresh_components(i).refresh_value(0, 0))
                            End If
                        ElseIf orefresh_components.refresh_components(i).mode = 2 Or orefresh_components.refresh_components(i).mode = 6 Or orefresh_components.refresh_components(i).mode = 21 Or orefresh_components.refresh_components(i).mode = 26 Or orefresh_components.refresh_components(i).mode = 210 Or orefresh_components.refresh_components(i).mode = 260 Then
                            REM standard display Table
                            REM first check restrictions only for type 2
                            If get_table_from_gallery(orefresh_components.refresh_components(i).mode_param2) = True Then
                                REM Cowan OCT 2012

                                If update_table_display(orefresh_components.refresh_components(i).locator, orefresh_components.refresh_components(i).refresh_value, orefresh_components.refresh_components(i).restriction_mode, orefresh_components.refresh_components(i).restriction_text, orefresh_components.refresh_components(i).mode_param2, orefresh_components.refresh_components(i).binary_files) = False Then
                                    orefresh_components.success = False
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Failed to update table display: " + orefresh_components.refresh_components(i).locator)

                                End If
                            Else
                                orefresh_components.success = False
                                ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Table Display for locator: " + orefresh_components.refresh_components(i).locator + " failed to load")
                            End If
                            REM cowan OCT 2012
                            orefresh_components.refresh_components(i).binary_files.clear()
                        ElseIf orefresh_components.refresh_components(i).mode = 3 Or orefresh_components.refresh_components(i).mode = 31 Or orefresh_components.refresh_components(i).mode = 310 Then

                            If get_chart_from_gallery(orefresh_components.refresh_components(i).mode_param2, orefresh_components.refresh_components(i).refresh_value, editChart, worddocument.name, orefresh_components.refresh_components(i).locator, -1, "", worddocument) = True Then
                                REM update_chart_display(orefresh_components.refresh_components(i).locator, orefresh_components.refresh_components(i).refresh_value)
                                Try
                                    REM try OLE first
                                    worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.PasteSpecial(Link:=False, DataType:=4, Placement:=1, DisplayAsIcon:=False)
                                Catch
                                    Try
                                        ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Using Alternate Paste.")
                                        worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.paste()

                                    Catch ex As Exception

                                        orefresh_components.success = False
                                        ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "paste failed: " + ex.Message)

                                    End Try
                                End Try

                                REM size chart to cell size
                                Try
                                    placeholder = orefresh_components.refresh_components(i).locator
                                    worddocument.Bookmarks(placeholder).Range.ShapeRange.LockAspectRatio = 0
                                    worddocument.Bookmarks(placeholder).Range.ShapeRange(1).Width = worddocument.Bookmarks(placeholder).Range.Tables(1).Cell(worddocument.Bookmarks(placeholder).Range.Information(13), worddocument.Bookmarks(placeholder).Range.Information(17)).Width
                                    worddocument.Bookmarks(placeholder).Range.ShapeRange(1).Height = worddocument.Bookmarks(placeholder).Range.Tables(1).Cell(worddocument.Bookmarks(placeholder).Range.Information(13), worddocument.Bookmarks(placeholder).Range.Information(17)).Height
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Sizing chart to " + CStr(worddocument.Bookmarks(placeholder).Range.ShapeRange(1).Width) + "," + CStr(worddocument.Bookmarks(placeholder).Range.ShapeRange(1).height))
                                Catch ex As Exception
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Failed to auto size chart: " + ex.Message)
                                End Try

                                If Not editChart Then
                                    Me.close_chart()
                                End If
                            ElseIf orefresh_components.refresh_components(i).mode = 1000 Then
                                REM dynamic(displays)

                            Else
                                orefresh_components.success = False
                                ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Chart Display for locator: " + orefresh_components.refresh_components(i).locator + " failed to load")
                            End If
                        End If
                    ElseIf orefresh_components.refresh_components(i).mode = 4 Or orefresh_components.refresh_components(i).mode = 5 Or orefresh_components.refresh_components(i).mode = 12 Then

                        REM save image to staging area
                        Dim fs As New bc_cs_file_transfer_services
                        If worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.InlineShapes.count > 0 Then
                            worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.InlineShapes(1).delete()
                        End If
                        If IsNothing(orefresh_components.refresh_components(i).objbyte) = False Then

                            REM SW cope with office versions
                            extensionsize = 0
                            extensionsize = (Len(orefresh_components.refresh_components(i).mode_param2) - (InStrRev(orefresh_components.refresh_components(i).mode_param2, ".") - 1))
                            fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + "tmp" + Right(orefresh_components.refresh_components(i).mode_param2, extensionsize), orefresh_components.refresh_components(i).objbyte, Nothing)

                            REM insert it into document
                            'If worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.InlineShapes.count > 0 Then
                            'worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.InlineShapes(1).delete()
                            'End If
                            If orefresh_components.refresh_components(i).mode_param2 = ".txt" Then
                                REM add .txt file
                                worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.text = fs.write_document_to_string(bc_cs_central_settings.local_repos_path + "tmp" + Right(orefresh_components.refresh_components(i).mode_param2, extensionsize))
                            Else
                                Try
                                    worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.InlineShapes.AddPicture(bc_cs_central_settings.local_repos_path + "tmp" + Right(orefresh_components.refresh_components(i).mode_param2, extensionsize))
                                Catch
                                    orefresh_components.success = False
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Inline Shape Could not be inserted at locator:" + orefresh_components.refresh_components(i).locator)
                                End Try

                            End If

                            REM delete from stage
                            Dim estr As String
                            estr = fs.delete_file(bc_cs_central_settings.local_repos_path + "tmp" + Right(orefresh_components.refresh_components(i).mode_param2, extensionsize))
                            If estr <> "" Then
                                ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "could not delete temp file: " + bc_cs_central_settings.local_repos_path + "tmp" + Right(orefresh_components.refresh_components(i).mode_param2, extensionsize) + " as in locked in use")
                            End If
                            orefresh_components.refresh_components(i).objbyte = Nothing
                        Else
                            ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Inline Shape not found for locator:" + orefresh_components.refresh_components(i).locator)
                        End If
                        REM document insertion
                    ElseIf orefresh_components.refresh_components(i).mode = 7 Then
                        Dim fs As New bc_cs_file_transfer_services
                        If IsArray(orefresh_components.refresh_components(i).objbyte) Then
                            fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + "tmp" + orefresh_components.refresh_components(i).mode_param2, orefresh_components.refresh_components(i).objbyte, Nothing)
                            REM insertion can only work if not already there the way this is done id to check for a table
                            REM if it exists table table out and insert file from here
                            If worddocument.Bookmarks.exists(orefresh_components.refresh_components(i).locator) Then
                                If worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.tables.count = 1 Then
                                    worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.select()
                                    worddocument.Bookmarks(orefresh_components.refresh_components(i).locator).range.tables(1).delete()
                                    worddocument.Application.Selection.InsertFile(FileName:=bc_cs_central_settings.local_repos_path + "tmp" + orefresh_components.refresh_components(i).mode_param2, Range:="", _
                                                              ConfirmConversions:=False, Link:=False, Attachment:=False)

                                End If
                                REM delete from stage
                                fs.delete_file(bc_cs_central_settings.local_repos_path + "tmp" + orefresh_components.refresh_components(i).mode_param2)
                            End If
                        End If
                    End If

                    'orefresh_components.refresh_components(i).last_refresh_date = Format(Date.Now, "dd-MMM-yyyy HH:mm:ss")
                    orefresh_components.refresh_components(i).last_refresh_date = Now.ToUniversalTime
                    Try
                        REM EFG June 2012
                        If orefresh_components.refresh_components(i).new_components.Count > 0 Then
                            set_new_components(orefresh_components.refresh_components(i), orefresh_components.success)
                        Else
                            If orefresh_components.refresh_type = 2 And orefresh_components.refresh_components(i).mode = 3 Then
                                remove_additional_components(orefresh_components.refresh_components(i))
                            End If
                        End If
                    Catch
                        orefresh_components.success = False
                    End Try
                    REM EFG June 2012
                End If
            Next


            REM set breaks in tabkes taht requre them
            'set_break_tables()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "set_component_markup", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.enable_track_changes()
            worddocument.activate()
            'orefresh_components.new_components.Clear()
            MyBase.close_table_gallery()
            MyBase.close_excel()


            worddocument.application.screenupdating = True
            otrace = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    REM EFG June 2012
    Private Sub remove_additional_components(ByVal orefresh_component As bc_om_refresh_component)
        Dim prev_stem As String = ""
        Try
            prev_stem = worddocument.CustomDocumentProperties("rnet_ad_stem").value
        Catch

        End Try

        If prev_stem <> "" Then
            REM PR only for type 3 
            If orefresh_component.mode = 3 Then
                For k = 1 To 400
                    If worddocument.Bookmarks.exists(prev_stem + CStr(k) + "_ins") Then
                        worddocument.Bookmarks(prev_stem + CStr(k) + "_ins").range.tables(1).delete()
                    End If
                Next
            End If
        End If
    End Sub
    Private Sub set_new_components(ByVal orefresh_component As bc_om_refresh_component, ByRef successs As Boolean)

        REM new displays to be inserted
        REM firstly clear out old displays
        REM only do if insert file exists
        REM if continuous disclosure insetion delete  charts not i
        REM if continuous disclosure insetion delete  charts not i
        Dim bm_stem As String = ""
        Dim found_stem As Boolean = False
        Dim prev_stem As String

        Dim placeholder_found As Boolean
        Dim placeholder As String = ""
        Dim officeExstention As String = ""
        Dim extensionsize As Integer = 0

        Dim rpcbookmatk As String

        Const chart_placeholder_count As Integer = 20

        Dim ocommentary As New bc_cs_activity_log("bc_ao_word", "set_new_components", bc_cs_activity_codes.COMMENTARY, "Setting Component Values into word")
        prev_stem = ""

        rpcbookmatk = orefresh_component.new_components(0).mode_param1

        Try
            prev_stem = worddocument.CustomDocumentProperties("rnet_ad_stem").value
        Catch

        End Try

        If prev_stem <> "" Then
            REM PR only for type 3 
            If orefresh_component.new_components(0).mode = 3 Then
                For k = 1 To 400
                    If worddocument.Bookmarks.exists(prev_stem + CStr(k) + "_ins") Then
                        worddocument.Bookmarks(prev_stem + CStr(k) + "_ins").range.tables(1).delete()
                    End If
                Next
            End If
        End If

        If orefresh_component.new_components.Count > 0 Then
            If worddocument.bookmarks.exists(rpcbookmatk) Then

                REM SW cope with office versions
                extensionsize = 0
                extensionsize = (Len(orefresh_component.new_components(0).mode_param2) - (InStrRev(orefresh_component.new_components(0).mode_param2, ".") - 1))
                'bm_stem = Left(orefresh_component.new_components(0).mode_param2, Len(orefresh_component.new_components(0).mode_param2) - 4)
                If orefresh_component.new_components(0).mode = 3 Then
                    bm_stem = Left(orefresh_component.new_components(0).mode_param2, Len(orefresh_component.new_components(0).mode_param2) - extensionsize)
                ElseIf orefresh_component.new_components(0).mode = 5 Then
                    bm_stem = orefresh_component.new_components(0).mode_param1()
                End If

                Try
                    worddocument.CustomDocumentProperties.add("rnet_ad_stem", False, 4, "")
                Catch
                End Try
                worddocument.CustomDocumentProperties("rnet_ad_stem").value = bm_stem

                If orefresh_component.new_components(0).mode = 3 Then
                    For k = 1 To 400
                        If worddocument.Bookmarks.exists(bm_stem + CStr(k) + "_ins") Then
                            worddocument.Bookmarks(bm_stem + CStr(k) + "_ins").range.tables(1).delete()
                        End If
                    Next
                    REM take out spaces

                    Try
                        worddocument.bookmarks(rpcbookmatk).range.select()
                        For i = 0 To 400
                            worddocument.Application.Selection.TypeBackspace()
                            If CStr(worddocument.application.selection.style.namelocal) = "disc_space" Then
                                worddocument.application.selection.delete()
                            End If
                        Next
                        worddocument.application.selection.text = vbCr
                        worddocument.application.selection.text = vbCr
                        worddocument.application.selection.movedown(5, 1)
                        worddocument.application.selection.bookmarks.add(rpcbookmatk)
                        worddocument.application.selection.text = vbCr
                    Catch ex As Exception
                        ocommentary = New bc_cs_activity_log("bc_am_at_ao", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Failed to delete blank rows on additional chart reset: " + ex.Message)
                    End Try
                End If
            End If
        End If

        For i = 0 To orefresh_component.new_components.Count - 1
            Me.update_refresh_status_bar("Refreshing additional display: ", CStr(i + 1), orefresh_component.new_components.Count)
            If (Not IsNothing(orefresh_component.new_components(i).refresh_value) And orefresh_component.new_components(i).mode = 3) Or (Not IsNothing(orefresh_component.new_components(i).objbyte) And orefresh_component.new_components(i).mode = 5) Then
                If orefresh_component.new_components(i).mode = 3 Or orefresh_component.new_components(i).mode = 5 Then
                    ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Attempting to add additional display:" + CStr(i + 1))
                    REM standard display Table
                    Dim insert As Boolean = False
                    If orefresh_component.new_components(i).mode = 3 Then
                        If get_chart_from_gallery(orefresh_component.new_components(i).mode_param2, orefresh_component.new_components(i).refresh_value, False, "", "", -1, "", worddocument) = True Then
                            insert = True
                        End If

                    ElseIf orefresh_component.new_components(i).mode = 5 Then
                        insert = True
                    End If

                    If insert = True Then

                        REM SW cope with office versions
                        extensionsize = 0
                        extensionsize = (Len(orefresh_component.new_components(i).mode_param2) - (InStrRev(orefresh_component.new_components(i).mode_param2, ".") - 1))

                        If orefresh_component.new_components(i).mode = 3 Then
                            bm_stem = Left(orefresh_component.new_components(i).mode_param2, Len(orefresh_component.new_components(i).mode_param2) - extensionsize)
                            'bm_stem = Left(orefresh_component.new_components(i).mode_param2, Len(orefresh_component.new_components(i).mode_param2) - 4)
                        ElseIf orefresh_component.new_components(i).mode = 5 Then
                            bm_stem = orefresh_component.new_components(i).mode_param1
                        End If

                        REM need to see if placeholder exists for this
                        REM allow up to the number of place holders specified by the chart_placeholder_count constant
                        placeholder_found = False
                        REM see if chart is already has place holder reset
                        If worddocument.bookmarks.exists(bm_stem + CStr(i + 1) + "_ins") Then
                            Dim col As Integer
                            Dim row As Integer
                            col = worddocument.Bookmarks(bm_stem + CStr(i + 1) + "_ins").Range.Information(17)
                            row = worddocument.Bookmarks(bm_stem + CStr(i + 1) + "_ins").Range.Information(13)
                            worddocument.Bookmarks(bm_stem + CStr(i + 1) + "_ins").range.tables(1).cell(row, col).range.bookmarks.add("tmp")
                            If orefresh_component.new_components(i).mode = 3 Then
                                worddocument.bookmarks(bm_stem + CStr(i + 1) + "_ins").range.delete()
                            End If
                            placeholder = bm_stem + CStr(i + 1) + "_ins"
                            worddocument.Bookmarks("tmp").range.bookmarks.add(bm_stem + CStr(i + 1) + "_ins")
                            worddocument.Bookmarks("tmp").delete()
                            placeholder_found = True
                        End If
                        If placeholder_found = False Then
                            For j = 1 To chart_placeholder_count
                                If worddocument.bookmarks.exists(bm_stem + CStr(j)) Then
                                    placeholder = bm_stem + CStr(j)
                                    placeholder_found = True
                                    Exit For
                                End If
                            Next
                        End If
                        If placeholder_found = False Then

                            REM SW cope with office versions
                            If bc_cs_central_settings.userOfficeStatus = 2 Then
                                officeExstention = ".docx"
                            Else
                                officeExstention = ".doc"
                            End If

                            REM insert another place holder page
                            Dim fs As New bc_cs_file_transfer_services
                            If fs.check_document_exists(bc_cs_central_settings.local_template_path + bm_stem + officeExstention) = False Then
                                successs = False
                                ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "no insert page:" + bc_cs_central_settings.local_repos_path + bm_stem + officeExstention + " so multiple charts not inserted")
                                Me.close_chart()
                                Exit Sub
                            End If
                            ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Attempting to add insert page:" + bc_cs_central_settings.local_repos_path + bm_stem + officeExstention)
                            REM insert at end unless specified
                            If worddocument.bookmarks.exists("rpcstart") Then
                                insert_file(bc_cs_central_settings.local_template_path + bm_stem + officeExstention, True, True, "rpcstart")
                            ElseIf worddocument.bookmarks.exists(rpcbookmatk) Then
                                REM insert without page break
                                insert_file(bc_cs_central_settings.local_template_path + bm_stem + officeExstention, False, True, rpcbookmatk)
                            Else
                                insert_file(bc_cs_central_settings.local_template_path + bm_stem + officeExstention, True, True)
                            End If
                            placeholder = bm_stem + "1"
                        End If
                        ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Attempting to place chart at locator:" + placeholder)
                        REM if not need to nsert a new page to provide place holder


                        If worddocument.bookmarks.exists(rpcbookmatk) Then
                            Try
                                Try
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, rpcbookmatk + ":Using Alternate Paste.")
                                    If orefresh_component.new_components(i).mode = 3 Then
                                        worddocument.Bookmarks(placeholder).range.PasteSpecial(Link:=False, DataType:=4, Placement:=1, DisplayAsIcon:=False)
                                    Else
                                        'PR TBD insert from binary
                                        Dim fs As New bc_cs_file_transfer_services
                                        If worddocument.Bookmarks(placeholder).range.InlineShapes.count > 0 Then
                                            worddocument.Bookmarks(placeholder).range.InlineShapes(1).delete()
                                        End If
                                        If IsNothing(orefresh_component.new_components(i).objbyte) = False Then

                                            REM SW cope with office versions
                                            extensionsize = 0
                                            extensionsize = (Len(orefresh_component.new_components(i).mode_param2) - (InStrRev(orefresh_component.new_components(i).mode_param2, ".") - 1))
                                            fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + "tmp" + Right(orefresh_component.new_components(i).mode_param2, extensionsize), orefresh_component.new_components(i).objbyte, Nothing)

                                            REM insert it into document
                                            'If worddocument.Bookmarks(orefresh_component.refresh_components(i).locator).range.InlineShapes.count > 0 Then
                                            'worddocument.Bookmarks(orefresh_component.refresh_components(i).locator).range.InlineShapes(1).delete()
                                            'End If
                                            If orefresh_component.new_components(i).mode_param2 = ".txt" Then
                                                REM add .txt file
                                                worddocument.Bookmarks(placeholder).range.text = fs.write_document_to_string(bc_cs_central_settings.local_repos_path + "tmp" + Right(orefresh_component.new_components(i).mode_param2, extensionsize))
                                            Else
                                                Try
                                                    worddocument.Bookmarks(placeholder).range.InlineShapes.AddPicture(bc_cs_central_settings.local_repos_path + "tmp" + Right(orefresh_component.new_components(i).mode_param2, extensionsize))
                                                Catch
                                                    successs = False
                                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Inline Shape Could not be inserted at locator:" + placeholder)
                                                End Try
                                            End If

                                            REM delete from stage
                                            Dim estr As String
                                            estr = fs.delete_file(bc_cs_central_settings.local_repos_path + "tmp" + Right(orefresh_component.new_components(i).mode_param2, extensionsize))
                                            If estr <> "" Then
                                                successs = False
                                                ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "could not delete temp file: " + bc_cs_central_settings.local_repos_path + "tmp" + Right(orefresh_component.new_components(i).mode_param2, extensionsize) + " as in locked in use")
                                            End If
                                            orefresh_component.new_components(i).objbyte = Nothing
                                        End If
                                    End If
                                Catch ex As Exception
                                    If orefresh_component.new_components(i).mode = 3 Then
                                        worddocument.bookmarks(placeholder).range.paste()
                                    End If
                                End Try
                                REM pr changed 08/11 as very flakey on the correct shape
                                Dim is_failed As Boolean
                                is_failed = False


                                If orefresh_component.new_components(i).mode_param2 <> ".txt" Then

                                    'EFG June 2012 only nee dto vary the height here as width should be set in table and insert file
                                    'Try
                                    '    MsgBox("1")
                                    '    'worddocument.Bookmarks(placeholder).Range.Tables(1).Cell(1, 2).Range.InlineShapes(1).width = worddocument.Bookmarks(placeholder).Range.Tables(1).Cell(1, worddocument.Bookmarks(placeholder).Range.Information(17)).Width()
                                    '    MsgBox("2")
                                    'Catch
                                    '    is_failed = True
                                    'End Try
                                    If is_failed = False Then
                                        Try
                                            worddocument.Bookmarks(placeholder).Range.Tables(1).Cell(1, worddocument.Bookmarks(placeholder).Range.Information(17)).height = worddocument.Bookmarks(placeholder).Range.Tables(1).Cell(1, 2).Range.InlineShapes(1).height
                                        Catch
                                            successs = False
                                            ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Inline Shape Could not be inserted at locator:" + placeholder)
                                            is_failed = True
                                        End Try
                                    End If

                                    If is_failed = True Then
                                        Dim co As Integer
                                        co = worddocument.Shapes.Count
                                        worddocument.Shapes(co).Width = worddocument.Bookmarks(placeholder).Range.Tables(1).Cell(1, worddocument.Bookmarks(placeholder).Range.Information(17)).Width
                                        Try
                                            worddocument.Bookmarks(placeholder).Range.Tables(1).Cell(1, worddocument.Bookmarks(placeholder).Range.Information(17)).height = worddocument.Shapes(co).height
                                        Catch ex As Exception
                                            MsgBox(ex.Message)
                                        End Try
                                    End If
                                End If


                            Catch ex As Exception
                                successs = False
                                ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "paste failed: " + ex.Message)
                            End Try
                        Else
                            Try
                                If orefresh_component.new_components(i).mode = 3 Then
                                    worddocument.Bookmarks(placeholder).range.PasteSpecial(Link:=False, DataType:=4, Placement:=1, DisplayAsIcon:=False)
                                Else
                                    REM not yet implemented for type 5
                                End If
                            Catch
                                Try
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Using Alternate Paste.")
                                    worddocument.Bookmarks(placeholder).range.paste()
                                Catch ex As Exception
                                    successs = False
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "paste failed: " + ex.Message)
                                End Try
                            End Try
                        End If

                        REM size chart to cell size
                        Try
                            worddocument.Bookmarks(placeholder).Range.ShapeRange.LockAspectRatio = 0
                            worddocument.Bookmarks(placeholder).Range.ShapeRange(1).Width = worddocument.Bookmarks(placeholder).Range.Tables(1).Cell(worddocument.Bookmarks(placeholder).Range.Information(13), worddocument.Bookmarks(placeholder).Range.Information(17)).Width
                            worddocument.Bookmarks(placeholder).Range.ShapeRange(1).Height = worddocument.Bookmarks(placeholder).Range.Tables(1).Cell(worddocument.Bookmarks(placeholder).Range.Information(13), worddocument.Bookmarks(placeholder).Range.Information(17)).Height
                            ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Sizing chart to " + CStr(worddocument.Bookmarks(placeholder).Range.ShapeRange(1).Width) + "," + CStr(worddocument.Bookmarks(placeholder).Range.ShapeRange(1).height))
                        Catch ex As Exception
                            ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Failed to auto size chart: " + ex.Message)
                        End Try
                        If orefresh_component.new_components(i).mode = 3 Then
                            Me.close_chart()
                        End If
                        REM add a now placed bookmark
                        If Right(placeholder, 4) <> "_ins" Then
                            REM add a now placed bookmark
                            worddocument.Bookmarks(placeholder).range.bookmarks.add(bm_stem + CStr(i + 1) + "_ins")
                            worddocument.Bookmarks(placeholder).delete()
                        End If
                        REM later on consider adding a proper locator
                    Else
                        successs = False
                        ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Chart Display for locator: " + orefresh_component.new_components(i).locator + " failed to load")
                    End If
                End If
            End If
            If i = orefresh_component.new_components.Count - 1 Then
                REM delete spares
                For k = i + 2 To 400
                    If worddocument.Bookmarks.exists(bm_stem + CStr(k) + "_ins") Then
                        worddocument.Bookmarks(bm_stem + CStr(k) + "_ins").range.tables(1).cell(1, 1).range.bookmarks.add("tmp")
                        worddocument.bookmarks(bm_stem + CStr(k) + "_ins").range.delete()
                        placeholder = bm_stem + CStr(k) + "_ins"
                        worddocument.Bookmarks("tmp").range.bookmarks.add(bm_stem + CStr(k) + "_ins")
                        worddocument.Bookmarks("tmp").delete()
                    End If
                Next
            End If
            REM clear down values
            orefresh_component.value = Nothing
            orefresh_component.objbyte = Nothing
        Next
    End Sub
    Private Sub remove_break_tables(ByVal id As String)
        Try
            Dim bms As New ArrayList
            Dim pn As Integer
            Dim maxbs As Integer

            For i = 1 To worddocument.bookmarks.count
                If Left(worddocument.bookmarks(i).name, 11) = "rnet_break" + id Then
                    bms.Add(worddocument.bookmarks(i).name)
                End If
            Next
            For i = 0 To bms.Count
                worddocument.bookmarks(bms(i)).range.select()
                worddocument.bookmarks(bms(i)).range.tables(1).delete()
                pn = worddocument.application.Selection.Information(3)
                maxbs = 0
                While worddocument.application.Selection.Information(3) = pn And maxbs < 100
                    worddocument.application.selection.TypeBackspace()
                    maxbs = maxbs + 1
                End While
            Next
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "remove_break_tables", bc_cs_activity_codes.TRACE_EXIT, ex.Message)
        End Try
    End Sub
    Private Sub set_break_tables(ByVal tid As String)
        Try
            Dim na As String
            For i = 1 To worddocument.bookmarks.count
                If Len(worddocument.bookmarks(i).name) >= 10 Then
                    If Left(worddocument.bookmarks(i).name, 11) = "rnet_break" + tid Then
                        If worddocument.bookmarks(i).Range.Information(14) > 1 Then
                            worddocument.bookmarks(i).range.InsertBreak()
                            na = worddocument.bookmarks(i).Name
                            worddocument.bookmarks(i).range.Tables(1).Cell(1, 1).Range.Bookmarks.Add(na)
                        End If
                    End If
                End If
            Next
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "set_break_tables", bc_cs_activity_codes.TRACE_EXIT, ex.Message)
        End Try
    End Sub
    REM updates a Standard Display Table
    REM Cowan OCT 2012
    Private hyperlinks As New List(Of cellhyperlink)
    Private Class cellhyperlink
        Public name As String
        Public fullname As String
        Public startword As Integer

    End Class
    Public Function update_table_display(ByVal locator As String, ByVal values As Object, ByVal restriction As Integer, _
                                    ByVal restriction_text As String, ByVal tableLocator As String, ByVal binary_files As ArrayList) As Boolean


        update_table_display = True
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim i As Integer
        Dim index As String
        Dim url As String
        Dim dn As String
        Dim numrows As Integer
        Dim multitable As String
        Dim btm As String
        Dim tid As String
        dn = ""
        Dim ocommentary As bc_cs_activity_log
        Try
            Try
                worddocument.update_table_display(worddocument, locator, values, restriction, restriction_text, tableLocator)
                Exit Function
            Catch ex As Exception

                Dim ocomm As New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Failed to call update_table_display in document using built it: " + ex.Message)


            End Try

            REM select current table and remove it
            worddocument.bookmarks(locator).range.tables(1).select()
            REM see if it a break table
            For i = 1 To worddocument.bookmarks(locator).range.tables(1).range.bookmarks.count
                btm = worddocument.bookmarks(locator).range.tables(1).range.bookmarks(i).name

                If Len(btm) >= 13 Then
                    If Left(btm, 13) = "rnet_breakref" Then
                        tid = Right(btm, Len(btm) - 13)
                        remove_break_tables(tid)
                        Exit For
                    End If
                End If

            Next

            worddocument.bookmarks(locator).range.tables(1).delete()
            REM fix for composites.  Cannot use table locator
            worddocument.application.selection.bookmarks.add(locator)
            worddocument.bookmarks(locator).range.select()
            REM paste in new table
            worddocument.application.selection.paste()
            REM add locator bookmark back
            Dim orange As Object
            Dim mrange As Object
            ' ensure you have the correct document selection object (using locator and not table locator)
            worddocument.bookmarks(locator).range.tables(1).select()
            orange = worddocument.Application.Selection.Tables(1).Cell(1, 1).range
            orange.bookmarks.add(locator, orange)
            REM restrictions
            If restriction = 1 Then
                ocommentary = New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Restriction so table not displayed")
                worddocument.application.selection.tables(1).cell(1, 1).range.text = restriction_text
                Exit Function
            End If
            If restriction = 2 Then
                ocommentary = New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Restriction so table not displayed")
                Exit Function
            End If
            REM add data
            Dim rtf As Boolean = False
            Dim num_rows As Integer
            num_rows = worddocument.Application.Selection.Tables(1).rows.count
            For i = 0 To UBound(values, 2)
                hyperlinks.Clear()

                Try
                    If CInt(values(1, i)) > num_rows Then
                        orange = worddocument.Application.Selection.Tables(1).Cell(num_rows, values(2, i)).range
                        For j = 1 To CInt(values(1, i)) - num_rows
                            orange.rows.add()
                        Next
                        num_rows = worddocument.Application.Selection.Tables(1).rows.count
                    End If
                    orange = worddocument.Application.Selection.Tables(1).Cell(values(1, i), values(2, i)).range
                    If Not IsNothing(values(0, i)) Then
                        REM check if item is to be indexed
                        index = 0
                        If Len(values(0, i)) > 6 Then
                            If Left(values(0, i), 4) = "[ind" Then
                                values(0, i) = Right(values(0, i), Len(values(0, i)) - 4)
                                index = Left(values(0, i), 1)
                                values(0, i) = Right(values(0, i), Len(values(0, i)) - 2)
                                orange.bookmarks.add("rnet_ind" + CStr(index) + "_" + CStr(Format(Now, "hhmmss")) + CStr(i))
                            End If
                            If Left(values(0, i), 5) = "[bmk]" Then
                                values(0, i) = Right(values(0, i), Len(values(0, i)) - 5)
                                multitable = Right(values(0, i), Len(values(0, i)))
                                values(0, i) = Right(values(0, i), Len(values(0, i)))
                                orange.bookmarks.add(multitable)
                                values(0, i) = "[bmk]"
                            End If
                            If Left(values(0, i), 9) = "[bmkword]" Then
                                values(0, i) = Right(values(0, i), Len(values(0, i)) - 9)
                                multitable = Right(values(0, i), Len(values(0, i)))
                                values(0, i) = Right(values(0, i), Len(values(0, i)))
                                'orange.text = CStr(values(0, i))
                                'Try
                                ' orange.style = values(3, i)
                                'Catch
                                'ocommentary = New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Style: " + CStr(values(3, i)) + " dont exist")
                                'End Try
                                Try
                                    orange.SetRange(orange.Start, orange.End - 1)
                                    orange.Bookmarks.Add(multitable)
                                    'orange.words(1).bookmarks.add(multitable)
                                    values(0, i) = "[bmkword]"
                                    REM worddocument.Application.Selection.Tables(1).Cell(values(1, i), values(2, i)).range.words(1).bookmarks.add(multitable)
                                Catch ex As Exception
                                    Dim ocomm As New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "failed to set word bookmark:" + CStr(values(0, i)))

                                End Try
                            End If
                            REM cross referance
                            If Left(values(0, i), 6) = "[xref]" Then
                                values(0, i) = Right(values(0, i), Len(values(0, i)) - 6)
                                Try
                                    orange.InsertCrossReference(ReferenceType:="Bookmark", ReferenceKind:= _
                                        -1, ReferenceItem:=CStr(values(0, i)), InsertAsHyperlink:=True, _
                                        IncludePosition:=False, SeparateNumbers:=False, SeparatorString:=" ")
                                    Try
                                        orange.style = values(3, i)
                                    Catch
                                        ocommentary = New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Style: " + CStr(values(3, i)) + " dont exist")
                                    End Try
                                    values(0, i) = "[xref]"
                                Catch
                                    values(0, i) = "ref not found"
                                    Dim ocomm As New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "failed to set Xref to bookmark:" + CStr(values(0, i)) + " doesnt exists")
                                End Try
                            End If
                            REM hyperlink
                            If Left(values(0, i), 11) = "[hyperlink]" Then
                                values(0, i) = Right(values(0, i), Len(values(0, i)) - 11)
                                REM seperate out URL and display name
                                url = Left(values(0, i), InStr(values(0, i), "[") - 1)
                                Try
                                    values(0, i) = Right(values(0, i), Len(values(0, i)) - InStr(values(0, i), "["))
                                    dn = Left(values(0, i), Len(values(0, i)) - 1)
                                    dn = LTrim(RTrim(dn))
                                    orange.Hyperlinks.Add(orange, url, "", "", CStr(dn))
                                    Try
                                        orange.style = values(3, i)
                                    Catch
                                        ocommentary = New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Style: " + CStr(values(3, i)) + " dont exist")
                                    End Try
                                Catch ex As Exception
                                    values(0, i) = "hyperlink failed"
                                    Dim ocomm As New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Hyperlink failed to set url: " + url + " display name " + dn + ": " + ex.Message)
                                End Try
                                values(0, i) = "[hyperlink]"
                            End If

                            REM merge
                            If Left(values(0, i), 10) = "[mergerow]" Then
                                values(0, i) = Right(values(0, i), Len(values(0, i)) - 10)
                                Try
                                    numrows = CInt(values(0, i))
                                    For r = 1 To numrows
                                        mrange = worddocument.Application.Selection.Tables(1).Cell(values(1, i) + r, values(2, i))
                                        worddocument.Application.Selection.Tables(1).Cell(values(1, i), values(2, i)).Merge(mrange)
                                    Next

                                Catch ex As Exception
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Failed to merge rows: " + CStr(values(0, i)) + ": " + ex.Message)
                                End Try
                                values(0, i) = "[mergerow]"
                            End If
                            If Left(values(0, i), 10) = "[mergecol]" Then
                                values(0, i) = Right(values(0, i), Len(values(0, i)) - 10)
                                Try
                                    orange.text = CStr(values(0, i))
                                    Try
                                        orange.style = values(3, i)
                                    Catch
                                        ocommentary = New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Style: " + CStr(values(3, i)) + " dont exist")
                                    End Try
                                    mrange = worddocument.Application.Selection.Tables(1).Cell(values(1, i), values(2, i) + 1)
                                    worddocument.Application.Selection.Tables(1).Cell(values(1, i), values(2, i)).Merge(mrange)
                                Catch ex As Exception
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Failed to merge col: " + CStr(values(0, i)) + ex.Message)
                                End Try
                                values(0, i) = "[mergecol]"
                            End If


                        End If
                        REM rtf in tables Dec 2010
                        rtf = False
                        If CStr(values(0, i)).Length > 5 Then
                            If CStr(values(0, i)).Substring(0, 5) = "[rtf]" Then
                                rtf = True
                                If Me.set_selection_from_rtf(orange, CStr(values(0, i)).Substring(5, CStr(values(0, i)).Length - 5)) = False Then
                                    orange.text = "failed to set from rtf"
                                End If
                            End If
                        End If

                        If rtf = False Then
                            If values(0, i) <> "[bmk]" And values(0, i) <> "[bmkword]" And values(0, i) <> "[hyperlink]" And values(0, i) <> "[xref]" And values(0, i) <> "[mergerow]" And values(0, i) <> "[mergecol]" Then
                                orange.text = CStr(values(0, i))
                                Try
                                    orange.style = values(3, i)
                                Catch
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Style: " + CStr(values(3, i)) + " dont exist")
                                End Try
                                REM check bolding
                                If InStr(1, CStr(values(0, i)), "ZBold") > 0 Then
                                    Dim w As String
                                    For k = 1 To orange.words.Count - 1
                                        w = orange.words(k).Text
                                        If InStr(1, w, "ZBold") > 0 Then
                                            If Len(w) > 5 Then
                                                orange.words(k).Text = Right(w, Len(w) - 5)
                                                orange.words(k).Bold = True
                                            End If
                                        End If
                                    Next

                                End If
                                REM hyper link andemail link

                                If InStr(1, CStr(values(0, i)), "ZHyperLinkStart") > 0 Then
                                    Try
                                        Dim sh, se As Integer
                                        Dim w As String
                                        For k = 1 To orange.words.Count - 1
                                            w = orange.words(k).Text
                                            If InStr(1, w, "ZHyperLinkStart") > 0 Then
                                                sh = k
                                            End If
                                            If InStr(1, w, "ZHyperLinkEnd") > 0 Then
                                                se = k
                                            End If
                                        Next
                                        Dim tx As String = ""
                                        Dim ii As Integer
                                        For ii = sh + 2 To se - 2
                                            tx = tx + orange.words(ii).text
                                        Next
                                        Dim otx As String
                                        otx = Replace(orange.text, "ZHyperLinkStart[" + tx + "]ZHyperLinkEnd", "tmp")
                                        If otx.Length > 2 Then
                                            otx = Left(otx, Len(otx) - 2)
                                        End If
                                        orange.text = RTrim(otx)
                                        Dim hl As New cellhyperlink
                                        hl.name = tx
                                        hl.fullname = tx
                                        hl.startword = sh

                                        hyperlinks.Add(hl)

                                    Catch ex As Exception
                                        Dim ocomm As New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Failed to set hyperlink: " + ex.Message)
                                    End Try
                                End If

                                REM email link
                                If InStr(1, CStr(values(0, i)), "ZEmailLinkStart") > 0 Then
                                    Try
                                        Dim sh, se As Integer
                                        Dim w As String
                                        For k = 1 To orange.words.Count - 1
                                            w = orange.words(k).Text
                                            If InStr(1, w, "ZEmailLinkStart") > 0 Then
                                                sh = k
                                            End If
                                            If InStr(1, w, "ZEmailLinkEnd") > 0 Then
                                                se = k
                                            End If
                                        Next
                                        Dim tx As String = ""
                                        Dim ii As Integer
                                        For ii = sh + 2 To se - 2
                                            tx = tx + orange.words(ii).text
                                        Next

                                        Dim otx As String
                                        otx = Replace(orange.text, "ZEmailLinkStart[" + tx + "]ZEmailLinkEnd", "tmp")
                                        If otx.Length > 2 Then
                                            otx = Left(otx, Len(otx) - 2)
                                        End If
                                        orange.text = RTrim(otx)
                                        Dim hl As New cellhyperlink
                                        hl.fullname = "mailto:" + tx
                                        hl.name = tx
                                        hl.startword = sh
                                        hyperlinks.Add(hl)

                                    Catch ex As Exception
                                        Dim ocomm As New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Failed to set email link: " + ex.Message)
                                    End Try
                                End If
                                Dim chl As Integer
                                chl = hyperlinks.Count - 1
                                For k = 0 To hyperlinks.Count - 1
                                    Try
                                        orange.words(hyperlinks(chl).startword).Hyperlinks.Add(orange.words(hyperlinks(chl).startword), hyperlinks(chl).fullname, , , hyperlinks(chl).name)
                                        chl = chl - 1
                                    Catch ex As Exception
                                        Dim ocomm As New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Failed to set hyperlink " + CStr(k) + ":" + ex.Message)
                                    End Try
                                Next
                            End If


                        End If
                    End If
                Catch
                    ocommentary = New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Coords: " + CStr(values(1, i)) + "," + CStr(values(2, i)) + " dont exist in table at:" + locator)
                End Try
            Next
            REM set widths and breaks
            Dim width As Double = 10.0
            Dim set_cell As Boolean
            For i = 0 To UBound(values, 2)
                Try
                    orange = worddocument.Application.Selection.Tables(1).Cell(values(1, i), values(2, i)).range
                    REM breaks
                    set_cell = False
                    If Len(values(0, i)) > 7 Then
                        If Left(values(0, i), 7) = "[break]" Then
                            values(0, i) = Right(values(0, i), Len(values(0, i)) - 7)
                            orange.bookmarks.add("rnet_break" + CStr(values(0, i)) + "_" + CStr(Format(Now, "hhmmss")) + CStr(i))
                            orange.text = ""
                            set_cell = False
                        End If
                        If Left(values(0, i), 10) = "[breakref]" Then
                            values(0, i) = Right(values(0, i), Len(values(0, i)) - 10)
                            orange.bookmarks.add("rnet_breakref" + CStr(values(0, i)))
                            orange.text = ""
                            set_cell = False
                        End If
                    End If
                    If Len(values(0, i)) > 7 Then
                        If Left(values(0, i), 7) = "[width=" Then
                            values(0, i) = Right(values(0, i), Len(values(0, i)) - 7)
                            Try
                                width = Left(values(0, i), InStr(values(0, i), "]") - 1)
                                REM now change cell width
                                orange.cells.width = width
                            Catch ex As Exception
                                Dim ocomm As New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Failed to get cell width: " + ex.Message)
                            End Try
                            values(0, i) = Right(values(0, i), Len(values(0, i)) - InStr(values(0, i), "]"))
                            set_cell = True
                        End If
                    End If
                    If set_cell = True Then
                        orange.text = CStr(values(0, i))
                    End If
                Catch

                End Try
            Next
            REM cowan OCT 2012
            REM images in cells
            Dim row, col As Integer
            Dim ext As String
            Dim fs As New bc_cs_file_transfer_services

            For i = 0 To binary_files.Count - 1
                row = binary_files(i).row
                col = binary_files(i).col
                ext = binary_files(i).ext
                ocommentary = New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Attempting to insert binary image into(" + CStr(row) + "," + CStr(col) + ")")

                fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + "tmp" + CStr(i) + "." + CStr(ext), binary_files(i).objbyte, Nothing)

                orange = worddocument.Application.Selection.Tables(1).Cell(row, col).range


                Try
                    orange.text = ""

                    Application.DoEvents()

                    orange.InlineShapes.AddPicture(bc_cs_central_settings.local_repos_path + "tmp" + CStr(i) + "." + CStr(ext), False)
               

                Catch ex As Exception
                    ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Inline Shape Could not be inserted")
                End Try


                REM delete from stage
                Dim estr As String

                estr = fs.delete_file(bc_cs_central_settings.local_repos_path + "tmp" + CStr(i) + "." + CStr(ext))
                If estr <> "" Then
                    ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "could not delete temp file: tmp." + CStr(ext) + " as in locked in use")
                End If
            Next

            REM delete rows marked for deletion
            Try
                For i = worddocument.Application.Selection.Tables(1).rows.count To 1 Step -1
                    If Left(worddocument.Application.Selection.Tables(1).Rows(i).Range.Text, 10) = "Delete Row" Then
                        worddocument.Application.Selection.Tables(1).Rows(i).Delete()
                    End If
                Next
            Catch

            End Try

            If worddocument.Application.Selection.Tables.count = 0 Then
                Exit Function
            End If
            REM see if it a break table

            Dim break_table As Boolean
            break_table = False
            For i = 1 To worddocument.bookmarks(locator).range.tables(1).range.bookmarks.count
                btm = worddocument.bookmarks(locator).range.tables(1).range.bookmarks(i).name
                If Len(btm) >= 13 Then
                    If Left(btm, 13) = "rnet_breakref" Then
                        REM get id of table
                        If Left(btm, 13) = "rnet_breakref" Then
                            tid = Right(btm, Len(btm) - 13)
                            set_break_tables(tid)
                            Exit For
                        End If
                    End If
                End If
            Next


        Catch ex As Exception
            update_table_display = False

            Dim db_err As New bc_cs_error_log("bc_ao_word", "update_table_display", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "update_table_display", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Function insert_landscape_page() As Boolean
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "insert_landscape_page", bc_cs_activity_codes.TRACE_ENTRY, "")
        insert_landscape_page = False
        Try
            worddocument.Application.Selection.InsertBreak(Type:=2)
            worddocument.Application.Selection.InsertBreak(Type:=2)
            worddocument.Application.Selection.MoveUp(Unit:=5, Count:=1)
            worddocument.Application.Selection.PageSetup.Orientation = 1
            Try
                worddocument.after_landscape_insert(bc_cs_central_settings.local_template_path)
            Catch
                Dim ocomm As New bc_cs_activity_log("bc_ao_word", "insert_landscape_page", bc_cs_activity_codes.COMMENTARY, "No after_landscape_insert in document")
            End Try
            insert_landscape_page = True
        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_word", "insert_landscape_page", bc_cs_activity_codes.COMMENTARY, "landscape page failed: " + ex.Message)
            Dim Omsg As New bc_cs_message("Blue Curve", "Failed to insert landscape page at current selection point", bc_cs_message.MESSAGE)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "insert_landscape_page", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Sub accept_changes()
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "accept_changes", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim pro As Integer
            pro = worddocument.ProtectionType
            If pro = 2 Then
                Try
                    worddocument.unprotect()
                Catch

                End Try
            End If
            worddocument.AcceptAllRevisions()
            worddocument.TrackRevisions = False
            If pro = 2 Then
                Try
                    worddocument.Protect(2)
                Catch

                End Try
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "accept_changes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "accept_changes", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Overrides Function server_connected() As String
        server_connected = ""
    End Function

    Public Overrides Sub set_file_name()

    End Sub
    Public Overrides Function set_selection(ByVal bm As String) As Boolean
        Try
            worddocument.Bookmarks(bm).range.select()
            worddocument.Bookmarks(bm).range.tables(1).delete()
            set_selection = True
        Catch ex As Exception
            MsgBox(ex.Message)
            set_selection = False
        End Try

    End Function

    Public Overrides Sub new_template()

        Dim log As New bc_cs_activity_log("bc_ao_word", "new_template", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            If word Is Nothing Then
                invoke_word()
            End If
            worddocument = word.documents.Add(, True, 0, True)
            ' set to print layout
            word.activewindow.view.type = 3

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "new_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "new_template", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub add_references()

        Dim log = New bc_cs_activity_log("bc_ao_word", "add_references", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            ' add reference to autotext checking it exists first
            If checkAutoTextExists() Then
                If bc_cs_central_settings.userOfficeStatus = 2 Then
                    worddocument.VBProject.References.AddFromFile(String.Concat(bc_cs_central_settings.local_template_path, "autotext.dotm"))
                    'ensure it's always loaded when template is opened
                    word.addins.add(String.Concat(bc_cs_central_settings.local_template_path, "autotext.dotm"))
                Else
                    worddocument.VBProject.References.AddFromFile(String.Concat(bc_cs_central_settings.local_template_path, "autotext.dot"))
                    'ensure it's always loaded when template is opened
                    word.addins.add(String.Concat(bc_cs_central_settings.local_template_path, "autotext.dot"))
                End If
            Else
                ' autotext not found so alert the user
                Dim errLog As New bc_cs_error_log("bc_ao_word", "add_references", bc_cs_error_codes.USER_DEFINED, "Autotext file not found, reference not added.")
            End If

            ' add default code
            worddocument.VBProject.VBComponents(1).CodeModule.AddFromFile(String.Concat(bc_cs_central_settings.local_template_path, "default_code.txt"))

        Catch ex As System.Runtime.InteropServices.COMException
            ' references already exists
            Dim errLog As New bc_cs_error_log("bc_ao_word", "add_references", bc_cs_error_codes.USER_DEFINED, String.Concat(ex.Message, vbCrLf, "Please check Word macro settings / VBA"))
        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "add_references", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "add_references", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub add_properties(ByVal templateID As Integer, ByVal templateName As String)

        Dim log = New bc_cs_activity_log("bc_ao_word", "add_properties", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            ' add the required custom properties
            worddocument.CustomDocumentProperties.add("rnet_template_id", False, 1, 0)
            worddocument.CustomDocumentProperties("rnet_template_id") = templateID
            worddocument.CustomDocumentProperties.add("xrnet_attached_template", False, 4, "")
            worddocument.CustomDocumentProperties("xrnet_attached_template") = templateName

        Catch ex As System.Runtime.InteropServices.COMException
            ' properties already exists
            log = New bc_cs_activity_log("bc_ao_word", "add_properties", bc_cs_activity_codes.COMMENTARY, ex.Message)
        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "add_properties", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "add_properties", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub update_properties(ByVal templateID As Integer, ByVal templateName As String)

        Dim log = New bc_cs_activity_log("bc_ao_word", "update_properties", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            ' update the required custom properties
            worddocument.CustomDocumentProperties("rnet_template_id") = templateID
            worddocument.CustomDocumentProperties("xrnet_attached_template") = templateName

        Catch ex As ArgumentException
            ' ignore as not all existing templates will have the 'xrnet_attached_template' property
            log = New bc_cs_activity_log("bc_ao_word", "update_properties", bc_cs_activity_codes.COMMENTARY, ex.Message)
        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "update_properties", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            ' don't prompt to save document
            worddocument.Saved = True
            Application.DoEvents()
            log = New bc_cs_activity_log("bc_ao_word", "update_properties", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub save_template(ByVal templateName As String)
        Dim log As New bc_cs_activity_log("bc_ao_word", "save_template", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim officeFormat As Integer = 0

        Try
            REM SW cope with office versions
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                officeFormat = 15
            Else
                officeFormat = 1
            End If
            'If bc_cs_central_settings.office_version < 12 And bc_cs_central_settings.user_office_version >= 12 Then
            '    officeFormat = 1
            'ElseIf bc_cs_central_settings.office_version < 12 Then
            '    officeFormat = 1
            'ElseIf bc_cs_central_settings.office_version >= 12 Then
            '    officeFormat = 1
            'End If

            worddocument.saveas(String.Concat(bc_cs_central_settings.local_template_path, templateName), officeFormat)

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "save_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "save_template", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides ReadOnly Property document_count() As Integer
        Get
            If Not word Is Nothing Then
                document_count = word.documents.count()
            End If
        End Get
    End Property

    Public Overrides Function open_template(ByVal templateName As String, ByVal visible As Boolean) As Boolean

        Dim log As New bc_cs_activity_log("bc_ao_word", "open_template", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim i As Integer = 1

        Try
            ' check file is available read / write and also autotext and table gallery are available
            If FileIsLocked(templateName) Then
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "The template file is locked, please close the file and try again.", bc_cs_message.MESSAGE)
                Return False
            End If
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                If FileIsLocked(String.Concat(bc_cs_central_settings.local_template_path, "Autotext.dotm")) Then
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Autotext file is locked, please close the file and try again.", bc_cs_message.MESSAGE)
                    Return False
                End If

                If FileIsLocked(String.Concat(bc_cs_central_settings.local_template_path, "BC Table Gallery.dotx")) Then
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "BC Table Gallery file is locked, please close the file and try again.", bc_cs_message.MESSAGE)
                    Return False
                End If
            Else
                If FileIsLocked(String.Concat(bc_cs_central_settings.local_template_path, "Autotext.dot")) Then
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Autotext file is locked, please close the file and try again.", bc_cs_message.MESSAGE)
                    Return False
                End If

                If FileIsLocked(String.Concat(bc_cs_central_settings.local_template_path, "BC Table Gallery.dot")) Then
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "BC Table Gallery file is locked, please close the file and try again.", bc_cs_message.MESSAGE)
                    Return False
                End If
            End If

            invoke_word()
            worddocument = word.documents.open(templateName)

            ' OFFICE 2010 asynchronous startup issues
            While worddocument Is Nothing And i < 16 ' max time allowed (15 seconds) to get a word document
                log = New bc_cs_activity_log("bc_ao_word", "open_template", bc_cs_activity_codes.COMMENTARY, "Failed to open template, retrying for 15 seconds: " + bc_cs_central_settings.local_template_path + templateName)
                worddocument = word.documents.open(templateName)
                If worddocument Is Nothing Then
                    Thread.Sleep(1000)
                    i = i + 1
                Else
                    Exit While
                End If
                Application.DoEvents()
            End While
            If i = 16 Then
                Dim omessage As New bc_cs_message("Blue Curve create", "Could not open template", bc_cs_message.MESSAGE)
                word.visible = True
                Exit Try
            End If

            If visible Then
                word.visible = visible
            End If

            Return True

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "open_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "open_template", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Public Overrides Sub copy_template(ByVal source As String, ByVal destination As String)

        Dim log As New bc_cs_activity_log("bc_ao_word", "copy_template", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            FileCopy(source, destination)

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "copy_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "copy_template", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Function checkAutoTextExists() As Boolean

        Dim log = New bc_cs_activity_log("bc_ao_word", "checkAutoTextExists", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            ' check to see if the autotext file exists and if not create it
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                If File.Exists(String.Concat(bc_cs_central_settings.local_template_path, "autotext.dotm")) Then
                    Return True
                Else
                    Return False
                End If
            Else
                If File.Exists(String.Concat(bc_cs_central_settings.local_template_path, "autotext.dot")) Then
                    Return True
                Else
                    Return False
                End If
            End If


        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "checkAutoTextExists", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "checkAutoTextExists", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Public Sub get_styles_from_auto_text(ByRef cbh As ComboBoxHelper)

        Dim log = New bc_cs_activity_log("bc_ao_word", "get_styles_from_auto_text", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim sourceDocStyle As Object
            Dim i As Integer

            invoke_word()

            If bc_cs_central_settings.userOfficeStatus = 2 Then
                worddocument = word.documents.open(String.Concat(bc_cs_central_settings.local_template_path, "autotext.dotm"), visible:=False, ReadOnly:=False)
            Else
                worddocument = word.documents.open(String.Concat(bc_cs_central_settings.local_template_path, "autotext.dot"), visible:=False, ReadOnly:=False)
            End If


            For Each sourceDocStyle In worddocument.Styles
                If Not sourceDocStyle.BuiltIn Or sourceDocStyle.InUse Then
                    i = i + 1
                    cbh.Add(i, sourceDocStyle.NameLocal)
                End If
            Next sourceDocStyle

            worddocument.close()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "get_styles_from_auto_text", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "get_styles_from_auto_text", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Public Overrides Sub show_all_markup()

        Dim log As New bc_cs_activity_log("bc_ao_word", "show_all_markup", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim autoTextName As String
            Dim bookMarkName As String
            Dim ocommentary As bc_cs_activity_log
            Dim templateID As Integer

            Dim i As Integer
            Dim j As Integer
            Dim k As Integer

            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Showing Markup...", 10, False, True)
            Cursor.Current = Cursors.WaitCursor

            invisible()

            templateID = worddocument.CustomDocumentProperties("rnet_template_id").value

            If bc_am_load_objects.obc_templates.template.Count > 1 Then
                Dim errLog As New bc_cs_error_log("bc_am_template", "show_all_markup", bc_cs_error_codes.USER_DEFINED, "System Error. Please close down all Word instances and try again.")
                Exit Try
            End If

            If bc_am_load_objects.obc_templates.template.Count = 1 Then
                bc_am_load_objects.obc_templates.template.RemoveAt(0)
            End If

            bc_am_load_objects.obc_templates.add(worddocument.name, worddocument.name, templateID)

            bc_cs_central_settings.progress_bar.increment("Showing Markup...")

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                REM rem read in via SOAP                
                ocommentary = New bc_cs_activity_log("bc_ao_word", "show_all_markup", bc_cs_activity_codes.COMMENTARY, "Loading Template via SOAP")
                bc_am_load_objects.obc_templates.template(0).tmode = bc_am_load_objects.obc_templates.template(0).tREAD
                bc_am_load_objects.obc_templates.template(0).transmit_to_server_and_receive(bc_am_load_objects.obc_templates.template(0), True)
            Else
                REM read in directly from database                
                REM templates
                ocommentary = New bc_cs_activity_log("bc_ao_word", "show_all_markup", bc_cs_activity_codes.COMMENTARY, "Loading Template from Database")
                bc_am_load_objects.obc_templates.template(0).db_read(templateID, bc_am_load_objects.obc_templates.template(0).certificate)
            End If

            bc_cs_central_settings.progress_bar.increment("Showing Markup...")

            For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                If bc_am_load_objects.obc_templates.template(i).filename = worddocument.name Then

                    bc_cs_central_settings.progress_bar.increment("Showing Markup...")

                    For j = 0 To bc_am_load_objects.obc_templates.template(i).components.component.count - 1
                        autoTextName = bc_am_load_objects.obc_templates.template(i).components.component(j).autotext_name
                        bookMarkName = bc_am_load_objects.obc_templates.template(i).components.component(j).bookmark_name
                        If insert_autotext(autoTextName, bookMarkName) = 0 Then
                            For k = 0 To bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component.count - 1

                                ' select the table
                                worddocument.Bookmarks(bookMarkName).range.tables(1).Select()
                                Dim x As Integer
                                Dim y As Integer
                                Dim desc As String
                                Dim value As String

                                x = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).row
                                y = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).col
                                desc = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).name
                                value = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).value

                                worddocument.Application.Selection.Tables(1).Cell(x, y).Select()
                                If desc <> "Text" And desc <> "Bookmark" Then
                                    worddocument.Application.Selection.Tables(1).Cell(x, y).Range.Text = desc
                                Else
                                    If desc = "Text" Or desc = "Bookmark" Then
                                        worddocument.Application.Selection.Tables(1).Cell(x, y).Range.Text = value
                                    End If
                                End If
                                x = worddocument.Application.Selection.Information(13) 'wdStartOfRangeRowNumber
                                y = worddocument.Application.Selection.Information(16) 'wdStartOfRangeColumnNumber
                                Try
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "show_all_markup", bc_cs_activity_codes.COMMENTARY, bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).style)
                                    worddocument.Application.Selection.Tables(1).Cell(x, y).Range.style = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).style
                                Catch
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "show_all_markup", bc_cs_activity_codes.COMMENTARY, _
                                        String.Concat(bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).style, " ", "does not exist"))
                                End Try
                            Next
                        End If
                    Next
                    Exit For
                End If
            Next

            bc_cs_central_settings.progress_bar.increment("Showing Markup...")

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "show_all_markup", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            visible()
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_ao_word", "show_all_markup", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub package_for_use()

        Dim log As New bc_cs_activity_log("bc_ao_word", "package_for_use", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim bms() As String
            Dim i As Integer
            Dim j As Integer
            Dim templateCounter As Integer
            Dim bookmarkFound As Boolean
            Dim bookmarks() As String = Nothing

            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Packaging Template...", 10, False, True)
            Cursor.Current = Cursors.WaitCursor

            invisible()

            ReDim bms(worddocument.bookmarks.Count - 1)
            REM get all bookmarks
            For i = 0 To worddocument.bookmarks.Count - 1
                bms(i) = worddocument.bookmarks(i + 1).name
            Next

            bc_cs_central_settings.progress_bar.increment("Packaging Template...")

            Dim halfWay As Integer
            halfWay = Math.Round(UBound(bms, 1) / 2, 0)

            ' hide all sub components
            For i = 0 To UBound(bms, 1)

                bookmarkFound = False

                If Right(bms(i), 5) = "_r1c1" Then
                    worddocument.bookmarks(bms(i)).select()
                    clear_table_text()
                    worddocument.Application.Selection.Tables(1).Cell(1, 1).Select()
                    worddocument.Application.Selection.bookmarks.Add(bms(i))

                End If

                If worddocument.bookmarks.Exists(bms(i)) Then
                    worddocument.bookmarks(bms(i)).Select()

                    Dim newBookmark As String
                    Dim autoTextName As String = ""
                    Dim l As Integer

                    If Right(bms(i), 5) <> "_r1c1" Then
                        ReDim bookmarks(worddocument.Application.Selection.bookmarks.Count)
                        For l = 0 To worddocument.Application.Selection.bookmarks.Count - 1
                            bookmarks(l) = worddocument.Application.Selection.bookmarks(l + 1).name
                        Next
                    End If

                    If i = halfWay Or i = Math.Round(halfWay / 2) Or i = Math.Round((halfWay / 2) + halfWay) Then
                        bc_cs_central_settings.progress_bar.increment("Packaging Template...")
                    End If

                    If Right(bms(i), 5) = "_r1c1" Then
                        newBookmark = Left(bms(i), Len(bms(i)) - 5)
                        For l = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                            If bc_am_load_objects.obc_templates.template(l).filename = worddocument.name Then
                                For j = 0 To bc_am_load_objects.obc_templates.template(l).components.component.count - 1
                                    If newBookmark = bc_am_load_objects.obc_templates.template(l).components.component(j).bookmark_name Then
                                        autoTextName = bc_am_load_objects.obc_templates.template(l).components.component(j).autotext_name
                                        bookmarkFound = True
                                        Exit For
                                    End If
                                Next
                                Exit For
                            End If
                        Next

                        worddocument.Application.Selection.Tables(1).Select()


                        If bookmarkFound Then

                            bookmarkFound = False

                            With worddocument.Application
                                For templateCounter = 1 To .Templates.Count
                                    If LCase(.Templates(templateCounter).name) = "autotext.dot" Or LCase(.Templates(templateCounter).name) = "autotext.dotm" Then
                                        Dim rng As Object
                                        rng = .Selection.Range
                                        .templates(templateCounter).AutoTextEntries.Add(Name:=autoTextName, Range:=rng)
                                        Exit For
                                    End If
                                Next
                            End With

                            ' update table gallery if already exists
                            copy_table()
                            If insert_table_into_gallery(newBookmark, True) Then
                                set_table_cell("", True) ' blank out the text in the table gallery
                            End If

                            ' reselect table in original doc in case table gallery was accessed.
                            worddocument.bookmarks(bms(i)).Select()

                            worddocument.Application.Selection.Tables(1).Select()

                            worddocument.Application.Selection.Tables(1).Delete()

                            REM reset all bookmarks 
                            With worddocument.Application
                                For l = 0 To UBound(bookmarks) - 1
                                    Dim rng As Object
                                    rng = .Selection.Range
                                    If Len(bookmarks(l)) > 5 Then
                                        If Right(bookmarks(l), 5) <> "_r1c1" Then
                                            .Selection.bookmarks.Add(bookmarks(l), rng)
                                        Else
                                            .Selection.bookmarks.Add(Left(bookmarks(l), Len(bookmarks(l)) - 5), rng)
                                        End If
                                    Else
                                        .Selection.bookmarks.Add(bookmarks(l), rng)
                                    End If
                                Next
                            End With
                        End If
                    End If

                End If
            Next

            bc_cs_central_settings.progress_bar.increment("Packaging Template...")

            ' save the template and autotext
            For templateCounter = 1 To worddocument.Application.Templates.Count
                If LCase(worddocument.Application.Templates(templateCounter).name) = "autotext.dot" Or LCase(worddocument.Application.Templates(templateCounter).name) = "autotext.dotm" Then
                    worddocument.Application.templates(templateCounter).Save()
                    Exit For
                End If
            Next
            save()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "package_for_use", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            close_table_gallery(True)
            visible()
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_ao_word", "package_for_use", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub get_all_bookmarks(ByRef lv As ListView)

        Dim log As New bc_cs_activity_log("bc_ao_word", "get_all_bookmarks", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim bookmarks() As String = Nothing

            Dim obmk As Object
            Dim obmksub As Object

            lv.Items.Clear()

            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Loading all bookmarks...", 10, False, True)
            Cursor.Current = Cursors.WaitCursor

            worddocument.Application.ScreenUpdating = False

            bc_cs_central_settings.progress_bar.increment("Loading all bookmarks...")

            For Each obmk In worddocument.bookmarks

                If Right(obmk.name, 5) = "_r1c1" Then

                    bc_cs_central_settings.progress_bar.increment("Loading all bookmarks...")

                    For Each obmksub In worddocument.bookmarks
                        'check if the <bookmark> and <bookmark>_r1c1 exists
                        If obmk.name.Substring(0, (obmk.name.Length - 5)) = obmksub.name Then

                            Dim rng As Object
                            rng = obmk.Range
                            Dim x As Integer
                            Dim y As Integer
                            Dim bkmk As String

                            If rng.Tables.Count > 0 Then

                                bc_cs_central_settings.progress_bar.increment("Loading all bookmarks...")

                                For Each rCell In rng.Tables(1).Range.Cells

                                    bkmk = obmksub.name
                                    x = rCell.Range.Information(13)
                                    y = rCell.Range.Information(16)

                                    'check if there is a value in the cell and the row doeesn't contain delete row
                                    If Len(rCell.Range.Text) > 2 And Left(rCell.Range.Text, 10) <> "Delete Row" Then

                                        Dim lvi = New ListViewItem()
                                        lvi.Text = CStr(bkmk)
                                        lvi.SubItems.Add(rCell.Range.Text.Substring(0, (rCell.Range.Text.Length - 2)))
                                        lvi.SubItems.Add(rCell.Range.Information(13))
                                        lvi.SubItems.Add(rCell.Range.Information(16))
                                        lvi.SubItems.Add(rCell.Range.Style.NameLocal.ToString) 'style
                                        lv.Items.Add(lvi)

                                    End If

                                Next rCell

                            End If

                        End If

                    Next

                End If

            Next

            bc_cs_central_settings.progress_bar.increment("Loading all bookmarks...")

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "get_all_bookmarks", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            worddocument.Application.ScreenUpdating = True
            close_table_gallery(True)
            visible()
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_ao_word", "get_all_bookmarks", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub clear_table_text()

        Dim log As New bc_cs_activity_log("bc_ao_word", "clear_table_text", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim j As Integer
            Dim k As Integer

            worddocument.Application.ScreenUpdating = False
            For j = 1 To worddocument.Application.Selection.Tables(1).Rows.Count
                For k = 1 To worddocument.Application.Selection.Tables(1).Columns.Count
                    Try
                        If worddocument.Application.Selection.Tables(1).Cell(j, k).Range.Fields.Count = 0 And Left(worddocument.Application.Selection.Tables(1).Cell(j, k).Range.Text, 10) <> "Delete Row" Then
                            worddocument.Application.Selection.Tables(1).Cell(j, k).Range.Text = ""
                        End If
                    Catch ex As Exception
                        Dim ocommentary = New bc_cs_activity_log("bc_ao_word", "clear_table_text", bc_cs_activity_codes.COMMENTARY, ex.Message)
                        ' ignore error as some columns are merged
                    End Try
                Next
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "clear_table_text", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            worddocument.Application.ScreenUpdating = True
            log = New bc_cs_activity_log("bc_ao_word", "clear_table_text", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides Function get_template_id() As String
        Dim log As New bc_cs_activity_log("bc_ao_word", "get_template_id", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            get_template_id = worddocument.customdocumentproperties("rnet_template_id").value

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "get_template_id", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_template_id = ""
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "get_template_id", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Public Overrides Sub clear_table_cell(ByVal bookmarkname As String)

        Dim log As New bc_cs_activity_log("bc_ao_word", "clear_table_cell", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim x As Integer
            Dim y As Integer
            Dim i As Integer
            Dim bookmarks() As String

            x = worddocument.Application.Selection.Information(13) 'wdStartOfRangeRowNumber
            y = worddocument.Application.Selection.Information(16) 'wdStartOfRangeColumnNumber

            'store bookmarks in cell so as they can be put back
            ReDim bookmarks(worddocument.Application.Selection.Tables(1).Cell(x, y).range.bookmarks.Count - 1)
            For i = 1 To worddocument.Application.Selection.Tables(1).Cell(x, y).range.bookmarks.Count
                bookmarks(i - 1) = worddocument.Application.Selection.Tables(1).Cell(x, y).range.bookmarks(i).name
            Next

            worddocument.Application.Selection.Tables(1).Cell(x, y).Range.Text = ""

            For i = 0 To UBound(bookmarks)
                If Right(bookmarks(i), 5) <> "_r1c1" And InStr(bookmarks(i), bookmarkname) = 0 Then
                    worddocument.Application.Selection.bookmarks.Add(bookmarks(i))
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "clear_table_cell", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "clear_table_cell", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Function get_selection_style() As String

        Dim log As New bc_cs_activity_log("bc_ao_word", "get_selection_style", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim x As Integer
            Dim y As Integer

            x = worddocument.Application.Selection.Information(13) 'wdStartOfRangeRowNumber
            y = worddocument.Application.Selection.Information(16) 'wdStartOfRangeColumnNumber
            get_selection_style = worddocument.Application.Selection.Tables(1).Cell(x, y).Range.Style.NameLocal

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "get_selection_style", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_selection_style = ""
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "get_selection_style", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Public Overrides Function check_autotext_entry_exists(ByVal autoTextEntry As String) As Boolean

        Dim log As New bc_cs_activity_log("bc_ao_word", "check_autotext_entry_exists", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer
            Dim j As Integer
            Dim k As Integer

            For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                If bc_am_load_objects.obc_templates.template(i).filename = worddocument.name Then
                    For j = 0 To bc_am_load_objects.obc_templates.template(i).components.component.count - 1
                        If LCase(autoTextEntry) = LCase(bc_am_load_objects.obc_templates.template(i).components.component(j).autotext_name) Then
                            Return True
                        End If
                    Next
                    With worddocument.Application
                        For j = 1 To .Templates.Count
                            If LCase(.Templates(j).name) = "autotext.dot" Or LCase(.Templates(j).name) = "autotext.dotm" Then
                                For k = 1 To .templates(j).AutoTextEntries.Count
                                    If LCase(.templates(j).AutoTextEntries(k).name) = LCase(autoTextEntry) Then
                                        Return True
                                    End If
                                Next
                                Exit For
                            End If
                        Next
                    End With
                    Exit For
                End If
            Next

            Return False

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "check_autotext_entry_exists", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "check_autotext_entry_exists", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Public Overrides Sub register_table(ByVal bookmarkName As String, ByVal autoTextEntry As String, ByVal logicalComponentsExist As Boolean)

        Dim log As New bc_cs_activity_log("bc_ao_word", "register_table", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim bookmarks() As String
            Dim i As Integer
            Dim r1c1Bookmark As String = ""

            worddocument.Application.Selection.Tables(1).Cell(1, 1).Select()
            ReDim bookmarks(worddocument.Application.Selection.bookmarks.Count - 1)
            For i = worddocument.Application.Selection.bookmarks.Count To 1 Step -1
                bookmarks(i - 1) = worddocument.Application.Selection.bookmarks(i).name
                If Right(bookmarks(i - 1), 5) = "_r1c1" Then
                    r1c1Bookmark = Left(bookmarks(i - 1), Len(bookmarks(i - 1)) - 5)
                End If
                worddocument.Application.Selection.bookmarks(i).delete()
            Next

            clear_table_text()

            With worddocument.Application
                .Selection.Tables(1).Cell(1, 1).Select()
                Dim rng As Object
                rng = .Selection.Range
                .Selection.bookmarks.Add(bookmarkName + "_r1c1", rng)

                .Selection.Tables(1).Select()
                With worddocument.Application
                    For i = 1 To .Templates.Count
                        If LCase(.Templates(i).name) = "autotext.dot" Or LCase(.Templates(i).name) = "autotext.dotm" Then
                            rng = .Selection.Range
                            .templates(i).AutoTextEntries.Add(Name:=autoTextEntry, Range:=rng)
                            .templates(i).Save()
                            Exit For
                        End If
                    Next
                End With

                .Selection.Tables(1).Select()
                rng = .Selection.Range

                .Selection.bookmarks.Add(bookmarkName, rng)
            End With

            ' reset bookmark if template has more than one logical definition
            For i = 0 To UBound(bookmarks)
                If Len(bookmarks(i)) > 5 Then
                    If Right(bookmarks(i), 5) <> "_r1c1" And _
                            (r1c1Bookmark <> bookmarks(i) Or logicalComponentsExist) Then

                        worddocument.Application.Selection.bookmarks.Add(bookmarks(i))
                    End If
                ElseIf (r1c1Bookmark <> bookmarks(i) Or logicalComponentsExist) Then
                    worddocument.Application.Selection.bookmarks.Add(bookmarks(i))
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "register_table", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "register_table", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub delete_table(ByVal logicalComponentsExist As Boolean)

        Dim log As New bc_cs_activity_log("bc_ao_word", "delete_table", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim bookmarks() As String
            Dim i As Integer
            Dim r1c1Bookmark As String = ""

            worddocument.Application.Selection.Tables(1).Cell(1, 1).Select()
            ReDim bookmarks(worddocument.Application.Selection.bookmarks.Count - 1)
            For i = 1 To worddocument.Application.Selection.bookmarks.Count
                bookmarks(i - 1) = worddocument.Application.Selection.bookmarks(i).name
                If Right(bookmarks(i - 1), 5) = "_r1c1" Then
                    r1c1Bookmark = Left(bookmarks(i - 1), Len(bookmarks(i - 1)) - 5)
                End If
            Next

            worddocument.Application.Selection.Tables(1).Delete()

            ' reset bookmark if template has more than one logical definition
            For i = 0 To UBound(bookmarks)
                If Len(bookmarks(i)) > 5 Then
                    If Right(bookmarks(i), 5) <> "_r1c1" And _
                            (r1c1Bookmark <> bookmarks(i) Or logicalComponentsExist) Then

                        worddocument.Application.Selection.bookmarks.Add(bookmarks(i))
                    End If
                ElseIf (r1c1Bookmark <> bookmarks(i) Or logicalComponentsExist) Then
                    worddocument.Application.Selection.bookmarks.Add(bookmarks(i))
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "delete_table", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "delete_table", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides Sub insert_registered_table(ByVal autoTextName As String, ByVal bookMarkName As String)

        Dim log As New bc_cs_activity_log("bc_ao_word", "insert_registered_table", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim rng As Object

            ' add the book mark so the autotext can be inserted
            rng = worddocument.Application.Selection.Range

            worddocument.Application.Selection.bookmarks.Add(bookMarkName, rng)

            insert_autotext(autoTextName, bookMarkName)

            ' add it again to encompass the table
            rng = worddocument.Application.Selection.Tables(1).Range

            worddocument.Application.Selection.bookmarks.Add(bookMarkName, rng)


        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "insert_registered_table", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "insert_registered_table", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub copy_table()

        Dim log As New bc_cs_activity_log("bc_ao_word", "copy_table", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            worddocument.Application.Selection.Tables(1).Range.Copy()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "copy_table", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "copy_table", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub set_table_cell(ByVal cellValue As String, ByVal standardDisplay As Boolean)

        Dim log As New bc_cs_activity_log("bc_ao_word", "set_table_cell", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim x As Integer
            Dim y As Integer

            If standardDisplay Then
                worddocument.Application.Selection.Tables(1).Cell(1, 1).Range.Text = cellValue
            Else
                x = worddocument.Application.Selection.Information(13) 'wdStartOfRangeRowNumber
                y = worddocument.Application.Selection.Information(16) 'wdStartOfRangeColumnNumber
                worddocument.Application.Selection.Tables(1).Cell(x, y).Range.Text = cellValue
            End If

        Catch ex As Exception
            ' ignore as not always possible to set text
            log = New bc_cs_activity_log("bc_ao_word", "set_table_cell", bc_cs_activity_codes.COMMENTARY, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "set_table_cell", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub set_table_cells(ByVal bookmarkName As String)

        Dim log As New bc_cs_activity_log("bc_ao_word", "set_table_cells", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer
            Dim j As Integer
            Dim k As Integer

            For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                If bc_am_load_objects.obc_templates.template(i).filename = worddocument.name Then
                    Dim ocommentary = New bc_cs_activity_log("bc_ao_word", "set_table_cells", bc_cs_activity_codes.COMMENTARY, worddocument.name)
                    For j = 0 To bc_am_load_objects.obc_templates.template(i).components.component.count - 1
                        ocommentary = New bc_cs_activity_log("bc_ao_word", "set_table_cells", bc_cs_activity_codes.COMMENTARY, bookmarkName)
                        ocommentary = New bc_cs_activity_log("bc_ao_word", "set_table_cells1", bc_cs_activity_codes.COMMENTARY, bc_am_load_objects.obc_templates.template(i).components.component(j).bookmark_name)
                        If bc_am_load_objects.obc_templates.template(i).components.component(j).bookmark_name = bookmarkName Then
                            ocommentary = New bc_cs_activity_log("bc_ao_word", "set_table_cells", bc_cs_activity_codes.COMMENTARY, bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component.count)
                            For k = 0 To bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component.count - 1
                                If bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).name <> "Text" And _
                                    bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).name <> "Bookmark" Then
                                    worddocument.Application.Selection.Tables(1).Cell(bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).row, _
                                                    bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).col).Range.Text = _
                                                        bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).name
                                Else
                                    If bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).name = "Text" Or _
                                        bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).name = "Bookmark" Then
                                        worddocument.Application.Selection.Tables(1).Cell(bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).row, _
                                                    bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).col).Range.Text = _
                                                        bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).value

                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "set_table_cells", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "set_table_cells", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub refresh_bookmark_markup(ByVal bookmarkName As String)
        Dim log As New bc_cs_activity_log("bc_ao_word", "refresh_bookmark_markup", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Dim i As Integer
            Dim j As Integer
            Dim k As Integer

            For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                If bc_am_load_objects.obc_templates.template(i).filename = worddocument.name Then

                    bc_cs_central_settings.progress_bar.increment("Refreshing Markup...")

                    For j = 0 To bc_am_load_objects.obc_templates.template(i).components.component.count - 1
                        If bookmarkName = bc_am_load_objects.obc_templates.template(i).components.component(j).bookmark_name Then
                            For k = 0 To bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component.count - 1

                                ' select the table
                                worddocument.Bookmarks(bookmarkName).range.tables(1).Select()
                                Dim x As Integer
                                Dim y As Integer
                                Dim desc As String
                                Dim value As String

                                x = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).row
                                y = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).col
                                desc = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).name
                                value = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).value

                                worddocument.Application.Selection.Tables(1).Cell(x, y).Select()
                                If desc <> "Text" And desc <> "Bookmark" Then
                                    worddocument.Application.Selection.Tables(1).Cell(x, y).Range.Text = desc
                                Else
                                    If desc = "Text" Or desc = "Bookmark" Then
                                        worddocument.Application.Selection.Tables(1).Cell(x, y).Range.Text = value
                                    End If
                                End If
                                x = worddocument.Application.Selection.Information(13) 'wdStartOfRangeRowNumber
                                y = worddocument.Application.Selection.Information(16) 'wdStartOfRangeColumnNumber
                                Dim ocommentary As bc_cs_activity_log
                                Try
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "refresh_bookmark_markup", bc_cs_activity_codes.COMMENTARY, bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).style)
                                    worddocument.Application.Selection.Tables(1).Cell(x, y).Range.style = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).style
                                Catch
                                    ocommentary = New bc_cs_activity_log("bc_ao_word", "refresh_bookmark_markup", bc_cs_activity_codes.COMMENTARY, _
                                        String.Concat(bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).style, " ", "does not exist"))
                                End Try
                            Next
                        End If
                    Next
                    Exit For
                End If
            Next


        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "refresh_bookmark_markup", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "refresh_bookmark_markup", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Function scan_document(ByVal listType As String, ByVal namespaceValue As String, _
                                            ByVal node As String) As ArrayList
        Dim log As New bc_cs_activity_log("bc_ao_word", "scan_document", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim returnList = New ArrayList
        Dim entry(2, 0) As String

        Try
            ' build the scanList based on list type
            Try
                Dim sql As New StringBuilder

                If IsNothing(namespaceValue) Or namespaceValue = "" Then
                    Dim errLog As New bc_cs_activity_log("bc_ao_word", "scan_document", bc_cs_activity_codes.COMMENTARY, "Document Scanning Failed no namespace set")
                    Return Nothing
                End If
                If IsNothing(node) Or node = "" Then
                    Dim errLog As New bc_cs_activity_log("bc_ao_word", "scan_document", bc_cs_activity_codes.COMMENTARY, "Document Scanning Failed no node set")
                    Return Nothing
                End If

                With sql
                    .Append("exec bc_core_cr_get_metadata ")
                    .Append("'")
                    .Append(listType) ' type of list    
                    .Append("'")
                End With

                Dim osql As New bc_om_sql(sql.ToString)

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    If osql.transmit_to_server_and_receive(osql, True) = False Then
                        Return Nothing
                    End If
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success = True Then


                    document_scan_list = New ArrayList

                    For i = 0 To UBound(osql.results, 2)
                        ' build the document scan list used in the match string predicate
                        ReDim entry(2, 0)
                        entry(0, 0) = osql.results(0, i)
                        entry(1, 0) = osql.results(1, i)
                        entry(2, 0) = osql.results(2, i)
                        document_scan_list.Add(entry)
                    Next i
                Else
                    Dim errLog As New bc_cs_activity_log("bc_ao_word", "scan_document", bc_cs_activity_codes.COMMENTARY, "Failed to retrieve meta-data for document scanning.")
                    Return Nothing
                    Exit Try
                End If
            Catch ex As Exception
                Dim errLog As New bc_cs_activity_log("bc_ao_word", "scan_document", bc_cs_activity_codes.COMMENTARY, "Document Scanning Failed: " + ex.Message)
                Return Nothing
            End Try

            Dim xmlDocument As XDocument = Nothing
            Dim xmlNS As XNamespace

            ' set the namespace
            xmlNS = namespaceValue

            ' store current embedded smart tags setting and the turn off
            Dim smartTagsOption As Boolean
            smartTagsOption = worddocument.EmbedSmartTags
            worddocument.EmbedSmartTags = False

            ' load the xml based on office version --- always using wordml 2003 due to proofErr nodes (spell checking)
            If bc_cs_central_settings.userOfficeStatus = 3 Then
                xmlDocument = XDocument.Parse(worddocument.Range.XML)
            ElseIf bc_cs_central_settings.userOfficeStatus = 1 Then
                xmlDocument = XDocument.Parse(worddocument.Range.XML)
            ElseIf bc_cs_central_settings.userOfficeStatus = 2 Then
                xmlDocument = XDocument.Parse(worddocument.Range.XML)
            End If

            'restore embedded smart tags setting
            worddocument.EmbedSmartTags = smartTagsOption

            ' find all descendants based on the passed in node path
            Dim xmlNodes = xmlDocument.Descendants(xmlNS + node).Where(matchStringPredicate).OrderBy(getKeyPredicate)

            ' clear down the array
            Erase entry

            ' build the return list based on matches found in the xml based on the scan list
            For Each xl In xmlNodes
                For Me.document_scan_list_index = document_scan_list.Count - 1 To 0 Step -1
                    If Regex.IsMatch(LCase(xl.Value.ToString), String.Concat("\b", LCase(document_scan_list(document_scan_list_index)(0, 0).ToString), "\b")) Then
                        ReDim entry(2, 0)
                        entry(0, 0) = document_scan_list(document_scan_list_index)(0, 0)
                        entry(1, 0) = document_scan_list(document_scan_list_index)(1, 0)
                        entry(2, 0) = document_scan_list(document_scan_list_index)(2, 0)
                        returnList.Add(entry)
                        document_scan_list.RemoveAt(document_scan_list_index)
                    End If
                Next document_scan_list_index
            Next

            Return returnList

        Catch ex As Exception
            Dim errLog As New bc_cs_activity_log("bc_ao_word", "scan_document", bc_cs_activity_codes.COMMENTARY, "Document Scanning Failed: " + ex.Message)
            Return Nothing
        Finally
            log = New bc_cs_activity_log("bc_ao_word", "scan_document", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Dim sdoc As String = ""
    Public Shared search_attributes As bc_om_search_attribute_for_entities

    'Public Overrides Sub scan_document_for_entities(ByRef taxonomies As List(Of bc_as_cat_taxonomy), search_attributes As bc_om_search_attribute_for_entities, extended_terms As Boolean)
    Public Overrides Sub scan_document_for_entities(ByRef taxonomies As List(Of bc_as_cat_taxonomy), extended_terms As Boolean)
        Dim log As New bc_cs_activity_log("bc_ao_word", "scan_document_for_entities(", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim returnList = New ArrayList

        ' build the scanList based on list type
        Try
            If sdoc = "" Then

                Dim xmlDocument As XDocument = Nothing

                ' store current embedded smart tags setting and the turn off
                Dim smartTagsOption As Boolean
                smartTagsOption = worddocument.EmbedSmartTags
                worddocument.EmbedSmartTags = False
                ' load the xml based on office version --- always using wordml 2003 due to proofErr nodes (spell checking)
                xmlDocument = XDocument.Parse(worddocument.Range.XML)
                sdoc = LCase(xmlDocument.ToString)
                sdoc = sdoc.Replace(":square", "")
                sdoc = sdoc.Replace("wrap type=""square""", "")
            End If
            'Dim kk As New StreamWriter("c:\d\m.txt", False)

            'kk.WriteLine(sdoc)
            'kk.Close()
            Dim na As String

            For i = 0 To taxonomies.Count - 1
                For j = 0 To taxonomies(i).items.Count - 1
                    taxonomies(i).items(j).scanned = False
                    na = LCase(taxonomies(i).items(j).display_name)
                    If Len(na) > 1 AndAlso Right(na, 1) = "." Then
                        na = Left(na, Len(na) - 1)
                    End If

                    If Regex.IsMatch(sdoc, String.Concat("\b", na, "\b")) Then
                        taxonomies(i).items(j).scanned = True
                    End If
                Next
            Next

            If extended_terms = True Then
                'REAL time now TBD
                If IsNothing(search_attributes) Then
                    search_attributes = New bc_om_search_attribute_for_entities
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        search_attributes.db_read()
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        search_attributes.tmode = bc_cs_soap_base_class.tREAD
                        If search_attributes.transmit_to_server_and_receive(search_attributes, True) = False Then
                            Exit Sub
                        End If

                    End If
                End If
                For k = 0 To search_attributes.search_values.Count - 1
                    If Regex.IsMatch(sdoc, String.Concat("\b", LCase(search_attributes.search_values(k).value), "\b")) Then
                        For i = 0 To taxonomies.Count - 1
                            If taxonomies(i).Class_id = search_attributes.search_values(k).class_id Then
                                For j = 0 To taxonomies(i).items.Count - 1
                                    If taxonomies(i).items(j).id = search_attributes.search_values(k).entity_Id Then
                                        taxonomies(i).items(j).scanned = True
                                        Exit For
                                    End If
                                Next
                            End If
                        Next

                    End If
                Next
            End If



        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_word", "scan_document_for_entities", bc_cs_error_codes.USER_DEFINED, "Document Scanning Failed: " + ex.Message)

        Finally
            log = New bc_cs_activity_log("bc_ao_word", "scan_document_for_entities", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

End Class

REM builds a powerpoint document
Public Class bc_ao_powerpoint
    Inherits bc_ao_at_object

    Public Const TABLE = 1
    Public Const PROP = 28
    Public Const TEXT = 2
    Public Const BOOKMARK = 27

    ' powerpoint
    Private powerpoint As Object
    ' power point document instance
    Public powerpointPresentation As Object
    ' is powerpoint aready open
    Private powerpointAlreadyOpen As Boolean

    Private tablegalleryloaded As Boolean
    Private tPP As Object
    Private gallery As Object

    ' window state
    Dim wdwState As Integer = 1 ' ppNormal

    Public Sub New()

    End Sub

   
  

    Public Overrides Function save_return_error()
        Try

            save_return_error = ""
            powerpointPresentation.Save()

        Catch ex As Exception
            save_return_error = ex.Message
            Exit Function
        End Try
    End Function
    Public Overrides Function saveas_return_error(ByVal filename As String) As String
        Try

            saveas_return_error = ""
            powerpointPresentation.SaveAs(bc_cs_central_settings.local_repos_path + filename + ".pptx")

        Catch ex As Exception
            saveas_return_error = ex.Message
            Exit Function
        End Try
    End Function
    Public Sub New(ByRef opowerpointpres As Object)
        powerpointPresentation = opowerpointpres
        powerpoint = powerpointPresentation.application
    End Sub
    Public Overrides Sub quit_if_no_docs()
        If powerpoint.presentations.count = 0 Then
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
            GC.WaitForPendingFinalizers()

            powerpoint.quit()
            System.Runtime.InteropServices.Marshal.ReleaseComObject(powerpoint)
            GC.Collect()
            'While (Marshal.FinalReleaseComObject(powerpoint) > 0)
            'End While

            'powerpoint = Nothing

            'For Each pptProcess As Process In Process.GetProcessesByName("POWERPNT")
            '    pptProcess.Kill()
            'Next
        Else
            powerpoint.visible = True
        End If
    End Sub

    Public Overrides Function rendertoPDF_With_office(fn As String) As String
        Try


            Try
                powerpointPresentation.SaveAs(fn, 32)

            Catch ex As Exception
                rendertoPDF_With_office = ex.Message
                Exit Function
            End Try

            rendertoPDF_With_office = ""
        Catch ex As Exception
            rendertoPDF_With_office = ex.Message
            Dim ocommentary = New bc_cs_activity_log("bc_ao_word", "rendertoPDF_with_office", bc_cs_activity_codes.COMMENTARY, ex.Message)

        End Try

    End Function
    Public Sub New(ByRef otemplate As bc_om_template, ByRef oproperties As bc_om_doc_properties, ByVal bvisible As Boolean, ByVal bastemplate As Boolean)
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim bc_cs_central_settings As New bc_cs_central_settings

            Dim ocommentary As bc_cs_activity_log

            Dim i As Integer = 1

            bsuccess = False

            ogtemplate = otemplate
            ogproperties = oproperties
            bc_am_load_objects.obc_current_document.filename = MyBase.generate_local_filename_prefix
            REM get a handle on powerpoint or startup powerpoint
            While i < 16 ' max time allowed (15 seconds) to get a powerpoint object
                Try
                    invoke_powerpoint()
                    powerpoint.visible = True
                    Exit While
                Catch ex As Exception
                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "new", bc_cs_activity_codes.COMMENTARY, "Powerpoint object not ready retrying for 15 seconds")
                    Thread.Sleep(1000)
                    i = i + 1
                End Try
                Application.DoEvents()
            End While
            If i = 16 Then
                Dim omessage As New bc_cs_message("Blue Curve create", "Could not create a new powerpoint session", bc_cs_message.MESSAGE)
                Exit Try
            End If
            i = 1
            wdwState = powerpoint.WindowState
            powerpoint.WindowState = 2 ' ppWindowMinimized
            ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "new", bc_cs_activity_codes.COMMENTARY, "Attempting to create Document from template: " + bc_cs_central_settings.local_template_path + otemplate.filename)
            If bastemplate = True Then
                powerpointPresentation = powerpoint.presentations.open(bc_cs_central_settings.local_template_path + otemplate.filename)
            Else
                powerpointPresentation = powerpoint.presentations.add()
            End If
            ' OFFICE 2010 asynchronous startup issues
            While powerpointPresentation Is Nothing And i < 16 ' max time allowed (15 seconds) to get a powerpoint slide
                ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "new", bc_cs_activity_codes.COMMENTARY, "Failed to create Document from template retrying for 15 seconds: " + bc_cs_central_settings.local_template_path + otemplate.filename)
                If bastemplate = True Then
                    powerpointPresentation = powerpoint.presentations.open(bc_cs_central_settings.local_template_path + otemplate.filename)
                Else
                    powerpointPresentation = powerpoint.presentations.add()
                End If
                If powerpointPresentation Is Nothing Then
                    Thread.Sleep(1000)
                    i = i + 1
                Else
                    Exit While
                End If
                Application.DoEvents()
            End While
            If i = 16 Then
                Dim omessage As New bc_cs_message("Blue Curve create", "Could not create a new powerpoint document", bc_cs_message.MESSAGE)
                powerpoint.visible = True
                Exit Try
            End If
            REM set docfilename temporaily to this new document
            ocommentary = New bc_cs_activity_log("bc_ao_word", "new", bc_cs_activity_codes.COMMENTARY, "Document created from template: " + bc_cs_central_settings.local_template_path + otemplate.filename)
            bsuccess = True
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
            powerpoint.visible = True
        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Private Sub invoke_powerpoint()
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "invoke_powerpoint", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            powerpoint = GetObject(, "powerpoint.application")
            powerpointAlreadyOpen = True

            REM SW cope with office versions
            bc_cs_central_settings.user_office_version = powerpoint.Version
        Catch
            Try
                powerpoint = CreateObject("powerpoint.application")
                powerpointAlreadyOpen = False

                REM SW cope with office versions
                bc_cs_central_settings.user_office_version = powerpoint.Version

            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "invoke_powerpoint", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try

        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "invoke_powerpoint", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try

    End Sub
    Private Sub invoke_powerpoint_new()
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "invoke_powerpoint_new", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            powerpoint = CreateObject("powerpoint.application")
            powerpointAlreadyOpen = False

            REM SW cope with office versions
            bc_cs_central_settings.user_office_version = powerpoint.Version

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "invoke_powerpoint_new", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "invoke_powerpoint_new", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try

    End Sub

    Private Function InsertTableFromGallery(ByVal tableName As String, ByVal gallerytablename As String, ByVal slideIndex As Integer) As Boolean

        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "insertTableFromGallery", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim shapeExists As Boolean
            InsertTableFromGallery = False

            openGallery()
            Dim i As Integer

            For i = 1 To gallery.Slides.count
                Try
                    If gallery.Slides(i).Shapes(gallerytablename).Name = gallerytablename Then
                        shapeExists = True
                        Exit For
                    End If
                Catch ex As Exception
                    'ignore as it may just not exist on this slide
                End Try
            Next

            If shapeExists Then
                gallery.Slides(i).Shapes(gallerytablename).Copy()
                ' remove shape and replace with table
                Dim top As Double
                Dim left As Double
                Dim height As Double
                Dim width As Double

                top = powerpointPresentation.Slides(slideIndex).Shapes(tableName.ToString).Top
                left = powerpointPresentation.Slides(slideIndex).Shapes(tableName.ToString).Left
                height = powerpointPresentation.Slides(slideIndex).Shapes(tableName.ToString).Height
                width = powerpointPresentation.Slides(slideIndex).Shapes(tableName.ToString).Width
                powerpointPresentation.Slides(slideIndex).Shapes(tableName.ToString).Delete()
                Try
                    ' random error where the above line silently fails but works on second attempt
                    powerpointPresentation.Slides(slideIndex).Shapes(tableName.ToString).Delete()
                Catch
                End Try
                powerpointPresentation.Slides(slideIndex).Shapes.Paste()
                powerpointPresentation.Slides(slideIndex).Shapes(powerpointPresentation.Slides(slideIndex).Shapes.Count).Top = top
                powerpointPresentation.Slides(slideIndex).Shapes(powerpointPresentation.Slides(slideIndex).Shapes.Count).Left = left
                powerpointPresentation.Slides(slideIndex).Shapes(powerpointPresentation.Slides(slideIndex).Shapes.Count).Height = height
                powerpointPresentation.Slides(slideIndex).Shapes(powerpointPresentation.Slides(slideIndex).Shapes.Count).Width = width
                powerpointPresentation.Slides(slideIndex).Shapes(powerpointPresentation.Slides(slideIndex).Shapes.Count).Name = tableName
                InsertTableFromGallery = True
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "insertTableFromGallery", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "insertTableFromGallery", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Function copyTableToGallery(ByVal tableName As String, ByVal updateIfExistsOnly As Boolean) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "copyTableToGallery", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim shapeExists As Boolean

            copyTableToGallery = False

            openGallery()

            Dim i As Integer
            Dim j As Integer

            For i = 1 To gallery.Slides.count
                For j = 1 To gallery.Slides(i).Shapes.Count
                    Try
                        If gallery.Slides(i).Shapes(j).Name = tableName Then
                            shapeExists = True
                            Exit For
                        End If
                    Catch ex As Exception
                        'ignore as it may just not exist on this slide
                    End Try
                Next
                If shapeExists Then
                    Exit For
                End If
            Next

            If shapeExists Then
                gallery.Slides(i).Shapes(tableName).Delete()
                gallery.Slides(i).Shapes.Paste()
                copyTableToGallery = True
            ElseIf Not updateIfExistsOnly Then
                Dim newSlide As Object
                Dim customLayout As Object
                customLayout = gallery.Slides(gallery.Slides.Count).CustomLayout
                newSlide = gallery.Slides.AddSlide(gallery.Slides.Count + 1, customLayout)
                newSlide.Select()
                newSlide.Shapes.Paste()
                copyTableToGallery = True
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "copyTableToGallery", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "copyTableToGallery", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Private Sub clear_table_text(ByVal slideIndex As Integer, ByVal tableName As String)

        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "clear_table_text", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim j As Integer
            Dim k As Integer

            For j = 1 To powerpointPresentation.Slides(slideIndex).Shapes(tableName).Table.Rows.Count
                For k = 1 To powerpointPresentation.Slides(slideIndex).Shapes(tableName).Table.Columns.Count
                    Try
                        powerpointPresentation.Slides(slideIndex).Shapes(tableName).Table.Cell(j, k).Shape.TextFrame.TextRange.Text = ""
                    Catch ex As Exception
                        Dim ocommentary = New bc_cs_activity_log("bc_ao_word", "clear_table_text", bc_cs_activity_codes.COMMENTARY, ex.Message)
                        ' ignore error as some columns are merged
                    End Try
                Next
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "clear_table_text", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "clear_table_text", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Private Sub openGallery()

        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "openGallery", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim ocommentary As bc_cs_activity_log

            If tablegalleryloaded = False Then
                Try
                    tPP = GetObject(, "powerpoint.application")
                Catch
                    Try
                        tPP = CreateObject("powerpoint.application")
                    Catch ex As Exception
                        Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "openGallery", bc_cs_error_codes.USER_DEFINED, ex.Message)
                        Exit Sub
                    End Try
                End Try

                If bc_cs_central_settings.userOfficeStatus = 2 Then
                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "openGallery", bc_cs_activity_codes.COMMENTARY, "Attemting to Open: " + bc_cs_central_settings.local_template_path + "BC Table Gallery.potx")
                    gallery = tPP.presentations.open(bc_cs_central_settings.local_template_path + "BC Table Gallery.potx")
                Else
                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "openGallery", bc_cs_activity_codes.COMMENTARY, "Attemting to Open: " + bc_cs_central_settings.local_template_path + "BC Table Gallery.pot")
                    gallery = tPP.presentations.open(bc_cs_central_settings.local_template_path + "BC Table Gallery.pot")
                End If


                gallery.Application.WindowState = 2 'ppMinimized
                tablegalleryloaded = True
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "openGallery", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "openGallery", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub closeTableGallery()
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "closeTableGallery", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            If Me.tablegalleryloaded = True Then
                gallery.close()
                Me.tablegalleryloaded = False
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "closeTableGallery", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "closeTableGallery", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Private Sub getTagNameLocation(ByVal key As String, ByRef slideIndex As Integer, ByRef shapeIndex As Integer, _
                                    ByRef row As Integer, ByRef col As Integer, Optional ByRef shapeName As String = "")

        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "getTagNameLocation", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer
            Dim j As Integer
            Dim k As Integer
            Dim found As Boolean

            row = -1
            col = -1
            For i = 1 To powerpointPresentation.slides.count
                If found Then
                    Exit For
                End If
                For j = 1 To powerpointPresentation.slides(i).shapes.count
                    If found Then
                        Exit For
                    End If
                    For k = 1 To powerpointPresentation.slides(i).shapes(j).Tags.Count
                        If LCase(key) = LCase(powerpointPresentation.slides(i).shapes(j).Tags.Name(k)) Then

                            slideIndex = i
                            shapeIndex = j
                            row = CInt(Left(powerpointPresentation.slides(i).shapes(j).Tags.Value(k), _
                                            InStr(powerpointPresentation.slides(i).shapes(j).Tags.Value(k), ",") - 1))
                            col = CInt(Mid(powerpointPresentation.slides(i).shapes(j).Tags.Value(k), _
                                            InStr(powerpointPresentation.slides(i).shapes(j).Tags.Value(k), ",") + 1))
                            shapeName = powerpointPresentation.slides(i).shapes(j).Name
                            found = True
                            Exit For
                        End If
                    Next
                Next
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "getTagNameLocation", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "getTagNameLocation", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Function insert_sub_components(ByVal slideIndex As Integer, ByVal tableName As String, ByVal osub_components As bc_om_sub_components, ByRef componentCount As Integer) As Integer
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "insert_sub_components", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim i, j As Integer
        Dim cell As Object
        Dim tag_name As String
        Dim pprop_name As String
        Dim ocommentary As bc_cs_activity_log

        Try
            For i = 0 To osub_components.sub_component.Count - 1
                With osub_components.sub_component(i)
                    REM select component
                    Try
                        cell = powerpointPresentation.Slides(slideIndex).Shapes(tableName).Table.Cell(.row, .col)
                    Catch
                        ocommentary = New bc_cs_activity_log("bc_ao_word", "insert_sub_components", bc_cs_activity_codes.COMMENTARY, "Rol/ Col:" + CStr(.row) + "," + CStr(.col) + " does not exist in table.")
                        Exit Function
                    End Try
                    If .type = TEXT Then
                        cell.Shape.TextFrame.TextRange.Text = .value
                    ElseIf (.type = BOOKMARK) Then
                        powerpointPresentation.Slides(slideIndex).Shapes(tableName).Tags.Add(.value + "_en" + CStr(bc_am_load_objects.obc_current_document.entity_id), String.Concat(CStr(.row), ",", CStr(.col)))
                    Else
                        REM if entity_id is 0 replace with a random number so display can be used
                        REM in composites
                        If bc_am_load_objects.obc_current_document.entity_id = 0 Then
                            tag_name = "rnet" + CStr(componentCount) + "_at" + CStr(.type) + "_en" + CStr(Format(Now, "hhmmss"))
                        Else
                            tag_name = "rnet" + CStr(componentCount) + "_at" + CStr(.type) + "_en" + CStr(bc_am_load_objects.obc_current_document.entity_id)
                        End If
                        ocommentary = New bc_cs_activity_log("bc_ao_word", "insert_sub_components", bc_cs_activity_codes.COMMENTARY, "Setting Shape:" + tag_name + " at position(" + CStr(.row) + "," + CStr(.col) + ")")
                        powerpointPresentation.Slides(slideIndex).Shapes(tableName).Tags.Add(tag_name, String.Concat(CStr(.row), ",", CStr(.col)))

                        componentCount = componentCount + 1

                        REM set paramters as properties for portability
                        Try
                            REM contributor id
                            pprop_name = tag_name + "_contributor"
                            powerpointPresentation.CustomDocumentProperties.add(pprop_name, False, 4, "")
                            powerpointPresentation.CustomDocumentProperties(pprop_name) = osub_components.sub_component(i).contributor_id
                            REM disabled set to enabled on build
                            pprop_name = tag_name + "_disabled"
                            powerpointPresentation.CustomDocumentProperties.add(pprop_name, False, 4, "")
                            powerpointPresentation.CustomDocumentProperties(pprop_name) = "enabled"
                            If osub_components.sub_component(i).parameters.component_template_parameters.count > 0 Then
                                For j = 0 To osub_components.sub_component(i).parameters.component_template_parameters.count - 1
                                    With osub_components.sub_component(i).parameters.component_template_parameters(j)
                                        pprop_name = tag_name + "_p" + CStr(j)
                                        powerpointPresentation.CustomDocumentProperties.add(pprop_name, False, 4, "")
                                        If .system_defined = 0 Then
                                            If IsNumeric(.default_value_id) Then
                                                powerpointPresentation.CustomDocumentProperties(pprop_name) = CStr(.default_value_id)
                                            Else
                                                powerpointPresentation.CustomDocumentProperties(pprop_name) = CStr(.default_value)
                                            End If
                                        Else
                                            powerpointPresentation.CustomDocumentProperties(pprop_name) = "system_defined"
                                        End If
                                    End With
                                Next
                            End If
                        Catch

                        End Try

                        REM see if parameters exists for sub component 
                        REM if so add component to document metadata
                        MyBase.set_component_in_document_metadata(osub_components.sub_component(i), tag_name, bc_am_load_objects.obc_current_document.entity_id, Nothing)
                    End If
                End With
            Next

            Return True

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "insert_sub_components", bc_cs_error_codes.USER_DEFINED, ex.Message)
            insert_sub_components = bc_cs_error_codes.RETURN_ERROR
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "insert_sub_components", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function



    Public Overrides Function getSlideIndex() As Integer

        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "getSlideIndex", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            getSlideIndex = powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Parent.SlideIndex()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "getSlideIndex", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "getSlideIndex", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Public Overrides Sub caption(ByVal caption As String)
        powerpointPresentation.Application.caption = caption
    End Sub

    Public Sub insert_file(ByVal filename As String, Optional ByVal new_page As Boolean = True, Optional ByVal link_to_previous As Boolean = True, Optional ByVal bookmark_name As String = "")
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "insert_file", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim slideIndex As Integer
            Dim shapeIndex As Integer
            Dim row As Integer
            Dim col As Integer

            getTagNameLocation(bookmark_name, slideIndex, shapeIndex, row, col)
            'If row <> -1 Then
            '    powerpointPresentation.slides(slideIndex).shapes(shapeIndex).Delete()
            '    powerpointPresentation.slides.InsertFromFile(filename:=filename, Index:=powerpointPresentation.slides.count)
            '    Exit Try
            'End If

            If bookmark_name = "" Then
                powerpointPresentation.slides.InsertFromFile(filename:=filename, Index:=powerpointPresentation.slides.count)
            Else
                powerpointPresentation.slides.InsertFromFile(filename:=filename, Index:=slideIndex)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_word", "insert_file", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_word", "insert_file", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    REM updates a Standard Display Table
    Public Sub update_table_display(ByVal locator As String, ByVal values As Object, ByVal restriction As Integer, _
                                    ByVal restriction_text As String, ByVal tableLocator As String)
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "update_table_display", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim i As Integer
        Dim index As String
        Dim ocommentary As bc_cs_activity_log
        Dim slideIndex As Integer
        Dim shapeIndex As Integer
        Dim row As Integer
        Dim col As Integer

        Try
            REM select current table 
            getTagNameLocation(locator, slideIndex, shapeIndex, row, col)

            REM restrictions
            If restriction = 1 Then
                ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Restriction so table not displayed")
                powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).table.cell(row, col).Shape.TextFrame.TextRange.Text = restriction_text
                Exit Sub
            End If
            If restriction = 2 Then
                ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Restriction so table not displayed")
                Exit Sub
            End If
            REM add data
            Dim cell As Object

            For i = 0 To UBound(values, 2)
                Try
                    cell = powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Table.Cell(values(1, i), values(2, i))
                    If Not IsNothing(values(0, i)) Then
                        REM check if item is to be indexed
                        index = 0
                        If Len(values(0, i)) > 6 Then
                            If Left(values(0, i), 4) = "[ind" Then
                                values(0, i) = Right(values(0, i), Len(values(0, i)) - 4)
                                index = Left(values(0, i), 1)
                                values(0, i) = Right(values(0, i), Len(values(0, i)) - 2)
                                powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Tags.Add("rnet_ind" + CStr(index) + "_" + CStr(Format(Now, "hhmmss")) + CStr(i), String.Concat(values(1, i), ",", values(2, i)))
                            End If
                        End If
                        cell.Shape.TextFrame.TextRange.Text = CStr(values(0, i))
                    Else
                        cell.Shape.TextFrame.TextRange.Text = ""
                    End If
                Catch
                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "update_table_display", bc_cs_activity_codes.COMMENTARY, "Coords: " + CStr(values(1, i)) + "," + CStr(values(2, i)) + " dont exist in table at:" + locator)
                End Try
            Next
            REM delete rows marked for deletion
            Try
                For i = powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Table.rows.count To 1 Step -1
                    If Left(powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Table.Rows(i).Cells(1).Shape.TextFrame.TextRange.Text, 10) = "Delete Row" Then
                        powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Table.Rows(i).Delete()
                    End If
                Next
            Catch

            End Try

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "update_table_display", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "update_table_display", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides Sub new_template()

        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "new_template", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            If powerpoint Is Nothing Then
                invoke_powerpoint()
            End If
            powerpointPresentation = powerpoint.presentations.Add()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "new_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "new_template", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Function open_template(ByVal templateName As String, ByVal visible As Boolean) As Boolean

        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "open_template", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim i As Integer = 1

        Try

            invoke_powerpoint()
            ' has to be visible to open
            powerpoint.visible = True
            powerpointPresentation = powerpoint.presentations.open(templateName)

            ' OFFICE 2010 asynchronous startup issues
            While powerpointPresentation Is Nothing And i < 16 ' max time allowed (15 seconds) to get a word document
                log = New bc_cs_activity_log("bc_ao_powerpoint", "open_template", bc_cs_activity_codes.COMMENTARY, "Failed to open template, retrying for 15 seconds: " + bc_cs_central_settings.local_template_path + templateName)
                powerpointPresentation = powerpoint.presentations.open(templateName)
                If powerpointPresentation Is Nothing Then
                    Thread.Sleep(1000)
                    i = i + 1
                Else
                    Exit While
                End If
                Application.DoEvents()
            End While
            If i = 16 Then
                Dim omessage As New bc_cs_message("Blue Curve create", "Could not open template", bc_cs_message.MESSAGE)
                powerpoint.visible = True
                Exit Try
            End If

            Return True

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "open_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "open_template", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Public Overrides Sub add_references()

        Dim log = New bc_cs_activity_log("bc_ao_powerpoint", "add_references", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim vbComp As Object

            ' add default code  
            vbComp = powerpointPresentation.VBProject.VBComponents.Add(1) 'vbext_ct_StdModule
            vbComp.Name = "ThisPresentation"
            vbComp.CodeModule.AddFromFile(String.Concat(bc_cs_central_settings.local_template_path, "default_pp_code.txt"))

        Catch ex As System.Runtime.InteropServices.COMException
            ' references already exists
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "add_references", bc_cs_error_codes.USER_DEFINED, String.Concat(ex.Message, vbCrLf, "Please check PowerPoint macro settings / VBA"))
        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "add_references", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "add_references", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub add_properties(ByVal templateID As Integer, ByVal templateName As String)

        Dim log = New bc_cs_activity_log("bc_ao_powerpoint", "add_properties", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            ' add the required custom properties
            powerpointPresentation.CustomDocumentProperties.add("rnet_template_id", False, 1, 0)
            powerpointPresentation.CustomDocumentProperties("rnet_template_id") = templateID
            powerpointPresentation.CustomDocumentProperties.add("xrnet_attached_template", False, 4, "")
            powerpointPresentation.CustomDocumentProperties("xrnet_attached_template") = templateName

        Catch ex As System.Runtime.InteropServices.COMException
            ' properties already exists
            log = New bc_cs_activity_log("bc_ao_powerpoint", "add_properties", bc_cs_activity_codes.COMMENTARY, ex.Message)
        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "add_properties", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "add_properties", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Function set_locator_for_component(ByVal component_id As Long, ByVal entity_id As Long, ByVal component_type As Integer, Optional ByVal bwith_table As Boolean = True) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_word", "set_locator_for_component", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim tag_name As String
        Dim row, col As Integer
        tag_name = ""


        Dim componentcount As Integer = 100
        Try

            REM if entity_id is 0 replace with a random number so display can be used
            REM in composites
            If entity_id = 0 Then
                tag_name = "rnet" + CStr(componentcount) + "_at" + CStr(component_id) + "_en" + CStr(Format(Now, "hhmmss"))
            Else
                tag_name = "rnet" + CStr(componentcount) + "_at" + CStr(component_id) + "_en" + CStr(entity_id)
                REM need to check if this tahe name already exists
                Dim slideindex As Integer
                Dim shapeindex As Integer
                Dim shapename As String = ""

                While shapename <> "nothing"
                    shapename = "nothing"
                    tag_name = "rnet" + CStr(componentcount) + "_at" + CStr(component_id) + "_en" + CStr(entity_id)
                    getTagNameLocation(tag_name, slideindex, shapeindex, row, col, shapename)
                    componentcount = componentcount + 1
                End While
            End If
            REM need to add a shape at insertion point
            REM text component
            Select Case component_type

                Case 2, 3

                    REM check you are in shape range
                    Try

                        powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange.Tags.Add(tag_name, "1,1")
                    Catch
                        Dim omsg As New bc_cs_message("Blue Curve", "You need to be in a shape before you can insert a table/chart component", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        tag_name = ""
                    End Try

                Case 1

                    REM check you are in table
                    Try
                        If powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).HasTable = False Then
                            powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange.Tags.Add(tag_name, "1,1")
                        Else
                            get_selection_values(row, col)
                            powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange.Tags.Add(tag_name, CStr(row) + "," + CStr(col))
                        End If
                    Catch
                        Dim omsg As New bc_cs_message("Blue Curve", "You need to be in a shape before you can insert a cell component", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        tag_name = ""
                    End Try


            End Select
            'componentcount = componentcount + 1


            'If entity_id = 0 Then
            '    bm = "rnet" + CStr(worddocument.Bookmarks.count) + "_at" + CStr(component_id) + "_en" + CStr(Format(Now, "hhmmss"))
            'Else
            '    bm = "rnet" + CStr(worddocument.bookmarks.count + 1) + "_at" + CStr(component_id) + "_en" + CStr(entity_id)
            '    REM if it exists increase count
            '    Dim i As Integer
            '    i = worddocument.bookmarks.count + 1
            '    While worddocument.bookmarks.exists(bm) = True
            '        i = i + 1
            '        bm = "rnet" + CStr(worddocument.bookmarks.count + 1 + i) + "_at" + CStr(component_id) + "_en" + CStr(entity_id)
            '    End While
            'End If
            'If bwith_table = True Then
            '    Try
            '        worddocument.Tables.Add(Range:=worddocument.application.Selection.Range, NumRows:=2, NumColumns:=1, DefaultTableBehavior:=1, AutoFitBehavior:=0)
            '    Catch
            '        worddocument.Application.Selection.Bookmarks.Add(bm)
            '    End Try
            'Else
            '    Select Case component_type
            '        Case 1, 3, 4, 5, 8, 9, 12
            '            If worddocument.Application.selection.tables.count = 0 Then

            '                Try
            '                    worddocument.Tables.Add(Range:=worddocument.application.Selection.Range, NumRows:=1, NumColumns:=1, DefaultTableBehavior:=1, AutoFitBehavior:=0)
            '                Catch ex As Exception

            '                    worddocument.Application.Selection.Tables(1).Cell(1, 1).Range.Bookmarks.Add(bm)
            '                    worddocument.Application.Selection.Tables(1).Borders.OutsideLineStyle = 0
            '                End Try
            '            Else

            '                Try
            '                    Dim r, c As Integer
            '                    r = worddocument.Application.Selection.Information(13)
            '                    c = worddocument.Application.Selection.Information(16)
            '                    worddocument.Application.Selection.Tables(1).Cell(r, c).Range.Bookmarks.Add(bm)
            '                Catch
            '                    Dim omsg As New bc_cs_message("Blue Curve", "Failed to insert component this selection point please move cussor to new insertion point", bc_cs_message.MESSAGE)
            '                End Try
            '            End If
            '            REM if table put table in first if table already there then disallow
            '        Case 2, 7
            '            If worddocument.Application.selection.tables.count > 0 Then
            '                Dim omsg As New bc_cs_message("Blue Curve", "Cannot insert a new table in an existing table please move cursor to new insertion point", bc_cs_message.MESSAGE)
            '                bm = ""
            '            Else
            '                Try
            '                    worddocument.Tables.Add(Range:=worddocument.application.Selection.Range, NumRows:=2, NumColumns:=1, DefaultTableBehavior:=1, AutoFitBehavior:=0)
            '                Catch
            '                    worddocument.Application.Selection.Bookmarks.Add(bm)
            '                End Try
            '            End If
            '    End Select
            'End If
        Catch ex As Exception
            Dim omessage As New bc_cs_message("Blue Curve", "Cant add component at selected location", bc_cs_message.MESSAGE)
            tag_name = ""
        Finally
            set_locator_for_component = tag_name
            otrace = New bc_cs_activity_log("bc_ao_word", "set_locator_for_component", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Overrides Sub save_template(ByVal templateName As String)
        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "save_template", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim officeFormat As Integer = 1
        Dim officeExstention As String = ""

        Try
            REM SW cope with office versions
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                officeFormat = 27
            Else
                officeFormat = 5
            End If
            'If bc_cs_central_settings.office_version < 12 And bc_cs_central_settings.user_office_version >= 12 Then
            '    officeFormat = 5
            'ElseIf bc_cs_central_settings.office_version < 12 Then
            '    officeFormat = 5
            'ElseIf bc_cs_central_settings.office_version >= 12 Then
            '    officeFormat = 5
            'End If

            powerpointPresentation.saveas(String.Concat(bc_cs_central_settings.local_template_path, templateName), 5)

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "save_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "save_template", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides Sub close(Optional ByVal keep_invisible As Boolean = False, Optional ByVal from_create As Boolean = False)
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "close", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            powerpointPresentation.close()

            'quit_if_no_docs()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "close", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "close", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides ReadOnly Property document_count() As Integer
        Get
            If Not powerpoint Is Nothing Then
                document_count = powerpoint.presentations.count()
            End If
        End Get
    End Property

    Public Overrides Sub quit()

        If Not powerpointAlreadyOpen And Not powerpoint Is Nothing Then

            powerpoint.quit()
            System.Runtime.InteropServices.Marshal.ReleaseComObject(powerpoint)
            GC.Collect()

        End If
    End Sub

    Public Overrides Sub update_properties(ByVal templateID As Integer, ByVal templateName As String)

        Dim log = New bc_cs_activity_log("bc_ao_powerpoint", "update_properties", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            ' update the required custom properties
            powerpointPresentation.CustomDocumentProperties("rnet_template_id") = templateID
            powerpointPresentation.CustomDocumentProperties("xrnet_attached_template") = templateName

        Catch ex As ArgumentException
            ' ignore as not all existing templates will have the 'xrnet_attached_template' property
            log = New bc_cs_activity_log("bc_ao_powerpoint", "update_properties", bc_cs_activity_codes.COMMENTARY, ex.Message)
        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "update_properties", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            ' don't prompt to save document
            powerpointPresentation.Saved = True
            Application.DoEvents()
            log = New bc_cs_activity_log("bc_ao_powerpoint", "update_properties", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overloads Overrides Sub visible()
        REM worddocument.application.visible = True
        If powerpoint.presentations.count = 0 And powerpointAlreadyOpen = False Then
            Me.quit()
            GC.Collect()
        Else
            powerpoint.visible = True
            powerpoint.WindowState = wdwState
        End If
    End Sub

    Public Overrides Sub activate()
        Dim i As Integer
        Dim s As String

        powerpoint.Activate()

        For i = 1 To powerpoint.presentations.Count
            If powerpoint.presentations(i).Name = powerpointPresentation.name Then
                s = powerpointPresentation.name
                Try
                    REM SW cope with office versions
                    If bc_cs_central_settings.userOfficeStatus = 3 Then
                        ' handle 2007 compatibility mode
                        powerpointPresentation.application.windows(String.Concat(s, " [Compatibility Mode]")).activate()
                        powerpointPresentation.application.windows(String.Concat(s, " [Compatibility Mode]")).ActiveWindow.WindowState = wdwState
                    Else
                        powerpointPresentation.application.windows(s).activate()
                        powerpointPresentation.application.windows(s).ActiveWindow.WindowState = wdwState
                    End If

                Catch

                    ' ignore error as only trying to activate window

                End Try
                Exit For
            End If
        Next
    End Sub

    Public Overrides Sub set_doc_id(ByVal doc_id As String)
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "set_doc_id", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            powerpointPresentation.customdocumentproperties("rnet_doc_id").value = CStr(doc_id)
        Catch ex As Exception
            Dim ocommentary As New bc_cs_activity_log("bc_ao_powerpoint", "set_doc_id", bc_cs_activity_codes.COMMENTARY, "Failed to find property: rnet_doc_id")
        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "set_doc_id", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub show()
        If Not IsNothing(powerpoint) Then
            powerpoint.visible = True
        End If
    End Sub

    Public Overrides Function get_doc_id()
        Try
            get_doc_id = powerpointPresentation.customdocumentproperties("rnet_doc_id").value
        Catch ex As Exception
            get_doc_id = "NONE"
            Dim ocommentary As New bc_cs_activity_log("bc_ao_powerpoint", "get_doc_id", bc_cs_activity_codes.COMMENTARY, "Property rnet_doc_id not set in Document.")
        End Try
    End Function

    Public Overrides Function get_name() As String
        get_name = powerpointPresentation.name
    End Function

    Public Overrides Function number_of_pages() As Integer
        number_of_pages = powerpointPresentation.Slides.Count
    End Function

    Public Overrides Function get_document_title() As String
        Try
            get_document_title = ""
            get_document_title = powerpointPresentation.customdocumentproperties("xrnet_title").value()
        Catch ex As Exception
            REM jsut log title property error to activity file
            get_document_title = ""
            Dim ocommentary As New bc_cs_activity_log("bc_ao_powerpoint", "get_document_title", bc_cs_activity_codes.COMMENTARY, "Property xrnet_title not set in Document.")
        End Try
    End Function

    Public Overrides Function get_document_subtitle() As String
        Try
            get_document_subtitle = powerpointPresentation.customdocumentproperties("xrnet_sub_title").value
        Catch ex As Exception
            REM just log sub title property error to activity log
            get_document_subtitle = ""
            Dim ocommentary As New bc_cs_activity_log("bc_ao_powerpoint", "get_document_subtitle", bc_cs_activity_codes.COMMENTARY, "Property xrnet_subtitle not set in Document.")
        End Try
    End Function

    Public Overrides Function get_document_summary() As String
        Dim prop_name As String = ""
        Try
            Dim i As Integer
            get_document_summary = ""
            i = 1
            While i > 0
                prop_name = "xrnet_summary" + CStr(i)
                If i > 1 Then
                    'get_document_summary = get_document_summary + " "
                    get_document_summary = get_document_summary
                End If
                get_document_summary = get_document_summary + powerpointPresentation.customdocumentproperties(prop_name).value
                i = i + 1
            End While
        Catch ex As Exception
            Dim ocommentary As New bc_cs_activity_log("bc_ao_powerpoint", "get_document_summary", bc_cs_activity_codes.COMMENTARY, "Property " + prop_name + " not set in Document.")
            Return ""
        Finally

        End Try
    End Function

    Public Overrides Sub save()

        powerpointPresentation.save()

    End Sub

    Public Overrides Function saveas(ByVal filename As String) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "saveas", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim officeFormat As Integer = 1
        Dim officeExstention As String = ""

        Try
            REM SW cope with office versions
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                officeFormat = 24
                officeExstention = ".pptx"
            Else
                officeFormat = 1
                officeExstention = ".ppt"
            End If

            saveas = "Failed"
            powerpointPresentation.saveas(bc_cs_central_settings.local_repos_path + filename + officeExstention, officeFormat)
            saveas = officeExstention

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "saveas", bc_cs_error_codes.USER_DEFINED, ex.Message)
            saveas = ""
        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "saveas", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Public Overrides Sub invisible()
        Try
            powerpoint.application.visible = False
        Catch
            Try
                powerpointPresentation.application.visible = False
            Catch
            End Try
        End Try

    End Sub

    Public Overrides Function get_object() As Object
        get_object = Me.powerpointPresentation
    End Function

    Public Overrides Sub build(ByVal template_id As Long)
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "build", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As bc_cs_activity_log
        Dim i As Integer
        Dim counter As Integer

        Try
            REM set and assign master properties for portability
            Try
                powerpointPresentation.CustomDocumentProperties.add("rnet_workflow_refresh", False, 4, "")
                powerpointPresentation.CustomDocumentProperties.add("rnet_data_at_data", False, 4, "")
                powerpointPresentation.CustomDocumentProperties.add("rnet_refresh_lang_id", False, 4, "")
                powerpointPresentation.CustomDocumentProperties("rnet_workflow_refresh") = CStr(bc_am_load_objects.obc_current_document.refresh_components.workflow_state)
                powerpointPresentation.CustomDocumentProperties("rnet_refresh_lang_id") = bc_am_load_objects.obc_current_document.language_id
                powerpointPresentation.CustomDocumentProperties("rnet_data_at_data") = CStr(bc_am_load_objects.obc_current_document.refresh_components.data_at_date)
            Catch ex As Exception
                ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "build", bc_cs_activity_codes.COMMENTARY, "Error Setting PortabiltyProperties:" + ex.Message)
            End Try

            REM set properties
            ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "build", bc_cs_activity_codes.COMMENTARY, "Setting Document Properties")
            MyBase.build(template_id)
            For i = 0 To ogproperties.doc_property.Count - 1
                With ogproperties.doc_property(i)
                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "build", bc_cs_activity_codes.COMMENTARY, "Attempting to set property: " + .name)
                    'Rama Comments : Added try block without catch as there is unspecified error
                    Try
                        Select Case .type
                            Case 1
                                powerpointPresentation.CustomDocumentProperties.Add(name:=CStr(.name), LinkToContent:=False, Type:=4, value:="")
                            Case 2
                                powerpointPresentation.CustomDocumentProperties.Add(name:=CStr(.name), LinkToContent:=False, Type:=2, value:=True)
                            Case 3
                                powerpointPresentation.CustomDocumentProperties.Add(name:=CStr(.name), LinkToContent:=False, Type:=1, value:=0)
                            Case 4
                                powerpointPresentation.CustomDocumentProperties.Add(name:=CStr(.name), LinkToContent:=False, Type:=3, value:="")
                        End Select
                    Catch
                        ' Dont catch the error as it will not set other properties.
                    End Try

                End With
            Next
            REM set mandatory metadata property values
            ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "build", bc_cs_activity_codes.COMMENTARY, "Attempting to set mandatory property values.")
            REM entity_id
            powerpointPresentation.CustomDocumentProperties("rnet_entity_id").value = CInt(bc_am_load_objects.obc_current_document.entity_id)
            REM analyst_id
            powerpointPresentation.CustomDocumentProperties("rnet_analyst_id").value = CStr(bc_am_load_objects.obc_current_document.originating_author)
            REM pub_type_id
            powerpointPresentation.CustomDocumentProperties("rnet_pub_type_id").value = CStr(bc_am_load_objects.obc_current_document.pub_type_id)
            REM language_id
            'worddocument.CustomDocumentProperties("rnet_language_id").value = cstr(bc_am_load_objects.obc_current_document.language_id)
            REM FIL June 2013
            REM set this here now as if n document changes happen it doesnt stick

            powerpointPresentation.CustomDocumentProperties("rnet_doc_id").value = CStr(bc_am_load_objects.obc_current_document.filename)

            Try
                For i = 0 To bc_am_load_objects.obc_users.user.Count - 1
                    If bc_am_load_objects.obc_users.user(i).id = bc_cs_central_settings.logged_on_user_id Then
                        powerpointPresentation.customdocumentproperties("rnet_user_role_name").value = bc_am_load_objects.obc_users.user(i).role
                        Exit For
                    End If
                Next
            Catch
                ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "build", bc_cs_activity_codes.COMMENTARY, "Property: rnet_user_role_name not set in document")
            End Try

            REM template components
            ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "build", bc_cs_activity_codes.COMMENTARY, "Attempting to set Components.")
            With ogtemplate.components
                counter = 1
                For i = 0 To .component.Count - 1
                    With .component(i)
                        Select Case .type
                            Case TABLE
                                REM place autotext
                                ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "build", bc_cs_activity_codes.COMMENTARY, "Setting Table: " + .autotext_name)
                                If InsertTableFromGallery(.autotext_name, .autotext_name, .r1c1_ref) = False Then
                                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "build", bc_cs_activity_codes.COMMENTARY, "Insert Table failed to find shape: " + .autotext_name + " or slide: " + .r1c1_ref)
                                Else
                                    REM sub components and component parameters
                                    If insert_sub_components(CInt(.r1c1_ref), .autotext_name, .sub_components, counter) = False Then
                                        ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "build", bc_cs_activity_codes.COMMENTARY, "Insert Sub Component failed to find shape: " + .autotext_name + " or slide: " + .r1c1_ref)
                                    End If
                                End If

                            Case PROP
                                ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "build", bc_cs_activity_codes.COMMENTARY, "Setting Property: " + .bookmark_name + " with value: " + .autotext_name)
                                powerpointPresentation.customdocumentproperties(.bookmark_name).value = CStr(.autotext_name)
                        End Select

                    End With
                Next
            End With

            REM finally save document metadata for this new document to disk
            MyBase.build(template_id)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "build", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            REM housekeep here
            closeTableGallery()
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "build", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub initialize()
        Dim ocommentary As bc_cs_activity_log
        REM call initialisation function
        Try
            ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "initialize", bc_cs_activity_codes.COMMENTARY, "Attempting to call PRESINIT on new")

            bc_cs_central_settings.progress_bar.unload()

            powerpointPresentation.Application.Run(String.Concat(powerpointPresentation.Name, "!ThisPresentation.PRESINIT"), "NEW")
        Catch ex As Exception
            ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "initialize", bc_cs_activity_codes.COMMENTARY, String.Concat("No PRESINIT in Document ", ex.Message, " ", ex.InnerException))
        End Try
    End Sub

    Overrides Function get_component_markup(ByVal with_value As Boolean) As bc_am_ao_locators
        REM parse bookmarks in document looking for automated components and
        REM store in array
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "get_component_markup", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim i As Integer
            Dim j As Integer
            Dim k As Integer
            Dim r As Integer
            Dim c As Integer
            Dim tagName As String
            Dim locators As New bc_am_ao_locators
            Dim locator As bc_am_ao_locator
            Dim value As String
            Dim ocommentary As bc_cs_activity_log
            Dim s As bc_cs_string_services

            For i = 1 To powerpointPresentation.slides.count
                For j = 1 To powerpointPresentation.slides(i).shapes.count
                    For k = 1 To powerpointPresentation.slides(i).shapes(j).Tags.Count
                        tagName = LCase(powerpointPresentation.slides(i).shapes(j).Tags.Name(k))
                        If InStr(tagName, "rnet", CompareMethod.Text) = 1 Then
                            value = ""
                            If with_value = True Then
                                Try
                                    getTagNameLocation(tagName, i, j, r, c)
                                    value = powerpointPresentation.slides(i).shapes(j).cell(r, c).shape.TextFrame.TextRange.Text
                                    value = Left(value, Len(value) - 2)
                                    s = New bc_cs_string_services(value)
                                    value = s.xml_compliant()
                                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "get_component_markup", bc_cs_activity_codes.COMMENTARY, "Text for Tag:" + tagName + " is: " + value)
                                Catch
                                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "get_component_markup", bc_cs_activity_codes.COMMENTARY, "No Text for Tag:" + tagName)
                                End Try
                            End If
                            locator = New bc_am_ao_locator(tagName, value)
                            locator.name = get_locator_user_friendly_name(tagName)
                            locators.locators.Add(locator)
                        End If
                    Next
                Next
            Next
            get_component_markup = locators

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "get_component_markup", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_component_markup = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "get_component_markup", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Public Overrides Sub set_component_markup(ByRef orefresh_components As bc_om_refresh_components, Optional ByVal show_form As Boolean = True, Optional ByVal editChart As Boolean = False)
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.TRACE_ENTRY, "")
        'Dim i, j, k As Integer
        Dim i As Integer
        'Dim placeholder_found As Boolean
        Dim placeholder As String = ""

        Dim slideIndex As Integer
        Dim shapeIndex As Integer
        Dim row As Integer
        Dim col As Integer
        Dim shapeName As String = ""

        Dim topPos As Double = 0
        Dim leftPos As Double = 0
        Dim width As Double
        Dim height As Double

        Dim officeExstention As String = ""
        Dim extensionsize As Integer = 0

        'Const chart_placeholder_count As Integer = 20

        Try
            MyBase.set_excel_running_flag()
            REM loop through list and set value at component

            If orefresh_components.refresh_type = 2 Then
                clear_new_components(orefresh_components.success)
                If (orefresh_components.success = False) Then
                    Exit Sub
                End If
            End If

            Dim ocommentary As New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Setting Component Values into powerpoint")
            For i = 0 To orefresh_components.refresh_components.Count - 1
                If orefresh_components.refresh_components(i).no_refresh = 0 And orefresh_components.refresh_components(i).disabled = 0 Then
                    If show_form = True Then
                        bc_cs_central_settings.progress_bar.change_caption("Refreshing item " + CStr(i + 1) + " of " + CStr(orefresh_components.refresh_components.Count))
                    End If
                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Refreshing Locator:" + orefresh_components.refresh_components(i).locator + " modes:" + CStr(orefresh_components.refresh_components(i).mode))

                    getTagNameLocation(orefresh_components.refresh_components(i).locator, slideIndex, shapeIndex, row, col, shapeName)

                    If Not IsNothing(orefresh_components.refresh_components(i).refresh_value) Then
                        If orefresh_components.refresh_components(i).mode = 1 Or orefresh_components.refresh_components(i).mode = 8 Or orefresh_components.refresh_components(i).mode = 9 Then
                            If UBound(orefresh_components.refresh_components(i).refresh_value, 2) > -1 Then
                                REM check restrictions
                                If orefresh_components.refresh_components(i).restriction_mode = 1 Then
                                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Setting Bookmark:" + orefresh_components.refresh_components(i).locator + " with value:" + orefresh_components.refresh_components(i).refresh_value(0, 0))
                                    powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Table.Cell(row, col).Shape.TextFrame.TextRange.Text = orefresh_components.refresh_components(i).restriction_text
                                ElseIf orefresh_components.refresh_components(i).restriction_mode = 2 Then
                                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Setting Bookmark:" + orefresh_components.refresh_components(i).locator + " with blank as restriction applies mode 2.")
                                    powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Table.Cell(row, col).Shape.TextFrame.TextRange.Text = ""
                                Else
                                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Setting Bookmark:" + orefresh_components.refresh_components(i).locator + " with value:" + orefresh_components.refresh_components(i).refresh_value(0, 0))
                                    Try
                                        powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Table.Cell(row, col).Shape.TextFrame.TextRange.Text = orefresh_components.refresh_components(i).refresh_value(0, 0)
                                    Catch
                                        powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).TextFrame.TextRange.Text = orefresh_components.refresh_components(i).refresh_value(0, 0)
                                    End Try
                                    powerpointPresentation.Slides(slideIndex).Shapes(shapeName).Tags.Add(orefresh_components.refresh_components(i).locator, "1,1")
                                End If
                            End If
                        ElseIf orefresh_components.refresh_components(i).mode = 2 Or orefresh_components.refresh_components(i).mode = 6 Or orefresh_components.refresh_components(i).mode = 26 Then
                            REM standard display Table
                            REM first check restrictions only for type 2
                            REM PR changed shapename to
                            If InsertTableFromGallery(shapeName, orefresh_components.refresh_components(i).mode_param2, slideIndex) = True Then
                                'add the locator back into the tag
                                powerpointPresentation.Slides(slideIndex).Shapes(shapeName).Tags.Add(orefresh_components.refresh_components(i).locator, String.Concat(CStr(row), ",", CStr(col)))
                                update_table_display(orefresh_components.refresh_components(i).locator, orefresh_components.refresh_components(i).refresh_value, orefresh_components.refresh_components(i).restriction_mode, orefresh_components.refresh_components(i).restriction_text, orefresh_components.refresh_components(i).mode_param2)
                            Else
                                ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Table Display for locator: " + orefresh_components.refresh_components(i).locator + " failed to load")
                            End If
                        ElseIf orefresh_components.refresh_components(i).mode = 3 Or orefresh_components.refresh_components(i).mode = 31 Then
                            REM standard display Table                            
                            If get_chart_from_gallery(orefresh_components.refresh_components(i).mode_param2, orefresh_components.refresh_components(i).refresh_value, editChart, powerpointPresentation.name, orefresh_components.refresh_components(i).locator, slideIndex, shapeName, powerpointPresentation) = True Then
                                REM update_chart_display(orefresh_components.refresh_components(i).locator, orefresh_components.refresh_components(i).refresh_value)
                                'Try
                                '    REM try OLE first
                                '    topPos = powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Table.Cell(row, col).Shape.Top
                                '    leftPos = powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Table.Cell(row, col).Shape.Left
                                '    width = powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Table.Cell(row, col).Shape.Width
                                '    height = powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Table.Cell(row, col).Shape.Height

                                '    powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).delete()

                                '    powerpointPresentation.Slides(slideIndex).Shapes.PasteSpecial(Link:=False, DataType:=4, Placement:=1, DisplayAsIcon:=False)

                                '    powerpointPresentation.slides(slideIndex).shapes(powerpointPresentation.slides(slideIndex).shapes.count).top = topPos
                                '    powerpointPresentation.slides(slideIndex).shapes(powerpointPresentation.slides(slideIndex).shapes.count).left = leftPos
                                '    powerpointPresentation.slides(slideIndex).shapes(powerpointPresentation.slides(slideIndex).shapes.count).width = width
                                '    powerpointPresentation.slides(slideIndex).shapes(powerpointPresentation.slides(slideIndex).shapes.count).height = height

                                '    powerpointPresentation.Slides(slideIndex).Shapes(powerpointPresentation.slides(slideIndex).shapes.count).Tags.Add(orefresh_components.refresh_components(i).locator, String.Concat(CStr(row), ",", CStr(col)))
                                '    powerpointPresentation.Slides(slideIndex).Shapes(powerpointPresentation.slides(slideIndex).shapes.count).Name = shapeName

                                'Catch
                                Try
                                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "attempting to paste chart.")

                                    Dim top As Double
                                    Dim left As Double

                                    top = powerpointPresentation.Slides(slideIndex).Shapes(shapeName).Top
                                    left = powerpointPresentation.Slides(slideIndex).Shapes(shapeName).Left
                                    height = powerpointPresentation.Slides(slideIndex).Shapes(shapeName).Height
                                    width = powerpointPresentation.Slides(slideIndex).Shapes(shapeName).Width
                                    powerpointPresentation.Slides(slideIndex).Shapes(shapeName.ToString).Delete()
                                    Try
                                        powerpointPresentation.Slides(slideIndex).Shapes.Paste()
                                    Catch
                                        Try
                                            ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Using Alternate Paste.")
                                            powerpointPresentation.Slides(slideIndex).Shapes.PasteSpecial(Link:=False, DataType:=1, DisplayAsIcon:=False)

                                        Catch ex As Exception
                                            orefresh_components.success = False
                                            ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "paste failed: " + ex.Message)

                                        End Try
                                    End Try
                                    powerpointPresentation.Slides(slideIndex).Shapes(powerpointPresentation.Slides(slideIndex).Shapes.Count).Top = top
                                    powerpointPresentation.Slides(slideIndex).Shapes(powerpointPresentation.Slides(slideIndex).Shapes.Count).Left = left
                                    powerpointPresentation.Slides(slideIndex).Shapes(powerpointPresentation.Slides(slideIndex).Shapes.Count).Height = height
                                    powerpointPresentation.Slides(slideIndex).Shapes(powerpointPresentation.Slides(slideIndex).Shapes.Count).Width = width
                                    powerpointPresentation.Slides(slideIndex).Shapes(powerpointPresentation.Slides(slideIndex).Shapes.Count).Name = shapeName

                                    powerpointPresentation.Slides(slideIndex).Shapes(shapeName).Tags.Add(orefresh_components.refresh_components(i).locator, "1,1")


                                Catch ex As Exception
                                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "paste failed: " + ex.Message)


                                End Try
                                If Not editChart Then
                                    Me.close_chart()
                                End If
                            Else
                                ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Chart Display for locator: " + orefresh_components.refresh_components(i).locator + " failed to load")
                            End If
                        End If
                    ElseIf orefresh_components.refresh_components(i).mode = 4 Or orefresh_components.refresh_components(i).mode = 5 Or orefresh_components.refresh_components(i).mode = 12 Then
                        REM save image to staging area
                        Dim fs As New bc_cs_file_transfer_services
                        If IsArray(orefresh_components.refresh_components(i).objbyte) Then


                            REM SW cope with office versions
                            extensionsize = 0
                            extensionsize = (Len(orefresh_components.refresh_components(i).mode_param2) - (InStrRev(orefresh_components.refresh_components(i).mode_param2, ".") - 1))
                            fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + "tmp" + Right(orefresh_components.refresh_components(i).mode_param2, extensionsize), orefresh_components.refresh_components(i).objbyte, Nothing)

                            Try
                                powerpointPresentation.Slides(slideIndex).Shapes.AddPicture(bc_cs_central_settings.local_repos_path + "tmp" + Right(orefresh_components.refresh_components(i).mode_param2, extensionsize), _
                                                                                            False, True, powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Left, _
                                                                                                        powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Top, powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Width, _
                                                                                                        powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Height)
                            Catch
                                ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Inline Shape Could not be inserted at locator:" + orefresh_components.refresh_components(i).locator)
                            End Try
                            REM delete from stage

                            REM SW cope with office versions
                            extensionsize = 0
                            extensionsize = (Len(orefresh_components.refresh_components(i).mode_param2) - (InStrRev(orefresh_components.refresh_components(i).mode_param2, ".") - 1))
                            fs.delete_file(bc_cs_central_settings.local_repos_path + "tmp" + Right(orefresh_components.refresh_components(i).mode_param2, extensionsize))

                        Else
                            ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Inline Shape not found for locator:" + orefresh_components.refresh_components(i).locator)
                        End If
                        REM document insertion
                    ElseIf orefresh_components.refresh_components(i).mode = 7 Then
                        Dim fs As New bc_cs_file_transfer_services
                        If IsArray(orefresh_components.refresh_components(i).objbyte) Then
                            fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + "tmp" + orefresh_components.refresh_components(i).mode_param2, orefresh_components.refresh_components(i).objbyte, Nothing)
                            REM insertion can only work if not already there the way this is done id to check for a table
                            REM if it exists table table out and insert file from here
                            If row <> -1 Then
                                If powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).HasTable Then
                                    powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Delete()
                                    powerpointPresentation.Slides.InsertFromFile(FileName:=bc_cs_central_settings.local_repos_path + "tmp" + orefresh_components.refresh_components(i).mode_param2, Index:=powerpointPresentation.slides.count)

                                End If
                                REM delete from stage
                                fs.delete_file(bc_cs_central_settings.local_repos_path + "tmp" + orefresh_components.refresh_components(i).mode_param2)
                            End If
                        End If
                    End If
                    orefresh_components.refresh_components(i).last_refresh_date = Now

                    If orefresh_components.refresh_components(i).new_components.Count > 0 Then
                        set_new_components(orefresh_components.refresh_components(i), orefresh_components.success)
                    End If

                End If

            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "set_component_markup", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            powerpointPresentation.application.activate()
            'orefresh_components.new_components.Clear()
            closeTableGallery()
            MyBase.close_excel()

            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Function get_slide_for_shape(shape As String)
        Try
            get_slide_for_shape = 0
            For i = 1 To powerpointPresentation.slides.count
                For j = 1 To powerpointPresentation.slides(i).shapes.count
                    If powerpointPresentation.slides(i).shapes(j).name = shape Then
                        get_slide_for_shape = i
                        Exit Function
                    End If
                Next
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "get_slide_for_shape", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Public Overrides Sub minimize()
        powerpointPresentation.Application.WindowState = 2
    End Sub
    Private Sub clear_new_components(ByRef success As Boolean)
        Try
            Dim na As String
            Dim endslide As Integer
            Dim remstartslide As Integer
            remstartslide = 0
            endslide = 0
            For i = 1 To powerpointPresentation.slides.count - 1
                For j = 1 To powerpointPresentation.slides(i).shapes.count
                    na = powerpointPresentation.slides(i).shapes(j).name
                    If Len(na) > 8 AndAlso Left(na, 9) = "insertrpc" Then
                        If remstartslide = 0 Then
                            remstartslide = i
                        End If
                        endslide = i
                        Exit For
                    End If
                Next
            Next
            
            If remstartslide > 0 Then
                For i = 1 To powerpointPresentation.slides(remstartslide).shapes.count
                    If powerpointPresentation.slides(remstartslide).shapes(i).name = "insertrpc0" Then
                        powerpointPresentation.slides(remstartslide).shapes(i).delete()
                        Exit For
                    End If
                Next
                For i = remstartslide + 1 To endslide
                    powerpointPresentation.slides(remstartslide + 1).delete()
                Next

            End If
            success = True
        Catch ex As Exception
            success = False
            Dim oerr As New bc_cs_error_log("bc_ao_powerpoint", "clear_new_components", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
    Private Sub set_new_components(ByVal orefresh_component As bc_om_refresh_component, ByRef success As Boolean)
        Try
            powerpointPresentation.Application.WindowState = 2
            success = False
            REM new displays to be inserted
            REM firstly clear out old displays
            REM only do if insert file exists

            Dim placeholder As String = ""
            Dim extensionsize As Integer = 0
            Dim officeExstention As String = ""

            Dim shapeName As String = ""

            Dim ocommentary As New bc_cs_activity_log("bc_ao_powerpoint", "set_new_components", bc_cs_activity_codes.COMMENTARY, "Setting Component Values into powerpoint")
            REM get start slide
            Dim startslide, endslide As Integer
            startslide = get_slide_for_shape("rpcstart")
            If startslide = 0 Then
                startslide = powerpointPresentation.slides.count
            End If

            REM insert new slides put first one on exisiting slide
            Dim bm_stem As String = ""

            For i = 0 To orefresh_component.new_components.Count - 2
                bm_stem = Left(orefresh_component.new_components(i).mode_param2, Len(orefresh_component.new_components(i).mode_param2) - 5)
                powerpointPresentation.slides.InsertFromFile(filename:=bc_cs_central_settings.local_template_path + bm_stem + ".pptx", Index:=startslide + i)

            Next
            For i = 0 To orefresh_component.new_components.Count - 1
                'Dim bm_stem As String = ""
                If Not IsNothing(orefresh_component.new_components(i).refresh_value) Then
                    If orefresh_component.new_components(i).mode = 3 Then
                        ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Attempting to add additional display:" + CStr(i + 1))
                        If get_chart_from_gallery(orefresh_component.new_components(i).mode_param2, orefresh_component.new_components(i).refresh_value, False, "", "", -1, "", powerpointPresentation) = True Then
                            Try
                                powerpointPresentation.slides(startslide + i).Shapes.paste()
                            Catch
                                Try
                                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Using Alternate Paste.")
                                    powerpointPresentation.slides(startslide + i + 1).Shapes.PasteSpecial(Link:=False, DataType:=4, Placement:=1, DisplayAsIcon:=False)

                                Catch ex As Exception
                                    success = False
                                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "paste failed: " + ex.Message)
                                End Try
                            End Try

                            powerpointPresentation.slides(startslide + i).Shapes(powerpointPresentation.slides(startslide + i).Shapes.count).name = "insertrpc" + CStr(i)

                            If i = 0 Then
                                Dim top As Double
                                Dim left As Double
                                For j = 1 To powerpointPresentation.slides(startslide + i).Shapes.count
                                    If powerpointPresentation.slides(startslide + i).Shapes(j).name = "rpcstart" Then
                                        top = powerpointPresentation.slides(startslide + i).Shapes(j).top
                                        left = powerpointPresentation.slides(startslide + i).Shapes(j).left

                                        Exit For
                                    End If
                                Next


                                For j = 1 To powerpointPresentation.slides(startslide + i).Shapes.count
                                    If powerpointPresentation.slides(startslide + i).Shapes(j).name = "insertrpc0" Then
                                        powerpointPresentation.slides(startslide + i).Shapes(powerpointPresentation.slides(startslide + i).Shapes.count).top = top
                                        powerpointPresentation.slides(startslide + i).Shapes(powerpointPresentation.slides(startslide + i).Shapes.count).left = left
                                    End If
                                Next
                            Else
                                powerpointPresentation.slides(startslide + i).Shapes(powerpointPresentation.slides(startslide + i).Shapes.count).top = powerpointPresentation.slides(startslide + i).Shapes(powerpointPresentation.slides(startslide + i).Shapes.count - 1).top
                                powerpointPresentation.slides(startslide + i).Shapes(powerpointPresentation.slides(startslide + i).Shapes.count).left = powerpointPresentation.slides(startslide + i).Shapes(powerpointPresentation.slides(startslide + i).Shapes.count - 1).left
                            End If
                            Me.close_chart()
                        End If
                    End If
                End If
            Next
            success = True
        Catch ex As Exception
            success = True
            Dim oerr As New bc_cs_error_log("bc_ao_powerpoint", "set_new_components", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    'Private Sub set_new_components(ByVal orefresh_component As bc_om_refresh_component, ByRef success As Boolean)


    '    REM new displays to be inserted
    '    REM firstly clear out old displays
    '    REM only do if insert file exists

    '    Dim j, k As Integer
    '    Dim placeholder_found As Boolean
    '    Dim placeholder As String = ""
    '    Dim extensionsize As Integer = 0
    '    Dim officeExstention As String = ""

    '    Dim slideIndex As Integer
    '    Dim shapeIndex As Integer
    '    Dim row As Integer
    '    Dim col As Integer
    '    Dim shapeName As String = ""

    '    Const chart_placeholder_count As Integer = 20

    '    Dim ocommentary As New bc_cs_activity_log("bc_ao_powerpoint", "set_new_components", bc_cs_activity_codes.COMMENTARY, "Setting Component Values into powerpoint")
    '    MsgBox(orefresh_component.new_components.Count)
    '    For i = 0 To orefresh_component.new_components.Count - 1
    '        Dim bm_stem As String = ""
    '        If Not IsNothing(orefresh_component.new_components(i).refresh_value) Then
    '            If orefresh_component.new_components(i).mode = 3 Then
    '                ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Attempting to add additional display:" + CStr(i + 1))
    '                REM standard display Table
    '                If get_chart_from_gallery(orefresh_component.new_components(i).mode_param2, orefresh_component.new_components(i).refresh_value, False, "", "", -1, "", powerpointPresentation) = True Then

    '                    REM SW cope with office versions
    '                    extensionsize = 0
    '                    extensionsize = (Len(orefresh_component.new_components(i).mode_param2) - (InStrRev(orefresh_component.new_components(i).mode_param2, ".") - 1))
    '                    bm_stem = Left(orefresh_component.new_components(i).mode_param2, Len(orefresh_component.new_components(i).mode_param2) - extensionsize)
    '                    'bm_stem = Left(orefresh_component.new_components(i).mode_param2, Len(orefresh_component.new_components(i).mode_param2) - 4)

    '                    REM need to see if placeholder exists for this
    '                    REM allow up to the number of place holders specified by the chart_placeholder_count constant
    '                    placeholder_found = False
    '                    REM see if chart is already has place holder reset
    '                    getTagNameLocation(bm_stem + CStr(i + 1) + "_ins", slideIndex, shapeIndex, row, col)
    '                    If row <> -1 Then
    '                        placeholder = bm_stem + CStr(i + 1) + "_ins"
    '                        placeholder_found = True
    '                    End If
    '                    If placeholder_found = False Then
    '                        For j = 1 To chart_placeholder_count
    '                            getTagNameLocation(bm_stem + CStr(i + 1) + "_ins", slideIndex, shapeIndex, row, col)
    '                            If row <> -1 Then
    '                                placeholder = bm_stem + CStr(j)
    '                                placeholder_found = True
    '                                Exit For
    '                            End If
    '                        Next
    '                    End If
    '                    If placeholder_found = False Then

    '                        REM SW cope with office versions
    '                        If bc_cs_central_settings.userOfficeStatus = 2 Then
    '                            officeExstention = ".pptx"
    '                        Else
    '                            officeExstention = ".ppt"
    '                        End If

    '                        REM insert another place holder page
    '                        Dim fs As New bc_cs_file_transfer_services
    '                        If fs.check_document_exists(bc_cs_central_settings.local_template_path + bm_stem + officeExstention) = False Then
    '                            ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "no insert page:" + bc_cs_central_settings.local_repos_path + bm_stem + officeExstention + " so multiple charts not inserted")
    '                            Exit Sub
    '                        End If
    '                        ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Attempting to add insert page:" + bc_cs_central_settings.local_repos_path + bm_stem + officeExstention)
    '                        REM insert at end unless specified
    '                        getTagNameLocation("rpcstart", slideIndex, shapeIndex, row, col)

    '                        If row <> -1 Then
    '                            insert_file(bc_cs_central_settings.local_template_path + bm_stem + officeExstention, True, True, "rpcstart")
    '                        Else
    '                            insert_file(bc_cs_central_settings.local_template_path + bm_stem + officeExstention, True, True)
    '                        End If
    '                        placeholder = bm_stem + "1"

    '                    End If
    '                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Attempting to place chart at locator:" + placeholder)
    '                    REM if not need to nsert a new page to provide place holder
    '                    MsgBox(slideIndex)
    '                    Try
    '                        powerpointPresentation.slides(slideIndex).Shapes.paste()
    '                    Catch
    '                        Try
    '                            ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Using Alternate Paste.")
    '                            powerpointPresentation.slides(slideIndex).Shapes.PasteSpecial(Link:=False, DataType:=4, Placement:=1, DisplayAsIcon:=False)

    '                        Catch ex As Exception
    '                            success = False
    '                            ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "paste failed: " + ex.Message)
    '                        End Try
    '                    End Try
    '                    Me.close_chart()
    '                    REM add a now placed bookmark
    '                    If Right(placeholder, 4) <> "_ins" Then
    '                        REM add a now placed shape
    '                        Dim newShape As Object
    '                        newShape = powerpointPresentation.slides(slideIndex).Shapes.addShape(1, _
    '                                                                                    powerpointPresentation.slides(slideIndex).Shapes(shapeIndex).Left, _
    '                                                                                    powerpointPresentation.slides(slideIndex).Shapes(shapeIndex).Top, _
    '                                                                                    powerpointPresentation.slides(slideIndex).Shapes(shapeIndex).Width, _
    '                                                                                    powerpointPresentation.slides(slideIndex).Shapes(shapeIndex).Height)
    '                        newShape.Name = bm_stem + CStr(i + 1) + "_ins"
    '                        powerpointPresentation.slides(slideIndex).Shapes(shapeIndex).delete()
    '                    End If
    '                    REM later on consider adding a proper locator
    '                Else
    '                    ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_component_markup", bc_cs_activity_codes.COMMENTARY, "Chart Display for locator: " + orefresh_component.new_components(i).locator + " failed to load")
    '                End If
    '            End If
    '        End If
    '        If i = orefresh_component.new_components.Count - 1 Then
    '            REM delete spares
    '            For k = i + 2 To 400
    '                getTagNameLocation(bm_stem + CStr(k) + "_ins", slideIndex, shapeIndex, row, col)
    '                If row <> -1 Then
    '                    powerpointPresentation.Slides(slideIndex).Shapes(shapeIndex).Delete()
    '                End If
    '            Next
    '        End If
    '    Next

    'End Sub
    Public Overrides Function componetize_powerpoint(ByRef ldoc As bc_om_document) As Boolean
        Try
            componetize_powerpoint = False
            Dim ocomp As bc_om_style_component
            ldoc.style_components = New bc_om_document_components()
            If remove_slide_images() = False Then
                Exit Function
            End If
            Dim slidecount As Integer
            slidecount = powerpointPresentation.slides.count
            REM save slides as png
            powerpointPresentation.SaveAs(bc_cs_central_settings.local_repos_path + "d", 18)
            Dim fs As New bc_cs_file_transfer_services
            Dim fn As String
            REM now put suck slides into object
            For i = 1 To slidecount

                fn = bc_cs_central_settings.local_repos_path + "d\Slide" + CStr(i) + ".PNG"

                If fs.check_document_exists(fn, Nothing) = True Then
                    ocomp = New bc_om_style_component
                    If fs.write_document_to_bytestream(fn, ocomp.image, Nothing) = False Then
                        Dim ocomm As New bc_cs_activity_log("bc_ao_powerpoint", "componetize_powerpoint", bc_cs_activity_codes.COMMENTARY, "componetize powerpoint failed slide could not be written to bytestream: " + fn)
                        Exit Function
                    End If
                    ocomp.bimage = True
                    ocomp.page_no = i
                    ocomp.text = "Slide " + CStr(i)
                    ldoc.style_components.components.Add(ocomp)

                Else
                    Dim ocomm As New bc_cs_activity_log("bc_ao_powerpoint", "componetize_powerpoint", bc_cs_activity_codes.COMMENTARY, "componetize powerpoint failed slide not found: " + fn)
                    Exit Function
                End If
            Next

            If remove_slide_images() = False Then
                Exit Function
            End If
            componetize_powerpoint = True
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "componetize_powerpoint", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Function remove_slide_images() As Boolean
        Try
            remove_slide_images = False
            Dim fs As New bc_cs_file_transfer_services
            Dim fn As String
            Dim i As Integer = 1
            Dim found As Boolean = True
            While found = True
                fn = bc_cs_central_settings.local_repos_path + "d\Slide" + CStr(i) + ".PNG"
                If fs.check_document_exists(fn, Nothing) Then
                    fs.delete_file(fn, Nothing)
                Else
                    found = False
                End If
                i = i + 1
            End While
            remove_slide_images = True
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "remove_slide_images", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try


    End Function

    Public Overrides Sub open(ByVal strfilename As String, ByVal visible As Boolean, Optional ByVal new_instance As Boolean = False)
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "open", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            If new_instance = True Then
                invoke_powerpoint_new()
            Else
                invoke_powerpoint()
            End If
            powerpoint.visible = True

            Dim ocommentary As New bc_cs_activity_log("bc_ao_powerpoint", "new", bc_cs_activity_codes.COMMENTARY, "Attempting to Open Document: " + strfilename)
            REM open locally
            powerpointPresentation = powerpoint.presentations.open(strfilename)
            REM set title
            If bc_am_load_objects.obc_current_document.title <> "" Then
                powerpointPresentation.Application.caption = bc_am_load_objects.obc_current_document.title
            End If
            powerpointPresentation.Application.WindowState = wdwState
            powerpointPresentation.Application.activate()
            docobject = powerpointPresentation
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "open", bc_cs_error_codes.USER_DEFINED, ex.Message)
            powerpoint.visible = True

        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "open", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try
    End Sub

    Public Overrides Function get_property_value(ByVal property_name As String) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "get_property_value", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            get_property_value = powerpointPresentation.customdocumentproperties(property_name).value()
        Catch ex As Exception
            get_property_value = "Error"
            Dim ocommentary As New bc_cs_activity_log("bc_ao_powerpoint", "get_property_value", bc_cs_activity_codes.COMMENTARY, "Failed to set property: " + property_name, Nothing)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "get_property_value", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Public Overrides Sub show_all_markup()

        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "show_all_markup", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim tableName As String
            Dim shapeName As String
            Dim slideIndex As Integer
            Dim ocommentary As bc_cs_activity_log
            Dim templateID As Integer

            Dim i As Integer
            Dim j As Integer
            Dim k As Integer

            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Showing Markup...", 10, False, True)
            Cursor.Current = Cursors.WaitCursor

            invisible()

            templateID = powerpointPresentation.CustomDocumentProperties("rnet_template_id").value

            If bc_am_load_objects.obc_templates.template.Count > 1 Then
                Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "show_all_markup", bc_cs_error_codes.USER_DEFINED, "System Error. Please contact support.")
            End If

            If bc_am_load_objects.obc_templates.template.Count = 1 Then
                bc_am_load_objects.obc_templates.template.RemoveAt(0)
            End If

            bc_am_load_objects.obc_templates.add(powerpointPresentation.name, powerpointPresentation.name, templateID)

            bc_cs_central_settings.progress_bar.increment("Showing Markup...")

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                REM rem read in via SOAP                
                ocommentary = New bc_cs_activity_log("bc_ao_word", "show_all_markup", bc_cs_activity_codes.COMMENTARY, "Loading Template via SOAP")
                bc_am_load_objects.obc_templates.template(0).tmode = bc_am_load_objects.obc_templates.template(0).tREAD
                bc_am_load_objects.obc_templates.template(0).transmit_to_server_and_receive(bc_am_load_objects.obc_templates.template(0), True)
            Else
                REM read in directly from database                
                REM templates
                ocommentary = New bc_cs_activity_log("bc_ao_word", "show_all_markup", bc_cs_activity_codes.COMMENTARY, "Loading Template from Database")
                bc_am_load_objects.obc_templates.template(0).db_read(templateID, bc_am_load_objects.obc_templates.template(0).certificate)
            End If

            bc_cs_central_settings.progress_bar.increment("Showing Markup...")

            For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                If bc_am_load_objects.obc_templates.template(i).filename = powerpointPresentation.name Then

                    bc_cs_central_settings.progress_bar.increment("Showing Markup...")

                    For j = 0 To bc_am_load_objects.obc_templates.template(i).components.component.count - 1
                        tableName = bc_am_load_objects.obc_templates.template(i).components.component(j).autotext_name
                        shapeName = bc_am_load_objects.obc_templates.template(i).components.component(j).bookmark_name
                        slideIndex = bc_am_load_objects.obc_templates.template(i).components.component(j).r1c1_ref 'used to store slide index for PowerPoint
                        powerpointPresentation.Slides(slideIndex).Shapes(shapeName).visible = True
                        If InsertTableFromGallery(tableName, tableName, slideIndex) Then
                            For k = 0 To bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component.count - 1

                                ' select the table
                                'powerpointPresentation.Slides(slideIndex).Shapes(shapeName).Select()
                                Dim x As Integer
                                Dim y As Integer
                                Dim desc As String
                                Dim value As String

                                x = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).row
                                y = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).col
                                desc = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).name
                                value = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).value

                                'powerpointPresentation.Slides(slideIndex).Shapes(shapeName).Table.Cell(x, y).Select()
                                If desc <> "Text" And desc <> "Bookmark" Then
                                    powerpointPresentation.Slides(slideIndex).Shapes(shapeName).Table.Cell(x, y).Shape.TextFrame.TextRange.Text = desc
                                Else
                                    If desc = "Text" Or desc = "Bookmark" Then
                                        powerpointPresentation.Slides(slideIndex).Shapes(shapeName).Table.Cell(x, y).Shape.TextFrame.TextRange.Text = value
                                    End If
                                End If

                                'Try
                                '    ocommentary = New bc_cs_activity_log("bc_ao_word", "show_all_markup", bc_cs_activity_codes.COMMENTARY, bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).style)
                                '    worddocument.Application.Selection.Tables(1).Cell(x, y).Range.style = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).style
                                'Catch
                                '    ocommentary = New bc_cs_activity_log("bc_ao_word", "show_all_markup", bc_cs_activity_codes.COMMENTARY, _
                                '        String.Concat(bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).style, " ", "does not exist"))
                                'End Try
                            Next
                        End If
                    Next
                    Exit For
                End If
            Next

            bc_cs_central_settings.progress_bar.increment("Showing Markup...")

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "show_all_markup", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            closeTableGallery()
            visible()
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_ao_powerpoint", "show_all_markup", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub package_for_use()

        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "package_for_use", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim shps(0, 2) As String
            Dim shapeCount As Integer
            Dim i As Integer
            Dim j As Integer
            Dim k As Integer

            Dim shapeFound As Boolean

            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Packaging Template...", 10, False, True)
            Cursor.Current = Cursors.WaitCursor

            invisible()

            ' work out the size of the array
            For i = 0 To powerpointPresentation.Slides.Count - 1
                If i > 0 Then
                    shapeCount = shapeCount + powerpointPresentation.Slides(i + 1).Shapes.Count - 1
                Else
                    shapeCount = shapeCount + powerpointPresentation.Slides(i + 1).Shapes.Count
                End If
                For j = 0 To powerpointPresentation.Slides(i + 1).Shapes.Count - 1
                    If i > 0 Then
                        k = j + shapeCount
                    Else
                        k = j
                    End If
                Next
            Next

            ReDim shps(k, 2)

            ' reset shapeCount
            shapeCount = 0

            REM get all shapes
            For i = 0 To powerpointPresentation.Slides.Count - 1
                If i > 0 Then
                    shapeCount = shapeCount + powerpointPresentation.Slides(i + 1).Shapes.Count - 1
                Else
                    shapeCount = shapeCount + powerpointPresentation.Slides(i + 1).Shapes.Count
                End If
                For j = 0 To powerpointPresentation.Slides(i + 1).Shapes.Count - 1
                    If i > 0 Then
                        k = j + shapeCount
                    Else
                        k = j
                    End If
                    ' shape name
                    shps(k, 0) = powerpointPresentation.Slides(i + 1).Shapes(j + 1).name
                    ' slide index
                    shps(k, 1) = i + 1
                Next
            Next

            bc_cs_central_settings.progress_bar.increment("Packaging Template...")

            Dim halfWay As Integer
            halfWay = Math.Round(UBound(shps, 1) / 2, 0)

            ' hide all sub components
            For i = 0 To UBound(shps, 1)

                shapeFound = False

                If i = halfWay Or i = Math.Round(halfWay / 2) Or i = Math.Round((halfWay / 2) + halfWay) Then
                    bc_cs_central_settings.progress_bar.increment("Packaging Template...")
                End If

                Dim l As Integer

                For l = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                    If bc_am_load_objects.obc_templates.template(l).filename = powerpointPresentation.name Then
                        For j = 0 To bc_am_load_objects.obc_templates.template(l).components.component.count - 1
                            If shps(i, 0) = bc_am_load_objects.obc_templates.template(l).components.component(j).bookmark_name Then
                                shapeFound = True
                                Exit For
                            End If
                        Next
                        Exit For
                    End If
                Next

                If shapeFound Then
                    clear_table_text(shps(i, 1), shps(i, 0))
                    shapeFound = False
                    powerpointPresentation.Slides(CType(shps(i, 1), Integer)).Shapes(shps(i, 0).ToString).Copy()
                    ' update table gallery if already exists
                    copyTableToGallery(shps(i, 0), True)

                    ' remove table leaving the shape
                    Dim top As Double
                    Dim left As Double
                    Dim height As Double
                    Dim width As Double
                    Dim newShape As Object

                    top = powerpointPresentation.Slides(CType(shps(i, 1), Integer)).Shapes(shps(i, 0).ToString).Top
                    left = powerpointPresentation.Slides(CType(shps(i, 1), Integer)).Shapes(shps(i, 0).ToString).Left
                    height = powerpointPresentation.Slides(CType(shps(i, 1), Integer)).Shapes(shps(i, 0).ToString).Height
                    width = powerpointPresentation.Slides(CType(shps(i, 1), Integer)).Shapes(shps(i, 0).ToString).Width
                    powerpointPresentation.Slides(CType(shps(i, 1), Integer)).Shapes(shps(i, 0).ToString).Delete()
                    newShape = powerpointPresentation.Slides(CType(shps(i, 1), Integer)).Shapes.AddShape(1, left, top, width, height) 'msoShapeRectangle
                    newShape.Name = shps(i, 0)
                    newShape.Visible = 0 ' msoFalse
                End If
            Next

            bc_cs_central_settings.progress_bar.increment("Packaging Template...")

            save()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "package_for_use", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            If Me.tablegalleryloaded = True Then
                gallery.save()
            End If
            closeTableGallery()
            visible()
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_ao_powerpoint", "package_for_use", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Function get_selection_values(ByRef row As Object, ByRef col As Object) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "get_selection_values", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim tbl As Object
            Dim rowCount As Integer
            Dim colCount As Integer
            Dim value As String = ""

            row = -1
            col = -1

            Try
                If Not powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).HasTable() Then
                    'check if it is a chart
                    If InStr(LCase(powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Tags.Name(1)), "rnet") Then
                        row = 1
                        col = 1
                    End If
                    Return ""
                End If
            Catch ex As Exception
                ' shape not selected
                Return ""
            End Try

            tbl = powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Table
            With tbl
                For rowCount = 1 To .Rows.Count
                    For colCount = 1 To .Columns.Count
                        If .Cell(rowCount, colCount).Selected Then
                            row = rowCount
                            col = colCount
                            value = .Cell(rowCount, colCount).Shape.TextFrame.TextRange.Text()
                            Exit For
                        End If
                    Next
                    If row <> -1 Then
                        Exit For
                    End If
                Next
            End With

            Return value

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "get_selection_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Return ""
        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "get_selection_values", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Public Overrides Function get_template_id() As String
        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "get_template_id", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            get_template_id = powerpointPresentation.customdocumentproperties("rnet_template_id").value

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "get_template_id", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_template_id = ""
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "get_template_id", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Public Overrides Function check_autotext_entry_exists(ByVal autoTextEntry As String) As Boolean

        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "check_autotext_entry_exists", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer
            Dim j As Integer
            Dim k As Integer
            Dim l As Integer

            For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                If bc_am_load_objects.obc_templates.template(i).filename = powerpointPresentation.name Then
                    For j = 0 To bc_am_load_objects.obc_templates.template(i).components.component.count - 1
                        If LCase(autoTextEntry) = LCase(bc_am_load_objects.obc_templates.template(i).components.component(j).autotext_name) Then
                            Return True
                        End If
                    Next

                    openGallery()

                    For k = 1 To gallery.Slides.count
                        For l = 1 To gallery.Slides(k).Shapes.Count
                            Try
                                If LCase(gallery.Slides(k).Shapes(l).Name) = LCase(autoTextEntry) Then
                                    Return True
                                End If
                            Catch ex As Exception
                                'ignore as it may just not exist on this slide
                            End Try
                        Next
                    Next
                    Exit For
                End If
            Next

            Return False

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "check_autotext_entry_exists", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            closeTableGallery()
            log = New bc_cs_activity_log("bc_ao_powerpoint", "check_autotext_entry_exists", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Public Overrides Sub register_table(ByVal bookmarkName As String, ByVal autoTextEntry As String, ByVal logicalComponentsExist As Boolean)

        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "register_table", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Name = autoTextEntry

            clear_table_text(powerpointPresentation.Application.ActiveWindow.Selection.SlideRange.SlideIndex, autoTextEntry)

            With powerpointPresentation.Application
                copy_table()
                copyTableToGallery(autoTextEntry, False)
                gallery.save()
                closeTableGallery()
            End With

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "register_table", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            powerpoint.WindowState = wdwState
            log = New bc_cs_activity_log("bc_ao_powerpoint", "register_table", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub insert_registered_table(ByVal autoTextName As String, ByVal bookMarkName As String)

        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "insert_registered_table", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            InsertTableFromGallery(autoTextName, autoTextName, powerpointPresentation.Application.ActiveWindow.Selection.SlideRange.SlideIndex)
            gallery.save()
            closeTableGallery()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "insert_registered_table", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "insert_registered_table", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub set_table_cells(ByVal bookmarkName As String)

        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "set_table_cells", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer
            Dim j As Integer
            Dim k As Integer

            For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                If bc_am_load_objects.obc_templates.template(i).filename = powerpointPresentation.name Then
                    Dim ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_table_cells", bc_cs_activity_codes.COMMENTARY, powerpointPresentation.name)
                    For j = 0 To bc_am_load_objects.obc_templates.template(i).components.component.count - 1
                        ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_table_cells", bc_cs_activity_codes.COMMENTARY, bookmarkName)
                        ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_table_cells1", bc_cs_activity_codes.COMMENTARY, bc_am_load_objects.obc_templates.template(i).components.component(j).bookmark_name)
                        If bc_am_load_objects.obc_templates.template(i).components.component(j).bookmark_name = bookmarkName Then
                            ocommentary = New bc_cs_activity_log("bc_ao_powerpoint", "set_table_cells", bc_cs_activity_codes.COMMENTARY, bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component.count)
                            For k = 0 To bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component.count - 1
                                If bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).name <> "Text" And _
                                    bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).name <> "Bookmark" Then
                                    powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Table.Cell(bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).row, _
                                                    bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).col).Shape.TextFrame.TextRange.Text = _
                                                        bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).name
                                Else
                                    If bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).name = "Text" Or _
                                        bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).name = "Bookmark" Then
                                        powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Table.Cell(bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).row, _
                                                    bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).col).Shape.TextFrame.TextRange.Text = _
                                                        bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).value

                                    End If
                                End If
                            Next
                        End If
                    Next
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "set_table_cells", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "set_table_cells", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides Sub delete_table(ByVal logicalComponentsExist As Boolean)

        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "delete_table", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Delete()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "delete_table", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "delete_table", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides Sub copy_table()

        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "copy_table", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Copy()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "copy_table", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "copy_table", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub refresh_bookmark_markup(ByVal bookmarkName As String)
        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "refresh_bookmark_markup", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Dim i As Integer
            Dim j As Integer
            Dim k As Integer

            For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                If bc_am_load_objects.obc_templates.template(i).filename = powerpointPresentation.name Then

                    bc_cs_central_settings.progress_bar.increment("Refreshing Markup...")

                    For j = 0 To bc_am_load_objects.obc_templates.template(i).components.component.count - 1
                        If bookmarkName = bc_am_load_objects.obc_templates.template(i).components.component(j).bookmark_name Then
                            For k = 0 To bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component.count - 1

                                ' select the table
                                powerpointPresentation.Slides(bc_am_load_objects.obc_templates.template(i).components.component(j).r1c1_ref).Shapes(bc_am_load_objects.obc_templates.template(i).components.component(j).bookmark_name).Select()
                                Dim x As Integer
                                Dim y As Integer
                                Dim desc As String
                                Dim value As String

                                x = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).row
                                y = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).col
                                desc = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).name
                                value = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).value

                                powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Table.Cell(x, y).Select()
                                If desc <> "Text" And desc <> "Bookmark" Then
                                    powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Table.Cell(x, y).Shape.TextFrame.TextRange.Text = desc
                                Else
                                    If desc = "Text" Or desc = "Bookmark" Then
                                        powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Table.Cell(x, y).Shape.TextFrame.TextRange.Text = value
                                    End If
                                End If
                                'x = worddocument.Application.Selection.Information(13) 'wdStartOfRangeRowNumber
                                'y = worddocument.Application.Selection.Information(16) 'wdStartOfRangeColumnNumber
                                'Dim ocommentary As bc_cs_activity_log
                                'Try
                                '    ocommentary = New bc_cs_activity_log("bc_ao_word", "refresh_bookmark_markup", bc_cs_activity_codes.COMMENTARY, bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).style)
                                '    worddocument.Application.Selection.Tables(1).Cell(x, y).Range.style = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).style
                                'Catch
                                '    ocommentary = New bc_cs_activity_log("bc_ao_word", "refresh_bookmark_markup", bc_cs_activity_codes.COMMENTARY, _
                                '        String.Concat(bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).style, " ", "does not exist"))
                                'End Try
                            Next
                        End If
                    Next
                    Exit For
                End If
            Next


        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "refresh_bookmark_markup", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "refresh_bookmark_markup", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides Sub set_table_cell(ByVal cellValue As String, ByVal standardDisplay As Boolean)

        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "set_table_cell", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim x As Integer
            Dim y As Integer

            If standardDisplay Then
                powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Table.Cell(1, 1).Shape.TextFrame.TextRange.Text = cellValue
            Else
                get_selection_values(x, y)
                powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Table.Cell(x, y).Shape.TextFrame.TextRange.Text = cellValue
            End If

        Catch ex As Exception
            ' ignore as not always possible to set text
            log = New bc_cs_activity_log("bc_ao_powerpoint", "set_table_cell", bc_cs_activity_codes.COMMENTARY, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "set_table_cell", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Function get_locator_for_display(ByVal atBookmarkOnly As Boolean) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "get_locator_for_display", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i, r, c As Integer
            Dim found As Integer
            Dim na As String

            get_locator_for_display = ""
            get_selection_values(r, c)

            ' check selection type is valid
            If powerpointPresentation.Application.ActiveWindow.Selection.Type = 0 Or _
                powerpointPresentation.Application.ActiveWindow.Selection.Type = 1 Then ' ppSelectionNone or ppSelectionSlide
                Return ""
            End If

            REM Blueprintz

            If Not atBookmarkOnly AndAlso powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).HasTable Then
                get_locator_for_display = powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Name
            End If

            If atBookmarkOnly = True Then
                found = False
                na = ""
                REM first see if bookmark exists in selected cell

                Try
                    For i = 1 To powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Tags.Count
                        If powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Tags.Value(i) = String.Concat(r, ",", c) Then
                            na = LCase(powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Tags.Name(i))
                            If InStr(1, na, "_at") > 1 Then
                                found = True
                                Exit For
                            End If
                        End If
                    Next
                Catch
                    'if merged cells are present try cell(1,1)
                End Try

                REM if not look in cell(1,1)
                If found = False Then
                    For i = 1 To powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Tags.Count
                        If powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Tags.Value(i) = String.Concat(1, ",", 1) Then
                            na = LCase(powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Tags.Name(i))
                            If InStr(1, na, "_at") > 1 Then
                                found = True
                                Exit For
                            End If
                        End If
                    Next
                End If
                If found = False Then
                    get_locator_for_display = ""
                Else
                    get_locator_for_display = na
                End If

            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "get_locator_for_display", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_locator_for_display = ""
        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "get_locator_for_display", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Public Overrides Function get_locator_user_friendly_name(ByVal locator As String) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "get_locator_user_friendly_name", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim ocommentary As New bc_cs_activity_log("bc_ao_powerpoint", "get_locator_user_friendly_name", bc_cs_activity_codes.COMMENTARY, "Locator Name: " + locator)
            get_locator_user_friendly_name = "Slide:"

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "get_locator_user_friendly_name", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_locator_user_friendly_name = ""
        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "get_locator_user_friendly_name", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Public Overrides Sub clear_table_cell(ByVal bookmarkname As String)

        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "clear_table_cell", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim x As Integer
            Dim y As Integer

            get_selection_values(x, y)
            powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Table.Cell(x, y).Shape.TextFrame.TextRange.Text = ""

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "clear_table_cell", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "clear_table_cell", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Function TableAlreadyRegistered() As Boolean
        Dim log As New bc_cs_activity_log("bc_ao_powerpoint", "TableAlreadyRegistered", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            TableAlreadyRegistered = check_autotext_entry_exists(powerpointPresentation.Application.ActiveWindow.Selection.ShapeRange(1).Name)

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_powerpoint", "TableAlreadyRegistered", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_powerpoint", "TableAlreadyRegistered", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Public Overrides Sub open_read_only(ByVal strfilename As String, ByVal name As String)
        Dim otrace As New bc_cs_activity_log("bc_ao_powerpoint", "open_read_only", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            invoke_powerpoint_new()

            powerpoint.visible = True
            REM open locally

            'powerpointPresentation = powerpoint.presentations.open(strfilename)
            'REM make application visible
            'powerpoint.visible = True


            Dim ocommentary As New bc_cs_activity_log("bc_ao_powerpoint", "new", bc_cs_activity_codes.COMMENTARY, "Attempting to Open Document: " + strfilename)
            REM open locally
            powerpointPresentation = powerpoint.presentations.open(strfilename)
            REM set title
            If bc_am_load_objects.obc_current_document.title <> "" Then
                powerpointPresentation.Application.caption = bc_am_load_objects.obc_current_document.title
            End If
            powerpointPresentation.Application.WindowState = wdwState
            powerpointPresentation.Application.activate()
            docobject = powerpointPresentation

            REM set title
            powerpointPresentation.Application.caption = name

            REM Protect
            Me.protect_all_document()


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_powerpoint", "open_read_only", bc_cs_error_codes.USER_DEFINED, ex.Message)
            powerpoint.visible = True

        Finally
            otrace = New bc_cs_activity_log("bc_ao_powerpoint", "open_read_only", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try

    End Sub

   

    Public Overrides Sub protect_all_document()
        Try

            powerpointPresentation.final = True

        Catch ex As Exception
            Dim ocomm As New bc_cs_activity_log("bc_ao_powerpoint", "protect_all_document", bc_cs_activity_codes.COMMENTARY, "Warning: cant lock document: " + ex.Message)

        End Try
    End Sub

End Class



REM ===========================================================================
REM abstract class containing methods used in any output application object
REM ===========================================================================
Public MustInherit Class bc_ao_at_object
    REM bc template
    Protected ogtemplate As New bc_om_template
    Protected ogproperties As New bc_om_doc_properties

    Public Const WORD_DOC = "word"
    Public Const POWERPOINT_DOC = "powerpoint"
    Public Const EXCEL_DOC = "excel"

    REM generic document object
    Public docobject As Object

    REM success flag
    Public bsuccess As Boolean

    Private tword As Object
    Private texcel As Object
    Private gallery As Object
    Private btablegalleryloaded As Boolean
    Private isexcelrunning As Boolean

    Private Const UNSUBMITTED_DOC_PREFIX As String = "BC_"

    Protected document_scan_list As ArrayList
    Protected document_scan_list_index As Integer

    Public Overridable Sub minimize()

    End Sub

    Public Overridable Sub scan_document_for_entities(ByRef taxnomies As List(Of bc_as_cat_taxonomy), extended_terms As Boolean)

    End Sub
    Public Overridable Function insert_side_comment() As String

    End Function
    Public Overridable Function componetize_powerpoint(ByRef ldoc As bc_om_document) As Boolean

    End Function

    Public Overridable Function rendertoPDF_With_office(fn As String) As String
        rendertoPDF_With_office = "Device driver not written for type " + fn
    End Function
    Public Overridable Function check_if_visible()
        check_if_visible = True
    End Function
    Public Overridable Function save_return_error()

    End Function
    Public Overridable Function saveas_return_error(ByVal filename As String) As String

    End Function
    Public Overridable Sub quit_if_no_docs()

    End Sub
    Public Overridable Sub abort_doc()

    End Sub
    Public Overridable Function get_extension() As String
        Return ""
    End Function
    Public Class bc_am_ao_locators
        Public locators As New ArrayList
        Public Sub New()

        End Sub
    End Class
    Public Class bc_am_ao_locator
        Public id As String
        Public value As String
        Public name As String
        Public Sub New(ByVal id As String, ByVal value As String)
            Me.id = id
            Me.value = value
        End Sub

    End Class
    REM abstract methods these are methods that are specifc to the Output object
    REM such as word or powerpoint
    Overridable Sub open(ByVal strfilename As String, ByVal visible As Boolean, Optional ByVal new_instance As Boolean = False)

    End Sub
    Overridable Sub show()

    End Sub
    Overridable Sub close_application()

    End Sub
    Overridable Function number_of_pages() As Integer

    End Function
    Overridable Function get_document_title() As String
        get_document_title = ""
    End Function
    Overridable Function get_document_subtitle() As String
        get_document_subtitle = ""
    End Function
    Overridable Function get_document_summary() As String
        get_document_summary = ""
    End Function
    Overridable Sub activate()

    End Sub
    Overridable Sub save()

    End Sub
    Public Overridable Sub udi_highlight(ByVal bm)
    End Sub
    Public Overridable Sub udi_delete_bookmark(ByVal bm As String)
    End Sub
    Public Overridable Sub udi_delete_bookmark_and_range(ByVal bm As String)

    End Sub
    Public Overridable Function bookmark_count() As Integer

    End Function
    Overridable Function saveas(ByVal filename As String) As String
        saveas = ""
    End Function
    Overridable Sub close(Optional ByVal keep_invisible As Boolean = False, Optional ByVal from_create As Boolean = False)

    End Sub
    Overridable Sub invisible()

    End Sub
    Overridable Sub visible()

    End Sub
    Overridable Sub quit()

    End Sub
    Overridable Sub lock_document()

    End Sub
    Overridable Sub unlock_document()

    End Sub
    Overridable Sub protect_all_document()

    End Sub
    Public Overridable Function get_selection_in_rtf(Optional ByVal locator As String = "") As String
        get_selection_in_rtf = ""
    End Function
    Public Overridable Function set_selection_from_rtf(ByVal bm As String, ByVal rtf As String, ByVal refresh_components As bc_om_refresh_components) As Boolean

    End Function
    Public Overridable Function udi_get_selected_udi_id() As Long
        udi_get_selected_udi_id = 0
    End Function
    Public Overridable Function check_for_sdc() As Boolean
        check_for_sdc = False
    End Function
    Public Overridable Function check_for_udc() As Boolean
        check_for_udc = False
    End Function

    Public Overridable Function udi_get_selected_text() As String
        udi_get_selected_text = ""
    End Function
    Public Overridable Function udi_get_first_paragraph() As String
        udi_get_first_paragraph = ""
    End Function
    Public Overridable Function udi_get_number_of_paragraphs() As String
        udi_get_number_of_paragraphs = ""
    End Function
    Public Overridable Sub udi_set_bookmark(ByVal bm As String)

    End Sub
    Public Overridable Sub udi_set_bookmark_around_paragraphs(ByVal bm As String, ByVal paras As Integer)

    End Sub
    Public Overridable Function udi_set_locator(ByVal udi_id As Long, ByVal entity_id As Long, Optional ByVal new_sel As Boolean = False) As String
        udi_set_locator = ""
    End Function
    Public Overridable Function udi_component_exists(ByVal name As String) As Integer
        udi_component_exists = 0
    End Function
    Overridable Sub build(ByVal template_id As Long)
        save_document_metadata_to_file()
        saveas(bc_am_load_objects.obc_current_document.filename)
    End Sub
    Overridable Sub initialize()

    End Sub
    Public Overridable Sub remove_invalid_bookmarks(ByVal orefresh As bc_om_refresh_components)



    End Sub

    Overridable Sub caption(ByVal caption As String)
    End Sub

    Overridable Sub after_open()

    End Sub
    Public Overridable Function set_next_selection(ByVal bm As String, Optional ByVal page_break As Boolean = True) As Boolean

    End Function
    Public Overridable Function get_background_color_for_locator(ByVal locator As String) As Long

    End Function
    Public Overridable Sub set_background_colour_for_locator(ByVal locator As String, ByVal colour_index As Long, ByVal type As Integer)
    End Sub
    Overridable Function get_value_for_locator(ByVal bm As String) As String
        get_value_for_locator = ""
    End Function
    Overridable Function get_object() As Object
        get_object = Nothing
    End Function
    Overridable Function get_name() As String
        get_name = ""
    End Function
    Overridable Function get_fullname() As String
        get_fullname = ""
    End Function
    REM gets whether server is connected or not by reading property
    Overridable Function server_connected() As String
        server_connected = ""
    End Function
    Public Overridable Function get_locator_user_friendly_name(ByVal locator As String) As String
        get_locator_user_friendly_name = ""
    End Function
    REM gets locator of a display
    Overridable Function get_locator_for_display(ByVal atBookmarkOnly As Boolean) As String
        get_locator_for_display = ""
    End Function
    REM gets markup up from Application object by parsing it
    Overridable Function get_component_markup(ByVal with_value As Boolean) As bc_am_ao_locators
        get_component_markup = Nothing
    End Function
    Public Overridable Function get_property_value(ByVal property_name As String) As String
        get_property_value = ""
    End Function
    REM sets markup up in Application object 
    Overridable Sub set_component_markup(ByRef orefresh_components As bc_om_refresh_components, Optional ByVal show_form As Boolean = True, Optional ByVal editChart As Boolean = False)

    End Sub
    Public Overridable Function componetize(ByRef ocomponents As bc_om_document_components, ByVal componetize_styles As bc_om_componetize_styles_for_template) As Boolean

    End Function

    Public Overridable Sub set_doc_id(ByVal doc_id As String)

    End Sub
    Public Overridable Function get_doc_id()
        get_doc_id = Nothing
    End Function
    REM =========================================================================
    REM Routines that are not specific to Output Application Object
    REM =========================================================================
    'Public Sub open_document(ByVal strfilename As String, ByVal connectivity As Boolean, ByRef odocmetadata As bc_om_document)
    'Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "open_document", bc_cs_activity_codes.TRACE_ENTRY, "")

    'Try
    'bsuccess = False
    'Dim fs As New bc_cs_file_transfer_services
    REM set document metadata
    ' bc_am_load_objects.obc_current_document = odocmetadata
    'If bc_am_load_objects.local_document = False Then
    'f bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
    'fs.file_copy(bc_cs_central_settings.central_repos_path + strfilename, bc_cs_central_settings.local_repos_path + strfilename)
    REM get document metadata
    'bc_am_load_objects.obc_current_document.db_read(bc_am_load_objects.obc_current_document.id)
    'ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
    REM get document metadata
    'bc_am_load_objects.obc_current_document = bc_am_load_objects.obc_current_document.read_xml_via_soap_client_request
    REM write file down from object to filesystem
    '   fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + strfilename, bc_am_load_objects.obc_current_document.byteDoc)
    '  End If
    'End If
    REM set connectvity paramter
    ' bc_am_load_objects.obc_current_document.connection_method = bc_cs_central_settings.selected_conn_method
    REM write down metadata to local area
    'save_document_metadata_to_file()
    REM call application specific object to open document
    'open(strfilename, True)
    'bsuccess = True

    'Catch ex As Exception
    '   Dim db_err As New bc_cs_error_log("bc_ao_word", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)

    'Finally
    '    otrace = New bc_cs_activity_log("bc_ao_word", "new", bc_cs_activity_codes.TRACE_EXIT, "")
    'End Try

    'End Sub

    Public Function get_document_metadata_from_file() As bc_om_document
        Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "get_document_metadata_from_file", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            Dim name As String
            REM get filename
            name = get_doc_id()

            If InStr(1, name, ".") Then
                name = Left(name, InStr(1, name, ".") - 1)
            End If
            Dim ocommentray As New bc_cs_activity_log("bc_ao_at_object", "get_document_metadata_from_file", bc_cs_activity_codes.COMMENTARY, "Attempting to Load Document Metadata from file: " + bc_cs_central_settings.local_repos_path + name + ".dat")

            REM load XML metadata for file into document object
            bc_am_load_objects.obc_current_document = bc_am_load_objects.obc_current_document.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
            'bc_am_load_objects.obc_current_document.filename = name
            get_document_metadata_from_file = bc_am_load_objects.obc_current_document
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_at_object", "get_document_metadata_from_file", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_document_metadata_from_file = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_ao_at_object", "get_document_metadata_from_file", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Protected Sub save_document_metadata_to_file()
        Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "save_document_metadata_to_file", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim fn As String
            Dim orefresh As New bc_om_refresh_component
            Dim coponetlist As New ArrayList

            REM if document doenst have an id yet then use filename minus extension
            If bc_am_load_objects.obc_current_document.id = 0 Then

                REM reorder refresh componets making type 5 last
                For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
                    If bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).mode = 5 Then
                        orefresh = bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i)
                        coponetlist.Add(orefresh)
                    End If
                Next
                For i = 0 To coponetlist.Count - 1
                    orefresh = coponetlist(i)
                    bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Remove(orefresh)
                    bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Add(orefresh)
                Next

                fn = bc_am_load_objects.obc_current_document.filename
            Else
                fn = CStr(bc_am_load_objects.obc_current_document.id)
            End If

            Dim ocommentary As New bc_cs_activity_log("bc_ao_at_object", "save_document_metadata", bc_cs_activity_codes.COMMENTARY, "Attempting to save document metadata file: " + bc_cs_central_settings.local_repos_path + fn + ".dat")
            bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + fn + ".dat")
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_at_object", "save_document_metadata_to_file", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_at_object", "save_document_metadata_to_file", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    REM called to save document to system
    REM at present network document implies ADO connectivity
    'Public Function save_to_system(ByRef odoc_metadata As bc_om_document)
    '    Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "save_to_system", bc_cs_activity_codes.TRACE_ENTRY, "")
    '    Dim bsuccess As Boolean
    '    Try
    '        bsuccess = False
    '        Dim ocommentary As bc_cs_activity_log
    '        Dim fs As New bc_cs_file_transfer_services
    '        Dim ext As String
    '        Dim sfile As System.IO.File
    '        If bc_cs_central_settings.selected_conn_method <> "local" Then
    '            REM Network Save via ado
    '            ocommentary = New bc_cs_activity_log("bc_ao_at_object", "save_to_system", bc_cs_activity_codes.COMMENTARY, "Attempting Network Save using method:" + bc_cs_central_settings.selected_conn_method)
    '            REM flag if document is new
    '            Dim bnew_doc As Boolean = False
    '            If odoc_metadata.id = 0 Then
    '                bnew_doc = True
    '            End If
    '            REM save metadata 
    '            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
    '                odoc_metadata.db_write(Nothing)
    '            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
    '                If odoc_metadata.id = 0 Then
    '                    REM update metadata to get new doc_id back
    '                    odoc_metadata.id = odoc_metadata.write_xml_via_soap_client_request()
    '                End If
    '            End If
    '            If odoc_metadata.id > 0 Then
    '                REM if sucessful save file changing name if newly created
    '                ext = saveas(odoc_metadata.id)
    '                If ext <> "Failed" Then
    '                    REM close file
    '                    close()
    '                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
    '                        REM copy file to repository
    '                        If fs.file_copy(bc_cs_central_settings.local_repos_path + CStr(odoc_metadata.id) + ext, bc_cs_central_settings.central_repos_path + CStr(odoc_metadata.id) + ext) <> bc_cs_error_codes.RETURN_ERROR Then
    '                            ocommentary = New bc_cs_activity_log("bc_ao_at_object", "save_to_system", bc_cs_activity_codes.COMMENTARY, "File Successfully Saved to: " + bc_cs_central_settings.central_repos_path + CStr(odoc_metadata.id) + ext)
    '                            REM delete local copy
    '                        Else
    '                            ocommentary = New bc_cs_activity_log("bc_ao_at_object", "save_to_system", bc_cs_activity_codes.COMMENTARY, "File Failed to save to: " + bc_cs_central_settings.central_repos_path + CStr(odoc_metadata.id) + ext)
    '                            Exit Function
    '                        End If
    '                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
    '                        REM Save file via SOAP
    '                        REM put the file into bytestream
    '                        ocommentary = New bc_cs_activity_log("bc_ao_at_object", "save_to_system", bc_cs_activity_codes.COMMENTARY, "Attempting to Submit file via SOAP for: " + bc_cs_central_settings.local_repos_path + CStr(odoc_metadata.id) + ext)
    '                        fs.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + CStr(odoc_metadata.id) + ext, Nothing)
    '                        odoc_metadata.filename = CStr(odoc_metadata.id) + odoc_metadata.extension
    '                        If odoc_metadata.write_xml_via_soap_client_request() = 0 Then
    '                            ocommentary = New bc_cs_activity_log("bc_ao_at_object", "save_to_system", bc_cs_activity_codes.COMMENTARY, "File Failed to save via soap: " + bc_cs_central_settings.soap_server + " local copy will not be removed")
    '                            Exit Function
    '                        End If
    '                    End If
    '                    ocommentary = New bc_cs_activity_log("bc_ao_at_object", "save_to_system", bc_cs_activity_codes.COMMENTARY, "Attempting to Delete: " + bc_cs_central_settings.local_repos_path + CStr(odoc_metadata.id) + ext)
    '                    sfile.Delete(bc_cs_central_settings.local_repos_path + CStr(odoc_metadata.id) + ext)
    '                    REM delete metadata file
    '                    If bnew_doc = True Then
    '                        ocommentary = New bc_cs_activity_log("bc_ao_at_object", "save_to_system", bc_cs_activity_codes.COMMENTARY, "Attemting to Delete: " + bc_cs_central_settings.local_repos_path + CStr(odoc_metadata.filename) + ".dat")
    '                        sfile.Delete(bc_cs_central_settings.local_repos_path + CStr(odoc_metadata.filename) + ".dat")
    '                    Else
    '                        ocommentary = New bc_cs_activity_log("bc_ao_at_object", "save_to_system", bc_cs_activity_codes.COMMENTARY, "Attemting to Delete: " + bc_cs_central_settings.local_repos_path + CStr(odoc_metadata.id) + ".dat")
    '                        sfile.Delete(bc_cs_central_settings.local_repos_path + CStr(odoc_metadata.id) + ".dat")
    '                    End If
    '                    ocommentary = New bc_cs_activity_log("bc_ao_at_object", "save_to_system", bc_cs_activity_codes.COMMENTARY, "Document successfully submitted: " + bc_cs_central_settings.central_repos_path + CStr(odoc_metadata.id) + ext)
    '                    REM check file back in TBD
    '                    bsuccess = True
    '                End If
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_ao_at_object", "save_to_system", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '    Finally
    '        If bsuccess = False Then
    '            Dim omessage As New bc_cs_message("Blue Curve create", "Document Failed to Submit Check Log Files!", bc_cs_message.MESSAGE)
    '        End If
    '        otrace = New bc_cs_activity_log("bc_ao_at_object", "save_to_system", bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try

    'End Function
    Public Sub save_to_local(ByRef odoc_metadata As bc_om_document)
        Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "save_to_local", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            REM save down metadata

            odoc_metadata.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(odoc_metadata.id) + ".dat")
            REM save file
            save()
            'If odoc_metadata.id > 0 Then
            'save()
            'Else
            '    saveas(get_name)
            'End If
            close()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_at_object", "save_to_local", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_at_object", "save_to_local", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overloads Sub close_table_gallery()
        Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "close_table_gallery", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            If Me.btablegalleryloaded = True Then
                gallery.close(0)
                Me.btablegalleryloaded = False
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_at_object", "close_table_gallery", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_at_object", "close_table_gallery", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overridable Sub highlight_locator(ByVal locator As String, ByVal mode As Integer)

    End Sub

    Public Overloads Sub close_table_gallery(ByVal saveChanges As Boolean)
        Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "close_table_gallery", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try

            If Me.btablegalleryloaded = True Then
                If saveChanges Then
                    gallery.close(-1)
                Else
                    gallery.close(0)
                End If
                Me.btablegalleryloaded = False
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_at_object", "close_table_gallery", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_at_object", "close_table_gallery", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Function get_table_from_gallery(ByVal locator As String) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "get_table_from_gallery", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim ocommentary As bc_cs_activity_log
            get_table_from_gallery = False
            If btablegalleryloaded = False Then
                Try
                    tword = GetObject(, "word.application")
                Catch
                    Try
                        tword = CreateObject("word.application")
                    Catch ex As Exception
                        Dim db_err As New bc_cs_error_log("bc_ao_at_object", "invoke_table_gallery", bc_cs_error_codes.USER_DEFINED, ex.Message)
                        Exit Function
                    End Try
                End Try
                If bc_cs_central_settings.userOfficeStatus = 2 Then
                    ocommentary = New bc_cs_activity_log("bc_ao_at_object", "get_table_from_gallery", bc_cs_activity_codes.COMMENTARY, "Attemting to Open: " + bc_cs_central_settings.local_template_path + "BC Table Gallery.dotx")
                    tword.application.screenupdating = False
                    gallery = tword.documents.open(bc_cs_central_settings.local_template_path + "BC Table Gallery.dotx")
                Else
                    ocommentary = New bc_cs_activity_log("bc_ao_at_object", "get_table_from_gallery", bc_cs_activity_codes.COMMENTARY, "Attemting to Open: " + bc_cs_central_settings.local_template_path + "BC Table Gallery.dot")
                    tword.application.screenupdating = False
                    gallery = tword.documents.open(bc_cs_central_settings.local_template_path + "BC Table Gallery.dot")
                End If

                btablegalleryloaded = True
                REM select table
            End If

            If gallery.bookmarks.exists(locator) Then
                gallery.bookmarks(locator).range.tables(1).select()
                gallery.application.Selection.copy()
                get_table_from_gallery = True
            Else
                ocommentary = New bc_cs_activity_log("bc_ao_at_object", "get_table_from_gallery", bc_cs_activity_codes.COMMENTARY, "Table with locator: " + locator + " does not exist in Table Gallery")
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_at_object", "get_table_from_gallery", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_at_object", "get_table_from_gallery", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Public Function insert_table_into_gallery(ByVal locator As String, ByVal updateIfExistsOnly As Boolean) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "insert_table_into_gallery", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim ocommentary As bc_cs_activity_log
            insert_table_into_gallery = False
            If btablegalleryloaded = False Then
                Try
                    tword = GetObject(, "word.application")
                Catch
                    Try
                        tword = CreateObject("word.application")
                    Catch ex As Exception
                        Dim db_err As New bc_cs_error_log("bc_ao_at_object", "invoke_table_gallery", bc_cs_error_codes.USER_DEFINED, ex.Message)
                        Exit Function
                    End Try
                End Try
                If bc_cs_central_settings.userOfficeStatus = 2 Then
                    ocommentary = New bc_cs_activity_log("bc_ao_at_object", "insert_table_into_gallery", bc_cs_activity_codes.COMMENTARY, "Attemting to Open: " + bc_cs_central_settings.local_template_path + "BC Table Gallery.dotx")
                    tword.application.screenupdating = False
                    gallery = tword.documents.open(bc_cs_central_settings.local_template_path + "BC Table Gallery.dotx")
                Else
                    ocommentary = New bc_cs_activity_log("bc_ao_at_object", "insert_table_into_gallery", bc_cs_activity_codes.COMMENTARY, "Attemting to Open: " + bc_cs_central_settings.local_template_path + "BC Table Gallery.dot")
                    tword.application.screenupdating = False
                    gallery = tword.documents.open(bc_cs_central_settings.local_template_path + "BC Table Gallery.dot")
                End If

                btablegalleryloaded = True
                REM select table
            End If

            If gallery.bookmarks.exists(locator) Then
                gallery.Bookmarks(locator).Range.Tables(1).Select()
                gallery.Bookmarks(locator).Range.Tables(1).Delete()
                gallery.application.Selection.paste()
                insert_table_into_gallery = True
            ElseIf Not updateIfExistsOnly Then
                Dim rng As Object
                rng = gallery.StoryRanges(1) ' wdMainTextStory
                rng.Collapse(0) ' wdCollapseEnd
                rng.InsertParagraph()
                rng.InsertParagraph()
                rng.Collapse(0)  ' wdCollapseEnd
                rng.Select()
                gallery.application.Selection.paste()
                insert_table_into_gallery = True
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_at_object", "insert_table_into_gallery", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_at_object", "insert_table_into_gallery", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    Public Sub set_excel_running_flag()
        isexcelrunning = False
    End Sub
    Public Overridable Function get_selection_in_rtf_compressed(ByRef success As Boolean, Optional ByVal lcator As String = "", Optional ByVal btable As Boolean = False, Optional ByRef compressed_html As Byte() = Nothing, Optional ByVal comp_type As Long = 0, Optional ByVal mode As Integer = 0) As Byte()
        Return Nothing
    End Function

    Public Sub close_excel()
        Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "close_excel", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            If isexcelrunning = True Then
                texcel.quit()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_at_object", "close_excel", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            texcel = Nothing
            GC.Collect()
            otrace = New bc_cs_activity_log("bc_ao_at_object", "close_excel", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overridable Function get_selection_values(ByRef row, ByRef col) As String
        get_selection_values = ""
    End Function
    REM ING JUNE 2012
    REM passed in application object
    Public Function get_chart_from_gallery(ByVal chart_name As String, ByVal values As Object, Optional ByVal editChart As Boolean = False, _
                                            Optional ByVal documentName As String = "", Optional ByVal bookmarkName As String = "", _
                                            Optional ByVal slideIndex As Integer = -1, Optional ByVal shapeName As String = "", Optional ByVal ao_object As Object = Nothing) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "get_chart_from_gallery", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                            System.Threading.Thread.CurrentThread.CurrentCulture

        Try

            System.Threading.Thread.CurrentThread.CurrentCulture = _
                      New System.Globalization.CultureInfo("en-GB")


            Dim workbook As Object
            Dim ocommentary As bc_cs_activity_log
            'If isexcelrunning = False Then
            Try
                texcel = CreateObject("excel.application")
                'isexcelrunning = True
            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_ao_at_object", "invoke_chart_gallery", bc_cs_error_codes.USER_DEFINED, ex.Message)
                Exit Function
            End Try
            texcel.application.screenupdating = False
            texcel.visible = False
            'End If
            Try
                ocommentary = New bc_cs_activity_log("bc_ao_at_object", "get_table_from_gallery", bc_cs_activity_codes.COMMENTARY, "Attempting to open workbook: " + bc_cs_central_settings.local_template_path + chart_name)
                If editChart Then
                    'File.Copy(bc_cs_central_settings.local_template_path + chart_name, bc_cs_central_settings.local_repos_path + chart_name, True)
                    REM change for not read only
                    Dim fs As New bc_cs_file_transfer_services
                    Dim obyte As Byte()
                    fs.write_document_to_bytestream(bc_cs_central_settings.local_template_path + chart_name, obyte, Nothing)
                    fs.write_bytestream_to_document(bc_cs_central_settings.local_repos_path + chart_name, obyte, Nothing)
                    workbook = texcel.workbooks.open(bc_cs_central_settings.local_repos_path + chart_name)

                Else
                    workbook = texcel.workbooks.open(bc_cs_central_settings.local_template_path + chart_name)
                End If
            Catch ex As Exception
                If editChart Then
                    ocommentary = New bc_cs_activity_log("bc_ao_at_object", "get_table_from_gallery", bc_cs_activity_codes.COMMENTARY, "Workbook: " + bc_cs_central_settings.local_repos_path + chart_name + " does not exists.")
                Else
                    ocommentary = New bc_cs_activity_log("bc_ao_at_object", "get_table_from_gallery", bc_cs_activity_codes.COMMENTARY, "Workbook: " + bc_cs_central_settings.local_template_path + chart_name + " does not exists.")
                End If
                Exit Function
            End Try
            REM SET DATA    
            REM always in second sheet
            Dim i As Integer
            REM firtly see if chart is populated in excel
            Try
                ocommentary = New bc_cs_activity_log("bc_ao_at_object", "get_table_from_gallery", bc_cs_activity_codes.COMMENTARY, "attempting to plot chart in excel")
                texcel.workbooks(1).plot_chart(values)
            Catch ex As Exception
                ocommentary = New bc_cs_activity_log("bc_ao_at_object", "get_table_from_gallery", bc_cs_activity_codes.COMMENTARY, "couldnt use excel plot so using system instead: " + ex.Message)
                ocommentary = New bc_cs_activity_log("bc_ao_at_object", "get_table_from_gallery", bc_cs_activity_codes.COMMENTARY, "Start writing data to excel")
                REM if not populate
                With texcel.Workbooks(1).Worksheets(2)
                    For i = 0 To UBound(values, 2)
                        If IsNumeric(values(1, i)) And IsNumeric(values(2, i)) Then
                            .cells(CInt(values(1, i)), CInt(values(2, i))).value = values(0, i)
                        End If
                    Next
                End With
                ocommentary = New bc_cs_activity_log("bc_ao_at_object", "get_table_from_gallery", bc_cs_activity_codes.COMMENTARY, "End writing data to excel")
            End Try

            REM attempt to run routine to select chart 
            Try
                If editChart Then
                    texcel.workbooks(1).getchart(documentName, bookmarkName, slideIndex, shapeName, ao_object)
                Else
                    texcel.workbooks(1).getchart()
                End If
                'texcel.workbooks(1).close(savechanges:=False)
                get_chart_from_gallery = True
                'close_chart()
                If editChart Then
                    texcel.visible = True
                End If
            Catch ex As Exception
                texcel.workbooks(1).close(savechanges:=False)
                ocommentary = New bc_cs_activity_log("bc_ao_at_object", "get_table_from_gallery", bc_cs_activity_codes.COMMENTARY, "Error running getchart routine in workbook:" + bc_cs_central_settings.local_template_path + chart_name + ": " + ex.Message)
            End Try
            'texcel.workbooks(1).close(savechanges:=False)
            'texcel.quit()
            'System.Runtime.InteropServices.Marshal.ReleaseComObject(texcel)


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_at_object", "get_chart_from_gallery", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            GC.Collect()
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            otrace = New bc_cs_activity_log("bc_ao_at_object", "get_chart_from_gallery", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public ready As Boolean = False

    Public Sub close_chart()
        Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "close_chart", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                                 System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = _
                 New System.Globalization.CultureInfo("en-GB")

        Try
            REM clear clipboard
            texcel.displayalerts = False
            texcel.workbooks(1).Sheets(1).Range("A1:A1").Copy()
            texcel.workbooks(1).close(savechanges:=False)
        Catch
            Try
                texcel.workbooks(1).Sheets(2).Range("A1:A1").Copy()
                texcel.workbooks(1).close(savechanges:=False)
            Catch ex As Exception

            End Try
            REM close excel        

        Finally
            texcel.displayalerts = True
            texcel.quit()
            GC.Collect()
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            otrace = New bc_cs_activity_log("bc_ao_at_object", "close_chart", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overridable Sub set_file_name()

    End Sub
    Public Sub load_index_components(ByRef index_components As bc_om_indexs)
        Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "load_index_components", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            get_index_components(index_components)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_at_object", "load_index_components", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_at_object", "load_index_components", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Overridable Sub get_index_components(ByRef index_components As bc_om_indexs)


    End Sub
    Public Function load_refresh_components(ByVal with_value As Boolean) As bc_om_refresh_components

        Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "load_refresh_components", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            REM parses locators and build up refresh object
            Dim locators As New bc_am_ao_locators
            Dim bc_om_refresh_components As New bc_om_refresh_components
            Dim i As Integer
            Dim st, stt, en As String
            Dim n, e As Integer

            REM get component markup
            locators = get_component_markup(with_value)
            REM fill out refresh components data from markup
            If Not locators Is Nothing Then
                For i = 0 To locators.locators.Count - 1
                    REM parse out component id
                    Dim ocomponent As New bc_om_refresh_component
                    st = locators.locators(i).id
                    Dim ocommentary As New bc_cs_activity_log("bc_ao_at_object", "load_refresh_components", bc_cs_activity_codes.COMMENTARY, "Locator found: " + st)
                    n = InStr(1, st, "at")
                    If n > 0 Then
                        st = Right(st, Len(st) - n - 1)
                        e = InStr(1, st, "_en")
                        stt = Left(st, e - 1)
                        en = Right(st, Len(st) - e - 2)
                        ocommentary = New bc_cs_activity_log("bc_ao_at_object", "load_refresh_components", bc_cs_activity_codes.COMMENTARY, "Component Id extracted: " + stt + " for entity_id: " + CStr(en))
                        ocomponent.type = CInt(stt)
                        ocomponent.locator = locators.locators(i).id
                        ocomponent.entity_id = CLng(en)
                        ocomponent.locator_description = locators.locators(i).name
                        bc_om_refresh_components.refresh_components.Add(ocomponent)
                        If with_value = True Then
                            ocomponent.value = locators.locators(i).value
                            ocommentary = New bc_cs_activity_log("bc_ao_at_object", "load_refresh_components", bc_cs_activity_codes.COMMENTARY, "Attaching Refresh component value:" + locators.locators(i).value)
                        End If
                    End If
                    n = 0
                    n = InStr(1, st, "udc")
                    If n > 0 Then
                        st = Right(st, Len(st) - n - 2)
                        e = InStr(1, st, "_en")
                        stt = Left(st, e - 1)
                        en = Right(st, Len(st) - e - 2)
                        ocommentary = New bc_cs_activity_log("bc_ao_at_object", "load_refresh_components", bc_cs_activity_codes.COMMENTARY, "Component Id extracted: " + stt + " for entity_id: " + CStr(en))
                        ocomponent.type = CInt(stt)
                        ocomponent.locator = locators.locators(i).id
                        ocomponent.entity_id = CLng(en)
                        ocomponent.locator_description = locators.locators(i).name
                        bc_om_refresh_components.refresh_components.Add(ocomponent)
                    End If
                    If InStr(st, "rnetrtg", CompareMethod.Text) > 0 Then
                        ocommentary = New bc_cs_activity_log("bc_ao_at_object", "load_refresh_components", bc_cs_activity_codes.COMMENTARY, "rnetetg not implemented for: " + st)
                    End If
                Next
            End If
            load_refresh_components = bc_om_refresh_components

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_at_object", "load_refresh_components", bc_cs_error_codes.USER_DEFINED, ex.Message)
            load_refresh_components = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_ao_at_object", "load_refresh_components", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Function generate_local_filename_prefix() As String
        generate_local_filename_prefix = UNSUBMITTED_DOC_PREFIX + bc_cs_central_settings.logged_on_user_name + CStr(Format(Now, "ddMMyyyyHHmmss"))
    End Function
    Overridable Sub set_parameter(ByVal prop As String, ByVal value As String)

    End Sub
    Public Sub set_parameters(ByRef params As Object)
        If IsNothing(params) Then
            Exit Sub
        End If
        Dim i As Integer
        For i = 0 To params.parameters.count - 1
            If params.parameters(i).type = 10 Then
                set_parameter(params.parameters(i).prop, params.parameters(i).value)
            End If
        Next
    End Sub
    Public Overridable Sub update_refresh_status_bar(ByVal caption As String, ByVal item As Integer, ByVal total_items As Integer)

    End Sub
    Public Overridable Sub convert_to_rtf(ByVal fn As String)

    End Sub
    Public Sub populate_document(ByRef orefresh As bc_om_refresh_components, Optional ByVal show_form As Boolean = True, Optional ByVal editChart As Boolean = False)
        Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "populate_document", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            REM single cell component
            set_component_markup(orefresh, show_form, editChart)


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_at_object", "populate_document", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_at_object", "populate_document", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub set_component_in_document_metadata(ByVal ocomponent As bc_om_sub_component, ByVal locator As String, ByVal entity_id As Long, ByVal taxonomy As List(Of bc_om_taxonomy))
        Dim otrace As New bc_cs_activity_log("bc_ao_at_object", "set_component_in_document_metadata", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim orefresh As New bc_om_refresh_component
            orefresh.type = ocomponent.type
            orefresh.name = ocomponent.name
            orefresh.row = ocomponent.row
            orefresh.col = ocomponent.col
            orefresh.contributor_id = ocomponent.contributor_id
            orefresh.parameters = ocomponent.parameters
            orefresh.mode = ocomponent.mode
            orefresh.mode_param1 = ocomponent.mode_param1
            orefresh.mode_param2 = ocomponent.mode_param2
            orefresh.mode_param3 = ocomponent.mode_param3
            REM instance based values for component
            orefresh.last_refresh_date = "9-9-9999"
            orefresh.entity_id = entity_id
            orefresh.locator = locator
            orefresh.refresh_type = ocomponent.refresh_type
            orefresh.web_service_name = ocomponent.web_service_name
            orefresh.cache_level = ocomponent.cache_level
            orefresh.format_file = ocomponent.format_file
            orefresh.external_id = ocomponent.external_id
            orefresh.parameters = ocomponent.parameters
            REM secondary entity
            Try
                orefresh.parent_entity_id = taxonomy(1).entity_id
            Catch

            End Try

            REM assign to document metadata objects
            bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Add(orefresh)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_at_object", "set_component_in_document_metadata", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_at_object", "set_component_in_document_metadata", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Sub New()

    End Sub

    Public Overridable Sub new_template()

    End Sub
    Public Overridable Sub set_activedocument(ByVal name As String)

    End Sub
    Public Overridable Function get_activedocument()
        get_activedocument = Nothing
    End Function
    Public Overridable Sub add_references()

    End Sub
    Public Overridable Sub enable_screen_updating()
    End Sub
    Public Overridable Sub disable_screen_updating()
    End Sub


    Public Overridable Function tmp_saveas(ByVal filename As String) As String
        tmp_saveas = ""
    End Function

    Public Overridable Sub disable_track_changes()

    End Sub
    Public Overridable Sub enable_track_changes()

    End Sub
    Public Overridable Sub add_properties(ByVal templateID As Integer, ByVal templateName As String)

    End Sub
    Public Overridable Sub show_sdc_udc(ByVal locator As String, ByVal colour_index As Long, ByVal type As Integer)

    End Sub

    Public Overridable Sub update_properties(ByVal templateID As Integer, ByVal templateName As String)

    End Sub
    Public Overridable Sub update_parameter_values_to_document(ByVal refresh_components As bc_om_refresh_components, ByRef ldoc As bc_om_document)

    End Sub
    Public Overridable Sub update_parameter_values_from_document(ByRef refresh_components As bc_om_refresh_components, ByRef ldoc As bc_om_document)

    End Sub


    Public Overridable Sub save_template(ByVal templateName As String)

    End Sub

    Public Overridable ReadOnly Property document_count() As Integer
        Get

        End Get
    End Property

    Public Overridable Function open_template(ByVal templateName As String, ByVal visible As Boolean) As Boolean

    End Function

    Public Overridable Sub copy_template(ByVal source As String, ByVal destination As String)

    End Sub

    Public Overridable Sub show_all_markup()

    End Sub

    Public Overridable Sub package_for_use()

    End Sub

    Public Overridable Sub get_all_bookmarks(ByRef ao_object As ListView)

    End Sub

    Public Overridable Function get_template_id() As String
        get_template_id = ""
    End Function

    Public Overridable Sub clear_table_cell(ByVal bookmarkName As String)

    End Sub
    Public Overridable Function set_selection(ByVal bm As String) As Boolean

    End Function
    Public Overridable Function page_break() As Boolean

    End Function
    Public Overridable Function get_selection_style() As String
        get_selection_style = ""
    End Function
    Public Overridable Function check_udc_in_document(ByVal udc_id As Long) As Boolean
    End Function
    Public Overridable Function get_images_in_selection(ByVal locator As String, Optional ByVal bwithtable As Boolean = False) As ArrayList
        Return Nothing
    End Function
    Public Overridable Function check_sdc_in_document(ByVal sdc_id As Long, ByVal type_id As Long, ByVal entity_id As Long) As Boolean
    End Function
    Public Overridable Function check_autotext_entry_exists(ByVal autoTextEntry As String) As Boolean

    End Function
    Public Overridable Function set_locator_for_component(ByVal component_id As Long, ByVal entity_id As Long, ByVal component_type As Integer, Optional ByVal bwith_table As Boolean = True) As String
        set_locator_for_component = ""
    End Function
    Public Overridable Sub open_read_only(ByVal strfilename As String, ByVal name As String)

    End Sub

    Public Overridable Sub register_table(ByVal bookmarkName As String, ByVal autoTextEntry As String, ByVal logicalComponentsExist As Boolean)

    End Sub

    Public Overridable Sub set_table_cell(ByVal cellValue As String, ByVal standardDisplay As Boolean)

    End Sub

    Public Overridable Sub delete_table(ByVal logicalComponentsExist As Boolean)

    End Sub

    Public Overridable Sub insert_registered_table(ByVal autoTextName As String, ByVal bookMarkName As String)

    End Sub

    Public Overridable Sub copy_table()

    End Sub

    Public Overridable Sub set_table_cells(ByVal bookmarkName As String)

    End Sub

    Public Overridable Sub refresh_bookmark_markup(ByVal bookmarkName As String)

    End Sub

    Public Function FileIsLocked(ByVal fileFullPathName As String) As Boolean

        Dim isLocked As Boolean = False
        Dim fileObj As System.IO.FileStream = Nothing

        Try
            fileObj = New System.IO.FileStream( _
                                                fileFullPathName, _
                                                System.IO.FileMode.Open, _
                                                System.IO.FileAccess.ReadWrite, _
                                                System.IO.FileShare.None)
        Catch ex As Exception
            isLocked = True
        Finally
            If Not fileObj Is Nothing Then
                fileObj.Close()
            End If
        End Try

        Return isLocked

    End Function

    Public Overridable Function getSlideIndex() As Integer

    End Function

    Public Overridable Function TableAlreadyRegistered() As Boolean

    End Function

    Public Overridable Function test_for_code() As Boolean

    End Function

    Public Overridable Function scan_document(ByVal listType As String, ByVal namespaceValue As String, _
                                              ByVal node As String) As ArrayList
        scan_document = Nothing
    End Function

    Protected matchStringPredicate As New Func(Of XElement, Boolean)(AddressOf matchString)

    Protected getKeyPredicate As New Func(Of XElement, String)(AddressOf getKey)

    Protected Function matchString(ByVal xmlElement As XElement) As Boolean

        For Me.document_scan_list_index = 0 To document_scan_list.Count - 1
            If Regex.IsMatch(LCase(xmlElement.Value.ToString), String.Concat("\b", LCase(document_scan_list(document_scan_list_index)(0, 0).ToString), "\b")) Then
                Return True
            End If
        Next document_scan_list_index

        Return False
    End Function

    Protected Function getKey(ByVal xmlElement As XElement) As String
        Return xmlElement.Value
    End Function

End Class

REM ==========================================
REM Blue Curve Limited 2005
REM Module:       AT/BP Application Object Specific
REM Type:         Application Object
REM Description:  Excel
REM Version:      1
REM Change history
REM ==========================================
REM builds a word document
Public Class bc_ao_excel
    Inherits bc_ao_at_object

    ' excel application 
    Private excel As Object
    ' excel workbook instance
    Public excelworkbook As Object
    Public Sub New(ao_object)
        docobject = ao_object

        excelworkbook = ao_object
        excel = ao_object.application
    End Sub
    'Public Overrides Sub save()
    '    excelworkbook.save()
    'End Sub

    Public Overrides Sub quit_if_no_docs()
        If excel.workbooks.count = 0 Then
            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
            GC.WaitForPendingFinalizers()
            excel.quit()
            Marshal.FinalReleaseComObject(excel)
            excel = Nothing
        Else
            excel.visible = True
        End If
    End Sub

    Public Overrides Function rendertoPDF_With_office(fn As String) As String
        Try




            Try
                docobject.ExportAsFixedFormat(Type:=0, Filename:=fn, Quality:=0, IncludeDocProperties:=True, IgnorePrintAreas:=False, OpenAfterPublish:=False)

            Catch ex As Exception
                rendertoPDF_With_office = ex.Message
                Exit Function
            End Try

            rendertoPDF_With_office = ""
        Catch ex As Exception
            rendertoPDF_With_office = ex.Message
            Dim ocommentary = New bc_cs_activity_log("bc_ao_word", "rendertoPDF_with_office", bc_cs_activity_codes.COMMENTARY, ex.Message)

        End Try

    End Function

    Public Function insert_data_into_workbook(ByVal data As Object)
        insert_data_into_workbook = False

        Try
            For i = 0 To UBound(data, 2)
                docobject.worksheets(data(1, i)).cells(data(2, i), data(3, i)) = data(0, i)
                REM hard code for now in 5.6 set alignment from sp
                docobject.worksheets(data(1, i)).cells(data(2, i), data(3, i)).HorizontalAlignment = -4152
            Next
            insert_data_into_workbook = True
        Catch ex As Exception

            Dim ocomm As New bc_cs_activity_log("bc_ao_excel", "insert_data_into_workbook", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try

    End Function
    REM routine to open a word document 
    Private Sub invoke_excel()
        Dim otrace As New bc_cs_activity_log("bc_ao_excel", "invoke_excel", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            excel = GetObject(, "excel.application")

            bc_cs_central_settings.user_office_version = excel.Version
        Catch
            Try
                excel = CreateObject("excel.application")

                REM SW cope with office versions
                bc_cs_central_settings.user_office_version = excel.Version

            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_ao_excel", "invoke_excel", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try

        Finally
            otrace = New bc_cs_activity_log("bc_ao_excel", "invoke_excel", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Function open_template(ByVal templateName As String, ByVal visible As Boolean) As Boolean

        Dim log As New bc_cs_activity_log("bc_ao_excel", "open_template", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            If excel Is Nothing Then
                invoke_excel()
            End If
            excelworkbook = excel.Workbooks.Open(templateName)

            'Application.DoEvents()

            If visible Then
                excel.visible = visible
            End If

            Return True

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_excel", "open_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_excel", "open_template", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Public Overrides Sub new_template()

        Dim log As New bc_cs_activity_log("bc_ao_excel", "new_template", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            If excel Is Nothing Then
                invoke_excel()
            End If

            ' add a new workbook
            excelworkbook = excel.Workbooks.Add

            excel.DisplayAlerts = False

            Dim i As Integer

            ' remove any extra sheets leaving just the one
            For i = excelworkbook.Sheets.Count To 2 Step -1
                excelworkbook.Sheets(i).Delete()
            Next

            excel.DisplayAlerts = True

            ' add chart and rename sheet 1 data sheets
            excelworkbook.Sheets(1).Name = "Data"
            Dim sheet As Object
            sheet = excelworkbook.Sheets.Add()
            sheet.Name = "Chart"


        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_excel", "new_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_excel", "new_template", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub add_references()

        Dim log = New bc_cs_activity_log("bc_ao_excel", "add_references", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            'If test_for_code() = False Then
            ' add default code
            excelworkbook.VBProject.VBComponents(1).CodeModule.AddFromFile(String.Concat(bc_cs_central_settings.local_template_path, "default_chart_code.txt"))
            'Else
            'MsgBox("code already exists!")
            'End If

        Catch ex As System.Runtime.InteropServices.COMException
            ' references already exists
            log = New bc_cs_activity_log("bc_ao_excel", "add_references", bc_cs_activity_codes.COMMENTARY, ex.Message)
        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_excel", "add_references", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_excel", "add_references", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    'Sam 2.1.8 register chart - check if vba code exists 
    Public Overrides Function test_for_code() As Boolean
        'returns true if excelworkbook contains VBA code, false otherwise
        Dim obj As Object
        Dim i As Integer
        For Each obj In excelworkbook.VBProject.VBComponents
            With obj.CodeModule
                '# lines - # declaration lines > 2 means we do have code
                i = i + ((.CountOfLines - .CountOfDeclarationLines) > 2)
            End With
            If i = 0 Then
                Exit For 'stop when 1st found
            End If
        Next obj
        Return CBool(i)

    End Function

    Public Overrides Sub save_template(ByVal templateName As String)
        Dim log As New bc_cs_activity_log("bc_ao_excel", "save_template", bc_cs_activity_codes.TRACE_ENTRY, "")


        Dim officeFormat As Integer = -4143

        Try

            REM SW cope with office versions
            If bc_cs_central_settings.userOfficeStatus = 3 Then
                officeFormat = 56
            ElseIf bc_cs_central_settings.userOfficeStatus = 1 Then
                officeFormat = -4143
            ElseIf bc_cs_central_settings.userOfficeStatus = 2 Then
                officeFormat = 52
            End If
            'If bc_cs_central_settings.office_version < 12 And bc_cs_central_settings.user_office_version >= 12 Then
            '    officeFormat = 56
            'ElseIf bc_cs_central_settings.office_version < 12 Then
            '    officeFormat = -4143
            'ElseIf bc_cs_central_settings.office_version >= 12 Then
            '    officeFormat = 51
            'End If

            'MsgBox(String.Concat(bc_cs_central_settings.local_template_path, templateName) + " " + officeFormat)
            excelworkbook.saveas(String.Concat(bc_cs_central_settings.local_template_path, templateName), officeFormat)

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_excel", "save_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_excel", "save_template", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub


    'REM saves document to filename
    Public Overrides Function saveas(ByVal filename As String) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_excel", "saveas", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim officeFormat As Integer = 0
        Dim officeExtention As String = ""

        Try
            REM SW cope with office versions
            If bc_cs_central_settings.userOfficeStatus = 3 Then
                officeFormat = 56
                officeExtention = ".xls"
            ElseIf bc_cs_central_settings.userOfficeStatus = 1 Then
                officeFormat = -4143
                officeExtention = ".xls"
            ElseIf bc_cs_central_settings.userOfficeStatus = 2 Then
                officeFormat = 52
                officeExtention = ".xlsx"
            End If

            saveas = "Failed"
            excelworkbook.saveas(bc_cs_central_settings.local_repos_path + filename + officeExtention, FileFormat:=officeFormat)
            saveas = officeExtention

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_excel", "saveas", bc_cs_error_codes.USER_DEFINED, ex.Message)
            saveas = ""
        Finally
            otrace = New bc_cs_activity_log("bc_ao_excel", "saveas", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function



    Public Overrides Sub close(Optional ByVal keep_invisible As Boolean = False, Optional ByVal from_create As Boolean = False)
        Dim otrace As New bc_cs_activity_log("bc_ao_excel", "close", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Try
               

                excel.workbooks(1).close(savechanges:=False)
                Try
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(excelworkbook)
                Catch

                End Try
                Try
                    System.Runtime.InteropServices.Marshal.ReleaseComObject(docobject)
                Catch

                End Try
               
            Catch

                docobject.close()
            End Try

            Catch ex As Exception
                Dim db_err As New bc_cs_error_log("bc_ao_excel", "close", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Finally
                otrace = New bc_cs_activity_log("bc_ao_excel", "close", bc_cs_activity_codes.TRACE_EXIT, "")
            End Try
    End Sub

    Public Overrides Sub quit()

        Dim otrace As New bc_cs_activity_log("bc_ao_excel", "quit", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
           
            excel.application.quit()


            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel)


            GC.Collect()
            GC.WaitForPendingFinalizers()
            GC.Collect()
            GC.WaitForPendingFinalizers()
            System.Runtime.InteropServices.Marshal.ReleaseComObject(excel)


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_excel", "quit", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            GC.Collect()

            otrace = New bc_cs_activity_log("bc_ao_excel", "quit", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides ReadOnly Property document_count() As Integer
        Get
            If Not excel Is Nothing Then
                document_count = excel.Workbooks.count()
            End If

        End Get
    End Property

    Public Overloads Overrides Sub visible()

        Dim otrace As New bc_cs_activity_log("bc_ao_excel", "visible", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            REM worddocument.application.visible = True
            If excel.Workbooks.count = 0 Then
                quit()
            Else
                excel.visible = True
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_excel", "visible", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_excel", "visible", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub activate()

        Dim otrace As New bc_cs_activity_log("bc_ao_excel", "activate", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer
            Dim s As String
            For i = 1 To excel.Workbooks.Count
                If excel.Workbooks(i).Name = excelworkbook.name Then
                    s = excelworkbook.name
                    excelworkbook.application.windows(s).activate()
                    Exit For
                End If
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_excel", "activate", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            otrace = New bc_cs_activity_log("bc_ao_excel", "activate", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub


    Public Overrides Sub copy_template(ByVal source As String, ByVal destination As String)

        Dim log As New bc_cs_activity_log("bc_ao_excel", "copy_template", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            FileCopy(source, destination)

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_ao_excel", "copy_template", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_ao_excel", "copy_template", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub


    Public Overrides Sub open(ByVal strfilename As String, ByVal visible As Boolean, Optional ByVal new_instance As Boolean = False)

        Dim otrace As New bc_cs_activity_log("bc_ao_excel", "open", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            excel = CreateObject("excel.application")
            docobject = excel.workbooks.open(strfilename)

        Catch ex As Exception


        Finally
            otrace = New bc_cs_activity_log("bc_ao_excel", "open", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try
    End Sub



    Public Sub New()

    End Sub
    Public Function populate_table(values As Object, worksheet As Integer, ByRef offset_row As Integer, offset_col As Integer)
        populate_table = False
        Try
            Dim rc As Integer
            For i = 0 To UBound(values, 2)
                excelworkbook.worksheets(worksheet).cells(offset_row + values(1, i), offset_col + values(2, i)) = values(0, i)
                Try
                    excelworkbook.worksheets(worksheet).cells(offset_row + values(1, i), offset_col + values(2, i)).style = values(3, i)
                Catch
                    Dim ocomm As New bc_cs_activity_log("bc_ao_excel", "populate_table", bc_cs_activity_codes.COMMENTARY, "style not found:" + CStr(values(3, i)))
                End Try
                rc = CInt(values(1, i))
            Next
            offset_row = offset_row + rc + 1
            populate_table = True
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_excel", "populate_table", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function
    Public Function set_component_markup_disclosures(ByRef orefresh_components As bc_om_refresh_components, worksheet As String, row_start As Integer, col_start As Integer, chart_row_offset As Integer, final_worksheet_selected As String) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_ao_excel", "set_component_markup_disclosures", bc_cs_activity_codes.TRACE_ENTRY, "")

        set_component_markup_disclosures = False

        Try
            Dim ocommentary As bc_cs_activity_log

            Dim ws As Integer = 0
            For i = 1 To excelworkbook.worksheets.count
                If excelworkbook.worksheets(i).name = worksheet Then
                    ws = i
                    Exit For
                End If
            Next

            If ws = 0 Then
                ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup_disclosures", bc_cs_activity_codes.COMMENTARY, "Excel disclosures failed cant find worksheet:" + worksheet)
                Exit Function
            End If
            excelworkbook.Worksheets(ws).Cells.Clear()
            For i = 1 To excelworkbook.Worksheets(ws).shapes.count
                excelworkbook.Worksheets(ws).shapes(1).delete()
            Next

            ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup_disclosures", bc_cs_activity_codes.COMMENTARY, "Setting Component Values into excel")
            For i = 0 To orefresh_components.refresh_components.Count - 1
                ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup_disclosures", bc_cs_activity_codes.COMMENTARY, "Refreshing type:" + CStr(orefresh_components.refresh_components(i).type))
                If orefresh_components.refresh_components(i).mode = 2 Then
                    If populate_table(orefresh_components.refresh_components(i).refresh_value, ws, row_start, col_start) = False Then
                        Exit Function
                    End If
                ElseIf orefresh_components.refresh_components(i).mode = 3 Then

                    For j = 0 To orefresh_components.refresh_components(i).new_components.Count - 1
                        If get_chart_from_gallery(orefresh_components.refresh_components(i).new_components(j).mode_param2, orefresh_components.refresh_components(i).new_components(j).refresh_value, False, "", "", -1, "", excelworkbook) = False Then
                            ocommentary = New bc_cs_activity_log("bc_ao_word", "set_component_markup_disclosures", bc_cs_activity_codes.COMMENTARY, "Excel disclosures failed cant get chart from gallery")
                            Exit Function
                        End If
                        excelworkbook.worksheets(ws).cells(row_start + 1, col_start + 1) = ""
                        excelworkbook.worksheets(ws).cells(row_start + 1, col_start + 1).PasteSpecial()
                        row_start = row_start + chart_row_offset + 2
                        Me.close_chart()

                    Next

                End If
            Next

            For i = 1 To excelworkbook.worksheets.count
                If excelworkbook.worksheets(i).name = final_worksheet_selected Then
                    excelworkbook.worksheets(i).select()
                    Exit For
                End If
            Next
            set_component_markup_disclosures = True
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_excel", "set_component_markup_disclosures", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            otrace = New bc_cs_activity_log("bc_ao_excel", "set_component_markup_disclosures", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
End Class

