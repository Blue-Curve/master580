
Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Windows.Forms

Public Class bc_am_monitor

    Public view As bc_am_cp_monitor
    Public container As bc_am_cp_container

    Public Sub New(ByVal container As bc_am_cp_container, Optional ByVal view As bc_am_cp_monitor = Nothing)
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

        'Dim fm As New bc_am_cp_monitor
        'Dim cfsm As New Cbc_am_cp_monitor(fm)

        'If cfsm.load_data = True Then
        '    fm.ShowDialog()
        'End If


        'view.LoadTranslationGroups()
        'view.refresh()


    End Sub




End Class
