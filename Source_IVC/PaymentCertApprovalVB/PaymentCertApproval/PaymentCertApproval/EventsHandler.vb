'------------------------------------------------------------------------------
' <auto-generated>
'     This code was generated by a tool.
'     Runtime Version:2.0.50727.8800
'
'     Changes to this file may cause incorrect behavior and will be lost if
'     the code is regenerated.
' </auto-generated>
'------------------------------------------------------------------------------

Option Strict Off
Option Explicit On

Imports B1WizardBase
Imports SAPbobsCOM
Imports SAPbouiCOM

Namespace PaymentCertApproval
    
    Public Class EventsHandler
        Inherits B1Event
        
        Public Sub New()
            MyBase.New
        End Sub


        <B1Listener(BoEventTypes.et_FORM_DATA_ADD, True)> _
        Public Overridable Function OnFormDataAdd(ByVal pVal As BusinessObjectInfo) As Boolean
            If pVal.FormTypeEx = "141" And pVal.Type = "18" Then

                B1Connections.theAppl.MessageBox("You cannot post AP Invoice without approved, please save as draft document")
                Return False
            End If
            Return True
        End Function

        <B1Listener(BoEventTypes.et_FORM_LOAD, False)> _
        Public Overridable Sub OnAfterFormLoad(ByVal pval As ItemEvent)

            If pval.FormTypeEx = DraftPaymentCert.FormType Then


            End If

        End Sub



        <B1Listener(BoEventTypes.et_ITEM_PRESSED, True)> _
        Public Overridable Function OnBeforeItemPressed(ByVal pVal As ItemEvent) As Boolean

            If pVal.FormTypeEx = DraftPaymentCert.FormType And pVal.ItemUID = DraftPaymentCert.uidSearch Then
                Dim oform As Form = B1Connections.theAppl.Forms.Item(pVal.FormUID)
                Dim dp As DraftPaymentCert = New DraftPaymentCert(oform)
                Dim ErrMsg As String = ""

                If dp.Search(ErrMsg) = False Then
                    SBOApplication.SetStatusBarMessage(ErrMsg)
                End If

            End If


            If pVal.FormTypeEx = DraftPaymentCert.FormType And pVal.ItemUID = DraftPaymentCert.uidRemove And pVal.EventType = BoEventTypes.et_ITEM_PRESSED Then
                Dim oform As Form = B1Connections.theAppl.Forms.Item(pVal.FormUID)
                Dim dp As DraftPaymentCert = New DraftPaymentCert(oform)
                Dim ErrMsg As String = ""

                If dp.Remove(ErrMsg) = False Then
                    SBOApplication.SetStatusBarMessage(ErrMsg)
                Else
                    dp.Search(ErrMsg)
                End If

            End If

            'If pVal.FormType = "" And pVal.ItemUID = "" And pVal.EventType = BoEventTypes.et_VALIDATE And pVal.BeforeAction = True Then

            'End If

            If pVal.FormTypeEx = DraftPaymentCert.FormType And pVal.ItemUID = DraftPaymentCert.uidSubmit Then
                Dim oform As Form = B1Connections.theAppl.Forms.Item(pVal.FormUID)

                Dim dp As DraftPaymentCert = New DraftPaymentCert(oform)
                Dim ErrMsg As String = ""
                If dp.UpdateApprovalStatus(ErrMsg) = False Then
                    SBOApplication.SetStatusBarMessage(ErrMsg)
                Else
                    SBOApplication.SetStatusBarMessage("Process Completed", BoMessageTime.bmt_Medium, False)
                    dp.ClearFoum()
                End If

            End If


            'ADD YOUR ACTION CODE HERE ...
            Return True
        End Function

        <B1Listener(BoEventTypes.et_VALIDATE, False)> _
        Public Overridable Sub OnAfterValidate(ByVal pVal As ItemEvent)

            If pVal.FormTypeEx = "frmpcapp" And pVal.ItemUID = "etPrjCode" Then
                Dim oForm As Form
                Try

                    oForm = SBOApplication.Forms.Item(pVal.FormUID)
                    If oForm.DataSources.UserDataSources.Item("UDPRJ").Value = "" Then
                        CType(oForm.Items.Item("stprjname").Specific, SAPbouiCOM.StaticText).Caption = String.Empty

                    End If
                Catch ex As Exception

                End Try

            End If

        End Sub

        <B1Listener(BoEventTypes.et_MENU_CLICK, True)> _
        Public Overridable Function OnBeforeMenuClick(ByVal pVal As MenuEvent) As Boolean

            If pVal.MenuUID = "IVC_1" Then
                Dim oForm As Form
                Dim dp As DraftPaymentCert
                oForm = CreateForm("FrmApproval.srf", True)
                dp = New DraftPaymentCert(oForm)
                dp.FormLoad()
            End If
            Return True
        End Function

        <B1Listener(BoEventTypes.et_CHOOSE_FROM_LIST, False)> _
        Public Overridable Sub OnAfterChooseFromList(ByVal pVal As ItemEvent)
            Dim oForm As SAPbouiCOM.Form

            Try

                If pVal.ChooseFromListUID = "CFLBPNAME" And pVal.FormTypeEx = DraftPaymentCert.FormType Then

                    oForm = SBOApplication.Forms.Item(pVal.FormUID)
                    Dim dp As DraftPaymentCert = New DraftPaymentCert(oForm)
                    Dim _dt As SAPbouiCOM.DataTable
                    _dt = pVal.SelectedObjects

                    If Not _dt Is Nothing Then
                        oForm.DataSources.UserDataSources.Item("UDBPNAME").Value = _dt.GetValue("CardName", 0)
                        oForm.DataSources.UserDataSources.Item("UDBP").Value = _dt.GetValue("CardCode", 0)

                        'dp.GetStaticText("lbCardName").Caption = _dt.GetValue("CardName", 0)
                    Else
                        oForm.DataSources.UserDataSources.Item("UDBP").Value = ""
                    End If

                    dp = Nothing

                End If


                If pVal.ChooseFromListUID = "CFLBP" And pVal.FormTypeEx = DraftPaymentCert.FormType Then

                    oForm = SBOApplication.Forms.Item(pVal.FormUID)
                    Dim dp As DraftPaymentCert = New DraftPaymentCert(oForm)
                    Dim _dt As SAPbouiCOM.DataTable
                    _dt = pVal.SelectedObjects

                    If Not _dt Is Nothing Then
                        oForm.DataSources.UserDataSources.Item("UDBP").Value = _dt.GetValue("CardCode", 0)
                        oForm.DataSources.UserDataSources.Item("UDBPNAME").Value = _dt.GetValue("CardName", 0)

                        'dp.GetStaticText("lbCardName").Caption = _dt.GetValue("CardName", 0)
                    Else
                        oForm.DataSources.UserDataSources.Item("UDBPNAME").Value = ""
                    End If
                    dp = Nothing

                End If

                If pVal.ChooseFromListUID = "CFLPRJ" Then
                    oForm = SBOApplication.Forms.Item(pVal.FormUID)
                    Dim dp As DraftPaymentCert = New DraftPaymentCert(oForm)
                    Dim _dt As SAPbouiCOM.DataTable
                    _dt = pVal.SelectedObjects

                    If Not _dt Is Nothing Then
                        'dp.GetEditText("etPrjCode").Value = _dt.GetValue("PrjCode", 0)
                        oForm.DataSources.UserDataSources.Item("UDPRJ").Value = _dt.GetValue("PrjCode", 0)
                        dp.GetStaticText("stprjname").Caption = _dt.GetValue("PrjName", 0)
                    Else
                        dp.GetStaticText("stprjname").Caption = ""
                    End If

                End If
            Catch ex As Exception
                SBOApplication.SetStatusBarMessage(ex.Message)

            End Try
        End Sub
    End Class
End Namespace
