Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Windows.Forms
Public Class bc_am_picture_attribute
    Implements Ibc_am_picture_attribute
    Event save(filename As String) Implements Ibc_am_picture_attribute.save

    Dim _filename As String
    Public Function load_view(cimage As System.Drawing.Image) As Boolean Implements Ibc_am_picture_attribute.load_view
        Try
            If Not IsNothing(cimage) Then
                Me.uxpicture.Image = cimage
                Me.bdelete.Enabled = True
            End If
            load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_picture_attribue", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub bsave_Click(sender As Object, e As EventArgs) Handles bsave.Click
        RaiseEvent save(_filename)
        Me.Hide()
    End Sub

   
    Private Sub badd_Click(sender As Object, e As EventArgs) Handles badd.Click
        Try
            Dim ofrmfile As New OpenFileDialog
            ofrmfile.Filter = "bitmap (*.bmp)|*.bmp|JPEG Compressed Image (*.jpg)|*.jpg|GIF files (*.gif)|*.gif|PNG (*.png)|*.png"
            ofrmfile.ShowDialog()
            If ofrmfile.FileName = "" Then
                Exit Sub
            End If
            Dim fs As New bc_cs_file_transfer_services

            Dim by As Byte()
            fs.write_document_to_bytestream(ofrmfile.FileName, by, Nothing)
            Dim ms As System.IO.MemoryStream = New System.IO.MemoryStream(by)
            Me.uxpicture.Image = System.Drawing.Image.FromStream(ms)
            _filename = ofrmfile.FileName
             Me.bsave.Enabled = True
            Me.bdelete.Enabled = True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_picture_attribue", "badd_Click", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub

    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles bdelete.Click
        Me.uxpicture.Image = Nothing
        Me.bdelete.Enabled = False
        Me.bsave.Enabled = True

    End Sub
End Class
Public Class Cbc_am_picture_attribue
    WithEvents _view As Ibc_am_picture_attribute
    Public _user As bc_om_user
    Public changed As Boolean = False
   

    Dim _fn As String = ""
    Public Function load_data(user As bc_om_user, view As Ibc_am_picture_attribute, cimage As System.Drawing.Image) As Boolean
        load_data = False
        Try
            _view = view
            _user = user
            If _user.picture_extension <> "" And Not IsNothing(_user.picture) Then
                _fn = bc_cs_central_settings.local_repos_path + CStr(_user.id) + _user.picture_extension
                Dim fs As New bc_cs_file_transfer_services
                fs.write_bytestream_to_document(_fn, _user.picture, Nothing)
            End If
            load_data = _view.load_view(cimage)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_picture_attribue", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function

    Private Sub save(filename As String) Handles _view.save
        Try
            If filename = "" Then
                _user.picture = Nothing
                _user.picture_extension = ""
            Else
                Dim fs As New bc_cs_file_transfer_services
                fs.write_document_to_bytestream(filename, _user.picture, Nothing)
                _user.picture_extension = filename.Substring(InStrRev(filename, ".") - 1, Len(filename) - InStrRev(filename, ".") + 1)
            End If
            changed = True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_picture_attribue", "save", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Sub
End Class
Public Interface Ibc_am_picture_attribute
    Function load_view(cimage As System.Drawing.Image) As Boolean
    Event save(filename As String)
End Interface
