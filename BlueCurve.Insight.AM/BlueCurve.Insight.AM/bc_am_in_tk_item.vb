Imports BlueCurve.Core.CS
Imports BlueCurve.Core.OM
Imports System.Collections
Public Class bc_am_in_tk_item
    Inherits System.Windows.Forms.Form
    Public contributor_names As New ArrayList
    Public cancel_selected As Boolean = True
    Public edit_mode As Boolean = False
    Public fcontext As bc_am_in_context
    Public gsymbol As String
    Public osheet As bc_om_insight_sheet
    Public from_load As Boolean = False
    Public set_tx As Boolean = False
    Public set_link As Boolean = False
    Public orig_tx As String
    Public orig_link As String
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
    Friend WithEvents bcancel As System.Windows.Forms.Button
    Friend WithEvents Bapply As System.Windows.Forms.Button
    Friend WithEvents ToolTip1 As System.Windows.Forms.ToolTip
    Friend WithEvents uxItems As System.Windows.Forms.GroupBox
    Friend WithEvents Cexist As System.Windows.Forms.ComboBox
    Friend WithEvents rexist As System.Windows.Forms.RadioButton
    Friend WithEvents rnew As System.Windows.Forms.RadioButton
    Friend WithEvents Label8 As System.Windows.Forms.Label
    Friend WithEvents ccont As System.Windows.Forms.ComboBox
    Friend WithEvents tlabel As System.Windows.Forms.TextBox
    Friend WithEvents cflexible As System.Windows.Forms.CheckBox
    Friend WithEvents tlinkcode As System.Windows.Forms.TextBox
    Friend WithEvents tlookup As System.Windows.Forms.TextBox
    Friend WithEvents Crepeat As System.Windows.Forms.ComboBox
    Friend WithEvents cstorage As System.Windows.Forms.ComboBox
    Friend WithEvents cdatatype As System.Windows.Forms.ComboBox
    Friend WithEvents csymbol As System.Windows.Forms.ComboBox
    Friend WithEvents cfactor As System.Windows.Forms.ComboBox
    Friend WithEvents Label10 As System.Windows.Forms.Label
    Friend WithEvents Label9 As System.Windows.Forms.Label
    Friend WithEvents llookup As System.Windows.Forms.Label
    Friend WithEvents Label7 As System.Windows.Forms.Label
    Friend WithEvents Label6 As System.Windows.Forms.Label
    Friend WithEvents Label4 As System.Windows.Forms.Label
    Friend WithEvents Label5 As System.Windows.Forms.Label
    Friend WithEvents Symbol As System.Windows.Forms.Label
    Friend WithEvents uxValidations As System.Windows.Forms.GroupBox
    Friend WithEvents lval As System.Windows.Forms.ListView
    Friend WithEvents ColumnHeader10 As System.Windows.Forms.ColumnHeader
    Friend WithEvents cmon As System.Windows.Forms.CheckBox
    Friend WithEvents Cprice As System.Windows.Forms.CheckBox
    <System.Diagnostics.DebuggerStepThrough()> Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container
        Dim resources As System.ComponentModel.ComponentResourceManager = New System.ComponentModel.ComponentResourceManager(GetType(bc_am_in_tk_item))
        Me.bcancel = New System.Windows.Forms.Button
        Me.Bapply = New System.Windows.Forms.Button
        Me.ToolTip1 = New System.Windows.Forms.ToolTip(Me.components)
        Me.Cexist = New System.Windows.Forms.ComboBox
        Me.cmon = New System.Windows.Forms.CheckBox
        Me.Cprice = New System.Windows.Forms.CheckBox
        Me.uxItems = New System.Windows.Forms.GroupBox
        Me.Label8 = New System.Windows.Forms.Label
        Me.ccont = New System.Windows.Forms.ComboBox
        Me.tlabel = New System.Windows.Forms.TextBox
        Me.cflexible = New System.Windows.Forms.CheckBox
        Me.tlinkcode = New System.Windows.Forms.TextBox
        Me.tlookup = New System.Windows.Forms.TextBox
        Me.Crepeat = New System.Windows.Forms.ComboBox
        Me.cstorage = New System.Windows.Forms.ComboBox
        Me.cdatatype = New System.Windows.Forms.ComboBox
        Me.csymbol = New System.Windows.Forms.ComboBox
        Me.cfactor = New System.Windows.Forms.ComboBox
        Me.Label10 = New System.Windows.Forms.Label
        Me.Label9 = New System.Windows.Forms.Label
        Me.llookup = New System.Windows.Forms.Label
        Me.Label7 = New System.Windows.Forms.Label
        Me.Label6 = New System.Windows.Forms.Label
        Me.Label4 = New System.Windows.Forms.Label
        Me.Label5 = New System.Windows.Forms.Label
        Me.Symbol = New System.Windows.Forms.Label
        Me.rnew = New System.Windows.Forms.RadioButton
        Me.rexist = New System.Windows.Forms.RadioButton
        Me.uxValidations = New System.Windows.Forms.GroupBox
        Me.lval = New System.Windows.Forms.ListView
        Me.ColumnHeader10 = New System.Windows.Forms.ColumnHeader
        Me.uxItems.SuspendLayout()
        Me.uxValidations.SuspendLayout()
        Me.SuspendLayout()
        '
        'bcancel
        '
        Me.bcancel.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.bcancel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.bcancel.Location = New System.Drawing.Point(198, 504)
        Me.bcancel.Name = "bcancel"
        Me.bcancel.Size = New System.Drawing.Size(75, 23)
        Me.bcancel.TabIndex = 3
        Me.bcancel.Text = "Cancel"
        '
        'Bapply
        '
        Me.Bapply.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Bapply.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Bapply.Location = New System.Drawing.Point(119, 504)
        Me.Bapply.Name = "Bapply"
        Me.Bapply.Size = New System.Drawing.Size(75, 23)
        Me.Bapply.TabIndex = 2
        Me.Bapply.Text = "Ok"
        '
        'Cexist
        '
        Me.Cexist.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Cexist.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Cexist.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cexist.Location = New System.Drawing.Point(76, 61)
        Me.Cexist.MaxLength = 250
        Me.Cexist.Name = "Cexist"
        Me.Cexist.Size = New System.Drawing.Size(136, 21)
        Me.Cexist.TabIndex = 3
        Me.ToolTip1.SetToolTip(Me.Cexist, "q")
        '
        'cmon
        '
        Me.cmon.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cmon.Location = New System.Drawing.Point(76, 39)
        Me.cmon.Name = "cmon"
        Me.cmon.Size = New System.Drawing.Size(80, 16)
        Me.cmon.TabIndex = 80
        Me.cmon.Text = "Monetary"
        Me.ToolTip1.SetToolTip(Me.cmon, "Check if item is monetary")
        '
        'Cprice
        '
        Me.Cprice.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Cprice.Location = New System.Drawing.Point(159, 39)
        Me.Cprice.Name = "Cprice"
        Me.Cprice.Size = New System.Drawing.Size(97, 16)
        Me.Cprice.TabIndex = 81
        Me.Cprice.Text = "Price Related"
        Me.ToolTip1.SetToolTip(Me.Cprice, "CHeck if item is price related")
        '
        'uxItems
        '
        Me.uxItems.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxItems.Controls.Add(Me.Cprice)
        Me.uxItems.Controls.Add(Me.cmon)
        Me.uxItems.Controls.Add(Me.Cexist)
        Me.uxItems.Controls.Add(Me.Label8)
        Me.uxItems.Controls.Add(Me.ccont)
        Me.uxItems.Controls.Add(Me.tlabel)
        Me.uxItems.Controls.Add(Me.cflexible)
        Me.uxItems.Controls.Add(Me.tlinkcode)
        Me.uxItems.Controls.Add(Me.tlookup)
        Me.uxItems.Controls.Add(Me.Crepeat)
        Me.uxItems.Controls.Add(Me.cstorage)
        Me.uxItems.Controls.Add(Me.cdatatype)
        Me.uxItems.Controls.Add(Me.csymbol)
        Me.uxItems.Controls.Add(Me.cfactor)
        Me.uxItems.Controls.Add(Me.Label10)
        Me.uxItems.Controls.Add(Me.Label9)
        Me.uxItems.Controls.Add(Me.llookup)
        Me.uxItems.Controls.Add(Me.Label7)
        Me.uxItems.Controls.Add(Me.Label6)
        Me.uxItems.Controls.Add(Me.Label4)
        Me.uxItems.Controls.Add(Me.Label5)
        Me.uxItems.Controls.Add(Me.Symbol)
        Me.uxItems.Controls.Add(Me.rnew)
        Me.uxItems.Controls.Add(Me.rexist)
        Me.uxItems.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxItems.Location = New System.Drawing.Point(8, 8)
        Me.uxItems.Name = "uxItems"
        Me.uxItems.Size = New System.Drawing.Size(264, 315)
        Me.uxItems.TabIndex = 0
        Me.uxItems.TabStop = False
        Me.uxItems.Text = "Item Name"
        '
        'Label8
        '
        Me.Label8.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label8.Location = New System.Drawing.Point(4, 61)
        Me.Label8.Name = "Label8"
        Me.Label8.Size = New System.Drawing.Size(68, 16)
        Me.Label8.TabIndex = 2
        Me.Label8.Text = "Display:"
        '
        'ccont
        '
        Me.ccont.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.ccont.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.ccont.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.ccont.Location = New System.Drawing.Point(76, 276)
        Me.ccont.MaxLength = 4000
        Me.ccont.Name = "ccont"
        Me.ccont.Size = New System.Drawing.Size(136, 21)
        Me.ccont.TabIndex = 19
        Me.ccont.Visible = False
        '
        'tlabel
        '
        Me.tlabel.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlabel.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tlabel.Location = New System.Drawing.Point(76, 61)
        Me.tlabel.MaxLength = 250
        Me.tlabel.Name = "tlabel"
        Me.tlabel.Size = New System.Drawing.Size(176, 21)
        Me.tlabel.TabIndex = 79
        '
        'cflexible
        '
        Me.cflexible.Anchor = CType((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cflexible.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cflexible.Location = New System.Drawing.Point(236, 245)
        Me.cflexible.Name = "cflexible"
        Me.cflexible.Size = New System.Drawing.Size(16, 16)
        Me.cflexible.TabIndex = 17
        '
        'tlinkcode
        '
        Me.tlinkcode.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlinkcode.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tlinkcode.Location = New System.Drawing.Point(76, 212)
        Me.tlinkcode.MaxLength = 250
        Me.tlinkcode.Name = "tlinkcode"
        Me.tlinkcode.Size = New System.Drawing.Size(176, 21)
        Me.tlinkcode.TabIndex = 13
        '
        'tlookup
        '
        Me.tlookup.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.tlookup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.tlookup.Location = New System.Drawing.Point(76, 276)
        Me.tlookup.Name = "tlookup"
        Me.tlookup.Size = New System.Drawing.Size(176, 21)
        Me.tlookup.TabIndex = 20
        '
        'Crepeat
        '
        Me.Crepeat.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.Crepeat.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.Crepeat.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Crepeat.Location = New System.Drawing.Point(76, 244)
        Me.Crepeat.Name = "Crepeat"
        Me.Crepeat.Size = New System.Drawing.Size(60, 21)
        Me.Crepeat.TabIndex = 15
        '
        'cstorage
        '
        Me.cstorage.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cstorage.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cstorage.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cstorage.Location = New System.Drawing.Point(76, 180)
        Me.cstorage.Name = "cstorage"
        Me.cstorage.Size = New System.Drawing.Size(136, 21)
        Me.cstorage.TabIndex = 11
        '
        'cdatatype
        '
        Me.cdatatype.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cdatatype.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cdatatype.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cdatatype.Location = New System.Drawing.Point(76, 90)
        Me.cdatatype.Name = "cdatatype"
        Me.cdatatype.Size = New System.Drawing.Size(136, 21)
        Me.cdatatype.TabIndex = 5
        '
        'csymbol
        '
        Me.csymbol.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.csymbol.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.csymbol.Location = New System.Drawing.Point(76, 151)
        Me.csymbol.MaxLength = 250
        Me.csymbol.Name = "csymbol"
        Me.csymbol.Size = New System.Drawing.Size(60, 21)
        Me.csymbol.TabIndex = 9
        '
        'cfactor
        '
        Me.cfactor.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.cfactor.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList
        Me.cfactor.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.cfactor.Location = New System.Drawing.Point(76, 122)
        Me.cfactor.Name = "cfactor"
        Me.cfactor.Size = New System.Drawing.Size(136, 21)
        Me.cfactor.TabIndex = 7
        '
        'Label10
        '
        Me.Label10.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label10.Location = New System.Drawing.Point(4, 185)
        Me.Label10.Name = "Label10"
        Me.Label10.Size = New System.Drawing.Size(72, 16)
        Me.Label10.TabIndex = 10
        Me.Label10.Text = "Storage:"
        '
        'Label9
        '
        Me.Label9.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label9.Location = New System.Drawing.Point(4, 93)
        Me.Label9.Name = "Label9"
        Me.Label9.Size = New System.Drawing.Size(72, 16)
        Me.Label9.TabIndex = 4
        Me.Label9.Text = "Data Type:"
        '
        'llookup
        '
        Me.llookup.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.llookup.Location = New System.Drawing.Point(4, 280)
        Me.llookup.Name = "llookup"
        Me.llookup.Size = New System.Drawing.Size(72, 16)
        Me.llookup.TabIndex = 18
        Me.llookup.Text = "Lookup:"
        '
        'Label7
        '
        Me.Label7.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label7.Location = New System.Drawing.Point(4, 218)
        Me.Label7.Name = "Label7"
        Me.Label7.Size = New System.Drawing.Size(72, 16)
        Me.Label7.TabIndex = 12
        Me.Label7.Text = "Link Code:"
        '
        'Label6
        '
        Me.Label6.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label6.Location = New System.Drawing.Point(4, 248)
        Me.Label6.Name = "Label6"
        Me.Label6.Size = New System.Drawing.Size(72, 16)
        Me.Label6.TabIndex = 14
        Me.Label6.Text = "Repeating:"
        '
        'Label4
        '
        Me.Label4.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label4.Location = New System.Drawing.Point(172, 247)
        Me.Label4.Name = "Label4"
        Me.Label4.Size = New System.Drawing.Size(56, 16)
        Me.Label4.TabIndex = 16
        Me.Label4.Text = "Flexible:"
        '
        'Label5
        '
        Me.Label5.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Label5.Location = New System.Drawing.Point(4, 125)
        Me.Label5.Name = "Label5"
        Me.Label5.Size = New System.Drawing.Size(56, 16)
        Me.Label5.TabIndex = 6
        Me.Label5.Text = "Factor:"
        '
        'Symbol
        '
        Me.Symbol.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Symbol.Location = New System.Drawing.Point(4, 154)
        Me.Symbol.Name = "Symbol"
        Me.Symbol.Size = New System.Drawing.Size(56, 16)
        Me.Symbol.TabIndex = 8
        Me.Symbol.Text = "Symbol:"
        '
        'rnew
        '
        Me.rnew.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rnew.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rnew.Location = New System.Drawing.Point(8, 15)
        Me.rnew.Name = "rnew"
        Me.rnew.Size = New System.Drawing.Size(64, 32)
        Me.rnew.TabIndex = 0
        Me.rnew.Text = "New"
        Me.rnew.Visible = False
        '
        'rexist
        '
        Me.rexist.Anchor = CType((System.Windows.Forms.AnchorStyles.Bottom Or System.Windows.Forms.AnchorStyles.Left), System.Windows.Forms.AnchorStyles)
        Me.rexist.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.rexist.Location = New System.Drawing.Point(72, 15)
        Me.rexist.Name = "rexist"
        Me.rexist.Size = New System.Drawing.Size(80, 32)
        Me.rexist.TabIndex = 1
        Me.rexist.Text = "Existing"
        Me.rexist.Visible = False
        '
        'uxValidations
        '
        Me.uxValidations.Anchor = CType((((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Bottom) _
                    Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.uxValidations.Controls.Add(Me.lval)
        Me.uxValidations.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.uxValidations.Location = New System.Drawing.Point(8, 329)
        Me.uxValidations.Name = "uxValidations"
        Me.uxValidations.Size = New System.Drawing.Size(264, 160)
        Me.uxValidations.TabIndex = 1
        Me.uxValidations.TabStop = False
        Me.uxValidations.Text = "Check Validations Required"
        '
        'lval
        '
        Me.lval.Anchor = CType(((System.Windows.Forms.AnchorStyles.Top Or System.Windows.Forms.AnchorStyles.Left) _
                    Or System.Windows.Forms.AnchorStyles.Right), System.Windows.Forms.AnchorStyles)
        Me.lval.CheckBoxes = True
        Me.lval.Columns.AddRange(New System.Windows.Forms.ColumnHeader() {Me.ColumnHeader10})
        Me.lval.Font = New System.Drawing.Font("Verdana", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.lval.FullRowSelect = True
        Me.lval.Location = New System.Drawing.Point(8, 24)
        Me.lval.MultiSelect = False
        Me.lval.Name = "lval"
        Me.lval.Size = New System.Drawing.Size(248, 128)
        Me.lval.TabIndex = 0
        Me.lval.UseCompatibleStateImageBehavior = False
        Me.lval.View = System.Windows.Forms.View.Details
        '
        'ColumnHeader10
        '
        Me.ColumnHeader10.Text = "Type"
        Me.ColumnHeader10.Width = 221
        '
        'bc_am_in_tk_item
        '
        Me.AutoScaleBaseSize = New System.Drawing.Size(5, 13)
        Me.BackColor = System.Drawing.Color.FromArgb(CType(CType(235, Byte), Integer), CType(CType(240, Byte), Integer), CType(CType(249, Byte), Integer))
        Me.ClientSize = New System.Drawing.Size(278, 532)
        Me.Controls.Add(Me.uxValidations)
        Me.Controls.Add(Me.uxItems)
        Me.Controls.Add(Me.bcancel)
        Me.Controls.Add(Me.Bapply)
        Me.Icon = CType(resources.GetObject("$this.Icon"), System.Drawing.Icon)
        Me.MaximizeBox = False
        Me.MinimizeBox = False
        Me.Name = "bc_am_in_tk_item"
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Item Properties"
        Me.uxItems.ResumeLayout(False)
        Me.uxItems.PerformLayout()
        Me.uxValidations.ResumeLayout(False)
        Me.ResumeLayout(False)

    End Sub

#End Region

    Private Sub pedit_Paint(ByVal sender As System.Object, ByVal e As System.Windows.Forms.PaintEventArgs)

    End Sub

    Private Sub cdatatype_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cdatatype.SelectedIndexChanged
        Try
            Dim i As Integer
            Me.cfactor.Enabled = False
            Me.cfactor.Text = 1
            Me.ccont.SelectedIndex = -1
            Me.csymbol.SelectedIndex = -1
            Select Case Me.cdatatype.SelectedIndex
                Case 1
                    Me.csymbol.Text = "(true/false)"
                Case 2
                    Me.csymbol.Text = "(text)"
                Case 3
                    Me.csymbol.Text = "(date)"
            End Select
            If Me.cdatatype.SelectedIndex = 0 Then
                Me.cfactor.Enabled = True
                Me.cfactor.SelectedIndex = -1
            End If
            Me.llookup.Text = "Lookup"
            Me.tlookup.Visible = True
            Me.ccont.Visible = False
            Me.lval.Enabled = True
            If cdatatype.Text = "Output" Then
                Me.llookup.Text = "Contributor"
                Me.tlookup.Visible = False
                Me.ccont.Visible = True
                For i = 0 To Me.lval.Items.Count - 1
                    Me.lval.Items(i).Checked = False
                Next
                Me.lval.Enabled = False
            End If
            check_apply()
        Catch ex As Exception
            Dim oerr As New bc_cs_error_log("bc_am_in_context_Load", "cdatatype_SelectedIndexChanged", bc_cs_error_codes.USER_DEFINED, ex.Message)
        Finally
        End Try
    End Sub

    Private Sub cstorage_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cstorage.SelectedIndexChanged
        Me.Crepeat.Enabled = False

        If Me.cstorage.SelectedIndex > 1 Then
            Me.Crepeat.Enabled = True
        End If
        Me.tlookup.Enabled = True
        If UCase(Me.cstorage.Text) = "PERIOD" Then
            Me.tlookup.Enabled = False
        End If
        check_apply()
    End Sub
    Private Sub cfactor_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cfactor.SelectedIndexChanged
        check_apply()
    End Sub
    Private Sub bc_am_in_tk_item_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Bapply.Enabled = False
        Me.Cexist.Visible = False
        Me.set_tx = False
    End Sub
    Private Sub check_apply()
        Me.Bapply.Enabled = False

        If (Trim(Me.tlabel.Text) <> "" Or Me.Cexist.SelectedIndex <> -1) And Me.cfactor.SelectedIndex > -1 And Me.cdatatype.SelectedIndex > -1 And Me.cstorage.SelectedIndex > -1 Then
            If Me.cdatatype.Text = "Output" And Me.ccont.SelectedIndex = -1 Then
                Exit Sub
            End If
            Me.Bapply.Enabled = True
        End If
    End Sub

    Private Sub PictureBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        cancel_selected = False
        Me.Hide()
    End Sub

    Private Sub PictureBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Hide()
    End Sub

    Private Sub tlabel_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tlabel.MouseLeave
        If Me.from_load = True Then
            Exit Sub
        End If
        If Me.tlabel.Text = "" Then
            check_apply()
            Exit Sub
        End If
        If Me.set_tx = False Then
            Exit Sub
        End If
        If Me.orig_tx = Me.tlabel.Text And Me.edit_mode = True Then
            Exit Sub
        End If
        Dim i As Integer
        If exist_in_template(Me.tlabel.Text) = True Then
            Me.tlabel.Text = ""
        Else
            If Me.edit_mode = False Then
                For i = 0 To bc_am_in_context.insight_items.insight_items.Count - 1
                    If UCase(Me.tlabel.Text) = UCase(bc_am_in_context.insight_items.insight_items(i).desc) Then
                        Dim omsg As New bc_cs_message("Blue Curve", "Display item: " + Me.tlabel.Text + " already exists please use existing option!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                        Me.tlabel.Text = ""
                        Me.tlabel.Focus()
                        check_apply()
                        Exit Sub
                    End If
                Next
            End If
        End If
        check_apply()
    End Sub
    Private Function exist_in_template(ByVal tx As String) As Boolean
        REM check item doesnt already exist in template
        exist_in_template = False
        Dim i, j As Integer
        For i = 0 To osheet.bc_om_insightsections.Count - 1
            For j = 0 To osheet.bc_om_insightsections(i).rows.count - 1
                If UCase(osheet.bc_om_insightsections(i).rows(j).label) = UCase(tx) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Display item: " + Me.tlabel.Text + " already exists in period section " + osheet.bc_om_insightsections(i).name + "!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    exist_in_template = True
                    Exit Function
                End If
            Next
        Next
        For i = 0 To osheet.bc_om_insightsections_static.Count - 1
            For j = 0 To osheet.bc_om_insightsections_static(i).rows.count - 1
                If UCase(osheet.bc_om_insightsections_static(i).rows(j).label) = UCase(tx) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Display item: " + Me.tlabel.Text + " already exists in static section " + osheet.bc_om_insightsections_static(i).name + "!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    exist_in_template = True
                    Exit Function
                End If
            Next
        Next

    End Function
    Private Function link_code_exist_in_template(ByVal tx As String) As Boolean
        REM check item doesnt already exist in template
        link_code_exist_in_template = False
        Dim i, j As Integer
        For i = 0 To osheet.bc_om_insightsections.Count - 1
            For j = 0 To osheet.bc_om_insightsections(i).rows.count - 1
                If UCase(osheet.bc_om_insightsections(i).rows(j).link_code) = UCase(tx) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Link code: " + tx + " already exists in period section " + osheet.bc_om_insightsections(i).name + "!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    link_code_exist_in_template = True
                    Exit Function
                End If
            Next
        Next
        For i = 0 To osheet.bc_om_insightsections_static.Count - 1
            For j = 0 To osheet.bc_om_insightsections_static(i).rows.count - 1
                If UCase(osheet.bc_om_insightsections_static(i).rows(j).link_code) = UCase(tx) Then
                    Dim omsg As New bc_cs_message("Blue Curve", "Link code: " + tx + " already exists in static section " + osheet.bc_om_insightsections_static(i).name + "!", bc_cs_message.MESSAGE, False, False, "Yes", "No", True)
                    link_code_exist_in_template = True
                    Exit Function
                End If
            Next
        Next

    End Function

    Private Sub csymbol_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles csymbol.SelectedIndexChanged
        check_apply()
    End Sub
    Private Sub Crepeat_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Crepeat.SelectedIndexChanged
        check_apply()
    End Sub

    Private Sub cflexible_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cflexible.CheckedChanged
        check_apply()
    End Sub

    Private Sub ccont_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ccont.SelectedIndexChanged
        check_apply()
    End Sub

    Private Sub tlookup_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tlookup.TextChanged
        Dim i As Integer
        check_apply()
        For i = 0 To Me.lval.Items.Count - 1
            If LCase(Me.lval.Items(i).Text) = "look up" Then
                If Me.tlookup.Text <> "" Then
                    Me.lval.Items(i).Checked = True
                Else
                    Me.lval.Items(i).Checked = False
                End If
                Exit For
            End If
        Next
    End Sub
    Private Sub rexist_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rexist.CheckedChanged
        Me.cmon.Enabled = True
        Me.Cprice.Enabled = True
        If Me.rexist.Checked = True Then
            Me.cmon.Enabled = False
            Me.Cprice.Enabled = False
            Me.Cexist.Visible = True
            Me.tlabel.Visible = False
            fcontext.load_existing_items(Me)
            Me.tlabel.Text = ""
            Me.Bapply.Enabled = False
        End If
    End Sub

    Private Sub rnew_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rnew.CheckedChanged
        If Me.rnew.Checked = True Then
            Me.Cexist.Visible = False
            Me.tlabel.Visible = True
            Me.Cexist.SelectedIndex = -1
            Me.tlabel.Text = ""
            Me.Bapply.Enabled = False
        End If
        check_apply()
    End Sub

    Private Sub lval_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles lval.SelectedIndexChanged
        check_apply()
    End Sub

    Private Sub lval_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles lval.Click
        check_apply()
    End Sub

    Private Sub csymbol_TextChanged(ByVal sender As Object, ByVal e As System.EventArgs) Handles csymbol.TextChanged
        check_apply()
    End Sub

    Private Sub Cexist_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cexist.SelectedIndexChanged
        If Me.Cexist.SelectedIndex = -1 Then
            Exit Sub
        End If
        If Me.from_load = True Then
            Exit Sub
        End If
        If Me.Cexist.SelectedIndex <> -1 Then
            If exist_in_template(Me.Cexist.Text) = True Then
                Me.Cexist.SelectedIndex = -1
                Exit Sub
            End If
        End If
        Me.cmon.Checked = False
        If bc_am_in_context.filter_insight_items.insight_items(Me.Cexist.SelectedIndex).monetary = 1 Then
            Me.cmon.Checked = True
        End If
        Me.Cprice.Checked = False
        If bc_am_in_context.filter_insight_items.insight_items(Me.Cexist.SelectedIndex).price_related = 1 Then
            Me.Cprice.Checked = True
        End If
        check_apply()

    End Sub

    Private Sub Bapply_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Bapply.Click
        Me.cancel_selected = False
        Me.Hide()
    End Sub

    Private Sub bcancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles bcancel.Click
        Me.Hide()
    End Sub

    Private Sub Cexist_MouseHover(ByVal sender As Object, ByVal e As System.EventArgs) Handles Cexist.MouseHover
        Me.ToolTip1.SetToolTip(Cexist, Cexist.Text)
    End Sub

    Private Sub tlabel_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tlabel.TextChanged
        Me.set_tx = True
        check_apply()
    End Sub

    Private Sub tlinkcode_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tlinkcode.MouseLeave
        If Me.from_load = True Then
            Exit Sub
        End If
        If Trim(Me.tlinkcode.Text) = "" Then
            Exit Sub
        End If
        If Me.set_link = False Then
            Exit Sub
        End If
        If UCase(Me.orig_link) = UCase(Me.tlinkcode.Text) And Me.edit_mode = True Then
            Exit Sub
        End If
        If link_code_exist_in_template(Me.tlinkcode.Text) = True Then
            Me.tlinkcode.Text = ""
        End If
        check_apply()
    End Sub
    Private Sub tlinkcode_TextChanged_1(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles tlinkcode.TextChanged
        Me.set_link = True
    End Sub

    Private Sub cmon_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cmon.CheckedChanged
        check_apply()
    End Sub

    Private Sub Cprice_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Cprice.CheckedChanged
        check_apply()
    End Sub
End Class
