Imports System.Data.SqlClient
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Environment
Imports SimpleControls.SimpleDatabase
Imports SimpleControls.SimpleCrypto

Public Class fmLogin

    Private myConn As SqlConnection
    Public Sub New()
        InitializeComponent()
        myConn = New SqlConnection()
        openSQLConnection()
        DefaultLogin()
    End Sub

    Public Sub New(ByVal isDefaultLogin As Boolean)
        InitializeComponent()
        myConn = New SqlConnection()
        openSQLConnection()
        If isDefaultLogin Then
            DefaultLogin()
        End If
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Login.Click
        Dim name As String = txt_Username.Text
        Dim password As String = txt_Password.Text

        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader

        Dim sql As String

        Dim hash = Encrypt(password)

        Dim dr As Windows.Forms.DialogResult

        Dim isSupervisor As String = ""
        Dim isValid As Boolean = False

        sql = "select SUPERVISOR from v_sc_users where Upper(LOGINNAME) = '" & name.ToUpper() & "' AND passwd = '" & hash & "'"
        myCmd = New SqlCommand(sql, myConn)

        myReader = myCmd.ExecuteReader()
        If myReader.Read() Then
            isSupervisor = myReader.Item(0).ToString()
            isValid = True
        Else
            MsgBox("Invalid Username or password", MsgBoxStyle.OkOnly, "PCMS")
            isValid = False
        End If

        myReader.Close()

        If isValid Then
            Me.Hide()
            Using fm As New fmMain(name, isSupervisor = "1")
                fm.ShowDialog()
                fm.Dispose()
                dr = fm.DialogResult
            End Using
        End If

        If dr = Windows.Forms.DialogResult.Retry Then
            Me.txt_Username.Text = ""
            Me.txt_Password.Text = ""
            Me.txt_Username.Focus()
            Me.txt_Username.Select()
            Me.Show()
        ElseIf dr = Windows.Forms.DialogResult.Cancel Then
            Environment.Exit(0)
        End If

    End Sub

    Private Sub DefaultLogin()

        Dim name As String
        Dim domainname As String

        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader

        Dim sql As String

        Dim isSupervisor As String = ""
        Dim isValid As Boolean = False

        Dim dr As Windows.Forms.DialogResult

        domainname = Environment.UserDomainName
        name = Environment.UserName

        'name = "fyleung"

        If domainname = "PAULY" Then
            sql = "select SUPERVISOR from v_sc_users where Upper(LOGINNAME) = '" & name.ToUpper() & "'"
            myCmd = New SqlCommand(sql, myConn)

            myReader = myCmd.ExecuteReader()
            If myReader.Read() Then
                isValid = True
                isSupervisor = myReader.Item(0).ToString()
            Else
                isValid = False
            End If

            myReader.Close()
        End If

        If isValid Then
            Me.Hide()
            Using fm As New fmMain(name, isSupervisor = "1")
                fm.ShowDialog()
                fm.Dispose()
                dr = fm.DialogResult
            End Using
        End If

        If dr = Windows.Forms.DialogResult.Retry Then
            Me.txt_Username.Text = ""
            Me.txt_Password.Text = ""
            Me.txt_Username.Focus()
            Me.txt_Username.Select()
            Me.Show()
        ElseIf dr = Windows.Forms.DialogResult.Cancel Then
            Environment.Exit(0)
        End If

        Me.txt_Username.Focus()
        Me.txt_Username.Select()

    End Sub

    Private Sub openSQLConnection()

        If myConn.State <> ConnectionState.Closed Then
            Try
                myConn.Close()
            Catch ex As Exception
                MsgBox("Fail to close connection.", MsgBoxStyle.OkOnly, "PCMS")
            End Try
        End If

        Dim connectionStr = "Initial Catalog=" & My.Resources.Server_HK_FE_DB & ";" & _
                "Data Source=" & My.Resources.Server_HK_FE_Server & ";User ID=" & My.Resources.Server_HK_FE_DBUserName & ";Password=" & My.Resources.Server_HK_FE_DBPassword
        myConn = New SqlConnection(connectionStr)

        Try
            myConn.Open()
        Catch ex As Exception
            MsgBox("Fail to open connection.", MsgBoxStyle.OkOnly, "PCMS")
        End Try

    End Sub

    Public Shared Function Encrypt(ByVal password As String) As String
        Dim md5 As CryptoHash = New CryptoHash(CryptoHash.Hashes.MD5)
        Dim hash As String = md5.GetHashInString(password)
        Return hash
    End Function

End Class