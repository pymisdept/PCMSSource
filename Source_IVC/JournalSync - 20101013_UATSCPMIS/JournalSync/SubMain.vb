Imports SAPbobsCOM
Imports CPS.CPSLIB.Debug
Module SubMain

    Private ReadOnly TotalConnects As Integer = 1
    Private _parameter As String
    Public commonRecordSet As SQL.RecordSet
    Public currentCompany As SAPbobsCOM.Company

    Public ReadOnly Argument As String = "Server Batch Job"
    Public Const RetryCount As Integer = 5

    'Log Path
    Public _LogPath As String
#Region "Get Parameter in Program"
    Public Function getDocEntry() As Integer
        If _parameter = "" Then
            Return ""
        Else
            Return _parameter.Split(",")(0)
        End If
    End Function

    Public Function getObjectKey() As Integer
        If _parameter = "" Then
            Return ""
        Else
            Return _parameter.Split(",")(1)
        End If
    End Function

    Public Function getTransType() As TransType
        If _parameter = "" Then
            Return TransType.tt_None
        Else
            Return _parameter.Split(",")(0)
        End If
    End Function

    Public Function getDirection() As Direction
        If _parameter = "" Then
            Return Direction.dt_NONE
        Else
            Return _parameter.Split(",")(1)
        End If
    End Function
#End Region

    Public Sub Main()
        TimeSet.Start()
        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName, TimeSet.Status.Start)
        Dim oCompanies As CPS.DI.Console.Company
        Dim AP_SrvInvoice As SyncMainClass.P_SrvInvoice
        Dim AR_ItmInvoice As SyncMainClass.S_Invoice
        Dim AP_AppInvoice As SyncMainClass.A_ItmInvoice
        Dim Master_BP As SyncMainClass.BusinessPartners
        Dim Master_PJ As SyncMainClass.Project_Code
        Dim AR_SalesQuotation As SyncMainClass.Sales_Quotation
        Dim AR_SalesOrder As SyncMainClass.Sales_Order
        Dim AP_PurchaseOrder As SyncMainClass.Purchase_Order
        Dim FI_JournalEntry As SyncMainClass.Direct_Expense
        Dim oPayment As SyncMainClass.IncomingPayment
        Dim APCreditMemo As SyncMainClass.APCreditMemo
        If isProcessExists(System.Diagnostics.Process.GetCurrentProcess.ProcessName()) = True Then
            Return

        End If
        WriteDebug("Sub Main")
        chk_TempPath()
        del_TempPath()
        'Karrson: Set Log Path
        _LogPath = get_LogPath()
        WriteDebug(_LogPath)
        oCompanies = Connects()
        currentCompany = Nothing

        'Get the inputted Parameter (Format: <Object Type>)
        _parameter = ""
        If Environment.GetCommandLineArgs.Length = 2 Then
            _parameter = Environment.GetCommandLineArgs.GetValue(1)
        Else
            _parameter = ""
        End If
        _parameter = _parameter.Trim
        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Parameter: " & _parameter, TimeSet.Status.Start)

        If Not oCompanies Is Nothing Then

            For Each oCompany As Company In oCompanies.CompanyList

                WriteDebug("Company DB: " & oCompany.CompanyDB)
                currentCompany = oCompany
                CPS.DI.Console.Company.setCurrentCompany(oCompany)

                commonRecordSet = Nothing
                commonRecordSet = New SQL.RecordSet()

                'Process Supplier Payment Cert and new connection with Flex accounting
                If _parameter = "18" Or _parameter = "18I" Or _parameter = "" Then
                    TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Supplier Payment Cert", TimeSet.Status.Start)
                    AP_AppInvoice = New SyncMainClass.A_ItmInvoice(oCompany)
                    AP_AppInvoice.Export()
                    AP_AppInvoice.Import()
                    'Karrson: Log Path
                    AP_AppInvoice.oLogMessage.FilePath = _LogPath
                    WriteDebug("Export Log To Text Files")
                    AP_AppInvoice.oLogMessage.ExportToText()
                    AP_AppInvoice.oFlexConnection.Disconnect()

                    TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Supplier Payment Cert", TimeSet.Status.Finish)
                End If

                'Process Sub Con Payment Cert and new connection with Flex accounting
                If _parameter = "18" Or _parameter = "18S" Or _parameter = "" Then
                    TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Sub-Contract Payment Cert", TimeSet.Status.Start)
                    AP_SrvInvoice = New SyncMainClass.P_SrvInvoice(oCompany)

                    AP_SrvInvoice.Export()

                    AP_SrvInvoice.Import()

                    'Karrson: Log Path

                    AP_SrvInvoice.oLogMessage.FilePath = _LogPath
                    TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Export Log", TimeSet.Status.Start)
                    AP_SrvInvoice.oLogMessage.ExportToText()
                    TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Export Log", TimeSet.Status.Finish)
                    AP_SrvInvoice.oFlexConnection.Disconnect()
                    TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Sub-Contract Payment Cert", TimeSet.Status.Finish)
                End If

                'Process Client Payment Cert and new connection with Flex accounting
                If _parameter = "13" Or _parameter = "" Then
                    AR_ItmInvoice = New SyncMainClass.S_Invoice(oCompany)
                    AR_ItmInvoice.Export()
                    AR_ItmInvoice.Import()
                    'Karrson: Log Path
                    AR_ItmInvoice.oLogMessage.FilePath = _LogPath

                    AR_ItmInvoice.oLogMessage.ExportToText()
                    AR_ItmInvoice.oFlexConnection.Disconnect()
                End If

                'Process Sales Quotation
                If _parameter = "23" Or _parameter = "" Then
                    WriteDebug("Project BQ:Define")
                    AR_SalesQuotation = New SyncMainClass.Sales_Quotation(oCompany)
                    WriteDebug("Project BQ:Export")
                    AR_SalesQuotation.Export()
                    WriteDebug("Project BQ:Import")
                    AR_SalesQuotation.Import()
                    'Karrson: Log Path
                    AR_SalesQuotation.oLogMessage.FilePath = _LogPath

                    WriteDebug("Project BQ:ExporttoText")
                    AR_SalesQuotation.oLogMessage.ExportToText()
                    WriteDebug("Project BQ:Finished")
                End If

                'Process Sales Order
                If _parameter = "17" Or _parameter = "" Then
                    AR_SalesOrder = New SyncMainClass.Sales_Order(oCompany)
                    AR_SalesOrder.Export()
                    AR_SalesOrder.Import()
                    'Karrson: Log Path
                    AR_SalesOrder.oLogMessage.FilePath = _LogPath
                    AR_SalesOrder.oLogMessage.ExportToText()
                End If

                'Process Purchase Order
                If _parameter = "22" Or _parameter = "" Then
                    AP_PurchaseOrder = New SyncMainClass.Purchase_Order(oCompany)
                    AP_PurchaseOrder.Export()
                    AP_PurchaseOrder.Import()
                    'Karrson: Log Path
                    AP_PurchaseOrder.oLogMessage.FilePath = _LogPath

                    AP_PurchaseOrder.oLogMessage.ExportToText()
                End If

                'Create Journal Entry in SAP
                If _parameter = "30" Or _parameter = "" Then
                    FI_JournalEntry = New SyncMainClass.Direct_Expense(oCompany)
                    FI_JournalEntry.Import()
                    'Karrson: Log Path
                    FI_JournalEntry.oLogMessage.FilePath = _LogPath

                    FI_JournalEntry.oLogMessage.ExportToText()
                    FI_JournalEntry.oFlexConnection.Disconnect()
                End If

                'Process Business Partner 
                If _parameter = "2" Or _parameter = "" Then
                    Master_BP = New SyncMainClass.BusinessPartners(oCompany)
                    Master_BP.Export()
                    Master_BP.Import()
                    'Karrson: Log Path
                    Master_BP.oLogMessage.FilePath = _LogPath

                    Master_BP.oLogMessage.ExportToText()
                    Master_BP.oFlexConnection.Disconnect()
                End If

                'Process Project Code
                If _parameter = "999" Or _parameter = "" Then
                    Master_PJ = New SyncMainClass.Project_Code(oCompany)
                    Master_PJ.Import()
                    'Karrson: Log Path
                    Master_PJ.oLogMessage.FilePath = _LogPath

                    Master_PJ.oLogMessage.ExportToText()
                    Master_PJ.oFlexConnection.Disconnect()
                End If

                'Payment from Flex system 
                If _parameter = "24" Or _parameter = "" Then
                    oPayment = New SyncMainClass.IncomingPayment(oCompany)
                    oPayment.Import()
                    'Karrson: Log Path
                    oPayment.oLogMessage.FilePath = _LogPath

                    oPayment.oLogMessage.ExportToText()
                    oPayment.oFlexConnection.Disconnect()
                End If

                'Process AP Credit Memo
                If _parameter = "19" Or _parameter = "" Then
                    TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",AP Credit Memo", TimeSet.Status.Start)
                    APCreditMemo = New SyncMainClass.APCreditMemo(oCompany)
                    APCreditMemo.Export()
                    APCreditMemo.Import()
                    'Karrson: Log Path
                    APCreditMemo.oLogMessage.FilePath = _LogPath
                    WriteDebug("Export Log To Text Files")
                    APCreditMemo.oLogMessage.ExportToText()
                    APCreditMemo.oFlexConnection.Disconnect()

                    TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",AP Credit Memo", TimeSet.Status.Finish)
                End If

            Next
        Else
            WriteDebug("Company Cannot Connect")

        End If

        TimeSet.Log(System.Reflection.MethodBase.GetCurrentMethod().ReflectedType.FullName & ",Parameter: " & _parameter, TimeSet.Status.Finish)
        TimeSet.Finish()

    End Sub

    'At 29th June 2009
    'By Jesse
    'For testing the outstanding 

    Public Function FLEX_DBName() As String
        Dim oFlex As FlexConnection
        Dim oDBName As String

        oFlex = New FlexConnection(False)
        oDBName = oFlex.DBName

        Return oDBName
    End Function

    Function Connects() As CPS.DI.Console.Company
        Dim oCompanies As CPS.DI.Console.Company

        oCompanies = New CPS.DI.Console.Company(TotalConnects)
        If oCompanies.Connects = 0 Then
            Return oCompanies
        Else
            Return Nothing
        End If
    End Function

    Sub chk_TempPath()
        Dim oDirectionaryInfo As IO.DirectoryInfo
        Dim strPath As String

        strPath = System.Reflection.Assembly.GetExecutingAssembly.Location
        strPath = Left(strPath, InStrRev(strPath, "\"))

        oDirectionaryInfo = New IO.DirectoryInfo(strPath & "TEMP")
        If Not oDirectionaryInfo.Exists Then
            oDirectionaryInfo.Create()
        End If
    End Sub

    Sub del_TempPath()
        Dim oDirectionaryInfo As IO.DirectoryInfo
        Dim oFileInfo As IO.FileInfo
        Dim strPath As String

        strPath = System.Reflection.Assembly.GetExecutingAssembly.Location
        strPath = Left(strPath, InStrRev(strPath, "\"))

        oDirectionaryInfo = New IO.DirectoryInfo(strPath & "TEMP")

        If oDirectionaryInfo.Exists Then
            For Each oFileInfo In oDirectionaryInfo.GetFiles
                Try
                    oFileInfo.Delete()
                Catch ex As Exception
                End Try
            Next
        End If
    End Sub

    Function get_TempPath() As String
        Dim strPath As String

        strPath = System.Reflection.Assembly.GetExecutingAssembly.Location
        strPath = Left(strPath, InStrRev(strPath, "\"))

        Return strPath & "TEMP"
    End Function
    ' Karrson: Check Log Path
    Function get_LogPath() As String
        Dim strPath As String = ""
        strPath = System.Reflection.Assembly.GetExecutingAssembly.Location
        strPath = Left(strPath, InStrRev(strPath, "\"))

        Dim strlogpath As String = ""
        strlogpath = Setting_Path.INIValue("Message Log Information", "LOG PATH", strPath & "Company.ini")
        If strlogpath = String.Empty Then
            strlogpath = strPath '& "Log"
        End If
        'strlogpath = strlogpath & String.Format("\{0}\", DateTime.Now.ToString("yyyy-MM-dd"))
        'Create Log Path
        Dim _di As New System.IO.DirectoryInfo(strlogpath)
        Try
            If _di.Exists = False Then
                _di.Create()
            End If
        Catch ex As Exception

        End Try

        Return strlogpath
    End Function
    Enum TransType As Integer
        tt_None = 0
        tt_BusinessPartner = 1
        tt_ItemMaster = 2
        tt_AR_Invoice = 3
        tt_AP_Invoice = 4
        tt_AR_CreditM = 5
        tt_AP_CreditM = 6
        tt_Payment = 7
    End Enum

    Enum Direction As Integer
        dt_NONE = 0
        dt_IN = 1
        dt_OUT = 2
    End Enum

    Public Enum ObjectType As Integer
        dt_ARInvoice = 13
        dt_APInvoice = 18
        mt_BusinessPartners = 2
        mt_ItemMaster = 4
    End Enum

    Public Enum DocumentType As Integer
        dt_ItemType = 0
        dt_Service = 1
    End Enum

    'When Success
    'oLogMessage.AddSuccessLine("Success Key", "Success Description")

    'When Failure
    'oLogMessage.AddFailureLine("Failure Key", "Failure Description")

    'When Exceiption
    'oLogMessage.AddExceptionSkip("Check Point", "Check Point Desc")

End Module
