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

public partial class ScGroup : BasePage
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

        gvData.HeaderDescriptions = "ID," + Resources.Labels.Code + "," + Resources.Labels.Name;
        if (this.CurrentCommand!=Consts.ButtonExport) SetDataSource();

    }//end of Page_Load

    protected void SetDataSource()
    {
        StringBuilder sb = new StringBuilder("select * from v_sc_groups where 1=1");
        
        if (!String.IsNullOrEmpty(txtGroupUser.Text))
        {
            sb.AppendFormat(" and (code like '%{0}%' or name like'%{0}%')", txtGroupUser.DBText.Trim());
        }
        sb.Append(" order by code");

        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this.Master;
        
        gvData.HeaderDescriptions = "ID," + Resources.Labels.Code + "," + Resources.Labels.Name;
    }

    void CheckDeletePrerequisite(PCTable table, Hashtable row)
    {        
        string gID = row[Consts.FieldID].ToString();

        string where = String.Format("groupid={0}", gID);
        PCTable gu = new PCTable(Consts.TableScGroupUser, this.SecurityInfo);
        gu.UseTransaction(table.InternalTransaction);
        gu.Delete(where);

        where = String.Format("sctype='g' and id={0}", gID);
        PCTable r = new PCTable(Consts.TableScRight, this.SecurityInfo);
        r.UseTransaction(table.InternalTransaction);
        r.Delete(where);
        
    }

    void DeleteRecord()
    {
        this.Master.DeleteRecord(Consts.TableScGroup, gvData, CheckDeletePrerequisite);
    }
}// end of class
