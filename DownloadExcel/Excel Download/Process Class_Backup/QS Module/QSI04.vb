Namespace Excel.ProcessClass
    ''' <summary>
    ''' Karrson: New Function on 2009-08-25
    ''' Modified by Alice on 18 Oct 2009
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QSI04
        Inherits Excel.Interface.FileInterface

        Private _PrjCode As String, _SubContractorCode As String

        Public Sub New(ByVal pRequestID As String)
            MyBase.New(New Microsoft.Office.Interop.Excel.Application)
            MyBase.RequestID = pRequestID
        End Sub

        Public WriteOnly Property ProjectCode() As String
            Set(ByVal value As String)
                _PrjCode = value.Trim
            End Set
        End Property

        Public Property SubContractorCode() As String
            Get
                Return _SubContractorCode
            End Get
            Set(ByVal value As String)
                _SubContractorCode = value
            End Set
        End Property

        Public Function SheetMapping() As String()

            Dim sqlStrs(3) As String
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID

            sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & _PrjCode & " ' + ' - ' + '" & oPrjName.Replace("'", "''") & "' PrjDesc"

            'sqlStrs(1) = "SELECT COSTCODE as LIST_CostCode, COSTNAME AS List_COSTDESC " & _
            '            "FROM CPS_View_CostCodeList WHERE COSTTYPE IN ('E','M','N','P','PS','S','CC')"

            sqlStrs(1) = "SELECT c.COSTCODE as LIST_CostCode, c.COSTNAME AS List_COSTDESC, a.U_ReportCode AS LIST_RptCode FROM CPS_View_CostCodeList c, OACT a " & _
                         "WHERE c.COSTTYPE IN ('E','M','N','P','PS','S','CC') AND c.COSTCODE = a.AcctCode"

            sqlStrs(2) = "select Code AS LIST_ItemType,[NAME] AS LIST_ItemDesc from [@ITMTYPE]"

            'sqlStrs(3) = "SELECT CardCode as List_CardCode, CardName as List_CardName FROM CPS_View_SubContractorList"
            sqlStrs(3) = "SELECT * from PrjBP_List('" & _PrjCode & "')"


            Return sqlStrs
        End Function
    End Class
End Namespace

