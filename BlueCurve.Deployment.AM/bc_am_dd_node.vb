Imports BlueCurve.Core.AS
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.IO
Imports System.Windows.Forms
Imports System.Xml

Public Class bc_am_dd_node
    Inherits TreeNode

    Private boolIsTable, boolHasBeenVisited, boolInReview, boolIgnore, boolExport, boolArchive, boolExclude, boolExportable, boolSelectAll, boolGranular As Boolean
    Private alKeys As ArrayList
    Private strImport, strTableName, strDisplayColumn, strFileName, strName, strParentOverride, strSelect, strUniquePath As String
    Private xnOutput As XmlNode
    Private xnForeignKey As XmlNode

    Property UseFolderGranularity() As Boolean
        Get
            Return boolGranular
        End Get
        Set(ByVal boolGranular As Boolean)
            Me.boolGranular = boolGranular
        End Set
    End Property

    Property SelectStatement() As String
        Get
            Return strSelect
        End Get
        Set(ByVal strSelect As String)
            Me.strSelect = strSelect
        End Set
    End Property

    Property SelectAll() As Boolean
        Get
            Return boolSelectAll
        End Get
        Set(ByVal boolSelectAll As Boolean)
            Me.boolSelectAll = boolSelectAll
        End Set
    End Property

    ReadOnly Property ParentText() As String
        Get
            If strParentOverride Is Nothing OrElse strParentOverride = "" Then
                Return Parent.Text
            Else
                Return strParentOverride
            End If
        End Get
    End Property

    Property UniquePath() As String
        Get
            Return strUniquePath
        End Get
        Set(ByVal strUniquePath As String)
            Me.strUniquePath = strUniquePath
        End Set
    End Property

    ReadOnly Property FolderName() As String
        Get
            'Return ParentText() & " - " & NodeXml.Name & "\"
            If NodeXml Is Nothing Then
                Return ParentText() & " - " & Text & "\"
            Else
                Return ParentText() & " - " & NodeXml.Name & "\"
            End If
        End Get
    End Property

    ReadOnly Property UniqueIdentifier() As String
        Get
            If Parent Is Nothing Then
                Return Text
            ElseIf OutputName Is Nothing Then
                Return FolderName & Text & "\" & Text
            Else
                Return FolderName & OutputName.TrimEnd & "\" & OutputName
            End If
        End Get
    End Property

    Property ForeignKey() As XmlNode
        Get
            Return xnForeignKey
        End Get
        Set(ByVal xnForeignKey As XmlNode)
            Me.xnForeignKey = xnForeignKey
        End Set
    End Property

    Property Export() As Boolean
        Get
            Return boolExport
        End Get
        Set(ByVal boolExport As Boolean)
            Me.boolExport = boolExport
        End Set
    End Property

    Property Exclude() As Boolean
        Get
            Return boolExclude
        End Get
        Set(ByVal boolExclude As Boolean)
            Me.boolExclude = boolExclude
        End Set
    End Property

    Property Archive() As Boolean
        Get
            Return boolArchive
        End Get
        Set(ByVal boolArchive As Boolean)
            Me.boolArchive = boolArchive
        End Set
    End Property

    Property Keys() As ArrayList
        Get
            Return alKeys
        End Get
        Set(ByVal alKeys As ArrayList)
            Me.alKeys = alKeys
        End Set
    End Property

    Property Exportable() As Boolean
        Get
            Return boolExportable
        End Get
        Set(ByVal boolExportable As Boolean)
            Me.boolExportable = boolExportable
        End Set
    End Property

    Property NodeXml() As XmlNode
        Get
            Return xnOutput
        End Get
        Set(ByVal xnOutput As XmlNode)
            Me.xnOutput = xnOutput
        End Set
    End Property

    Property Reviewing() As Boolean
        Get
            Return boolInReview
        End Get
        Set(ByVal boolInReview As Boolean)
            Me.boolInReview = boolInReview
        End Set
    End Property

    Property Ignore() As Boolean
        Get
            Return boolIgnore
        End Get
        Set(ByVal boolIgnore As Boolean)
            Me.boolIgnore = boolIgnore
        End Set
    End Property

    Property Visited() As Boolean
        Get
            Return boolHasBeenVisited
        End Get
        Set(ByVal boolHasBeenVisited As Boolean)
            Me.boolHasBeenVisited = boolHasBeenVisited
        End Set
    End Property

    Property IsTable() As Boolean
        Get
            Return boolIsTable
        End Get
        Set(ByVal boolIsTable As Boolean)
            Me.boolIsTable = boolIsTable
        End Set
    End Property

    Property FileName() As String
        Get
            If strFileName Is Nothing Then
                Return DisplayColumn
            Else
                Return strFileName
            End If
        End Get
        Set(ByVal strFileName As String)
            Me.strFileName = strFileName
        End Set
    End Property

    ReadOnly Property OutputName() As String
        Get
            If xnOutput Is Nothing Then
                Return Nothing
            ElseIf Not xnOutput.Attributes(FileName) Is Nothing Then
                Return EncodeFileName(xnOutput.Attributes(FileName).Value)
            Else
                Return Nothing
            End If
        End Get
    End Property

    Property DisplayColumn() As String
        Get
            Return strDisplayColumn
        End Get
        Set(ByVal strDisplayColumn As String)
            Me.strDisplayColumn = strDisplayColumn
        End Set
    End Property

    Property ImportProcedure() As String
        Get
            Return strImport
        End Get
        Set(ByVal strImport As String)
            Me.strImport = strImport
        End Set
    End Property

    Property TableName() As String
        Get
            Return strTableName
        End Get
        Set(ByVal strTableName As String)
            Me.strTableName = strTableName
        End Set
    End Property

    Public Overrides Function ToString() As String
        Return Name()
    End Function

    'End Properties

    Public Sub New(ByVal strName As String, ByVal boolIgnore As Boolean)
        MyBase.New(strName)
        Me.Text = strName
        Me.Ignore = boolIgnore
    End Sub

    Public Sub New(ByVal strName As String, ByVal strImport As String, ByVal strTableName As String, ByVal strDisplayColumn As String, ByVal boolIsTable As Boolean, ByVal strFileName As String, ByVal strFullPath As String, ByVal strParentOverride As String, ByVal alKeys As ArrayList)
        MyBase.New(strName)
        Me.Name = strFullPath & "\" & strName
        Me.Text = strName
        Me.strImport = strImport
        Me.DisplayColumn = strDisplayColumn
        Me.TableName = strTableName
        Me.IsTable = boolIsTable
        Me.strFileName = strFileName
        Me.strParentOverride = strParentOverride
        'Me.boolExportable = boolExportable
        Me.alKeys = alKeys
    End Sub

    Function GetFileName() As String
        If Not EncodeFileName(NodeXml.Attributes(FileName).Value).Trim = "" Then
            Return EncodeFileName(NodeXml.Attributes(FileName).Value)
        Else
            Return ""
        End If
    End Function

    Function GetDirectory(ByVal strFilePath As String) As String
        Dim strReturn As String = strFilePath & FolderName
        If UseFolderGranularity Then
            strReturn = strReturn & GetFileName()
        End If
        Return strReturn
    End Function

    Function GetFullPath(ByVal strFilePath As String) As String

        Dim strFullFile As String = Nothing
        Dim strFileName As String = GetFileName()

        Dim strDirectory As String = GetDirectory(strFilePath)

        If Not strFileName Is Nothing Then
            strFullFile = strDirectory & "\" & strFileName
        End If

        Dim strExtension As String = ".xml"
        If Archive Then
            strExtension = ".NOLONGERNEEDED"
        End If

        strFullFile = strFullFile & strExtension

        Return strFullFile

    End Function

    Public Sub ExportNode(ByVal strFilePath As String, ByRef alStoredData As ArrayList, ByVal boolOverwriteAll As Boolean)

        If Not Exclude AndAlso Not IsTable Then

            If Not NodeXml Is Nothing And Exportable Then

                For Each xnChild As XmlNode In NodeXml
                    xnChild.ParentNode.RemoveChild(xnChild)
                Next

                CType(NodeXml, XmlElement).IsEmpty = True

                Dim strDirectory As String = GetDirectory(strFilePath)
                If Not Directory.Exists(strDirectory) Then
                    Directory.CreateDirectory(strDirectory)
                End If

                Dim strFullFile As String = GetFullPath(strFilePath)
                If Not strFullFile Is Nothing Then
                    Dim strTarget As String = strFullFile
                    If boolOverwriteAll OrElse Not File.Exists(strTarget) OrElse Not ReadFile(strTarget) = NodeXml.OuterXml Then
                        WriteFile(strTarget, NodeXml.OuterXml)
                    End If
                End If

            End If

        Else

            If Not Parent Is Nothing AndAlso TypeOf Parent Is bc_am_dd_node AndAlso Not CType(Parent, bc_am_dd_node).NodeXml Is Nothing AndAlso CType(Parent, bc_am_dd_node).NodeXml.ChildNodes.Count > 0 Then
                NodeXml = CType(Parent, bc_am_dd_node).NodeXml
            End If

        End If

    End Sub

End Class
