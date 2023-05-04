Imports System.Web.Services
Imports System.Security.Principal
Imports System.Web.Services.Protocols
Imports System.ComponentModel
Imports System.IO
Imports BlueCurve.Core.CS

' To allow this Web Service to be called from script, using ASP.NET AJAX, uncomment the following line.
' <System.Web.Script.Services.ScriptService()> _
<WebService(Namespace:="http://BlueCurve.Authenticate.WS/")> _
<WebServiceBinding(ConformsTo:=WsiProfiles.BasicProfile1_1)> _
<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Public Class Service
    Inherits System.Web.Services.WebService

    '<WebMethod()> _
    'Public Function HelloWorld() As String
    '    Return "Hello World"
    'End Function

    <WebMethod()> _
   Public Function GetCustomToken() As String
        Try


            Dim bcs As New bc_cs_central_settings(True)
            Dim gdb As New bc_cs_db_services
            Dim certificate As New bc_cs_security.certificate

            Dim stoken As String
            Dim WinId As IIdentity
            WinId = HttpContext.Current.User.Identity


            Dim wi As WindowsIdentity
            wi = WinId


            certificate.user_id = WinId.Name
            REM token is username + id
            stoken = WinId.Name + wi.Token.ToString
            Dim ffs As New bc_cs_string_services(stoken)
            stoken = ffs.delimit_apostrophies
            ffs = New bc_cs_string_services(certificate.user_id)
            certificate.user_id = ffs.delimit_apostrophies

            REM if group is set then then also authenticate against group
            'If bc_cs_central_settings.ad_group.Trim <> "" Then
            Dim bad As New bc_cs_active_directory
            'Dim rstr As String
            'rstr = bad.Authenticate(WinId, wi.Token, Nothing, Nothing, certificate)
            'If rstr <> "" Then
            'Return "failed: " + rstr
            'Exit Function
            'End If
            'End If 

            Dim user_id As Long = 0
            Dim res As Object
            res = gdb.executesql("dbo.bc_core_check_active_directory '" + certificate.user_id + "','" + stoken + "'", certificate)
            If IsArray(res) Then
                If UBound(res, 2) > -1 Then
                    user_id = res(0, 0)
                End If
            End If

            If certificate.server_errors.Count > 0 Then
                Return "failed:" + certificate.server_errors(0)
                Exit Function
            End If
            If user_id = 0 Then
                Return "failed: could not authenticate against BC database"
            Else
                Return stoken
            End If

            Return stoken
        Catch ex As Exception
            Return "failed:" + ex.Message
        End Try
    End Function
    'Public Function Authenticate(ByVal token As IntPtr, ByVal username As String, ByVal password As String, ByVal certificate As bc_cs_security.certificate) As Boolean

    '    Dim otrace As New bc_cs_activity_log("bc_cs_active_directory", "Authenticate", bc_cs_activity_codes.TRACE_ENTRY, "", certificate)

    '    Dim windowsIdent As WindowsIdentity
    '    Dim impersonationContext As WindowsImpersonationContext

    '    Dim entry As DirectoryEntry
    '    Dim user As DirectoryEntry
    '    Dim deSearch As DirectorySearcher
    '    Dim results As SearchResult

    '    Dim obj As Object

    '    Try

    '        If IntPtr.Zero.Equals(bc_cs_central_settings.user_token) Then ' token not set so use the user name and password
    '            ' create directory entry
    '            entry = New DirectoryEntry( _
    '                                    bc_cs_central_settings.ldap_path, _
    '                                    username, _
    '                                    password, _
    '                                    AuthenticationTypes.Secure _
    '                                    )
    '        Else
    '            ' get the windows identity using the token provided
    '            windowsIdent = New WindowsIdentity(token)

    '            ' impersonate the token user
    '            impersonationContext = windowsIdent.Impersonate

    '            ' create directory entry
    '            entry = New DirectoryEntry( _
    '                                    bc_cs_central_settings.ldap_path, _
    '                                    Nothing, _
    '                                    Nothing, _
    '                                    AuthenticationTypes.Secure _
    '                                    )
    '        End If

    '        ' force authentication
    '        obj = entry.NativeObject



    '        ' if a group is specified then check the authenticated user
    '        ' is a member of a group
    '        If bc_cs_central_settings.ad_group.Trim <> "" Then

    '            ' get the user 
    '            deSearch = New DirectorySearcher
    '            deSearch.SearchRoot = entry
    '            deSearch.Filter = "(&(objectClass=user)(SAMAccountName=" + username + "))"
    '            deSearch.SearchScope = SearchScope.Subtree

    '            results = deSearch.FindOne()

    '            If Not results.Path Is Nothing Then

    '                'get user directory entry for checking groups
    '                If IntPtr.Zero.Equals(bc_cs_central_settings.user_token) Then ' token not set so use the user name and password
    '                    ' create directory entry
    '                    user = New DirectoryEntry( _
    '                                            results.Path, _
    '                                            username, _
    '                                            password, _
    '                                            AuthenticationTypes.Secure _
    '                                            )
    '                Else
    '                    ' create directory entry
    '                    user = New DirectoryEntry( _
    '                                            results.Path, _
    '                                            Nothing, _
    '                                            Nothing, _
    '                                            AuthenticationTypes.Secure _
    '                                            )
    '                End If
    '                Authenticate = isUserInGroup(entry, user, certificate)
    '            End If
    '        Else
    '            Authenticate = True
    '        End If

    '    Catch ex As Exception
    '        Authenticate = False

    '    Finally
    '        If Not impersonationContext Is Nothing Then
    '            ' stop impersonating user
    '            impersonationContext.Undo()
    '        End If
    '        If Not entry Is Nothing Then
    '            ' dispose of ldap directory object
    '            entry.Dispose()
    '        End If
    '        If Not user Is Nothing Then
    '            ' dispose of ldap directory object
    '            user.Dispose()
    '        End If
    '        If Not obj Is Nothing Then
    '            ' clear authenticated object
    '            obj = Nothing
    '        End If
    '        If Not results Is Nothing Then
    '            ' clear results object
    '            results = Nothing
    '        End If
    '        If Not deSearch Is Nothing Then
    '            ' dispose of search object
    '            deSearch.Dispose()
    '        End If
    '        otrace = New bc_cs_activity_log("bc_cs_active_directory", "Authenticate", bc_cs_activity_codes.TRACE_EXIT, "", certificate)
    '    End Try

    'End Function



End Class
