Imports System.Timers
Imports BlueCurve.Core.OM

Public Class bc_am_cp_assign
    Public cancel_selected As Boolean = True
    Public filter_class As String
    Public fcontainer As Object
    Public num As Integer
    Public new_entities As New ArrayList
    Private myTimer As Timer
    Private Const SEARCH_TIMER_WAIT_DURATION As Integer = 750
    Public entity_id As Long
    Public class_id As Long
    Public schema_id As Long
    Public bparent As Boolean

    Public entity_name As String
    Public class_name As String
    Public schema_name As String
    Public pref_type_id As Integer
    Public user_mode As Boolean = False
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Badd.Click
        If Me.lallparent.SelectedIndex = -1 Then
            assign_new_entity()
        Else
            assign_entity()
        End If

    End Sub
    Private Sub Bdel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bdel.Click
        deassign_entity()
    End Sub
    Private Sub assign_entity()
        If Me.lallparent.SelectedIndex = -1 Then
            Exit Sub
        End If
        check_ok()
        lselparent.Items.Add(Me.lallparent.Text)
        lallparent.Items.RemoveAt(Me.lallparent.SelectedIndex)
        Me.Badd.Enabled = False
        If Me.lselparent.Sorted = False Then
            Me.lselparent.SelectedIndex = Me.lselparent.Items.Count - 1
        End If
    End Sub
    Private Sub assign_new_entity()
        Try
            If Trim(Me.Tnew.Text) = "" Then
                Exit Sub
            End If
            check_ok()
            REM check name wasnt already existing is so dont treat it as new
            Dim i As Integer
            Dim found As Boolean = False
            For i = 0 To Me.lallparent.Items.Count - 1
                If Trim(UCase(Me.Tnew.Text)) = Trim(UCase(Me.lallparent.Items(i))) Then
                    Me.lallparent.Items.RemoveAt(i)
                    found = True
                    Exit For
                End If
            Next
            REM check item not already assigned
            For i = 0 To Me.lselparent.Items.Count - 1
                If Trim(UCase(Me.Tnew.Text)) = Trim(UCase(Me.lselparent.Items(i))) Then
                    Exit Sub
                End If
            Next
            If found = False Then
                Me.new_entities.Add(Me.Tnew.Text)
            End If
            lselparent.Items.Add(Me.Tnew.Text)
            If Me.lselparent.Sorted = False Then
                Me.lselparent.SelectedIndex = Me.lselparent.Items.Count - 1
            End If
        Catch ex As Exception
            MsgBox(ex.Message)

        Finally
            Me.Tnew.Text = ""
            Me.Badd.Enabled = False
        End Try
    End Sub
    Private Sub lallparent_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lallparent.SelectedIndexChanged
        Me.Badd.Enabled = False
        If Me.lallparent.SelectedIndex > -1 Then
            Me.Badd.Enabled = True
        End If
    End Sub
    Private Sub deassign_entity()
        If Me.lselparent.SelectedIndex = -1 Then
            Exit Sub
        End If
        check_ok(True)
        REM if it is new remove from new list
        Dim i As Integer
        For i = 0 To Me.new_entities.Count - 1
            If Me.new_entities(i) = Me.lselparent.Text Then
                Me.new_entities.RemoveAt(i)
                lselparent.Items.RemoveAt(Me.lselparent.SelectedIndex)

                Exit For
            End If
        Next
        If Me.lselparent.SelectedIndex > -1 Then
            If InStr(Me.lselparent.Text, "(inactive)") = 0 Then
                lallparent.Items.Add(Me.lselparent.Text)
            End If
            lselparent.Items.RemoveAt(Me.lselparent.SelectedIndex)
        End If
        Me.Bdel.Enabled = False
    End Sub
    Private Sub lallparent_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lallparent.DoubleClick
        assign_entity()
    End Sub
    Private Sub lselparent_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles lselparent.DoubleClick
        deassign_entity()
    End Sub

    Private Sub Bok_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bok.Click
        Me.cancel_selected = False
        Me.Hide()
    End Sub

    Private Sub Bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bcancel.Click
        Me.Hide()
    End Sub
    Private Sub lselparent_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lselparent.SelectedIndexChanged
        Me.Bdel.Enabled = False
        Me.bedit.Enabled = False
        Me.bup.Enabled = False
        Me.bdn.Enabled = False
        If Me.lselparent.SelectedIndex > -1 Then
            Me.bedit.Enabled = True
            Me.Bdel.Enabled = True
            If Me.lselparent.SelectedIndex > 0 Then
                Me.bup.Enabled = True
            End If
            If Me.lselparent.SelectedIndex < Me.lselparent.Items.Count - 1 Then
                Me.bdn.Enabled = True
            End If
        End If
    End Sub

    Private Sub tlfilter_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tlfilter.TextChanged
        If Not myTimer Is Nothing Then
            myTimer.Stop()
        End If
        myTimer = New Timer(SEARCH_TIMER_WAIT_DURATION)
        AddHandler myTimer.Elapsed, New ElapsedEventHandler(AddressOf run_small_entity_filter)
        myTimer.AutoReset = False
        myTimer.Enabled = True
    End Sub
    Public Sub check_ok(Optional ByVal remove As Boolean = False)
        Me.Bok.Enabled = False
        Select Case Me.num
            Case 0
                Me.Bok.Enabled = True
            Case -1
                If remove = False Then
                    If Me.lselparent.Items.Count + 1 > 0 Then
                        Me.Bok.Enabled = True
                    End If
                Else
                    If Me.lselparent.Items.Count > 1 Then
                        Me.Bok.Enabled = True
                    End If
                End If
            Case Is > 0
                If remove = False Then
                    If Me.lselparent.Items.Count + 1 = Me.num Then
                        Me.Bok.Enabled = True
                    End If
                Else

                    If Me.lselparent.Items.Count - 1 = Me.num Then
                        Me.Bok.Enabled = True
                    End If
                End If
        End Select
    End Sub
    Private Delegate Sub delFilter()
    Private Sub run_small_entity_filter()
        Dim df As delFilter = AddressOf small_entity_filter
        BeginInvoke(df)
    End Sub

    Public Sub small_entity_filter()

        lallparent.Items.Clear()
        Dim l As String
        If filter_class.Length >= 5 Then
            If filter_class.Substring(0, 5) = "Users" Then
                filter_class = "Users"
            End If
        End If
        If filter_class = "Users" Then
            If tlfilter.Text = "" Then
                For i = 0 To fcontainer.oUsers.user.Count - 1
                    If fcontainer.oUsers.user(i).inactive = 0 Then
                        lallparent.Items.Add(fcontainer.oUsers.user(i).first_name + " " + fcontainer.oUsers.user(i).surname)
                        For j = 0 To lselparent.Items.Count - 1
                            If lselparent.Items(j) = lallparent.Items(lallparent.Items.Count - 1) Then
                                lallparent.Items.RemoveAt(lallparent.Items.Count - 1)
                            End If
                        Next
                    End If
                Next
            Else
                l = tlfilter.Text.Length
                For i = 0 To fcontainer.oUsers.user.Count - 1
                    If fcontainer.oUsers.user(i).inactive = 0 Then
                        Dim str As String
                        str = fcontainer.oUsers.user(i).first_name + " " + fcontainer.oUsers.user(i).surname
                        If l <= str.Length Then
                            If UCase(str.Substring(0, l)) = UCase(tlfilter.Text) Then
                                lallparent.Items.Add(str)
                                For j = 0 To lselparent.Items.Count - 1
                                    If lselparent.Items(j) = str Then
                                        lallparent.Items.RemoveAt(lallparent.Items.Count - 1)
                                    End If
                                Next
                            End If
                        End If
                    End If
                Next
            End If
            Exit Sub
        End If

        If tlfilter.Text = "" Then
            REM entities of class
            For i = 0 To fcontainer.oEntities.entity.Count - 1
                If fcontainer.oEntities.entity(i).inactive = 0 Then
                    If fcontainer.oEntities.entity(i).class_name = filter_class Then
                        lallparent.Items.Add(fcontainer.oEntities.entity(i).name)
                        For j = 0 To lselparent.Items.Count - 1
                            If lselparent.Items(j) = fcontainer.oEntities.entity(i).name Then
                                lallparent.Items.RemoveAt(lallparent.Items.Count - 1)
                            End If
                        Next
                    End If
                End If
            Next
        Else
            For i = 0 To fcontainer.oEntities.entity.Count - 1
                If fcontainer.oEntities.entity(i).inactive = 0 Then

                    If fcontainer.oEntities.entity(i).class_name = filter_class Then
                        l = tlfilter.Text.Length
                        If l <= fcontainer.oEntities.entity(i).name.length Then
                            If UCase(fcontainer.oEntities.entity(i).name.Substring(0, l)) = UCase(tlfilter.Text) Then
                                lallparent.Items.Add(fcontainer.oEntities.entity(i).name)
                                For j = 0 To lselparent.Items.Count - 1
                                    If lselparent.Items(j) = fcontainer.oEntities.entity(i).name Then
                                        lallparent.Items.RemoveAt(lallparent.Items.Count - 1)
                                    End If
                                Next
                            End If
                        End If
                    End If
                End If
            Next
        End If

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tnew.TextChanged
        Me.lallparent.SelectedIndex = -1
        Me.Badd.Enabled = False
        If Me.Tnew.Text <> "" Then
            Me.Badd.Enabled = True
        End If
    End Sub

    Private Sub bc_am_cp_assign_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub bup_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bup.Click
        Dim tx As String
        Dim idx As Integer
        tx = Me.lselparent.Text
        idx = Me.lselparent.SelectedIndex
        Me.lselparent.Items.RemoveAt(idx)
        Me.lselparent.Items.Insert(idx - 1, tx)
        Me.bup.Enabled = False
        Me.bdn.Enabled = False
        Me.lselparent.Text = tx
        Me.Bok.Enabled = True
    End Sub
    Private Sub bdn_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bdn.Click
        Dim tx As String
        Dim idx As Integer
        tx = Me.lselparent.Text
        idx = Me.lselparent.SelectedIndex
        Me.lselparent.Items.RemoveAt(idx)
        Me.lselparent.Items.Insert(idx + 1, tx)
        Me.bup.Enabled = False
        Me.bdn.Enabled = False
        Me.lselparent.Text = tx
        Me.Bok.Enabled = True

    End Sub

    Private Sub Clist_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Clist.SelectedIndexChanged
        If Clist.SelectedIndex = -1 Then
            Exit Sub
        End If
        Me.Cursor = Windows.Forms.Cursors.WaitCursor

        Me.lallparent.Items.Clear()
        Me.lselparent.Items.Clear()
        For i = 0 To fcontainer.oentities.entity.count - 1
            If fcontainer.oentities.entity(i).class_name = Me.Clist.Text And fcontainer.oentities.entity(i).inactive = False Then
                BlueCurve_TextSearch1.SearchClass = fcontainer.oentities.entity(i).class_id
                Me.lallparent.Items.Add(fcontainer.oentities.entity(i).name)
            End If

        Next
        Me.filter_class = Me.Clist.Text
        Me.tlfilter.Text = ""
        Me.tlfilter.Enabled = True

        REM set uop full search
        'For kk = 0 To Container.oClasses.classes.Count - 1
        '    If Container.oClasses.classes(kk).class_name = class_name Then
        '        BlueCurve_TextSearch1.SearchClass = Container.oClasses.classes(kk).class_id
        '        Exit For

        '    End If
        'Next
        BlueCurve_TextSearch1.Visible = True
        BlueCurve_TextSearch1.Enabled = True
        BlueCurve_TextSearch1.AttributeControlBuild(False)
        BlueCurve_TextSearch1.SearchUserEntitiesOnly = False
        BlueCurve_TextSearch1.SearchBuildEntitiesOnly = False
        BlueCurve_TextSearch1.showinactive = False
        BlueCurve_TextSearch1.ExcludeControl = "lselparent"

        Me.Cursor = Windows.Forms.Cursors.Default

    End Sub

    Private Sub ltitle_Enter(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ltitle.Enter

    End Sub

    Private Sub bnew_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bnew.Click
        fcontainer.add_new(Me)



    End Sub

    Private Sub Button1_Click_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bedit.Click
        fcontainer.modify_attribute_menu(Me)
    End Sub

    Private Sub baudit_Click(sender As Object, e As EventArgs)
        Dim fs As New bc_am_audit_entity_links
        Dim cs As New Cbc_am_audit_entity_links
        Me.Cursor = Windows.Forms.Cursors.WaitCursor

        If user_mode = False Then
            If cs.load_data(fs, entity_id, class_id, schema_id, bparent, class_name, entity_name, schema_name, -1, 0, bc_om_audit_links.EAUDIT_TYPE.TAXONOMY) Then
                Me.Cursor = Windows.Forms.Cursors.Default
                fs.ShowDialog()
            End If
        Else
            If cs.load_data(fs, entity_id, 0, 0, 0, class_name, entity_name, "", pref_type_id, 0, bc_om_audit_links.EAUDIT_TYPE.USERS_FOR_PREF) Then
                Me.Cursor = Windows.Forms.Cursors.Default
                fs.ShowDialog()
            End If

        End If
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub
End Class

