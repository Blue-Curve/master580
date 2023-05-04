Imports System.Windows.Forms
Imports System.Threading


REM ==========================================
REM Bluecurve Limited 2005
REM Module:       Message Form service
REM Description:  New
REM Type:         CS
REM Version:      1
REM Change history
REM ==========================================
Public Class bc_cs_message
    Public Const MESSAGE = 1
    Public Const ERR = 2
    Public ireturn_code As Integer
    Public cancel_selected As Boolean

    Public Sub New(ByVal caption As String, ByVal strmessagetext As String, ByVal imode As Integer, Optional ByVal show_cancel As Boolean = False, Optional ByVal about_mode As Boolean = False, Optional ByVal ok_txt As String = "Ok", Optional ByVal cancel_txt As String = "Cancel", Optional ByVal system_style As Boolean = False)
        If Not bc_cs_central_settings.CommandLine Then

            REM if message raised form asynchronous thread then handle differently
            If Thread.CurrentThread.Name = "BC_ASYNC_PROCESS_EVENTS" Then
                bc_cs_central_settings.async_events_central_error_text.Add(strmessagetext)
                Dim ocomm As New bc_cs_activity_log("Async Events Processing", "Message", bc_cs_activity_codes.COMMENTARY, strmessagetext)
                Exit Sub
            End If


            If system_style = False Then
                Dim omessage As New bc_cs_message_form
                Dim ocommentary As New bc_cs_activity_log("Message: ", "", bc_cs_activity_codes.COMMENTARY, strmessagetext)
                bc_cs_progress.suspend = True

                omessage.Text = caption
                cancel_selected = False

                If about_mode = True Then
                    omessage.Text = "Blue Curve"
                    omessage.Label1.Text = "Copyright Blue Curve Limited" + CStr(Now.Year) + ". Version: " + bc_cs_central_settings.version
                Else
                    omessage.Label1.Text = strmessagetext
                End If
                If show_cancel = True Then
                    omessage.btncancel.Visible = True
                Else
                    omessage.btncancel.Visible = False
                End If
                omessage.Button1.Text = ok_txt
                omessage.btncancel.Text = cancel_txt
                omessage.ShowDialog()
                bc_cs_progress.suspend = False
                Me.cancel_selected = omessage.cancel_selected
            Else
                'Me.cancel_selected = False
                'If show_cancel = True Then
                '    If MessageBox.Show(strmessagetext, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                '        Me.cancel_selected = True
                '    End If
                'Else
                '    MessageBox.Show(strmessagetext, caption, MessageBoxButtons.OK, MessageBoxIcon.Information)
                'End If

                If show_cancel = True Then
                    If DevExpress.XtraEditors.XtraMessageBox.Show(strmessagetext, caption, MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.No Then
                        Me.cancel_selected = True
                    End If
                Else
                    DevExpress.XtraEditors.XtraMessageBox.Show(strmessagetext, caption, MessageBoxButtons.ok, MessageBoxIcon.Information)
                End If
            End If
        Else
            Console.WriteLine(strmessagetext)
        End If
    End Sub
    Public Sub New()

    End Sub
End Class

