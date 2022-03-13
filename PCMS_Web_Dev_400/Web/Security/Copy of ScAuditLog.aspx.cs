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
using System.IO;

public partial class ScAuditLog : BasePage
{
    const string TABLE_NAME = Consts.TableScAuditLog;
    const string VIEW_NAME = "v_sc_log";

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ClearError();
        RegisterClientId();
        txtDisplayValue.Font.Name = "Courier New";

        if (Page.IsPostBack)
        {
            string cmd = this.CurrentCommand;
            switch (cmd)
            {
                case Consts.ButtonExport:
                    ExportRecord();
                    break;
            }
        }
        else
        {
            txtStartLogTime.Text = DateTime.Today.ToString(Consts.DateFormat);
            txtEndLogTime.Text = DateTime.Today.ToString(Consts.DateFormat);
            OnInitDropDownListRegion();
        }
        gvData.HiddenFields = "ID,TABLENAME";
        gvData.HeaderDescriptions = "ID" + "," + Resources.Labels.LoginName + "," + Resources.Labels.AccessTime + "," + Resources.Labels.Action + "," + Resources.Labels.Function + "," + Resources.Labels.TableName + "," + Resources.Labels.IpAddress + "," + Resources.Labels.Module + "," + Resources.Labels.Project;
        if (this.CurrentCommand != Consts.ButtonExport) SetDataSource();
        SetToolBarButtonVisable();
        AjaxPro.Utility.RegisterTypeForAjax(typeof(ScAuditLog));
    }//end of Page_Load

    protected void OnInitDropDownListRegion()
    {
        PCDb.InitDropDownList(ddlLoginName, "v_sc_users", "ID", "LOGINNAME", Consts.DropDownOptionAll, null, null);
        PCDb.InitDropDownList(this.ddlModule, "sc_module", "ID", "Name", Consts.DropDownOptionAll, null, null, " SHOWORDER ");
        

        for (int i = 0; i < ddlModule.Items.Count; i++)
        {
            if (i == 0)
            {
                ddlModule.Items[i].Text = "-" + (string)HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "All") + "-";
            }
            else
            {
                //ddlModule.Items[i].Text = (string)HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, ddlModule.Items[i].Text);
            }
        }
        
        PCDb.InitDropDownList(ddlFunction, Consts.TableScFunction, "ID", "NAME", Consts.DropDownOptionAll, null, "mid='" + ddlModule.SelectedValue + "'");

        for (int i = 0; i < ddlFunction.Items.Count; i++)
        {
            if (i == 0)
            {
                ddlFunction.Items[i].Text = "-" + (string)HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "All") + "-";
            }
            else
            {
                //ddlFunction.Items[i].Text = (string)HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, ddlFunction.Items[i].Text);
            }
        }

        ListItem li1 = new ListItem("-" + Resources.Labels.All + "-","1");
        ListItem li2 = new ListItem(Resources.Labels.Insert, "Insert");
        ListItem li3 = new ListItem(Resources.Labels.Update, "Update");
        ListItem li4 = new ListItem(Resources.Labels.Delete, "Delete");
        ListItem[] li = new ListItem[] { li1,li2,li3,li4 };
        ddlAction.Items.AddRange(li);
    }

    protected void RegisterClientId()
    {
        string regID = " var divID = '" + divdisplayvalue.ClientID.ToString() + "';";

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
            "ACTION,FUNCNAME,TABLENAME,IPADDRESS,MODULE,PROJECT from {0} where 1=1  ", VIEW_NAME));
        Db db = PCDb.Db;

        if (ddlLoginName.SelectedIndex > 0)
        {
            sb.AppendFormat(" and loginname like '%{0}%'", ddlLoginName.SelectedItem.Text.Trim());
        }
        //if (!String.IsNullOrEmpty(ddlAction.SelectedValue))
        //{
        //    if (ddlAction.SelectedValue.Trim() != "-All-")
        //    sb.AppendFormat(" and Action like '%{0}%'", ddlAction.SelectedValue.Trim());
        //    else
        //    sb.AppendFormat(" and Action like '%Insert%' or Action like '%Update%' or Action like '%Delete%'");
        //}

        if (!String.IsNullOrEmpty(txtStartLogTime.Text) && SimpleRegex.IsDateYMD(txtStartLogTime.Text))
        {
            sb.AppendFormat(" and  logtime>= '{0}'", txtStartLogTime.DBText.Trim());
        }
        if (!String.IsNullOrEmpty(txtEndLogTime.Text) && SimpleRegex.IsDateYMD(txtEndLogTime.Text))
        {
            sb.AppendFormat(" and logtime<= '{0}'", txtEndLogTime.DBText.Trim() + Consts.MaxTime);
        }

        if (ddlModule.SelectedIndex > 0)
        {
            sb.Append(" and mid='" + ddlModule.SelectedValue + "'");
        }

        if (ddlFunction.SelectedIndex > 0)
        {
            sb.Append(" and funcid='" + ddlFunction.SelectedValue + "' ");
        }
        if (ddlAction.SelectedIndex < 1)
        {
            sb.Append("  and (Action like '%Insert%' or Action like '%Update%' or Action like '%Delete%')");
        }
        else
        {
            sb.Append("   and Action like '%" + ddlAction.SelectedValue + "%' ");
        }

        sb.Append(" order by logtime");


        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this.Master;
        
        gvData.HiddenFields = "ID,MODULE";
        gvData.HeaderDescriptions = "ID" + "," + Resources.Labels.LoginName + "," + Resources.Labels.AccessTime + "," + Resources.Labels.Action + "," + Resources.Labels.Function + "," + Resources.Labels.TableName + "," + Resources.Labels.IpAddress + "," + Resources.Labels.Module + "," + Resources.Labels.Project;
        gvData.DataSource = dsGridView;
       
    }

    void ExportRecord()
    {
        StringBuilder sb = new StringBuilder(String.Format("select ID,LOGINNAME,convert(varchar(10),logtime,120)+' '+left(convert(varchar(10),logtime,108),5) as LOGTIME,"+
            "ACTION,FUNCNAME,TABLENAME,IPADDRESS,MODULE,PROJECT from {0} where 1=1  ", VIEW_NAME));        

        if (ddlLoginName.SelectedIndex > 0)
        {
            sb.AppendFormat(" and loginname like '%{0}%'", ddlLoginName.SelectedItem.Text.Trim());
        }
        //if (!String.IsNullOrEmpty(ddlAction.SelectedValue))
        //{
        //    if (ddlAction.SelectedValue.Trim() != "-All-")
        //    sb.AppendFormat(" and Action like '%{0}%'", ddlAction.SelectedValue.Trim());
        //    else
        //    sb.AppendFormat(" and Action like '%Insert%' or Action like '%Update%' or Action like '%Delete%'");
        //}

        if (!String.IsNullOrEmpty(txtStartLogTime.Text) && SimpleRegex.IsDateYMD(txtStartLogTime.Text))
        {
            sb.AppendFormat(" and  logtime>= '{0}'", txtStartLogTime.DBText.Trim());
        }
        if (!String.IsNullOrEmpty(txtEndLogTime.Text) && SimpleRegex.IsDateYMD(txtEndLogTime.Text))
        {
            sb.AppendFormat(" and logtime<= '{0}'", txtEndLogTime.DBText.Trim() + Consts.MaxTime);
        }

        if (ddlModule.SelectedIndex > 0)
        {
            sb.Append(" and mid='" + ddlModule.SelectedValue + "'");
        }

        if (ddlFunction.SelectedIndex > 0)
        {
            sb.Append(" and funcid='" + ddlFunction.SelectedValue + "' ");
        }
        if (ddlAction.SelectedIndex < 1)
        {
            sb.Append("  and (Action like '%Insert%' or Action like '%Update%' or Action like '%Delete%')");
        }
        else
        {
            sb.Append("   and Action like '%" + ddlAction.SelectedValue + "%' ");
        }
        sb.Append(" order by logtime");

        DataTable dt = PCDb.Db.ExecQuery(sb.ToString());

        for (int i = 0; i < dt.Rows.Count; i++)
        {
            dt.Rows[i]["action"] = PCCore.ComFunction2.GetGlobalResourceObject_ByStringID("Labels", dt.Rows[i]["action"].ToString());
            dt.Rows[i]["funcname"] = PCCore.ComFunction2.GetGlobalResourceObject_ByStringID("Labels", dt.Rows[i]["funcname"].ToString());
            dt.Rows[i]["module"] = PCCore.ComFunction2.GetGlobalResourceObject_ByStringID("Labels", dt.Rows[i]["module"].ToString());
        }

        string hidFiled = "ID,MODULE";
        string header = "ID" + "," + Resources.Labels.LoginName + "," + Resources.Labels.AccessTime + "," + Resources.Labels.Action + "," + Resources.Labels.Function + "," + Resources.Labels.TableName + "," + Resources.Labels.IpAddress + "," + Resources.Labels.Module + "," + Resources.Labels.Remarks;


        ExportToExcel(dt, header, hidFiled);
    }
    void ExportToExcel(DataTable dt, string header, string hidFiled)
    {
        string fileName = String.Format("exp{0}.xls", DateTime.Now.ToString("yyMMddHHmmssfff"));
        string fullPath = String.Format("{0}\\{1}", SessionInfo.TempDir, fileName);

        if (!Directory.Exists(SessionInfo.TempDir))
            Directory.CreateDirectory(SessionInfo.TempDir);

        string caption = String.Empty;
        caption = HttpContext.GetGlobalResourceObject("Labels", "ScAuditLog", new System.Globalization.CultureInfo(SessionInfo.CurrentLanguage)).ToString();

        SimpleExcelExport.Export(dt, fullPath, header, hidFiled, caption, 0);
        SimpleWebUtils.DownloadFile(Page.Response, fullPath);
    }
    //protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    //{
    //    TableCellCollection cells;
    //    switch (e.Row.RowType)
    //    {
    //        case DataControlRowType.DataRow:
    //            cells = e.Row.Cells;
    //            e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text).ToString(Consts.DateFormat);
    //            StringBuilder sb = new StringBuilder();
    //            break;
    //    }
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

    protected void ddlAction_SelectedIndexChanged(object sender, EventArgs ex)
    {
        StringBuilder sb = new StringBuilder(String.Format("select ID,LOGINNAME,LOGTIME,ACTION,FUNCNAME,TABLENAME,IPADDRESS,MODULE from {0} where 1=1  ", VIEW_NAME));
        Db db = PCDb.Db;

        if (ddlLoginName.SelectedIndex > 0)
        {
            sb.AppendFormat(" and loginname like '%{0}%'", ddlLoginName.SelectedItem.Text.Trim());
        }
        //if (!String.IsNullOrEmpty(ddlAction.SelectedValue))
        //{
        //    if (ddlAction.SelectedValue.Trim() != "-All-")
        //    sb.AppendFormat(" and Action like '%{0}%'", ddlAction.SelectedValue.Trim());
        //    else
        //    sb.AppendFormat(" and Action like '%Insert%' or Action like '%Update%' or Action like '%Delete%'");
        //}

        if (!String.IsNullOrEmpty(txtStartLogTime.Text) && SimpleRegex.IsDateYMD(txtStartLogTime.Text))
        {
            sb.AppendFormat(" and  logtime>= '{0}'", txtStartLogTime.DBText.Trim());
        }
        if (!String.IsNullOrEmpty(txtEndLogTime.Text) && SimpleRegex.IsDateYMD(txtEndLogTime.Text))
        {
            sb.AppendFormat(" and logtime<= '{0}'", txtEndLogTime.DBText.Trim() + Consts.MaxTime);
        }

        if (ddlModule.SelectedIndex > 0)
        {
            sb.Append(" and mid='" + ddlModule.SelectedValue + "'");
        }

        if (ddlFunction.SelectedIndex > 0)
        {
            sb.Append(" and funcid='" + ddlFunction.SelectedValue + "' ");
        }
        if (ddlAction.SelectedIndex < 1)
        {
            sb.Append("  and (Action like '%Insert%' or Action like '%Update%' or Action like '%Delete%')");
        }
        else
        {
            sb.Append("   and Action like '%" + ddlAction.SelectedValue + "%' ");
        }

        sb.Append(" order by logtime");


        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this.Master;
        gvData.HiddenFields = "ID,TABLENAME";
        gvData.HeaderDescriptions = "ID" + "," + Resources.Labels.LoginName + "," + Resources.Labels.AccessTime + "," + Resources.Labels.Action + "," + Resources.Labels.Function + "," + Resources.Labels.TableName + "," + Resources.Labels.IpAddress + "," + Resources.Labels.Module + "," + Resources.Labels.Module;

    }

    protected void ddlModule_SelectedIndexChanged(object sender, EventArgs ex)
    {
        PCDb.InitDropDownList(ddlFunction, Consts.TableScFunction, "ID", "NAME", Consts.DropDownOptionAll, null, "mid='" + ddlModule.SelectedValue + "'");
        for (int i = 0; i < ddlFunction.Items.Count; i++)
        {
            if (i == 0)
            {
                ddlFunction.Items[i].Text = "-" + (string)HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "All") + "-";
            }
            else
            {
                //ddlFunction.Items[i].Text = (string)HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, ddlFunction.Items[i].Text);
            }
        }
    }

    [AjaxPro.AjaxMethod]
    public string GetLogDetail(string id)
    {
        string oldcontent = "", newcontent = "", content = "", change = ""; ; DataTable ldt = null;
        StringBuilder sb = new StringBuilder();
        ldt = PCDb.Db.ExecQuery("select s.*,l.action from sc_logdetail s inner join sc_log l on l.id=s.logid where logid=" + Convert.ToDecimal(id) + "");
        if (ldt.Rows.Count > 0)
        {
            for (int i = 0; i < ldt.Rows.Count; i++)
            {

                if (ldt.Rows[i]["action"].ToString() == "Update")
                {
                    newcontent = ldt.Rows[i]["NEWVALUE"].ToString();
                    oldcontent = ldt.Rows[i]["oldVALUE"].ToString();
                    change = ldt.Rows[i]["changed"].ToString();
                    if (change == "1")
                    {
                        if (oldcontent != "" && newcontent != "" && change == "1")
                        {
                            oldcontent = "*" + ldt.Rows[i]["FIELDNAME"].ToString() + " :" + ldt.Rows[i]["OLDVALUE"].ToString();
                            content += oldcontent + "->" + newcontent + " ";
                            content = SplitString(content.Trim());
                        }
                        if (oldcontent != "" && newcontent == "")
                        {
                            oldcontent = " " + ldt.Rows[i]["FIELDNAME"].ToString() + " :" + ldt.Rows[i]["OLDVALUE"].ToString();
                            content += oldcontent + " ";
                            content = SplitString(content.Trim());

                        }
                        if (oldcontent == "" && newcontent != "")
                        {
                            newcontent = " " + ldt.Rows[i]["FIELDNAME"].ToString() + " :" + ldt.Rows[i]["NEWVALUE"].ToString();
                            content += newcontent + " ";
                            content = SplitString(content.Trim());
                        }
                    }
                    else
                    {
                        oldcontent = " " + ldt.Rows[i]["FIELDNAME"].ToString() + " :" + ldt.Rows[i]["OLDVALUE"].ToString();
                        content += oldcontent + " ";
                        content = SplitString(content.Trim());
                       
                    }
                   
                }
                else
                {
                    if (ldt.Rows[i]["action"].ToString() == "Insert")
                    {
                        newcontent = ldt.Rows[i]["FIELDNAME"].ToString() + " :" + ldt.Rows[i]["NEWVALUE"].ToString();
                        content += newcontent + " ";
                        content = SplitString(content.Trim());
                    }

                    if (ldt.Rows[i]["action"].ToString() == "Delete")
                    {
                        oldcontent = ldt.Rows[i]["FIELDNAME"].ToString() + " :" + ldt.Rows[i]["OLDVALUE"].ToString();
                        content += oldcontent + " ";
                        content = SplitString(content.Trim());
                    }
                }
            }

            if (ldt.Rows[0]["action"].ToString() == "Update")
                content = " " + content;

        }
        return content;
    }
}
