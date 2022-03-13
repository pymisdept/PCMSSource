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

public partial class MasterBase : MasterBasePage
{

    protected void Page_Load(object sender, EventArgs e)
    {
        SessionInfo.CurrentTheme = Page.Theme;
        if (SessionInfo.CurrentFunction.ToUpper() == "DEFAULT")
        {
            this.Footer.Visible = false;
            divMain.Attributes["style"] = "height:100%;width:100%;overflow:auto;";
        }
        else
        {
            divMain.Attributes["style"] = "height:100%;width:100%;overflow:hidden;";
        }

    }
    
}
