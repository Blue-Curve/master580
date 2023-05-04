Imports BlueCurve.Core.CS
Imports System.IO
Imports System.Xml.Serialization
<Serializable()> Public Class bc_om_excel_data_elements
    Inherits bc_cs_soap_base_class
    Public elements As New ArrayList
    Public errors As New ArrayList
    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_write()
        Try
            Dim gdb As New bc_om_excel_io_db
            Dim vres As Object
            REM write data
            vres = gdb.write_data(Me.write_data_to_xml(MyBase.certificate), MyBase.certificate)
            REM construct errors back to clienet


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_excel_data_elements", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        End Try
    End Sub
    Public Overrides Function write_data_to_xml(ByRef certifiate As bc_cs_security.certificate) As String
        Dim otrace As New bc_cs_activity_log("bc_om_excel_data_elements", "write_data_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim cbc_data_services As New bc_cs_data_services
        Dim ocommentary As New bc_cs_activity_log("bc_om_excel_data_elements", "write_data_to_xml", bc_cs_activity_codes.COMMENTARY, "Attempting to write XML string")

        write_data_to_xml = soap_serialize_object_to_xml(Me, Me.certificate)

        otrace = New bc_cs_activity_log("bc_om_excel_data_elementst", "write_data_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    End Function

    Private Function soap_serialize_object_to_xml(ByVal o As Object, ByRef certificate As bc_cs_security.certificate, Optional ByVal packet_code As String = "") As String

        Dim otrace As New bc_cs_activity_log("bc_om_excel_data_elements", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Dim bc_cs_central_settings As New bc_cs_central_settings
        Dim by() As Byte

        Try
            Using ms As New MemoryStream

                Dim extratype(1) As Type
                extratype(0) = GetType(bc_om_excel_data_element)
                extratype(1) = GetType(bc_om_excel_data_element_error)

                Dim sf As New XmlSerializer(o.GetType, extratype)

                sf.Serialize(ms, o)

                by = ms.ToArray()

                ms.Close()

                'xml serialization does not always encode apostrophe's.  This causes an issue when passing to SQL Server
                soap_serialize_object_to_xml = Replace(System.Text.Encoding.UTF8.GetString(by), "'", "&#39;")

            End Using

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_excel_data_elements", "soap_serialize_object_to_xml", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            soap_serialize_object_to_xml = ""
        Finally
            otrace = New bc_cs_activity_log("bc_om_excel_data_elements", "soap_serialize_object_to_xml", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function

End Class
<Serializable()> Public Class bc_om_excel_data_element
    Inherits bc_cs_soap_base_class
    REM keys
    Public entity_key As String
    Public entity As String
    Public class_name As String
    Public item_key As String
    REM  validation check
    Public validation_text As String
    Public validation_failed As Boolean
    REM metadata
    Public submission_code As Integer
    Public draft As Boolean
    Public publish As Boolean
    Public date_from As Date
    Public date_to As Date
    Public year As Integer
    Public period_id As Integer
    Public contributor_id As Integer
    REM data
    Public row As Integer
    Public col As Integer
    Public value As String
End Class
<Serializable()> Public Class bc_om_excel_data_element_error
    Public row As Integer
    Public col As Integer
    Public err_text As String
End Class
REM this may become a config file instead
Public Class bc_om_excel_data_template
    REM descrition of template need one template for load type
    Public template_id As Long
    Public template_name As String
    REM identifier where and what the entity identifier is
    Public entity_class_name As String
    REM Entity_id= actual id Entity_name = entity_name <attribute_name> attribute of entity
    Public entity_identifier_type As String
    REM metadata
    Public draft As Boolean
    Public publish As Boolean
    REM data location
    Public entity_list_start_row As Integer
    Public entity_list_start_col As Integer
    Public entity_list_end_delimiter As String
    Public items As New ArrayList
    Public Class bc_om_excel_data_item
        Public entity_identifier_start_col As Integer
        Public item_identifier_type As String
        REM use_identifer = use identifier below, use_excek means read item_identifier from excel
        Public item_identifier As String
        REM ned to add params as well
        Public submission_code As Integer
        Public contributor_id As Integer
        Public mandatory As Boolean
        REM 0 number, 1 date, 2 string, boolean
        Public datatype As Integer
    End Class
End Class

Friend Class bc_om_excel_io_db
    Private gbc_db As New bc_cs_db_services
    Public Function write_data(ByVal xml_data As String, ByRef certificate As bc_cs_security.certificate) As Object
        Try
            MsgBox(xml_data)
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_excel_io_db", "write_data", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Function
End Class

