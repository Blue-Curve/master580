
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.IO
Imports iTextSharp.text.pdf
Imports iTextSharp.text.pdf.parser


<Serializable()> Public Class bc_om_index_document
    Inherits bc_cs_soap_base_class
    Public certificate As bc_cs_security.certificate


    Private _ErrorText As String

    Public Property ErrorText() As String
        Get
            Return _ErrorText
        End Get
        Set(ByVal value As String)
            _ErrorText = value
        End Set
    End Property


    Public Sub New()
        _ErrorText = ""

    End Sub



    Public Function index_pdf(odoc As bc_om_document, db As Object) As Boolean
        ''pdf_index

        Dim slog As New bc_cs_activity_log("bc_om_index_document", "index_pdf", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Dim Pagetext As String
        Dim IndexFilename As String

        Try

            'Get pdf filename from database
            odoc.db_read_for_filedetails(db)



            'Exit if not a pdf file
            If odoc.extension <> ".pdf" And odoc.extension <> "[imp].pdf" Then
                index_pdf = True
                Exit Function
            End If

            IndexFilename = bc_cs_central_settings.central_repos_path + odoc.filename


            Dim treader As New PdfReader(IndexFilename)
            Dim text_indexing As bc_om_index

            For x = 1 To treader.NumberOfPages

                Pagetext = PdfTextExtractor.GetTextFromPage(treader, x)

                text_indexing = New bc_om_index
                text_indexing.IndexTxid = 0
                text_indexing.IndexDocid = (odoc.id)
                text_indexing.Indexpageno = x
                text_indexing.Indexdoctext = Pagetext
                text_indexing.IndexDoctype = odoc.extension

                text_indexing.replace_bad()

                text_indexing.certificate = certificate


                text_indexing.write_mode = bc_om_index.UPDATE
                text_indexing.db_write(db)
                
            Next
            index_pdf = True
        Catch ex As Exception
            index_pdf = False
            _ErrorText = ex.Message
            'Dim db_err As New bc_cs_error_log("bc_om_index_document", "index_pdf", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally

            slog = New bc_cs_activity_log("bc_om_index_document", "index_pdf", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try
    End Function


End Class


<Serializable()> Public Class bc_om_index
    Inherits bc_cs_soap_base_class

    Public IndexTxid As Long
    Public IndexDoctype As String
    Public IndexDocid As Long
    Public Indexpageno As Long
    Public Indexdoctext As String

    Public write_mode As Integer = 0
    Public Const INSERT = 0
    Public Const UPDATE = 1
    Public Const DELETE = 2

    Public Sub New()


    End Sub


    Public Sub replace_bad()

        Indexdoctext = Indexdoctext.Replace("'", "''")

    End Sub


    Public Sub db_write(db As Object)
        Dim otrace As New bc_cs_activity_log("bc_om_index", "db_write", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

        Try

            Dim dbAction As New bc_om_at_index_db

            MyBase.certificate = certificate

            Select Case write_mode
                Case UPDATE

                    dbAction.write_action(db, Me.IndexTxid, Me.IndexDoctype, Me.IndexDocid, Indexpageno, Me.Indexdoctext, MyBase.certificate)

            End Select

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_om_index", "db_write", bc_cs_error_codes.USER_DEFINED, ex.Message, certificate)
        Finally
            otrace = New bc_cs_activity_log("bc_om_index", "db_write", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
        End Try

    End Sub

End Class


Public Class bc_om_at_index_db
    'Private gbc_db As New bc_cs_db_services
    REM write a tempate record and retuns Id
    Public Sub New()

    End Sub

    REM page
    Public Sub write_action(db As Object, ByVal txid As Long, ByVal doctype As String, ByVal docid As Long, ByVal pageno As Long, ByVal doctext As String, ByRef certificate As bc_cs_security.certificate)
        Dim sql As String

        sql = "bc_core_index_write_text " + CStr(txid) + ","
        sql = sql + "'" + CStr(doctype) + "',"
        sql = sql + CStr(docid) + ","
        sql = sql + CStr(pageno) + ","
        sql = sql + "'" + CStr(doctext) + "'"

        db.executesql_trans(sql, certificate)
 
    End Sub

End Class