using System;
using System.Data;
using System.Data.Common;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Text;
using PCCore;
using PCCore.PCMS;
using SimpleControls.Web;




public partial class MasterSearch : MasterBasePage
{
    string strtype = "";
    string straction = "";
    string strfunctionid = "";
    
    string userid = "";
    string groupid = "";
    

    bool _automaticMode = true;
    public bool AutomaticMode
    {
        get { return _automaticMode; }
        set { _automaticMode = value; }
    }

    public string RedirectUrl
    {
        get { return SessionInfo.RedirectUrl; }
        set { SessionInfo.RedirectUrl = value; }
    }
    //protected override void OnInit(EventArgs e)
    protected void OnInit(EventArgs e)
    {
        
        //// Setup SessionInfo 
        //_fullfuncCode = Sc.GetFullFunctionCode(Request.Path);
        //_funcCode = Sc.GetFunctionCode(Request.Path);


        //ScInfo _scInfo = new ScInfo(_funcCode);
        //SessionInfo.CurrentModule = _scInfo.ModuleName;
        //SessionInfo.ShowStar = _scInfo.ShowStar;


        //SessionInfo.CurrentModuleID = _scInfo.ModuleId;
        //SessionInfo.CurrentFunctionID = _scInfo.FunctionId;
        //SessionInfo.CurrentFunction = _scInfo.FunctionCode;
        //PCCore.Common.HRLog.RecordLog(SessionInfo.CurrentFunction);
        
        base.OnInit(e);

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        string cssLink = null;
        //ibSearch.ImageUrl = themeUrl + "/images/search.gif";
        
        cssLink = SimpleWebUtils.GetCssLink(Config.AppBaseUrl + "/control/gridview-security.css");
        LiteralControl css = new LiteralControl(cssLink);
        css.ID = "cssID";
        Page.Header.Controls.Add(css);
        if (SessionInfo.IsLogin)
        {
            if (!Page.IsPostBack)
            {
                try
                {
                    strtype = Request.QueryString["type"].ToString();
                    _type.Value = strtype;
                }
                catch (Exception ex) { }
                try
                {
                    straction = Request.QueryString["action"].ToString();
                    _action.Value = straction;
                }
                catch (Exception ex) { }
                try
                {
                    strfunctionid = Request.QueryString["function"].ToString();
                    _function.Value = strfunctionid;
                }
                catch (Exception ex) { }
                try
                {
                    userid = Request.QueryString["user"].ToString();
                    _user.Value = userid;
                }
                catch (Exception ex) { }
                if (userid == null || userid == String.Empty)
                {
                    userid = SessionInfo.UserId;
                }
                try
                {
                    groupid = Request.QueryString["group"].ToString();
                    _group.Value = groupid;
                }
                catch (Exception ex) { }
                PCCore.Common.HRLog.RecordLog(strtype);
                PCCore.Common.HRLog.RecordLog(groupid);
                PCCore.Common.HRLog.RecordLog(userid);

            }
           
            
            if (strtype == String.Empty)
                strtype = _type.Value;
            if (strfunctionid == String.Empty)
                strfunctionid = _function.Value;
            if (straction == String.Empty)
                straction = _action.Value;
            if (userid == String.Empty)
                userid = _user.Value;
            if (groupid == String.Empty)
                groupid = _group.Value;
            PCCore.Common.HRLog.RecordLog("strtype," + strtype);
            PCCore.Common.HRLog.RecordLog("strfunctionid," + strfunctionid);
            PCCore.Common.HRLog.RecordLog("straction," + straction);
            PCCore.Common.HRLog.RecordLog("struserid," + userid);
            PCCore.Common.HRLog.RecordLog("strgroup," + groupid);
            switch (strtype)
            {
                case "project":
                    BindProject(txtSearch1.Text);
                    break;
                case "userproject":
                    BindUserProject(txtSearch1.Text);
                    break;
                case "groupproject":
                    BindGroupProject(txtSearch1.Text);
                    break;
            }
        }

    }

    protected void BindUserProject()
    {
        BindUserProject(String.Empty);
    }

    protected void BindUserProject(string filterString)
    {
        PCCore.Common.HRLog.RecordLog("BindUserProject");
        string sql = "";


        string where = "";
        string tblname = "";
        if (filterString != String.Empty)
        {
            where += string.Format(" and (PrjCode like '%{0}%' or prj.prjname1 like '%{0}%')", txtSearch1.Text.Trim());
        }
        
        if (new PCCore.Database.User(userid).isSupervisor())
        {
            PCCore.Common.HRLog.RecordLog("Supervisor");
            tblname = "CPS_View_ProjectList";
            sql = string.Format("SELECT U_DocEntry as ID, prjcode,prjname as prjname1 From {0} order by prjcode", tblname);
        }
        else
        {
            PCCore.Common.HRLog.RecordLog("Not Supervisor");
            
            tblname = "CPS_View_UserProject";
            sql = string.Format("SELECT ID , prjcode,prjname1 From {0} prj where userid = {1} {2} order by prjcode", tblname,  userid, where);
        }
        PCCore.Common.HRLog.RecordLog(sql);

        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sql;
    }

    protected void BindGroupProject()
    {
        BindGroupProject(String.Empty);
    }

    protected void BindGroupProject(string filterString)
    {
        string sql = "";


        string where = "";
        string tblname = "";
        PCCore.Common.HRLog.RecordLog("groupid," + groupid);
        if (filterString != String.Empty)
        {
            where += string.Format(" and (PrjCode like '%{0}%' or prjname1 like '%{0}%')", txtSearch1.Text.Trim());
        }
        tblname = "CPS_View_GroupProject";
        sql = string.Format("SELECT id, prjcode,prjname1 From {0} prj where id = {1} {2} order by prjcode", tblname, groupid, where);
        
        PCCore.Common.HRLog.RecordLog(sql);

        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sql;
    }

    protected void BindProject()
    {
        BindProject(String.Empty);
    }
    protected void BindProject(string fileterString)
    {
        string sql = "";
        
        
        string where = " where 1 = 1 ";
        string tblname = "";
        if (fileterString != String.Empty)
        {
            where += string.Format(" and (prj.PrjCode like '%{0}%' or prj.U_ProjectFullName like '%{0}%')",txtSearch1.Text.Trim());
        }
        
        switch (straction)
        {
            case Consts.ActionAdd:
                tblname = "CPS_View_AddPermission";
                break;
            case Consts.ActionQuery:
                tblname = "CPS_View_QueryPermission";
                break;
            case Consts.ActionEdit:
                tblname = "CPS_View_EditPermission";
                break;
            case Consts.ActionDelete:
                tblname = "CPS_View_DeletePermission";
                break;
        }
        if (SessionInfo.IsSupervisor)
        {
            sql = "SELECT prj.U_DocEntry as id, prj.PrjCode as PrjCode , prj.U_ProjectFullName as PrjName From PCMS_BE.{0}.dbo.{1} prj ";
            sql = string.Format(sql, ConfigurationManager.AppSettings[SessionInfo.Database].ToString(), "OPRJ");
        }
        else
        {
            sql = "SELECT prj.U_DocEntry as id, sc.ProjCode as PrjCode , prj.U_ProjectFullName as PrjName From {0} sc inner join SC_Userproject up on up.userid = sc.userid and up.prjcode = sc.projcode left join PCMS_BE.{1}.dbo.{2} prj on prj.prjcode = sc.ProjCode ";
            sql = string.Format(sql, tblname, ConfigurationManager.AppSettings[SessionInfo.Database].ToString(), "OPRJ");
            where += string.Format(" and sc.fid = {0} ", strfunctionid);
            where += string.Format(" and sc.userid = {0} ", SessionInfo.UserId);

        }
        sql +=  where;
        sql += " order by prj.prjName";

        PCCore.Common.HRLog.RecordLog(sql);

        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sql;
        
        //dsGridView.ErrorHandler = this;
    }
    
    protected void ibSearch_Click(object sender, EventArgs e)
    {
        strtype = _type.Value;
        switch (strtype)
        {
            case "project":
                BindProject(txtSearch1.Text);
                break;
            case "userprojet":
                BindUserProject(txtSearch1.Text);
                break;
            case "groupproject":
                BindGroupProject(txtSearch1.Text);
                break;
        }
    }
}
