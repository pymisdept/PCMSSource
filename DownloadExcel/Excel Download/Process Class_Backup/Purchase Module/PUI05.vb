Namespace Excel.ProcessClass
    Public Class PUI05
        Inherits Excel.Interface.FileInterface

        Private _PrjCode As String, _InputType As String
        ' New Parameter (Vendor Code)
        Private _Vendor As String
        Public Sub New(ByVal pRequestID As String)
            MyBase.New(New Microsoft.Office.Interop.Excel.Application)
            MyBase.RequestID = pRequestID
        End Sub

        Public WriteOnly Property ProjectCode() As String
            Set(ByVal value As String)
                _PrjCode = value.Trim
            End Set
        End Property

        Public WriteOnly Property InputType() As String
            Set(ByVal value As String)
                _InputType = value.Trim
            End Set
        End Property

        ' New Parameter Vendor
        Public WriteOnly Property Vendor() As String
            Set(ByVal value As String)
                _Vendor = value.Trim

            End Set
        End Property
        Public Function SheetMapping() As String()
            Dim sqlStrs() As String = Nothing
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)

            Select Case getInputType(_InputType)
                Case eInputType.ct_CO
                    'Karrson Modify on 2009-08-23
                    'ReDim sqlStrs(2)
                    ReDim sqlStrs(1)

                    MyBase.FileType = MyBase.FunctionID & "_" & _InputType
                    'Karrson modify on 2009-08-23: Modify Query
                    'sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & oPrjName & "' PRJNAME "
                    sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & oPrjName.Replace("'", "''") & "' PRJNAME, OCRD.CardCode,OCRD.CardName FROM OCRD Where OCRD.CardCode = '" & _Vendor.Replace("'", "''") & "'"
                    sqlStrs(1) = "SELECT PCMSDOCNUM AS LIST_CONUM  FROM CPS_VIEW_OSCONFIRMATIONORDER WHERE PRJCODE = '" & _PrjCode & "'" & " AND CARDCODE = '" & _Vendor.Replace("'", "''") & "'"
                    'Karrson modify on 2009-08-23: Remove Query
                    'sqlStrs(2) = "SELECT CARDCODE AS LIST_CARDCODE, CARDNAME AS LIST_CARDNAME FROM CPS_VIEW_MATERIALSUPPLIERLIST"

                Case eInputType.ct_GR
                    'Karrson Modify on 2009-08-23
                    'ReDim sqlStrs(2)
                    ReDim sqlStrs(1)

                    MyBase.FileType = MyBase.FunctionID & "_" & _InputType
                    'Karrson modify on 2009-08-23: Modify Query
                    'sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & oPrjName & "' PRJNAME "
                    sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & oPrjName & "' PRJNAME, OCRD.CardCode,OCRD.CardName FROM OCRD Where OCRD.CardCode = '" & _Vendor.Replace("'", "''") & "'"
                    sqlStrs(1) = "SELECT PCMSDOCNUM AS LIST_DNNUM  FROM CPS_VIEW_OSGoodsReceived WHERE PRJCODE = '" & _PrjCode & "'" & " AND CARDCODE = '" & _Vendor.Replace("'", "''") & "'"
                    'Karrson modify on 2009-08-23: Remove Query
                    'sqlStrs(2) = "SELECT CARDCODE AS LIST_CARDCODE, CARDNAME AS LIST_CARDNAME FROM CPS_VIEW_MATERIALSUPPLIERLIST"

                    ''Case eInputType.ct_SMR
                    ''Karrson Modify on 2009-08-23
                    ''ReDim sqlStrs(2)
                    'ReDim sqlStrs(1)

                    'MyBase.FileType = MyBase.FunctionID & "_" & _InputType
                    ''Karrson modify on 2009-08-23: Modify Query
                    ''sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & oPrjName & "' PRJNAME "
                    'sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & oPrjName & "' PRJNAME, OCRD.CardCode,OCRD.CardName FROM OCRD Where OCRD.CardCode = '" & _Vendor.Replace("'", "''") & "'"
                    'sqlStrs(1) = "SELECT PCMSDOCNUM AS LIST_SMRNUM  FROM CPS_VIEW_OSMATERIALREQUISITION WHERE PRJCODE = '" & _PrjCode & "'" & " AND CARDCODE = '" & _Vendor.Replace("'", "''") & "'"
                    ''Karrson modify on 2009-08-23: Remove Query
                    ''sqlStrs(2) = "SELECT CARDCODE AS LIST_CARDCODE, CARDNAME AS LIST_CARDNAME FROM CPS_VIEW_MATERIALSUPPLIERLIST"


                Case eInputType.ct_PO
                    'Karrson Modify on 2009-08-23
                    'ReDim sqlStrs(2)
                    ReDim sqlStrs(1)

                    MyBase.FileType = MyBase.FunctionID & "_" & _InputType
                    'Karrson modify on 2009-08-23: Modify Query
                    'sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & oPrjName & "' PRJNAME "
                    sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & oPrjName & "' PRJNAME, OCRD.CardCode,OCRD.CardName FROM OCRD Where OCRD.CardCode = '" & _Vendor.Replace("'", "''") & "'"
                    sqlStrs(1) = "SELECT U_PCMSDOCNUM AS LIST_PONUM FROM CPS_VIEW_OSPURCHASEORDER WHERE PROJECT = '" & _PrjCode & "'" & " AND CARDCODE = '" & _Vendor.Replace("'", "''") & "'"
                    'Karrson modify on 2009-08-23: Remove Query
                    'sqlStrs(2) = "SELECT CARDCODE AS LIST_CARDCODE, CARDNAME AS LIST_CARDNAME FROM CPS_VIEW_MATERIALSUPPLIERLIST"

            End Select

            Return sqlStrs
        End Function

        Enum eInputType As Integer
            ct_CO
            ct_GR
            'ct_SMR
            ct_PO
        End Enum

        Function getInputType(ByVal pCERTTYPE As String) As eInputType
            Select Case pCERTTYPE
                Case "CO"
                    Return eInputType.ct_CO
                Case "GR"
                    Return eInputType.ct_GR
                    'Case "SMR"
                    'Return eInputType.ct_SMR
                Case "PO"
                    Return eInputType.ct_PO
                Case Else
                    Throw New BaseException(BaseException.ErrorType.Normal, "GETCERTTYPE", "Hasn't been define cert type: [" & pCERTTYPE & "]")
            End Select
        End Function

    End Class
End Namespace

