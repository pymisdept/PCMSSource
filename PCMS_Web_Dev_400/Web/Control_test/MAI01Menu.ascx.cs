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
using SimpleControls.Web;

public partial class MAI01Menu : System.Web.UI.UserControl
{
   
    protected override void OnInit(EventArgs e)
    {

        SLB.DefaultItemImageUrl = "../App_Themes/Default/images/arr_li.gif";
        string cssLink = null;
        switch (SessionInfo.CurrentLanguage.ToUpper())
        {
            case "EN-US":
            case "JA-JP":
                cssLink = SimpleWebUtils.GetCssLink(Config.AppBaseUrl + "/control/navmenu.css");
                break;
            default:
                cssLink = SimpleWebUtils.GetCssLink(Config.AppBaseUrl + "/control/navmenu-zh.css");
                break;
        }
        LiteralControl css = new LiteralControl(cssLink);
        css.ID = "navcssID";
        Page.Header.Controls.Add(css);
        base.OnInit(e);
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        
        ModuleEnable();
    }

    protected void ModuleEnable()
    {
        SLB.Visible = false;
    }
}
