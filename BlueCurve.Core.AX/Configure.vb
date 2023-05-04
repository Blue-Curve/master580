Option Strict On
Imports System
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Collections
Imports Microsoft.Win32
Imports Microsoft.VisualBasic
Imports System.Threading
Imports System.Diagnostics
Imports System.Data.sqlclient
Imports BlueCurve.Core.CS
Public Class Configure
    'Private strUser As String = Environment.CommandLine
    Private strTemplatesPath As String = ""
    Private strAppPath As String = ""
    Private bc_cs_central_settings As bc_cs_central_settings

    Private Property TemplatePath() As String
        Get
            Return strTemplatesPath
        End Get
        Set(ByVal Value As String)
            strTemplatesPath = Value
        End Set
    End Property
    Private Property ApplicationDirectory() As String
        Get
            Return strAppPath
        End Get
        Set(ByVal Value As String)
            strAppPath = Value
        End Set
    End Property
    Public Function ModifyINIFile(ByVal path As String) As Boolean

        Dim blnCopyBCConfig As Boolean = False
        Dim blnResult As Boolean = False
        Dim winroot As String
        Dim root As String
        Dim strBCFolder As String
        Dim strBCTemplatesFolder As String
        Try
            'set the application directory path
            winroot = System.Environment.GetEnvironmentVariable("SystemRoot")
            root = winroot.Substring(0, 3)
            Me.ApplicationDirectory = root & "Program Files\Blue Curve\Author Tool"
            'create folder structure
            Me.CreateFolderStructure()
            'now copy the bc config file
            strBCFolder = Me.GetUserProfileAppKeyValue & "\bluecurve"
            blnCopyBCConfig = Me.CopyBCCONFIGFile(Me.ApplicationDirectory, strBCFolder)
            'now instantiate central settings file
            bc_cs_central_settings = New bc_cs_central_settings(True)
            'now call main modify function
            blnResult = Me.Modify(path)
            'now create template folder
            strBCTemplatesFolder = Me.TemplatePath
            Me.CreateFolder(strBCTemplatesFolder)
        Catch ex As Exception
            Dim slog As New bc_cs_activity_log("blucurve.application.ax.Configure :error in ", "ModifyINIFile .", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
            blnResult = False
            blnCopyBCConfig = False
        Finally
            If blnCopyBCConfig AndAlso blnResult Then
                ModifyINIFile = True
            Else
                ModifyINIFile = False
            End If
        End Try
    End Function
    Public Function Modify(ByVal path As String) As Boolean
        Dim blnReplaceConfigValues As Boolean = False
        Dim blnFolderStructure As Boolean = False
        Dim blnSetRegistry As Boolean = False

        Try
            blnReplaceConfigValues = Me.ReplaceConfigValues(path)
            'now craete folder structure
            blnFolderStructure = Me.CopyAllFiles()
            'now execute the registry file only if the so far execution is true
            If blnReplaceConfigValues AndAlso blnFolderStructure Then
                blnSetRegistry = Me.RunRegistryFile()
            End If
        Catch ex As Exception
            Dim slog As New bc_cs_activity_log("blucurve.application.ax.Configure :error in ", "ModifyINIFile .", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
            blnReplaceConfigValues = False
            blnFolderStructure = False
            blnSetRegistry = False
        Finally
            If blnReplaceConfigValues AndAlso blnFolderStructure AndAlso blnSetRegistry Then
                Modify = True
            Else
                Modify = False
            End If
        End Try
    End Function

    Public Sub CreateFolderStructure()
        Dim strBCFolder As String
        Dim strBCLocalDBFolder As String
        Dim strBCLocalDocumentsFolder As String
        Dim strBCTemplatesFolder As String
        Try
            'first create bluecurve folder
            strBCFolder = Me.GetUserProfileAppKeyValue & "\bluecurve"
            Me.CreateFolder(strBCFolder)
            strBCLocalDocumentsFolder = Me.GetUserProfileAppKeyValue & "\bluecurve\Local Documents"
            Me.CreateFolder(strBCLocalDocumentsFolder)
            strBCLocalDBFolder = Me.GetUserProfileAppKeyValue & "\bluecurve\Local Database"
            Me.CreateFolder(strBCLocalDBFolder)
            strBCTemplatesFolder = Me.TemplatePath
            Me.CreateFolder(strBCTemplatesFolder)
        Catch ex As Exception
            Dim slog As New bc_cs_activity_log("blucurve.application.ax.Configure :error in ", "CreateFolderStructureandCopy .", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
        Finally
        End Try
    End Sub
    Public Function CopyAllFiles() As Boolean
        Dim finalresult As Boolean = False
        Dim blnBCFolder As Boolean = False
        Dim blnBCLocalDBFolder As Boolean = False
        Dim blnBCLocalDocumentsFolder As Boolean = False
        Dim blnBCTemplatesFolder As Boolean = False
        Dim blnCopyDB As Boolean = False
        Dim blnCopyBCConfig As Boolean = False
        Dim strBCFolder As String
        Dim strBCLocalDBFolder As String
        Dim strBCLocalDocumentsFolder As String

        Try
            strBCFolder = Me.GetUserProfileAppKeyValue & "\bluecurve"
            strBCLocalDocumentsFolder = Me.GetUserProfileAppKeyValue & "\bluecurve\Local Documents"
            strBCLocalDBFolder = Me.GetUserProfileAppKeyValue & "\bluecurve\Local Database"
            'copy the local database into new directory
            blnCopyDB = Me.CopyLocalDb(Me.ApplicationDirectory, strBCLocalDBFolder)
            'copy the bc configuration file into new directory
            blnCopyBCConfig = Me.ReplaceBCCONFIGFileValues(Me.ApplicationDirectory, strBCFolder)
        Catch ex As Exception
            Dim slog As New bc_cs_activity_log("blucurve.application.ax.Configure :error in ", "CopyAllFiles .", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
        Finally
            'if all successfull then send true otherwise false
            If blnCopyDB AndAlso blnCopyBCConfig Then
                CopyAllFiles = True
            Else
                CopyAllFiles = False
            End If
        End Try
    End Function
    Public Function CreateFolder(ByVal strPath As String) As Boolean
        Dim objDir As New DirectoryInfo(strPath)
        Dim blnCreated As Boolean = False
        Try
            If Not objDir.Exists Then
                objDir.Create()
            End If
            blnCreated = True
        Catch ex As Exception
            Dim slog As New bc_cs_activity_log("blucurve.application.ax.Configure :error in ", "CreateFolder .", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
            blnCreated = False
        Finally
            CreateFolder = blnCreated
        End Try
    End Function
    Public Function ReplaceConfigValues(ByVal path As String) As Boolean
        Dim result As Boolean = False
        Dim line As String
        Try
            Dim strContent As String = ""
            Dim intFirstTime As Integer = 0
            Dim sr As StreamReader = New StreamReader(path)
            Dim sw As StreamWriter
            Dim strTemplates As String
            Dim strLocalDocuments As String
            Dim strLocalDB As String
            ' Read and display the lines from the file until the end 
            ' of the file is reached.
            If Not sr Is Nothing Then
                Do
                    line = sr.ReadLine()
                    If Not line Is Nothing AndAlso line <> "" Then
                        If line.ToLower.Trim = "#shared#\templates\" Then
                            strTemplates = Me.GetUserProfileAppKeyValue & "\Microsoft\Templates\Blue Curve\"
                            'set the templates property
                            Me.TemplatePath = strTemplates
                            'replace this with predefined one
                            strContent = strContent & Environment.NewLine & strTemplates
                        ElseIf line.ToLower.Trim = "#private#\templates\" Then
                            strTemplates = Me.GetUserProfileAppKeyValue & "\bluecurve\Templates\"
                            'set the templates property
                            Me.TemplatePath = strTemplates
                            'replace this with predefined one
                            strContent = strContent & Environment.NewLine & strTemplates
                        ElseIf line.ToLower.Trim = "#userprofile#\local documents\" Then
                            strLocalDocuments = Me.GetUserProfileAppKeyValue & "\bluecurve\Local Documents\"
                            strContent = strContent & Environment.NewLine & strLocalDocuments
                        ElseIf line.ToLower.Trim = "#userprofile#\local database\atadminmodule.mdb" Then
                            strLocalDB = Me.GetUserProfileAppKeyValue & "\bluecurve\Local Database\ATAdminModule.mdb"
                            strContent = strContent & Environment.NewLine & strLocalDB
                        Else
                            'because for the first time use content alone
                            If intFirstTime = 0 Then
                                strContent = line
                            Else
                                strContent = strContent & Environment.NewLine & line
                            End If
                        End If
                        intFirstTime = intFirstTime + 1
                    End If
                Loop Until line Is Nothing
            End If
            sr.Close()
            sw = New StreamWriter(Me.GetUserProfileAppKeyValue & "\bluecurve\single_author_tool.ini")
            sw.Write(strContent)
            sw.Close()
            result = True
        Catch ex As Exception
            Dim slog As New bc_cs_activity_log("blucurve.application.ax.Configure :error ", "ReplaceConfigValues", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
            result = False
        Finally
            ReplaceConfigValues = result
        End Try
    End Function
    Public Function GetUserProfileAppKeyValue() As String
        Dim strpath As String = ""
        Dim regcurrentuser As RegistryKey = Registry.CurrentUser
        Dim regappdata As RegistryKey
        Try
            Try
                regappdata = regcurrentuser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders")
                If Not regappdata Is Nothing Then
                    strpath = CStr(regappdata.GetValue("AppData"))
                Else
                    strpath = ""
                End If
            Catch
                regappdata = regcurrentuser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders")
                If Not regappdata Is Nothing Then
                    strpath = CStr(regappdata.GetValue("AppData"))
                Else
                    strpath = ""
                End If

            End Try
        Catch ex As Exception
            Dim slog As New bc_cs_activity_log("blucurve.application.ax.Configure :error in ", "GetUserProfileAppKeyValue .", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
            strpath = ""
        Finally
            GetUserProfileAppKeyValue = strpath
        End Try
    End Function
    Public Function GetAllUsersAppKeyValue() As String
        Dim strpath As String = ""
        Dim regcurrentuser As RegistryKey = Registry.LocalMachine
        Dim regappdata As RegistryKey
        Try
            Try
                regappdata = regcurrentuser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\User Shell Folders")
                If Not regappdata Is Nothing Then
                    strpath = CStr(regappdata.GetValue("Common AppData"))
                Else
                    strpath = ""
                End If
            Catch
                regappdata = regcurrentuser.OpenSubKey("Software\Microsoft\Windows\CurrentVersion\Explorer\Shell Folders")
                If Not regappdata Is Nothing Then
                    strpath = CStr(regappdata.GetValue("Common AppData"))
                Else
                    strpath = ""
                End If

            End Try
        Catch ex As Exception
            Dim slog As New bc_cs_activity_log("blucurve.application.ax.Configure :error in ", "GetAllUsersAppKeyValue .", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
            strpath = ""
        Finally
            GetAllUsersAppKeyValue = strpath
        End Try
    End Function
    Public Function RunRegistryFile() As Boolean
        Dim result As Boolean = False
        Try
            Dim proc As System.Diagnostics.Process
            Dim winroot As String
            winroot = System.Environment.GetEnvironmentVariable("SystemRoot")
            Dim strCmdLine As String
            strCmdLine = winroot & "\regedit.exe"
            Dim strCmdParam As String
            Dim strRegFileName As String
            strRegFileName = ControlChars.Quote & Me.ReadConfigRegKey() & "\Binary\setup.reg" & ControlChars.Quote
            strCmdParam = " /s /c " & strRegFileName
            'proc = System.Diagnostics.Process.Start(strCmdLine, strCmdParam)
            proc = System.Diagnostics.Process.Start(strCmdLine, strCmdParam)
            '  proc.Close()
            result = True
        Catch ex As Exception
            Dim slog As New bc_cs_activity_log("blucurve.application.ax.Configure :error in ", "RunRegistryFile .", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
            result = False
        Finally
            RunRegistryFile = result
        End Try
    End Function
    Public Function CopyLocalDb(ByVal sourceDir As String, ByVal destDir As String) As Boolean
        'just copy the mdb file 
        Dim result As Boolean = False
        Try
            ' Copy the file.
            System.IO.File.Copy(sourceDir & "\Local Database\ATAdminModule.mdb", destDir & "\ATAdminModule.mdb", True)
            result = True
        Catch ex As Exception
            Dim slog As New bc_cs_activity_log("blucurve.application.ax.Configure :error in ", "CopyLocalDb .", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
            result = False
        Finally
            CopyLocalDb = result
        End Try
    End Function
    Public Function ReplaceBCCONFIGFileValues(ByVal sourceDir As String, ByVal destDir As String) As Boolean
        'just copy the mdb file 
        Dim result As Boolean = False
        Try
            'after copying the bc config file replace the local template repos tag with appropriate value
            Dim xmlfileBCConfig As New System.Xml.XmlDocument
            xmlfileBCConfig.Load(destDir & "\bc_config.xml")
            Dim ndTemplate As System.Xml.XmlNode = xmlfileBCConfig.SelectSingleNode("/settings/templates/local_repos")
            ndTemplate.InnerText = Me.TemplatePath
            'save the file back to the original place.
            xmlfileBCConfig.Save(destDir & "\bc_config.xml")
            result = True
        Catch ex As Exception
            Dim slog As New bc_cs_activity_log("blucurve.application.ax.Configure :error in ", "CopyBCCONFIGFile .", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
            result = False
        Finally
            ReplaceBCCONFIGFileValues = result
        End Try
    End Function
    Public Function CopyBCCONFIGFile(ByVal sourceDir As String, ByVal destDir As String) As Boolean
        'just copy the mdb file 
        Dim result As Boolean = False
        Try
            ' Copy the file.
            System.IO.File.Copy(sourceDir & "\bc_config.xml", destDir & "\bc_config.xml", True)
            result = True
        Catch ex As Exception
            Dim slog As New bc_cs_activity_log("blucurve.application.ax.Configure :error in ", "CopyBCCONFIGFile .", CStr(bc_cs_activity_codes.COMMENTARY), ex.Message)
            result = False
        Finally
            CopyBCCONFIGFile = result
        End Try
    End Function
    Public Function ReadConfigRegKey() As String
        Dim strpath As String = ""
        Try
            Dim winroot As String
            Dim root As String
            ' strpath = Environment.GetEnvironmentVariable("BCConfigurationFilePath")
            'set the application directory path
            winroot = System.Environment.GetEnvironmentVariable("SystemRoot")
            root = winroot.Substring(0, 3)

            Dim regLocalMachine As RegistryKey
            regLocalMachine = Registry.LocalMachine.OpenSubKey("SOFTWARE\Blue Curve\BCConfigurationFile", False)
            If Not regLocalMachine Is Nothing Then
                strpath = CStr(regLocalMachine.GetValue("Path"))
            End If
            'if there is no path value then use default user profile's directory to locate config file
            If strpath Is Nothing Or strpath.Trim = "" Then
                strpath = root & "Program Files\Blue Curve\Author Tool"
            End If
        Catch ex As Exception
            strpath = ""
        Finally
            'MsgBox("aa: " + strpath)
            ReadConfigRegKey = strpath
        End Try
    End Function

End Class
