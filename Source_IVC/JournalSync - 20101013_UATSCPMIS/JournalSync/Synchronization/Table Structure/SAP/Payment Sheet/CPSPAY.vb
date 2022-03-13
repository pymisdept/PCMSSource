Imports CPS.SQL.Condition

Namespace Datatable.SAP.PCMS
    Public Class CPSPAY
        Inherits CPS.SQL.Interface.RecordSet

        Private ReadOnly CheckPoint As String = "Sync_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

        Public Sub New()
            MyBase.New(TableName)
            Me.add(_COMP_CDE)
            Me.add(_ACC_CDE)
            Me.add(_STL_DOC_NUM)
            Me.add(_STL_DOC_DTE)
            Me.add(_STL_DOC_TYP)
            Me.add(_STL_DOC_ANA1)
            Me.add(_STL_DOC_ANA2)
            Me.add(_STL_DOC_ANA3)
            Me.add(_STL_DOC_ANA4)
            Me.add(_STL_DOC_ANA5)
            Me.add(_APP_DOC_NUM)
            Me.add(_STL_AMT)
            Me.add(_APP_DOC_ANA1)
            Me.add(_APP_DOC_ANA2)
            Me.add(_APP_DOC_ANA3)
            Me.add(_APP_DOC_ANA4)
            Me.add(_APP_DOC_ANA5)
            Me.add(_CCY_CDE)
            Me.add(_EXC_RAT)
            Me.add(_STL_DOC_VOU)
            Me.add(_APP_DOC_VOU)
            Me.add(_FLEX_EXP_DTE)
        End Sub

#Region "Constanst Value"
        Public Const TableName As String = "CPSPAY"
        Public Const ObjType As String = "24"

        Public Const _COMP_CDE As String = "COMP_CDE"
        Public Const _ACC_CDE As String = "ACC_CDE"
        Public Const _STL_DOC_NUM As String = "STL_DOC_NUM"
        Public Const _STL_DOC_DTE As String = "STL_DOC_DTE"
        Public Const _STL_DOC_TYP As String = "STL_DOC_TYP"
        Public Const _STL_DOC_ANA1 As String = "STL_DOC_ANA1"
        Public Const _STL_DOC_ANA2 As String = "STL_DOC_ANA2"
        Public Const _STL_DOC_ANA3 As String = "STL_DOC_ANA3"
        Public Const _STL_DOC_ANA4 As String = "STL_DOC_ANA4"
        Public Const _STL_DOC_ANA5 As String = "STL_DOC_ANA5"
        Public Const _APP_DOC_NUM As String = "APP_DOC_NUM"
        Public Const _STL_AMT As String = "STL_AMT"
        Public Const _APP_DOC_ANA1 As String = "APP_DOC_ANA1"
        Public Const _APP_DOC_ANA2 As String = "APP_DOC_ANA2"
        Public Const _APP_DOC_ANA3 As String = "APP_DOC_ANA3"
        Public Const _APP_DOC_ANA4 As String = "APP_DOC_ANA4"
        Public Const _APP_DOC_ANA5 As String = "APP_DOC_ANA5"
        Public Const _CCY_CDE As String = "CCY_CDE"
        Public Const _EXC_RAT As String = "EXC_RAT"
        Public Const _STL_DOC_VOU As String = "STL_DOC_VOU"
        Public Const _APP_DOC_VOU As String = "APP_DOC_VOU"
        Public Const _FLEX_EXP_DTE As String = "FLEX_EXP_DTE"
#End Region

#Region "Assign Value"
        Public WriteOnly Property COMP_CDE() As String
            Set(ByVal value As String)
                If Not Me.modify(_COMP_CDE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Company Code]")
                End If
            End Set
        End Property
        Public WriteOnly Property ACC_CDE() As String
            Set(ByVal value As String)
                If Not Me.modify(_ACC_CDE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Account Code]")
                End If
            End Set
        End Property
        Public WriteOnly Property STL_DOC_NUM() As String
            Set(ByVal value As String)
                If Not Me.modify(_STL_DOC_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Settlement document number]")
                End If
            End Set
        End Property
        Public WriteOnly Property STL_DOC_DTE() As Date
            Set(ByVal value As Date)
                If Not Me.modify(_STL_DOC_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Settlement document date]")
                End If
            End Set
        End Property
        Public WriteOnly Property STL_DOC_TYP() As String
            Set(ByVal value As String)
                If Not Me.modify(_STL_DOC_TYP, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Settlement document type]")
                End If
            End Set
        End Property
        Public WriteOnly Property STL_DOC_ANA1() As String
            Set(ByVal value As String)
                If Not Me.modify(_STL_DOC_ANA1, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Analysis Code 1]")
                End If
            End Set
        End Property
        Public WriteOnly Property STL_DOC_ANA2() As String
            Set(ByVal value As String)
                If Not Me.modify(_STL_DOC_ANA2, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Analysis Code 2]")
                End If
            End Set
        End Property
        Public WriteOnly Property STL_DOC_ANA3() As String
            Set(ByVal value As String)
                If Not Me.modify(_STL_DOC_ANA3, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Analysis Code 3]")
                End If
            End Set
        End Property
        Public WriteOnly Property STL_DOC_ANA4() As String
            Set(ByVal value As String)
                If Not Me.modify(_STL_DOC_ANA4, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Analysis Code 4]")
                End If
            End Set
        End Property
        Public WriteOnly Property STL_DOC_ANA5() As String
            Set(ByVal value As String)
                If Not Me.modify(_STL_DOC_ANA5, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Analysis Code 4]")
                End If
            End Set
        End Property
        Public WriteOnly Property APP_DOC_NUM() As String
            Set(ByVal value As String)
                If Not Me.modify(_APP_DOC_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Apply document number (PK)]")
                End If
            End Set
        End Property
        Public WriteOnly Property STL_AMT() As Double
            Set(ByVal value As Double)
                If Not Me.modify(_STL_AMT, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Settlement amount]")
                End If
            End Set
        End Property
        Public WriteOnly Property APP_DOC_ANA1() As String
            Set(ByVal value As String)
                If Not Me.modify(_APP_DOC_ANA1, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Analysis code 1]")
                End If
            End Set
        End Property
        Public WriteOnly Property APP_DOC_ANA2() As String
            Set(ByVal value As String)
                If Not Me.modify(_APP_DOC_ANA2, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Analysis code 2]")
                End If
            End Set
        End Property
        Public WriteOnly Property APP_DOC_ANA3() As String
            Set(ByVal value As String)
                If Not Me.modify(_APP_DOC_ANA3, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Analysis code 3]")
                End If
            End Set
        End Property
        Public WriteOnly Property APP_DOC_ANA4() As String
            Set(ByVal value As String)
                If Not Me.modify(_APP_DOC_ANA4, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Analysis code 4]")
                End If
            End Set
        End Property
        Public WriteOnly Property APP_DOC_ANA5() As String
            Set(ByVal value As String)
                If Not Me.modify(_APP_DOC_ANA5, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Analysis code 5]")
                End If
            End Set
        End Property
        Public WriteOnly Property CCY_CDE() As String
            Set(ByVal value As String)
                If Not Me.modify(_CCY_CDE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Currency Code]")
                End If
            End Set
        End Property
        Public WriteOnly Property EXC_RAT() As Double
            Set(ByVal value As Double)
                If Not Me.modify(_EXC_RAT, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Exchange rate]")
                End If
            End Set
        End Property
        Public WriteOnly Property STL_DOC_VOU() As String
            Set(ByVal value As String)
                If Not Me.modify(_STL_DOC_VOU, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Settlement document's voucher number]")
                End If
            End Set
        End Property
        Public WriteOnly Property APP_DOC_VOU() As String
            Set(ByVal value As String)
                If Not Me.modify(_APP_DOC_VOU, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Apply document's voucher number]")
                End If
            End Set
        End Property
        Public WriteOnly Property FLEX_EXP_DTE() As Date
            Set(ByVal value As Date)
                If Not Me.modify(_FLEX_EXP_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Flex export date]")
                End If
            End Set
        End Property
#End Region

#Region "Get Filter Information Structure"
        Public Sub getPayment(ByVal pCompCDE As String, _
                              ByVal pAccCDE As String, _
                              ByVal pStlDocNum As String, _
                              ByVal pAppDocNum As String)
            Dim oCondition As Condition

            Me.clearFilter()

            '( COMP_CDE = '' ) 
            oCondition = New Condition
            oCondition.BracketOpenNum = 1
            oCondition.Alias = CPSPAY._COMP_CDE
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pCompCDE
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = CPSPAY._ACC_CDE
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pAccCDE
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.BracketOpenNum = 1
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.Alias = CPSPAY._STL_DOC_NUM
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pStlDocNum
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            oCondition = New Condition
            oCondition.BracketOpenNum = 1
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.Alias = CPSPAY._APP_DOC_NUM
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pAppDocNum
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

        End Sub
#End Region

    End Class
End Namespace