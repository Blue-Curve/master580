Imports System
Imports System.Collections
Imports System.ComponentModel
Imports System.Drawing
Imports System.Data
Imports System.Windows.Forms

Public Class ComboBoxListView
    Inherits System.Windows.Forms.ListView

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'UserControl1 overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()

    End Sub

#End Region

    Private Const WM_HSCROLL As Integer = &H114
    Private Const WM_VSCROLL As Integer = &H115
    Private Const WM_MOUSEWHEEL As Integer = &H20A
    Private Const WM_MOUSEHWHEEL As Integer = &H20E
    Private Const LVM_FIRST = &H1000
    Private Const LVM_GETHEADER = (LVM_FIRST + 31)


    Private _combo As ComboBox

    Public Property combo() As ComboBox
        Get
            combo = _combo
        End Get
        Set(ByVal Value As ComboBox)
            _combo = Value
        End Set
    End Property


    Protected Overrides Sub WndProc(ByRef msg As Message)
        ' Look for the WM_VSCROLL or the WM_HSCROLL messages.
        Try
            Select Case msg.Msg
                Case WM_VSCROLL, WM_HSCROLL, WM_MOUSEWHEEL, WM_MOUSEHWHEEL, LVM_GETHEADER
                    _combo.Visible = False
            End Select
        Catch

        End Try

        ' Pass message to default handler.
        MyBase.WndProc(msg)
    End Sub

End Class

