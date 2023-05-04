Imports System.Windows.Forms.Screen
Imports System.io
Imports Microsoft.Win32
Imports System.Windows.forms
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Public Class bc_am_in_retrieval
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
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents litems As System.Windows.Forms.ListView
    Friend WithEvents Item As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader1 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader3 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader4 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader5 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader6 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader2 As System.Windows.Forms.ColumnHeader
    Friend WithEvents ColumnHeader7 As System.Windows.Forms.ColumnHeader
    Friend WithEvents uxDataRetrievalItems As System.Windows.Forms.GroupBox
    Friend WithEvents uxImageList As System.Windows.Forms.ImageList
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_retrieval))
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.litems = New System.Windows.Forms.ListView
        Me.Item = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader1 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader3 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader4 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader5 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader6 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader2 = New System.Windows.Forms.ColumnHeader
        Me.ColumnHeader7 = New System.Windows.Forms.ColumnHeader
        Me.uxImageList = New System.Windows.Forms.ImageList(Me.components)
        Me.uxDataRetrievalItems = New System.Windows.Forms.GroupBox
        Me.uxDataRetrievalItems.SuspendLayout()
        Me.SuspendLayout()
        '
        'litems
        '
        Me.litems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.litems.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.Item, Me.ColumnHeader1, Me.ColumnHeader3, Me.ColumnHeader4, Me.ColumnHeader5, Me.ColumnHeader6, Me.ColumnHeader2, Me.ColumnHeader7})
        Me.litems.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.litems.FullRowSelect = True
        Me.litems.HideSelection = False
        Me.litems.Location = New System.Drawing.Point(8, 25)
        Me.litems.MultiSelect = False
        Me.litems.Name = "litems"
        Me.litems.Size = New System.Drawing.Size(648, 359)
        Me.litems.SmallImageList = Me.uxImageList
        Me.litems.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.litems, "Displays all retrieve values for installation")
        Me.litems.UseCompatibleStateImageBehavior = False
        Me.litems.View = System.Windows.Forms.View.Details
        '
        'Item
        '
        Me.Item.Text = "Item"
        Me.Item.Width = 108
        '
        'ColumnHeader1
        '
        Me.ColumnHeader1.Text = "Output Sheet"
        Me.ColumnHeader1.Width = 102
        '
        'ColumnHeader3
        '
        Me.ColumnHeader3.Text = "Row"
        Me.ColumnHeader3.Width = 41
        '
        'ColumnHeader4
        '
        Me.ColumnHeader4.Text = "Column"
        Me.ColumnHeader4.Width = 64
        '
        'ColumnHeader5
        '
        Me.ColumnHeader5.Text = "Scale Factor"
        Me.ColumnHeader5.Width = 96
        '
        'ColumnHeader6
        '
        Me.ColumnHeader6.Text = "Storage"
        Me.ColumnHeader6.Width = 66
        '
        'ColumnHeader2
        '
        Me.ColumnHeader2.Text = "Dimension"
        Me.ColumnHeader2.Width = 85
        '
        'ColumnHeader7
        '
        Me.ColumnHeader7.Text = "Order"
        '
        'uxImageList
        '
        Me.uxImageList.ImageStream = CType(resources.GetObject("uxImageList.ImageStream"), System.Windows.Forms.ImageListStreamer)
        Me.uxImageList.TransparentColor = System.Drawing.Color.Transparent
        Me.uxImageList.Images.SetKeyName(0, "")
        '
        'uxDataRetrievalItems
        '
        Me.uxDataRetrievalItems.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxDataRetrievalItems.Controls.Add(Me.litems)
        Me.uxDataRetrievalItems.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxDataRetrievalItems.Location = New System.Drawing.Point(8, 8)
        Me.uxDataRetrievalItems.Name = "uxDataRetrievalItems"
        Me.uxDataRetrievalItems.Size = New System.Drawing.Size(664, 392)
        Me.uxDataRetrievalItems.TabIndex = 0
        Me.uxDataRetrievalItems.TabStop = False
        Me.uxDataRetrievalItems.Text = "Data Retrieval Items"
        '
        'bc_am_in_retrieval
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(680, 408)
        Me.ControlBox = False
        Me.Controls.Add(Me.uxDataRetrievalItems)
        Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None
        Me.Name = "bc_am_in_retrieval"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.uxDataRetrievalItems.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public tk_main As Object
    Private Sub bc_am_in_retrieval_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        load_start()
    End Sub
    Public Sub load_start()

        Dim i, j As Integer
        Dim lvew As ListViewItem = Nothing
        Dim found As Boolean

        Me.litems.Items.Clear()

        REM now load items

        For i = 0 To bc_am_insight_retrieve_values.retrieve_values.Count - 1
            found = False
            For j = 0 To bc_am_in_context.insight_items.insight_items.Count - 1
                If bc_am_in_context.insight_items.insight_items(j).label_code = CStr(bc_am_insight_retrieve_values.retrieve_values(i).attribute_code) Then
                    found = True
                    lvew = New ListViewItem(CStr(bc_am_in_context.insight_items.insight_items(j).desc), 0)
                    Exit For
                End If
            Next
            If found = False Then
                lvew = New ListViewItem(CStr(bc_am_insight_retrieve_values.retrieve_values(i).attribute_code), 0)
            End If

            If bc_am_insight_retrieve_values.retrieve_values(i).sheet = "" Then
                lvew.SubItems.Add("System Sheets")
            Else
                lvew.SubItems.Add(CStr(bc_am_insight_retrieve_values.retrieve_values(i).sheet))
            End If
            lvew.SubItems.Add(CStr(bc_am_insight_retrieve_values.retrieve_values(i).row))
            lvew.SubItems.Add(CStr(bc_am_insight_retrieve_values.retrieve_values(i).col))
            lvew.SubItems.Add(CStr(bc_am_insight_retrieve_values.retrieve_values(i).scale_factor))
            Select Case bc_am_insight_retrieve_values.retrieve_values(i).submission_code
                Case 10
                    lvew.SubItems.Add("class")
                    lvew.SubItems.Add(CStr(bc_am_insight_retrieve_values.retrieve_values(i).dimension))
                    lvew.SubItems.Add("n/a")
                Case 1
                    lvew.SubItems.Add("value")
                    lvew.SubItems.Add(CStr(bc_am_insight_retrieve_values.retrieve_values(i).dimension))
                    lvew.SubItems.Add("n/a")
                Case 2
                    lvew.SubItems.Add("time series")
                    lvew.SubItems.Add(CStr(bc_am_insight_retrieve_values.retrieve_values(i).dimension))
                    lvew.SubItems.Add("n/a")
                Case 3
                    lvew.SubItems.Add("repeating")
                    lvew.SubItems.Add(CStr(bc_am_insight_retrieve_values.retrieve_values(i).dimension))
                    lvew.SubItems.Add(CStr(bc_am_insight_retrieve_values.retrieve_values(i).order))
                Case 4
                    lvew.SubItems.Add("repeating time series")
                    lvew.SubItems.Add(CStr(bc_am_insight_retrieve_values.retrieve_values(i).dimension))
                    lvew.SubItems.Add(CStr(bc_am_insight_retrieve_values.retrieve_values(i).order))

            End Select

            Me.litems.Items.Add(lvew)
        Next
    End Sub


    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)

    End Sub













    Public Sub create_xml_file(Optional ByVal show_message As Boolean = True, Optional ByVal upload_to_server As Boolean = True)
        Dim sxml As String = ""
        Dim ixml As String
        Dim i, j As Integer
        Dim found As Boolean
        Try
            Dim fs As bc_cs_file_transfer_services

            sxml = sxml + "<retrieve_values>" + vbCrLf

            For i = 0 To Me.litems.Items.Count - 1
                found = False
                sxml = sxml + "<retrieve_value>"
                sxml = sxml + "<attribute_code>"
                For j = 0 To bc_am_in_context.insight_items.class_names.Count - 1
                    If bc_am_in_context.insight_items.class_names(j) = Me.litems.Items(i).SubItems(0).Text Then
                        found = True
                        Exit For
                    End If
                Next
                If found = True Then
                    sxml = sxml + Me.litems.Items(i).SubItems(0).Text
                Else
                    For j = 0 To bc_am_in_context.insight_items.insight_items.Count - 1

                        If bc_am_in_context.insight_items.insight_items(j).row_flag = 0 Then
                            If bc_am_in_context.insight_items.insight_items(j).desc = Me.litems.Items(i).SubItems(0).Text Then
                                sxml = sxml + bc_am_in_context.insight_items.insight_items(j).label_code
                            End If
                        End If
                    Next
                End If
                sxml = sxml + "</attribute_code>" + vbCrLf

                sxml = sxml + "<sheet>"
                If Me.litems.Items(i).SubItems(1).Text <> "System Sheets" Then
                    sxml = sxml + Me.litems.Items(i).SubItems(1).Text
                End If
                sxml = sxml + "</sheet>" + vbCrLf
                sxml = sxml + "<scale_factor>"
                sxml = sxml + Me.litems.Items(i).SubItems(4).Text
                sxml = sxml + "</scale_factor>" + vbCrLf
                sxml = sxml + "<use_entity_id>"
                If Me.litems.Items(i).SubItems(1).Text <> "System Sheets" Then
                    sxml = sxml + "0"
                Else
                    sxml = sxml + "1"
                End If
                sxml = sxml + "</use_entity_id>" + vbCrLf

                sxml = sxml + "<row>"
                sxml = sxml + Me.litems.Items(i).SubItems(2).Text
                sxml = sxml + "</row>" + vbCrLf

                sxml = sxml + "<col>"
                sxml = sxml + Me.litems.Items(i).SubItems(3).Text
                sxml = sxml + "</col>" + vbCrLf

                sxml = sxml + "<submission_code>"
                If Me.litems.Items(i).SubItems(5).Text = "value" Then
                    sxml = sxml + "1"
                ElseIf Me.litems.Items(i).SubItems(5).Text = "time series" Then
                    sxml = sxml + "2"
                ElseIf Me.litems.Items(i).SubItems(5).Text = "repeating" Then
                    sxml = sxml + "3"
                ElseIf Me.litems.Items(i).SubItems(5).Text = "class" Then
                    sxml = sxml + "10"
                Else
                    sxml = sxml + "4"
                End If
                sxml = sxml + "</submission_code>" + vbCrLf
                sxml = sxml + "<order>"
                If Me.litems.Items(i).SubItems(7).Text = "n/a" Then
                    sxml = sxml + "0"
                Else
                    sxml = sxml + Me.litems.Items(i).SubItems(7).Text()
                End If
                sxml = sxml + "</order>" + vbCrLf
                sxml = sxml + "<dimension>"
                sxml = sxml + Me.litems.Items(i).SubItems(6).Text()
                sxml = sxml + "</dimension>" + vbCrLf

                sxml = sxml + "</retrieve_value>" + vbCrLf
            Next

            sxml = sxml + "</retrieve_values>"
            REM read in exisiting config file if it exists
            fs = New bc_cs_file_transfer_services
            If fs.check_document_exists(bc_cs_central_settings.local_template_path + "bc_am_insight_config.xml") Then
                Dim ifs As New StreamReader(bc_cs_central_settings.local_template_path + "bc_am_insight_config.xml", False)
                ixml = ifs.ReadToEnd
                ifs.Close()
                REM strip out existing retrueve settings bit
                i = InStr(ixml, "<retrieve_values>", CompareMethod.Text)
                If i > 0 Then
                    ixml = ixml.Substring(0, i - 1)
                    sxml = ixml + sxml + vbCrLf + "</insight_settings>"
                Else
                    i = InStr(ixml, "</insight_settings>", CompareMethod.Text)
                    ixml = ixml.Substring(0, i - 1)
                    sxml = ixml + sxml + vbCrLf + "</insight_settings>"
                End If
            Else
                sxml = "<insight_settings>" + vbCrLf + sxml + vbCrLf + "</insight_settings>"
            End If
            REM now write out file overwritting previous copy
            Dim sfs As New StreamWriter(bc_cs_central_settings.local_template_path + "bc_am_insight_config.xml", False)
            sfs.Write(sxml)
            sfs.Close()
            If upload_to_server = True Then
                fs = New bc_cs_file_transfer_services
                Dim oinsight_format As New bc_om_insight_config_files
                fs.write_document_to_bytestream(bc_cs_central_settings.local_template_path + "bc_am_insight_config.xml", oinsight_format.xml_file, Nothing)
                oinsight_format.format_file_name = "ignore"

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    oinsight_format.db_write()
                    If show_message = True Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Upload Complete", bc_cs_message.MESSAGE)
                    End If
                Else
                    oinsight_format.no_send_back = True
                    oinsight_format.tmode = bc_cs_soap_base_class.tWRITE
                    If oinsight_format.transmit_to_server_and_receive(oinsight_format, True) = True Then
                        If show_message = True Then
                            Dim omsg As New bc_cs_message("Blue Curve", "Upload Complete", bc_cs_message.MESSAGE)
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_retrieval", "create_xml_file", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub


    Public Sub set_change()
        tk_main.set_change(3)
    End Sub

    Private Sub litems_SelectedIndexChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles litems.SelectedIndexChanged
        If litems.SelectedItems.Count = 0 Then
            tk_main.set_button_disabled(1)
            tk_main.set_button_disabled(2)
            Exit Sub
        End If
        tk_main.set_button_enabled(1)
        tk_main.set_button_enabled(2)
    End Sub

    Private Sub litems_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles litems.DoubleClick
        tk_main.ret_item(False)
    End Sub
End Class