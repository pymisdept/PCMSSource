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
using SimpleControls.Web;

public partial class DownloadManager : BasePage
{
    const string TABLE_NAME = "DocumentProperty";
    const string FILE_TYPE = "7001";

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


    protected void Page_Load(object sender, EventArgs e)
    {
        //MasterPrjectReport.ClearError();
        

        if (Page.IsPostBack)
        {
            if (invProjCode.Text != String.Empty)
            {
                ddlProject.SelectedValue = invProjCode.Text;
                
            }
            string cmd = this.CurrentCommand;
            switch (cmd)
            {
                case Consts.ButtonDelete:
                    //DeleteRecord();
                    break;
                case Consts.Confirm:
                    //ConfirmRecord();
                    break;

            }
        }
        else
        {
            this.InitDropDownList();
            
        }

        
        //gvData.HiddenFields += "descid,description";
        // karrson: Error Status  [id],descid,[Allowdownload],cast('' as nvarchar(100)) as [TemplateName],[FileData],[FileType],[CreateDate] ,[CreateUser],[DocStatus],[Project],[FullName]
        //gvData.HiddenFields += "id,descid,Allowdownload, FileType,CreateUser,DocStatus,FunctionCode,[IP ADDRESS]";
        gvData.HiddenFields += "IPADDRESS,IPADDR,id,descid,Allowdownload, FileType,CreateUser,FunctionCode";

        //karrson: Change display field
        //gvData.HeaderDescriptions = "ID,," + Resources.Labels.FileName + "," + Resources.Labels.SizeKB + "," + Resources.Labels.Description + "," + Resources.Labels.UploadBy + "," + Resources.Labels.UploadTime + "," + Resources.Labels.Project ;
        //gvData.HeaderDescriptions = "ID,,," + Resources.Labels.TemplateName + "," + Resources.Labels.FileName + ",,"  + Resources.Labels.Date + "," + Resources.Labels.Time  + ",,," + Resources.Labels.Project + "," + Resources.Labels.UserName + ",," + Resources.Labels.IpAddress ;
        gvData.HeaderDescriptions = ",,," + Resources.Labels.DocumentName + "," + Resources.Labels.TemplateName + "," + Resources.Labels.Status + ",," + Resources.Labels.RequestTime + ",," + Resources.Labels.Project + "," + Resources.Labels.UserName + ",," + Resources.Labels.IpAddress;
        if (this.CurrentCommand != Consts.ButtonExport)
        {
            if (invProjCode.Text == String.Empty)
                SetDataSource();
        }
        invProjCode.Text = String.Empty;

    }//end of Page_Load

    protected void SetDataSource()
    {
        string where = "";
        //if (!String.IsNullOrEmpty(txtFileName.Text.Trim()))
        //{
        //    where += string.Format(" and b.filename like '%{0}%'", txtFileName.Text.Trim());
        //}

        if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()))
        {
            where += string.Format(" and CreateDate >='{0}'", txtStartDate.Text.Trim());
        }
        if (!string.IsNullOrEmpty(txtEndDate.Text.Trim()))
        {
            where += string.Format(" and CreateDate <='{0} 23:59:59'", txtEndDate.Text.Trim());
        }
        //if (!SessionInfo.IsSupervisor)
        //    where += string.Format(" and CreateUser = {0}", SessionInfo.UserId);
            
        
        
        //if (Convert.ToDecimal(ddlUser.SelectedValue) > 0)
        //{
        //    where += string.Format(" and b.uploadbyid={0}", ddlUser.SelectedValue);
        //}

        if (ddlProject.SelectedValue != "-1")
        {
            where += string.Format(" and Project='{0}'", ddlProject.SelectedValue);
        }
        if (ddlTemplate.SelectedValue != "-1")
        {
            where += string.Format(" and FileType = '{0}'", ddlTemplate.SelectedValue);
        }
        // karrson: display field 
        if (!ckbDownloaded.Checked)
            where += string.Format(" and DocStatus = '{0}'", Consts.STATUS_OPEN);
        //StringBuilder sb = ne StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,a.Docnum,b.filesize,a.description,c.loginname,convert(varchar(20),b.uploadtime,120) as uploadtime,a.projectCode, d.StatusName, a.IsError, cast('' as nvarchar(10)) as [Checklist], a.DocStatus from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join Status d on (d.StatusCode = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
       // StringBuilder sb = new StringBuilder(string.Format("select [id],[id] as descid,[Allowdownload],cast('' as nvarchar(100)) as [TemplateName],cast('' as nvarchar(100)) as [File],[FileType],[CreateDate] , Cast(datepart(hh,[CreateDate]) as nvarchar(2)) + ':' + cast(datepart(mi,[CreateDate]) as nvarchar(2)) + ':' + cast(datepart(ss,[CreateDate]) as nvarchar(2)) as [CreateTime],[CreateUser],[DocStatus],[Project],[FullName],[FunctionCode],isNull(IPAddr,'') AS [IP ADDRESS] from {0} a where 1=1 {1}", "View_OSDownloadRequest", where));
        String pSql = "select [id],[id] as descid,[Allowdownload],cast('' as nvarchar(100)) as [TemplateName],cast('' as nvarchar(30)) as [File]";
               pSql = pSql + ", case when [DocStatus] = 'C' then 'Downloaded' else (case when allowdownload = 'N' then 'Processing' else 'NEW' end) end ";
               pSql = pSql + ",[FileType],convert(varchar(20),[CreateDate],120),[CreateUser],[Project],[FullName],[FunctionCode],isNull(IPAddr,'') AS [IPADDRESS]";
        

        //StringBuilder sb = new StringBuilder(string.Format(pSql + " from {0} a where 1=1 {1}", "View_OSDownloadRequest", where));
        // By Project
               StringBuilder sb;
               if (SessionInfo.IsSupervisor)
               {
                   sb = new StringBuilder(string.Format(pSql + " from {0} a where 1=1 {1}", "View_OSDownloadRequest", where));
               }
               else
               {

                   where = where + " AND p.userid = " + SessionInfo.UserId;
                   
                   //sb = new StringBuilder(string.Format(pSql + " from {0} a inner join CPS_View_QueryPermission p on (p.fid = {2} and p.userid = {3} and ((isNull(a.Project,'') = '' and p.userid = a.CreateUser ) or (isNull(a.Project,'') <> '' and p.projcode = a.Project))) where 1=1 {1}", "View_OSDownloadRequest", where, SessionInfo.CurrentFunctionID, SessionInfo.UserId));
                   sb = new StringBuilder(string.Format(pSql + " from {0} a inner join CPS_View_QueryPermission p on (p.fid = a.FileType and p.userid = a.CreateUser and a.Project = p.ProjCode)  where 1=1 {1}", "View_OSDownloadRequest", where ));
               }
        
        //sb.Append(SessionInfo.DataFilter);
        sb.Append(" order by CreateDate desc,Project asc");
        PCCore.Common.HRLog.RecordLog(sb.ToString());

        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this.Master;
    }

    void CheckDeletePrerequisite(PCTable table, Hashtable row)
    {
        int count = (int) PCDb.Db.ExecScalar(string.Format("select count(*) from CM_SessionFiles where recordid in (select recordid from CM_SessionFiles where id={0})", row["ID"].ToString()));
        if (count > 1) return;
        //string id = row[Consts.FieldID].ToString();
        string id = PCDb.Db.ExecScalar(string.Format("select recordid from CM_SessionFiles where id={0}", row["ID"].ToString())).ToString();

        string where = String.Format("id={0}", id);
        PCTable desc = new PCTable(TABLE_NAME, this.SecurityInfo);
        desc.UseTransaction(table.InternalTransaction);
        desc.Delete(where);
    }
   
    void DeleteRecord()
    {
        this.Master.DeleteRecord("CM_SessionFiles", gvData, CheckDeletePrerequisite);        
    }
   
    private void InitDropDownList()
    {
        
        PCDb.InitDropDownList(this.ddlUser, "sc_user", "ID", "loginName", Consts.DropDownOptionAll, null);
        PCDb.InitDropDownList(this.ddlTemplate, "View_TemplateList", "ID", "CODE", Consts.DropDownOptionAll, null, true);
        //PCDb.InitDropDownList(this.ddlProject, "v_project", "PrjCode", "U_PrjFName", Consts.DropDownOptionNone, null, string.Format("prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','")));
        PCCore.Database.ValidationList.AllowQueryDropDownProjectList(ddlProject,Consts.DropDownOptionAll);

    }

    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                //e.Row.Cells[3].Text = GetGlobalResourceObject(Consts.ResourcesLabels, e.Row.Cells[12].Text.Trim()).ToString();
                try
                {
                    e.Row.Cells[3].Text = GetGlobalResourceObject(Consts.ResourcesLabels, e.Row.Cells[11].Text.Trim()).ToString();
                }
                catch (Exception ex)
                { }
                e.Row.Cells[3].Text = e.Row.Cells[3].Text.Replace("Download and Upload", "");
                if (e.Row.Cells[2].Text.Trim() == "Y")
                    //e.Row.Cells[4].Text = "<a href='javascript:DownloadFile(" + "\"" + e.Row.Cells[0].Text + "\"" + ",\"" + e.Row.Cells[10].Text + "\"" + "," + "\"" + e.Row.Cells[12].Text.ToString() + "\")'>" + Resources.Labels.Available + "</a>";
                    e.Row.Cells[4].Text = "<a href='javascript:DownloadFile(" + "\"" + e.Row.Cells[0].Text + "\"" + ",\"" + e.Row.Cells[9].Text + "\"" + "," + "\"" + e.Row.Cells[11].Text.ToString() + "\")'>" + Resources.Labels.Available + "</a>";
                
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
    /// <summary>
    /// btnConfirm_Click: Confirm Click 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void btnConfirm_Click(object sender, EventArgs e){}
        
        ////if (gvData.SelectedIndex >= 0) 
        //// {
        //try
        //{
        //    Hashtable _htConfirm = new Hashtable();
        //    _htConfirm.Add(Consts.FieldID, Convert.ToInt32(gvData.SelectedRow.Cells[0].Text.Trim()));
        //    _htConfirm.Add("DocStatus", "CP");
        //    this.Table.Update(_htConfirm);
        //}
        //catch (Exception ex)
        //{
        //    throw ex;
        //}
            

        
        //}
    
}
