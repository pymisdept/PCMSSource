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

public partial class Banner : System.Web.UI.UserControl
{
    protected void Page_Load(object sender, EventArgs e)
    {
        lbldate.Text = System.DateTime.Now.ToString("yyyy-MM-dd HH:mm");
        //lbldate.Text = System.DateTime.Now.ToString("F");
        if (SessionInfo.IsLogin)
        {
            //lblProject.Visible = true;
            //lblProjectDesc.Visible = true;
            lblLoginID.Visible = true;
            
            //LoginID.Text = SessionInfo.LoginName;
            LoginID.Text = SessionInfo.UserName;
            
            if (SessionInfo.CurrentProject != null && SessionInfo.CurrentProject != "")
            {
                
                if (SessionInfo.CurrentProject != Consts.ProjectAll)
                    lblProjectDesc.Text = SessionInfo.CurrentProject + " - " + SessionInfo.CurrentProjectName;
                else
                    lblProjectDesc.Text = Consts.ProjectAll;
            }
            else
            {
                
            }
        }
        else
        {
            lblLoginID.Visible = false;
            LoginID.Visible = false;
            lblProject.Visible = false;
            lblProjectDesc.Visible = false;
        }
        
    }
}
