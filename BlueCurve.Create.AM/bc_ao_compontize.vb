Imports System
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports Microsoft.VisualBasic.Strings
Imports System.IO
Imports System.Windows.Forms
Imports System.Collections
Imports System.Data

'This file is created by Rama Boya 
Public Class bc_ao_componetize_word
    Inherits bc_ao_componentize


    Public oworddocument As Object
    Public objconf As bc_cs_configure
    Dim intTextCount As Integer = 0
    Dim intNoBookmarkTextCount As Integer = 0
    Private intTableCount As Integer = 0
    Private TablesDataCount As Integer = 0
    Private intImageCount As Integer
    Private TablesInfoArray(20000, 3) As String
    Private TablesDataArray(20000, 5) As String
    Private TextDataArray(20000, 2) As String
    Private VarcharMultiDataArray(1000) As String
    Private intVarcharMultiCount As Integer = 0
    Private NoBookMarkTextDataArray(10000, 2) As String
    Private ImageDataArray(1000, 2) As String
    Private undeftable As New DataSet
    Private intImageCounter As Integer = 0
    Private strComponentizeBy As String
    Private blnFinalResult As Boolean = True
    'THIS property is set to false when there is an error 
    Private Function format_rtf(ByVal str As String) As String
        Try

            Dim stripoutvalues As New ArrayList
            stripoutvalues.Add("themedata")
            stripoutvalues.Add("colorschememapping")
            stripoutvalues.Add("datastore")

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
        Catch ex As Exception
            format_rtf = ""
            Dim oerr As New bc_cs_error_log("bc_ao_componetize", "format_rtf", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Function
    Public Property Result() As Boolean
        Get
            Return blnFinalResult
        End Get
        Set(ByVal Value As Boolean)
            blnFinalResult = Value
        End Set
    End Property
    Public Sub New(ByRef ao_object As Object)
        Dim blnResult As Boolean
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "New", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            oworddocument = ao_object
            'read the composite configuration file.
            objconf = New bc_cs_configure
            Dim strErr As String
            blnResult = objconf.Read_Componentize_ini()
            'set the componentiser constant variables.
            'set the error message flag here.
            strErr = "Successfully read"
            ComponentiserConstants.ShowErrorMessage = objconf.DisplayErrorMsg()

            'set the table RTF flag
            If objconf.table_read_rich_text_conversion.ToLower.Trim = "yes" Then
                ComponentiserConstants.TableTextConverToRTF = True
            End If
            'set the main text RTF flag
            If objconf.main_area_read_rich_text_conversion.ToLower.Trim = "yes" Then
                ComponentiserConstants.MainTextConverToRTF = True
            End If
            strErr = strErr & "successfully set RTF flags"
            'set the central repository flag
            'Rama Boya modified the following code on 20/12/2007 to incoporate separate repository.
            If objconf.cen_reposit_read_flag.ToLower.Trim = "yes" Then
                '
                ComponentiserConstants.PubTypeId = CStr(Me.GetDocumentProperty("rnet_pub_type_id"))
                ComponentiserConstants.Set_Central_repository()
                'check if the value is null or not
                'if its null then set it to central repository
                If ComponentiserConstants.CentralRepository.Trim = "" Then
                    ComponentiserConstants.CentralRepository = bc_cs_central_settings.central_repos_path
                End If
            Else
                ComponentiserConstants.CentralRepository = bc_cs_central_settings.central_repos_path
            End If
            strErr = strErr & "successfully set CRP flags"




            'set the  text RTF flag
            If objconf.text_read_rich_text_conversion.ToLower.Trim = "yes" Then
                ComponentiserConstants.TextConverToRTF = True
            End If
            'Capture the activity log file info
            ComponentiserConstants.LogActivityFile = objconf.LogActivity()
            'set the table height and width info flag
            If objconf.table_capture_heightWidth_flag.ToLower.Trim = "yes" Then
                ComponentiserConstants.CaptureHeightWidthInfoofTable = True
            Else
                ComponentiserConstants.CaptureHeightWidthInfoofTable = False
            End If
            'set the table cell merged flag
            If objconf.table_has_merged_cells.ToLower.Trim = "yes" Then
                ComponentiserConstants.TableHasMergedCells = True
            Else
                ComponentiserConstants.TableHasMergedCells = False
            End If
            'set the CaptureTableCellRGBValue flag
            If objconf.CapTableCellRGB.ToLower.Trim = "yes" Then
                ComponentiserConstants.CaptureTableCellRGBValue = True
            Else
                ComponentiserConstants.CaptureTableCellRGBValue = False
            End If
            'set the table cell merged flag
            If objconf.table_contains_images.ToLower.Trim = "yes" Then
                ComponentiserConstants.TableHasImages = True
            Else
                ComponentiserConstants.TableHasImages = False
            End If
            'set the result flag property
            Me.Result = blnResult
            If Not blnResult Then
                ' stop the execution if there is an error in reading composite configuration file
                Exit Sub
            End If
        Catch ex As Exception
            Me.Result = False
            Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "new", bc_cs_activity_codes.COMMENTARY, "Error in new(constructor) method." & ex.Message)
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "New", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Property TextCount() As Integer
        Get
            Return intTextCount
        End Get
        Set(ByVal Value As Integer)
            intTextCount = Value
        End Set
    End Property
    Public Property NoBookMarkTextCount() As Integer
        Get
            Return intNoBookmarkTextCount
        End Get
        Set(ByVal Value As Integer)
            intNoBookmarkTextCount = Value
        End Set
    End Property

    Public Property ImageCount() As Integer
        Get
            Return intImageCount
        End Get
        Set(ByVal Value As Integer)
            intImageCount = Value
        End Set
    End Property
    Public Property TableCount() As Integer
        Get
            Return intTableCount
        End Get
        Set(ByVal Value As Integer)
            intTableCount = Value
        End Set
    End Property
    Public Property VarcharMultiCountCount() As Integer
        Get
            Return intVarcharMultiCount
        End Get
        Set(ByVal Value As Integer)
            intVarcharMultiCount = Value
        End Set
    End Property

    Public Property ArrayTableInfo() As Array
        Get
            Return TablesInfoArray
        End Get
        Set(ByVal Value As Array)
            TablesInfoArray = Value
        End Set
    End Property
    Public Property ArrayTableData() As Array
        Get
            Return TablesDataArray
        End Get
        Set(ByVal Value As Array)
            TablesDataArray = Value
        End Set
    End Property
    Public Property ArrayTextData() As Array
        Get
            Return TextDataArray
        End Get
        Set(ByVal Value As Array)
            TextDataArray = Value
        End Set
    End Property
    Public Property ArrayVarcharMultiData() As Array
        Get
            Return VarcharMultiDataArray
        End Get
        Set(ByVal Value As Array)
            VarcharMultiDataArray = Value
        End Set
    End Property
    Public Property ArrayNoBookmarkTextData() As Array
        Get
            Return NoBookMarkTextDataArray
        End Get
        Set(ByVal Value As Array)
            NoBookMarkTextDataArray = Value
        End Set
    End Property
    Public Property ArrayImageData() As Array
        Get
            Return ImageDataArray
        End Get
        Set(ByVal Value As Array)
            ImageDataArray = Value
        End Set
    End Property

    Public Property ActDocument() As Object
        Get
            Return oworddocument
        End Get
        Set(ByVal Value As Object)
            oworddocument = Value
        End Set
    End Property
    Public Property UndefTableData() As DataSet
        Get
            Return undeftable
        End Get
        Set(ByVal Value As DataSet)
            undeftable = Value
        End Set
    End Property
    Public Property ImageFileCount() As Integer
        Get
            Return intImageCounter
        End Get
        Set(ByVal Value As Integer)
            intImageCounter = Value
        End Set
    End Property
    Public Property ComponentizeBy() As String
        Get
            Return strComponentizeBy
        End Get
        Set(ByVal Value As String)
            strComponentizeBy = Value
        End Set
    End Property
    'This function returns the given document property value.
    Public Overrides Function GetDocumentProperty(ByVal PropertyName As String) As Object
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "GetDocumentProperty", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim strDocPropertyValue As String = ""
        Try
            Dim returnValue As IEnumerator
            Dim strParameter As String

            returnValue = ActDocument.CustomDocumentProperties.GetEnumerator()
            While (returnValue.MoveNext)
                strParameter = returnValue.Current.name
                If InStr(strParameter.ToLower.Trim, PropertyName.ToLower.Trim) > 0 Then
                    strDocPropertyValue = returnValue.Current.value
                End If
            End While
            '  otrace = New bc_cs_activity_log("bc_ao_componetize_word", "GetDocumentProperty", bc_cs_activity_codes.TRACE_EXIT, "")
        Catch ex As Exception
            Me.Result = False
            strDocPropertyValue = ""
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "GetDocumentProperty", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "GetDocumentProperty", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If strDocPropertyValue <> "" Then
                GetDocumentProperty = strDocPropertyValue
            Else
                GetDocumentProperty = ""
            End If
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "get_main_text", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    'This function returns the given bookmark value
    Public Overrides Function GetBookMarkValue(ByVal BookMarkName As String) As Object
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "GetBookMarkValue", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim strDocBMValue As String = ""
        Dim strActual As String
        Dim strlocvalue As String
        Dim data As IDataObject
        Try
            Dim strParameter As String
            Dim i As Int16
            For i = 1 To ActDocument.Bookmarks.Count
                strParameter = CStr(ActDocument.Bookmarks(i).Name)
                If InStr(strParameter.ToLower.Trim, BookMarkName.ToLower.Trim) > 0 Then
                    strlocvalue = ActDocument.Bookmarks(i).Range.Text
                    If ComponentiserConstants.TextConverToRTF AndAlso CStr(strlocvalue).Trim <> "" Then
                        Try
                            ''Convert text into rich text 
                            ActDocument.Bookmarks(i).Range.CopyAsPicture()
                            data = Clipboard.GetDataObject()
                            If Not data Is Nothing AndAlso _
                                data.GetDataPresent("Rich Text Format") Then
                                strDocBMValue = data.GetData("Rich Text Format", True)
                                REM PR FEB 2014
                                strDocBMValue = format_rtf(strDocBMValue)
                            Else
                                strDocBMValue = ""
                            End If
                            'set data is nothing
                            data = Nothing
                        Catch ex As Exception
                            Me.Result = False
                            strDocBMValue = ""
                            If ComponentiserConstants.ShowErrorMessage Then
                                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
                            Else
                                Dim otrace3 As New bc_cs_activity_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_activity_codes.COMMENTARY, ex.Message)
                            End If
                        End Try
                    ElseIf Not strlocvalue Is Nothing AndAlso strlocvalue.Trim <> "" Then
                        strActual = ComponentiserConstants.RemoveSpecialChars(strlocvalue)
                        If Not strActual Is Nothing AndAlso strActual <> "" Then
                            strDocBMValue = strActual
                        Else
                            strDocBMValue = ""
                        End If
                    End If
                End If
            Next
            '  otrace = New bc_cs_activity_log("bc_ao_componetize_word", "GetBookMarkValue", bc_cs_activity_codes.TRACE_EXIT, "")
        Catch ex As Exception
            Me.Result = False
            strDocBMValue = ""
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "GetBookMarkValue", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "GetBookMarkValue", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If strDocBMValue.Trim <> "" Then
                GetBookMarkValue = strDocBMValue.Trim
            Else
                GetBookMarkValue = ""
            End If
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "get_main_text", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    'This function will return the matched bookmark name from the given part bookmark name in a document
    Public Function GetBookMarkName(ByVal BookMarkName As String) As String
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "GetBookMarkName", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim strParameter As String
            Dim i As Int16
            Dim strDocBMName As String = ""
            For i = 1 To ActDocument.Bookmarks.Count
                strParameter = CStr(ActDocument.Bookmarks(i).Name)
                If InStr(strParameter.ToLower.Trim, BookMarkName.ToLower.Trim) > 0 Then
                    strDocBMName = strParameter
                    Return strDocBMName.Trim
                End If
            Next
            'If the bookmark is not found send null value.
            Return ""
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "GetBookMarkName", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "GetBookMarkName", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
            Return ""
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "get_main_text", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
        ' otrace = New bc_cs_activity_log("bc_ao_componetize_word", "GetBookMarkName", bc_cs_activity_codes.TRACE_EXIT, "")
    End Function
    'This function will return the matched bookmark name from the given part bookmark name in a document
    Public Function GetBookMarkNameArray(ByVal BookMarkName As String) As Boolean
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "GetBookMarkNameArray", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim blnInsideLoop As Boolean = False
        Try
            Dim strParameter As String
            Dim i As Int16
            Dim strDocBMName As String = ""
            For i = 1 To ActDocument.Bookmarks.Count
                strParameter = CStr(ActDocument.Bookmarks(i).Name)
                If InStr(strParameter.ToLower.Trim, BookMarkName.ToLower.Trim) > 0 Then
                    Me.ArrayVarcharMultiData(VarcharMultiCountCount) = strParameter
                    VarcharMultiCountCount = VarcharMultiCountCount + 1
                    blnInsideLoop = True
                End If
            Next
            If blnInsideLoop Then VarcharMultiCountCount = VarcharMultiCountCount - 1
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "GetBookMarkNameArray", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "GetBookMarkNameArray", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            GetBookMarkNameArray = blnInsideLoop
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "get_main_text", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    ' This function set's the single cell properties values(TextCount,ArrayTextData) .
    Public Overrides Function GetSingleCellValues()
        Return get_single_cell_components()
    End Function
    ' This function set's the image properties  values(ImageCount,ArrayImageData).
    Public Overrides Function GetImageValues()
        Return get_image_components()
    End Function
    ' This function set's the table properties values(TableCount,TablesInfoArray,TablesDataArray).
    Public Overrides Sub GetTableCellValues()
        Dim TableOutput(300, 2) As String
        get_table_components()
        MsgBox("total count is ..." & TableCount)
    End Sub
    ''This function adds the component object to components collection
    Public Sub AddComponentObject(ByRef CompsContainer As ComponentContainer, ByRef ObjComp As bc_am_component)
        If ComponentiserConstants.LogActivityFile Then If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "AddComponentObject", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim blnObjExists As Boolean = False
            Dim exsObj As bc_am_component
            'loop through the collection to verify whether that object is already existing in the collection or not.
            For Each exsObj In CompsContainer.Components
                If exsObj.Locator.ToLower.Trim = ObjComp.Locator.ToLower.Trim Then
                    blnObjExists = True
                    Exit For
                End If
            Next
            If Not blnObjExists Then
                CompsContainer.Components.Add(ObjComp)
            End If
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "AddComponentObject", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "AddComponentObject", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "AddComponentObject", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    'This function returns the undefined component rich text in a document.
    Public Function get_undefined_text(ByRef CompsContainer As ComponentContainer) As String
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "get_main_text", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim strMainText As String = ""
        Dim strParagraphText As String = ""

        'Used Late Binding for Microsoft.Office.Interop.Word.Paragraph
        Dim oParagragh As Object
        'oParagragh = CreateObject("Microsoft.Office.Interop.Word.Paragraph")

        Dim data As IDataObject
        Dim strPropertyText As String
        Dim strCompanyTitle As String
        Dim intUndefOrder As Integer = 0
        Dim strActual As String
        strCompanyTitle = Me.GetDocumentProperty("mdap_Company")
        Try

            For Each oParagragh In ActDocument.StoryRanges(1).Paragraphs 'Microsoft.Office.Interop.Word.WdStoryType.wdMainTextStory
                intUndefOrder = intUndefOrder + 1
                ' Loop through each paragraphs and collect text,table and images.
                If oParagragh.Range.InlineShapes.Count = 0 AndAlso _
                   oParagragh.Range.Tables.Count = 0 Then
                    'Then this is a text value
                    strPropertyText = oParagragh.Range.Text
                    ' convert the plain text into rich text except for document property paragraph element.
                    If strPropertyText.ToLower.Trim <> strCompanyTitle.ToLower.Trim AndAlso _
                        strPropertyText.Trim.Length > 1 Then
                        'convert into rich text only when the xml configuration flag is set to true.
                        If ComponentiserConstants.MainTextConverToRTF Then
                            Try
                                oParagragh.Range.CopyAsPicture()
                                data = Clipboard.GetDataObject()
                                If Not data Is Nothing AndAlso _
                                    data.GetDataPresent("Rich Text Format") Then
                                    strParagraphText = data.GetData("Rich Text Format", True)
                                    'PR FEB 2104
                                    strParagraphText = format_rtf(strParagraphText)

                                End If
                            Catch ex As Exception
                                Me.Result = False
                                strParagraphText = ""
                                If ComponentiserConstants.ShowErrorMessage Then
                                    Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_undefined_text", bc_cs_error_codes.USER_DEFINED, ex.Message)
                                Else
                                    Dim otrace3 As New bc_cs_activity_log("bc_ao_componetize_word", "get_undefined_text", bc_cs_activity_codes.COMMENTARY, ex.Message)
                                End If
                            End Try
                        Else
                            strParagraphText = oParagragh.Range.Text
                            'replace special characters
                            If Not strParagraphText Is Nothing AndAlso strParagraphText.Trim <> "" Then
                                strActual = ComponentiserConstants.RemoveSpecialChars(strParagraphText)
                                If Not strActual Is Nothing AndAlso strActual <> "" Then
                                    strParagraphText = strActual
                                Else
                                    strParagraphText = ""
                                End If
                            End If
                        End If
                        strMainText = strParagraphText
                        Dim ObjComp As bc_am_component
                        Dim intPageno As String
                        Dim strCellStyle As String
                        intPageno = oParagragh.Range.Information(3) 'WdInformation.wdActiveEndPageNumber
                        strCellStyle = "PageNo=" & intPageno & ","
                        strCellStyle = strCellStyle & "HorzPos=" & oParagragh.Range.Information(5) & "," 'WdInformation.wdHorizontalPositionRelativeToPage
                        strCellStyle = strCellStyle & "VertPos=" & oParagragh.Range.Information(6) & "," 'WdInformation.wdVerticalPositionRelativeToPage

                        'Instantiate component object with all single text values and add the same to components collection.
                        ObjComp = New bc_am_component
                        ObjComp.ComponentType = bc_am_component.CompType.Text
                        'Generate locator id as we dont have definied type for main text
                        ObjComp.Locator = "Undefined_Text_" & intUndefOrder
                        'No entity and component id's as there is no bookmark
                        ObjComp.Value = strMainText
                        ObjComp.OrderNo = intUndefOrder
                        ObjComp.Style = strCellStyle

                        Me.AddComponentObject(CompsContainer, ObjComp)
                        ObjComp = Nothing
                    End If
                End If
            Next
            Return strMainText

        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_undefined_text", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "get_undefined_text", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
            Return ""
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "get_main_text", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    'This function returns an array with all single text values with no book marks in a document.
    Public Function get_text_comp_with_nobookmark() As Object
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "get_text_comp_with_nobookmark", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim strActual As String
            Dim output(500, 2) As String
            Dim rnet_nobookmark_count As Integer = 0

            'Used Late Binding for Microsoft.Office.Interop.Word.Shape
            'Dim oShape As Microsoft.Office.Interop.Word.Shape
            Dim oShape As Object
            'oShape = CreateObject("Microsoft.Office.Interop.Word.Shape")

            For Each oShape In ActDocument.Shapes
                If oShape.TextFrame.HasText _
                   AndAlso oShape.TextFrame.TextRange.Bookmarks.Count = 0 Then
                    rnet_nobookmark_count = rnet_nobookmark_count + 1
                    'As there is no bookmark generate one and use the same.
                    output(rnet_nobookmark_count, 0) = "NoBookMark_Text_" & rnet_nobookmark_count
                    If Not CStr(oShape.TextFrame.TextRange.Text) Is Nothing Then
                        strActual = ComponentiserConstants.RemoveSpecialChars(oShape.TextFrame.TextRange.Text)
                        If Not strActual Is Nothing AndAlso strActual <> "" Then
                            output(rnet_nobookmark_count, 1) = strActual
                        Else
                            output(rnet_nobookmark_count, 1) = ""
                        End If
                    Else
                        output(rnet_nobookmark_count, 1) = ""
                    End If
                End If
            Next
            Me.NoBookMarkTextCount = rnet_nobookmark_count
            ' MsgBox(rnet_bookmark_count)
            Me.NoBookMarkTextDataArray = output
            get_text_comp_with_nobookmark = output
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_text_comp_with_nobookmark", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "get_text_comp_with_nobookmark", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If

            get_text_comp_with_nobookmark = Nothing
            Me.NoBookMarkTextCount = 0
            Me.NoBookMarkTextDataArray = Nothing
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "get_text_comp_with_nobookmark", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    ' This function set's the single cell properties values(TextCount,ArrayTextData) .
    Public Function get_single_cell_components() As Object
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "get_single_cell_components", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim output(500, 2) As String
            Dim i As Integer
            Dim rnet_bookmark_count As Integer
            Dim strlocname As String
            Dim strlocvalue As String
            Dim intAsciivalue As Integer = 0
            Dim data As IDataObject
            Dim strActual As String
            Dim strBookMarkName As String
            Dim strTextBookname1 As String = objconf.ComponentizeTextString1
            Dim strTextBookname2 As String = objconf.ComponentizeTextString2
            'if the table option values are null then assign with some dummy string to avoid infinite loop
            If strTextBookname1.Trim = "" Then
                strTextBookname1 = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ"
            End If
            If strTextBookname2.Trim = "" Then
                strTextBookname2 = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ"
            End If
            rnet_bookmark_count = 0
            'set the search string

            If objconf.ComponentizeBy.ToLower.Trim = bc_cs_configure.strAnyWhere.ToLower.Trim Then
                For i = 1 To ActDocument.Bookmarks.Count
                    strBookMarkName = CStr(ActDocument.Bookmarks(i).Name)
                    If (InStr(strBookMarkName.ToLower.Trim, strTextBookname1.ToLower.Trim) > 0) Or _
                            (InStr(strBookMarkName.ToLower.Trim, strTextBookname2.ToLower.Trim) > 0) Then
                        rnet_bookmark_count = rnet_bookmark_count + 1
                        strlocname = ActDocument.Bookmarks(i).Name
                        strlocvalue = ActDocument.Bookmarks(i).Range.Text
                        output(rnet_bookmark_count, 0) = ActDocument.Bookmarks(i).Name
                        'Convert to RTF if the flag is set to true.
                        If ComponentiserConstants.TextConverToRTF AndAlso CStr(strlocvalue).Trim <> "" Then
                            Try
                                ''Convert text into rich text 
                                ActDocument.Bookmarks(i).Range.CopyAsPicture()
                                data = Clipboard.GetDataObject()
                                If Not data Is Nothing AndAlso _
                                    data.GetDataPresent("Rich Text Format") Then
                                    strlocvalue = data.GetData("Rich Text Format", True)
                                    strlocvalue = format_rtf(strlocvalue)
                                Else
                                    strlocvalue = ""
                                End If
                                output(rnet_bookmark_count, 1) = strlocvalue
                                'set data is nothing
                                data = Nothing
                            Catch ex As Exception
                                Me.Result = False
                                strlocvalue = ""
                                If ComponentiserConstants.ShowErrorMessage Then
                                    Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
                                Else
                                    Dim otrace3 As New bc_cs_activity_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_activity_codes.COMMENTARY, ex.Message)
                                End If
                            End Try
                        ElseIf Not strlocvalue Is Nothing AndAlso strlocvalue.Trim <> "" Then
                            strActual = ComponentiserConstants.RemoveSpecialChars(strlocvalue)
                            If Not strActual Is Nothing AndAlso strActual <> "" Then
                                output(rnet_bookmark_count, 1) = strActual
                            Else
                                output(rnet_bookmark_count, 1) = ""
                            End If
                        End If
                    End If
                Next
            ElseIf objconf.ComponentizeBy.ToLower.Trim = bc_cs_configure.strPrefixorSuffix.ToLower.Trim Then
                For i = 1 To ActDocument.Bookmarks.Count
                    strBookMarkName = CStr(ActDocument.Bookmarks(i).Name)
                    If (Left(strBookMarkName.ToLower.Trim, strTextBookname1.Length) = strTextBookname1.ToLower.Trim) Or _
                                               (Right(strBookMarkName.ToLower.Trim, strTextBookname2.Length) = strTextBookname2.ToLower.Trim) Then
                        rnet_bookmark_count = rnet_bookmark_count + 1
                        strlocname = ActDocument.Bookmarks(i).Name
                        strlocvalue = ActDocument.Bookmarks(i).Range.Text
                        output(rnet_bookmark_count, 0) = ActDocument.Bookmarks(i).Name
                        'Convert to RTF if the flag is set to true.
                        If ComponentiserConstants.TextConverToRTF AndAlso CStr(strlocvalue).Trim <> "" Then
                            Try
                                ''Convert text into rich text 
                                ActDocument.Bookmarks(i).Range.CopyAsPicture()
                                data = Clipboard.GetDataObject()
                                If Not data Is Nothing AndAlso _
                                    data.GetDataPresent("Rich Text Format") Then
                                    strlocvalue = data.GetData("Rich Text Format", True)
                                Else
                                    strlocvalue = ""
                                End If
                                output(rnet_bookmark_count, 1) = strlocvalue
                                'set data is nothing
                                data = Nothing
                            Catch ex As Exception
                                Me.Result = False
                                strlocvalue = ""
                                If ComponentiserConstants.ShowErrorMessage Then
                                    Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
                                Else
                                    Dim otrace3 As New bc_cs_activity_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_activity_codes.COMMENTARY, ex.Message)
                                End If
                            End Try
                        ElseIf Not strlocvalue Is Nothing AndAlso strlocvalue.Trim <> "" Then
                            strActual = ComponentiserConstants.RemoveSpecialChars(strlocvalue)
                            If Not strActual Is Nothing AndAlso strActual <> "" Then
                                output(rnet_bookmark_count, 1) = strActual
                            Else
                                output(rnet_bookmark_count, 1) = ""
                            End If
                        End If
                    End If
                Next
            End If
            Me.TextCount = rnet_bookmark_count
            ' MsgBox(rnet_bookmark_count)
            Me.ArrayTextData = output
            get_single_cell_components = output
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_single_cell_components", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "get_single_cell_components", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
            get_single_cell_components = Nothing
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "get_single_cell_components", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    ' This function set's the image properties  values(ImageCount,ArrayImageData).
    Public Function get_image_components() As Object
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "get_image_components", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim imageoutput(500, 2) As String
            Dim m As Integer
            Dim strImageBookname1 As String = objconf.ComponentizeImageString1
            Dim strImageBookname2 As String = objconf.ComponentizeImageString2
            'if the table option values are null then assign with some dummy string to avoid infinite loop
            If strImageBookname1.Trim = "" Then
                strImageBookname1 = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ"
            End If
            If strImageBookname2.Trim = "" Then
                strImageBookname2 = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ"
            End If
            ' slog = New bc_cs_activity_log("bc_ao_componetize_word", "componentize by--" & objconf.ComponentizeBy, bc_cs_activity_codes.COMMENTARY, "")
            ' slog = New bc_cs_activity_log("bc_ao_componetize_word", "strImageBookname1--" & strImageBookname1, bc_cs_activity_codes.COMMENTARY, "")
            '  slog = New bc_cs_activity_log("bc_ao_componetize_word", "strImageBookname2--" & strImageBookname2, bc_cs_activity_codes.COMMENTARY, "")

            Dim strBookMarkName As String
            Dim rnetrtg_bookmark_count As Integer

            'Used Alte Binding for Microsoft.Office.Interop.Word
            'Dim oParagraghundef As Microsoft.Office.Interop.Word.Paragraph
            Dim oParagraghundef As Object
            'oParagraghundef = CreateObject("Microsoft.Office.Interop.Word.Paragraph")

            Dim intUndefImageCount As Integer = 0
            Dim strUndefImageName As String
            Dim intUndefOrder As Integer = 0
            rnetrtg_bookmark_count = 0
            'Collect defined images
            '  blnInlineImageexists = False
            If objconf.ComponentizeBy.ToLower.Trim = bc_cs_configure.strAnyWhere.ToLower.Trim Then
                For m = 1 To ActDocument.Bookmarks.Count
                    strBookMarkName = CStr(ActDocument.Bookmarks(m).Name)
                    If (InStr(strBookMarkName.ToLower.Trim, strImageBookname1.ToLower.Trim) > 0) Or _
                            (InStr(strBookMarkName.ToLower.Trim, strImageBookname2.ToLower.Trim) > 0) Then

                        'Used late binding
                        'Dim oRange As Microsoft.Office.Interop.Word.Range
                        'Dim oRange = CreateObject("Microsoft.Office.Interop.Word.Range")
                        Dim oRange As Object
                        oRange = ActDocument.Bookmarks(m).Range
                        'get the image only if its not in table....as we are getting images from tables separately
                        If ActDocument.Bookmarks(m).Range.Tables.Count = 0 Then
                            Me.FindinlineImages(oRange, imageoutput, rnetrtg_bookmark_count, m)
                        End If
                    End If
                Next
            ElseIf objconf.ComponentizeBy.ToLower.Trim = bc_cs_configure.strPrefixorSuffix.ToLower.Trim Then
                For m = 1 To ActDocument.Bookmarks.Count
                    strBookMarkName = CStr(ActDocument.Bookmarks(m).Name)
                    If (Left(strBookMarkName.ToLower.Trim, strImageBookname1.Length) = strImageBookname1.ToLower.Trim) Or _
                            (Right(strBookMarkName.ToLower.Trim, strImageBookname2.Length) = strImageBookname2.ToLower.Trim) Then

                        'Used late binding
                        'Dim oRange As Microsoft.Office.Interop.Word.Range
                        'Dim oRange = CreateObject("Microsoft.Office.Interop.Word.Range")
                        Dim oRange As Object
                        oRange = ActDocument.Bookmarks(m).Range
                        'get the image only if its not in table....as we are getting images from tables separately
                        If ActDocument.Bookmarks(m).Range.Tables.Count = 0 Then
                            Me.FindinlineImages(oRange, imageoutput, rnetrtg_bookmark_count, m)
                        End If
                    End If
                Next
            End If
            ' Collect undefined images too
            For Each oParagraghundef In ActDocument.StoryRanges(1).Paragraphs 'Subscript 1 = Microsoft.Office.Interop.Word.WdStoryType.wdMainTextStory
                intUndefOrder = intUndefOrder + 1
                ' Loop through each paragraphs and collect  images only
                If oParagraghundef.Range.InlineShapes.Count <> 0 AndAlso _
                    oParagraghundef.Range.Tables.Count = 0 Then
                    'Then this is a image
                    strUndefImageName = "Undefined_Image_" & intUndefOrder
                    oParagraghundef.Range.Bookmarks.Add(strUndefImageName, oParagraghundef.Range)
                    Me.FindinlineImages(oParagraghundef.Range, imageoutput, rnetrtg_bookmark_count, _
                                strUndefImageName)
                End If
            Next
            Me.ImageCount = rnetrtg_bookmark_count
            Me.ArrayImageData = imageoutput
            get_image_components = imageoutput
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_image_components", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "get_image_components", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
            Return Nothing
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "get_image_components", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    'This functions finds the image in a given bookmark range and saves the image to harddisk 
    'and returns the saved file name.
    Public Function get_image_components(ByVal strLocator As String) As String
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "get_image_components", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            'User late binding
            'Dim oRange As Microsoft.Office.Interop.Word.Range
            'Dim oRange = CreateObject("Microsoft.Office.Interop.Word.Range")
            Dim oRange As Object
            oRange = ActDocument.Bookmarks(strLocator).Range
            'get the image only if its not in table....as we are getting images from tables separately
            get_image_components = Me.FindinlineImages(oRange)
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_image_components", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "get_image_components", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
            get_image_components = Nothing
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "get_image_components", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    'This functions finds all the images in a given range 
    'and saves the images to harddisk and returns array of saved file names with corresponding bookmark names.
    Public Sub FindinlineImages(ByRef oRange As Object, _
                ByRef imageoutput(,) As String, ByRef rnetrtg_bookmark_count As Integer, ByVal m As String)
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "FindinlineImages-for given range", bc_cs_activity_codes.TRACE_ENTRY, "")

        'Used Late Binding
        'Dim InlineShape As Microsoft.Office.Interop.Word.InlineShape
        Dim InlineShape As Object

        'Dim InlineShape = CreateObject("Microsoft.Office.Interop.Word.InlineShape")

        Dim strSubFileName As String = ""
        Dim blnInlineImageexists As Boolean
        Try
            '' // Iterate through all inline shapes in the document
            For Each InlineShape In oRange.InlineShapes
                InlineShape.Select()
                InlineShape.Range.CopyAsPicture()
                blnInlineImageexists = True
                strSubFileName = Me.SaveImage(rnetrtg_bookmark_count)
                rnetrtg_bookmark_count = rnetrtg_bookmark_count + 1
                If ActDocument.Bookmarks.Exists(m) AndAlso Not CStr(ActDocument.Bookmarks(m).Name) Is Nothing Then
                    imageoutput(rnetrtg_bookmark_count, 0) = ActDocument.Bookmarks(m).Name
                    imageoutput(rnetrtg_bookmark_count, 1) = strSubFileName
                End If
            Next
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "FindinlineImages", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "FindinlineImages", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "FindinlineImages", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    'This functions finds the image in a given range and saves the image to harddisk and returns the saved file name.
    Public Function FindinlineImages(ByVal oRange As Object) As String

        'Used late binding
        'Dim InlineShape As Microsoft.Office.Interop.Word.InlineShape
        'Dim InlineShape = CreateObject("Microsoft.Office.Interop.Word.InlineShape")
        Dim InlineShape As Object
        Dim strSubFileName As String = ""
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "FindinlineImages-for given range", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            '' // Iterate through all inline shapes in the document
            For Each InlineShape In oRange.InlineShapes
                InlineShape.Select()
                InlineShape.Range.CopyAsPicture()
                Me.ImageFileCount = Me.ImageFileCount + 1
                strSubFileName = Me.SaveImage(Me.ImageFileCount)
            Next
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "FindinlineImages", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "FindinlineImages", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
            strSubFileName = ""
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "FindinlineImages", bc_cs_activity_codes.TRACE_EXIT, "")
            FindinlineImages = strSubFileName
        End Try
    End Function
  
    'This functions finds all the images in a given range 
    'and saves the images to harddisk and returns array of saved file names with corresponding bookmark names.
    Public Sub FindinlineImagesByADO(ByRef oRange As Object, _
                ByRef imageoutput(,) As String, ByRef rnetrtg_bookmark_count As Integer, ByVal m As String)
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "FindinlineImages-for given range", bc_cs_activity_codes.TRACE_ENTRY, "")

        'Used late binding
        'Dim InlineShape As Microsoft.Office.Interop.Word.InlineShape
        Dim InlineShape As Object

        'Dim InlineShape = CreateObject("Microsoft.Office.Interop.Word.InlineShape")

        Dim blnInlineImageexists As Boolean
        Dim ImageStream As Object
        Dim filename As String
        Dim strdate As String
        Dim strSubFileName As String = ""
        Try
            '' // Iterate through all inline shapes in the document
            For Each InlineShape In oRange.InlineShapes
                ' InlineShape.Select()
                ' InlineShape.Range.CopyAsPicture()
                ImageStream = CreateObject("ADODB.Stream")
                strdate = CStr(DateAdd(DateInterval.Second, rnetrtg_bookmark_count, System.DateTime.Now))
                strdate = strdate.Replace("/", "-")
                strdate = strdate.Replace(" ", "-")
                strdate = strdate.Replace(":", ".")
                strSubFileName = "Image-" & strdate & ".bmp"
                filename = bc_cs_central_settings.local_repos_path & strSubFileName
                With ImageStream
                    .Type = 1 ' adTypeBinary
                    .Open()
                    .Write(InlineShape.Range.EnhMetaFileBits)
                    .SaveToFile(filename)
                    .Close()
                End With
                ImageStream = Nothing
                blnInlineImageexists = True
                rnetrtg_bookmark_count = rnetrtg_bookmark_count + 1
                If ActDocument.Bookmarks.Exists(m) AndAlso Not CStr(ActDocument.Bookmarks(m).Name) Is Nothing Then
                    imageoutput(rnetrtg_bookmark_count, 0) = ActDocument.Bookmarks(m).Name
                    imageoutput(rnetrtg_bookmark_count, 1) = strSubFileName
                End If
            Next
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "FindinlineImages", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "FindinlineImages", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "FindinlineImages", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    'This functions finds the image in a given range and saves the image to harddisk and returns the saved file name.
    Public Function FindinlineImagesByADO(ByRef oRange As Object) As String

        'Used late binding
        'Dim InlineShape As Microsoft.Office.Interop.Word.InlineShape
        Dim InlineShape As Object

        'Dim InlineShape = CreateObject("Microsoft.Office.Interop.Word.InlineShape")

        Dim ImageStream As Object
        Dim filename As String
        Dim strdate As String
        Dim strSubFileName As String = ""
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "FindinlineImages-for given range", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            '' // Iterate through all inline shapes in the document
            For Each InlineShape In oRange.InlineShapes
                ' InlineShape.Select()
                'InlineShape.Range.CopyAsPicture()
                Me.ImageFileCount = Me.ImageFileCount + 1
                ImageStream = CreateObject("ADODB.Stream")
                strdate = CStr(DateAdd(DateInterval.Second, Me.ImageFileCount, System.DateTime.Now))
                strdate = strdate.Replace("/", "-")
                strdate = strdate.Replace(" ", "-")
                strdate = strdate.Replace(":", ".")
                strSubFileName = "Image-" & strdate & ".bmp"
                filename = bc_cs_central_settings.local_repos_path & strSubFileName
                With ImageStream
                    .Type = 1 ' adTypeBinary
                    .Open()
                    .Write(InlineShape.Range.EnhMetaFileBits)
                    .SaveToFile(filename)
                    .Close()
                End With
                ImageStream = Nothing
            Next
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "FindinlineImages", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "FindinlineImages", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
            strSubFileName = ""
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "FindinlineImages", bc_cs_activity_codes.TRACE_EXIT, "")
            FindinlineImagesByADO = strSubFileName
        End Try
    End Function
    'This function saves the image existing in clipboard and returns the saved file name.
    Public Function SaveImage(ByVal intCount As Integer) As String
        Dim bmp As System.Drawing.Bitmap
        Dim data As IDataObject
        Dim strdate As String
        Dim filename As String
        Dim strSubFileName As String = ""
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "SaveImage-for given range", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            data = Clipboard.GetDataObject()
            If Not data Is Nothing AndAlso _
                               data.GetDataPresent(DataFormats.Bitmap) Then
                bmp = CType(data.GetData(GetType(System.Drawing.Bitmap)), System.Drawing.Bitmap)
                strdate = CStr(DateAdd(DateInterval.Second, intCount, System.DateTime.Now))
                strdate = strdate.Replace("/", "-")
                strdate = strdate.Replace(" ", "-")
                strdate = strdate.Replace(":", ".")
                strSubFileName = "Image-" & strdate & ".bmp"
                filename = bc_cs_central_settings.local_repos_path & strSubFileName
                bmp.Save(filename)
            End If
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "SaveImage", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "SaveImage", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
            strSubFileName = ""
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "SaveImage-for given range", bc_cs_activity_codes.TRACE_ENTRY, "")
            SaveImage = strSubFileName
        End Try
    End Function
    Public Function get_table_position_info(ByVal locator As String) As String

        Dim strCellStyle As String = ""

        'Used late binding
        'Dim oRange As Microsoft.Office.Interop.Word.Range
        Dim oRange As Object

        'Dim oRange = CreateObject("Microsoft.Office.Interop.Word.Range")


        Dim strRelHor As String
        Dim strRelVer As String
        Dim intRelHor As Integer
        Dim intRelVer As Integer
        Dim intPageNo As Integer

        'Used late binding
        'Dim oCell As Microsoft.Office.Interop.Word.Cell
        Dim oCell As Object

        'Dim oCell = CreateObject("Microsoft.Office.Interop.Word.Cell")

        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "SaveImage-for given range", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            If ActDocument.Bookmarks(locator).Range.Tables.Count > 0 Then
                oRange = ActDocument.Bookmarks(locator).Range.Tables(1).Range
                oCell = oRange.Cells(1)
                intPageNo = oCell.Range.Information(3) 'Subscript 3 = WdInformation.wdActiveEndPageNumber
                strCellStyle = "PageNo=" & intPageNo & ","
                strCellStyle = strCellStyle & "HorzPos=" & ActDocument.Bookmarks(locator).Range.Tables(1).Range.Information(5) & "," '5 = WdInformation.wdHorizontalPositionRelativeToPage
                strCellStyle = strCellStyle & "VertPos=" & ActDocument.Bookmarks(locator).Range.Tables(1).Range.Information(6) & "," '6 = WdInformation.wdVerticalPositionRelativeToPage
                '  intRelHor = oRange.Tables(1).Rows.RelativeHorizontalPosition
                Select Case intRelHor
                    Case 0
                        strRelHor = "Margin"
                    Case 1
                        strRelHor = "Page"
                    Case 2
                        strRelHor = "Column"
                    Case 3
                        strRelHor = "Character"
                End Select
                strCellStyle = strCellStyle & "RelHorzPos=Page" & ","
                ' intRelVer = oRange.Tables(1).Rows.RelativeVerticalPosition
                Select Case intRelVer
                    Case 0
                        strRelVer = "Margin"
                    Case 1
                        strRelVer = "Page"
                    Case 2
                        strRelVer = "Paragraph"
                    Case 3
                        strRelVer = "Line"
                End Select
                strCellStyle = strCellStyle & "RelVertPos=Page" & ","
                strCellStyle = strCellStyle & "DistLeft=" & oRange.Tables(1).Rows.DistanceLeft & ","
                strCellStyle = strCellStyle & "DistRight=" & oRange.Tables(1).Rows.DistanceRight & ","
                strCellStyle = strCellStyle & "DistTop=" & oRange.Tables(1).Rows.DistanceTop & ","
                strCellStyle = strCellStyle & "DistBottom=" & oRange.Tables(1).Rows.DistanceBottom & ","
            End If

        Catch ex As Exception
            Me.Result = False
            strCellStyle = ""
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "SaveImage", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "SaveImage", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If

        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "SaveImage-for given range", bc_cs_activity_codes.TRACE_ENTRY, "")
            get_table_position_info = strCellStyle
        End Try
    End Function

    ' This function set's the table properties values(TableCount,TablesInfoArray,TablesDataArray).
    Public Sub get_table_components()
        If ComponentiserConstants.LogActivityFile Then Dim slog = New bc_cs_activity_log("bc_ao_componetize_word", "get_table_components", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim i As Integer
        'Dim oRangeundef As Microsoft.Office.Interop.Word.Range = Nothing
        Dim oRangeundef As Object

        'Dim oRangeundef = CreateObject("Microsoft.Office.Interop.Word.Range")
        oRangeundef = Nothing

        'Dim oCheckRangeundef As Microsoft.Office.Interop.Word.Range
        Dim oCheckRangeundef As Object
        'Dim oCheckRangeundef = CreateObject("Microsoft.Office.Interop.Word.Range")

        'Dim oParagraghundef As Microsoft.Office.Interop.Word.Paragraph
        Dim oParagraghundef As Object
        'Dim oParagraghundef = CreateObject("Microsoft.Office.Interop.Word.Paragraph")

        Dim strUndefTableName As String
        Dim strUndefTableNametobe As String
        Dim intUndefOrder As Integer = 0
        Dim intUndefTablesCount As Integer = 0
        Me.TableCount = 0
        Dim intUndefActualTableCount As Integer = 0
        Dim strTableBookname1 As String = objconf.ComponentizeTableString1
        Dim strTableBookname2 As String = objconf.ComponentizeTableString2
        'if the table option values are null then assign with some dummy string to avoid infinite loop
        If strTableBookname1.Trim = "" Then
            strTableBookname1 = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ"
        End If
        If strTableBookname2.Trim = "" Then
            strTableBookname2 = "ZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZZ"
        End If
        Try
            'Collect defined tables information
            If objconf.ComponentizeBy.ToLower.Trim = bc_cs_configure.strAnyWhere.ToLower.Trim Then
                For i = 1 To ActDocument.Bookmarks.Count
                    If InStr(CStr(ActDocument.Bookmarks(i).Name).ToLower.Trim, strTableBookname1.ToLower.Trim) > 0 OrElse _
                       InStr(CStr(ActDocument.Bookmarks(i).Name).ToLower.Trim, strTableBookname2.ToLower.Trim) > 0 Then
                        Me.TableCount = Me.TableCount + 1
                        Me.new_get_table_cell_values(ActDocument.Bookmarks(i).Name, Me.TableCount)

                        'Me.get_table_cell_values(ActDocument.Bookmarks(i).Name, Me.TableCount)
                    End If
                Next
            ElseIf objconf.ComponentizeBy.ToLower.Trim = bc_cs_configure.strPrefixorSuffix.ToLower.Trim Then
                For i = 1 To ActDocument.Bookmarks.Count
                    If Left(CStr(ActDocument.Bookmarks(i).Name).ToLower.Trim, strTableBookname1.Length) = strTableBookname1.ToLower.Trim OrElse _
                            Right(CStr(ActDocument.Bookmarks(i).Name).ToLower.Trim, strTableBookname2.Length) = strTableBookname2.ToLower.Trim Then
                        Me.TableCount = Me.TableCount + 1
                        Me.new_get_table_cell_values(ActDocument.Bookmarks(i).Name, Me.TableCount)

                        'Me.get_table_cell_values(ActDocument.Bookmarks(i).Name, Me.TableCount)
                    End If
                Next
            End If
            ' set the property UndefTableData to know the undefined tables count and their rows and columns  
            intUndefTablesCount = ActDocument.StoryRanges(1).Tables.Count '1 = Microsoft.Office.Interop.Word.WdStoryType.wdMainTextStory
            Dim undefImplicationBookmarkName As String = ""
            Dim intCheckTableCount As Integer = 0
            Dim blnTableBreak As Boolean = True
            'Get the undefined table implications bookmark name...
            undefImplicationBookmarkName = objconf.read_implications_bookmark_name()
            For Each oParagraghundef In ActDocument.StoryRanges(1).Paragraphs '1 = Microsoft.Office.Interop.Word.WdStoryType.wdMainTextStory
                intUndefOrder = intUndefOrder + 1
                '  oParagraghundef.Range.Select()
                If oParagraghundef.Range.Tables.Count <> 0 Then
                    ' increase the check count to get the range of the new undefined table
                    If blnTableBreak = True AndAlso intCheckTableCount < intUndefTablesCount Then
                        intCheckTableCount = intCheckTableCount + 1
                    End If
                    blnTableBreak = False
                    'intinsidetablecount = intinsidetablecount + 1
                    If intUndefActualTableCount < intUndefTablesCount Then
                        'findout whether we have already componentised this table or not.
                        oCheckRangeundef = ActDocument.StoryRanges(1).Tables(intCheckTableCount).Range() ' 1= Microsoft.Office.Interop.Word.WdStoryType.wdMainTextStory
                        If intUndefActualTableCount = 0 OrElse _
                                (Not oRangeundef Is Nothing AndAlso Not oCheckRangeundef.IsEqual(oRangeundef)) Then
                            intUndefActualTableCount = intUndefActualTableCount + 1
                            oRangeundef = ActDocument.StoryRanges(1).Tables(intUndefActualTableCount).Range() ' 1=Microsoft.Office.Interop.Word.WdStoryType.wdMainTextStory
                            strUndefTableNametobe = "Undefined_Table_" & intUndefOrder
                            'findout if the items of the table has "invInsert" bookmark name or not
                            Dim intImpBookCount As Integer
                            Dim blnInvImplicationsNameExists As Boolean = False
                            For intImpBookCount = 1 To oRangeundef.Bookmarks.Count
                                If oRangeundef.Bookmarks(intImpBookCount).Name.ToLower.Trim = undefImplicationBookmarkName.ToLower.Trim Then
                                    blnInvImplicationsNameExists = True
                                    Exit For
                                End If
                            Next
                            'If the name of the table bookmark is "invInsert" then leave the name as it is..
                            If blnInvImplicationsNameExists Then
                                strUndefTableName = undefImplicationBookmarkName.ToLower.Trim
                            Else
                                strUndefTableName = strUndefTableNametobe
                                oRangeundef.Bookmarks.Add(strUndefTableNametobe, oRangeundef)
                            End If
                            Me.TableCount = Me.TableCount + 1
                            Me.new_get_table_cell_values(strUndefTableName, Me.TableCount)

                            'Me.get_table_cell_values(strUndefTableName, Me.TableCount)
                            ' intinsidetablecount = 0
                        End If
                    End If
                Else
                    blnTableBreak = True
                End If
            Next

        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_table_components", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "get_table_components", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If

        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "get_table_components", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    ' This function updates the table properties values(TableCount,TablesInfoArray,TablesDataArray)
    '' with the given table(locator) information.
    'Public Sub ffget_table_cell_values(ByVal locator As String, ByVal TableCount As Object)
    '    If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "get_table_cell_values -- " & locator, bc_cs_activity_codes.TRACE_ENTRY, "")
    '    Try
    '        Dim data As IDataObject
    '        Dim CellWidth As Decimal
    '        Dim CellHeight As Decimal
    '        Dim strCellStyle As String = ""
    '        REM select table
    '        ActDocument.Bookmarks(locator).Select()

    '        'Dim oRange As Microsoft.Office.Interop.Word.Range
    '        Dim oRange As Object
    '        'Dim oRange = CreateObject("Microsoft.Office.Interop.Word.Range")

    '        Dim strFilename As String = ""

    '        'Dim oCell As Microsoft.Office.Interop.Word.Cell
    '        Dim oCell As Object
    '        'Dim oCell = CreateObject("Microsoft.Office.Interop.Word.Cell")

    '        Dim strCellValue As String
    '        Dim strMainText As String = ""
    '        Dim strParagraphText As String = ""
    '        Dim strActualValue As String
    '        Dim strRGBValue As String = ""
    '        If ActDocument.Bookmarks(locator).Range.Tables.Count > 0 Then
    '            oRange = ActDocument.Bookmarks(locator).Range.Tables(1).Range
    '            Dim TableRowsCount As Object
    '            TableRowsCount = 0
    '            Dim TableColumnsCount As Object
    '            TableColumnsCount = 0
    '            Dim x As Integer
    '            Dim y As Integer
    '            Dim intRowCount As Integer = 1
    '            Dim intColNumber As Integer = 0
    '            TableRowsCount = ActDocument.Application.Selection.Tables(1).Rows.Count
    '            TableColumnsCount = ActDocument.Application.Selection.Tables(1).Columns.Count
    '            If oRange.Tables.Count > 0 Then
    '                For Each oCell In oRange.Cells
    '                    x = oCell.Range.Information(14) ' 14=Microsoft.Office.Interop.Word.WdInformation.wdEndOfRangeRowNumber
    '                    If intRowCount <> x Then
    '                        intRowCount = x
    '                        intColNumber = 0
    '                    End If
    '                    intColNumber = intColNumber + 1
    '                    y = intColNumber
    '                    TablesDataCount = TablesDataCount + 1
    '                    TablesDataArray(TablesDataCount, 0) = locator
    '                    TablesDataArray(TablesDataCount, 1) = x
    '                    TablesDataArray(TablesDataCount, 2) = y
    '                    'As there would be either image or text or acii we would store single field only in one cell
    '                    'If there is an image then store the file name otherwise store the cell value.
    '                    ' oCell.Range.Font.Name = "Arial"
    '                    strCellValue = oCell.Range.Text
    '                    If Len(strCellValue) > 2 Then
    '                        strCellValue = Left(strCellValue, Len(strCellValue) - 2)
    '                    End If
    '                    strFilename = Me.FindinlineImages(oCell.Range)
    '                    If strFilename <> "" Then
    '                        TablesDataArray(TablesDataCount, 3) = strFilename
    '                    Else
    '                        If ComponentiserConstants.TableTextConverToRTF Then
    '                            'the following code converts cell text into rich text
    '                            If CStr(strCellValue).Trim <> "" Then
    '                                Try
    '                                    'Convert text into rich text 
    '                                    oCell.Range.CopyAsPicture()
    '                                    data = Clipboard.GetDataObject()
    '                                    If Not data Is Nothing AndAlso _
    '                                        data.GetDataPresent("Rich Text Format") Then
    '                                        strParagraphText = data.GetData("Rich Text Format", True)
    '                                        TablesDataArray(TablesDataCount, 3) = strParagraphText.Trim
    '                                    Else
    '                                        TablesDataArray(TablesDataCount, 3) = ""
    '                                    End If
    '                                Catch ex As Exception
    '                                    Me.Result = False
    '                                    TablesDataArray(TablesDataCount, 3) = ""
    '                                    If ComponentiserConstants.ShowErrorMessage Then
    '                                        Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '                                    Else
    '                                        Dim otrace3 As New bc_cs_activity_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_activity_codes.COMMENTARY, ex.Message)
    '                                    End If
    '                                Finally
    '                                    'set data is nothing
    '                                    data = Nothing
    '                                    'set the rich text string value to null.
    '                                    strParagraphText = ""
    '                                End Try
    '                            Else
    '                                TablesDataArray(TablesDataCount, 3) = ""
    '                            End If
    '                        Else
    '                            'Capture plain text after replacing special charatcters.
    '                            If CStr(strCellValue) <> "" Then
    '                                strActualValue = ComponentiserConstants.RemoveSpecialChars(strCellValue.Trim)
    '                                If Not strActualValue Is Nothing AndAlso strActualValue <> "" Then
    '                                    TablesDataArray(TablesDataCount, 3) = strActualValue
    '                                Else
    '                                    TablesDataArray(TablesDataCount, 3) = ""
    '                                End If
    '                            Else
    '                                TablesDataArray(TablesDataCount, 3) = ""
    '                            End If
    '                        End If
    '                    End If
    '                    strRGBValue = ""
    '                    strCellStyle = ""
    '                    'capture the height and width information depending upon the xml flag.
    '                    If ComponentiserConstants.CaptureHeightWidthInfoofTable Then
    '                        'now fill the width and height information of this cell
    '                        CellWidth = oCell.Width
    '                        CellHeight = oCell.Height
    '                        strCellStyle = "width=" & CellWidth & ",height=" & CellHeight
    '                    End If
    '                    'capture the RGB information depending upon the xml flag.
    '                    If ComponentiserConstants.CaptureTableCellRGBValue Then
    '                        'now fill the RGB info of this cell
    '                        Try
    '                            If oCell.Range.Font.Color = RGB(ComponentiserConstants.RedR, _
    '                                                            ComponentiserConstants.RedG, _
    '                                                            ComponentiserConstants.RedB) Then
    '                                'Red arrow
    '                                strRGBValue = "RGB=" & ComponentiserConstants.RedR & ";" _
    '                                                        & ComponentiserConstants.RedG & ";" _
    '                                                        & ComponentiserConstants.RedB & ","

    '                            ElseIf oCell.Range.Font.Color = RGB(ComponentiserConstants.GreenR, _
    '                                                            ComponentiserConstants.GreenG, _
    '                                                            ComponentiserConstants.GreenB) Then
    '                                'Green arrow
    '                                strRGBValue = "RGB=" & ComponentiserConstants.GreenR & ";" _
    '                                                        & ComponentiserConstants.GreenG & ";" _
    '                                                        & ComponentiserConstants.GreenB & ","
    '                            End If

    '                        Catch ex As Exception
    '                            Me.Result = False
    '                            strRGBValue = ""
    '                            If ComponentiserConstants.ShowErrorMessage Then
    '                                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '                            Else
    '                                Dim otrace3 As New bc_cs_activity_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_activity_codes.COMMENTARY, ex.Message)
    '                            End If
    '                        End Try
    '                        strCellStyle = strCellStyle & "," & strRGBValue
    '                    End If
    '                    If strCellStyle <> "" Then
    '                        TablesDataArray(TablesDataCount, 4) = strCellStyle
    '                    Else
    '                        TablesDataArray(TablesDataCount, 4) = ""
    '                    End If
    '                Next
    '                TablesInfoArray(TableCount, 0) = locator
    '                TablesInfoArray(TableCount, 1) = TableRowsCount
    '                TablesInfoArray(TableCount, 2) = TableColumnsCount
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Me.Result = False
    '        If ComponentiserConstants.ShowErrorMessage Then
    '            Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '        Else
    '            Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_activity_codes.COMMENTARY, ex.Message)
    '        End If
    '    Finally
    '        If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try
    'End Sub
    'This function sets the table1 values of a ObjComp.TableData dataset
    ' with the exact cell values of a given table(locator) information.
    'Public Sub get_table_cell_values(ByRef ObjComp As bc_am_component, _
    '                      ByVal locator As String, ByVal TableCount As Object, _
    '                      ByVal strDisclosureMergedCell As String, ByVal strDisclosureHeightWidth As String, _
    '                      ByVal strDisclosureContainsImages As String)
    '    If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "get_disclosure_table", bc_cs_activity_codes.TRACE_ENTRY, "")
    '    Dim Tables_Data_Count As Integer = 0
    '    Dim oRows As DataRow
    '    Dim oTableData As DataTable = ObjComp.TableData.Tables(1)
    '    Dim data As IDataObject
    '    Dim CellWidth As Decimal
    '    Dim CellHeight As Decimal
    '    Dim strCellStyle As String = ""
    '    Dim strParagraphText As String = ""
    '    Dim strActualValue As String
    '    Dim strRGBValue As String = ""
    '    Try
    '        REM select table
    '        ' ActDocument.Bookmarks(locator).Select()
    '        'Dim oRange As Microsoft.Office.Interop.Word.Range
    '        Dim oRange As Object

    '        'Dim oRange = CreateObject("Microsoft.Office.Interop.Word.Range")

    '        Dim strFilename As String = ""

    '        'Dim oCell As Microsoft.Office.Interop.Word.Cell
    '        Dim oCell As Object

    '        'Dim oCell = CreateObject("Microsoft.Office.Interop.Word.Cell")

    '        Dim strCellValue As String
    '        If ActDocument.Bookmarks(locator).Range.Tables.Count > 0 Then
    '            oRange = ActDocument.Bookmarks(locator).Range.Tables(1).Range
    '            Dim x As Integer
    '            Dim y As Integer
    '            Dim intRowCount As Integer = 1
    '            Dim intColNumber As Integer = 0
    '            Dim intCurrentRow As Integer = 1
    '            Dim intCurrentCell As Integer = 0
    '            Dim intCellNumber As Integer = 0
    '            ObjComp.TableRowCount = oRange.Tables(1).Rows.Count
    '            ObjComp.TableColumnCount = oRange.Tables(1).Columns.Count
    '            If oRange.Tables.Count > 0 Then
    '                For Each oCell In oRange.Cells
    '                    oRows = oTableData.NewRow()
    '                    'findout whether table has merged cells or not.
    '                    'if a table has merged cells then only use information object
    '                    'otherwise go by table row count and column count properties.
    '                    If strDisclosureMergedCell.ToLower.Trim = "yes" Then
    '                        x = oCell.Range.Information(14) ' 14=Microsoft.Office.Interop.Word.WdInformation.wdEndOfRangeRowNumber
    '                        If intRowCount <> x Then
    '                            intRowCount = x
    '                            intColNumber = 0
    '                        End If
    '                        oRows(0) = x
    '                        intColNumber = intColNumber + 1
    '                        y = intColNumber
    '                        oRows(1) = y
    '                    Else
    '                        intCurrentCell = intCurrentCell + 1
    '                        If intCurrentCell = (ObjComp.TableColumnCount + 1) Then
    '                            intRowCount = intRowCount + 1
    '                            intColNumber = 0
    '                            intCurrentCell = 1
    '                        End If
    '                        oRows(0) = intRowCount
    '                        intColNumber = intColNumber + 1
    '                        y = intColNumber
    '                        oRows(1) = y
    '                    End If
    '                    strCellValue = oCell.Range.Text
    '                    If strDisclosureContainsImages.ToLower.Trim = "yes" Then
    '                        strFilename = Me.FindinlineImages(oCell.Range)
    '                    Else
    '                        strFilename = ""
    '                    End If

    '                    'If Len(strCellValue) > 2 Then
    '                    '    strCellValue = Left(oCell.Range.Text, _
    '                    '                        Len(oCell.Range.Text) - 2)
    '                    'End If
    '                    If Not strFilename Is Nothing AndAlso strFilename.Trim <> "" Then
    '                        oRows(2) = strFilename.Trim
    '                    Else
    '                        If ComponentiserConstants.TableTextConverToRTF Then
    '                            If CStr(strCellValue).Trim <> "" Then
    '                                Try
    '                                    ''Convert text into rich text 
    '                                    oCell.Range.CopyAsPicture()
    '                                    data = Clipboard.GetDataObject()
    '                                    If Not data Is Nothing AndAlso _
    '                                        data.GetDataPresent("Rich Text Format") Then
    '                                        strParagraphText = data.GetData("Rich Text Format", True)
    '                                    Else
    '                                        strParagraphText = ""
    '                                    End If
    '                                    oRows(2) = strParagraphText
    '                                    'set data is nothing
    '                                    data = Nothing
    '                                Catch ex As Exception
    '                                    Me.Result = False
    '                                    oRows(2) = ""
    '                                    If ComponentiserConstants.ShowErrorMessage Then
    '                                        Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '                                    Else
    '                                        Dim otrace3 As New bc_cs_activity_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_activity_codes.COMMENTARY, ex.Message)
    '                                    End If
    '                                End Try
    '                            Else
    '                                oRows(2) = ""
    '                            End If
    '                        Else
    '                            If CStr(strCellValue) <> "" Then
    '                                strActualValue = ComponentiserConstants.RemoveSpecialChars(strCellValue.Trim)
    '                                If Not strActualValue Is Nothing AndAlso strActualValue <> "" Then
    '                                    oRows(2) = strActualValue
    '                                Else
    '                                    oRows(2) = ""
    '                                End If
    '                            Else
    '                                oRows(2) = ""
    '                            End If
    '                        End If
    '                    End If
    '                    strRGBValue = ""
    '                    strCellStyle = ""
    '                    'capture the height and width information depending upon the xml flag.
    '                    If strDisclosureHeightWidth.ToLower.Trim = "yes" Then
    '                        'now fill the width and height information of this cell
    '                        CellWidth = oCell.Width
    '                        CellHeight = oCell.Height
    '                        strCellStyle = "width=" & CellWidth & ",height=" & CellHeight
    '                    End If
    '                    'capture the RGB information depending upon the xml flag.
    '                    If ComponentiserConstants.CaptureTableCellRGBValue Then
    '                        'now fill the RGB info of this cell
    '                        Try
    '                            If oCell.Range.Font.Color = RGB(ComponentiserConstants.RedR, _
    '                                                            ComponentiserConstants.RedG, _
    '                                                            ComponentiserConstants.RedB) Then
    '                                'Red arrow
    '                                strRGBValue = "RGB=" & ComponentiserConstants.RedR & ";" _
    '                                                        & ComponentiserConstants.RedG & ";" _
    '                                                        & ComponentiserConstants.RedB & ","

    '                            ElseIf oCell.Range.Font.Color = RGB(ComponentiserConstants.GreenR, _
    '                                                            ComponentiserConstants.GreenG, _
    '                                                            ComponentiserConstants.GreenB) Then
    '                                'Green arrow
    '                                strRGBValue = "RGB=" & ComponentiserConstants.GreenR & ";" _
    '                                                        & ComponentiserConstants.GreenG & ";" _
    '                                                        & ComponentiserConstants.GreenB & ","
    '                            End If
    '                        Catch ex As Exception
    '                            Me.Result = False
    '                            strRGBValue = ""
    '                            If ComponentiserConstants.ShowErrorMessage Then
    '                                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '                            Else
    '                                Dim otrace3 As New bc_cs_activity_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_activity_codes.COMMENTARY, ex.Message)
    '                            End If
    '                        End Try
    '                        strCellStyle = strCellStyle & "," & strRGBValue
    '                    End If
    '                    If strCellStyle <> "" Then
    '                        oRows(4) = strCellStyle
    '                    Else
    '                        oRows(4) = ""
    '                    End If
    '                    oTableData.Rows.Add(oRows)
    '                Next
    '            End If
    '        End If
    '    Catch ex As Exception
    '        Me.Result = False
    '        If ComponentiserConstants.ShowErrorMessage Then
    '            Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '        Else
    '            Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_activity_codes.COMMENTARY, ex.Message)
    '        End If
    '    Finally
    '        If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "get_disclosure_table", bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try
    'End Sub
    REM PR comment september 2009
    Public Sub new_get_table_cell_values(ByVal locator As String, ByVal TableCount As Object)
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "new_get_table_cell_values -- " & locator, bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim data As IDataObject = Nothing
            Dim CellWidth As Decimal
            Dim CellHeight As Decimal
            Dim strCellStyle As String = ""
            REM select table
            ActDocument.Bookmarks(locator).Select()
            Dim oRange As Object
            Dim strFilename As String = ""
            Dim oCell As Object
            Dim strCellValue As String
            Dim strMainText As String = ""
            Dim strParagraphText As String = ""
            Dim strActualValue As String
            Dim strRGBValue As String = ""
            If ActDocument.Bookmarks(locator).Range.Tables.Count > 0 Then
                oRange = ActDocument.Bookmarks(locator).Range.Tables(1).Range
                Dim TableRowsCount As Object
                TableRowsCount = 0
                Dim TableColumnsCount As Object
                TableColumnsCount = 0
                Dim x As Integer
                Dim y As Integer
                Dim intRowCount As Integer = 1
                Dim intColNumber As Integer = 0
                TableRowsCount = ActDocument.Application.Selection.Tables(1).Rows.Count
                TableColumnsCount = ActDocument.Application.Selection.Tables(1).Columns.Count
                If oRange.Tables.Count > 0 Then
                    For Each oCell In oRange.Cells
                        x = oCell.Range.Information(14)
                        If intRowCount <> x Then
                            intRowCount = x
                            intColNumber = 0
                        End If
                        intColNumber = intColNumber + 1
                        y = intColNumber
                        TablesDataCount = TablesDataCount + 1
                        TablesDataArray(TablesDataCount, 0) = locator
                        TablesDataArray(TablesDataCount, 1) = x
                        TablesDataArray(TablesDataCount, 2) = y
                        'As there would be either image or text or acii we would store single field only in one cell
                        'If there is an image then store the file name otherwise store the cell value.
                        ' oCell.Range.Font.Name = "Arial"
                        strCellValue = oCell.Range.Text
                        If Len(strCellValue) > 2 Then
                            strCellValue = Left(strCellValue, Len(strCellValue) - 2)
                        End If
                        strFilename = Me.FindinlineImages(oCell.Range)
                        If strFilename <> "" Then
                            TablesDataArray(TablesDataCount, 3) = strFilename
                        Else
                            If ComponentiserConstants.TableTextConverToRTF Then
                                'the following code converts cell text into rich text
                                If CStr(strCellValue).Trim <> "" Then
                                    Try
                                        'Convert text into rich text 
                                        Try
                                            oCell.Range.CopyAsPicture()
                                        Catch
                                            Dim ocomm = New bc_cs_activity_log("aaa", "RANGE FAILED:" + CStr(x) + ": " + CStr(y), bc_cs_activity_codes.COMMENTARY, "", Nothing)
                                        End Try
                                        Try
                                            data = Clipboard.GetDataObject()
                                        Catch
                                            Dim ocomm = New bc_cs_activity_log("aaa", "CLIPBOARD FAILED" + CStr(x) + ": " + CStr(y), bc_cs_activity_codes.COMMENTARY, "", Nothing)
                                        End Try
                                        If Not data Is Nothing AndAlso _
                                            data.GetDataPresent("Rich Text Format") Then
                                            Try
                                                strParagraphText = data.GetData("Rich Text Format", True)
                                                strParagraphText = format_rtf(strParagraphText)
                                                REM PROBLEM WAS HERE
                                                TablesDataArray(TablesDataCount, 3) = strParagraphText.Trim
                                            Catch ex As Exception
                                                Dim ocomm As New bc_cs_activity_log("aaa", "BLANK CELL: " + locator + ": " + CStr(x + 1) + ":" + CStr(y + 1), bc_cs_activity_codes.COMMENTARY, ex.Message, Nothing)
                                            End Try
                                        Else
                                            TablesDataArray(TablesDataCount, 3) = ""
                                        End If
                                    Catch ex As Exception
                                        Me.Result = False
                                        TablesDataArray(TablesDataCount, 3) = ""
                                        If ComponentiserConstants.ShowErrorMessage Then
                                            Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "3new_get_table_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
                                        Else
                                            Dim otrace3 As New bc_cs_activity_log("bc_ao_componetize_word", "3new_get_table_cell_values", bc_cs_activity_codes.COMMENTARY, ex.Message)
                                        End If
                                    Finally
                                        'set data is nothing
                                        data = Nothing
                                        'set the rich text string value to null.
                                        strParagraphText = ""
                                    End Try
                                Else
                                    TablesDataArray(TablesDataCount, 3) = ""
                                End If
                            Else
                                'Capture plain text after replacing special charatcters.
                                If CStr(strCellValue) <> "" Then
                                    strActualValue = ComponentiserConstants.RemoveSpecialChars(strCellValue.Trim)
                                    If Not strActualValue Is Nothing AndAlso strActualValue <> "" Then
                                        TablesDataArray(TablesDataCount, 3) = strActualValue
                                    Else
                                        TablesDataArray(TablesDataCount, 3) = ""
                                    End If
                                Else
                                    TablesDataArray(TablesDataCount, 3) = ""
                                End If
                            End If
                        End If
                        strRGBValue = ""
                        strCellStyle = ""
                        'capture the height and width information depending upon the xml flag.
                        If ComponentiserConstants.CaptureHeightWidthInfoofTable Then
                            'now fill the width and height information of this cell
                            CellWidth = oCell.Width
                            CellHeight = oCell.Height
                            strCellStyle = "width=" & CellWidth & ",height=" & CellHeight
                        End If
                        'capture the RGB information depending upon the xml flag.
                        If ComponentiserConstants.CaptureTableCellRGBValue Then
                            'now fill the RGB info of this cell
                            Try
                                If oCell.Range.Font.Color = RGB(ComponentiserConstants.RedR, _
                                                                ComponentiserConstants.RedG, _
                                                                ComponentiserConstants.RedB) Then
                                    'Red arrow
                                    strRGBValue = "RGB=" & ComponentiserConstants.RedR & ";" _
                                                            & ComponentiserConstants.RedG & ";" _
                                                            & ComponentiserConstants.RedB & ","

                                ElseIf oCell.Range.Font.Color = RGB(ComponentiserConstants.GreenR, _
                                                                ComponentiserConstants.GreenG, _
                                                                ComponentiserConstants.GreenB) Then
                                    'Green arrow
                                    strRGBValue = "RGB=" & ComponentiserConstants.GreenR & ";" _
                                                            & ComponentiserConstants.GreenG & ";" _
                                                            & ComponentiserConstants.GreenB & ","
                                End If

                            Catch ex As Exception
                                Me.Result = False
                                strRGBValue = ""
                                If ComponentiserConstants.ShowErrorMessage Then
                                    Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "4new_get_table_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
                                Else
                                    Dim otrace3 As New bc_cs_activity_log("bc_ao_componetize_word", "4new_get_table_cell_values", bc_cs_activity_codes.COMMENTARY, ex.Message)
                                End If
                            End Try
                            strCellStyle = strCellStyle & "," & strRGBValue
                        End If
                        If strCellStyle <> "" Then
                            TablesDataArray(TablesDataCount, 4) = strCellStyle
                        Else
                            TablesDataArray(TablesDataCount, 4) = ""
                        End If
                    Next
                    TablesInfoArray(TableCount, 0) = locator
                    TablesInfoArray(TableCount, 1) = TableRowsCount
                    TablesInfoArray(TableCount, 2) = TableColumnsCount
                End If
            End If
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "5new_get_table_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "5new_get_table_cell_values", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "new_get_table_cell_values", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub new_get_table_cell_values(ByRef ObjComp As bc_am_component, _
                       ByVal locator As String, ByVal TableCount As Object, _
                       ByVal strDisclosureMergedCell As String, ByVal strDisclosureHeightWidth As String, _
                       ByVal strDisclosureContainsImages As String)
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_ao_componetize_word", "new_get_table_cell_values", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim Tables_Data_Count As Integer = 0
        'Dim oRows As DataRow
        Dim oRows As bc_om_component_table_cell
        'Dim oTableData As DataTable = ObjComp.TableData.Tables(1)
        Dim oTableData As New ArrayList
        Dim data As IDataObject
        Dim CellWidth As Decimal
        Dim CellHeight As Decimal
        Dim strCellStyle As String = ""
        Dim strParagraphText As String = ""
        Dim strActualValue As String
        Dim strRGBValue As String = ""
        Try
            REM select table
            ' ActDocument.Bookmarks(locator).Select()
            'Dim oRange As Microsoft.Office.Interop.Word.Range
            'Dim oRange = CreateObject("Microsoft.Office.Interop.Word.Range")
            Dim oRange As Object

            Dim strFilename As String = ""

            'Dim oCell As Microsoft.Office.Interop.Word.Cell
            'Dim oCell = CreateObject("Microsoft.Office.Interop.Word.Cell")
            Dim oCell As Object

            Dim strCellValue As String
            If ActDocument.Bookmarks(locator).Range.Tables.Count > 0 Then
                oRange = ActDocument.Bookmarks(locator).Range.Tables(1).Range
                Dim x As Integer
                Dim y As Integer
                Dim intRowCount As Integer = 1
                Dim intColNumber As Integer = 0
                Dim intCurrentRow As Integer = 1
                Dim intCurrentCell As Integer = 0
                Dim intCellNumber As Integer = 0
                ObjComp.TableRowCount = oRange.Tables(1).Rows.Count
                ObjComp.TableColumnCount = oRange.Tables(1).Columns.Count
                If oRange.Tables.Count > 0 Then
                    For Each oCell In oRange.Cells
                        oRows = New bc_om_component_table_cell
                        'findout whether table has merged cells or not.
                        'if a table has merged cells then only use information object
                        'otherwise go by table row count and column count properties.
                        If strDisclosureMergedCell.ToLower.Trim = "yes" Then
                            x = oCell.Range.Information(14) 'Microsoft.Office.Interop.Word.WdInformation.wdEndOfRangeRowNumber
                            If intRowCount <> x Then
                                intRowCount = x
                                intColNumber = 0
                            End If
                            oRows.RowValue = x
                            intColNumber = intColNumber + 1
                            y = intColNumber
                            oRows.ColumnValue = y
                        Else
                            intCurrentCell = intCurrentCell + 1
                            If intCurrentCell = (ObjComp.TableColumnCount + 1) Then
                                intRowCount = intRowCount + 1
                                intColNumber = 0
                                intCurrentCell = 1
                            End If
                            oRows.RowValue = intRowCount
                            intColNumber = intColNumber + 1
                            y = intColNumber
                            oRows.ColumnValue = y
                        End If
                        strCellValue = oCell.Range.Text
                        If strDisclosureContainsImages.ToLower.Trim = "yes" Then
                            strFilename = Me.FindinlineImages(oCell.Range)
                        Else
                            strFilename = ""
                        End If

                        'If Len(strCellValue) > 2 Then
                        '    strCellValue = Left(oCell.Range.Text, _
                        '                        Len(oCell.Range.Text) - 2)
                        'End If
                        If Not strFilename Is Nothing AndAlso strFilename.Trim <> "" Then
                            oRows.Value = strFilename.Trim
                        Else
                            If ComponentiserConstants.TableTextConverToRTF Then
                                If CStr(strCellValue).Trim <> "" Then
                                    Try
                                        ''Convert text into rich text 
                                        oCell.Range.CopyAsPicture()
                                        data = Clipboard.GetDataObject()
                                        If Not data Is Nothing AndAlso _
                                            data.GetDataPresent("Rich Text Format") Then
                                            strParagraphText = data.GetData("Rich Text Format", True)

                                        Else
                                            strParagraphText = ""
                                        End If
                                        oRows.Value = strParagraphText
                                        'set data is nothing
                                        data = Nothing
                                    Catch ex As Exception
                                        Me.Result = False
                                        oRows.Value = ""
                                        If ComponentiserConstants.ShowErrorMessage Then
                                            Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "new_get_table_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
                                        Else
                                            Dim otrace3 As New bc_cs_activity_log("bc_ao_componetize_word", "new_get_table_cell_values", bc_cs_activity_codes.COMMENTARY, ex.Message)
                                        End If
                                    End Try
                                Else
                                    oRows.Value = ""
                                End If
                            Else
                                If CStr(strCellValue) <> "" Then
                                    strActualValue = ComponentiserConstants.RemoveSpecialChars(strCellValue.Trim)
                                    If Not strActualValue Is Nothing AndAlso strActualValue <> "" Then
                                        oRows.Value = strActualValue
                                    Else
                                        oRows.Value = ""
                                    End If
                                Else
                                    oRows.Value = ""
                                End If
                            End If
                        End If
                        strRGBValue = ""
                        strCellStyle = ""
                        'capture the height and width information depending upon the xml flag.
                        If strDisclosureHeightWidth.ToLower.Trim = "yes" Then
                            'now fill the width and height information of this cell
                            CellWidth = oCell.Width
                            CellHeight = oCell.Height
                            strCellStyle = "width=" & CellWidth & ",height=" & CellHeight
                        End If
                        'capture the RGB information depending upon the xml flag.
                        If ComponentiserConstants.CaptureTableCellRGBValue Then
                            'now fill the RGB info of this cell
                            Try
                                If oCell.Range.Font.Color = RGB(ComponentiserConstants.RedR, _
                                                                ComponentiserConstants.RedG, _
                                                                ComponentiserConstants.RedB) Then
                                    'Red arrow
                                    strRGBValue = "RGB=" & ComponentiserConstants.RedR & ";" _
                                                            & ComponentiserConstants.RedG & ";" _
                                                            & ComponentiserConstants.RedB & ","

                                ElseIf oCell.Range.Font.Color = RGB(ComponentiserConstants.GreenR, _
                                                                ComponentiserConstants.GreenG, _
                                                                ComponentiserConstants.GreenB) Then
                                    'Green arrow
                                    strRGBValue = "RGB=" & ComponentiserConstants.GreenR & ";" _
                                                            & ComponentiserConstants.GreenG & ";" _
                                                            & ComponentiserConstants.GreenB & ","
                                End If
                            Catch ex As Exception
                                Me.Result = False
                                strRGBValue = ""
                                If ComponentiserConstants.ShowErrorMessage Then
                                    Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
                                Else
                                    Dim otrace3 As New bc_cs_activity_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_activity_codes.COMMENTARY, ex.Message)
                                End If
                            End Try
                            strCellStyle = strCellStyle & "," & strRGBValue
                        End If
                        If strCellStyle <> "" Then
                            oRows.Style = strCellStyle
                        Else
                            oRows.Style = ""
                        End If
                        oTableData.Add(oRows)
                    Next
                End If
            End If
            ObjComp.newstructuretable = oTableData

        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_ao_componetize_word", "get_table_cell_values", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_ao_componetize_word", "get_disclosure_table", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

End Class

Public MustInherit Class bc_ao_componentize
    Public Overridable Function GetSingleCellValues() As Object
        Return Nothing
    End Function
    Public Overridable Function GetImageValues() As Object
        Return Nothing
    End Function
    Public Overridable Sub GetTableCellValues()

    End Sub
    Public Overridable Function GetDocumentProperty(ByVal PropertyName As String) As Object
        Return Nothing
    End Function
    Public Overridable Function GetBookMarkValue(ByVal BookMarkName As String) As Object
        Return Nothing
    End Function

End Class
