Imports CPS.SQL.Condition

Namespace Datatable.Flex
    ''' <summary>
    ''' Interface voucher DB [PIVOU]
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PIVOU
        Inherits CPS.SQL.Interface.RecordSet

#Region "Constanst Value"
        Private ReadOnly CheckPoint As String = "Sync_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

        Public Const TableName As String = "PIVOU"
        Public Const _PIVOU_COM_CDE As String = "PIVOU_COM_CDE"    'Company code (PK)
        Public Const _PIVOU_REF_NUM As String = "PIVOU_REF_NUM"    'Ref. No. (PK)
        Public Const _PIVOU_LIN_NUM As String = "PIVOU_LIN_NUM"    'Line (PK)
        Public Const _PIVOU_BCH_ID As String = "PIVOU_BCH_ID" 'PCMS Batch ID
        Public Const _PIVOU_VOU_TYP As String = "PIVOU_VOU_TYP"    'Voucher Type 
        Public Const _PIVOU_VOU_DTE As String = "PIVOU_VOU_DTE"    'Voucher date 
        Public Const _PIVOU_DES As String = "PIVOU_DES"    'Description
        Public Const _PIVOU_ACC_CDE As String = "PIVOU_ACC_CDE"    'A/C
        Public Const _PIVOU_ANA_CDE1 As String = "PIVOU_ANA_CDE1"   'Analysis code 1
        Public Const _PIVOU_ANA_CDE2 As String = "PIVOU_ANA_CDE2"   'Analysis code 2
        Public Const _PIVOU_ANA_CDE3 As String = "PIVOU_ANA_CDE3"   'Analysis code 3
        Public Const _PIVOU_ANA_CDE4 As String = "PIVOU_ANA_CDE4"   'Analysis code 4
        Public Const _PIVOU_ANA_CDE5 As String = "PIVOU_ANA_CDE5"   'Analysis code 5
        Public Const _PIVOU_DOC_NUM As String = "PIVOU_DOC_NUM"    'Document no.
        Public Const _PIVOU_ALT_DOC_NUM As String = "PIVOU_ALT_DOC_NUM"    'Alt. Document no.
        Public Const _PIVOU_DOC_TYP As String = "PIVOU_DOC_TYP"    'Document type
        Public Const _PIVOU_DOC_DTE As String = "PIVOU_DOC_DTE"    'Document date
        Public Const _PIVOU_DOC_DUE_DTE As String = "PIVOU_DOC_DUE_DTE"    'Document due date
        Public Const _PIVOU_CCY_CDE As String = "PIVOU_CCY_CDE"    'Currency
        Public Const _PIVOU_AMT As String = "PIVOU_AMT"    'Amount
        Public Const _PIVOU_EXC_RAT As String = "PIVOU_EXC_RAT"    'Exchange Rate
        Public Const _PIVOU_AMT_BAS As String = "PIVOU_AMT_BAS"    'Equv. Amount
        Public Const _PIVOU_D_C As String = "PIVOU_D_C"    'D/C
        Public Const _PIVOU_DOC_PAY_TRM As String = "PIVOU_DOC_PAY_TRM"    'Payment term
        Public Const _PIVOU_QTY As String = "PIVOU_QTY"    'Quantity
        Public Const _PIVOU_UNI As String = "PIVOU_UNI"    'Unit
        Public Const _PIVOU_DES1 As String = "PIVOU_DES1"   'Particular 1
        Public Const _PIVOU_DES2 As String = "PIVOU_DES2"   'Particular 2
        Public Const _PIVOU_ANA_CDE01 As String = "PIVOU_ANA_CDE01"  'Extended Analysis code 1
        Public Const _PIVOU_ANA_CDE02 As String = "PIVOU_ANA_CDE02"  'Extended Analysis code 2
        Public Const _PIVOU_ANA_CDE03 As String = "PIVOU_ANA_CDE03"  'Extended Analysis code 3
        Public Const _PIVOU_ANA_CDE04 As String = "PIVOU_ANA_CDE04"  'Extended Analysis code 4
        Public Const _PIVOU_ANA_CDE05 As String = "PIVOU_ANA_CDE05"  'Extended Analysis code 5
        Public Const _PIVOU_ANA_CDE06 As String = "PIVOU_ANA_CDE06"  'Extended Analysis code 6
        Public Const _PIVOU_ANA_CDE07 As String = "PIVOU_ANA_CDE07"  'Extended Analysis code 7
        Public Const _PIVOU_ANA_CDE08 As String = "PIVOU_ANA_CDE08"  'Extended Analysis code 8
        Public Const _PIVOU_ANA_CDE09 As String = "PIVOU_ANA_CDE09"  'Extended Analysis code 9
        Public Const _PIVOU_ANA_CDE10 As String = "PIVOU_ANA_CDE10"  'Extended Analysis code 10
        Public Const _PIVOU_RMK As String = "PIVOU_RMK"    'Remark
        Public Const _PIVOU_FLX_BCH_ID As String = "PIVOU_FLX_BCH_ID" 'Batch Number
        Public Const _PIVOU_FLX_VOU_NUM As String = "PIVOU_FLX_VOU_NUM"    'Voucher Number
        Public Const _PIVOU_FLX_STA As String = "PIVOU_FLX_STA"    'Flex Status
        Public Const _PIVOU_FLX_UPD_DTE As String = "PIVOU_FLX_UPD_DTE"    'Flex Update Date
        Public Const _PIVOU_PCMS_STA As String = "PIVOU_PCMS_STA"   'PCMS Status
        Public Const _PIVOU_PCMS_UPD_DTE As String = "PIVOU_PCMS_UPD_DTE"   'PCMS Update Date
        Public Const _PIVOU_PCMS_RMK As String = "PIVOU_PCMS_RMK"   'PCMS Remark

        Public ReadOnly Property sqlStr() As String
            Get
                Return "Select Distinct PIVOU_BCH_ID From [" & FLEX_DBName() & "].[dbo].[" & TableName & "]"
            End Get
        End Property

        Public ReadOnly Property BatchSql(ByVal strBatchID As String) As String
            Get
                WriteDebug("Payment Cert Query CPSFIN Query: " & "Select * From [" & FLEX_DBName() & "].[dbo].[" & TableName & "] Where  PIVOU_BCH_ID = '" & strBatchID & "'")
                Return "Select * From [" & FLEX_DBName() & "].[dbo].[" & TableName & "] Where  PIVOU_BCH_ID = '" & strBatchID & "'" & _
                        "and (PIVOU_PCMS_STA is null or ltrim(rtrim(PIVOU_PCMS_STA))='')"
            End Get
        End Property
#End Region

#Region "Define New"
        Public Sub New()
            MyBase.New(TableName, SubMain.FLEX_DBName)
            Me.add(_PIVOU_COM_CDE)
            Me.add(_PIVOU_REF_NUM)
            Me.add(_PIVOU_LIN_NUM)
            Me.add(_PIVOU_BCH_ID)
            Me.add(_PIVOU_VOU_TYP)
            Me.add(_PIVOU_VOU_DTE)
            Me.add(_PIVOU_DES)
            Me.add(_PIVOU_ACC_CDE)
            Me.add(_PIVOU_ANA_CDE1)
            Me.add(_PIVOU_ANA_CDE2)
            Me.add(_PIVOU_ANA_CDE3)
            Me.add(_PIVOU_ANA_CDE4)
            Me.add(_PIVOU_ANA_CDE5)
            Me.add(_PIVOU_DOC_NUM)
            Me.add(_PIVOU_ALT_DOC_NUM)
            Me.add(_PIVOU_DOC_TYP)
            Me.add(_PIVOU_DOC_DTE)
            Me.add(_PIVOU_DOC_DUE_DTE)
            Me.add(_PIVOU_CCY_CDE)
            Me.add(_PIVOU_AMT)
            Me.add(_PIVOU_EXC_RAT)
            Me.add(_PIVOU_AMT_BAS)
            Me.add(_PIVOU_D_C)
            Me.add(_PIVOU_DOC_PAY_TRM)
            Me.add(_PIVOU_QTY)
            Me.add(_PIVOU_UNI)
            Me.add(_PIVOU_DES1)
            Me.add(_PIVOU_DES2)
            Me.add(_PIVOU_ANA_CDE01)
            Me.add(_PIVOU_ANA_CDE02)
            Me.add(_PIVOU_ANA_CDE03)
            Me.add(_PIVOU_ANA_CDE04)
            Me.add(_PIVOU_ANA_CDE05)
            Me.add(_PIVOU_ANA_CDE06)
            Me.add(_PIVOU_ANA_CDE07)
            Me.add(_PIVOU_ANA_CDE08)
            Me.add(_PIVOU_ANA_CDE09)
            Me.add(_PIVOU_ANA_CDE10)
            Me.add(_PIVOU_RMK)
            Me.add(_PIVOU_FLX_BCH_ID)
            Me.add(_PIVOU_FLX_VOU_NUM)
            Me.add(_PIVOU_FLX_STA)
            Me.add(_PIVOU_FLX_UPD_DTE)
            Me.add(_PIVOU_PCMS_STA)
            Me.add(_PIVOU_PCMS_UPD_DTE)
            Me.add(_PIVOU_PCMS_RMK)
        End Sub
#End Region

#Region "Get Filter Information Structure"
        Public Sub getByBatchID(ByVal pBatchID As String)
            Dim oCondition As Condition

            Me.clearFilter()

            oCondition = New Condition
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_BCH_ID
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pBatchID
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

        End Sub

        Public Sub getSrvAR(ByVal pStatus As flexStatus)
            Dim oCondition As Condition

            Me.clearFilter()

            '(((PIVOU_FLXSTA = 'A') AND (PIVOU_BCH_ID Like 'C%') AND ((PIVOU_PCMS_STA = '') OR (PIVOU_PCMS_STA IS NULL)))
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_FLX_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = getFlexStatus(pStatus)
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_BCH_ID
            oCondition.Operation = Condition.eOperation.op_START
            oCondition.Value = "C"
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = ""
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_IS_NULL
            oCondition.BracketCloseNum = 3
            Me.addFilter(oCondition)

        End Sub

        'Add by michael, begin, 20140115
        Public Sub getSrvAR_Reversed(ByVal pStatus As flexStatus)
            Dim oCondition As Condition

            Me.clearFilter()

            '(((PIVOU_FLXSTA = 'A') AND (PIVOU_BCH_ID Like 'D%') AND ((PIVOU_PCMS_STA = '') OR (PIVOU_PCMS_STA IS NULL)))
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_FLX_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = getFlexStatus(pStatus)
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_BCH_ID
            oCondition.Operation = Condition.eOperation.op_START
            oCondition.Value = "D"
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = ""
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_IS_NULL
            oCondition.BracketCloseNum = 3
            Me.addFilter(oCondition)

        End Sub
        'Add by michael, end, 20140115

        Public Sub getSrvAR(ByVal pBatchID As String)
            Dim oCondition As Condition

            Me.clearFilter()

            '(((PIVOU_BCH_ID = pBatchID) AND ((PIVOU_PCMS_STA = '') OR (PIVOU_PCMS_STA IS NULL)))
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_BCH_ID
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pBatchID
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = ""
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_IS_NULL
            oCondition.BracketCloseNum = 3
            Me.addFilter(oCondition)

        End Sub

        Public Sub getItemAP(ByVal pStatus As flexStatus)
            Dim oCondition As Condition

            Me.clearFilter()

            '(((PIVOU_FLXSTA = 'A') AND (PIVOU_BCH_ID Like 'M%') AND ((PIVOU_PCMS_STA = '') OR (PIVOU_PCMS_STA IS NULL)))
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_FLX_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = getFlexStatus(pStatus)
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_BCH_ID
            oCondition.Operation = Condition.eOperation.op_START
            oCondition.Value = "M"
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = ""
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_IS_NULL
            oCondition.BracketCloseNum = 3
            Me.addFilter(oCondition)

        End Sub
        Public Sub getItemAP(ByVal pBatchID As String)
            Dim oCondition As Condition

            Me.clearFilter()

            '((PIVOU_FLXSTA = 'A') AND (PIVOU_BCH_ID = pBatchID) AND ((PIVOU_PCMS_STA = '') OR (PIVOU_PCMS_STA IS NULL)))
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_BCH_ID
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pBatchID
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = ""
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_IS_NULL
            oCondition.BracketCloseNum = 3
            Me.addFilter(oCondition)

        End Sub

        Public Sub getSrvAP(ByVal pStatus As flexStatus)
            Dim oCondition As Condition

            Me.clearFilter()

            '(((PIVOU_FLXSTA = 'A') AND (PIVOU_BCH_ID Like 'S%') AND ((PIVOU_PCMS_STA = '') OR (PIVOU_PCMS_STA IS NULL)))
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_FLX_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = getFlexStatus(pStatus)
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_BCH_ID
            oCondition.Operation = Condition.eOperation.op_START
            oCondition.Value = "S"
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = ""
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_IS_NULL
            oCondition.BracketCloseNum = 3
            Me.addFilter(oCondition)

        End Sub

        'Add by michael, begin, 20140115
        Public Sub getSrvAP_Reversed(ByVal pStatus As flexStatus)
            Dim oCondition As Condition

            Me.clearFilter()

            '(((PIVOU_FLXSTA = 'A') AND (PIVOU_BCH_ID Like 'S%') AND ((PIVOU_PCMS_STA = '') OR (PIVOU_PCMS_STA IS NULL)))
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_FLX_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = getFlexStatus(pStatus)
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_BCH_ID
            oCondition.Operation = Condition.eOperation.op_START
            oCondition.Value = "T"
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = ""
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_IS_NULL
            oCondition.BracketCloseNum = 3
            Me.addFilter(oCondition)

        End Sub
        'Add by michael, end, 20140115

        Public Sub getSrvAP(ByVal pBatchID As String)
            Dim oCondition As Condition

            Me.clearFilter()

            '(((PIVOU_FLXSTA = 'A') AND (PIVOU_BCH_ID = pBatchID) AND ((PIVOU_PCMS_STA = '') OR (PIVOU_PCMS_STA IS NULL)))
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_BCH_ID
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pBatchID
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = ""
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_IS_NULL
            oCondition.BracketCloseNum = 3
            Me.addFilter(oCondition)

        End Sub

        Public Sub getItemCM(ByVal pStatus As flexStatus)
            Dim oCondition As Condition

            Me.clearFilter()

            '(((PIVOU_FLXSTA = 'A') AND (PIVOU_BCH_ID Like 'M%') AND ((PIVOU_PCMS_STA = '') OR (PIVOU_PCMS_STA IS NULL)))
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_FLX_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = getFlexStatus(pStatus)
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_BCH_ID
            oCondition.Operation = Condition.eOperation.op_START
            oCondition.Value = "R"
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = ""
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_IS_NULL
            oCondition.BracketCloseNum = 3
            Me.addFilter(oCondition)

        End Sub
        Public Sub getItemCM(ByVal pBatchID As String)
            Dim oCondition As Condition

            Me.clearFilter()

            '((PIVOU_FLXSTA = 'A') AND (PIVOU_BCH_ID = pBatchID) AND ((PIVOU_PCMS_STA = '') OR (PIVOU_PCMS_STA IS NULL)))
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_BCH_ID
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pBatchID
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = ""
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PIVOU._PIVOU_PCMS_STA
            oCondition.Operation = Condition.eOperation.op_IS_NULL
            oCondition.BracketCloseNum = 3
            Me.addFilter(oCondition)

        End Sub
#End Region

        Public Enum flexStatus As Integer
            fs_Accepted = 1
            fs_Rejected = 2
            fs_Posted = 3
            fs_Error = 4
            fs_Delete = 5
        End Enum

        Public Function getFlexStatus(ByVal pStatus As flexStatus) As String
            Select Case pStatus
                Case flexStatus.fs_Accepted
                    'User change logic at 9th Nov 2009, the confirmation status is [ P ] not [ A ]
                    'Return "A"
                    Return "P"
                Case flexStatus.fs_Delete
                    Return "D"
                Case flexStatus.fs_Error
                    Return "E"
                Case flexStatus.fs_Posted
                    Return "P"
                Case flexStatus.fs_Rejected
                    Return "R"
                Case Else
                    Return ""
            End Select
        End Function

#Region "Assign Value"
        Public WriteOnly Property PIVOU_COM_CDE() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_COM_CDE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Company code (PK)]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_REF_NUM() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_REF_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Ref. No. (PK)]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_LIN_NUM() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_LIN_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Line (PK)]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_BCH_ID() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_BCH_ID, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[PCMS Batch ID]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_VOU_TYP() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_VOU_TYP, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Voucher Type ]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_VOU_DTE() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_VOU_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Voucher date ]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_DES() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_DES, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Description]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ACC_CDE() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ACC_CDE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[A/C]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ANA_CDE1() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ANA_CDE1, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Analysis code 1]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ANA_CDE2() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ANA_CDE2, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Analysis code 2]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ANA_CDE3() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ANA_CDE3, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Analysis code 3]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ANA_CDE4() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ANA_CDE4, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Analysis code 4]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ANA_CDE5() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ANA_CDE5, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Analysis code 5]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_DOC_NUM() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_DOC_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document no.]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ALT_DOC_NUM() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ALT_DOC_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Alt. Document no.]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_DOC_TYP() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_DOC_TYP, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document type]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_DOC_DTE() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_DOC_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document date]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_DOC_DUE_DTE() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_DOC_DUE_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document due date]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_CCY_CDE() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_CCY_CDE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Currency]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_AMT() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_AMT, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Amount]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_EXC_RAT() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_EXC_RAT, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Exchange Rate]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_AMT_BAS() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_AMT_BAS, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Equv. Amount]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_D_C() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_D_C, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[D/C]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_DOC_PAY_TRM() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_DOC_PAY_TRM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Payment term]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_QTY() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_QTY, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Quantity]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_UNI() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_UNI, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Unit]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_DES1() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_DES1, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Particular 1]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_DES2() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_DES2, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Particular 2]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ANA_CDE01() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ANA_CDE01, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 1]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ANA_CDE02() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ANA_CDE02, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 2]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ANA_CDE03() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ANA_CDE03, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 3]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ANA_CDE04() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ANA_CDE04, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 4]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ANA_CDE05() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ANA_CDE05, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 5]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ANA_CDE06() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ANA_CDE06, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 6]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ANA_CDE07() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ANA_CDE07, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 7]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ANA_CDE08() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ANA_CDE08, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 8]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ANA_CDE09() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ANA_CDE09, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 9]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_ANA_CDE10() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_ANA_CDE10, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 10]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_RMK() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_RMK, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Remark]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_FLX_BCH_ID() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_FLX_BCH_ID, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Batch Number]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_FLX_VOU_NUM() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_FLX_VOU_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Voucher Number]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_FLX_STA() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_FLX_STA, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Flex Status]")
                End If
            End Set
        End Property
        Public WriteOnly Property PIVOU_FLX_UPD_DTE() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_FLX_UPD_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Flex Update Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property PCMS_Status() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_PCMS_STA, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[PCMS Status S/E]")
                End If
            End Set
        End Property
        Public WriteOnly Property PCMS_UpdateDate() As Date
            Set(ByVal value As Date)
                If Not Me.modify(_PIVOU_PCMS_UPD_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[PCMS Update Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property PCMS_Remark() As String
            Set(ByVal value As String)
                If Not Me.modify(_PIVOU_PCMS_RMK, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[PCMS Remark]")
                End If
            End Set
        End Property
#End Region

    End Class

End Namespace

