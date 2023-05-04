Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
'Public Class bc_am_comments
'    Public user_id As Long
'    Public doc_id As Long
'    Public stage_id As Long
'    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click

'        REM save it down
'        Dim bcs As New bc_cs_central_settings(True)

'        Dim oc As New bc_om_comment
'        oc.certificate = New bc_cs_security.certificate
'        oc.certificate.user_id = user_id


'        oc.doc_id = doc_id
'        oc.user_id = user_id
'        oc.comment = Me.TextBox1.Text
'        oc.stage_id = stage_id

'        oc.db_write()


'        REM retrieved comments back
'        load_comments()

'    End Sub
'    Public Sub clear_comments()
'        Me.TreeView1.Nodes.Clear()
'    End Sub
'    Public Sub load_comments_for_new_doc()
'        'Me.Rshownum.Checked = False
'        ' Me.Rshownum.Checked = True
'        'Me.Cdays.SelectedIndex = 0
'        load_comments()

'    End Sub

'    Public Sub load_comments()
'        REM retrieved comments back
'        Dim ac As New bc_om_comments

'        'If Me.RadioButton2.Checked = True Then
'        '    ac.number_to_show = -1
'        'Else
'        '    ac.number_to_show = Me.Cdays.SelectedIndex + 1
'        'End If
'        'ac.days_back = Me.Cdays.SelectedIndex + 1
'        ac.doc_id = doc_id
'        ac.certificate.user_id = doc_id

'        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
'            ac.db_read()
'        Else
'            ac.tmode = bc_cs_soap_base_class.tREAD
'            If ac.transmit_to_server_and_receive(ac, True) = False Then
'                Exit Sub

'            End If
'        End If
'        Me.TreeView1.Nodes.Clear()
'        Dim co As Integer

'        If Me.Rshowall.Checked = True Then
'            co = ac.comments.Count
'        Else
'            co = Me.Cdays.SelectedIndex + 1
'        End If


'        For i = 0 To ac.comments.Count - 1
'            Dim da As Date
'            If i >= ac.comments.Count - co Then
'                da = ac.comments(i).da
'                da = da.ToLocalTime
'                Me.TreeView1.Nodes.Add(ac.comments(i).user_name + ": " + CStr(da) + " " + ac.comments(i).stage_name)
'                Me.TreeView1.Nodes(Me.TreeView1.Nodes.Count - 1).Nodes.Add(ac.comments(i).comment)
'            End If
'        Next
'        If Me.TreeView1.Nodes.Count > 0 Then
'            Me.TreeView1.ExpandAll()
'            Me.TreeView1.Nodes(Me.TreeView1.Nodes.Count - 1).EnsureVisible()
'        End If
'        Me.TextBox1.Text = ""
'        Me.Button1.Enabled = False
'    End Sub

'    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
'        If Me.TextBox1.Text <> "" Then
'            Me.Button1.Enabled = True
'        Else
'            Me.Button1.Enabled = False
'        End If
'    End Sub

'    Private Sub bc_am_comments_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

'        For i = 1 To 100
'            Me.Cdays.Items.Add(CStr(i))
'        Next
'        Me.Cdays.SelectedIndex = 0

'    End Sub

'    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rshowall.CheckedChanged
'        If Me.Rshowall.Checked = True Then
'            Me.Cdays.Enabled = False
'            Me.Cdays.SelectedIndex = -1
'            load_comments()
'        End If

'    End Sub

'    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Rshownum.CheckedChanged
'        If Me.Rshownum.Checked = True Then
'            Me.TreeView1.Nodes.Clear()
'            Me.Cdays.SelectedIndex = -1
'            Me.Cdays.Enabled = True
'        End If
'    End Sub

'    Private Sub TextBox1_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs)

'    End Sub

'    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
'        Me.TextBox1.Text = ""
'    End Sub

'    Private Sub Cdays_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cdays.SelectedIndexChanged
'        If Me.Cdays.SelectedIndex > -1 Then
'            load_comments()
'        End If
'    End Sub
'End Class

