
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

public partial class MasterSecurityForm : MasterBasePage, IErrorHandler
{
    bool _automaticMode = true;
    public bool AutomaticMode
    {
        get { return _automaticMode; }
        set { _automaticMode = value; }
    }

    protected override void OnInit(EventArgs e)
    {        
        FormMode = (FormModes) SimpleUtils.IfNull(Request.QueryString[Consts.QueryStringMode], typeof(FormModes), FormModes.None);
        RecordID = Request.QueryString[Consts.QueryStringID];
        RecordPID = Request.QueryString[Consts.QueryStringPID];
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
        switch (FormMode)
        {
            case FormModes.New:
                if (!SecurityInfo.CanNew)
                {
                    SetToDisplay();
                }
                else
                {
                    if (!String.IsNullOrEmpty(ParentTableName))
                    {
                        if (String.IsNullOrEmpty(RecordPID))
                        {
                            SetToDisplay();
                        }
                        else
                        {
                            switch (SecurityInfo.PublishType) // check publish type
                            {
                                case Sc.PublishTypes.Private:
                                case Sc.PublishTypes.PrivateUpdate:
                                    string sql = String.Format("SELECT CREATEDBY FROM {0} WHERE ID={1}", ParentTableName, RecordPID);
                                    DataTable dt = PCDb.Db.ExecQuery(sql);
                                    if (dt != null && dt.Rows.Count > 0)
                                    {
                                        DataRow dr = dt.Rows[0];
                                        string createdBy = SimpleUtils.IfNull(dr[Consts.FieldCreatedBy], String.Empty);
                                        if (createdBy != SessionInfo.LoginName)
                                        {
                                            SetToDisplay();
                                        }
                                    }
                                    else
                                    {
                                        SetToDisplay();
                                    }
                                    break;
                            }
                        }
                    }
                }
                break;
            case FormModes.Edit:
                if (!SecurityInfo.CanEdit)
                {
                    SetToDisplay();
                }
                else
                {
                    switch (SecurityInfo.PublishType) // check publish type
                    {
                        case Sc.PublishTypes.Private:
                        case Sc.PublishTypes.PrivateUpdate:
                            if (_dr != null)
                            {
                                string createdBy = SimpleUtils.IfNull(_dr[Consts.FieldCreatedBy], String.Empty);
                                if (createdBy != SessionInfo.LoginName)
                                {
                                    SetToDisplay();
                                }
                            }
                            else //postback
                            {
                                if (DisplayOnly)
                                {
                                    SetToDisplay();
                                }
                            }
                            break;
                    }
                }
                break;
        }

    }

    protected void Page_Load(object sender, EventArgs e)
    {
        //the position is very important, must after content page's init/load
        InitSecurity();

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

        if (Save != null)
        {
            Save(sender, e);
        }
    }
    #endregion Save Event

    public override void DisableForm()
    {
        btnSave.Enabled = false; 
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
                case "HrCheckBox":
                    System.Web.UI.WebControls.CheckBox chk = c as System.Web.UI.WebControls.CheckBox;
                    chk.Enabled = false;
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
                    htmlgen.Visible = false;
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
                    case "HrCheckBox":
                        System.Web.UI.WebControls.CheckBox schk = subc as System.Web.UI.WebControls.CheckBox;
                        schk.Enabled = false;
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
                        htmlgen.Visible = false;
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
                case "HrCheckBox":
                    System.Web.UI.WebControls.CheckBox schk = subc as System.Web.UI.WebControls.CheckBox;
                    schk.Enabled = false;
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
                    htmlgen.Visible = false;
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
                    chk.Checked = false;
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


    #region IErrorHandler Members

    void IErrorHandler.HandleError(Exception ex)
    {
        throw new NotImplementedException();
    }

    void IErrorHandler.ClearError()
    {
        throw new NotImplementedException();
    }

    #endregion
}
