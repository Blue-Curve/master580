Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Public Class bc_am_cp_deploy
    Public releases As New bc_om_releases

    Public Sub load_all()
        releases.db_read()

        load_releases()
    End Sub
    Public Sub load_releases()
        Try
            Me.mset.Visible = False
            Me.mdel.Visible = False
            Me.mdep.Visible = False
            Me.treleases.Nodes.Clear()
            For i = 0 To Me.releases.releases.Count - 1
                If releases.releases(i).status = bc_om_release.WORKING Then
                    Me.treleases.Nodes.Add("Working Release")
                    Me.treleases.Nodes(0).Nodes.Add(CStr(releases.releases(i).release_number) + ": " + releases.releases(i).create_date)
                    Me.treleases.Nodes(0).Nodes(Me.treleases.Nodes(0).Nodes.Count - 1).Nodes.Add(releases.releases(i).desc)
                    Exit For
                End If
            Next
            For i = 0 To Me.releases.releases.Count - 1
                If releases.releases(i).status = bc_om_release.ACTIVE Then
                    Me.treleases.Nodes.Add("Active Release")
                    Me.treleases.Nodes(Me.treleases.Nodes.Count - 1).Nodes.Add(CStr(releases.releases(i).release_number) + ": " + releases.releases(i).release_date)
                    Me.treleases.Nodes(Me.treleases.Nodes.Count - 1).Nodes(Me.treleases.Nodes(Me.treleases.Nodes.Count - 1).Nodes.Count - 1).Nodes.Add(releases.releases(i).desc)
                    Exit For
                End If
            Next
            Me.treleases.Nodes.Add("Previous Releases")
            For i = 0 To Me.releases.releases.Count - 1
                If releases.releases(i).status = bc_om_release.PREVIOUS Then
                    Me.treleases.Nodes(Me.treleases.Nodes.Count - 1).Nodes.Add(CStr(releases.releases(i).release_number) + ": " + releases.releases(i).release_date)
                    Me.treleases.Nodes(Me.treleases.Nodes.Count - 1).Nodes(Me.treleases.Nodes(Me.treleases.Nodes.Count - 1).Nodes.Count - 1).Nodes.Add(releases.releases(i).desc)
                End If
            Next
            Me.treleases.Nodes(0).ExpandAll()

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_deploy", "load_releases", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub

    Private Sub OkToolStripMenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles OkToolStripMenuItem.Click
        create_release()
    End Sub
    Private Sub create_release()
        Dim orel As New bc_om_release
        orel.desc = Me.mreldesc.Text
        orel.create_date = Now
        orel.status = bc_om_release.working
        releases.releases.Add(orel)
        load_releases()
    End Sub

    Private Sub bc_am_cp_deploy_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim bcs As New bc_cs_central_settings(True)
        load_all()
    End Sub

    
    Private Sub treleases_AfterSelect(ByVal sender As System.Object, ByVal e As System.Windows.Forms.TreeViewEventArgs) Handles treleases.AfterSelect
        Me.mset.Visible = False
        Me.mdel.Visible = False
        Me.mdep.Visible = False
        Me.mcr.Visible = False
        If Me.treleases.Nodes(0).Text <> "Working Release" Then
            Me.mcr.Visible = True
        End If
        If Me.treleases.SelectedNode.Text = "Working Release" Then
            Me.mdel.Visible = True
            Me.mdep.Visible = True
        End If

        Try
            If Me.treleases.SelectedNode.Parent.Text = "Working Release" Then
                Me.mdel.Visible = True
                Me.mdep.Visible = True
            End If

            If Me.treleases.SelectedNode.Parent.Text = "Previous Releases" Then
                Me.mset.Visible = True
            End If
        Catch

        End Try
    End Sub

    Private Sub treleases_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles treleases.DoubleClick
        load_all()
    End Sub

    Private Sub mdel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mdel.Click
        delete_working_release()
    End Sub
    Private Sub delete_working_release()
        Try
            For i = 0 To Me.releases.releases.Count - 1
                If Me.releases.releases(i).status = bc_om_release.WORKING Then
                    Me.releases.releases.RemoveAt(i)
                    load_releases()
                    Exit For
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_cp_deploy", "delete_working_release", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub mdep_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mdep.Click
        For i = 0 To Me.releases.releases.Count - 1
            
            If Me.releases.releases(i).status = bc_om_release.ACTIVE Then
                Me.releases.releases(i).status = bc_om_release.PREVIOUS
            End If
            If Me.releases.releases(i).status = bc_om_release.WORKING Then
                Me.releases.releases(i).status = bc_om_release.ACTIVE
                Me.releases.releases(i).release_date = Now
            End If
        Next
        load_releases()
    End Sub
    Private Sub mset_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mset.Click
        Try
            Dim rel_num As Integer

            rel_num = Me.treleases.SelectedNode.Text.Substring(0, 1)
            delete_working_release()
            For i = 0 To Me.releases.releases.Count - 1
                If Me.releases.releases(i).status = bc_om_release.PREVIOUS Then
                    If Me.releases.releases(i).release_number = rel_num Then
                        Dim orel As New bc_om_release
                        orel.status = bc_om_release.WORKING
                        orel.desc = Me.releases.releases(i).desc + "(copied from release: " + CStr(Me.releases.releases(i).release_number)
                        orel.create_date = Now
                        releases.releases.Add(orel)
                        load_releases()
                        Exit For
                    End If
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("mset", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class