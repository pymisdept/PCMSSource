Imports CPS.SQL.Condition

Namespace Datatable.SAP.Draft
    Public Class Freight
        Inherits CPS.SQL.Interface.RecordSet

#Region "Constanst Value"
        Public Const TableName As String = "DRF3"
        Public Const DocEntry As String = "DocEntry"
        Public Const FreightCode As String = "ExpnsCode"
        Public Const ObjType As String = "ObjType"
        Public Const LC_Total As String = "LineTotal"
        Public Const FC_Total As String = "TotalFrgn"
#End Region

#Region "Define New"
        Public Sub New()
            MyBase.New(TableName)
            Me.add(DocEntry)
            Me.add(ObjType)
            Me.add(FreightCode)
            Me.add(LC_Total)
            Me.add(FC_Total)
        End Sub
#End Region

#Region "Get Filter Information Structure"

        Public Sub getFreightEntry(ByVal pDocEntry As Integer)
            Dim oCondition As Condition

            Me.clearFilter()

            '((DocEntry = '#####')
            oCondition = New Condition
            oCondition.BracketOpenNum = 2
            oCondition.Alias = Freight.DocEntry
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = pDocEntry
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            ' AND (ObjType = '18')
            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = Freight.ObjType
            oCondition.Operation = Condition.eOperation.op_EQUAL
            oCondition.Value = "18"
            oCondition.BracketCloseNum = 1
            Me.addFilter(oCondition)

            ' AND (LineTotal <> 0))
            oCondition = New Condition
            oCondition.Relation = Condition.eRelation.re_AND
            oCondition.BracketOpenNum = 1
            oCondition.Alias = Freight.LC_Total
            oCondition.Operation = Condition.eOperation.op_NOT_EQUAL
            oCondition.Value = 0
            oCondition.BracketCloseNum = 2
            Me.addFilter(oCondition)

        End Sub

#End Region

#Region "Get Freight Account Code"

        ''' <summary>
        ''' Get SAP Freight Account Code
        ''' </summary>
        ''' <param name="IntrnalKey"></param>
        ''' <returns></returns>
        ''' <remarks></remarks>
        Public Function getAcctCode(ByVal IntrnalKey As String) As String
            Dim sqlStr As String = ""
            Dim oRecSet As SAPbobsCOM.Recordset
            Dim pTransType As TransType = TransType.tt_AR

            Select Case pTransType
                Case TransType.tt_AR
                    sqlStr = "Select RevAcct AcctCode From OEXD Where ExpnsCode = '" & IntrnalKey & "'"
                Case TransType.tt_AP
                    sqlStr = "Select ExpnsAcct AcctCode From OEXD Where ExpnsCode = '" & IntrnalKey & "'"
            End Select

            oRecSet = commonRecordSet.execute(sqlStr)
            If oRecSet.RecordCount = 0 Then
                Return Nothing
            Else
                Return oRecSet.Fields.Item("AcctCode").Value
            End If

        End Function

        Enum TransType As Integer
            tt_AR = 0
            tt_AP = 1
        End Enum

#End Region

    End Class
End Namespace

