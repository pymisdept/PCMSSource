using System;
using System.Data;
using System.Data.Common;
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
using SimpleControls.Web;

public partial class MAI010201 : BasePage
{
    const string TABLE_NAME = "DocumentProperty";
    const string FILE_TYPE = "4001201";
    string projcode = null;
    string id = null;
    string period = null;
    protected PCTable _table = null;
    protected PCTable _view = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;
    PCCore.PCMS.PMReportTable objpmtable;
    System.Data.DataTable _dtControl;
    string dtControlName = PCCore.PCMS.PMReportTable.PMR_PRJPARTICULAR;
    PCCore.PCMS.PMReport _pmreport;
    Hashtable _htControl;

    /* Rows Setting */
    const int row_count = 9;

    private void setData()
    {
        if (_htControl.Count > 0 && _htControl.ContainsKey("1"))
        {

            txtValue1.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ClientName].ToString();
            txtValue2.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ArchiName].ToString();
            txtValue3.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_DesignName].ToString();
            txtValue4.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_BSName].ToString();
            txtValue5.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_QSName].ToString();
            txtValue6.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_LandscapeArchiName].ToString();
            txtValue7.Text = PCCore.ComFunction2.DateString(((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_StartDate].ToString());

            txtValue8.Text = PCCore.ComFunction2.DateString(((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_CompDate].ToString());
            txtValue9.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ContractDuration].ToString();
            txtValue10.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_TIMEELAPSED].ToString();
            txtValue11.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_EOTGRANTED].ToString();
            txtValue12.Text = PCCore.ComFunction2.DateString(((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ExtCompDate].ToString());
            txtValue13.Text = "0"; // Contract Sum
            txtValue14.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_FORECAST_AMT].ToString();
            txtValue15.Text = "0"; // Certed Amt
        }
    }

    protected PCTable Table
    {
        get
        {
            if (_table == null) _table = new PCTable("DocumentProperty", this.SecurityInfo);
            return _table;
        }
    }


    protected void Page_Load(object sender, EventArgs e)
    {
        Master.ClearError();
        PCCore.Common.HRLog.RecordLog("Page Load");
        btnFirst.ImageUrl = Config.GetThemeBaseUrl(Page.Theme) + "/images/arrow_first_black.gif";
        btnPrev.ImageUrl = Config.GetThemeBaseUrl(Page.Theme) + "/images/arrow_left.gif";
        btnNext.ImageUrl = Config.GetThemeBaseUrl(Page.Theme) + "/images/arrow_right.gif";
        btnLast.ImageUrl = Config.GetThemeBaseUrl(Page.Theme) + "/images/arrow_last_black.gif";
        btnFirst.Enabled = false;
        btnPrev.Enabled = true;
        btnNext.Enabled = true;
        btnLast.Enabled = true;
        PCCore.Common.HRLog.RecordLog("IsPostBack? " + Page.IsPostBack.ToString());
        PCCore.Common.HRLog.RecordLog("CurrentCommand " + this.CurrentCommand);
        PCCore.Common.HRLog.RecordLog("HRCommand " + this.HRCommandValue);
        
        // get projectcode
        projcode = Request.QueryString["projcode"] as string;
        id = Request.QueryString["id"] as string;
        period = Request.QueryString["period"] as string;
        _pmreport = new PCCore.PCMS.PMReport(this.projcode, id, period);
        if (Page.IsPostBack)
        {
            string cmd = this.CurrentCommand;
            InitPMReport();
            //_dtControl = (DataTable)objpmtable.ht_PMRPT[dtControlName];
            switch (cmd)
            {
                case Consts.Save:
                    Save();
                    break;
                case Consts.SaveDB:
                    Save();
                    _pmreport.Save();
                    break;
                case Consts.Submit:
                    Save();
                    _pmreport.Save();
                    _pmreport.Post();
                    break;
            }
        }
            else
            {
                //this.InitDropDownList();
                this.InitPMReport();
                setData();
                this.txtProject.Text = this.projcode;
                this.txtProjName.Text = _pmreport.prjname;
                this.txtperiodfrom.Text = _pmreport.last_period_from.ToString("yyyy-MM-dd");

                this.txtperiodto.Text = _pmreport.last_period_to.ToString("yyyy-MM-dd");

                // Check the document can modify or not
                txtValue1.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
                txtValue2.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
                txtValue3.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
                txtValue4.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
                txtValue5.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
                txtValue6.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
                txtValue7.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
                txtValue8.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
                txtValue9.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
                txtValue10.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
                txtValue11.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
                txtValue12.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
                //txtValue13.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
                txtValue14.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
                //txtValue15.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
        }
        
        
       

    }//end of Page_Load




    protected void btnFirst_Click(object sender, ImageClickEventArgs e)
    {
        // Karrson: NO Active in this method

    }
    protected void btnPrev_Click(object sender, ImageClickEventArgs e)
    {
        // Karrson: No Active  in this method
    }
    protected void btnNext_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("MAI0102.aspx?projcode=" + projcode.ToString());
    }
    protected void btnLast_Click(object sender, ImageClickEventArgs e)
    {
        Response.Redirect("MAI0107.aspx?projcode=" + projcode.ToString());
    }
   
    //protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    //{

    //    PCCore.Common.HRLog.RecordLog("Command: " + this.CurrentCommand);
        
        
    //        switch (e.Row.RowType)
    //        {

    //            case DataControlRowType.DataRow:
    //                //Response.Write(e.Row.Cells[1].Width);
    //                e.Row.Cells[1].Font.Bold = true;
    //                e.Row.Cells[2].Font.Bold = true;
    //                e.Row.Cells[5].Font.Bold = true;
    //                PCCore.TextBox _txtvalue = new PCCore.TextBox();
    //                _txtvalue.ID = "txtValue";
    //                _txtvalue.Width = 300;
    //                if (this.CurrentCommand != Consts.Save)
    //                {
    //                    switch (e.Row.RowIndex + 1)
    //                    {
    //                        case 1:
    //                            _txtvalue.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ClientName].ToString();

    //                            break;
    //                        case 2:
    //                            _txtvalue.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ArchiName].ToString();
    //                            break;
    //                        case 3:
    //                            _txtvalue.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_DesignName].ToString();
    //                            break;
    //                        case 4:
    //                            _txtvalue.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_StructName].ToString();
    //                            break;
    //                        case 5:
    //                            _txtvalue.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_BSName].ToString();
    //                            break;
    //                        case 6:
    //                            _txtvalue.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_QSName].ToString();
    //                            break;
    //                        case 7:
    //                            _txtvalue.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_LandscapeArchiName].ToString();
    //                            break;
    //                        case 8:
    //                            _txtvalue.Text = Convert.ToDateTime(((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_StartDate].ToString()).ToString();
    //                            _txtvalue.DataType = SimpleControls.Web.SimpleTextBox.DataTypes.Date;
    //                            break;
    //                        case 9:
    //                            _txtvalue.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_CompDate].ToString();
    //                            _txtvalue.DataType = SimpleControls.Web.SimpleTextBox.DataTypes.Date;
    //                            break;
    //                        case 10:
    //                            _txtvalue.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ContractDuration].ToString();
    //                            _txtvalue.DataType = SimpleControls.Web.SimpleTextBox.DataTypes.Integer;
    //                            break;
    //                        case 11:
    //                            _txtvalue.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_TIMEELAPSED].ToString();
    //                            _txtvalue.DataType = SimpleControls.Web.SimpleTextBox.DataTypes.Integer;
    //                            break;
    //                        case 12:
    //                            _txtvalue.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_EOTGRANTED].ToString();
    //                            _txtvalue.DataType = SimpleControls.Web.SimpleTextBox.DataTypes.Integer;
    //                            break;
    //                        case 13:
    //                            _txtvalue.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ExtCompDate].ToString();
    //                            _txtvalue.DataType = SimpleControls.Web.SimpleTextBox.DataTypes.Date;
    //                            break;
    //                        case 14:
    //                            _txtvalue.Text = "0"; // get From Database direct
    //                            _txtvalue.DataType = SimpleControls.Web.SimpleTextBox.DataTypes.Currency;
    //                            break;
    //                        case 15:
    //                            _txtvalue.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_FORECAST_AMT].ToString();
    //                            _txtvalue.DataType = SimpleControls.Web.SimpleTextBox.DataTypes.Currency;
    //                            break;
    //                        case 16:
    //                            _txtvalue.Text = "0";// get from database 
    //                            _txtvalue.DataType = SimpleControls.Web.SimpleTextBox.DataTypes.Currency;
    //                            break;

    //                    }
    //                }
    //                e.Row.Cells[3].Controls.Add(_txtvalue);
    //                //e.Row.Cells[3].Style.Add("padding-right", "5px");
    //                break;
    //        }
            
        
    //}

    void InitPMReport()
    {

        try
        {
            _pmreport = new PCCore.PCMS.PMReport(this.projcode, id, period);
            // This Checking is most important for get information
            if (_pmreport.SessionExists())
            {
                //objpmtable = _pmreport._PMReportTable;
                Hashtable _ht = PCCore.Common.XMLPMSession.Read();
                _htControl = (Hashtable)_ht[dtControlName];
            }
            else
            {
                // set all value to session file first
                Hashtable _ht = _pmreport._pmobject.toHashTale();
                PCCore.Common.XMLPMSession.Write(_ht);

                _htControl = (Hashtable)PCCore.Common.XMLPMSession.Read(dtControlName);
                //objpmtable = _pmreport._pmobject;
            }
        }
        catch (Exception ex)
        {
            PCCore.Common.HRLog.RecordException("InitPMReport", ex);
            throw ex;
        }

    }

    private void Save()
    {
        PCCore.Common.HRLog.RecordLog("Save Function");
        if (_htControl.Count == 0)
            PCCore.PCMS.PMReportTable.EmptyRow(ref _htControl, PCCore.PCMS.PMReportTable.SE_HT_FIELDS_PMR_PRJPARTICULAR, "1");

        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ClientName] = txtValue1.Text;
        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ArchiName] = txtValue2.Text;
        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_DesignName] = txtValue3.Text;
        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_BSName] = txtValue4.Text;
        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_QSName] = txtValue5.Text;
        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_LandscapeArchiName] = txtValue6.Text;
        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_StartDate]= txtValue7.Text;
        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_CompDate] = txtValue8.Text;
        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ContractDuration] = txtValue9.Text;
        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_TIMEELAPSED] = txtValue10.Text;
        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_EOTGRANTED] = txtValue11.Text;
        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ExtCompDate] = txtValue12.Text ;
        
        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_FORECAST_AMT] = txtValue14.Text.Replace(",","");

        PCCore.Common.XMLPMSession.Write(dtControlName, _htControl);

        // Clear Old Record when Save
        //_htControl.Clear();
        // Replace With new Value from the web page
    //    try
    //    {

    //        PCCore.Common.HRLog.RecordLog("Control Count: " + gvData.Rows[1].Cells[2].Controls.Count);
    //        for (int i = 0; i < gvData.Rows[1].Cells[3].Controls.Count; i++)
    //        {
    //            PCCore.Common.HRLog.RecordLog("Control Name: " + gvData.Rows[1].Cells[3].Controls[i].ID);
    //            PCCore.Common.HRLog.RecordLog("Control Name: " + gvData.Rows[1].Cells[3].Controls[i].ClientID);
    //        }
            
    //        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ArchiName] = ((PCCore.TextBox)gvData.Rows[1].Cells[3].FindControl("txtValue")).Text;
    //        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_BSName] = ((PCCore.TextBox)gvData.Rows[2].FindControl("txtValue")).Text;
    //        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ClientName] = ((PCCore.TextBox)gvData.FindControl("txtValue")).Text;
    //        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_CompDate] = ((PCCore.TextBox)gvData.FindControl("txtValue")).Text;
    //        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ContractDuration] = ((PCCore.TextBox)gvData.FindControl("txtValue")).Text;
    //        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_DesignName] = ((PCCore.TextBox)gvData.FindControl("txtValue")).Text;
    //        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_EOTGRANTED] = ((PCCore.TextBox)gvData.FindControl("txtValue")).Text;
    //        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_ExtCompDate] = ((PCCore.TextBox)gvData.FindControl("txtValue")).Text;
    //        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_FORECAST_AMT] = ((PCCore.TextBox)gvData.FindControl("txtValue")).Text;
    //        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_LandscapeArchiName] = ((PCCore.TextBox)gvData.FindControl("txtValue")).Text;
    //        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_QSName] = ((PCCore.TextBox)gvData.FindControl("txtValue")).Text;
    //        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_StartDate] = ((PCCore.TextBox)gvData.FindControl("txtValue")).Text;
    //        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_StructName] = ((PCCore.TextBox)gvData.FindControl("txtValue")).Text;
    //        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_PART_TIMEELAPSED] = ((PCCore.TextBox)gvData.FindControl("txtValue")).Text;
    //    }
    //    catch (Exception ex)
    //    { PCCore.Common.HRLog.RecordException("Save", ex); }
    //    //((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote1.Text;
    //    //((Hashtable)_htControl["2"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote2.Text;
    //    //((Hashtable)_htControl["3"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote3.Text;
    //    //((Hashtable)_htControl["4"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote4.Text;
    //    //((Hashtable)_htControl["5"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote5.Text;
    //    //((Hashtable)_htControl["6"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote6.Text;
    //    //((Hashtable)_htControl["7"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote7.Text;
    //    //((Hashtable)_htControl["8"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote8.Text;
    //    //((Hashtable)_htControl["9"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote9.Text;

    //    // Write the new value to Session xml file replace with old one
    //    PCCore.Common.XMLPMSession.Write(dtControlName, _htControl);
    //    // Finish to Save
    //    /*********************************************************************************
    //    //PCCore.Common.HRLog.RecordLog("_dtControl Count: " + _dtControl.Rows.Count.ToString());
    //    //if (_dtControl.Rows.Count > 0)
    //    //    _dtControl.Rows[0][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote1.Text;
    //    //if (_dtControl.Rows.Count > 1)
    //    //    _dtControl.Rows[1][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote2.Text;
    //    //if (_dtControl.Rows.Count > 2)
    //    //    _dtControl.Rows[2][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote3.Text;
    //    //if (_dtControl.Rows.Count > 3)
    //    //    _dtControl.Rows[3][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote4.Text;
    //    //if (_dtControl.Rows.Count > 4)
    //    //    _dtControl.Rows[4][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote5.Text;
    //    //if (_dtControl.Rows.Count > 5)
    //    //    _dtControl.Rows[5][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote6.Text;
    //    //if (_dtControl.Rows.Count > 6)
    //    //    _dtControl.Rows[6][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote7.Text;
    //    //if (_dtControl.Rows.Count > 7)
    //    //    _dtControl.Rows[7][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote8.Text;
    //    //if (_dtControl.Rows.Count > 8)
    //    //    _dtControl.Rows[8][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote9.Text;
    //    ******************************************************************************************/

    }
}
