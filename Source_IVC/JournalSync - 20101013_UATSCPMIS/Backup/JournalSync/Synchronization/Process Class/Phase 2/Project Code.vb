Namespace SyncMainClass
    Class Project_Code
        Inherits [Interface].Synchronization

        Private ReadOnly mJobReference As String = "Project Code Synchronization (Fiex & SAP)"

        Public Sub New(ByVal pCompany As SAPbobsCOM.Company)
            MyBase.New(pCompany)
            MyBase.JobName = mJobReference
            ObjType = 999

            oLogMessage.FileName = "ProjectCode"
        End Sub

        Public Overrides Sub Export()

        End Sub

        Public Overrides Sub Import()
            Dim oPNACD As New Datatable.Flex.PNACD
            Dim oDataTable As System.Data.DataTable
            Dim sqlStr As String
            Dim SyncHistory As Datatable.SAP.Sync_History
            Dim oPJ_Return As sReturn
            Dim oPrjCode As String = ""

            oPNACD.getProjectData()
            sqlStr = oPNACD.SelectQuery & " " & oPNACD.filterQuery

            oDataTable = oFlexConnection.DataTable(sqlStr)

            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        oPJ_Return = PJ_Mapping(oDataRow)
                        oPrjCode = oPJ_Return.PrjCode

                        Me.StartTransaction()

                        'Create & Update Module
                        If oPJ_Return.Frozen = eFrozen.fn_NO Then
                            If Not Exists_PJ(oPJ_Return.PrjCode) Then
                                'Create Module
                                oLogMessage.AddReferenceLine("" & oPJ_Return.PrjCode & "", " - Create Module")
                                Create_PJ(oPJ_Return)

                                'Log History into Synchronization History table
                                SyncHistory = New Datatable.SAP.Sync_History
                                SyncHistory.Add_CreatePJ(oPJ_Return.PrjCode)
                            Else
                                'Update Module
                                oLogMessage.AddReferenceLine("" & oPJ_Return.PrjCode & "", " - Update Module")
                                Update_PJ(oPJ_Return)

                                'Log History into Synchronization History table
                                SyncHistory = New Datatable.SAP.Sync_History
                                SyncHistory.Add_UpdatePJ(oPJ_Return.PrjCode)
                            End If
                        End If

                        'Forzen Module
                        If oPJ_Return.Frozen = eFrozen.fn_YES Then
                            If Not Exists_PJ(oPJ_Return.PrjCode) Then
                                Throw New BaseException(BaseException.ErrorType.System, _
                                                      "", _
                                                      "Unable to Delete this record", _
                                                      -9999, _
                                                      "Project Code [" & oPJ_Return.PrjCode & "] is not found in SAP")
                            Else
                                'Delete Module
                                oLogMessage.AddReferenceLine("" & oPJ_Return.PrjCode & "", " - Frozen Module")
                                Frozen_PJ(oPJ_Return)

                                'Log History into Synchronization History table
                                SyncHistory = New Datatable.SAP.Sync_History
                                SyncHistory.Add_FrozenPJ(oPJ_Return.PrjCode)
                            End If
                        End If

                        Me.UpdateFlex(oDataRow(Datatable.Flex.PNACD._PNACD_FLEX_EXP_DTE))

                        'End Transaction
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                        oLogMessage.AddSuccessLine(oPrjCode, "Operation is Success")
                    Catch b_ex As BaseException
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(oPrjCode, b_ex.toString)
                    Catch ex As Exception
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(oPrjCode, ex.ToString)
                    End Try
                Next
            End If
        End Sub

        Structure sReturn
            Dim Company As String
            Dim Category As eBPType
            Dim PrjCode As String
            Dim PrjName As String
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
            Dim LegCde As String
        End Structure

        Enum eBPType As Integer
            bt_Client = 1
            bt_Project = 3
        End Enum

        Enum eFrozen As Integer
            fn_YES = 0
            fn_NO = 1
        End Enum

#Region "Project Code Process in SAP system"

        Function PJ_Mapping(ByVal pDataRow As System.Data.DataRow) As sReturn
            Dim PJ_Return As sReturn

            PJ_Return = New sReturn
            PJ_Return.PrjCode = pDataRow(Datatable.Flex.PNACD._PNACD_ANA_CDE).ToString.Trim
            PJ_Return.PrjName = pDataRow(Datatable.Flex.PNACD._PNACD_DES).ToString.Trim

            If pDataRow(Datatable.Flex.PNACD._PNACD_ANA_CAT).ToString.Trim = "1" Then
                PJ_Return.Category = eBPType.bt_Client
            Else
                PJ_Return.Category = eBPType.bt_Project
            End If

            PJ_Return.Address1 = pDataRow(Datatable.Flex.PNACD._PNACD_ADDR1).ToString.Trim
            PJ_Return.Address2 = pDataRow(Datatable.Flex.PNACD._PNACD_ADDR2).ToString.Trim

            PJ_Return.CntctName = pDataRow(Datatable.Flex.PNACD._PNACD_CON_PSN).ToString.Trim
            PJ_Return.PhoneNumber = pDataRow(Datatable.Flex.PNACD._PNACD_PHO_NUM).ToString.Trim
            PJ_Return.FaxNumber = pDataRow(Datatable.Flex.PNACD._PNACD_FAX_NUM).ToString.Trim

            PJ_Return.Remark1 = pDataRow(Datatable.Flex.PNACD._PNACD_RMK1).ToString.Trim
            PJ_Return.Remark2 = pDataRow(Datatable.Flex.PNACD._PNACD_RMK2).ToString.Trim
            PJ_Return.Remark3 = pDataRow(Datatable.Flex.PNACD._PNACD_RMK3).ToString.Trim
            PJ_Return.Remark4 = pDataRow(Datatable.Flex.PNACD._PNACD_RMK4).ToString.Trim

            If pDataRow(Datatable.Flex.PNACD._PNACD_FRO_STAT).ToString.Trim = "Y" Then
                PJ_Return.Frozen = eFrozen.fn_YES
            Else
                PJ_Return.Frozen = eFrozen.fn_NO
            End If

            PJ_Return.LegCde = pDataRow(Datatable.Flex.PNACD._PNACD_LEGCDE).ToString.Trim

            Return PJ_Return
        End Function

        Sub Create_PJ(ByVal pPNACD As sReturn)
            Dim oOPRJ As Datatable.SAP.Admin.OPRJ

            oOPRJ = New Datatable.SAP.Admin.OPRJ
            oOPRJ.PrjCode = pPNACD.PrjCode
            oOPRJ.PrjName = pPNACD.PrjCode
            oOPRJ.KeyIndex = oOPRJ.getKeyIndex

            If pPNACD.PrjName = "" Then
                oOPRJ.PrjFullName = pPNACD.PrjCode
            Else
                oOPRJ.PrjFullName = pPNACD.PrjName
            End If

            oOPRJ.DocStatus = "P"
            oOPRJ.LegCde = pPNACD.LegCde

            oOPRJ.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
        End Sub

        Sub Update_PJ(ByVal pPNACD As sReturn)
            Dim oOPRJ As Datatable.SAP.Admin.OPRJ

            oOPRJ = New Datatable.SAP.Admin.OPRJ
            oOPRJ.getProjectEntry(pPNACD.PrjCode)

            oOPRJ.PrjName = pPNACD.PrjCode

            If pPNACD.PrjName = "" Then
                oOPRJ.PrjFullName = pPNACD.PrjCode
            Else
                oOPRJ.PrjFullName = pPNACD.PrjName
            End If

            oOPRJ.DocStatus = "P"
            oOPRJ.LegCde = pPNACD.LegCde

            oOPRJ.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)
        End Sub

        Sub Delete_PJ(ByVal pPNACD As sReturn)
            Dim oOPRJ As Datatable.SAP.Admin.OPRJ

            oOPRJ = New Datatable.SAP.Admin.OPRJ
            oOPRJ.getProjectEntry(pPNACD.PrjCode)

            'Avoid truncate error, 20170523, begin
            'oOPRJ.PrjName = pPNACD.PrjName
            oOPRJ.PrjName = pPNACD.PrjCode
            'Avoid truncate error, 20170523, end

            oOPRJ.DocStatus = "PRBD"

            oOPRJ.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)
        End Sub

        Sub Frozen_PJ(ByVal pPNACD As sReturn)
            Dim oOPRJ As Datatable.SAP.Admin.OPRJ

            oOPRJ = New Datatable.SAP.Admin.OPRJ
            oOPRJ.getProjectEntry(pPNACD.PrjCode)

            'Avoid truncate error, 20170523, begin
            'oOPRJ.PrjName = pPNACD.PrjName
            oOPRJ.PrjName = pPNACD.PrjCode
            'Avoid truncate error, 20170523, end

            oOPRJ.DocStatus = "PRBD"

            oOPRJ.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)
        End Sub

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

#Region "Project Code Checking"

        Function Exists_PJ(ByVal pPrjCode As String) As Boolean
            Dim oRecSet As SAPbobsCOM.Recordset
            Dim oSqlStr As String

            oSqlStr = "Select 1 From OPRJ Where PrjCode = '" & pPrjCode & "'"
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
