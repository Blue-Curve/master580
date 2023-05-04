Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Collections
Public Class bc_am_workflow_params
    Inherits System.Windows.Forms.Form

#Region " Windows Form Designer generated code "

    Public Sub New()
        MyBase.New()

        'This call is required by the Windows Form Designer.
        InitializeComponent()

        'Add any initialization after the InitializeComponent() call

    End Sub

    'Form overrides dispose to clean up the component list.
    Protected Overloads Overrides Sub Dispose(ByVal disposing As Boolean)
        If disposing Then
            If Not (components Is Nothing) Then
                components.Dispose()
            End If
        End If
        MyBase.Dispose(disposing)
    End Sub

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    Friend WithEvents PictureBox1 As System.Windows.Forms.PictureBox
    Friend WithEvents Panel1 As System.Windows.Forms.Panel
    Friend WithEvents PictureBox2 As System.Windows.Forms.PictureBox
    Friend WithEvents Label3 As System.Windows.Forms.Label
    Friend WithEvents btnsubmit As System.Windows.Forms.Button
    Friend WithEvents Btncancel As System.Windows.Forms.Button
    Public WithEvents lcomment As System.Windows.Forms.Label
    Friend WithEvents DateTimePicker1 As System.Windows.Forms.DateTimePicker
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label2 As System.Windows.Forms.Label
    Friend WithEvents cminute As System.Windows.Forms.ComboBox
    Friend WithEvents chour As System.Windows.Forms.ComboBox
    Public WithEvents tcomment As System.Windows.Forms.TextBox
    Friend WithEvents cno As System.Windows.Forms.ComboBox
    Friend WithEvents Cyes As System.Windows.Forms.ComboBox
    Friend WithEvents Cexpire As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_workflow_params))
        Me.PictureBox1 = New System.Windows.Forms.PictureBox
        Me.Panel1 = New System.Windows.Forms.Panel
        Me.Cexpire = New System.Windows.Forms.CheckBox
        Me.cno = New System.Windows.Forms.ComboBox
        Me.Label2 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Cyes = New System.Windows.Forms.ComboBox
        Me.cminute = New System.Windows.Forms.ComboBox
        Me.chour = New System.Windows.Forms.ComboBox
        Me.DateTimePicker1 = New System.Windows.Forms.DateTimePicker
        Me.tcomment = New System.Windows.Forms.TextBox
        Me.lcomment = New System.Windows.Forms.Label
        Me.Label1 = New System.Windows.Forms.Label
        Me.PictureBox2 = New System.Windows.Forms.PictureBox
        Me.Label3 = New System.Windows.Forms.Label
        Me.btnsubmit = New System.Windows.Forms.Button
        Me.Btncancel = New System.Windows.Forms.Button
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.Panel1.SuspendLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'PictureBox1
        '
        Me.PictureBox1.Image = CType(resources.GetObject("PictureBox1.Image"), System.Drawing.Image)
        Me.PictureBox1.Location = New System.Drawing.Point(-24, 0)
        Me.PictureBox1.Name = "PictureBox1"
        Me.PictureBox1.Size = New System.Drawing.Size(520, 64)
        Me.PictureBox1.TabIndex = 4
        Me.PictureBox1.TabStop = False
        '
        'Panel1
        '
        Me.Panel1.BackColor = System.Drawing.Color.FromArgb(CType(CType(239, Byte), Integer), CType(CType(243, Byte), Integer), CType(CType(255, Byte), Integer))
        Me.Panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle
        Me.Panel1.Controls.Add(Me.Cexpire)
        Me.Panel1.Controls.Add(Me.cno)
        Me.Panel1.Controls.Add(Me.Label2)
        Me.Panel1.Controls.Add(Me.Label4)
        Me.Panel1.Controls.Add(Me.Cyes)
        Me.Panel1.Controls.Add(Me.cminute)
        Me.Panel1.Controls.Add(Me.chour)
        Me.Panel1.Controls.Add(Me.DateTimePicker1)
        Me.Panel1.Controls.Add(Me.tcomment)
        Me.Panel1.Controls.Add(Me.lcomment)
        Me.Panel1.Controls.Add(Me.Label1)
        Me.Panel1.Location = New System.Drawing.Point(0, 64)
        Me.Panel1.Name = "Panel1"
        Me.Panel1.Size = New System.Drawing.Size(392, 152)
        Me.Panel1.TabIndex = 7
        '
        'Cexpire
        '
        Me.Cexpire.AccessibleRole = System.Windows.Forms.AccessibleRole.None
        Me.Cexpire.Checked = True
        Me.Cexpire.CheckState = System.Windows.Forms.CheckState.Checked
        Me.Cexpire.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cexpire.Location = New System.Drawing.Point(24, 112)
        Me.Cexpire.Name = "Cexpire"
        Me.Cexpire.Size = New System.Drawing.Size(112, 24)
        Me.Cexpire.TabIndex = 15
        Me.Cexpire.Text = "Completion Date:"
        '
        'cno
        '
        Me.cno.Location = New System.Drawing.Point(184, 168)
        Me.cno.Name = "cno"
        Me.cno.Size = New System.Drawing.Size(80, 21)
        Me.cno.TabIndex = 14
        Me.cno.Visible = False
        '
        'Label2
        '
        Me.Label2.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label2.Location = New System.Drawing.Point(32, 168)
        Me.Label2.Name = "Label2"
        Me.Label2.Size = New System.Drawing.Size(128, 16)
        Me.Label2.TabIndex = 13
        Me.Label2.Text = "Next stage when reject :"
        Me.Label2.Visible = False
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(32, 144)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(160, 16)
        Me.Label4.TabIndex = 12
        Me.Label4.Text = "Next stage when satisfied:"
        Me.Label4.Visible = False
        '
        'Cyes
        '
        Me.Cyes.Location = New System.Drawing.Point(184, 144)
        Me.Cyes.Name = "Cyes"
        Me.Cyes.Size = New System.Drawing.Size(80, 21)
        Me.Cyes.TabIndex = 9
        Me.Cyes.Visible = False
        '
        'cminute
        '
        Me.cminute.Location = New System.Drawing.Point(320, 112)
        Me.cminute.Name = "cminute"
        Me.cminute.Size = New System.Drawing.Size(48, 21)
        Me.cminute.TabIndex = 7
        '
        'chour
        '
        Me.chour.Location = New System.Drawing.Point(264, 112)
        Me.chour.Name = "chour"
        Me.chour.Size = New System.Drawing.Size(50, 21)
        Me.chour.TabIndex = 6
        '
        'DateTimePicker1
        '
        Me.DateTimePicker1.AllowDrop = True
        Me.DateTimePicker1.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.DateTimePicker1.Location = New System.Drawing.Point(144, 112)
        Me.DateTimePicker1.Name = "DateTimePicker1"
        Me.DateTimePicker1.Size = New System.Drawing.Size(112, 22)
        Me.DateTimePicker1.TabIndex = 5
        '
        'tcomment
        '
        Me.tcomment.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tcomment.Location = New System.Drawing.Point(24, 27)
        Me.tcomment.MaxLength = 200
        Me.tcomment.Multiline = True
        Me.tcomment.Name = "tcomment"
        Me.tcomment.Size = New System.Drawing.Size(344, 77)
        Me.tcomment.TabIndex = 2
        '
        'lcomment
        '
        Me.lcomment.Font = New System.Drawing.Font("Arial Unicode MS", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lcomment.Location = New System.Drawing.Point(24, 8)
        Me.lcomment.Name = "lcomment"
        Me.lcomment.Size = New System.Drawing.Size(344, 16)
        Me.lcomment.TabIndex = 0
        Me.lcomment.Text = "Comment for users assigned to next stage"
        '
        'Label1
        '
        Me.Label1.Font = New System.Drawing.Font("Arial", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label1.Location = New System.Drawing.Point(304, 112)
        Me.Label1.Name = "Label1"
        Me.Label1.RightToLeft = System.Windows.Forms.RightToLeft.Yes
        Me.Label1.Size = New System.Drawing.Size(17, 16)
        Me.Label1.TabIndex = 8
        Me.Label1.Text = "::"
        '
        'PictureBox2
        '
        Me.PictureBox2.Image = CType(resources.GetObject("PictureBox2.Image"), System.Drawing.Image)
        Me.PictureBox2.Location = New System.Drawing.Point(10, 224)
        Me.PictureBox2.Name = "PictureBox2"
        Me.PictureBox2.Size = New System.Drawing.Size(184, 37)
        Me.PictureBox2.TabIndex = 8
        Me.PictureBox2.TabStop = False
        '
        'Label3
        '
        Me.Label3.Font = New System.Drawing.Font("Verdana", 6.75!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label3.Location = New System.Drawing.Point(9, 256)
        Me.Label3.Name = "Label3"
        Me.Label3.Size = New System.Drawing.Size(256, 32)
        Me.Label3.TabIndex = 9
        Me.Label3.Text = "© Blue Curve Limited. All rights reserved. "
        '
        'btnsubmit
        '
        Me.btnsubmit.BackColor = System.Drawing.SystemColors.ControlLight
        Me.btnsubmit.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.btnsubmit.Location = New System.Drawing.Point(312, 224)
        Me.btnsubmit.Name = "btnsubmit"
        Me.btnsubmit.Size = New System.Drawing.Size(72, 24)
        Me.btnsubmit.TabIndex = 13
        Me.btnsubmit.Text = "&Ok"
        Me.btnsubmit.UseVisualStyleBackColor = False
        '
        'Btncancel
        '
        Me.Btncancel.BackColor = System.Drawing.SystemColors.ControlLight
        Me.Btncancel.Font = New System.Drawing.Font("Arial Unicode MS", 9.0!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Btncancel.Location = New System.Drawing.Point(232, 224)
        Me.Btncancel.Name = "Btncancel"
        Me.Btncancel.Size = New System.Drawing.Size(72, 24)
        Me.Btncancel.TabIndex = 14
        Me.Btncancel.Text = "&Cancel"
        Me.Btncancel.UseVisualStyleBackColor = False
        '
        'bc_am_workflow_params
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.White
        Me.ClientSize = New System.Drawing.Size(392, 278)
        Me.Controls.Add(Me.btnsubmit)
        Me.Controls.Add(Me.Btncancel)
        Me.Controls.Add(Me.Panel1)
        Me.Controls.Add(Me.PictureBox1)
        Me.Controls.Add(Me.Label3)
        Me.Controls.Add(Me.PictureBox2)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_workflow_params"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Blue Curve - Process - Change Stage"
        CType(Me.PictureBox1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.Panel1.ResumeLayout(False)
        Me.Panel1.PerformLayout()
        CType(Me.PictureBox2, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

#End Region
    Public new_stage As String
    Public next_stages As New ArrayList
    Public ldoc As bc_om_document
    Public ok_selected As Boolean
    Private Sub bc_am_workflow_params_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Try

            Me.tcomment.Text = ldoc.main_note
            Dim i As Integer
            Me.chour.Items.Clear()
            Me.cminute.Items.Clear()
            For i = 0 To 23
                If Len(CStr(i)) = 1 Then
                    Me.chour.Items.Add("0" + CStr(i))
                Else
                    Me.chour.Items.Add(CStr(i))
                End If
            Next
            For i = 0 To 59
                If Len(CStr(i)) = 1 Then
                    Me.cminute.Items.Add("0" + CStr(i))
                Else
                    Me.cminute.Items.Add(CStr(i))
                End If
            Next

            Me.DateTimePicker1.Text = Now
            Me.chour.SelectedIndex() = CInt(Format(Now, "HH"))
            Me.cminute.SelectedIndex() = CInt(Format(Now, "mm"))
            Me.Cexpire.Checked = False
            Me.DateTimePicker1.Enabled = False
            Me.chour.Enabled = False
            Me.cminute.Enabled = False

            Me.Cexpire.Checked = False
            If ldoc.stage_expire_date <> System.DateTime.Parse("9-9-9999") Then
                Me.Cexpire.Checked = True
                Me.DateTimePicker1.Text = ldoc.stage_expire_date
                Me.chour.SelectedIndex() = ldoc.stage_expire_date.Hour()
                Me.cminute.SelectedIndex() = ldoc.stage_expire_date.Minute()
            End If

        Catch ex As Exception
            'Dim oerr As New bc_cs_error_log("bc_am_workflow_params", "Load", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally

        End Try
    End Sub
    Private Sub btnsubmit_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnsubmit.Click
        Try

            ldoc.main_note = Me.tcomment.Text
            If Me.Cexpire.Checked = True Then

                Dim sdate As String
                sdate = CStr(Me.DateTimePicker1.Value)

                'sdate = sdate.Substring(0, 10)
                'sdate = sdate + " " + Me.chour.Text + ":" + Me.cminute.Text + ":00"
                'ldoc.stage_expire_date = CDate(sdate)

                REM 21/06/2011 Steve Date Changes
                Dim servicedate As New bc_cs_date_services
                ldoc.doc_date = CDate(servicedate.DateToLocalShort(sdate) + " " + Me.chour.Text + ":" + Me.cminute.Text + ":00")

                ldoc.main_note = ldoc.main_note
            Else
                ldoc.stage_expire_date = System.DateTime.Parse("9-9-9999")
            End If
            Me.Close()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("btnsubmit", "click", bc_cs_error_codes.USER_DEFINED, ex.Message)

        Finally
            Me.ok_selected = True
        End Try
    End Sub
    Private Sub Cexpire_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cexpire.CheckedChanged
        Try
            If Me.Cexpire.Checked = False Then
                Me.DateTimePicker1.Enabled = False
                Me.chour.Enabled = False
                Me.cminute.Enabled = False
            Else
                Me.DateTimePicker1.Enabled = True
                Me.chour.Enabled = True
                Me.cminute.Enabled = True
            End If
        Catch

        End Try
    End Sub

    Private Sub Btncancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Btncancel.Click
        Me.ok_selected = False
        Me.Close()
    End Sub

    Private Sub Panel1_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs) Handles Panel1.Paint

    End Sub
End Class
