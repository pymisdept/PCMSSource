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
using System.Text;
using PCCore;
using SimpleControls.Web;

public partial class ScAnnouncement : BasePage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ClearError();
        if (SessionInfo.IsSupervisor)
        {
            
        }
        if (Page.IsPostBack)
        {
            string cmd = this.CurrentCommand;
            switch (cmd)
            {
                case Consts.ButtonDelete:
                    DeleteRecord();
                    break;
            }
        }
        else
        {
        }

        gvData.HeaderDescriptions = ",ID," + Resources.Labels.AnnouncementTitle + "," + Resources.Labels.Project + "," + Resources.Labels.UserName +  "," + Resources.Labels.EffectiveDate + "," + Resources.Labels.ExpiryDate;
        gvData.HiddenFields += "priorityid";
        if (this.CurrentCommand!=Consts.ButtonExport) SetDataSource();

    }//end of Page_Load
    protected void gvData_RowCreated(object sender, GridViewRowEventArgs e)
    {
        
    }

    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        CheckBox _chk;
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                _chk = new CheckBox();
                _chk.ID = "chkFlag_" + e.Row.ClientID;
                PCCore.Common.HRLog.RecordLog("CheckBox ID: " + _chk.ID);
                PCCore.Common.HRLog.RecordLog("CheckBox ClientID: " + _chk.ClientID);
                e.Row.Cells[0].Controls.Add(_chk);
                break;
            case DataControlRowType.Header:
                //_chk = new CheckBox();
                //_chk.ID = "chkAllFlag";
                //_chk.Attributes["onclick"] = "javascript:checkAll();";
                //e.Row.Cells[0].Controls.Add(_chk);

            break;


        }

    }

    protected void SetDataSource()
    {
        //StringBuilder sb = new StringBuilder("select ID,title,convert(varchar(10),effectivedate,120) as effectivedate,convert(varchar(10),expirydate,120) as expirydate from SC_ANNOUNCEMENT where 1=1");
        StringBuilder sb = new StringBuilder("select '' as flag,s.ID,s.title,s.PrjCode,u.FullName,convert(varchar(10),s.effectivedate,120) as effectivedate,convert(varchar(10),s.expirydate,120) as expirydate from SC_ANNOUNCEMENT s inner join sc_user u on u.id = s.createdid where 1=1");
        
        if (!String.IsNullOrEmpty(txtSearchBox.Text))
        {
            sb.AppendFormat(" and title like '%{0}%'", txtSearchBox.DBText.Trim());
        }
        // User
        if (SessionInfo.IsSupervisor == false)
        {
            sb.AppendFormat(string.Format(" and createdid = {0} ", SessionInfo.UserId));
        }
        if (chkDeleted.Checked == false)
        {
            sb.AppendFormat(" and isNull(s.cancel,0) = 0 ");

        }
        else
        {
            sb.AppendFormat(" and isNull(s.cancel,0) = 1 ");
        }
        sb.Append(" order by effectivedate,expirydate");
        PCCore.Common.HRLog.RecordLog(sb.ToString());
        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this.Master;

    }

    void CheckDeletePrerequisite(PCTable table, Hashtable row)
    {        

    }

    void DeleteRecord()
    {
        this.Master.DeleteRecord("SC_ANNOUNCEMENT", gvData, CheckDeletePrerequisite);
    }

    protected void Regscript()
    {
        
    }
    protected void btnDelete1_Click(object sender, EventArgs e)
    {
        MarkAsDelete();
        Response.Redirect(Request.Path);
    }
    protected void btnActive1_Click(object sender, EventArgs e)
    {
        MarkAsActive();
        Response.Redirect(Request.Path);
    }
    protected void btnDelete2_Click(object sender, EventArgs e)
    {
        MarkAsDelete();
        Response.Redirect(Request.Path);
    }
    protected void btnActive2_Click(object sender, EventArgs e)
    {
        MarkAsActive();
        Response.Redirect(Request.Path);
    }

    private void MarkAsDelete()
    {
        string RowID = string.Empty;

        for (int i = 0; i < gvData.Rows.Count; i++)
        {
            if (gvData.Rows[i].RowType == DataControlRowType.DataRow)
            {
                RowID = gvData.Rows[i].Cells[1].Text;
                if (((CheckBox)gvData.Rows[i].Cells[0].Controls[0]).Checked)
                {
                    // Update Cancel to 'Y'
                    PCCore.Database.SC_Announcements.DeleteAnnouncement(RowID);
                    
                }
            }
        }
    }
    private void MarkAsActive()
    {
        string RowID = string.Empty;

        for (int i = 0; i < gvData.Rows.Count; i++)
        {
            if (gvData.Rows[i].RowType == DataControlRowType.DataRow)
            {
                RowID = gvData.Rows[i].Cells[1].Text;
                if (((CheckBox)gvData.Rows[i].Cells[0].Controls[0]).Checked)
                {
                    // Update Cancel to 'N'
                    PCCore.Database.SC_Announcements.ActiveAnnouncement(RowID);
                }
            }
        }
    }
}
