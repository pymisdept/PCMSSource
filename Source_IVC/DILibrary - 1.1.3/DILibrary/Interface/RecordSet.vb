Namespace SQL.Interface
    Public MustInherit Class RecordSet

        Public Sub New(ByVal pTableName As String, Optional ByVal pDBName As String = "")
            _TableColumn = New Collection
            _TableValue = New Collection
            _OrderBy = New Collection
            _Filter = New Collection
            _AdvanceFilter = ""
            _TableName = pTableName
            _DatabaseName = pDBName
            _Limit = 0
        End Sub

#Region "Declare Variable:"
        Private _TableColumn As Collection
        Private _TableValue As Collection
        Private _Filter As Collection
        Private _OrderBy As Collection
        Private _AdvanceFilter As String
        Private _TableName, _DatabaseName As String
        Private ReadOnly _SpecInedx() As String = {"%", "~", "`", "!", "@", "#", "$", "^", ";", _
                                                   "&", "(", ")", "-", "_", "+", "=", "{", "}", _
                                                   "[", "]", "<", ">", "?", "/", "|", "\"}
        Private ReadOnly _AdvIndex() As String = {"'"}
        Private _Limit As Integer
#End Region

#Region "General Setting"
        Public ReadOnly Property Database() As String
            Get
                Return _DatabaseName
            End Get
        End Property
        Public ReadOnly Property Table() As String
            Get
                Return _TableName
            End Get
        End Property
#End Region

#Region "Get Value List"

        Public Function getColumnList() As Collection
            Return Me._TableColumn
        End Function

        'Public Function getTableList() As List(Of TableStructure)
        '    Dim oTSs As List(Of TableStructure)
        '    Dim oTS As TableStructure

        '    oTSs = New List(Of TableStructure)
        '    For i As Integer = 1 To _TableColumn.Count
        '        oTS = New TableStructure
        '        oTS.Column = _TableColumn.Item(i)
        '        oTS.Value = _TableValue.Item(i)

        '        oTSs.Add(oTS)
        '    Next

        '    Return oTSs
        'End Function

#End Region

#Region "Adjust table column"
        Public Function add(ByVal pColumnID As String, Optional ByVal pColumnValue As Object = Nothing) As Boolean
            'if column id is exist then return false
            'other wise                 return true
            If _TableColumn.Contains(pColumnID) Then
                Return False
            End If

            If _TableValue.Contains(pColumnID) Then
                _TableValue.Remove(pColumnID)
            End If

            _TableColumn.Add(pColumnID, pColumnID)
            _TableValue.Add(pColumnValue, pColumnID)

            Return True
        End Function

        Public Function modify(ByVal pColumnID As String, ByVal pColumnValue As Object) As Boolean
            'if column id is exist then return true
            'other wise                 return false
            If Not _TableColumn.Contains(pColumnID) Then
                Return False
            End If

            If _TableColumn.Contains(pColumnID) Then
                _TableColumn.Remove(pColumnID)
            End If

            If _TableValue.Contains(pColumnID) Then
                _TableValue.Remove(pColumnID)
            End If

            _TableColumn.Add(pColumnID, pColumnID)
            _TableValue.Add(pColumnValue, pColumnID)

            Return True
        End Function

        '' Added by Ken, 20180806, begin

        Public Function limit(ByVal pLimit As Integer)

            If pLimit > 0 Then
                _Limit = pLimit
            Else
                _Limit = 0
            End If

            Return True

        End Function

        '' Added by Ken, 20180806, end
#End Region

#Region "Adjust Filter Information"
        Public Sub addFilter(ByVal pCondition As SQL.Condition.Condition)
            _Filter.Add(pCondition, _Filter.Count)
        End Sub

        Public Property itemFilter(ByVal pKey As Integer) As SQL.Condition.Condition
            Get
                Return _Filter.Item(pKey)
            End Get
            Set(ByVal value As SQL.Condition.Condition)
                _Filter.Remove(pKey)
                _Filter.Add(value, pKey)
            End Set
        End Property

        Public Sub clearFilter()
            _Filter.Clear()
            _Filter = New Collection
            _AdvanceFilter = ""
        End Sub

        Public Sub AdvanceFilter(ByVal pFilter As String)
            _AdvanceFilter = pFilter
        End Sub

#End Region

#Region "Adjust Ordered Criteria"
        Public Function Add_OrderBy(ByVal pColumnID As String) As Boolean
            If _OrderBy.Contains(pColumnID) Then
                Return False
            Else
                _OrderBy.Add(pColumnID)
            End If
        End Function

        Public Sub Clear_OrderBy()
            _OrderBy = New Collection
        End Sub
#End Region

#Region "Data Structure"
        Public Enum Status
            stt_INSERT
            stt_UPDATE
            stt_DELETE
        End Enum
        Public Structure TableStructure
            Dim Column As String
            Dim Value As Object
        End Structure
#End Region

#Region "Return Query String"

        Public Function SelectQuery() As String
            Dim sqlStr As String

            sqlStr = ""
            sqlStr &= " Select "

            If _Limit > 0 Then
                sqlStr &= " top " & _Limit
            End If

            For Each oColumn As String In _TableColumn
                sqlStr &= oColumn & ", "
            Next
            sqlStr = Left(sqlStr, sqlStr.Length - 2)
            sqlStr &= " From "
            If Not _DatabaseName = "" Then
                sqlStr &= "[" & _DatabaseName & "]."
            End If
            sqlStr &= "[dbo].[" & _TableName & "]"
            Return sqlStr
        End Function

        Public Function InsertQuery() As String
            Dim sqlStr As String = ""
            Dim sqlColumn As String, sqlValue As String
            Dim oDateTime As Date

            sqlColumn = ""
            sqlValue = ""

            For i As Integer = 1 To _TableColumn.Count
                If Not String.IsNullOrEmpty(_TableValue.Item(i)) Then
                    If _TableValue.Item(i).GetType.Equals(GetType(System.DateTime)) Then
                        oDateTime = CDate(_TableValue.Item(i))

                        If oDateTime > "1900 Jan 31" Then
                            sqlColumn &= _TableColumn.Item(i) & ", "
                            sqlValue &= "'" & Format(CDate(_TableValue.Item(i)), "yyyyMMdd HH:mm:ss") & "." & Right("000" & CStr(oDateTime.Millisecond).Trim, 3) & "', "
                        End If
                    Else
                        sqlColumn &= _TableColumn.Item(i) & ", "

                        'sqlValue &= "'" & AdvConvert(_TableValue.Item(i)).Trim & "', "
                        sqlValue &= "N'" & AdvConvert(_TableValue.Item(i)).Trim & "', "
                    End If

                End If
            Next

            sqlColumn = Left(sqlColumn, sqlColumn.Length - 2)
            sqlValue = Left(sqlValue, sqlValue.Length - 2)

            sqlStr &= " Insert Into "
            If Not _DatabaseName = "" Then
                sqlStr &= "[" & _DatabaseName & "]."
            End If
            sqlStr &= "[dbo].[" & _TableName & "]"
            sqlStr &= " ( "
            sqlStr &= sqlColumn
            sqlStr &= " ) Values ( "
            sqlStr &= sqlValue
            sqlStr &= " ) "

            Return sqlStr
        End Function

        Public Function UpdateQuery() As String
            Dim sqlStr As String = ""
            Dim oUpdateSript As String = ""

            sqlStr &= " Update "
            If Not _DatabaseName = "" Then
                sqlStr &= "[" & _DatabaseName & "]."
            End If
            sqlStr &= "[dbo].[" & _TableName & "]" & vbCrLf
            sqlStr &= " Set " & vbCrLf

            For i As Integer = 1 To _TableColumn.Count
                If Not _TableValue.Item(i) Is Nothing Then
                    If _TableValue.Item(i).GetType.Equals(GetType(System.String)) Then
                        If Not String.IsNullOrEmpty(_TableValue.Item(i)) Then
                            oUpdateSript &= _TableColumn.Item(i) & " = '" & AdvConvert(_TableValue.Item(i)) & "', " & vbCrLf
                        End If
                    ElseIf _TableValue.Item(i).GetType.Equals(GetType(System.DateTime)) Then
                        oUpdateSript &= _TableColumn.Item(i) & " = '" & Format(CDate(_TableValue.Item(i)), "yyyyMMdd") & "', " & vbCrLf
                    Else
                        oUpdateSript &= _TableColumn.Item(i) & " = '" & AdvConvert(_TableValue.Item(i)) & "', " & vbCrLf
                    End If
                End If
            Next

            oUpdateSript = Left(oUpdateSript.Trim, oUpdateSript.Trim.Length - 1)
            sqlStr &= oUpdateSript

            Return sqlStr
        End Function

        Public Function DeleteQuery() As String
            Dim sqlStr As String = ""

            sqlStr &= "Delete From "
            If Not _DatabaseName = "" Then
                sqlStr &= "[" & _DatabaseName & "]."
            End If
            sqlStr &= "[dbo].[" & _TableName & "]" & vbCrLf

            Return sqlStr
        End Function

#End Region

#Region "Return Filter String"
        Public Function filterQuery() As String
            Dim sqlStr As String = ""

            If _Filter.Count > 0 Then
                sqlStr = " WHERE " & vbCrLf

                For Each oCondition As SQL.Condition.Condition In _Filter
                    sqlStr &= oCondition.spRelation

                    sqlStr &= Replace(LSet("", oCondition.BracketOpenNum), " ", "(") & " "

                    sqlStr &= oCondition.Alias

                    sqlStr &= oCondition.spOperate

                    If oCondition.Operation = SQL.Condition.Condition.eOperation.op_BETWEEN Or _
                       oCondition.Operation = SQL.Condition.Condition.eOperation.op_NOT_BETWEEN Then
                        sqlStr &= "'" & StrConvert(oCondition.Value) & "' And '" & StrConvert(oCondition.ValueEnd) & "'"
                    ElseIf oCondition.Operation = SQL.Condition.Condition.eOperation.op_IS_NULL Or _
                           oCondition.Operation = SQL.Condition.Condition.eOperation.op_NOT_NULL Then
                        sqlStr &= ""
                    ElseIf oCondition.Operation = SQL.Condition.Condition.eOperation.op_CONTAIN Or _
                           oCondition.Operation = SQL.Condition.Condition.eOperation.op_START Or _
                           oCondition.Operation = SQL.Condition.Condition.eOperation.op_END Or _
                           oCondition.Operation = SQL.Condition.Condition.eOperation.op_NOT_CONTAIN Then
                        Select Case oCondition.Operation
                            Case SQL.Condition.Condition.eOperation.op_CONTAIN
                                sqlStr &= "'%" & StrConvert(oCondition.Value) & "%'"
                            Case SQL.Condition.Condition.eOperation.op_START
                                sqlStr &= "'" & StrConvert(oCondition.Value) & "%'"
                            Case SQL.Condition.Condition.eOperation.op_END
                                sqlStr &= "'%" & StrConvert(oCondition.Value) & "'"
                        End Select
                    Else
                        sqlStr &= "'" & StrConvert(oCondition.Value) & "'"
                    End If

                    sqlStr &= Replace(LSet("", oCondition.BracketCloseNum), " ", ")") & " "

                    sqlStr &= vbCrLf
                Next
            End If

            If sqlStr = "" Then
                sqlStr = " WHERE " & vbCrLf
                sqlStr &= _AdvanceFilter & vbCrLf
            Else
                sqlStr &= _AdvanceFilter & vbCrLf
            End If

            Return sqlStr
        End Function
#End Region

#Region "Return Order By string"
        Public Function OrderByQuery() As String
            Dim sqlStr As String = ""

            If _OrderBy.Count = 0 Then
                Return ""
            Else
                sqlStr = " Order By "
                For Each oColumn As String In _OrderBy
                    sqlStr &= oColumn & ", "
                Next
                sqlStr = Left(sqlStr, sqlStr.Length - 2)
                Return sqlStr
            End If
        End Function
#End Region

#Region "Process Query"
        Public Function Execute(ByVal _Query As String) As SAPbobsCOM.Recordset
            If Not DI.Console.Company.Company.Connected Then
                Throw New Common_Exception(Common_Exception.ErrorType.SAP, "RECNEW_000", "Pass-in Company isn't connected")
            End If

            Dim oRecSet As SAPbobsCOM.Recordset = DI.Console.Company.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Dim _ErrorMessage As String = ""
            Dim sqlStr As String = ""

            Try
                sqlStr = Me.SelectQuery & " " & Me.filterQuery
                oRecSet.DoQuery(_Query)
            Catch ex As Exception

                _ErrorMessage = ""
                _ErrorMessage &= vbCrLf & "[SQLs Server] Unable to Process query (Select Query)"
                _ErrorMessage &= vbCrLf & "******************************************"
                _ErrorMessage &= vbCrLf & sqlStr
                _ErrorMessage &= vbCrLf & "******************************************"
                _ErrorMessage &= vbCrLf & "Error Description:"
                _ErrorMessage &= vbCrLf & ex.ToString
                _ErrorMessage &= vbCrLf & "******************************************"

                Throw New Common_Exception(Common_Exception.ErrorType.Normal, "EXC0001", _ErrorMessage)
            End Try

            Return oRecSet
        End Function
        Public Function Execute() As SAPbobsCOM.Recordset
            If Not DI.Console.Company.Company.Connected Then
                Throw New Common_Exception(Common_Exception.ErrorType.SAP, "RECNEW_000", "Pass-in Company isn't connected")
            End If

            Dim oRecSet As SAPbobsCOM.Recordset = DI.Console.Company.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Dim _ErrorMessage As String = ""
            Dim sqlStr As String = ""

            Try
                sqlStr = Me.SelectQuery & " " & Me.filterQuery
                oRecSet.DoQuery(sqlStr)
            Catch ex As Exception

                _ErrorMessage = ""
                _ErrorMessage &= vbCrLf & "[SQLs Server] Unable to Process query (Select Query)"
                _ErrorMessage &= vbCrLf & "******************************************"
                _ErrorMessage &= vbCrLf & sqlStr
                _ErrorMessage &= vbCrLf & "******************************************"
                _ErrorMessage &= vbCrLf & "Error Description:"
                _ErrorMessage &= vbCrLf & ex.ToString
                _ErrorMessage &= vbCrLf & "******************************************"

                Throw New Common_Exception(Common_Exception.ErrorType.Normal, "EXC0001", _ErrorMessage)
            End Try

            Return oRecSet
        End Function

        Public Sub Process(ByVal pStatus As Status)
            If Not DI.Console.Company.Company.Connected Then
                Throw New Common_Exception(Common_Exception.ErrorType.SAP, "RECNEW_000", "Pass-in Company isn't connected")
            End If

            Dim oRecSet As SAPbobsCOM.Recordset = DI.Console.Company.Company.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset)
            Dim _ErrorMessage As String = ""
            Dim sqlStr As String = ""

            Select Case pStatus
                Case Status.stt_INSERT
                    Try
                        sqlStr = Me.InsertQuery
                        oRecSet.DoQuery(sqlStr)
                    Catch ex As Exception

                        _ErrorMessage = ""
                        _ErrorMessage &= vbCrLf & "[SQLs Server] Unable to Process query (Insert Query)"
                        _ErrorMessage &= vbCrLf & "******************************************"
                        _ErrorMessage &= vbCrLf & sqlStr
                        _ErrorMessage &= vbCrLf & "******************************************"
                        _ErrorMessage &= vbCrLf & "Error Description:"
                        _ErrorMessage &= vbCrLf & ex.ToString
                        _ErrorMessage &= vbCrLf & "******************************************"

                        Throw New Common_Exception(Common_Exception.ErrorType.Normal, "PRS0001", _ErrorMessage)
                    End Try
                Case Status.stt_UPDATE
                    Try
                        sqlStr = Me.UpdateQuery & " " & Me.filterQuery
                        oRecSet.DoQuery(sqlStr)
                    Catch ex As Exception

                        _ErrorMessage = ""
                        _ErrorMessage &= vbCrLf & "[SQLs Server] Unable to Process query (Update Query)"
                        _ErrorMessage &= vbCrLf & "******************************************"
                        _ErrorMessage &= vbCrLf & sqlStr
                        _ErrorMessage &= vbCrLf & "******************************************"
                        _ErrorMessage &= vbCrLf & "Error Description:"
                        _ErrorMessage &= vbCrLf & ex.ToString
                        _ErrorMessage &= vbCrLf & "******************************************"

                        Throw New Common_Exception(Common_Exception.ErrorType.Normal, "PRS0002", _ErrorMessage)
                    End Try
                Case Status.stt_DELETE
                    Try
                        sqlStr = Me.DeleteQuery & " " & Me.filterQuery
                        oRecSet.DoQuery(sqlStr)
                    Catch ex As Exception

                        _ErrorMessage = ""
                        _ErrorMessage &= vbCrLf & "[SQLs Server] Unable to Process query (Delete Query)"
                        _ErrorMessage &= vbCrLf & "******************************************"
                        _ErrorMessage &= vbCrLf & sqlStr
                        _ErrorMessage &= vbCrLf & "******************************************"
                        _ErrorMessage &= vbCrLf & "Error Description:"
                        _ErrorMessage &= vbCrLf & ex.ToString
                        _ErrorMessage &= vbCrLf & "******************************************"

                        Throw New Common_Exception(Common_Exception.ErrorType.Normal, "PRS0003", _ErrorMessage)
                    End Try
            End Select
        End Sub
#End Region

        ''' <summary>
        ''' String Converter: 
        ''' Replace ' >> '' //
        ''' Replace Special (!, @, #, ...) to ([!], [@], [#], ...)
        ''' </summary>
        ''' <param name="pString"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        ''' 
        Public Function AdvConvert(ByVal pString As String) As String
            Dim oString As String = ""

            If pString Is Nothing Then
                pString = ""
            End If

            'Check the Search Value exists special character or not -----
            For i As Integer = 0 To pString.Length - 1
                If Array.IndexOf(_AdvIndex, pString.Substring(i, 1)) > -1 Then
                    oString &= pString.Substring(i, 1) & pString.Substring(i, 1)
                Else
                    oString &= pString.Substring(i, 1)
                End If
            Next
            '------------------------------------------------------------

            'If oString.IndexOf("'") > -1 Then
            '    oString = oString.Replace("'", "''")
            'End If

            Return oString
        End Function

        Public Function StrConvert(ByVal pString As String) As String
            Dim oString As String = ""

            If pString Is Nothing Then
                pString = ""
            End If

            'Check the Search Value exists special character or not -----
            For i As Integer = 0 To pString.Length - 1
                If Array.IndexOf(_SpecInedx, pString.Substring(i, 1)) > -1 Then
                    oString &= "[" & pString.Substring(i, 1) & "]"
                Else
                    oString &= pString.Substring(i, 1)
                End If
            Next
            '------------------------------------------------------------

            If oString.IndexOf("'") > -1 Then
                oString = oString.Replace("'", "''")
            End If

            Return oString
        End Function

    End Class
End Namespace

