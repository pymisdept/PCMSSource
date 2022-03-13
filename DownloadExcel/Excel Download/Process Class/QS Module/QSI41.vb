Namespace Excel.ProcessClass
    ''' <summary>
    ''' Karrson: New Function on 2009-08-25
    ''' Modified by Alice on 18 Oct 2009
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QSI41
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

            Dim sqlStrs(0) As String
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID

            sqlStrs(0) = "Select * from QSI41_Form('" & _PrjCode.Replace("'", "''") & "', " & _
                         "'" & _SubContractorCode & "', " & _
                         "'" & _SubContractNo.Replace("'", "''") & "') "

            Return sqlStrs
        End Function
    End Class
End Namespace

