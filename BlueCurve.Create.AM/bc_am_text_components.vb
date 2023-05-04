Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.Windows.forms
Public Class bc_am_text_components
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents TextBox1 As System.Windows.Forms.TextBox
    Friend WithEvents ListView1 As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ImageList1 As System.Windows.Forms.ImageList
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents btnapply As System.Windows.Forms.Button
    Friend WithEvents btnclose As System.Windows.Forms.Button
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents Button1 As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_text_components))
        Me.TextBox1 = New System.Windows.Forms.TextBox
        Me.ListView1 = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.btnapply = New System.Windows.Forms.Button
        Me.btnclose = New System.Windows.Forms.Button
        Me.ImageList1 = New System.Windows.Forms.ImageList(Me.components)
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.Button1 = New System.Windows.Forms.Button
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TextBox1
        '
        Me.TextBox1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.TextBox1.Location = New System.Drawing.Point(8, 168)
        Me.TextBox1.Multiline = True
        Me.TextBox1.Name = "TextBox1"
        Me.TextBox1.Size = New System.Drawing.Size(520, 184)
        Me.TextBox1.TabIndex = 0
        '
        'ListView1
        '
        Me.ListView1.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ListView1.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4})
        Me.ListView1.FullRowSelect = True
        Me.ListView1.Location = New System.Drawing.Point(8, 8)
        Me.ListView1.MultiSelect = False
        Me.ListView1.Name = "ListView1"
        Me.ListView1.Size = New System.Drawing.Size(520, 144)
        Me.ListView1.TabIndex = 1
        Me.ListView1.UseCompatibleStateImageBehavior = False
        Me.ListView1.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Component"
        Me.ColumnHeader1.Width = 201
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Length"
        Me.ColumnHeader2.Width = 81
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Characters Used"
        Me.ColumnHeader3.Width = 127
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Mandatory"
        Me.ColumnHeader4.Width = 91
        '
        'btnapply
        '
        Me.btnapply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnapply.Location = New System.Drawing.Point(456, 360)
        Me.btnapply.Name = "btnapply"
        Me.btnapply.Size = New System.Drawing.Size(72, 32)
        Me.btnapply.TabIndex = 2
        Me.btnapply.Text = "Apply Selected"
        '
        'btnclose
        '
        Me.btnclose.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnclose.Location = New System.Drawing.Point(456, 400)
        Me.btnclose.Name = "btnclose"
        Me.btnclose.Size = New System.Drawing.Size(72, 32)
        Me.btnclose.TabIndex = 3
        Me.btnclose.Text = "Close"
        '
        'ImageList1
        '
        Me.ImageList1.ImageStream = CType(resources.GetObject("ImageList1.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.ImageList1.TransparentColor = System.Drawing.Color.Transparent
        Me.ImageList1.Images.SetKeyName(0, "")
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(5, 374)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(184, 59)
        Me.PictureBox2.TabIndex = 8
        Me.PictureBox2.TabStop = False
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 416)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(248, 21)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "© Blue Curve Limited. All rights reserved."
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.Location = New System.Drawing.Point(352, 360)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(72, 32)
        Me.Button1.TabIndex = 10
        Me.Button1.Text = "Accept Changes"
        '
        'bc_am_text_components
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.ClientSize = New System.Drawing.Size(536, 438)
        Me.ControlBox = False
        Me.Controls.Add(Me.Button1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.btnclose)
        Me.Controls.Add(Me.btnapply)
        Me.Controls.Add(Me.ListView1)
        Me.Controls.Add(Me.TextBox1)
        Me.Name = "bc_am_text_components"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue curve - Edit Text Components"
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

#End Region
    Public currentindex As Integer
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnclose.Click
        REM if any mandatory values arnt filled out notify 
        Try
            Dim i, j As Integer


            For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
                REM only interested in types 8 and 9
                With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i)
                    If .mode = 8 Then
                        If .value = "" Then
                            Dim obc_message As New bc_cs_message("Text Components", "Componenet: " + CStr(.name) + " is Mandatory please enter:", bc_cs_message.MESSAGE)
                            For j = 0 To Me.ListView1.Items.Count - 1
                                If Me.ListView1.Items(i).Text = .name Then
                                    Me.ListView1.Items(i).Selected = True
                                    Exit Sub
                                End If
                            Next
                            Exit Sub
                        End If
                    End If
                End With
            Next
            Me.Hide()
        Catch

        End Try
    End Sub

    Private Sub bc_am_text_components_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim i As Integer


        Dim lvw As ListViewItem
        Me.btnapply.Enabled = False
        Me.TextBox1.Enabled = False

        For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
            REM only interested in types 8 and 9
            With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i)
                If .mode = 8 Or .mode = 9 Then
                    lvw = New ListViewItem(CStr(.name))
                    lvw.SubItems.Add(CStr(.mode_param2))
                    lvw.SubItems.Add(Len(.value))
                    If .mode = 8 Then
                        lvw.SubItems.Add("yes")
                    Else
                        lvw.SubItems.Add("no")
                    End If
                    Me.ListView1.Items.Add(lvw)
                End If
            End With
        Next
    End Sub
    Private Sub ListView1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ListView1.SelectedIndexChanged
        Try
            Dim i, j As Integer
            Me.btnapply.Enabled = False
            Me.TextBox1.Enabled = True
            REM applz previos index
            If currentindex <> -1 Then
                apply()
            End If
            Me.TextBox1.Text = ""
            currentindex = Me.ListView1.SelectedItems(0).Index

            For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
                REM only interested in types 8 and 9
                With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i)
                    If .mode = 8 Or .mode = 9 Then
                        If j = Me.ListView1.SelectedItems(0).Index Then
                            Me.TextBox1.Text = .value
                            Me.TextBox1.MaxLength = CStr(.mode_param2)
                            Me.ListView1.SelectedItems(0).SubItems(2).Text = Me.TextBox1.Text.Length
                        End If
                        j = j + 1
                    End If
                End With
            Next
            For i = 0 To Me.ListView1.Items.Count - 1
                Me.ListView1.Items(i).BackColor = System.Drawing.Color.White
            Next
            Me.ListView1.SelectedItems(0).BackColor = System.Drawing.Color.LightBlue
            Me.ListView1.Refresh()
        Catch

        End Try
    End Sub
    Private Sub btnapply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnapply.Click
        Dim i, j As Integer

        For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
            REM only interested in types 8 and 9
            With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i)
                If .mode = 8 Or .mode = 9 Then
                    If j = Me.ListView1.SelectedItems(0).Index And Me.TextBox1.Text <> "" Then
                        bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).value = Me.TextBox1.Text
                        bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).disabled = 1
                        Try
                            bc_am_edit_text_components.odoc.set_value_for_locator(.locator, Me.TextBox1.Text)
                        Catch ex As Exception
                        End Try
                        Exit For
                    End If
                    j = j + 1
                End If
            End With
        Next
        If bc_am_load_objects.obc_current_document.id <> 0 Then
            bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(bc_am_load_objects.obc_current_document.id) + ".dat")
        Else
            bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(bc_am_load_objects.obc_current_document.filename) + ".dat")
        End If
        Me.btnapply.Enabled = False
    End Sub
    Private Sub apply()
        Dim i, j As Integer

        For i = 0 To bc_am_load_objects.obc_current_document.refresh_components.refresh_components.Count - 1
            REM only interested in types 8 and 9
            With bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i)
                If .mode = 8 Or .mode = 9 Then
                    If j = currentindex And Me.TextBox1.Text <> "" Then
                        bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).value = Me.TextBox1.Text
                        bc_am_load_objects.obc_current_document.refresh_components.refresh_components(i).disabled = 1
                        Try
                            bc_am_edit_text_components.odoc.set_value_for_locator(.locator, Me.TextBox1.Text)
                        Catch ex As Exception
                        End Try
                        Exit For
                    End If
                    j = j + 1
                End If
            End With
        Next
        If bc_am_load_objects.obc_current_document.id <> 0 Then
            bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(bc_am_load_objects.obc_current_document.id) + ".dat")
        Else
            bc_am_load_objects.obc_current_document.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(bc_am_load_objects.obc_current_document.filename) + ".dat")
        End If
    End Sub

    Private Sub TextBox1_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TextBox1.TextChanged
        Me.btnapply.Enabled = True
        Me.ListView1.SelectedItems(0).SubItems(2).Text = Me.TextBox1.Text.Length
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        bc_am_edit_text_components.odoc.accept_changes()
        Me.Hide()
    End Sub
End Class
