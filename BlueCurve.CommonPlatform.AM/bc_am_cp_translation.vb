
Imports System.Collections
Imports System.IO
Imports System.Windows.Forms
Imports System.Windows.Forms.VisualStyles
Imports System.Drawing
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS

Public Class bc_am_cp_translation

    Public controller As Object
    Public TranslationGroups As New bc_om_translation_groups
    Public TranslationItems As New bc_om_translation_items
    Private CurrentGroup As Long
    Private LoadingGrid As Boolean = True
    Private CurrentFilter As String = ""

    Public Sub LoadTranslationGroups()

        Dim selectGroup As Long
        Dim groupId As Long
        Dim entityTableId As Long
        Dim selectItem As Long
        Dim itemDesc As String

        Try

            If CurrentGroup = 0 Then
                If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                    TranslationGroups.db_read()
                ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                    TranslationGroups.tmode = bc_cs_soap_base_class.tREAD
                    If TranslationGroups.transmit_to_server_and_receive(TranslationGroups, True) = False Then
                        Exit Sub
                    End If
                Else
                    Dim omsg As New bc_cs_message("Blue Curve", "Cannot connect to " + bc_cs_central_settings.soap_server + " application cannot be launched.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                End If

                RefreshGroups()

                If uxGroupTree.Nodes.Count > 0 Then
                    REM Setup grid
                    groupId = 0
                    entityTableId = 0
                    itemDesc = ""
                    If uxGroupTree.Nodes(1).Level <> 0 Then
                        selectGroup = uxGroupTree.Nodes(0).Parent.Tag
                        itemDesc = uxGroupTree.Nodes(0).Text
                        selectItem = uxGroupTree.Nodes(0).Tag
                    Else
                        selectGroup = uxGroupTree.Nodes(0).Tag
                        itemDesc = ""
                    End If
                    groupId = selectGroup

                    LoadTranslationItems(groupId, entityTableId)
                    CurrentGroup = groupId
                    If itemDesc <> "" Then
                        FindGridItem(itemDesc)
                    End If
                End If
                'selectGroup = uxUsers.Items(0).Tag
                'groupid = TranslationGroups.translationgroups(selectGroup).collectioncode
                'LoadTranslationItems(groupid, entityTableId)
                'currentGroup = groupid
                'FindGridItem(uxUsers.Items(0).Text)

            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_cp_translation", "LoadTranslationGroups", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub

    Private Sub TraslatorSplit_Panel2_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles TraslatorSplit.Panel2.Paint

    End Sub

    Private Sub RefreshGroups()

        Dim groupNode As TreeNode = Nothing
        Dim itemNode As TreeNode = Nothing
        Dim currentCollection As String

        Try

            uxGroupTree.Nodes.Clear()
            currentCollection = ""

            'Dim lwarn As ListViewItem
            'uxUsers.BeginUpdate()
            'Me.uxUsers.Items.Clear()
            'Me.uxCompanies.Items.Clear()

            For i = 0 To TranslationGroups.TranslationGroups.Count - 1

                'TreeView Parents
                If currentCollection <> CStr(TranslationGroups.TranslationGroups(i).collectiondesc) Then
                    If currentCollection <> "" Then
                        uxGroupTree.Nodes.Add(groupNode)
                    End If
                    groupNode = New TreeNode
                    groupNode.Text = CStr(TranslationGroups.TranslationGroups(i).collectiondesc)
                    groupNode.Tag = TranslationGroups.TranslationGroups(i).collectioncode
                    currentCollection = CStr(TranslationGroups.TranslationGroups(i).collectiondesc)
                End If

                'TreeView Children
                itemNode = New TreeNode
                itemNode.Text = CStr(TranslationGroups.TranslationGroups(i).name)
                itemNode.Tag = i
                If TranslationGroups.TranslationGroups(i).CollectionType = "C" Then
                    itemNode.ImageIndex = 1
                End If
                If TranslationGroups.TranslationGroups(i).CollectionType = "T" Then
                    itemNode.ImageIndex = 5
                End If
                If TranslationGroups.TranslationGroups(i).CollectionType = "B" Then
                    itemNode.ImageIndex = 6
                End If
                groupNode.Nodes.Add(itemNode)

                'lwarn = New ListViewItem(CStr(TranslationGroups.translationgroups(i).name))
                'lwarn.Tag = i

                'If TranslationGroups.translationgroups(i).collectiondesc = "User" Then
                '    Me.uxUsers.Items.Add(lwarn)
                'End If

                'If TranslationGroups.translationgroups(i).collectiondesc = "Company" Then
                '    Me.uxCompanies.Items.Add(lwarn)
                'End If

            Next
            If currentCollection <> "" Then
                uxGroupTree.Nodes.Add(groupNode)
            End If

            'uxUsers.EndUpdate()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_cp_translation", "RefreshGroups", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try

    End Sub

    Private Sub FindGridItem(ByVal itemName As String)

        For i = 0 To TranslationGrid.Rows.Count - 1
            If TranslationGrid.Rows(i).Cells("title").Value = itemName Then
                TranslationGrid.FirstDisplayedScrollingRowIndex = TranslationGrid.Rows(i).Index
            End If
        Next
    End Sub


    Private Sub RefreshGrid(Optional ByVal textFilter As String = "")

        Dim rowId As Integer
        Dim columnId As Integer
        Dim citemName As String = " "
        Dim citemDesc As String = " "
        Dim columnName As String = " "
        Dim found As Boolean = False
        Dim loadText As String = ""

        Try
            LoadingGrid = True
            Me.Cursor = Cursors.WaitCursor

            REM add language columns
            TranslationGrid.Columns.Clear()
            REM TranslationGrid.Columns.Add("classname", "class")
            columnId = TranslationGrid.Columns.Add("title", "Item Title")
            'TranslationGrid.Columns(columnId).AutoSizeMode = DataGridViewAutoSizeColumnsMode.Fill
            TranslationGrid.Columns(columnId).AutoSizeMode = DataGridViewAutoSizeColumnsMode.None
            TranslationGrid.Columns(columnId).Width = 200
            TranslationGrid.Columns(columnId).SortMode = DataGridViewColumnSortMode.NotSortable
            columnId = TranslationGrid.Columns.Add("fielddesc", "Field Description")
            TranslationGrid.Columns(columnId).SortMode = DataGridViewColumnSortMode.NotSortable
            TranslationGrid.Columns(columnId).AutoSizeMode = DataGridViewAutoSizeColumnsMode.None
            columnId = TranslationGrid.Columns.Add("defaultvalue", "Default Value")
            TranslationGrid.Columns(columnId).AutoSizeMode = DataGridViewAutoSizeColumnsMode.None
            TranslationGrid.Columns(columnId).SortMode = DataGridViewColumnSortMode.NotSortable
            For i = 0 To TranslationItems.Languages.Count - 1
                If TranslationItems.Languages(i).LanguageCode <> "en" Then
                    columnName = TranslationItems.Languages(i).LanguageCode + "_" + TranslationItems.Languages(i).CountryCode
                    columnId = TranslationGrid.Columns.Add(columnName, columnName)
                    TranslationGrid.Columns(columnId).AutoSizeMode = DataGridViewAutoSizeColumnsMode.None
                    TranslationGrid.Columns(columnId).SortMode = DataGridViewColumnSortMode.NotSortable
                    TranslationGrid.Columns(columnId).Width = 100
                End If
            Next
            TranslationGrid.Columns(columnId).AutoSizeMode = DataGridViewAutoSizeColumnsMode.Fill

            REM Add data
            TranslationGrid.Rows.Clear()
            rowId = 0
            For i = 0 To TranslationItems.TranslationItems.Count - 1

                REM Filter
                found = False
                If TranslationItems.TranslationItems(i).CollectionType = "B" Then
                    loadText = TranslationItems.TranslationItems(i).ItemDesc
                Else
                    loadText = TranslationItems.TranslationItems(i).ItemName
                End If
                If textFilter <> "" Then
                    If textFilter.Length <= loadText.Length Then
                        If InStr(UCase(loadText), UCase(textFilter)) > 0 Then
                            REM If UCase(loadText.Substring(0, textFilter.Length)) = UCase(textFilter) Then
                            found = True
                        End If
                    End If
                Else
                    found = True
                End If

                If found = True Then
                    REM Item data
                    If TranslationItems.TranslationItems(i).itemname <> citemName Then
                        REM rowid = TranslationGrid.Rows.Add(TranslationItems.translationitems(i).collectiondesc, TranslationItems.translationitems(i).itemname)
                        rowId = TranslationGrid.Rows.Add(TranslationItems.TranslationItems(i).itemname)
                        TranslationGrid.Rows(rowId).ReadOnly = True
                        REM TranslationGrid.Rows(rowid).Cells("classname").Style.Font = New Font(Me.TranslationGrid.Font, FontStyle.Bold)
                        TranslationGrid.Rows(rowId).Cells("title").Style.Font = New Font(Me.TranslationGrid.Font, FontStyle.Bold)
                        For i1 = 0 To TranslationGrid.Columns.Count - 1
                            TranslationGrid.Rows(rowId).Cells(i1).Style.BackColor = Me.BackColor
                        Next
                        citemName = TranslationItems.TranslationItems(i).itemname
                        citemDesc = " "
                    End If

                    REM Field data
                    If TranslationItems.TranslationItems(i).itemdesc <> citemDesc Then
                        rowId = TranslationGrid.Rows.Add()
                        For i1 = 0 To TranslationGrid.Columns.Count - 1
                            TranslationGrid.Rows(rowId).Cells(i1).Style.BackColor = Me.BackColor
                        Next
                        If TranslationItems.TranslationItems(i).CollectionType = "B" Then
                            TranslationGrid.Rows(rowId).Cells("title").Value = TranslationItems.TranslationItems(i).ItemDesc
                        Else
                            TranslationGrid.Rows(rowId).Cells("fielddesc").Value = TranslationItems.TranslationItems(i).ItemDesc
                        End If
                        TranslationGrid.Rows(rowId).Cells("fielddesc").ReadOnly = True
                        TranslationGrid.Rows(rowId).Cells("title").ReadOnly = True
                        TranslationGrid.Rows(rowId).Cells("defaultvalue").Value = TranslationItems.TranslationItems(i).ItemDefaultValue
                        TranslationGrid.Rows(rowId).Cells("defaultvalue").ReadOnly = True
                        citemDesc = TranslationItems.TranslationItems(i).itemdesc
                    End If

                    REM Language Value
                    If TranslationItems.TranslationItems(i).LanguageCode <> "en" Then
                        columnName = TranslationItems.TranslationItems(i).LanguageCode + "_" + TranslationItems.TranslationItems(i).CountryCode
                        With TranslationGrid.Rows(rowId).Cells(columnName)
                            .Value = TranslationItems.TranslationItems(i).LabelValue
                            .Tag = i
                            If controller.Container.mode > bc_am_cp_container.VIEW Then
                                .ReadOnly = False
                                .Style.BackColor = Color.White
                            Else
                                .ReadOnly = True
                            End If
                        End With
                    End If
                End If
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_cp_translation", "RefreshGrid", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            LoadingGrid = False
            Me.Cursor = Cursors.Default
        End Try


    End Sub

    Public Sub LoadTranslationItems(ByVal groupid As Long, ByVal entityTableId As Long)

        Try

            TranslationItems.ReadCollectionCode = groupid
            TranslationItems.ReadItemId = entityTableId

            If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                TranslationItems.db_read()
            ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                TranslationItems.tmode = bc_cs_soap_base_class.tREAD
                If TranslationItems.transmit_to_server_and_receive(TranslationItems, True) = False Then
                    Exit Sub
                End If
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Cannot connect to " + bc_cs_central_settings.soap_server + " application cannot be launched.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If

            RefreshGrid()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_cp_translation", "LoadTranslationItems", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub


    'Private Sub uxCompanies_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxCompanies.SelectedIndexChanged

    '    Dim selectGroup As Long
    '    Dim groupid As Long
    '    Dim entityTableId As Long

    '    If uxCompanies.SelectedItems.Count > 0 Then

    '        groupid = 0
    '        entityTableId = 0

    '        selectGroup = uxCompanies.SelectedItems.Item(0).Tag
    '        groupid = TranslationGroups.translationgroups(selectGroup).collectioncode
    '        'entityTableId = TranslationGroups.translationgroups(selectGroup).itemid

    '        If currentGroup <> groupid And controller.Container.uncomitted_data Then
    '            MsgBox("You have uncommitted data please commit or discard before changing group", MsgBoxStyle.Information, "Blue Curve")
    '            Exit Sub
    '        End If

    '        If currentGroup <> groupid Then
    '            LoadTranslationItems(groupid, entityTableId)
    '            currentGroup = groupid
    '        End If

    '        FindGridItem(uxCompanies.SelectedItems.Item(0).Text)

    '    End If

    'End Sub

    'Private Sub uxUsers_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxUsers.SelectedIndexChanged
    '    Dim selectGroup As Long
    '    Dim groupid As Long
    '    Dim entityTableId As Long

    '    If uxUsers.SelectedItems.Count > 0 Then
    '        groupid = 0
    '        entityTableId = 0

    '        selectGroup = uxUsers.SelectedItems.Item(0).Tag
    '        groupid = TranslationGroups.translationgroups(selectGroup).collectioncode

    '        If currentGroup <> groupid And controller.Container.uncomitted_data = True Then
    '            MsgBox("You have uncommitted data please commit or discard before changing group", MsgBoxStyle.Information, "Blue Curve")
    '            Exit Sub
    '        End If

    '        If currentGroup <> groupid Then
    '            LoadTranslationItems(groupid, entityTableId)
    '            currentGroup = groupid
    '        End If

    '        FindGridItem(uxUsers.SelectedItems.Item(0).Text)
    '    End If

    'End Sub

    Private Sub TranslationGrid_CellBeginEdit(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellCancelEventArgs) Handles TranslationGrid.CellBeginEdit
        If LoadingGrid = False Then
            uxTranchanges.Enabled = True
            uxTrandiscard.Enabled = True
        End If
    End Sub

    Private Sub TranslationGrid_CellValueChanged(ByVal sender As Object, ByVal e As System.Windows.Forms.DataGridViewCellEventArgs) Handles TranslationGrid.CellValueChanged

        Dim selectGroup As Long

        If LoadingGrid = False Then
            selectGroup = TranslationGrid.CurrentCell.Tag
            TranslationItems.TranslationItems(selectGroup).labelvalue = TranslationGrid.CurrentCell.Value
            controller.Container.uncomitted_data = True
            TranslationItems.TranslationItems(selectGroup).itemchanged = True
        End If
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

    Private Sub uxTranchanges_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles uxTranchanges.MouseUp
        controller.commit_changes()
        uxTranchanges.Enabled = False
        uxTrandiscard.Enabled = False
    End Sub

    Private Sub uxTrandiscard_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTrandiscard.Click
        LoadTranslationItems(CurrentGroup, 0)
        uxTranchanges.Enabled = False
        uxTrandiscard.Enabled = False
        controller.Container.uncomitted_data = False
    End Sub

    Private Sub uxGroupTree_AfterExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles uxGroupTree.AfterExpand
        Dim selectGroup As Long
        Dim groupId As Long
        Dim entityTableId As Long
        Dim itemDesc As String

        Try
            groupId = 0
            entityTableId = 0
            itemDesc = ""

            selectGroup = e.Node.Tag
            groupId = selectGroup

            If CurrentGroup <> groupId And controller.Container.uncomitted_data = True Then
                MsgBox("You have uncommitted data please commit or discard before changing group.", MsgBoxStyle.Information, "Blue Curve")
                Exit Sub
            End If

            If CurrentGroup <> groupId Then
                LoadTranslationItems(groupId, entityTableId)
                CurrentGroup = groupId
            End If

            If itemDesc <> "" Then
                FindGridItem(itemDesc)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_cp_translation", "AfterExpand", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try


    End Sub

    Private Sub uxGroupTree_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles uxGroupTree.AfterSelect
        Dim selectGroup As Long
        Dim selectItem As Long
        Dim groupId As Long
        Dim entityTableId As Long
        Dim itemDesc As String

        Try

            groupId = 0
            entityTableId = 0
            itemDesc = ""

            If uxGroupTree.SelectedNode.Level <> 0 Then
                selectGroup = uxGroupTree.SelectedNode.Parent.Tag
                itemDesc = uxGroupTree.SelectedNode.Text
                selectItem = uxGroupTree.SelectedNode.Tag
            Else
                selectGroup = uxGroupTree.SelectedNode.Tag
                itemDesc = ""
            End If
            groupId = selectGroup

            If CurrentGroup <> groupId And controller.Container.uncomitted_data = True Then
                MsgBox("You have uncommitted data please commit or discard before changing group.", MsgBoxStyle.Information, "Blue Curve")
                Exit Sub
            End If

            CurrentFilter = ""
            uxSearch.SearchText = ""

            If CurrentGroup <> groupId Then
                Me.Cursor = Cursors.WaitCursor
                LoadTranslationItems(groupId, entityTableId)
                CurrentGroup = groupId
                Me.Cursor = Cursors.Default
            End If

            If itemDesc <> "" Then
                FindGridItem(itemDesc)
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_cp_translation", "AfterSelect", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

    Private Sub uxGroupTree_BeforeExpand(ByVal sender As Object, ByVal e As System.Windows.Forms.TreeViewCancelEventArgs) Handles uxGroupTree.BeforeExpand

        Dim selectGroup As Long
        Dim groupId As Long
        Dim itemDesc As String

        Try

            groupId = 0
            itemDesc = ""

            selectGroup = e.Node.Tag
            groupId = selectGroup

            If CurrentGroup <> groupId And controller.Container.uncomitted_data = True Then
                MsgBox("You have uncommitted data please commit or discard before changing group.", MsgBoxStyle.Information, "Blue Curve")
                e.Cancel = True
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_cp_translation", "BeforeExpand", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

    Private Sub uxTranchanges_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTranchanges.Click

    End Sub

    Private Sub uxSearch_FireSearch(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxSearch.FireSearch
        Dim filerText As String

        Try

            filerText = Me.uxSearch.SearchText

            If CurrentFilter <> filerText Then
                RefreshGrid(filerText)
                CurrentFilter = filerText
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_cp_translation", "FireSearch", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub

    Public Sub searchforitem(ByVal searchtext As String)

    End Sub


    Private Sub uxSearch_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxSearch.Load

    End Sub
End Class
