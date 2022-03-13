Imports CPS.SQL.Condition

Namespace Datatable.SAP.ApprovalProcedure
    Public Class Header
        Inherits CPS.SQL.Interface.RecordSet

        Private ReadOnly CheckPoint As String = "Sync_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

#Region "Constanst Value"
        Public Const TableName As String = "OWDD"
        Public Const _WddCode As String = "WddCode"  'Internal Number
        Public Const _WtmCode As String = "WtmCode"  'Authorization Template
        Public Const _OwnerID As String = "OwnerID"  'Production Code
        Public Const _DocEntry As String = "DocEntry" 'Document Counter
        Public Const _ObjType As String = "ObjType"  'Object Type
        Public Const _DocDate As String = "DocDate"  'Posting Date
        Public Const _CurrStep As String = "CurrStep" 'Stage Code
        Public Const _Status As String = "Status"   'Status
        Public Const _Remarks As String = "Remarks"  'Remarks
        Public Const _UserSign As String = "UserSign" 'User Signature
        Public Const _CreateDate As String = "CreateDate"   'Production Date
        Public Const _CreateTime As String = "CreateTime"   'Production Time
        Public Const _IsDraft As String = "IsDraft"  'Draft
        Public Const _MaxReqr As String = "MaxReqr"  'No. of Authorizers
#End Region

#Region "Define New"
        Public Sub New()
            MyBase.New(TableName)
            Me.add(_WddCode)
            Me.add(_WtmCode)
            Me.add(_OwnerID)
            Me.add(_DocEntry)
            Me.add(_ObjType)
            Me.add(_DocDate)
            Me.add(_CurrStep)
            Me.add(_Status)
            Me.add(_Remarks)
            Me.add(_UserSign)
            Me.add(_CreateDate)
            Me.add(_CreateTime)
            Me.add(_IsDraft)
            Me.add(_MaxReqr)
        End Sub
#End Region

#Region "Get Filter Information Structure"
        Public Sub getApproval_Transaction(ByVal pEntry As Integer)
            Dim oCondition As Condition

            Me.clearFilter()
            '((WddCode = '#####')
            oCondition = New Condition
            oCondition.BracketOpenNum = 1
            oCondition.Alias = _WddCode
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pEntry
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

        End Sub

        Public Sub getApprovalByDocEntry(ByVal pEntry As Integer)
            Dim oCondition As Condition

            Me.clearFilter()
            '((WddCode = '#####')
            oCondition = New Condition
            oCondition.BracketOpenNum = 1
            oCondition.Alias = _DocEntry
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pEntry
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

        End Sub
#End Region

#Region "Assign Value"
        Public WriteOnly Property WddCode() As String
            Set(ByVal value As String)
                If Not Me.Modify(_WddCode, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Internal Number]")
                End If
            End Set
        End Property
        Public WriteOnly Property WtmCode() As String
            Set(ByVal value As String)
                If Not Me.Modify(_WtmCode, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Authorization Template]")
                End If
            End Set
        End Property
        Public WriteOnly Property OwnerID() As String
            Set(ByVal value As String)
                If Not Me.Modify(_OwnerID, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Production Code]")
                End If
            End Set
        End Property
        Public WriteOnly Property DocEntry() As String
            Set(ByVal value As String)
                If Not Me.Modify(_DocEntry, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document Counter]")
                End If
            End Set
        End Property
        Public WriteOnly Property ObjType() As String
            Set(ByVal value As String)
                If Not Me.Modify(_ObjType, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Object Type]")
                End If
            End Set
        End Property
        Public WriteOnly Property DocDate() As String
            Set(ByVal value As String)
                If Not Me.Modify(_DocDate, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Posting Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property CurrStep() As String
            Set(ByVal value As String)
                If Not Me.Modify(_CurrStep, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Stage Code]")
                End If
            End Set
        End Property
        Public WriteOnly Property Status() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Status, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Status]")
                End If
            End Set
        End Property
        Public WriteOnly Property Remarks() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Remarks, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Remarks]")
                End If
            End Set
        End Property
        Public WriteOnly Property UserSign() As String
            Set(ByVal value As String)
                If Not Me.Modify(_UserSign, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[User Signature]")
                End If
            End Set
        End Property
        Public WriteOnly Property CreateDate() As String
            Set(ByVal value As String)
                If Not Me.Modify(_CreateDate, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Production Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property CreateTime() As String
            Set(ByVal value As String)
                If Not Me.Modify(_CreateTime, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Production Time]")
                End If
            End Set
        End Property
        Public WriteOnly Property IsDraft() As String
            Set(ByVal value As String)
                If Not Me.Modify(_IsDraft, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Draft]")
                End If
            End Set
        End Property
        Public WriteOnly Property MaxReqr() As String
            Set(ByVal value As String)
                If Not Me.Modify(_MaxReqr, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[No. of Authorizers]")
                End If
            End Set
        End Property
#End Region

    End Class
End Namespace

