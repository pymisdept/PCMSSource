
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
using System.Collections.Generic;
using SimpleControls;
using SimpleControls.Web;
using PCCore;
using Excel = Microsoft.Office.Interop.Excel;
using System.Reflection;

public partial class MasterFormMUpl : MasterBasePage, IErrorHandler
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

    public void SetToDisplay()
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
        
        this.btnSave.Attributes.Add("onclick", "this.disabled=true;" + this.Page.ClientScript.GetPostBackEventReference(this.btnSave, ""));
        
        if (Page.IsPostBack)
        {
            try
            {
                //btnSave.Enabled = false;
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
        //btnSave.Enabled = true;
    }

    #endregion Save Event
    
    public override void DisableForm()
    {
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
    
    public Button ButtonSave
    {
        get { return btnSave; }
    }

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
        
        if (FormMode == MasterFormMUpl.FormModes.New)
        {
            
            //MasterForm.ShowWarning(UpdateLoad.UploadFile.Count.ToString());
            if (UpdateLoad.UploadFile.Count < 1)
            {
                errMessage =  Resources.Messages.UploadFile;
                return errMessage;
            }
            else
            {
                for (int i = 0; i < UpdateLoad.UploadFile.Count; i++)
                {
                    if (UpdateLoad.UploadFile[i].ContentLength <= 0)
                    {
                        errMessage = Resources.Messages.UploadFile;
                        return errMessage;//return false;
                    }
                    PCCore.Common.HRLog.RecordLog("FileName: " + UpdateLoad.UploadFile[i].FileName.ToString());
                    PCCore.Common.HRLog.RecordLog("File Contenttype: " + UpdateLoad.UploadFile[i].ContentType.ToString());

                    if (UpdateLoad.UserDefineFuncName != "mai01")
                    {

                        //if (UpdateLoad.UploadFile[i].ContentType != "application/vnd.ms-excel")
                        if (UpdateLoad.UploadFile[i].ContentType != "application/vnd.ms-excel.sheet.macroEnabled.12" && UpdateLoad.UploadFile[i].ContentType != "application/vnd.ms-excel")
                        {
                            errMessage = Resources.Messages.UploadExcelFile;
                            return errMessage;//return false;
                        }
                    }
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

    // Modified by Ken, 20161115, begin
    public static List<int> CheckForm(UpdateLoadFile UpdateLoad, FormModes FormMode, int Limit)
    {
        string errMessage = "";
        List<int> errList = new List<int>();

        if (FormMode == MasterFormMUpl.FormModes.New)
        {
            //MasterForm.ShowWarning(UpdateLoad.UploadFile.Count.ToString());
            for (int i = 0; i < Limit; i++)
            {
                if (UpdateLoad.UploadFile[i].ContentLength <= 0)
                {
                    errList.Add(i);
                }
                PCCore.Common.HRLog.RecordLog("FileName: " + UpdateLoad.UploadFile[i].FileName.ToString());
                PCCore.Common.HRLog.RecordLog("File Contenttype: " + UpdateLoad.UploadFile[i].ContentType.ToString());

                if (UpdateLoad.UserDefineFuncName != "mai01")
                {
                    if (UpdateLoad.UploadFile[i].ContentType != "application/vnd.ms-excel")
                    {
                        errList.Add(i);
                    }
                }
            }
        }

        return errList;
    }
    // Modified by Ken, 20161115, end

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
}
