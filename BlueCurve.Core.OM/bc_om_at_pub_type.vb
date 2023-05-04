Imports BlueCurve.Core.CS
Imports System.IO
Imports System.Xml
Imports BlueCurve.Core.OM.Nbc_om_pub_type_structures


#Region "Changes"
REM -------------------------------------------------------------------------------------------------------------------
REM Changes:
REM Tracker                 Initials                   Date                      Synopsis
REM FIL JIRA 7301           PR                         2/01/2014                 added user_id option to custom workflow event   
#End Region
#Region "changes"
'Changes:
'Tracker                 Initials                   Date                      Synopsis
'FIL 6835                PR                         8/1/2014                  Added default support doc
'FIL 7301                PR                         8/1/2014                  Added user_id for workflow actions
#End Region
<Serializable()> Public Class bc_om_pub_type_entity_classify_class
    Inherits bc_cs_soap_base_class
    Public pt_id As Long
    Public class_id As Long
    Public class_name As String
    Public mandatory As Boolean
    Public max_number As Integer
    Public include_roles As New List(Of Integer)
    Public dependent_class_list As New List(Of Long)
    Public show_implicit_taxonomy As Boolean
    Public set_default_Value As Boolean
    Public Sub db_read()
        Try
            Dim gdb As New bc_cs_db_services
            Dim sql As String
            Dim ares As Object

            sql = "exec dbo.bc_core_get_include_roles_for_ptec " + CStr(pt_id) + "," + CStr(class_id)
            ares = gdb.executesql(sql, certificate)
            If IsArray(ares) Then
                For j = 0 To UBound(ares, 2)
                    include_roles.Add(ares(0, j))
                Next
            End If
            sql = "exec dbo.bc_core_get_dep_class_list_for_ptec " + CStr(pt_id) + "," + CStr(class_id)
            ares = gdb.executesql(sql, certificate)
            If IsArray(ares) Then
                For j = 0 To UBound(ares, 2)
                    dependent_class_list.Add(ares(0, j))
                Next
            End If



        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_pub_type_entity_classify_class", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_in_stage_action
    Public stage_id As Long
    Public actions As New List(Of bc_om_workflow_action)
End Class
<Serializable()> Public Class bc_om_pub_type_distribution
    Inherits bc_cs_soap_base_class
    Public pub_type_id As Long
    Public channels As New List(Of bc_om_pub_type_distribution_channel)
    <Serializable()> Public Class bc_om_pub_type_distribution_channel
        Public id As Long
        Public name As String
        Public automatic As Boolean
        Public list As Boolean
        Public net_send As Boolean
    End Class

    Public Overrides Sub process_object()
        Select Case Me.tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()

        Dim otrace As New bc_cs_activity_log("bc_om_pub_type_distribution", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim vres As Object
        Dim db As New db_bc_om_pub_type_distribution
        Dim oc As bc_om_pub_type_distribution_channel
        vres = db.get_target_distribution_channels_for_pub_type(Me.pub_type_id, MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                oc = New bc_om_pub_type_distribution_channel
                oc.id = vres(0, i)
                oc.name = vres(1, i)
                oc.automatic = vres(2, i)
                oc.list = vres(3, i)
                oc.net_send = False
                channels.Add(oc)
            Next
        End If

        Try
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_pub_type_distribution", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_pub_type_distribution", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        End Try
    End Sub
    Private Class db_bc_om_pub_type_distribution
        Dim gdb As New bc_cs_db_services

        Function get_target_distribution_channels_for_pub_type(id As Long, ByRef certificate As bc_cs_security.certificate) As Object
            Dim sql As String
            sql = "select d.distribution_type_id,d.distribution_type_name,p.auto_send_flag, d.list from distribution_type d" + _
                 " inner  join pub_type_distribution_table p on p.distribution_type_id=d.distribution_type_id" + _
                   " and p.pub_type_id=" + CStr(id)
            get_target_distribution_channels_for_pub_type = gdb.executesql(sql, certificate)
        End Function

    End Class

End Class

<Serializable()> Public Class bc_om_extended_classification
    Inherits bc_cs_soap_base_class
    Public Lists As New List(Of bc_om_extended_classification_list)
    Public entity_id As Long
    Public pub_type_id As Long
    REM 5.3 refic
    Public doc_id As String
    Public Overrides Sub process_object()
        Select Case Me.tmode
            Case tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_extended_classification", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim gdb As New db_bc_om_extended_classification
            Dim res, nes As Object
            Dim list As New bc_om_extended_classification_list
            Dim item As New bc_om_extended_classification_list_item

            res = gdb.get_lists(certificate)
            If IsArray(res) Then
                For i = 0 To UBound(res, 2)

                    list = New bc_om_extended_classification_list
                    list.id = res(0, i)
                    list.name = res(1, i)
                    nes = gdb.get_list_items(list.id, entity_id, pub_type_id, doc_id, MyBase.certificate)
                    If IsArray(nes) Then
                        For j = 0 To UBound(nes, 2)
                            item = New bc_om_extended_classification_list_item
                            item.key = nes(0, j)
                            item.value = nes(1, j)
                            item.selected = nes(2, j)
                            list.items.Add(item)
                        Next
                    End If
                    Me.Lists.Add(list)
                Next
            End If
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_extended_classification", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_extended_classification", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        End Try


    End Sub
    Class db_bc_om_extended_classification
        Dim gdb As New bc_cs_db_services

        Public Function get_lists(ByRef certificate As bc_cs_security.certificate)

            Dim sql As String
            sql = "exec dbo.bc_core_get_ec_lists"
            get_lists = gdb.executesql(sql, certificate)

        End Function
        Public Function get_list_items(ByVal id As Integer, ByVal entity_id As Long, ByVal pub_type_id As Long, ByVal doc_id As String, ByRef certificate As bc_cs_security.certificate)
            Dim sql As String
            If bc_cs_central_settings.server_flag = 0 Then
                sql = "exec dbo.bc_core_get_list_items " + CStr(id) + "," + CStr(entity_id) + "," + CStr(pub_type_id) + "," + CStr(bc_cs_central_settings.logged_on_user_id) + ",'" + CStr(doc_id) + "'"
            Else
                sql = "exec dbo.bc_core_get_list_items " + CStr(id) + "," + CStr(entity_id) + "," + CStr(pub_type_id) + ",'" + CStr(certificate.user_id) + "','" + CStr(doc_id) + "'"
            End If
            get_list_items = gdb.executesql(sql, certificate)

        End Function
    End Class

End Class
<Serializable()> Public Class bc_om_extended_classification_list
    Public id As Integer
    Public name As String
    Public items As New List(Of bc_om_extended_classification_list_item)

End Class
<Serializable()> Public Class bc_om_extended_classification_list_item
    Public key As Long
    Public value As String
    Public selected As Boolean
End Class
REM====
Public Class bc_om_files_to_commit
    Public Const CONTROL_FILE = 1
    Public Const FILE_COPY = 2
    Public Const DELETE = 3
    Public Const IMAGE = 4
    Public mode As Integer
    Public contents As String
    Public target_file As String
    Public source_file As String
    Public target_dir As String
    Public bimage As Byte()

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_clone_document_list
    Inherits bc_cs_soap_base_class

    Private pt_id As Long
    Private entity_id As Long
    Private sub_entity_id As Long
    Public documents As New bc_om_documents
    Public Sub New(ByVal pt_id As Long, ByVal entity_id As Long, ByVal sub_entity_id As Long)
        Me.pt_id = pt_id
        Me.entity_id = entity_id
        Me.sub_entity_id = sub_entity_id
    End Sub

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_clone_document_list", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Select Case Me.tmode
                Case tREAD
                    db_read()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_clone_document_list", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_clone_document_list", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Sub


    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_clone_document_list", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim gdb As New bc_om_clone_doc_db
        Try
            Dim vres As Object
            Dim odoc As bc_om_document
            Me.documents.document.Clear()
            vres = gdb.get_document_list(pt_id, entity_id, sub_entity_id, certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    odoc = New bc_om_document
                    odoc.id = vres(0, i)
                    REM use this for support doc
                    odoc.pub_type_id = vres(1, i)
                    odoc.title = vres(2, i)
                    odoc.doc_date = vres(3, i)
                    Me.documents.document.Add(odoc)
                Next
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_clone_document_list", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)

        Finally
            otrace = New bc_cs_activity_log("bc_om_clone_document_list", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)


        End Try
    End Sub
End Class
REM ===============================================
REM Bluecurve Limited 2005
REM Module:       Author Tool Pub Type Object Model
REM Type:         Object Model
REM Description:  Template
REM Components, 
REM sub Components
REM Version:      1
REM Change history
REM ===============================================
<Serializable()> Public Class bc_om_process_swicthes
    Inherits bc_cs_soap_base_class
    Public import_master As Boolean = True
    Public register_document As Boolean = True
    Public reattach_master As Boolean = True
    Public distribute As Boolean = False
    Public regular_reports As Boolean = False
    Public attestation_screen As Boolean = False
    Public attributes_screen As Boolean = False
    Public stop_document As Boolean = False
    Public fast_track As Boolean = False
    Public attribute_change As Boolean = False
    Public html_summary As Boolean = False
    Public disclosure_file As Boolean = False
    Public auto_assign_disclosures As Boolean = False
    Public set_def_master_support_title As Boolean = True
    Public surname_first As Boolean = False
    Public regular_report_admin_only As Boolean = True
    Public regular_report_default_global As Boolean = False
    Public sizeable_cat_screen As Boolean = True
    Public show_implicit_tagging As Boolean = False
    Public show_publish_default As Boolean = False
    Public minimize_on_view As Boolean = True
    Public import_if_open As Boolean = False
    Public assign_author_on_import As Boolean = True
    Public show_alternate_summary_info As Boolean = False
    Public show_audit_on_summary As Boolean = False
    Public show_audit_before_buttons As Boolean = False
    Public attribute_column_name As String = ""
    Public Sub db_read()
        Dim gdb As New bc_cs_db_services
        Dim sql As String
        sql = "exec dbo.bc_core_get_process_switches"
        Dim vres As Object
        vres = gdb.executesql(sql, certificate)


        If IsArray(vres) Then

            If UBound(vres, 2) = 0 Then

                import_master = vres(0, 0)
                register_document = vres(1, 0)
                reattach_master = vres(2, 0)
                distribute = vres(3, 0)
                regular_reports = vres(4, 0)
                attestation_screen = vres(5, 0)
                attributes_screen = vres(6, 0)
                stop_document = vres(7, 0)
                fast_track = vres(8, 0)
                attribute_change = vres(9, 0)
                html_summary = vres(10, 0)
                disclosure_file = vres(11, 0)
                auto_assign_disclosures = vres(12, 0)
                Try
                    set_def_master_support_title = vres(13, 0)
                    surname_first = vres(14, 0)
                Catch

                End Try
                Try
                    regular_report_admin_only = vres(15, 0)
                    regular_report_default_global = vres(16, 0)
                    sizeable_cat_screen = vres(17, 0)
                    show_implicit_tagging = vres(18, 0)
                    show_publish_default = vres(19, 0)
                    minimize_on_view = vres(20, 0)
                    import_if_open = vres(21, 0)
                    assign_author_on_import = vres(22, 0)

                Catch

                End Try
                Try
                    show_alternate_summary_info = vres(23, 0)
                    show_audit_on_summary = vres(24, 0)
                    show_audit_before_buttons = vres(25, 0)
                    attribute_column_name = vres(26, 0)
                Catch ex As Exception

                End Try


            End If
        End If

    End Sub
End Class
<Serializable()> Public Class bc_om_pub_types
    Inherits bc_cs_soap_base_class
    Public pubtype As New ArrayList
    Public contributors As New ArrayList
    Public languages As New ArrayList
    Public actions As New bc_om_workflow_actions
    Public regular_reports As New bc_om_regular_reports
    Public get_inactive As Boolean = False
    Public stages As New ArrayList
    Public process_switches As New bc_om_process_swicthes
    Public pt_only As Boolean = False
    REM create central settings
    'Public show_urgent_flag As Boolean = False
    'Public show_doc_scan As Boolean = False
    'Public show_support_docs As Boolean = False
    'Public show_linked_docs As Boolean = False
    'Public linked_docs_count As Boolean = False

    REM teaser text option
    Public show_teaser_text As Boolean
    Public bus_areas As New List(Of bc_om_bus_area)
    Public Sub New()
        Me.show_teaser_text = False
    End Sub

    Public Sub Remove(ByVal index As Integer)
        pubtype.RemoveAt(index)
    End Sub

    REM reads entire template suite into memory overwrites what is 
    REM already there
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_pub_types", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om__pub_types", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om__pub_types", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_pub_types", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim i As Integer
            Dim vpubtypes As Object
            Dim vcontributors As Object
            Dim vlanguages As Object
            Dim opubtype As bc_om_pub_type
            Dim ocontributors As bc_om_contributor
            Dim olanguages As bc_om_language
            Dim db_pub_type As New bc_om_at_pub_type_db
            pubtype.Clear()
            contributors.Clear()
            languages.Clear()
            stages.Clear()
            vpubtypes = db_pub_type.read_all_pub_types(MyBase.certificate, get_inactive)

            If pt_only = True Then
                If IsArray(vpubtypes) Then
                    For i = 0 To UBound(vpubtypes, 2)
                        opubtype = New bc_om_pub_type(vpubtypes(0, i), vpubtypes(1, i), vpubtypes(2, i), vpubtypes(3, i), vpubtypes(4, i), vpubtypes(5, i), vpubtypes(6, i), vpubtypes(7, i), MyBase.certificate)
                        pubtype.Add(opubtype)
                    Next
                End If
                Exit Sub
            End If

            process_switches.certificate = certificate
            process_switches.db_read()



            If IsArray(vpubtypes) Then
                For i = 0 To UBound(vpubtypes, 2)
                    opubtype = New bc_om_pub_type(vpubtypes(0, i), vpubtypes(1, i), vpubtypes(2, i), vpubtypes(3, i), vpubtypes(4, i), vpubtypes(5, i), vpubtypes(6, i), vpubtypes(7, i), MyBase.certificate)
                    Try
                        opubtype.parent_category2 = vpubtypes(8, i)
                    Catch
                        Dim ocommentary As New bc_cs_activity_log("bc_om_pub_types", "db_read", bc_cs_activity_codes.COMMENTARY, "Super 2 not set up in database", MyBase.certificate)
                    End Try
                    opubtype.comment = vpubtypes(9, i)
                    opubtype.user_id = vpubtypes(10, i)
                    opubtype.inactive = vpubtypes(11, i)
                    opubtype.sub_entity_class = vpubtypes(12, i)
                    opubtype.is_clonable = vpubtypes(13, i)
                    opubtype.support_doc_only = vpubtypes(14, i)
                    opubtype.default_support_pub_type = vpubtypes(15, i)
                    opubtype.mandatory_default_support_doc = vpubtypes(16, i)
                    opubtype.folder_name = vpubtypes(17, i)

                    pubtype.Add(opubtype)
                    pubtype(i).db_read(vpubtypes(0, i), MyBase.certificate)
                Next
            End If

            vcontributors = db_pub_type.read_all_contributors(MyBase.certificate)
            If IsArray(vcontributors) Then
                For i = 0 To UBound(vcontributors, 2)
                    ocontributors = New bc_om_contributor(vcontributors(0, i), vcontributors(1, i))
                    contributors.Add(ocontributors)
                Next
            End If

            vlanguages = db_pub_type.read_all_languages(MyBase.certificate)
            If IsArray(vlanguages) Then
                For i = 0 To UBound(vlanguages, 2)
                    olanguages = New bc_om_language(vlanguages(0, i), vlanguages(1, i), vlanguages(2, i))
                    languages.Add(olanguages)
                Next
            End If
            actions.certificate = MyBase.certificate
            actions.db_read()
            REM options
            REM teaser text option
            Dim vteaser As Object
            vteaser = db_pub_type.test_for_teaser_text(MyBase.certificate)
            If IsArray(vteaser) Then
                If vteaser(0, 0) = "Error" Then
                    Dim ocommentary As New bc_cs_activity_log("bc_om_pub_types", "db_read", bc_cs_activity_codes.COMMENTARY, "Teaser text not set up in database", MyBase.certificate)
                Else
                    Me.show_teaser_text = True
                End If
            End If
            'regular_reports.certificate = MyBase.certificate
            'regular_reports.db_read()

            Dim vstages As Object
            vstages = db_pub_type.read_all_stages(MyBase.certificate)
            If IsArray(vstages) Then
                For i = 0 To UBound(vstages, 2)
                    'ostages = New bc_om_stage(vstages(0, i), vstages(1, i))
                    'stages.Add(ostages)
                    stages.Add(vstages(1, i))
                Next
            End If

            'Dim vsettings As Object
            'vsettings = db_pub_type.read_pub_type_settings(MyBase.certificate)
            'If IsArray(vsettings) Then
            '    If UBound(vsettings, 1) = 2 Then
            '        Me.show_urgent_flag = vsettings(0, 0)
            '        Me.show_doc_scan = vsettings(1, 0)
            '    End If
            'End If

            Dim vbusareas As Object
            vbusareas = db_pub_type.read_bus_Areas(MyBase.certificate)
            Dim oba As bc_om_bus_area

            If IsArray(vbusareas) Then
                For i = 0 To UBound(vbusareas, 2)
                    oba = New bc_om_bus_area
                    oba.id = vbusareas(0, i)
                    oba.description = vbusareas(1, i)
                    bus_areas.Add(oba)
                Next
            End If



        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_pub_types", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_pub_types", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
    'Public Overloads Overrides Function call_web_service() As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_pub_types", "call_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    If IsNumeric(bc_cs_central_settings.timeout) Then
    '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
    '    End If
    '    call_web_service = webservice.LoadPubTypes()
    '    otrace = New bc_cs_activity_log("bc_om_pub_types", "call_web_service", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    'End Function
    'Public Overrides Function write_xml_via_soap_client_request() As Object

    'End Function

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
<Serializable()> Public Class bc_om_cat_lists
    Inherits bc_cs_soap_base_class
    Public id As Long
    Public name As String
    Public single_select As Integer
    Public items As New ArrayList
    Public pt_id As Long
    Private vres As Object
    Private olist As bc_om_cat_list_item
    Public Sub db_read()
        Dim gdb As New bc_om_at_pub_type_db
        Dim i As Integer
        items.Clear()
        vres = gdb.get_list_items_for_pub_type(id, pt_id, MyBase.certificate)
        If IsArray(vres) Then
            For i = 0 To UBound(vres, 2)
                olist = New bc_om_cat_list_item
                olist.item_id = vres(1, i)
                olist.name = vres(2, i)
                items.Add(olist)
            Next
        End If

    End Sub
    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_cat_list_item
    Public item_id As String
    Public name As String

    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_distribution_channel
    Public channel_id As Integer
    Public name As String
    Public type As Integer
End Class
REM EFG June 2012
<Serializable()> Public Class bc_om_pub_type
    Inherits bc_cs_soap_base_class
    REM prod tools

    Public structures As New bc_om_pub_type_structures
    Public table_wizard As New Nbc_om_table_wizard.bc_om_table_wizard
    REM FIL JIRA 6835
    Public default_support_pub_type As Long
    REM END JIRA
    Public id As Long
    Public name As String
    Public description As String
    Public parent_category As Long
    Public parent_category2 As Long
    Public child_category As Long
    Public sub_entity_class As Long
    Public language As Integer
    Public stage As Integer
    Public parameters As New bc_om_parameters
    'Public taxonomy As New arraylist
    Public products As New bc_om_products
    'document workflow from initial stage
    Public workflow As New bc_om_workflow
    'financial workflow
    Public financial_workflow As New bc_om_financial_workflow
    'default financial workflow stage
    Public default_financial_workflow_stage
    'composite flag
    Public composite As Boolean = False
    'bus area
    Public bus_area_id As Long
    'composite composition
    Public composite_composition As New bc_om_composite
    'additional taxonomy
    'Public taxonomy As New ArrayList
    Public taxonomy As New List(Of bc_om_pub_type_entity_classify_class)
    Public folder_name As String
    'additonal selection lists
    Public lists As New ArrayList
    REM holds array of linked translated pub types
    Public translated_pub_types As New ArrayList
    REM show in wizard flag
    Public show_in_wizard As Boolean
    REM insert components for pub type
    Public insertable_components As New ArrayList
    REM flags if pub type should only be shown in create
    Public create_only As Boolean = False
    REM audit 
    Public comment As String
    Public user_id As Long
    Public write_mode As Integer = 0
    REM inactive
    Public inactive As Boolean = False
    Public delete_error As String
    REM EFG June 2012
    Public is_clonable As Boolean = False

    Public support_doc_only As Boolean

    Public show_urgent_flag As Boolean = False
    Public show_doc_scan As Boolean = False
    Public show_support_docs As Boolean = False
    Public show_linked_docs As Boolean = False
    Public show_analytic_docs As Boolean = False
    Public num_analytic_docs As Integer = 0
    Public linked_docs_count As Integer = 3
    Public support_doc_title As String = ""
    Public support_doc_filter As String = ""
    Public support_doc_mandatory As Boolean = False
    Public support_doc_maximum As Integer = 0
    Public prefix_text As String = ""
    Public show_comments As Boolean = False

    Public Const INSERT = 0
    Public Const UPDATE_PT = 1
    Public Const DELETE = 2
    Public Const SET_ACTIVE = 3
    Public Const SET_INACTIVE = 4
    Public Const INSERT_AND_SET_DEFAULT_ATTRIBUTE = 5
    REM FIL 5.3
    Public mandatory_default_support_doc As Boolean
    Public build_components As New List(Of Long)
    Public in_stage_actions As New List(Of bc_om_in_stage_action)

    Public distributable As Boolean
    REM WF DEC 2016
    Public max_title_length As Integer = 0
    Public max_sub_title_length As Integer = 0
    Public custom_field_name As String
    Public custom_field_length As Integer = 0
    Public custom_field_default_value As String = ""
    Public html_editor_length As Integer = 0
    Public html_editor_default_font As String = ""
    Public html_editor_default_font_size As Double = 8
    Public html_editor_only_bold_italic_underline As Boolean
    Public master_doc_filter As String = ""
    Public hide_submission_date As Boolean = False
    Public hide_next_stage_date As Boolean = False
    Public mandatory_linked_doc As Boolean = False

    Public disable_title_st_summary As Boolean = False
    Public custom_field_name2 As String
    Public custom_field_name2_length As Integer
    Public linked_documents_name As String
    Public tab_sel_idx As Integer
    Public cat_width_chars_width As Integer
    Public dont_show_pages As Boolean = False

    Public distribution_channels As New List(Of bc_om_distribution_channel)


    REM =============

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_pub_type", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Select Case Me.tmode
                Case tWRITE
                    db_write()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_pub_type", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_pub_type", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Sub
    Public Sub db_write()
        Dim otrace As New bc_cs_activity_log("bc_om_pub_type", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim gdb As New bc_om_at_pub_type_db
            Dim fstr As bc_cs_string_services
            Dim fname As String
            Dim fdesc As String
            fname = Me.name
            fdesc = Me.description

            fstr = New bc_cs_string_services(Me.name)
            Me.name = fstr.delimit_apostrophies
            fstr = New bc_cs_string_services(Me.description)
            Me.description = fstr.delimit_apostrophies
            If bc_cs_central_settings.server_flag = 0 Then
                MyBase.certificate.user_id = bc_cs_central_settings.logged_on_user_id
            End If
            Select Case Me.write_mode
                REM JIRA 6825
                Case INSERT
                    Me.id = gdb.insert_pub_type(Me.name, Me.description, Me.language, Me.child_category, Me.bus_area_id, Me.support_doc_only, Me.default_support_pub_type, Me.mandatory_default_support_doc, Me.folder_name, MyBase.certificate)
                    Me.comment = Format("hh:MM:ss", Now)
                Case INSERT_AND_SET_DEFAULT_ATTRIBUTE
                    Me.id = gdb.insert_pub_type(Me.name, Me.description, Me.language, Me.child_category, Me.bus_area_id, Me.support_doc_only, Me.default_support_pub_type, Me.mandatory_default_support_doc, Me.folder_name, MyBase.certificate)
                    Me.comment = Format("hh:MM:ss", Now)

                    gdb.set_default_attribute_values(Me.id, MyBase.certificate)
                Case UPDATE_PT
                    gdb.update_pub_type(Me.id, Me.name, Me.description, Me.language, Me.child_category, Me.bus_area_id, Me.support_doc_only, Me.default_support_pub_type, Me.mandatory_default_support_doc, Me.folder_name, MyBase.certificate)
                    Me.comment = Format("hh:MM:ss", Now)
                    REM JIRA 6825
                Case DELETE
                    Me.delete_error = gdb.delete_pub_type(Me.id, MyBase.certificate)
                Case SET_ACTIVE
                    gdb.set_inactive(Me.id, False, MyBase.certificate)
                    Me.comment = "set active " + Format("hh:MM:ss", Now)
                Case SET_INACTIVE
                    gdb.set_inactive(Me.id, True, MyBase.certificate)
                    Me.comment = "set inactive " + Format("hh:MM:ss", Now)

            End Select
            Me.name = fname
            Me.description = fdesc

            Me.user_id = certificate.user_id
            Dim ugdb As New bc_om_user_db
            'ugdb.set_sync(MyBase.certificate)

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_pub_type", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_pub_type", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try

    End Sub
    Public Overrides Function ToString() As String
        Return name
    End Function

    Public Sub New(ByVal iid As Integer, ByVal strname As String, ByVal strdescription As String, ByVal lchild_category As Long, ByVal lparent_category As Long, ByVal ilanguage As Integer, ByVal default_financial_workflow_stage As Long, ByVal bus_area_id As Long, ByVal certificate As bc_cs_security.certificate)
        Me.certificate = certificate
        id = iid
        name = strname
        description = strdescription
        parent_category = lparent_category
        child_category = lchild_category
        language = ilanguage
        Me.default_financial_workflow_stage = default_financial_workflow_stage
        composite_composition.pub_type_id = Me.id
        Me.bus_area_id = bus_area_id
        Dim db_pubtype As New bc_om_at_pub_type_db
        show_in_wizard = db_pubtype.get_show_in_wizard_flag(Me.id, MyBase.certificate)
    End Sub
    Public Sub New()

    End Sub

    Public Sub update(ByVal iid As Integer, ByVal strname As String, ByVal strdescription As String, ByVal lparent_category As Long, ByVal lchild_category As Long, ByVal ilanguage As Integer)
        id = iid
        name = strname
        description = strdescription
        parent_category = lparent_category
        child_category = lchild_category
        language = ilanguage

    End Sub

    Public Sub db_read(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Try
            Dim gdb As New bc_om_at_pub_type_db
            Dim otaxonomy As Object
            Dim olists As Object
            Dim otpubtypes As Object
            Dim i As Integer
            REM now read products
            products.db_read(id, certificate)
            REM now read parameters for pub type
            parameters.db_read(id, certificate)
            REM read workflow from initial stage
            workflow.db_read(id, 0, True, id, 0, certificate)
            REM read in financial workflow
            financial_workflow.db_read(id, certificate)
            REM see if composite
            Me.composite = gdb.is_composite(Me.id, certificate)
            REM taxonomy

            Dim res As Object
            res = gdb.get_pub_type_entity_classify(Me.id, certificate)
            Dim pt_ic As bc_om_pub_type_entity_classify_class

            If IsArray(res) Then
                For i = 0 To UBound(res, 2)
                    pt_ic = New bc_om_pub_type_entity_classify_class
                    pt_ic.pt_id = Me.id
                    pt_ic.class_id = res(0, i)

                    pt_ic.class_name = res(1, i)
                    pt_ic.mandatory = res(2, i)
                    pt_ic.max_number = res(3, i)
                    pt_ic.show_implicit_taxonomy = res(4, i)
                    pt_ic.set_default_Value = res(5, i)

                    pt_ic.db_read()
                    taxonomy.Add(pt_ic)
                Next
            End If
            'otaxonomy = gdb.read_taxonomy_for_pub_type(Me.id, certificate)
            'If IsArray(otaxonomy) Then
            '    Me.taxonomy.Clear()
            '    For i = 0 To UBound(otaxonomy, 2)
            '        Me.taxonomy.Add(otaxonomy(0, i))
            '    Next
            'End If
            REM lists 
            olists = gdb.get_lists_for_pub_type(Me.id, MyBase.certificate)
            Dim llist As bc_om_cat_lists
            If IsArray(olists) Then
                If UBound(olists, 2) > -1 Then
                    If CStr(olists(0, 0)) = "Error" Then
                        Dim ocomm As New bc_cs_activity_log("bc_om_pub_type", "db_read", bc_cs_activity_codes.COMMENTARY, "Warning: Lists datastructures not installed", certificate)
                    Else
                        Me.lists.Clear()
                        For i = 0 To UBound(olists, 2)
                            llist = New bc_om_cat_lists
                            llist.id = olists(0, i)
                            llist.name = CStr(olists(1, i))
                            llist.single_select = olists(2, i)
                            llist.pt_id = Me.id
                            llist.db_read()
                            Me.lists.Add(llist)
                        Next
                    End If
                End If
            End If
            REM translated pub types
            otpubtypes = gdb.get_translated_pubtypes(id, certificate)
            If IsArray(otpubtypes) Then
                Me.translated_pub_types.Clear()
                For i = 0 To UBound(otpubtypes, 2)
                    Me.translated_pub_types.Add(otpubtypes(0, i))
                Next

            End If
            REM insertable components
            Dim oincomps As Object
            oincomps = gdb.get_insertable_components(Me.id, certificate)
            If IsArray(oincomps) Then
                For i = 0 To UBound(oincomps, 2)
                    Me.insertable_components.Add(oincomps(0, i))
                Next
            End If
            REM check if use in create only
            Me.create_only = gdb.use_in_create_only(Me.id, certificate)

            REM FIL 5.3
            Dim bcomps As Object
            bcomps = gdb.read_build_comps(Me.id, MyBase.certificate)
            If IsArray(bcomps) Then
                For i = 0 To UBound(bcomps, 2)
                    Me.build_components.Add(bcomps(0, i))
                Next
            End If

            REM SEPT 2015
            Dim vres As Object
            Dim pstage_id As Integer = 0
            Dim isa As bc_om_in_stage_action
            Dim action As bc_om_workflow_action


            vres = gdb.read_in_stage_actions(Me.id, certificate)
            Me.in_stage_actions.Clear()

            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    If pstage_id <> vres(0, i) Then
                        If pstage_id <> 0 Then
                            Me.in_stage_actions.Add(isa)
                        End If
                        isa = New bc_om_in_stage_action
                        isa.stage_id = vres(0, i)
                    End If
                    action = New bc_om_workflow_action
                    action.id = vres(1, i)
                    action.name = vres(2, i)
                    action.calling_object = vres(3, i)
                    action.calling_method = vres(4, i)
                    action.mandatory = vres(5, i)
                    action.display_name = vres(6, i)
                    isa.actions.Add(action)
                    pstage_id = vres(0, i)
                Next
                If Not IsNothing(isa) Then
                    Me.in_stage_actions.Add(isa)
                End If
            End If

            REM pub type settings
            Dim vsettings As Object
            vsettings = gdb.read_pub_type_settings(Me.id, MyBase.certificate)
            If IsArray(vsettings) Then
                If UBound(vsettings, 2) > -1 And UBound(vsettings, 1) >= 10 Then
                    Me.show_urgent_flag = vsettings(0, 0)
                    Me.show_doc_scan = vsettings(1, 0)
                    Me.show_support_docs = vsettings(2, 0)
                    Me.show_linked_docs = vsettings(3, 0)
                    Me.linked_docs_count = vsettings(4, 0)
                    Me.support_doc_title = vsettings(5, 0)
                    Me.support_doc_filter = vsettings(6, 0)
                    Me.support_doc_mandatory = vsettings(7, 0)
                    Me.support_doc_maximum = vsettings(8, 0)
                    Me.prefix_text = vsettings(9, 0)
                    If UBound(vsettings, 1) >= 11 Then
                        Me.show_comments = vsettings(10, 0)
                    End If
                    If UBound(vsettings, 1) >= 13 Then

                        Me.max_title_length = vsettings(11, 0)
                        Me.max_sub_title_length = vsettings(12, 0)
                        Me.custom_field_name = vsettings(13, 0)
                        Me.custom_field_length = vsettings(14, 0)
                        Me.custom_field_default_value = vsettings(15, 0)
                        Me.html_editor_length = vsettings(16, 0)
                        Me.html_editor_default_font = vsettings(17, 0)
                        Me.html_editor_default_font_size = vsettings(18, 0)
                        Me.html_editor_only_bold_italic_underline = vsettings(19, 0)
                        Me.master_doc_filter = vsettings(20, 0)
                        Me.hide_submission_date = vsettings(21, 0)
                        Me.hide_next_stage_date = vsettings(22, 0)
                        Me.mandatory_linked_doc = vsettings(23, 0)
                        Me.show_analytic_docs = vsettings(24, 0)
                        Me.num_analytic_docs = vsettings(25, 0)
                        Me.disable_title_st_summary = vsettings(26, 0)
                        Me.custom_field_name2 = vsettings(27, 0)
                        Me.custom_field_name2_length = vsettings(28, 0)
                        Me.linked_documents_name = vsettings(29, 0)
                        Me.tab_sel_idx = vsettings(30, 0)
                        Me.cat_width_chars_width = vsettings(31, 0)
                        Me.dont_show_pages = vsettings(32, 0)
                    End If
                End If
                REM backward compatibity
                If UBound(vsettings, 2) > -1 And UBound(vsettings, 1) = 5 Then
                    Me.show_urgent_flag = vsettings(0, 0)
                    Me.show_doc_scan = vsettings(1, 0)
                    Me.show_support_docs = vsettings(2, 0)
                    Me.show_linked_docs = vsettings(3, 0)
                    Me.linked_docs_count = vsettings(4, 0)
                End If
            End If
            REM structure wizard
            REM turn this on when ready
            Me.structures.pub_type_id = Me.id
            Me.structures.db_read()
            Me.table_wizard.pub_type_id = Me.id
            Me.table_wizard.db_read()
            REM Distribution
            Me.distributable = False
            vres = gdb.is_distributable(Me.id, certificate)
            If IsArray(vres) Then
                If UBound(vres, 2) > -1 Then
                    Me.distributable = vres(0, 0)
                End If
            End If
            vres = gdb.channels_for_pub_type(Me.id, certificate)
            Dim dc As bc_om_distribution_channel
            distribution_channels.Clear()


            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    dc = New bc_om_distribution_channel
                    dc.channel_id = vres(0, i)
                    dc.name = vres(1, i)
                    dc.type = vres(2, i)
                    distribution_channels.Add(dc)
                Next
            End If



        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_pub_type", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        End Try

    End Sub
End Class
<Serializable()> Public Class bc_om_contributor
    Public id As Long
    Public name As String
    Public Sub New(ByVal id, ByVal name)
        Me.id = id
        Me.name = name
    End Sub
End Class
<Serializable()> Public Class bc_om_language
    Public id As Long
    Public language_code As String
    Public name As String
    Public Sub New(ByVal id, ByVal language_code, ByVal name)
        Me.id = id
        Me.language_code = language_code
        Me.name = name
    End Sub
    Public Overrides Function ToString() As String
        Return name
    End Function
End Class
<Serializable()> Public Class bc_om_stage
    Public id As Long
    Public name As String
    Public Sub New(ByVal id, ByVal name)
        Me.id = id
        Me.name = name
    End Sub
    Public Overrides Function ToString() As String
        Return name
    End Function
End Class
<Serializable()> Public Class bc_om_products
    Inherits bc_cs_soap_base_class
    Public product As New ArrayList
    Public Sub New()

    End Sub

    REM reads entire template suite into memory overwrites what is 
    REM already there
    Public Sub db_read(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_om_products", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim i As Integer
            Dim vproducts As Object
            Dim oproduct As bc_om_product
            Dim db_pub_type As New bc_om_at_pub_type_db
            MyBase.certificate = certificate
            vproducts = db_pub_type.read_all_products(id, MyBase.certificate)
            If IsArray(vproducts) Then
                For i = 0 To UBound(vproducts, 2)
                    oproduct = New bc_om_product(vproducts(0, i), vproducts(1, i), vproducts(2, i), vproducts(3, i))
                    Try
                        oproduct.super_value2 = vproducts(4, i)
                    Catch
                        Dim ocommentary As New bc_cs_activity_log("bc_om_products", "db_read", bc_cs_activity_codes.TRACE_EXIT, "Super value 2 not implemented", certificate)
                    End Try
                    product.Add(oproduct)
                    'product(i).db_read(vproducts(0, i), MyBase.certificate)
                Next
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_products", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_products", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub


End Class
<Serializable()> Public Class bc_om_product
    Inherits bc_cs_soap_base_class

    Public Id As Integer
    Public super_value As String
    Public super_value2 As String
    Public name As String
    Public template_id As Integer
    Public Sub New()

    End Sub
    Public Sub New(ByVal iid As Integer, ByVal strsuper_value As String, ByVal strproduct_name As String, ByVal itemplate_id As Integer)
        Id = iid
        super_value = strsuper_value
        name = strproduct_name
        template_id = itemplate_id
    End Sub
    Public Sub update(ByVal iid As Integer, ByVal strsuper_value As String, ByVal strproduct_name As String, ByVal itemplate_id As Integer)
        Id = iid
        super_value = strsuper_value
        name = strproduct_name
        template_id = itemplate_id
    End Sub
    Public Sub db_read()

    End Sub

End Class
<Serializable()> Public Class bc_om_parameters
    Inherits bc_cs_soap_base_class

    Public parameters As New ArrayList
    Public Sub db_read(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)

        MyBase.certificate = certificate
        Dim vparams As Object
        Dim oparameter As bc_om_parameter
        Dim db_pub_type As New bc_om_at_pub_type_db
        Dim i As Integer
        vparams = db_pub_type.read_all_parameters(id, MyBase.certificate)
        If IsArray(vparams) Then
            For i = 0 To UBound(vparams, 2)
                oparameter = New bc_om_parameter
                oparameter.caption = vparams(0, i)
                oparameter.prop = vparams(1, i)
                oparameter.type = vparams(2, i)
                parameters.Add(oparameter)
            Next
            oparameter = New bc_om_parameter
        End If
    End Sub
End Class
REM only text and label paramters are supported 8 and 10
<Serializable()> Public Class bc_om_parameter
    Inherits bc_cs_soap_base_class

    Public caption As String
    Public type As String
    Public prop As String
    Public value As String
    Public Sub New()

    End Sub
End Class
<Serializable()> Public Class bc_om_financial_workflow
    Inherits bc_cs_soap_base_class

    Public stages As New ArrayList
    Public Sub New()

    End Sub
    Public Sub db_read(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim db As New bc_om_at_pub_type_db
        Dim vpubtypes As Object
        Dim oworkflowstage As bc_om_workflow_stage
        MyBase.certificate = certificate
        Dim i As Integer
        vpubtypes = db.read_financial_workflow_stages(CStr(id), MyBase.certificate)
        For i = 0 To UBound(vpubtypes, 2)
            If i = 0 Then
                oworkflowstage = New bc_om_workflow_stage(vpubtypes(0, i), vpubtypes(1, i), True, 0, 0, -1, MyBase.certificate)
            Else
                oworkflowstage = New bc_om_workflow_stage(vpubtypes(0, i), vpubtypes(1, i), False, 0, 0, -1, MyBase.certificate)
            End If
            stages.Add(oworkflowstage)
        Next
    End Sub
End Class
<Serializable()> Public Class bc_om_workflow
    Inherits bc_cs_soap_base_class

    Public stages As New List(Of bc_om_workflow_stage)
    Public Sub New()

    End Sub
    REM SIM MAY 2013
    Public Shadows Sub db_read(ByVal id As Long, ByVal current_stage As Long, ByVal initial_flag As Boolean, ByVal pub_type_id As Long, ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim otrace As New bc_cs_activity_log("bc_om_workflow", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim db As New bc_om_at_pub_type_db
            MyBase.certificate = certificate
            Dim vstage As Object
            Dim vactions As Object
            Dim oworkflowstage As bc_om_workflow_stage
            Dim i, j As Integer
            If initial_flag = True Then
                REM get first stage and then routes via pub_type_id
                vstage = db.get_initial_stage(id, MyBase.certificate)
                If IsArray(vstage) Then
                    If UBound(vstage, 2) = 0 Then
                        oworkflowstage = New bc_om_workflow_stage(vstage(0, 0), vstage(1, 0), 1, vstage(2, 0), 0, -1, MyBase.certificate)
                        oworkflowstage.stage_type = "N"
                        Try
                            oworkflowstage.stage_type = CStr(vstage(2, 0))
                        Catch
                            oworkflowstage.stage_type = "N"
                        End Try
                        If IsNothing(oworkflowstage.stage_type) Then
                            oworkflowstage.stage_type = "N"
                        End If

                        stages.Add(oworkflowstage)
                        current_stage = vstage(0, 0)
                    End If
                End If
            End If
            REM get routes routes
            vstage = db.read_workflow_stages(id, initial_flag, MyBase.certificate)
            If IsArray(vstage) Then
                For i = 0 To UBound(vstage, 2)
                    oworkflowstage = New bc_om_workflow_stage(vstage(0, i), vstage(1, i), 0, vstage(2, i), vstage(3, i), vstage(4, i), MyBase.certificate)
                    oworkflowstage.stage_type = "N"
                    Try
                        oworkflowstage.stage_type = CStr(vstage(3, i))
                    Catch
                        oworkflowstage.stage_type = "N"
                    End Try
                    If IsNothing(oworkflowstage.stage_type) Then
                        oworkflowstage.stage_type = "N"
                    End If
                    REM actions for stage
                    oworkflowstage.action_ids.Clear()
                    vactions = db.read_transitional_actions(pub_type_id, current_stage, MyBase.certificate)
                    For j = 0 To UBound(vactions, 2)
                        If vstage(0, i) = vactions(0, j) Then
                            If vactions(7, j) <> 0 Then
                                oworkflowstage.action_ids.Add(vactions(7, j))
                            End If
                        End If
                    Next
                    REM current approvers
                    Dim vapprovers As New Object
                    vapprovers = db.read_approvers(id, vstage(0, i), doc_id)
                    oworkflowstage.approved_by.Clear()
                    If IsArray(vapprovers) Then
                        For j = 0 To UBound(vapprovers, 2)
                            'oworkflowstage.approved_by.Add(vapprovers(j, 0))
                            'PR SB JAN 2018
                            oworkflowstage.approved_by.Add(vapprovers(0, j))
                        Next
                    End If
                    stages.Add(oworkflowstage)
                Next
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_workflow", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_workflow", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_workflow_stages
    Inherits bc_cs_soap_base_class
    Public stages As New ArrayList
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
        End Select
    End Sub
    Public Sub db_read()
        Dim db_pub_type As New bc_om_at_pub_type_db
        Dim vstages As Object
        vstages = db_pub_type.get_stages(certificate)
        If IsArray(vstages) Then
            For i = 0 To UBound(vstages, 2)
                stages.Add(vstages(0, i))
            Next
        End If
    End Sub
End Class


<Serializable()> Public Class bc_om_workflow_stage
    Inherits bc_cs_soap_base_class

    Public stage_id As Integer
    Public stage_name As String
    Public num_approvers As Integer

    Public approved_by As New ArrayList
    Public action_ids As New ArrayList
    Public initial_stage As Boolean
    Public stage_type As String
    REM SIM MAY 2013
    Public author_approval As Integer
    Public approval_role As Long
    REM

    Public Sub New()

    End Sub
    Public Sub New(ByVal stage_id As Long, ByVal stage_name As String, ByVal initial_stage As Long, ByVal num_approvers As Integer, ByVal author_approval As Integer, ByVal approval_role As Long, ByRef certificate As bc_cs_security.certificate)
        MyBase.certificate = certificate
        Me.stage_id = stage_id
        Me.stage_name = stage_name
        Me.initial_stage = initial_stage
        Me.num_approvers = num_approvers
        Me.author_approval = author_approval
        Me.approval_role = approval_role
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class
<Serializable()> Public Class bc_om_workflow_actions
    Inherits bc_cs_soap_base_class

    Public actions As New ArrayList

    Public Sub New()

    End Sub
    Public Sub db_read()
        Dim db As New bc_om_at_pub_type_db
        Dim i As Integer
        Dim vactions As Object
        Dim oaction As bc_om_workflow_action
        vactions = db.read_actions(MyBase.certificate)
        If IsArray(vactions) Then
            For i = 0 To UBound(vactions, 2)
                oaction = New bc_om_workflow_action(vactions(0, i), vactions(1, i), vactions(2, i), vactions(3, i), MyBase.certificate)
                actions.Add(oaction)
            Next
        End If

    End Sub
End Class
<Serializable()> Public Class bc_om_workflow_action
    Inherits bc_cs_soap_base_class

    Public id As Long
    Public name As String
    Public calling_object As String
    Public calling_method As String
    Public mandatory As Boolean
    Public display_name As String
    Public Sub New()

    End Sub
    Public Sub New(ByVal id As Long, ByVal name As String, ByVal calling_object As String, ByVal calling_method As String, ByRef certificate As bc_cs_security.certificate)
        MyBase.certificate = certificate
        Me.id = id
        Me.name = name
        Me.calling_object = calling_object
        Me.calling_method = calling_method
    End Sub

    Protected Overrides Sub Finalize()
        MyBase.Finalize()
    End Sub
End Class

<Serializable()> Public Class bc_om_composite
    Inherits bc_cs_soap_base_class
    REM resultant document list
    Public documents As New bc_om_documents
    REM final merged document
    Public final_document As New bc_om_document
    Public pub_type_id As Long
    Public customParam As String
    Public no_merge As Boolean = False
    Public Sub New()

    End Sub
    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_composite", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            REM this is always specific to object
            If Me.tmode = bc_cs_soap_base_class.tREAD Then
                db_read()
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_composite", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_composite", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Sub

    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_composite", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim gdb As New bc_om_at_pub_type_db
            Dim docs As New ArrayList
            Dim titles As New ArrayList
            Dim bc_om_document As bc_om_document
            Dim i As Integer
            docs = gdb.get_documents_for_composite(Me.pub_type_id, titles, MyBase.certificate, Me.customParam)
            For i = 0 To docs.Count - 1
                REM get meta and document for each document
                REM required in composite
                bc_om_document = New bc_om_document
                bc_om_document.certificate = MyBase.certificate
                Dim id As Long
                id = docs(i)
                bc_om_document.id = docs(i)
                bc_om_document.bcheck_out = False
                bc_om_document.btake_revision = False
                bc_om_document.bwith_document = True
                If no_merge = True And i > 0 Then
                    bc_om_document.bwith_document = False
                End If
                bc_om_document.db_read_for_create()
                bc_om_document.id = id
                bc_om_document.title = titles(i)

                Me.documents.document.Add(bc_om_document)
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_workflow", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_composite", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub
    'Public Overloads Overrides Function call_web_service(ByVal s As String) As String
    '    Dim otrace As New bc_cs_activity_log("bc_om_composite", "call_web_service", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
    '    If IsNumeric(bc_cs_central_settings.timeout) Then
    '        webservice.Timeout = CLng(bc_cs_central_settings.timeout)
    '        Dim ocommentary As New bc_cs_activity_log("bc_om_web services", "call_web_service", bc_cs_activity_codes.COMMENTARY, "Setting Web Service Timeout to: " + bc_cs_central_settings.timeout)
    '    End If

    '    call_web_service = webservice.LoadCompositeComposition(s)
    '    otrace = New bc_cs_activity_log("bc_om_composite", "call_web_service", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    'End Function
    'Public Shadows Function write_xml_to_soap_server_reponse(ByVal id As String) As String
    '    Dim cs As New bc_cs_central_settings(True)
    '    Dim otrace As New bc_cs_activity_log("bc_om_composite", "write_xml_to_soap_server_reponse", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

    '    Try
    '        REM initialse database
    '        Dim db As New bc_cs_db_services(False)
    '        REM product response and return
    '        Dim osec As New bc_cs_security
    '        id = osec.decrypt_xml(id, MyBase.certificate)
    '        Me.pub_type_id = CLng(id)
    '        Me.db_read()
    '        REM write object to string
    '        write_xml_to_soap_server_reponse = osec.encrypt_xml(Me.write_xml_to_string, MyBase.certificate)
    '    Catch ex As Exception
    '        Dim db_err As New bc_cs_error_log("bc_om_composite", "write_xml_to_soap_server_reponse", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
    '    Finally
    '        otrace = New bc_cs_activity_log("bc_om_composite", "write_xml_to_soap_server_reponse", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try

    'End Function
End Class
REM complete workflow for pub type used for workflow summary chart
<Serializable()> Public Class bc_om_pub_type_workflow_stage
    Inherits bc_cs_soap_base_class

    Public current_stage As Long
    Public current_stage_name As String
    Public routes As List(Of bc_om_pub_type_workflow_stage)
    Public next_stage As Boolean
    Public duplicate As Boolean
    Public approvers As Integer
    Public actions As New ArrayList
    REM SIM MAY 2013
    Public author_approval As Integer = 0
    Public approval_role As Long = -1
    REM =========================
    Public Sub New()
        duplicate = False
        next_stage = False
    End Sub
    Public Sub db_write(ByVal pub_type_id As Long, Optional ByRef first As Boolean = False)
        Try
            If IsNothing(Me.routes) Then
                Exit Sub
            End If
            For i = 0 To Me.routes.Count - 1
                Dim db_pubtype As New bc_om_at_pub_type_db
                db_pubtype.add_process_route_for_pub_type(pub_type_id, Me.current_stage_name, Me.routes(i).current_stage_name, Me.routes(i).approvers, first, Me.routes(i).author_approval, Me.routes(i).approval_role, MyBase.certificate)
                For j = 0 To Me.routes(i).actions.Count - 1
                    db_pubtype.add_process_route_actions_for_pub_type(pub_type_id, Me.current_stage_name, Me.routes(i).current_stage_name, Me.routes(i).actions(j), MyBase.certificate)
                Next
                Me.routes(i).certificate = certificate
                Me.routes(i).db_write(pub_type_id)
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_pub_type_workflow_stage", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)


        End Try
    End Sub
End Class
<Serializable()> Public Class bc_om_pub_type_workflow
    Inherits bc_cs_soap_base_class
    Public id As Long
    Public stages_used As New List(Of String)

    Public next_stages As New List(Of String)
    Public current_stage As String
    Public nstage As New bc_om_pub_type_workflow_stage
    Public bwithactions_and_approvers As Boolean = False
    Public Overrides Sub process_object()
        Select Case tmode
            Case bc_cs_soap_base_class.tREAD
                db_read()
            Case bc_cs_soap_base_class.tWRITE
                db_write()
        End Select
    End Sub
    Public Sub db_read()
        load()
    End Sub
    Public Sub db_write()
        REM gdb delete_pub_type_Workflow(id, certificate)
        Dim db_pubtype As New bc_om_at_pub_type_db
        db_pubtype.delete_process_for_pub_type(id, MyBase.certificate)
        Me.nstage.certificate = certificate
        Me.nstage.db_write(id, True)
    End Sub
    Public Sub load()
        Try
            REM get initial stage

            Dim db_pubtype As New bc_om_at_pub_type_db
            Dim vstage As Object
            Dim i As Integer
            Me.stages_used.Clear()
            vstage = db_pubtype.get_initial_stage(Me.id, MyBase.certificate)
            If IsArray(vstage) Then
                nstage.current_stage = vstage(0, 0)
                nstage.current_stage_name = vstage(1, 0)
            End If
            Me.stages_used.Add(nstage.current_stage_name)
            nstage.routes = getroutes(nstage.current_stage_name, nstage.current_stage, MyBase.certificate)
            If current_stage = nstage.current_stage Then
                Me.next_stages.Clear()
                For i = 0 To nstage.routes.Count - 1
                    Me.next_stages.Add(nstage.routes(i).current_stage_name)
                Next
            End If
        Catch

        End Try
    End Sub
    Public Function getroutes(ByVal stage As String, ByVal stage_id As Long, ByRef certificate As bc_cs_security.certificate) As List(Of bc_om_pub_type_workflow_stage)
        Dim otrace As New bc_cs_activity_log("bc_om_pub_type_workflow", "getroutes", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim o As New Object
            Dim oo As New List(Of bc_om_pub_type_workflow_stage)
            Dim l As bc_om_pub_type_workflow_stage
            Dim db_pubtype As New bc_om_at_pub_type_db
            Dim found = False
            Dim i, j As Integer
            oo.Clear()
            o = db_pubtype.read_workflow_stages_for_pt_stage(Me.id, stage_id, certificate)
            For i = 0 To UBound(o, 2)
                l = New bc_om_pub_type_workflow_stage
                REM if stage exists dont proceed else one will end up in a loo
                found = False
                For j = 0 To stages_used.Count - 1
                    If CStr(o(1, i)) = stages_used(j) Then
                        found = True
                    End If
                Next


                stages_used.Add(o(1, i))
                l.current_stage = o(0, i)
                l.current_stage_name = o(1, i)
                l.approvers = o(2, i)
                REM SIM MAY 2013
                l.author_approval = o(3, i)
                l.approval_role = o(4, i)
                REM =====================
                REM process admin change
                REM actions
                l.actions.Clear()
                If Me.bwithactions_and_approvers = True Then
                    Dim vactions As Object
                    vactions = db_pubtype.read_all_transitional_actions(Me.id, stage_id, MyBase.certificate)
                    For j = 0 To UBound(vactions, 2)
                        If l.current_stage = vactions(0, j) Then
                            If vactions(7, j) <> 0 Then
                                l.actions.Add(vactions(7, j))
                            End If
                            l.approvers = vactions(4, j)
                        End If
                    Next
                    REM current approvers
                End If





                REM --------------------------

                REM now use recursion from here
                REM only propogate from current stage
                If found = False Then
                    l.routes = getroutes(CStr(o(1, i)), o(0, i), certificate)
                    l.duplicate = False
                    If l.current_stage = current_stage Then
                        next_stages.Clear()
                        For j = 0 To l.routes.Count - 1
                            next_stages.Add(l.routes(j).current_stage_name)
                        Next
                    End If
                Else
                    l.duplicate = True
                End If
                oo.Add(l)
            Next
            getroutes = oo
        Catch ex As Exception
            'Dim db_err As New bc_cs_error_log("bc_om_pub_type_workflow", "getroutes", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            getroutes = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_om_pub_type_workflow", "getroutes", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function

    Public Sub New()

    End Sub
End Class

REM =========================================================================
REM Database interaction layer
REM =========================================================================
Public Class bc_om_at_pub_type_db
    Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub
    Public Function read_in_stage_actions(ByVal pt_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_get_in_stage_actions_for_pt " + CStr(pt_id)
        read_in_stage_actions = gbc_db.executesql(sql, certificate)
    End Function
    REM reads taxonomy for pub type
    Public Function read_taxonomy_for_pub_type(ByVal pub_type_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As Object
        sql = "select class_name from pub_type_entity_classify p, entity_class_tbl e where pub_type_id=" + CStr(pub_type_id) + " and p.class_id=e.class_id"
        read_taxonomy_for_pub_type = gbc_db.executesql(sql, certificate)
    End Function
    REM reads all pub type in database
    Public Function read_all_pub_types(ByRef certificate As bc_cs_security.certificate, ByVal get_inactive As Boolean) As Object
        Dim sql As String
        Dim ores As Object
        REM FIL JIRA 6835
        If get_inactive = False Then
            sql = "select pub_type_table.pub_type_id, pub_type_name,  pub_desc, pub_type_category, super_value, coalesce(pub_type_language,1), coalesce(financial_workflow_stage_id,1), pub_type_bus_area_id,coalesce(super_value2,0), coalesce(comment,''), coalesce(user_id,0), coalesce(inactive,0), coalesce(secondary_class,0), coalesce(can_clone,0),coalesce(support_doc_only,0),coalesce(default_support_pub_type,0),coalesce(mandatory_default_support_doc,0), coalesce(virtual_folder_name,'') from pub_type_table, at_pub_type_category where pub_type_table.pub_type_id = at_pub_type_category.pub_type_id  and coalesce(deleted,0) = 0 and  coalesce(inactive,0) = 0  order by pub_type_name asc"
        Else
            sql = "select pub_type_table.pub_type_id, pub_type_name,  pub_desc, pub_type_category, super_value, coalesce(pub_type_language,1), coalesce(financial_workflow_stage_id,1), pub_type_bus_area_id,coalesce(super_value2,0), coalesce(comment,''), coalesce(user_id,0), coalesce(inactive,0), coalesce(secondary_class,0),0,coalesce(support_doc_only,0),coalesce(default_support_pub_type,0),coalesce(mandatory_default_support_doc,0), coalesce(virtual_folder_name,'')   from pub_type_table, at_pub_type_category where pub_type_table.pub_type_id = at_pub_type_category.pub_type_id  and coalesce(deleted,0) = 0  order by pub_type_name asc"
        End If

        ores = gbc_db.executesql_show_no_error(sql)
        If IsArray(ores) Then
            If UBound(ores, 2) > -1 Then
                If CStr(ores(0, 0)) <> "Error" Then
                    read_all_pub_types = ores
                    Exit Function
                End If
            End If
        End If
        sql = "select pub_type_table.pub_type_id, pub_type_name,  pub_desc, pub_type_category, super_value, coalesce(pub_type_language,1), coalesce(financial_workflow_stage_id,0), pub_type_bus_area_id,coalesce(comment,''), coalesce(user_id,0), coalesce(inactive,0) from pub_type_table, at_pub_type_category where pub_type_table.pub_type_id = at_pub_type_category.pub_type_id  and coalesce(deleted,0) = 0  order by pub_type_name asc"
        read_all_pub_types = gbc_db.executesql(sql, certificate)
    End Function

    Public Function read_all_products(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        Dim ores As Object
        REM new double entity class version
        sql = "select product_id, super_value,product_name, product_template_template_id,coalesce(super_value2,0) from at_product, at_product_template where pub_type_id=" + CStr(id) + " and product_template_product_id = product_id order by product_name asc"
        ores = gbc_db.executesql_show_no_error(sql)
        If IsArray(ores) Then
            If UBound(ores, 2) > -1 Then
                If CStr(ores(0, 0)) <> "Error" Then
                    read_all_products = ores
                    Exit Function
                End If
            End If
        End If
        sql = "select product_id, super_value,product_name, product_template_template_id from at_product, at_product_template where pub_type_id=" + CStr(id) + " and product_template_product_id = product_id order by product_name asc"
        read_all_products = gbc_db.executesql(sql, certificate)

    End Function
    Public Function read_all_parameters(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select at_parameter_name, at_parameter_doc_property_name, at_parameter_type, product_parameter_order from " + _
              " at_product_parameters, at_parameters where " + _
              " at_parameter_id = product_parameter_id and publication_type_id=" + CStr(id) + " and (at_parameter_type=8 or at_parameter_type=10) order by " + _
              " product_parameter_order Asc"
        read_all_parameters = gbc_db.executesql(sql, certificate)
    End Function
    Public Function test_for_teaser_text(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select coalesce(teaser_text,'') from document_table"
        test_for_teaser_text = gbc_db.executesql_show_no_error(sql)
    End Function
    REM JIRA 6836
    Public Sub set_default_attribute_values(pt_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim change_user_id As Long
        If bc_cs_central_settings.server_flag = 0 Then
            change_user_id = bc_cs_central_settings.logged_on_user_id
        Else
            change_user_id = certificate.user_id
        End If
        sql = "exec dbo.bc_core_cp_set_def_att_values " + CStr(pt_id) + "," + CStr(change_user_id) + ",-6"
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Function insert_pub_type(ByVal name As String, ByVal description As String, ByVal language As Long, ByVal child_category As Long, ByVal bus_area_id As Long, ByVal support_doc_only As Boolean, ByVal default_support_pub_type_id As Long, ByVal mandatory_default_support_doc As Boolean, folder_name As String, ByRef certificate As bc_cs_security.certificate) As Long
        Dim sql As String
        Dim vres As Object
        Dim id As Long

        sql = "insert into pub_type_table(pub_type_id,pub_type_name, pub_type_bus_area_id, financial_workflow_stage_id, comment,user_id,support_doc_only,default_support_pub_type,mandatory_default_support_doc, virtual_folder_name) select coalesce(max(pub_type_id),0) + 1,'" + name + "'," + CStr(bus_area_id) + ",1,convert(varchar(20),getdate())," + CStr(certificate.user_id) + ",'" + CStr(support_doc_only) + "'," + CStr(default_support_pub_type_id) + ",'" + CStr(mandatory_default_support_doc) + "','" + folder_name + "' from pub_type_table"
        gbc_db.executesql(sql, certificate)
        sql = "select max(pub_type_id) from pub_type_table"
        vres = gbc_db.executesql(sql, certificate)
        If IsArray(vres) Then
            id = vres(0, 0)
        End If
        sql = "insert into at_pub_type_category (pub_type_id, pub_type_category, super_value, pub_desc, pub_type_language, super_value2, secondary_class, can_clone)" & _
                "values(" + CStr(id) + "," + CStr(child_category) + ",-1,'" + description + "'," + CStr(language) + ",-1,0,0)"
        gbc_db.executesql(sql, certificate)
        insert_pub_type = id
    End Function
    Public Function insert_pub_type_base(ByVal name As String, ByRef certificate As bc_cs_security.certificate) As Long
        Dim vres As Object
        Dim id As Long
        Dim fs As New bc_cs_string_services(name)
        name = fs.delimit_apostrophies
        vres = gbc_db.executesql("exec dbo.bc_core_cp_add_pub_type '" + name + "'," + CStr(certificate.user_id), certificate)
        If IsArray(vres) Then
            id = vres(0, 0)
        End If

        insert_pub_type_base = id
    End Function

    Public Function update_pub_type(ByVal id As Long, ByVal name As String, ByVal description As String, ByVal language As Long, ByVal child_category As Long, ByVal bus_area_id As Long, ByVal support_doc_only As Boolean, ByVal default_support_pub_type_id As Long, ByVal mandatory_default_support_doc As Boolean, folder_name As String, ByRef certificate As bc_cs_security.certificate) As Long
        Dim sql As String
        sql = "update pub_type_table set virtual_folder_name = '" + folder_name + "', pub_type_name='" + name + "',pub_type_bus_area_id=" + CStr(bus_area_id) + ", comment=convert(varchar(20),getdate()),user_id=" + CStr(certificate.user_id) + ", support_doc_only='" + CStr(support_doc_only) + "', default_support_pub_type=" + CStr(default_support_pub_type_id) + ", mandatory_default_support_doc='" + CStr(mandatory_default_support_doc) + "'  where pub_type_id=" + CStr(id)
        gbc_db.executesql(sql, certificate)
        sql = "update at_pub_type_category set pub_type_category=" + CStr(child_category) + ",pub_desc='" + description + "', pub_type_language=" + CStr(language) + " where pub_type_id=" + CStr(id)
        gbc_db.executesql(sql, certificate)
    End Function
    REM END JIRA

    Public Sub add_process_route_actions_for_pub_type(ByVal pub_type_id As Long, ByVal stage_from As String, ByVal stage_to As String, ByVal action As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "dbo.bc_core_cp_add_process_action " + CStr(pub_type_id) + ",'" + stage_from + "','" + stage_to + "','" + CStr(action) + "'"
        gbc_db.executesql(sql, certificate)

    End Sub

    Public Sub add_process_route_for_pub_type(ByVal pub_type_id As Long, ByVal stage_from As String, ByVal stage_to As String, ByVal num_approvers As Integer, ByVal first As Boolean, ByVal author_approval As Integer, ByVal approval_role As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "dbo.bc_core_cp_add_process_for_pt " + CStr(pub_type_id) + ",'" + stage_from + "','" + stage_to + "'," + CStr(num_approvers) + "," + CStr(first) + "," + CStr(author_approval) + "," + CStr(approval_role)
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Function get_initial_stage(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select s.stage_id, stage_name from stage_tbl s, wf_pubtype_stage_tbl p where s.stage_id=p.stage_id and pubtype_id=" + CStr(id) + " and initial_stage = 1 order by initial_stage desc"
        get_initial_stage = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_workflow_stages(ByVal id As Long, ByVal initial_flag As Boolean, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        If initial_flag = True Then
            REM pub type id
            sql = "exec dbo.wf_get_initial_stage_routes " + CStr(id)
        Else
            REM container id
            sql = "exec dbo.wf_get_routes " + CStr(id)
        End If
        read_workflow_stages = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_workflow_stages_for_pt_stage(ByVal pid As Long, ByVal stage_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.wf_get_routes_pub_type_stage " + CStr(pid) + "," + CStr(stage_id)
        read_workflow_stages_for_pt_stage = gbc_db.executesql_show_no_error(sql)
    End Function
    Public Function read_transitional_actions(ByVal id As Long, ByVal current_stage_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select stage_to_id,stage_to_name, psr_id,ps_to_id,num_approvers, automatic,psra_id,a.action_id,a.action_name, action_required, action_sequence " + _
        " from wf_pubtype_stages_routes_trans_events " + _
        " inner join action_tbl a on a.action_id=wf_pubtype_stages_routes_trans_events.action_id " + _
        " where " + _
        " pubtype_id =" + CStr(id) + _
        " and " + _
        " stage_from_id = " + CStr(current_stage_id) + _
        " and calling_object <> 'custom server side'" + _
        " and calling_object <> 'core server side'" + _
        " and calling_object <> 'core server side distribution'" + _
        " order by stage_to_id, action_sequence"
        read_transitional_actions = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_all_transitional_actions(ByVal id As Long, ByVal current_stage_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select stage_to_id,stage_to_name, psr_id,ps_to_id,num_approvers, automatic,psra_id,a.action_id,a.action_name, action_required, action_sequence " + _
        " from wf_pubtype_stages_routes_trans_events " + _
        " inner join action_tbl a on a.action_id=wf_pubtype_stages_routes_trans_events.action_id " + _
        " where " + _
        " pubtype_id =" + CStr(id) + _
        " and " + _
        " stage_from_id = " + CStr(current_stage_id) + _
        " order by stage_to_id, action_sequence"
        read_all_transitional_actions = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_financial_workflow_stages(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select stage_tbl.stage_id, stage_name from stage_tbl where stage_name = 'Draft' or stage_name='Publish' order by stage_name asc"
        read_financial_workflow_stages = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_actions(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select action_id, action_name, calling_object, calling_method from action_tbl  order by action_name asc"
        read_actions = gbc_db.executesql(sql, certificate)
    End Function
    Public Sub set_inactive(ByVal id As Long, ByVal inactive_flag As Boolean, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        If inactive_flag = False Then
            sql = "update pub_type_table set inactive=0, comment='set active ' +convert(varchar(20), getdate()), user_id=" + CStr(certificate.user_id) + " where pub_type_id = " + CStr(id)
        Else
            sql = "update pub_type_table set inactive=1, comment='set inactive ' + convert(varchar(20), getdate()), user_id=" + CStr(certificate.user_id) + " where pub_type_id = " + CStr(id)
        End If
        gbc_db.executesql(sql, certificate)

    End Sub
    Public Function update_pubtype_name(ByVal id As Long, name As String, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        sql = "update pub_type_table set pub_type_name='" + name + "' where pub_type_id=" + CStr(id)
        gbc_db.executesql(sql, certificate)
    End Function

    Public Function delete_pub_type(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As String
        Dim sql As String
        sql = "exec dbo.bcc_core_cp_generic_delete 'pub_type'," + CStr(id) + "," + CStr(certificate.user_id)
        delete_pub_type = gbc_db.test_sql(sql, certificate)
    End Function
    Public Function read_all_contributors(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select contributor_id, contributor_name from contributor_tbl order by contributor_name asc"
        read_all_contributors = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_all_languages(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim luser As String
        Dim lname As String
        If bc_cs_central_settings.server_flag = 0 Then
            luser = bc_cs_central_settings.logged_on_user_id
            lname = bc_cs_central_settings.logged_on_user_name
        Else
            luser = certificate.user_id
            lname = certificate.name
        End If

        sql = "select l.language_id, v.language_code, v.label_value from language_table l, language_table m, label_value_tbl v where " + _
              " l.label_code = v.label_code and " + _
              " v.language_code = m.language_code and v.language_code='en' order by v.label_value asc"
        read_all_languages = gbc_db.executesql(sql, certificate)

    End Function
    Public Function channels_for_pub_type(pub_type_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_get_dist_channels_for_pubtype " + CStr(pub_type_id)
        channels_for_pub_type = gbc_db.executesql(sql, certificate)
    End Function

    Public Function is_distributable(pub_type_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_pt_distributable " + CStr(pub_type_id)
        is_distributable = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_pub_type_settings(pub_type_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String

        sql = "exec dbo.bc_core_get_pub_type_central_settings " + CStr(pub_type_id)
        read_pub_type_settings = gbc_db.executesql(sql, certificate)
    End Function
    Public Function read_bus_Areas(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select bus_area_id,bus_area_name from bus_area_table where coalesce(deleted,0)=0 order by bus_area_name asc"
        read_bus_Areas = gbc_db.executesql(sql, certificate)
    End Function

    Public Function read_all_stages(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim luser As String
        Dim lname As String
        If bc_cs_central_settings.server_flag = 0 Then
            luser = bc_cs_central_settings.logged_on_user_id
            lname = bc_cs_central_settings.logged_on_user_name
        Else
            luser = certificate.user_id
            lname = certificate.name
        End If

        sql = "select stage_id, stage_name from stage_tbl"
        read_all_stages = gbc_db.executesql(sql, certificate)

    End Function

    Public Sub delete_process_for_pub_type(ByVal pt_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_cp_delete_process_for_pt " + CStr(pt_id)
        gbc_db.executesql(sql, certificate)
    End Sub

    Public Function get_stages(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select stage_name from stage_tbl order by stage_id asc"
        get_stages = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_show_in_wizard_flag(ByVal pub_type_id As Long, ByRef certificate As bc_cs_security.certificate) As Boolean
        Dim sql As String
        Dim res As Object
        get_show_in_wizard_flag = True
        sql = "select count(*) from at_product where pub_type_id=" + CStr(pub_type_id)
        res = gbc_db.executesql(sql, certificate)
        If IsArray(res) Then
            If res(0, 0) = 0 Then
                get_show_in_wizard_flag = False
            End If
        End If

    End Function
    REM first version does not support entities or recursion
    Public Function get_documents_for_composite(ByVal pub_type_id As Long, ByRef doc_titles As ArrayList, ByRef certificate As bc_cs_security.certificate, Optional ByRef customParam As String = "") As ArrayList
        Dim otrace As New bc_cs_activity_log("bc_om_at_pub_type_db", "get_documents_for_composite", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try

            Dim sql As String
            Dim strings As Object
            Dim docs As Object
            Dim i, j As Integer
            Dim doc_count As Integer = 0
            Dim doc_ids As New ArrayList

            REM get query strings
            sql = "select query_string from at_composite_docs where pub_type_id=" + CStr(pub_type_id) + " order by ord asc"
            REM execute all query strings to retrieve document list
            strings = gbc_db.executesql(sql, certificate)
            If IsArray(strings) Then
                If UBound(strings, 2) > -1 Then
                    For i = 0 To UBound(strings, 2)
                        sql = strings(0, i)
                        'replace custom param in the sql
                        'currently only supports one custom parameter
                        If customParam <> "" Then
                            sql = sql.Replace("?", customParam)
                        End If
                        docs = gbc_db.executesql(sql, certificate)
                        If IsArray(docs) Then
                            If UBound(docs, 2) > -1 Then
                                For j = 0 To UBound(docs, 2)
                                    doc_ids.Add(docs(0, j))
                                    doc_titles.Add(docs(1, j))
                                    doc_count = doc_count + 1
                                Next
                            End If
                        End If
                    Next
                End If
            End If
            get_documents_for_composite = doc_ids
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_at_pub_type_db", "get_documents_for_composite", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            get_documents_for_composite = Nothing
        Finally
            otrace = New bc_cs_activity_log("bc_om_at_pub_type_db", "get_documents_for_composite", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function get_pub_type_entity_classify(pt_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_pub_type_entity_classify " + CStr(pt_id)
        get_pub_type_entity_classify = gbc_db.executesql(sql, certificate)
    End Function
    Public Function is_composite(ByVal pub_type_id As Long, ByRef certificate As bc_cs_security.certificate) As Boolean
        Dim otrace As New bc_cs_activity_log("is_composite", "get_documents_for_composite", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim sql As String
            Dim res As Object
            is_composite = False
            sql = "select count(*) from at_composite_docs where pub_type_id=" + CStr(pub_type_id)
            res = gbc_db.executesql(sql, certificate)
            If IsArray(res) Then
                If UBound(res, 2) > -1 Then
                    If res(0, 0) > 0 Then
                        is_composite = True
                    End If
                End If
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("is_composite", "get_documents_for_composite", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("is_composite", "get_documents_for_composite", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    Public Function get_lists_for_pub_type(ByVal pt As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("is_composite", "get_lists_for_pub_type", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim sql As String
            sql = "exec dbo.bcc_core_get_lists_for_pt " + CStr(pt)
            get_lists_for_pub_type = gbc_db.executesql_show_no_error(sql)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("is_composite", "get_lists_for_pub_type", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            get_lists_for_pub_type = Nothing
        Finally
            otrace = New bc_cs_activity_log("is_composite", "get_lists_for_pub_type", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function get_list_items_for_pub_type(ByVal id As Long, ByVal pt As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("is_composite", "get_list_items_for_pub_type", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim sql As String
            sql = "exec dbo.bcc_core_cat_get_list_items " + CStr(pt) + "," + CStr(id)
            get_list_items_for_pub_type = gbc_db.executesql(sql, certificate)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("is_composite", "get_list_items_for_pub_type", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            get_list_items_for_pub_type = Nothing
        Finally
            otrace = New bc_cs_activity_log("is_composite", "get_list_items_for_pub_type", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Public Function get_translated_pubtypes(ByVal id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim otrace As New bc_cs_activity_log("is_composite", "get_translated_pubtypes", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Dim sql As String
            sql = "select pub_type_id from pub_type_translation_tbl where lead_pub_type_id=" + CStr(id)
            get_translated_pubtypes = gbc_db.executesql_show_no_error(sql)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("is_composite", "get_translated_pubtypes", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
            get_translated_pubtypes = Nothing
        Finally
            otrace = New bc_cs_activity_log("is_composite", "get_translated_pubtypes", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function

    Public Function read_approvers(ByVal id As Long, ByVal stage_to As Long, ByVal doc_id As Long) As Object
        Dim sql As String
        sql = "select  doc_approvers_for_stage.user_id, container_Id from doc_approvers_for_stage, container_tbl where doc_id =" + CStr(doc_id) + " And  stage_to = " + CStr(stage_to) + " and container_tbl.stage_id=stage_from and container_id=" + CStr(id)
        read_approvers = gbc_db.executesql_show_no_error(sql)
    End Function
    REM FIL 5.3
    Public Function read_build_comps(ByVal id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "exec dbo.bc_core_get_build_comps " + CStr(id)
        read_build_comps = gbc_db.executesql(sql, certificate)
    End Function
    REM ===
    Public Function get_insertable_components(ByVal pub_type_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select component_id from bcc_core_insert_comp_pt where pub_type_id=" + CStr(pub_type_id)
        get_insertable_components = gbc_db.executesql_show_no_error(sql)
    End Function
    Public Function use_in_create_only(ByVal pub_type_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        Dim res As Object
        use_in_create_only = False
        sql = "select count(*) from bcc_core_pt_link_cr_reach where child_pub_type_id=" + CStr(pub_type_id)
        res = gbc_db.executesql_show_no_error(sql)
        If IsArray(res) Then
            If IsNumeric(res(0, 0)) Then
                If CInt(res(0, 0) > 0) Then
                    use_in_create_only = True
                    Exit Function
                End If
            End If
        End If


    End Function
End Class


<Serializable()> Public Class bc_om_wf_custom_events
    Inherits bc_cs_soap_base_class
    REM interface parameters
    Public instateactions As Boolean = False
    Public entity_id As Long
    Public doc_id As String
    Public stage_from As Integer
    Public stage_to As Integer
    Public stage_name_from As String
    Public stage_name_to As String
    REM other details
    Public pub_type_id As Long
    Public container_id As Long
    REM return codes
    Public err_text As String
    REM was event successful
    Public success As Boolean
    REM text returned from event
    Public output_text As String
    REM
    Public current_filename As String
    Public last_revision_filename As String
    Public revision_from_process As Boolean = False
    REM override stage if event returns this
    Public override_stage_id As Integer = 0
    Public override_stage_name As String = ""
    Public override_event As String = ""
    Public cdoc As bc_om_document
    Public refresh_components As bc_om_refresh_components
    Public style_components As bc_om_document_components
    Dim events As New ArrayList

    Private Function event_already_run(ByVal id As Long, ByVal doc_id As Long)
        Dim gdb As New bc_om_wf_custom_events_db
        Dim vres As Object
        event_already_run = False
        vres = gdb.can_event_run(id, doc_id, MyBase.certificate)
        If IsArray(vres) Then
            If vres(0, 0) = 1 Then
                event_already_run = True
            End If
        End If
    End Function
    Private Function revert_document() As Boolean
        Dim ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", " revert_document", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim err_text As String
        revert_document = True
        Try
            Dim fs As New bc_cs_file_transfer_services
            If fs.check_document_exists(bc_cs_central_settings.central_repos_path + Me.last_revision_filename) Then
                fs.file_copy(bc_cs_central_settings.central_repos_path + Me.last_revision_filename, bc_cs_central_settings.central_repos_path + Me.current_filename)
                fs.delete_file(bc_cs_central_settings.central_repos_path + Me.last_revision_filename)
            End If

        Catch ex As Exception
            revert_document = False
            err_text = ex.Message
            ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "revert_document", bc_cs_activity_codes.COMMENTARY, "File revert failed: " + ex.Message, MyBase.certificate)

        Finally
            ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "revert_document", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function


    Private Function rollback_events_trans(ByVal idx As Integer) As Boolean
        Dim ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "rollback_events", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim gdb As New bc_om_wf_custom_events_db

        Try
            Dim i As Integer
            REM rollback each previously successful event
            For i = 0 To idx - 1
                events(idx - 1 - i).rollback_event(Me.entity_id, Me.doc_id, Me.stage_from, Me.stage_to)
                If events(idx - 1 - i).rollback_text = "" Or events(idx - 1 - i).rollback_text = "distribution files not committed" Then
                    gdb.new_write_history(Me.doc_id, "Server side custom event rollback: " + events(idx - 1 - i).name, "SE", Me.stage_from, Me.stage_to, MyBase.certificate)
                    ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "run_events", bc_cs_activity_codes.COMMENTARY, "Server side custom event rollback: " + events(idx - 1 - i).name + " succeded", MyBase.certificate)

                Else
                    gdb.new_write_history(Me.doc_id, "Server side custom event rollback failed: " + events(idx - 1 - i).name + " " + events(idx - 1 - i).rollback_text, "SE", Me.stage_from, Me.stage_to, MyBase.certificate)
                    ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "run_events", bc_cs_activity_codes.COMMENTARY, "Server side custom event rollback failed: " + events(idx - 1 - i).name + " " + events(idx - 1 - i).rollback_text, MyBase.certificate)
                End If
            Next

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_wf_custom_events", "rollback_events", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "rollback_events", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        End Try
    End Function



    Private Function get_server_side_events() As Boolean
        Dim ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "get_server_side_events", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim gdb As New bc_om_wf_custom_events_db
            get_server_side_events = False
            Dim i As Integer
            Dim custom_event As bc_om_wf_custom_event
            events.Clear()
            Dim vres As Object

            vres = gdb.get_server_side_events(Me.pub_type_id, Me.stage_from, Me.stage_to, MyBase.certificate)
            If IsArray(vres) Then
                For i = 0 To UBound(vres, 2)
                    custom_event = New bc_om_wf_custom_event
                    custom_event.core = False
                    custom_event.id = vres(0, i)
                    custom_event.name = vres(1, i)
                    custom_event.sp_name = vres(2, i)
                    custom_event.web_service_name = vres(3, i)
                    custom_event.internal = False
                    If custom_event.web_service_name = "" Then
                        custom_event.internal = True
                    End If
                    custom_event.format_filename = vres(4, i)
                    custom_event.external_id = vres(5, i)
                    If vres(6, i) = "core server side distribution" Then
                        custom_event.distribution_event = True
                        custom_event.core = True
                    ElseIf vres(6, i) = "core server side" Then
                        custom_event.core = True
                    End If


                    custom_event.certificate = MyBase.certificate
                    events.Add(custom_event)

                Next
            End If


            get_server_side_events = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_wf_custom_events", "get_server_side_events", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "get_server_side_events", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        End Try

    End Function

    Public Function run_events_trans() As Boolean
        REM get list from database
        Dim files_to_commit As New ArrayList
        Dim json_file_required As Boolean = False
        Dim ocomm As New bc_cs_activity_log("bc_om_wf_custom_events", "run_events_trans", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Dim gdb As New bc_om_wf_custom_events_db
        REM transaction SQL object
        Dim db As bc_cs_db_services

        Try
            REM now run
            Dim i, j As Integer
            run_events_trans = False
            If Me.instateactions = False Then
                If get_server_side_events() = False Then
                    Exit Function
                End If
            End If

            If events.Count = 0 And IsNothing(Me.refresh_components) And IsNothing(style_components) Then
                Me.success = True
                run_events_trans = True
                Exit Function
            End If

            db = New bc_cs_db_services
            db.open_conn(certificate)
            db.success = True
            REM compontize components
            If Not IsNothing(Me.refresh_components) Then
                If Me.refresh_components.db_write_components(Me.doc_id, Me.stage_name_to, certificate, db) = False Then
                    events_failed(db, events.Count, True)
                    gdb.new_write_history(Me.doc_id, "Failed to write componetize components: " + Me.refresh_components.err_text, "SE", Me.stage_from, Me.stage_to, MyBase.certificate)
                    Me.err_text = Me.refresh_components.err_text
                    Exit Function
                End If
            End If

            REM style components

            If Not IsNothing(style_components) Then
                style_components.certificate = certificate
                json_file_required = style_components.db_write(db, files_to_commit)
                If Me.style_components.err_text <> "" Then
                    events_failed(db, events.Count, True)
                    gdb.new_write_history(Me.doc_id, "Failed to write style components: " + Me.style_components.err_text, "SE", Me.stage_from, Me.stage_to, MyBase.certificate)
                    Me.err_text = "Failed to write style components: " + Me.style_components.err_text
                    Exit Function
                End If
                Me.success = True

            End If

            If Me.instateactions = False Then
                Me.success = True
                ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "run_events_trans", bc_cs_activity_codes.COMMENTARY, "Attemting to run " + CStr(events.Count) + " events on stage_transition from: " + CStr(Me.stage_from) + " to " + CStr(Me.stage_to), MyBase.certificate)

                For i = 0 To events.Count - 1
                    REM check if event can only be run once
                    If event_already_run(events(i).id, Me.doc_id) = True Then
                        gdb.new_write_history(Me.doc_id, "Server side  event: " + events(i).name + " suppressed as can only be run once ", "SE", Me.stage_from, Me.stage_to, MyBase.certificate)
                    Else
                        If events(i).run_event(Me.entity_id, Me.doc_id, Me.stage_from, Me.stage_to, Me.cdoc, files_to_commit, db) = False Then
                            REM  need to rollback non database stuff that has happened
                            REM TBD
                            Me.success = False


                            events_failed(db, i)
                            Exit For

                        Else
                            REM write event history
                            For j = 0 To events(i).history.count - 1
                                gdb.new_write_history(Me.doc_id, events(i).history(j), "SE", Me.stage_from, Me.stage_to, MyBase.certificate, db)
                            Next
                            REM if event set output text then set here
                            If Me.output_text <> "" Then
                                Me.output_text = Me.output_text + "; " + events(i).output_text
                            Else
                                Me.output_text = events(i).output_text
                            End If
                            gdb.new_write_history(Me.doc_id, "Server side  event: " + events(i).name + " successfully executed", "SE", Me.stage_from, Me.stage_to, MyBase.certificate, db)
                            ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "run_events", bc_cs_activity_codes.COMMENTARY, "Server side custom event" + events(i).name + " successfully executed", MyBase.certificate)
                            REM routing override event TBD

                            If events(i).override_stage_id <> 0 Then
                                Me.override_stage_id = events(i).override_stage_id
                                Me.override_stage_name = events(i).override_stage_name
                                Me.override_event = events(i).name
                            End If
                        End If
                    End If
                Next
            End If
            REM if event failed then reset stage_change
            If Me.success = True Then



                Dim commit_files As Boolean = False
                Dim tx As String = ""
                commit_files = True
                If files_to_commit.Count > 0 Then
                    tx = write_commit_files(files_to_commit, db)
                    If tx <> "" Then
                        commit_files = False
                    End If
                End If
                If commit_files = False Then
                    events_failed(db, events.Count, True)
                    gdb.new_write_history(Me.doc_id, "Failed to commit file: " + tx, "SE", Me.stage_from, Me.stage_to, MyBase.certificate)
                    Me.err_text = tx
                Else
                    REM all good so commit data
                    db.close_conn(True, certificate)

                    REM if style components see if json file is required
                    If Not IsNothing(style_components) AndAlso json_file_required = True Then
                        Try
                            Dim endpointAddress As System.ServiceModel.EndpointAddress
                            If bc_cs_central_settings.soap_server = "" Then
                                endpointAddress = New System.ServiceModel.EndpointAddress("http://localhost/bc_core_components_svc/components.svc")
                            Else
                                endpointAddress = New System.ServiceModel.EndpointAddress(bc_cs_central_settings.soap_server)
                            End If
                            Dim basicHttpBinding As New System.ServiceModel.BasicHttpBinding
                            Dim service As New System.ServiceModel.ChannelFactory(Of ServiceReference1.IComponentsService)(basicHttpBinding, endpointAddress)
                            Dim gj As ServiceReference1.IComponentsService = service.CreateChannel
                            gj.GetJsonForDocumentCompostion(Me.doc_id, True)
                        Catch ex As Exception
                            If InStr(ex.Message, "quota") = 0 Then
                                success = False
                                err_text = "Failed to call web service for Json generation: " + ex.Message
                                Exit Function
                            End If
                        End Try
                    End If


                    If Me.override_stage_id <> 0 Then
                        gdb.new_write_history(Me.doc_id, "Server side custom event: " + Me.override_event + " changed routing to stage: " + CStr(Me.override_stage_name), "SE", Me.stage_from, Me.stage_to, MyBase.certificate)
                    End If
                    run_events_trans = True
                End If
            End If

        Catch ex As Exception
            gdb.new_write_history(Me.doc_id, "Server side custom event handler failed container will stay in stage: " + Me.stage_name_from, "SE", Me.stage_from, Me.stage_to, MyBase.certificate)
            Dim oerr As New bc_cs_error_log("bc_om_wf_custom_events", "run_events_trans", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "run_events_trans", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Function
    Private Function write_commit_files(ByVal files_to_commit As ArrayList, ByRef db As bc_cs_db_services)
        Dim ocomm As New bc_cs_activity_log("bc_om_wf_custom_events", "write_commit_files", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        Dim gdb As New bc_om_wf_custom_events_db
        Dim ds As New bc_cs_file_transfer_services

        write_commit_files = ""

        Try
            For i = 0 To files_to_commit.Count - 1
                Select Case files_to_commit(i).mode
                    Case bc_om_files_to_commit.CONTROL_FILE
                        Dim fs As New StreamWriter(files_to_commit(i).target_file, False)
                        fs.WriteLine(files_to_commit(i).contents)
                        fs.Close()
                        gdb.new_write_history(Me.doc_id, "Control File Committed: " + files_to_commit(i).target_file, "SE", Me.stage_from, Me.stage_to, MyBase.certificate, db)
                    Case bc_om_files_to_commit.FILE_COPY
                        If ds.check_document_exists(files_to_commit(i).source_file) Then
                            FileCopy(files_to_commit(i).source_file, files_to_commit(i).target_file)
                            gdb.new_write_history(Me.doc_id, "Document File Committed: " + files_to_commit(i).target_file, "SE", Me.stage_from, Me.stage_to, MyBase.certificate, db)
                        End If
                    Case bc_om_files_to_commit.DELETE

                        If ds.check_document_exists(files_to_commit(i).target_file) Then
                            ds.delete_file(files_to_commit(i).target_file)
                            gdb.new_write_history(Me.doc_id, "File Deleted: " + files_to_commit(i).target_file, "SE", Me.stage_from, Me.stage_to, MyBase.certificate, db)
                        End If
                    Case bc_om_files_to_commit.IMAGE
                        REM create dirctory
                        Try
                            System.IO.Directory.CreateDirectory(files_to_commit(i).target_dir)
                        Catch ex As Exception
                            write_commit_files = "Failed to create directory: " + files_to_commit(i).target_dir
                            Exit Function
                        End Try
                        REM write file
                        Dim ofs As New bc_cs_file_transfer_services
                        If ofs.write_bytestream_to_document(files_to_commit(i).target_dir + "\" + files_to_commit(i).target_file, files_to_commit(i).bimage, certificate, False) = False Then
                            write_commit_files = "Failed to write file: " + files_to_commit(i).target_dir + "\" + files_to_commit(i).target_file + " from bytestream: " + ofs.err_text
                            Exit Function
                        End If

                    Case Else
                        write_commit_files = "Invalid control file type: " + CStr(files_to_commit(i).mode)
                        Exit Function
                End Select
            Next
        Catch ex As Exception
            write_commit_files = ex.Message
        Finally
            ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "write_commit_files", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Private Sub events_failed(ByRef db As bc_cs_db_services, ByVal i As Integer, Optional ByVal commit_files As Boolean = False)
        Dim gdb As New bc_om_wf_custom_events_db

        db.close_conn(False, certificate)
        gdb.new_write_history(Me.doc_id, "Server side custom events failed " + Me.stage_name_from + " to " + Me.stage_name_to + " container will stay in stage: " + Me.stage_name_from, "SE", Me.stage_from, Me.stage_to, MyBase.certificate)
        Dim ocomm As New bc_cs_activity_log("bc_om_wf_custom_events", "run_events", bc_cs_activity_codes.COMMENTARY, "Server Side Custom Events failed Container reverted back to stage: " + Me.stage_name_from, MyBase.certificate)
        rollback_events_trans(i)
        If commit_files = False Then
            Me.err_text = events(i).err_text

            gdb.new_write_history(Me.doc_id, "Server side  event: " + events(i).name + " failed execution: " + events(i).err_text, "SE", Me.stage_from, Me.stage_to, MyBase.certificate)
            ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "run_events", bc_cs_activity_codes.COMMENTARY, "Server side custom event" + events(i).name + " failed execution: " + events(i).err_text, MyBase.certificate)

        End If
        REM if document itself has changed rollback only do if process stage change
        REM as could loose oridnary changes
        If Me.last_revision_filename <> "" And revision_from_process = True Then
            If revert_document() = False Then
                gdb.new_write_history(doc_id, "Failed to revert file after event failure", "SE", Me.stage_from, Me.stage_to, MyBase.certificate)
                ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "run_events", bc_cs_activity_codes.COMMENTARY, "Failed to revert file after event failure", MyBase.certificate)
            Else
                gdb.new_write_history(doc_id, "File Successfully reverted back  after event failure from: " + Me.last_revision_filename, "SE", Me.stage_from, Me.stage_to, MyBase.certificate)
                ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "run_events", bc_cs_activity_codes.COMMENTARY, "File successfully  reverted file after event failure", MyBase.certificate)
            End If
        Else
            'If Me.last_revision_filename <> "" And revision_from_process = False Then
            Dim fs As New bc_cs_file_transfer_services
            If Not IsNothing(cdoc.preeventbyteDoc) Then
                fs.write_bytestream_to_document(bc_cs_central_settings.central_repos_path + cdoc.filename, cdoc.preeventbyteDoc, certificate)
                REM need to let this work from a create submission
                gdb.new_write_history(doc_id, "File Successfully reverted back prior to events from create submission:" + cdoc.filename, "SE", Me.stage_from, Me.stage_to, MyBase.certificate)
                ocomm = New bc_cs_activity_log("bc_om_wf_custom_events", "run_events", bc_cs_activity_codes.COMMENTARY, "File Successfully reverted back prior to events from create submission:" + cdoc.filename, MyBase.certificate)
                cdoc.preeventbyteDoc = Nothing
            End If
            'End If
        End If

    End Sub

    Public Sub New()

    End Sub
End Class

<Serializable()> Public Class bc_om_wf_custom_event
    Inherits bc_cs_soap_base_class
    Public id As Long
    Public name As String
    REM 0 internal, 1 external
    Public internal As Boolean
    Public distribution_event As Boolean = False
    REM client or server side executed
    Public server_side As Boolean
    REM is it object based
    Public object_bases As Boolean
    REM object action
    REM 1 refresh, 2 render, 3 copy and refresh
    Public object_action As Integer
    Public suppress_state_change_in_fail As Boolean = False
    REM internal just supply as sp name
    Public core As Boolean = False
    REM external this is webmethod name
    Public sp_name As String
    REM used for external only
    Public format_filename As String
    Public web_service_name As String
    Public external_id As String
    REM rollback stuff here

    REM output
    Public err_text As String
    REM error reproting from a rollback
    Public rollback_text As String

    REM text returned from event
    Public output_text As String
    REM override stage if event returns this
    Public override_stage_id As Integer = 0
    Public override_stage_name As String = ""
    Public history As New ArrayList
    <NonSerialized()> Public control_file_name As String
    <NonSerialized()> Public target_location As String
    Public stage_to_id As Long

    Public Function rollback_event(ByVal entity_id As Long, ByVal doc_id As String, ByVal stage_from As Long, ByVal stage_to As Long) As Boolean
        Try
            Me.rollback_text = "no rollback facility"
            Me.history.Clear()
            If internal = True Then
                REM sp is now in a transaction
                If Me.core = False Then
                    REM SP based this in now done in a transaction
                    rollback_text = ""
                ElseIf Me.distribution_event = True Then
                    rollback_text = "distribution files not committed"
                Else
                    rollback_text = ""
                End If
            Else
                rollback_text = "Cant rollback an external event"
                REM TBD  need to work out how to rollback external data
            End If
        Catch ex As Exception
            rollback_text = "bc_om_wf_custom_event:rollback_event " + ex.Message
        End Try
    End Function
    Public Function run_core_event(ByVal doc_id As String, ByRef cdoc As bc_om_document, ByRef files_to_commit As ArrayList, ByRef db As bc_cs_db_services) As Boolean
        Try

            If Me.name = "Rename PDF" Then
                Dim rpdf As New bc_om_rename_pdf(doc_id, MyBase.certificate)
                run_core_event = True
                If rpdf.rename(db, files_to_commit) = False Then
                    run_core_event = False
                    Me.err_text = rpdf.err_text
                End If
            ElseIf Me.name = "Undistribute" Then
                Dim bou As New bc_om_undistribute(doc_id, db)
                run_core_event = True
                If bou.remove_docs(files_to_commit, MyBase.certificate) = False Then
                    run_core_event = False
                    Me.err_text = bou.err_text
                End If
                REM WF nov 2011
            ElseIf Me.name = "Generate Html" Then
                REM see if email preview required
                Dim res As Object
                res = db.executesql_trans("exec dbo.bc_core_get_email_template_id " + CStr(doc_id), certificate, True)
                run_core_event = True
                If IsArray(res) AndAlso UBound(res, 2) = 0 Then
                    Dim email_template_id As Integer
                    email_template_id = res(0, 0)
                    If email_template_id = 0 Then
                        run_core_event = True
                        Exit Function

                    End If
                    Dim et As New bc_cs_email_preview_services(certificate)
                    If et.create_preview(0, email_template_id, doc_id, 0, stage_to_id, db) = False Then
                        run_core_event = False
                        Me.err_text = "failed to generate html file: " + et.err_txt
                    End If
                Else
                    run_core_event = False
                    Me.err_text = "failed to generate html file no template_id configured."
                End If
            ElseIf Me.name = "Index PDF" Then

                Dim idxdoc As New bc_om_index_document
                idxdoc.certificate = certificate

                Dim iddoc As New bc_om_document
                iddoc.id = doc_id
                If idxdoc.index_pdf(iddoc, db) = False Then

                    run_core_event = False
                    Me.err_text = idxdoc.ErrorText
                Else

                    run_core_event = True
                End If




            Else
                run_core_event = False
                Me.err_text = "Core server side event: " + Me.name + " does not exist"
            End If
        Catch ex As Exception
            run_core_event = False
            Me.err_text = ex.Message

        End Try
    End Function


    Public Function run_core_distribution_event(ByVal doc_id As String, ByRef files_to_commit As ArrayList, Optional ByVal db As bc_cs_db_services = Nothing) As Boolean
        REM get extended parameters
        Dim gdb As New bc_om_wf_custom_events_db
        Dim res As Object
        Dim vres As Object

        Dim sp As String
        Dim fn As String
        fn = ""
        run_core_distribution_event = False

        Try

            Dim xdParams As New XmlDocument

            res = gdb.get_distribution_action_params(Me.id, MyBase.certificate, db)
            If IsNothing(db) = False Then
                If db.success = False Then
                    Exit Function
                End If
            End If

            If IsArray(res) Then
                Try
                    xdParams.LoadXml(res(0, 0))
                Catch ex As Exception
                    Me.err_text = "Distribution Actions failed to load."
                    Exit Function
                End Try
            Else
                Me.err_text = "Distribution Actions failed to load."
                Exit Function
            End If

            For Each xnParam As XmlNode In xdParams.GetElementsByTagName("bc_core_distribution_action_parameters")

                If xnParam.Attributes.Count = 0 Then

                    Me.err_text = "no distribution action parameters defined"
                    Exit Function

                Else

                    Me.control_file_name = Nothing
                    Me.target_location = Nothing

                    Dim strStoredProc As String = Nothing

                    If Not xnParam.Attributes("control_file_name") Is Nothing Then
                        Me.control_file_name = xnParam.Attributes("control_file_name").Value
                    End If
                    If Not xnParam.Attributes("target_location") Is Nothing Then
                        Me.target_location = xnParam.Attributes("target_location").Value
                    End If
                    If Not xnParam.Attributes("sp_name") Is Nothing Then
                        strStoredProc = xnParam.Attributes("sp_name").Value
                    End If

                    Dim alFiles As New ArrayList()

                    sp = "exec dbo." + Me.sp_name + " " + CStr(doc_id)

                    If Not strStoredProc Is Nothing Then

                        Dim xdFiles As New XmlDocument
                        Dim hdb As New bc_cs_db_services
                        'If IsNothing(db) = True Then
                        'res = hdb.executexmlsql_show_no_error(strStoredProc & " " & doc_id, certificate)
                        'Else
                        res = db.executesql_trans(strStoredProc & " " & doc_id, certificate, True)

                        'End If
                        If Not IsArray(res) Then
                            Me.err_text = "Distribution files failed to load."
                            Exit Function
                        End If
                        If db.success = False Then
                            Me.err_text = "Failed to execute SP: " + strStoredProc & " " & doc_id
                            Exit Function
                        End If

                        Try
                            REM AUG 2017 discoverd xml from an sp gets shopped into several rows if large
                            Dim rxml As String = ""
                            For i = 0 To UBound(res, 2)
                                rxml = rxml + res(0, i)
                            Next
                            'xdFiles.LoadXml(res(0, 0))
                            xdFiles.LoadXml(rxml)
                        Catch ex As Exception
                            Me.err_text = "Distribution files failed to load." + ex.Message

                            Exit Function
                        End Try
                        For Each xdFile As XmlNode In xdFiles.GetElementsByTagName("files")

                            Dim strFolder As String = Nothing
                            Dim strName As String = Nothing
                            Dim strOriginal As String = Nothing
                            Dim strGenerator As String = Nothing

                            If Not xdFile.Attributes("target_directory") Is Nothing Then
                                strFolder = xdFile.Attributes("target_directory").Value
                            End If

                            If Not xdFile.Attributes("file_name") Is Nothing Then
                                strName = xdFile.Attributes("file_name").Value
                            End If

                            If Not xdFile.Attributes("original_file_name") Is Nothing Then
                                strOriginal = xdFile.Attributes("original_file_name").Value
                            End If

                            If Not xdFile.Attributes("generated") Is Nothing AndAlso xdFile.Attributes("generated").Value = 1 Then
                                strGenerator = sp
                            End If

                            alFiles.Add(New DestinationFile(strFolder, strName, strOriginal, strGenerator))

                        Next

                    Else

                        Me.control_file_name = Me.control_file_name.Replace("<doc_id>", CStr(doc_id))
                        Me.target_location = Me.target_location.Replace("<repos>", bc_cs_central_settings.central_repos_path)

                        Dim strOriginal As String

                        res = gdb.get_file_name_for_doc(doc_id, MyBase.certificate, db)
                        If IsNothing(db) = False Then
                            If db.success = False Then
                                Exit Function
                            End If
                        End If
                        If IsArray(res) Then
                            strOriginal = res(0, 0)
                        End If

                        alFiles.Add(New DestinationFile(Me.target_location, Me.control_file_name, Nothing, sp))

                    End If

                    For Each df As DestinationFile In alFiles

                        Dim strFileContents As String = Nothing

                        If Not df.strGenerator Is Nothing Then



                            vres = db.executesql_trans(df.strGenerator, certificate, True)

                            If IsNothing(db) = False Then
                                If db.success = False Then
                                    Me.err_text = "Error executing SP: " + Me.sp_name
                                    Exit Function
                                End If
                            End If
                            If IsArray(vres) Then
                                If Left(vres(0, 0), 5) = "Error" Then
                                    Me.err_text = "Error executing SP: " + Me.sp_name + ":" + Right(vres(0, 0), Len(vres(0, 0)) - 5)
                                    Exit Function
                                End If
                                strFileContents = CStr(vres(0, 0))
                            End If


                        End If

                        If Not strFileContents Is Nothing Then
                            If strFileContents <> "" Then
                                Dim fc As New bc_om_files_to_commit
                                fc.mode = bc_om_files_to_commit.CONTROL_FILE
                                fc.contents = strFileContents
                                fc.target_file = df.strFolder + "\" + df.strName
                                files_to_commit.Add(fc)
                            End If

                        Else

                            If Me.target_location <> bc_cs_central_settings.central_repos_path Then
                                Try
                                    Dim file_found As Boolean = False
                                    Dim ft As New bc_cs_file_transfer_services
                                    If ft.check_document_exists(bc_cs_central_settings.central_repos_path + "\" + df.strOriginal) Then
                                        Dim fc As New bc_om_files_to_commit
                                        fc.mode = bc_om_files_to_commit.FILE_COPY
                                        fc.source_file = bc_cs_central_settings.central_repos_path + "\" + df.strOriginal
                                        fc.target_file = df.strFolder + "\" + df.strName
                                        files_to_commit.Add(fc)
                                        file_found = True
                                    Else
                                        REM see if its in commit buffer
                                        For i = 0 To files_to_commit.Count - 1
                                            If files_to_commit(i).target_dir + "\" + files_to_commit(i).target_file = bc_cs_central_settings.central_repos_path + df.strOriginal Then
                                                Dim fc As New bc_om_files_to_commit
                                                fc.mode = bc_om_files_to_commit.FILE_COPY
                                                fc.source_file = bc_cs_central_settings.central_repos_path + "\" + df.strOriginal
                                                fc.target_file = df.strFolder + "\" + df.strName
                                                files_to_commit.Add(fc)
                                                file_found = True
                                                Exit For
                                            End If
                                        Next
                                    End If
                                    If file_found = False Then
                                        Me.err_text = "cant find file " + bc_cs_central_settings.central_repos_path + df.strOriginal
                                        Exit Function
                                    End If

                                Catch ex As Exception
                                    Me.err_text = "cant copy file to: " + Me.target_location + df.strOriginal + " :" + ex.Message
                                    Exit Function
                                End Try

                            End If

                        End If

                    Next

                End If

            Next

            run_core_distribution_event = True

        Catch ex As Exception
            run_core_distribution_event = False
            Me.err_text = ex.Message
        End Try

    End Function

    Private Class DestinationFile
        Public strFolder As String
        Public strName As String
        Public strOriginal As String
        Public strGenerator As String
        Public Sub New(ByVal strFolder As String, ByVal strName As String, ByVal strOriginal As String, ByVal strGenerator As String)
            Me.strFolder = strFolder
            Me.strName = strName
            Me.strOriginal = strOriginal
            Me.strGenerator = strGenerator
        End Sub
    End Class

    Public Function run_event(ByVal entity_id As Long, ByVal doc_id As String, ByVal stage_from As Long, ByVal stage_to As Long, ByRef cdoc As bc_om_document, ByRef files_to_commit As ArrayList, Optional ByRef db As bc_cs_db_services = Nothing) As Boolean
        Try
            Dim gdb As New bc_om_wf_custom_events_db
            Dim bcs As New bc_cs_db_services


            run_event = False
            REM
            If internal = True Then
                If Me.core = True Then
                    If distribution_event = False Then
                        stage_to_id = stage_to


                        If run_core_event(doc_id, cdoc, files_to_commit, db) = False Then
                            run_event = False
                        Else
                            run_event = True
                        End If
                    Else
                        If run_core_distribution_event(doc_id, files_to_commit, db) = False Then
                            run_event = False
                        Else
                            run_event = True
                        End If
                    End If
                Else
                    Dim vres As Object
                    Dim sql As String
                    REM FIL JIRA 7301 add user id
                    sql = "exec dbo." + sp_name + " " + CStr(entity_id) + ",'" + CStr(doc_id) + "'," + CStr(stage_from) + "," + CStr(stage_to)
                    REM check number of paramters
                    If bcs.get_sp_number_of_parameters(sp_name, sql, certificate) = 5 Then
                        If bc_cs_central_settings.server_flag = 1 Then
                            sql = "exec dbo." + sp_name + " " + CStr(entity_id) + ",'" + CStr(doc_id) + "'," + CStr(stage_from) + "," + CStr(stage_to) + "," + CStr(certificate.user_id)
                        Else
                            sql = "exec dbo." + sp_name + " " + CStr(entity_id) + ",'" + CStr(doc_id) + "'," + CStr(stage_from) + "," + CStr(stage_to) + "," + CStr(bc_cs_central_settings.logged_on_user_id)
                        End If
                    ElseIf bcs.get_sp_number_of_parameters(sp_name, sql, certificate) = 7 Then
                        If bc_cs_central_settings.server_flag = 1 Then
                            sql = "exec dbo." + sp_name + " " + CStr(entity_id) + ",'" + CStr(doc_id) + "'," + CStr(stage_from) + "," + CStr(stage_to) + ",1,'9-9-9999'," + CStr(certificate.user_id)
                        Else
                            sql = "exec dbo." + sp_name + " " + CStr(entity_id) + ",'" + CStr(doc_id) + "'," + CStr(stage_from) + "," + CStr(stage_to) + ",1,'9-9-9999'," + CStr(bc_cs_central_settings.logged_on_user_id)
                        End If
                    End If
                    REM END JIRA
                    REM sp standard interface
                    Dim ocomm As New bc_cs_activity_log("bc_om_wf_custom_event", "run_event", bc_cs_activity_codes.COMMENTARY, "about to execute: " + sql, MyBase.certificate)

                    vres = gdb.run_server_side_event(sql, certificate, db)

                    If IsArray(vres) Then

                        Try
                            If CStr(vres(0, 0)) = "Error" Then
                                run_event = False
                                err_text = "Error executing SP: " + sql
                                Exit Function
                            End If
                        Catch
                            err_text = Me.name + ": invalid return code: " + sql
                            Exit Function
                        End Try

                        Try
                            If vres(0, 0) = "0" Then
                                run_event = True
                            Else
                                run_event = False
                                Try
                                    'Me.err_text = Me.name + " Error executing SP: " + sql + ": " + CStr(vres(1, 0))
                                    Me.err_text = Me.name + ": " + CStr(vres(1, 0))
                                    'err_text = vres(0, 0)
                                Catch
                                    Me.err_text = Me.name + ": " + CStr(vres(0, 0))
                                End Try
                            End If
                        Catch
                            err_text = Me.name
                            run_event = False
                        End Try
                        Try
                            Me.output_text = vres(2, 0)
                        Catch
                            Me.output_text = ""
                        End Try
                        Try
                            Me.override_stage_id = vres(3, 0)
                        Catch
                            Me.override_stage_id = 0
                        End Try
                        Try
                            Me.override_stage_name = vres(4, 0)
                        Catch
                            Me.override_stage_name = ""
                        End Try

                    End If
                End If
            Else
                REM external_read()
                If external_read(entity_id, doc_id, stage_from, stage_to, False, db) = False Then
                    run_event = False
                Else
                    run_event = True
                End If

            End If

            REM log event has been run successfully
            REM ABG JUNE 2012
            If run_event = True Then
                gdb.log_event(Me.id, doc_id, certificate, db)
            End If
            REM ======================
        Catch ex As Exception
            run_event = False
            err_text = "bc_om_wf_custom_event:run_event " + ex.Message

        End Try
    End Function
    Public Function external_read(ByVal entity_id As Long, ByVal doc_id As String, ByVal stage_from As Long, ByVal stage_to As Long, ByVal rollback_mode As Boolean, Optional ByRef db As bc_cs_db_services = Nothing) As Boolean
        Dim ocomm As New bc_cs_activity_log("bc_om_wf_custom_event", "external_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            REM check if data is alread cached
            REM now call the external web serice
            If bc_cs_central_settings.server_flag = 0 Then
                MyBase.certificate = New bc_cs_security.certificate
                MyBase.certificate.user_id = bc_cs_central_settings.logged_on_user_id
                MyBase.certificate.os_name = bc_cs_central_settings.GetLoginName
                MyBase.certificate.name = bc_cs_central_settings.logged_on_user_name
            End If
            Dim oconn As New bc_cs_third_party_web_service
            Dim res As Object
            res = oconn.call_web_service_dynamic(Me.sp_name, Me.web_service_name, external_id, entity_id, 0, 0, 0, 0, "9-9-9999", doc_id, 0, 0, 0, Nothing, rollback_mode, MyBase.certificate, "", db)
            If oconn.err = True Then
                external_read = False
                Me.err_text = oconn.error_text
                Exit Function
            End If
            Dim oxml As String
            If IsNothing(res) = False Then
                If LCase(res.GetType.ToString) = "system.string" Then
                    oxml = CStr(res)
                Else
                    Me.err_text = "invalid data type returned from webservice: " + CStr(res.GetType.ToString)
                    external_read = False
                    Exit Function
                End If
            Else
                REM if nothing returned then proceed
                oxml = ""
            End If

            REM add parameter to external data and wrap external data
            oxml = "<all_data><external_data>" + oxml + "</external_data>" + Me.write_params_to_xml(entity_id, stage_from, stage_to, doc_id, external_id, rollback_mode) + "</all_data>"
            If bc_cs_central_settings.log_external_data = 1 Then
                ocomm = New bc_cs_activity_log("bc_om_refresh_component", "external_read", bc_cs_activity_codes.COMMENTARY, "Data Before Transformtaion: " + oxml, MyBase.certificate)
            End If
            If translate_xml(oxml) = False Then
                external_read = False
                Exit Function
            End If
            external_read = True
        Catch ex As Exception
            Me.err_text = "bc_om_wf_custom_event:external_read" + ex.Message
        Finally
            ocomm = New bc_cs_activity_log("bc_om_wf_custom_event", "external_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function
    Private Function write_params_to_xml(ByVal entity_id As Long, ByVal stage_from As Long, ByVal stage_to As Long, ByVal doc_id As String, ByVal external_id As Long, ByVal rollback_mode As Boolean) As String
        Dim xml As String
        xml = "<bc_parameters><core>"
        xml = xml + "<entity_id>" + CStr(entity_id) + "</entity_id>"
        xml = xml + "<stage_from>" + CStr(stage_from) + "</stage_from>"
        xml = xml + "<stage_to>" + CStr(stage_to) + "</stage_to>"
        xml = xml + "<doc_id>" + CStr(doc_id) + "</doc_id>"
        xml = xml + "<external_id>" + CStr(external_id) + "</external_id>"
        xml = xml + "<rollback_mode>" + CStr(rollback_mode) + "</rollback_mode>"
        xml = xml + "</core>"
        xml = xml + "</bc_parameters>"
        write_params_to_xml = xml
    End Function
    Private Function translate_xml(ByVal xml_native As String) As Boolean
        '        REM mode 1 single cell, 2 table, 3 chart, 4 db image, 5 fs image, 6 index, 7 document insertion from db, 12, file system non entity image
        '        REM 1 <value>
        '        REM 2,6 <value><row><col><style>
        '        REM 3 <value><row><col>
        '        REM 4,5,7,12 <value> blob
        Dim xmldoc As New XmlDocument
        Dim xmlNReader As XmlNodeReader
        Dim xsltran As New System.Xml.Xsl.XslCompiledTransform
        Dim outdata As New Object
        Dim xslsettings As New System.Xml.Xsl.XsltSettings(True, False)
        Try
            translate_xml = False
            REM firstly try and load xslt cannot proceed without correct formed xslt
            Try
                xsltran.Load(bc_cs_central_settings.central_template_path + Me.format_filename, xslsettings, New XmlUrlResolver)
            Catch ex As Exception
                err_text = "Error with " + Me.format_filename + " " + ex.Message
                Exit Function
            End Try
            Try
                xmldoc.LoadXml(xml_native)
            Catch ex As Exception
                err_text = "error loading native xml" + ex.Message
                Exit Function
            End Try
            REM now apply the transformation to the xml
            Dim memStream As New MemoryStream
            Dim xmlsettings As New XmlWriterSettings
            xmlsettings.Encoding = New System.Text.UTF8Encoding(False)
            Dim strWriter As New StringWriter
            Dim outDoc = XmlWriter.Create(memStream, xmlsettings)
            Try
                xmlNReader = New XmlNodeReader(xmldoc)
                xsltran.Transform(xmlNReader, outDoc)
                outDoc.Flush()
                outDoc.Close()
                If bc_cs_central_settings.log_external_data = 1 Then
                    Dim ocomm As New bc_cs_activity_log("bc_om_refresh_component", "tranlate_xml", bc_cs_activity_codes.COMMENTARY, "XML after transformation: " + System.Text.Encoding.UTF8.GetString(memStream.ToArray), MyBase.certificate)
                End If
            Catch ex As Exception
                err_text = "unable to transform xml " + ex.Message
                Exit Function
            End Try
            REM now load the transformed xml
            Dim outxmldoc As New XmlDocument
            Try
                outxmldoc.LoadXml(System.Text.Encoding.UTF8.GetString(memStream.ToArray))
            Catch ex As Exception
                err_text = "unable to load transformed xml from webservice " + ex.Message
                Exit Function
            End Try
            REM now parse the output into object
            Dim myXmlNodeList As Xml.XmlNodeList

            myXmlNodeList = outxmldoc.SelectNodes("/data/success")
            If myXmlNodeList.Count = 1 Then
                If myXmlNodeList(0).InnerXml = 0 Then
                    translate_xml = True
                End If
            Else
                err_text = "cannot find node /data/success in transformed output"
                translate_xml = False
                Exit Function
            End If
            myXmlNodeList = outxmldoc.SelectNodes("/data/error_text")
            If myXmlNodeList.Count = 1 Then
                Me.err_text = myXmlNodeList(0).InnerXml
            End If
            myXmlNodeList = outxmldoc.SelectNodes("/data/output_text")
            If myXmlNodeList.Count = 1 Then
                Me.output_text = myXmlNodeList(0).InnerXml
            End If
            myXmlNodeList = outxmldoc.SelectNodes("/data/override_stage_id")
            If myXmlNodeList.Count = 1 Then
                If IsNumeric(myXmlNodeList(0).InnerXml) Then
                    Me.override_stage_id = myXmlNodeList(0).InnerXml
                End If
            End If
            myXmlNodeList = outxmldoc.SelectNodes("/data/override_stage_name")
            If myXmlNodeList.Count = 1 Then
                Me.override_stage_name = myXmlNodeList(0).InnerXml
            End If
            REM stop supporting this from April 2013
            'myXmlNodeList = outxmldoc.SelectNodes("/data/filerename/newname")
            'If myXmlNodeList.Count = 1 Then
            '    Dim nfilename As String
            '    Dim doc_id As String
            '    nfilename = myXmlNodeList(0).InnerXml
            '    myXmlNodeList = outxmldoc.SelectNodes("/data/filerename/doc_id")
            '    If myXmlNodeList.Count = 1 Then 
            '        doc_id = myXmlNodeList(0).InnerXml
            '        Me.err_text = move_file(doc_id, nfilename, Nothing)
            '        If Me.err_text <> "" Then
            '            translate_xml = False
            '            Exit Function
            '        End If
            '    Else
            '        Me.err_text = "No doc_id returned from xml for file rename"
            '        translate_xml = False
            '    End If
            'End If

            REM now look for any data manipulation required from the event
            'myXmlNodeList = outxmldoc.SelectNodes("/data/executesql")
            'Dim gdb As New bc_om_wf_custom_events_db
            'For i = 0 To myXmlNodeList.Count - 1
            '    Dim ocomm As New bc_cs_activity_log("bc_om_wf_custom_event", "translate_xml", bc_cs_activity_codes.COMMENTARY, "attempting to execute sql: " + myXmlNodeList(i).InnerXml, certificate)
            '    Me.err_text = gdb.execute_returned_sql(myXmlNodeList(i).InnerXml)
            '    If Me.err_text <> "" Then
            '        translate_xml = False
            '        Exit For
            '    End If
            'Next

            REM see if any history needs to be set as everything else is fine
            'myXmlNodeList = outxmldoc.SelectNodes("/data/history")
            'For i = 0 To myXmlNodeList.Count - 1
            '    Dim ocomm As New bc_cs_activity_log("bc_om_wf_custom_event", "translate_xml", bc_cs_activity_codes.COMMENTARY, "attempting to execute sql: " + myXmlNodeList(i).InnerXml, certificate)
            '    history.Add(myXmlNodeList(i).InnerXml)
            'Next
        Catch ex As Exception
            err_text = "bc_om_wf_custom_event:translate_xml " + ex.Message
            translate_xml = False
            Dim oerr As New bc_cs_error_log("bc_om_wf_custom_event", "translate_xml", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally

        End Try
    End Function
    Private Function move_file(ByVal doc_id As String, ByVal nfilename As String, ByVal db As bc_cs_db_services) As String
        Try
            Dim gdb As New bc_om_wf_custom_events_db
            Dim vres As Object
            Dim ofn As String = ""
            Dim fs As New bc_cs_file_transfer_services

            vres = gdb.get_file_name_for_doc(doc_id, certificate, db)
            If Left(Right(nfilename, 4), 1) <> "." Then
                nfilename = nfilename + Right(CStr(vres(1, 0)), 4)
            End If
            If IsArray(vres) Then
                ofn = vres(0, 0)
                If ofn = nfilename Then
                    history.Add("filename: " + nfilename + " already exists")
                    move_file = ""
                    Exit Function
                End If
                If fs.file_copy(bc_cs_central_settings.central_repos_path + ofn, bc_cs_central_settings.central_repos_path + nfilename, True) <> 0 Then
                    move_file = "could not move file: " + ofn + " to: " + nfilename
                    Exit Function
                End If
                fs.delete_file(bc_cs_central_settings.central_repos_path + ofn, certificate, False)
                REM now update db
                gdb.update_doc(doc_id, nfilename, certificate, db)
                move_file = ""
            Else
                move_file = "could not move to new file: " + nfilename
            End If
            history.Add("File renamed from " + ofn + " to " + nfilename)
        Catch ex As Exception
            move_file = "could not move file: " + ex.Message
        Finally
        End Try
    End Function


    Public Sub New()

    End Sub
End Class
Public Class bc_om_clone_doc_db
    Private gbc_db As New bc_cs_db_services
    Public Function get_document_list(ByVal pt_id As Long, ByVal entity_id As Long, ByVal sub_entity_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_get_cloned_document_list " + CStr(pt_id) + "," + CStr(entity_id) + "," + CStr(sub_entity_id)
        get_document_list = gbc_db.executesql(sql, certificate)
    End Function

End Class
Public Class bc_om_wf_custom_events_db
    Private gbc_db As New bc_cs_db_services
    Public Sub update_doc(ByVal doc_id As String, ByVal filename As String, ByRef certificate As bc_cs_security.certificate, Optional ByRef db As bc_cs_db_services = Nothing)
        Dim sql As String
        Dim str As New bc_cs_string_services(filename)
        filename = str.delimit_apostrophies
        sql = "update document_table set doc_filename='" + filename + "', doc_object='" + filename + "' where doc_id=" + CStr(doc_id)
        If IsNothing(db) = True Then
            gbc_db.executesql_show_no_error(sql)
        Else
            db.executesql_trans(sql, certificate, True)

        End If
    End Sub

    Public Function get_file_name_for_doc(ByVal doc_id As String, ByRef certificate As bc_cs_security.certificate, Optional ByRef db As bc_cs_db_services = Nothing)
        Dim sql As String

        sql = "select doc_filename, doc_extension from document_table where doc_id='" + CStr(doc_id) + "'"
        If IsNothing(db) = True Then
            get_file_name_for_doc = gbc_db.executesql(sql, certificate)
        Else
            get_file_name_for_doc = db.executesql_trans(sql, certificate, True)
        End If
    End Function
    REM ABG JUNE 2012
    Public Function can_event_run(ByVal id As Long, ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "exec dbo.bc_core_check_event " + CStr(id) + "," + CStr(doc_id)
        can_event_run = gbc_db.executesql(sql, certificate)

    End Function
    Public Sub log_event(ByVal id As Long, ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate, Optional ByRef db As bc_cs_db_services = Nothing)
        Dim sql As String
        sql = "insert into dbo.bc_core_log_event (id, doc_id, run_date) values(" + CStr(id) + "," + CStr(doc_id) + ",getdate())"
        If IsNothing(db) = True Then
            gbc_db.executesql(sql, certificate)
        Else
            db.executesql_trans(sql, certificate, True)

        End If
    End Sub
    REM ==================
    Public Function execute_returned_sql(ByVal sql As String) As String
        Dim vres As Object

        execute_returned_sql = ""
        vres = gbc_db.executesql_show_no_error(sql)
        If IsArray(vres) Then
            If vres(0, 0) = "Error" Then
                execute_returned_sql = "error with returned sql: " + sql
            End If
        End If

    End Function

    Public Function get_server_side_events(ByVal pub_type_id As Long, ByVal stage_from As Long, ByVal stage_to As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "select a.action_id, a.action_name,calling_method,coalesce(web_service_name,''),coalesce(format_file,''),coalesce(external_id,0),calling_object from action_tbl a" + _
              " inner join wf_pubtype_stages_routes_trans_events w " + _
              " on w.pubtype_id=" + CStr(pub_type_id) + " and w.stage_from_id=" + CStr(stage_from) + " And w.stage_to_id = " + CStr(stage_to) + _
              " where a.action_id=w.action_id  and calling_object in ('custom server side', 'core server side distribution','core server side')" + _
              " order by action_sequence asc"
        get_server_side_events = gbc_db.executesql(sql, certificate)
    End Function
    Public Function get_distribution_action_params(ByVal action_id As Long, ByRef certificate As bc_cs_security.certificate, Optional ByRef db As bc_cs_db_services = Nothing) As Object
        Dim sql As String
        sql = "select control_file_name,target_location,sp_name from bc_core_distribution_action_parameters where action_id=" + CStr(action_id) + " for xml auto"
        If IsNothing(db) = True Then
            get_distribution_action_params = gbc_db.executexmlsql_show_no_error(sql, certificate)
        Else
            get_distribution_action_params = db.executesql_trans(sql, certificate, True)

        End If
    End Function
    Public Function run_server_side_event(ByVal sql As String, Optional ByRef certificate As bc_cs_security.certificate = Nothing, Optional ByRef db As bc_cs_db_services = Nothing) As Object
        If IsNothing(db) = True Then
            run_server_side_event = gbc_db.executesql_show_no_error(sql)
        Else
            run_server_side_event = db.executesql_trans(sql, certificate, True)
        End If
    End Function
    Public Sub set_container_stage(ByVal container_id As Long, ByVal stage_from_id As Long, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "update container_tbl set stage_id=" + CStr(stage_from_id) + " where container_id=" + CStr(container_id)
        gbc_db.executesql(sql, certificate)
    End Sub

    'Public Sub write_history(ByVal container_id As Long, ByVal desc As String, ByRef certificate As bc_cs_security.certificate)
    '    Dim sql As String
    '    Dim str As New bc_cs_string_services(desc)
    '    desc = str.delimit_apostrophies
    '    sql = "insert into history_tbl (CONTAINER_ID, HISTORY_DATE, DESCRIPTION, HISTORY_COMMENT, HISTORY_CODE) values (" + CStr(container_id) + ",getdate(),'" + desc + "','Server Side Custom Event','E')"
    '    gbc_db.executesql(sql, certificate)
    'End Sub
    Public Sub new_write_history(ByVal doc_id As Long, ByVal desc As String, ByVal code As String, ByVal stage_from As Long, ByVal stage_to As Long, ByRef certificate As bc_cs_security.certificate, Optional ByRef db As bc_cs_db_services = Nothing)

        Dim sql As String
        Dim user_Id As Long
        Dim str As New bc_cs_string_services(desc)
        desc = str.delimit_apostrophies
        If bc_cs_central_settings.server_flag = 0 Then
            user_Id = bc_cs_central_settings.logged_on_user_id
        Else
            user_Id = certificate.user_id
        End If

        sql = "exec dbo.bc_core_write_history " + CStr(doc_id) + ",'" + desc + "'," + CStr(user_Id) + ",'" + code + "'," + CStr(stage_from) + "," + CStr(stage_to)
        If IsNothing(db) Then
            gbc_db.executesql(sql, certificate)
        Else
            db.executesql_trans(sql, certificate, True)

        End If
    End Sub
    Public Function get_stage_name_from_id(ByVal stage_id As Long, ByRef certificate As bc_cs_security.certificate) As String

        Dim sql As String
        Dim vres As Object
        get_stage_name_from_id = 1
        sql = "select stage_name from stage_tbl where stage_id=" + CStr(stage_id)
        vres = gbc_db.executesql(sql, certificate)
        If IsArray(vres) Then
            get_stage_name_from_id = vres(0, 0)
        End If

    End Function

End Class
Public Class bc_om_rename_pdf
    Public err_text As String
    Dim certificate As bc_cs_security.certificate
    Dim doc_id As Long
    Dim gdb As New bc_om_rename_pdf_db
    Public Sub New(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate)
        Me.doc_id = doc_id
        Me.certificate = certificate
    End Sub
    Public Function rename(ByRef db As bc_cs_db_services, ByRef commit_files As ArrayList) As Boolean
        Try
            Dim file_details As Object
            Dim ffrom As String
            Dim fto As String
            Dim fs As New bc_cs_file_transfer_services
            Dim oc As New bc_om_files_to_commit

            rename = False
            REM call db to get file name from and to
            file_details = gdb.get_file_details(doc_id, certificate, db)
            If db.success = False Then
                err_text = "failed to execute exec dbo.bc_core_rename_pdf_file_details " + CStr(doc_id)
                Exit Function
            End If
            If IsArray(file_details) Then
                Try
                    ffrom = file_details(0, 0)
                    fto = file_details(1, 0)
                    If ffrom = "" Then
                        err_text = "err: pdf could not be found for document"
                        Exit Function
                    End If
                    If ffrom = fto Then
                        rename = True
                    End If
                    If ffrom = fto Then
                        rename = True
                        Exit Function
                    End If

                    If fs.check_document_exists(bc_cs_central_settings.central_repos_path + ffrom) = True Then
                        oc = New bc_om_files_to_commit
                        oc.mode = bc_om_files_to_commit.FILE_COPY
                        oc.source_file = bc_cs_central_settings.central_repos_path + ffrom
                        oc.target_file = bc_cs_central_settings.central_repos_path + fto
                        commit_files.Add(oc)
                        oc = New bc_om_files_to_commit
                        oc.mode = bc_om_files_to_commit.DELETE
                        oc.target_file = bc_cs_central_settings.central_repos_path + ffrom
                        commit_files.Add(oc)
                        gdb.set_new_filename(doc_id, fto, db, certificate)
                        If db.success = False Then
                            err_text = "error executing:  dbo.bc_core_rename_filename " + CStr(doc_id) + ",'" + fto + "'"
                            Exit Function
                        End If
                        rename = True
                    Else
                        err_text = "err: file " + bc_cs_central_settings.central_repos_path + ffrom + " not found on server"
                        Exit Function
                    End If

                Catch ex As Exception
                    err_text = "err: invalid file details"
                    Exit Function

                End Try

            End If

            rename = True
        Catch ex As Exception
            err_text = "err: " + ex.Message
        End Try

    End Function
    Public Class bc_om_rename_pdf_db
        Private gbc_db As New bc_cs_db_services(False)
        Public Function get_file_details(ByVal doc_id As Long, ByRef certificate As bc_cs_security.certificate, ByRef db As bc_cs_db_services) As Object
            Dim sql As String
            sql = "exec dbo.bc_core_rename_pdf_file_details " + CStr(doc_id)
            get_file_details = db.executesql_trans(sql, certificate, True)
        End Function
        Public Sub set_new_filename(ByVal doc_id As Long, ByVal filename As String, ByRef db As bc_cs_db_services, ByVal certificate As bc_cs_security.certificate)
            Dim sql As String
            Dim str As New bc_cs_string_services(filename)
            filename = str.delimit_apostrophies
            sql = "exec dbo.bc_core_rename_filename " + CStr(doc_id) + ",'" + filename + "'"
            db.executesql_trans(sql, certificate, True)
        End Sub


    End Class
End Class
