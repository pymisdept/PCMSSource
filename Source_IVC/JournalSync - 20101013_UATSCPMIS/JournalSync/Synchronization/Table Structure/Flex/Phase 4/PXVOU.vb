Imports CPS.SQL.Condition

Namespace Datatable.Flex
    Public Class PXVOU
        Inherits CPS.SQL.Interface.RecordSet

#Region "Constructor"
        Public Sub New()

            MyBase.New(TableName, SubMain.FLEX_DBName)
            Me.add(_Company_Code)
            Me.add(_Voucher_Type)
            Me.add(_Voucher_No)
            Me.add(_Line)
            Me.add(_Voucher_Date)
            Me.add(_Description)
            Me.add(_Account)
            Me.add(_AnalysisCode1)
            Me.add(_AnalysisCode2)
            Me.add(_AnalysisCode3)
            Me.add(_AnalysisCode4)
            Me.add(_AnalysisCode5)
            Me.add(_DocumentNo)
            Me.add(_AltDocumentNo)
            Me.add(_DocumentType)
            Me.add(_DocumentDate)
            Me.add(_DocumentDueDate)
            Me.add(_Currency)
            Me.add(_Amount)
            Me.add(_ExchangeRate)
            Me.add(_EquvAmount)
            Me.add(_Allocation)
            Me.add(_PaymentTerm)
            Me.add(_Quantity)
            Me.add(_Unit)
            Me.add(_Particular1)
            Me.add(_Particular2)
            Me.add(_ExtendedAnalysisCode_01)
            Me.add(_ExtendedAnalysisCode_02)
            Me.add(_ExtendedAnalysisCode_03)
            Me.add(_ExtendedAnalysisCode_04)
            Me.add(_ExtendedAnalysisCode_05)
            Me.add(_ExtendedAnalysisCode_06)
            Me.add(_ExtendedAnalysisCode_07)
            Me.add(_ExtendedAnalysisCode_08)
            Me.add(_ExtendedAnalysisCode_09)
            Me.add(_ExtendedAnalysisCode_10)
            Me.add(_Flex_ExportDate)
            Me.add(_PCMS_Date)
            Me.add(_PCMS_Remark)
            Me.add(_PCMS_Status)

        End Sub
#End Region

#Region "Constanst Value"

        Private ReadOnly CheckPoint As String = "PNACD_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

        Public Const TableName As String = "PXVOU"
        Public Const _Company_Code As String = "PXVOU_LEG_CDE" 'Company Code (PK)
        Public Const _Voucher_Type As String = "PXVOU_VOU_TYP" 'Voucher Type
        Public Const _Voucher_No As String = "PXVOU_VOU_NUM"   'Voucher No (PK)
        Public Const _Line As String = "PXVOU_VOU_EXT" 'Line (PK)
        Public Const _Voucher_Date As String = "PXVOU_VOU_DTE" 'Voucher Date
        Public Const _Description As String = "PXVOU_DES"  'Description
        Public Const _Account As String = "PXVOU_ACC_CDE"  'Account
        Public Const _AnalysisCode1 As String = "PXVOU_ANA_CDE1"    'AnalysisCode 1
        Public Const _AnalysisCode2 As String = "PXVOU_ANA_CDE2"    'AnalysisCode 2
        Public Const _AnalysisCode3 As String = "PXVOU_ANA_CDE3"    'AnalysisCode 3
        Public Const _AnalysisCode4 As String = "PXVOU_ANA_CDE4"    'AnalysisCode 4
        Public Const _AnalysisCode5 As String = "PXVOU_ANA_CDE5" 'AnalysisCode 5
        Public Const _DocumentNo As String = "PXVOU_DOC_NUM"   'Document No
        Public Const _AltDocumentNo As String = "PXVOU_ALT_DOC"    'Alt. Document No
        Public Const _DocumentType As String = "PXVOU_DOC_TYP" 'Document Type
        Public Const _DocumentDate As String = "PXVOU_DOC_DTE" 'Document Date
        Public Const _DocumentDueDate As String = "PXVOU_DUE_DTE"  'Document Due Date
        Public Const _Currency As String = "PXVOU_CCY_CDE" 'Currency
        Public Const _Amount As String = "PXVOU_AMT"   'Amount
        Public Const _ExchangeRate As String = "PXVOU_EXC_RAT" 'Exchange Rate
        Public Const _EquvAmount As String = "PXVOU_AMT_BAS"   'Equv. Amount
        Public Const _Allocation As String = "PXVOU_D_C"   'Allocation
        Public Const _PaymentTerm As String = "PXVOU_PAY_TRM"  'Payment Term
        Public Const _Quantity As String = "PXVOU_QTY" 'Quantity
        Public Const _Unit As String = "PXVOU_UNI_CDE" 'Unit
        Public Const _Particular1 As String = "PXVOU_DES1"  'Particular 1
        Public Const _Particular2 As String = "PXVOU_DES2"  'Particular 2
        Public Const _ExtendedAnalysisCode_01 As String = "PXVOU_ANA_CDE01"  'Extended Analysis Code 01
        Public Const _ExtendedAnalysisCode_02 As String = "PXVOU_ANA_CDE02"  'Extended Analysis Code 02
        Public Const _ExtendedAnalysisCode_03 As String = "PXVOU_ANA_CDE03"  'Extended Analysis Code 03
        Public Const _ExtendedAnalysisCode_04 As String = "PXVOU_ANA_CDE04"  'Extended Analysis Code 04
        Public Const _ExtendedAnalysisCode_05 As String = "PXVOU_ANA_CDE05"  'Extended Analysis Code 05
        Public Const _ExtendedAnalysisCode_06 As String = "PXVOU_ANA_CDE06"  'Extended Analysis Code 06
        Public Const _ExtendedAnalysisCode_07 As String = "PXVOU_ANA_CDE07" 'Extended Analysis Code 07
        Public Const _ExtendedAnalysisCode_08 As String = "PXVOU_ANA_CDE08"  'Extended Analysis Code 08
        Public Const _ExtendedAnalysisCode_09 As String = "PXVOU_ANA_CDE09"  'Extended Analysis Code 09
        Public Const _ExtendedAnalysisCode_10 As String = "PXVOU_ANA_CDE10"  'Extended Analysis Code 10
        Public Const _Flex_ExportDate As String = "PXVOU_EXP_DTE"  'Flex Export Date

        Public Const _PCMS_Status As String = "PXVOU_STA"
        Public Const _PCMS_Date As String = "PXVOU_UPD_DTE"
        Public Const _PCMS_Remark As String = "PXVOU_REM"

        Public Shared Function sqlStr() As String
            Dim oSqlStr As String

            oSqlStr = "Select Distinct" & vbCrLf
            oSqlStr &= Datatable.Flex.PXVOU._Company_Code & ", " & vbCrLf
            oSqlStr &= Datatable.Flex.PXVOU._Voucher_No & vbCrLf
            oSqlStr &= "From " & vbCrLf
            oSqlStr &= "[" & FLEX_DBName() & "].[dbo].[" & Datatable.Flex.PXVOU.TableName & "]" & vbCrLf

            Return oSqlStr
        End Function

#End Region

#Region "Assign Value"
        Public WriteOnly Property Company_Code() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Company_Code, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Company Code (PK)]")
                End If
            End Set
        End Property
        Public WriteOnly Property Voucher_Type() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Voucher_Type, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Voucher Type]")
                End If
            End Set
        End Property
        Public WriteOnly Property Voucher_No() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Voucher_No, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Voucher No (PK)]")
                End If
            End Set
        End Property
        Public WriteOnly Property Line() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Line, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Line (PK)]")
                End If
            End Set
        End Property
        Public WriteOnly Property Voucher_Date() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Voucher_Date, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Voucher Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property Description() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Description, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Description]")
                End If
            End Set
        End Property
        Public WriteOnly Property Account() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Account, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Account]")
                End If
            End Set
        End Property
        Public WriteOnly Property AnalysisCode1() As String
            Set(ByVal value As String)
                If Not Me.Modify(_AnalysisCode1, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[AnalysisCode 1]")
                End If
            End Set
        End Property
        Public WriteOnly Property AnalysisCode2() As String
            Set(ByVal value As String)
                If Not Me.Modify(_AnalysisCode2, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[AnalysisCode 2]")
                End If
            End Set
        End Property
        Public WriteOnly Property AnalysisCode3() As String
            Set(ByVal value As String)
                If Not Me.Modify(_AnalysisCode3, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[AnalysisCode 3]")
                End If
            End Set
        End Property
        Public WriteOnly Property AnalysisCode4() As String
            Set(ByVal value As String)
                If Not Me.Modify(_AnalysisCode4, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[AnalysisCode 4]")
                End If
            End Set
        End Property
        Public WriteOnly Property DocumentNo() As String
            Set(ByVal value As String)
                If Not Me.Modify(_DocumentNo, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document No]")
                End If
            End Set
        End Property
        Public WriteOnly Property AltDocumentNo() As String
            Set(ByVal value As String)
                If Not Me.Modify(_AltDocumentNo, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Alt. Document No]")
                End If
            End Set
        End Property
        Public WriteOnly Property DocumentType() As String
            Set(ByVal value As String)
                If Not Me.Modify(_DocumentType, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document Type]")
                End If
            End Set
        End Property
        Public WriteOnly Property DocumentDate() As String
            Set(ByVal value As String)
                If Not Me.Modify(_DocumentDate, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property DocumentDueDate() As String
            Set(ByVal value As String)
                If Not Me.Modify(_DocumentDueDate, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document Due Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property Currency() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Currency, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Currency]")
                End If
            End Set
        End Property
        Public WriteOnly Property Amount() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Amount, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Amount]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExchangeRate() As String
            Set(ByVal value As String)
                If Not Me.Modify(_ExchangeRate, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Exchange Rate]")
                End If
            End Set
        End Property
        Public WriteOnly Property EquvAmount() As String
            Set(ByVal value As String)
                If Not Me.Modify(_EquvAmount, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Equv. Amount]")
                End If
            End Set
        End Property
        Public WriteOnly Property Allocation() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Allocation, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Allocation]")
                End If
            End Set
        End Property
        Public WriteOnly Property PaymentTerm() As String
            Set(ByVal value As String)
                If Not Me.Modify(_PaymentTerm, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Payment Term]")
                End If
            End Set
        End Property
        Public WriteOnly Property Quantity() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Quantity, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Quantity]")
                End If
            End Set
        End Property
        Public WriteOnly Property Unit() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Unit, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Unit]")
                End If
            End Set
        End Property
        Public WriteOnly Property Particular1() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Particular1, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Particular 1]")
                End If
            End Set
        End Property
        Public WriteOnly Property Particular2() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Particular2, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Particular 2]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_01() As String
            Set(ByVal value As String)
                If Not Me.Modify(_ExtendedAnalysisCode_01, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 01]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_02() As String
            Set(ByVal value As String)
                If Not Me.Modify(_ExtendedAnalysisCode_02, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 02]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_03() As String
            Set(ByVal value As String)
                If Not Me.Modify(_ExtendedAnalysisCode_03, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 03]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_04() As String
            Set(ByVal value As String)
                If Not Me.Modify(_ExtendedAnalysisCode_04, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 04]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_05() As String
            Set(ByVal value As String)
                If Not Me.Modify(_ExtendedAnalysisCode_05, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 05]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_06() As String
            Set(ByVal value As String)
                If Not Me.Modify(_ExtendedAnalysisCode_06, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 06]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_07() As String
            Set(ByVal value As String)
                If Not Me.modify(_ExtendedAnalysisCode_07, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 07]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_08() As String
            Set(ByVal value As String)
                If Not Me.Modify(_ExtendedAnalysisCode_08, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 08]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_09() As String
            Set(ByVal value As String)
                If Not Me.Modify(_ExtendedAnalysisCode_09, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 09]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_10() As String
            Set(ByVal value As String)
                If Not Me.Modify(_ExtendedAnalysisCode_10, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 10]")
                End If
            End Set
        End Property
        Public WriteOnly Property Flex_ExportDate() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Flex_ExportDate, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Flex Export Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property PCMS_UpdateDate() As Date
            Set(ByVal value As Date)
                If Not Me.modify(_PCMS_Date, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[PCMS Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property PCMS_Remark() As String
            Set(ByVal value As String)
                If Not Me.modify(_PCMS_Remark, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[PCMS Remark]")
                End If
            End Set
        End Property
        Public WriteOnly Property PCMS_Status() As String
            Set(ByVal value As String)
                If Not Me.modify(_PCMS_Status, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[PCMS Status]")
                End If
            End Set
        End Property

#End Region

#Region "Get Filter Information Structure"
        Public Sub getJournalEntry()
            Dim oCondition As Condition

            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PXVOU._PCMS_Status
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = ""
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PXVOU._PCMS_Status
            oCondition.Operation = Condition.eOperation.op_IS_NULL
            oCondition.BracketCloseNum = 2
            Me.addFilter(oCondition)

        End Sub
        Public Sub getJournalEntry(ByVal pCompanyCode As String, ByVal pVoucher As String)
            Dim oCondition As Condition

            Me.clearFilter()

            '(( PNACD_PCMS_STAT Is Null ) 
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PXVOU._Company_Code
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pCompanyCode
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            'Or ( PNACD_PCMS_STAT = ''))
            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PXVOU._Voucher_No
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pVoucher
            oCondition.BracketCloseNum = 2
            Me.addFilter(oCondition)

        End Sub
#End Region

    End Class
End Namespace