Module SQLConnection

    Public Function SQLServer_DataReader(ByVal sqlconn As SqlClient.SqlConnection, ByVal strsql As String) As SqlClient.SqlDataReader
        Dim sqlcmd As New Data.SqlClient.SqlCommand(strsql, sqlconn)
        sqlcmd.CommandTimeout = 0
        Dim sqldr As Data.SqlClient.SqlDataReader = sqlcmd.ExecuteReader
        Return sqldr
    End Function

    Public Sub SQLServer_NonQuery(ByVal sqlconn As SqlClient.SqlConnection, ByVal strsql As String)
        Dim sqlcmd As SqlClient.SqlCommand = New SqlClient.SqlCommand(strsql, sqlconn)
        sqlcmd.CommandTimeout = 0

        sqlcmd.ExecuteNonQuery()
    End Sub

    Public Function SQLServer_Scalar(ByVal sqlconn As SqlClient.SqlConnection, ByVal strsql As String) As String
        Dim sqlcmd As SqlClient.SqlCommand = New SqlClient.SqlCommand(strsql, sqlconn)
        sqlcmd.CommandTimeout = 0
        Dim result As String = sqlcmd.ExecuteScalar()
        Return result
    End Function

    Public Function SQLServer_Connect() As SqlClient.SqlConnection
        Dim serverlocation As String = pServer
        Dim dbuser As String = pUserID
        Dim dbpass As String = pUserPW
        Dim database As String = pReportDataDB
        Dim connstr As String = "Server=" & serverlocation & ";uid=" & dbuser & ";pwd=" & dbpass & ";database=" & database & ";"
        Dim sqlconn As SqlClient.SqlConnection
        sqlconn = New Data.SqlClient.SqlConnection(connstr)
        sqlconn.Open()
        Return sqlconn
    End Function

    Public Sub SQLServer_KillConnect(ByVal sqlconn As SqlClient.SqlConnection)
        sqlconn.Close()
    End Sub
End Module
