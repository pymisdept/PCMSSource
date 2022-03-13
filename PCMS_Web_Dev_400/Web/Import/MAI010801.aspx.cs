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

public partial class MAI010801 : BasePage
{
    const string TABLE_NAME = "DocumentProperty";
    const string FILE_TYPE = "4011801";
    string projcode = null;
    string id = null;
    string period = null;
    protected PCTable _table = null;
    protected PCTable _view = null;
    protected DataTable _dt = null;
    protected DataRow _dr = null;
    Hashtable _htControl;
    PCCore.PCMS.PMReport _pmreport;
    string dtControlName = PCCore.PCMS.PMReportTable.PMR_Images;
    bool viewmode;
    /* Rows Setting */
    const int row_count = 9;

    private void setData()
    {
        DataTable _dtControl = initDataTable();
        // insert record from hashtable to datatable 
        DataRow _dr;
        foreach (object o in _htControl.Keys)
        {
            _dr = _dtControl.NewRow();
            _dr["delButton"] = "1";
            _dr[PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number] = o.ToString();
            _dr[PCCore.PCMS.PMReportTable.PMR_IMG_PATH] = ((Hashtable) _htControl[o.ToString()])[PCCore.PCMS.PMReportTable.PMR_IMG_PATH].ToString();
            _dr[PCCore.PCMS.PMReportTable.PMR_IMG_DESCRIPTION] = ((Hashtable)_htControl[o.ToString()])[PCCore.PCMS.PMReportTable.PMR_IMG_DESCRIPTION].ToString();
            _dr[PCCore.PCMS.PMReportTable.PMR_IMG_FILENAME] = ((Hashtable)_htControl[o.ToString()])[PCCore.PCMS.PMReportTable.PMR_IMG_FILENAME].ToString();
            _dr["image"] = "1";
            _dtControl.Rows.Add(_dr);
        }

        //gvData.HeaderDescriptions = "SEQ,imgpath,File,Description";
        //gvData.HiddenFields += "SEQ"; 
        //dsGridView.SelectCommandType = SqlDataSourceCommandType.Text;
        //dsGridView.SelectCommand = "SELECT SEQ, imgpath, description, '' as link FROM [$ImageUpload]";
        //dsGridView.ErrorHandler = this.Master;
        
        gvData.DataSource = _dtControl;
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

    DataTable initDataTable()
    {
        DataTable _dt = new DataTable();
        
        _dt.Columns.Add(new DataColumn(PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number));
        _dt.Columns.Add(new DataColumn(PCCore.PCMS.PMReportTable.PMR_IMG_PATH));
        _dt.Columns.Add(new DataColumn(PCCore.PCMS.PMReportTable.PMR_IMG_DESCRIPTION));
        _dt.Columns.Add(new DataColumn(PCCore.PCMS.PMReportTable.PMR_IMG_FILENAME));
        _dt.Columns.Add(new DataColumn("image"));
        _dt.Columns.Add(new DataColumn("delButton"));
        return _dt;
    }
    protected void Page_Load(object sender, EventArgs e)
    {
        PCCore.Common.HRLog.RecordLog("Page_Load");
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
        try
        {
            _htControl = (Hashtable) ViewState[dtControlName];
            
        }
        catch (Exception ex)
        { }
        if (_htControl == null)
            _htControl = _pmreport.InitData(dtControlName);
            PCCore.Common.HRLog.RecordLog("_htControl Count" + _htControl.Count.ToString());

        if (Page.IsPostBack)
        {
            string cmd = this.CurrentCommand;
            PCCore.Common.HRLog.RecordLog("Current Command: " + cmd);
            if (cmd == String.Empty || cmd == "")
                setData();
            else
            {
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
        }
        else
        {
            //this.InitDropDownList();
            
            setData();

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
        switch (e.Row.RowType)
        {
            case DataControlRowType.DataRow:

                Button btnDelete = new Button();
                btnDelete.CommandName = "delete";
                btnDelete.ID = "btnDelete";
                btnDelete.Text = Resources.Labels.Delete;
                
                Image _img = new Image();
                _img.ID = "img" + e.Row.Cells[0].Text.ToString();
                _img.ImageUrl = e.Row.Cells[1].Text;
                
                
                e.Row.Cells[4].Controls.Add(_img);
                e.Row.Cells[5].Controls.Add(btnDelete);
                
                break;
        }
    }


    protected void gvData_Command(object sender, GridViewCommandEventArgs e)
    {
        GridViewRow row = ((Control)e.CommandSource).NamingContainer as GridViewRow;
        if (e.CommandName == "delete")
        {
            DeleteRecord(row.Cells[1].ToString());
        }
            
    }

        private void CreateSecondHeader()
        {
            GridViewRow row = new GridViewRow(0, -1, DataControlRowType.Header, DataControlRowState.Normal);
            row.Cells.Add(new TableHeaderCell());
            row.Cells.Add(new TableHeaderCell());
            row.Cells.Add(new TableHeaderCell());
            TableCell left = new TableHeaderCell();
            left.ColumnSpan = 3;
            left.Text = "Section 1";
            left.BackColor = System.Drawing.Color.Blue;
            row.Cells.Add(left);

            TableCell totals = new TableHeaderCell();
            totals.ColumnSpan = 3;
            totals.Text = "Section 2";
            totals.BackColor = System.Drawing.Color.Blue;
            row.Cells.Add(totals);
            
            this.InnerTable.Rows.AddAt(0, row);
        }
    protected Table InnerTable
        {
            get
            {
                if (this.HasControls())
                {
                    
                    return (Table)gvData.Controls[0];
                }

                return null;
            
        }

    }
    private void Save()
    {

    }

    protected void btnSumbit_Click(object sender, EventArgs e)
    {
        string returnPath;
        string filename;
        PCCore.Common.PMRptImages _pmrptImage = new PCCore.Common.PMRptImages(uploadImg);
        returnPath =  _pmrptImage.Save(PCCore.Common.PMRptImages.SESSION);

        
        int _newLine;
        if (returnPath != String.Empty)
        {
            filename = _pmrptImage._imgFile.Name;
            _newLine = PCCore.PCMS.PMReport.NewLineNumber(_htControl);
            PCCore.PCMS.PMReportTable.EmptyRow(ref _htControl,PCCore.PCMS.PMReportTable.SE_HT_FIELDS_PMR_Images,_newLine.ToString());
            try
            {
                //public static string SE_HT_FIELDS_PMR_Images = PMR_COMM_Project_Code + "," + PMR_COMM_ID + "," + PMR_COMM_PCMS_DOCNUM + "," + PMR_COMM_Line_Number + "," + PMR_COMM_Description + "," + PMR_IMG_PATH;
                ((Hashtable)_htControl[_newLine.ToString()])[PCCore.PCMS.PMReportTable.PMR_MSTR_Project_Code] = this.projcode;
                ((Hashtable)_htControl[_newLine.ToString()])[PCCore.PCMS.PMReportTable.PMR_COMM_ID] = this.id;
                ((Hashtable)_htControl[_newLine.ToString()])[PCCore.PCMS.PMReportTable.PMR_COMM_Line_Number] = _newLine;
                ((Hashtable)_htControl[_newLine.ToString()])[PCCore.PCMS.PMReportTable.PMR_IMG_PATH] = returnPath;
                ((Hashtable)_htControl[_newLine.ToString()])[PCCore.PCMS.PMReportTable.PMR_IMG_DESCRIPTION] = this.txtDesc.Text;
                ((Hashtable)_htControl[_newLine.ToString()])[PCCore.PCMS.PMReportTable.PMR_IMG_FILENAME] = filename;
                ((Hashtable)_htControl[_newLine.ToString()])[PCCore.PCMS.PMReportTable.PMR_MSTR_PCMS_DOCNUM] = "";
                ViewState[dtControlName] = _htControl;
               
                PCCore.Common.XMLPMSession.Write(dtControlName, _htControl);
                this.CurrentCommand = "";
                PCCore.Common.HRLog.RecordLog("Add");
                PCCore.Common.HRLog.RecordLog("Path:" + HttpContext.Current.Request.Url.AbsoluteUri);
                refresh();
            } catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Submit to HashTable",ex);
            }
        }

    }
    
    
    private void DeleteRecord(string key)

    {
        PCCore.Common.HRLog.RecordLog("DeleteRecord: " + key);
        try
        {
            if (_htControl.ContainsKey(key))
                _htControl.Remove(key);
        }
        catch (Exception ex)
        {
            PCCore.Common.HRLog.RecordException("Delete Record", ex);
        }
        PCCore.Common.XMLPMSession.Write(dtControlName, _htControl);
        refresh();
    }

    void refresh()
    {

        Response.Redirect(HttpContext.Current.Request.Url.AbsoluteUri);
    }
    

}
