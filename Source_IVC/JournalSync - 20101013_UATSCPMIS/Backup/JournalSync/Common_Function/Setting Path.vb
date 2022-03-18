Public Class Setting_Path

    Private ReadOnly _FileName As String = "Path.ini"

    Private _OINV_Item_Path As String
    Private _OPCH_Item_Path As String
    Private _OPCH_Serv_Path As String
    Private _BusinessPartner As String
    Private _ItemMaster As String
    Private _ProjectCode As String
    Private _IncomingPayment As String
    Private _DirectExpense As String
    Private _PurchaseOrder As String
    Private _SalesOrder As String
    Private _SalesQuotation As String
    Private _ApprovalProcedure As String
    

    Public Sub New()
        Dim strPath As String

        strPath = System.Reflection.Assembly.GetExecutingAssembly.Location
        strPath = Left(strPath, InStrRev(strPath, "\"))
        strPath = strPath & "\" & _FileName


        _OINV_Item_Path = GetINIValue("Phase 1", "Client Payment Certificate Data", strPath)
        _OPCH_Item_Path = GetINIValue("Phase 1", "Sub Contractor Certificate Data", strPath)
        _OPCH_Serv_Path = GetINIValue("Phase 1", "Supplier Payment Certificate Data", strPath)
        _BusinessPartner = GetINIValue("Phase 2", "Business Partner", strPath)
        _ItemMaster = GetINIValue("Phase 2", "Item Master Data", strPath)
        _ProjectCode = GetINIValue("Phase 2", "Project Code", strPath)
        _IncomingPayment = GetINIValue("Phase 3", "Incoming Payment", strPath)
        _DirectExpense = GetINIValue("Phase 4", "Direct Expense", strPath)
        _PurchaseOrder = GetINIValue("Additional Phase", "Purchase Order", strPath)
        _SalesOrder = GetINIValue("Additional Phase", "Sales Order", strPath)
        _SalesQuotation = GetINIValue("Additional Phase", "Sales Quotatoin", strPath)
        _ApprovalProcedure = GetINIValue("Additional Phase", "Approval Procedure", strPath)

    End Sub

#Region "INI Function"

    Private Declare Function GetPrivateProfileSectionNames Lib "kernel32.dll" Alias "GetPrivateProfileSectionNamesA" (ByVal lpszReturnBuffer() As Byte, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    Private Declare Function GetPrivateProfileString Lib "kernel32" Alias "GetPrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpDefault As String, ByVal lpReturnedString As System.Text.StringBuilder, ByVal nSize As Integer, ByVal lpFileName As String) As Integer
    Private Declare Function WritePrivateProfileString Lib "kernel32" Alias "WritePrivateProfileStringA" (ByVal lpApplicationName As String, ByVal lpKeyName As String, ByVal lpString As String, ByVal lpFileName As String) As Integer

    Const MAX_ENTRY As Integer = 32768

    Public Function GetINIValue(ByVal sSection As String, ByVal sVariableName As String, ByVal sFilename As String) As String
        Try
            Dim sb As New System.Text.StringBuilder(MAX_ENTRY)
            Dim intRetVal As Integer = GetPrivateProfileString(sSection, sVariableName, "", sb, MAX_ENTRY, sFilename)
            Return sb.ToString
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Shared Function INIValue(ByVal sSection As String, ByVal sVariableName As String, ByVal sFilename As String) As String
        Try
            Dim sb As New System.Text.StringBuilder(MAX_ENTRY)
            Dim intRetVal As Integer = GetPrivateProfileString(sSection, sVariableName, "", sb, MAX_ENTRY, sFilename)
            Return sb.ToString
        Catch ex As Exception
            Return ""
        End Try
    End Function

    Public Function WriteINIValue(ByVal sSection As String, ByVal sVariableName As String, ByVal sValue As String, ByVal sFilename As String) As Boolean
        Try
            WritePrivateProfileString(sSection, sVariableName, sValue, sFilename)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function DelteINIValue(ByVal sSection As String, ByVal sVariableName As String, ByVal sFilename As String) As Boolean
        Try
            WritePrivateProfileString(sSection, sVariableName, vbNullString, sFilename)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function AddINISection(ByVal sSection As String, ByVal sFilename As String, Optional ByVal sVariableName As String = Nothing) As Boolean
        Try
            WritePrivateProfileString(sSection, sVariableName, vbNullString, sFilename)
            Return True
        Catch ex As Exception
            Return False
        End Try

    End Function

    Public Function DeleteINISection(ByVal sSection As String, ByVal sFileName As String) As Boolean
        Try
            WritePrivateProfileString(sSection, vbNullString, vbNullString, sFileName)
            Return True
        Catch ex As Exception
            Return False
        End Try
    End Function

    Public Function GetINISections(ByVal sFilename As String) As ArrayList
        GetINISections = New ArrayList
        Dim bBuffer(MAX_ENTRY) As Byte
        Dim strBuffer As String
        Dim intPrevPos As Integer = 0
        Dim intLength As Integer
        Try
            intLength = GetPrivateProfileSectionNames(bBuffer, MAX_ENTRY, sFilename)
        Catch
            Exit Function
        End Try
        Dim ASCII As New System.Text.ASCIIEncoding
        If intLength > 0 Then
            strBuffer = ASCII.GetString(bBuffer)
            intLength = 0
            intPrevPos = -1
            Do
                intLength = strBuffer.IndexOf(ControlChars.NullChar, intPrevPos + 1)
                If intLength - intPrevPos = 1 OrElse intLength = -1 Then Exit Do
                Try
                    GetINISections.Add(strBuffer.Substring(intPrevPos + 1, intLength - intPrevPos))
                Catch
                End Try
                intPrevPos = intLength
            Loop
        End If
    End Function

#End Region

End Class
