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

public partial class Control_ACI08ExportPDF : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        if (!Page.IsPostBack)
        {
            CostCodeFr.Text = "00000";
	        CostCodeTo.Text = "ZZZZZ";
            PrdChoice.Text = "2";
	        PeriodFr.Text = "201102";
        }
    }
    static public bool IsNumeric(string target)
    {
        if (!Regex.IsMatch(target, "^[0-9.]+$"))
        {
            return false;
        }
        return true;
    }
    static public bool IsAlphabet(string target)
    {
        if (!Regex.IsMatch(target, "^[0-9a-zA-Z.]+$"))
        {
            return false;
        }
        return true;
    }
    protected void ExportButton_Click(object sender, EventArgs e)
    {
        Error_Label.Text = "";
        if (String.IsNullOrEmpty(CostCodeFr.Text) == true)
        {
            Error_Label.Text = "Cost Code From can not be blank";
            return;
        }
        if (String.IsNullOrEmpty(CostCodeTo.Text) == true)
        {
            Error_Label.Text = "Cost Code To can not be blank";
            return;
        }
        if (String.IsNullOrEmpty(PrdChoice.Text) == true)
        {
            Error_Label.Text = "PrdChoice can not be blank";
            return;
        }
        if (String.IsNullOrEmpty(PeriodFr.Text) == true)
        {
            Error_Label.Text = "Period From can not be blank";
            return;
        }
        if (IsAlphabet(CostCodeFr.Text) == false)
        {
            Error_Label.Text = "Cost code From is wrong alphabet";
            return;
        }
        if (IsAlphabet(CostCodeTo.Text) == false)
        {
            Error_Label.Text = "Cost code To is wrong alphabet";
            return;
        }
        if (IsAlphabet(PeriodFr.Text) == false)
        {
            Error_Label.Text = "PeriodFr is wrong alphabet";
            return;
        }
        if (IsAlphabet(PrdChoice.Text) == false)
        {
            Error_Label.Text = "PrdChoice is wrong alphabet";
            return;
        }
        if (CostCodeFr.Text.Length != 5 || CostCodeTo.Text.Length != 5)
        {
            Error_Label.Text = "Cost code length should be 5";
            return;
        }
        if (IsNumeric(PrdChoice.Text) == false)
        {
            Error_Label.Text = "PrdChoice should be numeric";
            return;
        }
        if (PeriodFr.Text.Length == 6)
        {
            DateTime result = DateTime.Now;
            if (DateTime.TryParse("1-" + PeriodFr.Text.Substring(4, 2) + "-" + PeriodFr.Text.Substring(0, 4), out result) == false)
            {
                Error_Label.Text = "Period From is not correct date format";
                return;
            }
        }
        if(String.IsNullOrEmpty(ConfigurationManager.AppSettings["ACI03CrystalReportPath"]) == true)
        {
            Error_Label.Text = "configuration file ACI03CrystalReportPath can not be blank";
            return;
        }
        if(String.IsNullOrEmpty(ConfigurationManager.AppSettings["BackEndServer"]) == true)
        {
            Error_Label.Text = "configuration file BackEndServer can not be blank";
            return;
        }
        if(String.IsNullOrEmpty(ConfigurationManager.AppSettings["BackEndServerUser"]) == true)
        {
            Error_Label.Text = "configuration file BackEndServerUser can not be blank";
            return;
        }
        if(String.IsNullOrEmpty(ConfigurationManager.AppSettings["BackEndServerPassword"]) == true)
        {
            Error_Label.Text = "configuration file BackEndServerPassword can not be blank";
            return;
        }
        if (String.IsNullOrEmpty(ConfigurationManager.AppSettings["PCMS800"]) == true)
        {
            Error_Label.Text = "configuration file PCMS800 can not be blank";
            return;
        }

        if (!String.IsNullOrEmpty(Request.QueryString["ProjectCode"]) && !String.IsNullOrEmpty(Request.QueryString["UserID"])
            && !String.IsNullOrEmpty(CostCodeFr.Text)
            && !String.IsNullOrEmpty(CostCodeTo.Text)
            && !String.IsNullOrEmpty(PrdChoice.Text)
            && !String.IsNullOrEmpty(PeriodFr.Text)
            )
        {
            //File.AppendAllText((@"E:\MartinDebug.txt", "before Request.QueryString[ProjectCode].ToString()");
            string projValue = Request.QueryString["ProjectCode"].ToString();

            //File.AppendAllText((@"E:\MartinDebug.txt", "after Request.QueryString[ProjectCode].ToString()");

            ReportDocument objRpt = new ReportDocument();
            //File.AppendAllText((@"E:\MartinDebug.txt", "after new ReportDocument()");

            string reportPath = ConfigurationManager.AppSettings["ACI08CrystalReportPath"].ToString();

            //File.AppendAllText((@"E:\MartinDebug.txt", "ConfigurationManager.AppSettings[ACI03CrystalReportPath");

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
            objRpt.SetParameterValue(1, CostCodeFr.Text);
            objRpt.SetParameterValue(2, CostCodeTo.Text);
            objRpt.SetParameterValue(3, PeriodFr.Text);
            objRpt.SetParameterValue(4, Convert.ToInt32(PrdChoice.Text));
            objRpt.SetParameterValue(5, projValue.Replace("?mode=New", "").Replace("?mode=New&", "").Replace("?mode=", ""));

            //File.AppendAllText((@"E:\MartinDebug.txt", projValue.ToString());

            CrTables = objRpt.Database.Tables;

            foreach (CrystalDecisions.CrystalReports.Engine.Table CrTable in CrTables)
            {
                crtableLogoninfo = CrTable.LogOnInfo;
                crtableLogoninfo.ConnectionInfo = crConnectionInfo;
                CrTable.ApplyLogOnInfo(crtableLogoninfo);
            }

            //CrystalReportViewer1.ReportSource = objRpt;
            //CrystalReportViewer1.RefreshReport();

            //File.AppendAllText((@"E:\MartinDebug.txt", "before ExportToHttpResponse");
            objRpt.ExportToHttpResponse(CrystalDecisions.Shared.ExportFormatType.PortableDocFormat, this.Page.Response, true, "ACI08" + DateTime.Now.ToString("yyyyMMdd"));
            //File.AppendAllText((@"E:\MartinDebug.txt", "after ExportToHttpResponse");
        }
    }
}
