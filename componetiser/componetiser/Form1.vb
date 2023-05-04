Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Public Class Form1
    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        Dim off As New bc_am_user_defined_components

        off.mode = 0
        off.Show()
    End Sub

    Private Sub Button2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Dim off As New bc_am_user_defined_components

        off.mode = 1
        off.Show()
    End Sub


    Private Sub Button3_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button3.Click
        Dim oexcel As Object
        oexcel = GetObject(, "excel.application")
        REM set up parameters
        Dim bcs As New bc_cs_central_settings(True)
        Dim otemplate As New bc_om_excel_data_template
        otemplate.entity_class_name = "Instrument"
        otemplate.entity_identifier_type = "attribute.ric_code"
        otemplate.entity_list_start_col = 2
        otemplate.entity_list_start_row = 3
        otemplate.entity_list_end_delimiter = ""
        otemplate.draft = False
        otemplate.publish = True
        Dim oitem As New bc_om_excel_data_template.bc_om_excel_data_item
        oitem.entity_identifier_start_col = 8
        oitem.item_identifier = "attribute.core.price"
        oitem.submission_code = 2
        oitem.datatype = 0
        oitem.mandatory = True
        otemplate.items.Add(oitem)
        load_data(otemplate, oexcel)

    End Sub
    Private Sub load_data(ByVal otemplate As bc_om_excel_data_template, ByVal oexcel As Object)
        Dim oelements As New bc_om_excel_data_elements
        Dim oelement As bc_om_excel_data_element
        Dim row As Integer
        Dim i As Integer
        Dim err_tx As String
        Dim eerr As bc_om_excel_data_element_error
        Dim errors As New ArrayList
        row = otemplate.entity_list_start_row

        While oexcel.activesheet.cells(row, otemplate.entity_list_start_col).value <> otemplate.entity_list_end_delimiter
            REM read in each item
            For i = 0 To otemplate.items.Count - 1
                oelement = New bc_om_excel_data_element
                oelement.entity_key = oexcel.activesheet.cells(row, otemplate.entity_list_start_col).value
                oelement.item_key = otemplate.items(i).item_identifier
                oelement.value = oexcel.activesheet.cells(row, otemplate.items(i).entity_identifier_start_col).value
                oelement.submission_code = otemplate.items(i).submission_code
                err_tx = client_side_validate(oelement.value, otemplate.items(i).mandatory, otemplate.items(i).datatype, oelement.entity_key, oelement.item_key)
                If err_tx = "" Then
                    oelements.elements.Add(oelement)
                Else
                    eerr = New bc_om_excel_data_element_error
                    eerr.row = row
                    eerr.col = otemplate.items(i).entity_identifier_start_col
                    eerr.err_text = err_tx
                    errors.Add(eerr)
                End If
            Next
            row = row + 1

        End While
        If errors.Count = 0 Then
            oelements.db_write()
        Else
            For i = 0 To errors.Count - 1
                Debug.Print(errors(i).err_text + " cell(" + CStr(errors(i).row) + ":" + CStr(errors(i).col))
            Next
        End If


    End Sub
    Private Function client_side_validate(ByVal value As String, ByVal mandatory As Boolean, ByVal datatype As Integer, ByVal entity_key As String, ByVal item_key As String) As String
        client_side_validate = ""
        If mandatory = True And Trim(value) = "" Then
            client_side_validate = item_key + "  must be entered for " + entity_key
        End If
    End Function

End Class