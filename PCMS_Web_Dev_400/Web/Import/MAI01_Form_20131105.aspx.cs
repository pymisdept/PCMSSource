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

public partial class MAI01_Form : BasePage
{
    protected PCTable _table = null;
    protected PCTable _view = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;
    const string FILE_TYPE = "4001";
    string _PmReportID = "";
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

        Master.Title = Resources.Labels.MAI01;
       
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
            row.Add("DOCSTATUS", Consts.DOCUMENT_DRAFT); // Update-Pending
            row.Add("ISERROR", 0); // Error Status
        }

        //if (Master.FormMode == MasterForm.FormModes.Edit)
        //{
        //    row.Add("DOCSTATUS", Consts.DOCUMENT_DRAFT_PENDING); // Update-Pending
        //    row.Add("ISERROR", 0); // Error Status
        //}

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

        // Find out the latest period and create new period in documentproperty
        PCCore.PCMS.PMReport _pmrpt = new PCCore.PCMS.PMReport(ddlProject.SelectedValue);
        row.Add("DOCNUM",_pmrpt.NewPeriodRunningCode(ddlProject.SelectedValue));

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

        // Check PMs Report
        string errMessage = PCCore.PCMS.PMReport.NewAddValidate(ddlProject.SelectedValue);
        if (errMessage != String.Empty)
        {
            Master.ShowMessage(errMessage);
            return;
        }
        if (PCCore.PCMS.PMReport.hasOutStandPMsReport(ddlProject.SelectedValue))
        {
            Master.ShowMessage(GetGlobalResourceObject(Consts.ResourcesMessages,"PMsReportExist").ToString());
            return;
        }


        //string errMessage = MasterForm.CheckForm(UpdateLoad, Master.FormMode);
        //if (errMessage != "")
        //{
        //    Master.ShowMessage(errMessage);
        //    return;
        //}

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
                    
                    

                    
                    
                    /* Remove single PO input way*/
                    /*
                    // modify by karrson: upload file 
                    UpdateLoad.SaveToFile(Convert.ToDecimal(row["ID"]), Table.InternalCommand, Resources.TemplateInfo.PUI02,ddlProject.SelectedValue);
                    */
                
                     
                
                    //Master.ShowMessage(String.Format(Resources.Common.SaveOK, ddlProject.SelectedItem.Text));
                    //Master.ShowMessage(Resources.Common.RecordSaveOK);
                    // Get Saved ID 
                    _PmReportID = PCCore.Database.DocumentProperty.FindID("DocNum", row["DOCNUM"].ToString());
                    PMReportID.Value = _PmReportID;
                    
                    Page.RegisterStartupScript("GoPMReport", "<script language='javascript'>{PMReport();}</script>");
                    Master.ClearForm();
                    break;

                case MasterForm.FormModes.Edit:
                    this.Table.BeginTransaction();
                    this.Table.Update(row);
                    UpdateLoad.SaveFiles(Convert.ToDecimal(row["ID"]),Table.InternalCommand);
                    this.Table.CommitTransaction();
                   
                    /* remove single input way */
                    // modify by karrson: upload file 
                    /*
                    UpdateLoad.SaveToFile(Convert.ToDecimal(row["ID"]), Table.InternalCommand, Resources.TemplateInfo.PUI02,ddlProject.SelectedValue,true);
                    */
                     
                      
                    //Master.ShowMessage(String.Format(Resources.Common.UpdateOK, ddlProject.SelectedItem.Text));
                    //Master.ShowMessage(Resources.Common.RecordUpdateOK);
                    //Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{self.close();}</script>");
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
        
        

        return true;
    }
    protected void btnDownload_Click(object sender, EventArgs e)
    {

    }
    protected void ddlProject_SelectedIndexChanged(object sender, EventArgs e)
    {
        //if (ddlProject.SelectedValue != "" && ddlProject.SelectedValue != "-1" && ddlProject.SelectedValue != "-2")
        //{
        //    if (ddlInputType.SelectedValue == "MR")
        //    {
        //        ddlmrno.Items.Clear();
        //        ddlmrno.Enabled = true;
        //        PCCore.Database.ValidationList.SMRList(this.ddlmrno, ddlProject.SelectedValue,"PO");
        //    }
        //    else
        //    {
        //        ddlmrno.Items.Clear();
        //        ddlmrno.Enabled = false;
        //        ddlmrno.Items.Add(new ListItem(Consts.DropDownNone, Consts.DropDownOptionNone.ToString()));
        //    }
            
        //}
        //else
        //{

        //    ddlmrno.Items.Clear();
        //    ddlmrno.Items.Add(new ListItem(Resources.Labels.NA, Resources.Labels.NA.ToString()));
        //    ddlmrno.Enabled = false;
        //}
    }
    
}//end of class
