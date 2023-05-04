Imports System.Windows.Forms

Module ColumnExtenstion
    <System.Runtime.CompilerServices.Extension()> _
    <System.Diagnostics.DebuggerStepThrough()> _
    Function IsTimeCell(ByVal GridView As DataGridView, ByVal e As DataGridViewCellEventArgs) As Boolean
        Return TypeOf GridView.Columns(e.ColumnIndex) Is TimeColumn AndAlso Not e.RowIndex = -1
    End Function
End Module
