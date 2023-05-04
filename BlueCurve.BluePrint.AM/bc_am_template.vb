Imports BlueCurve.Core.CS
Imports BlueCurve.Core.AS
Imports BlueCurve.Core.OM
Imports System.Text
Imports System.IO
Imports System.Xml
Imports System.Windows.Forms

''' <summary>
''' Module:         BluePrint
''' Type:           AM
''' Desciption:     template management
''' Version:        1.0
''' Change history:
''' </summary>
''' 

Public Class bc_am_template

    Private Const WORD_EXT = ".dot"
    Private Const POWERPOINT_EXT = ".pot"
    Private Const EXCEL_EXT = ".xls"
    Private Const WORD2007_EXT = ".dotx"
    Private Const POWERPOINT2007_EXT = ".potx"
    Private Const EXCEL2007_EXT = ".xlsx"
    Private Const WORD2007_MACRO_ENABLED_EXT = ".dotm"
    Private Const POWERPOINT2007_MACRO_ENABLED_EXT = ".potm"
    Private Const EXCEL2007_MACRO_ENABLED_EXT = ".xlsm"

    Private fileExt As String
    Private fileName As String

    Public Shared column As Integer
    Public Shared SortToggle As Boolean = True

    Private viewClone As bc_am_bp_clone
    Private viewEditTemplateDetails As bc_am_bp_edit_template_details

    Friend Sub New(ByVal view As bc_am_bp_edit_template_details, ByVal filePathName As String)

        Dim extensionsize As Integer = 0

        view.Controller = Me
        Me.viewEditTemplateDetails = view

        REM SW cope with office versions
        extensionsize = (Len(filePathName) - (InStrRev(filePathName, ".") - 1))

        fileExt = Right(filePathName, extensionsize)
        fileName = Left(Mid(filePathName, InStrRev(filePathName, "\") + 1), _
                        Len(Mid(filePathName, InStrRev(filePathName, "\") + 1)) - extensionsize)

    End Sub

    Friend Sub New(ByVal view As bc_am_bp_clone, ByVal filePathName As String)

        Dim extensionsize As Integer = 0

        view.Controller = Me
        Me.viewClone = view

        REM SW cope with office versions
        extensionsize = (Len(filePathName) - (InStrRev(filePathName, ".") - 1))

        fileExt = Right(filePathName, extensionsize)
        fileName = Left(Mid(filePathName, InStrRev(filePathName, "\") + 1), _
                        Len(Mid(filePathName, InStrRev(filePathName, "\") + 1)) - extensionsize)

    End Sub

    Public Sub New(ByVal filePathName As String)

        Dim extensionsize As Integer = 0

        REM SW cope with office versions
        extensionsize = (Len(filePathName) - (InStrRev(filePathName, ".") - 1))

        fileExt = Right(filePathName, extensionsize)
        fileName = Left(Mid(filePathName, InStrRev(filePathName, "\") + 1), _
                        Len(Mid(filePathName, InStrRev(filePathName, "\") + 1)) - extensionsize)

    End Sub

    Friend Sub Open(ByVal templateID As Integer)

        Dim log = New bc_cs_activity_log("bc_am_template", "Open", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim template As bc_ao_at_object

            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Opening...", 10, False, True)
            Cursor.Current = Cursors.WaitCursor

            'determine file type
            Select Case fileExt
                Case WORD_EXT
                    template = New bc_ao_word
                Case EXCEL_EXT
                    template = New bc_ao_excel
                Case POWERPOINT_EXT
                    template = New bc_ao_powerpoint
                Case WORD2007_EXT, WORD2007_MACRO_ENABLED_EXT
                    template = New bc_ao_word
                Case EXCEL2007_EXT, EXCEL2007_MACRO_ENABLED_EXT
                    template = New bc_ao_excel
                Case POWERPOINT2007_EXT, POWERPOINT2007_MACRO_ENABLED_EXT
                    template = New bc_ao_powerpoint
                Case Else
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "File Type: " + fileExt + " not supported.", bc_cs_message.MESSAGE)
                    Exit Try
            End Select

            bc_cs_central_settings.progress_bar.increment("Opening...")

            ' open the template
            If Not template.open_template(String.Concat(bc_cs_central_settings.local_template_path, fileName, fileExt), False) Then
                Exit Try
            End If

            bc_cs_central_settings.progress_bar.increment("Opening...")

            ' update the document properties
            template.update_properties(templateID, String.Concat(fileName, fileExt))

            bc_cs_central_settings.progress_bar.increment("Opening...")

            template.visible()

            template.activate()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "Open", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_template", "Open", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Function Register(ByVal createNew As Boolean, ByRef name As String, ByVal directory As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_template", "Register", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim template As bc_ao_at_object = Nothing

            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Registering...", 10, False, True)
            Cursor.Current = Cursors.WaitCursor

            'determine file type
            Select Case fileExt
                Case WORD_EXT
                    template = New bc_ao_word
                Case EXCEL_EXT
                    template = New bc_ao_excel
                Case POWERPOINT_EXT
                    template = New bc_ao_powerpoint
                Case WORD2007_EXT, WORD2007_MACRO_ENABLED_EXT
                    template = New bc_ao_word
                Case EXCEL2007_EXT, EXCEL2007_MACRO_ENABLED_EXT
                    template = New bc_ao_excel
                Case POWERPOINT2007_EXT, POWERPOINT2007_MACRO_ENABLED_EXT
                    template = New bc_ao_powerpoint
                Case Else
                    ' raise error file type not supported
                    Err.Raise(bc_cs_error_codes.RETURN_ERROR, , "File Type: " + fileExt + " not supported.")
            End Select

            ' register in db
            Dim newIndex As Integer
            newIndex = bc_am_load_objects.obc_templates.add(fileName, String.Concat(fileName, fileExt))

            bc_cs_central_settings.progress_bar.increment("Registering...")

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                ' write template definition via soap
                log = New bc_cs_activity_log("bc_am_template", "Register", bc_cs_activity_codes.COMMENTARY, "Registering template via SOAP")
                bc_am_load_objects.obc_templates.template(newIndex).tmode = bc_am_load_objects.obc_templates.template(newIndex).tWRITE
                bc_am_load_objects.obc_templates.template(newIndex).transmit_to_server_and_receive(bc_am_load_objects.obc_templates.template(newIndex), True)
            Else
                ' write template definition via ado
                log = New bc_cs_activity_log("bc_am_template", "Register", bc_cs_activity_codes.COMMENTARY, "Registering template via ADO")
                bc_am_load_objects.obc_templates.template(newIndex).db_write()
            End If

            bc_cs_central_settings.progress_bar.increment("Registering...")

            If bc_am_load_objects.obc_templates.template(newIndex).id = -1 Then
                bc_am_load_objects.obc_templates.Remove(newIndex)
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Registering Template Failed!", bc_cs_message.MESSAGE)
                Exit Try
            End If

            If bc_am_load_objects.obc_templates.template(newIndex).id = 0 Then
                bc_am_load_objects.obc_templates.Remove(newIndex)
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Template already Registered", bc_cs_message.MESSAGE)
                Exit Try
            End If

            bc_cs_central_settings.progress_bar.increment("Registering...")


            ' update template id
            Dim tmp As bc_om_template
            tmp = bc_am_load_objects.obc_templates.template(newIndex)
            bc_am_load_objects.obc_templates.Remove(newIndex)
            newIndex = bc_am_load_objects.obc_templates.add(tmp.name, tmp.filename, tmp.id)

            'create new template 
            If createNew Then
                template.new_template()
            Else

                If Not template.open_template(String.Concat(directory, fileName, fileExt), False) Then
                    Exit Try
                End If
            End If

            'Sam 2.1.8 Register Chart
            ' If fileExt = EXCEL_EXT Or EXCEL2007_EXT Or EXCEL2007_MACRO_ENABLED_EXT Then
            If template.test_for_code() = True Then
                tmp.delete = True
                tmp.db_write()
                bc_am_load_objects.obc_templates.Remove(newIndex)
                Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "VBA code already exists in the workbook!  Please temporarily copy and remove this code before registering.", bc_cs_message.MESSAGE)
                template.close()
                Exit Try
            End If
            ' End If

            bc_cs_central_settings.progress_bar.increment("Registering...")

            ' add references
            template.add_references()

            ' add custom properties
            template.add_properties(tmp.id, String.Concat(fileName, fileExt))

            bc_cs_central_settings.progress_bar.increment("Registering...")

            'MsgBox(bc_cs_central_settings.local_template_path + String.Concat(fileName, fileExt))

            'save template and close word if no documents open
            template.save_template(String.Concat(fileName, fileExt))

            template.close()
            If template.document_count = 0 Then
                template.quit()
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                ' update physical template definition via soap
                bc_am_load_objects.obc_templates.template(newIndex).WriteDocumentToByteStream()
                bc_am_load_objects.obc_templates.template(newIndex).UpdatePhysicalFileOnly = True
                log = New bc_cs_activity_log("bc_am_template", "Register", bc_cs_activity_codes.COMMENTARY, "Updating physical template via SOAP")
                bc_am_load_objects.obc_templates.template(newIndex).tmode = bc_am_load_objects.obc_templates.template(newIndex).tWRITE
                bc_am_load_objects.obc_templates.template(newIndex).transmit_to_server_and_receive(bc_am_load_objects.obc_templates.template(newIndex), True)
                bc_am_load_objects.obc_templates.template(newIndex).UpdatePhysicalFileOnly = False
            End If

            bc_cs_central_settings.progress_bar.increment("Registering...")

            name = fileName

            Register = True

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "Register", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_template", "Register", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Sub Delete(ByVal index As Integer)

        Dim log = New bc_cs_activity_log("bc_am_template", "Delete", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim template As bc_ao_at_object

            'determine file type
            Select Case fileExt
                Case WORD_EXT
                    template = New bc_ao_word
                Case EXCEL_EXT
                    template = New bc_ao_excel
                Case POWERPOINT_EXT
                    template = New bc_ao_powerpoint
                Case WORD2007_EXT, WORD2007_MACRO_ENABLED_EXT
                    template = New bc_ao_word
                Case EXCEL2007_EXT, EXCEL2007_MACRO_ENABLED_EXT
                    template = New bc_ao_excel
                Case POWERPOINT2007_EXT, POWERPOINT2007_MACRO_ENABLED_EXT
                    template = New bc_ao_powerpoint
                Case Else
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "File Type: " + fileExt + " not supported.", bc_cs_message.MESSAGE)
                    Exit Try
            End Select

            ' mark template for deletion
            bc_am_load_objects.obc_templates.template(index).delete = True

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                ' write template definition via soap
                log = New bc_cs_activity_log("bc_am_template", "CreateNew", bc_cs_activity_codes.COMMENTARY, "Deleting template via SOAP")
                bc_am_load_objects.obc_templates.template(index).tmode = bc_am_load_objects.obc_templates.template(index).tWRITE
                bc_am_load_objects.obc_templates.template(index).transmit_to_server_and_receive(bc_am_load_objects.obc_templates.template(index), True)
            Else
                ' write template definition via ado
                log = New bc_cs_activity_log("bc_am_template", "CreateNew", bc_cs_activity_codes.COMMENTARY, "Deleting template via ADO")
                bc_am_load_objects.obc_templates.template(index).db_write()
            End If

            ' remove from memory
            bc_am_load_objects.obc_templates.Remove(index)


        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "Delete", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_template", "Delete", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Function Clone(ByVal templateID As Integer, ByRef name As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_template", "Clone", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            viewClone.uxOK.Enabled = False

            If viewClone.ShowDialog() = DialogResult.OK Then

                'check template does not already exist
                If templateExists(viewClone.uxTemplateName.Text) Then
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Template already exists.", bc_cs_message.MESSAGE)
                    Exit Try
                End If

                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Cloning...", 10, False, True)
                Cursor.Current = Cursors.WaitCursor

                Dim template As bc_ao_at_object = Nothing

                'determine file type
                Select Case fileExt
                    Case WORD_EXT
                        template = New bc_ao_word
                    Case EXCEL_EXT
                        template = New bc_ao_excel
                    Case POWERPOINT_EXT
                        template = New bc_ao_powerpoint
                    Case WORD2007_EXT, WORD2007_MACRO_ENABLED_EXT
                        template = New bc_ao_word
                    Case EXCEL2007_EXT, EXCEL2007_MACRO_ENABLED_EXT
                        template = New bc_ao_excel
                    Case POWERPOINT2007_EXT, POWERPOINT2007_MACRO_ENABLED_EXT
                        template = New bc_ao_powerpoint
                    Case Else
                        ' raise error file type not supported
                        Err.Raise(bc_cs_error_codes.RETURN_ERROR, , "File Type: " + fileExt + " not supported.")
                End Select

                bc_cs_central_settings.progress_bar.increment("Cloning...")

                ' register in DB
                Dim sql As New StringBuilder
                With sql
                    .Append("exec dbo.bcc_core_bp_clone_template ")
                    .Append(templateID)
                    .Append(", '")
                    .Append(viewClone.uxTemplateName.Text)
                    .Append("', '")
                    If viewClone.uxCreatePhysicalFile.Checked Then
                        .Append(String.Concat(viewClone.uxTemplateName.Text, fileExt))
                    Else
                        .Append(String.Concat(fileName, fileExt))
                    End If
                    .Append("'")
                End With
                Dim osql As New bc_om_sql(sql.ToString)
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                bc_cs_central_settings.progress_bar.increment("Cloning...")

                If osql.success = True Then
                    If viewClone.uxCreatePhysicalFile.Checked Then
                        ' copy template
                        template.copy_template(String.Concat(bc_cs_central_settings.local_template_path, fileName, fileExt), _
                                                String.Concat(bc_cs_central_settings.local_template_path, viewClone.uxTemplateName.Text, fileExt))

                        bc_cs_central_settings.progress_bar.increment("Cloning...")

                        Dim newIndex As Integer

                        newIndex = bc_am_load_objects.obc_templates.add(viewClone.uxTemplateName.Text, String.Concat(viewClone.uxTemplateName.Text, fileExt), osql.results(0, 0))

                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                            ' update physical template definition via soap
                            bc_am_load_objects.obc_templates.template(newIndex).WriteDocumentToByteStream()
                            bc_am_load_objects.obc_templates.template(newIndex).UpdatePhysicalFileOnly = True
                            log = New bc_cs_activity_log("bc_am_template", "Clone", bc_cs_activity_codes.COMMENTARY, "Updating physical template via SOAP")
                            bc_am_load_objects.obc_templates.template(newIndex).tmode = bc_am_load_objects.obc_templates.template(newIndex).tWRITE
                            bc_am_load_objects.obc_templates.template(newIndex).transmit_to_server_and_receive(bc_am_load_objects.obc_templates.template(newIndex), True)
                            bc_am_load_objects.obc_templates.template(newIndex).UpdatePhysicalFileOnly = False
                        End If

                    End If

                    ' refresh templates object
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        Dim ocommentary = New bc_cs_activity_log("bc_am_composite", "Clone", bc_cs_activity_codes.COMMENTARY, "Loading Templates via SOAP")
                        bc_am_load_objects.obc_templates.tmode = bc_cs_soap_base_class.tREAD
                        bc_am_load_objects.obc_templates.transmit_to_server_and_receive(bc_am_load_objects.obc_templates, True)
                    Else
                        Dim ocommentary = New bc_cs_activity_log("bc_am_composite", "Clone", bc_cs_activity_codes.COMMENTARY, "Loading Templates from Database")
                        bc_am_load_objects.obc_templates.db_read()
                    End If

                    bc_cs_central_settings.progress_bar.increment("Cloning...")

                    'open template if selected
                    If viewClone.uxOpenNow.Checked Then
                        If viewClone.uxCreatePhysicalFile.Checked Then
                            template.open_template(String.Concat(bc_cs_central_settings.local_template_path, viewClone.uxTemplateName.Text, fileExt), True)
                            template.update_properties(osql.results(0, 0), String.Concat(viewClone.uxTemplateName.Text, fileExt))
                        Else
                            template.open_template(String.Concat(bc_cs_central_settings.local_template_path, fileName, fileExt), True)
                            template.update_properties(osql.results(0, 0), String.Concat(bc_cs_central_settings.local_template_path, fileName, fileExt))
                        End If
                    Else
                        If viewClone.uxCreatePhysicalFile.Checked Then
                            If template.document_count = 0 Then
                                template.quit()
                            End If
                        End If
                    End If

                    name = viewClone.uxTemplateName.Text
                    Clone = True
                Else
                    Clone = False
                    Dim errLog As New bc_cs_error_log("bc_am_template", "Clone", bc_cs_error_codes.USER_DEFINED, "Failed to clone template")
                    Exit Try
                End If

                bc_cs_central_settings.progress_bar.increment("Cloning...")

            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "Clone", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_template", "Clone", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Function Validate() As Boolean

        Dim log = New bc_cs_activity_log("bc_am_template", "Validate", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Validate = True

            If Trim(viewClone.uxTemplateName.Text) = "" Then
                Validate = False
            End If

            If Validate = True Then
                viewClone.uxOK.Enabled = True
            Else
                viewClone.uxOK.Enabled = False
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "Validate", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_template", "Validate", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    'Sam 2.1.10 Export/Import Templates
    Friend Function Export(ByVal index As Integer, ByVal directory As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_template", "Export", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim osql As New bc_om_sql(String.Concat("bcc_core_bp_export ", bc_am_load_objects.obc_templates.template(index).id))

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success Then
                Dim sw As StreamWriter
                sw = File.CreateText(String.Concat(directory, fileName, fileExt))
                sw.Write(osql.results(0, 0))
                sw.Close()
                Export = True
            Else
                Dim errLog As New bc_cs_error_log("bc_am_template", "Export", bc_cs_error_codes.USER_DEFINED, "Export Failed")
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "Export", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_template", "Export", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    'Sam 2.1.10 Export/Import Templates
    Friend Function Import(ByRef templateName As String, ByVal directory As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_template", "Import", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim xmlDoc As New XmlDocument

            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Importing...", 10, False, True)
            Cursor.Current = Cursors.WaitCursor

            xmlDoc.Load(String.Concat(directory, fileName, fileExt))

            Dim osql As New bc_om_sql(String.Concat("bcc_core_bp_import 0, ", "'", Replace(xmlDoc.OuterXml, "'", "''"), "'"))

            bc_cs_central_settings.progress_bar.increment("Importing...")

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            bc_cs_central_settings.progress_bar.increment("Importing...")

            If osql.success Then
                If osql.results(0, 0) = -1 Then ' template already exists
                    If MessageBox.Show("Template already exists.  Do you wish to overwrite?", "Import", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then
                        osql = New bc_om_sql(String.Concat("bcc_core_bp_import 1, ", "'", Replace(xmlDoc.OuterXml, "'", "''"), "'"))

                        bc_cs_central_settings.progress_bar.increment("Importing...")

                        ' import again specifying overwrite

                        If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                            osql.tmode = bc_cs_soap_base_class.tREAD
                            osql.transmit_to_server_and_receive(osql, True)
                        ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                            osql.db_read()
                        End If
                        If Not osql.success Then
                            Dim errLog As New bc_cs_error_log("bc_am_template", "Import", bc_cs_error_codes.USER_DEFINED, "Import Failed")
                            Exit Try
                        End If
                    Else
                        Exit Try
                    End If
                End If

                If osql.results(0, 0) = -2 Then ' invalid format import file
                    Dim omessage As New bc_cs_message("BluePrint - Blue Curve", "Import file is an invalid format.", bc_cs_message.MESSAGE)
                    Exit Try
                End If

                ' refresh templates object
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    Dim ocommentary = New bc_cs_activity_log("bc_am_composite", "Import", bc_cs_activity_codes.COMMENTARY, "Loading Templates via SOAP")
                    bc_am_load_objects.obc_templates.tmode = bc_cs_soap_base_class.tREAD
                    bc_am_load_objects.obc_templates.transmit_to_server_and_receive(bc_am_load_objects.obc_templates, True)
                Else
                    Dim ocommentary = New bc_cs_activity_log("bc_am_composite", "Import", bc_cs_activity_codes.COMMENTARY, "Loading Templates from Database")
                    bc_am_load_objects.obc_templates.db_read()
                End If
                bc_cs_central_settings.progress_bar.increment("Importing...")
                templateName = osql.results(1, 0)
                Import = True
            Else
                Dim errLog As New bc_cs_error_log("bc_am_template", "Import", bc_cs_error_codes.USER_DEFINED, "Import Failed")
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "Import", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_template", "Import", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Public Sub ShowAllMarkup(ByRef ao_object As Object)

        Dim log = New bc_cs_activity_log("bc_am_template", "ShowAllMarkup", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim template As bc_ao_at_object = Nothing

            'determine file type
            Select Case fileExt
                Case WORD_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case WORD2007_EXT, WORD2007_MACRO_ENABLED_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT2007_EXT, POWERPOINT2007_MACRO_ENABLED_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case Else
                    ' raise error file type not supported
                    Err.Raise(bc_cs_error_codes.RETURN_ERROR, , "File Type: " + fileExt + " not supported.")
            End Select

            template.show_all_markup()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "ShowAllMarkup", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_template", "ShowAllMarkup", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Function PackageForUse(ByRef ao_object As Object) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_template", "PackageForUse", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim template As bc_ao_at_object = Nothing

            'determine file type
            Select Case fileExt
                Case WORD_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case WORD2007_EXT, WORD2007_MACRO_ENABLED_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT2007_EXT, POWERPOINT2007_MACRO_ENABLED_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case Else
                    ' raise error file type not supported
                    Err.Raise(bc_cs_error_codes.RETURN_ERROR, , "File Type: " + fileExt + " not supported.")
            End Select

            template.package_for_use()

            PackageForUse = True

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "PackageForUse", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_template", "PackageForUse", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function


    Public Function Advanced(ByRef ao_object As Object) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_template", "Advanced", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim template As bc_ao_at_object = Nothing

        Try

            Dim templateID As String

            'determine file type
            Select Case fileExt
                Case WORD_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case WORD2007_EXT, WORD2007_MACRO_ENABLED_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT2007_EXT, POWERPOINT2007_MACRO_ENABLED_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case Else
                    ' raise error file type not supported
                    Err.Raise(bc_cs_error_codes.RETURN_ERROR, , "File Type: " + fileExt + " not supported.")
            End Select

            Dim advancedComponents As New bc_am_bp_advanced
            Dim advancedComponentsMgmt As New bc_am_blueprint(advancedComponents)

            'populate uxDbDefinitionList with db definition data
            advancedComponents.uxDbDefinitionList.Items.Clear()

            For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                For j = 0 To bc_am_load_objects.obc_templates.template(i).components.component.count - 1
                    For k = 0 To bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component.count - 1

                        Dim lvi = New ListViewItem()
                        lvi.Text = bc_am_load_objects.obc_templates.template(i).components.component(j).bookmark_name.ToString
                        lvi.Tag = bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).subcompid 'SK added.  Modified bc_om_at_template

                        lvi.SubItems.Add(bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).value.ToString())
                        lvi.SubItems.Add(bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).row)
                        lvi.SubItems.Add(bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).col)
                        'lvi.SubItems.Add(bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).type)
                        lvi.SubItems.Add(bc_am_load_objects.obc_templates.template(i).components.component(j).sub_components.sub_component(k).style)

                        advancedComponents.uxDbDefinitionList.Items.Add(lvi)

                    Next
                Next
            Next

            If advancedComponents.uxDbDefinitionList.Items.Count > 0 Then
                advancedComponents.uxDbDefinitionList.Items(0).Selected = True
            End If

            templateID = template.get_template_id

            'populate uxTempDefinitionList with template definition data
            template.get_all_bookmarks(advancedComponents.uxTempDefinitionList)

            'Pass the templateID to the template listview tag
            advancedComponents.uxTempDefinitionList.Tag = templateID

            advancedComponents.uxTempDefinitionList.Sorting = SortOrder.Ascending
            advancedComponents.uxDbDefinitionList.Sorting = SortOrder.Ascending

            FindDuplicate(advancedComponents.uxDbDefinitionList)

            FindMismatch(advancedComponents.uxTempDefinitionList, advancedComponents.uxDbDefinitionList)

            FindMismatch(advancedComponents.uxDbDefinitionList, advancedComponents.uxTempDefinitionList)

            advancedComponents.Show()

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "Advanced", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_template", "Advanced", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Public Sub FindMismatch(ByRef lv1 As ListView, ByRef lv2 As ListView)

        Dim CheckI As String = Nothing
        Dim CheckJ As String = Nothing

        For i = 0 To lv1.Items.Count - 1

            Dim found As Boolean = False

            For j = 0 To lv2.Items.Count - 1
                'check whether the item exists in the other listview
                If lv1.Items(i).Text = lv2.Items(j).Text And lv1.Items(i).SubItems(1).Text = lv2.Items(j).SubItems(1).Text And _
                lv1.Items(i).SubItems(2).Text = lv2.Items(j).SubItems(2).Text And lv1.Items(i).SubItems(3).Text = lv2.Items(j).SubItems(3).Text Then
                    found = True
                    Exit For
                End If
            Next
            'if not found then highlight red and add tooltip
            If found = False Then
                lv1.Items(i).BackColor = Drawing.Color.Red
                lv1.Items(i).ToolTipText = "Value missing on the other listview."
            End If

        Next

    End Sub

    Public Sub FindDuplicate(ByRef lv As ListView)

        Dim itemI As ListViewItem
        Dim itemJ As ListViewItem

        For i = 0 To lv.Items.Count - 1

            itemI = lv.Items(i)

            For j = i + 1 To lv.Items.Count - 1

                itemJ = lv.Items(j)
                'check whether there is a duplicate
                If itemI.Text = itemJ.Text And itemI.SubItems(1).Text = itemJ.SubItems(1).Text And itemI.SubItems(2).Text = itemJ.SubItems(2).Text _
                And itemI.SubItems(3).Text = itemJ.SubItems(3).Text Then
                    'if duplicate found then highlight red and add tooltip
                    itemJ.BackColor = Drawing.Color.Red
                    itemJ.ToolTipText = "Duplicate value in listview."

                End If

            Next

        Next

    End Sub




    Public Sub DeleteSubComponent(ByRef ao_object As Object)

        Dim log = New bc_cs_activity_log("bc_am_template", "DeleteSubComponent", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim template As bc_ao_at_object = Nothing
            Dim bookmarkName As String
            Dim templateID As String

            'determine file type
            Select Case fileExt
                Case WORD_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case WORD2007_EXT, WORD2007_MACRO_ENABLED_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT2007_EXT, POWERPOINT2007_MACRO_ENABLED_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case Else
                    ' raise error file type not supported
                    Err.Raise(bc_cs_error_codes.RETURN_ERROR, , "File Type: " + fileExt + " not supported.")
            End Select

            Dim x As Integer
            Dim y As Integer
            Dim component As String

            component = template.get_selection_values(x, y)

            If component = "" Then ' cursor is not in the correct position
                Dim omessage As New bc_cs_message("Delete Component", "There is no component located at the current cursor location", bc_cs_message.MESSAGE)
                Exit Try
            End If

            bookmarkName = template.get_locator_for_display(False)
            If Right(bookmarkName, 5) = "_r1c1" Then
                bookmarkName = Left(bookmarkName, Len(bookmarkName) - 5)
            End If

            If Trim(bookmarkName) = "" Then
                Dim omessage As New bc_cs_message("Delete Component", "There is no component located at the current cursor location", bc_cs_message.MESSAGE)
                Exit Try
            End If

            If MessageBox.Show("Are you sure you wish to remove this component?", "Template Component", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                templateID = template.get_template_id

                If Not IsNumeric(templateID) Then
                    Dim errLog As New bc_cs_error_log("bc_am_template", "DeleteSubComponent", bc_cs_error_codes.USER_DEFINED, "Template ID not set.")
                    Exit Try
                End If

                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Deleting Sub Component...", 10, False, True)
                Cursor.Current = Cursors.WaitCursor

                Dim sql As New StringBuilder
                With sql
                    .Append("exec bcc_core_bp_delete_sub_component ")
                    .Append(templateID)
                    .Append(", '")
                    .Append(bookmarkName)
                    .Append("',")
                    .Append(x)
                    .Append(",")
                    .Append(y)
                End With
                Dim osql As New bc_om_sql(sql.ToString)

                bc_cs_central_settings.progress_bar.increment("Deleting Sub Component...")

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                bc_cs_central_settings.progress_bar.increment("Deleting Sub Component...")

                If osql.success Then
                    bc_cs_central_settings.progress_bar.increment("Deleting Sub Component...")
                    template.clear_table_cell(bookmarkName)
                    bc_cs_central_settings.progress_bar.increment("Deleting Sub Component...")
                    refreshTemplateMarkup(templateID, template.get_name)
                Else
                    Dim errLog As New bc_cs_error_log("bc_am_template", "DeleteSubComponent", bc_cs_error_codes.USER_DEFINED, "Component failed to be removed")
                    Exit Try
                End If
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "DeleteSubComponent", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_template", "DeleteSubComponent", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Sub AddUpdateSubComponent(ByRef ao_object As Object)

        Dim log = New bc_cs_activity_log("bc_am_template", "AddUpdateSubComponent", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim template As bc_ao_at_object = Nothing

        Try

            Dim bookmarkName As String
            Dim templateID As String

            'determine file type
            Select Case fileExt
                Case WORD_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case WORD2007_EXT, WORD2007_MACRO_ENABLED_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT2007_EXT, POWERPOINT2007_MACRO_ENABLED_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case Else
                    ' raise error file type not supported
                    Err.Raise(bc_cs_error_codes.RETURN_ERROR, , "File Type: " + fileExt + " not supported.")
            End Select

            Dim x As Integer
            Dim y As Integer

            template.get_selection_values(x, y)

            If x = -1 Then ' cursor is not in the correct position
                Dim omessage As New bc_cs_message("Add / Update Component", "There is no registered table located at the current cursor location", bc_cs_message.MESSAGE)
                Exit Try
            End If

            Dim templateComponents As New bc_am_bp_template_components
            Dim templateComponentsMgmt As New bc_am_blueprint(templateComponents)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                REM rem read in via SOAP                
                Dim ocommentary = New bc_cs_activity_log("bc_ao_word", "AddUpdateSubComponent", bc_cs_activity_codes.COMMENTARY, "Loading Components via SOAP")
                bc_am_load_objects.obc_templates.component_types.tmode = bc_am_load_objects.obc_templates.template(0).tREAD
                bc_am_load_objects.obc_templates.component_types.transmit_to_server_and_receive(bc_am_load_objects.obc_templates.component_types, True)
            Else
                REM read in directly from database                
                REM templates
                Dim ocommentary = New bc_cs_activity_log("bc_ao_word", "AddUpdateSubComponent", bc_cs_activity_codes.COMMENTARY, "Loading Components from Database")
                bc_am_load_objects.obc_templates.component_types.db_read()
            End If

            templateComponentsMgmt.SetRoleUserDB(templateComponents)
            templateComponentsMgmt.LoadComponents(templateComponents)

            If templateComponents.uxGenericList.Items.Count > 0 Then
                templateComponents.uxGenericList.Items(0).Selected = True
            End If

            templateID = template.get_template_id
            bookmarkName = template.get_locator_for_display(False)
            If Right(bookmarkName, 5) = "_r1c1" Then
                bookmarkName = Left(bookmarkName, Len(bookmarkName) - 5)
            End If

            Dim sql As New StringBuilder
            With sql
                .Append("exec bcc_core_bp_get_sub_component ")
                .Append(templateID)
                .Append(", '")
                .Append(bookmarkName)
                .Append("', ")
                .Append(x)
                .Append(",")
                .Append(y)
            End With
            Dim osql As New bc_om_sql(sql.ToString)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success Then

                If osql.results Is Nothing Then
                    Dim omessage As New bc_cs_message("Available Components", "There is no registered table located at the current cursor location", bc_cs_message.MESSAGE)
                    Exit Try
                End If

                Dim lvi As ListViewItem

                For Each lvi In templateComponents.uxGenericList.Items
                    If bc_am_load_objects.obc_templates.component_types.component_types(lvi.Tag).component_id = osql.results(2, 0) Then
                        lvi.Selected = True
                        Exit For
                    End If
                Next

                ' if no item in the list is selected then select the first item
                If templateComponents.uxGenericList.Items.Count > 0 And templateComponents.uxGenericList.SelectedItems.Count = 0 Then
                    templateComponents.uxGenericList.Items(0).Selected = True
                End If

                ' if the component type is text then enable the text entry
                If osql.results(2, 0) = 2 Then
                    templateComponents.uxTextEntry.Enabled = True
                    templateComponents.uxTextEntry.Text = osql.results(1, 0)
                Else
                    templateComponents.uxTextEntry.Enabled = False
                    templateComponents.uxTextEntry.Text = ""
                End If
            Else
                Dim omessage As New bc_cs_message("Available Components", "The components list failed to load.", bc_cs_message.MESSAGE)
                Exit Try
            End If

            'get the current style
            templateComponents.uxSelectedStyle.Text = template.get_selection_style

            If templateComponents.ShowDialog() = DialogResult.OK Then

                ' if standard display then always put in the first cell
                If bc_am_load_objects.obc_templates.component_types.component_types(templateComponents.uxGenericList.SelectedItems(0).Tag).mode = 2 Then
                    If x <> 1 Or y <> 1 Then
                        Dim omessage As New bc_cs_message("Add / Update Component", "The selected component must be placed in the first cell within the table. Please reposition the cursor.", bc_cs_message.MESSAGE)
                        Exit Try
                    End If
                End If

                sql = New StringBuilder
                With sql
                    .Append("exec bcc_core_bp_insert_sub_component ")
                    .Append(templateID)
                    .Append(",")
                    .Append(bc_am_load_objects.obc_templates.component_types.component_types(templateComponents.uxGenericList.SelectedItems(0).Tag).component_id)
                    .Append(", '")
                    .Append(templateComponents.uxTextEntry.Text.Replace("'", "''"))
                    .Append("','")
                    .Append(bookmarkName)
                    .Append("','")
                    .Append(CType(template.get_selection_style, String))
                    .Append("',")
                    .Append(x)
                    .Append(",")
                    .Append(y)
                End With
                osql = New bc_om_sql(sql.ToString)

                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Adding / updating component...", 10, False, True)
                Cursor.Current = Cursors.WaitCursor

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                bc_cs_central_settings.progress_bar.increment("Adding / updating component...")

                If osql.success Then

                    bc_cs_central_settings.progress_bar.increment("Adding / updating component...")

                    If bc_am_load_objects.obc_templates.component_types.component_types(templateComponents.uxGenericList.SelectedItems(0).Tag).component_id = 2 Then
                        template.set_table_cell(templateComponents.uxTextEntry.Text, False)
                    Else
                        If bc_am_load_objects.obc_templates.component_types.component_types(templateComponents.uxGenericList.SelectedItems(0).Tag).mode = 2 Then
                            template.set_table_cell(templateComponents.uxGenericList.SelectedItems(0).Text, True)
                        Else
                            template.set_table_cell(templateComponents.uxGenericList.SelectedItems(0).Text, False)
                        End If
                    End If

                    If fileExt = WORD_EXT Or fileExt = WORD2007_EXT Or fileExt = WORD2007_MACRO_ENABLED_EXT Then
                        If bc_am_load_objects.obc_templates.component_types.component_types(templateComponents.uxGenericList.SelectedItems(0).Tag).mode = 2 Then ' standard display
                            ' add to BC table gallery
                            template.copy_table()
                            If template.insert_table_into_gallery(bookmarkName, False) = False Then
                                Dim omessage As New bc_cs_message("Available Components", "Failed to add to table gallery.", bc_cs_message.MESSAGE)
                            End If
                        End If
                        If bc_am_load_objects.obc_templates.component_types.component_types(templateComponents.uxGenericList.SelectedItems(0).Tag).mode = 2 Then
                            template.set_table_cell("", True) ' blank out the text in the table gallery
                        End If
                    End If

                    bc_cs_central_settings.progress_bar.increment("Adding / updating component...")

                    refreshTemplateMarkup(templateID, template.get_name)

                    bc_cs_central_settings.progress_bar.increment("Adding / updating component...")
                Else
                    Dim omessage As New bc_cs_message("Available Components", "The component failed to add / update.", bc_cs_message.MESSAGE)
                    Exit Try
                End If
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "AddUpdateSubComponent", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            template.close_table_gallery(True)
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_template", "AddUpdateSubComponent", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Sub RegisterNewTable(ByRef ao_object As Object)

        Dim log = New bc_cs_activity_log("bc_am_template", "RegisterNewTable", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim template As bc_ao_at_object = Nothing
            Dim templateID As Integer

            Dim slideIndex As Integer

            'determine file type
            Select Case fileExt
                Case WORD_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case WORD2007_EXT, WORD2007_MACRO_ENABLED_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT2007_EXT, POWERPOINT2007_MACRO_ENABLED_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case Else
                    ' raise error file type not supported
                    Err.Raise(bc_cs_error_codes.RETURN_ERROR, , "File Type: " + fileExt + " not supported.")
            End Select

            Dim x As Integer
            Dim y As Integer

            template.get_selection_values(x, y)

            If x = -1 Then ' cursor is not in the correct position
                Dim omessage As New bc_cs_message("Register Table", "There is no table at the current cursor location", bc_cs_message.MESSAGE)
                Exit Try
            End If

            'Sam 2.2.3 Register new table in word - check is tablename already exists
            If fileExt = POWERPOINT_EXT Or fileExt = POWERPOINT2007_EXT Or fileExt = POWERPOINT2007_MACRO_ENABLED_EXT Or fileExt = WORD_EXT _
            Or fileExt = WORD2007_EXT Or fileExt = WORD2007_MACRO_ENABLED_EXT Then
                If template.TableAlreadyRegistered Then
                    Dim omessage As New bc_cs_message("Register Table", "The selected table is already registered", bc_cs_message.MESSAGE)
                    Exit Try
                End If
            End If

            Dim regTable As New bc_am_bp_register_table
            Dim regTableMgmt As New bc_am_blueprint(regTable)

            regTableMgmt.SetRoleUserDB(regTable)

            regTable.uxOK.Enabled = False

            If regTable.ShowDialog = DialogResult.OK Then

                ' only populated for powerpoint otherwise zero
                slideIndex = 0
                slideIndex = template.getSlideIndex

                templateID = template.get_template_id

                ' check entry does not already exist
                If template.check_autotext_entry_exists(regTable.uxTableName.Text) = True Then
                    Dim omessage As New bc_cs_message("Register Table", "Table name already exists.", bc_cs_message.MESSAGE)
                    Exit Try
                Else
                    bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Registering new table...", 10, False, True)
                    Cursor.Current = Cursors.WaitCursor

                    Dim sql As New StringBuilder
                    With sql
                        .Append("exec bcc_core_bp_register_table ")
                        .Append(templateID)
                        .Append(",'")
                        .Append(regTable.uxTableName.Text)
                        .Append("',")
                        .Append(0) ' do not include sub components to be added
                        .Append(",")
                        .Append(-1)
                        .Append(",")
                        .Append(slideIndex)
                    End With
                    Dim osql As New bc_om_sql(sql.ToString)

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        osql.tmode = bc_cs_soap_base_class.tREAD
                        osql.transmit_to_server_and_receive(osql, True)
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        osql.db_read()
                    End If

                    bc_cs_central_settings.progress_bar.increment("Registering new table...")

                    If osql.success Then
                        template.register_table(regTable.uxTableName.Text, regTable.uxTableName.Text, False)

                        refreshTemplateMarkup(templateID, template.get_name)
                    Else
                        Dim errLog As New bc_cs_error_log("bc_am_template", "RegisterNewTable", bc_cs_error_codes.USER_DEFINED, "Table failed to register.")
                        Exit Try
                    End If
                    bc_cs_central_settings.progress_bar.increment("Registering new table...")
                End If

            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "RegisterNewTable", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_template", "RegisterNewTable", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Sub InsertRegisteredTable(ByRef ao_object As Object)

        Dim log = New bc_cs_activity_log("bc_am_template", "InsertRegisteredTable", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim template As bc_ao_at_object = Nothing
            Dim templateID As Integer

            'determine file type
            Select Case fileExt
                Case WORD_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case WORD2007_EXT, WORD2007_MACRO_ENABLED_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT2007_EXT, POWERPOINT2007_MACRO_ENABLED_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case Else
                    ' raise error file type not supported
                    Err.Raise(bc_cs_error_codes.RETURN_ERROR, , "File Type: " + fileExt + " not supported.")
            End Select

            Dim x As Integer
            Dim y As Integer

            template.get_selection_values(x, y)

            If x <> -1 Then ' cursor is not in the correct position
                Dim omessage As New bc_cs_message("Register Table", "A table cannot be inserted within a table", bc_cs_message.MESSAGE)
                Exit Try
            End If

            Dim regTables As New bc_am_bp_registered_tables
            Dim regTablesMgmt As New bc_am_blueprint(regTables)

            regTablesMgmt.SetRoleUserDB(regTables)

            regTables.uxTableOnly.Checked = True
            regTables.uxAdd.Enabled = False

            Dim osql As New bc_om_sql("exec bcc_core_bp_get_all_registered_tables")

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success Then
                regTables.allRegisteredTables = osql.results
            Else
                Dim errLog As New bc_cs_error_log("bc_am_template", "InsertRegisteredTable", bc_cs_error_codes.USER_DEFINED, "Registered tables failed to load.")
                Exit Try
            End If

            osql = New bc_om_sql("exec bcc_core_bp_get_distinct_registered_tables")

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success Then
                regTables.distinctRegisteredTables = osql.results
            Else
                Dim errLog As New bc_cs_error_log("bc_am_template", "InsertRegisteredTable", bc_cs_error_codes.USER_DEFINED, "Registered tables failed to load.")
                Exit Try
            End If

            regTablesMgmt.ToggleRegisteredTablesView(True)

            templateID = template.get_template_id

            If Not IsNumeric(templateID) Then
                Dim errLog As New bc_cs_error_log("bc_am_template", "DeleteRegisteredTable", bc_cs_error_codes.USER_DEFINED, "Template ID not set.")
                Exit Try
            End If

            If regTables.ShowDialog = DialogResult.OK Then

                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Inserting Registered Table...", 10, False, True)
                Cursor.Current = Cursors.WaitCursor

                Dim sql As New StringBuilder
                With sql
                    .Append("exec bcc_core_bp_register_table ")
                    .Append(templateID)
                    .Append(",'")
                    If regTables.uxTableOnly.Checked Then
                        .Append(regTables.distinctRegisteredTables(0, regTables.uxGenericList.SelectedItems(0).Tag))
                    Else
                        .Append(regTables.allRegisteredTables(0, regTables.uxGenericList.SelectedItems(0).Tag))
                    End If
                    .Append("',")
                    If regTables.uxTableOnly.Checked Then
                        .Append(0)
                    Else
                        .Append(1)
                        .Append(",")
                        .Append(regTables.allRegisteredTables(2, regTables.uxGenericList.SelectedItems(0).Tag))
                    End If
                End With
                osql = New bc_om_sql(sql.ToString)

                bc_cs_central_settings.progress_bar.increment("Inserting Registered Table...")

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                bc_cs_central_settings.progress_bar.increment("Inserting Registered Table...")

                If osql.success Then

                    If regTables.uxTableOnly.Checked Then
                        template.insert_registered_table(regTables.distinctRegisteredTables(0, regTables.uxGenericList.SelectedItems(0).Tag), _
                                                            regTables.distinctRegisteredTables(0, regTables.uxGenericList.SelectedItems(0).Tag))
                    Else
                        template.insert_registered_table(regTables.allRegisteredTables(0, regTables.uxGenericList.SelectedItems(0).Tag), _
                                                            regTables.allRegisteredTables(0, regTables.uxGenericList.SelectedItems(0).Tag))
                    End If

                    bc_cs_central_settings.progress_bar.increment("Inserting Registered Table...")

                    refreshTemplateMarkup(templateID, template.get_name)
                    bc_cs_central_settings.progress_bar.increment("Inserting Registered Table...")
                    If regTables.uxTableAndComponents.Checked Then
                        'loop through sub components setting the cell descriptions
                        template.set_table_cells(regTables.allRegisteredTables(0, regTables.uxGenericList.SelectedItems(0).Tag))
                    End If
                Else
                    Dim errLog As New bc_cs_error_log("bc_am_template", "InsertRegisteredTable", bc_cs_error_codes.USER_DEFINED, "Table failed to register.")
                    Exit Try
                End If

            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "InsertRegisteredTable", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_template", "InsertRegisteredTable", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Sub DeleteRegisteredTable(ByRef ao_object As Object)

        Dim log = New bc_cs_activity_log("bc_am_template", "DeleteRegisteredTable", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim template As bc_ao_at_object = Nothing
            Dim bookmarkName As String
            Dim templateID As String

            'determine file type
            Select Case fileExt
                Case WORD_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case WORD2007_EXT, WORD2007_MACRO_ENABLED_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT2007_EXT, POWERPOINT2007_MACRO_ENABLED_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case Else
                    ' raise error file type not supported
                    Err.Raise(bc_cs_error_codes.RETURN_ERROR, , "File Type: " + fileExt + " not supported.")
            End Select

            Dim x As Integer
            Dim y As Integer

            template.get_selection_values(x, y)

            If x = -1 Then ' cursor is not in the correct position
                Dim omessage As New bc_cs_message("Delete Registered Table", "There is no registered table located at the current cursor location", bc_cs_message.MESSAGE)
                Exit Try
            End If

            bookmarkName = template.get_locator_for_display(False)
            If Right(bookmarkName, 5) = "_r1c1" Then
                bookmarkName = Left(bookmarkName, Len(bookmarkName) - 5)
            End If

            If Trim(bookmarkName) = "" Then
                Dim omessage As New bc_cs_message("Delete Registered Table", "There is no registered table located at the current cursor location", bc_cs_message.MESSAGE)
                Exit Try
            End If

            If MessageBox.Show("Are you sure you wish to remove this registered table?", "Template Component", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes Then

                bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Deleting Registered Table...", 10, False, True)
                Cursor.Current = Cursors.WaitCursor

                templateID = template.get_template_id

                If Not IsNumeric(templateID) Then
                    Dim errLog As New bc_cs_error_log("bc_am_template", "DeleteRegisteredTable", bc_cs_error_codes.USER_DEFINED, "Template ID not set.")
                    Exit Try
                End If

                Dim sql As New StringBuilder
                With sql
                    .Append("exec bcc_core_bp_delete_registered_table ")
                    .Append(templateID)
                    .Append(", '")
                    .Append(bookmarkName)
                    .Append("'")
                End With
                Dim osql As New bc_om_sql(sql.ToString)

                bc_cs_central_settings.progress_bar.increment("Deleting Registered Table...")

                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                bc_cs_central_settings.progress_bar.increment("Deleting Registered Table...")

                If osql.success Then
                    bc_cs_central_settings.progress_bar.increment("Deleting Registered Table...")
                    template.delete_table(CBool(osql.results(0, 0)))
                    bc_cs_central_settings.progress_bar.increment("Deleting Registered Table...")
                    refreshTemplateMarkup(templateID, template.get_name)
                Else
                    Dim errLog As New bc_cs_error_log("bc_am_template", "DeleteRegisteredTable", bc_cs_error_codes.USER_DEFINED, "Registered table failed to be removed")
                End If
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "DeleteRegisteredTable", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_template", "DeleteRegisteredTable", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Function Copy(ByVal newLocation As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_template", "Copy", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim template As bc_ao_at_object

            template = New bc_ao_word

            Cursor.Current = Cursors.WaitCursor

            If newLocation = "" And bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then

                Dim templateObj As New bc_om_template

                templateObj.updatePhysicalFileOnly = True
                templateObj.filename = String.Concat(fileName, fileExt)
                templateObj.WriteDocumentToByteStream()
                templateObj.tmode = bc_cs_soap_base_class.tWRITE
                templateObj.transmit_to_server_and_receive(templateObj, True)
                templateObj.updatePhysicalFileOnly = False

            Else

                'MsgBox(String.Concat(bc_cs_central_settings.local_template_path, fileName + "_", Date.Now.ToString("yyyyMMddHHmmss"), fileExt))
                'SK make copy of template in current folder with timestamp in the file name
                If File.Exists(String.Concat(newLocation, "\", fileName, fileExt)) Then
                    'MsgBox("File found.")

                    My.Computer.FileSystem.RenameFile(String.Concat(newLocation, "\", fileName, fileExt), _
                                        String.Concat(fileName + "_", Date.Now.ToString("yyyyMMddHHmmss"), fileExt))

                    template.copy_template(String.Concat(bc_cs_central_settings.local_template_path, fileName, fileExt), _
                                       String.Concat(newLocation, "\", fileName, fileExt))

                Else
                    'MsgBox("File not found.")
                    template.copy_template(String.Concat(bc_cs_central_settings.local_template_path, fileName, fileExt), _
                                           String.Concat(newLocation, "\", fileName, fileExt))
                End If
            End If

            Copy = True

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "Copy", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_template", "Copy", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Sub refreshTemplateMarkup(ByVal templateID As Integer, ByVal templateName As String)

        Dim log = New bc_cs_activity_log("bc_am_template", "refreshTemplateMarkup", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            ' remove any existing templates

            If bc_am_load_objects.obc_templates.template.Count > 1 Then
                Dim errLog As New bc_cs_error_log("bc_am_template", "refreshTemplateMarkup", bc_cs_error_codes.USER_DEFINED, "System Error. Please contact support.")
                Exit Try
            End If

            If bc_am_load_objects.obc_templates.template.Count = 1 Then
                bc_am_load_objects.obc_templates.template.RemoveAt(0)
            End If
            bc_am_load_objects.obc_templates.add(templateName, templateName, templateID)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                REM rem read in via SOAP                
                Dim ocommentary = New bc_cs_activity_log("bc_ao_word", "refreshTemplateMarkup", bc_cs_activity_codes.COMMENTARY, "Loading Template via SOAP")
                bc_am_load_objects.obc_templates.template(0).tmode = bc_am_load_objects.obc_templates.template(0).tREAD
                bc_am_load_objects.obc_templates.template(0).transmit_to_server_and_receive(bc_am_load_objects.obc_templates.template(0), True)
            Else
                REM read in directly from database                
                REM templates
                Dim ocommentary = New bc_cs_activity_log("bc_ao_word", "refreshTemplateMarkup", bc_cs_activity_codes.COMMENTARY, "Loading Template from Database")
                bc_am_load_objects.obc_templates.template(0).db_read(templateID, bc_am_load_objects.obc_templates.template(0).certificate)
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "refreshTemplateMarkup", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_template", "refreshTemplateMarkup", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Function templateExists(ByVal templateName As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_template", "templateExists", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer

            For i = 0 To bc_am_load_objects.obc_templates.template.Count - 1
                If LCase(bc_am_load_objects.obc_templates.template(i).name) = LCase(templateName) Then
                    templateExists = True
                    Exit For
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "templateExists", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_template", "templateExists", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Function Edit(ByVal index As Integer, ByRef name As String) As Boolean

        Dim log = New bc_cs_activity_log("bc_am_template", "Edit", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            viewEditTemplateDetails.uxOK.Enabled = False

            viewEditTemplateDetails.uxTemplateName.Text = bc_am_load_objects.obc_templates.template(index).name
            viewEditTemplateDetails.uxTemplateFileName.Text = bc_am_load_objects.obc_templates.template(index).filename

            If viewEditTemplateDetails.ShowDialog() = DialogResult.OK And _
                (LCase(viewEditTemplateDetails.uxTemplateName.Text) <> LCase(bc_am_load_objects.obc_templates.template(index).name)) Or _
                (LCase(viewEditTemplateDetails.uxTemplateFileName.Text) <> LCase(bc_am_load_objects.obc_templates.template(index).filename)) Then

                Cursor.Current = Cursors.WaitCursor

                ' save changes
                Dim sql As New StringBuilder
                With sql
                    .Append("exec dbo.bcc_core_bp_update_template ")
                    .Append(bc_am_load_objects.obc_templates.template(index).id)
                    .Append(", '")
                    .Append(viewEditTemplateDetails.uxTemplateName.Text)
                    .Append("', '")
                    .Append(viewEditTemplateDetails.uxTemplateFileName.Text)
                    .Append("'")
                End With
                Dim osql As New bc_om_sql(sql.ToString)
                If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                    osql.tmode = bc_cs_soap_base_class.tREAD
                    osql.transmit_to_server_and_receive(osql, True)
                ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                    osql.db_read()
                End If

                If osql.success = True Then

                    If Not osql.results Is Nothing AndAlso osql.results(0, 0) = -1 Then ' template already exists
                        Dim omessage As New bc_cs_message("Edit Template", "Template already exists", bc_cs_message.MESSAGE)
                        Exit Try
                    End If

                    bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Refreshing Templates", 10, False, True)

                    ' refresh templates object
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        Dim ocommentary = New bc_cs_activity_log("bc_am_composite", "Clone", bc_cs_activity_codes.COMMENTARY, "Loading Templates via SOAP")
                        bc_am_load_objects.obc_templates.tmode = bc_cs_soap_base_class.tREAD
                        bc_am_load_objects.obc_templates.transmit_to_server_and_receive(bc_am_load_objects.obc_templates, True)
                    Else
                        Dim ocommentary = New bc_cs_activity_log("bc_am_composite", "Clone", bc_cs_activity_codes.COMMENTARY, "Loading Templates from Database")
                        bc_am_load_objects.obc_templates.db_read()
                    End If

                    bc_cs_central_settings.progress_bar.increment("Refreshing Templates...")

                    name = viewEditTemplateDetails.uxTemplateName.Text
                    Edit = True
                Else
                    Edit = False
                    Dim errLog As New bc_cs_error_log("bc_am_template", "Clone", bc_cs_error_codes.USER_DEFINED, "Failed to edit template")
                    Exit Try
                End If
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "Edit", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Cursor.Current = Cursors.Default
            bc_cs_central_settings.progress_bar.unload()
            log = New bc_cs_activity_log("bc_am_template", "Edit", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Friend Function ValidateTemplate() As Boolean

        Dim log = New bc_cs_activity_log("bc_am_template", "ValidateTemplate", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            ValidateTemplate = True

            If Trim(viewEditTemplateDetails.uxTemplateName.Text) = "" Then
                ValidateTemplate = False
            End If

            If Trim(viewEditTemplateDetails.uxTemplateFileName.Text) = "" Then
                ValidateTemplate = False
            End If

            If ValidateTemplate = True Then
                viewEditTemplateDetails.uxOK.Enabled = True
            Else
                viewEditTemplateDetails.uxOK.Enabled = False
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "ValidateTemplate", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_template", "ValidateTemplate", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Public Sub UpdateRegisteredTable(ByRef ao_object As Object)

        Dim log = New bc_cs_activity_log("bc_am_template", "UpdateRegisteredTable", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim template As bc_ao_at_object = Nothing
        Dim componentTypeID As Integer
        Dim i As Integer

        Try
            Dim templateID As Integer
            Dim component_id As Integer
            Dim origBookmarkName As String
            Dim origAutotextName As String
            Dim bookmarkName As String

            'determine file type
            Select Case fileExt
                Case WORD_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case WORD2007_EXT, WORD2007_MACRO_ENABLED_EXT
                    template = New bc_ao_word(ao_object)
                Case POWERPOINT2007_EXT, POWERPOINT2007_MACRO_ENABLED_EXT
                    template = New bc_ao_powerpoint(ao_object)
                Case Else
                    ' raise error file type not supported
                    Err.Raise(bc_cs_error_codes.RETURN_ERROR, , "File Type: " + fileExt + " not supported.")
            End Select

            Dim x As Integer
            Dim y As Integer

            template.get_selection_values(x, y)

            If x = -1 Then ' cursor is not in the correct position
                Dim omessage As New bc_cs_message("Update Registered Table", "There is no table at the current cursor location", bc_cs_message.MESSAGE)
                Exit Try
            End If

            Dim regTable As New bc_am_bp_update_registered_table
            Dim regTableMgmt As New bc_am_blueprint(regTable)

            regTableMgmt.SetRoleUserDB(regTable)

            templateID = template.get_template_id
            bookmarkName = template.get_locator_for_display(False)
            If Right(bookmarkName, 5) = "_r1c1" Then
                bookmarkName = Left(bookmarkName, Len(bookmarkName) - 5)
            End If

            Dim sql As New StringBuilder
            With sql
                .Append("exec bcc_core_bp_get_registered_table ")
                .Append(templateID)
                .Append(",'")
                .Append(bookmarkName)
                .Append("'")
            End With
            Dim osql As New bc_om_sql(sql.ToString)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If osql.success Then
                component_id = osql.results(0, 0)
                regTable.uxTableName.Text = osql.results(1, 0)
                origBookmarkName = osql.results(1, 0)
                regTable.uxAutoTextName.Text = osql.results(2, 0)
                origAutotextName = osql.results(2, 0)
            Else
                Dim errLog As New bc_cs_error_log("bc_am_template", "UpdateRegisteredTable", bc_cs_error_codes.USER_DEFINED, "Cannot find registered table.")
                Exit Try
            End If

            sql = New StringBuilder
            With sql
                .Append("exec bcc_core_bp_get_sub_component ")
                .Append(templateID)
                .Append(", '")
                .Append(regTable.uxTableName.Text)
                .Append("', ")
                .Append(1)
                .Append(",")
                .Append(1)
            End With
            osql = New bc_om_sql(sql.ToString)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                osql.tmode = bc_cs_soap_base_class.tREAD
                osql.transmit_to_server_and_receive(osql, True)
            ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                osql.db_read()
            End If

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                REM rem read in via SOAP                
                Dim ocommentary = New bc_cs_activity_log("bc_ao_word", "AddUpdateSubComponent", bc_cs_activity_codes.COMMENTARY, "Loading Components via SOAP")
                bc_am_load_objects.obc_templates.component_types.tmode = bc_am_load_objects.obc_templates.template(0).tREAD
                bc_am_load_objects.obc_templates.component_types.transmit_to_server_and_receive(bc_am_load_objects.obc_templates.component_types, True)
            Else
                REM read in directly from database                
                REM templates
                Dim ocommentary = New bc_cs_activity_log("bc_ao_word", "AddUpdateSubComponent", bc_cs_activity_codes.COMMENTARY, "Loading Components from Database")
                bc_am_load_objects.obc_templates.component_types.db_read()
            End If

            If osql.success Then
                If Not osql.results Is Nothing Then
                    componentTypeID = osql.results(2, 0)
                End If
            Else
                Dim errLog As New bc_cs_error_log("bc_am_template", "UpdateRegisteredTable", bc_cs_error_codes.USER_DEFINED, "Update Registered Table failed.")
                Exit Try
            End If

            regTable.uxOK.Enabled = False

            If fileExt = POWERPOINT_EXT Or fileExt = POWERPOINT2007_EXT Or fileExt = POWERPOINT2007_MACRO_ENABLED_EXT Then
                regTable.uxAutoTxtName.Text = "Table Name"
                regTable.uxTblName.Text = "Shape Name"
            End If

            If regTable.ShowDialog = DialogResult.OK Then

                templateID = template.get_template_id

                If LCase(regTable.uxTableName.Text) <> LCase(origBookmarkName) AndAlso template.check_autotext_entry_exists(regTable.uxTableName.Text) = True Then
                    Dim omessage As New bc_cs_message("Update Registered Table", "Table name already exists.", bc_cs_message.MESSAGE)
                    Exit Try
                End If

                For i = 0 To bc_am_load_objects.obc_templates.component_types.component_types.Count - 1
                    If bc_am_load_objects.obc_templates.component_types.component_types(i).component_id = componentTypeID Then
                        If bc_am_load_objects.obc_templates.component_types.component_types(i).mode = 2 Then ' standard display
                            If regTable.uxTableName.Text = origBookmarkName Then
                                Dim omessage As New bc_cs_message("Update Registered Table", "Table name must be changed when updating table components.", bc_cs_message.MESSAGE)
                                Exit Try
                            End If
                        End If
                        Exit For
                    End If
                Next

                ' check entry does not already exist
                If LCase(regTable.uxAutoTextName.Text) <> LCase(origAutotextName) AndAlso template.check_autotext_entry_exists(regTable.uxAutoTextName.Text) = True Then
                    Dim omessage As New bc_cs_message("Update Registered Table", "Autotext name already exists.", bc_cs_message.MESSAGE)
                    Exit Try
                Else
                    bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Updating Registered new table...", 10, False, True)
                    Cursor.Current = Cursors.WaitCursor

                    sql = New StringBuilder
                    With sql
                        .Append("exec bcc_core_bp_update_registered_table ")
                        .Append(templateID)
                        .Append(",")
                        .Append(component_id)
                        .Append(",'")
                        .Append(origBookmarkName)
                        .Append("','")
                        .Append(regTable.uxTableName.Text)
                        .Append("','")
                        .Append(regTable.uxAutoTextName.Text)
                        .Append("'")
                    End With
                    osql = New bc_om_sql(sql.ToString)

                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.SOAP Then
                        osql.tmode = bc_cs_soap_base_class.tREAD
                        osql.transmit_to_server_and_receive(osql, True)
                    ElseIf bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        osql.db_read()
                    End If

                    bc_cs_central_settings.progress_bar.increment("Updating registered new table...")

                    If osql.success Then
                        template.register_table(regTable.uxTableName.Text, regTable.uxAutoTextName.Text, osql.results(0, 0))

                        bc_cs_central_settings.progress_bar.increment("Updating registered new table...")

                        If fileExt = WORD_EXT Or fileExt = WORD2007_EXT Or fileExt = WORD2007_MACRO_ENABLED_EXT Then
                            If componentTypeID > 0 Then
                                For i = 0 To bc_am_load_objects.obc_templates.component_types.component_types.Count - 1
                                    If bc_am_load_objects.obc_templates.component_types.component_types(i).component_id = componentTypeID Then
                                        If bc_am_load_objects.obc_templates.component_types.component_types(i).mode = 2 Then ' standard display                                        
                                            ' add to BC table gallery
                                            template.copy_table()
                                            If template.insert_table_into_gallery(regTable.uxTableName.Text, False) = False Then
                                                Dim omessage As New bc_cs_message("Update registered table", "Failed to add to table gallery.", bc_cs_message.MESSAGE)
                                            End If
                                            template.set_table_cell("", True) ' blank out the text in the table gallery
                                        End If
                                        Exit For
                                    End If
                                Next
                            End If
                        End If

                        refreshTemplateMarkup(templateID, template.get_name)
                        template.refresh_bookmark_markup(regTable.uxTableName.Text)
                    Else
                        Dim errLog As New bc_cs_error_log("bc_am_template", "UpdateRegisteredTable", bc_cs_error_codes.USER_DEFINED, "Table failed to register.")
                        Exit Try
                    End If
                End If

            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_template", "UpdateRegisteredTable", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            template.close_table_gallery(True)
            bc_cs_central_settings.progress_bar.unload()
            Cursor.Current = Cursors.Default
            log = New bc_cs_activity_log("bc_am_template", "UpdateRegisteredTable", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
End Class

