Imports System.IO
Imports System.Xml
Imports System.Windows.Forms
Imports BlueCurve.Core.AS
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM

Friend Module bc_am_dd_tools

    Const boolDebug As Boolean = False

    Private Const WM_SETREDRAW = &HB
    Declare Auto Function SendMessage Lib "user32.dll" (ByVal hWnd As IntPtr, ByVal msg As Integer, ByVal wParam As IntPtr, ByVal lParam As IntPtr) As IntPtr

    ReadOnly Property Debugging() As Boolean
        Get
            Return boolDebug
        End Get
    End Property

    Friend Sub SuspendDrawing(ByVal c As Control)
        SendMessage(c.Handle, WM_SETREDRAW, IntPtr.Zero, IntPtr.Zero)
    End Sub

    Friend Sub ResumeDrawing(ByVal c As Control)
        SendMessage(c.Handle, WM_SETREDRAW, New IntPtr(1), IntPtr.Zero)
    End Sub

    Friend Function BitToBool(ByVal i As Integer) As Boolean
        Return i = 1
    End Function

    Friend Function BoolToBit(ByVal bool As Boolean) As Integer
        Return Math.Abs(CType(bool, Integer))
    End Function

    Friend Function GetXmlNodeFromQuery(ByRef alStoredData As ArrayList, ByVal strCommand As String, _
                                        ByVal strTableName As String, ByVal alKeys As ArrayList) As XmlNodeList

        'Dim METHOD_NAME As String = System.Reflection.MethodBase.GetCurrentMethod().Name
        'Dim log = New bc_cs_activity_log("bc_am_dd_tools", METHOD_NAME, bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim badsd As bc_am_dd_stored_data = Nothing

        For Each sd As bc_am_dd_stored_data In alStoredData
            If sd.Query = strCommand Then
                badsd = sd
                Exit For
            End If
        Next

        If badsd Is Nothing Then

            Dim osql As bc_om_sql = bc_as_tools.QueryDatabase(strCommand, True)

            If osql.success And Not osql.results Is Nothing Then
                Dim xd As XmlDocument = Nothing
                If osql.results(0, 0) <> "" Then
                    xd = New XmlDocument
                    xd.PreserveWhitespace = True
                    xd.LoadXml(osql.results(0, 0))
                End If
                badsd = New bc_am_dd_stored_data(strCommand, strTableName, xd)
                alStoredData.Add(badsd)
            ElseIf osql.success Then
            Else
                'Error
            End If

        End If

        If badsd.Xml Is Nothing Then
            Return New bc_am_dd_xnl
        End If

        If alKeys Is Nothing OrElse alKeys.Count = 0 Then
            Return badsd.Xml.ChildNodes(0).ChildNodes
        End If

        Dim badx As New bc_am_dd_xnl()

        For Each xn As XmlNode In badsd.Nodes
            Dim boolTrig = True
            For Each badk As bc_am_dd_key In alKeys
                If xn.Attributes(badk.KeyName) Is Nothing OrElse xn.Attributes(badk.KeyName).Value <> badk.KeyValue Then
                    boolTrig = False
                End If
            Next
            If boolTrig Then
                badx.Add(xn)
            End If
        Next

        'log = New bc_cs_activity_log("bc_am_dd_tools", METHOD_NAME, bc_cs_activity_codes.TRACE_EXIT, "")

        Return badx

    End Function

    Friend Function FixCapitalisation(ByVal tag As String) As String
        Dim i As Integer = 0
        Dim charArray As Char() = tag.ToCharArray()
        Dim charPrevious As Char
        Dim output As String = ""
        While i < charArray.GetLength(0)
            If charPrevious = " " Or i = 0 Then
                charPrevious = charArray(i).ToString.ToUpper()
            Else
                charPrevious = charArray(i)
            End If
            If charPrevious = "_" Then
                charPrevious = " "
            End If
            output = output & charPrevious
            i += 1
        End While
        Return output
    End Function

    Friend Function ReadFile(ByVal strFileName As String) As String
        Using reader As StreamReader = New StreamReader(strFileName)
            Return reader.ReadToEnd
        End Using
    End Function

    Friend Sub WriteFile(ByVal strFileName As String, ByVal strContents As String)

        Dim strDirectory As String = ""
        Dim i As Integer = 0
        Dim strChunks() As String = strFileName.Split("\")
        While i < strChunks.Length - 1
            If Not i = 0 Then
                strDirectory = strDirectory & "\"
            End If
            strDirectory = strDirectory & strChunks(i)
            i += 1
        End While

        If Not Directory.Exists(strDirectory) Then
            Directory.CreateDirectory(strDirectory)
        End If

        Using writer As StreamWriter = New StreamWriter(strFileName)
            writer.Write(strContents)
        End Using

    End Sub

    Friend Function EncodeFileName(ByVal strFileName As String, Optional ByVal boolSaveBackSlash As Boolean = False) As String
        If strFileName Is Nothing Then
            strFileName = ""
        End If
        strFileName = strFileName.Replace("%", "%" & Asc("%"))
        For Each c In IO.Path.GetInvalidFileNameChars
            strFileName = strFileName.Replace(c, "%" & Asc(c))
        Next
        For Each c In IO.Path.GetInvalidPathChars
            strFileName = strFileName.Replace(c, "%" & Asc(c))
        Next
        If boolSaveBackSlash Then
            strFileName = Replace(strFileName, "%92", "\")
        End If
        Return strFileName
    End Function

    Friend Function SanitizeTagName(ByVal strTagName As String) As String
        Return XmlConvert.EncodeName(Replace(strTagName.ToLower, " ", "_"))
    End Function

    Public Sub GetFileCount(ByVal strSourceDirectory As String, ByRef intCount As Integer, Optional ByVal intLevel As Integer = 0)

        intCount = 150000

        Exit Sub

        'Dim strDirectories As String() = Directory.GetDirectories(strSourceDirectory)
        'Dim boolHasDirectory As Boolean = False
        'If intLevel < 1 Then
        '    For Each strDirectory As String In strDirectories
        '        If (File.GetAttributes(strDirectory) And FileAttributes.ReparsePoint) <> FileAttributes.ReparsePoint Then
        '            GetFileCount(strDirectory, intCount, intLevel + 1)
        '            intCount += 1
        '            boolHasDirectory = True
        '        End If
        '    Next
        'Else
        '    intCount += strDirectories.Length * 2
        'End If

        'If Not boolHasDirectory Then
        '    intCount += 1
        'End If

    End Sub

    Friend Function GetNodeWithoutAttributeCount(ByVal xn As XmlNode, ByVal strAttributeName As String) As Integer
        Dim count As Integer = BoolToBit(xn.Attributes(strAttributeName) Is Nothing)
        If Not xn.SelectSingleNode("display") Is Nothing Then
            For Each xnChild As XmlNode In xn.SelectSingleNode("display").ChildNodes
                count += GetNodeWithoutAttributeCount(xnChild, strAttributeName)
            Next
        End If
        Return count
    End Function

    Friend Sub DebugLine(ByVal strMessage As String)
        If boolDebug Then
            WriteToConsole(strMessage)
        End If
    End Sub

    Friend Function GetPercentage(ByVal intIndex As Integer, ByVal intCount As Integer) As Decimal
        Return Math.Max(1, CType(intIndex, Decimal) / intCount * 100)
    End Function

    Friend Sub IncrementProgress(ByRef badm As bc_am_dd_main, ByVal strMessage As String, ByVal decIndex As Decimal, ByVal decCount As Decimal)
        IncrementProgress(badm, strMessage, decIndex, decCount, False)
    End Sub

    Friend Sub IncrementProgress(ByRef badm As bc_am_dd_main, ByVal decIndex As Decimal, ByVal decCount As Decimal)
        IncrementProgress(badm, Nothing, decIndex, decCount, False)
    End Sub

    Dim strLastMessage As String
    Friend Sub IncrementProgress(ByRef badm As bc_am_dd_main, ByVal strMessage As String, ByVal decIndex As Decimal, ByVal decCount As Decimal, ByVal boolOverride As Boolean)
        Try
            Dim intModFactor = Math.Max(CType(decCount / 200, Integer), 1)
            If boolOverride OrElse decIndex Mod intModFactor = 0 Then
                Dim decPercent As Decimal = decIndex / decCount * 100
                'If Not decSubCount = 0 Then
                '    decPercent += decSubIndex / decSubCount * 100 / decCount
                'End If
                decPercent = Math.Min(Math.Max(decPercent, 1), 100)
                If Not badm Is Nothing AndAlso Not bc_cs_central_settings.progress_bar Is Nothing Then
                    If Not strMessage Is Nothing Then
                        bc_cs_central_settings.progress_bar.change_caption(strMessage)
                    End If
                    bc_cs_central_settings.progress_bar.increment(decPercent)
                Else
                    If Not strMessage Is Nothing AndAlso Not strMessage.Trim = "" AndAlso strLastMessage <> strMessage Then
                        Console.WriteLine()
                        WriteToConsole(strMessage)
                        strLastMessage = strMessage
                    End If
                    Console.Write(CInt(decPercent) & "% done." & ControlChars.Cr)
                End If
                Application.DoEvents()
            End If
        Catch ex As Exception
        End Try
    End Sub

    Friend Function DisplayMessage(ByRef badm As bc_am_dd_main, ByVal strMessage As String, _
                              ByVal mbb As MessageBoxButtons, ByVal mbi As MessageBoxIcon) As DialogResult
        If Not badm Is Nothing Then
            Return badm.ShowMessageBox(strMessage, mbb, mbi)
        Else
            WriteToConsole(strMessage)
            Return Nothing
        End If
        Application.DoEvents()
    End Function

    Friend Sub UnloadProgressBar(ByRef badm As bc_am_dd_main)

        If Not badm Is Nothing Then
            'badm.Enabled = True
            badm.Refresh()
        End If

        If Not bc_cs_central_settings.progress_bar Is Nothing Then
            bc_cs_central_settings.progress_bar.unload()
        End If

    End Sub

    Friend Sub CreateProgressBar(ByRef badm As bc_am_dd_main, ByVal strMessage As String)
        If Not badm Is Nothing Then
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve Deployment", strMessage, 4, False, True)
            bc_cs_central_settings.progress_bar.SetTopmost()
            bc_cs_central_settings.progress_bar.increment(1D)
            'badm.Enabled = False
        Else
            WriteToConsole(strMessage)
        End If
    End Sub

    Friend Function FindTreeNode(ByVal tv As TreeView, ByVal strPath As String)
        Dim strLevels() As String = strPath.Split("\")
        Dim tn As TreeNode = tv.Nodes(0)
        Dim intIndex As Integer = 1
        While intIndex < strLevels.Length
            For Each tnChild As TreeNode In tn.Nodes
                If tnChild.Text = strLevels(intIndex) Then
                    tn = tnChild
                    Exit For
                End If
            Next
            intIndex += 1
        End While
        Return tn
    End Function

    Friend Function GetKeys(ByVal xd As XmlDocument, ByVal strNodeName As String) As ArrayList

        Dim alKeys As New ArrayList

        Dim xnlKeys As XmlNodeList = xd.GetElementsByTagName(strNodeName)
        If xnlKeys.Count > 0 Then
            For Each xn As XmlNode In xnlKeys
                For Each xnKey As XmlNode In xn.SelectNodes("keys/key")
                    alKeys.Add(New bc_am_dd_key(xnKey.InnerXml, Nothing))
                Next
            Next
        End If

        Return alKeys

    End Function

    Friend Function AddNodeToDocument(ByRef xn As XmlNode, ByVal strName As String, ByVal strValue As String)
        Dim xd As XmlDocument = xn.OwnerDocument
        Dim xnChild As XmlNode = xd.CreateElement(strName)
        xn.AppendChild(xnChild)
        xnChild.InnerText = strValue
        Return xn
    End Function

    Public Function isValidFile(ByVal strFile As String) As Boolean
        Try
            Dim fi As New FileInfo(strFile)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function isValidDirectory(ByVal strDirectory As String) As Boolean
        Try
            Return Path.IsPathRooted(strDirectory)
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Sub WriteToConsole(ByVal strMessage As String)
        Console.WriteLine(strMessage)
    End Sub

    Public Function IsParent(ByRef badnParent As bc_am_dd_node, ByVal badnChild As bc_am_dd_node, ByVal strParent As String, ByVal strParentColumn As String, ByVal strChildColumn As String) As Boolean
        If badnParent.NodeXml.Name = strParent Then
            If Not badnParent.NodeXml Is Nothing AndAlso Not badnParent.NodeXml.Attributes(strParentColumn) Is Nothing Then
                If badnParent.NodeXml.Attributes(strParentColumn).Value = badnChild.NodeXml.Attributes(strChildColumn).Value Then
                    Return True
                End If
            End If
        End If
        Return False
    End Function

    Friend Sub UpdateList(ByRef alListByCommand As ArrayList, ByRef badn As bc_am_dd_node, ByVal boolAdd As Boolean, ByVal strKeyName As String, ByVal strTableName As String)
        If Not badn.ImportProcedure Is Nothing Then
            Dim badl As bc_am_dd_named_list = Nothing
            Dim boolFound As Boolean = False
            For Each badl In alListByCommand
                If badl.Name = strTableName Then
                    boolFound = True
                    Exit For
                End If
            Next
            If Not boolFound Then
                badl = New bc_am_dd_named_list(strTableName)
                alListByCommand.Add(badl)
            End If
            Dim strKey As String = Nothing
            If strKeyName Is Nothing Then
                strKey = badn.UniqueIdentifier
            Else
                strKey = badn.NodeXml.Attributes(strKeyName).Value
            End If
            If boolAdd Then 'And Not badl.Contains(badn) Then
                badl.Add(strKey, badn)
            Else
                badl.Remove(strKey)
            End If
        End If
    End Sub

End Module
