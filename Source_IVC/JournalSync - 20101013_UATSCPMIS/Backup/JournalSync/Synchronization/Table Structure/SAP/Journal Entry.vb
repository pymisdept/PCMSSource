Imports CPS.SQL.Condition

Namespace Datatable.SAP
    Public Class CPSVOU
        Inherits CPS.SQL.Interface.RecordSet

        Dim _htQuery As Hashtable
#Region "Constructor"
        Public Sub New()

            MyBase.New(TableName)
            'Karrson Set Database

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

        Public Const TableName As String = "CPSVOU"
        Public Const _Company_Code As String = "LEG_CDE" 'Company Code (PK)
        Public Const _Voucher_Type As String = "VOU_TYP" 'Voucher Type
        Public Const _Voucher_No As String = "VOU_NUM"   'Voucher No (PK)
        Public Const _Line As String = "VOU_EXT" 'Line (PK)
        Public Const _Voucher_Date As String = "VOU_DTE" 'Voucher Date
        Public Const _Description As String = "DES"  'Description
        Public Const _Account As String = "ACC_CDE"  'Account
        Public Const _AnalysisCode1 As String = "ANA_CDE1"    'AnalysisCode 1
        Public Const _AnalysisCode2 As String = "ANA_CDE2"    'AnalysisCode 2
        Public Const _AnalysisCode3 As String = "ANA_CDE3"    'AnalysisCode 3
        Public Const _AnalysisCode4 As String = "ANA_CDE4"    'AnalysisCode 4
        Public Const _AnalysisCode5 As String = "ANA_CDE5" 'AnalysisCode 5
        Public Const _DocumentNo As String = "DOC_NUM"   'Document No
        Public Const _AltDocumentNo As String = "ALT_DOC"    'Alt. Document No
        Public Const _DocumentType As String = "DOC_TYP" 'Document Type
        Public Const _DocumentDate As String = "DOC_DTE" 'Document Date
        Public Const _DocumentDueDate As String = "DUE_DTE"  'Document Due Date
        Public Const _Currency As String = "CCY_CDE" 'Currency
        Public Const _Amount As String = "AMT"   'Amount
        Public Const _ExchangeRate As String = "EXC_RAT" 'Exchange Rate
        Public Const _EquvAmount As String = "AMT_BAS"   'Equv. Amount
        Public Const _Allocation As String = "D_C"   'Allocation
        Public Const _PaymentTerm As String = "PAY_TRM"  'Payment Term
        Public Const _Quantity As String = "QTY" 'Quantity
        Public Const _Unit As String = "UNI_CDE" 'Unit
        Public Const _Particular1 As String = "DES1"  'Particular 1
        Public Const _Particular2 As String = "DES2"  'Particular 2
        Public Const _ExtendedAnalysisCode_01 As String = "ANA_CDE01"  'Extended Analysis Code 01
        Public Const _ExtendedAnalysisCode_02 As String = "ANA_CDE02"  'Extended Analysis Code 02
        Public Const _ExtendedAnalysisCode_03 As String = "ANA_CDE03"  'Extended Analysis Code 03
        Public Const _ExtendedAnalysisCode_04 As String = "ANA_CDE04"  'Extended Analysis Code 04
        Public Const _ExtendedAnalysisCode_05 As String = "ANA_CDE05"  'Extended Analysis Code 05
        Public Const _ExtendedAnalysisCode_06 As String = "ANA_CDE06"  'Extended Analysis Code 06
        Public Const _ExtendedAnalysisCode_07 As String = "ANA_CDE07" 'Extended Analysis Code 07
        Public Const _ExtendedAnalysisCode_08 As String = "ANA_CDE08"  'Extended Analysis Code 08
        Public Const _ExtendedAnalysisCode_09 As String = "ANA_CDE09"  'Extended Analysis Code 09
        Public Const _ExtendedAnalysisCode_10 As String = "ANA_CDE10"  'Extended Analysis Code 10
        Public Const _Flex_ExportDate As String = "EXP_DTE"  'Flex Export Date

        Public Const _PCMS_Status As String = "STA"
        Public Const _PCMS_Date As String = "UPD_DTE"
        Public Const _PCMS_Remark As String = "REM"

        Public Shared Function sqlStr() As String
            Dim oSqlStr As String

            oSqlStr = "Select Distinct" & vbCrLf
            oSqlStr &= Datatable.SAP.CPSVOU._Company_Code & ", " & vbCrLf
            oSqlStr &= Datatable.SAP.CPSVOU._Voucher_No & vbCrLf
            oSqlStr &= "From " & vbCrLf
            oSqlStr &= "[" & FLEX_DBName() & "].[dbo].[" & Datatable.SAP.CPSVOU.TableName & "]" & vbCrLf

            Return oSqlStr
        End Function

#End Region

#Region "Assign Value"
        Public WriteOnly Property Company_Code() As String
            Set(ByVal value As String)
                If Not Me.modify(_Company_Code, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Company Code (PK)]")
                End If
            End Set
        End Property
        Public WriteOnly Property Voucher_Type() As String
            Set(ByVal value As String)
                If Not Me.modify(_Voucher_Type, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Voucher Type]")
                End If
            End Set
        End Property
        Public WriteOnly Property Voucher_No() As String
            Set(ByVal value As String)
                If Not Me.modify(_Voucher_No, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Voucher No (PK)]")
                End If
            End Set
        End Property
        Public WriteOnly Property Line() As String
            Set(ByVal value As String)
                If Not Me.modify(_Line, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Line (PK)]")
                End If
            End Set
        End Property
        Public WriteOnly Property Voucher_Date() As Date
            Set(ByVal value As Date)
                If Not Me.modify(_Voucher_Date, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Voucher Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property Description() As String
            Set(ByVal value As String)
                If Not Me.modify(_Description, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Description]")
                End If
            End Set
        End Property
        Public WriteOnly Property Account() As String
            Set(ByVal value As String)
                If Not Me.modify(_Account, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Account]")
                End If
            End Set
        End Property
        Public WriteOnly Property AnalysisCode1() As String
            Set(ByVal value As String)
                If Not Me.modify(_AnalysisCode1, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[AnalysisCode 1]")
                End If
            End Set
        End Property
        Public WriteOnly Property AnalysisCode2() As String
            Set(ByVal value As String)
                If Not Me.modify(_AnalysisCode2, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[AnalysisCode 2]")
                End If
            End Set
        End Property
        Public WriteOnly Property AnalysisCode3() As String
            Set(ByVal value As String)
                If Not Me.modify(_AnalysisCode3, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[AnalysisCode 3]")
                End If
            End Set
        End Property
        Public WriteOnly Property AnalysisCode4() As String
            Set(ByVal value As String)
                If Not Me.modify(_AnalysisCode4, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[AnalysisCode 4]")
                End If
            End Set
        End Property
        Public WriteOnly Property AnalysisCode5() As String
            Set(ByVal value As String)
                If Not Me.modify(_AnalysisCode4, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[AnalysisCode 5]")
                End If
            End Set
        End Property
        Public WriteOnly Property DocumentNo() As String
            Set(ByVal value As String)
                If Not Me.modify(_DocumentNo, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document No]")
                End If
            End Set
        End Property
        Public WriteOnly Property AltDocumentNo() As String
            Set(ByVal value As String)
                If Not Me.modify(_AltDocumentNo, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Alt. Document No]")
                End If
            End Set
        End Property
        Public WriteOnly Property DocumentType() As String
            Set(ByVal value As String)
                If Not Me.modify(_DocumentType, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document Type]")
                End If
            End Set
        End Property
        Public WriteOnly Property DocumentDate() As Date
            Set(ByVal value As Date)
                If Not Me.modify(_DocumentDate, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property DocumentDueDate() As Date
            Set(ByVal value As Date)
                If Not Me.modify(_DocumentDueDate, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document Due Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property Currency() As String
            Set(ByVal value As String)
                If Not Me.modify(_Currency, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Currency]")
                End If
            End Set
        End Property
        Public WriteOnly Property Amount() As String
            Set(ByVal value As String)
                If Not Me.modify(_Amount, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Amount]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExchangeRate() As String
            Set(ByVal value As String)
                If Not Me.modify(_ExchangeRate, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Exchange Rate]")
                End If
            End Set
        End Property
        Public WriteOnly Property EquvAmount() As String
            Set(ByVal value As String)
                If Not Me.modify(_EquvAmount, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Equv. Amount]")
                End If
            End Set
        End Property
        Public WriteOnly Property Allocation() As String
            Set(ByVal value As String)
                If Not Me.modify(_Allocation, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Allocation]")
                End If
            End Set
        End Property
        Public WriteOnly Property PaymentTerm() As String
            Set(ByVal value As String)
                If Not Me.modify(_PaymentTerm, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Payment Term]")
                End If
            End Set
        End Property
        Public WriteOnly Property Quantity() As String
            Set(ByVal value As String)
                If Not Me.modify(_Quantity, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Quantity]")
                End If
            End Set
        End Property
        Public WriteOnly Property Unit() As String
            Set(ByVal value As String)
                If Not Me.modify(_Unit, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Unit]")
                End If
            End Set
        End Property
        Public WriteOnly Property Particular1() As String
            Set(ByVal value As String)
                If Not Me.modify(_Particular1, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Particular 1]")
                End If
            End Set
        End Property
        Public WriteOnly Property Particular2() As String
            Set(ByVal value As String)
                If Not Me.modify(_Particular2, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Particular 2]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_01() As String
            Set(ByVal value As String)
                If Not Me.modify(_ExtendedAnalysisCode_01, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 01]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_02() As String
            Set(ByVal value As String)
                If Not Me.modify(_ExtendedAnalysisCode_02, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 02]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_03() As String
            Set(ByVal value As String)
                If Not Me.modify(_ExtendedAnalysisCode_03, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 03]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_04() As String
            Set(ByVal value As String)
                If Not Me.modify(_ExtendedAnalysisCode_04, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 04]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_05() As String
            Set(ByVal value As String)
                If Not Me.modify(_ExtendedAnalysisCode_05, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 05]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_06() As String
            Set(ByVal value As String)
                If Not Me.modify(_ExtendedAnalysisCode_06, value) Then
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
                If Not Me.modify(_ExtendedAnalysisCode_08, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 08]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_09() As String
            Set(ByVal value As String)
                If Not Me.modify(_ExtendedAnalysisCode_09, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 09]")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysisCode_10() As String
            Set(ByVal value As String)
                If Not Me.modify(_ExtendedAnalysisCode_10, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis Code 10]")
                End If
            End Set
        End Property
        Public WriteOnly Property Flex_ExportDate() As Date
            Set(ByVal value As Date)
                If Not Me.modify(_Flex_ExportDate, value) Then
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
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 2
            oCondition.Alias = CPSVOU._PCMS_Status
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = ""
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = CPSVOU._PCMS_Status
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
            oCondition.Alias = CPSVOU._Company_Code
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pCompanyCode
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            'Or ( PNACD_PCMS_STAT = ''))
            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = CPSVOU._Voucher_No
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pVoucher
            oCondition.BracketCloseNum = 2
            Me.addFilter(oCondition)

        End Sub
#End Region
#Region "Query"
        Public Sub setField(ByVal _col As String, ByVal value As Object)
            Dim _date As DateTime
            Dim _string As String
            If _htQuery Is Nothing Then
                _htQuery = New Hashtable()
            End If
            ' Check Date Field
            If Not value = Nothing Then
                If IsDate(value) Then
                    _date = Convert.ToDateTime(value)
                    'date Format
                    value = _date.ToString("yyyy-MM-dd 00:00:00")
                End If
                If IsNumeric(value) Then
                    _string = Convert.ToString(value)
                    value = _string.Replace(",", ".")
                End If
                If _htQuery.ContainsKey(_col) Then
                    _htQuery(_col) = value
                Else
                    _htQuery.Add(_col, value)
                End If
            End If
        End Sub
        
        Public Function Insert_Query() As String
            Dim _queryformat As String = "INSERT INTO dbo.[{0}] ({1}) VALUES ({2})"
            Dim _sql As String = String.Empty
            Dim _field As String = String.Empty
            Dim _value As String = String.Empty
            ' Write Field Name
            For i As Integer = 0 To _htQuery.Count - 1
                _field = _field & _htQuery.Keys(i).ToString()
                Try

                    _value = _value & "'" & _htQuery(_htQuery.Keys(i)).ToString().Replace("'", "''") & "'"

                Catch ex As Exception
                    _value = _value & "''"
                End Try

                If i < _htQuery.Count - 1 Then
                    _field = _field & ","
                    _value = _value & ","
                End If
            Next
            _sql = String.Format(_queryformat, CPSVOU.TableName, _field, _value)
            _htQuery.Clear()


            Return _sql
        End Function
#End Region

    End Class
End Namespace