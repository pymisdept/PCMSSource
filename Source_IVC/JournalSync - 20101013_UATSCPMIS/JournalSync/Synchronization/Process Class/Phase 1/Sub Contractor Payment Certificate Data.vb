Imports SAPbobsCOM
Imports CPS.CPSLIB.Debug
Namespace SyncMainClass
    ''' <summary>
    ''' Sub Contractor Payment Certificate Data, Inherits Snchronization Class
    ''' </summary>
    ''' <remarks></remarks>
    Class P_SrvInvoice
        Inherits [Interface].Synchronization

        Private ReadOnly mJobReference As String = "A/P Service Invoice Synchronization (Fiex & SAP)"
        Private LocalCurrency As String

        Public Sub New(ByVal pCompany As SAPbobsCOM.Company)
            MyBase.New(pCompany)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            MyBase.JobName = mJobReference
            ObjType = 18
            oLogMessage.FileName = "AP-SrvInvoice"

            Dim sboBob As SBObob = pCompany.GetBusinessObject(BoObjectTypes.BoBridge)
            LocalCurrency = ""
            LocalCurrency = Trim(sboBob.GetLocalCurrency.Fields.Item(0).Value)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Sub-Contract Payment Cert", TimeSet.Status.Finish)
        End Sub

#Region "Data Mapping"

        Function result(ByVal pFields As SAPbobsCOM.Fields, ByVal pField_Dtls As SAPbobsCOM.Fields) As ExportPCMS
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oReturnResult As ExportPCMS
            Dim oDocEntry As Integer
            Dim oDocCurrency As String

            oReturnResult = New ExportPCMS
            With pFields
                'Get the document entry form the record set result
                oDocEntry = .Item(Datatable.SAP.AP.SubCon_Hdr.DocEntry).Value
                oDocCurrency = Trim(.Item(Datatable.SAP.AP.SubCon_Hdr.DocCur).Value)

                oReturnResult.RefNo = oDocEntry
                oReturnResult.SubsidiaryCode = .Item(Datatable.SAP.AP.SubCon_Hdr.SubsiCode).Value
                'Modify by michael, begin, 20140115
                'Supplier Payment Certificate Data
                If .Item(Datatable.SAP.AP.SubCon_Hdr._U_RevFlag).Value = "R" Then
                    oReturnResult.CertEntry = "T" & RSet(.Item(Datatable.SAP.AP.SubCon_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                Else
                    oReturnResult.CertEntry = "S" & RSet(.Item(Datatable.SAP.AP.SubCon_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                End If
                'Modify by michael, end, 20140115
                'oReturnResult.VoucherType  (GJ)
                'Karrson: Change
                'oReturnResult.ValuationDate = .Item(Datatable.SAP.AP.SubCon_Hdr.DocDate).Value
                oReturnResult.ValuationDate = .Item(Datatable.SAP.AP.SubCon_Hdr.DocumentDate).Value

                'oReturnResult.Description  (General Journal)
                oReturnResult.AcctCode = pField_Dtls.Item(Datatable.SAP.AP.SubCon_Dtl.AcctCode).Value
                oReturnResult.BPCode = .Item(Datatable.SAP.AP.SubCon_Hdr.CardCode).Value
                oReturnResult.ProjectCode = pField_Dtls.Item(Datatable.SAP.AP.SubCon_Dtl.PrjCode).Value
                oReturnResult.CostCode = pField_Dtls.Item(Datatable.SAP.AP.SubCon_Dtl.AcctCode).Value
                oReturnResult.CertNumber = .Item(Datatable.SAP.AP.SubCon_Hdr.PCMSDocNum).Value
                oReturnResult.VInvoiceNo = Nothing
                'oReturnResult.DocType
                oReturnResult.CertDate = .Item(Datatable.SAP.AP.SubCon_Hdr.DocumentDate).Value
                oReturnResult.CertDueDate = .Item(Datatable.SAP.AP.SubCon_Hdr.DocDueDate).Value
                'oReturnResult.DocCurrency = LocalCurrency
                oReturnResult.DocCurrency = .Item(Datatable.SAP.AP.SubCon_Hdr.DocCur).Value
                oReturnResult.SubCon_No = Right(.Item(Datatable.SAP.AP.SubCon_Hdr.PCMSDocNum).Value, 6)
                oReturnResult.RefCardCode = pField_Dtls.Item(Datatable.SAP.AP.SubCon_Dtl._U_RefCardCode).Value

                'Calculate the Total Before Discount 
                Dim oDocTotal As Double, oDisPercent As Double
                'If Me.LocalCurrency.ToUpper = oDocCurrency.ToUpper Then
                '    oReturnResult.DocRate = 1
                '    oDocTotal = pField_Dtls.Item(Datatable.SAP.AP.SubCon_Dtl.LC_Total).Value
                'Else
                '    oReturnResult.DocRate = pField_Dtls.Item(Datatable.SAP.AP.SubCon_Dtl.DocRate).Value
                '    oDocTotal = pField_Dtls.Item(Datatable.SAP.AP.SubCon_Dtl.FC_Total).Value
                'End If

                oReturnResult.DocRate = 1
                oDocTotal = pField_Dtls.Item(Datatable.SAP.AP.SubCon_Dtl._StockSum).Value
                oDisPercent = .Item(Datatable.SAP.AP.SubCon_Dtl.DisPercent).Value

                oReturnResult.TotalBefDis = oDocTotal '/ (1 + oDisPercent / 100)
                oReturnResult.DocTotal = oDocTotal
                'oReturnResult.Allocation

                'Karrson: Change
                oReturnResult.CertData = .Item(Datatable.SAP.AP.SubCon_Hdr.DocSubject).Value

                'Dim oCertData As String = ""
                'Dim oPANo As String, oCONo As String, oPONo As String

                'oPANo = .Item(Datatable.SAP.AP.SubCon_Hdr._U_PANo).Value
                'oCONo = .Item(Datatable.SAP.AP.SubCon_Hdr._U_CONo).Value
                'oPONo = .Item(Datatable.SAP.AP.SubCon_Hdr._U_PONo).Value

                'If Not oPANo = "" Then
                '    oCertData &= "PA No. " & oPANo & "/"
                'End If

                'If Not oCONo = "" Then
                '    oCertData &= "CO No. " & oCONo & "/"
                'End If

                'If Not oPONo = "" Then
                '    oCertData &= "PO No. " & oPONo & "/"
                'End If

                'If Not oCertData = "" Then
                '    oCertData = Left(oCertData, oCertData.Length - 1)
                'End If

                'oReturnResult.CertData = oCertData
            End With
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            Return oReturnResult
        End Function

        Function CredtorResult(ByVal pFields As SAPbobsCOM.Fields) As ExportPCMS
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oReturnResult As ExportPCMS
            Dim oDocEntry As Integer
            Dim oDocCurrency As String

            oReturnResult = New ExportPCMS
            With pFields
                'Get the document entry form the record set result
                oDocEntry = .Item(Datatable.SAP.AP.SubCon_Hdr.DocEntry).Value
                oDocCurrency = Trim(.Item(Datatable.SAP.AP.SubCon_Hdr.DocCur).Value)

                oReturnResult.RefNo = oDocEntry
                oReturnResult.SubsidiaryCode = .Item(Datatable.SAP.AP.SubCon_Hdr.SubsiCode).Value
                'Modify by michael, begin, 20140115
                'Supplier Payment Certificate Data
                If .Item(Datatable.SAP.AP.SubCon_Hdr._U_RevFlag).Value = "R" Then
                    oReturnResult.CertEntry = "T" & RSet(.Item(Datatable.SAP.AP.SubCon_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                Else
                    oReturnResult.CertEntry = "S" & RSet(.Item(Datatable.SAP.AP.SubCon_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                End If
                'Modify by michael, end, 20140115
                'oReturnResult.VoucherType  (GJ)
                'Karrson: Change
                'oReturnResult.ValuationDate = .Item(Datatable.SAP.AP.SubCon_Hdr.DocDate).Value
                oReturnResult.ValuationDate = .Item(Datatable.SAP.AP.SubCon_Hdr.DocumentDate).Value

                'oReturnResult.Description  (General Journal)
                oReturnResult.AcctCode = ControlAcct(.Item(Datatable.SAP.AP.SubCon_Hdr.CardCode).Value)
                oReturnResult.BPCode = .Item(Datatable.SAP.AP.SubCon_Hdr.CardCode).Value
                oReturnResult.ProjectCode = .Item(Datatable.SAP.AP.SubCon_Hdr.Project).Value
                oReturnResult.CostCode = ControlAcct(.Item(Datatable.SAP.AP.SubCon_Hdr.CardCode).Value)
                oReturnResult.CertNumber = .Item(Datatable.SAP.AP.SubCon_Hdr.PCMSDocNum).Value
                oReturnResult.VInvoiceNo = Nothing
                'oReturnResult.DocType
                oReturnResult.CertDate = .Item(Datatable.SAP.AP.SubCon_Hdr.DocumentDate).Value
                oReturnResult.CertDueDate = .Item(Datatable.SAP.AP.SubCon_Hdr.DocDueDate).Value
                'oReturnResult.DocCurrency = LocalCurrency
                oReturnResult.DocCurrency = .Item(Datatable.SAP.AP.SubCon_Hdr.DocCur).Value
                oReturnResult.RefCardCode = Nothing '.Item(Datatable.SAP.AP.SubCon_Dtl._U_RefCardCode).Value

                'Calculate the Total Before Discount 
                Dim oDocTotal As Double, oDisPercent As Double
                'If Me.LocalCurrency.ToUpper = oDocCurrency.ToUpper Then
                '    oDocTotal = .Item(Datatable.SAP.AP.SubCon_Hdr.DocTotal).Value
                'Else
                '    oDocTotal = .Item(Datatable.SAP.AP.SubCon_Hdr.DocTotalFC).Value
                'End If

                oDocTotal = .Item(Datatable.SAP.AP.SubCon_Hdr.DocTotal).Value
                oDisPercent = .Item(Datatable.SAP.AP.SubCon_Hdr.DisPercent).Value

                'oReturnResult.TotalBefDis = oDocTotal / (1 + oDisPercent / 100)
                'oReturnResult.DocRate = .Item(Datatable.SAP.AP.SubCon_Hdr.DocRate).Value
                'oReturnResult.DocTotal = oDocTotal

                oReturnResult.TotalBefDis = oDocTotal
                oReturnResult.DocRate = 1
                oReturnResult.DocTotal = oDocTotal

                'oReturnResult.Allocation
                oReturnResult.CertData = .Item(Datatable.SAP.AP.SubCon_Hdr.DocSubject).Value
            End With
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            Return oReturnResult
        End Function

        Function FreightResult(ByVal pField_Hdr As Fields) As ExportPCMS()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oDocEntry As Integer
            Dim oDocCurrency As String
            Dim Itm_PCH6 As Datatable.SAP.AP.SubCon_Frg
            Dim oRecSet As SAPbobsCOM.Recordset
            Dim oExportPCMSs(), oExportPCMS As ExportPCMS

            oDocEntry = pField_Hdr.Item(Datatable.SAP.AP.SubCon_Hdr.DocEntry).Value
            oDocCurrency = pField_Hdr.Item(Datatable.SAP.AP.SubCon_Hdr.DocCur).Value

            Itm_PCH6 = New Datatable.SAP.AP.SubCon_Frg
            Itm_PCH6.getFreightEntry(oDocEntry)
            oExportPCMSs = Nothing

            oRecSet = Itm_PCH6.Execute
            Do Until oRecSet.EoF
                oExportPCMS = New ExportPCMS

                With oExportPCMS
                    .RefNo = oDocEntry
                    .SubsidiaryCode = pField_Hdr.Item(Datatable.SAP.AP.SubCon_Hdr.SubsiCode).Value
                    'Modify by michael, begin, 20140115
                    'Supplier Payment Certificate Data
                    If pField_Hdr.Item(Datatable.SAP.AP.SubCon_Hdr._U_RevFlag).Value = "R" Then
                        .CertEntry = "T" & RSet(pField_Hdr.Item(Datatable.SAP.AP.SubCon_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                    Else
                        .CertEntry = "S" & RSet(pField_Hdr.Item(Datatable.SAP.AP.SubCon_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                    End If
                    'Modify by michael, end, 20140115
                    'oReturnResult.VoucherType  (GJ)
                    'karrson: Change
                    '.ValuationDate = pField_Hdr.Item(Datatable.SAP.AP.SubCon_Hdr.DocDate).Value
                    .ValuationDate = pField_Hdr.Item(Datatable.SAP.AP.SubCon_Hdr.DocumentDate).Value

                    'oReturnResult.Description  (General Journal)
                    .AcctCode = Itm_PCH6.getAcctCode(oRecSet.Fields.Item(Datatable.SAP.AP.SubCon_Frg.FreightCode).Value)
                    .BPCode = pField_Hdr.Item(Datatable.SAP.AP.SubCon_Hdr.CardCode).Value
                    .ProjectCode = pField_Hdr.Item(Datatable.SAP.AP.SubCon_Hdr.Project).Value
                    .CostCode = Itm_PCH6.getAcctCode(oRecSet.Fields.Item(Datatable.SAP.AP.SubCon_Frg.FreightCode).Value)
                    .CertNumber = pField_Hdr.Item(Datatable.SAP.AP.SubCon_Hdr.PCMSDocNum).Value
                    .VInvoiceNo = Nothing
                    'oReturnResult.DocType
                    .CertDate = pField_Hdr.Item(Datatable.SAP.AP.SubCon_Hdr.DocumentDate).Value
                    .CertDueDate = pField_Hdr.Item(Datatable.SAP.AP.SubCon_Hdr.DocDueDate).Value
                    .DocCurrency = oDocCurrency

                    'Calculate the Total Before Discount 
                    Dim oDocTotal As Double
                    'If Me.LocalCurrency.ToUpper = oDocCurrency.ToUpper Then
                    oDocTotal = oRecSet.Fields.Item(Datatable.SAP.AP.SubCon_Frg.LC_Total).Value
                    'Else
                    'oDocTotal = oRecSet.Fields.Item(Datatable.SAP.AP.SubCon_Frg.FC_Total).Value
                    'End If
                    .TotalBefDis = oDocTotal

                    .DocRate = pField_Hdr.Item(Datatable.SAP.AP.SubCon_Hdr.DocRate).Value
                    .DocTotal = oDocTotal

                    'oReturnResult.Allocation
                    'Karrson: Change
                    '.CertData = Nothing

                    .CertData = oRecSet.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.DocSubject).Value
                End With

                If oExportPCMSs Is Nothing Then
                    ReDim oExportPCMSs(0)
                Else
                    ReDim Preserve oExportPCMSs(oExportPCMSs.Length)
                End If
                oExportPCMSs(oExportPCMSs.Length - 1) = oExportPCMS

                oRecSet.MoveNext()
            Loop
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            Return oExportPCMSs
        End Function

#End Region

        ''' <summary>
        ''' Import data from PCMS to Flex structure
        ''' </summary>
        ''' <param name="pExportPCMS">Collection data from PCMS Structure</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function fDataMapping(ByVal pExportPCMS As ExportPCMS) As ImportFLEX
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
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
                'Karrson Change:
                oImport.AcctCode = ExAcctcode(.AcctCode, pExportPCMS).Trim
                'oImport.AcctCode = MapAcctCode(.AcctCode).Trim
                oImport.AnalysisCode1 = Left(.BPCode.Trim, 8)
                oImport.AnalysisCode2 = Nothing
                oImport.AnalysisCode3 = .ProjectCode

                oImport.AnalysisCode4 = Right(.AcctCode, 5) & "000"
                oImport.AnalysisCode5 = [Interface].Synchronization.GetAnalysisCode5(.AcctCode)
                oImport.DocumentNo = .CertNumber
                oImport.AltDocNumber = .VInvoiceNo
                oImport.DocType = ExportPCMS.DocType
                oImport.DocDate = .ValuationDate
                oImport.DocDueDate = .CertDueDate
                oImport.Currency = .DocCurrency

                'oImport.Allocation = "C"
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
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            Return oImport
        End Function

        Function fCreditor(ByVal pExportPCMS As ExportPCMS) As ImportFLEX
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
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
                oImport.AcctCode = ExAcctcode(ControlAcct(.BPCode), pExportPCMS).Trim
                oImport.AnalysisCode1 = Left(.BPCode.Trim, 8)
                oImport.AnalysisCode2 = Nothing
                oImport.AnalysisCode3 = .ProjectCode
                oImport.AnalysisCode4 = Nothing
                oImport.AnalysisCode5 = Nothing
                oImport.DocumentNo = .CertNumber
                oImport.AltDocNumber = .VInvoiceNo
                oImport.DocType = ExportPCMS.DocType
                oImport.DocDate = .ValuationDate
                oImport.DocDueDate = .CertDueDate
                oImport.Currency = .DocCurrency

                'oImport.Allocation = "D"
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
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            Return oImport
        End Function

        Public Overrides Sub Export()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            'Declare Datatable 
            Dim oPTVOU As New Datatable.Flex.PTVOU
            Dim Srv_OPCH As Datatable.SAP.AP.SubCon_Hdr
            Dim Srv_PCH1 As New Datatable.SAP.AP.SubCon_Dtl
            Dim oSynchHistory As New Datatable.SAP.Sync_History

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
                Srv_OPCH = New Datatable.SAP.AP.SubCon_Hdr
                recset_Hdr = Srv_OPCH.Execute
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & "Query: " & Srv_OPCH.SelectQuery & " " & Srv_OPCH.filterQuery & " " & Srv_OPCH.OrderByQuery, TimeSet.Status.Start)
                Do Until recset_Hdr.EoF
                    Try
                        oDocEntry = recset_Hdr.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.DocEntry).Value
                        oDocCurrency = Trim(recset_Hdr.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.DocCur).Value)

                        If Chk_ErrOverLap(oDocEntry, eObjType.ot_PurchaseInvoice) Then
                            GoTo Move_Next
                        End If

                        'Begin transaction
                        Me.StartTransaction()

                        'Get Document Detail by Document entry
                        Srv_PCH1 = New Datatable.SAP.AP.SubCon_Dtl
                        Srv_PCH1.getPurchaseInvoice(oDocEntry)
                        recset_Dtl = Srv_PCH1.Execute

                        oExports = Nothing
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & "Detail Query: " & Srv_PCH1.SelectQuery & " " & Srv_PCH1.filterQuery & " " & Srv_PCH1.OrderByQuery, TimeSet.Status.Start)
                        WriteDebug("SubCon Detail Count: " & recset_Dtl.RecordCount.ToString)
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
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & "Detail Query: " & Srv_PCH1.SelectQuery & " " & Srv_PCH1.filterQuery & " " & Srv_PCH1.OrderByQuery, TimeSet.Status.Finish)
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
                        oExport.SubCon_No = Right(Trim(recset_Hdr.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.PCMSDocNum).Value), 6)
                        oImport = fCreditor(oExport)
                        'Import Data into Datatable PTVOU
                        'Karrson: Debug
                        WriteDebug("ToFlex")

                        Me.ToFlex(oImport)
                        WriteDebug("SyncHistory")
                        oSynchHistory = New Datatable.SAP.Sync_History
                        ' Karrson: Change
                        'oSynchHistory.Add_Submitted("S" & RSet(oDocEntry.ToString.Trim, 9).Replace(" ", "0"))
                        oSynchHistory.Add_Acknowledge("S" & RSet(oDocEntry.ToString.Trim, 9).Replace(" ", "0"))

                        Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                        oLogMessage.AddSuccessLine("[EXPORT]AP " & oDocEntry, "Operation Success")
                    Catch b_ex As BaseException
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception: " & b_ex.Message, TimeSet.Status.Start)
                        oLogMessage.AddExceptionSkip("[EXPORT]AP " & oDocEntry, b_ex.toString)

                        If Not oDocEntry = 0 Then
                            ToErrorTable(oDocEntry, _
                                         18, _
                                         -9999, _
                                         "[EXPORT]AP " & oDocEntry, _
                                         b_ex.toString)
                        End If

                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception", TimeSet.Status.Finish)
                    Catch ex As Exception
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception: " & ex.Message, TimeSet.Status.Start)
                        oLogMessage.AddExceptionSkip("[EXPORT]AP " & oDocEntry, ex.ToString)

                        If Not oDocEntry = 0 Then
                            ToErrorTable(oDocEntry, _
                                         18, _
                                         -9999, _
                                         "[EXPORT]AP " & oDocEntry, _
                                         ex.ToString)
                        End If

                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception", TimeSet.Status.Finish)
                    End Try

Move_Next:
                    recset_Hdr.MoveNext()
                Loop
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & "Query: " & Srv_OPCH.SelectQuery & " " & Srv_OPCH.filterQuery & " " & Srv_OPCH.OrderByQuery, TimeSet.Status.Finish)
            Catch b_ex As BaseException
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception: " & b_ex.Message, TimeSet.Status.Start)
                oLogMessage.AddExceptionSkip("[EXPORT]AP " & oDocEntry, b_ex.toString)

                If Not oDocEntry = 0 Then
                    ToErrorTable(oDocEntry, _
                                 18, _
                                 -9999, _
                                 "[EXPORT]AP " & oDocEntry, _
                                 b_ex.toString)
                End If
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception: " & b_ex.Message, TimeSet.Status.Finish)
            Catch ex As Exception
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception: " & ex.Message, TimeSet.Status.Start)
                oLogMessage.AddExceptionSkip("[EXPORT]AP " & oDocEntry, ex.ToString)

                If Not oDocEntry = 0 Then
                    ToErrorTable(oDocEntry, _
                                 18, _
                                 -9999, _
                                 "[EXPORT]AP " & oDocEntry, _
                                 ex.ToString)
                End If
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception: " & ex.Message, TimeSet.Status.Finish)
            End Try
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            oLogMessage.AddReferenceLine("EXPORT", "--------------------------------")
        End Sub

        Public Overrides Sub Import()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Try
                Me.Import_Posted()
                'Add by michael, begin, 20140115
                Me.Import_Posted_Reversed()
                'Add by michael, end, 20140115
                Me.Import_Exception()
                Me.Import_Reject()
                'Me.Import_Delete()
            Catch ex As Exception
                oLogMessage.AddExceptionSkip("MAIN", ex.ToString)
            End Try
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Sub

        'Add by michael, begin
        'Process Success Data with Accept
        Public Sub Import_Posted()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oPIVOU As Datatable.Flex.PIVOU
            Dim oDataTable As System.Data.DataTable
            Dim BatchID As String = ""
            Dim sqlStr As String
            Dim SyncHistory As Datatable.SAP.Sync_History
            Dim oNewObjKey As Integer

            oLogMessage.AddReferenceLine("POSTED", "Create New A/P Service Invoice in SAP System.")
            oPIVOU = New Datatable.Flex.PIVOU

            oPIVOU.getSrvAP(Datatable.Flex.PIVOU.flexStatus.fs_Accepted)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery

            WriteDebug("Sub-Contact Payment Cert Import Posted Query: " & sqlStr)
            oDataTable = oFlexConnection.DataTable(sqlStr)

            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim

                        If Chk_ErrOverLap(BatchID, eObjType.ot_PurchaseInvoice) Then
                            GoTo Move_Next
                        End If

                        'Add by Michael, begin
                        Dim oRecSet As SAPbobsCOM.Recordset
                        Dim cRevTrans As String
                        Dim cComCode As String = ""
                        Dim cPIVOU_REF_NUM As String = ""
                        Dim nDocEntry As Long = 0

                        Dim _dtBatch As System.Data.DataTable
                        _dtBatch = oFlexConnection.DataTable(oPIVOU.BatchSql(BatchID))

                        WriteDebug("SubCon-Temp-1")
                        For Each ooDataRow As System.Data.DataRow In _dtBatch.Rows
                            cComCode = ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_COM_CDE)

                            If Len(Trim(ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_REF_NUM))) > 3 Then
                                cPIVOU_REF_NUM = Left(Trim(ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_REF_NUM)), Len(Trim(ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_REF_NUM))) - 3) & "/" & Right(Trim(ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_REF_NUM)), 3)
                            Else
                                cPIVOU_REF_NUM = ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_REF_NUM)
                            End If
                        Next
                        WriteDebug("SubCon-Temp-2 : " & cPIVOU_REF_NUM)
                        sqlStr = "select ISNULL(DocEntry, 0) as DocEntry, ISNULL(Rev_DocEntry,0) as Rev_DocEntry from PCMS_FE.PCMS800.dbo.DocumentProperty where DocNum = '" & cPIVOU_REF_NUM.Trim & "' and DocStatus = 'PPFA'"
                        'sqlStr = "select ISNULL(DocEntry, 0) as DocEntry, ISNULL(Rev_DocEntry,0) as Rev_DocEntry from PCMS_FE.MCPCMS100.dbo.DocumentProperty where DocNum = '" & cPIVOU_REF_NUM.Trim & "' and DocStatus = 'PPFA'"
                        WriteDebug("SubCon-Test1 : " & sqlStr)
                        oRecSet = commonRecordSet.execute(sqlStr)

                        WriteDebug("SubCon-Test2 : " & String.Format(oRecSet.RecordCount).Trim)

                        If Not (oRecSet.RecordCount = 0) Then
                            WriteDebug("SubCon-Test3")
                            If oRecSet.Fields.Item("REV_DOCENTRY").Value = 0 Then
                                cRevTrans = "N"
                                nDocEntry = oRecSet.Fields.Item("DOCENTRY").Value
                            Else
                                cRevTrans = "Y"
                                nDocEntry = oRecSet.Fields.Item("REV_DOCENTRY").Value
                            End If
                            WriteDebug("SubCon-Test4")
                            'Add by Michael, end

                            'Start Transaction
                            Me.StartTransaction()

                            'Draw Data into CPSFIN for Backup
                            Me.INSERT_CPSFIN(BatchID, cRevTrans, nDocEntry)

                            'Create A/P Item Invoice in SAP
                            oNewObjKey = Me.CreateDocument(BatchID)

                            If Not oNewObjKey = 0 Then
                                'Update Document Draft Status in [C] when Delete
                                Me.CloseDraft(BatchID)

                                'Update PIVOU table in Flex Server
                                Me.UpdateFlex(BatchID)

                                'Log History into Synchronization History table
                                SyncHistory = New Datatable.SAP.Sync_History
                                'Karrson: Remove
                                'SyncHistory.Add_Acknowledge(BatchID)
                                SyncHistory.Add_Posted(BatchID)

                                'End Transaction
                                Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                                oLogMessage.AddSuccessLine(BatchID, "Create Document Success")
                            Else
                                Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                            End If
                            'Add by Michael, begin
                        End If
                        'Add by Michael, end
                    Catch b_ex As BaseException
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception: " & b_ex.Message, TimeSet.Status.Start)
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, b_ex.toString)
                        'karrson: Debug
                        WriteDebug("LN 592, TO Error Table")
                        ToErrorTable(BatchID, _
                                     18, _
                                     -9999, _
                                     "[IMPORT]AP " & BatchID, _
                                     b_ex.toString)
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception: " & b_ex.Message, TimeSet.Status.Finish)
                    Catch ex As Exception
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception: " & ex.Message, TimeSet.Status.Start)
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, ex.ToString)
                        WriteDebug("LN 601, TO Error Table")
                        ToErrorTable(BatchID, _
                                     18, _
                                     -9999, _
                                     "[IMPORT]AP " & BatchID, _
                                     ex.ToString)
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception: " & ex.Message, TimeSet.Status.Finish)
                    End Try
Move_Next:
                Next
            End If
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            oLogMessage.AddReferenceLine("POSTED", "-----------------------------------------")
        End Sub

        'Process Return data with Exception
        Public Sub Import_Exception()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oPIVOU As Datatable.Flex.PIVOU
            Dim oDataTable As System.Data.DataTable
            Dim BatchID As String = ""
            Dim sqlStr As String
            Dim SyncHistory As Datatable.SAP.Sync_History

            oLogMessage.AddReferenceLine("ERROR", "Error the document draft from backend table in SAP.")
            oPIVOU = New Datatable.Flex.PIVOU

            oPIVOU.getSrvAP(Datatable.Flex.PIVOU.flexStatus.fs_Error)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery
            oDataTable = oFlexConnection.DataTable(sqlStr)
            WriteDebug("Sub-Contact Payment Cert Import Exception Query: " & sqlStr)
            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim

                        If Chk_ErrOverLap(BatchID, eObjType.ot_PurchaseInvoice) Then
                            GoTo Move_Next
                        End If

                        'Start Transaction
                        Me.StartTransaction()

                        'Update Document Draft Status in [D] when Delete
                        Me.DeleteDraft(BatchID)

                        'Update PIVOU table in Flex Server
                        Me.UpdateFlex(BatchID)

                        'Log History into Synchronization History table
                        SyncHistory = New Datatable.SAP.Sync_History
                        'Karrson: Remove
                        'SyncHistory.Add_Acknowledge(BatchID)
                        SyncHistory.Add_Rejected(BatchID)

                        'End Transaction
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                        oLogMessage.AddSuccessLine(BatchID, "Delete Document Draft Success")
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
Move_Next:
                Next

            End If
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            oLogMessage.AddReferenceLine("ERROR", "----------------------------------------------------")
        End Sub

        'Process Return Data with Reject
        Public Sub Import_Reject()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oPIVOU As Datatable.Flex.PIVOU
            Dim oDataTable As System.Data.DataTable
            Dim BatchID As String = ""
            Dim sqlStr As String
            Dim SyncHistory As Datatable.SAP.Sync_History

            oLogMessage.AddReferenceLine("REJECT", "Reject the document draft from backend table in SAP.")
            oPIVOU = New Datatable.Flex.PIVOU

            oPIVOU.getSrvAP(Datatable.Flex.PIVOU.flexStatus.fs_Rejected)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery
            oDataTable = oFlexConnection.DataTable(sqlStr)
            ' KArrson: WriteDEbug
            WriteDebug(sqlStr)
            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim

                        If Chk_ErrOverLap(BatchID, eObjType.ot_PurchaseInvoice) Then
                            GoTo Move_Next
                        End If

                        'Start Transaction
                        Me.StartTransaction()

                        'Update Document Draft Status in [D] when Delete
                        Me.DeleteDraft(BatchID)

                        'Update PIVOU table in Flex Server
                        Me.UpdateFlex(BatchID)

                        'Log History into Synchronization History table
                        SyncHistory = New Datatable.SAP.Sync_History
                        'Karrson: Remove
                        'SyncHistory.Add_Acknowledge(BatchID)
                        SyncHistory.Add_Rejected(BatchID)

                        'End Transaction
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                        oLogMessage.AddSuccessLine(BatchID, "Delete Document Draft Success")
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
Move_Next:
                Next
            End If
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            oLogMessage.AddReferenceLine("REJECT", "----------------------------------------------------")
        End Sub

        'Process Return Data with Delete
        Public Sub Import_Delete()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oPIVOU As Datatable.Flex.PIVOU
            Dim oDataTable As System.Data.DataTable
            Dim BatchID As String = ""
            Dim sqlStr As String
            Dim SyncHistory As Datatable.SAP.Sync_History
            Dim oNewObjKey As Integer

            'oLogMessage.AddReferenceLine("DELETE", "Delete Document posted into SAP by A/P Credit memo")
            oLogMessage.AddReferenceLine("DELETE", "Batch Delete Accepted Document by Flex")
            oPIVOU = New Datatable.Flex.PIVOU

            oPIVOU.getSrvAP(Datatable.Flex.PIVOU.flexStatus.fs_Delete)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery
            oDataTable = oFlexConnection.DataTable(sqlStr)

            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim

                        If Chk_ErrOverLap(BatchID, eObjType.ot_PurchaseInvoice) Then
                            GoTo Move_Next
                        End If

                        'Start Transaction
                        Me.StartTransaction()

                        ''Create A/P Credit Memo base on A/P Invoice
                        'oNewObjKey = Me.DeleteDocument(BatchID)

                        'If Not oNewObjKey = 0 Then

                        '    'Update PIVOU table in Flex Server
                        '    Me.UpdateFlex(BatchID)

                        '    'Log History into Synchronization History table
                        '    SyncHistory = New Datatable.SAP.Sync_History
                        '    'Karrson: Remove
                        '    'SyncHistory.Add_Acknowledge(BatchID)
                        '    SyncHistory.Add_Deleted(BatchID)

                        '    'End Transaction
                        '    Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                        '    oLogMessage.AddSuccessLine(BatchID, "Delete Document.")
                        'Else
                        '    Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        'End If

                        'Update Document Draft Status in [D] when Delete
                        Me.DeleteDraft(BatchID)

                        'Update PIVOU table in Flex Server
                        Me.UpdateFlex(BatchID)

                        'Log History into Synchronization History table
                        SyncHistory = New Datatable.SAP.Sync_History
                        SyncHistory.Add_Deleted(BatchID)

                        'End Transaction
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                        oLogMessage.AddSuccessLine(BatchID, "Delete Document Draft Success")

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
Move_Next:
                Next
            End If
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            oLogMessage.AddReferenceLine("DELETE", "--------------------------------------------------")
        End Sub

        'Add by Michael, 20140115, begin
        'Process Success Data with Accept
        Public Sub Import_Posted_Reversed()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oPIVOU As Datatable.Flex.PIVOU
            Dim oDataTable As System.Data.DataTable
            Dim BatchID As String = ""
            Dim sqlStr As String
            Dim SyncHistory As Datatable.SAP.Sync_History
            Dim oNewObjKey As Integer

            oLogMessage.AddReferenceLine("POSTED", "Create New A/P Service Invoice in SAP System.")
            oPIVOU = New Datatable.Flex.PIVOU

            oPIVOU.getSrvAP_Reversed(Datatable.Flex.PIVOU.flexStatus.fs_Accepted)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery

            WriteDebug("Sub-Contact Payment Cert Import Posted Query: " & sqlStr)
            oDataTable = oFlexConnection.DataTable(sqlStr)

            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim

                        If Chk_ErrOverLap(BatchID, eObjType.ot_PurchaseInvoice) Then
                            GoTo Move_Next
                        End If

                        'Add by Michael, begin
                        Dim oRecSet As SAPbobsCOM.Recordset
                        Dim cRevTrans As String
                        Dim cComCode As String = ""
                        Dim cPIVOU_REF_NUM As String = ""
                        Dim nDocEntry As Long = 0

                        Dim _dtBatch As System.Data.DataTable
                        _dtBatch = oFlexConnection.DataTable(oPIVOU.BatchSql(BatchID))

                        WriteDebug("SubCon-Temp-1")
                        For Each ooDataRow As System.Data.DataRow In _dtBatch.Rows
                            cComCode = ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_COM_CDE)

                            If Len(Trim(ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_REF_NUM))) > 3 Then
                                cPIVOU_REF_NUM = Left(Trim(ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_REF_NUM)), Len(Trim(ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_REF_NUM))) - 3) & "/" & Right(Trim(ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_REF_NUM)), 3)
                            Else
                                cPIVOU_REF_NUM = ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_REF_NUM)
                            End If
                        Next
                        WriteDebug("SubCon-Temp-2 : " & cPIVOU_REF_NUM)
                        sqlStr = "select ISNULL(DocEntry, 0) as DocEntry, ISNULL(Rev_DocEntry,0) as Rev_DocEntry from PCMS_FE.PCMS800.dbo.DocumentProperty where DocNum = '" & cPIVOU_REF_NUM.Trim & "' and DocStatus = 'PPFA'"
                        'sqlStr = "select ISNULL(DocEntry, 0) as DocEntry, ISNULL(Rev_DocEntry,0) as Rev_DocEntry from PCMS_FE.MCPCMS100.dbo.DocumentProperty where DocNum = '" & cPIVOU_REF_NUM.Trim & "' and DocStatus = 'PPFA'"
                        WriteDebug("SubCon-Test1 : " & sqlStr)
                        oRecSet = commonRecordSet.execute(sqlStr)

                        WriteDebug("SubCon-Test2 : " & String.Format(oRecSet.RecordCount).Trim)

                        If Not (oRecSet.RecordCount = 0) Then
                            WriteDebug("SubCon-Test3")
                            If oRecSet.Fields.Item("REV_DOCENTRY").Value = 0 Then
                                cRevTrans = "N"
                                nDocEntry = oRecSet.Fields.Item("DOCENTRY").Value
                            Else
                                cRevTrans = "Y"
                                nDocEntry = oRecSet.Fields.Item("REV_DOCENTRY").Value
                            End If
                            WriteDebug("SubCon-Test4")
                            'Add by Michael, end

                            'Start Transaction
                            Me.StartTransaction()

                            'Draw Data into CPSFIN for Backup
                            Me.INSERT_CPSFIN(BatchID, cRevTrans, nDocEntry)

                            'Create A/P Item Invoice in SAP
                            oNewObjKey = Me.CreateDocument(BatchID)

                            If Not oNewObjKey = 0 Then
                                'Update Document Draft Status in [C] when Delete
                                Me.CloseDraft(BatchID)

                                'Update PIVOU table in Flex Server
                                Me.UpdateFlex(BatchID)

                                'Log History into Synchronization History table
                                SyncHistory = New Datatable.SAP.Sync_History
                                'Karrson: Remove
                                'SyncHistory.Add_Acknowledge(BatchID)
                                SyncHistory.Add_Posted(BatchID)

                                'End Transaction
                                Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                                oLogMessage.AddSuccessLine(BatchID, "Create Document Success")
                            Else
                                Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                            End If
                            'Add by Michael, begin
                        End If
                        'Add by Michael, end
                    Catch b_ex As BaseException
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception: " & b_ex.Message, TimeSet.Status.Start)
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, b_ex.toString)
                        'karrson: Debug
                        WriteDebug("LN 592, TO Error Table")
                        ToErrorTable(BatchID, _
                                     18, _
                                     -9999, _
                                     "[IMPORT]AP " & BatchID, _
                                     b_ex.toString)
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception: " & b_ex.Message, TimeSet.Status.Finish)
                    Catch ex As Exception
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception: " & ex.Message, TimeSet.Status.Start)
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, ex.ToString)
                        WriteDebug("LN 601, TO Error Table")
                        ToErrorTable(BatchID, _
                                     18, _
                                     -9999, _
                                     "[IMPORT]AP " & BatchID, _
                                     ex.ToString)
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception: " & ex.Message, TimeSet.Status.Finish)
                    End Try
Move_Next:
                Next
            End If
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            oLogMessage.AddReferenceLine("POSTED", "-----------------------------------------")
        End Sub
        'Add by Michael, 20140115, end

#Region "Document Flow in SAP system"
        Function CreateDocument(ByVal oBatchID As String) As Integer
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oDocuments As SAPbobsCOM.Documents
            Dim oDocumentLines As SAPbobsCOM.Document_Lines
            Dim oDocumentExpenses As SAPbobsCOM.DocumentsAdditionalExpenses
            Dim oRecSet As SAPbobsCOM.Recordset
            Dim oDraft As Datatable.SAP.AP.SubCon_Hdr
            Dim oDraft_Dtls As Datatable.SAP.AP.SubCon_Dtl
            Dim oDraft_Frgs As Datatable.SAP.AP.SubCon_Frg

            Dim oDocEntry As Integer
            Dim oBaseEntry, oBaseLine As Integer
            Dim oBaseType As String

            oDocuments = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oPurchaseInvoices)
            oDocumentLines = oDocuments.Lines

            oDraft = New Datatable.SAP.AP.SubCon_Hdr
            oDraft_Dtls = New Datatable.SAP.AP.SubCon_Dtl
            oDraft_Frgs = New Datatable.SAP.AP.SubCon_Frg

            oDocEntry = Right(oBatchID, oBatchID.Length - 1)

            oDraft.getPurchaseInvoice(oDocEntry)
            oDraft_Dtls.getPurchaseInvoice(oDocEntry)
            oDraft_Frgs.getFreightEntry(oDocEntry)

            oRecSet = oDraft.Execute
            'Assign value into Header record
            If oRecSet.RecordCount = 0 Then
                Throw New BaseException(BaseException.ErrorType.System, "CREATEDOC_0470", "No record was found base on Draft Entry [" & oDocEntry & "]")
            End If

            Do Until oRecSet.EoF
                With oRecSet.Fields
                    oDocuments.CardCode = .Item(Datatable.SAP.AP.SubCon_Hdr.CardCode).Value
                    oDocuments.CardName = .Item(Datatable.SAP.AP.SubCon_Hdr.CardName).Value
                    oDocuments.DocDate = .Item(Datatable.SAP.AP.SubCon_Hdr.DocDate).Value
                    oDocuments.DocDueDate = .Item(Datatable.SAP.AP.SubCon_Hdr.DocDueDate).Value
                    oDocuments.TaxDate = .Item(Datatable.SAP.AP.SubCon_Hdr.DocumentDate).Value

                    If Not .Item(Datatable.SAP.AP.SubCon_Hdr.BillToAddress).Value.ToString.Trim = "" Then
                        oDocuments.Address = .Item(Datatable.SAP.AP.SubCon_Hdr.BillToAddress).Value
                    End If

                    If Not .Item(Datatable.SAP.AP.SubCon_Hdr.ShipToAddress).Value.ToString.Trim = "" Then
                        oDocuments.Address2 = .Item(Datatable.SAP.AP.SubCon_Hdr.ShipToAddress).Value
                    End If

                    If .Item(Datatable.SAP.AP.SubCon_Hdr.DocType).Value = "I" Then
                        oDocuments.DocType = BoDocumentTypes.dDocument_Items
                    Else
                        oDocuments.DocType = BoDocumentTypes.dDocument_Service
                    End If

                    If Not .Item(Datatable.SAP.AP.SubCon_Hdr._Indicator).Value = "" Then
                        oDocuments.Indicator = .Item(Datatable.SAP.AP.SubCon_Hdr._Indicator).Value
                    End If

                    'Delivery Instruction
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.DelIns).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.DelIns).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.CntctName).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.CntctName).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.CntctTel).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.CntctTel).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.PCMSDocNum).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.PCMSDocNum).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.DocSubject).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.DocSubject).Value

                    If .Item(Datatable.SAP.AP.SubCon_Hdr.RefDate1).Value > "2000 Jan 30" Then
                        oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.RefDate1).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.RefDate1).Value
                    End If
                    If .Item(Datatable.SAP.AP.SubCon_Hdr.RefDate2).Value > "2000 Jan 30" Then
                        oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.RefDate2).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.RefDate2).Value
                    End If

                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.SubsiCode).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.SubsiCode).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.PayTermDesc).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.PayTermDesc).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.SlpName).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.SlpName).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.SlpTel).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.SlpTel).Value

                    oDocuments.DocCurrency = .Item(Datatable.SAP.AP.SubCon_Hdr.DocCur).Value
                    oDocuments.DocRate = .Item(Datatable.SAP.AP.SubCon_Hdr.DocRate).Value
                    oDocuments.Project = .Item(Datatable.SAP.AP.SubCon_Hdr.Project).Value

                    '14 Mar 2010
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.AppCC).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.AppCC).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.AppClaim).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.AppClaim).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.AppDW).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.AppDW).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.AppMOS).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.AppMOS).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.AppVO).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.AppVO).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.AppDAP).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.AppDAP).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.AppRefundDAP).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.AppRefundDAP).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.AppWork).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.AppWork).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.RetenPrcnt).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.RetenPrcnt).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.RetenMaxAmt).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.RetenMaxAmt).Value

                    '17 April 2018
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.GSTPrcnt).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.GSTPrcnt).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.ThisGST).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.ThisGST).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Hdr.CumGST).Value = .Item(Datatable.SAP.AP.SubCon_Hdr.CumGST).Value
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

                        oBaseEntry = .Item(Datatable.SAP.AP.SubCon_Dtl.BaseEntry).Value
                        oBaseType = .Item(Datatable.SAP.AP.SubCon_Dtl.BaseType).Value
                        oBaseLine = .Item(Datatable.SAP.AP.SubCon_Dtl.BaseLine).Value

                        If oBaseEntry > 0 And Not oBaseType = "" And oBaseLine > 0 Then
                            oDocumentLines.BaseEntry = oBaseEntry
                            oDocumentLines.BaseLine = oBaseLine
                            oDocumentLines.BaseType = oBaseType

                            GoTo Record_MoveNext
                        End If

                        If oDocuments.DocType = BoDocumentTypes.dDocument_Items Then
                            oDocumentLines.ItemCode = .Item(Datatable.SAP.AP.SubCon_Dtl.ItemCode).Value
                            oDocumentLines.Quantity = .Item(Datatable.SAP.AP.SubCon_Dtl.Quantity).Value
                            oDocumentLines.Price = .Item(Datatable.SAP.AP.SubCon_Dtl.UnitPrice).Value
                        End If

                        If .Item(Datatable.SAP.AP.SubCon_Dtl._ShipDate).Value > "2000 Jan 30" Then
                            oDocumentLines.ShipDate = .Item(Datatable.SAP.AP.SubCon_Dtl._ShipDate).Value
                        End If

                        oDocumentLines.AccountCode = .Item(Datatable.SAP.AP.SubCon_Dtl.AcctCode).Value
                        oDocumentLines.ItemDescription = .Item(Datatable.SAP.AP.SubCon_Dtl.ItemName).Value

                        'If LocalCurrency = oDocuments.DocCurrency Then
                        oDocumentLines.LineTotal = .Item(Datatable.SAP.AP.SubCon_Dtl.LC_Total).Value
                        'Else
                        'oDocumentLines.LineTotal = .Item(Datatable.SAP.AP.SubCon_Dtl.FC_Total).Value
                        'End If

                        oDocumentLines.ProjectCode = .Item(Datatable.SAP.AP.SubCon_Dtl.PrjCode).Value

                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_Size).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_Size).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_Size).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_Packing).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_Packing).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_Packing).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_Color).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_Color).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_Color).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_Brand).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_Brand).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_Brand).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_Model).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_Model).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_Model).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_SupInvNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_SupInvNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_SupInvNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_QuoteNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_QuoteNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_QuoteNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_SourceType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_SourceType).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_SourceType).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_SourceLine).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_SourceLine).Value

                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_DestType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_DestType).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_DestType).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_UOM).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_UOM).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_UOM).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_PCMSDocNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_PCMSDocNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_PCMSDocNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_BillNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_BillNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_BillNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_SecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_SecNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_SecNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_SubSecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_SubSecNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_SubSecNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_PageNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_PageNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_PageNum).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_Quantity).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_Quantity).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_Price).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_Price).Value

                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_ItemType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_ItemType).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_ItemType).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_MCBillNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_MCBillNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_MCBillNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_MCSecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_MCSecNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_MCSecNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_MCSubSecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_MCSubSecNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_MCSubSecNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_MCPageNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_MCPageNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_MCPageNum).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_PriceType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_PriceType).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_PriceType).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_AppMethod).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_AppMethod).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_AppMethod).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_LineType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_LineType).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_LineType).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_MCLineNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_MCLineNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_MCLineNum).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_OpenPrcnt).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_OpenPrcnt).Value

                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_ContraFlag).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_ContraFlag).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_ContraFlag).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_RecoverFlag).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_RecoverFlag).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_RecoverFlag).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_RecoverStatus).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_RecoverStatus).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_RecoverStatus).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_SubLineNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_SubLineNum).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_MCSubLineNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_MCSubLineNum).Value

                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_ClientRef).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_ClientRef).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_ClientRef).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_SourceEntry).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_SourceEntry).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_DestEntry).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_DestEntry).Value

                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_IncomeCode).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_IncomeCode).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_IncomeCode).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_IPCode).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_IPCode).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_IPCode).Value
                        End If
                        If .Item(Datatable.SAP.AP.SubCon_Dtl._U_BillLineNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_BillLineNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_BillLineNum).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_BillSubLineNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_BillSubLineNum).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_ItemNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_ItemNum).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_RefNum).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_RefNum).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_FullDesc).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_FullDesc).Value

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_Budget).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_Budget).Value
                        'Karrson: 2011-02-14: RefCardCode,RefCardName
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AP.SubCon_Dtl._U_RefCardCode).Value = .Item(Datatable.SAP.AP.SubCon_Dtl._U_RefCardCode).Value
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
                        'oDocumentExpenses.ExpenseCode = .Item(Datatable.SAP.AP.SubCon_Frg.FreightCode).Value
                        'If LocalCurrency = oDocuments.DocCurrency Then
                        oDocumentExpenses.LineTotal = .Item(Datatable.SAP.AP.SubCon_Frg.LC_Total).Value
                        'Else
                        'oDocumentExpenses.LineTotal = .Item(Datatable.SAP.AP.SubCon_Frg.FC_Total).Value
                        'End If
                        oRecSet.MoveNext()
                    Loop
                End If
            End With

            If oDocuments.Add = 0 Then
                Dim NewObjKey As Integer

                NewObjKey = Company.GetNewObjectKey
                MyBase.ToMapping(NewObjKey, 18, oDocEntry)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
                Return Company.GetNewObjectKey
            Else
                oLogMessage.AddFailureLine(oBatchID, Company.GetLastErrorDescription)
                ToErrorTable(oDocEntry, _
                             18, _
                             Company.GetLastErrorCode, _
                             "Unable to Create A/P Service Invoice", _
                             Company.GetLastErrorDescription)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
                Return 0
            End If

        End Function
        Function DeleteDocument(ByVal oBatchID As String) As Integer
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
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
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
                Return Company.GetNewObjectKey
            Else
                oLogMessage.AddFailureLine(oBatchID, Company.GetLastErrorDescription)
                ToErrorTable(oDraftKey, _
                             18, _
                             Company.GetLastErrorCode, _
                             "Unable to Create A/P Service Invoice", _
                             Company.GetLastErrorDescription)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
                Return 0
            End If
            WriteDebug(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " End")
        End Function
        Function CloseDraft(ByVal pBatchID As String) As Integer
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oDocEntry As Integer
            Dim oDraft As New Datatable.SAP.AP.SubCon_Hdr

            oDocEntry = Right(pBatchID, pBatchID.Length - 1)

            oDraft.getPurchaseInvoice(oDocEntry)
            oDraft.DocStatus = "C"
            oDraft.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Function
        Function DeleteDraft(ByVal pBatchID As String) As Integer
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oDocEntry As Integer
            Dim oDraft As New Datatable.SAP.AP.SubCon_Hdr

            oDocEntry = Right(pBatchID, pBatchID.Length - 1)

            oDraft.getPurchaseInvoice(oDocEntry)
            'oDraft.DocStatus = "D"
            oDraft.DocStatus = "C"
            oDraft.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Function
        Function UpdateFlex(ByVal pBatchID As String) As Integer
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oPIVOU As Datatable.Flex.PIVOU

            oPIVOU = New Datatable.Flex.PIVOU
            oPIVOU.getItemAP(pBatchID)
            'oPIVOU.PCMS_Remark = "Operation is Success"
            oPIVOU.PCMS_Status = "C"
            oPIVOU.PCMS_UpdateDate = Now

            oFlexConnection.Process(oPIVOU.UpdateQuery & " " & oPIVOU.filterQuery)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
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
