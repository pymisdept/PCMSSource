Imports System.Data.SqlClient
Imports System.IO
Imports System.Runtime.InteropServices

Public Class fmMain

    Private myConn As SqlConnection
    Public myConfig As Config
    Public prevPR2 As List(Of String)
    Public prevTD2 As List(Of String)
    Public prevPR4 As List(Of String)

#Region "Constructor"
    Sub New()
        InitializeComponent()
        myConn = New SqlConnection()
        myConfig = New Config(My.Resources.SettingFile)

        If myConfig.loadSetting() = False Then
            MsgBox("Unable to load the configuration file, please check the config file")
            End 'Force Close
        End If

        prevPR2 = New List(Of String)
        prevTD2 = New List(Of String)
        prevPR4 = New List(Of String)


        ImportHistory()
        checkProjectSelected()

        Dim dt As Date = DateSerial(Year(Today()), Month(Today()), 0)
        txt_fromPeriod.Text = dt.Month.ToString().PadLeft(2, "0") & "/" & dt.Year.ToString().PadLeft(4, "0")
        txt_toPeriod.Text = txt_fromPeriod.Text

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

        sql = "select distinct ProjectCode from DocumentProperty where ProjectCode like '%" & Trim(txt_PrjCode.Text) & "%' AND Type = '1040' AND DocStatus IN ('D', 'P') AND dt01 between '" & fromDate.ToString("yyyy-MM-dd") & "' AND '" & toDate.ToString("yyyy-MM-dd") & "'"

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
            MsgBox("Please select a project")
        End If

    End Sub

    Private Sub btn_fromENList_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_fromENList.Click
        cbx_Result.Items.Clear()
        checkSQLConnection()

        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader
        Dim sql As String

        sql = "select PrjCode from CPSPRADM"

        myCmd = New SqlCommand(sql, myConn)
        myReader = myCmd.ExecuteReader()
        While myReader.Read()
            cbx_Result.Items.Add(myReader.Item(0), True)
        End While

        checkResultState()
        myReader.Close()
        myCmd.Dispose()

    End Sub

    Private Sub btn_deletePrj_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_deletePrj.Click
        If cbx_Export.CheckedItems.Count > 0 Then
            For i As Integer = cbx_Export.CheckedIndices.Count - 1 To 0 Step -1
                cbx_Export.Items.RemoveAt(cbx_Export.CheckedIndices.Item(i))
            Next
            checkExportState()
            checkProjectSelected()
        Else
            MsgBox("Please select a project")
        End If

    End Sub

    Private Sub btn_Next_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Next.Click

        Using fm As New fmExport(myConfig, cbx_Export.CheckedItems.Cast(Of String).ToArray(), txt_fromPeriod.Text, txt_toPeriod.Text, prevPR2, prevTD2, prevPR4)
            fm.ShowDialog()
        End Using

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
                MsgBox(count & " project(s) imported")
            End If
        Catch ex As Exception
            MsgBox("Failed to import text file")
        End Try
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

    Private Sub checkSQLConnection()

        Dim source As String = myConfig.FEDataSource
        Dim dbname As String = myConfig.FEDatabase

        If myConn.State <> ConnectionState.Open Then
            Dim connectionStr = "Initial Catalog=" & dbname & ";" & _
                    "Data Source=" & source & ";User ID=" & My.Resources.FE_DBUserName & ";Password=" & My.Resources.FE_DBPassword

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

            sql = "select 1 from PCMS_BE.PAY800.dbo.OPRJ where PrjCode = '" & PrjCode & "'"

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
        Try
            cbx_Export.Items.Clear()
            prevPR2.Clear()
            prevTD2.Clear()
            prevPR4.Clear()

            Using sr As New StreamReader(My.Resources.HistoryFile)
                While sr.EndOfStream = False
                    tmpStr = sr.ReadLine()
                    If tmpStr = "VER----------------------" Then
                        tmpStr = sr.ReadLine()
                        If Trim(tmpStr) <> My.Resources.HistoryVersion Then
                            Exit Sub
                        End If
                    ElseIf tmpStr = "PER----------------------" Then
                        tmpStr = sr.ReadLine()
                        txt_fromPeriod.Text = tmpStr

                        tmpStr = sr.ReadLine()
                        txt_toPeriod.Text = tmpStr
                    ElseIf tmpStr = "PRJ----------------------" Then
                        Flag = "PRJ"
                    ElseIf tmpStr = "PR2----------------------" Then
                        Flag = "PR2"
                    ElseIf tmpStr = "TD2----------------------" Then
                        Flag = "TD2"
                    ElseIf tmpStr = "PR4----------------------" Then
                        Flag = "PR4"
                    End If

                    If Flag = "PRJ" Then
                        If tmpStr <> "PRJ----------------------" And tmpStr <> "" Then
                            cbx_Export.Items.Add(Trim(tmpStr))
                        End If
                    ElseIf Flag = "PR2" Then
                        If tmpStr <> "PR2----------------------" And tmpStr <> "" Then
                            prevPR2.Add(tmpStr)
                        End If
                    ElseIf Flag = "TD2" Then
                        If tmpStr <> "TD2----------------------" And tmpStr <> "" Then
                            prevTD2.Add(tmpStr)
                        End If
                    ElseIf Flag = "PR4" Then
                        If tmpStr <> "PR4----------------------" And tmpStr <> "" Then
                            prevPR4.Add(tmpStr)
                        End If
                    End If
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

End Class
