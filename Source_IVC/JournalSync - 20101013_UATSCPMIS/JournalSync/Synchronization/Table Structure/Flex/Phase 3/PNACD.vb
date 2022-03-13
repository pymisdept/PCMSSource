Imports CPS.SQL.Condition

Namespace Datatable.Flex
    Public Class PNACD
        Inherits CPS.SQL.Interface.RecordSet

        Public Sub New()
            MyBase.New(TableName, SubMain.FLEX_DBName)
            Me.add(_PNACD_COMP_CDE)
            Me.add(_PNACD_ANA_CAT)
            Me.add(_PNACD_ANA_CDE)
            Me.add(_PNACD_DES)
            Me.add(_PNACD_ADDR1)
            Me.add(_PNACD_ADDR2)
            Me.add(_PNACD_CON_PSN)
            Me.add(_PNACD_PHO_NUM)
            Me.add(_PNACD_FAX_NUM)
            Me.add(_PNACD_RMK1)
            Me.add(_PNACD_RMK2)
            Me.add(_PNACD_RMK3)
            Me.add(_PNACD_RMK4)
            Me.add(_PNACD_FRO_STAT)
            Me.add(_PNACD_FLEX_EXP_DTE)
            Me.add(_PNACD_PCMS_STAT)
            Me.add(_PNACD_PCMS_UPD_DTE)
            Me.add(_PNACD_PCMS_RMK)
            Me.add(_PNACD_LEGCDE)

            Me.Add_OrderBy(_PNACD_FLEX_EXP_DTE)
        End Sub

#Region "Constanst Value"

        Private ReadOnly CheckPoint As String = "PNACD_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

        Public Const TableName As String = "PNACD"
        Public Const _PNACD_COMP_CDE As String = "PNACD_COMP_CDE"
        Public Const _PNACD_ANA_CAT As String = "PNACD_ANA_CAT"
        Public Const _PNACD_ANA_CDE As String = "PNACD_ANA_CDE"
        Public Const _PNACD_DES As String = "PNACD_DES"
        Public Const _PNACD_ADDR1 As String = "PNACD_ADDR1"
        Public Const _PNACD_ADDR2 As String = "PNACD_ADDR2"
        Public Const _PNACD_CON_PSN As String = "PNACD_CON_PSN"
        Public Const _PNACD_PHO_NUM As String = "PNACD_PHO_NUM"
        Public Const _PNACD_FAX_NUM As String = "PNACD_FAX_NUM"
        Public Const _PNACD_RMK1 As String = "PNACD_RMK1"
        Public Const _PNACD_RMK2 As String = "PNACD_RMK2"
        Public Const _PNACD_RMK3 As String = "PNACD_RMK3"
        Public Const _PNACD_RMK4 As String = "PNACD_RMK4"
        Public Const _PNACD_FRO_STAT As String = "PNACD_FRO_STAT"
        Public Const _PNACD_FLEX_EXP_DTE As String = "PNACD_FLEX_EXP_DTE"
        Public Const _PNACD_PCMS_STAT As String = "PNACD_PCMS_STAT"
        Public Const _PNACD_PCMS_UPD_DTE As String = "PNACD_PCMS_UPD_DTE"
        Public Const _PNACD_PCMS_RMK As String = "PNACD_PCMS_RMK"
        Public Const _PNACD_LEGCDE As String = "PNACD_LEG_CDE1"

#End Region

#Region "Get Filter Information Structure"
        Public Sub getBusinessPartnerData()
            Dim oCondition As Condition

            Me.clearFilter()

            '(( PNACD_PCMS_STAT Is Null ) 
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PNACD._PNACD_PCMS_STAT
            oCondition.Operation = Condition.eOperation.op_IS_NULL
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            'Or ( PNACD_PCMS_STAT = ''))
            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PNACD._PNACD_PCMS_STAT
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = ""
            oCondition.BracketCloseNum = 2
            Me.addFilter(oCondition)

            'And ( PNACD_ANA_CAT = '1' )
            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PNACD._PNACD_ANA_CAT
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = "1"
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)
        End Sub

        Public Sub getProjectData()
            Dim oCondition As Condition

            Me.clearFilter()

            '(( PNACD_PCMS_STAT Is Null ) 
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = PNACD._PNACD_PCMS_STAT
            oCondition.Operation = Condition.eOperation.op_IS_NULL
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            'Or ( PNACD_PCMS_STAT = ''))
            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_OR
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PNACD._PNACD_PCMS_STAT
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = ""
            oCondition.BracketCloseNum = 2
            Me.addFilter(oCondition)

            'And ( PNACD_ANA_CAT = '3' )
            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PNACD._PNACD_ANA_CAT
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = "3"
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)
        End Sub

        Public Sub getProcessEntry(ByVal pKeyEntry As String)
            Dim oCondition As Condition

            Me.clearFilter()

            '(( PNACD_PCMS_STAT Is Null ) 
            oCondition = New Condition
            oCondition.BracketOpenNum = 1
            oCondition.Alias = PNACD._PNACD_FLEX_EXP_DTE
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pKeyEntry
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)
        End Sub
#End Region

#Region "Assign Value"
        Public WriteOnly Property PCMS_Status() As String
            Set(ByVal value As String)
                If Not Me.modify(_PNACD_PCMS_STAT, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[PCMS Status S/E]")
                End If
            End Set
        End Property
        Public WriteOnly Property PCMS_UpdateDate() As Date
            Set(ByVal value As Date)
                If Not Me.modify(_PNACD_PCMS_UPD_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[PCMS Update Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property PCMS_Remark() As String
            Set(ByVal value As String)
                If Not Me.modify(_PNACD_PCMS_RMK, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[PCMS Remark]")
                End If
            End Set
        End Property
#End Region

    End Class
End Namespace

