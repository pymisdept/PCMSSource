Imports CPS.CPSLIB.Debug
Namespace DI.Interface

    Public MustInherit Class Connection
        Implements IDisposable

        Private _Server As String
        Private _DBUserID As String
        Private _DBUserPW As String
        Private _DBName As String
        Private _Connection As SqlClient.SqlConnection
        Private _Transaction As SqlClient.SqlTransaction
        Private _Connected As Boolean

        Private _BeginTrans As Boolean

        Public Sub New()
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
            _BeginTrans = False
            _Transaction = Nothing
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
        End Sub

#Region "Define Connection Parameter"
        Public Property Server() As String
            Get
                Return _Server
            End Get
            Set(ByVal value As String)
                _Server = value.Trim
            End Set
        End Property
        Public Property DBUserID() As String
            Get
                Return _DBUserID
            End Get
            Set(ByVal value As String)
                _DBUserID = value.Trim
            End Set
        End Property
        Public Property DBUserPW() As String
            Get
                Return _DBUserPW
            End Get
            Set(ByVal value As String)
                _DBUserPW = value.Trim
            End Set
        End Property
        Public Property DBName() As String
            Get
                Return _DBName
            End Get
            Set(ByVal value As String)
                _DBName = value.Trim
            End Set
        End Property
#End Region

#Region "Connection To Database"
        Public Sub Connect()

            Dim oConnectionString As String

            oConnectionString = "Data Source=" + _Server + ";"
            oConnectionString += "User ID=" + _DBUserID + ";"
            oConnectionString += "Password=" + _DBUserPW + ";"
            oConnectionString += "Initial Catalog=" + _DBName
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Connection String: " & oConnectionString, TimeSet.Status.Start)
            _Connection = New SqlClient.SqlConnection
            _Connection.ConnectionString = oConnectionString


            Try
                _Connection.Open()
                _Connected = True

            Catch ex As Exception
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception on Connect: " & ex.Message, TimeSet.Status.Start)
                Throw New Common_Exception(Common_Exception.ErrorType.Normal, "PCMS_Connect", ex.ToString)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Exception on Connect: " & ex.Message, TimeSet.Status.Finish)
            End Try
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Connection String: " & oConnectionString, TimeSet.Status.Start)
        End Sub

        Public Sub Disconnect()
            _Connection.Close()
            _Connection.Dispose()
            _Connection = Nothing
            _Connected = False
        End Sub
#End Region

#Region "Transaction Contraction"
        Public ReadOnly Property InTransaction() As Boolean
            Get
                Return _BeginTrans
            End Get
        End Property

        Public Function StartTransaction() As Boolean
            _BeginTrans = True
            _Transaction = _Connection.BeginTransaction()
        End Function

        Public Function EndTransaction(ByVal pStatus As TransStatus) As Boolean
            _BeginTrans = False

            Select Case pStatus
                Case TransStatus.ts_Commit
                    _Transaction.Commit()
                Case TransStatus.ts_Rollback
                    _Transaction.Rollback()
            End Select
        End Function
#End Region

#Region "Query Execute"
        Public Sub Process(ByVal pSqlStr As String)
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Query" & pSqlStr, TimeSet.Status.Start)
            Dim oSqlCommand As SqlClient.SqlCommand

            Try
                oSqlCommand = New SqlClient.SqlCommand
                oSqlCommand.Connection = _Connection
                If _BeginTrans Then
                    oSqlCommand.Transaction = _Transaction
                End If
                oSqlCommand.CommandText = pSqlStr
                oSqlCommand.ExecuteNonQuery()
                oSqlCommand.Dispose()
            Catch ex As Exception
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Exception: " & ex.Message, TimeSet.Status.Start)
                Dim alertMessage As String = ""

                alertMessage &= "Unable to Process [Advance] Query" & vbCrLf
                alertMessage &= "--------------------------------------------" & vbCrLf
                alertMessage &= pSqlStr
                alertMessage &= "--------------------------------------------" & vbCrLf

                Throw New Common_Exception(Common_Exception.ErrorType.System, _
                                        "PCMS_PROCESS", _
                                        alertMessage, _
                                        "-999", _
                                        ex.ToString)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Exception: " & ex.Message, TimeSet.Status.Finish)
            End Try
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Query" & pSqlStr, TimeSet.Status.Finish)
        End Sub

        Public Function Execute(ByVal pSqlStr As String) As Object
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Query" & pSqlStr, TimeSet.Status.Start)
            Dim oSqlCommand As SqlClient.SqlCommand

            Try
                oSqlCommand = New SqlClient.SqlCommand
                oSqlCommand.Connection = _Connection
                If _BeginTrans Then
                    oSqlCommand.Transaction = _Transaction
                End If
                oSqlCommand.CommandText = pSqlStr
                oSqlCommand.CommandTimeout = 0
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Query" & pSqlStr, TimeSet.Status.Finish)
                Return oSqlCommand.ExecuteScalar
            Catch ex As Exception
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Exception: " & ex.Message, TimeSet.Status.Start)
                Dim alertMessage As String = ""

                alertMessage &= "Unable to Execute [Select] Query" & vbCrLf
                alertMessage &= "--------------------------------------------" & vbCrLf
                alertMessage &= pSqlStr
                alertMessage &= "--------------------------------------------" & vbCrLf

                Throw New Common_Exception(Common_Exception.ErrorType.System, _
                                        "PCMS_PROCESS", _
                                        alertMessage, _
                                        "-998", _
                                        ex.ToString)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Exception: " & ex.Message, TimeSet.Status.Finish)
            End Try

        End Function

        Public Function Executes(ByVal pSqlStr As String) As SqlClient.SqlDataReader
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Query: " & pSqlStr, TimeSet.Status.Start)
            Dim oSqlCommand As SqlClient.SqlCommand

            Try
                oSqlCommand = New SqlClient.SqlCommand
                oSqlCommand.Connection = _Connection
                If _BeginTrans Then
                    oSqlCommand.Transaction = _Transaction
                End If
                oSqlCommand.CommandText = pSqlStr
                oSqlCommand.CommandTimeout = 0
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Query: " & pSqlStr, TimeSet.Status.Finish)
                Return oSqlCommand.ExecuteReader
            Catch ex As Exception
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Exception: " & ex.Message, TimeSet.Status.Start)
                Dim alertMessage As String = ""

                alertMessage &= "Unable to Execute [Select] Query" & vbCrLf
                alertMessage &= "--------------------------------------------" & vbCrLf
                alertMessage &= pSqlStr
                alertMessage &= "--------------------------------------------" & vbCrLf

                Throw New Common_Exception(Common_Exception.ErrorType.System, _
                                        "PCMS_PROCESS", _
                                        alertMessage, _
                                        "-998", _
                                        ex.ToString)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Exception: " & ex.Message, TimeSet.Status.Finish)
            End Try
        End Function

        Public Function Adapter(ByVal pSqlStr As String) As SqlClient.SqlDataAdapter
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Query: " & pSqlStr, TimeSet.Status.Start)
            Try
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Query: " & pSqlStr, TimeSet.Status.Finish)
                Return New SqlClient.SqlDataAdapter(pSqlStr, _Connection)
            Catch ex As Exception
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Exception: " & ex.Message, TimeSet.Status.Start)
                Dim alertMessage As String = ""

                alertMessage &= "Unable to Execute [Select] Query" & vbCrLf
                alertMessage &= "--------------------------------------------" & vbCrLf
                alertMessage &= pSqlStr
                alertMessage &= "--------------------------------------------" & vbCrLf

                Throw New Common_Exception(Common_Exception.ErrorType.System, _
                                        "PCMS_PROCESS", _
                                        alertMessage, _
                                        "-998", _
                                        ex.ToString)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Exception: " & ex.Message, TimeSet.Status.Finish)
            End Try

        End Function

        Public Function DataTable(ByVal pSqlStr As String) As System.Data.DataTable
            TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Query: " & pSqlStr, TimeSet.Status.Start)
            Dim oSqlDataAdapter As SqlClient.SqlDataAdapter
            Dim oDataTable As System.Data.DataTable

            Try
                oSqlDataAdapter = New SqlClient.SqlDataAdapter(pSqlStr, _Connection)
                oSqlDataAdapter.SelectCommand.CommandTimeout = 0
                oDataTable = New System.Data.DataTable

                oSqlDataAdapter.Fill(oDataTable)

                If oDataTable.Rows.Count = 0 Then
                    TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Query: " & pSqlStr, TimeSet.Status.Finish)
                    Return Nothing
                Else
                    TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Query: " & pSqlStr, TimeSet.Status.Finish)
                    Return oDataTable
                End If
            Catch ex As Exception
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Exception: " & ex.Message, TimeSet.Status.Start)
                Dim alertMessage As String = ""

                alertMessage &= "Unable to Execute [Select] Query" & vbCrLf
                alertMessage &= "--------------------------------------------" & vbCrLf
                alertMessage &= pSqlStr
                alertMessage &= "--------------------------------------------" & vbCrLf

                Throw New Common_Exception(Common_Exception.ErrorType.System, _
                                        "FLEX_PROCESS", _
                                        alertMessage, _
                                        "-998", _
                                        ex.ToString)
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Exception: " & ex.Message, TimeSet.Status.Finish)
            End Try
        End Function
#End Region

        Enum TransStatus As Integer
            ts_Commit = 0
            ts_Rollback = 1
        End Enum

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

        Public MustOverride ReadOnly Property ErrorDescription() As String

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free other state (managed objects).
                    If Not _Connection Is Nothing Then
                        SqlClient.SqlConnection.ClearPool(_Connection)
                        _Connection = Nothing
                    End If
                End If

                ' TODO: free your own state (unmanaged objects).
                ' TODO: set large fields to null.
            End If
            Me.disposedValue = True
        End Sub

#Region " IDisposable Support "
        ' This code added by Visual Basic to correctly implement the disposable pattern.
        Public Sub Dispose() Implements IDisposable.Dispose
            ' Do not change this code.  Put cleanup code in Dispose(ByVal disposing As Boolean) above.
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace

