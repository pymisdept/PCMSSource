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

namespace PCCore
{
    [ToolboxData("<{0}:TextBox runat=server></{0}:ListBar>")]
    [ToolboxBitmap(typeof(Label))]
    public class ListBar:SimpleListBar
    {
        public ListBar()
        {
        }
        protected override void OnInit(EventArgs e)
        {
            //this.Visible = _visible;
            if (SessionInfo.CurrentModule.ToUpper() == Consts.BasicModule || SessionInfo.CurrentModule.ToUpper() == Consts.Staff || SessionInfo.CurrentModule.ToUpper()==Consts.PayrollModule)
            {
            }
            else
            {
                this.Visible = false;
                return;
            }

            base.OnInit(e);
        }
    }//end of class
}
