Namespace [Global].Setting.DataStructure
    Class LogMessage
        Private _Key As String
        Private _Description As String
        Private _Status As TransType

        Public Sub New()
            _Key = ""
            _Description = ""
            _Status = Nothing
        End Sub

        Public Property KeyValue() As String
            Get
                Return _Key
            End Get
            Set(ByVal value As String)
                _Key = value
            End Set
        End Property

        Public Property Description() As String
            Get
                Return _Description
            End Get
            Set(ByVal value As String)
                _Description = value
            End Set
        End Property

        Public Property Status() As TransType
            Get
                Return _Status
            End Get
            Set(ByVal value As TransType)
                _Status = value
            End Set
        End Property

        Enum TransType
            tt_Success = 1
            tt_Failure = 2
            tt_Exception = 3
            tt_Description = 4
        End Enum

    End Class
End Namespace