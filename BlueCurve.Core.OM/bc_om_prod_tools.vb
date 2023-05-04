Imports BlueCurve.Core.CS
Namespace Nbc_om_table_wizard
    <Serializable> Public Class bc_om_table_wizard
        Inherits bc_cs_soap_base_class
        Public pub_type_id As Long
        Public sources As New List(Of String)
        Public style_libraries As New List(Of bc_om_style_library)
        <NonSerialized> Public table_width As Double
        Public table_sizes As New List(Of bc_om_table_size)

        Public Enum LINE_WEIGHT
            none = 0
            wdLineWidth025pt = 2
            wdLineWidth050pt = 4
            wdLineWidth075pt = 6
            wdLineWidth100pt = 8
            wdLineWidth150pt = 12
            wdLineWidth225pt = 18
            wdLineWidth300pt = 24
            wdLineWidth450pt = 36
            wdLineWidth600pt = 48
        End Enum
        Public Overrides Sub process_object()
            Select Case MyBase.tmode
                Case bc_cs_soap_base_class.tREAD
                    db_read()
            End Select
        End Sub
        Public Sub db_read()
            Dim res As Object
            Dim gdb As New db_bc_om_table_wizard
            Me.sources.Clear()
            res = gdb.get_source_names(pub_type_id, certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    Me.sources.Add(res(0, i))
                Next
            End If
            Me.style_libraries.Clear()
            res = gdb.get_style_libraries(pub_type_id, certificate)
            Dim sl As bc_om_style_library
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    sl = New bc_om_style_library
                    sl.library_name = res(0, i)
                    sl.title_style = res(1, i)
                    sl.title_overline_weight = res(2, i)
                    sl.title_overline_colour = res(3, i)
                    sl.title_underline_weight = res(4, i)
                    sl.title_underline_colour = res(5, i)
                    sl.title_fill = res(6, i)

                    sl.sub_title_style = res(7, i)
                    sl.sub_title_underline_weight = res(8, i)
                    sl.sub_title_underline_colour = res(9, i)
                    sl.sub_title_fill = res(10, i)

                    sl.heading_style = res(11, i)
                    sl.heading_underline_weight = res(12, i)
                    sl.heading_underline_colour = res(13, i)
                    sl.heading_fill = res(14, i)

                    sl.label_style = res(15, i)
                    sl.label_alt_style = res(16, i)

                    sl.data_style = res(17, i)
                    sl.data_alt_row_style = res(18, i)
                    sl.data_row_fill = res(19, i)
                    sl.data_alt_row_fill = res(20, i)

                    sl.source_style = res(21, i)
                    sl.source_overline_weight = res(22, i)
                    sl.source_overline_colour = res(23, i)
                    sl.source_underline_weight = res(24, i)
                    sl.source_underline_colour = res(25, i)
                    sl.source_fill = res(26, i)
                    Me.style_libraries.Add(sl)
                Next
            End If
            Me.table_sizes.Clear()
            res = gdb.get_table_sizes(pub_type_id, certificate)
            Dim ts As bc_om_table_size
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    ts = New bc_om_table_size
                    ts.name = res(0, i)
                    ts.rel_margin = res(1, i)
                    ts.in_left_margin = res(2, i)
                    ts.in_right_margin = res(3, i)
                    ts.perc_width = res(4, i)
                    ts.perc_left_offset = res(5, i)
                    Me.table_sizes.Add(ts)
                Next
            End If
        End Sub
        Public Class bc_om_fonts_for_style
            Public table_area As TABLE_AREA
            Public style_name As String
            Public font_name As String
            Public font_size As Single
            Public bold As Boolean
            Public italic As Boolean
            Public alignment As Integer
            Public colour As System.Drawing.Color
            Public objFont As System.Drawing.Font
        End Class
        <Serializable> Public Class bc_om_table_size
            Public name As String
            Public rel_margin As Boolean = True
            Public in_left_margin As Boolean = False
            Public in_right_margin As Boolean = False
            Public perc_width
            Public perc_left_offset
            REM run time calculated values
            <NonSerialized> Public calculated_width As Double
            <NonSerialized> Public calculated_offset As Double

        End Class
        Public Enum TABLE_AREA
            TITLE = 1
            SUB_TITLE = 2
            HEADING = 3
            LABEL = 5
            LABEL_ALT = 6
            DATA = 8
            DATA_ALT = 9
            SOURCE = 10
        End Enum

        <Serializable> Public Class bc_om_style_library
            Public id As Integer
            Public library_name As String
            <NonSerialized> Public exclude As Boolean = False
            <NonSerialized>Public fonts_for_style As New List(Of bc_om_fonts_for_style)
            Public title_style As String
            Public title_overline_weight As LINE_WEIGHT = 0
            Public title_overline_colour As String = ""
            Public title_underline_weight As LINE_WEIGHT = 0
            Public title_underline_colour As String = ""
            Public title_fill As String = ""

            Public sub_title_style As String
            Public sub_title_underline_weight As LINE_WEIGHT = 0
            Public sub_title_underline_colour As String
            Public sub_title_fill As String = ""

            Public heading_style As String
            Public heading_underline_weight As LINE_WEIGHT = 0
            Public heading_underline_colour As String
            Public heading_fill As String = ""

            Public label_style As String
            Public label_alt_style As String


            Public data_style As String
            Public data_alt_row_style As String
            Public data_row_fill As String
            Public data_alt_row_fill As String


            Public source_style As String
            Public source_overline_weight As LINE_WEIGHT = 0
            Public source_overline_colour As String
            Public source_underline_weight As LINE_WEIGHT = 0
            Public source_underline_colour As String
            Public source_fill As String

        End Class
        Class db_bc_om_table_wizard
            Dim gdb As New bc_cs_db_services
            Friend Function get_source_names(pub_type_id As Long, ByRef certificate As bc_cs_security.certificate)
                Dim sql As String
                sql = "exec dbo.bc_core_pt_tw_get_source_names " + CStr(pub_type_id)
                get_source_names = gdb.executesql(sql, certificate)
            End Function
            Friend Function get_style_libraries(pub_type_id As Long, ByRef certificate As bc_cs_security.certificate)
                Dim sql As String
                sql = "exec dbo.bc_core_pt_tw_get_style_libraries " + CStr(pub_type_id)
                get_style_libraries = gdb.executesql(sql, certificate)
            End Function
            Friend Function get_table_sizes(pub_type_id As Long, ByRef certificate As bc_cs_security.certificate)
                Dim sql As String
                sql = "exec dbo.bc_core_pt_tw_get_table_sizes " + CStr(pub_type_id)
                get_table_sizes = gdb.executesql(sql, certificate)
            End Function
        End Class
    End Class

End Namespace
Namespace Nbc_om_pub_type_structures

    <Serializable> Public Class bc_om_pub_type_structures
        Inherits bc_cs_soap_base_class
        Public pub_type_id As Long
        Public structures As New List(Of bc_om_structure)

        Public Overrides Sub process_object()
            Select Case MyBase.tmode
                Case bc_cs_soap_base_class.tREAD
                    db_read()

            End Select
        End Sub
        Public Sub db_read()
            Try
                Dim res, eres As Object
                Dim gdb As New db_bc_om_pub_type_structures
                Dim st As bc_om_structure
                Dim box As bc_om_structure.bc_om_box
                structures.Clear()

                res = gdb.get_structures_for_pub_type(Me.pub_type_id, certificate)
                If IsArray(res) Then
                    For i = 0 To UBound(res, 2)
                        st = New bc_om_structure
                        st.id = res(0, i)
                        st.name = res(1, i)
                        st.for_page_orientation = res(2, i)
                        st.left_offset = res(3, i)
                        st.top_offset = res(4, i)
                        st.offset_margin = res(5, i)
                        st.horizontal_absolute = res(6, i)
                        st.vertical_absolute = res(7, i)
                        st.page_structure = res(8, i)

                        REM page exclusions
                        eres = gdb.get_page_exclusions_for_structure(st.id, certificate)
                        If IsArray(eres) Then
                            For j = 0 To UBound(eres, 2)
                                st.page_structure_page_exclusions.Add(eres(0, j))
                            Next
                        End If
                        REM section exclsions
                        eres = gdb.get_section_exclusions_for_structure(st.id, certificate)
                        If IsArray(eres) Then
                            For j = 0 To UBound(eres, 2)
                                st.page_structure_sections_exclusions.Add(eres(0, j))
                            Next
                        End If
                        REM boxes
                        eres = gdb.get_boxes_for_structure(st.id, certificate)
                        If IsArray(eres) Then
                            For j = 0 To UBound(eres, 2)
                                box = New bc_om_structure.bc_om_box
                                box.height = eres(0, j)
                                box.width = eres(1, j)
                                box.left_offset = eres(2, j)
                                box.top_offset = eres(3, j)
                                st.boxes.Add(box)

                            Next
                        End If
                        structures.Add(st)
                    Next
                End If

            Catch ex As Exception
                Dim oerr As New bc_cs_error_log("bc_om_pub_type_structures", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            End Try
        End Sub
        Class db_bc_om_pub_type_structures
            Dim gdb As New bc_cs_db_services
            Friend Function get_structures_for_pub_type(pt_id As Long, ByRef certificate As bc_cs_security.certificate)
                get_structures_for_pub_type = gdb.executesql("exec dbo.bc_core_prod_tools_get_structures_for_pt " + CStr(pt_id), certificate)
            End Function
            Friend Function get_page_exclusions_for_structure(id As Long, ByRef certificate As bc_cs_security.certificate)
                get_page_exclusions_for_structure = gdb.executesql("exec dbo.bc_core_prod_tools_get_page_excl_for_structure " + CStr(id), certificate)
            End Function
            Friend Function get_section_exclusions_for_structure(id As Long, ByRef certificate As bc_cs_security.certificate)
                get_section_exclusions_for_structure = gdb.executesql("exec dbo.bc_core_prod_tools_get_section_excl_for_structure " + CStr(id), certificate)
            End Function
            Friend Function get_boxes_for_structure(id As Long, ByRef certificate As bc_cs_security.certificate)
                get_boxes_for_structure = gdb.executesql("exec dbo.bc_core_prod_tools_get_boxes_for_structure " + CStr(id), certificate)
            End Function
        End Class
    End Class

    <Serializable> Public Class bc_om_structure
        Public id As Long
        Public boxes As New List(Of bc_om_box)
        Public name As String
        Public for_page_orientation As INSERT_PAGE_ORIENTATION = INSERT_PAGE_ORIENTATION.BOTH
        Public left_offset As Double
        Public top_offset As Double
        Public offset_margin As Boolean = False
        Public horizontal_absolute As Boolean = True
        Public vertical_absolute As Boolean = True
        REM page structures
        Public page_structure As Boolean = False
        Public page_structure_page_exclusions As New List(Of Integer)
        Public page_structure_sections_exclusions As New List(Of Integer)

        <Serializable> Public Class bc_om_box
            Public height As Double
            Public width As Double
            Public left_offset As Double
            Public top_offset As Double
            REM settings to set it up in word
            Public setwidth As Double
            Public setheight As Double
            Public setxpos As Double
            Public setypos As Double
            Public setrow As Integer
        End Class
        Public Enum INSERT_PAGE_ORIENTATION
            BOTH = 0
            PORTRAIT = 1
            LANDSCAPE = 2
        End Enum
        Public Enum PAGE_ORIENTATION
            PORTRAIT = 0
            LANDSCAPE = 1
        End Enum
       
    End Class
End Namespace

