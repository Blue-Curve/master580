Imports BlueCurve.Core.OM
Imports BlueCurve.Core.CS
Imports System.Windows.Forms
Imports System.Collections

Public Class bc_am_cp_edit
    Public cancel_selected As Boolean = True
    Private current_user As bc_om_user = Nothing

    Public Sub New(Optional ByVal pcurrent_user As bc_om_user = Nothing)

        current_user = pcurrent_user
        Call InitializeComponent()

    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tentry.TextChanged
        Me.Bok.Enabled = False
        If Trim(Me.Tentry.Text) <> "" Then
            Me.Bok.Enabled = True
        End If
    End Sub
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Me.Hide()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bok.Click
        If Me.Tlength.Text <> "" And IsNumeric(Me.Tlength.Text) <> True Then
            Dim omsg As New bc_cs_message("Blue Curve", "Length must be a valid number", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Else
            Me.cancel_selected = False
            Me.Hide()
        End If
    End Sub

    Private Sub centry_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles centry.SelectedIndexChanged
        If Me.centry.SelectedIndex > -1 Then
            Me.Bok.Enabled = True
        End If
    End Sub

    Private Sub Cdt_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cdt.SelectedIndexChanged
        Me.Tlength.Enabled = False
        Me.cpopup.Enabled = False
        Me.Tlength.Text = ""
        If Me.Cdt.SelectedIndex = 0 Then
            Me.Tlength.Enabled = True
            Me.cpopup.Enabled = True
        End If

        Me.Bok.Enabled = False
        check_ok()
    End Sub
    Private Sub check_ok()
        If Me.Cdt.SelectedIndex > -1 And Trim(Me.Tattname.Text) <> "" Then
            Me.Bok.Enabled = True
        End If
    End Sub

    Private Sub clookup_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles clookup.CheckedChanged
        If Me.clookup.Checked Then
            Me.Tsql.Enabled = True
            Me.Bok.Enabled = False
            Me.Tlength.Text = ""
            Me.Tlength.Enabled = False
            Me.cpopup.Enabled = False
            Me.cpopup.Checked = False
            If Me.clookup.Checked = True And Trim(Me.Tsql.Text) <> "" And Trim(Me.Tattname.Text) <> "" Then
                Me.Bok.Enabled = True
            End If
        Else
            Me.Tsql.Enabled = False
            If Trim(Me.Tattname.Text) <> "" Then
                Me.Bok.Enabled = True
            Else
                Me.Bok.Enabled = False
            End If
            Me.Tlength.Enabled = True
            Me.cpopup.Enabled = True
        End If
    End Sub

    Private Sub Tsql_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tsql.TextChanged
        Me.Bok.Enabled = True
    End Sub

    Private Sub Tattname_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Tattname.TextChanged
        Me.Bok.Enabled = False
        If Trim(Me.Tattname.Text) <> "" Then
            Me.Bok.Enabled = True
        End If
    End Sub


    Private Sub ltitle_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ltitle.TextChanged

    End Sub

    Private Sub Caudit_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Caudit.CheckedChanged
        check_ok()
    End Sub

    Private Sub cmandatory_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles cmandatory.CheckedChanged
        check_ok()
    End Sub

    Private Sub Cworkflow_CheckedChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cworkflow.CheckedChanged
        check_ok()
    End Sub

    Private Sub bc_am_cp_edit_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub uxRemovePicture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxRemovePicture.Click
        Dim fs As New bc_cs_file_transfer_services
        defaultpic.Image.Save(bc_cs_central_settings.local_repos_path + "tmp.bmp")
        fs.write_document_to_bytestream(bc_cs_central_settings.local_repos_path + "tmp.bmp", current_user.picture, Nothing)
        userpic.ImageLocation = bc_cs_central_settings.local_repos_path + "tmp.bmp"
        userpic.Image = defaultpic.Image

        Me.Bok.Enabled = True
    End Sub


    Private Sub uxAssignPicture_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAssignPicture.Click
        REM add a user image
        Dim ofrmfile As New OpenFileDialog
        ofrmfile.Filter = "bitmap (*.bmp)|*.bmp|JPEG Compressed Image (*.jpg)|*.jpg|GIF files (*.gif)|*.gif"
        ofrmfile.ShowDialog()
        If ofrmfile.FileName = "" Then
            Exit Sub
        End If
        Dim fs As New bc_cs_file_transfer_services
        fs.write_document_to_bytestream(ofrmfile.FileName, current_user.picture, Nothing)
        userpic.ImageLocation = ofrmfile.FileName
        Me.Bok.Enabled = True
        userpic.Image = defaultpic.Image
    End Sub

    Private Sub chkdef_CheckedChanged(sender As Object, e As EventArgs) Handles chkdef.CheckedChanged
        If Me.chkdef.Checked = False Then
            Me.defsql.Text = ""
            Me.defsql.Enabled = False
        Else
            Me.defsql.Enabled = True
        End If
    End Sub
End Class

