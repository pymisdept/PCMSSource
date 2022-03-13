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
using System.Security.Permissions;

namespace PCCore
{
    [AspNetHostingPermission(SecurityAction.Demand, Level = AspNetHostingPermissionLevel.Minimal)]
    [AspNetHostingPermission(SecurityAction.InheritanceDemand, Level = AspNetHostingPermissionLevel.Minimal)]
    [ToolboxData("<{0}:Lable runat=server></{0}:Label>")]
    [ToolboxBitmap(typeof(Label))]
    public class Label : System.Web.UI.WebControls.Label
    {
        const string ResLabelCssGrey_EN = "PCCore.Control.Label.Label.css";
        const string ResLabelCssGrey_ZH = "PCCore.Control.Label.Label_zh.css";
        const string ResLabelCssGrey_TW = "PCCore.Control.Label.Label_tw.css";
        const string ResLabelCssGrey_JP = "PCCore.Control.Label.Label_jp.css";

        public Label()
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
        Type _t = typeof(Label);
        #endregion Resource


        #region xLabelStyle
        public enum xLabelStyle
        {
            xButton,
            xLabel,
            xTitle,
            xTitlePersonal,
            xMenu,
            xPanel,
            xSiteMap,
            xNavMenu,
            xPrint
        }

        string _xLabelStyle = string.Empty;
        public xLabelStyle LabelStyle
        {
            set { _xLabelStyle = ((xLabelStyle)value).ToString(); }
        }

        //protected string xCssSet()
        //{
        //    string _sCssStyle = string.Empty;
        //    _sCssStyle = _xLabelStyle;
        //    switch (_xLabelStyle)
        //    {
        //        case "xButton":

        //            break;
        //        case "xLabel":
        //            break;
        //        case "xTitle":
        //            break;
        //        case "xTitlePersonal":
        //            break;
        //        case "xMenu":
        //            break;
        //        case "xPanel":
        //            break;
        //        case "xSiteMap":
        //            break;
        //        case "xNavMenu":
        //            break;
        //    }
        //    return _sCssStyle;
        //}

        protected string GetCssLink()
        {
            string resCss = null;

            switch (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToUpper())
            {
                case "EN-US":
                    resCss = ResLabelCssGrey_EN;
                    break;
                case "ZH-CN":
                    resCss = ResLabelCssGrey_ZH;
                    break;
                case "ZH-TW":
                    resCss = ResLabelCssGrey_TW;
                    break;
                case "JA":
                    resCss = ResLabelCssGrey_JP;
                    break;
                default:
                    resCss = ResLabelCssGrey_EN;
                    break;
            }


            string cssLink = SimpleWebUtils.GetCssLink(_cs.GetWebResourceUrl(_t, resCss));

            return cssLink;

        }
        #endregion


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _cs = Page.ClientScript;
            this.RegisterClientScripts();

            #region Regedit Css file link

            if (Page.Header != null && !string.IsNullOrEmpty(_xLabelStyle))
            {
                string cssId = "PCCorelablecss";
                System.Web.UI.Control c = Page.Header.FindControl(cssId);

                //if (c != null)
                //{
                //    //LiteralControl cssold = new LiteralControl(cssLink);
                //    //cssold.ID = cssId;
                //    Page.Header.Controls.Remove(c);
                //    c = null;
                //}
                if (c == null)
                {
                    string cssLink = GetCssLink();
                    LiteralControl css = new LiteralControl(cssLink);
                    css.ID = cssId;
                    Page.Header.Controls.Add(css);
                }
            }
            if (!string.IsNullOrEmpty(_xLabelStyle))
            {
                if (!string.IsNullOrEmpty(this.CssClass))
                {
                    this.CssClass = string.Format("{0} {1}", this.CssClass, _xLabelStyle);
                }
                else
                {
                    this.CssClass = _xLabelStyle;
                }
            }

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
