Public Partial Class WebForm1
    Inherits System.Web.UI.Page

    Protected Sub Page_Load(ByVal sender As Object, ByVal e As System.EventArgs) Handles Me.Load

    End Sub

    Protected Sub Button1_Click(ByVal sender As Object, ByVal e As EventArgs) Handles Button1.Click
        Response.Write("<script language='Javascript'>window.open('ReportFrame.aspx?Report=QS46.rpt&FPrjCode=500&UserID=adawong');</script>")
    End Sub
End Class