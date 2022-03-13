Imports CPS.SQL.Condition

Namespace Datatable.SAP
    Public Class Draft_Mapping
        Inherits CPS.SQL.Interface.RecordSet

        Private ReadOnly CheckPoint As String = "Sync_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

        Public Const _TableName As String = "CPSDFF"
        Public Const _DocEntry As String = "DocEntry"
        Public Const _ObjType As String = "ObjType"
        Public Const _DraftKey As String = "DraftKey"
        Public Const _CreateDate As String = "CreateDate"

        Public Sub New()
            MyBase.New(_TableName)
            Me.add(_DocEntry)
            Me.add(_ObjType)
            Me.add(_DraftKey)
            Me.add(_CreateDate, Format(Now, "yyyyMMdd HH:mm:ss"))
        End Sub

#Region "Assign Value"
        Public WriteOnly Property DocumentKey() As String
            Set(ByVal value As String)
                If Not Me.modify(_DocEntry, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Document Entry]")
                End If
            End Set
        End Property
        Public WriteOnly Property ObjectType() As String
            Set(ByVal value As String)
                If Not Me.modify(_ObjType, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Object Type]")
                End If
            End Set
        End Property
        Public WriteOnly Property DraftEntry() As Integer
            Set(ByVal value As Integer)
                If Not Me.modify(_DraftKey, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Draft Key.]")
                End If
            End Set
        End Property
        Public WriteOnly Property CreateDate() As Date
            Set(ByVal value As Date)
                If Not Me.modify(_CreateDate, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Create Document Date]")
                End If
            End Set
        End Property
#End Region

#Region "Get Filter Information Structure"
        Public Sub getPurchaseInvoice(ByVal pDraftKey As Integer)
            Dim oCondition As Condition

            Me.clearFilter()

            '((ObjType = '18')
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = Draft_Mapping._ObjType
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = "18"
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            'And (DocEntry = '<DocEntry>'))
            'And (ObjType = '18')
            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = Draft_Mapping._DraftKey
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pDraftKey
            oCondition.BracketCloseNum = 2
            Me.addFilter(oCondition)

        End Sub
        Public Sub getSalesInvoice(ByVal pDraftKey As Integer)
            Dim oCondition As Condition

            Me.clearFilter()

            '((ObjType = '13')
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = Draft_Mapping._ObjType
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = "13"
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            'And (DocEntry = '<DocEntry>'))
            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = Draft_Mapping._DraftKey
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pDraftKey
            oCondition.BracketCloseNum = 2
            Me.addFilter(oCondition)

        End Sub
        Public Sub getAPCreditMemo(ByVal pDraftKey As Integer)
            Dim oCondition As Condition

            Me.clearFilter()

            '((ObjType = '18')
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = Draft_Mapping._ObjType
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = "19"
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            'And (DocEntry = '<DocEntry>'))
            'And (ObjType = '19')
            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = Draft_Mapping._DraftKey
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pDraftKey
            oCondition.BracketCloseNum = 2
            Me.addFilter(oCondition)

        End Sub
#End Region

    End Class
End Namespace