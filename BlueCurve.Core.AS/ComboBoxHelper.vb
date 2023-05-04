Imports System.windows.Forms

Public Class ComboBoxHelper
    Inherits CollectionBase

    Public Sub New()
        Add(-1, "")
    End Sub

    Public Sub New(ByVal code As String)
        Add("", "")
    End Sub

    Default Public ReadOnly Property item(ByVal index As Integer) As ComboBoxItem
        Get
            Return CType(Me.List(index), ComboBoxItem)
        End Get
    End Property

    Public Overloads Sub Add(ByVal id As Integer, ByVal name As String)
        Dim cbi As New ComboBoxItem

        cbi.ID = id
        cbi.Name = name

        list.Add(cbi)

    End Sub

    Public Overloads Sub Add(ByVal code As String, ByVal name As String)
        Dim cbi As New ComboBoxItem

        cbi.Code = code
        cbi.Name = name

        list.Add(cbi)

    End Sub

    Public Sub Sort()

        innerlist.Sort(New SimpleComparer("Name", SortOrder.Ascending))

    End Sub

    Private Class SimpleComparer
        Implements IComparer

        Private _propertyToSort As String
        Private _sortOrder As SortOrder

        Public Sub New(ByVal propertyToSort As String, ByVal sortOrder As SortOrder)

            MyBase.new()
            _propertyToSort = propertyToSort
            _sortOrder = sortOrder

        End Sub

        Private Function Compare(ByVal x As Object, ByVal y As Object) As Integer _
            Implements System.Collections.IComparer.Compare
            Dim prop As Reflection.PropertyInfo = x.GetType.GetProperty(Me.PropertyToSort)

            If Me.SortOrder = SortOrder.None OrElse prop.GetValue(x, Nothing) = _
                prop.GetValue(y, Nothing) Then
                Return 0
            Else
                If LCase(prop.GetValue(x, Nothing)) > LCase(prop.GetValue(y, Nothing)) Then
                    If Me.SortOrder = System.Windows.Forms.SortOrder.Ascending Then
                        Return 1
                    Else
                        Return -1
                    End If
                Else
                    If Me.SortOrder = System.Windows.Forms.SortOrder.Ascending Then
                        Return -1
                    Else
                        Return 1
                    End If
                End If
            End If
        End Function

        Private Property SortOrder() As SortOrder
            Get
                Return _sortOrder
            End Get
            Set(ByVal Value As SortOrder)
                _sortOrder = Value
            End Set
        End Property

        Private Property PropertyToSort() As String
            Get
                Return _propertyToSort
            End Get
            Set(ByVal Value As String)
                _propertyToSort = Value
            End Set
        End Property
    End Class


    Public Class ComboBoxItem
        Private _id As Integer
        Private _code As String
        Private _name As String

        Public Property ID() As Integer
            Get
                ID = _id
            End Get
            Set(ByVal Value As Integer)
                _id = Value
            End Set
        End Property
        Public Property Code() As String
            Get
                Code = _code
            End Get
            Set(ByVal Value As String)
                _code = Value
            End Set
        End Property

        Public Property Name() As String
            Get
                Name = _name
            End Get
            Set(ByVal Value As String)
                _name = Value
            End Set
        End Property

        Public Overrides Function ToString() As String

            Return Name

        End Function

    End Class

End Class


