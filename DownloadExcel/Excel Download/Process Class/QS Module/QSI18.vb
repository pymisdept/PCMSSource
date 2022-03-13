Namespace Excel.ProcessClass
    ''' <summary>
    ''' Created by Alice on 01 Nov 2009
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QSI18
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
            Dim sqlStrs() As String = Nothing
            Dim oPrjName As String
            Dim oPrjNature As String

            oPrjName = getPrjName(_PrjCode)
            oPrjNature = getPrjNature(_PrjCode)


           

            Select Case oPrjNature


                    
                Case Is = "BLDG"


                    ReDim sqlStrs(1)
                    MyBase.FileType = MyBase.FunctionID & "_" & "Bill"

                    sqlStrs(0) = "select * from QSI18_Cover('" & _PrjCode & "')"
                    sqlStrs(1) = "select * from QSI18_Bill('" & _PrjCode & "')"


                Case Is = "CVL"

                    ReDim sqlStrs(6)
                    MyBase.FileType = MyBase.FunctionID & "_" & "Work"
                    sqlStrs(0) = "select * from QSI18_Cover('" & _PrjCode & "')"
                    sqlStrs(1) = "select * from QSI18_Work('" & _PrjCode & "')"
                    sqlStrs(2) = "select * from QSI18_MOS('" & _PrjCode & "')"
                    sqlStrs(3) = "select * from QSI18_EVO('" & _PrjCode & "')"
                    sqlStrs(4) = "select * from QSI18_EDW('" & _PrjCode & "')"
                    sqlStrs(5) = "select * from QSI18_ECL('" & _PrjCode & "')"
                    sqlStrs(6) = "select * from QSI18_Bill('" & _PrjCode & "')"

                Case Else
                    ReDim sqlStrs(0)
                    MyBase.FileType = -9999
                    sqlStrs(0) = "select 'Project Nature doesn''t exists' as ErrMsg"


            End Select


            Return sqlStrs
        End Function


        Enum eInputType As Integer
            ct_Bill
            ct_Work

        End Enum

        'Function getInputType(ByVal pINPUTTYPE As String) As eInputType
        '    Select Case pINPUTTYPE
        '        Case "Bill"
        '            Return eInputType.ct_Bill
        '        Case "Work"
        '            Return eInputType.ct_Work

        '        Case Else
        '            Throw New BaseException(BaseException.ErrorType.Normal, "GETINPUTTYPE", "Hasn't been define cert type: [" & pINPUTTYPE & "]")
        '    End Select
        'End Function
    End Class
End Namespace
