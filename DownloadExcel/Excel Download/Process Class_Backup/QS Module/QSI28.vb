Namespace Excel.ProcessClass
    ''' <summary>
    ''' Created by Alice on 01 Nov 2009
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QSI28
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
            Dim sqlStrs(0) As String
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID

            sqlStrs(0) = "SELECT * FROM CPS_View_WS_QSI28_Form WHERE PRJCODE = '" & _PrjCode & "'"


            Return sqlStrs
        End Function

    End Class
End Namespace