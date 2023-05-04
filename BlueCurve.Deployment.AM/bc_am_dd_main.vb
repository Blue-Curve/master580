Imports BlueCurve.Core.AS
Imports BlueCurve.Core.CS
Imports System.Windows.Forms
Public Class bc_am_dd_main

    Private badController As bc_am_deployment

    Friend Event ConfigurationButtonPressed()
    Friend Event AboutButtonPressed()

    Friend ReadOnly Property TreeView() As bc_am_dd_tree
        Get
            Return uxObjectTreeView
        End Get
    End Property

    Friend Property Controller() As bc_am_deployment
        Get
            Return badController
        End Get
        Set(ByVal badController As bc_am_deployment)
            Me.badController = badController
        End Set
    End Property

    Friend ReadOnly Property ExclusionFile() As String
        Get
            Return badupExclusionFile.Uri
        End Get
    End Property

    Friend ReadOnly Property DifferenceFile() As String
        Get
            Return badupDifferenceFile.Uri
        End Get
    End Property

    Private Sub uxToolBarMain_ButtonClick(ByVal sender As System.Object, ByVal e As System.Windows.Forms.ToolBarButtonClickEventArgs) Handles uxToolBarMain.ButtonClick
        badController.ToolBarButtonPressed(e)
    End Sub

    Friend Function ShowMessageBox(ByRef strMessage As String, ByVal m As MessageBoxButtons, ByVal i As MessageBoxIcon) As DialogResult
        Return MessageBox.Show(strMessage, "Blue Curve Deployment", m, MessageBoxIcon.Information)
    End Function

    Private Sub badupExportPath_UriSelected() Handles badupExportPath.UriSelected
        Controller.SourcePath = badupExportPath.Uri
    End Sub

    Private Sub badupImportPath_UriSelected() Handles badupImportPath.UriSelected
        Controller.DestinationPath = badupImportPath.Uri
    End Sub
    
    Private Sub uxExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxExit.Click
        uxExit.Checked = Not uxExit.Checked
        Controller.SetDatabaseStatus(uxExit.Checked)
    End Sub

    Private Sub miConfiguration_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        RaiseEvent ConfigurationButtonPressed()
    End Sub

    Private Sub cbCheckDependencies_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbCheckDependencies.CheckedChanged
        Controller.CheckDependencies = cbCheckDependencies.Checked
    End Sub

    Private Sub cbValidateOnly_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbValidateOnly.CheckedChanged
        Controller.ValidateOnly = cbValidateOnly.Checked
    End Sub

    Private Sub MenuItem_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miCompare.Click, miExport.Click, miImport.Click, miLock.Click, miUnlock.Click
        Controller.SaveOperationFile(CType(sender, MenuItem).Text)
    End Sub

    Private Sub badupDifferenceFile_UriSelected() Handles badupDifferenceFile.UriSelected
        Controller.InstructionsFile = badupDifferenceFile.Uri
    End Sub

    Private Sub badupExclusionFile_UriSelected() Handles badupExclusionFile.UriSelected
        Controller.ExclusionsFile = badupExclusionFile.Uri
    End Sub

    Private Sub txtArchiveExtension_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles txtArchiveExtension.TextChanged
        Controller.ArchiveExtension = txtArchiveExtension.Text
    End Sub

    Private Sub cbArchive_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbArchiveSource.CheckedChanged
        Controller.SourceArchived = cbArchiveSource.Checked
    End Sub

    Private Sub cbUpdateDestination_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbUpdateDestination.CheckedChanged
        Controller.UpdateDestinationFiles = cbUpdateDestination.Checked
    End Sub

    Private Sub uxAbout_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles uxAbout.Click
        RaiseEvent AboutButtonPressed()
    End Sub

    Private Sub cbOverwriteAll_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbOverwriteAll.CheckedChanged
        Controller.OverwriteAll = cbOverwriteAll.Checked
    End Sub

    Private Sub badupError_UriSelected() Handles badupError.UriSelected
        Controller.ErrorFile = badupError.Uri
    End Sub

    Private Sub cbIncludeDependencies_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbIncludeDependencies.CheckedChanged
        Controller.IncludeDependencies = cbIncludeDependencies.Checked
    End Sub
   
    Private Sub miExit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles miExit.Click
        Me.Close()
    End Sub
End Class