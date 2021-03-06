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

public partial class ScUser_Form : BasePage
{
    protected PCTable _table = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;
    protected ListItem li;
    const string VIEW_SCUSER = "V_SC_USERS";
    protected Sc.PublishTypes scpType = Sc.PublishTypes.Public;


    protected override void OnInit(EventArgs e)
    {
        base.ShowWebMenu = false;
        base.OnInit(e);
        if (Master.FormMode == MasterSecurityForm.FormModes.Edit)
            Page.ClientScript.RegisterStartupScript(Page.GetType(), "initpwd", String.Format("document.getElementById('{0}').value='********';", txtPassword.ClientID), true);

    }

    protected PCTable View
    {
        get
        {
            if (_table == null) _table = new PCTable(VIEW_SCUSER, this.SecurityInfo);
            return _table;
        }
    }

    protected PCTable Table
    {
        get
        {
            if (_table == null) _table = new PCTable(Consts.TableScUser, this.SecurityInfo);
            return _table;
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        Master.Save += new MasterSecurityForm.SaveEventHandler(Master_Save);

        Master.Title = Resources.Labels.ScUser;
        ((Button)Master.FindControl("btnRequest")).Visible = false;


        if (Page.IsPostBack)
        {
        }
        else
        {
            PCCore.Database.ValidationList.SourceUserList(ddlsuser,Convert.ToInt32(Master.RecordID));
            ddlsuser.Enabled = false;
            //ddlsuser.Items.Insert(0,new ListItem(Resources.Labels.All,Convert.ToString(Consts.DropDownOptionNone)));
            SetCheckBoxListItems(cblProject);
            switch (Master.FormMode)
            {
                case  MasterSecurityForm.FormModes.New:
                    int rowcount = View.SelectCount("1=1");
                    //if (rowcount >= Convert.ToInt16(License.UserNumber))
                    //{
                    //    //this.Master.ButtonSave.Enabled = false;
                    //}
                    break;

                case MasterSecurityForm.FormModes.Edit:
                    txtID.Text = Master.RecordID;
                    _dr = this.View.GetRow(Master.RecordID);
                    if (_dr != null)
                    {

                        Master.Record = _dr;
                        Page.DataBind();
                        // Enable or Disable 
                        if (_dr["Supervisor"].ToString() == "1")
                        {
                            chkcopyuser.Enabled = false;
                            ddlsuser.Enabled = false;
                        }
                        else
                        {
                            chkcopyuser.Enabled = true;
                            ddlsuser.Enabled = true;
                        }
                    }
                    else
                    {
                        Master.ShowWarning(String.Format(Resources.Common.InvalidIDOrMultipleRecord, Master.RecordID));
                    }
                    break;
            }
            PCCore.Common.HRLog.RecordLog("SETDATASOURCE");
            if (this.CurrentCommand != Consts.ButtonExport) SetDataSource();
        }

        if (_dr != null)
        {
            SetCheckBoxValue(_dr);
        }
        SystemParameter sp = SystemParameter.Get(PCDb.Db);
        txtMinPasswordLength.Text = sp.MinPasswordLength.ToString();
        
        SetTextBoxClientId();
        Regscript();
        // set attribute
               
    }

    Hashtable GetRow()
    {
        Hashtable row = new Hashtable();
        for (int i = 0; i < this.Table.Fields.Count; i++)
        {
            PCCore.Common.HRLog.RecordLog(this.Table.Fields[i].BaseFieldName.ToString());
        }
        row.Add("sapuser", this.txtsapuser.Text);
        
        PCCore.Common.HRLog.RecordLog("GetRow: " + row["sapuser"].ToString());
        if (Master.FormMode == MasterSecurityForm.FormModes.New)
        {
            //row.Add(Consts.FieldID, PCDb.GetNextID());
            row.Add(Consts.FieldID, PCDb.getNextUserSign());
        }
        else
        {
            row.Add(Consts.FieldID, Convert.ToDecimal(txtID.Text));
        }
        // New Password
        PCCore.Common.Security _s = new PCCore.Common.Security();
        
        PCCore.Common.HRLog.RecordLog("Encrypted Password:" + _s.Encrypt(txtPassword.Text));
        if (txtPassword.Text.Trim() != "********")
        {
            row.Add("PASSWD2", _s.Encrypt(txtPassword.Text));
            row.Add("PASSWD", Sc.Encrypt(txtPassword.Text.Trim()));
        }
        
        row.Add("LOGINNAME", txtLoginName.Text.Trim());
        row.Add("FULLNAME", txtFullName.Text.Trim());
        row.Add("EMAIL", txtEmail.Text.Trim());
        if (chkLocked.Checked)
            row.Add("LOCKED", Convert.ToDecimal("1"));
        else
            row.Add("LOCKED", Convert.ToDecimal("0"));
        if (!string.IsNullOrEmpty(txtExpireDate.Text))
        {
            row.Add("EXPIREDATE", Convert.ToDateTime(txtExpireDate.Text));
        }
        else
        {
            row.Add("EXPIREDATE", DBNull.Value);
        }

        //Martin update begin
        if (chkReverse.Checked)
            row.Add("REVERSE", Convert.ToDecimal("1"));
        else
            row.Add("REVERSE", Convert.ToDecimal("0"));
        //Martin update end

        if (chkSupervisor.Checked)
            row.Add("SUPERVISOR", Convert.ToDecimal("1"));
        else
            row.Add("SUPERVISOR", Convert.ToDecimal("0"));

        // Ken update begin
        if (chkBatch.Checked)
            row.Add("BATCHUPLOAD", Convert.ToDecimal("1"));
        else
            row.Add("BATCHUPLOAD", Convert.ToDecimal("0"));
        // Ken update end

        // Ken update begin, 20190123
        row.Add("FLEXUSER", txtflexuser.Text.Trim());
        // Ken update end, 20190123

        txtProjectname.Text = "";
        for (int i = 0; i < cblProject.Items.Count; i++)
        {
            string id = cblProject.Items[i].Value + ",";

            if (cblProject.Items[i].Selected)
            {
                txtProjectname.Text += id;
            }
        }
        PCCore.Common.HRLog.RecordLog("txtProject Length: " + txtProjectname.Text.ToString().Length.ToString());
        PCCore.Common.HRLog.RecordLog("txtProject: " + txtProjectname.Text.ToString());
        if (!string.IsNullOrEmpty(txtProjectname.Text))
        {
            txtProjectname.Text = txtProjectname.Text.Substring(0, txtProjectname.Text.Length - 1);
            //row.Add("ACCESSPROJECTID", txtProjectname.Text);
            //row.Add("ACCESSPROJECTID", DBNull.Value);
        }
        else
            row.Add("ACCESSPROJECTID", DBNull.Value);

        row.Add("PROJECTCODE", string.Empty);

        return row;
    }

    void Master_Save(object sender, EventArgs e)
    {
        PCCore.Common.HRLog.RecordLog("GridView Row Count: " + gvData.Rows.Count);
        if (!CheckForm()) return;
        try
        {
            Hashtable row = GetRow();
            PCCore.Common.HRLog.RecordLog("sapuser:" + row["sapuser"].ToString());
            switch (Master.FormMode)
            {
                case MasterSecurityForm.FormModes.New:
                    this.Table.BeginTransaction();
                    this.Table.Insert(row);

                    this.Table.CommitTransaction();
                    //PCDb.Db.ExecUpdate(string.Format("UPDATE SC_USER SET SAPUSER = '{0}' WHERE ID = {1}", row["sapuser"].ToString(), row[Consts.FieldID]));
                    PCDb.Db.ExecUpdate(string.Format("UPDATE SC_USER SET SAPUSER = '{0}', FLEXUSER = '{1}' WHERE ID = {2}", row["sapuser"].ToString(), row["FLEXUSER"].ToString(), row[Consts.FieldID]));
                    SaveGroupUser(row);
                    SaveUserProject(row);
                    // Karrson: Copy User Rights
                    if (chkcopyuser.Checked)
                    {
                        try
                        {

                            PCDb.Db.ExecQuery(string.Format("exec CPS_SP_PCMSCopyUser {0},{1},{2}", ddlsuser.SelectedValue, row[Consts.FieldID], SessionInfo.UserId));
                        }
                        catch (Exception ex)
                        {
                            PCCore.Common.HRLog.RecordException("Copy User", ex);
                        }
                    }
                    Master.ShowMessage(String.Format(Resources.Common.SaveOK, txtLoginName.Text));
                    //Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{self.close();}</script>");
                    Master.ClearForm();
                    break;

                case MasterSecurityForm.FormModes.Edit:
                    this.Table.BeginTransaction();
                    PCCore.Common.HRLog.RecordLog("Sql:" + this.Table.InternalCommand.CommandText);
                    
                    
                    this.Table.Update(row);
                    
                    if (txtID.Text == SessionInfo.UserId)
                    {
                        SessionInfo.LoginName = txtLoginName.Text;                       
                        SessionInfo.IsSupervisor = chkSupervisor.Checked;
                    }
                    
                    this.Table.CommitTransaction();
                    //PCDb.Db.ExecUpdate(string.Format("UPDATE SC_USER SET SAPUSER = '{0}' WHERE ID = {1}", row["sapuser"].ToString(), row[Consts.FieldID]));
                    PCDb.Db.ExecUpdate(string.Format("UPDATE SC_USER SET SAPUSER = '{0}', FLEXUSER = '{1}' WHERE ID = {2}", row["sapuser"].ToString(), row["FLEXUSER"].ToString(), row[Consts.FieldID]));
                    SaveGroupUser(row);
                    SaveUserProject(row);
                    // Karrson: Copy User Rights
                    if (chkcopyuser.Checked)
                    {
                        try
                        {
                            
                            PCDb.Db.ExecQuery(string.Format("exec CPS_SP_PCMSCopyUser {0},{1},{2}", ddlsuser.SelectedValue,row[Consts.FieldID],SessionInfo.UserId));
                        }
                        catch (Exception ex)
                        {
                            PCCore.Common.HRLog.RecordException("Copy User", ex);
                        }
                    }
                    Master.ShowMessage(String.Format(Resources.Common.UpdateOK, txtLoginName.Text));
                    //Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{self.close();}</script>");
                    break;
            }
        }
        catch (Exception ex)
        {
            Master.HandleError(ex);
            PCCore.Common.HRLog.RecordException("Insert/Update User:",ex);
            this.Table.RollbackTransaction();
        }

        
    }
    // Save Project to UserProject Table
    void SaveUserProject(Hashtable userRow)
    {
        PCCore.Common.HRLog.RecordLog("Grid Row Count: " + gvData.Rows.Count);
        //Hashtable _ht = new Hashtable();
        // Project ArrayList
        ArrayList _al = new ArrayList();
        //for (int i = 0; i < cblProject.Items.Count; i++)
        //{
        //    if (cblProject.Items[i].Selected)
        //    {
            
        //        _al.Add(cblProject.Items[i].Value);
        //    }
            
        //}
        ArrayList _alGroup = new ArrayList();
        
        //PCCore.Common.HRLog.RecordLog("Selected Row Value: " + gvData.SelectedRowValues);
        //ICollection ic = gvData.SelectedRowValuesCollection;
        //IEnumerator ie = ic.GetEnumerator();
        //ie.Reset();


        //while (ie.MoveNext())
        //{
        //    PCCore.Common.HRLog.RecordLog("Group:" + ie.Current.ToString());
        //    _alGroup.Add(ie.Current.ToString());

        //}
        _alGroup = getSelectedGroup();
        // Project = Group
        _al = _alGroup;
        PCCore.Common.HRLog.RecordLog("Group Count: " + _alGroup.Count);
        PCCore.Common.HRLog.RecordLog("Project Count: " + _al.Count);
        //PCCore.Database.UserProject.Delete(userRow[Consts.FieldID].ToString(), "u");
        PCCore.Database.UserProject.SaveByUser(_al,_alGroup, userRow[Consts.FieldID].ToString());
    }
    
    void SaveGroupUser(Hashtable userRow)
    {
        ArrayList _algroup = getSelectedGroup();
        string uID = userRow[Consts.FieldID].ToString();
        scpType = this.SecurityInfo.PublishType;
        this.SecurityInfo.PublishType = Sc.PublishTypes.Public;
        PCTable gu = new PCTable(Consts.TableScGroupUser, this.SecurityInfo);
        try
        {
            gu.BeginTransaction();
            Hashtable row = new Hashtable();
            PCCore.Common.HRLog.RecordLog("UserID: " + uID);
            string where = String.Format("userid={0}", uID);
            gu.Delete(where);
            string groupValue = "";

            //foreach (DataListItem dli in lsGroup.Items)
            //{
            //    try
            //    {
            //        CheckBox _chk = (CheckBox)dli.FindControl("chkGroup");
            //        if (_chk.Checked)
            //        {
                        
            //            PCCore.Label _lbl = (PCCore.Label)dli.FindControl("lblID");
            //            PCCore.Common.HRLog.RecordLog("_lbl," + _lbl.Text);
            //            _algroup.Add(_lbl.Text.ToString());
            //            groupValue = _lbl.Text.ToString();
            //            break;
            //        }
            //    }
            //    catch (Exception ex)
            //    {
            //        PCCore.Common.HRLog.RecordException("Find Label", ex);
            //    }
            //}
            
            //ICollection ic = gvData.SelectedRowValuesCollection;
            //IEnumerator ie = ic.GetEnumerator();
            //ie.Reset();
            //while (ie.MoveNext())
            foreach (object _o in _algroup.ToArray())
            {
                row.Clear();
                
                //PCCore.Common.HRLog.RecordLog("groupValue," + groupValue);
                PCCore.Common.HRLog.RecordLog("Group ID:" + _o.ToString());
                row.Add("GROUPID", Convert.ToDecimal(_o.ToString()));
                row.Add("USERID", uID);
                row.Add("PROJECTCODE", string.Empty);
                gu.Insert(row);
            
                
            }
            gu.CommitTransaction();
        }
        catch (Exception ex)
        {
            PCCore.Common.HRLog.RecordException("Save", ex);
            gu.RollbackTransaction();
            gvData.ClearSelectedRowValues = false;
            throw ex;
        }
        finally
        {
            this.SecurityInfo.PublishType = scpType;
        }
    }

    bool CheckForm()
    {
        //if (String.IsNullOrEmpty(StaffSearch1.FullName))
        //{
        //    Master.ShowMessage(txtFullName1.RequiredErrorMessage);
        //    return false;
        //}
        if (String.IsNullOrEmpty(txtLoginName.Text))
        {
            Master.ShowWarning(txtLoginName.RequiredErrorMessage);
            return false;
        }
        if (String.IsNullOrEmpty(txtPassword.Text))
        {
            Master.ShowWarning(txtPassword.RequiredErrorMessage);
            return false;
        }

        else
        {
            SystemParameter sp=SystemParameter.Get(PCDb.Db);
            if (txtPassword.Text.Trim().Length < sp.MinPasswordLength)
            {
                Master.ShowWarning(Resources.Messages.MinPasswordLength + " " + sp.MinPasswordLength.ToString());
            }
        }

        if (!string.IsNullOrEmpty(txtExpireDate.Text) && !SimpleControls.SimpleRegex.IsDateYMD(txtExpireDate.Text))
        {
            Master.ShowWarning(Resources.Messages.InputDate);
            return false;
        }
        if (Master.FormMode == MasterBasePage.FormModes.New)
        {
            if (PCCore.Database.User.isExists(txtLoginName.Text))
            {
                Master.ShowWarning(GetGlobalResourceObject(Consts.ResourcesMessages, "DuplicateLoginName").ToString());
                return false;
            } 

        }
        if (Master.FormMode == MasterBasePage.FormModes.Edit)
        {
            if (PCCore.Database.User.isExists(txtLoginName.Text, txtID.Text))
            {
                Master.ShowWarning(GetGlobalResourceObject(Consts.ResourcesMessages, "DuplicateLoginName").ToString());
                return false;
            } 
        }
        return true;
       
    }

    protected void SetDataSource()
    {
        StringBuilder sb = new StringBuilder();

        switch (Master.FormMode)
        {
            case MasterSecurityForm.FormModes.New:
                //sb.AppendFormat("select id,ingroup,groupcode,groupname,groupcode + '     - '  + groupname as group_1 from v_sc_newusergroup");
                sb.AppendFormat("select groupid as id,ingroup,groupcode, groupname, groupcode + '    - ' + groupname as group_1 from CPS_GetAllUserProject(0,{0})", "'N'");

                break;
            case MasterSecurityForm.FormModes.Edit:
                //sb.AppendFormat("select groupid as id,ingroup,groupcode, groupname, groupcode + '    - ' + groupname as group_1 from v_sc_editusergroup where id={0}", Master.RecordID);
                sb.AppendFormat("select groupid as id,ingroup,groupcode, groupname, groupcode + '    - ' + groupname as group_1 from CPS_GetAllUserProject({0},{1})", Master.RecordID, "'E'");
                break;
            default:
                return;
        }
        sb.Append(" order by groupcode");

        PCCore.Common.HRLog.RecordLog(sb.ToString());
        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = sb.ToString();
        dsGridView.ErrorHandler = this.Master;

        gvData.HiddenFields += "INGROUP";
        // gvData.HeaderDescriptions = "ID,In Group,Group Code,Group Name";
        gvData.HeaderDescriptions = "ID," + Resources.Labels.InGroup + "," + Resources.Labels.GroupCode + "," + Resources.Labels.GroupName;
    
        
    }

    protected void SetTextBoxClientId()
    {
        string lgId = "txtUid='" + txtID.ClientID.ToString() + "';";
        lgId += "txtLName='" + txtLoginName.ClientID.ToString() + "';";
        lgId += "txtPwd='" + txtPassword.ClientID.ToString() + "';";

        Page.ClientScript.RegisterStartupScript(Page.GetType(), "setClientId", "<script language='javascript'>" + lgId + "</script>");
    }

    protected void SetCheckBoxValue(DataRow drRow)
    {
        if (drRow["LOCKED"].ToString() == "1")
            chkLocked.Checked = true;
        else
            chkLocked.Checked = false;

        if (drRow["REVERSE"].ToString() == "1")
            chkReverse.Checked = true;
        else
            chkReverse.Checked = false;

        if (drRow["SUPERVISOR"].ToString() == "1")
            chkSupervisor.Checked = true;
        else
            chkSupervisor.Checked = false;
        if (drRow["EXPIREDATE"] != DBNull.Value)
        {
            txtExpireDate.Text = Convert.ToDateTime(drRow["EXPIREDATE"]).ToString(Consts.DateFormat);
        }

        if (drRow["BATCHUPLOAD"].ToString() == "1")
            chkBatch.Checked = true;
        else
            chkBatch.Checked = false;

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

    protected void SetCheckBoxListItems(CheckBoxList cbl)
    {
        //string cmd = "select prjCode,U_PrjFName from v_Project";
        //DataRowCollection rows = PCDb.Db.ExecQuery(cmd).Rows;
        string cmd = PCCore.Database.ValidationList.project_sql;
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
        cbTotal.Attributes["onclick"] = "javascript:checkAll();";
        ClientScript.RegisterStartupScript(Page.GetType(), "function", CreateCmd);
        

    }

    protected string[] SplitProjectName(string Projectname)
    {
        if (string.IsNullOrEmpty(Projectname)) return null;
        char[] separate = new char[] { ',' };
        string[] result;
        result = Projectname.Split(separate, StringSplitOptions.RemoveEmptyEntries);
        return result;
    }

    protected void gvData_SelectedIndexChanged(object sender, EventArgs e)
    {
        PCCore.Common.HRLog.RecordLog("gvData_SelectedIndexChanged");
        int selectedkey = gvData.SelectedIndex;

        if (selectedkey >= 0)
        {
            int cnt = gvData.Rows[selectedkey].Cells[0].Controls.Count;
            for (int i = 0; i < cnt; i++)
            {
                PCCore.Common.HRLog.RecordLog(gvData.Rows[selectedkey].Cells[0].Controls[i].UniqueID);
            }
        }
    }

    protected void lsGroup_ItemDataBound(Object sender, DataListItemEventArgs e)
    {
        PCCore.Label _lblingroup;
        CheckBox _chk;
        try
        {
            _lblingroup = (PCCore.Label)e.Item.FindControl("lblingroup");
            _chk = (CheckBox)e.Item.FindControl("chkGroup");
            if (_lblingroup.Text == "1")
            {
                _chk.Checked = true;
            }
        }
        catch (Exception ex)
        {

        }
    }

    ArrayList getSelectedGroup()
    {
        ArrayList _al = new ArrayList();
        foreach (DataListItem dli in lsGroup.Items)
        {
            try
            {
                CheckBox _chk = (CheckBox)dli.FindControl("chkGroup");
                PCCore.Label lblid = (PCCore.Label)dli.FindControl("lblID");
                if (_chk.Checked)
                {
                    _al.Add(lblid.Text);
                }
            }
            catch (Exception ex)
            {

            }
        }    
        
        return _al;
    }


    protected void chkcopyuser_CheckedChanged(object sender, EventArgs e)
    {
        if (chkcopyuser.Checked)
            ddlsuser.Enabled = true;
        else
            ddlsuser.Enabled = false;
    }
    protected void chkSupervisor_CheckedChanged(object sender, EventArgs e)
    {
        if (chkSupervisor.Checked)
        {
            chkcopyuser.Enabled = false;
            ddlsuser.Enabled = false;
        }
        else
        {
            chkcopyuser.Enabled = true;
            ddlsuser.Enabled = true;
        }
       
    }
}//end of class
