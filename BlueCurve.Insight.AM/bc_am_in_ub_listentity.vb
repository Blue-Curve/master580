Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Xml
Imports System.Windows.Forms

Public Class bc_am_in_ub_listentity

    Public Event AddNewItem(ByRef baiul As bc_am_in_ub_listentity)
    Public Event RemoveItem(ByRef baiul As bc_am_in_ub_listentity)

    Public Event itemchanged(ByRef baiul As bc_am_in_ub_listentity)

    Friend Sub loadLists(ByVal lLists As List(Of bc_om_entity_class))
        For Each boc In lLists
            cbAggregationLists.Items.Add(boc)
        Next
    End Sub
    Friend Function all_lists_selected() As Boolean
        all_lists_selected = False
        If Me.cbClasses.SelectedIndex > -1 And Me.cbEntities.SelectedIndex > -1 And Me.cbAggregationLists.SelectedIndex > -1 Then
            all_lists_selected = True
        End If
    End Function
    Friend Function operator_selected() As Boolean
        operator_selected = False
        If Me.cbOperator.SelectedIndex > 0 Then
            operator_selected = True
        End If
    End Function
    Friend Sub operator_highlighted()
        Me.cbOperator.Select()
    End Sub
    Friend Sub clear_operator()
        Me.cbOperator.SelectedIndex = -1
    End Sub
    Friend Sub loadClasses(ByRef boc As bc_om_entity_class)

        If Not boc Is Nothing Then
            cbClasses.Items.Clear()
            If TypeOf ParentForm Is bc_am_in_universe_builder Then
                For Each bocl As bc_om_class_link_info In boc.child_links
                    For Each bocChild As bc_om_entity_class In CType(ParentForm, bc_am_in_universe_builder).classes.classes
                        If bocChild.class_id = bocl.child_class_id Then
                            cbClasses.Items.Add(bocChild)
                        End If
                    Next
                Next
            End If
        End If

    End Sub

    Friend Sub loadEntities(ByRef boc As bc_om_entity_class)

        cbEntities.Items.Clear()
        If Not boc Is Nothing Then
            If TypeOf ParentForm Is bc_am_in_universe_builder Then
                Dim entities As bc_om_entities = CType(ParentForm, bc_am_in_universe_builder).entities
                For Each entity As bc_om_entity In entities.entity
                    If entity.class_id = boc.class_id Then
                        cbEntities.Items.Add(entity)
                    End If
                Next
            End If
        End If

    End Sub

    Private Sub cbClasses_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbClasses.SelectedIndexChanged
        loadEntities(cbClasses.SelectedItem)
        RaiseEvent itemchanged(Me)
    End Sub

    Private Sub uxAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAdd.Click
        RaiseEvent AddNewItem(Me)
        RaiseEvent itemchanged(Me)
    End Sub

    Private Sub cbAggregationLists_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbAggregationLists.SelectedIndexChanged
        loadClasses(cbAggregationLists.SelectedItem)
        RaiseEvent itemchanged(Me)

    End Sub

    Public Sub loadXml(ByVal strXml As String)

        Dim xd As New XmlDocument
        xd.LoadXml(strXml)
        Dim l As XmlNodeList = xd.GetElementsByTagName("paren1")
        If l.Count > 0 Then
            txtParen1.Text = l(0).InnerText
        End If
        l = xd.GetElementsByTagName("paren2")
        If l.Count > 0 Then
            txtParen2.Text = l(0).InnerText
        End If
        l = xd.GetElementsByTagName("paren3")
        If l.Count > 0 Then
            'txtParen3.Text = l(0).InnerText
        End If

        cbAggregationLists.SelectedItem = Nothing
        l = xd.GetElementsByTagName("list")
        If l.Count > 0 Then
            For Each boa As bc_om_entity_class In cbAggregationLists.Items
                If boa.class_name = l(0).InnerText Then
                    cbAggregationLists.SelectedItem = boa
                    Exit For
                End If
            Next
        End If

        cbClasses.SelectedItem = Nothing
        l = xd.GetElementsByTagName("class")
        If l.Count > 0 Then
            For Each boc As bc_om_entity_class In cbClasses.Items
                If boc.class_id.ToString = l(0).InnerText Then
                    cbClasses.SelectedItem = boc
                    'loadEntities(boc)
                    Exit For
                End If
            Next
        End If

        cbEntities.SelectedItem = Nothing
        l = xd.GetElementsByTagName("entity")
        If l.Count > 0 Then
            For Each boe As bc_om_entity In cbEntities.Items
                If boe.id.ToString = l(0).InnerText Then
                    cbEntities.SelectedItem = boe
                    Exit For
                End If
            Next
        End If

        cbOperator.SelectedItem = Nothing
        l = xd.GetElementsByTagName("operator")
        If l.Count > 0 Then
            For Each strList As String In cbOperator.Items
                If strList = l(0).InnerText Then
                    cbOperator.SelectedItem = strList
                    Exit For
                End If
            Next
        End If

    End Sub

    Public Overrides Function ToString() As String
        Dim strOutput As String = ""
        If Not txtParen1.Text.Length = 0 Then
            strOutput = strOutput & txtParen1.Text
        End If
        If Not cbEntities.SelectedItem Is Nothing Then
            strOutput = strOutput & "<" & CType(cbEntities.SelectedItem, bc_om_entity).id & ">"
        End If
        If Not txtParen2.Text.Length = 0 Then
            strOutput = strOutput & txtParen2.Text
        End If
        If Not cbOperator.SelectedItem Is Nothing Then
            strOutput = strOutput & cbOperator.SelectedItem.ToString
        End If
        'If Not txtParen3.Text.Length = 0 Then
        '    strOutput = strOutput & txtParen3.Text
        'End If
        Return strOutput
    End Function

    Public Function toXml() As String
        Dim strOutput As String = "<list_entity>"
        If Not txtParen1.Text.Length = 0 Then
            strOutput = strOutput & "<paren1>" & txtParen1.Text & "</paren1>"
        End If
        If Not txtParen2.Text.Length = 0 Then
            strOutput = strOutput & "<paren2>" & txtParen2.Text & "</paren2>"
        End If
        'If Not txtParen3.Text.Length = 0 Then
        '    strOutput = strOutput & "<paren3>" & txtParen3.Text & "</paren3>"
        'End If
        If Not cbEntities.SelectedItem Is Nothing Then
            strOutput = strOutput & "<entity>" & CType(cbEntities.SelectedItem, bc_om_entity).id & "</entity>"
        End If
        If Not cbClasses.SelectedItem Is Nothing Then
            strOutput = strOutput & "<class>" & CType(cbClasses.SelectedItem, bc_om_entity_class).class_id & "</class>"
        End If
        If Not cbAggregationLists.SelectedItem Is Nothing Then
            strOutput = strOutput & "<list>" & cbAggregationLists.SelectedItem.ToString & "</list>"
        End If
        If Not cbOperator.SelectedItem Is Nothing Then
            strOutput = strOutput & "<operator>" & cbOperator.SelectedItem.ToString & "</operator>"
        End If
        strOutput = strOutput & "</list_entity>"
        Return strOutput
    End Function

    Private Sub txtParenLeft_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtParen1.KeyPress
        If e.KeyChar <> "(" Then
            e.Handled = True
        End If
    End Sub

    Private Sub txtParenRight_KeyPress(ByVal sender As Object, ByVal e As System.Windows.Forms.KeyPressEventArgs) Handles txtParen2.KeyPress
        If e.KeyChar <> ")" Then
            e.Handled = True
        End If
    End Sub

    Private Sub uxRemove_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxRemove.Click
        RaiseEvent RemoveItem(Me)
        RaiseEvent itemchanged(Me)
    End Sub

    Private Sub cbEntities_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbEntities.SelectedIndexChanged
        RaiseEvent itemchanged(Me)
    End Sub

    Private Sub cbOperator_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbOperator.SelectedIndexChanged
        RaiseEvent itemchanged(Me)
    End Sub

    Private Sub txtParen1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtParen1.TextChanged
        RaiseEvent itemchanged(Me)
    End Sub

    Private Sub txtParen2_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txtParen2.TextChanged
        RaiseEvent itemchanged(Me)
    End Sub

    Private Sub txtParen3_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        RaiseEvent itemchanged(Me)
    End Sub
End Class
