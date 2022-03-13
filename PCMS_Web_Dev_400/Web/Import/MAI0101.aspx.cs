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

public partial class MAI0101 : BasePage
{
    const string TABLE_NAME = "DocumentProperty";
    const string FILE_TYPE = "40011";
    string projcode = null;
    string id = null;
    string period = null;
    protected PCTable _table = null;
    protected PCTable _view = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;
    PCCore.PCMS.PMReport _pmreport;
    PCCore.PCMS.PMReportTable objpmtable;
    System.Data.DataTable _dtControl;
    Hashtable _htControl;
    string dtControlName = PCCore.PCMS.PMReportTable.PMR_EXECSUMMARY;
    
    /* Rows Setting */
    const int row_count = 9;

    private void setData()
    {
        
        if (_htControl.Count > 0)
            txtnote1.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description].ToString();
            
        if (_htControl.Count > 1)
            txtnote2.Text = ((Hashtable)_htControl["2"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description].ToString();
            
        if (_htControl.Count > 2)
            txtnote3.Text = ((Hashtable)_htControl["3"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description].ToString();
        if (_htControl.Count > 3)
            txtnote4.Text = ((Hashtable)_htControl["4"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description].ToString();
        if (_htControl.Count > 4)
            txtnote5.Text = ((Hashtable)_htControl["5"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description].ToString();
        if (_htControl.Count > 5)
            txtnote6.Text = ((Hashtable)_htControl["6"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description].ToString();
        if (_htControl.Count > 6)
            txtnote7.Text = ((Hashtable)_htControl["7"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description].ToString();
        if (_htControl.Count > 7)
            txtnote8.Text = ((Hashtable)_htControl["8"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description].ToString();
        if (_htControl.Count > 8)
            txtnote9.Text = ((Hashtable)_htControl["9"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description].ToString();
        
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
        projcode = Request.QueryString["projcode"] as string;
        id = Request.QueryString["id"] as string;
        period = Request.QueryString["period"] as string;

        

        btnFirst.ImageUrl = Config.GetThemeBaseUrl(Page.Theme) + "/images/arrow_first_black.gif";
        btnPrev.ImageUrl = Config.GetThemeBaseUrl(Page.Theme) + "/images/arrow_left.gif";
        btnNext.ImageUrl = Config.GetThemeBaseUrl(Page.Theme) + "/images/arrow_right.gif";
        btnLast.ImageUrl = Config.GetThemeBaseUrl(Page.Theme) + "/images/arrow_last_black.gif";
        btnFirst.Enabled = false;
        btnPrev.Enabled = true;
        btnNext.Enabled = true;
        btnLast.Enabled = true;
        // get projectcode
        PCCore.Common.HRLog.RecordLog("IsPostBack? " + Page.IsPostBack.ToString());
        PCCore.Common.HRLog.RecordLog("CurrentCommand " + this.CurrentCommand);
        PCCore.Common.HRLog.RecordLog("HRCommand " + this.HRCommandValue);
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
            PCCore.Common.HRLog.RecordLog("InitPMReport");
            InitPMReport();
            
            // DataTable not use in this version
            /*(*******************************************************
           // _dtControl = (DataTable)objpmtable.ht_PMRPT[dtControlName];
            
            
            //for (int i = 0; i < _dtControl.Columns.Count; i++)
            //{
            //    PCCore.Common.HRLog.RecordLog("Column Name of CPSPMX: " + _dtControl.Columns[i].ColumnName);
            //}
             * ****************************************************/
            setData();
            this.txtProject.Text = this.projcode;
            this.txtProjName.Text = _pmreport.prjname;
            this.txtperiodfrom.Text = _pmreport.last_period_from.ToString("yyyy-MM-dd");

            this.txtperiodto.Text = _pmreport.last_period_to.ToString("yyyy-MM-dd");

            // Check the document can modify or not
            txtnote1.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
            txtnote2.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
            txtnote3.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
            txtnote4.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
            txtnote5.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
            txtnote6.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
            txtnote7.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
            txtnote8.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
            txtnote9.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
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
   
    
    void InitPMReport()
    {

        try
        {
            if (_pmreport == null)
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
        // Clear Old Record when Save
        //_htControl.Clear();
        // Replace With new Value from the web page
        try
        {
            if (!_htControl.ContainsKey("1"))
                PCCore.PCMS.PMReportTable.EmptyRow(ref _htControl, PCCore.PCMS.PMReportTable.SE_HT_FIELDS_PMR_EXECSUMMARY, "1");
            if (!_htControl.ContainsKey("2"))
                PCCore.PCMS.PMReportTable.EmptyRow(ref _htControl, PCCore.PCMS.PMReportTable.SE_HT_FIELDS_PMR_EXECSUMMARY, "2");
            if (!_htControl.ContainsKey("3"))
                PCCore.PCMS.PMReportTable.EmptyRow(ref _htControl, PCCore.PCMS.PMReportTable.SE_HT_FIELDS_PMR_EXECSUMMARY, "3");
            if (!_htControl.ContainsKey("4"))
                PCCore.PCMS.PMReportTable.EmptyRow(ref _htControl, PCCore.PCMS.PMReportTable.SE_HT_FIELDS_PMR_EXECSUMMARY, "4");
            if (!_htControl.ContainsKey("5"))
                PCCore.PCMS.PMReportTable.EmptyRow(ref _htControl, PCCore.PCMS.PMReportTable.SE_HT_FIELDS_PMR_EXECSUMMARY, "5");
            if (!_htControl.ContainsKey("6"))
                PCCore.PCMS.PMReportTable.EmptyRow(ref _htControl, PCCore.PCMS.PMReportTable.SE_HT_FIELDS_PMR_EXECSUMMARY, "6");
            if (!_htControl.ContainsKey("7"))
                PCCore.PCMS.PMReportTable.EmptyRow(ref _htControl, PCCore.PCMS.PMReportTable.SE_HT_FIELDS_PMR_EXECSUMMARY, "7");
            if (!_htControl.ContainsKey("8"))
                PCCore.PCMS.PMReportTable.EmptyRow(ref _htControl, PCCore.PCMS.PMReportTable.SE_HT_FIELDS_PMR_EXECSUMMARY, "8");
            if (!_htControl.ContainsKey("9"))
                PCCore.PCMS.PMReportTable.EmptyRow(ref _htControl, PCCore.PCMS.PMReportTable.SE_HT_FIELDS_PMR_EXECSUMMARY, "9");
            

            
        }

        catch (Exception ex)
        {
            PCCore.Common.HRLog.RecordException("Save Function", ex);
        }
        ((Hashtable) _htControl["1"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote1.Text;
        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number] = "1";
        ((Hashtable)_htControl["2"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote2.Text;
        ((Hashtable)_htControl["2"])[PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number] = "2";
        ((Hashtable)_htControl["3"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote3.Text;
        ((Hashtable)_htControl["3"])[PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number] = "3";
        ((Hashtable)_htControl["4"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote4.Text;
        ((Hashtable)_htControl["4"])[PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number] = "4";
        ((Hashtable)_htControl["5"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote5.Text;
        ((Hashtable)_htControl["5"])[PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number] = "5";
        ((Hashtable)_htControl["6"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote6.Text;
        ((Hashtable)_htControl["6"])[PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number] = "6";
        ((Hashtable)_htControl["7"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote7.Text;
        ((Hashtable)_htControl["7"])[PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number] = "7";
        ((Hashtable)_htControl["8"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote8.Text;
        ((Hashtable)_htControl["8"])[PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number] = "8";
        ((Hashtable)_htControl["9"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote9.Text;
        ((Hashtable)_htControl["9"])[PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number] = "9";
        
        // Write the new value to Session xml file replace with old one
        PCCore.Common.XMLPMSession.Write(dtControlName, _htControl);
        // Finish to Save
        /*********************************************************************************
        //PCCore.Common.HRLog.RecordLog("_dtControl Count: " + _dtControl.Rows.Count.ToString());
        //if (_dtControl.Rows.Count > 0)
        //    _dtControl.Rows[0][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote1.Text;
        //if (_dtControl.Rows.Count > 1)
        //    _dtControl.Rows[1][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote2.Text;
        //if (_dtControl.Rows.Count > 2)
        //    _dtControl.Rows[2][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote3.Text;
        //if (_dtControl.Rows.Count > 3)
        //    _dtControl.Rows[3][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote4.Text;
        //if (_dtControl.Rows.Count > 4)
        //    _dtControl.Rows[4][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote5.Text;
        //if (_dtControl.Rows.Count > 5)
        //    _dtControl.Rows[5][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote6.Text;
        //if (_dtControl.Rows.Count > 6)
        //    _dtControl.Rows[6][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote7.Text;
        //if (_dtControl.Rows.Count > 7)
        //    _dtControl.Rows[7][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote8.Text;
        //if (_dtControl.Rows.Count > 8)
        //    _dtControl.Rows[8][PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtnote9.Text;
        ******************************************************************************************/
        
    }
    
    
}
