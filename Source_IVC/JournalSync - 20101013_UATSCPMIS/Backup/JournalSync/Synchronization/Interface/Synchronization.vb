Imports CPS.CPSLIB.Debug
Namespace [Interface]
    Public MustInherit Class Synchronization
        Public oLogMessage As CPS.Global.Setting.LogMessage
        Private mCompany As SAPbobsCOM.Company
        Private mRecSet As SAPbobsCOM.Recordset
        Private Shared isSubLedger As Boolean, isContraCharge As Boolean
        Public oFlexConnection As FlexConnection
        Public mObjType As Integer
        Public MustOverride Property ObjType() As Integer

        Public Sub New(ByVal pCompany As SAPbobsCOM.Company)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)

            oLogMessage = New CPS.Global.Setting.LogMessage(pCompany)
            mCompany = pCompany
            mRecSet = pCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Me.Argument = JournalSync.Argument
            oFlexConnection = New FlexConnection
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Sub

        Public Sub New(ByVal pCompany As SAPbobsCOM.Company, _
                       ByVal pFlexConnection As FlexConnection)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            oLogMessage = New CPS.Global.Setting.LogMessage(pCompany)
            mCompany = pCompany
            mRecSet = pCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Me.Argument = JournalSync.Argument
            oFlexConnection = pFlexConnection
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Sub

        Public ReadOnly Property Company() As SAPbobsCOM.Company
            Get
                Return mCompany
            End Get
        End Property

        Public WriteOnly Property JobName() As String
            Set(ByVal value As String)
                oLogMessage.JobName = value
            End Set
        End Property

        Public WriteOnly Property Argument() As String
            Set(ByVal value As String)
                oLogMessage.Argument = value
            End Set
        End Property

        Public MustOverride Sub Export()
        Public MustOverride Sub Import()

        Public Sub ToMapping(ByVal pDocentry As Integer, ByVal pObjType As Integer, ByVal pDraftKey As Integer)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oMapping As Datatable.SAP.Draft_Mapping

            oMapping = New Datatable.SAP.Draft_Mapping
            oMapping.DocumentKey = pDocentry
            oMapping.ObjectType = pObjType
            oMapping.DraftEntry = pDraftKey

            oMapping.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Sub

        Public Sub ToFlex(ByVal pImport As ImportFLEX)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            'Karrson: Debug

            Dim oTransitory As Datatable.Flex.PTVOU
            Dim sqlStr As String



            oTransitory = New Datatable.Flex.PTVOU
            'Karrson: Debug
            WriteDebug("CompanyCode")
            WriteDebug(Convert.ToString(pImport.CompanyCode))
            oTransitory.CompanyCode = pImport.CompanyCode
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_COM_CDE, pImport.CompanyCode)
            'Karrson: Debug
            WriteDebug("pImport.DocumentNo")
            WriteDebug(Convert.ToString(pImport.DocumentNo))
            oTransitory.RefNO = pImport.DocumentNo.ToUpper
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_REF_NUM, pImport.DocumentNo)

            'Karrson: Debug
            WriteDebug("pImport.LineNo")
            WriteDebug(Convert.ToString(pImport.LineNo))
            oTransitory.LineNo = pImport.LineNo
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_LIN_NUM, pImport.LineNo)

            'Karrson: Debug
            WriteDebug("pImport.BatchID")
            WriteDebug(Convert.ToString(pImport.BatchID))
            oTransitory.BatchID = pImport.BatchID
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_BCH_ID, pImport.BatchID)

            'Karrson: Debug
            WriteDebug("pImport.VoucherType")
            WriteDebug(Convert.ToString(pImport.VoucherType))
            oTransitory.VoucherType = pImport.VoucherType
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_VOU_TYP, pImport.VoucherType)

            'Karrson: Debug
            WriteDebug(" pImport.VoucherDate")
            WriteDebug(Convert.ToString(pImport.VoucherDate))
            oTransitory.VoucherDate = pImport.VoucherDate
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_VOU_DTE, pImport.VoucherDate)

            'Karrson: Debug
            WriteDebug("pImport.Description")
            WriteDebug(Convert.ToString(pImport.Description))
            oTransitory.Description = pImport.Description
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_DES, pImport.Description)

            'Karrson: Debug
            WriteDebug("pImport.AcctCode")
            WriteDebug(Convert.ToString(pImport.AcctCode))
            oTransitory.AcctCode = pImport.AcctCode
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ACC_CDE, pImport.AcctCode)

            'Karrson: Change To Upper Case 
            'oTransitory.AnalysisCode1 = pImport.AnalysisCode1
            'oTransitory.AnalysisCode2 = pImport.AnalysisCode2
            'oTransitory.AnalysisCode3 = pImport.AnalysisCode3
            'oTransitory.AnalysisCode4 = pImport.AnalysisCode4
            'oTransitory.AnalysisCode5 = pImport.AnalysisCode5
            WriteDebug(Convert.ToString(pImport.AnalysisCode1))
            WriteDebug(Convert.ToString(pImport.AnalysisCode2))
            WriteDebug(Convert.ToString(pImport.AnalysisCode3))
            WriteDebug(Convert.ToString(pImport.AnalysisCode4))
            WriteDebug(Convert.ToString(pImport.AnalysisCode5))
            WriteDebug("AnalysisCode1")
            If Not pImport.AnalysisCode1 Is Nothing Then
                oTransitory.AnalysisCode1 = pImport.AnalysisCode1.ToUpper
                'Karrson: SetInsertQuery
                oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ANA_CDE1, pImport.AnalysisCode1)
            End If
            WriteDebug("AnalysisCode2")
            If Not pImport.AnalysisCode2 Is Nothing Then
                oTransitory.AnalysisCode2 = pImport.AnalysisCode2
                'Karrson: SetInsertQuery
                oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ANA_CDE2, pImport.AnalysisCode2)
            End If

            WriteDebug("AnalysisCode3")
            If Not pImport.AnalysisCode3 Is Nothing Then
                oTransitory.AnalysisCode3 = pImport.AnalysisCode3.ToUpper
                'Karrson: SetInsertQuery
                oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ANA_CDE3, pImport.AnalysisCode3.ToUpper)
            End If
            WriteDebug("AnalysisCode4")
            If Not pImport.AnalysisCode4 Is Nothing Then
                oTransitory.AnalysisCode4 = pImport.AnalysisCode4.ToUpper
                'Karrson: SetInsertQuery
                oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ANA_CDE4, pImport.AnalysisCode4.ToUpper)
            End If
            WriteDebug("AnalysisCode5")
            If Not pImport.AnalysisCode5 Is Nothing Then
                oTransitory.AnalysisCode5 = pImport.AnalysisCode5.ToUpper
                'Karrson: SetInsertQuery
                oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ANA_CDE5, pImport.AnalysisCode5.ToUpper)
            End If

            'oTransitory.AnalysisCode1 = pImport.AnalysisCode1.ToUpper
            'oTransitory.AnalysisCode2 = pImport.AnalysisCode2
            'oTransitory.AnalysisCode3 = pImport.AnalysisCode3.ToUpper
            'oTransitory.AnalysisCode4 = pImport.AnalysisCode4.ToUpper
            'oTransitory.AnalysisCode5 = pImport.AnalysisCode5.ToUpper

            'Karrson: Debug
            WriteDebug("pImport.DocumentNo")
            WriteDebug(Convert.ToString(pImport.DocumentNo))

            'Testing only
            'Dim bch_id As String
            'bch_id = pImport.BatchID.Substring(0, 1).ToUpper()

            'If bch_id = "C" Or bch_id = "D" Then
            'oTransitory.DocumentNo = pImport.DocumentNo.ToUpper & "_" & pImport.RevNo
            'Else
            oTransitory.DocumentNo = pImport.DocumentNo.ToUpper
            'End If
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_DOC_NUM, pImport.DocumentNo)

            'Karrson: Debug
            WriteDebug("pImport.AltDocNumber")
            WriteDebug(pImport.AltDocNumber)

            Try


                If pImport.AltDocNumber.Trim.Length > 20 Then
                    WriteDebug("Length greater than 20 chars")
                    oTransitory.AltDocument = Left(pImport.AltDocNumber, 20)
                    WriteDebug(pImport.AltDocNumber.Substring(0, 20))
                    WriteDebug(Left(pImport.AltDocNumber, 20))
                    oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ALT_DOC_NUM, Left(pImport.AltDocNumber, 20))
                Else
                    WriteDebug("Length smaller or equal than 20 chars")
                    oTransitory.AltDocument = pImport.AltDocNumber

                    oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ALT_DOC_NUM, pImport.AltDocNumber)
                End If

            Catch ex As Exception
                oTransitory.AltDocument = String.Empty
                oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ALT_DOC_NUM, String.Empty)
            End Try

            'Karrson: Debug
            WriteDebug("pImport.DocType")
            WriteDebug(Convert.ToString(pImport.DocType))

            oTransitory.DocumentType = pImport.DocType
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_DOC_TYP, pImport.DocType)

            'Karrson: Debug
            WriteDebug("pImport.DocDate")
            WriteDebug(Convert.ToString(pImport.DocDate))
            oTransitory.DocumentDate = pImport.DocDate
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_DOC_DTE, pImport.DocDate)

            'Karrson: Debug
            WriteDebug("pImport.DocDueDate")
            WriteDebug(Convert.ToString(pImport.DocDueDate))
            oTransitory.DocumentDueDate = pImport.DocDueDate
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_DOC_DUE_DTE, pImport.DocDueDate)

            'Karrson: Debug
            WriteDebug("pImport.Currency")
            WriteDebug(Convert.ToString(pImport.Currency))
            oTransitory.Currency = pImport.Currency
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_CCY_CDE, pImport.Currency)

            'Karrson: Debug
            WriteDebug("pImport.Amount")
            WriteDebug(Convert.ToString(pImport.Amount))

            If pImport.Amount >= 0 Then
                oTransitory.Allocation = pImport.Allocation
                'Karrson: SetInsertQuery
                oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_D_C, pImport.Allocation)

                oTransitory.Amount = pImport.Amount
                'Karrson: SetInsertQuery
                oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_AMT, pImport.Amount)

                oTransitory.EquvAmuont = pImport.EquvAmount
                'Karrson: SetInsertQuery
                oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_AMT_BAS, pImport.EquvAmount)
            Else
                Select Case pImport.Allocation
                    Case "D"
                        oTransitory.Allocation = "C"
                        'Karrson: SetInsertQuery
                        oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_D_C, "C")
                    Case "C"
                        oTransitory.Allocation = "D"
                        'Karrson: SetInsertQuery
                        oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_D_C, "D")
                End Select
                oTransitory.Amount = pImport.Amount * -1
                'Karrson: SetInsertQuery
                oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_AMT, pImport.Amount * -1)

                oTransitory.EquvAmuont = pImport.EquvAmount * -1
                'Karrson: SetInsertQuery
                oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_AMT_BAS, pImport.EquvAmount * -1)
            End If

            oTransitory.ExchangeRate = pImport.ExchangeRate
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_EXC_RAT, pImport.ExchangeRate)

            oTransitory.PaymentTerm = pImport.PaymentTerm
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_DOC_PAY_TRM, pImport.PaymentTerm)

            oTransitory.Quantity = pImport.Quantity
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_QTY, pImport.Quantity)

            oTransitory.Unit = pImport.Unit
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_UNI, pImport.Unit)

            WriteDebug("Particular")
            WriteDebug(Convert.ToString(pImport.Particular1))
            WriteDebug(Convert.ToString(pImport.Particular2))
            oTransitory.Particular1 = pImport.Particular1
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_DES1, pImport.Particular1)

            oTransitory.Particular2 = pImport.Particular2
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_DES2, pImport.Particular2)

            WriteDebug("ExtendedAnalysis")
            WriteDebug(Convert.ToString(pImport.ExtendedAnalysis1))
            WriteDebug(Convert.ToString(pImport.ExtendedAnalysis2))
            WriteDebug(Convert.ToString(pImport.ExtendedAnalysis3))
            WriteDebug(Convert.ToString(pImport.ExtendedAnalysis4))
            WriteDebug(Convert.ToString(pImport.ExtendedAnalysis5))
            WriteDebug(Convert.ToString(pImport.ExtendedAnalysis6))
            WriteDebug(Convert.ToString(pImport.ExtendedAnalysis7))
            WriteDebug(Convert.ToString(pImport.ExtendedAnalysis8))
            WriteDebug(Convert.ToString(pImport.ExtendedAnalysis9))
            WriteDebug(Convert.ToString(pImport.ExtendedAnalysis10))

            oTransitory.ExtendedAnalysis1 = pImport.ExtendedAnalysis1
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ANA_CDE01, pImport.ExtendedAnalysis1)

            oTransitory.ExtendedAnalysis2 = pImport.ExtendedAnalysis2
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ANA_CDE02, pImport.ExtendedAnalysis2)

            oTransitory.ExtendedAnalysis3 = pImport.ExtendedAnalysis3
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ANA_CDE03, pImport.ExtendedAnalysis3)
            oTransitory.ExtendedAnalysis4 = pImport.ExtendedAnalysis4
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ANA_CDE04, pImport.ExtendedAnalysis4)

            oTransitory.ExtendedAnalysis5 = pImport.ExtendedAnalysis5
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ANA_CDE05, pImport.ExtendedAnalysis5)

            oTransitory.ExtendedAnalysis6 = pImport.ExtendedAnalysis6
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ANA_CDE06, pImport.ExtendedAnalysis6)
            oTransitory.ExtendedAnalysis7 = pImport.ExtendedAnalysis7
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ANA_CDE07, pImport.ExtendedAnalysis7)
            oTransitory.ExtendedAnalysis8 = pImport.ExtendedAnalysis8
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ANA_CDE08, pImport.ExtendedAnalysis8)
            oTransitory.ExtendedAnalysis9 = pImport.ExtendedAnalysis9
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ANA_CDE09, pImport.ExtendedAnalysis9)
            oTransitory.ExtendedAnalysis10 = pImport.ExtendedAnalysis10
            'Karrson: SetInsertQuery
            oTransitory.setField(Datatable.Flex.PTVOU.PTVOU_ANA_CDE10, pImport.ExtendedAnalysis10)



            sqlStr = oTransitory.InsertQuery
            'Karrson: Manual Insert Query
            Dim _sqlStr As String = oTransitory.Insert_Query()

            'Karrson Debug
            WriteDebug("Insert Query: " & sqlStr)
            WriteDebug("Manual Insert Query: " & _sqlStr)
            WriteDebug("Flex Connection: " & (oFlexConnection Is Nothing))
            'Karrson: if Sql Fail then execute manual Query
            Try
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & "To Flex Insert Query: " & sqlStr, TimeSet.Status.Start)
                oFlexConnection.Process(sqlStr)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & "To Flex Insert Query: " & sqlStr, TimeSet.Status.Finish)
            Catch ex As Exception
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & "To Flex Insert Query: " & _sqlStr, TimeSet.Status.Start)
                oFlexConnection.Process(_sqlStr)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & "To Flex Insert Query: " & _sqlStr, TimeSet.Status.Finish)
            End Try


            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Sub

        Public Sub ToErrorTable(ByVal pKeyIndex As String, _
                                ByVal pObjType As String, _
                                ByVal pErrorCode As String, _
                                ByVal pErrorDesc As String, _
                                ByVal pExceptDes As String)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Try
                Dim oCPSFSE As Datatable.SAP.PCMS.CPSFSE

                oCPSFSE = New Datatable.SAP.PCMS.CPSFSE
                oCPSFSE.DraftKey = pKeyIndex
                oCPSFSE.ObjType = pObjType
                oCPSFSE.LogInstanc = oCPSFSE.keyIndex
                oCPSFSE.ErrorCode = Left(pErrorCode, 50)
                oCPSFSE.ErrorDesc = Left(pErrorDesc, 254)
                oCPSFSE.Exception = Left(pExceptDes, 2000)

                oCPSFSE.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
            Catch ex As Exception
                oLogMessage.AddExceptionSkip("ToError", ex.ToString)
            End Try
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Sub

        Sub INSERT_CPSFIN(ByVal pBatchID As String, ByVal pRevTrans As String, ByVal pDocEntry As Long)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oDataTable As System.Data.DataTable
            Dim oPIVOU As Datatable.Flex.PIVOU
            Dim oCPSFIN As Datatable.SAP.PCMS.CPSFIN
            Dim oRecSet As FlexConnection

            oPIVOU = New Datatable.Flex.PIVOU
            oPIVOU.getByBatchID(pBatchID)

            oRecSet = New FlexConnection
            oDataTable = oRecSet.DataTable(oPIVOU.SelectQuery & " " & oPIVOU.filterQuery)

            For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                oCPSFIN = New Datatable.SAP.PCMS.CPSFIN
                oCPSFIN.COM_CDE = oDataRow(Datatable.Flex.PIVOU._PIVOU_COM_CDE)
                oCPSFIN.REF_NUM = oDataRow(Datatable.Flex.PIVOU._PIVOU_REF_NUM)
                oCPSFIN.LIN_NUM = oDataRow(Datatable.Flex.PIVOU._PIVOU_LIN_NUM)

                'Add by Michael, begin
                oCPSFIN.REVTRANS = pRevTrans
                oCPSFIN.DOCENTRY = pDocEntry
                'Add by Michael, end

                oCPSFIN.BCH_ID = oDataRow(Datatable.Flex.PIVOU._PIVOU_BCH_ID)
                oCPSFIN.VOU_TYP = oDataRow(Datatable.Flex.PIVOU._PIVOU_VOU_TYP)
                oCPSFIN.VOU_DTE = oDataRow(Datatable.Flex.PIVOU._PIVOU_VOU_DTE)
                oCPSFIN.DES = oDataRow(Datatable.Flex.PIVOU._PIVOU_DES)
                oCPSFIN.ACC_CDE = oDataRow(Datatable.Flex.PIVOU._PIVOU_ACC_CDE)
                oCPSFIN.ANA_CDE1 = oDataRow(Datatable.Flex.PIVOU._PIVOU_ANA_CDE01)
                oCPSFIN.ANA_CDE2 = oDataRow(Datatable.Flex.PIVOU._PIVOU_ANA_CDE02)
                oCPSFIN.ANA_CDE3 = oDataRow(Datatable.Flex.PIVOU._PIVOU_ANA_CDE03)
                oCPSFIN.ANA_CDE4 = oDataRow(Datatable.Flex.PIVOU._PIVOU_ANA_CDE04)
                oCPSFIN.ANA_CDE5 = oDataRow(Datatable.Flex.PIVOU._PIVOU_ANA_CDE05)
                oCPSFIN.DOC_NUM = oDataRow(Datatable.Flex.PIVOU._PIVOU_DOC_NUM)
                oCPSFIN.ALT_DOC_NUM = oDataRow(Datatable.Flex.PIVOU._PIVOU_ALT_DOC_NUM)
                oCPSFIN.DOC_TYP = oDataRow(Datatable.Flex.PIVOU._PIVOU_DOC_TYP)
                oCPSFIN.DOC_DTE = oDataRow(Datatable.Flex.PIVOU._PIVOU_DOC_DTE)
                oCPSFIN.DOC_DUE_DTE = oDataRow(Datatable.Flex.PIVOU._PIVOU_DOC_DUE_DTE)
                oCPSFIN.CCY_CDE = oDataRow(Datatable.Flex.PIVOU._PIVOU_CCY_CDE)
                oCPSFIN.AMT = oDataRow(Datatable.Flex.PIVOU._PIVOU_AMT)
                oCPSFIN.EXC_RAT = oDataRow(Datatable.Flex.PIVOU._PIVOU_EXC_RAT)
                oCPSFIN.AMT_BAS = oDataRow(Datatable.Flex.PIVOU._PIVOU_AMT_BAS)
                oCPSFIN.D_C = oDataRow(Datatable.Flex.PIVOU._PIVOU_D_C)
                oCPSFIN.DOC_PAY_TRM = oDataRow(Datatable.Flex.PIVOU._PIVOU_DOC_PAY_TRM)
                oCPSFIN.QTY = oDataRow(Datatable.Flex.PIVOU._PIVOU_QTY)
                oCPSFIN.UNI = oDataRow(Datatable.Flex.PIVOU._PIVOU_UNI)
                oCPSFIN.DES1 = oDataRow(Datatable.Flex.PIVOU._PIVOU_DES1)
                oCPSFIN.DES2 = oDataRow(Datatable.Flex.PIVOU._PIVOU_DES2)
                oCPSFIN.ANA_CDE01 = oDataRow(Datatable.Flex.PIVOU._PIVOU_ANA_CDE01)
                oCPSFIN.ANA_CDE02 = oDataRow(Datatable.Flex.PIVOU._PIVOU_ANA_CDE02)
                oCPSFIN.ANA_CDE03 = oDataRow(Datatable.Flex.PIVOU._PIVOU_ANA_CDE03)
                oCPSFIN.ANA_CDE04 = oDataRow(Datatable.Flex.PIVOU._PIVOU_ANA_CDE04)
                oCPSFIN.ANA_CDE05 = oDataRow(Datatable.Flex.PIVOU._PIVOU_ANA_CDE05)
                oCPSFIN.ANA_CDE06 = oDataRow(Datatable.Flex.PIVOU._PIVOU_ANA_CDE06)
                oCPSFIN.ANA_CDE07 = oDataRow(Datatable.Flex.PIVOU._PIVOU_ANA_CDE07)
                oCPSFIN.ANA_CDE08 = oDataRow(Datatable.Flex.PIVOU._PIVOU_ANA_CDE08)
                oCPSFIN.ANA_CDE09 = oDataRow(Datatable.Flex.PIVOU._PIVOU_ANA_CDE09)
                oCPSFIN.ANA_CDE10 = oDataRow(Datatable.Flex.PIVOU._PIVOU_ANA_CDE10)
                oCPSFIN.RMK = oDataRow(Datatable.Flex.PIVOU._PIVOU_RMK)
                oCPSFIN.FLX_BCH_ID = oDataRow(Datatable.Flex.PIVOU._PIVOU_FLX_BCH_ID)
                oCPSFIN.FLX_VOU_NUM = oDataRow(Datatable.Flex.PIVOU._PIVOU_FLX_VOU_NUM)
                oCPSFIN.FLX_STA = oDataRow(Datatable.Flex.PIVOU._PIVOU_FLX_STA)
                oCPSFIN.FLX_UPD_DTE = oDataRow(Datatable.Flex.PIVOU._PIVOU_FLX_UPD_DTE)

                oCPSFIN.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
            Next
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            oRecSet.Disconnect()
        End Sub

        Public Structure ExportPCMS

            Dim SubsidiaryCode As String
            Dim RefNo As Integer
            Dim LineNo As Integer
            ''' <summary>
            ''' C######### = Client Payment Certificate Data
            ''' M######### = Supplier Payment Certificate Data
            ''' S######### = Sub-Contractor Payment Certificate
            ''' CM######### = AP Credit Memo Data
            ''' </summary>
            ''' <remarks></remarks>
            Dim CertEntry As String
            Dim CertNumber As String
            Const VoucherType As String = "GJ"
            Dim ValuationDate As Date
            Dim ObjType As Integer
            'Const Description As String = "General Journal"
            Const Description As String = "GENERAL JOURNAL"
            Dim AcctCode As String
            ''' <summary>
            ''' Customer / Vendor Code in SAP
            ''' </summary>
            ''' <remarks></remarks>
            Dim BPCode As String
            ''' <summary>
            ''' Project code
            ''' </summary>
            ''' <remarks></remarks>
            Dim ProjectCode As String
            ''' <summary>
            ''' Client Payment Certificate Data: Nothing
            ''' Supplier Payment Certificate Data: Nothing
            ''' Sub-Contractor Payment Certificate: Last 6 digits of Sub-Contractor No.
            ''' </summary>
            ''' <remarks></remarks>
            Dim SubCon_No As String
            ''' <summary>
            ''' Client Payment Certificate Data: Nothing 
            ''' Supplier Payment Certificate Data: Cost Code
            ''' Sub-Contractor Payment Certificate: Cost Code
            ''' </summary>
            ''' <remarks></remarks>
            Dim CostCode As String
            Dim VInvoiceNo As String
            Const DocType As String = "O"
            Dim CertDate As Date
            Dim CertDueDate As Date
            Dim DocCurrency As String
            Dim TotalBefDis As Double
            Dim DocRate As String
            Dim DocTotal As Double
            ''' <summary>
            ''' D for Debit, C for Credit
            ''' </summary>
            ''' <remarks></remarks>
            Dim Allocation As String
            Dim CertData As String
            Dim RefCardCode As String

            '''Added by Ken, 20181102
            Dim RevNo As String
        End Structure

        Public Structure ImportFLEX
            Dim CompanyCode As String
            Dim RefNo As Integer
            Dim LineNo As Integer
            Dim BatchID As String
            Dim VoucherType As String
            Dim VoucherDate As Date
            Dim Description As String
            Dim AcctCode As String
            Dim AnalysisCode1 As String
            Dim AnalysisCode2 As String
            Dim AnalysisCode3 As String
            Dim _AnalysisCode4 As String
            Public Property AnalysisCode4() As String
                Get
                    Return _AnalysisCode4
                End Get
                Set(ByVal value As String)
                    If isContraCharge Then
                        _AnalysisCode4 = value
                    Else
                        _AnalysisCode4 = Nothing
                    End If
                End Set
            End Property
            Dim _AnalysisCode5 As String
            Public Property AnalysisCode5() As String
                Get
                    Return _AnalysisCode5
                End Get
                Set(ByVal value As String)
                    If isSubLedger Then
                        _AnalysisCode5 = Nothing
                    Else
                        _AnalysisCode5 = value
                    End If
                End Set
            End Property
            Dim _DocumentNo As String
            Public Property DocumentNo() As String
                Get
                    Return _DocumentNo
                End Get
                Set(ByVal value As String)
                    If Not value.IndexOf("/") = value.LastIndexOf("/") Then
                        _DocumentNo = value.Remove(value.LastIndexOf("/"), 1)
                    Else
                        _DocumentNo = value
                    End If
                End Set
            End Property
            Dim AltDocNumber As String
            Dim DocType As String
            Dim DocDate As Date
            Dim DocDueDate As Date
            Dim Currency As String
            Dim Amount As Double
            Dim ExchangeRate As Double
            Dim EquvAmount As Double
            Dim Allocation As String
            Dim PaymentTerm As String
            Dim Quantity As Integer
            Dim Unit As String
            Dim Particular1 As String
            Dim Particular2 As String
            Dim ExtendedAnalysis1 As String
            Dim ExtendedAnalysis2 As String
            Dim ExtendedAnalysis3 As String
            Dim ExtendedAnalysis4 As String
            Dim ExtendedAnalysis5 As String
            Dim ExtendedAnalysis6 As String
            Dim ExtendedAnalysis7 As String
            Dim ExtendedAnalysis8 As String
            Dim ExtendedAnalysis9 As String
            Dim ExtendedAnalysis10 As String

            '''Added by Ken, 20181102
            Dim RevNo As String

        End Structure

        Public Sub StartTransaction()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            'Begin transaction in PCMS Server
            'Karrson Debug
            WriteDebug("Setup SAP Transaction")
            WriteDebug("Setup StartTransaction:" & currentCompany.InTransaction)
            If Not currentCompany.InTransaction Then
                WriteDebug("Start SAP Transaction")
                currentCompany.StartTransaction()
            End If
            WriteDebug("Setup Flex Transaction")
            'Begin transaction in FLEX Server
            If Not oFlexConnection Is Nothing Then
                If Not oFlexConnection.InTransaction Then
                    WriteDebug("Start Flex Transaction")
                    oFlexConnection.StartTransaction()
                End If
            End If
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)

        End Sub

        Public Sub EndTransaction(ByVal pStatus As FlexConnection.TransStatus)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Select Case pStatus
                Case FlexConnection.TransStatus.ts_Commit
                    'Commit this transaction in PCMS Server
                    If currentCompany.InTransaction Then
                        currentCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                    End If

                    'Commit this transaction in FLEX Server
                    If Not oFlexConnection Is Nothing Then
                        If oFlexConnection.InTransaction Then
                            oFlexConnection.EndTransaction(FlexConnection.TransStatus.ts_Commit)
                        End If
                    End If

                Case FlexConnection.TransStatus.ts_Rollback
                    'Roll Back this transaction in PCMS Server
                    If currentCompany.InTransaction Then
                        currentCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
                    End If

                    'Roll Back this transaction in FLEX Server
                    If Not oFlexConnection Is Nothing Then
                        If oFlexConnection.InTransaction Then
                            oFlexConnection.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        End If
                    End If
            End Select
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Sub

        ''' <summary>
        ''' MapAcctCode
        ''' Map AcctCode to OACT.AccntntCod
        ''' </summary>
        ''' <param name="pAcctCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function MapAcctCode(ByVal pAcctCode As String) As String
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim sqlStr As String
            Dim oRecSet As SAPbobsCOM.Recordset
            MapAcctCode = pAcctCode



            isSubLedger = False
            isContraCharge = False

            sqlStr = "Select AccntntCod From OACT Where AcctCode = '" & pAcctCode.Trim & "'"
            oRecSet = commonRecordSet.execute(sqlStr)

            If oRecSet.RecordCount = 0 Then
                'No record found base on this Account Code

            Else
                MapAcctCode = oRecSet.Fields.Item("AccntntCod").Value

            End If
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)

        End Function

        ''' <summary>
        ''' Account Code mapping from Flex to SAP
        ''' If SAP Account Code = 'CC' then 
        ''' return [External Code] + Left(Customer Code, 5)
        ''' Else
        ''' return [External Code]
        ''' End If
        ''' </summary>
        ''' <param name="pAcctCode">SAP Account Code</param>
        ''' <returns>Flex Account Code</returns>
        ''' <remarks></remarks>
        Function ExAcctcode(ByVal pAcctCode As String, _
                            ByVal pExportPCMS As ExportPCMS) As String
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim sqlStr As String
            Dim oRecSet As SAPbobsCOM.Recordset
            Dim CostType As String
            Dim Prefix As String
            Dim oContractChge_Code As String

            Dim oCardCode As String, _
                oRefCardCode As String, _
                oPCMSDocNum As String, _
                oProjectCode As String

            oCardCode = pExportPCMS.BPCode
            oRefCardCode = pExportPCMS.RefCardCode
            oPCMSDocNum = pExportPCMS.CertNumber
            oProjectCode = pExportPCMS.ProjectCode


            isSubLedger = False
            isContraCharge = False

            sqlStr = "Select U_CostType, U_FlexAcctPrefix, AccntntCod From OACT Where AcctCode = '" & pAcctCode & "'"
            oRecSet = commonRecordSet.execute(sqlStr)

            If oRecSet.RecordCount = 0 Then
                'No record found base on this Account Code
                CostType = ""
                Prefix = ""
            Else
                CostType = oRecSet.Fields.Item("U_CostType").Value
                Prefix = oRecSet.Fields.Item("U_FlexAcctPrefix").Value
            End If

            If Not CostType = Datatable.SAP.ControlAccount.CT_ContraCharge Then
                isSubLedger = True
            Else
                isContraCharge = True
            End If

            Select Case CostType.Trim
                Case Datatable.SAP.ControlAccount.CT_ContraCharge

                    'Parameter: 1. Project Code, 2. Project Code / Contract Charge Account value after (.) CC.SC001 ==> SC001

                    oLogMessage.AddReferenceLine("ExAcctCode", "Get SubConBQ Info (Project Code: " & oProjectCode & ", Account Code: " & pAcctCode & ")")
                    oContractChge_Code = oProjectCode.Trim & "/" & pAcctCode.Split(".")(1).Trim
                    'sqlStr = "Select CardCode From OPOR Where U_PCMSDocNum = '" & oContractChge_Code & "'"
                    sqlStr = "Select CardCode From OPOR Where DocType='S' and  CANCELED='N' and U_PCMSDocNum = '" & oContractChge_Code & "'"
                    oRecSet = commonRecordSet.execute(sqlStr)

                    oCardCode = oRecSet.Fields.Item("CardCode").Value
                    Return Prefix & Left(oCardCode, 5)

                Case Datatable.SAP.ControlAccount.CT_RetentionAR
                    Return Prefix & Left(oCardCode, 5)

                Case Datatable.SAP.ControlAccount.CT_RetentionAP
                    Return Prefix & Left(oCardCode, 5)

                Case Datatable.SAP.ControlAccount.CT_Trade_AP
                    Return Prefix & Left(oCardCode, 5)

                Case Datatable.SAP.ControlAccount.AC_Trade_AR
                    Return Prefix & Left(oCardCode, 5)

                Case Else
                    isSubLedger = False
                    Return oRecSet.Fields.Item("AccntntCod").Value

            End Select
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Function

        'Logic Change at 2009-09-29
        'Function ExAcctcode(ByVal pAcctCode As String, ByVal pCardCode As String) As String
        '    Dim sqlStr As String
        '    Dim oRecSet As SAPbobsCOM.Recordset
        '    Dim ExternalCode As String

        '    sqlStr = "Select AccntntCod From OACT Where AcctCode = '" & pAcctCode & "'"
        '    oRecSet = commonRecordSet.execute(sqlStr)

        '    If oRecSet.RecordCount = 0 Then
        '        'No record found base on this Account Code
        '        ExternalCode = ""
        '    Else
        '        ExternalCode = oRecSet.Fields.Item("AccntntCod").Value
        '    End If

        '    If Left(pAcctCode.Trim, 2).ToUpper = "CC" Then
        '        Return ExternalCode.Trim & Left(pCardCode.Trim, 5)
        '    Else
        '        Return ExternalCode
        '    End If
        'End Function

        Shared Function GetAnalysisCode5(ByVal pAcctCode As String) As String
            WriteDebug(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Start")
            Dim oRecSet As SAPbobsCOM.Recordset
            Dim sqlStr As String = "Select U_FlexAna5 From OACT Where AcctCode = '" & pAcctCode & "'"

            oRecSet = commonRecordSet.execute(sqlStr)
            WriteDebug(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " End")
            Return oRecSet.Fields.Item("U_FlexAna5").Value

        End Function

        ''' <summary>
        ''' Sub Ledger record for flex account 
        ''' </summary>
        ''' <param name="pCardCode"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Function ControlAcct(ByVal pCardCode As String) As String
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim sqlStr As String
            Dim oRecSet As SAPbobsCOM.Recordset

            sqlStr = ""
            sqlStr &= "	Select OCRD.DebPayAcct From OCRD	" & vbCrLf
            sqlStr &= "	Where	" & vbCrLf
            sqlStr &= "	CardCode = '" & pCardCode & "'	" & vbCrLf

            oRecSet = commonRecordSet.execute(sqlStr)
            If oRecSet.RecordCount = 0 Then
                'No record found base on this Account Code
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
                Return ""
            Else
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
                Return oRecSet.Fields.Item("DebPayAcct").Value
            End If
        End Function

        Enum Sync_Object As Integer
            so_BusinessPartner = 2
            so_ItemMaster = 4
            so_SalesOrder = 17
            so_SalesInvoice = 13
            so_PurchaseOrder = 22
            so_PurchaseInvoice = 18
        End Enum

        Function setDraftToAPDocument(ByVal pDocEntry As Integer) As Integer
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            WriteDebug("setDraftToAPDocument: " & pDocEntry)
            Dim oDrafts As SAPbobsCOM.Documents
            Dim oDocuments As SAPbobsCOM.Documents
            Dim oXMLKey As String = Guid.NewGuid.ToString.Trim

            Company.XmlExportType = SAPbobsCOM.BoXmlExportTypes.xet_ExportImportMode
            Company.XMLAsString = Nothing

            oDrafts = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts)
            oDrafts.GetByKey(pDocEntry)
            oDrafts.UserFields.Fields.Item("U_CertStatus").Value = "A"
            WriteDebug("SaveXML Path: " & get_TempPath() & "\" & oXMLKey & ".xml")
            oDrafts.SaveXML(get_TempPath() & "\" & oXMLKey & ".xml")
            AdjustXML_File("18", oXMLKey)

            oDocuments = Company.GetBusinessObjectFromXML(get_TempPath() & "\" & oXMLKey & ".xml", 0)

            ' Debug: Check Quantity
            WriteDebug("Validate Quantity")
            'For i As Integer = 0 To oDocuments.Lines.Count - 1
            '    oDocuments.Lines.SetCurrentLine(i)
            '    WriteDebug(String.Format("PCMSNo: {2} Line Number {0}: Quantity: {1}", i, oDocuments.Lines.Quantity, oDocuments.UserFields.Fields.Item("U_PCMSDocNum").Value), "A/P Invoice")
            'Next

            If oDocuments.Add = 0 Then
                WriteDebug("setDraftToAPDocument: Success")
                Dim NewObjKey As Integer

                NewObjKey = Company.GetNewObjectKey
                WriteDebug("NewObjKey: " & NewObjKey)
                ToMapping(NewObjKey, 18, pDocEntry)

                Update_ApprovalStatus_Y(pDocEntry)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
                Return NewObjKey
            Else
                WriteDebug("setDraftToAPDocument: Failure")
                oLogMessage.AddFailureLine(pDocEntry, Company.GetLastErrorDescription)

                ToErrorTable(pDocEntry, _
                             112, _
                             Company.GetLastErrorCode, _
                             "Unable to Approve A/P Item Invoice", _
                             Company.GetLastErrorDescription)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            End If

        End Function

        Function setDraftToAPCMDocument(ByVal pDocEntry As Integer) As Integer
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            WriteDebug("setDraftToAPCMDocument: " & pDocEntry)
            Dim oDrafts As SAPbobsCOM.Documents
            Dim oDocuments As SAPbobsCOM.Documents
            Dim oXMLKey As String = Guid.NewGuid.ToString.Trim

            Company.XmlExportType = SAPbobsCOM.BoXmlExportTypes.xet_ExportImportMode
            Company.XMLAsString = Nothing

            oDrafts = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oDrafts)
            oDrafts.GetByKey(pDocEntry)
            WriteDebug("SaveXML Path: " & get_TempPath() & "\" & oXMLKey & ".xml")
            oDrafts.SaveXML(get_TempPath() & "\" & oXMLKey & ".xml")
            AdjustXML_File("19", oXMLKey)

            oDocuments = Company.GetBusinessObjectFromXML(get_TempPath() & "\" & oXMLKey & ".xml", 0)

            ' Debug: Check Quantity
            WriteDebug("Validate Quantity")
            'For i As Integer = 0 To oDocuments.Lines.Count - 1
            '    oDocuments.Lines.SetCurrentLine(i)
            '    WriteDebug(String.Format("PCMSNo: {2} Line Number {0}: Quantity: {1}", i, oDocuments.Lines.Quantity, oDocuments.UserFields.Fields.Item("U_PCMSDocNum").Value), "A/P Invoice")
            'Next

            If oDocuments.Add = 0 Then
                WriteDebug("setDraftToAPCMDocument: Success")
                Dim NewObjKey As Integer

                NewObjKey = Company.GetNewObjectKey
                WriteDebug("NewObjKey: " & NewObjKey)
                ToMapping(NewObjKey, 19, pDocEntry)

                Update_CM_ApprovalStatus_Y(pDocEntry)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
                Return NewObjKey
            Else
                WriteDebug("setDraftToAPCMDocument: Failure")
                oLogMessage.AddFailureLine(pDocEntry, Company.GetLastErrorDescription)

                ToErrorTable(pDocEntry, _
                             112, _
                             Company.GetLastErrorCode, _
                             "Unable to Approve AP Credit Memo", _
                             Company.GetLastErrorDescription)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
            End If

        End Function

        Sub AdjustXML_File(ByVal pObjType As String, _
                           ByVal pFilePath As String)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oFileStream As New IO.FileStream(get_TempPath() & "\" & pFilePath & ".xml", _
                                                 IO.FileMode.OpenOrCreate, _
                                                 IO.FileAccess.ReadWrite, _
                                                 IO.FileShare.ReadWrite)
            Dim oStreamWriter As New IO.StreamWriter(oFileStream)
            Dim oStreamReader As New IO.StreamReader(oFileStream)
            Dim oFileContext As String

            oFileContext = oStreamReader.ReadToEnd
            oFileContext = oFileContext.Replace("<Object>112</Object>", "<Object>" & pObjType & "</Object>")

            oFileStream.Position = 0
            oFileStream.SetLength(oFileContext.Length)

            oStreamWriter.Write(oFileContext)
            oStreamWriter.Flush()
            oStreamWriter.Close()
            oFileStream.Close()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Sub

        Sub Update_ApprovalStatus_Y(ByVal pDocEntry As Integer)

            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oHdr_Approve As Datatable.SAP.ApprovalProcedure.Header
            Dim oDtl_Approve As Datatable.SAP.ApprovalProcedure.Detail
            Dim oWDD_Code As String
            Dim oSqlStr As String = "Update OPCH Set DraftKey = '" & pDocEntry & "', WddStatus = 'P' Where DocEntry = '" & Company.GetNewObjectKey & "'"

            oHdr_Approve = New Datatable.SAP.ApprovalProcedure.Header
            oDtl_Approve = New Datatable.SAP.ApprovalProcedure.Detail

            oHdr_Approve.getApprovalByDocEntry(pDocEntry)
            oWDD_Code = oHdr_Approve.Execute().Fields.Item(Datatable.SAP.ApprovalProcedure.Header._WddCode).Value
            oDtl_Approve.getApproval_Transaction(oWDD_Code)

            oHdr_Approve.DocEntry = Company.GetNewObjectKey
            oHdr_Approve.ObjType = "18"
            oHdr_Approve.Status = "Y"
            oHdr_Approve.IsDraft = "N"
            oHdr_Approve.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)

            oDtl_Approve.Status = "Y"
            oDtl_Approve.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)
            WriteDebug("Update_ApprovalStatus_Y, SQL Query: " & oSqlStr)
            commonRecordSet.execute(oSqlStr)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Sub

        Sub Update_CM_ApprovalStatus_Y(ByVal pDocEntry As Integer)

            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oHdr_Approve As Datatable.SAP.ApprovalProcedure.Header
            Dim oDtl_Approve As Datatable.SAP.ApprovalProcedure.Detail
            Dim oWDD_Code As String
            Dim oSqlStr As String = "Update ORPC Set DraftKey = '" & pDocEntry & "', WddStatus = 'P' Where DocEntry = '" & Company.GetNewObjectKey & "'"

            oHdr_Approve = New Datatable.SAP.ApprovalProcedure.Header
            oDtl_Approve = New Datatable.SAP.ApprovalProcedure.Detail

            oHdr_Approve.getApprovalByDocEntry(pDocEntry)
            oWDD_Code = oHdr_Approve.Execute().Fields.Item(Datatable.SAP.ApprovalProcedure.Header._WddCode).Value
            oDtl_Approve.getApproval_Transaction(oWDD_Code)

            oHdr_Approve.DocEntry = Company.GetNewObjectKey
            oHdr_Approve.ObjType = "19"
            oHdr_Approve.Status = "Y"
            oHdr_Approve.IsDraft = "N"
            oHdr_Approve.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)

            oDtl_Approve.Status = "Y"
            oDtl_Approve.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)
            WriteDebug("Update_CM_ApprovalStatus_Y, SQL Query: " & oSqlStr)
            commonRecordSet.execute(oSqlStr)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Sub

        Sub Update_ApprovalStatus_N(ByVal pDocEntry As Integer)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            Dim oHdr_Approve As Datatable.SAP.ApprovalProcedure.Header
            Dim oDtl_Approve As Datatable.SAP.ApprovalProcedure.Detail
            Dim oWDD_Code As String


            oHdr_Approve = New Datatable.SAP.ApprovalProcedure.Header
            oDtl_Approve = New Datatable.SAP.ApprovalProcedure.Detail

            oHdr_Approve.getApprovalByDocEntry(pDocEntry)
            oWDD_Code = oHdr_Approve.Execute().Fields.Item(Datatable.SAP.ApprovalProcedure.Header._WddCode).Value
            oDtl_Approve.getApproval_Transaction(oWDD_Code)

            'oHdr_Approve.DocEntry = Company.GetNewObjectKey
            oHdr_Approve.Status = "N"
            oHdr_Approve.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)

            oDtl_Approve.Status = "N"
            oDtl_Approve.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Sub

        'Function getApproveRemark(ByVal pDocEntry As Integer) As String
        '    Dim sqlStr As String

        '    sqlStr = "Select Remarks from OWDD Where DocEntry = '" & pDocEntry & "' And ObjType = '112' And [Status] = 'W'"

        'End Function

        Function Chk_ErrOverLap(ByVal pTransCode As String, ByVal pObjType As eObjType) As Boolean
            Return False
            'Dim oRecSet As SAPbobsCOM.Recordset
            'Dim sqlStr As String = "Select 1 From CPSFSE Where DraftKey = '" & pTransCode & "' and ObjType = '" & pObjType & "'"

            'oRecSet = commonRecordSet.execute(sqlStr)
            'If oRecSet.RecordCount >= SubMain.RetryCount Then
            '    Return True
            'Else
            '    Return False
            'End If
        End Function

        Enum eObjType As Integer
            ot_SalesInvoice = 13
            ot_PurchaseInvoice = 18
            ot_APCreditMemo = 19
            ot_Draft = 112
            ot_SalesQuotation = 23
            ot_SalesOrder = 17
            ot_PurchaseOrder = 22
        End Enum

    End Class
End Namespace