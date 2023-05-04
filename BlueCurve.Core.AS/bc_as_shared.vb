Imports BlueCurve.Core.CS
Imports Microsoft.Win32
Imports System.Runtime.InteropServices
Public Class bc_am_mime_types
    Private mime_types As New ArrayList
    Public Sub New()
        Dim slog As New bc_cs_activity_log("bc_am_mime_types", "new", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            REM put here mime types whose workflow events can work on document open
            Dim omime As bc_am_mime_types.mime_type
            omime = New bc_am_mime_types.mime_type
            omime.extension = ".doc"
            omime.editable = True
            mime_types.Add(omime)
            omime = New bc_am_mime_types.mime_type
            omime.extension = "[imp].doc"
            omime.editable = False
            omime.render_only = True
            mime_types.Add(omime)
            omime = New bc_am_mime_types.mime_type
            omime.extension = ".docx"
            omime.editable = True
            mime_types.Add(omime)
            omime = New bc_am_mime_types.mime_type
            omime.extension = "[imp].docx"
            omime.render_only = True
            omime.editable = False
            mime_types.Add(omime)
            omime = New bc_am_mime_types.mime_type
            omime.extension = ".docm"
            omime.editable = True
            mime_types.Add(omime)
            omime = New bc_am_mime_types.mime_type
            omime.extension = "[imp].docm"
            omime.render_only = True
            omime.editable = False
            mime_types.Add(omime)

        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_mime_types", "new", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            slog = New bc_cs_activity_log("bc_am_mime_types", "new", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Sub

    REM opens up any mime type file for view only use
    Public Function view(ByVal filename As String, ByVal mime_type As String) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_mime_types", "view", bc_cs_activity_codes.TRACE_ENTRY, filename)
        Dim extensionsize As Integer

        If Len(mime_type) > 5 Then
            If Left(mime_type, 5) = "[imp]" Then

                REM SW cope with office versions
                extensionsize = 0
                extensionsize = (Len(mime_type) - (InStrRev(mime_type, ".") - 1))

                mime_type = Right(mime_type, extensionsize)
            End If
        End If
        Dim regclasses As RegistryKey = Registry.ClassesRoot
        Dim s, p As String
        Dim k As RegistryKey
        Try
            view = False
            k = regclasses.OpenSubKey(mime_type)
            s = k.GetValue("")
            k = regclasses.OpenSubKey(s + "\shell\open\command\")
            s = k.GetValue("")
            p = InStr(1, s, "%")
            If p > 0 Then
                s = Left(s, p - 1)
            End If

            Dim i As Integer
            i = InStr(3, s, """")
            If i > 0 Then
                s = Left(s, i + 1)
            End If
            If Right(s, 2) = """C" Then
                's = """" + Left(s, Len(s) - 2) + """"
                Dim omsg As New bc_cs_message("Blue Curve", "System has no default viewer for mime type: " + mime_type, bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Dim ocomm As New bc_cs_activity_log("bc_am_mime_types", "view", bc_cs_activity_codes.COMMENTARY, "shell: " + s + " """ + filename + """")
                Exit Function
            End If
            Shell(s + " """ + filename + """", AppWinStyle.MaximizedFocus)
        
            Dim ocommentray As New bc_cs_activity_log("bc_am_mime_types", "view", bc_cs_activity_codes.COMMENTARY, "shell: " + s + " """ + filename + """")
            view = True
        Catch ex As Exception
            Dim omessage As New bc_cs_message("Blue Curve - process", "Cannot View file mime type: " + mime_type + " has no supported application please fo to local documents and open file manually", bc_cs_message.MESSAGE)
        Finally
            slog = New bc_cs_activity_log("bc_am_mime_types", "view", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Function get_image_index_for_mime_type(ByVal mime_type As String, ByVal isEnabled As Boolean) As Long
        Dim slog As New bc_cs_activity_log("get_image_index_for_mime_type", "get_default_icon", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try

            Dim extensionsize As Integer

            REM SW cope with office versions
            ' mime_type = Right(mime_type, 4)
            extensionsize = 0
            extensionsize = (Len(mime_type) - (InStrRev(mime_type, ".") - 1))
            mime_type = Right(mime_type, extensionsize)

            If mime_type = ".doc" Then
                If isEnabled Then
                    get_image_index_for_mime_type = 0
                Else
                    get_image_index_for_mime_type = 1
                End If
            ElseIf mime_type = ".docx" Then
                If isEnabled Then
                    get_image_index_for_mime_type = 0
                Else
                    get_image_index_for_mime_type = 1
                End If

            ElseIf mime_type = ".pdf" Then
                If isEnabled Then
                    get_image_index_for_mime_type = 4
                Else
                    get_image_index_for_mime_type = 5
                End If

            ElseIf mime_type = ".xls" Then
                If isEnabled Then
                    get_image_index_for_mime_type = 2
                Else
                    get_image_index_for_mime_type = 3
                End If
            ElseIf mime_type = ".ppt" Then
                If isEnabled Then
                    get_image_index_for_mime_type = 6
                Else
                    get_image_index_for_mime_type = 7
                End If
            ElseIf mime_type = ".pptx" Then
                If isEnabled Then
                    get_image_index_for_mime_type = 6
                Else
                    get_image_index_for_mime_type = 7
                End If
            ElseIf mime_type = ".txt" Then
                If isEnabled Then
                    get_image_index_for_mime_type = 8
                Else
                    get_image_index_for_mime_type = 9
                End If
            ElseIf mime_type = ".bmp" Then
                If isEnabled Then
                    get_image_index_for_mime_type = 10
                Else
                    get_image_index_for_mime_type = 10
                End If
            ElseIf mime_type = ".gif" Or mime_type = ".bmp" Then
                If isEnabled Then
                    get_image_index_for_mime_type = 10
                Else
                    get_image_index_for_mime_type = 10
                End If
            ElseIf mime_type = ".xml" Then
                If isEnabled Then
                    get_image_index_for_mime_type = 11
                Else
                    get_image_index_for_mime_type = 11
                End If
            Else
                If isEnabled Then
                    get_image_index_for_mime_type = 8
                Else
                    get_image_index_for_mime_type = 9
                End If
            End If

        Catch ex As Exception
            Dim ocommentray As New bc_cs_activity_log("get_image_index_for_mime_type", "get_default_icon", bc_cs_activity_codes.COMMENTARY, "Mime type: " + mime_type + " cant find default icon")
        Finally
            slog = New bc_cs_activity_log(" get_image_index_for_mime_type", "get_default_icon", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Function get_default_icon(ByVal mime_type As String) As String
        Dim slog As New bc_cs_activity_log("bc_am_mime_types", "get_default_icon", bc_cs_activity_codes.TRACE_ENTRY, "")
        Try
            Dim regclasses As RegistryKey = Registry.ClassesRoot
            Dim s As String
            Dim k As RegistryKey

            get_default_icon = ""
            k = regclasses.OpenSubKey(mime_type)
            s = k.GetValue("")
            k = regclasses.OpenSubKey(s + "\DefaultIcon")
            s = k.GetValue("")
            get_default_icon = s
        Catch ex As Exception
            Dim ocommentray As New bc_cs_activity_log("bc_am_mime_types", "get_default_icon", bc_cs_activity_codes.COMMENTARY, "Mime type: " + mime_type + " cant find default icon")
            get_default_icon = ""
        Finally
            slog = New bc_cs_activity_log("bc_am_mime_types", "get_default_icon", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try
    End Function
    Public Function can_edit(ByVal mime_type As String, Optional ByRef render_only As Boolean = False) As Boolean
        Dim slog As New bc_cs_activity_log("bc_am_mime_types", "can_edit", bc_cs_activity_codes.TRACE_ENTRY, "")
        REM is mime type editable
        Dim i As Integer
        Try
            can_edit = False
            For i = 0 To mime_types.Count - 1
                If mime_types(i).extension = mime_type Then
                    If mime_types(i).editable = True Then
                        render_only = mime_types(i).render_only
                        can_edit = True
                        Exit For
                    End If
                End If
            Next
        Catch ex As Exception
            Dim db_err As New bc_cs_error_log("bc_am_mime_types", "can_edit", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
            slog = New bc_cs_activity_log("bc_am_mime_types", "can_edit", bc_cs_activity_codes.TRACE_EXIT, "")
        End Try

    End Function
    Private Class mime_type
        Public extension As String
        Public editable As Boolean
        Public render_only As Boolean = False
        Public Sub New()

        End Sub
    End Class
End Class

'added to handle Office 2010 startup changes
<ComImport(), Guid("00000016-0000-0000-C000-000000000046"), InterfaceTypeAttribute(ComInterfaceType.InterfaceIsIUnknown)> _
Public Interface IOleMessageFilter
    <PreserveSig()> Function HandleInComingCall(ByVal dwCallType As Integer, ByVal hTaskCaller As IntPtr, ByVal dwTickCount As Integer, ByVal lpInterfaceInfo As IntPtr) As Integer
    <PreserveSig()> Function RetryRejectedCall(ByVal hTaskCallee As IntPtr, ByVal dwTickCount As Integer, ByVal dwRejectType As Integer) As Integer
    <PreserveSig()> Function MessagePending(ByVal hTaskCallee As IntPtr, ByVal dwTickCount As Integer, ByVal dwPendingType As Integer) As Integer
End Interface

Public Class MessageFilter
    Implements IOleMessageFilter
    <DllImport("Ole32.dll")> _
    Private Shared Function CoRegisterMessageFilter(ByVal newFilter As IOleMessageFilter, ByRef oldFilter As IOleMessageFilter) As Integer
    End Function
    Public Shared Sub Register()
        Dim newFilter As IOleMessageFilter = New MessageFilter()
        Dim oldFilter As IOleMessageFilter = Nothing
        CoRegisterMessageFilter(newFilter, oldFilter)
    End Sub
    Public Shared Sub Revoke()
        Dim oldFilter As IOleMessageFilter = Nothing
        CoRegisterMessageFilter(Nothing, oldFilter)
    End Sub
    Public Function HandleInComingCall(ByVal dwCallType As Integer, ByVal hTaskCaller As System.IntPtr, ByVal dwTickCount As Integer, ByVal lpInterfaceInfo As System.IntPtr) As Integer Implements IOleMessageFilter.HandleInComingCall
        Return 0
    End Function
    Public Function MessagePending(ByVal hTaskCallee As System.IntPtr, ByVal dwTickCount As Integer, ByVal dwPendingType As Integer) As Integer Implements IOleMessageFilter.MessagePending
        Return 2
    End Function
    Public Function RetryRejectedCall(ByVal hTaskCallee As System.IntPtr, ByVal dwTickCount As Integer, ByVal dwRejectType As Integer) As Integer Implements IOleMessageFilter.RetryRejectedCall
        If (dwRejectType = 2) Then
            Return 99
            'Value >=0 and <100: the call is to be retried immediately
            'Value >=100: COM will wait for this many milliseconds and then retry the call
            'Value -1: the call should be canceled. COM the Returns RPC_E_CALL_REJECTED for the original method call
        Else
            Return -1
        End If
    End Function
End Class



