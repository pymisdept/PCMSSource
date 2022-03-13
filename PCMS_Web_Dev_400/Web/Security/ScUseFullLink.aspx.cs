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

public partial class ScUseFullLink : BasePage
{
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
        }

        gvData.HeaderDescriptions = "ID," + Resources.Labels.Name + "," + Resources.Labels.UrlAddress + "," + Resources.Labels.Priority;
        gvData.HiddenFields += "priorityid";
        if (this.CurrentCommand!=Consts.ButtonExport) SetDataSource();

    }//end of Page_Load

    protected void SetDataSource()
    {
        StringBuilder sb = new StringBuilder("select ID,  Name, UrlAddress ,PriorityID from Sc_Usefull_Link where 1=1");
        
        if (!String.IsNullOrEmpty(txtSearchBox.Text))
        {
            sb.AppendFormat(" and (Name like '%{0}%' or UrlAddress like'%{0}%')", txtSearchBox.DBText.Trim());
        }

        sb.Append(" order by priorityid,Name");

        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this.Master;

    }

    void CheckDeletePrerequisite(PCTable table, Hashtable row)
    {        

    }

    void DeleteRecord()
    {
        this.Master.DeleteRecord("sc_usefull_link", gvData, CheckDeletePrerequisite);
    }

    protected void gvData_RowCreated(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
            case DataControlRowType.DataRow:
                e.Row.Cells[1].Width = Unit.Pixel(350);
                e.Row.Cells[2].Width = Unit.Pixel(350);
                //e.Row.Cells[3].Width = Unit.Pixel(50);
                break;
        }
    }
}
