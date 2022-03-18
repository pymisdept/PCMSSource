Imports CPS.SQL.Condition

Namespace Datatable.Flex
    Public Class POSTL
        Inherits CPS.SQL.Interface.RecordSet

#Region "Constanst Value"

        Private ReadOnly CheckPoint As String = "PNACD_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

        Public Const TableName As String = "POSTL"
        Public Const _POSTL_COMP_CDE As String = "POSTL_COMP_CDE"
        Public Const _POSTL_ACC_CDE As String = "POSTL_ACC_CDE"
        Public Const _POSTL_STL_DOC_NUM As String = "POSTL_STL_DOC_NUM"
        Public Const _POSTL_STL_DOC_DTE As String = "POSTL_STL_DOC_DTE"
        Public Const _POSTL_STL_DOC_TYP As String = "POSTL_STL_DOC_TYP"
        Public Const _POSTL_STL_DOC_ANA1 As String = "POSTL_STL_DOC_ANA1"
        Public Const _POSTL_STL_DOC_ANA2 As String = "POSTL_STL_DOC_ANA2"
        Public Const _POSTL_STL_DOC_ANA3 As String = "POSTL_STL_DOC_ANA3"
        Public Const _POSTL_STL_DOC_ANA4 As String = "POSTL_STL_DOC_ANA4"
        Public Const _POSTL_STL_DOC_ANA5 As String = "POSTL_STL_DOC_ANA5"
        Public Const _POSTL_APP_DOC_NUM As String = "POSTL_APP_DOC_NUM"
        Public Const _POSTL_STL_AMT As String = "POSTL_STL_AMT"
        Public Const _POSTL_APP_DOC_ANA1 As String = "POSTL_APP_DOC_ANA1"
        Public Const _POSTL_APP_DOC_ANA2 As String = "POSTL_APP_DOC_ANA2"
        Public Const _POSTL_APP_DOC_ANA3 As String = "POSTL_APP_DOC_ANA3"
        Public Const _POSTL_APP_DOC_ANA4 As String = "POSTL_APP_DOC_ANA4"
        Public Const _POSTL_APP_DOC_ANA5 As String = "POSTL_APP_DOC_ANA5"
        Public Const _POSTL_CCY_CDE As String = "POSTL_CCY_CDE"
        Public Const _POSTL_EXC_RAT As String = "POSTL_EXC_RAT"
        Public Const _POSTL_STL_DOC_VOU As String = "POSTL_STL_DOC_VOU"
        Public Const _POSTL_APP_DOC_VOU As String = "POSTL_APP_DOC_VOU"
        Public Const _POSTL_FLEX_EXP_DTE As String = "POSTL_FLEX_EXP_DTE"
        Public Const _POSTL_PCMS_STAT As String = "POSTL_PCMS_STAT"
        Public Const _POSTL_PCMS_UPD_DTE As String = "POSTL_PCMS_UPD_DTE"
        Public Const _POSTL_PCMS_RMK As String = "POSTL_PCMS_RMK"
#End Region

#Region "Constructor"
        Public Sub New()
            MyBase.New(TableName, SubMain.FLEX_DBName)
            Me.add(_POSTL_COMP_CDE)
            Me.add(_POSTL_ACC_CDE)
            Me.add(_POSTL_STL_DOC_NUM)
            Me.add(_POSTL_STL_DOC_DTE)
            Me.add(_POSTL_STL_DOC_TYP)
            Me.add(_POSTL_STL_DOC_ANA1)
            Me.add(_POSTL_STL_DOC_ANA2)
            Me.add(_POSTL_STL_DOC_ANA3)
            Me.add(_POSTL_STL_DOC_ANA4)
            Me.add(_POSTL_STL_DOC_ANA5)
            Me.add(_POSTL_APP_DOC_NUM)
            Me.add(_POSTL_STL_AMT)
            Me.add(_POSTL_APP_DOC_ANA1)
            Me.add(_POSTL_APP_DOC_ANA2)
            Me.add(_POSTL_APP_DOC_ANA3)
            Me.add(_POSTL_APP_DOC_ANA4)
            Me.add(_POSTL_APP_DOC_ANA5)
            Me.add(_POSTL_CCY_CDE)
            Me.add(_POSTL_EXC_RAT)
            Me.add(_POSTL_STL_DOC_VOU)
            Me.add(_POSTL_APP_DOC_VOU)
            Me.add(_POSTL_FLEX_EXP_DTE)
            Me.add(_POSTL_PCMS_STAT)
            Me.add(_POSTL_PCMS_UPD_DTE)
            Me.add(_POSTL_PCMS_RMK)
        End Sub
#End Region

#Region "Get Filter Information Structure"
        Public Sub getPayment()
            Dim oCondition As Condition

            Me.clearFilter()

            '(( PNACD_PCMS_STAT Is Null ) 
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = POSTL._POSTL_PCMS_STAT
            oCondition.Operation = Condition.eOperation.op_IS_NULL
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            'Or ( PNACD_PCMS_STAT = ''))
            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = POSTL._POSTL_PCMS_STAT
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = ""
            oCondition.BracketCloseNum = 2
            Me.addFilter(oCondition)
        End Sub

        Public Sub getPayment(ByVal pKeyEntry As String)
            Dim oCondition As Condition

            Me.clearFilter()

            '(( PNACD_PCMS_STAT Is Null ) 
            oCondition = New Condition
            oCondition.BracketOpenNum = 1
            oCondition.Alias = POSTL._POSTL_FLEX_EXP_DTE
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pKeyEntry
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)
        End Sub
#End Region

#Region "Assign Value"
        Public WriteOnly Property PCMS_Status() As String
            Set(ByVal value As String)
                If Not Me.modify(_POSTL_PCMS_STAT, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[PCMS Status S/E]")
                End If
            End Set
        End Property
        Public WriteOnly Property PCMS_UpdateDate() As Date
            Set(ByVal value As Date)
                If Not Me.modify(_POSTL_PCMS_UPD_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[PCMS Update Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property PCMS_Remark() As String
            Set(ByVal value As String)
                If Not Me.modify(_POSTL_PCMS_RMK, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[PCMS Remark]")
                End If
            End Set
        End Property
#End Region

    End Class
End Namespace

