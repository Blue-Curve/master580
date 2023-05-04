Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS
Imports DevExpress.XtraTreeList
Imports System.Drawing
Imports System.Windows.Forms


Public Class bc_dx_clone_document
    Implements Ibc_dx_clone_document
    Event clone(doc_id As Long) Implements Ibc_dx_clone_document.clone

    Public Sub load_list(title As String, documents As ArrayList) Implements Ibc_dx_clone_document.load_list

        Try
            Me.ltitle.Text = title
            Dim n As DevExpress.XtraTreeList.Nodes.TreeListNode
            Dim tln As Nodes.TreeListNode = Nothing
            Dim bcs As New bc_cs_icon_services
            Dim image As Bitmap
            Dim i As Integer
            For i = 4 To Me.uxDLImageList.Images.Count - 1
                Me.uxDLImageList.Images.RemoveAt(4)
            Next
            uxdocs.BeginUpdate()
            Try
                uxdocs.ClearNodes()
            Catch
            End Try
            For i = 0 To documents.Count - 1
                With documents(i)
                    n = uxdocs.AppendNode(New Object() {.title}, tln)
                    n.Tag = CStr(.id)
                    n.SetValue(1, .doc_date.tolocaltime)

                    image = Nothing
                    image = bcs.get_icon_for_file_type(".docx")
                    If Not IsNothing(image) Then
                        Me.uxDLImageList.Images.Add(image)
                        n.StateImageIndex = Me.uxDLImageList.Images.Count - 1
                    Else
                        n.StateImageIndex = 0
                    End If
                End With
            Next


        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_clone_document", "load_list", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            uxdocs.EndUpdate()
            bc_cs_central_settings.progress_bar.hide()
        End Try

    End Sub

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub bok_Click(sender As Object, e As EventArgs) Handles bok.Click
        Me.Hide()
        RaiseEvent clone(Me.uxdocs.Selection.Item(0).Tag)
    End Sub
End Class
Public Class Cbc_dx_clone_document
    WithEvents _view As Ibc_dx_clone_document
    Public pub_type_Id As Long
    Public entity1_id As Long
    Public entity2_id As Long
    Public cancel As Boolean = True
    Public master_doc_id As Long = 0
    Public sub_doc_id As Long = 0
    Dim oclone As bc_om_clone_document_list
    Public Sub New(view As Ibc_dx_clone_document, lpub_type_Id As Long, lentity1_id As Long, lentity2_id As Long)
        _view = view
        pub_type_Id = lpub_type_Id
        entity1_id = lentity1_id
        entity2_id = lentity2_id
    End Sub
    Public Function load_data() As Boolean
        Try
            Dim title As String
            load_data = False


            oclone = New bc_om_clone_document_list(pub_type_Id, entity1_id, entity2_id)
            bc_cs_central_settings.progress_bar = New bc_cs_progress("Blue Curve", "Retrieving Document List", 0, False, True)

            If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                oclone.db_read()
            Else
                oclone.tmode = bc_cs_soap_base_class.tREAD
                If oclone.transmit_to_server_and_receive(oclone, True) = False Then
                    Exit Function
                End If
            End If

            If oclone.documents.document.Count = 0 Then
                Dim omsg As New bc_cs_message("Blue Curve", "There are no previous documents for this combination. Please create a new document.", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                load_data = False
                Exit Function
            End If



            Dim pt_name As String = ""
            Dim ent_name As String = ""
            Dim sub_ent_name As String = ""
            For i = 0 To bc_am_load_objects.obc_pub_types.pubtype.Count - 1
                If bc_am_load_objects.obc_pub_types.pubtype(i).id = pub_type_Id Then
                    pt_name = bc_am_load_objects.obc_pub_types.pubtype(i).name
                    Exit For
                End If
            Next
            If entity1_id <> 0 Then
                For i = 0 To bc_am_load_objects.obc_entities.entity.Count - 1
                    If bc_am_load_objects.obc_entities.entity(i).id = entity1_id Then
                        ent_name = bc_am_load_objects.obc_entities.entity(i).name
                        If entity2_id = 0 Then
                            Exit For
                        End If
                    End If
                    If entity2_id <> 0 Then
                        If bc_am_load_objects.obc_entities.entity(i).id = entity2_id Then
                            sub_ent_name = bc_am_load_objects.obc_entities.entity(i).name
                        End If
                    End If
                Next
            End If
            If entity1_id = 0 Then
                title = "Please select a report of type: " + pt_name + " to clone from"
            ElseIf entity2_id = 0 Then
                title = "Please select a report of type: " + pt_name + " for: " + ent_name + " to clone from"
            Else
                title = "Please select a report of type: " + pt_name + " for: " + ent_name + " and " + sub_ent_name + " to clone from"
            End If
            _view.load_list(title, oclone.documents.document)
            load_data = True

        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_clone_document", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            bc_cs_central_settings.progress_bar.unload()
        End Try

    End Function

    Sub clone(doc_Id As Long) Handles _view.clone
        master_doc_id = doc_Id
        For i = 0 To oclone.documents.document.Count - 1
            If oclone.documents.document(i).id = doc_Id Then
                sub_doc_id = oclone.documents.document(i).pub_Type_Id
                Exit For
            End If
        Next

        cancel = False
    End Sub
End Class
Public Interface Ibc_dx_clone_document
    Event clone(doc_id As Long)
    Sub load_list(title As String, documents As ArrayList)
End Interface

Public Class bc_am_clone_document
    Public Function invoke_clone(ByVal pt_id As Long, ByVal entity_id As Long, ByVal sub_entity_id As Long) As Boolean
        Dim otrace As New bc_cs_activity_log("bc_am_at_wizard_main", "invoke_clone", bc_cs_activity_codes.TRACE_ENTRY, "")
        invoke_clone = False
        Try
            Dim vcd As New bc_dx_clone_document
            Dim ccd As New Cbc_dx_clone_document(vcd, pt_id, entity_id, sub_entity_id)
            If ccd.load_data = False Then
                Exit Function
            End If

            vcd.ShowDialog()
            If ccd.cancel = True Then
                Exit Function
            End If
            Dim ocopydoc As New bc_am_copy_document(ccd.master_doc_id, ccd.sub_doc_id)
            If ocopydoc.copy_doc = True Then
                invoke_clone = True
            End If

           
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_at_wizard_main", "invoke_clone", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            otrace = New bc_cs_activity_log("bc_am_at_wizard_main", "invoke_clone", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

End Class



