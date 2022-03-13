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
using SimpleControls;
using SimpleControls.Web;
using SimpleControls.SimpleDatabase;

public partial class ScAccessLog : BasePage
{
    const string TABLE_NAME = Consts.TableScAuditLog;

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ClearError();
        RegisterClientId();

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
            txtStartLogTime.Text = DateTime.Today.ToString(Consts.DateFormat);
            txtEndLogTime.Text = DateTime.Today.ToString(Consts.DateFormat);
            OnInitDropDownListRegion();
        }
        gvData.HiddenFields = "IPADDRESS,ID,TABLENAME";
        gvData.HeaderDescriptions = "ID" + "," + Resources.Labels.LoginName + "," + Resources.Labels.AccessTime + "," + Resources.Labels.Action  
            + "," + Resources.Labels.FunctionName + "," + Resources.Labels.TableName + "," + Resources.Labels.IpAddress;
        if (this.CurrentCommand != Consts.ButtonExport) SetDataSource();
        SetToolBarButtonVisable();
        AjaxPro.Utility.RegisterTypeForAjax(typeof(ScAccessLog));

    }//end of Page_Load

    protected void OnInitDropDownListRegion()
    {
        //PCDb.InitDropDownList(ddlLoginName, "v_sc_users", "ID", "LOGINNAME", Consts.DropDownOptionAll, null, String.Format(" pid in(select pid from v_ps_employee where 1=1 {0})", SessionInfo.DataFilter));
        PCDb.InitDropDownList(ddlLoginName, "v_sc_users", "ID", "LOGINNAME", Consts.DropDownOptionAll, null, null);

        ListItem li1 = new ListItem("-" + Resources.Labels.All + "-", "1");
        ListItem li2 = new ListItem(Resources.Labels.LOGIN, "Login");
        ListItem li3 = new ListItem(Resources.Labels.Logout, "Loginout");
        ListItem li4 = new ListItem(Resources.Labels.Enter, "Enter");
        ListItem li5 = new ListItem(Resources.Labels.Operation, "Operation");
        ListItem li6 = new ListItem(Resources.Labels.TimeOut, "Timeout");

        ListItem[] li = new ListItem[] { li1, li2, li3, li4, li5, li6 };
        ddlActiveType.Items.AddRange(li);
        ddlActiveType.SelectedIndex = 4;
    }

    protected void RegisterClientId()
    {
        string regID = "divID = '" + divdisplayvalue.ClientID.ToString() + "';";

        Page.ClientScript.RegisterStartupScript(Page.GetType(), "register", "<script language='javascript'>"+regID+"</script>");
    }

    protected void SetToolBarButtonVisable()
    {
        ToolBar tb = this.tbToolBar; 
        tb.ButtonSaveVisible = false;
        tb.ButtonNewVisible = false;
        tb.ButtonDeleteVisible = false;
        tb.ButtonEditVisible = false;
    }

    protected void SetDataSource()
    {
        StringBuilder sb = new StringBuilder(String.Format("select ID,LOGINNAME,convert(varchar(10),logtime,120)+' '+left(convert(varchar(10),logtime,108),5) as LOGTIME,"+
            "ACTION,FUNCNAME,TABLENAME,IPADDRESS from {0} where 1=1 and Action not like '%Insert%' and Action not like '%Update%' and Action not like '%Delete%' ", TABLE_NAME));
        Db db =PCDb.Db;
        if (ddlLoginName.SelectedIndex > 0)
        {
            sb.AppendFormat(" and loginname like '%{0}%'", ddlLoginName.SelectedItem.Text);
        }
        if (ddlActiveType.SelectedIndex > 0)
        {
            sb.AppendFormat(" and Action like  '%{0}%' ", ddlActiveType.SelectedItem.Text);
        }

        //if (!String.IsNullOrEmpty(ddlAction.SelectedValue))
        //{
        //    if (ddlAction.SelectedValue.Trim() != "-All-")
        //        sb.AppendFormat(" and Action like '%{0}%'", ddlAction.SelectedValue.Trim()); 
        //    else
        //        sb.AppendFormat(" and Action not like '%Insert%' and Action not like '%Update%' and Action not like '%Delete%'");
        //}
        if (!String.IsNullOrEmpty(txtStartLogTime.Text) && SimpleRegex.IsDateYMD(txtStartLogTime.Text))
        {
            sb.AppendFormat(" and  logtime>= '{0}'", txtStartLogTime.DBText.Trim());
        }
        if (!String.IsNullOrEmpty(txtEndLogTime.Text) && SimpleRegex.IsDateYMD(txtEndLogTime.Text))
        {
            sb.AppendFormat(" and logtime<= '{0}'", txtEndLogTime.DBText.Trim() + Consts.MaxTime);
        }

        //sb.AppendFormat(" and (loginname in(select distinct loginname from v_ps_employee where 1=1 {0}) or loginname = '{1}')", SessionInfo.DataFilter,SessionInfo.LoginName);
        
        sb.Append(" order by logtime");

        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this.Master;
        //gvData.HiddenFields = "id,funcname,tablename,CONTENT";
        //gvData.HeaderDescriptions = "ID" + "," + Resources.Labels.LoginName + "," + Resources.Labels.AccessTime + "," + Resources.Labels.Action + "," + Resources.Labels.Function + "," + Resources.Labels.TableName + "," + Resources.Labels.IpAddress;

    }

    void CheckDeletePrerequisite(PCTable table, Hashtable row)
    {
        //check whether the record can be deleted£¬ throw exception if error
    }

    void DeleteRecord()
    {
        this.Master.DeleteRecord(TABLE_NAME, gvData, CheckDeletePrerequisite);
    }

    //protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    TableCellCollection cells;
    //    switch (e.Row.RowType)
    //    {
    //        case DataControlRowType.DataRow:
    //            cells = e.Row.Cells;
    //            //e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text).ToString(Consts.DateFormat);
    //           // StringBuilder sb = new StringBuilder();
    //            // sb.AppendFormat("ID: {0}", cells[0].Text);
    //            //if (cells[7].Text == "&nbsp;")
    //            //{
    //            //    sb.AppendFormat("Content: {0}", "\n" + "No Record.");
    //            //}
    //            //else
    //            //{
    //            //    sb.AppendFormat("Content: {0}", "\n" + SplitString(cells[7].Text, 9));
    //            //    cells[7].Text = SplitString(cells[7].Text.Trim());
    //            //}
    //            //e.Row.ToolTip = sb.ToString();
    //            break;
    //    }
    //}
    protected void gvData_OnPageIndexChanging(object sender, GridViewPageEventArgs e)
    {
        if (e.NewPageIndex >= gvData.PageCount)
            gvData.PageIndex = gvData.PageCount;
        else
            gvData.PageIndex = e.NewPageIndex;
        gvData.DataBind();
    }
    //protected void gvData_OnSorting(object sender, GridViewSortEventArgs e)
    //{
    //    SetDataSource();
    //    //gvData.Sort(e.SortExpression, e.SortDirection);
    //    //gvData.DataBind();
    //}
    protected string  SplitString(string str,int iCount)
    {
        char[] charsplit = new char[] {'\n'};
        string[] split = str.Split(charsplit);
        string value=null;
        int i;
        if (split.Length > iCount)
        {
            for (i = 0; i <= iCount; i++)
            {
                value += split[i] + "\n";
            }
            value += "......\n";
        }
        else
        {
            for (i = 0; i <= split.Length-1; i++)
            {
                value += split[i] + "\n";
            }
        }
        return value;
    }

    protected string SplitString(string str)
    {
        char[] charsplit = new char[] { '\n' };
        string[] split = str.Split(charsplit);
        string value = null;
        int i;

        for (i = 0; i <= split.Length - 1; i++)
        {
            value += split[i] + "&^&";
        }

        return value;
    }

    [AjaxPro.AjaxMethod]
    public string GetLogDetail(string id)
    {
        string oldcontent = "", newcontent = "", content = ""; DataTable ldt = null;
        StringBuilder sb = new StringBuilder();
        ldt = PCDb.Db.ExecQuery("select s.*,l.action from sc_logdetail s inner join sc_log l on l.id=s.logid where logid=" + Convert.ToDecimal(id) + "");
        if (ldt.Rows.Count > 0)
        {
            for (int i = 0; i < ldt.Rows.Count; i++)
            {
                if (ldt.Rows[i]["OLDVALUE"].ToString() != "")
                    oldcontent = ldt.Rows[i]["FIELDNAME"].ToString() + " :" + ldt.Rows[i]["OLDVALUE"].ToString();
                if (ldt.Rows[i]["NEWVALUE"].ToString() != "")
                {
                    if (ldt.Rows[i]["action"].ToString() == "Update")
                    {
                        newcontent = ldt.Rows[i]["NEWVALUE"].ToString();
                    }
                    else
                        newcontent = ldt.Rows[i]["FIELDNAME"].ToString() + " :" + ldt.Rows[i]["NEWVALUE"].ToString();

                }
                if (oldcontent != "" && newcontent != "")
                {
                    content += oldcontent + "->" + newcontent + " ";
                    content = SplitString(content.Trim());
                }
                if (oldcontent == "" && newcontent != "")
                {
                    content += newcontent + " ";
                    content = SplitString(content.Trim());
                }
                if (oldcontent != "" && newcontent == "")
                {
                    content += oldcontent + " ";
                    content = SplitString(content.Trim());
                }
            }
        }

        return content;
    }

}
