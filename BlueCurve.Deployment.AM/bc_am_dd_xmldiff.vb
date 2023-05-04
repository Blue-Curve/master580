Imports System.IO
Imports System.Xml
Imports System.Text

Module bc_am_dd_xmldiff

    Friend Sub PickNodes(ByRef xn As XmlNode, ByRef xnParent As XmlNode, ByVal xdKeys As XmlDocument, ByVal qt As QueryType)
        If xn.HasChildNodes Then
            For Each xnChild As XmlNode In xn.ChildNodes
                PickNodes(xnChild, xnParent, xdKeys, qt)
            Next
        ElseIf Not xn.Attributes Is Nothing AndAlso xn.Attributes.Count > 0 Then
            Dim xnTransaction As XmlNode = xnParent.OwnerDocument.CreateElement("transaction")
            'Dim xaType As XmlAttribute = xnParent.OwnerDocument.CreateAttribute("type")
            'xaType.Value = qt.ToString
            'xnTransaction.Attributes.Append(xaType)
            Dim xnKey As XmlNode = xdKeys.SelectSingleNode("//" & xn.Name)
            'Dim xaDepth As XmlAttribute = xnTransaction.OwnerDocument.CreateAttribute("depth")
            'If Not xnKey Is Nothing AndAlso Not xnKey.Attributes("depth") Is Nothing Then
            '    xaDepth.Value = xnKey.Attributes("depth").Value
            'Else
            '    xaDepth.Value = 0
            'End If
            'xnTransaction.Attributes.Append(xaDepth)
            AddDepthAndType(xnTransaction, xnKey, qt)
            xnTransaction.InnerXml = "<destination>" & xn.OuterXml & "<location>" & xn.ParentNode.ParentNode.ChildNodes(1).ChildNodes(0).InnerText & "</location></destination>"
            xnParent.AppendChild(xnTransaction)
        End If
    End Sub

    Friend Sub CheckDependencies(ByRef xn As XmlNode, ByRef xdKeys As XmlDocument, ByVal qt As QueryType, ByRef xdRemovedNodes As XmlDocument, ByRef xdResults As XmlDocument)
        If xn.ChildNodes.Count > 0 Then
            Dim strNodeName As String = xn.ChildNodes(0).Name
            Dim xnData As XmlNode = xn.ChildNodes(0)
            Dim xnlForeignKeys As XmlNode = xdKeys.SelectSingleNode("//" & strNodeName & "/foreign_keys")
            If Not xnlForeignKeys Is Nothing Then
                For Each xnForeignKey As XmlNode In xnlForeignKeys
                    If strNodeName = xnForeignKey.SelectSingleNode("parent/table").InnerText And qt = QueryType.DELETE Then
                        Dim strChildTable As String = xnForeignKey.SelectSingleNode("child/table").InnerText
                        Dim strChildColumn As String = xnForeignKey.SelectSingleNode("child/column").InnerText
                        Dim strParentColumn As String = xnForeignKey.SelectSingleNode("parent/column").InnerText
                        Dim xnlPossibleChildren As XmlNodeList = xdRemovedNodes.SelectNodes("//" & strChildTable & "[@" & strChildColumn & "=""" & xnData.Attributes(strParentColumn).Value & """]")
                        Dim xnlResultsPossibleChildrenDelete As XmlNodeList = xdResults.SelectNodes("//transaction[@type=""DELETE""]/destination/" & strChildTable & "[@" & strChildColumn & "=""" & xnData.Attributes(strParentColumn).Value & """]")
                        If xnlPossibleChildren.Count > 0 And Not xnlPossibleChildren.Count = xnlResultsPossibleChildrenDelete.Count Then
                            Throw New Exception("Transaction would violate system integrity." & ControlChars.Lf & xnData.OuterXml)
                        End If
                    ElseIf strNodeName = xnForeignKey.SelectSingleNode("child/table").InnerText And (qt = QueryType.INSERT Or qt = QueryType.UPDATE) Then
                        Dim strParentTable As String = xnForeignKey.SelectSingleNode("parent/table").InnerText
                        Dim strParentColumn As String = xnForeignKey.SelectSingleNode("parent/column").InnerText
                        Dim strChildColumn As String = xnForeignKey.SelectSingleNode("child/column").InnerText
                        If Not xnData.Attributes(strChildColumn) Is Nothing Then
                            Dim xnlPossibleParents As XmlNodeList = xdRemovedNodes.SelectNodes("//" & strParentTable & "[@" & strParentColumn & "=""" & xnData.Attributes(strChildColumn).Value & """]")
                            Dim xnlResultsPossibleParentsInsert As XmlNodeList = xdResults.SelectNodes("//transaction[@type=""INSERT""]/destination/" & strParentTable & "[@" & strParentColumn & "=""" & xnData.Attributes(strChildColumn).Value & """]")
                            Dim xnlResultsPossibleParentsUpdate As XmlNodeList = xdResults.SelectNodes("//transaction[@type=""UPDATE""]/destination/" & strParentTable & "[@" & strParentColumn & "=""" & xnData.Attributes(strChildColumn).Value & """]")
                            If xnlPossibleParents.Count = 0 And xnlResultsPossibleParentsInsert.Count = 0 And xnlResultsPossibleParentsUpdate.Count = 0 Then
                                Throw New Exception("Transaction would violate system integrity." & ControlChars.Lf & xnData.OuterXml)
                            End If
                        End If
                        End If
                Next
            End If
        End If
    End Sub
    Friend Sub AddDepthAndType(ByRef xnTransaction As XmlNode, ByVal xnKey As XmlNode, ByVal qt As QueryType)
        Dim xaDepth As XmlAttribute = xnTransaction.OwnerDocument.CreateAttribute("depth")
        If Not xnKey Is Nothing AndAlso Not xnKey.Attributes("depth") Is Nothing Then
            xaDepth.Value = xnKey.Attributes("depth").Value
        Else
            xaDepth.Value = 0
        End If
        xnTransaction.Attributes.Append(xaDepth)
        Dim xaType As XmlAttribute = xnTransaction.OwnerDocument.CreateAttribute("type")
        xaType.Value = qt.ToString
        xnTransaction.Attributes.Append(xaType)
    End Sub

    'Friend Sub CompareFolders(ByVal view As bc_am_dd_main, ByVal intIndex As Integer, ByVal intCount As Integer, ByVal strSourceDirectory As String, ByVal strDestinationDirectory As String)

    '    IncrementProgress(view, intIndex, intCount)

    '    intIndex += 1

    '    Dim strFiles As String() = Directory.GetFiles(strSourceDirectory)
    '    For Each strFileName As String In strFiles
    '        If strFileName.IndexOf(".xml") = strFileName.Length - 4 Then
    '            Dim strDestinationFile As String = strFileName.Replace(strSourceDirectory, strDestinationDirectory)
    '            If Not File.Exists(strDestinationFile) OrElse ReadFile(strFileName) <> ReadFile(strDestinationFile) Then

    '            End If
    '        End If
    '    Next

    '    Dim strDirectories As String() = Directory.GetDirectories(strSourceDirectory)
    '    For Each strDirectory As String In strDirectories
    '        If (File.GetAttributes(strDirectory) And FileAttributes.ReparsePoint) <> FileAttributes.ReparsePoint Then
    '            CompareFolders(view, strSourceDirectory, strDestinationDirectory, intIndex, intCount)
    '        End If
    '    Next

    'End Sub

    Friend Sub CompareDocument(ByRef xnSource As XmlNode, ByRef xdDestination As XmlDocument, ByVal strPath As String, ByRef xdKeys As XmlDocument, ByRef xdOutput As XmlDocument, ByRef alNodesToRemove As ArrayList, ByRef xdRemovedNodes As XmlDocument)

        Dim d As DateTime = DateTime.Now
        DebugLine(d)

        For Each xnSourceChild As XmlNode In xnSource.ChildNodes
            If xnSource.Name <> "location" Then
                CompareDocument(xnSourceChild, xdDestination, strPath & "/" & xnSource.Name, xdKeys, xdOutput, alNodesToRemove, xdRemovedNodes)
            End If
        Next

        For Each xn As XmlNode In alNodesToRemove
            If xn.ChildNodes.Count = 0 Then
                xdRemovedNodes.ChildNodes(0).AppendChild(xdRemovedNodes.ImportNode(xn, True))
                xn.ParentNode.RemoveChild(xn)
            End If
        Next

        alNodesToRemove.Clear()

        Dim boolFound As Boolean = False
        Dim boolNodeExists As Boolean

        boolFound = True
        boolNodeExists = False

        'Dim xnDestination As XmlNode

        Dim alKeys As ArrayList = GetKeys(xdKeys, xnSource.Name)

        'DebugLine(strPath & "/" & xnSource.Name)

        'Using reader As XmlNodeReader = New XmlNodeReader(xdDestination)

        '    While Not reader.EOF

        '        boolFound = True
        '        boolNodeExists = False

        '        If reader.NodeType = XmlNodeType.Element AndAlso reader.Name = xnSource.Name Then

        '            boolFound = True
        '            boolNodeExists = True

        '            If xnSource.Attributes.Count > 0 Then

        '                boolFound = False

        '                If alKeys.Count = 0 Then
        '                    boolFound = True
        '                Else
        '                    For Each badk In alKeys
        '                        If Not xnSource.Attributes(badk.keyname) Is Nothing AndAlso Not xnDestination.Attributes(badk.keyname) Is Nothing _
        '                            AndAlso xnSource.Attributes(badk.keyname).Value = xnDestination.Attributes(badk.keyname).Value Then
        '                            boolFound = True
        '                        Else
        '                            boolFound = False
        '                            Exit For
        '                        End If
        '                    Next
        '                End If

        '            End If

        '            For Each xaSource As XmlAttribute In xnSource.Attributes

        '                If xnDestination.Attributes(xaSource.Name) Is Nothing OrElse xaSource.Value <> xnDestination.Attributes(xaSource.Name).Value Then
        '                    If boolFound Then
        '                        Dim xnUpdate As XmlNode = xdOutput.SelectSingleNode("/root")
        '                        Dim xnTransaction As XmlNode = xnUpdate.OwnerDocument.CreateElement("transaction")
        '                        Dim xnKey As XmlNode = xdKeys.SelectSingleNode("//" & xnSource.Name)
        '                        AddDepthAndType(xnTransaction, xnKey, QueryType.UPDATE)
        '                        xnTransaction.InnerXml = "<source>" & xnSource.OuterXml & "</source><destination>" & xnDestination.OuterXml & "</destination>" '& _
        '                        xnUpdate.AppendChild(xnTransaction)
        '                    End If
        '                    Exit For
        '                End If
        '                'found
        '            Next

        '            If boolFound Then
        '                For Each xaDestination As XmlAttribute In xnDestination.Attributes
        '                    If xnSource.Attributes(xaDestination.Name) Is Nothing OrElse xaDestination.Value <> xnSource.Attributes(xaDestination.Name).Value Then
        '                        Dim xnUpdate As XmlNode = xdOutput.SelectSingleNode("/root")
        '                        Dim xnTransaction As XmlNode = xnUpdate.OwnerDocument.CreateElement("transaction")
        '                        Dim xnKey As XmlNode = xdKeys.SelectSingleNode("//" & xnSource.Name)
        '                        AddDepthAndType(xnTransaction, xnKey, QueryType.UPDATE)
        '                        xnTransaction.InnerXml = "<source>" & xnSource.OuterXml & "</source><destination>" & xnDestination.OuterXml & "</destination>" '& _
        '                        xnUpdate.AppendChild(xnTransaction)
        '                    End If
        '                Next
        '            End If
        '            If boolFound And Not xnSource.HasChildNodes Then
        '                alNodesToRemove.Add(xnSource)
        '                alNodesToRemove.Add(xnDestination)
        '                Exit While
        '            End If
        '        End If

        '        reader.Read()
        '    End While

        'End Using

        For Each xnDestination In xdDestination.SelectNodes(strPath & "/" & xnSource.Name)

            boolFound = True
            boolNodeExists = False

            If xnSource.Name = xnDestination.Name Then

                boolFound = True
                boolNodeExists = True

                If xnSource.Attributes.Count > 0 Then

                    boolFound = False

                    If alKeys.Count = 0 Then
                        boolFound = True
                    Else
                        For Each badk In alKeys
                            If Not xnSource.Attributes(badk.keyname) Is Nothing AndAlso Not xnDestination.Attributes(badk.keyname) Is Nothing _
                                AndAlso xnSource.Attributes(badk.keyname).Value = xnDestination.Attributes(badk.keyname).Value Then
                                boolFound = True
                            Else
                                boolFound = False
                                Exit For
                            End If
                        Next
                    End If

                End If

                For Each xaSource As XmlAttribute In xnSource.Attributes

                    If xnDestination.Attributes(xaSource.Name) Is Nothing OrElse xaSource.Value <> xnDestination.Attributes(xaSource.Name).Value Then
                        If boolFound Then
                            Dim xnUpdate As XmlNode = xdOutput.SelectSingleNode("/root")
                            Dim xnTransaction As XmlNode = xnUpdate.OwnerDocument.CreateElement("transaction")
                            Dim xnKey As XmlNode = xdKeys.SelectSingleNode("//" & xnSource.Name)
                            AddDepthAndType(xnTransaction, xnKey, QueryType.UPDATE)
                            xnTransaction.InnerXml = "<source>" & xnSource.OuterXml & "</source><destination>" & xnDestination.OuterXml & "</destination>" '& _
                            xnUpdate.AppendChild(xnTransaction)
                        End If
                        Exit For
                    End If
                    'found
                Next

                'If boolFound AndAlso xnSource.OuterXml <> xnDestination.OuterXml Then
                '    Dim xnUpdate As XmlNode = xdOutput.SelectSingleNode("/root")
                '    Dim xnTransaction As XmlNode = xnUpdate.OwnerDocument.CreateElement("transaction")
                '    Dim xnKey As XmlNode = xdKeys.SelectSingleNode("//" & xnSource.Name)
                '    AddDepthAndType(xnTransaction, xnKey, QueryType.UPDATE)
                '    xnTransaction.InnerXml = "<source>" & xnSource.OuterXml & "</source><destination>" & xnDestination.OuterXml & "</destination>" '& _
                '    xnUpdate.AppendChild(xnTransaction)
                'End If

                If boolFound Then
                    For Each xaDestination As XmlAttribute In xnDestination.Attributes
                        If xnSource.Attributes(xaDestination.Name) Is Nothing OrElse xaDestination.Value <> xnSource.Attributes(xaDestination.Name).Value Then
                            Dim xnUpdate As XmlNode = xdOutput.SelectSingleNode("/root")
                            Dim xnTransaction As XmlNode = xnUpdate.OwnerDocument.CreateElement("transaction")
                            Dim xnKey As XmlNode = xdKeys.SelectSingleNode("//" & xnSource.Name)
                            AddDepthAndType(xnTransaction, xnKey, QueryType.UPDATE)
                            xnTransaction.InnerXml = "<source>" & xnSource.OuterXml & "</source><destination>" & xnDestination.OuterXml & "</destination>" '& _
                            xnUpdate.AppendChild(xnTransaction)
                        End If
                    Next
                End If
                If boolFound And Not xnSource.HasChildNodes Then
                    alNodesToRemove.Add(xnSource)
                    alNodesToRemove.Add(xnDestination)
                    Exit For
                End If
            End If

        Next

        DebugLine(DateTime.Now.Subtract(d).ToString)

    End Sub

    Public Sub IterateFiles(ByRef view As bc_am_dd_main, ByRef ht As Hashtable, ByRef xd As XmlDocument, ByRef xdKeys As XmlDocument, ByRef sbRemovedNodes As StringBuilder, ByVal strDirectory As String, ByVal strBaseDirectory As String, ByVal strPairedDirectory As String, ByVal qt As QueryType, ByVal boolIsSource As Boolean, ByRef intIndex As Integer, ByVal intCount As Integer)

        IncrementProgress(view, intIndex, intCount)

        intIndex += 1

        Dim strFiles As String() = Directory.GetFiles(strDirectory)
        For Each strFileName As String In strFiles            
            If strFileName.IndexOf(".xml") = strFileName.Length - 4 And Not ht.Contains(strFileName) Then
                Dim strPairedFile As String = Replace(strFileName, strBaseDirectory, strPairedDirectory)
                Dim boolExists As Boolean = File.Exists(strPairedFile)
                Dim strFileContents As String = Nothing
                Dim strPairedFileContents As String = Nothing
                strFileContents = ReadFile(strFileName)
                If File.Exists(strPairedFile) Then
                    strPairedFileContents = ReadFile(strPairedFile)
                End If
                Dim boolEquals As Boolean = Not strPairedFileContents Is Nothing AndAlso strFileContents = strPairedFileContents
                Dim xnTransaction, xnKey As XmlNode
                xnTransaction = Nothing
                Dim xdSource As XmlDocument = Nothing
                Dim tQt As QueryType = qt
                If Not boolExists OrElse Not boolEquals Then
                    xdSource = New XmlDocument
                    xdSource.LoadXml(strFileContents)
                    xnTransaction = xd.CreateElement("transaction")
                    xnKey = xdKeys.SelectSingleNode("//" & xdSource.DocumentElement.Name)
                    If boolExists And Not boolEquals Then
                        tQt = QueryType.UPDATE
                    End If
                    AddDepthAndType(xnTransaction, xnKey, tQt)
                    xd.DocumentElement.AppendChild(xnTransaction)
                    Dim xnlKeys As XmlNodeList = xdKeys.SelectNodes("//" & xdSource.DocumentElement.Name & "[@import]")
                    If xnlKeys.Count > 0 AndAlso Not xnlKeys(0).Attributes("import") Is Nothing Then
                        Dim xa As XmlAttribute = xd.CreateAttribute("import")
                        xa.Value = xnlKeys(0).Attributes("import").Value
                        xnTransaction.Attributes.Append(xa)
                    Else
                        'Error
                        Dim s As String = ""
                    End If
                End If
                If Not boolExists Then
                    xnTransaction.InnerXml = "<destination>" & strFileContents & "<location>" & XmlConvert.EncodeName(strFileName) & "</location></destination>"
                ElseIf boolExists And Not boolEquals Then
                    If boolIsSource Then
                        xnTransaction.InnerXml = "<source>" & strFileContents & "<location>" & XmlConvert.EncodeName(strFileName) & _
                            "</location></source><destination>" & strPairedFileContents & "<location>" & XmlConvert.EncodeName(strPairedFile) & "</location></destination>"
                    Else
                        xnTransaction.InnerXml = "<source>" & strPairedFileContents & "<location>" & XmlConvert.EncodeName(strPairedFile) & _
                            "</location></source><destination>" & strFileContents & "<location>" & XmlConvert.EncodeName(strFileName) & "</location></destination>"
                    End If
                Else
                    sbRemovedNodes.Append(strFileContents)
                    If Not strPairedFileContents Is Nothing Then
                        sbRemovedNodes.Append(strPairedFileContents)
                    End If
                End If
                ht.Add(strFileName, strFileName)
                ht.Add(strPairedFile, strPairedFile)
            End If
        Next

        Dim strDirectories As String() = Directory.GetDirectories(strDirectory)
        For Each strSubDirectory As String In strDirectories
            If (File.GetAttributes(strDirectory) And FileAttributes.ReparsePoint) <> FileAttributes.ReparsePoint Then
                IterateFiles(view, ht, xd, xdKeys, sbRemovedNodes, strSubDirectory, strBaseDirectory, strPairedDirectory, qt, boolIsSource, intIndex, intCount)
            End If
        Next

    End Sub

End Module
