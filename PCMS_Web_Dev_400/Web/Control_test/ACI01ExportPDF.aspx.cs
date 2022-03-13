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

using CrystalDecisions.CrystalReports.Engine;
using CrystalDecisions.Shared;

using System.IO;

public partial class Control_ACI01ExportPDF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["ACI01CrystalReportPath"]) == true)
            {
                Error_Label.Text = "configuration file ACI03CrystalReportPath can not be blank";
                return;
            }
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["BackEndServer"]) == true)
            {
                Error_Label.Text = "configuration file BackEndServer can not be blank";
                return;
            }
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["BackEndServerUser"]) == true)
            {
                Error_Label.Text = "configuration file BackEndServerUser can not be blank";
                return;
            }
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["BackEndServerPassword"]) == true)
            {
                Error_Label.Text = "configuration file BackEndServerPassword can not be blank";
                return;
            }
            if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["PCMS800"]) == true)
            {
                Error_Label.Text = "configuration file PCMS800 can not be blank";
                return;
            }
            if (!String.IsNullOrEmpty(Request.QueryString["ProjectCode"]) && !String.IsNullOrEmpty(Request.QueryString["UserID"]))
            {
                //File.AppendAllText((@"E:\MartinDebug.txt", "before Request.QueryString[ProjectCode].ToString()");
                string projValue = Request.QueryString["ProjectCode"].ToString();

                //File.AppendAllText((@"E:\MartinDebug.txt", "after Request.QueryString[ProjectCode].ToString()");

                ReportDocument objRpt = new ReportDocument();
                //File.AppendAllText((@"E:\MartinDebug.txt", "after new ReportDocument()");

                string reportPath = ConfigurationManager.AppSettings["ACI01CrystalReportPath"].ToString();

                //File.AppendAllText((@"E:\MartinDebug.txt", "ConfigurationManager.AppSettings[ACI01CrystalReportPath");

                ////File.AppendAllText((@"E:\MartinDebug.txt", "before objRpt.Load(reportPath)");
                objRpt.Load(reportPath);

                //File.AppendAllText((@"E:\MartinDebug.txt", "objRpt.Load(reportPath)");
                //int projindex = ddlProject.SelectedIndex;
                //string projValue = ddlProject.Items[projindex].Value.ToString();

                TableLogOnInfo crtableLogoninfo = new TableLogOnInfo();
                ConnectionInfo crConnectionInfo = new ConnectionInfo();
                CrystalDecisions.CrystalReports.Engine.Tables CrTables;

                //File.AppendAllText((@"E:\MartinDebug.txt", "CrystalDecisions.CrystalReports.Engine.Tables CrTables;");

                crConnectionInfo.ServerName = ConfigurationManager.AppSettings["BackEndServer"].ToString();
                crConnectionInfo.UserID = ConfigurationManager.AppSettings["BackEndServerUser"].ToString();
                crConnectionInfo.Password = ConfigurationManager.AppSettings["BackEndServerPassword"].ToString();
                crConnectionInfo.DatabaseName = ConfigurationManager.AppSettings["PCMS800"].ToString();

                objRpt.SetParameterValue(0, Request.QueryString["UserID"].ToString().Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", ""));
                objRpt.SetParameterValue(1, projValue.Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", ""));

                //File.AppendAllText((@"E:\MartinDebug.txt", projValue.ToString());

                CrTables = objRpt.Database.Tables;

                foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
                {
                    crtableLogoninfo = CrTable.LogOnInfo;
                    crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                    CrTable.ApplyLogOnInfo(crtableLogoninfo);
                }
                /*
                ExportOptions exportOpts = new ExportOptions();
                DiskFileDestinationOptions diskOpts = new DiskFileDestinationOptions();
                exportOpts = objRpt.ExportOptions;
                exportOpts.ExportDestinationType = ExportDestinationType.DiskFile;
                exportOpts.ExportFormatType = ExportFormatType.PortableDocFormat;
                diskOpts.DiskFileName = @"C:\inetpub\wwwroot\pcms_800\ReportsFile\ACIReport.rpt";
                exportOpts.DestinationOptions = diskOpts;

                objRpt.ExportToHttpResponse(exportOpts, this.Page.Response, true, "ACI01" + DateTime.Now.ToString("yyyyMMdd"));
                */
                //File.AppendAllText((@"E:\MartinDebug.txt", "before ExportToHttpResponse");
                objRpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, this.Page.Response, true, "ACI01" + DateTime.Now.ToString("yyyyMMdd"));
                //File.AppendAllText((@"E:\MartinDebug.txt", "after ExportToHttpResponse");
            }
        }
    }
}
