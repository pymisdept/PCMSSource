Imports SAPbobsCOM

Namespace SQL
    Public Class RecordSet

        Private _RecordSet As SAPbobsCOM.Recordset
        Private _ErrorMessage As String

        Public Sub New()
            _RecordSet = currentCompany.GetBusinessObject(BoObjectTypes.BoRecordset)
            _ErrorMessage = ""
        End Sub

        ''' <summary>
        ''' Handling the select statement [SELECT * FROM DATATABLE]
        ''' </summary>
        ''' <returns>Success: return currenct recordset, Catch exception: return nothing</returns>
        ''' <remarks></remarks>
        Public Function execute(ByVal pQuery As String) As SAPbobsCOM.Recordset
            Try
                _RecordSet.DoQuery(pQuery)
                Return _RecordSet
            Catch ex As Exception
                _ErrorMessage = ""
                _ErrorMessage &= vbCrLf & "[SQLs Server] Unable to execute query (Select Query)"
                _ErrorMessage &= vbCrLf & "******************************************"
                _ErrorMessage &= vbCrLf & pQuery
                _ErrorMessage &= vbCrLf & "******************************************"
                _ErrorMessage &= vbCrLf & "Error Description:"
                _ErrorMessage &= vbCrLf & ex.ToString
                _ErrorMessage &= vbCrLf & "******************************************"

                Return Nothing
            End Try
        End Function

        ''' <summary>
        ''' Handling the advance query [INSERT, UPDATE, DELETE, ...]
        ''' </summary>
        ''' <returns>Success: return true, Catch exception: return false</returns>
        ''' <remarks></remarks>
        Public Function process(ByVal pQuery As String) As Boolean
            Try
                _RecordSet.DoQuery(pQuery)
                Return True
            Catch ex As Exception
                _ErrorMessage = ""
                _ErrorMessage &= vbCrLf & "[SQLs Server] Unable to process query (Advance Query)"
                _ErrorMessage &= vbCrLf & "******************************************"
                _ErrorMessage &= vbCrLf & pQuery
                _ErrorMessage &= vbCrLf & "******************************************"
                _ErrorMessage &= vbCrLf & "Error Description:"
                _ErrorMessage &= vbCrLf & ex.ToString
                _ErrorMessage &= vbCrLf & "******************************************"

                Return False
            End Try
        End Function

        Public Function errorMessage() As String
            Return _ErrorMessage
        End Function

    End Class
End Namespace