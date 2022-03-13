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
using SimpleControls.Web;


public partial class ChangePassword : BasePage 
{

    protected override void OnInit(EventArgs e)
    {
        //base.LoginRequired = false;

        base.OnInit(e);

        //_src = Request.QueryString["src"];

        //if (String.IsNullOrEmpty(_src))
        //    _src = Config.AppHomeUrl;

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

        lblTitle.Text = Resources.Labels.ChangePassword + " : " + SessionInfo.LoginName;
        this.Title = Resources.Labels.ChangePassword;

    }

    protected void btnOK_Click(object sender, EventArgs e)
    {
        try
        {
            if (Sc.UserChangePassword(txtOldPwd.Text.Trim(), txtNewPwd1.Text.Trim(), txtNewPwd2.Text.Trim()))
            {
                Note.ShowMessage("Password changed.");
                return;
            }
            else
            {
                Note.ShowWarning("Failed to change password.");
            }
        }
        catch (Exception ex)
        {
            Note.ShowException(ex);
        }
    }


}
