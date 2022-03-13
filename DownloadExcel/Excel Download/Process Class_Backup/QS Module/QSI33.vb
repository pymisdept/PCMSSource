Namespace Excel.ProcessClass
    ''' <summary>
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QSI33
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
            Dim sqlStrs(3) As String
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID


            sqlStrs(0) = "Select * from QSI33_Form('" & _PrjCode & "')"

            sqlStrs(1) = "select Code as LIST_LNCode,[Name] as LIST_LNName from [@LNType]"
            sqlStrs(2) = "SELECT CODE AS LIST_PriceType, [NAME] AS LIST_PriceDesc FROM [@PRICETYPE]"

            sqlStrs(3) = "select Code AS LIST_ItemType,[NAME] AS LIST_ItemDesc from [@ITMTYPE]"

            Return sqlStrs
        End Function

    End Class
End Namespace


