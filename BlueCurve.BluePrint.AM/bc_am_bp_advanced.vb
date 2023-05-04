Imports BlueCurve.Core.AS
Imports System.Windows.Forms

Public Class bc_am_bp_advanced
    Inherits System.Windows.Forms.Form

    Private ctrllr As bc_am_blueprint

    Private m_SortingColumn As ColumnHeader

    Friend WriteOnly Property Controller() As bc_am_blueprint

        Set(ByVal Value As bc_am_blueprint)
            ctrllr = Value
        End Set

    End Property

    Private Sub uxAdd_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAdd.Click

        ctrllr.AddTempOrDbBookmark()

    End Sub

    Private Sub uxDelete_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDelete.Click

        ctrllr.DeleteTempOrDbBookmark()

    End Sub

    Private Sub uxDbDefinitionList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxDbDefinitionList.SelectedIndexChanged

        ctrllr.ValidateDefinitionList(uxDbDefinitionList)

    End Sub

    Private Sub uxTempDefinitionList_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxTempDefinitionList.SelectedIndexChanged

        ctrllr.ValidateDefinitionList(uxTempDefinitionList)

    End Sub

End Class
