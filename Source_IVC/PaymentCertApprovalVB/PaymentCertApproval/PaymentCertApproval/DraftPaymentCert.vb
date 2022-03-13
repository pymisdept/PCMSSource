Imports SAPbouiCOM
Imports SAPbobsCOM
Public Class DraftPaymentCert

    Dim oForm As SAPbouiCOM.Form
    Public Const etCardCode As String = "etCardCode"
    Public Const etCardName As String = "etCardName"
    Public Const lbCardName As String = "lbCardName"
    Public Const etPrjCode As String = "etPrjCode"
    Public Const FormType As String = "frmpcapp"
    Public Const uidcbappstatus As String = "cbstatus1"
    Public Const uidcbsetappstatus As String = "13"
    Public Const uidgrdresult As String = "grdresult"
    Public Const uidSubmit As String = "10"
    Public Const uidSearch As String = "btnsearch"
    Public Const uidRemove As String = "btnremove"


    Public Const SQL_PCApprovalStatus As String = "EXEC IVC_SP_UPDATEPCAPPROVALSTATUS '{0}','{1}','{2}'"
    Public Const SQL_GetDraftPC As String = "SELECT * FROM IVC_FUNC_GETDRAFTPC('{0}','{1}','{2}') order by 7 asc"
    Public Const FLD_ColSel As String = "colsel"
    Public Const FLD_CertApproved As String = "CertApproved"
    Public Const FLD_DOCENTRY As String = "DocEntry"
    Public Const FLD_Project As String = "Project"
    Public Const FLD_CardCode As String = "CardCode"
    Public Const FLD_CardName As String = "CardName"
    Public Const FLD_DocTotal As String = "DocTotal"
    Public Const FLD_DocCur As String = "DocCur"
    Public Const FLD_APPStatus As String = "CertStatus"

    Dim cbApprovalStatus As ComboBox
    Dim cbSetApprovalStatus As ComboBox
    Dim grdResult As Grid

    Public Sub New(ByVal oForm As SAPbouiCOM.Form)
        Me.oForm = oForm
        cbApprovalStatus = GetComboBox(uidcbappstatus)
        'cbSetApprovalStatus = GetComboBox(uidcbsetappstatus)

        grdResult = GetGrid(uidgrdresult)

    End Sub

    Public Sub FormLoad()
        oForm.Freeze(True)
        ' User Data Source
        oForm.DataSources.UserDataSources.Add("UDBP", BoDataType.dt_SHORT_TEXT, 20)
        oForm.DataSources.UserDataSources.Add("UDPRJ", BoDataType.dt_SHORT_TEXT, 15)
        oForm.DataSources.UserDataSources.Add("UDBPNAME", BoDataType.dt_SHORT_TEXT, 100)
        ' init
        GetEditText(etCardCode).DataBind.SetBound(True, "", "UDBP")
        GetEditText(etCardCode).ChooseFromListUID = "CFLBP"
        GetEditText(etCardCode).ChooseFromListAlias = "CardCode"


        GetEditText(etCardName).DataBind.SetBound(True, "", "UDBPNAME")
        GetEditText(etCardName).ChooseFromListUID = "CFLBPNAME"
        GetEditText(etCardName).ChooseFromListAlias = "CardName"


        GetEditText(etPrjCode).DataBind.SetBound(True, "", "UDPRJ")
        GetEditText(etPrjCode).ChooseFromListUID = "CFLPRJ"
        GetEditText(etPrjCode).ChooseFromListAlias = "PrjCode"


        ' Assign Approval Status
        cbApprovalStatus.ValidValues.Add("", "ALL")
        cbApprovalStatus.ValidValues.Add("P", "Pending")
        cbApprovalStatus.ValidValues.Add("R", "Reject")
        cbApprovalStatus.Select("P", BoSearchKey.psk_ByValue)

        'cbSetApprovalStatus.ValidValues.Add("A", "Approved")
        'cbSetApprovalStatus.ValidValues.Add("R", "Reject")

        oForm.Freeze(False)

    End Sub

    Public Function UpdateApprovalStatus(ByRef _ErrMsg) As Boolean
        Dim _ret As Boolean = True
        Dim _grd As Grid = grdResult
        Dim _CardCode As String = GetEditText(etCardCode).Value
        Dim _PrjCode As String = GetEditText(etPrjCode).Value
        'Dim _appstatus As String = cbSetApprovalStatus.Selected.Value.ToString()
        Dim _rs As Recordset
        Dim _rs1 As Recordset
        Dim _oCompany As SAPbobsCOM.Company = SBOCompany
        Dim _dt As SAPbouiCOM.DataTable


        Try
            
            If SBOApplication.MessageBox("Confirm submit selected payment cert to Flex System?", 1, "Yes", "No", "") = 1 Then

                

                _rs = _oCompany.GetBusinessObject(BoObjectTypes.BoRecordset)
                _dt = _grd.DataTable
                _oCompany.StartTransaction()
                For i As Integer = 0 To _dt.Rows.Count - 1

                    If _dt.GetValue(FLD_ColSel, i).ToString() = "Y" Then

                        If _dt.GetValue(FLD_APPStatus, i).ToString().ToUpper() = "REJECT" Then
                            Throw New Exception("Rejected Payment Cert cannot submit.")
                        Else
                            _rs1 = _oCompany.GetBusinessObject(BoObjectTypes.BoRecordset)
                            _rs1.DoQuery(String.Format("Exec IVC_SP_TransactionNotification '{0}',{1}", "112", _dt.GetValue(FLD_DOCENTRY, i).ToString()))
                            If (_rs1.EoF = False) Then
                                If _rs1.Fields.Item(0).Value.ToString() = "Y" Then
                                    _rs.DoQuery(String.Format(SQL_PCApprovalStatus, _dt.GetValue(FLD_DOCENTRY, i).ToString(), "A", _oCompany.UserSignature))
                                Else
                                    Throw New Exception(_rs1.Fields.Item(1).Value.ToString())
                                End If

                            Else
                                Throw New Exception("Internal Error")

                            End If
                        End If

                    End If

                Next

                If _oCompany.InTransaction Then
                    _oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                End If
                _rs = Nothing

            End If


        Catch ex As Exception
            _ret = False
            _ErrMsg = ex.Message
            If _oCompany.InTransaction Then
                _oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If

        End Try


        Return _ret
    End Function

    Public Sub ClearFoum()
        GetEditText(etCardCode).Value = ""
        GetEditText(etPrjCode).Value = ""
        GetStaticText(lbCardName).Caption = ""
        'cbApprovalStatus.Select("P")
        grdResult.DataTable.Clear()

    End Sub

    Public Function Remove(ByRef _ErrMsg As String) As Boolean
        Dim _ret As Boolean = True
        Dim _grd As Grid = grdResult
        Dim _CardCode As String = GetEditText(etCardCode).Value
        Dim _PrjCode As String = GetEditText(etPrjCode).Value
        'Dim _appstatus As String = cbSetApprovalStatus.Selected.Value.ToString()
        Dim _rs As Recordset
        Dim _oCompany As SAPbobsCOM.Company = SBOCompany
        Dim _dt As SAPbouiCOM.DataTable
        Dim _oDoc As SAPbobsCOM.Documents

        Try

            If SBOApplication.MessageBox("Confirm remove selected draft payment cert?", 1, "Yes", "No", "") = 1 Then

                

                '_rs = _oCompany.GetBusinessObject(BoObjectTypes.BoRecordset)
                _dt = _grd.DataTable
                _oCompany.StartTransaction()
                For i As Integer = 0 To _dt.Rows.Count - 1

                    If _dt.GetValue(FLD_ColSel, i).ToString() = "Y" Then
                        _oDoc = _oCompany.GetBusinessObject(BoObjectTypes.oDrafts)
                        If _oDoc.GetByKey(_dt.GetValue(FLD_DOCENTRY, i).ToString()) Then
                            If _oDoc.Remove() <> 0 Then
                                Throw New Exception(_oCompany.GetLastErrorDescription())
                            End If
                            
                        End If
                    End If

                Next

                If _oCompany.InTransaction Then
                    _oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_Commit)
                End If
                _rs = Nothing

            End If


        Catch ex As Exception
            _ret = False
            _ErrMsg = ex.Message
            If _oCompany.InTransaction Then
                _oCompany.EndTransaction(SAPbobsCOM.BoWfTransOpt.wf_RollBack)
            End If

        End Try


        Return _ret
    End Function
    Public Function Search(ByRef _ErrMsg As String) As Boolean
        Dim _ret As Boolean = True
        Dim _grd As Grid = grdResult
        Dim _CardCode As String = GetEditText(etCardCode).Value
        Dim _PrjCode As String = GetEditText(etPrjCode).Value
        '        Dim _appstatus As String = cbApprovalStatus.Selected.Value.ToString()
        Try
            oForm.Freeze(True)
            _grd.DataTable.ExecuteQuery(String.Format(SQL_GetDraftPC, _CardCode.Replace("'", "''"), _PrjCode.Replace("'", "''"), cbApprovalStatus.Selected.Value.ToString()))

            If _grd.DataTable.Rows.Count > 0 Then
                CType(oForm.Items.Item("lbTotal").Specific, StaticText).Caption = Convert.ToDecimal(_grd.DataTable.GetValue(9, 0)).ToString("###,###,###.##")
            Else
                CType(oForm.Items.Item("lbTotal").Specific, StaticText).Caption = String.Empty
            End If
            _grd.Columns.Item(0).Type = SAPbouiCOM.BoGridColumnType.gct_CheckBox
            _grd.Columns.Item(0).TitleObject.Caption = ""
            _grd.Columns.Item(1).TitleObject.Caption = "Cert Number"
            _grd.Columns.Item(1).TitleObject.Sortable = True
            _grd.Columns.Item(1).Editable = False

            _grd.Columns.Item(2).TitleObject.Caption = "Cert Status"
            _grd.Columns.Item(2).Editable = False
            _grd.Columns.Item(2).TitleObject.Sortable = True
            _grd.Columns.Item(3).TitleObject.Caption = "Project Code"
            _grd.Columns.Item(3).Editable = False
            _grd.Columns.Item(3).TitleObject.Sortable = True
            _grd.Columns.Item(4).TitleObject.Caption = "Supplier Code"
            _grd.Columns.Item(4).Editable = False
            _grd.Columns.Item(4).TitleObject.Sortable = True
            _grd.Columns.Item(5).TitleObject.Caption = "Supplier Name"
            _grd.Columns.Item(5).Editable = False
            _grd.Columns.Item(5).TitleObject.Sortable = True
            _grd.Columns.Item(6).TitleObject.Caption = "Draft Number"
            _grd.Columns.Item(6).Editable = False
            _grd.Columns.Item(6).TitleObject.Sortable = True

            CType(_grd.Columns.Item(6), EditTextColumn).LinkedObjectType = "112"
            _grd.Columns.Item(7).TitleObject.Caption = "Amount"
            _grd.Columns.Item(7).Editable = False
            _grd.Columns.Item(7).TitleObject.Sortable = True
            _grd.Columns.Item(8).TitleObject.Caption = "Currency"
            _grd.Columns.Item(8).Editable = False
            _grd.Columns.Item(8).TitleObject.Sortable = True

            _grd.Columns.Item(9).Visible = False

        Catch ex As Exception
            _ErrMsg = ex.Message
            _ret = False
        Finally
            oForm.Freeze(False)
        End Try

        Return _ret

    End Function


    Public Function GetObject(ByVal val As String, ByRef obj As EditText) As Boolean
        Dim ret As Boolean = True
        Try
            obj = oForm.Items.Item(val).Specific
        Catch ex As Exception
            ret = False
        End Try
        Return ret
    End Function



    Public Function GetEditText(ByVal val As String) As EditText
        Dim obj As EditText
        Try
            obj = oForm.Items.Item(val).Specific
        Catch ex As Exception

        End Try
        Return obj
    End Function

    Public Function GetComboBox(ByVal val As String) As ComboBox
        Dim obj As ComboBox
        Try

            obj = oForm.Items.Item(val).Specific

        Catch ex As Exception

        End Try
        Return obj
    End Function

    Public Function GetStaticText(ByVal val As String) As StaticText
        Dim obj As StaticText
        Try
            obj = oForm.Items.Item(val).Specific
        Catch ex As Exception

        End Try
        Return obj
    End Function

    Public Function GetGrid(ByVal val As String) As Grid
        Dim obj As Grid
        Try
            obj = oForm.Items.Item(val).Specific
        Catch ex As Exception

        End Try
        Return obj
    End Function




End Class
