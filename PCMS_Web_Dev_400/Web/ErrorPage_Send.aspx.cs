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

public partial class ErrorPage_Send : BasePage
{
    string error = "";    
    protected void Page_Load(object sender, EventArgs e)
    {
        //Master.Save += new MasterForm.SaveEventHandler(Master_Save);

        Master.Title = Resources.Labels.SendErrorReport;
        this.Master.ButtonSave.Text=Resources.Labels.Send;
        error = Request.QueryString["error"].ToString();
        if (Page.IsPostBack)
        {
            if (this.CurrentCommand == Consts.ButtonSave)
            {
                SendError();
            }
        }
        else
        {
            
            
            Master.ShowMessage(Resources.Labels.SendErrorReport);
                       
            ShowError();
            
        }

        Page.ClientScript.RegisterStartupScript(Page.GetType(),"divError",string.Format("{0}=document.getElementById('{1}')",divError.ID,divError.ClientID),true);

        AjaxPro.Utility.RegisterTypeForAjax(typeof(ErrorPage_Send));
    }

    void ShowError()
    {       
                
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

        //while (appException.InnerException != null)
        //{
        //    appException = appException.InnerException;
        //}

        //errMessage.AppendFormat("The following error occurred:<br/><font color=red><b>{0}</b></font><br/><br/>{1}",
        //    appException.Message,
        //    appException.StackTrace.Replace("\n", "<br/>"));

       // string subject="";
        StringBuilder body = new StringBuilder();
        //subject = String.Format("FlexiHR Error Report - {0}",SessionInfo.DefaultSysCompany);
        body.Append("Dear FlexiHR Support,\n\n");
        body.Append("Please find below error report:\n");

        //body.AppendFormat("<br/><font color=red><b>{0}</b></font><br/><br/>{1}",
        //    error,
        //    detailError.Replace("\n", "<br/>"));

        body.AppendFormat("<font color=red><b>{0}</b></font>\n\n", error);        
        //body.AppendFormat("{0}\n", detailError.Replace("\n", "<br/>"));
        //body.AppendFormat("{0}\n", SessionInfo.ErrorStackTrace);

        body.Append("Regards\n");
        body.AppendFormat("{0}\n", SessionInfo.LoginName);
        //body.AppendFormat("{0}\n", SessionInfo.Company);
        
        string fromEmail =String.Empty;//= Config.SupportEmail;
        DataTable dt = PCDb.Db.ExecQuery(String.Format("select email from ps_personal where id={0}", SessionInfo.UserId));
        if (dt != null && dt.Rows.Count == 1)
            fromEmail = dt.Rows[0][0].ToString();

        txtFromName.Text = SessionInfo.UserName;
        txtFromEmail.Text = fromEmail;        
        txtReceiptor.Text = SessionInfo.SupportEmail;
        txtSubject.Text = "";//subject;
        txtErrorInformation.Text = txtError.Text;
    }
   
    //void Master_Save(object sender, EventArgs e)
    //{
    //    if (!CheckForm()) return;
    //    try
    //    {
            
    //    }
    //    catch (Exception ex)
    //    {
           
    //    }
    //}   

    bool CheckForm()
    {        
        if (String.IsNullOrEmpty(txtReceiptor.Text.Trim()))
        {
            Master.ShowMessage(txtReceiptor.RequiredErrorMessage);
            txtReceiptor.Focus();
            return false;
        }
        if (String.IsNullOrEmpty(txtSubject.Text.Trim()))
        {
            Master.ShowMessage(txtSubject.RequiredErrorMessage);
            txtSubject.Focus();
            return false;
        }
        if (String.IsNullOrEmpty(txtBody.Text.Trim()))
        {
            Master.ShowMessage(txtBody.RequiredErrorMessage);
            txtBody.Focus();
            return false;
        }
        return true;
    }

    void SendError()
    {
        

        string subject;
        string body="";
        subject = txtSubject.Text.Trim();

        body += "Dear FlexiHR Support,<br/><br/>";
        body += txtBody.Text.Replace("\n","<br/>".Trim());
        body += "<br/><br/>URL: " + error;
        body += "<br/><br/>" + HttpUtility.UrlDecode(txtError.Text);
        body += ("<br/><br/>Regards<br/>");
        body += String.Format("{0}<br/>", SessionInfo.UserName);
        //body += String.Format("{0}<br/>", SessionInfo.Company);

        string receiptor = txtReceiptor.Text.Trim();
        string fromEmail = String.Empty;
        //DataTable dt = PCDb.Db.ExecQuery(String.Format("select email from ps_personal where id={0}", SessionInfo.UserId));
        //if (dt != null && dt.Rows.Count == 1)
        //    fromEmail = dt.Rows[0][0].ToString();
        fromEmail = txtFromEmail.Text;
        
        SimpleMail sm = new SimpleMail(Config.SMTPServer,fromEmail, txtFromName.Text.Trim());
        sm.UserName = Config.SMTPUserName;
        sm.Password = Config.SMTPPassword;
        
        try
        {
            sm.Send(receiptor, receiptor, subject, body);
            Master.ShowMessage(Resources.Messages.SendSuccessfully);
        }
        catch(Exception ex)
        {
            Master.ShowWarning(ex.Message);
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "sendError", string.Format("alert('{0}');",ex.Message), true);
        }
        finally
        {
        }

    }

    [AjaxPro.AjaxMethod]
    public string GetStaffInfo(string staffid)
    {
        string getInfo="";
        
        return getInfo;
    }


}//end of class
