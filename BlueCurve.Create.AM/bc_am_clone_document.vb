Imports System.Windows.Forms

'Public Class bc_am_clone_document
'    Public proceed As Boolean = False
'    Public mdoc_id As Long
'    Public sdoc_id As Long
'    Public Sub load_list(ByVal documents As ArrayList)
'        Me.bok.Visible = False
'        Me.bokgrey.Visible = True

'        Dim lvw As ListViewItem
'        For i = 0 To documents.Count - 1
'            lvw = New ListViewItem(CStr(documents(i).title))
'            lvw.SubItems.Add(CStr(documents(i).doc_date))
'            lvw.SubItems.Add(CStr(documents(i).id))
'            lvw.SubItems.Add(CStr(documents(i).pub_type_id))
'            Me.Ldocs.Items.Add(lvw)
'        Next

'    End Sub
'    Private Sub bok_mdn(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bok.MouseDown
'        Me.bokgrey.Visible = True
'        Me.bok.Visible = False
'    End Sub
'    Private Sub bok_mup(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bok.MouseUp
'        'Me.Hide()
'        Dim tmdoc_id, tsdoc_id As String
'        tmdoc_id = Me.Ldocs.Items(Me.Ldocs.SelectedItems(0).Index).SubItems(2).Text
'        tsdoc_id = Me.Ldocs.Items(Me.Ldocs.SelectedItems(0).Index).SubItems(3).Text
'        If IsNumeric(tmdoc_id) Then
'            Me.mdoc_id = tmdoc_id
'        End If
'        If IsNumeric(tsdoc_id) Then
'            Me.sdoc_id = tsdoc_id
'        End If
'        Close()
'        proceed = True
'    End Sub

'    Private Sub bleftgrey_mdn(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bleftgrey.MouseDown
'        Me.bleftgrey.Visible = False
'        Me.Btnback.Visible = True

'    End Sub
'    Private Sub bleftgrey_mup(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bleftgrey.MouseUp
'        'Me.Hide()
'        Close()
'        proceed = False
'    End Sub

'    Private Sub bleftgrey_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bleftgrey.Click

'    End Sub

'    Private Sub Ldocs_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ldocs.SelectedIndexChanged
'        Me.bok.Visible = True
'        Me.bokgrey.Visible = False
'    End Sub
'    Public sorttoggle As Boolean
'    Public Shared column As Integer
'    Private Sub Ldocs_ColumnClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ColumnClickEventArgs) Handles Ldocs.ColumnClick




'        column = CInt(e.Column.ToString)

'        Select Case CInt(e.Column.ToString)

'            Case 0

'                REM title

'                Ldocs.ListViewItemSorter() = New CompareByname

'                If CompareByname.toggle = False Then

'                    CompareByname.toggle = True
'                    sorttoggle = False

'                Else

'                    CompareByname.toggle = False
'                    sorttoggle = True

'                End If

'            Case 1

'                REM date

'                Ldocs.ListViewItemSorter() = New CompareBydate

'                If CompareBydate.toggle = False Then

'                    CompareBydate.toggle = True
'                    sorttoggle = False

'                Else

'                    CompareBydate.toggle = False
'                    sorttoggle = True

'                End If

'        End Select


'    End Sub

'    Class CompareByname
'        Implements System.Collections.IComparer
'        Public Shared toggle As Boolean = False

'        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
'        Implements System.Collections.IComparer.Compare

'            Dim item1 As ListViewItem = CType(x, ListViewItem)
'            Dim item2 As ListViewItem = CType(y, ListViewItem)
'            If toggle = True Then
'                Return String.Compare(item1.SubItems(column).Text, item2.SubItems(column).Text)
'            Else
'                Return String.Compare(item2.SubItems(column).Text, item1.SubItems(column).Text)
'            End If
'        End Function

'        Public Sub New()

'        End Sub
'    End Class
'    Class CompareBydate
'        Implements System.Collections.IComparer
'        Public Shared toggle As Boolean = False

'        Function compare(ByVal x As Object, ByVal y As Object) As Integer _
'        Implements System.Collections.IComparer.Compare
'            Try
'                Dim item1 As ListViewItem = CType(x, ListViewItem)
'                Dim item2 As ListViewItem = CType(y, ListViewItem)
'                If toggle = True Then
'                    Return Date.Compare(item1.SubItems(column).Text, item2.SubItems(column).Text)
'                Else
'                    Return Date.Compare(item2.SubItems(column).Text, item1.SubItems(column).Text)
'                End If
'            Catch

'            End Try
'        End Function
'    End Class
'End Class