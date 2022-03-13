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
    //public class DeleteButton:Button

    public class DeleteButton : ImageButton
    {
        string themeUrl = "";
        
        public DeleteButton(string id)
        {
            this.ID = id;
            themeUrl = Config.GetThemeBaseUrl(SessionInfo.CurrentTheme);
            //this.Height = Unit.Percentage(20);
            //this.Width = Unit.Percentage(20);
            this.ToolTip = System.Web.HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Delete").ToString();
            this.ImageUrl = themeUrl + "/images/Updelete.gif";
            //this.Text = HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Delete").ToString();
            this.CommandName = Consts.Delete;
            
        }
    }//end of class

   

}

