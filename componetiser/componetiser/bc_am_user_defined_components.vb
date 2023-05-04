Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports System.IO
Public Class bc_am_user_defined_components
    'Public oworddocument As Object
    Public loading As Boolean = False
    Public ocomps As New bc_om_user_defined_components
    Public mode As Integer
    Public ao_word As New bc_ao_word


    Private Sub bc_am_user_defined_components_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Dim oword As Object
        oword = GetObject(, "word.application")
        ao_word.worddocument = oword.activedocument
        Dim bcs As New bc_cs_central_settings(True)
        If mode = 1 Then
            set_insert_mode()
        Else
            set_preview()
        End If
    End Sub
    Private Sub set_insert_mode()
        loading = True
        Me.Text = "Insert User Defined Component Into Document - Blue Curve"
        Dim tp As TimeSpan
        tp = New TimeSpan(28, 0, 0, 0, 0)
        Dim da As DateTime
        da = Now
        Me.Dfrom.Value = da.Subtract(tp)
        Me.Dto.Value = Now
        Me.Ltitle.Visible = False
        Me.Ttitle.Visible = False
        Me.breload.Visible = False
        Me.TabControl1.TabPages(1).Enabled = False
        Me.rprivate.Visible = True
        Me.rpublic.Visible = True
        Me.bsave.Enabled = False
        Me.bsave.Text = "Insert"
        Me.rprivate.Checked = True
        loading = False
        load_comps(True)
    End Sub
    Private Sub load_comps(ByVal from_db As Boolean)
        Me.Cursor = Cursors.WaitCursor
        Try
            Me.TabControl1.TabPages(1).Enabled = False
            Me.bsave.Enabled = False
            Me.tpreview.Rtf = ""
            Me.TabControl1.TabPages(1).Text = "Preview"
            If loading = True Then
                Exit Sub
            End If
            ocomps.date_from = Me.Dfrom.Value
            ocomps.date_to = Me.Dto.Value
            ocomps.user_id = 4
            ocomps.public_flag = Me.rpublic.Checked
            Me.Lcomps.Items.Clear()
            If from_db = True Then
                ocomps.db_read()
                load_dynamic_lists()
            End If
            Dim lvw As ListViewItem


            For i = 0 To ocomps.udcs.Count - 1
                If (Me.Cpubtype.Text = "All" Or Me.Cpubtype.Text = ocomps.udcs(i).pub_type) And (Me.Cauthor.Text = "All" Or Me.Cauthor.Text = ocomps.udcs(i).author) And (Me.Centity.Text = "All" Or Me.Centity.Text = ocomps.udcs(i).entity) Then

                    lvw = New ListViewItem(CStr(Format(ocomps.udcs(i).comp_date, "dd-MMM-yyyy")))
                    lvw.SubItems.Add(ocomps.udcs(i).title)
                    lvw.SubItems.Add(ocomps.udcs(i).pub_type)
                    lvw.SubItems.Add(ocomps.udcs(i).author)
                    lvw.SubItems.Add(ocomps.udcs(i).entity)
                    lvw.SubItems.Add(ocomps.udcs(i).udc_id)
                    Me.Lcomps.Items.Add(lvw)
                End If
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_user_defined_components", "load_comps", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub
    Private Sub set_preview()
        Try
           

            Dim rtf As String
            Me.Text = "Create User Defined Component - Blue Curve"

            Me.tpreview.Rtf = ""
            Me.Ttitle.Text = ""
            If Me.TabControl1.TabPages.Count > 1 Then
                Me.TabControl1.TabPages.RemoveAt(0)
            End If

            REM firstly see if inside a user dfined component already
            Me.bsave.Text = "Save"
            Me.TabControl1.TabPages(0).Text = "Preview New Component"
            If ao_word.udi_get_selected_udi_id > 0 Then
                Me.bsave.Text = "Update"
                Me.Text = "Update User Defined Component - Blue Curve"
                Me.TabControl1.TabPages(0).Text = "Preview Existing"
            End If

            rtf = ao_word.get_selection_in_rtf
            If rtf = "" Then
                Dim omsg As New bc_cs_message("Blue Curve", "Nothing selected in document!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Me.Hide()
                Exit Sub
            End If
            Me.tpreview.Rtf = rtf

            Dim tx As String
            tx = ao_word.udi_get_first_paragraph
             If Asc(tx.Substring(tx.Length - 1, 1)) = 13 Then
                tx = tx.Substring(0, tx.Length - 1)
            End If
            Me.Ttitle.Text = tx
        Catch ex As Exception
            Me.tpreview.Rtf = ""
            Me.Ttitle.Text = ""
            Dim omsg As New bc_cs_message("Blue Curve", "Nothing selected in document!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

        End Try
    End Sub

    Private Sub Button4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button4.Click
        Me.Hide()
    End Sub

    Private Sub breload_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles breload.Click
        set_preview()
    End Sub

    Private Sub tpreview_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs)

    End Sub

    Private Sub cwrap_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cwrap.CheckedChanged
        If Cwrap.Checked = True Then
            Me.tpreview.WordWrap = True
        Else
            Me.tpreview.WordWrap = False
        End If
    End Sub

    Private Sub Ttitle_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ttitle.TextChanged
        If Trim(Ttitle.Text) <> "" Then
            Me.bsave.Enabled = True
        End If
    End Sub

    Private Sub bsave_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bsave.Click
        If Me.TabControl1.TabPages.Count = 1 Then
            Dim udc_id As Long = 0
            If Me.bsave.Text = "Update" Then
                udc_id = ao_word.udi_get_selected_udi_id

            End If
            save_user_component(udc_id)
            Me.Hide()
        Else
            insert_component()
        End If
    End Sub
    Private Sub save_user_component(ByVal udc_id As Long)
        REM get metata from document pub type, entity author
        Dim ocomp As New bc_om_user_defined_component
        ocomp.udc_id = udc_id
        ocomp.title = Me.Ttitle.Text
        ocomp.rtf = Me.tpreview.Rtf
        ocomp.public_flag = Me.rpublic.Checked
        ocomp.pub_type_id = 1
        ocomp.author_id = 4
        ocomp.entity_id = 1
        ocomp.last_updated_from_doc_id = 11
        ocomp.paras = ao_word.udi_get_number_of_paragraphs()
        ocomp.db_write()
        REM save down and retrieve new id
        REM set id around selection
        ao_word.udi_set_bookmark("rnet_udc_" + CStr(ocomp.udc_id))
       End Sub
    Private Sub insert_component()
        REM firstly check it doesnt alreay exist
        Dim bm As String
        Dim pg As Integer
        Dim omsg As bc_cs_message
        For i = 0 To Me.ocomps.udcs.Count - 1
            If Me.ocomps.udcs(i).udc_id = CInt(Me.Lcomps.SelectedItems(0).SubItems(5).Text) Then
                bm = "rnet_udc_" + CStr(Me.Lcomps.SelectedItems(0).SubItems(5).Text)
                pg = ao_word.udi_component_exists(bm)
                If pg > 0 Then
                    omsg = New bc_cs_message("Blue Curve", "Version of Component: " + Me.ocomps.udcs(i).title + " already exists on page: " + CStr(pg) + " of  document cannot insert again!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    Exit Sub
                Else
                    REM place text and bookmark it
                    REM cant place a component in an existing component
                    If ao_word.udi_get_selected_udi_id > 0 Then
                        omsg = New bc_cs_message("Blue Curve", "insertion point is already inside another user defined component, please select another area of the document.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Exit Sub
                    End If
                    ao_word.udi_set_bookmark(bm)
                    ao_word.set_selection_from_rtf(Me.ocomps.udcs(i).rtf)
                    ao_word.udi_set_bookmark_around_paragraphs(bm, Me.ocomps.udcs(i).paras)
                    REM now make sure bookmark goes around insertion
                    Me.Lcomps.SelectedItems.Clear()
                    Me.Hide()
                End If

                Exit For
            End If
        Next

    End Sub

    


                
  

    Private Sub TabControl1_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles TabControl1.SelectedIndexChanged
        If Me.TabControl1.SelectedIndex = 1 Then
            Me.rprivate.Visible = False
            Me.rpublic.Visible = False
        Else
            Me.rprivate.Visible = True
            Me.rpublic.Visible = True
        End If
    End Sub

    Private Sub Panel2_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel2.Paint

    End Sub

    Private Sub Ttitle_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Ttitle.TextChanged

    End Sub

    Private Sub RadioButton1_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rprivate.CheckedChanged
        If Me.TabControl1.TabPages.Count > 1 Then
            If Me.rprivate.Checked = True Then
                load_comps(True)
            End If
        End If
    End Sub

    Private Sub Lcomps_DoubleClick(ByVal sender As Object, ByVal e As System.EventArgs) Handles Lcomps.DoubleClick
        If Lcomps.SelectedItems.Count = 1 Then
            Me.bsave.Enabled = True
            show_preview()
            Me.TabControl1.SelectedTab = Me.TabControl1.TabPages(1)


        End If
    End Sub
    Private Sub clear_preview()
        Me.TabControl1.TabPages(1).Enabled = False
        Me.bsave.Enabled = False
        Me.tpreview.Rtf = ""
        Me.TabControl1.TabPages(1).Text = "Preview"
        Me.TabControl1.SelectedTab = Me.TabControl1.TabPages(0)
    End Sub
    Private Sub Lcomps_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Lcomps.SelectedIndexChanged
        clear_preview()
        If Lcomps.SelectedItems.Count = 1 Then
            Me.bsave.Enabled = True

            show_preview()
        End If
    End Sub
    Private Sub show_preview()
        For i = 0 To ocomps.udcs.Count - 1
            If ocomps.udcs(i).udc_id = CInt(Me.Lcomps.SelectedItems(0).SubItems(5).Text) Then
                Me.tpreview.Rtf = ocomps.udcs(i).rtf
                Me.TabControl1.TabPages(1).Enabled = True
                Me.bsave.Enabled = True
                Me.TabControl1.TabPages(1).Text = "Preview: " + Me.Lcomps.SelectedItems(0).SubItems(1).Text

                Exit For
            End If
        Next
    End Sub
    Private Sub RadioButton2_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rpublic.CheckedChanged
        If Me.TabControl1.TabPages.Count > 1 Then

            If Me.rpublic.Checked = True Then
                load_comps(True)
            End If
        End If
    End Sub

    Private Sub Dfrom_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Dfrom.ValueChanged
        load_comps(True)
    End Sub

    Private Sub Dto_ValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Dto.ValueChanged
        load_comps(True)
    End Sub
    Private Sub load_dynamic_lists()
        Try
            loading = True
            Dim op As String
            Dim oa As String
            Dim oe As String
            Dim os As String
            op = Me.Cpubtype.Text
            oa = Me.Cauthor.Text
            oe = Me.Centity.Text

            Me.Cpubtype.Items.Clear()
            Me.Cpubtype.Items.Add("All")
            Me.Centity.Items.Clear()
            Me.Centity.Items.Add("All")
            Me.Cauthor.Items.Clear()
            Me.Cauthor.Items.Add("All")
            Dim i, j As Integer
            Dim found As Boolean
            For i = 0 To ocomps.udcs.Count - 1
                found = False
                For j = 1 To Me.Cpubtype.Items.Count - 1
                    If CStr(Me.Cpubtype.Items(j)) = CStr(ocomps.udcs(i).pub_type) Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    Me.Cpubtype.Items.Add(CStr(ocomps.udcs(i).pub_type))
                End If

                found = False
                For j = 1 To Me.Centity.Items.Count - 1
                    If CStr(Me.Centity.Items(j)) = CStr(ocomps.udcs(i).entity) Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    Me.Centity.Items.Add(ocomps.udcs(i).entity)
                End If
                found = False
                For j = 1 To Me.Cauthor.Items.Count - 1
                    If CStr(Me.Cauthor.Items(j)) = CStr(ocomps.udcs(i).author) Then
                        found = True
                        Exit For
                    End If
                Next
                If found = False Then
                    Me.Cauthor.Items.Add(ocomps.udcs(i).author)
                End If

            Next

            Try
                Me.Cpubtype.Text = op
                If Me.Cpubtype.SelectedIndex = -1 Then
                    Me.Cpubtype.SelectedIndex = 0
                End If
            Catch ex As Exception
                Me.Cpubtype.SelectedIndex = 0
            End Try
            Try
                Me.Centity.Text = oe
                If Me.Centity.SelectedIndex = -1 Then
                    Me.Centity.SelectedIndex = 0
                End If
            Catch ex As Exception
                Me.Centity.SelectedIndex = 0
            End Try
            Try
                Me.Cauthor.Text = oa
                If Me.Cauthor.SelectedIndex = -1 Then
                    Me.Cauthor.SelectedIndex = 0
                End If
            Catch ex As Exception
                Me.Cauthor.SelectedIndex = 0

            End Try



        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_ins_adhoc_component", "load_dynamic_lists", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            loading = False
        End Try
    End Sub

    Private Sub Cauthor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cauthor.SelectedIndexChanged
        load_comps(False)
    End Sub

    Private Sub Cpubtype_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cpubtype.SelectedIndexChanged
        load_comps(False)
    End Sub

    Private Sub Centity_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Centity.SelectedIndexChanged
        load_comps(False)
    End Sub
End Class

