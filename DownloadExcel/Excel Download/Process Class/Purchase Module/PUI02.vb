Namespace Excel.ProcessClass
    Public Class PUI02
        Inherits Excel.Interface.FileInterface

        Private _PrjCode As String
        Private _MRNO As String
        Private _InputType As String



        Public Sub New(ByVal pRequestID As String)
            MyBase.New(New Microsoft.Office.Interop.Excel.Application)
            MyBase.RequestID = pRequestID
        End Sub

        Public WriteOnly Property ProjectCode() As String
            Set(ByVal value As String)
                _PrjCode = value.Trim
            End Set
        End Property

        Public WriteOnly Property inputType() As String
            Set(ByVal value As String)
                _InputType = value.Trim
            End Set
        End Property

        Public WriteOnly Property MRNO() As String
            Set(ByVal value As String)
                _MRNO = value
            End Set
        End Property

        Public Function SheetMapping() As String()
            Dim sqlStrs() As String = Nothing
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)


            Select Case getInputType(_InputType)
                Case eInputType.ct_PO

                    ReDim sqlStrs(7)
                    MyBase.FileType = MyBase.FunctionID & "_" & _InputType

                    sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & oPrjName.Replace("'", "''") & "' PRJDesc "
                    sqlStrs(1) = "SELECT PYMNTGROUP AS List_PYMNTGROUP FROM OCTG"
                    sqlStrs(2) = "SELECT ITEMCODE AS LIST_ITEMCODE,ITEMNAME AS LIST_ITEMNAME FROM CPS_VIEW_MATERIALLIST"
                    sqlStrs(3) = "SELECT CARDCODE AS LIST_SCCODE,CARDNAME AS LIST_SCNAME FROM CPS_VIEW_SUBCONTRACTORLIST"
                    sqlStrs(4) = "SELECT COSTCODE as List_CostCode, COSTNAME AS List_COSTDESC FROM CPS_View_CostCodeList"
                    sqlStrs(5) = "SELECT PRCCODE AS LIST_SUBSICODE,PRCNAME AS LIST_SUBSINAME FROM OPRC WHERE DIMCODE = 1 AND ISNULL(LOCKED,'N') = 'N'"
                    sqlStrs(6) = "SELECT CODE AS LIST_REQUESTCODE,[NAME] AS LIST_REQUESTNAME FROM [@REASONCODE]"
                    sqlStrs(7) = "SELECT CARDCODE AS LIST_CARDCODE, CARDNAME AS LIST_CARDNAME FROM CPS_VIEW_MATERIALSUPPLIERLIST"

                Case eInputType.ct_MR

                    ReDim sqlStrs(5)
                    MyBase.FileType = MyBase.FunctionID & "_" & _InputType
                    sqlStrs(0) = "SELECT * FROM CPS_VIEW_WS_PUI02_FORM WHERE PRJCODE = '" & _PrjCode & "'AND PCMSDOCNUM = '" & _MRNO & "'"
                    sqlStrs(1) = "SELECT PYMNTGROUP AS List_PYMNTGROUP FROM OCTG"
                    sqlStrs(2) = "SELECT CARDCODE AS LIST_SCCODE,CARDNAME AS LIST_SCNAME FROM CPS_VIEW_SUBCONTRACTORLIST"
                    sqlStrs(3) = "SELECT COSTCODE as List_CostCode, COSTNAME AS List_COSTDESC FROM CPS_View_CostCodeList"
                    sqlStrs(4) = "SELECT CARDCODE AS LIST_CARDCODE, CARDNAME AS LIST_CARDNAME FROM CPS_VIEW_MATERIALSUPPLIERLIST"
                    sqlStrs(5) = "SELECT PRCCODE AS LIST_SUBSICODE,PRCNAME AS LIST_SUBSINAME FROM OPRC WHERE DIMCODE = 1 AND ISNULL(LOCKED,'N') = 'N'"
            End Select

       


            Return sqlStrs

        End Function


        Enum eInputType As Integer
            ct_PO
            ct_MR

        End Enum

        Function getInputType(ByVal pINPUTTYPE As String) As eInputType
            Select Case pINPUTTYPE
                Case "PO"
                    Return eInputType.ct_PO
                Case "MR"
                    Return eInputType.ct_MR
                Case Else
                    Throw New BaseException(BaseException.ErrorType.Normal, "GETINPUTTYPE", "Hasn't been define cert type: [" & pINPUTTYPE & "]")
            End Select
        End Function

    End Class
End Namespace

