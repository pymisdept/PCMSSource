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
using SimpleControls.SimpleDatabase;

public partial class ScUseFullLink_Form : BasePage
{
    protected PCTable _table = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;

    protected PCTable Table
    {
        get
        {
            if (_table == null) _table = new PCTable("Sc_Usefull_Link", this.SecurityInfo);
            return _table;
        }
    }


    protected override void OnInit(EventArgs e)
    {
        base.ShowWebMenu = false;   
        base.OnInit(e);
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Save += new MasterSecurityForm.SaveEventHandler(Master_Save);
        ((Button)Master.FindControl("btnRequest")).Visible = false;
        Master.Title = Resources.Labels.ScUseFullLink;

        if (Page.IsPostBack)
        {
            
        }
        else
        {
            InitDropDownlist();
            switch (Master.FormMode)
            {
                case MasterSecurityForm.FormModes.New:
                    txtUrlAddress.Text = "http://";
                    break;

                case MasterSecurityForm.FormModes.Edit:
                    txtID.Text = Master.RecordID;

                    _dr = this.Table.GetRow(Master.RecordID);
                    if (_dr != null)
                    {
                        Master.Record = _dr;
                        Page.DataBind();
                        //Init ddl value
                        ddlPriority.SelectedValue = _dr["priorityid"].ToString();
                    }
                    else
                    {
                        Master.ShowWarning(String.Format(Resources.Common.InvalidIDOrMultipleRecord, Master.RecordID));
                    }
                    break;
            }
            
        }
    }

    void InitDropDownlist()
    {
        ListItem li = null;
        li = new ListItem(Resources.Labels.None, "99");
        ddlPriority.Items.Add(li);
        for (int i = 1; i < 21; i++)
        {
            li = new ListItem(i.ToString(), i.ToString());
            ddlPriority.Items.Add(li);
        }
        ddlPriority.SelectedIndex = 0;
    }

    Hashtable GetRow()
    {
        Hashtable row = new Hashtable();
        if (Master.FormMode == MasterSecurityForm.FormModes.New)
            row.Add(Consts.FieldID, PCDb.GetNextID());
        else
            row.Add(Consts.FieldID, Convert.ToDecimal(txtID.Text));
        row.Add(Consts.FieldName, txtName.Text.Trim());
        row.Add("URLADDRESS", txtUrlAddress.Text.Trim());
        row.Add("PRIORITYID", Convert.ToDecimal(ddlPriority.SelectedValue));
        return row;
    }

    void Master_Save(object sender, EventArgs e)
    {
        if (!CheckForm()) return;
        try
        {
            Hashtable row = GetRow();
            switch (Master.FormMode)
            {
                case MasterSecurityForm.FormModes.New:
                    this.Table.BeginTransaction();
                    this.Table.Insert(row);
                    this.Table.CommitTransaction();
                    Master.ShowMessage(String.Format(Resources.Common.SaveOK, txtName.Text));
                    Master.ClearForm();
                    break;

                case MasterSecurityForm.FormModes.Edit:
                    this.Table.BeginTransaction();
                    this.Table.Update(row);
                    this.Table.CommitTransaction();
                    Master.ShowMessage(String.Format(Resources.Common.UpdateOK, txtName.Text));
                    break;
            }
        }
        catch (Exception ex)
        {
            Master.HandleError(ex);
            this.Table.RollbackTransaction();
        }
    }

    bool CheckForm()
    {
        if (String.IsNullOrEmpty(txtName.Text))
        {
            Master.ShowMessage(txtName.RequiredErrorMessage);
            return false;
        }
        if (String.IsNullOrEmpty(txtUrlAddress.Text))
        {
            Master.ShowMessage(txtUrlAddress.RequiredErrorMessage);
            return false;
        }
        return true;
    }

}//end of class
