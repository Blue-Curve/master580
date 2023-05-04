
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Windows.Forms
Public Class bc_am_translation
    Public view As bc_am_cp_translation
    Public container As bc_am_cp_container

    Public Sub New(ByVal container As bc_am_cp_container, Optional ByVal view As bc_am_cp_translation = Nothing)
        If Not view Is Nothing Then
            view.controller = Me
            Me.view = view
        End If
        If Not container Is Nothing Then
            Me.container = container
        End If

        Me.load_all()
    End Sub

    Public Sub load_all()

        view.LoadTranslationGroups()
        view.Refresh()

    End Sub

    Public Sub commit_changes()

        Dim oTranslate As bc_om_translation_item

        Try
            view.Cursor = Cursors.WaitCursor
            For i = 0 To view.TranslationItems.TranslationItems.Count - 1
                If view.TranslationItems.TranslationItems(i).itemchanged = True Then

                    oTranslate = view.TranslationItems.TranslationItems(i)

                    oTranslate.WriteMode = bc_om_translation_item.UPDATE
                    If bc_cs_central_settings.selected_conn_method = bc_cs_central_settings.ADO Then
                        oTranslate.db_write()
                    Else
                        oTranslate.tmode = bc_om_user_role.tWRITE
                        If oTranslate.transmit_to_server_and_receive(oTranslate, True) = False Then
                            Exit Sub
                        End If

                    End If
                    view.TranslationItems.TranslationItems(i).itemchanged = False
                End If
            Next
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_translation", "commit_changes", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

            container.uncomitted_data = False
            view.Cursor = Cursors.Default
        End Try

    End Sub

End Class
