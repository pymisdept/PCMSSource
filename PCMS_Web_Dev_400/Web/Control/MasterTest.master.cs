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

public partial class MasterTest : MasterBasePage, IErrorHandler
{
    
    const string TABLE_NAME = "DocumentProperty";
    string FILE_TYPE = string.Empty;
    string Current_Command = "";

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
        }
        else
        {
            this.InitDropDownList();

        }

        gvData.HiddenFields += "id,descid,description,isError,filesize,DocStatus";
        gvData.HeaderDescriptions = ",," + Resources.Labels.FileName + "," + Resources.Labels.DocumentNo + "," + Resources.Labels.SizeKB + "," + Resources.Labels.Description + "," + Resources.Labels.UploadBy + "," + Resources.Labels.UploadTime + "," + Resources.Labels.Project + "," + Resources.Labels.Status + "," + Resources.Labels.CheckList;

        if (this.Current_Command != Consts.ButtonExport)
            SetDataSource();

        
    }
    protected void SetDataSource()
    {
        string where = "";
        if (!String.IsNullOrEmpty(txtFileName.Text.Trim()))
        {
            where += string.Format(" and b.filename like '%{0}%'", txtFileName.Text.Trim());
        }

        if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()))
        {
            where += string.Format(" and b.uploadtime >='{0}'", txtStartDate.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txtEndDate.Text.Trim()))
        {
            where += string.Format(" and b.uploadtime <='{0} 23:59:59'", txtEndDate.Text.Trim());
        }

        //if (Convert.ToDecimal(ddlUser.SelectedValue) > 0)
        //{
        //    where += string.Format(" and b.uploadbyid={0}", ddlUser.SelectedValue);
        //}

        if (ddlProject.SelectedValue != "-1")
        {
            where += string.Format(" and a.projectCode='{0}'", ddlProject.SelectedValue);
        }
        // karrson: display field 
        //StringBuilder sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,b.filesize,a.description,c.loginname,convert(varchar(20),b.uploadtime,120) as uploadtime,a.projectCode from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
        StringBuilder sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,a.Docnum,b.filesize,a.description,c.loginname,convert(varchar(20),b.uploadtime,120) as uploadtime,a.projectCode, d.DisplayName, a.IsError, cast('' as nvarchar(10)) as [Checklist], a.DocStatus from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join CPS_View_DocStatus d on (d.Code = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
        sb.Append(SessionInfo.DataFilter);
        sb.Append(" order by b.uploadtime desc");

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
        PCDb.InitDropDownList(this.ddlUser, "sc_user", "ID", "loginName", Consts.DropDownOptionAll, null);

        //PCDb.InitDropDownList(this.ddlProject, "v_project", "PrjCode", "U_PrjFName", Consts.DropDownOptionNone, null, string.Format("prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','")));
        PCCore.Database.ValidationList.ProjectList(ddlProject);

    }

    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {


        switch (e.Row.RowType)
        {


            case DataControlRowType.DataRow:


                if (!string.IsNullOrEmpty(e.Row.Cells[1].Text.Trim()) && e.Row.Cells[1].Text.Trim() != "&nbsp;")
                {
                    e.Row.Cells[2].Text = string.Format("<a href='javascript:DownloadFile({0})'>{1}</a>", e.Row.Cells[0].Text, e.Row.Cells[2].Text);
                }
                // karrson: add error message link
                // 2009-06-30 karrson: change all record contain checklist
                if (e.Row.Cells[12].Text.Trim() != Consts.DOCUMENT_DRAFT_PENDING)
                {
                    e.Row.Cells[11].Text = string.Format("<a href='javascript:PrintCheckList({0})'>{1}</a>", e.Row.Cells[1].Text, Resources.Labels.View);
                }
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

    protected void btnTest_Click(object sender, EventArgs e)
    {
        PCCore.Common.HRLog.RecordLog(txtStartDate.Text);
        PCCore.Common.HRLog.RecordLog(gvData.SelectedIndex);
        System.Threading.Thread.Sleep(3000);
        Response.Redirect("QSI27.aspx");
    }
    
}

