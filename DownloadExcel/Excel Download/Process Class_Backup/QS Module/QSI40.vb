Namespace Excel.ProcessClass
    ''' <summary>
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QSI40
        Inherits Excel.Interface.FileInterface

        Private _PrjCode As String
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
        Public WriteOnly Property ProjectCode() As String
            Set(ByVal value As String)
                _PrjCode = value.Trim
            End Set
        End Property


        Public Function SheetMapping() As String()
            Dim sqlStrs(8) As String
            Dim oPrjName As String
            Dim _dtCutOff As DateTime
            Dim _paraCutOff As String
            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID
            Try
                _dtCutOff = Convert.ToDateTime(_CutOffDate)
                _paraCutOff = _dtCutOff.ToString("yyyyMMdd")
            Catch ex As Exception

            End Try
            If _isBlankTemplate Then
                MyBase.FileType = MyBase.FunctionID & "_Blank"
                sqlStrs(0) = "Select * from QSI40_Blank('" & _PrjCode & "','" & _paraCutOff & "')"
                sqlStrs(1) = "Select '" & _PrjCode & "' as PrjCode1"
                sqlStrs(2) = "Select '" & _PrjCode & "' as PrjCode1"
                sqlStrs(3) = "Select '" & _PrjCode & "' as PrjCode2"
                sqlStrs(4) = "Select '" & _PrjCode & "' as PrjCode3"
                sqlStrs(5) = "Select '" & _PrjCode & "' as PrjCode4"
                sqlStrs(6) = "Select '" & _PrjCode & "' as PrjCode4"

            Else
                sqlStrs(0) = "Select * from QSI40_PR1('" & _PrjCode & "','" & _paraCutOff & "')"
                sqlStrs(1) = "Select * from QSI40_PR2('" & _PrjCode & "','" & _paraCutOff & "')"
                sqlStrs(2) = "Select * from QSI40_PR3('" & _PrjCode & "','" & _paraCutOff & "')"
                sqlStrs(3) = "Select * from QSI40_PR4('" & _PrjCode & "','" & _paraCutOff & "')"
                sqlStrs(4) = "Select * from QSI40_TD1('" & _PrjCode & "','" & _paraCutOff & "')"
                sqlStrs(5) = "Select * from QSI40_TD2('" & _PrjCode & "','" & _paraCutOff & "')"
                sqlStrs(6) = "Select * from QSI40_TD3('" & _PrjCode & "','" & _paraCutOff & "')"

            End If

            sqlStrs(7) = "select CardCode as List_EmployerCode, CardName as List_EmployerName from CPS_View_ClientList order by List_EmployerName"
            sqlStrs(8) = "select p.PrjCode as List_ProjectCode, p.U_ProjectFullName as List_ProjectName, p.U_SubsidiaryCode as List_CompanyCode, c.SubsiName as List_CompanyName From CPS_View_SubsidiaryList c, OPRJ p where p.U_SubsidiaryCode COLLATE Latin1_General_BIN2 = c.SubsiCode order by PrjCode"

            Return sqlStrs
        End Function

    End Class
End Namespace


