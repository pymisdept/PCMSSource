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

public partial class ScLog : BasePage
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
            //CDADb.InitDropDownListRegion(ddlRegion, Consts.DropDownOptionAll, null);
        }

        if (this.CurrentCommand!=Consts.ButtonExport) SetDataSource();
        SetToolBarButtonVisable();

    }//end of Page_Load

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
        StringBuilder sb = new StringBuilder(String.Format("select ID,LOGINNAME,LOGTIME,ACTION,FUNCNAME,TABLENAME,IPADDRESS,CONTENT from {0} where 1=1 ", TABLE_NAME));
        Db db =PCDb.Db; 
        if (!String.IsNullOrEmpty(txtLoginName.Text))
        {
            sb.AppendFormat(" and loginname like '%{0}%'", txtLoginName.DBText.Trim());
        }
        if (!String.IsNullOrEmpty(ddlAction.SelectedValue))
        {
            if (ddlAction.SelectedValue.Trim() != "-All-")
                sb.AppendFormat(" and Action like '%{0}%'", ddlAction.SelectedValue.Trim()); 
            else
                sb.AppendFormat(" and Action not like '%Insert%' and Action not like '%Update%' and Action not like '%Delete%'");
        }
        if (!String.IsNullOrEmpty(txtStartLogTime.Text) && SimpleRegex.IsDateYMD(txtStartLogTime.Text))
        {
            sb.AppendFormat(" and  logtime>= '{0}'", txtStartLogTime.DBText.Trim());
        }
        if (!String.IsNullOrEmpty(txtEndLogTime.Text) && SimpleRegex.IsDateYMD(txtEndLogTime.Text))
        {
            sb.AppendFormat(" and logtime<= '{0}'", txtEndLogTime.DBText.Trim() + Consts.MaxTime);
        }
        //sb.Append(" order by logtime");
        sb.Append(" order by logtime DESC");


        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this.Master;
        gvData.HeaderDescriptions = "ID" + "," + Resources.Labels.LoginName + "," + Resources.Labels.LogTime + "," + Resources.Labels.Action + "," + Resources.Labels.Function + "," + Resources.Labels.TableName + "," + Resources.Labels.IpAddress;

    }

    void CheckDeletePrerequisite(PCTable table, Hashtable row)
    {
        //check whether the record can be deleted£¬ throw exception if error
    }

    void DeleteRecord()
    {
        this.Master.DeleteRecord(TABLE_NAME, gvData, CheckDeletePrerequisite);
    }

    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        TableCellCollection cells;
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                cells = e.Row.Cells;
                e.Row.Cells[2].Text = Convert.ToDateTime(e.Row.Cells[2].Text).ToString(Consts.DateFormat) + " " +
                    Convert.ToDateTime(e.Row.Cells[2].Text).ToLongTimeString().ToString();
                StringBuilder sb = new StringBuilder();
               // sb.AppendFormat("ID: {0}", cells[0].Text);
                if (cells[7].Text == "&nbsp;")
                {
                    sb.AppendFormat("Content: {0}", "\n" + "No Record.");
                }
                else
                {
                    sb.AppendFormat("Content: {0}", "\n" + SplitString(cells[7].Text,9));
                    cells[7].Text = SplitString(cells[7].Text.Trim());
                }
                e.Row.ToolTip = sb.ToString();
                break;
        }
    }

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

}
