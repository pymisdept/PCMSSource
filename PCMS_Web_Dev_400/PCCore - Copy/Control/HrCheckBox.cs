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

    [ToolboxData("<{0}:CheckBox runat=server></{0}:CheckBox>")]
    [ToolboxBitmap(typeof(CheckBox))]
    public class HrCheckBox : System.Web.UI.WebControls.CheckBox
    {
        public HrCheckBox()
        {
        }

        #region Resource
        ClientScriptManager _cs = null;
        Type _t = typeof(CheckBox);
        #endregion Resource

        #region RegisterClientVariable
        protected bool _RegisterClientVariable = false;
        [Category("Behavior"), DefaultValue(false),
        Description("定义是否注册客户端脚本  , add by jawance")]
        public bool RegisterClientVariable
        {
            get { return _RegisterClientVariable; }
            set { _RegisterClientVariable = value; }
        }
        #endregion RegisterClientVariable

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _cs = Page.ClientScript;
            this.RegisterClientScripts();
            if (SessionInfo.CurrentLanguage.ToUpper() == "EN-US")
            {
                this.Font.Size = FontUnit.Point(8);
            }
            else
            {
                this.Font.Size = FontUnit.Point(11);
            }
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
