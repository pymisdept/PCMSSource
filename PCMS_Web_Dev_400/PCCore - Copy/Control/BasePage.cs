using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using System.Threading;
using System.Globalization;
using SimpleControls.Web;
using SimpleControls;

namespace PCCore
{

    /// <summary>
    /// Summary description for BasePage
    /// </summary>
    public class BasePage : System.Web.UI.Page
    {
        public BasePage()
        {
            
        }

        #region SecurityInfo
        private string _funcCode = null;
        private string _fullfuncCode = null;
        private ScInfo _scInfo = null;
        public ScInfo SecurityInfo
        {
            get { return _scInfo; }
        }
        #endregion SecurityInfo

        #region LoginRequired
        private bool _loginRequired = true;
        public bool LoginRequired
        {
            get { return _loginRequired; }
            set { _loginRequired = value; }
        }
        #endregion LoginRequired

        #region CurrentCommand
        string _currentCommand = null;
        public string CurrentCommand
        {
            get { return _currentCommand; }
            set { _currentCommand = value; }
        }
        #endregion CurrentCommand

        #region ShowWebMenu
        private bool _ShowWebMenu = true;
        public bool ShowWebMenu
        {
            get { return _ShowWebMenu; }
            set { _ShowWebMenu = value; }
        }
        #endregion ShowWebMenu

        #region Compress ViewState
        private SimpleCompressedPageStatePersister _compressedPageStatePersister = null;
        private bool _enableCompressedPageState = false; //set to false because we may use Http Compress filter
        public bool EnableCompressedPageState
        {
            get { return _enableCompressedPageState; }
            set { _enableCompressedPageState = value; }
        }
        protected override PageStatePersister PageStatePersister
        {
            get
            {
                if (_enableCompressedPageState)
                {
                    if (_compressedPageStatePersister == null)
                        _compressedPageStatePersister = new SimpleCompressedPageStatePersister(this);
                    return _compressedPageStatePersister;
                }
                else
                {
                    return base.PageStatePersister;
                }
            }
        }
        #endregion Compress ViewState

        protected override void OnInit(EventArgs e)
        {

            base.OnInit(e);
            //if (String.IsNullOrEmpty(SessionInfo.AccessLevel)) SessionInfo.AccessLevel = "999";
            //if (String.IsNullOrEmpty(SessionInfo.CompanyID)) SessionInfo.CompanyID = "-2";

            string appUrl = Config.AppBaseUrl;
            string appImg = Config.GetImageBaseUrl(Page.Theme);


            //if (String.IsNullOrEmpty(Config.DefaultDatabase) || Config.DefaultDatabase == "abcdefg")
            //if (String.IsNullOrEmpty(Config.DefaultConnectionString) && Request.Url.AbsolutePath.ToString() != Config.AppHomeUrl && Request.Url.AbsolutePath.ToString() != Config.AppBaseUrl)
            //{
            //    Response.Redirect(Config.AppHomeUrl);
            //}


            #region Client Registration
            ClientScriptManager cs = this.ClientScript;
            Type t = this.GetType();

            SimpleJS.RegisterClientScripts(this.Page, SimpleJS.JSTypes.Common);

            cs.RegisterClientScriptInclude(t, "common", Config.AppBaseUrl + "/common.js");
            //cs.RegisterClientScriptInclude(t, "stm31", Config.AppBaseUrl + "/stm31.js");
            //cs.RegisterClientScriptInclude(t, "webmenu", Config.AppBaseUrl + "/webmenu.js");
            //switch (SessionInfo.CurrentLanguage)
            //{
            //    case "zh-cn":
            //        cs.RegisterClientScriptInclude(t, "genmenu", Config.AppBaseUrl + "/genwebmenu_zh-cn.js");
            //        break;
            //    case "zh-tw":
            //        cs.RegisterClientScriptInclude(t, "genmenu", Config.AppBaseUrl + "/genwebmenu_zh-tw.js");
            //        break;
            //    case "ja":
            //        cs.RegisterClientScriptInclude(t, "genmenu", Config.AppBaseUrl + "/genwebmenu_ja.js");
            //        break;
            //    default:
            //        cs.RegisterClientScriptInclude(t, "genmenu", Config.AppBaseUrl + "/genwebmenu.js");
            //        break;
            //}                      


            if (Page.IsPostBack)
            {
                _currentCommand = Request.Form[Consts.HiddenInputCommand];
                cs.RegisterHiddenField(Consts.HiddenInputSave, Request.Form[Consts.HiddenInputSave]);
            }
            else
            {
                _currentCommand = String.Empty;
                cs.RegisterHiddenField(Consts.HiddenInputSave, String.Empty);
            }

            if (_currentCommand == "logout")
            {
                //Inert Login Table
                PCCore.PCMS.UserLog.Logout(Convert.ToInt32(SessionInfo.UserId), System.Web.HttpContext.Current.Session.SessionID);

                string lang = SessionInfo.CurrentLanguage;
                ScLog.Insert(ScLog.Actions.Logout);

                SessionInfo.Init();
                SessionInfo.CurrentLanguage = lang;

                Session.Abandon();
                PCCore.Common.HRLog.RecordLog("Logout");
                PCCore.Common.HRLog.RecordLog(_currentCommand); 
                Response.Redirect(Config.AppBaseUrl + "/Default.aspx", true);

            }

            OnInitRegisterHiddenFieldInputCommand();
            if (string.IsNullOrEmpty(HRCommandValue))
            {
                //don't need to keep status for these
                cs.RegisterHiddenField(Consts.HiddenInputCommand, String.Empty);
            }
            else
            {
                cs.RegisterHiddenField(Consts.HiddenInputCommand, HRCommandValue);
            }
            cs.RegisterHiddenField(Consts.HiddenInputChange, String.Empty);
            cs.RegisterHiddenField(Consts.HiddenInputWinRetVal, String.Empty);

            StringBuilder script = new StringBuilder();
            script.AppendFormat("var {0}=document.getElementById('{0}');", Consts.HiddenInputCommand);
            script.AppendFormat("var {0}=document.getElementById('{0}');", Consts.HiddenInputSave);
            script.AppendFormat("var {0}=document.getElementById('{0}');", Consts.HiddenInputChange);
            script.AppendFormat("var {0}=document.getElementById('{0}');", Consts.HiddenInputWinRetVal);

            script.AppendFormat("var MSG_SELECT_EDIT='{0}';", GetGlobalResourceObject(Consts.ResourcesCommon, "SelectEdit"));
            script.AppendFormat("var MSG_SELECT_DELETE='{0}';", GetGlobalResourceObject(Consts.ResourcesCommon, "SelectDelete"));
            script.AppendFormat("var MSG_DATA_CHANGED='{0}';", GetGlobalResourceObject(Consts.ResourcesCommon, "DataChanged"));
            script.AppendFormat("var PROMPT_CONFIRM_DELETE='{0}';", GetGlobalResourceObject(Consts.ResourcesCommon, "ConfirmDelete"));


            //string winRetVal = Request[Consts.HiddenInputWinRetVal];
            //if (!String.IsNullOrEmpty(winRetVal))
            //{
            //    script.AppendFormat("window.returnValue='{0}';", winRetVal);
            //}

            script.AppendFormat("var DISPLAYING_RECORD='{0}';", GetGlobalResourceObject(Consts.ResourcesCommon, "DisplayingRecord"));
            script.AppendFormat("var ADDING_RECORD='{0}';", GetGlobalResourceObject(Consts.ResourcesCommon, "AddingRecord"));
            script.AppendFormat("var EDITING_RECORD='{0}';", GetGlobalResourceObject(Consts.ResourcesCommon, "EditingRecord"));

            script.AppendFormat("var appUrl='{0}';", appUrl);
            script.AppendFormat("var appImg='{0}';", appImg);
            script.Append("initForm();");

            script.Append("window.setTimeout('chekState()', 100);");

            if (SessionInfo.IsLogin && _ShowWebMenu)
            {
                //script.Append("initMenu();");
                //script.Append("genMenu('','');");
            }

#if DEBUG
            StringBuilder scriptAjaxBlock = new StringBuilder();
            scriptAjaxBlock.Append(@" 
                if (typeof(AjaxPro) != 'undefined')
                {
                 AjaxPro.MyOnError = function(e) 
                    { 
                        var sBool = false;
                        if (e.error !=null) 
                        {
                            alert(e.error.Message);
                            sBool = true;
                        }
                        return sBool;
                    } 
                 }   
                 ");
            cs.RegisterStartupScript(t, "MyAjaxPro", scriptAjaxBlock.ToString(), true);
#endif

            cs.RegisterStartupScript(t, "BasePage", script.ToString(), true);
            #endregion Client Registration

            #region Security
            
            //if (!License.IsLicensed)
            //{
            //    if (SessionInfo.CurrentFunction.ToUpper() != "LICENSE")
            //    {
            //        Response.Redirect(Config.AppBaseUrl + "/License_Request.aspx", false);
            //        return;
            //    }
            //}

            //if (SessionInfo.CurrentFunction.ToUpper() != "DEFAULT" && 
            //    SessionInfo.CurrentFunction.ToUpper() != "" && 
            //    _funcCode != "" && 
            //    _funcCode.ToUpper() != "DEFAULT" && 
            //    !_funcCode.ToUpper().StartsWith("LICENSE"))
            //{
            //    try
            //    {
            //        DateTime expiredate = Convert.ToDateTime(License.ExpiryDate);
            //    }
            //    catch
            //    {
            //        throw new ApplicationException("License Expired Date is invalid!");
            //    }

            //    if (Convert.ToDateTime(License.ExpiryDate) < System.DateTime.Today)
            //    {
            //        throw new ApplicationException("License Expired!");
            //    }
            //}
            PCCore.Common.HRLog.RecordLog("Session Timeout:" + HttpContext.Current.Session.Timeout.ToString());
            PCCore.Common.HRLog.RecordLog(_funcCode);
            PCCore.Common.HRLog.RecordLog(Consts.Function_RequestDownload.ToUpper());
            if (_loginRequired && _scInfo.LoginRequired)
            {

                if (!SessionInfo.IsLogin)
                {
                    PCCore.Common.HRLog.RecordLog("Not SessionInfo.isLogin");
                    PCCore.Common.HRLog.RecordLog(SessionInfo.UserId);
                    PCCore.Common.HRLog.RecordLog(SessionInfo.UserName);
                    PCCore.Common.HRLog.RecordLog(HttpContext.Current.ToString());
                    PCCore.Common.HRLog.RecordLog(HttpContext.Current.Session.ToString());
                    PCCore.Common.HRLog.RecordLog(HttpContext.Current.Session.SessionID);
                    
                    if (_funcCode.ToUpper() != Consts.Function_RequestDownload.ToUpper() && _funcCode.ToUpper() != "DOWNLOADFILE")
                    {
                        Response.Redirect(Config.AppBaseUrl + "/Default.aspx?src=" + Server.UrlEncode(Request.RawUrl), true);
                    }
                    else
                    {
                        string _script = "<script language='javascript>\n";
                        _script += "window.close();\n";
                        _script += "</script>";
                        //Page.RegisterStartupScript("", _script);
                        Response.Write(script);
                    }
                    return;
            
                }

                if (_funcCode != "" && 
                    _funcCode.ToUpper() != "DEFAULT" && 
                    _funcCode.ToUpper() != "LICENSE" && 
                    _fullfuncCode.ToUpper() != "DEFAULT" && 
                    _fullfuncCode.ToUpper() != "" && 
                    _fullfuncCode.ToUpper() != "ADMIN_DEFAULT" &&
                    _fullfuncCode.ToUpper() != "DOWNLOADFILE" && 
                    _fullfuncCode.ToUpper() != "CHANGEPASSWORD" &&
                    // Add Check List
                    _fullfuncCode.ToUpper() != "CHECKLIST" &&
                    _fullfuncCode.ToUpper() != "UPLOADMANAGER" &&
                    // Add Request Download
                    _fullfuncCode.ToUpper() != Consts.Function_RequestDownload &&
                    // Add Download Manager
                    _fullfuncCode.ToUpper() != "DOWNLOADMANAGER"
                    )
                {
                    ///
                    /// License Control 
                    ///          
                    //if (!License.ModuleEnable(SessionInfo.CurrentModuleID))
                    //{
                    //    //Response.Redirect(Config.AppBaseUrl + "/Security/NotExistPage.aspx", false);
                    //    throw new ApplicationException("No License for Module " + SessionInfo.CurrentModuleID);
                    //}

                    // Security Checking Move to Each Page For Project base security structure.
                    
                    // Only Supervisor Access Security Module
                    if (SessionInfo.IsSupervisor == false && SessionInfo.CurrentModuleID == Consts.ModuleSecurityID)
                    {
                        Response.Redirect(Config.AppBaseUrl + "/Security/AccessDenied.aspx", false);
                        return;
                    }
                    if (SessionInfo.CurrentModuleID != "9")
                    {
                        
                        //if (!_scInfo.HasAccess && !SessionInfo.IsSupervisor)
                        if (!PCCore.PCMS.Authorization.PageAllowAccess(SessionInfo.CurrentFunctionID) && !SessionInfo.IsSupervisor)
                        {
                            Response.Redirect(Config.AppBaseUrl + "/Security/AccessDenied.aspx", false);
                            return;

                        }
                    }
                }
                //if (SessionInfo.CurrentModuleID == "9")
                //{
                //}
                //else
                //{
                //    if (Config.DataFilterModule(SessionInfo.CurrentModuleID))
                //    {
                //        // for other site, record can't be view
                //        if (SessionInfo.CurrentModule.ToUpper() == Consts.PersonModule)
                //        {
                //            SessionInfo.DataFilter = String.Format(" and accesslevelid>={0} and (companyid in({1}) or (companyid is null and pcreatedby = '{2}'))", SessionInfo.AccessLevel, SessionInfo.CompanyID, SessionInfo.LoginName);
                //        }
                //        else
                //        {
                //            SessionInfo.DataFilter = String.Format(" and accesslevelid>={0} and companyid in({1})", SessionInfo.AccessLevel, SessionInfo.CompanyID);
                //        }
                //    }
                //    else
                //    {
                //        //for hk site, only personal->employment(basic salary,accesslevel) can't be view/update 
                //        SessionInfo.DataFilter = String.Empty;
                //    }
                //}

                if (SessionInfo.CurrentModule.ToUpper() == Consts.PersonModule)
                {
                    //if (SessionInfo.CurrentPersonalID == String.Empty)
                    //{
                    //    string src = this.Request.Path;
                    //    SessionInfo.BackUrl = src;
                    //    Response.Redirect(Config.AppBaseUrl + "/Personal/Personal.aspx?src=" + src, false);
                    //}

                }
                else
                {
                    //To do : 保留SessionInfo 直到被改变 10-27-2006 
                    if (SessionInfo.CurrentFunction != "atrosterhistory")
                    {
                        //SessionInfo.CurrentPersonalID = String.Empty;
                        //SessionInfo.CurrentStaffID = String.Empty;
                        //SessionInfo.CurrentStaffName = String.Empty;
                    }
                }

                //if (SessionInfo.CurrentPersonalID == String.Empty)
                //{
                //    //if (_funcCode != Consts.PsQueryFunc && SessionInfo.CurrentModule == Consts.PersonModule && SessionInfo.CurrentFunction != Consts.PsPersonalFunc)
                //    if (SessionInfo.CurrentModule == Consts.PersonModule)
                //    {
                //        //Response.Redirect(Config.AppBaseUrl + "/PersonalQuery.aspx?go="+SessionInfo.CurrentFunction, true);

                //        string src = this.Request.Path;
                //        SessionInfo.BackUrl = src;
                //        Response.Redirect(Config.AppBaseUrl + "/Personal/Personal.aspx?src=" + src, false);
                //    }
                //}

                //cs.RegisterStartupScript(t, "ScInfo", _scInfo.BuildScript(), true);
            }
            #endregion Security

        }

        protected override void InitializeCulture()
        {
            base.InitializeCulture();

            if (String.IsNullOrEmpty(Config.DefaultConnectionString) &&
                Request.Url.AbsolutePath.ToUpper() != Config.AppHomeUrl.ToUpper() &&
                Request.Url.AbsolutePath.ToUpper() != Config.AppBaseUrl.ToUpper())
            {
                //Log Logout
                //PCCore.PCMS.UserLog.Logout(System.Web.HttpContext.Current.Session.SessionID);
                PCCore.Common.HRLog.RecordLog("InitializeCuture");
                PCCore.Common.HRLog.RecordLog("Userid: " + SessionInfo.UserId);
                PCCore.Common.HRLog.RecordLog("Database: " + SessionInfo.Database);
                PCCore.Common.HRLog.RecordLog("Sessionid: " + System.Web.HttpContext.Current.Session.SessionID);
                PCCore.Common.HRLog.RecordLog(Request.Url.AbsolutePath.ToUpper());
                PCCore.Common.HRLog.RecordLog(Config.AppHomeUrl.ToUpper());
                PCCore.Common.HRLog.RecordLog(Config.AppBaseUrl.ToUpper());
                PCCore.Common.HRLog.RecordLog(String.IsNullOrEmpty(Config.DefaultConnectionString));
                
                _funcCode = System.IO.Path.GetFileName(System.Web.HttpContext.Current.Request.Url.AbsolutePath);
                _funcCode = _funcCode.Substring(0, _funcCode.LastIndexOf('.'));
                PCCore.Common.HRLog.RecordLog("FuncCode:" + _funcCode);
                PCCore.Common.HRLog.RecordLog("Session Timeout:" + HttpContext.Current.Session.Timeout.ToString());
                
                PCCore.Common.HRLog.RecordLog(Consts.Function_RequestDownload.ToUpper());
                //if (_funcCode.ToUpper() != Consts.Function_RequestDownload.ToUpper() && _funcCode.ToUpper() != "DOWNLOADFILE")
                //{
                    Response.Redirect(Config.AppBaseUrl + "/Default.aspx?src=" + Server.UrlEncode(Request.RawUrl), true);
                //}
                //else
                //{
                //    PCCore.Common.HRLog.RecordLog("Return");

                //    //string _script = "<script language='javascript>\n";
                //    //_script += "window.close();\n";
                //    //_script += "</script>";
                //    ////Page.RegisterStartupScript("", _script);
                //    //Response.Write(_script);
                //}
                
                
                //throw new ApplicationException(
                //    HttpContext.GetGlobalResourceObject(Consts.ResourcesMessages, "TimeOut").ToString()
                //    );
            }

            #region Init Language
            string language = Request.QueryString["lang"];
            string cookiename = "PClang";
            HttpCookie mycookie;
            HttpCookie mydbcookie;
            DateTime now;
            if (language == "" || language == null)
            {
                if (Request.Cookies[cookiename] != null && Request.Cookies[cookiename].Value != String.Empty)
                {
                    language = Request.Cookies[cookiename].Value;
                    SessionInfo.CurrentLanguage = language;
                }
                else
                {
                    language = "en-us";

                    Response.Cookies.Remove(cookiename);
                    mycookie = new HttpCookie(cookiename);
                    mycookie.Value = language;
                    now = DateTime.Now;
                    mycookie.Secure = false;
                    mycookie.Expires = now.AddYears(1);
                    Response.Cookies.Add(mycookie);

                    SessionInfo.CurrentLanguage = language;

                }

            }
            else
            {

                Response.Cookies.Remove(cookiename);
                mycookie = new HttpCookie(cookiename);
                mycookie.Value = language;
                now = DateTime.Now;
                mycookie.Secure = false;
                mycookie.Expires = now.AddYears(1);
                Response.Cookies.Add(mycookie);

                SessionInfo.CurrentLanguage = language;
            }
            #endregion Init Language

            #region Database Cookie
            if (!String.IsNullOrEmpty(SessionInfo.Database))
            {
                string databasecookie = "PCDatabase";
                Response.Cookies.Remove(databasecookie);
                mydbcookie = new HttpCookie(databasecookie);
                mydbcookie.Value = SessionInfo.Database;
                now = DateTime.Now;
                mydbcookie.Secure = false;
                mydbcookie.Expires = now.AddYears(1);
                Response.Cookies.Add(mydbcookie);
            }
            #endregion Database Cookie

            #region Database Cookie
            if (!String.IsNullOrEmpty(SessionInfo.LoginName))
            {
                string loginnamecookie = "LoginName";
                Response.Cookies.Remove(loginnamecookie);
                mydbcookie = new HttpCookie(loginnamecookie);
                mydbcookie.Value = SessionInfo.LoginName;
                now = DateTime.Now;
                mydbcookie.Secure = false;
                mydbcookie.Expires = now.AddYears(1);
                Response.Cookies.Add(mydbcookie);
            }
            #endregion Database Cookie


            #region Init CurrentCulture and CurrentUICulture

            ComFunction2.InitCulture(Thread.CurrentThread);

            #endregion Init CurrentCulture and CurrentUICulture


            #region Security
            _fullfuncCode = Sc.GetFullFunctionCode(Request.Path);
            _funcCode = Sc.GetFunctionCode(Request.Path);



            if (_funcCode.ToLower() == "default") return;
            if (_funcCode.ToLower() == "newdefault") return;

            
            
            OnInitFunctionCode();
            //SessionInfo.CurrentPersonalID = _pID;
            _scInfo = new ScInfo(_funcCode);
            SessionInfo.CurrentModule = _scInfo.ModuleName;
            SessionInfo.ShowStar = _scInfo.ShowStar;
            

            SessionInfo.CurrentModuleID = _scInfo.ModuleId;
            SessionInfo.CurrentFunctionID = _scInfo.FunctionId;
            SessionInfo.CurrentFunction = _scInfo.FunctionCode;
            #endregion Security

        }

        private string GetSubModule(string _submoduleID)
        {
            string m="";
            //switch (_submoduleID)
            //{
            //    case "1":
            //        m = Consts.BsSubModulePs;
            //        break;
            //    case "2":
            //        m = Consts.BsSubModuleEmp;
            //        break;
            //    case "3":
            //        m = Consts.BsSubModulePr;
            //        break;
            //    case "4":
            //        m = Consts.BsSubModuleAt;
            //        break;
            //    case "5":
            //        m = Consts.BsSubModuleLv;
            //        break;
            //    default:
            //        m = String.Empty;
            //        break;
            //}
            return m;
        }
              

        #region For Dynamic Web Page
        public virtual void OnInitFunctionCode() { }

        public virtual void OnInitRegisterHiddenFieldInputCommand() { }

        /// <summary>
        /// 定义当前页面对应的FunctinCode ,Init ScInformation
        /// </summary>
        public string FunctionCode
        {
            get { return _funcCode; }
            set { _funcCode = value; }
        }
        private string _HRCommandValue = string.Empty;
        public string HRCommandValue
        {
            get { return _HRCommandValue; }
            set { _HRCommandValue = value; }
        }

        #endregion

    }// end of class
}