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
using System.Text;
using SimpleControls;
using SimpleControls.SimpleDatabase;
using SimpleControls.Web;


public partial class RDP1 : BasePage
{
    string appUrl;
    string themeUrl;
    string Current_Command = "";
    protected override void OnInit(EventArgs e)
    {
        string cssLink = null;
        switch (SessionInfo.CurrentFunction.ToUpper())
        {
            case Consts.ScDropdownSetting:
            case Consts.ScGroupRightFunc:
            case Consts.ScUserRightFunc:
            case Consts.ScMandatorySetting:
            case Consts.ScFunctionFunc:
            case Consts.ScUserFunctionFunc:
                cssLink = SimpleWebUtils.GetCssLink(Config.AppBaseUrl + "/control/gridview-security-nopagedown.css");
                break;
            default:
                cssLink = SimpleWebUtils.GetCssLink(Config.AppBaseUrl + "/control/gridview-security.css");
                break;
        }
        LiteralControl css = new LiteralControl(cssLink);
        css.ID = "cssID";
        Page.Header.Controls.Add(css);
        
        base.LoginRequired = true;
        // base.ShowWebMenu = false;
        
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
       
        
    }

    void CheckCommand()
    {
        switch (Current_Command)
        {
            case Consts.Submit:
                ProjectSelected();
                break;
        }
    }

    void initImage()
    {
      
    }
    void SearchProject()
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        
    }
    protected void btnProjSearch_Click(object sender, ImageClickEventArgs e)
    {
      
    }
    void ProjectSelected()
    {
        
       
    }
    void setProjectDataSource(string projfilter)
    {
      

    }
    protected void chkAllProj_CheckedChanged(object sender, EventArgs e)
    {
    }
}
