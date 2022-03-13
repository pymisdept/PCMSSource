using System;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Drawing;
using System.Text;
using System.Web.UI.HtmlControls;

namespace PCCore
{
    [ToolboxData("<{0}:Title runat=server></{0}:Title>")]
    [ToolboxBitmap(typeof(Label))]
    public class Title : WebControl
    {
        Type _t = typeof(Title);

        public Title()
        {
        }
        protected string _mName = null;
        public string ModuleName
        {
            get { return _mName; }
            set { _mName = value; }
        }

        protected string _fName = null;
        public string FunctionName
        {
            get { return _fName; }
            set { _fName = value; }
        }

        protected string _pName = "";
        public string PageName
        {
            get { return _pName; }
            set { _pName = value; }
        }


        protected bool _IsAllowClick = true;
        /// <summary>
        /// ÊÇ·ñÐèÒªClick cursor ,return false;
        /// </summary>
        public bool IsAllowClick
        {
            get
            {
                return _IsAllowClick;
            }
            set {
                _IsAllowClick = value;
            }

        }

        protected string _childPageName = "";
        public string ChildPageName
        {
            get { return _childPageName; }
            set { _childPageName = value; }
        }
        // Staff 's Full Name -> Funtion Name 2007-03-19
        protected string _CurrectFullName = "";
        public string CurrectFullName
        {
            get { return _CurrectFullName; }
            set { _CurrectFullName = value; }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            RegisterClientScripts();
        }

        protected override void Render(HtmlTextWriter writer)
        {
            BasePage bp = this.Page as BasePage;
            ScInfo scInfo = bp.SecurityInfo;
            string title = null;


            string strLink = "Admin_Default.aspx";
            //switch (scInfo.ModuleName.ToUpper())
            switch (SessionInfo.CurrentModule.ToUpper())
            {

                case "PROJECT":
                    strLink = Config.AppBaseUrl + "/Import/ProjectStatusImport.aspx?_slbg=0&_slbi=0";
                    break;

                case "BASIC":
                    strLink = Config.AppBaseUrl + "/Basic/ManuReason.aspx?_slbg=0&amp;_slbi=0";
                    break;
                case "PERSON": strLink = Config.AppBaseUrl + "/Personal/Personal.aspx?menu=1";
                    break;
                case "PAYROLL":
                    if (ChildPageName != "")
                    {
                        strLink = Config.AppBaseUrl + "/Payroll/" + ChildPageName;
                    }
                    else
                        strLink = Config.AppBaseUrl + "/Payroll/PrFixedItem.aspx";
                    //strLink = Config.AppBaseUrl + "/Personal/psEmploymentHistory.aspx?menu=1";
                    break;
                case "ATTENDANCE": strLink = Config.AppBaseUrl + "/Attendance/AtImportData.aspx";
                    break;
                case "STATISTICS": strLink = Config.AppBaseUrl + "/Statistics/StNoOfHeadcounts.aspx";
                    break;
                case "LEAVE": strLink = Config.AppBaseUrl + "/Leave/LvLeaveApplication.aspx";
                    break;
                case "REPORT":
                    if (ChildPageName != "")
                    {
                        strLink = Config.AppBaseUrl + ChildPageName;
                    }
                    else
                        strLink = Config.AppBaseUrl + "/Report/RptStaffProfile.aspx";
                    break;
                case "SECURITY": strLink = Config.AppBaseUrl + "/Security/ScGroup.aspx";
                    break;
                case "LINK": strLink = Config.AppBaseUrl + "/Link/LkLeavePlan.aspx";
                    break;
                case "STAFFINFORMATION": strLink = Config.AppBaseUrl + "/StaffInformation/TodayLeave.aspx";
                    break;
                case "TRAINING": strLink = Config.AppBaseUrl + "/Training/TrTrainingCourse.aspx";
                    break;
                default: break;
            }
            
            PCCore.Common.HRLog.RecordLog("_mName: " + _mName);
            PCCore.Common.HRLog.RecordLog("_fName: " + _fName);

            //title = String.Format("<img src='" + Config.ThemesBaseUrl + "/Default/images/dotP.gif' width='9' height='9' align='absmiddle'>&nbsp;<a href={1} class='{0}' onclick='javaScript:TitleModuOnClientClick();'>{2}</a> > {3}"
            //title = String.Format("<img src='" + Config.ThemesBaseUrl + "/Default/images/dotP.gif' width='9' height='9' align='absmiddle'>&nbsp;<a href=../{1} class='{0}' onclick='javaScript:TitleModuOnClientClick();'>{2}</a> > {3}"

            title = String.Format("<img src='" + Config.ThemesBaseUrl + "/Default/images/dotP.gif' width='9' height='9' align='absmiddle'>&nbsp;{2} > {3}"
                , "navigationlink"
                , strLink
                , HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, (_mName == null ? SessionInfo.CurrentModule : _mName))
                , HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, (_fName == null ? scInfo.FunctionName : _fName)));





            writer.Write(title);
            PCCore.Common.HRLog.RecordLog(title);
            //string  mm =HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Add").ToString();

        }

        protected void RegisterClientScripts()
        {
            StringBuilder sbJaveXcript = new StringBuilder();
            sbJaveXcript.Append(" "
              + "  function CheckStaffName() "
              + " {  "
              + "      if (typeof(ChildCheckStaffName)==\"function\") "
              + "      { "
              + "          return  ChildCheckStaffName(); "
              + "      }"
              + "    return true; "
              + "  } ");

            sbJaveXcript.Append(@"function TitleModuOnClientClick() 
             {  
                if (typeof(SelfModuTitleOnClinetClick)=='function') 
                { 
                   SelfModuTitleOnClinetClick(); 
                }
            } ");

            sbJaveXcript.Append(@"function TitleFuncOnClientClick() 
             {  
                if (typeof(SelfFuncTitleOnClinetClick)=='function') 
                { 
                   SelfFuncTitleOnClinetClick(); 
                }
            } ");

            Page.ClientScript.RegisterStartupScript(_t, this.ClientID,
                    sbJaveXcript.ToString(), true);


        }


        /////////////////////
    }


}
