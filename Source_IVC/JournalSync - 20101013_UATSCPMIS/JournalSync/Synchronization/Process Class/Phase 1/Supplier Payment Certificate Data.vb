Imports SAPbobsCOM

Namespace SyncMainClass
    ''' <summary>
    ''' Supplier Payment Certificate Data, Inherits Snchronization Class
    ''' </summary>
    ''' <remarks></remarks>
    Class P_ItmInvoice
        Inherits [Interface].Synchronization

        Private ReadOnly mJobReference As String = "A/P Item Invoice Synchronization (Fiex & SAP)"
        Private LocalCurrency As String

        Public Sub New(ByVal pCompany As SAPbobsCOM.Company)
            MyBase.New(pCompany, New FlexConnection)
            MyBase.JobName = mJobReference
            oLogMessage.FileName = "AP-ItmInvoice"

            Dim sboBob As SBObob = pCompany.GetBusinessObject(BoObjectTypes.BoBridge)
            LocalCurrency = ""
            LocalCurrency = Trim(sboBob.GetLocalCurrency.Fields.Item(0).Value)
        End Sub

        ''' <summary>
        ''' Import data from PCMS to Flex structure
        ''' </summary>
        ''' <param name="pExportPCMS">Collection data from PCMS Structure</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function fDataMapping(ByVal pExportPCMS As ExportPCMS) As ImportFLEX
            Dim oImport As ImportFLEX

            oImport = New ImportFLEX

            'Import the basic entry
            With pExportPCMS
                oImport.CompanyCode = .SubsidiaryCode
                oImport.LineNo = .LineNo
                oImport.RefNo = .RefNo
                'oImport.LineNo = Nothing
                oImport.BatchID = .CertEntry
                oImport.VoucherType = ExportPCMS.VoucherType
                oImport.VoucherDate = .ValuationDate
                oImport.Description = ExportPCMS.Description
                oImport.AcctCode = ExAcctcode(.AcctCode, .BPCode, .RefCardCode).Trim
                oImport.AnalysisCode1 = Left(.BPCode.Trim, 8)
                oImport.AnalysisCode2 = Nothing
                oImport.AnalysisCode3 = .ProjectCode
                oImport.AnalysisCode4 = Right(.AcctCode, 5) & "000"
                oImport.AnalysisCode5 = .CostCode
                oImport.DocumentNo = .CertNumber
                oImport.AltDocNumber = .VInvoiceNo
                oImport.DocType = ExportPCMS.DocType
                oImport.DocDate = .ValuationDate
                oImport.DocDueDate = .CertDueDate
                oImport.Currency = .DocCurrency

                oImport.Allocation = "D"
                oImport.Amount = .TotalBefDis
                oImport.EquvAmount = .DocTotal

                'oImport.Amount = .TotalBefDis
                oImport.ExchangeRate = .DocRate
                'oImport.EquvAmount = .DocTotal
                oImport.PaymentTerm = Nothing
                oImport.Quantity = Nothing
                oImport.Unit = Nothing
                oImport.Particular1 = .CertData
                oImport.Particular2 = Nothing
                oImport.ExtendedAnalysis1 = Nothing
                oImport.ExtendedAnalysis2 = Nothing
                oImport.ExtendedAnalysis3 = Nothing
                oImport.ExtendedAnalysis4 = Nothing
                oImport.ExtendedAnalysis5 = Nothing
                oImport.ExtendedAnalysis6 = Nothing
                oImport.ExtendedAnalysis7 = Nothing
                oImport.ExtendedAnalysis8 = Nothing
                oImport.ExtendedAnalysis9 = Nothing
                oImport.ExtendedAnalysis10 = Nothing
            End With

            Return oImport
        End Function

        Function fCreditor(ByVal pExportPCMS As ExportPCMS) As ImportFLEX
            Dim oImport As ImportFLEX

            oImport = New ImportFLEX

            'Import the basic entry
            With pExportPCMS
                oImport.CompanyCode = .SubsidiaryCode
                oImport.LineNo = .LineNo
                oImport.RefNo = .RefNo
                'oImport.LineNo = Nothing
                oImport.BatchID = .CertEntry
                oImport.VoucherType = ExportPCMS.VoucherType
                oImport.VoucherDate = .ValuationDate
                oImport.Description = ExportPCMS.Description
                oImport.AcctCode = ExAcctcode(ControlAcct(.BPCode), .BPCode, .RefCardCode).Trim
                oImport.AnalysisCode1 = Left(.BPCode.Trim, 8)
                oImport.AnalysisCode2 = Nothing
                oImport.AnalysisCode3 = .ProjectCode
                oImport.AnalysisCode4 = Right(.AcctCode, 5) & "000"
                oImport.AnalysisCode5 = .CostCode
                oImport.DocumentNo = .CertNumber
                oImport.AltDocNumber = .VInvoiceNo
                oImport.DocType = ExportPCMS.DocType
                oImport.DocDate = .ValuationDate
                oImport.DocDueDate = .CertDueDate
                oImport.Currency = .DocCurrency

                oImport.Allocation = "C"
                oImport.Amount = .TotalBefDis
                oImport.EquvAmount = .DocTotal

                'oImport.Amount = .TotalBefDis
                oImport.ExchangeRate = .DocRate
                'oImport.EquvAmount = .DocTotal
                oImport.PaymentTerm = Nothing
                oImport.Quantity = Nothing
                oImport.Unit = Nothing
                oImport.Particular1 = .CertData
                oImport.Particular2 = Nothing
                oImport.ExtendedAnalysis1 = Nothing
                oImport.ExtendedAnalysis2 = Nothing
                oImport.ExtendedAnalysis3 = Nothing
                oImport.ExtendedAnalysis4 = Nothing
                oImport.ExtendedAnalysis5 = Nothing
                oImport.ExtendedAnalysis6 = Nothing
                oImport.ExtendedAnalysis7 = Nothing
                oImport.ExtendedAnalysis8 = Nothing
                oImport.ExtendedAnalysis9 = Nothing
                oImport.ExtendedAnalysis10 = Nothing
            End With

            Return oImport
        End Function

#Region "Data Mapping"
        Function result(ByVal pFields As SAPbobsCOM.Fields, ByVal pField_Dtls As SAPbobsCOM.Fields) As ExportPCMS
            Dim oReturnResult As ExportPCMS
            Dim oDocEntry As Integer
            Dim oDocCurrency As String

            oReturnResult = New ExportPCMS
            With pFields
                'Get the document entry form the record set result
                oDocEntry = .Item(Datatable.SAP.AP.Supplier_Hdr.DocEntry).Value
                oDocCurrency = Trim(.Item(Datatable.SAP.AP.Supplier_Hdr.DocCur).Value)

                oReturnResult.RefNo = oDocEntry
                oReturnResult.SubsidiaryCode = .Item(Datatable.SAP.AP.Supplier_Hdr.SubsiCode).Value
                'Supplier Payment Certificate Data
                oReturnResult.CertEntry = "M" & RSet(.Item(Datatable.SAP.AP.Supplier_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                'oReturnResult.VoucherType  (GJ)
                oReturnResult.ValuationDate = .Item(Datatable.SAP.AP.Supplier_Hdr.DocDate).Value
                'oReturnResult.Description  (General Journal)
                oReturnResult.AcctCode = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl.AcctCode).Value
                oReturnResult.BPCode = .Item(Datatable.SAP.AP.Supplier_Hdr.CardCode).Value
                oReturnResult.ProjectCode = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl.PrjCode).Value
                oReturnResult.CostCode = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl.AcctCode).Value
                oReturnResult.CertNumber = .Item(Datatable.SAP.AP.Supplier_Hdr.PCMSDocNum).Value
                oReturnResult.VInvoiceNo = Trim(pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl._U_SupInvNum).Value)
                'oReturnResult.DocType
                oReturnResult.CertDate = .Item(Datatable.SAP.AP.Supplier_Hdr.DocumentDate).Value
                oReturnResult.CertDueDate = .Item(Datatable.SAP.AP.Supplier_Hdr.DocDueDate).Value
                oReturnResult.DocCurrency = LocalCurrency
                oReturnResult.SubCon_No = Right(CStr(.Item(Datatable.SAP.AP.Supplier_Hdr.PCMSDocNum).Value).Trim, 6)
                oReturnResult.RefCardCode = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl._U_RefCardCode).Value

                'Calculate the Total Before Discount 
                Dim oDocTotal As Double ', oDisPercent As Double
                'If Me.LocalCurrency.ToUpper = oDocCurrency.ToUpper Then
                '    oReturnResult.DocRate = 1
                '    oDocTotal = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl.LC_Total).Value
                'Else
                '    oReturnResult.DocRate = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl.DocRate).Value
                '    oDocTotal = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl.FC_Total).Value
                'End If
                oReturnResult.DocRate = 1
                oDocTotal = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl._StockSum).Value
                'oDisPercent = .Item(Datatable.SAP.AP.Supplier_Dtl.DisPercent).Value

                oReturnResult.TotalBefDis = oDocTotal '/ (1 + oDisPercent / 100)
                oReturnResult.DocTotal = oDocTotal
                'oReturnResult.Allocation
                oReturnResult.CertData = Me.getPONum(oDocEntry)
            End With

            Return oReturnResult
        End Function

        Function CredtorResult(ByVal pFields As SAPbobsCOM.Fields) As ExportPCMS
            Dim oReturnResult As ExportPCMS
            Dim oDocEntry As Integer
            Dim oDocCurrency As String

            oReturnResult = New ExportPCMS
            With pFields
                'Get the document entry form the record set result
                oDocEntry = .Item(Datatable.SAP.AP.Supplier_Hdr.DocEntry).Value
                oDocCurrency = Trim(.Item(Datatable.SAP.AP.Supplier_Hdr.DocCur).Value)
                oReturnResult.LineNo = 1
                oReturnResult.RefNo = oDocEntry
                oReturnResult.SubsidiaryCode = .Item(Datatable.SAP.AP.Supplier_Hdr.SubsiCode).Value
                'Supplier Payment Certificate Data
                oReturnResult.CertEntry = "M" & RSet(.Item(Datatable.SAP.AP.Supplier_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                'oReturnResult.VoucherType  (GJ)
                oReturnResult.ValuationDate = .Item(Datatable.SAP.AP.Supplier_Hdr.DocumentDate).Value
                'oReturnResult.Description  (General Journal)
                oReturnResult.AcctCode = ControlAcct(.Item(Datatable.SAP.AP.Supplier_Hdr.CardCode).Value)
                oReturnResult.BPCode = .Item(Datatable.SAP.AP.Supplier_Hdr.CardCode).Value
                oReturnResult.ProjectCode = .Item(Datatable.SAP.AP.Supplier_Hdr.Project).Value
                oReturnResult.CostCode = ControlAcct(.Item(Datatable.SAP.AP.Supplier_Hdr.CardCode).Value)
                oReturnResult.CertNumber = .Item(Datatable.SAP.AP.Supplier_Hdr.PCMSDocNum).Value
                oReturnResult.VInvoiceNo = Nothing
                'oReturnResult.DocType
                oReturnResult.CertDate = .Item(Datatable.SAP.AP.Supplier_Hdr.DocDate).Value
                oReturnResult.CertDueDate = .Item(Datatable.SAP.AP.Supplier_Hdr.DocDueDate).Value
                oReturnResult.DocCurrency = LocalCurrency
                oReturnResult.SubCon_No = Right(CStr(.Item(Datatable.SAP.AP.Supplier_Hdr.PCMSDocNum).Value).Trim, 6)

                'Calculate the Total Before Discount 
                Dim oDocTotal As Double, oDisPercent As Double
                'If Me.LocalCurrency.ToUpper = oDocCurrency.ToUpper Then
                '    oDocTotal = .Item(Datatable.SAP.AP.Supplier_Hdr.DocTotal).Value
                'Else
                '    oDocTotal = .Item(Datatable.SAP.AP.Supplier_Hdr.DocTotalFC).Value
                'End If
                oDocTotal = .Item(Datatable.SAP.AP.Supplier_Hdr.DocTotal).Value
                oDisPercent = .Item(Datatable.SAP.AP.Supplier_Hdr.DisPercent).Value

                oReturnResult.DocRate = 1
                oReturnResult.TotalBefDis = oDocTotal '/ (1 + oDisPercent / 100)
                oReturnResult.DocTotal = oDocTotal

                'oReturnResult.Allocation
                oReturnResult.CertData = Me.getPONum(oDocEntry)
            End With

            Return oReturnResult
        End Function

        Function FreightResult(ByVal pField_Hdr As Fields) As ExportPCMS()
            Dim oDocEntry As Integer
            Dim oDocCurrency As String
            Dim Itm_PCH6 As Datatable.SAP.AP.Supplier_Frg
            Dim oRecSet As SAPbobsCOM.Recordset
            Dim oExportPCMSs(), oExportPCMS As ExportPCMS

            oDocEntry = pField_Hdr.Item(Datatable.SAP.AP.Supplier_Hdr.DocEntry).Value
            oDocCurrency = pField_Hdr.Item(Datatable.SAP.AP.Supplier_Hdr.DocCur).Value

            Itm_PCH6 = New Datatable.SAP.AP.Supplier_Frg
            Itm_PCH6.getFreightEntry(oDocEntry)
            oExportPCMSs = Nothing

            oRecSet = Itm_PCH6.Execute
            Do Until oRecSet.EoF
                oExportPCMS = New ExportPCMS

                With oExportPCMS
                    .RefNo = oDocEntry
                    .SubsidiaryCode = pField_Hdr.Item(Datatable.SAP.AP.Supplier_Hdr.SubsiCode).Value
                    'Supplier Payment Certificate Data
                    .CertEntry = "M" & RSet(pField_Hdr.Item(Datatable.SAP.AP.Supplier_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                    'oReturnResult.VoucherType  (GJ)
                    .ValuationDate = pField_Hdr.Item(Datatable.SAP.AP.Supplier_Hdr.DocDate).Value
                    'oReturnResult.Description  (General Journal)
                    .AcctCode = Itm_PCH6.getAcctCode(oRecSet.Fields.Item(Datatable.SAP.AP.Supplier_Frg.FreightCode).Value)
                    .BPCode = pField_Hdr.Item(Datatable.SAP.AP.Supplier_Hdr.CardCode).Value
                    .ProjectCode = pField_Hdr.Item(Datatable.SAP.AP.Supplier_Hdr.Project).Value
                    .CostCode = Itm_PCH6.getAcctCode(oRecSet.Fields.Item(Datatable.SAP.AP.Supplier_Frg.FreightCode).Value)
                    .CertNumber = pField_Hdr.Item(Datatable.SAP.AP.Supplier_Hdr.PCMSDocNum).Value
                    .VInvoiceNo = Nothing
                    'oReturnResult.DocType
                    .CertDate = pField_Hdr.Item(Datatable.SAP.AP.Supplier_Hdr.DocumentDate).Value
                    .CertDueDate = pField_Hdr.Item(Datatable.SAP.AP.Supplier_Hdr.DocDueDate).Value
                    .DocCurrency = LocalCurrency
                    .SubCon_No = Right(CStr(pField_Hdr.Item(Datatable.SAP.AP.Supplier_Hdr.PCMSDocNum).Value).Trim, 6)

                    'Calculate the Total Before Discount 
                    Dim oDocTotal As Double
                    'If Me.LocalCurrency.ToUpper = oDocCurrency.ToUpper Then
                    '    oDocTotal = oRecSet.Fields.Item(Datatable.SAP.AP.Supplier_Frg.LC_Total).Value
                    'Else
                    '    oDocTotal = oRecSet.Fields.Item(Datatable.SAP.AP.Supplier_Frg.FC_Total).Value
                    'End If
                    oDocTotal = oRecSet.Fields.Item(Datatable.SAP.AP.Supplier_Frg.LC_Total).Value

                    .DocRate = 1
                    .TotalBefDis = oDocTotal
                    .DocTotal = oDocTotal

                    'oReturnResult.Allocation
                    .CertData = Me.getPONum(oDocEntry)
                End With

                If oExportPCMSs Is Nothing Then
                    ReDim oExportPCMSs(0)
                Else
                    ReDim Preserve oExportPCMSs(oExportPCMSs.Length)
                End If
                oExportPCMSs(oExportPCMSs.Length - 1) = oExportPCMS

                oRecSet.MoveNext()
            Loop

            Return oExportPCMSs
        End Function
#End Region

        ''' <summary>
        ''' Get the Purchase Order PCMS Number
        ''' </summary>
        ''' <param name="pDocEntry">A/P Invoice Document Key</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function getPONum(ByVal pDocEntry As Integer) As String
            Dim oRecSet As Recordset
            Dim sqlStr As String = ""

            sqlStr &= "	Select Top 1 " & Datatable.SAP.AP.Supplier_Hdr.TableName & "." & Datatable.SAP.AP.Supplier_Hdr.PCMSDocNum & " From " & Datatable.SAP.AP.Supplier_Dtl.TableName & " 	" & vbCrLf
            sqlStr &= "	Inner Join " & Datatable.SAP.AP.Supplier_Hdr.TableName & " On " & Datatable.SAP.AP.Supplier_Hdr.TableName & "." & Datatable.SAP.AP.Supplier_Hdr.DocEntry & " = " & Datatable.SAP.AP.Supplier_Dtl.TableName & "." & Datatable.SAP.AP.Supplier_Dtl.BaseEntry & " And " & Datatable.SAP.AP.Supplier_Hdr.TableName & "." & Datatable.SAP.AP.Supplier_Hdr.ObjType & " = " & Datatable.SAP.AP.Supplier_Dtl.TableName & "." & Datatable.SAP.AP.Supplier_Dtl.BaseType & "	" & vbCrLf
            sqlStr &= "	Where " & Datatable.SAP.AP.Supplier_Dtl.TableName & "." & Datatable.SAP.AP.Supplier_Dtl.DocEntry & " = '" & CStr(pDocEntry).Trim & "'	" & vbCrLf
            oRecSet = commonRecordSet.execute(sqlStr)

            If oRecSet.RecordCount = 0 Then
                Return Nothing
            Else
                Return oRecSet.Fields.Item(Datatable.SAP.AP.Supplier_Hdr.PCMSDocNum).Value
            End If
        End Function

        Public Overrides Sub Export()
            'Declare Datatable 
            Dim oPTVOU As New Datatable.Flex.PTVOU
            Dim Itm_OPCH As Datatable.SAP.AP.Supplier_Hdr
            Dim Itm_PCH1 As New Datatable.SAP.AP.Supplier_Dtl

            Dim oSynchHistory As Datatable.SAP.Sync_History

            'Document Entry
            Dim oDocEntry As Integer = 0
            Dim oDocCurrency As String

            'Data Structure
            Dim oExport As ExportPCMS, oExports As ExportPCMS()
            Dim oFrgExports As ExportPCMS()
            Dim oImport As ImportFLEX

            Dim LineNumber As Integer

            'Common Object
            Dim recset_Hdr As SAPbobsCOM.Recordset, recset_Dtl As SAPbobsCOM.Recordset

            oLogMessage.AddReferenceLine("EXPORT", "Export Data To Flex Environment.")

            Try
                Itm_OPCH = New Datatable.SAP.AP.Supplier_Hdr
                recset_Hdr = Itm_OPCH.Execute

                Do Until recset_Hdr.EoF
                    Try
                        'Begin transaction
                        Me.StartTransaction()

                        oDocEntry = recset_Hdr.Fields.Item(Datatable.SAP.AP.Supplier_Hdr.DocEntry).Value
                        oDocCurrency = Trim(recset_Hdr.Fields.Item(Datatable.SAP.AP.Supplier_Hdr.DocCur).Value)

                        'Get Document Detail by Document entry
                        Itm_PCH1 = New Datatable.SAP.AP.Supplier_Dtl
                        Itm_PCH1.getPurchaseInvoice(oDocEntry)
                        recset_Dtl = Itm_PCH1.Execute

                        oExports = Nothing

                        'Loop the Detail record
                        Do Until recset_Dtl.EoF
                            'Export PCMS data to Structure Class (ExportPCMS)
                            oExport = result(recset_Hdr.Fields, recset_Dtl.Fields)

                            If oExports Is Nothing Then
                                ReDim oExports(0)
                            Else
                                ReDim Preserve oExports(oExports.Length)
                            End If
                            oExports(oExports.Length - 1) = oExport

                            recset_Dtl.MoveNext()
                        Loop

                        'Draw data from Additional Charge
                        oFrgExports = Me.FreightResult(recset_Hdr.Fields)

                        'Import Data into Tree
                        If Not oFrgExports Is Nothing Then
                            For Each oFrgExport As ExportPCMS In oFrgExports
                                If oExports Is Nothing Then
                                    ReDim oExports(0)
                                Else
                                    ReDim Preserve oExports(oExports.Length)
                                End If
                                oExports(oExports.Length - 1) = oFrgExport
                            Next
                        End If

                        LineNumber = 2

                        'Import Data into Flex Table
                        If Not oExports Is Nothing Then
                            For Each oExport In oExports
                                oExport.LineNo = LineNumber
                                LineNumber += 1
                                oImport = fDataMapping(oExport)
                                Me.ToFlex(oImport)
                            Next
                        End If

                        'Draw data into Credit Side
                        oExport = CredtorResult(recset_Hdr.Fields)
                        oExport.LineNo = 1
                        oImport = fCreditor(oExport)
                        'Import Data into Datatable PTVOU
                        Me.ToFlex(oImport)

                        oSynchHistory = New Datatable.SAP.Sync_History
                        oSynchHistory.Add_Submitted("M" & RSet(oDocEntry.ToString.Trim, 9).Replace(" ", "0"))

                        Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                        oLogMessage.AddSuccessLine("[EXPORT]AP " & oDocEntry, "Operation Success")
                    Catch b_ex As BaseException
                        If oDocEntry = 0 Then
                            oLogMessage.AddExceptionSkip("AP18 Itm", b_ex.toString)
                            ToErrorTable(oDocEntry, _
                                         114, _
                                         -9999, _
                                         "AP18 Itm", _
                                         b_ex.toString)
                        Else
                            oLogMessage.AddExceptionSkip("[EXPORT]AP " & oDocEntry, b_ex.toString)
                            ToErrorTable(oDocEntry, _
                                         114, _
                                         -9999, _
                                         "[EXPORT]AP " & oDocEntry, _
                                         b_ex.toString)
                        End If

                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                    Catch ex As Exception
                        If oDocEntry = 0 Then
                            oLogMessage.AddExceptionSkip("AP18 Itm", ex.ToString)
                            ToErrorTable(oDocEntry, _
                                         114, _
                                         -9999, _
                                         "AP18 Itm", _
                                         ex.ToString)
                        Else
                            oLogMessage.AddExceptionSkip("[EXPORT]AP " & oDocEntry, ex.ToString)
                            ToErrorTable(oDocEntry, _
                                         114, _
                                         -9999, _
                                         "[EXPORT]AP " & oDocEntry, _
                                         ex.ToString)
                        End If

                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                    End Try

                    recset_Hdr.MoveNext()
                Loop
            Catch b_ex As BaseException
                If oDocEntry = 0 Then
                    oLogMessage.AddExceptionSkip("AP18 Itm", b_ex.toString)
                    ToErrorTable(oDocEntry, _
                                 114, _
                                 -9999, _
                                 "AP18 Itm", _
                                 b_ex.toString)
                Else
                    oLogMessage.AddExceptionSkip("[EXPORT]AP " & oDocEntry, b_ex.toString)
                    ToErrorTable(oDocEntry, _
                                 114, _
                                 -9999, _
                                 "[IMPORT]AP " & oDocEntry, _
                                 b_ex.toString)
                End If
            Catch ex As Exception
                If oDocEntry = 0 Then
                    oLogMessage.AddExceptionSkip("AP18 Itm", ex.ToString)
                    ToErrorTable(oDocEntry, _
                                 114, _
                                 -9999, _
                                 "AP18 Itm", _
                                 ex.ToString)
                Else
                    oLogMessage.AddExceptionSkip("[EXPORT]AP " & oDocEntry, ex.ToString)
                    ToErrorTable(oDocEntry, _
                                 18, _
                                 -9999, _
                                 "[EXPORT]AP " & oDocEntry, _
                                 ex.ToString)
                End If
            End Try
            oLogMessage.AddReferenceLine("EXPORT", "--------------------------------")
        End Sub

        Public Overrides Sub Import()
            Try
                Me.Import_Posted()
                Me.Import_Exception()
                Me.Import_Reject()
                Me.Import_Delete()
            Catch ex As Exception
                oLogMessage.AddExceptionSkip("MAIN", ex.ToString)

                ToErrorTable("MAIN", _
                             18, _
                             -9999, _
                             "Main Operation Error", _
                             ex.ToString)
            End Try

        End Sub

        'Process Success Data with Accept
        Public Sub Import_Posted()
            Dim oPIVOU As Datatable.Flex.PIVOU
            Dim oDataTable As System.Data.DataTable
            Dim BatchID As String = ""
            Dim sqlStr As String
            Dim SyncHistory As Datatable.SAP.Sync_History
            Dim oNewObjKey As Integer

            oLogMessage.AddReferenceLine("POSTED", "Create New A/P Item Invoice in SAP System.")

            oPIVOU = New Datatable.Flex.PIVOU

            oPIVOU.getItemAP(Datatable.Flex.PIVOU.flexStatus.fs_Accepted)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery
            oDataTable = oFlexConnection.DataTable(sqlStr)

            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim

                        'Start Transaction
                        Me.StartTransaction()

                        'Draw Data into CPSFIN for Backup
                        Me.INSERT_CPSFIN(BatchID)

                        'Create A/P Item Invoice in SAP
                        oNewObjKey = Me.CreateDocument(BatchID)

                        If Not oNewObjKey = 0 Then
                            'Update Document Draft Status in [C] when Delete
                            Me.CloseDraft(BatchID)

                            'Update PIVOU table in Flex Server
                            Me.UpdateFlex(BatchID)

                            'Log History into Synchronization History table
                            SyncHistory = New Datatable.SAP.Sync_History
                            SyncHistory.Add_Acknowledge(BatchID)
                            SyncHistory.Add_Posted(BatchID)

                            'End Transaction
                            Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                            oLogMessage.AddSuccessLine(BatchID, "Create Document Success")
                        Else
                            Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        End If
                    Catch b_ex As BaseException
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, b_ex.toString)

                        ToErrorTable(BatchID, _
                                     18, _
                                     -9999, _
                                     "[IMPORT]AP " & BatchID, _
                                     b_ex.toString)
                    Catch ex As Exception
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, ex.ToString)


                        ToErrorTable(BatchID, _
                                     18, _
                                     -9999, _
                                     "[IMPORT]AP " & BatchID, _
                                     ex.ToString)
                    End Try
                Next
            End If

            oLogMessage.AddReferenceLine("POSTED", "-----------------------------------------")
        End Sub

        'Process Return Data with Error
        Public Sub Import_Exception()
            Dim oPIVOU As Datatable.Flex.PIVOU
            Dim oDataTable As System.Data.DataTable
            Dim BatchID As String = ""
            Dim sqlStr As String
            Dim SyncHistory As Datatable.SAP.Sync_History

            oLogMessage.AddReferenceLine("ERROR", "Reject the document draft from backend table in SAP.")
            oPIVOU = New Datatable.Flex.PIVOU

            oPIVOU.getItemAP(Datatable.Flex.PIVOU.flexStatus.fs_Error)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery
            oDataTable = oFlexConnection.DataTable(sqlStr)

            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim
                        'Start Transaction
                        Me.StartTransaction()

                        'Update Document Draft Status in [D] when Delete
                        Me.DeleteDraft(BatchID)

                        'Update PIVOU table in Flex Server
                        Me.UpdateFlex(BatchID)

                        'Log History into Synchronization History table
                        SyncHistory = New Datatable.SAP.Sync_History
                        SyncHistory.Add_Acknowledge(BatchID)
                        SyncHistory.Add_Rejected(BatchID)

                        'End Transaction
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                        oLogMessage.AddSuccessLine(BatchID, "Reject Document Draft Success")
                    Catch b_ex As BaseException
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, b_ex.toString)

                        ToErrorTable(BatchID, _
                                     18, _
                                     -9999, _
                                     "[IMPORT]AP " & BatchID, _
                                     b_ex.toString)
                    Catch ex As Exception
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, ex.ToString)

                        ToErrorTable(BatchID, _
                                     18, _
                                     -9999, _
                                     "[IMPORT]AP " & BatchID, _
                                     ex.ToString)
                    End Try
                Next

            End If

            oLogMessage.AddReferenceLine("ERROR", "----------------------------------------------------")
        End Sub

        'Process Return Data with Reject
        Public Sub Import_Reject()
            Dim oPIVOU As Datatable.Flex.PIVOU
            Dim oDataTable As System.Data.DataTable
            Dim BatchID As String = ""
            Dim sqlStr As String
            Dim SyncHistory As Datatable.SAP.Sync_History

            oLogMessage.AddReferenceLine("REJECT", "Reject the document draft from backend table in SAP.")
            oPIVOU = New Datatable.Flex.PIVOU

            oPIVOU.getItemAP(Datatable.Flex.PIVOU.flexStatus.fs_Rejected)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery
            oDataTable = oFlexConnection.DataTable(sqlStr)

            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim
                        'Start Transaction
                        Me.StartTransaction()

                        'Update Document Draft Status in [D] when Delete
                        Me.DeleteDraft(BatchID)

                        'Update PIVOU table in Flex Server
                        Me.UpdateFlex(BatchID)

                        'Log History into Synchronization History table
                        SyncHistory = New Datatable.SAP.Sync_History
                        SyncHistory.Add_Acknowledge(BatchID)
                        SyncHistory.Add_Rejected(BatchID)

                        'End Transaction
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                        oLogMessage.AddSuccessLine(BatchID, "Reject Document Draft Success")
                    Catch b_ex As BaseException
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, b_ex.toString)

                        ToErrorTable(BatchID, _
                                     18, _
                                     -9999, _
                                     "[IMPORT]AP " & BatchID, _
                                     b_ex.toString)
                    Catch ex As Exception
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, ex.ToString)

                        ToErrorTable(BatchID, _
                                     18, _
                                     -9999, _
                                     "[IMPORT]AP " & BatchID, _
                                     ex.ToString)
                    End Try
                Next

            End If

            oLogMessage.AddReferenceLine("REJECT", "----------------------------------------------------")
        End Sub

        'Process Return Data with Delete
        Public Sub Import_Delete()
            Dim oPIVOU As Datatable.Flex.PIVOU
            Dim oDataTable As System.Data.DataTable
            Dim BatchID As String = ""
            Dim sqlStr As String
            Dim SyncHistory As Datatable.SAP.Sync_History
            Dim oNewObjKey As Integer

            oLogMessage.AddReferenceLine("DELETE", "Delete Document posted into SAP by A/P Credit memo")
            oPIVOU = New Datatable.Flex.PIVOU

            oPIVOU.getItemAP(Datatable.Flex.PIVOU.flexStatus.fs_Delete)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery
            oDataTable = oFlexConnection.DataTable(sqlStr)

            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim
                        'Start Transaction
                        Me.StartTransaction()

                        'Create A/P Credit Memo base on A/P Invoice
                        oNewObjKey = Me.DeleteDocument(BatchID)

                        If Not oNewObjKey = 0 Then

                            'Update PIVOU table in Flex Server
                            Me.UpdateFlex(BatchID)

                            'Log History into Synchronization History table
                            SyncHistory = New Datatable.SAP.Sync_History
                            SyncHistory.Add_Acknowledge(BatchID)
                            SyncHistory.Add_Deleted(BatchID)

                            'End Transaction
                            Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                            oLogMessage.AddSuccessLine(BatchID, "Delete Document.")
                        Else
                            Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        End If
                    Catch b_ex As BaseException
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, b_ex.toString)

                        ToErrorTable(BatchID, _
                                     18, _
                                     -9999, _
                                     "[IMPORT]AP " & BatchID, _
                                     b_ex.toString)
                    Catch ex As Exception
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, ex.ToString)

                        ToErrorTable(BatchID, _
                                     18, _
                                     -9999, _
                                     "[IMPORT]AP " & BatchID, _
                                     ex.ToString)
                    End Try
                Next
            End If

            oLogMessage.AddReferenceLine("DELETE", "--------------------------------------------------")
        End Sub

#Region "Document Flow in SAP system"
        Function CreateDocument(ByVal oBatchID As String) As Integer
            Dim oDocuments As SAPbobsCOM.Documents
            Dim oDocumentLines As SAPbobsCOM.Document_Lines
            Dim oDocumentExpenses As SAPbobsCOM.DocumentsAdditionalExpenses
            Dim oRecSet As SAPbobsCOM.Recordset
            Dim oDraft As Datatable.SAP.AP.Supplier_Hdr
            Dim oDraft_Dtls As Datatable.SAP.AP.Supplier_Dtl
            Dim oDraft_Frgs As Datatable.SAP.AP.Supplier_Frg

            Dim oDocEntry As Integer
            Dim oBaseEntry, oBaseLine As Integer
            Dim oBaseType As String

            oDocuments = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices)
            oDocumentLines = oDocuments.Lines

            oDraft = New Datatable.SAP.AP.Supplier_Hdr
            oDraft_Dtls = New Datatable.SAP.AP.Supplier_Dtl
            oDraft_Frgs = New Datatable.SAP.AP.Supplier_Frg

            oDocEntry = Right(oBatchID, oBatchID.Length - 1)

            oDraft.getPurchaseInvoice(oDocEntry)
            oDraft_Dtls.getPurchaseInvoice(oDocEntry)
            oDraft_Frgs.getFreightEntry(oDocEntry)

            oRecSet = oDraft.Execute
            'Assign value into Header record
            If oRecSet.RecordCount = 0 Then
                Throw New BaseException(BaseException.ErrorType.System, "CREATEDOC_0516", "No record was found base on Draft Entry [" & oDocEntry & "]")
            End If

            Do Until oRecSet.EoF
                With oRecSet.Fields
                    oDocuments.CardCode = .Item(Datatable.SAP.AP.Supplier_Hdr.CardCode).Value
                    oDocuments.CardName = .Item(Datatable.SAP.AP.Supplier_Hdr.CardName).Value
                    oDocuments.DocDate = .Item(Datatable.SAP.AP.Supplier_Hdr.DocDate).Value
                    oDocuments.DocDueDate = .Item(Datatable.SAP.AP.Supplier_Hdr.DocDueDate).Value
                    oDocuments.TaxDate = .Item(Datatable.SAP.AP.Supplier_Hdr.DocumentDate).Value

                    If Not .Item(Datatable.SAP.AP.Supplier_Hdr.BillToAddress).Value.ToString.Trim = "" Then
                        oDocuments.Address = .Item(Datatable.SAP.AP.Supplier_Hdr.BillToAddress).Value
                    End If

                    If Not .Item(Datatable.SAP.AP.Supplier_Hdr.ShipToAddress).Value.ToString.Trim = "" Then
                        oDocuments.Address2 = .Item(Datatable.SAP.AP.Supplier_Hdr.ShipToAddress).Value
                    End If

                    If .Item(Datatable.SAP.AP.Supplier_Hdr.DocType).Value = "I" Then
                        oDocuments.DocType = BoDocumentTypes.dDocument_Items
                    Else
                        oDocuments.DocType = BoDocumentTypes.dDocument_Service
                    End If

                    'Delivery Instruction
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Hdr.DelIns).Value = .Item(Datatable.SAP.AP.Supplier_Hdr.DelIns).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Hdr.CntctName).Value = .Item(Datatable.SAP.AP.Supplier_Hdr.CntctName).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Hdr.CntctTel).Value = .Item(Datatable.SAP.AP.Supplier_Hdr.CntctTel).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Hdr.PCMSDocNum).Value = .Item(Datatable.SAP.AP.Supplier_Hdr.PCMSDocNum).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Hdr.DocSubject).Value = .Item(Datatable.SAP.AP.Supplier_Hdr.DocSubject).Value

                    If .Item(Datatable.SAP.AP.Supplier_Hdr.RefDate1).Value > "2000 Jan 30" Then
                        oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Hdr.RefDate1).Value = .Item(Datatable.SAP.AP.Supplier_Hdr.RefDate1).Value
                    End If
                    If .Item(Datatable.SAP.AP.Supplier_Hdr.RefDate2).Value > "2000 Jan 30" Then
                        oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Hdr.RefDate2).Value = .Item(Datatable.SAP.AP.Supplier_Hdr.RefDate2).Value
                    End If

                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Hdr.SubsiCode).Value = .Item(Datatable.SAP.AP.Supplier_Hdr.SubsiCode).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Hdr.PayTermDesc).Value = .Item(Datatable.SAP.AP.Supplier_Hdr.PayTermDesc).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Hdr.SlpName).Value = .Item(Datatable.SAP.AP.Supplier_Hdr.SlpName).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Hdr.SlpTel).Value = .Item(Datatable.SAP.AP.Supplier_Hdr.SlpTel).Value

                    oDocuments.DocCurrency = .Item(Datatable.SAP.AP.Supplier_Hdr.DocCur).Value
                    oDocuments.DocRate = .Item(Datatable.SAP.AP.Supplier_Hdr.DocRate).Value
                    oDocuments.Project = .Item(Datatable.SAP.AP.Supplier_Hdr.Project).Value
                End With
                oRecSet.MoveNext()
            Loop
            'Assign value into Detail record
            oRecSet = oDraft_Dtls.Execute
            If Not oRecSet.RecordCount = 0 Then
                oDocumentLines = oDocuments.Lines

                Do Until oRecSet.EoF
                    With oRecSet.Fields
                        If Not oRecSet.BoF Then
                            oDocumentLines.Add()
                        End If

                        oBaseEntry = .Item(Datatable.SAP.AP.Supplier_Dtl.BaseEntry).Value
                        oBaseType = .Item(Datatable.SAP.AP.Supplier_Dtl.BaseType).Value
                        oBaseLine = .Item(Datatable.SAP.AP.Supplier_Dtl.BaseLine).Value

                        If oBaseEntry > 0 And Not oBaseType = "" And oBaseLine > 0 Then
                            oDocumentLines.BaseEntry = oBaseEntry
                            oDocumentLines.BaseLine = oBaseLine
                            oDocumentLines.BaseType = oBaseType

                            GoTo Record_MoveNext
                        End If

                        If oDocuments.DocType = BoDocumentTypes.dDocument_Items Then
                            oDocumentLines.ItemCode = .Item(Datatable.SAP.AP.Supplier_Dtl.ItemCode).Value
                            oDocumentLines.Quantity = .Item(Datatable.SAP.AP.Supplier_Dtl.Quantity).Value
                            oDocumentLines.Price = .Item(Datatable.SAP.AP.Supplier_Dtl.UnitPrice).Value
                        End If

                        oDocumentLines.AccountCode = .Item(Datatable.SAP.AP.Supplier_Dtl.AcctCode).Value
                        oDocumentLines.ItemDescription = .Item(Datatable.SAP.AP.Supplier_Dtl.ItemName).Value

                        If .Item(Datatable.SAP.AP.Supplier_Dtl._ShipDate).Value > "2000 Jan 30" Then
                            oDocumentLines.ShipDate = .Item(Datatable.SAP.AP.Supplier_Dtl._ShipDate).Value
                        End If

                        If LocalCurrency = oDocuments.DocCurrency Then
                            oDocumentLines.LineTotal = .Item(Datatable.SAP.AP.Supplier_Dtl.LC_Total).Value
                        Else
                            oDocumentLines.LineTotal = .Item(Datatable.SAP.AP.Supplier_Dtl.FC_Total).Value
                        End If

                        oDocumentLines.ProjectCode = .Item(Datatable.SAP.AP.Supplier_Dtl.PrjCode).Value

                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_Size).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_Size).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_Size).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_Packing).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_Packing).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_Packing).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_Color).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_Color).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_Color).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_Brand).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_Brand).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_Brand).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_Model).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_Model).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_Model).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_SupInvNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_SupInvNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_SupInvNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_QuoteNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_QuoteNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_QuoteNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_SourceType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_SourceType).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_SourceType).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_SourceLine).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_SourceLine).Value

                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_DestType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_DestType).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_DestType).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_UOM).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_UOM).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_UOM).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_PCMSDocNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_PCMSDocNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_PCMSDocNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_BillNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_BillNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_BillNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_SecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_SecNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_SecNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_SubSecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_SubSecNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_SubSecNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_PageNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_PageNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_PageNum).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_Quantity).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_Quantity).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_Price).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_Price).Value

                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_ItemType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_ItemType).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_ItemType).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_MCBillNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_MCBillNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_MCBillNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_MCSecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_MCSecNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_MCSecNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_MCSubSecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_MCSubSecNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_MCSubSecNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_MCPageNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_MCPageNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_MCPageNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_PriceType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_PriceType).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_PriceType).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_AppMethod).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_AppMethod).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_AppMethod).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_LineType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_LineType).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_LineType).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_MCLineNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_MCLineNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_MCLineNum).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_OpenPrcnt).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_OpenPrcnt).Value

                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_ContraFlag).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_ContraFlag).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_ContraFlag).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_RecoverFlag).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_RecoverFlag).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_RecoverFlag).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_RecoverStatus).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_RecoverStatus).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_RecoverStatus).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_SubLineNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_SubLineNum).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_MCSubLineNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_MCSubLineNum).Value

                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_ClientRef).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_ClientRef).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_ClientRef).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_SourceEntry).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_SourceEntry).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_DestEntry).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_DestEntry).Value

                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_IncomeCode).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_IncomeCode).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_IncomeCode).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_IPCode).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_IPCode).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_IPCode).Value
                        End If
                        If .Item(Datatable.SAP.AP.Supplier_Dtl._U_BillLineNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_BillLineNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_BillLineNum).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.Supplier_Dtl._U_BillSubLineNum).Value = .Item(Datatable.SAP.AP.Supplier_Dtl._U_BillSubLineNum).Value

                    End With

Record_MoveNext:
                    oRecSet.MoveNext()
                Loop
            Else
                Throw New BaseException(BaseException.ErrorType.System, "CREATEDOC_0550", "No record was found base on Draft Entry [" & oDocEntry & "]")
            End If

            'Assign value into Freight Charge Table.
            oRecSet = oDraft_Frgs.Execute
            With oRecSet.Fields
                If Not oRecSet.RecordCount = 0 Then
                    oDocumentExpenses = oDocuments.Expenses

                    Do Until oRecSet.EoF
                        If Not oRecSet.BoF Then
                            oDocumentExpenses.Add()
                        End If
                        oDocumentExpenses.ExpenseCode = .Item(Datatable.SAP.AP.Supplier_Frg.FreightCode).Value
                        If LocalCurrency = oDocuments.DocCurrency Then
                            oDocumentExpenses.LineTotal = .Item(Datatable.SAP.AP.Supplier_Frg.LC_Total).Value
                        Else
                            oDocumentExpenses.LineTotal = .Item(Datatable.SAP.AP.Supplier_Frg.FC_Total).Value
                        End If
                        oRecSet.MoveNext()
                    Loop
                End If
            End With

            If oDocuments.Add = 0 Then
                Dim NewObjKey As Integer

                NewObjKey = Company.GetNewObjectKey
                MyBase.ToMapping(NewObjKey, 18, oDocEntry)

                Return Company.GetNewObjectKey
            Else
                oLogMessage.AddFailureLine(oBatchID, Company.GetLastErrorDescription)

                ToErrorTable(oDocEntry, _
                             18, _
                             Company.GetLastErrorCode, _
                             "Unable to Create A/P Service Invoice", _
                             Company.GetLastErrorDescription)
                Return 0
            End If
        End Function
        Function DeleteDocument(ByVal oBatchID As String) As Integer
            Dim oMapping As Datatable.SAP.Draft_Mapping

            Dim oRecSet As Recordset
            Dim oRinDocuments, oInvDocuments As SAPbobsCOM.Documents
            Dim oRinDocumentLines, oInvDocumentLines As SAPbobsCOM.Document_Lines
            Dim oDraftKey As Integer
            Dim oInvEntry, oInvObjType As Integer

            oDraftKey = Right(oBatchID, oBatchID.Length - 1)
            oMapping = New Datatable.SAP.Draft_Mapping
            oMapping.getPurchaseInvoice(oDraftKey)
            oRecSet = oMapping.Execute()

            'Get the A/P Item Invoice Document Entry and Object in SAP
            With oRecSet
                If oRecSet.RecordCount = 0 Then
                    Throw New BaseException(BaseException.ErrorType.Normal, "DELETE_0001", "No Record base on Document Draft Key [" & oDraftKey & "]")
                End If
                Do Until .EoF
                    oInvEntry = .Fields.Item(Datatable.SAP.Draft_Mapping._DocEntry).Value
                    oInvObjType = .Fields.Item(Datatable.SAP.Draft_Mapping._ObjType).Value

                    .MoveNext()
                Loop
            End With

            oInvDocuments = currentCompany.GetBusinessObject(BoObjectTypes.oPurchaseInvoices)
            oRinDocuments = currentCompany.GetBusinessObject(BoObjectTypes.oPurchaseCreditNotes)

            If Not oInvDocuments.GetByKey(oInvEntry) Then
                Throw New BaseException(BaseException.ErrorType.Normal, "DELETE_0002", "No A/P Invoice was found base on Invoice Key [" & oInvEntry & "]")
            End If

            oInvDocumentLines = oInvDocuments.Lines

            'Assign Value into A/P Credit Memo
            oRinDocuments.CardCode = oInvDocuments.CardCode
            oRinDocuments.CardName = oInvDocuments.CardName
            oRinDocuments.DocDate = oInvDocuments.DocDate
            oRinDocuments.DocDueDate = oInvDocuments.DocDueDate
            oRinDocuments.TaxDate = oInvDocuments.TaxDate
            oRinDocuments.NumAtCard = oInvDocuments.NumAtCard
            oRinDocuments.DocType = oInvDocuments.DocType

            oRinDocumentLines = oRinDocuments.Lines

            For i As Integer = 0 To oInvDocumentLines.Count - 1
                oInvDocumentLines.SetCurrentLine(i)

                If i > 0 Then
                    oRinDocumentLines.Add()
                End If
                oRinDocumentLines.BaseEntry = oInvDocuments.DocEntry
                oRinDocumentLines.BaseLine = oInvDocumentLines.LineNum
                oRinDocumentLines.BaseType = 18
            Next

            oRinDocuments.DiscountPercent = oInvDocuments.DiscountPercent

            If oRinDocuments.Add = 0 Then
                Return Company.GetNewObjectKey
            Else
                oLogMessage.AddFailureLine(oBatchID, Company.GetLastErrorDescription)

                ToErrorTable(oDraftKey, _
                             18, _
                             Company.GetLastErrorCode, _
                             "Unable to Create A/P Credit Memo", _
                             Company.GetLastErrorDescription)
                Return 0
            End If
        End Function
        Function CloseDraft(ByVal pBatchID As String) As Integer
            Dim oDocEntry As Integer
            Dim oDraft As New Datatable.SAP.AP.Supplier_Hdr

            oDocEntry = Right(pBatchID, pBatchID.Length - 1)

            oDraft.getPurchaseInvoice(oDocEntry)
            oDraft.DocStatus = "C"
            oDraft.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)
        End Function
        Function DeleteDraft(ByVal pBatchID As String) As Integer
            Dim oDocEntry As Integer
            Dim oDraft As New Datatable.SAP.AP.Supplier_Hdr

            oDocEntry = Right(pBatchID, pBatchID.Length - 1)

            oDraft.getPurchaseInvoice(oDocEntry)
            oDraft.DocStatus = "D"
            oDraft.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)
        End Function
        Function UpdateFlex(ByVal pBatchID As String) As Integer
            Dim oPIVOU As Datatable.Flex.PIVOU

            oPIVOU = New Datatable.Flex.PIVOU
            oPIVOU.getItemAP(pBatchID)
            'oPIVOU.PCMS_Remark = "Operation is Success"
            oPIVOU.PCMS_Status = "C"
            oPIVOU.PCMS_UpdateDate = Now

            oFlexConnection.Process(oPIVOU.UpdateQuery & " " & oPIVOU.filterQuery)
        End Function
#End Region

    End Class
End Namespace