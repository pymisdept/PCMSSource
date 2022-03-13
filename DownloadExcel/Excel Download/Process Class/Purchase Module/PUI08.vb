Namespace Excel.ProcessClass
    Public Class PUI08
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

        Public Function SheetMapping() As String()
            Dim sqlStrs(2) As String
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID

            sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & oPrjName.Replace("'", "''") & "' PRJDESC "
            sqlStrs(1) = "select ItmsGrpCod AS GroupCode, U_LngGroups AS GroupName from OITB where U_LngGroups IS NOT NULL order by U_LngGroups"
            sqlStrs(2) = "select CardCode as List_CardCode, CardName as List_CardName From CPS_View_SubContractorList"


            Return sqlStrs
        End Function

    End Class
End Namespace

