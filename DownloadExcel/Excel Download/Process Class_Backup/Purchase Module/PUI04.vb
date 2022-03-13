Namespace Excel.ProcessClass
    Public Class PUI04
        Inherits Excel.Interface.FileInterface

        Private _PrjCode As String, _PANum As String

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

            'sqlStrs(0) = "SELECT BaseLine as Base_Line,PrjCode as PrjCode,SMRDate,PRJNAME, MRNO AS SMRNO,ITEMCODE AS ITEMCODE,UNIT AS UOM,PRICE AS RATE,COSTCODE AS ACCTCODE,PANUM AS PANUM,CARDCODE AS CARDCODE FROM CPS_VIEW_WS_PUI04_FORM WHERE PRJCODE = '" & Me._PrjCode & "' AND PANUM = '" & Me._PANum & "'"
            sqlStrs(0) = "SELECT * FROM CPS_VIEW_WS_PUI04_FORM WHERE PRJCODE = '" & _PrjCode & _
                         "' AND PANUM = '" & _PANum & "'"
            'sqlStrs(1) = "SELECT PYMNTGROUP AS List_PYMNTGROUP FROM OCTG"
            sqlStrs(1) = "SELECT ITEMCODE AS LIST_ITEMCODE,ITEMNAME AS LIST_ITEMNAME FROM CPS_VIEW_MATERIALLIST"
            'sqlStrs(3) = "SELECT CARDCODE AS LIST_SCCODE,CARDNAME AS LIST_SCNAME FROM CPS_VIEW_SUBCONTRACTORLIST"
            'sqlStrs(4) = "SELECT COSTCODE LIST_ACCTCODE,COSTNAME  AS LIST_ACCTNAME FROM CPS_VIEW_WS_PUI03_COSTCODELIST"
            sqlStrs(2) = "SELECT COSTCODE LIST_CostCODE,COSTNAME  AS LIST_CostNAME FROM CPS_VIEW_CostCodeList"
            'sqlStrs(5) = "SELECT PRCCODE AS LIST_SUBSICODE,PRCNAME AS LIST_SUBSINAME FROM OPRC WHERE DIMCODE = 1 AND ISNULL(LOCKED,'N') = 'N'"
            'sqlStrs(6) = "SELECT CODE AS LIST_REQUESTCODE,[NAME] AS LIST_REQUESTNAME FROM [@REASONCODE]"


            Return sqlStrs
        End Function

    End Class
End Namespace