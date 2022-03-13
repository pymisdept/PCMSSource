Namespace Excel.ProcessClass
    ''' <summary>
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QSI31
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
            MyBase.FileType = MyBase.FunctionID


            sqlStrs(0) = "Select * from CPS_View_ProjectList"
            sqlStrs(1) = "select [Code] as List_StatusCode, Name as List_StatusDesc from [@PrjStatus] order by [Code]"
            sqlStrs(2) = "select PrcCode AS LIST_NATURECODE, U_PrcDesc AS LIST_NATURENAME  from OPRC" & _
                        " where DimCode=4 and U_FatherCode not in ('DM','FM','CHN')"

            Return sqlStrs
        End Function

    End Class
End Namespace


