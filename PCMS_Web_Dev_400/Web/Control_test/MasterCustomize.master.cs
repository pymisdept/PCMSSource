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
using PCCore;
using SimpleControls;
using SimpleControls.SimpleDatabase;
using SimpleControls.Web;

public partial class MasterCustomize : MasterBasePage, IErrorHandler
{
    protected override void OnInit(EventArgs e)
    {
        string cssLink =null;
        switch (SessionInfo.CurrentFunction.ToUpper())
        {
            case Consts.ScDropdownSetting:
            case Consts.ScGroupRightFunc:
            case Consts.ScUserRightFunc:
            case Consts.ScMandatorySetting:
            case Consts.ScFunctionFunc:
            case Consts.ScUserFunctionFunc:
                cssLink = SimpleWebUtils.GetCssLink(Config.AppBaseUrl + "/control/gridview-security-nopagedown.css");
                break;
            default:
                cssLink = SimpleWebUtils.GetCssLink(Config.AppBaseUrl+"/control/gridview-security.css");
                break;
        }
        LiteralControl css = new LiteralControl(cssLink);
        css.ID = "cssID";
        Page.Header.Controls.Add(css);
        string str = Request.Path.ToString();
        FormMode = (FormModes)SimpleUtils.IfNull(Request.QueryString[Consts.QueryStringMode], typeof(FormModes), FormModes.None);
        RecordID = Request.QueryString[Consts.QueryStringID];
        base.OnInit(e);
    }

    //End Copy from MasterForm
    private void InitSecurity()
    {
        
        if (SessionInfo.IsLogin && IsFirstEnter && SecurityInfo != null)
        {
            ScLog.Insert(ScLog.Actions.Enter, int.Parse(SecurityInfo.FunctionId), SecurityInfo.FunctionName);
        }
    }

    protected void Page_Load(object sender, EventArgs e)
    {
        InitSecurity();
        
        //if (SessionInfo.CurrentFunction == Consts.ScGroupFunc)
        //{
        

        //}
    }
    #region DeleteRecord

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

    public override void DeleteRecord(string tableName, PCCore.GridView gridView, CheckDeletePrerequisite checkPrerequisite, SimpleNote pNote)
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
            //ShowMessage(Resources.Common.DeleteOK);
            pNote.ShowMessage(Resources.Common.DeleteOK);
        }
        catch (Exception ex)
        {
            gridView.ClearSelectedRowValues = false;
            //HandleError(ex);
            PCCore.ComFunction2.HandleError(ex,pNote);
            
            table.RollbackTransaction();
        }
    }

    #endregion

    
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

    #region Error Handling
    public override void HandleError(Exception ex)
    {
        Note.HRShowException(ex);
        Note.Visible = true;
        NoteSepLine.Visible = true;
    }

    public override void ClearError()
    {
        Note.Clear();
        Note.Visible = false;
        NoteSepLine.Visible = false;
    }

    public override void ShowMessage(string message)
    {
        ShowMessage(message, null);
    }

    public override void ShowMessage(string message, string tooltip)
    {
        Note.ShowMessage(message, tooltip);
        NoteSepLine.Visible = true;
    }

    public override void ShowWarning(string warning)
    {
        ShowWarning(warning, null);
    }

    public override void ShowWarning(string warning, string tooltip)
    {
        Note.ShowWarning(warning, tooltip);
        NoteSepLine.Visible = true;
    }
    #endregion Error Handling

    //public ToolBar ToolBar
    //{
    //    get { return tbToolBar; }
    //}

    public SimpleNote MasterNote
    {
        get { return Note; }
    }

    public HtmlTableRow MasterNoteSepLine
    {
        get { return NoteSepLine; }
    }

    //public string HRProcess
    //{
    //    set { this._HRProc = value; }
    //    get { return this._HRProc; }
    //}

}

