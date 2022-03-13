Public Class GetProjectCode
    Private mCounter As Integer
    Public ReadOnly Property RetrieveProjectList() As DataTable
        Get
            Dim mdt As DataTable
            mCounter = 0

            mdt = New DataTable
            mdt.Columns.Add(New DataColumn("DataValueField", GetType(String)))
            mdt.Columns.Add(New DataColumn("DataTextField", GetType(String)))


            Dim mSQLConn As SqlClient.SqlConnection = SQLServer_Connect()
            Dim mSQL As String = "SELECT PrjCode, PrjName FROM CPS_CR_GetProject()"
            Dim mSQLReader As SqlClient.SqlDataReader = SQLServer_DataReader(mSQLConn, mSQL)

            Dim mdr As DataRow
            If (mSQLReader.HasRows = True) Then
                While mSQLReader.Read
                    mdr = mdt.NewRow()
                    mdr("DataValueField") = mSQLReader("PrjCode")
                    mdr("DataTextField") = mSQLReader("PrjName")
                    mdt.Rows.Add(mdr)
                    mCounter += 1
                End While
            End If
            Return mdt
        End Get
    End Property
End Class
