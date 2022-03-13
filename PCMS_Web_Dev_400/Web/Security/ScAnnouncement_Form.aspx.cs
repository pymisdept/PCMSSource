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

public partial class ScAnnouncement_Form : BasePage
{
    protected PCTable _table = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;

    protected PCTable Table
    {
        get
        {
            if (_table == null) _table = new PCTable("SC_ANNOUNCEMENT", this.SecurityInfo);
            return _table;
        }
    }

    protected void gvData_RowCreated(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
                e.Row.Cells[0].HorizontalAlign = System.Web.UI.WebControls.HorizontalAlign.Left;
                e.Row.Cells[0].BackColor = System.Drawing.Color.Gray;
                e.Row.Cells[1].BackColor = System.Drawing.Color.Gray;
                e.Row.Cells[2].BackColor = System.Drawing.Color.Gray;
                e.Row.Cells[3].BackColor = System.Drawing.Color.Gray;
                e.Row.Cells[4].BackColor = System.Drawing.Color.Gray;
                break;
            case DataControlRowType.DataRow:
                //e.Row.Cells[1].Style.Add("display", "none");//INGROUP
                //e.Row.Cells[2].Style.Add("display", "none");//GROUPID
                break;
        }
    }

    protected override void OnInit(EventArgs e)
    {

        base.ShowWebMenu = false;   
        base.OnInit(e);
        
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Save += new MasterForm.SaveEventHandler(Master_Save);

        Master.Title = Resources.Labels.ScAnnouncement;

        if (SessionInfo.IsSupervisor)
        {
            rbMth.Items[1].Enabled = true;
        }
        else
        {
            rbMth.Items[1].Enabled = false;
        }

        ((Button)Master.FindControl("btnRequest")).Visible = false;
        
        
        //SetDataSource(ddlProject.SelectedValue);
        
        Regscript();
        if (Page.IsPostBack)
        {
           
                if (invProject.Text != String.Empty)
                {
                    PCCore.Common.HRLog.RecordLog(invProject.Text);
                    PCCore.Common.HRLog.RecordLog(ddlProject.Items.Count);

                    ddlProject.SelectedValue = invProject.Text;
                    invProject.Text = String.Empty;
                }
           
        }
        else
        {
            ddlFunction.Enabled = false;
            // Bind Function List
            PCCore.Database.ValidationList.AnnouncementFunctionList(ddlFunction);
            if (SessionInfo.IsSupervisor == false)
                PCCore.Database.ValidationList.UserProjectList(ddlProject, SessionInfo.UserId, Consts.DropDownOptionAll);
            else
                PCCore.Database.ValidationList.ProjectList(ddlProject);

            rbMth.Items[0].Selected = true;
            this.tblUser.Visible = true;
            this.tblProject.Visible = true;
            // Set Project and User
            PCCore.Database.ValidationList.SetProjectCheckBoxListItems(cblProject);
            SetDataSource(Consts.DropDownAllValue);
            
            switch (Master.FormMode)
            {
                case MasterForm.FormModes.New:
                    txtEffectiveDate.Text = DateTime.Today.ToString(Consts.DateFormat);
                    break;

                case MasterForm.FormModes.Edit:
                    txtID.Text = Master.RecordID;

                    _dr = this.Table.GetRow(Master.RecordID);
                    if (_dr != null)
                    {
                        Master.Record = _dr;
                        Page.DataBind();
                        //Init ddl value
                        txtEffectiveDate.Text = String.IsNullOrEmpty(_dr["EFFECTIVEDATE"].ToString()) ? String.Empty : Convert.ToDateTime(_dr["EFFECTIVEDATE"]).ToString(Consts.DateFormat); 
                        
                        txtExpiryDate.Text = String.IsNullOrEmpty(_dr["EXPIRYDATE"].ToString()) ? String.Empty:Convert.ToDateTime(_dr["EXPIRYDATE"]).ToString(Consts.DateFormat);
                    }
                    else
                    {
                        Master.ShowWarning(String.Format(Resources.Common.InvalidIDOrMultipleRecord, Master.RecordID));
                    }
                    break;
            }
            
        }
        
    }

    Hashtable GetRow()
    {
        Hashtable row = new Hashtable();
        if (Master.FormMode == MasterForm.FormModes.New)
            row.Add(Consts.FieldID, PCDb.GetNextID());
        else
            row.Add(Consts.FieldID, Convert.ToDecimal(txtID.Text));
        row.Add("TITLE", txtAnnouncementTitle.Text.Trim());
        row.Add("BODY", txtBody.Text.Trim());
        row.Add("EFFECTIVEDATE", txtEffectiveDate.Text);
        if (!String.IsNullOrEmpty(txtExpiryDate.Text))
        {
            row.Add("EXPIRYDATE", txtExpiryDate.Text);
        }
        else
        {
            row.Add("EXPIRYDATE", DBNull.Value);
        }
        return row;
    }

    void Master_Save(object sender, EventArgs e)
    {

        if (!CheckForm()) return;
        switch (rbMth.SelectedIndex)
        {
            case 0:
                SaveByUser();
                break;
            case 1:
                SaveGlobally();
                break;
            case 2:
                SaveByFunction();
                break;
         
        }

        //try
        //{
        //    Hashtable row = GetRow();
        //    switch (Master.FormMode)
        //    {
        //        case MasterForm.FormModes.New:


        //            //this.Table.BeginTransaction();
        //            //this.Table.Insert(row);
        //            //this.Table.CommitTransaction();
        //            Master.ShowMessage(String.Format(Resources.Common.SaveOK, txtAnnouncementTitle.Text));
        //            Master.ClearForm();


        //            break;

        //        case MasterForm.FormModes.Edit:
        //            //this.Table.BeginTransaction();
        //            //this.Table.Update(row);
        //            //this.Table.CommitTransaction();
        //            Master.ShowMessage(String.Format(Resources.Common.UpdateOK, txtAnnouncementTitle.Text));
        //            break;
        //    }
        //}
        //catch (Exception ex)
        //{
        //    Master.HandleError(ex);
        //    this.Table.RollbackTransaction();
        //}
    }

    bool CheckForm()
    {
        bool _selcnt = false;
        switch (rbMth.SelectedIndex)
        {
                
            case 0:
                //bool _selcnt = false;
                ICollection ic = gvData.SelectedRowValuesCollection;
                //IEnumerator ie = ic.GetEnumerator();
                if (ic.Count == 0)
                {
                    Master.ShowMessage(Resources.Messages.RequiredUserCode);
                    return false;
                }
                
                break;
            case 2:
                if (ddlFunction.SelectedIndex == 0)
                {
                    Master.ShowMessage(Resources.Messages.RequiredFuctionCode);
                    return false;
                }

                break;

        }
        if (String.IsNullOrEmpty(txtAnnouncementTitle.Text))
        {
            Master.ShowMessage(txtAnnouncementTitle.RequiredErrorMessage);
            return false;
        }
        if (String.IsNullOrEmpty(txtBody.Text))
        {
            Master.ShowMessage(txtBody.RequiredErrorMessage);
            return false;
        }
        if (String.IsNullOrEmpty(txtEffectiveDate.Text))
        {
            Master.ShowMessage(txtEffectiveDate.RequiredErrorMessage);
            return false;
        }
        
        return true;
    }

    //protected void SetDataSource()
    protected void SetDataSource(string projcode)
    {
        StringBuilder sb = new StringBuilder();
        gvData.HiddenFields += "ID,INID";
        if (projcode == Consts.DropDownAllValue)
        {

            if (SessionInfo.IsSupervisor)
            {
                sb.AppendFormat("select 0 as INID,ID,LOGINNAME,FULLNAME from SC_User where id <> {0} ",SessionInfo.UserId);
            }
            else
            {
                sb.AppendFormat("select 0 as INID,ID,LOGINNAME,FULLNAME from SC_User where id <> {0} ", SessionInfo.UserId);
                //sb.AppendFormat("select ID,LOGINNAME,FULLNAME from CPS_View_GroupUserbyUser where id = {0}", SessionInfo.UserId);
            }
        }
        else
        {
            PCCore.Common.HRLog.RecordLog("Project Code," + projcode);

            sb.AppendFormat("select 0 as INID,up.userid as ID, u.loginname, u.fullname from CPS_View_UserProject up inner join SC_User u on u.id = up.userid where up.prjcode = '{0}'", projcode);
            
            
        }
        sb.Append(" order by loginname");

        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this.Master;
        gvData.HeaderDescriptions = Resources.Labels.InGroup + ",ID," + Resources.Labels.LoginName + "," + Resources.Labels.FullName;

    }


    protected void RadioButtonList1_SelectedIndexChanged(object sender, EventArgs e)
    {
        this.tblUser.Visible = (rbMth.SelectedIndex== 0);
        this.tblProject.Visible = (rbMth.SelectedIndex == 0);
        this.ddlFunction.Enabled = (rbMth.SelectedIndex == 2);
            
        
    }

    protected void Regscript()
    {
        //string cmd = "select prjCode,U_PrjFname from v_Project";
        string cmd = PCCore.Database.ValidationList.project_sql;
        //DataRowCollection rows = PCDb.Db.ExecQuery(cmd).Rows;
        DataRowCollection rows = SAPDb.Db.ExecQuery(cmd).Rows;
        string CreateCmd = "<script>" + "\n\n function checkAll(){ " +
            "\n if(cbTotal.checked){ \n";
        for (int i = 0; i < rows.Count; i++)
        {
            CreateCmd += "\ndocument.getElementById('ctl00_ContentPlaceHolder_cblProject_" + i.ToString() + "').checked=true;\n";

        }
        //CreateCmd += "txtCount.value=" + rows.Count;
        CreateCmd += "\n}else{\n";
        for (int i = 0; i < rows.Count; i++)
        {
            CreateCmd += "\ndocument.getElementById('ctl00_ContentPlaceHolder_cblProject_" + i.ToString() + "').checked=false;\n";
        }
        //CreateCmd += "txtCount.value=0";
        CreateCmd += "\n}}\n </script>";
        ClientScript.RegisterStartupScript(Page.GetType(), "function", CreateCmd);

        cbTotal.Attributes["onclick"] = "javascript:checkAll();";
        for (int i = 0; i < cblProject.Items.Count; i++)
        {
            cblProject.Items[i].Attributes["onclick"] = "javascript:checkstatus(ctl00_ContentPlaceHolder_cblProject_" + i.ToString() + ");";
        }
    }
        
    void SaveByUser()
    {
        string msg = String.Empty;
        decimal id = 0;
        ArrayList _alUser = new ArrayList();
        if (Master.FormMode == MasterBasePage.FormModes.New)
            id = PCDb.GetNextID(Consts.KeyAnnouncementID);

        // Read Selected User
        ICollection ic = gvData.SelectedRowValuesCollection;
        PCCore.Common.HRLog.RecordLog("Selected User Count:" + ic.Count.ToString());
        IEnumerator ie = ic.GetEnumerator();
        ie.Reset();
        while (ie.MoveNext())
        {
            PCCore.Common.HRLog.RecordLog("SC_Announcement User ID: " + ie.Current.ToString());
            _alUser.Add(ie.Current.ToString());
        }

        msg = PCCore.Database.SC_Announcements.SaveByProject(getSCValues(), _alUser, id, ddlProject.SelectedValue);
        if (msg == String.Empty)
        {
            Master.ShowMessage(String.Format(Resources.Common.SaveOK, txtAnnouncementTitle.Text));
            Master.ClearForm();
        }
        else
        {
            Master.ShowMessage(msg);
            Master.ClearForm();
        }
        
    }
    void SaveGlobally()
    {
        decimal id;
        id = 0;
        string msg = "";
        if (Master.FormMode == MasterBasePage.FormModes.New)
            id = PCDb.GetNextID(Consts.KeyAnnouncementID);

        // Read Selected User
        msg = PCCore.Database.SC_Announcements.SaveSystemMessage(getSCValues(), id);
        if (msg == String.Empty)
        {
            Master.ShowMessage(String.Format(Resources.Common.SaveOK, txtAnnouncementTitle.Text));
            Master.ClearForm();
        }
        else
        {
            Master.ShowMessage(msg);
            Master.ClearForm();
        }

    }

    void SaveByFunction()
    {
        decimal id;
        id = 0;
        string msg = "";
        if (Master.FormMode == MasterBasePage.FormModes.New)
            id = PCDb.GetNextID(Consts.KeyAnnouncementID);

        
        msg = PCCore.Database.SC_Announcements.SaveByFunction(getSCValues(), id);
        if (msg == String.Empty)
        {
            Master.ShowMessage(String.Format(Resources.Common.SaveOK, txtAnnouncementTitle.Text));
            Master.ClearForm();
        }
        else
        {
            Master.ShowMessage(msg);
            Master.ClearForm();
        }

    }

    Hashtable getSCValues()
    {
        Hashtable row = PCCore.Database.SC_Announcements.GetHeaderHashTable();
        row["TITLE"] = txtAnnouncementTitle.Text.Trim();
        row["BODY"] = txtBody.Text.Trim();
        row["EFFECTIVEDATE"] = txtEffectiveDate.Text;
        if (!String.IsNullOrEmpty(txtExpiryDate.Text))
        {
            row["EXPIRYDATE"] = txtExpiryDate.Text;
        }
        else
        {
            row["EXPIRYDATE"] = DBNull.Value;
        }
        switch (rbMth.SelectedIndex)
        {
            case 0:
                break;
            case 1:
                break;
            case 2:
                row["NOTICETYPE"] = ddlFunction.SelectedValue;
                break;
        }
        
        return row;
    }
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        SetDataSource(ddlProject.SelectedValue);
        
    }
}//end of class
