Public Class CPSMapping

    Private vTableName As String
    Private vTableType As String
    Private vTableKey As String
    Private vFields As String
    Private vFieldCount As Integer
    Private vFieldType As List(Of String)
    Private vIsHeader As Boolean
    Private vRowType As String

    Sub New()
        vTableName = ""
        vTableType = ""
        vTableKey = ""
        vFields = ""
        vFieldCount = 0
        vFieldType = New List(Of String)
        vIsHeader = False
        vRowType = ""
    End Sub

    Public Property TableName() As String
        Get
            Return vTableName
        End Get
        Set(ByVal value As String)
            vTableName = value
        End Set
    End Property

    Public Property TableType() As String
        Get
            Return vTableType
        End Get
        Set(ByVal value As String)
            vTableType = value
        End Set
    End Property

    Public Property TableKey() As String
        Get
            Return vTableKey
        End Get
        Set(ByVal value As String)
            vTableKey = value
        End Set
    End Property

    Public Property Fields() As String
        Get
            Return vFields
        End Get
        Set(ByVal value As String)
            vFields = value
        End Set
    End Property


    Public Property FieldCount() As Integer
        Get
            Return vFieldCount
        End Get
        Set(ByVal value As Integer)
            vFieldCount = value
        End Set
    End Property

    Public Property FieldType() As List(Of String)
        Get
            Return vFieldType
        End Get
        Set(ByVal value As List(Of String))
            vFieldType = value
        End Set
    End Property

    Public Property IsHeader() As Boolean
        Get
            Return vIsHeader
        End Get
        Set(ByVal value As Boolean)
            vIsHeader = value
        End Set
    End Property

    Public Property RowType() As String
        Get
            Return vRowType
        End Get
        Set(ByVal value As String)
            vRowType = value
        End Set
    End Property

    Public Function ShallowCopy() As CPSMapping
        Return DirectCast(Me.MemberwiseClone(), CPSMapping)
    End Function
End Class
