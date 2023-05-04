Imports BlueCurve.core.cs
Imports System.IO
Imports System.Windows.Forms
'==========================================
' Bluecurve Limited 2008
' Module:         BluePrint
' Type:           AM
' Desciption:     configuration management
' Public Methods: Show
'  
' Version:        1.0
' Change history:
'==========================================
Public Class bc_am_configuration

    Private view As bc_am_bp_configuration
    Private configFileList As New ArrayList

    Friend Sub New(ByVal view As bc_am_bp_configuration)

        view.Controller = Me
        Me.view = view

    End Sub

    Friend Function Show() As Boolean

        Dim log = New bc_cs_activity_log("bc_am_configuration", "Show", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim currentSelectedIndex As Integer = -1

            getConfigFiles()

            Dim i As Integer

            For i = 0 To configFileList.Count - 1
                view.uxEnvironment.Items.Add(configFileList(i))
                If configFileList(i).Environment = bc_cs_central_settings.environment_name Then
                    view.uxEnvironment.SelectedIndex = i
                    If currentSelectedIndex = -1 Then
                        currentSelectedIndex = i
                    End If
                End If
            Next i

            If view.ShowDialog() = DialogResult.OK And currentSelectedIndex <> view.uxEnvironment.SelectedIndex Then
                'rename and reload config and then sync
                changeConfig()
                Show = True
            End If

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_configuration", "Show", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_configuration", "Show", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function

    Private Sub getConfigFiles()

        Dim log = New bc_cs_activity_log("bc_am_configuration", "getConfigFiles", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim configFile As String
            Dim configFiles() As String
            Dim userDir As String

            userDir = bc_cs_central_settings.get_user_dir()

            configFiles = Directory.GetFiles(String.Concat(userDir, "\"), "bc_config*")

            configFileList.Clear()

            For Each configFile In configFiles

                Dim config As New bc_config(configFile)
                If config.Valid Then
                    configFileList.Add(config)
                End If
            Next

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_configuration", "getConfigFiles", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_configuration", "getConfigFiles", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Friend Sub PopulateConfig()

        Dim log = New bc_cs_activity_log("bc_am_configuration", "PopulateConfig", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            view.uxConnectivityMethod.Text = configFileList(view.uxEnvironment.SelectedIndex).ConnectivityMethod
            view.uxServer.Text = configFileList(view.uxEnvironment.SelectedIndex).Server
            view.uxTemplateCentralRepos.Text = configFileList(view.uxEnvironment.SelectedIndex).TemplateCentralRepos
            view.uxTemplateLocalRepos.Text = configFileList(view.uxEnvironment.SelectedIndex).TemplateLocalRepos

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_configuration", "PopulateConfig", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_configuration", "PopulateConfig", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Sub changeConfig()

        Dim log = New bc_cs_activity_log("bc_am_configuration", "changeConfig", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim i As Integer

            For i = 0 To configFileList.Count - 1
                If configFileList(i).Environment = bc_cs_central_settings.environment_name Then
                    Dim fInfo As New FileInfo(configFileList(i).FileName)
                    fInfo.MoveTo(String.Concat(Left(configFileList(i).FileName, Len(configFileList(i).FileName) - 4), "_", configFileList(i).Environment, ".xml"))
                    Exit For
                End If
            Next i

            For i = 0 To configFileList.Count - 1
                If configFileList(i).Environment = configFileList(view.uxEnvironment.SelectedIndex).Environment Then
                    Dim fInfo As New FileInfo(configFileList(i).FileName)
                    fInfo.MoveTo(String.Concat(Left(configFileList(i).FileName, InStrRev(configFileList(i).FileName, "_") - 1), ".xml"))
                    Exit For
                End If
            Next i

        Catch ex As Exception
            Dim errLog As New bc_cs_error_log("bc_am_configuration", "changeConfig", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            log = New bc_cs_activity_log("bc_am_configuration", "changeConfig", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    Private Class bc_config

        Private _environment As String
        Private _connectivityMethod As String
        Private _server As String
        Private _templateLocalRepos As String
        Private _templateCentralRepos As String
        Private _fileName As String
        Private _valid As Boolean

        Public Sub New(ByVal configFile As String)

            Dim xmlload As New Xml.XmlDocument

            Try

                xmlload.Load(configFile)

                FileName = configFile

                Environment = xmlload.SelectSingleNode("/settings/environment").InnerXml()
                ConnectivityMethod = xmlload.SelectSingleNode("/settings/connectivity/method").InnerXml()

                If ConnectivityMethod = bc_cs_central_settings.ADO Then
                    Server = String.Concat(xmlload.SelectSingleNode("/settings/connectivity/ado/server").InnerXml(), " - ", _
                                            xmlload.SelectSingleNode("/settings//connectivity/ado/name").InnerXml())
                Else
                    Server = xmlload.SelectSingleNode("/settings/connectivity/soap/server").InnerXml()
                End If

                TemplateLocalRepos = xmlload.SelectSingleNode("/settings/templates/local_repos").InnerXml()
                TemplateCentralRepos = xmlload.SelectSingleNode("/settings/templates/central_repos").InnerXml()

                _valid = True

            Catch ex As Exception
                Dim errLog As New bc_cs_error_log("bc_am_configuration", "Configuration file " & FileName & " is not valid.  Please contact support.", bc_cs_error_codes.USER_DEFINED, "")
            End Try

        End Sub

        Public Property Environment() As String
            Get
                Environment = _environment
            End Get
            Set(ByVal Value As String)
                _environment = Value
            End Set
        End Property

        Public Property ConnectivityMethod() As String
            Get
                ConnectivityMethod = _connectivityMethod
            End Get
            Set(ByVal Value As String)
                _connectivityMethod = Value
            End Set
        End Property

        Public Property Server() As String
            Get
                Server = _server
            End Get
            Set(ByVal Value As String)
                _server = Value
            End Set
        End Property

        Public Property TemplateCentralRepos() As String
            Get
                TemplateCentralRepos = _templateCentralRepos
            End Get
            Set(ByVal Value As String)
                _templateCentralRepos = Value
            End Set
        End Property

        Public Property TemplateLocalRepos() As String
            Get
                TemplateLocalRepos = _templateLocalRepos
            End Get
            Set(ByVal Value As String)
                _templateLocalRepos = Value
            End Set
        End Property

        Public Property FileName() As String
            Get
                FileName = _fileName
            End Get
            Set(ByVal Value As String)
                _fileName = Value
            End Set
        End Property

        Public Property Valid() As String
            Get
                Valid = _valid
            End Get
            Set(ByVal Value As String)
                _valid = Valid
            End Set
        End Property

        Public Overrides Function ToString() As String

            Return Environment

        End Function

    End Class

End Class
