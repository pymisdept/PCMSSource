Imports CrystalDecisions
Partial Public Class Index
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load
        If IsPostBack = False Then
            Dim mpathroot As String = Server.MapPath("")
            mpathroot = mpathroot + "\CrystalReports\"
            Dim mFileList(), mFileFull() As String
            Dim mdt As DataTable
            Dim mCounter As Integer

            Dim mGetList As New GetCrystalFiles(mpathroot)
            mCounter = mGetList.RetrieveCounter
            mFileList = mGetList.RetrieveFileName
            mFileFull = mGetList.RetrieveFileFull
            mdt = mGetList.RetrieveCrystalList

            ReportList.DataSource = mdt
            ReportList.DataBind()
        End If

    End Sub
    Protected Sub ListBox1_SelectedIndexChanged(ByVal sender As Object, ByVal e As EventArgs) Handles ReportList.SelectedIndexChanged
        Dim mReport As String = ""
        mReport = ReportList.SelectedValue
        mReport = mReport.Replace("\", "load1report2replace3icon")
        Response.Write("<script language='javascript'>parent.F1R2.location.href='ReportFrame.aspx?Report=" & mReport & "';</script>")
    End Sub
End Class