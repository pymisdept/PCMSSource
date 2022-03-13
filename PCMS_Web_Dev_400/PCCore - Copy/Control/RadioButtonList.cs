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
    [ToolboxData("<{0}:RadioButtonList runat=server></{0}:RadioButtonList>")]
    [ToolboxBitmap(typeof(RadioButtonList))]
    public class RadioButtonList: System.Web.UI.WebControls.RadioButtonList
    {
        const string ResRadioButtonListCssGrey_DF = "RadioButtonList";
        const string ResRadioButtonListCssGrey_EN = "RadioButtonListEN";
        const string ResRadioButtonListCssGrey_ZH = "RadioButtonListZH";

        public RadioButtonList()
        {
            base.Attributes["onclick"] = "javascript:setChange();";
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _cs = Page.ClientScript;
            this.RegisterClientScripts();

            #region InitUse what culture nature language in Css Style
            string resCss = ResRadioButtonListCssGrey_DF;
            switch (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToUpper())
            {
                case "EN-US":
                    resCss = ResRadioButtonListCssGrey_EN;
                    break;
                case "ZH-CN":
                    resCss = ResRadioButtonListCssGrey_ZH;
                    break;
                case "ZH-TW":
                    resCss = ResRadioButtonListCssGrey_ZH;
                    break;
                default:
                    resCss = ResRadioButtonListCssGrey_DF;
                    break;
            }
            this.CssClass = resCss;
            #endregion
        }

       #region Resource
        ClientScriptManager _cs = null;
        Type _t = typeof(RadioButtonList);
        #endregion Resource


        protected bool _RegisterClientVariable = false;
        [Category("Behavior"), DefaultValue(false),
        Description("定义是否注册客户端脚本  , add by jawance")]
        public bool RegisterClientVariable
        {
            get { return _RegisterClientVariable; }
            set { _RegisterClientVariable = value; }
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



    }//end of class



}
