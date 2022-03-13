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

public partial class QSI12C : BasePage
{
    const string TABLE_NAME = "DocumentProperty";
    const string FILE_TYPE = "1012";
    string id = String.Empty;

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ClearError();

        if (Page.IsPostBack)
        {
            string cmd = this.CurrentCommand;
            switch (cmd)
            {
                case Consts.ButtonDelete:
                    
                    break;
            }
        }
        else
        {
            id = Request.QueryString["id"] as String;
            
        }

        gvData.HiddenFields += "filename,descid,description,filename,filesize,DocStatus,DocEntry";
        //gvData.HeaderDescriptions = "ID,,," + Resources.Labels.FileName + "," + Resources.Labels.SizeKB + "," + Resources.Labels.Description + "," + Resources.Labels.DocumentType + "," + Resources.Labels.DocumentNo + "," + Resources.Labels.UploadBy + "," + Resources.Labels.UploadTime + "," + Resources.Labels.Project + "," + Resources.Labels.Status + "," + Resources.Labels.Report + "," + Resources.Labels.Docentry;
        gvData.HeaderDescriptions = "ID," + Resources.Labels.SheetName + ",,," + Resources.Labels.FileName + "," + Resources.Labels.SizeKB + "," + Resources.Labels.Description + "," + Resources.Labels.DocumentType + "," + Resources.Labels.DocumentNo + "," + Resources.Labels.UploadBy + "," + Resources.Labels.UploadTime + "," + Resources.Labels.Project + "," + Resources.Labels.Status + "," + Resources.Labels.Report + "," + Resources.Labels.Docentry;

        if (this.CurrentCommand != Consts.ButtonExport)
            SetDataSource();

    }//end of Page_Load

    protected void SetDataSource()
    {
        string where = "";
        StringBuilder sb = new StringBuilder(string.Format("select id ,SheetName,descid,filename,filesize,description,DocNum,case when DocStatus = 'UA' then 'Draft' else 'Open' end as [Document Type], DocNum,loginname,convert(varchar(20),uploadtime,120) as uploadtime,projectCode,StatusName, cast('' as nvarchar(50)) as [Report], DocStatus as DocStatus , isnull(cast(DocEntry as nvarchar(50)),'-1') as DocEntry from {0} where type={1} {2}", "v_SCPayCertCheckList", FILE_TYPE, where));
        //StringBuilder sb = new StringBuilder(string.Format("select b.id ,a.id as descid, b.filename,b.filesize,a.description,a.DocNum,case when DocStatus = 'UA' then 'Draft' else 'Open' end as [Document Type], a.DocNum,c.loginname,convert(varchar(20),b.uploadtime,120) as uploadtime,a.projectCode,d.StatusName, cast('' as nvarchar(50)) as [Report], a.DocStatus as DocStatus , isnull(cast(a.DocEntry as nvarchar(50)),'-1') as DocEntry from {0} a left join CM_SessionFiles b on b.recordid=a.id left join sc_user c on c.id=b.uploadbyid left join Status d on d.StatusCode = a.DocStatus where type={1} {2}", TABLE_NAME, FILE_TYPE, where));
        sb.Append(SessionInfo.DataFilter);
        sb.Append(" and descid = " + id);
        sb.Append(" and DocStatus in ('CP','OP','UA','CA')");
        sb.Append(" order by uploadtime desc");

        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this.Master;
    }

    void CheckDeletePrerequisite(PCTable table, Hashtable row)
    {
       
    }

    void DeleteRecord()
    {
        this.Master.DeleteRecord("CM_SessionFiles", gvData, CheckDeletePrerequisite);        
    }
   
    private void InitDropDownList()
    {
        
    }

    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                if (!string.IsNullOrEmpty(e.Row.Cells[0].Text.Trim()) && e.Row.Cells[0].Text.Trim()!="&nbsp;")
                {
                    e.Row.Cells[3].Text = string.Format("<a href='javascript:DownloadFile({0})'>{1}</a>", e.Row.Cells[0].Text, e.Row.Cells[2].Text);
                }
                String Status = e.Row.Cells[13].Text;
                e.Row.Cells[12].Text = "<a href='javascript:PrintDocumentReport(" + "\"" + e.Row.Cells[0].Text + "\"" + ",\"" + e.Row.Cells[14].Text + "\"" + "," + "\"" + Status + "\")'>" + Resources.Labels.Report + "</a>";
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
                
                
                
                break;            
        }

    }
}
