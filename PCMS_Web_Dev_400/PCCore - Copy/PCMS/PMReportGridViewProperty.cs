using System;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using System.Data;

namespace PCCore.PCMS
{
#region "Main Class"
    public class PmReportGridViewProperty
    {
        #region "Control Readonly/ Writable"

        public static void Enable(bool isEnable,PmReportGridView _gridview)
        {
            foreach (System.Web.UI.Control o in _gridview.Controls)
            {
                PCCore.Common.HRLog.RecordLog(o.GetType().Name);
                switch (o.GetType().Name)
                {
                       
                    case "Textbox":
                        ((TextBox)o).Enabled = isEnable;
                        break;
                    case "CheckBox":
                        ((CheckBox)o).Enabled = isEnable;
                        break;
                    case "SimpleTextBox":
                        ((PCCore.TextBox)o).Enabled = isEnable;
                        break;
                    case "Button":
                        ((Button)o).Enabled = isEnable;
                        break;
                    case "SimpleDropDownList":
                        ((PCCore.DropDownList)o).Enabled = isEnable;
                        break;
                }
            }
        }
        #endregion

        #region "Control in Column"
        public static void ActionBound(object sender, GridViewRowEventArgs e, int ActionColumn)
        {

            PCCore.Control.PmsReport.DeleteButton _delbtn = new PCCore.Control.PmsReport.DeleteButton("delBtn");
            e.Row.Cells[ActionColumn].HorizontalAlign = HorizontalAlign.Center;
            e.Row.Cells[ActionColumn].VerticalAlign = VerticalAlign.Middle;
            e.Row.Cells[ActionColumn].Controls.Add(_delbtn);
        }

        public static void DescriptionBound(object sender, GridViewRowEventArgs e, int Column)
        {
            PCCore.Common.HRLog.RecordLog("DescriptionBound");
            PCCore.Control.PmsReport.MultiLineTextBox _txtNote = new PCCore.Control.PmsReport.MultiLineTextBox("txtDesc");
            e.Row.Cells[Column].Controls.Add(_txtNote);
        }

        public static void TextBoxBound(object sender, GridViewRowEventArgs e, int Column)
        {
            PCCore.Common.HRLog.RecordLog("TextBoxBound");
            PCCore.Control.PmsReport.PMTextBox _txtNote = new PCCore.Control.PmsReport.PMTextBox("txtDesc");
            e.Row.Cells[Column].Controls.Add(_txtNote);
        }


        public static void NumericBound(object sender, GridViewRowEventArgs e, int Column, string id)
        {
            PCCore.TextBox _txtbox = new PCCore.TextBox();
            _txtbox.ID = id;
            _txtbox.DataType = SimpleControls.Web.SimpleTextBox.DataTypes.Double;
            _txtbox.Width = Unit.Percentage(100);
            e.Row.Cells[Column].Controls.Add(_txtbox);
        }

        public static void DateBound(object sender, GridViewRowEventArgs e, int Column, string id)
        {
            PCCore.TextBox _txtbox = new PCCore.TextBox();
            _txtbox.ID = id;
            _txtbox.DataType = SimpleControls.Web.SimpleTextBox.DataTypes.Date;
            _txtbox.Width = Unit.Percentage(100);
            e.Row.Cells[Column].Controls.Add(_txtbox);
        }
        #endregion
        
        public static void DeleteRecord(Object sender, GridViewCommandEventArgs e)
        {
            
        }
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            int index = Convert.ToInt32(e.CommandArgument);
            GridViewRow row = ((GridView)(e.CommandSource)).Rows[index];

            switch (e.CommandName)
            {
                case Consts.Add:
                    break;
                case Consts.Delete:
                    break;

            }

            switch (e.CommandSource.ToString())
            {

            }
        }

        #region "Footer"
        //public static void CreateFooter(PCCore.PmReportGridView gvData, string SectionId)
        //public static void CreateFooter(object sender, GridViewRowEventArgs e,string tablename)
        //{
        //    Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
        //    bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
        //    bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
        //    if (allowDelete)
        //    {
                
        //        PCCore.Control.PmsReport.MultiLineTextBox _txtNote = new PCCore.Control.PmsReport.MultiLineTextBox("newDesc");
        //        _txtNote.Width = Unit.Percentage(100);
        //        ge.Row.Cells[1].Controls.Add(_txtNote);
        //        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
        //        _addBtn.Width = Unit.Percentage(100);
        //        ge.Row.Cells[2].Controls.Add(_addBtn);
        //        break;

        //    }

        //}
        #endregion

        #region "Key"

        // Checking Error
        public static int getLineNum(DataTable _dt)
        {
            if (_dt == null)
            {
                return 1;
            }
            else
            {
                int maxLineNum = 0;
                PCCore.Common.HRLog.RecordTable("Datatable in getLineNum", _dt);
                foreach (DataRow _dr in _dt.Rows)
                {
                    PCCore.Common.HRLog.RecordLog("drLineNum: " + _dr[PMReportTable.PMR_COMM_Line_Number].ToString());
                    int curr_LineNum = Convert.ToInt32(_dr[PMReportTable.PMR_COMM_Line_Number]);
                    if (curr_LineNum > maxLineNum)
                        maxLineNum = curr_LineNum;
                }
                return maxLineNum + 1;
            }
        }
        #endregion
        #region "Control"

        public static void ClearControl(object sender, GridViewRowEventArgs e)
        {
            for (int i = 0; i < e.Row.Cells.Count; i++)
            {
                try
                {
                    e.Row.Cells[i].Controls.Clear();
                }
                catch (Exception ex)
                {
                    PCCore.Common.HRLog.RecordException("ClearControl", ex);
                }
            }
        }
        #endregion
        #region "GridView Column Structure"


        public static DataTable getDataTable(string tablename,string field)
        {
            
            string[] _field = field.Split(',');
            DataTable _dt = new DataTable();
            if (_field.Length > 0)
            {
                foreach (string _s in _field)
                {
                    DataColumn _dc = new DataColumn(_s);
                    _dc.Caption = _s;
                    // Set Caption, use fieldname when cannot find in label resource.


                    _dt.Columns.Add(_dc);

                }
            }
            // Add Row
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowAdd = (Convert.ToInt32(_htInfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htInfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            if (allowDelete || allowAdd)
            {
                _dt.Columns.Add(new DataColumn(PMReportTable.PMR_FD_Action));
            }
            PCCore.Common.HRLog.RecordTable("", _dt);
            return _dt;

        }

        public static void GridViewConfig(PmReportGridView _gridview, string tablename)
        {
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
        }
        #endregion
        #region "Data Row"
        public static string FindControl(TableCell _tc)
        {

            PCCore.Control.PmsReport.MultiLineTextBox _multxtBox;
            PCCore.Control.PmsReport.CurrencyBox _curtxtBox;
            PCCore.Control.PmsReport.NumericBox _numtxtBox;
            PCCore.Control.PmsReport.PMDateBox _dtetxtBox;
            PCCore.Control.PmsReport.PMTextBox _txtBox;


            foreach (System.Web.UI.Control o in _tc.Controls)
            {
                _multxtBox = null;
                _curtxtBox = null;
                _numtxtBox = null;
                _dtetxtBox = null;
                _txtBox = null;
                PCCore.Common.HRLog.RecordLog(o.ID.ToString());
                try
                { _txtBox = (PCCore.Control.PmsReport.PMTextBox)o; }
                catch (Exception ex)
                {// Error means that control is not correct component} }
                    PCCore.Common.HRLog.RecordException("", ex);
                }
                if (_txtBox != null)
                    return _txtBox.Text;

                try
                { _curtxtBox = (PCCore.Control.PmsReport.CurrencyBox)o; }
                catch (Exception ex)
                {// Error means that control is not correct component}
                    PCCore.Common.HRLog.RecordException("", ex);
                }
                if (_curtxtBox != null)
                    return _curtxtBox.Text;

                try
                { _numtxtBox = (PCCore.Control.PmsReport.NumericBox)o; }
                catch (Exception ex)
                {// Error means that control is not correct component}
                    PCCore.Common.HRLog.RecordException("", ex);
                }
                if (_numtxtBox != null)
                    return _numtxtBox.Text;

                try
                { _dtetxtBox = (PCCore.Control.PmsReport.PMDateBox)o; }
                catch (Exception ex)
                {// Error means that control is not correct component}
                    PCCore.Common.HRLog.RecordException("", ex);
                }
                if (_dtetxtBox != null)
                    return _dtetxtBox.Text;
                try
                { _multxtBox = (PCCore.Control.PmsReport.MultiLineTextBox)o; }
                catch (Exception ex)
                {// Error means that control is not correct component}
                    PCCore.Common.HRLog.RecordException("", ex);
                }
                if (_multxtBox != null)
                    return _multxtBox.Text;

                
            }
            // Cannot find any control, return Cell Text
            return _tc.Text;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_gridview"></param>
        /// <param name="fieldCollection"></param>
        /// <returns></returns>
        public static DataTable GridViewToDataTable(PmReportGridView _gridview,string tablename, string fieldCollection)
        {
            DataTable _dt = PmReportGridViewProperty.getDataTable(tablename, fieldCollection);
            
            PCCore.Common.HRLog.RecordLog("Row Count in GridView:" + _gridview.Rows.Count.ToString());

            foreach (GridViewRow _gvr in _gridview.Rows)
            {
                DataRow _dr = _dt.NewRow();

                if (_gvr.RowType == DataControlRowType.DataRow)
                {
                    for (int i = 0; i < _gvr.Cells.Count; i++)
                    {

                        TableCell _tc = _gvr.Cells[i];
                        string _val = PmReportGridViewProperty.FindControl(_tc);
                        try
                        {
                            _dr[i] = _val;
                        }
                        catch (Exception ex)
                        { }
                    }
                    _dt.Rows.Add(_dr);
                    
                }
            }
            return _dt;

        }

            /// <summary>
            /// 
            /// </summary>
            /// <param name="_dt"></param>
            /// <param name="_htInfo"></param>
            /// <returns></returns>
            public static DataTable InsertRow(DataTable _dt, Hashtable _htInfo)
            {
                DataRow _dr;
                if (_dt != null)
                {
                    
                    
                    for (int i = 0;i < Convert.ToInt32(_htInfo[PMReportTable.PMR_SecInfo_RowCount]);i++)

                    {
                        _dr = _dt.NewRow();
                        //Set Default Value for Control
                        foreach (DataColumn _dc in _dt.Columns)
                        {
                            _dr[_dc.ColumnName] = "_";
                        }
                        _dt.Rows.Add(_dr);
                    }
                }
                PCCore.Common.HRLog.RecordTable("", _dt);
                return _dt;
            }
        #endregion

        #region "Row Action"
            public static void RowDelete(Object sender, GridViewCommandEventArgs e, PmReportGridView _gridview)
            {
                GridViewRow _gvr = (GridViewRow)((PCCore.Control.PmsReport.DeleteButton)e.CommandSource).NamingContainer;
                System.Data.DataTable _dt;
                PCCore.Common.HRLog.RecordLog("RowDelete");
                int key = _gvr.RowIndex;
                _dt = (DataTable)_gridview.DataSource;
                PCCore.Common.HRLog.RecordLog("Datatable row count: " + _dt.Rows.Count);
                PCCore.Common.HRLog.RecordTable("dt on RowDelete", _dt);
                _dt.Rows[key].Delete();
                _gridview.DataSource = _dt;
                _gridview.DataBind();

                PCCore.Common.HRLog.RecordTable("dt on RowDelete", _dt);
                
                
                
            }
        #endregion
        
    }


    #region "Section1"
    public class Section1
    {
        
        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;
        
        
        public const string GridViewID = "gvData1";
        

        public static string tablename = PMReportTable.PMR_EXECSUMMARY;

        public Section1(PmReportGridView _gridview,string _reportid,string _periodcode,string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;
            

            switch (_rptmode)
            {
                case (int) Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int) Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int) Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }
            
            
        }
                
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt= null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {

                    //_gridview.DataSourceID = "dsGridView1";
                    //_gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }
                
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section1.setGridView()",ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section1.tablename,PMReportTable.SE_HT_FIELDS_PMR_EXECSUMMARY);
                // Add Row
                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section1.tablename);
        }


        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            
            //PCCore.GridView _gridview = (PCCore.GridView)e.CommandSource;
            PCCore.PmReportGridView _gridview = (PCCore.PmReportGridView) _page.FindControl(Section1.GridViewID);
            //for (int i = 0; i < _page.Controls.Count; i++)
            //{
            //    PCCore.Common.HRLog.RecordLog("ControlID:" + _page.Controls[i].ID);
            //}
            DataTable _dt = (DataTable)_gridview.DataSource;
            DataRow _newRow = _dt.NewRow();

            switch (e.CommandName.ToString())
            {

                case Consts.Add:
                    /* Key Check */

                    /* Get Maxiumn LineNumber */


                    break;
                case Consts.Delete:
                    PmReportGridViewProperty.RowDelete(sender, e, _gridview);
                    break;
            }
        }

        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section1 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);

            PCCore.Control.PmsReport.MultiLineTextBox _txtNote;

            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section1.DataRow");
                    e.Row.Cells[0].Width = Unit.Percentage(10);
                    e.Row.Cells[3].Width = Unit.Percentage(90);
                    
                    //Description Bound
                    //PCCore.PCMS.PmReportGridViewProperty.DescriptionBound(sender, e, 3);
                    _txtNote = new PCCore.Control.PmsReport.MultiLineTextBox("txtDesc");
                    _txtNote.Text = e.Row.Cells[3].Text;
                    e.Row.Cells[3].Controls.Add(_txtNote);

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[2].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 4);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section1.Footer");
                    _txtNote = new PCCore.Control.PmsReport.MultiLineTextBox("newDesc");
                    _txtNote.Width = Unit.Percentage(100);
                    e.Row.Cells[3].Controls.Add(_txtNote);
                    PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                    _addBtn.Width = Unit.Percentage(100);
                    e.Row.Cells[4].Controls.Add(_addBtn);
                    break;
            }
        }
        public void Save()
        {

        }
        public DataTable getDataTable()
        {
            return dtGridView;
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section1.tablename, PMReportTable.SE_HT_FIELDS_PMR_EXECSUMMARY);
        }

        #region Insert Record
        public static DataTable Insert(PCCore.PmReportGridView _gridview)
        {
            // Assign maximum line number 
            DataTable _dt = PmReportGridViewProperty.GridViewToDataTable(_gridview,Section1.tablename,PMReportTable.SE_HT_FIELDS_PMR_EXECSUMMARY);
            int NewLineNum = PmReportGridViewProperty.getLineNum(_dt);
            PCCore.Common.HRLog.RecordLog("Insert");
            foreach (GridViewRow _gvr in _gridview.Rows)
            {
                PCCore.Common.HRLog.RecordLog("Check Row");
                if (_gvr.RowType == DataControlRowType.Footer)
                {
                    PCCore.Common.HRLog.RecordLog("Footer");
                    if (((PCCore.Control.PmsReport.MultiLineTextBox) _gvr.Cells[3].Controls[0]).Text != String.Empty)
                    {
                        PCCore.Common.HRLog.RecordLog("NewLineNum: " + NewLineNum.ToString());
                        DataRow _dr = _dt.NewRow();
                        _dr[PMReportTable.PMR_COMM_Line_Number] = NewLineNum;
                        

                        _dr[PMReportTable.PMR_COMM_Description] = ((PCCore.Control.PmsReport.MultiLineTextBox)_gvr.Cells[3].Controls[0]).Text;
                        _dt.Rows.Add(_dr);
                    }
                }
            }
            return _dt;
        }
        #endregion

    }
    #endregion

    #region "Section2_1"
    public class Section2_1
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_PRJPARTICULAR;

        public Section2_1(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section2_1.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section2_1.tablename, "LineNum,Title,Value,DocEntry");
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section1.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;
            int dftRow;
            const string column1 = "linenum";
            const string column2 = "title";
            const string column3 = "value";
            const string column4 = "DocEntry";


            if (_dt == null)
            {
                // Retreive Data Structure
                //_dt = PmReportGridViewProperty.getDataTable(PMReportTable.SE_HT_FIELDS_PMR_EXECSUMMARY);
                _dt = new DataTable();
                // DataColumn Structure
                DataColumn _dc1 = new DataColumn(column1);
                // Will be Retreive from Resource
                
                _dc1.Caption = "LineNum";
                _dt.Columns.Add(_dc1);
                DataColumn _dc2 = new DataColumn(column2);
                _dc2.Caption = "Title";
                _dt.Columns.Add(_dc2);
                DataColumn _dc3 = new DataColumn(column3);
                _dc3.Caption = "Value";
                _dt.Columns.Add(_dc3);
                DataColumn _dc4 = new DataColumn(column4);
                _dc4.Caption = "DocEntry";
                _dt.Columns.Add(_dc4);
                
                // Add Row

                allowAdd = (Convert.ToInt32(_htInfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
                allowDelete = (Convert.ToInt32(_htInfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
                dftRow = (Convert.ToInt32(_htInfo[PMReportTable.PMR_SecInfo_RowCount]));
                if (allowDelete || allowAdd)
                {
                    _dt.Columns.Add(new DataColumn(PMReportTable.PMR_FD_Action));
                }
                //_dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
                // Insert Row
                PCCore.Common.HRLog.RecordTable("Section2_1",_dt);
                for (int i = 1;i<=dftRow;i++)
                {
                    DataRow _dr = _dt.NewRow();
                    _dr[column1] = i.ToString();
                    _dr[column3] = "_";
                    _dr[column4] = reportid;
                    switch (i)
                    {
                        case 1:
                            _dr[column2] = "Employer";
                            break;
                        case 2:
                            _dr[column2] = "Architect";
                            break;
                        case 3:
                            _dr[column2] = "Structural Engineer";
                        break;
                        case 4:
                            _dr[column2] = "B.S. Engineer";
                        break;
                        case 5:
                            _dr[column2] = "Q.S. Consultant";
                        break;
                        case 6:
                            _dr[column2] = "Contract Commencement Date";
                        break;
                        case 7:
                            _dr[column2] = "Original Contract Completion Date";
                        break;
                        case 8:
                            _dr[column2] = "Original Contract Duration";
                        break;
                        case 9:
                            _dr[column2] = "Extended Contract Completion Date";
                        break;
                        case 10:
                            _dr[column2] = "Extended Contract Duration";
                        break;
                        case 11:
                            _dr[column2] = "Anticipated Contract Completion Date ";
                        break;
                        case 12:
                            _dr[column2] = "Time Elapsed";
                        break;
                        case 13:
                            _dr[column2] = "% of Time Elapsed";
                        break;
                        case 14:
                            _dr[column2] = "Original Contract Value ";
                        break;
                        case 15:
                            _dr[column2] = "Forecast Final Contract Sum";
                        break;
                        case 16:
                            _dr[column2] = "Amount Certified ";
                        break;
                        case 17:
                            _dr[column2] = "% of Amount Certified";
                        break;
                    }
                    _dt.Rows.Add(_dr);
                }

            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();
            PCCore.Common.HRLog.RecordTable("Section2_1", _dt);
            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e, Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_1 RowDataBound");
            
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    //PmReportGridViewProperty.ClearControl(sender, e);
                    e.Row.Cells[1].Width = Unit.Percentage(30);
                    e.Row.Cells[2].Width = Unit.Percentage(70);
                    
                    PCCore.Control.PmsReport.PMDateBox _datebox = new PCCore.Control.PmsReport.PMDateBox("_txt");
                    PCCore.Control.PmsReport.PMTextBox _txtbox = new PCCore.Control.PmsReport.PMTextBox("_txt");
                    PCCore.Control.PmsReport.NumericBox _numbox = new PCCore.Control.PmsReport.NumericBox("_txt");
                    PCCore.Control.PmsReport.CurrencyBox _currbox = new PCCore.Control.PmsReport.CurrencyBox("_txt");
                    
                    _txtbox.Enabled = true;
                    _txtbox.Width = Unit.Percentage(90);
                    _datebox.Enabled = true;
                    _datebox.Width = Unit.Percentage(90);
                    _numbox.Enabled = true;
                    _numbox.Width = Unit.Percentage(90);
                    _currbox.Enabled = true;
                    _currbox.Width = Unit.Percentage(90);
                    PCCore.Common.HRLog.RecordLog("LineNun: " + Convert.ToInt32(e.Row.Cells[0].Text.ToString()));
                    switch (Convert.ToInt32(e.Row.Cells[0].Text.ToString()))
                    {
                        case 1:
                            PCCore.Common.HRLog.RecordLog("LineNum 1");
                            e.Row.Cells[1].Controls.Clear();
                            e.Row.Cells[2].Controls.Add(_txtbox);
                            break;
                        case 2:
                            PCCore.Common.HRLog.RecordLog("LineNum 2");
                            e.Row.Cells[2].Controls.Add(_txtbox);
                            break;
                        case 3:
                            PCCore.Common.HRLog.RecordLog("LineNum 3");
                            e.Row.Cells[2].Controls.Add(_txtbox);
                            break;
                        case 4:
                            PCCore.Common.HRLog.RecordLog("LineNum 4");
                            e.Row.Cells[2].Controls.Add(_txtbox);
                            break;
                        case 5:
                            PCCore.Common.HRLog.RecordLog("LineNum 5");
                            e.Row.Cells[2].Controls.Add(_txtbox);
                            break;
                        case 6:
                            PCCore.Common.HRLog.RecordLog("LineNum 6");
                            e.Row.Cells[2].Controls.Add(_txtbox);
                            break;
                        case 7:
                            PCCore.Common.HRLog.RecordLog("LineNum 7");
                            e.Row.Cells[2].Controls.Add(_datebox);
                            break;
                        case 8:
                            PCCore.Common.HRLog.RecordLog("LineNum 8");
                            e.Row.Cells[2].Controls.Add(_numbox);
                            break;
                        case 9:
                            PCCore.Common.HRLog.RecordLog("LineNum 9");
                            e.Row.Cells[2].Controls.Add(_datebox);
                            break;
                        case 10:
                            PCCore.Common.HRLog.RecordLog("LineNum 10");
                            e.Row.Cells[2].Controls.Add(_numbox);
                            break;
                        case 11:
                            PCCore.Common.HRLog.RecordLog("LineNum 11");
                            e.Row.Cells[2].Controls.Add(_datebox);
                            break;
                        case 12:
                            PCCore.Common.HRLog.RecordLog("LineNum 12");
                            e.Row.Cells[2].Controls.Add(_numbox);
                            break;
                        case 13:
                            PCCore.Common.HRLog.RecordLog("LineNum 13");
                            e.Row.Cells[2].Controls.Add(_numbox);
                            break;
                        case 14:
                            PCCore.Common.HRLog.RecordLog("LineNum 14");
                            _currbox.Enabled = false;
                            e.Row.Cells[2].Controls.Add(_currbox);
                            break;
                        case 15:
                            PCCore.Common.HRLog.RecordLog("LineNum 15");
                            _currbox.Enabled = false;
                            e.Row.Cells[2].Controls.Add(_currbox);
                            break;
                        case 16:
                            PCCore.Common.HRLog.RecordLog("LineNum 16");
                            _currbox.Enabled = false;
                            e.Row.Cells[2].Controls.Add(_currbox);
                            break;
                        case 17:
                            PCCore.Common.HRLog.RecordLog("LineNum 17");
                            _numbox.Enabled = false;
                            e.Row.Cells[2].Controls.Add(_numbox);
                            break;

                    }
                    break;
                case DataControlRowType.Footer:
                    
                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section2_2"
    public class Section2_2
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_ScopeOfWorks;

        public Section2_2(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section2_2.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section2_2.tablename, PMReportTable.SE_HT_FIELDS_PMR_ScopeOfWorks);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_2.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section2_2.tablename,PMReportTable.SE_HT_FIELDS_PMR_ScopeOfWorks);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e, Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_2 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_2.DataRow");
                    e.Row.Cells[0].Width = Unit.Percentage(10);
                    e.Row.Cells[3].Width = Unit.Percentage(90);
                    PCCore.PCMS.PmReportGridViewProperty.DescriptionBound(sender, e, 3);
                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[2].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 4);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_2.Footer");
                    PCCore.Control.PmsReport.MultiLineTextBox _txtNote = new PCCore.Control.PmsReport.MultiLineTextBox("newDesc");
                    _txtNote.Width = Unit.Percentage(100);
                    e.Row.Cells[3].Controls.Add(_txtNote);
                    PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                    _addBtn.Width = Unit.Percentage(100);
                    e.Row.Cells[4].Controls.Add(_addBtn);
                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section2_3"
    public class Section2_3
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_OverallProgress;

        public Section2_3(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section2_3.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section2_3.tablename, PMReportTable.SE_HT_FIELDS_PMR_OverallProgress);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section1.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section2_3.tablename,PMReportTable.SE_HT_FIELDS_PMR_OverallProgress);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e, Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section1 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section1.DataRow");
                    e.Row.Cells[0].Width = Unit.Percentage(10);
                    e.Row.Cells[3].Width = Unit.Percentage(90);
                    PCCore.PCMS.PmReportGridViewProperty.DescriptionBound(sender, e, 3);
                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[2].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 4);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section1.Footer");
                    PCCore.Control.PmsReport.MultiLineTextBox _txtNote = new PCCore.Control.PmsReport.MultiLineTextBox("newDesc");
                    _txtNote.Width = Unit.Percentage(100);
                    e.Row.Cells[3].Controls.Add(_txtNote);
                    PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                    _addBtn.Width = Unit.Percentage(100);
                    e.Row.Cells[4].Controls.Add(_addBtn);
                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section2_4"
    public class Section2_4
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_ProgressSummary;

        public Section2_4(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section2_4.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section2_4.tablename, PMReportTable.SE_HT_FIELDS_PMR_ProgressSummary);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section1.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section2_4.tablename,PMReportTable.SE_HT_FIELDS_PMR_ProgressSummary);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e, Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section1 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_4.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtCode",80));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.PMDateBox("_txtDate1",80));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.PMDateBox("_txtDate2", 80));
                    e.Row.Cells[6].Controls.Add(new PCCore.Control.PmsReport.NumericBox("_txtDay", 80));
                    e.Row.Cells[7].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtPeriod", 70));
                    e.Row.Cells[8].Controls.Add(new PCCore.Control.PmsReport.PMDateBox("_txtDate3",80));
                    
                    //e.Row.Cells[0].Width = Unit.Percentage(10);
                    //e.Row.Cells[3].Width = Unit.Percentage(90);
                    //PCCore.PCMS.PmReportGridViewProperty.DescriptionBound(sender, e, 3);
                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[9].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 9);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_4.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addCode", 80));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.PMDateBox("_addDate1", 80));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.PMDateBox("_addDate2", 80));
                    e.Row.Cells[6].Controls.Add(new PCCore.Control.PmsReport.NumericBox("_addDay", 80));
                    e.Row.Cells[7].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addPeriod", 70));
                    e.Row.Cells[8].Controls.Add(new PCCore.Control.PmsReport.PMDateBox("_addDate3", 80));
                    
                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[9].Controls.Add(_addBtn);
                    }
                    
                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section2_5"
    public class Section2_5
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Instruction;

        public Section2_5(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section2_5.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section2_5.tablename, PMReportTable.SE_HT_FIELDS_PMR_Instruction);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_5.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section2_6.tablename,PMReportTable.SE_HT_FIELDS_PMR_Instruction);
                // Add Row

                
                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e, Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section1 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_5.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));
                    
                    

                    //e.Row.Cells[0].Width = Unit.Percentage(10);
                    //e.Row.Cells[3].Width = Unit.Percentage(90);
                    //PCCore.PCMS.PmReportGridViewProperty.DescriptionBound(sender, e, 3);
                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[5].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 5);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_5.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));
                    
                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[5].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section2_6"
    public class Section2_6
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Information;

        public Section2_6(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section2_6.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section2_6.tablename, PMReportTable.SE_HT_FIELDS_PMR_Information);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section2_6.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e, Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType",90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));
                    
                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));
                    
                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section3_1"
    public class Section3_1
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Information;

        public Section3_1(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section3_1.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            // Receive from Table
            //return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section3_1.tablename, PMReportTable.se_ht_.SE_HT_FIELDS_PMR_ScopeOfWorks);
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section3_1.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section3_2"
    public class Section3_2
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Information;

        public Section3_2(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section3_2.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            //return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section3_2.tablename, PMReportTable.SE_HT_FIELDS_PMR_ScopeOfWorks);
            return null;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section3_2.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                
                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section3_3"
    public class Section3_3
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Information;

        public Section3_3(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section3_3.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section3_3.tablename, PMReportTable.SE_HT_FIELDS_PMR_DiffReason);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section3_3.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section3_2_1"
    public class Section3_2_1
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Information;

        public Section3_2_1(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section3_2_1.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section3_2_1.tablename, PMReportTable.SE_HT_FIELDS_PMR_DiffReason);
            
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section3_2_1.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section3_4"
    public class Section3_4
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Information;

        public Section3_4(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section3_4.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section3_4.tablename, PMReportTable.SE_HT_FIELDS_PMR_EOTClaims);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section3_4.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                
                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section3_4_1"
    public class Section3_4_1
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Information;

        public Section3_4_1(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section3_4_1.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section3_4_1.tablename, PMReportTable.SE_HT_FIELDS_PMR_EOTDesc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section3_4.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                
                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion
    #region "Section3_5"
    public class Section3_5
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Information;

        public Section3_5(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section3_5.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section3_5.tablename, PMReportTable.SE_HT_FIELDS_PMR_LD);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section3_5.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section3_5_1"
    public class Section3_5_1
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Information;

        public Section3_5_1(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section3_5_1.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section3_5_1.tablename, PMReportTable.SE_HT_FIELDS_PMR_LDCmmt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section3_5_1.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                
                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section3_6"
    public class Section3_6
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Information;

        public Section3_6(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section3_6.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section3_6.tablename, PMReportTable.SE_HT_FIELDS_PMR_CostClaimsStatus);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section3_6.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section3_6_1"
    public class Section3_6_1
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Information;

        public Section3_6_1(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section3_6_1.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section3_6_1.tablename, PMReportTable.SE_HT_FIELDS_PMR_CostClaimsCmmt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section3_6_1.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section4_1"
    public class Section4_1
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Information;

        public Section4_1(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section4_1.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section4_1.tablename, PMReportTable.SE_HT_FIELDS_PMR_ManPower);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section4_1.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                
                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section4_2"
    public class Section4_2
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Information;

        public Section4_2(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section4_2.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section4_2.tablename, PMReportTable.SE_HT_FIELDS_PMR_ManpowerPlant);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section4_2.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section4_3"
    public class Section4_3
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_ResDesc;

        public Section4_3(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section4_3.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section4_3.tablename, PMReportTable.SE_HT_FIELDS_PMR_ResDesc);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section4_3.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section5_1"
    public class Section5_1
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Accident;

        public Section5_1(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section5_1.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section5_1.tablename, PMReportTable.SE_HT_FIELDS_PMR_Accident);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section5_1.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section5_2"
    public class Section5_2
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Penalty;

        public Section5_2(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section5_2.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section5_2.tablename, PMReportTable.SE_HT_FIELDS_PMR_Penalty);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section5_2.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section5_3"
    public class Section5_3
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_AccidentCmmt;

        public Section5_3(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section5_3.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section5_3.tablename, PMReportTable.SE_HT_FIELDS_PMR_AccidentCmmt);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section5_3.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section6"
    public class Section6
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_QtyAssIssues;

        public Section6(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section6.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section6.tablename, PMReportTable.SE_HT_FIELDS_PMR_QtyAssIssues);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section6.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

    #region "Section7"
    public class Section7
    {

        public PmReportGridView _gridview;
        public DataTable dtGridView;
        public string reportid;
        public string periodcode;
        public string PrjCode;
        public int rptmode;


        public static string tablename = PMReportTable.PMR_Images;

        public Section7(PmReportGridView _gridview, string _reportid, string _periodcode, string _PrjCode, int _rptmode)
        {
            this._gridview = _gridview;
            this.reportid = _reportid;
            this.periodcode = _periodcode;
            this.PrjCode = _PrjCode;
            this.rptmode = _rptmode;

            switch (_rptmode)
            {
                case (int)Consts.ReportMode.Edit:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;
                case (int)Consts.ReportMode.View:
                    dtGridView = this.setGridView(this.reportid);
                    PmReportGridViewProperty.Enable(false, _gridview);
                    break;
                case (int)Consts.ReportMode.New:
                    dtGridView = this.InitGridView();
                    PmReportGridViewProperty.Enable(true, _gridview);
                    break;

            }


        }
        public static void GridViewConfig(PmReportGridView gridview)
        {
            PmReportGridViewProperty.GridViewConfig(gridview, Section7.tablename);
        }
        public static DataTable GridViewtoDataTable(PCCore.PmReportGridView _gridview)
        {
            return PmReportGridViewProperty.GridViewToDataTable(_gridview, Section7.tablename, PMReportTable.SE_HT_FIELDS_PMR_Images);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="_dt"></param>
        /// 
        public DataTable setGridView(string id)
        {
            DataTable _dt = null;
            string sql = string.Format("select * from {0} where {1} = {2} order by {3} asc", tablename, PMReportTable.PMR_COMM_ID, id, PMReportTable.PMR_COMM_Line_Number);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt != null)
                {
                    _gridview.DataSourceID = null;
                    _gridview.DataSource = _dt;
                    _gridview.DataBind();
                }
            }

            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Section2_6.setGridView()", ex);
            }
            return _dt;
        }

        public DataTable InitGridView()
        {
            DataTable _dt = null;
            Hashtable _htInfo = PMReportTable.SectionInfo(tablename);
            bool allowDelete;
            bool allowAdd;

            if (_dt == null)
            {
                // Retreive Data Structure
                _dt = PmReportGridViewProperty.getDataTable(Section7.tablename,PMReportTable.SE_HT_FIELDS_PMR_Information);
                // Add Row

                _dt = PmReportGridViewProperty.InsertRow(_dt, _htInfo);
            }
            _gridview.HiddenFields += _htInfo[PMReportTable.PMR_SecInfo_HiddenField];
            _gridview.DataSourceID = null;
            _gridview.DataSource = _dt;
            _gridview.DataBind();

            return _dt;
        }

        /// <summary>
        /// 
        /// </summary>
        public static void RowCommand(Object sender, GridViewCommandEventArgs e,Page _page)
        {
            switch (e.CommandName.ToString())
            {
                case Consts.Add:

                    break;
                case Consts.Delete:

                    break;
            }
        }
        public static void RowDataBound(object sender, GridViewRowEventArgs e)
        {
            PCCore.Common.HRLog.RecordLog("Inner Section2_6 RowDataBound");
            PCCore.Common.HRLog.RecordLog("Sender: " + sender.ToString());
            Hashtable _htinfo = PMReportTable.SectionInfo(Section1.tablename);
            bool allowAdd = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowAdd]) == 1);
            bool allowDelete = (Convert.ToInt32(_htinfo[PMReportTable.PMR_SecInfo_allowDelete]) == 1);
            switch (e.Row.RowType)
            {
                case DataControlRowType.DataRow:
                    PCCore.Common.HRLog.RecordLog("Section2_6.DataRow");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_txtType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_txtCumCurr", 90));

                    if (allowAdd || allowDelete)
                    {
                        e.Row.Cells[6].Width = Unit.Percentage(10);
                        PCCore.PCMS.PmReportGridViewProperty.ActionBound(sender, e, 6);
                    }
                    break;
                case DataControlRowType.Footer:
                    PCCore.Common.HRLog.RecordLog("Section2_6.Footer");
                    e.Row.Cells[3].Controls.Add(new PCCore.Control.PmsReport.PMTextBox("_addType", 90));
                    e.Row.Cells[4].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumTotal", 90));
                    e.Row.Cells[5].Controls.Add(new PCCore.Control.PmsReport.CurrencyBox("_addCumCurr", 90));

                    if (allowAdd)
                    {
                        PCCore.Control.PmsReport.AddButton _addBtn = new PCCore.Control.PmsReport.AddButton("btnAdd");
                        _addBtn.Width = Unit.Percentage(100);
                        e.Row.Cells[6].Controls.Add(_addBtn);
                    }

                    break;
            }
        }
        public void Save()
        {

        }
        public void getDataTable()
        {

        }

    }
    #endregion

}


#endregion
