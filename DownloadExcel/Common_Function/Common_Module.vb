Module Common_Module
    'Setting ini Section
    Public Const OperationSettings As String = "Operation"
    Public Log_Path As String, LogFileName As String

    ' Kill Task Variable
    Public oTaskKill As String = "TASKKILL"
    Public oProcessName As String = "PROCESSNAME"
    Public oParameter As String = "PARAMETER"
    ' Kill Task Value 

    Public TaskKill As Boolean = False
    Public ProcessName As String = "Excel.exe"
    Public AdditParameter As String = ""


    Public settingPath As String

    Enum dbServerType
        db_MSSQL = 1
        db_MSSQL_2005 = 4
    End Enum

    Enum db_UserTrusted
        dbTrue = 0
        dbFalse = 1
    End Enum

#Region "Task Kill"
    Public Sub OperationSetup()
        TaskKill = (GetINIValue(OperationSettings, oTaskKill, System.Environment.CurrentDirectory & "\[SETTING]EXCEL.ini") = "Y")
        AdditParameter = GetINIValue(OperationSettings, oParameter, System.Environment.CurrentDirectory & "\[SETTING]EXCEL.ini")


    End Sub
#End Region
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
    Public Function isProtectSheet(ByVal type As String) As Boolean

        'Dim _protectws As String = GetINIValue("Excel Information", "PROTECTID", System.Environment.CurrentDirectory & "\[SETTING]EXCEL.ini")
        'Dim _nportectws As String = GetINIValue("Excel Information", "NOT PROTECTID", System.Environment.CurrentDirectory & "\[SETTING]EXCEL.ini")
        'Dim _sw As New System.IO.StreamWriter("c:\Debug.txt", True)
        'Try
        '    _sw.WriteLine("Currency Directory: " & System.Environment.CurrentDirectory)
        '    _sw.WriteLine("Protect ID: " & _protectws)
        '    _sw.WriteLine("Not Protect ID: " & _nportectws)
        'Catch ex As Exception

        'End Try
        'If _protectws.IndexOf(type) >= 0 Then
        '    _sw.WriteLine(type & ": Protect")
        '    _sw.Close()
        '    Return True
        'Else
        '    _sw.WriteLine(type & ": No Protect")
        '    _sw.Close()
        '    If _nportectws.IndexOf(type) >= 0 Then
        '        Return False
        '    Else
        '        Return False
        '    End If
        'End If
        Return True
    End Function
#End Region

End Module
