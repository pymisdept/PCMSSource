Imports CPS.SQL.Condition

Namespace Datatable.SAP.PCMS
    Public Class CPSFIN
        Inherits CPS.SQL.Interface.RecordSet

        Private ReadOnly CheckPoint As String = "Sync_VAdjust"
        Private ReadOnly ErrorDescription As String = "Modify Not Success"

        Public Sub New()
            MyBase.New(TableName)
            Me.add(_COM_CDE)
            Me.add(_REF_NUM)
            Me.add(_LIN_NUM)

            'Add by Michael, begin
            Me.add(_REVTRANS)
            Me.add(_DOCENTRY)
            'Add by Michael, end
            Me.add(_BCH_ID)
            Me.add(_VOU_TYP)
            Me.add(_VOU_DTE)
            Me.add(_DES)
            Me.add(_ACC_CDE)
            Me.add(_ANA_CDE1)
            Me.add(_ANA_CDE2)
            Me.add(_ANA_CDE3)
            Me.add(_ANA_CDE4)
            Me.add(_ANA_CDE5)
            Me.add(_DOC_NUM)
            Me.add(_ALT_DOC_NUM)
            Me.add(_DOC_TYP)
            Me.add(_DOC_DTE)
            Me.add(_DOC_DUE_DTE)
            Me.add(_CCY_CDE)
            Me.add(_AMT)
            Me.add(_EXC_RAT)
            Me.add(_AMT_BAS)
            Me.add(_D_C)
            Me.add(_DOC_PAY_TRM)
            Me.add(_QTY)
            Me.add(_UNI)
            Me.add(_DES1)
            Me.add(_DES2)
            Me.add(_ANA_CDE01)
            Me.add(_ANA_CDE02)
            Me.add(_ANA_CDE03)
            Me.add(_ANA_CDE04)
            Me.add(_ANA_CDE05)
            Me.add(_ANA_CDE06)
            Me.add(_ANA_CDE07)
            Me.add(_ANA_CDE08)
            Me.add(_ANA_CDE09)
            Me.add(_ANA_CDE10)
            Me.add(_RMK)
            Me.add(_FLX_BCH_ID)
            Me.add(_FLX_VOU_NUM)
            Me.add(_FLX_STA)
            Me.add(_FLX_UPD_DTE)
        End Sub

#Region "Constanst Value"
        Public Const TableName As String = "CPSFIN"
        Public Const _COM_CDE As String = "COM_CDE"  'Company code (PK)
        Public Const _REF_NUM As String = "REF_NUM"  'Ref. No. (PK)
        Public Const _LIN_NUM As String = "LIN_NUM"  'Line (PK)
        'Add by Michael, begin
        Public Const _REVTRANS As String = "REVTRANS"  'REVTRANS (PK)
        Public Const _DOCENTRY As String = "DOCENTRY"  'DOCENTRY (PK)
        'Add by Michael, end
        Public Const _BCH_ID As String = "BCH_ID"   'PCMS Batch ID
        Public Const _VOU_TYP As String = "VOU_TYP"  'Voucher Type 
        Public Const _VOU_DTE As String = "VOU_DTE"  'Voucher date 
        Public Const _DES As String = "DES"  'Description
        Public Const _ACC_CDE As String = "ACC_CDE"  'A/C
        Public Const _ANA_CDE1 As String = "ANA_CDE1" 'Analysis code 1
        Public Const _ANA_CDE2 As String = "ANA_CDE2" 'Analysis code 2
        Public Const _ANA_CDE3 As String = "ANA_CDE3" 'Analysis code 3
        Public Const _ANA_CDE4 As String = "ANA_CDE4" 'Analysis code 4
        Public Const _ANA_CDE5 As String = "ANA_CDE5" 'Analysis code 5
        Public Const _DOC_NUM As String = "DOC_NUM"  'Document no.
        Public Const _ALT_DOC_NUM As String = "ALT_DOC_NUM"  'Alt. Document no.
        Public Const _DOC_TYP As String = "DOC_TYP"  'Document type
        Public Const _DOC_DTE As String = "DOC_DTE"  'Document date
        Public Const _DOC_DUE_DTE As String = "DOC_DUE_DTE"  'Document due date
        Public Const _CCY_CDE As String = "CCY_CDE"  'Currency
        Public Const _AMT As String = "AMT"  'Amount
        Public Const _EXC_RAT As String = "EXC_RAT"  'Exchange Rate
        Public Const _AMT_BAS As String = "AMT_BAS"  'Equv. Amount
        Public Const _D_C As String = "D_C"  'D/C
        Public Const _DOC_PAY_TRM As String = "DOC_PAY_TRM"  'Payment term
        Public Const _QTY As String = "QTY"  'Quantity
        Public Const _UNI As String = "UNI"  'Unit
        Public Const _DES1 As String = "DES1" 'Particular 1
        Public Const _DES2 As String = "DES2" 'Particular 2
        Public Const _ANA_CDE01 As String = "ANA_CDE01"    'Extended Analysis code 1
        Public Const _ANA_CDE02 As String = "ANA_CDE02"    'Extended Analysis code 2
        Public Const _ANA_CDE03 As String = "ANA_CDE03"    'Extended Analysis code 3
        Public Const _ANA_CDE04 As String = "ANA_CDE04"    'Extended Analysis code 4
        Public Const _ANA_CDE05 As String = "ANA_CDE05"    'Extended Analysis code 5
        Public Const _ANA_CDE06 As String = "ANA_CDE06"    'Extended Analysis code 6
        Public Const _ANA_CDE07 As String = "ANA_CDE07"    'Extended Analysis code 7
        Public Const _ANA_CDE08 As String = "ANA_CDE08"    'Extended Analysis code 8
        Public Const _ANA_CDE09 As String = "ANA_CDE09"    'Extended Analysis code 9
        Public Const _ANA_CDE10 As String = "ANA_CDE10"    'Extended Analysis code 10
        Public Const _RMK As String = "RMK"  'Remark
        Public Const _FLX_BCH_ID As String = "FLX_BCH_ID"   'Batch Number
        Public Const _FLX_VOU_NUM As String = "FLX_VOU_NUM"  'Voucher Number
        Public Const _FLX_STA As String = "FLX_STA"  'Flex Status
        Public Const _FLX_UPD_DTE As String = "FLX_UPD_DTE"  'Flex Update Date
#End Region

#Region "Assign Value"
        Public WriteOnly Property COM_CDE() As String
            Set(ByVal value As String)
                If Not Me.modify(_COM_CDE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Company code (PK)]")
                End If
            End Set
        End Property
        Public WriteOnly Property REF_NUM() As String
            Set(ByVal value As String)
                If Not Me.modify(_REF_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Ref. No. (PK)]")
                End If
            End Set
        End Property
        Public WriteOnly Property LIN_NUM() As String
            Set(ByVal value As String)
                If Not Me.modify(_LIN_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Line (PK)]")
                End If
            End Set
        End Property
        Public WriteOnly Property BCH_ID() As String
            Set(ByVal value As String)
                If Not Me.modify(_BCH_ID, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[PCMS Batch ID]")
                End If
            End Set
        End Property

        'Add by Michael, begin
        Public WriteOnly Property REVTRANS() As String
            Set(ByVal value As String)
                If Not Me.modify(_REVTRANS, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[REVTRANS]")
                End If
            End Set
        End Property

        Public WriteOnly Property DOCENTRY() As String
            Set(ByVal value As String)
                If Not Me.modify(_DOCENTRY, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[DOCENTRY]")
                End If
            End Set
        End Property
        'Add by Michael, end

        Public WriteOnly Property VOU_TYP() As String
            Set(ByVal value As String)
                If Not Me.modify(_VOU_TYP, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Voucher Type ]")
                End If
            End Set
        End Property
        Public WriteOnly Property VOU_DTE() As Date
            Set(ByVal value As Date)
                If Not Me.modify(_VOU_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Voucher date ]")
                End If
            End Set
        End Property
        Public WriteOnly Property DES() As String
            Set(ByVal value As String)
                If Not Me.modify(_DES, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Description]")
                End If
            End Set
        End Property
        Public WriteOnly Property ACC_CDE() As String
            Set(ByVal value As String)
                If Not Me.modify(_ACC_CDE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[A/C]")
                End If
            End Set
        End Property
        Public WriteOnly Property ANA_CDE1() As String
            Set(ByVal value As String)
                If Not Me.modify(_ANA_CDE1, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Analysis code 1]")
                End If
            End Set
        End Property
        Public WriteOnly Property ANA_CDE2() As String
            Set(ByVal value As String)
                If Not Me.modify(_ANA_CDE2, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Analysis code 2]")
                End If
            End Set
        End Property
        Public WriteOnly Property ANA_CDE3() As String
            Set(ByVal value As String)
                If Not Me.modify(_ANA_CDE3, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Analysis code 3]")
                End If
            End Set
        End Property
        Public WriteOnly Property ANA_CDE4() As String
            Set(ByVal value As String)
                If Not Me.modify(_ANA_CDE4, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Analysis code 4]")
                End If
            End Set
        End Property
        Public WriteOnly Property ANA_CDE5() As String
            Set(ByVal value As String)
                If Not Me.modify(_ANA_CDE5, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Analysis code 5]")
                End If
            End Set
        End Property
        Public WriteOnly Property DOC_NUM() As String
            Set(ByVal value As String)
                If Not Me.modify(_DOC_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document no.]")
                End If
            End Set
        End Property
        Public WriteOnly Property ALT_DOC_NUM() As String
            Set(ByVal value As String)
                If Not Me.modify(_ALT_DOC_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Alt. Document no.]")
                End If
            End Set
        End Property
        Public WriteOnly Property DOC_TYP() As String
            Set(ByVal value As String)
                If Not Me.modify(_DOC_TYP, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document type]")
                End If
            End Set
        End Property
        Public WriteOnly Property DOC_DTE() As Date
            Set(ByVal value As Date)
                If Not Me.modify(_DOC_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document date]")
                End If
            End Set
        End Property
        Public WriteOnly Property DOC_DUE_DTE() As Date
            Set(ByVal value As Date)
                If Not Me.modify(_DOC_DUE_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Document due date]")
                End If
            End Set
        End Property
        Public WriteOnly Property CCY_CDE() As String
            Set(ByVal value As String)
                If Not Me.modify(_CCY_CDE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Currency]")
                End If
            End Set
        End Property
        Public WriteOnly Property AMT() As String
            Set(ByVal value As String)
                If Not Me.modify(_AMT, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Amount]")
                End If
            End Set
        End Property
        Public WriteOnly Property EXC_RAT() As String
            Set(ByVal value As String)
                If Not Me.modify(_EXC_RAT, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Exchange Rate]")
                End If
            End Set
        End Property
        Public WriteOnly Property AMT_BAS() As String
            Set(ByVal value As String)
                If Not Me.modify(_AMT_BAS, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Equv. Amount]")
                End If
            End Set
        End Property
        Public WriteOnly Property D_C() As String
            Set(ByVal value As String)
                If Not Me.modify(_D_C, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[D/C]")
                End If
            End Set
        End Property
        Public WriteOnly Property DOC_PAY_TRM() As String
            Set(ByVal value As String)
                If Not Me.modify(_DOC_PAY_TRM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Payment term]")
                End If
            End Set
        End Property
        Public WriteOnly Property QTY() As String
            Set(ByVal value As String)
                If Not Me.modify(_QTY, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Quantity]")
                End If
            End Set
        End Property
        Public WriteOnly Property UNI() As String
            Set(ByVal value As String)
                If Not Me.modify(_UNI, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Unit]")
                End If
            End Set
        End Property
        Public WriteOnly Property DES1() As String
            Set(ByVal value As String)
                If Not Me.modify(_DES1, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Particular 1]")
                End If
            End Set
        End Property
        Public WriteOnly Property DES2() As String
            Set(ByVal value As String)
                If Not Me.modify(_DES2, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Particular 2]")
                End If
            End Set
        End Property
        Public WriteOnly Property ANA_CDE01() As String
            Set(ByVal value As String)
                If Not Me.modify(_ANA_CDE01, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 1]")
                End If
            End Set
        End Property
        Public WriteOnly Property ANA_CDE02() As String
            Set(ByVal value As String)
                If Not Me.modify(_ANA_CDE02, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 2]")
                End If
            End Set
        End Property
        Public WriteOnly Property ANA_CDE03() As String
            Set(ByVal value As String)
                If Not Me.modify(_ANA_CDE03, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 3]")
                End If
            End Set
        End Property
        Public WriteOnly Property ANA_CDE04() As String
            Set(ByVal value As String)
                If Not Me.modify(_ANA_CDE04, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 4]")
                End If
            End Set
        End Property
        Public WriteOnly Property ANA_CDE05() As String
            Set(ByVal value As String)
                If Not Me.modify(_ANA_CDE05, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 5]")
                End If
            End Set
        End Property
        Public WriteOnly Property ANA_CDE06() As String
            Set(ByVal value As String)
                If Not Me.modify(_ANA_CDE06, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 6]")
                End If
            End Set
        End Property
        Public WriteOnly Property ANA_CDE07() As String
            Set(ByVal value As String)
                If Not Me.modify(_ANA_CDE07, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 7]")
                End If
            End Set
        End Property
        Public WriteOnly Property ANA_CDE08() As String
            Set(ByVal value As String)
                If Not Me.modify(_ANA_CDE08, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 8]")
                End If
            End Set
        End Property
        Public WriteOnly Property ANA_CDE09() As String
            Set(ByVal value As String)
                If Not Me.modify(_ANA_CDE09, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 9]")
                End If
            End Set
        End Property
        Public WriteOnly Property ANA_CDE10() As String
            Set(ByVal value As String)
                If Not Me.modify(_ANA_CDE10, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Extended Analysis code 10]")
                End If
            End Set
        End Property
        Public WriteOnly Property RMK() As String
            Set(ByVal value As String)
                If Not Me.modify(_RMK, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Remark]")
                End If
            End Set
        End Property
        Public WriteOnly Property FLX_BCH_ID() As String
            Set(ByVal value As String)
                If Not Me.modify(_FLX_BCH_ID, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Batch Number]")
                End If
            End Set
        End Property
        Public WriteOnly Property FLX_VOU_NUM() As String
            Set(ByVal value As String)
                If Not Me.modify(_FLX_VOU_NUM, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Voucher Number]")
                End If
            End Set
        End Property
        Public WriteOnly Property FLX_STA() As String
            Set(ByVal value As String)
                If Not Me.modify(_FLX_STA, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Flex Status]")
                End If
            End Set
        End Property
        Public WriteOnly Property FLX_UPD_DTE() As Date
            Set(ByVal value As Date)
                If Not Me.modify(_FLX_UPD_DTE, value) Then
                    Throw New BaseException(BaseException.ErrorType.System, CheckPoint, ErrorDescription & " " & "[Flex Update Date]")
                End If
            End Set
        End Property
#End Region

    End Class
End Namespace

