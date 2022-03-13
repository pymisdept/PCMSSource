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

public partial class QSI22_Form : BasePage
{
    protected PCTable _table = null;
    protected PCTable _view = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;
    const string FILE_TYPE = "1023";

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
    }

    protected void Page_Load(object sender, EventArgs e)    
    {
        Master.ClearError();
        Master.Save += new MasterForm.SaveEventHandler(Master_Save);

        Master.Title = Resources.Labels.QSI23;
       
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
        if(Master.FormMode==MasterForm.FormModes.New)
            row.Add(Consts.FieldID, PCDb.GetNextID());
        else
            row.Add(Consts.FieldID, Convert.ToDecimal(txtID.Text));

        row.Add("FILENAME", txtFileName.Text);
        row.Add("DESCRIPTION", txtDescription.Text);
        row.Add("REMARKS",txtRemarks.Text);
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
                    // modify by karrson: upload file 

                    UpdateLoad.SaveToFile(Convert.ToDecimal(row["ID"]), Table.InternalCommand, Resources.TemplateInfo.QSI23,ddlProject.SelectedValue);
                    //Master.ShowMessage(String.Format(Resources.Common.SaveOK, ddlProject.SelectedItem.Text));
                    Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{window.opener.location.reload(false);self.close();}</script>");
                    Master.ShowMessage(Resources.Common.RecordSaveOK);

                    Master.ClearForm();
                    break;

                case MasterForm.FormModes.Edit:
                    this.Table.BeginTransaction();
                    this.Table.Update(row);
                    UpdateLoad.SaveFiles(Convert.ToDecimal(row["ID"]),Table.InternalCommand);
                    this.Table.CommitTransaction();
                    // modify by karrson: upload file 

                    UpdateLoad.SaveToFile(Convert.ToDecimal(row["ID"]), Table.InternalCommand, Resources.TemplateInfo.QSI23,ddlProject.SelectedValue,true);
                    //Master.ShowMessage(String.Format(Resources.Common.UpdateOK, ddlProject.SelectedItem.Text));
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
}//end of class
