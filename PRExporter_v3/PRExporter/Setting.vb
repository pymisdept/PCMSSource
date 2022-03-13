Imports System.Xml

Public Class Setting

    Private XMLFile As String

    Private _CurrentUser As String
    Private _CurrentUserIsS As Boolean
    Private _CurrentPrjList As String

    Private _FEServer As String
    Private _FEDatabase As String
    Private _FEUsername As String
    Private _FEPassword As String

    Private _BEServer As String
    Private _BEDatabase As String
    Private _BEUsername As String
    Private _BEPassword As String

    Private _DefaultServer As String
    Private _ShowTesting As Boolean
    Private _ExportType As String
    Private _LogExportResult As Boolean

    Public Sub New(ByVal xml As String)
        XMLFile = xml
    End Sub

    Public Property FEServer() As String
        Get
            Return _FEServer
        End Get

        Set(ByVal value As String)
            _FEServer = value
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

    Public Property FEUsername() As String
        Get
            Return _FEUsername
        End Get

        Set(ByVal value As String)
            _FEUsername = value
        End Set
    End Property

    Public Property FEPassword() As String
        Get
            Return _FEPassword
        End Get

        Set(ByVal value As String)
            _FEPassword = value
        End Set
    End Property

    Public Property BEServer() As String
        Get
            Return _BEServer
        End Get

        Set(ByVal value As String)
            _BEServer = value
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

    Public Property BEUsername() As String
        Get
            Return _BEUsername
        End Get

        Set(ByVal value As String)
            _BEUsername = value
        End Set
    End Property

    Public Property BEPassword() As String
        Get
            Return _BEPassword
        End Get

        Set(ByVal value As String)
            _BEPassword = value
        End Set
    End Property

    Public Property DefaultServer() As String
        Get
            Return _DefaultServer
        End Get

        Set(ByVal value As String)
            _DefaultServer = value
        End Set
    End Property

    Public Property ShowTesting() As Boolean
        Get
            Return _ShowTesting
        End Get

        Set(ByVal value As Boolean)
            _ShowTesting = value
        End Set
    End Property

    Public Property ExportType() As String
        Get
            Return _ExportType
        End Get

        Set(ByVal value As String)
            _ExportType = value
        End Set
    End Property

    Public Property LogExportResult() As Boolean
        Get
            Return _LogExportResult
        End Get

        Set(ByVal value As Boolean)
            _LogExportResult = value
        End Set
    End Property

    Public Function loadSetting()
        Try
            Dim doc As New XmlDocument()
            doc.Load(XMLFile)
            Dim nodelist As XmlNodeList = doc.SelectNodes("Settings/Setting")
            For Each node As XmlElement In nodelist
                If node.ChildNodes(0).InnerText = "DefaultServer" Then
                    DefaultServer = node.ChildNodes(1).InnerText
                ElseIf node.ChildNodes(0).InnerText = "ShowTesting" Then
                    ShowTesting = node.ChildNodes(1).InnerText
                ElseIf node.ChildNodes(0).InnerText = "ExportType" Then
                    ExportType = node.ChildNodes(1).InnerText
                ElseIf node.ChildNodes(0).InnerText = "LogExportResult" Then
                    LogExportResult = node.ChildNodes(1).InnerText
                End If
            Next
        Catch ex As Exception
            Return False
        End Try
        Return True
    End Function

    Public Property CurrentUser() As String
        Get
            Return _CurrentUser
        End Get

        Set(ByVal value As String)
            _CurrentUser = value
        End Set
    End Property

    Public Property CurrentUserIsS() As Boolean
        Get
            Return _CurrentUserIsS
        End Get

        Set(ByVal value As Boolean)
            _CurrentUserIsS = value
        End Set
    End Property

    Public Property CurrentPrjList() As String
        Get
            Return _CurrentPrjList
        End Get

        Set(ByVal value As String)
            _CurrentPrjList = value
        End Set
    End Property

End Class
