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

public partial class MAI010303 : BasePage
{
    const string TABLE_NAME = "DocumentProperty";
    const string FILE_TYPE = "4001303";
    string projcode = null;
    string id = null;
    string period = null;
    protected PCTable _table = null;
    protected PCTable _view = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;
    PCCore.PCMS.PMReportTable objpmtable;
    PCCore.PCMS.PMReport _pmreport;
    System.Data.DataTable _dtControl;
    string dtControlName = PCCore.PCMS.PMReportTable.PMR_EOTClaims;
    Hashtable _htControl;
    bool viewmode;
    /* Rows Setting */
    const int row_count = 9;
    
    private void setData()
    {
        if (_htControl.Count > 0 && _htControl.ContainsKey("1"))
        {
            txtValue.Text = ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description].ToString();
            
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
            viewmode = PCCore.PCMS.PMReport.CanModify(projcode, id);
            txtValue.Enabled = viewmode;
            
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
    private void Save()
    {
        if (_htControl.Count == 0)
            PCCore.PCMS.PMReportTable.EmptyRow(ref _htControl, PCCore.PCMS.PMReportTable.SE_HT_FIELDS_PMR_ScopeOfWorks, "1");

        foreach (object o in _htControl.Keys)
        {
            PCCore.Common.HRLog.RecordLog("Contains Key:" + o.ToString());
        }
        PCCore.Common.HRLog.RecordLog("Contains Key:" + _htControl.ContainsKey("1").ToString());

        try
        {
            ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description] = txtValue.Text;
            ((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number] = "1";
        }
        catch (Exception ex)
        {
            PCCore.Common.HRLog.RecordException("Save", ex);
        }
        PCCore.Common.HRLog.RecordLog(((Hashtable)_htControl["1"])[PCCore.PCMS.PMReportTable.PMR_COMM_Description]);
        PCCore.Common.XMLPMSession.Write(dtControlName, _htControl);

    }
    
}
