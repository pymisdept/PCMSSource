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



namespace PCCore
{
    public class CollaspeButton:PCCore.ImageButton
    {
        string themeUrl;
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            PCCore.Common.HRLog.RecordLog("CollaspeButton");

            themeUrl = Config.GetImageBaseUrl(Page.Theme);
            PCCore.Common.HRLog.RecordLog(themeUrl);
            this.ImageUrl = themeUrl + "/images/collapse.jpg";
        }
        public CollaspeButton()
        {
           
            
            
        }
    }//end of class

   

}

