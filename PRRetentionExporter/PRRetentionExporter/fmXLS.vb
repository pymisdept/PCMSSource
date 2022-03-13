Imports System.Data.SqlClient
Imports System.Threading
Imports System.ComponentModel
Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices
Imports System.IO
Imports System.Runtime.Serialization.Formatters.Binary

Imports System.Environment
Public Class fmXLS

    Private myConn As SqlConnection
    Private myConfig As Config
    Private isStart As Boolean

    Private ws1Name As String
    Private ws2Name As String

    Private oExcel As Excel.Application
    Private oBook As Excel.Workbook
    Private oSheet_D1 As Excel.Worksheet
    Private oSheet_D2 As Excel.Worksheet
    Private oSheet_DR As Excel.Worksheet

#Region "Form"

    Private Sub fmXLS_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = Me.Text & " (Version: " & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build & ")"
    End Sub

#End Region

#Region "Constructor"

    Sub New()
        InitializeComponent()
        myConn = New SqlConnection()
        myConfig = New Config(My.Resources.SettingFile)

        If myConfig.loadSetting() = False Then
            MsgBox("Unable to load the configuration file, please check the config file")
            End 'Force Close
        Else
            If myConfig.BEDataSource = My.Resources.PROD_BE And myConfig.FEDataSource = My.Resources.PROD_FE Then
                lbl_testingSvr.Visible = False
            Else
                lbl_testingSvr.Visible = True
            End If
        End If

        getColumnsFromCPSPRXMAP()
        setWSName()
        ImportHistory()
        Dim dt As Date = DateSerial(Year(Today()), Month(Today()), 0)
        txt_fromPeriod.Text = dt.Month.ToString().PadLeft(2, "0") & "/" & dt.Year.ToString().PadLeft(4, "0")
        txt_toPeriod.Text = txt_fromPeriod.Text
    End Sub

#End Region

#Region "Textbox Event"

    Private Sub txt_fromPeriod_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_fromPeriod.TextChanged
        Dim str As String = ""
        If txt_fromPeriod.Text.Length = 6 And Not txt_fromPeriod.Text.Contains("/") Then
            str = txt_fromPeriod.Text.Substring(0, 2) & "/" & txt_fromPeriod.Text.Substring(2, 4)
            txt_fromPeriod.Text = str
        ElseIf txt_fromPeriod.Text.Length = 7 And txt_fromPeriod.Text.Contains("/") Then
            txt_toPeriod.Focus()
        End If
        EnableExportButton()
    End Sub

    Private Sub txt_toPeriod_TextChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles txt_toPeriod.TextChanged
        Dim str As String = ""
        If txt_toPeriod.Text.Length = 6 And Not txt_toPeriod.Text.Contains("/") Then
            str = txt_toPeriod.Text.Substring(0, 2) & "/" & txt_toPeriod.Text.Substring(2, 4)
            txt_toPeriod.Text = str
        ElseIf txt_toPeriod.Text.Length = 7 And txt_toPeriod.Text.Contains("/") Then
            txt_PrjCode.Focus()
        End If
        EnableExportButton()
    End Sub

#End Region

#Region "Checkbox List Event"

    Private Sub cbx_Result_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbx_Result.SelectedIndexChanged
        checkCBXState("RES")
    End Sub

    Private Sub cbx_Export_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbx_Export.SelectedIndexChanged
        checkCBXState("EXP")
        EnableExportButton()
    End Sub

    Private Sub cbx_PR2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbx_PR2.SelectedIndexChanged
        checkCBXState("PR2")
        EnableExportButton()
    End Sub

    Private Sub cbx_TD2_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbx_TD2.SelectedIndexChanged
        checkCBXState("TD2")
        EnableExportButton()
    End Sub

    Private Sub cbx_PR4_SelectedIndexChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cbx_PR4.SelectedIndexChanged
        checkCBXState("PR4")
        EnableExportButton()
    End Sub

#End Region

#Region "Checkbox Event"

    Private Sub cb_PR2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cb_PR2.Click
        If cb_PR2.CheckState = Windows.Forms.CheckState.Checked Then
            For idx As Integer = 0 To cbx_PR2.Items.Count - 1
                Me.cbx_PR2.SetItemCheckState(idx, Windows.Forms.CheckState.Checked)
            Next
        ElseIf cb_PR2.CheckState = Windows.Forms.CheckState.Unchecked Then
            For idx As Integer = 0 To cbx_PR2.Items.Count - 1
                Me.cbx_PR2.SetItemCheckState(idx, Windows.Forms.CheckState.Unchecked)
            Next
        End If
        EnableExportButton()
    End Sub

    Private Sub cb_TD2_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cb_TD2.Click
        If cb_TD2.CheckState = Windows.Forms.CheckState.Checked Then
            For idx As Integer = 0 To cbx_TD2.Items.Count - 1
                Me.cbx_TD2.SetItemCheckState(idx, Windows.Forms.CheckState.Checked)
            Next
        ElseIf cb_TD2.CheckState = Windows.Forms.CheckState.Unchecked Then
            For idx As Integer = 0 To cbx_TD2.Items.Count - 1
                Me.cbx_TD2.SetItemCheckState(idx, Windows.Forms.CheckState.Unchecked)
            Next
        End If
        EnableExportButton()
    End Sub

    Private Sub cb_PR4_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cb_PR4.Click
        If cb_PR4.CheckState = Windows.Forms.CheckState.Checked Then
            For idx As Integer = 0 To cbx_PR4.Items.Count - 1
                Me.cbx_PR4.SetItemCheckState(idx, Windows.Forms.CheckState.Checked)
            Next
        ElseIf cb_PR4.CheckState = Windows.Forms.CheckState.Unchecked Then
            For idx As Integer = 0 To cbx_PR4.Items.Count - 1
                Me.cbx_PR4.SetItemCheckState(idx, Windows.Forms.CheckState.Unchecked)
            Next
        End If
        EnableExportButton()
    End Sub

    Private Sub cb_result_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cb_result.Click
        If cb_result.CheckState = Windows.Forms.CheckState.Checked Then
            For idx As Integer = 0 To cbx_Result.Items.Count - 1
                Me.cbx_Result.SetItemCheckState(idx, Windows.Forms.CheckState.Checked)
            Next
        ElseIf cb_result.CheckState = Windows.Forms.CheckState.Unchecked Then
            For idx As Integer = 0 To cbx_Result.Items.Count - 1
                Me.cbx_Result.SetItemCheckState(idx, Windows.Forms.CheckState.Unchecked)
            Next
        End If
        EnableExportButton()
    End Sub

    Private Sub cb_projects_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cb_projects.Click
        If cb_projects.CheckState = Windows.Forms.CheckState.Checked Then
            For idx As Integer = 0 To cbx_Export.Items.Count - 1
                Me.cbx_Export.SetItemCheckState(idx, Windows.Forms.CheckState.Checked)
            Next
        ElseIf cb_projects.CheckState = Windows.Forms.CheckState.Unchecked Then
            For idx As Integer = 0 To cbx_Export.Items.Count - 1
                Me.cbx_Export.SetItemCheckState(idx, Windows.Forms.CheckState.Unchecked)
            Next
        End If
        EnableExportButton()
    End Sub

#End Region

#Region "Button Event"

    Private Sub bt_retrieve_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_retrieve.Click

        cbx_Result.Items.Clear()
        checkSQLConnection()

        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader
        Dim sql As String
        Dim fromDate As Date
        Dim toDate As Date

        Dim isErr As Boolean = False
        Dim ErrMsg As String = ""

        If Trim(txt_fromPeriod.Text) <> "" Then
            Dim arr As String()
            arr = txt_fromPeriod.Text.Split("/")
            If arr.Length <> 2 Then
                isErr = True
                ErrMsg = ErrMsg & "Invalid from period" & Environment.NewLine
            Else
                Try
                    fromDate = New Date(Integer.Parse(arr(1)), Integer.Parse(arr(0)), 1)
                Catch EX As ArgumentOutOfRangeException
                    isErr = True
                    ErrMsg = ErrMsg & "Invalid from period" & Environment.NewLine
                End Try
            End If
        Else
            isErr = True
            ErrMsg = ErrMsg & "Invalid from period" & Environment.NewLine
        End If

        If Trim(txt_toPeriod.Text) <> "" Then
            Dim arr As String()
            arr = txt_toPeriod.Text.Split("/")
            If arr.Length <> 2 Then
                isErr = True
                ErrMsg = ErrMsg & "Invalid to period" & Environment.NewLine
            Else
                Try
                    toDate = New Date(Integer.Parse(arr(1)), Integer.Parse(arr(0)), 1)
                    toDate = toDate.AddMonths(1).AddDays(-1)
                Catch EX As ArgumentOutOfRangeException
                    isErr = True
                    ErrMsg = ErrMsg & "Invalid to period" & Environment.NewLine
                End Try
            End If
        Else
            isErr = True
            ErrMsg = ErrMsg & "Invalid to period" & Environment.NewLine
        End If

        If isErr = True Then
            MsgBox(ErrMsg)
            Exit Sub
        End If

        sql = "select distinct ProjectCode from PCMS_FE.PCMS800.dbo.DocumentProperty where ProjectCode like '%" & Trim(txt_PrjCode.Text) & "%' AND Type = '1040' AND DocStatus IN ('D', 'P') AND dt01 between '" & fromDate.ToString("yyyy-MM-dd") & "' AND '" & toDate.ToString("yyyy-MM-dd") & "'"

        myCmd = New SqlCommand(sql, myConn)
        myReader = myCmd.ExecuteReader()
        While myReader.Read()
            cbx_Result.Items.Add(myReader.Item(0), True)
        End While

        checkCBXState("RES")
        myReader.Close()
        myCmd.Dispose()

    End Sub

    Private Sub btn_import_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_import.Click
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

                checkCBXState("RES")
                MsgBox(count & " project(s) imported")
            End If
        Catch ex As Exception
            MsgBox("Failed to import text file")
        End Try
    End Sub

    Private Sub btn_Export_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Export.Click
        checkSQLConnection()

        btn_Export.Enabled = False
        btn_retrieve.Enabled = False
        btn_import.Enabled = False
        btn_addPrj.Enabled = False
        btn_deletePrj.Enabled = False

        lbl_processing.Visible = True

        txt_fromPeriod.Enabled = False
        txt_toPeriod.Enabled = False
        txt_PrjCode.Text = ""
        txt_PrjCode.Enabled = False

        cbx_Export.SelectionMode = SelectionMode.None
        cbx_Result.SelectionMode = SelectionMode.None
        cbx_PR2.SelectionMode = SelectionMode.None
        cbx_PR4.SelectionMode = SelectionMode.None
        cbx_TD2.SelectionMode = SelectionMode.None

        cb_projects.Enabled = False
        cb_result.Enabled = False
        cb_PR2.Enabled = False
        cb_PR4.Enabled = False
        cb_TD2.Enabled = False

        isStart = True

        ExportHistory()

        bgWorker.RunWorkerAsync()
    End Sub

    Private Sub btn_addPrj_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_addPrj.Click

        If cbx_Result.CheckedItems.Count > 0 Then
            For Each itm As String In cbx_Result.CheckedItems
                If Not cbx_Export.Items.Contains(itm) Then
                    cbx_Export.Items.Add(itm, True)
                End If
            Next
            checkCBXState("EXP")
            EnableExportButton()
        Else
            MsgBox("Please select a project")
        End If

    End Sub

    Private Sub btn_deletePrj_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_deletePrj.Click
        If cbx_Export.CheckedItems.Count > 0 Then
            For i As Integer = cbx_Export.CheckedIndices.Count - 1 To 0 Step -1
                cbx_Export.Items.RemoveAt(cbx_Export.CheckedIndices.Item(i))
            Next
            checkCBXState("EXP")
            EnableExportButton()
        Else
            MsgBox("Please select a project")
        End If
    End Sub

#End Region

#Region "Background Worker"

    Private Sub bgWorker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bgWorker.DoWork
        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)

        If (worker.CancellationPending) Then
            e.Cancel = True
            Return
        End If

        doExport(worker)
    End Sub

    Private Sub bgWorker_ProgressChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bgWorker.ProgressChanged
        Me.progressbar.Value = e.ProgressPercentage
    End Sub

    Private Sub bgWorker_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgWorker.RunWorkerCompleted

        btn_addPrj.Enabled = True
        btn_deletePrj.Enabled = True
        btn_retrieve.Enabled = True
        btn_import.Enabled = True
        btn_Export.Enabled = True

        lbl_processing.Visible = False

        txt_fromPeriod.Enabled = True
        txt_toPeriod.Enabled = True
        txt_PrjCode.Enabled = True

        cbx_Result.SelectionMode = SelectionMode.One
        cbx_Export.SelectionMode = SelectionMode.One
        cbx_PR2.SelectionMode = SelectionMode.One
        cbx_PR4.SelectionMode = SelectionMode.One
        cbx_TD2.SelectionMode = SelectionMode.One

        cb_PR2.Enabled = True
        cb_PR4.Enabled = True
        cb_TD2.Enabled = True
        cb_projects.Enabled = True
        cb_result.Enabled = True

        isStart = False

    End Sub

#End Region

#Region "Custom Function"

    Private Sub checkCBXState(ByVal val As String)

        Dim cbl, cb

        If val = "RES" Then
            cbl = cbx_Result
            cb = cb_result
        ElseIf val = "EXP" Then
            cbl = cbx_Export
            cb = cb_projects
        ElseIf val = "PR2" Then
            cbl = cbx_PR2
            cb = cb_PR2
        ElseIf val = "TD2" Then
            cbl = cbx_TD2
            cb = cb_TD2
        Else
            cbl = cbx_PR4
            cb = cb_PR4
        End If

        If cbl.CheckedItems.Count = 0 Then
            cb.CheckState = Windows.Forms.CheckState.Unchecked
        ElseIf cbl.CheckedItems.Count <> cbl.Items.Count Then
            cb.CheckState = Windows.Forms.CheckState.Indeterminate
        ElseIf cbl.CheckedItems.Count = cbl.Items.Count Then
            cb.CheckState = Windows.Forms.CheckState.Checked
        End If

    End Sub

    Private Sub EnableExportButton()

        If isStart = False Then
            Dim isCBChecked As Boolean
            isCBChecked = cbx_PR2.CheckedItems.Count > 0 Or cbx_PR4.CheckedItems.Count > 0 Or cbx_TD2.CheckedItems.Count > 0

            If Trim(txt_fromPeriod.Text).Length > 0 And Trim(txt_toPeriod.Text).Length > 0 And isCBChecked = True And cbx_Export.CheckedItems.Count > 0 Then
                btn_Export.Enabled = True
            Else
                btn_Export.Enabled = False
            End If
        Else
            btn_Export.Enabled = False
        End If

    End Sub

    Private Sub checkSQLConnection()
        ' Username & Password

        Dim userName As String = My.Resources.BE_DBUserName
        Dim userPwd As String = My.Resources.BE_DBPassword
        ' ''''''''''
        Dim source As String = myConfig.BEDataSource
        Dim dbname As String = myConfig.BEDatabase

        If myConn.State <> ConnectionState.Open Then
            Dim connectionStr = "Initial Catalog=" & dbname & ";" & _
                    "Data Source=" & source & ";User ID=" & userName & ";Password=" & userPwd

            myConn = New SqlConnection(connectionStr)

            Try
                myConn.Open()
            Catch ex As Exception
                MsgBox("Fail to open connection.")
            End Try
        End If
    End Sub

    Private Function isProjectExist(ByVal PrjCode As String)

        If Trim(PrjCode) <> "" Then
            checkSQLConnection()


            Dim myCmd As SqlCommand
            Dim myReader As SqlDataReader
            Dim sql As String

            sql = "select 1 from OPRJ where PrjCode = '" & PrjCode & "'"

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
        Dim tmpStr As String = ""
        Dim Flag As String = "N"
        Dim dir As String = GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\\" & My.Resources.ApplicationName

        Try
            cbx_Export.Items.Clear()

            If Not Directory.Exists(dir) Then
                Directory.CreateDirectory(dir)
            End If

            Using sr As New StreamReader(dir + "\\" + My.Resources.HistoryFile)
                While sr.EndOfStream = False
                    tmpStr = sr.ReadLine()
                    If tmpStr = "[VERSION]" Then
                        tmpStr = sr.ReadLine()
                        If Trim(tmpStr) <> My.Resources.HistoryVersion Then
                            Exit Sub
                        End If
                    ElseIf tmpStr = "[SELECTED PERIOD]" Then
                        tmpStr = sr.ReadLine()
                        txt_fromPeriod.Text = tmpStr

                        tmpStr = sr.ReadLine()
                        txt_toPeriod.Text = tmpStr
                    ElseIf tmpStr = "[PROJECTS]" Then
                        Flag = "PRJ"
                    ElseIf tmpStr = "[PR2 OUPUT]" Then
                        Flag = "PR2"
                    ElseIf tmpStr = "[TD2 OUPUT]" Then
                        Flag = "TD2"
                    ElseIf tmpStr = "[PR4 OUPUT]" Then
                        Flag = "PR4"
                    End If

                    If Flag = "PRJ" Then
                        If tmpStr <> "[PROJECTS]" And tmpStr <> "" Then
                            cbx_Export.Items.Add(Trim(tmpStr), False)
                        End If
                    ElseIf Flag = "PR2" Then
                        If tmpStr <> "[PR2 OUPUT]" And tmpStr <> "" Then
                            cbx_PR2.SetItemCheckState(cbx_PR2.Items.IndexOf(tmpStr), CheckState.Checked)
                        End If
                    ElseIf Flag = "TD2" Then
                        If tmpStr <> "[TD2 OUPUT]" And tmpStr <> "" Then
                            cbx_TD2.SetItemCheckState(cbx_TD2.Items.IndexOf(tmpStr), CheckState.Checked)
                        End If
                    ElseIf Flag = "PR4" Then
                        If tmpStr <> "[PR4 OUPUT]" And tmpStr <> "" Then
                            cbx_PR4.SetItemCheckState(cbx_PR4.Items.IndexOf(tmpStr), CheckState.Checked)
                        End If
                    End If
                End While
                sr.Close()
            End Using

            checkCBXState("PR2")
            checkCBXState("TD2")
            checkCBXState("PR4")

        Catch ex As Exception
            '  MsgBox(ex.Message)
            ' MsgBox(ex.InnerException)
        End Try
    End Sub

    Private Sub ExportHistory()
        Dim sw As System.IO.StreamWriter
        Dim dir As String = GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\\" & My.Resources.ApplicationName

        sw = My.Computer.FileSystem.OpenTextFileWriter(dir + "\\" + My.Resources.HistoryFile, False)

        sw.WriteLine("[VERSION]")
        sw.WriteLine(My.Resources.HistoryVersion)
        sw.WriteLine(String.Empty)

        sw.WriteLine("[SELECTED PERIOD]")
        sw.WriteLine(txt_fromPeriod.Text)
        sw.WriteLine(txt_toPeriod.Text)
        sw.WriteLine(String.Empty)

        sw.WriteLine("[PROJECTS]")
        If cbx_Export.CheckedItems.Count > 0 Then
            For Each itm As String In cbx_Export.CheckedItems
                sw.WriteLine(Trim(itm))
            Next
            sw.WriteLine(String.Empty)
        End If

        sw.WriteLine("[PR2 OUPUT]")
        If cbx_PR2.CheckedItems.Count > 0 Then
            For Each itm As String In cbx_PR2.CheckedItems
                sw.WriteLine(Trim(itm))
            Next
            sw.WriteLine(String.Empty)
        End If

        sw.WriteLine("[TD2 OUPUT]")
        If cbx_TD2.CheckedItems.Count > 0 Then
            For Each itm As String In cbx_TD2.CheckedItems
                sw.WriteLine(Trim(itm))
            Next
            sw.WriteLine(String.Empty)
        End If

        sw.WriteLine("[PR4 OUPUT]")
        If cbx_PR4.CheckedItems.Count > 0 Then
            For Each itm As String In cbx_PR4.CheckedItems
                sw.WriteLine(Trim(itm))
            Next
            sw.WriteLine(String.Empty)
        End If

        sw.Close()
    End Sub

    Private Sub doExport(ByVal worker As BackgroundWorker)
        Dim progress, totalCount As Integer

        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader
        Dim sql, sql2 As String
        Dim fromDate As Date
        Dim toDate As Date

        Dim d1_row, d1_col, d2_row, d2_col As Integer

        Dim WS2Body As String = ""
        Dim isWS2 As Boolean = False

        Dim prjText As String = ""
        Dim mapDesc As String = ""
        Dim ErrMsg As String = ""

        Dim isErr As Boolean = False

        worker.ReportProgress(0)

        '''' Error Handle
        If Trim(txt_fromPeriod.Text) <> "" Then
            Dim arr As String()
            arr = txt_fromPeriod.Text.Split("/")
            If arr.Length <> 2 Then
                isErr = True
                ErrMsg = ErrMsg & "Invalid from period" & Environment.NewLine
            Else
                Try
                    fromDate = New Date(Integer.Parse(arr(1)), Integer.Parse(arr(0)), 1)
                Catch EX As Exception
                    isErr = True
                    ErrMsg = ErrMsg & "Invalid from period" & Environment.NewLine
                End Try
            End If
        Else
            isErr = True
            ErrMsg = ErrMsg & "Invalid from period" & Environment.NewLine
        End If

        If Trim(txt_toPeriod.Text) <> "" Then
            Dim arr As String()
            arr = txt_toPeriod.Text.Split("/")
            If arr.Length <> 2 Then
                isErr = True
                ErrMsg = ErrMsg & "Invalid to period" & Environment.NewLine
            Else
                Try
                    toDate = New Date(Integer.Parse(arr(1)), Integer.Parse(arr(0)), 1)
                    toDate = toDate.AddMonths(1).AddDays(-1)
                Catch EX As Exception
                    isErr = True
                    ErrMsg = ErrMsg & "Invalid to period" & Environment.NewLine
                End Try
            End If
        Else
            isErr = True
            ErrMsg = ErrMsg & "Invalid to period" & Environment.NewLine
        End If

        If isErr = False Then
            ' Updated by Ken, 2016-12-08, remove day limit
            'Dim ts As TimeSpan = toDate.Subtract(fromDate)
            'If ts.TotalDays > My.Resources.ExportPeriod_Limit * 30.5 Then
            'isErr = True
            'ErrMsg = ErrMsg & "You are just allow to export data within " & My.Resources.ExportPeriod_Limit & " months in each export" & Environment.NewLine
            'End If

            If fromDate.Year < 1900 Then
                isErr = True
                ErrMsg = ErrMsg & "Invalid from period" & Environment.NewLine
            End If

            If toDate.Year < 1900 Then
                isErr = True
                ErrMsg = ErrMsg & "Invalid to period" & Environment.NewLine
            End If
        End If

        If cbx_Export.CheckedItems.Count > 0 Then
            For Each itm As String In cbx_Export.CheckedItems
                prjText = prjText & "'" & itm & "',"
            Next
            prjText = prjText.Remove(prjText.Length - 1, 1)
            ' Updated by Ken, 2016-12-08, remove project code limit
            'Else
            'isErr = True
            'ErrMsg = ErrMsg & "You are just allow to export (max) " & My.Resources.ExportPrj_Limit & " projects in each export" & Environment.NewLine
        End If

        If isErr = False Then
            sql = "select count(*) from PCMS_FE.PCMS800.dbo.DocumentProperty where ProjectCode IN (" & prjText & ") AND DocStatus IN ('D', 'P') AND Type = '1040' AND dt01 between '" & fromDate.ToString("yyyy-MM-dd") & "' AND '" & toDate.ToString("yyyy-MM-dd") & "'"
            WriteLog(sql)
            myCmd = New SqlCommand(sql, myConn)
            myReader = myCmd.ExecuteReader()

            If myReader.Read() Then
                If myReader(0) = 0 Then
                    isErr = True
                    ErrMsg = ErrMsg & "No record found" & Environment.NewLine
                Else
                    totalCount = CInt(myReader(0))
                End If
            End If

            myReader.Close()
            myCmd.Dispose()
        End If

        If isErr = True Then
            MsgBox(ErrMsg, MsgBoxStyle.OkOnly, "PCMS")
            Exit Sub
        End If

        ''' Start Excel Application
        oExcel = Nothing
        oBook = Nothing
        oSheet_D1 = Nothing
        oSheet_D2 = Nothing
        oSheet_DR = Nothing

        ' Set Excel localization 
        Dim oldCI As System.Globalization.CultureInfo = System.Threading.Thread.CurrentThread.CurrentCulture
        System.Threading.Thread.CurrentThread.CurrentCulture = New System.Globalization.CultureInfo("en-US")

        Try
            oExcel = CreateObject("Excel.Application")
            oBook = oExcel.Workbooks.Add(Type.Missing)

            oSheet_DR = oBook.Worksheets.Add()
            oSheet_D2 = oBook.Worksheets.Add()
            oSheet_D1 = oBook.Worksheets.Add()
            oSheet_D1.Name = ws1Name
            oSheet_D2.Name = ws2Name
            oSheet_DR.Name = ws2Name & "_Remarks"

            setD2_Remarks(oSheet_DR)
        Catch ex As Exception
            MsgBox(ex.Message)
            MsgBox("Failed to start Excel Application")
        End Try

        If oExcel Is Nothing Or oBook Is Nothing Or oSheet_D1 Is Nothing Or oSheet_D2 Is Nothing Or oSheet_DR Is Nothing Then
            Exit Sub
        End If

        Try
            oBook.Sheets("Sheet1").Delete()
            oBook.Sheets("Sheet2").Delete()
            oBook.Sheets("Sheet3").Delete()
        Catch ex As Exception
        End Try

        Try
            oBook.Sheets("工作表1").Delete()
            oBook.Sheets("工作表2").Delete()
            oBook.Sheets("工作表3").Delete()
        Catch ex As Exception
        End Try

        d1_row = 1
        d1_col = 1
        d2_row = 1
        d2_col = 1

        If cbx_PR2.CheckedItems.Count > 0 Then
            For Each itm As String In cbx_PR2.CheckedItems
                mapDesc = mapDesc & "'" & itm & "',"
            Next
        End If

        If cbx_TD2.CheckedItems.Count > 0 Then
            For Each itm As String In cbx_TD2.CheckedItems
                mapDesc = mapDesc & "'" & itm & "',"
            Next
        End If

        If cbx_PR4.CheckedItems.Count > 0 Then
            For Each itm As String In cbx_PR4.CheckedItems
                mapDesc = mapDesc & "'" & itm & "',"
            Next
        End If

        mapDesc = mapDesc.Remove(mapDesc.Length - 1, 1)

        ' ''' Get data from CPSPRXMAP (FE)
        sql = "select ID, Description,  Tbl, TblType, TblKey, DataType, Fld, SourceWS, DestWS from PCMS_FE.PCMS800.dbo.CPSPRXMAP where Description IN (" & mapDesc & ") Or (SourceWS = 'All' AND DestWS = 'All') order by Seq"
        WriteLog(sql)
        myCmd = New SqlCommand(sql, myConn)
        myReader = myCmd.ExecuteReader()

        Dim dict_DB1 As Dictionary(Of String, CPSMapping)
        Dim dict_DB2 As Dictionary(Of String, CPSMapping)
        Dim tmpDict As Dictionary(Of String, CPSMapping)
        Dim myMapping As CPSMapping, tempStr, tempCnt
        Dim tempArr As List(Of String)
        Dim hdr, bdy As String

        tempArr = New List(Of String)
        dict_DB1 = New Dictionary(Of String, CPSMapping)
        dict_DB2 = New Dictionary(Of String, CPSMapping)
        tmpDict = New Dictionary(Of String, CPSMapping)

        hdr = ""
        bdy = ""

        While myReader.Read()

            If myReader("DestWS") = ws1Name Then
                tmpDict = dict_DB1
                oSheet_D1.Cells(d1_row, d1_col) = myReader("Description")
                d1_col += 1
            ElseIf myReader("DestWS") = ws2Name Then
                If (myReader("Tbl") = "NA" And myReader("TblType") = "Body") Then
                    If hdr.Length > 0 Then
                        oSheet_D2.Cells(d2_row, d2_col) = myReader("Description")
                        d2_col += 1
                    End If
                ElseIf myReader("Tbl") <> "NA" Then
                    oSheet_D2.Cells(d2_row, d2_col) = myReader("Description")
                    d2_col += 1
                End If
                tmpDict = dict_DB2
            Else

                If myReader("SourceWS") = "All" And myReader("DestWS") = "All" Then
                    oSheet_D1.Cells(d1_row, d1_col) = myReader("Description") & " *"
                    oSheet_D2.Cells(d2_row, d2_col) = myReader("Description") & " *"

                    oSheet_D1.Cells(d1_row, d1_col).Font.ColorIndex = 3
                    oSheet_D2.Cells(d2_row, d2_col).Font.ColorIndex = 3
                Else
                    oSheet_D1.Cells(d1_row, d1_col) = myReader("Description")
                    oSheet_D2.Cells(d2_row, d2_col) = myReader("Description")
                End If

                tmpDict = Nothing
                d1_col += 1
                d2_col += 1
            End If

            If myReader("Tbl") = "NA" Then
                If myReader("TblType") = "Header" Then
                    hdr = hdr & myReader("ID") & ";"
                Else
                    bdy = bdy & myReader("ID") & ";"
                End If
                Continue While
            End If

            If tmpDict Is Nothing Then
                Dim k As Integer = 0
                For k = 0 To 1
                    If k = 0 Then
                        tmpDict = dict_DB1
                    ElseIf k = 1 Then
                        tmpDict = dict_DB2
                    Else
                        Exit For
                    End If

                    If tmpDict.ContainsKey(myReader("Tbl")) Then
                        tempStr = tmpDict(myReader("Tbl")).Fields
                        tempCnt = tmpDict(myReader("Tbl")).FieldCount
                        tempArr = tmpDict(myReader("Tbl")).FieldType

                        'tmpDict(myReader("Tbl")).Fields = tempStr & ", " & tmpInitial & "." & myReader("Fld")
                        tmpDict(myReader("Tbl")).Fields = tempStr & ", " & myReader("Fld")
                        tmpDict(myReader("Tbl")).FieldCount = tempCnt + 1

                        tempArr.Add(myReader("DataType"))
                        tmpDict(myReader("Tbl")).FieldType = tempArr
                    Else
                        tempArr = New List(Of String)
                        myMapping = New CPSMapping

                        myMapping.TableName = myReader("Tbl")
                        myMapping.TableType = myReader("TblType")
                        myMapping.TableKey = myReader("TblKey")
                        myMapping.Fields = myReader("Fld")
                        myMapping.FieldCount = 1
                        myMapping.IsHeader = True

                        tempArr.Add(myReader("DataType"))
                        myMapping.FieldType = tempArr

                        tmpDict.Add(myReader("Tbl"), myMapping)
                    End If
                Next
            Else
                If tmpDict.ContainsKey(myReader("Tbl")) Then
                    tempStr = tmpDict(myReader("Tbl")).Fields
                    tempCnt = tmpDict(myReader("Tbl")).FieldCount
                    tempArr = tmpDict(myReader("Tbl")).FieldType

                    'tmpDict(myReader("Tbl")).Fields = tempStr & ", " & tmpInitial & "." & myReader("Fld")
                    tmpDict(myReader("Tbl")).Fields = tempStr & ", " & myReader("Fld")
                    tmpDict(myReader("Tbl")).FieldCount = tempCnt + 1

                    tempArr.Add(myReader("DataType"))
                    tmpDict(myReader("Tbl")).FieldType = tempArr
                Else
                    tempArr = New List(Of String)
                    myMapping = New CPSMapping
                    myMapping.TableName = myReader("Tbl")
                    myMapping.TableType = myReader("TblType")
                    myMapping.TableKey = myReader("TblKey")
                    myMapping.Fields = myReader("Fld")
                    myMapping.FieldCount = 1

                    tempArr.Add(myReader("DataType"))
                    myMapping.FieldType = tempArr

                    tmpDict.Add(myReader("Tbl"), myMapping)
                End If
            End If
        End While

        myReader.Close()
        myCmd.Dispose()

        Dim i, j As Integer
        Dim conditionKey As String
        Dim conditionKeyArr As List(Of String)
        Dim hdrArr = hdr.Split(";")
        Dim bdyArr = bdy.Split(";")
        conditionKey = ""
        conditionKeyArr = New List(Of String)

        If hdrArr.Length > 1 And bdyArr.Length > 1 Then

            For i = 1 To hdrArr.Length
                conditionKey = ""
                For j = 1 To bdyArr.Length
                    If hdrArr(i - 1) <> "" And bdyArr(j - 1) <> "" Then
                        conditionKey = conditionKey & "'" & hdrArr(i - 1) & "," & bdyArr(j - 1) & "',"
                    End If
                Next
                If conditionKey <> "" Then
                    conditionKey = conditionKey.Remove(conditionKey.Length - 1, 1)
                    conditionKeyArr.Add(conditionKey)
                End If
            Next
        End If

        If conditionKeyArr.Count > 0 Then
            tmpDict = dict_DB2
            For b As Integer = 0 To conditionKeyArr.Count - 1
                conditionKey = conditionKeyArr(b)
                If conditionKey <> "" Then
                    sql = "select Description, ID, Tbl, TblType, TblKey, DataType, Fld, DestWS from PCMS_FE.PCMS800.dbo.CPSPRXMAP where Condition IN (" & conditionKey & ") order by Seq"
                    WriteLog(sql)
                    myCmd = New SqlCommand(sql, myConn)
                    myReader = myCmd.ExecuteReader()


                    If tmpDict.ContainsKey("CPSTRR") Then
                        myMapping = CopyCPSMapping(tmpDict("CPSTRR"))
                    Else
                        myMapping = New CPSMapping()
                    End If
                    tempArr = CopyStringList(myMapping.FieldType)

                    While myReader.Read()
                        If myReader("Tbl") <> "NA" Then
                            myMapping.TableName = myReader("Tbl")
                            myMapping.TableType = myReader("TblType")
                            myMapping.TableKey = myReader("TblKey")
                            myMapping.IsHeader = False

                            If myMapping.FieldCount = 0 Then
                                myMapping.Fields = myReader("Fld")
                            Else
                                myMapping.Fields = myMapping.Fields & ", " & myReader("Fld")
                            End If

                            myMapping.FieldCount += 1

                            If myReader("Description").Contains("-") Then
                                myMapping.RowType = myReader("Description").Split("-")(0)
                            ElseIf myReader("Description") <> "" Then
                                myMapping.RowType = ""
                            End If

                            tempArr.Add(myReader("DataType"))
                            myMapping.FieldType = tempArr
                        End If
                    End While

                    tmpDict.Add(myMapping.TableName & "_" & b, myMapping)
                    myMapping = Nothing
                    myReader.Close()
                    myCmd.Dispose()
                End If
            Next
            If tmpDict.ContainsKey("CPSTRR") Then
                dict_DB2.Remove("CPSTRR")
                tmpDict = Nothing
            End If
        End If


        oSheet_D1.Range("A1:" & ColumnIndexToColumnLetter(d1_col - 1) & "1").Interior.ColorIndex = 40
        oSheet_D1.Range("A1:" & ColumnIndexToColumnLetter(d1_col - 1) & "1").HorizontalAlignment = Excel.Constants.xlCenter

        oSheet_D2.Range("A1:" & ColumnIndexToColumnLetter(d2_col - 1) & "1").Interior.ColorIndex = 40
        oSheet_D2.Range("A1:" & ColumnIndexToColumnLetter(d2_col - 1) & "1").HorizontalAlignment = Excel.Constants.xlCenter

        ''' Get data from DocumentProperty (FE)
        sql = "select d.ID, d.DocEntry, d.ProjectCode, d.dt01, p.U_ProjectFullName from PCMS_FE.PCMS800.dbo.DocumentProperty d, OPRJ p where d.ProjectCode = p.PrjCode AND ProjectCode IN (" & prjText & ") AND DocStatus IN ('D', 'P') AND Type = '1040' AND dt01 between '" & fromDate.ToString("yyyy-MM-dd") & "' AND '" & toDate.ToString("yyyy-MM-dd") & "' order by ProjectCode, ID"
        WriteLog(sql)
        myCmd = New SqlCommand(sql, myConn)
        myReader = myCmd.ExecuteReader()

        Dim selectColumn As Integer

        Dim arrList As List(Of DocumentProperty) = New List(Of DocumentProperty)

        Dim doc As DocumentProperty

        While myReader.Read()
            doc = New DocumentProperty()
            doc.ID = myReader("ID")
            doc.DocEntry = myReader("DocEntry")
            doc.ProjectCode = "'" & myReader("ProjectCode") & "'"
            arrList.Add(doc)
        End While

        myReader.Close()
        myCmd.Dispose()

        d1_col = 1
        d2_col = 1
        d1_row = 2
        d2_row = 2

        Dim headerProject As Dictionary(Of String, String) = New Dictionary(Of String, String)
        Dim headerRow As Integer = 0
        Dim headerType As String = ""
        Dim flds As String = ""
        Dim whes As String = ""
        Dim tbls As String = ""
        For Each arr In arrList
            'sql2 = "select " & flds.Substring(1) & " from " & tbls.Substring(1) & " where " & whes.Substring(1)
            For Each kvp As KeyValuePair(Of String, CPSMapping) In dict_DB1
                Dim v1 As String = kvp.Key
                Dim v2 As CPSMapping = kvp.Value

                selectColumn = 0
                sql2 = ""
                ''' Get details from database (BE)
                If (v2.TableType = "Table") Then
                    sql2 = "select " & v2.Fields & " from " & v2.TableName & " where " & v2.TableKey & " = " & arr.getValueByName(v2.TableKey)
                ElseIf (v2.TableType = "Function") Then
                    sql2 = "select " & v2.Fields & " from " & v2.TableName & "(" & arr.getValueByName(v2.TableKey) & ") "
                End If

                If sql2 <> "" Then
                    WriteLog(sql2)
                    myCmd = New SqlCommand(sql2, myConn)
                    myReader = myCmd.ExecuteReader()

                    selectColumn = 0

                    If myReader.Read() Then
                        While selectColumn < v2.FieldCount
                            oSheet_D1.Cells(d1_row, d1_col) = myReader(selectColumn)

                            If v2.FieldType(selectColumn).ToString() = "Number" Then
                                oSheet_D1.Cells(d1_row, d1_col).NumberFormat = My.Resources.XLS_NumFormat
                            ElseIf v2.FieldType(selectColumn).ToString() = "Period" Then
                                oSheet_D1.Cells(d1_row, d1_col).NumberFormat = My.Resources.XLS_PeriodFormat
                            ElseIf v2.FieldType(selectColumn).ToString() = "Date" Then
                                oSheet_D1.Cells(d1_row, d1_col).NumberFormat = My.Resources.XLS_DateFormat
                            ElseIf v2.FieldType(selectColumn).ToString() = "Decimal" Then
                                oSheet_D1.Cells(d1_row, d1_col).NumberFormat = My.Resources.XLS_DecimalFormat
                            Else
                                oSheet_D1.Cells(d1_row, d1_col).NumberFormat = My.Resources.XLS_StringFormat
                            End If

                            d1_col += 1
                            selectColumn += 1
                        End While
                    End If

                    myReader.Close()
                    myCmd.Dispose()
                End If
            Next

            headerProject.Clear()
            headerRow = 0
            headerType = ""

            For Each kvp As KeyValuePair(Of String, CPSMapping) In dict_DB2
                Dim v1 As String = kvp.Key
                Dim v2 As CPSMapping = kvp.Value

                selectColumn = 0
                sql2 = ""
                If (v2.TableType = "Table") Then
                    sql2 = "select " & v2.Fields & " from " & v2.TableName & " where " & v2.TableKey & " = " & arr.getValueByName(v2.TableKey)
                ElseIf (v2.TableType = "Function") Then
                    sql2 = "select " & v2.Fields & " from " & v2.TableName & "(" & arr.getValueByName(v2.TableKey) & ")"
                End If

                If sql2 <> "" Then
                    WriteLog(sql2)
                    myCmd = New SqlCommand(sql2, myConn)
                    myReader = myCmd.ExecuteReader()

                    While myReader.Read()

                        selectColumn = 0

                        If d2_row <> headerRow Then
                            For Each pval As KeyValuePair(Of String, String) In headerProject
                                oSheet_D2.Cells(d2_row, d2_col) = pval.Key.Split("~~")(0)
                                oSheet_D2.Cells(d2_row, d2_col).NumberFormat() = pval.Value
                                d2_col += 1
                            Next
                        End If

                        While selectColumn < v2.FieldCount
                            oSheet_D2.Cells(d2_row, d2_col) = myReader(selectColumn)
                            If v2.FieldType(selectColumn).ToString() = "Number" Then
                                headerType = My.Resources.XLS_NumFormat
                            ElseIf v2.FieldType(selectColumn).ToString() = "Period" Then
                                headerType = My.Resources.XLS_PeriodFormat
                            ElseIf v2.FieldType(selectColumn).ToString() = "Date" Then
                                headerType = My.Resources.XLS_DateFormat
                            ElseIf v2.FieldType(selectColumn).ToString() = "Decimal" Then
                                headerType = My.Resources.XLS_DecimalFormat
                            Else
                                headerType = My.Resources.XLS_StringFormat
                            End If

                            oSheet_D2.Cells(d2_row, d2_col).NumberFormat() = headerType

                            If v2.IsHeader = True Then
                                headerProject.Add(myReader(selectColumn) & "~~" & selectColumn, headerType)
                                headerRow = d2_row
                            End If
                            d2_col += 1
                            selectColumn += 1
                        End While

                        customBGColor(oSheet_D2, d2_row, d2_col, v2.RowType)

                        If v2.IsHeader = False Then
                            d2_row += 1
                            d2_col = 1
                        End If

                    End While
                    myReader.Close()
                    myCmd.Dispose()
                End If
            Next

            d1_row += 1
            d1_col = 1

            If headerRow = d2_row Then
                d2_row += 1
                d2_col = 1
            End If

            progress += 1
            worker.ReportProgress(CInt((progress / totalCount) * 100))
        Next

        ''' Set Excel format
        oSheet_D1.Columns.Font.Name = "Arial"
        oSheet_D1.Columns.Font.Size = 10
        oSheet_D1.Columns.AutoFit()

        oSheet_D2.Columns.Font.Name = "Arial"
        oSheet_D2.Columns.Font.Size = 10
        oSheet_D2.Columns.AutoFit()

        oExcel.Visible = True

        Marshal.ReleaseComObject(oSheet_D1)
        Marshal.ReleaseComObject(oSheet_D2)
        Marshal.ReleaseComObject(oBook)
        Marshal.ReleaseComObject(oExcel)

        dict_DB1 = Nothing
        dict_DB2 = Nothing
        tmpDict = Nothing
        tempArr = Nothing
        arrList = Nothing
        headerProject = Nothing

        ' Reset Excel localization 
        System.Threading.Thread.CurrentThread.CurrentCulture = oldCI

    End Sub

    Private Sub WriteLog(ByVal text As String)
        If myConfig.LogSQL = True Then
            Dim file As System.IO.StreamWriter
            file = My.Computer.FileSystem.OpenTextFileWriter(My.Resources.SQLFile, True)
            file.WriteLine(DateTime.Now & "  -- " & text)
            file.Close()
        End If
    End Sub

    Private Function ColumnIndexToColumnLetter(ByVal colIndex As Integer) As String
        Dim div As Integer = colIndex
        Dim colLetter As String = String.Empty
        Dim modnum As Integer = 0

        While div > 0
            modnum = (div - 1) Mod 26
            colLetter = Chr(65 + modnum) & colLetter
            div = CInt((div - modnum) \ 26)
        End While

        Return colLetter
    End Function

    Private Sub getColumnsFromCPSPRXMAP()
        checkSQLConnection()

        cbx_PR2.Items.Clear()
        cbx_TD2.Items.Clear()
        cbx_PR4.Items.Clear()

        Dim sql As String = ""
        sql = "select SourceWS, Description from PCMS_FE.PCMS800.dbo.CPSPRXMAP where UI = 'Y' order by ID"


        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader

        myCmd = New SqlCommand(sql, myConn)
        myReader = myCmd.ExecuteReader()

        While myReader.Read()
            If myReader.Item("SourceWS") = "PR2" Then
                cbx_PR2.Items.Add(myReader.Item("Description"))
            ElseIf myReader.Item("SourceWS") = "TD2" Then
                cbx_TD2.Items.Add(myReader.Item("Description"))
            ElseIf myReader.Item("SourceWS") = "PR4" Then
                cbx_PR4.Items.Add(myReader.Item("Description"))
            End If
        End While

        myReader.Close()
        myCmd.Dispose()
    End Sub

    Function CopyCPSMapping(ByVal cpsmap As CPSMapping) As CPSMapping
        Return cpsmap.ShallowCopy()
    End Function

    Function CopyStringList(ByVal list) As List(Of String)
        Dim ListB As List(Of String) = New List(Of String)
        For Each elm In list
            ListB.Add(elm)
        Next
        Return ListB
    End Function

    Private Sub setWSName()
        checkSQLConnection()

        Dim sql As String = "select distinct DestWS from PCMS_FE.PCMS800.dbo.CPSPRXMAP where UI = 'Y' AND DestWS <> 'All' order by DestWS "

        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader
        Dim cnt As Integer = 0

        myCmd = New SqlCommand(sql, myConn)
        myReader = myCmd.ExecuteReader()

        While myReader.Read()
            If cnt = 0 Then
                ws1Name = myReader.Item(0)
            ElseIf cnt = 1 Then
                ws2Name = myReader.Item(0)
            Else
                Exit While
            End If
            cnt += 1
        End While

        myReader.Close()
        myCmd.Dispose()
    End Sub

    Private Sub customBGColor(ByVal ws As Excel.Worksheet, ByVal curRow As Integer, ByVal curCol As Integer, ByVal type As String)
        Dim curColLetter As String = ColumnIndexToColumnLetter(curCol - 1)

        Select Case (Trim(type))
            Case "Completion Release 1"
                ws.Range("A" & curRow & ":" & curColLetter & curRow).Interior.ColorIndex = My.Resources.ColorIndex_C1
            Case "Completion Release 2"
                ws.Range("A" & curRow & ":" & curColLetter & curRow).Interior.ColorIndex = My.Resources.ColorIndex_C2
            Case "DLP"
                ws.Range("A" & curRow & ":" & curColLetter & curRow).Interior.ColorIndex = My.Resources.ColorIndex_DLP
            Case ""
                '      ws.Range("A" & curRow & ":" & curColLetter & curRow).Interior.ColorIndex = My.Resources.ColorIndex_DEF
        End Select
    End Sub

    Private Sub setD2_Remarks(ByVal ws As Excel.Worksheet)
        ws.Cells(1, 1) = "Remarks:"
        ws.Cells(2, 2) = "No Retention Details"
        ws.Cells(3, 2) = "Completion Release 1"
        ws.Cells(4, 2) = "Completion Release 2"
        ws.Cells(5, 2) = "DLP"

        ws.Range("A1").Font.Bold = True

        ws.Range("A2").Interior.ColorIndex = My.Resources.ColorIndex_DEF
        ws.Range("A3").Interior.ColorIndex = My.Resources.ColorIndex_C1
        ws.Range("A4").Interior.ColorIndex = My.Resources.ColorIndex_C2
        ws.Range("A5").Interior.ColorIndex = My.Resources.ColorIndex_DLP

        ws.Columns.Font.Name = "Arial"
        ws.Columns.Font.Size = 10
        ws.Columns.AutoFit()
    End Sub
#End Region

    Private Sub fmXLS_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If (bgWorker.IsBusy) Then
            bgWorker.CancelAsync()

            Try
                System.Runtime.InteropServices.Marshal.ReleaseComObject(oExcel)
                oExcel = Nothing
            Catch ex As Exception
                oExcel = Nothing
            Finally
                GC.Collect()
            End Try

        End If
    End Sub
End Class