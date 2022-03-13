using System;
using System.Data;
using System.Configuration;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.ComponentModel;
using System.Drawing;
using System.Text;
using SimpleControls.Web;
using SimpleControls;
using System.Text.RegularExpressions;
using System.Globalization;
using System.IO;

namespace PCCore
{
    [ToolboxData("<{0}:GridView runat=server></{0}:GridView>")]
    [ToolboxBitmap(typeof(GridView))]
    public class PmReportGridView : SimpleGridView
    {
        const string GridviewPageTextBoxOnKeyDownClineKey = "GridviewPageTextBoxOnKeyDownKey";
        public enum DateType : int
        {
            DateTime,
            Time
        }
        string datetimefield = null;
        public string DateTimeField
        {

            get { return datetimefield; }
            set { datetimefield = value; }
        }
        string timefield = null;
        public string TimeField
        {

            get { return timefield; }
            set { timefield = value; }
        }

        ClientScriptManager _cs = null;
        public PmReportGridView()
        {
            //Disable Double Click
            if (SessionInfo.CurrentModuleID == "6")
                base.ClientRowDblClicked = "if(typeof(Edit)=='function') Edit();";
            //base.HiddenFields = "ID,PID,CREATED,CREATEDBY,MODIFIED,MODIFIEDBY,";
            base.EmptyDataText = HttpContext.GetGlobalResourceObject(Consts.ResourcesCommon, "NoData", new CultureInfo(SessionInfo.CurrentLanguage)).ToString();
        }

        protected bool _autoToolTip = true;

        [DefaultValue(true)]
        public bool AutoToolTip
        {
            get { return _autoToolTip; }
            set { _autoToolTip = value; }
        }

        protected bool _showPageIndex = true;
        [DefaultValue(true)]
        public bool ShowPageIndex
        {
            get { return _showPageIndex; }
            set { _showPageIndex = value; }
        }


        protected bool _isReportType = false;
        /// <summary>
        /// 设置是否显示Grid Page '<< <  > >>'
        /// </summary>
        [DefaultValue(false)]
        public bool IsReportType
        {
            get { return _isReportType; }
            set { _isReportType = value; }
        }


        protected bool _isUseSelfExportExcel = false;
        /// <summary>
        /// 设置是否自己本页有导出的功能操作。 2006-11-16
        /// </summary>
        public bool IsUseSelfExportExcel
        {
            get { return _isUseSelfExportExcel; }
            set { _isUseSelfExportExcel = value; }
        }


        protected string _UseDataRowValueGetResoucesColumn = "_isEmptyJ";
        /// <summary>
        /// 设置是否允许DataRow的值根据 Resources来定义
        /// </summary>
        public string UseDataRowValueGetResoucesColumn
        {
            get { return _UseDataRowValueGetResoucesColumn; }
            set { _UseDataRowValueGetResoucesColumn = value; }
        }

        protected bool _UseDefaultSystemDataFromatDataRow = false;
        /// <summary>
        /// 设置是否允许DataRow的值根据 类型进行默认的格式化
        /// </summary>
        public bool UseDefaultSystemDataFromatDataRow
        {
            get { return _UseDefaultSystemDataFromatDataRow; }
            set { _UseDefaultSystemDataFromatDataRow = value; }
        }

        protected bool _UserDefineReSelected = false;
        /// <summary>
        /// 
        /// </summary>
        public bool UserDefineReSelected
        {
            get { return _UserDefineReSelected; }
            set { _UserDefineReSelected = value; }
        }

        protected string _UserDefineReSelectedCurrPageRowIndex = "-1";
        /// <summary>
        /// 
        /// </summary>
        public string UserDefineReSelectedCurrPageRowIndex
        {
            get { return _UserDefineReSelectedCurrPageRowIndex; }
            set { _UserDefineReSelectedCurrPageRowIndex = value; }
        }

        #region ReSelect Row By Default ID column in dv's RowIndex (1) 2007-07-27
        protected override void OnPageIndexChanging(GridViewPageEventArgs e)
        {
            this.PageIndexChanging += new GridViewPageEventHandler(gvData_PageIndexChanging);
            this.UserDefineReSelected = false;   //PageIndex changed No selected row
            base.OnPageIndexChanging(e);
        }
        void gvData_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
        }

        protected override void OnSorting(GridViewSortEventArgs e)
        {
            this.Sorting += new GridViewSortEventHandler(gvData_Sorting);
            this.UserDefineReSelected = true; //PageIndex changed  selected row
            base.OnSorting(e);
        }
        void gvData_Sorting(object sender, GridViewSortEventArgs e)
        {
            //throw new Exception("The method or operation is not implemented.");
        }


        #endregion

        protected override void PerformDataBinding(System.Collections.IEnumerable data)
        {

            System.Collections.IEnumerable myData = null;
            DataView sdv = null;
            //sdv = data as DataView;
            if (data != null)
                sdv = new DataView((data as DataView).Table);

            #region ReSelect Row By Default ID column in dv's RowIndex (2) 2007-07-27
            Int32 OldSelectedRowIndexByID = -1;
            if (sdv != null)
            {
                if (UserDefineReSelected)
                {

                    if (this.SelectedRowValue != string.Empty)
                    {
                        if (this.SelectValueField == "ID")
                        {
                            for (Int32 i = 0; i < sdv.Table.Rows.Count; i++)
                            {
                                if (sdv[i][this.SelectValueField].ToString() == this.SelectedRowValue)
                                {
                                    OldSelectedRowIndexByID = i;
                                    break;
                                }
                            }

                            if (OldSelectedRowIndexByID <= this.PageSize)
                            {
                                this.PageIndex = 0;
                                UserDefineReSelectedCurrPageRowIndex = Convert.ToInt32((OldSelectedRowIndexByID % this.PageSize) + 1).ToString();
                            }
                            else
                            {
                                this.PageIndex = Convert.ToInt32(Math.Round(Convert.ToDecimal(OldSelectedRowIndexByID / this.PageSize), 0));
                                UserDefineReSelectedCurrPageRowIndex = Convert.ToInt32((OldSelectedRowIndexByID % this.PageSize) + 1).ToString();
                            }

                        }
                    }

                }

            }
            #endregion

            if (sdv != null)
            {
                string[] ColumnNum = null;
                ColumnNum = UseDataRowValueGetResoucesColumn.Split(new char[] { ',' });

                if (ColumnNum[0].ToString() != "_isEmptyJ")
                {
                    for (int i = 0; i < sdv.Count; i++)
                    {
                        for (int k = 0; k < ColumnNum.Length; k++)
                        {

                            int sValue;
                            string sResourceValue = string.Empty;
                            sValue = Convert.ToInt32(ColumnNum[k]);

                            sResourceValue = SimpleUtils.IfNull(HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, sdv[i][sValue].ToString()), sdv[i][sValue].ToString());
                            sdv[i][sValue] = sResourceValue;
                        }

                    }

                }
                sdv.Sort = (data as DataView).Sort;
                myData = sdv as System.Collections.IEnumerable;
                PCCore.Common.HRLog.RecordLog(base.ID);
                base.PerformDataBinding(myData);
            }
            else
            {
                base.PerformDataBinding(data);
            }

        }

        protected override void OnRowDataBound(GridViewRowEventArgs e)
        {
            base.OnRowDataBound(e);
            if (!_autoToolTip) return;
            TableCellCollection cells;
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    
                    DataRowView drv;
                    string sTime = null;
                    cells = e.Row.Cells;
                    drv = e.Row.DataItem as DataRowView;

                    int i;
                    StringBuilder sb = new StringBuilder();
                    i = this.FieldIndex(Consts.FieldID);
                    if (i >= 0)
                        //sb.AppendFormat("ID: {0}", cells[i].Text);
                        sb.AppendFormat("ID: {0}", cells[1].Text);
                    i = this.FieldIndex(Consts.FieldCreated);
                    if (i >= 0)
                    {
                        sTime = drv[i].ToString() == String.Empty ? "" : Convert.ToDateTime(drv[i]).ToString(Consts.DefaultDateTimeFormat);
                        sb.AppendFormat("\nCreated: {0}", sTime);
                    }

                    i = this.FieldIndex(Consts.FieldCreatedBy);
                    if (i >= 0)
                        sb.AppendFormat("\nCreated By: {0}", cells[i].Text);

                    i = this.FieldIndex(Consts.FieldModified);
                    if (i >= 0)
                    {
                        sTime = drv[i].ToString() == String.Empty ? "" : Convert.ToDateTime(drv[i]).ToString(Consts.DefaultDateTimeFormat);
                        sb.AppendFormat("\nModified: {0}", sTime);
                    }

                    i = this.FieldIndex(Consts.FieldModifiedBy);
                    if (i >= 0)
                        sb.AppendFormat("\nModified By: {0}", cells[i].Text);
                    e.Row.ToolTip = sb.ToString();

                    int count;
                    if (!this._allowMultipleSelect)
                    {
                        for (count = 0; count <= cells.Count - 1; count++)
                        {
                            if (cells[count].Text == "&nbsp;")
                                cells[count].Text = "";
                            if (drv.Row[count].GetType().Name == "DateTime")
                            {
                                if (!string.IsNullOrEmpty(DateTimeField) && GetIndex(DateTimeField, count))
                                {
                                    ShowDate(DateTimeField, DateType.DateTime, e);
                                }
                                else if (!string.IsNullOrEmpty(TimeField) && GetIndex(TimeField, count))
                                {
                                    ShowDate(TimeField, DateType.Time, e);
                                }
                                else
                                {
                                    try
                                    {
                                        cells[count].Text = Convert.ToDateTime(cells[count].Text).ToString(Consts.DateFormat);
                                    }
                                    catch
                                    {
                                        cells[count].Text = "";
                                    }
                                }
                            }
                            if (drv.Row[count].GetType().Name == "Decimal" || drv.Row[count].GetType().Name == "Integer" || drv.Row[count].GetType().Name == "Double" || drv.Row[count].GetType().Name == "Currency")
                            {
                                //if (cells[count].Visible && cells[count].Width.Value >0)

                                cells[count].HorizontalAlign = HorizontalAlign.Right;
                                if ((this.FieldIndex("ID") != count && UseDefaultSystemDataFromatDataRow)
                                    || (this.FieldIndex("ID") != count && SessionInfo.CurrentModule == Consts.PayrollModule))
                                {
                                    cells[count].Text = String.IsNullOrEmpty(cells[count].Text.ToString()) ? String.Empty : Convert.ToDecimal(cells[count].Text.ToString()).ToString(Consts.CurrencyFormat);
                                }

                            }
                        }
                    }
                    else
                    {
                        for (count = 1; count <= cells.Count - 2; count++)
                        {

                            if (cells[count].Text == "&nbsp;")
                                cells[count].Text = "";
                            if (drv.Row[count].GetType().Name == "DateTime")
                            {
                                if (!string.IsNullOrEmpty(DateTimeField) && GetIndex(DateTimeField, count))
                                {
                                    ShowDate(DateTimeField, DateType.DateTime, e);
                                }
                                else if (!string.IsNullOrEmpty(TimeField) && GetIndex(TimeField, count))
                                {
                                    ShowDate(TimeField, DateType.Time, e);
                                }
                                else
                                {
                                    try
                                    {
                                        cells[count + 1].Text = Convert.ToDateTime(cells[count + 1].Text).ToString(Consts.DateFormat);
                                    }
                                    catch
                                    {
                                        cells[count + 1].Text = "";
                                    }
                                }
                            }
                            if (drv.Row[count].GetType().Name == "Decimal" || drv.Row[count].GetType().Name == "Integer" || drv.Row[count].GetType().Name == "Double" || drv.Row[count].GetType().Name == "Currency")
                            {
                                //if (cells[count + 1].Visible && cells[count+1].Width.Value > 0)

                                cells[count + 1].HorizontalAlign = HorizontalAlign.Right;
                                if ((this.FieldIndex("ID") != count + 1 && UseDefaultSystemDataFromatDataRow)
                                    || (this.FieldIndex("ID") != count + 1 && SessionInfo.CurrentModule == Consts.PayrollModule))
                                {
                                    cells[count + 1].Text = String.IsNullOrEmpty(cells[count + 1].Text.ToString()) ? String.Empty : Convert.ToDecimal(cells[count + 1].Text.ToString()).ToString(Consts.CurrencyFormat);
                                }
                            }
                        }
                    }
                    break;
            }

        }

        private bool GetIndex(string Field, int count)
        {
            if (string.IsNullOrEmpty(Field)) return false;
            string delimStr = ",";
            char[] delimiter = delimStr.ToCharArray();
            string[] split = null;
            int i;
            split = Field.Split(delimiter);
            foreach (string s in split)
            {
                i = Convert.ToInt32(s);
                if (i == count)
                    return true;
            }
            return false;
        }

        private void ShowDate(string Field, DateType datetype, GridViewRowEventArgs e)
        {
            if (string.IsNullOrEmpty(Field)) return;
            DataRowView drv;
            drv = e.Row.DataItem as DataRowView;
            string delimStr = ",";
            char[] delimiter = delimStr.ToCharArray();
            string[] split = null;
            int i;
            split = Field.Split(delimiter);

            foreach (string s in split)
            {
                i = Convert.ToInt32(s);
                if (drv.Row[i].GetType().Name == "DateTime")
                {
                    switch (datetype)
                    {
                        case DateType.DateTime:
                            try
                            {
                                e.Row.Cells[i].Text = Convert.ToDateTime(e.Row.Cells[i].Text).ToString(Consts.ShortTimeFormat);
                            }
                            catch
                            {
                                e.Row.Cells[i].Text = "";
                            }
                            break;
                        case DateType.Time:
                            try
                            {
                                e.Row.Cells[i].Text = Convert.ToDateTime(e.Row.Cells[i].Text).ToString(Consts.ShortTimeFormat);
                            }
                            catch
                            {
                                e.Row.Cells[i].Text = "";
                            }
                            break;
                    }
                }
            }
        }

        protected override void OnDataBound(EventArgs e)
        {
            base.OnDataBound(e);

            BasePage bp = Page as BasePage;
            if (bp != null)
            {
                if ((bp.CurrentCommand == Consts.ButtonExport || bp.Request.QueryString["export"] != null)
                    && (base.DataView != null) && (!IsUseSelfExportExcel))
                {
                    ExportToExcel();
                }
            }
        }

        public void ExportToExcel()
        {
            string fileName = String.Format("exp{0}.xls", DateTime.Now.ToString("yyMMddHHmmssfff"));
            string fullPath = String.Format("{0}\\{1}", SessionInfo.TempDir, fileName);

            if (!Directory.Exists(SessionInfo.TempDir))
                Directory.CreateDirectory(SessionInfo.TempDir);


            BasePage bp = this.Page as BasePage;
            string caption = String.Empty;
            if (bp != null)
            {
                ScInfo scInfo = bp.SecurityInfo;
                if (scInfo != null)
                {
                    //caption = scInfo.FunctionName;
                    caption = SimpleUtils.IfNull(HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, scInfo.FunctionName), scInfo.FunctionName);
                }
            }

            base.ExportToExcel(fullPath, caption);

            SimpleWebUtils.DownloadFile(Page.Response, fullPath);
        }

        public override void RenderPager(HtmlTextWriter writer, PagerPosition position)
        {
            //if (this.PageCount < 1) return;
            #region ReSelect Row By Default ID column in dv's RowIndex (3) 2007-07-27
            base.Attributes["UserDefineReSelectedCurrPageRowIndex"] = UserDefineReSelectedCurrPageRowIndex;
            #endregion
            writer.Write(string.Format("<tr><td class='{0}'>", GetCurrentModuleClass()));
            writer.Write("<table width='100%' height='100%' border='0' cellspacing='0' cellpadding='0'><tr>");

            if (position == PagerPosition.Bottom)
            {
                switch (_isReportType)
                {
                    case false:
                        writer.Write("<td class='{0}'><table width='100%' height='100%' border='0' cellspacing='0' cellpadding='0'><tr><td></td></tr></table></td></tr>", "BottomHeader");
                        break;
                    case true:
                        break;
                }
                writer.Write("<tr><td><table width='100%' height='100%' border='0' cellspacing='0' cellpadding='0'><tr><td align='right' valign='middle'>");
                writer.Write("<td align='right' valign='middle' style='padding-right:5px;'>");
            }
            else if (position == PagerPosition.Top)
            {
                if (this.PageCount < 1)
                {
                    writer.Write(String.Format("<td><table width='100%' height='100%' border='0' cellspacing='0' cellpadding='0'><tr><td align='left' valign='middle' style='padding-left:5px;'> " + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Record").ToString() + " {0} - {1} " + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Of").ToString() + " {2}</td>", 0, 0, 0));
                }
                else
                {
                    if (this.PageIndex == 0)
                    {
                        writer.Write(String.Format("<td><table width='100%' height='100%' border='0' cellspacing='0' cellpadding='0'><tr><td align='left' valign='middle' style='padding-left:5px;'> " + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Record").ToString() + " {0} - {1} " + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Of").ToString() + " {2}</td>", this.PageIndex + 1, this.Rows.Count, this.RecordCount));
                    }
                    else
                    {
                        writer.Write(String.Format("<td><table width='100%' height='100%' border='0' cellspacing='0' cellpadding='0'><tr><td align='left' valign='middle' style='padding-left:5px;'>" + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Record").ToString() + " {0} - {1} " + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Of").ToString() + " {2}</td>", this.PageIndex * PageSize + 1, this.Rows.Count + this.PageIndex * PageSize, this.RecordCount));
                    }
                }
                writer.Write("<td align='left' valign='middle' style='padding-right:5px;'>");

            }

            if (_showPageIndex)
            {
                HyperLink hl;
                int Pageindex = PageIndex + 1;
                switch (_isReportType)
                {

                    case false:
                        hl = GetPagerLink("Prev", PageIndex > 0);
                        hl.RenderControl(writer);

                        writer.Write(" " + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Page").ToString() + " ");

                        PCCore.TextBox txtCurrentpage = new PCCore.TextBox();
                        txtCurrentpage.CssClass = "pageInput";
                        txtCurrentpage.Text = Pageindex.ToString();
                        txtCurrentpage.ClientReadOnly = this.Rows.Count == 0 ? true : false;
                        txtCurrentpage.Attributes["onkeydown"] =
                            string.Format(@"javascript:if (typeof(GridviewPageTextBoxOnKeyDown)=='function')
                                                    {{ GridviewPageTextBoxOnKeyDown('{0}',this.value);}} else {{  return false;}}"
                            , this.ClientID.Replace(this.ClientIDSeparator, this.IdSeparator).ToString());
                        txtCurrentpage.RenderControl(writer);
                        if (PageCount < 1)
                        {
                            writer.Write(string.Format(" " + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Of").ToString() + " " + "{0}", 1));
                        }
                        else
                        {
                            writer.Write(string.Format(" " + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Of").ToString() + " " + "{0}", PageCount));
                        }
                        hl = GetPagerLink("Next", PageIndex + 1 < PageCount);
                        hl.RenderControl(writer);
                        break;
                    case true:
                        hl = GetPagerLink("Prev", 1 > 0);
                        hl.Attributes["onclick"] = "javascript:Prev(); return false;";
                        hl.RenderControl(writer);

                        writer.Write(" " + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Page").ToString() + " ");


                        PCCore.TextBox txtCurrentpage_report = new PCCore.TextBox();
                        txtCurrentpage_report.CssClass = "pageInput";
                        txtCurrentpage_report.Text = Pageindex.ToString();
                        txtCurrentpage_report.Attributes["onkeydown"] = GetPagerEnterScript_ForReport("'Page$' + this.value");
                        txtCurrentpage_report.Attributes["onkeypress"] = "if(window.event.keyCode==13) GotoPageZip(this.value);";

                        txtCurrentpage_report.ID = "txtCurrentpage_report";
                        txtCurrentpage_report.RenderControl(writer);

                        //writer.Write("&nbsp;&nbsp;&nbsp;&nbsp;");
                        //if (PageCount < 1)
                        //{
                        //    writer.Write(string.Format(" " + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Of").ToString() + " " + "{0}", 1));
                        //}
                        //else
                        //{
                        //    writer.Write(string.Format(" " + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Of").ToString() + " " + "{0}", PageCount));
                        //}
                        writer.Write(string.Format(" " + HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "Of").ToString()));
                        PCCore.Label lblRecordcount = new PCCore.Label();
                        lblRecordcount.ID = "lblRecordcount_report";
                        lblRecordcount.RenderControl(writer);

                        writer.Write(" ");
                        hl = GetPagerLink("Next", 1 < 2);
                        hl.Attributes["onclick"] = "javascript:Next();return false;";
                        hl.RenderControl(writer);
                        break;
                }


            }
            else
            {

                PCCore.TextBox txtCurrentpage = new PCCore.TextBox();
                txtCurrentpage.CssClass = "pageInput0";
                txtCurrentpage.ClientReadOnly = this.Rows.Count == 0 ? true : false;
                txtCurrentpage.RenderControl(writer);
                writer.Write("&nbsp;");
            }

            writer.Write("</td>");
            writer.Write("</tr></table>");

            writer.Write("</td>");
            writer.Write("</tr></table>");
            writer.Write("</td></tr>");


        }

        private string GetPagerEnterScript_ForReport(string page)
        {
            try
            {

                return String.Format("javascript:if (!((window.event.keyCode>=48 && window.event.keyCode<=57) || (window.event.keyCode>=96 && window.event.keyCode<=105) || window.event.keyCode==13 || window.event.keyCode==8 || window.event.keyCode==46)) return false; ");

            }
            catch
            {
                return String.Format("javascript:return false;");
            }
        }

        private HyperLink GetPagerLink(string command, bool available)
        {

            HyperLink hl = new HyperLink();
            hl.CssClass = "sgvPagerSymbol";
            switch (command)
            {
                case "Prev":
                    if (_isReportType)
                    {
                        hl.ImageUrl = Config.AppBaseUrl + "/App_Themes/Default/images/arrow_left_black.gif";
                    }
                    else
                    {
                        hl.ImageUrl = Config.AppBaseUrl + "/App_Themes/Default/images/arrow_left.gif";
                    }
                    hl.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "PreviousPage").ToString();
                    break;
                case "Next":
                    if (_isReportType)
                    {
                        hl.ImageUrl = Config.AppBaseUrl + "/App_Themes/Default/images/arrow_right_black.gif";
                    }
                    else
                    {
                        hl.ImageUrl = Config.AppBaseUrl + "/App_Themes/Default/images/arrow_right.gif";
                    }
                    hl.ToolTip = HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "NextPage").ToString();
                    break;
                default:
                    hl.Text = command;
                    hl.ToolTip = String.Format("Page {0}", command);
                    break;
            }
            if (available)
            {
                hl.NavigateUrl = GetPagerScript(String.Format("'Page${0}'", command));
            }
            else
            {
                hl.NavigateUrl = "#";
            }
            return hl;
        }

        private string GetPagerScript(string page)
        {
            return String.Format("javascript:setCommand('refresh');__doPostBack('{0}', {1});"
                , this.ClientID.Replace(this.ClientIDSeparator, this.IdSeparator)
                , page);
        }

        // add for Define Set PagerPosition 
        // modi by Jawance 
        Boolean _isUserDefine = false;
        public bool IsUserDefinePagerPosition
        {
            set { _isUserDefine = value; }
            get { return _isUserDefine; }
        }
        protected override void Render(HtmlTextWriter writer)
        {
            if (!this.IsUserDefinePagerPosition)
            {
                this.PagerSettings.Position = PagerPosition.TopAndBottom;
            }

            base.Render(writer);
            _cs = Page.ClientScript;

            RegisterGridviewPageTextBoxOnKeyDownClientScripts();


        }


        protected void RegisterGridviewPageTextBoxOnKeyDownClientScripts()
        {
            _cs.RegisterStartupScript(typeof(GridView), GridviewPageTextBoxOnKeyDownClineKey
                , string.Format(@" function GridviewPageTextBoxOnKeyDown(objID,e)
                            {{
                        if (!((window.event.keyCode>=48 && window.event.keyCode<=57) || (window.event.keyCode>=96 && window.event.keyCode<=105) || window.event.keyCode==13 || window.event.keyCode==8 || window.event.keyCode==46)) 
                                         return false; 
                                      if( ('Page$' + e)!='Page$' && window.event.keyCode==13 ) 
                                        {{ if (typeof(__doPostBack)=='function') 
                                            __doPostBack(objID, 'Page$' + e);
                                        }} 
                            }}"
                 )
                , true);
        }


        private string GetPagerEnterScript(string page)
        {
            try
            {
                string sReturn = string.Empty;
                StringWriter sw = new StringWriter();

                System.Web.HttpContext.Current.Server.HtmlDecode(
                     String.Format("if (!((window.event.keyCode>=48 && window.event.keyCode<=57) || (window.event.keyCode>=96 && window.event.keyCode<=105) || window.event.keyCode==13 || window.event.keyCode==8 || window.event.keyCode==46)) "
                                  + "       return false; "
                                  + "   if( {1}!=\"Page$\" && window.event.keyCode==13 ) __doPostBack('{0}', {1});"
                     , this.ClientID.Replace(this.ClientIDSeparator, this.IdSeparator), page.Trim()), sw);

                return sw.ToString();
                //return String.Format("javascript:return false;", this.ClientID.Replace(this.ClientIDSeparator, this.IdSeparator), page);
            }
            catch
            {
                return String.Format("javascript:return false;");
            }
        }

        protected string GetCurrentModuleClass()
        {
            string cssclass = null;
            switch (SessionInfo.CurrentModule)
            {
                case Consts.BasicModule:
                    cssclass = "subwinpagetxt01";
                    break;
                case Consts.PersonModule:
                    cssclass = "subwinpagetxt02";
                    break;
                case Consts.AttendanceModule:
                    cssclass = "subwinpagetxt03";
                    break;
                case Consts.PayrollModule:
                    cssclass = "subwinpagetxt04";
                    break;
                case Consts.LeaveModule:
                    cssclass = "subwinpagetxt05";
                    break;
                case Consts.ReportModule:
                    cssclass = "subwinpagetxt06";
                    break;
                case Consts.SecurityModule:
                    cssclass = "subwinpagetxt07";
                    break;
                default:
                    cssclass = "subwinpagetxt00";
                    break;
            }
            cssclass = "sgvPagerContainer";
            return cssclass;
        }


        public string GetCurrentModuleClass(string cssclass)
        {
            return cssclass;
        }




    }//end of class
}