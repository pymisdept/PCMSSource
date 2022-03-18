Namespace SQL.Condition
    Public Class Conditions

        Private _Condition As Collection

        Public Sub New()
            _Condition = New Collection
        End Sub

        Public Sub add(ByVal pCondition As Condition)
            _Condition.Add(pCondition, _Condition.Count)
        End Sub

        Public Property item(ByVal pKey As Integer) As Condition
            Get
                Return _Condition.Item(pKey)
            End Get
            Set(ByVal value As Condition)
                If _Condition.Contains(pKey) Then
                    _Condition.Remove(pKey)
                    _Condition.Add(value, pKey)
                End If
            End Set
        End Property

    End Class
End Namespace