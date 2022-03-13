Public Partial Class index1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = True Then
            Exit Sub
        Else
            'Starting Point

            'Check Pass in Parameters
            Dim mUserCode As String = ""
            Dim mReportName As String = ""
            Dim mProjectCode As String = ""
            mUserCode = Request.QueryString("UserID")
            mReportName = Request.QueryString("ReportName")
            mProjectCode = Request.QueryString("ProjectCode")

            Dim mInit As Boolean = LoadCrystalReport(mUserCode, mReportName, mProjectCode)
            If mInit = True Then
                'Program Initialize for further selection
                ProgramInit(mUserCode, mReportName, mProjectCode)
            End If
        End If
    End Sub

    Private Sub ProgramInit(ByVal pUserID As String, ByVal pReportName As String, ByVal pProjectCode As String)
        'Get Server Path
        'Declare Variables
        Dim mFileList(), mFileFull() As String
        Dim mdt As DataTable
        Dim mCounter As Integer

        'Get Data Table of Crystal Report Files
        'From Class GetCrystalFiles 
        Dim mGetList As New GetCrystalFiles(Session("ReportFolder"))
        mCounter = mGetList.RetrieveCounter
        mFileList = mGetList.RetrieveFileName
        mFileFull = mGetList.RetrieveFileFull
        mdt = mGetList.RetrieveCrystalList
        'Bind Crystal Report File Data Source
        REPORT_ListBox.DataSource = mdt
        REPORT_ListBox.DataBind()

        'Get Data Table of User List
        'From Class GetUserID
        Dim mGetUser As New GetUserID()
        mdt = mGetUser.RetrieveUserList()
        'Bind User Data Source
        USER_DropDownList.DataSource = mdt
        USER_DropDownList.DataBind()

        'Get Data Table of Project
        'From Class GetProjectCode
        Dim mGetProject As New GetProjectCode()
        mdt = mGetProject.RetrieveProjectList()
        PRJ_DropDownList.DataSource = mdt
        PRJ_DropDownList.DataBind()
    End Sub

    Private Sub REPORT_Button_Click(ByVal sender As Object, ByVal e As System.EventArgs) Handles REPORT_Button.Click
        Dim mUserCode As String = ""
        Dim mReportName As String = ""
        Dim mProjectCode As String = ""
        mUserCode = USER_DropDownList.SelectedValue
        mReportName = REPORT_ListBox.SelectedItem.Text
        mProjectCode = PRJ_DropDownList.SelectedValue

        LoadCrystalReport(mUserCode, mReportName, mProjectCode)
    End Sub

    Private Function LoadCrystalReport(ByVal pUserCode As String, ByVal pReportName As String, ByVal pProjectCode As String) As Boolean
        If Not (pReportName <> "") Then
            Return True
        Else
            Dim mpathroot As String = Server.MapPath("")
            mpathroot = mpathroot + "\" & Session("ReportFolder") & "\" + pReportName
            mpathroot = mpathroot.Replace("\", "CPS1000")
            Session("Report_FullPath") = mpathroot
            Session("Report_UserID") = pUserCode
            Session("Report_ProjectCode") = pProjectCode
            Dim mRedirect As String = "<script language='javascript'>location.href='ReportFrame.aspx?Report=" & pReportName & "&FPrjCode=20041&TPrjCode=20041';</script>"
            'Dim mRedirect As String = "<script language='javascript'>location.href='ReportFrame.aspx?Report=" & pReportName & "&DocEntry=6';</script>"
            'Dim mRedirect As String = "<script language='javascript'>location.href='ReportFrame.aspx?Report=" & pReportName & "';</script>"
            Response.Write(mRedirect)
            Return False
        End If
    End Function
End Class