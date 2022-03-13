Imports System.Data.SqlClient
Imports System.IO
Imports System.Runtime.InteropServices
Imports System.Environment
Imports System.Net

Public Class fmMain

    Private myConn As SqlConnection
    Public mySetting As Setting
    Public Shared OutputFolder As String = ""
    Public crx As CrExport

    Protected _fe_server As String = ""
    Private _fe_dbname As String = ""
    Private _be_dbname As String = ""
    Private _username As String = ""
    Private _password As String = ""
    Private selectedServ As String = ""

#Region "Constructor"
    Sub New(ByVal curUser As String, ByVal isS As Boolean)
        InitializeComponent()
        myConn = New SqlConnection()
        mySetting = New Setting(My.Resources.SettingFile)

        'Set Current User & Ledger
        mySetting.CurrentUser() = curUser
        mySetting.CurrentUserIsS() = isS

        ImportHistory()

        If mySetting.loadSetting() = False Then
            MsgBox("Unable to load the configuration file, please check the config file", MsgBoxStyle.OkOnly, "PCMS")
            End 'Force Close
        End If

        checkProjectSelected()

        Dim dt As Date = DateSerial(Year(Today()), Month(Today()), 0)
        txt_fromPeriod.Text = dt.Month.ToString().PadLeft(2, "0") & "/" & dt.Year.ToString().PadLeft(4, "0")
        txt_toPeriod.Text = txt_fromPeriod.Text

        Me.rb_server_testing.Visible = mySetting.ShowTesting

        If selectedServ <> "" Then
            changeDB(selectedServ)
        Else
            changeDB(mySetting.DefaultServer)
        End If
    End Sub

#End Region

#Region "Form Load"

    Private Sub fmMain_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = Me.Text & " (Version: " & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build & ")"
    End Sub

#End Region

#Region "Button Event"
    Private Sub btn_Find_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Find.Click

        cbx_Result.Items.Clear()

        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader
        Dim sql As String
        Dim fromDate As Date
        Dim toDate As Date

        Dim isErr As Boolean = False
        Dim ErrMsg As String = ""

        fromDate = getDate(txt_fromPeriod.Text)
        toDate = getDate(txt_toPeriod.Text)
        txt_PrjCode.Text = Replace(Trim(txt_PrjCode.Text), "'", "")

        If fromDate = Date.MinValue Then
            isErr = True
            ErrMsg = "Invalid From Period" & Environment.NewLine
        End If

        If toDate = Date.MinValue Then
            isErr = True
            ErrMsg = ErrMsg & "Invalid To Period" & Environment.NewLine
        Else
            toDate = toDate.AddMonths(1).AddDays(-1)
        End If

        If isErr = True Then
            MsgBox(ErrMsg, MsgBoxStyle.OkOnly, "PCMS")
            Exit Sub
        End If


        ' sql = "select distinct p.ProjectCode from DocumentProperty d, OPRJ p where ProjectCode like '%" & Trim(txt_PrjCode.Text) & "%' AND Type = '1040' AND DocStatus IN ('D', 'P') AND dt01 between '" & fromDate.ToString("yyyy-MM-dd") & "' AND '" & toDate.ToString("yyyy-MM-dd") & "'"
        sql = "select distinct d.ProjectCode from PCMS_BE." & mySetting.BEDatabase & ".dbo.OPRJ p, DocumentProperty d"
        sql = sql & " where p.PrjCode = d.ProjectCode AND"

        If mySetting.CurrentPrjList <> "All" Then
            sql = sql & " p.PrjCode IN (" & mySetting.CurrentPrjList & ") AND"
        End If

        sql = sql & " d.ProjectCode LIKE '%" & Trim(txt_PrjCode.Text) & "%' AND"
        sql = sql & " d.Type = '1040' AND"
        sql = sql & " d.DocStatus IN ('D', 'P') AND"
        sql = sql & " d.dt01 between '" & fromDate.ToString("yyyy-MM-dd") & "' AND '" & toDate.ToString("yyyy-MM-dd") & "'"
        sql = sql & " order by d.ProjectCode"

        myCmd = New SqlCommand(sql, myConn)
        myReader = myCmd.ExecuteReader()
        While myReader.Read()
            cbx_Result.Items.Add(myReader.Item(0), True)
        End While

        checkResultState()
        myReader.Close()
        myCmd.Dispose()

    End Sub

    Private Sub btn_addPrj_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_addPrj.Click

        If cbx_Result.CheckedItems.Count > 0 Then
            For Each itm As String In cbx_Result.CheckedItems
                If Not cbx_Export.Items.Contains(itm) Then
                    cbx_Export.Items.Add(itm, True)
                End If
            Next
            checkExportState()
            checkProjectSelected()
        Else
            MsgBox("Please select a project", MsgBoxStyle.OkOnly, "PCMS")
        End If

    End Sub

    Private Sub btn_fromENList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_fromENList.Click
        cbx_Result.Items.Clear()

        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader
        Dim sql As String

        If mySetting.CurrentPrjList = "All" Then
            sql = "select c.PrjCode from CPSPRADM c, PCMS_BE." & mySetting.BEDatabase & ".dbo.OPRJ p where c.PrjCode = p.PrjCode and p.Locked = 'N' "
        Else
            sql = "select c.PrjCode from CPSPRADM c, PCMS_BE." & mySetting.BEDatabase & ".dbo.OPRJ p where c.PrjCode = p.PrjCode and p.Locked = 'N' AND p.PrjCode IN (" & mySetting.CurrentPrjList & ")"
        End If

        Try
            myCmd = New SqlCommand(sql, myConn)
            myReader = myCmd.ExecuteReader()
            While myReader.Read()
                cbx_Result.Items.Add(myReader.Item(0), True)
            End While

            checkResultState()
            myReader.Close()
            myCmd.Dispose()
        Catch ex As Exception

        End Try

    End Sub

    Private Sub btn_deletePrj_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_deletePrj.Click
        If cbx_Export.CheckedItems.Count > 0 Then
            For i As Integer = cbx_Export.CheckedIndices.Count - 1 To 0 Step -1
                cbx_Export.Items.RemoveAt(cbx_Export.CheckedIndices.Item(i))
            Next
            checkExportState()
            checkProjectSelected()
        Else
            MsgBox("Please select a project", MsgBoxStyle.OkOnly, "PCMS")
        End If

    End Sub

    Private Sub btn_Next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Next.Click

        Dim fromDate As Date = getDate(txt_fromPeriod.Text)
        Dim toDate As Date = getDate(txt_toPeriod.Text)

        If fromDate <> Date.MinValue And toDate <> Date.MinValue Then
            Using fm As New fmExport(mySetting, cbx_Export.CheckedItems.Cast(Of String).ToArray(), txt_fromPeriod.Text, txt_toPeriod.Text, selectedServ, OutputFolder)
                fm.ShowDialog()
            End Using
        Else
            MsgBox("Invalid From / To period", MsgBoxStyle.OkOnly, "PCMS")
        End If

    End Sub

    Private Sub btn_importsfile_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_importfile.Click
        ofDlg.Multiselect = False
        ofDlg.Filter = "Text Files|*.txt"
        ofDlg.FileName = ""
        Dim count As Integer
        Dim dr As DialogResult = ofDlg.ShowDialog()
        Dim curPrj As String = ""
        Try
            If (dr = Windows.Forms.DialogResult.OK) Then
                Dim stream As Stream = ofDlg.OpenFile()
                cbx_Result.Items.Clear()
                Using sr As New StreamReader(stream)
                    While sr.EndOfStream = False
                        curPrj = sr.ReadLine()
                        If isProjectExist(curPrj) = True Then
                            cbx_Result.Items.Add(curPrj, True)
                            count += 1
                        End If
                    End While
                    sr.Close()
                End Using

                checkResultState()
                MsgBox(count & " project(s) imported", MsgBoxStyle.OkOnly, "PCMS")
            End If
        Catch ex As Exception
            MsgBox("Failed to import text file", MsgBoxStyle.OkOnly, "PCMS")
        End Try
    End Sub

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button1.Click
        MessageBox.Show("Initial Catalog=" & mySetting.FEDatabase & ";" & _
                "Data Source=" & mySetting.FEServer & ";User ID=" & mySetting.FEUsername & ";Password=" & mySetting.FEPassword)
    End Sub

#End Region

#Region "CheckList Event"

    Private Sub cbx_Result_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbx_Result.SelectedIndexChanged
        checkResultState()
    End Sub

    Private Sub cbx_Export_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbx_Export.SelectedIndexChanged
        checkExportState()
        checkProjectSelected()
    End Sub

#End Region

#Region "Checkbox Event"

    Private Sub CheckBox1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox1.Click
        If CheckBox1.CheckState = Windows.Forms.CheckState.Checked Then
            For idx As Integer = 0 To cbx_Result.Items.Count - 1
                Me.cbx_Result.SetItemCheckState(idx, Windows.Forms.CheckState.Checked)
            Next
        ElseIf CheckBox1.CheckState = Windows.Forms.CheckState.Unchecked Then
            For idx As Integer = 0 To cbx_Result.Items.Count - 1
                Me.cbx_Result.SetItemCheckState(idx, Windows.Forms.CheckState.Unchecked)
            Next
        End If
    End Sub

    Private Sub CheckBox2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CheckBox2.Click
        If CheckBox2.CheckState = Windows.Forms.CheckState.Checked Then
            For idx As Integer = 0 To cbx_Export.Items.Count - 1
                Me.cbx_Export.SetItemCheckState(idx, Windows.Forms.CheckState.Checked)
            Next
        ElseIf CheckBox2.CheckState = Windows.Forms.CheckState.Unchecked Then
            For idx As Integer = 0 To cbx_Export.Items.Count - 1
                Me.cbx_Export.SetItemCheckState(idx, Windows.Forms.CheckState.Unchecked)
            Next
        End If

        checkProjectSelected()
    End Sub

#End Region

#Region "Textbox Event"

    Private Sub txt_fromPeriod_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_fromPeriod.TextChanged
        If txt_fromPeriod.TextLength = 6 And txt_fromPeriod.Text.IndexOf("/") = -1 Then
            txt_fromPeriod.Text = txt_fromPeriod.Text.Substring(0, 2) & "/" & txt_fromPeriod.Text.Substring(2, 4)
        ElseIf txt_fromPeriod.TextLength = 7 And txt_fromPeriod.Text.IndexOf("/") > -1 Then
            Dim fromDate As Date = getDate(txt_fromPeriod.Text)
            Dim toDate As Date = getDate(txt_toPeriod.Text)

            If fromDate <> Date.MinValue And toDate <> Date.MinValue Then
                If fromDate > toDate Then
                    txt_toPeriod.Text = txt_fromPeriod.Text
                End If
            End If
            ' txt_toPeriod.Select()
            ' txt_toPeriod.Focus()
        End If
    End Sub

    Private Sub txt_toPeriod_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_toPeriod.TextChanged
        If txt_toPeriod.TextLength = 6 And txt_toPeriod.Text.IndexOf("/") = -1 Then
            txt_toPeriod.Text = txt_toPeriod.Text.Substring(0, 2) & "/" & txt_toPeriod.Text.Substring(2, 4)
        ElseIf txt_toPeriod.TextLength = 7 And txt_toPeriod.Text.IndexOf("/") > -1 Then
            Dim fromDate As Date = getDate(txt_fromPeriod.Text)
            Dim toDate As Date = getDate(txt_toPeriod.Text)

            If fromDate <> Date.MinValue And toDate <> Date.MinValue Then
                If toDate < fromDate Then
                    txt_fromPeriod.Text = txt_toPeriod.Text
                End If
            End If
        End If
    End Sub

#End Region

#Region "Radio Button Event"
    Private Sub rb_server_hk_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_server_hk.CheckedChanged
        If rb_server_hk.Checked = True Then
            rb_server_hk.BackColor = Color.PowderBlue
            selectedServ = "HK"
            changeDB(selectedServ)
        Else
            rb_server_hk.BackColor = SystemColors.Control
        End If
    End Sub

    Private Sub rb_server_testing_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_server_testing.CheckedChanged
        If rb_server_testing.Checked = True Then
            rb_server_testing.BackColor = Color.PowderBlue
            selectedServ = "Testing"
            changeDB(selectedServ)
        Else
            rb_server_testing.BackColor = SystemColors.Control
        End If
    End Sub

    Private Sub rb_server_msc_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_server_msc.CheckedChanged
        If rb_server_msc.Checked = True Then
            rb_server_msc.BackColor = Color.PowderBlue
            selectedServ = "MSC"
            changeDB(selectedServ)
        Else
            rb_server_msc.BackColor = SystemColors.Control
        End If
    End Sub

    Private Sub rb_server_tw_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles rb_server_tw.CheckedChanged
        If rb_server_tw.Checked = True Then
            rb_server_tw.BackColor = Color.PowderBlue
            selectedServ = "TW"
            changeDB(selectedServ)
        Else
            rb_server_tw.BackColor = SystemColors.Control
        End If
    End Sub

#End Region

    Private Sub changeDB(ByVal type As String)
        If type = "HK" Then
            mySetting.FEServer = My.Resources.Server_HK_FE_Server
            mySetting.FEDatabase = My.Resources.Server_HK_FE_DB
            mySetting.FEUsername = My.Resources.Server_HK_FE_DBUserName
            mySetting.FEPassword = My.Resources.Server_HK_FE_DBPassword

            mySetting.BEServer = My.Resources.Server_HK_BE_Server
            mySetting.BEDatabase = My.Resources.Server_HK_BE_DB
            mySetting.BEUsername = My.Resources.Server_HK_BE_DBUserName
            mySetting.BEPassword = My.Resources.Server_HK_BE_DBPassword
            rb_server_hk.Checked = True
        ElseIf type = "MSC" Then
            mySetting.FEServer = My.Resources.Server_MS_FE_Server
            mySetting.FEDatabase = My.Resources.Server_MS_FE_DB
            mySetting.FEUsername = My.Resources.Server_MS_FE_DBUserName
            mySetting.FEPassword = My.Resources.Server_MS_FE_DBPassword

            mySetting.BEServer = My.Resources.Server_MS_BE_Server
            mySetting.BEDatabase = My.Resources.Server_MS_BE_DB
            mySetting.BEUsername = My.Resources.Server_MS_BE_DBUserName
            mySetting.BEPassword = My.Resources.Server_MS_BE_DBPassword
            rb_server_msc.Checked = True
        ElseIf type = "TW" Then
            mySetting.FEServer = My.Resources.Server_TW_FE_Server
            mySetting.FEDatabase = My.Resources.Server_TW_FE_DB
            mySetting.FEUsername = My.Resources.Server_TW_FE_DBUserName
            mySetting.FEPassword = My.Resources.Server_TW_FE_DBPassword

            mySetting.BEServer = My.Resources.Server_TW_BE_Server
            mySetting.BEDatabase = My.Resources.Server_TW_BE_DB
            mySetting.BEUsername = My.Resources.Server_TW_BE_DBUserName
            mySetting.BEPassword = My.Resources.Server_TW_BE_DBPassword
            rb_server_tw.Checked = True
        ElseIf type = "Testing" Then
            mySetting.FEServer = My.Resources.Server_TS_FE_Server
            mySetting.FEDatabase = My.Resources.Server_TS_FE_DB
            mySetting.FEUsername = My.Resources.Server_TS_FE_DBUserName
            mySetting.FEPassword = My.Resources.Server_TS_FE_DBPassword

            mySetting.BEServer = My.Resources.Server_TS_BE_Server
            mySetting.BEDatabase = My.Resources.Server_TS_BE_DB
            mySetting.BEUsername = My.Resources.Server_TS_BE_DBUserName
            mySetting.BEPassword = My.Resources.Server_TS_BE_DBPassword

            rb_server_testing.Visible = True
            rb_server_testing.Checked = True
        End If

        changeSQLConnection()
        checkSupervisor()
        resetList()
        updateProjectList()
    End Sub

    Private Function getBEDB(ByVal serv) As String
        If serv = "HK" Then
            Return My.Resources.Server_HK_BE_DB
        ElseIf serv = "MSC" Then
            Return My.Resources.Server_MS_BE_DB
        ElseIf serv = "TW" Then
            Return My.Resources.Server_TW_BE_DB
        ElseIf serv = "Testing" Then
            Return My.Resources.Server_TS_BE_DB
        Else
            Return My.Resources.Server_HK_BE_DB
        End If
    End Function

    Private Function getFEDB(ByVal serv) As String
        If serv = "HK" Then
            Return My.Resources.Server_HK_FE_DB
        ElseIf serv = "MSC" Then
            Return My.Resources.Server_MS_FE_DB
        ElseIf serv = "TW" Then
            Return My.Resources.Server_TW_FE_DB
        ElseIf serv = "Testing" Then
            Return My.Resources.Server_TS_FE_DB
        Else
            Return My.Resources.Server_HK_FE_DB
        End If
    End Function

    Private Sub changeSQLConnection()

        If myConn.State <> ConnectionState.Closed Then
            Try
                myConn.Close()
            Catch ex As Exception
                MsgBox("Fail to close connection.", MsgBoxStyle.OkOnly, "PCMS")
            End Try
        End If

        Dim connectionStr = "Initial Catalog=" & mySetting.FEDatabase & ";" & _
                "Data Source=" & mySetting.FEServer & ";User ID=" & mySetting.FEUsername & ";Password=" & mySetting.FEPassword
        myConn = New SqlConnection(connectionStr)

        Try
            myConn.Open()
        Catch ex As Exception
            MsgBox("Fail to open connection.", MsgBoxStyle.OkOnly, "PCMS")
        End Try

    End Sub

    Private Function isProjectExist(ByVal PrjCode As String)

        If Trim(PrjCode) <> "" Then

            Dim myCmd As SqlCommand
            Dim myReader As SqlDataReader
            Dim sql As String

            If mySetting.CurrentPrjList = "All" Then
                sql = "select 1 from PCMS_BE." & mySetting.BEDatabase & ".dbo.OPRJ where PrjCode = '" & PrjCode & "' AND Locked = 'N'"
            Else
                sql = "select 1 from PCMS_BE." & mySetting.BEDatabase & ".dbo.OPRJ where PrjCode = '" & PrjCode & "' AND Locked = 'N' AND PrjCode IN (" & mySetting.CurrentPrjList & ")"
            End If

            myCmd = New SqlCommand(sql, myConn)
            myReader = myCmd.ExecuteReader()

            If myReader.Read() Then
                myReader.Close()
                myCmd.Dispose()
                Return True
            Else
                myReader.Close()
                myCmd.Dispose()
                Return False
            End If
        Else
            Return False
        End If

    End Function

    Private Sub ImportHistory()
        Dim row As Integer = 1
        Dim dir As String = GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\\" & My.Resources.ApplicationName
        Try
            cbx_Export.Items.Clear()

            If Not Directory.Exists(dir) Then
                Directory.CreateDirectory(dir)
            End If

            Using sr As New StreamReader(dir + "\\" + My.Resources.HistoryFile)
                While sr.EndOfStream = False
                    If row = 2 Then
                        If Trim(sr.ReadLine()) <> My.Resources.HistoryVersion Then
                            Exit Sub
                        End If
                    ElseIf row = 5 Then
                        OutputFolder = Trim(sr.ReadLine())
                    ElseIf row = 8 Then
                        selectedServ = Trim(sr.ReadLine())
                    ElseIf row >= 11 Then
                        cbx_Export.Items.Add(Trim(sr.ReadLine()))
                    Else
                        sr.ReadLine()
                    End If
                    row += 1
                End While
                sr.Close()
            End Using
        Catch ex As Exception
        End Try
    End Sub

    Private Sub checkProjectSelected()
        If cbx_Export.CheckedItems.Count > 0 Then
            btn_Next.Enabled = True
        Else
            btn_Next.Enabled = False
        End If
    End Sub

    Private Sub checkExportState()
        If cbx_Export.CheckedItems.Count = 0 Then
            CheckBox2.CheckState = Windows.Forms.CheckState.Unchecked
        ElseIf cbx_Export.CheckedItems.Count <> cbx_Export.Items.Count Then
            CheckBox2.CheckState = Windows.Forms.CheckState.Indeterminate
        ElseIf cbx_Export.CheckedItems.Count = cbx_Export.Items.Count Then
            CheckBox2.CheckState = Windows.Forms.CheckState.Checked
        End If
    End Sub

    Private Sub checkResultState()
        If cbx_Result.CheckedItems.Count = 0 Then
            CheckBox1.CheckState = Windows.Forms.CheckState.Unchecked
        ElseIf cbx_Result.CheckedItems.Count <> cbx_Result.Items.Count Then
            CheckBox1.CheckState = Windows.Forms.CheckState.Indeterminate
        ElseIf cbx_Result.CheckedItems.Count = cbx_Result.Items.Count Then
            CheckBox1.CheckState = Windows.Forms.CheckState.Checked
        End If
    End Sub

    Private Sub resetList()

        cbx_Result.Items.Clear()
        CheckBox1.CheckState = CheckState.Unchecked
        CheckBox2.CheckState = CheckState.Unchecked

        For idx As Integer = 0 To cbx_Export.Items.Count - 1
            Me.cbx_Export.SetItemCheckState(idx, Windows.Forms.CheckState.Unchecked)
        Next

    End Sub

    Private Sub updateProjectList()
        Dim db As String
        Dim serv As String
        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader
        Dim tempPrj As String

        If mySetting.CurrentUserIsS Then
            mySetting.CurrentPrjList = "All"
            Exit Sub
        End If

        If selectedServ = "" Then
            db = getBEDB(mySetting.DefaultServer)
            serv = mySetting.DefaultServer
        Else
            db = getBEDB(selectedServ)
            serv = selectedServ
        End If

        'Dim sql2 As String = "SELECT (SELECT distinct(CompanyCode) + ',' from PCMS_BE." & db & ".dbo.[CPS_View_UserLedger] where Upper(LOGINNAME) = '" & mySetting.CurrentUser & "' FOR XML PATH('')) AS PLIST"
        Dim sql2 As String = "SELECT ACCESSProjectID AS PLIST from SC_User u where Upper(u.LOGINNAME) = '" & mySetting.CurrentUser & "'"

        myCmd = New SqlCommand(sql2, myConn)
        myReader = myCmd.ExecuteReader()

        If myReader.Read() Then
            If myReader.Item(0).ToString().Trim(",") = "" Then
                MsgBox("No valid project for you in " & serv & " PCMS", MsgBoxStyle.OkOnly, "PCMS")
                mySetting.CurrentPrjList = "'-'"
                cbx_Export.Items.Clear()
            Else
                tempPrj = myReader.Item(0).ToString().Trim(",")
                tempPrj = tempPrj.Replace(",", "','")
                mySetting.CurrentPrjList = "'" & tempPrj & "'"
            End If
        Else
            mySetting.CurrentPrjList = "'-'"
            MsgBox("No valid project for you in " & serv & " PCMS", MsgBoxStyle.OkOnly, "PCMS")
            cbx_Export.Items.Clear()
        End If

        myReader.Close()
        myCmd.Dispose()
    End Sub

    Private Sub fmMain_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If myConn.State <> ConnectionState.Open Then
            Try
                myConn.Close()
            Catch ex As Exception
                MsgBox("Fail to close connection.", MsgBoxStyle.OkOnly, "PCMS")
            End Try
        End If
    End Sub

    Private Function getDate(ByVal dateStr As String) As Date

        Dim inputDate As Date

        If Trim(dateStr) <> "" Then
            Dim arr As String()
            arr = dateStr.Split("/")
            If arr.Length <> 2 Then
                Return Date.MinValue
            Else
                Try
                    inputDate = New Date(Integer.Parse(arr(1)), Integer.Parse(arr(0)), 1)

                    If inputDate.Year < 1900 Then
                        Return Date.MinValue
                    End If

                Catch EX As Exception
                    Return Date.MinValue
                End Try
            End If
        Else
            Return Date.MinValue
        End If

        Return inputDate

    End Function

    Private Sub checkSupervisor()
        If Trim(mySetting.CurrentUser) <> "" Then

            Dim myCmd As SqlCommand
            Dim myReader As SqlDataReader
            Dim sql As String

            sql = "select SUPERVISOR from v_sc_users where Upper(LOGINNAME) = '" & mySetting.CurrentUser & "'"

            myCmd = New SqlCommand(sql, myConn)
            myReader = myCmd.ExecuteReader()

            If myReader.Read() Then
                mySetting.CurrentUserIsS = myReader.Item(0).ToString() = "1"
                myReader.Close()
                myCmd.Dispose()
            Else
                mySetting.CurrentUserIsS = False
                myReader.Close()
                myCmd.Dispose()
            End If
        Else
            mySetting.CurrentUserIsS = False
        End If
    End Sub

    Private Sub btn_switchuser_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_switchuser.Click
        Me.Close()
        Me.DialogResult = Windows.Forms.DialogResult.Retry
    End Sub

End Class
