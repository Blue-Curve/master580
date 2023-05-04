Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports BlueCurve.Create.AM
Imports System.Windows.Forms
Imports System.Collections
Imports System.IO

Public Class bc_am_at_supporting_docs_mgmt
    Inherits System.Windows.Forms.Form

    Private ldoc As New bc_om_document
    Private supportingDocs As New ArrayList
    Private selectedDocId As Long
    Private tempSupportingDocId As Integer = 0


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
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents blogo As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents cmdOK As System.Windows.Forms.Button
    Friend WithEvents cmdCancel As System.Windows.Forms.Button
    Friend WithEvents docComment As System.Windows.Forms.TextBox
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents ViewSupportDoc As System.Windows.Forms.PictureBox
    Friend WithEvents title As System.Windows.Forms.ColumnHeader
    Friend WithEvents id As System.Windows.Forms.ColumnHeader
    Friend WithEvents dat As System.Windows.Forms.ColumnHeader
    Friend WithEvents type As System.Windows.Forms.ColumnHeader
    Friend WithEvents checkedout As System.Windows.Forms.ColumnHeader
    Friend WithEvents AddNewSupportingDoc As System.Windows.Forms.PictureBox
    Friend WithEvents RemoveSupportingDoc As System.Windows.Forms.PictureBox
    Friend WithEvents OpenFileDialog As System.Windows.Forms.OpenFileDialog
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents SupportingDocsList As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents btnset As System.Windows.Forms.Button
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_at_supporting_docs_mgmt))
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.SupportingDocsList = New System.Windows.Forms.ListView
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.docComment = New System.Windows.Forms.TextBox
        Me.Label1 = New System.Windows.Forms.Label
        Me.RemoveSupportingDoc = New System.Windows.Forms.PictureBox
        Me.AddNewSupportingDoc = New System.Windows.Forms.PictureBox
        Me.ViewSupportDoc = New System.Windows.Forms.PictureBox
        Me.btnset = New System.Windows.Forms.Button
        Me.title = New System.Windows.Forms.ColumnHeader
        Me.id = New System.Windows.Forms.ColumnHeader
        Me.dat = New System.Windows.Forms.ColumnHeader
        Me.type = New System.Windows.Forms.ColumnHeader
        Me.checkedout = New System.Windows.Forms.ColumnHeader
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.blogo = New System.Windows.Forms.PictureBox
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.cmdOK = New System.Windows.Forms.Button
        Me.cmdCancel = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.OpenFileDialog = New System.Windows.Forms.OpenFileDialog
        Me.Panel1.SuspendLayout()
        CType(Me.RemoveSupportingDoc, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.AddNewSupportingDoc, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.ViewSupportDoc, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.blogo, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.SupportingDocsList)
        Me.Panel1.Controls.Add(Me.docComment)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Controls.Add(Me.RemoveSupportingDoc)
        Me.Panel1.Controls.Add(Me.AddNewSupportingDoc)
        Me.Panel1.Controls.Add(Me.ViewSupportDoc)
        Me.Panel1.Controls.Add(Me.btnset)
        Me.Panel1.Location = New System.Drawing.Point(0, 64)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(552, 264)
        Me.Panel1.TabIndex = 4
        '
        'SupportingDocsList
        '
        Me.SupportingDocsList.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.SupportingDocsList.BackColor = System.Drawing.Color.White
        Me.SupportingDocsList.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader1, Me.ColumnHeader2, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5})
        Me.SupportingDocsList.Font = New System.Drawing.Font("Verdana", 7.25!)
        Me.SupportingDocsList.FullRowSelect = True
        Me.SupportingDocsList.Location = New System.Drawing.Point(8, 8)
        Me.SupportingDocsList.MultiSelect = False
        Me.SupportingDocsList.Name = "SupportingDocsList"
        Me.SupportingDocsList.Size = New System.Drawing.Size(488, 184)
        Me.SupportingDocsList.TabIndex = 21
        Me.SupportingDocsList.UseCompatibleStateImageBehavior = False
        Me.SupportingDocsList.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Title"
        Me.ColumnHeader1.Width = 242
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "id"
        Me.ColumnHeader2.Width = 0
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Date"
        Me.ColumnHeader3.Width = 91
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Type"
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Checked Out "
        Me.ColumnHeader5.Width = 89
        '
        'docComment
        '
        Me.docComment.Anchor = CType(((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.docComment.Enabled = False
        Me.docComment.Location = New System.Drawing.Point(8, 214)
        Me.docComment.Multiline = True
        Me.docComment.Name = "docComment"
        Me.docComment.Size = New System.Drawing.Size(488, 40)
        Me.docComment.TabIndex = 1
        '
        'Label1
        '
        Me.Label1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label1.Location = New System.Drawing.Point(8, 200)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(104, 16)
        Me.Label1.TabIndex = 20
        Me.Label1.Text = "Comments:"
        '
        'RemoveSupportingDoc
        '
        Me.RemoveSupportingDoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.RemoveSupportingDoc.BackgroundImage = CType(resources.GetObject("RemoveSupportingDoc.BackgroundImage"), System.Drawing.Image)
        Me.RemoveSupportingDoc.Image = CType(resources.GetObject("RemoveSupportingDoc.Image"), System.Drawing.Image)
        Me.RemoveSupportingDoc.Location = New System.Drawing.Point(506, 112)
        Me.RemoveSupportingDoc.Name = "RemoveSupportingDoc"
        Me.RemoveSupportingDoc.Size = New System.Drawing.Size(32, 32)
        Me.RemoveSupportingDoc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.RemoveSupportingDoc.TabIndex = 18
        Me.RemoveSupportingDoc.TabStop = False
        '
        'AddNewSupportingDoc
        '
        Me.AddNewSupportingDoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.AddNewSupportingDoc.BackgroundImage = CType(resources.GetObject("AddNewSupportingDoc.BackgroundImage"), System.Drawing.Image)
        Me.AddNewSupportingDoc.Image = CType(resources.GetObject("AddNewSupportingDoc.Image"), System.Drawing.Image)
        Me.AddNewSupportingDoc.Location = New System.Drawing.Point(506, 72)
        Me.AddNewSupportingDoc.Name = "AddNewSupportingDoc"
        Me.AddNewSupportingDoc.Size = New System.Drawing.Size(32, 32)
        Me.AddNewSupportingDoc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.AddNewSupportingDoc.TabIndex = 17
        Me.AddNewSupportingDoc.TabStop = False
        '
        'ViewSupportDoc
        '
        Me.ViewSupportDoc.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ViewSupportDoc.BackgroundImage = CType(resources.GetObject("ViewSupportDoc.BackgroundImage"), System.Drawing.Image)
        Me.ViewSupportDoc.Image = CType(resources.GetObject("ViewSupportDoc.Image"), System.Drawing.Image)
        Me.ViewSupportDoc.Location = New System.Drawing.Point(506, 32)
        Me.ViewSupportDoc.Name = "ViewSupportDoc"
        Me.ViewSupportDoc.Size = New System.Drawing.Size(32, 32)
        Me.ViewSupportDoc.SizeMode = System.Windows.Forms.PictureBoxSizeMode.CenterImage
        Me.ViewSupportDoc.TabIndex = 13
        Me.ViewSupportDoc.TabStop = False
        Me.ToolTip1.SetToolTip(Me.ViewSupportDoc, "View support document")
        '
        'btnset
        '
        Me.btnset.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.btnset.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.btnset.Location = New System.Drawing.Point(504, 216)
        Me.btnset.Name = "btnset"
        Me.btnset.Size = New System.Drawing.Size(40, 32)
        Me.btnset.TabIndex = 15
        Me.btnset.Text = "&Set"
        Me.btnset.UseVisualStyleBackColor = False
        '
        'title
        '
        Me.title.Text = "Title"
        Me.title.Width = 240
        '
        'id
        '
        Me.id.Text = "id"
        Me.id.Width = 0
        '
        'dat
        '
        Me.dat.Text = "Date"
        Me.dat.Width = 79
        '
        'type
        '
        Me.type.Text = "Type"
        Me.type.Width = 68
        '
        'checkedout
        '
        Me.checkedout.Text = "Checked Out "
        Me.checkedout.Width = 91
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(440, 64)
        Me.PictureBox1.TabIndex = 5
        Me.PictureBox1.TabStop = False
        '
        'blogo
        '
        Me.blogo.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.blogo.Image = CType(resources.GetObject("blogo.Image"), System.Drawing.Image)
        Me.blogo.Location = New System.Drawing.Point(492, -4)
        Me.blogo.Name = "blogo"
        Me.blogo.Size = New System.Drawing.Size(64, 64)
        Me.blogo.TabIndex = 10
        Me.blogo.TabStop = False
        '
        'PictureBox2
        '
        Me.PictureBox2.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(5, 336)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(184, 48)
        Me.PictureBox2.TabIndex = 11
        Me.PictureBox2.TabStop = False
        '
        'Label3
        '
        Me.Label3.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.Label3.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(4, 373)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(248, 21)
        Me.Label3.TabIndex = 12
        Me.Label3.Text = "© Blue Curve Limited. All rights reserved."
        '
        'cmdOK
        '
        Me.cmdOK.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdOK.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdOK.Location = New System.Drawing.Point(472, 336)
        Me.cmdOK.Name = "cmdOK"
        Me.cmdOK.Size = New System.Drawing.Size(80, 24)
        Me.cmdOK.TabIndex = 13
        Me.cmdOK.Text = "&OK"
        Me.cmdOK.UseVisualStyleBackColor = False
        '
        'cmdCancel
        '
        Me.cmdCancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cmdCancel.BackColor = System.Drawing.Color.FromArgb(CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer), CType(CType(224, Byte), Integer))
        Me.cmdCancel.Location = New System.Drawing.Point(472, 368)
        Me.cmdCancel.Name = "cmdCancel"
        Me.cmdCancel.Size = New System.Drawing.Size(80, 24)
        Me.cmdCancel.TabIndex = 14
        Me.cmdCancel.Text = "&Cancel"
        Me.cmdCancel.UseVisualStyleBackColor = False
        '
        'bc_am_at_supporting_docs_mgmt
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(560, 398)
        Me.Controls.Add(Me.cmdCancel)
        Me.Controls.Add(Me.cmdOK)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PictureBox2)
        Me.Controls.Add(Me.blogo)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Panel1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "bc_am_at_supporting_docs_mgmt"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve Create - Supporting Documents"
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.RemoveSupportingDoc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.AddNewSupportingDoc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.ViewSupportDoc, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.blogo, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub ViewSupportDoc_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles ViewSupportDoc.MouseDown
        ViewSupportDoc.BorderStyle = BorderStyle.Fixed3D
    End Sub

   

    Private Sub AddNewSupportingDoc_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles AddNewSupportingDoc.MouseDown
        AddNewSupportingDoc.BorderStyle = BorderStyle.Fixed3D
    End Sub

    Private Sub AddNewSupportingDoc_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles AddNewSupportingDoc.MouseUp
        Try
            If OpenFileDialog.ShowDialog() = DialogResult.OK And OpenFileDialog.FileName <> "" Then
                REM create a supporting document record
                Dim sdoc As New bc_om_document
                sdoc.id = 0
                sdoc.master_flag = False
                sdoc.pub_type_id = ldoc.pub_type_id
                sdoc.pub_type_name = ldoc.pub_type_name
                sdoc.originating_author = ldoc.originating_author
                sdoc.bus_area = ldoc.bus_area
                sdoc.checked_out_user = 0
                sdoc.doc_date = ldoc.doc_date
                sdoc.entity_id = ldoc.entity_id
                sdoc.originating_author = ldoc.originating_author
                sdoc.title = OpenFileDialog.FileName.Substring(0, Len(OpenFileDialog.FileName) - 4)
                sdoc.title = sdoc.title.Substring(InStrRev(sdoc.title, "\"), Len(sdoc.title) - InStrRev(sdoc.title, "\"))
             
                sdoc.extension = OpenFileDialog.FileName.Substring(Len(OpenFileDialog.FileName) - 4, 4)
                sdoc.register_only = False
                sdoc.filename = OpenFileDialog.FileName
                sdoc.bwith_document = True
                sdoc.support_doc_state = 2

                Dim fs As New bc_cs_file_transfer_services
                If fs.write_document_to_bytestream(OpenFileDialog.FileName, sdoc.byteDoc, Nothing, False) = False Then
                    Dim omessage As New bc_cs_message("Blue Curve", "File: " + OpenFileDialog.FileName + " can't be accessed", bc_cs_message.MESSAGE)
                    Exit Sub
                End If
                REM now add to master document support docs list
                ldoc.support_documents.document.Add(sdoc)
                addToListView(sdoc)
            End If
            AddNewSupportingDoc.BorderStyle = BorderStyle.None

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_supporting_docs_mgmt", "AddNewSupportingDoc", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try

    End Sub

    Private Function nextNewSupportingDocId() As Integer
        tempSupportingDocId -= 1
        nextNewSupportingDocId = tempSupportingDocId
    End Function

    Private Sub RemoveSupportingDoc_MouseUp(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RemoveSupportingDoc.MouseUp
        If SupportingDocsList.SelectedItems.Count > 0 Then
            'Remove from collection
            supportingDocs.Remove(selectedDocId)
            'Remove from listview
            SupportingDocsList.SelectedItems(0).Remove()
            docComment.Clear()
            selectedDocId = 0
        Else
            Dim omessage = New bc_cs_message("Blue Curve create", "Please select a supporting document from the list first", bc_cs_message.MESSAGE)
        End If

        RemoveSupportingDoc.BorderStyle = BorderStyle.None
    End Sub

    Private Sub RemoveSupportingDoc_MouseDown(ByVal sender As Object, ByVal e As System.Windows.Forms.MouseEventArgs) Handles RemoveSupportingDoc.MouseDown
        RemoveSupportingDoc.BorderStyle = BorderStyle.Fixed3D
    End Sub

    Private Sub cmdOK_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdOK.Click
        If ldoc.id > 0 Then
            ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.id) + ".dat")
        Else
            ldoc.write_data_to_file(bc_cs_central_settings.local_repos_path + CStr(ldoc.filename) + ".dat")
        End If
        Me.Close()
    End Sub

    Private Sub cmdCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmdCancel.Click
        Me.Close()
    End Sub

    'Called to load list view with any existing supporting documents for a specific master document
    Public Function Initialize(ByVal ao_object As Object, ByVal ao_type As String) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_am_at_supporting_docs_mgmt", "Initialize", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim success As Boolean = True

        Try
            Dim omessage As bc_cs_message
            Dim odoc As bc_ao_at_object = Nothing
            Dim name As String
            Dim fs As New bc_cs_file_transfer_services
            Dim settings As bc_cs_central_settings = New bc_cs_central_settings(True)

            If ao_type <> bc_ao_at_object.WORD_DOC And ao_type <> bc_ao_at_object.POWERPOINT_DOC Then
                omessage = New bc_cs_message("Blue Curve create", "Application Object Type: " + ao_type + " not supported", bc_cs_message.MESSAGE)
                otrace = New bc_cs_activity_log("bc_am_at_supporting_docs_mgmt", "Initialize", bc_cs_activity_codes.TRACE_EXIT, "")
                Exit Function
            End If

            REM instantiate correct AO object
            If ao_type = bc_ao_at_object.WORD_DOC Then
                odoc = New bc_ao_word(ao_object)
            End If
            If ao_type = bc_ao_at_object.POWERPOINT_DOC Then
                omessage = New bc_cs_message("Blue Curve create", "Not Yet Implemented!", bc_cs_message.MESSAGE)
                Exit Function
            End If
            name = odoc.get_doc_id
            Dim recreate_flag As Boolean
            If name = "" Or name = "NONE" Then
                name = ldoc.id
            End If
            If fs.check_document_exists_and_can_be_read(bc_cs_central_settings.local_repos_path + name + ".dat") = False Then
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.LOCAL Then
                    omessage = New bc_cs_message("Blue Curve create", "Cannot Submit Document as no network and document was not opened via Create.", bc_cs_message.MESSAGE)
                    Exit Function
                End If
                REM xml metadata file doesnt exists so attempt to recreate
                Dim ordm As New bc_am_recreate_doc_metadata
                If ordm.recreate_doc_metadata(name, odoc) = False Then
                    Exit Function
                Else
                    ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
                    recreate_flag = True
                End If
            Else
                REM load metadata
                ldoc = ldoc.read_data_from_file(bc_cs_central_settings.local_repos_path + name + ".dat")
            End If

            bc_cs_central_settings.selected_conn_method = ldoc.connection_method

            'rem now load list of supporting docs into GUI
            Dim i As Integer
            For i = 0 To ldoc.support_documents.document.Count - 1
                addToListView(ldoc.support_documents.document(i))
            Next
            RemoveSupportingDoc.Visible = False
            Me.ViewSupportDoc.Enabled = False


            'Determine if we have to force a load of the supporting docs information
            'that isn't included by default

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_supporting_docs_mgmt", "Initialize", bc_cs_error_codes.USER_DEFINED, ex.Message)
            success = False
        Finally

            otrace = New bc_cs_activity_log("bc_am_at_supporting_docs_mgmt", "Initialize", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

        Initialize = success

    End Function

    'Write the details of the supporting doc object to the list view
    Private Sub addToListView(ByVal supportingDoc As bc_om_document)
        If supportingDoc.isdeleted Then Exit Sub

        With SupportingDocsList
            Dim newListItem As ListViewItem = .Items.Add(supportingDoc.title)
            newListItem.SubItems.Add(supportingDoc.id)
            newListItem.SubItems.Add(supportingDoc.doc_date.ToShortDateString())
            newListItem.SubItems.Add(supportingDoc.extension)
            If supportingDoc.support_doc_state = 2 Then
                newListItem.SubItems.Add("New")
            Else
                newListItem.SubItems.Add(IIf(supportingDoc.checked_out_user > 0, "Yes", "No"))
            End If

        End With
    End Sub



    Private Sub AddNewSupportingDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles AddNewSupportingDoc.Click

    End Sub

    Private Sub bc_am_at_supporting_docs_mgmt_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load

    End Sub

    Private Sub bc_am_at_supporting_docs_mgmt_DragEnter(ByVal sender As Object, ByVal e As System.Windows.Forms.DragEventArgs) Handles MyBase.DragEnter

    End Sub

    Private Sub SupportingDocsList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles SupportingDocsList.SelectedIndexChanged
        Try
            Me.RemoveSupportingDoc.Visible = False
            Me.docComment.Enabled = False
            Me.docComment.Text = ""
            Me.ViewSupportDoc.Enabled = True
            Me.btnset.Visible = False

            Dim i As Integer
            For i = 0 To ldoc.support_documents.document.Count - 1
                If SupportingDocsList.SelectedItems(0).SubItems(1).Text <> "0" Then
                    If SupportingDocsList.SelectedItems(0).SubItems(1).Text = CStr(ldoc.support_documents.document(i).id) Then
                        Me.docComment.Text = ldoc.support_documents.document(i).summary

                        Exit For
                    End If

                End If
                If SupportingDocsList.SelectedItems(0).SubItems(1).Text = "0" Then
                    If SupportingDocsList.SelectedItems(0).SubItems(0).Text = CStr(ldoc.support_documents.document(i).title) Then
                        Me.docComment.Text = ldoc.support_documents.document(i).summary
                        Exit For
                    End If

                End If
            Next
            If SupportingDocsList.SelectedItems(0).SubItems(4).Text = "New" Then
                Me.RemoveSupportingDoc.Visible = True
                Me.docComment.Enabled = True
                Me.btnset.Enabled = False
                Me.btnset.Visible = True
            End If
        Catch
        End Try
    End Sub

    Private Sub RemoveSupportingDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles RemoveSupportingDoc.Click

    End Sub

    Private Sub ViewSupportDoc_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ViewSupportDoc.Click
        Dim owf As New bc_am_workflow
        Dim omime As New bc_am_mime_types
        Dim ext As String
        Dim i As Integer
        Dim fn As String
        For i = 0 To ldoc.support_documents.document.Count - 1
            If SupportingDocsList.SelectedItems(0).SubItems(1).Text <> "0" Then
                If SupportingDocsList.SelectedItems(0).SubItems(1).Text = CStr(ldoc.support_documents.document(i).id) Then
                    owf.look_at_support_doc(ldoc.support_documents.document(i).id, ldoc)
                    Exit For
                End If

            End If
            If SupportingDocsList.SelectedItems(0).SubItems(1).Text = "0" Then
                If SupportingDocsList.SelectedItems(0).SubItems(0).Text = CStr(ldoc.support_documents.document(i).title) Then
                    ext = ldoc.support_documents.document(i).extension

                    fn = owf.save_view_only_file(ldoc.support_documents.document(i))
                    If fn <> "" Then
                        omime = New bc_am_mime_types
                        omime.view(bc_cs_central_settings.local_repos_path + fn + ext, ext)
                        REM if AT document then mark as read only else it may get submitted or refreshed
                    End If
                    Exit For
                End If

            End If
        Next
        Me.ViewSupportDoc.BorderStyle = BorderStyle.None



    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnset.Click
        Dim i As Integer
        For i = 0 To ldoc.support_documents.document.Count - 1
            If SupportingDocsList.SelectedItems(0).SubItems(1).Text = "0" Then
                If SupportingDocsList.SelectedItems(0).SubItems(0).Text = CStr(ldoc.support_documents.document(i).title) Then
                    ldoc.support_documents.document(i).summary = Me.docComment.Text
                    Me.btnset.Enabled = False
                    Exit For
                End If

            End If
        Next
    End Sub

    Private Sub docComment_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles docComment.TextChanged
        Me.btnset.Enabled = True
    End Sub
End Class
