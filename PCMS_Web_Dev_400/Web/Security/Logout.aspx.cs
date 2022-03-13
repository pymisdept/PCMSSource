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

public partial class Security_Logout : System.Web.UI.Page
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Sc.Logout();

        string src = Request.QueryString["src"];

        if (String.IsNullOrEmpty(src))
            src = Config.AppHomeUrl;

        Response.Redirect(src,true);
    }
}
