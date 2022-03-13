Namespace Excel.ProcessClass
    Public Class PUI06
        Inherits Excel.Interface.FileInterface

        Private _PrjCode As String
        Private _PANum As String

        Public Sub New(ByVal pRequestID As String)
            MyBase.New(New Microsoft.Office.Interop.Excel.Application)
            MyBase.RequestID = pRequestID
        End Sub

        Public WriteOnly Property ProjectCode() As String
            Set(ByVal value As String)
                _PrjCode = value.Trim
            End Set
        End Property

        Public WriteOnly Property PANumber() As String
            Set(ByVal value As String)
                _PANum = value.Trim
            End Set
        End Property

        Public Function SheetMapping() As String()
            Dim sqlStrs(2) As String
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID

            sqlStrs(0) = "SELECT * FROM CPS_VIEW_WS_PUI04_FORM WHERE PRJCODE = '" & _PrjCode & _
                         "' AND PANUM = '" & _PANum & "'"

            sqlStrs(1) = "SELECT ITEMCODE AS LIST_ITEMCODE,ITEMNAME AS LIST_ITEMNAME FROM CPS_VIEW_MATERIALLIST"
            sqlStrs(2) = "SELECT COSTCODE LIST_CostCODE,COSTNAME  AS LIST_CostNAME FROM CPS_VIEW_CostCodeList"

            

            Return sqlStrs
        End Function

    End Class
End Namespace

