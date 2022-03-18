#Region "Imports"
Imports SAPbobsCOM
#End Region

Namespace [Global].Setting
    Public Class LogMessage

#Region "Declare variable"
        Private _Company As SAPbobsCOM.Company
        Private _JobName As String, _Arguments As String
        Private _ProcessLists, _SuccessLists, _FailureLists, _SkipLists As DataStructure.LogMessage()
        Private _FileName As String, _FilePath As String
#End Region

#Region "Constructor"

        ''' <summary>
        ''' Log Message Standard format
        ''' </summary>
        ''' <param name="pCompany"></param>
        ''' <remarks>Project Property --> Debug --> Command line Arguments</remarks>
        Public Sub New(ByVal pCompany As SAPbobsCOM.Company)
            _Company = pCompany
            _SuccessLists = Nothing
            _FailureLists = Nothing
            _SkipLists = Nothing
            _ProcessLists = Nothing
            _FilePath = ""
        End Sub

        Public Sub New(ByVal pCompany As DI.Interface.Connection)
            _Company = New SAPbobsCOM.Company
            _Company.CompanyDB = pCompany.DBName
            _Company.Server = pCompany.Server
            _Company.UserName = pCompany.DBUserID
        End Sub

#End Region

        ''' <summary>
        ''' Define the File name, return "Process" if file name is blank
        ''' </summary>
        ''' <value></value>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        Public Property FilePath() As String
            Get
                Return _FilePath
            End Get
            Set(ByVal value As String)
                If System.IO.Directory.Exists(value) Then
                    _FilePath = value
                Else
                    Throw New IO.DirectoryNotFoundException("Folder[" & value & "] isn't exists")
                End If
            End Set
        End Property

        Public Property FileName() As String
            Get
                Return _FileName
            End Get
            Set(ByVal value As String)
                _FileName = value
            End Set
        End Property

        Public Property JobName() As String
            Get
                Return _JobName
            End Get
            Set(ByVal value As String)
                _JobName = value
            End Set
        End Property

        ''' <summary>
        ''' the execution file of pass in parameter
        ''' </summary>
        ''' <value>String</value>
        ''' <returns></returns>
        ''' <remarks>Project Property --> Debug --> Command line Arguments</remarks>
        Public Property Argument() As String
            Get
                Return _Arguments
            End Get
            Set(ByVal value As String)
                _Arguments = value
            End Set
        End Property

        Public Function ExportFormat() As String
            Dim oReturnMessage As String
            Dim oLineMessage As String
            Dim oTotalProcess As Integer, oTotalSuccess As Integer, oTotalSkip As Integer, oTotalFailure As Integer

            oTotalProcess = 0
            oTotalSuccess = 0
            oTotalSkip = 0
            oTotalFailure = 0

            oReturnMessage = ""
            oReturnMessage &= LSet("", 67).Replace(" ", "*") & vbCrLf
            oReturnMessage &= "Job name: " & Me.JobName & vbCrLf
            oReturnMessage &= "Argument: " & Me.Argument & vbCrLf
            oReturnMessage &= "Start Time: " & Me.getSystemDate & vbCrLf

            If Not _Company Is Nothing Then
                oReturnMessage &= "Login to SAP Success, The following is the login information:" & vbCrLf
                oReturnMessage &= LSet("", 67).Replace(" ", "-") & vbCrLf
                oReturnMessage &= "SAP Database: " & _Company.CompanyDB & vbCrLf
                oReturnMessage &= "Login Server: " & _Company.Server & vbCrLf
                oReturnMessage &= "Login User  : " & _Company.UserName & vbCrLf
                oReturnMessage &= LSet("", 67).Replace(" ", "-") & vbCrLf
            End If

            oReturnMessage &= "" & vbCrLf

            'Total Batch of Result
            If Not _ProcessLists Is Nothing Then
                oTotalProcess = 0

                For Each oProcessList As DataStructure.LogMessage In _ProcessLists
                    If oProcessList.Status = DataStructure.LogMessage.TransType.tt_Description Then
                        oLineMessage = ""
                        oLineMessage &= vbCrLf & oProcessList.KeyValue & " " & oProcessList.Description & vbCrLf
                    Else
                        oTotalProcess += 1

                        oLineMessage = ""
                        oLineMessage = Me.getSystemDate.Trim & ": Process Record: [" & oProcessList.KeyValue & "]"
                        Select Case oProcessList.Status
                            Case DataStructure.LogMessage.TransType.tt_Success
                                oLineMessage &= ", Success"
                            Case DataStructure.LogMessage.TransType.tt_Failure
                                oLineMessage &= ", Failure"
                            Case DataStructure.LogMessage.TransType.tt_Exception
                                oLineMessage &= ", Exception"
                        End Select
                    End If
                    oReturnMessage &= oLineMessage & vbCrLf
                Next
            Else
                oTotalProcess = 0
            End If

            'Total Success of Result
            If Not _SuccessLists Is Nothing Then
                oTotalSuccess = _SuccessLists.Length
            Else
                oTotalSuccess = 0
            End If

            oReturnMessage &= "" & vbCrLf

            'Total Failure of Result
            If Not _FailureLists Is Nothing Then
                oReturnMessage &= "Failure Record:" & vbCrLf
                oReturnMessage &= LSet("", 67).Replace(" ", "-") & vbCrLf

                For Each oFailureList As DataStructure.LogMessage In _FailureLists
                    oReturnMessage &= "Process Record: [" & oFailureList.KeyValue & "]" & vbCrLf
                    oReturnMessage &= "Error Reason: " & vbCrLf & oFailureList.Description & vbCrLf
                Next
                oReturnMessage &= LSet("", 67).Replace(" ", "-") & vbCrLf
                oTotalFailure = _FailureLists.Length
            Else
                oTotalFailure = 0
            End If

            oReturnMessage &= "" & vbCrLf

            'Total Skip of Result
            If Not _SkipLists Is Nothing Then
                oReturnMessage &= "Exception Skip Record:" & vbCrLf
                oReturnMessage &= LSet("", 67).Replace(" ", "-") & vbCrLf

                For Each oSkipList As DataStructure.LogMessage In _SkipLists
                    oReturnMessage &= "Process Record: [" & oSkipList.KeyValue & "]" & vbCrLf
                    oReturnMessage &= "Exception Detail: " & vbCrLf & oSkipList.Description & vbCrLf
                Next
                oReturnMessage &= LSet("", 67).Replace(" ", "-") & vbCrLf
                oTotalSkip = _SkipLists.Length
            Else
                oTotalSkip = 0
            End If

            oReturnMessage &= "" & vbCrLf

            oReturnMessage &= "No of records process: " & oTotalProcess & vbCrLf
            oReturnMessage &= "No of records success: " & oTotalSuccess & vbCrLf
            oReturnMessage &= "No of records skip: " & oTotalSkip & vbCrLf
            oReturnMessage &= "No of records fail: " & oTotalFailure & vbCrLf

            oReturnMessage &= "" & vbCrLf

            oReturnMessage &= "End Time: " & Me.getSystemDate & vbCrLf
            oReturnMessage &= "Job Complete " & IIf(oTotalFailure > 0 Or oTotalSkip > 0, "[Failure]", "[Success]") & vbCrLf

            oReturnMessage &= LSet("", 67).Replace(" ", "*") & vbCrLf

            Return oReturnMessage
        End Function

#Region "Increase Login Transaction"

        Public Sub AddSuccessLine(ByVal pKeyValue As String, ByVal pDescription As String)
            Dim oSuccessList As DataStructure.LogMessage

            'Create Log Message Content
            oSuccessList = New DataStructure.LogMessage
            oSuccessList.KeyValue = pKeyValue
            oSuccessList.Description = pDescription
            oSuccessList.Status = DataStructure.LogMessage.TransType.tt_Success

            'Import into Message Log List
            If _SuccessLists Is Nothing Then
                ReDim _SuccessLists(0)
            Else
                ReDim Preserve _SuccessLists(_SuccessLists.Length)
            End If
            _SuccessLists(_SuccessLists.Length - 1) = oSuccessList

            'Import into Message Log List
            If _ProcessLists Is Nothing Then
                ReDim _ProcessLists(0)
            Else
                ReDim Preserve _ProcessLists(_ProcessLists.Length)
            End If
            _ProcessLists(_ProcessLists.Length - 1) = oSuccessList
        End Sub

        Public Sub AddFailureLine(ByVal pKeyValue As String, ByVal pDescription As String)
            Dim oFailureList As DataStructure.LogMessage

            'Create Log Message Content
            oFailureList = New DataStructure.LogMessage
            oFailureList.KeyValue = pKeyValue
            oFailureList.Description = pDescription
            oFailureList.Status = DataStructure.LogMessage.TransType.tt_Failure

            'Import into Message Log List
            If _FailureLists Is Nothing Then
                ReDim _FailureLists(0)
            Else
                ReDim Preserve _FailureLists(_FailureLists.Length)
            End If
            _FailureLists(_FailureLists.Length - 1) = oFailureList

            'Import into Message Log List
            If _ProcessLists Is Nothing Then
                ReDim _ProcessLists(0)
            Else
                ReDim Preserve _ProcessLists(_ProcessLists.Length)
            End If
            _ProcessLists(_ProcessLists.Length - 1) = oFailureList
        End Sub

        Public Sub AddExceptionSkip(ByVal pKeyValue As String, ByVal pDescription As String)
            Dim oSkipList As DataStructure.LogMessage

            'Create Log Message Content
            oSkipList = New DataStructure.LogMessage
            oSkipList.KeyValue = pKeyValue
            oSkipList.Description = pDescription
            oSkipList.Status = DataStructure.LogMessage.TransType.tt_Exception

            'Import into Message Log List
            If _SkipLists Is Nothing Then
                ReDim _SkipLists(0)
            Else
                ReDim Preserve _SkipLists(_SkipLists.Length)
            End If
            _SkipLists(_SkipLists.Length - 1) = oSkipList

            'Import into Message Log List
            If _ProcessLists Is Nothing Then
                ReDim _ProcessLists(0)
            Else
                ReDim Preserve _ProcessLists(_ProcessLists.Length)
            End If
            _ProcessLists(_ProcessLists.Length - 1) = oSkipList
        End Sub

        Public Sub AddReferenceLine(ByVal pKeyValue As String, ByVal pDescription As String)
            Dim oDescription As DataStructure.LogMessage

            'Create Reference Description Content
            oDescription = New DataStructure.LogMessage
            oDescription.KeyValue = "[" & pKeyValue.Trim & "]"
            oDescription.Description = pDescription
            oDescription.Status = DataStructure.LogMessage.TransType.tt_Description

            'Import into Message Log List
            If _ProcessLists Is Nothing Then
                ReDim _ProcessLists(0)
            Else
                ReDim Preserve _ProcessLists(_ProcessLists.Length)
            End If
            _ProcessLists(_ProcessLists.Length - 1) = oDescription
        End Sub

#End Region

#Region "Export to Object"

        Public Function ExportToText() As Boolean
            CreateLog(_FilePath, _FileName)
            Common_Module.LogMessage(ExportFormat)
        End Function

        Public Function ExportToMail() As Boolean
            Throw New NotImplementedException
        End Function

        Public Function ExportToSAP(ByVal pRecipient() As String) As Boolean
            Throw New NotImplementedException
        End Function

#End Region

        Private Function getSystemDate() As String
            Return Format(Now, "u")
        End Function

    End Class
End Namespace