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

public partial class QSI40_Form : BasePage
{
    protected PCTable _table = null;
    protected PCTable _view = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;
    const string FILE_TYPE = "1040";

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

        Master.Title = Resources.Labels.PUI02;
       
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
    
    Hashtable GetRow(int i, bool isSingleFile)
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

        if (ddlProject.SelectedValue != "-2" && isSingleFile)
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
        // Modified by Ken, 20161107, begin
        if (UpdateLoad.UploadFile.Count < 1)
        {
            Master.ShowWarning(Resources.Messages.UploadFile);
            return;
        }
        else
        {
            if (!SessionInfo.IsBatchUpload)
            {
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
                string errMsg = "";
                bool isError = false;

                errMsg = MasterFormMUpl.CheckForm(UpdateLoad, Master.FormMode);
                if (errMsg != "")
                    isError = true;

                if (UpdateLoad.UploadFile.Count > Config.BatchUploadLimit)
                {
                    isError = true;
                    if (errMsg != "")
                        errMsg += string.Format(Resources.Messages.ExceedBatchUploadLimit, Config.BatchUploadLimit) + "\\n";
                    else
                        errMsg = string.Format(Resources.Messages.ExceedBatchUploadLimit, Config.BatchUploadLimit);
                }
                if (!isError)
                {
                    for (int k = 0; k < UpdateLoad.UploadFile.Count; k++)
                    {
                        string prj = UpdateLoad.UploadFile[k].FileName;
                        try
                        {
                            prj = prj.Substring(prj.LastIndexOf("\\") + 1, 8);
                        }
                        catch (Exception ex)
                        {
                            // Master.ShowMessage(string.Format(Resources.Common.InvalidExcelFileName, prj));
                            isError = true;
                            prj = prj.Substring(prj.LastIndexOf("\\") + 1);
                            errMsg += string.Format(Resources.Common.InvalidExcelFileName, prj) + "\\n";
                        }

                        if (prj.Length == 8)
                        {
                            if (!PCCore.PCMS.Validation.CheckProjectExist(prj))
                            {
                                //Master.ShowMessage(Resources.Common.InvalidProject + ": " + prj);
                                isError = true;
                                errMsg += Resources.Common.InvalidProject + ": " + prj + "\\n";
                            }
                            else if (!SessionInfo.IsSupervisor)
                            {
                                if (!PCCore.PCMS.Validation.CheckProjectAccess(FILE_TYPE, SessionInfo.UserId, prj))
                                {
                                    //Master.ShowMessage(string.Format(Resources.Messages.AccessdeniedPRBatch, prj));
                                    isError = true;
                                    errMsg += string.Format(Resources.Messages.AccessdeniedPRBatch, prj) + "\\n";
                                }
                            }
                        }
                    }
                }

                if (isError)
                {
                    Page.RegisterStartupScript("PRError", "<script type='text/javascript' language='javascript'>alert('" + errMsg + "');</script>");
                    Master.ShowMessage(Resources.Messages.PRBatchUploadFailed);
                    return;
                }
            }
        }
        // Modified by Ken, 20161107, end

        //MasterFormMUpl.ButtonSave = false ;
        //Master.("Start uploading");
        #region Old
        //string SavePath;
        //if (Config.UploadFolder != String.Empty)
        //    SavePath = Config.UploadFolder + "QSI40";
        //else
        //    SavePath = Server.MapPath(Resources.TemplateInfo.UPLOADPATH + "QSI40");

        //string val = txt_UploadPath.Text;
        //string[] list = System.IO.Directory.GetFiles(val);
        //string[] prjList = list;
        //string tmp_filename = "";
        //for (int i = 0; i < list.Length; i++)
        //{
        //    if (list[i].Substring(list[i].LastIndexOf('.') + 1) != "xls")
        //        continue;
        //    try
        //    {
        //        tmp_filename = list[i].Substring(list[i].LastIndexOf('\\') + 2, 8);
        //        prjList[i] = tmp_filename;
        //    }
        //    catch (Exception ex)
        //    {
        //        Master.ShowMessage(String.Format(Resources.Common.InvalidExcelFileName,tmp_filename));
        //        return;
        //    }
        //}

        //for (int i = 0; i < list.Length; i++)
        //{
        //    if (list[i].Substring(list[i].LastIndexOf('.') + 1) != "xls")
        //        continue;

        //    System.Web.HttpPostedFile HIFs = hifColl[hifCon]; 
        //    HIFs.SaveAs(SavePath + "\\" + SaveName);
        //    System.IO.File.Copy(list[i], SavePath + prjList[i] + ".xls");
        //}
        //Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{self.close();}</script>");
        //return;
        //if (!CheckForm()) return;
        #endregion
        try
        {
            // Modified by Ken, 20161107, begin
            for (int j = 0; j < UpdateLoad.UploadFile.Count; j++)
            {
                Hashtable row;

                if (UpdateLoad.UploadFile.Count == 1)
                    row = GetRow(j, true);
                else
                    row = GetRow(j, false);

                switch (Master.FormMode)
                {
                    case MasterFormMUpl.FormModes.New:
                        this.Table.BeginTransaction();
                        this.Table.Insert(row);
                        UpdateLoad.SaveFiles(Convert.ToDecimal(row["ID"]), Table.InternalCommand, j);
                        this.Table.CommitTransaction();
                        // modify by karrson: upload file 
                        UpdateLoad.SaveToFile(Convert.ToDecimal(row["ID"]), Table.InternalCommand, Resources.TemplateInfo.QSI40, row["PROJECTCODE"].ToString(),false, j);
                        //UpdateLoad.SaveToFile(Convert.ToDecimal(row["ID"]), Table.InternalCommand, Resources.TemplateInfo.QSI40, row["PROJECTCODE"]);
                        
                        //Master.ShowMessage(String.Format(Resources.Common.SaveOK, ddlProject.SelectedItem.Text));
                        //Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{self.close();}</script>");
                        break;

                    case MasterFormMUpl.FormModes.Edit:
                        //this.Table.BeginTransaction();
                        //this.Table.Update(row);
                        //UpdateLoad.SaveFiles(Convert.ToDecimal(row["ID"]), Table.InternalCommand);
                        //this.Table.CommitTransaction();
                        // modify by karrson: upload file 

                        //UpdateLoad.SaveToFile(Convert.ToDecimal(row["ID"]), Table.InternalCommand, Resources.TemplateInfo.PUI02, ddlProject.SelectedValue, true);
                        //Master.ShowMessage(String.Format(Resources.Common.UpdateOK, ddlProject.SelectedItem.Text));
                        //Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{self.close();}</script>");
                        //Master.ShowMessage(Resources.Common.RecordUpdateOK);
                        break;
                }
            }

            Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{self.close();}</script>");
            Master.ShowMessage(Resources.Common.RecordSaveOK);
            Master.ClearForm();
            // Modified by Ken, 20161107, end
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
