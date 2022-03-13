
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
using SimpleControls;
using SimpleControls.Web;
using PCCore;

//add by Martin
using System.Data.SqlClient;
using System.Data.Common;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

using System.Collections.Generic;

public partial class ExportMasterForm : MasterBasePage, IErrorHandler
{
    bool _automaticMode = true;
    public bool AutomaticMode
    {
        get { return _automaticMode; }
        set { _automaticMode = value; }
    }

    public string RedirectUrl
    {
        get { return SessionInfo.RedirectUrl; }
        set { SessionInfo.RedirectUrl = value; }
    }
    protected override void OnInit(EventArgs e)
    {
        FormMode = (FormModes)SimpleUtils.IfNull(Request.QueryString[Consts.QueryStringMode], typeof(FormModes), FormModes.None);
        RecordID = Request.QueryString[Consts.QueryStringID];
        RecordPID = Request.QueryString[Consts.QueryStringPID];
        SessionInfo.RedirectUrl = "";
        base.OnInit(e);

    }

    const string DISPLAY_ONLY = "DisplayOnly";
    protected bool DisplayOnly
    {
        get { return SimpleUtils.IfNull(ViewState[DISPLAY_ONLY], false); }
        set { ViewState[DISPLAY_ONLY] = value; }
    }

    protected void SetToDisplay()
    {
        FormMode = FormModes.Display;
        DisplayOnly = true;
        if (_automaticMode)
        {
            DisableForm();
        }
    }

    private void InitSecurity()
    {
        
        //switch (FormMode)
        //{
        //    case FormModes.New:
        //        if (!SecurityInfo.CanNew)
        //        {
        //            SetToDisplay();
        //        }
        //        else
        //        {
        //            if (!String.IsNullOrEmpty(ParentTableName))
        //            {
        //                if (String.IsNullOrEmpty(RecordPID))
        //                {
        //                    SetToDisplay();
        //                }
        //                else
        //                {
        //                    switch (SecurityInfo.PublishType) // check publish type
        //                    {
        //                        case Sc.PublishTypes.Private:
        //                        case Sc.PublishTypes.PrivateUpdate:
        //                            string sql = String.Format("SELECT CREATEDBY FROM {0} WHERE ID={1}", ParentTableName, RecordPID);
        //                            DataTable dt = PCDb.Db.ExecQuery(sql);
        //                            if (dt != null && dt.Rows.Count > 0)
        //                            {
        //                                DataRow dr = dt.Rows[0];
        //                                string createdBy = SimpleUtils.IfNull(dr[Consts.FieldCreatedBy], String.Empty);
        //                                if (createdBy != SessionInfo.LoginName)
        //                                {
        //                                    SetToDisplay();
        //                                }
        //                            }
        //                            else
        //                            {
        //                                SetToDisplay();
        //                            }
        //                            break;
        //                    }
        //                }
        //            }
        //        }
        //        break;
        //    case FormModes.Edit:
        //        if (!SecurityInfo.CanEdit)
        //        {
        //            SetToDisplay();
        //        }
        //        else
        //        {
        //            switch (SecurityInfo.PublishType) // check publish type
        //            {
        //                case Sc.PublishTypes.Private:
        //                case Sc.PublishTypes.PrivateUpdate:
        //                    if (_dr != null)
        //                    {
        //                        string createdBy = SimpleUtils.IfNull(_dr[Consts.FieldCreatedBy], String.Empty);
        //                        if (createdBy != SessionInfo.LoginName)
        //                        {
        //                            SetToDisplay();
        //                        }
        //                    }
        //                    else //postback
        //                    {
        //                        if (DisplayOnly)
        //                        {
        //                            SetToDisplay();
        //                        }
        //                    }
        //                    break;
        //            }
        //        }
        //        break;
        //}

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //the position is very important, must after content page's init/load
        InitSecurity();
        // Check Download Manager Permission to enable Download Request Permission
        if (PCCore.PCMS.Authorization.CanAccess(Consts.Function_DownloadManager)) 
        
            btnRequest.Enabled=true;
        
        else
        
            btnRequest.Enabled=false;

        /*
        if (refreshData)
        {
            if (this.Current_Command == Consts.ButtonExport)
            {
                Response.Redirect("")
            }
        }*/
        //set title after InitSecurity, because formMode may changed to display
        //Page.Header.Title = String.Format("{0} {1}", HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, FormMode.ToString()), _title);

        if (Note.IsEmplty)
        {
            switch (FormMode)
            {
                case FormModes.New:
                    ShowMessage(Resources.Common.AddingRecord);
                    break;
                case FormModes.Edit:
                    ShowMessage(Resources.Common.EditingRecord);
                    break;
                case FormModes.Display:
                    ShowMessage(Resources.Common.DisplayingRecord);
                    break;
            }
        }
        if (Page.IsPostBack)
        {
            try
            {
                PCCore.DropDownList _ddlProject = (PCCore.DropDownList)ContentPlaceHolder.FindControl("ddlProject");
            //PCCore.TextBox _txtProjCode = (PCCore.TextBox)Master.FindControl("invProjCode");
                if (invProjCode.Text != String.Empty)
                {
                    _ddlProject.SelectedValue = invProjCode.Text;
                    
                    invProjCode.Text = String.Empty;
                    
                }
            } catch (Exception ex)
            {

            }
        }
    }

    #region Error Handling
    public override void HandleError(Exception ex)
    {
        Note.HRShowException(ex);
    }

    public override void ClearError()
    {
        Note.Clear();
    }

    public override void ShowMessage(string message)
    {
        ShowMessage(message, null);
    }

    public override void ShowMessage(string message, string tooltip)
    {
        Note.ShowMessage(message, tooltip);
    }

    public override void ShowWarning(string warning)
    {
        ShowWarning(warning, null);
    }

    public override void ShowWarning(string warning, string tooltip)
    {
        Note.ShowWarning(warning, tooltip);
    }

    #endregion Error Handling

    #region Save Event
    public delegate void SaveEventHandler(object sender, System.EventArgs e);
    public event SaveEventHandler Save;

    protected void btnSave_Click(object sender, EventArgs e)
    {
        PCCore.Common.HRLog.RecordLog("btnSave_Click");
        PCCore.Common.HRLog.RecordLog("FormMode");
        PCCore.Common.HRLog.RecordLog(FormMode);
        PCCore.Common.HRLog.RecordLog("CanNew");
        PCCore.Common.HRLog.RecordLog(SecurityInfo.CanNew);
        switch (FormMode)
        {
            case FormModes.New:
                if (!SecurityInfo.CanNew) return;
                break;
            case FormModes.Edit:
                if (!SecurityInfo.CanEdit) return;
                break;
            default:
                return;
        }
        PCCore.Common.HRLog.RecordLog("Save");
        if (Save != null)
        {
            Save(sender, e);
        }
    }
    #endregion Save Event
    
    public override void DisableForm()
    {
        //btnSave.Enabled = false;
        foreach (Control c in this.ContentPlaceHolder.Controls)
        {
            switch (c.GetType().Name)
            {
                case "Label":
                case "Literal":
                case "LiteralControl":
                case "Note":
                case "HtmlTableCell":
                case "control_formbutton_ascx":
                case "SimpleValidationSummary":
                    break;
                case "TextBox":
                case "SimpleTextBox":
                    System.Web.UI.WebControls.TextBox tb = c as System.Web.UI.WebControls.TextBox;
                    tb.Enabled = false;
                    break;

                case "DropDownList":
                    System.Web.UI.WebControls.DropDownList ddl = c as System.Web.UI.WebControls.DropDownList;
                    ddl.Enabled = false;
                    break;

                case "GridView":
                case "SimpleGridView":
                    System.Web.UI.WebControls.GridView gv = c as System.Web.UI.WebControls.GridView;
                    gv.Enabled = false;
                    break;

                case "ToolBar":
                    PCCore.ToolBar Tb = c as PCCore.ToolBar;
                    Tb.Visible = false;
                    break;

                case "CheckBox":
                    System.Web.UI.WebControls.CheckBox chk2 = c as System.Web.UI.WebControls.CheckBox;
                    chk2.Enabled = true;
                    break;
                case "HrCheckBox":
                    System.Web.UI.WebControls.CheckBox chk = c as System.Web.UI.WebControls.CheckBox;
                    chk.Enabled = true;
                    break;

                case "Image":
                    System.Web.UI.WebControls.Image img = c as System.Web.UI.WebControls.Image;
                    img.Visible = false;
                    break;

                //case "control_staffsearch_ascx":
                //    StaffSearch ss = c as StaffSearch;
                //    ss.Enabled = false;
                //    ss.StaffIDEnabled = false;
                //    break;

                case "HtmlImage":
                    System.Web.UI.HtmlControls.HtmlImage htmlimage = c as System.Web.UI.HtmlControls.HtmlImage;
                    htmlimage.Visible = false;
                    break;

                //case "control_choseccallocation_ascx":
                //    ChoseCCAllocation allo = c as ChoseCCAllocation;
                //    allo.CCisDisabled = true;
                //    break;

                case "HtmlGenericControl":
                    System.Web.UI.HtmlControls.HtmlGenericControl htmlgen = c as System.Web.UI.HtmlControls.HtmlGenericControl;
                    htmlgen.Visible = true;
                    break;

                case "control_updateloadfile_ascx":
                    UpdateLoadFile upload = c as UpdateLoadFile;
                    upload.UserDefineisDisabled = false;
                    break;

                case "RadioButtonList":
                    System.Web.UI.WebControls.RadioButtonList list = c as System.Web.UI.WebControls.RadioButtonList;
                    list.Enabled = false;
                    break;

                //case "control_customizingfield_ascx":
                //    CustomizingField cf = c as CustomizingField;
                //    DisableCustomizingField(cf);
                //    break;

                case "HtmlTableRow":
                    System.Web.UI.HtmlControls.HtmlTableRow row = c as System.Web.UI.HtmlControls.HtmlTableRow;
                    DisableHtmlTableRow(row);
                    break;
                case "Panel":
                    System.Web.UI.WebControls.Panel panel = c as System.Web.UI.WebControls.Panel;
                    DisablePanel(panel);
                    //panel.Enabled = false;
                    break;
                default:
                    break;
            }
        }
    }
    
    void DisableHtmlTableRow(HtmlTableRow tmprow)
    {
        foreach (Control subrow in tmprow.Controls)
        {
            foreach (Control subc in subrow.Controls)
            {
                switch (subc.GetType().Name)
                {
                    case "Label":
                    case "Literal":
                    case "LiteralControl":
                    case "Note":
                    case "HtmlTableCell":
                    case "control_formbutton_ascx":
                    case "SimpleValidationSummary":
                        break;
                    case "TextBox":
                    case "SimpleTextBox":
                        System.Web.UI.WebControls.TextBox stb = subc as System.Web.UI.WebControls.TextBox;
                        stb.Enabled = false;
                        break;

                    case "DropDownList":
                        System.Web.UI.WebControls.DropDownList sddl = subc as System.Web.UI.WebControls.DropDownList;
                        sddl.Enabled = false;
                        break;

                    case "GridView":
                    case "SimpleGridView":
                        System.Web.UI.WebControls.GridView sgv = subc as System.Web.UI.WebControls.GridView;
                        sgv.Enabled = false;
                        break;

                    case "ToolBar":
                        PCCore.ToolBar sTb = subc as PCCore.ToolBar;
                        sTb.Visible = false;
                        break;

                    case "CheckBox":
                        System.Web.UI.WebControls.CheckBox schk2 = subc as System.Web.UI.WebControls.CheckBox;
                        schk2.Enabled = true;
                        break;
                    case "HrCheckBox":
                        System.Web.UI.WebControls.CheckBox schk = subc as System.Web.UI.WebControls.CheckBox;
                        schk.Enabled = true;
                        break;

                    case "Image":
                        System.Web.UI.WebControls.Image simg = subc as System.Web.UI.WebControls.Image;
                        simg.Visible = false;
                        break;

                    //case "control_staffsearch_ascx":
                    //    StaffSearch ss = subc as StaffSearch;
                    //    ss.Enabled = false;
                    //    ss.StaffIDEnabled = false;
                    //    break;

                    case "HtmlImage":
                        System.Web.UI.HtmlControls.HtmlImage htmlimage = subc as System.Web.UI.HtmlControls.HtmlImage;
                        htmlimage.Visible = false;
                        break;

                    //case "control_choseccallocation_ascx":
                    //    ChoseCCAllocation allo = subc as ChoseCCAllocation;
                    //    allo.CCisDisabled = true;
                    //    break;

                    case "HtmlGenericControl":
                        System.Web.UI.HtmlControls.HtmlGenericControl htmlgen = subc as System.Web.UI.HtmlControls.HtmlGenericControl;
                        htmlgen.Visible = true;
                        break;

                    case "control_updateloadfile_ascx":
                        UpdateLoadFile upload = subc as UpdateLoadFile;
                        upload.UserDefineisDisabled = false;
                        break;

                    case "RadioButtonList":
                        System.Web.UI.WebControls.RadioButtonList list = subc as System.Web.UI.WebControls.RadioButtonList;
                        list.Enabled = false;
                        break;

                    //case "control_customizingfield_ascx":
                    //    CustomizingField cf = subc as CustomizingField;
                    //    DisableCustomizingField(cf);
                    //    break;

                    case "HtmlTableRow":
                        System.Web.UI.HtmlControls.HtmlTableRow row = subc as System.Web.UI.HtmlControls.HtmlTableRow;
                        DisableHtmlTableRow(row);
                        break;
                    case "Panel":
                        System.Web.UI.WebControls.Panel panel = subc as System.Web.UI.WebControls.Panel;
                        DisablePanel(panel);
                        break;
                    default:
                        break;
                }
            }
        }
    }

    void DisablePanel(System.Web.UI.WebControls.Panel tmppanel)
    {
        foreach (Control subc in tmppanel.Controls)
        {
            switch (subc.GetType().Name)
            {
                case "Label":
                case "Literal":
                case "LiteralControl":
                case "Note":
                case "HtmlTableCell":
                case "control_formbutton_ascx":
                case "SimpleValidationSummary":
                    break;
                case "TextBox":
                case "SimpleTextBox":
                    System.Web.UI.WebControls.TextBox stb = subc as System.Web.UI.WebControls.TextBox;
                    stb.Enabled = false;
                    break;

                case "DropDownList":
                    System.Web.UI.WebControls.DropDownList sddl = subc as System.Web.UI.WebControls.DropDownList;
                    sddl.Enabled = false;
                    break;

                case "GridView":
                case "SimpleGridView":
                    System.Web.UI.WebControls.GridView sgv = subc as System.Web.UI.WebControls.GridView;
                    sgv.Enabled = false;
                    break;

                case "ToolBar":
                    PCCore.ToolBar sTb = subc as PCCore.ToolBar;
                    sTb.Visible = false;
                    break;

                case "CheckBox":
                    System.Web.UI.WebControls.CheckBox schk2 = subc as System.Web.UI.WebControls.CheckBox;
                    schk2.Enabled = true;
                    break;
                case "HrCheckBox":
                    System.Web.UI.WebControls.CheckBox schk = subc as System.Web.UI.WebControls.CheckBox;
                    schk.Enabled = true;
                    break;

                case "Image":
                    System.Web.UI.WebControls.Image simg = subc as System.Web.UI.WebControls.Image;
                    simg.Visible = false;
                    break;

                //case "control_staffsearch_ascx":
                //    StaffSearch ss = subc as StaffSearch;
                //    ss.Enabled = false;
                //    ss.StaffIDEnabled = false;
                //    break;

                case "HtmlImage":
                    System.Web.UI.HtmlControls.HtmlImage htmlimage = subc as System.Web.UI.HtmlControls.HtmlImage;
                    htmlimage.Visible = false;
                    break;

                //case "control_choseccallocation_ascx":
                //    ChoseCCAllocation allo = subc as ChoseCCAllocation;
                //    allo.CCisDisabled = true;
                //    break;

                case "HtmlGenericControl":
                    System.Web.UI.HtmlControls.HtmlGenericControl htmlgen = subc as System.Web.UI.HtmlControls.HtmlGenericControl;
                    htmlgen.Visible = true;
                    break;

                case "control_updateloadfile_ascx":
                    UpdateLoadFile upload = subc as UpdateLoadFile;
                    upload.UserDefineisDisabled = false;
                    break;

                case "RadioButtonList":
                    System.Web.UI.WebControls.RadioButtonList list = subc as System.Web.UI.WebControls.RadioButtonList;
                    list.Enabled = false;
                    break;

                //case "control_customizingfield_ascx":
                //    CustomizingField cf = subc as CustomizingField;
                //    DisableCustomizingField(cf);
                //    break;

                case "HtmlTableRow":
                    System.Web.UI.HtmlControls.HtmlTableRow row = subc as System.Web.UI.HtmlControls.HtmlTableRow;
                    DisableHtmlTableRow(row);
                    break;
                case "Panel":
                    System.Web.UI.WebControls.Panel panel = subc as System.Web.UI.WebControls.Panel;
                    DisablePanel(panel);
                    break;
                default:
                    break;
            }
        }
    }
    

    public override void ClearForm()
    {
        foreach (Control c in this.ContentPlaceHolder.Controls)
        {
            switch (c.GetType().Name)
            {
                case "TextBox":
                case "SimpleTextBox":
                    System.Web.UI.WebControls.TextBox tb = c as System.Web.UI.WebControls.TextBox;
                    tb.Text = String.Empty;
                    break;
                case "CheckBox":
                    System.Web.UI.WebControls.CheckBox chk = c as System.Web.UI.WebControls.CheckBox;
                    chk.Checked = true;
                    break;
            }
        }
    }
    //Button Save
    /*
    public Button ButtonSave
    {
        get { return btnSave; }
    }
    */
    public Button ButtonClose
    {
        get { return btnClose; }
    }

    #region Title
    string _title = String.Empty;
    public string Title
    {
        get { return _title; }
        set { _title = value; }
    }
    #endregion Title

    #region Record
    DataRow _dr = null;
    public DataRow Record
    {
        get { return _dr; }
        set { _dr = value; }
    }
    #endregion Record

    #region ParentTableName
    string _parentTableName = String.Empty;
    public string ParentTableName
    {
        get { return _parentTableName; }
        set { _parentTableName = value; }
    }
    #endregion ParentTableName

    //add by kamte 2006/05/07
    #region Delete

    public override void DeleteRecord(string tableName, PCCore.GridView gridView, CheckDeletePrerequisite checkPrerequisite)
    {
        PCTable table = new PCTable(tableName, this.SecurityInfo);
        try
        {
            table.BeginTransaction();
            Hashtable row = new Hashtable();
            row.Add(Consts.FieldID, String.Empty);
            if (gridView.AllowMultipleSelect)
            {
                ICollection ic = gridView.SelectedRowValuesCollection;
                IEnumerator ie = ic.GetEnumerator();
                ie.Reset();
                while (ie.MoveNext())
                {
                    row[Consts.FieldID] = ie.Current;
                    if (checkPrerequisite != null)
                        checkPrerequisite(table, row);
                    table.Delete(row);
                }
            }
            else
            {
                row[Consts.FieldID] = gridView.SelectedRowValue;
                if (checkPrerequisite != null)
                    checkPrerequisite(table, row);
                table.Delete(row);
            }
            table.CommitTransaction();
            ShowMessage(Resources.Common.DeleteOK);
        }
        catch (Exception ex)
        {
            gridView.ClearSelectedRowValues = false;
            HandleError(ex);
            table.RollbackTransaction();
        }
    }
    #endregion Delete

    public PCCore.PopNote MasterNote
    {
        get { return Note; }
    }

    
    public static string CheckForm(UpdateLoadFile UpdateLoad,FormModes FormMode)
    {
        string errMessage = "";
        //if (string.IsNullOrEmpty(txtFileName.Text.Trim()))
        //{
        //    Master.ShowMessage(Resources.Messages.InputFileName);
        //    txtFileName.Focus();
        //    return false;
        //}

        if (FormMode == ExportMasterForm.FormModes.New)
        {
            
            //MasterForm.ShowWarning(UpdateLoad.UploadFile.Count.ToString());
            if (UpdateLoad.UploadFile.Count < 1)
            {
                errMessage =  Resources.Messages.UploadFile;
                return errMessage;
            }
            else
            {
                if (UpdateLoad.UploadFile[0].ContentLength <= 0)
                {
                    errMessage =  Resources.Messages.UploadFile;
                    return errMessage;//return false;
                }
                PCCore.Common.HRLog.RecordLog("FileName: " + UpdateLoad.UploadFile[0].FileName.ToString());
                PCCore.Common.HRLog.RecordLog("File Contenttype: " + UpdateLoad.UploadFile[0].ContentType.ToString());

                if (UpdateLoad.UploadFile[0].ContentType != "application/vnd.ms-excel")
                {
                    errMessage = Resources.Messages.UploadExcelFile;
                    return errMessage;//return false;
                }
            }
            //



        }

        //if (Master.FormMode == MasterForm.FormModes.Edit)
        //{
        //    if (UpdateLoad.UploadFile.Count < 1 && UpdateLoad.NeedDeleteRecordID !="")
        //    {
        //        Master.ShowWarning(Resources.Messages.UploadFile);
        //        return false;
        //    }
        //}

        return errMessage;
    }


    public string ProjectCode
    {
        set { invProjCode.Text = value; }
        get { return invProjCode.Text; }
    }
    //public void startUpdate()
    //{
    //    this.tblanimation.Style["display"] = "";
    //}
    //public void endUpdate()
    //{
    //    this.tblanimation.Style["display"] = "none";
    //}
    protected string IntegerToColumnName(int a)
    {
        string firstchar = "";
        string secondchar = "";

        if (a > 52)
        {
            secondchar = Convert.ToChar(a - 52 + 64).ToString();
            firstchar = "B";
            firstchar = firstchar + secondchar;
        }
        else if (a > 26)
        {
            secondchar = Convert.ToChar(a - 26 + 64).ToString();
            firstchar = "A";
            firstchar = firstchar + secondchar;
        }
        else
            firstchar = Convert.ToChar(a + 64).ToString(); ;
        return firstchar;
    }
    protected void btnRequest_Click(object sender, EventArgs e)
    {
        System.Threading.Thread.Sleep(3000);
        //add by Martin 2011 Dec 6
        PCCore.TextBox cutoffTextBox = (PCCore.TextBox)ContentPlaceHolder.FindControl("txtcutoff");
        PCCore.DropDownList sectionCode = (PCCore.DropDownList)ContentPlaceHolder.FindControl("ddlSectionCode");
        //if (string.IsNullOrEmpty(cutoffTextBox.Text))
        //    return;
        //if (sectionCode.SelectedIndex == 0)
        //    return;
        //add by Martin
        Random ranObj = new Random();
        int start = 1;
        int end = 100;
        int number = ranObj.Next(start, end);

        // Add by Martin
        string[] dbs = null;
        dbs = Config.SAPDatabase.Split(new char[] { ',' });

        string _connectionString = ConfigurationManager.ConnectionStrings["SAP"].ConnectionString;
        _connectionString = string.Format(_connectionString, dbs[0]);


        object Missing = System.Type.Missing;
        Excel._Workbook oWB = null;
        Excel._Worksheet oSheet = null;
        Excel._Worksheet oSheet2 = null;
        Excel.Range oRng = null;
        Excel.Range oRng2 = null;
        Excel.Application oXL = new Excel.Application();

        //PCCore.Common.HRLog.RecordLog("Before new application");
        /*
        try
        {
            oXL = new Excel.Application();
        }
        catch (Exception ex)
        {
            //PCCore.Common.HRLog.RecordLog("error");
            //PCCore.Common.HRLog.RecordLog(ex.Message);
        }
        */
        //if(oXL == null)
        //PCCore.Common.HRLog.RecordLog("application is null");
        //else
        //PCCore.Common.HRLog.RecordLog("what happen");
        try
        {
            //PCCore.Common.HRLog.RecordLog("new application");

            oXL.Visible = false;
            oXL.DisplayAlerts = false;

            //Get a new workbook.
            oWB = (Excel._Workbook)(oXL.Workbooks.Add(Missing));
            oSheet = (Excel._Worksheet)oWB.ActiveSheet;
            //oSheet.Name = "A";

            int intRow = 3;
            //int I = 1;

            string userid = "";
            using (SqlConnection connection = new SqlConnection(_connectionString))
            {
                try
                {
                    SqlCommand command = new SqlCommand("sp_MA08_Export_Fiscal_Report_Data", connection);
                    command.CommandType = CommandType.StoredProcedure;

                    //add by Martin 2011 Dec 6
                    command.Parameters.Add(new SqlParameter("@SecCode", sectionCode.SelectedValue));
                    //add by Martin
                    command.Parameters.Add(new SqlParameter("@CutOffDate", Convert.ToDateTime(cutoffTextBox.Text).ToString("yyyy/MM/dd")));

                    connection.Open();
                    command.Connection = connection;

                    string previousShortName = "";
                    string previousTurnEarn = "";
                    string previousPrjStatus = "";
                    int current_line = 3;
                    int total_line = 0;
                    int date_line = 0;
                    Boolean IsFirst = false;
                    List<int> total_list = new List<int>();
                    List<int> total_list2 = new List<int>();
                    Dictionary<String, DateTime> column_list = new Dictionary<String, DateTime>();
                    Dictionary<String, DateTime> column_list2 = new Dictionary<String, DateTime>();
                    //List<DateTime> datetime_list = new List<DateTime>();
                    DateTime period1 = DateTime.Now;
                    DateTime period2 = DateTime.Now;
                    DateTime period3 = DateTime.Now;
                    DateTime period4 = DateTime.Now;
                    DateTime period5 = DateTime.Now;
                    DateTime period6 = DateTime.Now;
                    DateTime period7 = DateTime.Now;
                    DateTime period8 = DateTime.Now;
                    DateTime period9 = DateTime.Now;
                    DateTime period10 = DateTime.Now;
                    DateTime period11 = DateTime.Now;
                    DateTime period12 = DateTime.Now;
                    column_list.Add("B", DateTime.Now);
                    column_list.Add("C", DateTime.Now);
                    column_list.Add("D", DateTime.Now);
                    column_list.Add("E", DateTime.Now);
                    column_list.Add("F", DateTime.Now);
                    column_list.Add("G", DateTime.Now);
                    column_list.Add("H", DateTime.Now);
                    column_list.Add("I", DateTime.Now);
                    column_list.Add("J", DateTime.Now);
                    column_list.Add("K", DateTime.Now);
                    column_list.Add("L", DateTime.Now);
                    column_list.Add("M", DateTime.Now);
                    column_list2.Add("B", DateTime.Now);
                    column_list2.Add("C", DateTime.Now);
                    column_list2.Add("D", DateTime.Now);
                    column_list2.Add("E", DateTime.Now);
                    column_list2.Add("F", DateTime.Now);
                    column_list2.Add("G", DateTime.Now);
                    column_list2.Add("H", DateTime.Now);
                    column_list2.Add("I", DateTime.Now);
                    column_list2.Add("J", DateTime.Now);
                    column_list2.Add("K", DateTime.Now);
                    column_list2.Add("L", DateTime.Now);
                    column_list2.Add("M", DateTime.Now);
                    Boolean IsFirst2 = false;
                    int complement = 0;
                    Boolean reach_end = false;
                    int number_of_sheet = 0;

                    using (SqlDataReader datareader = command.ExecuteReader())
                    {
                        if (datareader.HasRows == true)
                        {
                            while (datareader.Read())
                            {
                                if (datareader["SectionShortName"] != System.DBNull.Value)
                                {
                                    if (!datareader["SectionShortName"].ToString().Equals(previousShortName))
                                    {
                                        oSheet = (Excel._Worksheet)oWB.Worksheets.Add(Missing, oSheet, 1, Missing);
                                        oSheet.Name = "Data-" + datareader["SectionShortName"].ToString();
                                        oSheet.Activate();
                                        current_line = 3;

                                        if (datareader["SectionName"] != System.DBNull.Value)
                                            oRng = (Excel.Range)oSheet.Cells[1, 1];

                                        //oSheet.Cells[1, 3] = "PRINT DATE : " + DateTime.Now.ToString("dd-mmm-yyyy");

                                        IsFirst = true;
                                        IsFirst2 = true;
                                        reach_end = false;
                                        previousShortName = datareader["SectionShortName"].ToString();
                                        number_of_sheet = number_of_sheet + 1;

                                        oSheet.Columns.HorizontalAlignment = 1;
                                        oSheet.Columns.VerticalAlignment = -4160;
                                        oSheet.Columns.Orientation = 0;
                                        oSheet.Columns.AddIndent = false;
                                        oSheet.Columns.IndentLevel = 0;
                                        oSheet.Columns.ShrinkToFit = false;
                                        oSheet.Columns.ReadingOrder = -5002;
                                        oSheet.Columns.MergeCells = false;
                                        oSheet.Columns.Font.Name = "Arial";
                                        oSheet.Columns.Font.FontStyle = "Normal";
                                        oSheet.Columns.Font.Size = 10;
                                        oSheet.Columns.Font.Strikethrough = false;
                                        oSheet.Columns.Font.Superscript = false;
                                        oSheet.Columns.Font.Subscript = false;
                                        oSheet.Columns.Font.OutlineFont = false;
                                        oSheet.Columns.Font.Shadow = false;
                                        oSheet.Columns.Font.Underline = -4142;
                                        oSheet.Columns.Font.ColorIndex = -4105;

                                        oRng.Font.Bold = true;

                                        oSheet.Cells[1, 1] = datareader["SectionName"].ToString();
                                    }
                                }
                                if (datareader["PrjStatus"] != System.DBNull.Value)
                                {
                                    if (!datareader["PrjStatus"].ToString().Equals(previousPrjStatus))
                                    {

                                        if (IsFirst == true)
                                        {
                                            current_line = current_line + 1;
                                            IsFirst = false;
                                        }
                                        else
                                            current_line = current_line + 5;


                                        oRng = (Excel.Range)oSheet.Cells[current_line, 1];
                                        oRng.Font.Bold = true;

                                        if (datareader["PrjStatus"].ToString() != "Total")
                                            current_line = current_line + 1;
                                        //else
                                        //complement = complement + 1;

                                        oSheet.Cells[current_line, 1] = datareader["PrjStatus"].ToString();

                                        previousPrjStatus = datareader["PrjStatus"].ToString();

                                        if (reach_end == false && IsFirst2 != true && total_list.Count > 0 && (previousPrjStatus == "Total" || previousPrjStatus == "Unsecured Projects"))
                                        {
                                            if (previousPrjStatus == "Unsecured Projects")
                                                reach_end = true;

                                            total_line = total_line + 2;
                                            int c = 2;
                                            int d = 2;
                                            string result = "";

                                            string previous_ch = "";
                                            foreach (KeyValuePair<string, DateTime> ch in column_list)
                                            {
                                                oRng = (Excel.Range)oSheet.Cells[total_line + 3, d];
                                                if (string.IsNullOrEmpty(previous_ch))
                                                    oRng.Formula = "=" + ch.Key.ToString() + (total_line).ToString();
                                                else
                                                    oRng.Formula = "=" + ch.Key.ToString() + (total_line).ToString() + "-" + previous_ch + (total_line).ToString();
                                                oRng.NumberFormatLocal = "#,##0;#,##0";
                                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                                oRng = (Excel.Range)oSheet.Cells[total_line + 4, d];
                                                if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(ch.Value).Days >= 0)
                                                {
                                                    if (string.IsNullOrEmpty(previous_ch))
                                                        oRng.Formula = "=" + ch.Key.ToString() + (total_line + 1).ToString();
                                                    else
                                                        oRng.Formula = "=" + ch.Key.ToString() + (total_line + 1).ToString() + "-" + previous_ch + (total_line + 1).ToString();
                                                }
                                                else
                                                    oRng.Formula = "";
                                                oRng.NumberFormatLocal = "#,##0;#,##0";
                                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                                d = d + 1;
                                                previous_ch = ch.Key.ToString();
                                                for (int i = 0; i < 3; i++)
                                                {
                                                    foreach (int line in total_list)
                                                    {
                                                        if (string.IsNullOrEmpty(result))
                                                            result = ch.Key + (line + i).ToString();
                                                        else
                                                            result = result + "+" + ch.Key + (line + i).ToString();
                                                    }
                                                    oRng = (Excel.Range)oSheet.Cells[total_line + i, c];
                                                    if (total_line + i == 54)
                                                        Console.Write("");
                                                    if (i == 2)
                                                    {
                                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(ch.Value).Days < 0)
                                                        {
                                                            //oRng = (Excel.Range)oSheet.Cells[total_line + i, c];
                                                            if (!string.IsNullOrEmpty(result))
                                                            {
                                                                oRng.Formula = "=" + result;
                                                                oRng.NumberFormatLocal = "#,##0;#,##0";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //oRng = (Excel.Range)oSheet.Cells[total_line + i, c];
                                                            oRng.Formula = "";
                                                        }
                                                    }
                                                    else if (i == 0 || i == 3)
                                                    {
                                                        //oRng = (Excel.Range)oSheet.Cells[total_line + i, c];
                                                        if (!string.IsNullOrEmpty(result))
                                                        {
                                                            oRng.Formula = "=" + result;
                                                            oRng.NumberFormatLocal = "#,##0;#,##0";
                                                        }
                                                    }
                                                    else
                                                    {
                                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(ch.Value).Days >= 0)
                                                        {
                                                            //oRng = (Excel.Range)oSheet.Cells[total_line + i, c];
                                                            if (!string.IsNullOrEmpty(result))
                                                            {
                                                                oRng.Formula = "=" + result;
                                                                oRng.NumberFormatLocal = "#,##0;#,##0";
                                                            }
                                                        }
                                                        else
                                                        {
                                                            //oRng = (Excel.Range)oSheet.Cells[total_line + i, c];
                                                            oRng.Formula = "";
                                                        }
                                                    }
                                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
                                                    result = "";
                                                }
                                                c = c + 1;
                                            }
                                            //foreach (int t in total_list)
                                            //total_list2.Add(t);
                                            //foreach (KeyValuePair<string, DateTime> ch in column_list)
                                            //column_list2.Add(ch.Key, ch.Value);

                                            column_list.Clear();
                                            total_list.Clear();
                                        }
                                        if (previousPrjStatus == "Total")
                                            total_line = current_line;

                                        current_line = current_line + 1;
                                        IsFirst2 = false;
                                    }
                                }

                                if (datareader["TurnEarn"] != System.DBNull.Value)
                                {
                                    //if (!datareader["TurnEarn"].ToString().Equals(previousTurnEarn))
                                    //{
                                    oSheet.Cells[current_line, 1] = datareader["TurnEarn"].ToString();
                                    previousTurnEarn = datareader["TurnEarn"].ToString();
                                    date_line = current_line;


                                    //}
                                }

                                if (datareader["Period1"] != System.DBNull.Value)
                                {
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 2];

                                    oRng = (Excel.Range)oSheet.Cells[date_line, 2];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng.NumberFormat = "MMM-yy";
                                    oRng.Value2 = Convert.ToDateTime(datareader["Period1"]);
                                    period1 = Convert.ToDateTime(datareader["Period1"]);
                                    column_list["B"] = period1;
                                }

                                if (datareader["Period2"] != System.DBNull.Value)
                                {
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 3];

                                    oRng = (Excel.Range)oSheet.Cells[date_line, 3];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;


                                    oRng.NumberFormat = "MMM-yy";
                                    oRng.Value2 = Convert.ToDateTime(datareader["Period2"]);
                                    period2 = Convert.ToDateTime(datareader["Period2"]);
                                    column_list["C"] = period2;
                                    column_list2["C"] = period2;
                                }

                                if (datareader["Period3"] != System.DBNull.Value)
                                {
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 4];

                                    oRng = (Excel.Range)oSheet.Cells[date_line, 4];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;


                                    oRng.NumberFormat = "MMM-yy";
                                    oRng.Value2 = Convert.ToDateTime(datareader["Period3"]);
                                    period3 = Convert.ToDateTime(datareader["Period3"]);
                                    column_list["D"] = period3;
                                }

                                if (datareader["Period4"] != System.DBNull.Value)
                                {
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 5];

                                    oRng = (Excel.Range)oSheet.Cells[date_line, 5];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng.NumberFormat = "MMM-yy";
                                    oRng.Value2 = Convert.ToDateTime(datareader["Period4"]);
                                    period4 = Convert.ToDateTime(datareader["Period4"]);
                                    column_list["E"] = period4;
                                }

                                if (datareader["Period5"] != System.DBNull.Value)
                                {
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 6];

                                    oRng = (Excel.Range)oSheet.Cells[date_line, 6];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng.NumberFormat = "MMM-yy";
                                    oRng.Value2 = Convert.ToDateTime(datareader["Period5"]);
                                    period5 = Convert.ToDateTime(datareader["Period5"]);
                                    column_list["F"] = period5;
                                }

                                if (datareader["Period6"] != System.DBNull.Value)
                                {
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 7];

                                    oRng = (Excel.Range)oSheet.Cells[date_line, 7];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng.NumberFormat = "MMM-yy";
                                    oRng.Value2 = Convert.ToDateTime(datareader["Period6"]);
                                    period6 = Convert.ToDateTime(datareader["Period6"]);
                                    column_list["G"] = period6;
                                }

                                if (datareader["Period7"] != System.DBNull.Value)
                                {
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 8];

                                    oRng = (Excel.Range)oSheet.Cells[date_line, 8];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng.NumberFormat = "MMM-yy";
                                    oRng.Value2 = Convert.ToDateTime(datareader["Period7"]);
                                    period7 = Convert.ToDateTime(datareader["Period7"]);
                                    column_list["H"] = period7;
                                }

                                if (datareader["Period8"] != System.DBNull.Value)
                                {
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 9];

                                    oRng = (Excel.Range)oSheet.Cells[date_line, 9];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng.NumberFormat = "MMM-yy";
                                    oRng.Value2 = Convert.ToDateTime(datareader["Period8"]);
                                    period8 = Convert.ToDateTime(datareader["Period8"]);
                                    column_list["I"] = period8;
                                }

                                if (datareader["Period9"] != System.DBNull.Value)
                                {
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 10];
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 10];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng.NumberFormat = "MMM-yy";
                                    oRng.Value2 = Convert.ToDateTime(datareader["Period9"]);
                                    period9 = Convert.ToDateTime(datareader["Period9"]);
                                    column_list["J"] = period9;
                                }

                                if (datareader["Period10"] != System.DBNull.Value)
                                {
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 11];
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 11];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng.NumberFormat = "MMM-yy";
                                    oRng.Value2 = Convert.ToDateTime(datareader["Period10"]);
                                    period10 = Convert.ToDateTime(datareader["Period10"]);
                                    column_list["K"] = period10;
                                }

                                if (datareader["Period11"] != System.DBNull.Value)
                                {
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 12];
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 12];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng.NumberFormat = "MMM-yy";
                                    oRng.Value2 = Convert.ToDateTime(datareader["Period11"]);
                                    period11 = Convert.ToDateTime(datareader["Period11"]);
                                    column_list["L"] = period11;
                                }

                                if (datareader["Period12"] != System.DBNull.Value)
                                {
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 13];
                                    oRng = (Excel.Range)oSheet.Cells[date_line, 13];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng.NumberFormat = "MMM-yy";
                                    oRng.Value2 = Convert.ToDateTime(datareader["Period12"]);
                                    period12 = Convert.ToDateTime(datareader["Period12"]);
                                    column_list["M"] = period12;
                                }

                                current_line = current_line + 1;
                                oRng = (Excel.Range)oSheet.Cells[current_line, 1];
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                oSheet.Cells[current_line, 1] = "Target";
                                current_line = current_line + 1;

                                oRng = (Excel.Range)oSheet.Cells[current_line, 1];
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                oSheet.Cells[current_line, 1] = "Actual";
                                current_line = current_line + 1;

                                oRng = (Excel.Range)oSheet.Cells[current_line, 1];
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                oSheet.Cells[current_line, 1] = "Forecast";
                                current_line = current_line + 1;

                                oRng = (Excel.Range)oSheet.Cells[current_line, 1];
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                oSheet.Cells[current_line, 1] = "Target increased";
                                current_line = current_line + 1;

                                oRng = (Excel.Range)oSheet.Cells[current_line, 1];
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                oSheet.Cells[current_line, 1] = "Actual increased";
                                current_line = current_line + 1;

                                current_line = current_line - 5;

                                int target_line = 0;
                                int actual_line = 0;

                                target_line = current_line;

                                if (previousPrjStatus != "Total")
                                {
                                    total_list.Add(current_line);
                                    if (datareader["Period1_IncTarYTD"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 2] = datareader["Period1_IncTarYTD"].ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 2];

                                        //oRng = (Excel.Range)oSheet.Cells[current_line, 2];
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 2] = "0";
                                    current_line = current_line + 1;

                                    actual_line = current_line;
                                    //if (total_list.Count == 1)
                                    //total_list.Add(current_line);
                                    if (datareader["Period1_IncActYTD"] != System.DBNull.Value)
                                    {
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 2];
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period1).Days >= 0)
                                            oSheet.Cells[current_line, 2] = Math.Round(Convert.ToDouble(datareader["Period1_IncActYTD"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 2] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 2];
                                        //oRng.NumberFormatLocal = "#,##0;#,##0";
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        oRng = (Excel.Range)oSheet.Cells[date_line, 2];
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;


                                        oRng = (Excel.Range)oSheet.Cells[date_line, 2];
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period1).Days >= 0)
                                            oSheet.Cells[current_line, 2] = "0";
                                        else
                                            oSheet.Cells[current_line, 2] = "";
                                    }
                                    current_line = current_line + 1;

                                    //if (total_list.Count == 2)
                                    //total_list.Add(current_line);
                                    if (datareader["Period1_IncYrForThisRpt"] != System.DBNull.Value)
                                    {
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 2];
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period1).Days >= 0)
                                            oSheet.Cells[current_line, 2] = "";
                                        else
                                            oSheet.Cells[current_line, 2] = Math.Round(Convert.ToDouble(datareader["Period1_IncYrForThisRpt"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 2];
                                        //oRng.NumberFormatLocal = "#,##0;#,##0";
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 2];
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                        oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period1).Days >= 0)
                                            oSheet.Cells[current_line, 2] = "";
                                        else
                                            oSheet.Cells[current_line, 2] = "0";
                                    }
                                    current_line = current_line + 1;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 2];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    // buster0
                                    //if (total_list.Count == 3)
                                    //total_list.Add(current_line);
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 2];
                                    oRng.Formula = "=B" + target_line;
                                    /*
                                    if (datareader["Period1_TargetIncrease"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 2] = datareader["Period1_TargetIncrease"].ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 2];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 2] = "0";
                                    */
                                    current_line = current_line + 1;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 2];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 2];

                                    //if (total_list.Count == 4)
                                    //total_list.Add(current_line);
                                    if (datareader["Period1_ActualIncrease"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period1).Days >= 0)
                                            oRng.Formula = "=B" + actual_line;
                                        //oSheet.Cells[current_line, 2] = datareader["Period1_ActualIncrease"].ToString();
                                        else
                                            oSheet.Cells[current_line, 2] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 2];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period1).Days >= 0)
                                            oSheet.Cells[current_line, 2] = "0";
                                        else
                                            oSheet.Cells[current_line, 2] = "";
                                    }
                                    current_line = current_line - 4;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 3];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    target_line = current_line;
                                    if (datareader["Period2_IncTarYTD"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 3] = Math.Round(Convert.ToDouble(datareader["Period2_IncTarYTD"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 3];
                                        //oRng.NumberFormatLocal = "#,##0;#,##0";
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 3] = "0";
                                    current_line = current_line + 1;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 3];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    actual_line = current_line;
                                    if (datareader["Period2_IncActYTD"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period2).Days >= 0)
                                            oSheet.Cells[current_line, 3] = Math.Round(Convert.ToDouble(datareader["Period2_IncActYTD"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 3] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 3];
                                        //oRng.NumberFormatLocal = "#,##0;#,##0";
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period2).Days >= 0)
                                            oSheet.Cells[current_line, 3] = "0";
                                        else
                                            oSheet.Cells[current_line, 3] = "";
                                    }
                                    current_line = current_line + 1;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 3];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    if (datareader["Period2_IncYrForThisRpt"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period2).Days >= 0)
                                            oSheet.Cells[current_line, 3] = "";
                                        else
                                            oSheet.Cells[current_line, 3] = Math.Round(Convert.ToDouble(datareader["Period2_IncYrForThisRpt"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 3];
                                        //oRng.NumberFormatLocal = "#,##0;#,##0";
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period2).Days >= 0)
                                            oSheet.Cells[current_line, 3] = "";
                                        else
                                            oSheet.Cells[current_line, 3] = "0";
                                    }
                                    current_line = current_line + 1;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 3];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 3];
                                    oRng.Formula = "=C" + target_line + "-B" + target_line;
                                    /*
                                    if (datareader["Period2_TargetIncrease"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 3] = datareader["Period2_TargetIncrease"].ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 3];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 3] = "0";
                                    */
                                    current_line = current_line + 1;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 3];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 3];
                                    if (datareader["Period2_ActualIncrease"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period2).Days >= 0)
                                            oRng.Formula = "=C" + actual_line + "-B" + actual_line;
                                        //oSheet.Cells[current_line, 3] = datareader["Period2_ActualIncrease"].ToString();
                                        else
                                            oSheet.Cells[current_line, 3] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 3];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period2).Days >= 0)
                                            oSheet.Cells[current_line, 3] = "0";
                                        else
                                            oSheet.Cells[current_line, 3] = "";
                                    }
                                    current_line = current_line - 4;

                                    //***

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 4];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    target_line = current_line;
                                    if (datareader["Period3_IncTarYTD"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 4] = Math.Round(Convert.ToDouble(datareader["Period3_IncTarYTD"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 4];
                                        //oRng.NumberFormatLocal = "#,##0;#,##0";
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 4] = "0";
                                    current_line = current_line + 1;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 4];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    actual_line = current_line;
                                    if (datareader["Period3_IncActYTD"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period3).Days >= 0)
                                            oSheet.Cells[current_line, 4] = Math.Round(Convert.ToDouble(datareader["Period3_IncActYTD"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 4] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 4];
                                        //oRng.NumberFormatLocal = "#,##0;#,##0";
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period3).Days >= 0)
                                            oSheet.Cells[current_line, 4] = "0";
                                        else
                                            oSheet.Cells[current_line, 4] = "";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 4];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    if (datareader["Period3_IncYrForThisRpt"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period3).Days >= 0)
                                            oSheet.Cells[current_line, 4] = "";
                                        else
                                            oSheet.Cells[current_line, 4] = Math.Round(Convert.ToDouble(datareader["Period3_IncYrForThisRpt"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 4];
                                        //oRng.NumberFormatLocal = "#,##0;#,##0";
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period3).Days >= 0)
                                            oSheet.Cells[current_line, 4] = "";
                                        else
                                            oSheet.Cells[current_line, 4] = "0";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 4];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 4];
                                    oRng.Formula = "=D" + target_line + "-C" + target_line;
                                    /*
                                    if (datareader["Period3_TargetIncrease"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 4] = Math.Round(Convert.ToDouble(datareader["Period3_TargetIncrease"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 4];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 4] = "0";*/
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 4];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 4];
                                    if (datareader["Period3_ActualIncrease"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period3).Days >= 0)
                                            oRng.Formula = "=D" + actual_line + "-C" + actual_line;
                                        //oSheet.Cells[current_line, 4] = Math.Round(Convert.ToDouble(datareader["Period3_ActualIncrease"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 4] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 4];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period3).Days >= 0)
                                            oSheet.Cells[current_line, 4] = "0";
                                        else
                                            oSheet.Cells[current_line, 4] = "";
                                    }
                                    current_line = current_line - 4;
                                    //***
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 5];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    target_line = current_line;
                                    if (datareader["Period4_IncTarYTD"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 5] = Math.Round(Convert.ToDouble(datareader["Period4_IncTarYTD"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 5];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 5] = "0";
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 5];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    actual_line = current_line;
                                    if (datareader["Period4_IncActYTD"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period4).Days >= 0)
                                            oSheet.Cells[current_line, 5] = Math.Round(Convert.ToDouble(datareader["Period4_IncActYTD"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 5] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 5];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period4).Days >= 0)
                                            oSheet.Cells[current_line, 5] = "0";
                                        else
                                            oSheet.Cells[current_line, 5] = "";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 5];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    if (datareader["Period4_IncYrForThisRpt"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period4).Days >= 0)
                                            oSheet.Cells[current_line, 5] = "";
                                        else
                                            oSheet.Cells[current_line, 5] = Math.Round(Convert.ToDouble(datareader["Period4_IncYrForThisRpt"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 5];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period4).Days >= 0)
                                            oSheet.Cells[current_line, 5] = "";
                                        else
                                            oSheet.Cells[current_line, 5] = "0";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 5];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 5];
                                    oRng.Formula = "=E" + target_line + "-D" + target_line;
                                    /*
                                    if (datareader["Period4_TargetIncrease"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 5] = Math.Round(Convert.ToDouble(datareader["Period4_TargetIncrease"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 5];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 5] = "0";*/
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 5];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 5];
                                    if (datareader["Period4_ActualIncrease"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period4).Days >= 0)
                                            oRng.Formula = "=E" + actual_line + "-D" + actual_line;
                                        //oSheet.Cells[current_line, 5] = Math.Round(Convert.ToDouble(datareader["Period4_ActualIncrease"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 5] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 5];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period4).Days >= 0)
                                            oSheet.Cells[current_line, 5] = "0";
                                        else
                                            oSheet.Cells[current_line, 5] = "";
                                    }
                                    current_line = current_line - 4;
                                    //***
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 6];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    target_line = current_line;
                                    if (datareader["Period5_IncTarYTD"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 6] = Math.Round(Convert.ToDouble(datareader["Period5_IncTarYTD"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 6];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 6] = "0";
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 6];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    actual_line = current_line;
                                    if (datareader["Period5_IncActYTD"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period5).Days >= 0)
                                            oSheet.Cells[current_line, 6] = Math.Round(Convert.ToDouble(datareader["Period5_IncActYTD"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 6] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 6];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period5).Days >= 0)
                                            oSheet.Cells[current_line, 6] = "0";
                                        else
                                            oSheet.Cells[current_line, 6] = "";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 6];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    if (datareader["Period5_IncYrForThisRpt"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period5).Days >= 0)
                                            oSheet.Cells[current_line, 6] = "";
                                        else
                                            oSheet.Cells[current_line, 6] = Math.Round(Convert.ToDouble(datareader["Period5_IncYrForThisRpt"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 6];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period5).Days >= 0)
                                            oSheet.Cells[current_line, 6] = "";
                                        else
                                            oSheet.Cells[current_line, 6] = "0";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 6];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 6];
                                    oRng.Formula = "=F" + target_line + "-E" + target_line;
                                    /*
                                    if (datareader["Period5_TargetIncrease"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 6] = Math.Round(Convert.ToDouble(datareader["Period5_TargetIncrease"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 6];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 6] = "0";*/
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 6];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 6];
                                    if (datareader["Period5_ActualIncrease"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period5).Days >= 0)
                                            oRng.Formula = "=F" + actual_line + "-E" + actual_line;
                                        //oSheet.Cells[current_line, 6] = Math.Round(Convert.ToDouble(datareader["Period5_ActualIncrease"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 6] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 6];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period5).Days >= 0)
                                            oSheet.Cells[current_line, 6] = "0";
                                        else
                                            oSheet.Cells[current_line, 6] = "";
                                    }
                                    current_line = current_line - 4;
                                    //***
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 7];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    target_line = current_line;
                                    if (datareader["Period6_IncTarYTD"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 7] = Math.Round(Convert.ToDouble(datareader["Period6_IncTarYTD"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 7];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 7] = "0";
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 7];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    actual_line = current_line;
                                    if (datareader["Period6_IncActYTD"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period6).Days >= 0)
                                            oSheet.Cells[current_line, 7] = Math.Round(Convert.ToDouble(datareader["Period6_IncActYTD"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 7] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 7];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period6).Days >= 0)
                                            oSheet.Cells[current_line, 7] = "0";
                                        else
                                            oSheet.Cells[current_line, 7] = "";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 7];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;



                                    if (datareader["Period6_IncYrForThisRpt"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period6).Days >= 0)
                                            oSheet.Cells[current_line, 7] = "";
                                        else
                                            oSheet.Cells[current_line, 7] = Math.Round(Convert.ToDouble(datareader["Period6_IncYrForThisRpt"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 7];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period6).Days >= 0)
                                            oSheet.Cells[current_line, 7] = "";
                                        else
                                            oSheet.Cells[current_line, 7] = "0";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 7];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;


                                    oRng = (Excel.Range)oSheet.Cells[current_line, 7];
                                    oRng.Formula = "=G" + target_line + "-F" + target_line;
                                    /*
                                    if (datareader["Period6_TargetIncrease"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 7] = Math.Round(Convert.ToDouble(datareader["Period6_TargetIncrease"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 7];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 7] = "0";*/
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 7];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 7];
                                    if (datareader["Period6_ActualIncrease"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period6).Days >= 0)
                                            oRng.Formula = "=G" + actual_line + "-F" + actual_line;
                                        //oSheet.Cells[current_line, 7] = Math.Round(Convert.ToDouble(datareader["Period6_ActualIncrease"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 7] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 7];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period6).Days >= 0)
                                            oSheet.Cells[current_line, 7] = "0";
                                        else
                                            oSheet.Cells[current_line, 7] = "";
                                    }
                                    current_line = current_line - 4;
                                    //***
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 8];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    target_line = current_line;
                                    if (datareader["Period7_IncTarYTD"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 8] = Math.Round(Convert.ToDouble(datareader["Period7_IncTarYTD"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 8];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 8] = "0";
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 8];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    actual_line = current_line;
                                    if (datareader["Period7_IncActYTD"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period7).Days >= 0)
                                            oSheet.Cells[current_line, 8] = Math.Round(Convert.ToDouble(datareader["Period7_IncActYTD"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 8] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 8];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period7).Days >= 0)
                                            oSheet.Cells[current_line, 8] = "0";
                                        else
                                            oSheet.Cells[current_line, 8] = "";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 8];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    if (datareader["Period7_IncYrForThisRpt"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period7).Days >= 0)
                                            oSheet.Cells[current_line, 8] = "";
                                        else
                                            oSheet.Cells[current_line, 8] = Math.Round(Convert.ToDouble(datareader["Period7_IncYrForThisRpt"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 8];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period7).Days >= 0)
                                            oSheet.Cells[current_line, 8] = "";
                                        else
                                            oSheet.Cells[current_line, 8] = "0";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 8];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 8];
                                    oRng.Formula = "=H" + target_line + "-G" + target_line;
                                    /*
                                    if (datareader["Period7_TargetIncrease"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 8] = Math.Round(Convert.ToDouble(datareader["Period7_TargetIncrease"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 8];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 8] = "0";*/
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 8];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 8];
                                    if (datareader["Period7_ActualIncrease"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period7).Days >= 0)
                                            oRng.Formula = "=H" + actual_line + "-G" + actual_line;
                                        //oSheet.Cells[current_line, 8] = Math.Round(Convert.ToDouble(datareader["Period7_ActualIncrease"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 8] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 8];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period7).Days >= 0)
                                            oSheet.Cells[current_line, 8] = "0";
                                        else
                                            oSheet.Cells[current_line, 8] = "";
                                    }
                                    current_line = current_line - 4;
                                    //***
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 9];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    target_line = current_line;
                                    if (datareader["Period8_IncTarYTD"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 9] = Math.Round(Convert.ToDouble(datareader["Period8_IncTarYTD"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 9];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 9] = "0";
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 9];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    actual_line = current_line;
                                    if (datareader["Period8_IncActYTD"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period8).Days >= 0)
                                            oSheet.Cells[current_line, 9] = Math.Round(Convert.ToDouble(datareader["Period8_IncActYTD"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 9] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 9];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period8).Days >= 0)
                                            oSheet.Cells[current_line, 9] = "0";
                                        else
                                            oSheet.Cells[current_line, 9] = "";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 9];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    if (datareader["Period8_IncYrForThisRpt"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period8).Days >= 0)
                                            oSheet.Cells[current_line, 9] = "";
                                        else
                                            oSheet.Cells[current_line, 9] = Math.Round(Convert.ToDouble(datareader["Period8_IncYrForThisRpt"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 9];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period8).Days >= 0)
                                            oSheet.Cells[current_line, 9] = "";
                                        else
                                            oSheet.Cells[current_line, 9] = "0";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 9];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 9];
                                    oRng.Formula = "=I" + target_line + "-H" + target_line;
                                    /*
                                    if (datareader["Period8_TargetIncrease"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 9] = Math.Round(Convert.ToDouble(datareader["Period8_TargetIncrease"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 9];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 9] = "0";*/
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 9];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 9];
                                    if (datareader["Period8_ActualIncrease"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period8).Days >= 0)
                                            oRng.Formula = "=I" + actual_line + "-H" + actual_line;
                                        //oSheet.Cells[current_line, 9] = Math.Round(Convert.ToDouble(datareader["Period8_ActualIncrease"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 9] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 9];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period8).Days >= 0)
                                            oSheet.Cells[current_line, 9] = "0";
                                        else
                                            oSheet.Cells[current_line, 9] = "";
                                    }
                                    current_line = current_line - 4;
                                    //***
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 10];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    target_line = current_line;
                                    if (datareader["Period9_IncTarYTD"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 10] = Math.Round(Convert.ToDouble(datareader["Period9_IncTarYTD"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 10];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 10] = "0";
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 10];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    actual_line = current_line;
                                    if (datareader["Period9_IncActYTD"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period9).Days >= 0)
                                            oSheet.Cells[current_line, 10] = Math.Round(Convert.ToDouble(datareader["Period9_IncActYTD"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 10] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 10];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period9).Days >= 0)
                                            oSheet.Cells[current_line, 10] = "0";
                                        else
                                            oSheet.Cells[current_line, 10] = "";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 10];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    if (datareader["Period9_IncYrForThisRpt"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period9).Days >= 0)
                                            oSheet.Cells[current_line, 10] = "";
                                        else
                                            oSheet.Cells[current_line, 10] = Math.Round(Convert.ToDouble(datareader["Period9_IncYrForThisRpt"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 10];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period9).Days >= 0)
                                            oSheet.Cells[current_line, 10] = "";
                                        else
                                            oSheet.Cells[current_line, 10] = "0";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 10];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 10];
                                    oRng.Formula = "=J" + target_line + "-I" + target_line;
                                    /*
                                    if (datareader["Period9_TargetIncrease"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 10] = Math.Round(Convert.ToDouble(datareader["Period9_TargetIncrease"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 10];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 10] = "0";*/
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 10];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 10];
                                    if (datareader["Period9_ActualIncrease"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period9).Days >= 0)
                                            oRng.Formula = "=J" + actual_line + "-I" + actual_line;
                                        //oSheet.Cells[current_line, 10] = Math.Round(Convert.ToDouble(datareader["Period9_ActualIncrease"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 10] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 10];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period9).Days >= 0)
                                            oSheet.Cells[current_line, 10] = "0";
                                        else
                                            oSheet.Cells[current_line, 10] = "";
                                    }
                                    current_line = current_line - 4;
                                    //***
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 11];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    target_line = current_line;
                                    if (datareader["Period10_IncTarYTD"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 11] = Math.Round(Convert.ToDouble(datareader["Period10_IncTarYTD"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 11];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 11] = "0";
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 11];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    actual_line = current_line;
                                    if (datareader["Period10_IncActYTD"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period10).Days >= 0)
                                            oSheet.Cells[current_line, 11] = Math.Round(Convert.ToDouble(datareader["Period10_IncActYTD"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 11] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 11];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period10).Days >= 0)
                                            oSheet.Cells[current_line, 11] = "0";
                                        else
                                            oSheet.Cells[current_line, 11] = "";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 11];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    if (datareader["Period10_IncYrForThisRpt"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period10).Days >= 0)
                                            oSheet.Cells[current_line, 11] = "";
                                        else
                                            oSheet.Cells[current_line, 11] = Math.Round(Convert.ToDouble(datareader["Period10_IncYrForThisRpt"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 11];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period10).Days >= 0)
                                            oSheet.Cells[current_line, 11] = "";
                                        else
                                            oSheet.Cells[current_line, 11] = "0";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 11];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 11];
                                    oRng.Formula = "=K" + target_line + "-J" + target_line;
                                    /*
                                    if (datareader["Period10_TargetIncrease"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 11] = Math.Round(Convert.ToDouble(datareader["Period10_TargetIncrease"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 11];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 11] = "0";*/
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 11];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 11];
                                    if (datareader["Period10_ActualIncrease"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period10).Days >= 0)
                                            oRng.Formula = "=K" + actual_line + "-J" + actual_line;
                                        //oSheet.Cells[current_line, 11] = Math.Round(Convert.ToDouble(datareader["Period10_ActualIncrease"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 11] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 11];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period10).Days >= 0)
                                            oSheet.Cells[current_line, 11] = "0";
                                        else
                                            oSheet.Cells[current_line, 11] = "";
                                    }
                                    current_line = current_line - 4;
                                    //***
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 12];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    target_line = current_line;
                                    if (datareader["Period11_IncTarYTD"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 12] = Math.Round(Convert.ToDouble(datareader["Period11_IncTarYTD"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 12];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 12] = "0";
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 12];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    actual_line = current_line;
                                    if (datareader["Period11_IncActYTD"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period11).Days >= 0)
                                            oSheet.Cells[current_line, 12] = Math.Round(Convert.ToDouble(datareader["Period11_IncActYTD"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 12] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 12];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period11).Days >= 0)
                                            oSheet.Cells[current_line, 12] = "0";
                                        else
                                            oSheet.Cells[current_line, 12] = "";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 12];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    if (datareader["Period11_IncYrForThisRpt"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period11).Days >= 0)
                                            oSheet.Cells[current_line, 12] = "";
                                        else
                                            oSheet.Cells[current_line, 12] = Math.Round(Convert.ToDouble(datareader["Period11_IncYrForThisRpt"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 12];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period11).Days >= 0)
                                            oSheet.Cells[current_line, 12] = "";
                                        else
                                            oSheet.Cells[current_line, 12] = "0";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 12];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 12];
                                    oRng.Formula = "=L" + target_line + "-K" + target_line;
                                    /*
                                    if (datareader["Period11_TargetIncrease"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 12] = Math.Round(Convert.ToDouble(datareader["Period11_TargetIncrease"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 12];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 12] = "0";*/
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 12];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 12];
                                    if (datareader["Period11_ActualIncrease"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period11).Days >= 0)
                                            oRng.Formula = "=L" + actual_line + "-K" + actual_line;
                                        //oSheet.Cells[current_line, 12] = Math.Round(Convert.ToDouble(datareader["Period11_ActualIncrease"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 12] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 12];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period11).Days >= 0)
                                            oSheet.Cells[current_line, 12] = "0";
                                        else
                                            oSheet.Cells[current_line, 12] = "";
                                    }
                                    current_line = current_line - 4;
                                    //***
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 13];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    target_line = current_line;
                                    if (datareader["Period12_IncTarYTD"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 13] = Math.Round(Convert.ToDouble(datareader["Period12_IncTarYTD"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 13];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 13] = "0";
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 13];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    actual_line = current_line;
                                    if (datareader["Period12_IncActYTD"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period12).Days >= 0)
                                            oSheet.Cells[current_line, 13] = Math.Round(Convert.ToDouble(datareader["Period12_IncActYTD"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 13] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 13];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period12).Days >= 0)
                                            oSheet.Cells[current_line, 13] = "0";
                                        else
                                            oSheet.Cells[current_line, 13] = "";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 13];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    if (datareader["Period12_IncYrForThisRpt"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period12).Days >= 0)
                                            oSheet.Cells[current_line, 13] = "";
                                        else
                                            oSheet.Cells[current_line, 13] = Math.Round(Convert.ToDouble(datareader["Period12_IncYrForThisRpt"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 13];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period12).Days >= 0)
                                            oSheet.Cells[current_line, 13] = "";
                                        else
                                            oSheet.Cells[current_line, 13] = "0";
                                    }
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 13];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;

                                    oRng = (Excel.Range)oSheet.Cells[current_line, 13];
                                    oRng.Formula = "=M" + target_line + "-L" + target_line;
                                    /*
                                    if (datareader["Period12_TargetIncrease"] != System.DBNull.Value)
                                    {
                                        oSheet.Cells[current_line, 13] = Math.Round(Convert.ToDouble(datareader["Period12_TargetIncrease"]),0).ToString();
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 13];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                        oSheet.Cells[current_line, 13] = "0";*/
                                    current_line = current_line + 1;
                                    oRng = (Excel.Range)oSheet.Cells[current_line, 13];
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;


                                    oRng = (Excel.Range)oSheet.Cells[current_line, 13];
                                    if (datareader["Period12_ActualIncrease"] != System.DBNull.Value)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period12).Days >= 0)
                                            oRng.Formula = "=M" + actual_line + "-L" + actual_line;
                                        //oSheet.Cells[current_line, 13] = Math.Round(Convert.ToDouble(datareader["Period12_ActualIncrease"]),0).ToString();
                                        else
                                            oSheet.Cells[current_line, 13] = "";
                                        oRng = (Excel.Range)oSheet.Cells[current_line, 13];
                                        oRng.NumberFormatLocal = "#,##0;#,##0";
                                    }
                                    else
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(period12).Days >= 0)
                                            oSheet.Cells[current_line, 13] = "0";
                                        else
                                            oSheet.Cells[current_line, 13] = "";
                                    }
                                    current_line = current_line - 4;

                                    current_line = current_line + 1;


                                    oRng = oSheet.get_Range("A1", "Z65536");
                                    oRng.EntireColumn.AutoFit();
                                }

                            }
                            /*
                            string result2 = "";
                            int c2 = 2;
                            total_line = total_line + 2;
                            foreach (KeyValuePair<string, DateTime> ch in column_list)
                            {
                                for (int i = 0; i < 5; i++)
                                {
                                    foreach (int line in total_list)
                                    {
                                        if (string.IsNullOrEmpty(result2))
                                            result2 = ch.Key + (line + i).ToString();
                                        else
                                            result2 = result2 + "+" + ch.Key + (line + i).ToString();
                                    }
                                    oRng = (Excel.Range)oSheet.Cells[total_line + i, c2];
                                    if (i == 2)
                                    {
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(ch.Value).Days < 0)
                                        {
                                            //oRng = (Excel.Range)oSheet.Cells[total_line + i, c2];
                                            if (!string.IsNullOrEmpty(result2))
                                            {
                                                oRng.Formula = "=" + result2;
                                                oRng.NumberFormatLocal = "#,##0;#,##0";
                                            }
                                        }
                                        else
                                        {
                                            //oRng = (Excel.Range)oSheet.Cells[total_line + i, c2];
                                            oRng.Formula = "";
                                        }
                                    }
                                    else if (i == 0 || i == 3)
                                    {
                                        //oRng = (Excel.Range)oSheet.Cells[total_line + i, c2];
                                        if (!string.IsNullOrEmpty(result2))
                                        {
                                            oRng.Formula = "=" + result2;
                                            oRng.NumberFormatLocal = "#,##0;#,##0";
                                        }
                                    }
                                    else
                                    {
                                        //oRng = (Excel.Range)oSheet.Cells[total_line + i, c2];
                                        if (Convert.ToDateTime(cutoffTextBox.Text).Subtract(ch.Value).Days >= 0)
                                        {
                                            if (!string.IsNullOrEmpty(result2))
                                            {
                                                oRng.Formula = "=" + result2;
                                                oRng.NumberFormatLocal = "#,##0;#,##0";
                                            }
                                        }
                                        else
                                        {
                                            oRng.Formula = "";
                                        }                                        
                                    }
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeLeft).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeRight).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeTop).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    oRng.Borders.get_Item(Excel.XlBordersIndex.xlEdgeBottom).LineStyle = Excel.XlLineStyle.xlContinuous;
                                    result2 = "";
                                }
                                c2 = c2 + 1;
                            }*/
                        }
                    }

                    command.Dispose();
                    connection.Close();
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    /*
                    System.Diagnostics.Trace.Write(DateTime.Now.ToString("dd/MM/yyyy") + " " + ex.Message);
                    System.Diagnostics.Trace.Flush();

                    connection.Close();
                    Error_Label.Text = Error_Label.Text + "sp_Login error " + ex.Message;
                    */
                }
            }
            intRow = intRow + 1;
            int intCount = 0;

            //oSheet = (Excel._Worksheet)oWB.Sheets[];
            ((Excel.Worksheet)oWB.Sheets["Sheet1"]).Delete();
            ((Excel.Worksheet)oWB.Sheets["Sheet2"]).Delete();
            ((Excel.Worksheet)oWB.Sheets["Sheet3"]).Delete();

            //Deal with Formula at summary sheet of Building Edited by Martin 20120420
            if (sectionCode.SelectedValue == "BLDG")
            {
                //oSheet = (Excel.Worksheet)oWB.Sheets["Data-BLDG"];
                oSheet = (Excel.Worksheet)oWB.Sheets[1];
                Double Result;

                Boolean IsTotal = false;
                int Total_row_count = 0;
                Boolean IsBlank = false;
                for (int i = 1; i <= 100; i++)
                {
                    Total_row_count = 0;
                    Boolean IsActual = false;
                    Boolean IsTarget = false;
                    //if ((Total_row_count == 0 || Total_row_count > 13))
                    //{
                    for (int j = 1; j <= 13; j++)
                    {
                        oRng = (Excel.Range)oSheet.Cells[i, j];
                        if (oRng.Value2 != null)
                        {
                            if (oRng.get_Value(Missing).ToString() == "Target")
                            {
                                IsTarget = true;
                            }
                            if (oRng.get_Value(Missing).ToString() == "Actual")
                            {
                                IsActual = true;
                            }
                            if (oRng.get_Value(Missing).ToString() == "Total")
                            {
                                IsTotal = true;
                            }
                            if( IsTotal == true)
                                Total_row_count = Total_row_count + 1;

                            if (String.IsNullOrEmpty(oRng.get_Value(Missing).ToString()) == true)
                            {
                                IsTotal = false;
                            }
                            if ((IsTarget == true || IsActual == true) && IsTotal == false)
                            {
                                if (Double.TryParse(oRng.get_Value(Missing).ToString(), out Result) == true)
                                {
                                    for (int k = 2; k <= oWB.Sheets.Count; k++)
                                    {
                                        oSheet2 = (Excel.Worksheet)oWB.Sheets[k];
                                        if (k == 2)
                                        {
                                            oSheet.Cells[i, j] = "='" + oSheet2.Name + "'!" + IntegerToColumnName(j) + i.ToString();
                                        }
                                        else
                                        {
                                            oRng2 = (Excel.Range)oSheet.Cells[i, j];
                                            oSheet.Cells[i, j] = oRng2.Formula + "+" + "'" + oSheet2.Name + "'!" + IntegerToColumnName(j) + i.ToString();
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            //Other column together with the same row of "Total can not update this
                            if ((Total_row_count == 0 || Total_row_count > 13))
                                IsTotal = false;
                        }
                    //}
                    }
                }
            }
            oRng = oSheet.get_Range("A1", "A65536");

            string fname = Server.MapPath("..\\ReportsFile\\ExportFile" + number.ToString() + ".xls");
            if (System.IO.File.Exists(fname))
            {
                System.IO.File.Delete(fname);
                oWB.SaveCopyAs(fname);
            }
            else
                oWB.SaveCopyAs(fname);

            oWB.Close(null, null, null);
            oXL.Workbooks.Close();
            oXL.Quit();
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(oRng);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oXL);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oWB);
            oSheet = null;
            oWB = null;
            oXL = null;
            GC.Collect();
        }
        catch (Exception ex)
        {
            //PCCore.Common.HRLog.RecordLog("can not open Excel");
            //PCCore.Common.HRLog.RecordLog(ex.Message);

            //Error_Label.Text = "No file generated! " + " Error Occur - Generation of Excel " + ex.Message;
            oWB.Close(null, null, null);
            oXL.Workbooks.Close();
            oXL.Quit();
            //System.Runtime.InteropServices.Marshal.ReleaseComObject(oRng);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oXL);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oSheet);
            System.Runtime.InteropServices.Marshal.ReleaseComObject(oWB);
            oSheet = null;
            oWB = null;
            oXL = null;
            GC.Collect();
        }
        //PCCore.Common.HRLog.RecordLog("end");
        string path = Server.MapPath("..").Replace("\\", "/");
        SimpleWebUtils.DownloadFile(Page.Response, path + "/ReportsFile/ExportFile" + number.ToString() + ".xls");
    }
}
