Imports SAPbobsCOM
Imports CPS.CPSLIB.Debug
Namespace SyncMainClass
    ''' <summary>
    ''' Client Payment Certificate Data, Inherits Snchronization Class
    ''' </summary>
    ''' <remarks></remarks>
    Class S_Invoice
        Inherits [Interface].Synchronization

        Private ReadOnly mJobReference As String = "A/R Service Invoice Synchronization (Fiex & SAP)"
        Private LocalCurrency As String

#Region "Constructor"
        Public Sub New(ByVal pCompany As SAPbobsCOM.Company)
            MyBase.New(pCompany)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            MyBase.JobName = mJobReference
            ObjType = 13
            oLogMessage.FileName = "AR-SrvInvoice"

            Dim sboBob As SBObob = pCompany.GetBusinessObject(BoObjectTypes.BoBridge)
            LocalCurrency = ""
            LocalCurrency = Trim(sboBob.GetLocalCurrency.Fields.Item(0).Value)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Sub
#End Region

#Region "Data Mapping"
        'Line Level Export to PCMS structure 
        Function result(ByVal pFields As SAPbobsCOM.Fields, ByVal pField_Dtls As SAPbobsCOM.Fields) As ExportPCMS
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oReturnResult As ExportPCMS
            Dim oDocEntry As Integer
            Dim oDocCurrency As String

            oReturnResult = New ExportPCMS
            With pFields
                'Get the document entry form the record set result
                WriteDebug("AR Invoice: Fields: " & Datatable.SAP.AR.Client_Hdr.DocEntry)
                oDocEntry = .Item(Datatable.SAP.AR.Client_Hdr.DocEntry).Value
                WriteDebug("AR Invoice: Fields: " & Datatable.SAP.AR.Client_Hdr.DocCur)
                oDocCurrency = Trim(.Item(Datatable.SAP.AR.Client_Hdr.DocCur).Value)

                oReturnResult.RefNo = oDocEntry
                WriteDebug("AR Invoice: Fields: " & Datatable.SAP.AR.Client_Hdr.SubsiCode)
                oReturnResult.SubsidiaryCode = .Item(Datatable.SAP.AR.Client_Hdr.SubsiCode).Value

                'Modify by Michael, begin, 20140115
                'Supplier Payment Certificate Data
                If .Item(Datatable.SAP.AR.Client_Hdr._U_RevFlag).Value = "R" Then
                    oReturnResult.CertEntry = "D" & RSet(.Item(Datatable.SAP.AR.Client_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                Else
                    oReturnResult.CertEntry = "C" & RSet(.Item(Datatable.SAP.AR.Client_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                End If

                'Modify by Michael, end, 20140115
                'oReturnResult.VoucherType  (GJ)
                'Karrson: Change
                'oReturnResult.ValuationDate = .Item(Datatable.SAP.AR.Client_Hdr.DocDate).Value
                WriteDebug("AR Invoice: Fields: " & Datatable.SAP.AR.Client_Hdr.DocumentDate)
                oReturnResult.ValuationDate = .Item(Datatable.SAP.AR.Client_Hdr.DocumentDate).Value

                'oReturnResult.Description  (General Journal)
                WriteDebug("AR Invoice: Fields: " & Datatable.SAP.AR.Client_Dtl.AcctCode)
                ' Karrson Change: Map to OACT
                'oReturnResult.AcctCode = pField_Dtls.Item(Datatable.SAP.AR.Client_Dtl.AcctCode).Value
                oReturnResult.AcctCode = pField_Dtls.Item(Datatable.SAP.AR.Client_Dtl.AcctCode).Value
                WriteDebug("AR Invoice: Fields: " & Datatable.SAP.AR.Client_Hdr.CardCode)
                oReturnResult.BPCode = .Item(Datatable.SAP.AR.Client_Hdr.CardCode).Value
                WriteDebug("AR Invoice: Fields: " & Datatable.SAP.AR.Client_Dtl.PrjCode)
                oReturnResult.ProjectCode = pField_Dtls.Item(Datatable.SAP.AR.Client_Dtl.PrjCode).Value
                oReturnResult.CostCode = Nothing
                WriteDebug("AR Invoice: Fields: " & Datatable.SAP.AR.Client_Hdr.PCMSDocNum)

                'Old
                'oReturnResult.CertNumber = .Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value
                'oReturnResult.RevNo = .Item(Datatable.SAP.AR.Client_Hdr.RevNo).Value
                WriteDebug("Cert Number: " & .Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value)
                WriteDebug("Cert Number Length: " & Len(Trim(.Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value)).ToString())

                If Len(Trim(.Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value)) = 20 Or Len(Trim(.Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value)) = 21 Then
                    oReturnResult.CertNumber = .Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value
                    oReturnResult.RevNo = ""
                    WriteDebug("Lenght in 20,21")
                Else
                    oReturnResult.CertNumber = .Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value & "_" & .Item(Datatable.SAP.AR.Client_Hdr.RevNo).Value
                    oReturnResult.RevNo = .Item(Datatable.SAP.AR.Client_Hdr.RevNo).Value
                    WriteDebug("Lenght not in 20,21")
                End If

                oReturnResult.VInvoiceNo = Nothing
                'oReturnResult.DocType
                WriteDebug("AR Invoice: Fields: " & Datatable.SAP.AR.Client_Hdr.DocumentDate)
                oReturnResult.CertDate = .Item(Datatable.SAP.AR.Client_Hdr.DocumentDate).Value
                WriteDebug("AR Invoice: Fields: " & Datatable.SAP.AR.Client_Hdr.DocDueDate)
                oReturnResult.CertDueDate = .Item(Datatable.SAP.AR.Client_Hdr.DocDueDate).Value
                oReturnResult.DocCurrency = oDocCurrency
                oReturnResult.SubCon_No = Nothing
                WriteDebug("AR Invoice: Fields: " & Datatable.SAP.AR.Client_Dtl._U_RefCardCode)
                oReturnResult.RefCardCode = pField_Dtls.Item(Datatable.SAP.AR.Client_Dtl._U_RefCardCode).Value

                'Calculate the Total Before Discount 
                Dim oDocTotal As Double, oDisPercent As Double
                'If Me.LocalCurrency.ToUpper = oDocCurrency.ToUpper Then
                oReturnResult.DocRate = 1
                WriteDebug("AR Invoice: Fields: " & Datatable.SAP.AR.Client_Dtl.LC_Total)
                oDocTotal = pField_Dtls.Item(Datatable.SAP.AR.Client_Dtl.LC_Total).Value
                'Else
                WriteDebug("AR Invoice: Fields: " & Datatable.SAP.AR.Client_Hdr.DocRate)
                oReturnResult.DocRate = pFields.Item(Datatable.SAP.AR.Client_Hdr.DocRate).Value
                'WriteDebug("AR Invoice: Fields: " & Datatable.SAP.AR.Client_Dtl.FC_Total)
                'oDocTotal = pField_Dtls.Item(Datatable.SAP.AR.Client_Dtl.FC_Total).Value
                'End If
                WriteDebug("AR Invoice: Fields: " & Datatable.SAP.AR.Client_Dtl.DisPercent)
                oDisPercent = .Item(Datatable.SAP.AR.Client_Dtl.DisPercent).Value
                WriteDebug("DocTotal: " & oDocTotal)
                WriteDebug("oDisPercent: " & oDisPercent)

                oReturnResult.TotalBefDis = oDocTotal / (1 + oDisPercent / 100)
                WriteDebug("TotalBefDis: " & oReturnResult.TotalBefDis)
                oReturnResult.DocTotal = oDocTotal
                'oReturnResult.Allocation
                oReturnResult.CertData = oReturnResult.BPCode & " " & oReturnResult.CertNumber
            End With
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            Return oReturnResult
        End Function

        'Customer Level Export to PCMS structure
        Function DebtorResult(ByVal pFields As SAPbobsCOM.Fields) As ExportPCMS
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oReturnResult As ExportPCMS
            Dim oDocEntry As Integer
            Dim oDocCurrency As String

            oReturnResult = New ExportPCMS
            With pFields
                'Get the document entry form the record set result
                oDocEntry = .Item(Datatable.SAP.AR.Client_Hdr.DocEntry).Value
                oDocCurrency = Trim(.Item(Datatable.SAP.AR.Client_Hdr.DocCur).Value)

                oReturnResult.RefNo = oDocEntry
                oReturnResult.SubsidiaryCode = .Item(Datatable.SAP.AR.Client_Hdr.SubsiCode).Value
                'Modify by Michael, begin, 20140115
                'Supplier Payment Certificate Data
                If .Item(Datatable.SAP.AR.Client_Hdr._U_RevFlag).Value = "R" Then
                    oReturnResult.CertEntry = "D" & RSet(.Item(Datatable.SAP.AR.Client_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                Else
                    oReturnResult.CertEntry = "C" & RSet(.Item(Datatable.SAP.AR.Client_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                End If
                'Modify by Michael, end, 20140115
                'oReturnResult.VoucherType  (GJ)
                'Karrson: change
                'oReturnResult.ValuationDate = .Item(Datatable.SAP.AR.Client_Hdr.DocDate).Value
                oReturnResult.ValuationDate = .Item(Datatable.SAP.AR.Client_Hdr.DocumentDate).Value

                'oReturnResult.Description  (General Journal)
                'oReturnResult.AcctCode = ExAcctcode(ControlAcct(.Item(Datatable.SAP.AR.Client_Hdr.CardCode).Value), .Item(Datatable.SAP.AR.Client_Hdr.CardCode).Value)
                oReturnResult.AcctCode = Datatable.SAP.ControlAccount.AC_Trade_AR & _
                                         Left(.Item(Datatable.SAP.AR.Client_Hdr.CardCode).Value, 5)
                oReturnResult.BPCode = .Item(Datatable.SAP.AR.Client_Hdr.CardCode).Value
                oReturnResult.ProjectCode = .Item(Datatable.SAP.AR.Client_Hdr.Project).Value
                oReturnResult.CostCode = ControlAcct(.Item(Datatable.SAP.AR.Client_Hdr.CardCode).Value)

                'Testing
                'oReturnResult.CertNumber = .Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value
                'oReturnResult.RevNo = .Item(Datatable.SAP.AR.Client_Hdr.RevNo).Value
                If Len(Trim(.Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value)) = 20 Or Len(Trim(.Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value)) = 21 Then
                    oReturnResult.CertNumber = .Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value
                    oReturnResult.RevNo = ""
                Else
                    oReturnResult.CertNumber = .Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value & "_" & .Item(Datatable.SAP.AR.Client_Hdr.RevNo).Value
                    oReturnResult.RevNo = .Item(Datatable.SAP.AR.Client_Hdr.RevNo).Value
                End If

                oReturnResult.VInvoiceNo = Nothing
                'oReturnResult.DocType
                oReturnResult.CertDate = .Item(Datatable.SAP.AR.Client_Hdr.DocumentDate).Value
                oReturnResult.CertDueDate = .Item(Datatable.SAP.AR.Client_Hdr.DocDueDate).Value
                oReturnResult.DocCurrency = oDocCurrency

                'Calculate the Total Before Discount 
                Dim oDocTotal As Double, oDisPercent As Double
                WriteDebug("DebtorResult Local Currency: " & Me.LocalCurrency)
                WriteDebug("DebtorResult Doc Currency: " & oDocCurrency)
                WriteDebug("DebtorResult DocTotal: " & .Item(Datatable.SAP.AR.Client_Hdr.DocTotal).Value)
                WriteDebug("DebtorResult DocTotalFC: " & .Item(Datatable.SAP.AR.Client_Hdr.DocTotalFC).Value)
                WriteDebug("DebtorResult DisPercent: " & .Item(Datatable.SAP.AR.Client_Hdr.DisPercent).Value)
                WriteDebug("DebtorResult DocRate: " & .Item(Datatable.SAP.AR.Client_Hdr.DocRate).Value)

                'If Me.LocalCurrency.ToUpper = oDocCurrency.ToUpper Then
                oDocTotal = .Item(Datatable.SAP.AR.Client_Hdr.DocTotal).Value
                'Else
                'oDocTotal = .Item(Datatable.SAP.AR.Client_Hdr.DocTotalFC).Value
                'End If
                oDisPercent = .Item(Datatable.SAP.AR.Client_Hdr.DisPercent).Value

                oReturnResult.TotalBefDis = oDocTotal / (1 + oDisPercent / 100)
                oReturnResult.DocRate = .Item(Datatable.SAP.AR.Client_Hdr.DocRate).Value
                oReturnResult.DocTotal = oDocTotal
                'oReturnResult.Allocation
                oReturnResult.CertData = oReturnResult.BPCode.Trim & " " & oReturnResult.CertNumber.Trim
            End With
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            Return oReturnResult
        End Function

        'Freight Level Export to PCMS structure
        Function FreightResult(ByVal pField_Hdr As Fields) As ExportPCMS()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oDocEntry As Integer
            Dim oDocCurrency As String
            Dim Srv_PCH6 As Datatable.SAP.AR.Client_Frg
            Dim oRecSet As SAPbobsCOM.Recordset
            Dim oExportPCMSs(), oExportPCMS As ExportPCMS

            oDocEntry = pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.DocEntry).Value
            oDocCurrency = pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.DocCur).Value

            Srv_PCH6 = New Datatable.SAP.AR.Client_Frg
            Srv_PCH6.getFreightEntry(oDocEntry)
            oExportPCMSs = Nothing

            oRecSet = Srv_PCH6.Execute
            Do Until oRecSet.EoF
                oExportPCMS = New ExportPCMS

                With oExportPCMS
                    .RefNo = oDocEntry
                    .SubsidiaryCode = pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.SubsiCode).Value
                    'Modify by Michael, begin, 20140115
                    'Supplier Payment Certificate Data
                    If pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.SubsiCode).Value = "R" Then
                        .CertEntry = "D" & RSet(pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                    Else
                        .CertEntry = "C" & RSet(pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                    End If
                    'Modify by Michael, end, 20140115
                    'oReturnResult.VoucherType  (GJ)
                    'Karrson: Change
                    '.ValuationDate = pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.DocDate).Value
                    .ValuationDate = pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.DocumentDate).Value

                    'oReturnResult.Description  (General Journal)
                    .AcctCode = Srv_PCH6.getAcctCode(oRecSet.Fields.Item(Datatable.SAP.AR.Client_Frg.FreightCode).Value)
                    .BPCode = pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.CardCode).Value
                    .ProjectCode = pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.Project).Value
                    .CostCode = Srv_PCH6.getAcctCode(oRecSet.Fields.Item(Datatable.SAP.AR.Client_Frg.FreightCode).Value)

                    'Testing 
                    '.CertNumber = pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value
                    '.RevNo = pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.RevNo).Value

                    If Len(Trim(pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value)) = 20 Then
                        .CertNumber = pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value
                        .RevNo = ""
                    Else
                        .CertNumber = pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value & "_" & pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.RevNo).Value
                        .RevNo = pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.RevNo).Value
                    End If

                    .VInvoiceNo = Nothing
                    'oReturnResult.DocType
                    .CertDate = pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.DocumentDate).Value
                    .CertDueDate = pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.DocDueDate).Value
                    .DocCurrency = oDocCurrency

                    'Calculate the Total Before Discount 
                    Dim oDocTotal As Double
                    'If Me.LocalCurrency.ToUpper = oDocCurrency.ToUpper Then
                    oDocTotal = oRecSet.Fields.Item(Datatable.SAP.AR.Client_Frg.LC_Total).Value
                    'Else
                    'oDocTotal = oRecSet.Fields.Item(Datatable.SAP.AR.Client_Frg.FC_Total).Value
                    'End If
                    .TotalBefDis = oDocTotal

                    .DocRate = pField_Hdr.Item(Datatable.SAP.AR.Client_Hdr.DocRate).Value
                    .DocTotal = oDocTotal

                    'oReturnResult.Allocation
                    .CertData = .BPCode.Trim & " " & .CertNumber.Trim
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

#Region "PCMS & FLEX - Transaction Mapping"
        ''' <summary>
        ''' Import data from PCMS to Flex structure
        ''' </summary>
        ''' <param name="pExportPCMS">Collection data from PCMS Structure</param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        Function fDataMapping(ByVal pExportPCMS As ExportPCMS) As ImportFLEX
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oImport As New ImportFLEX

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
                oImport.AcctCode = ExAcctcode(.AcctCode, pExportPCMS).Trim
                oImport.AnalysisCode1 = Left(.BPCode.Trim, 8)
                oImport.AnalysisCode2 = Nothing
                oImport.AnalysisCode3 = .ProjectCode
                oImport.AnalysisCode4 = Right(.AcctCode, 5) & "000"
                oImport.AnalysisCode5 = [Interface].Synchronization.GetAnalysisCode5(.AcctCode)
                oImport.DocumentNo = .CertNumber
                oImport.AltDocNumber = Nothing
                oImport.DocType = ExportPCMS.DocType
                oImport.DocDate = .ValuationDate
                oImport.DocDueDate = Nothing
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

                oImport.RevNo = .RevNo
            End With
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            Return oImport
        End Function

        Function fDebitor(ByVal pExportPCMS As ExportPCMS) As ImportFLEX
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
                WriteDebug("fDebotr Cert Number" & .CertNumber)
                oImport.DocumentNo = .CertNumber
                oImport.AltDocNumber = Nothing
                oImport.DocType = ExportPCMS.DocType
                oImport.DocDate = .ValuationDate
                oImport.DocDueDate = Nothing
                oImport.Currency = .DocCurrency

                oImport.Allocation = "D"
                oImport.Amount = .TotalBefDis
                WriteDebug("fDebitor.TotalBefDis: " & oImport.Amount)
                oImport.EquvAmount = .DocTotal
                WriteDebug("fDebitor.EquvAmount: " & oImport.EquvAmount)
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

                oImport.RevNo = .RevNo
            End With
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            Return oImport
        End Function

#End Region

        Public Overrides Sub Export()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            'Declare Datatable 
            Dim oPTVOU As New Datatable.Flex.PTVOU
            Dim Itm_OINV As Datatable.SAP.AR.Client_Hdr
            Dim Itm_INV1 As New Datatable.SAP.AR.Client_DtlExp
            Dim oSynchHistory As New Datatable.SAP.Sync_History

            'Document Entry
            Dim oDocEntry As Integer = 0
            Dim oDocCurrency As String

            'Data Structure
            Dim oExport As ExportPCMS, oExports As ExportPCMS()
            Dim oFrgExports As ExportPCMS()
            Dim oImport As ImportFLEX

            Dim oRecSet As SQL.RecordSet

            Dim LineNumber As Integer

            'Common Object
            Dim recset_Hdr As SAPbobsCOM.Recordset, recset_Dtl As SAPbobsCOM.Recordset

            oLogMessage.AddReferenceLine("EXPORT", "Export Data To Flex Environment.")

            Try
                oRecSet = New SQL.RecordSet

                Itm_OINV = New Datatable.SAP.AR.Client_Hdr
                recset_Hdr = Itm_OINV.Execute
                WriteDebug("Client Payment Cert Header: " & Itm_OINV.SelectQuery & " " & Itm_OINV.filterQuery)
                Do Until recset_Hdr.EoF
                    Try
                        oDocEntry = 0

                        oDocEntry = recset_Hdr.Fields.Item(Datatable.SAP.AR.Client_Hdr.DocEntry).Value
                        oDocCurrency = Trim(recset_Hdr.Fields.Item(Datatable.SAP.AR.Client_Hdr.DocCur).Value)

                        If Chk_ErrOverLap(oDocEntry, eObjType.ot_SalesInvoice) Then
                            GoTo Move_Next
                        End If

                        'Begin transaction
                        Me.StartTransaction()

                        'Get Document Detail by Document entry
                        Itm_INV1 = New Datatable.SAP.AR.Client_DtlExp
                        Itm_INV1.getDocumentLine(oDocEntry)
                        recset_Dtl = Itm_INV1.Execute
                        WriteDebug("Payment Cert Export Query: " & Itm_INV1.SelectQuery & " " & Itm_INV1.filterQuery & " ")
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
                        'LineNumber = -998
                        'Import Data into Flex Table
                        If Not oExports Is Nothing Then
                            For Each oExport In oExports
                                oExport.LineNo = LineNumber
                                LineNumber += 1
                                oImport = fDataMapping(oExport)
                                WriteDebug("Debtor Document Number: " & oImport.DocumentNo)

                                Me.ToFlex(oImport)
                            Next
                        End If

                        'Draw data into Credit Side
                        oExport = DebtorResult(recset_Hdr.Fields)
                        oExport.LineNo = 1
                        'oExport.LineNo = 1 - 1000
                        'Karrson: Debug Only

                        oImport = fDebitor(oExport)


                        'Import Data into Datatable PTVOU

                        Me.ToFlex(oImport)

                        'Karrson: End Debug

                        'Karrson change
                        'oSynchHistory.Add_Submitted("C" & RSet(oDocEntry.ToString.Trim, 9).Replace(" ", "0"))
                        oSynchHistory.Add_Acknowledge("C" & RSet(oDocEntry.ToString.Trim, 9).Replace(" ", "0"))

                        oLogMessage.AddSuccessLine("[EXPORT]AR " & oDocEntry, "Operation Success")

                        Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)
                    Catch b_ex As BaseException
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip("[EXPORT]AR " & oDocEntry, b_ex.toString)

                        If Not oDocEntry = 0 Then
                            ToErrorTable(oDocEntry, _
                                         13, _
                                         -9999, _
                                         "[EXPORT]AR " & oDocEntry, _
                                         b_ex.toString)
                        End If
                    Catch ex As Exception
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip("[EXPORT]AR " & oDocEntry, ex.ToString)

                        If Not oDocEntry = 0 Then
                            ToErrorTable(oDocEntry, _
                                         13, _
                                         -9999, _
                                         "[EXPORT]AR " & oDocEntry, _
                                         ex.ToString)
                        End If
                    End Try

Move_Next:
                    recset_Hdr.MoveNext()
                Loop
            Catch b_ex As BaseException
                Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                oLogMessage.AddExceptionSkip("[EXPORT]AR " & oDocEntry, b_ex.toString)

                If Not oDocEntry = 0 Then
                    ToErrorTable(oDocEntry, _
                                 13, _
                                 -9999, _
                                 "[EXPORT]AR " & oDocEntry, _
                                 b_ex.toString)
                End If
            Catch ex As Exception
                Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                oLogMessage.AddExceptionSkip("[EXPORT]AR " & oDocEntry, ex.ToString)

                If Not oDocEntry = 0 Then
                    ToErrorTable(oDocEntry, _
                                 13, _
                                 -9999, _
                                 "[EXPORT]AR " & oDocEntry, _
                                 ex.ToString)
                End If
            End Try
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            oLogMessage.AddReferenceLine("EXPORT", "--------------------------------")
        End Sub

        Public Overrides Sub Import()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Try
                Me.Import_Posted()
                'Add by Michael, 20140115, begin
                Me.Import_Posted_Reversed()
                'Add by Michael, 20140115, end
                'Me.Import_Exception()
                'Me.Import_Reject()
                'Me.Import_Delete()
            Catch ex As Exception
                oLogMessage.AddExceptionSkip("MAIN", ex.ToString)
            End Try
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Sub

        'Process Success Data with Accept
        Public Sub Import_Posted()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oPIVOU As Datatable.Flex.PIVOU
            Dim oDataTable As System.Data.DataTable
            Dim BatchID As String = ""
            Dim sqlStr As String
            Dim SyncHistory As Datatable.SAP.Sync_History
            Dim oNewObjKey As Integer

            oLogMessage.AddReferenceLine("POSTED", "Create New A/R Service Invoice in SAP System.")
            oPIVOU = New Datatable.Flex.PIVOU

            oPIVOU.getSrvAR(Datatable.Flex.PIVOU.flexStatus.fs_Accepted)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery
            oDataTable = oFlexConnection.DataTable(sqlStr)
            WriteDebug("Payment Cert Import Posted: " & sqlStr)
            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim
                        If MyBase.Chk_ErrOverLap(BatchID, eObjType.ot_SalesInvoice) Then
                            GoTo MoveNext
                        End If

                        'Add by Michael, begin
                        Dim oRecSet As SAPbobsCOM.Recordset
                        Dim cRevTrans As String
                        Dim cComCode As String = ""
                        Dim cPIVOU_REF_NUM As String = ""
                        Dim nDocEntry As Long = 0

                        Dim _dtBatch As System.Data.DataTable
                        _dtBatch = oFlexConnection.DataTable(oPIVOU.BatchSql(BatchID))
                        For Each ooDataRow As System.Data.DataRow In _dtBatch.Rows
                            cComCode = ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_COM_CDE)
                            cPIVOU_REF_NUM = ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_REF_NUM)

                            'Added by Ken, 20181106
                            If cPIVOU_REF_NUM.Contains("_") Then
                                cPIVOU_REF_NUM = cPIVOU_REF_NUM.Substring(0, cPIVOU_REF_NUM.LastIndexOf("_"))
                            End If

                            If Len(Trim(cPIVOU_REF_NUM)) > 12 Then    'Other Income Payment Cert
                                cPIVOU_REF_NUM = Left(Trim(cPIVOU_REF_NUM), 17) & "/" & Right(Trim(cPIVOU_REF_NUM), 3)
                                'cPIVOU_REF_NUM = Left(Trim(cPIVOU_REF_NUM), 11) + "/" + Right(Trim(cPIVOU_REF_NUM), 3)
                            End If
                        Next

                        sqlStr = "select ISNULL(DocEntry, 0) as DOCENTRY, ISNULL(Rev_DocEntry,0) as REV_DOCENTRY from PCMS_FE.PCMS800.dbo.DocumentProperty where DocNum = '" & cPIVOU_REF_NUM.Trim & "' and DocStatus = 'PPFA'"
                        'sqlStr = "select ISNULL(DocEntry, 0) as DOCENTRY, ISNULL(Rev_DocEntry,0) as REV_DOCENTRY from PCMS_FE.MCPCMS100.dbo.DocumentProperty where DocNum = '" & cPIVOU_REF_NUM.Trim & "' and DocStatus = 'PPFA'"
                        WriteDebug("Test1 : " & sqlStr)

                        oRecSet = commonRecordSet.execute(sqlStr)

                        WriteDebug("Test2 : " & String.Format(oRecSet.RecordCount).Trim)
                        If Not (oRecSet.RecordCount = 0) Then
                            WriteDebug("Test3 : " & sqlStr)
                            If oRecSet.Fields.Item("REV_DOCENTRY").Value = 0 Then
                                cRevTrans = "N"
                                nDocEntry = oRecSet.Fields.Item("DOCENTRY").Value
                            Else
                                cRevTrans = "Y"
                                nDocEntry = oRecSet.Fields.Item("REV_DOCENTRY").Value
                            End If
                            WriteDebug("Test4 : " & sqlStr)
                            'Add by Michael, end

                            'Start Transaction
                            Me.StartTransaction()

                            WriteDebug("Entry : " & String.Format(nDocEntry).Trim & "   " & cRevTrans)

                            'Draw Data into CPSFIN for Backup
                            Me.INSERT_CPSFIN(BatchID, cRevTrans, nDocEntry)

                            WriteDebug("Insert CPSFIN End")

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
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, b_ex.toString)

                        ToErrorTable(BatchID, _
                                     13, _
                                     -9999, _
                                     "[IMPORT]AR " & BatchID, _
                                     b_ex.toString)
                    Catch ex As Exception
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, ex.ToString)

                        ToErrorTable(BatchID, _
                                     13, _
                                     -9999, _
                                     "[IMPORT]AR " & BatchID, _
                                     ex.ToString)
                    End Try

MoveNext:
                Next
            End If
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            oLogMessage.AddReferenceLine("POSTED", "-----------------------------------------")
        End Sub

        'Process Return Data with Exception
        Public Sub Import_Exception()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oPIVOU As Datatable.Flex.PIVOU
            Dim oDataTable As System.Data.DataTable
            Dim BatchID As String = ""
            Dim sqlStr As String
            Dim SyncHistory As Datatable.SAP.Sync_History

            oLogMessage.AddReferenceLine("ERROR", "Error the document draft from backend table in SAP.")
            oPIVOU = New Datatable.Flex.PIVOU
            'Karrson: Fix Bug
            'oPIVOU.getSrvAP(Datatable.Flex.PIVOU.flexStatus.fs_Error)
            oPIVOU.getSrvAR(Datatable.Flex.PIVOU.flexStatus.fs_Error)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery
            oDataTable = oFlexConnection.DataTable(sqlStr)

            WriteDebug("Client Payment Cert Query Import Exception: " & sqlStr)
            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim

                        If MyBase.Chk_ErrOverLap(BatchID, eObjType.ot_SalesInvoice) Then
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
                                     13, _
                                     -9999, _
                                     "[IMPORT]AR " & BatchID, _
                                     b_ex.toString)
                    Catch ex As Exception
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, ex.ToString)

                        ToErrorTable(BatchID, _
                                     13, _
                                     -9999, _
                                     "[IMPORT]AR " & BatchID, _
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

            oPIVOU.getSrvAR(Datatable.Flex.PIVOU.flexStatus.fs_Rejected)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery
            oDataTable = oFlexConnection.DataTable(sqlStr)

            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim

                        If Chk_ErrOverLap(BatchID, eObjType.ot_SalesInvoice) Then
                            GoTo MoveNext
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
                                     13, _
                                     -9999, _
                                     "[IMPORT]AR " & BatchID, _
                                     b_ex.toString)
                    Catch ex As Exception
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, ex.ToString)

                        ToErrorTable(BatchID, _
                                     13, _
                                     -9999, _
                                     "[IMPORT]AR " & BatchID, _
                                     ex.ToString)
                    End Try

MoveNext:
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

            oPIVOU.getSrvAR(Datatable.Flex.PIVOU.flexStatus.fs_Delete)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery
            oDataTable = oFlexConnection.DataTable(sqlStr)

            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim

                        If Chk_ErrOverLap(BatchID, eObjType.ot_SalesInvoice) Then
                            GoTo MoveNext
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
                                     13, _
                                     -9999, _
                                     "[IMPORT]AR " & BatchID, _
                                     b_ex.toString)
                    Catch ex As Exception
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, ex.ToString)

                        ToErrorTable(BatchID, _
                                     13, _
                                     -9999, _
                                     "[IMPORT]AR " & BatchID, _
                                     ex.ToString)
                    End Try

MoveNext:
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

            oLogMessage.AddReferenceLine("POSTED", "Create New A/R Service Invoice in SAP System.")
            oPIVOU = New Datatable.Flex.PIVOU

            oPIVOU.getSrvAR_Reversed(Datatable.Flex.PIVOU.flexStatus.fs_Accepted)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery
            oDataTable = oFlexConnection.DataTable(sqlStr)
            WriteDebug("Payment Cert Import Posted: " & sqlStr)
            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim
                        If MyBase.Chk_ErrOverLap(BatchID, eObjType.ot_SalesInvoice) Then
                            GoTo MoveNext
                        End If

                        'Add by Michael, begin
                        Dim oRecSet As SAPbobsCOM.Recordset
                        Dim cRevTrans As String
                        Dim cComCode As String = ""
                        Dim cPIVOU_REF_NUM As String = ""
                        Dim nDocEntry As Long = 0

                        Dim _dtBatch As System.Data.DataTable
                        _dtBatch = oFlexConnection.DataTable(oPIVOU.BatchSql(BatchID))
                        For Each ooDataRow As System.Data.DataRow In _dtBatch.Rows
                            cComCode = ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_COM_CDE)
                            cPIVOU_REF_NUM = ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_REF_NUM)

                            'Added by Ken, 20181106
                            If cPIVOU_REF_NUM.Contains("_") Then
                                cPIVOU_REF_NUM = cPIVOU_REF_NUM.Substring(0, cPIVOU_REF_NUM.LastIndexOf("_"))
                            End If

                            If Len(Trim(cPIVOU_REF_NUM)) > 12 Then    'Other Income Payment Cert
                                cPIVOU_REF_NUM = Left(Trim(cPIVOU_REF_NUM), 17) & "/" & Right(Trim(cPIVOU_REF_NUM), 3)
                                'cPIVOU_REF_NUM = Left(Trim(cPIVOU_REF_NUM), 11) + "/" + Right(Trim(cPIVOU_REF_NUM), 3)
                            End If
                        Next

                        sqlStr = "select ISNULL(DocEntry, 0) as DOCENTRY, ISNULL(Rev_DocEntry,0) as REV_DOCENTRY from PCMS_FE.PCMS800.dbo.DocumentProperty where DocNum = '" & cPIVOU_REF_NUM.Trim & "' and DocStatus = 'PPFA'"
                        'sqlStr = "select ISNULL(DocEntry, 0) as DOCENTRY, ISNULL(Rev_DocEntry,0) as REV_DOCENTRY from PCMS_FE.MCPCMS100.dbo.DocumentProperty where DocNum = '" & cPIVOU_REF_NUM.Trim & "' and DocStatus = 'PPFA'"
                        WriteDebug("Test1 : " & sqlStr)

                        oRecSet = commonRecordSet.execute(sqlStr)

                        WriteDebug("Test2 : " & String.Format(oRecSet.RecordCount).Trim)
                        If Not (oRecSet.RecordCount = 0) Then
                            WriteDebug("Test3 : " & sqlStr)
                            If oRecSet.Fields.Item("REV_DOCENTRY").Value = 0 Then
                                cRevTrans = "N"
                                nDocEntry = oRecSet.Fields.Item("DOCENTRY").Value
                            Else
                                cRevTrans = "Y"
                                nDocEntry = oRecSet.Fields.Item("REV_DOCENTRY").Value
                            End If
                            WriteDebug("Test4 : " & sqlStr)
                            'Add by Michael, end

                            'Start Transaction
                            Me.StartTransaction()

                            WriteDebug("Entry : " & String.Format(nDocEntry).Trim & "   " & cRevTrans)

                            'Draw Data into CPSFIN for Backup
                            Me.INSERT_CPSFIN(BatchID, cRevTrans, nDocEntry)

                            WriteDebug("Insert CPSFIN End")

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
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, b_ex.toString)

                        ToErrorTable(BatchID, _
                                     13, _
                                     -9999, _
                                     "[IMPORT]AR " & BatchID, _
                                     b_ex.toString)
                    Catch ex As Exception
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(BatchID, ex.ToString)

                        ToErrorTable(BatchID, _
                                     13, _
                                     -9999, _
                                     "[IMPORT]AR " & BatchID, _
                                     ex.ToString)
                    End Try

MoveNext:
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
            Dim oDraft As Datatable.SAP.AR.Client_Hdr
            Dim oDraft_Dtls As Datatable.SAP.AR.Client_Dtl
            Dim oDraft_Frgs As Datatable.SAP.AR.Client_Frg

            Dim oDocEntry As Integer
            Dim oBaseEntry, oBaseLine As Integer
            Dim oBaseType As String

            oDocuments = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oInvoices)
            oDocumentLines = oDocuments.Lines

            oDraft = New Datatable.SAP.AR.Client_Hdr
            oDraft_Dtls = New Datatable.SAP.AR.Client_Dtl
            oDraft_Frgs = New Datatable.SAP.AR.Client_Frg

            oDocEntry = Right(oBatchID, oBatchID.Length - 1)

            oDraft.getSalesInvoice(oDocEntry)
            oDraft_Dtls.getDocumentLine(oDocEntry)
            oDraft_Frgs.getFreightEntry(oDocEntry)

            oRecSet = oDraft.Execute
            'Assign value into Header record
            If oRecSet.RecordCount = 0 Then
                Throw New BaseException(BaseException.ErrorType.System, "CREATEDOC_0470", "No record was found base on Draft Entry [" & oDocEntry & "]")
            End If

            Do Until oRecSet.EoF
                With oRecSet.Fields
                    oDocuments.CardCode = .Item(Datatable.SAP.AR.Client_Hdr.CardCode).Value
                    oDocuments.CardName = .Item(Datatable.SAP.AR.Client_Hdr.CardName).Value
                    oDocuments.DocDate = .Item(Datatable.SAP.AR.Client_Hdr.DocDate).Value
                    oDocuments.DocDueDate = .Item(Datatable.SAP.AR.Client_Hdr.DocDueDate).Value
                    oDocuments.TaxDate = .Item(Datatable.SAP.AR.Client_Hdr.DocumentDate).Value

                    If Not .Item(Datatable.SAP.AR.Client_Hdr.BillToAddress).Value.ToString.Trim = "" Then
                        oDocuments.Address = .Item(Datatable.SAP.AR.Client_Hdr.BillToAddress).Value
                    End If

                    If Not .Item(Datatable.SAP.AR.Client_Hdr.ShipToAddress).Value.ToString.Trim = "" Then
                        oDocuments.Address2 = .Item(Datatable.SAP.AR.Client_Hdr.ShipToAddress).Value
                    End If

                    If .Item(Datatable.SAP.AR.Client_Hdr.DocType).Value = "I" Then
                        oDocuments.DocType = BoDocumentTypes.dDocument_Items
                    Else
                        oDocuments.DocType = BoDocumentTypes.dDocument_Service
                    End If

                    'Indicator
                    If Not .Item(Datatable.SAP.AR.Client_Hdr._Indicator).Value = "" Then
                        oDocuments.Indicator = .Item(Datatable.SAP.AR.Client_Hdr._Indicator).Value
                    End If

                    'Delivery Instruction
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.DelIns).Value = .Item(Datatable.SAP.AR.Client_Hdr.DelIns).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.CntctName).Value = .Item(Datatable.SAP.AR.Client_Hdr.CntctName).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.CntctTel).Value = .Item(Datatable.SAP.AR.Client_Hdr.CntctTel).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value = .Item(Datatable.SAP.AR.Client_Hdr.PCMSDocNum).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.DocSubject).Value = .Item(Datatable.SAP.AR.Client_Hdr.DocSubject).Value

                    If .Item(Datatable.SAP.AR.Client_Hdr.RefDate1).Value > "2000 Jan 30" Then
                        oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.RefDate1).Value = .Item(Datatable.SAP.AR.Client_Hdr.RefDate1).Value
                    End If
                    If .Item(Datatable.SAP.AR.Client_Hdr.RefDate2).Value > "2000 Jan 30" Then
                        oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.RefDate2).Value = .Item(Datatable.SAP.AR.Client_Hdr.RefDate2).Value
                    End If

                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.SubsiCode).Value = .Item(Datatable.SAP.AR.Client_Hdr.SubsiCode).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.PayTermDesc).Value = .Item(Datatable.SAP.AR.Client_Hdr.PayTermDesc).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.SlpName).Value = .Item(Datatable.SAP.AR.Client_Hdr.SlpName).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.SlpTel).Value = .Item(Datatable.SAP.AR.Client_Hdr.SlpTel).Value

                    oDocuments.DocCurrency = .Item(Datatable.SAP.AR.Client_Hdr.DocCur).Value
                    oDocuments.DocRate = .Item(Datatable.SAP.AR.Client_Hdr.DocRate).Value
                    oDocuments.Project = .Item(Datatable.SAP.AR.Client_Hdr.Project).Value

                    '14 Mar 10
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.AppWork).Value = .Item(Datatable.SAP.AR.Client_Hdr.AppWork).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.AppMOS).Value = .Item(Datatable.SAP.AR.Client_Hdr.AppMOS).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.AppDW).Value = .Item(Datatable.SAP.AR.Client_Hdr.AppDW).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.AppClaim).Value = .Item(Datatable.SAP.AR.Client_Hdr.AppClaim).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.AppVO).Value = .Item(Datatable.SAP.AR.Client_Hdr.AppVO).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.AppCC).Value = .Item(Datatable.SAP.AR.Client_Hdr.AppCC).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.RetenPrcnt).Value = .Item(Datatable.SAP.AR.Client_Hdr.RetenPrcnt).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.RetenMaxAmt).Value = .Item(Datatable.SAP.AR.Client_Hdr.RetenMaxAmt).Value

                    '20 Apr 18
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.GSTPrcnt).Value = .Item(Datatable.SAP.AR.Client_Hdr.GSTPrcnt).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.ThisGST).Value = .Item(Datatable.SAP.AR.Client_Hdr.ThisGST).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.CumGST).Value = .Item(Datatable.SAP.AR.Client_Hdr.CumGST).Value

                    '02 Nov 18
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.Client_Hdr.RevNo).Value = .Item(Datatable.SAP.AR.Client_Hdr.RevNo).Value
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

                        oBaseEntry = .Item(Datatable.SAP.AR.Client_Dtl.BaseEntry).Value
                        oBaseType = .Item(Datatable.SAP.AR.Client_Dtl.BaseType).Value
                        oBaseLine = .Item(Datatable.SAP.AR.Client_Dtl.BaseLine).Value

                        If oBaseEntry > 0 And Not oBaseType = "" And oBaseLine > 0 Then
                            oDocumentLines.BaseEntry = oBaseEntry
                            oDocumentLines.BaseLine = oBaseLine
                            oDocumentLines.BaseType = oBaseType

                            GoTo Record_MoveNext
                        End If

                        If oDocuments.DocType = BoDocumentTypes.dDocument_Items Then
                            oDocumentLines.ItemCode = .Item(Datatable.SAP.AR.Client_Dtl.ItemCode).Value
                            oDocumentLines.Quantity = .Item(Datatable.SAP.AR.Client_Dtl.Quantity).Value
                            oDocumentLines.Price = .Item(Datatable.SAP.AR.Client_Dtl.UnitPrice).Value
                        End If

                        If .Item(Datatable.SAP.AR.Client_Dtl._ShipDate).Value > "2000 Jan 30" Then
                            oDocumentLines.ShipDate = .Item(Datatable.SAP.AR.Client_Dtl._ShipDate).Value
                        End If

                        oDocumentLines.AccountCode = .Item(Datatable.SAP.AR.Client_Dtl.AcctCode).Value
                        oDocumentLines.ItemDescription = .Item(Datatable.SAP.AR.Client_Dtl.ItemName).Value

                        'If LocalCurrency = oDocuments.DocCurrency Then
                        oDocumentLines.LineTotal = .Item(Datatable.SAP.AR.Client_Dtl.LC_Total).Value
                        ' Else
                        'oDocumentLines.LineTotal = .Item(Datatable.SAP.AR.Client_Dtl.FC_Total).Value
                        'End If

                        oDocumentLines.ProjectCode = .Item(Datatable.SAP.AR.Client_Dtl.PrjCode).Value

                        If .Item(Datatable.SAP.AR.Client_Dtl._U_Size).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_Size).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_Size).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_Packing).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_Packing).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_Packing).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_Color).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_Color).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_Color).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_Brand).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_Brand).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_Brand).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_Model).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_Model).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_Model).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_SupInvNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_SupInvNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_SupInvNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_QuoteNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_QuoteNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_QuoteNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_SourceType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_SourceType).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_SourceType).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_SourceLine).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_SourceLine).Value

                        If .Item(Datatable.SAP.AR.Client_Dtl._U_DestType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_DestType).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_DestType).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_UOM).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_UOM).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_UOM).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_PCMSDocNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_PCMSDocNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_PCMSDocNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_BillNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_BillNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_BillNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_SecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_SecNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_SecNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_SubSecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_SubSecNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_SubSecNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_PageNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_PageNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_PageNum).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_Quantity).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_Quantity).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_Price).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_Price).Value

                        If .Item(Datatable.SAP.AR.Client_Dtl._U_ItemType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_ItemType).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_ItemType).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_MCBillNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_MCBillNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_MCBillNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_MCSecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_MCSecNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_MCSecNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_MCSubSecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_MCSubSecNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_MCSubSecNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_MCPageNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_MCPageNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_MCPageNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_PriceType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_PriceType).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_PriceType).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_AppMethod).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_AppMethod).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_AppMethod).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_LineType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_LineType).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_LineType).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_MCLineNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_MCLineNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_MCLineNum).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_OpenPrcnt).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_OpenPrcnt).Value

                        If .Item(Datatable.SAP.AR.Client_Dtl._U_ContraFlag).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_ContraFlag).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_ContraFlag).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_RecoverFlag).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_RecoverFlag).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_RecoverFlag).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_RecoverStatus).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_RecoverStatus).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_RecoverStatus).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_SubLineNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_SubLineNum).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_MCSubLineNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_MCSubLineNum).Value

                        If .Item(Datatable.SAP.AR.Client_Dtl._U_ClientRef).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_ClientRef).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_ClientRef).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_SourceEntry).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_SourceEntry).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_DestEntry).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_DestEntry).Value

                        If .Item(Datatable.SAP.AR.Client_Dtl._U_IncomeCode).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_IncomeCode).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_IncomeCode).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_IPCode).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_IPCode).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_IPCode).Value
                        End If
                        If .Item(Datatable.SAP.AR.Client_Dtl._U_BillLineNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_BillLineNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_BillLineNum).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.Client_Dtl._U_BillSubLineNum).Value = .Item(Datatable.SAP.AR.Client_Dtl._U_BillSubLineNum).Value
                    End With
Record_MoveNext:
                    oRecSet.MoveNext()
                Loop
            Else
                Throw New BaseException(BaseException.ErrorType.System, _
                                        "CREATEDOC_0550", _
                                        "No record was found base on Draft Entry [" & oDocEntry & "]")
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
                        oDocumentExpenses.ExpenseCode = .Item(Datatable.SAP.AR.Client_Frg.FreightCode).Value
                        'If LocalCurrency = oDocuments.DocCurrency Then
                        oDocumentExpenses.LineTotal = .Item(Datatable.SAP.AR.Client_Frg.LC_Total).Value
                        ' Else
                        'oDocumentExpenses.LineTotal = .Item(Datatable.SAP.AR.Client_Frg.FC_Total).Value
                        'End If
                        oRecSet.MoveNext()
                    Loop
                End If
            End With

            If oDocuments.Add = 0 Then
                Dim NewObjKey As Integer

                NewObjKey = Company.GetNewObjectKey
                MyBase.ToMapping(NewObjKey, 13, oDocEntry)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
                Return Company.GetNewObjectKey
            Else

                oLogMessage.AddFailureLine(oBatchID, Company.GetLastErrorDescription)

                ToErrorTable(oDocEntry, _
                             13, _
                             Company.GetLastErrorCode, _
                             "Unable to Create A/R Invoice", _
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
            oMapping.getSalesInvoice(oDraftKey)
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

            oInvDocuments = currentCompany.GetBusinessObject(BoObjectTypes.oInvoices)
            oRinDocuments = currentCompany.GetBusinessObject(BoObjectTypes.oCreditNotes)

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
                oRinDocumentLines.BaseType = 13
            Next

            oRinDocuments.DiscountPercent = oInvDocuments.DiscountPercent

            If oRinDocuments.Add = 0 Then
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
                Return Company.GetNewObjectKey
            Else
                oLogMessage.AddFailureLine(oBatchID, Company.GetLastErrorDescription)

                ToErrorTable(oDraftKey, _
                             13, _
                             Company.GetLastErrorCode, _
                             "Unable to Delete Draft in SAP", _
                             Company.GetLastErrorDescription)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
                Return 0
            End If
        End Function
        Function CloseDraft(ByVal pBatchID As String) As Integer
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oDocEntry As Integer
            Dim oDraft As New Datatable.SAP.AR.Client_Hdr

            oDocEntry = Right(pBatchID, pBatchID.Length - 1)

            oDraft.getSalesInvoice(oDocEntry)
            oDraft.DocStatus = "C"
            oDraft.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Function
        Function DeleteDraft(ByVal pBatchID As String) As Integer
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oDocEntry As Integer
            Dim oDraft As New Datatable.SAP.AR.Client_Hdr

            oDocEntry = Right(pBatchID, pBatchID.Length - 1)

            oDraft.getSalesInvoice(oDocEntry)
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