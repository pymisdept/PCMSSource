Namespace DI.Interface
    Class Setting
        Private _System_SerName, _
              _System_DBUserName, _
              _System_DBUserPwds As String
        Private _System_DBServerType As dbServerType
        Private _System_UserTrusted As db_UserTrusted
        Private _Company_Licence, _
                _Company_DBName, _
                _Company_UserName, _
                _Company_Password As String
        Private strPath As String = System.Reflection.Assembly.GetExecutingAssembly.Location & "\" & settingPath & ".ini"

        Public Sub New()
            load()
        End Sub
        Public Sub refresh()
            load()
        End Sub
        Public Sub load()
            _System_SerName = ""
            _System_DBServerType = dbServerType.db_MSSQL
            _System_DBUserName = ""
            _System_DBUserPwds = ""
            _System_UserTrusted = db_UserTrusted.dbTrue
            _Company_DBName = ""
            _Company_UserName = ""
            _Company_Password = ""
            _Company_Licence = ""

            _System_SerName = GetINIValue("System Information", "Server Name", strPath)
            Select Case GetINIValue("System Information", "DB Server Type", strPath)
                Case Common_Module.dbServerType.db_MSSQL
                    _System_DBServerType = dbServerType.db_MSSQL
                Case Common_Module.dbServerType.db_MSSQL_2005
                    _System_DBServerType = dbServerType.db_MSSQL_2005
                Case Else
                    _System_DBServerType = dbServerType.db_MSSQL
            End Select

            _System_DBUserName = GetINIValue("System Information", "DB UserName", strPath)
            _System_DBUserPwds = GetINIValue("System Information", "DB Password", strPath)
            Select Case GetINIValue("System Information", "User Trusted", strPath)
                Case Common_Module.db_UserTrusted.dbTrue
                    _System_UserTrusted = True
                Case Common_Module.db_UserTrusted.dbTrue
                    _System_UserTrusted = False
                Case Else
                    _System_UserTrusted = True
            End Select

            _Company_DBName = GetINIValue("Company connect information", "Database Name", strPath)
            _Company_UserName = GetINIValue("Company connect information", "SAP UserName", strPath)
            _Company_Password = GetINIValue("Company connect information", "SAP Password", strPath)
            _Company_Licence = GetINIValue("Company connect information", "SAP Licence", strPath)
        End Sub
        Public Sub save()
            WriteINIValue("System Information", "Server Name", Me._System_SerName, strPath)
            WriteINIValue("System Information", "DB Server Type", Me._System_DBServerType, strPath)
            WriteINIValue("System Information", "DB UserName", Me._System_DBUserName, strPath)
            WriteINIValue("System Information", "DB Password", Me._System_DBUserPwds, strPath)
            WriteINIValue("System Information", "User Trusted", Me._System_UserTrusted, strPath)
            WriteINIValue("Company connect information", "Database Name", Me._Company_DBName, strPath)
            WriteINIValue("Company connect information", "SAP UserName", Me._Company_UserName, strPath)
            WriteINIValue("Company connect information", "SAP Password", Me._Company_Password, strPath)
            WriteINIValue("Company connect information", "SAP Licence", Me._Company_Licence, strPath)
        End Sub

        Public Property System_SerName() As String
            Get
                If Me._System_SerName = "" Then
                    Me._System_SerName = "127.0.0.1"
                End If
                Return Me._System_SerName
            End Get
            Set(ByVal Value As String)
                Me._System_SerName = Value
            End Set
        End Property
        Public Property System_DBUserName() As String
            Get
                Return Me._System_DBUserName
            End Get
            Set(ByVal Value As String)
                Me._System_DBUserName = Value
            End Set
        End Property
        Public Property System_DBUserPwds() As String
            Get
                Return Me._System_DBUserPwds
            End Get
            Set(ByVal Value As String)
                Me._System_DBUserPwds = Value
            End Set
        End Property
        Public Property System_DBServerType() As String
            Get
                Return Me._System_DBServerType
            End Get
            Set(ByVal Value As String)
                Me._System_DBServerType = Value
            End Set
        End Property
        Public Property System_UserTrusted() As db_UserTrusted
            Get
                Return Me._System_UserTrusted
            End Get
            Set(ByVal Value As db_UserTrusted)
                _System_UserTrusted = Value
            End Set
        End Property

        Public Property Company_Licence() As String
            Get
                If Me._Company_Licence = "" Then
                    Me._Company_Licence = "127.0.0.1:30000"
                End If
                Return Me._Company_Licence
            End Get
            Set(ByVal Value As String)
                Me._Company_Licence = Value
            End Set
        End Property
        Public Property Company_DBName() As String
            Get
                Return Me._Company_DBName
            End Get
            Set(ByVal Value As String)
                Me._Company_DBName = Value
            End Set
        End Property
        Public Property Company_UserName() As String
            Get
                Return Me._Company_UserName
            End Get
            Set(ByVal Value As String)
                Me._Company_UserName = Value
            End Set
        End Property
        Public Property Company_Password() As String
            Get
                Return Me._Company_Password
            End Get
            Set(ByVal Value As String)
                Me._Company_Password = Value
            End Set
        End Property

    End Class
End Namespace