Imports SAPbobsCOM
Imports CPS.CPSLIB.Debug
Namespace SyncMainClass
    ''' <summary>
    ''' Supplier Payment Certificate Data, Inherits Snchronization Class
    ''' </summary>
    ''' <remarks></remarks>
    Class A_ItmInvoice
        Inherits [Interface].Synchronization

        Private ReadOnly mJobReference As String = "A/P Item Invoice with Approval Procedure Synchronization (Fiex & SAP)"
        Private LocalCurrency As String

        Public Sub New(ByVal pCompany As SAPbobsCOM.Company)
            MyBase.New(pCompany, New FlexConnection)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            MyBase.JobName = mJobReference
            ObjType = 18
            'oLogMessage.FileName = "Appr-Invoice"
            oLogMessage.FileName = "AP-ItmInvoice"

            Dim sboBob As SBObob = pCompany.GetBusinessObject(BoObjectTypes.BoBridge)
            LocalCurrency = ""
            LocalCurrency = Trim(sboBob.GetLocalCurrency.Fields.Item(0).Value)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Sub

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
                oImport.AcctCode = ExAcctcode(.AcctCode, pExportPCMS).Trim
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

#Region "Data Mapping"
        Function result(ByVal pFields As SAPbobsCOM.Fields, ByVal pField_Dtls As SAPbobsCOM.Fields) As ExportPCMS
            Dim oReturnResult As ExportPCMS
            Dim oDocEntry As Integer, oLineNums As Integer
            Dim oDocCurrency As String

            oReturnResult = New ExportPCMS
            With pFields
                'Get the document entry form the record set result
                oDocEntry = .Item(Datatable.SAP.AP.Supplier_Hdr.DocEntry).Value
                oLineNums = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl.LineNo).Value
                oDocCurrency = Trim(.Item(Datatable.SAP.AP.Supplier_Hdr.DocCur).Value)

                oReturnResult.RefNo = oDocEntry
                oReturnResult.SubsidiaryCode = .Item(Datatable.SAP.AP.Supplier_Hdr.SubsiCode).Value
                'Supplier Payment Certificate Data
                oReturnResult.CertEntry = "M" & RSet(.Item(Datatable.SAP.AP.Supplier_Hdr.DocEntry).Value, 9).Replace(" ", "0")
                'oReturnResult.VoucherType  (GJ)
                'Karrson: CHange
                'oReturnResult.ValuationDate = .Item(Datatable.SAP.AP.Supplier_Hdr.DocDate).Value
                oReturnResult.ValuationDate = .Item(Datatable.SAP.AP.Supplier_Hdr.DocumentDate).Value

                'oReturnResult.Description  (General Journal)
                oReturnResult.AcctCode = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl.AcctCode).Value
                oReturnResult.BPCode = .Item(Datatable.SAP.AP.Supplier_Hdr.CardCode).Value
                oReturnResult.ProjectCode = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl.PrjCode).Value
                oReturnResult.CostCode = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl.AcctCode).Value
                oReturnResult.CertNumber = .Item(Datatable.SAP.AP.Supplier_Hdr.PCMSDocNum).Value
                oReturnResult.VInvoiceNo = Trim(pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl._U_RefNum).Value)
                'oReturnResult.DocType
                oReturnResult.CertDate = .Item(Datatable.SAP.AP.Supplier_Hdr.DocumentDate).Value
                oReturnResult.CertDueDate = .Item(Datatable.SAP.AP.Supplier_Hdr.DocDueDate).Value
                oReturnResult.DocCurrency = oDocCurrency
                oReturnResult.SubCon_No = Right(CStr(.Item(Datatable.SAP.AP.Supplier_Hdr.PCMSDocNum).Value).Trim, 6)
                oReturnResult.RefCardCode = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl._U_RefCardCode).Value

                'Calculate the Total Before Discount 
                Dim oDocTotal As Double ', oDisPercent As Double
                'If Me.LocalCurrency.ToUpper = oDocCurrency.ToUpper Then
                '    oReturnResult.DocRate = 1
                '   oDocTotal = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl.LC_Total).Value
                'Else
                '    oReturnResult.DocRate = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl.DocRate).Value
                '    oDocTotal = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl.FC_Total).Value
                'End If
                oReturnResult.DocRate = 1
                oDocTotal = pField_Dtls.Item(Datatable.SAP.Draft.Detail._INMPrice).Value * _
                            pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl.Quantity).Value
                'oDisPercent = .Item(Datatable.SAP.AP.Supplier_Dtl.DisPercent).Value

                oReturnResult.TotalBefDis = oDocTotal '/ (1 + oDisPercent / 100)
                oReturnResult.DocTotal = oDocTotal
                'oReturnResult.Allocation

                Dim oCertData As String = ""
                Dim oPANo As String, oCONo As String, oPONo As String
                'Dim oSourceType As String, oSourcePCMSDocNum As String

                '--- remark on 20140731
                'oPANo = .Item(Datatable.SAP.AP.SubCon_Hdr._U_PANo).Value
                'oCONo = .Item(Datatable.SAP.AP.SubCon_Hdr._U_CONo).Value
                'oPONo = .Item(Datatable.SAP.AP.SubCon_Hdr._U_PONo).Value
                '-----

                'oSourceType = .Item(Datatable.SAP.AP.Supplier_Dtl._U_SourceType).Value
                'oSourcePCMSDocNum = .Item(Datatable.SAP.AP.Supplier_Dtl._U_SourcePCMSDocNum).Value

                '--- remark on 20140731
                'If Not oPANo = "" Then
                'oCertData &= "PA No. " & oPANo & "/"
                'End If

                'If Not oCONo = "" Then
                'oCertData &= "CO No. " & oCONo & "/"
                'End If

                'If Not oPONo = "" Then
                'oCertData &= "PO No. " & oPONo & "/"
                'End If
                '-----

                'If Not oSourceType = "PA" Then
                '    oCertData &= "PA No. " & oSourcePCMSDocNum
                'End If

                'If Not oSourceType = "CO" Then
                '    oCertData &= "CO No. " & oSourcePCMSDocNum
                'End If

                'If Not oSourceType = "PO" Then
                '    oCertData &= "PO No. " & oSourcePCMSDocNum
                'End If

                oCertData = pField_Dtls.Item(Datatable.SAP.AP.Supplier_Dtl.ItemName).Value

                If Not oCertData = "" Then
                    'oCertData = Left(oCertData, oCertData.Length - 1)
                    oCertData = Left(oCertData, 60)
                End If

                oReturnResult.CertData = oCertData
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
                oReturnResult.DocCurrency = oDocCurrency
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
                'oReturnResult.CertData = Me.getPONum(oDocEntry)

                'Karrson: Start
                Dim oCertData As String = ""
                Dim oPANo As String, oCONo As String, oPONo As String

                'Dim oSourceType As String, oSourcePCMSDocNum As String

                oPANo = .Item(Datatable.SAP.AP.SubCon_Hdr._U_PANo).Value
                oCONo = .Item(Datatable.SAP.AP.SubCon_Hdr._U_CONo).Value
                oPONo = .Item(Datatable.SAP.AP.SubCon_Hdr._U_PONo).Value

                'oSourceType = .Item(Datatable.SAP.AP.Supplier_Dtl._U_SourceType).Value
                'oSourcePCMSDocNum = .Item(Datatable.SAP.AP.Supplier_Dtl._U_SourcePCMSDocNum).Value

                If Not oPANo = "" Then
                    oCertData &= "PA No. " & oPANo & "/"
                End If

                If Not oCONo = "" Then
                    oCertData &= "CO No. " & oCONo & "/"
                End If

                If Not oPONo = "" Then
                    oCertData &= "PO No. " & oPONo & "/"
                End If


                'If Not oSourceType = "PA" Then
                '    oCertData &= "PA No. " & oSourcePCMSDocNum
                'End If

                'If Not oSourceType = "CO" Then
                '    oCertData &= "CO No. " & oSourcePCMSDocNum
                'End If

                'If Not oSourceType = "PO" Then
                '    oCertData &= "PO No. " & oSourcePCMSDocNum
                'End If

                If Not oCertData = "" Then
                    oCertData = Left(oCertData, oCertData.Length - 1)
                End If

                oReturnResult.CertData = oCertData
                'Karrson: End

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
                    'Karrson: Change
                    '.ValuationDate = pField_Hdr.Item(Datatable.SAP.AP.Supplier_Hdr.DocDate).Value
                    .ValuationDate = pField_Hdr.Item(Datatable.SAP.AP.Supplier_Hdr.DocumentDate).Value
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
                    .DocCurrency = oDocCurrency
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
                    '.CertData = Me.getPONum(oDocEntry)
                    'Karrson: Add Start
                    Dim oCertData As String = ""
                    Dim oPANo As String, oCONo As String, oPONo As String

                    oPANo = oRecSet.Fields.Item(Datatable.SAP.AP.SubCon_Hdr._U_PANo).Value
                    oCONo = oRecSet.Fields.Item(Datatable.SAP.AP.SubCon_Hdr._U_CONo).Value
                    oPONo = oRecSet.Fields.Item(Datatable.SAP.AP.SubCon_Hdr._U_PONo).Value

                    If Not oPANo = "" Then
                        oCertData &= "PA No. " & oPANo & "/"
                    End If

                    If Not oCONo = "" Then
                        oCertData &= "CO No. " & oCONo & "/"
                    End If

                    If Not oPONo = "" Then
                        oCertData &= "PO No. " & oPONo & "/"
                    End If

                    If Not oCertData = "" Then
                        oCertData = Left(oCertData, oCertData.Length - 1)
                    End If

                    .CertData = oCertData

                    'End Start

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
        Function getPONum(ByVal pDocEntry As Integer, _
                          ByVal pLineNum As Integer) As String
            Dim oRecSet As Recordset
            Dim sqlStr As String = ""
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            'sqlStr &= "	Select Top 1 " & Datatable.SAP.Draft.Header.TableName & "." & Datatable.SAP.Draft.Header.PCMSDocNum & " From " & Datatable.SAP.AP.Supplier_Dtl.TableName & " 	" & vbCrLf
            'sqlStr &= "	Inner Join " & Datatable.SAP.Draft.Header.TableName & " On " & Datatable.SAP.Draft.Header.TableName & "." & Datatable.SAP.Draft.Header.DocEntry & " = " & Datatable.SAP.AP.Supplier_Dtl.TableName & "." & Datatable.SAP.AP.Supplier_Dtl.BaseEntry & " And " & Datatable.SAP.Draft.Header.TableName & "." & Datatable.SAP.Draft.Header.ObjType & " = " & Datatable.SAP.AP.Supplier_Dtl.TableName & "." & Datatable.SAP.AP.Supplier_Dtl.BaseType & "	" & vbCrLf
            'sqlStr &= "	Where " & Datatable.SAP.AP.Supplier_Dtl.TableName & "." & Datatable.SAP.AP.Supplier_Dtl.DocEntry & " = '" & CStr(pDocEntry).Trim & "'	" & vbCrLf
            sqlStr = "Select PCMSDocNum From getSourcePCMSDocNum(" & pDocEntry & ", " & pLineNum & ")"
            oRecSet = commonRecordSet.execute(sqlStr)

            If oRecSet.RecordCount = 0 Then
                Return Nothing
            Else
                Return oRecSet.Fields.Item("PCMSDocNum").Value
            End If
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Function

        Public Overrides Sub Export()
            'Declare Datatable 
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oPTVOU As New Datatable.Flex.PTVOU
            Dim Itm_OPCH As Datatable.SAP.Draft.Header
            Dim Itm_PCH1 As New Datatable.SAP.Draft.Detail

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
                Itm_OPCH = New Datatable.SAP.Draft.Header
                recset_Hdr = Itm_OPCH.Execute
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Qeuery: " & Itm_OPCH.SelectQuery & " " & Itm_OPCH.filterQuery & " " & Itm_OPCH.OrderByQuery, TimeSet.Status.Start)
                WriteDebug("Supplier Payment Cert Header Qeuery: " & Itm_OPCH.SelectQuery & " " & Itm_OPCH.filterQuery & " " & Itm_OPCH.OrderByQuery)
                Do Until recset_Hdr.EoF
                    Try
                        oDocEntry = recset_Hdr.Fields.Item(Datatable.SAP.Draft.Header.DocEntry).Value
                        oDocCurrency = Trim(recset_Hdr.Fields.Item(Datatable.SAP.Draft.Header.DocCur).Value)

                        If MyBase.Chk_ErrOverLap(oDocEntry, eObjType.ot_PurchaseInvoice) Then
                            GoTo Move_Next
                        End If

                        'Begin transaction
                        Me.StartTransaction()

                        'Get Document Detail by Document entry
                        Itm_PCH1 = New Datatable.SAP.Draft.Detail
                        Itm_PCH1.getDocumentLine(oDocEntry)
                        recset_Dtl = Itm_PCH1.Execute

                        oExports = Nothing
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Qeuery: " & Itm_PCH1.SelectQuery & " " & Itm_PCH1.filterQuery & Itm_PCH1.OrderByQuery, TimeSet.Status.Start)
                        WriteDebug("Supplier Payment Cert Detail Qeuery: " & Itm_PCH1.SelectQuery & " " & Itm_PCH1.filterQuery & " " & Itm_PCH1.OrderByQuery)
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
                        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Qeuery: " & Itm_PCH1.SelectQuery & " " & Itm_PCH1.filterQuery & Itm_PCH1.OrderByQuery, TimeSet.Status.Finish)

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
                        ' Karrson: Change
                        'oSynchHistory.Add_Submitted("M" & RSet(oDocEntry.ToString.Trim, 9).Replace(" ", "0"), True)
                        WriteDebug("Supplier Payment Cert Export")
                        oSynchHistory.Add_Acknowledge("M" & RSet(oDocEntry.ToString.Trim, 9).Replace(" ", "0"))

                        Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                        oLogMessage.AddSuccessLine("[EXPORT]AP " & oDocEntry, "Operation Success")
                    Catch b_ex As BaseException
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip("[EXPORT]AP " & oDocEntry, b_ex.toString)
                        Try
                            WriteDebug(b_ex.StackTrace())
                        Catch ex As Exception

                        End Try

                        If Not oDocEntry = 0 Then
                            ToErrorTable(oDocEntry, _
                                             18, _
                                             -9999, _
                                             "[EXPORT]AP " & oDocEntry, _
                                             b_ex.toString)
                        End If
                    Catch ex As Exception
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip("[EXPORT]AP " & oDocEntry, ex.ToString)

                        If Not oDocEntry = 0 Then
                            ToErrorTable(oDocEntry, _
                                         18, _
                                         -9999, _
                                         "[EXPORT]AP " & oDocEntry, _
                                         ex.ToString)
                        End If
                    End Try

                    Itm_OPCH.Execute(String.Format("UPDATE ODRF SET U_CertStatus = 'T' where DocEntry = {0}", oDocEntry))

Move_Next:
                    recset_Hdr.MoveNext()
                Loop
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Qeuery: " & Itm_OPCH.SelectQuery & " " & Itm_OPCH.filterQuery & Itm_OPCH.OrderByQuery, TimeSet.Status.Finish)
            Catch b_ex As BaseException
                Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                oLogMessage.AddExceptionSkip("[EXPORT]AP " & oDocEntry, b_ex.toString)

                If Not oDocEntry = 0 Then
                    ToErrorTable(oDocEntry, _
                                 18, _
                                 -9999, _
                                 "[EXPORT]AP " & oDocEntry, _
                                 b_ex.toString)
                End If
            Catch ex As Exception
                Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                oLogMessage.AddExceptionSkip("[EXPORT]AP " & oDocEntry, ex.ToString)

                If Not oDocEntry = 0 Then
                    ToErrorTable(oDocEntry, _
                                 18, _
                                 -9999, _
                                 "[EXPORT]AP " & oDocEntry, _
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
                Me.Import_Exception()
                Me.Import_Reject()
                Me.Import_Delete()
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

            oLogMessage.AddReferenceLine("POSTED", "Create New A/P Item Invoice in SAP System.")

            oPIVOU = New Datatable.Flex.PIVOU

            oPIVOU.getItemAP(Datatable.Flex.PIVOU.flexStatus.fs_Accepted)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery
            oDataTable = oFlexConnection.DataTable(sqlStr)
            WriteDebug("Supplier Payment Cert, Import_Posted: SQL Query" & sqlStr)
            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim

                        If Chk_ErrOverLap(BatchID, eObjType.ot_PurchaseInvoice) Then
                            GoTo Move_Next
                        End If

                        'Add by Michael, begin
                        Dim cRevTrans As String
                        'Dim cComCode As String = ""
                        'Dim cPIVOU_REF_NUM As String = ""
                        Dim nDocEntry As Long = 0

                        'Dim _dtBatch As System.Data.DataTable
                        '_dtBatch = oFlexConnection.DataTable(oPIVOU.BatchSql(BatchID))
                        'For Each ooDataRow As System.Data.DataRow In _dtBatch.Rows
                        '    cComCode = ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_COM_CDE)
                        '    cPIVOU_REF_NUM = ooDataRow.Item(Datatable.Flex.PIVOU._PIVOU_REF_NUM)
                        'Next

                        'Add by Michael, begin
                        cRevTrans = "N"
                        nDocEntry = 0
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
            oLogMessage.AddReferenceLine("POSTED", "-----------------------------------------")
        End Sub

        'Process Return Data with Error
        Public Sub Import_Exception()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
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
            WriteDebug("Supplier Payment Cert Import Exception Query: " & sqlStr)
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

            oPIVOU.getItemAP(Datatable.Flex.PIVOU.flexStatus.fs_Rejected)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery
            oDataTable = oFlexConnection.DataTable(sqlStr)
            WriteDebug("Supplier Payment Cert, Import_Reject SQL Query: " & sqlStr)
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

            oPIVOU.getItemAP(Datatable.Flex.PIVOU.flexStatus.fs_Delete)
            sqlStr = oPIVOU.sqlStr & oPIVOU.filterQuery
            oDataTable = oFlexConnection.DataTable(sqlStr)
            WriteDebug("Supplier Payment Cert Import Exception Query: " & sqlStr)
            WriteDebug("oDataTable is nothing : " & (oDataTable Is Nothing).ToString)
            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        WriteDebug("Check Point 1")
                        BatchID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID).ToString.Trim
                        WriteDebug("Check Point 2")
                        If Chk_ErrOverLap(BatchID, eObjType.ot_PurchaseInvoice) Then
                            GoTo Move_Next
                        End If

                        'Start Transaction
                        Me.StartTransaction()
                        WriteDebug("Check Point 3")
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
                        WriteDebug("Check Point 4")
                        Me.DeleteDraft(BatchID)

                        'Update PIVOU table in Flex Server
                        WriteDebug("Check Point 5")
                        Me.UpdateFlex(BatchID)

                        'Log History into Synchronization History table
                        SyncHistory = New Datatable.SAP.Sync_History
                        WriteDebug("Check Point 6")
                        SyncHistory.Add_Deleted(BatchID)

                        'End Transaction
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)
                        WriteDebug("Check Point 7")
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

#Region "Document Flow in SAP system"
        Function CreateDocument(ByVal oBatchID As String) As Integer
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oDocEntry As Integer

            oDocEntry = Right(oBatchID, oBatchID.Length - 1)
            Return MyBase.setDraftToAPDocument(oDocEntry)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
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
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Function
        Function CloseDraft(ByVal pBatchID As String) As Integer
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oDocEntry As Integer
            Dim oDraft As New Datatable.SAP.Draft.Header

            oDocEntry = Right(pBatchID, pBatchID.Length - 1)

            oDraft.getPurchaseInvoice(oDocEntry)
            oDraft.WddStatus = "Y"
            oDraft.DocStatus = "C"
            oDraft.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)
            oDraft.Execute("Update ODRF set U_CertStatus = 'A' Where DocEntry = " & oDocEntry)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Function
        Function DeleteDraft(ByVal pBatchID As String) As Integer
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oDocEntry As Integer
            Dim oDraft As Datatable.SAP.Draft.Header
            WriteDebug("Supplier Payment: Delete Draft, Batch ID: " & pBatchID)
            oDocEntry = Right(pBatchID, pBatchID.Length - 1)
            WriteDebug("DocEntry: " & oDocEntry.ToString())
            WriteDebug("Check Point Delete 1")
            MyBase.Update_ApprovalStatus_N(oDocEntry)
            WriteDebug("Check Point Delete 2")
            oDraft = New Datatable.SAP.Draft.Header
            oDraft.WddStatus = "N"
            oDraft.DocStatus = "C"
            'Karrson: add approval status

            oDraft.getPurchaseInvoice(oDocEntry)
            WriteDebug("Check Point Delete 3")
            oDraft.Execute("Update ODRF set U_CertStatus = 'R' Where DocEntry = " & oDocEntry)
            WriteDebug("Check Point Delete 4")
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