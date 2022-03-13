Imports System.Xml

Public Class Config

    Private XMLFile As String
    Private _FEDataSource As String
    Private _FEDatabase As String
    Private _BEDataSource As String
    Private _BEDatabase As String
    Private _LogSQL As Boolean

    Public Sub New(ByVal xml As String)
        XMLFile = xml
    End Sub

    Public Property FEDataSource() As String
        Get
            Return _FEDataSource
        End Get

        Set(ByVal value As String)
            _FEDataSource = value
        End Set
    End Property

    Public Property FEDatabase() As String
        Get
            Return _FEDatabase
        End Get

        Set(ByVal value As String)
            _FEDatabase = value
        End Set
    End Property

    Public Property BEDataSource() As String
        Get
            Return _BEDataSource
        End Get

        Set(ByVal value As String)
            _BEDataSource = value
        End Set
    End Property

    Public Property BEDatabase() As String
        Get
            Return _BEDatabase
        End Get

        Set(ByVal value As String)
            _BEDatabase = value
        End Set
    End Property

    Public Property LogSQL() As Boolean
        Get
            Return _LogSQL
        End Get

        Set(ByVal value As Boolean)
            _LogSQL = value
        End Set
    End Property

    Public Function loadSetting()
        Try
            Dim doc As New XmlDocument()
            doc.Load(XMLFile)
            Dim nodelist As XmlNodeList = doc.SelectNodes("Settings/Setting")
            For Each node As XmlElement In nodelist
                If node.ChildNodes(0).InnerText = "FEDataSource" Then
                    FEDataSource = node.ChildNodes(1).InnerText
                ElseIf node.ChildNodes(0).InnerText = "FEDatabase" Then
                    FEDatabase = node.ChildNodes(1).InnerText
                ElseIf node.ChildNodes(0).InnerText = "BEDataSource" Then
                    BEDataSource = node.ChildNodes(1).InnerText
                ElseIf node.ChildNodes(0).InnerText = "BEDatabase" Then
                    BEDatabase = node.ChildNodes(1).InnerText
                ElseIf node.ChildNodes(0).InnerText = "LogSQL" Then
                    LogSQL = node.ChildNodes(1).InnerText
                End If
            Next
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function
End Class
