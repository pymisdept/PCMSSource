Namespace Excel.ProcessClass
    ''' <summary>
    ''' </summary>
    ''' <remarks></remarks>
    Public Class MA08
        Inherits Excel.Interface.FileInterface

        Private _SectionCode As String
        Private _CutOffDate As String
        Private _isBlankTemplate As Boolean

        Public Sub New(ByVal pRequestID As String)
            MyBase.New(New Microsoft.Office.Interop.Excel.Application)
            MyBase.RequestID = pRequestID
        End Sub

        Public WriteOnly Property CutOffDate() As String
            Set(ByVal value As String)
                _CutOffDate = value
            End Set
        End Property
        Public WriteOnly Property isBlankTemplate() As Boolean
            Set(ByVal value As Boolean)
                _isBlankTemplate = value
            End Set
        End Property

        Public WriteOnly Property SectionCode() As String
            Set(ByVal value As String)
                _SectionCode = value
            End Set
        End Property

        Public Function SheetMapping() As String()
            Dim sqlStrs(0) As String
            Dim oPrjName As String
            Dim _dtCutOff As DateTime
            Dim _paraCutOff As String

            MyBase.FileType = MyBase.FunctionID
            Try
                _dtCutOff = Convert.ToDateTime(_CutOffDate)
                _paraCutOff = _dtCutOff.ToString("yyyyMMdd")
            Catch ex As Exception

            End Try

            MyBase.FileType = MyBase.FunctionID
            sqlStrs(0) = "Select * from MA08_Form('" & _SectionCode & "','" & _paraCutOff & "')"

            


            Return sqlStrs
        End Function

    End Class
End Namespace


