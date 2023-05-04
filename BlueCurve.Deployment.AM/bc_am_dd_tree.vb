Imports System.Windows.Forms
Imports System.Xml

Public Class bc_am_dd_tree
    Inherits TreeView

    Private mt As MaskType = MaskType.Blank
    Friend boolLoading As Boolean = False

    Friend Property MaskType() As MaskType
        Get
            Return mt
        End Get
        Set(ByVal mt As MaskType)
            Me.mt = mt
        End Set
    End Property

    Friend Property Loading() As Boolean
        Get
            Return boolLoading
        End Get
        Set(ByVal boolLoading As Boolean)
            Me.boolLoading = boolLoading
        End Set
    End Property

    ' End Properties
    Protected Overrides Sub OnAfterCheck(ByVal e As System.Windows.Forms.TreeViewEventArgs)
        MyBase.OnAfterCheck(e)
        If Not Loading Then
            Loading = True
            PropagateChecked(e.Node, e.Node.Checked)
            If TypeOf e.Node Is bc_am_dd_node AndAlso CType(e.Node, bc_am_dd_node).SelectAll AndAlso Not e.Node.Parent Is Nothing Then
                For Each n As TreeNode In e.Node.Parent.Nodes
                    n.Checked = e.Node.Checked
                Next
            End If
            Loading = False
        End If
    End Sub

    Friend Sub PropagateStatus(ByVal badn As bc_am_dd_node, ByVal boolStatus As Boolean, ByVal mt As MaskType)
        If mt = bc_am_dd_enums.MaskType.Include Then
            badn.Export = boolStatus
        ElseIf mt = bc_am_dd_enums.MaskType.Exclude Then
            badn.Exclude = boolStatus
        ElseIf mt = bc_am_dd_enums.MaskType.Archive Then
            badn.Archive = boolStatus
        End If
        For Each badnChild As bc_am_dd_node In badn.Nodes
            PropagateStatus(badnChild, boolStatus, mt)
        Next
    End Sub

    Private Sub PropagateChecked(ByVal tn As TreeNode, ByVal boolChecked As Boolean)
        tn.Checked = boolChecked
        For Each tnChild As TreeNode In tn.Nodes
            PropagateChecked(tnChild, boolChecked)
        Next
    End Sub

End Class
