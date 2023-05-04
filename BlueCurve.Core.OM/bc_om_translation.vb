Imports System.IO
Imports System.Xml
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM



Public Class bc_om_translation

End Class


<Serializable()> Public Class bc_om_translation_groups
    Inherits bc_cs_soap_base_class
    Public TranslationGroups As New ArrayList
    Public Sub New()

    End Sub

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_translation_groups", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Select Case Me.tmode
                Case tREAD
                    db_read()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_translation_groups", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_translation_groups", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Sub


    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_translation_groups", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim i As Integer
            Dim vLabel As Object
            Dim oLabel As bc_om_translation_group
            Dim dbInput As New bc_om_at_translation_group_db
            MyBase.certificate = certificate

            TranslationGroups.Clear()
            vLabel = dbInput.read_inputs(MyBase.certificate)
            If IsArray(vLabel) Then
                For i = 0 To UBound(vLabel, 2)
                    oLabel = New bc_om_translation_group(vLabel(0, i), vLabel(1, i), vLabel(2, i), vLabel(3, i))
                    oLabel.TableName = vLabel(4, i)
                    oLabel.Prefix = vLabel(5, i)
                    oLabel.Name = vLabel(6, i)
                    oLabel.ItemId = vLabel(7, i)
                    TranslationGroups.Add(oLabel)
                Next
            End If
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_translation_groups", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_translation_groups", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class


<Serializable()> Public Class bc_om_translation_group
    Inherits bc_cs_soap_base_class

    Public CollectionCode As Long
    Public CollectionDesc As String
    Public CollectionType As String
    Public ClassId As Long
    Public TableName As String
    Public Prefix As String
    Public Name As String
    Public ItemId As Long

    Public Sub New()

    End Sub
    Public Sub New(ByVal collcode As Long, ByVal strcoll_desc As String, ByVal strcoll_type As String, ByVal strcoll_classid As Long)
        CollectionCode = collcode
        CollectionDesc = strcoll_desc
        CollectionType = strcoll_type
        ClassId = strcoll_classid
    End Sub

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_translation_group", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Select Case Me.tmode
                Case tREAD
                    db_read()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_translation_group", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_translation_group", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Sub

    Public Sub db_read()

    End Sub

End Class


Public Class bc_om_at_translation_group_db
    Private gbc_db As New bc_cs_db_services
    Public Sub New()

    End Sub

    REM reads all droups for traslator
    Public Function read_inputs(ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_core_trans_getgroups"
        read_inputs = gbc_db.executesql(sql, certificate)
    End Function


End Class


<Serializable()> Public Class bc_om_translation_items
    REM Traslation items
    Inherits bc_cs_soap_base_class
    Public ReadCollectionCode As Long
    Public ReadItemId As Long
    Public Languages As New ArrayList
    Public TranslationItems As New ArrayList
    Public Sub New()

    End Sub

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_translation_items", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Select Case Me.tmode
                Case tREAD
                    db_read()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_translation_items", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_translation_items", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Sub

    Public Sub db_read()
        Dim otrace As New bc_cs_activity_log("bc_om_translation_items", "db_read", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try
            Dim i As Integer
            Dim vLabel As Object
            Dim oLabel As bc_om_translation_item
            Dim dbInput As New bc_om_at_translation_item_db
            Dim vLanguages As Object
            Dim oLanguages As bc_om_translation_language
            MyBase.certificate = certificate

            TranslationItems.Clear()
            Languages.Clear()
            vLabel = dbInput.read_items(Me.ReadCollectionCode, Me.ReadItemId, MyBase.certificate)
            If IsArray(vLabel) Then
                For i = 0 To UBound(vLabel, 2)
                    oLabel = New bc_om_translation_item(vLabel(0, i), vLabel(1, i), vLabel(2, i), vLabel(3, i))
                    oLabel.ItemDesc = vLabel(4, i)
                    oLabel.ItemName = vLabel(5, i)
                    oLabel.ItemDefaultValue = vLabel(6, i)
                    oLabel.ItemLabel = vLabel(7, i)
                    oLabel.CountryCode = vLabel(8, i)
                    oLabel.LanguageCode = vLabel(9, i)
                    oLabel.CollectionType = vLabel(10, i)
                    oLabel.LabelValue = vLabel(11, i)
                    oLabel.ItemChanged = False
                    TranslationItems.Add(oLabel)
                Next
            End If

            vLanguages = dbInput.read_all_languages(MyBase.certificate)
            If IsArray(vLanguages) Then
                For i = 0 To UBound(vLanguages, 2)
                    oLanguages = New bc_om_translation_language(vLanguages(0, i), vLanguages(1, i), vLanguages(2, i))
                    Languages.Add(oLanguages)
                Next
            End If


        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_translation_items", "db_read", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_translation_items", "db_read", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class


<Serializable()> Public Class bc_om_translation_item
    Inherits bc_cs_soap_base_class

    Public CollectionCode As Long
    Public CollectionDesc As String
    Public CollectionType As String
    Public ItemId As Long
    Public Attribute As String
    Public ItemDesc As String
    Public ItemName As String
    Public ItemDefaultValue As String
    Public ItemLabel As String
    Public CountryCode As String
    Public LanguageCode As String
    Public LabelValue As String
    Public ItemChanged As Boolean
    Public WriteMode As Integer = 0

    Public Const INSERT = 0
    Public Const UPDATE = 1
    Public Const DELETE = 2
    Public Const SET_ACTIVE = 3
    Public Const SET_INACTIVE = 4

    Public Sub New()

    End Sub
    Public Sub New(ByVal collcode As Long, ByVal strcoll_desc As String, ByVal strcoll_itemid As Long, ByVal strattribute As String)
        CollectionCode = collcode
        CollectionDesc = strcoll_desc
        ItemId = strcoll_itemid
        Attribute = strattribute
    End Sub

    Public Overrides Sub process_object()
        Dim otrace As New bc_cs_activity_log("bc_om_translation_item", "process_object", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)
        Try
            Select Case Me.tmode
                Case tWRITE
                    db_write()
                Case tREAD
                    db_read()
            End Select
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_om_translation_item", "process_object", bc_cs_error_codes.USER_DEFINED, ex.Message, MyBase.certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_translation_item", "process_object", bc_cs_activity_codes.TRACE_EXIT, "", certificate)

        End Try
    End Sub


    Public Sub db_read()

    End Sub

    Public Sub db_write()
        Dim gdb As New bc_om_at_translation_item_db

        Select Case WriteMode
            Case INSERT
                gdb.update_transitem(Me.ItemLabel, Me.ItemDesc, Me.CountryCode, Me.LanguageCode, Me.LabelValue, MyBase.certificate)
            Case UPDATE
                gdb.update_transitem(Me.ItemLabel, Me.ItemDesc, Me.CountryCode, Me.LanguageCode, Me.LabelValue, MyBase.certificate)
        End Select

    End Sub

End Class

Public Class bc_om_at_translation_item_db
    Private gbc_db As New bc_cs_db_services
    Public Sub New()

    End Sub

    REM reads translation items
    Public Function read_items(ByVal collectioncode As Long, ByVal entitytableid As Long, ByRef certificate As bc_cs_security.certificate) As Object
        Dim sql As String
        sql = "bc_core_trans_getitemdata " + CStr(collectioncode) + "," + CStr(entitytableid)
        read_items = gbc_db.executesql(sql, certificate)
    End Function


    Public Function read_all_languages(ByRef certificate As bc_cs_security.certificate)
        Dim sql As String
        sql = "select l.language_id,l.country_code, l.language_code from language_table l"
        read_all_languages = gbc_db.executesql(sql, certificate)
    End Function


    Public Sub update_transitem(ByVal trans_label_code As String, ByVal row_name As String, ByVal trans_country As String, ByVal trans_language As String, ByVal trans_value As String, ByRef certificate As bc_cs_security.certificate)

        Dim sql As String
        REM update label code
        sql = "bc_core_trans_writelabel '" + trans_label_code + "','" + row_name + "','" + trans_country + "','" + trans_language + "',N'" + trans_value + "'"
        gbc_db.executesql(sql, certificate)
    End Sub

End Class

<Serializable()> Public Class bc_om_translation_language
    Public LanquageId As Long
    Public CountryCode As String
    Public LanguageCode As String
    Public Sub New(ByVal pid, ByVal pcountry_code, ByVal planguage_code)
        Me.LanquageId = pid
        Me.CountryCode = pcountry_code
        Me.LanguageCode = planguage_code
    End Sub
    Public Overrides Function ToString() As String
        Return LanquageId.ToString
    End Function
End Class