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

public partial class MasterReport : MasterBasePage, IErrorHandler
{

    const string TABLE_NAME = "DocumentProperty";
    string FILE_TYPE = "";
    string Current_Command = "";
    protected override void OnInit(EventArgs e)
    {
        FILE_TYPE = PCCore.Database.ValidationList.FunctionFileType(SessionInfo.CurrentFunctionID);
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

    protected void Page_Load(object sender, EventArgs e)
    {
        InitSecurity();
        ClearError();

        if (Page.IsPostBack)
        {
            Current_Command = Request.Form[Consts.HiddenInputCommand];
            if (invProjCode.Text != String.Empty)
            {
                ddlProject.SelectedValue = invProjCode.Text;
                invProjCode.Text = String.Empty;
            }
        }
        else
        {
            this.txtStartDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            this.txtEndDate.Text = DateTime.Now.ToString("yyyy-MM-dd");
            Current_Command = String.Empty;
            this.InitDropDownList();

        }

        gvData.HiddenFields += "filename,Docnum_1,descid,description,DocumentType,filename,filesize,DocStatus,DocEntry";
        gvData.HeaderDescriptions = "ID,," + Resources.Labels.FileName + "," + Resources.Labels.SizeKB + "," + Resources.Labels.Description + ",," + Resources.Labels.DocumentType + "," + Resources.Labels.DocumentNo + "," + Resources.Labels.UploadBy + "," + Resources.Labels.UploadTime + "," + Resources.Labels.PostDate + "," + Resources.Labels.Project + "," + Resources.Labels.Status + "," + Resources.Labels.Report + "," + Resources.Labels.Docentry + "," + Resources.Labels.Attachments;
        //                          b.id ,a.id as descid, b.filename,                                       b.filesize              ,a.description                        ,a.DocNum,'0' as DocumentType,              a.DocNum,                           c.loginname,                      convert(varchar(20),b.uploadtime,120) as uploadtime,a.projectCode,d.DisplayName, cast('' as nvarchar(50)) as [Report],a.DocStatus as DocStatus , isNull(Cast(a.docentry as nvarchar(50)),'-1') as DocEntry
        if (this.Current_Command != Consts.ButtonExport)
            SetDataSource();

    }

    protected void SetDataSource()
    {
        string where = "";
        if (!String.IsNullOrEmpty(txtFileName.Text.Trim()))
        {
            //where += string.Format(" and b.filename like '%{0}%'", txtFileName.Text.Trim());
            where += string.Format(" and a.DocNum like '%{0}%'", txtFileName.Text.Trim());
        }

        if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()))
        {
            where += string.Format(" and b.uploadtime >='{0}'", txtStartDate.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txtEndDate.Text.Trim()))
        {
            where += string.Format(" and b.uploadtime <='{0} 23:59:59'", txtEndDate.Text.Trim());
        }

        if (Convert.ToDecimal(ddlUser.SelectedValue) > 0)
        {
            where += string.Format(" and b.uploadbyid={0}", ddlUser.SelectedValue);
        }

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
        //StringBuilder sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,b.filesize,a.description,a.DocNum,case when DocStatus = 'UA' then 'Draft' else 'Open' end as [Document Type], a.DocNum,c.loginname,convert(varchar(20),b.uploadtime,120) as uploadtime,a.projectCode,d.StatusName, cast('' as nvarchar(50)) as [Report],a.DocStatus as DocStatus , isNull(Cast(a.docentry as nvarchar(50)),'-1') as DocEntry from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join Status d on d.StatusCode = a.DocStatus where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
        //StringBuilder sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,b.filesize,a.description,a.DocNum as DocNum_1,'0' as DocumentType, a.DocNum,c.loginname,convert(varchar(20),b.uploadtime,120) as uploadtime,a.projectCode,d.DisplayName, cast('' as nvarchar(50)) as [Report],a.DocStatus as DocStatus , isNull(Cast(a.docentry as nvarchar(50)),'-1') as DocEntry, '' as Attachment from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on d.Code = a.DocStatus where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
        StringBuilder sb;
        if (SessionInfo.CurrentFunctionID == "1125")
        {
            FILE_TYPE = "1040";
        }
        sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,b.filesize,a.description,a.DocNum as DocNum_1,'0' as DocumentType, a.DocNum,c.FullName,convert(varchar(20),b.uploadtime,120) as uploadtime,convert(varchar(20),a.modified,120) as postdate,a.projectCode,d.DisplayName, cast('' as nvarchar(50)) as [Report],a.DocStatus as DocStatus , isNull(Cast(a.docentry as nvarchar(50)),'-1') as DocEntry, '' as Attachment from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on d.Code = a.DocStatus where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
        // Query Project Permission
        PCCore.Database.SC_Function _sc_function = new PCCore.Database.SC_Function(FILE_TYPE);
        if (_sc_function.ProjectFunction() && SessionInfo.IsSupervisor == false)
        {
            sb.Append(string.Format(" and exists(select 1 from {0} sr where sr.projcode = a.projectCode and sr.fid = {1} and sr.userid = {2} )", "CPS_View_QueryPermission", FILE_TYPE, SessionInfo.UserId));
        }
        if (!SessionInfo.IsSupervisor)
        {
            sb.Append(SessionInfo.DataFilter);
        }
        sb.Append(string.Format(" and a.DocStatus in ('{0}','{1}','{2}','{3}','{4}')", Consts.DOCUMENT_CLOSED, Consts.DOCUMENT_DRAFT, Consts.DOCUMENT_POSTED, Consts.DOCUMENT_POST_PENDING_SUBMIT, Consts.DOCUMENT_POST_PENDING_FA));
        // 'PPPS', 'PPFA'
        //sb.Append(" and a.DocStatus in ('CP','OP','UA','CA')");
        sb.Append(" order by b.uploadtime desc");
        PCCore.Common.HRLog.RecordLog(sb.ToString());
        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this;
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

    private void InitDropDownList()
    {
        //PCDb.InitDropDownList(this.ddlUser, "sc_user", "ID", "loginName", Consts.DropDownOptionAll, null);
        PCDb.InitDropDownList(this.ddlUser, "sc_user", "ID", "FullName", Consts.DropDownOptionAll, null);
        //PCDb.InitDropDownList(this.ddlProject, "v_project", "PrjCode", "U_PrjFName", Consts.DropDownOptionAll, null, string.Format("prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','")));
        PCCore.Database.ValidationList.AllowQueryDropDownProjectList(ddlProject,Consts.DropDownOptionAll);

        PCDb.InitDropDownList2(this.ddlStatus, "[@DocStatus]", "U_Status", "U_DisplayName", "U_Status", Consts.DropDownOptionAll, null);
    }
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                if (!string.IsNullOrEmpty(e.Row.Cells[0].Text.Trim()) && e.Row.Cells[0].Text.Trim() != "&nbsp;")
                {
                    e.Row.Cells[2].Text = string.Format("<a href='javascript:DownloadFile({0})'>{1}</a>", e.Row.Cells[0].Text, e.Row.Cells[2].Text);
                }
                String Status = e.Row.Cells[14].Text;
                //e.Row.Cells[12].Text = "<a href='javascript:PrintDocumentReport(" + "\"" + e.Row.Cells[13].Text + "\"" + "," + "\"" + Resources.Labels.BackEnd + "\")'>" + Resources.Labels.Report + "</a>";
                //e.Row.Cells[12].Text = "<a href='javascript:PrintDocumentReport(" + "\"" + e.Row.Cells[1].Text + "\"" + ",\"" + e.Row.Cells[14].Text + "\"" + "," + "\"" + Status + "\")'>" + Resources.Labels.Report + "</a>";
                e.Row.Cells[13].Text = "<a href='javascript:PrintDocumentReport(" + "\"" + e.Row.Cells[1].Text + "\"" + ",\"" + e.Row.Cells[15].Text + "\"" + "," + "\"" + Status + "\")'>" + Resources.Labels.View + "</a>";

                if ((SessionInfo.CurrentFunctionID == "2103") || (SessionInfo.CurrentFunctionID == "2104") || (SessionInfo.CurrentFunctionID == "1129") || (SessionInfo.CurrentFunctionID == "1130"))
                {
                    e.Row.Cells[13].Text += "&nbsp;|&nbsp;";
                    e.Row.Cells[13].Text += "<a href='javascript:ExportDocumentReport(" + "\"" + e.Row.Cells[1].Text + "\"" + ",\"" + e.Row.Cells[15].Text + "\"" + "," + "\"" + Status + "\")'>" + Resources.Labels.PDF + "</a>";
                }
                //if (Status == "CA")
                //{
                //    //e.Row.Cells[12].Text = string.Format("<a href='javascript:ShowWebReport({0})'>{1}</a>", e.Row.Cells[13].Text, Resources.Labels.Report, Resources.Labels.BackEnd,SessionInfo.CurrentLanguage);
                //    e.Row.Cells[12].Text = "<a href='javascript:ShowReport(" + "\"" + e.Row.Cells[13].Text + "\"" + "," + "\"" + Resources.Labels.BackEnd + "\")'>" + Resources.Labels.Report + "</a>";
                //}
                //else
                //{
                //    //e.Row.Cells[12].Text = string.Format("<a href='javascript:ShowWebReport({0})'>{1}</a>", e.Row.Cells[0].Text, Resources.Labels.Report, Resources.Labels.FrontEnd,SessionInfo.CurrentLanguage);
                //    e.Row.Cells[12].Text = "<a href='javascript:ShowReport(" + "\"" + e.Row.Cells[0].Text + "\"" + "," + "\"" + Resources.Labels.FrontEnd + "\")'>" + Resources.Labels.Report + "</a>";
                //}

                e.Row.Cells[3].Style.Add("padding-right", "5px");
                //e.Row.Cells[15].Text = "<a href=''>" + Resources.Labels.Attachments  + "</a>";
                e.Row.Cells[16].Text = "<a href=''>" + Resources.Labels.View + "</a>";
                break;
        }

    }
    public override void ClearForm()
    {
        foreach (Control c in this.ContentPlaceHolder.Controls)
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

}

