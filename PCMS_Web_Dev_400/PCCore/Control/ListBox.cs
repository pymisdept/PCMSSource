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
    [ToolboxData("<{0}:ListBox runat=server></{0}:ListBox>")]
    [ToolboxBitmap(typeof(ListBox))]
    public class ListBox : System.Web.UI.WebControls.ListBox
    {
        const string ResListBoxCssGrey_DF = "ListBox";
        const string ResListBoxCssGrey_EN = "ListBoxEN";
        const string ResListBoxCssGrey_ZH = "ListBoxZH";

        public ListBox()
        {

        }
        #region RegisterClientVariable
        protected bool _RegisterClientVariable = false;
        [Category("Behavior"), DefaultValue("false"),
        Description("是否激活自注册脚本属性，控制 Register a client script variable ")]
        public bool RegisterClientVariable
        {
            get { return _RegisterClientVariable; }
            set { _RegisterClientVariable = value; }
        }
        #endregion RegisterClientVariable

        #region Resource
        ClientScriptManager _cs = null;
        Type _t = typeof(ListBox);
        #endregion Resource

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _cs = Page.ClientScript;
            this.RegisterClientScripts();

            #region InitUse what culture nature language in Css Style
            string resCss = ResListBoxCssGrey_DF;
            switch (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToUpper())
            {
                case "EN-US":
                    resCss = ResListBoxCssGrey_EN;
                    break;
                case "ZH-CN":
                    resCss = ResListBoxCssGrey_ZH;
                    break;
                case "ZH-TW":
                    resCss = ResListBoxCssGrey_ZH;
                    break;
                default:
                    resCss = ResListBoxCssGrey_DF;
                    break;
            }
            this.CssClass = resCss;
            #endregion
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
