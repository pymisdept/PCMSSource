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
using System.Collections.Generic;

public partial class QSI04_Form : BasePage
{
    protected PCTable _table = null;
    protected PCTable _view = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;
    const string FILE_TYPE = "1004";

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
        Master.Save += new MasterFormMUpl.SaveEventHandler(Master_Save);

        Master.Title = Resources.Labels.QSI04;
       
        if (Page.IsPostBack)
        {
        }
        else
        {
            switch (Master.FormMode)
            {
                case MasterFormMUpl.FormModes.New:
                    break;

                case MasterFormMUpl.FormModes.Edit:
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
        if (Master.FormMode == MasterFormMUpl.FormModes.New)
        {
            row.Add("DOCSTATUS", Consts.DOCUMENT_DRAFT_PENDING); // Update-Pending
            row.Add("ISERROR", 0); // Error Status
        }
        if (Master.FormMode == MasterFormMUpl.FormModes.Edit)
        {
            row.Add("DOCSTATUS", Consts.DOCUMENT_DRAFT_PENDING); // Update-Pending
            row.Add("ISERROR", 0); // Error Status
        }
        if (Master.FormMode == MasterFormMUpl.FormModes.New)
            row.Add(Consts.FieldID, PCDb.GetNextID());
        else
            row.Add(Consts.FieldID, Convert.ToDecimal(txtID.Text));

        row.Add("FILENAME", txtFileName.Text);
        row.Add("DESCRIPTION", txtDescription.Text);
        row.Add("REMARKS", txtRemarks.Text);
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

    Hashtable GetRow(int i, bool isMulti)
    {
        Hashtable row = new Hashtable();
        //Karrson: Add Status
        if (Master.FormMode == MasterFormMUpl.FormModes.New)
        {
            row.Add("DOCSTATUS", Consts.DOCUMENT_DRAFT_PENDING); // Update-Pending
            row.Add("ISERROR", 0); // Error Status
        }
        if (Master.FormMode == MasterFormMUpl.FormModes.Edit)
        {
            row.Add("DOCSTATUS", Consts.DOCUMENT_DRAFT_PENDING); // Update-Pending
            row.Add("ISERROR", 0); // Error Status
        }
        if (Master.FormMode == MasterFormMUpl.FormModes.New)
            row.Add(Consts.FieldID, PCDb.GetNextID());
        else
            row.Add(Consts.FieldID, Convert.ToDecimal(txtID.Text));

        string tmpFile = this.UpdateLoad.UploadFile[i].FileName;

        row.Add("FILENAME", tmpFile);
        row.Add("DESCRIPTION", txtDescription.Text);
        row.Add("REMARKS", txtRemarks.Text);
        row.Add("TYPE", FILE_TYPE);
        if (!isMulti)
        {
            row.Add("PROJECTCODE", ddlProject.SelectedValue);        
        }
        else
        {
            try
            {
                row.Add("PROJECTCODE", tmpFile.Substring(tmpFile.LastIndexOf("\\") + 1, 8));
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        return row;
    }

    void Master_Save(object sender, EventArgs e)
    {
        // Modified by Ken, 20161205, begin
        List<int> errList = new List<int>();
        int uploadedCount = 0;
        bool isErr = false;
        string errMsg = "";

        bool exceedLimit = false;
        int limit = 0;
        // Modified by Ken, 20161205, end

        // Modified by Ken, 20161205, begin
        if (UpdateLoad.UploadFile.Count < 1)
        {
            Master.ShowWarning(Resources.Messages.UploadFile);
            return;
        }
        else
        {
            if (!SessionInfo.IsBatchUpload || UpdateLoad.UploadFile.Count == 1)
            {
                limit = UpdateLoad.UploadFile.Count;
                if (!PCCore.PCMS.Validation.CheckProject(ddlProject))
                {
                    Master.ShowMessage(Resources.Common.InvalidProject);
                    return;
                }
                string errMessage = MasterFormMUpl.CheckForm(UpdateLoad, Master.FormMode);
                if (errMessage != "")
                {
                    Master.ShowMessage(errMessage);
                    return;
                }
            }
            else
            {
                // Modified by Ken, 20161205, begin
                exceedLimit = UpdateLoad.UploadFile.Count > Config.BatchSCUploadLimit;
                limit = exceedLimit ? Config.BatchSCUploadLimit : UpdateLoad.UploadFile.Count;

                List<int> checkFormResult = MasterFormMUpl.CheckForm(UpdateLoad, Master.FormMode, limit);

                if (limit == 0)
                {
                    isErr = true;
                    errMsg = "* " + Resources.Messages.UploadExcelFile + "\\n";
                }
                else if (limit == 1 && UpdateLoad.UploadFile[0].FileName == "")
                {
                    isErr = true;
                    errMsg = "* " + Resources.Messages.UploadExcelFile + "\\n";
                }
                else if (limit >= 1 && exceedLimit)
                {
                    isErr = true;
                    errMsg = "* " + string.Format(Resources.Messages.ExceedBatchUploadLimit, Config.BatchSCUploadLimit) + "\\n";
                }

                for (int k = 0; k < limit; k++)
                {
                    string prjcode = UpdateLoad.UploadFile[k].FileName;
                    string filename = UpdateLoad.UploadFile[k].FileName;

                    if (filename == "")
                        continue;

                    filename = filename.Substring(filename.LastIndexOf("\\") + 1);

                    #region check if the file is in excel format
                    if (checkFormResult.Contains(k))
                    {
                        isErr = true;
                        errMsg += "- " + string.Format(Resources.Messages.InvalidExcelFile, filename) + "\\n";
                        errList.Add(k);
                        continue;
                    }
                    #endregion

                    #region check if the file name is start with project code
                    try
                    {
                        prjcode = filename.Substring(0, 8);
                    }
                    catch (Exception ex)
                    {
                        isErr = true;
                        errMsg += "- " + string.Format(Resources.Messages.InvalidExcelFile, filename) + "\\n";
                        errList.Add(k);
                        continue;
                    }
                    #endregion

                    #region check if the project code is valid
                    if (!PCCore.PCMS.Validation.CheckProjectExist(prjcode))
                    {
                        isErr = true;
                        errMsg += "- " + Resources.Common.InvalidProject + ": " + prjcode + "\\n";
                        errList.Add(k);
                    }
                    #endregion

                    #region check if the user has access right to upload project report
                    else if (!SessionInfo.IsSupervisor)
                    {
                        if (!PCCore.PCMS.Validation.CheckProjectAccess(FILE_TYPE, SessionInfo.UserId, prjcode))
                        {
                            isErr = true;
                            errMsg += "- " + string.Format(Resources.Messages.AccessdeniedPRBatch, prjcode) + "\\n";
                            errList.Add(k);
                        }
                    }
                    #endregion
                }

                if (isErr)
                    Page.RegisterStartupScript("PRError", "<script type='text/javascript' language='javascript'>alert('" + errMsg + "');</script>");

                // Modified by Ken, 20161205, end
            }
        }

        //if (!CheckForm()) return;
        try
        {
            // Modified by Ken, 20160220, begin
            bool allowMulti = false;
            if (SessionInfo.IsBatchUpload && UpdateLoad.UploadFile.Count > 1)
                allowMulti = true;
            // Modified by Ken, 20160220, end

            for (int j = 0; j < limit; j++)
            {
                // Modified by Ken, 20161205, begin
                if (UpdateLoad.UploadFile[j].FileName == "")
                    continue;

                if (errList.Contains(j))
                    continue;

                uploadedCount += 1;
                // Modified by Ken, 20161205, end

                // Modified by Ken, 20160220, begin
                Hashtable row = GetRow(j, allowMulti);
                // Modified by Ken, 20160220, end

                switch (Master.FormMode)
                {
                    case MasterFormMUpl.FormModes.New:
                        this.Table.BeginTransaction();
                        this.Table.Insert(row);
                        UpdateLoad.SaveFiles(Convert.ToDecimal(row["ID"]), Table.InternalCommand, j);
                        this.Table.CommitTransaction();
                        // modify by karrson: upload file 

                        UpdateLoad.SaveToFile(Convert.ToDecimal(row["ID"]), Table.InternalCommand, Resources.TemplateInfo.QSI04, row["PROJECTCODE"].ToString(), false, j);
                        //Master.ShowMessage(String.Format(Resources.Common.SaveOK, ddlProject.SelectedItem.Text));
                        //Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{self.close();}</script>");
                        //Master.ShowMessage(Resources.Common.RecordSaveOK);

                        Master.ClearForm();
                        break;

                    case MasterFormMUpl.FormModes.Edit:
                        //this.Table.BeginTransaction();
                        //this.Table.Update(row);
                        //UpdateLoad.SaveFiles(Convert.ToDecimal(row["ID"]), Table.InternalCommand);
                        //this.Table.CommitTransaction();
                        //// modify by karrson: upload file 

                        //UpdateLoad.SaveToFile(Convert.ToDecimal(row["ID"]), Table.InternalCommand, Resources.TemplateInfo.PUI02, ddlProject.SelectedValue, true);
                        ////Master.ShowMessage(String.Format(Resources.Common.UpdateOK, ddlProject.SelectedItem.Text));
                        //Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{self.close();}</script>");
                        //Master.ShowMessage(Resources.Common.RecordUpdateOK);
                        break;
                }
            }

            if (uploadedCount > 0)
            {
                Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{window.opener.location.reload(false);self.close();}</script>");
                Master.ShowMessage(Resources.Common.RecordSaveOK);
                Master.ClearForm();
            }
            else if (isErr && uploadedCount == 0)
            {
                Master.ShowMessage(Resources.Messages.PRBatchUploadFailed);
                Master.ClearForm();
                Page.RegisterStartupScript("PRError2", "<script type='text/javascript' language='javascript'>alert('" + Resources.Messages.NoFileUploaded + "');</script>");
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
        if (Master.FormMode == MasterFormMUpl.FormModes.New)
        {
            if (UpdateLoad.UploadFile.Count < 1)
            {
                Master.ShowWarning(Resources.Messages.UploadFile);
                return false;
            }
        }

        //if (Master.FormMode == MasterFormMUpl.FormModes.Edit)
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
