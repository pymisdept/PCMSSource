Imports CPS.SQL.Condition

Namespace Datatable.SAP.AP
    Public Class OPOR_Hdr
        Inherits CPS.SQL.Interface.RecordSet

        Private ReadOnly CheckPoint As String = "Sync_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

#Region "Constanst Value"
        Public Const sapObjType As String = "22"
        Public Const TableName As String = "PRE_OPOR"
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
        'Public Const TotalBefDis As String = "TotalBefDis"
        Public Const DocTotal As String = "DocTotal"
        Public Const DocTotalFC As String = "DocTotalFC"

        Public Const ShipToAddress As String = "Address2"
        Public Const BillToAddress As String = "Address"

        Public Const _Indicator As String = "Indicator"

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

        '11 Dec 2009
        Public Const _U_AppWork As String = "U_AppWork"
        Public Const _U_AppMOS As String = "U_AppMOS"
        Public Const _U_AppDW As String = "U_AppDW"
        Public Const _U_AppClaim As String = "U_AppClaim"
        Public Const _U_AppVO As String = "U_AppVO"

        '19 Dec 2009
        Public Const _U_AppCC As String = "U_AppCC"
        Public Const _U_RetenPrcnt As String = "U_RetenPrcnt"
        Public Const _U_RetenMaxAmt As String = "U_RetenMaxAmt"

        ' Confirmation Order Number
        Public Const _U_CONO As String = "U_CONo"
        ' MR Number
        Public Const _U_MRNO As String = "U_MRNo"
        ' PO Number
        Public Const _U_PONO As String = "U_PONo"

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

            '11 Dec 2009
            Me.add(_U_AppWork)
            Me.add(_U_AppMOS)
            Me.add(_U_AppDW)
            Me.add(_U_AppClaim)
            Me.add(_U_AppVO)

            '19 Dec 2009
            Me.add(_U_AppCC)
            Me.add(_U_RetenMaxAmt)
            Me.add(_U_RetenPrcnt)

            Me.add(_U_MRNO)
            Me.add(_U_CONO)
            Me.add(_U_PONO)
            Me.getDocument()
        End Sub
#End Region

#Region "Get Filter Information Structure"

        Public Sub getDocument()
            Dim oCondition As Condition
            Dim sqlStr As String

            Me.clearFilter()

            'Karrson: Remove Start

            '((ObjType = '23')
            'oCondition = New Condition
            'oCondition.BracketOpenNum = 2
            'oCondition.Alias = OPOR_Hdr.ObjType
            'oCondition.Operation = Condition.eOperation.op_EQUAL
            'oCondition.Value = sapObjType
            'oCondition.BracketCloseNum = 1
            ''Karrson: Debug
            'WriteDebug("Condition:" & oCondition.ToString())

            'Me.addFilter(oCondition)
            'Karrson: Remove End

            'And (DocStatus = 'O'))
            oCondition = New Condition
            'Karrson:  Remove
            'oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = OPOR_Hdr._DocStatus
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = "O"
            'Karrson: Change
            'oCondition.BracketCloseNum = 2
            oCondition.BracketCloseNum = 1


            Me.addFilter(oCondition)

            sqlStr = ""
            sqlStr &= "	Not Exists(Select 1 From [" & currentCompany.CompanyDB & "].[dbo].[" & Datatable.SAP.Sync_History.TableName & "]	" & vbCrLf
            sqlStr &= "	           Where	" & vbCrLf
            sqlStr &= "	           " & Datatable.SAP.AP.OPOR_Hdr.TableName & ".DocEntry = [" & Datatable.SAP.Sync_History.TableName & "].DocEntry And 	" & vbCrLf
            'sqlStr &= "	           " & Datatable.SAP.AP.OPOR_Hdr.TableName & ".ObjType  = [" & Datatable.SAP.Sync_History.TableName & "].ObjType	" & vbCrLf
            sqlStr &= "	           " & Datatable.SAP.AP.OPOR_Hdr.TableName & ".ObjType  = [" & Datatable.SAP.Sync_History.TableName & "].ObjType And	" & vbCrLf
            sqlStr &= "	           " & "[" & Datatable.SAP.Sync_History.TableName & "].ReasonCode = 'D112' " & vbCrLf
            sqlStr &= "	)	" & vbCrLf
            sqlStr &= "	AND " & Datatable.SAP.AP.OPOR_Hdr.TableName & ".DocStatus  = 'O'" & vbCrLf

            Me.AdvanceFilter("And " & sqlStr)
            WriteDebug("Purchase Query: " & sqlStr)
        End Sub

        Public Sub getDocument(ByVal pDocEntry As Integer)
            Dim oCondition As Condition

            Me.clearFilter()
            '
            '((DocEntry = '#####')
            oCondition = New Condition
            'Karrson: Change
            'oCondition.BracketOpenNum = 2
            oCondition.BracketOpenNum = 1
            oCondition.Alias = OPOR_Hdr.DocEntry
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pDocEntry
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)


            'And (ObjType = '23')
            'Karrson: Remove Start
            'oCondition = New Condition
            'oCondition.Relation = Condition.eRelation.re_AND
            'oCondition.BracketOpenNum = 1
            'oCondition.Alias = OPOR_Hdr.ObjType
            'oCondition.Operation = Condition.eOperation.op_EQUAL
            'oCondition.Value = sapObjType


            'oCondition.BracketCloseNum = 2
            'Me.addFilter(oCondition)
            'Karrson: Remove end


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