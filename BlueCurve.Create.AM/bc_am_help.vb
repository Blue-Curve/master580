Imports BlueCurve.Core.CS
Imports System.io
Imports System.Windows.Forms
Imports System.Collections
Public Class bc_am_help
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
    Friend WithEvents Button1 As System.Windows.Forms.Button
    Friend WithEvents PictureBox3 As System.Windows.Forms.PictureBox
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents Panel2 As System.Windows.Forms.Panel
    Friend WithEvents rtfhelp As System.Windows.Forms.RichTextBox
    Friend WithEvents Panel3 As System.Windows.Forms.Panel
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents bsearch As System.Windows.Forms.Button
    Friend WithEvents bf As System.Windows.Forms.Button
    Friend WithEvents tsearch As System.Windows.Forms.ComboBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_help))
        Me.rtfhelp = New System.Windows.Forms.RichTextBox
        Me.Button1 = New System.Windows.Forms.Button
        Me.PictureBox3 = New System.Windows.Forms.PictureBox
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Panel3 = New System.Windows.Forms.Panel
        Me.tsearch = New System.Windows.Forms.ComboBox
        Me.bf = New System.Windows.Forms.Button
        Me.bsearch = New System.Windows.Forms.Button
        Me.Label1 = New System.Windows.Forms.Label
        Me.Panel2 = New System.Windows.Forms.Panel
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        Me.Panel3.SuspendLayout()
        Me.SuspendLayout()
        '
        'rtfhelp
        '
        Me.rtfhelp.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.rtfhelp.Location = New System.Drawing.Point(16, 72)
        Me.rtfhelp.Name = "rtfhelp"
        Me.rtfhelp.ReadOnly = True
        Me.rtfhelp.Size = New System.Drawing.Size(392, 264)
        Me.rtfhelp.TabIndex = 0
        Me.rtfhelp.Text = ""
        '
        'Button1
        '
        Me.Button1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Button1.BackColor = System.Drawing.Color.Silver
        Me.Button1.Location = New System.Drawing.Point(504, 368)
        Me.Button1.Name = "Button1"
        Me.Button1.Size = New System.Drawing.Size(64, 24)
        Me.Button1.TabIndex = 2
        Me.Button1.Text = "Close"
        Me.Button1.UseVisualStyleBackColor = False
        '
        'PictureBox3
        '
        Me.PictureBox3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.PictureBox3.Image = CType(resources.GetObject("PictureBox3.Image"), System.Drawing.Image)
        Me.PictureBox3.Location = New System.Drawing.Point(0, 0)
        Me.PictureBox3.Name = "PictureBox3"
        Me.PictureBox3.Size = New System.Drawing.Size(672, 64)
        Me.PictureBox3.TabIndex = 42
        Me.PictureBox3.TabStop = False
        '
        'PictureBox1
        '
        Me.PictureBox1.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(12, 354)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(175, 40)
        Me.PictureBox1.TabIndex = 43
        Me.PictureBox1.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel1.Controls.Add(Me.Panel3)
        Me.Panel1.Location = New System.Drawing.Point(0, 64)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(592, 280)
        Me.Panel1.TabIndex = 44
        '
        'Panel3
        '
        Me.Panel3.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel3.BackColor = System.Drawing.Color.Silver
        Me.Panel3.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D
        Me.Panel3.Controls.Add(Me.tsearch)
        Me.Panel3.Controls.Add(Me.bf)
        Me.Panel3.Controls.Add(Me.bsearch)
        Me.Panel3.Controls.Add(Me.Label1)
        Me.Panel3.Location = New System.Drawing.Point(416, 8)
        Me.Panel3.Name = "Panel3"
        Me.Panel3.Size = New System.Drawing.Size(152, 264)
        Me.Panel3.TabIndex = 0
        '
        'tsearch
        '
        Me.tsearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tsearch.Location = New System.Drawing.Point(8, 32)
        Me.tsearch.MaxDropDownItems = 100
        Me.tsearch.Name = "tsearch"
        Me.tsearch.Size = New System.Drawing.Size(128, 20)
        Me.tsearch.Sorted = True
        Me.tsearch.TabIndex = 5
        '
        'bf
        '
        Me.bf.Location = New System.Drawing.Point(104, 64)
        Me.bf.Name = "bf"
        Me.bf.Size = New System.Drawing.Size(32, 24)
        Me.bf.TabIndex = 3
        Me.bf.Text = ">>"
        '
        'bsearch
        '
        Me.bsearch.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bsearch.Location = New System.Drawing.Point(8, 64)
        Me.bsearch.Name = "bsearch"
        Me.bsearch.Size = New System.Drawing.Size(56, 24)
        Me.bsearch.TabIndex = 2
        Me.bsearch.Text = "Search"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Microsoft Sans Serif", 6.75!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(8, 8)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(96, 16)
        Me.Label1.TabIndex = 1
        Me.Label1.Text = "Index"
        '
        'Panel2
        '
        Me.Panel2.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Panel2.BackColor = System.Drawing.Color.White
        Me.Panel2.Location = New System.Drawing.Point(368, 0)
        Me.Panel2.Name = "Panel2"
        Me.Panel2.Size = New System.Drawing.Size(224, 64)
        Me.Panel2.TabIndex = 43
        '
        'bc_am_help
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(584, 406)
        Me.Controls.Add(Me.Panel2)
        Me.Controls.Add(Me.rtfhelp)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.PictureBox3)
        Me.Controls.Add(Me.Button1)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.Name = "bc_am_help"
        Me.Text = "Blue Curve - Help"
        CType(Me.PictureBox3, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel3.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public helpfile As String
    Public selected_keyword As String = ""
    Public Shared keywords As New ArrayList
    Public tcaption As String
    Public load_failed As Boolean = False
    Public search_pos As Integer
    Public Shared loaded As Boolean = False
    Public Shared help_text As String
    Public orig_colour As System.Drawing.Color
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        REM Steve Test
        Me.Hide()
    End Sub

    Private Sub bc_am_help_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim ffs As New bc_cs_file_transfer_services
            If loaded = False Then
                If ffs.check_document_exists(bc_cs_central_settings.local_template_path + helpfile) = False Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Help file not installed: " + helpfile, bc_cs_message.MESSAGE)
                    load_failed = True
                    Exit Sub
                End If
                Dim fs As New StreamReader(bc_cs_central_settings.local_template_path + helpfile, FileMode.Open)
                Me.rtfhelp.Rtf = fs.ReadToEnd
                bc_am_help.help_text = Me.rtfhelp.Rtf
                read_key_file()
            Else
                Me.rtfhelp.Rtf = bc_am_help.help_text
            End If
            Me.Text = tcaption
            Me.tsearch.Items.Clear()
            Dim i As Integer
            For i = 0 To keywords.Count - 1
                Me.tsearch.Items.Add(keywords(i))
            Next
            If Me.selected_keyword <> "" Then
                Me.tsearch.Text = Me.selected_keyword
                search()
            End If
            loaded = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_help_load", "load", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub read_key_file()
        Dim xmlload As New Xml.XmlDocument
        Dim myXmlNodeList As Xml.XmlNodeList

        Dim myxmlnode As Xml.XmlNode
        Dim keyfile As String
        Dim i As Integer
        Try
            keyfile = bc_cs_central_settings.local_template_path + Me.helpfile.Substring(0, Me.helpfile.Length - 4) + "_key.xml"

            Try
                xmlload.Load(keyfile)
            Catch ex As Exception
                Exit Sub
            End Try

            myXmlNodeList = xmlload.SelectNodes("/Keywords/Keyword/name")
            i = 0
            bc_am_help.keywords.Clear()
            For Each myxmlnode In myXmlNodeList
                bc_am_help.keywords.Add(myxmlnode.InnerXml)
                i = i + 1
            Next

        Catch ex As Exception
            MsgBox(ex.Message)
        Finally

        End Try
    End Sub
    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsearch.Click
        search()
    End Sub
    Private Sub search()
        Try
            Try
                Me.rtfhelp.SelectionColor = Me.orig_colour
            Catch

            End Try
            search_pos = Me.rtfhelp.Find(Me.tsearch.Text)
            Try
                Me.orig_colour = Me.rtfhelp.SelectionColor
            Catch

            End Try
            Me.rtfhelp.SelectionColor = Drawing.Color.Red
            Me.rtfhelp.Select(search_pos, Me.tsearch.Text.Length)
            Me.rtfhelp.ScrollToCaret()
        Catch

        End Try
    End Sub

    Private Sub tsearch_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.bsearch.Enabled = True
    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub
    Private Sub bf_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bf.Click
        Try
            Try
                Me.rtfhelp.SelectionColor = Me.orig_colour
            Catch

            End Try
            search_pos = Me.rtfhelp.Find(Me.tsearch.Text, Me.search_pos + Me.tsearch.Text.Length, RichTextBoxFinds.None)
            Try
                Me.orig_colour = Me.rtfhelp.SelectionColor
            Catch

            End Try
            Me.rtfhelp.SelectionColor = Drawing.Color.Red
            Me.rtfhelp.ScrollToCaret()
        Catch
        End Try
    End Sub

    Private Sub tsearch_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tsearch.SelectedIndexChanged
        search()
    End Sub
End Class
