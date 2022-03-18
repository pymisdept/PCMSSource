Imports CPS.SQL.Condition

Namespace Datatable.SAP.PCMS
    Public Class CPSFSE
        Inherits CPS.SQL.Interface.RecordSet

        Private mDraftKey As String, mObjType As String
        Private ReadOnly CheckPoint As String = "Sync_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

        Public Sub New()
            MyBase.New(TableName)
            Me.add(_DraftKey)
            Me.add(_ObjType)
            Me.add(_LogInstanc)
            Me.add(_CreateDate, Now)
            Me.add(_ErrorCode)
            Me.add(_ErrorDesc)
            Me.add(_Exception)
        End Sub

        Public Function keyIndex() As String
            Dim oRecSet As SAPbobsCOM.Recordset
            Dim sqlStr As String

            If mObjType = "" Or mDraftKey = "" Then
                Throw New BaseException(BaseException.ErrorType.Normal, "KEYINDEX", "Draft Key Equal [0] or Object Type is Null")
            End If

            sqlStr = "SELECT 1 FROM " & TableName & " WHERE " & _DraftKey & " = '" & mDraftKey & "' AND " & _ObjType & " = '" & mObjType & "'"
            oRecSet = commonRecordSet.execute(sqlStr)

            If oRecSet.RecordCount = 0 Then
                Return 1
            Else
                Return oRecSet.RecordCount + 1
            End If

        End Function

#Region "Constanst Value"
        Public Const TableName As String = "CPSFSE" 'Table Name
        Public Const _DraftKey As String = "DraftKey" 'Draft Key
        Public Const _ObjType As String = "ObjType"  'Object Type
        Public Const _LogInstanc As String = "LogInstanc"   'Log Instance
        Public Const _CreateDate As String = "CreateDate"   'Transaction Create Date
        Public Const _ErrorCode As String = "ErrorCode"    'Check Point Code
        Public Const _ErrorDesc As String = "ErrorDesc"    'Check Point Descript
        Public Const _Exception As String = "Exception"    'SAP / VB Return Error Description
#End Region

#Region "Assign Value"
        Public WriteOnly Property DraftKey() As String
            Set(ByVal value As String)
                If Not Me.Modify(_DraftKey, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Draft Key]")
                End If
                mDraftKey = value
            End Set
        End Property
        Public WriteOnly Property ObjType() As String
            Set(ByVal value As String)
                If Not Me.Modify(_ObjType, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Object Type]")
                End If
                mObjType = value.Trim
            End Set
        End Property
        Public WriteOnly Property LogInstanc() As String
            Set(ByVal value As String)
                If Not Me.Modify(_LogInstanc, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Log Instance]")
                End If
            End Set
        End Property
        Public WriteOnly Property CreateDate() As String
            Set(ByVal value As String)
                If Not Me.Modify(_CreateDate, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Transaction Create Date]")
                End If
            End Set
        End Property
        Public WriteOnly Property ErrorCode() As String
            Set(ByVal value As String)
                If Not Me.Modify(_ErrorCode, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Check Point Code]")
                End If
            End Set
        End Property
        Public WriteOnly Property ErrorDesc() As String
            Set(ByVal value As String)
                If Not Me.Modify(_ErrorDesc, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Check Point Descript]")
                End If
            End Set
        End Property
        Public WriteOnly Property Exception() As String
            Set(ByVal value As String)
                If Not Me.Modify(_Exception, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[SAP / VB Return Error Description]")
                End If
            End Set
        End Property
#End Region

    End Class
End Namespace

