Namespace Excel.ProcessClass
    Public Class PUI03
        Inherits Excel.Interface.FileInterface

        Private _PrjCode As String
        '//remove multiple input way selection
        'Private _MRNO As String
        'Private _InputType As String

        Public Sub New(ByVal pRequestID As String)
            MyBase.New(New Microsoft.Office.Interop.Excel.Application)
            MyBase.RequestID = pRequestID
        End Sub

        Public WriteOnly Property ProjectCode() As String
            Set(ByVal value As String)
                _PrjCode = value.Trim
            End Set
        End Property

        '//remove multiple input way selection
        'Public WriteOnly Property inputType() As String
        '    Set(ByVal value As String)
        '        _InputType = value.Trim
        '    End Set
        'End Property

        'Public WriteOnly Property MRNO() As String
        '    Set(ByVal value As String)
        '        _MRNO = value
        '    End Set
        'End Property

        Public Function SheetMapping() As String()
            Dim sqlStrs() As String = Nothing
            Dim oPrjName As String
            Dim oSubsiName As String

            oPrjName = getPrjName(_PrjCode)
            oSubsiName = getSubsiName(_PrjCode)

            '//remove multiple input way selection
            'Select Case getInputType(_InputType)
            '    Case eInputType.ct_PA

            ReDim sqlStrs(7)
            'MyBase.FileType = MyBase.FunctionID & "_" & _InputType

            MyBase.FileType = MyBase.FunctionID

            If _PrjCode = "-2" Then
                sqlStrs(0) = "SELECT '-2' AS PrjCode, 'No Project Base' as PrjDesc"
            Else
                sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & oPrjName.Replace("'", "''") & "' PRJDesc, '" & oSubsiName & "' as SubsiName"

            End If
            sqlStrs(1) = "SELECT PYMNTGROUP AS List_PYMNTGROUP FROM OCTG"
            sqlStrs(2) = "SELECT ITEMCODE AS LIST_ITEMCODE,ITEMNAME AS LIST_ITEMNAME FROM CPS_VIEW_MATERIALLIST"
            sqlStrs(3) = "SELECT CARDCODE AS LIST_CARDCODE, CARDNAME AS LIST_CARDNAME, CardAddrs as List_CardAddrs FROM CPS_VIEW_MATERIALSUPPLIERLIST"

            sqlStrs(4) = "SELECT COSTCODE as LIST_CostCode, COSTNAME AS List_COSTDESC " & _
                        "FROM CPS_View_CostCodeList WHERE COSTTYPE IN ('E','M','N','P','PS','S')"

            sqlStrs(5) = "SELECT SubsiCode AS LIST_SUBSICODE,SubsiName AS LIST_SUBSINAME FROM CPS_View_SubsidiaryList"
            sqlStrs(6) = "select Code as List_RequestCode,U_ReasonDesc as List_RequestName from [@reasoncode] where U_ReasonType='MR'"
            sqlStrs(7) = "select CurrCode AS LIST_CurCODE, FrgnName as LIST_CurName from OCRN"

            Return sqlStrs
        End Function

        '//remove multiple input way selction
        'Enum eInputType As Integer
        '    ct_PA
        '    ct_MR

        'End Enum

        'Function getInputType(ByVal pINPUTTYPE As String) As eInputType
        '    Select Case pINPUTTYPE
        '        Case "PA"
        '            Return eInputType.ct_PA
        '        Case "MR"
        '            Return eInputType.ct_MR
        '        Case Else
        '            Throw New BaseException(BaseException.ErrorType.Normal, "GETINPUTTYPE", "Hasn't been define cert type: [" & pINPUTTYPE & "]")
        '    End Select
        'End Function
    End Class
End Namespace