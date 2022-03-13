Imports SAPbobsCOM

Namespace SyncMainClass
    Class IncomingPayment
        Inherits [Interface].Synchronization

        Private ReadOnly mJobReference As String = "Payment Synchronization (Fiex & SAP)"
        Private LocalCurrency As String

        Public Sub New(ByVal pCompany As Company)
            MyBase.New(pCompany)
            oLogMessage.FileName = "AR-Payment"
            ObjType = 24

            Dim sboBob As SBObob = pCompany.GetBusinessObject(BoObjectTypes.BoBridge)
            LocalCurrency = ""
            LocalCurrency = Trim(sboBob.GetLocalCurrency.Fields.Item(0).Value)
        End Sub

        Public Overrides Sub Export()

        End Sub

        Public Overrides Sub Import()
            Dim oPOSTL As Datatable.Flex.POSTL
            Dim oCPSPay As Datatable.SAP.PCMS.CPSPAY, oCPSApa As Datatable.SAP.PCMS.CPSAPA
            Dim oDataTable As System.Data.DataTable
            Dim sqlStr As String
            Dim SyncHistory As Datatable.SAP.Sync_History
            Dim oDocNum As String = ""
            Dim Comp_CDE, Acc_CDE, StlDocNum, AppDocNum As String

            oPOSTL = New Datatable.Flex.POSTL
            oPOSTL.getPayment()
            sqlStr = oPOSTL.SelectQuery & " " & oPOSTL.filterQuery & " " & oPOSTL.OrderByQuery

            oDataTable = oFlexConnection.DataTable(sqlStr)
            StlDocNum = ""
            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        Me.StartTransaction()

                        Comp_CDE = oDataRow(Datatable.Flex.POSTL._POSTL_COMP_CDE)
                        Acc_CDE = oDataRow(Datatable.Flex.POSTL._POSTL_ACC_CDE)
                        StlDocNum = oDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_NUM)
                        AppDocNum = oDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_NUM)

                        oCPSPay = Me.Normal_Table(oDataRow)
                        oCPSApa = Me.HistoryTable(oDataRow)

                        SyncHistory = New Datatable.SAP.Sync_History

                        If Me.getExistsPayment(Comp_CDE, Acc_CDE, StlDocNum, AppDocNum) Then
                            oCPSPay.getPayment(Comp_CDE, Acc_CDE, StlDocNum, AppDocNum)

                            oCPSPay.Process(CPS.SQL.Interface.RecordSet.Status.stt_UPDATE)
                            SyncHistory.Add_UpdatePY(StlDocNum)
                        Else
                            oCPSPay.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
                            SyncHistory.Add_CreatePY(StlDocNum)
                        End If
                        oCPSApa.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)

                        Me.UpdateFlex(oDataRow(Datatable.Flex.POSTL._POSTL_FLEX_EXP_DTE))

                        'End Transaction
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Commit)

                        oLogMessage.AddSuccessLine(StlDocNum, "Operation is Success")
                    Catch b_ex As BaseException
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(StlDocNum, b_ex.toString)
                    Catch ex As Exception
                        Me.EndTransaction(FlexConnection.TransStatus.ts_Rollback)
                        oLogMessage.AddExceptionSkip(StlDocNum, ex.ToString)
                    End Try
                Next
            End If
        End Sub

#Region "Get Filter Information Structure"
        Public Function getExistsPayment(ByVal pComCDE As String, _
                                         ByVal pAccCDE As String, _
                                         ByVal pStlDocNum As String, _
                                         ByVal pAppDocNum As String)
            Dim oCPSPAY As New Datatable.SAP.PCMS.CPSPAY
            
            oCPSPAY.getPayment(pComCDE, pAccCDE, pStlDocNum, pAppDocNum)
            If oCPSPAY.Execute.RecordCount = 0 Then
                Return False
            Else
                Return True
            End If
        End Function
#End Region

#Region "Payment Flow in SAP system"
        Function HistoryTable(ByVal pDataRow As System.Data.DataRow) As Datatable.SAP.PCMS.CPSAPA
            Dim oCPSPay As New Datatable.SAP.PCMS.CPSAPA

            oCPSPay.COMP_CDE = pDataRow(Datatable.Flex.POSTL._POSTL_COMP_CDE).ToString.Trim
            oCPSPay.ACC_CDE = pDataRow(Datatable.Flex.POSTL._POSTL_ACC_CDE).ToString.Trim
            oCPSPay.STL_DOC_NUM = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_NUM).ToString.Trim
            oCPSPay.STL_DOC_DTE = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_DTE).ToString.Trim
            oCPSPay.STL_DOC_TYP = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_TYP)
            oCPSPay.STL_DOC_ANA1 = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_ANA1).ToString.Trim
            oCPSPay.STL_DOC_ANA2 = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_ANA2).ToString.Trim
            oCPSPay.STL_DOC_ANA3 = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_ANA3).ToString.Trim
            oCPSPay.STL_DOC_ANA4 = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_ANA4).ToString.Trim
            oCPSPay.STL_DOC_ANA5 = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_ANA5).ToString.Trim
            oCPSPay.APP_DOC_NUM = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_NUM).ToString.Trim
            oCPSPay.STL_AMT = pDataRow(Datatable.Flex.POSTL._POSTL_STL_AMT)
            oCPSPay.APP_DOC_ANA1 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA1).ToString.Trim
            oCPSPay.APP_DOC_ANA2 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA2)
            oCPSPay.APP_DOC_ANA3 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA3).ToString.Trim
            oCPSPay.APP_DOC_ANA4 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA4).ToString.Trim
            oCPSPay.APP_DOC_ANA5 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA5).ToString.Trim
            oCPSPay.STL_AMT = pDataRow(Datatable.Flex.POSTL._POSTL_STL_AMT)
            oCPSPay.APP_DOC_ANA1 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA1).ToString.Trim
            oCPSPay.APP_DOC_ANA2 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA2).ToString.Trim
            oCPSPay.APP_DOC_ANA3 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA3).ToString.Trim
            oCPSPay.APP_DOC_ANA4 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA4).ToString.Trim
            oCPSPay.APP_DOC_ANA5 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA5).ToString.Trim
            oCPSPay.CCY_CDE = pDataRow(Datatable.Flex.POSTL._POSTL_CCY_CDE)
            oCPSPay.EXC_RAT = pDataRow(Datatable.Flex.POSTL._POSTL_EXC_RAT)
            oCPSPay.STL_DOC_VOU = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_VOU)
            oCPSPay.APP_DOC_VOU = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_VOU)
            oCPSPay.FLEX_EXP_DTE = pDataRow(Datatable.Flex.POSTL._POSTL_FLEX_EXP_DTE)

            Return oCPSPay
        End Function
        Function Normal_Table(ByVal pDataRow As System.Data.DataRow) As Datatable.SAP.PCMS.CPSPAY
            Dim oCPSPay As New Datatable.SAP.PCMS.CPSPAY

            oCPSPay.COMP_CDE = pDataRow(Datatable.Flex.POSTL._POSTL_COMP_CDE)
            oCPSPay.ACC_CDE = pDataRow(Datatable.Flex.POSTL._POSTL_ACC_CDE)
            oCPSPay.STL_DOC_NUM = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_NUM)
            oCPSPay.STL_DOC_DTE = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_DTE)
            oCPSPay.STL_DOC_TYP = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_TYP)
            oCPSPay.STL_DOC_ANA1 = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_ANA1)
            oCPSPay.STL_DOC_ANA2 = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_ANA2)
            oCPSPay.STL_DOC_ANA3 = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_ANA3)
            oCPSPay.STL_DOC_ANA4 = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_ANA4)
            oCPSPay.STL_DOC_ANA5 = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_ANA5)
            oCPSPay.APP_DOC_NUM = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_NUM)
            oCPSPay.STL_AMT = pDataRow(Datatable.Flex.POSTL._POSTL_STL_AMT)
            oCPSPay.APP_DOC_ANA1 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA1)
            oCPSPay.APP_DOC_ANA2 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA2)
            oCPSPay.APP_DOC_ANA3 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA3)
            oCPSPay.APP_DOC_ANA4 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA4)
            oCPSPay.APP_DOC_ANA5 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA5)
            oCPSPay.STL_AMT = pDataRow(Datatable.Flex.POSTL._POSTL_STL_AMT)
            oCPSPay.APP_DOC_ANA1 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA1)
            oCPSPay.APP_DOC_ANA2 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA2)
            oCPSPay.APP_DOC_ANA3 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA3)
            oCPSPay.APP_DOC_ANA4 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA4)
            oCPSPay.APP_DOC_ANA5 = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_ANA5)
            oCPSPay.CCY_CDE = pDataRow(Datatable.Flex.POSTL._POSTL_CCY_CDE)
            oCPSPay.EXC_RAT = pDataRow(Datatable.Flex.POSTL._POSTL_EXC_RAT)
            oCPSPay.STL_DOC_VOU = pDataRow(Datatable.Flex.POSTL._POSTL_STL_DOC_VOU)
            oCPSPay.APP_DOC_VOU = pDataRow(Datatable.Flex.POSTL._POSTL_APP_DOC_VOU)
            oCPSPay.FLEX_EXP_DTE = pDataRow(Datatable.Flex.POSTL._POSTL_FLEX_EXP_DTE)

            Return oCPSPay
        End Function

        Function UpdateFlex(ByVal pExportDate As Date) As Integer
            Dim oPOSTL As Datatable.Flex.POSTL

            oPOSTL = New Datatable.Flex.POSTL
            oPOSTL.getPayment(Format(pExportDate, "yyyyMMdd HH:mm:ss") & "." & Right("000" & CStr(pExportDate.Millisecond).Trim, 3))

            'oPOSTL.PCMS_Remark = "Operation is Success"
            oPOSTL.PCMS_Status = "C"
            oPOSTL.PCMS_UpdateDate = Now

            oFlexConnection.Process(oPOSTL.UpdateQuery & " " & oPOSTL.filterQuery)
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