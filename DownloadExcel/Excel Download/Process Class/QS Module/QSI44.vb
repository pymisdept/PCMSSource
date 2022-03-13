Namespace Excel.ProcessClass

    Public Class QSI44
        Inherits Excel.Interface.FileInterface

        Private _PrjCode As String
        Private _CustCode As String

        Public Sub New(ByVal pRequestID As String)
            MyBase.New(New Microsoft.Office.Interop.Excel.Application)
            MyBase.RequestID = pRequestID
        End Sub

        Public WriteOnly Property ProjectCode() As String
            Set(ByVal value As String)
                _PrjCode = value.Trim
            End Set
        End Property

        Public WriteOnly Property CustCode() As String
            Set(ByVal value As String)
                _CustCode = value.Trim
            End Set
        End Property

        Public Function SheetMapping() As String()
            Dim sqlStrs(3) As String
            Dim oPrjName As String
            Dim oCustName As String

            oPrjName = getPrjName(_PrjCode)
            oCustName = getCustName(_CustCode)
            MyBase.FileType = MyBase.FunctionID

            sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & _PrjCode & " ' + ' - ' + '" & oPrjName.Replace("'", "''") & "' PrjDesc"
            sqlStrs(1) = "select * from QSI44_Form ('" & _PrjCode & "','" & _CustCode & "')"
            sqlStrs(2) = "select CardCode as List_CardCode, CardName as List_CardName From CPS_View_ClientList"
            sqlStrs(3) = "select '" & oCustName & "' CustName "

            Return sqlStrs
        End Function

    End Class
End Namespace