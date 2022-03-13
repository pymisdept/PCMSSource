Public Class DocumentProperty

    Private vID As String
    Private vDocEntry As String
    Private vProjectCode As String

    Sub New()
        vID = ""
        vDocEntry = ""
        vProjectCode = ""
    End Sub

    Public Property ID() As String
        Get
            Return vID
        End Get
        Set(ByVal value As String)
            vID = value
        End Set
    End Property

    Public Property DocEntry() As String
        Get
            Return vDocEntry
        End Get
        Set(ByVal value As String)
            vDocEntry = value
        End Set
    End Property

    Public Property ProjectCode() As String
        Get
            Return vProjectCode
        End Get
        Set(ByVal value As String)
            vProjectCode = value
        End Set
    End Property

    Public Function getValueByName(ByVal name As String)
        If name = "ID" Then
            Return vID
        ElseIf name = "DocEntry" Then
            Return vDocEntry
        ElseIf name = "ProjectCode" Then
            Return vProjectCode
        ElseIf name = "PrjCode" Then
            Return vProjectCode
        Else
            Return ""
        End If
    End Function

End Class
