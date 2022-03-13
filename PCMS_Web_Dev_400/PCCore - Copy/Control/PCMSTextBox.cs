
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
    public class PCMSTextBox : System.Web.UI.WebControls.TextBox
               
    //public class TextBox : System.Web.UI.WebControls.TextBox
    {
        public SimpleTextBox.DataTypes DataType{ get ; set; }
        
        public bool Required { get ; set; }
        public string RequiredErrorMessage { get; set; }
        public bool ShowRequiredStar { get; set; }
        public string ValidationErrorMessage { get; set; }
        public PCMSTextBox()
        {
            
            //base.Attributes["onchange"] = "javascript:setChange();";
            
            //if (this.DataType == DataTypes.Currency)
            //{
            //base.Attributes["onkeydown"] = "javascript:InputOnlyCurrency();";
            //}
            //if (base.DataType == DataTypes.Integer)
            //{
            //    base.Attributes["onkeydown"] = "javascript:InputOnlyInteger();";
            //}
            //if (base.DataType == DataTypes.Double)
            //{
            //    base.Attributes["onkeydown"] = "javascript:InputOnlyCurrency();";
            //}

        }
        #region UserDefineErrorMessage
        protected string _UserDefineErrorMessage = String.Empty;
        [Category("Behavior"), DefaultValue(""),
       Description("定义自己的额外错误信息  , add by jawance")]
        public string UserDefineErrorMessage
        {
            get { return _UserDefineErrorMessage; }
            set { _UserDefineErrorMessage = value; }
        }
        #endregion UserDefineErrorMessage

        #region InputUserDefineErrorMessage
        protected string _InputUserDefineErrorMessage = String.Empty;
        [Category("Behavior"), DefaultValue(""),
       Description("Input定义自己的额外错误信息  ")]
        public string InputUserDefineErrorMessage
        {
            get { return _InputUserDefineErrorMessage; }
            set { _InputUserDefineErrorMessage = value; }
        }
        #endregion InputUserDefineErrorMessage

        #region IsRequired
        protected string _IsRequired = String.Empty;
        [Category("Behavior"), DefaultValue("N"),
           Description("是否设置了必录，提供给客户端检测")]
        public string IsRequired
        {
            get { return _IsRequired; }
            set { _IsRequired = value; }
        }
        #endregion IsRequired

        #region ValidationErrorMessage
        // protected string _ValidationErrorMessage = String.Empty;
        // [Category("Behavior"), DefaultValue(""),
        //Description("定义自己的额外错误信息")]
        // public string ValidationErrorMessage
        // {
        //     get { return _ValidationErrorMessage; }
        //     set { _ValidationErrorMessage = value; }
        // }
        #endregion ValidationErrorMessage

        //#region No Space
        //protected bool _NoSpace =false;
        //[Category("Behavior"), DefaultValue(false),
        // Description("是否显示'*'号")]
        //public bool NoSpace
        //{
        //    get { return _NoSpace; }
        //    set { _NoSpace = value; }
        //}
        //#endregion No Space

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

        #region newChange
        protected bool _newchange = false;
        [Category("Behavior"), DefaultValue(false),
       Description("定义控件的对象ID")]
        public bool newChange
        {
            get { return _newchange; }
            set { _newchange = value; }
        }
        #endregion newChange

        /// <summary>
        ///  use for sql string .
        /// </summary>
        string _DBstr = "";
        public string DBText
        {
            get
            {
                _DBstr = base.Text.Replace("'", "''");
                _DBstr = _DBstr.Replace(" ", "%");//Keywords Search 2007-05-07
                return _DBstr;
            }
        }

        string _clientBeforeChange = null;
        public string ClientBeforeChange
        {
            get { return _clientBeforeChange; }
            set { _clientBeforeChange = value; }
        }

        string _clientAfterChange = null;
        public string ClientAfterChange
        {
            get { return _clientAfterChange; }
            set { _clientAfterChange = value; }
        }

        string _clientBeforeBlur = null;
        public string ClientBeforeBlur
        {
            get { return _clientBeforeBlur; }
            set { _clientBeforeBlur = value; }
        }

        string _clientAfterBlur = null;
        public string ClientAfterBlur
        {
            get { return _clientAfterBlur; }
            set { _clientAfterBlur = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            RegionScript();
            object o = SimpleCache.Get(ComFunction2.SysSystemSet);
            if (o != null)
            {
                if (!String.IsNullOrEmpty(this.ObjectId))
                {
                    string strKey = this.ObjectId.Trim();
                    DataTable tb = o as DataTable;
                    DataView dv = new DataView(tb);

                    dv.RowFilter = string.Format("ObjectID={0}", strKey);
                    if (dv.ToTable().Rows.Count > 0)
                    {
                        DataRow row = dv.ToTable().Rows[0];

                        if (row["Required"].ToString() == "1")
                        {
                            if (row["ResourceKey"] != null)
                            {
                                Required = true;
                                IsRequired = "Y";
                                //加上判断，是否在页面已经设置了比这个拼在一起的提示更好的。为空的话，用系统默认的。 
                                if (RequiredErrorMessage == string.Empty)
                                {
                                    RequiredErrorMessage = HttpContext.GetGlobalResourceObject(Consts.ResourcesMessages, "PleaseInput").ToString()
                                        + " " + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, row["Resourcekey"].ToString()).ToString() + ".";
                                }
                            }
                            else
                            {
                                Required = true;
                                RequiredErrorMessage = HttpContext.GetGlobalResourceObject(Consts.ResourcesMessages, "PleaseInput").ToString() + " [Data] .";
                            }
                        }
                        else
                        {
                            Required = false;
                            IsRequired = "N";
                        }
                    }
                }
            }

            if (SessionInfo.ShowStar == "1")
            {
                if (this.ObjectId == "10000")
                {
                    Required = false;
                    ShowRequiredStar = false;
                }

                if (Required == true)
                {
                    ShowRequiredStar = true;
                }
                ////2007-05-24 Currency 就不用show * ，奇怪的代码注销掉。
                //if (DataType == DataTypes.Currency)
                //{
                //    ShowRequiredStar = false;
                //}
            }
            else
            {
                ShowRequiredStar = false;
            }

            if (this.CssClass == "Invisible" || this.Width == 0)
            {
                //Required = false;
                ShowRequiredStar = false;
            }

            base.OnInit(e);
            AjaxPro.Utility.RegisterTypeForAjax(typeof(PCCore.TextBox));
        }
        protected override void Render(HtmlTextWriter output)
        {
            //ShowRequiredStar = _showrequridstar;

            base.Attributes["UserDefineErrorMessage"] = UserDefineErrorMessage;
            base.Attributes["InputUserDefineErrorMessage"] = InputUserDefineErrorMessage;
            base.Attributes["ValidationErrorMessage"] = ValidationErrorMessage;
            base.Attributes["newChange"] = _newchange.ToString();
            base.Attributes["ObjectId"] = _objectid;
            base.Attributes["IsRequired"] = IsRequired;

            switch (DataType)
            { 
                case SimpleTextBox.DataTypes.Integer:
                    this.ValidationErrorMessage = ComFunction2.GetGlobalResourceObject_ByStringID("Common", "PlsInputInteger");
                    break;
            }

            string tmpbefore = "";
            string tmpafter = "";
            switch (DataType)
            {
                case SimpleTextBox.DataTypes.Double:
                    base.Attributes["onkeypress"] = "javascript:Currencykeypress(this);";
                    base.Attributes["onkeydown"] = "javascript:Currencykeydown(this);";

                    if (!String.IsNullOrEmpty(this.ClientBeforeBlur))
                    {
                        tmpbefore += this.ClientBeforeBlur + ";";
                    }
                    if (!String.IsNullOrEmpty(this.ClientAfterBlur))
                    {
                        tmpafter += this.ClientAfterBlur + ";";
                    }
                    //Karrson: Remark
                    //base.Attributes["onblur"] = String.Format("{0}ActiveOut(this);{1}", tmpbefore, tmpafter);

                    //base.Attributes["onchange"] = "ActiveOut(this)";
                    if (!String.IsNullOrEmpty(this.ClientBeforeChange))
                    {
                        tmpbefore += this.ClientBeforeChange + ";";
                    }
                    if (!String.IsNullOrEmpty(this.ClientAfterChange))
                    {
                        tmpafter += this.ClientAfterChange + ";";
                    }
                    //base.Attributes["onchange"] = String.Format("{0}ActiveOut(this);{1}", tmpbefore, tmpafter);

                    base.CssClass = "TextBoxInputPre";
                    break;

                case SimpleTextBox.DataTypes.Integer:
                    //base.Attributes["onmouseout"] = "ActiveOut(this)";
                    //base.Attributes["onblur"] = "ActiveOut(this)";
                    base.Attributes["onkeypress"] = "javascript:IntegerKeyPress(this);";
                    base.Attributes["onkeydown"] = "javascript:IntegerKeyDown(this);";

                    if (!String.IsNullOrEmpty(this.ClientBeforeBlur))
                    {
                        tmpbefore += this.ClientBeforeBlur + ";";
                    }
                    if (!String.IsNullOrEmpty(this.ClientAfterBlur))
                    {
                        tmpafter += this.ClientAfterBlur + ";";
                    }
                    //base.Attributes["onblur"] = String.Format("{0}{2};{1}", tmpbefore, tmpafter, string.IsNullOrEmpty(base.Attributes["onblur"]) ? "" : base.Attributes["onblur"]);

                    //base.Attributes["onchange"] = "ActiveOut(this)";
                    if (!String.IsNullOrEmpty(this.ClientBeforeChange))
                    {
                        tmpbefore += this.ClientBeforeChange + ";";
                    }
                    if (!String.IsNullOrEmpty(this.ClientAfterChange))
                    {
                        tmpafter += this.ClientAfterChange + ";";
                    }
                    //base.Attributes["onchange"] = String.Format("{0}{2};{1}", tmpbefore, tmpafter, string.IsNullOrEmpty(base.Attributes["onchange"]) ? "" : base.Attributes["onchange"]);

                    base.CssClass = "TextBoxInputPre";
                    break;
                case SimpleTextBox.DataTypes.Currency:
                    base.Attributes["onkeypress"] = "javascript:Currencykeypress(this);";
                    base.Attributes["onkeydown"] = "javascript:Currencykeydown(this);";
                    //base.Attributes["onchange"] = "javascript:Currencychange(this);";
                    if (!String.IsNullOrEmpty(this.ClientBeforeChange))
                    {
                        tmpbefore += this.ClientBeforeChange + ";";
                    }
                    if (!String.IsNullOrEmpty(this.ClientAfterChange))
                    {
                        tmpafter += this.ClientAfterChange + ";";
                    }
                    //base.Attributes["onchange"] = String.Format("{0}Currencychange(this);{1}", tmpbefore, tmpafter);
                    base.CssClass = "TextBoxInputPre";
                    break;
                case SimpleTextBox.DataTypes.Time:
                    base.MaxLength = 5;
                    base.Attributes["onkeypress"] = "javascript:Timekeypress(this);";
                    base.Attributes["onkeydown"] = "javascript:Timekeydown(this);";
                    //base.Attributes["onkeyup"] = "javascript:keyup(this);";
                    //base.Attributes["onblur"] = "javascript:HrAutoSetTimeHHMM(this);";
                    if (!String.IsNullOrEmpty(this.ClientBeforeBlur))
                    {
                        tmpbefore += this.ClientBeforeBlur + ";";
                    }
                    if (!String.IsNullOrEmpty(this.ClientAfterBlur))
                    {
                        tmpafter += this.ClientAfterBlur + ";";
                    }
                    base.Attributes["onblur"] = String.Format("{0}HrAutoSetTimeHHMM(this);{1}", tmpbefore, tmpafter);
                    

                    break;
                case SimpleTextBox.DataTypes.Date:
                    if (!String.IsNullOrEmpty(this.ClientBeforeChange))
                    {
                        tmpbefore += this.ClientBeforeChange + ";";
                    }
                    if (!String.IsNullOrEmpty(this.ClientAfterChange))
                    {
                        tmpafter += this.ClientAfterChange + ";";
                    }
                    base.Attributes["onblur"] = String.Format("{0}CheckDate(this);{1}","","");
                    if (String.IsNullOrEmpty(base.Attributes["onchange"]))
                    {
                        base.Attributes["onchange"] = String.Format("{0}CheckDate(this);{1}", tmpbefore, tmpafter);
                    }
                    else
                    {

                        if (base.Attributes["onchange"].Substring(base.Attributes["onchange"].Length - 1, 1) == ";")
                        {
                            base.Attributes["onchange"] += String.Format("{0}CheckDate(this);{1}", tmpbefore, tmpafter);
                        }
                        else
                        {
                            base.Attributes["onchange"] += ";" + String.Format("{0}CheckDate(this);{1}", tmpbefore, tmpafter);
                        }
                    }
                    base.Attributes["onkeypress"] = "javascript:Datekeypress(this);";
                    base.Attributes["onkeydown"] = "javascript:Datekeydown(this);";
                    break;
                case SimpleTextBox.DataTypes.YearMonth:
                    if (!String.IsNullOrEmpty(this.ClientBeforeBlur))
                    {
                        tmpbefore += this.ClientBeforeBlur + ";";
                    }
                    if (!String.IsNullOrEmpty(this.ClientAfterBlur))
                    {
                        tmpafter += this.ClientAfterBlur + ";";
                    }
                    base.Attributes["onblur"] = String.Format("{0}CheckYMDate(this);{1}", tmpbefore, tmpafter);
                    //base.Attributes["onblur"] = "CheckYMDate(this);";
                    base.Attributes["onkeypress"] = "javascript:Datekeypress(this);";
                    base.Attributes["onkeydown"] = "javascript:Datekeydown(this);";
                    break;
                default:
                    break;
            }
            base.Render(output);

            //if (!ShowRequiredStar && NoSpace)
            //{
            //    Label lblSpace = new Label();
            //    lblSpace.Text = "&nbsp;&nbsp;&nbsp;&nbsp;";
            //    lblSpace.Font.Bold = true;
            //    lblSpace.Font.Size = this.Font.Size;
            //    lblSpace.RenderControl(output);
            //}

        }

        protected void RegionScript()
        {
            string message = HttpContext.GetGlobalResourceObject(Consts.ResourcesMessages, "InputValidData").ToString();
            string cmd = "<script language=\"javascript\">" +
                "\n function CheckDate(v){ \n" +
                "\n if(!SimpleJS.isNullOrEmpty(v.value)){ var objvalue=v.value; \n" +
                //"\n if(v.DataType=='YearMonth') objvalue = v.value+'-01';" +
                "\n     if(!SimpleJS.isDateYMD(objvalue)) { \n" +
                "\n          alert('" + message + "'); \n" +
                "\n            v.value=''; \n" +
                "\n          v.focus();}else{ \n" +
                "\n var re = /(\\/|\\.)/g; \n" +
                "\n v.value = v.value.replace(re,'-'); }} }\n" +
                 "\n function CheckYMDate(v){ \n" +
                "\n if(!SimpleJS.isNullOrEmpty(v.value)){ var objvalue=v.value+'-01'; \n" +
                "\n     if(!SimpleJS.isDateYMD(objvalue)) { \n" +
                "\n          alert('" + message + "'); \n" +
                "\n            v.value=''; \n" +
                "\n          v.focus();}else{ \n" +
                "\n var re = /(\\/|\\.)/g; \n" +
                "\n v.value = v.value.replace(re,'-'); }} }\n" +
                "\n </script> \n";
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "CheckDateFormat", cmd);
        }

        #region Dynaic set SQL-sb 2007-10-31
        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void oSetRestSQLStr(string pVString)
        {
            HttpContext.Current.Session["GO" + SessionInfo.CurrentFunctionID + HttpContext.Current.Session.SessionID] = pVString;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public void oSetSearcSQLStr(string pPISKey, string pVString)
        {
            HttpContext.Current.Session[pPISKey] = pVString;
        }

        [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
        public string oGetSearcSQLStr(string pPISKey)
        {
            string _Value;
            object SessionObj = null;
            if (HttpContext.Current.Session[pPISKey] == null)
            {
                SessionObj = string.Empty;
            }
            else
            {
                SessionObj = HttpContext.Current.Session[pPISKey];
            }
            _Value = SessionObj.ToString();

            return _Value;

        }
        #endregion



        public override string Text
        {
            get
            {
                return base.Text;
            }
            set
            {
                if (this.DataType == SimpleTextBox.DataTypes.Currency && !String.IsNullOrEmpty(value) && value != "N/A")
                {
                    base.Text = Convert.ToDecimal(value).ToString(Consts.CurrencyFormat);
                }
                else
                {
                    base.Text = value;
                }
            }
        }
    }//end of class

   

}
