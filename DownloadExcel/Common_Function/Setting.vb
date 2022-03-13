Namespace Excel.Setting
    Public Class Setting

        Private ReadOnly _FileName As String = "[SETTING]EXCEL.ini"
        Private _FileFolder As String, _TempFolder As String, _AddinPath As String, _LogFolder As String, _ServerType As String
        Private _Password As String

        Public Sub New()
            Dim strPath As String

            strPath = System.Reflection.Assembly.GetExecutingAssembly.Location
            strPath = Left(strPath, InStrRev(strPath, "\"))
            strPath = strPath & "\" & _FileName

            _FileFolder = GetINIValue("System Information", "IMPORT FOLDER", strPath)
            _TempFolder = GetINIValue("System Information", "EXPORT FOLDER", strPath)
            _AddinPath = GetINIValue("System Information", "ADD-IN FOLDER", strPath)
            _LogFolder = GetINIValue("System Information", "LOG FOLDER", strPath)

            If _LogFolder = "" Then
                _LogFolder = strPath
            End If

            _Password = GetINIValue("System Information", "PRODUCTION PASSWORD", strPath)

            '''' Added on 2018-07-13
            _ServerType = GetINIValue("System Information", "PCMS SERVER TYPE", strPath)

            ''' HK-T = HK Testing Server
            ''' HK = HK Production Server
            ''' MSC = MSC Server
            ''' TW = Tai Wah Server

        End Sub

        Public ReadOnly Property Export_Path() As String
            Get
                Return _TempFolder
            End Get
        End Property

        Public ReadOnly Property Import_Path() As String
            Get
                Return _FileFolder
            End Get
        End Property

        Public ReadOnly Property Addin_Path() As String
            Get
                Return _AddinPath
            End Get
        End Property

        Public ReadOnly Property Log_Path() As String
            Get
                Return _LogFolder
            End Get
        End Property

        Public ReadOnly Property Password() As String
            Get
                Return _Password
            End Get
        End Property

        Public ReadOnly Property ServerType() As String
            Get
                Return _ServerType
            End Get
        End Property

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

    End Class
End Namespace