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

namespace PCCore
{
    [ToolboxData("<{0}:TextBox runat=server></{0}:TextBox>")]
    [ToolboxBitmap(typeof(TextBox))]
    public class TTextBox: System.Web.UI.WebControls.TextBox
    {
        public TTextBox()
        {   
        }

        #region RegisterClientVariable
        protected bool _registerClientVariable = false;
        [Category("Behavior"), DefaultValue(false),
        Description("Register a client script variable")]
        public bool RegisterClientVariable
        {
            get { return _registerClientVariable; }
            set { _registerClientVariable = true; }
        }
        #endregion RegisterClientVariable

        Label _star = null;
        ClientScriptManager _cs = null;

        #region ObjectId
        protected string _objectid = String.Empty;
        [Category("Behavior"), DefaultValue(""),
       Description("定义控件的对象ID")]
        public string ObjectId
        {
            get { return _objectid; }
            set { _objectid = value; }
        }
        #endregion ObjectId
     

        protected override void OnInit(EventArgs e)
        {           
                _star = new Label();
                _star.Text = "*";
                _star.ForeColor = Color.Red;
                _star.Font.Bold = true;
                _star.Font.Size = this.Font.Size;
                Controls.Add(_star);
            
            base.OnInit(e);
        }
        protected override void Render(HtmlTextWriter output)
        {
            //ShowRequiredStar = _showrequridstar;

            base.Attributes["onkeypress"] = "javascript:Currencykeypress(this);";
            base.Attributes["onkeydown"] = "javascript:Currencykeydown(this);";
            base.Attributes["onchange"] = "javascript:Currencychange(this);";
            //base.Attributes["onblur"] = "javascript:HrAutoSetTimeHHMM(this);";
            base.Render(output);
            if (_objectid == "1000")
            {
                output.Write(HtmlTextWriter.SpaceChar);
                _star.RenderControl(output);
            }

            _cs = Page.ClientScript;
            RegisterClientScripts();
           
        }
        protected override void OnPreRender(EventArgs e)
        {
            base.OnPreRender(e);
            //    this.Page.ClientScript.RegisterClientScriptBlock(Type.GetType("text/javascript").Name,this.ID,"<script>var " + this.ID.ToString() + "=document.getElementById(" + this.ClientID.ToString() + ");</script>");
        }

        protected void RegisterClientScripts()
        {
          //  SimpleJS.RegisterClientScripts(Page, SimpleJS.JSTypes.Common);            

            if (this.RegisterClientVariable)
            {
                _cs.RegisterStartupScript(Page.GetType(), this.ClientID,
                    String.Format("var {0}=document.getElementById('{1}');", this.ID, this.ClientID),
                    true);
            }

        }

    }//end of class




}
