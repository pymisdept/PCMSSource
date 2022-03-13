Namespace Excel.ProcessClass
    ''' <summary>
    ''' Created by Alice on 21 Oct 2009
    ''' </summary>
    ''' <remarks></remarks>
    Public Class SEI01
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
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID


            sqlStrs(0) = String.Format("Select '{1}' as PrjDesc,'{2}' as PrjCode,List_CardCode as BPCode from SEI01_ProjectBP('{0}')", _PrjCode.Replace("'", "''"), oPrjName.Replace("'", "''"), _PrjCode.Replace("'", "''"))
            sqlStrs(1) = "Select * From SEI01_Project()"
            sqlStrs(2) = "Select * from SEI01_BP()"
            'sqlStrs(3) = "Select * from SEI01_User()"


            Return sqlStrs
        End Function

    End Class
End Namespace


