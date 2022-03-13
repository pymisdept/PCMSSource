Imports CPS.SQL.Condition

Namespace Datatable.PCMS
    Public Class DLNMGR
        Inherits CPS.SQL.Interface.RecordSet

#Region "Constanst Value"
        Private ReadOnly CheckPoint As String = "Sync_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

        Public Const TableName As String = "DLNMGR"
        Public Const _RequestID As String = "id"
        Public Const _Parameter As String = "Parameter"
        Public Const _Ready As String = "Allowdownload"
        Public Const _Data As String = "FileData"
        Public Const _Type As String = "FileType"
        Public Const _CreateDate As String = "CreateDate"
        Public Const _CreateUser As String = "CreateUser"
        Public Const _DocStatus As String = "DocStatus"
        Public Const _Project As String = "Project"

#End Region

#Region "Define New"
        Public Sub New()
            MyBase.New(TableName)
            Me.add(_RequestID)
            Me.add(_Parameter)
            Me.add(_Ready)
            Me.add(_Data)
            Me.add(_Type)
            Me.add(_CreateDate)
            Me.add(_CreateUser)
            Me.add(_DocStatus)
            Me.add(_Project)
        End Sub
#End Region

#Region "Get Filter Information Structure"
        Public Sub getDownloadRequest(ByVal pID As String)
            Dim oCondition As Condition

            oCondition = New Condition
            oCondition.BracketOpenNum = 1
            oCondition.Alias = DLNMGR._RequestID
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pID
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

        End Sub

        Public Sub getDownloadRequest()
            Dim oCondition As Condition

            oCondition = New Condition
            oCondition.BracketOpenNum = 1
            oCondition.Alias = DLNMGR._Data
            oCondition.Operation = Condition.eOperation.op_IS_NULL
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

        End Sub
#End Region

#Region "Update Status in DLNMGR"
        'Public Sub Update(ByVal pExcelData As Byte())
        Public Sub Update()
            Dim oSqlDataAdapter As SqlClient.SqlDataAdapter
            Dim oDataTable As System.Data.DataTable
            Dim oSqlStr As String

            oSqlStr = Me.SelectQuery & " " & Me.filterQuery
            oSqlDataAdapter = oPCMSConnection.Adapter(oSqlStr)

            Using New SqlClient.SqlCommandBuilder(oSqlDataAdapter)
                oDataTable = New System.Data.DataTable

                'Fill in data into System Datatable
                oSqlDataAdapter.Fill(dataTable:=oDataTable)
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    'oDataRow(Datatable.PCMS.DLNMGR._Data) = pExcelData
                    oDataRow(Datatable.PCMS.DLNMGR._Ready) = "Y"
                Next
                oSqlDataAdapter.Update(oDataTable)
            End Using
        End Sub
#End Region

    End Class
End Namespace