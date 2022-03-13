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

public partial class MasterProjectReport : MasterBasePage, IErrorHandler
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
        //PCCore.Database.ValidationList.ReportProjectList(ddlProject, Consts.DropDownOptionAll);
        if (SessionInfo.CurrentFunctionID != "1145") 
        PCCore.Database.ValidationList.ReportProjectList(ddlProject, Consts.DropDownOptionNone);
        //if (SessionInfo.CurrentFunction == Consts.ScGroupFunc)
        //{
        

        //}
        if (SessionInfo.CurrentFunctionID == "1145")
        {
            ddltProject.Visible = true;
            ddlttProject.Visible = true;
            ddlProject.AutoPostBack = true;
            ddltProject.AutoPostBack = true;
            ddlttProject.AutoPostBack = true;
            Label2.Visible = true;
            Label3.Visible = true;
            Label4.Visible = true;
            Label5.Visible = true;
            ddlProject.SelectedIndexChanged += new System.EventHandler(ddlProject_SelectedIndexChanged);
            ddltProject.SelectedIndexChanged += new System.EventHandler(ddltProject_SelectedIndexChanged);
        }
        if (SessionInfo.CurrentFunctionID == "2113")
        {
            Label6.Visible = true;
            Label7.Visible = true;
            txtcutoff.Visible = true;
        }
        ScriptManager.RegisterClientScriptBlock(Page, Page.GetType(), "test", "<script language='javascript'>var subcon ='" + ddlttProject.Text + "';</script>", false);

        if (Page.IsPostBack)
        {
            if (invProjCode.Text != String.Empty)
            {
                // Get docentry of project
                PCCore.Database.Project _p = new PCCore.Database.Project(invProjCode.Text);
                try
                {
                    ddlProject.SelectedValue = Convert.ToString(_p.getValue("U_DocEntry"));
                }
                catch (Exception ex)
                {
                    PCCore.Common.HRLog.RecordException("Set Project Code", ex);
                }
                //ddlProject.SelectedValue = invProjCode.Text;
                invProjCode.Text = String.Empty;
            }
        }
		else
        {

            // -- ada 20130530 
            if (SessionInfo.CurrentFunctionID == "1145")
            {
//                SAPDb.InitDropDownList(ddlProject, "CPS_View_ProjectList", "projectcode", "PrjDesc", Consts.DropDownOptionNone, null, null);
                if (SessionInfo.IsSupervisor)
                {
                    SAPDb.InitDropDownList(ddlProject, "CPS_View_ProjectList", "projectcode", "PrjDesc", Consts.DropDownOptionNone, null, null);
                }
                else
                {
                    SAPDb.InitDropDownList(ddlProject, "CPS_View_ProjectList", "projectcode", "PrjDesc", Consts.DropDownOptionNone, null, string.Format(" prjCode in('{0}')", SessionInfo.ProjectID.Replace(",", "','"), Consts.PROJECT_OPEN));
                }
                //PCCore.Database.ValidationList.ReportProjectList(ddlProject, Consts.DropDownOptionNone);
            }

        }
        //add by Martin, begin
        if (SessionInfo.CurrentFunctionID == "3101" || SessionInfo.CurrentFunctionID == "3102" || SessionInfo.CurrentFunctionID == "3103" || SessionInfo.CurrentFunctionID == "3108")
        {
            ExportPDFButton.Visible = true;
            ExportExcelButton.Visible = true;
        }
        else
        {
            ExportPDFButton.Visible = false;
            ExportExcelButton.Visible = false;
        }
        //add by Martin, end
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

    protected void ExportPDFButton_Click(object sender, EventArgs e)
    {

    }
    private void ddlProject_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        //invProjCode.Text = ddlProject.SelectedValue;
        if (ddlProject.SelectedValue != "-2")
        {
            PCCore.Database.ValidationList.SubContractorList(ddltProject, ddlProject.SelectedValue, Consts.DropDownOptionNone);
            ddlttProject.Items.Clear();
        }
        else
        {
            ddlttProject.Items.Clear();
            ddltProject.Items.Clear();
        }

    }

    private void ddltProject_SelectedIndexChanged(object sender, System.EventArgs e)
    {
        if (ddltProject.SelectedValue != "-2")
        {
            PCCore.Database.ValidationList.SubContractorNoList(ddlttProject, ddlProject.SelectedValue, ddltProject.SelectedValue, Consts.DropDownOptionNone);
        }
        else
        {
            ddlttProject.Items.Clear();
        }
    }	
}

