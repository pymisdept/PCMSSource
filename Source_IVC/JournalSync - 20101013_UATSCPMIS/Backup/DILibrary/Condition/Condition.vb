Namespace SQL.Condition
    Public Class Condition

        Public Sub New()
            _Alias = ""
            _Operation = eOperation.op_EQUAL
            _Relation = eRelation.re_NONE
            _Value = ""
            _ValueEnd = ""
        End Sub

#Region "Declare Variable"
        Private _Alias As String, _Value As String, _ValueEnd As String
        Private _BracketOpenNum, _BracketCloseNum As Integer
        Private _Operation As eOperation
        Private _Relation As eRelation
#End Region

#Region "Input Parameter"
        Public Property BracketOpenNum() As Integer
            Get
                Return _BracketOpenNum
            End Get
            Set(ByVal value As Integer)
                _BracketOpenNum = value
            End Set
        End Property
        Public Property [Alias]() As String
            Get
                Return _Alias
            End Get
            Set(ByVal value As String)
                _Alias = value
            End Set
        End Property
        Public Property Operation() As eOperation
            Get
                Return _Operation
            End Get
            Set(ByVal value As eOperation)
                _Operation = value
            End Set
        End Property
        Public Property [Value]() As String
            Get
                Return _Value
            End Get
            Set(ByVal value As String)
                _Value = value
            End Set
        End Property
        Public Property ValueEnd() As String
            Get
                Return _ValueEnd
            End Get
            Set(ByVal value As String)
                _ValueEnd = value
            End Set
        End Property
        Public Property BracketCloseNum() As Integer
            Get
                Return _BracketCloseNum
            End Get
            Set(ByVal value As Integer)
                _BracketCloseNum = value
            End Set
        End Property
        Public Property Relation() As eRelation
            Get
                Return _Relation
            End Get
            Set(ByVal value As eRelation)
                _Relation = value
            End Set
        End Property
#End Region

#Region "Data Structure"
        Public Enum eOperation
            op_EQUAL
            op_GRATER_THAN
            op_LESS_THAN
            op_GRATER_EQUAL
            op_LESS_EQUAL
            op_NOT_EQUAL
            op_CONTAIN
            op_NOT_CONTAIN
            op_START
            op_END
            op_BETWEEN
            op_NOT_BETWEEN
            op_IS_NULL
            op_NOT_NULL
        End Enum
        Public Enum eRelation
            re_AND
            re_OR
            re_NONE
        End Enum
#End Region

#Region "Return Value"
        Public Function spOperate() As String
            Select Case _Operation
                Case eOperation.op_EQUAL
                    Return " = "
                Case eOperation.op_NOT_EQUAL
                    Return " <> "
                Case eOperation.op_IS_NULL
                    Return " Is Null "
                Case eOperation.op_NOT_NULL
                    Return " Is Not Null "
                Case eOperation.op_CONTAIN
                    Return " Like "
                Case eOperation.op_START
                    Return " Like "
                Case eOperation.op_END
                    Return " Like "
                Case eOperation.op_NOT_CONTAIN
                    Return " Not Like "
                Case eOperation.op_LESS_THAN
                    Return " < "
                Case eOperation.op_LESS_EQUAL
                    Return " <= "
                Case eOperation.op_GRATER_THAN
                    Return " > "
                Case eOperation.op_GRATER_EQUAL
                    Return " >= "
                Case eOperation.op_BETWEEN
                    Return " Between "
                Case eOperation.op_NOT_BETWEEN
                    Return " Not Between "
                Case Else
                    Return ""
            End Select
        End Function

        Public Function spRelation() As String
            Select Case _Relation
                Case eRelation.re_AND
                    Return " AND "
                Case eRelation.re_OR
                    Return " OR "
                Case eRelation.re_NONE
                    Return "  "
                Case Else
                    Return "  "
            End Select
        End Function
#End Region

    End Class
End Namespace