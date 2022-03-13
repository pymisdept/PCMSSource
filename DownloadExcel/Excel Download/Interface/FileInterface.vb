Imports Microsoft.Office.Interop
Imports Microsoft.Office.Interop.Excel

Namespace Excel.Interface
    Public MustInherit Class FileInterface
        Implements IDisposable

        Public _Application As Microsoft.Office.Interop.Excel.Application
        Private _FileType As String

#Region "Import Module Need"
        Private prgPath As String
        Private _Function As String

#End Region

#Region "Export Module Need"
        Private _RequestID As String
        Private _FileName As String
        Private _fname As String
#End Region

        Public Sub New(ByVal xlsApplication As Microsoft.Office.Interop.Excel.Application)
            Dim strPath As String

            strPath = System.Reflection.Assembly.GetExecutingAssembly.Location
            strPath = Left(strPath, InStrRev(strPath, "\"))

            prgPath = strPath
            _Application = xlsApplication
            _Application.DisplayAlerts = False
        End Sub

        Public ReadOnly Property strPath() As String
            Get
                Return prgPath
            End Get
        End Property

        Public WriteOnly Property RequestID() As String
            Set(ByVal value As String)
                _RequestID = value
            End Set
        End Property

        Public Property FunctionID() As String
            Set(ByVal value As String)
                _Function = value
            End Set
            Get
                Return _Function
            End Get
        End Property

        Public WriteOnly Property FileType() As String
            Set(ByVal value As String)
                _FileType = value
            End Set
        End Property

#Region "Excel Control"
        'Karrson Change: Import(pSqlStr(), ProtectSheet as Boolean)
        'Public Sub Import(ByVal pSqlStr() As String)
        Public Sub Import(ByVal pSqlStr() As String, Optional ByVal ProtectSheet As Boolean = True)
            Dim oWorkBook As Microsoft.Office.Interop.Excel.Workbook
            Dim vbaCollection() As VBA.Collection
            Dim oFileName As String
            WriteDebug("Import: RsToCollection")
            vbaCollection = RsToCollection(pSqlStr)
            WriteDebug("Import: Run Application")
            _Application.Run("Imports", vbaCollection)
            WriteDebug("Import: set FileName")
            oFileName = CStr(Excel_MapPath.Item(_FileType))


            WriteDebug("Import: Open Workbook")
            WriteDebug("File Names:" & oFileName)
            'oWorkBook = _Application.Workbooks.Open(oFileName, False)
            'Karrson: Disable Macro
            _Application.EnableEvents = False
            'Karrson Test Visible
            '_Application.Visible = True
            oWorkBook = _Application.Workbooks.Open(oFileName, False)
            WriteDebug("Import: UnProtectSheet")
            UnProtectSheet(oWorkBook)
            WriteDebug("Import: BreakLinked")
            'Break Linking between Add-In and Excel File
            BreakLinked(oWorkBook)
            WriteDebug("Import: ReplaceAllData")
            _Application.DisplayAlerts = False
            ReplaceAllData(oWorkBook)
            _Application.DisplayAlerts = True
            WriteDebug("Import: ProtectSheet")
            ' Karrson Change: ProtectedSheet By Ini Config

            ProtectedSheet(oWorkBook, ProtectSheet)



            'Saved Format as [Request ID][FileType]yyyyMMdd hhmmss.xls
            _fname = _RequestID.Trim & "-" & _FileType.Trim & "-" & Format(Now, "yyyyMMdd hhmmss") & ".xlsm"
            oFileName = oSetting.Export_Path & "\" & "" & _fname
            _FileName = oFileName
            WriteDebug("Import: Save")
            'oWorkBook.SaveAs(oFileName)
            'oWorkBook.SaveAs(oFileName, XlFileFormat.xlOpenXMLWorkbook)
            oWorkBook.SaveAs(oFileName, XlFileFormat.xlOpenXMLWorkbookMacroEnabled)
            WriteDebug("Import: Close")
            oWorkBook.Close()
            WriteDebug("Import: Closed")
            Quit()
        End Sub

        'Karrson: Quit
        Public Sub Quit()
            _Application.Quit()
        End Sub
        Public Sub Export()
            Dim oDLNMGR As Datatable.PCMS.DLNMGR
            Dim sqlstr As String
            'Dim oTempFile() As Byte

            'oTempFile = ExcelDownload.FileReader(_FileName)
            oDLNMGR = New Datatable.PCMS.DLNMGR()
            oDLNMGR.getDownloadRequest(_RequestID)

            oDLNMGR.Update()

            'update dlnmgr table path & filename
            Try
                CheckPoint = "Update dlnmgr table path and filename field"
                sqlstr = "update dlnmgr set filedata='', path='" & oSetting.Export_Path & "', filename='" & _fname & "' where id=" & _RequestID
                oPCMSConnection.Execute(sqlstr)
            Catch ex As Exception
                oLogMessage.AddExceptionSkip(CheckPoint, ex.ToString)
            End Try

        End Sub

        Public Function RsToCollection(ByVal pSqlStr() As String) As VBA.Collection()
            Dim oRecSet As SqlClient.SqlDataReader
            Dim oColumn As VBA.Collection
            Dim oRow As VBA.Collection() = Nothing
            Dim oRecCount As Integer
            Dim oMapType As New MapType

            ReDim oRow(0)

            For Each oSqlStr As String In pSqlStr
                'Karrson Debug
                WriteDebug(oSqlStr)
                Dim oColumnID As String
                Dim oColumnValue As Object
                Dim oStringToExcel As String, _
                    oDoubleToExcel As Double, _
                    oIntegeToExcel As Integer, _
                    oDateToExcel As Date
                Dim oValueToExcel As Object

                oRecSet = oSAP_Collection.Executes(oSqlStr)
                oRecCount = 1
                'Karrson: Try 
                WriteDebug(oSetting.Addin_Path & "\" & ExcelDownload.VBAFileName)
                Try


                    While oRecSet.Read
                        'Karrson: Debug
                        If pSqlStr.Contains("Cons003") Then
                            WriteDebug("MA008 RecCount" & oRecCount)
                        End If
                        ' WriteDebug("RecCount" & oRecCount)
                        If oRow Is Nothing Then
                            If pSqlStr.Contains("Cons003") Then
                                WriteDebug("MA008 oRow is nothing")
                            End If
                            WriteDebug("oRow is nothing")
                            oColumn = _Application.Run("'" & oSetting.Addin_Path & "\" & ExcelDownload.VBAFileName & "'!getCollect")
                        Else
                            If pSqlStr.Contains("Cons003") Then
                                WriteDebug("MA008 oRow Lenght: " & oRow.Length)
                                WriteDebug("MA008 oRecCount: " & oRecCount)
                            End If
                            ' WriteDebug("oRow Lenght: " & oRow.Length)
                            ' WriteDebug("oRecCount: " & oRecCount)

                            If oRow.Length >= oRecCount + 1 Then
                                oColumn = oRow(oRecCount)
                            Else

                                oColumn = _Application.Run("'" & oSetting.Addin_Path & "\" & ExcelDownload.VBAFileName & "'!getCollect")
                            End If
                        End If

                        For i As Integer = 0 To oRecSet.FieldCount - 1
                            oColumnID = oRecSet.GetName(i)

                            If pSqlStr.Contains("Cons003") Then
                                WriteDebug("MA008 Line: " & i.ToString())
                                WriteDebug("MA008 Column: " & oColumnID)
                                WriteDebug("MA008 Value: " & oRecSet.GetValue(i).ToString())
                            End If

                            'WriteDebug(String.Format("Line: {0} Column: {1} Value: {2}", i.ToString(), oColumnID, oRecSet.GetValue(i).ToString()))
                            'WriteDebug("Line: " & i.ToString())
                            'WriteDebug("Column: " & oColumnID)
                            'WriteDebug("Value: " & oRecSet.GetValue(i).ToString())
                            If IsDBNull(oRecSet.GetValue(i)) Then
                                oColumnValue = ""
                            Else
                                oColumnValue = oRecSet.GetValue(i)
                            End If
                            'Karrson

                            Select Case TypeName(oColumnValue)
                                Case oMapType.mString(MapType.IDE_Type.IDE_VBNET) '"S" type in VB6
                                    oStringToExcel = oColumnValue '& MapType.SplitCode & oMapType.mString(MapType.IDE_Type.IDE_VB6)
                                    oValueToExcel = oStringToExcel
                                Case oMapType.mDecimal(MapType.IDE_Type.IDE_VBNET) '"N" type in VB6
                                    oDoubleToExcel = oColumnValue ' & MapType.SplitCode & oMapType.mDecimal(MapType.IDE_Type.IDE_VB6)
                                    oValueToExcel = oDoubleToExcel
                                Case oMapType.mInteger(MapType.IDE_Type.IDE_VBNET) '"I" type in VB6
                                    oIntegeToExcel = oColumnValue ' & MapType.SplitCode & oMapType.mInteger(MapType.IDE_Type.IDE_VB6)
                                    oValueToExcel = oIntegeToExcel
                                Case oMapType.mDouble(MapType.IDE_Type.IDE_VBNET) '"N" type in VB6
                                    oDoubleToExcel = oColumnValue ' & MapType.SplitCode & oMapType.mDouble(MapType.IDE_Type.IDE_VB6)
                                    oValueToExcel = oDoubleToExcel
                                Case oMapType.mDateTime(MapType.IDE_Type.IDE_VBNET) '"D" type in VB6
                                    oDateToExcel = oColumnValue ' & MapType.SplitCode & oMapType.mDateTime(MapType.IDE_Type.IDE_VB6)
                                    oValueToExcel = oDateToExcel
                                Case Else '"S" type in VB6
                                    oStringToExcel = oColumnValue ' & MapType.SplitCode & oMapType.mString(MapType.IDE_Type.IDE_VB6)
                                    oValueToExcel = oStringToExcel
                            End Select


                            Try
                                oColumn.Add(oValueToExcel, oColumnID.Trim)
                            Catch ex As Exception
                            End Try
                        Next

                        If oRow Is Nothing Then
                            ReDim oRow(0)
                            oRow(oRow.Length - 1) = oColumn
                        ElseIf oRow.Length >= oRecCount + 1 Then
                            oRow(oRecCount) = oColumn
                        Else
                            ReDim Preserve oRow(oRow.Length)
                            oRow(oRow.Length - 1) = oColumn
                        End If

                        oRecCount += 1
                    End While

                    oRecSet.Close()
                Catch ex As Exception
                    'Karrson: Close RecSet
                    Try
                        oRecSet.Close()
                    Catch ex1 As Exception

                    End Try
                    If pSqlStr.Contains("Cons003") Then
                        WriteDebug("MA008 Exception: " & ex.Message)
                    End If
                    WriteDebug("Exception: " & ex.Message)
                    'Karrson: Exception
                    Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "FileInterface", ex.ToString)
                End Try

            Next

            If oRow.Length = 1 Then
                ReDim Preserve oRow(oRow.Length)
                oRow(oRow.Length - 1) = CType(_Application.Run("'" & oSetting.Addin_Path & "\" & ExcelDownload.VBAFileName & "'!getCollect"), VBA.Collection)
            End If

            Return oRow
        End Function

        Public Sub FormulaSearch(ByRef pWorkSheet As Microsoft.Office.Interop.Excel.Worksheet)
            Dim currentFind As Microsoft.Office.Interop.Excel.Range = Nothing
            Dim firstFind As Microsoft.Office.Interop.Excel.Range = Nothing
            Dim oFormula As Object

            pWorkSheet.Protect(UserInterfaceOnly:=True)

            currentFind = pWorkSheet.Columns.Find("=RecSet", pWorkSheet.Range("A1"), _
                                      Microsoft.Office.Interop.Excel.XlFindLookIn.xlFormulas, Microsoft.Office.Interop.Excel.XlLookAt.xlPart)

            While Not currentFind Is Nothing

                ' Keep track of the first range you find.
                If firstFind Is Nothing Then
                    firstFind = currentFind

                    ' If you didn't move to a new range, you are done.
                ElseIf currentFind.Address = firstFind.Address Then
                    Exit While
                End If

                oFormula = currentFind.Formula
                currentFind.Formula = Nothing
                currentFind.Formula = oFormula

                currentFind = pWorkSheet.Columns.Find("=RecSet", pWorkSheet.Range(currentFind.Address), _
                                      Microsoft.Office.Interop.Excel.XlFindLookIn.xlFormulas, Microsoft.Office.Interop.Excel.XlLookAt.xlPart)

            End While

        End Sub

        Public Sub BreakLinked(ByRef pWorkBook As Microsoft.Office.Interop.Excel.Workbook)
            Dim oLinkLists As Array

            'Draw the Link Collections into current open excel
            oLinkLists = CType(pWorkBook.LinkSources(Microsoft.Office.Interop.Excel.XlLinkType.xlLinkTypeExcelLinks), Array)

            If Not oLinkLists Is Nothing Then
                For Each oLinkList As Object In oLinkLists
                    pWorkBook.BreakLink(oLinkList, Microsoft.Office.Interop.Excel.XlLinkType.xlLinkTypeExcelLinks)
                Next
            End If
        End Sub

        Public Sub ReplaceAllData(ByRef pWorkBook As Microsoft.Office.Interop.Excel.Workbook)

            For Each oWorkSheet As Microsoft.Office.Interop.Excel.Worksheet In pWorkBook.Sheets
                WriteDebug("Now is replace: " & oWorkSheet.Name)

                Try
                    oWorkSheet.Unprotect("compass2008")
                    'If oWorkSheet.ProtectContents = False Then
                    'oWorkSheet.Cells.Replace("!!!!!", "")
                    'Else
                    oWorkSheet.Cells.Replace("!!!!!", _
                                        vbNullString, _
                                        Microsoft.Office.Interop.Excel.XlLookAt.xlPart, _
                                        Microsoft.Office.Interop.Excel.XlSearchOrder.xlByColumns, False)
                    'oWorkSheet.Cells.Replace("!!!!!", "")
                    'End If

                Catch ex As Exception
                    WriteDebug("Exception on Replace Data:" & ex.Message)
                End Try
                WriteDebug("Next")
            Next

        End Sub
        'Karrson: Change ProtectSheet by Function
        Public Sub ProtectedSheet(ByRef pWorkBook As Microsoft.Office.Interop.Excel.Workbook, ByVal ProtectSheet As Boolean)

            Dim oWorkSheet As Microsoft.Office.Interop.Excel.Worksheet
            Dim oExcelPassword As String = oSetting.Password

            For Each oWorkSheet In pWorkBook.Worksheets
                'Karrson: Change

                If ProtectSheet Then
                    oWorkSheet.Protect(Password:=oExcelPassword, UserInterfaceOnly:=True, AllowInsertingRows:=True, AllowFiltering:=True)
                Else

                End If

                'oWorkSheet.Protect()
            Next
        End Sub

        Public Sub UnProtectSheet(ByRef pWorkBook As Microsoft.Office.Interop.Excel.Workbook)
            Dim oWorkSheet As Microsoft.Office.Interop.Excel.Worksheet

            For Each oWorkSheet In pWorkBook.Worksheets
                oWorkSheet.Unprotect()
            Next

        End Sub
#End Region

#Region "SAP Control"
        Public Function getPrjName(ByVal pPrjCode As String) As String
            Dim sqlStr As String
            Dim oPrjName As String

            'sqlStr = "Select PrjCode + '-' + isNull(U_ProjectFullName,'') as PrjName From OPRJ Where PrjCode = '" & pPrjCode & "'"
            sqlStr = "Select isNull(U_ProjectFullName,'') as PrjName From OPRJ Where PrjCode = '" & pPrjCode & "'"
            oPrjName = oSAP_Collection.Execute(sqlStr)

            Return oPrjName
        End Function

        Public Function getPrjNature(ByVal pPrjCode As String) As String
            Dim sqlStr As String
            Dim oPrjNature As String

            'sqlStr = "Select PrjCode + '-' + isNull(U_ProjectFullName,'') as PrjName From OPRJ Where PrjCode = '" & pPrjCode & "'"
            sqlStr = "select u_fatherCode from OPRJ inner join OPRC on oprj.u_prjNature=OPRC.prccode " & _
                    "Where PrjCode = '" & pPrjCode & "'"
            Try
                oPrjNature = oSAP_Collection.Execute(sqlStr)
            Catch ex As Exception
                oPrjNature = String.Empty
            End Try


            Return oPrjNature
        End Function


        Public Function getSubsiName(ByVal pPrjCode As String) As String
            Dim sqlStr As String
            Dim oSubsiName As String

            sqlStr = "Select OPRC.U_PrcDesc as SubsiName From OPRC " & _
                        "inner join OPRJ on OPRC.PrcCode = OPRJ.U_SubsidiaryCode " & _
                        "Where OPRJ.PrjCode = '" & pPrjCode & "'"

            oSubsiName = oSAP_Collection.Execute(sqlStr)

            Return oSubsiName
        End Function

        Public Function getCustName(ByVal custCode As String) As String
            Dim sqlStr As String
            Dim oCustName As String

            sqlStr = "Select isNull(CardName,'') From CPS_View_ClientList Where CardCode = '" & custCode & "'"
            oCustName = oSAP_Collection.Execute(sqlStr)

            Return oCustName
        End Function

#End Region

        Private disposedValue As Boolean = False        ' To detect redundant calls

        ' IDisposable
        Protected Overridable Sub Dispose(ByVal disposing As Boolean)
            If Not Me.disposedValue Then
                If disposing Then
                    ' TODO: free other state (managed objects).
                    Try
                        _Application.Quit()
                    Catch ex As Exception
                    End Try

                    _Application = Nothing
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
            'Karrson: Quit Application
            _Application.Quit()
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
#End Region

    End Class
End Namespace