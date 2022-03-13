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
using SimpleControls;
using SimpleControls.SimpleDatabase;
using SimpleControls.Web;
using System.Text;

using System.Data.SqlClient;

public partial class MasterDocument : MasterBasePage, IErrorHandler
{
    
    const string TABLE_NAME = "DocumentProperty";
    string FILE_TYPE = string.Empty;
    string Current_Command = "";
    bool refreshData = false;

    protected PCTable _table = null;
    protected PCTable _view = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;
    
    protected PCTable Table
    {
        get
        {
            if (_table == null) _table = new PCTable("DocumentProperty", this.SecurityInfo);
            return _table;
        }
    }

    protected override void OnInit(EventArgs e)
    {
        if (Page.IsPostBack)
        {
            Current_Command = Request.Form[Consts.HiddenInputCommand];
            //cs.RegisterHiddenField(Consts.HiddenInputSave, Request.Form[Consts.HiddenInputSave]);
        }
        else
        {
            Current_Command = String.Empty;
            //cs.RegisterHiddenField(Consts.HiddenInputSave, String.Empty);
        }
        string cssLink =null;
        switch (SessionInfo.CurrentFunction.ToUpper())
        {
            case Consts.ScDropdownSetting:
            case Consts.ScGroupRightFunc:
            case Consts.ScUserRightFunc:
            case Consts.ScMandatorySetting:
            case Consts.ScFunctionFunc:
            case Consts.ScUserFunctionFunc:
                cssLink = SimpleWebUtils.GetCssLink(Config.AppBaseUrl + "/control/gridview-security-nopagedown.css");
                break;
            default:
                cssLink = SimpleWebUtils.GetCssLink(Config.AppBaseUrl+"/control/gridview-security.css");
                break;
        }
        LiteralControl css = new LiteralControl(cssLink);
        css.ID = "cssID";
        Page.Header.Controls.Add(css);
        string str = Request.Path.ToString();
        FormMode = (FormModes)SimpleUtils.IfNull(Request.QueryString[Consts.QueryStringMode], typeof(FormModes), FormModes.None);
        RecordID = Request.QueryString[Consts.QueryStringID];
        
        base.OnInit(e);
    }
    
    
    //End Copy from MasterForm
    private void InitSecurity()
    {
        if (SessionInfo.IsLogin && IsFirstEnter && SecurityInfo != null)
        {
            ScLog.Insert(ScLog.Actions.Enter, int.Parse(SecurityInfo.FunctionId), SecurityInfo.FunctionName);
        }
    }

    public void CheckCommand(string cmd)
    {
        switch (cmd)
        {
            case Consts.ButtonDelete:
                DeleteRecord();
                break;
            case Consts.Confirm:
                ConfirmRecord();
                break;
            //add by michael, begin
            case Consts.Reverse:
                ReverseRecord();
                break;
            //add by michael, end
        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        refreshData = true;
        FILE_TYPE = SessionInfo.CurrentFunctionID.ToString();
        InitSecurity();
        this.ClearError();
        //tblannouncement =  PCCore.Database.SC_Announcements.GetHtmlTableByFunction(SessionInfo.CurrentFunction);
        PCCore.PCMS.Announcement.GetHtmlTableByFunction(tblannouncement, SessionInfo.CurrentFunction);
        

        
        if (Page.IsPostBack)
        {
            string cmd = this.Current_Command;
            switch (cmd)
            {
                case Consts.ButtonDelete:
                    DeleteRecord();
                    break;
                case Consts.Confirm:
                    ConfirmRecord();
                    break;
                //add by michael, begin
                case Consts.Reverse:
                    ReverseRecord();
                    break;
                //add by michael, end
            }
            if (invProject.Text != String.Empty)
            {
                refreshData = false;
                ddlProject.SelectedValue = invProject.Text;
                invProject.Text = String.Empty;
            }
        }
        else
        {
            this.txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.InitDropDownList();
            
        }
        //Add by Eric
        if (SessionInfo.CurrentFunctionID == "1017" || SessionInfo.CurrentFunctionID == "1012")
        {
             gvData.HiddenFields += "id,descid,description,isError,filesize,DocStatus1,DocEntry,owner,btn";
        }
        //End by Eric
        if (SessionInfo.CurrentFunctionID == "1040" || SessionInfo.CurrentFunctionID == "4001")
        {
            //gvData.HeaderDescriptions += "," + Resources.Labels.Report;
            // update Docuement No. with cut off date by Martin at 24 June 2011
            gvData.HiddenFields += "id,descid,description,isError,filesize,DocStatus,owner,btn";          
            gvData.HeaderDescriptions = ",," + Resources.Labels.FileName + "," + Resources.Labels.CutOffDate + "," + Resources.Labels.SizeKB + "," + Resources.Labels.Description + "," + Resources.Labels.UploadBy + "," + Resources.Labels.UploadTime + "," + Resources.Labels.PostDate + "," + Resources.Labels.Project + "," + Resources.Labels.Status + ",," + Resources.Labels.AccountResponseTime + "," + Resources.Labels.CheckList + ",,," + Resources.Labels.Document;
            
           
        }
        else if (SessionInfo.CurrentFunctionID == "4108")
        {
            gvData.HiddenFields += "id,descid,description,isError,filesize,DocStatus,owner,btn";
            gvData.HeaderDescriptions = ",," + Resources.Labels.FileName + "," + Resources.Labels.Works + "," + Resources.Labels.CutOffDate + "," + Resources.Labels.SizeKB + "," + Resources.Labels.Description + "," + Resources.Labels.UploadBy + "," + Resources.Labels.UploadTime + "," + Resources.Labels.PostDate + "," + Resources.Labels.Status + ",," + Resources.Labels.AccountResponseTime + "," + Resources.Labels.CheckList + ",,," + Resources.Labels.Document;
        }
        //else if (SessionInfo.CurrentFunctionID != "4001")
        //{
        //    gvData.HiddenFields += "id,descid,description,isError,filesize,DocStatus,owner,btn";
        //    gvData.HeaderDescriptions = ",," + Resources.Labels.FileName + "," + Resources.Labels.DocumentNo + "," + Resources.Labels.SizeKB + "," + Resources.Labels.Description + "," + Resources.Labels.UploadBy + "," + Resources.Labels.UploadTime + "," + Resources.Labels.PostDate + "," + Resources.Labels.Project + "," + Resources.Labels.Status + ",," + Resources.Labels.AccountResponseTime + "," + Resources.Labels.CheckList + ",,," + Resources.Labels.Document;
            //gvData.HeaderDescriptions = ",," + Resources.Labels.FileName + "," + Resources.Labels.CutOffDate + "," + Resources.Labels.SizeKB + "," + Resources.Labels.Description + "," + Resources.Labels.UploadBy + "," + Resources.Labels.UploadTime + "," + Resources.Labels.PostDate + "," + Resources.Labels.Project + "," + Resources.Labels.Status + ",," + Resources.Labels.AccountResponseTime + "," + Resources.Labels.CheckList + ",,," + Resources.Labels.Document;
        //}
        else
        {
            //gvData.HiddenFields += "filename,id,descid,description,isError,filesize,DocStatus,owner,btn";
            gvData.HiddenFields += "id,descid,description,isError,filesize,DocStatus,owner,btn";
            gvData.HeaderDescriptions = ",," + Resources.Labels.FileName + "," + Resources.Labels.DocumentNo + "," + Resources.Labels.SizeKB + "," + Resources.Labels.Description + "," + Resources.Labels.UploadBy + "," + Resources.Labels.UploadTime + "," + Resources.Labels.PostDate + "," + Resources.Labels.Project + "," + Resources.Labels.Status + ",," + Resources.Labels.AccountResponseTime + "," + Resources.Labels.View;
        }
        if (SessionInfo.CurrentFunctionID != "1012" && SessionInfo.CurrentFunctionID != "1018" && SessionInfo.CurrentFunctionID != "2005")
            gvData.HiddenFields += ",dt02";

        if (SessionInfo.CurrentFunctionID == "1040" || SessionInfo.CurrentFunctionID == "4108" || SessionInfo.CurrentFunctionID == "4001")
        {
            gvData.HeaderDescriptions += "," + Resources.Labels.Report;
        }
        PCCore.Common.HRLog.RecordLog("HiddenFields: " + gvData.HiddenFields);

        //add by michael, begin
        if (SessionInfo.CurrentFunctionID == "1017" || SessionInfo.CurrentFunctionID == "1018")
            btnReverse.Visible = true;
        else
            btnReverse.Visible = false;
        //add by michael, end

        //add by michael, begin
        if (SessionInfo.CurrentFunctionID == "4108")
            btnExport.Visible = true;
        else
            btnExport.Visible = false;
        //add by michael, end

        if (refreshData)
        {
            if (this.Current_Command != Consts.ButtonExport)
                SetDataSource();
        }
                
    }
    protected void SetDataSource()
    {
        string where = "";
        if (!String.IsNullOrEmpty(txtFileName.Text.Trim()))
        {
            //if (SessionInfo.CurrentFunctionID != "4001")
                //where += string.Format(" and b.filename like '%{0}%'", txtFileName.Text.Trim());
                where += string.Format(" and a.DocNum like '%{0}%'", txtFileName.Text.Trim());
        }

        if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()))
        {
            //if (SessionInfo.CurrentFunctionID != "4001")
            //    where += string.Format(" and b.uploadtime >='{0}'", txtStartDate.Text.Trim());
            //else
                where += string.Format(" and a.created >='{0}'", txtStartDate.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txtEndDate.Text.Trim()))
        {
            //if (SessionInfo.CurrentFunctionID != "4001")
            //    where += string.Format(" and b.uploadtime <='{0} 23:59:59'", txtEndDate.Text.Trim());
            //else
                where += string.Format(" and a.created <='{0} 23:59:59'", txtEndDate.Text.Trim());
        }
        
        /* Remove manual filter by selection */
        
        if (Convert.ToDecimal(ddlUser.SelectedValue) > 0)
        {
            //where += string.Format(" and b.uploadbyid={0}", ddlUser.SelectedValue);
            where += string.Format(" and a.createdby='{0}'", PCCore.Database.User.getLoginName(ddlUser.SelectedValue));
        }
        

        /* Add auto filter by login user */
        /* Display By Project Base */
        //if (!SessionInfo.IsSupervisor)
        //{
        //    if (SessionInfo.CurrentFunctionID != "4001")
        //        where += string.Format(" and b.uploadbyid = {0}", SessionInfo.UserId);
        //    else
        //        where += string.Format(" and a.createdby = '{0}'", SessionInfo.LoginName);
        //}
        
        if (ddlProject.SelectedValue != "-1")
        {
            where += string.Format(" and a.projectCode='{0}'", ddlProject.SelectedValue);
        
        }
        // Begin update by Martin 21 March 2011
        if (ddlStatus.SelectedValue != "-1")
        {
            //where += string.Format(" and b.uploadbyid={0}", ddlUser.SelectedValue);

            string _connectionString = ConfigurationManager.ConnectionStrings["Default"].ConnectionString;

            string[] dbs = null;
            dbs = Config.Database.Split(new char[] { ',' });

            _connectionString = string.Format(_connectionString, dbs[0]);
            string queryString = "select U_Status, Code from [@DocStatus]";
            string code_string = "";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                SqlCommand command = connection.CreateCommand();
                command.CommandText = queryString;
                connection.Open();
                using (SqlDataReader datareader = command.ExecuteReader())
                {
                    while (datareader.Read())
                    {
                        if (datareader["U_Status"] != System.DBNull.Value)
                        {
                            if (datareader["U_Status"].ToString() == ddlStatus.SelectedValue.Trim())
                            {
                                code_string = code_string + "'" + datareader["Code"].ToString() + "',";
                            }
                        }
                    }
                    datareader.Close();
                }
                connection.Close();
            }
            code_string = code_string.Substring(0, code_string.Length - 1);
            if (!string.IsNullOrEmpty(code_string))
            {
                where += string.Format(" and a.docstatus in (" + code_string + ")");
            }
        }
        // End update by Martin 21 March 2011

        PCCore.Database.SC_Function _sc_func = new PCCore.Database.SC_Function(FILE_TYPE);

        // karrson: display field 
        //StringBuilder sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,b.filesize,a.description,c.loginname,convert(varchar(20),b.uploadtime,120) as uploadtime,a.projectCode from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
        //StringBuilder sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,a.Docnum,b.filesize,a.description,c.loginname,convert(varchar(20),b.uploadtime,120) as uploadtime,a.projectCode, d.DisplayName, a.IsError, cast('' as nvarchar(10)) as [Checklist], a.DocStatus from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
        StringBuilder sb;
        //if (SessionInfo.CurrentFunctionID != "4001")
            //sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,a.Docnum,b.filesize,a.description,c.FullName,convert(varchar(20),b.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, cast('' as nvarchar(10)) as [Checklist], a.DocStatus,a.createdby as owner, '' as btn from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
            if (SessionInfo.IsSupervisor)
                sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,a.Docnum,b.filesize,a.description,c.FullName,convert(varchar(20),b.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, convert(varchar(20),a.dt02,120) as dt02 ,cast('' as nvarchar(10)) as [Checklist], a.DocStatus,a.createdby as owner, '' as btn from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
            else
                sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,a.Docnum,b.filesize,a.description,c.FullName,convert(varchar(20),b.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, convert(varchar(20),a.dt02,120) as dt02, cast('' as nvarchar(10)) as [Checklist], a.DocStatus,a.createdby as owner, '' as btn from {0} a inner join CPS_View_QueryPermission p on (p.fid = {3} and p.userid = {4} and p.projcode = a.projectcode)  left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where, SessionInfo.CurrentFunctionID, SessionInfo.UserId));
        //else
            
            //sb = new StringBuilder(string.Format("select a.id ,a.id as descid, '' as filename,a.DocNum,'' as filesize,a.description,c.FullName,convert(varchar(20),a.created,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, cast('' as nvarchar(10)) as [View], a.DocStatus,a.createdby as owner, '' as btn from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.LoginName=a.createdby left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", "CPS_View_PMReportInfo", FILE_TYPE, where));
        //    if (SessionInfo.IsSupervisor)
        //        sb = new StringBuilder(string.Format("select a.id ,a.id as descid, '' as filename,a.DocNum,'' as filesize,a.description,c.FullName,convert(varchar(20),a.created,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, isNull(a.dt02,'') as dt02, cast('' as nvarchar(10)) as [View], a.DocStatus,a.createdby as owner, '' as btn, '' as document from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.LoginName=a.createdby left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", "CPS_View_PMReportInfo", FILE_TYPE, where));
        //    else
        //        sb = new StringBuilder(string.Format("select a.id ,a.id as descid, '' as filename,a.DocNum,'' as filesize,a.description,c.FullName,convert(varchar(20),a.created,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, isNull(a.dt02,'') as dt02,cast('' as nvarchar(10)) as [View], a.DocStatus,a.createdby as owner, '' as btn, '' as document from {0} a inner join CPS_View_QueryPermission p on (p.fid = {3} and p.userid = {4} and p.projcode = a.projectcode) left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.LoginName=a.createdby left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", "CPS_View_PMReportInfo", FILE_TYPE, where, SessionInfo.CurrentFunctionID, SessionInfo.UserId));

            if (SessionInfo.CurrentFunctionID == "1040" || SessionInfo.CurrentFunctionID == "4001")
        {
            if (SessionInfo.IsSupervisor)
                sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,a.dt01 as Docnum,b.filesize,a.description,c.FullName,convert(varchar(20),b.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, convert(varchar(20),a.dt02,120) as dt02 ,cast('' as nvarchar(10)) as [Checklist], a.DocStatus,a.createdby as owner, '' as btn,'' as report from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
            else
                sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,a.dt01 as Docnum,b.filesize,a.description,c.FullName,convert(varchar(20),b.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, convert(varchar(20),a.dt02,120) as dt02, cast('' as nvarchar(10)) as [Checklist], a.DocStatus,a.createdby as owner, '' as btn, '' as report from {0} a inner join CPS_View_QueryPermission p on (p.fid = {3} and p.userid = {4} and p.projcode = a.projectcode)  left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where, SessionInfo.CurrentFunctionID, SessionInfo.UserId));
        }
        if (SessionInfo.CurrentFunctionID == "4108")
        {
            if (SessionInfo.IsSupervisor)
                //sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,a.chr02,a.dt01 as Docnum,b.filesize,a.description,c.FullName,convert(varchar(20),b.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,d.DisplayName, a.IsError,convert(varchar(20),a.dt02,120) as dt02 ,cast('' as nvarchar(10)) as [Checklist], a.DocStatus,a.createdby as owner, '' as btn,'' as report from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
                sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,case a.chr02 when null then null else (select worksname from CPS_View_WorksList where dimcode=4 and workscode=a.chr02) end,a.dt01 as Docnum,b.filesize,a.description,c.FullName,convert(varchar(20),b.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,d.DisplayName, a.IsError,convert(varchar(20),a.dt02,120) as dt02 ,cast('' as nvarchar(10)) as [Checklist], a.DocStatus,a.createdby as owner, '' as btn,'' as report from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
            else
                //sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,a.chr02,a.dt01 as Docnum,b.filesize,a.description,c.FullName,convert(varchar(20),b.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,d.DisplayName, a.IsError,convert(varchar(20),a.dt02,120) as dt02 ,cast('' as nvarchar(10)) as [Checklist], a.DocStatus,a.createdby as owner, '' as btn,'' as report from {0} a inner join CPS_View_QueryPermission p on (p.fid = {3} and p.userid = {4} and p.projcode = a.projectcode)  left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where, SessionInfo.CurrentFunctionID, SessionInfo.UserId));
                sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,case a.chr02 when null then null else (select worksname from CPS_View_WorksList where dimcode=4 and workscode=a.chr02) end,a.dt01 as Docnum,b.filesize,a.description,c.FullName,convert(varchar(20),b.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,d.DisplayName, a.IsError,convert(varchar(20),a.dt02,120) as dt02 ,cast('' as nvarchar(10)) as [Checklist], a.DocStatus,a.createdby as owner, '' as btn,'' as report from {0} a inner join CPS_View_QueryPermission p on (p.fid = {3} and p.userid = {4})  left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where, SessionInfo.CurrentFunctionID, SessionInfo.UserId));
        }

        //Add by Eric
        if (SessionInfo.CurrentFunctionID == "1017" || SessionInfo.CurrentFunctionID == "1012")
        {
            if (SessionInfo.IsSupervisor)
                sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,a.DocNum,b.filesize,a.description,c.FullName,convert(varchar(20),b.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, convert(varchar(20),a.dt02,120) as dt02 ,cast('' as nvarchar(10)) as [Checklist], a.DocStatus,a.createdby as owner, '' as btn,'' as report, a.DocStatus as DocStatus, a.DocEntry as DocEntry from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
            else
                sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,a.DocNum,b.filesize,a.description,c.FullName,convert(varchar(20),b.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, convert(varchar(20),a.dt02,120) as dt02, cast('' as nvarchar(10)) as [Checklist], a.DocStatus,a.createdby as owner, '' as btn, '' as report, a.DocStatus as DocStatus, a.DocEntry as DocEntry from {0} a inner join CPS_View_QueryPermission p on (p.fid = {3} and p.userid = {4} and p.projcode = a.projectcode)  left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where, SessionInfo.CurrentFunctionID, SessionInfo.UserId));
        }
        //End

        //sb.Append(SessionInfo.DataFilter);
        if (_sc_func.ProjectFunction())
        {
            if (!SessionInfo.IsSupervisor)
            {
                sb.Append(SessionInfo.DataFilter);
            }
        }
        else

            sb.Append(" and (ProjectCode is null or ProjectCode in ('-1','-2','-3')) ");

        //if (SessionInfo.CurrentFunctionID != "4001")
            sb.Append(" order by b.uploadtime desc");
        //else
        //    sb.Append(" order by a.created desc");
        
        PCCore.Common.HRLog.RecordLog(sb.ToString());
        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this;
    }
    protected void btnConfirm_Click(object sender, EventArgs e) { }
    //add by michael, begin
    protected void btnReverse_Click(object sender, EventArgs e) { }
    //add by michael, end
    //add by michael, begin
    protected void btnExport_Click(object sender, EventArgs e) { }
    //add by michael, end

    void CheckDeletePrerequisite(PCTable table, Hashtable row)
    {
        int count = (int)PCDb.Db.ExecScalar(string.Format("select count(*) from CM_SessionFiles where recordid in (select recordid from CM_SessionFiles where id={0})", row["ID"].ToString()));
        if (count > 1) return;
        //string id = row[Consts.FieldID].ToString();
        string id = PCDb.Db.ExecScalar(string.Format("select recordid from CM_SessionFiles where id={0}", row["ID"].ToString())).ToString();

        string where = String.Format("id={0}", id);
        PCTable desc = new PCTable(TABLE_NAME, this.SecurityInfo);
        desc.UseTransaction(table.InternalTransaction);
        desc.Delete(where);
    }
    void ConfirmRecord()
    {
        
        try
        {
            
            PCCore.Common.HRLog.RecordLog("Confirm");
            PCCore.Common.HRLog.RecordLog(gvData.SelectedRowValue.ToString());
            PCCore.Common.HRLog.RecordLog(FILE_TYPE);
            
            PCDb.Db.ExecQuery(string.Format("exec sp_PCMS_toConfirm {0},{1}", gvData.SelectedRowValue.ToString(), FILE_TYPE));
            
            
        }
        catch (Exception ex)
        {
            throw ex;
        }
        
    }

    //add by Michael, begin
    void ReverseRecord()
    {

        try
        {

            PCCore.Common.HRLog.RecordLog("Reverse");
            PCCore.Common.HRLog.RecordLog(gvData.SelectedRowValue.ToString());
            PCCore.Common.HRLog.RecordLog(FILE_TYPE);

            PCDb.Db.ExecQuery(string.Format("exec sp_PCMS_toReverse {0},{1}", gvData.SelectedRowValue.ToString(), FILE_TYPE));


        }
        catch (Exception ex)
        {
            throw ex;
        }

    }
    // add by Michael, End

    void DeleteRecord()
    {
        this.DeleteRecord("CM_SessionFiles", gvData, CheckDeletePrerequisite);
    }
    #region DeleteRecord

    public override void DeleteRecord(string tableName, PCCore.GridView gridView, CheckDeletePrerequisite checkPrerequisite)
    {
        PCTable table = new PCTable(tableName, this.SecurityInfo);
        try
        {
            table.BeginTransaction();
            Hashtable row = new Hashtable();
            row.Add(Consts.FieldID, String.Empty);
            if (gridView.AllowMultipleSelect)
            {
                ICollection ic = gridView.SelectedRowValuesCollection;
                IEnumerator ie = ic.GetEnumerator();
                ie.Reset();
                while (ie.MoveNext())
                {
                    row[Consts.FieldID] = ie.Current;
                    if (checkPrerequisite != null)
                        checkPrerequisite(table, row);
                    table.Delete(row);
                }
            }
            else
            {
                row[Consts.FieldID] = gridView.SelectedRowValue;
                if (checkPrerequisite != null)
                    checkPrerequisite(table, row);
                table.Delete(row);
            }
            table.CommitTransaction();
            ShowMessage(Resources.Common.DeleteOK);
        }
        catch (Exception ex)
        {
            gridView.ClearSelectedRowValues = false;
            HandleError(ex);
            table.RollbackTransaction();
        }
    }

    public override void DeleteRecord(string tableName, PCCore.GridView gridView, CheckDeletePrerequisite checkPrerequisite, SimpleNote pNote)
    {
        PCTable table = new PCTable(tableName, this.SecurityInfo);
        try
        {
            table.BeginTransaction();
            Hashtable row = new Hashtable();
            row.Add(Consts.FieldID, String.Empty);
            if (gridView.AllowMultipleSelect)
            {
                ICollection ic = gridView.SelectedRowValuesCollection;
                IEnumerator ie = ic.GetEnumerator();
                ie.Reset();
                while (ie.MoveNext())
                {
                    row[Consts.FieldID] = ie.Current;
                    if (checkPrerequisite != null)
                        checkPrerequisite(table, row);
                    table.Delete(row);
                }
            }
            else
            {
                row[Consts.FieldID] = gridView.SelectedRowValue;
                if (checkPrerequisite != null)
                    checkPrerequisite(table, row);
                table.Delete(row);
            }
            table.CommitTransaction();
            //ShowMessage(Resources.Common.DeleteOK);
            pNote.ShowMessage(Resources.Common.DeleteOK);
        }
        catch (Exception ex)
        {
            gridView.ClearSelectedRowValues = false;
            //HandleError(ex);
            PCCore.ComFunction2.HandleError(ex,pNote);
            
            table.RollbackTransaction();
        }
    }

    #endregion

    public override void ClearForm()
    {
        foreach (Control c in this.ContentPlaceHolder1.Controls)
        {
            switch (c.GetType().Name)
            {
                case "TextBox":
                case "SimpleTextBox":
                    System.Web.UI.WebControls.TextBox tb = c as System.Web.UI.WebControls.TextBox;
                    tb.Text = String.Empty;
                    break;
                case "CheckBox":
                    System.Web.UI.WebControls.CheckBox chk = c as System.Web.UI.WebControls.CheckBox;
                    chk.Checked = false;
                    break;
            }
        }
    }

    #region Error Handling
    public override void HandleError(Exception ex)
    {
        Note.HRShowException(ex);
        Note.Visible = true;
        NoteSepLine.Visible = true;
    }

    public override void ClearError()
    {
        Note.Clear();
        Note.Visible = false;
        NoteSepLine.Visible = false;
    }

    public override void ShowMessage(string message)
    {
        ShowMessage(message, null);
    }

    public override void ShowMessage(string message, string tooltip)
    {
        Note.ShowMessage(message, tooltip);
        NoteSepLine.Visible = true;
    }

    public override void ShowWarning(string warning)
    {
        ShowWarning(warning, null);
    }

    public override void ShowWarning(string warning, string tooltip)
    {
        Note.ShowWarning(warning, tooltip);
        NoteSepLine.Visible = true;
    }
    #endregion Error Handling

    //public ToolBar ToolBar
    //{
    //    get { return tbToolBar; }
    //}

    public SimpleNote MasterNote
    {
        get { return Note; }
    }

    public HtmlTableRow MasterNoteSepLine
    {
        get { return NoteSepLine; }
    }

    private void InitDropDownList()
    {
        //PCDb.InitDropDownList(this.ddlUser, "sc_user", "ID", "loginName", Consts.DropDownOptionAll, null);
        if (SessionInfo.IsSupervisor)
            PCDb.InitDropDownList(this.ddlUser, "sc_user", "id", "FullName", Consts.DropDownOptionAll, null);
        else
            PCDb.InitDropDownList(this.ddlUser, "sc_user", "id", "FullName", Consts.DropDownOptionAll, null,string.Format("ID = {0}",SessionInfo.UserId));
        //PCDb.InitDropDownList(this.ddlProject, "v_project", "PrjCode", "U_PrjFName", Consts.DropDownOptionNone, null, string.Format("prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','")));
        //PCCore.Database.ValidationList.ProjectList(ddlProject);

        // Begin update by Martin 21 March 2011
        PCDb.InitDropDownList2(this.ddlStatus, "[@DocStatus]", "U_Status", "U_DisplayName", "U_Status", Consts.DropDownOptionAll, null);
        // End update by Martin 21 March 2011

        PCCore.Database.ValidationList.AllowQueryDropDownProjectList(ddlProject, Consts.DropDownOptionAll);
    



    }

    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        //e.Row.Cells[2].Wrap = true;
        //e.Row.Cells[2].Width = System.Web.UI.WebControls.Unit.Percentage(15);

        switch (e.Row.RowType)
        {
                

            case DataControlRowType.DataRow:

                // Create Post Button

                if (!string.IsNullOrEmpty(e.Row.Cells[1].Text.Trim()) && e.Row.Cells[1].Text.Trim() != "&nbsp;")
                {
                    e.Row.Cells[2].Text = string.Format("<a href='javascript:DownloadFile({0})'>{1}</a>", e.Row.Cells[0].Text, e.Row.Cells[2].Text);

                    //if (e.Row.Cells[2].Text.Length > 25)
                    //{
                    //    e.Row.Cells[2].Text = string.Format("<a href='javascript:DownloadFile({0})'>{1}</a>", e.Row.Cells[0].Text, e.Row.Cells[2].Text.Substring(0,22) + "...");
                    //}
                    //else
                    //{
                    //    e.Row.Cells[2].Text = string.Format("<a href='javascript:DownloadFile({0})'>{1}</a>", e.Row.Cells[0].Text, e.Row.Cells[2].Text);
                    //}
                    
                }
                
                //switch On screen checklist
                //if (e.Row.Cells[12].Text.Trim() != Consts.DOCUMENT_DRAFT_PENDING)
                if (e.Row.Cells[14].Text.Trim() == Consts.DOCUMENT_DRAFT_REJECTED)
                {
                    //if (SessionInfo.CurrentFunctionID != "4001")
                        e.Row.Cells[13].Text = string.Format("<a href='javascript:PrintCheckList({0})'>{1}</a>", e.Row.Cells[1].Text, Resources.Labels.View);

                    
                }
                if (SessionInfo.CurrentFunctionID == "1040" || SessionInfo.CurrentFunctionID == "4108" || SessionInfo.CurrentFunctionID == "4001")
                {
                    if (e.Row.Cells[14].Text.Trim() == Consts.DOCUMENT_DRAFT || e.Row.Cells[14].Text.Trim() == Consts.DOCUMENT_POSTED)
                    {
                        e.Row.Cells[17].Text = string.Format("<a href='javascript:PrintReport({0})'>{1}</a>", e.Row.Cells[1].Text, Resources.Labels.Report);
                    }
                }

                //add by Michael, begin
                if (e.Row.Cells[14].Text.Trim() == Consts.DOCUMENT_POSTED)
                {
                    if (SessionInfo.CurrentFunctionID == "1017" || SessionInfo.CurrentFunctionID == "1018")
                    {
                        int count = (int)PCDb.Db.ExecScalar(string.Format("select count(*) from DocumentMessage where ID={0}", e.Row.Cells[1].Text));
                        if (count != 0)
                            e.Row.Cells[13].Text = string.Format("<a href='javascript:PrintCheckList({0})'>{1}</a>", e.Row.Cells[1].Text, Resources.Labels.View);
                    }
                } 
                //add by Michael, end

                //add by Eric
                if (SessionInfo.CurrentFunctionID == "1017" || SessionInfo.CurrentFunctionID == "1012")
                {
                    if (e.Row.Cells[14].Text.Trim() == Consts.DOCUMENT_DRAFT || e.Row.Cells[14].Text.Trim() == Consts.DOCUMENT_POSTED || e.Row.Cells[14].Text.Trim() == "PPFA")
                    {
                        e.Row.Cells[17].Text = "<a href='javascript:PrintDocumentReport(" + "\"" + e.Row.Cells[1].Text + "\"" + ",\"" + e.Row.Cells[19].Text + "\"" + "," + "\"" + e.Row.Cells[18].Text + "\")'>" + Resources.Labels.Report + "</a>";
                    }
                }
                //

                //if (SessionInfo.CurrentFunctionID == "4001")
                //{
                    
                //    string status = e.Row.Cells[14].Text.Trim();
                //    if (status != Consts.DOCUMENT_POST_PENDING_SUBMIT)
                //    {
                        //"<a href='javascript:PrintDocumentReport(" + "\"" + e.Row.Cells[1].Text + "\"" + ",\"" + e.Row.Cells[14].Text + "\"" + "," + "\"" + Status + "\")'>" + Resources.Labels.View + "</a>";
                        //e.Row.Cells[11].Text = string.Format("<a href='javascript:PMReport('{0}','{1}')'>{2}</a>", e.Row.Cells[1].Text,status, Resources.Labels.View);
                //        e.Row.Cells[13].Text = "<a href='javascript:PMReport(" + e.Row.Cells[1].Text + "," + "\"" + status + "\")'>" + Resources.Labels.View + "</a>";
                //    }
                //    e.Row.Cells[17].Text = string.Format("<a href='{0}' target='_blank'>",Config.AttachmentPath.Replace("%PROJECT%",e.Row.Cells[9].Text.ToString().Trim())) + Resources.Labels.Document + "</a>";
                //}
                e.Row.Cells[4].Style.Add("padding-right", "5px");
                break;
        }
    }

    protected void gvData_Sorted(object sender, EventArgs e)
    {
        //String s = gvData.SelectedRow.Cells[0].Text;
    }


    protected void gvData_RowCommand(object sender, EventArgs e)
    {
        //String s = "";

    }
    protected void gvData_SelectedIndexChanging(object sender, EventArgs e)
    {
        //String s = gvData.SelectedRow.Cells[0].Text;
    }

    protected void gvData_SelectedChanged(object sender, EventArgs e)
    {
        String s = gvData.SelectedRow.Cells[0].Text;
    }
    public void Test()
    {
    }
    PCCore.GridView gridView
    {
        get { return this.gvData; }
    }
    
}

