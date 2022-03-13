Namespace DI.Console
    Public Class Company
        Implements IDisposable

        Private _Company() As SAPbobsCOM.Company
        Private Shared _currCompany As SAPbobsCOM.Company

        Public Sub New(ByVal pCount As Integer)
            If pCount = 0 Then
                Throw New NotSupportedException("Unable to pass in variable smaller than 1")
            End If

            _Company = Nothing
            For i As Integer = 1 To pCount
                If _Company Is Nothing Then
                    ReDim _Company(0)
                Else
                    ReDim Preserve _Company(_Company.Length)
                End If
                _Company(_Company.Length - 1) = New SAPbobsCOM.Company
            Next
        End Sub

        Public Shared ReadOnly Property Company() As SAPbobsCOM.Company
            Get
                If Not _currCompany Is Nothing AndAlso _currCompany.Connected Then
                    Return _currCompany
                Else
                    Throw New Common_Exception(Common_Exception.ErrorType.Normal, "COMPANY", "Current Company Object is nothing")
                End If
            End Get
        End Property

        ''' <summary>
        ''' If connection request is 1, then system will automatic copy this object to Current Company
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public ReadOnly Property CompanyList() As SAPbobsCOM.Company()
            Get
                'If connection request is 1, then system will automatic copy this object to Current Company
                If _Company.Length = 1 Then
                    _currCompany = _Company(0)
                End If

                Return _Company
            End Get
        End Property

        Public ReadOnly Property GetLastErrorDescription() As String()
            Get
                Dim oRtnMessage() As String = Nothing

                For Each oCompany As SAPbobsCOM.Company In _Company
                    If oRtnMessage Is Nothing Then
                        ReDim oRtnMessage(0)
                    Else
                        ReDim Preserve oRtnMessage(oRtnMessage.Length)
                    End If
                    oRtnMessage(oRtnMessage.Length - 1) &= oCompany.GetLastErrorCode & " : " & oCompany.GetLastErrorDescription
                Next

                Return oRtnMessage
            End Get
        End Property

        Public Function Connects() As Integer
            CPSLIB.Debug.TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, CPSLIB.Debug.TimeSet.Status.Start)
            Dim oSetting As CPS.Global.Setting.Company
            Dim sCompany As sCompany
            Dim sErrorCount As Integer = 0

            oSetting = New [Global].Setting.Company(_Company.Length)

            For i As Integer = 0 To _Company.Length - 1
                CPSLIB.Debug.TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Database: " & oSetting.Company_DBName(i), CPSLIB.Debug.TimeSet.Status.Start)
                'Collect information need for login to SAP
                sCompany = New sCompany
                sCompany.Server = oSetting.System_SerName(i)
                sCompany.DbUserName = oSetting.System_DBUserName(i)
                sCompany.DbPassword = oSetting.System_DBUserPwds(i)
                Select Case oSetting.System_DBServerType
                    Case [Global].Setting.Company.eDBType.eDB_2000
                        sCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL
                    Case [Global].Setting.Company.eDBType.eDB_2005
                        sCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2005
                    Case [Global].Setting.Company.eDBType.eDB_2008
                        sCompany.DbServerType = SAPbobsCOM.BoDataServerTypes.dst_MSSQL2008
                End Select

                sCompany.LicenceServer = oSetting.Company_Licence_Server(i)
                sCompany.CompanyDB = oSetting.Company_DBName(i)
                sCompany.UserName = oSetting.Company_UserName(i)
                sCompany.Password = oSetting.Company_Password(i)

                _Company(i) = Connect(_Company(i), sCompany)
                If Not _Company(i).Connected Then
                    sErrorCount += 1
                End If
                CPSLIB.Debug.TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & " Database: " & oSetting.Company_DBName(i), CPSLIB.Debug.TimeSet.Status.Finish)
            Next
            CPSLIB.Debug.TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, CPSLIB.Debug.TimeSet.Status.Finish)
            Return sErrorCount
        End Function

        Public Shared Sub setCurrentCompany(ByVal pCompany As SAPbobsCOM.Company)
            If pCompany.Connected Then
                _currCompany = pCompany
            Else
                Throw New Common_Exception(Common_Exception.ErrorType.SAP, "SETCURCMP_000", "Pass-in Company isn't connected")
            End If
        End Sub

        Private Structure sCompany
            Dim Server As String
            Dim LicenceServer As String
            Dim DbServerType As SAPbobsCOM.BoDataServerTypes
            Dim CompanyDB As String
            Dim DbUserName As String
            Dim DbPassword As String
            Dim UserName As String
            Dim Password As String
        End Structure

        Private Function Connect(ByVal pCompany As SAPbobsCOM.Company, _
                                 ByVal sCompany As sCompany) As SAPbobsCOM.Company

            CPSLIB.Debug.TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, CPSLIB.Debug.TimeSet.Status.Start)
            pCompany.Server = sCompany.Server
            pCompany.LicenseServer = sCompany.LicenceServer
            pCompany.DbServerType = sCompany.DbServerType
            pCompany.CompanyDB = sCompany.CompanyDB
            pCompany.DbUserName = sCompany.DbUserName
            pCompany.DbPassword = sCompany.DbPassword
            pCompany.UserName = sCompany.UserName
            pCompany.Password = sCompany.Password

            pCompany.Connect()
            CPSLIB.Debug.TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, CPSLIB.Debug.TimeSet.Status.Finish)
            Return pCompany
        End Function

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free other state (managed objects).
                    If Not _Company Is Nothing Then
                        For Each oCompany In _Company
                            If oCompany.Connected Then
                                oCompany.Disconnect()
                                oCompany = Nothing
                            End If
                        Next

                        If _currCompany.Connected Then
                            _currCompany.Disconnect()
                        End If
                        _currCompany = Nothing
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