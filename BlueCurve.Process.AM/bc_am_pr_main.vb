Imports System.Drawing
Imports DevExpress.XtraSplashScreen
Imports BlueCurve.Core.CS


Public Class bc_am_pr_main
    Public columnorder As New ArrayList
    Public columnwidth As New ArrayList

    Private ctrllr As bc_am_process
    Private initialised As Boolean

    'FILTER GROUPS
    Friend Const DOCUMENT_STATUS = "Document Status"
    Friend Const PUBLICATION_TYPE = "Publication Type"
    Friend Const ENTITY = "Entity"
    Friend Const STAGE = "Stage"
    Friend Const AUTHOR = "Author"
    Friend Const DIST = "Distribution Status"
    Friend Const DOC_LOCATION = "Location"

    Public Sub New()


        ' This call is required by the Windows Form Designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call.

        DevExpress.Skins.SkinManager.EnableFormSkins()
        DevExpress.UserSkins.BonusSkins.Register()
        DevExpress.LookAndFeel.UserLookAndFeel.Default.SetSkinStyle("Blue")

        initialised = True

    End Sub

    Friend WriteOnly Property Controller() As bc_am_process

        Set(ByVal Value As bc_am_process)
            ctrllr = Value
        End Set

    End Property
    Private Sub uxFilterDateFromCheckControl_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxFilterDateFromCheckControl.EditValueChanged

        ctrllr.mainFormRibbonAction(uxFilterDateFromCheck.Tag.ToString)
    End Sub

    Private Sub uxFilterDateFrom_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxFilterDateFrom.EditValueChanged
        ctrllr.mainFormRibbonAction(uxFilterDateFrom.Tag.ToString)
    End Sub

    Private Sub uxFilterDateToCheckControl_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxFilterDateToCheckControl.EditValueChanged
        ctrllr.mainFormRibbonAction(uxFilterDateToCheck.Tag.ToString)
    End Sub

    Private Sub uxFilterDateTo_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxFilterDateTo.EditValueChanged
        ctrllr.mainFormRibbonAction(uxFilterDateTo.Tag.ToString)
    End Sub

    Private Sub uxFileMenuExit_ItemClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.Ribbon.BackstageViewItemEventArgs) Handles uxFileMenuExit.ItemClick
        ctrllr.mainFormRibbonAction(uxFileMenuExit.Tag.ToString)
    End Sub

    Private Sub bc_am_pr_main_FormClosed(ByVal sender As Object, ByVal e As System.Windows.Forms.FormClosedEventArgs) Handles Me.FormClosed
        ctrllr.processExit()
    End Sub

    Private Sub uxRefresh_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxRefresh.ItemClick
        ctrllr.mainFormRibbonAction(uxRefresh.Tag.ToString)
    End Sub

    Private Sub uxShowDetails_CheckedChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxShowDetails.CheckedChanged
        Try
            initialised = False
            ctrllr.mainFormRibbonAction(uxShowDetails.Tag.ToString)
        Catch
            'mainFormRibbonAction handles any errors
        Finally
            initialised = True
        End Try
    End Sub


    Friend Sub filterAction(ByVal sender As DevExpress.XtraNavBar.NavBarItem, ByVal e As DevExpress.XtraNavBar.NavBarLinkEventArgs)

        Dim refresh As Boolean

        Try

            refresh = ctrllr.AutoRefresh
            ctrllr.AutoRefresh = True

            If Not e.Link.Item.Appearance.Font.Bold Then
                e.Link.Item.Appearance.Font = New Font(e.Link.Item.Appearance.Font, FontStyle.Bold)
            Else
                e.Link.Item.Appearance.Font = New Font(e.Link.Item.Appearance.Font, FontStyle.Regular)
            End If

            Select Case e.Link.Group.Caption
                Case DOCUMENT_STATUS
                    ctrllr.Mode = e.Link.Item.Tag



                Case PUBLICATION_TYPE
                    ctrllr.mainFormFilterAction(PUBLICATION_TYPE, e.Link.Item)

                Case ENTITY
                    ctrllr.mainFormFilterAction(ENTITY, e.Link.Item)

                Case STAGE
                    ctrllr.mainFormFilterAction(STAGE, e.Link.Item)

                Case AUTHOR
                    ctrllr.mainFormFilterAction(AUTHOR, e.Link.Item)
                Case DIST

                    ctrllr.mainFormFilterAction(DIST, e.Link.Item)
                Case DOC_LOCATION
                    ctrllr.clear_filters()
                    ctrllr.location = e.Link.Item.Tag

            End Select
            ctrllr.RetrieveDocs(True, True)
        Finally
            ctrllr.AutoRefresh = refresh
        End Try

    End Sub

    Private Sub uxSplitContainer_SplitGroupPanelCollapsed(ByVal sender As Object, ByVal e As DevExpress.XtraEditors.SplitGroupPanelCollapsedEventArgs) Handles uxSplitContainer.SplitGroupPanelCollapsed
        If initialised Then
            uxShowDetails.Checked = Not uxShowDetails.Checked
        End If
    End Sub

    Private Sub uxCreate_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxCreate.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxCreate.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default

    End Sub

    Private Sub uxViewDocument_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxViewDocument.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxViewDocument.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub


    Private Sub uxDocumentState_CheckedChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxDocumentState.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxDocumentState.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxForceCheckIn_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxForceCheckIn.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxForceCheckIn.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxCategoriseDocument_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxCategoriseDocument.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxCategoriseDocument.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxImportSupportDocument_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxImportSupportDocument.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxImportSupportDocument.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxReattachMasterDocument_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxReattachMasterDocument.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxReattachMasterDocument.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxImportMasterDocument_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxImportMasterDocument.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxImportMasterDocument.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxImportRegisteredDocument_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxImportRegisteredDocument.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxImportRegisteredDocument.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxRegisterMasterDocument_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxRegisterMasterDocument.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxRegisterMasterDocument.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub


    Private Sub uxSettings_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxSettings.ItemClick
        ctrllr.mainFormRibbonAction(uxSettings.Tag.ToString)
    End Sub

    Private Sub bc_am_pr_main_Leave(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Leave

    End Sub

    Private Sub bc_am_pr_main_SizeChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.SizeChanged
        If Not IsNothing(ctrllr) AndAlso Me.WindowState <> Windows.Forms.FormWindowState.Minimized AndAlso ctrllr.refreshsizechanged = True Then
            ctrllr.RetrieveDocs()
        End If
    End Sub
    Private Sub bc_am_pr_main_reSize(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Resize
        If Not IsNothing(ctrllr) AndAlso Me.WindowState <> Windows.Forms.FormWindowState.Minimized AndAlso ctrllr.refreshsizechanged = True Then
            ctrllr.RetrieveDocs()
        End If
    End Sub

    Private Sub Tpolltmp_EditValueChanged(ByVal sender As System.Object, ByVal e As System.EventArgs)
        ctrllr.RetrieveDocs()
    End Sub


    Private Sub uxIncludePublishedcontrol_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles uxIncludePublishedControl.EditValueChanged

        ctrllr.mainFormRibbonAction(uxIncludePublished.Tag.ToString)
    End Sub
    Private Sub uxpublishonlycontrol_EditValueChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles RepositoryItemCheckEdit1.EditValueChanged

        ctrllr.mainFormRibbonAction(uxpublishonly.Tag.ToString)
    End Sub

    Private Sub uxalert_AlertClick(ByVal sender As Object, ByVal e As DevExpress.XtraBars.Alerter.AlertClickEventArgs) Handles uxalert.AlertClick
        ctrllr.alert_clicked()
        e.AlertForm.StopAnimation()
        e.AlertForm.Close()
    End Sub
    Private Sub uxresetfilters_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxresetfilters.ItemClick
        ctrllr.mainFormRibbonAction(uxresetfilters.Tag.ToString)
    End Sub

    Private Sub uxShowFilters_CheckedChanged(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxShowFilters.CheckedChanged
        ctrllr.mainFormRibbonAction(uxShowFilters.Tag.ToString)


    End Sub





    Private Sub bc_am_pr_main_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        'If bc_cs_central_settings.process_caption = "" Then
        '    uxFileMenuPanel.Image = Nothing
        'End If

    End Sub

    Private Sub BarButtonItem1_ItemClick(ByVal sender As System.Object, ByVal e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxcomponents.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxcomponents.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxFilterWindow_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxFilterWindow.Click

    End Sub

    Private Sub BarButtonItem2_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxhtmlpreview.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor

        ctrllr.mainFormRibbonAction(uxhtmlpreview.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub



    Private Sub uxDocumentListPanel_Paint(sender As Object, e As Windows.Forms.PaintEventArgs) Handles uxDocumentListPanel.Paint


    End Sub

    Private Sub uxFileMenuPanel_Click(sender As Object, e As EventArgs) Handles uxFileMenuPanel.Click

    End Sub

    Private Sub uxDocumentState_CheckedChanged_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxDocumentState.CheckedChanged
        Me.uxDocumentState.Checked = False


    End Sub

    Private Sub BarButtonItem2_ItemClick_1(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxdistribution.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxdistribution.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default

    End Sub




    Private Sub uxFilterDateFromCheck_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxFilterDateFromCheck.ItemClick

    End Sub



    Private Sub uxdiststatus_CalcGroupClientHeight(sender As Object, e As DevExpress.XtraNavBar.NavBarCalcGroupClientHeightEventArgs) Handles uxdiststatus.CalcGroupClientHeight

    End Sub

    Private Sub uxregular_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxregular.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxregular.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxattestation_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxattestation.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxattestation.tag.tostring)
        Me.Cursor = Windows.Forms.Cursors.Default



    End Sub

    Private Sub uxstop_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxstop.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxstop.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxrejectdoc_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxrejectdoc.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxrejectdoc.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxviewattestation_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxviewattestation.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxviewattestation.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxattributes_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxattributes.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction(uxattributes.Tag.ToString)
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxRole_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxRole.ItemClick

    End Sub

    Private Sub BarButtonItem3_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles BarButtonItem3.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction("VIEW")
        Me.Cursor = Windows.Forms.Cursors.Default

    End Sub

    Private Sub BarButtonItem5_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxdisclosurefile.ItemClick
        Me.Cursor = Windows.Forms.Cursors.WaitCursor
        ctrllr.mainFormRibbonAction("DISCLOSURE_FILE")
        Me.Cursor = Windows.Forms.Cursors.Default
    End Sub

    Private Sub uxpublishonly_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxpublishonly.ItemClick

    End Sub

    Private Sub uxdiststatus_ItemChanged(sender As Object, e As EventArgs) Handles uxdiststatus.ItemChanged

    End Sub

    Private Sub uxIncludePublished_ItemClick(sender As Object, e As DevExpress.XtraBars.ItemClickEventArgs) Handles uxIncludePublished.ItemClick

    End Sub
End Class