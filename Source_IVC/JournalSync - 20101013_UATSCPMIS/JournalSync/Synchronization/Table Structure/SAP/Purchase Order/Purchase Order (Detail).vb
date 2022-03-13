Imports CPS.SQL.Condition

Namespace Datatable.SAP.AP
    Public Class OPOR_Dtl
        Inherits CPS.SQL.Interface.RecordSet

        Private ReadOnly CheckPoint As String = "Sync_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

#Region "Constanst Value"
        Public Const sapObjType As String = "22"
        Public Const TableName As String = "PRE_POR1"
        Public Const DocEntry As String = "DocEntry"
        Public Const ObjType As String = "ObjType"
        Public Const LineNo As String = "LineNum"
        Public Const ItemCode As String = "ItemCode"
        Public Const ItemName As String = "Dscription"
        Public Const Quantity As String = "Quantity"
        Public Const UnitPrice As String = "PriceBefDi"
        Public Const AcctCode As String = "AcctCode"
        Public Const PrjCode As String = "Project"
        Public Const DocCurr As String = "Currency"
        Public Const DocRate As String = "Rate"

        Public Const BaseEntry As String = "BaseEntry"
        Public Const BaseLine As String = "BaseLine"
        Public Const BaseType As String = "BaseType"

        Public Const DisPercent As String = "DiscPrcnt"
        Public Const LC_Total As String = "LineTotal"
        Public Const SC_Total As String = "TotalSumSy"
        Public Const FC_Total As String = "TotalFrgn"

        Public Const _ShipDate As String = "ShipDate"

        Public Const _U_Size As String = "U_Size"
        Public Const _U_Packing As String = "U_Packing"
        Public Const _U_Color As String = "U_Color"
        Public Const _U_Brand As String = "U_Brand"
        Public Const _U_Model As String = "U_Model"
        Public Const _U_SupInvNum As String = "U_SupInvNum"
        Public Const _U_QuoteNum As String = "U_QuoteNum"
        Public Const _U_SourceType As String = "U_SourceType"
        Public Const _U_SourceLine As String = "U_SourceLine"
        Public Const _U_DestType As String = "U_DestType"
        Public Const _U_UOM As String = "U_UOM"
        Public Const _U_PCMSDocNum As String = "U_PCMSDocNum"
        Public Const _U_BillNum As String = "U_BillNum"
        Public Const _U_SecNum As String = "U_SecNum"
        Public Const _U_SubSecNum As String = "U_SubSecNum"
        Public Const _U_PageNum As String = "U_PageNum"
        Public Const _U_Quantity As String = "U_Quantity"
        Public Const _U_Price As String = "U_Price"
        Public Const _U_ItemType As String = "U_ItemType"
        Public Const _U_MCBillNum As String = "U_MCBillNum"
        Public Const _U_MCSecNum As String = "U_MCSecNum"
        Public Const _U_MCSubSecNum As String = "U_MCSubSecNum"
        Public Const _U_MCPageNum As String = "U_MCPageNum"
        Public Const _U_PriceType As String = "U_PriceType"
        Public Const _U_AppMethod As String = "U_AppMethod"
        Public Const _U_LineType As String = "U_LineType"
        Public Const _U_MCLineNum As String = "U_MCLineNum"
        Public Const _U_OpenPrcnt As String = "U_OpenPrcnt"
        Public Const _U_ContraFlag As String = "U_ContraFlag"
        Public Const _U_RecoverFlag As String = "U_RecoverFlag"
        Public Const _U_RecoverStatus As String = "U_RecoverStatus"
        Public Const _U_SubLineNum As String = "U_SubLineNum"
        Public Const _U_MCSubLineNum As String = "U_MCSubLineNum"
        Public Const _U_ClientRef As String = "U_ClientRef"
        Public Const _U_SourceEntry As String = "U_SourceEntry"
        Public Const _U_DestEntry As String = "U_DestEntry"
        Public Const _U_IncomeCode As String = "U_IncomeCode"
        Public Const _U_IPCode As String = "U_IPCode"
        Public Const _U_BillLineNum As String = "U_BillLineNum"
        Public Const _U_BillSubLineNum As String = "U_BillSubLineNum"

        '// Additional UDF at 19 Dec 2009
        Public Const _U_ItemNum As String = "U_ItemNum"
        Public Const _U_VONum As String = "U_VONum"
        Public Const _U_RefNum As String = "U_RefNum"
        Public Const _U_Budget As String = "U_Budget"
        Public Const _U_FullDesc As String = "U_FullDesc"
        Public Const _U_RefCardCode As String = "U_RefCardCode"
        Public Const _U_RefCardName As String = "U_RefCardName"
        Public Const _U_ReasonCode As String = "U_ReasonCode"
        Public Const _U_ReasonDesc As String = "U_ReasonDesc"

#End Region

#Region "Define New"
        Public Sub New()
            MyBase.New(TableName)
            Me.add(DocEntry)
            Me.add(ObjType)
            Me.add(LineNo)
            Me.add(AcctCode)
            Me.add(ItemCode)
            Me.add(ItemName)
            Me.add(PrjCode)
            Me.add(DocCurr)
            Me.add(DocRate)
            Me.add(DisPercent)
            Me.add(_U_SupInvNum)
            Me.add(LC_Total)
            Me.add(SC_Total)
            Me.add(FC_Total)

            Me.add(_ShipDate)

            Me.add(BaseEntry)
            Me.add(BaseType)
            Me.add(BaseLine)
            Me.add(Quantity)
            Me.add(UnitPrice)

            Me.add(_U_Size)
            Me.add(_U_Packing)
            Me.add(_U_Color)
            Me.add(_U_Brand)
            Me.add(_U_Model)
            Me.add(_U_SupInvNum)
            Me.add(_U_QuoteNum)
            Me.add(_U_SourceType)
            Me.add(_U_SourceLine)
            Me.add(_U_DestType)
            Me.add(_U_UOM)
            Me.add(_U_PCMSDocNum)
            Me.add(_U_BillNum)
            Me.add(_U_SecNum)
            Me.add(_U_SubSecNum)
            Me.add(_U_PageNum)
            Me.add(_U_Quantity)
            Me.add(_U_Price)
            Me.add(_U_ItemType)
            Me.add(_U_MCBillNum)
            Me.add(_U_MCSecNum)
            Me.add(_U_MCSubSecNum)
            Me.add(_U_MCPageNum)
            Me.add(_U_PriceType)
            Me.add(_U_AppMethod)
            Me.add(_U_LineType)
            Me.add(_U_MCLineNum)
            Me.add(_U_OpenPrcnt)
            Me.add(_U_ContraFlag)
            Me.add(_U_RecoverFlag)
            Me.add(_U_RecoverStatus)
            Me.add(_U_SubLineNum)
            Me.add(_U_MCSubLineNum)
            Me.add(_U_ClientRef)
            Me.add(_U_SourceEntry)
            Me.add(_U_DestEntry)
            Me.add(_U_IncomeCode)
            Me.add(_U_IPCode)
            Me.add(_U_BillLineNum)
            Me.add(_U_BillSubLineNum)

            Me.add(_U_ItemNum)
            Me.add(_U_VONum)
            Me.add(_U_RefNum)
            Me.add(_U_Budget)
            Me.add(_U_FullDesc)
            Me.add(_U_RefCardCode)
            Me.add(_U_RefCardName)
            Me.add(_U_ReasonCode)
            Me.add(_U_ReasonDesc)
        End Sub
#End Region

#Region "Get Filter Information Structure"
        Public Sub getDocumentLine(ByVal pDocEntry As Integer)
            Dim oCondition As Condition

            Me.clearFilter()

            '((DocEntry = '#####')
            oCondition = New Condition
            'Karrson: Change
            'oCondition.BracketOpenNum = 2
            oCondition.BracketOpenNum = 1
            oCondition.Alias = DocEntry
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pDocEntry
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            'And (ObjType = ''))
            'Karrson Remove Start
            'oCondition = New Condition
            'oCondition.Relation = Condition.eRelation.re_AND
            'oCondition.BracketOpenNum = 1
            'oCondition.Alias = ObjType
            'oCondition.Operation = Condition.eOperation.op_EQUAL
            'oCondition.Value = sapObjType
            'oCondition.BracketCloseNum = 2
            'Me.addFilter(oCondition)
            'Karrson REmove End
        End Sub
#End Region

#Region "Assign Value"
        Public WriteOnly Property U_Size() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_Size, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_Packing() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_Packing, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_Color() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_Color, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_Brand() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_Brand, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_Model() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_Model, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_SupInvNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_SupInvNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_QuoteNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_QuoteNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_SourceType() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_SourceType, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_SourceLine() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_SourceLine, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_DestType() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_DestType, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_UOM() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_UOM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_PCMSDocNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_PCMSDocNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_BillNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_BillNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_SecNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_SecNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_SubSecNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_SubSecNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_PageNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_PageNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_Quantity() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_Quantity, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_Price() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_Price, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_ItemType() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_ItemType, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_MCBillNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_MCBillNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_MCSecNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_MCSecNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_MCSubSecNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_MCSubSecNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_MCPageNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_MCPageNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_PriceType() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_PriceType, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_AppMethod() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_AppMethod, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_LineType() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_LineType, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_MCLineNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_MCLineNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_OpenPrcnt() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_OpenPrcnt, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_ContraFlag() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_ContraFlag, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_RecoverFlag() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_RecoverFlag, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_RecoverStatus() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_RecoverStatus, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_SubLineNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_SubLineNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_MCSubLineNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_MCSubLineNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_ClientRef() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_ClientRef, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_SourceEntry() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_SourceEntry, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_DestEntry() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_DestEntry, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_IncomeCode() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_IncomeCode, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_IPCode() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_IPCode, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_BillLineNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_BillLineNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property
        Public WriteOnly Property U_BillSubLineNum() As String
            Set(ByVal value As String)
                If Not Me.Modify(_U_BillSubLineNum, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[]")
                End If
            End Set
        End Property

#End Region

    End Class
End Namespace
