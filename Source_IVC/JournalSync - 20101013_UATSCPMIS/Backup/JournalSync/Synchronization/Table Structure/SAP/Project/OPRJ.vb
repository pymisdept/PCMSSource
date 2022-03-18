Imports CPS.SQL.Condition

Namespace Datatable.SAP.Admin
    Public Class OPRJ
        Inherits CPS.SQL.Interface.RecordSet

        Private ReadOnly CheckPoint As String = "Oprj_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

        Public Const TableName As String = "OPRJ"
        Public Const _ProjectKey As String = "U_DocEntry"
        Public Const _PrjCode As String = "PrjCode"
        Public Const _PrjName As String = "PrjName"
        Public Const _PrjFullName As String = "U_ProjectFullName"
        Public Const _DocStatus As String = "U_DocStatus"
        Public Const _Locked As String = "Locked"
        Public Const _LegCde As String = "U_SubsidiaryCode"

        Public Sub New()
            MyBase.New(TableName)
            Me.add(_ProjectKey)
            Me.add(_PrjCode)
            Me.add(_PrjName)
            Me.add(_PrjFullName)
            Me.add(_DocStatus)
            Me.add(_Locked)
            Me.add(_LegCde)
        End Sub

        Public Sub getProjectEntry(ByVal pPrjCode As String)
            Dim oCondition As Condition

            Me.clearFilter()

            '(PrjCode = '<pPrjCode>')
            oCondition = New Condition
            oCondition.BracketOpenNum = 1
            oCondition.Alias = OPRJ._PrjCode
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pPrjCode
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

        End Sub

#Region "Assign Value"
        Public WriteOnly Property KeyIndex() As Integer
            Set(ByVal value As Integer)
                If Not Me.modify(_ProjectKey, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Project Key]")
                End If
            End Set
        End Property

        Public WriteOnly Property PrjCode() As String
            Set(ByVal value As String)
                If Not Me.modify(_PrjCode, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Project Code]")
                End If
            End Set
        End Property

        Public WriteOnly Property PrjName() As String
            Set(ByVal value As String)
                If Not Me.modify(_PrjName, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Project Name]")
                End If
            End Set
        End Property

        Public WriteOnly Property PrjFullName() As String
            Set(ByVal value As String)
                If Not Me.modify(_PrjFullName, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Project Full Name]")
                End If
            End Set
        End Property

        Public WriteOnly Property Locked() As String
            Set(ByVal value As String)
                If Not Me.modify(_Locked, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Project Lock]")
                End If
            End Set
        End Property

        Public WriteOnly Property DocStatus() As String
            Set(ByVal value As String)
                If Not Me.modify(_DocStatus, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Project Lock]")
                End If
            End Set
        End Property

        Public WriteOnly Property LegCde() As String
            Set(ByVal value As String)
                If Not Me.modify(_LegCde, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Ledger Code]")
                End If
            End Set
        End Property

        Public Function getKeyIndex() As Integer
            Dim sqlStr As String = "Select Top 1 IsNull(" & _ProjectKey & ", 0) " & _ProjectKey & " From [" & TableName & "] Order By " & _ProjectKey & " Desc"
            Dim oRecSet As SAPbobsCOM.Recordset

            oRecSet = commonRecordSet.execute(sqlStr)
            If oRecSet.RecordCount = 0 Then
                Return 1
            Else
                Return oRecSet.Fields.Item(_ProjectKey).Value + 1
            End If
        End Function
#End Region

    End Class
End Namespace

