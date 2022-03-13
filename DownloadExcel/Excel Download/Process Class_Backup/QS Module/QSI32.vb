Namespace Excel.ProcessClass
    ''' <summary>
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QSI32
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
            Dim sqlStrs(1) As String
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID


            sqlStrs(0) = "Select * from QSI32_Form ('" & _PrjCode & "')"
            sqlStrs(1) = "SELECT COSTCODE as LIST_CostCode, COSTNAME AS List_COSTDESC " & _
                       "FROM CPS_View_CostCodeList WHERE COSTTYPE IN ('E','M','N','P','PS','S')"



            Return sqlStrs
        End Function

    End Class
End Namespace


