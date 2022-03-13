Public Class MapType
    Public Const SplitCode As String = "|||||"
    Public Enum IDE_Type
        IDE_VBNET
        IDE_VB6
    End Enum

#Region "Return Data Type"

    Public Function mString(ByVal pType As IDE_Type) As String
        Select Case pType
            Case IDE_Type.IDE_VBNET
                Return "String"
            Case IDE_Type.IDE_VB6
                Return "S"
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function mDateTime(ByVal pType As IDE_Type) As String
        Select Case pType
            Case IDE_Type.IDE_VBNET
                Return "Date"
            Case IDE_Type.IDE_VB6
                Return "D"
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function mDecimal(ByVal pType As IDE_Type) As String
        Select Case pType
            Case IDE_Type.IDE_VBNET
                Return "Decimal"
            Case IDE_Type.IDE_VB6
                Return "N"
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function mDouble(ByVal pType As IDE_Type) As String
        Select Case pType
            Case IDE_Type.IDE_VBNET
                Return "Double"
            Case IDE_Type.IDE_VB6
                Return "N"
            Case Else
                Return Nothing
        End Select
    End Function

    Public Function mInteger(ByVal pType As IDE_Type) As String
        Select Case pType
            Case IDE_Type.IDE_VBNET
                Return "Integer"
            Case IDE_Type.IDE_VB6
                Return "I"
            Case Else
                Return Nothing
        End Select
    End Function

#End Region

End Class