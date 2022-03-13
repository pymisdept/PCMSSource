using System;
using System.Data;
using System.Data.SqlClient;
//using System.Data.OracleClient;
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

    [ToolboxData("<{0}:Note runat=server></{0}:Note>")]
    [ToolboxBitmap(typeof(Note))]
    public class PopNote : SimpleNote
    {


        public PopNote()
        {            
        }

        const string ResNotePopInfo = "PCCore.Control.PopNote.pop_ico.gif";
        const string ResNoteCssPop = "PCCore.Control.PopNote.pop_note.css";

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _cs = Page.ClientScript;
            this.popNoteRegisterClientScripts();
        }

        #region RegisterClientVariable

        #region Resource
        ClientScriptManager _cs = null;
        Type _t = typeof(PopNote);
        #endregion Resource


        protected bool _RegisterClientVariable = false;
        [Category("Behavior"), DefaultValue(false),
        Description("定义是否注册客户端脚本  , add by jawance")]
        public bool RegisterClientVariable
        {
            get { return _RegisterClientVariable; }
            set { _RegisterClientVariable = value; }
        }


        protected void popNoteRegisterClientScripts()
        {
            SimpleJS.RegisterClientScripts(Page, SimpleJS.JSTypes.Common);

            if (this.RegisterClientVariable)
            {
                _cs.RegisterStartupScript(_t, this.ClientID,
                    String.Format("var {0}=document.getElementById('{1}');", this.ID, this.ClientID),
                    true);
            }

        }


        #endregion RegisterClientVariable


        protected override void Render(HtmlTextWriter output)
        {
            output.WriteBeginTag("div");
            output.WriteAttribute("id", this.ClientID);
            output.WriteAttribute("name", this.ClientID);
            //output.WriteAttribute("class", "snContainer");
            output.WriteAttribute("infoImage", GetResource(ResNotePopInfo));
            output.WriteAttribute("infoWarning", GetResource(ResNotePopInfo));           
            if (!this.Visible)
            {
                output.WriteAttribute("style", "display:none");
            }
            output.WriteAttribute("title", ToolTip);
            output.Write(HtmlTextWriter.TagRightChar);

            string imageId = GetImageClientID();
            string imageUrl = GetPopResource(ResNotePopInfo);
            
            output.Write("<table width='100%' height='100%' border='0' cellpadding='0' cellspacing='0'><tr><td width='44px'>");
            output.WriteBeginTag("img");
            output.WriteAttribute("id", imageId);
            output.WriteAttribute("name", imageId);
            output.WriteAttribute("src", imageUrl);
            //output.WriteAttribute("class", "snImage");
            output.WriteAttribute("width", "44px");
            output.WriteAttribute("height", "44px");
            output.Write(HtmlTextWriter.SelfClosingTagEnd);
            output.Write("</td><td style='padding:9 0 11 0'>");
            output.Write("<table width='100%' height='24' border='0' cellpadding='0' cellspacing='0' class='pop_info'><tr><td>");            
            //output.Write("<table width='100%' height='24' border='0' cellpadding='0' cellspacing='0' class='pop_info_error'><tr><td>");            
            output.Write(SimpleWebUtils.ReplaceNewLineToBR(Text));
            output.Write("</td></tr></table>");
            output.Write("</td></tr></table>");

            output.WriteEndTag("div");            
        }

        protected string GetPopResource(string res)
        {
            string url = _cs.GetWebResourceUrl(_t, res);
            return url;
        }

        public void HRShowException(Exception ex)
        {
            base.ShowWarning(this.ErrorMessage(ex), String.Format("{0}\n{1}", ex.Message, ex.StackTrace));
            //base.ShowWarning("<Font Color=Red>" + this.ErrorMessage(ex) + "</Font>", String.Format("{0}\n{1}", ex.Message, ex.StackTrace)); 
        }
       
        public string ErrorMessage(Exception ex)
        {
            string _message = String.Empty;
            switch (ex.GetType().Name)
            {
                case "NotOwnerException":
                    _message = HttpContext.GetGlobalResourceObject(Consts.ResourcesMessages, "Accessdenied").ToString();
                    break;

                //case "TransactionException":
                //    break;

                //case "MissingPrimaryKeyValueException":
                //    break;


                case "SqlException":
                    SqlException mySqlEx = (SqlException)ex;
                    switch (mySqlEx.Number)
                    {
                        case 2601:// Duplicate key
                            _message= HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Duplicatekey").ToString();
                            break;
                        case 2627:
                            _message = "Violation of UNIQUE KEY constraint .Cannot insert duplicate key in object";
                            break;
                        case 547:
                            _message = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "DeleteStatement").ToString();
                            break;
                        case 8114:
                            _message = "Error converting data type varchar to numeric.";
                            break;
                        case 8152:
                            _message = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "InputTooLang").ToString();
                            break;
                        default :
                            _message = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "UnknowSQLError").ToString();
                            break;
                    }
                    break;
                case "OracleException":
     
                    break;
                case "Exception":
                    _message = ex.Message.ToString();
                    break;

                default:  
                    _message = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "UnknowError").ToString();
                    break;
            }

            return _message;
        }

    }


}
