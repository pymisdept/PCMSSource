
Imports SAPbobsCOM

Namespace Datatable.SAP
    Public Class Sync_History
        Inherits CPS.SQL.Interface.RecordSet

        Private ReadOnly CheckPoint As String = "Sync_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

        Public Const TableName As String = "CPSFSP"
        Public Const PK_Index As String = "JobIndex"
        Public Const DocEntry As String = "DocEntry"
        Public Const ObjType As String = "ObjType"
        Public Const CreateUID As String = "CreateUID"
        Public Const CreateTime As String = "CreateTime"
        Public Const ReasonCode As String = "ReasonCode"
        'Karrson: New Field
        'Public Const TableID As String = "TableID"

        Public Shadows Const Status As String = "Status"

        Public Sub New()
            MyBase.New(TableName)
            Me.add(PK_Index)
            Me.add(DocEntry)
            Me.add(ObjType)
            Me.add(CreateUID)
            Me.add(CreateTime, Format(Now, "yyyyMMdd HH:mm:ss"))
            Me.add(ReasonCode)
            Me.add(Status)
            'Karrson: New Field
            'Me.add(TableID)
        End Sub

        Public Function keyIndex() As String
            Dim sqlStr As String = "Select Top 1 IsNull(" & PK_Index & ", 0) " & PK_Index & " From [" & TableName & "] Order By " & PK_Index & " Desc"
            Dim oRecSet As Recordset

            oRecSet = commonRecordSet.execute(sqlStr)
            If oRecSet.RecordCount = 0 Then
                Return 1
            Else
                Return oRecSet.Fields.Item(PK_Index).Value + 1
            End If
        End Function

#Region "Document Log History"

        Public Sub Add_Submitted(ByVal pBatchID As String, _
                                 Optional ByVal isApproval As Boolean = False)
            Dim oHistory As Datatable.SAP.Sync_History
            Dim DocumentEntry As Integer = Right(pBatchID, pBatchID.Length - 1)
            Dim DocumentType As String = Left(pBatchID, 1)
            Dim oObjType As Integer
            Dim TableID As String
            Select Case DocumentType
                Case "C"
                    oObjType = 13

                Case "S"
                    oObjType = 18
                Case "M"

                    oObjType = 112
                Case "R"

                    oObjType = 112

            End Select

            oHistory = New Datatable.SAP.Sync_History
            oHistory.IndexKey = oHistory.keyIndex
            oHistory.DocumentKey = DocumentEntry
            oHistory.ObjectType = oObjType
            oHistory.CreateUser = SubMain.currentCompany.UserName
            oHistory.DocumentType = DocumentType & oObjType
            oHistory.JobStatus = "PPPS"
            'Karrson: Add Field
            'Select Case oObjType
            '    Case 13
            '        oHistory.Table_ID = "OINV"
            '    Case 18
            '        oHistory.Table_ID = "OPCH"
            '    Case 22
            '        oHistory.Table_ID = "OPOR"

            'End Select
            oHistory.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
        End Sub

        Public Sub Add_Acknowledge(ByVal pBatchID As String)
            Dim oHistory As Datatable.SAP.Sync_History
            Dim DocumentEntry As Integer = Right(pBatchID, pBatchID.Length - 1)
            Dim DocumentType As String = Left(pBatchID, 1)
            Dim oObjType As Integer

            Select Case DocumentType
                Case "C"
                    oObjType = 13
                Case "S"
                    oObjType = 18
                Case "M"
                    'Karrson: Change to 18
                    oObjType = 112
                    'oObjType = 18
                Case "R"
                    oObjType = 112
            End Select

            oHistory = New Datatable.SAP.Sync_History
            oHistory.IndexKey = oHistory.keyIndex
            oHistory.DocumentKey = DocumentEntry
            oHistory.ObjectType = oObjType
            oHistory.CreateUser = SubMain.currentCompany.UserName
            oHistory.DocumentType = DocumentType & oObjType
            oHistory.JobStatus = "PPFA"
            'Karrson: Add Field
            'Select Case oObjType
            '    Case 13
            '        oHistory.Table_ID = "OINV"
            '    Case 18
            '        oHistory.Table_ID = "OPCH"
            '    Case 22
            '        oHistory.Table_ID = "OPOR"

            'End Select
            oHistory.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
        End Sub

        Public Sub Add_Rejected(ByVal pBatchID As String)
            Dim oHistory As Datatable.SAP.Sync_History
            Dim DocumentEntry As Integer = Right(pBatchID, pBatchID.Length - 1)
            Dim DocumentType As String = Left(pBatchID, 1)
            Dim oObjType As Integer

            Select Case DocumentType
                Case "C"
                    oObjType = 13
                Case "S"
                    oObjType = 18
                Case "M"
                    oObjType = 18
                Case "R"
                    oObjType = 19
            End Select

            oHistory = New Datatable.SAP.Sync_History
            oHistory.IndexKey = oHistory.keyIndex
            oHistory.DocumentKey = DocumentEntry
            oHistory.ObjectType = oObjType
            oHistory.CreateUser = SubMain.currentCompany.UserName
            oHistory.DocumentType = DocumentType & oObjType
            oHistory.JobStatus = "PRDR"
            'Karrson: Add Field
            'Select Case oObjType
            '    Case 13
            '        oHistory.Table_ID = "OINV"
            '    Case 18
            '        oHistory.Table_ID = "OPCH"
            '    Case 22
            '        oHistory.Table_ID = "OPOR"

            'End Select
            oHistory.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
        End Sub

        Public Sub Add_Deleted(ByVal pBatchID As String)
            Dim oHistory As Datatable.SAP.Sync_History
            Dim DocumentEntry As Integer = Right(pBatchID, pBatchID.Length - 1)
            Dim DocumentType As String = Left(pBatchID, 1)
            Dim oObjType As Integer

            Select Case DocumentType
                Case "C"
                    oObjType = 13
                Case "S"
                    oObjType = 18
                Case "M"
                    oObjType = 18
                Case "R"
                    oObjType = 19
            End Select

            oHistory = New Datatable.SAP.Sync_History
            oHistory.IndexKey = oHistory.keyIndex
            oHistory.DocumentKey = DocumentEntry
            oHistory.ObjectType = oObjType
            oHistory.CreateUser = SubMain.currentCompany.UserName
            oHistory.DocumentType = DocumentType & oObjType
            oHistory.JobStatus = "PRBD"
            'Karrson: Add Field
            'Select Case oObjType
            '    Case 13
            '        oHistory.Table_ID = "OINV"
            '    Case 18
            '        oHistory.Table_ID = "OPCH"
            '    Case 22
            '        oHistory.Table_ID = "OPOR"

            'End Select
            oHistory.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
        End Sub

        Public Sub Add_Posted(ByVal pBatchID As String)
            Dim oHistory As Datatable.SAP.Sync_History
            Dim DocumentEntry As Integer = Right(pBatchID, pBatchID.Length - 1)
            Dim DocumentType As String = Left(pBatchID, 1)
            Dim oObjType As Integer

            'Modify by michael, begin, 20140115
            Select Case DocumentType
                Case "Q"            'Quotation
                    oObjType = 23
                Case "O"            'Sales Order
                    oObjType = 17
                Case "P"            'Purchase Order
                    oObjType = 22
                Case "D"            'Draft
                    oObjType = 112
                    'oObjType = 22
                Case "C"            'Client Payment Cert
                    oObjType = 13
                Case "S"            'Service A/P Invoice
                    oObjType = 18
                Case "M"            'Item A/P Invoice
                    oObjType = 18
                Case "R"            'AP Credit Memo
                    oObjType = 19

                Case "D"            'Reversed Client Payment Cert
                    oObjType = 13
                Case "T"            'Reversed Service A/P Invoice
                    oObjType = 18
            End Select
            'Modify by michael, end, 20140115

            oHistory = New Datatable.SAP.Sync_History
            oHistory.IndexKey = oHistory.keyIndex
            oHistory.DocumentKey = DocumentEntry
            oHistory.ObjectType = oObjType
            oHistory.CreateUser = SubMain.currentCompany.UserName
            'Modify by michael, begin, 20140115
            If DocumentType = "D" Then
                oHistory.DocumentType = "C" & oObjType
            Else
                If DocumentType = "T" Then
                    oHistory.DocumentType = "S" & oObjType
                Else
                    oHistory.DocumentType = DocumentType & oObjType
                End If
            End If
            'Modify by michael, begin, 20140115
            oHistory.JobStatus = "P"
            'Karrson: Add Field
            'Select Case oObjType
            '    Case 13
            '        oHistory.Table_ID = "OINV"
            '    Case 18
            '        oHistory.Table_ID = "OPCH"
            '    Case 22
            '        oHistory.Table_ID = "OPOR"

            'End Select
            oHistory.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
        End Sub

#End Region

#Region "BP Log History"
        Public Sub Add_CreateBP(ByVal pCardCode As String)
            Dim oHistory As Datatable.SAP.Sync_History
            Dim oObjType As Integer = 2

            oHistory = New Datatable.SAP.Sync_History
            oHistory.IndexKey = oHistory.keyIndex
            oHistory.DocumentKey = 0
            oHistory.ObjectType = oObjType
            oHistory.CreateUser = SubMain.currentCompany.UserName
            oHistory.DocumentType = pCardCode
            oHistory.JobStatus = "P"
            oHistory.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
        End Sub

        Public Sub Add_UpdateBP(ByVal pCardCode As String)
            Dim oHistory As Datatable.SAP.Sync_History
            Dim oObjType As Integer = 2

            oHistory = New Datatable.SAP.Sync_History
            oHistory.IndexKey = oHistory.keyIndex
            oHistory.DocumentKey = 0
            oHistory.ObjectType = oObjType
            oHistory.CreateUser = SubMain.currentCompany.UserName
            oHistory.DocumentType = pCardCode
            oHistory.JobStatus = "A"
            oHistory.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
        End Sub

        Public Sub Add_FrozenBP(ByVal pCardCode As String)
            Dim oHistory As Datatable.SAP.Sync_History
            Dim oObjType As Integer = 2

            oHistory = New Datatable.SAP.Sync_History
            oHistory.IndexKey = oHistory.keyIndex
            oHistory.DocumentKey = 0
            oHistory.ObjectType = oObjType
            oHistory.CreateUser = SubMain.currentCompany.UserName
            oHistory.DocumentType = pCardCode
            oHistory.JobStatus = "F"
            oHistory.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
        End Sub
#End Region

#Region "Journal Entry Log History"
        Public Sub Add_CreatejE(ByVal pTransID As String)
            Dim oHistory As Datatable.SAP.Sync_History

            oHistory = New Datatable.SAP.Sync_History
            oHistory.IndexKey = oHistory.keyIndex
            oHistory.DocumentKey = pTransID
            oHistory.ObjectType = "30"
            oHistory.CreateUser = SubMain.currentCompany.UserName
            oHistory.DocumentType = ""
            oHistory.JobStatus = "PRBD"
            oHistory.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
        End Sub
#End Region

#Region "Project Log History"
        Public Sub Add_CreatePJ(ByVal pPrjCode As String)
            Dim oHistory As Datatable.SAP.Sync_History
            Dim oObjType As Integer = 999

            oHistory = New Datatable.SAP.Sync_History
            oHistory.IndexKey = oHistory.keyIndex
            oHistory.DocumentKey = 0
            oHistory.ObjectType = oObjType
            oHistory.CreateUser = SubMain.currentCompany.UserName
            oHistory.DocumentType = pPrjCode
            oHistory.JobStatus = "P"
            oHistory.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
        End Sub

        Public Sub Add_UpdatePJ(ByVal pPrjCode As String)
            Dim oHistory As Datatable.SAP.Sync_History
            Dim oObjType As Integer = 999

            oHistory = New Datatable.SAP.Sync_History
            oHistory.IndexKey = oHistory.keyIndex
            oHistory.DocumentKey = 0
            oHistory.ObjectType = oObjType
            oHistory.CreateUser = SubMain.currentCompany.UserName
            oHistory.DocumentType = pPrjCode
            oHistory.JobStatus = "A"
            oHistory.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
        End Sub

        Public Sub Add_FrozenPJ(ByVal pPrjCode As String)
            Dim oHistory As Datatable.SAP.Sync_History
            Dim oObjType As Integer = 999

            oHistory = New Datatable.SAP.Sync_History
            oHistory.IndexKey = oHistory.keyIndex
            oHistory.DocumentKey = 0
            oHistory.ObjectType = oObjType
            oHistory.CreateUser = SubMain.currentCompany.UserName
            oHistory.DocumentType = pPrjCode
            oHistory.JobStatus = "F"
            oHistory.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
        End Sub
#End Region

#Region "Payment Log History"
        Public Sub Add_CreatePY(ByVal pDocNum As String)
            Dim oHistory As Datatable.SAP.Sync_History
            Dim oObjType As Integer = 24

            oHistory = New Datatable.SAP.Sync_History
            oHistory.IndexKey = oHistory.keyIndex
            oHistory.DocumentKey = 0
            oHistory.ObjectType = oObjType
            oHistory.CreateUser = SubMain.currentCompany.UserName
            oHistory.DocumentType = pDocNum
            oHistory.JobStatus = "P"
            oHistory.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
        End Sub
        Public Sub Add_UpdatePY(ByVal pDocNum As String)
            Dim oHistory As Datatable.SAP.Sync_History
            Dim oObjType As Integer = 24

            oHistory = New Datatable.SAP.Sync_History
            oHistory.IndexKey = oHistory.keyIndex
            oHistory.DocumentKey = 0
            oHistory.ObjectType = oObjType
            oHistory.CreateUser = SubMain.currentCompany.UserName
            oHistory.DocumentType = pDocNum
            oHistory.JobStatus = "A"
            oHistory.Process(CPS.SQL.Interface.RecordSet.Status.stt_INSERT)
        End Sub
#End Region

#Region "Assign Value"
        Public WriteOnly Property IndexKey() As String
            Set(ByVal value As String)
                If Not Me.modify(PK_Index, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Job Index Key (PK)]")
                End If
            End Set
        End Property
        Public WriteOnly Property DocumentKey() As String
            Set(ByVal value As String)
                If Not Me.modify(DocEntry, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Document Entry]")
                End If
            End Set
        End Property
        Public WriteOnly Property ObjectType() As String
            Set(ByVal value As String)
                If Not Me.modify(ObjType, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Object Type]")
                End If
            End Set
        End Property
        'Karrson: New Field
        'Public WriteOnly Property Table_ID() As String
        '    Set(ByVal value As String)
        '        If Not Me.modify(TableID, value) Then
        '            Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Object Type]")
        '        End If
        '    End Set
        'End Property

        Public WriteOnly Property CreateUser() As String
            Set(ByVal value As String)
                If Not Me.modify(CreateUID, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & " [Create UID]")
                End If
            End Set
        End Property
        Public WriteOnly Property DocumentType() As String
            Set(ByVal value As String)
                If Not Me.modify(ReasonCode, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document Type]")
                End If
            End Set
        End Property
        Public WriteOnly Property JobStatus()
            Set(ByVal value)
                If Not Me.modify(Status, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Job Status (S / A)]")
                End If
            End Set
        End Property
#End Region

    End Class
End Namespace