Namespace Excel.ProcessClass
    Public Class PUI01
        Inherits Excel.Interface.FileInterface

        Private _PrjCode As String

        Public Sub New(ByVal pRequestID As String)
            MyBase.New(New Microsoft.Office.Interop.Excel.Application)
            MyBase.RequestID = pRequestID
        End Sub

        Public WriteOnly Property ProjectCode() As String
            Set(ByVal value As String)
                _PrjCode = value.Trim
            End Set
        End Property

        Public Function SheetMapping() As String()
            Dim sqlStrs(5) As String
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID

            sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & oPrjName.Replace("'", "''") & "' PRJNAME "

            sqlStrs(1) = "SELECT COSTCODE as LIST_CostCode, COSTNAME AS List_COSTDESC " & _
                        "FROM CPS_View_CostCodeList WHERE COSTTYPE IN ('E','M','N','P','PS','S')"

            sqlStrs(2) = "select CardCode as List_SCCode, CardName as List_SCName From CPS_View_SubContractorList"
        
            sqlStrs(3) = "select Code as List_RequestCode,U_ReasonDesc as List_RequestName from [@reasoncode] where U_ReasonType='MR'"
            sqlStrs(4) = "select ItemCode as List_ItemCode,ItemName as List_ItemName From CPS_View_MaterialList"
            sqlStrs(5) = "select MaterialType as List_MaterialType, MaterialDesc as List_MaterialDesc From CPS_View_MaterialTypeList"
            Return sqlStrs
        End Function

    End Class
End Namespace

