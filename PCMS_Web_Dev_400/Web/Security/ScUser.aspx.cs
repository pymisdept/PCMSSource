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

public partial class ScUser : BasePage
{
    const string TABLE_NAME = Consts.TableScUser;
    const string VIEW_NAME = "v_sc_users";

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ClearError();

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
            InitDropDownListRegion();
        }

        gvData.HeaderDescriptions = "ID," + Resources.Labels.FullName + "," + Resources.Labels.LoginName + "," + Resources.Labels.Locked + "," + Resources.Labels.Supervisor + "," + Resources.Labels.Reverse;
        if (this.CurrentCommand != Consts.ButtonExport) SetDataSource();

    }//end of Page_Load

    protected void InitDropDownListRegion()
    {
        PCDb.InitDropDownList(ddlLoninName, VIEW_NAME, "ID", "LOGINNAME", Consts.DropDownOptionAll, null, null);
    }

    protected void SetDataSource()
    {
        // Martin update begin 12 April 2011
        StringBuilder sb = new StringBuilder(String.Format("select id,fullname,loginname, locked, supervisor, reverse from {0} where 1=1 ", VIEW_NAME));
        // Martin update end 12 April 2011

        if (ddlLoninName.SelectedIndex > 0  )
        {
            sb.AppendFormat(" and id = '{0}' ", ddlLoninName.SelectedValue);
        }
        if (!String.IsNullOrEmpty(txtFullName.Text))
        {
            sb.AppendFormat(" and fullname like '%{0}%'", txtFullName.DBText.Trim());
        }        
        sb.Append(" order by loginname");

        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this.Master;

        gvData.HeaderDescriptions = "ID," + Resources.Labels.FullName + "," + Resources.Labels.LoginName + "," + Resources.Labels.Locked + "," + Resources.Labels.Supervisor + "," + Resources.Labels.Reverse;
    }

    void CheckDeletePrerequisite(PCTable table, Hashtable row)
    {
        //check whether the record can be deleted£¬ throw exception if error
        string guID = row[Consts.FieldID].ToString();

        string where = String.Format("userid={0}", guID);
        PCTable gu = new PCTable(Consts.TableScGroupUser, this.SecurityInfo);
        gu.UseTransaction(table.InternalTransaction);
        gu.Delete(where);
    }

    void DeleteRecord()
    {
        this.Master.DeleteRecord(TABLE_NAME, gvData, CheckDeletePrerequisite);
    }

    protected void gvData_RowCreated(object sender, GridViewRowEventArgs e)
    {
        if (e.Row.Cells.Count > 1)
            e.Row.Cells[0].Style.Add("display", "none");
    }   
}
