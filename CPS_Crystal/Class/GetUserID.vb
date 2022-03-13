Public Class GetUserID
    Private mCounter As Integer
    Public ReadOnly Property RetrieveUserList() As DataTable
        Get
            Dim mdt As DataTable
            mCounter = 0

            mdt = New DataTable
            mdt.Columns.Add(New DataColumn("DataValueField", GetType(String)))
            mdt.Columns.Add(New DataColumn("DataTextField", GetType(String)))


            Dim mSQLConn As SqlClient.SqlConnection = SQLServer_Connect()
            Dim mSQL As String = "SELECT USERID, USERNAME FROM CPS_CR_GetUser()"
            Dim mSQLReader As SqlClient.SqlDataReader = SQLServer_DataReader(mSQLConn, mSQL)

            Dim mdr As DataRow
            If (mSQLReader.HasRows = True) Then
                While mSQLReader.Read
                    mdr = mdt.NewRow()
                    mdr("DataValueField") = mSQLReader("USERID")
                    mdr("DataTextField") = mSQLReader("USERNAME")
                    mdt.Rows.Add(mdr)
                    mCounter += 1
                End While
            End If
            Return mdt
        End Get
    End Property
End Class
