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
using PCCore;

public partial class Security_ForgotPassword : BasePage
{
    string _src;

    protected override void OnInit(EventArgs e)
    {
        base.LoginRequired = false;

        base.OnInit(e);

        _src = Request.QueryString["src"];

        if (String.IsNullOrEmpty(_src))
            _src = Config.AppHomeUrl;

        //Note.Visible = false;

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        //if (SessionInfo.IsLogin)
        //{
        //    Response.Redirect(_src, true);
        //    return;
        //}

        //if (Page.IsPostBack)
        //{
        //}//end if ispostpack

    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            if (Sc.RecoverPassword(txtEmail.Text))
            {
                Note.ShowMessage("Email sent. Please check your mailbox.");
                return;
            }
            else
            {
                Note.ShowWarning("Failed to recover password.");
            }
        }
        catch (Exception ex)
        {
            Note.ShowException(ex);
        }
    }

    protected void btnCancel_Click(object sender, EventArgs e)
    {
        Response.Redirect(_src);
    }
}
