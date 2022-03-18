Imports SAPbobsCOM

Namespace SyncMainClass

    ''' <summary>
    ''' Direct Expense
    ''' </summary>
    ''' <remarks></remarks>
    ''' 

    Public Class Direct_Expense
        Inherits [Interface].Synchronization

        Private ReadOnly mJobReference As String = "Journal Entry Synchronization (Fiex & SAP)"
        Private LocalCurrency As String

#Region "Constructor"
        Public Sub New(ByVal pCompany As SAPbobsCOM.Company)
            MyBase.New(pCompany)
            MyBase.JobName = mJobReference
            ObjType = 30
            oLogMessage.FileName = "FI-JournalEntry"

            Dim sboBob As SBObob = pCompany.GetBusinessObject(BoObjectTypes.BoBridge)
            LocalCurrency = ""
            LocalCurrency = Trim(sboBob.GetLocalCurrency.Fields.Item(0).Value)
        End Sub
#End Region

        Public Overrides Sub Export()
            Throw New NotImplementedException
        End Sub

        Public Overrides Sub Import()
            Dim oPXVOU As Datatable.Flex.PXVOU
            Dim oDataTable As System.Data.DataTable
            Dim sqlStr As String
            Dim BatchID As String = ""
            Dim oCompanyCode, oVoucherNo As String
            Dim oNewObjKey As Integer
            Dim SyncHistory As Datatable.SAP.Sync_History
            Dim oAccMsg As String
            Dim hasError As Boolean

            Try
                oLogMessage.AddReferenceLine("POSTED", "Create New Journal Entry in SAP System.")

                oPXVOU = New Datatable.Flex.PXVOU

                oPXVOU.getJournalEntry()
                sqlStr = Datatable.Flex.PXVOU.sqlStr & oPXVOU.filterQuery
                'Karrson: Write Debug
                WriteDebug("Module: Direct_Expense: Sql String: " & sqlStr)
                oDataTable = oFlexConnection.DataTable(sqlStr)

                oVoucherNo = ""

                If Not oDataTable Is Nothing Then
                    For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                        hasError = False
                        oAccMsg = String.Empty
                        Try
                            BatchID = oDataRow(Datatable.Flex.PXVOU._Company_Code).ToString.Trim & _
                                      oDataRow(Datatable.Flex.PXVOU._Voucher_No).ToString.Trim
                            oCompanyCode = oDataRow(Datatable.Flex.PXVOU._Company_Code).ToString.Trim
                            oVoucherNo = oDataRow(Datatable.Flex.PXVOU._Voucher_No).ToString.Trim

                            'Start Transaction
                            'Karrson: Debug
                            Try
                                Me.INSERTCPSVOU(oCompanyCode, oVoucherNo)
                            Catch ex As Exception
                                hasError = True
                                oLogMessage.AddExceptionSkip(BatchID, ex.ToString)

                                ToErrorTable(oVoucherNo, _
                                             30, _
                                             -9999, _
                                             "[IMPORT]FI " & oVoucherNo, _
                                             Left(ex.ToString, 2000))

                            End Try
                            If hasError = False Then


                                WriteDebug("LN 67: StartTransaction")
                                Me.StartTransaction()



                                'Karrson: Validate Account Code
                                oAccMsg = Me.ValidateAccountCode(oCompanyCode, oVoucherNo)
                                If oAccMsg = String.Empty Then


                                    'Create Journal Entry in SAP
                                    oNewObjKey = Me.CreateJournalEntry(oCompanyCode, oVoucherNo)

                                    If Not oNewObjKey = 0 Then
                                        'Karrson  Change
                                        'Me.UpdateFlex(oCompanyCode, oVoucherNo)
                                        Me.UpdateFlex(oCompanyCode, oVoucherNo, oNewObjKey)

                                        'Log History into Synchronization History table
                                        SyncHistory = New Datatable.SAP.Sync_History
                                        SyncHistory.Add_CreatejE(oNewObjKey)
                                        'Karrson: Debug
                                        WriteDebug("Ln 80 : End Transaction: ")
                                        Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                                        oLogMessage.AddSuccessLine(oVoucherNo, "Create Document Success")

                                    End If
                                Else
                                    oLogMessage.AddFailureLine(oVoucherNo, oAccMsg)
                                    ToErrorTable(oVoucherNo, _
                                                 30, _
                                                 "-9999", _
                                                 oVoucherNo, _
                                                 oAccMsg)
                                    Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                                End If
                            End If

                        Catch b_ex As BaseException
                            'Karrson: Debug 
                            WriteDebug("Ln 90: Base Excecption: End Transaction")
                            Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                            oLogMessage.AddExceptionSkip(BatchID, b_ex.toString)

                            ToErrorTable(oVoucherNo, _
                                         30, _
                                         -9999, _
                                         "[IMPORT]FI " & oVoucherNo, _
                                         Left(b_ex.toString, 2000))
                        Catch ex As Exception
                            Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                            oLogMessage.AddExceptionSkip(BatchID, ex.ToString)

                            ToErrorTable(oVoucherNo, _
                                         30, _
                                         -9999, _
                                         "[IMPORT]FI " & oVoucherNo, _
                                         Left(ex.ToString, 2000))
                        End Try
                    Next
                End If
            Catch ex As Exception
                oLogMessage.AddExceptionSkip("MAIN", ex.ToString)

                ToErrorTable("MAIN", _
                             30, _
                             -9999, _
                             "Main Operation Error", _
                             ex.ToString)
            End Try
        End Sub

#Region "Jouranl Transaction Flow in SAP system"
        Function ValidateAccountCode(ByVal pCompanyCode As String, ByVal pVoucherNo As String) As String
            Dim oMsg As String = String.Empty
            'Declare Selection Variable
            Dim oPXVOU As Datatable.Flex.PXVOU
            Dim oDataTableJE As System.Data.DataTable

            'Declare SAP Variable

            Dim oJE_Connection As FlexConnection





            oPXVOU = New Datatable.Flex.PXVOU

            oPXVOU.getJournalEntry(pCompanyCode, pVoucherNo)
            oPXVOU.Add_OrderBy(Datatable.Flex.PXVOU._Line)

            Dim oCondition As New CPS.SQL.Condition.Condition
            oCondition.Relation = CPS.SQL.Condition.Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = Datatable.Flex.PXVOU._Particular2
            oCondition.Operation = CPS.SQL.Condition.Condition.eOperation.op_EQUAL
            oCondition.Value = "Z0201"
            oPXVOU.addFilter(oCondition)

            WriteDebug("Condition1")
            oCondition = New CPS.SQL.Condition.Condition
            oCondition.Relation = CPS.SQL.Condition.Condition.eRelation.re_OR
            oCondition.Alias = Datatable.Flex.PXVOU._Particular2
            oCondition.Operation = CPS.SQL.Condition.Condition.eOperation.op_EQUAL
            oCondition.Value = "R0101"
            oPXVOU.addFilter(oCondition)
            WriteDebug("Condition2")
            oCondition = New CPS.SQL.Condition.Condition
            oCondition.Relation = CPS.SQL.Condition.Condition.eRelation.re_OR
            oCondition.Alias = Datatable.Flex.PXVOU._Particular2
            oCondition.Operation = CPS.SQL.Condition.Condition.eOperation.op_EQUAL
            oCondition.Value = "R0102"
            oPXVOU.addFilter(oCondition)
            WriteDebug("Condition3")
            oCondition = New CPS.SQL.Condition.Condition
            oCondition.Relation = CPS.SQL.Condition.Condition.eRelation.re_OR
            oCondition.Alias = Datatable.Flex.PXVOU._Particular2
            oCondition.Operation = CPS.SQL.Condition.Condition.eOperation.op_EQUAL
            oCondition.Value = "R0201"
            oCondition.BracketCloseNum = 1
            oPXVOU.addFilter(oCondition)
            WriteDebug("ln191")
            WriteDebug("Query in ValidationAccountCode: " & oPXVOU.SelectQuery & oPXVOU.filterQuery & oPXVOU.OrderByQuery)

            oJE_Connection = New FlexConnection
            oDataTableJE = oJE_Connection.DataTable(oPXVOU.SelectQuery & _
                                                   oPXVOU.filterQuery & _
                                                   oPXVOU.OrderByQuery)

            WriteDebug("LN199")

            If Not oDataTableJE Is Nothing Then
                If oDataTableJE.Rows.Count > 0 Then


                    For Each oDataRow As System.Data.DataRow In oDataTableJE.Rows
                        oMsg = oMsg & "[" & oDataRow(Datatable.Flex.PXVOU._Particular2).ToString().Trim & "] Invalid Acount Code."
                    Next
                End If
            End If

            WriteDebug("LN204")
            oJE_Connection.Disconnect()
            Return oMsg
        End Function

        Function GetExchangeRate(ByVal _Company As SAPbobsCOM.Company, ByVal _Currency As String, ByVal DocDate As DateTime) As Decimal
            Dim _ret As Decimal = 0
            Dim _rs As Recordset
            Dim _sql As String = "SELECT RATE FROM ORTT WHERE RATEDATE = '{0}' AND CURRENCY = '{1}'"
            _rs = _Company.GetBusinessObject(BoObjectTypes.BoRecordset)
            Try
                _rs.DoQuery(String.Format(_sql, DocDate.ToString("yyyyMMdd"), _Currency.Replace("'", "''")))
                If _rs.EoF = False Then
                    _ret = Convert.ToDecimal(_rs.Fields.Item(0).Value)
                End If
            Catch ex As Exception

            End Try
            Return _ret
        End Function

        Function CreateJournalEntry(ByVal pCompanyCode As String, _
                                    ByVal pVoucherNo As String) As Integer
            'Declare Selection Variable
            Dim oPXVOU As Datatable.Flex.PXVOU
            Dim oDataTableJE As System.Data.DataTable
            ' 20140626




            'Declare SAP Variable
            Dim oJournalEntries As SAPbobsCOM.JournalEntries
            Dim oJournalEntries_Lines As SAPbobsCOM.JournalEntries_Lines

            Dim oJE_Connection As FlexConnection

            'Declare Account Calculate Variable
            Dim Pcms_AcctCode As String, Card_AcctCode As String, Flex_AcctCode As String
            Dim oCurrency As String
            Dim oFirstRow As Boolean
            Dim _RowCnt As Integer
            Dim _LocalDebit As Decimal
            Dim _LocalCredit As Decimal

            oPXVOU = New Datatable.Flex.PXVOU
            oPXVOU.getJournalEntry(pCompanyCode, pVoucherNo)
            'oPXVOU.Add_OrderBy(Datatable.Flex.PXVOU._Line & " desc ")
            oPXVOU.Add_OrderBy(Datatable.Flex.PXVOU._Line)

            oJE_Connection = New FlexConnection
            oDataTableJE = oJE_Connection.DataTable(oPXVOU.SelectQuery & _
                                                   oPXVOU.filterQuery & _
                                                   oPXVOU.OrderByQuery)

            oJournalEntries = Company.GetBusinessObject(BoObjectTypes.oJournalEntries)
            oJournalEntries.Reference = pCompanyCode
            oJournalEntries.Reference2 = pVoucherNo

            oFirstRow = True
            _RowCnt = 0
            _LocalCredit = 0
            _LocalDebit = 0
            For Each oDataRow As System.Data.DataRow In oDataTableJE.Rows
                _RowCnt = _RowCnt + 1
                oJournalEntries_Lines = oJournalEntries.Lines

                If Not oFirstRow Then
                    oJournalEntries_Lines.Add()
                Else
                    oFirstRow = False
                End If

                Flex_AcctCode = oDataRow(Datatable.Flex.PXVOU._Account).ToString.Trim
                Pcms_AcctCode = oDataRow(Datatable.Flex.PXVOU._AnalysisCode5).ToString.Trim
                Card_AcctCode = oDataRow(Datatable.Flex.PXVOU._AnalysisCode1).ToString.Trim

                oCurrency = oDataRow(Datatable.Flex.PXVOU._Currency).ToString.Trim

                oJournalEntries.ReferenceDate = oDataRow(Datatable.Flex.PXVOU._Voucher_Date)

                Select Case Left(Flex_AcctCode, 3)
                    Case Datatable.SAP.ControlAccount.AC_ContraCharge
                        'Karrson: Change
                        'oJournalEntries_Lines.AccountCode = Me.AcctCode(Flex_AcctCode, _
                        '                       Pcms_AcctCode)
                        oJournalEntries_Lines.AccountCode = oDataRow(Datatable.Flex.PXVOU._Particular2).ToString.Trim

                    Case Datatable.SAP.ControlAccount.AC_RetentionAP
                        'Karrson: Change
                        'oJournalEntries_Lines.AccountCode = Me.AcctCode(Flex_AcctCode, _
                        '                       Pcms_AcctCode)
                        oJournalEntries_Lines.AccountCode = oDataRow(Datatable.Flex.PXVOU._Particular2).ToString.Trim
                    Case Datatable.SAP.ControlAccount.AC_RetentionAR
                        'Karrson: Change
                        'oJournalEntries_Lines.AccountCode = Me.AcctCode(Flex_AcctCode, _
                        '                        Pcms_AcctCode)
                        oJournalEntries_Lines.AccountCode = oDataRow(Datatable.Flex.PXVOU._Particular2).ToString.Trim
                    Case Datatable.SAP.ControlAccount.AC_Trade_AP
                        'if Analysis Code 1 (CardCode) isn't blank (this transaction is throw control account)
                        Chk_BPCode(Card_AcctCode)
                        oJournalEntries_Lines.ShortName = Card_AcctCode & SyncMainClass.BusinessPartners._SupplierType

                    Case Datatable.SAP.ControlAccount.AC_Trade_AR
                        'if Analysis Code 1 (CardCode) isn't blank (this transaction is throw control account)
                        Chk_BPCode(Card_AcctCode)
                        oJournalEntries_Lines.ShortName = Card_AcctCode & SyncMainClass.BusinessPartners._CustomerType

                    Case Else
                        'Karrson Change:
                        'oJournalEntries_Lines.AccountCode = Me.AcctCode(Flex_AcctCode, _
                        '                        Pcms_AcctCode)
                        oJournalEntries_Lines.AccountCode = oDataRow(Datatable.Flex.PXVOU._Particular2).ToString.Trim

                End Select
                'Karrson Change
                'oJournalEntries_Lines.Reference1 = pVoucherNo
                'oJournalEntries_Lines.Reference2 = oDataRow(Datatable.Flex.PXVOU._Line).ToString.Trim
                oJournalEntries_Lines.Reference1 = pCompanyCode
                oJournalEntries_Lines.Reference2 = pVoucherNo & " - " & oDataRow(Datatable.Flex.PXVOU._Line).ToString.Trim
                'oJournalEntries_Lines.LineMemo = oDataRow(Datatable.Flex.PXVOU._Particular1).ToString.Trim

                oJournalEntries_Lines.UserFields.Fields.Item("U_Dscription").Value = oDataRow(Datatable.Flex.PXVOU._Particular1).ToString.Trim


                If oDataRow(Datatable.Flex.PXVOU._Allocation).ToString.Trim = "C" Then
                    If oCurrency = LocalCurrency Then
                        oJournalEntries_Lines.Credit = System.Math.Round(oDataRow(Datatable.Flex.PXVOU._Amount), 2)
                    Else

                        oJournalEntries_Lines.FCCredit = System.Math.Round(oDataRow(Datatable.Flex.PXVOU._Amount), 2)
                        '20140626 
                        If oDataTableJE.Rows.Count = _RowCnt Then
                            oJournalEntries_Lines.Credit = System.Math.Round(_LocalDebit - _LocalCredit, 2)
                        Else
                            oJournalEntries_Lines.Credit = System.Math.Round(oDataRow(Datatable.Flex.PXVOU._Amount) * GetExchangeRate(Company, oCurrency, oJournalEntries.ReferenceDate), 2)
                        End If



                    End If
                ElseIf oDataRow(Datatable.Flex.PXVOU._Allocation).ToString.Trim = "D" Then
                    If oCurrency = LocalCurrency Then
                        oJournalEntries_Lines.Debit = System.Math.Round(oDataRow(Datatable.Flex.PXVOU._Amount), 2)
                    Else
                        If oDataTableJE.Rows.Count = _RowCnt Then
                            oJournalEntries_Lines.Debit = System.Math.Round(_LocalCredit - _LocalDebit, 2)
                        Else
                            oJournalEntries_Lines.Debit = System.Math.Round(oDataRow(Datatable.Flex.PXVOU._Amount) * GetExchangeRate(Company, oCurrency, oJournalEntries.ReferenceDate), 2)
                        End If


                        oJournalEntries_Lines.FCDebit = System.Math.Round(oDataRow(Datatable.Flex.PXVOU._Amount), 2)
                    End If
                End If
                WriteDebug(oCurrency, "Create Journal Entry")
                If oCurrency <> LocalCurrency Then
                    oJournalEntries_Lines.FCCurrency = oCurrency
                End If
                'Karrson: Add Project Code
                oJournalEntries_Lines.ProjectCode = oDataRow(Datatable.Flex.PXVOU._AnalysisCode3).ToString.Trim
                'Karrson: Add Reference CardCode
                oJournalEntries_Lines.UserFields.Fields.Item("U_RefCard").Value = Card_AcctCode & SyncMainClass.BusinessPartners._SupplierType
                _LocalCredit = _LocalCredit + System.Math.Round(oJournalEntries_Lines.Credit, 2)
                _LocalDebit = _LocalDebit + System.Math.Round(oJournalEntries_Lines.Debit, 2)


            Next
            'For i As Integer = 0 To oJournalEntries.Lines.Count - 1
            '    oJournalEntries.Lines.SetCurrentLine(i)
            '    WriteDebug("Account Code")
            '    WriteDebug(oJournalEntries.Lines.AccountCode)
            '    WriteDebug("Debit")
            '    WriteDebug(oJournalEntries.Lines.Debit)
            '    WriteDebug("Credit")
            '    WriteDebug(oJournalEntries.Lines.Credit)
            '    WriteDebug("FgnCurrency")
            '    WriteDebug(oJournalEntries.Lines.FCCurrency)
            '    WriteDebug("FCDebit")
            '    WriteDebug(oJournalEntries.Lines.FCDebit)
            '    WriteDebug("FCCredit")
            '    WriteDebug(oJournalEntries.Lines.FCCredit)
            '    WriteDebug("ShortName")
            '    WriteDebug(oJournalEntries.Lines.ShortName)
            'Next

            oJE_Connection.Disconnect()

            If oJournalEntries.Add = 0 Then
                Dim NewObjKey As Integer

                NewObjKey = Company.GetNewObjectKey
                MyBase.ToMapping(NewObjKey, 30, 0)

                Return Company.GetNewObjectKey
            Else
                oLogMessage.AddFailureLine(pVoucherNo, Company.GetLastErrorDescription)
                ToErrorTable(pVoucherNo, _
                             30, _
                             Company.GetLastErrorCode, _
                             pCompanyCode, _
                             Left(Company.GetLastErrorDescription, 2000))

                Return 0
            End If
        End Function
        ''' <summary>
        ''' Change: Karrson: Generate Insert Query manully. Execute when original faile.
        ''' </summary>
        ''' <param name="pCompanyCode"></param>
        ''' <param name="pVoucherNo"></param>
        ''' <remarks></remarks>
        Sub INSERTCPSVOU(ByVal pCompanyCode As String, _
                              ByVal pVoucherNo As String)
            'Declare Selection Variable
            Dim oPXVOU As Datatable.Flex.PXVOU
            Dim oDataTable As System.Data.DataTable
            Dim oJE_Connection As FlexConnection

            'Declare CPSVOU table
            Dim oCPSVOU As Datatable.SAP.CPSVOU

            oPXVOU = New Datatable.Flex.PXVOU
            oPXVOU.getJournalEntry(pCompanyCode, pVoucherNo)
            oPXVOU.Add_OrderBy(Datatable.Flex.PXVOU._Line)

            oJE_Connection = New FlexConnection
            WriteDebug("JE: InsertCPSVOU Query")
            WriteDebug(oPXVOU.SelectQuery & oPXVOU.filterQuery & oPXVOU.OrderByQuery)
            oDataTable = oJE_Connection.DataTable(oPXVOU.SelectQuery & _
                                                   oPXVOU.filterQuery & _
                                                   oPXVOU.OrderByQuery)

            For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                oCPSVOU = New Datatable.SAP.CPSVOU
                WriteDebug("Set Rows")
                oCPSVOU.Company_Code = oDataRow(Datatable.Flex.PXVOU._Company_Code).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._Company_Code, oDataRow(Datatable.Flex.PXVOU._Company_Code).ToString.Trim)
                WriteDebug("Company")
                oCPSVOU.Voucher_Type = oDataRow(Datatable.Flex.PXVOU._Voucher_Type).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._Voucher_Type, oDataRow(Datatable.Flex.PXVOU._Voucher_Type).ToString.Trim)

                oCPSVOU.Voucher_No = oDataRow(Datatable.Flex.PXVOU._Voucher_No).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._Voucher_No, oDataRow(Datatable.Flex.PXVOU._Voucher_No).ToString.Trim)
                WriteDebug("Voucher No")

                oCPSVOU.Line = oDataRow(Datatable.Flex.PXVOU._Line).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._Line, oDataRow(Datatable.Flex.PXVOU._Line).ToString.Trim)

                oCPSVOU.Voucher_Date = oDataRow(Datatable.Flex.PXVOU._Voucher_Date)
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._Voucher_Date, oDataRow(Datatable.Flex.PXVOU._Voucher_Date))

                oCPSVOU.Description = oDataRow(Datatable.Flex.PXVOU._Description).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._Description, oDataRow(Datatable.Flex.PXVOU._Description).ToString.Trim)
                WriteDebug("Description")
                oCPSVOU.Account = oDataRow(Datatable.Flex.PXVOU._Account).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._Account, oDataRow(Datatable.Flex.PXVOU._Account).ToString.Trim)
                WriteDebug("Account")
                oCPSVOU.AnalysisCode1 = oDataRow(Datatable.Flex.PXVOU._AnalysisCode1).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._AnalysisCode1, oDataRow(Datatable.Flex.PXVOU._AnalysisCode1).ToString.Trim)

                oCPSVOU.AnalysisCode2 = oDataRow(Datatable.Flex.PXVOU._AnalysisCode2).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._AnalysisCode2, oDataRow(Datatable.Flex.PXVOU._AnalysisCode2).ToString.Trim)

                oCPSVOU.AnalysisCode3 = oDataRow(Datatable.Flex.PXVOU._AnalysisCode3).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._AnalysisCode3, oDataRow(Datatable.Flex.PXVOU._AnalysisCode3).ToString.Trim)

                oCPSVOU.AnalysisCode4 = oDataRow(Datatable.Flex.PXVOU._AnalysisCode4).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._AnalysisCode4, oDataRow(Datatable.Flex.PXVOU._AnalysisCode4).ToString.Trim)
                oCPSVOU.AnalysisCode5 = oDataRow(Datatable.Flex.PXVOU._AnalysisCode5).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._AnalysisCode5, oDataRow(Datatable.Flex.PXVOU._AnalysisCode5).ToString.Trim)
                oCPSVOU.DocumentNo = oDataRow(Datatable.Flex.PXVOU._DocumentNo).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._DocumentNo, oDataRow(Datatable.Flex.PXVOU._DocumentNo).ToString.Trim)

                oCPSVOU.AltDocumentNo = oDataRow(Datatable.Flex.PXVOU._AltDocumentNo).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._AltDocumentNo, oDataRow(Datatable.Flex.PXVOU._AltDocumentNo).ToString.Trim)

                oCPSVOU.DocumentType = oDataRow(Datatable.Flex.PXVOU._DocumentType).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._DocumentType, oDataRow(Datatable.Flex.PXVOU._DocumentType).ToString.Trim)

                oCPSVOU.DocumentDate = oDataRow(Datatable.Flex.PXVOU._DocumentDate)
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._DocumentDate, oDataRow(Datatable.Flex.PXVOU._DocumentDate))


                oCPSVOU.DocumentDueDate = oDataRow(Datatable.Flex.PXVOU._DocumentDueDate)
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._DocumentDueDate, oDataRow(Datatable.Flex.PXVOU._DocumentDueDate))


                oCPSVOU.Currency = oDataRow(Datatable.Flex.PXVOU._Currency).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._Currency, oDataRow(Datatable.Flex.PXVOU._Currency).ToString.Trim)
                WriteDebug("Currency")

                oCPSVOU.Amount = oDataRow(Datatable.Flex.PXVOU._Amount)
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._Amount, oDataRow(Datatable.Flex.PXVOU._Amount))

                oCPSVOU.ExchangeRate = oDataRow(Datatable.Flex.PXVOU._ExchangeRate)
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._ExchangeRate, oDataRow(Datatable.Flex.PXVOU._ExchangeRate))

                oCPSVOU.EquvAmount = oDataRow(Datatable.Flex.PXVOU._EquvAmount)
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._EquvAmount, oDataRow(Datatable.Flex.PXVOU._EquvAmount))

                oCPSVOU.Allocation = oDataRow(Datatable.Flex.PXVOU._Allocation).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._Allocation, oDataRow(Datatable.Flex.PXVOU._Allocation).ToString.Trim)

                oCPSVOU.PaymentTerm = oDataRow(Datatable.Flex.PXVOU._PaymentTerm).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._PaymentTerm, oDataRow(Datatable.Flex.PXVOU._PaymentTerm).ToString.Trim)

                oCPSVOU.Quantity = oDataRow(Datatable.Flex.PXVOU._Quantity)
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._Quantity, oDataRow(Datatable.Flex.PXVOU._Quantity))

                oCPSVOU.Unit = oDataRow(Datatable.Flex.PXVOU._Unit)
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._Unit, oDataRow(Datatable.Flex.PXVOU._Unit))
                WriteDebug("Unit")
                WriteDebug("Des1: " & oDataRow(Datatable.Flex.PXVOU._Particular1).ToString.Trim)
                oCPSVOU.Particular1 = oDataRow(Datatable.Flex.PXVOU._Particular1).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._Particular1, oDataRow(Datatable.Flex.PXVOU._Particular1).ToString.Trim)

                oCPSVOU.Particular2 = oDataRow(Datatable.Flex.PXVOU._Particular2).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._Particular2, oDataRow(Datatable.Flex.PXVOU._Particular2).ToString.Trim)

                oCPSVOU.ExtendedAnalysisCode_01 = oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_01).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._ExtendedAnalysisCode_01, oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_01).ToString.Trim)

                oCPSVOU.ExtendedAnalysisCode_02 = oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_02).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._ExtendedAnalysisCode_02, oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_02).ToString.Trim)
                oCPSVOU.ExtendedAnalysisCode_03 = oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_03).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._ExtendedAnalysisCode_03, oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_03).ToString.Trim)

                oCPSVOU.ExtendedAnalysisCode_04 = oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_04).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._ExtendedAnalysisCode_04, oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_04).ToString.Trim)

                oCPSVOU.ExtendedAnalysisCode_05 = oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_05).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._ExtendedAnalysisCode_05, oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_05).ToString.Trim)

                oCPSVOU.ExtendedAnalysisCode_06 = oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_06).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._ExtendedAnalysisCode_06, oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_06).ToString.Trim)

                oCPSVOU.ExtendedAnalysisCode_07 = oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_07).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._ExtendedAnalysisCode_07, oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_07).ToString.Trim)

                oCPSVOU.ExtendedAnalysisCode_08 = oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_08).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._ExtendedAnalysisCode_08, oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_08).ToString.Trim)

                oCPSVOU.ExtendedAnalysisCode_09 = oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_09).ToString.Trim
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._ExtendedAnalysisCode_09, oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_09).ToString.Trim)

                oCPSVOU.ExtendedAnalysisCode_10 = oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_10)
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._ExtendedAnalysisCode_10, oDataRow(Datatable.Flex.PXVOU._ExtendedAnalysisCode_10).ToString.Trim)
                WriteDebug("AnalysisCode")
                oCPSVOU.Flex_ExportDate = oDataRow(Datatable.Flex.PXVOU._Flex_ExportDate)
                'Karrson: Set Field
                oCPSVOU.setField(Datatable.SAP.CPSVOU._Flex_ExportDate, oDataRow(Datatable.Flex.PXVOU._Flex_ExportDate))

                'oCPSVOU.Process(oCPSVOU.InsertQuery())
                Dim _query As String = oCPSVOU.Insert_Query()
                WriteDebug("Query By Library: " & oCPSVOU.InsertQuery())
                WriteDebug("Query By Manual: " & _query)
                Try
                    oCPSVOU.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
                Catch ex As Exception

                    Dim rs As Recordset = Me.Company.GetBusinessObject(BoObjectTypes.BoRecordset)
                    rs.DoQuery(_query)
                    rs = Nothing
                End Try

            Next

            oJE_Connection.Disconnect()
        End Sub


        Public Function AcctCode(ByVal Flex_AcctCode As String, _
                                 ByVal Pcms_AcctCode As String) As String
            Dim oRecSet As Recordset = Company.GetBusinessObject(BoObjectTypes.BoRecordset)
            Dim sqlStr As String = ""
            Dim rtn_AcctCode As String = ""

            If Pcms_AcctCode = "" Then
                'Supend Account
                sqlStr = "Select Top 1 AcctCode From OACT Where U_CostType = 'SU'"
                oRecSet.DoQuery(sqlStr)

                rtn_AcctCode = oRecSet.Fields.Item("AcctCode").Value
            Else
                'Current account is an valid account in SAP
                sqlStr = "SELECT T0.[AcctCode] FROM OACT T0 WHERE T0.[AccntntCod] = '" & Flex_AcctCode & "' And T0.[AcctCode] = '" & Pcms_AcctCode & "'"
                oRecSet.DoQuery(sqlStr)

                If oRecSet.RecordCount = 0 Then
                    WriteDebug("Direct Expense Fail in AcctCode Funtion")
                    WriteDebug("Flex_AcctCode:" & Flex_AcctCode)
                    WriteDebug("Pcms_AcctCode:" & Pcms_AcctCode)
                    Throw New BaseException(BaseException.ErrorType.Normal, "DE_AcctCode", "Flex Analysis Code 5 isn't match with PCMS Account Code")
                Else
                    rtn_AcctCode = oRecSet.Fields.Item("AcctCode").Value
                End If
            End If

            Return rtn_AcctCode

        End Function

        Public Sub Chk_BPCode(ByVal pCardCode As String)
            Dim oRecSet As Recordset = Company.GetBusinessObject(BoObjectTypes.BoRecordset)
            Dim sqlStr As String

            sqlStr = "Select 1 From OCRD Where CardCode = '" & pCardCode & SyncMainClass.BusinessPartners._CustomerType & "' Or CardCode = '" & pCardCode & SyncMainClass.BusinessPartners._SupplierType & "'"
            oRecSet.DoQuery(sqlStr)

            If oRecSet.RecordCount = 0 Then
                Throw New BaseException(BaseException.ErrorType.Normal, "CHK_BPCODE", "Not Record Found by [" & pCardCode & "]")
            End If
        End Sub

        Function UpdateFlex(ByVal oCompanyCode As String, _
                            ByVal oVoucherCode As String) As Integer
            Dim oPXVOU As Datatable.Flex.PXVOU

            oPXVOU = New Datatable.Flex.PXVOU
            oPXVOU.getJournalEntry(oCompanyCode, oVoucherCode)

            'oPXVOU.PCMS_Remark = "Operation is Success"
            oPXVOU.PCMS_Status = "C"
            oPXVOU.PCMS_UpdateDate = Now

            oFlexConnection.Process(oPXVOU.UpdateQuery & " " & oPXVOU.filterQuery)
        End Function

        ''' <summary>
        ''' Karrson New Function: Update JE's Object Key to Flex Common Table for remark.
        ''' </summary>
        ''' <param name="oCompanyCode"></param>
        ''' <param name="oVoucherCode"></param>
        ''' <param name="oNewObjectKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function UpdateFlex(ByVal oCompanyCode As String, _
                            ByVal oVoucherCode As String, ByVal oNewObjectKey As Integer) As Integer
            Dim oPXVOU As Datatable.Flex.PXVOU

            oPXVOU = New Datatable.Flex.PXVOU
            oPXVOU.getJournalEntry(oCompanyCode, oVoucherCode)

            'oPXVOU.PCMS_Remark = "Operation is Success"
            oPXVOU.PCMS_Remark = "TransID: " & oNewObjectKey.ToString()
            oPXVOU.PCMS_Status = "C"
            oPXVOU.PCMS_UpdateDate = Now

            oFlexConnection.Process(oPXVOU.UpdateQuery & " " & oPXVOU.filterQuery)
        End Function

#End Region

        Public Overrides Property ObjType() As Integer
            Get
                Return MyBase.mObjType
            End Get
            Set(ByVal value As Integer)
                MyBase.mObjType = value
            End Set
        End Property

    End Class
End Namespace