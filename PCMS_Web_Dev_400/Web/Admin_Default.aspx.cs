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
using System.Text;
using SimpleControls;
using SimpleControls.SimpleDatabase;
using SimpleControls.Web;


public partial class Admin_Default : BasePage
{
    string appUrl;
    string themeUrl;
    string Current_Command = "";
    protected override void OnInit(EventArgs e)
    {
        string cssLink = null;
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
                cssLink = SimpleWebUtils.GetCssLink(Config.AppBaseUrl + "/control/gridview-security.css");
                break;
        }
        LiteralControl css = new LiteralControl(cssLink);
        css.ID = "cssID";
        Page.Header.Controls.Add(css);
        
        base.LoginRequired = true;
        // base.ShowWebMenu = false;
        
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
       
        Note.Visible = false;
        
        PCCore.Common.HRLog.RecordLog("Page_Load");
        
        //PCCore.PCMS.Announcement.GetHtmlTable(tblAnnouncement);
        appUrl = Config.AppBaseUrl;
        themeUrl = Config.GetThemeBaseUrl(Page.Theme);

        initImage();

        if (SessionInfo.CurrentProject != "")
            setProjectDataSource(SessionInfo.CurrentProjectName);
        else
            setProjectDataSource("");

        gvPrjSearch.HeaderDescriptions = "," + Resources.Labels.Project + "," + Resources.Labels.Name + ",";
        Current_Command = Request.Form[Consts.HiddenInputCommand];
        CheckCommand();
        //flag as flag,id,prjcode,title,sender,msgtype
        gvData.HeaderDescriptions = ",ID," + Resources.Labels.Date + "," + Resources.Labels.Project + "," + Resources.Labels.AnnouncementTitle + "," + Resources.Labels.Sender + ",";
        gvData.HiddenFields += "id,msgtype,hasread";
        SetDataSource();
    }

    void CheckCommand()
    {
        switch (Current_Command)
        {
            case Consts.Submit:
                ProjectSelected();
                break;
        }
    }

    void initImage()
    {
        btnProjSearch.ImageUrl = themeUrl + "/images/search.gif";
    }
    void SearchProject()
    {

    }
    protected void btnSubmit_Click(object sender, EventArgs e)
    {
        if (!chkAllProj.Checked)
        {
            if (gvPrjSearch.SelectedRowValue == null || gvPrjSearch.SelectedRowValue == "")
            {
                Note.Visible = true;
                Note.ShowMessage(Resources.Messages.InValidProjectCode);
            }
            else
            {
                ProjectSelected();
            }
        }
        else
        {
            ProjectSelected();
        }
    }
    protected void btnProjSearch_Click(object sender, ImageClickEventArgs e)
    {
        setProjectDataSource(txtSearchProject.Text);
    }
    void ProjectSelected()
    {
        
        // All Project Funtion
        if (!chkAllProj.Checked)
        {
            if (gvPrjSearch.SelectedRowValue.ToString() != "")
            {
                SessionInfo.CurrentProject = gvPrjSearch.SelectedRowValue.ToString();
                PCCore.Common.HRLog.RecordLog(SessionInfo.CurrentProject);
                //SessionInfo.CurrentProjectName = gvPrjSearch.SelectedRow.Ce[2].Text;
                PCCore.Database.Project _prj = new PCCore.Database.Project(gvPrjSearch.SelectedRowValue.ToString());
                SessionInfo.CurrentProjectName = _prj.ProjectName();
            }
        }
        else
        {
            PCCore.Common.HRLog.RecordLog(Consts.ProjectAll);
            SessionInfo.CurrentProject = Consts.ProjectAll;
            SessionInfo.CurrentProjectName = "";
        }
        Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
    }
    void setProjectDataSource(string projfilter)
    {
        string filter = "";
        string sql = "";
        gvPrjSearch.HiddenFields += "descid,docentry";
        if (projfilter != "")
        {
            filter += string.Format("{0} like '%{1}%'",Consts.FieldProjectName,projfilter);
        } else {
            filter += " 1 = 1 ";
        }
        filter += string.Format(" AND PrjCode in ('{0}')", SessionInfo.ProjectID.Replace(",", "','"));
        sql = string.Format("select docentry,{0},{1},'' as descid from CPS_View_ProjectList where 1 = 1  AND {2}", Consts.FieldProject, Consts.FieldProjectName, filter);
        PCCore.Common.HRLog.RecordLog(sql);

        dsPrjSearch.SelectCommandType = SqlDataSourceCommandType.Text;
        dsPrjSearch.SelectCommand = sql;
        

    }
    protected void chkAllProj_CheckedChanged(object sender, EventArgs e)
    {
        // Disable Project Search Grid

        txtSearchProject.Enabled = (!chkAllProj.Checked);
        btnProjSearch.Enabled = (!chkAllProj.Checked);
        gvPrjSearch.Enabled = (!chkAllProj.Checked);
        trSearchProj.Visible = (!chkAllProj.Checked);
    }
    protected void SetDataSource()
    {
        //StringBuilder sb = new StringBuilder("select ID,title,convert(varchar(10),effectivedate,120) as effectivedate,convert(varchar(10),expirydate,120) as expirydate from SC_ANNOUNCEMENT where 1=1");
        StringBuilder sb = new StringBuilder("select flag as flag,id,createdate,prjcode,title,sender,msgtype,hasread from CPS_View_UserAnnouncement where 1=1 ");
        sb.Append(string.Format(" and (userid = {0}) ", SessionInfo.UserId));
        sb.Append(" order by effectivedate,expirydate");

        PCCore.Common.HRLog.RecordLog(sb.ToString());
        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        gvData.HiddenFields += "hasread,msgtype";
        //dsGridView.ErrorHandler = this.Master;

    }
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
                e.Row.Cells[4].Text = string.Format("<a href='javascript:ViewAnnouncement({0},\"U\",{1})'>{2}</a>", e.Row.Cells[1].Text, SessionInfo.UserId, e.Row.Cells[4].Text);
                

                if (e.Row.Cells[7].Text == "0")
                    e.Row.Font.Bold = true;
                else
                    e.Row.Font.Bold = false;
                break;
            case DataControlRowType.Header:
                _chk = new CheckBox();
                _chk.ID = "chkAllFlag";
                _chk.Attributes["onclick"] = "javascript:checkAll();";
                e.Row.Cells[0].Controls.Add(_chk);

                break;


        }

    }
    private void MarkAsRead()
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
                    PCCore.Database.SC_Announcements.ReadAnnouncement(RowID,gvData.Rows[i].Cells[5].Text);
                }
            }
            
        }
        Response.Redirect(HttpContext.Current.Request.Path);
    }
    private void MarkAsUnRead()
    {
        string RowID = string.Empty;

        for (int i = 0; i < gvData.Rows.Count; i++)
        {
            if (gvData.Rows[i].RowType == DataControlRowType.DataRow)
            {
                RowID = gvData.Rows[i].Cells[1].Text;
                if (((CheckBox)gvData.Rows[i].Cells[0].Controls[0]).Checked)
                {
                    
                    PCCore.Database.SC_Announcements.UnReadAnnouncement(RowID, gvData.Rows[i].Cells[5].Text);
                }
            }
        }
        
        Response.Redirect(HttpContext.Current.Request.Path);
        
    }
    protected void btnRead1_Click(object sender, EventArgs e)
    {
        MarkAsRead();
    }

    protected void btnUnRead1_Click(object sender, EventArgs e)
    {
        MarkAsUnRead();
    }

    protected void btnRead2_Click(object sender, EventArgs e)
    {
        MarkAsRead();
    }
    protected void btnUnRead2_Click(object sender, EventArgs e)
    {
        MarkAsUnRead();
    }

}
