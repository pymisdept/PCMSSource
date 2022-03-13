Namespace Excel.ProcessClass

    Public Class QSI17
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
            Dim sqlStrs(6) As String
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID

            sqlStrs(0) = "select * from QSI17_Cover ('" & _PrjCode & "')"
            sqlStrs(1) = "select * from QSI17_Work ('" & _PrjCode & "')"
            sqlStrs(2) = "select * from QSI17_MOS ('" & _PrjCode & "')"
            sqlStrs(3) = "select * from QSI17_EVO ('" & _PrjCode & "')"
            sqlStrs(4) = "select * from QSI17_EDW ('" & _PrjCode & "')"
            sqlStrs(5) = "select * from QSI17_ECL ('" & _PrjCode & "')"
            sqlStrs(6) = "select * from QSI17_Bill ('" & _PrjCode & "')"




            Return sqlStrs
        End Function

    End Class
End Namespace