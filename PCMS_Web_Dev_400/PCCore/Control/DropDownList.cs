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
    [ToolboxData("<{0}:DropDownList runat=server></{0}:DropDownList>")]
    [ToolboxBitmap(typeof(DropDownList))]
    //public class DropDownList: System.Web.UI.WebControls.DropDownList
    public class DropDownList: SimpleDropDownList   
    {
        const string ResDropDownlistCssGrey_DF = "DropDownList";
        const string ResDropDownlistCssGrey_EN = "DropDownListEN";
        const string ResDropDownlistCssGrey_ZH = "DropDownListZH";

        public DropDownList()
        {            
            base.Attributes["onchange"] = "javascript:setChange();";
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

        #region UserDefinedStyle
        protected string _style = String.Empty;
        [Category("Behavior"), DefaultValue(""),
       Description("定义控件的对象样式")]
        public string UserDefinedStyle
        {
            get { return _style; }
            set { _style = value; }
        }
        #endregion UserDefinedStyle
        //#region RegisterClientVariable
        //protected bool _registerClientVariable = false;
        //[Category("Behavior"), DefaultValue(false),
        //Description("Register a client script variable , add by jawance")]
        //public bool RegisterClientVariable
        //{
        //    get { return _registerClientVariable; }
        //    set { _registerClientVariable = true; }
        //}
        //#endregion RegisterClientVariable

        //#region No Space
        //protected bool _NoSpace = false;
        //[Category("Behavior"), DefaultValue(false),
        // Description("是否显示'*'号")]
        //public bool NoSpace
        //{
        //    get { return _NoSpace; }
        //    set { _NoSpace = value; }
        //}
        //#endregion No Space


        protected override void OnInit(EventArgs e)
        {
            this.EmptyValue = Consts.DropDownNoneValue.ToString();
            object o = SimpleCache.Get(ComFunction2.SysSystemSet);
            if (o != null)
            {
                #region 2007-03-21
                //if (!String.IsNullOrEmpty(this.ObjectId))
                //{
                //    Hashtable row = o as Hashtable;
                //    string strKey = this.ObjectId.Trim();
                //    if (row.ContainsKey(strKey))
                //    {
                //        if (row[strKey].ToString() == "1")
                //        {
                //            Required = true;
                //            RequiredErrorMessage = String.Format(HttpContext.GetGlobalResourceObject(Consts.ResourcesMessages, "PleaseSelect").ToString() + " " + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, row[strKey + "_Error"].ToString()).ToString()) + ".";
                //        }
                //        else
                //            Required = false;
                //    }
                //}
                #endregion
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
            }
            else
            {
                ShowRequiredStar = false;
            }

            if (this.CssClass == "Invisible" || this.Width == 0)
            {
                Required = false;
                ShowRequiredStar = false;
            }

            #region InitUse what culture nature language in Css Style
            string resCss = ResDropDownlistCssGrey_DF;
            switch (System.Threading.Thread.CurrentThread.CurrentCulture.Name.ToUpper())
            {
                case "EN-US":
                    resCss = ResDropDownlistCssGrey_EN;
                    break;
                case "ZH-CN":
                    resCss = ResDropDownlistCssGrey_ZH;
                    break;
                case "ZH-TW":
                    resCss = ResDropDownlistCssGrey_ZH;
                    break;
                default:
                    resCss = ResDropDownlistCssGrey_DF;
                    break;
            }
            this.CssClass = resCss;
            #endregion

            base.OnInit(e);
        }

        protected override void Render(HtmlTextWriter writer)
        {
            //if (SessionInfo.ShowStar == "1")
            //{
            //    object o = SimpleCache.Get("SY_SYSTEMSET");
            //    if (o != null)
            //    {
            //        if (!String.IsNullOrEmpty(this.ObjectId))
            //        {
            //            Hashtable row = o as Hashtable;
            //            string strKey = this.ObjectId.Trim();
            //            if (row.ContainsKey(strKey))
            //            {
            //                if (row[strKey].ToString() == "1")
            //                {
            //                    Required = true;
            //                    ShowRequiredStar = true;
            //                    RequiredErrorMessage = String.Format(HttpContext.GetGlobalResourceObject(Consts.ResourcesMessages, "PleaseInput").ToString() + " " + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, row[strKey + "_Error"].ToString()).ToString());
            //                }
            //                else
            //                    ShowRequiredStar = false;
            //            }
            //        }
            //    }                
            //}
            //else
            //{
            //    ShowRequiredStar = false;
            //}

            //
           
            //
            //if (_registerClientVariable)
            //{
            //    Page.ClientScript.RegisterStartupScript(Page.GetType(), this.ClientID, String.Format("var {0}=document.getElementById('{1}');", this.ID, this.ClientID), true);
            //}
            if (!String.IsNullOrEmpty(_style))
            {
                base.Attributes["style"] = _style;
            }
            base.Attributes["UserDefineErrorMessage"] = UserDefineErrorMessage;
            base.Render(writer);
            //if (!ShowRequiredStar && NoSpace)
            //{
            //    Label lblSpace = new Label();
            //    lblSpace.Text = "&nbsp;&nbsp;&nbsp;&nbsp;";
            //    lblSpace.Font.Bold = true;
            //    lblSpace.Font.Size = this.Font.Size;
            //    lblSpace.RenderControl(writer);
            //}
        }

    }//end of class
}
