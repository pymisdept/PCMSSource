using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using PCCore;
using System.Collections.Specialized;
using System.Management;

public partial class _Default : BasePage
{
    string _src;
    string cookiename = "PCDatabase";
    string cookieloginname = "LoginName";
    string database;

    protected override void OnInit(EventArgs e)
    {
        
        base.LoginRequired = false;
        base.ShowWebMenu = false;
        //Config.DefaultDatabase = "abcdefg";
        SessionInfo.CurrentFunction = "Default";
        base.OnInit(e);

        //_src = Request.QueryString["src"];
        _src = string.Format("{0}/Admin_Default.aspx", Config.AppBaseUrl);

        if (String.IsNullOrEmpty(_src))
            _src = Config.AppHomeUrl;
        string cssLink = Config.ThemesBaseUrl + "calendar.css";
        LiteralControl css = new LiteralControl(cssLink);
        css.Visible = false;
        css.ID = "calendarcss";
        Page.Header.Controls.Add(css);

        #region Customer Logo
        if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(Config.ThemesBaseUrl + "/Source_Customerlogo.gif")))
        {
            //CustomerLogo.Visible = false;
            //tdCustomerLogo.Style["display"] = "none";
            //tdHoliday.Style["height"] = "186px";
        }

        if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(Config.ThemesBaseUrl + "/Source_Customerlogo_left.gif")))
        {
            CustomerLogoLeft.Visible = false;
            tdCustomerLogoLeft.Style["display"] = "none";
        }

        if (!System.IO.File.Exists(HttpContext.Current.Server.MapPath(Config.ThemesBaseUrl + "/Source_Customerlogo_left2.gif")))
        {
            CustomerLogoLeft2.Visible = false;
            tdCustomerLogoLeft2.Style["display"] = "none";
        }
        #endregion

    }

    protected void Page_Load(object sender, EventArgs e)
    {

        // Query for all the enabled network adapters

        ManagementObjectSearcher objSearcher = new ManagementObjectSearcher("SELECT * FROM Win32_NetworkAdapterConfiguration WHERE IPEnabled = 'TRUE'");

        ManagementObjectCollection objCollection = objSearcher.Get();

        // Loop through all available network interfaces

        foreach (ManagementObject obj in objCollection)
        {

            // List all IP addresses of the current network interface

            string[] AddressList = (string[])obj["IPAddress"];

            foreach (string Address in AddressList)
            {

                PCCore.Common.HRLog.RecordLog("IPAddress:" + Address);

            }

        }

        //txtCalendar0.Text = DateTime.Today.ToString(Consts.DateFormat).Substring(0, 7) + "-01".ToString();
        if (PCCore.SessionInfo.CurrentLanguage.ToUpper() != "EN-US" && PCCore.SessionInfo.CurrentLanguage.ToUpper() != "JA-JP")
        {
            //Label1.Font.Size = FontUnit.Point(10);
        }

        RegisterClient();

        //if (!SessionInfo.IsLogin)
        //{
        //    Response.Redirect(Config.AppHomeUrl, false);
        //}
        //get cookie for database

        if (Request.Cookies[cookiename] != null && Request.Cookies[cookiename].Value != String.Empty)
        {
            database = Request.Cookies[cookiename].Value;
            SessionInfo.Database = database;
            //txtDatabase.Text = database;
        }

        string loginname;
        if (Request.Cookies[cookieloginname] != null && Request.Cookies[cookieloginname].Value != String.Empty)
        {
            loginname = Request.Cookies[cookieloginname].Value;

            //txtLoginName.Text = loginname;
            //txtPassword.Focus();
        }
        else
        {
            //txtLoginName.Focus();
        }
        //if (String.IsNullOrEmpty(SessionInfo.Database))
        this.ddlDatabase.Items.Clear();

        string[] dbs = null;
        dbs = Config.Database.Split(new char[] { ',' });
        for (int i = 0; i < dbs.Length; i++)
        {
            this.ddlDatabase.Items.Add(dbs[i].ToString());
        }
        ListItem li = null;

        if (dbs.Length > 0)
        {
            if (String.IsNullOrEmpty(SessionInfo.Database))
            {
                SessionInfo.Database = dbs[0].ToString();
            }

            li = ddlDatabase.Items.FindByText(SessionInfo.Database);
            if (li != null)
            {
                ddlDatabase.SelectedIndex = ddlDatabase.Items.IndexOf(li);
            }
        }

        SessionInfo.Database = ddlDatabase.Text;

        //ShowHoliday();

        if (!Page.IsPostBack)
        {
            InitUsefullLinTable();
        }
        AjaxPro.Utility.RegisterTypeForAjax(typeof(_Default));
    }

    void InitUsefullLinTable()
    {
        #region "DataSource"
        DataTable _dt = null;

        _dt = PCDb.Db.ExecQuery(string.Format("select top {0} id ,Name ,UrlAddress from sc_usefull_link where 1=1 order by priorityid,name ", "4"));
        #endregion

        HtmlTable UsefullLinTable = new HtmlTable();

        // width="100%" border="0" cellspacing="0" cellpadding="3"
        UsefullLinTable.Width = "100%";
        UsefullLinTable.Border = 0;
        UsefullLinTable.CellSpacing = 0;
        UsefullLinTable.CellPadding = 3;


        HtmlTableRow _trow = null;
        HtmlTableCell _tcellA = null;
        HtmlTableCell _tcellB = null;
        HtmlTableCell _tcellC = null;

        HtmlAnchor _a = null;

        string _strID = "";
        string _strName = "";
        string _strUrlAddress = "";

        if (_dt != null && _dt.Rows.Count > 0)
        {
            for (int i = 0; i < _dt.Rows.Count; i++)
            {
                _trow = new HtmlTableRow();
                _trow.ID = Convert.ToString(i + 1);
                _tcellA = new HtmlTableCell();
                _tcellB = new HtmlTableCell();
                _tcellC = new HtmlTableCell();
                _a = new HtmlAnchor();

                _strID = _dt.Rows[i]["id"].ToString();
                _strName = _dt.Rows[i]["Name"].ToString();
                _strUrlAddress = _dt.Rows[i]["UrlAddress"].ToString();

                _tcellA.InnerHtml = _dt.Rows[i]["id"].ToString();
                _tcellA.Attributes.Add("style", "display:none");

                _tcellB.InnerHtml = "<IMG SRC='" + Config.ThemesBaseUrl + "/Default/Images/dotarrow11.jpg'  width=\"7\" height=\"11\" /> ";
                _tcellB.Attributes.Add("width", "10px");
                _tcellB.Attributes.Add("valign", "Top");

                _tcellC.InnerHtml = "<a href=\"" + System.Web.HttpUtility.HtmlEncode(_dt.Rows[i]["UrlAddress"].ToString()) + "\" class=\"smalltxt\"  target=\"_blank\" >"
                           + System.Web.HttpUtility.HtmlEncode(_strName) + "</a>";

                _tcellC.Attributes.CssStyle.Add(HtmlTextWriterStyle.BackgroundImage, Config.ThemesBaseUrl + "/Default/Images/linedot_18X19.gif");

                _tcellC.Attributes.CssStyle.Add("background-repeat", "repeat-x");
                _tcellC.Attributes.CssStyle.Add("background-position", "bottom");

                _trow.Cells.Add(_tcellA);
                _trow.Cells.Add(_tcellB);
                _trow.Cells.Add(_tcellC);


                UsefullLinTable.Controls.Add(_trow);
            }
            UsefullLinkTd.Controls.Add(UsefullLinTable);
        }

    }

    protected void RegisterClient()
    {
        //string regID = " tdHoliday= '" + tdHoliday.ClientID.ToString() + "';";
        //regID += " txtCalendar0='" + txtCalendar0.ClientID.ToString() + "';";
        //regID += " jsCalendar='" + jsCalendar.ClientID.ToString() + "';";

        //Page.ClientScript.RegisterStartupScript(Page.GetType(), "register", "<script language='javascript'>" + regID + "</script>");
    }


    /// <summary>
    /// 获取当前webconfig 设置的win Height / Width
    /// </summary>
    /// <param name="pType"></param>
    /// <returns></returns>
    [AjaxPro.AjaxMethod]
    public Int32 WinScrollHeightOrWidth(string pType)
    {
        Int32 sReturn = 0;
        switch (pType)
        {
            case "H":
                if (Config.winScrollHeight > 0)
                {
                    sReturn = Config.winScrollHeight;
                }
                break;
            case "W":
                if (Config.winScrollWidth > 0)
                {
                    sReturn = Config.winScrollWidth;
                }
                break;
        }

        return sReturn;
    }




    //public string Login(string uid, string pwd, string database)
    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public string Login(string uid, string pwd, string database)
    {
        SessionInfo.Database = database;

        //Config.DefaultDatabase = database;

        try
        {
            if (Sc.Login(uid, pwd))
            {
                ClearRecordLoginFailCount(uid);
                
                    return "SystemUser";
                

            }
            else
            {
                // Login Fail Process
                if (ExistUser(uid))
                {
                    RecordLoginFailCount(uid);

                    if (Session[GetLoginFailCountSessionName(uid)] != null)
                    {
                        SystemParameter sp = SystemParameter.Get(PCDb.Db);

                        if (int.Parse(Session[GetLoginFailCountSessionName(uid)].ToString()) >= sp.MaxLoginTimes)
                        {
                            LockUser(uid);
                        }
                    }

                    LogRecord(uid);
                }

                if (IsLocked(uid))
                {
                    throw new Exception(Resources.Messages.UserLocked);
                }
                else
                {
                    return Resources.Messages.LoginFailed;
                }
            }
        }
        catch (Exception ex)
        {
            throw ex;
        }
    }


    #region Login Fail Process
    /// <summary>
    /// 指示当前用户是否存在
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    private bool ExistUser(string uid)
    {
        string sql = string.Format("select * from v_sc_users where loginname = '{0}' ", uid);
        DataTable dt = PCDb.Db.ExecQuery(sql);

        if (dt.Rows.Count > 0)
        {
            return true;
        }

        return false;
    }

    /// <summary>
    /// 记录当前用户的登陆失败次数
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    private void RecordLoginFailCount(string uid)
    {
        if (Session[GetLoginFailCountSessionName(uid)] == null)
        {
            Session[GetLoginFailCountSessionName(uid)] = 1;
        }
        else
        {
            int n = int.Parse(Session[GetLoginFailCountSessionName(uid)].ToString());
            n++;
            Session[GetLoginFailCountSessionName(uid)] = n;
        }
    }

    private void ClearRecordLoginFailCount(string uid)
    {
        Session[GetLoginFailCountSessionName(uid)] = 0;
    }

    /// <summary>
    /// 锁定当前用户
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    private void LockUser(string uid)
    {
        string sql = string.Format("select id from v_sc_users where loginname = '{0}'", uid);

        DataTable dt = PCDb.Db.ExecQuery(sql);

        if (dt.Rows.Count > 0)
        {
            int loginID = int.Parse(dt.Rows[0][0].ToString());

            string sqlLock = string.Format("update sc_user set locked = 1 where id = {0}", loginID);

            PCDb.Db.ExecQuery(sqlLock);
        }
    }

    /// <summary>
    /// 记录登陆失败次数
    /// </summary>
    /// <param name="uid"></param>
    private void LogRecord(string uid)
    {
        Hashtable hash = new Hashtable();
        NameValueCollection nvc = HttpContext.Current.Request.ServerVariables;

        //hash["ID"] = PCDb.GetNextID();
        hash["ID"] = PCDb.GetNextLogID();
        hash["LOGTIME"] = DateTime.Now.ToString(Consts.DefaultDateTimeFormat);
        hash["LOGINNAME"] = uid;
        hash["ACTION"] = "Login Fail";
        hash["FUNCNAME"] = "Login";
        hash["IPADDRESS"] = nvc["LOCAL_ADDR"];
        hash["HOSTNAME"] = Config.ServerIPAddress;

        PCTable hrtb = new PCTable("sc_log");

        hrtb.EnableLog = false;

        hrtb.BeginTransaction();
        hrtb.Insert(hash);
        hrtb.CommitTransaction();
    }

    /// <summary>
    /// 获取登陆失败的次数信息
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    private string GetLoginFailCountSessionName(string uid)
    {
        return string.Format("{0}LoginFailCount", uid);
    }

    /// <summary>
    /// 判断某个用户是否己经被锁定
    /// </summary>
    /// <param name="uid"></param>
    /// <returns></returns>
    private bool IsLocked(string uid)
    {
        string sql = string.Format("select id from v_sc_users where loginname = '{0}'", uid);

        DataTable dt = PCDb.Db.ExecQuery(sql);

        if (dt.Rows.Count > 0)
        {
            int loginID = int.Parse(dt.Rows[0][0].ToString());
            string sqlLock = string.Format("select locked from sc_user where id = {0}", loginID);
            DataTable dtLock = PCDb.Db.ExecQuery(sqlLock);

            if (dtLock.Rows.Count > 0)
            {
                if (dtLock.Rows[0][0].ToString() == "1") return true;
            }
        }

        return false;
    }
    #endregion


    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public DataTable GetHoliday(string db, string month)
    {
        if (String.IsNullOrEmpty(month))
            month = DateTime.Today.ToString(Consts.DateFormat).Substring(0, 7);
        string lang = SessionInfo.CurrentLanguage;
        SessionInfo.Init();
        SessionInfo.Database = db;
        SessionInfo.CurrentLanguage = lang;
        string sql;
        string DistinctFiled;
        DataTable dt;
        DateTime startdate = Convert.ToDateTime(month + "-01");
        DateTime enddate = startdate.AddMonths(1).AddDays(-1);
        try
        {
            switch (SessionInfo.CurrentLanguage.ToUpper())
            {
                case "EN-US":
                case "JA-JP":
                    DistinctFiled = "substring(cast(date as varchar(100)),1,6)";
                    sql = String.Format("select distinct {1} as clddate ,remark as name from {0} where 1=1", "BS_CALENDARDETAIL", DistinctFiled);
                    break;
                default:
                    DistinctFiled = string.Format("cast(month(date) as varchar(2))+'{0}'+ (Case When Len(cast(day(date) as varchar(2))) >1 THEN cast(day(date) as varchar(2)) ELSE '0'+ cast(day(date) as varchar(2)) END)+'{1}'"
                        , HttpContext.GetGlobalResourceObject("Labels", "Mth", new System.Globalization.CultureInfo(SessionInfo.CurrentLanguage)).ToString()
                        , HttpContext.GetGlobalResourceObject("Labels", "Day", new System.Globalization.CultureInfo(SessionInfo.CurrentLanguage)).ToString());

                    sql = String.Format("select distinct {1} as clddate ,remark as name from {0} where 1=1", "BS_CALENDAR", DistinctFiled);
                    break;
            }
            sql += String.Format(" and date>='{0}' and date<='{1}' order by {2}", startdate.ToString(Consts.DateFormat), enddate.ToString(Consts.DateFormat), DistinctFiled);

            dt = PCDb.Db.ExecQuery(sql);
            return dt;
        }
        catch (Exception ex)
        {
            throw new ApplicationException(ex.Message);
        }
    }

    string FormatHtmlString(string pString)
    {
        string _string = string.Empty;
        _string = pString;
        Int32 AscLength = 0;
        AscLength = System.Text.Encoding.ASCII.GetBytes(_string).Length;
        for (int i = 0; i < AscLength; i++)
        {
            if (System.Text.Encoding.ASCII.GetBytes(_string).GetValue(i).ToString() == "OD")
            {
                System.Text.Encoding.ASCII.GetBytes(_string).SetValue("\r", i);
            }
        }
        return _string;
    }

   

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.ReadWrite)]
    public HtmlTable GetHtmlTable(string db)
    {
        string lang = SessionInfo.CurrentLanguage;
        SessionInfo.Init();
        SessionInfo.CurrentLanguage = lang;
        SessionInfo.Database = db;
        string _CusMessage = string.Empty;        
        bool isShowAnnouncement = false;

        #region "DataSource"
        DataTable _dtAnnouncement = null;
        HtmlTable _htmlt = new HtmlTable();
        try
        {
            
            //_dtAnnouncement = PCDb.Db.ExecQuery(String.Format("select id,title,body,expirydate from {0} where 1=1 and effectivedate<='{1}' and isNull(isPublic,0) = 1 and  (expirydate>'{1}' or expirydate is null)", "SC_ANNOUNCEMENT", DateTime.Today.ToString(Consts.DateFormat)));
            _dtAnnouncement = PCDb.Db.ExecQuery(String.Format("select id,title,body,expirydate from CPS_View_SystemAnnouncement"));

        #endregion


            // width="100%" border="0" cellspacing="0" cellpadding="3"
            _htmlt.Width = "100%";
            _htmlt.Border = 0;
            _htmlt.CellSpacing = 0;
            _htmlt.CellPadding = 3;

            HtmlTableRow _trow = null;            
            HtmlTableCell _tcellA = null;
            

            //_IsShowCusMessage = false;

            isShowAnnouncement = true;
            if (isShowAnnouncement)
            {
                if (_dtAnnouncement != null && _dtAnnouncement.Rows.Count > 0)
                {
                    for (int i = 0; i < _dtAnnouncement.Rows.Count; i++)
                    {
                        _trow = new HtmlTableRow();
                        _tcellA = new HtmlTableCell();
                        _tcellA.Attributes.Add("style", "font-weight:bold;");
                        _tcellA.InnerHtml = "<IMG SRC='" + Config.ThemesBaseUrl + "/Default/Images/Larr.gif'  width=\"9\" height=\"12\" align=\"absmiddle\" /> " + _dtAnnouncement.Rows[i]["title"].ToString() + "<hr size=1/>";
                        _trow.Cells.Add(_tcellA);
                        _htmlt.Rows.Add(_trow);

                        _trow = new HtmlTableRow();
                        _tcellA = new HtmlTableCell();
                        _tcellA.Attributes.Add("style", "padding-bottom:20px;");
                        _tcellA.InnerHtml = _dtAnnouncement.Rows[i]["body"].ToString().Replace("\n", "<br/>");
                        _trow.Cells.Add(_tcellA);
                        _htmlt.Rows.Add(_trow);
                    }
                }
                //return _htmlt;
            }
        }
        catch
        {
        }

        return _htmlt;
    }

    void initCustomerHomePageHtmlTableCell(string pTypeID, string pTableName, HtmlTableCell pHtmlTableCell)
    {
        HtmlTableRow _trowUse = null;
        HtmlTableCell _tcellAUse = null;
        HtmlTableCell _tcellBUse = null;
        HtmlAnchor _a = null;
        HtmlAnchor _b = null;
        DataTable _dtUse = null;
        _dtUse = PCDb.Db.ExecQuery(
                      string.Format("SELECT  id,name FROM {0} order by code ", pTableName));
        HtmlTable _htmltUse = new HtmlTable();

        if (_dtUse != null && _dtUse.Rows.Count > 0)
        {

            _htmltUse.Width = "100%";
            _htmltUse.Border = 0;
            _htmltUse.CellSpacing = 0;
            _htmltUse.CellPadding = 3;

            for (int j = 0; j < _dtUse.Rows.Count; j += 2)
            {
                _trowUse = new HtmlTableRow();
                _tcellAUse = new HtmlTableCell();
                _tcellBUse = new HtmlTableCell();
                _tcellAUse.Width = "50%";
                _tcellBUse.Width = "50%";
                _a = new HtmlAnchor();
                _b = new HtmlAnchor();

                if (j < _dtUse.Rows.Count || (j + 1) < _dtUse.Rows.Count)
                {
                    if (j < _dtUse.Rows.Count)
                    {
                        _a.Attributes.Add("class", "ACutHProw");
                        _a.HRef = string.Format("javascript:SelfStaffSearch({0},'{1}')", pTypeID, _dtUse.Rows[j]["id"].ToString());
                        _a.InnerText = _dtUse.Rows[j]["name"].ToString();
                        _a.Title = _dtUse.Rows[j]["name"].ToString();
                    }
                    if ((j + 1) < _dtUse.Rows.Count)
                    {
                        _b.Attributes.Add("class", "ACutHProw");
                        _b.HRef = string.Format("javascript:SelfStaffSearch({0},'{1}')", pTypeID, _dtUse.Rows[j + 1]["id"].ToString());
                        _b.InnerText = _dtUse.Rows[j + 1]["name"].ToString();
                        _b.Title = _dtUse.Rows[j + 1]["name"].ToString();
                    }
                }


                _tcellAUse.Controls.Add(_a);

                _tcellBUse.Controls.Add(_b);


                _trowUse.Cells.Add(_tcellAUse);
                _trowUse.Cells.Add(_tcellBUse);
                _htmltUse.Controls.Add(_trowUse);
            }
            pHtmlTableCell.Controls.Add(_htmltUse);
        }
        else
        {
            _trowUse = new HtmlTableRow();
            _tcellAUse = new HtmlTableCell();
            _a = new HtmlAnchor();
            _a.InnerText = "N / A";
            _a.Attributes.Add("class", "ACutHProw");
            _tcellAUse.Controls.Add(_a);

            _trowUse.Cells.Add(_tcellAUse);
            _htmltUse.Controls.Add(_trowUse);

            pHtmlTableCell.Align = "center";
            pHtmlTableCell.Controls.Add(_htmltUse);
        }
    }

    [AjaxPro.AjaxMethod]
    public bool IsShowStaffSarch(string db)
    {
        SessionInfo.Database = db;
        bool _IsShowStaffSerch = false;
        //DataTable _dtCustomizePage = null;
        //try
        //{
        //    //_IsShowStaffSerch = SystemParameter.Get(PCDb.Db).ShowStaffSearchAtHomePage;

        //    /*
        //    _dtCustomizePage = PCDb.Db.ExecQuery(string.Format("select id,value from sy_systemsetting where (id=21) "));
        //    if (_dtCustomizePage != null && _dtCustomizePage.Rows.Count > 0)
        //    {
        //        _IsShowStaffSerch = Convert.ToDecimal(_dtCustomizePage.Rows[0]["value"]) > 0 ? true : false;
        //    }*/
        //}
        //catch
        //{
        //}
        return _IsShowStaffSerch;
    }
    [AjaxPro.AjaxMethod]
    public bool IsShowStaffInformation(string db)
    {
        SessionInfo.Database = db;
        bool _IsShowStaffInformation = false;
        //DataTable _dtCustomizePage = null;
        //try
        //{
        //    //_IsShowStaffInformation = SystemParameter.Get(PCDb.Db).ShowStaffInfoAtHomePage;
        //    /*
        //    _dtCustomizePage = PCDb.Db.ExecQuery(string.Format("select id,value from sy_systemsetting where (id=22) "));
        //    if (_dtCustomizePage != null && _dtCustomizePage.Rows.Count > 0)
        //    {
        //        _IsShowStaffInformation = Convert.ToDecimal(_dtCustomizePage.Rows[0]["value"]) > 0 ? true : false;
        //    }*/
        //}
        //catch
        //{
        //}
        return _IsShowStaffInformation;
    }

    [AjaxPro.AjaxMethod(AjaxPro.HttpSessionStateRequirement.Read)]
    public bool IsEnableRSASecurityChecking()
    {
        bool _IsEnablecCheck = false;
        bool _IsEnableRSASecurityChecking = false;
        bool _IsOnlyApplyInternetChecking = false;
        string _IPAddress = string.Empty;
        try
        {
            System.Collections.Specialized.NameValueCollection nvc = HttpContext.Current.Request.ServerVariables;
            _IPAddress = nvc["REMOTE_ADDR"].ToString();

            //_IsOnlyApplyInternetChecking = SystemParameter.Get(PCDb.Db).RSACheckInternetOnly;
            /*
            DataTable _dtCheckIntranet = null;
            _dtCheckIntranet = PCDb.Db.ExecQuery(string.Format("select id,value from sy_systemsetting where (id=25) "));
            if (_dtCheckIntranet != null && _dtCheckIntranet.Rows.Count > 0)
            {
                _IsOnlyApplyInternetChecking = Convert.ToDecimal(_dtCheckIntranet.Rows[0]["value"]) > 0 ? true : false;
            }
             */
            //_IsEnableRSASecurityChecking = SystemParameter.Get(PCDb.Db).RSACheckingEnabled;
            /*
            DataTable _dtRSAChecking = null;
            _dtRSAChecking = PCDb.Db.ExecQuery(string.Format("select id,value from sy_systemsetting where (id=23) "));
            if (_dtRSAChecking != null && _dtRSAChecking.Rows.Count > 0)
            {
                _IsEnableRSASecurityChecking = Convert.ToDecimal(_dtRSAChecking.Rows[0]["value"]) > 0 ? true : false;
            }
            */
            if (_IsEnableRSASecurityChecking)
            {
                if (SimpleControls.SimpleNet.IsPublicIPAddress(_IPAddress))
                {
                    _IsEnablecCheck = true;
                }
                else
                {
                    if (_IsOnlyApplyInternetChecking)
                    {
                        _IsEnablecCheck = false;
                    }
                    else
                    {
                        _IsEnablecCheck = true;
                    }
                }
            }
            else
            {
                //Todo : 10.10.0.1 内部网不检测
                _IsEnablecCheck = false;
            }
        }
        catch
        {
        }

        return _IsEnablecCheck;
    }

    protected void ddlDatabase_SelectedIndexChanged(object sender, EventArgs e)
    {

    }
}
