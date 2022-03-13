Imports System.Threading
Imports CrystalDecisions.CrystalReports.Engine
Imports CrystalDecisions.Shared



Partial Public Class ReportA
    Inherits System.Web.UI.Page
    Private WithEvents mRD As ReportDocument
    Protected Sub Page_Init(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Init
        'Parameter pass in

        Try
            WriteFile("Start")
            Dim mReport As String = Request.QueryString("Report")

            Dim mFPrjCode As String = Request.QueryString("FPrjCode")
            'WriteLog("Page Init: mFPrjCode: " & mFPrjCode, Server.MapPath(""))

            Dim mTPrjCode As String = Request.QueryString("TPrjCode")

            Dim mDocEntry As String = Request.QueryString("DocEntry")

            'Dim mFDocEntry As String = Request.QueryString("FDocEntry")

            Dim mUserID As String = Request.QueryString("UserID")

            Dim mDocDate As String = Request.QueryString("DocDate")

            If mReport <> "" Then
                mReport = Request("Report")
            End If

            If mFPrjCode <> "" Then
                mFPrjCode = Request("FPrjCode")
            End If

            If mTPrjCode <> "" Then
                mTPrjCode = Request("TPrjCode")
            End If

            If mUserID <> "" Then
                mUserID = Request("UserID")
            End If

            If mDocDate <> "" Then
                mDocDate = Request("DocDate")
            End If

            WriteFile(mReport)

            Dim mServer As String = Session("Server")
            Dim mDB As String = Session("B1Database")
            Dim mpass As String = Session("UserPW")
            Dim rpt As String = Session("ReportFolder")

            If mReport <> "" Then

                mReport = mReport.Replace("CPS1000", "\")

                WriteFile("Robin " & mServer)
                WriteFile(mDB)
                WriteFile(mpass)
                WriteFile("ReportFolder")
                CallReport(mReport, mDocEntry, mFPrjCode, mTPrjCode, mUserID, mDocDate)

            Else
                ' Add By karrson
                WriteFile("mReport = ''")
                Response.Redirect("NotFound.aspx")
            End If
        Catch ex As Exception

            'Response.Write("<script language='Javascript'>Parent.location.href='NotFound.aspx';</script>")
            WriteFile("Exeption in Page Init")
            Response.Redirect("NotFound.aspx")
        End Try

    End Sub

    Private Sub CallReport(ByVal pReport As String, _
                           Optional ByVal pDocEntry As String = "", Optional ByVal pFPrjCode As String = "", _
                           Optional ByVal pTPrjCode As String = "", Optional ByVal pUserID As String = "", Optional ByVal pDocDate As String = "")
        Try
            'Get Parameters from session variable
            'Dim mServer As String = Session("Server")
            'Dim mDB As String = Session("B1Database")
            'Dim muserid As String = Session("UserID")
            'Dim mpass As String = Session("UserPW")
           
            'Response.Write(mServer)
            'Open ReportDocument class
            mRD = New ReportDocument()
            'Run Report

            Dim mReport As String = Session("ReportFolder") + "\" + pReport

            If IO.File.Exists(mReport) = False Then

                'Response.Write("<script language='Javascript'>Parent.location.href='NotFound.aspx';</script>")
                WriteFile("File not exists")
                Response.Redirect("NotFound.aspx")
            Else

                mRD.Load(mReport)
                WriteFile("After Load Report")
                mRD.ReportOptions.EnableSaveDataWithReport = False
                mRD.ReportOptions.EnableSavePreviewPicture = False
                mRD.ReportOptions.EnableUseDummyData = False

                'Set Page Title
                WriteFile("Set Page Title")
                Me.Title = "Report Name: " & pReport
                'Set Report Document to Report Source
                CrystalReportViewer1.HasRefreshButton = True
                CrystalReportViewer1.HasDrillUpButton = False
                CrystalReportViewer1.HasDrilldownTabs = False
                CrystalReportViewer1.HasToggleGroupTreeButton = False

                CrystalReportViewer1.PrintMode = CrystalDecisions.Web.PrintMode.ActiveX
                CrystalReportViewer1.HasToggleParameterPanelButton = False
                CrystalReportViewer1.EnableDatabaseLogonPrompt = False
                CrystalReportViewer1.EnableDrillDown = False
                CrystalReportViewer1.ToolPanelView = CrystalDecisions.Web.ToolPanelViewType.None
                WriteFile("Set Connection")
                'WriteFile(mServer)
                'WriteFile(mDB)
                'WriteFile(muserid)
                'WriteFile(mpass)
                For i As Integer = 0 To mRD.DataSourceConnections.Count - 1
                    'mRD.DataSourceConnections(i).SetConnection(mServer, mDB, muserid, mpass)
                    mRD.DataSourceConnections(i).SetConnection("PCMS_DB", "MCPAY110", "sa", "Compass2008")
                Next


                WriteFile("End Set Connection")
                mRD.Refresh()
                CrystalReportViewer1.ReportSource = mRD
                WriteFile("End Report Source")
                mRD.Refresh()

                WriteFile("Pass Parameter")
                fn_PassParameters(pDocEntry:=pDocEntry, pFPrjCode:=pFPrjCode, _
                                    pTPrjCode:=pTPrjCode, pUserID:=pUserID, pDocDate:=pDocDate)
                WriteFile("End Pass Parameter")
            End If
        Catch ex As Exception
            Response.Write("<script language='javascript'>alert('" & ex.Message & "');</script>")
            ' test by karrson
            WriteFile("Exception in Call Report")
            Response.Redirect("NotFound.aspx")
        End Try
    End Sub



    Private Sub SetDBLogonForReport(ByVal myConnectionInfo As ConnectionInfo)
        Dim myTableLogOnInfos As TableLogOnInfos = CrystalReportViewer1.LogOnInfo
        For Each myTableLogOnInfo As TableLogOnInfo In myTableLogOnInfos
            myTableLogOnInfo.ConnectionInfo = myConnectionInfo
        Next
    End Sub

#Region "Parameter"
    Private Sub fn_PassParameters(Optional ByVal pDocEntry As String = "", Optional ByVal pFPrjCode As String = "", _
                           Optional ByVal pTPrjCode As String = "", Optional ByVal pUserID As String = "", Optional ByVal pDocDate As String = "", Optional ByVal mFDocEntry As String = "")

        Dim params As CrystalDecisions.Shared.ParameterValues
        'pDocEntry = "100"
        'pUserID = "1234"

        For Each param As CrystalDecisions.CrystalReports.Engine.ParameterFieldDefinition _
          In mRD.DataDefinition.ParameterFields
            Dim discretevalue As CrystalDecisions.Shared.ParameterDiscreteValue = New ParameterDiscreteValue
            WriteFile("Parameter: " & param.Name)
            If param.Name = "UserID" And Not pUserID Is Nothing Then
                discretevalue.Value = pUserID
                params = mRD.DataDefinition.ParameterFields("UserID").DefaultValues
                params.AddValue(pUserID)
                mRD.DataDefinition.ParameterFields("UserID").ApplyCurrentValues(params)

            ElseIf param.Name = "DocEntry" And Not pDocEntry Is Nothing Then
                discretevalue.Value = pDocEntry
                params = mRD.DataDefinition.ParameterFields("DocEntry").DefaultValues
                params.Add(discretevalue)
                mRD.DataDefinition.ParameterFields("DocEntry").ApplyCurrentValues(params)

            ElseIf param.Name = "FPrjCode" And Not pFPrjCode Is Nothing Then
                If discretevalue.IsRange Then
                    discretevalue.Value = pFPrjCode
                Else
                    discretevalue.Value = pFPrjCode
                End If
                params = mRD.DataDefinition.ParameterFields("FPrjCode").DefaultValues
                params.Add(discretevalue)

                mRD.DataDefinition.ParameterFields("FPrjCode").ApplyCurrentValues(params)
            End If
            If param.Name = "DocDate" And Not pDocDate Is Nothing Then
                discretevalue.Value = pDocDate
                params = mRD.DataDefinition.ParameterFields("DocDate").DefaultValues
                params.AddValue(pDocDate)
                mRD.DataDefinition.ParameterFields("DocDate").ApplyCurrentValues(params)
            End If

        Next
    End Sub

    Private Sub fn_ParameterDiscreteValue(ByVal myParameterFields As ParameterFields, ByVal pFieldName As String, _
                                          ByVal pValue As String)
        Dim currentParameterValues As ParameterValues = New ParameterValues()
        Dim myParameterDiscreteValue As ParameterDiscreteValue = New ParameterDiscreteValue()

        myParameterDiscreteValue.Value = pValue.ToString()
        currentParameterValues.Add(myParameterDiscreteValue)

        Dim myParameterField As ParameterField = myParameterFields(pFieldName)
        myParameterField.CurrentValues = currentParameterValues


    End Sub

    Private Sub fn_ParameterRangeValue(ByVal myParameterFields As ParameterFields, ByVal pFieldName As String, _
                                       ByVal pStartVal As String, ByVal pEndVal As String)
        Dim myParameterRangeValue As ParameterRangeValue = New ParameterRangeValue()
        myParameterRangeValue.StartValue = pStartVal
        myParameterRangeValue.EndValue = pEndVal
        myParameterRangeValue.LowerBoundType = RangeBoundType.BoundInclusive
        myParameterRangeValue.UpperBoundType = RangeBoundType.BoundInclusive
        Dim myParameterField As ParameterField = myParameterFields(pFieldName)
        myParameterField.CurrentValues.Clear()
        myParameterField.CurrentValues.Add(myParameterRangeValue)
    End Sub
#End Region

    Private Sub Page_Unload(ByVal Sender As System.Object, ByVal e As System.EventArgs) Handles Me.Unload

        ' Release the memory used by the report.
        If Not mRD Is Nothing Then
            mRD.Close()
            mRD.Dispose()
        End If
        ' Dispose of the viewer to free up resources.
        CrystalReportViewer1.Dispose()

        GC.Collect()

    End Sub

    Private Sub Report_Init(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles mRD.InitReport
        'Response.Write("<script language='Javascript'>alert('OK');</script>")
    End Sub

    Private Sub Viewer_Err(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles CrystalReportViewer1.Error
        If CrystalReportViewer1.PrintMode = CrystalDecisions.Web.PrintMode.ActiveX Then

        ElseIf CrystalReportViewer1.PrintMode = CrystalDecisions.Web.PrintMode.Pdf Then
        Else
            Response.Redirect("NotFound.aspx")
        End If


        'Dim mSQLConna As SqlClient.SqlConnection = SQLServer_Connect()
        '
        'Response.Write("<script language='Javascript'>this.location.href='NotFound.aspx';</script>")'
    End Sub

    ' Test Funciton
    Sub WriteFile(ByVal s As String)
        Try
            Dim sw As New IO.StreamWriter(Server.MapPath("templog.txt"), True)
            sw.WriteLine(DateTime.Now.ToString("yyyy-MM-dd hh:mm:ss") + ":" + s)

            sw.Close()
        Catch ex As Exception

        End Try
    End Sub

End Class