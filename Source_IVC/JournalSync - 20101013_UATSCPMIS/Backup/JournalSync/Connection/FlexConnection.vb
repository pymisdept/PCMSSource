Imports CPS.CPSLIB.Debug
Public Class FlexConnection
    Inherits CPS.DI.Interface.Connection

    Private ReadOnly _FileName As String = "Flex.ini"
    Private _ErrorMessage As String

    Public Sub New(Optional ByVal pNeedConnect As Boolean = True)
        MyBase.New()
        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
        Dim strPath As String

        strPath = System.Reflection.Assembly.GetExecutingAssembly.Location
        strPath = Left(strPath, InStrRev(strPath, "\"))
        strPath = strPath & "\" & _FileName

        MyBase.Server = GetINIValue("System Information", "Server Name", strPath)
        MyBase.DBName = GetINIValue("System Information", "DB Name", strPath)
        MyBase.DBUserID = GetINIValue("System Information", "DB UserName", strPath)
        MyBase.DBUserPW = GetINIValue("System Information", "DB Password", strPath)

        _ErrorMessage = ""

        If pNeedConnect Then
            Try
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & "Flex Connect: " & MyBase.Server & ", " & MyBase.DBName & ", " & MyBase.DBUserID & ", " & MyBase.DBUserPW, TimeSet.Status.Start)
                MyBase.Connect()
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & "Flex Connect: " & MyBase.Server & ", " & MyBase.DBName & ", " & MyBase.DBUserID & ", " & MyBase.DBUserPW, TimeSet.Status.Finish)
            Catch ex As Exception
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & "Exception on Flex Connnect", TimeSet.Status.Start)
                Me._ErrorMessage = ex.ToString
                TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & "Exception on Flex Connnect", TimeSet.Status.Finish)
            End Try
        End If
        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Finish)
    End Sub

    Public Overrides ReadOnly Property ErrorDescription() As String
        Get
            Return _ErrorMessage
        End Get
    End Property

End Class
