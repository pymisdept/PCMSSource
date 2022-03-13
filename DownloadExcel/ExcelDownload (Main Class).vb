
Module ExcelDownload
    Private ReadOnly TotalConnects As Integer = 1
    Private _parameter As String
    Public oPCMSConnection As PCMSConnection, oSAP_Collection As SAP_Collection
    Public oSetting As Excel.Setting.Setting
    Public Excel_MapPath As Collection
    Public oLogMessage As CPS.Global.Setting.LogMessage
    Public CheckPoint As String = "", CheckMessage As String = ""
    Public Const VBAFileName As String = "CPS-Addin.xla"

    'Karrson: Modify on 2009-08-22 
    '         Add QS Module
    Public Sub Main()


        If isProcessExists(System.Diagnostics.Process.GetCurrentProcess.ProcessName) Then
            Return
        End If

        OperationSetup()
        Dim oDLNMGR As New Datatable.PCMS.DLNMGR

        'Get the inputted Parameter (Format: <Request ID>)
        If Environment.GetCommandLineArgs.Length = 2 Then
            _parameter = Environment.GetCommandLineArgs.GetValue(1)
        Else
            _parameter = ""
        End If

        CheckPoint = "MAIN00024"
        CheckMessage = "System Database Connection"
        'Define the collection between SAP and PCMS
        oSAP_Collection = New SAP_Collection
        oPCMSConnection = New PCMSConnection

        'Excel Folder Setting
        oSetting = New Excel.Setting.Setting
        oLogMessage = New CPS.Global.Setting.LogMessage(oSAP_Collection)

        oLogMessage.JobName = "Excel Download Request"
        oLogMessage.Argument = _parameter

        'Mapping the Function ID to call which File Name
        Mapping()

        Try
            If Not oSAP_Collection.ErrorDescription = "" Or Not oPCMSConnection.ErrorDescription = "" Then
                Throw New BaseException(BaseException.ErrorType.System, _
                                         CheckPoint, _
                                         CheckMessage, _
                                         "-9999", _
                                         oSAP_Collection.ErrorDescription & vbCrLf & oPCMSConnection.ErrorDescription)
            End If

            'Define Datatable for DLNMGR (Download Excel request table)
            oDLNMGR = New Datatable.PCMS.DLNMGR

            'If parameter is exists, just only filter the Parameter, other wise process all outstanding request.
            If _parameter = "" Then
                oDLNMGR.getDownloadRequest()
            Else
                oDLNMGR.getDownloadRequest(_parameter)
            End If

            Dim ID, Parameter, AllowDownload, Project, FunctionID As String
            Dim oSqlStr() As String = Nothing
            Dim oParameter As Excel.Setting.Parameter
            Dim oDataTable As System.Data.DataTable

            CheckPoint = "MAIN00056"
            CheckMessage = "Execute Query for Download Requested "
            'Start Excel Generate Operation
            oDataTable = oPCMSConnection.DataTable(oDLNMGR.SelectQuery & " " & oDLNMGR.filterQuery)

            ID = ""
            If Not oDataTable Is Nothing Then
                For Each oDataRow As System.Data.DataRow In oDataTable.Rows
                    Try
                        ID = oDataRow(Datatable.PCMS.DLNMGR._RequestID)
                        Parameter = oDataRow(Datatable.PCMS.DLNMGR._Parameter)
                        AllowDownload = oDataRow(Datatable.PCMS.DLNMGR._Ready)
                        Project = oDataRow(Datatable.PCMS.DLNMGR._Project)
                        FunctionID = oDataRow(Datatable.PCMS.DLNMGR._Type)

                        oParameter = New Excel.Setting.Parameter(FunctionID, Parameter)

                        Select Case CType(FunctionID, ProcessMap)
                            'Purchasing Module
                            Case ProcessMap.pm_PUI01
                                xls_PUI01(ID, oParameter)
                            Case ProcessMap.pm_PUI02
                                xls_PUI02(ID, oParameter)
                            Case ProcessMap.pm_PUI03
                                xls_PUI03(ID, oParameter)
                            Case ProcessMap.pm_PUI04
                                xls_PUI04(ID, oParameter)
                            Case ProcessMap.pm_PUI05
                                xls_PUI05(ID, oParameter)
                            Case ProcessMap.pm_PUI06
                                xls_PUI06(ID, oParameter)
                            Case ProcessMap.pm_PUI08
                                xls_PUI08(ID, oParameter)
                            Case ProcessMap.pm_QSI01
                                xls_QSI01(ID, oParameter)
                            Case ProcessMap.pm_QSI02
                                xls_QSI02(ID, oParameter)
                            Case ProcessMap.pm_QSI03
                                xls_QSI03(ID, oParameter)
                            Case ProcessMap.pm_QSI04
                                xls_QSI04(ID, oParameter, oSetting.ServerType)
                            Case ProcessMap.pm_QSI07
                                xls_QSI07(ID, oParameter)
                            Case ProcessMap.pm_QSI08
                                xls_QSI08(ID, oParameter)
                            Case ProcessMap.pm_QSI12
                                xls_QSI12(ID, oParameter, oSetting.ServerType)
                            Case ProcessMap.pm_QSI17
                                xls_QSI17(ID, oParameter)
                            Case ProcessMap.pm_QSI18
                                xls_QSI18(ID, oParameter)
                            Case ProcessMap.pm_QSI20
                                xls_QSI20(ID, oParameter)
                            Case ProcessMap.pm_QSI21
                                xls_QSI21(ID, oParameter)
                            Case ProcessMap.pm_QSI22
                                xls_QSI22(ID, oParameter)
                            Case ProcessMap.pm_QSI23
                                xls_QSI23(ID, oParameter)
                            Case ProcessMap.pm_QSI24
                                xls_QSI24(ID, oParameter)
                            Case ProcessMap.pm_QSI25
                                xls_QSI25(ID, oParameter)
                            Case ProcessMap.pm_QSI26
                                xls_QSI26(ID, oParameter)
                            Case ProcessMap.pm_QSI27
                                xls_QSI27(ID, oParameter)
                            Case ProcessMap.pm_QSI28
                                xls_QSI28(ID, oParameter)
                            Case ProcessMap.pm_QSI29
                                xls_QSI29(ID, oParameter)
                            Case ProcessMap.pm_QSI30
                                xls_QSI30(ID, oParameter)
                            Case ProcessMap.pm_QSI31
                                xls_QSI31(ID, oParameter)
                            Case ProcessMap.pm_QSI32
                                xls_QSI32(ID, oParameter)
                            Case ProcessMap.pm_QSI33
                                xls_QSI33(ID, oParameter)
                            Case ProcessMap.pm_QSI40
                                xls_QSI40(ID, oParameter)
                            Case ProcessMap.pm_QSI41
                                xls_QSI41(ID, oParameter)
                            Case ProcessMap.pm_QSI42
                                xls_QSI42(ID, oParameter)
                            Case ProcessMap.pm_QSI43
                                xls_QSI43(ID, oParameter)
                            Case ProcessMap.pm_QSI44
                                xls_QSI44(ID, oParameter)
                                'Accounts Module
                            Case ProcessMap.pm_ACI01
                                xls_ACI01(ID, oParameter)
                            Case ProcessMap.pm_ACI02
                                xls_ACI02(ID, oParameter)
                                'Management Module
                            Case ProcessMap.pm_MAI02
                                xls_MAI02(ID, oParameter)
                            Case ProcessMap.pm_MA08
                                xls_MA08(ID, oParameter)
                                ' Security Module
                            Case ProcessMap.pm_SEI01
                                xls_SEI01(ID, oParameter)

                        End Select
                    Catch b_ex As BaseException
                        oLogMessage.AddExceptionSkip("Request ID: " & ID, b_ex.toString)
                    Catch ex As Exception
                        oLogMessage.AddExceptionSkip("Request ID: " & ID, ex.ToString)
                    End Try
                   
                Next
            End If
            WriteDebug("End")
End_If_Program:
            oSAP_Collection.Disconnect()
            oPCMSConnection.Disconnect()
            WriteDebug("Disconnect")
            'Karrson: 2010-03-13
            'Kill Excel.exe 
            If TaskKill Then
                Shell(String.Format("cmd.exe /c taskkill  /IM {0} {1}", ProcessName, AdditParameter))
            End If

        Catch ex As Exception
            oLogMessage.AddExceptionSkip(CheckPoint, ex.ToString)
        End Try

        oLogMessage.FileName = "Transaction"
        WriteDebug("Log Path:" & oSetting.Log_Path)
        oLogMessage.FilePath = oSetting.Log_Path
        oLogMessage.ExportToText()
    End Sub

    Enum ProcessMap As Integer
        'Blank Template
        pm_Blank = -9999
        'Purchasing Moudle
        pm_PUI01 = 2003
        pm_PUI02 = 2001
        pm_PUI03 = 2002
        pm_PUI04 = 2004
        pm_PUI05 = 2005
        pm_PUI06 = 2006
        pm_PUI08 = 2008

        'Commercial Moudle
        pm_QSI01 = 1001
        pm_QSI02 = 1002
        pm_QSI03 = 1003
        pm_QSI04 = 1004
        pm_QSI07 = 1007
        pm_QSI08 = 1008
        pm_QSI12 = 1012
        pm_QSI17 = 1017
        pm_QSI18 = 1018
        pm_QSI19 = 1019
        pm_QSI20 = 1020
        pm_QSI21 = 1021
        pm_QSI22 = 1022
        pm_QSI23 = 1023
        pm_QSI24 = 1024
        pm_QSI25 = 1025
        pm_QSI26 = 1026
        pm_QSI27 = 1027
        pm_QSI28 = 1028
        pm_QSI29 = 1029
        pm_QSI30 = 1030
        pm_QSI31 = 1031
        pm_QSI32 = 1032
        pm_QSI33 = 1033
        pm_QSI40 = 1040
        pm_QSI41 = 1041
        pm_QSI42 = 1042
        pm_QSI43 = 1043
        pm_QSI44 = 1044

        'Accounts Moudle
        pm_ACI01 = 3001
        pm_ACI02 = 3002

        'Accounts Moudle
        pm_MAI02 = 4002

        ' Security = Module
        pm_SEI01 = 6010

        pm_MA08 = 4108

    End Enum

    Public Sub Mapping()
        ExcelDownload.Excel_MapPath = New Collection

        'Purchase Module
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.PUI01, ProcessMap.pm_PUI01)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.PUI02_PO, Str(ProcessMap.pm_PUI02).Trim & "_PO")
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.PUI02_MR, Str(ProcessMap.pm_PUI02).Trim & "_MR")

        '//remove multiple input way selection
        'Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.PUI03_PA, Str(ProcessMap.pm_PUI03).Trim & "_PA")
        'Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.PUI03_MR, Str(ProcessMap.pm_PUI03).Trim & "_MR")

        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.PUI03_PA, ProcessMap.pm_PUI03)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.PUI04, ProcessMap.pm_PUI04)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.PUI05_CO, Str(ProcessMap.pm_PUI05).Trim & "_CO")
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.PUI05_SMR, Str(ProcessMap.pm_PUI05).Trim & "_SMR")
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.PUI05_GR, Str(ProcessMap.pm_PUI05).Trim & "_GR")
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.PUI05_PO, Str(ProcessMap.pm_PUI05).Trim & "_PO")
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.PUI06, ProcessMap.pm_PUI06)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.PUI08, ProcessMap.pm_PUI08)

        'Commercial Module
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI01, ProcessMap.pm_QSI01)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI02, ProcessMap.pm_QSI02)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI03, ProcessMap.pm_QSI03)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI04, ProcessMap.pm_QSI04)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI07, ProcessMap.pm_QSI07)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI08, ProcessMap.pm_QSI08)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI12, ProcessMap.pm_QSI12)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI17, ProcessMap.pm_QSI17)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI18_Bill, Str(ProcessMap.pm_QSI18).Trim & "_Bill")
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI18_Work, Str(ProcessMap.pm_QSI18).Trim & "_Work")
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI19, ProcessMap.pm_QSI19)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI20, ProcessMap.pm_QSI20)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI21, ProcessMap.pm_QSI21)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI22, ProcessMap.pm_QSI22)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI23, ProcessMap.pm_QSI23)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI24, ProcessMap.pm_QSI24)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI25, ProcessMap.pm_QSI25)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI26, ProcessMap.pm_QSI26)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI27, ProcessMap.pm_QSI27)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI28, ProcessMap.pm_QSI28)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI29, ProcessMap.pm_QSI29)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI30, ProcessMap.pm_QSI30)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI31, ProcessMap.pm_QSI31)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI32, ProcessMap.pm_QSI32)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI33, ProcessMap.pm_QSI33)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI40, ProcessMap.pm_QSI40)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI40_Blank, Str(ProcessMap.pm_QSI40).Trim & "_Blank")
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI41, ProcessMap.pm_QSI41)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI42, ProcessMap.pm_QSI42)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI43, ProcessMap.pm_QSI43)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.QSI44, ProcessMap.pm_QSI44)
        'Accounts Module
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.ACI01, ProcessMap.pm_ACI01)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.ACI02, ProcessMap.pm_ACI02)

        'Management Module
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.MAI02, ProcessMap.pm_MAI02)
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.MA08, ProcessMap.pm_MA08)
        'Security Module
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.SEI01, ProcessMap.pm_SEI01)
        'Blank Template
        Excel_MapPath.Add(oSetting.Import_Path & "\" & Excel.Setting.ExcelPath.Blank, ProcessMap.pm_Blank)

    End Sub

    Public Function FileReader(ByVal pFilePath As String) As Byte()
        ' Open a file that is to be loaded into a byte array
        Dim oFile As System.IO.FileInfo
        oFile = New System.IO.FileInfo(pFilePath)

        Dim oFileStream As System.IO.FileStream = oFile.OpenRead()
        Dim lBytes As Long = oFileStream.Length

        If (lBytes > 0) Then
            Dim fileData(lBytes - 1) As Byte

            ' Read the file into a byte array
            oFileStream.Read(fileData, 0, lBytes)
            oFileStream.Close()

            Return fileData
        Else
            Return Nothing
        End If
    End Function

#Region "Excel Generate Source"

    'Purchase Module
    Public Sub xls_PUI01(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oPUI01 As New Excel.ProcessClass.PUI01(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "PUI01_0001"
            CheckMessage = "Define Parameter Variable"
            oPUI01.ProjectCode = pParameter.Parameter.projCode
            oPUI01.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "PUI01_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oPUI01.SheetMapping

            CheckPoint = "PUI01_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oPUI01.Import(oSqlStr)
            oPUI01.Import(oSqlStr, isProtectSheet("PUI01"))

            CheckPoint = "PUI01_0004"
            CheckMessage = "Export Excel result into Database"
            oPUI01.Export()

            CheckPoint = "PUI01_0005"
            CheckMessage = "Release Resource"
            oPUI01.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oPUI01.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "PUI01", ex.ToString)
        End Try
    End Sub
    Public Sub xls_PUI02(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)

        Dim oPUI02 As New Excel.ProcessClass.PUI02(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "PUI02_0001"
            CheckMessage = "Define Parameter Variable"
            oPUI02.ProjectCode = pParameter.Parameter.projCode
            oPUI02.FunctionID = pParameter.Parameter.functionID
            oPUI02.inputType = pParameter.Parameter.inputType
            oPUI02.MRNO = pParameter.Parameter.mrNo

            CheckPoint = "PUI02_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oPUI02.SheetMapping

            CheckPoint = "PUI02_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oPUI02.Import(oSqlStr)
            oPUI02.Import(oSqlStr, isProtectSheet("PUI02"))

            CheckPoint = "PUI02_0004"
            CheckMessage = "Export Excel result into Database"
            oPUI02.Export()

            CheckPoint = "PUI02_0005"
            CheckMessage = "Release Resource"
            oPUI02.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oPUI02.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "PUI02", ex.ToString)
        End Try
    End Sub

    Public Sub xls_PUI03(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oPUI03 As New Excel.ProcessClass.PUI03(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "PUI03_0001"
            CheckMessage = "Define Parameter Variable"
            oPUI03.ProjectCode = pParameter.Parameter.projCode
            oPUI03.FunctionID = pParameter.Parameter.functionID
            '//remove multiple input way selection
            'oPUI03.inputType = pParameter.Parameter.inputType
            'oPUI03.MRNO = pParameter.Parameter.mrNo

            CheckPoint = "PUI03_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oPUI03.SheetMapping

            CheckPoint = "PUI03_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oPUI03.Import(oSqlStr)
            oPUI03.Import(oSqlStr, isProtectSheet("PUI03"))

            CheckPoint = "PUI03_0004"
            CheckMessage = "Export Excel result into Database"
            oPUI03.Export()

            CheckPoint = "PUI03_0005"
            CheckMessage = "Release Resource"
            oPUI03.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oPUI03.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "PUI03", ex.ToString)
        End Try
    End Sub
    Public Sub xls_PUI04(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oPUI04 As New Excel.ProcessClass.PUI04(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "PUI04_0001"
            CheckMessage = "Define Parameter Variable"
            oPUI04.ProjectCode = pParameter.Parameter.projCode
            oPUI04.FunctionID = pParameter.Parameter.functionID
            oPUI04.PANumber = pParameter.Parameter.PANum

            CheckPoint = "PUI04_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oPUI04.SheetMapping

            CheckPoint = "PUI04_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oPUI04.Import(oSqlStr)
            oPUI04.Import(oSqlStr, isProtectSheet("PUI04"))

            CheckPoint = "PUI04_0004"
            CheckMessage = "Export Excel result into Database"
            oPUI04.Export()

            CheckPoint = "PUI04_0005"
            CheckMessage = "Release Resource"
            oPUI04.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oPUI04.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "PUI04", ex.ToString)
        End Try
    End Sub


    ''' <summary>
    ''' ' Karrson: Modify on 2009-08-23 '''' New Parameter (Vendor Code)
    ''' </summary>
    ''' <param name="pID"></param>
    ''' <param name="pParameter"></param>
    ''' <remarks></remarks>
    Public Sub xls_PUI05(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oPUI05 As New Excel.ProcessClass.PUI05(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "PUI05_0001"
            CheckMessage = "Define Parameter Variable"
            oPUI05.ProjectCode = pParameter.Parameter.projCode
            oPUI05.FunctionID = pParameter.Parameter.functionID

            oPUI05.InputType = pParameter.Parameter.inputType

            ' New Parameter
            oPUI05.Vendor = pParameter.Parameter.Vendor


            CheckPoint = "PUI05_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oPUI05.SheetMapping

            CheckPoint = "PUI05_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oPUI05.Import(oSqlStr)
            oPUI05.Import(oSqlStr, isProtectSheet("PUI05"))

            CheckPoint = "PUI05_0004"
            CheckMessage = "Export Excel result into Database"
            oPUI05.Export()

            CheckPoint = "PUI05_0005"
            CheckMessage = "Release Resource"
            oPUI05.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oPUI05.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "PUI05", ex.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' ' Karrson: Modify on 2009-09-22 '''' New Parameter (Vendor Code)
    ''' </summary>
    ''' <param name="pID"></param>
    ''' <param name="pParameter"></param>
    ''' <remarks></remarks>
    Public Sub xls_PUI06(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oPUI06 As New Excel.ProcessClass.PUI06(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "PUI06_0001"
            CheckMessage = "Define Parameter Variable"
            oPUI06.ProjectCode = pParameter.Parameter.projCode
            oPUI06.FunctionID = pParameter.Parameter.functionID
            oPUI06.PANumber = pParameter.Parameter.PANum

            ' New Parameter
            'oPUI05.Vendor = pParameter.Parameter.Vendor


            CheckPoint = "PUI06_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oPUI06.SheetMapping

            CheckPoint = "PUI06_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oPUI06.Import(oSqlStr)
            oPUI06.Import(oSqlStr, isProtectSheet("PUI06"))

            CheckPoint = "PUI06_0004"
            CheckMessage = "Export Excel result into Database"
            oPUI06.Export()

            CheckPoint = "PUI06_0005"
            CheckMessage = "Release Resource"
            oPUI06.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oPUI06.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "PUI06", ex.ToString)
        End Try
    End Sub


    Public Sub xls_PUI08(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oPUI08 As New Excel.ProcessClass.PUI08(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "PUI08_0001"
            CheckMessage = "Define Parameter Variable"
            oPUI08.ProjectCode = pParameter.Parameter.projCode
            oPUI08.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "PUI08_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oPUI08.SheetMapping

            CheckPoint = "PUI08_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oPUI06.Import(oSqlStr)
            oPUI08.Import(oSqlStr, isProtectSheet("PUI08"))

            CheckPoint = "PUI08_0004"
            CheckMessage = "Export Excel result into Database"
            oPUI08.Export()

            CheckPoint = "PUI08_0005"
            CheckMessage = "Release Resource"
            oPUI08.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oPUI08.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "PUI08", ex.ToString)
        End Try
    End Sub

    'Sales Module
    ''' <summary>
    ''' Karrson: Modified Function on 2009-08-25
    ''' </summary>
    ''' <param name="pID"></param>
    ''' <param name="pParameter"></param>
    ''' <remarks></remarks>
    Public Sub xls_QSI01(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI01 As New Excel.ProcessClass.QSI01(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI01_0001"

            CheckMessage = "Define Parameter Variable"
            'Karrson Debug
            WriteDebug(CheckPoint)
            WriteDebug(CheckMessage)
            oQSI01.ProjectCode = pParameter.Parameter.projCode
            oQSI01.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI01_0002"
            CheckMessage = "Define Sql Collection"
            'Karrson Debug
            WriteDebug(CheckPoint)
            WriteDebug(CheckMessage)
            oSqlStr = oQSI01.SheetMapping

            CheckPoint = "QSI01_0003"
            CheckMessage = "Import Collection into Excel Application"
            'Karrson Debug
            WriteDebug(CheckPoint)
            WriteDebug(CheckMessage)
            'oQSI01.Import(oSqlStr)
            oQSI01.Import(oSqlStr, isProtectSheet("QSI01"))

            CheckPoint = "QSI01_0004"
            CheckMessage = "Export Excel result into Database"
            'Karrson Debug
            WriteDebug(CheckPoint)
            WriteDebug(CheckMessage)
            oQSI01.Export()

            CheckPoint = "QSI01_0005"
            CheckMessage = "Release Resource"
            'Karrson Debug
            WriteDebug(CheckPoint)
            WriteDebug(CheckMessage)
            oQSI01.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI01.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI01", ex.ToString)
            'Karrson Debug
            WriteDebug("Exception:" & ex.Message)

        End Try


    End Sub
    ''' <summary>
    ''' Karrson: Modified Function on 2009-08-25
    ''' </summary>
    ''' <param name="pID"></param>
    ''' <param name="pParameter"></param>
    ''' <remarks></remarks>
    Public Sub xls_QSI02(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI02 As New Excel.ProcessClass.QSI02(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI02_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI02.ProjectCode = pParameter.Parameter.projCode
            oQSI02.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI02_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI02.SheetMapping

            CheckPoint = "QSI02_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI02.Import(oSqlStr)
            oQSI02.Import(oSqlStr, isProtectSheet("QSI02"))

            CheckPoint = "QSI02_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI02.Export()

            CheckPoint = "QSI02_0005"
            CheckMessage = "Release Resource"
            oQSI02.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI02.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI02", ex.ToString)
        End Try

    End Sub
    ''' <summary>
    ''' Karrson: Modified Function on 2009-08-25
    ''' </summary>
    ''' <param name="pID"></param>
    ''' <param name="pParameter"></param>
    ''' <remarks></remarks>
    Public Sub xls_QSI03(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI03 As New Excel.ProcessClass.QSI03(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI03_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI03.ProjectCode = pParameter.Parameter.projCode
            oQSI03.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI03_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI03.SheetMapping

            CheckPoint = "QSI03_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI03.Import(oSqlStr)
            oQSI03.Import(oSqlStr, isProtectSheet("QSI03"))

            CheckPoint = "QSI03_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI03.Export()

            CheckPoint = "QSI03_0005"
            CheckMessage = "Release Resource"
            oQSI03.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI03.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI03", ex.ToString)
        End Try
    End Sub
    ''' <summary>
    ''' Karrson: Modified Function on 2009-08-25
    ''' </summary>
    ''' <param name="pID"></param>
    ''' <param name="pParameter"></param>
    ''' <remarks></remarks>
    Public Sub xls_QSI04(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter, ByVal serverType As String)
        Dim oQSI04 As New Excel.ProcessClass.QSI04(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI04_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI04.ProjectCode = pParameter.Parameter.projCode
            oQSI04.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI04_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI04.SheetMapping(serverType)

            CheckPoint = "QSI04_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI04.Import(oSqlStr)
            oQSI04.Import(oSqlStr, isProtectSheet("QSI04"))

            CheckPoint = "QSI04_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI04.Export()

            CheckPoint = "QSI04_0005"
            CheckMessage = "Release Resource"
            oQSI04.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI04.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI04", ex.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' QSI07
    ''' </summary>
    ''' <param name="pID"></param>
    ''' <param name="pParameter"></param>
    ''' <remarks></remarks>
    Public Sub xls_QSI07(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI07 As New Excel.ProcessClass.QSI07(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI07_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI07.ProjectCode = pParameter.Parameter.projCode
            oQSI07.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI07_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI07.SheetMapping

            CheckPoint = "QSI07_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI07.Import(oSqlStr)
            oQSI07.Import(oSqlStr, isProtectSheet("QSI07"))

            CheckPoint = "QSI07_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI07.Export()

            CheckPoint = "QSI07_0005"
            CheckMessage = "Release Resource"
            oQSI07.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI07.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI07", ex.ToString)
        End Try
    End Sub

    ''' <summary>
    ''' QSI08
    ''' </summary>
    ''' <param name="pID"></param>
    ''' <param name="pParameter"></param>
    ''' <remarks></remarks>
    Public Sub xls_QSI08(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI08 As New Excel.ProcessClass.QSI08(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI08_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI08.ProjectCode = pParameter.Parameter.projCode
            oQSI08.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI08_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI08.SheetMapping

            CheckPoint = "QSI08_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI08.Import(oSqlStr)
            oQSI08.Import(oSqlStr, isProtectSheet("QSI08"))

            CheckPoint = "QSI08_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI08.Export()

            CheckPoint = "QSI08_0005"
            CheckMessage = "Release Resource"
            oQSI08.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI08.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI08", ex.ToString)
        End Try
    End Sub
    ''' <summary>
    ''' Karrson: New Function on 2009-08-25
    ''' </summary>
    ''' <param name="pID"></param>
    ''' <param name="pParameter"></param>
    ''' <remarks></remarks>
    Public Sub xls_QSI12(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter, ByVal serverType As String)
        Dim oQSI12 As New Excel.ProcessClass.QSI12(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI12_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI12.ProjectCode = pParameter.Parameter.projCode
            oQSI12.FunctionID = pParameter.Parameter.functionID
            oQSI12.SubContractNo = pParameter.Parameter.subContract
            oQSI12.SubContractorCode = pParameter.Parameter.subContractor

            CheckPoint = "QSI12_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI12.SheetMapping(serverType)

            CheckPoint = "QSI12_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI12.Import(oSqlStr)
            oQSI12.Import(oSqlStr, isProtectSheet("QSI12"))

            CheckPoint = "QSI12_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI12.Export()

            CheckPoint = "QSI12_0005"
            CheckMessage = "Release Resource"
            oQSI12.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI12.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI12", ex.ToString)
        End Try

    End Sub
    ''' <summary>
    ''' Karrson: Add Function on 2009-08-26
    ''' </summary>
    ''' <param name="pID"></param>
    ''' <param name="pParameter"></param>
    ''' <remarks></remarks>
    Public Sub xls_QSI17(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI17 As New Excel.ProcessClass.QSI17(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI17_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI17.ProjectCode = pParameter.Parameter.projCode
            oQSI17.FunctionID = pParameter.Parameter.functionID


            CheckPoint = "QSI17_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI17.SheetMapping

            CheckPoint = "QSI17_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI17.Import(oSqlStr)
            oQSI17.Import(oSqlStr, isProtectSheet("QSI17"))

            CheckPoint = "QSI17_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI17.Export()

            CheckPoint = "QSI17_0005"
            CheckMessage = "Release Resource"
            oQSI17.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI17.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI17", ex.ToString)
        End Try
    End Sub
    Public Sub xls_QSI18(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI18 As New Excel.ProcessClass.QSI18(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI18_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI18.ProjectCode = pParameter.Parameter.projCode
            oQSI18.FunctionID = pParameter.Parameter.functionID
            WriteDebug(CheckPoint)


            CheckPoint = "QSI18_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI18.SheetMapping

            WriteDebug(CheckPoint)

            CheckPoint = "QSI18_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI18.Import(oSqlStr)
            oQSI18.Import(oSqlStr, isProtectSheet("QSI18"))

            WriteDebug(CheckPoint)

            CheckPoint = "QSI18_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI18.Export()

            WriteDebug(CheckPoint)

            CheckPoint = "QSI18_0005"
            CheckMessage = "Release Resource"
            oQSI18.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            WriteDebug("QSI18 Exception: " & ex.Message)
            oQSI18.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI18", ex.ToString)
        End Try
    End Sub
    Public Sub xls_QSI19(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)

    End Sub
    Public Sub xls_QSI20(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI20 As New Excel.ProcessClass.QSI20(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI20_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI20.ProjectCode = pParameter.Parameter.projCode
            oQSI20.FunctionID = pParameter.Parameter.functionID


            CheckPoint = "QSI20_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI20.SheetMapping

            CheckPoint = "QSI20_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI20.Import(oSqlStr)
            oQSI20.Import(oSqlStr, isProtectSheet("QSI20"))

            CheckPoint = "QSI20_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI20.Export()

            CheckPoint = "QSI20_0005"
            CheckMessage = "Release Resource"
            oQSI20.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI20.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI20", ex.ToString)
        End Try
    End Sub

    Public Sub xls_QSI21(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI21 As New Excel.ProcessClass.QSI21(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI21_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI21.ProjectCode = pParameter.Parameter.projCode
            oQSI21.FunctionID = pParameter.Parameter.functionID


            CheckPoint = "QSI21_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI21.SheetMapping

            CheckPoint = "QSI21_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI21.Import(oSqlStr)
            oQSI21.Import(oSqlStr, isProtectSheet("QSI21"))

            CheckPoint = "QSI21_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI21.Export()

            CheckPoint = "QSI21_0005"
            CheckMessage = "Release Resource"
            oQSI21.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI21.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI21", ex.ToString)
        End Try
    End Sub
    Public Sub xls_QSI22(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI22 As New Excel.ProcessClass.QSI22(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI22_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI22.ProjectCode = pParameter.Parameter.projCode
            oQSI22.FunctionID = pParameter.Parameter.functionID


            CheckPoint = "QSI22_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI22.SheetMapping

            CheckPoint = "QSI22_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI22.Import(oSqlStr)
            oQSI22.Import(oSqlStr, isProtectSheet("QSI22"))

            CheckPoint = "QSI22_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI22.Export()

            CheckPoint = "QSI22_0005"
            CheckMessage = "Release Resource"
            oQSI22.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI22.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI22", ex.ToString)
        End Try
    End Sub
    Public Sub xls_QSI23(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI23 As New Excel.ProcessClass.QSI23(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI23_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI23.ProjectCode = pParameter.Parameter.projCode
            oQSI23.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI23_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI23.SheetMapping

            CheckPoint = "QSI23_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI23.Import(oSqlStr)
            oQSI23.Import(oSqlStr, isProtectSheet("QSI23"))

            CheckPoint = "QSI23_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI23.Export()

            CheckPoint = "QSI23_0005"
            CheckMessage = "Release Resource"
            oQSI23.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI23.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI23", ex.ToString)
        End Try
    End Sub
    Public Sub xls_QSI24(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI24 As New Excel.ProcessClass.QSI24(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI24_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI24.ProjectCode = pParameter.Parameter.projCode
            oQSI24.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI24_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI24.SheetMapping

            CheckPoint = "QSI24_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI24.Import(oSqlStr)
            oQSI24.Import(oSqlStr, isProtectSheet("QSI24"))

            CheckPoint = "QSI24_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI24.Export()

            CheckPoint = "QSI24_0005"
            CheckMessage = "Release Resource"
            oQSI24.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI24.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI24", ex.ToString)
        End Try
    End Sub

    Public Sub xls_QSI25(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI25 As New Excel.ProcessClass.QSI25(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI25_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI25.ProjectCode = pParameter.Parameter.projCode
            oQSI25.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI25_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI25.SheetMapping

            CheckPoint = "QSI25_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI25.Import(oSqlStr)
            oQSI25.Import(oSqlStr, isProtectSheet("QSI25"))

            CheckPoint = "QSI25_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI25.Export()

            CheckPoint = "QSI25_0005"
            CheckMessage = "Release Resource"
            oQSI25.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI25.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI25", ex.ToString)
        End Try
    End Sub

    Public Sub xls_QSI26(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI26 As New Excel.ProcessClass.QSI26(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI26_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI26.ProjectCode = pParameter.Parameter.projCode
            oQSI26.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI26_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI26.SheetMapping

            CheckPoint = "QSI26_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI26.Import(oSqlStr)
            oQSI26.Import(oSqlStr, isProtectSheet("QSI26"))

            CheckPoint = "QSI26_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI26.Export()

            CheckPoint = "QSI26_0005"
            CheckMessage = "Release Resource"
            oQSI26.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI26.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI26", ex.ToString)
        End Try
    End Sub

    Public Sub xls_QSI27(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI27 As New Excel.ProcessClass.QSI27(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI27_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI27.ProjectCode = pParameter.Parameter.projCode
            oQSI27.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI27_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI27.SheetMapping

            CheckPoint = "QSI27_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI27.Import(oSqlStr)
            oQSI27.Import(oSqlStr, isProtectSheet("QSI27"))

            CheckPoint = "QSI27_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI27.Export()

            CheckPoint = "QSI27_0005"
            CheckMessage = "Release Resource"
            oQSI27.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI27.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI27", ex.ToString)
        End Try
    End Sub

    Public Sub xls_QSI28(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI28 As New Excel.ProcessClass.QSI28(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI28_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI28.ProjectCode = pParameter.Parameter.projCode
            oQSI28.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI28_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI28.SheetMapping

            CheckPoint = "QSI28_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI28.Import(oSqlStr)
            oQSI28.Import(oSqlStr, isProtectSheet("QSI28"))

            CheckPoint = "QSI28_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI28.Export()

            CheckPoint = "QSI28_0005"
            CheckMessage = "Release Resource"
            oQSI28.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI28.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI28", ex.ToString)
        End Try
    End Sub

    Public Sub xls_QSI29(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI29 As New Excel.ProcessClass.QSI29(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI29_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI29.ProjectCode = pParameter.Parameter.projCode
            oQSI29.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI29_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI29.SheetMapping

            CheckPoint = "QSI29_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI29.Import(oSqlStr)
            oQSI29.Import(oSqlStr, isProtectSheet("QSI29"))

            CheckPoint = "QSI29_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI29.Export()

            CheckPoint = "QSI29_0005"
            CheckMessage = "Release Resource"
            oQSI29.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI29.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI29", ex.ToString)
        End Try
    End Sub

    Public Sub xls_QSI30(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI30 As New Excel.ProcessClass.QSI30(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI30_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI30.ProjectCode = pParameter.Parameter.projCode
            oQSI30.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI30_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI30.SheetMapping

            CheckPoint = "QSI30_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI30.Import(oSqlStr)
            oQSI30.Import(oSqlStr, isProtectSheet("QSI30"))

            CheckPoint = "QSI30_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI30.Export()

            CheckPoint = "QSI30_0005"
            CheckMessage = "Release Resource"
            oQSI30.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI30.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI30", ex.ToString)
        End Try
    End Sub

    Public Sub xls_QSI31(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI31 As New Excel.ProcessClass.QSI31(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI31_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI31.ProjectCode = pParameter.Parameter.projCode
            oQSI31.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI31_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI31.SheetMapping

            CheckPoint = "QSI31_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI31.Import(oSqlStr)
            oQSI31.Import(oSqlStr, isProtectSheet("QSI31"))

            CheckPoint = "QSI31_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI31.Export()

            CheckPoint = "QSI31_0005"
            CheckMessage = "Release Resource"
            oQSI31.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI31.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI31", ex.ToString)
        End Try
    End Sub

    Public Sub xls_QSI32(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI32 As New Excel.ProcessClass.QSI32(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI32_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI32.ProjectCode = pParameter.Parameter.projCode
            oQSI32.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI32_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI32.SheetMapping

            CheckPoint = "QSI32_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI32.Import(oSqlStr)
            oQSI32.Import(oSqlStr, isProtectSheet("QSI32"))

            CheckPoint = "QSI32_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI32.Export()

            CheckPoint = "QSI32_0005"
            CheckMessage = "Release Resource"
            oQSI32.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI32.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI32", ex.ToString)
        End Try
    End Sub

    Public Sub xls_QSI33(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI33 As New Excel.ProcessClass.QSI33(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI33_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI33.ProjectCode = pParameter.Parameter.projCode
            oQSI33.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI33_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI33.SheetMapping

            CheckPoint = "QSI33_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oQSI33.Import(oSqlStr)
            oQSI33.Import(oSqlStr, isProtectSheet("QSI33"))

            CheckPoint = "QSI33_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI33.Export()

            CheckPoint = "QSI33_0005"
            CheckMessage = "Release Resource"
            oQSI33.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI33.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI33", ex.ToString)
        End Try
    End Sub

    'Accounts Module
    Public Sub xls_ACI01(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oACI01 As New Excel.ProcessClass.ACI01(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "ACI01_0001"
            CheckMessage = "Define Parameter Variable"
            oACI01.ProjectCode = pParameter.Parameter.projCode
            oACI01.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "ACI01_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oACI01.SheetMapping

            CheckPoint = "ACI01_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oACI01.Import(oSqlStr)
            oACI01.Import(oSqlStr, isProtectSheet("ACI01"))

            CheckPoint = "ACI01_0004"
            CheckMessage = "Export Excel result into Database"
            oACI01.Export()

            CheckPoint = "ACI01_0005"
            CheckMessage = "Release Resource"
            oACI01.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oACI01.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "ACI01", ex.ToString)
        End Try
    End Sub

    Public Sub xls_ACI02(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oACI02 As New Excel.ProcessClass.ACI02(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "ACI02_0001"
            CheckMessage = "Define Parameter Variable"
            oACI02.ProjectCode = pParameter.Parameter.projCode
            oACI02.FunctionID = pParameter.Parameter.functionID
            oACI02.SectionCode = pParameter.Parameter.sectionCode


            CheckPoint = "ACI02_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oACI02.SheetMapping

            CheckPoint = "ACI02_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oACI02.Import(oSqlStr)
            oACI02.Import(oSqlStr, isProtectSheet("ACI02"))

            CheckPoint = "ACI02_0004"
            CheckMessage = "Export Excel result into Database"
            oACI02.Export()

            CheckPoint = "ACI02_0005"
            CheckMessage = "Release Resource"
            oACI02.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oACI02.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "ACI02", ex.ToString)
        End Try
    End Sub

    Public Sub xls_MAI02(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oMAI02 As New Excel.ProcessClass.MAI02(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "MAI02_0001"
            CheckMessage = "Define Parameter Variable"
            oMAI02.ProjectCode = pParameter.Parameter.projCode
            oMAI02.FunctionID = pParameter.Parameter.functionID
            oMAI02.SectionCode = pParameter.Parameter.sectionCode

            CheckPoint = "MAI02_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oMAI02.SheetMapping

            CheckPoint = "MAI02_0003"
            CheckMessage = "Import Collection into Excel Application"
            'oMAI02.Import(oSqlStr)
            oMAI02.Import(oSqlStr, isProtectSheet("MAI02"))

            CheckPoint = "MAI02_0004"
            CheckMessage = "Export Excel result into Database"
            oMAI02.Export()

            CheckPoint = "MAI02_0005"
            CheckMessage = "Release Resource"
            oMAI02.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oMAI02.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "MAI02", ex.ToString)
        End Try
    End Sub

    Public Sub xls_SEI01(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oSEI01 As New Excel.ProcessClass.SEI01(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "SEI01_0001"
            CheckMessage = "Define Parameter Variable"
            oSEI01.ProjectCode = pParameter.Parameter.projCode
            oSEI01.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "SEI01_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oSEI01.SheetMapping

            CheckPoint = "SEI01_0003"
            CheckMessage = "Import Collection into Excel Application"
            oSEI01.Import(oSqlStr, isProtectSheet("SEI01"))

            CheckPoint = "SEI01_0004"
            CheckMessage = "Export Excel result into Database"
            oSEI01.Export()

            CheckPoint = "SEI01_0005"
            CheckMessage = "Release Resource"
            oSEI01.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oSEI01.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "MAI02", ex.ToString)
        End Try
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="pID"></param>
    ''' <param name="pParameter"></param>
    ''' <remarks></remarks>
    Public Sub xls_QSI40(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI40 As New Excel.ProcessClass.QSI40(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI40_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI40.ProjectCode = pParameter.Parameter.projCode
            oQSI40.FunctionID = pParameter.Parameter.functionID
            oQSI40.CutOffDate = pParameter.Parameter.CutOffDate
            oQSI40.isBlankTemplate = pParameter.Parameter.isBlankTemplate

            CheckPoint = "QSI40_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI40.SheetMapping

            CheckPoint = "QSI40_0003"
            CheckMessage = "Import Collection into Excel Application"

            oQSI40.Import(oSqlStr, isProtectSheet("QSI40"))

            CheckPoint = "QS40_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI40.Export()

            CheckPoint = "QSI40_0005"
            CheckMessage = "Release Resource"
            oQSI40.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI40.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI02", ex.ToString)
        End Try

    End Sub
    Public Sub xls_QSI42(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI42 As New Excel.ProcessClass.QSI42(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI42_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI42.ProjectCode = pParameter.Parameter.projCode
            oQSI42.FunctionID = pParameter.Parameter.functionID

            CheckPoint = "QSI42_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI42.SheetMapping

            CheckPoint = "QSI42_0003"
            CheckMessage = "Import Collection into Excel Application"

            oQSI42.Import(oSqlStr, isProtectSheet("QSI42"))

            CheckPoint = "QS42_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI42.Export()

            CheckPoint = "QSI42_0005"
            CheckMessage = "Release Resource"
            oQSI42.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI42.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI42", ex.ToString)
        End Try

    End Sub
    Public Sub xls_QSI43(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI43 As New Excel.ProcessClass.QSI43(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI43_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI43.ProjectCode = pParameter.Parameter.projCode
            oQSI43.FunctionID = pParameter.Parameter.functionID
            oQSI43.CustCode = pParameter.Parameter.custCode

            CheckPoint = "QSI43_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI43.SheetMapping

            CheckPoint = "QSI43_0003"
            CheckMessage = "Import Collection into Excel Application"

            oQSI43.Import(oSqlStr, isProtectSheet("QSI43"))

            CheckPoint = "QS43_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI43.Export()

            CheckPoint = "QSI43_0005"
            CheckMessage = "Release Resource"
            oQSI43.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI43.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI43", ex.ToString)
        End Try

    End Sub

    Public Sub xls_QSI44(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI44 As New Excel.ProcessClass.QSI44(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI44_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI44.ProjectCode = pParameter.Parameter.projCode
            oQSI44.FunctionID = pParameter.Parameter.functionID
            oQSI44.CustCode = pParameter.Parameter.custCode

            CheckPoint = "QSI44_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI44.SheetMapping

            CheckPoint = "QSI44_0003"
            CheckMessage = "Import Collection into Excel Application"

            oQSI44.Import(oSqlStr, isProtectSheet("QSI44"))

            CheckPoint = "QS44_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI44.Export()

            CheckPoint = "QSI44_0005"
            CheckMessage = "Release Resource"
            oQSI44.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI44.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI44", ex.ToString)
        End Try

    End Sub

    Public Sub xls_MA08(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oMA08 As New Excel.ProcessClass.MA08(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "MA08_0001"
            CheckMessage = "Define Parameter Variable"
            oMA08.SectionCode = pParameter.Parameter.sectionCode
            oMA08.FunctionID = pParameter.Parameter.functionID
            oMA08.CutOffDate = pParameter.Parameter.CutOffDate


            CheckPoint = "MA08_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oMA08.SheetMapping

            CheckPoint = "MA08_0003"
            CheckMessage = "Import Collection into Excel Application"

            oMA08.Import(oSqlStr, isProtectSheet("MA08"))

            CheckPoint = "MA08_0004"
            CheckMessage = "Export Excel result into Database"
            oMA08.Export()

            CheckPoint = "MA08_0005"
            CheckMessage = "Release Resource"
            oMA08.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oMA08.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "MA08", ex.ToString)
        End Try

    End Sub

    Public Sub xls_QSI41(ByVal pID As String, ByVal pParameter As Excel.Setting.Parameter)
        Dim oQSI41 As New Excel.ProcessClass.QSI41(pID)
        Dim oSqlStr() As String

        Try
            CheckPoint = "QSI41_0001"
            CheckMessage = "Define Parameter Variable"
            oQSI41.ProjectCode = pParameter.Parameter.projCode
            oQSI41.FunctionID = pParameter.Parameter.functionID
            oQSI41.SubContractNo = pParameter.Parameter.subContract
            oQSI41.SubContractorCode = pParameter.Parameter.subContractor

            CheckPoint = "QSI41_0002"
            CheckMessage = "Define Sql Collection"
            oSqlStr = oQSI41.SheetMapping

            CheckPoint = "QSI41_0003"
            CheckMessage = "Import Collection into Excel Application"

            oQSI41.Import(oSqlStr, isProtectSheet("QSI41"))

            CheckPoint = "QSI41_0004"
            CheckMessage = "Export Excel result into Database"
            oQSI41.Export()

            CheckPoint = "QSI41_0005"
            CheckMessage = "Release Resource"
            oQSI41.Dispose()

            oLogMessage.AddSuccessLine(pID, "Success")
        Catch ex As Exception
            oQSI41.Dispose()
            Throw New BaseException(BaseException.ErrorType.Normal, CheckPoint, CheckMessage, "QSI41", ex.ToString)
        End Try

    End Sub
#End Region

End Module
