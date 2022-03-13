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

public partial class NotExistPage : BasePage
{
    protected override void OnInit(EventArgs e)
    {
        base.LoginRequired = false;
        base.ShowWebMenu = false;
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {

    }
}
