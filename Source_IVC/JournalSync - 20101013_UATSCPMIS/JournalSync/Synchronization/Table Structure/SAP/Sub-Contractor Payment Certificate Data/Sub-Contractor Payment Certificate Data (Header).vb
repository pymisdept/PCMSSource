Imports CPS.SQL.Condition

Namespace Datatable.SAP.AP
    Public Class SubCon_Hdr
        Inherits CPS.SQL.Interface.RecordSet

        Private ReadOnly CheckPoint As String = "Sync_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

#Region "Constanst Value"
        Public Const TableName As String = "PRE_OPCH"
        Public Const DocEntry As String = "DocEntry"
        Public Const DocNum As String = "DocNum"
        Public Const DocType As String = "DocType"
        Public Const CANCELED As String = "CANCELED"
        Public Const _DocStatus As String = "DocStatus"
        Public Const Project As String = "Project"
        Public Const ObjType As String = "ObjType"
        Public Const DocDate As String = "DocDate"
        Public Const DocDueDate As String = "DocDueDate"
        Public Const DocumentDate As String = "TaxDate"
        Public Const CardCode As String = "CardCode"
        Public Const CardName As String = "CardName"
        Public Const DocCur As String = "DocCur"
        Public Const DocRate As String = "DocRate"
        Public Const DisPercent As String = "DiscPrcnt"
        Public Const _Indicator As String = "Indicator"
        'Public Const TotalBefDis As String = "TotalBefDis"
        Public Const DocTotal As String = "DocTotal"
        Public Const DocTotalFC As String = "DocTotalFC"

        Public Const ShipToAddress As String = "Address2"
        Public Const BillToAddress As String = "Address"

        ''' <summary>
        ''' Delivery Instruction
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DelIns As String = "U_DelIns"
        ''' <summary>
        ''' Contact Name
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CntctName As String = "U_CntctName"
        ''' <summary>
        ''' Contact Tel No.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const CntctTel As String = "U_CntctTel"
        ''' <summary>
        ''' PCMS Document No.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PCMSDocNum As String = "U_PCMSDocNum"
        ''' <summary>
        ''' Document Subject
        ''' </summary>
        ''' <remarks></remarks>
        Public Const DocSubject As String = "U_DocSubject"
        ''' <summary>
        ''' Ref Date 1
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RefDate1 As String = "U_RefDate1"
        ''' <summary>
        ''' Ref Date 2
        ''' </summary>
        ''' <remarks></remarks>
        Public Const RefDate2 As String = "U_RefDate2"
        ''' <summary>
        ''' Payment Term Desc
        ''' </summary>
        ''' <remarks></remarks>
        Public Const PayTermDesc As String = "U_PayTermDesc"
        ''' <summary>
        ''' Subsidiary Code
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SubsiCode As String = "U_SubsiCode"
        ''' <summary>
        ''' Sales Employee Name
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SlpName As String = "U_SlpName"
        ''' <summary>
        ''' Sales Employee Tel No.
        ''' </summary>
        ''' <remarks></remarks>
        Public Const SlpTel As String = "U_SlpTel"

        '14 Mar 2010
        Public Const AppWork As String = "U_AppWork"
        Public Const AppMOS As String = "U_AppMOS"
        Public Const AppDW As String = "U_AppDW"
        Public Const AppClaim As String = "U_AppClaim"
        Public Const AppVO As String = "U_AppVO"
        Public Const AppDAP As String = "U_AppDAP"
        Public Const AppRefundDAP As String = "U_AppRefundDAP"
        Public Const AppCC As String = "U_AppCC"
        Public Const RetenPrcnt As String = "U_RetenPrcnt"
        Public Const RetenMaxAmt As String = "U_RetenMaxAmt"

        '21 Jan 2010
        Public Const _U_PANo As String = "U_PurchaseAgreement"
        Public Const _U_CONo As String = "U_CONo"
        Public Const _U_PONo As String = "U_PONo"

        Public Const _U_RevFlag As String = "U_RevFlag"

        '17 April 2018
        Public Const GSTPrcnt As String = "U_GSTPrcnt"
        Public Const ThisGST As String = "U_AppGST"
        Public Const CumGST As String = "U_CumGST"

#End Region

#Region "Define New"
        Public Sub New()
            MyBase.New(TableName)
            Me.add(DocEntry)
            Me.add(DocNum)
            Me.add(DocType)
            Me.add(CANCELED)
            Me.add(_DocStatus)
            Me.add(ObjType)
            Me.add(DocDate)
            Me.add(DocDueDate)
            Me.add(DocumentDate)
            Me.add(CardCode)
            Me.add(CardName)
            Me.add(Project)
            Me.add(DocCur)
            Me.add(DocRate)
            Me.add(DisPercent)
            Me.add(_Indicator)
            'Me.add(TotalBefDis)
            Me.add(DocTotal)
            Me.add(DocTotalFC)
            Me.add(DelIns)
            Me.add(CntctName)
            Me.add(CntctTel)
            Me.add(PCMSDocNum)
            Me.add(DocSubject)
            Me.add(RefDate1)
            Me.add(RefDate2)
            Me.add(PayTermDesc)
            Me.add(SubsiCode)
            Me.add(SlpName)
            Me.add(SlpTel)

            Me.add(ShipToAddress)
            Me.add(BillToAddress)

            '14 Mar 2010
            Me.add(AppWork)
            Me.add(AppMOS)
            Me.add(AppDW)
            Me.add(AppClaim)
            Me.add(AppVO)
            Me.add(AppDAP)
            Me.add(AppRefundDAP)
            Me.add(AppCC)
            Me.add(RetenMaxAmt)
            Me.add(RetenPrcnt)

            '21 Jan 2010
            Me.add(_U_PANo)
            Me.add(_U_PONo)
            Me.add(_U_CONo)

            Me.add(_U_RevFlag)

            '17 April 2018
            Me.add(GSTPrcnt)
            Me.add(ThisGST)
            Me.add(CumGST)

            Me.getSubConstInvoice()
        End Sub
#End Region

#Region "Get Filter Information Structure"
        Public Sub getSubConstInvoice()
            Dim oCondition As Condition
            Dim sqlStr As String

            Me.clearFilter()

            '((ObjType = '13')
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = SubCon_Hdr.ObjType
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = "18"
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            'And (DocType = 'S'))
            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = SubCon_Hdr.DocType
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = "S"
            oCondition.BracketCloseNum = 2
            Me.addFilter(oCondition)

            sqlStr = ""
            sqlStr &= "	Not Exists(Select 1 From [" & currentCompany.CompanyDB & "].[dbo].[" & Datatable.SAP.Sync_History.TableName & "]	" & vbCrLf
            sqlStr &= "	           Where	" & vbCrLf
            sqlStr &= "	           " & Datatable.SAP.AP.SubCon_Hdr.TableName & ".DocEntry = [" & Datatable.SAP.Sync_History.TableName & "].DocEntry And " & vbCrLf
            sqlStr &= "	           " & Datatable.SAP.AP.SubCon_Hdr.TableName & ".ObjType  = [" & Datatable.SAP.Sync_History.TableName & "].ObjType And " & vbCrLf
            sqlStr &= "	           [" & Datatable.SAP.Sync_History.TableName & "].ReasonCode='S18' " & vbCrLf
            sqlStr &= "	)	" & vbCrLf
            sqlStr &= " AND " & Datatable.SAP.AP.SubCon_Hdr.TableName & ".DocStatus  = 'O'" & vbCrLf
            'sqlStr &= " AND " & Datatable.SAP.AP.SubCon_Hdr.TableName & ".DocTotal > 0" & vbCrLf
            sqlStr &= " AND " & Datatable.SAP.AP.SubCon_Hdr.TableName & ".DocTotal <> 0" & vbCrLf

            sqlStr &= " AND ((" & Datatable.SAP.AP.SubCon_Hdr.TableName & ".Project <> '19067B3H'" & vbCrLf
            sqlStr &= " AND " & Datatable.SAP.AP.SubCon_Hdr.TableName & ".Project <> '15008B3H'" & vbCrLf
            sqlStr &= " AND " & Datatable.SAP.AP.SubCon_Hdr.TableName & ".Project <> '03085B1H'" & vbCrLf
            sqlStr &= " AND " & Datatable.SAP.AP.SubCon_Hdr.TableName & ".Project <> '04011C3H'" & vbCrLf
            sqlStr &= " AND " & Datatable.SAP.AP.SubCon_Hdr.TableName & ".Project <> '16028B1H'" & vbCrLf
            sqlStr &= " AND " & Datatable.SAP.AP.SubCon_Hdr.TableName & ".Project <> '20092BAH')" & vbCrLf
            sqlStr &= " OR (" & Datatable.SAP.AP.SubCon_Hdr.TableName & ".Project IN ('19067B3H','15008B3H','03085B1H','04011C3H','16028B1H','20092BAH')" & vbCrLf
            sqlStr &= " AND " & Datatable.SAP.AP.SubCon_Hdr.TableName & ".DocEntry IN (SELECT A.DocEntry from PCMS_FE.PCMS800.dbo.DocumentProperty A where A.DocStatus = 'PPPS' and A.Type = 1012 and exists (select * from PMIS_BE.SPMMLPYE.dbo.[cmdocument] where convert(int, quantity2) = A.ID and cmdocumentstatus IN (21, 147) and alternativeno COLLATE LATIN1_GENERAL_BIN2 = A.DocNum))))" & vbCrLf

            Me.AdvanceFilter("And " & sqlStr)

            WriteDebug("Get SubCOnstInoice Query: " & Me.SelectQuery & filterQuery() & sqlStr)
        End Sub


        Public Sub getPurchaseInvoice(ByVal pDocEntry As Integer)
            Dim oCondition As Condition

            Me.clearFilter()

            '((DocEntry = '#####')
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = Supplier_Hdr.DocEntry
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pDocEntry
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            'And (ObjType = '18')
            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = Supplier_Hdr.ObjType
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = "18"
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            'And (DocType = 'I'))
            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = Supplier_Hdr.DocType
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = "S"
            oCondition.BracketCloseNum = 2
            Me.addFilter(oCondition)

        End Sub


#End Region

#Region "Assign Value"
        Public WriteOnly Property DocStatus() As String
            Set(ByVal value As String)
                If Not Me.modify(_DocStatus, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Document Status]")
                End If
            End Set
        End Property
#End Region

    End Class
End Namespace