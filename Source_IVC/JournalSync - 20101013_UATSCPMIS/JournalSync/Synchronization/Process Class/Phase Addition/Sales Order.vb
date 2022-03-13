Imports SAPbobsCOM

Namespace SyncMainClass
    Class Sales_Order
        Inherits [Interface].Synchronization

        Private ReadOnly mJobReference As String = "Sales Order (SAP)"
        Private LocalCurrency As String

        Public Sub New(ByVal pCompany As SAPbobsCOM.Company)
            MyBase.New(pCompany)
            MyBase.JobName = mJobReference
            ObjType = 17
            oLogMessage.FileName = "AR-Order"

            Dim sboBob As SBObob = pCompany.GetBusinessObject(BoObjectTypes.BoBridge)
            LocalCurrency = ""
            LocalCurrency = Trim(sboBob.GetLocalCurrency.Fields.Item(0).Value)
        End Sub

        Public Overrides Sub Export()
            'Useless
        End Sub

        Public Overrides Sub Import()
            Dim oDraft As Datatable.SAP.AR.ORDR_Hdr
            Dim oRecSet As SAPbobsCOM.Recordset
            Dim SyncHistory As Datatable.SAP.Sync_History
            Dim oDocEntry As Integer = 0
            Dim oNewObjKey As Integer

            oDraft = New Datatable.SAP.AR.ORDR_Hdr
            oLogMessage.AddReferenceLine("POSTED", "Create New Sales Order in SAP System.")

            oRecSet = oDraft.Execute
            Do Until oRecSet.EoF
                Try
                    Me.StartTransaction()
                    oDocEntry = oRecSet.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.DocEntry).Value
                    'Create A/P Item Invoice in SAP
                    oNewObjKey = Me.CreateDocument(oDocEntry)

                    If Not oNewObjKey = 0 Then
                        'Update Document Draft Status in [C] when Delete
                        Me.CloseDraft(oDocEntry)

                        'Log History into Synchronization History table
                        SyncHistory = New Datatable.SAP.Sync_History
                        SyncHistory.Add_Posted("O" & CStr(oDocEntry).Trim)

                        'End Transaction
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                        oLogMessage.AddSuccessLine(oDocEntry, "Create Document Success")
                    Else
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                    End If
                Catch b_ex As BaseException
                    Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                    oLogMessage.AddExceptionSkip(oDocEntry, b_ex.toString)

                    ToErrorTable(oDocEntry, _
                                 17, _
                                 -9999, _
                                 "[IMPORT]AR " & oDocEntry, _
                                 b_ex.toString)
                Catch ex As Exception
                    Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                    oLogMessage.AddExceptionSkip(oDocEntry, ex.ToString)

                    ToErrorTable(oDocEntry, _
                                 17, _
                                 -9999, _
                                 "[IMPORT]AR " & oDocEntry, _
                                 ex.ToString)
                End Try

                oRecSet.MoveNext()
            Loop
        End Sub

#Region "Document Flow in SAP system"
        Function CreateDocument(ByVal pDocEntry As Integer) As Integer
            Dim oDocuments As SAPbobsCOM.Documents
            Dim oDocumentLines As SAPbobsCOM.Document_Lines
            Dim oDocumentExpenses As SAPbobsCOM.DocumentsAdditionalExpenses
            Dim oRecSet As SAPbobsCOM.Recordset
            Dim oDraft As Datatable.SAP.AR.ORDR_Hdr
            Dim oDraft_Dtls As Datatable.SAP.AR.ORDR_Dtl
            Dim oDraft_Frgs As Datatable.SAP.AR.ORDR_Frg

            Dim oBaseEntry, oBaseLine As Integer
            Dim oBaseType As String

            oDocuments = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oOrders)
            oDocumentLines = oDocuments.Lines

            oDraft = New Datatable.SAP.AR.ORDR_Hdr
            oDraft_Dtls = New Datatable.SAP.AR.ORDR_Dtl
            oDraft_Frgs = New Datatable.SAP.AR.ORDR_Frg

            oDraft.getDocument(pDocEntry)
            oDraft_Dtls.getDocumentLine(pDocEntry)
            oDraft_Frgs.getFreightEntry(pDocEntry)

            oRecSet = oDraft.Execute
            'Assign value into Header record
            If oRecSet.RecordCount = 0 Then
                Throw New BaseException(BaseException.ErrorType.System, "CREATEDOC_0470", "No record was found base on Draft Entry [" & pDocEntry & "]")
            End If

            Do Until oRecSet.EoF
                With oRecSet.Fields
                    oDocuments.CardCode = .Item(Datatable.SAP.AR.ORDR_Hdr.CardCode).Value
                    oDocuments.CardName = .Item(Datatable.SAP.AR.ORDR_Hdr.CardName).Value
                    oDocuments.DocDate = .Item(Datatable.SAP.AR.ORDR_Hdr.DocDate).Value
                    oDocuments.DocDueDate = .Item(Datatable.SAP.AR.ORDR_Hdr.DocDueDate).Value
                    oDocuments.TaxDate = .Item(Datatable.SAP.AR.ORDR_Hdr.DocumentDate).Value

                    If Not .Item(Datatable.SAP.AR.ORDR_Hdr.BillToAddress).Value.ToString.Trim = "" Then
                        oDocuments.Address = .Item(Datatable.SAP.AR.ORDR_Hdr.BillToAddress).Value
                    End If

                    If Not .Item(Datatable.SAP.AR.ORDR_Hdr.ShipToAddress).Value.ToString.Trim = "" Then
                        oDocuments.Address2 = .Item(Datatable.SAP.AR.ORDR_Hdr.ShipToAddress).Value
                    End If

                    If .Item(Datatable.SAP.AR.ORDR_Hdr.DocType).Value = "I" Then
                        oDocuments.DocType = BoDocumentTypes.dDocument_Items
                    Else
                        oDocuments.DocType = BoDocumentTypes.dDocument_Service
                    End If

                    'Indicator
                    If Not .Item(Datatable.SAP.AR.ORDR_Hdr._Indicator).Value = "" Then
                        oDocuments.Indicator = .Item(Datatable.SAP.AR.ORDR_Hdr._Indicator).Value
                    End If

                    'Delivery Instruction
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.DelIns).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.DelIns).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.CntctName).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.CntctName).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.CntctTel).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.CntctTel).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.PCMSDocNum).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.PCMSDocNum).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.DocSubject).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.DocSubject).Value

                    If .Item(Datatable.SAP.AR.ORDR_Hdr.RefDate1).Value > "2000 Jan 30" Then
                        oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.RefDate1).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.RefDate1).Value
                    End If
                    If .Item(Datatable.SAP.AR.ORDR_Hdr.RefDate2).Value > "2000 Jan 30" Then
                        oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.RefDate2).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.RefDate2).Value
                    End If

                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.SubsiCode).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.SubsiCode).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.PayTermDesc).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.PayTermDesc).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.SlpName).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.SlpName).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.SlpTel).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.SlpTel).Value

                    oDocuments.DocCurrency = .Item(Datatable.SAP.AR.ORDR_Hdr.DocCur).Value
                    oDocuments.DocRate = .Item(Datatable.SAP.AR.ORDR_Hdr.DocRate).Value
                    oDocuments.Project = .Item(Datatable.SAP.AR.ORDR_Hdr.Project).Value

                    '14 Mar 10
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.AppWork).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.AppWork).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.AppMOS).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.AppMOS).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.AppDW).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.AppDW).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.AppClaim).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.AppClaim).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.AppVO).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.AppVO).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.AppCC).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.AppCC).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.RetenPrcnt).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.RetenPrcnt).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.RetenMaxAmt).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.RetenMaxAmt).Value

                    '18 Apr 18

                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.AppGST).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.AppGST).Value
                    oDocuments.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Hdr.GSTPrcnt).Value = .Item(Datatable.SAP.AR.ORDR_Hdr.GSTPrcnt).Value

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

                        oBaseEntry = .Item(Datatable.SAP.AR.ORDR_Dtl.BaseEntry).Value
                        oBaseType = .Item(Datatable.SAP.AR.ORDR_Dtl.BaseType).Value
                        oBaseLine = .Item(Datatable.SAP.AR.ORDR_Dtl.BaseLine).Value

                        If oBaseEntry > 0 And Not oBaseType = "" And oBaseLine > 0 Then
                            oDocumentLines.BaseEntry = oBaseEntry
                            oDocumentLines.BaseLine = oBaseLine
                            oDocumentLines.BaseType = oBaseType

                            GoTo Record_MoveNext
                        End If

                        If oDocuments.DocType = BoDocumentTypes.dDocument_Items Then
                            oDocumentLines.ItemCode = .Item(Datatable.SAP.AR.ORDR_Dtl.ItemCode).Value
                            oDocumentLines.Quantity = .Item(Datatable.SAP.AR.ORDR_Dtl.Quantity).Value
                            oDocumentLines.UnitPrice = .Item(Datatable.SAP.AR.ORDR_Dtl.Quantity).Value
                        End If

                        If .Item(Datatable.SAP.AR.ORDR_Dtl._ShipDate).Value > "2000 Jan 30" Then
                            oDocumentLines.ShipDate = .Item(Datatable.SAP.AR.ORDR_Dtl._ShipDate).Value
                        End If

                        oDocumentLines.AccountCode = .Item(Datatable.SAP.AR.ORDR_Dtl.AcctCode).Value
                        oDocumentLines.ItemDescription = .Item(Datatable.SAP.AR.ORDR_Dtl.ItemName).Value

                        If LocalCurrency = oDocuments.DocCurrency Then
                            oDocumentLines.LineTotal = .Item(Datatable.SAP.AR.ORDR_Dtl.LC_Total).Value
                        Else
                            oDocumentLines.LineTotal = .Item(Datatable.SAP.AR.ORDR_Dtl.FC_Total).Value
                        End If

                        oDocumentLines.ProjectCode = .Item(Datatable.SAP.AR.ORDR_Dtl.PrjCode).Value

                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_Size).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_Size).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_Size).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_Packing).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_Packing).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_Packing).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_Color).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_Color).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_Color).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_Brand).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_Brand).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_Brand).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_Model).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_Model).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_Model).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_SupInvNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_SupInvNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_SupInvNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_QuoteNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_QuoteNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_QuoteNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_SourceType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_SourceType).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_SourceType).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_SourceLine).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_SourceLine).Value

                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_DestType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_DestType).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_DestType).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_UOM).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_UOM).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_UOM).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_PCMSDocNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_PCMSDocNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_PCMSDocNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_BillNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_BillNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_BillNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_SecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_SecNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_SecNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_SubSecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_SubSecNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_SubSecNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_PageNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_PageNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_PageNum).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_Quantity).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_Quantity).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_Price).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_Price).Value

                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_ItemType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_ItemType).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_ItemType).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_MCBillNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_MCBillNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_MCBillNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_MCSecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_MCSecNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_MCSecNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_MCSubSecNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_MCSubSecNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_MCSubSecNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_MCPageNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_MCPageNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_MCPageNum).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_PriceType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_PriceType).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_PriceType).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_AppMethod).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_AppMethod).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_AppMethod).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_LineType).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_LineType).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_LineType).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_MCLineNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_MCLineNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_MCLineNum).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_OpenPrcnt).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_OpenPrcnt).Value

                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_ContraFlag).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_ContraFlag).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_ContraFlag).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_RecoverFlag).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_RecoverFlag).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_RecoverFlag).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_RecoverStatus).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_RecoverStatus).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_RecoverStatus).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_SubLineNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_SubLineNum).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_MCSubLineNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_MCSubLineNum).Value

                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_ClientRef).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_ClientRef).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_ClientRef).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_SourceEntry).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_SourceEntry).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_DestEntry).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_DestEntry).Value

                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_IncomeCode).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_IncomeCode).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_IncomeCode).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_IPCode).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_IPCode).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_IPCode).Value
                        End If
                        If .Item(Datatable.SAP.AR.ORDR_Dtl._U_BillLineNum).Value <> "" Then
                            oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_BillLineNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_BillLineNum).Value
                        End If

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_BillSubLineNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_BillSubLineNum).Value


                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_ItemNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_ItemNum).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_VONum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_VONum).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_RefNum).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_RefNum).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_Budget).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_Budget).Value

                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_FullDesc).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_FullDesc).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_RefCardCode).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_RefCardCode).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_RefCardName).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_RefCardName).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_ReasonCode).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_ReasonCode).Value
                        oDocumentLines.UserFields.Fields.Item(Datatable.SAP.AR.ORDR_Dtl._U_ReasonDesc).Value = .Item(Datatable.SAP.AR.ORDR_Dtl._U_ReasonDesc).Value
                    End With

Record_MoveNext:
                    oRecSet.MoveNext()
                Loop
            Else
                Throw New BaseException(BaseException.ErrorType.System, "CREATEDOC_0550", "No record was found base on Draft Entry [" & pDocEntry & "]")
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
                        oDocumentExpenses.ExpenseCode = .Item(Datatable.SAP.AR.ORDR_Frg.FreightCode).Value
                        If LocalCurrency = oDocuments.DocCurrency Then
                            oDocumentExpenses.LineTotal = .Item(Datatable.SAP.AR.ORDR_Frg.LC_Total).Value
                        Else
                            oDocumentExpenses.LineTotal = .Item(Datatable.SAP.AR.ORDR_Frg.FC_Total).Value
                        End If
                        oRecSet.MoveNext()
                    Loop
                End If
            End With

            If oDocuments.Add = 0 Then
                Dim NewObjKey As Integer

                NewObjKey = Company.GetNewObjectKey
                MyBase.ToMapping(NewObjKey, 17, pDocEntry)

                Return Company.GetNewObjectKey
            Else
                oLogMessage.AddFailureLine(pDocEntry, Company.GetLastErrorDescription)

                ToErrorTable(pDocEntry, _
                             17, _
                             Company.GetLastErrorCode, _
                             "Unable to Create Sales Order", _
                             Company.GetLastErrorDescription)

                Return 0
            End If
        End Function
        Function CloseDraft(ByVal pDocEntry As Integer) As Integer
            Dim oDraft As New Datatable.SAP.AR.ORDR_Hdr

            oDraft.getDocument(pDocEntry)
            oDraft.DocStatus = "C"
            oDraft.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)
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


