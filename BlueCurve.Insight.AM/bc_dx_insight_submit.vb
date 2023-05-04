Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports BlueCurve.Core.AS

Public Class bc_dx_insight_submit
    Implements Ibc_dx_insight_submit
    Dim lsec As bc_om_in_submission_security

    Event validate_only() Implements Ibc_dx_insight_submit.validate_only
    Event submit() Implements Ibc_dx_insight_submit.submit
    Public Sub New()

        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")
        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

    End Sub
    Public Function load_view(osec As bc_om_in_submission_security) As Boolean Implements Ibc_dx_insight_submit.load_view
        load_view = False
        Try
            lsec = osec
            If osec.approval_type = 2 Then
                If osec.proxy_user_ids.Count > 1 Then
                    lauthor.Text = "Choose Analyst"
                    For i = 0 To osec.proxy_user_names.Count - 1
                        uxanalyst.Properties.Items.Add(osec.proxy_user_names(i))
                    Next
                    uxanalyst.Enabled = True
                    Me.bok.Enabled = False
                Else
                    lauthor.Text = "Analyst"
                    uxanalyst.Enabled = False
                    uxanalyst.Properties.Items.Add(osec.proxy_user_names(0))
                    uxanalyst.SelectedIndex = 0
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.author_id = osec.proxy_user_ids(0)
                    bc_am_load_objects.obc_om_insight_contribution_for_entity.author_name = osec.proxy_user_names(0)
                End If
            Else
                uxanalyst.Enabled = False
                lauthor.Text = "Analyst"
                uxanalyst.Properties.Items.Add(bc_am_load_objects.obc_om_insight_contribution_for_entity.author_name)
                uxanalyst.SelectedIndex = 0
                bc_am_load_objects.obc_om_insight_contribution_for_entity.author_id = bc_cs_central_settings.logged_on_user_id
            End If
            If bc_am_load_objects.obc_prefs.financial_workflow_stages.Count = 0 Then
                Me.uxstage.Properties.Items.Add("Draft")
                Me.uxstage.Properties.Items.Add("Publish")
                Me.uxstage.SelectedIndex = 0
            Else
                For i = 0 To bc_am_load_objects.obc_prefs.financial_workflow_stages.Count - 1
                    If bc_am_load_objects.obc_prefs.financial_workflow_stages(i) = 1 Then
                        Me.uxstage.Properties.Items.Add("Draft")
                    End If
                    If bc_am_load_objects.obc_prefs.financial_workflow_stages(i) = 8 Then
                        Me.uxstage.Properties.Items.Add("Publish")
                    End If
                Next
                Me.uxstage.SelectedIndex = 0
                If bc_am_load_objects.obc_prefs.default_financial_workflow_stage = 0 And Me.uxstage.Properties.Items.Count = 2 Then
                    Me.uxstage.SelectedIndex = 1
                End If
                If Me.uxstage.Properties.Items.Count = 1 Then
                    Me.uxstage.SelectedIndex = 0
                    Me.uxstage.Enabled = False
                End If
            End If
            load_view = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_dx_insight_submit", "load_view", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try

    End Function

    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub bok_Click(sender As Object, e As EventArgs) Handles bok.Click
        If Me.uxstage.Text = "Publish" Then
            Dim ocommentary As New bc_cs_activity_log("bc_am_in_main_load", "Btnnext_Click", bc_cs_activity_codes.COMMENTARY, "Financial Workflow Stage set to 8")
            bc_am_load_objects.obc_om_insight_contribution_for_entity.workflow_stage = 8
        Else
            Dim ocommentary As New bc_cs_activity_log("bc_am_in_main_load", "Btnnext_Click", bc_cs_activity_codes.COMMENTARY, "Financial Workflow Stage set to 1")
            bc_am_load_objects.obc_om_insight_contribution_for_entity.workflow_stage = 1
        End If
            If lsec.approval_type = 2 And lsec.proxy_user_ids.Count > 1 Then
            bc_am_load_objects.obc_om_insight_contribution_for_entity.author_id = lsec.proxy_user_ids(uxanalyst.SelectedIndex)
            bc_am_load_objects.obc_om_insight_contribution_for_entity.author_name = lsec.proxy_user_names(uxanalyst.SelectedIndex)
        End If
        Me.Hide()
        RaiseEvent submit()



    End Sub

   
    Private Sub SimpleButton1_Click(sender As Object, e As EventArgs) Handles SimpleButton1.Click
        Me.Hide()
        RaiseEvent validate_only()
    End Sub

    Private Sub uxanalyst_SelectedIndexChanged(sender As Object, e As EventArgs) Handles uxanalyst.SelectedIndexChanged
        If Me.uxanalyst.SelectedIndex > -1 Then
            Me.bok.Enabled = True

        End If
    End Sub
End Class
Public Class Cbc_dx_insight_submit
    Public validate_only As Boolean = False
    Public submit As Boolean = False
    WithEvents _view As Ibc_dx_insight_submit
    Public Function load_data(view As Ibc_dx_insight_submit, osec As bc_om_in_submission_security) As Boolean
        load_data = False
        Try
            _view = view
            Return _view.load_view(osec)

            load_data = True
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("Cbc_dx_insight_submit", "load_data", bc_cs_error_codes.USER_DEFINED, ex.Message)
        End Try
    End Function
    Public Sub svalidate_only() Handles _view.validate_only
        validate_only = True
    End Sub
    Public Sub ssubmit() Handles _view.submit
        submit = True
    End Sub
End Class
Public Interface Ibc_dx_insight_submit
    Function load_view(lsec As bc_om_in_submission_security) As Boolean
    Event validate_only()
    Event submit()
End Interface
