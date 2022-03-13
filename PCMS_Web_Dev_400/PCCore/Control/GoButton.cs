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

namespace PCCore
{
    [ToolboxData("<{0}:SearchButton runat=server></{0}:SearchButton>")]
    [ToolboxBitmap(typeof(Button))]
    public class GoButton :  System.Web.UI.WebControls.Button    //AspButton
    {
        public GoButton()
        {
            //事件顺序
            //(BasePage)InitializeCulture-->(BasePage)Init--> (SearchButton)Init-->  (SearchButton)OnLoad--> (PsExperience.aspx)PageLoad -->(MasterPersonal)PageLoad
        }


        #region RegisterClientVariable
        protected bool _registerClientVariable = false;
        [Category("Behavior"), DefaultValue(false),
        Description("Register a client script variable ")]
        public bool RegisterClientVariable
        {
            get { return _registerClientVariable; }
            set { _registerClientVariable = true; }
        }
        #endregion RegisterClientVariable


        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
        }

        //protected override void OnLoad(EventArgs e)
        //{
        //    //第二
        //    base.OnLoad(e);
        //}

        //protected string GetButton(string function, string title, string image)
        //{

        //    return String.Format("<A onclick=\"{0}\" href=\"#\"><IMG class='ToolbarImage' title=\"{1}\" src=\"{2}/{3}\"></A>", function, title, _imageBaseUrl, image);
        //}

        protected override void Render(HtmlTextWriter writer)
        {
            //StringBuilder sb = new StringBuilder();

            //if (this.Visible)
            //    sb.Append(GetButton("Refresh();", "Search", "go.gif"));

            //writer.Write(sb.ToString());

            //base.CssClass = "ToolbarImage";
            //base.Style.Add("cssClass","btngo02");
            //base.Attributes.Add("Value", "Go");
            base.Attributes.Add("alt", HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Search").ToString());
            base.Attributes.Add("class", "btngo02");
            
            //base.Style.Add("class", "btngo02");
            //base.Attributes.Add("onclick", "setCommand(COMMAND_REFRESH);if(!Refresh()) return false;");
            base.Text = "GO";
            //string _imageBaseUrl = Config.GetImageBaseUrl(Page.Theme);
            //base.ImageUrl = String.Format("{0}/go.gif", _imageBaseUrl);
            //base.OnClientClick = "Refresh();return false";

            if (_registerClientVariable)
            {
                Page.ClientScript.RegisterStartupScript(Page.GetType(), this.ClientID, String.Format("var {0}=document.getElementById('{1}');", this.ID, this.ClientID), true);
            }

            base.Render(writer);

        }


    }
}