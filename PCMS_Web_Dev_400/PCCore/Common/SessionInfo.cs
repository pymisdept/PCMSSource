using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.SessionState;
using System.Data;
using System.Collections;
using SimpleControls.SimpleDatabase;

namespace PCCore
{
    [Serializable]
    public class SessionInfo
    {
        #region session
        // Upload Wait Control
        protected const string SE_UPLOADWAIT = "UploadWait";
        // Download Request Wait Control
        protected const string SE_DLNWAIT = "DlnWait";

        public const string SE_TEMPDIR = "TempDir";
        protected const string SE_TEMPDIRURL = "TempDirUrl";

        protected const string SE_ISLOGIN = "IsLogin";
        public const string SE_LOGINNAME = "LoginName";
        protected const string SE_USERID = "UserId";
        protected const string SE_USERNAME = "UserName";
        protected const string SE_PROJECTID = "ProjectID";
        protected const string SE_USERTITLE = "UserTitle";
        protected const string SE_ISSUPERVISOR = "IsSupervisor";
        //add by Michael, begin
        protected const string SE_ISREVERSE = "IsReverse";
        //add by Michael, end
        //add by Ken, begin
        protected const string SE_ISBATCHUPLOAD = "IsBatchUpload";
        //add by Ken, end
        protected const string SE_CURRENTPROJECT = "CurProject";
        protected const string SE_CURRENTPROJECTNAME = "CurProjName";
        public const string SE_REMOTEADDR = "RemoteAddr";
        public const string SE_REMOTEHOST = "RemoteHost";
        public const string SE_REMOTEUSER = "RemoteUser";
        public const string SE_CURRENTMODULE = "CurrentModule";
        public const string SE_CURRENTSUBMODULE = "CurrentSubModule";
        public const string SE_CURRENTMODULEID = "CurrentModuleId";
        public const string SE_CURRENTFUNCTION = "CurrentFunction";
        public const string SE_CURRENTFUNCTIONID = "CurrentFunctionId";
        public const string SE_CURRENTLANGUAGE = "CurrentLanguage";
        public const string SE_RedirectURL = "RedirectURL";
        public const string SE_BACKURL = "BackUrl";
        public const string SE_SHOWSTAR = "ShowStar";
        public const string SE_DATABASE = "Database";     
        public const string SE_DATAFILTER = "DataFilter";
        public const string SE_SESSIONDB = "SessionDB";
        public const string SE_SESSIONID = "SessionID";
        public const string SE_USERTYPE = "UserType";
        public const string SE_SUPPORTEMAIL = "SupportEmail";
        public const string SE_SYSTEMREGION = "SystemRegion";//HK,Macau,China....        
        

        public const string SE_FOREXPORTSQL = "ForExportSql";
        public const string SE_FOREXPORTGRIDHEAD = "ForExportGridHead";
        public const string SE_PERSONALVIEWINDEX = "PersonalViewIndex";

        // PM's Report PeriodCode for Session
        public const string SE_PMRptPeriod = "PMRptPeriod";
        public const string SE_PMRptProject = "PMRptProject";
        public const string SE_CurrentTheme = "CurrTheme";
        public const string SE_FORRUNNINGSQL = "ForRunningSql";
        public const string SE_REPORT = "Report";
        public const string SE_VIEWTYPE = "ViewType";
        public const string SE_FORRUNNINGSQLCONDITION = "ForRunningSqlCondition";
        public const string SE_CONDITION = "Condition";
        public const string SE_USERCONDITION = "UserCondition";
        public const string SE_SSORTBY = "Sortby";
        public const string SE_SGROUPBY = "Groupby";
        public const string SE_GROUPBYNOUSE = "SGroupby";
        public const string SE_STAFFSTATUS = "StaffStatus";
        public const string SE_PRINTPROGRESS = "PrintProgress";
        public const string SE_PRINTEND = "PrintEnd";
        public const string SE_PRINTFONT_Label = "PrintFontLabel";
        public const string SE_PRINTFONT_TextBox = "PrintFontTextBox";
        public const string SE_PRINTFONT = "PrintFont";
        public const string SE_PMREPORT = "PMREPORTTABLE";

        public const string SE_UserPassword = "USERPW";
        

        #endregion

        public static void Init()
        {
            //Session.SessionID stored database 
            SimpleControls.SimpleCache.Remove(HttpContext.Current.Session.SessionID);

            HttpContext.Current.Session.Clear();
            IsLogin = false;
            IsSupervisor = false;
            //add by Michael
            IsReverse = false;
            //add by Michael, end
            //add by Ken, begin
            IsBatchUpload = false;
            //add by Ken, end 
            LoginName = String.Empty;
            UserName = String.Empty;
            UserId = String.Empty;
            ProjectID = "-1";
            RemoteAddr = String.Empty;
            RemoteHost = String.Empty;
            RemoteUser = String.Empty;
            
            
            CurrentModule = String.Empty;
            CurrentModuleID = String.Empty;
            CurrentFunctionID = String.Empty;
            CurrentFunction = String.Empty;
            BackUrl = String.Empty;
            ShowStar = String.Empty;
            Database = String.Empty;

            DataFilter = String.Empty;
            UserType = String.Empty;
            SupportEmail = String.Empty;

            TempDirUrl = String.Format("{0}/{1}{2}", Config.TempBaseUrl, "t_", HttpContext.Current.Session.SessionID);
            
            //report 
            IsPrintEnd = false;

            // Set Session Timeout from web.conig
            HttpContext.Current.Session.Timeout = Config.SessionTimeout;
        }

        protected static object GetValue(string name)
        {
            //modi by jawance 2006-09-21 for check is null happen, 
            object SessionObj = null;

            if (HttpContext.Current == null || HttpContext.Current.Session == null || HttpContext.Current.Session[name] == null)
            {
                SessionObj = string.Empty;
            }
            else
            {
                SessionObj = HttpContext.Current.Session[name];
            }
            return SessionObj;

        }

        protected static void SetValue(string name, object val)
        {
            HttpContext.Current.Session[name] = val;
        }

        public static string UserPassword
        {
            get { return GetValue(SE_UserPassword).ToString(); }
            set { SetValue(SE_UserPassword,value); }
        }
        public static string TempDirUrl
        {
            get { return GetValue(SE_TEMPDIRURL).ToString(); }
            set
            {
                SetValue(SE_TEMPDIRURL, value);
                TempDir = HttpContext.Current.Server.MapPath(TempDirUrl);
            }
        }

        public static string TempDir
        {
            get { return GetValue(SE_TEMPDIR).ToString(); }
            protected set { SetValue(SE_TEMPDIR, value); }
        }

        public static string UploadWait
        {
            get
            {
                return Convert.ToString(GetValue(SE_UPLOADWAIT));
            }
            set
            {
                SetValue(SE_UPLOADWAIT, value);
            }
        }
        public static string DlnWait
        {
            get
            {
                return Convert.ToString(GetValue(SE_DLNWAIT));
            }
            set
            {
                SetValue(SE_DLNWAIT, value);
            }
        }

        public static bool IsLogin
        {
            get
            {
                object o = GetValue(SE_ISLOGIN);
                if (o == null || String.IsNullOrEmpty(o.ToString())) return false;
                return Convert.ToBoolean(o);
            }
            set { SetValue(SE_ISLOGIN, value); }
        }

        public static string UserId
        {
            get { return GetValue(SE_USERID).ToString(); }
            set { SetValue(SE_USERID, value); }
        }
        public static string PMRptPeriod
        {
            get { return GetValue(SE_PMRptPeriod).ToString(); }
            set { SetValue(SE_PMRptPeriod, value); }
        }

        public static string PMRptProject
        {
            get { return GetValue(SE_PMRptProject).ToString(); }
            set { SetValue(SE_PMRptProject, value); }
        }
        public static string CurrentTheme
        {
            get { return GetValue(SE_CurrentTheme).ToString(); }
            set { SetValue(SE_CurrentTheme, value); }
        }
        public static string LoginName
        {
            get { return GetValue(SE_LOGINNAME).ToString(); }
            set { SetValue(SE_LOGINNAME, value); }
        }
        public static string UserName
        {
            get { return GetValue(SE_USERNAME).ToString(); }
            set { SetValue(SE_USERNAME, value); }
        }

        public static string UserTitle
        {
            get { return GetValue(SE_USERTITLE).ToString(); }
            set { SetValue(SE_USERTITLE, value); }
        }

        public static string CurrentProject
        {
            get { return GetValue(SE_CURRENTPROJECT).ToString(); }
            set { SetValue(SE_CURRENTPROJECT, value); }
        }
        public static string CurrentProjectName
        {
            get { return GetValue(SE_CURRENTPROJECTNAME).ToString(); }
            set { SetValue(SE_CURRENTPROJECTNAME, value); }
        }
        public static string ProjectID
        {
            get { return GetValue(SE_PROJECTID).ToString(); }
            set { SetValue(SE_PROJECTID, value); }
        }

        public static bool IsSupervisor
        {
            get { return Convert.ToBoolean(GetValue(SE_ISSUPERVISOR)); }
            set { SetValue(SE_ISSUPERVISOR, value); }
        }

        //add by Michael, begin
        public static bool IsReverse
        {
            get { return Convert.ToBoolean(GetValue(SE_ISREVERSE)); }
            set { SetValue(SE_ISREVERSE, value); }
        }
        //add by Michael, end

        //add by Ken, begin
        public static bool IsBatchUpload
        {
            get { return Convert.ToBoolean(GetValue(SE_ISBATCHUPLOAD)); }
            set { SetValue(SE_ISBATCHUPLOAD, value); }
        }
        //add by ,, end

        public static string RemoteAddr
        {
            get { return GetValue(SE_REMOTEADDR).ToString(); }
            set { SetValue(SE_REMOTEADDR, value); }
        }
        public static string RemoteHost
        {
            get { return GetValue(SE_REMOTEHOST).ToString(); }
            set { SetValue(SE_REMOTEHOST, value); }
        }
        public static string RemoteUser
        {
            get { return GetValue(SE_REMOTEUSER).ToString(); }
            set { SetValue(SE_REMOTEUSER, value); }
        }

        public static string CurrentModule
        {
            get { return GetValue(SE_CURRENTMODULE).ToString(); }
            set { SetValue(SE_CURRENTMODULE, value); }
        }
        public static string CurrentModuleID
        {
            get { return GetValue(SE_CURRENTMODULEID).ToString(); }
            set { SetValue(SE_CURRENTMODULEID, value); }
        }
        public static string CurrentSubModule
        {
            get { return GetValue(SE_CURRENTSUBMODULE).ToString(); }
            set { SetValue(SE_CURRENTSUBMODULE, value); }
        }
        public static string CurrentFunctionID
        {
            get { return GetValue(SE_CURRENTFUNCTIONID).ToString(); }
            set { SetValue(SE_CURRENTFUNCTIONID, value); }
        }
        public static string CurrentFunction
        {
            get { return GetValue(SE_CURRENTFUNCTION).ToString(); }
            set { SetValue(SE_CURRENTFUNCTION, value); }
        }
        public static string CurrentLanguage
        {
            get { return GetValue(SE_CURRENTLANGUAGE).ToString(); }
            set { SetValue(SE_CURRENTLANGUAGE, value); }
        }
        public static string BackUrl
        {
            get { return GetValue(SE_BACKURL).ToString(); }
            set { SetValue(SE_BACKURL, value); }
        }

        public static string ShowStar
        {
            get { return GetValue(SE_SHOWSTAR).ToString(); }
            set { SetValue(SE_SHOWSTAR, value); }
        }
        public static string Database
        {
            get { return GetValue(SE_DATABASE).ToString(); }
            set { SetValue(SE_DATABASE, value); }
        }

        public static string DataFilter
        {
            get { return GetValue(SE_DATAFILTER).ToString(); }
            set { SetValue(SE_DATAFILTER, value); }
        }

        public static string UserType
        {
            get { return GetValue(SE_USERTYPE).ToString(); }
            set { SetValue(SE_USERTYPE, value); }
        }

        public static string SupportEmail
        {
            get { return GetValue(SE_SUPPORTEMAIL).ToString(); }
            set { SetValue(SE_SUPPORTEMAIL, value); }
        }

        
        #region Report And Chart
        /// <summary>
        /// 共用的SQL条件
        /// </summary>
        public static string ForRunningSqlCondition
        {
            get { return GetValue(SE_FORRUNNINGSQLCONDITION).ToString(); }
            set { SetValue(SE_FORRUNNINGSQLCONDITION, value); }
        }

        /// <summary>
        /// 共运行的SQL 语句 
        /// </summary>
        /// <returns></returns>
        public static string ForRunningSql
        {
            get { return GetValue(SE_FORRUNNINGSQL).ToString(); }
            set { SetValue(SE_FORRUNNINGSQL, value); }
        }

        /// <summary>
        /// 共用的条件
        /// </summary>
        /// <returns></returns>
        public static string Condition
        {
            get { return GetValue(SE_CONDITION).ToString(); }
            set { SetValue(SE_CONDITION, value); }
        }

        /// <summary>
        /// 用户自定义的条件
        /// </summary>
        /// <returns></returns>
        public static string UserCondition
        {
            get { return GetValue(SE_USERCONDITION).ToString(); }
            set { SetValue(SE_USERCONDITION, value); }
        }

        /// <summary>
        /// Sort By
        /// </summary>
        public static string sSortBy
        {
            get { return GetValue(SE_SSORTBY).ToString(); }
            set { SetValue(SE_SSORTBY, value); }
        }

        /// <summary>
        /// Group By
        /// </summary>
        public static string sGroupBy
        {
            get { return GetValue(SE_GROUPBYNOUSE).ToString(); }
            set { SetValue(SE_GROUPBYNOUSE, value); }
        }
        /// <summary>
        /// Printprogress
        /// </summary>
        public static string Printprogress
        {
            get { return GetValue(SE_PRINTPROGRESS).ToString(); }
            set { SetValue(SE_PRINTPROGRESS, value); }
        }

        /// <summary>
        /// IsPrintEnd
        /// </summary>
        public static bool IsPrintEnd
        {
            get { return Convert.ToBoolean(GetValue(SE_PRINTEND)); }
            set { SetValue(SE_PRINTEND, value); }
        }
        /// <summary>
        /// PrintFont  Label
        /// </summary>
        public static string PrintFontLabel
        {
            get { return GetValue(SE_PRINTFONT_Label).ToString(); }
            set { SetValue(SE_PRINTFONT_Label, value); }
        }

        /// <summary>
        /// PrintFont  TextBox
        /// </summary>
        public static string PrintFontTextBox
        {
            get { return GetValue(SE_PRINTFONT_TextBox).ToString(); }
            set { SetValue(SE_PRINTFONT_TextBox, value); }
        }

        /// <summary>
        /// PrintFont 
        /// </summary>
        public static string PrintFont
        {
            get { return GetValue(SE_PRINTFONT).ToString(); }
            set { SetValue(SE_PRINTFONT, value); }
        }

        /// <summary>
        /// report
        /// </summary>
        public static string Report
        {
            get { return GetValue(SE_REPORT).ToString(); }
            set { SetValue(SE_REPORT, value); }
        }

        /// <summary>
        /// ViewType
        /// </summary>
        public static string ViewType
        {
            get { return GetValue(SE_VIEWTYPE).ToString(); }
            set { SetValue(SE_VIEWTYPE, value); }
        }

        /// <summary>
        /// Staff Status
        /// </summary>
        public static string StaffStatus
        {
            get { return GetValue(SE_STAFFSTATUS).ToString(); }
            set { SetValue(SE_STAFFSTATUS, value); }
        }
        public static string SessionID
        {
            get { return GetValue(SE_SESSIONID).ToString(); }
            set { SetValue(SE_SESSIONID, value); }
        }

        public static string RedirectUrl
        {
            get {  return GetValue(SE_RedirectURL).ToString();}
            set { SetValue(SE_RedirectURL, value); }
        }
        #endregion End Report

       

        #region 2007-1-12
        public static string ForExportSQL
        {
            get { return GetValue(SE_FOREXPORTSQL).ToString(); }
            set { SetValue(SE_FOREXPORTSQL, value); }
        }

        public static string ForExportGridHead
        {
            get { return GetValue(SE_FOREXPORTGRIDHEAD).ToString(); }
            set { SetValue(SE_FOREXPORTGRIDHEAD, value); }
        }

        #endregion

    }
}
