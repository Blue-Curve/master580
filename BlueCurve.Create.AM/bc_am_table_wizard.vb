Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM.Nbc_om_table_wizard
Imports BlueCurve.Core.OM.Nbc_om_pub_type_structures
Imports BlueCurve.Core.AS
Imports System.Runtime.InteropServices
Imports System.Drawing
Imports System.Windows.Forms




Public Class bc_am_table_wizard
    Implements Ibc_am_table_wizard
    Dim _table As New Cbc_am_table_wizard.bc_am_tw_table
    Dim _total_width As Double
    Dim loading As Boolean
    Dim _static_controls_count As Integer
    Dim _settings As bc_om_table_wizard
    'Dim _word As Object
    Const PTS_TO_PIXELS = 0.75
    Event insert_table(table As Cbc_am_table_wizard.bc_am_tw_table, style_library As Integer, table_width As Double, left_offset As Double, rel_to_margin As Boolean, col_dist As Cbc_am_table_wizard.bc_am_tw_table.COLUMN_WIDTH, save_excel As Boolean, do_save As Boolean) Implements Ibc_am_table_wizard.insert_table
    Event get_excel_selection(instance As Integer, update_data As Boolean, refresh As Boolean) Implements Ibc_am_table_wizard.get_excel_selection
    Event format_table(style_library As Integer) Implements Ibc_am_table_wizard.format_table
    Event delete_table() Implements Ibc_am_table_wizard.delete_table
    Event update_data() Implements Ibc_am_table_wizard.update_data

    Dim num_rows As Integer
    Dim num_cols As Integer
    Dim head_rows As Integer
    Dim label_cols As Integer
    Dim format_only As Boolean = False
    Const CM_POINTS = 28.3464
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()

        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")
        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Function load_view_update(settings As bc_om_table_wizard, table As Cbc_am_table_wizard.bc_am_tw_table, tx As String) Implements Ibc_am_table_wizard.load_view_update
        Try
          


            format_only = True
            _settings = settings
            Me.Height = 180
            Me.Text = "Blue Curve - Table Wizard Update Table"
            If tx <> "" Then
                Me.uxmsg.Visible = True
                Me.uxmsg.Text = tx
                Me.uxdata.Enabled = False
                Me.Height = 190 + Me.uxmsg.Height
            ElseIf table.has_data_source = True Then
                Me.uxdata.Enabled = True
                Me.uxdatasource.Visible = True
                Me.uxrefresh.Visible = False
                Me.uxdatasource.Text = "Workbook: " + table.pathname + " Sheet: " + table.sheetname + " Range: " + table.range
            End If

            Me.AutoScroll = False

            For i = 0 To _settings.style_libraries.Count - 1
                Me.uxlibrary.Properties.Items.Add(_settings.style_libraries(i).library_name)
            Next
            Me.uxlibrary.SelectedIndex = table.library
            If Me.uxlibrary.Properties.Items.Count < 2 Then
                Me.uxlibrary.Enabled = False
            End If
            If table.has_data_source = True Then
                Me.XtraTabControl1.TabPages.RemoveAt(1)
                Me.XtraTabControl1.TabPages.RemoveAt(1)
                Me.XtraTabControl1.TabPages.RemoveAt(2)
            Else
                Me.XtraTabControl1.TabPages.RemoveAt(0)
                Me.XtraTabControl1.TabPages.RemoveAt(0)
                Me.XtraTabControl1.TabPages.RemoveAt(0)
                Me.XtraTabControl1.TabPages.RemoveAt(1)
            End If
            Me.pupdate.Visible = True
            Me.pinsert.Visible = False


            load_view_update = True
        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Table Wizard Failed to Load View Format: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

        End Try
    End Function
    Public Function load_excel_preview(table As Cbc_am_table_wizard.bc_am_tw_table, never_saved As Boolean, requires_save As Boolean) Implements Ibc_am_table_wizard.load_excel_preview
        Try

            Me.Cursor = Cursors.WaitCursor
            loading = True
            _table = table

            If _table.has_title = True Then
                Me.chktitle.Checked = True
            End If
            If _table.has_sub_title = True Then
                Me.chksubtitle.Checked = True
            End If
            If _table.has_source = True Then
                Me.chksource.Checked = True
            End If


            loading = False
            Me.uxtitle.Width = _settings.table_width / PTS_TO_PIXELS

            Me.uxsubtitle.Width = _settings.table_width / PTS_TO_PIXELS
            Me.uxsource.Width = _settings.table_width / PTS_TO_PIXELS
            Me.Width = (_settings.table_width / PTS_TO_PIXELS) + Me.uxtitle.Location.X + 50

            Me.uxsource.Properties.Items.Clear()
            If table.has_source = True Then
                Me.uxsource.Properties.Items.Add(_table.source)
            End If


            For i = 0 To _settings.sources.Count - 1
                Me.uxsource.Properties.Items.Add(_settings.sources(i))
            Next

            If _table.has_title = True Then
                Me.chktitle.Checked = True
            End If
            If _table.has_sub_title = True Then
                Me.chksubtitle.Checked = True
            End If
            If _table.has_source = True Then
                Me.chksource.Checked = True
            End If

            load_rows()

            Me.uxheadingrows.Properties.MinValue = 0
            Me.uxlabelcolumns.Properties.MinValue = 0
            If _table.rows.Count > 0 Then
                Me.uxheadingrows.Properties.MaxValue = _table.rows.Count
                Me.uxlabelcolumns.Properties.MaxValue = _table.rows(0).cells.Count
            Else
                Me.uxheadingrows.Properties.MaxValue = 0
                Me.uxlabelcolumns.Properties.MaxValue = 0
            End If

            loading = False

            If never_saved = True Then
                Me.chksave.Checked = False
                Me.chksave.Enabled = False
                lsave.Text = "Workbook has never been saved."
            Else
                If requires_save = True Then
                    Me.chksave.Checked = False
                    Me.chksave.Enabled = True
                    lsave.Text = "Workbook requires a save."
                Else
                    Me.chksave.Checked = True
                    Me.chksave.Enabled = False
                    lsave.Text = "Workbook save is current."
                End If
            End If

        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Table Wizard Failed to Load Excel Preview: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Finally
            Me.panel1.Visible = True
        End Try
    End Function
    Public Function load_view(settings As bc_om_table_wizard, table_type As Cbc_am_table_wizard.TABLE_TYPE, excel_instances As List(Of Cbc_am_table_wizard.excel_instance)) Implements Ibc_am_table_wizard.load_view
        Try

            load_view = False
            loading = True
            _settings = settings
            Me.Cursor = Cursors.WaitCursor


            If table_type = Cbc_am_table_wizard.TABLE_TYPE.STRUCTURE_TABLE Or table_type = Cbc_am_table_wizard.TABLE_TYPE.STRUCTURE_OTHER Then
                Me.uxsize.Text = "Structure Width: " + CStr(settings.table_width / CM_POINTS).Substring(0, 5) + "cm"
            Else
                Me.uxsize.Properties.Items.Add("Autosize: " + CStr(settings.table_width / CM_POINTS).Substring(0, 5) + "cm")
                Me.uxsize.SelectedIndex = 0
                REM load other sizes
                For i = 0 To settings.table_sizes.Count - 1
                    Me.uxsize.Properties.Items.Add(settings.table_sizes(i).name + ":  " + CStr(settings.table_sizes(i).calculated_width / CM_POINTS).Substring(0, 5) + "cm")
                Next
            End If


            If Me.uxsize.Properties.Items.Count < 2 Then
                Me.uxsize.Enabled = False
            End If

            Me.XtraTabControl1.SelectedTabPageIndex = 0
            Me.uxinstances.Visible = False
            Me.uxinstances.Enabled = False
            If excel_instances.Count > 0 Then
                Me.uxinstances.Visible = True
                If excel_instances.Count > 1 Then
                    Me.uxinstances.Enabled = True
                End If

                For i = 0 To excel_instances.Count - 1
                    Me.uxinstances.Properties.Items.Add(excel_instances(i).Name)
                Next
                Me.uxinstances.SelectedIndex = 0
            End If


            For i = 0 To _settings.sources.Count - 1
                Me.uxsource.Properties.Items.Add(_settings.sources(i))
            Next

            For i = 0 To _settings.style_libraries.Count - 1
                Me.uxlibrary.Properties.Items.Add(_settings.style_libraries(i).library_name)
            Next
            Me.uxlibrary.SelectedIndex = 0
            If Me.uxlibrary.Properties.Items.Count < 2 Then
                Me.uxlibrary.Enabled = False
            End If


            Dim heading_rows As Integer = 0
            Dim label_cols As Integer = 0


            _static_controls_count = Me.Panel1.Controls.Count



            loading = False





            Me.uxheadingrows.Properties.MinValue = 0
            Me.uxlabelcolumns.Properties.MinValue = 0

            If excel_instances.Count > 0 Then

                Me.uxtype.SelectedIndex = 1
                Me.uxdistribution.SelectedIndex = 0
                Me.uxdistribution.Properties.Items(0).Enabled = True

            Else

                Me.uxtype.SelectedIndex = 0
                Me.uxdistribution.SelectedIndex = 1
                Me.uxdistribution.Properties.Items(0).Enabled = False
                Me.uxtype.Enabled = False
                _table = New Cbc_am_table_wizard.bc_am_tw_table
                set_up_blank_table()
            End If

            loading = False
            load_view = True
        Catch ex As Exception

            Dim omsg As New bc_cs_message("Blue Curve", "Table Wizard Failed to Load View: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Finally
            Me.Cursor = Cursors.Default

        End Try
    End Function

    Private Function set_font_in_control(control As Control, table_area As bc_om_table_wizard.TABLE_AREA) As Double
        set_font_in_control = 0

        With _settings.style_libraries(uxlibrary.SelectedIndex)

            For i = 0 To .fonts_for_style.Count - 1
                If table_area = .fonts_for_style(i).table_area Then
                    control.Font = .fonts_for_style(i).objFont
                    control.ForeColor = .fonts_for_style(i).colour




                    If .fonts_for_style(i).alignment <> 0 Then
                        control.RightToLeft = Windows.Forms.RightToLeft.Yes
                    Else
                        control.RightToLeft = Windows.Forms.RightToLeft.No
                    End If
                    set_font_in_control = .fonts_for_style(i).font_size
                    Exit Function
                End If


            Next
        End With

    End Function




    Private Sub load_rows()

        Dim i As Integer = 0
        If loading = True Then
            Exit Sub
        End If
        Try
            Me.Cursor = Cursors.WaitCursor
            'Me.panel1.Visible = False
            
            If Me.uxsize.SelectedIndex > 0 Then
                Me.uxtitle.Width = _settings.table_sizes(Me.uxsize.SelectedIndex - 1).calculated_width / PTS_TO_PIXELS
                Me.uxsubtitle.Width = Me.uxtitle.Width
                Me.uxsource.Width = Me.uxtitle.Width
            Else
                Me.uxtitle.Width = _settings.table_width / PTS_TO_PIXELS
                Me.uxsubtitle.Width = Me.uxtitle.Width
                Me.uxsource.Width = Me.uxtitle.Width
            End If
            Dim fwidth As Double
            fwidth = 607
            If Me.uxtitle.Width + Me.uxtitle.Location.X > 607 Then
                Me.Width = Me.uxtitle.Width + Me.uxtitle.Location.X + 50
            Else
                Me.Width = fwidth
            End If

            Me.linetitletop.Visible = False
            Me.linetitlebottom.Visible = False
            Me.linesubtitlebottom.Visible = False
            Me.lineheadingbottom.Visible = False
            Me.linesourcetop.Visible = False
            Me.linesourcebottom.Visible = False
            Me.uxtitle.Visible = False
            Me.uxsubtitle.Visible = False
            Me.uxtitle.BackColor = get_colour_from_rgb("255255255")
            Me.uxsubtitle.BackColor = get_colour_from_rgb("255255255")
            Me.uxsource.BackColor = get_colour_from_rgb("255255255")



            Dim height As Double
            Dim dp As System.Drawing.Point
            If _table.has_title = True Then

                Me.uxtitle.Text = _table.title
                Me.uxtitle.Visible = True
                'Me.chktitle.Checked = True
                height = set_font_in_control(Me.uxtitle, bc_om_table_wizard.TABLE_AREA.TITLE)

                If height <> 0.0 Then
                    Me.uxtitle.Height = height
                End If
                If _settings.style_libraries(Me.uxlibrary.SelectedIndex).title_underline_weight <> 0.0 Then
                    set_line(Me.linetitlebottom, Me.uxtitle, False, _settings.style_libraries(Me.uxlibrary.SelectedIndex).title_underline_weight, _settings.style_libraries(Me.uxlibrary.SelectedIndex).title_underline_colour)
                End If
                If _settings.style_libraries(Me.uxlibrary.SelectedIndex).title_overline_weight <> 0.0 Then
                    set_line(Me.linetitletop, Me.uxtitle, True, _settings.style_libraries(Me.uxlibrary.SelectedIndex).title_overline_weight, _settings.style_libraries(Me.uxlibrary.SelectedIndex).title_overline_colour)
                End If
                If _settings.style_libraries(Me.uxlibrary.SelectedIndex).title_fill <> "" Then
                    set_fill(Me.uxtitle, _settings.style_libraries(Me.uxlibrary.SelectedIndex).title_fill)
                End If

            End If
            If _table.has_sub_title = True Then
                Me.uxsubtitle.Text = _table.sub_title
                Me.uxsubtitle.Visible = True
                height = set_font_in_control(Me.uxsubtitle, bc_om_table_wizard.TABLE_AREA.SUB_TITLE)
                If height <> 0.0 Then
                    Me.uxsubtitle.Height = height
                End If
                dp.X = Me.uxsubtitle.Location.X
                dp.Y = Me.uxtitle.Location.Y + Me.uxtitle.Height
                Me.uxsubtitle.Location = dp
                dp.X = Me.chksubtitle.Location.X
                Me.chksubtitle.Location = dp
                If _settings.style_libraries(Me.uxlibrary.SelectedIndex).sub_title_underline_weight <> 0.0 Then
                    set_line(Me.linesubtitlebottom, Me.uxsubtitle, False, _settings.style_libraries(Me.uxlibrary.SelectedIndex).sub_title_underline_weight, _settings.style_libraries(Me.uxlibrary.SelectedIndex).sub_title_underline_colour)
                End If
                If _settings.style_libraries(Me.uxlibrary.SelectedIndex).sub_title_fill <> "" Then
                    set_fill(Me.uxsubtitle, _settings.style_libraries(Me.uxlibrary.SelectedIndex).sub_title_fill)
                End If
            End If

            If _table.has_source = True Then
                Me.uxsource.Text = _table.source
                Me.uxsource.Visible = True

                height = set_font_in_control(Me.uxsource, bc_om_table_wizard.TABLE_AREA.SOURCE)
                If height <> 0.0 Then
                    Me.uxsource.Height = height
                End If
            End If


            _total_width = 0.0
            If _table.rows.Count > 0 Then
                For i = 0 To _table.rows(0).cells.Count - 1
                    _total_width = _total_width + _table.rows(0).cells(i).width
                Next
            End If

            Dim cell As DevExpress.XtraEditors.TextEdit


            Dim heading_rows As Integer = 0
            Dim label_cols As Integer = 0

            Dim dcontrols As Integer
            dcontrols = Me.Panel1.Controls.Count


            For i = _static_controls_count To dcontrols - 1
                Me.Panel1.Controls.RemoveAt(_static_controls_count)
            Next


            Dim toggle_row As Boolean = False
            Dim rolling_width As Double

            REM starting point 
            If _table.has_sub_title = True Then
                dp.Y = Me.uxsubtitle.Location.Y + Me.uxsubtitle.Height
            Else
                dp.Y = Me.uxtitle.Location.Y + Me.uxtitle.Height
            End If

            Dim lastheadercell = New DevExpress.XtraEditors.TextEdit



            For i = 0 To _table.rows.Count - 1
                rolling_width = Me.uxtitle.Location.X
                If Not IsNothing(cell) Then
                    dp.Y = dp.Y + cell.Height
                End If

                If _table.rows(i).row_type = Cbc_am_table_wizard.bc_am_tw_table.WORD_TABLE_ROW_TYPE.HEADING Then
                    heading_rows = heading_rows + 1
                Else
                    toggle_row = Not toggle_row

                End If
                label_cols = 0

                For j = 0 To _table.rows(i).cells.Count - 1

                    cell = New DevExpress.XtraEditors.TextEdit



                    If heading_rows > 0 And j = 0 And heading_rows = i + 1 Then
                        lastheadercell = cell
                    End If
                    cell.BackColor = Color.White
                    cell.Visible = True
                    cell.Properties.ReadOnly = True

                    If Me.uxtype.SelectedIndex = 1 Then
                        cell.BorderStyle = DevExpress.XtraEditors.Controls.BorderStyles.NoBorder
                        If Me.uxdistribution.SelectedIndex = 0 Then
                            cell.Width = (_table.rows(i).cells(j).width / _total_width) * Me.uxtitle.Width
                        Else
                            cell.Width = Me.uxtitle.Width / _table.rows(0).cells.Count
                        End If
                    Else
                        cell.Width = Me.uxtitle.Width / _table.rows(0).cells.Count
                    End If



                    If Me.uxtype.SelectedIndex = 1 Then
                        cell.Text = _table.rows(i).cells(j).value
                    End If
                    cell.Text = _table.rows(i).cells(j).value

                    dp.X = rolling_width

                    rolling_width = rolling_width + cell.Width


                    If j = 0 Then
                        height = set_font_in_control(cell, bc_om_table_wizard.TABLE_AREA.DATA)
                    Else
                        set_font_in_control(cell, bc_om_table_wizard.TABLE_AREA.DATA)
                    End If



                    If _table.rows(i).row_type = Cbc_am_table_wizard.bc_am_tw_table.WORD_TABLE_ROW_TYPE.HEADING Then
                        If Me.uxtype.SelectedIndex = 0 Then
                            cell.Text = "head"
                        End If
                        set_font_in_control(cell, bc_om_table_wizard.TABLE_AREA.HEADING)
                        If Me.chkshowheading.Checked = True Then
                            cell.BackColor = Color.Green
                        Else
                            If _settings.style_libraries(Me.uxlibrary.SelectedIndex).heading_fill <> "" Then
                                set_fill(cell, _settings.style_libraries(Me.uxlibrary.SelectedIndex).heading_fill)
                            End If
                        End If
                    Else
                        If Me.uxtype.SelectedIndex = 0 And i >= heading_rows Then
                            cell.Text = "data"
                        End If
                        If toggle_row = True Then
                            If _settings.style_libraries(Me.uxlibrary.SelectedIndex).data_row_fill <> "" Then
                                set_fill(cell, _settings.style_libraries(Me.uxlibrary.SelectedIndex).data_row_fill)
                            End If
                        Else
                            set_font_in_control(cell, bc_om_table_wizard.TABLE_AREA.DATA_ALT)
                            If _settings.style_libraries(Me.uxlibrary.SelectedIndex).data_alt_row_fill <> "" Then
                                set_fill(cell, _settings.style_libraries(Me.uxlibrary.SelectedIndex).data_alt_row_fill)
                            End If
                        End If

                    End If


                    If _table.rows(i).cells(j).column_type = Cbc_am_table_wizard.bc_am_tw_table.WORD_TABLE_COLUMN_TYPE.LABEL Then
                        If Me.uxtype.SelectedIndex = 0 And i >= heading_rows Then
                            cell.Text = "label"
                        End If
                        If toggle_row = True Then
                            set_font_in_control(cell, bc_om_table_wizard.TABLE_AREA.LABEL)
                        Else
                            set_font_in_control(cell, bc_om_table_wizard.TABLE_AREA.LABEL_ALT)
                        End If
                        If Me.chkshowlabel.Checked = True And i > Me.uxheadingrows.Value - 1 Then
                            cell.BackColor = Color.Aqua
                        End If

                        label_cols = label_cols + 1
                    End If

                    cell.Location = dp
                    cell.Height = height
                    Me.Panel1.Controls.Add(cell)
                Next
            Next
            loading = True
            Me.uxheadingrows.Value = heading_rows
            Me.uxlabelcolumns.Value = label_cols
            loading = False


            dp.X = Me.uxsource.Location.X
            dp.Y = cell.Location.Y + cell.Height
            Me.uxsource.Location = dp
            dp.X = Me.chksource.Location.X
            Me.chksource.Location = dp

            If heading_rows > 0 And _settings.style_libraries(Me.uxlibrary.SelectedIndex).heading_underline_weight <> 0.0 Then
                set_line(Me.lineheadingbottom, lastheadercell, False, _settings.style_libraries(Me.uxlibrary.SelectedIndex).heading_underline_weight, _settings.style_libraries(Me.uxlibrary.SelectedIndex).heading_underline_colour)
            End If

            If _table.has_source = True Then
                If _settings.style_libraries(Me.uxlibrary.SelectedIndex).source_overline_weight <> 0.0 Then
                    set_line(Me.linesourcetop, Me.uxsource, True, _settings.style_libraries(Me.uxlibrary.SelectedIndex).source_overline_weight, _settings.style_libraries(Me.uxlibrary.SelectedIndex).source_overline_colour)
                End If
                If _settings.style_libraries(Me.uxlibrary.SelectedIndex).source_underline_weight <> 0.0 Then
                    set_line(Me.linesourcebottom, Me.uxsource, False, _settings.style_libraries(Me.uxlibrary.SelectedIndex).source_underline_weight, _settings.style_libraries(Me.uxlibrary.SelectedIndex).source_underline_colour)
                End If
                If _settings.style_libraries(Me.uxlibrary.SelectedIndex).source_fill <> "" Then
                    set_fill(Me.uxsource, _settings.style_libraries(Me.uxlibrary.SelectedIndex).source_fill)
                End If
            End If



            'Me.ppreview.Height = cell.Location.Y + Me.ppreview.Location.Y + 10


            Me.Height = cell.Location.Y + 150 + Me.panel1.Location.Y


            If Me.Height > 800 Then
                Me.Height = 800
            End If
        Catch ex As Exception
            'Dim omsg As New bc_cs_message("Blue Curve", "Table Wizard Failed to Load Rows in View : " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Finally
            Me.Cursor = Cursors.Default
            'Me.panel1.Visible = True
        End Try

    End Sub
    Private Sub set_line(linecontrol As Control, relcontrol As Control, above As Boolean, weight As Double, colour As String)

        linecontrol.Height = (weight / PTS_TO_PIXELS) / 8
        linecontrol.Width = Me.uxtitle.Width
        Dim dp As System.Drawing.Point
        dp.X = relcontrol.Location.X
        If above = True Then
            dp.Y = relcontrol.Location.Y
        Else
            dp.Y = relcontrol.Location.Y + relcontrol.Height
        End If
        linecontrol.Location = dp
        linecontrol.BackColor = get_colour_from_rgb(colour)



        linecontrol.Visible = True
    End Sub
    Private Sub set_fill(fillcontrol As Control, colour As String)
        fillcontrol.BackColor = get_colour_from_rgb(colour)

    End Sub
    Private Function get_colour_from_rgb(color As String) As System.Drawing.Color
        Try
            Dim R As Integer
            Dim G As Integer
            Dim B As Integer
            If Len(color = 9) Then
                R = color.Substring(0, 3)
                G = color.Substring(3, 3)
                B = color.Substring(6, 3)
                get_colour_from_rgb = System.Drawing.ColorTranslator.FromWin32(RGB(R, G, B))
            End If
        Catch
            get_colour_from_rgb = System.Drawing.ColorTranslator.FromWin32(RGB(0, 0, 0))
        End Try
    End Function
    Private Sub uxheadingrows_EditValueChanged(sender As Object, e As EventArgs) Handles uxheadingrows.EditValueChanged
        For i = 0 To _table.rows.Count - 1
            If i < uxheadingrows.Value Then
                _table.rows(i).row_type = Cbc_am_table_wizard.bc_am_tw_table.WORD_TABLE_ROW_TYPE.HEADING
            Else
                _table.rows(i).row_type = Cbc_am_table_wizard.bc_am_tw_table.WORD_TABLE_ROW_TYPE.DATA
            End If
        Next
        Me.head_rows = uxheadingrows.Value

        load_rows()
    End Sub

    Private Sub uxlabelcolumns_EditValueChanged(sender As Object, e As EventArgs) Handles uxlabelcolumns.EditValueChanged
        For i = 0 To _table.rows.Count - 1
            For j = 0 To _table.rows(i).cells.Count - 1
                If j < uxlabelcolumns.Value Then
                    _table.rows(i).cells(j).column_type = Cbc_am_table_wizard.bc_am_tw_table.WORD_TABLE_COLUMN_TYPE.LABEL
                Else
                    _table.rows(i).cells(j).column_type = Cbc_am_table_wizard.bc_am_tw_table.WORD_TABLE_COLUMN_TYPE.DATA
                End If
            Next
        Next
        Me.label_cols = uxlabelcolumns.Value

        load_rows()
    End Sub

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub chksource_CheckedChanged(sender As Object, e As EventArgs) Handles chksource.CheckedChanged
        If Me.chksource.Checked = True Then
            Me.uxsource.Visible = True
            _table.has_source = True

        Else
            Me.uxsource.Visible = False
            _table.has_source = False
        End If

        load_rows()
    End Sub

    Private Sub chktitle_CheckedChanged(sender As Object, e As EventArgs) Handles chktitle.CheckedChanged

        If Me.chktitle.Checked = True Then

            Me.chksubtitle.Visible = True
            _table.has_title = True
        Else

            _table.has_title = False
            _table.has_sub_title = False
            Me.chksubtitle.Visible = False
            Me.chksubtitle.Checked = False
        End If
        load_rows()

    End Sub

    Private Sub chksubtitle_CheckedChanged(sender As Object, e As EventArgs) Handles chksubtitle.CheckedChanged
        If Me.chksubtitle.Checked = True Then
            _table.has_sub_title = True
        Else
            _table.has_sub_title = False
        End If
        load_rows()
    End Sub



    Private Sub uxsource_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxsource.SelectedIndexChanged

    End Sub

    Private Sub uxtitle_EditValueChanged(sender As Object, e As EventArgs) Handles uxtitle.EditValueChanged
        _table.title = Me.uxtitle.Text
    End Sub
    Private Sub uxsubtitle_EditValueChanged(sender As Object, e As EventArgs) Handles uxsubtitle.EditValueChanged
        _table.sub_title = Me.uxsubtitle.Text
    End Sub
    Private Sub uxsource_EditValueChanged(sender As Object, e As EventArgs) Handles uxsource.EditValueChanged
        _table.source = Me.uxsource.Text
    End Sub

    Private Sub binsert_Click(sender As Object, e As EventArgs) Handles binsert.Click
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim omsg As bc_cs_message

            If Me.uxtype.SelectedIndex = 1 Then
                If Me.chksave.Checked = False Then
                    If Me.chksave.Enabled = False Then
                        omsg = New bc_cs_message("Blue Curve", "Source workbook has never been saved. You will not be able to use the update data feature on this table." + vbCrLf + "If you wish to save please select no, save the workbook, refresh the preview and insert again." + vbCrLf + "Proceed without saving?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                        If omsg.cancel_selected = True Then
                            Exit Sub
                        End If
                    Else
                        omsg = New bc_cs_message("Blue Curve", "Source workbook requires a save. You will not be able to use the update data feature on this table." + vbCrLf + "If you wish to save select no, and check save and insert again." + vbCrLf + "Proceed without saving?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                        If omsg.cancel_selected = True Then
                            Exit Sub
                        End If
                    End If
                End If

            End If



            Dim col_Width As Cbc_am_table_wizard.bc_am_tw_table.COLUMN_WIDTH
            If uxdistribution.SelectedIndex = 0 Then
                col_Width = Cbc_am_table_wizard.bc_am_tw_table.COLUMN_WIDTH.FROM_EXCEL
            ElseIf uxdistribution.SelectedIndex = 1 Then
                col_Width = Cbc_am_table_wizard.bc_am_tw_table.COLUMN_WIDTH.EVEN
            Else
                col_Width = Cbc_am_table_wizard.bc_am_tw_table.COLUMN_WIDTH.BEST_FIT
            End If
            If Me.uxsize.SelectedIndex > 0 Then
                RaiseEvent insert_table(_table, Me.uxlibrary.SelectedIndex, _settings.table_sizes(Me.uxsize.SelectedIndex - 1).calculated_width, _settings.table_sizes(Me.uxsize.SelectedIndex - 1).calculated_offset, _settings.table_sizes(Me.uxsize.SelectedIndex - 1).rel_margin, col_Width, Me.chksave.Checked, Me.chksave.Enabled)
            Else
                RaiseEvent insert_table(_table, Me.uxlibrary.SelectedIndex, _settings.table_width, 0.0, True, col_Width, Me.chksave.Checked, Me.chksave.Enabled)
            End If
            Me.Hide()
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub uxlibrary_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxlibrary.SelectedIndexChanged
        If format_only = True Then
            Exit Sub
        End If
        load_rows()
    End Sub

    Private Sub chkshowheading_CheckedChanged(sender As Object, e As EventArgs) Handles chkshowheading.CheckedChanged
        load_rows()
    End Sub

    Private Sub chkshowlabel_CheckedChanged(sender As Object, e As EventArgs) Handles chkshowlabel.CheckedChanged
        load_rows()
    End Sub

    Private Sub RadioGroup1_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxtype.SelectedIndexChanged
        load_excel_selection(False)
    End Sub
    Private Sub load_excel_selection(refresh As Boolean)
        Try

            Me.Cursor = Cursors.WaitCursor
            Me.chksave.Checked = False
            Me.chksave.Enabled = False
            _table.rows.Clear()
            Me.chktitle.Checked = False
            Me.chksource.Checked = False
            Me.uxinstances.Enabled = False
            Me.panel1.Visible = False

            If Me.uxtype.SelectedIndex = 1 Then
                Me.uxinstances.Enabled = True
                Me.chktitle.Enabled = False
                Me.chksubtitle.Enabled = False
                Me.uxtitle.Properties.ReadOnly = True
                Me.uxsubtitle.Properties.ReadOnly = True
                Me.chksource.Enabled = False


                RaiseEvent get_excel_selection(Me.uxinstances.SelectedIndex, False, refresh)

                Me.lrow.Visible = False
                Me.lcol.Visible = False
                Me.uxdlrows.Visible = False
                Me.uxcols.Visible = False
                Me.uxdistribution.Properties.Items(0).Enabled = True
                Me.uxdistribution.SelectedIndex = 0
                Me.uxrefresh.Enabled = True
            Else
                Me.chktitle.Enabled = True
                Me.chksubtitle.Enabled = True
                Me.uxtitle.Properties.ReadOnly = False
                Me.uxsubtitle.Properties.ReadOnly = False
                Me.chksource.Enabled = True
                Me.uxrefresh.Enabled = False
                lsave.Text = ""
                set_up_blank_table()
                Me.panel1.Visible = True
            End If
        Catch
            Me.Cursor = Cursors.Default
        Finally


        End Try
    End Sub
    Private Sub set_up_blank_table()
        _table = New Cbc_am_table_wizard.bc_am_tw_table
        _table.has_title = True
        _table.has_sub_title = False
        _table.has_source = True
        Me.chktitle.Checked = True
        Me.chksource.Checked = True
        Me.lrow.Visible = True
        Me.lcol.Visible = True
        Me.uxdlrows.Visible = True
        Me.uxcols.Visible = True
        Me.uxdistribution.Properties.Items(0).Enabled = False
        Me.uxdistribution.SelectedIndex = 2



        Me.uxdlrows.Value = 4
        Me.uxcols.Value = 4
        num_rows = 4
        num_cols = 4
        label_cols = 1
        head_rows = 1
        load_new_rows_cells()
        load_rows()
    End Sub

    Private Sub uxdlrows_EditValueChanged(sender As Object, e As EventArgs) Handles uxdlrows.EditValueChanged
        num_rows = uxdlrows.Value.ToString

        load_new_rows_cells()
    End Sub
    Private Sub uxcols_EditValueChanged(sender As Object, e As EventArgs) Handles uxcols.EditValueChanged
        num_cols = uxcols.Value.ToString
        load_new_rows_cells()
    End Sub
    Private Sub load_new_rows_cells()
        Dim row As New Cbc_am_table_wizard.bc_am_tw_table.bc_am_tw_table_row
        Dim cell As New Cbc_am_table_wizard.bc_am_tw_table.bc_am_tw_table_cell



        _table.rows.Clear()
        For i = 0 To num_rows - 1

            row = New Cbc_am_table_wizard.bc_am_tw_table.bc_am_tw_table_row
            If i < head_rows Then
                row.row_type = Cbc_am_table_wizard.bc_am_tw_table.WORD_TABLE_ROW_TYPE.HEADING
            Else
                row.row_type = Cbc_am_table_wizard.bc_am_tw_table.WORD_TABLE_ROW_TYPE.DATA
            End If


            For j = 0 To num_cols - 1
                cell = New Cbc_am_table_wizard.bc_am_tw_table.bc_am_tw_table_cell
                If j < label_cols Then
                    cell.column_type = Cbc_am_table_wizard.bc_am_tw_table.WORD_TABLE_COLUMN_TYPE.LABEL
                Else
                    cell.column_type = Cbc_am_table_wizard.bc_am_tw_table.WORD_TABLE_COLUMN_TYPE.DATA
                End If

                row.cells.Add(cell)
            Next
            _table.rows.Add(row)
        Next
        load_rows()


    End Sub

    Private Sub uxdistribution_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxdistribution.SelectedIndexChanged
        load_rows()
    End Sub

    Private Sub uxsize_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxsize.SelectedIndexChanged
        If Me.uxsize.SelectedIndex > -1 Then
            load_rows()
        End If
    End Sub


    Private Sub uxinstances_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxinstances.SelectedIndexChanged
        If loading = True Then
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor


        If Me.uxinstances.SelectedIndex > -1 Then

            RaiseEvent get_excel_selection(Me.uxinstances.SelectedIndex, False, False)
        End If
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub uxrefresh_Click(sender As Object, e As EventArgs) Handles uxrefresh.Click
        load_excel_selection(True)
    End Sub

    Private Sub bcancel2_Click(sender As Object, e As EventArgs) Handles bcancel2.Click
        Me.Hide()
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Me.Cursor = Cursors.WaitCursor
        RaiseEvent format_table(Me.uxlibrary.SelectedIndex)
        Me.Cursor = Cursors.Default
        Me.Hide()
    End Sub

    Private Sub bdelete_Click(sender As Object, e As EventArgs) Handles bdelete.Click

        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you want to delete  this table?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = True Then
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor
        RaiseEvent delete_table()
        Me.Cursor = Cursors.Default
        Me.Hide()
    End Sub

    Private Sub Button2_Click(sender As Object, e As EventArgs) Handles uxdata.Click
        Dim omsg As New bc_cs_message("Blue Curve", "Are you sure you want to update the data in  this table?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
        If omsg.cancel_selected = True Then
            Exit Sub
        End If
        Me.Cursor = Cursors.WaitCursor
        RaiseEvent update_data()
        Me.Cursor = Cursors.Default
        Me.Hide()
    End Sub

    
   
End Class
Public Class Cbc_am_table_wizard
    WithEvents _view As Ibc_am_table_wizard
    Private Declare Function GetDesktopWindow Lib "user32" () As Integer
    Private Declare Function EnumChildWindows Lib "user32.dll" (ByVal WindowHandle As IntPtr, ByVal Callback As EnumWindowsProc, ByVal lParam As IntPtr) As Boolean
    Private Declare Function GetClassName Lib "user32.dll" Alias "GetClassNameA" (ByVal hWnd As IntPtr, ByVal lpClassName As String, ByVal nMaxCount As Integer) As Integer
    Private Delegate Function EnumWindowsProc(ByVal hwnd As IntPtr, ByVal lParam As Int32) As Boolean
    Private Declare Function AccessibleObjectFromWindow Lib "oleacc" (ByVal Hwnd As Int32, ByVal dwId As Int32, ByRef riid As Guid, <MarshalAs(UnmanagedType.IUnknown)> ByRef ppvObject As Object) As Int32

    Private Const OBJID_NATIVE = &HFFFFFFF0



    Dim _excel As Object
    Dim _table_data(0, 0) As String
    Dim _table_cell_widths(0, 0) As Double
    Dim _table As bc_am_tw_table

    Dim settings As New bc_om_table_wizard
    Dim table_width As Double = 0.0
    Dim _excel_instances As New List(Of excel_instance)
    Dim _excel_selection_available As Boolean = False
    Dim _openworkbooks As New List(Of String)
    Dim _ao_object As bc_ao_prod_tools
    Dim _pub_type_id As Long

    Public Class excel_instance
        Public Name As String
        Public FullName As Object
        Public def As Boolean
        Public instance As Object
    End Class

    Public Function load_data(view As Ibc_am_table_wizard, obj As Object) As Boolean
        Try



            load_data = False
            Dim sobj As String
            sobj = CStr(obj.GetType.ToString)
            If InStr(LCase(sobj), "word") > 0 Then
                _ao_object = New bc_ao_word_prod_tools(obj)
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Application object: " + sobj + " not supported", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If


            REM check bc document
            _pub_type_id = _ao_object.get_pub_type_id
            If _pub_type_id = 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "Document was not created with Blue Curve", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If

            'REM load form dat files in final
            If get_config_settings() = False Then
                Exit Function
            End If

            load_data = False
            _view = view

            Dim table_type As TABLE_TYPE
            table_type = _ao_object.get_table_type()
            If table_type = Cbc_am_table_wizard.TABLE_TYPE.INVALID_INSERTION_POINT Then
                Dim omsg As New bc_cs_message("Blue Curve", "Insertion Point not valid for Table Wizard. Please change your cursor poistion.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If
            REM check insertion point valid
            If table_type = Cbc_am_table_wizard.TABLE_TYPE.NO_TABLE Or table_type = Cbc_am_table_wizard.TABLE_TYPE.STRUCTURE_TABLE Or table_type = Cbc_am_table_wizard.TABLE_TYPE.STRUCTURE_OTHER Then
                settings.table_width = _ao_object.structure_width

                If settings.table_width = 0.0 Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Insertion Point not valid for Table Wizard. Please change your cursor poistion.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                End If
                REM get page settings
                If table_type <> Cbc_am_table_wizard.TABLE_TYPE.STRUCTURE_TABLE And table_type <> Cbc_am_table_wizard.TABLE_TYPE.STRUCTURE_OTHER Then
                    If _ao_object.load_page_settings() = False Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Insertion Point not valid for Table Wizard. Please change your cursor poistion.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Function
                    End If

                End If
                REM load table wizard settings
                If load_settings(settings, table_type) = False Then
                    Exit Function
                End If
                If get_open_excel_instances() = False Then
                    Exit Function
                End If

                GetExcelOpenWorkBooks()
                REM only allow one workbook per instance
                Dim found As Boolean = False
                Dim i As Integer = 0
                While i < _excel_instances.Count
                    found = False
                    For j = 0 To _openworkbooks.Count - 1
                        If _openworkbooks(j) = _excel_instances(i).Name Then
                            found = True
                            Exit For
                        End If
                    Next
                    If found = False Then
                        _excel_instances.RemoveAt(i)
                        i = i - 1
                    End If
                    i = i + 1
                End While


                If _view.load_view(settings, table_type, _excel_instances) = True Then
                    load_data = True
                End If
            Else

                Dim sel_table As New bc_am_tw_table
                If _ao_object.get_selected_table_structure(sel_table) = False Then
                    Exit Function
                End If
                Dim tx As String
                If table_type = Cbc_am_table_wizard.TABLE_TYPE.OTHER Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Table was not created with Table  Wizard.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Function
                    'sel_table.library = 0
                    'tx = "Table wasnt created with table wizard. Table Wizard will attempt a best fit format"
                    '_ao_object.table = sel_table
                Else
                    If set_up_styles() = False Then
                        Exit Function
                    End If
                    sel_table.library = _ao_object.table.library
                    If sel_table <> _ao_object.table Then
                        tx = "Table has changed structure since it was originally inserted. Table Wizard will attempt a best fit format."
                        sel_table.library = _ao_object.table.library
                        _ao_object.table = sel_table
                    Else
                        REM if if workbook range is saved


                    End If
                End If
                load_data = _view.load_view_update(settings, _ao_object.table, tx)
            End If
        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Table Wizard Failed to Load: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Finally

        End Try
    End Function
    Public Sub delete_table() Handles _view.delete_table

        _ao_object.delete_table()

    End Sub
    Public Sub update_data() Handles _view.update_data
        Try
            Dim omsg As bc_cs_message

            REM open up workbook in new instance of exce;
            Dim fs As New bc_cs_file_transfer_services
            If fs.check_document_exists(_ao_object.table.pathname) = False Then
                omsg = New bc_cs_message("Blue Curve", "Cannot find workbook: " + _ao_object.table.pathname, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                Exit Sub
            End If
            Dim new_excel As Object
            new_excel = CreateObject("excel.application")
            new_excel.workbooks.add(_ao_object.table.pathname)

            Try
                new_excel.activeworkbook.worksheets(_ao_object.table.sheetname).select()
            Catch
                omsg = New bc_cs_message("Blue Curve", "Cannot find worksheet: " + _ao_object.table.sheetname + " in workbook: " + _ao_object.table.pathname, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                Exit Sub
            End Try
            Try
                new_excel.activeworkbook.Worksheets(_ao_object.table.sheetname).Range(_ao_object.table.range).Select()
            Catch
                omsg = New bc_cs_message("Blue Curve", "cannot select range: " + _ao_object.table.range + "f or worksheet: " + _ao_object.table.sheetname + " in workbook: " + _ao_object.table.pathname, bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                Exit Sub
            End Try

            _excel = new_excel
            get_excel_selection(-1, True, False)



            _ao_object.table = _table
            _ao_object.screen_updating(False)
            If _ao_object.insert_data_into_table() = False Then
                Exit Sub
            End If


        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Table Wizard Failed to Update Data: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Finally
            If Not IsNothing(_excel) Then
                _excel.quit()
                Marshal.ReleaseComObject(_excel.application)
            End If
            _ao_object.screen_updating(True)
        End Try
    End Sub
    Public Sub get_excel_selection(instance As Integer, update_data As Boolean, refresh As Boolean) Handles _view.get_excel_selection
        Try

            If update_data = False And refresh = False Then
                If Not IsNothing(_excel) Then
                    Marshal.ReleaseComObject(_excel.application)
                End If
                _excel = _excel_instances(instance).instance
            End If

            REM if not blank workbook
            _table = New Cbc_am_table_wizard.bc_am_tw_table
            Try
                ReDim _table_data(_excel.selection.rows.count - 1, _excel.selection.columns.count - 1)
                ReDim _table_cell_widths(_excel.selection.rows.count - 1, _excel.selection.columns.count - 1)
            Catch
                Dim omsg As New bc_cs_message("Blue Curve", "No Excel Selection in workbook.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

                Exit Sub
            End Try



            For i = 0 To _excel.selection.rows.count - 1
                For j = 0 To _excel.selection.columns.count - 1
                    _table_data(i, j) = _excel.selection.cells(i + 1, j + 1).text
                    _table_cell_widths(i, j) = _excel.selection.cells(i + 1, j + 1).width
                Next
            Next

            REM see if header

            _table.title = check_non_tabular_row(0)
            If UBound(_table_data, 1) > 0 Then
                _table.sub_title = check_non_tabular_row(1)
            End If
            If UBound(_table_data, 1) > 1 Then
                _table.source = check_non_tabular_row(UBound(_table_data, 1))
                If _table.source <> "" Then
                    _table.has_source = True
                End If
            End If
            Dim row As Integer = 0
            If _table.title <> "" Then
                row = row + 1
                _table.has_title = True
            End If
            If _table.sub_title <> "" Then
                row = row + 1
                _table.has_sub_title = True
            End If
            Dim end_row As Integer
            If _table.source <> "" Then
                end_row = UBound(_table_data, 1)
            Else
                end_row = UBound(_table_data, 1) + 1
            End If
            Dim trow As bc_am_tw_table.bc_am_tw_table_row
            Dim rcell As bc_am_tw_table.bc_am_tw_table_cell
            Dim start_row As Integer
            start_row = row
            Dim total_width As Double = 0.0
            For i = 0 To UBound(_table_data, 2)
                total_width = total_width + _table_cell_widths(0, i)

            Next



            While (row < end_row)
                trow = New bc_am_tw_table.bc_am_tw_table_row
                If check_heading_row(row) = True Then
                    trow.row_type = bc_am_tw_table.WORD_TABLE_ROW_TYPE.HEADING
                    rcell = New bc_am_tw_table.bc_am_tw_table_cell
                    rcell.column_type = bc_am_tw_table.WORD_TABLE_COLUMN_TYPE.DATA
                    rcell.value = ""
                    rcell.width = _table_cell_widths(row, 0)
                    rcell.percent_width = rcell.width / total_width

                    trow.cells.Add(rcell)
                    For i = 1 To UBound(_table_data, 2)
                        rcell = New bc_am_tw_table.bc_am_tw_table_cell
                        rcell.column_type = bc_am_tw_table.WORD_TABLE_COLUMN_TYPE.DATA
                        rcell.value = _table_data(row, i)
                        rcell.width = _table_cell_widths(row, i)
                        rcell.percent_width = rcell.width / total_width
                        trow.cells.Add(rcell)
                    Next
                Else
                    For i = 0 To UBound(_table_data, 2)
                        rcell = New bc_am_tw_table.bc_am_tw_table_cell
                        If check_label_col(start_row, i) = True Then
                            rcell.column_type = bc_am_tw_table.WORD_TABLE_COLUMN_TYPE.LABEL
                        Else
                            rcell.column_type = bc_am_tw_table.WORD_TABLE_COLUMN_TYPE.DATA
                        End If
                        rcell.value = _table_data(row, i)
                        rcell.width = _table_cell_widths(row, i)
                        rcell.percent_width = rcell.width / total_width
                        trow.cells.Add(rcell)
                    Next
                    trow.row_type = bc_am_tw_table.WORD_TABLE_ROW_TYPE.DATA
                End If
                _table.rows.Add(trow)
                row = row + 1
            End While

            If update_data = False Then
                Dim never_saved As Boolean = True
                Dim fn As String
                fn = _excel.ActiveWorkbook.FullName
                If Len(fn) > 5 Then
                    fn = Right(fn, 5)
                    If Left(fn, 1) = "." Then
                        never_saved = False
                    End If
                End If
                _view.load_excel_preview(_table, never_saved, Not (_excel.ActiveWorkbook.saved))
            End If
        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Table Wizard Failed to Load Excel Selection: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        End Try
    End Sub
    Public Function get_config_settings() As Boolean
        Try


            Dim bcs As New bc_cs_central_settings(True)
            bc_am_load_objects.obc_pub_types = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + "bc_pub_types.dat")

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = _pub_type_id Then
                    settings = bc_am_load_objects.obc_pub_types.pubtype(i).table_wizard
                    get_config_settings = True
                    Exit Function
                End If
            Next

        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Table Wizard Failed to Load Config Settings: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

        End Try
    End Function



    Public Function load_settings(ByRef settings As bc_om_table_wizard, table_type As TABLE_TYPE) As Boolean
        Try


            load_settings = False

            REM calculate dyncic width and left offsets if table width options
            If table_type <> Cbc_am_table_wizard.TABLE_TYPE.STRUCTURE_TABLE And table_type <> Cbc_am_table_wizard.TABLE_TYPE.STRUCTURE_OTHER Then
                For i = 0 To settings.table_sizes.Count - 1
                    If settings.table_sizes(i).rel_margin = True Then
                        settings.table_sizes(i).calculated_width = _ao_object.page_settings.awidth * (settings.table_sizes(i).perc_width / 100.0)
                        settings.table_sizes(i).calculated_offset = _ao_object.page_settings.awidth * (settings.table_sizes(i).perc_left_offset / 100.0)
                    Else
                        If settings.table_sizes(i).in_left_margin Then
                            settings.table_sizes(i).calculated_width = _ao_object.page_settings.lmargin * (settings.table_sizes(i).perc_width / 100.0)
                            settings.table_sizes(i).calculated_offset = _ao_object.page_settings.lmargin * (settings.table_sizes(i).perc_left_offset / 100.0)
                        ElseIf settings.table_sizes(i).in_right_margin Then
                            settings.table_sizes(i).calculated_width = _ao_object.page_settings.rmargin * (settings.table_sizes(i).perc_width / 100.0)
                            settings.table_sizes(i).calculated_offset = _ao_object.page_settings.lmargin + _ao_object.page_settings.awidth + (_ao_object.page_settings.rmargin * (settings.table_sizes(i).perc_left_offset / 100.0))
                        Else
                            settings.table_sizes(i).calculated_width = _ao_object.page_settings.pwidth * (settings.table_sizes(i).perc_width / 100.0)
                            settings.table_sizes(i).calculated_offset = _ao_object.page_settings.pwidth * (settings.table_sizes(i).perc_left_offset / 100.0)
                        End If
                    End If
                Next
            End If
            load_settings = set_up_styles()

        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to Load Settings: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        End Try
    End Function

    Public Function set_up_styles()
        Try
            REM now check styles all exits if a style is missing eliminate libaray
            For i = 0 To settings.style_libraries.Count - 1
                With settings.style_libraries(i)
                    settings.style_libraries(i).fonts_for_style = New List(Of bc_om_table_wizard.bc_om_fonts_for_style)

                    Dim fs As bc_om_table_wizard.bc_om_fonts_for_style
                    REM check all styles exist in document for library
                    fs = New bc_om_table_wizard.bc_om_fonts_for_style
                    fs.table_area = bc_om_table_wizard.TABLE_AREA.TITLE
                    fs.style_name = settings.style_libraries(i).title_style
                    If set_up_font(fs) = False Then
                        .exclude = True
                        Continue For
                    End If
                    settings.style_libraries(i).fonts_for_style.Add(fs)
                    fs = New bc_om_table_wizard.bc_om_fonts_for_style
                    fs.table_area = bc_om_table_wizard.TABLE_AREA.SUB_TITLE
                    fs.style_name = settings.style_libraries(i).sub_title_style
                    If set_up_font(fs) = False Then
                        .exclude = True
                        Continue For
                    End If
                    settings.style_libraries(i).fonts_for_style.Add(fs)

                    fs = New bc_om_table_wizard.bc_om_fonts_for_style
                    fs.table_area = bc_om_table_wizard.TABLE_AREA.HEADING
                    fs.style_name = settings.style_libraries(i).heading_style
                    If set_up_font(fs) = False Then
                        .exclude = True
                        Continue For
                    End If
                    settings.style_libraries(i).fonts_for_style.Add(fs)

                    fs = New bc_om_table_wizard.bc_om_fonts_for_style
                    fs.table_area = bc_om_table_wizard.TABLE_AREA.LABEL
                    fs.style_name = settings.style_libraries(i).label_style
                    If set_up_font(fs) = False Then
                        .exclude = True
                        Continue For
                    End If

                    settings.style_libraries(i).fonts_for_style.Add(fs)
                    fs = New bc_om_table_wizard.bc_om_fonts_for_style
                    fs.table_area = bc_om_table_wizard.TABLE_AREA.LABEL_ALT
                    fs.style_name = settings.style_libraries(i).label_alt_style

                    If set_up_font(fs) = False Then
                        .exclude = True
                        Continue For
                    End If
                    settings.style_libraries(i).fonts_for_style.Add(fs)


                    fs = New bc_om_table_wizard.bc_om_fonts_for_style
                    fs.table_area = bc_om_table_wizard.TABLE_AREA.DATA
                    fs.style_name = settings.style_libraries(i).data_style
                    If set_up_font(fs) = False Then
                        .exclude = True
                        Continue For
                    End If
                    settings.style_libraries(i).fonts_for_style.Add(fs)


                    fs = New bc_om_table_wizard.bc_om_fonts_for_style
                    fs.table_area = bc_om_table_wizard.TABLE_AREA.DATA_ALT
                    fs.style_name = settings.style_libraries(i).data_alt_row_style
                    If set_up_font(fs) = False Then
                        .exclude = True
                        Continue For
                    End If
                    settings.style_libraries(i).fonts_for_style.Add(fs)


                    fs = New bc_om_table_wizard.bc_om_fonts_for_style
                    fs.table_area = bc_om_table_wizard.TABLE_AREA.SOURCE
                    fs.style_name = settings.style_libraries(i).source_style
                    If set_up_font(fs) = False Then
                        .exclude = True
                        Continue For
                    End If
                    settings.style_libraries(i).fonts_for_style.Add(fs)

                End With
            Next
            Dim j As Integer
            While j < settings.style_libraries.Count
                If settings.style_libraries(j).exclude = True Then
                    settings.style_libraries.RemoveAt(j)
                    j = j - 1
                End If
                j = j + 1
            End While
            If settings.style_libraries.Count = 0 Then
                Dim omg As New bc_cs_message("Blue Curve", "No Valid Complete Style Set found in Template for Table Wizard. Please check configured template styles.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                set_up_styles = False
                Exit Function
            End If


            set_up_styles = True
        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to Set Up Styles: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        End Try

    End Function

    Private Function set_up_font(ByRef fs As bc_om_table_wizard.bc_om_fonts_for_style) As Boolean
        set_up_font = False

        Try

            Dim objFont As System.Drawing.Font = Nothing
            fs.font_name = _ao_object.get_font_name(fs.style_name)

            fs.font_size = _ao_object.get_font_size(fs.style_name)
            fs.bold = _ao_object.get_font_bold(fs.style_name)
            fs.italic = _ao_object.get_font_italic(fs.style_name)
            fs.alignment = _ao_object.get_font_alignment(fs.style_name)
            Dim R, G, B As Integer
            Dim colour As Integer

            colour = _ao_object.get_font_colour(fs.style_name)


            R = colour Mod 256
            G = (colour \ 256) Mod 256
            B = ((colour \ 256) \ 256) Mod 256
            Try
                fs.colour = Color.FromArgb(R, G, B)
            Catch

            End Try


            If fs.bold = True And fs.italic = True Then
                fs.objFont = New System.Drawing.Font(fs.font_name, fs.font_size, FontStyle.Bold Or FontStyle.Italic)
            ElseIf fs.bold = True Then
                fs.objFont = New System.Drawing.Font(fs.font_name, fs.font_size, FontStyle.Bold)
            ElseIf fs.italic = True Then
                fs.objFont = New System.Drawing.Font(fs.font_name, fs.font_size, FontStyle.Italic)
            Else
                fs.objFont = New System.Drawing.Font(fs.font_name, fs.font_size)
            End If

            set_up_font = True

        Catch ex As Exception

            Dim ocomm As New bc_cs_activity_log("Cbc_am_table_wizard", "set_up_font", bc_cs_activity_codes.COMMENTARY, "Style: " + fs.style_name + " does not exist in template")

        End Try

    End Function

    Public Sub insert_table(table As Cbc_am_table_wizard.bc_am_tw_table, style_library As Integer, width As Double, left_offset As Double, rel_to_margin As Boolean, col_width As Cbc_am_table_wizard.bc_am_tw_table.COLUMN_WIDTH, save_table As Boolean, do_save As Boolean) Handles _view.insert_table
        Try
            _ao_object.table = table
            _ao_object.screen_updating(False)


            If _ao_object.insert_table(style_library, width, left_offset, rel_to_margin, col_width) = False Then
                Exit Sub
            End If

            If _ao_object.insert_data_into_table() = False Then
                Exit Sub
            End If



            If _ao_object.format_table(settings) = False Then
                Exit Sub
            End If


            If save_table = True Then
                If do_save = True Then
                    Try
                        _excel.activeworkbook.save()
                    Catch ex As Exception
                        Dim omsg As New bc_cs_message("Blue Curve", "Failed to Save Workbook " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End Try
                End If

                If _ao_object.set_excel_reference(_excel.activeworkbook.fullname, _excel.activesheet.name, _excel.Selection.Address) = False Then
                    Exit Sub
                End If
            End If

        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to Insert Table: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Finally
            _ao_object.screen_updating(True)
        End Try
    End Sub

    Public Enum TABLE_TYPE
        NO_TABLE = 0
        STRUCTURE_TABLE = 1
        TABLE_WIZARD = 2
        OTHER = 3
        STRUCTURE_OTHER = 4
        INVALID_INSERTION_POINT = 5
    End Enum


    Public Function update_styles(library As Integer) Handles _view.format_table
        Try
            _ao_object.screen_updating(False)

            _ao_object.table.library = library

            _ao_object.format_table(settings)
        Catch

        Finally
            _ao_object.screen_updating(True)
        End Try


    End Function

    Class bc_am_tw_table
        Public title As String
        Public sub_title As String
        Public source As String
        Public has_title As Boolean = False
        Public has_sub_title As Boolean = False
        Public has_source As Boolean = False
        Public rows As New List(Of bc_am_tw_table_row)
        Public column_width_method As COLUMN_WIDTH
        Public heading_rows As Integer
        Public data_rows As Integer
        Public label_cols As Integer
        Public total_cols As Integer
        Public library As Integer
        Public table_id As Integer
        Public pathname As String
        Public sheetname As String
        Public range As String
        Public has_data_source As Boolean = False
        Public Shared Operator =(ByVal t1 As bc_am_tw_table,
                            ByVal t2 As bc_am_tw_table) As Boolean
            Dim orc, nrc As Integer
            orc = 0
            nrc = 0
            orc = t1.heading_rows + t1.data_rows
            nrc = t2.heading_rows + t2.data_rows



            Return t1.has_title = t2.has_title And t1.has_sub_title = t2.has_sub_title And t1.has_source = t2.has_source And t1.total_cols = t2.total_cols And orc = nrc

        End Operator
        Public Shared Operator <>(ByVal t1 As bc_am_tw_table,
                           ByVal t2 As bc_am_tw_table) As Boolean
            Dim orc, nrc As Integer
            orc = 0
            nrc = 0
            orc = t1.heading_rows + t1.data_rows
            nrc = t2.heading_rows + t2.data_rows


            Return t1.has_title <> t2.has_title Or t1.has_sub_title <> t2.has_sub_title Or t1.has_source <> t2.has_source Or t1.total_cols <> t2.total_cols Or orc <> nrc

        End Operator


        Class bc_am_tw_table_row
            Public row_type As WORD_TABLE_ROW_TYPE
            Public cells As New List(Of bc_am_tw_table_cell)
        End Class
        Class bc_am_tw_table_cell
            Public column_type As WORD_TABLE_COLUMN_TYPE
            Public value As String
            Public width As Double
            Public percent_width As Double
        End Class
        Public Enum WORD_TABLE_ROW_TYPE
            HEADING = 1
            DATA = 2
        End Enum
        Public Enum WORD_TABLE_COLUMN_TYPE
            LABEL = 1
            DATA = 2
        End Enum
        Public Enum COLUMN_WIDTH
            FROM_EXCEL = 1
            EVEN = 2
            BEST_FIT = 3
        End Enum

    End Class
    Function check_non_tabular_row(row_id) As String
        Dim value As String = ""
        Dim tx As String = ""
        For i = 0 To UBound(_table_data, 2)
            If i = 0 Then
                value = _table_data(row_id, 0)
            Else
                tx = tx + _table_data(row_id, i)
            End If
        Next
        If tx <> "" Then
            value = ""
        End If
        Return value
    End Function
    Function check_heading_row(row_id) As String
        check_heading_row = False
        If _table_data(row_id, 0) = "" Then
            check_heading_row = True
        End If

    End Function
    Function check_label_col(row_id, col_id) As String
        check_label_col = False
        If _table_data(row_id, col_id) = "" Then
            check_label_col = True
        End If

    End Function

    Private Sub GetExcelOpenWorkBooks()
        Try
            'Get handle to desktop
            Dim WindowHandle As IntPtr = GetDesktopWindow()

            'Enumerate through the windows (objects) that are open
            EnumChildWindows(WindowHandle, AddressOf GetExcelWindows, 0)

            'List the workbooks out if we have something
            'If lstWorkBooks.Count > 0 Then MsgBox(String.Join(Environment.NewLine, lstWorkBooks))

        Catch ex As Exception
        End Try

    End Sub

    Private Function get_open_excel_instances() As Boolean
        get_open_excel_instances = False
        Dim strFileName As String
        Dim inti As Integer
        Try
            Dim localByName As System.Diagnostics.Process() = System.Diagnostics.Process.GetProcessesByName("Excel")
            For inti = 0 To localByName.Length - 1
                strFileName = Replace(localByName(inti).MainWindowTitle(), "Microsoft Excel -", "")
                If strFileName <> "" Then
                    _openworkbooks.Add(Trim(strFileName))
                End If
            Next
            get_open_excel_instances = True
        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to get Excel instances: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

        End Try
    End Function
    Public Function GetExcelWindows(ByVal hwnd As IntPtr, ByVal lParam As Int32) As Boolean

        Dim Ret As Integer = 0
        Dim className As String = Space(255) 'Return the string with some padding...

        Ret = GetClassName(hwnd, className, 255)
        className = className.Substring(0, Ret)

        If className = "EXCEL7" Then
            Dim ExcelApplication As Object
            Dim ExcelObject As Object = Nothing
            Dim IDispatch As Guid
            Dim ei As excel_instance
            Dim found As Boolean
            AccessibleObjectFromWindow(hwnd, OBJID_NATIVE, IDispatch, ExcelObject)

            'Did we get anything?
            If ExcelObject IsNot Nothing Then
                ExcelApplication = ExcelObject.Application
                'Make sure we have the instance...
                If ExcelApplication IsNot Nothing Then
                    'Go through the workbooks...
                    For Each wrk As Object In ExcelApplication.Workbooks
                        found = False
                        'If workbook ins't in the list then add it...
                        'If Not lstWorkBooks.Contains(wrk.Name) Then
                        For j = 0 To _excel_instances.Count - 1
                            If wrk.Name = _excel_instances(j).Name Then
                                found = True
                                Exit For
                            End If
                        Next
                        If found = False Then
                            ei = New excel_instance
                            ei.Name = wrk.Name
                            ei.FullName = wrk.fullname
                            ei.instance = ExcelApplication

                            _excel_instances.Add(ei)
                        End If

                        'End If
                    Next
                End If

            End If
        End If

        Return True

    End Function
End Class
Public Interface Ibc_am_table_wizard
    Function load_view(settings As bc_om_table_wizard, table_type As Cbc_am_table_wizard.TABLE_TYPE, excel_instances As List(Of Cbc_am_table_wizard.excel_instance))
    Function load_view_update(settings As bc_om_table_wizard, table As Cbc_am_table_wizard.bc_am_tw_table, tx As String)

    Function load_excel_preview(table As Cbc_am_table_wizard.bc_am_tw_table, never_saved As Boolean, requires_save As Boolean)
    Event insert_table(table As Cbc_am_table_wizard.bc_am_tw_table, style_library As Integer, table_Width As Double, left_Offset As Double, rel_to_margin As Boolean, col_dist As Cbc_am_table_wizard.bc_am_tw_table.COLUMN_WIDTH, save_excel As Boolean, do_save As Boolean)
    Event get_excel_selection(instance As Integer, update_data As Boolean, refresh As Boolean)
    Event format_table(style_library As Integer)
    Event update_data()
    Event delete_table()
End Interface
REM this should go in AS layer
Public MustInherit Class bc_ao_prod_tools
    Protected _ao_object As Object
    Public table As Cbc_am_table_wizard.bc_am_tw_table
    Public structuretable As Object
    Public structure_width As Double = 0.0
    Public page_settings = New Cbc_am_page_structure.page_settings
    Public table_id As Integer = 0
    Public Overridable Function delete_table() As Boolean
        Return False
    End Function
    Public Sub New(ao_object)
        _ao_object = ao_object
    End Sub
    Public Overridable Sub screen_updating(turn_on As Boolean)

    End Sub
    Public Overridable Function get_pub_type_id() As Long
        Return 0
    End Function
    Public Overridable Function set_excel_reference(fullname As String, sheetname As String, range As String)
        Return False
    End Function
    Public Overridable Function get_table_type() As Cbc_am_table_wizard.TABLE_TYPE
        Return False
    End Function
    Public Overridable Function get_selected_table_structure(ByRef table As Cbc_am_table_wizard.bc_am_tw_table) As Boolean
        Return False
    End Function
    Public Overridable Function load_page_settings() As Boolean
        Return False
    End Function
    Public Overridable Function insert_data_into_table() As Boolean
        Return False
    End Function
    Public Overridable Function get_font_name(style_name As String)
        Return ""
    End Function
    Public Overridable Function get_font_size(style_name As String)
        Return ""
    End Function
    Public Overridable Function get_font_bold(style_name As String)
        Return ""
    End Function
    Public Overridable Function get_font_italic(style_name As String)
        Return ""
    End Function
    Public Overridable Function get_font_alignment(style_name As String)
        Return ""
    End Function
    Public Overridable Function get_font_colour(style_name As String)
        Return ""
    End Function
    Public Overridable Function insert_table(style_library As Integer, width As Double, left_offset As Double, rel_to_margin As Boolean, col_width As Cbc_am_table_wizard.bc_am_tw_table.COLUMN_WIDTH) As Boolean
        Return False
    End Function
    Public Overridable Function format_table(settings As bc_om_table_wizard) As Boolean
        Return False
    End Function

    Public Overridable Function get_sheetname_as_property() As String
        get_sheetname_as_property = ""
    End Function
    Public Overridable Function get_ref_as_property() As String
        get_ref_as_property = ""
    End Function

    Public Overridable Function get_filename_as_property() As String
        get_filename_as_property = ""
    End Function
    Public Overridable Function check_cursor_insetion_point()
        check_cursor_insetion_point = False
    End Function

    Public Function get_rgb(color As String) As Integer
        Try
            Dim R As Integer
            Dim B As Integer
            Dim G As Integer
            R = color.Substring(0, 3)
            B = color.Substring(3, 3)
            G = color.Substring(6, 3)
            Dim c As New System.Drawing.Color()
            get_rgb = RGB(R, B, G)
        Catch
            get_rgb = RGB(255, 255, 255)
        End Try
    End Function
    REM structure wizard
    Public Overridable Function delete_structure_on_page() As Boolean
        Return False
    End Function
    Public Overridable Function InsertStructureAsTables(oStructure As bc_om_structure, vrel As Integer) As Boolean
        Return False
    End Function
    REM end structure wizard
End Class
Public Class bc_ao_word_prod_tools
    Inherits bc_ao_prod_tools


    Public Sub New(ao_object)
        MyBase.New(ao_object)
    End Sub
    REM stucture wizard
    Public Overrides Function delete_structure_on_page() As Boolean
        Try
            Dim i As Integer = 1
            While i <= _ao_object.Tables.Count
                If _ao_object.application.selection.information(3) = _ao_object.Tables(i).range.information(3) Then
                    _ao_object.Tables(i).delete()
                    i = i - 1
                End If
                i = i + 1
            End While
            delete_structure_on_page = True
        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to delete structures on page. " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

        End Try
    End Function

    Public Overrides Function InsertStructureAsTables(oStructure As bc_om_structure, vrel As Integer) As Boolean
        InsertStructureAsTables = False
        Try


            Dim insertPoint As Object
            insertPoint = _ao_object.application.Selection.Range
            insertPoint.InsertAfter(vbCr)
            insertPoint = _ao_object.Range(insertPoint.Start + 1, insertPoint.End + 1)
            insertPoint.Collapse(1)


            'Dim structureBoxDefintion As clsBox, structureTable As Word.Table
            Dim structureTable As Object
            For Each structureBoxDefintion In oStructure.boxes

                'Insert an inline table for the new structure box
                structureTable = _ao_object.Tables.Add(Range:=insertPoint, NumRows:=1, NumColumns:=1, AutoFitBehavior:=0)

                With structureTable
                    'Remove cell paddings
                    .TopPadding = 0
                    .BottomPadding = 0
                    .LeftPadding = 0
                    .RightPadding = 0

                    'Set dimensions
                    .Cell(1, 1).PreferredWidth = structureBoxDefintion.setwidth

                    .Cell(1, 1).HeightRule = 2
                    .Cell(1, 1).Height = structureBoxDefintion.setheight

                    'Set position on page
                    .Rows.WrapAroundText = True
                    .Rows.RelativeHorizontalPosition = 1
                    .Rows.RelativeVerticalPosition = vrel
                    .Rows.HorizontalPosition = structureBoxDefintion.setxpos

                    .Rows.VerticalPosition = structureBoxDefintion.setypos

                    .Rows.AllowOverlap = False

                    'Apply border
                    'If oStructure.Border <> "(none)" Then ApplyTableBorder(structureTable, oStructure.Border)

                    'Wrapping
                    'ApplyTableWrapping(structureTable, oStructure.wrapping)
                    'ApplyTableWrapping(structureTable, "")

                    'Extend the insert point's position by 2 (table + end of table row)
                    'so that when we insert the next structure we don't insert it into the table we've just created
                    insertPoint = _ao_object.Range(insertPoint.Start + 2, insertPoint.End + 2)
                    'Also insert a new paragraph which we'll hide so that each table is not associated with the same
                    'range so th at when the Information object is used on the table range it will not amalgamate the
                    'tables together - was causing problems when componentising structures
                    'If oStructure.boxes.Count > 1 Then
                    '    insertPoint.InsertAfter(vbCr)
                    '    'On Error Resume Next
                    '    'insertPoint.Paragraphs(1).Style = HIDDEN_STYLE
                    '    'If Err.Number <> 0 Then
                    '    '    'Create style
                    '    '    createHiddenStyle()
                    '    '    'Now apply again
                    '    '    insertPoint.Paragraphs(1).Style = HIDDEN_STYLE
                    '    'End If
                    '    'On Error GoTo errH
                    '    'insertPoint.Collapse wdCollapseEnd
                    '    'verticalOffset = verticalOffset - HIDDEN_PARA_HEIGHT
                    'End If

                End With
            Next

            'Insert a text wrapping break to force content after structures to be full margin width and
            'not be affected by wrapping of table structures
            insertPoint.InsertBreak(11)
            InsertStructureAsTables = True
        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to insert structure on page: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

        End Try


    End Function

    REM end structure wizard

    Public Overrides Sub screen_updating(turn_on As Boolean)
        _ao_object.application.screenupdating = turn_on
    End Sub
    Public Overrides Function delete_table() As Boolean
        Try
            structuretable.delete()

        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to get Delete Table : " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

        End Try
    End Function
    Public Overrides Function get_pub_type_id() As Long
        Try
            Return _ao_object.CustomDocumentProperties("rnet_pub_type_id").value
        Catch
            Return 0
        End Try
    End Function
    Public Overrides Function get_filename_as_property() As String
        get_filename_as_property = ""
        Try
            Dim pname As String
            Dim i As Integer = 1


            Dim found As Boolean = True
            While found
                Try
                    pname = "tw_" + CStr(table.table_id) + "_" + CStr(i)
                    get_filename_as_property = get_filename_as_property + _ao_object.CustomDocumentProperties(pname).value
                    i = i + 1
                Catch ex As Exception
                    found = False
                End Try
            End While

        Catch ex As Exception
            get_filename_as_property = ""
        End Try
    End Function
    Public Overrides Function get_sheetname_as_property() As String
        get_sheetname_as_property = ""
        Try
            Dim pname As String
            pname = "ts_" + CStr(table.table_id)
            get_sheetname_as_property = _ao_object.CustomDocumentProperties(pname).value()

        Catch ex As Exception
            get_sheetname_as_property = ""
        End Try
    End Function
    Public Overrides Function get_ref_as_property() As String
        get_ref_as_property = ""
        Try
            Dim pname As String
            pname = "tr_" + CStr(table.table_id)
            get_ref_as_property = _ao_object.CustomDocumentProperties(pname).value()

        Catch ex As Exception
            get_ref_as_property = ""
        End Try
    End Function
    Public Overrides Function insert_table(style_library As Integer, width As Double, left_offset As Double, rel_to_margin As Boolean, col_width As Cbc_am_table_wizard.bc_am_tw_table.COLUMN_WIDTH) As Boolean
        insert_table = False
        Try

            table_id = 0
            Dim rows As Integer = 0
            Dim cols As Integer = 0
            Dim split_start_row As Integer = 0
            Dim split_end_row As Integer = 0
            Dim table_width As Double = 0.0


            If table.has_title = True Then
                rows = rows + 1
                If table.has_sub_title = True Then
                    rows = rows + 1
                End If
            End If
            split_start_row = rows + 1

            rows = table.rows.Count + rows
            If (table.rows.Count > 0) Then
                cols = table.rows(0).cells.Count
            End If
            split_end_row = rows
            If table.has_source = True Then
                rows = rows + 1
            End If
            table_width = width

            Dim in_text_frame As Boolean = False
            If _ao_object.application.selection.range.tables.count = 0 Then
                Try

                    Dim insertPoint As Object
                    If _ao_object.application.selection.shaperange.count > 0 Then
                        insertPoint = _ao_object.application.selection.shaperange(1).TextFrame.TextRange
                        in_text_frame = True
                    Else

                        insertPoint = _ao_object.application.Selection.Range
                        insertPoint.InsertAfter(vbCr)
                        insertPoint = _ao_object.Range(insertPoint.Start + 1, insertPoint.End + 1)
                        insertPoint.Collapse(1)
                    End If
                    structuretable = _ao_object.Tables.Add(insertPoint, 1, 1)
                    With structuretable
                        'Remove cell paddings
                        .TopPadding = 0
                        .BottomPadding = 0
                        .LeftPadding = 0
                        .RightPadding = 0

                        'Set dimensions
                        .Cell(1, 1).PreferredWidth = table_width

                        .Cell(1, 1).HeightRule = 2
                        If in_text_frame = False Then


                            .Rows.WrapAroundText = True
                            If rel_to_margin = False Then
                                .Rows.RelativeHorizontalPosition = 1
                            Else
                                .Rows.RelativeHorizontalPosition = 2
                            End If
                        End If

                        .Rows.HorizontalPosition = left_offset
                        For i = 0 To rows - 2
                            structuretable.rows(1).range.rows.add()
                        Next

                    End With

                Catch ex As Exception
                    MsgBox(ex.Message)

                End Try
            Else
                structuretable = _ao_object.application.Selection.Tables(1)
                structuretable.Cell(1, 1).HeightRule = 0
                For i = 0 To rows - 2
                    structuretable.rows(1).range.rows.add()
                Next
            End If

            table_width = width
            structuretable.allowautofit = False

            structuretable.rows.HeightRule = 1

            For i = split_start_row To split_end_row
                structuretable.rows(i).Cells(1).Split(1, cols)
            Next
            Dim row_toggle As Boolean = False
            Dim last_heading_row As Integer
            Dim header_rows As Integer = 0
            Dim data_rows As Integer = 0
            Dim label_cols As Integer = 0
            For i = 0 To table.rows.Count - 1
                Select Case table.rows(i).row_type
                    Case Cbc_am_table_wizard.bc_am_tw_table.WORD_TABLE_ROW_TYPE.HEADING
                        header_rows = header_rows + 1
                    Case Else
                        data_rows = data_rows + 1
                End Select
                For j = 0 To table.rows(i).cells.Count - 1
                    With table.rows(i).cells(j)
                        Select Case col_width
                            Case Cbc_am_table_wizard.bc_am_tw_table.COLUMN_WIDTH.FROM_EXCEL
                                structuretable.rows(i + split_start_row).cells(j + 1).width = table_width * .percent_width
                            Case Cbc_am_table_wizard.bc_am_tw_table.COLUMN_WIDTH.EVEN
                                structuretable.rows(i + split_start_row).cells(j + 1).width = table_width / table.rows(i).cells.Count
                            Case Cbc_am_table_wizard.bc_am_tw_table.COLUMN_WIDTH.BEST_FIT
                                structuretable.allowautofit = True
                        End Select


                        Select Case table.rows(i).row_type


                            Case Cbc_am_table_wizard.bc_am_tw_table.WORD_TABLE_ROW_TYPE.DATA

                                If .column_type = Cbc_am_table_wizard.bc_am_tw_table.WORD_TABLE_COLUMN_TYPE.LABEL Then

                                    label_cols = label_cols + 1

                                End If


                        End Select
                    End With
                Next
            Next


            Dim bm As String
            Dim bmc As Integer
            bmc = _ao_object.range.bookmarks.count + 1
            bm = "tw_" + CStr(bmc) + "_sg" + CStr(style_library)
            If table.has_title Then
                bm = bm + "_t1"
            Else
                bm = bm + "_t0"
            End If
            If table.has_sub_title Then
                bm = bm + "_u1"
            Else
                bm = bm + "_u0"
            End If
            If table.has_source Then
                bm = bm + "_s1"
            Else
                bm = bm + "_s0"
            End If
            Dim c As Integer = 1
            table.heading_rows = header_rows

            table.data_rows = data_rows



            If header_rows + data_rows > 0 Then
                table.total_cols = table.rows(0).cells.Count
            End If

            If data_rows <> 0 Then
                table.label_cols = label_cols / data_rows
            End If



            REM if saved add this

            bm = bm + "_h" + CStr(table.heading_rows) + "_d" + CStr(table.data_rows) + "_l" + CStr(table.label_cols) + "_c" + CStr(table.total_cols)
            structuretable.range.bookmarks.add(bm)
            table_id = bmc

            table.library = style_library
            _ao_object.bookmarks(bm).range.select()


            insert_table = True



        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to Insert Table: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            screen_updating(True)

        Finally

        End Try
    End Function
    Public Overrides Function insert_data_into_table() As Boolean
        insert_data_into_table = False
        Try

            Dim rows As Integer = 0
            Dim cols As Integer = 0
            Dim split_start_row As Integer = 0
            Dim split_end_row As Integer = 0
            Dim table_width As Double = 0.0


            If table.has_title = True Then
                rows = rows + 1
                If table.has_sub_title = True Then
                    rows = rows + 1
                End If
            End If
            split_start_row = rows + 1

            rows = table.rows.Count + rows
            If (table.rows.Count > 0) Then
                cols = table.rows(0).cells.Count
            End If
            split_end_row = rows
            If table.has_source = True Then
                rows = rows + 1
            End If


            If table.has_title = True Then
                structuretable.range.rows(1).range.text = table.title
                If table.has_sub_title = True Then
                    structuretable.rows(2).range.text = table.sub_title
                End If
            End If


            If table.has_source = True Then
                structuretable.rows(rows).range.text = table.source
            End If

            For i = 0 To table.rows.Count - 1
                For j = 0 To table.rows(i).cells.Count - 1
                    With table.rows(i).cells(j)
                        structuretable.rows(i + split_start_row).cells(j + 1).range.text = .value
                    End With
                Next
            Next
            insert_data_into_table = True

        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to Insert  Data into Table: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            screen_updating(True)
        Finally
        End Try
    End Function
    Public Overrides Function set_excel_reference(fullname As String, sheetname As String, range As String)

        set_excel_reference = False
        Try
            Dim pname As String
            Const MAX As Integer = 250
            Dim pval As String
            Dim i As Integer = 0
            If table_id = 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "Failed to set Source Reference table id not set.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function

            End If

            pname = "tr_" + CStr(table_id)
            Try
                _ao_object.CustomDocumentProperties.Add(name:="tr_" + CStr(table_id), LinkToContent:=False, Type:=4, value:=range)
            Catch
                _ao_object.CustomDocumentProperties(pname).value = range
            End Try

            pname = "ts_" + CStr(table_id)
            Try
                _ao_object.CustomDocumentProperties.Add(name:="ts_" + CStr(table_id), LinkToContent:=False, Type:=4, value:=sheetname)
            Catch
                _ao_object.CustomDocumentProperties(pname).value = sheetname
            End Try


            pname = "tw_" + CStr(table_id) + "_" + CStr(i)
            While Len(fullname) > MAX
                pval = fullname.Substring(0, MAX)
                fullname = fullname.Substring(MAX, Len(fullname) - MAX)
                i = i + 1
                pname = "tw_" + CStr(table_id) + "_" + CStr(i)
                Try
                    _ao_object.CustomDocumentProperties.Add(name:=CStr(pname), LinkToContent:=False, Type:=4, value:=pval)
                Catch
                    _ao_object.CustomDocumentProperties(pname).value = pval
                End Try
            End While
            If Len(fullname) > 0 Then
                pname = "tw_" + CStr(table_id) + "_" + CStr(i + 1)
                Try
                    _ao_object.CustomDocumentProperties.Add(name:=CStr(pname), LinkToContent:=False, Type:=4, value:=fullname)
                Catch
                    _ao_object.CustomDocumentProperties(pname).value = fullname
                End Try
            End If

            REM split up into 255 parts
            set_excel_reference = True

        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to set Source Reference: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        End Try
    End Function

    Public Overrides Function format_table(settings As bc_om_table_wizard) As Boolean
        Try

            Dim rows As Integer
            Dim header_row_start As Integer = 1
            structuretable.Borders.Enable = False
            structuretable.Shading.BackgroundPatternColor = get_rgb("255255255")
            If table.has_title = True Then
                rows = rows + 1
                header_row_start = header_row_start + 1
                If settings.style_libraries(table.library).title_overline_weight <> 0.0 Then
                    structuretable.rows(1).Borders(-1).visible = True
                    structuretable.rows(1).Borders(-1).color = get_rgb(settings.style_libraries(table.library).title_overline_colour)
                    structuretable.rows(1).Borders(-1).LineWidth = settings.style_libraries(table.library).title_overline_weight
                End If
                If settings.style_libraries(table.library).title_underline_weight <> 0.0 Then
                    structuretable.rows(1).Borders(-3).visible = True
                    structuretable.rows(1).Borders(-3).color = get_rgb(settings.style_libraries(table.library).title_underline_colour)
                    structuretable.rows(1).Borders(-3).LineWidth = settings.style_libraries(table.library).title_underline_weight
                End If
                If settings.style_libraries(table.library).title_fill <> "" Then
                    If settings.style_libraries(table.library).title_fill <> "255255255" Then
                        structuretable.rows(1).range.shading.BackgroundPatternColor = get_rgb(settings.style_libraries(table.library).title_fill)
                    End If
                End If
                structuretable.rows(1).cells(1).range.style = settings.style_libraries(table.library).title_style


                If table.has_sub_title = True Then
                    rows = rows + 1
                    header_row_start = header_row_start + 1
                    structuretable.rows(2).cells(1).range.style = settings.style_libraries(table.library).sub_title_style
                    If settings.style_libraries(table.library).sub_title_underline_weight <> 0.0 Then
                        structuretable.rows(2).Borders(-3).visible = True
                        structuretable.rows(2).Borders(-3).color = get_rgb(settings.style_libraries(table.library).sub_title_underline_colour)
                        structuretable.rows(1).Borders(-1).LineWidth = settings.style_libraries(table.library).title_overline_weight
                    End If
                    If settings.style_libraries(table.library).sub_title_fill <> "" Then
                        If settings.style_libraries(table.library).sub_title_fill <> "255255255" Then
                            structuretable.rows(2).range.shading.BackgroundPatternColor = get_rgb(settings.style_libraries(table.library).sub_title_fill)
                        End If
                    End If
                End If
            End If

            rows = rows + table.heading_rows + table.data_rows + 1
            If table.has_source = True Then
                structuretable.rows(rows).cells(1).range.style = settings.style_libraries(table.library).source_style
                If settings.style_libraries(table.library).source_overline_weight <> 0.0 Then
                    structuretable.rows(rows).Borders(-1).visible = True
                    structuretable.rows(rows).Borders(-1).color = get_rgb(settings.style_libraries(table.library).source_overline_colour)
                    structuretable.rows(rows).Borders(-1).LineWidth = settings.style_libraries(table.library).source_overline_weight
                End If
                If settings.style_libraries(table.library).source_underline_weight <> 0.0 Then
                    structuretable.rows(rows).Borders(-3).visible = True
                    structuretable.rows(rows).Borders(-3).color = get_rgb(settings.style_libraries(table.library).source_underline_colour)
                    structuretable.rows(rows).Borders(-3).LineWidth = settings.style_libraries(table.library).source_underline_weight
                End If
                If settings.style_libraries(table.library).source_fill <> "" Then
                    If settings.style_libraries(table.library).source_fill <> "255255255" Then
                        structuretable.rows(rows).range.shading.BackgroundPatternColor = get_rgb(settings.style_libraries(table.library).source_fill)
                    End If
                End If
            End If



            Dim row_toggle As Boolean = False


            For i = header_row_start To header_row_start + table.heading_rows - 1
                For j = 1 To table.total_cols
                    structuretable.rows(i).cells(j).range.style = settings.style_libraries(table.library).heading_style
                Next
                If settings.style_libraries(table.library).heading_fill <> "" Then
                    If settings.style_libraries(table.library).heading_fill <> "255255255" Then
                        structuretable.rows(i).range.shading.BackgroundPatternColor = get_rgb(settings.style_libraries(table.library).heading_fill)
                    End If
                End If

                If i = header_row_start + table.heading_rows - 1 And settings.style_libraries(table.library).heading_underline_weight <> 0.0 Then
                    structuretable.rows(i).Borders(-3).visible = True
                    structuretable.rows(i).Borders(-3).color = get_rgb(settings.style_libraries(table.library).heading_underline_colour)
                    structuretable.rows(i).Borders(-3).LineWidth = settings.style_libraries(table.library).heading_underline_weight
                End If
            Next

            For i = header_row_start + table.heading_rows To header_row_start + table.heading_rows + table.data_rows - 1
                row_toggle = Not row_toggle


                For j = 1 To table.total_cols
                    If j <= table.label_cols Then
                        If row_toggle = True Then
                            structuretable.rows(i).cells(j).range.style = settings.style_libraries(table.library).label_style
                            If settings.style_libraries(table.library).data_row_fill <> "" Then
                                If settings.style_libraries(table.library).data_row_fill <> "255255255" Then
                                    structuretable.rows(i).cells(j).range.shading.BackgroundPatternColor = get_rgb(settings.style_libraries(table.library).data_row_fill)
                                End If
                            End If
                        Else
                            structuretable.rows(i).cells(j).range.style = settings.style_libraries(table.library).label_alt_style
                            If row_toggle = False And settings.style_libraries(table.library).data_alt_row_fill <> "" Then
                                If settings.style_libraries(table.library).data_alt_row_fill <> "255255255" Then
                                    structuretable.rows(i).cells(j).range.shading.BackgroundPatternColor = get_rgb(settings.style_libraries(table.library).data_alt_row_fill)
                                End If
                            End If
                        End If
                    Else
                        If row_toggle = True Then
                            structuretable.rows(i).cells(j).range.style = settings.style_libraries(table.library).data_style
                            If settings.style_libraries(table.library).data_row_fill <> "" Then
                                If settings.style_libraries(table.library).data_row_fill <> "255255255" Then
                                    structuretable.rows(i).cells(j).range.shading.BackgroundPatternColor = get_rgb(settings.style_libraries(table.library).data_row_fill)
                                End If
                            End If
                        Else
                            structuretable.rows(i).cells(j).range.style = settings.style_libraries(table.library).data_alt_row_style
                            If row_toggle = False And settings.style_libraries(table.library).data_alt_row_fill <> "" Then
                                If settings.style_libraries(table.library).data_alt_row_fill <> "255255255" Then
                                    structuretable.rows(i).cells(j).range.shading.BackgroundPatternColor = get_rgb(settings.style_libraries(table.library).data_alt_row_fill)
                                End If
                            End If
                        End If
                    End If
                Next
            Next

            format_table = True
        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to format Table: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            screen_updating(True)
        Finally

        End Try
    End Function

    Public Overrides Function get_font_name(style_name As String)
        Return _ao_object.styles(style_name).font.name
    End Function
    Public Overrides Function get_font_size(style_name As String)
        Return _ao_object.styles(style_name).font.size
    End Function
    Public Overrides Function get_font_bold(style_name As String)
        Return _ao_object.styles(style_name).font.bold
    End Function
    Public Overrides Function get_font_italic(style_name As String)
        Return _ao_object.styles(style_name).font.italic
    End Function
    Public Overrides Function get_font_alignment(style_name As String)
        Return _ao_object.styles(style_name).ParagraphFormat.Alignment()
    End Function
    Public Overrides Function get_font_colour(style_name As String)
        Return _ao_object.styles(style_name).font.color
    End Function
    Public Overrides Function check_cursor_insetion_point()
        check_cursor_insetion_point = False
        Try
            If _ao_object.application.selection.range.tables.count > 0 Then
                Exit Function
            End If
            check_cursor_insetion_point = True
        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to get cursor insertion point: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

        End Try
    End Function
    Public Overrides Function load_page_settings() As Boolean
        Try
            page_settings = New Cbc_am_page_structure.page_settings


            page_settings.page_number = _ao_object.application.Selection.Information(3)
            page_settings.section_number = _ao_object.application.Selection.Information(2)

            Try
                page_settings.pwidth = _ao_object.application.Selection.PageSetup.PageWidth()
            Catch
                load_page_settings = False
                Exit Function

            End Try
            page_settings.pheight = _ao_object.application.Selection.PageSetup.Pageheight()
            page_settings.orientation = _ao_object.application.Selection.PageSetup.orientation


            page_settings.tmargin = _ao_object.application.Selection.PageSetup.TopMargin()
            page_settings.bmargin = _ao_object.application.Selection.PageSetup.bottomMargin()
            page_settings.lmargin = _ao_object.application.Selection.PageSetup.leftMargin()
            page_settings.rmargin = _ao_object.application.Selection.PageSetup.rightMargin()
            page_settings.awidth = page_settings.pwidth - page_settings.lmargin - page_settings.rmargin
            page_settings.aheight = page_settings.pheight - page_settings.tmargin - page_settings.bmargin

            page_settings.cursor_pos = _ao_object.application.Selection.Information(6)
            load_page_settings = True
        Catch ex As Exception
            load_page_settings = False
        End Try

    End Function


    Public Overrides Function get_table_type() As Cbc_am_table_wizard.TABLE_TYPE
        Try
            If _ao_object.application.selection.shaperange.count > 0 Then
                If _ao_object.application.selection.shaperange(1).TextFrame.TextRange.tables.count = 1 Then
                    If is_table_tw(_ao_object.application.selection.shaperange(1).TextFrame.TextRange.tables(1)) <> "" Then
                        get_table_type = Cbc_am_table_wizard.TABLE_TYPE.TABLE_WIZARD
                        structuretable = _ao_object.application.selection.shaperange(1).TextFrame.TextRange.tables(1)
                    Else
                        get_table_type = Cbc_am_table_wizard.TABLE_TYPE.OTHER
                        structuretable = _ao_object.application.selection.shaperange(1).TextFrame.TextRange.tables(1)
                    End If
                Else

                    get_table_type = Cbc_am_table_wizard.TABLE_TYPE.STRUCTURE_OTHER
                    structure_width = _ao_object.application.selection.shaperange.width
                End If
            Else
                If _ao_object.application.Selection.range.tables.count = 0 Then
                    get_table_type = Cbc_am_table_wizard.TABLE_TYPE.NO_TABLE
                    structure_width = _ao_object.application.Selection.Range.PageSetup.PageWidth - _ao_object.application.Selection.Range.PageSetup.RightMargin - _ao_object.application.Selection.Range.PageSetup.leftMargin

                Else
                    If is_table_tw(_ao_object.application.selection.range.tables(1)) <> "" Then
                        get_table_type = Cbc_am_table_wizard.TABLE_TYPE.TABLE_WIZARD
                        structuretable = _ao_object.application.selection.range.tables(1)
                    ElseIf _ao_object.application.selection.range.tables(1).columns.count = 1 And _ao_object.application.selection.range.tables(1).rows.count = 1 Then
                        get_table_type = Cbc_am_table_wizard.TABLE_TYPE.STRUCTURE_TABLE
                        structuretable = _ao_object.application.selection.range.tables(1)
                        structure_width = structuretable.Cell(1, 1).Width
                    Else
                        get_table_type = Cbc_am_table_wizard.TABLE_TYPE.OTHER
                        structuretable = _ao_object.application.selection.range.tables(1)
                    End If
                End If
            End If

        Catch ex As Exception
            get_table_type = Cbc_am_table_wizard.TABLE_TYPE.INVALID_INSERTION_POINT
        End Try
    End Function

    Private Function is_table_tw(structuretable As Object) As String
        Try
            Dim bm As String
            is_table_tw = ""
            For i = 1 To structuretable.range.bookmarks.count
                bm = structuretable.range.bookmarks(i).name
                If Len(bm) > 2 And bm.Substring(0, 2) = "tw" Then
                    If parse_tw_bookmark(bm) = True Then
                        is_table_tw = bm
                    End If
                    Exit Function
                End If
            Next
        Catch
            is_table_tw = ""
        End Try
    End Function

    Private Function parse_tw_bookmark(bm As String) As Boolean
        parse_tw_bookmark = False
        Try
            REM parse bookmark for
            REM bm = bm.Substring(7, b'm.Length - 7)
            table = New Cbc_am_table_wizard.bc_am_tw_table
            table.table_id = bm.Substring(3, InStr(bm, "_sg") - 4)

            bm = bm.Substring(InStr(bm, "sg") + 1, bm.Length - InStr(bm, "sg") - 1)
            table.library = Left(bm, InStr(bm, "_") - 1)
            bm = bm.Substring(InStr(bm, "_") + 1, bm.Length - (InStr(bm, "_") + 1))
            table.has_title = Left(bm, InStr(bm, "_") - 1)
            bm = bm.Substring(InStr(bm, "_") + 1, bm.Length - (InStr(bm, "_") + 1))
            table.has_sub_title = Left(bm, InStr(bm, "_") - 1)
            bm = bm.Substring(InStr(bm, "_") + 1, bm.Length - (InStr(bm, "_") + 1))
            table.has_source = Left(bm, InStr(bm, "_") - 1)
            bm = bm.Substring(InStr(bm, "_") + 1, bm.Length - (InStr(bm, "_") + 1))
            table.heading_rows = Left(bm, InStr(bm, "_") - 1)
            bm = bm.Substring(InStr(bm, "_") + 1, bm.Length - (InStr(bm, "_") + 1))
            table.data_rows = Left(bm, InStr(bm, "_") - 1)
            bm = bm.Substring(InStr(bm, "_") + 1, bm.Length - (InStr(bm, "_") + 1))
            table.label_cols = Left(bm, InStr(bm, "_") - 1)
            bm = bm.Substring(InStr(bm, "_") + 1, bm.Length - (InStr(bm, "_") + 1))
            table.total_cols = bm
            table.pathname = get_filename_as_property()
            table.sheetname = get_sheetname_as_property()
            table.range = get_ref_as_property()
            table.has_data_source = False
            If table.pathname <> "" And table.sheetname <> "" And table.range <> "" Then
                table.has_data_source = True
            End If

            parse_tw_bookmark = True
        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to parse Tabe Wizard bookmark", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

        End Try
    End Function

    Public Overrides Function get_selected_table_structure(ByRef table As Cbc_am_table_wizard.bc_am_tw_table) As Boolean
        Try
            screen_updating(False)

            Dim total_rows As Integer = structuretable.rows.count
            Dim rows As Integer = 0

            If total_rows > 0 Then
                If structuretable.rows(1).cells.count = 1 Then
                    table.has_title = True
                    rows = rows + 1
                End If

                If total_rows > 1 Then
                    If structuretable.rows(2).cells.count = 1 Then
                        table.has_sub_title = True
                        rows = rows + 1
                    End If
                End If
            End If
            If total_rows > rows Then
                If structuretable.rows(total_rows).cells.count = 1 Then
                    table.has_source = True
                End If
            End If
            rows = rows + 1
            Dim header_stop As Boolean = False
            If table.has_source = False Then
                total_rows = total_rows + 1
            End If
            For i = rows To total_rows - 1
                If Len(structuretable.rows(i).cells(1).range.text) = 2 And header_stop = False Then
                    table.heading_rows = table.heading_rows + 1
                Else
                    header_stop = True

                    table.data_rows = table.data_rows + 1
                End If
                Dim label_stop As Boolean = False
                If i = rows Then
                    For j = 1 To structuretable.rows(i).cells.count
                        If Len(structuretable.rows(i).cells(j).range.text) = 2 And label_stop = False Then
                            table.label_cols = table.label_cols + 1
                        Else
                            label_stop = True
                        End If
                        table.total_cols = table.total_cols + 1
                    Next
                End If
            Next

            get_selected_table_structure = True
        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to Load Selected Table Structure: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        Finally
            screen_updating(True)

        End Try
    End Function


End Class









