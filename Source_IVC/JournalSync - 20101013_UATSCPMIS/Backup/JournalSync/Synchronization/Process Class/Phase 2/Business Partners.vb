Namespace SyncMainClass
    Class BusinessPartners
        Inherits [Interface].Synchronization

        Private ReadOnly mJobReference As String = "Business Parnter Synchronization (Fiex & SAP)"
        Public Const _SupplierType As String = "S"
        Public Const _CustomerType As String = "C"
        Private ReadOnly _BPType() As String = {_SupplierType, _CustomerType}

        Public Sub New(ByVal pCompany As SAPbobsCOM.Company)
            MyBase.New(pCompany)
            MyBase.JobName = mJobReference
            ObjType = 2

            oLogMessage.FileName = "BusinessPartner"
        End Sub

        Public Overrides Sub Export()

        End Sub

        Public Overrides Sub Import()
            Dim oPNACD As New Datatable.Flex.PNACD
            Dim oDataTable As System.Data.DataTable
            Dim sqlStr As String
            Dim SyncHistory As Datatable.SAP.Sync_History
            Dim oBP_Return As sReturn
            Dim oCardCode As String = ""

            oPNACD.getBusinessPartnerData()
            sqlStr = oPNACD.SelectQuery & " " & oPNACD.filterQuery & " " & oPNACD.OrderByQuery

            oDataTable = oFlexConnection.DataTable(sqlStr)
            WriteDebug("Bussiness Partners: " & sqlStr, mJobReference)
            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        Me.StartTransaction()

                        For Each oBPType As String In _BPType
                            oBP_Return = BP_Mapping(oDataRow, oBPType)
                            oCardCode = oBP_Return.CardCode

                            'Create & Update Module
                            If oBP_Return.Frozen = eFrozen.fn_NO Then
                                If Not Exists_BP(oBP_Return.CardCode) Then
                                    'Create Module
                                    oLogMessage.AddReferenceLine("" & oBP_Return.CardCode & "", " - Create Module")
                                    Create_BP(oBP_Return)

                                    'Log History into Synchronization History table
                                    SyncHistory = New Datatable.SAP.Sync_History
                                    SyncHistory.Add_CreateBP(oBP_Return.CardCode)
                                Else
                                    'Update Module
                                    oLogMessage.AddReferenceLine("" & oBP_Return.CardCode & "", " - Update Module")
                                    Update_BP(oBP_Return)

                                    'Log History into Synchronization History table
                                    SyncHistory = New Datatable.SAP.Sync_History
                                    SyncHistory.Add_UpdateBP(oBP_Return.CardCode)
                                End If
                            End If

                            'Forzen Module
                            If oBP_Return.Frozen = eFrozen.fn_YES Then
                                If Not Exists_BP(oBP_Return.CardCode) Then
                                    Throw New BaseException(BaseException.ErrorType.System, _
                                                            "", _
                                                            "Unable to Delete this record", _
                                                            -9999, _
                                                            "Business Partner Code [" & oBP_Return.CardCode & "] is not found in SAP")
                                Else
                                    'Delete Module
                                    oLogMessage.AddReferenceLine("" & oBP_Return.CardCode & "", " - Frozen Module")
                                    Frozen_BP(oBP_Return)

                                    'Log History into Synchronization History table
                                    SyncHistory = New Datatable.SAP.Sync_History
                                    SyncHistory.Add_FrozenBP(oBP_Return.CardCode)
                                End If
                            End If
                        Next

                        Me.UpdateFlex(oDataRow(Datatable.Flex.PNACD._PNACD_FLEX_EXP_DTE))
                        'End Transaction
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                        oLogMessage.AddSuccessLine(Left(oCardCode, oCardCode.Length - 1), "Operation is Success")
                    Catch b_ex As BaseException
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(oCardCode, b_ex.toString)
                    Catch ex As Exception
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(oCardCode, ex.ToString)
                    End Try
                Next
            End If
        End Sub

        Structure sReturn
            Dim Company As String
            Dim Category As eTransType
            Dim BPType As eBPType
            Dim CardCode As String
            Dim CardName As String
            Dim Address1 As String
            Dim Address2 As String
            Dim CntctName As String
            Dim PhoneNumber As String
            Dim FaxNumber As String
            Dim Remark1 As String
            Dim Remark2 As String
            Dim Remark3 As String
            Dim Remark4 As String
            Dim Frozen As eFrozen
        End Structure

        Enum eTransType As Integer
            bt_Client = 1
            bt_Project = 3
        End Enum

        Enum eBPType As Integer
            bt_Customer = 1
            bt_Supplier = 2
        End Enum

        Enum eFrozen As Integer
            fn_YES = 0
            fn_NO = 1
        End Enum

#Region "Master Data Flow in SAP system"
        Function BP_Mapping(ByVal pDataRow As System.Data.DataRow, ByVal pBPType As String) As sReturn
            Dim BP_Return As sReturn

            BP_Return = New sReturn
            BP_Return.CardCode = pDataRow(Datatable.Flex.PNACD._PNACD_ANA_CDE).ToString.Trim & pBPType
            BP_Return.CardName = pDataRow(Datatable.Flex.PNACD._PNACD_DES).ToString.Trim

            If pDataRow(Datatable.Flex.PNACD._PNACD_ANA_CAT).ToString.Trim = "1" Then
                BP_Return.Category = eTransType.bt_Client
            Else
                BP_Return.Category = eTransType.bt_Project
            End If

            If pBPType = _SupplierType Then
                BP_Return.BPType = eBPType.bt_Supplier
            ElseIf pBPType = _CustomerType Then
                BP_Return.BPType = eBPType.bt_Customer
            End If

            BP_Return.Address1 = pDataRow(Datatable.Flex.PNACD._PNACD_ADDR1).ToString.Trim
            BP_Return.Address2 = pDataRow(Datatable.Flex.PNACD._PNACD_ADDR2).ToString.Trim

            'BP_Return.CntctName = pDataRow(Datatable.Flex.PNACD._PNACD_CON_PSN).ToString.Trim
            BP_Return.PhoneNumber = pDataRow(Datatable.Flex.PNACD._PNACD_PHO_NUM).ToString.Trim
            BP_Return.FaxNumber = pDataRow(Datatable.Flex.PNACD._PNACD_FAX_NUM).ToString.Trim

            BP_Return.Remark1 = pDataRow(Datatable.Flex.PNACD._PNACD_RMK1).ToString.Trim
            BP_Return.Remark2 = pDataRow(Datatable.Flex.PNACD._PNACD_RMK2).ToString.Trim
            BP_Return.Remark3 = pDataRow(Datatable.Flex.PNACD._PNACD_RMK3).ToString.Trim
            BP_Return.Remark4 = pDataRow(Datatable.Flex.PNACD._PNACD_RMK4).ToString.Trim

            If pDataRow(Datatable.Flex.PNACD._PNACD_FRO_STAT).ToString.Trim = "Y" Then
                BP_Return.Frozen = eFrozen.fn_YES
            Else
                BP_Return.Frozen = eFrozen.fn_NO
            End If

            Return BP_Return
        End Function

        Function Create_BP(ByVal pPNACD As sReturn)
            Dim oBusinessPartner As SAPbobsCOM.BusinessPartners
            Dim oBPAddress As SAPbobsCOM.BPAddresses
            Dim oBPContract As SAPbobsCOM.ContactEmployees
            Dim CheckPoint, CheckMessage As String
            Dim oProcessStatus As Integer = -1


            CheckPoint = ""
            CheckMessage = ""
            Try
                CheckPoint = "CREATEBP_001"
                CheckMessage = "Assign Header Information by DI"
                'Business Partner - Header data assign
                oBusinessPartner = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                oBusinessPartner.CardCode = pPNACD.CardCode
                oBusinessPartner.CardName = pPNACD.CardName
                oBusinessPartner.CardForeignName = pPNACD.CntctName
                oBusinessPartner.Currency = "##"

                Select Case pPNACD.BPType
                    Case eBPType.bt_Customer
                        oBusinessPartner.CardType = SAPbobsCOM.BoCardTypes.cCustomer
                    Case eBPType.bt_Supplier
                        oBusinessPartner.CardType = SAPbobsCOM.BoCardTypes.cSupplier
                End Select

                oBusinessPartner.Phone1 = pPNACD.PhoneNumber
                oBusinessPartner.Fax = pPNACD.FaxNumber

                If pPNACD.BPType = eBPType.bt_Supplier Then
                    'Set property "Material Supplier" 
                    oBusinessPartner.Properties(1) = SAPbobsCOM.BoYesNoEnum.tYES
                    'Set property "Sub Contractor"
                    oBusinessPartner.Properties(2) = SAPbobsCOM.BoYesNoEnum.tYES
                End If

                CheckPoint = "CREATEBP_002"
                CheckMessage = "Assign Address Information by DI"
                'Business Parnter - Address data assign
                oBPAddress = oBusinessPartner.Addresses

                'Bill To Address
                oBPAddress.AddressName = pPNACD.CardName & " (B)"
                oBPAddress.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo
                oBPAddress.Street = pPNACD.Address1
                oBPAddress.Block = pPNACD.Address2

                'Ship To Address
                oBPAddress.Add()
                oBPAddress.AddressName = pPNACD.CardName & " (S)"
                oBPAddress.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo
                oBPAddress.Street = pPNACD.Address1
                oBPAddress.Block = pPNACD.Address2



                CheckPoint = "CREATEBP_003"
                CheckMessage = "Assign Contact Information by DI"
                'Business Partner - Contact Person
                'oBPContract = oBusinessPartner.ContactEmployees
                'oBPContract.Name = pPNACD.CntctName
                'oBPContract.Phone1 = pPNACD.PhoneNumber
                'oBPContract.Fax = pPNACD.FaxNumber

                CheckPoint = "CREATEBP_004"
                CheckMessage = "Add new BP Record to SAP by DI"
                oProcessStatus = oBusinessPartner.Add
            Catch ex As Exception
                Throw New BaseException(BaseException.ErrorType.System, _
                                        CheckPoint, _
                                        CheckMessage, _
                                        -1, _
                                        ex.ToString)
            End Try

            If oProcessStatus = 0 Then
                Return pPNACD.CardCode
            Else
                Throw New BaseException(BaseException.ErrorType.SAP, _
                                        "", _
                                        "", _
                                        Company.GetLastErrorCode, _
                                        Company.GetLastErrorDescription)
            End If
        End Function

        Function Update_BP(ByVal pPNACD As sReturn) As String
            Dim oBusinessPartner As SAPbobsCOM.BusinessPartners
            Dim oBPAddress As SAPbobsCOM.BPAddresses
            Dim oBPContract As SAPbobsCOM.ContactEmployees
            Dim CheckPoint, CheckMessage As String
            Dim oProcessStatus As Integer = -1

            CheckPoint = ""
            CheckMessage = ""
            Try
                CheckPoint = "UPDATEBP_001"
                CheckMessage = "Assign Header Information by DI"
                'Business Partner - Header data assign
                oBusinessPartner = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                oBusinessPartner.GetByKey(pPNACD.CardCode)
                oBusinessPartner.CardName = pPNACD.CardName
                oBusinessPartner.CardForeignName = pPNACD.CntctName
                'Karrson: Add
                If pPNACD.Frozen = eFrozen.fn_YES Then
                    oBusinessPartner.Frozen = eFrozen.fn_YES
                    oBusinessPartner.Valid = SAPbobsCOM.BoYesNoEnum.tNO
                Else
                    oBusinessPartner.Valid = SAPbobsCOM.BoYesNoEnum.tYES
                    oBusinessPartner.Frozen = SAPbobsCOM.BoYesNoEnum.tNO
                End If
                'oBusinessPartner.Frozen = pPNACD.Frozen
                'oBusinessPartner.ValidFrom = 
                'oBusinessPartner.ValidTo = String.Empty

                Select Case pPNACD.BPType
                    Case eBPType.bt_Customer
                        oBusinessPartner.CardType = SAPbobsCOM.BoCardTypes.cCustomer
                    Case eBPType.bt_Supplier
                        oBusinessPartner.CardType = SAPbobsCOM.BoCardTypes.cSupplier
                End Select

                oBusinessPartner.Phone1 = pPNACD.PhoneNumber
                oBusinessPartner.Fax = pPNACD.FaxNumber

                CheckPoint = "UPDATEBP_002"
                CheckMessage = "Assign Address Information by DI"
                'Business Parnter - Address data assign
                oBPAddress = oBusinessPartner.Addresses

                'Bill To Address
                oBPAddress.SetCurrentLine(0)
                oBPAddress.AddressName = pPNACD.CardName & " (B)"
                oBPAddress.AddressType = SAPbobsCOM.BoAddressType.bo_BillTo
                oBPAddress.Street = pPNACD.Address1
                oBPAddress.Block = pPNACD.Address2

                'Ship To Address
                Try
                    oBPAddress.SetCurrentLine(1)
                    oBPAddress.AddressName = pPNACD.CardName & " (S)"
                    oBPAddress.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo
                    oBPAddress.Street = pPNACD.Address1
                    oBPAddress.Block = pPNACD.Address2
                Catch ex As Exception
                    oBPAddress.Add()
                    oBPAddress.AddressName = pPNACD.CardName & " (S)"
                    oBPAddress.AddressType = SAPbobsCOM.BoAddressType.bo_ShipTo
                    oBPAddress.Street = pPNACD.Address1
                    oBPAddress.Block = pPNACD.Address2
                End Try

                CheckPoint = "UPDATEBP_003"
                CheckMessage = "Assign Contact Information by DI"
                'Business Partner - Contact Person
                'oBPContract = oBusinessPartner.ContactEmployees
                'oBPContract.SetCurrentLine(0)
                'oBPContract.Name = pPNACD.CntctName
                'oBPContract.Phone1 = pPNACD.PhoneNumber
                'oBPContract.Fax = pPNACD.FaxNumber

                CheckPoint = "UPDATEBP_004"
                CheckMessage = "Add new BP Record to SAP by DI"
                oProcessStatus = oBusinessPartner.Update
            Catch ex As Exception
                Throw New BaseException(BaseException.ErrorType.System, _
                                        CheckPoint, _
                                        CheckMessage, _
                                        -1, _
                                        ex.ToString)
            End Try

            If oProcessStatus = 0 Then
                Return pPNACD.CardCode
            Else
                Throw New BaseException(BaseException.ErrorType.SAP, _
                                        "", _
                                        "", _
                                        Company.GetLastErrorCode, _
                                        Company.GetLastErrorDescription)
            End If
        End Function

        Function Delete_BP(ByVal pPNACD As sReturn)
            Dim oBusinessPartner As SAPbobsCOM.BusinessPartners
            Dim CheckPoint, CheckMessage As String
            Dim oProcessStatus As Integer = -1

            CheckPoint = ""
            CheckMessage = ""
            Try
                CheckPoint = "DELETEBP_001"
                CheckMessage = "Assign Header Information by DI"
                'Business Partner - Header data assign
                oBusinessPartner = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                oBusinessPartner.GetByKey(pPNACD.CardCode)

                CheckPoint = "DELETEBP_003"
                CheckMessage = "Delete new BP Record to SAP by DI"
                oProcessStatus = oBusinessPartner.Remove
            Catch ex As Exception
                Throw New BaseException(BaseException.ErrorType.System, _
                                        CheckPoint, _
                                        CheckMessage, _
                                        -1, _
                                        ex.ToString)
            End Try

            If oProcessStatus = 0 Then
                Return pPNACD.CardCode
            Else
                Throw New BaseException(BaseException.ErrorType.SAP, _
                                        "", _
                                        "", _
                                        Company.GetLastErrorCode, _
                                        Company.GetLastErrorDescription)
            End If
        End Function

        Function Frozen_BP(ByVal pPNACD As sReturn)
            Dim oBusinessPartner As SAPbobsCOM.BusinessPartners
            Dim CheckPoint, CheckMessage As String
            Dim oProcessStatus As Integer = -1

            CheckPoint = ""
            CheckMessage = ""
            Try
                CheckPoint = "FROZENBP_001"
                CheckMessage = "Assign Header Information by DI"
                'Business Partner - Header data assign
                oBusinessPartner = Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.oBusinessPartners)
                oBusinessPartner.GetByKey(pPNACD.CardCode)

                oBusinessPartner.Frozen = SAPbobsCOM.BoYesNoEnum.tYES
                oBusinessPartner.Valid = SAPbobsCOM.BoYesNoEnum.tNO

                CheckPoint = "FROZENBP_003"
                CheckMessage = "Add new BP Record to SAP by DI"
                oProcessStatus = oBusinessPartner.Update
            Catch ex As Exception
                Throw New BaseException(BaseException.ErrorType.System, _
                                        CheckPoint, _
                                        CheckMessage, _
                                        -1, _
                                        ex.ToString)
            End Try

            If oProcessStatus = 0 Then
                Return pPNACD.CardCode
            Else
                Throw New BaseException(BaseException.ErrorType.SAP, _
                                        "", _
                                        "", _
                                        Company.GetLastErrorCode, _
                                        Company.GetLastErrorDescription)
            End If
        End Function

        Function UpdateFlex(ByVal pExportDate As Date) As Integer
            Dim oPNACD As Datatable.Flex.PNACD

            oPNACD = New Datatable.Flex.PNACD
            oPNACD.getProcessEntry(Format(pExportDate, "yyyyMMdd HH:mm:ss") & "." & Right("000" & CStr(pExportDate.Millisecond).Trim, 3))

            'oPNACD.PCMS_Remark = "Operation is Success"
            oPNACD.PCMS_Status = "C"
            oPNACD.PCMS_UpdateDate = Now

            oFlexConnection.Process(oPNACD.UpdateQuery & " " & oPNACD.filterQuery)
        End Function
#End Region

#Region "Business Parnter Data Checking"
        Function Exists_BP(ByVal pCardCode As String) As Boolean
            Dim oRecSet As SAPbobsCOM.Recordset
            Dim oSqlStr As String

            oSqlStr = "Select 1 From OCRD Where CardCode = '" & pCardCode & "'"
            oRecSet = commonRecordSet.execute(oSqlStr)

            If oRecSet.RecordCount = 0 Then
                Return False
            Else
                Return True
            End If
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