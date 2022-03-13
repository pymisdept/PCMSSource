Namespace Excel.ProcessClass
    ''' <summary>
    ''' Karrson: New Function on 2009-08-25
    ''' Modified by Alice on 18 Oct 2009
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QSI12
        Inherits Excel.Interface.FileInterface

        Private _PrjCode As String, _SubContractNo As String, _SubContractorCode As String

        Public Sub New(ByVal pRequestID As String)
            MyBase.New(New Microsoft.Office.Interop.Excel.Application)
            MyBase.RequestID = pRequestID
        End Sub

        Public WriteOnly Property ProjectCode() As String
            Set(ByVal value As String)
                _PrjCode = value.Trim
            End Set
        End Property

        Public Property SubContractNo() As String
            Get
                Return _SubContractNo
            End Get
            Set(ByVal value As String)
                _SubContractNo = value
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

        Public Function SheetMapping(ByVal serverType As String) As String()
            Dim sqlStrs(13) As String
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID

            Dim ccno As String
            ccno = _SubContractNo.Replace("'", "''")
            ccno = ccno.Substring(ccno.IndexOf("/") + 1)
            ccno = "CC." & ccno

            sqlStrs(0) = "select *, '" & serverType & "' AS PCMSSERV from QSI12_Cert('" & _PrjCode.Replace("'", "''") & "', " & _
                         "'" & _SubContractorCode & "', " & _
                         "'" & _SubContractNo.Replace("'", "''") & "') "


            sqlStrs(1) = "select * from QSI12_Work('" & _PrjCode.Replace("'", "''") & "','" & _SubContractNo.Replace("'", "''") & "')"

            sqlStrs(2) = "select * from QSI12_MOS('" & _PrjCode.Replace("'", "''") & "','" & _SubContractNo.Replace("'", "''") & "')"
            'sqlStrs(3) = "select * from QSI12_EVO('" & _PrjCode.Replace("'", "''") & "','" & _SubContractNo.Replace("'", "''") & "')"
            'Karrson: Sorting
            sqlStrs(3) = "select * from QSI12_EVO('" & _PrjCode.Replace("'", "''") & "','" & _SubContractNo.Replace("'", "''") & "') ORDER BY EVO_SubLineNum asc"


            sqlStrs(4) = "select * from QSI12_IVO('" & _PrjCode.Replace("'", "''") & "','" & _SubContractNo.Replace("'", "''") & "')"



            sqlStrs(5) = "select * from QSI12_CCVO('" & _PrjCode.Replace("'", "''") & "','" & _SubContractNo.Replace("'", "''") & "')"


            sqlStrs(6) = "select * from QSI12_EDW('" & _PrjCode.Replace("'", "''") & "','" & _SubContractNo.Replace("'", "''") & "') ORDER BY EDW_SubLineNum asc"
            sqlStrs(7) = "select * from QSI12_IDW('" & _PrjCode.Replace("'", "''") & "','" & _SubContractNo.Replace("'", "''") & "')"
            sqlStrs(8) = "select * from QSI12_CCDW('" & _PrjCode.Replace("'", "''") & "','" & _SubContractNo.Replace("'", "''") & "')"


            sqlStrs(9) = "select * from QSI12_CC('" & _PrjCode.Replace("'", "''") & "','" & _SubContractNo.Replace("'", "''") & "')"

            sqlStrs(10) = "select * from QSI12_DAP('" & _PrjCode.Replace("'", "''") & "','" & _SubContractNo.Replace("'", "''") & "')"
            sqlStrs(11) = "select * from QSI12_Approval('" & _PrjCode.Replace("'", "''") & "','" & _SubContractNo.Replace("'", "''") & "')"

            'sqlStrs(12) = "SELECT COSTCODE as LIST_CostCode, COSTNAME AS List_COSTDESC " & _
            '                "FROM CPS_View_CostCodeList WHERE COSTTYPE IN ('E','M','N','P','PS','S','CC')"
            'sqlStrs(11) = "SELECT CardCode as List_CardCode, CardName as List_CardName FROM CPS_View_SubContractorList"

            sqlStrs(12) = "SELECT c.COSTCODE as LIST_CostCode, c.COSTNAME AS List_COSTDESC, a.U_ReportCode AS LIST_RptCode FROM CPS_View_CostCodeList c, OACT a " & _
                         "WHERE c.COSTTYPE IN ('E','M','N','P','PS','S','CC') AND c.COSTCODE = a.AcctCode AND c.COSTCODE <> '" & ccno & "'"

            sqlStrs(13) = "SELECT * from PrjBP_List('" & _PrjCode & "')"


            Return sqlStrs
        End Function

    End Class
End Namespace