Namespace Excel.ProcessClass
    ''' <summary>
    ''' Karrson: New Function on 2009-08-25
    ''' Modified by Alice on 18 Oct 2009
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QSI42
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

        Public Function SheetMapping() As String()

            Dim sqlStrs(5) As String
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID

            sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & _PrjCode.Replace("'", "''") & " ' + ' - ' + '" & oPrjName.Replace("'", "''") & "' PrjDesc"


            sqlStrs(1) = "Select * from QSI42_VO ('" & _PrjCode.Replace("'", "''") & "')"
            sqlStrs(2) = "Select * from QSI42_DW ('" & _PrjCode.Replace("'", "''") & "')"
            sqlStrs(3) = "Select * from QSI42_Claims ('" & _PrjCode.Replace("'", "''") & "')"
            sqlStrs(4) = "SELECT COSTCODE as LIST_CostCode, COSTNAME AS List_COSTDESC " & _
                        "FROM CPS_View_CostCodeList WHERE COSTTYPE IN ('E','M','N','P','PS','S')"
            sqlStrs(5) = "SELECT * from PrjBP_List('" & _PrjCode & "')"


            Return sqlStrs
        End Function
    End Class
End Namespace

