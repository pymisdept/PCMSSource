Imports System.Data.SqlClient
Imports System.Threading
Imports System.ComponentModel
Imports System.Environment
Imports System.Net

Public Class fmExport

    Public mySetting As Setting

    Private myConn As SqlConnection
    Private isDone As Boolean
    Private servType As String

    Private vFromPeriod As String
    Private vToPeriod As String
    Private vOutPath As String

    Private xPDF As Boolean
    Private xExcel As Boolean

#Region "Constructor"
    Public Sub New(ByVal ms As Setting, ByVal itm As Object, ByVal fromPeriod As String, ByVal toPeriod As String, ByVal sT As String, ByVal outPath As String)
        InitializeComponent()

        mySetting = ms
        lbx_Export.Items.Clear()
        lbx_Export.Items.AddRange(itm)

        vFromPeriod = fromPeriod
        vToPeriod = toPeriod
        vOutPath = outPath

        xPDF = cb_PDF.Checked
        xExcel = cb_Excel.Checked

        txt_fromPeriod.Text = fromPeriod
        txt_toPeriod.Text = toPeriod
        txt_outputPath.Text = outPath

        servType = sT

        EnableExportButton()

        myConn = New SqlConnection()
    End Sub
#End Region

#Region "Button Event"
    Private Sub btn_Export_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Export.Click
        loadSQLConnection()

        xPDF = cb_PDF.Checked
        xExcel = cb_Excel.Checked

        ExportHistory()

        btn_Export.Enabled = False
        btn_back.Enabled = False
        btn_browse.Enabled = False
        lbl_processing.Visible = True
        txt_fromPeriod.Enabled = False
        txt_toPeriod.Enabled = False

        bgWorker.RunWorkerAsync()
    End Sub

    Private Sub btn_back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_back.Click
        Me.Close()
    End Sub

    Private Sub btn_browse_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_browse.Click

        folderDlg.RootFolder = Environment.SpecialFolder.Desktop
        folderDlg.SelectedPath = txt_outputPath.Text
        Dim dr As DialogResult = folderDlg.ShowDialog()

        If dr = Windows.Forms.DialogResult.OK Then
            txt_outputPath.Text = folderDlg.SelectedPath
            vOutPath = folderDlg.SelectedPath
        End If

        EnableExportButton()
    End Sub

#End Region

#Region "Textbox Event"

    Private Sub txt_fromPeriod_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_fromPeriod.KeyUp
        EnableExportButton()
    End Sub

    Private Sub txt_toPeriod_KeyUp(ByVal sender As System.Object, ByVal e As System.Windows.Forms.KeyEventArgs) Handles txt_toPeriod.KeyUp
        EnableExportButton()
    End Sub

#End Region

#Region "Checkbox Event"

    Private Sub cb_PDF_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cb_PDF.CheckedChanged
        EnableExportButton()
    End Sub

    Private Sub cb_Excel_CheckedChanged(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles cb_Excel.CheckedChanged
        EnableExportButton()
    End Sub

#End Region

#Region "Background Work"

    Private Sub bgWorker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bgWorker.DoWork
        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
        doExport(worker)
    End Sub

    Private Sub bgWorker_ProgressChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bgWorker.ProgressChanged
        Me.progressbar.Value = e.ProgressPercentage
    End Sub

    Private Sub bgWorker_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgWorker.RunWorkerCompleted
        If isDone = True Then
            MsgBox("Export process completed", MsgBoxStyle.OkOnly, "PCMS")
            Process.Start("explorer.exe", vOutPath)
            ''End     'End Program
        End If
        btn_Export.Enabled = True
        btn_back.Enabled = True
        btn_browse.Enabled = True

        lbl_processing.Visible = False

        txt_fromPeriod.Enabled = True
        txt_toPeriod.Enabled = True

        Me.Close()

    End Sub

#End Region

    Private Sub loadSQLConnection()

        ' Username & Password
        Dim userName As String = mySetting.FEUsername
        Dim userPwd As String = mySetting.FEPassword
        ' ''''''''''
        Dim source As String = mySetting.FEServer
        Dim dbname As String = mySetting.FEDatabase

        If myConn.State <> ConnectionState.Open Then
            Dim connectionStr = "Initial Catalog=" & dbname & ";" & _
                    "Data Source=" & source & ";User ID=" & userName & ";Password=" & userPwd

            myConn = New SqlConnection(connectionStr)

            Try
                myConn.Open()
            Catch ex As Exception
                MsgBox("Fail to open connection.", MsgBoxStyle.OkOnly, "PCMS")
            End Try
        End If
    End Sub

    Private Sub doExport(ByVal worker As BackgroundWorker)

        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader
        Dim ProcessProperties As New ProcessStartInfo
        Dim sql, sql2 As String
        Dim fromDate, toDate As Date
        Dim rowCount As Integer
        Dim pCmd As String
        Dim outFile As String
        Dim outXLSFile As String
        Dim dtime As Date
        Dim progress As Integer

        Dim prjText As String
        Dim isErr As Boolean = False
        Dim ErrMsg As String = ""
        prjText = ""

        Dim paramList As New List(Of String)
        Dim cr As New CrExport()

        Dim key As String = My.Resources.AuthKey

        If Trim(vFromPeriod) <> "" Then
            Dim arr As String()
            arr = vFromPeriod.Split("/")
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
        End If

        If Trim(vToPeriod) <> "" Then
            Dim arr As String()
            arr = vToPeriod.Split("/")
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
        End If

        If isErr = True Then
            MsgBox(ErrMsg, MsgBoxStyle.OkOnly, "PCMS")
            worker.CancelAsync()
            Exit Sub
        End If

        If lbx_Export.Items.Count > 0 Then
            For Each itm As String In lbx_Export.Items
                prjText = prjText & "'" & itm & "',"
            Next
            prjText = prjText.Remove(prjText.Length - 1, 1)
        End If

        sql = "select ID,ProjectCode,dt01 from DocumentProperty where Type = '1040' AND DocStatus IN ('D', 'P') AND dt01 between '" & fromDate.ToString("yyyy-MM-dd") & "' AND '" & toDate.ToString("yyyy-MM-dd") & "' AND ProjectCode IN (" & prjText & ")"

        sql2 = "select count(*) from DocumentProperty where Type = '1040' AND DocStatus IN ('D', 'P') AND dt01 between '" & fromDate.ToString("yyyy-MM-dd") & "' AND '" & toDate.ToString("yyyy-MM-dd") & "' AND ProjectCode IN (" & prjText & ")"
        myCmd = New SqlCommand(sql2, myConn)
        myReader = myCmd.ExecuteReader()
        If myReader.Read() Then
            rowCount = myReader(0)
            myReader.Close()
        End If

        If rowCount = 0 Then
            MsgBox("No project report created within this period.", MsgBoxStyle.OkOnly, "PCMS")
            'End     'End Program
            Exit Sub
        Else
            If xPDF And xExcel Then
                rowCount = rowCount * 2
            End If
        End If

        'Dim ts As TimeSpan
        'ts = New TimeSpan(0, 0, rowCount * 15)

        'Dim estimatedTime As String
        'estimatedTime = ts.Minutes.ToString() & " mins "
        'If ts.Seconds > 0 Then
        '    estimatedTime = estimatedTime & ts.Seconds.ToString() & " seconds "
        'End If        
        'MsgBox(rowCount & " project reports will be generated, it may take around " & estimatedTime & Environment.NewLine & "Please wait... ", "PCMS")
        MsgBox(rowCount & " project report files will be generated / downloaded, please wait... ", MsgBoxStyle.OkOnly, "PCMS")

        Try
            myCmd = New SqlCommand(sql, myConn)
            myReader = myCmd.ExecuteReader()
            If (Not System.IO.Directory.Exists(vOutPath)) Then
                System.IO.Directory.CreateDirectory(vOutPath)
            End If

            While myReader.Read()
                dtime = myReader(2)
                If vOutPath.Chars(vOutPath.Length - 1) <> "\" Then
                    outFile = vOutPath & "\" & myReader(1) & "_" & dtime.ToString("yyyyMM") & "." & mySetting.ExportType
                    outXLSFile = vOutPath & "\" & myReader(1) & "_" & dtime.ToString("yyyyMM") & ".xls"
                Else
                    outFile = vOutPath & myReader(1) & "_" & dtime.ToString("yyyyMM") & "." & mySetting.ExportType
                    outXLSFile = vOutPath & myReader(1) & "_" & dtime.ToString("yyyyMM") & ".xls"
                End If

                If xPDF Then
                    pCmd = " -F " & My.Resources.RPTFile & " -E " & mySetting.ExportType & " -O """ & outFile & """ -S " & mySetting.BEServer & " -D " & mySetting.BEDatabase & " -U " & mySetting.BEUsername & " -P " & mySetting.BEPassword & " -a ""DocEntry:" & myReader(0) & """ -a ""UserID:PCMS"""

                    If mySetting.LogExportResult = True Then
                        pCmd = pCmd & " -l "
                    End If

                    paramList.Clear()

                    ' RPT File
                    paramList.Add("-F")
                    paramList.Add(System.AppDomain.CurrentDomain.BaseDirectory & My.Resources.RPTFile)

                    ' Ouput File
                    paramList.Add("-O")
                    paramList.Add(outFile)

                    ' Output Format (extension)
                    paramList.Add("-E")
                    paramList.Add(mySetting.ExportType)

                    ' Server
                    paramList.Add("-S")
                    paramList.Add(mySetting.BEServer)

                    ' Database
                    paramList.Add("-D")
                    paramList.Add(mySetting.BEDatabase)

                    ' Username
                    paramList.Add("-U")
                    paramList.Add(mySetting.BEUsername)

                    ' Password
                    paramList.Add("-P")
                    paramList.Add(mySetting.BEPassword)

                    ' Doc ID
                    paramList.Add("-a")
                    paramList.Add("DocEntry:" & myReader(0))

                    ' Print User
                    paramList.Add("-a")
                    paramList.Add("UserID:PCMS")

                    ' Log
                    If mySetting.LogExportResult = True Then
                        paramList.Add("-l")
                    End If

                    'ProcessProperties.FileName = fmMain.ExporterFile
                    'ProcessProperties.Arguments = pCmd
                    'ProcessProperties.WindowStyle = ProcessWindowStyle.Hidden
                    'Process.Start(ProcessProperties)

                    'Thread.Sleep(SleepSec * 1000)   ' Delay (millisecond)

                    Dim exportResult As Boolean = cr.doExport(paramList.ToArray())

                    If exportResult = True Then
                        progress += 1
                        worker.ReportProgress(CInt((progress / rowCount) * 100))
                    End If
                End If

                If xExcel Then
                    Dim remoteUri As String = "http://" & mySetting.FEServer & "/pcms_dl/Download.aspx?a=" & key & "&i=" & myReader(0)
                    Dim client = New WebClient()

                    client.DownloadFile(remoteUri, outXLSFile)
                    progress += 1
                    worker.ReportProgress(CInt((progress / rowCount) * 100))

                End If
            End While

            isDone = True
            myReader.Close()
            myCmd.Dispose()
        Catch ex As Exception
            MsgBox("Unable to export Project Report", MsgBoxStyle.OkOnly, "PCMS")
        End Try

    End Sub

    Private Sub ExportHistory()
        Dim sw As System.IO.StreamWriter
        Dim dir As String = GetFolderPath(Environment.SpecialFolder.LocalApplicationData) & "\\" & My.Resources.ApplicationName

        sw = My.Computer.FileSystem.OpenTextFileWriter(dir + "\\" + My.Resources.HistoryFile, False)

        sw.WriteLine("[VERSION]")
        sw.WriteLine(My.Resources.HistoryVersion)
        sw.WriteLine(String.Empty)

        sw.WriteLine("[EXPORT PATH]")
        sw.WriteLine(vOutPath)
        sw.WriteLine(String.Empty)

        sw.WriteLine("[SERVER]")
        sw.WriteLine(servType)
        sw.WriteLine(String.Empty)

        sw.WriteLine("[PROJECTS]")
        If lbx_Export.Items.Count > 0 Then
            For Each itm As String In lbx_Export.Items
                sw.WriteLine(Trim(itm))
            Next
        End If

        fmMain.OutputFolder = vOutPath

        sw.Close()
    End Sub

    Private Sub EnableExportButton()
        Dim isTick As Boolean
        isTick = cb_PDF.Checked Or cb_Excel.Checked

        If Trim(vFromPeriod) <> "" And Trim(vToPeriod) <> "" And Trim(vOutPath) <> "" And isTick Then
            btn_Export.Enabled = True
        Else
            btn_Export.Enabled = False
        End If
    End Sub

    Private Sub fmExport_FormClosing(ByVal sender As System.Object, ByVal e As System.Windows.Forms.FormClosingEventArgs) Handles MyBase.FormClosing
        If myConn.State <> ConnectionState.Open Then
            Try
                myConn.Close()
            Catch ex As Exception
                MsgBox("Fail to close connection.", MsgBoxStyle.OkOnly, "PCMS")
            End Try
        End If
    End Sub


End Class