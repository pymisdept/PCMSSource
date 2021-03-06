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

public partial class AC03 : BasePage
{
    const string TABLE_NAME = "DocumentProperty";
    const string FILE_TYPE = "3103";

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
        Master.ClearError();
        
        if (Page.IsPostBack)
        {
            string cmd = this.CurrentCommand;
            switch (cmd)
            {
                //case Consts.ButtonDelete:
                //    DeleteRecord();
                //    break;
                //case Consts.Confirm:
                //    ConfirmRecord();
                //    break;

            }
        }
        else
        {
            this.InitDropDownList();
            
        }
        

        
        //gvData.HiddenFields += "descid,description";
        // karrson: Error Status 
        //gvData.HiddenFields += "id,descid,description,isError";
        //karrson: Change display field
        //gvData.HeaderDescriptions = "ID,," + Resources.Labels.FileName + "," + Resources.Labels.SizeKB + "," + Resources.Labels.Description + "," + Resources.Labels.UploadBy + "," + Resources.Labels.UploadTime + "," + Resources.Labels.Project ;
        //gvData.HeaderDescriptions = ",," + Resources.Labels.FileName + "," + Resources.Labels.SizeKB + "," + Resources.Labels.Description + "," + Resources.Labels.UploadBy + "," + Resources.Labels.UploadTime + "," + Resources.Labels.Project + "," + Resources.Labels.Status + "," + Resources.Labels.ErrorMessage ;

        //if (this.CurrentCommand != Consts.ButtonExport)
        //    SetDataSource();

    }//end of Page_Load

    //protected void SetDataSource()
    //{
    //    string where = "";
    //    if (!String.IsNullOrEmpty(txtFileName.Text.Trim()))
    //    {
    //        where += string.Format(" and b.filename like '%{0}%'", txtFileName.Text.Trim());
    //    }

    //    if (!string.IsNullOrEmpty(txtStartDate.Text.Trim()))
    //    {
    //        where += string.Format(" and b.uploadtime >='{0}'", txtStartDate.Text.Trim());
    //    }
    //    if (!string.IsNullOrEmpty(txtEndDate.Text.Trim()))
    //    {
    //        where += string.Format(" and b.uploadtime <='{0} 23:59:59'", txtEndDate.Text.Trim());
    //    }

    //    if (Convert.ToDecimal(ddlUser.SelectedValue) > 0)
    //    {
    //        where += string.Format(" and b.uploadbyid={0}", ddlUser.SelectedValue);
    //    }

    //    if (ddlProject.SelectedValue != "-1")
    //    {
    //        where += string.Format(" and a.projectCode='{0}'", ddlProject.SelectedValue);
    //    }
    //    // karrson: display field 
    //    //StringBuilder sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,b.filesize,a.description,c.loginname,convert(varchar(20),b.uploadtime,120) as uploadtime,a.projectCode from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
    //    StringBuilder sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,b.filesize,a.description,c.loginname,convert(varchar(20),b.uploadtime,120) as uploadtime,a.projectCode, d.StatusName, a.IsError, cast('' as nvarchar(10)) as [Error Message] from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join Status d on (d.StatusCode = a.DocStatus) where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
    //    sb.Append(SessionInfo.DataFilter);
    //    sb.Append(" order by b.uploadtime desc");

    //    dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
    //    dsGridView.SelectCommand = sb.ToString();
    //    dsGridView.ErrorHandler = this.Master;
    //}

    //void CheckDeletePrerequisite(PCTable table, Hashtable row)
    //{
    //    int count = (int) PCDb.Db.ExecScalar(string.Format("select count(*) from CM_SessionFiles where recordid in (select recordid from CM_SessionFiles where id={0})", row["ID"].ToString()));
    //    if (count > 1) return;
    //    //string id = row[Consts.FieldID].ToString();
    //    string id = PCDb.Db.ExecScalar(string.Format("select recordid from CM_SessionFiles where id={0}", row["ID"].ToString())).ToString();

    //    string where = String.Format("id={0}", id);
    //    PCTable desc = new PCTable(TABLE_NAME, this.SecurityInfo);
    //    desc.UseTransaction(table.InternalTransaction);
    //    desc.Delete(where);
    //}
    //void ConfirmRecord()
    //{
    //    //String s = gvData.SelectedValue.ToString();
    //    //Hashtable _ht = new Hashtable();

    //    //_ht.Add(Consts.FieldID, gvData.SelectedRowValue.ToString());
    //    //_ht.Add("DocStatus", "CP");
    //    try
    //    {
    //        this.Table.Db.ExecQuery("UPDATE DOCUMENTPROPERTY SET DOCSTATUS = 'CP' WHERE ID = " + gvData.SelectedRowValue.ToString());
    //    }
    //    catch (Exception ex)
    //    {
    //        throw ex;
    //    }
    //    //this.Table.BeginTransaction();   
    //    //this.Table.Update(_ht);
    //    //this.Table.CommitTransaction();
    //}
    //void DeleteRecord()
    //{
    //    this.Master.DeleteRecord("CM_SessionFiles", gvData, CheckDeletePrerequisite);        
    //}
   
    private void InitDropDownList()
    {
        //PCDb.InitDropDownList(this.ddlUser, "sc_user", "ID", "loginName", Consts.DropDownOptionAll, null);
        //PCDb.InitDropDownList(this.ddlProject, "v_project", "PrjCode", "U_PrjFName", Consts.DropDownOptionAll, null, string.Format("prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','")));
        //PCCore.Database.ValidationList.ReportProjectList(ddlProject, Consts.DropDownOptionNone);
        //PCCore.Database.ValidationList.ReportProjectList(ddltProject, Consts.DropDownOptionNone);
    }

    //protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    //{

        
    //    switch (e.Row.RowType)
    //    {
                

    //        case DataControlRowType.DataRow:
                
                
    //            if (!string.IsNullOrEmpty(e.Row.Cells[1].Text.Trim()) && e.Row.Cells[1].Text.Trim()!="&nbsp;")
    //            {
    //                e.Row.Cells[2].Text = string.Format("<a href='javascript:DownloadFile({0})'>{1}</a>", e.Row.Cells[0].Text, e.Row.Cells[2].Text);
    //            }
    //            // karrson: add error message link
    //            if (e.Row.Cells[9].Text.Trim() == "1")
    //            {
    //                e.Row.Cells[10].Text = string.Format("<a href='javascript:ViewErrorMessage({0})'>{1}</a>", e.Row.Cells[1].Text, Resources.Labels.View);
    //            }
    //            e.Row.Cells[3].Style.Add("padding-right", "5px");
    //            break;            
    //    }
    //}


    //protected void gvData_Sorted(object sender, EventArgs e)
    //   {
    //       //String s = gvData.SelectedRow.Cells[0].Text;
    //   }


    //protected void gvData_RowCommand(object sender, EventArgs e)
    //{
    //    //String s = "";
        
    //}

    //protected void gvData_SelectedIndexChanging(object sender, EventArgs e)
    //{
    //    //String s = gvData.SelectedRow.Cells[0].Text;
    //}
    
    //protected void gvData_SelectedChanged(object sender, EventArgs e)
    //{
    //    String s = gvData.SelectedRow.Cells[0].Text;
    //}
    /// <summary>
    /// btnConfirm_Click: Confirm Click 
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    //protected void btnConfirm_Click(object sender, EventArgs e){}
        
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
    /// <summary>
    /// SearchButton_Click: Choose Project Code and Start to Create Project Report
    ///                     Redirect to First Content of Project Report
    /// </summary>
    /// <param name="sender"></param>
    /// <param name="e"></param>
    protected void SearchButton_Click(object sender, EventArgs e)
    {
        
    }
}
