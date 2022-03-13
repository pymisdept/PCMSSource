Namespace Excel.ProcessClass
    ''' <summary>
    ''' Karrson: New Function on 2009-08-25
    ''' Modified by Alice on 13 Oct 2009
    ''' </summary>
    ''' <remarks></remarks>
    Public Class QSI01
        Inherits Excel.Interface.FileInterface

        Private _PrjCode As String

        Public Sub New(ByVal pRequestID As String)
            MyBase.New(New Microsoft.Office.Interop.Excel.Application)
            MyBase.RequestID = pRequestID
        End Sub

        Public WriteOnly Property ProjectCode() As String
            Set(ByVal value As String)
                _PrjCode = value.Trim
            End Set
        End Property

        ' 2009-08-22 Modify by Karrson

        Public Function SheetMapping() As String()
            'Dim sqlStrs(1) As String
            Dim sqlStrs(8) As String
            Dim oPrjName As String

            oPrjName = getPrjName(_PrjCode)
            MyBase.FileType = MyBase.FunctionID

            sqlStrs(0) = "Select '" & _PrjCode & "' PRJCODE, '" & oPrjName.Replace("'", "''") & "' PRJNAME "


            sqlStrs(1) = "SELECT CARDCODE AS LIST_CLIENTCODE, CARDNAME AS LIST_CLIENTNAME FROM CPS_View_ClientList"

            sqlStrs(2) = "SELECT PRCCODE AS LIST_SUBSICODE , U_PRCDESC AS LIST_SUBSINAME FROM CPS_View_SubsidiaryList"
            sqlStrs(3) = "SELECT CODE AS LIST_CONTTYPE,[NAME] AS LIST_CONTNAME FROM [@PRJTYPE]"
            sqlStrs(4) = "select CODE AS LIST_BONDTYPE ,[NAME] AS LIST_BONDNAME  from [@bondtype]"
            sqlStrs(5) = "select PrcCode AS LIST_NATURECODE, U_PrcDesc AS LIST_NATURENAME  from OPRC"
            sqlStrs(5) = sqlStrs(5) + " where DimCode=4 and U_FatherCode not in ('DM','FM','CHN')"
            sqlStrs(6) = "select code AS LIST_GEOGCODE, [name] LIST_GEOGNAME from [@ZONECODE]"
            sqlStrs(7) = "select CurrCode AS LIST_CurCODE, FrgnName as LIST_CurName from OCRN"
            sqlStrs(8) = "Select * from QSI01_Form ('" & _PrjCode.Replace("'", "''") & "')"

            Return sqlStrs
        End Function

    End Class
End Namespace

