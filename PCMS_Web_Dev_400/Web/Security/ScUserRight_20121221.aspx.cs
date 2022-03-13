using System;
using System.Data;
using System.Configuration;
using System.Collections;
using System.Collections.Generic;
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

public partial class ScUserRight : BasePage
{
    const string TABLE_NAME = Consts.TableScRight;
    void ButtonPermission()
    {
        
        //__ibRefresh.Visible = true;
        //__ibSave.Visible = true;
    }
    protected void __ibRefresh_Click(object sender, EventArgs e)
    {
        
    }
    protected void __ibSave_Click(object sender, EventArgs e)
    {
        this.SaveCheckRecord();
        this.Page_Load(sender, e);
    }
    void ButtonProperty()
    {
        //string _imageBaseUrl = Config.GetImageBaseUrl(Page.Theme);
        //__ibSave.CausesValidation = true;
        //__ibSave.ImageUrl = _imageBaseUrl + "/save.gif";
        //__ibSave.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Save").ToString();
        //__ibSave.CssClass = "ToolbarImage";
        ////__ibSave.OnClientClick = "if (typeof(Save)=='function'){if(!Save()) return false; }else{ return false;}";
        //__ibSave.Visible = true;

        //__ibRefresh.CausesValidation = false;

        //__ibRefresh.ImageUrl = _imageBaseUrl + "/refresh.gif";
        //__ibRefresh.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "Refresh").ToString();
        //__ibRefresh.CssClass = "ToolbarImage";
        ////__ibRefresh.OnClientClick = "if(!Refresh()) return false;";
        //__ibRefresh.Visible = true;
        //ButtonPermission();
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        ButtonProperty();
        string themeUrl = Config.GetThemeBaseUrl(Page.Theme);
        //img1.Src = themeUrl + "/activity.gif";
        Master.ClearError();
        
        if (Page.IsPostBack)
        {
            if (invProjCode.Text != String.Empty)
            {
                ddlProject.SelectedValue = invProjCode.Text;
            }
            string cmd = this.CurrentCommand;
            switch (cmd)
            {
                    
                case Consts.ButtonDelete:
                    break;

                case Consts.ButtonSave:
                    if (SaveCheckRecord() == true)
                    {
                        //  gvData.DataBind
                    }
                    else
                    {
                        gvData.ClearDirtyRows = false;
                    }
                    break;
            }
            if (cmd.ToString() != "")
            {
            }
            
                
            PCCore.Common.HRLog.RecordLog("ddlProject.SelectedValue: " + ddlProject.SelectedValue);
          
            
        }
        else
        {
            chkfunctype.Attributes.Add("onchecked", "Refresh()");
            OnInitDropDownList();
        }

        if (this.CurrentCommand != Consts.ButtonExport)
        {
            if (invProjCode.Text == String.Empty)
                SetDataSource();
        }
        SetToolBarButtonVisable();
        invProjCode.Text = String.Empty;

    }//end of Page_Load


    protected void OnInitDropDownList()
    {      
        PCDb.InitDropDownList(this.ddlUser, "v_sc_users", "ID", "Loginname", 0, null,"isNull(SUPERVISOR,0) = 0");
        
        if (ddlUser.Items.Count > 2)
        {
            if (!Master.SecurityInfo.IsSupervisor)
            {
                this.ddlUser.SelectedIndex = 1;
            }
        }
        PCCore.Database.ValidationList.UserProjectList(ddlProject, ddlUser.SelectedValue.ToString());
        if (Master.SecurityInfo.IsSupervisor)
        {
            //add All for Supervisor Set
            //PCDb.InitDropDownList(this.ddlModule, "sc_module", "ID", "Name", 1, null,null, " SHOWORDER ");
            PCDb.InitDropDownList(this.ddlModule, "sc_module", "ID", "ModuleValue", 1, null, string.Format(" not {0} in ({1}) ", Consts.FieldID, Consts.ModuleSecurityID), " SHOWORDER ");
        }
        else
        {
            //PCDb.InitDropDownList(this.ddlModule, "sc_module", "ID", "Name", 0, null, null, " SHOWORDER ");
            PCDb.InitDropDownList(this.ddlModule, "sc_module", "ID", "ModuleValue", 0, null, string.Format(" not {0} in ({1}) ", Consts.FieldID, Consts.ModuleSecurityID), " SHOWORDER ");
        }
           
        for (int i = 0; i < ddlModule.Items.Count; i++)
        {
            //ddlModule.Items[i].Text = (string)HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, ddlModule.Items[i].Text);
        }
        ListItem liall = new ListItem("-" + Resources.Labels.All + "-", "-1");
        ListItem li0 = new ListItem(Resources.Labels.AllRight, "0");
        ListItem li1 = new ListItem(Resources.Labels.QueryRight, "1");
        ListItem li2 = new ListItem(Resources.Labels.NewRight, "2");
        ListItem li3 = new ListItem(Resources.Labels.EditRight, "3");
        ListItem li4 = new ListItem(Resources.Labels.DeleteRight, "4");
        // Approve
        ListItem li5 = new ListItem(Resources.Labels.ApproveRight, "5");
        ListItem[] li = new ListItem[] { liall, li0, li1, li2, li3, li4,li5 };
        ddlSearch.Items.AddRange(li);


    }

    //Hashtable GetRow(string UID, string FUNCID, string FALL, string FQRY, string FNEW, string FEDT, string FDEL)
    Hashtable GetRow(string UID, string FUNCID, string FALL, string FQRY, string FNEW, string FEDT, string FDEL,string FAPR, string Project)
    {
        Hashtable row = new Hashtable();
        row.Add("SCTYPE", "u");
        row.Add("ID", UID);
        row.Add("FUNCID", FUNCID);
        row.Add("FALL", FALL);
        row.Add("FQRY", FQRY);
        row.Add("FNEW", FNEW);
        row.Add("FEDT", FEDT);
        row.Add("FDEL", FDEL);
        PCCore.Common.HRLog.RecordLog("Row Add: Approve," + FAPR);
        row.Add("FAPR", FAPR);
        row.Add("PROJCODE", Project);
        return row;
    }

    bool SaveCheckRecord()
    {
        PCCore.Common.HRLog.RecordLog("SaveCHeckRecord");
        PCTable table = new PCTable(TABLE_NAME, this.SecurityInfo);

        // get changed values 
        
        ICollection dirtyRows = gvData.DirtyRowsCollection;
        if ((dirtyRows.Count <= 0) || (gvData.RecordCount == 0))
            return false;
        IEnumerator ie = dirtyRows.GetEnumerator();
        Hashtable dirtyrow = new Hashtable();
        string Project = "";
        string sUID = "", sFUNCID = "", sFALL = "", sFQRY = "", sFNEW = "", sFEDT = "", sFDEL = "",sFAPR = "";
        ie.Reset();
        while (ie.MoveNext())
        {
            if(ie.Current.ToString()=="-1") 
                continue;
            dirtyrow["RowID"] = ie.Current;
            int i = Convert.ToInt32(dirtyrow["RowID"]);

            PCCore.Common.HRLog.RecordLog(gvData.Rows.Count);
            sUID = gvData.Rows[i].Cells[1].Text;
            sFUNCID = gvData.Rows[i].Cells[6].Text;

            Project = gvData.Rows[i].Cells[9].Text;

            SimpleImageCheckBox chkFALL;
            //chkFALL = gvData.Rows[i].Cells[8].FindControl("chkFALL") as SimpleImageCheckBox;
            chkFALL = gvData.Rows[i].Cells[10].FindControl("chkFALL") as SimpleImageCheckBox;
            //sFALL = chkFALL.Checked.ToString() == "True" ? "1" : "0";
            switch (chkFALL.Value)
            {
                case 1:// SimpleCheckBox.CheckBoxValues.Checked:
                    sFALL="1";
                    break;
                case 2://SimpleCheckBox.CheckBoxValues.Indeterminate:
                    sFALL="2";
                    break;
                case 0://SimpleCheckBox.CheckBoxValues.UnChecked:
                    sFALL="0";
                    break;
            }
            SimpleImageCheckBox chkFQRY;
            //chkFQRY = gvData.Rows[i].Cells[9].FindControl("chkFQRY") as SimpleImageCheckBox;
            chkFQRY = gvData.Rows[i].Cells[11].FindControl("chkFQRY") as SimpleImageCheckBox;
            //sFQRY = chkFQRY.Checked.ToString() == "True" ? "1" : "0";
            switch (chkFQRY.Value)
            {
                case 1://SimpleCheckBox.CheckBoxValues.Checked:
                    sFQRY = "1";
                    break;
                case 2://SimpleCheckBox.CheckBoxValues.Indeterminate:
                    sFQRY = "2";
                    break;
                case 0://SimpleCheckBox.CheckBoxValues.UnChecked:
                    sFQRY = "0";
                    break;
            }
            SimpleImageCheckBox chkFNEW;

            //chkFNEW = gvData.Rows[i].Cells[10].FindControl("chkFNEW") as SimpleImageCheckBox;
            chkFNEW = gvData.Rows[i].Cells[12].FindControl("chkFNEW") as SimpleImageCheckBox;
            //sFNEW = chkFNEW.Checked.ToString() == "True" ? "1" : "0";
            switch (chkFNEW.Value)
            {
                case 1://SimpleCheckBox.CheckBoxValues.Checked:
                    sFNEW = "1";
                    break;
                case 2://SimpleCheckBox.CheckBoxValues.Indeterminate:
                    sFNEW = "2";
                    break;
                case 0://SimpleCheckBox.CheckBoxValues.UnChecked:
                    sFNEW = "0";
                    break;
            }
            SimpleImageCheckBox chkFEDT;
            
            //chkFEDT = gvData.Rows[i].Cells[11].FindControl("chkFEDT") as SimpleImageCheckBox;
            chkFEDT = gvData.Rows[i].Cells[13].FindControl("chkFEDT") as SimpleImageCheckBox;
            //sFEDT = chkFEDT.Checked.ToString() == "True" ? "1" : "0";
            switch (chkFEDT.Value)
            {
                case 1://SimpleCheckBox.CheckBoxValues.Checked:
                    sFEDT = "1";
                    break;
                case 2://SimpleCheckBox.CheckBoxValues.Indeterminate:
                    sFEDT = "2";
                    break;
                case 0://SimpleCheckBox.CheckBoxValues.UnChecked:
                    sFEDT = "0";
                    break;
            }
            SimpleImageCheckBox chkFDEL;

            //chkFDEL = gvData.Rows[i].Cells[12].FindControl("chkFDEL") as SimpleImageCheckBox;
            chkFDEL = gvData.Rows[i].Cells[14].FindControl("chkFDEL") as SimpleImageCheckBox;
            //sFDEL = chkFDEL.Checked.ToString() == "True" ? "1" : "0";
            switch (chkFDEL.Value)
            {
                case 1://SimpleCheckBox.CheckBoxValues.Checked:
                    sFDEL = "1";
                    break;
                case 2://SimpleCheckBox.CheckBoxValues.Indeterminate:
                    sFDEL = "2";
                    break;
                case 0://SimpleCheckBox.CheckBoxValues.UnChecked:
                    sFDEL = "0";
                    break;
            }
            SimpleImageCheckBox chkFAPR;
            chkFAPR = gvData.Rows[i].Cells[15].FindControl("chkFAPR") as SimpleImageCheckBox;
            //sFALL = chkFALL.Checked.ToString() == "True" ? "1" : "0";
            PCCore.Common.HRLog.RecordLog("Approve Value:" + chkFAPR.Value);
            switch (chkFAPR.Value)
            {
                case 1:// SimpleCheckBox.CheckBoxValues.Checked:
                    sFAPR = "1";
                    break;
                case 2://SimpleCheckBox.CheckBoxValues.Indeterminate:
                    sFAPR = "2";
                    break;
                case 0://SimpleCheckBox.CheckBoxValues.UnChecked:
                    sFAPR = "0";
                    break;
            }

            PCCore.PCMS.FunctionRole _role = new PCCore.PCMS.FunctionRole(gvData.Rows[i].Cells[6].Text);
            if (!_role.FullRights())
                sFALL = "0";
            if (!_role.AllowQuery())
                sFQRY = "0";
            if (!_role.AllowAdd())
                sFNEW = "0";
            if (!_role.AllowEdit())
                sFEDT = "0";
            if (!_role.AllowDelete())
                sFDEL = "0";
            if (!_role.AllowApprove())
                sFAPR = "0";
            
               
            try
            {
                if (chkfunctype.Checked)

                    Project = "";

                if (Project != Resources.Labels.All)
                {
                    //Hashtable row = GetRow(sUID, sFUNCID, sFALL, sFQRY, sFNEW, sFEDT, sFDEL);
                    Hashtable row = GetRow(sUID, sFUNCID, sFALL, sFQRY, sFNEW, sFEDT, sFDEL, sFAPR, Project);

                    //string sWhere = String.Format("ID={0} and FUNCID={1} and SCTYPE='u'", sUID, sFUNCID);
                    string sWhere = String.Format("ID={0} and FUNCID={1} and SCTYPE='u' and ProjCode = '{2}'", sUID, sFUNCID, Project);

                    table.BeginTransaction();
                    table.Delete(sWhere);
                    PCCore.Common.HRLog.RecordLog("CommandText: " + table.InternalCommand.CommandText);
                    PCCore.Common.HRLog.RecordLog("Before Insert: Approve:" + row["FAPR"]);
                    table.Insert(row);
                    
                    table.CommitTransaction();
                }
                else
                {
                    ArrayList alUserProject;
                    try
                    {
                        alUserProject = PCCore.Database.UserProject.UserProjectArray(sUID);
                        if (alUserProject.Count > 0)
                        {
                            foreach (Object o in alUserProject)
                            {
                                //Hashtable row = GetRow(sUID, sFUNCID, sFALL, sFQRY, sFNEW, sFEDT, sFDEL);
                                Hashtable row = GetRow(sUID, sFUNCID, sFALL, sFQRY, sFNEW, sFEDT, sFDEL,sFAPR, Convert.ToString(o));

                                //string sWhere = String.Format("ID={0} and FUNCID={1} and SCTYPE='u'", sUID, sFUNCID);
                                string sWhere = String.Format("ID={0} and FUNCID={1} and SCTYPE='u' and ProjCode = '{2}'", sUID, sFUNCID, Convert.ToString(o));

                                table.BeginTransaction();
                                table.Delete(sWhere);
                                PCCore.Common.HRLog.RecordLog("CommandText: " + table.InternalCommand.CommandText);
                                table.Insert(row);
                                table.CommitTransaction();
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        PCCore.Common.HRLog.RecordException("Get User Project", ex);
                    }
                }
            }
            catch (Exception ex)
            {

                Master.HandleError(ex);
                table.RollbackTransaction();
                return false;
            }
        }
        PCCore.Common.HRLog.RecordLog("End of RecrodSaveCheck");
        Master.ShowMessage(String.Format(Resources.Common.SaveOK, Resources.Labels.ScUserRight));
        return true;
    }

    protected void SetToolBarButtonVisable()
    {
        ToolBar tb = this.tbToolBar;
        tb.ButtonSaveVisible = true;
        tb.ButtonNewVisible = false;
        tb.ButtonDeleteVisible = false;
        tb.ButtonEditVisible = false;
        //tb.ButtonApproveVisible = false;
    }

    protected void SetDataSource()
    {

        gvData.DataSource = null;
        gvData.DataSourceID = null;
        gvData.DataBind();
        gvData.DataSourceID = dsGridView.ID;
        gvData.Visible = true;
        if (Convert.ToInt32(ddlSearch.SelectedValue) >= 0)
        {

            StringBuilder sb = new StringBuilder(string.Format("select convert(varchar,T0.ID)+Convert(varchar,T0.FuncID) as IDFUNCID,T0.ID,u.LOGINNAME,u.FULLNAME ,MID,m.NAME AS MNAME,T0.FUNCID,f.Code as funcCode,f.NAME as FUNCNAME,'' as PrjCode,T0.FALL,T0.FQRY,T0.FNEW,T0.FEDT,T0.FDEL,T0.FAPR,T0.CREATEDBY,T0.MODIFIEDBY,T0.CREATED,T0.MODIFIED from {0} T0 INNER JOIN dbo.SC_FUNCTION AS f on f.id = T0.Funcid INNER JOIN dbo.SC_MODULE AS m ON f.MID = m.ID inner join dbo.sc_user AS u on u.id = T0.id left join sc_role ro on ro.id = f.roleid where 1=1", Consts.TableScRight));
            // exclude security module
            sb.Append(string.Format(" and MID not in ({0}) ", Consts.ModuleSecurityID));
            if (!chkfunctype.Checked)
                sb.Append(" and isNull(ro.ProjectFunc,0) = 1");
            else
                sb.Append(" and isNull(ro.ProjectFunc,0) = 0");
            sb.Append(string.Format(" and T0.ID = {0} and SCType = '{1}'",ddlUser.SelectedValue,"u"));
            if (ddlProject.SelectedValue != "")
            {
                sb.Append(string.Format(" and T0.ProjCode = '{0}'", ddlProject.SelectedValue));
                switch (ddlSearch.SelectedValue)
                {
                    case "-1":
                        break;
                    case "0":
                        sb.AppendFormat(" and T0.FALL=1 ");
                        break;
                    case "1":
                        sb.AppendFormat(" and T0.FQRY=1 ");
                        break;
                    case "2":
                        sb.AppendFormat(" and T0.FNEW=1 ");
                        break;
                    case "3":
                        sb.AppendFormat(" and T0.FEDT=1 ");
                        break;
                    case "4":
                        sb.AppendFormat(" and T0.FDEL=1 ");
                        break;
                    case "5":
                        sb.AppendFormat(" and T0.FAPR=1 ");
                        break;
                }
                
            }
            else
            {
                if (!chkfunctype.Checked)
                sb.Append(" and 1 = 2 ");
            }
            sb.Append(" order by loginname,mname,funcname");
            dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
            dsGridView.SelectCommand = sb.ToString();
            PCCore.Common.HRLog.RecordLog("ScUserRight: " + sb.ToString());
            dsGridView.ErrorHandler = this.Master;

            dsGridView.DataBind(); //in order to set DirayRows value is right
            
        }
        else
        {


            //StringBuilder sb = new StringBuilder("select convert(varchar,ID)+Convert(varchar,FuncID) as IDFUNCID,ID,LOGINNAME,FULLNAME ,MID,MNAME,FUNCID,FUNCNAME,FALL,FQRY,FNEW,FEDT,FDEL,CREATEDBY,MODIFIEDBY,CREATED,MODIFIED from V_SC_EDITUSERRIGHT where 1=1");
            StringBuilder sb = new StringBuilder("select convert(varchar,T0.ID)+Convert(varchar,T0.FuncID) as IDFUNCID,T0.ID,T0.LOGINNAME,T0.FULLNAME ,T0.MID,T0.MNAME,T0.FUNCID,T0.FuncName as funcCode,T0.FUNCNAME as Funcname,'' as PrjCode,T0.FALL,T0.FQRY,T0.FNEW,T0.FEDT,T0.FDEL,T0.FAPR,T0.CREATEDBY,T0.MODIFIEDBY,T0.CREATED,T0.MODIFIED from V_SC_EDITUSERRIGHT T0 left join sc_role ro on ro.id = T0.roleid where 1=1");
            if (!chkfunctype.Checked)
                sb.Append(" and isNull(ro.ProjectFunc,0) = 1");
            else
                sb.Append(" and isNull(ro.ProjectFunc,0) = 0");

            if (Convert.ToDecimal(ddlUser.SelectedValue) > 0)
            {
                sb.AppendFormat(" and T0.id = {0}", ddlUser.SelectedValue);
            }
            if (Convert.ToDecimal(ddlModule.SelectedValue) >= 0)
            {
                sb.AppendFormat(" and T0.mid = {0}", ddlModule.SelectedValue);
            }
            //switch (ddlSearch.SelectedValue)
            //{
            //    case "-1":
            //        break;
            //    case "0":
            //        sb.AppendFormat(" and FALL=1 ");
            //        break;
            //    case "1":
            //        sb.AppendFormat(" and FQRY=1 ");
            //        break;
            //    case "2":
            //        sb.AppendFormat(" and FNEW=1 ");
            //        break;
            //    case "3":
            //        sb.AppendFormat(" and FEDT=1 ");
            //        break;
            //    case "4":
            //        sb.AppendFormat(" and FDEL=1 ");
            //        break;
            //}

            if (chkfunctype.Checked == false && ddlProject.Items.Count == 0)
                sb.Append(" and 1 = 2 ");

            //sb.AppendFormat(" and id in(select id from v_ps_employee where 1=1 {0})", SessionInfo.DataFilter);
            sb.Append(" order by loginname,mname,funcname");

            dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
            dsGridView.SelectCommand = sb.ToString();
            PCCore.Common.HRLog.RecordLog("ScUserRight: " + sb.ToString());
            dsGridView.ErrorHandler = this.Master;

            dsGridView.DataBind(); //in order to set DirayRows value is right
        }
    }


    protected void gvData_RowCreated(object sender, GridViewRowEventArgs e)
    {
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
                //e.Row.Cells[8].Visible = (!chkfunctype.Checked);
                //break;
            case DataControlRowType.Header:
                //e.Row.Cells[8].Visible = (!chkfunctype.Checked);
                e.Row.Cells[0].Style.Add("display", "none");//IDFUNCID
                //e.Row.Cells[1].Style.Add("display", "none");//ID
                //e.Row.Cells[2].Style.Add("display", "none");//Login Name
                e.Row.Cells[4].Style.Add("display", "none");//Mid
                e.Row.Cells[6].Style.Add("display", "none");//Funcid
                break;
        }
    }



    protected void gvData_Init(object sender, EventArgs e)
    {
        

        //gvData.HeaderDescriptions = "ID," + Resources.Labels.LoginName + "," + Resources.Labels.FullName + ",MID," + Resources.Labels.GroupName + ",FUNCID," + Resources.Labels.FunctionName + "," + Resources.Labels.All +
        //                       "," + Resources.Labels.QRY + "," + Resources.Labels.NEW + "," + Resources.Labels.EDT + "," + Resources.Labels.DEL + ",CREATEDBY ,MODIFIEDBY ,CREATED ,MODIFIED";
    }
    #region ClientCheckBoxChangeEvent

    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
        string sFALLCID = "", sFQRYCID = "", sFNEWCID = "", sFEDTCID = "", sFDELCID = "", sFAPRCID;//, sFLOGINCID = "", sFPUBCID = "";
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:
               // e.Row.Cells[8].Visible = (!chkfunctype.Checked);
                // Assign Project to Cell
                if (!chkfunctype.Checked)
                {
                    if (ddlProject.SelectedValue != "-2")
                        e.Row.Cells[9].Text = ddlProject.SelectedValue;
                    else
                        //e.Row.Cells[9].Text = Resources.Labels.All;
                        e.Row.Cells[9].Text = string.Empty;
                }
                else
                    e.Row.Cells[9].Text = String.Empty;

                // Find out current record 
               
                TableCellCollection cellobject = e.Row.Cells;

                SimpleImageCheckBox chkFALL;
                //chkFALL = e.Row.Cells[6].FindControl("chkFALL") as SimpleImageCheckBox;
                chkFALL = e.Row.Cells[10].FindControl("chkFALL") as SimpleImageCheckBox;

                SimpleImageCheckBox chkFQRY;
                //chkFQRY = e.Row.Cells[7].FindControl("chkFQRY") as SimpleImageCheckBox;
                chkFQRY = e.Row.Cells[11].FindControl("chkFQRY") as SimpleImageCheckBox;

                //CheckBox chkFNEW;
                SimpleImageCheckBox chkFNEW;
                //chkFNEW = e.Row.Cells[8].FindControl("chkFNEW") as SimpleImageCheckBox;
                chkFNEW = e.Row.Cells[12].FindControl("chkFNEW") as SimpleImageCheckBox;

                SimpleImageCheckBox chkFEDT;

                //chkFEDT = e.Row.Cells[9].FindControl("chkFEDT") as SimpleImageCheckBox;
                chkFEDT = e.Row.Cells[13].FindControl("chkFEDT") as SimpleImageCheckBox;

                SimpleImageCheckBox chkFDEL;
                //chkFDEL = e.Row.Cells[10].FindControl("chkFDEL") as SimpleImageCheckBox;
                chkFDEL = e.Row.Cells[14].FindControl("chkFDEL") as SimpleImageCheckBox;

                SimpleImageCheckBox chkFAPR;
                //chkFDEL = e.Row.Cells[10].FindControl("chkFDEL") as SimpleImageCheckBox;
                chkFAPR = e.Row.Cells[15].FindControl("chkFAPR") as SimpleImageCheckBox;
                
                sFALLCID = chkFALL.ClientID.ToString();
                sFQRYCID = chkFQRY.ClientID.ToString();
                sFNEWCID = chkFNEW.ClientID.ToString();
                sFEDTCID = chkFEDT.ClientID.ToString();
                sFDELCID = chkFDEL.ClientID.ToString();
                sFAPRCID = chkFAPR.ClientID.ToString();

                // Assign Role
                PCCore.PCMS.FunctionRole _frole = new PCCore.PCMS.FunctionRole(e.Row.Cells[6].Text.ToString());
                chkFALL.Visible = _frole.FullRights();
                chkFQRY.Visible = _frole.AllowQuery();
                chkFNEW.Visible = _frole.AllowAdd();
                chkFEDT.Visible = _frole.AllowEdit();
                chkFDEL.Visible = _frole.AllowDelete();
                chkFAPR.Visible = _frole.AllowApprove();

                
                // Find out current record 
                DataRow _drRights = PCCore.Database.SCRight.Find(ddlUser.SelectedValue, e.Row.Cells[6].Text.ToString(), e.Row.Cells[9].Text.ToString(), "u");
                if (_drRights != null)
                {
                    PCCore.Common.HRLog.RecordLog("Found");
                    chkFALL.Value = Convert.ToInt32(_drRights["FALL"]);
                    chkFQRY.Value = Convert.ToInt32(_drRights["FQRY"]);
                    chkFNEW.Value = Convert.ToInt32(_drRights["FNEW"]);
                    chkFEDT.Value = Convert.ToInt32(_drRights["FEDT"]);
                    chkFDEL.Value = Convert.ToInt32(_drRights["FDEL"]);
                    chkFAPR.Value = Convert.ToInt32(_drRights["FAPR"]);
                }
                else
                {
                    
                    chkFALL.Value = 0;
                    chkFQRY.Value = 0;
                    chkFNEW.Value = 0;
                    chkFEDT.Value = 0;
                    chkFDEL.Value = 0;
                    chkFAPR.Value = 0;
                    PCCore.Common.HRLog.RecordLog(chkFALL.Value);
                }

                //switch (ddlSearch.SelectedValue)
                //{
                //    case "-1":
                //        break;
                //    case "0":
                //        if (chkFALL.Value != 1)
                //            e.Row.Visible = false;
                        
                //        break;
                //    case "1":
                //        if (chkFQRY.Value != 1)
                //            e.Row.Visible = false;
                        
                //        break;
                //    case "2":
                //        if (chkFNEW.Value != 1)
                //            e.Row.Visible = false;
                        
                        
                //        break;
                //    case "3":
                //        if (chkFEDT.Value != 1)
                //            e.Row.Visible = false;
                        
                //        break;
                //    case "4":
                //        if (chkFDEL.Value != 1)
                //            e.Row.Visible = false;
                        
                        
                //        break;
                //    //e.Row.Visible=false;
                //}
                

                break;
            case DataControlRowType.Header:
                e.Row.Cells[0].Text = "IDFUNCID";
                //e.Row.Cells[8].Visible = (!chkfunctype.Checked);
                e.Row.Cells[1].Text = "ID";
                e.Row.Cells[2].Text = Resources.Labels.LoginName;
                e.Row.Cells[3].Text = Resources.Labels.FullName;
                e.Row.Cells[5].Text = Resources.Labels.Module;
                e.Row.Cells[6].Text = Resources.Labels.FunctionID;
                e.Row.Cells[7].Text = Resources.Labels.Function;
                e.Row.Cells[8].Text = Resources.Labels.FunctionID;
                
                SimpleImageCheckBox chkHALL;
                //chkHALL = e.Row.Cells[6].FindControl("chkHALL") as SimpleImageCheckBox;
                chkHALL = e.Row.Cells[10].FindControl("chkHALL") as SimpleImageCheckBox;

                SimpleImageCheckBox chkHQRY;
                //chkHQRY = e.Row.Cells[7].FindControl("chkHQRY") as SimpleImageCheckBox;
                chkHQRY = e.Row.Cells[11].FindControl("chkHQRY") as SimpleImageCheckBox;

                //CheckBox chkFNEW;
                SimpleImageCheckBox chkHNEW;
                //chkHNEW = e.Row.Cells[8].FindControl("chkHNEW") as SimpleImageCheckBox;
                chkHNEW = e.Row.Cells[12].FindControl("chkHNEW") as SimpleImageCheckBox;

                SimpleImageCheckBox chkHEDT;
                //chkHEDT = e.Row.Cells[9].FindControl("chkHEDT") as SimpleImageCheckBox;
                chkHEDT = e.Row.Cells[13].FindControl("chkHEDT") as SimpleImageCheckBox;

                SimpleImageCheckBox chkHDEL;
                //chkHDEL = e.Row.Cells[10].FindControl("chkHDEL") as SimpleImageCheckBox;
                chkHDEL = e.Row.Cells[14].FindControl("chkHDEL") as SimpleImageCheckBox;

                SimpleImageCheckBox chkHAPR;
                //chkHDEL = e.Row.Cells[10].FindControl("chkHDEL") as SimpleImageCheckBox;
                chkHAPR = e.Row.Cells[15].FindControl("chkHAPR") as SimpleImageCheckBox;

                //chkHALL.Attributes["onclick"] = "javascript:SimpleImageCheckBoxHeader(8);";//SimpleGridCheckBox
                //chkHQRY.Attributes["onclick"] = "javascript:SimpleImageCheckBoxHeader(9);";
                //chkHNEW.Attributes["onclick"] = "javascript:SimpleImageCheckBoxHeader(10);";
                //chkHEDT.Attributes["onclick"] = "javascript:SimpleImageCheckBoxHeader(11);";
                //chkHDEL.Attributes["onclick"] = "javascript:SimpleImageCheckBoxHeader(12);";

                chkHALL.Attributes["onclick"] = "javascript:SimpleImageCheckBoxHeader(10);";//SimpleGridCheckBox
                chkHQRY.Attributes["onclick"] = "javascript:SimpleImageCheckBoxHeader(11);";
                chkHNEW.Attributes["onclick"] = "javascript:SimpleImageCheckBoxHeader(12);";
                chkHEDT.Attributes["onclick"] = "javascript:SimpleImageCheckBoxHeader(13);";
                chkHDEL.Attributes["onclick"] = "javascript:SimpleImageCheckBoxHeader(14);";
                chkHAPR.Attributes["onclick"] = "javascript:SimpleImageCheckBoxHeader(15);";

                e.Row.HorizontalAlign = HorizontalAlign.Left;               
                break;
        }
        //gvData.HeaderDescriptionsForExport = "IDFUNCID,ID," + Resources.Labels.LoginName + "," + Resources.Labels.FullName + ",MID," + Resources.Labels.Module + ",FUNCID," + Resources.Labels.Function + "," + Resources.Labels.All +
        gvData.HeaderDescriptionsForExport = "IDFUNCID,ID," + Resources.Labels.LoginName + "," + Resources.Labels.FullName + ",MID," + Resources.Labels.Module + ",FUNCID," + Resources.Labels.Function + "," + Resources.Labels.Project + "," + Resources.Labels.All +
                               "," + Resources.Labels.QRY + "," + Resources.Labels.NEW + "," + Resources.Labels.EDT + "," + Resources.Labels.DEL + "," + Resources.Labels.APPR  + ",CREATEDBY ,MODIFIEDBY ,CREATED ,MODIFIED";
    
    }
    #endregion
    protected void ddlUser_SelectedIndexChanged(object sender, EventArgs e)
    {
        gvData.Visible = false;
        PCCore.Database.ValidationList.UserProjectList(ddlProject,ddlUser.SelectedValue);
    }
    protected void chkfunctype_CheckedChanged(object sender, EventArgs e)
    {
        gvData.Visible = false;
            ddlProject.Enabled = (!chkfunctype.Checked);
            ddlUser.AutoPostBack = (!chkfunctype.Checked);
        
        
    }
}
