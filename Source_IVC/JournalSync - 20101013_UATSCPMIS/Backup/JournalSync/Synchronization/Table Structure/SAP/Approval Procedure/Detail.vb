Imports CPS.SQL.Condition

Namespace Datatable.SAP.ApprovalProcedure
    Public Class Detail
        Inherits CPS.SQL.Interface.RecordSet

        Private ReadOnly CheckPoint As String = "Sync_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

#Region "Constanst Value"
        Public Const TableName As String = "WDD1"
        Public Const _WddCode As String = "WddCode"  'Internal ID
        Public Const _StepCode As String = "StepCode" 'Stage Key
        Public Const _UserID As String = "UserID"   'Authorizer Code
        Public Const _Status As String = "Status"   'Status
        Public Const _Remarks As String = "Remarks"  'Remarks
        Public Const _UserSign As String = "UserSign" 'User Signature
        Public Const _CreateDate As String = "CreateDate"   'Request Date
        Public Const _CreateTime As String = "CreateTime"   'Request Time
#End Region

#Region "Define New"
        Public Sub New()
            MyBase.New(TableName)
            Me.add(_WddCode)
            Me.add(_StepCode)
            Me.add(_UserID)
            Me.add(_Status)
            Me.add(_Remarks)
            Me.add(_UserSign)
            Me.add(_CreateDate)
            Me.add(_CreateTime)
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
#End Region

#Region "Assign Value"
        Public WriteOnly Property WddCode() As String
            Set(ByVal value As String)
                If Not Me.modify(_WddCode, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Internal ID]")
                End If
            End Set
        End Property
        Public WriteOnly Property StepCode() As String
            Set(ByVal value As String)
                If Not Me.modify(_StepCode, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Stage Key]")
                End If
            End Set
        End Property
        Public WriteOnly Property UserID() As String
            Set(ByVal value As String)
                If Not Me.modify(_UserID, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Authorizer Code]")
                End If
            End Set
        End Property
        Public WriteOnly Property Status() As String
            Set(ByVal value As String)
                If Not Me.modify(_Status, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Status]")
                End If
            End Set
        End Property
        Public WriteOnly Property Remarks() As String
            Set(ByVal value As String)
                If Not Me.modify(_Remarks, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Remarks]")
                End If
            End Set
        End Property
        Public WriteOnly Property UserSign() As String
            Set(ByVal value As String)
                If Not Me.modify(_UserSign, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[User Signature]")
                End If
            End Set
        End Property
        Public WriteOnly Property CreateDate() As String
            Set(ByVal value As String)
                If Not Me.modify(_CreateDate, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Request Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property CreateTime() As String
            Set(ByVal value As String)
                If Not Me.modify(_CreateTime, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Request Time]")
                End If
            End Set
        End Property
#End Region

    End Class
End Namespace

