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
    [ToolboxData("<{0}:Panel runat=server></{0}:Panel>")]
    [ToolboxBitmap(typeof(Panel))]
    public class Panel : System.Web.UI.WebControls.Panel
    {
        public Panel()
        {

        }
        #region RegisterClientVariable
        protected bool _RegisterClientVariable = false;
        [Category("Behavior"), DefaultValue("false"),
        Description("是否激活自注册脚本属性，控制 Register a client script variable , add by jawance")]
        public bool RegisterClientVariable
        {
            get { return _RegisterClientVariable; }
            set { _RegisterClientVariable = value; }
        }
        #endregion RegisterClientVariable

        #region Resource
        ClientScriptManager _cs = null;
        Type _t = typeof(Panel);
        #endregion Resource

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _cs = Page.ClientScript;
            this.RegisterClientScripts();
        }



        protected void RegisterClientScripts()
        {
            SimpleJS.RegisterClientScripts(Page, SimpleJS.JSTypes.Common);

            if (this.RegisterClientVariable)
            {
                _cs.RegisterStartupScript(_t, this.ClientID,
                    String.Format("var {0}=document.getElementById('{1}');", this.ID, this.ClientID),
                    true);
            }

        }
    }
}
