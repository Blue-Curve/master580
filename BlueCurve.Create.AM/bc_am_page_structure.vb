Imports BlueCurve.Core.AS
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM.Nbc_om_pub_type_structures
Imports System.Windows.Forms




Public Class bc_am_page_structure
    Implements Ibc_am_page_structure

    Event insert_structure(st As bc_om_structure) Implements Ibc_am_page_structure.insert_structure



    Dim inbox As Boolean = False
    Dim _word_gui_ratio As Double
    Dim _page_structure As Cbc_am_page_structure.page_settings
    Const CM_POINTS = 28.3464
    Public _pub_type_structures As bc_om_pub_type_structures
    Public Sub New()

        ' This call is required by the designer.
        InitializeComponent()
        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")
        ' Add any initialization after the InitializeComponent() call.

    End Sub

    Function load_view(pub_type_structures As bc_om_pub_type_structures, page_structure As Cbc_am_page_structure.page_settings) As Boolean Implements Ibc_am_page_structure.load_view
        _pub_type_structures = pub_type_structures
        _page_structure = page_structure

        clear_structures()
        set_page()
        lstructures.Properties.Items.Clear()
        For i = 0 To _pub_type_structures.structures.Count - 1
            lstructures.Properties.Items.Add(_pub_type_structures.structures(i).name)
        Next
        If _pub_type_structures.structures.Count > 0 Then
            lstructures.SelectedIndex = 0

        End If
        load_view = True


    End Function

    Private Sub Button2_Click(sender As Object, e As EventArgs)
        clear_structures()
        set_page()
    End Sub
    Private Sub set_page()
        Try
            REM set page up at correct aspect ratio
            If _page_structure.orientation = bc_om_structure.PAGE_ORIENTATION.PORTRAIT Then
                Me.ppage.Width = 400
                Me.ppage.Height = Me.ppage.Width * (_page_structure.pheight / _page_structure.pwidth)
                Me.lstructures.Width = 400

            Else
                Me.ppage.Height = 400
                Me.ppage.Width = Me.ppage.Height * (_page_structure.pwidth / _page_structure.pheight)
                Me.lstructures.Width = Me.ppage.Width
            End If


            Me.parea.Width = Me.ppage.Width * (_page_structure.awidth / _page_structure.pwidth)
            Me.parea.Height = Me.ppage.Height * (_page_structure.aheight / _page_structure.pheight)
            Me.Height = Me.ppage.Height + 200
            Me.Width = Me.ppage.Width + 70

            REM set page margins 
            Dim dp As System.Drawing.Point
            dp.X = (ppage.Width * (_page_structure.lmargin / _page_structure.pwidth))
            dp.Y = (ppage.Height * (_page_structure.tmargin / _page_structure.pheight))
            Me.parea.Parent = Me.ppage

            Me.parea.Location = dp

            _word_gui_ratio = ppage.Width / _page_structure.pwidth


        Catch ex As Exception
            MsgBox(ex.Message)

        End Try

    End Sub

    Private Sub clear_structures()
        Dim k As Integer
        Me.ldesc.Text = ""
        While k < Me.ppage.Controls.Count
            If Me.ppage.Controls(k).Name.Substring(0, 4) = "bcst" Then
                Me.ppage.Controls.RemoveAt(k)
            Else
                k = k + 1
            End If
        End While
    End Sub




    WithEvents st As Panel
    Private Sub lstructures_SelectedIndexChanged(sender As Object, e As EventArgs) Handles lstructures.SelectedIndexChanged


        Dim tx As String
        Dim dp As System.Drawing.Point

        If lstructures.SelectedIndex < 0 Then
            Exit Sub
        End If
        Dim top As Double
        Dim left As Double
        Dim awidth As Double
        Dim aheight As Double

        If _pub_type_structures.structures(lstructures.SelectedIndex).offset_margin = True Then
            left = (_pub_type_structures.structures(lstructures.SelectedIndex).left_offset * _word_gui_ratio) + Me.parea.Location.X
            top = (_pub_type_structures.structures(lstructures.SelectedIndex).top_offset * _word_gui_ratio) + Me.parea.Location.Y
            awidth = parea.Width
            aheight = parea.Height
            If _pub_type_structures.structures(lstructures.SelectedIndex).page_structure = False Then
                top = _page_structure.cursor_pos * _word_gui_ratio
                tx = "Cursor Relative Structure"
            Else
                tx = "Page Structure"
            End If

        Else
            left = _pub_type_structures.structures(lstructures.SelectedIndex).left_offset
            top = _pub_type_structures.structures(lstructures.SelectedIndex).top_offset
            awidth = ppage.Width
            aheight = ppage.Height
            If _pub_type_structures.structures(lstructures.SelectedIndex).page_structure = False Then
                top = _page_structure.cursor_pos * _word_gui_ratio
                tx = "Cursor Relative Structure"
            Else
                tx = "Page Structure"
            End If
        End If


        clear_structures()

        For i = 0 To _pub_type_structures.structures(lstructures.SelectedIndex).boxes.Count - 1
            With _pub_type_structures.structures(lstructures.SelectedIndex).boxes(i)
                st = New Panel
                st.Visible = False
                st.Name = "bcst" + CStr(i)
                st.BackColor = System.Drawing.Color.Black
                st.Parent = ppage
                If _pub_type_structures.structures(lstructures.SelectedIndex).horizontal_absolute = False Then
                    dp.X = left + (.left_offset / 100.0) * awidth
                    st.Width = awidth * .width / 100.0

                Else
                    dp.X = left + (.left_offset * _word_gui_ratio)
                    st.Width = .width * _word_gui_ratio

                End If
                If _pub_type_structures.structures(lstructures.SelectedIndex).vertical_absolute = False Then
                    dp.Y = top + (.top_offset / 100.0) * aheight
                    st.Height = aheight * .height / 100.0


                Else
                    dp.Y = top + (.top_offset * _word_gui_ratio)
                    st.Height = .height * _word_gui_ratio

                End If
            End With
            st.Location = dp
            st.Visible = True
            AddHandler st.MouseMove, AddressOf show_box_size
            Me.ppage.Controls.Add(st)
            Me.ppage.Controls(Me.ppage.Controls.Count - 1).BringToFront()




        Next
        REM set desc
        Me.ldesc.Text = tx
        Me.binsert.Enabled = True

    End Sub
   

    Private Sub show_box_size(sender As Object, e As EventArgs) Handles st.MouseMove
        inbox = True
        setpos(sender)
    End Sub

    Private Sub binsert_Click(sender As Object, e As EventArgs) Handles binsert.Click
        RaiseEvent insert_structure(_pub_type_structures.structures(lstructures.SelectedIndex))
        Me.Hide()
    End Sub

    Private Sub ppage_MouseMove(sender As Object, e As MouseEventArgs) Handles ppage.MouseMove
        inbox = False
        setpos(Nothing)
    End Sub
    Private Sub parea_MouseMove(sender As Object, e As MouseEventArgs) Handles parea.MouseMove
        inbox = False
        setpos(Nothing)
    End Sub
    Sub setpos(sender As Object)
        Dim MousePos As System.Drawing.Point = Me.PointToClient(MousePosition)
        If inbox = False Then
            If runits.SelectedIndex = 0 Then
                lpos.Text = "Position: " + CStr(Math.Round((MousePos.X - Me.ppage.Location.X) / (CM_POINTS * _word_gui_ratio), 1)) + " : " + CStr(Math.Round((MousePos.Y - Me.ppage.Location.Y) / (CM_POINTS * _word_gui_ratio), 1))
            Else
                lpos.Text = "Position: " + CStr(Math.Round((MousePos.X - Me.ppage.Location.X) / (_word_gui_ratio), 1)) + " : " + CStr(Math.Round((MousePos.Y - Me.ppage.Location.Y) / (_word_gui_ratio), 1))
            End If
        Else
            If runits.SelectedIndex = 0 Then
                Me.lpos.Text = "Width: " + CStr(Math.Round(sender.Width / (CM_POINTS * _word_gui_ratio))) + " Height: " + CStr(Math.Round(sender.Height / (CM_POINTS * _word_gui_ratio)))

            Else
                Me.lpos.Text = "Width: " + CStr(Math.Round(sender.Width / _word_gui_ratio)) + " Height: " + CStr(Math.Round(sender.Height / _word_gui_ratio))
            End If

        End If
    End Sub

    Private Sub runits_SelectedIndexChanged(sender As Object, e As EventArgs) Handles runits.SelectedIndexChanged
        setpos(sender)
    End Sub

    Private Sub ppage_Paint(sender As Object, e As PaintEventArgs) Handles ppage.Paint

    End Sub

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub bc_am_page_structure_Load(sender As Object, e As EventArgs) Handles MyBase.Load

    End Sub

    Private Sub ldesc_Click(sender As Object, e As EventArgs) Handles ldesc.Click

    End Sub

 
End Class



Public Class Cbc_am_page_structure
    WithEvents _view As Ibc_am_page_structure
    Dim _ao_object As bc_ao_prod_tools

    Public Function load_data(view As Ibc_am_page_structure, obj As Object)
        load_data = False
        Try
            _view = view
            load_data = False
            Dim sobj As String
            sobj = CStr(obj.GetType.ToString)
            If InStr(LCase(sobj), "word") > 0 Then
                _ao_object = New bc_ao_word_prod_tools(obj)
            Else
                Dim omsg As New bc_cs_message("Blue Curve", "Application object: " + sobj + " not supported", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If





            REM load page settings
            If _ao_object.load_page_settings() = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Cannot insert structure at this position please move cursor position on page.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                load_data = False
                Exit Function
            End If
            Dim cursor_insertion_valid As Boolean
            cursor_insertion_valid = _ao_object.check_cursor_insetion_point()


            REM load structures
            Dim structures As New bc_om_pub_type_structures
            If populate_structures(structures) = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Failed to Load structures.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                load_data = False
                Exit Function
            End If

            Dim i As Integer
            Dim remove As Boolean = False
            Dim inc_page_structure As Boolean = False
            While i < structures.structures.Count
                remove = False
                With structures.structures(i)
                    If .page_structure = True Then
                        inc_page_structure = True
                    End If
                    For j = 0 To .page_structure_page_exclusions.Count - 1
                        If .page_structure_page_exclusions(j) = _ao_object.page_settings.page_number Then
                            remove = True
                            Exit For
                        End If
                    Next
                    For j = 0 To .page_structure_sections_exclusions.Count - 1
                        If .page_structure_sections_exclusions(j) = _ao_object.page_settings.section_number Then
                            remove = True
                            Exit For
                        End If
                    Next
                    If (.for_page_orientation = bc_om_structure.INSERT_PAGE_ORIENTATION.LANDSCAPE And _ao_object.page_settings.orientation <> bc_om_structure.PAGE_ORIENTATION.LANDSCAPE) Or (.for_page_orientation = bc_om_structure.INSERT_PAGE_ORIENTATION.PORTRAIT And _ao_object.page_settings.orientation <> bc_om_structure.PAGE_ORIENTATION.PORTRAIT) Then
                        remove = True
                    End If
                    If (.page_structure = False And cursor_insertion_valid = False) Then
                        remove = True
                    End If

                    If remove = True Then
                        structures.structures.RemoveAt(i)
                        i = i - 1
                    End If
                End With
                i = i + 1
            End While

            If structures.structures.Count = 0 And cursor_insertion_valid = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "No structures are defined for insertion at this part of the document.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                load_data = False
                Exit Function
            ElseIf structures.structures.Count = 0 And cursor_insertion_valid = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Cursor Needs to be in body of document. Please change cursor position", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                load_data = False
                Exit Function
            ElseIf structures.structures.Count > 0 And cursor_insertion_valid = False Then
                Dim omsg As New bc_cs_message("Blue Curve", "Cursor is not in body of document so only page structures are can be used.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
            End If


            Return _view.load_view(structures, _ao_object.page_settings)
        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to load data: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
        End Try
    End Function
  
    
    Private Function insert_structure(st As bc_om_structure) Handles _view.insert_structure
        insert_structure = False
        Try
            REM if a page structure then previous structures on the page will be deleted
            insert_structure = False
            Dim i As Integer = 1
            If st.page_structure = True Then
                Dim omsg As New bc_cs_message("Blue Curve", "Page structure will remove all existing structures and their content on this page. Do you wish to continue?", bc_cs_message.MESSAGE, True, False, "Yes", "No", True)
                If omsg.cancel_selected = True Then
                    insert_structure = False
                    Exit Function
                End If
                i = 1
                If _ao_object.delete_structure_on_page = False Then
                    Exit Function
                End If
            End If


            REM set up structure settings in object
            Dim toffset As Double
            Dim loffset As Double
            Dim width As Double
            Dim height As Double

            REM do overall offsets from top and left

            If st.offset_margin = False Then
                width = _ao_object.page_settings.pwidth
                height = _ao_object.page_settings.pheight


                loffset = st.left_offset
                If st.page_structure = True Then
                    toffset = st.top_offset
                Else
                    toffset = _ao_object.page_settings.cursor_pos
                End If


            Else
                width = _ao_object.page_settings.awidth
                height = _ao_object.page_settings.aheight
                loffset = st.left_offset + _ao_object.page_settings.lmargin
                If st.page_structure = True Then
                    toffset = st.top_offset + _ao_object.page_settings.tmargin
                Else
                    toffset = _ao_object.page_settings.cursor_pos
                End If
            End If
            REM now evaluate boxes
            Dim rowpos As Integer = 1
            For i = 0 To st.boxes.Count - 1

                With st.boxes(i)
                    .setrow = 1
                    If i > 0 AndAlso .setxpos <> st.boxes(i - 1).setxpos Then
                        rowpos = rowpos + 1
                        .setrow = rowpos
                    End If

                    If st.horizontal_absolute = True Then
                        .setxpos = loffset + .left_offset
                        .setwidth = .width
                    Else
                        .setxpos = loffset + (.left_offset / 100) * width
                        .setwidth = (.width / 100) * width
                    End If
                    If st.vertical_absolute = True Then
                        .setheight = .height
                        If st.page_structure = True Then
                            .setypos = toffset + .top_offset
                        Else
                            REM relative to cursor-paragraph
                            .setypos = .top_offset
                        End If
                    Else
                        .setypos = toffset + (.top_offset / 100) * height
                        .setheight = (.height / 100) * height
                    End If
                    'MsgBox(CStr(.setxpos) + ":" + CStr(.setypos) + " : " + CStr(.setwidth) + " : " + CStr(.setheight))



                End With
            Next
            REM now set the structures in word
            If st.page_structure = True Then
                _ao_object.InsertStructureAsTables(st, 1)
            Else
                _ao_object.InsertStructureAsTables(st, 2)
            End If
            insert_structure = True
        Catch ex As Exception
            Dim omsg As New bc_cs_message("Blue Curve", "Failed to insert structure: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

        End Try
    End Function

    Public Class page_settings
        Public pwidth As Double
        Public pheight As Double
        Public awidth As Double
        Public aheight As Double
        Public lmargin As Double
        Public rmargin As Double
        Public tmargin As Double
        Public bmargin As Double
        Public word_gui_ratio As Double
        Public page_number As Integer
        Public section_number As Integer
        Public tword As Object
        Public orientation As bc_om_structure.PAGE_ORIENTATION


        Public cursor_pos As Double
    End Class
  

    REM this will be from database at the end
    Function populate_structures(ByRef structures As bc_om_pub_type_structures) As Boolean
        Try
            Dim omsg As bc_cs_message
            Dim pub_type_id As String = ""

            pub_type_id = _ao_object.get_pub_type_id()
            If pub_type_id = "0" Then
                omsg = New bc_cs_message("Blue Curve create", "Document is not a Blue Curve document cannot insert structure", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Function
            End If

            Try
                Dim bcs As New bc_cs_central_settings(True)
                bc_am_load_objects.obc_pub_types = bc_am_load_objects.obc_pub_types.read_data_from_file(bc_cs_central_settings.local_template_path + "bc_pub_types.dat")
            Catch ex As Exception
                omsg = New bc_cs_message("Blue Curve create", "Cannot read metadata file: " + ex.Message, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)

                Exit Function
            End Try

          
            structures.structures.Clear()

            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = pub_type_id Then
                    For j = 0 To bc_am_load_objects.obc_pub_types.pubtype(i).structures.structures.count - 1
                        structures.structures.Add(bc_am_load_objects.obc_pub_types.pubtype(i).structures.structures(j))
                    Next
                    Exit For
                End If
            Next

            populate_structures = True

           

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_am_page_structure", "populate_structures", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function

End Class

Public Interface Ibc_am_page_structure
    Function load_view(structures As bc_om_pub_type_structures, page_settings As Cbc_am_page_structure.page_settings) As Boolean
    Event insert_structure(st As bc_om_structure)
End Interface


