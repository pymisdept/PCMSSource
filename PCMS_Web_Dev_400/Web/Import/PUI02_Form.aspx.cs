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
using SimpleControls;
using SimpleControls.SimpleDatabase;

public partial class PUI02_Form : BasePage
{
    protected PCTable _table = null;
    protected PCTable _view = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;
    const string FILE_TYPE = "2001";

    protected PCTable Table
    {
        get
        {
            if (_table == null) _table = new PCTable("DocumentProperty", this.SecurityInfo);
            return _table;
        }
    }

    protected PCTable View
    {
        get
        {
            if (_view == null) _view = new PCTable("DocumentProperty", this.SecurityInfo);
            return _view;
        }
    }
    protected override void OnInit(EventArgs e)
    {        
        base.ShowWebMenu = false;
        base.OnInit(e);
        ddlmrno.Items.Add(new ListItem(Consts.DropDownNone, Consts.DropDownOptionNone.ToString()));
    }

    protected void Page_Load(object sender, EventArgs e)    
    {
        PCCore.Common.HRLog.RecordLog("Page_Load");
        Master.ClearError();
        Master.Save += new MasterForm.SaveEventHandler(Master_Save);

        Master.Title = Resources.Labels.PUI02;
       
        if (Page.IsPostBack)
        {
        }
        else
        {
            switch (Master.FormMode)
            {
                case MasterForm.FormModes.New:
                    break;

                case MasterForm.FormModes.Edit:
                    txtID.Text = Master.RecordID;
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
            //PCDb.InitDropDownList(this.ddlProject, "v_project", "PrjCode", "U_PrjFName", Consts.DropDownOptionNone, null, string.Format("prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','")));
            //PCCore.Database.ValidationList.ProjectList(ddlProject, Consts.DropDownOptionNone);
            PCCore.Database.ValidationList.AllowAddDropDownProjectList(ddlProject, Consts.DropDownOptionNone);
            
            //ddlProject = PCCore.Database.ValidationList.AllowAddDropDownProjectList(ddlProject);
            
            ddlInputType.Items.Add(new ListItem("Manual", "PO"));
            ddlInputType.Items.Add(new ListItem("From Site Material Requisition", "MR"));
            ddlmrno.Items.Add(new ListItem(Resources.Labels.NA, Resources.Labels.NA.ToString()));
            ddlmrno.Enabled = false;

            if (_dr != null)
            {
                this.SetListValue(_dr);
            }
            else
            {
                if (ddlProject.Items.Count == 2)
                {
                    ddlProject.SelectedIndex = 1;
                }
            }
        }
    }

    private void SetListValue(DataRow dr)
    {
        txtFileName.Text = dr["FileName"].ToString();
        txtDescription.Text = dr["Description"].ToString();
        txtRemarks.Text = dr["Remarks"].ToString();
        ddlProject.SelectedValue = dr["projectcode"].ToString();
    }



    Hashtable GetRow()
    {
        Hashtable row = new Hashtable();
        //Karrson: Add Status
        if (Master.FormMode == MasterForm.FormModes.New)
        {
            row.Add("DOCSTATUS", Consts.DOCUMENT_DRAFT_PENDING); // Update-Pending
            row.Add("ISERROR", 0); // Error Status
        }
        if (Master.FormMode == MasterForm.FormModes.Edit)
        {
            row.Add("DOCSTATUS", Consts.DOCUMENT_DRAFT_PENDING); // Update-Pending
            row.Add("ISERROR", 0); // Error Status
        }
        if (Master.FormMode == MasterForm.FormModes.New)
            row.Add(Consts.FieldID, PCDb.GetNextID());
        else
            row.Add(Consts.FieldID, Convert.ToDecimal(txtID.Text));

        row.Add("FILENAME", txtFileName.Text);
        row.Add("DESCRIPTION", txtDescription.Text);
        row.Add("REMARKS", txtRemarks.Text);
        PCCore.Common.HRLog.RecordLog("FileType:" + FILE_TYPE);
        row.Add("TYPE", FILE_TYPE);
        if (ddlProject.SelectedValue != "-2")
        {
            row.Add("PROJECTCODE", ddlProject.SelectedValue);
        }
        else
        {
            row.Add("PROJECTCODE", DBNull.Value);
        }




        return row;
    }

    void Master_Save(object sender, EventArgs e)
    {
        PCCore.Common.HRLog.RecordLog("Master_Save");
        //Master.startUpdate();
        if (!PCCore.PCMS.Validation.CheckProject(ddlProject))
        {
            Master.ShowMessage(Resources.Common.InvalidProject);
            return;
        }
        string errMessage = MasterForm.CheckForm(UpdateLoad, Master.FormMode);
        if (errMessage != "")
        {
            Master.ShowMessage(errMessage);
            return;
        }

        //if (!CheckForm()) return;
        try
        {
            Hashtable row = GetRow();
            switch (Master.FormMode)
            {
                case MasterForm.FormModes.New:
                    this.Table.BeginTransaction();                    
                    this.Table.Insert(row);
                    UpdateLoad.SaveFiles(Convert.ToDecimal(row["ID"]),Table.InternalCommand);
                    this.Table.CommitTransaction();
                    

                    if (ddlInputType.SelectedValue == "MR")
                    {
                        UpdateLoad.SaveToFile(Convert.ToDecimal(row["ID"]), Table.InternalCommand, Resources.TemplateInfo.PUI02_MR, ddlProject.SelectedValue, true);
                    }
                   if (ddlInputType.SelectedValue == "PO")
                    {
                        UpdateLoad.SaveToFile(Convert.ToDecimal(row["ID"]), Table.InternalCommand, Resources.TemplateInfo.PUI02_PO, ddlProject.SelectedValue, true);
                    }
                    
                    /* Remove single PO input way*/
                    /*
                    // modify by karrson: upload file 
                    UpdateLoad.SaveToFile(Convert.ToDecimal(row["ID"]), Table.InternalCommand, Resources.TemplateInfo.PUI02,ddlProject.SelectedValue);
                    */
                
                
                
                    //Master.ShowMessage(String.Format(Resources.Common.SaveOK, ddlProject.SelectedItem.Text));
                   //Master.ShowMessage(Resources.Common.RecordSaveOK);
                   Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{window.opener.location.reload(false);self.close();}</script>");
                    Master.ShowMessage(Resources.Common.RecordSaveOK);

                    Master.ClearForm();
                    break;

                case MasterForm.FormModes.Edit:
                    this.Table.BeginTransaction();
                    this.Table.Update(row);
                    UpdateLoad.SaveFiles(Convert.ToDecimal(row["ID"]),Table.InternalCommand);
                    this.Table.CommitTransaction();
                    
                
                    if (ddlInputType.SelectedValue == "MR")
                    {
                        UpdateLoad.SaveToFile(Convert.ToDecimal(row["ID"]), Table.InternalCommand, Resources.TemplateInfo.PUI02_MR, ddlProject.SelectedValue, true);
                    }
                    if (ddlInputType.SelectedValue == "PO")
                    {
                        UpdateLoad.SaveToFile(Convert.ToDecimal(row["ID"]), Table.InternalCommand, Resources.TemplateInfo.PUI02_PO, ddlProject.SelectedValue, true);
                    }


                    /* remove single input way */
                    // modify by karrson: upload file 
                    /*
                    UpdateLoad.SaveToFile(Convert.ToDecimal(row["ID"]), Table.InternalCommand, Resources.TemplateInfo.PUI02,ddlProject.SelectedValue,true);
                    */
                     
                      
                    //Master.ShowMessage(String.Format(Resources.Common.UpdateOK, ddlProject.SelectedItem.Text));
                    //Master.ShowMessage(Resources.Common.RecordSaveOK);
                    Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{window.opener.location.reload(false);self.close();}</script>");
                    Master.ShowMessage(Resources.Common.RecordUpdateOK);
                    break;
            }
        }
        catch (Exception ex)
        {
            Master.HandleError(ex);
            this.Table.RollbackTransaction();
        }
        //Master.endUpdate();
    }   

    bool CheckForm()
    {
        
        //if (string.IsNullOrEmpty(txtFileName.Text.Trim()))
        //{
        //    Master.ShowMessage(Resources.Messages.InputFileName);
        //    txtFileName.Focus();
        //    return false;
        //}
        if (Master.FormMode == MasterForm.FormModes.New)
        {
            if (UpdateLoad.UploadFile.Count < 1)
            {
                Master.ShowWarning(Resources.Messages.UploadFile);
                return false;
            }
        }

        //if (Master.FormMode == MasterForm.FormModes.Edit)
        //{
        //    if (UpdateLoad.UploadFile.Count < 1 && UpdateLoad.NeedDeleteRecordID !="")
        //    {
        //        Master.ShowWarning(Resources.Messages.UploadFile);
        //        return false;
        //    }
        //}

        return true;
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {

    }
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        if (ddlProject.SelectedValue != "" && ddlProject.SelectedValue != "-1" && ddlProject.SelectedValue != "-2")
        {
            if (ddlInputType.SelectedValue == "MR")
            {
                ddlmrno.Items.Clear();
                ddlmrno.Enabled = true;
                PCCore.Database.ValidationList.SMRList(this.ddlmrno, ddlProject.SelectedValue,"PO");
            }
            else
            {
                ddlmrno.Items.Clear();
                ddlmrno.Enabled = false;
                ddlmrno.Items.Add(new ListItem(Consts.DropDownNone, Consts.DropDownOptionNone.ToString()));
            }
            
        }
        else
        {

            ddlmrno.Items.Clear();
            ddlmrno.Items.Add(new ListItem(Resources.Labels.NA, Resources.Labels.NA.ToString()));
            ddlmrno.Enabled = false;
        }
    }
    protected void ddlInputType_SelectedIndexChanged(object sender, EventArgs e)
    {
        /*
        if (ddlInputType.SelectedValue == "MR" || ddlProject.SelectedValue != "" || ddlProject.SelectedValue != "-1" || ddlProject.SelectedValue != "-2")
        {
            ddlmrno.Items.Clear();
            ddlmrno.Items.Add(new ListItem(Consts.DropDownNone, Consts.DropDownOptionNone.ToString()));

        }
        else
        {
            ddlmrno.Items.Clear();
            PCCore.Database.ValidationList.SMRList(this.ddlmrno, ddlProject.SelectedValue);
        }*/

        if (ddlInputType.SelectedValue == "MR")
        {
            ddlmrno.Items.Clear();
            ddlmrno.Enabled = true;
            PCCore.Database.ValidationList.SMRList(this.ddlmrno, ddlProject.SelectedValue, "PO");
        }
        else
        {
            ddlmrno.Items.Clear();
            ddlmrno.Items.Add(new ListItem(Resources.Labels.NA, Resources.Labels.NA.ToString()));
            ddlmrno.Enabled = false;

        }
    }
}//end of class
