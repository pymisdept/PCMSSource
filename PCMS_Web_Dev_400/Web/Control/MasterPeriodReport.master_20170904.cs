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

public partial class MasterPeriodReport : MasterBasePage, IErrorHandler
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

        }
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PCCore.PCMS.Announcement.GetHtmlTableByFunction(tblannouncement, SessionInfo.CurrentFunction);
        if (SessionInfo.CurrentFunctionID == "4106" || SessionInfo.CurrentFunctionID == "4107" || SessionInfo.CurrentFunctionID == "4108" || SessionInfo.CurrentFunctionID == "4111" || SessionInfo.CurrentFunctionID == "4112" || SessionInfo.CurrentFunctionID == "4113")
        {
            ddlProject.Enabled = false;
            ibSearchProject.Enabled = false;


        }
        else
        {
            ddlProject.Enabled = true;
            ibSearchProject.Enabled = true;
        }
        refreshData = true;
        FILE_TYPE = SessionInfo.CurrentFunctionID.ToString();
        InitSecurity();
        this.ClearError();

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
            // Management Report
        if (SessionInfo.CurrentFunctionID == "4106" || SessionInfo.CurrentFunctionID == "4107" || SessionInfo.CurrentFunctionID == "4108" || SessionInfo.CurrentFunctionID == "4111" || SessionInfo.CurrentFunctionID == "4112" || SessionInfo.CurrentFunctionID == "4113")
        {
            gvData.HiddenFields += "projectcode,prjentry, duedate,filename,id,descid,description,isError,filesize,DocStatus,owner,btn";
        }
        else
        {
            gvData.HiddenFields += "prjentry, duedate,filename,id,descid,description,isError,filesize,DocStatus,owner,btn";
        }

            gvData.HeaderDescriptions = ",," + Resources.Labels.FileName + "," + Resources.Labels.DocumentNo + "," + Resources.Labels.SizeKB + "," + Resources.Labels.Description + "," + Resources.Labels.Printby + "," + Resources.Labels.PrintTime + "," + Resources.Labels.PostDate + "," + Resources.Labels.Project + "," + Resources.Labels.Status + "," + Resources.Labels.CheckList + "," + Resources.Labels.Report + "," + Resources.Labels.CheckList + "," + "DocEntry" + "," + "Due Date";
        

        if (refreshData)
        {
            if (this.Current_Command != Consts.ButtonExport)
                SetDataSource();
        }
        

        
    }
    protected void SetDataSource()
    {
        string where = "";
        
        if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()))
        {
           
                where += string.Format(" and a.created >='{0}'", txtStartDate.Text.Trim());
            
        }
        if (!string.IsNullOrEmpty(txtEndDate.Text.Trim()))
        {
                where += string.Format(" and a.created <='{0} 23:59:59'", txtEndDate.Text.Trim());
            
        }
        
        /* Remove manual filter by selection */
        if (SessionInfo.IsSupervisor)
        {
            if (Convert.ToDecimal(ddlUser.SelectedValue) > 0)
            {
                //where += string.Format(" and b.uploadbyid={0}", ddlUser.SelectedValue);
                where += string.Format(" and a.createdby='{0}'", PCCore.Database.User.getLoginName(ddlUser.SelectedValue));
            }
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
        /* Add auto filter by login user */

        //if (!SessionInfo.IsSupervisor)
        //{
        //        where += string.Format(" and a.createdby = '{0}'", SessionInfo.LoginName);
            
        //}

        if (SessionInfo.CurrentFunctionID == "4106" || SessionInfo.CurrentFunctionID == "4107" || SessionInfo.CurrentFunctionID == "4108" || SessionInfo.CurrentFunctionID == "4111" || SessionInfo.CurrentFunctionID == "4112" || SessionInfo.CurrentFunctionID == "4113")
        {
            if (ddlProject.SelectedValue != "-1")
            {
                where += string.Format(" and a.projectCode='{0}'", ddlProject.SelectedValue);

            }
        }
        if (txtFileName.Text != String.Empty)
        {
            where += string.Format(" and a.DocNum like '%{0}%' ", txtFileName.Text);
        }
        /*

        if (DropDownList1.SelectedValue != "-1")
        {
            
            where += string.Format(" and a.projectCode='{0}'", DropDownList1.SelectedValue);
        }
        */

        PCCore.Database.SC_Function _sc_func = new PCCore.Database.SC_Function(FILE_TYPE);

        if (SessionInfo.IsSupervisor == false && SessionInfo.CurrentFunctionID != "1125")
        {
            where += string.Format(" and Exists( SELECT 1 FROM CPS_VIEW_QUERYPERMISSION WHERE FID = {0} AND USERID = {1} ) ", SessionInfo.CurrentFunctionID, SessionInfo.UserId);
        }
        
        StringBuilder sb;
        
        //sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,a.Docnum,b.filesize,a.description,c.FullName,convert(varchar(20),b.uploadtime,120) as uploadtime,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, cast('' as nvarchar(10)) as [Checklist], a.DocStatus from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
        //sb = new StringBuilder(string.Format("select a.id ,a.id as descid, '' as filename,a.Docnum,'' as filesize,a.description,c.FullName,a.createdate as uploadtime,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, cast('' as nvarchar(10)) as Report, a.DocStatus from {0} a left join sc_user c on c.loginname = a.createdby left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
        
        if (SessionInfo.CurrentFunctionID == "1125")
        {
            //sb = new StringBuilder(string.Format("select a.id ,a.id as descid, '' as filename,a.DocNum as Docnum,'' as filesize,a.description,c.FullName,convert(varchar(20),a.created,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, cast('' as nvarchar(10)) as [Report], a.DocStatus, prj.U_DocEntry as prjentry, prjrpth.DocDate as duedate from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.LoginName=a.createdby left join CPS_View_DocStatus d on (d.Code = a.DocStatus) left join PCMS_BE.{3}.DBO.OPRJ prj on prj.prjcode = a.ProjectCode left join PCMS_BE.{3}.DBO.CPSMPC_RPTH  prjrpth on prjrpth.DocEntry = a.id where type={1} {2}", Consts.Table_DocumentProperty, FILE_TYPE, where, Config.SAPDatabase));
            if (SessionInfo.IsSupervisor)
                sb = new StringBuilder(string.Format("select a.id ,a.id as descid, '' as filename,a.DocNum as Docnum,'' as filesize,a.description,c.FullName,convert(varchar(20),a.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, cast('' as nvarchar(10)) as [Report], a.DocStatus, prj.U_DocEntry as prjentry, a.dt01 as duedate, a.createdby as owner, '' as btn,'' as CheckList from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.LoginName=a.createdby left join CPS_View_DocStatus d on (d.Code = a.DocStatus) left join PCMS_BE.{3}.DBO.OPRJ prj on prj.prjcode = a.ProjectCode where type={1} {2}", Consts.Table_DocumentProperty, FILE_TYPE, where, Config.SAPDatabase));
            else
                sb = new StringBuilder(string.Format("select a.id ,a.id as descid, '' as filename,a.DocNum as Docnum,'' as filesize,a.description,c.FullName,convert(varchar(20),a.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, cast('' as nvarchar(10)) as [Report], a.DocStatus, prj.U_DocEntry as prjentry, a.dt01 as duedate, a.createdby as owner, '' as btn, '' as CheckList from {0} a inner join CPS_View_QueryPermission p on (p.fid = {4} and p.userid = {5} and p.projcode = a.projectcode) left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.LoginName=a.createdby left join CPS_View_DocStatus d on (d.Code = a.DocStatus) left join PCMS_BE.{3}.DBO.OPRJ prj on prj.prjcode = a.ProjectCode where type={1} {2}", Consts.Table_DocumentProperty, FILE_TYPE, where, Config.SAPDatabase, SessionInfo.CurrentFunctionID,SessionInfo.UserId));
        }
        else
        {
            if (SessionInfo.IsSupervisor)
                sb = new StringBuilder(string.Format("select a.id ,a.id as descid, '' as filename,a.DocNum as Docnum,'' as filesize,a.description,c.FullName,convert(varchar(20),a.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, cast('' as nvarchar(10)) as [Report], a.DocStatus, '' as prjentry, a.dt01 as duedate,a.createdby as owner,'' as btn from {0} a  left join sc_user c on c.LoginName=a.createdby left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", Consts.Table_DocumentProperty, FILE_TYPE, where));
            else
                sb = new StringBuilder(string.Format("select a.id ,a.id as descid, '' as filename,a.DocNum as Docnum,'' as filesize,a.description,c.FullName,convert(varchar(20),a.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,case when (a.projectCode is null or a.projectCode in ('-1','-2','-3')) then '' else a.projectCode end as ProjectCode, d.DisplayName, a.IsError, cast('' as nvarchar(10)) as [Report], a.DocStatus, '' as prjentry, a.dt01 as duedate, a.createdby as owner,'' as btn from {0} a  left join sc_user c on c.LoginName=a.createdby left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", Consts.Table_DocumentProperty, FILE_TYPE, where));
        }
        //sb.Append(SessionInfo.DataFilter);
        
        if (_sc_func.ProjectFunction())
        {
            
            if (ddlProject.SelectedValue != "-1" && ddlProject.SelectedValue != "-2" && ddlProject.SelectedValue != "-3")
            {
                sb.Append(string.Format(" and ProjectCode = '{0}'",ddlProject.SelectedValue));
            }
            if (!SessionInfo.IsSupervisor)
            {
                sb.Append(SessionInfo.DataFilter);
            }
        }
        else
        {
            sb.Append(" and (ProjectCode is null or ProjectCode in ('-1','-2','-3')) ");
        }
        if (SessionInfo.CurrentFunctionID != "4001")
            sb.Append(" order by b.uploadtime desc");
        else
            sb.Append(" order by a.created desc");

        PCCore.Common.HRLog.RecordLog(sb.ToString());

        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this;

    }
    protected void btnConfirm_Click(object sender, EventArgs e) { }

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
        //String s = gvData.SelectedValue.ToString();
        //Hashtable _ht = new Hashtable();

        //_ht.Add(Consts.FieldID, gvData.SelectedRowValue.ToString());
        //_ht.Add("DocStatus", "CP");
        try
        {
            //this.Table.Db.ExecQuery("UPDATE DOCUMENTPROPERTY SET DOCSTATUS = 'CP' WHERE ID = " + gvData.SelectedRowValue.ToString());
            PCCore.Common.HRLog.RecordLog("Confirm");
            PCCore.Common.HRLog.RecordLog(gvData.SelectedRowValue.ToString());
            PCCore.Common.HRLog.RecordLog(FILE_TYPE);
            PCDb.Db.ExecQuery(string.Format("exec sp_PCMS_toConfirm {0},{1}", gvData.SelectedRowValue.ToString(),FILE_TYPE));
            //this.Table.Db.ExecQuery(string.Format("UPDATE {0} SET DOCSTATUS = '{1}' WHERE ID = {2}",TABLE_NAME,Consts.DOCUMENT_POST_PENDING_DA,gvData.SelectedRowValue.ToString()));
        }
        catch (Exception ex)
        {
            throw ex;
        }
        //this.Table.BeginTransaction();   
        //this.Table.Update(_ht);
        //this.Table.CommitTransaction();
    }
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
            PCDb.InitDropDownList(this.ddlUser, "sc_user", "ID", "FullName", Consts.DropDownOptionAll, null);
        
        //PCDb.InitDropDownList(this.ddlProject, "v_project", "PrjCode", "U_PrjFName", Consts.DropDownOptionNone, null, string.Format("prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','")));
        PCCore.Database.ValidationList.ProjectList(ddlProject);

        PCDb.InitDropDownList2(this.ddlStatus, "[@DocStatus]", "U_Status", "U_DisplayName", "U_Status", Consts.DropDownOptionAll, null);

    }

    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        switch (e.Row.RowType)
        {


            case DataControlRowType.DataRow:

                //switch On screen checklist
                //if (e.Row.Cells[12].Text.Trim() != Consts.DOCUMENT_DRAFT_PENDING)
                
                //e.Row.Cells[11].Text = string.Format("<a href='javascript:ShowReport({0})'>{1}</a>", e.Row.Cells[1].Text, Resources.Labels.Report);
                DateTime _duedate;
                try
                {
                    _duedate = Convert.ToDateTime(e.Row.Cells[15].Text);

                    if (e.Row.Cells[13].Text != Consts.DOCUMENT_DRAFT_REJECTED && e.Row.Cells[13].Text != Consts.DOCUMENT_CANCELED
                                && e.Row.Cells[13].Text != Consts.DOCUMENT_DRAFT_PENDING)
                    {
                        //e.Row.Cells[12].Text = "<a href='javascript:ShowReport(" + e.Row.Cells[0].Text + ",\"" + _duedate.ToString("yyyy-MM-dd") + "\"," + SessionInfo.UserId.ToString() + ");'>" + Resources.Labels.Report + "</a>";
                        //Begin update by Martin 22 March 2011

                        e.Row.Cells[12].Text = "<a href='javascript:ShowReport(" + e.Row.Cells[0].Text + ",\"" + _duedate.ToString("yyyy-MM-dd") + "\",\"" + SessionInfo.UserName.ToString() + "\");'>" + Resources.Labels.Report + "</a>";

                        //End update by Martin 22 March 2011
                    }
                    PCCore.Common.HRLog.RecordLog(e.Row.Cells[11].Text);
                    e.Row.Cells[4].Style.Add("padding-right", "5px");
                }
                catch (Exception ex)
                {

                }
                //if (e.Row.Cells[13].Text != Consts.DOCUMENT_DRAFT_REJECTED && e.Row.Cells[13].Text != Consts.DOCUMENT_CANCELED)
                //{
                //    if (SessionInfo.CurrentFunctionID == "4106" || SessionInfo.CurrentFunctionID == "4107" || SessionInfo.CurrentFunctionID == "4108" || SessionInfo.CurrentFunctionID == "4111" || SessionInfo.CurrentFunctionID == "4112" || SessionInfo.CurrentFunctionID == "4113")
                //    {
                //        e.Row.Cells[12].Text = "<a href='javascript:ShowReport(" + e.Row.Cells[0].Text + ",\"" + "NA" + "\"," + SessionInfo.UserId.ToString() + ");'>" + Resources.Labels.Report + "</a>";
                //    }
                //}
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

