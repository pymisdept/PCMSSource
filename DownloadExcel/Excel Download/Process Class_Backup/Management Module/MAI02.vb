Namespace Excel.ProcessClass
    ''' <summary>
    ''' Created by Alice on 21 Oct 2009
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MAI02
        Inherits Excel.Interface.FileInterface

        Private _PrjCode As String, _SectionCode As String

        Public Sub New(ByVal pRequestID As String)
            MyBase.New(New Microsoft.Office.Interop.Excel.Application)
            MyBase.RequestID = pRequestID
        End Sub

        Public WriteOnly Property ProjectCode() As String
            Set(ByVal value As String)
                _PrjCode = value.Trim
            End Set
        End Property
        Public Property SectionCode() As String
            Get
                Return _SectionCode
            End Get
            Set(ByVal value As String)
                _SectionCode = value
            End Set
        End Property

        Public Function SheetMapping() As String()
            Dim sqlStrs(0) As String
            
            MyBase.FileType = MyBase.FunctionID


            sqlStrs(0) = "Select * from MAI02_Form('" & _SectionCode & "')"


            Return sqlStrs
        End Function

    End Class
End Namespace


