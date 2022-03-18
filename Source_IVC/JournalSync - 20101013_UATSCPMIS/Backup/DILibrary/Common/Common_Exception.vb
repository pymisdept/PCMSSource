Public Class Common_Exception
    Inherits SystemException

    Private SAP_ErrorCode As String
    Private SAP_ErrorMsgs As String
    Private Sys_ErrorCode As String
    Private Sys_ErrorMsgs As String
    Private mCheck_Point As String
    Private mManual_Msgs As String
    Private mErrorType As ErrorType

    Private Transaction_Type As Integer
    Private Transaction_Entry As String

    Public Enum ErrorType
        Normal = 0
        SAP = 1
        System = 2
    End Enum

    'Define New Operation
    Public Sub New(ByVal pErrorType As ErrorType, _
                   ByRef Check_Point As String, _
                   Optional ByRef manual_Msgs As String = "", _
                   Optional ByVal ErrorCode As String = "", _
                   Optional ByVal ErrorDesc As String = "", _
                   Optional ByVal ObjectType As Integer = Nothing, _
                   Optional ByVal TransEntry As String = Nothing)
        MyBase.New()

        Me.mCheck_Point = Check_Point
        Me.mManual_Msgs = manual_Msgs

        SAP_ErrorCode = ""
        SAP_ErrorMsgs = ""
        Sys_ErrorCode = ""
        Sys_ErrorMsgs = ""

        Select Case pErrorType
            Case ErrorType.Normal
                Sys_ErrorMsgs = ErrorDesc
            Case ErrorType.SAP
                SAP_ErrorCode = ErrorCode
                SAP_ErrorMsgs = ErrorDesc
            Case ErrorType.System
                Sys_ErrorCode = ErrorCode
                Sys_ErrorMsgs = ErrorDesc
        End Select

        mErrorType = pErrorType
        Check_Point = ""
        manual_Msgs = ""

        Transaction_Entry = TransEntry
        Transaction_Type = ObjectType
    End Sub

    Public Function TransEntry() As String
        Return Transaction_Entry
    End Function

    Public Function TransType() As Integer
        Return Transaction_Type
    End Function

    Public Overrides Function toString() As String
        Dim oErrorMessage As String = ""

        Select Case mErrorType
            Case ErrorType.Normal
                oErrorMessage &= vbCrLf & "System Operation - Process Exception"
                oErrorMessage &= vbCrLf & "System Check Point: " & mCheck_Point
                oErrorMessage &= vbCrLf & "System Error code: " & Sys_ErrorCode
                oErrorMessage &= vbCrLf & "System Error message: " & Sys_ErrorMsgs
            Case ErrorType.SAP
                oErrorMessage &= vbCrLf & "SAP Business One - Process Exception"
                oErrorMessage &= vbCrLf & "System Check Point: " & mCheck_Point
                oErrorMessage &= vbCrLf & "SAP Error code: " & SAP_ErrorCode
                oErrorMessage &= vbCrLf & "SAP Error message: " & SAP_ErrorMsgs
            Case ErrorType.System
                oErrorMessage &= vbCrLf & "System Operation - Process Exception"
                oErrorMessage &= vbCrLf & "System Check Point: " & mCheck_Point
                oErrorMessage &= vbCrLf & "System Error code: " & Sys_ErrorCode
                oErrorMessage &= vbCrLf & "System Error message: " & Sys_ErrorMsgs
        End Select
        oErrorMessage &= vbCrLf
        oErrorMessage &= vbCrLf & "Detail Description: --------------------"
        oErrorMessage &= vbCrLf & mManual_Msgs
        oErrorMessage &= vbCrLf & "----------------------------------------"

        Return oErrorMessage
    End Function

End Class