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

public partial class MAI010204 : BasePage
{
    const string TABLE_NAME = "DocumentProperty";
    const string FILE_TYPE = "4001204";
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
    bool viewmode;
    Hashtable _htControl;
    /* Rows Setting */
    const int row_count = 5;

    private void setData()
    {
       
        
        
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
            
            setData();
            this.txtProject.Text = this.projcode;
            this.txtProjName.Text = _pmreport.prjname;
            this.txtperiodfrom.Text = _pmreport.last_period_from.ToString("yyyy-MM-dd");
            this.txtperiodto.Text = _pmreport.last_period_to.ToString("yyyy-MM-dd");

            // Check the document can modify or not
            viewmode = PCCore.PCMS.PMReport.CanModify(projcode, id);
            txtFOrgin_1.Enabled = viewmode;
            txtFOrgin_2.Enabled = viewmode;
            txtFOrgin_3.Enabled = viewmode;
            txtFOrgin_4.Enabled = viewmode;
            txtFOrgin_5.Enabled = viewmode;
            txtFVisied_1.Enabled = viewmode;
            txtFVisied_2.Enabled = viewmode;
            txtFVisied_3.Enabled = viewmode;
            txtFVisied_4.Enabled = viewmode;
            txtFVisied_5.Enabled = viewmode;
            txtSOrgin_1.Enabled = viewmode;
            txtSOrgin_2.Enabled = viewmode;
            txtSOrgin_3.Enabled = viewmode;
            txtSOrgin_4.Enabled = viewmode;
            txtSOrgin_5.Enabled = viewmode;
            txtSVisied_1.Enabled = viewmode;
            txtSVisied_2.Enabled = viewmode;
            txtSVisied_3.Enabled = viewmode;
            txtSVisied_4.Enabled = viewmode;
            txtSVisied_5.Enabled = viewmode;


            
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

    }

}
