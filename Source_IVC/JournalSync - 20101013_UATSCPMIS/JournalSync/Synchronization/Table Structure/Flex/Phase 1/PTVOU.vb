Namespace Datatable.Flex
    ''' <summary>
    ''' Transitory voucher DB [PTVOU]
    ''' </summary>
    ''' <remarks></remarks>
    Public Class PTVOU
        Inherits CPS.SQL.Interface.RecordSet

        Private ReadOnly CheckPoint As String = "PTVOU_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"
        'Karrson: Manual Generate Query Variable
        Dim _htQuery As Hashtable

#Region "Constanst Value"
        Public Const TableName As String = "PTVOU"
        Public Const PTVOU_COM_CDE As String = "PTVOU_COM_CDE"
        Public Const PTVOU_REF_NUM As String = "PTVOU_REF_NUM"
        Public Const PTVOU_LIN_NUM As String = "PTVOU_LIN_NUM"
        Public Const PTVOU_BCH_ID As String = "PTVOU_BCH_ID"
        Public Const PTVOU_VOU_TYP As String = "PTVOU_VOU_TYP"
        Public Const PTVOU_VOU_DTE As String = "PTVOU_VOU_DTE"
        Public Const PTVOU_DES As String = "PTVOU_DES"
        Public Const PTVOU_ACC_CDE As String = "PTVOU_ACC_CDE"
        Public Const PTVOU_ANA_CDE1 As String = "PTVOU_ANA_CDE1"
        Public Const PTVOU_ANA_CDE2 As String = "PTVOU_ANA_CDE2"
        Public Const PTVOU_ANA_CDE3 As String = "PTVOU_ANA_CDE3"
        Public Const PTVOU_ANA_CDE4 As String = "PTVOU_ANA_CDE4"
        Public Const PTVOU_ANA_CDE5 As String = "PTVOU_ANA_CDE5"
        Public Const PTVOU_DOC_NUM As String = "PTVOU_DOC_NUM"
        Public Const PTVOU_ALT_DOC_NUM As String = "PTVOU_ALT_DOC_NUM"
        Public Const PTVOU_DOC_TYP As String = "PTVOU_DOC_TYP"
        Public Const PTVOU_DOC_DTE As String = "PTVOU_DOC_DTE"
        Public Const PTVOU_DOC_DUE_DTE As String = "PTVOU_DOC_DUE_DTE"
        Public Const PTVOU_CCY_CDE As String = "PTVOU_CCY_CDE"
        Public Const PTVOU_AMT As String = "PTVOU_AMT"
        Public Const PTVOU_EXC_RAT As String = "PTVOU_EXC_RAT"
        Public Const PTVOU_AMT_BAS As String = "PTVOU_AMT_BAS"
        Public Const PTVOU_D_C As String = "PTVOU_D_C"
        Public Const PTVOU_DOC_PAY_TRM As String = "PTVOU_DOC_PAY_TRM"
        Public Const PTVOU_QTY As String = "PTVOU_QTY"
        Public Const PTVOU_UNI As String = "PTVOU_UNI"
        Public Const PTVOU_DES1 As String = "PTVOU_DES1"
        Public Const PTVOU_DES2 As String = "PTVOU_DES2"
        Public Const PTVOU_ANA_CDE01 As String = "PTVOU_ANA_CDE01"
        Public Const PTVOU_ANA_CDE02 As String = "PTVOU_ANA_CDE02"
        Public Const PTVOU_ANA_CDE03 As String = "PTVOU_ANA_CDE03"
        Public Const PTVOU_ANA_CDE04 As String = "PTVOU_ANA_CDE04"
        Public Const PTVOU_ANA_CDE05 As String = "PTVOU_ANA_CDE05"
        Public Const PTVOU_ANA_CDE06 As String = "PTVOU_ANA_CDE06"
        Public Const PTVOU_ANA_CDE07 As String = "PTVOU_ANA_CDE07"
        Public Const PTVOU_ANA_CDE08 As String = "PTVOU_ANA_CDE08"
        Public Const PTVOU_ANA_CDE09 As String = "PTVOU_ANA_CDE09"
        Public Const PTVOU_ANA_CDE10 As String = "PTVOU_ANA_CDE10"
#End Region

#Region "Define New"
        Public Sub New()
            MyBase.New(TableName, SubMain.FLEX_DBName)
            Me.add(PTVOU_COM_CDE)
            Me.add(PTVOU_REF_NUM)
            Me.add(PTVOU_LIN_NUM)
            Me.add(PTVOU_BCH_ID)
            Me.add(PTVOU_VOU_TYP)
            Me.add(PTVOU_VOU_DTE)
            Me.add(PTVOU_DES)
            Me.add(PTVOU_ACC_CDE)
            Me.add(PTVOU_ANA_CDE1)
            Me.add(PTVOU_ANA_CDE2)
            Me.add(PTVOU_ANA_CDE3)
            Me.add(PTVOU_ANA_CDE4)
            Me.add(PTVOU_ANA_CDE5)
            Me.add(PTVOU_DOC_NUM)
            Me.add(PTVOU_ALT_DOC_NUM)
            Me.add(PTVOU_DOC_TYP)
            Me.add(PTVOU_DOC_DTE)
            Me.add(PTVOU_DOC_DUE_DTE)
            Me.add(PTVOU_CCY_CDE)
            Me.add(PTVOU_AMT)
            Me.add(PTVOU_EXC_RAT)
            Me.add(PTVOU_AMT_BAS)
            Me.add(PTVOU_D_C)
            Me.add(PTVOU_DOC_PAY_TRM)
            Me.add(PTVOU_QTY)
            Me.add(PTVOU_UNI)
            Me.add(PTVOU_DES1)
            Me.add(PTVOU_DES2)
            Me.add(PTVOU_ANA_CDE01)
            Me.add(PTVOU_ANA_CDE02)
            Me.add(PTVOU_ANA_CDE03)
            Me.add(PTVOU_ANA_CDE04)
            Me.add(PTVOU_ANA_CDE05)
            Me.add(PTVOU_ANA_CDE06)
            Me.add(PTVOU_ANA_CDE07)
            Me.add(PTVOU_ANA_CDE08)
            Me.add(PTVOU_ANA_CDE09)
            Me.add(PTVOU_ANA_CDE10)
        End Sub
#End Region

#Region "Assign Value"

        Public WriteOnly Property CompanyCode() As String
            Set(ByVal value As String)




                If Not Me.modify(PTVOU_COM_CDE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Company Code (PK)")
                End If
            End Set
        End Property
        Public WriteOnly Property RefNO() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_REF_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Reference No (PK)")
                End If
            End Set
        End Property
        Public WriteOnly Property LineNo() As Integer
            Set(ByVal value As Integer)
                If Not Me.modify(PTVOU_LIN_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Line No (PK)")
                End If
            End Set
        End Property
        Public WriteOnly Property BatchID() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_BCH_ID, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Batch ID")
                End If
            End Set
        End Property
        Public WriteOnly Property VoucherType() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_VOU_TYP, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Voucher Type")
                End If
            End Set
        End Property
        Public WriteOnly Property VoucherDate() As Date
            Set(ByVal value As Date)
                If Not Me.modify(PTVOU_VOU_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Voucher Date")
                End If
            End Set
        End Property
        Public WriteOnly Property Description() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_DES, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Description")
                End If
            End Set
        End Property
        Public WriteOnly Property AcctCode() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ACC_CDE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "A/C")
                End If
            End Set
        End Property
        Public WriteOnly Property AnalysisCode1() As String
            Set(ByVal value As String)

                If Not Me.modify(PTVOU_ANA_CDE1, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Analysis Code 1")
                End If
            End Set
        End Property
        Public WriteOnly Property AnalysisCode2() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ANA_CDE2, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Analysis Code 2")
                End If
            End Set
        End Property
        Public WriteOnly Property AnalysisCode3() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ANA_CDE3, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Analysis Code 3")
                End If
            End Set
        End Property
        Public WriteOnly Property AnalysisCode4() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ANA_CDE4, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Analysis Code 4")
                End If
            End Set
        End Property
        Public WriteOnly Property AnalysisCode5() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ANA_CDE5, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Analysis Code 5")
                End If
            End Set
        End Property
        Public WriteOnly Property DocumentNo() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_DOC_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Document no.")
                End If
            End Set
        End Property
        Public WriteOnly Property AltDocument() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ALT_DOC_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Alt. Document No.")
                End If
            End Set
        End Property
        Public WriteOnly Property DocumentType() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_DOC_TYP, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Document Type")
                End If
            End Set
        End Property
        Public WriteOnly Property DocumentDate() As Date
            Set(ByVal value As Date)
                If Not Me.modify(PTVOU_DOC_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Document Date")
                End If
            End Set
        End Property
        Public WriteOnly Property DocumentDueDate() As Date
            Set(ByVal value As Date)
                If Not Me.modify(PTVOU_DOC_DUE_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Document due date")
                End If
            End Set
        End Property
        Public WriteOnly Property Currency() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_CCY_CDE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "Currency")
                End If
            End Set
        End Property
        Public WriteOnly Property Amount() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_AMT, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Amount)")
                End If
            End Set
        End Property
        Public WriteOnly Property ExchangeRate() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_EXC_RAT, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Exchange Rate)")
                End If
            End Set
        End Property
        Public WriteOnly Property EquvAmuont() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_AMT_BAS, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Equv. Amount)")
                End If
            End Set
        End Property
        Public WriteOnly Property Allocation() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_D_C, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(D/C)")
                End If
            End Set
        End Property
        Public WriteOnly Property PaymentTerm() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_DOC_PAY_TRM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Payment Term)")
                End If
            End Set
        End Property
        Public WriteOnly Property Quantity() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_QTY, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Quantity)")
                End If
            End Set
        End Property
        Public WriteOnly Property Unit() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_UNI, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Unit)")
                End If
            End Set
        End Property
        Public WriteOnly Property Particular1() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_DES1, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Particular 1)")
                End If
            End Set
        End Property
        Public WriteOnly Property Particular2() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_DES2, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Particular 2)")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysis1() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ANA_CDE01, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Extended Analysis Code 1)")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysis2() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ANA_CDE02, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Extended Analysis Code 2)")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysis3() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ANA_CDE03, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Extended Analysis Code 3)")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysis4() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ANA_CDE04, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Extended Analysis Code 4)")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysis5() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ANA_CDE05, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Extended Analysis Code 5)")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysis6() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ANA_CDE06, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Extended Analysis Code 6)")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysis7() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ANA_CDE07, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Extended Analysis Code 7)")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysis8() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ANA_CDE08, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Extended Analysis Code 8)")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysis9() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ANA_CDE09, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Extended Analysis Code 9)")
                End If
            End Set
        End Property
        Public WriteOnly Property ExtendedAnalysis10() As String
            Set(ByVal value As String)
                If Not Me.modify(PTVOU_ANA_CDE10, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "(Extended Analysis Code 10)")
                End If
            End Set
        End Property


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
            Dim _queryformat As String = "INSERT INTO [{0}].dbo.[{1}] ({2}) VALUES ({3})"
            Dim _sql As String = String.Empty
            Dim _field As String = String.Empty
            Dim _value As String = String.Empty
            ' Write Field Name
            For i As Integer = 0 To _htQuery.Count - 1
                _field = _field & _htQuery.Keys(i).ToString()
                Try

                    '_value = _value & "'" & _htQuery(_htQuery.Keys(i)).ToString().Replace("'", "''") & "'"
                    _value = _value & "N'" & _htQuery(_htQuery.Keys(i)).ToString().Replace("'", "''") & "'"

                Catch ex As Exception
                    _value = _value & "''"
                End Try

                If i < _htQuery.Count - 1 Then
                    _field = _field & ","
                    _value = _value & ","
                End If
            Next
            _sql = String.Format(_queryformat, Me.Database, PTVOU.TableName, _field, _value)
            _htQuery.Clear()


            Return _sql
        End Function
#End Region
    End Class
End Namespace
