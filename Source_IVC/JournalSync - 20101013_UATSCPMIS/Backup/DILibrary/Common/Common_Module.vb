Imports System.Security.Cryptography

Module Common_Module
    Public Log_Path As String, LogFileName As String
    Public settingPath As String

    Public oSetting As CPS.DI.Interface.Setting

    Enum dbServerType
        db_MSSQL = 1
        db_MSSQL_2005 = 4
    End Enum

    Enum db_UserTrusted
        dbTrue = 0
        dbFalse = 1
    End Enum

    Public Sub CreateLog(Optional ByVal pFilePath As String = "", _
                         Optional ByVal pFileName As String = "Transaction")
        Dim oDirectoryInfo As System.IO.DirectoryInfo
        Dim oFileInfo As System.IO.FileInfo
        Dim strPath As String

        If pFilePath = "" Then
            strPath = System.Reflection.Assembly.GetExecutingAssembly.Location
            strPath = Left(strPath, InStrRev(strPath, "\"))
        Else
            strPath = pFilePath
            If Not Right(strPath, 1) = "\" Then
                strPath = strPath & "\"
            End If
        End If

        oDirectoryInfo = New System.IO.DirectoryInfo(strPath)

        If oDirectoryInfo.Name = "Log" Then
            'If user set the log path into currnet folder
            strPath = oDirectoryInfo.Parent.FullName
        Else
            strPath = oDirectoryInfo.FullName
        End If

        oDirectoryInfo = New System.IO.DirectoryInfo(strPath & "Log")

        If Not oDirectoryInfo.Exists Then
            oDirectoryInfo.Create()
        End If

        If Not System.IO.Directory.Exists(oDirectoryInfo.FullName & Format(Now, "yyyyMMdd")) Then
            oDirectoryInfo = System.IO.Directory.CreateDirectory(oDirectoryInfo.FullName & "\" & Format(Now, "yyyyMMdd"))
        Else
            oDirectoryInfo = New System.IO.DirectoryInfo(oDirectoryInfo.FullName & Format(Now, "yyyyMMdd"))
        End If

        Log_Path = oDirectoryInfo.FullName

        If oDirectoryInfo.Exists Then
            LogFileName = Right("0000" & Str(oDirectoryInfo.GetFiles.Length + 1).Trim, 4) & "-" & pFileName.Trim & ".log"
        Else
            LogFileName = "0001-" & pFileName.Trim & ".log"
        End If

        oFileInfo = New System.IO.FileInfo(oDirectoryInfo.FullName & "\" & LogFileName)
        If Not oFileInfo.Exists Then
            oFileInfo.CreateText().Close()
        End If

    End Sub

    Public Sub LogMessage(ByVal strLogMsg As String)
        Dim oStreamWriter As System.IO.StreamWriter

        oStreamWriter = System.IO.File.AppendText(Log_Path & "\" & LogFileName)
        oStreamWriter.WriteLine(strLogMsg)
        oStreamWriter.Flush()
        oStreamWriter.Close()
    End Sub

#Region "Value Checking"
    'place this part in a module
    Public Function IsIP(ByVal TestAddress As String) As Boolean

        Dim IPt As String
        Dim TQ As Long
        Dim TT As Long
        Dim TW As Long
        Dim IPTemp As Long

        IsIP = False  'Set return value as false

        On Error GoTo cockup
        'if an error occures the string is not valid

        If Left(TestAddress, 1) = "." Then Exit Function
        If Right(TestAddress, 1) = "." Then Exit Function
        'check first and last are not "."

        For TQ = 1 To Len(TestAddress)     'test all chars
            IPt = Mid(TestAddress, TQ, 1)
            If IPt <> "." Then   'if its not a "." it must be 0-9
                If Asc(IPt) > 57 Or Asc(IPt) < 48 Then Exit Function
            End If
        Next TQ

        TQ = InStr(1, TestAddress, ".", vbTextCompare)
        'find the three dots
        TT = InStr(TQ + 1, TestAddress, ".", vbTextCompare)
        TW = InStr(TT + 1, TestAddress, ".", vbTextCompare)
        If InStr(TW + 1, TestAddress, ".", vbTextCompare) <> 0 Then Exit Function
        'if there is a fourth then the string is invalid

        'test each number is between 0 and 255
        IPTemp = Val(Left(TestAddress, TQ - 1))
        If IPTemp > 255 Or IPTemp < 0 Then Exit Function

        IPTemp = Val(Mid(TestAddress, TQ + 1, TT - TQ - 1))
        If IPTemp > 255 Or IPTemp < 0 Then Exit Function

        IPTemp = Val(Mid(TestAddress, TT + 1, TW - TT - 1))
        If IPTemp > 255 Or IPTemp < 0 Then Exit Function

        IPTemp = Val(Right(TestAddress, Len(TestAddress) - TW))
        If IPTemp > 255 Or IPTemp < 0 Then Exit Function

        IsIP = True 'it has passed all tests so make it true

cockup:
    End Function
#End Region

#Region "Data Checking"
    Public Function EncryptData(ByVal plaintext As String) As String

        ' Convert the plaintext string to a byte array.
        Dim plaintextBytes() As Byte = _
            System.Text.Encoding.Unicode.GetBytes(plaintext)

        ' Create the stream.
        Dim ms As New System.IO.MemoryStream
        ' Create the encoder to write to the stream.
        Dim encStream As New CryptoStream(ms, _
            TripleDES.Create(), _
            System.Security.Cryptography.CryptoStreamMode.Write)

        ' Use the crypto stream to write the byte array to the stream.
        encStream.Write(plaintextBytes, 0, plaintextBytes.Length)
        encStream.FlushFinalBlock()

        ' Convert the encrypted stream to a printable string.
        Return Convert.ToBase64String(ms.ToArray)
    End Function

    Public Function DecryptData(ByVal encryptedtext As String) As String

        ' Convert the encrypted text string to a byte array.
        Dim encryptedBytes() As Byte = Convert.FromBase64String(encryptedtext)

        ' Create the stream.
        Dim ms As New System.IO.MemoryStream
        ' Create the decoder to write to the stream.
        Dim decStream As New CryptoStream(ms, _
            TripleDES.Create(), _
            System.Security.Cryptography.CryptoStreamMode.Write)

        ' Use the crypto stream to write the byte array to the stream.
        decStream.Write(encryptedBytes, 0, encryptedBytes.Length)
        decStream.FlushFinalBlock()

        ' Convert the plaintext stream to a string.
        Return System.Text.Encoding.Unicode.GetString(ms.ToArray)
    End Function
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

#End Region

End Module
