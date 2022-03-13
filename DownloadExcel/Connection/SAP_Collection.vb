Public Class SAP_Collection
    Inherits CPS.DI.Interface.Connection

    Private ReadOnly _FileName As String = "[CONNECTION]SAP.ini"
    Private _ErrorMessage As String

    Public Sub New()
        MyBase.New()

        Dim strPath As String

        strPath = System.Reflection.Assembly.GetExecutingAssembly.Location
        strPath = Left(strPath, InStrRev(strPath, "\"))
        strPath = strPath & "\" & _FileName

        MyBase.Server = GetINIValue("System Information", "Server Name", strPath)
        MyBase.DBName = GetINIValue("System Information", "DB Name", strPath)
        MyBase.DBUserID = GetINIValue("System Information", "DB UserName", strPath)
        MyBase.DBUserPW = GetINIValue("System Information", "DB Password", strPath)


        _ErrorMessage = ""

        Try
            MyBase.Connect()
        Catch ex As Exception
            Me._ErrorMessage = ex.ToString
        End Try
    End Sub

    Public Overrides ReadOnly Property ErrorDescription() As String
        Get
            Return _ErrorMessage
        End Get
    End Property
End Class
