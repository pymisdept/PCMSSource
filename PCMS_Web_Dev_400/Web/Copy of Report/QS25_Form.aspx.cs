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

public partial class QS25_Form : BasePage
{
    protected PCTable _table = null;
    protected PCTable _view = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;
    const string FILE_TYPE = "1125"; 

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

        Master.Title = Resources.Labels.QS25;
       
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
            row.Add("DOCSTATUS",Consts.DOCUMENT_DRAFT_PENDING); // Update-Pending
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

        //Cut Off Date
        row.Add(Consts.Field_DateTime_1, txtcutoff.Text);
        row.Add(Consts.Field_Upload_By, SessionInfo.UserId);
        row.Add(Consts.Field_Upload_Time, DateTime.Now.ToString());

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
        if (txtcutoff.Text != String.Empty)
        {
            PCCore.Common.HRLog.RecordLog("DocNum:" + string.Format("{0}/{1}", ddlProject.SelectedValue, Convert.ToDateTime(txtcutoff.Text).Month.ToString("D2")));
            row.Add("DocNum",string.Format("{0}-{1}",ddlProject.SelectedValue,Convert.ToDateTime(txtcutoff.Text).ToString("yy") + Convert.ToDateTime(txtcutoff.Text).Month.ToString("D2")));
        }
        

        return row;
    }
        




    void Master_Save(object sender, EventArgs e)
    {

        PCCore.Common.HRLog.RecordLog("Master_Save");
        if (!PCCore.PCMS.Validation.CheckProject(ddlProject))
        {
            Master.ShowMessage(Resources.Common.InvalidProject);
            return;
        }

        if (txtcutoff.Text == String.Empty)
        {
            Master.ShowMessage(Resources.Common.InputCalendarDate);
            return;
        }
        else
        {
            DateTime _d = Convert.ToDateTime(txtcutoff.Text);
            DateTime _startd = new DateTime(_d.Year, _d.Month, 1);
            PCCore.Common.HRLog.RecordLog("DateCompare:" + DateTime.Compare(Convert.ToDateTime(txtcutoff.Text), _startd.AddMonths(1).AddDays(-1)));
            if (DateTime.Compare(Convert.ToDateTime(txtcutoff.Text), _startd.AddMonths(1).AddDays(-1)) != 0)
            {
                Master.ShowMessage(Resources.Common.InputMonthEndDate);
                return;
            }
            else
            {
                
            }
        }

        //if (!CheckForm()) return;
        try
        {
            Hashtable row = GetRow();
            switch (Master.FormMode)
            {
                case MasterForm.FormModes.New:
                    PCCore.PCMS.ProjectReport _prjrpt = new PCCore.PCMS.ProjectReport(ddlProject.SelectedValue, Convert.ToDateTime(txtcutoff.Text));
                    if (_prjrpt.isPosted())
                    {
                        int ret = _prjrpt.CancelOldPostedPeriod(row["DocNum"].ToString());
                        //Master.ShowMessage(Resources.Common.PostedPeriod);

                        //return;
                    }

                    PCCore.Common.HRLog.RecordLog("DocNum1:" + row["DocNum"].ToString());
                    this.Table.BeginTransaction();                    
                    this.Table.Insert(row);
                    this.Table.CommitTransaction();

                    // Store Procedure
                   // SAPDb.Db.ExecQuery(string.Format("exec CPS_SP_PCMS_CreateReport {0},{1},'{2}',{3},'{4}','{5}'", row[Consts.FieldID].ToString(), FILE_TYPE,this.txtcutoff.Text.ToString(),SessionInfo.UserId,ddlProject.SelectedValue.ToString(),row["DocNum"]));

                    Page.RegisterStartupScript("CloseForm", "<script language='javascript'>{self.close();}</script>");
                    Master.ShowMessage(Resources.Common.RecordSaveOK);

                    Master.ClearForm();
                    break;

                case MasterForm.FormModes.Edit:
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
            
            //if (UpdateLoad.UploadFile.Count < 1)
            //{

            //    Master.ShowWarning(Resources.Messages.UploadFile);
            //    return false;
            //}

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
