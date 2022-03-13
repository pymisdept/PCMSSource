using System;
using System.Collections;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml.Linq;


using System.Text.RegularExpressions;

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using System.IO;

public partial class Control_MA07ExportPDF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            doExport();
        }
    }

    protected void doExport()
    {
        string docEntry = Request.QueryString["DocEntry"].ToString();
        string docDate = Request.QueryString["DocDate"].ToString();
        string userId = Request.QueryString["userid"].ToString();

        ReportDocument objRpt = new ReportDocument();
        string reportPath = ConfigurationManager.AppSettings["MA07CrystalReportPath"].ToString();
        objRpt.Load(reportPath);

        TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
        ConnectionInfo crConnectionInfo = new ConnectionInfo();
        CrystalDecisions.CrystalReports.Engine.Tables CrTables;

        crConnectionInfo.ServerName = ConfigurationManager.AppSettings["BackEndServer"].ToString();
        crConnectionInfo.UserID = ConfigurationManager.AppSettings["BackEndServerUser"].ToString();
        crConnectionInfo.Password = ConfigurationManager.AppSettings["BackEndServerPassword"].ToString();
        crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["PCMS800"].ToString();

        objRpt.SetParameterValue(0, userId);
        objRpt.SetParameterValue(1, docEntry);

        CrTables = objRpt.Database.Tables;

        foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
        {
            crtableLogoninfo = CrTable.LogOnInfo;
            crtableLogoninfo.ConnectionInfo = crConnectionInfo;
            CrTable.ApplyLogOnInfo(crtableLogoninfo);
        }

        objRpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, this.Page.Response, true, "MA07_" + DateTime.Now.ToString("yyyyMMddHHmmss"));

    }
}
