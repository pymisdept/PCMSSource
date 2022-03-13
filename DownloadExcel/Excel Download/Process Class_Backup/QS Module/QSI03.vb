Namespace Excel.ProcessClass
    ''' <summary>
    ''' Karrson: New Function on 2009-08-25
    ''' Modified by Alice on 18 Oct 2009
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QSI03
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
            Dim sqlStrs(2) As String
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID

            sqlStrs(0) = "select * FROM QSI03_Form('" & _PrjCode.Replace("'", "''") & "')"

            sqlStrs(1) = "SELECT CODE AS LIST_ItemType, [NAME] AS LIST_ItemDesc FROM [@ITMTYPE]"

            'sqlStrs(2) = "SELECT COSTCODE as LIST_CostCode, COSTNAME AS List_COSTDESC " & _
            '            "FROM CPS_View_CostCodeList WHERE COSTTYPE IN ('E','M','N','P','PS','S')"

            sqlStrs(2) = "SELECT c.COSTCODE as LIST_CostCode, c.COSTNAME AS List_COSTDESC, a.U_ReportCode AS LIST_RptCode FROM CPS_View_CostCodeList c, OACT a " & _
                         "WHERE c.COSTTYPE IN ('E','M','N','P','PS','S') AND c.COSTCODE = a.AcctCode"

            Return sqlStrs
        End Function
    End Class
End Namespace