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
using System.Collections;


namespace PCCore
{

    [ToolboxData("<{0}:Note runat=server></{0}:Note>")]
    [ToolboxBitmap(typeof(Note))]
    public class Note : SimpleNote
    {


        public Note()
        {
        }

        public enum NoteExectionType
        {
            AnyEr0000 = 0,
            SqlEr2601 = 2601,
            SqlEr2627 = 2627
        }


        //public override void  ShowWarning(string warning, string tooltip)
        //{
        //    base.ShowWarning("<Font Color=Red>" + warning + "</Font>",tooltip);
        //}    

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            _cs = Page.ClientScript;
            this.NoteRegisterClientScripts();
        }

        #region RegisterClientVariable

        #region Resource
        ClientScriptManager _cs = null;
        Type _t = typeof(SimpleNote);
        #endregion Resource


        protected bool _RegisterClientVariable = false;
        [Category("Behavior"), DefaultValue(false),
        Description("定义是否注册客户端脚本  , add by jawance")]
        public bool RegisterClientVariable
        {
            get { return _RegisterClientVariable; }
            set { _RegisterClientVariable = value; }
        }


        protected void NoteRegisterClientScripts()
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

        /// <summary>
        /// 单错误自定义信息
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="pEt"></param>
        /// <param name="pCustomerErrorMessage"></param>
        public void HRShowException(Exception ex, NoteExectionType pEt, string pCustomerErrorMessage)
        {
            NoteExectionTypeObj sNTobj = new NoteExectionTypeObj();
            sNTobj.ET = pEt;
            sNTobj.CustomerErrorMessage = pCustomerErrorMessage;

            base.ShowWarning(this.ErrorMessage(ex, sNTobj));
        }

        /// <summary>
        /// 多对应错误自定义信息
        /// </summary>
        /// <param name="ex"></param>
        /// <param name="pEtOjb"></param>
        public void HRShowException(Exception ex, NoteExectionTypeObj pEtOjb)
        {
            base.ShowWarning(this.ErrorMessage(ex, pEtOjb));
        }


        public void HRShowException(Exception ex)
        {
            NoteExectionTypeObj sNTobj = new NoteExectionTypeObj();
            sNTobj.ET = NoteExectionType.AnyEr0000;
            sNTobj.CustomerErrorMessage = string.Empty;

            base.ShowWarning(this.ErrorMessage(ex, sNTobj), String.Format("{0}\n{1}", ex.Message, ex.StackTrace));
            //base.ShowWarning("<Font Color=Red>" + this.ErrorMessage(ex) + "</Font>", String.Format("{0}\n{1}", ex.Message, ex.StackTrace)); 
        }
        public string ErrorMessage(Exception ex)
        {
            return ErrorMessage(ex, null);
        }


        public string ErrorMessage(Exception ex, NoteExectionTypeObj pETobj)
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
                            if (pETobj != null && (int)pETobj.ET == 2601 && !String.IsNullOrEmpty(pETobj.CustomerErrorMessage))
                            {
                                _message = pETobj.CustomerErrorMessage;
                            }
                            else
                            {
                                _message = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Duplicatekey").ToString();
                            }
                            break;
                        case 2627:
                            if (pETobj != null && (int)pETobj.ET == 2627 && !String.IsNullOrEmpty(pETobj.CustomerErrorMessage))
                            {
                                _message = pETobj.CustomerErrorMessage;
                            }
                            else
                            {
                                _message = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "ViolationofUNIQUEKEY").ToString();
                            }
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
                        case -2:
                            _message = "Sql Connetion Time Out";
                            break;
                        default:
                            _message = ex.Message;// HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "UnknowSQLError").ToString();
                            break;
                    }
                    break;
                case "OracleException":

                    break;
                case "Exception":
                    _message = ex.Message.ToString();
                    break;

                default:
                    _message = ex.Message;// HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "UnknowError").ToString();
                    break;
            }

            return _message;
        }

    }

    /// <summary>
    /// 自定义错误类型和自定义出错信息
    /// </summary>
    public class NoteExectionTypeObj : Hashtable
    {

        private Note.NoteExectionType _ET = Note.NoteExectionType.AnyEr0000;
        private string _CusMessage = string.Empty;

        public Note.NoteExectionType ET
        {
            get
            {
                return _ET;
            }
            set
            {
                _ET = value;
            }
        }

        public String CustomerErrorMessage
        {
            get
            {
                return _CusMessage;
            }
            set
            {
                _CusMessage = value;
            }
        }

    }


}
