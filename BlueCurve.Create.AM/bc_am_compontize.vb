Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Exception
Imports System.Data

'This file is created by Rama Boya 
Public Class bc_am_at_componentize
    Public comp_obj As bc_ao_componetize_word
    Public intOrderCount As Integer = 0
    Public blnFinalResult As Boolean = True
    Public objconf As New bc_cs_configure
    Public bc_cs_central_settings As New bc_cs_central_settings(True)
    Public Property OrderCount() As Integer
        Get
            Return intOrderCount
        End Get
        Set(ByVal Value As Integer)
            intOrderCount = Value
        End Set
    End Property
    'THIS property is set to false when there is an error 
    Public Property Result() As Boolean
        Get
            Return blnFinalResult
        End Get
        Set(ByVal Value As Boolean)
            blnFinalResult = Value
        End Set
    End Property
    Private ObjComponentContainer As New ComponentContainer
    Public Function Componentise(ByVal show_form As Boolean, ByVal ao_object As Object, ByVal ao_type As String, ByVal doc_id As String) As Boolean
        Dim blnResult As Boolean = False
        Try
            If ao_type = "word" Then
                Me.ComponentObject = New bc_ao_componetize_word(ao_object)
                'Componentize the document only if there is no error in reading composite config file.
                If Me.ComponentObject.Result Then
                    blnResult = ComponentizeDocument(doc_id)
                End If
            Else
                Dim omessage As New bc_cs_message("Blue Curve create", "mime type: " + ao_type + " not supported!", bc_cs_message.MESSAGE)
            End If
        Catch ex As Exception
            blnResult = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_am_componetize", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace As New bc_cs_activity_log("bc_am_componetize", "new", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            Componentise = blnResult
            If ComponentiserConstants.LogActivityFile Then If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_am_componetize", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Sub New()

    End Sub
    Public Property ComponentObject() As bc_ao_componetize_word
        Get
            Return comp_obj
        End Get
        Set(ByVal Value As bc_ao_componetize_word)
            comp_obj = Value
        End Set
    End Property
    Public Property ComponentContainer() As ComponentContainer
        Get
            Return ObjComponentContainer
        End Get
        Set(ByVal Value As ComponentContainer)
            ObjComponentContainer = Value
        End Set
    End Property
    Public Function ComponentizeDocument(ByVal doc_id As String) As Boolean
        If ComponentiserConstants.LogActivityFile Then If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_am_componetize", "ComponentizeDocument", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim blnResult As Boolean
        Dim objCentralSettings As New bc_cs_central_settings(True)
        Dim blnReadConfigResult As Boolean
        Dim blnDBwriteResult As Boolean
        Try
            
            Dim intDisclosures As Integer
            'Dim doc_id As String
            Dim entity_id As String
            Dim strpubtypeid As String
            Dim strDisclosureAtt As String
            Dim strDisclosureName As String
            Dim strDisclosureMergedCell As String
            Dim strDisclosureHeightWidth As String
            Dim strDisclosureContainsImages As String
            Dim arrDisclosureName(200) As String
            Dim intDisclosureTablecount As Integer = 0
            Dim blnSearchFound As Boolean
            'Get the publication type id
            strpubtypeid = CStr(Me.ComponentObject.GetDocumentProperty("rnet_pub_type_id"))
            'Read the configuration file based on the publication type.
            blnReadConfigResult = objconf.read_ini_file(strpubtypeid)
            If Not blnReadConfigResult Then
                ' stop the execution if there is an error in reading composite configuration file
                MsgBox("Composite Config File Error. Please contact Administartor ", MsgBoxStyle.Critical, "Blue Curve")
                Exit Function
            End If

            'Get the documentname 
            'PR removed this as wont work before first submit 25-2-2008
            'doc_id = CStr(Me.ComponentObject.GetDocumentProperty("doc_id"))

            'if the pub type exists in the configuration file and is of "composite"
            'or "locatorsonly" then componentize specified locators only
            'Otherwise componentize all.
            If objconf.PubTypeExists AndAlso _
              (objconf.PubType.ToLower.Trim = "composite" OrElse objconf.PubType.ToLower.Trim = "locatorsonly") Then
                'This is composite document so componentize only given bookmarks.
                entity_id = Me.ComponentObject.GetDocumentProperty("entity_id")
                For intDisclosures = 1 To objconf.LocatorCount
                    strDisclosureAtt = objconf.ArrLocator(intDisclosures, 0)
                    strDisclosureName = objconf.ArrLocator(intDisclosures, 1)
                    strDisclosureMergedCell = objconf.ArrLocator(intDisclosures, 2)
                    strDisclosureHeightWidth = objconf.ArrLocator(intDisclosures, 3)
                    strDisclosureContainsImages = objconf.ArrLocator(intDisclosures, 4)
                    'Get the complete bookmark name 
                    'If the value has character "*"  at the end of disclosure name
                    'then get the array of similar bookmarks otherwise 
                    'get the single bookmark
                    Dim strCharSearch As String
                    If Right(strDisclosureName, 1) = "*" Then
                        strCharSearch = Left(strDisclosureName, Len(strDisclosureName) - 1)
                        'set the searchfound as false initially
                        blnSearchFound = False
                        blnSearchFound = Me.ComponentObject.GetBookMarkNameArray(strCharSearch)
                        Dim intDiscCount As Integer
                        'Componentise the document only if the search string is found
                        If blnSearchFound Then
                            For intDiscCount = 0 To Me.ComponentObject.VarcharMultiCountCount
                                strDisclosureName = Me.ComponentObject.ArrayVarcharMultiData(intDiscCount)
                                If strDisclosureName <> "" Then
                                    CompositeComponentize(strDisclosureAtt, strDisclosureName, _
                                        intDisclosureTablecount, strDisclosureMergedCell, _
                                        strDisclosureHeightWidth, strDisclosureContainsImages)
                                End If
                            Next
                            'reset the array values and count
                            Me.ComponentObject.VarcharMultiCountCount = 0
                        End If
                    Else
                        strDisclosureName = Me.ComponentObject.GetBookMarkName(strDisclosureName)
                        If strDisclosureName <> "" Then
                            CompositeComponentize(strDisclosureAtt, strDisclosureName, _
                                        intDisclosureTablecount, strDisclosureMergedCell, _
                                        strDisclosureHeightWidth, strDisclosureContainsImages)
                        End If
                    End If
                Next
            Else
                'This is unit document so componentize all.
                'Get the entityid
                entity_id = Me.ComponentObject.GetDocumentProperty("entity_id")
                'Get the single text values
                Me.SetSingleCellComponents()
                'Get the single text values with no bookmark ids.
                Me.SetSingleTextwithNoBookmark()
                'Get the implication values of the document if at all
                ' if there is any configuration section existsing for the invinsert
                If objconf.Componentise_InvInsert_Properties Then
                    Me.SetImplicationsImageValues()
                End If
                'get the main text value.
                Me.SetMainTextComponent()
                'Get the title of the document.
                Me.SetTitleComponent()
                'Get the Image values
                Me.SetImageComponents()
                'Get t he tables info
                Me.NewSetTableComponents()
            End If

            'Set the datefrom and To so that we can have the same for all the components against single document.
            Dim dtcurrent As Date = System.DateTime.Now
            Me.ComponentContainer.DateFrom = dtcurrent
            Me.ComponentContainer.DateTo = System.DateTime.Parse("9/9/9999")
            '  set the document id
            Me.ComponentContainer.DocumentId = doc_id
            Me.ComponentContainer.EntityId = entity_id
            Dim gbc_db As New bc_cs_db_services(False)
            ' This is the place to write the data into DB. 
            'If it is ado mode then use the following code directly otherwise use serializable soap services
            'just check connection type and make sure that you are able to use the connection.
            Dim blnConnectionOK As Boolean
            blnConnectionOK = objCentralSettings.check_connection(bc_cs_central_settings.connection_method, True)


            If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO AndAlso blnConnectionOK Then
                blnDBwriteResult = Me.ComponentContainer.db_write(Nothing)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP AndAlso blnConnectionOK Then
                Me.ComponentContainer.tmode = bc_cs_soap_base_class.tWRITE
                Me.ComponentContainer.transmit_to_server_and_receive(Me.ComponentContainer, True)
                'now get the result value
                blnDBwriteResult = Me.ComponentContainer.Result()
            Else
                blnDBwriteResult = False
                If ComponentiserConstants.ShowErrorMessage Then
                    Dim db_err As New bc_cs_error_log("bc_am_componetize", "ComponentizeDocument", bc_cs_error_codes.USER_DEFINED, "No suitable connection type exists")
                Else
                    Dim otrace1 As New bc_cs_activity_log("bc_am_componetize", "ComponentizeDocument", bc_cs_activity_codes.COMMENTARY, "No suitable connection type exists")
                End If
                REM local so report message
            End If
            If Not blnDBwriteResult Then
                Me.Result = False
            End If
            'set the result falg to success if there is no error in the componentisation process
            If Me.Result And Me.ComponentObject.Result Then
                blnResult = True
            Else
                blnResult = False
            End If

        Catch ex As Exception
            blnResult = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_am_componetize", "ComponentizeDocument", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_am_componetize", "ComponentizeDocument", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            ComponentizeDocument = blnResult
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_am_componetize", "ComponentizeDocument", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Function CompositeComponentize(ByVal strDisclosureAtt As String, ByVal strDisclosureName As String, _
                    ByRef intDisclosureTablecount As Integer, _
                    ByVal strDisclosureMergedCell As String, ByVal strDisclosureHeightWidth As String, _
                    ByVal strDisclosureContainsImages As String) As Boolean

        If CStr(strDisclosureAtt.ToLower.Trim) = "47" Then
            'this is a text
            'Get the bookmark value to componentize.
            Me.SetSingleCellComponents(strDisclosureName.Trim)
        ElseIf CStr(strDisclosureAtt.ToLower.Trim) = "48" Then
            'this is an image
            'Get the image to componentize.
            Me.SetImageComponents(strDisclosureName.Trim)
        ElseIf CStr(strDisclosureAtt.ToLower.Trim) = "49" Then
            'this is a table
            intDisclosureTablecount = intDisclosureTablecount + 1
            'Get the table  to componentize.
            Me.newSetTableComponents(strDisclosureName.Trim, intDisclosureTablecount, _
                       strDisclosureMergedCell, strDisclosureHeightWidth, strDisclosureContainsImages)
        End If
    End Function
    ' This function is used to componentize all single text values of a document.
    Public Function SetSingleCellComponents() As Boolean
        If ComponentiserConstants.LogActivityFile Then If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_am_componetize", "SetSingleCellComponents", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            'Get the single text values
            Dim ObjComp As bc_am_component
            Dim inti As Integer
            Dim strEntityid As String = ""
            Dim strCompid As Long
            Dim strsplitter As String
            Dim strLocator As String
            Me.ComponentObject.get_single_cell_components()
            'Instantiate component object with all single text values and add the same to components collection.
            For inti = 1 To Me.ComponentObject.TextCount
                ObjComp = New bc_am_component
                ObjComp.ComponentType = bc_am_component.CompType.Text
                ObjComp.Locator = Me.ComponentObject.ArrayTextData(inti, 0)
                Me.OrderCount = Me.OrderCount + 1
                ObjComp.OrderNo = Me.OrderCount
                strLocator = ObjComp.Locator
                'Get the entityid and component id from the locator.
                'rnet31_at358_en773
                For Each strsplitter In strLocator.Split("_")
                    If InStr(strsplitter, "at") > 0 Then
                        If IsNumeric(strsplitter.Substring(2)) Then
                            strCompid = strsplitter.Substring(2)
                        Else
                            strCompid = 0
                        End If
                    End If
                    If InStr(strsplitter, "en") > 0 Then
                        If IsNumeric(strsplitter.Substring(2)) Then
                            strEntityid = strsplitter.Substring(2)
                        Else
                            strEntityid = "0"
                        End If
                    End If
                Next
                ObjComp.Id = strCompid
                ObjComp.EntityId = strEntityid
                ObjComp.Value = Me.ComponentObject.ArrayTextData(inti, 1)
                'Set the single text orderno 
                ObjComp.OrderNo = inti
                Me.AddComponentObject(ObjComp)
                ObjComp = Nothing
                strCompid = Nothing
            Next
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_am_componetize", "SetSingleCellComponents", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_am_componetize", "SetSingleCellComponents", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_am_componetize", "SetSingleCellComponents", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    'This function adds the component object to components collection
    Public Sub AddComponentObject(ByRef ObjComp As bc_am_component)
        If ComponentiserConstants.LogActivityFile Then If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_am_componetize", "AddComponentObject", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim blnObjExists As Boolean = False
            Dim exsObj As bc_am_component
            'loop through the collection to verify whether that object is already existing in the collection or not.
            For Each exsObj In Me.ComponentContainer.Components
                If exsObj.Locator.ToLower.Trim = ObjComp.Locator.ToLower.Trim Then
                    blnObjExists = True
                    Exit For
                End If
            Next
            If Not blnObjExists Then
                Me.ComponentContainer.Components.Add(ObjComp)
            End If
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_am_componetize", "AddComponentObject", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_am_componetize", "AddComponentObject", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_am_componetize", "AddComponentObject", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try


    End Sub
    ' This function is used to componentize given  bookmark in a document.
    Public Function SetSingleCellComponents(ByVal strLocator As String) As Boolean
        If ComponentiserConstants.LogActivityFile Then If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_am_componetize", "SetDisclosureTextComponent", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim ObjComp As bc_am_component
            'get the main text value.
            Dim strMainText As String
            Dim strsplitter As String
            Dim strEntityid As String = ""
            Dim strCompid As Long
            strMainText = Me.ComponentObject.GetBookMarkValue(strLocator)
            'Instantiate component object with all single text values and add the same to components collection.
            ObjComp = New bc_am_component
            ObjComp.ComponentType = bc_am_component.CompType.Text
            'Generate locator id as we dont have definied type for main text
            ObjComp.Locator = strLocator
            'Get the entityid and component id from the locator.
            'rnet31_at358_en773
            For Each strsplitter In strLocator.Split("_")
                If InStr(strsplitter, "at") > 0 Then
                    If IsNumeric(strsplitter.Substring(2)) Then
                        strCompid = strsplitter.Substring(2)
                    Else
                        strCompid = 0
                    End If
                End If
                If InStr(strsplitter, "en") > 0 Then
                    If IsNumeric(strsplitter.Substring(2)) Then
                        strEntityid = strsplitter.Substring(2)
                    Else
                        strEntityid = "0"
                    End If
                End If
            Next
            ObjComp.EntityId = strEntityid
            ObjComp.Id = strCompid
            'No entity and component id's as there is no bookmark
            ObjComp.Value = strMainText
            Me.OrderCount = Me.OrderCount + 1
            ObjComp.OrderNo = Me.OrderCount
            Me.AddComponentObject(ObjComp)
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_am_componetize", "SetSingleCellComponents", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_am_componetize", "SetSingleCellComponents", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_am_componetize", "SetDisclosureTextComponent", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    ' This function is used to componentize all single text values of a document with No Bookmark .
    Public Function SetSingleTextwithNoBookmark() As Boolean
        If ComponentiserConstants.LogActivityFile Then If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_am_componetize", "SetSingleTextwithNoBookmark", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            'Get the single text values with no bookmark ids.
            Dim ObjComp As bc_am_component
            Dim inti As Integer
            Me.ComponentObject.get_text_comp_with_nobookmark()
            'Instantiate component object with all single text values and add the same to components collection.
            For inti = 1 To Me.ComponentObject.NoBookMarkTextCount
                ObjComp = New bc_am_component
                ObjComp.ComponentType = bc_am_component.CompType.Text
                ObjComp.Locator = Me.ComponentObject.ArrayNoBookmarkTextData(inti, 0)
                'No entity and component id's as there is no bookmark
                ObjComp.Value = Me.ComponentObject.ArrayNoBookmarkTextData(inti, 1)
                Me.AddComponentObject(ObjComp)
                ObjComp = Nothing
            Next
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_am_componetize", "SetSingleTextwithNoBookmark", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_am_componetize", "SetSingleTextwithNoBookmark", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If

        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_am_componetize", "SetSingleTextwithNoBookmark", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    'Rama created on 7/7/2006
    ' This function is used to componentize all implication values of a document.
    Public Function SetImplicationsImageValues() As Boolean
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_am_componetize", "SetImplicationsImages", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim ObjComp As bc_am_component
            Dim inti As Integer
            Dim strImplicationLoc As String
            'Initially componentize main implication image value
            ObjComp = New bc_am_component
            ObjComp.ComponentType = bc_am_component.CompType.Text
            ObjComp.Locator = "invr_imp_for_for"
            Me.OrderCount = Me.OrderCount + 1
            ObjComp.OrderNo = Me.OrderCount
            'No entity and component id's as there is no bookmark
            ObjComp.Value = Me.ComponentObject.GetDocumentProperty("invr_imp_for_for")
            Me.AddComponentObject(ObjComp)
            ObjComp = Nothing
            'Instantiate component object with all implication values and add the same to components collection.
            For inti = 0 To 9
                ObjComp = New bc_am_component
                ObjComp.ComponentType = bc_am_component.CompType.Text
                strImplicationLoc = "inv_impv" & inti
                ObjComp.Locator = strImplicationLoc
                'No entity and component id's as there is no bookmark
                ObjComp.Value = Me.ComponentObject.GetDocumentProperty(strImplicationLoc)
                Me.AddComponentObject(ObjComp)
                ObjComp = Nothing
            Next
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_am_componetize", "SetImplicationsImages", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_am_componetize", "SetImplicationsImages", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_am_componetize", "SetImplicationsImages", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    ' This function is used to componentize main text value of a document.
    Public Function SetMainTextComponent() As Boolean
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_am_componetize", "SetMainTextComponent", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim strMainText As String
        Try
            strMainText = Me.ComponentObject.get_undefined_text(Me.ComponentContainer)
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_am_componetize", "SetMainTextComponent", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_am_componetize", "SetMainTextComponent", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_am_componetize", "SetMainTextComponent", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    ' This function is used to componentize title of a document.
    Public Function SetTitleComponent() As Boolean
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_am_componetize", "SetTitleComponent", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim ObjComp As bc_am_component
            'get the main text value.
            Dim strTitle As String
            strTitle = Me.ComponentObject.GetDocumentProperty("xrnet_title")
            'Instantiate component object with all single text values and add the same to components collection.
            ObjComp = New bc_am_component
            ObjComp.ComponentType = bc_am_component.CompType.Text
            'Generate locator id as we dont have definied type for main text
            ObjComp.Locator = "xrnet_title"
            'No entity and component id's as there is no bookmark
            ObjComp.Value = strTitle
            Me.AddComponentObject(ObjComp)
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_am_componetize", "SetTitleComponentt", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_am_componetize", "SetTitleComponentt", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_am_componetize", "SetTitleComponent", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    ' This function is used to componentize all images in a document.
    Public Function SetImageComponents() As Boolean
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_am_componetize", "SetImageComponents", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            'Get the Image values
            Dim ObjComp As bc_am_component
            Dim inti As Integer
            Dim strCompid As Long
            Dim strsplitter As String
            Dim strLocator As String
            Dim intUndefCounter As Integer
            Dim intUndefinedOrder As Integer
            Dim strEntityid As String = ""
            Me.ComponentObject.get_image_components()
            'Instantiate component object with all single text values and add the same to components collection.
            Dim fts As New bc_cs_file_transfer_services
            For inti = 1 To Me.ComponentObject.ImageCount
                If Not Me.ComponentObject.ArrayImageData(inti, 0) Is Nothing Then
                    ObjComp = New bc_am_component
                    ObjComp.ComponentType = bc_am_component.CompType.Image
                    ObjComp.Locator = Me.ComponentObject.ArrayImageData(inti, 0)
                    Me.OrderCount = Me.OrderCount + 1
                    ObjComp.OrderNo = Me.OrderCount
                    strLocator = ObjComp.Locator
                    'Get the entityid and component id from the locator.
                    'rnet31_at358_en773
                    intUndefCounter = 0
                    intUndefinedOrder = 0
                    For Each strsplitter In strLocator.Split("_")
                        intUndefCounter = intUndefCounter + 1
                        If InStr(strsplitter, "at") > 0 Then
                            If IsNumeric(strsplitter.Substring(2)) Then
                                strCompid = strsplitter.Substring(2)
                            Else
                                strCompid = 0
                            End If
                        End If
                        If intUndefCounter = 3 AndAlso _
                            InStr(strLocator.ToLower.Trim, "undefined_image") > 0 Then
                            intUndefinedOrder = strsplitter
                        End If
                        If InStr(strsplitter, "en") > 0 Then
                            If IsNumeric(strsplitter.Substring(2)) Then
                                strEntityid = strsplitter.Substring(2)
                            Else
                                strEntityid = "0"
                            End If
                        End If
                    Next
                    ObjComp.EntityId = strEntityid
                    ObjComp.Id = strCompid
                    ObjComp.Value = Me.ComponentObject.ArrayImageData(inti, 1)
                    'Set the image orderno 
                    If CStr(intUndefinedOrder) <> "0" Then
                        ObjComp.OrderNo = intUndefinedOrder
                    Else
                        ObjComp.OrderNo = inti
                    End If

                    fts.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + ObjComp.Value, ObjComp.Image, Nothing)
                    fts.delete_file(bc_cs_central_settings.local_repos_path + ObjComp.Value)

                    Me.AddComponentObject(ObjComp)
                    ObjComp = Nothing
                    strCompid = Nothing
                End If
            Next
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_am_componetize", "SetImageComponents", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_am_componetize", "SetImageComponents", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_am_componetize", "SetImageComponents", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    ' This function is used to componentize given image in a document.
    Public Function SetImageComponents(ByVal strLocator As String) As Boolean
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_am_componetize", "SetImageComponents", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            'Get the Image values
            Dim ObjComp As bc_am_component
            Dim strCompid As Long
            Dim strsplitter As String
            Dim strEntityid As String = ""
            Dim strFileName As String
            strFileName = Me.ComponentObject.get_image_components(strLocator)
            If strFileName.Trim <> "" Then
                'Instantiate component object with all single text values and add the same to components collection.
                Dim fts As New bc_cs_file_transfer_services
                ObjComp = New bc_am_component
                ObjComp.ComponentType = bc_am_component.CompType.Image
                For Each strsplitter In strLocator.Split("_")
                    If InStr(strsplitter, "at") > 0 Then
                        If IsNumeric(strsplitter.Substring(2)) Then
                            strCompid = strsplitter.Substring(2)
                        Else
                            strCompid = 0
                        End If
                    End If
                    If InStr(strsplitter, "en") > 0 Then
                        If IsNumeric(strsplitter.Substring(2)) Then
                            strEntityid = strsplitter.Substring(2)
                        Else
                            strEntityid = "0"
                        End If
                    End If
                Next
                ObjComp.Id = strCompid
                ObjComp.EntityId = strEntityid
                ObjComp.Locator = strLocator
                ObjComp.Value = strFileName
                Me.OrderCount = Me.OrderCount + 1
                ObjComp.OrderNo = Me.OrderCount
                fts.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + strFileName, ObjComp.Image, Nothing)
                fts.delete_file(bc_cs_central_settings.local_repos_path + ObjComp.Value)
                Me.AddComponentObject(ObjComp)
            End If

        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_am_componetize", "SetImageComponents", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_am_componetize", "SetImageComponents", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_am_componetize", "SetImageComponents", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    ' This function is used to componentize all tables in a document.
    'Public Function SetTableComponents() As Boolean
    '    '    If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_am_componetize", "SetTableComponents", bc_cs_activity_codes.TRACE_ENTRY, "")
    '    Try
    '        Dim ObjComp As bc_am_component
    '        Dim intm As Integer
    '        Dim intj As Integer
    '        Dim intk As Integer = 1
    '        Dim inti As Integer
    '        Dim strLocator As String
    '        Dim strEntityid As String = ""
    '        Dim fts As New bc_cs_file_transfer_services
    '        Dim intUndefCounter As Integer
    '        Dim intUndefinedOrder As Integer
    '        Dim strsplitter As String
    '        Dim strTablePositionDetails As String = ""
    '        'Get the tables info
    '        Me.ComponentObject.get_table_components()
    '        'Instantiate component object with all single text values and add the same to components collection.
    '        For intm = 1 To Me.ComponentObject.TableCount
    '            ObjComp = New bc_am_component
    '            Dim oTable As New DataTable
    '            Dim oRows As DataRow
    '            Dim strFileName As String
    '            Dim strCompid As Long
    '            ObjComp.TableData.DataSetName = "TablesData"
    '            ObjComp.TableData.Namespace = "Component"
    '            ObjComp.TableData.Tables.Add("CompTable")
    '            'Add in the Header Columns
    '            'Dim RowValue As DataColumn = New DataColumn("RowValue")
    '            'RowValue.DataType = System.Type.GetType("System.DateTime")
    '            ObjComp.TableData.Tables(0).Columns.Add("RowValue", System.Type.GetType("System.Int16"))
    '            ObjComp.TableData.Tables(0).Columns.Add("ColumnValue", System.Type.GetType("System.Int16"))
    '            ObjComp.TableData.Tables(0).Columns.Add("Value", System.Type.GetType("System.Object"))
    '            ObjComp.TableData.Tables(0).Columns.Add("ImageBytes", System.Type.GetType("System.Object"))
    '            ObjComp.TableData.Tables(0).Columns.Add("Style", System.Type.GetType("System.String"))
    '            ObjComp.ComponentType = bc_am_component.CompType.Table
    '            ObjComp.Locator = Me.ComponentObject.ArrayTableData(intk, 0)
    '            ' Get the table position details 
    '            strTablePositionDetails = Me.ComponentObject.get_table_position_info(ObjComp.Locator)
    '            ObjComp.Style = strTablePositionDetails
    '            Me.OrderCount = Me.OrderCount + 1
    '            ObjComp.OrderNo = Me.OrderCount
    '            intUndefCounter = 0
    '            intUndefinedOrder = 0
    '            For Each strsplitter In ObjComp.Locator.Split("_")
    '                intUndefCounter = intUndefCounter + 1
    '                If intUndefCounter = 3 AndAlso _
    '                    InStr(ObjComp.Locator.ToLower.Trim, "undefined_table") > 0 Then
    '                    intUndefinedOrder = strsplitter
    '                End If
    '                If InStr(strsplitter, "at") > 0 Then
    '                    If IsNumeric(strsplitter.Substring(2)) Then
    '                        strCompid = strsplitter.Substring(2)
    '                    Else
    '                        strCompid = 0
    '                    End If
    '                End If
    '                If InStr(strsplitter, "en") > 0 Then
    '                    If IsNumeric(strsplitter.Substring(2)) Then
    '                        strEntityid = strsplitter.Substring(2)
    '                    Else
    '                        strEntityid = "0"
    '                    End If
    '                End If
    '            Next
    '            ObjComp.Id = strCompid
    '            ObjComp.EntityId = strEntityid
    '            'Set the table orderno 
    '            If CStr(intUndefinedOrder) <> "0" Then
    '                ObjComp.OrderNo = intUndefinedOrder
    '            Else
    '                ObjComp.OrderNo = intm
    '            End If

    '            strLocator = ObjComp.Locator
    '            If strLocator = CStr(Me.ComponentObject.ArrayTableInfo(intm, 0)) Then
    '                'There will be no component id and entity idfor table components.
    '                ObjComp.TableRowCount = Me.ComponentObject.ArrayTableInfo(intm, 1)
    '                ObjComp.TableColumnCount = Me.ComponentObject.ArrayTableInfo(intm, 2)
    '                oTable = ObjComp.TableData.Tables(0)
    '                For inti = 1 To ObjComp.TableRowCount
    '                    For intj = 1 To ObjComp.TableColumnCount
    '                        'Check if that particular column is existing in the arraytable data
    '                        'if exists then only add that corresponding value otherwise
    '                        ' treat that those columns have been merged 
    '                        If intj = CInt(Me.ComponentObject.ArrayTableData(intk, 2)) Then
    '                            oRows = oTable.NewRow()
    '                            oRows(0) = CStr(Me.ComponentObject.ArrayTableData(intk, 1))
    '                            oRows(1) = CStr(Me.ComponentObject.ArrayTableData(intk, 2))
    '                            If Not Me.ComponentObject.ArrayTableData(intk, 3) Is Nothing Then
    '                                oRows(2) = CStr(Me.ComponentObject.ArrayTableData(intk, 3))
    '                                ' Check if the cell value is an image and write to hard disk if its an image.
    '                                strFileName = CStr(Me.ComponentObject.ArrayTableData(intk, 3))
    '                                strFileName = strFileName.Trim
    '                            Else
    '                                oRows(2) = ""
    '                                strFileName = ""
    '                            End If
    '                            If strFileName <> "" AndAlso strFileName.Length > 4 AndAlso _
    '                                        strFileName.Substring((strFileName.Length - 4), 4) = ".bmp" Then
    '                                fts.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + strFileName, ObjComp.Image, Nothing)
    '                                fts.delete_file(bc_cs_central_settings.local_repos_path + strFileName)
    '                                oRows(3) = ObjComp.Image
    '                            Else
    '                                oRows(3) = ""
    '                            End If
    '                            If Not Me.ComponentObject.ArrayTableData(intk, 4) Is Nothing Then
    '                                'get the style info
    '                                oRows(4) = CStr(Me.ComponentObject.ArrayTableData(intk, 4))
    '                            Else
    '                                oRows(4) = ""
    '                            End If
    '                            oTable.Rows.Add(oRows)
    '                            intk = intk + 1
    '                            'after writing the file reset string value as null
    '                            strFileName = ""
    '                        Else
    '                            'If a particular column is not existing insert null value 
    '                            oRows = oTable.NewRow()
    '                            oRows(0) = inti
    '                            oRows(1) = intj
    '                            oRows(2) = ""
    '                            oRows(3) = ""
    '                            oRows(4) = ""
    '                            oTable.Rows.Add(oRows)
    '                        End If
    '                    Next
    '                Next
    '                '    ObjComp.TableData.WriteXml("c:/" & ObjComp.Locator & ".dat")
    '                Me.AddComponentObject(ObjComp)
    '                ObjComp = Nothing
    '                oTable = Nothing
    '                oRows = Nothing
    '            End If
    '        Next
    '    Catch ex As Exception
    '        Me.Result = False
    '        If ComponentiserConstants.ShowErrorMessage Then
    '            Dim db_err As New bc_cs_error_log("bc_am_componetize", "SetTableComponents", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '        Else
    '            Dim otrace2 As New bc_cs_activity_log("bc_am_componetize", "SetTableComponents", bc_cs_activity_codes.COMMENTARY, ex.Message)
    '        End If
    '    Finally
    '        If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_am_componetize", "SetTableComponents", bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try
    'End Function
    Public Function NewSetTableComponents() As Boolean
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_am_componetize", "SetTableComponents", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim ObjComp As bc_am_component
            Dim intm As Integer
            Dim intj As Integer
            Dim intk As Integer = 1
            Dim inti As Integer
            Dim strLocator As String
            Dim strEntityid As String = ""
            Dim fts As New bc_cs_file_transfer_services
            Dim intUndefCounter As Integer
            Dim intUndefinedOrder As Integer
            Dim strsplitter As String
            Dim strTablePositionDetails As String = ""
            'Get the tables info
            Me.ComponentObject.get_table_components()
            'Instantiate component object with all single text values and add the same to components collection.
            For intm = 1 To Me.ComponentObject.TableCount
                ObjComp = New bc_am_component
                Dim oTable As New ArrayList
                Dim oRows As bc_om_component_table_cell
                Dim strFileName As String
                Dim strCompid As Long
                'ObjComp.TableData.DataSetName = "TablesData"
                'ObjComp.TableData.Namespace = "Component"
                'ObjComp.TableData.Tables.Add("CompTable")
                'Add in the Header Columns
                'Dim RowValue As DataColumn = New DataColumn("RowValue")
                'RowValue.DataType = System.Type.GetType("System.DateTime")
                'ObjComp.TableData.Tables(0).Columns.Add("RowValue", System.Type.GetType("System.Int16"))
                'ObjComp.TableData.Tables(0).Columns.Add("ColumnValue", System.Type.GetType("System.Int16"))
                'ObjComp.TableData.Tables(0).Columns.Add("Value", System.Type.GetType("System.Object"))
                'ObjComp.TableData.Tables(0).Columns.Add("ImageBytes", System.Type.GetType("System.Object"))
                'ObjComp.TableData.Tables(0).Columns.Add("Style", System.Type.GetType("System.String"))
                ObjComp.ComponentType = bc_am_component.CompType.NewTable
                ObjComp.Locator = Me.ComponentObject.ArrayTableData(intk, 0)
                ' Get the table position details 
                strTablePositionDetails = Me.ComponentObject.get_table_position_info(ObjComp.Locator)
                ObjComp.Style = strTablePositionDetails
                Me.OrderCount = Me.OrderCount + 1
                ObjComp.OrderNo = Me.OrderCount
                intUndefCounter = 0
                intUndefinedOrder = 0
                For Each strsplitter In ObjComp.Locator.Split("_")
                    intUndefCounter = intUndefCounter + 1
                    If intUndefCounter = 3 AndAlso _
                        InStr(ObjComp.Locator.ToLower.Trim, "undefined_table") > 0 Then
                        intUndefinedOrder = strsplitter
                    End If
                    If InStr(strsplitter, "at") > 0 Then
                        If IsNumeric(strsplitter.Substring(2)) Then
                            strCompid = strsplitter.Substring(2)
                        Else
                            strCompid = 0
                        End If
                    End If
                    If InStr(strsplitter, "en") > 0 Then
                        If IsNumeric(strsplitter.Substring(2)) Then
                            strEntityid = strsplitter.Substring(2)
                        Else
                            strEntityid = "0"
                        End If
                    End If
                Next
                ObjComp.Id = strCompid
                ObjComp.EntityId = strEntityid
                'Set the table orderno 
                If CStr(intUndefinedOrder) <> "0" Then
                    ObjComp.OrderNo = intUndefinedOrder
                Else
                    ObjComp.OrderNo = intm
                End If

                strLocator = ObjComp.Locator
                If strLocator = CStr(Me.ComponentObject.ArrayTableInfo(intm, 0)) Then
                    'There will be no component id and entity idfor table components.
                    ObjComp.TableRowCount = Me.ComponentObject.ArrayTableInfo(intm, 1)
                    ObjComp.TableColumnCount = Me.ComponentObject.ArrayTableInfo(intm, 2)
                    'oTable = ObjComp.TableData.Tables(0)
                    For inti = 1 To ObjComp.TableRowCount
                        For intj = 1 To ObjComp.TableColumnCount
                            'Check if that particular column is existing in the arraytable data
                            'if exists then only add that corresponding value otherwise
                            ' treat that those columns have been merged 
                            If intj = CInt(Me.ComponentObject.ArrayTableData(intk, 2)) Then
                                oRows = New bc_om_component_table_cell

                                oRows.RowValue = CStr(Me.ComponentObject.ArrayTableData(intk, 1))
                                oRows.ColumnValue = CStr(Me.ComponentObject.ArrayTableData(intk, 2))
                                If Not Me.ComponentObject.ArrayTableData(intk, 3) Is Nothing Then
                                    oRows.Value = CStr(Me.ComponentObject.ArrayTableData(intk, 3))
                                    ' Check if the cell value is an image and write to hard disk if its an image.
                                    strFileName = CStr(Me.ComponentObject.ArrayTableData(intk, 3))
                                    strFileName = strFileName.Trim
                                Else
                                    oRows.Value = ""
                                    strFileName = ""
                                End If
                                If strFileName <> "" AndAlso strFileName.Length > 4 AndAlso _
                                            strFileName.Substring((strFileName.Length - 4), 4) = ".bmp" Then
                                    fts.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + strFileName, ObjComp.Image, Nothing)
                                    fts.delete_file(bc_cs_central_settings.local_repos_path + strFileName)
                                    oRows.Imagebytes = ObjComp.Image
                                Else
                                    oRows.Imagebytes = ""
                                End If
                                If Not Me.ComponentObject.ArrayTableData(intk, 4) Is Nothing Then
                                    'get the style info
                                    oRows.Style = CStr(Me.ComponentObject.ArrayTableData(intk, 4))
                                Else
                                    oRows.Style = ""
                                End If
                                ObjComp.newstructuretable.Add(oRows)

                                intk = intk + 1
                                'after writing the file reset string value as null
                                strFileName = ""
                            Else
                                'If a particular column is not existing insert null value 
                                oRows = New bc_om_component_table_cell
                                oRows.RowValue = inti
                                oRows.ColumnValue = intj
                                oRows.Value = ""
                                oRows.Imagebytes = ""
                                oRows.Style = ""
                                ObjComp.newstructuretable.Add(oRows)

                            End If
                        Next
                    Next
                    '    ObjComp.TableData.WriteXml("c:/" & ObjComp.Locator & ".dat")
                    Me.AddComponentObject(ObjComp)
                    ObjComp = Nothing
                    oTable = Nothing
                    oRows = Nothing
                End If
            Next
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_am_componetize", "SetTableComponents", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_am_componetize", "SetTableComponents", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_am_componetize", "SetTableComponents", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function

    ' This function is used to componentize a given table in a document.
    'Public Function SetTableComponents(ByVal strTableName As String, ByVal intTableCount As Integer, _
    '              ByVal strDisclosureMergedCell As String, ByVal strDisclosureHeightWidth As String, _
    '              ByVal strDisclosureContainsImages As String) As Boolean
    '    If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_am_componetize", "SetDisclosureTableComponent", bc_cs_activity_codes.TRACE_ENTRY, "")
    '    Try
    '        Dim ObjComp As bc_am_component

    '        Dim intj As Integer
    '        Dim intk As Integer = 0
    '        Dim inti As Integer
    '        Dim table_data_count As Integer = 1

    '        Dim strEntityid As String = ""
    '        Dim strCompid As Long
    '        Dim fts As New bc_cs_file_transfer_services
    '        'Instantiate component object with all single text values and add the same to components collection.
    '        ObjComp = New bc_am_component
    '        Dim oTable0 As New DataTable
    '        Dim oTable1 As New DataTable
    '        Dim oRows As DataRow
    '        Dim strsplitter As String
    '        Dim strFileName As String
    '        'ObjComp.TableData.DataSetName = "TablesData"
    '        'ObjComp.TableData.Namespace = "Component"
    '        ''Add the table to hold tables original data and null values in missing cells
    '        'ObjComp.TableData.Tables.Add("CompTable")
    '        'ObjComp.TableData.Tables(0).Columns.Add("RowValue", System.Type.GetType("System.Int16"))
    '        'ObjComp.TableData.Tables(0).Columns.Add("ColumnValue", System.Type.GetType("System.Int16"))
    '        'ObjComp.TableData.Tables(0).Columns.Add("Value", System.Type.GetType("System.Object"))
    '        'ObjComp.TableData.Tables(0).Columns.Add("ImageBytes", System.Type.GetType("System.Object"))
    '        'ObjComp.TableData.Tables(0).Columns.Add("Style", System.Type.GetType("System.String"))
    '        ''Add the table to hold tables original data
    '        'ObjComp.TableData.Tables.Add("CompTableData")
    '        'ObjComp.TableData.Tables(1).Columns.Add("RowValue", System.Type.GetType("System.Int16"))
    '        'ObjComp.TableData.Tables(1).Columns.Add("ColumnValue", System.Type.GetType("System.Int16"))
    '        'ObjComp.TableData.Tables(1).Columns.Add("Value", System.Type.GetType("System.Object"))
    '        'ObjComp.TableData.Tables(1).Columns.Add("ImageBytes", System.Type.GetType("System.Object"))
    '        'ObjComp.TableData.Tables(1).Columns.Add("Style", System.Type.GetType("System.String"))
    '        'ObjComp.ComponentType = bc_am_component.CompType.newTable
    '        ObjComp.TableRowCount = 0
    '        ObjComp.TableColumnCount = 0
    '        ObjComp.Locator = strTableName
    '        For Each strsplitter In ObjComp.Locator.Split("_")
    '            If InStr(strsplitter, "at") > 0 Then
    '                If IsNumeric(strsplitter.Substring(2)) Then
    '                    strCompid = strsplitter.Substring(2)
    '                Else
    '                    strCompid = 0
    '                End If
    '            End If
    '            If InStr(strsplitter, "en") > 0 Then
    '                If IsNumeric(strsplitter.Substring(2)) Then
    '                    strEntityid = strsplitter.Substring(2)
    '                Else
    '                    strEntityid = "0"
    '                End If
    '            End If
    '        Next
    '        ObjComp.Id = strCompid
    '        ObjComp.EntityId = strEntityid
    '        Me.OrderCount = Me.OrderCount + 1
    '        ObjComp.OrderNo = Me.OrderCount

    '        'Get the tables info
    '        Me.ComponentObject.get_table_cell_values(ObjComp, strTableName, intTableCount, _
    '                                 strDisclosureMergedCell, strDisclosureHeightWidth, _
    '                                 strDisclosureContainsImages)
    '        oTable0 = ObjComp.TableData.Tables(0)
    '        oTable1 = ObjComp.TableData.Tables(1)
    '        ' Populate the table0 with table1 information and add null values for missing cells in table1
    '        For inti = 1 To ObjComp.TableRowCount
    '            For intj = 1 To ObjComp.TableColumnCount
    '                If (intk < oTable1.Rows.Count) AndAlso _
    '                         (intj = oTable1.Rows(intk)(1)) Then
    '                    oRows = oTable0.NewRow()
    '                    oRows(0) = inti
    '                    oRows(1) = intj
    '                    If Not oTable1.Rows(intk)(2) Is Nothing Then
    '                        oRows(2) = oTable1.Rows(intk)(2)
    '                        ' Check if the cell value is an image and write to hard disk if its an image.
    '                        strFileName = CStr(oTable1.Rows(intk)(2))
    '                        strFileName = strFileName.Trim
    '                    Else
    '                        oRows(2) = ""
    '                        strFileName = ""
    '                    End If
    '                    If strFileName <> "" AndAlso strFileName.Length > 4 AndAlso _
    '                             strFileName.Substring((strFileName.Length - 4), 4) = ".bmp" Then
    '                        oRows(3) = fts.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + strFileName, ObjComp.Image, Nothing)
    '                        fts.delete_file(bc_cs_central_settings.local_repos_path + strFileName)
    '                        oRows(3) = ObjComp.Image
    '                    Else
    '                        oRows(3) = ""
    '                    End If
    '                    If Not oTable1.Rows(intk)(4) Is Nothing Then
    '                        'add the style row
    '                        oRows(4) = oTable1.Rows(intk)(4)
    '                    Else
    '                        oRows(4) = ""
    '                    End If
    '                    oTable0.Rows.Add(oRows)
    '                    intk = intk + 1
    '                Else
    '                    'If a particular column is not existing insert null value 
    '                    oRows = oTable0.NewRow()
    '                    oRows(0) = inti
    '                    oRows(1) = intj
    '                    oRows(2) = ""
    '                    oRows(3) = ""
    '                    oRows(4) = ""
    '                    oTable0.Rows.Add(oRows)
    '                End If
    '            Next
    '        Next
    '        Me.AddComponentObject(ObjComp)
    '        ObjComp = Nothing
    '    Catch ex As Exception
    '        Me.Result = False
    '        If ComponentiserConstants.ShowErrorMessage Then
    '            Dim db_err As New bc_cs_error_log("bc_am_componetize", "SetTableComponents", bc_cs_error_codes.USER_DEFINED, ex.Message)
    '        Else
    '            Dim otrace2 As New bc_cs_activity_log("bc_am_componetize", "SetTableComponents", bc_cs_activity_codes.COMMENTARY, ex.Message)
    '        End If
    '    Finally
    '        If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_am_componetize", "SetDisclosureTableComponent", bc_cs_activity_codes.TRACE_EXIT, "")
    '    End Try
    'End Function
    Public Function NewSetTableComponents(ByVal strTableName As String, ByVal intTableCount As Integer, _
                    ByVal strDisclosureMergedCell As String, ByVal strDisclosureHeightWidth As String, _
                    ByVal strDisclosureContainsImages As String) As Boolean
        If ComponentiserConstants.LogActivityFile Then Dim otrace As New bc_cs_activity_log("bc_am_componetize", "NewSetTableComponents", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim ObjComp As bc_am_component
            Dim table_data_count As Integer = 1
            Dim strEntityid As String = ""
            Dim strCompid As Long
            Dim fts As New bc_cs_file_transfer_services
            'Instantiate component object with all single text values and add the same to components collection.
            ObjComp = New bc_am_component
            'Dim oTable0 As New DataTable
            'Dim oTable1 As New DataTable
            Dim strsplitter As String
            'ObjComp.TableData.DataSetName = "TablesData"
            'ObjComp.TableData.Namespace = "Component"
            ''Add the table to hold tables original data and null values in missing cells
            'ObjComp.TableData.Tables.Add("CompTable")
            'ObjComp.TableData.Tables(0).Columns.Add("RowValue", System.Type.GetType("System.Int16"))
            'ObjComp.TableData.Tables(0).Columns.Add("ColumnValue", System.Type.GetType("System.Int16"))
            'ObjComp.TableData.Tables(0).Columns.Add("Value", System.Type.GetType("System.Object"))
            'ObjComp.TableData.Tables(0).Columns.Add("ImageBytes", System.Type.GetType("System.Object"))
            'ObjComp.TableData.Tables(0).Columns.Add("Style", System.Type.GetType("System.String"))
            ''Add the table to hold tables original data
            'ObjComp.TableData.Tables.Add("CompTableData")
            'ObjComp.TableData.Tables(1).Columns.Add("RowValue", System.Type.GetType("System.Int16"))
            'ObjComp.TableData.Tables(1).Columns.Add("ColumnValue", System.Type.GetType("System.Int16"))
            'ObjComp.TableData.Tables(1).Columns.Add("Value", System.Type.GetType("System.Object"))
            'ObjComp.TableData.Tables(1).Columns.Add("ImageBytes", System.Type.GetType("System.Object"))
            'ObjComp.TableData.Tables(1).Columns.Add("Style", System.Type.GetType("System.String"))
            ObjComp.ComponentType = bc_am_component.CompType.NewTable
            ObjComp.TableRowCount = 0
            ObjComp.TableColumnCount = 0
            ObjComp.Locator = strTableName
            For Each strsplitter In ObjComp.Locator.Split("_")
                If InStr(strsplitter, "at") > 0 Then
                    If IsNumeric(strsplitter.Substring(2)) Then
                        strCompid = strsplitter.Substring(2)
                    Else
                        strCompid = 0
                    End If
                End If
                If InStr(strsplitter, "en") > 0 Then
                    If IsNumeric(strsplitter.Substring(2)) Then
                        strEntityid = strsplitter.Substring(2)
                    Else
                        strEntityid = "0"
                    End If
                End If
            Next
            ObjComp.Id = strCompid
            ObjComp.EntityId = strEntityid
            Me.OrderCount = Me.OrderCount + 1
            ObjComp.OrderNo = Me.OrderCount

            'Get the tables info
            Me.ComponentObject.new_get_table_cell_values(ObjComp, strTableName, intTableCount, _
                                     strDisclosureMergedCell, strDisclosureHeightWidth, _
                                     strDisclosureContainsImages)

            'oTable0 = ObjComp.TableData.Tables(0)
            'oTable1 = ObjComp.newstructuretable

            ' Populate the table0 with table1 information and add null values for missing cells in table1
            'For inti = 1 To ObjComp.TableRowCount
            '    For intj = 1 To ObjComp.TableColumnCount
            '        If (intk < oTable1.Rows.Count) AndAlso _
            '                 (intj = oTable1.Rows(intk)(1)) Then
            '            oRows = oTable0.NewRow()
            '            oRows(0) = inti
            '            oRows(1) = intj
            '            If Not oTable1.Rows(intk)(2) Is Nothing Then
            '                oRows(2) = oTable1.Rows(intk)(2)
            '                ' Check if the cell value is an image and write to hard disk if its an image.
            '                strFileName = CStr(oTable1.Rows(intk)(2))
            '                strFileName = strFileName.Trim
            '            Else
            '                oRows(2) = ""
            '                strFileName = ""
            '            End If
            '            If strFileName <> "" AndAlso strFileName.Length > 4 AndAlso _
            '                     strFileName.Substring((strFileName.Length - 4), 4) = ".bmp" Then
            '                oRows(3) = fts.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + strFileName, ObjComp.Image, Nothing)
            '                fts.delete_file(bc_cs_central_settings.local_repos_path + strFileName)
            '                oRows(3) = ObjComp.Image
            '            Else
            '                oRows(3) = ""
            '            End If
            '            If Not oTable1.Rows(intk)(4) Is Nothing Then
            '                'add the style row
            '                oRows(4) = oTable1.Rows(intk)(4)
            '            Else
            '                oRows(4) = ""
            '            End If
            '            oTable0.Rows.Add(oRows)
            '            intk = intk + 1
            '        Else
            '            'If a particular column is not existing insert null value 
            '            oRows = oTable0.NewRow()
            '            oRows(0) = inti
            '            oRows(1) = intj
            '            oRows(2) = ""
            '            oRows(3) = ""
            '            oRows(4) = ""
            '            oTable0.Rows.Add(oRows)
            '        End If
            '    Next
            'Next
            Me.AddComponentObject(ObjComp)
            ObjComp = Nothing
        Catch ex As Exception
            Me.Result = False
            If ComponentiserConstants.ShowErrorMessage Then
                Dim db_err As New bc_cs_error_log("bc_am_componetize", "SetTableComponents", bc_cs_error_codes.USER_DEFINED, ex.Message)
            Else
                Dim otrace2 As New bc_cs_activity_log("bc_am_componetize", "SetTableComponents", bc_cs_activity_codes.COMMENTARY, ex.Message)
            End If
        Finally
            If ComponentiserConstants.LogActivityFile Then Dim otrace1 As New bc_cs_activity_log("bc_am_componetize", "SetDisclosureTableComponent", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
   
End Class
