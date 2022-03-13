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

public partial class ScGroup_Form : BasePage
{
    protected PCTable _table = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;
    protected ListItem li;

    protected PCTable Table
    {
        get
        {
            if (_table == null) _table = new PCTable(Consts.TableScGroup, this.SecurityInfo);
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
        dsGridView.AutoRestoreSelectCommand = false;
        Master.Save += new MasterSecurityForm.SaveEventHandler(Master_Save);
        

        Master.Title = Resources.Labels.ScGroup;
        ((Button)Master.FindControl("btnRequest")).Visible = false;
        if (Page.IsPostBack)
        {
        }
        else
        {
            SetCheckBoxListItems(cblProject);
            switch (Master.FormMode)
            {
                case MasterSecurityForm.FormModes.New:
                    break;

                case MasterSecurityForm.FormModes.Edit:
                    txtID.Text = Master.RecordID;
                    // disable code and name
                    txtCode.Enabled = false;
                    txtDescription.Enabled = false;
                    
                    _dr = this.Table.GetRow(Master.RecordID);
                    if (_dr != null)
                    {
                        Master.Record = _dr;
                        Page.DataBind();
                    }
                    else
                    {
                        Master.ShowWarning(String.Format(Resources.Common.InvalidIDOrMultipleRecord, Master.RecordID));
                    }
                    break;
            }
        }

        if (_dr != null)
        {
            SetCheckBoxValue(_dr);
        }

        gvData.HeaderDescriptions = Resources.Labels.InGroup + ",GID,ID," + Resources.Labels.LoginName + "," + Resources.Labels.FullName;
        if (this.CurrentCommand!=Consts.ButtonExport) SetDataSource();

        Regscript();
    }

    Hashtable GetRow()
    {
        Hashtable row = new Hashtable();
        if (Master.FormMode == MasterSecurityForm.FormModes.New)
            row.Add(Consts.FieldID, PCDb.GetNextID());
        else
            row.Add(Consts.FieldID, Convert.ToDecimal(txtID.Text));
        row.Add(Consts.FieldCode, txtCode.Text.Trim());
        row.Add(Consts.FieldName, txtDescription.Text.Trim());

        txtProjectname.Text = "";
        for (int i = 0; i < cblProject.Items.Count; i++)
        {
            string id = cblProject.Items[i].Value + ",";

            if (cblProject.Items[i].Selected)
            {
                txtProjectname.Text += id;
            }
        }
        if (!string.IsNullOrEmpty(txtProjectname.Text))
        {
            txtProjectname.Text = txtProjectname.Text.Substring(0, txtProjectname.Text.Length - 1);
            row.Add("ACCESSPROJECTID", txtProjectname.Text);
        }
        else
            row.Add("ACCESSPROJECTID", DBNull.Value);

        return row;
    }

    protected void SetCheckBoxValue(DataRow drRow)
    {
        if (!string.IsNullOrEmpty(drRow["ACCESSPROJECTID"].ToString()) && !Convert.IsDBNull(drRow["ACCESSPROJECTID"].ToString()))
        {
            string[] Projectname = SplitProjectName(drRow["ACCESSPROJECTID"].ToString());
            if (Projectname.Length > 0)
            {
                for (int i = 0; i < Projectname.Length; i++)
                {
                    if (cblProject.Items.FindByValue(Projectname[i]) != null)
                        cblProject.Items.FindByValue(Projectname[i].ToString()).Selected = true;
                }
                txtCount.Text = Projectname.Length.ToString();
                if (Projectname.Length == cblProject.Items.Count)
                    cbTotal.Checked = true;
            }
            txtProjectname.Text = drRow["ACCESSPROJECTID"].ToString();
        }
    }
    void Master_Save(object sender, EventArgs e)
    {
        ArrayList _alUserid;
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
                    
                    SaveGroupUser(row);
                    // Save Group User Project
                    SaveUserProject(row);
                    
                    Master.ShowMessage(String.Format(Resources.Common.SaveOK, txtCode.Text));
                    Master.ClearForm();
                    break;

                case MasterSecurityForm.FormModes.Edit:
                    this.Table.BeginTransaction();
                    this.Table.Update(row);
                    this.Table.CommitTransaction();
                    if (row.Count > 0)
                        SaveGroupUser(row);
                    // Save Group User Project
                    SaveUserProject(row);
                    Master.ShowMessage(String.Format(Resources.Common.UpdateOK, txtCode.Text));
                    break;
            }
        }
        catch (Exception ex)
        {
            Master.HandleError(ex);
            this.Table.RollbackTransaction();
        }
    }
    void SaveUserProject(Hashtable groupRow)
    {
        ArrayList _alProject = new ArrayList();
        ArrayList _alUser = new ArrayList();
        //for (int i = 0; i < cblProject.Items.Count; i++)
        //{
        //    if (cblProject.Items[i].Selected)
        //        _alProject.Add(cblProject.Items[i].Value);
            
        //}
        _alProject.Add(txtCode.Text);
        ICollection ic = gvData.SelectedRowValuesCollection;
        IEnumerator ie = ic.GetEnumerator();
        ie.Reset();
        while (ie.MoveNext())
        {
            // Update Project to SC_User
            PCCore.Database.User u = new PCCore.Database.User(ie.Current.ToString());
            u.UpdateUserProject(txtCode.Text);
            _alUser.Add(ie.Current.ToString());
        }
        // Update Project to SC_User

        PCCore.Database.UserProject.SaveByGroup(_alProject, _alUser, groupRow[Consts.FieldID].ToString());
        
    }
    //ArrayList getSelectedGroup()
    //{
    //    ArrayList _al = new ArrayList();
    //    foreach (DataListItem dli in lsGroup.Items)
    //    {
    //        try
    //        {
    //            CheckBox _chk = (CheckBox)dli.FindControl("chkGroup");
    //            PCCore.Label lblid = (PCCore.Label)dli.FindControl("lblID");
    //            if (_chk.Checked)
    //            {
    //                _al.Add(lblid.Text);
    //            }
    //        }
    //        catch (Exception ex)
    //        {

    //        }
    //    }

    //    return _al;
    //}


    void SaveGroupUser(Hashtable groupRow)
    {
        string gID = groupRow[Consts.FieldID].ToString();
        PCTable gu = new PCTable(Consts.TableScGroupUser, this.SecurityInfo);

        try
        {
            gu.BeginTransaction();
            Hashtable row = new Hashtable();

            string where = String.Format("groupid={0}", gID);
            gu.Delete(where);

            ICollection ic = gvData.SelectedRowValuesCollection;
            IEnumerator ie = ic.GetEnumerator();
            ie.Reset();
            while (ie.MoveNext())
            {
                row.Clear();
                row.Add("GROUPID", gID);
                row.Add("USERID", ie.Current);
                gu.Insert(row);
            }
            gu.CommitTransaction();
        }
        catch (Exception ex)
        {
            gu.RollbackTransaction();
            gvData.ClearSelectedRowValues = false;
            throw ex;
        }
    }

    bool CheckForm()
    {
        if (String.IsNullOrEmpty(txtCode.Text))
        {
            Master.ShowMessage(txtCode.RequiredErrorMessage);
            return false;
        }
        if (String.IsNullOrEmpty(txtDescription.Text))
        {
            Master.ShowMessage(txtDescription.RequiredErrorMessage);
            return false;
        }
        return true;
    }

    protected void SetDataSource()
    {
        StringBuilder sb = new StringBuilder();

        switch (Master.FormMode)
        {
            case MasterSecurityForm.FormModes.New:
                sb.AppendFormat("select INGROUP,GROUPID,ID,LOGINNAME,FULLNAME from v_sc_newgroupuser");                

                break;
            case MasterSecurityForm.FormModes.Edit:
                sb.AppendFormat("select INGROUP,GROUPID,ID,LOGINNAME,FULLNAME from v_sc_editgroupuser where 1=1 and (groupid={0} or groupid=0)", Master.RecordID);                
                break;
            default:
                return;
        }
        sb.Append(" order by loginname");

        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this.Master;
        gvData.HeaderDescriptions = Resources.Labels.InGroup + ",GID,ID," + Resources.Labels.LoginName + "," + Resources.Labels.FullName;
       
        
    }

    protected void gvData_RowCreated(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.Header:
            case DataControlRowType.DataRow:
                e.Row.Cells[1].Style.Add("display", "none");//INGROUP
                e.Row.Cells[2].Style.Add("display", "none");//GROUPID
                break;
        }
    }

    protected void SetCheckBoxListItems(CheckBoxList cbl)
    {
        //string cmd = "select prjCode,U_PrjFName from v_Project";
        string cmd = PCCore.Database.ValidationList.project_sql;
        //DataRowCollection rows = PCDb.Db.ExecQuery(cmd).Rows;
        DataRowCollection rows = SAPDb.Db.ExecQuery(cmd).Rows;
        cbl.Items.Clear();
        if (rows.Count > 0)
        {
            for (int i = 0; i < rows.Count; i++)
            {
                li = new ListItem(rows[i][1].ToString(), rows[i][0].ToString());
                cbl.Items.Add(li);
            }
            txtTotal.Text = rows.Count.ToString();
        }

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
        CreateCmd += "txtCount.value=" + rows.Count;
        CreateCmd += "\n}else{\n";
        for (int i = 0; i < rows.Count; i++)
        {
            CreateCmd += "\ndocument.getElementById('ctl00_ContentPlaceHolder_cblProject_" + i.ToString() + "').checked=false;\n";
        }
        CreateCmd += "txtCount.value=0";
        CreateCmd += "\n}}\n </script>";
        ClientScript.RegisterStartupScript(Page.GetType(), "function", CreateCmd);

        cbTotal.Attributes["onclick"] = "javascript:checkAll();";
        for (int i = 0; i < cblProject.Items.Count; i++)
        {
            cblProject.Items[i].Attributes["onclick"] = "javascript:checkstatus(ctl00_ContentPlaceHolder_cblProject_" + i.ToString() + ");";
        }
    }

    protected string[] SplitProjectName(string Projectname)
    {
        if (string.IsNullOrEmpty(Projectname)) return null;
        char[] separate = new char[] { ',' };
        string[] result;
        result = Projectname.Split(separate, StringSplitOptions.RemoveEmptyEntries);
        return result;
    }
}//end of class
