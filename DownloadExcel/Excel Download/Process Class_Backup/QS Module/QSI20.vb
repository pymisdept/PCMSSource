Namespace Excel.ProcessClass
    ''' <summary>
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QSI20
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


        Public Function SheetMapping() As String()
            Dim sqlStrs(4) As String
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID


            sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & _PrjCode & " ' + ' - ' + '" & oPrjName & "' PrjDesc"
            sqlStrs(1) = "select * from QSI20_Exten('" & _PrjCode.Replace("'", "''") & "')"
            sqlStrs(2) = "select * from QSI20_Pro('" & _PrjCode.Replace("'", "''") & "')"
            sqlStrs(3) = "select * from QSI20_Fin('" & _PrjCode.Replace("'", "''") & "')"
            sqlStrs(4) = "select * from QSI20_Cash('" & _PrjCode.Replace("'", "''") & "')"

            


            Return sqlStrs
        End Function

    End Class
End Namespace