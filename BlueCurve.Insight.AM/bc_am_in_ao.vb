Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports BlueCurve.Create.AM
Imports Microsoft.VisualBasic
Imports System.Collections
Imports System.Math


REM ==========================================
REM Bluecurve Limited 2005
REM Module:       Insight Application Object
REM               Excel specific
REM Type:         Application Object
REM Description:  Excel
REM Version:      1
REM Change history
REM ==========================================
REM class used to pass .net colors into excel

#Region "changes"
'Changes:
'Tracker                 Initials                   Date                      Synopsis
'FIL 8115                PR                         8/1/2014                  "." in submission
#End Region
Public Class bc_ao_in_excel
    Inherits bc_ao_in_object
    Public exceldoc As Object
    Public excelapp As Object
    Public link_error As Boolean = False
    Private current_cell As Integer
    Private current_row As Integer
    Private Const const_header_start_row = 3
    Private Const const_header_start_col = 9
    Private Const const_row_start = 7
    Private Const const_col_start = 9
    Private Const const_section_space = 2
    Private Const MAX_ROWS = 500
    Const YEAR_ROW_START = 7
    Const COL_START = 11
    Const PERIOD_ROW_START = 8
    Const LABEL_ROW_START = 9


    Shared sheet_count As Integer = 0
    REM FIL 5.2 August 2014
    Public Function insert_data_into_workbook(ByVal data As Object)
        insert_data_into_workbook = False

        Try
            For i = 0 To UBound(data, 2)
                exceldoc.worksheets(data(1, i)).cells(data(2, i), data(3, i)) = data(0, i)
                REM hard code for now in 5.6 set alignment from sp
                exceldoc.worksheets(data(1, i)).cells(data(2, i), data(3, i)).HorizontalAlignment = -4152
            Next
            insert_data_into_workbook = True
        Catch ex As Exception

            Dim ocomm As New bc_cs_activity_log("bc_ao_in_excel", "insert_data_into_workbook", bc_cs_activity_codes.COMMENTARY, ex.Message)
        End Try

    End Function
    Public Overrides Sub populate_header_errors(errors As ArrayList)
        Dim ffs As New bc_dx_cell_validation
        Dim cfs As New cbc_dx_cell_validation
        Dim warningstype As New ArrayList
        Dim warningssheet As New ArrayList
        Dim warningstx As New ArrayList
        Dim warningsrow As New ArrayList
        Dim warningscol As New ArrayList
        Dim warningsaddress As New ArrayList
        Dim ef As New bc_am_ef_functions

        For i = 0 To errors.Count - 1
            warningstype.Add("Data")
            warningssheet.Add(errors(i).sheetname)
            warningstx.Add(errors(i).msg)
            warningsrow.Add(errors(i).row)
            warningscol.Add(errors(i).col)
            warningsaddress.Add(ef.excel_address(errors(i).row, errors(i).col))

        Next
        If cfs.load_data(ffs, Me, warningstype, warningssheet, warningstx, warningsrow, warningscol, warningsaddress) = True Then
            Dim oapi As New API
            'API.SetWindowPos(bc_am_in_main_frm.Handle.ToInt32, API.HWND_TOPMOST, 10, 10, 1, 1, 1)
            ffs.Show()
            ffs.TopMost = True

        End If
    End Sub
    Public Shared Function CreateNewExcelInstance() As Object

        Try
            Dim excel As Object
            excel = CreateObject("Excel.Application")
            CreateNewExcelInstance = excel
            open_addins(excel)
        Catch ex As Exception
            Dim err As New bc_cs_error_log("bc_ao_in_excel", "CreateNewExcelInstance", bc_cs_error_codes.USER_DEFINED, ex.Message)
            CreateNewExcelInstance = Nothing
        End Try
    End Function
    Public Shared Sub open_addins(ByVal oxlApp As Object)

        'Open excel addins 
        Try

            Dim di As New IO.DirectoryInfo(oxlApp.StartupPath)
            Dim diar1 As IO.FileInfo() = di.GetFiles()
            Dim dra As IO.FileInfo
            For Each dra In diar1
                Try
                    If dra.ToString.Substring(0, 1) <> "~" Then
                        oxlApp.Workbooks.Open(oxlApp.StartupPath & "\" & dra.ToString)
                    End If
                Catch ex As Exception
                End Try
            Next
            For i = 1 To oxlApp.AddIns.Count
                Try
                    oxlApp.Workbooks.Open(oxlApp.AddIns(i).FullName)
                Catch ex As Exception
                End Try
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_object", "open_addins", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try

    End Sub
    REM ===================

    Public Sub open(ByVal fn As String)
        Dim slog As New bc_cs_activity_log("bc_ao_in_excel", "open", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            exceldoc.Workbooks.Open(fn)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "open", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "open", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Function open_clone(ByVal fn As String, ByVal caption As String) As Object
        Dim slog As New bc_cs_activity_log("bc_ao_in_excel", "ope_clonen", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            open_clone = exceldoc.application.Workbooks.Open(fn)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "open_clone", bc_cs_error_codes.USER_DEFINED, ex.Message)
            open_clone = Nothing
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "open_clone", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Overrides Function issaved() As Boolean
        issaved = exceldoc.Saved
    End Function
    Public Overrides Function filename() As String
        filename = exceldoc.fullname
    End Function


    Public Sub New(ByRef oexceldocument As Object)
        exceldoc = oexceldocument
    End Sub

    Public Sub new_sheet(ByVal sheet_name As String)
        Dim slog As New bc_cs_activity_log("bc_ao_in_excel", "new_sheet", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            exceldoc.sheets.add()
            exceldoc.sheets(exceldoc.sheets.count - 1).name = sheet_name
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "new_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "new_sheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Sub disable_application_alerts()
        exceldoc.application.displayalerts = False
    End Sub
    Public Overrides Sub enable_application_alerts()
        exceldoc.application.displayalerts = True
    End Sub
    Public Overrides Function get_linked_comment(ByVal f As String) As String
        Dim slog As New bc_cs_activity_log("bc_ao_in_excel", "get_linked_comment", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim i As Integer
        Dim sheet As String
        Dim cell As String

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            If Len(f) < 9 Then
                get_linked_comment = ""
                Exit Function
            End If
            If InStr(f, "ISBLANK") > 0 Then
                f = Right(f, Len(f) - 12)
                If Left(f, 1) = "'" Then
                    f = Right(f, Len(f) - 1)
                    i = InStr(f, "'") - 1
                    sheet = Left(f, i)
                    cell = Right(f, Len(f) - i - 2)
                    cell = Left(cell, InStr(cell, ")") - 1)
                Else
                    sheet = Left(f, InStr(f, "!") - 1)

                    i = InStr(f, "!") - 1
                    cell = Right(f, Len(f) - i - 1)
                    cell = Left(cell, InStr(cell, ")") - 1)
                End If
            Else
                i = InStr(f, "!")
                sheet = Left(f, i - 1)
                sheet = Right(sheet, Len(sheet) - 1)
                cell = Right(f, Len(f) - i)
            End If

            'j = InStr(formula, """")
            'formula = Right(formula, Len(formula) - j - 2)
            'formula = Left(formula, Len(formula) - 1)
            'sheet = Left(formula, InStr(1, formula, "!") - 1)
            'cell = Right(formula, Len(formula) - InStr(1, formula, "!"))
            Dim ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "get_linked_comment", bc_cs_activity_codes.COMMENTARY, "Sheet: " + sheet)
            exceldoc.Sheets(sheet).Select()
            get_linked_comment = ""
            Try
                get_linked_comment = exceldoc.Application.Range(cell).Comment.Text()
            Catch
                ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "get_linked_comment", bc_cs_activity_codes.COMMENTARY, "Error getting linked comment on sheet: " + sheet + " cell: " + cell)
            End Try

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "get_linked_comment", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_linked_comment = ""
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "get_linked_comment", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Sub put_value_at_link_source(ByVal formula As String, ByVal value As String)
        Dim slog As New bc_cs_activity_log("bc_ao_in_excel", "put_value_at_link_source", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim i, j, k As Integer
            Dim sheet As String
            Dim cell As String
            k = InStr(1, formula, """")
            If k > 0 Then
                formula = Right(formula, Len(formula) - k - 2)
                formula = Left(formula, Len(formula) - 1)
                If Left(formula, 1) = "'" Then
                    formula = Right(formula, Len(formula) - 1)
                    i = InStr(1, formula, "!")
                    formula = Left(formula, i - 2) + Right(formula, Len(formula) - i + 1)
                End If
            Else
                If Left(formula, 2) = "='" Then
                    formula = Right(formula, Len(formula) - 2)
                    i = InStr(1, formula, "!")
                    formula = Left(formula, i - 2) + Right(formula, Len(formula) - i + 1)
                Else
                    formula = Right(formula, Len(formula) - 1)
                End If
            End If

            i = InStr(1, formula, "!")
            If i > 0 Then
                sheet = Left(formula, i - 1)
                For j = 1 To exceldoc.Worksheets.Count
                    If exceldoc.Worksheets(j).Name = sheet Then
                        exceldoc.Worksheets(j).Activate()
                        cell = Right(formula, Len(formula) - i)
                        exceldoc.worksheets(j).Cells.Range(cell).Select()
                        unprotect_sheet()
                        exceldoc.worksheets(j).cells.range(cell).value = value
                        exceldoc.worksheets(j).cells.range(cell).locked = True
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "put_value_at_link_source", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "put_value_at_link_source", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Sub follow_link(ByVal formula As String, ByVal comment As String)
        Dim slog As New bc_cs_activity_log("bc_ao_in_excel", "follow_link", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                          System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim i, j, k As Integer
            Dim sheet As String
            Dim cell As String
            k = InStr(1, formula, """")
            If k > 0 Then
                formula = Right(formula, Len(formula) - k - 2)
                formula = Left(formula, Len(formula) - 1)
                If Left(formula, 1) = "'" Then
                    formula = Right(formula, Len(formula) - 1)
                    i = InStr(1, formula, "!")
                    formula = Left(formula, i - 2) + Right(formula, Len(formula) - i + 1)
                End If
            Else
                If Left(formula, 2) = "='" Then
                    formula = Right(formula, Len(formula) - 2)
                    i = InStr(1, formula, "!")
                    formula = Left(formula, i - 2) + Right(formula, Len(formula) - i + 1)
                Else
                    formula = Right(formula, Len(formula) - 1)
                End If
            End If

            i = InStr(1, formula, "!")
            If i > 0 Then
                sheet = Left(formula, i - 1)
                For j = 1 To exceldoc.Worksheets.Count
                    If exceldoc.Worksheets(j).Name = sheet Then
                        exceldoc.Worksheets(j).Activate()
                        cell = Right(formula, Len(formula) - i)
                        exceldoc.worksheets(j).Cells.Range(cell).Select()
                        If comment <> "" Then
                            unprotect_sheet()
                            exceldoc.worksheets(j).cells.range(cell).ClearComments()
                            exceldoc.worksheets(j).cells.range(cell).addcomment()
                            exceldoc.worksheets(j).cells.range(cell).comment.text(Text:=comment & Chr(10) & "")
                        End If
                        Exit For
                    End If
                Next
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "follow_link", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI

            slog = New bc_cs_activity_log("bc_ao_in_excel", "follow_link", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Sub write_upload_values(ByVal values As bc_om_insight_retrieve_values)

        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "write_upload_values", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                  System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Dim i, j, k As Integer
        Dim co As Integer
        co = exceldoc.worksheets.count
        If co > 8 Then
            co = 8
        End If
        Try
            For i = 0 To values.values.Count - 1
                For j = 1 To co
                    If values.values(i).sheet <> "" Then
                        REM use sheet name
                        If exceldoc.worksheets(j).name = values.values(i).sheet Then
                            exceldoc.sheets(j).unprotect()
                            For k = 0 To values.values(i).value.count - 1
                                If values.values(i).value(k) <> "" Then
                                    exceldoc.sheets(j).cells(values.values(i).row, values.values(i).col).value = values.values(i).value(k)
                                    exceldoc.sheets(j).protect()
                                    Exit For
                                End If
                            Next
                            Exit For
                        End If
                    Else
                        REM use BC entity id
                        If Left(exceldoc.worksheets(j).cells(1, 1).value, 7) = "BCSHEET" Then
                            exceldoc.sheets(j).unprotect()
                            For k = 0 To values.values(i).value.count - 1
                                If values.values(i).value_entity(k) = exceldoc.worksheets(j).cells(3, 1).value Then
                                    If values.values(i).value(k) <> "" Then
                                        If IsNumeric(values.values(i).scale_factor) And IsNumeric(values.values(i).value(k)) And values.values(i).scale_factor > 0 Then
                                            exceldoc.sheets(j).cells(values.values(i).row, values.values(i).col).value = CDbl(values.values(i).value(k)) * values.values(i).scale_factor
                                        Else
                                            exceldoc.sheets(j).cells(values.values(i).row, values.values(i).col).value = values.values(i).value(k)
                                        End If

                                    End If
                                    Exit For
                                End If
                            Next
                            Try
                                exceldoc.sheets(j).protect(DrawingObjects:=False, Contents:=True, Scenarios:=True)
                            Catch

                            End Try
                        End If
                    End If
                Next
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "write_upload_values", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "write_upload_values", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Sub select_master_sheet()
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "select_master_sheet", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim i As Integer
        Dim cellvalue As String

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            For i = 1 To exceldoc.sheets.count
                cellvalue = exceldoc.worksheets(i).cells(1, 1).text
                If Left(cellvalue, 8) = "BCSHEET1" And Len(cellvalue) = 8 Then
                    exceldoc.worksheets(i).select()
                    Exit For
                End If
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "select_master_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "select_master_sheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Overrides Sub set_caption(ByVal str As String)
        exceldoc.application.caption = str
    End Sub
    Public Overrides Sub delete_system_sheets()
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "delete_system_sheets", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                      System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Dim i, j As Integer
        Try

            Dim cellvalue As String
            j = 1
            For i = 1 To exceldoc.sheets.count
                cellvalue = exceldoc.worksheets(j).cells(1, 1).value
                If Left(cellvalue, 7) = "BCSHEET" Then
                    exceldoc.worksheets(j).delete()
                Else
                    j = j + 1
                End If
            Next
        Catch ex As Exception
            'Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "delete_system_sheets", bc_cs_error_codes.USER_DEFINED, ex.Message + " :" + CStr(j))
        Finally
            exceldoc.Worksheets(1).Activate()
            exceldoc.Worksheets(1).Range("a1").Select()
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "delete_system_sheets", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides Function selectsheet(ByVal icontributor_id As Long, ByVal ientity_id As Long) As Integer
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "selectsheet", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                            System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim i As Integer
            Dim cellvalue As String
            Dim contributor_id As String
            Dim entity_id As String
            REM ===
            selectsheet = -1
            REM ===
            For i = 1 To exceldoc.sheets.count
                Try
                    cellvalue = exceldoc.worksheets(i).cells(1, 1).text
                    If Left(cellvalue, 7) = "BCSHEET" Then
                        contributor_id = CStr(exceldoc.worksheets(i).cells(2, 1).value)
                        entity_id = CStr(exceldoc.worksheets(i).cells(3, 1).value)
                        If icontributor_id = contributor_id And ientity_id = entity_id Then
                            exceldoc.worksheets(i).select()
                            selectsheet = i
                            Exit Function
                        End If
                    End If
                Catch ex As Exception
                    Dim ocomm As New bc_cs_activity_log("bc_ai_in_excel", "selectsheet", bc_cs_activity_codes.COMMENTARY, "Failed to select sheet:" + CStr(i) + ": " + ex.Message)
                End Try
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "selectsheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "selectsheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Overrides Sub status_bar_text(ByVal text As String)
        exceldoc.application.statusbar = text
    End Sub
    Public Overrides Sub screen_updating(ByVal off As Boolean)
        If off = True Then
            exceldoc.application.screenupdating = False
        Else
            exceldoc.application.screenupdating = True
        End If
    End Sub
    Public Overrides Sub close()
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "close", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            screen_updating(False)
            exceldoc.close()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "close", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "close", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides Function tmp_saveas(ByVal filename As String) As String
        Dim otrace As New bc_cs_activity_log("bc_ao_in_excel", "saveas", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim officeFormat As Integer = 0

        Try
            tmp_saveas = "Failed"

            REM SW cope with office versions
            If bc_cs_central_settings.userOfficeStatus = 3 Then
                officeFormat = 56
            ElseIf bc_cs_central_settings.userOfficeStatus = 1 Then
                officeFormat = -4143
            ElseIf bc_cs_central_settings.userOfficeStatus = 2 Then
                officeFormat = 51
            End If
            'If bc_cs_central_settings.office_version < 12 And bc_cs_central_settings.user_office_version >= 12 Then
            '    officeFormat = 56
            'ElseIf bc_cs_central_settings.office_version < 12 Then
            '    officeFormat = -4143
            'ElseIf bc_cs_central_settings.office_version >= 12 Then
            '    officeFormat = 51
            'End If

            exceldoc.saveas(bc_cs_central_settings.local_repos_path + filename, fileformat:=officeFormat, ReadOnlyRecommended:=False, addtomru:=False)

            REM SW cope with office versions
            If bc_cs_central_settings.userOfficeStatus = 2 Then
                tmp_saveas = ".xlsx"
            Else
                tmp_saveas = ".xls"
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_activity_log("bc_ao_in_exceld", "tmp_saveas", bc_cs_activity_codes.COMMENTARY, "cannot save to bc_tmp.xls file may already be open")
            tmp_saveas = ""
        Finally
            otrace = New bc_cs_activity_log("bc_ao_in_excel", "tmp_saveas", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Overrides Function save(ByVal filename As String, ByVal read_only As Boolean) As Boolean
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "save", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim officeFormat As Integer = 0
        save = False
        Try
            Dim ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "save", bc_cs_activity_codes.COMMENTARY, "Attempting to save: " + filename)
            exceldoc.save(filename)
            save = True
        Catch
            Try
                Dim ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "save", bc_cs_activity_codes.COMMENTARY, "Attempting to saveas: " + filename)


                REM SW cope with office versions
                If bc_cs_central_settings.userOfficeStatus = 3 Then
                    officeFormat = 56
                ElseIf bc_cs_central_settings.userOfficeStatus = 1 Then
                    officeFormat = -4143
                ElseIf bc_cs_central_settings.userOfficeStatus = 2 Then
                    officeFormat = 51
                End If
                'If bc_cs_central_settings.office_version < 12 And bc_cs_central_settings.user_office_version >= 12 Then
                '    officeFormat = 56
                'ElseIf bc_cs_central_settings.office_version < 12 Then
                '    officeFormat = -4143
                'ElseIf bc_cs_central_settings.office_version >= 12 Then
                '    officeFormat = 51
                'End If

                If read_only = False Then
                    exceldoc.saveas(filename, fileformat:=officeFormat, ReadOnlyRecommended:=False, addtomru:=False)
                Else
                    exceldoc.saveas(filename, fileformat:=officeFormat, ReadOnlyRecommended:=True, addtomru:=False)
                End If
                save = True
            Catch ex As Exception
                Dim omessage As New bc_cs_message("Blue Curve - Insight", "Cannot save workbook: " + filename + " as you currently have it open!", bc_cs_message.MESSAGE)
                'Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "save", bc_cs_error_codes.USER_DEFINED, ex.Message)
            End Try
        Finally
            slog = New bc_cs_activity_log("bc_ao_in_excel", "save", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
        slog = New bc_cs_activity_log("bc_ao_in_excel", "save", bc_cs_activity_codes.TRACE_EXIT, "")

    End Function
    Protected Overrides Sub validate_failed(ByVal msg As String, ByVal row As Integer, ByVal col As Integer, ByVal sheetname As String, ByVal comment_req As String)
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "validate_failed", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            REM generic fail message/log
            MyBase.validate_failed(msg, row, col, sheetname, comment_req)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "validate_failed", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ao_in_excel", "validate_failed", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Function get_cell_value(ByVal row, ByVal col)
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "get_cell_value", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            get_cell_value = exceldoc.activesheet.cells(row, col).value
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "get_cell_value", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_cell_value = ""
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "get_cell_value", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Function get_sheet_name_from_tab()
        get_sheet_name_from_tab = ""
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "get_sheet_name_from_tab", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            get_sheet_name_from_tab = exceldoc.activesheet.name
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "get_sheet_name_from_tab", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", " get_sheet_name_from_tab", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    REM get sheet
    Public Overrides Function get_sheet_name() As String
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "get_sheet_name", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            get_sheet_name = exceldoc.activesheet.cells(1, 1).value

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "get_sheet_name", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_sheet_name = ""
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "get_sheet_name", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    REM gets name of xml file behind sheet
    Public Overrides Function get_sheet_keys() As String
        REM search for master keys
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "get_sheet_keys", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                     System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try

            Dim i As Integer
            Dim cellvalue As String
            Dim contributor_id As String
            Dim entity_id As String
            get_sheet_keys = "No Keys Found"
            exceldoc.Worksheets(1).Activate()
            exceldoc.Worksheets(1).Range("a1").Select()
            For i = 1 To exceldoc.sheets.count
                Try

                    REM handle if worksheet is a full chart
                    cellvalue = exceldoc.worksheets(i).cells(1, 1).text
                    If cellvalue = "BCSHEET1" Then
                        contributor_id = exceldoc.worksheets(i).cells(2, 1).value
                        entity_id = exceldoc.worksheets(i).cells(3, 1).value
                        get_sheet_keys = "insight_" + entity_id + "_" + contributor_id
                        exceldoc.Worksheets(i).Activate()
                        exceldoc.Worksheets(i).Range("a1").Select()
                        If UCase(exceldoc.worksheets(i).cells(1, 2).value) = "READ ONLY" Then
                            get_sheet_keys = "Read Only"
                        End If

                        Exit For
                    End If
                Catch
                End Try
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "get_sheet_keys", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_sheet_keys = ""
        Finally

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "get_sheet_keys", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function


    Public Overrides Function get_key_value(ByVal row As Integer, ByVal sheet As String) As String
        REM search for master keys
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "get_key_value", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                     System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Dim i As Integer
            Dim cellvalue As String
            get_key_value = ""
            exceldoc.Worksheets(1).Activate()
            exceldoc.Worksheets(1).Range("a1").Select()
            For i = 1 To exceldoc.sheets.count
                Try
                    REM handle if worksheet is a full chart
                    cellvalue = exceldoc.worksheets(i).cells(1, 1).text
                    If cellvalue = sheet Then
                        get_key_value = exceldoc.worksheets(i).cells(row, 1).value
                        Exit For
                    End If
                Catch
                End Try
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "get_key_value", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_key_value = ""
        Finally

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "get_key_value", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Overrides Sub set_key_value(ByVal row As Integer, ByVal col As Integer, ByVal sheet As String, ByVal value As String)
        REM search for master keys
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "set_key_value", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                     System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Dim i As Integer
            Dim cellvalue As String
            exceldoc.Worksheets(1).Activate()
            exceldoc.Worksheets(1).Range("a1").Select()
            For i = 1 To exceldoc.sheets.count
                Try
                    REM handle if worksheet is a full chart
                    cellvalue = exceldoc.worksheets(i).cells(1, 1).text
                    If cellvalue = sheet Then
                        Dim ocomm As New bc_cs_activity_log("bc_ao_in_excel", "set_key_value", bc_cs_activity_codes.COMMENTARY, "Setting " + sheet + "(" + CStr(row) + ":" + CStr(col) + ") " + CStr(value))
                        Try
                            exceldoc.worksheets(i).unprotect()
                        Catch
                            ocomm = New bc_cs_activity_log("bc_ao_in_excel", "set_key_value", bc_cs_activity_codes.COMMENTARY, "Could not unprotect sheet")
                        End Try
                        exceldoc.worksheets(i).cells(row, col).value = value
                        Exit For
                    End If
                Catch
                End Try
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "set_key_value", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "set_key_value", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Sub set_sheet_name(ByVal sheet As String, ByVal value As String, ByVal alt_value As String)
        REM search for master keys
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "set_sheet_name", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                     System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Dim i As Integer
            Dim cellvalue As String
            exceldoc.Worksheets(1).Activate()
            exceldoc.Worksheets(1).Range("a1").Select()
            For i = 1 To exceldoc.sheets.count
                Try
                    REM handle if worksheet is a full chart
                    cellvalue = exceldoc.worksheets(i).cells(1, 1).text
                    If cellvalue = sheet Then
                        Try
                            exceldoc.worksheets(i).unprotect()
                        Catch
                            Dim ocomm As New bc_cs_activity_log("bc_ao_in_excel", "set_key_value", bc_cs_activity_codes.COMMENTARY, "Could not unprotect sheet")
                        End Try
                        Try
                            exceldoc.worksheets(i).name = value
                        Catch
                            exceldoc.worksheets(i).name = alt_value
                        End Try
                        Exit For
                    End If
                Catch
                End Try
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "set_sheet_name", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "set_sheet_name", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides Function is_workbook_open(ByVal fn) As Boolean
        is_workbook_open = False
        For i = 1 To exceldoc.Application.Workbooks.Count
            If exceldoc.Application.Workbooks(i).name = fn Then
                is_workbook_open = True
            End If
        Next
    End Function
    Public Overrides Sub read_only(ByVal set_flag As Boolean)
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "set_read_only", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                     System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Dim i As Integer
            Dim cellvalue As String

            exceldoc.Worksheets(1).Activate()
            exceldoc.Worksheets(1).Range("a1").Select()
            For i = 1 To exceldoc.sheets.count
                Try
                    REM handle if worksheet is a full chart
                    cellvalue = exceldoc.worksheets(i).cells(1, 1).text
                    If cellvalue = "BCSHEET1" Then
                        exceldoc.worksheets(i).unprotect()
                        If set_flag = True Then
                            exceldoc.worksheets(i).cells(1, 2).value = "Read Only"
                            exceldoc.worksheets(i).cells(1, 6).value = "(READ ONLY COPY)"
                        Else
                            exceldoc.worksheets(i).cells(1, 2).value = ""
                            exceldoc.worksheets(i).cells(1, 6).value = ""
                        End If
                        exceldoc.worksheets(i).protect(DrawingObjects:=False, Contents:=True, Scenarios:=True)
                        Exit For
                    End If
                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "set_read_only", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "set_read_only", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Function get_sheet_parameters() As String()
        REM search for master keys
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "get_sheet_parameters", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                     System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Dim i As Integer
            Dim cellvalue As String

            Dim parameters(6) As String

            exceldoc.Worksheets(1).Activate()
            exceldoc.Worksheets(1).Range("a1").Select()
            For i = 1 To exceldoc.sheets.count
                Try
                    REM handle if worksheet is a full chart
                    cellvalue = exceldoc.worksheets(i).cells(1, 1).text
                    If cellvalue = "BCSHEET1" Then
                        REM class id, class_name, entity_id, entity_name, contributor_id, contributor_name
                        parameters(0) = exceldoc.worksheets(i).cells(4, 1).value
                        parameters(1) = exceldoc.worksheets(i).cells(4, 6).value
                        parameters(2) = exceldoc.worksheets(i).cells(3, 1).value
                        parameters(3) = exceldoc.worksheets(i).cells(3, 6).value
                        parameters(4) = exceldoc.worksheets(i).cells(2, 1).value
                        parameters(5) = exceldoc.worksheets(i).cells(2, 6).value

                        exceldoc.Worksheets(i).Activate()
                        exceldoc.Worksheets(i).Range("a1").Select()
                        Exit For
                    End If
                Catch
                End Try
            Next
            get_sheet_parameters = parameters
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "get_sheet_parameters", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_sheet_parameters = Nothing
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI

            slog = New bc_cs_activity_log("bc_ao_in_excel", "get_sheet_parameters", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Friend Overrides Sub loadheaderdatafromsheet(ByRef header_values As header_values_for_sheet, ByRef insight_sheet As bc_om_insight_sheet)

        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "loadheaderdatafromsheet", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                                 System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Dim i As Integer
            Dim j As Integer

            REM read in years
            i = COL_START
            j = LABEL_ROW_START
            unprotect_sheet()
            status_bar_text("Loading Header Data from Sheet: " + CStr(insight_sheet.sheet_name))
            While CStr(exceldoc.activesheet.cells(YEAR_ROW_START, i).value) <> ""
                Dim header_values_header As New header_values_header
                If bc_am_insight_formats.linked_Header = "1" Then

                    REM if headers are linke replace exisiting headers
                    header_values_header.linked_year = (CStr(exceldoc.activesheet.cells(2, i).text))
                    If header_values_header.linked_year.Length <> 0 Then
                        header_values_header.linked_period = (CStr(exceldoc.activesheet.cells(3, i).text))
                        header_values_header.linked_e_a = (CStr(exceldoc.activesheet.cells(4, i).text))
                        header_values_header.linked_acc = (CStr(exceldoc.activesheet.cells(1, i).text))
                        REM parse this and replace existing ones
                        Dim output_year As String
                        Dim output_period As String
                        Dim output_e_a As String
                        Dim output_acc As String
                        Dim link_acc As Boolean
                        link_acc = False
                        If header_values_header.linked_acc <> "" Then
                            link_acc = True
                        End If
                        output_year = "Invalid"
                        output_period = "Invalid"
                        output_acc = "Invalid"
                        output_e_a = ""

                        parse_for_year_and_period(header_values_header.linked_year, header_values_header.linked_period, output_year, output_period, header_values_header.linked_e_a, output_e_a, "", link_acc, header_values_header.linked_acc, output_acc)

                        If output_year = "Invalid" Then
                            exceldoc.activesheet.cells(7, i).formula = exceldoc.activesheet.cells(2, i).formula
                        Else
                            exceldoc.activesheet.cells(7, i).value = output_year + output_e_a
                        End If
                        If output_period = "Invalid" Then
                            exceldoc.activesheet.cells(8, i).formula = exceldoc.activesheet.cells(3, i).formula
                        Else
                            exceldoc.activesheet.cells(8, i).value = output_period
                        End If
                        If header_values_header.linked_acc <> "" Then
                            If output_acc = "Invalid" Then
                                exceldoc.activesheet.cells(6, i).formula = exceldoc.activesheet.cells(1, i).formula
                            Else
                                exceldoc.activesheet.cells(6, i).value = output_acc
                            End If
                        End If
                    End If
                End If

                header_values_header.year = (CStr(exceldoc.activesheet.cells(7, i).value))
                header_values_header.period = (CStr(exceldoc.activesheet.cells(8, i).value))

                header_values_header.row = YEAR_ROW_START
                header_values_header.acc = (CStr(exceldoc.activesheet.cells(6, i).value))

                header_values_header.col = i
                i = i + 1
                header_values.header_values_header.Add(header_values_header)
            End While
            protect_sheet()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "loadheaderdatafromsheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI

            slog = New bc_cs_activity_log("bc_ao_in_excel", "loadheaderdatafromsheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Enum excel_error_codes
        xlErrDiv0 = -2146826281
        xlErrNA = -2146826246
        xlErrName = -2146826259
        xlErrNull = -2146826288
        xlErrNum = -2146826252
        xlErrRef = -2146826265
        xlErrValue = -2146826273
    End Enum

    Public Function test_value_is_valid(ByVal value As String, ByVal row As Long, ByVal col As Long)
        test_value_is_valid = False
        Dim tx As String = ""

        Select Case value
            Case excel_error_codes.xlErrDiv0
                tx = "#Div/0!"
            Case excel_error_codes.xlErrNA
                tx = "#N/A"
            Case excel_error_codes.xlErrName
                tx = "#Name?"
            Case excel_error_codes.xlErrNull
                tx = "#Null!"
            Case excel_error_codes.xlErrNum
                tx = "#Num!"
            Case excel_error_codes.xlErrRef
                tx = "#Ref!"
            Case excel_error_codes.xlErrValue
                tx = "#Value!"
                REM FIL IMSR-8115 
            Case "."
                tx = "."
            Case Else
                test_value_is_valid = True

        End Select
        If CStr(Asc(Trim(value))) = 46 Then
            test_value_is_valid = False
            tx = "."
        End If

        If tx <> "" Then
            Dim ocomm As New bc_cs_activity_log("bc_ao_in_excel", "test_value_is_valid", bc_cs_activity_codes.COMMENTARY, "Cell error(" + CStr(row) + "," + CStr(col) + ") " + tx)

        End If

    End Function

    '    MsgBox "#DIV/0! error"
    'xlErrNA)
    '    MsgBox "#N/A error"
    'xlErrName)
    '    MsgBox "#NAME? error"
    'xlErrNull)
    '    MsgBox "#NULL! error"
    'xlErrNum)
    '    MsgBox "#NUM! error"
    'xlErrRef)
    '    MsgBox "#REF! error"
    'xlErrValue)
    '    MsgBox "#VALUE! error"
    Protected Overrides Sub load_data_from_sheet(ByRef insight_sheet As bc_om_insight_sheet, ByVal output_only As Boolean)
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "load_data_from_sheet", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                           System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try

            Dim i As Integer
            Dim j As Integer
            Dim section_id As Integer
            Dim row_id As Integer
            Dim row As Long
            Dim scalefactor As Double
            Dim submission_code As Integer
            Dim flexible_label_flag As String = ""
            Dim comment_req As Boolean
            Dim sname As String
            Dim custom_flag As Boolean
            Dim value As bc_om_insight_rows_cell_value
            Dim enabled As String
            exceldoc.application.screenupdating = False
            REM read in years
            i = COL_START
            j = LABEL_ROW_START
            status_bar_text("Loading Period Data from Sheet: " + CStr(insight_sheet.sheet_name))
            Dim ocommentary As New bc_cs_activity_log("bc_ao_in_excel", "load_data_from_sheet", bc_cs_activity_codes.COMMENTARY, "Reading Period Data into Memory")
            While exceldoc.activesheet.cells(j - 2, 1).value <> "END"
                Dim header_values_header As New header_values_header
                REM read in period
                header_values_header.year = exceldoc.activesheet.cells(YEAR_ROW_START, i).value
                header_values_header.period = exceldoc.activesheet.cells(PERIOD_ROW_START, i).value
                header_values_header.acc = exceldoc.activesheet.cells(6, i).value
                header_values_header.row = YEAR_ROW_START
                header_values_header.col = i
                REM read in label codes
                Dim row_count As Integer = 0

                REM get label value


                section_id = -1
                row_id = -1
                custom_flag = False
                comment_req = get_row_for_insertion(exceldoc.activesheet.cells(j, 1).value, section_id, row_id, insight_sheet, row, scalefactor, submission_code, flexible_label_flag)
                While exceldoc.activesheet.cells(YEAR_ROW_START, i).value <> ""
                    REM only use if enabled
                    enabled = exceldoc.activesheet.cells(5, i).value
                    If enabled = "enabled" Or enabled = "" Then
                        REM other populate values directly into object model
                        value = New bc_om_insight_rows_cell_value(row, scalefactor, submission_code, flexible_label_flag)
                        value.year = exceldoc.activesheet.cells(YEAR_ROW_START, i).value
                        value.period_id = exceldoc.activesheet.cells(PERIOD_ROW_START, i).value
                        value.accounting_standard = exceldoc.activesheet.cells(6, i).value
                        REM PR 8-2-2008 changed this to value
                        If bc_am_insight_link_names.cell_format = "text" Then
                            If Left(exceldoc.activesheet.cells(j, i).text, 1) <> "#" Then
                                value.value = exceldoc.activesheet.cells(j, i).text
                            End If
                        Else
                            Try
                                value.value = exceldoc.activesheet.cells(j, i).value
                                If test_value_is_valid(value.value, j, i) = False Then
                                    value.value = ""
                                End If
                            Catch
                                Dim ocomm As New bc_cs_activity_log("bc_ao_in_excel", "load_data_from_sheet", bc_cs_activity_codes.COMMENTARY, "Inavlid value in cell(" + CStr(j) + "," + CStr(i) + ")")
                            End Try
                        End If
                        value.row = j
                        value.column = i
                        value.comment = ""
                        If flexible_label_flag = "True" Then
                            value.flexible_label_value = exceldoc.activesheet.cells(j, 9).value
                        End If

                        If comment_req = True Then
                            If exceldoc.activesheet.cells(j, i).hasformula Then
                                sname = exceldoc.activesheet.name
                                value.comment = get_linked_comment(exceldoc.activesheet.cells(j, i).formula)
                                exceldoc.sheets(sname).select()
                            Else
                                value.comment = get_cell_comment(insight_sheet.sheet_name, j, i)
                            End If
                        End If
                        REM assign value object to master object
                        If section_id > -1 And row_id > -1 Then
                            assign_value_for_label_code(section_id, row_id, value, insight_sheet, exceldoc.activesheet.cells(j, 9).value, output_only)
                        End If
                    End If

                    i = i + 1
                End While
                i = COL_START
                REM read in data
                j = j + 1
            End While
            REM static Data
            REM i = COL_START + 3
            i = 7
            j = 9
            ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "load_data_from_sheet", bc_cs_activity_codes.COMMENTARY, "Reading Static Data into Memory")
            Dim prev_label_code As String = ""
            Dim ord As Integer = 0
            status_bar_text("Loading Static Data from Sheet: " + CStr(insight_sheet.sheet_name))
            While (exceldoc.activesheet.cells(j, 3).value <> "END" And exceldoc.activesheet.cells(7, 3).value <> "END")
                Dim header_values_header As New header_values_header
                REM read in label codes
                Dim row_count As Integer = 0

                If exceldoc.activesheet.cells(j, 3).value <> "" Then
                    If exceldoc.activesheet.cells(j, 3).value = prev_label_code Then
                        ord = ord + 1
                    Else
                        ord = 0
                    End If
                    section_id = -1
                    row_id = -1
                    comment_req = False
                    REM other populate values directly into object model
                    custom_flag = False
                    comment_req = get_row_for_insertion_static(exceldoc.activesheet.cells(j, 3).value, section_id, row_id, insight_sheet, row, scalefactor, submission_code, flexible_label_flag)
                    value = New bc_om_insight_rows_cell_value(row, scalefactor, submission_code, flexible_label_flag)
                    If bc_am_insight_link_names.cell_format = "text" Then
                        If Left(exceldoc.activesheet.cells(j, 7).text, 1) <> "#" Then
                            value.value = exceldoc.activesheet.cells(j, 7).text
                        End If
                    Else
                        Try
                            'SK 20130204 Added a check for # to eliminate the return of a large negative number
                            'If Left(exceldoc.activesheet.cells(j, 7).text, 1) <> "#" Then
                            value.value = exceldoc.activesheet.cells(j, 7).value
                            ' End If
                            If test_value_is_valid(value.value, j, i) = False Then
                                value.value = ""
                            End If
                            'value.value = exceldoc.activesheet.cells(j, 7).value
                        Catch
                            Dim ocomm As New bc_cs_activity_log("bc_ao_in_excel", "load_data_from_sheet", bc_cs_activity_codes.COMMENTARY, "Inavlid value in cell(" + CStr(j) + ",7)")
                        End Try
                    End If
                    value.row = j
                    value.column = i
                    If flexible_label_flag = "True" Then
                        value.flexible_label_value = exceldoc.activesheet.cells(j, 5).value
                    End If
                    value.order = CStr(ord)
                    If comment_req = True Then
                        If exceldoc.activesheet.cells(j, 7).hasformula Then
                            sname = exceldoc.activesheet.name
                            value.comment = get_linked_comment(exceldoc.activesheet.cells(j, 7).formula)
                            exceldoc.sheets(sname).select()
                        Else
                            value.comment = get_cell_comment(insight_sheet.sheet_name, j, 7)
                        End If
                    End If
                    REM assign value object to master object
                    If section_id > -1 And row_id > -1 Then
                        assign_value_for_label_code_static(section_id, row_id, value, insight_sheet, exceldoc.activesheet.cells(j, 5).value, output_only)
                    End If
                End If
                REM read in data
                prev_label_code = exceldoc.activesheet.cells(j, 3).value
                j = j + 1
                exceldoc.application.screenupdating = False
            End While

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "load_data_from_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI

            exceldoc.application.screenupdating = False
            slog = New bc_cs_activity_log("bc_ao_in_excel", "load_data_from_sheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Protected Overrides Sub clear_validation_comments(ByVal sheetno As Integer, ByVal row As Integer, ByVal col As Integer)
        REM clear cell comments
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "clear_validation_comments", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try

            exceldoc.worksheets(sheetno).cells(row, col).ClearComments()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "clear_validation_comments sheets:" + CStr(sheetno) + " row:" + CStr(row) + " col:" + CStr(col), bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "clear_validation_comments", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Sub set_logical_template_id(ByVal logical_template_id As Long)
        exceldoc.activesheet.cells(2, 1).value = logical_template_id
    End Sub
    Public Sub add_config_sheet()
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "add_config_sheet", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            REM attempt to open format sheet
            Dim na, fn As String
            Dim insert_na As String
            Dim i As Integer
            exceldoc.application.ActiveWorkbook.Worksheets.add()
            na = exceldoc.Name
            Dim sheet_id As Integer
            sheet_id = exceldoc.activesheet.index

            fn = bc_cs_central_settings.local_template_path + "bc_core_insight_config.xls"

            exceldoc.application.workbooks.open(fn)
            insert_na = exceldoc.application.ActiveWorkbook.name
            exceldoc.application.ActiveWorkbook.Worksheets(1).Cells.Select()
            exceldoc.Application.Selection.Copy()
            For i = 1 To exceldoc.Application.Workbooks.Count
                If exceldoc.Application.Workbooks(i).Name = na Then
                    exceldoc.Application.Workbooks(i).Worksheets(sheet_id).Activate()
                    exceldoc.Application.ActiveSheet.Paste()
                    Exit For
                End If
            Next
            For i = 1 To exceldoc.Application.Workbooks.Count
                If exceldoc.Application.workbooks(i).name = insert_na Then
                    exceldoc.Application.workbooks(i).activate()
                    exceldoc.application.ActiveWorkbook.Worksheets(1).Cells.Select()
                    exceldoc.Application.workbooks(i).save()
                    exceldoc.Application.Workbooks(i).close()
                End If
            Next
            populate_headers(sheet_id)
            exceldoc.worksheets(sheet_id).cells(2, 1) = 0
            exceldoc.worksheets(sheet_id).cells(2, 2) = "ENTER NAME HERE..."
            exceldoc.worksheets(sheet_id).name = "New Template"

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "add_config_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "add_config_sheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub populate_headers(ByVal sheet_id As Integer)
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "populate_headers", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try

            exceldoc.worksheets(sheet_id).cells(1, 1) = "BC_IC"
            exceldoc.worksheets(sheet_id).cells(1, 2) = "Logical Template"
            exceldoc.worksheets(sheet_id).cells(4, 2) = "Period/"
            exceldoc.worksheets(sheet_id).cells(5, 2) = "Static"
            exceldoc.worksheets(sheet_id).cells(4, 3) = "Label"
            exceldoc.worksheets(sheet_id).cells(4, 4) = "Section"
            exceldoc.worksheets(sheet_id).cells(4, 5) = "Scale"
            exceldoc.worksheets(sheet_id).cells(5, 5) = "Symbol"
            exceldoc.worksheets(sheet_id).cells(4, 6) = "Scale"
            exceldoc.worksheets(sheet_id).cells(5, 6) = "Factor"
            exceldoc.worksheets(sheet_id).cells(4, 7) = "Flexible"
            exceldoc.worksheets(sheet_id).cells(5, 7) = "Label"
            exceldoc.worksheets(sheet_id).cells(4, 8) = "Link"
            exceldoc.worksheets(sheet_id).cells(5, 8) = "Code"
            exceldoc.worksheets(sheet_id).cells(4, 9) = "Data"
            exceldoc.worksheets(sheet_id).cells(5, 9) = "Type"
            exceldoc.worksheets(sheet_id).cells(4, 10) = "Submission"
            exceldoc.worksheets(sheet_id).cells(5, 10) = "Code"
            exceldoc.worksheets(sheet_id).cells(4, 11) = "Repeating"
            exceldoc.worksheets(sheet_id).cells(5, 11) = "Count"
            exceldoc.worksheets(sheet_id).cells(4, 12) = "lookup"
            exceldoc.worksheets(sheet_id).cells(3, 14) = "Validations"

            current_row = 7
            'For i = current_row To MAX_ROWS
            REM list box
            'With exceldoc.worksheets(sheet_id).cells(i, 2).Validation
            '.Delete()
            '.Add(Type:=3, AlertStyle:=1, Operator:=1, Formula1:="period,static")
            '.IgnoreBlank = True
            '.InCellDropdown = True
            '.InputTitle = ""
            '.ErrorTitle = ""
            '.InputMessage = ""
            '.ErrorMessage = ""
            '.ShowInput = True
            '.ShowError = True
            'End With
            'With exceldoc.worksheets(sheet_id).cells(i, 6).Validation
            '.Delete()
            '.Add(Type:=3, AlertStyle:=1, Operator:=1, Formula1:="0.01,1,10,100,1000000")
            '.IgnoreBlank = True
            '.InCellDropdown = True
            '.InputTitle = ""
            '.ErrorTitle = ""
            '.InputMessage = ""
            '.ErrorMessage = ""
            '.ShowInput = True
            '.ShowError = True
            'End With
            'With exceldoc.worksheets(sheet_id).cells(i, 7).Validation
            '.Delete()
            '.Add(Type:=3, AlertStyle:=1, Operator:=1, Formula1:="false,true")
            '.IgnoreBlank = True
            '.InCellDropdown = True
            '.InputTitle = ""
            '.ErrorTitle = ""
            '.InputMessage = ""
            '.ErrorMessage = ""
            '.ShowInput = True
            '.ShowError = True
            'End With
            'With exceldoc.worksheets(sheet_id).cells(i, 9).Validation
            '.Delete()
            '.Add(Type:=3, AlertStyle:=1, Operator:=1, Formula1:="number,boolean,string,datetime")
            '.IgnoreBlank = True
            '.InCellDropdown = True
            '.InputTitle = ""
            '.ErrorTitle = ""
            '.InputMessage = ""
            '.ErrorMessage = ""
            '.ShowInput = True
            '.ShowError = True
            'End With
            'With exceldoc.worksheets(sheet_id).cells(i, 10).Validation
            '.Delete()
            '.Add(Type:=3, AlertStyle:=1, Operator:=1, Formula1:="period,value,value time series, repeating, repeating time series")
            '.IgnoreBlank = True
            '.InCellDropdown = True
            '.InputTitle = ""
            '.ErrorTitle = ""
            '.InputMessage = ""
            '.ErrorMessage = ""
            '.ShowInput = True
            '.ShowError = True
            'End With
            'Next
            exceldoc.application.Goto(Reference:="R6C2")
            exceldoc.Application.ActiveWindow.FreezePanes = True

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "populate_headers", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "populate_headers", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Function worksheet_count() As Integer
        Return exceldoc.application.ActiveWorkbook.activesheet.index
    End Function

    Private Function insert_format_sheet() As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "insert_format_sheet", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As bc_cs_activity_log
        Dim fn As String = ""
        Dim oldCI As System.Globalization.CultureInfo = _
                                     System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim na, insert_na As String

            Dim i As Integer
            Dim fs As New bc_cs_file_transfer_services
            Dim orig_wb As Object

            insert_format_sheet = False
            REM attempt to open format sheet
            If bc_am_insight_formats.format_filename = "" Then
                Exit Function
            End If
            fn = bc_cs_central_settings.local_template_path + bc_am_insight_formats.format_filename
            If fs.check_document_exists(fn) = False Then
                ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Insight format file error: " + fn + " " + Err.Description)
                Exit Function
            End If
            na = exceldoc.Name

            exceldoc.worksheets.add()
            exceldoc.activesheet.name = "bc_tmp"

            orig_wb = exceldoc.application.activeworkbook

            exceldoc.application.workbooks.open(fn)
            insert_na = exceldoc.application.ActiveWorkbook.name

            exceldoc.application.ActiveWorkbook.Worksheets.Copy(before:=orig_wb.sheets(1))

            ' no longer required as worksheet is copied
            'exceldoc.application.ActiveWorkbook.Worksheets(1).Cells.Select()
            'exceldoc.Application.Selection.Copy()
            'For i = 1 To exceldoc.Application.Workbooks.Count
            '    If exceldoc.Application.Workbooks(i).Name = na Then
            '        exceldoc.Application.Workbooks(i).Worksheets(1).Activate()
            '        exceldoc.Application.ActiveSheet.Paste()
            '        Exit For
            '    End If
            'Next

            For i = 1 To exceldoc.Application.Workbooks.Count
                If exceldoc.Application.workbooks(i).name = insert_na Then
                    exceldoc.Application.workbooks(i).activate()
                    exceldoc.application.ActiveWorkbook.Worksheets(1).Cells.Select()
                    exceldoc.Application.workbooks(i).save()
                    exceldoc.Application.Workbooks(i).close()
                End If
            Next

            Me.disable_application_alerts()
            For i = 1 To exceldoc.worksheets.count
                If exceldoc.worksheets(i).name = "bc_tmp" Then
                    exceldoc.worksheets(i).delete()
                    Exit For
                End If
            Next

            insert_format_sheet = True
        Catch ex As Exception
            ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Insight format file error: " + fn + " " + Err.Description)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_excel", "insert_format_sheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Overrides Function insert_sheet(ByVal name As String, ByVal filename As String, Optional ByVal preview_mode As Boolean = False) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "insert_sheet", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try

            Dim i, j As Integer
            Dim fs As New bc_cs_file_transfer_services
            Dim orig_wb As Object
            Dim insert_wb As Object

            If filename = "" Then
                Exit Function

            End If
            REM attempt to open format shee
            'na = exceldoc.Name

            exceldoc.worksheets.add()
            exceldoc.activesheet.name = "bc_tmp"

            orig_wb = exceldoc.application.activeworkbook

            exceldoc.application.workbooks.open(filename:=filename, addtomru:=False)

            insert_wb = exceldoc.application.activeworkbook
            'copy in one go instead of one at a time (see commented out line below)
            insert_wb.Worksheets.Copy(before:=orig_wb.sheets(1))

            For j = 1 To insert_wb.Worksheets.count
                'insert_wb.Worksheets(insert_wb.Worksheets.count - j + 1).copy(before:=orig_wb.sheets(1))
                If preview_mode = True Then
                    Try
                        exceldoc.worksheets(j).cells(1, 1) = "Preview INTSHEET"
                    Catch ex As Exception
                    End Try
                End If
            Next
            'insert_wb.save()

            insert_wb.close()
            exceldoc = orig_wb

            For i = 1 To exceldoc.worksheets.count
                If exceldoc.worksheets(i).name = "bc_tmp" Then
                    exceldoc.worksheets(i).delete()
                    Exit For
                End If
            Next
            insert_sheet = True
        Catch ex As Exception

            insert_sheet = False
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "insert_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_excel", "insert_sheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Overrides Sub set_property(ByVal name As String, ByVal value As String)
        Try
            Try
                exceldoc.CustomDocumentProperties(name).Delete()
            Catch

            End Try
            exceldoc.CustomDocumentProperties.Add(name:=name, LinkToContent:=False, Type:=4, value:=value, LinkSource:=False)
        Catch ex As Exception

            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", " set_property", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Sub
    Public Overrides Function get_property(ByVal name As String) As String
        get_property = ""
        Try
            get_property = exceldoc.CustomDocumentProperties(name).value
        Catch

        End Try

    End Function
    Public Overrides Sub protect_sheet()
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "protect_sheet", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As bc_cs_activity_log
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim h As String
            h = exceldoc.activesheet.cells(1, 1).value
            If Len(h) > 7 Then
                If h.Substring(0, 7) = "BCSHEET" Then
                    exceldoc.activesheet.protect(DrawingObjects:=False, Contents:=True, Scenarios:=True)
                End If
            End If
        Catch ex As Exception
            ocommentary = New bc_cs_activity_log("bc_am_in_excel", "protect_sheet", bc_cs_activity_codes.COMMENTARY, "Failed to Protect worksheet")
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI

            slog = New bc_cs_activity_log("bc_am_in_excel", "protect_sheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides Sub unprotect_sheet()
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "unprotect_sheet", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim ocommentary As bc_cs_activity_log
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            exceldoc.activesheet.unprotect()
        Catch ex As Exception
            ocommentary = New bc_cs_activity_log("bc_am_in_excel", "unprotect_sheet", bc_cs_activity_codes.COMMENTARY, "Failed to Protect worksheet")
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI

            slog = New bc_cs_activity_log("bc_am_in_excel", "unprotect_sheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides Sub addsheet(ByVal class_id As Long, ByVal class_name As String, ByVal entity_id As Long, ByVal entity_name As String, ByVal contributor_id As String, ByVal contributor_name As String, ByVal sheet_name As String, ByVal template_id As Long)
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Dim ocalc As Integer
        ocalc = exceldoc.application.calculation



        Try
            Dim ocommentary As bc_cs_activity_log

            REM attempt to insert format sheet
            If insert_format_sheet() = False Then
                If exceldoc.application.ActiveWorkbook.Worksheets(1).visible = False Then
                    exceldoc.application.ActiveWorkbook.Worksheets(1).visible = True
                    exceldoc.application.ActiveWorkbook.Worksheets(1).select()
                    exceldoc.application.ActiveWorkbook.Worksheets.add()
                    exceldoc.application.ActiveWorkbook.Worksheets(2).visible = False
                Else
                    exceldoc.application.ActiveWorkbook.Worksheets.add()
                End If

                exceldoc.application.ActiveWorkbook.Worksheets(1).Cells.locked = False
            End If
            Me.screen_updating(True)
            exceldoc.worksheets(1).activate()
            unprotect_sheet()
            exceldoc.worksheets(1).cells(1, 1) = sheet_name
            exceldoc.worksheets(1).cells(const_header_start_row - 1, const_header_start_col - 4) = "Schema:"
            exceldoc.worksheets(1).cells(const_header_start_row, const_header_start_col - 4) = "Entity:"
            exceldoc.worksheets(1).cells(const_header_start_row + 1, const_header_start_col - 4) = "Class:"
            Try
                exceldoc.worksheets(1).cells(const_header_start_row - 1, const_header_start_col - 4).style = CStr(bc_am_insight_formats.header_Name_Style)
                exceldoc.worksheets(1).cells(const_header_start_row, const_header_start_col - 4).style = CStr(bc_am_insight_formats.header_Name_Style)
                exceldoc.worksheets(1).cells(const_header_start_row + 1, const_header_start_col - 4).style = CStr(bc_am_insight_formats.header_Name_Style)
            Catch
                ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.header_Name_Style) + " not found in workbook.")
            End Try

            exceldoc.worksheets(1).cells(const_header_start_row - 1, const_header_start_col - 3) = contributor_name
            exceldoc.worksheets(1).cells(const_header_start_row, const_header_start_col - 3) = entity_name
            exceldoc.worksheets(1).cells(const_header_start_row + 1, const_header_start_col - 3) = class_name
            Try
                exceldoc.worksheets(1).cells(const_header_start_row - 1, const_header_start_col - 3).style = CStr(bc_am_insight_formats.header_Value_Style)
                exceldoc.worksheets(1).cells(const_header_start_row, const_header_start_col - 3).style = CStr(bc_am_insight_formats.header_Value_Style)
                exceldoc.worksheets(1).cells(const_header_start_row + 1, const_header_start_col - 3).style = CStr(bc_am_insight_formats.header_Value_Style)
            Catch ex As Exception
                ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.header_Value_Style) + " not found in workbook.")
            End Try

            'exceldoc.worksheets(1).cells(6, 1) = "Period Label Code"
            'exceldoc.worksheets(1).cells(6, 2) = "Period Link Code"
            'exceldoc.worksheets(1).cells(6, 3) = "Static Label Code"
            'exceldoc.worksheets(1).cells(6, 4) = "Static Link Code"
            'exceldoc.worksheets(1).cells(6, 5) = "Static Label"
            'exceldoc.worksheets(1).cells(6, 6) = "Symbol"
            'exceldoc.worksheets(1).cells(6, 9) = "Period Label"
            'exceldoc.worksheets(1).cells(6, 10) = "Symbol"
            'exceldoc.worksheets(1).cells(7, 10) = "Year"
            'exceldoc.worksheets(1).cells(8, 10) = "Period"
            Try
                exceldoc.worksheets(1).cells(6, 1).style = CStr(bc_am_insight_formats.row_Header_Style)
                exceldoc.worksheets(1).cells(6, 2).style = CStr(bc_am_insight_formats.row_Header_Style)
                exceldoc.worksheets(1).cells(6, 3).style = CStr(bc_am_insight_formats.row_Header_Style)
                exceldoc.worksheets(1).cells(6, 4).style = CStr(bc_am_insight_formats.row_Header_Style)
                exceldoc.worksheets(1).cells(6, 5).style = CStr(bc_am_insight_formats.row_Header_Style)
                exceldoc.worksheets(1).cells(6, 6).style = CStr(bc_am_insight_formats.row_Header_Style)
                exceldoc.worksheets(1).cells(6, 7).style = CStr(bc_am_insight_formats.row_Header_Style)
                exceldoc.worksheets(1).cells(6, 9).style = CStr(bc_am_insight_formats.row_Header_Style)
                'exceldoc.worksheets(1).cells(6, 10).style = CStr(bc_am_insight_formats.row_header_style)
                'exceldoc.worksheets(1).cells(7, 10).style = CStr(bc_am_insight_formats.row_header_style)
                'exceldoc.worksheets(1).cells(8, 10).style = CStr(bc_am_insight_formats.row_header_style)
                'exceldoc.worksheets(1).cells(6, 11).style = CStr(bc_am_insight_formats.row_header_style)

            Catch ex As Exception
                ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.row_Header_Style) + " not found in workbook.")
            End Try
            REM keep entity_id and class_id in hidden row A
            exceldoc.worksheets(1).cells(2, 1) = CStr(contributor_id)
            exceldoc.worksheets(1).cells(3, 1) = CStr(entity_id)
            exceldoc.worksheets(1).cells(4, 1) = CStr(class_id)
            exceldoc.worksheets(1).cells(5, 1) = CStr(template_id)
            REM hide row A


            REM name sheet tab
            Try
                'Dim oldCI As System.Globalization.CultureInfo = _
                'System.Threading.Thread.CurrentThread.CurrentCulture

                System.Threading.Thread.CurrentThread.CurrentCulture = _
                          New System.Globalization.CultureInfo("en-GB")
                exceldoc.worksheets(1).name = class_name + "_" + CStr(sheet_name) + "_" + CStr(contributor_id)
                exceldoc.worksheets(1).name = class_name + " " + entity_name
                System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            Catch
                'Dim oldCI As System.Globalization.CultureInfo = _
                'System.Threading.Thread.CurrentThread.CurrentCulture

                System.Threading.Thread.CurrentThread.CurrentCulture = _
                          New System.Globalization.CultureInfo("en-GB")
                exceldoc.worksheets(1).name = class_name + "_" + CStr(sheet_name) + "_" + CStr(contributor_id)
                System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            End Try
            REM

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "addsheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Sub hide_worksheets()
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "hide_worksheets", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")


        Try
            If bc_am_insight_formats.hide_Sheets = 1 Then

                Dim ocommentary As New bc_cs_activity_log("bc_am_in_excel", "hide_worksheets", bc_cs_activity_codes.COMMENTARY, "Attempting to hide sheet.")
                Dim i As Integer
                For i = 1 To exceldoc.worksheets.count
                    If Left(exceldoc.worksheets(i).cells(1, 1).value, 7) = "BCSHEET" Then
                        Try

                            exceldoc.worksheets(i).visible = False
                        Catch
                            REM workbook closed
                        End Try
                    End If
                Next
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "hide_worksheets", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_excel", "hide_worksheets", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Sub show_worksheets()
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "show_worksheets", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            If bc_am_insight_formats.hide_Sheets = 1 Then
                Dim ocommentary As New bc_cs_activity_log("bc_am_in_excel", "show_worksheets", bc_cs_activity_codes.COMMENTARY, "Attempting to hide sheet.")
                Dim i As Integer
                For i = 1 To exceldoc.worksheets.count
                    If Left(exceldoc.worksheets(i).cells(1, 1).value, 7) = "BCSHEET" Then
                        exceldoc.worksheets(i).visible = True
                    End If
                Next
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "show_worksheets", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_excel", "show_worksheets", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Sub highlight_cell(ByVal sheetname As String, ByVal row As Integer, ByVal col As Integer, ByVal comment As String, ByVal bfollow_link As Boolean)
        Dim slog As New bc_cs_activity_log("bc_am_in_link", "highlight_cell", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim oldCI As System.Globalization.CultureInfo = _
                                           System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")


        Try
            Me.show_worksheets()

            REM set cursor at failed cell
            Dim i As Integer

            For i = 1 To exceldoc.worksheets.count
                Try
                    If exceldoc.worksheets(i).cells(1, 1).value = sheetname Then
                        exceldoc.worksheets(i).select()
                    End If
                Catch

                End Try
            Next
            exceldoc.activesheet.cells(row, col).select()
            REM linked header
            If (row = 6 Or row = 7 Or row = 8) And bc_am_insight_formats.linked_Header = "1" And exceldoc.activesheet.cells(row, col).hasformula = True Then
                follow_link(exceldoc.activesheet.cells(row - 5, col).formula, "")
            End If
            If bfollow_link = True And exceldoc.activesheet.cells(row, col).hasformula = True Then
                follow_link(exceldoc.activesheet.cells(row, col).formula, comment)
            Else
                REM add comment
                If comment <> "" Then
                    unprotect_sheet()
                    exceldoc.activesheet.cells(row, col).ClearComments()
                    exceldoc.activesheet.cells(row, col).addcomment()
                    exceldoc.activesheet.cells(row, col).comment.text(Text:=comment & Chr(10) & "")
                    protect_sheet()
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_link", "highlight_cell", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.hide_worksheets()

            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI

            slog = New bc_cs_activity_log("bc_am_in_link", "highlight_cell", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Function get_cell_comment(ByVal sheetname As String, ByVal row As Integer, ByVal col As Integer) As String
        Dim slog As New bc_cs_activity_log("bc_am_in_link", "get_cell_comment", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Dim i As Integer
        Try

            unprotect_sheet()
            For i = 1 To exceldoc.worksheets.count
                If exceldoc.worksheets(i).cells(1, 1).value = sheetname Then
                    exceldoc.worksheets(i).select()
                    Exit For
                End If
            Next
            exceldoc.activesheet.cells(row, col).select()
            If exceldoc.activesheet.cells(row, col).hasformula Then
                get_cell_comment = get_linked_comment(exceldoc.activesheet.cells(row, col).formula)
            Else
                Try
                    get_cell_comment = exceldoc.activesheet.cells(row, col).comment.text()
                Catch
                    get_cell_comment = ""
                End Try
            End If

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_link", "get_cell_comment", bc_cs_error_codes.USER_DEFINED, ex.Message)
            get_cell_comment = ""
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_link", "get_cell_comment", bc_cs_activity_codes.TRACE_EXIT, "")
            protect_sheet()
        End Try
    End Function
    Public Overrides Sub populate_output_data(ByRef bc_om_is As bc_om_insight_sheet)
        REM populates output values
        Dim slog = New bc_cs_activity_log("bc_am_in_link", "populate_output_data", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim i, j, k, m As Integer
        Dim c As New bc_om_insight_row
        Dim d As New bc_om_insight_rows_cell_value

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            unprotect_sheet()
            For m = 1 To exceldoc.worksheets.count
                If exceldoc.worksheets(m).cells(1, 1).value = bc_om_is.sheet_name Then
                    exceldoc.worksheets(m).select()
                    Exit For
                End If
            Next
            REM period data
            For i = 0 To bc_om_is.bc_om_insightsections.Count - 1
                For j = 0 To bc_om_is.bc_om_insightsections(i).rows.count - 1
                    If bc_om_is.bc_om_insightsections(i).rows(j).datatype = 6 Then
                        c = bc_om_is.bc_om_insightsections(i).rows(j)
                        For k = 0 To bc_om_is.bc_om_insightsections(i).rows(j).values_list.count - 1
                            d = bc_om_is.bc_om_insightsections(i).rows(j).values_list(k)
                            If d.current = True Then
                                Try
                                    REM if formula in dcell put value at source
                                    If d.value <> "" Then
                                        If exceldoc.worksheets(m).cells(c.excel_row, k + 11).hasformula Then
                                            put_value_at_link_source(exceldoc.worksheets(m).cells(c.excel_row, k + 11).formula, d.value)
                                        Else
                                            exceldoc.worksheets(m).cells(c.excel_row, k + 11).value = d.value
                                            Try
                                                exceldoc.worksheets(m).cells(c.excel_row, k + 11).locked = True
                                            Catch

                                            End Try
                                        End If
                                    End If
                                Catch
                                    Dim omsg As New bc_cs_message("Blue Curve", "There was a problem writing output values to excel please try a rebuild first or check log file", bc_cs_message.MESSAGE)
                                    Dim ocomm As New bc_cs_activity_log("bc_ao_in_excel", "populate_output_data", bc_cs_activity_codes.COMMENTARY, "Warning outputing data to excelsheet: " + CStr(m) + ":" + CStr(c.excel_row) + ":" + CStr(k + 11) + ":" + d.value)
                                    Exit Sub
                                End Try
                            End If
                        Next
                    End If

                Next
            Next
            REM static data
            For i = 0 To bc_om_is.bc_om_insightsections_static.Count - 1
                For j = 0 To bc_om_is.bc_om_insightsections_static(i).rows.count - 1
                    If bc_om_is.bc_om_insightsections_static(i).rows(j).datatype = 6 Then
                        c = bc_om_is.bc_om_insightsections_static(i).rows(j)
                        For k = 0 To bc_om_is.bc_om_insightsections_static(i).rows(j).values_list.count - 1
                            d = bc_om_is.bc_om_insightsections_static(i).rows(j).values_list(k)
                            If d.current = True Then
                                Try
                                    REM if formula in cell put value at source
                                    If exceldoc.worksheets(m).cells(c.excel_row, 7).hasformula Then
                                        put_value_at_link_source(exceldoc.worksheets(m).cells(c.excel_row, 7).formula, d.value)
                                    Else
                                        exceldoc.worksheets(m).cells(c.excel_row, 7).value = d.value
                                        Try
                                            exceldoc.worksheets(m).cells(c.excel_row, 7).locked = True
                                        Catch

                                        End Try
                                    End If
                                Catch ex As Exception
                                    Dim omsg As New bc_cs_message("Blue Curve", "There was a problem writing output values to excel please try a rebuild first or check log file", bc_cs_message.MESSAGE)
                                    Dim ocomm As New bc_cs_activity_log("bc_ao_in_excel", "populate_output_data", bc_cs_activity_codes.COMMENTARY, "Warning outputing data to excelsheet: " + CStr(m) + ":" + CStr(c.excel_row) + ":" + CStr(k + 11) + ":" + d.value)
                                    Exit Sub

                                End Try
                            End If
                        Next
                    End If

                Next
            Next


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_link", "populate_output_data", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_link", "populate_output_data", bc_cs_activity_codes.TRACE_EXIT, "")
            protect_sheet()
        End Try
    End Sub
    Public Sub remove_config_sheets()
        Dim slog As New bc_cs_activity_log("bc_am_in_link", "remove_config_sheets", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim i, j As Integer
            For i = 1 To exceldoc.sheets.count
                j = 1
                If exceldoc.worksheets(j).cells(1, 1).text = "BC_IC" Then
                    exceldoc.worksheets(j).delete()
                Else
                    j = j + 1
                End If
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_link", "remove_config_sheets", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_link", "remove_config_sheets", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Overrides Sub populate_sheet(ByRef bc_om_is As bc_om_insight_sheet)
        Dim slog = New bc_cs_activity_log("bc_am_in_link", "populate_sheet", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Dim list_count As Integer = 1

        Try
            REM loop for all sections
            REM current_cell
            Dim ocommentary As bc_cs_activity_log
            Dim max_row As Integer
            Dim list_row_start As Integer = 0
            Dim list_row_end As Integer = 0
            Dim list_boxes As New ArrayList
            current_row = const_row_start
            current_cell = const_col_start
            Dim i, j, k, l As Integer
            For i = 0 To bc_om_is.bc_om_insightsections.Count - 1
                With bc_om_is.bc_om_insightsections(i)
                    exceldoc.worksheets(1).cells(current_row, current_cell) = .name()
                    Try
                        exceldoc.worksheets(1).cells(current_row, current_cell).style = bc_am_insight_formats.section_style
                    Catch
                        ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.row_Style) + " not found in workbook.")
                    End Try
                    current_row = current_row + const_section_space
                    REM rows for section
                    For j = 0 To .rows.count - 1
                        exceldoc.worksheets(1).cells(current_row, current_cell) = .rows(j).label
                        If CStr(.rows(j).datatype) = "6" Then
                            exceldoc.worksheets(1).cells(current_row, current_cell + 1) = "(Output)"
                        Else
                            exceldoc.worksheets(1).cells(current_row, current_cell + 1) = .rows(j).scale_symbol
                        End If
                        Try
                            If .rows(j).flexible_label_flag = True Then
                                ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Flexible Label Found unlocking cell(" + CStr(current_row) + "," + CStr(current_cell) + ")")
                                Try
                                    exceldoc.worksheets(1).cells(current_row, current_cell).style = bc_am_insight_formats.flexible_Label_Style
                                    exceldoc.worksheets(1).cells(current_row, current_cell).locked = False
                                Catch
                                    ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.flexible_Label_Style) + " not found in workbook.")
                                End Try
                            Else
                                exceldoc.worksheets(1).cells(current_row, current_cell).style = bc_am_insight_formats.row_Style
                            End If
                            exceldoc.worksheets(1).cells(current_row, current_cell + 1).style = bc_am_insight_formats.row_Style
                        Catch
                            ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.row_Style) + " not found in workbook.")
                        End Try
                        REM put label code in hidden area
                        exceldoc.worksheets(1).cells(current_row, 1) = .rows(j).label_code
                        exceldoc.worksheets(1).cells(current_row, 2) = .rows(j).link_code

                        REM write row number back to object model
                        .rows(j).excel_row() = current_row
                        current_row = current_row + 1
                    Next
                    current_row = current_row + 1
                End With
            Next
            exceldoc.worksheets(1).Range("I7:I100").Select()
            exceldoc.application.Selection.AutoFormat(Format:=-4154, Number:=False, Font _
                :=False, Alignment:=False, Border:=False, Pattern:=False, Width:=True)
            exceldoc.worksheets(1).Range("J5:J100").Select()
            exceldoc.application.Selection.AutoFormat(Format:=-4154, Number:=False, Font _
                :=False, Alignment:=False, Border:=False, Pattern:=False, Width:=True)

            exceldoc.worksheets(1).cells(current_row, 1) = "END"
            max_row = current_row

            current_row = const_row_start
            For i = 0 To bc_om_is.bc_om_insightsections_static.Count - 1
                With bc_om_is.bc_om_insightsections_static(i)
                    exceldoc.worksheets(1).cells(current_row, current_cell - 4) = .name()
                    Try
                        exceldoc.worksheets(1).cells(current_row, current_cell - 4).style = bc_am_insight_formats.section_style
                    Catch
                        ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.section_style) + " not found in workbook.")
                    End Try
                    current_row = current_row + const_section_space
                    REM rows for section
                    For j = 0 To .rows.count - 1
                        exceldoc.worksheets(1).cells(current_row, current_cell - 4) = .rows(j).label
                        If CStr(.rows(j).datatype) = "6" Then
                            exceldoc.worksheets(1).cells(current_row, 6) = "(Output)"
                        Else
                            exceldoc.worksheets(1).cells(current_row, current_cell - 3) = .rows(j).scale_symbol
                        End If
                        Try
                            If .rows(j).flexible_label_flag = True Then
                                ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Flexible Label Found unlocking cell(" + CStr(current_row) + "," + CStr(current_cell - 4) + ")")
                                Try
                                    exceldoc.worksheets(1).cells(current_row, current_cell - 4).style = bc_am_insight_formats.flexible_Label_Style
                                    exceldoc.worksheets(1).cells(current_row, current_cell - 4).locked = False
                                Catch
                                    ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.flexible_Label_Style) + " not found in workbook.")
                                End Try
                            Else
                                exceldoc.worksheets(1).cells(current_row, current_cell - 4).style = bc_am_insight_formats.row_Style
                            End If
                            exceldoc.worksheets(1).cells(current_row, current_cell - 3).style = bc_am_insight_formats.row_Style
                        Catch
                            ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.row_Style) + " not found in workbook.")
                        End Try
                        REM put label code in hidden area
                        exceldoc.worksheets(1).cells(current_row, 3) = .rows(j).label_code
                        exceldoc.worksheets(1).cells(current_row, 4) = .rows(j).link_code
                        current_row = current_row + 1
                        If .rows(j).repeating_count > 0 Then
                            If .rows(j).flexible_label_value.count > 0 Then
                                If .rows(j).label = .rows(j).flexible_label_value.item(0) Then
                                    exceldoc.worksheets(1).cells(current_row - 1, current_cell - 4) = .rows(j).label + " " + CStr(1)
                                Else
                                    exceldoc.worksheets(1).cells(current_row - 1, current_cell - 4) = .rows(j).flexible_label_value.item(0)
                                End If
                            Else
                                exceldoc.worksheets(1).cells(current_row - 1, current_cell - 4) = .rows(j).label + " " + CStr(1)
                            End If

                            exceldoc.worksheets(1).cells(current_row - 1, 4) = .rows(j).link_code + "_1"
                            For k = 1 To .rows(j).repeating_count
                                If .rows(j).flexible_label_value.count > 0 Then
                                    If .rows(j).label = .rows(j).flexible_label_value.item(k) Then
                                        exceldoc.worksheets(1).cells(current_row, current_cell - 4) = .rows(j).label + " " + CStr(k + 1)
                                    Else
                                        exceldoc.worksheets(1).cells(current_row, current_cell - 4) = .rows(j).flexible_label_value.item(k)
                                    End If
                                Else
                                    exceldoc.worksheets(1).cells(current_row, current_cell - 4) = .rows(j).label + " " + CStr(k + 1)
                                End If
                                exceldoc.worksheets(1).cells(current_row, current_cell - 3) = .rows(j).scale_symbol
                                If .rows(j).flexible_label_flag = True Then
                                    exceldoc.worksheets(1).cells(current_row, current_cell - 4).style = bc_am_insight_formats.flexible_Label_Style
                                    exceldoc.worksheets(1).cells(current_row, current_cell - 4).locked = False
                                End If
                                exceldoc.worksheets(1).cells(current_row, 4) = .rows(j).link_code + "_" + CStr(k + 1)
                                REM put label code in hidden area
                                exceldoc.worksheets(1).cells(current_row, 3) = .rows(j).label_code
                                current_row = current_row + 1
                            Next
                        End If
                        REM write row number back to object model
                        .rows(j).excel_row() = current_row - 1
                        REM if lookup validation then create drop down here
                        For l = 0 To .rows(j).validations.bc_om_cell_validation.count - 1
                            If .rows(j).validations.bc_om_cell_validation(l).validation_id = "7" Then
                                Dim lb As New bc_om_list_box
                                lb.row = current_row - 1 - k
                                lb.column = 7
                                For p = 0 To .rows(j).validations.bc_om_cell_validation(l).valid_values_list.count - 1
                                    lb.values.Add(.rows(j).validations.bc_om_cell_validation(l).valid_values_list(p))
                                Next

                                lb.count = list_count
                                list_boxes.Add(lb)

                                list_count = list_count + 1
                            End If
                        Next
                    Next
                    current_row = current_row + 1
                End With
            Next
            exceldoc.worksheets(1).cells(current_row, 3) = "END"
            exceldoc.worksheets(1).Range("E7:E100").Select()
            exceldoc.application.Selection.AutoFormat(Format:=-4154, Number:=False, Font _
                :=False, Alignment:=False, Border:=False, Pattern:=False, Width:=True)
            exceldoc.worksheets(1).Range("F7:F100").Select()
            exceldoc.application.Selection.AutoFormat(Format:=-4154, Number:=False, Font _
                :=False, Alignment:=False, Border:=False, Pattern:=False, Width:=True)

            If current_row < max_row Then
                current_row = max_row

            End If
            current_row = current_row + 10
            populate_validation_list_boxes(1, list_boxes, current_row)

            REM loop for all rows
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_link", "populate_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            protect_sheet()
            slog = New bc_cs_activity_log("bc_am_in_link", "populate_sheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Public Overrides Sub evaluate_list_boxes(ByVal class_id As Long, ByVal entity_id As Long, ByVal contributor_id As Long)
        Dim slog As New bc_cs_activity_log("bc_am_in_ao", "evaluate_list_boxes", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i As Integer
            Dim worksheet As Integer
            Dim repeat As Integer = 0
            With bc_am_load_objects.obc_om_insight_contribution_for_entity
                For i = 0 To .insight_sheets.bc_om_insight_sheets.Count - 1
                    With .insight_sheets.bc_om_insight_sheets(i)
                        worksheet = selectsheet(bc_am_load_objects.obc_om_insight_contribution_for_entity.contributor_id, .entity_id)
                    End With

                    evaluate_list_boxes_for_sheet(.insight_sheets.bc_om_insight_sheets(i), worksheet)
                Next
            End With
            select_master_sheet()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_ao", "evaluate_list_boxes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
        slog = New bc_cs_activity_log("bc_am_in_ao", "evaluate_list_boxes", bc_cs_activity_codes.TRACE_EXIT, "")
    End Sub

    Public Sub evaluate_list_boxes_for_sheet(ByRef bc_om_is As bc_om_insight_sheet, ByVal worksheet As Integer)
        Dim slog = New bc_cs_activity_log("bc_am_in_link", "evaluate_list_boxes_for_sheet", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Dim list_count As Integer = 1

        Try
            REM loop for all sections
            REM current_cell
            Dim max_row As Integer
            Dim list_row_start As Integer = 0
            Dim list_row_end As Integer = 0
            Dim list_boxes As New ArrayList
            current_row = const_row_start
            current_cell = const_col_start
            Dim i, j, k, l As Integer
            For i = 0 To bc_om_is.bc_om_insightsections.Count - 1
                With bc_om_is.bc_om_insightsections(i)
                    current_row = current_row + const_section_space
                    REM rows for section
                    For j = 0 To .rows.count - 1
                        current_row = current_row + 1
                    Next
                    current_row = current_row + 1
                End With
            Next
            max_row = current_row
            current_row = const_row_start



            For i = 0 To bc_om_is.bc_om_insightsections_static.Count - 1
                With bc_om_is.bc_om_insightsections_static(i)
                    current_row = current_row + const_section_space
                    REM rows for section
                    For j = 0 To .rows.count - 1
                        current_row = current_row + 1
                        If .rows(j).repeating_count > 0 Then

                            For k = 1 To .rows(j).repeating_count
                                current_row = current_row + 1
                            Next
                        End If

                        REM if lookup validation then create drop down here
                        For l = 0 To .rows(j).validations.bc_om_cell_validation.count - 1
                            If .rows(j).validations.bc_om_cell_validation(l).validation_id = "7" Then
                                Dim lb As New bc_om_list_box
                                lb.row = current_row - 1 - k
                                lb.column = 7
                                For p = 0 To .rows(j).validations.bc_om_cell_validation(l).valid_values_list.count - 1
                                    lb.values.Add(.rows(j).validations.bc_om_cell_validation(l).valid_values_list(p))
                                Next

                                lb.count = list_count
                                list_boxes.Add(lb)

                                list_count = list_count + 1
                            End If
                        Next
                    Next
                    current_row = current_row + 1
                End With
            Next







            If current_row < max_row Then
                current_row = max_row
            End If
            current_row = current_row + 10
            populate_validation_list_boxes(worksheet, list_boxes, current_row)

            REM loop for all rows
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_link", "evaluate_list_boxes_for_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            protect_sheet()
            slog = New bc_cs_activity_log("bc_am_in_link", "evaluate_list_boxes_for_sheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Sub populate_validation_list_boxes(ByVal worksheet As Integer, ByVal list_boxes As ArrayList, ByVal current_row As Integer)
        Try
            Dim list_row_end As Integer
            Dim list_formula As String
            Dim ef As New bc_am_ef_functions

            list_row_end = 0
            Me.unprotect_sheet()

            REM sort out validaion list boxes
            For i = 0 To list_boxes.Count - 1
                list_formula = "=" + CStr(ef.excel_address(current_row, list_boxes(i).count)) + ":" + CStr(ef.excel_address(current_row + list_boxes(i).values.count - 1, list_boxes(i).count))
                With exceldoc.worksheets(worksheet).cells(list_boxes(i).row, list_boxes(i).column).Validation
                    .Delete()
                    .Add(Type:=3, AlertStyle:=1, Operator:=1, Formula1:=list_formula)
                    .IgnoreBlank = True
                    .InCellDropdown = True
                    .InputTitle = ""
                    .ErrorTitle = ""
                    .InputMessage = ""
                    .ErrorMessage = ""
                    .ShowInput = True
                    .ShowError = True
                End With
                For j = 0 To list_boxes(i).values.count - 1
                    exceldoc.worksheets(worksheet).cells(current_row + j, list_boxes(i).count) = list_boxes(i).values(j)
                    If current_row + j > list_row_end Then
                        list_row_end = list_row_end + 1
                    End If
                Next
            Next

            If list_row_end > current_row Then

                exceldoc.worksheets(worksheet).Rows(CStr(current_row) + ":" + CStr(list_row_end)).Select()
                exceldoc.application.Selection.EntireRow.Hidden = True

            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_in_excel", "populate_validation_list_boxes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Me.protect_sheet()
        End Try
    End Sub
    Public Function test_for_config_sheet(ByVal sheet_id As Integer) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "test_for_config_sheet", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim parameters(6) As String

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            test_for_config_sheet = False
            REM is sheet config sheet 
            If exceldoc.worksheets(sheet_id).cells(1, 1).text = "BC_IC" Then
                test_for_config_sheet = True
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "test_for_config_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_excel", "test_for_config_sheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Function get_logical_template_id(ByVal sheet_id As Integer) As Integer
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "get_logical_template_id", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim parameters(6) As String

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            get_logical_template_id = exceldoc.worksheets(sheet_id).cells(2, 1).value
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "get_logical_template_id", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_excel", "get_logical_template_id", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Function delete_sheet(ByVal sheet_id As Integer) As Integer
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "get_logical_template_id", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim parameters(6) As String

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            exceldoc.worksheets(sheet_id).delete()
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "get_logical_template_id", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_excel", "get_logical_template_id", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Function read_config_data(ByVal sheet_id As Integer, ByRef oconfigdata As bc_om_insight_template_config) As Integer
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "read_config_data", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim parameters(6) As String
        Const start_row = 7
        Const start_col = 2

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            status_bar_text("Reading in data...")
            read_config_data = 0

            Dim values(MAX_ROWS + start_row, 11 + start_col) As String
            Dim i, j As Integer
            REM firstly read data in
            For i = start_row To start_row + MAX_ROWS
                For j = start_col To 12
                    values(i, j) = exceldoc.worksheets(sheet_id).cells(i, j).text
                Next
            Next
            status_bar_text("Validating  data...")
            REM check for blanks
            For i = start_row To start_row + MAX_ROWS
                exceldoc.worksheets(sheet_id).cells(i, 11).ClearComments()
                If values(i, 2) = "period" Or values(i, 2) = "static" Then
                    exceldoc.worksheets(sheet_id).cells(i, 3).ClearComments()
                    If exceldoc.worksheets(sheet_id).cells(i, 3).text = "" Then
                        exceldoc.worksheets(sheet_id).cells(i, 3).AddComment("label name must be entered")
                        read_config_data = 1
                    End If
                    exceldoc.worksheets(sheet_id).cells(i, 4).ClearComments()
                    If exceldoc.worksheets(sheet_id).cells(i, 4).text = "" Then
                        exceldoc.worksheets(sheet_id).cells(i, 4).AddComment("section name must be entered")
                        read_config_data = 1
                    End If
                    exceldoc.worksheets(sheet_id).cells(i, 6).ClearComments()
                    If exceldoc.worksheets(sheet_id).cells(i, 6).text = "" Then
                        exceldoc.worksheets(sheet_id).cells(i, 6).AddComment("scale factor must be entered")
                        read_config_data = 1
                    End If
                    exceldoc.worksheets(sheet_id).cells(i, 7).ClearComments()
                    If exceldoc.worksheets(sheet_id).cells(i, 7).text = "" Then
                        exceldoc.worksheets(sheet_id).cells(i, 7).AddComment("flexible label must be entered")
                        read_config_data = 1
                    End If
                    exceldoc.worksheets(sheet_id).cells(i, 9).ClearComments()
                    If exceldoc.worksheets(sheet_id).cells(i, 9).text = "" Then
                        exceldoc.worksheets(sheet_id).cells(i, 9).AddComment("data type must be entered")
                        read_config_data = 1
                    End If
                    exceldoc.worksheets(sheet_id).cells(i, 10).ClearComments()
                    If exceldoc.worksheets(sheet_id).cells(i, 10).text = "" Then
                        exceldoc.worksheets(sheet_id).cells(i, 10).AddComment("submission code must be entered")
                        read_config_data = 1
                    End If
                    exceldoc.worksheets(sheet_id).cells(i, 11).ClearComments()
                    If exceldoc.worksheets(sheet_id).cells(i, 11).text = "" Then
                        exceldoc.worksheets(sheet_id).cells(i, 11).AddComment("repeating count must be entered")
                        read_config_data = 1
                    End If

                End If
            Next
            REM submission code column 9 
            For i = start_row To start_row + MAX_ROWS
                exceldoc.worksheets(sheet_id).cells(i, 10).ClearComments()
                If values(i, 2) = "period" And values(i, 10) <> "period" And values(i, 11) <> "" Then
                    exceldoc.worksheets(sheet_id).cells(i, 10).AddComment("submission code must be period for period data")
                    read_config_data = 1
                End If
                If values(i, 2) <> "period" And values(i, 10) = "period" And values(i, 11) <> "" Then
                    exceldoc.worksheets(sheet_id).cells(i, 10).AddComment("submission code cant be period for static data")
                    read_config_data = 1
                End If

            Next
            REM repeating count column 11 must be numeric
            For i = start_row To start_row + MAX_ROWS
                exceldoc.worksheets(sheet_id).cells(i, 11).ClearComments()
                If IsNumeric(values(i, 11)) = False And values(i, 11) <> "" Then
                    exceldoc.worksheets(sheet_id).cells(i, 11).AddComment("Numeric must be entered")
                    read_config_data = 1
                End If
            Next

            If read_config_data = 1 Then
                status_bar_text("")
                Exit Function
            End If
            status_bar_text("Translating Data...")
            REM assign to object
            REM headline data
            oconfigdata.logical_template_id = exceldoc.worksheets(sheet_id).cells(2, 1).value
            oconfigdata.logical_template_name = exceldoc.worksheets(sheet_id).cells(2, 2).text
            oconfigdata.context_id = 1
            oconfigdata.rows.Clear()
            Dim orow As bc_om_insight_row_config
            For i = start_row To start_row + MAX_ROWS
                If values(i, 2) <> "" Then
                    orow = New bc_om_insight_row_config
                    orow.order = i
                    For j = start_col To 12
                        If values(i, 2) = "period" Then
                            orow.static_flag = 0
                        Else
                            orow.static_flag = 1
                        End If
                        orow.label = values(i, 3)
                        orow.section_name = values(i, 4)
                        orow.scale_symbol = values(i, 5)
                        orow.scale_factor = values(i, 6)
                        If LCase(values(i, 7)) = "false" Then
                            orow.flexible_label_flag = 0
                        Else
                            orow.flexible_label_flag = 1
                        End If
                        orow.link_code = values(i, 8)
                        If values(i, 9) = "numeric" Then
                            orow.data_type = 0
                        ElseIf values(i, 9) = "boolean" Then
                            orow.data_type = 1
                        ElseIf values(i, 9) = "string" Then
                            orow.data_type = 2
                        ElseIf values(i, 9) = "datetime" Then
                            orow.data_type = 3
                        End If
                        If values(i, 10) = "period" Then
                            orow.submission_code = 0
                        ElseIf values(i, 10) = "value" Then
                            orow.submission_code = 1
                        ElseIf values(i, 10) = "value time series" Then
                            orow.submission_code = 2
                        ElseIf values(i, 10) = "repeating" Then
                            orow.submission_code = 3
                        ElseIf values(i, 10) = "repeating time series" Then
                            orow.submission_code = 4
                        End If
                        orow.repeating_count = values(i, 11)
                        orow.lookup_sql = values(i, 12)
                    Next
                    REM validations
                    Dim val_col As Integer
                    val_col = 14
                    orow.validations.Clear()
                    While exceldoc.worksheets(sheet_id).cells(5, val_col).text <> ""
                        If UCase(exceldoc.worksheets(sheet_id).cells(i, val_col).text) = "TRUE" Then
                            orow.validations.Add(exceldoc.worksheets(sheet_id).cells(6, val_col).text)
                        End If
                        val_col = val_col + 1
                    End While
                    oconfigdata.rows.Add(orow)
                End If

            Next

            status_bar_text("")
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "read_config_data", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_excel", "read_config_data", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Public Sub config_populate_sheet(ByVal bc_om_is As bc_om_insight_sheet, ByVal sheet_id As Integer)
        Dim slog = New bc_cs_activity_log("bc_am_in_excel", "config_populate_sheet", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim i, j As Integer
            Dim current_row As Integer
            current_row = 7
            populate_headers(sheet_id)
            exceldoc.worksheets(sheet_id).cells(2, 1) = bc_om_is.logical_template_id
            exceldoc.worksheets(sheet_id).cells(2, 2) = bc_om_is.logical_template_name
            Dim dt As Integer

            REM set up dymnamic validations
            For i = 0 To bc_om_is.validations_ids.Count - 1
                exceldoc.worksheets(sheet_id).cells(5, i + 14) = bc_om_is.validations_names(i)
                exceldoc.worksheets(sheet_id).cells(6, i + 14) = bc_om_is.validations_ids(i)
                'For j = current_row To current_row + MAX_ROWS
                'With exceldoc.worksheets(sheet_id).cells(j, i + 14).Validation
                'Delete()
                '.Add(Type:=3, AlertStyle:=1, Operator:=1, Formula1:="false,true")
                '.IgnoreBlank = True
                '.InCellDropdown = True
                '.InputTitle = ""
                '.ErrorTitle = ""
                '.InputMessage = ""
                '.ErrorMessage = ""
                '.ShowInput = True
                '.ShowError = True
                'End With
            Next



            Dim o As New bc_om_insight_row
            For i = 0 To bc_om_is.bc_om_insightsections.Count - 1
                For j = 0 To bc_om_is.bc_om_insightsections(i).rows.count - 1
                    exceldoc.worksheets(sheet_id).cells(current_row, 2) = "period"
                    exceldoc.worksheets(sheet_id).cells(current_row, 3) = bc_om_is.bc_om_insightsections(i).rows(j).label
                    exceldoc.worksheets(sheet_id).cells(current_row, 4) = bc_om_is.bc_om_insightsections(i).name
                    exceldoc.worksheets(sheet_id).cells(current_row, 5) = bc_om_is.bc_om_insightsections(i).rows(j).scale_symbol
                    exceldoc.worksheets(sheet_id).cells(current_row, 6) = bc_om_is.bc_om_insightsections(i).rows(j).scale_factor

                    If bc_om_is.bc_om_insightsections(i).rows(j).flexible_label_flag = 0 Then
                        exceldoc.worksheets(sheet_id).cells(current_row, 7) = "false"
                    Else
                        exceldoc.worksheets(sheet_id).cells(current_row, 7) = "true"
                    End If
                    exceldoc.worksheets(sheet_id).cells(current_row, 8) = bc_om_is.bc_om_insightsections(i).rows(j).link_code
                    dt = bc_om_is.bc_om_insightsections(i).rows(j).datatype
                    Select Case dt
                        Case 0
                            exceldoc.worksheets(sheet_id).cells(current_row, 9) = "number"
                        Case 1
                            exceldoc.worksheets(sheet_id).cells(current_row, 9) = "boolean"
                        Case 2
                            exceldoc.worksheets(sheet_id).cells(current_row, 9) = "string"
                        Case 3
                            exceldoc.worksheets(sheet_id).cells(current_row, 9) = "datetime"
                        Case Is > 3
                            exceldoc.worksheets(sheet_id).cells(current_row, 9) = "string"
                    End Select
                    exceldoc.worksheets(sheet_id).cells(current_row, 10) = "period"
                    exceldoc.worksheets(sheet_id).cells(current_row, 11) = 0
                    exceldoc.worksheets(sheet_id).cells(current_row, 12) = bc_om_is.bc_om_insightsections(i).rows(j).lookup_sql

                    Dim l, k As Integer
                    For k = 0 To bc_om_is.validations_ids.Count - 1
                        exceldoc.worksheets(sheet_id).cells(current_row, k + 14) = "false"
                        For l = 0 To bc_om_is.bc_om_insightsections(i).rows(j).validations.bc_om_cell_validation.Count - 1
                            If bc_om_is.validations_ids(k) = bc_om_is.bc_om_insightsections(i).rows(j).validations.bc_om_cell_validation(l).validation_id Then
                                exceldoc.worksheets(sheet_id).cells(current_row, k + 14) = "true"
                            End If
                        Next
                    Next
                    current_row = current_row + 1
                Next
                current_row = current_row + 1
            Next
            For i = 0 To bc_om_is.bc_om_insightsections_static.Count - 1
                For j = 0 To bc_om_is.bc_om_insightsections_static(i).rows.count - 1
                    exceldoc.worksheets(sheet_id).cells(current_row, 2) = "static"
                    exceldoc.worksheets(sheet_id).cells(current_row, 3) = bc_om_is.bc_om_insightsections_static(i).rows(j).label
                    exceldoc.worksheets(sheet_id).cells(current_row, 4) = bc_om_is.bc_om_insightsections_static(i).name
                    exceldoc.worksheets(sheet_id).cells(current_row, 5) = bc_om_is.bc_om_insightsections_static(i).rows(j).scale_symbol
                    exceldoc.worksheets(sheet_id).cells(current_row, 6) = bc_om_is.bc_om_insightsections_static(i).rows(j).scale_factor
                    If bc_om_is.bc_om_insightsections_static(i).rows(j).flexible_label_flag = 0 Then
                        exceldoc.worksheets(sheet_id).cells(current_row, 7) = "false"
                    Else
                        exceldoc.worksheets(sheet_id).cells(current_row, 7) = "true"
                    End If
                    exceldoc.worksheets(sheet_id).cells(current_row, 8) = bc_om_is.bc_om_insightsections_static(i).rows(j).link_code
                    dt = bc_om_is.bc_om_insightsections_static(i).rows(j).datatype
                    Select Case dt
                        Case 0
                            exceldoc.worksheets(sheet_id).cells(current_row, 9) = "number"
                        Case 1
                            exceldoc.worksheets(sheet_id).cells(current_row, 9) = "boolean"
                        Case 2
                            exceldoc.worksheets(sheet_id).cells(current_row, 9) = "string"
                        Case 3
                            exceldoc.worksheets(sheet_id).cells(current_row, 9) = "datetime"
                        Case Is > 3
                            exceldoc.worksheets(sheet_id).cells(current_row, 9) = "string"
                    End Select
                    dt = bc_om_is.bc_om_insightsections_static(i).rows(j).submission_code
                    Select Case dt
                        Case 0
                            exceldoc.worksheets(sheet_id).cells(current_row, 10) = "Invalid code:0"
                        Case 1
                            exceldoc.worksheets(sheet_id).cells(current_row, 10) = "value"
                        Case 2
                            exceldoc.worksheets(sheet_id).cells(current_row, 10) = "value time series"
                        Case 3
                            exceldoc.worksheets(sheet_id).cells(current_row, 10) = "repeating"
                        Case 4
                            exceldoc.worksheets(sheet_id).cells(current_row, 10) = "repeating time series"

                    End Select
                    exceldoc.worksheets(sheet_id).cells(current_row, 11) = bc_om_is.bc_om_insightsections_static(i).rows(j).repeating_count
                    exceldoc.worksheets(sheet_id).cells(current_row, 12) = bc_om_is.bc_om_insightsections_static(i).rows(j).lookup_sql
                    REM validations
                    Dim l, k As Integer
                    For k = 0 To bc_om_is.validations_ids.Count - 1
                        exceldoc.worksheets(sheet_id).cells(current_row, k + 14) = "false"
                        For l = 0 To bc_om_is.bc_om_insightsections_static(i).rows(j).validations.bc_om_cell_validation.Count - 1
                            If bc_om_is.validations_ids(k) = bc_om_is.bc_om_insightsections_static(i).rows(j).validations.bc_om_cell_validation(l).validation_id Then
                                exceldoc.worksheets(sheet_id).cells(current_row, k + 14) = "true"
                            End If
                        Next
                    Next
                    current_row = current_row + 1
                Next
                current_row = current_row + 1
            Next
            set_config_status("Last downloaded: " + CStr(Date.Now.Day) + "-" + CStr(Date.Now.Month) + "-" + CStr(Date.Now.Year) + " " + CStr(Date.Now.Hour) + ":" + CStr(Date.Now.Minute) + ":" + CStr(Date.Now.Second), sheet_id)

            Try
                exceldoc.worksheets(sheet_id).name = Left(bc_om_is.logical_template_name, 30)
            Catch
                Try
                    exceldoc.worksheets(sheet_id).name = "logical template " + CStr(bc_om_is.logical_template_id)
                Catch

                End Try
            End Try
            REM hide column one as controlled area
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "config_populate_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_excel", "config_populate_sheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub set_config_status(ByVal st As String, ByVal sheet_id As Integer)
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "set_config_status", bc_cs_activity_codes.TRACE_EXIT, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            exceldoc.worksheets(sheet_id).cells(2, 9) = st
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "set_config_status", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_excel", "set_config_status", bc_cs_activity_codes.TRACE_EXIT, "")

        End Try
    End Sub
    Protected Overrides Function getsheetnofromname(ByVal name As String) As Integer

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim i As Integer
            For i = 1 To exceldoc.worksheets.count
                If exceldoc.worksheets(i).cells(1, 1).value = name Then
                    getsheetnofromname = i
                    Exit For
                End If
            Next
        Catch

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try

    End Function
    Public Function write_io_data(ByVal elements As bc_io_output_elements, Optional ByVal backward As Boolean = False) As Boolean

        Dim oldCI As System.Globalization.CultureInfo = _
                                           System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            If backward = False Then
                For i = 0 To elements.elements.Count - 1
                    If elements.elements(i).row > 0 And elements.elements(i).col > 0 Then
                        If IsNumeric(elements.elements(i).value) Then
                            exceldoc.activesheet.cells(elements.elements(i).row, elements.elements(i).col).value = CDbl(elements.elements(i).value)
                        ElseIf IsDate(elements.elements(i).value) Then
                            exceldoc.activesheet.cells(elements.elements(i).row, elements.elements(i).col).value = CDate(elements.elements(i).value)
                        Else
                            exceldoc.activesheet.cells(elements.elements(i).row, elements.elements(i).col).value = elements.elements(i).value
                        End If
                    End If
                Next
            Else
                Dim lrc As Integer
                Dim row_inc As Integer = 0
                Dim prev_row As Integer = 0
                If elements.elements.Count > 0 Then
                    lrc = elements.elements(elements.elements.Count - 1).row
                    For i = 0 To elements.elements.Count - 1
                        If prev_row <> elements.elements(i).row And i > 0 Then
                            row_inc = row_inc + 1
                        End If
                        If elements.elements(i).row > 0 And elements.elements(i).col > 0 Then
                            If IsNumeric(elements.elements(i).value) Then
                                exceldoc.activesheet.cells(lrc - row_inc, elements.elements(i).col).value = CDbl(elements.elements(i).value)
                            ElseIf IsDate(elements.elements(i).value) Then
                                exceldoc.activesheet.cells(lrc - row_inc, elements.elements(i).col).value = CDate(elements.elements(i).value)
                            Else
                                exceldoc.activesheet.cells(lrc - row_inc, elements.elements(i).col).value = elements.elements(i).value
                            End If
                        End If
                        prev_row = elements.elements(i).row
                    Next
                End If
            End If
            write_io_data = True
        Catch ex As Exception
            write_io_data = False
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "write_io_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Function

    Public Function write_io_periodheaders(ByVal periodheaders As bc_om_period_headers, ByVal entity As String, ByVal attribute As String) As Boolean

        Dim oldCI As System.Globalization.CultureInfo = _
                                           System.Threading.Thread.CurrentThread.CurrentCulture


        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try

            Dim lrc As Integer
            Dim lcol As Integer = 0

            REM show headers
            If periodheaders.periodheadings.Count > 0 Then

                lrc = 3
                lcol = 1
                exceldoc.activesheet.cells(1, 1).value = entity
                exceldoc.activesheet.cells(2, 1).value = attribute

                For i = 0 To periodheaders.periodheadings.Count - 1
                    exceldoc.activesheet.cells(lrc, lcol + 1).value = Str(periodheaders.periodheadings(i).periodyear)
                    exceldoc.activesheet.cells(lrc + 1, lcol + 1).value = periodheaders.periodheadings(i).periodeaflag
                    exceldoc.activesheet.cells(lrc + 2, lcol + 1).value = periodheaders.periodheadings(i).periodname
                    lcol = lcol + 2
                Next
            End If

            write_io_periodheaders = True
        Catch ex As Exception
            write_io_periodheaders = False
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "write_io_periodheaders", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try
    End Function



    Public Function read_io_data(ByVal template As bc_in_excel_io_template, ByRef oelements As bc_om_excel_data_elements, ByRef errors As bc_om_excel_data_element_errors) As Boolean
        Dim oelement As bc_om_excel_data_element
        Dim row As Integer
        Dim i As Integer
        Dim err_tx As String
        Dim eerr As bc_om_excel_data_element_error
        Dim col As Integer
        Dim single_entity As Boolean = False
        Dim endBlockColumn As Long

        row = template.list_start_row
        read_io_data = True
        oelements.elements.Clear()

        Dim oldCI As System.Globalization.CultureInfo = _
                                           System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            If template.io_mode = "time series" Then
                REM only allowed one entiy and one item
                row = template.items(0).start_row
                col = template.items(0).start_col
                While CStr(exceldoc.activesheet.cells(row, col).value) <> template.list_end_delimiter
                    If CStr(exceldoc.activesheet.cells(row, col).value) <> "" Then
                        oelement = New bc_om_excel_data_element
                        oelement.entity_row = template.entity_name_row_id
                        oelement.entity_col = template.entity_name_col_id
                        oelement.entity_key = template.entity_name
                        oelement.entity_key_name = template.entity_identifier
                        oelement.item_key = template.items(0).item_identifier
                        oelement.copy_to_draft = template.items(0).copy_to_draft
                        oelement.value = CStr(exceldoc.activesheet.cells(row, col).value)
                        oelement.submission_code = template.items(0).submission_code
                        oelement.contributor = template.items(0).contributor
                        oelement.row = row
                        oelement.col = col
                        REM date if required
                        If template.items(0).date_col <> 0 Then
                            Try
                                oelement.date_from = CStr(exceldoc.activesheet.cells(row, template.items(0).date_col).value)
                            Catch
                                eerr = New bc_om_excel_data_element_error
                                eerr.worksheet = exceldoc.activesheet.name
                                eerr.type = "Cell"
                                eerr.row = row
                                eerr.col = template.items(0).date_col
                                eerr.err_tx = "invalid date format"
                                add_error(errors, eerr)
                            End Try
                        Else
                            eerr = New bc_om_excel_data_element_error
                            eerr.worksheet = exceldoc.activesheet.name
                            eerr.type = "Cell"
                            eerr.row = row
                            eerr.col = col
                            eerr.err_tx = "no date supplied for item in time series set"
                            add_error(errors, eerr)
                            read_io_data = False
                        End If
                        err_tx = client_side_validate(oelement.value, template.items(0).mandatory, template.items(0).datatype, oelement.entity_key, template.items(0).item_display_name)
                        If err_tx = "" Then
                            oelements.elements.Add(oelement)
                        Else
                            eerr = New bc_om_excel_data_element_error
                            eerr.worksheet = exceldoc.activesheet.name

                            eerr.row = row
                            eerr.col = col
                            eerr.err_tx = err_tx
                            add_error(errors, eerr)
                            read_io_data = False
                        End If
                    End If
                    row = row + 1
                    If row = 5000 And template.list_end_delimiter <> "" Then
                        eerr = New bc_om_excel_data_element_error
                        eerr.worksheet = exceldoc.activesheet.name
                        eerr.type = "Cell"
                        eerr.row = template.list_start_row
                        eerr.col = template.list_start_col
                        eerr.err_tx = "no end delimiter found on column:" + template.list_end_delimiter
                        add_error(errors, eerr)
                        read_io_data = False
                        Exit Function
                    End If
                End While

            ElseIf template.io_mode = "current" And template.read_entity_mode <> "from list" Then
                REM single entity
                row = template.list_start_row
                oelement = New bc_om_excel_data_element

                REM read in each item
                row = template.entity_name_row_id
                For i = 0 To template.items.Count - 1
                    oelement = New bc_om_excel_data_element
                    oelement.entity_key_name = template.entity_identifier
                    oelement.entity_key = exceldoc.activesheet.cells(template.entity_name_row_id, template.entity_name_col_id).value
                    oelement.item_key = template.items(i).item_identifier
                    If template.items(i).start_row <> 0 Then
                        row = template.items(i).start_row
                    End If
                    oelement.value = CStr(exceldoc.activesheet.cells(row, template.items(i).start_col).value)
                    oelement.submission_code = template.items(i).submission_code
                    oelement.contributor = template.items(i).contributor
                    oelement.copy_to_draft = template.items(i).copy_to_draft
                    oelement.row = row
                    oelement.col = col

                    REM date if required
                    If template.items(i).date_col <> 0 Then
                        Try
                            oelement.date_from = CStr(exceldoc.activesheet.cells(row, template.items(i).date_col).value)
                        Catch
                            eerr = New bc_om_excel_data_element_error
                            eerr.worksheet = exceldoc.activesheet.name
                            eerr.type = "Cell"
                            eerr.row = row
                            eerr.col = template.items(i).date_col
                            eerr.err_tx = "invalid date format"
                            add_error(errors, eerr)
                            read_io_data = False
                        End Try
                    End If
                    err_tx = client_side_validate(oelement.value, template.items(i).mandatory, template.items(i).datatype, oelement.entity_key, template.items(i).item_display_name)
                    If err_tx = "" Then
                        oelements.elements.Add(oelement)
                    Else
                        eerr = New bc_om_excel_data_element_error
                        eerr.worksheet = exceldoc.activesheet.name
                        eerr.row = row
                        eerr.col = template.items(i).start_col
                        eerr.err_tx = err_tx
                        add_error(errors, eerr)
                        read_io_data = False
                    End If
                Next
            ElseIf template.io_mode = "current" And template.read_entity_mode = "from list" Then
                row = template.list_start_row
                col = template.list_start_col
                Dim end_col As Boolean
                Dim col_count As Integer

                While CStr(exceldoc.activesheet.cells(row, col).value) <> template.list_end_delimiter
                    If CStr(exceldoc.activesheet.cells(row, col).value) <> "" Then
                        For i = 0 To template.items.Count - 1
                            end_col = False
                            col_count = 0
                            While end_col = False
                                oelement = New bc_om_excel_data_element
                                oelement.entity_row = row
                                oelement.entity_col = col
                                oelement.entity_key = exceldoc.activesheet.cells(row, col).value
                                oelement.entity_key_name = template.entity_identifier
                                oelement.item_key = template.items(i).item_identifier
                                oelement.value = CStr(exceldoc.activesheet.cells(row, template.items(i).start_col + col_count).value)
                                oelement.submission_code = template.items(i).submission_code
                                oelement.contributor = template.items(i).contributor
                                oelement.copy_to_draft = template.items(i).copy_to_draft
                                oelement.row = row
                                oelement.col = template.items(i).start_col + col_count
                                oelement.period_row = template.items(i).period_header_row
                                oelement.period_end_item_key = template.items(i).period_end_item_key
                                oelement.period_end_item_row = template.items(i).period_end_item_row


                                REM period data
                                If template.items(i).submission_code = 0 Then
                                    Dim tyear As String
                                    tyear = CStr(exceldoc.activesheet.cells(template.items(i).year_header_row, template.items(i).start_col + col_count).value)
                                    If tyear = template.items(i).item_col_delimiter Then
                                        end_col = True
                                        Exit While
                                    Else
                                        If template.items(i).year_header_row = template.items(i).ea_header_row Then
                                            If Len(tyear) <> 5 Then
                                                eerr = New bc_om_excel_data_element_error
                                                eerr.worksheet = exceldoc.activesheet.name
                                                eerr.type = "Cell"
                                                eerr.row = template.items(i).year_header_row
                                                eerr.col = template.items(i).start_col + col_count
                                                eerr.err_tx = "invalid year/period format: " + tyear
                                                add_error(errors, eerr)
                                            Else
                                                If tyear.Substring(4, 1) <> "A" And tyear.Substring(4, 1) <> "E" Then
                                                    eerr = New bc_om_excel_data_element_error
                                                    eerr.worksheet = exceldoc.activesheet.name
                                                    eerr.type = "Cell"
                                                    eerr.row = template.items(i).year_header_row
                                                    eerr.col = template.items(i).start_col + col_count
                                                    eerr.err_tx = "invalid e/a format: " + tyear.Substring(4, 1)
                                                    add_error(errors, eerr)
                                                Else
                                                    If tyear.Substring(4, 1) = "A" Then
                                                        oelement.estimate = False
                                                    Else
                                                        oelement.estimate = True
                                                    End If
                                                    Try
                                                        oelement.year = tyear.Substring(0, 4)
                                                    Catch ex As Exception
                                                        eerr = New bc_om_excel_data_element_error
                                                        eerr.worksheet = exceldoc.activesheet.name
                                                        eerr.type = "Cell"
                                                        eerr.row = template.items(i).year_header_row
                                                        eerr.col = template.items(i).start_col + col_count
                                                        eerr.err_tx = "invalid year: " + tyear.Substring(0, 4)
                                                        add_error(errors, eerr)
                                                    End Try
                                                End If
                                            End If
                                        End If
                                        oelement.period = CStr(exceldoc.activesheet.cells(template.items(i).period_header_row, template.items(i).start_col + col_count).value)

                                        If oelement.period = "" Then
                                            eerr = New bc_om_excel_data_element_error
                                            eerr.worksheet = exceldoc.activesheet.name
                                            eerr.type = "Cell"
                                            eerr.row = template.items(i).period_header_row
                                            eerr.col = template.items(i).start_col + col_count
                                            eerr.err_tx = "period type must be entered"
                                            add_error(errors, eerr)
                                        End If

                                        Dim tdate As String

                                        If template.items(i).period_end_item_row > 0 Then
                                            tdate = CStr(exceldoc.activesheet.cells(template.items(i).period_end_item_row, template.items(i).start_col + col_count).value)
                                            If tdate = "" Then
                                                eerr = New bc_om_excel_data_element_error
                                                eerr.worksheet = exceldoc.activesheet.name
                                                eerr.type = "Cell"
                                                eerr.row = template.items(i).period_end_item_row
                                                eerr.col = template.items(i).start_col + col_count
                                                eerr.err_tx = "year end date must be entered "
                                                add_error(errors, eerr)
                                            Else
                                                Try
                                                    oelement.period_end_date = CDate(tdate)
                                                Catch
                                                    eerr = New bc_om_excel_data_element_error
                                                    eerr.worksheet = exceldoc.activesheet.name
                                                    eerr.type = "Cell"
                                                    eerr.row = template.items(i).period_end_item_row
                                                    eerr.col = template.items(i).start_col + col_count
                                                    eerr.err_tx = "invalid year end date: " + tdate
                                                    add_error(errors, eerr)
                                                End Try
                                            End If
                                        End If
                                    End If
                                ElseIf oelement.submission_code = 2 Then
                                    REM date if required
                                    If template.items(i).date_col <> 0 Then
                                        Try
                                            oelement.date_from = CStr(exceldoc.activesheet.cells(row, template.items(i).date_col).value)
                                        Catch
                                            eerr = New bc_om_excel_data_element_error
                                            eerr.worksheet = exceldoc.activesheet.name
                                            eerr.type = "Cell"
                                            eerr.row = row
                                            eerr.col = template.items(i).date_col
                                            eerr.err_tx = "invalid date format"
                                            add_error(errors, eerr)
                                        End Try
                                    Else
                                        eerr = New bc_om_excel_data_element_error
                                        eerr.worksheet = exceldoc.activesheet.name
                                        eerr.type = "Cell"
                                        eerr.row = row
                                        eerr.col = template.items(i).start_col
                                        eerr.err_tx = "no date supplied for item in time series set"
                                        add_error(errors, eerr)
                                        read_io_data = False
                                    End If
                                End If
                                If end_col = False Then
                                    err_tx = client_side_validate(oelement.value, template.items(i).mandatory, template.items(i).datatype, oelement.entity_key, template.items(i).item_display_name)
                                    If err_tx = "" Then

                                        oelements.elements.Add(oelement)
                                    Else
                                        eerr = New bc_om_excel_data_element_error
                                        eerr.worksheet = exceldoc.activesheet.name
                                        eerr.row = row
                                        eerr.col = template.items(i).start_col + col_count

                                        eerr.err_tx = err_tx
                                        add_error(errors, eerr)
                                        read_io_data = False
                                    End If
                                    col_count = col_count + 1
                                End If
                                If oelement.submission_code > 0 Then
                                    end_col = True
                                End If
                            End While
                        Next
                    End If

                    row = row + 1
                    If row = 5000 And template.list_end_delimiter <> "" Then
                        eerr = New bc_om_excel_data_element_error
                        eerr.worksheet = exceldoc.activesheet.name
                        eerr.type = "Cell"
                        eerr.row = template.list_start_row
                        eerr.col = template.list_start_col
                        eerr.err_tx = "no end delimiter found on column:" + template.list_end_delimiter
                        add_error(errors, eerr)
                        read_io_data = False
                        Exit Function
                    End If
                End While
                REM new nov 2 2012 SW/PR 

            ElseIf template.io_mode = "period time series" Then
                row = template.list_start_row
                col = template.list_start_col
                Dim end_col As Boolean
                Dim col_count As Integer

                REM set Period sheet
                REM exceldoc.application.ActiveWorkbook.sheets(3).select()
                REM bexceldoc.application.ActiveWorkbook.Worksheets.add().name = CorpAction.Adjustments.eventactions(i).adjustshort
                REM exceldoc.ActiveSheet.Next.Select()

                exceldoc.application.ActiveWorkbook.sheets(template.items(0).item_display_name).select()


                Dim offset_identifier As String
                Dim offset_row_max As Integer
                Dim offset_col As Integer
                Dim offset_row As Long

                offset_identifier = template.offset_identifier
                offset_row_max = template.offset_row_max
                offset_col = template.offset_col
                offset_row = 0
                row = 1
                While row <= offset_row_max
                    If CStr(exceldoc.activesheet.cells(row, offset_col).value) = offset_identifier Then
                        offset_row = row
                        Exit While
                    End If
                    row = row + 1
                End While

                endBlockColumn = GetEndColumn(offset_row, 1)
                col_count = 0
                For x = 1 To endBlockColumn Step 2
                    row = offset_row + 3
                    end_col = False
                    col_count = col_count + 1
                    While end_col = False

                        If CStr(exceldoc.activesheet.cells(row, x + 1).value) = "" Then
                            end_col = True
                            Exit While
                        End If

                        REM validate period data
                        If CStr(exceldoc.activesheet.cells(row, x + 1).value) < 0 Then
                            eerr = New bc_om_excel_data_element_error
                            eerr.worksheet = exceldoc.activesheet.name
                            eerr.type = "Cell"
                            eerr.row = row
                            eerr.col = x
                            eerr.err_tx = "Can not submit period data. Data Error in: " + template.items(0).item_display_name
                            add_error(errors, eerr)
                            read_io_data = False
                            Exit Function
                        End If

                        REM entity
                        oelement = New bc_om_excel_data_element
                        oelement.entity_row = 1
                        oelement.entity_col = 1
                        oelement.entity_key = exceldoc.activesheet.cells(1, 1).value
                        oelement.entity_key_name = template.entity_identifier

                        REM value
                        oelement.value = CStr(exceldoc.activesheet.cells(row, x + 1).value)
                        oelement.submission_code = template.items(0).submission_code
                        oelement.contributor = template.items(0).contributor
                        oelement.copy_to_draft = template.items(0).copy_to_draft
                        oelement.row = row
                        oelement.col = col_count

                        REM Item
                        oelement.period_row = offset_row + 2
                        oelement.period_end_item_key = template.items(0).period_end_item_key
                        oelement.period_end_item_row = template.items(0).period_end_item_row
                        oelement.item_key = template.items(0).item_identifier

                        If CStr(exceldoc.activesheet.cells(offset_row + 1, x + 1).value) = "A" Then
                            oelement.estimate = False
                        Else
                            oelement.estimate = True
                        End If
                        oelement.year = CStr(exceldoc.activesheet.cells(offset_row, x + 1).value)
                        oelement.period = CStr(exceldoc.activesheet.cells(offset_row + 2, x + 1).value)

                        oelement.date_from = CStr(exceldoc.activesheet.cells(row, x).value)

                        oelements.elements.Add(oelement)
                        row = row + 1

                    End While
                Next

            End If

        Catch ex As Exception
            read_io_data = False
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "read_io_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try

    End Function
    Private Sub add_error(ByRef errors As bc_om_excel_data_element_errors, ByRef err As bc_om_excel_data_element_error)
        Dim found As Boolean = False
        For i = 0 To errors.errors.Count - 1
            If errors.errors(i).row = err.row And errors.errors(i).col = err.col And errors.errors(i).err_tx = err.err_tx Then
                found = True
                Exit Sub
            End If
        Next
        If found = False Then
            errors.errors.Add(err)
        End If
    End Sub
    Private Function client_side_validate(ByVal value As String, ByVal mandatory As Boolean, ByVal datatype As Integer, ByVal entity_key As String, ByVal item_key As String) As String
        client_side_validate = ""
        If mandatory = True And Trim(value) = "" Then
            client_side_validate = item_key + "  must be entered for " + entity_key
        ElseIf Trim(value) <> "" Then
            Select Case datatype
                Case 0
                    If Not IsNumeric(value) Then
                        client_side_validate = item_key + "  must be numeric for " + entity_key
                    End If
                Case 1
                    If Not IsDate(value) Then
                        client_side_validate = item_key + "  must be datetime for " + entity_key
                    End If
                Case 3
                    Dim t As Boolean
                    Try
                        t = CBool(value)
                    Catch
                        client_side_validate = item_key + "  must be true/false for " + entity_key
                    End Try
            End Select
        End If
    End Function
    Public Sub goto_cell(ByVal worksheet As String, ByVal row As Integer, ByVal col As Integer)

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try

            For i = 1 To exceldoc.worksheets.count
                If exceldoc.worksheets(i).name = worksheet Then
                    exceldoc.worksheets(i).select()
                    exceldoc.activesheet.cells(row, col).select()
                    Exit For
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_ao_in_excel", "goto_cell", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI

        End Try
    End Sub
    Public Overrides Sub set_items()
        'exceldoc.Workbooks(1).Worksheets(1).cells(1, 1) = "KK"
    End Sub
    Public Overrides Sub get_cell_formulas(ByVal with_header As Boolean)
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "get_cell_formulas", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim i, j, l, k, m As Integer
        Dim h As String
        Dim ignore_sheet As Boolean
        Dim r As Object
        Dim ocommentary As bc_cs_activity_log
        Dim oldCI As System.Globalization.CultureInfo = _
                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")


        Try
            Dim visible As Boolean = True
            Dim was_protected As Boolean

            link_error = False
            For l = 1 To exceldoc.Worksheets.Count
                visible = True
                was_protected = False

                If exceldoc.Worksheets(l).Visible = 0 Then
                    exceldoc.Worksheets(l).Visible = -1
                    visible = False
                End If
                Try
                    exceldoc.Worksheets(l).Select()
                    With exceldoc.Worksheets(l)

                        .Cells(1, 1).Activate()
                        ignore_sheet = False
                        h = .Cells(1, 1).value
                        If Len(h) > 7 Then
                            If h.Substring(0, 7) = "BCSHEET" Or h.Substring(0, 5) = "BC_IC" Then
                                ignore_sheet = True
                            End If
                        End If
                        If Len(h) = 15 Then
                            If h.Substring(0, 15) = "Preview BCSHEET" Then
                                ignore_sheet = True
                            End If
                        End If
                        If Len(h) = 5 Then
                            If h = "BC_IC" Then
                                ignore_sheet = True
                            End If
                        End If

                        REM check for exlcuded sheets
                        If bc_am_insight_link_names.use_include_sheets = True Then
                            If ignore_sheet = False Then
                                ignore_sheet = True
                                For m = 0 To bc_am_insight_link_names.include_sheets.Count - 1
                                    If .name = bc_am_insight_link_names.include_sheets(m) Then
                                        ignore_sheet = False
                                        ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "get_cell_formulas", bc_cs_activity_codes.COMMENTARY, "Worksheet: " + .name + " included")
                                    End If
                                Next
                            End If
                        Else
                            If ignore_sheet = False Then
                                For m = 0 To bc_am_insight_link_names.exclude_sheets.Count - 1
                                    If .name = bc_am_insight_link_names.exclude_sheets(m) Then
                                        ignore_sheet = True
                                        ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "get_cell_formulas", bc_cs_activity_codes.COMMENTARY, "Worksheet: " + .name + " ignored")
                                    End If
                                Next
                            End If
                        End If
                        If ignore_sheet = False Then
                            'PR 1-11-2007 unhide hoden rowsames.hide_cells = "1" Then
                            'LS 14-10-2009 can now find within hidden cells so no need to unprotect or unhide
                            'Try
                            '    If bc_am_insight_link_names.hide_cells = "1" Then
                            '        If exceldoc.Worksheets(l).ProtectContents = True Then
                            '            exceldoc.Worksheets(l).unprotect()
                            '            was_protected = True
                            '        End If
                            '        exceldoc.Worksheets(l).Range("A1:A1000").Select()
                            '        If exceldoc.Application.Selection.EntireRow.Hidden = True Then
                            '            exceldoc.Application.Selection.EntireRow.Hidden = False
                            '            exceldoc.Worksheets(l).Range("A1:A2").Select()
                            '            exceldoc.Application.Selection.EntireColumn.Hidden = False
                            '            exceldoc.Application.Selection.ColumnWidth = 0.08
                            '            exceldoc.Worksheets(l).Range("B1:B1000").Select()
                            '            exceldoc.Application.Selection.EntireRow.Hidden = False
                            '            exceldoc.Worksheets(l).Range("B1:B2").Select()
                            '            exceldoc.Application.Selection.EntireColumn.Hidden = False
                            '            exceldoc.Application.Selection.ColumnWidth = 0.08
                            '        End If
                            '        exceldoc.Worksheets(l).Range("A1:A1000").Select()
                            '        If exceldoc.Application.Selection.Entirecolumn.Hidden = True Then
                            '            exceldoc.Worksheets(l).Range("A1:A1000").Select()
                            '            exceldoc.Application.Selection.EntireRow.Hidden = False
                            '            exceldoc.Worksheets(l).Range("A1:A2").Select()
                            '            exceldoc.Application.Selection.EntireColumn.Hidden = False
                            '        End If
                            '    End If
                            'Catch ex As Exception
                            '    Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "get_cell_formulas", bc_cs_error_codes.USER_DEFINED, ex.Message)
                            'End Try

                            r = exceldoc.Worksheets(l).cells.find(bc_am_insight_link_names.year_head, LookAt:=1, LookIn:=-4123) 'look in formulas
                            'PR fix back so formulas work in link codes
                            'Dim bhidden As Boolean
                            'Dim bprotected As Boolean




                            If Not IsNothing(r) Then

                                'bhidden = exceldoc.Worksheets(l).Columns(1).Hidden
                                'bprotected = exceldoc.Worksheets(l).ProtectContents
                                'activesheet
                                'If bprotected = True Then
                                ' exceldoc.Worksheets(l).Unprotect()
                                'End If
                                'If bhidden = True Then
                                'exceldoc.Worksheets(l).Columns(1).Hidden = False
                                'End If
                                For i = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Count - 1
                                    REM period
                                    For j = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections.count - 1
                                        With bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections(j)
                                            For k = 0 To .rows.count - 1
                                                'If .rows(k).link_found = False Then
                                                link(exceldoc.Worksheets(l), .rows(k), with_header)
                                                'End If
                                            Next
                                        End With
                                    Next
                                Next
                                'If bhidden = True Then
                                ' exceldoc.Worksheets(l).Columns(1).Hidden = True
                                'End If

                                'If bprotected = True Then
                                'exceldoc.Worksheets(l).Protect()
                                'End If



                            End If
                        End If
                    End With
                    If visible = False Then
                        exceldoc.Worksheets(l).Visible = 0
                    End If
                    'If was_protected = True Then
                    '    exceldoc.Worksheets(l).protect(DrawingObjects:=False, Contents:=True, Scenarios:=True)
                    'End If
                Catch ex As Exception
                    ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "get_cell_formulas", bc_cs_activity_codes.COMMENTARY, "Worksheet failed to select: " + exceldoc.Worksheets(l).name)
                End Try
            Next
            If link_error = True Then
                Dim omessage As New bc_cs_message("Blue Curve - insight", "There were errors linking please check log file", bc_cs_message.MESSAGE)
                link_error = False
            End If

        Catch ex As Exception
            ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "get_cell_formulas", bc_cs_activity_codes.COMMENTARY, "Worksheet failed to select: " + exceldoc.Worksheets(l).name)


            'MsgBox(exceldoc.Worksheets(l).name)
            'Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "get_cell_formulas", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "get_cell_formulas", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Sub get_cell_formulas_static()
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "get_cell_formulas_static", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim i, j, l, k, m As Integer
        Dim h As String
        Dim ignore_sheet As Boolean
        Dim r As Object
        Dim ocommentary As bc_cs_activity_log

        Dim oldCI As System.Globalization.CultureInfo = _
                                        System.Threading.Thread.CurrentThread.CurrentCulture
        Try


            System.Threading.Thread.CurrentThread.CurrentCulture = _
                      New System.Globalization.CultureInfo("en-GB")

            Dim visible As Boolean = True
            For l = 1 To exceldoc.Worksheets.Count
                visible = True
                If exceldoc.Worksheets(l).Visible = 0 Then
                    exceldoc.Worksheets(l).Visible = -1
                    visible = False
                End If
                Try
                    exceldoc.Worksheets(l).Select()
                    With exceldoc.Worksheets(l)
                        .Cells(1, 1).Activate()
                        ignore_sheet = False
                        h = .Cells(1, 1).value
                        If Len(h) > 7 Then
                            If h.Substring(0, 7) = "BCSHEET" Then
                                ignore_sheet = True
                            End If
                        End If
                        If Len(h) > 15 Then
                            If h.Substring(0, 15) = "Preview BCSHEET" Then
                                ignore_sheet = True
                            End If
                        End If
                        If Len(h) = 5 Then
                            If h = "BC_IC" Then
                                ignore_sheet = True
                            End If
                        End If

                        REM check for exlcuded sheets
                        If bc_am_insight_link_names.use_include_sheets = True Then
                            If ignore_sheet = False Then
                                ignore_sheet = True
                                For m = 0 To bc_am_insight_link_names.include_sheets.Count - 1
                                    If .name = bc_am_insight_link_names.include_sheets(m) Then
                                        ignore_sheet = False
                                        ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "get_cell_formulas", bc_cs_activity_codes.COMMENTARY, "Worksheet: " + .name + " included")
                                    End If
                                Next
                            End If
                        Else
                            If ignore_sheet = False Then
                                For m = 0 To bc_am_insight_link_names.exclude_sheets.Count - 1
                                    If .name = bc_am_insight_link_names.exclude_sheets(m) Then
                                        ignore_sheet = True
                                        ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "get_cell_formulas", bc_cs_activity_codes.COMMENTARY, "Worksheet: " + .name + " ignored")
                                    End If
                                Next
                            End If
                        End If

                        If ignore_sheet = False Then

                            r = exceldoc.Worksheets(l).cells.find(bc_am_insight_link_names.static_head, LookAt:=1, LookIn:=-4123) 'look in formulas

                            If Not IsNothing(r) Then
                                For i = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Count - 1
                                    REM period
                                    For j = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections_static.count - 1
                                        With bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections_static(j)
                                            For k = 0 To .rows.count - 1
                                                If .rows(k).link_found = False And .rows(k).link_code <> "" Then
                                                    link_static(exceldoc.Worksheets(l), .rows(k), r.column)
                                                End If
                                            Next
                                        End With
                                    Next
                                Next
                            End If
                        End If
                    End With
                    If visible = False Then
                        exceldoc.Worksheets(l).Visible = 0
                    End If
                Catch ex As Exception
                    ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "get_cell_formulas_static", bc_cs_activity_codes.COMMENTARY, "Worksheet failed to select: " + exceldoc.Worksheets(l).name)
                End Try

            Next

        Catch ex As Exception
            ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "get_cell_formulas_static", bc_cs_activity_codes.COMMENTARY, "Worksheet failed to select: " + exceldoc.Worksheets(l).name)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "get_cell_formulas_static", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub link(ByVal activeworksheet As Object, ByRef row As bc_om_insight_row, ByVal with_header As Boolean)
        'Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "link", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try

            Dim r As Object
            Dim c As Object

            Dim formula As String

            REM scroll threw link codes from object model
            REM =======================

            If row.link_code = "" Then
                Exit Sub
            End If

            Dim range As Object
            REM only look in column A for code later on make this configurable
            range = activeworksheet.range("A:A")
            'r = activeworksheet.Cells.Find(row.link_code, LookAt:=1, LookIn:=-4163)
            'MsgBox(row.link_code)
            r = range.find(row.link_code, LookAt:=1, LookIn:=-4123) ' look in value
            If IsNothing(r) Then
                'MsgBox("yes")
                r = activeworksheet.Cells.Find(row.link_code, LookAt:=1, LookIn:=-4163) 'look in formula
            End If
            'done like this to cater for hidden row but need both if formula is link code
            'r = activeworksheet.Cells.Find(row.link_code, LookAt:=1, LookIn:=-4163)
            REM loop for years attaching formula to object model
            If Not IsNothing(r) Then
                row.link_found = True
                REM if flexible label put in link for label
                If r.row > 1 Then
                    If row.flexible_label_flag = 1 Then
                        c = activeworksheet.Cells.Find(bc_am_insight_link_names.label_head, LookAt:=1, LookIn:=-4123) ' look in formulas

                        formula = "='" + activeworksheet.Name + "'!" + CStr(activeworksheet.Cells(r.Row, c.column).Address)
                        row.label_formula = formula
                    End If
                    find_header_for_value(activeworksheet, r.row, row, row.link_code)
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "link", bc_cs_error_codes.USER_DEFINED, ex.Message + ": " + row.link_code)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            'slog = New bc_cs_activity_log("bc_ao_in_excel", "link", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub link_static(ByVal activeworksheet As Object, ByRef row As bc_om_insight_row, ByVal col As Long)
        'Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "link_static", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim i As Integer
            Dim c As Object
            Dim r As Object
            Dim formula As String = ""
            REM scroll threw link codes from object model
            REM =======================
            For i = 1 To row.repeating_count + 1
                If row.repeating_count > 0 Then
                    r = activeworksheet.Cells.Find(row.link_code + "_" + CStr(i), LookAt:=1, LookIn:=-4123) 'look in formulas
                    If IsNothing(r) Then
                        activeworksheet.Cells.Find(row.link_code + "_" + CStr(i), LookAt:=1, LookIn:=-4163) 'look in formula
                    End If

                Else
                    r = activeworksheet.Cells.Find(row.link_code, LookAt:=1, LookIn:=-4123) 'look in formulas
                    If IsNothing(r) Then
                        activeworksheet.Cells.Find(row.link_code, LookAt:=1, LookIn:=-4163) 'look in formula
                    End If
                End If
                REM loop for years attaching formula to object model
                If Not IsNothing(r) Then
                    'If row.link_code = activeworksheet.cells(r.row, r.column).text  or
                    If row.flexible_label_flag = 1 Then
                        c = activeworksheet.Cells.Find(bc_am_insight_link_names.label_head, LookAt:=1, LookIn:=-4123) ' look in formulas
                        formula = "='" + activeworksheet.Name + "'!" + CStr(activeworksheet.Cells(r.Row, c.column).Address)
                        row.label_formula = formula
                    End If
                    row.link_found = True
                    create_row_value_formula("", "", activeworksheet, r.row, col, row, i - 1, "", "", "", "", "", formula, "")
                    'End If
                End If
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "link_static", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            'slog = New bc_cs_activity_log("bc_ao_in_excel", "link_static", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub find_header_for_value(ByVal activesheet As Object, ByVal value_row As Long, ByRef row As bc_om_insight_row, ByVal link_code As String)
        'Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "find_header_for_value", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim r As Object
        Dim e_a As Object
        Dim p_d As Object
        Dim e_d As Object
        Dim oacc As Object = Nothing
        Dim year As String
        Dim header_row As Integer
        Dim e_a_row As Integer
        Dim p_d_row As Integer
        Dim e_d_row As Integer
        Dim e_a_value As String

        Dim ocommentary As bc_cs_activity_log

        Dim i As Integer
        Const MAX_COLS = 10000

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try

            Dim yd = bc_am_insight_link_names.year_head
            Dim ead = bc_am_insight_link_names.e_a_head
            Dim pd = bc_am_insight_link_names.period_head
            Dim col = bc_am_insight_link_names.start_column
            Dim enable_disable = bc_am_insight_link_names.enable_disable
            Dim concurrent_blanks = bc_am_insight_link_names.concurrent_blanks
            Dim acc = bc_am_insight_link_names.accounting_head
            Dim ped As String
            Dim bc_year As String = ""
            Dim bc_period As String = ""
            Dim blank_col_count As Integer
            Dim bc_e_a_value As String = ""
            Dim year_formula As String
            Dim e_a_value_formula As String
            Dim enable_disable_formula As String
            Dim ped_formula As String
            Dim acc_formula As String = ""
            Dim acc_row As Integer
            Dim bc_acc As String = ""
            Dim link_acc As Boolean
            Dim acc_val_str As String = ""
            Dim per_val_str As String = ""
            acc_row = 0
            header_row = 0
            e_a_row = 0
            p_d_row = 0
            e_d_row = 0
            r = activesheet.cells.find(yd, LookAt:=1, LookIn:=-4123) 'look in formulas

            e_a = activesheet.cells.find(ead, LookAt:=1, LookIn:=-4123) 'look in formulas

            p_d = activesheet.cells.find(pd, LookAt:=1, LookIn:=-4123) 'look in formulas


            If (enable_disable <> "") Then
                e_d = activesheet.cells.find(enable_disable, LookAt:=1, LookIn:=-4123) 'look in formulas
            End If

            If Not IsNothing(r) Then
                header_row = r.row
            End If
            If Not IsNothing(e_a) Then
                e_a_row = e_a.row
            End If
            If Not IsNothing(p_d) Then
                p_d_row = p_d.row()
            End If
            If Not IsNothing(e_d) Then
                e_d_row = e_d.row()
            End If

            REM optional accounting standard
            link_acc = False
            If acc <> "" Then
                oacc = activesheet.cells.find(acc, LookAt:=1, LookIn:=-4123) 'look in formulas                
            End If
            If Not IsNothing(oacc) Then
                acc_row = oacc.row()
                link_acc = True
            End If

            If header_row > 0 And e_a_row > 0 And p_d_row > 0 Then
                REM find first year
                blank_col_count = 0
                For i = col To MAX_COLS
                    Try
                        If CStr(activesheet.cells(header_row, i).value) = "" Then
                            blank_col_count = blank_col_count + 1
                            If blank_col_count = concurrent_blanks Then
                                REM concurent blanks exceeded so assume end of data
                                Exit For
                            End If
                        Else
                            blank_col_count = 0
                            year = CStr(activesheet.cells(header_row, i).value)
                            e_a_value = CStr(activesheet.cells(e_a_row, i).value)
                            ped = CStr(activesheet.cells(p_d_row, i).value)
                            If acc_row > 0 Then
                                acc = CStr(activesheet.cells(acc_row, i).value)
                                acc_formula = "='" + activesheet.Name + "'!" + activesheet.cells(acc_row, i).address
                            End If
                            year_formula = "='" + activesheet.Name + "'!" + activesheet.cells(header_row, i).address
                            e_a_value_formula = "='" + activesheet.Name + "'!" + activesheet.cells(e_a_row, i).address
                            ped_formula = "='" + activesheet.Name + "'!" + activesheet.cells(p_d_row, i).address
                            enable_disable_formula = ""
                            If (e_d_row <> 0) Then
                                enable_disable_formula = "='" + activesheet.Name + "'!" + activesheet.cells(e_d_row, i).address
                            End If

                            REM mark header row as green first
                            If bc_am_insight_link_names.show_header_errors_in_cell = True Then
                                Try
                                    activesheet.Cells(header_row, i).select()
                                    activesheet.application.selection.ClearComments()
                                    activesheet.Cells(e_a_row, i).select()
                                    activesheet.application.selection.ClearComments()
                                    activesheet.Cells(p_d_row, i).select()
                                    activesheet.application.selection.ClearComments()
                                    activesheet.Cells(acc_row, i).select()
                                    activesheet.application.selection.ClearComments()
                                Catch
                                    ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "find_header_for_value", bc_cs_activity_codes.COMMENTARY, "Parse Comments failed Invalid header code ")
                                End Try
                                Dim n As Integer
                                For n = 0 To bc_am_insight_link_names.accounting_values.Count - 1
                                    If n = 0 Then
                                        acc_val_str = bc_am_insight_link_names.accounting_values(n).external_code
                                    Else
                                        acc_val_str = acc_val_str + "; " + bc_am_insight_link_names.accounting_values(n).external_code
                                    End If
                                Next
                                For n = 0 To bc_am_insight_link_names.period_values.Count - 1
                                    If n = 0 Then
                                        per_val_str = bc_am_insight_link_names.period_values(n).external_code
                                    Else
                                        per_val_str = per_val_str + "; " + bc_am_insight_link_names.period_values(n).external_code
                                    End If
                                Next
                            End If
                            Dim pcode As Integer
                            pcode = parse_for_year_and_period(year, ped, bc_year, bc_period, e_a_value, bc_e_a_value, CStr(activesheet.name) + " row: " + CStr(row.excel_row) + "col: " + CStr(i), link_acc, acc, bc_acc)

                            If pcode = 0 Then
                                create_row_value_formula(bc_year + bc_e_a_value, bc_period, activesheet, value_row, i, row, 0, year_formula, e_a_value_formula, ped_formula, bc_acc, acc_formula, "", enable_disable_formula )
                            Else
                                REM now mark header cells that could notbe parsed as red
                                If bc_am_insight_link_names.show_header_errors_in_cell = True Then
                                    REM mark header row as green first
                                    If pcode >= 1000 Then
                                        If p_d_row <> header_row Then
                                            activesheet.Cells(header_row, i).select()
                                            activesheet.application.selection.addcomment("Couldnt parse to Blue Curve format: invalid year")
                                        End If
                                    End If
                                    If pcode = 1 Or pcode = 11 Or pcode = 101 Or pcode = 111 Or pcode = 1001 Or pcode = 1011 Or pcode = 1111 Or pcode = 1101 Then
                                        If p_d_row <> header_row Then
                                            'activesheet.Cells(header_row, i).select()
                                            'activesheet.application.selection.addcomment("Couldnt parse to Blue Curve format: invalid year")
                                            activesheet.Cells(p_d_row, i).select()
                                            activesheet.application.selection.addcomment("Couldnt parse to Blue Curve format: valid values are: " + per_val_str)
                                        Else
                                            activesheet.Cells(p_d_row, i).select()
                                            activesheet.application.selection.addcomment("Couldnt parse to Blue Curve format: valid values are: " + per_val_str)
                                        End If
                                    End If
                                    If pcode = 10 Or pcode = 11 Or pcode = 110 Or pcode = 111 Or pcode = 1010 Or pcode = 1011 Or pcode = 1111 Or pcode = 1110 Then
                                        activesheet.Cells(e_a_row, i).select()
                                        activesheet.application.selection.addcomment("Couldnt Parse to Blue Curve format: valid values are: " + bc_am_insight_link_names.actual_prefix + " or " + bc_am_insight_link_names.estimate_prefix)
                                    End If
                                    If pcode = 100 Or pcode = 101 Or pcode = 110 Or pcode = 111 Or pcode = 1111 Or pcode = 1100 Or pcode = 1110 Or pcode = 1101 Then
                                        If acc_row > 0 Then
                                            activesheet.Cells(acc_row, i).select()
                                            activesheet.application.selection.addcomment("Couldnt Parse to Blue Curve format: valid values are: " + acc_val_str)
                                        End If
                                    End If
                                End If
                            End If
                            End If

                    Catch ex As Exception
                        ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "find_header_for_value", bc_cs_activity_codes.COMMENTARY, "Link Error: Period link code: " + CStr(pd) + " not found on sheet for link code. " + CStr(activesheet.name) + ",code:" + link_code + ", row: " + CStr(row.excel_row) + ": " + Err.Description)
                    End Try
                Next
            Else
                If header_row = 0 Then
                    link_error = True
                    ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "find_header_for_value", bc_cs_activity_codes.COMMENTARY, "Link Error: Year link code: " + CStr(yd) + " not found on sheet for link code." + CStr(activesheet.name) + ",code:" + link_code + ", row: " + CStr(row.excel_row))
                End If
                If e_a_row = 0 Then
                    link_error = True
                    ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "find_header_for_value", bc_cs_activity_codes.COMMENTARY, "Link Error: E/A link code: " + CStr(ead) + " not found on sheet for link code." + CStr(activesheet.name) + ",code:" + link_code + ", row: " + CStr(row.excel_row))
                End If
                If p_d_row = 0 Then
                    link_error = True
                    ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "find_header_for_value", bc_cs_activity_codes.COMMENTARY, "Link Error: Period link code: " + CStr(pd) + " not found on sheet for link code." + CStr(activesheet.name) + ",code:" + link_code + ", row: " + CStr(row.excel_row))
                End If
            End If
            If acc_row = 0 Then
                ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "find_header_for_value", bc_cs_activity_codes.COMMENTARY, "Didnt Link Accounting Standard as header code invalid" + CStr(acc) + " not found on sheet for link code." + CStr(activesheet.name) + ",code:" + link_code + ", row: " + CStr(row.excel_row))
            End If

        Catch ex As Exception
            Dim otext As String
            otext = "Link Error header format not supported: sheet:" + CStr(activesheet.name) + ", row: " + CStr(row.excel_row) + " column: " + CStr(i)
            Dim omessage As New bc_cs_message("Blue Curve insight", otext, bc_cs_message.MESSAGE)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            'slog = New bc_cs_activity_log("bc_ao_in_excel", "find_header_for_value", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Function parse_for_year_and_period(ByVal input_year As String, ByVal input_period As String, ByRef output_year As String, ByRef output_period As String, ByVal input_e_a As String, ByRef bc_e_a_value As String, ByVal worksheet_params As String, ByVal link_acc As Boolean, ByVal input_acc As String, ByRef output_acc As String) As Integer
        'Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "parse_for_year_and_period", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim failed As Boolean
        Dim bc_code As String
        Dim h, m As Integer
        Dim oyear As String
        Dim i As Integer
        Dim found = True
        Dim e_a_failed As Boolean
        Dim pcode As Integer

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim ocommentary As bc_cs_activity_log

            parse_for_year_and_period = 1
            pcode = 0
            If input_year = input_period Then
                For h = 0 To bc_am_insight_link_names.period_values.Count - 1
                    failed = True
                    If Len(input_year) = Len(bc_am_insight_link_names.period_values(h).external_code) Then
                        With bc_am_insight_link_names.period_values(h)
                            failed = True
                            oyear = ""
                            For i = 0 To Len(.external_code) - 1
                                If input_year.Substring(i, 1) = .external_code.Substring(i, 1) Or (.external_code.Substring(i, 1) = "Y" And IsNumeric(input_year.Substring(i, 1))) Then
                                    If .external_code.Substring(i, 1) = "Y" Then
                                        If i < .external_code.length - 1 Then
                                            If .external_code.Substring(i + 1, 1) = "Y" Then
                                                oyear = oyear + input_year.Substring(i, 1)
                                            ElseIf i > 0 Then
                                                If .external_code.Substring(i - 1, 1) = "Y" Then
                                                    oyear = oyear + input_year.Substring(i, 1)
                                                End If
                                            End If
                                        Else
                                            oyear = oyear + input_year.Substring(i, 1)
                                        End If
                                    End If
                                    If i = Len(.external_code) - 1 Then
                                        'failed = False
                                        If Len(oyear) = 2 Then
                                            REM year isnt full
                                            If oyear > 50 Then
                                                oyear = "19" + oyear
                                            Else
                                                oyear = "20" + oyear
                                            End If
                                        End If
                                        output_year = oyear
                                        output_period = .bc_code
                                        pcode = 0
                                        failed = False
                                        Exit For
                                    End If
                                    REM PR
                                Else
                                    Exit For
                                End If
                            Next
                        End With
                    End If
                    If failed = False Then
                        Exit For
                    End If
                Next
                If failed = True Then
                    pcode = 1
                    'ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "parse_for_year_and_period", bc_activity_codes.COMMENTARY, "Couldn't parse year:" + CStr(input_year) + " for: " + CStr(worksheet_params))
                End If
                If failed = False Then
                    pcode = 0
                    If Len(output_year) = 3 Then
                        output_year = output_year.Substring(1, 2)
                        bc_code = bc_am_insight_link_names.period_values(h).bc_code
                    End If
                End If
                e_a_failed = False
                failed = False
                If failed = False Then
                    failed = True
                    If input_e_a = bc_am_insight_link_names.actual_prefix Then
                        bc_e_a_value = "A"
                        If link_acc = False Then
                            failed = False
                        End If
                    ElseIf input_e_a = bc_am_insight_link_names.estimate_prefix Then
                        bc_e_a_value = "E"
                        If link_acc = False Then
                            failed = False
                        End If
                    Else
                        'ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "parse_for_year_and_period", bc_activity_codes.COMMENTARY, "Couldn't parse e_a value:" + CStr(input_e_a) + " for: " + CStr(worksheet_params))
                        failed = True
                        e_a_failed = True
                        REM e_a failed
                        pcode = pcode + 10
                    End If
                    REM here
                    e_a_failed = False
                    If link_acc = True And e_a_failed = False Then
                        failed = True
                        For m = 0 To bc_am_insight_link_names.accounting_values.Count - 1
                            With bc_am_insight_link_names.accounting_values(m)
                                If .external_code = input_acc Then
                                    output_acc = .bc_code
                                    failed = False
                                End If
                            End With
                        Next
                        If failed = True Then
                            pcode = pcode + 100
                            'ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "parse_for_year_and_period", bc_activity_codes.COMMENTARY, "Couldn't parse accounting code:" + CStr(input_acc) + " for: " + CStr(worksheet_params))
                            Exit Function
                        End If
                    End If
                End If

            Else
                failed = True
                pcode = 1
                If IsNumeric(input_year) = False Or input_year.Length <> 4 Then
                    pcode = pcode + 1000
                Else
                    output_year = input_year
                End If
                REM parse for  period
                e_a_failed = False
                For h = 0 To bc_am_insight_link_names.period_values.Count - 1
                    With bc_am_insight_link_names.period_values(h)
                        If .external_code = input_period Then
                            output_period = .bc_code
                            pcode = pcode - 1
                            Exit For
                        End If
                    End With
                Next
                If input_e_a = bc_am_insight_link_names.actual_prefix Then
                    bc_e_a_value = "A"
                    If link_acc = False Then
                        failed = False
                    End If
                ElseIf input_e_a = bc_am_insight_link_names.estimate_prefix Then
                    bc_e_a_value = "E"
                    If link_acc = False Then
                        failed = False
                    End If
                Else
                    e_a_failed = True
                    pcode = pcode + 10
                    ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "parse_for_year_and_period", bc_cs_activity_codes.COMMENTARY, "Couldn't parse e_a value:" + CStr(input_e_a) + " for: " + CStr(worksheet_params))
                End If
                REM accouting standard
                If link_acc = True Then
                    failed = True
                    For m = 0 To bc_am_insight_link_names.accounting_values.Count - 1
                        With bc_am_insight_link_names.accounting_values(m)
                            If .external_code = input_acc Then
                                output_acc = .bc_code
                                parse_for_year_and_period = 0
                                failed = False
                            End If
                        End With
                    Next
                    If failed = True Then
                        pcode = pcode + 100
                        ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "parse_for_year_and_period", bc_cs_activity_codes.COMMENTARY, "Couldn't parse accounting code:" + CStr(input_acc) + " for: " + CStr(worksheet_params))
                        Exit Function
                    End If
                End If
            End If
        Catch ex As Exception
            link_error = True
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "parse_for_year_and_period", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            If pcode > 0 Then
                parse_for_year_and_period = pcode
                Dim ocommentary As New bc_cs_activity_log("bc_ao_in_excel", "parse_for_year_and_period", bc_cs_activity_codes.COMMENTARY, "Couldn't parse header year: " + CStr(input_year) + " period: " + CStr(input_period) + " e_a_prefix: " + CStr(input_e_a) + " Accounting code: " + CStr(input_acc) + " for: " + CStr(worksheet_params) + " Code:" + CStr(pcode))
            End If
            parse_for_year_and_period = pcode
            'slog = New bc_cs_activity_log("bc_ao_in_excel", "parse_for_year_and_period", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Private Sub create_row_value_formula(ByVal year As String, ByVal period_id As String, ByVal activesheet As Object, ByVal value_row As Integer, ByVal c As Integer, ByRef row As bc_om_insight_row, ByVal ord As Integer, ByVal year_formula As String, ByVal e_a_formula As String, ByVal ped_formula As String, ByVal bc_acc As String, ByVal acc_formula As String, ByVal label_formula As String, enable_disable_formula As String)
        'Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "create_row_value_formula", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim row_value As New bc_om_insight_rows_cell_value
            row_value.year = year
            row_value.period_id = period_id
            row_value.accounting_standard = bc_acc
            row_value.formula = "=if(isblank('" + activesheet.Name + "'!" + CStr(activesheet.Cells(value_row, c).Address) + "),"""",'" + activesheet.Name + "'!" + CStr(activesheet.Cells(value_row, c).Address) + ")"
            row_value.label_formula = label_formula
            'row_value.formula = "='" + activesheet.Name + "'!" + CStr(activesheet.Cells(value_row, c).Address)
            row_value.order = ord
            If bc_am_insight_formats.linked_Header = "1" Then
                row_value.year_formula = year_formula
                row_value.e_a_formula = e_a_formula
                row_value.period_id_formula = ped_formula
                row_value.acc_formula = acc_formula
                row_value.enable_disable_formula = enable_disable_formula
            End If
            row.values_list.Add(row_value)


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "create_row_value_formula", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            'slog = New bc_cs_activity_log("bc_ao_in_excel", "create_row_value_formula", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Sub write_out_formula(ByVal with_header As Boolean)
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "write_out_formula", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                              System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Dim i As Integer
            Dim j As Integer
            Dim k As Integer
            Dim l As Integer
            Dim m As Integer
            Dim p As Integer
            Dim tmp_headers As New tmp_period_headers
            Dim tmp_header As tmp_period_header

            Dim col_found As Integer
            REM write out formulas to BC Sheets
            show_worksheets()
            For i = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Count - 1
                REM select sheet
                For m = 1 To exceldoc.worksheets.count
                    If exceldoc.worksheets(m).cells(1, 1).value = bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).sheet_name Then
                        exceldoc.worksheets(m).select()
                        unprotect_sheet()
                        Exit For
                    End If
                Next
                REM period firstly get unique headers and sort
                For j = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections.count - 1
                    With bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections(j)
                        For k = 0 To .rows.count - 1
                            If .rows(k).link_found = True Then
                                REM search for code in insight sheet
                                REM put in link formula
                                For l = 0 To .rows(k).values_list.count - 1
                                    'PR not sure wot this bit does need to check
                                    'PR fix to sort headers
                                    tmp_header = New tmp_period_header(.rows(k).values_list(l).year, .rows(k).values_list(l).period_id, .rows(k).values_list(l).accounting_standard, .rows(k).values_list(l).year_formula, .rows(k).values_list(l).e_a_formula, .rows(k).values_list(l).period_id_formula, .rows(k).values_list(l).acc_formula, tmp_headers, .rows(k).values_list(l).enable_disable_formula)
                                Next
                            End If
                        Next
                    End With
                Next
                tmp_headers.sort()
                REM write out headers in correct order
                For j = 0 To tmp_headers.headers.Count - 1
                    With tmp_headers.headers(j)
                        exceldoc.worksheets(m).cells(7, j + 11) = .year
                        exceldoc.worksheets(m).cells(8, j + 11) = .period
                        If .acc <> "" Then
                            exceldoc.worksheets(m).cells(6, j + 11) = .acc
                        End If
                        REM add drop down for disabling column submission
                        'With exceldoc.worksheets(m).cells(5, j + 11).Validation
                        '    .Delete()
                        '    .Add(Type:=3, AlertStyle:=1, Operator:=1, Formula1:="enabled,disabled")
                        '    .IgnoreBlank = True
                        '    .InCellDropdown = True
                        '    .InputTitle = ""
                        '    .ErrorTitle = ""
                        '    .InputMessage = ""
                        '    .ErrorMessage = ""
                        '    .ShowInput = True
                        '    .ShowError = True
                        'End With
                        'exceldoc.worksheets(m).cells(5, j + 11) = "enabled"
                        exceldoc.worksheets(m).cells(5, j + 11) = .enable_disable_formula
                        REM linked headers
                        If bc_am_insight_formats.linked_Header = "1" Then
                            exceldoc.worksheets(m).cells(2, j + 11).formula = .year_formula
                            exceldoc.worksheets(m).cells(3, j + 11).formula = .period_formula
                            exceldoc.worksheets(m).cells(4, j + 11).formula = .e_a_formula
                            If .acc <> "" Then
                                exceldoc.worksheets(m).cells(1, j + 11) = .acc_formula
                            End If
                        End If
                    End With
                Next

                REM now write out data assigning to correct header
                For j = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections.count - 1
                    With bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections(j)
                        For k = 0 To .rows.count - 1
                            If .rows(k).link_found = True Then
                                REM search for code in insight sheet
                                REM put in link formula
                                For l = 0 To .rows(k).values_list.count - 1
                                    REM find header position
                                    col_found = 0
                                    For p = 0 To tmp_headers.headers.Count - 1
                                        If tmp_headers.headers(p).year = .rows(k).values_list(l).year And tmp_headers.headers(p).period = .rows(k).values_list(l).period_id Then
                                            If tmp_headers.headers(p).acc <> "" And tmp_headers.headers(p).acc = .rows(k).values_list(l).accounting_standard Then
                                                col_found = p + 11
                                                Exit For
                                            ElseIf (tmp_headers.headers(p).acc = "") Then
                                                col_found = p + 11
                                                Exit For
                                            End If
                                        End If
                                    Next
                                    If col_found <> 0 Then
                                        Try
                                            exceldoc.worksheets(m).cells(.rows(k).excel_row, col_found).formula = .rows(k).values_list(l).formula
                                        Catch
                                            Dim kk As Integer
                                            Dim formula As String
                                            Dim ocommentary As New bc_cs_activity_log("bc_ao_in_excel", "write_out_formula", bc_cs_activity_codes.COMMENTARY, "Formula not valid format: " + .rows(k).values_list(l).formula + " " + CStr(m) + ":" + CStr(.rows(k).excel_row) + ":" + CStr(col_found))
                                            formula = .rows(k).values_list(l).formula
                                            kk = InStr(1, formula, """")
                                            If kk > 0 Then
                                                formula = Right(formula, Len(formula) - kk - 2)
                                                formula = Left(formula, Len(formula) - 1)
                                            End If
                                            Try
                                                exceldoc.worksheets(m).cells(.rows(k).excel_row, col_found).formula = "=" + formula
                                            Catch
                                                ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "write_out_formula", bc_cs_activity_codes.COMMENTARY, "Formula not valid format: " + formula)
                                            End Try
                                        End Try
                                    End If
                                Next
                                If .rows(k).label_formula <> "" Then
                                    exceldoc.worksheets(m).cells(.rows(k).excel_row, 9).formula = .rows(k).label_formula
                                End If
                            End If
                        Next
                    End With
                Next
                REM static
                For j = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections_static.count - 1
                    With bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections_static(j)
                        For k = 0 To .rows.count - 1
                            If .rows(k).link_found = True And .rows(k).link_code <> "" Then
                                REM search for code in insight sheet
                                If .rows(k).repeating_count = 0 Then
                                    exceldoc.worksheets(m).cells(.rows(k).excel_row, 7).formula = .rows(k).values_list(0).formula
                                    If .rows(k).label_formula <> "" Then
                                        exceldoc.worksheets(m).cells(.rows(k).excel_row, 5).formula = .rows(k).label_formula
                                    End If
                                Else
                                    For l = 0 To .rows(k).repeating_count
                                        exceldoc.worksheets(m).cells(.rows(k).excel_row - .rows(k).repeating_Count + l, 7).formula = .rows(k).values_list(l).formula
                                        If .rows(k).values_list(l).label_formula <> "" Then
                                            exceldoc.worksheets(m).cells(.rows(k).excel_row - .rows(k).repeating_Count + l, 5).formula = .rows(k).values_list(l).label_formula
                                        End If
                                    Next
                                End If
                            End If
                        Next
                    End With
                Next

                protect_sheet()
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "write_out_formula", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            hide_worksheets()
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "write_out_formula", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub write_out_formula_preview(ByVal with_header As Boolean)
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "write_out_formula", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                              System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")
        Try
            Dim i As Integer
            Dim j As Integer
            Dim k As Integer
            Dim l As Integer
            Dim m As Integer
            Dim p As Integer
            Dim tmp_headers As New tmp_period_headers
            Dim tmp_header As tmp_period_header
            Dim col_found As Integer
            show_worksheets()
            REM write out formulas to BC Sheets
            For i = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Count - 1
                REM select sheet
                For m = 1 To exceldoc.worksheets.count
                    If exceldoc.worksheets(m).cells(1, 1).value = bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).sheet_name Then
                        exceldoc.worksheets(m).select()
                        unprotect_sheet()
                        Exit For
                    End If
                Next
                REM period firstly get unique headers and sort
                For j = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections.count - 1
                    With bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections(j)
                        For k = 0 To .rows.count - 1
                            If .rows(k).link_found = True Then
                                REM search for code in insight sheet
                                REM put in link formula
                                For l = 0 To .rows(k).values_list.count - 1
                                    'PR not sure wot this bit does need to check
                                    'PR fix to sort headers
                                    tmp_header = New tmp_period_header(.rows(k).values_list(l).year, .rows(k).values_list(l).period_id, .rows(k).values_list(l).accounting_standard, .rows(k).values_list(l).year_formula, .rows(k).values_list(l).e_a_formula, .rows(k).values_list(l).period_id_formula, .rows(k).values_list(l).acc_formula, tmp_headers, .rows(k).values_list(l).enable_disable_formula)
                                Next
                            End If
                        Next
                    End With
                Next
                tmp_headers.sort()
                REM write out headers in correct order
                For j = 0 To tmp_headers.headers.Count - 1
                    With tmp_headers.headers(j)
                        exceldoc.worksheets(m).cells(7, j + 11) = .year
                        exceldoc.worksheets(m).cells(8, j + 11) = .period
                        If .acc <> "" Then
                            exceldoc.worksheets(m).cells(6, j + 11) = .acc
                        End If
                        REM add drop down for disabling column submission
                        With exceldoc.worksheets(m).cells(5, j + 11).Validation
                            .Delete()
                            .Add(Type:=3, AlertStyle:=1, Operator:=1, Formula1:="enabled,disabled")
                            .IgnoreBlank = True
                            .InCellDropdown = True
                            .InputTitle = ""
                            .ErrorTitle = ""
                            .InputMessage = ""
                            .ErrorMessage = ""
                            .ShowInput = True
                            .ShowError = True
                        End With
                        exceldoc.worksheets(m).cells(5, j + 11) = "enabled"
                        REM linked headers
                        If bc_am_insight_formats.linked_Header = "1" Then
                            exceldoc.worksheets(m).cells(2, j + 11).formula = .year_formula
                            exceldoc.worksheets(m).cells(3, j + 11).formula = .period_formula
                            exceldoc.worksheets(m).cells(4, j + 11).formula = .e_a_formula
                            If .acc <> "" Then
                                exceldoc.worksheets(m).cells(1, j + 11) = .acc_formula
                            End If
                        End If
                    End With
                Next

                REM now write out data assigning to correct header
                For j = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections.count - 1
                    With bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections(j)
                        For k = 0 To .rows.count - 1
                            If .rows(k).link_found = True Then
                                REM search for code in insight sheet
                                REM put in link formula
                                For l = 0 To .rows(k).values_list.count - 1
                                    REM find header position
                                    col_found = 0
                                    For p = 0 To tmp_headers.headers.Count - 1
                                        If tmp_headers.headers(p).year = .rows(k).values_list(l).year And tmp_headers.headers(p).period = .rows(k).values_list(l).period_id Then
                                            If tmp_headers.headers(p).acc <> "" And tmp_headers.headers(p).acc = .rows(k).values_list(l).accounting_standard Then
                                                col_found = p + 11
                                                Exit For
                                            ElseIf (tmp_headers.headers(p).acc = "") Then
                                                col_found = p + 11
                                                Exit For
                                            End If
                                        End If
                                    Next
                                    If col_found <> 0 Then
                                        Try
                                            exceldoc.worksheets(m).cells(.rows(k).excel_row, col_found).formula = .rows(k).values_list(l).formula
                                        Catch
                                            Dim kk As Integer
                                            Dim formula As String
                                            Dim ocommentary As New bc_cs_activity_log("bc_ao_in_excel", "write_out_formula", bc_cs_activity_codes.COMMENTARY, "Formula not valid format: " + .rows(k).values_list(l).formula + " " + CStr(m) + ":" + CStr(.rows(k).excel_row) + ":" + CStr(col_found))
                                            formula = .rows(k).values_list(l).formula
                                            kk = InStr(1, formula, """")
                                            If kk > 0 Then
                                                formula = Right(formula, Len(formula) - kk - 2)
                                                formula = Left(formula, Len(formula) - 1)
                                            End If
                                            Try
                                                exceldoc.worksheets(m).cells(.rows(k).excel_row, col_found).formula = "=" + formula
                                            Catch
                                                ocommentary = New bc_cs_activity_log("bc_ao_in_excel", "write_out_formula", bc_cs_activity_codes.COMMENTARY, "Formula not valid format: " + formula)
                                            End Try
                                        End Try
                                    End If
                                Next
                                If .rows(k).label_formula <> "" Then
                                    exceldoc.worksheets(m).cells(.rows(k).excel_row, 9).formula = .rows(k).label_formula
                                End If
                            End If
                        Next
                    End With
                Next

                REM static
                For j = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections_static.count - 1
                    With bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections_static(j)
                        For k = 0 To .rows.count - 1
                            If .rows(k).link_found = True And .rows(k).link_code <> "" Then
                                REM search for code in insight sheet
                                If .rows(k).repeating_count = 0 Then
                                    exceldoc.worksheets(m).cells(.rows(k).excel_row, 7).formula = .rows(k).values_list(0).formula
                                Else
                                    For l = 0 To .rows(k).repeating_count
                                        exceldoc.worksheets(m).cells(.rows(k).excel_row - .rows(k).repeating_Count + l, 7).formula = .rows(k).values_list(l).formula
                                    Next
                                End If
                                If .rows(k).label_formula <> "" Then
                                    exceldoc.worksheets(m).cells(.rows(k).excel_row, 5).formula = .rows(k).label_formula
                                End If
                            End If
                        Next
                    End With
                Next
                protect_sheet()
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "write_out_formula", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            hide_worksheets()
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "write_out_formula", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Public Sub BACKUP_write_out_formula(ByVal with_header As Boolean)
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "write_out_formula", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim i As Integer
            Dim j As Integer
            Dim k As Integer
            Dim l As Integer
            Dim m As Integer
            Dim last_full_year_prefix As String = ""



            REM write out formulas to BC Sheets
            For i = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Count - 1
                REM select sheet
                For m = 1 To exceldoc.worksheets.count
                    If exceldoc.worksheets(m).cells(1, 1).value = bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).sheet_name Then
                        exceldoc.worksheets(m).select()
                        unprotect_sheet()
                        Exit For
                    End If
                Next
                REM period
                For j = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections.count - 1
                    With bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections(j)
                        For k = 0 To .rows.count - 1
                            If .rows(k).link_found = True Then
                                REM search for code in insight sheet
                                REM put in link formula
                                For l = 0 To .rows(k).values_list.count - 1
                                    If with_header = True Then
                                        If Len(.rows(k).values_list(l).year) = 5 Then
                                            last_full_year_prefix = Left(.rows(k).values_list(l).year, 2)
                                        Else
                                            .rows(k).values_list(l).year = last_full_year_prefix + .rows(k).values_list(l).year
                                        End If
                                        exceldoc.worksheets(m).cells(7, l + 11) = .rows(k).values_list(l).year
                                        exceldoc.worksheets(m).cells(8, l + 11) = .rows(k).values_list(l).period_id
                                        If .rows(k).values_list(l).accounting_standard <> "" Then
                                            exceldoc.worksheets(m).cells(6, l + 11) = .rows(k).values_list(l).accounting_standard
                                        End If
                                        REM linked headers
                                        If bc_am_insight_formats.linked_Header = "1" Then
                                            exceldoc.worksheets(m).cells(2, l + 11).formula = .rows(k).values_list(l).year_formula
                                            exceldoc.worksheets(m).cells(3, l + 11).formula = .rows(k).values_list(l).period_id_formula
                                            exceldoc.worksheets(m).cells(4, l + 11).formula = .rows(k).values_list(l).e_a_formula
                                            If .rows(k).values_list(l).accounting_standard <> "" Then
                                                exceldoc.worksheets(m).cells(1, l + 11) = .rows(k).values_list(l).acc_formula
                                            End If
                                        End If
                                    End If
                                    Try
                                        exceldoc.worksheets(m).cells(.rows(k).excel_row, l + 11).formula = .rows(k).values_list(l).formula
                                    Catch
                                        Dim kk As Integer
                                        Dim formula As String
                                        formula = .rows(k).values_list(l).formula
                                        kk = InStr(1, formula, """")
                                        If kk > 0 Then
                                            formula = Right(formula, Len(formula) - kk - 2)
                                            formula = Left(formula, Len(formula) - 1)
                                        End If
                                        Try
                                            exceldoc.worksheets(m).cells(.rows(k).excel_row, l + 11).formula = "=" + formula
                                        Catch
                                            Dim ocommentary As New bc_cs_activity_log("bc_ao_in_excel", "write_out_formula", bc_cs_activity_codes.COMMENTARY, "Formula not valid format: " + formula)
                                        End Try
                                    End Try
                                Next
                                If .rows(k).label_formula <> "" Then
                                    exceldoc.worksheets(m).cells(.rows(k).excel_row, 9).formula = .rows(k).label_formula
                                End If
                            End If
                        Next
                    End With
                Next
                REM static
                For j = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections_static.count - 1
                    With bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i).bc_om_insightsections_static(j)
                        For k = 0 To .rows.count - 1
                            If .rows(k).link_found = True Then
                                REM search for code in insight sheet
                                If .rows(k).repeating_count = 0 Then
                                    exceldoc.worksheets(m).cells(.rows(k).excel_row, 7).formula = .rows(k).values_list(0).formula
                                Else
                                    For l = 0 To .rows(k).repeating_count
                                        exceldoc.worksheets(m).cells(.rows(k).excel_row - .rows(k).repeating_Count + l, 7).formula = .rows(k).values_list(l).formula
                                    Next
                                End If
                                If .rows(k).label_formula <> "" Then
                                    exceldoc.worksheets(m).cells(.rows(k).excel_row, 5).formula = .rows(k).label_formula
                                End If
                            End If
                        Next
                    End With
                Next
                protect_sheet()
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "write_out_formula", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_ao_in_excel", "write_out_formula", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub build_workbook_preview(ByVal template_name As String, ByVal template_id As String, ByVal template As bc_om_insight_sheet)
        Dim slog As New bc_cs_activity_log("bc_am_in_ao", "build_workbook_preview", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim i, j As Integer

            Dim repeat As Integer = 0
            Me.screen_updating(True)
            Me.disable_application_alerts()
            REM load settings

            For i = 1 To Me.exceldoc.worksheets.count
                If Me.exceldoc.worksheets(i).name = "Preview BCSHEET " + template_name Then
                    exceldoc.worksheets(1).delete()

                    Exit For
                End If
            Next
            addsheet_preview(template_name, template_id, template)
            template.sheet_name = "Preview BCSHEET " + template_name
            populate_sheet_preview(template)
            Dim pis As New bc_om_insight_contribution_for_entity
            Dim ltemplate As New bc_om_insight_sheet
            Dim lsec As New bc_om_insight_section
            Dim lrow As New bc_om_insight_row

            pis.insight_sheets.bc_om_insight_sheets.Clear()
            REM clear out existing values
            For i = 0 To template.bc_om_insightsections.Count - 1
                For j = 0 To template.bc_om_insightsections(i).rows.count - 1
                    template.bc_om_insightsections(i).rows(j).link_found = False
                    template.bc_om_insightsections(i).rows(j).values_list.Clear()
                Next
            Next
            For i = 0 To template.bc_om_insightsections_static.Count - 1
                For j = 0 To template.bc_om_insightsections_static(i).rows.count - 1
                    template.bc_om_insightsections_static(i).rows(j).link_found = False
                    template.bc_om_insightsections_static(i).rows(j).values_list.Clear()
                Next
            Next
            pis.insight_sheets.bc_om_insight_sheets.Add(template)
            bc_am_load_objects.obc_om_insight_contribution_for_entity = pis

            REM simulate linking
            Me.link_workbook(True)
            REM simulate retrieve values
            retreive_values_preview("Preview BCSHEET " + template_name)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_ao", "build_workbook_preview", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            Me.enable_application_alerts()
            Me.screen_updating(False)
            slog = New bc_cs_activity_log("bc_am_in_ao", "build_workbook_preview", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Private Sub retreive_values_preview(ByVal system_sheet As String)
        Dim slog As New bc_cs_activity_log("bc_am_in_ao", "retreive_values_preview", bc_cs_activity_codes.TRACE_EXIT, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim i, j, k As Integer
            REM system sheets
            For j = 1 To exceldoc.worksheets.count
                If exceldoc.worksheets(j).cells(1, 1).text = system_sheet Then
                    For i = 0 To bc_am_insight_retrieve_values.retrieve_values.Count - 1
                        With bc_am_insight_retrieve_values.retrieve_values(i)
                            If .sheet = "" Then
                                Try
                                    exceldoc.worksheets(j).cells(CInt(.row), CInt(.col)) = "[" + CStr(.attribute_code) + "]"
                                Catch
                                    Dim omsg As New bc_cs_message("Blue Curve", "Error with retrieve value: " + CStr(.attribute_code) + "Continue with other retrieve values?", bc_cs_message.MESSAGE, True, False, "Yes", "No")
                                    If omsg.cancel_selected = True Then
                                        Exit Sub
                                    End If
                                End Try
                            End If
                        End With
                    Next
                    Exit For
                End If
            Next
            REM random sheet but only if sheet inserted
            Dim found As Boolean = False
            For i = 0 To bc_am_insight_retrieve_values.retrieve_values.Count - 1
                found = False
                With bc_am_insight_retrieve_values.retrieve_values(i)
                    For j = 1 To exceldoc.worksheets.count
                        If CStr(.sheet) <> "" Then
                            If exceldoc.worksheets(j).name = CStr(.sheet) Then
                                found = True
                                Try
                                    exceldoc.worksheets(j).cells(CInt(.row), CInt(.col)) = "[" + CStr(.attribute_code) + "]"

                                    For k = 0 To bc_am_in_context.insight_items.insight_items.Count - 1
                                        If bc_am_in_context.insight_items.insight_items(k).row_flag = 0 Then
                                            If bc_am_in_context.insight_items.insight_items(k).label_code = CStr(.attribute_code) Then
                                                exceldoc.worksheets(j).cells(CInt(.row), CInt(.col)) = "[" + CStr(bc_am_in_context.insight_items.insight_items(k).desc) + "]"
                                            End If
                                        End If


                                    Next
                                    exceldoc.worksheets(j).cells(CInt(.row), CInt(.col)) = "[" + CStr(.attribute_code) + "]"
                                Catch
                                    Dim omsg As New bc_cs_message("Blue Curve", "Error with retrieve value: " + CStr(.attribute_code) + "Continue with other retrieve values?", bc_cs_message.MESSAGE, True, False, "Yes", "No")
                                    If omsg.cancel_selected = True Then
                                        Exit Sub
                                    End If
                                End Try
                            End If
                        End If
                    Next
                    If found = False And CStr(.sheet) <> "" Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Cannot preview retrieve value: " + CStr(.attribute_code) + " as format sheet: " + CStr(.sheet) + " doesnt exist in sample. Continue with other retriveval values?", bc_cs_message.MESSAGE, True, False, "Yes", "No")
                        If omsg.cancel_selected = True Then
                            Exit Sub
                        End If
                    End If
                End With
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_ao", "retreive_values_preview", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_ao", "retreive_values_preview", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Function check_sheet_exists(ByVal sheet_name As String) As Boolean
        Dim i As Integer
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "check_sheet_exists", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            check_sheet_exists = False
            For i = 1 To exceldoc.Worksheets.Count
                If exceldoc.Worksheets(i).Name = sheet_name Then
                    check_sheet_exists = True
                    Exit Function
                End If
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "check_sheet_exists", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_excel", "check_sheet_exists", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Overrides Sub delete_preview_sheet(ByVal sheet_name As String)
        Dim i As Integer
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "delete_sheet", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            For i = 1 To exceldoc.Worksheets.Count
                If exceldoc.Worksheets(i).Name = sheet_name Then
                    exceldoc.Worksheets(i).delete()
                    Exit For
                End If
            Next
            For i = 1 To exceldoc.Worksheets.Count
                If InStr(exceldoc.Worksheets(i).cells(1, 1).text, "Preview BCSHEET") > 0 Then
                    exceldoc.Worksheets(i).delete()
                    Exit For
                End If
            Next
            Dim k As Integer
            k = 0
            For i = 1 To exceldoc.Worksheets.Count
                k = k + 1
                If InStr(exceldoc.Worksheets(k).cells(1, 1).text, "Preview INTSHEET") > 0 Then
                    exceldoc.Worksheets(k).delete()
                    k = k - 1
                End If
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "delete_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_excel", "delete_sheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overrides Sub delete_int_sheet(ByVal sheet_name As String)
        Dim i As Integer
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "delete_sheet", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            For i = 1 To exceldoc.Worksheets.Count
                If exceldoc.Worksheets(i).Name = sheet_name Then
                    exceldoc.Worksheets(i).delete()
                    Exit Sub
                End If
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "delete_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_excel", "delete_sheet", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub

    Private Sub addsheet_preview(ByVal template_name As String, ByVal template_id As String, ByVal template As bc_om_insight_sheet)
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "addsheet_preview", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            Dim ocommentary As bc_cs_activity_log
            Try
                exceldoc.Styles("bc_section").Delete()
                exceldoc.Styles("bc_row").Delete()
                exceldoc.Styles("bc_header_name").Delete()
                exceldoc.Styles("bc_header_value").Delete()
                exceldoc.Styles("bc_flexible").Delete()
            Catch ex As Exception

            End Try
            REM attempt to insert format sheet

            If insert_format_sheet() = False Then
                If exceldoc.application.ActiveWorkbook.Worksheets(1).visible = False Then
                    exceldoc.application.ActiveWorkbook.Worksheets(1).visible = True
                    exceldoc.application.ActiveWorkbook.Worksheets(1).select()
                    exceldoc.application.ActiveWorkbook.Worksheets.add()
                    exceldoc.application.ActiveWorkbook.Worksheets(2).visible = False
                Else
                    exceldoc.application.ActiveWorkbook.Worksheets.add()
                End If

                exceldoc.application.ActiveWorkbook.Worksheets(1).Cells.locked = False
            End If
            exceldoc.worksheets(1).activate()
            unprotect_sheet()
            exceldoc.worksheets(1).cells(1, 1) = "Preview BCSHEET " + template_name
            exceldoc.worksheets(1).cells(const_header_start_row - 1, const_header_start_col - 4) = "Schema:"
            exceldoc.worksheets(1).cells(const_header_start_row, const_header_start_col - 4) = "Entity:"
            exceldoc.worksheets(1).cells(const_header_start_row + 1, const_header_start_col - 4) = "Class:"
            Try
                exceldoc.worksheets(1).cells(const_header_start_row - 1, const_header_start_col - 4).style = CStr(bc_am_insight_formats.header_Name_Style)
                exceldoc.worksheets(1).cells(const_header_start_row, const_header_start_col - 4).style = CStr(bc_am_insight_formats.header_Name_Style)
                exceldoc.worksheets(1).cells(const_header_start_row + 1, const_header_start_col - 4).style = CStr(bc_am_insight_formats.header_Name_Style)
            Catch
                ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.header_Name_Style) + " not found in workbook.")
            End Try

            exceldoc.worksheets(1).cells(const_header_start_row - 1, const_header_start_col - 3) = "[contributor_name]"
            exceldoc.worksheets(1).cells(const_header_start_row, const_header_start_col - 3) = "[entity_name]"
            exceldoc.worksheets(1).cells(const_header_start_row + 1, const_header_start_col - 3) = "[class_name]"
            Try
                exceldoc.worksheets(1).cells(const_header_start_row - 1, const_header_start_col - 3).style = CStr(bc_am_insight_formats.header_Value_Style)
                exceldoc.worksheets(1).cells(const_header_start_row, const_header_start_col - 3).style = CStr(bc_am_insight_formats.header_Value_Style)
                exceldoc.worksheets(1).cells(const_header_start_row + 1, const_header_start_col - 3).style = CStr(bc_am_insight_formats.header_Value_Style)
            Catch ex As Exception
                ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.header_Value_Style) + " not found in workbook.")
            End Try

            'exceldoc.worksheets(1).cells(6, 1) = "Period Label Code"
            'exceldoc.worksheets(1).cells(6, 2) = "Period Link Code"
            'exceldoc.worksheets(1).cells(6, 3) = "Static Label Code"
            'exceldoc.worksheets(1).cells(6, 4) = "Static Link Code"
            'exceldoc.worksheets(1).cells(6, 5) = "Static Label"
            'exceldoc.worksheets(1).cells(6, 6) = "Symbol"
            'exceldoc.worksheets(1).cells(6, 9) = "Period Label"
            'exceldoc.worksheets(1).cells(6, 10) = "Symbol"
            'exceldoc.worksheets(1).cells(7, 10) = "Year"
            'exceldoc.worksheets(1).cells(8, 10) = "Period"
            Try
                exceldoc.worksheets(1).cells(6, 1).style = CStr(bc_am_insight_formats.row_Header_Style)
                exceldoc.worksheets(1).cells(6, 2).style = CStr(bc_am_insight_formats.row_Header_Style)
                exceldoc.worksheets(1).cells(6, 3).style = CStr(bc_am_insight_formats.row_Header_Style)
                exceldoc.worksheets(1).cells(6, 4).style = CStr(bc_am_insight_formats.row_Header_Style)
                exceldoc.worksheets(1).cells(6, 5).style = CStr(bc_am_insight_formats.row_Header_Style)
                exceldoc.worksheets(1).cells(6, 6).style = CStr(bc_am_insight_formats.row_Header_Style)
                exceldoc.worksheets(1).cells(6, 7).style = CStr(bc_am_insight_formats.row_Header_Style)
                exceldoc.worksheets(1).cells(6, 9).style = CStr(bc_am_insight_formats.row_Header_Style)
                'exceldoc.worksheets(1).cells(6, 10).style = CStr(bc_am_insight_formats.row_header_style)
                'exceldoc.worksheets(1).cells(7, 10).style = CStr(bc_am_insight_formats.row_header_style)
                'exceldoc.worksheets(1).cells(8, 10).style = CStr(bc_am_insight_formats.row_header_style)
                'exceldoc.worksheets(1).cells(6, 11).style = CStr(bc_am_insight_formats.row_header_style)

            Catch ex As Exception
                ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet_preview", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.row_Header_Style) + " not found in workbook.")
            End Try
            REM keep entity_id and class_id in hidden row A
            exceldoc.worksheets(1).cells(2, 1) = CStr("[ins] contributor_id")
            exceldoc.worksheets(1).cells(3, 1) = CStr("[ins] entity_id")
            exceldoc.worksheets(1).cells(4, 1) = CStr("[ins] class_id")
            exceldoc.worksheets(1).cells(5, 1) = CStr(template_id)
            REM hide row A



            REM name sheet tab
            Try
                'Dim oldCI As System.Globalization.CultureInfo = _
                'System.Threading.Thread.CurrentThread.CurrentCulture

                System.Threading.Thread.CurrentThread.CurrentCulture = _
                          New System.Globalization.CultureInfo("en-GB")
                exceldoc.worksheets(1).name = "Preview BCSHEET " + template_name
                System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            Catch

            End Try
            REM

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_excel", "addsheet_preview", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            slog = New bc_cs_activity_log("bc_am_in_excel", "addsheet_preview", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Sub populate_sheet_preview(ByRef bc_om_is As bc_om_insight_sheet)
        Dim slog = New bc_cs_activity_log("bc_am_in_link", "populate_sheet_preview", bc_cs_activity_codes.TRACE_ENTRY, "")

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            REM loop for all sections
            REM current_cell
            Dim ocommentary As bc_cs_activity_log
            current_row = const_row_start
            current_cell = const_col_start
            Dim i, j, k, l As Integer
            For i = 0 To bc_om_is.bc_om_insightsections.Count - 1
                With bc_om_is.bc_om_insightsections(i)
                    exceldoc.worksheets(1).cells(current_row, current_cell) = .name()
                    Try
                        exceldoc.worksheets(1).cells(current_row, current_cell).style = bc_am_insight_formats.section_style
                    Catch
                        ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.row_Style) + " not found in workbook.")
                    End Try
                    current_row = current_row + const_section_space
                    REM rows for section
                    For j = 0 To .rows.count - 1
                        exceldoc.worksheets(1).cells(current_row, current_cell) = .rows(j).label
                        If CStr(.rows(j).datatype) = "6" Then
                            exceldoc.worksheets(1).cells(current_row, current_cell + 1) = "(Output)"
                        Else
                            exceldoc.worksheets(1).cells(current_row, current_cell + 1) = .rows(j).scale_symbol
                        End If
                        Try
                            If .rows(j).flexible_label_flag = True Then
                                ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Flexible Label Found unlocking cell(" + CStr(current_row) + "," + CStr(current_cell) + ")")
                                Try
                                    exceldoc.worksheets(1).cells(current_row, current_cell).style = bc_am_insight_formats.flexible_Label_Style
                                    exceldoc.worksheets(1).cells(current_row, current_cell).locked = False
                                Catch
                                    ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.flexible_Label_Style) + " not found in workbook.")
                                End Try
                            Else
                                exceldoc.worksheets(1).cells(current_row, current_cell).style = bc_am_insight_formats.row_Style
                            End If
                            exceldoc.worksheets(1).cells(current_row, current_cell + 1).style = bc_am_insight_formats.row_Style
                        Catch
                            ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.row_Style) + " not found in workbook.")
                        End Try
                        REM put label code in hidden area
                        exceldoc.worksheets(1).cells(current_row, 1) = .rows(j).label_code
                        exceldoc.worksheets(1).cells(current_row, 2) = .rows(j).link_code

                        REM write row number back to object model
                        .rows(j).excel_row() = current_row
                        current_row = current_row + 1
                    Next
                    current_row = current_row + 1
                End With
            Next
            REM format width
            exceldoc.worksheets(1).Range("I7:I100").Select()
            exceldoc.application.Selection.AutoFormat(Format:=-4154, Number:=False, Font _
                :=False, Alignment:=False, Border:=False, Pattern:=False, Width:=True)
            exceldoc.worksheets(1).Range("J7:J100").Select()
            exceldoc.application.Selection.AutoFormat(Format:=-4154, Number:=False, Font _
                :=False, Alignment:=False, Border:=False, Pattern:=False, Width:=True)


            exceldoc.worksheets(1).cells(current_row, 1) = "END"
            current_row = const_row_start
            For i = 0 To bc_om_is.bc_om_insightsections_static.Count - 1
                With bc_om_is.bc_om_insightsections_static(i)
                    exceldoc.worksheets(1).cells(current_row, current_cell - 4) = .name()
                    Try
                        exceldoc.worksheets(1).cells(current_row, current_cell - 4).style = bc_am_insight_formats.section_style
                    Catch
                        ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.section_style) + " not found in workbook.")
                    End Try
                    current_row = current_row + const_section_space
                    REM rows for section
                    For j = 0 To .rows.count - 1
                        exceldoc.worksheets(1).cells(current_row, current_cell - 4) = .rows(j).label
                        Try
                            exceldoc.worksheets(1).cells(current_row, current_cell - 4).style = bc_am_insight_formats.row_Style
                        Catch
                            ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.row_Style) + " not found in workbook.")
                        End Try
                        If CStr(.rows(j).datatype) = "6" Then
                            exceldoc.worksheets(1).cells(current_row, 6) = "(Output)"
                        Else
                            exceldoc.worksheets(1).cells(current_row, current_cell - 3) = .rows(j).scale_symbol
                        End If
                        Try
                            If .rows(j).flexible_label_flag = True Then
                                ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Flexible Label Found unlocking cell(" + CStr(current_row) + "," + CStr(current_cell - 4) + ")")
                                Try
                                    exceldoc.worksheets(1).cells(current_row, current_cell - 4).style = bc_am_insight_formats.flexible_Label_Style
                                    exceldoc.worksheets(1).cells(current_row, current_cell - 4).locked = False
                                Catch
                                    ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.flexible_Label_Style) + " not found in workbook.")
                                End Try
                            Else
                                exceldoc.worksheets(1).cells(current_row, current_cell - 4).style = bc_am_insight_formats.row_Style
                            End If
                            exceldoc.worksheets(1).cells(current_row, current_cell - 3).style = bc_am_insight_formats.row_Style
                        Catch
                            ocommentary = New bc_cs_activity_log("bc_am_in_excel", "addsheet", bc_cs_activity_codes.COMMENTARY, "Excel Style: " + CStr(bc_am_insight_formats.row_Style) + " not found in workbook.")
                        End Try
                        REM put label code in hidden area
                        exceldoc.worksheets(1).cells(current_row, 3) = .rows(j).label_code
                        exceldoc.worksheets(1).cells(current_row, 4) = .rows(j).link_code
                        current_row = current_row + 1
                        If .rows(j).repeating_count > 0 Then
                            If .rows(j).flexible_label_value.count > 0 Then
                                exceldoc.worksheets(1).cells(current_row - 1, current_cell - 4) = .rows(j).flexible_label_value.item(0)
                            Else
                                exceldoc.worksheets(1).cells(current_row - 1, current_cell - 4) = .rows(j).label + " " + CStr(1)
                            End If

                            exceldoc.worksheets(1).cells(current_row - 1, 4) = .rows(j).link_code + "_1"
                            For k = 1 To .rows(j).repeating_count
                                If .rows(j).flexible_label_value.count > 0 Then
                                    exceldoc.worksheets(1).cells(current_row, current_cell - 4) = .rows(j).flexible_label_value.item(k)
                                Else
                                    exceldoc.worksheets(1).cells(current_row, current_cell - 4) = .rows(j).label + " " + CStr(k + 1)
                                End If
                                exceldoc.worksheets(1).cells(current_row, current_cell - 3) = .rows(j).scale_symbol
                                If .rows(j).flexible_label_flag = True Then
                                    exceldoc.worksheets(1).cells(current_row, current_cell - 4).style = bc_am_insight_formats.flexible_Label_Style
                                    exceldoc.worksheets(1).cells(current_row, current_cell - 4).locked = False
                                End If
                                exceldoc.worksheets(1).cells(current_row, 4) = .rows(j).link_code + "_" + CStr(k + 1)
                                REM put label code in hidden area
                                exceldoc.worksheets(1).cells(current_row, 3) = .rows(j).label_code
                                current_row = current_row + 1
                            Next
                        End If
                        REM write row number back to object model
                        .rows(j).excel_row() = current_row - 1
                        REM if lookup validation then create drop down here
                        For l = 0 To .rows(j).validations.bc_om_cell_validation.count - 1
                            If .rows(j).validations.bc_om_cell_validation(l).validation_id = "7" Then
                                If .rows(j).validations.bc_om_cell_validation(l).valid_values_list <> "" Then
                                    Dim list = .rows(j).validations.bc_om_cell_validation(l).valid_values_list

                                    For k = 0 To .rows(j).repeating_count
                                        With exceldoc.worksheets(1).cells(current_row - 1 - k, 7).Validation
                                            .Delete()
                                            .Add(Type:=3, AlertStyle:=1, Operator:=1, Formula1:=list)
                                            .IgnoreBlank = True
                                            .InCellDropdown = True
                                            .InputTitle = ""
                                            .ErrorTitle = ""
                                            .InputMessage = ""
                                            .ErrorMessage = ""
                                            .ShowInput = True
                                            .ShowError = True
                                        End With
                                    Next
                                End If
                            End If
                        Next
                    Next
                    current_row = current_row + 1
                End With
            Next
            exceldoc.worksheets(1).cells(current_row, 3) = "END"
            exceldoc.worksheets(1).Range("E7:E100").Select()
            exceldoc.application.Selection.AutoFormat(Format:=-4154, Number:=False, Font _
                :=False, Alignment:=False, Border:=False, Pattern:=False, Width:=True)
            exceldoc.worksheets(1).Range("F7:F100").Select()
            exceldoc.application.Selection.AutoFormat(Format:=-4154, Number:=False, Font _
                :=False, Alignment:=False, Border:=False, Pattern:=False, Width:=True)

            REM loop for all rows
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_link", "populate_sheet_preview", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            protect_sheet()
            slog = New bc_cs_activity_log("bc_am_in_link", "populate_sheet_preview", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Public Function create_format_sheet(ByVal fn As String, ByVal editcol As ao_rgb, ByVal contcol As ao_rgb, ByVal headcol As ao_rgb, ByVal header_name_font As ao_font, ByVal header_value_font As ao_font, ByVal section_font As ao_font, ByVal row_font As ao_font, ByVal flexible_font As ao_font, ByVal head_font As ao_font, ByVal edit_font As ao_font) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_in_excel", "create_format_sheet", bc_cs_activity_codes.TRACE_EXIT, "")
        Dim new_workbook As Object

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try
            create_format_sheet = True
            REM add a new workbook
            Me.disable_application_alerts()
            exceldoc.Application.Workbooks.Add()
            new_workbook = exceldoc.application.workbooks(exceldoc.application.workbooks.count)
            REM remove 2 and 3 sheets
            new_workbook.Worksheets(2).Delete()
            new_workbook.Worksheets(2).Delete()
            new_workbook.Worksheets(1).Name = "BC Format Sheet"
            REM unlock all cells
            new_workbook.Worksheets(1).Cells.Locked = False
            REM system area
            new_workbook.Worksheets(1).Range("A1:D1").Select()
            new_workbook.application.Selection.EntireColumn.Hidden = True
            REM set headers
            new_workbook.Worksheets(1).Cells(5, 10) = "Enabled"
            new_workbook.Worksheets(1).Cells(6, 10) = "Acc"
            new_workbook.Worksheets(1).Cells(7, 10) = "Year"
            new_workbook.Worksheets(1).Cells(8, 10) = "Period"
            REM lock system areas
            Try
                new_workbook.Worksheets(1).Cells.Select()
                With new_workbook.application.Selection
                    .font.name = edit_font.name
                    .font.bold = edit_font.bold
                    .font.italic = edit_font.italic
                    .font.size = edit_font.size
                    .interior.Color = RGB(editcol.red, editcol.green, editcol.blue)
                End With
                new_workbook.Worksheets(1).Range("A1:D1").Select()
                new_workbook.application.Selection.EntireColumn.Hidden = True
                new_workbook.Worksheets(1).Range("a1:J8").Select()
                new_workbook.Application.Selection.Locked = True
                With new_workbook.application.Selection.Interior
                    .Color = RGB(contcol.red, contcol.green, contcol.blue)
                End With
                new_workbook.Worksheets(1).Range("a1:f1000").Select()
                new_workbook.Application.Selection.Locked = True
                With new_workbook.application.Selection.Interior
                    .Color = RGB(contcol.red, contcol.green, contcol.blue)
                End With
                new_workbook.Worksheets(1).Range("h5:j1000").Select()
                new_workbook.Application.Selection.Locked = True
                With new_workbook.application.Selection.Interior
                    .Color = RGB(contcol.red, contcol.green, contcol.blue)
                End With
                new_workbook.Worksheets(1).Range("k1:bb4").Select()
                new_workbook.Application.Selection.Locked = True
                With new_workbook.application.Selection.Interior
                    .Color = RGB(contcol.red, contcol.green, contcol.blue)
                End With
                REM header area
                new_workbook.Worksheets(1).Range("j5:bb8").Select()
                new_workbook.Application.Selection.Locked = False
                With new_workbook.application.Selection
                    .font.name = head_font.name
                    .font.bold = head_font.bold
                    .font.italic = head_font.italic
                    .font.size = head_font.size
                    .interior.Color = RGB(headcol.red, headcol.green, headcol.blue)
                End With
                Try
                    new_workbook.Styles("bc_section").Delete()
                    new_workbook.Styles("bc_row").Delete()
                    new_workbook.Styles("bc_header_name").Delete()
                    new_workbook.Styles("bc_header_value").Delete()
                    new_workbook.Styles("bc_flexible").Delete()
                Catch

                End Try

                REM add system styles
                new_workbook.Styles.Add(Name:="bc_section")
                new_workbook.Styles.Add(Name:="bc_row")
                new_workbook.Styles.Add(Name:="bc_header_name")
                new_workbook.Styles.Add(Name:="bc_header_value")
                new_workbook.Styles.Add(Name:="bc_flexible")
                Try
                    new_workbook.worksheets(1).cells(7, 5).style = "bc_section"
                    new_workbook.worksheets(1).cells(9, 5).style = "bc_row"
                    new_workbook.worksheets(1).cells(2, 5).style = "bc_header_name"
                    new_workbook.worksheets(1).cells(2, 6).style = "bc_header_value"
                    new_workbook.worksheets(1).cells(1, 2).style = "bc_flexible"

                    REM now set style fonts
                    Dim i As Integer
                    For i = 1 To new_workbook.Styles.Count
                        REM header name
                        If new_workbook.Styles(i).name = "bc_header_name" Then
                            new_workbook.Styles(i).font.name = header_name_font.name
                            new_workbook.Styles(i).font.bold = header_name_font.bold
                            new_workbook.Styles(i).font.italic = header_name_font.italic
                            new_workbook.Styles(i).font.size = header_name_font.size
                            new_workbook.Styles(i).interior.color = RGB(header_name_font.colour.red, header_name_font.colour.green, header_name_font.colour.blue)
                        End If
                        REM header value
                        If new_workbook.Styles(i).name = "bc_header_value" Then
                            new_workbook.Styles(i).font.name = header_value_font.name
                            new_workbook.Styles(i).font.bold = header_value_font.bold
                            new_workbook.Styles(i).font.italic = header_value_font.italic
                            new_workbook.Styles(i).font.size = header_value_font.size
                            new_workbook.Styles(i).interior.color = RGB(header_value_font.colour.red, header_value_font.colour.green, header_value_font.colour.blue)
                        End If
                        REM section name
                        If new_workbook.Styles(i).name = "bc_section" Then
                            new_workbook.Styles(i).font.name = section_font.name
                            new_workbook.Styles(i).font.bold = section_font.bold
                            new_workbook.Styles(i).font.italic = section_font.italic
                            new_workbook.Styles(i).font.size = section_font.size
                            new_workbook.Styles(i).interior.color = RGB(section_font.colour.red, section_font.colour.green, section_font.colour.blue)
                        End If
                        REM row name
                        If new_workbook.Styles(i).name = "bc_row" Then
                            new_workbook.Styles(i).font.name = row_font.name
                            new_workbook.Styles(i).font.bold = row_font.bold
                            new_workbook.Styles(i).font.italic = row_font.italic
                            new_workbook.Styles(i).font.size = row_font.size
                            new_workbook.Styles(i).interior.color = RGB(row_font.colour.red, section_font.colour.green, section_font.colour.blue)
                        End If
                        REM flexible name
                        If new_workbook.Styles(i).name = "bc_flexible" Then
                            new_workbook.Styles(i).font.name = flexible_font.name
                            new_workbook.Styles(i).font.bold = flexible_font.bold
                            new_workbook.Styles(i).font.italic = flexible_font.italic
                            new_workbook.Styles(i).font.size = flexible_font.size
                            new_workbook.Styles(i).interior.color = RGB(row_font.colour.red, section_font.colour.green, section_font.colour.blue)
                            new_workbook.Styles(i).font.color = RGB(flexible_font.colour.red, flexible_font.colour.green, flexible_font.colour.blue)
                        End If
                    Next
                    REM now set data input and header styles

                Catch ex As Exception
                    MsgBox(ex.Message)
                End Try
            Catch ex As Exception
                MsgBox(ex.Message)
            End Try

            REM SW cope with office versions
            If bc_cs_central_settings.userOfficeStatus = 3 Then
                new_workbook.saveas(filename:=bc_cs_central_settings.local_template_path + fn, fileformat:=56)
            ElseIf bc_cs_central_settings.userOfficeStatus = 1 Then
                new_workbook.saveas(filename:=bc_cs_central_settings.local_template_path + fn, fileformat:=-4143)
            ElseIf bc_cs_central_settings.userOfficeStatus = 2 Then
                new_workbook.saveas(filename:=bc_cs_central_settings.local_template_path + fn, fileformat:=51)
            End If
            'If bc_cs_central_settings.office_version < 12 And bc_cs_central_settings.user_office_version >= 12 Then
            '    new_workbook.saveas(Filename:=bc_cs_central_settings.local_template_path + fn, fileformat:=56)
            'ElseIf bc_cs_central_settings.office_version < 12 Then
            '    new_workbook.saveas(Filename:=bc_cs_central_settings.local_template_path + fn, fileformat:=-4143)
            'ElseIf bc_cs_central_settings.office_version >= 12 Then
            '    new_workbook.saveas(Filename:=bc_cs_central_settings.local_template_path + fn, fileformat:=51)
            'End If

            new_workbook.close()
            REM highlight data entry areas
        Catch ex As Exception
            create_format_sheet = False
            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "create_format_sheet", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            Me.enable_application_alerts()
        End Try

    End Function

    Public Overrides Sub BuildCorpAction(ByVal CorpAction As bc_om_corp_action)

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Dim adjustment As bc_om_eventaction
        Dim calculation As bc_om_eventcalculation
        Dim ExcelError As New bc_om_excel_data_element_errors
        Dim LoadRow As Long
        Dim loadColumnOrigonal As Long
        Dim loadColumnAdjustment As Long
        Dim endBlockRow As Long
        Dim endBlockColumn As Long
        Dim cellCount As Long

        Dim formatFilename As String
        Dim progressIncremant As Long = 0
        Dim progressValue As Long = 0
        Dim exdateRow As Long = 0

        Dim i As Integer
        Dim eventList As New bc_om_corp_events()
        Dim eventType As bc_om_corp_event = Nothing
        Dim workRow As Long

        Dim slog As New bc_cs_activity_log("bc_ao_in_excel", "build_corp_action", bc_cs_activity_codes.TRACE_ENTRY, "")

        LoadRow = 13
        loadColumnOrigonal = 2
        loadColumnAdjustment = 0

        If bc_cs_central_settings.userOfficeStatus = 2 Then
            formatFilename = "bc_Ca_default.xlsx"
        Else
            formatFilename = "bc_Ca_default.xls"
        End If

        Try

            If bc_cs_central_settings.connection_method = bc_cs_central_settings.ADO Then
                eventList.db_read()
            ElseIf bc_cs_central_settings.connection_method = bc_cs_central_settings.SOAP Then
                eventList.tmode = bc_cs_soap_base_class.tREAD
                If eventList.transmit_to_server_and_receive(eventList, True) = False Then
                    Exit Sub
                End If
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Cannot connect to " + bc_cs_central_settings.soap_server + " application cannot be launched.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
            For i = 0 To eventList.ActionType.Count - 1
                If eventList.ActionType(i).id = CorpAction.ActionEventId Then
                    eventType = eventList.ActionType(i)
                End If
            Next

            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve insight", "Building Workbook", 5, False, True)
            progressValue = 5

            exceldoc.activesheet.Cells(1, 1).Select()
            REM exceldoc.activesheet.Range("A13", "Z13").Select()
            exceldoc.Application.Activewindow.FreezePanes = False

            exceldoc.application.screenupdating = False

            If exceldoc.worksheets(1).Name <> "CorpAction" Then

                Dim oexcel As New bc_ao_in_excel(exceldoc)
                If formatFilename <> "" Then
                    If Me.CheckFormatFileExists(formatFilename) = True Then
                        REM load it
                        oexcel.insert_sheet(formatFilename, bc_cs_central_settings.local_template_path + formatFilename)
                    Else
                        Dim omsg As New bc_cs_message("Blue Curve", "Format filename " + formatFilename + "doesnt exist", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    End If
                End If
                exceldoc.activesheet.visible = False

            End If

            exceldoc.worksheets(2).select()
            exceldoc.ActiveSheet.name = "Details"

            'Clean up any old data
            exceldoc.ActiveSheet.Unprotect()
            exceldoc.activesheet.Cells.Select()
            exceldoc.application.Selection.Delete()
            exceldoc.activesheet.Range("B8").Select()

            exceldoc.activesheet.Columns("A:A").ColumnWidth = 30
            exceldoc.activesheet.Columns("B:Z").ColumnWidth = 14

            exceldoc.activesheet.cells(1, 1).value = "Corporate Action Adjustment"
            exceldoc.activesheet.cells(1, 1).style = "CorpAction_Heading"

            exceldoc.activesheet.cells(3, 1).value = "ActionID:"
            exceldoc.activesheet.cells(4, 1).value = "Entity:"
            exceldoc.activesheet.cells(5, 1).value = "Event:"
            exceldoc.activesheet.cells(6, 1).value = "Date:"
            exceldoc.activesheet.cells(7, 1).value = "Ex-Date -1 Day:"
            exceldoc.activesheet.cells(3, 1).style = "CAHeading3"
            exceldoc.activesheet.cells(4, 1).style = "CAHeading3"
            exceldoc.activesheet.cells(5, 1).style = "CAHeading3"
            exceldoc.activesheet.cells(6, 1).style = "CAHeading3"
            exceldoc.activesheet.cells(7, 1).style = "CAHeading3"

            exceldoc.activesheet.cells(3, 2).value = CorpAction.ActionId
            exceldoc.activesheet.cells(3, 2).style = "CADataText"
            exceldoc.activesheet.cells(4, 2).value = CorpAction.EntityDescription
            exceldoc.activesheet.cells(4, 2).style = "CADataText"
            exceldoc.activesheet.cells(4, 3).value = CorpAction.ActionEntityId
            exceldoc.activesheet.cells(4, 3).style = "CADataText"
            exceldoc.activesheet.cells(5, 2).value = CorpAction.EventDescriprion
            exceldoc.activesheet.cells(5, 2).style = "CADataText"
            exceldoc.activesheet.cells(6, 2).value = DateValue(Now)
            exceldoc.activesheet.cells(6, 2).style = "CADataDate"
            exceldoc.activesheet.cells(7, 2).value = DateAdd(DateInterval.Day, -1, CorpAction.ActionExDate)
            exceldoc.activesheet.cells(7, 2).style = "CADataDate"

            exceldoc.activesheet.cells(8, 1).value = "Adjustment Factor"
            exceldoc.activesheet.cells(8, 1).style = "CAHeading3"
            'exceldoc.activesheet.cells(7, 2).value = CorpAction.calcadjustment

            REM Fair Value
            exceldoc.activesheet.cells(9, 1).value = "Adjustment Factor for FV"
            exceldoc.activesheet.cells(9, 1).style = "CAHeading3"

            REM Show Inputs
            exceldoc.activesheet.cells(3, 4).value = "Input Values"
            exceldoc.activesheet.cells(3, 4).style = "CAOldHeading2"
            exceldoc.activesheet.cells(4, 4).value = "Shares:"
            exceldoc.activesheet.cells(4, 4).style = "CAHeading3"
            exceldoc.activesheet.cells(5, 4).value = "Ex-Date:"
            exceldoc.activesheet.cells(5, 4).style = "CAHeading3"
            exceldoc.activesheet.cells(6, 4).value = "Price:"
            exceldoc.activesheet.cells(6, 4).style = "CAHeading3"
            exceldoc.activesheet.cells(7, 4).value = "Ratio:"
            exceldoc.activesheet.cells(7, 4).style = "CAHeading3"

            REM set headings from static data
            For i = 0 To eventType.DataInputs.input.Count - 1
                If eventType.DataInputs.input(i).inputcode = "date" Then
                    exceldoc.activesheet.cells(5, 4).value = eventType.DataInputs.input(i).inputtype
                    exceldoc.activesheet.cells(7, 1).value = eventType.DataInputs.input(i).inputtype + " -1 Day:"
                    exceldoc.activesheet.cells(5, 5).value = CorpAction.ActionExDate
                    exceldoc.activesheet.cells(5, 5).style = "CADataDate"
                End If
                If eventType.DataInputs.input(i).inputcode = "numb" Then
                    exceldoc.activesheet.cells(4, 4).value = eventType.DataInputs.input(i).inputtype
                    exceldoc.activesheet.cells(4, 5).value = CorpAction.ActionNewShares
                    exceldoc.activesheet.cells(4, 5).Style = "CACanAdjustInt"
                End If
                If eventType.DataInputs.input(i).inputcode = "mony" Then
                    exceldoc.activesheet.cells(6, 4).value = eventType.DataInputs.input(i).inputtype
                    exceldoc.activesheet.cells(6, 5).value = CorpAction.ActionPrice
                    exceldoc.activesheet.cells(6, 5).Style = "CACanAdjustDecimal"
                End If
                If eventType.DataInputs.input(i).inputcode = "rati" Then
                    exceldoc.activesheet.cells(7, 4).value = eventType.DataInputs.input(i).inputtype
                    exceldoc.activesheet.cells(7, 5).value = CorpAction.ActionRatio
                    exceldoc.activesheet.cells(7, 5).Style = "CACanAdjustDecimal"
                End If
            Next


            If CorpAction.EventType <> "ALLHIST" Then

                REM Show Start Values
                exceldoc.activesheet.cells(3, 7).value = "Current Values"
                exceldoc.activesheet.cells(3, 7).style = "CAOldHeading2"

                exceldoc.activesheet.cells(4, 7).value = "Old Shares:"
                exceldoc.activesheet.cells(4, 7).style = "CAHeading3"
                exceldoc.activesheet.cells(4, 8).value = CorpAction.StartShares
                exceldoc.activesheet.cells(4, 8).Style = "CACanAdjustDecimal"

                exceldoc.activesheet.cells(5, 7).value = "Old MCAP:"
                exceldoc.activesheet.cells(5, 7).style = "CAHeading3"
                exceldoc.activesheet.cells(5, 8).value = CorpAction.StartMcap
                exceldoc.activesheet.cells(5, 8).style = "CAOldValue"
                exceldoc.activesheet.cells(5, 8).Style = "CACanAdjustDecimal"

                exceldoc.activesheet.cells(6, 7).value = "Close Price:"
                exceldoc.activesheet.cells(6, 7).style = "CAHeading3"
                exceldoc.activesheet.cells(6, 8).value = CorpAction.StartPrice
                exceldoc.activesheet.cells(6, 8).style = "CAOldValue"
                exceldoc.activesheet.cells(6, 8).Style = "CACanAdjustDecimal"

                REM Show Calculations
                exceldoc.activesheet.cells(3, 10).value = "Calculated Values"
                exceldoc.activesheet.cells(3, 10).style = "CAOldHeading2"
                exceldoc.activesheet.cells(4, 10).value = "Shares:"
                exceldoc.activesheet.cells(4, 10).style = "CAHeading3"
                exceldoc.activesheet.cells(4, 11).style = "CANewValue"
                exceldoc.activesheet.cells(5, 10).value = "Capital Increase:"
                exceldoc.activesheet.cells(5, 10).style = "CAHeading3"
                exceldoc.activesheet.cells(5, 11).style = "CANewValue"
                exceldoc.activesheet.cells(6, 10).value = "Adjusted Price:"
                exceldoc.activesheet.cells(6, 10).style = "CAHeading3"
                exceldoc.activesheet.cells(6, 11).style = "CANewValue"
                exceldoc.activesheet.cells(7, 10).value = "Cash Dividend:"
                exceldoc.activesheet.cells(7, 10).style = "CAHeading3"
                exceldoc.activesheet.cells(7, 11).style = "CANewValue"

                REM Fair Value
                exceldoc.activesheet.cells(7, 7).value = "FV:"
                exceldoc.activesheet.cells(7, 8).value = CorpAction.FairValue
                exceldoc.activesheet.cells(7, 8).style = "CAOldValue"
                exceldoc.activesheet.cells(7, 8).Style = "CACanAdjustDecimal"
                exceldoc.activesheet.cells(7, 7).style = "CAHeading3"
                exceldoc.activesheet.cells(8, 7).value = "Old FV MCAP:"
                exceldoc.activesheet.cells(8, 7).style = "CAHeading3"
                exceldoc.activesheet.cells(8, 8).Style = "CACanAdjustDecimal"
                exceldoc.activesheet.cells(8, 10).value = "Adjusted FV:"
                exceldoc.activesheet.cells(8, 10).style = "CAHeading3"
                exceldoc.activesheet.cells(8, 11).style = "CANewValue"

                For i = 0 To CorpAction.Calculations.eventcalculations.Count - 1
                    calculation = CorpAction.Calculations.eventcalculations(i)
                    exceldoc.activesheet.cells(calculation.targetrow, calculation.targetcolumn).value = calculation.adjformula
                Next
            End If

            REM If the adjustment factor is 0 set it to 1
            If exceldoc.activesheet.cells(8, 2).value = 0 And CorpAction.EventType = "ALLHIST" Then
                exceldoc.activesheet.cells(8, 2).value = 1
            End If

            REM Fair Value
            If exceldoc.activesheet.cells(9, 2).value = 0 And CorpAction.EventType = "ALLHIST" Then
                exceldoc.activesheet.cells(9, 2).value = 1
            End If
            If exceldoc.activesheet.cells(9, 2).value <> 0 Or exceldoc.activesheet.cells(9, 2).formula <> "" Then
                exceldoc.activesheet.cells(9, 2).Style = "CANewValue"
            End If
            exceldoc.activesheet.Range("A13", "Z13").Select()
            exceldoc.Application.ActiveWindow.FreezePanes = True

            If exceldoc.activesheet.cells(8, 2).value <> 0 Or exceldoc.activesheet.cells(8, 2).formula <> "" Then
                exceldoc.activesheet.cells(8, 2).Style = "CANewValue"
                exceldoc.activesheet.Range("B8").Select()
            Else
                exceldoc.activesheet.Range("K4").Select()
            End If

            REM find the number of cells needed
            cellCount = 0
            For i = 0 To CorpAction.Adjustments.eventactions.Count - 1
                adjustment = CorpAction.Adjustments.eventactions(i)
                If adjustment.datatype = "time series" Then
                    If adjustment.datecolumn = 0 Then
                        cellCount = cellCount + 1
                    Else
                        cellCount = cellCount + 2
                    End If
                End If
            Next i
            loadColumnAdjustment = cellCount + 1

            REM  Show data
            exceldoc.activesheet.cells(11, loadColumnOrigonal).value = "Original Values"
            exceldoc.activesheet.cells(11, loadColumnOrigonal).style = "CAOldHeading2"
            exceldoc.activesheet.cells(11, loadColumnAdjustment + 2).value = "Adjusted Values"
            exceldoc.activesheet.cells(11, loadColumnAdjustment + 2).style = "CAOldHeading2"

            Dim oio As New bc_am_excel_data_io(exceldoc, "EXCEL")

            If CorpAction.Adjustments.eventactions.Count > 0 Then
                progressIncremant = System.Math.Round(90 / CorpAction.Adjustments.eventactions.Count)
            End If

            LoadRow = 13
            loadColumnOrigonal = 0
            For i = 0 To CorpAction.Adjustments.eventactions.Count - 1
                adjustment = CorpAction.Adjustments.eventactions(i)
                If adjustment.datatype = "time series" Then

                    If adjustment.datecolumn = 0 Then
                        loadColumnOrigonal = loadColumnOrigonal + 1
                        loadColumnAdjustment = loadColumnAdjustment + 1
                    Else
                        loadColumnOrigonal = loadColumnOrigonal + 2
                        loadColumnAdjustment = loadColumnAdjustment + 2
                    End If

                    exceldoc.activesheet.cells(12, loadColumnOrigonal).value() = adjustment.adjustshort
                    exceldoc.activesheet.cells(12, loadColumnOrigonal).style = "CAOldTitle"
                    exceldoc.activesheet.cells(12, loadColumnAdjustment).value() = adjustment.adjustshort
                    exceldoc.activesheet.cells(12, loadColumnAdjustment).style = "CANewTitle"
                    If adjustment.datecolumn = 1 Then
                        exceldoc.activesheet.cells(12, loadColumnAdjustment - 1).style = "CAOldTitle"
                        exceldoc.activesheet.cells(12, loadColumnOrigonal - 1).style = "CAOldTitle"
                    End If

                    If adjustment.displaytemplate <> "" Then
                        'Dim oio As New bc_am_excel_data_io(exceldoc, "EXCEL")
                        oio.run_template(adjustment.displaytemplate, ExcelError, False)
                        endBlockRow = GetEndRow(LoadRow, loadColumnOrigonal)

                        'If this is a date column set the exdate row (this is used to set the style)
                        If adjustment.datecolumn = 1 Then
                            exdateRow = GetExdateRow(LoadRow, loadColumnOrigonal, exdateRow, CorpAction)
                        End If
                        BuildCorpActionAdjustment(LoadRow, endBlockRow, loadColumnAdjustment, loadColumnOrigonal, CorpAction.CalcAdjustment, adjustment.adjusttype, adjustment.datecolumn, exdateRow, CorpAction.EventType)
                    End If

                    progressValue = progressValue + progressIncremant
                    bc_cs_central_settings.progress_bar.increment(adjustment.adjustshort, progressValue)
                End If
            Next


            REM Check dates to make sure the date columns are correct. 
            'If CorpAction.EventType = "ALLHIST" Then
            LoadRow = 13
            loadColumnOrigonal = 0
            loadColumnAdjustment = cellCount + 1
            For i = 0 To CorpAction.Adjustments.eventactions.Count - 1
                adjustment = CorpAction.Adjustments.eventactions(i)

                If adjustment.datatype = "time series" Then

                    If adjustment.datecolumn = 0 Then
                        loadColumnOrigonal = loadColumnOrigonal + 1
                        loadColumnAdjustment = loadColumnAdjustment + 1
                    Else
                        loadColumnOrigonal = loadColumnOrigonal + 2
                        loadColumnAdjustment = loadColumnAdjustment + 2
                    End If

                    If adjustment.datecolumn = 1 Then
                        endBlockRow = GetEndRow(LoadRow, loadColumnOrigonal - 1)
                        exdateRow = GetExdateRow(LoadRow, loadColumnOrigonal, exdateRow, CorpAction)
                        workRow = LoadRow
                        While workRow < endBlockRow
                            If CStr(exceldoc.activesheet.cells(workRow, loadColumnAdjustment - 1).value) = "" Then
                                If IsDate(exceldoc.activesheet.cells(workRow, loadColumnOrigonal - 1).value) Then
                                    exceldoc.activesheet.cells(workRow, loadColumnAdjustment - 1).value = CDate(exceldoc.activesheet.cells(workRow, loadColumnOrigonal - 1).value)
                                    If workRow <= exdateRow Then
                                        exceldoc.activesheet.cells(workRow, loadColumnAdjustment - 1).style = "CADataDate"
                                    Else
                                        exceldoc.activesheet.cells(workRow, loadColumnAdjustment - 1).style = "CADateAfter"
                                    End If

                                End If
                            End If
                            If exceldoc.activesheet.cells(workRow, loadColumnOrigonal - 1).style.name = "Normal" Then
                                If workRow <= exdateRow Then
                                    exceldoc.activesheet.cells(workRow, loadColumnOrigonal - 1).style = "CADataDate"
                                Else
                                    exceldoc.activesheet.cells(workRow, loadColumnOrigonal - 1).style = "CADateAfter"
                                End If
                            End If
                            workRow = workRow + 1
                        End While
                    End If
                End If
            Next
            'End If

            REM Steve Wooderson 21/09/2012
            REM Load period data
            REM --------------------------

            For i = 0 To CorpAction.Adjustments.eventactions.Count - 1
                adjustment = CorpAction.Adjustments.eventactions(i)
                If adjustment.datatype = "period time series" Then

                    If check_sheet_exists(CorpAction.Adjustments.eventactions(i).adjustshort) = True Then
                        exceldoc.application.ActiveWorkbook.sheets(CorpAction.Adjustments.eventactions(i).adjustshort).select()

                        exceldoc.ActiveSheet.Unprotect()
                        exceldoc.activesheet.Cells.Select()
                        exceldoc.application.Selection.Delete()
                        exceldoc.activesheet.Range("A1").Select()
                    Else
                        exceldoc.application.ActiveWorkbook.sheets(3).select()
                        exceldoc.application.ActiveWorkbook.Worksheets.add().name = CorpAction.Adjustments.eventactions(i).adjustshort
                    End If


                    'If exceldoc.activesheet.index > 3 Then
                    '    exceldoc.application.ActiveWorkbook.Worksheets.add()
                    'Else
                    '    exceldoc.ActiveSheet.Next.Select()
                    'End If

                    exceldoc.activesheet.cells(1, 1).value = exceldoc.Worksheets(2).cells(4, 2).value
                    exceldoc.activesheet.cells(1, 1).style = "CAHeading3"
                    exceldoc.activesheet.cells(2, 1).style = "CAHeading3"

                    If adjustment.displaytemplate <> "" Then
                        oio.run_template(adjustment.displaytemplate, ExcelError, False)

                        REM Format the data
                        endBlockColumn = GetEndColumn(6, 1)
                        endBlockRow = 0
                        For x = 1 To endBlockColumn Step 2

                            REM Format headers
                            exceldoc.activesheet.cells(3, x + 1).style = "CADataText"
                            exceldoc.activesheet.cells(4, x + 1).style = "CADataText"
                            exceldoc.activesheet.cells(5, x + 1).style = "CAOldTitle"
                            exceldoc.activesheet.cells(5, x).style = "CAOldTitle"

                            workRow = 6
                            While CStr(exceldoc.activesheet.cells(workRow, x).value) <> ""
                                exceldoc.activesheet.cells(workRow, x).style = "CADataDate"
                                exceldoc.activesheet.cells(workRow, x + 1).style = "CADataText"
                                workRow = workRow + 1
                                If workRow > endBlockRow Then
                                    endBlockRow = workRow
                                End If
                            End While
                        Next x

                        REM show adjusted Rows
                        endBlockColumn = GetEndColumn(6, 1)
                        workRow = 6
                        exceldoc.activesheet.cells(endBlockRow + 3, 1).value = "OFFSET"
                        exceldoc.activesheet.cells(endBlockRow + 3, 1).NumberFormat = ";;;"

                        For x = 1 To endBlockColumn Step 2

                            If CStr(exceldoc.activesheet.cells(3, x + 1).value) <> "" Then
                                exceldoc.activesheet.cells(endBlockRow + 3, x + 1).value() = exceldoc.activesheet.cells(3, x + 1).value()
                                exceldoc.activesheet.cells(endBlockRow + 3, x + 1).style = "CADataText"
                                exceldoc.activesheet.cells(endBlockRow + 4, x + 1).value() = exceldoc.activesheet.cells(4, x + 1).value()
                                exceldoc.activesheet.cells(endBlockRow + 4, x + 1).style = "CADataText"
                                exceldoc.activesheet.cells(endBlockRow + 5, x + 1).value() = exceldoc.activesheet.cells(5, x + 1).value()
                                exceldoc.activesheet.cells(endBlockRow + 5, x).style = "CAOldTitle"
                                exceldoc.activesheet.cells(endBlockRow + 5, x + 1).style = "CANewTitle"
                            End If
                            BuildCorpActionAdjustmentPeriod(6, endBlockRow, x, x, CorpAction.CalcAdjustment, adjustment.adjusttype, adjustment.datecolumn, exdateRow, CorpAction)
                        Next x

                    End If
                End If

                exceldoc.ActiveSheet.Protect(DrawingObjects:=True, Contents:=True, Scenarios:=True _
                , AllowFormattingColumns:=True, AllowFormattingRows:=True)
            Next

            exceldoc.sheets(2).select()
            bc_cs_central_settings.progress_bar.increment("Completing", 100)
            exceldoc.ActiveSheet.Protect(DrawingObjects:=True, Contents:=True, Scenarios:=True _
            , AllowFormattingColumns:=True, AllowFormattingRows:=True)

        Catch ex As Exception

            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "build_corp_action", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
            exceldoc.application.screenupdating = True
            bc_cs_central_settings.progress_bar.unload()
        End Try

    End Sub

    'Public Sub apply_corp_action_adjustment(ByVal startrow As Long, ByVal startcolumn As Long, ByVal adjfactor As Double, ByVal adjtype As Char)


    '    Dim workrow As Long
    '    Dim workcolumn As Long

    '    Try

    '        workrow = startrow
    '        workcolumn = startcolumn

    '        While CStr(exceldoc.activesheet.cells(workrow, startcolumn).value) <> ""
    '            If IsNumeric(CStr(exceldoc.activesheet.cells(workrow, startcolumn).value)) Then

    '                If adjtype = "/" Then
    '                    exceldoc.activesheet.cells(workrow, startcolumn).value = exceldoc.activesheet.cells(workrow, startcolumn).value / adjfactor
    '                End If
    '                If adjtype = "*" Then
    '                    exceldoc.activesheet.cells(workrow, startcolumn).value = exceldoc.activesheet.cells(workrow, startcolumn).value * adjfactor
    '                End If

    '            End If
    '            workrow = workrow + 1
    '        End While

    '    Catch ex As Exception

    '        Dim db_err As New bc_cs_error_log("bc_am_in_excel", "apply_corp_action_adjustment", bc_cs_error_codes.USER_DEFINED, ex.Message)

    '    End Try


    'End Sub


    Public Sub BuildCorpActionAdjustment(ByVal startRow As Long, ByVal endRow As Long, ByVal startColumn As Long, ByVal readColumn As Long, ByVal adjFactor As Double, ByVal adjType As Char, ByVal dateColumn As Integer, ByVal exdateRow As Long, ByVal eventType As String)

        Dim workRow As Long
        Dim workColumn As Long
        Dim formula As String

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try

            workRow = startRow
            workColumn = startColumn

            While workRow < endRow

                REM Adjust using formula
                If IsNumeric(CStr(exceldoc.activesheet.cells(workRow, readColumn).value)) And (adjType = "/" Or adjType = "*" Or adjType = "F" Or adjType = "X") Then

                    formula = ""
                    If adjType = "/" Then
                        formula = "=" & CStr(ConvertColToLetter(readColumn) & workRow) & "/" & "$B$8"
                    End If
                    If adjType = "*" Then
                        formula = "=" & CStr(ConvertColToLetter(readColumn) & workRow) & "*" & "$B$8"
                    End If
                    If adjType = "F" Then
                        formula = "=" & CStr(ConvertColToLetter(readColumn) & workRow)
                    End If

                    REM Fair Value
                    If adjType = "X" Then
                        formula = "=" & CStr(ConvertColToLetter(readColumn) & workRow) & "*" & "$B$9"
                    End If

                    exceldoc.activesheet.cells(workRow, readColumn).style = "CAOldValue"

                    If adjType = "/" Or adjType = "*" Or adjType = "F" Or adjType = "X" Then
                        If workRow <= exdateRow Then
                            exceldoc.activesheet.cells(workRow, workColumn).formula = formula
                            exceldoc.activesheet.cells(workRow, workColumn).style = "CANewValue"
                        Else
                            exceldoc.activesheet.cells(workRow, workColumn).value = exceldoc.activesheet.cells(workRow, readColumn).value
                            exceldoc.activesheet.cells(workRow, workColumn).style = "CAOldValue"
                        End If
                    End If

                    REM date colum
                    If dateColumn = 1 Then
                        If IsDate(exceldoc.activesheet.cells(workRow, readColumn - 1).value) Then
                            REM Fair Value
                            REM exceldoc.activesheet.cells(workRow, workColumn - 1).value = CDate(exceldoc.activesheet.cells(workRow, readColumn - 1).value)
                            If workRow <= exdateRow Then
                                formula = "=" & CStr(ConvertColToLetter(readColumn - 1) & workRow)
                                exceldoc.activesheet.cells(workRow, workColumn - 1).formula = formula
                            Else
                                exceldoc.activesheet.cells(workRow, workColumn - 1).value = CDate(exceldoc.activesheet.cells(workRow, readColumn - 1).value)
                            End If
                        End If
                        If workRow <= exdateRow Then
                            exceldoc.activesheet.cells(workRow, readColumn - 1).style = "CADataDate"
                            exceldoc.activesheet.cells(workRow, workColumn - 1).style = "CADataDate"
                        Else
                            exceldoc.activesheet.cells(workRow, readColumn - 1).style = "CADateAfter"
                            exceldoc.activesheet.cells(workRow, workColumn - 1).style = "CADateAfter"
                        End If
                    End If

                End If

                REM Text in formula col 
                If Not IsNumeric(CStr(exceldoc.activesheet.cells(workRow, readColumn).value)) And adjType = "*" Then
                    exceldoc.activesheet.cells(workRow, workColumn).value = exceldoc.activesheet.cells(workRow, readColumn).value
                    exceldoc.activesheet.cells(workRow, readColumn).style = "CAOldValue"
                    If workRow <= exdateRow Then
                        exceldoc.activesheet.cells(workRow, workColumn).style = "CANewValue"
                    Else
                        exceldoc.activesheet.cells(workRow, workColumn).style = "CAOldValue"
                    End If


                    REM date colum
                    If dateColumn = 1 Then
                        If IsDate(exceldoc.activesheet.cells(workRow, readColumn - 1).value) Then
                            exceldoc.activesheet.cells(workRow, workColumn - 1).value = CDate(exceldoc.activesheet.cells(workRow, readColumn - 1).value)
                        End If
                        If workRow <= exdateRow Then
                            exceldoc.activesheet.cells(workRow, readColumn - 1).style = "CADataDate"
                            exceldoc.activesheet.cells(workRow, workColumn - 1).style = "CADataDate"
                        Else
                            exceldoc.activesheet.cells(workRow, readColumn - 1).style = "CADateAfter"
                            exceldoc.activesheet.cells(workRow, workColumn - 1).style = "CADateAfter"
                        End If
                    End If
                End If


                REM Number of shares
                If IsNumeric(CStr(exceldoc.activesheet.cells(workRow, readColumn).value)) And adjType = "S" Then
                    exceldoc.activesheet.cells(workRow, readColumn).style = "CAOldValue"
                    If adjType = "S" Then
                        If workRow > exdateRow Then
                            formula = "=" & "$K$4"
                            exceldoc.activesheet.cells(workRow, workColumn).formula = formula
                            exceldoc.activesheet.cells(workRow, workColumn).style = "CANewValue"
                        Else
                            formula = "=" & CStr(ConvertColToLetter(readColumn) & workRow) & "/" & "$B$8"
                            exceldoc.activesheet.cells(workRow, workColumn).formula = formula
                            exceldoc.activesheet.cells(workRow, workColumn).style = "CANewValue"
                        End If
                    End If
                    REM date colum
                    If dateColumn = 1 Then
                        If IsDate(exceldoc.activesheet.cells(workRow, readColumn - 1).value) Then
                            exceldoc.activesheet.cells(workRow, workColumn - 1).value = CDate(exceldoc.activesheet.cells(workRow, readColumn - 1).value)
                        End If
                        If workRow <= exdateRow Then
                            exceldoc.activesheet.cells(workRow, readColumn - 1).style = "CADataDate"
                            exceldoc.activesheet.cells(workRow, workColumn - 1).style = "CADataDate"
                        Else
                            exceldoc.activesheet.cells(workRow, readColumn - 1).style = "CADateAfter"
                            exceldoc.activesheet.cells(workRow, workColumn - 1).style = "CADateAfter"
                        End If
                    End If
                End If


                REM Adjust Current Stock Value only
                If IsNumeric(CStr(exceldoc.activesheet.cells(workRow, readColumn).value)) And (adjType = "C" Or adjType = "O") Then
                    exceldoc.activesheet.cells(workRow, readColumn).style = "CAOldValue"

                    If adjType = "C" Then
                        If workRow > exdateRow Then
                            formula = "=" & "$K$4"
                            exceldoc.activesheet.cells(workRow, workColumn).formula = formula
                            exceldoc.activesheet.cells(workRow, workColumn).style = "CANewValue"
                        Else
                            formula = "=" & CStr(ConvertColToLetter(readColumn) & workRow)
                            exceldoc.activesheet.cells(workRow, workColumn).formula = formula
                            exceldoc.activesheet.cells(workRow, workColumn).style = "CAOldValue"
                        End If
                    End If

                    If adjType = "O" Then
                        exceldoc.activesheet.cells(workRow, workColumn).value = exceldoc.activesheet.cells(workRow, readColumn).value
                        exceldoc.activesheet.cells(workRow, workColumn).style = "CAOldValue"
                    End If

                    REM date colum
                    If dateColumn = 1 Then
                        If IsDate(exceldoc.activesheet.cells(workRow, readColumn - 1).value) Then
                            exceldoc.activesheet.cells(workRow, workColumn - 1).value = CDate(exceldoc.activesheet.cells(workRow, readColumn - 1).value)
                            exceldoc.activesheet.cells(workRow, readColumn - 1).style = "CADataDate"
                            exceldoc.activesheet.cells(workRow, workColumn - 1).style = "CADataDate"
                        End If
                    End If

                End If

                REM Adjust value. No formula.
                If adjType = "V" Then
                    If IsNumeric(exceldoc.activesheet.cells(workRow, readColumn).value) Then
                        exceldoc.activesheet.cells(workRow, readColumn).style = "CAOldValue"
                        If workRow <= exdateRow Then
                            exceldoc.activesheet.cells(workRow, workColumn).value = exceldoc.activesheet.cells(workRow, readColumn).value
                            exceldoc.activesheet.cells(workRow, workColumn).style = "CANewValue"
                        Else
                            exceldoc.activesheet.cells(workRow, workColumn).value = exceldoc.activesheet.cells(workRow, readColumn).value
                            exceldoc.activesheet.cells(workRow, workColumn).style = "CAOldValue"
                        End If
                    Else
                        exceldoc.activesheet.cells(workRow, workColumn).value = exceldoc.activesheet.cells(workRow, readColumn).value
                        exceldoc.activesheet.cells(workRow, readColumn).style = "CAOldValue"
                        If workRow <= exdateRow Then
                            exceldoc.activesheet.cells(workRow, workColumn).style = "CANewValue"
                        Else
                            exceldoc.activesheet.cells(workRow, workColumn).style = "CAOldValue"
                        End If
                    End If

                    REM date colum
                    If dateColumn = 1 Then
                        If IsDate(exceldoc.activesheet.cells(workRow, readColumn - 1).value) Then
                            exceldoc.activesheet.cells(workRow, workColumn - 1).value = CDate(exceldoc.activesheet.cells(workRow, readColumn - 1).value)
                        End If
                        If workRow <= exdateRow Then
                            exceldoc.activesheet.cells(workRow, readColumn - 1).style = "CADataDate"
                            exceldoc.activesheet.cells(workRow, workColumn - 1).style = "CADataDate"
                        Else
                            exceldoc.activesheet.cells(workRow, readColumn - 1).style = "CADateAfter"
                            exceldoc.activesheet.cells(workRow, workColumn - 1).style = "CADateAfter"
                        End If
                    End If
                End If

                REM Adjust value. No formula. No exdate locking
                If adjType = "T" Then
                    If IsNumeric(exceldoc.activesheet.cells(workRow, readColumn).value) Then
                        exceldoc.activesheet.cells(workRow, readColumn).style = "CAOldValue"
                        exceldoc.activesheet.cells(workRow, workColumn).value = exceldoc.activesheet.cells(workRow, readColumn).value
                        exceldoc.activesheet.cells(workRow, workColumn).style = "CANewValue"
                    Else
                        exceldoc.activesheet.cells(workRow, workColumn).value = exceldoc.activesheet.cells(workRow, readColumn).value
                        exceldoc.activesheet.cells(workRow, readColumn).style = "CAOldValue"
                        exceldoc.activesheet.cells(workRow, workColumn).style = "CANewValue"
                    End If

                    REM date colum
                    If dateColumn = 1 Then
                        If IsDate(exceldoc.activesheet.cells(workRow, readColumn - 1).value) Then
                            exceldoc.activesheet.cells(workRow, workColumn - 1).value = CDate(exceldoc.activesheet.cells(workRow, readColumn - 1).value)
                        End If
                        exceldoc.activesheet.cells(workRow, readColumn - 1).style = "CADataDate"
                        REM exceldoc.activesheet.cells(workRow, workColumn - 1).style = "CADataDate"
                        exceldoc.activesheet.cells(workRow, workColumn - 1).style = "CADataDateAdjust"
                    End If
                End If

                'End If
                workRow = workRow + 1

            End While


            'For corporate Actions only
            'Handle values if exdate is in the future. 
            If workRow - 1 = exdateRow And eventType <> "ALLHIST" Then

                If dateColumn = 1 And adjType <> "X" Then
                    If IsDate(exceldoc.activesheet.cells(5, 5).value) Then
                        exceldoc.activesheet.cells(workRow, workColumn - 1).value = CDate(exceldoc.activesheet.cells(5, 5).value)
                    End If
                    exceldoc.activesheet.cells(workRow, workColumn - 1).style = "CADateAfter"
                End If


                If adjType = "/" Or adjType = "*" Or adjType = "F" Or adjType = "V" Or adjType = "O" Then
                    'exceldoc.activesheet.cells(workRow, workColumn).formula = 0
                    exceldoc.activesheet.cells(workRow, workColumn).style = "CAOldValue"
                End If

                If adjType = "S" Then
                    formula = "=" & "$K$4"
                    exceldoc.activesheet.cells(workRow, workColumn).formula = formula
                    exceldoc.activesheet.cells(workRow, workColumn).style = "CANewValue"
                End If

                If adjType = "C" Then
                    formula = "=" & "$K$4"
                    exceldoc.activesheet.cells(workRow, workColumn).formula = formula
                    exceldoc.activesheet.cells(workRow, workColumn).style = "CANewValue"
                End If
            End If

            'REM Cludge to put a date in when value is in the future
            'If exceldoc.activesheet.cells(workRow, dateColumn).value <> "" And eventType = "ALLHIST" Then
            '    exceldoc.activesheet.cells(workRow, dateColumn).style = "CADateAfter"
            '    exceldoc.activesheet.cells(workRow, startColumn).value = CDate(exceldoc.activesheet.cells(workRow, dateColumn).value)
            '    exceldoc.activesheet.cells(workRow, startColumn).style = "CADateAfter"
            'End If

            'If workRow - 1 = exdateRow And eventType = "ALLHIST" Then
            '    If IsDate(exceldoc.activesheet.cells(5, 5).value) Then
            '        exceldoc.activesheet.cells(workRow, workColumn - 1).value = CDate(exceldoc.activesheet.cells(5, 5).value)
            '    End If
            '    exceldoc.activesheet.cells(workRow, workColumn - 1).style = "CADateAfter"
            '    REM exceldoc.activesheet.cells(workRow, workColumn - 1).style = "CADataDateAdjust"
            '    exceldoc.activesheet.cells(workRow, workColumn).style = "CANewValue"
            'End If

        Catch ex As Exception

            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "apply_corp_action_adjustment", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try


    End Sub


    Public Sub BuildCorpActionAdjustmentPeriod(ByVal readRow As Long, ByVal targetRowOffset As Long, ByVal targetColumn As Long, ByVal readColumn As Long, ByVal adjFactor As Double, ByVal adjType As Char, ByVal dateColumn As Integer, ByVal exdateRow As Long, ByVal corpAction As bc_om_corp_action)

        Dim workRow As Long
        Dim workColumn As Long
        Dim formula As String
        Dim periodExdateRow As Long

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try

            REM find period Exdate Row
            periodExdateRow = readRow
            workRow = readRow
            workColumn = targetColumn
            While CStr(exceldoc.activesheet.cells(workRow, readColumn).value) <> ""
                If exceldoc.activesheet.cells(workRow, readColumn).value < corpAction.ActionExDate Then
                    periodExdateRow = workRow
                End If
                workRow = workRow + 1
            End While

            workRow = readRow
            workColumn = targetColumn

            While CStr(exceldoc.activesheet.cells(workRow, readColumn).value) <> ""

                exceldoc.activesheet.cells(workRow + targetRowOffset, workColumn).value = exceldoc.activesheet.cells(workRow, readColumn).value
                exceldoc.activesheet.cells(workRow + targetRowOffset, workColumn).style = "CADataDate"

                If IsNumeric(CStr(exceldoc.activesheet.cells(workRow, readColumn + 1).value)) And (adjType = "/" Or adjType = "*" Or adjType = "F" Or adjType = "X") Then

                    formula = ""
                    If adjType = "/" Then
                        formula = "=" & CStr(ConvertColToLetter(readColumn + 1) & workRow) & "/" & "Details!$B$8"
                    End If
                    If adjType = "*" Then
                        formula = "=" & CStr(ConvertColToLetter(readColumn + 1) & workRow) & "*" & "Details!$B$8"
                    End If
                    If adjType = "F" Then
                        formula = "=" & CStr(ConvertColToLetter(readColumn + 1) & workRow)
                    End If

                    REM Fair Value
                    If adjType = "X" Then
                        formula = "=" & CStr(ConvertColToLetter(readColumn + 1) & workRow) & "*" & "Details!$B$9"
                    End If


                    If adjType = "/" Or adjType = "*" Or adjType = "F" Or adjType = "X" Then
                        If workRow <= periodExdateRow Then
                            exceldoc.activesheet.cells(workRow + targetRowOffset, workColumn + 1).formula = formula
                            exceldoc.activesheet.cells(workRow + targetRowOffset, workColumn + 1).style = "CANewValue"
                        Else
                            exceldoc.activesheet.cells(workRow + targetRowOffset, workColumn + 1).value = exceldoc.activesheet.cells(workRow, readColumn + 1).value
                            exceldoc.activesheet.cells(workRow + targetRowOffset, workColumn + 1).style = "CAOldValue"
                        End If
                    End If
                End If

                workRow = workRow + 1
            End While



        Catch ex As Exception

            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "BuildCorpActionAdjustmentPeriod", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try


    End Sub

    Public Function GetEndRow(ByVal startRow As Long, ByVal startColumn As Long) As Long

        Dim workRow As Long
        Dim workColumn As Long

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try

            workRow = startRow
            workColumn = startColumn

            While CStr(exceldoc.activesheet.cells(workRow, startColumn).value) <> ""
                workRow = workRow + 1
            End While

            GetEndRow = workRow

        Catch ex As Exception

            GetEndRow = 0

            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "get_end_row", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try

    End Function

    Public Function GetEndColumn(ByVal startRow As Long, ByVal startColumn As Long) As Long

        Dim workRow As Long
        Dim workColumn As Long

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try

            workRow = startRow
            workColumn = startColumn

            While CStr(exceldoc.activesheet.cells(startRow, workColumn).value) <> "" Or CStr(exceldoc.activesheet.cells(startRow, workColumn + 1).value) <> ""
                workColumn = workColumn + 1
            End While

            GetEndColumn = workColumn - 2

        Catch ex As Exception

            GetEndColumn = 0

            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "GetEndColumn", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try

    End Function



    Public Function GetExdateRow(ByVal startRow As Long, ByVal startColumn As Long, ByVal oldExdateRow As Long, ByVal corpAction As bc_om_corp_action) As Long

        REM Check for the last row before the action exdate

        Dim workrow As Long
        Dim workcolumn As Long

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try

            workrow = startRow
            workcolumn = startColumn
            GetExdateRow = oldExdateRow

            While CStr(exceldoc.activesheet.cells(workrow, startColumn).value) <> ""
                If exceldoc.activesheet.cells(workrow, startColumn - 1).value < corpAction.ActionExDate Then
                    GetExdateRow = workrow
                End If
                workrow = workrow + 1
            End While

        Catch ex As Exception

            GetExdateRow = oldExdateRow

            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "get_exdate_row", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try

    End Function


    Public Function GetActionId() As Long

        Dim oldCI As System.Globalization.CultureInfo = _
                                       System.Threading.Thread.CurrentThread.CurrentCulture

        System.Threading.Thread.CurrentThread.CurrentCulture = _
                  New System.Globalization.CultureInfo("en-GB")

        Try

            exceldoc.sheets(2).select()

            If IsNumeric(exceldoc.activesheet.cells(3, 2).value) And exceldoc.activesheet.cells(1, 1).value = "Corporate Action Adjustment" Then
                GetActionId = exceldoc.activesheet.cells(3, 2).value
            Else
                GetActionId = 0
            End If

        Catch ex As Exception
            GetActionId = 0
        Finally
            System.Threading.Thread.CurrentThread.CurrentCulture = oldCI
        End Try

    End Function

    Private Function ConvertColToLetter(ByVal icol As Integer) As String

        Dim iAlpha As Integer
        Dim iRemainder As Integer

        ConvertColToLetter = ""

        iAlpha = Int(icol / 27)
        iRemainder = icol - (iAlpha * 26)
        If iAlpha > 0 Then
            ConvertColToLetter = Chr(iAlpha + 64)
        End If
        If iRemainder > 0 Then
            ConvertColToLetter = ConvertColToLetter & Chr(iRemainder + 64)
        End If

        'If col < 91 Then
        '    ConvertColToLetter = Chr(64 + col)
        'Else
        '    ConvertColToLetter = "A" & Chr(64 + (col - 90))
        'End If

    End Function

    Private Function CheckFormatFileExists(ByVal fn As String) As Boolean

        Dim fs As New bc_cs_file_transfer_services
        CheckFormatFileExists = False
        If fs.check_document_exists(bc_cs_central_settings.local_template_path + fn) Then
            CheckFormatFileExists = True
        End If

    End Function

    Public Overrides Sub SubmitCorpAction(ByRef CorpAction As bc_om_corp_action)

        Dim adjustment As bc_om_eventaction = Nothing
        Dim ExcelError As New bc_om_excel_data_element_errors

        Dim progressIncremant As Long = 0
        Dim progressValue As Long = 0
        Dim slog As New bc_cs_activity_log("bc_ao_in_excel", "submit_corp_action", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim errorsFound As Boolean

        bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve insight", "Submitting Workbook", 5, False, True)
        progressValue = 5
        If CorpAction.Adjustments.eventactions.Count > 0 Then
            progressIncremant = System.Math.Round(90 / CorpAction.Adjustments.eventactions.Count)
        End If

        Try

            REM  Show origonal time series data
            Dim oio As New bc_am_excel_data_io(exceldoc, "EXCEL")

            For i = 0 To CorpAction.Adjustments.eventactions.Count - 1

                adjustment = CorpAction.Adjustments.eventactions(i)

                If adjustment.datatype = "time series" Then
                    If adjustment.loadtemplate <> "" Then
                        'Dim oio As New bc_am_excel_data_io(exceldoc, "EXCEL")
                        oio.run_template(adjustment.loadtemplate, ExcelError, True)
                    End If
                End If

                REM 01/11/2012 steve wooderson period data
                If adjustment.datatype = "period time series" Then
                    If adjustment.loadtemplate <> "" Then
                        oio.run_template(adjustment.loadtemplate, ExcelError, True)
                    End If

                End If

                progressValue = progressValue + progressIncremant
                bc_cs_central_settings.progress_bar.increment(adjustment.adjustshort, progressValue)
            Next
            oio.show_errors(ExcelError)

            errorsFound = False
            For i = 0 To ExcelError.errors.Count - 1
                If ExcelError.errors(i).type <> "success" Then
                    errorsFound = True
                End If
            Next

            If errorsFound = False Then
                CorpAction.Submitted = 1
            End If

            bc_cs_central_settings.progress_bar.increment("Completing", 100)

        Catch ex As Exception

            Dim db_err As New bc_cs_error_log("bc_am_in_excel", "build_corp_action", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            bc_cs_central_settings.progress_bar.unload()

        End Try

    End Sub

    Public Sub bc_array_excel_export(ByVal exportlist As Array, Optional text As Boolean = False, Optional title As String = "Aggregation Audit Information")
        Try
            Dim row As Long
            Dim col As Long

            exceldoc.activesheet.cells(1, 1).value = title

            row = 3
            col = 1

            For i = 0 To UBound(exportlist, 1)
                For j = 0 To UBound(exportlist, 2)
                    Try
                        If text = False Then
                            exceldoc.activesheet.cells(row + i, j + col).value = exportlist(i, j).ToString
                        Else
                            exceldoc.activesheet.cells(row + i, j + col) = exportlist(i, j).ToString
                        End If
                    Catch

                    End Try
                Next j
            Next i
        Catch ex As Exception

            Dim oerr As New bc_cs_error_log("bc_ao_in_excel", "bc_array_excel_export", bc_cs_error_codes.USER_DEFINED, ex.Message)

        End Try
    End Sub





    REM class to hold headers temporily so can be sorted
    Private Class tmp_period_headers
        Public headers As New ArrayList
        REM sorts array list
        Public Sub sort()
            Dim i, j As Integer

            For i = 0 To headers.Count - 2
                j = i
                While j >= 0
                    If CInt(Left(headers(j + 1).year, 4)) < CInt(Left(headers(j).year, 4)) Then
                        swap(j)
                    End If
                    j = j - 1
                End While

            Next

        End Sub
        Private Sub swap(ByVal i)
            Dim tmp As Object
            tmp = headers(i)
            headers(i) = headers(i + 1)
            headers(i + 1) = tmp
        End Sub
    End Class
    Private Class tmp_period_header
        Public year As String
        Public period As String
        Public acc As String
        Public year_formula As String
        Public e_a_formula As String
        Public period_formula As String
        Public acc_formula As String
        Public enable_disable_formula As String
        Public Sub New(ByVal year As String, ByVal period As String, ByVal acc As String, ByVal year_formula As String, ByVal e_a_formula As String, ByVal period_formula As String, ByVal acc_formula As String, ByRef headers As tmp_period_headers, enable_disable_formula As String)
            Dim i As Integer
            REM only assign if not already there
            For i = 0 To headers.headers.Count - 1
                With headers.headers(i)
                    If .year = year And .period = period Then
                        If .acc <> "" And .acc = acc Then
                            Exit Sub
                        End If
                        If .acc = "" Then
                            Exit Sub
                        End If
                    End If
                End With
            Next
            Me.year = year
            Me.period = period
            Me.acc = acc
            Me.year_formula = year_formula
            Me.period_formula = period_formula
            Me.e_a_formula = e_a_formula
            Me.acc_formula = acc_formula
            Me.enable_disable_formula = enable_disable_formula


            headers.headers.Add(Me)
        End Sub
    End Class

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub

End Class
REM ==========================================
REM Bluecurve Limited 2005
REM Module:       Insight Application Object 
REM Type:         Application Object generic
REM               Generic classes for insight
REM               independent of output object
REM Description:  
REM Version:      1
REM Change history
REM ==========================================
Public MustInherit Class bc_ao_in_object
    REM insight excel contribution
    Public Const EXCEL_DOC = "excel"
    Public cell_errors As New bc_om_cell_errors
    Public Overridable Sub status_bar_text(ByVal text As String)


    End Sub
    Public Overridable Sub set_items()

    End Sub
    Public Overridable Sub protect_sheet()

    End Sub
    Public Overridable Sub unprotect_sheet()

    End Sub
    Public Overridable Sub delete_system_sheets()

    End Sub
    Public Overridable Sub screen_updating(ByVal off As Boolean)

    End Sub
    Public Overridable Sub set_caption(ByVal str As String)

    End Sub
    Public Overridable Sub close()

    End Sub
    Public Overridable Sub follow_link(ByVal cell As String, ByVal comment As String)

    End Sub
    Public Overridable Function save(ByVal filename As String, ByVal read_only As Boolean) As Boolean

    End Function
    Public Overridable Sub disable_application_alerts()

    End Sub
    Public Overridable Function get_sheet_keys() As String
        Return ""
    End Function
    Public Overridable Sub hide_worksheets()

    End Sub
    Public Overridable Sub show_worksheets()

    End Sub

    Public Overridable Sub populate_sheet(ByRef bc_om_is As bc_om_insight_sheet)

    End Sub
    Public Overridable Sub addsheet(ByVal entity_id As Long, ByVal entity_name As String, ByVal class_id As Long, ByVal class_name As String, ByVal contributor_id As String, ByVal contributor_name As String, ByVal sheet_name As String, ByVal template_id As Long)

    End Sub
    Public Overridable Sub set_property(ByVal name As String, ByVal value As String)

    End Sub
    Public Overridable Function get_property(ByVal name As String) As String
        Return ""
    End Function
    Public Overridable Function selectsheet(ByVal contributor_id As Long, ByVal entity_id As Long) As Integer

    End Function
    Public Overridable Sub highlight_cell(ByVal sheetname As String, ByVal row As Integer, ByVal col As Integer, ByVal comment As String, ByVal follow_link As Boolean)

    End Sub
    Public Overridable Sub enable_application_alerts()

    End Sub
    Public Overridable Function get_linked_comment(ByVal formula As String) As String
        Return ""
    End Function
    Public Overridable Function get_cell_comment(ByVal sheetname As String, ByVal row As Integer, ByVal col As Integer) As String
        Return ""
    End Function
    Friend Overridable Sub loadheaderdatafromsheet(ByRef header_values As header_values_for_sheet, ByRef insight_sheet As bc_om_insight_sheet)

    End Sub
    Protected Overridable Function getsheetnofromname(ByVal name As String) As Integer

    End Function
    Protected Overridable Sub validate_failed(ByVal msg As String, ByVal row As Integer, ByVal col As Integer, ByVal sheet As String, ByVal comment_req As String)
        'Dim omessage As New bc_cs_message("BlueCurve Insight", msg + " at cell(" + CStr(row) + "," + CStr(col) + " on sheet: " + sheet + ")", bc_cs_message.MESSAGE)
        Dim ocommentary As New bc_cs_activity_log("bc_ao_in_object", "validate_failed", bc_cs_activity_codes.COMMENTARY, "Sheet Validation Failed: " + msg + " at cell(" + CStr(row) + "," + CStr(col) + " on sheet: " + sheet + ")")
        Dim cell_error As New bc_om_cell_error(msg, row, col, sheet, comment_req)
        cell_errors.bc_om_cell_errors.Add(cell_error)
    End Sub
    Protected Overridable Sub clear_validation_comments(ByVal sheetno As Integer, ByVal row As Integer, ByVal col As Integer)

    End Sub
    Protected Overridable Sub load_data_from_sheet(ByRef insight_sheet As bc_om_insight_sheet, ByVal output_only As Boolean)

    End Sub
    Public Sub link_workbook(ByVal with_header As Boolean)
        Dim slog = New bc_cs_activity_log("bc_ao_in_object", "link", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            get_cell_formulas(with_header)
            get_cell_formulas_static()
            write_out_formula(with_header)
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_object", "link", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ao_in_object", "link", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub
    Protected Function get_row_for_insertion(ByVal label_code, ByRef section_id, ByRef row_id, ByRef insight_sheet, ByRef row, ByRef scale_factor, ByRef submission_code, ByRef flexible_label_flag) As Boolean
        'Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "get_row_for_insertion", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim i, j As Integer
            get_row_for_insertion = False
            For i = 0 To insight_sheet.bc_om_insightsections.Count - 1
                With insight_sheet.bc_om_insightsections(i)
                    For j = 0 To .rows.count - 1
                        If .rows(j).label_code = label_code Then
                            section_id = i
                            row_id = j
                            row = .rows(j).row_id
                            If Not IsNumeric(.rows(j).scale_factor) Then
                                scale_factor = 1
                            Else
                                scale_factor = .rows(j).scale_factor
                            End If
                            submission_code = 0
                            flexible_label_flag = .rows(j).flexible_label_flag
                            get_row_for_insertion = .rows(j).change_validation_flag
                            Exit Function
                        End If
                    Next
                End With
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_object", "get_row_for_insertion", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            'slog = New bc_cs_activity_log("bc_ao_in_excel", "get_row_for_insertion", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Protected Function get_row_for_insertion_static(ByVal label_code As String, ByRef section_id As Integer, ByRef row_id As Integer, ByRef insight_sheet As bc_om_insight_sheet, ByRef row As Long, ByRef scale_factor As Double, ByRef submission_code As Integer, ByRef flexible_label_flag As String) As Boolean
        Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "get_row_for_insertion", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim i, j As Integer

        Try

            get_row_for_insertion_static = False
            For i = 0 To insight_sheet.bc_om_insightsections_static.Count - 1
                With insight_sheet.bc_om_insightsections_static(i)
                    For j = 0 To .rows.count - 1
                        If .rows(j).label_code = label_code Then
                            section_id = i
                            row_id = j
                            row = .rows(j).row_id
                            If Not IsNumeric(.rows(j).scale_factor) Then
                                scale_factor = 1
                            Else
                                scale_factor = .rows(j).scale_factor
                            End If
                            submission_code = .rows(j).submission_code
                            flexible_label_flag = .rows(j).flexible_label_flag
                            get_row_for_insertion_static = .rows(j).change_validation_flag
                            Exit Function
                        End If
                    Next
                End With
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_object", "get_row_for_insertion_static", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            slog = New bc_cs_activity_log("bc_ao_in_excel", "get_row_for_insertion_static", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Overridable Sub delete_preview_sheet(ByVal sheet_name As String)

    End Sub
    Public Overridable Function tmp_saveas(ByVal filename As String) As String
        Return ""
    End Function
    Protected Sub assign_value_for_label_code(ByVal section As Integer, ByVal row As Integer, ByRef value As bc_om_insight_rows_cell_value, ByRef insight_sheet As bc_om_insight_sheet, ByVal flexible_label As String, ByVal output_only As Boolean)
        'Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "assign_value_for_label_code", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            With insight_sheet.bc_om_insightsections(section).rows(row)
                If .flexible_label_flag = True Then
                    Dim fs As New bc_cs_string_services(flexible_label)
                    value.flexible_label_value = fs.delimit_apostrophies
                End If
                REM dont load datatype 6 as this ios outpout only
                If output_only = True And CStr(.datatype) = "6" Then
                    .values_list.add(value)
                End If
                If output_only = False And CStr(.datatype) <> "6" Then
                    .values_list.add(value)
                Else
                    Dim ocommentary As New bc_cs_activity_log("bc_ao_in_object", "assign_value_for_label_code", bc_cs_activity_codes.COMMENTARY, "Row: " + CStr(.label_code) + " not loaded as output only.")
                End If
            End With
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_object", "assign_value_for_label_code", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            'slog = New bc_cs_activity_log("bc_ao_in_excel", "assign_value_for_label_code", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Overridable Function open_clone(ByVal fn As String, ByVal caption As String) As Object
        open_clone = Nothing
    End Function
    Protected Sub assign_value_for_label_code_static(ByVal section As Integer, ByVal row As Integer, ByRef value As bc_om_insight_rows_cell_value, ByRef insight_sheet As bc_om_insight_sheet, ByVal flexible_label As String, ByVal output_only As Boolean)
        'Dim slog = New bc_cs_activity_log("bc_ao_in_excel", "assign_value_for_label_code", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            With insight_sheet.bc_om_insightsections_static(section).rows(row)
                If .flexible_label_flag = True Then
                    Dim fs As New bc_cs_string_services(flexible_label)
                    value.flexible_label_value = fs.delimit_apostrophies
                End If
                If output_only = True And CStr(.datatype) = "6" Then
                    .values_list.add(value)
                End If
                If output_only = False And CStr(.datatype) <> "6" Then
                    .values_list.add(value)
                Else
                    Dim ocommentary As New bc_cs_activity_log("bc_ao_in_object", "assign_value_for_label_code_static", bc_cs_activity_codes.COMMENTARY, "Row: " + CStr(.label_code) + " not loaded as output only.")
                End If

            End With

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_object", "assign_value_for_label_cod", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            'slog = New bc_cs_activity_log("bc_ao_in_excel", "assign_value_for_label_code", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Friend Function validateheaderdata(ByVal headerdata As header_values) As Boolean
        Dim slog = New bc_cs_activity_log("bc_ao_in_object", "validateheaderdata", bc_cs_activity_codes.TRACE_ENTRY, "")

        Try
            Dim i, j, k As Integer
            Dim suffix As String
            Dim year As String
            Dim sheet_no As Integer
            Dim period As String
            validateheaderdata = True
            status_bar_text("Validating Headers...")
            For k = 0 To headerdata.header_values_for_sheet.Count - 1
                sheet_no = getsheetnofromname(headerdata.header_values_for_sheet(k).sheet_name)
                Dim ocommentary As New bc_cs_activity_log("bc_ao_in_object", "validatedata", bc_cs_activity_codes.COMMENTARY, "Validating Sheet: " + headerdata.header_values_for_sheet(k).sheet_name)
                REM validate header's
                For i = 0 To headerdata.header_values_for_sheet(k).header_values_header.Count - 1
                    With headerdata.header_values_for_sheet(k).header_values_header(i)
                        REM check year
                        year = Left(.year, 4)
                        period = .period
                        REM header checks
                        If Not IsNumeric(year) Or Len(year) <> 4 Then
                            validate_failed("Incorrect Value for Year: " + CStr(.year), .row, .col, headerdata.header_values_for_sheet(k).sheet_name, "0")
                            validateheaderdata = False
                        ElseIf Len(.year) <> 5 Then
                            validate_failed("Year Must have Suffix A or E: " + CStr(.year), .row, .col, headerdata.header_values_for_sheet(k).sheet_name, "0")
                            validateheaderdata = False
                        Else
                            REM check suffix
                            suffix = Right(.year, 1)
                            If CStr(suffix) <> "A" And CStr(suffix) <> "E" Then
                                validate_failed("Incorrect Syntax for Year Suffix: " + CStr(.year), .row, .col, headerdata.header_values_for_sheet(k).sheet_name, "0")
                                validateheaderdata = False
                            End If
                        End If

                        REM need to check duplicates as well
                        If validateheaderdata = True Then
                            For j = 0 To headerdata.header_values_for_sheet(k).header_values_header.Count - 1
                                If j <> i Then
                                    If .acc = "" Then
                                        If .year = headerdata.header_values_for_sheet(k).header_values_header(j).year And .period = headerdata.header_values_for_sheet(k).header_values_header(j).period Then
                                            validate_failed("Duplicate Year-Period: " + CStr(.year), .row, .col, headerdata.header_values_for_sheet(k).sheet_name, "0")
                                            validateheaderdata = False
                                        End If
                                    Else
                                        If .year = headerdata.header_values_for_sheet(k).header_values_header(j).year And .period = headerdata.header_values_for_sheet(k).header_values_header(j).period And .acc = headerdata.header_values_for_sheet(k).header_values_header(j).acc Then
                                            validate_failed("Duplicate Year-Period-Accounting Standard: " + CStr(.year), .row, .col, headerdata.header_values_for_sheet(k).sheet_name, "0")
                                            validateheaderdata = False
                                        End If
                                    End If

                                End If
                            Next
                        End If
                        REM period checks
                        Dim kk As Integer
                        Dim valid As Boolean
                        valid = False
                        For kk = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.bc_om_result_types.bc_om_result_types.Count - 1
                            With bc_am_load_objects.obc_om_insight_contribution_for_entity.bc_om_result_types
                                If period = .bc_om_result_types(kk).description Then
                                    valid = True
                                End If
                            End With
                        Next
                        If valid = False Then
                            validate_failed("Incorrect Result Type: " + CStr(.period), .row + 1, .col, headerdata.header_values_for_sheet(k).sheet_name, "0")
                            validateheaderdata = False
                        End If
                        REM accoutning standard check
                        REM accounting standard
                        Dim acc As String
                        If .acc <> "" Then
                            valid = False
                            acc = .acc
                            For kk = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.accounting_standards.bc_om_accounting_standards.Count - 1
                                With bc_am_load_objects.obc_om_insight_contribution_for_entity.accounting_standards
                                    If acc = .bc_om_accounting_standards(kk).name Then
                                        valid = True
                                        Exit For
                                    End If
                                End With
                            Next
                            If valid = False Then
                                validate_failed("Incorrect Accounting Standard: " + CStr(.acc), .row - 1, .col, headerdata.header_values_for_sheet(k).sheet_name, "0")
                                validateheaderdata = False
                            End If
                        End If
                    End With
                Next
            Next

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_object", "validateheaderdata", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ao_in_object", "validateheaderdata", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Overridable Function get_key_value(ByVal row As Integer, ByVal sheet As String) As String
        get_key_value = ""
    End Function
    Public Overridable Sub get_cell_formulas_static()

    End Sub
    Public Overridable Sub write_out_formula(ByVal with_header As Boolean)

    End Sub
    Public Overridable Function get_sheet_name() As String
        Return ""
    End Function
    Public Overridable Sub populate_output_data(ByRef bc_om_is As bc_om_insight_sheet)

    End Sub
    Public Overridable Sub populate_header_errors(errors As ArrayList)

    End Sub

    Public Function populate_object_with_data(ByVal output_only As Boolean) As Boolean
        REM loop through metadata for each BC sheet
        Dim slog = New bc_cs_activity_log("bc_ao_in_object", "populate_object_with_data", bc_cs_activity_codes.TRACE_ENTRY, "")
        bc_cs_central_settings.progress_bar.increment("Validating Headers...")

        Try
            populate_object_with_data = False
            Dim obc_objects As New bc_am_load_objects
            Dim i As Integer
            Dim headervalues As New header_values
            REM Jul 2015
            REM pull sheets from object that dont exist in workbook

            Dim k As Integer = 0
            With bc_am_load_objects.obc_om_insight_contribution_for_entity
                While k < .insight_sheets.bc_om_insight_sheets.Count
                    If selectsheet(.contributor_id, .insight_sheets.bc_om_insight_sheets(k).entity_id) = -1 Then
                        bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.RemoveAt(k)
                        k = k - 1
                    End If
                    k = k + 1
                End While
            End With

            For i = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Count - 1
                Dim headervalues_for_sheet As New header_values_for_sheet
                With bc_am_load_objects.obc_om_insight_contribution_for_entity
                    Dim ocommentary As New bc_cs_activity_log("bc_ao_in_object", "populate_object_with_data", bc_cs_activity_codes.COMMENTARY, "Attempting to Select Sheet Contributor:" + CStr(.contributor_id) + " Entity:" + CStr(.insight_sheets.bc_om_insight_sheets(i).entity_id))
                    headervalues_for_sheet.sheet_name = (.insight_sheets.bc_om_insight_sheets(i).sheet_name)
                    selectsheet(.contributor_id, .insight_sheets.bc_om_insight_sheets(i).entity_id)
                    REM load header data from application object
                    loadheaderdatafromsheet(headervalues_for_sheet, .insight_sheets.bc_om_insight_sheets(i))
                End With
                headervalues.header_values_for_sheet.Add(headervalues_for_sheet)
            Next
            REM validate header data
            REM validate data
            If validateheaderdata(headervalues) = False Then
                bc_cs_central_settings.progress_bar.unload()
                Dim ocommentary = New bc_cs_activity_log("bc_ao_in_object", "populate_object_with_data", bc_cs_activity_codes.COMMENTARY, "Header Validation Failed.")
                If cell_errors.bc_om_cell_errors.Count > 0 Then
                    populate_header_errors(cell_errors.bc_om_cell_errors)
                End If
            Else
                REM header validation is correct so populate data
                bc_cs_central_settings.progress_bar.increment("Reading in Data...")
                Dim ocommentary = New bc_cs_activity_log("bc_ao_in_object", "populate_object_with_data", bc_cs_activity_codes.COMMENTARY, "Header Validation Successful.")
                For i = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Count - 1
                    With bc_am_load_objects.obc_om_insight_contribution_for_entity
                        ocommentary = New bc_cs_activity_log("bc_ao_in_object", "populate_object_with_data", bc_cs_activity_codes.COMMENTARY, "Attempting to Select Sheet Contributor:" + CStr(.contributor_id) + " Entity:" + CStr(.insight_sheets.bc_om_insight_sheets(i).entity_id))
                        selectsheet(.contributor_id, .insight_sheets.bc_om_insight_sheets(i).entity_id)
                        REM load header data from application object
                        load_data_from_sheet(.insight_sheets.bc_om_insight_sheets(i), output_only)
                    End With
                Next
                REM FIL 5.5 switch so this can be server side

                If bc_am_insight_formats.client_side_validation = 1 Then
                    bc_cs_central_settings.progress_bar.increment("Validating Data...")
                    If validate_cells() = False Then
                        bc_cs_central_settings.progress_bar.unload()
                        populate_header_errors(cell_errors.bc_om_cell_errors)
                        Return False
                    Else
                        Return True
                    End If
                Else
                    slog = New bc_cs_activity_log("bc_ao_in_object", "Client Side Validation turned off", bc_cs_activity_codes.TRACE_EXIT, "")
                    Return True
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_object", "populate_object_with_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ao_in_object", "populate_object_with_data", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Public Overridable Sub read_only(ByVal set_flag As Boolean)

    End Sub
    Public Class ao_rgb
        Public red As Byte
        Public blue As Byte
        Public green As Byte
    End Class
    Public Class ao_font
        Public name As String
        Public size As String
        Public italic As Boolean
        Public bold As Boolean
        Public colour As New ao_rgb
        Public Sub New()

        End Sub
    End Class

    Public Sub build_workbook(ByVal class_id As Long, ByVal entity_id As Long, ByVal contributor_id As Long)
        Dim slog As New bc_cs_activity_log("bc_am_in_ao", "build_workbook", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim i, j, k As Integer

            Dim repeat As Integer = 0
            With bc_am_load_objects.obc_om_insight_contribution_for_entity
                For i = 0 To .insight_sheets.bc_om_insight_sheets.Count - 1
                    With .insight_sheets.bc_om_insight_sheets(i)
                        addsheet(.class_id, .class_name, .entity_id, .entity_name, bc_am_load_objects.obc_om_insight_contribution_for_entity.contributor_id, bc_am_load_objects.obc_om_insight_contribution_for_entity.contributor_name, .sheet_name, .logical_template_id)
                    End With

                    If i > 0 Then
                        If .insight_sheets.bc_om_insight_sheets(i).class_id = bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets(i - 1).class_id Then
                            repeat = repeat + 1
                            With .insight_sheets.bc_om_insight_sheets(i)
                                For j = 0 To .bc_om_insightsections.count - 1
                                    For k = 0 To .bc_om_insightsections(j).rows.count - 1
                                        .bc_om_insightsections(j).rows(k).link_code = .bc_om_insightsections(j).rows(k).link_code + "_" + CStr(repeat)
                                    Next
                                Next
                                For j = 0 To .bc_om_insightsections_static.count - 1
                                    For k = 0 To .bc_om_insightsections_static(j).rows.count - 1
                                        .bc_om_insightsections_static(j).rows(k).link_code = .bc_om_insightsections_static(j).rows(k).link_code + "_" + CStr(repeat)
                                    Next
                                Next
                            End With

                        Else
                            repeat = 0
                        End If
                    End If
                    populate_sheet(.insight_sheets.bc_om_insight_sheets(i))
                Next
            End With
            select_master_sheet()

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_in_ao", "build_workbook", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
        slog = New bc_cs_activity_log("bc_am_in_ao", "build_workbook", bc_cs_activity_codes.TRACE_EXIT, "")
    End Sub
    Public Overridable Sub select_master_sheet()

    End Sub
    Private Function validate_cells() As Boolean
        Dim slog = New bc_cs_activity_log("bc_ao_in_object", "validate_cells", bc_cs_activity_codes.TRACE_ENTRY, "")
        Dim i, j, k, l, m As Integer
        Dim bc_validate_cell As bc_ao_cell_validator
        Dim msg As String
        Dim sheet_name As String
        Dim msgout As String
        Dim comment_req As String
        Try
            REM validate cell data
            cell_errors.bc_om_cell_errors.Clear()
            validate_cells = True
            For i = 0 To bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets.bc_om_insight_sheets.Count - 1
                With bc_am_load_objects.obc_om_insight_contribution_for_entity.insight_sheets
                    status_bar_text("Validating Data for Sheet: " + .bc_om_insight_sheets(i).sheet_name)
                    For j = 0 To .bc_om_insight_sheets(i).bc_om_insightsections.count - 1
                        sheet_name = .bc_om_insight_sheets(i).sheet_name
                        With .bc_om_insight_sheets(i).bc_om_insightsections(j)
                            For k = 0 To .rows.count - 1
                                REM validations for row
                                With .rows(k)
                                    For m = 0 To .values_list.count - 1
                                        If .values_list(m).current = True Then
                                            msg = ""
                                            msgout = ""
                                            comment_req = "0"
                                            For l = 0 To .validations.bc_om_cell_validation.count - 1
                                                REM validate each cell
                                                If Not IsNumeric(.datatype) Then
                                                    .datatype = "2"
                                                End If
                                                bc_validate_cell = New bc_ao_cell_validator(.values_list, m, .validations.bc_om_cell_validation(l).validation_id, .validations.bc_om_cell_validation(l).mode, .validations.bc_om_cell_validation(l).value1, .validations.bc_om_cell_validation(l).value2, False, CInt(.datatype))
                                                msgout = bc_validate_cell.validate()
                                                REM if returns failure call routine to place validation
                                                If msgout <> "Success" Then
                                                    msg = msg + " " + msgout
                                                    validate_cells = False
                                                    If .validations.bc_om_cell_validation(l).comment_req = 1 Then
                                                        comment_req = "1"
                                                    End If
                                                End If
                                            Next
                                            If msg <> "" Then
                                                validate_failed(.label + " " + msg, .values_list(m).row, .values_list(m).column, sheet_name, comment_req)
                                            End If
                                        End If
                                    Next
                                End With
                            Next
                        End With
                    Next
                    For j = 0 To .bc_om_insight_sheets(i).bc_om_insightsections_static.count - 1
                        sheet_name = .bc_om_insight_sheets(i).sheet_name
                        With .bc_om_insight_sheets(i).bc_om_insightsections_static(j)
                            For k = 0 To .rows.count - 1
                                REM validations for row
                                With .rows(k)

                                    For m = 0 To .values_list.count - 1
                                        If .values_list(m).current = True Then
                                            msg = ""
                                            msgout = ""
                                            comment_req = "0"
                                            For l = 0 To .validations.bc_om_cell_validation.count - 1
                                                REM validate each cell
                                                bc_validate_cell = New bc_ao_cell_validator(.values_list, m, .validations.bc_om_cell_validation(l).validation_id, .validations.bc_om_cell_validation(l).mode, .validations.bc_om_cell_validation(l).value1, .validations.bc_om_cell_validation(l).value2, True, .datatype)
                                                msgout = bc_validate_cell.validate()
                                                REM if returns failure call routine to place validation
                                                If msgout <> "Success" Then
                                                    msg = msg + " " + msgout
                                                    validate_cells = False
                                                    If .validations.bc_om_cell_validation(l).comment_req = 1 Then
                                                        comment_req = "1"
                                                    End If
                                                End If
                                            Next
                                            If msg <> "" Then
                                                validate_failed(.label + " " + msg, .values_list(m).row, 7, sheet_name, comment_req)
                                            End If
                                        End If
                                    Next
                                End With
                            Next
                        End With
                    Next
                End With
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_in_object", "validate_cells", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ao_in_object", "validate_cells", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Overridable Sub get_cell_formulas(ByVal with_header As Boolean)


    End Sub
    Public Overridable Sub write_upload_values(ByVal values As bc_om_insight_retrieve_values)

    End Sub
    Friend Class header_values
        Public header_values_for_sheet As New ArrayList

        Public Sub New()

        End Sub
    End Class
    Friend Class header_values_for_sheet
        Public header_values_header As New ArrayList
        Public sheet_name As String
    End Class
    Friend Class header_values_header
        Public year As String
        Public period As String
        Public acc As String
        Public linked_year As String
        Public linked_period As String
        Public linked_e_a As String
        Public linked_acc As String
        Public row As Long
        Public col As Long
        Public header_values As New ArrayList

        Public Sub New()

        End Sub
    End Class
    Public Overridable Function check_sheet_exists(ByVal sheet_name As String) As Boolean


    End Function
    Public Overridable Function is_workbook_open(ByVal fn) As Boolean

    End Function

    Public Overridable Sub set_key_value(ByVal row As Integer, ByVal col As Integer, ByVal sheet As String, ByVal value As String)

    End Sub
    Public Overridable Sub set_sheet_name(ByVal sheet As String, ByVal value As String, ByVal alt_value As String)

    End Sub
    Public Overridable Sub delete_int_sheet(ByVal sheet_name As String)

    End Sub

    Public Overridable Function issaved() As Boolean
        issaved = False
    End Function
    Public Overridable Function filename() As String
        filename = ""
    End Function
    Public Overridable Function insert_sheet(ByVal name As String, ByVal filename As String, Optional ByVal preview_mode As Boolean = False) As Boolean

    End Function
    Public Sub New()

    End Sub

    Public Overridable Sub BuildCorpAction(ByVal CorpAction As bc_om_corp_action)
        REM steve test

    End Sub
    Public Overridable Sub evaluate_list_boxes(ByVal class_id As Long, ByVal entity_id As Long, ByVal contributor_id As Long)

    End Sub
    Public Overridable Sub SubmitCorpAction(ByRef CorpAction As bc_om_corp_action)
        REM steve test

    End Sub

End Class
REM ==========================================
REM Bluecurve Limited 2005
REM Module:       Insight Application Object
REM               Excel specific
REM Type:         Application Object
REM Description:  Excel
REM Version:      1
REM Change history
REM ==========================================
Public Class bc_ao_cell_validator
    Dim value As String = ""
    Dim comment As String
    Dim oldvalue As String = ""
    Dim oldcomment As String = ""
    Dim validation_id As Integer
    Dim mode As Integer
    Dim value1 As Double
    Dim value2 As Double
    Dim datatype As Integer
    Public Sub New(ByRef values_list As ArrayList, ByVal item As Integer, ByVal validation_id As Integer, ByVal mode As Integer, ByVal value1 As String, ByVal value2 As String, ByVal static_flag As Boolean, ByVal datatype As Integer)
        Dim slog = New bc_cs_activity_log("bc_ao_cell_validator", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim year As String
            Dim period_id As String
            Dim k As Integer
            Me.value = values_list(item).value
            Me.comment = values_list(item).comment
            Me.validation_id = validation_id
            Me.mode = mode
            Me.value1 = value1
            Me.value2 = value2
            Me.datatype = datatype
            If mode = 1 Or mode = 2 Then
                If values_list.Count > 0 Then
                    If static_flag = False Then
                        REM search for last old value for this 
                        year = values_list(item).year
                        period_id = values_list(item).period_id
                        k = item - 1
                        While k > -1
                            If values_list(k).year = year And values_list(k).period_id = period_id And values_list(k).current = False Then
                                oldvalue = values_list(k).value
                                oldcomment = values_list(k).comment
                                Exit While
                            End If
                            k = k - 1
                        End While
                    Else
                        If item > 0 Then
                            oldvalue = values_list(item - 1).value
                            oldcomment = values_list(item - 1).comment
                        End If
                    End If
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_cell_validator", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_ao_cell_validator", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Sub
    Public Function validate() As String
        Dim slog = New bc_cs_activity_log("bc_ao_cell_validator", "validate", bc_cs_activity_codes.TRACE_ENTRY, "")

        validate = "Success"

        Try
            Dim ocommentary As New bc_cs_activity_log("bc_ao_cell_validator", "validate", bc_cs_activity_codes.COMMENTARY, "Validating Value: " + value + " against method: " + CStr(validation_id))

            Select Case validation_id
                Case 1
                    If value = "" Or value = " " Then
                        validate = "Value must be entered"
                    End If
                Case 2
                    If value <> "" Then
                        Select Case CInt(datatype)
                            Case 2
                                REM string
                            Case 1
                                REM boolean
                                If value <> "True" And value <> "False" And value <> "0" And value <> "1" Then
                                    validate = "value must be true or false"
                                End If
                            Case 0
                                REM numeric
                                If Not IsNumeric(value) Then
                                    validate = "Value Must be Numeric"
                                End If
                            Case 3
                                REM numeric
                                If Not IsDate(value) Then
                                    validate = "Value Must be a Date"
                                End If
                        End Select
                    End If
                    REM custom validations
                Case Is >= 3
                    Select Case mode
                        Case 1

                            REM changed irrespective of tolerance
                            If value <> oldvalue And oldvalue <> "" Then
                                If comment = "" Then
                                    validate = "value has changed from " + oldvalue + " to " + value + " Comment must be entered"
                                Else
                                    If comment = oldcomment Then
                                        validate = "value has changed from " + oldvalue + " to " + value + " Comment must be updated"
                                    End If
                                End If
                            End If
                        Case 2
                            REM tolerance checking
                            If IsNumeric(value) And IsNumeric(oldvalue) Then
                                Dim inc = Abs(CDbl(oldvalue) * (CDbl(value1) / 100))
                                If CDbl(value) < (CDbl(oldvalue) - inc) Or CDbl(value) > (CDbl(oldvalue) + inc) Then
                                    If comment = "" Then
                                        validate = "value has changed from " + CStr(oldvalue) + " to " + CStr(value) + " by more than tolerance of: " + CStr(value1) + "% Comment must be entered"
                                    Else
                                        If comment = oldcomment Then
                                            validate = "value has changed from " + CStr(oldvalue) + " to " + CStr(value) + " by more than tolerance of: " + CStr(value1) + "% Comment must be updated"
                                        End If
                                    End If
                                End If
                            End If
                        Case 3
                            REM range checking
                            If IsNumeric(value1) And IsNumeric(value2) Then
                                If CDbl(value) < CDbl(value1) Or CDbl(value) > CDbl(value2) Then
                                    validate = "value must be between: " + CStr(value1) + " and " + CStr(value2)
                                End If
                            End If
                    End Select
            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_ao_cell_validator", "validate", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            Dim ocommentary As bc_cs_activity_log
            If validate <> "Success" Then
                ocommentary = New bc_cs_activity_log("bc_ao_cell_validator", "Validate", bc_cs_activity_codes.COMMENTARY, "Validation failed for value: " + CStr(value) + " method: " + CStr(validation_id) + " " + validate)
            Else
                ocommentary = New bc_cs_activity_log("bc_ao_cell_validator", "Validate", bc_cs_activity_codes.COMMENTARY, "Validation succeeded for value: " + CStr(value) + " method: " + CStr(validation_id) + " " + validate)
            End If
            slog = New bc_cs_activity_log("bc_ao_cell_validator", "validate", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

End Class


