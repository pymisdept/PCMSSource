using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using PCCore;
using SimpleControls;

public partial class ErrorPage : System.Web.UI.Page
{


    protected void Page_Load(object sender, EventArgs e)
    {
        //string logout = "";
        
        System.Text.StringBuilder errMessage = new StringBuilder();
        System.Exception appException = Server.GetLastError();
        if (SessionInfo.IsLogin)
        {
            lblLoginID.Visible = true;
            LoginID.Text = SessionInfo.LoginName;
        }
        else
        {
            lblLoginID.Visible = false;
            LoginID.Visible = false;
        }

        /*
            if (appException is HttpException)
            {
                HttpException checkException = (HttpException)appException;
                switch (checkException.GetHttpCode())
                {
                    case 403:
                        {
                            errMessage.Append("You are not allowed to view that page.");
                            break;
                        }
                    case 404:
                        {
                            errMessage.Append("The page you requested cannot be found.");
                            break;
                        }
                    case 408:
                        {
                            errMessage.Append("The request has timed out");
                            break;
                        }

                    //case 500:
                    //    {
                    //        errMessage.Append("The server cannot fullfill your request.");
                    //        break;
                    //    }
                    default:
                        {
                            errMessage.AppendFormat("The following error occurred:<br/>{0}<br/><br/>{1}", 
                                appException.Message,
                                appException.StackTrace.Replace("\n","<br/>"));
                            //errMessage.Append("Ther server has experienced an error.");
                            break;
                        }

                }
            }
            else
            {
                // The exception was not an HttpException.
                errMessage.AppendFormat("The following error occurred:<br/>{0}<br/><br/>{1}",
                    appException.Message,
                    appException.StackTrace.Replace("\n", "<br/>"));
            }

         */

        StringBuilder sbStack = new StringBuilder();
        if (appException != null)
        {
            sbStack.AppendFormat("{0}\n\n", appException.StackTrace);
            while (appException.InnerException != null)
            {
                appException = appException.InnerException;
                sbStack.AppendFormat("{0}\n\n", appException.StackTrace);
            }
        }

        string message = (appException == null) ? "Unknown error" : appException.Message;
        string stack = sbStack.Replace("\n", "<br/>").ToString();

        if (SessionInfo.CurrentModuleID == "8" || Request.RawUrl.IndexOf("RptViewer.aspx") > 0)
        {
            errMessage.AppendFormat("DataBase: {0}<br/>URL: {1}<br/>Report name: {2}<br/>Report Criteria: </n>{3}<br/><br/>The following error occurred:<br/><font color=red><b>{4}</b></font><br/><br/>{5}",
                SessionInfo.Database, Request.RawUrl, SessionInfo.Report, SessionInfo.UserCondition, message, stack);
        }
        else
        {
            errMessage.AppendFormat("DataBase: {0}<br/>URL: {1}<br/><br/>The following error occurred:<br/><font color=red><b>{2}</b></font><br/><br/>{3}",
                SessionInfo.Database, Request.RawUrl, message, stack);
        }

        lblErrorMessage.Text = errMessage.ToString();
        
        //error.Text = appException.Message;
        //error.Text = Request.RawUrl;
        
        //detailError.Text = appException.StackTrace;
        Server.ClearError();

    }
}
