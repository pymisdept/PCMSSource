using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using SimpleControls.Web;
using SimpleControls;
using System.Collections;
using System.Web;
using System;
using PCCore;



namespace PCCore.Control.PmsReport
{
    public class AddButton:ImageButton
    {
        string themeUrl;
        public AddButton(String id)
        {
            this.ID = id;
            //this.Text = HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Add").ToString();
            themeUrl = Config.GetThemeBaseUrl(SessionInfo.CurrentTheme);
            this.Height = Unit.Pixel(16);
            this.Width = Unit.Pixel(16);
            this.ToolTip = System.Web.HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "New").ToString();
            this.ImageUrl = themeUrl + "/images/new.gif";
            
            this.CommandName = Consts.Add;
            
        }
    }//end of class

   

}

