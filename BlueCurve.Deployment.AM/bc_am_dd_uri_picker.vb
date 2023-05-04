Imports System.Windows.Forms
Imports System.ComponentModel

Public Class bc_am_dd_uri_picker

    Dim pt As UriPickerType
    Dim boolSave As Boolean
    Dim strDescription, strDefaultUri, strUri, strFileMask As String

    Private Const OFFSET As Integer = 38

    Event UriSelected()

    <Browsable(True)> _
    Public Property Type() As UriPickerType
        Get
            Return pt
        End Get
        Set(ByVal pt As UriPickerType)
            Me.pt = pt
        End Set
    End Property

    <Browsable(True)> _
    Public Property Description() As String
        Get
            Return strDescription
        End Get
        Set(ByVal strDescription As String)
            Me.strDescription = strDescription
        End Set
    End Property

    <Browsable(True)> _
    Public Property DefaultUri() As String
        Get
            Return strDefaultUri
        End Get
        Set(ByVal strDefaultUri As String)
            Me.strDefaultUri = strDefaultUri
            strUri = strDefaultUri
        End Set
    End Property

    Public Property Uri() As String
        Get
            Return strUri
        End Get
        Set(ByVal strUri As String)
            Me.strUri = strUri
            txtUri.Text = strUri
        End Set
    End Property

    Public Property FileMask() As String
        Get
            Return strFileMask
        End Get
        Set(ByVal strFileMask As String)
            Me.strFileMask = strFileMask
        End Set
    End Property

    <Browsable(True)> _
    Public Property SaveDialog() As Boolean
        Get
            Return boolSave
        End Get
        Set(ByVal boolSave As Boolean)
            Me.boolSave = boolSave
        End Set
    End Property

    Private Sub btnUri_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnUri.Click

        If Type = UriPickerType.Folder Then

            Dim fbUri As New FolderBrowserDialog()
            fbUri.Description = Description
            If DefaultUri Is Nothing OrElse DefaultUri = "" Then
                fbUri.SelectedPath = "C:\"
            Else
                fbUri.SelectedPath = DefaultUri
            End If

            If fbUri.ShowDialog = DialogResult.OK Then
                Uri = fbUri.SelectedPath & "\"
            End If

            RaiseEvent UriSelected()

        ElseIf Type = UriPickerType.File Then

            Dim fd
            If boolSave Then
                fd = New SaveFileDialog
            Else
                fd = New OpenFileDialog
            End If

            fd.Filter = FileMask
            fd.RestoreDirectory = True

            If fd.ShowDialog() = DialogResult.OK Then
                Uri = fd.FileName
            End If

            RaiseEvent UriSelected()

        End If

    End Sub

    Private Sub txtUri_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txtUri.KeyUp

        Uri = txtUri.Text

        RaiseEvent UriSelected()

    End Sub
End Class
