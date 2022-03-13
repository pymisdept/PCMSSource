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

public partial class MAI010203 : BasePage
{
    const string TABLE_NAME = "DocumentProperty";
    const string FILE_TYPE = "4001203";
    
    protected PCTable _table = null;
    protected PCTable _view = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;
    string projcode = null;
    string id = null;
    string period = null;
    Hashtable _htControl;
    PCCore.PCMS.PMReport _pmreport;
    string dtControlName = PCCore.PCMS.PMReportTable.PMR_OverallProgress;

    /* Rows Setting */
    const int row_count = 3;

    private void setData()
    {
        if (_htControl.ContainsKey("1"))
            txtValue1.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description].ToString();
        if (_htControl.ContainsKey("2"))
            txtValue2.Text = ((Hashtable)_htControl["2"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description].ToString();
        if (_htControl.ContainsKey("3"))
            txtValue3.Text = ((Hashtable)_htControl["3"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description].ToString();

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
        btnFirst.ImageUrl = Config.GetThemeBaseUrl(Page.Theme) + "/images/arrow_first_black.gif";
        btnPrev.ImageUrl = Config.GetThemeBaseUrl(Page.Theme) + "/images/arrow_left.gif";
        btnNext.ImageUrl = Config.GetThemeBaseUrl(Page.Theme) + "/images/arrow_right.gif";
        btnLast.ImageUrl = Config.GetThemeBaseUrl(Page.Theme) + "/images/arrow_last_black.gif";
        btnFirst.Enabled = false;
        btnPrev.Enabled = true;
        btnNext.Enabled = true;
        btnLast.Enabled = true;
        // get projectcode
        projcode = Request.QueryString["projcode"] as string;
        id = Request.QueryString["id"] as string;
        period = Request.QueryString["period"] as string;
        _pmreport = new PCCore.PCMS.PMReport(this.projcode, id, period);
        if (Page.IsPostBack)
        {
            string cmd = this.CurrentCommand;
            _htControl = _pmreport.InitData(dtControlName);
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
            _htControl = _pmreport.InitData(dtControlName);

            setData();
            this.txtProject.Text = this.projcode;
            this.txtProjName.Text = _pmreport.prjname;
            this.txtperiodfrom.Text = _pmreport.last_period_from.ToString("yyyy-MM-dd");
            this.txtperiodto.Text = _pmreport.last_period_to.ToString("yyyy-MM-dd");


            // Check the document can modify or not
            txtValue1.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
            txtValue2.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
            txtValue3.Enabled = PCCore.PCMS.PMReport.CanModify(projcode, id);
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
    protected void gvData_DataBound(object sender, EventArgs e)
    {
        //gvData.Columns[0].ItemStyle.Width = 5;
        //gvData.Columns[1].ItemStyle.Width = 95;
    }
    
    private void Save()
    {
        PCCore.Common.HRLog.RecordLog("Save");
        try
        {
            if (_htControl.Count < 1)
            {
                PCCore.PCMS.PMReportTable.EmptyRow(ref _htControl, PCCore.PCMS.PMReportTable.SE_HT_FIELDS_PMR_OverallProgress, "1");
                ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number] = 1;
            }
            if (_htControl.Count < 2)
            {
                PCCore.PCMS.PMReportTable.EmptyRow(ref _htControl, PCCore.PCMS.PMReportTable.SE_HT_FIELDS_PMR_OverallProgress, "2");
                ((Hashtable)_htControl["2"])[PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number] = 2;
            }
            if (_htControl.Count < 3)
            {
                PCCore.PCMS.PMReportTable.EmptyRow(ref _htControl, PCCore.PCMS.PMReportTable.SE_HT_FIELDS_PMR_OverallProgress, "3");
                ((Hashtable)_htControl["3"])[PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number] = 3;
            }

        }
        catch (Exception ex)
        { PCCore.Common.HRLog.RecordException("Save", ex); }
        PCCore.Common.HRLog.RecordLog("Save1");
        ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtValue1.Text;
        ((Hashtable)_htControl["2"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtValue2.Text;
        ((Hashtable)_htControl["3"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtValue3.Text;
        
        PCCore.Common.HRLog.RecordLog(((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description]);
        PCCore.Common.HRLog.RecordLog(((Hashtable)_htControl["2"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description]);
        PCCore.Common.HRLog.RecordLog(((Hashtable)_htControl["3"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description]);
        PCCore.Common.XMLPMSession.Write(dtControlName, _htControl);
    }
}