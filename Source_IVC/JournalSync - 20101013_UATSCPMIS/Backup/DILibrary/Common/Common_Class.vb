Namespace [Global].Setting
    Class Company
        Private _strPath As String
        Private _System_SerName, _System_DBUserName, _System_DBUserPwds, _System_DBServerType As String
        Private _Company_DBName, _Company_UserName, _Company_Password, _Company_Licence As String
        Private _MessageLog As String
        Private _ConnectCount As Integer

        ''' <summary>
        ''' To Define the Login setting in SAP, the 
        ''' class only support for background batch job
        ''' </summary>
        ''' <param name="pConnects">Insert the total Connect company variable here</param>
        ''' <remarks></remarks>
        Public Sub New(ByVal pConnects As Integer)
            Dim localPath As String = System.Reflection.Assembly.GetExecutingAssembly.Location

            localPath = Microsoft.VisualBasic.Left(localPath, InStrRev(localPath, "\"))
            _strPath = localPath & "Company.ini"
            Me._ConnectCount = pConnects

            _System_SerName = Me.chkVariable_Syndex(GetINIValue("System Information", "Server Name", _strPath))
            _System_DBUserName = Me.chkVariable_Syndex(GetINIValue("System Information", "DB UserName", _strPath))
            _System_DBUserPwds = Me.chkVariable_Syndex(GetINIValue("System Information", "DB Password", _strPath))
            _System_DBServerType = Me.chkVariable_Syndex(GetINIValue("System Information", "DB Server Type", _strPath))

            _Company_Licence = Me.chkVariable_Syndex(GetINIValue("Company connect information", "SAP Licence", _strPath))
            _Company_DBName = Me.chkVariable_Syndex(GetINIValue("Company connect information", "Database Name", _strPath))
            _Company_UserName = Me.chkVariable_Syndex(GetINIValue("Company connect information", "SAP UserName", _strPath))
            _Company_Password = Me.chkVariable_Syndex(GetINIValue("Company connect information", "SAP Password", _strPath))

            _MessageLog = GetINIValue("Message Log Information", "Path", _strPath)
            If _MessageLog = "" Then
                _MessageLog = localPath
            End If

            Me.Save()
        End Sub

        ''' <summary>
        ''' Check the connection parameter is enough for pass in total connected company or not
        ''' </summary>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function chkVariable_Syndex(ByVal pCParameter As String) As String
            Dim parameterCount As Integer
            Dim parameterSyndex As String

            parameterSyndex = ""

            If pCParameter = "" Then
                parameterCount = 0
                parameterSyndex = LSet(parameterSyndex, _ConnectCount - 1).Replace(" ", ",")
            Else
                parameterCount = pCParameter.Split(",").Length
                If _ConnectCount > parameterCount Then
                    parameterSyndex = LSet(pCParameter.Trim, _
                                           _ConnectCount - parameterCount - 1).Replace(" ", "," _
                                           )
                Else
                    parameterSyndex = pCParameter.Trim
                End If
            End If

            Return parameterSyndex
        End Function

        Public Sub Save()
            WriteINIValue("System Information", "Server Name", _System_SerName, _strPath)
            WriteINIValue("System Information", "DB UserName", _System_DBUserName, _strPath)
            WriteINIValue("System Information", "DB Password", _System_DBUserPwds, _strPath)
            WriteINIValue("System Information", "DB Server Type", _System_DBServerType, _strPath)

            WriteINIValue("Company connect information", "SAP Licence", _Company_Licence, _strPath)
            WriteINIValue("Company connect information", "Database Name", _Company_DBName, _strPath)
            WriteINIValue("Company connect information", "SAP UserName", _Company_UserName, _strPath)
            WriteINIValue("Company connect information", "SAP Password", _Company_Password, _strPath)

            WriteINIValue("Message Log Information", "Path", _MessageLog, _strPath)
        End Sub

#Region "Return system Message"
        Public ReadOnly Property System_SerName() As String()
            Get
                Return Me._System_SerName.Split(",")
            End Get
        End Property
        Public ReadOnly Property System_DBUserName() As String()
            Get
                Return Me._System_DBUserName.Split(",")
            End Get
        End Property
        Public ReadOnly Property System_DBUserPwds() As String()
            Get
                Return Me._System_DBUserPwds.Split(",")
            End Get
        End Property
        Public ReadOnly Property System_DBServerType() As eDBType
            Get
                Select Case _System_DBServerType
                    Case "MSSQL"
                        Return eDBType.eDB_2000
                    Case "MSSQL2005"
                        Return eDBType.eDB_2005
                    Case "MSSQL2008"
                        Return eDBType.eDB_2008
                End Select
            End Get
        End Property

        Public ReadOnly Property Company_DBName() As String()
            Get
                Return Me._Company_DBName.Split(",")
            End Get
        End Property
        Public ReadOnly Property Company_UserName() As String()
            Get
                Return Me._Company_UserName.Split(",")
            End Get
        End Property
        Public ReadOnly Property Company_Password() As String()
            Get
                Return Me._Company_Password.Split(",")
            End Get
        End Property
        Public ReadOnly Property Company_Licence_Server() As String()
            Get
                Return Me._Company_Licence.Split(",")
            End Get
        End Property
        Public ReadOnly Property MessageLog() As String
            Get
                Return Me._MessageLog
            End Get
        End Property
#End Region

        Public Enum eDBType As Integer
            eDB_2000
            eDB_2005
            eDB_2008
        End Enum

    End Class
End Namespace