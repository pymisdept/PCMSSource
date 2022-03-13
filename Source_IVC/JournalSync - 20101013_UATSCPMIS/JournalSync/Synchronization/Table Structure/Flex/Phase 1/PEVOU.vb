Namespace Datatable.Flex
    Public Class PEVOU
        Inherits CPS.SQL.Interface.RecordSet

#Region "Constanst Value"
        Public Const TableName As String = "PEVOU"
        Public Const PEVOU_COM_CDE As String = "PEVOU_COM_CDE"
        Public Const PEVOU_REF_NUM As String = "PEVOU_REF_NUM"
        Public Const PEVOU_EXT As String = "PEVOU_EXT"
        Public Const PEVOU_ERR As String = "PEVOU_ERR"
        Public Const PEVOU_UPD_DTE As String = "PEVOU_UPD_DTE"
        Public Const PEVOU_PCMS_STA As String = "PEVOU_PCMS_STA"
        Public Const PEVOU_PCMS_UPD_DTE As String = "PEVOU_PCMS_UPD_DTE"
        Public Const PEVOU_PCMS_RMK As String = "PEVOU_PCMS_RMK"
#End Region

#Region "Define New"
        Public Sub New()
            MyBase.New(TableName, SubMain.FLEX_DBName)
            Me.add(PEVOU_COM_CDE)
            Me.add(PEVOU_REF_NUM)
            Me.add(PEVOU_EXT)
            Me.add(PEVOU_ERR)
            Me.add(PEVOU_UPD_DTE)
            Me.add(PEVOU_PCMS_STA)
            Me.add(PEVOU_PCMS_UPD_DTE)
            Me.add(PEVOU_PCMS_RMK)
        End Sub
#End Region

    End Class
End Namespace