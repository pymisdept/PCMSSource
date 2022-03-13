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

public partial class MAI010302 : BasePage
{
    const string TABLE_NAME = "DocumentProperty";
    const string FILE_TYPE = "4001302";
    string projcode = null;
    string id = null;
    string period = null;
    protected PCTable _table = null;
    protected PCTable _view = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;
    PCCore.PCMS.PMReport _pmreport;
    
    /* Rows Setting */
    const int row_count = 9;

    private void setData()
    {
       
        gvData.HeaderDescriptions = "SEQ,NAME,,Last Month,This Month,Next Month";
        gvData.HiddenFields += "SEQ"; 
        dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        dsGridView.SelectCommand = "SELECT SEQ,NAME,':',ISNULL(LastMonth,0) as [Last Month],ISNULL(ThisMonth,0) as [This Month],ISNULL(NextMonth,0) as [Next Month] FROM [$PaymentCert]";
        dsGridView.ErrorHandler = this.Master;

        gvData.DataBind();
        
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
           
        }
        else
        {
            //this.InitDropDownList();
            setData();
            this.txtProject.Text = this.projcode;
            this.txtProjName.Text = _pmreport.prjname;
            this.txtperiodfrom.Text = _pmreport.last_period_from.ToString("yyyy-MM-dd");
            this.txtperiodto.Text = _pmreport.last_period_to.ToString("yyyy-MM-dd");
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
    protected void gvData_RowDataBound(object sender, GridViewRowEventArgs e)
    {
       
    }
    
}
