Imports System.Data.SqlClient
Imports System.Threading
Imports System.ComponentModel
Imports Microsoft.Office.Interop
Imports System.Runtime.InteropServices

Public Class fmExport

    Private myConn As SqlConnection
    Public myConfig As Config
    Private isStart As Boolean
    Private isProcessed As Boolean

#Region "Constructor"
    Public Sub New(ByVal mc As Config, ByVal itm As Object, ByVal fromPeriod As String, ByVal toPeriod As String, ByVal prevPR2 As List(Of String), ByVal prevTD2 As List(Of String), ByVal prevPR4 As List(Of String))
        InitializeComponent()

        isProcessed = False
        isStart = False
        myConfig = mc
        '  lbx_Export.Items.Clear()
        '  lbx_Export.Items.AddRange(itm)

        If prevPR2.Count > 0 Then
            For Each item In prevPR2
                If cbx_PR2.Items.Contains(item) Then
                    cbx_PR2.SetItemCheckState(cbx_PR2.Items.IndexOf(item), CheckState.Checked)
                End If
            Next
            checkCBXState("PR2")
        End If

        If prevTD2.Count > 0 Then
            For Each item In prevTD2
                If cbx_TD2.Items.Contains(item) Then
                    cbx_TD2.SetItemCheckState(cbx_TD2.Items.IndexOf(item), CheckState.Checked)
                End If
            Next
            checkCBXState("TD2")
        End If


        If prevPR4.Count > 0 Then
            For Each item In prevPR4
                If cbx_PR4.Items.Contains(item) Then
                    cbx_PR4.SetItemCheckState(cbx_PR4.Items.IndexOf(item), CheckState.Checked)
                End If
            Next
            checkCBXState("PR4")
        End If

        txt_fromPeriod.Text = fromPeriod
        txt_toPeriod.Text = toPeriod

        EnableExportButton()

        myConn = New SqlConnection()
    End Sub
#End Region

#Region "Button Event"
    Private Sub btn_Export_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btn_Export.Click
        checkSQLConnection()

        btn_Export.Enabled = False
     
        lbl_processing.Visible = True

        txt_fromPeriod.Enabled = False
        txt_toPeriod.Enabled = False

        cbx_PR2.SelectionMode = SelectionMode.None
        cbx_PR4.SelectionMode = SelectionMode.None
        cbx_TD2.SelectionMode = SelectionMode.None

        cb_PR2.Enabled = False
        cb_PR4.Enabled = False
        cb_TD2.Enabled = False

        isStart = True

        ExportHistory()

        bgWorker.RunWorkerAsync()
    End Sub

    Private Sub btn_back_Click(ByVal sender As System.Object, ByVal e As System.EventArgs)
        Me.Close()
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

#Region "Background Work"

    Private Sub bgWorker_DoWork(ByVal sender As System.Object, ByVal e As System.ComponentModel.DoWorkEventArgs) Handles bgWorker.DoWork
        Dim worker As BackgroundWorker = CType(sender, BackgroundWorker)
        doExport(worker)
    End Sub

    Private Sub bgWorker_ProgressChanged(ByVal sender As System.Object, ByVal e As System.ComponentModel.ProgressChangedEventArgs) Handles bgWorker.ProgressChanged
        Me.progressbar.Value = e.ProgressPercentage
    End Sub

    Private Sub bgWorker_RunWorkerCompleted(ByVal sender As System.Object, ByVal e As System.ComponentModel.RunWorkerCompletedEventArgs) Handles bgWorker.RunWorkerCompleted
        btn_Export.Enabled = True

        lbl_processing.Visible = False

        txt_fromPeriod.Enabled = True
        txt_toPeriod.Enabled = True

        cbx_PR2.SelectionMode = SelectionMode.One
        cbx_PR4.SelectionMode = SelectionMode.One
        cbx_TD2.SelectionMode = SelectionMode.One

        cb_PR2.Enabled = True
        cb_PR4.Enabled = True
        cb_TD2.Enabled = True

        isStart = False


        If (isProcessed = True) Then
            Me.Close()
        End If

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

    Private Sub doExport(ByVal worker As BackgroundWorker)
        Dim progress, totalCount As Integer

        Dim oExcel As Excel.Application
        Dim oBook As Excel.Workbook
        Dim oSheet_D1 As Excel.Worksheet
        Dim oSheet_D2 As Excel.Worksheet

        Dim myCmd As SqlCommand
        Dim myReader As SqlDataReader
        Dim sql, sql2 As String
        Dim fromDate As Date
        Dim toDate As Date
        Dim d1_row, d1_col, d2_row, d2_col As Integer

        Dim prjText, mapDesc As String
        prjText = ""
        mapDesc = ""

        Dim isErr As Boolean = False
        Dim ErrMsg As String = ""

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

        'If lbx_Export.Items.Count > 0 Then
        '    For Each itm As String In lbx_Export.Items
        '        prjText = prjText & "'" & itm & "',"
        '    Next
        '    prjText = prjText.Remove(prjText.Length - 1, 1)
        'End If

        isProcessed = True

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

        If isErr = True Then
            MsgBox(ErrMsg)
            Exit Sub
        End If

        ''' Start Excel Application
        oExcel = CreateObject("Excel.Application")
        oBook = oExcel.Workbooks.Add(Type.Missing)

        oSheet_D2 = oBook.Worksheets.Add()
        oSheet_D1 = oBook.Worksheets.Add()
        oSheet_D1.Name = "Database_1"
        oSheet_D2.Name = "Database_2"

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

        oSheet_D1.Range("A1:B1").Font.ColorIndex = 3
        oSheet_D1.Cells(1, 1) = "Project *"
        oSheet_D1.Cells(1, 2) = "Project Financial Period *"

        oSheet_D2.Range("A1:B1").Font.ColorIndex = 3
        oSheet_D2.Cells(1, 1) = "Project *"
        oSheet_D2.Cells(1, 2) = "Project Financial Period *"

        d1_row = 1
        d1_col = 3
        d2_row = 1
        d2_col = 3


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

        '''' Get data from CPSPRXMAP (FE)
        sql = "select ID, Description,  Tbl, TblType, TblKey,DataType, Fld, DestWS  from PCMS_FE.PCMS800.dbo.CPSPRXMAP where Description IN (" & mapDesc & ") order by Seq"
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
            If myReader("Tbl") = "NA" Then
                If myReader("TblType") = "Header" Then
                    hdr = hdr & myReader("ID") & ";"
                Else
                    bdy = bdy & myReader("ID") & ";"
                End If
            Else
                If myReader("DestWS") = "Database_1" Then
                    tmpDict = dict_DB1
                    oSheet_D1.Cells(d1_row, d1_col) = myReader("Description")
                    d1_col += 1
                Else
                    tmpDict = dict_DB2
                    oSheet_D2.Cells(d2_row, d2_col) = myReader("Description")
                    d2_col += 1
                End If

                If tmpDict.ContainsKey(myReader("Tbl")) Then
                    tempStr = tmpDict(myReader("Tbl")).Fields
                    tempCnt = tmpDict(myReader("Tbl")).FieldCount
                    tempArr = tmpDict(myReader("Tbl")).FieldType

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
        Dim hdrArr = hdr.Split(";")
        Dim bdyArr = bdy.Split(";")
        conditionKey = ""

        If hdrArr.Length > 1 And bdyArr.Length > 1 Then

            For i = 1 To hdrArr.Length
                For j = 1 To bdyArr.Length
                    If hdrArr(i - 1) <> "" And bdyArr(j - 1) <> "" Then
                        conditionKey = conditionKey & "'" & hdrArr(i - 1) & "," & bdyArr(j - 1) & "',"
                    End If
                Next
            Next
            conditionKey = conditionKey.Remove(conditionKey.Length - 1, 1)
        End If

        If conditionKey <> "" Then
            sql = "select Description, ID, Tbl, TblType, TblKey, DataType, Fld, DestWS from PCMS_FE.PCMS800.dbo.CPSPRXMAP where Condition IN (" & conditionKey & ") order by Seq"
            WriteLog(sql)
            myCmd = New SqlCommand(sql, myConn)
            myReader = myCmd.ExecuteReader()
            While myReader.Read()

                If myReader("DestWS") = "Database_1" Then
                    tmpDict = dict_DB1
                    oSheet_D1.Cells(d1_row, d1_col) = myReader("Description")
                    d1_col += 1
                Else
                    tmpDict = dict_DB2
                    oSheet_D2.Cells(d2_row, d2_col) = myReader("Description")
                    d2_col += 1
                End If

                If myReader("Tbl") <> "NA" Then
                    If tmpDict.ContainsKey(myReader("Tbl")) Then
                        tempStr = tmpDict(myReader("Tbl")).Fields
                        tempCnt = tmpDict(myReader("Tbl")).FieldCount
                        tempArr = tmpDict(myReader("Tbl")).FieldType

                        tmpDict(myReader("Tbl")).Fields = tempStr & ", " & myReader("Fld")
                        tmpDict(myReader("Tbl")).FieldCount = tempCnt + 1

                        tempArr.Add(myReader("DataType"))
                        tmpDict(myReader("Tbl")).FieldType = tempArr
                    Else
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
        End If

        oSheet_D1.Range("A1:" & ColumnIndexToColumnLetter(d1_col - 1) & "1").Interior.ColorIndex = 40
        'oSheet_D1.Range("A1:" & ColumnIndexToColumnLetter(d1_col - 1) & "1").Font.Bold = True
        oSheet_D1.Range("A1:" & ColumnIndexToColumnLetter(d1_col - 1) & "1").HorizontalAlignment = Excel.Constants.xlCenter

        oSheet_D2.Range("A1:" & ColumnIndexToColumnLetter(d2_col - 1) & "1").Interior.ColorIndex = 40
        'oSheet_D2.Range("A1:" & ColumnIndexToColumnLetter(d2_col - 1) & "1").Font.Bold = True
        oSheet_D2.Range("A1:" & ColumnIndexToColumnLetter(d2_col - 1) & "1").HorizontalAlignment = Excel.Constants.xlCenter

        ''' Get data from DocumentProperty (FE)
        sql = "select d.ID, d.DocEntry, d.ProjectCode, d.dt01, p.U_ProjectFullName from PCMS_FE.PCMS800.dbo.DocumentProperty d, OPRJ p where d.ProjectCode = p.PrjCode AND ProjectCode IN (" & prjText & ") AND DocStatus IN ('D', 'P') AND Type = '1040' AND dt01 between '" & fromDate.ToString("yyyy-MM-dd") & "' AND '" & toDate.ToString("yyyy-MM-dd") & "' order by ProjectCode, ID"
        WriteLog(sql)
        myCmd = New SqlCommand(sql, myConn)
        myReader = myCmd.ExecuteReader()

        Dim selectRow As Integer
        
        Dim arrList As List(Of DocumentProperty)
        arrList = New List(Of DocumentProperty)

        Dim doc As DocumentProperty

        While myReader.Read()
            doc = New DocumentProperty()
            doc.ID = myReader("ID")
            doc.DocEntry = myReader("DocEntry")
            doc.ProjectCode = myReader("ProjectCode")
         
            arrList.Add(doc)
        End While

        myReader.Close()
        myCmd.Dispose()

        d1_col = 3
        d2_col = 3
        d1_row = 2
        d2_row = 2

        For Each arr In arrList
            oSheet_D1.Cells(d1_row, 1) = arr.getValueByName("ProjectCode") & " - " & arr.getValueByName("ProjectDesc")
            oSheet_D1.Cells(d1_row, 2) = arr.getValueByName("dt01")


            For Each kvp As KeyValuePair(Of String, CPSMapping) In dict_DB1
                Dim v1 As String = kvp.Key
                Dim v2 As CPSMapping = kvp.Value

                selectRow = 0
                sql2 = ""
                ''' Get details from database (BE)
                If (v2.TableType = "Table") Then
                    sql2 = "select " & v2.Fields & " from " & v2.TableName & " where " & v2.TableKey & " = " & arr.getValueByName(v2.TableKey)
                ElseIf (v2.TableType = "Function") Then
                    sql2 = "select " & v2.Fields & " from " & v2.TableName & "(" & arr.getValueByName(v2.TableKey) & ")"
                End If

                If sql2 <> "" Then
                    WriteLog(sql2)
                    myCmd = New SqlCommand(sql2, myConn)
                    myReader = myCmd.ExecuteReader()

                    selectRow = 0

                    If myReader.Read() Then
                        While selectRow < v2.FieldCount
                            oSheet_D1.Cells(d1_row, d1_col) = myReader(selectRow)

                            If v2.FieldType(selectRow).ToString() = "Number" Then
                                oSheet_D1.Cells(d1_row, d1_col).NumberFormat = My.Resources.XLS_NumFormat
                            ElseIf v2.FieldType(selectRow).ToString() = "Period" Then
                                oSheet_D1.Cells(d1_row, d1_col).NumberFormat = My.Resources.XLS_PeriodFormat
                            ElseIf v2.FieldType(selectRow).ToString() = "Date" Then
                                oSheet_D1.Cells(d1_row, d1_col).NumberFormat = My.Resources.XLS_DateFormat
                            ElseIf v2.FieldType(selectRow).ToString() = "Decimal" Then
                                oSheet_D1.Cells(d1_row, d1_col).NumberFormat = My.Resources.XLS_DecimalFormat
                            Else
                                oSheet_D1.Cells(d1_row, d1_col).NumberFormat = My.Resources.XLS_StringFormat
                            End If

                            d1_col += 1
                            selectRow += 1
                        End While
                    End If

                    myReader.Close()
                    myCmd.Dispose()
                End If
            Next

            For Each kvp As KeyValuePair(Of String, CPSMapping) In dict_DB2
                Dim v1 As String = kvp.Key
                Dim v2 As CPSMapping = kvp.Value

                selectRow = 0
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

                    selectRow = 0

                    While myReader.Read()

                        oSheet_D2.Cells(d2_row, 1) = arr.getValueByName("ProjectCode") & " - " & arr.getValueByName("ProjectDesc")
                        oSheet_D2.Cells(d2_row, 2) = arr.getValueByName("dt01")
                        selectRow = 0

                        While selectRow < v2.FieldCount
                            oSheet_D2.Cells(d2_row, d2_col) = myReader(selectRow)
                            If v2.FieldType(selectRow).ToString() = "Number" Then
                                oSheet_D2.Cells(d2_row, d2_col).NumberFormat = My.Resources.XLS_NumFormat
                            ElseIf v2.FieldType(selectRow).ToString() = "Period" Then
                                oSheet_D2.Cells(d2_row, d2_col).NumberFormat = My.Resources.XLS_PeriodFormat
                            ElseIf v2.FieldType(selectRow).ToString() = "Date" Then
                                oSheet_D2.Cells(d2_row, d2_col).NumberFormat = My.Resources.XLS_DateFormat
                            ElseIf v2.FieldType(selectRow).ToString() = "Decimal" Then
                                oSheet_D2.Cells(d2_row, d2_col).NumberFormat = My.Resources.XLS_DecimalFormat
                            Else
                                oSheet_D2.Cells(d2_row, d2_col).NumberFormat = My.Resources.XLS_StringFormat
                            End If

                            d2_col += 1
                            selectRow += 1
                        End While
                        d2_row += 1
                        d2_col = 3
                    End While
                    myReader.Close()
                    myCmd.Dispose()
                End If
            Next

            d1_row += 1
            d1_col = 3

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

        oSheet_D1 = Nothing
        oSheet_D2 = Nothing
        oBook = Nothing
        oExcel = Nothing

        dict_DB1 = Nothing
        dict_DB2 = Nothing
        tmpDict = Nothing
        tempArr = Nothing
        arrList = Nothing

    End Sub

    Private Sub ExportHistory()
        Dim sw As System.IO.StreamWriter
        sw = My.Computer.FileSystem.OpenTextFileWriter(My.Resources.HistoryFile, False)

        sw.WriteLine("VER----------------------")
        sw.WriteLine(My.Resources.HistoryVersion)
        sw.WriteLine("PRJ----------------------")
        'If lbx_Export.Items.Count > 0 Then
        '    For Each itm As String In lbx_Export.Items
        '        sw.WriteLine(Trim(itm))
        '    Next
        'End If
        sw.WriteLine("PR2----------------------")
        fmMain.prevPR2.Clear()
        If cbx_PR2.CheckedItems.Count > 0 Then
            For Each itm As String In cbx_PR2.CheckedItems
                sw.WriteLine(Trim(itm))
                fmMain.prevPR2.Add(Trim(itm))
            Next
        End If
        sw.WriteLine("TD2----------------------")
        fmMain.prevTD2.Clear()
        If cbx_TD2.CheckedItems.Count > 0 Then
            For Each itm As String In cbx_TD2.CheckedItems
                sw.WriteLine(Trim(itm))
                fmMain.prevTD2.Add(Trim(itm))
            Next
        End If
        sw.WriteLine("PR4----------------------")
        fmMain.prevPR4.Clear()
        If cbx_PR4.CheckedItems.Count > 0 Then
            For Each itm As String In cbx_PR4.CheckedItems
                sw.WriteLine(Trim(itm))
                fmMain.prevPR4.Add(Trim(itm))
            Next
        End If

        'fmMain.OutputFolder = txt_outputPath.Text

        sw.Close()
    End Sub

    Private Sub EnableExportButton()

        If isStart = False Then
            Dim isCBChecked As Boolean
            isCBChecked = cbx_PR2.CheckedItems.Count > 0 Or cbx_PR4.CheckedItems.Count > 0 Or cbx_TD2.CheckedItems.Count > 0

            If Trim(txt_fromPeriod.Text) <> "" And Trim(txt_toPeriod.Text) <> "" And isCBChecked = True Then
                btn_Export.Enabled = True
            Else
                btn_Export.Enabled = False
            End If
        Else
            btn_Export.Enabled = False
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

    Private Sub checkCBXState(ByVal val As String)
        Dim cbl, cb
        If val = "PR2" Then
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

    Private Sub WriteLog(ByVal text As String)
        If myConfig.LogSQL = True Then
            Dim file As System.IO.StreamWriter
            file = My.Computer.FileSystem.OpenTextFileWriter(My.Resources.SQLFile, True)
            file.WriteLine(DateTime.Now & "  -- " & text)
            file.Close()
        End If
    End Sub

    Private Sub fmExport_Load(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Load
        Me.Text = Me.Text & " (Version: " & My.Application.Info.Version.Major & "." & My.Application.Info.Version.Minor & "." & My.Application.Info.Version.Build & ")"
    End Sub
End Class