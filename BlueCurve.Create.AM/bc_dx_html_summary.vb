Imports BlueCurve.Core.CS
Imports System.Text.RegularExpressions
Imports DevExpress.XtraRichEdit



Public Class bc_dx_html_summary
    Implements Ibc_dx_html_summary

    Dim _content_length As Integer
    Dim _def_font As String
    Dim _def_font_size As Double
    Dim _simple_mode As Boolean
    Public Function load_view(text As String, rtf As String, html As String, content_length As Integer, def_font As String, def_font_size As Double, simple_mode As Boolean) As Boolean Implements Ibc_dx_html_summary.load_view

        Me.RibbonControl1.ToolbarLocation = DevExpress.XtraBars.Ribbon.RibbonQuickAccessToolbarLocation.Hidden

        Me.RichEditControl1.HtmlText = html
        _content_length = content_length
        _def_font = def_font
        _def_font_size = def_font_size
        _simple_mode = simple_mode

        If _simple_mode = True Then
            Me.InsertRibbonPage1.Visible = False
            Me.ViewRibbonPage1.Visible = False
            'Me.BarButtonGroup1.Visibility = DevExpress.XtraBars.BarItemVisibility.Never
            Me.BarButtonGroup1.Enabled = False

            Me.BarButtonGroup7.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

            Me.BarButtonGroup3.Visibility = DevExpress.XtraBars.BarItemVisibility.Never

            For i = 0 To Me.BarButtonGroup2.ItemLinks.Count - 1
                If i > 2 Then
                    Me.BarButtonGroup2.ItemLinks.RemoveAt(3)
                End If
            Next
            REM set default font and style
            Me.RichEditControl1.Document.DefaultCharacterProperties.FontName = _def_font
            Me.RichEditControl1.Document.DefaultCharacterProperties.FontSize = _def_font_size


        End If

        load_view = True


    End Function
    Function CountWords(ByVal value As String) As Integer
        ' Count matches.
        Dim collection As MatchCollection = Regex.Matches(value, "\S+")
        Return collection.Count
    End Function
    Public Event Save(text As String, rtf As String, html As String) Implements Ibc_dx_html_summary.Save
    Public Event Cancel() Implements Ibc_dx_html_summary.Cancel

    Private Sub bok_Click(sender As Object, e As EventArgs) Handles bok.Click

        If _content_length > 0 Then
            Dim wc As Integer
            wc = CountWords(Me.RichEditControl1.Text)
            If wc > _content_length Then
                Dim omsg As New bc_cs_message("Blue Curve", "Content length is too long (" + CStr(wc) + " words). Please limit to " + CStr(_content_length) + " words", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                Exit Sub
            End If
        End If
        'DocumentRange range = rtbAppointment.Document.Selection;
        '       SubDocument doc = range.BeginUpdateDocument();

        '       var props = doc.BeginUpdateCharacters(range);

        '       props.FontName = _font.Name;
        '       props.FontSize = _font.Size;
        '       props.Bold = _font.Bold;
        '       props.Italic = _font.Italic;

        '       doc.EndUpdateCharacters(props);
        '       range.EndUpdateDocument(doc);
        If _simple_mode = True Then
            set_font()
           

        End If
        RaiseEvent Save(Me.RichEditControl1.Text, Me.RichEditControl1.RtfText, Me.RichEditControl1.HtmlText)
        Me.Hide()
    End Sub
    Private Sub set_font()
        If _simple_mode = True Then
            Me.RichEditControl1.Document.SelectAll()
            Dim r As DevExpress.XtraRichEdit.API.Native.SubDocument
            Dim c As DevExpress.XtraRichEdit.API.Native.CharacterProperties

            r = Me.RichEditControl1.Document.Selection.BeginUpdateDocument()
            c = r.BeginUpdateCharacters(Me.RichEditControl1.Document.Selection)

            c.FontName = _def_font
            c.FontSize = _def_font_size
            r.EndUpdateCharacters(c)
            Me.RichEditControl1.Document.Selection.EndUpdateDocument(r)
        End If
    End Sub
    Private Sub bcancel_Click(sender As Object, e As EventArgs) Handles bcancel.Click
        RaiseEvent Cancel()
        Me.Hide()
    End Sub

    'Private Sub bc_dx_html_summary_MouseMove(sender As Object, e As Windows.Forms.MouseEventArgs) Handles Me.MouseMove
    '    set_font()

    'End Sub

    Private Sub bok_MouseEnter(sender As Object, e As EventArgs) Handles bok.MouseEnter
        set_font()
    End Sub
End Class
Public Class Cbc_dx_html_summary
    Public bsave As Boolean = False
    Public text As String
    Public rtf As String
    Public html As String
    WithEvents _view As Ibc_dx_html_summary
    Public Sub New(view As Ibc_dx_html_summary)
        _view = view

    End Sub

    Public Function load_data(text As String, rtf As String, html As String, content_length As Integer, def_font As String, def_font_size As Double, simple_mode As Boolean)
        Return _view.load_view(text, rtf, html, content_length, def_font, def_font_size, simple_mode)

    End Function
    Public Sub cancel() Handles _view.Cancel
        bsave = False
    End Sub
    Public Sub save(_text As String, _rtf As String, _html As String) Handles _view.Save
        text = _text
        rtf = _rtf
        html = _html
        bsave = True
    End Sub
End Class
Public Interface Ibc_dx_html_summary
    Function load_view(text As String, rtf As String, html As String, content_length As Integer, def_font As String, def_font_size As Double, simple_mode As Boolean) As Boolean

    Event Save(text As String, rtf As String, html As String)
    Event Cancel()
End Interface