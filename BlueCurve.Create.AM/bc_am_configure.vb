Imports Microsoft.Win32
Imports System.io
Imports System.Threading
Imports System.Xml
Imports System.Collections
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
'This file is created by Rama Boya 
Public Class bc_cs_configure
    Const BC_COMPOSITE_CONFIG_FILE = "bc_composite_config.xml"
    Const BC_APPLICATION_USER_AREA = "Bluecurve"
    Private pub_type_id As String
    Private pub_type As String
    Private arr_pages(10, 2) As String
    Private arr_locator(500, 5) As String
    Private str_locator As String
    Private str_page As String
    Private intLocCount As Integer
    Private blPubTypeexists As Boolean = False
    REM componentize section added by Rama Boya on 4-10-2006
    Private strComponentizeBy As String
    Private strComponentizeTableString1 As String
    Private strComponentizeTableString2 As String
    Private strComponentizeImageString1 As String
    Private strComponentizeImageString2 As String
    Private strComponentizeTextString1 As String
    Private strComponentizeTextString2 As String
    Public Shared strPrefixorSuffix As String = "PrefixorSuffix"
    Public Shared strAnyWhere As String = "Anywhere"
    Public Property ComponentizeBy() As String
        Get
            Return strComponentizeBy
        End Get
        Set(ByVal Value As String)
            strComponentizeBy = Value
        End Set
    End Property
    Public Property ComponentizeTableString1() As String
        Get
            Return strComponentizeTableString1
        End Get
        Set(ByVal Value As String)
            strComponentizeTableString1 = Value
        End Set
    End Property
    Public Property ComponentizeTableString2() As String
        Get
            Return strComponentizeTableString2
        End Get
        Set(ByVal Value As String)
            strComponentizeTableString2 = Value
        End Set
    End Property
    Public Property ComponentizeImageString1() As String
        Get
            Return strComponentizeImageString1
        End Get
        Set(ByVal Value As String)
            strComponentizeImageString1 = Value
        End Set
    End Property
    Public Property ComponentizeImageString2() As String
        Get
            Return strComponentizeImageString2
        End Get
        Set(ByVal Value As String)
            strComponentizeImageString2 = Value
        End Set
    End Property
    Public Property ComponentizeTextString1() As String
        Get
            Return strComponentizeTextString1
        End Get
        Set(ByVal Value As String)
            strComponentizeTextString1 = Value
        End Set
    End Property
    Public Property ComponentizeTextString2() As String
        Get
            Return strComponentizeTextString2
        End Get
        Set(ByVal Value As String)
            strComponentizeTextString2 = Value
        End Set
    End Property
    Public Property PubTypeId() As String
        Get
            Return pub_type_id
        End Get
        Set(ByVal Value As String)
            pub_type_id = Value
        End Set
    End Property
    Public Property PubType() As String
        Get
            Return pub_type
        End Get
        Set(ByVal Value As String)
            pub_type = Value
        End Set
    End Property
    Public Property PubTypeExists() As Boolean
        Get
            Return blPubTypeexists
        End Get
        Set(ByVal Value As Boolean)
            blPubTypeexists = Value
        End Set
    End Property
    Public Property ArrLocator() As Array
        Get
            Return arr_locator
        End Get
        Set(ByVal Value As Array)
            arr_locator = Value
        End Set
    End Property
    Public Property ArrPage() As Array
        Get
            Return arr_pages
        End Get
        Set(ByVal Value As Array)
            arr_pages = Value
        End Set
    End Property
    Public Property LocatorCount() As Integer
        Get
            Return intLocCount
        End Get
        Set(ByVal Value As Integer)
            intLocCount = Value
        End Set
    End Property

    Public Function read_ini_file(ByVal PubType As String) As Boolean
        Dim blnResult As Boolean
        Dim xmlload As New XmlDocument
        Dim strfn As String
        Try
            strfn = bc_cs_central_settings.local_template_path + BC_COMPOSITE_CONFIG_FILE
            xmlload.Load(strfn)
            blnResult = Me.ReadXMLFile(xmlload, PubType)
        Catch
            blnResult = False
            MsgBox("Composite Config File Error: " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
        Finally
            read_ini_file = blnResult
        End Try
    End Function
    Public Function read_implications_bookmark_name() As String
        Dim xmlload As New XmlDocument
        Dim strfn As String
        Try
            strfn = bc_cs_central_settings.local_template_path + BC_COMPOSITE_CONFIG_FILE
            xmlload.Load(strfn)
            Return xmlload.SelectSingleNode("/settings/Composite/ImplicationsBookmarkName").InnerXml()
        Catch
            MsgBox("Composite Config File Error: " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
            Return ""
        End Try
    End Function
    Private Function ReadXMLFile(ByRef xmlload As XmlDocument, ByVal DocPubType As String) As Boolean
        Dim myXmlNodeList As XmlNodeList
        Dim blnResult As Boolean = True
        Dim myLocatorNodelist As XmlNodeList
        Dim myLocatorNode As XmlNode
        Dim myXmlNode As XmlNode

        Dim strpubtypeid As String

        Dim inti As String = 0
        Try
            myXmlNodeList = xmlload.SelectNodes("descendant::Pubtypeid")
            For Each myXmlNode In myXmlNodeList
                strpubtypeid = myXmlNode.Attributes("value").InnerText
                If strpubtypeid.Trim = DocPubType.Trim Then
                    Me.PubType = myXmlNode.Attributes("name").InnerText
                    myLocatorNodelist = myXmlNode.SelectNodes("descendant::Locators/Locator")
                    Me.PubTypeExists = True
                    For Each myLocatorNode In myLocatorNodelist
                        inti = inti + 1
                        Me.ArrLocator(inti, 0) = myLocatorNode.Attributes("attribute").InnerText.Trim
                        Me.ArrLocator(inti, 1) = myLocatorNode.InnerText.Trim
                        If CStr(Me.ArrLocator(inti, 0)) = "49" AndAlso _
                               Not myLocatorNode.Attributes("MergedCells") Is Nothing Then
                            Me.ArrLocator(inti, 2) = myLocatorNode.Attributes("MergedCells").InnerText.Trim
                        ElseIf CStr(Me.ArrLocator(inti, 0)) = "49" Then
                            Me.ArrLocator(inti, 2) = "no"
                        End If
                        If CStr(Me.ArrLocator(inti, 0)) = "49" AndAlso _
                              Not myLocatorNode.Attributes("HeightWidthInfo") Is Nothing Then
                            Me.ArrLocator(inti, 3) = myLocatorNode.Attributes("HeightWidthInfo").InnerText.Trim
                        ElseIf Me.ArrLocator(inti, 0) = "49" Then
                            Me.ArrLocator(inti, 3) = "no"
                        End If
                        If CStr(Me.ArrLocator(inti, 0)) = "49" AndAlso _
                            Not myLocatorNode.Attributes("ContainsImages") Is Nothing Then
                            Me.ArrLocator(inti, 4) = myLocatorNode.Attributes("ContainsImages").InnerText.Trim
                        ElseIf Me.ArrLocator(inti, 0) = "49" Then
                            Me.ArrLocator(inti, 4) = "no"
                        End If
                    Next
                    Me.LocatorCount = inti
                    blnResult = True
                    Exit Function
                End If
            Next
        Catch
            blnResult = False
            MsgBox("Composite Config File Error: " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
        Finally
            ReadXMLFile = blnResult
        End Try
    End Function
    Public Function Read_Componentize_ini() As Boolean
        Dim blnResult As Boolean
        REM Composite section elements added by Rama Boya on 4-10-2006
        Try
            Dim xmlload As New XmlDocument
            Dim strfn As String
            Dim ndComponentize As Xml.XmlNode
            Dim myXmlNode As Xml.XmlNode
            Dim myXmlNodeList As Xml.XmlNodeList
            Dim strIndCompType As String = ""
            strfn = bc_cs_central_settings.local_template_path + BC_COMPOSITE_CONFIG_FILE
            xmlload.Load(strfn)
            ndComponentize = xmlload.SelectSingleNode("settings/ComponentizeBy")
            Me.ComponentizeBy = ndComponentize.Attributes("apply").InnerText.Trim
            myXmlNodeList = xmlload.SelectNodes("descendant::ComponentizeBy/Componentize")
            For Each myXmlNode In myXmlNodeList
                strIndCompType = myXmlNode.Attributes("name").InnerText
                If strIndCompType.ToLower.Trim = ComponentizeBy.ToLower.Trim Then
                    Me.ComponentizeTableString1 = myXmlNode.SelectSingleNode("descendant::Table/Option_1").InnerXml
                    Me.ComponentizeTableString2 = myXmlNode.SelectSingleNode("descendant::Table/Option_2").InnerXml
                    Me.ComponentizeImageString1 = myXmlNode.SelectSingleNode("descendant::Image/Option_1").InnerXml
                    Me.ComponentizeImageString2 = myXmlNode.SelectSingleNode("descendant::Image/Option_2").InnerXml
                    Me.ComponentizeTextString1 = myXmlNode.SelectSingleNode("descendant::Text/Option_1").InnerXml
                    Me.ComponentizeTextString2 = myXmlNode.SelectSingleNode("descendant::Text/Option_2").InnerXml
                    Exit For
                End If
            Next
            blnResult = True
        Catch
            blnResult = False
            MsgBox("Composite Config File Error: " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
        Finally
            Read_Componentize_ini = blnResult
        End Try
    End Function
    Public Function table_read_rich_text_conversion() As String
        Dim xmlload As New XmlDocument
        Dim strfn As String
        Try
            strfn = bc_cs_central_settings.local_template_path + BC_COMPOSITE_CONFIG_FILE
            xmlload.Load(strfn)
            Return xmlload.SelectSingleNode("/settings/RichTextFlags/TableComponent/RichTextConversion").InnerXml()
        Catch
            MsgBox("Composite Config File Error: " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
            Return ""
        End Try
    End Function
    Public Function table_capture_heightWidth_flag() As String
        Dim xmlload As New XmlDocument
        Dim strfn As String
        Dim myXmlNodeList As Xml.XmlNodeList
        Try
            strfn = bc_cs_central_settings.local_template_path + BC_COMPOSITE_CONFIG_FILE
            xmlload.Load(strfn)
            myXmlNodeList = xmlload.SelectNodes("descendant::StandardAssumptions/TableComponent/CaptureHeightWidthInfo")
            If myXmlNodeList.Count > 0 Then
                Return xmlload.SelectSingleNode("/settings/StandardAssumptions/TableComponent/CaptureHeightWidthInfo").InnerXml()
            Else
                Return "no"
            End If
        Catch
            MsgBox("Composite Config File Error:table_capture_heightWidth_flag " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
            Return ""
        End Try
    End Function
    Public Function table_has_merged_cells() As String
        Dim xmlload As New XmlDocument
        Dim strfn As String
        Dim myXmlNodeList As Xml.XmlNodeList
        Try
            strfn = bc_cs_central_settings.local_template_path + BC_COMPOSITE_CONFIG_FILE
            xmlload.Load(strfn)
            myXmlNodeList = xmlload.SelectNodes("descendant::StandardAssumptions/TableComponent/MergedCells")
            If myXmlNodeList.Count > 0 Then
                Return xmlload.SelectSingleNode("/settings/StandardAssumptions/TableComponent/MergedCells").InnerXml()
            Else
                Return "no"
            End If
        Catch
            MsgBox("Composite Config File Error:table_has_merged_cells " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
            Return ""
        End Try
    End Function
    Public Function table_contains_images() As String
        Dim xmlload As New XmlDocument
        Dim strfn As String
        Dim myXmlNodeList As Xml.XmlNodeList
        Try
            strfn = bc_cs_central_settings.local_template_path + BC_COMPOSITE_CONFIG_FILE
            xmlload.Load(strfn)
            myXmlNodeList = xmlload.SelectNodes("descendant::StandardAssumptions/TableComponent/ContainsImages")
            If myXmlNodeList.Count > 0 Then
                Return xmlload.SelectSingleNode("/settings/StandardAssumptions/TableComponent/ContainsImages").InnerXml()
            Else
                Return "no"
            End If
        Catch
            MsgBox("Composite Config File Error:table_contains_images " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
            Return ""
        End Try
    End Function
    Public Function main_area_read_rich_text_conversion() As String
        Dim xmlload As New XmlDocument
        Dim strfn As String
        Try
            strfn = bc_cs_central_settings.local_template_path + BC_COMPOSITE_CONFIG_FILE
            xmlload.Load(strfn)
            Return xmlload.SelectSingleNode("/settings/RichTextFlags/MainTextComponent/RichTextConversion").InnerXml()
        Catch
            MsgBox("Composite Config File Error: " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
            Return ""
        End Try
    End Function
    Public Function cen_reposit_read_flag() As String
        Dim xmlload As New XmlDocument
        Dim strfn As String
        Try
            strfn = bc_cs_central_settings.local_template_path + BC_COMPOSITE_CONFIG_FILE
            xmlload.Load(strfn)
            Return xmlload.SelectSingleNode("/settings/PubTypeCentralRepos").InnerXml()
        Catch
            MsgBox("Composite Config File Error: " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
            Return ""
        End Try
    End Function
    Public Function text_read_rich_text_conversion() As String
        Dim xmlload As New XmlDocument
        Dim strfn As String
        Try
            strfn = bc_cs_central_settings.local_template_path + BC_COMPOSITE_CONFIG_FILE
            xmlload.Load(strfn)
            Return xmlload.SelectSingleNode("/settings/RichTextFlags/TextComponent/RichTextConversion").InnerXml()
        Catch
            MsgBox("Composite Config File Error: " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
            Return ""
        End Try
    End Function
    Public Function DisplayErrorMsg() As Boolean
        Dim xmlload As New XmlDocument
        Dim strfn As String
        Dim strShowMsg As String
        Dim myXmlNodeList As Xml.XmlNodeList
        Try
            strfn = bc_cs_central_settings.local_template_path + BC_COMPOSITE_CONFIG_FILE
            xmlload.Load(strfn)
            myXmlNodeList = xmlload.SelectNodes("descendant::ShowErrorMessage")
            If myXmlNodeList.Count > 0 Then
                strShowMsg = xmlload.SelectSingleNode("settings/ShowErrorMessage").InnerXml()
                If strShowMsg.ToLower.Trim = "yes" Then
                    DisplayErrorMsg = True
                Else
                    DisplayErrorMsg = False
                End If
            Else
                DisplayErrorMsg = True
            End If
        Catch
            MsgBox("Composite Config File Error: " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
        End Try
    End Function
    Public Function LogActivity() As Boolean
        Dim xmlload As New XmlDocument
        Dim strfn As String
        Dim strLogFile As String
        Dim myXmlNodeList As Xml.XmlNodeList
        Try
            strfn = bc_cs_central_settings.local_template_path + BC_COMPOSITE_CONFIG_FILE
            xmlload.Load(strfn)
            myXmlNodeList = xmlload.SelectNodes("descendant::LogActivityFile")
            If myXmlNodeList.Count > 0 Then
                strLogFile = xmlload.SelectSingleNode("settings/LogActivityFile").InnerXml()
                If strLogFile.ToLower.Trim = "yes" Then
                    LogActivity = True
                Else
                    LogActivity = False
                End If
            Else
                LogActivity = False
            End If
        Catch
            MsgBox("Composite Config File Error: " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
        End Try
    End Function


    Public Function Implications_Bookmark_Name() As String
        Dim myXmlNodeList As Xml.XmlNodeList
        Dim xmlload As New XmlDocument
        Dim strfn As String
        Try
            strfn = bc_cs_central_settings.local_template_path + BC_COMPOSITE_CONFIG_FILE
            xmlload.Load(strfn)
            myXmlNodeList = xmlload.SelectNodes("descendant::ImplicationsBookmarkName")
            If myXmlNodeList.Count > 0 Then
                Return xmlload.SelectSingleNode("settings/Composite/ImplicationsBookmarkName").InnerXml()
            Else
                Return ""
            End If
        Catch
            MsgBox("Composite Config File Error: " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
            Return ""
        End Try

    End Function
    Public Function CapTableCellRGB() As String
        Dim xmlload As New XmlDocument
        Dim strfn As String
        Dim myXmlNodeList As Xml.XmlNodeList
        Try
            strfn = bc_cs_central_settings.local_template_path + BC_COMPOSITE_CONFIG_FILE
            xmlload.Load(strfn)
           
            'capture red & green RGB values
            myXmlNodeList = xmlload.SelectNodes("descendant::StandardAssumptions/TableComponent/RedR")
            If myXmlNodeList.Count > 0 Then
                ComponentiserConstants.RedR = xmlload.SelectSingleNode("/settings/StandardAssumptions/TableComponent/RedR").InnerXml()
            End If
            myXmlNodeList = xmlload.SelectNodes("descendant::StandardAssumptions/TableComponent/RedG")
            If myXmlNodeList.Count > 0 Then
                ComponentiserConstants.RedG = xmlload.SelectSingleNode("/settings/StandardAssumptions/TableComponent/RedG").InnerXml()
            End If
            myXmlNodeList = xmlload.SelectNodes("descendant::StandardAssumptions/TableComponent/RedB")
            If myXmlNodeList.Count > 0 Then
                ComponentiserConstants.RedB = xmlload.SelectSingleNode("/settings/StandardAssumptions/TableComponent/RedB").InnerXml()
            End If
            'capture green values
            myXmlNodeList = xmlload.SelectNodes("descendant::StandardAssumptions/TableComponent/GreenR")
            If myXmlNodeList.Count > 0 Then
                ComponentiserConstants.GreenR = xmlload.SelectSingleNode("/settings/StandardAssumptions/TableComponent/GreenR").InnerXml()
            End If
            myXmlNodeList = xmlload.SelectNodes("descendant::StandardAssumptions/TableComponent/GreenG")
            If myXmlNodeList.Count > 0 Then
                ComponentiserConstants.GreenG = xmlload.SelectSingleNode("/settings/StandardAssumptions/TableComponent/GreenG").InnerXml()
            End If
            myXmlNodeList = xmlload.SelectNodes("descendant::StandardAssumptions/TableComponent/GreenB")
            If myXmlNodeList.Count > 0 Then
                ComponentiserConstants.GreenB = xmlload.SelectSingleNode("/settings/StandardAssumptions/TableComponent/GreenB").InnerXml()
            End If

            myXmlNodeList = xmlload.SelectNodes("descendant::StandardAssumptions/TableComponent/CaptureTableCellRGB")
            If myXmlNodeList.Count > 0 Then
                Return xmlload.SelectSingleNode("/settings/StandardAssumptions/TableComponent/CaptureTableCellRGB").InnerXml()
            Else
                Return "no"
            End If
        Catch
            MsgBox("Composite Config File Error:CapTableCellRGB " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
            Return ""
        End Try
    End Function

    Public Function Componentise_InvInsert_Properties() As Boolean
        Dim myXmlNodeList As Xml.XmlNodeList
        Dim xmlload As New XmlDocument
        Dim strfn As String
        Try
            strfn = bc_cs_central_settings.local_template_path + BC_COMPOSITE_CONFIG_FILE
            xmlload.Load(strfn)
            myXmlNodeList = xmlload.SelectNodes("descendant::InvInsert")
            If myXmlNodeList.Count > 0 Then
                Return True
            Else
                Return False
            End If
        Catch
            MsgBox("Composite Config File Error: " + Err.Description, MsgBoxStyle.Critical, "Blue Curve")
        End Try

    End Function
End Class
