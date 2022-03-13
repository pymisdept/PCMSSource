using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Data.Common;
using System.Collections;

namespace PCCore.PCMS
{
    public class PMReport
    {
        public string prjcode;
        public string prjname;
        public int version_nbr;
        public PMReportTable _pmobject;
        public int last_period_year;
        public int last_period_month;
        public string last_period;
        public string periodid;
        public string period;
        public DateTime last_period_from;
        public DateTime last_period_to;
        public Boolean b_canModify;
        public Boolean b_isNewRecord;
        public Boolean b_canCancel;

        const string main_filter = "PRJCODE = ";
        /// <summary>
        /// For search period
        /// </summary>
        /// <param name="ProjectCode"></param>
        public PMReport(string ProjectCode)
        //: this(ProjectCode, "-3", new PMReportTable(), true)
        {
            this.prjcode = ProjectCode;
            //FindLastPeriod();
        }
        /// <summary>
        /// for get record from PMCS Database,
        /// If period id smaller then 0, it's new pm's report and get information from B1
        /// 
        /// </summary>
        /// <param name="ProjectCode"></param>
        /// <param name="periodid"></param>
        public PMReport(string ProjectCode, string periodid, string period)
            : this(ProjectCode, periodid, period, new PMReportTable(), true)
        {

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="prjcode"></param>
        /// <param name="periodid"></param>
        /// <param name="_PMReportTable"></param>
        /// 
        public PMReport(string ProjectCode, string periodid, string period, PMReportTable _PMReportTable, Boolean getData)
        {
            if (Convert.ToInt32(periodid) < 1)
                b_isNewRecord = true;
            else
                b_isNewRecord = false;
            SessionInfo.PMRptPeriod = period;
            SessionInfo.PMRptProject = ProjectCode;
            PCCore.Database.Project prj = new PCCore.Database.Project(ProjectCode);
            this._pmobject = _PMReportTable;
            this.prjname = prj.ProjectName();
            this.prjcode = ProjectCode;
            this.periodid = periodid;
            this.period = period;

            if (this.period.LastIndexOf('-') >= 0)
            {
                this.last_period_year = Convert.ToInt32(this.period.Substring(0, this.period.LastIndexOf('-')));
                this.last_period_month = Convert.ToInt32(this.period.Substring(this.period.LastIndexOf('-') + 1));
                this.last_period_from = new DateTime(this.last_period_year, this.last_period_month, 1);
                this.last_period_to = this.last_period_from.AddMonths(1).AddDays(-1);
            }

            if (getData)
            {
                if (isNewRecord())
                {
                    //version_nbr = 1;
                    // GET PROJECT INFORMATION FROM PROJECT TABLE
                    ProjectInfo();
                }
                else
                {
                    // GET MAX VERSION NUMBER 
                    //CurrentVersionNumber();
                    // GET PROJECT INFORMATION FROM PM'S REPORT TABLE
                    PMReportInfo();
                }

            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// 

        public string NewPeriodRunningCode(string projcode)
        {
            string code = "";
            
            DataTable _dt;
            string sql;
            sql = string.Format("SELECT TOP 1 DocRunningNum FROM {0} WHERE ProjectCode = '{1}' ORDER BY DOCRUNNINGNUM DESC", "CPS_View_PMReportInfo", this.prjcode);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                {
                    code = projcode + "/" + (Convert.ToInt32(_dt.Rows[0][0]) + 1).ToString("D3");
                    PCCore.Common.HRLog.RecordLog("code:" + code);
                }
                else
                {
                    sql = string.Format("SELECT TOP 1 DocRunningNum FROM {0} WHERE PrjCode = '{1}' ORDER BY DOCRUNNINGNUM DESC", "CPS_View_OSPMReportInfo", this.prjcode);
                    _dt = PCDb.Db.ExecQuery(sql);
                    if (_dt.Rows.Count > 0)
                    {
                        code = projcode + "/" + (Convert.ToInt32(_dt.Rows[0][0]) + 1).ToString("D3");
                    }
                    else
                    {
                        code = projcode + "/001";
                    }
                    
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("NewPeriodRunningID", ex);
            }
            return code;
        }

        //private void FindLastPeriod()
        //{
        //    DataTable _dt;
        //    string sql;
        //    DateTime _periodfrom;
        //    DateTime _periodto;

        //    if (this.periodid != null)
        //    {
        //        if (Convert.ToInt32(this.periodid) > 0)
        //        {
        //            sql = string.Format("SELECT * FROM {0} WHERE {1} = {2}", PMReportTable.PMR_Mstr, PMReportTable.PMR_COMM_ID, this.periodid);
        //            try
        //            {
        //                _dt = PCDb.Db.ExecQuery(sql);
        //                if (_dt.Rows.Count > 0)
        //                {
        //                    _periodfrom = Convert.ToDateTime(Convert.ToDateTime(_dt.Rows[0][PMReportTable.PMR_MSTR_Period_From]));
        //                    _periodto = Convert.ToDateTime(Convert.ToDateTime(_dt.Rows[0][PMReportTable.PMR_MSTR_Period_to]));
        //                    last_period_month = _periodfrom.Month;
        //                    last_period_year = _periodfrom.Year;
        //                }
        //                else
        //                {
        //                    // Error
        //                }
        //            }
        //            catch (Exception ex)
        //            {
        //                throw ex;
        //            }
        //        }
        //        else
        //        {

        //        }
        //    }
        //}
        Boolean CanModify()
        {

            return PCCore.PCMS.PMReport.CanModify(this.prjcode, this.periodid);
        }

        public static Boolean CanModify(string projcode, string periodid)
        {
            DataTable _dt;
            string sql = string.Format("SELECT 1 FROM {0} WHERE {1} = '{2}' AND {3} = '{4}' AND {5} > (SELECT TOP 1 {5} FROM {0} WHERE {6} = {7})", PMReportTable.PMR_Mstr, PMReportTable.PMR_MSTR_Project_Code, projcode, PMReportTable.PMR_MSTR_Document_Status, PMReportTable.PMR_CLOSE_STATUS, PMReportTable.PMR_MSTR_Period_From, PMReportTable.PMR_COMM_ID, periodid);
            PCCore.Common.HRLog.RecordLog(sql);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                    return false;
                else
                    return true;
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordLog("CanModify", ex);
                return false;
            }

        }
        Boolean CanCancel()
        {
            return PCCore.PCMS.PMReport.CanCancel(this.prjcode, this.period);
        }
        public static Boolean CanCancel(string prjcode, string periodid)
        {
            return CanModify(prjcode, periodid);
        }
        Boolean ProjectInfo()
        {

            DataTable _dt = ExecuteDataTable("OPRJ", "*", false);
            if (_pmobject.ht_PMRPT.Count != PMReportTable.conTableCount)
            {
                _pmobject = new PMReportTable();
            }
            DataTable _dtMstr = (DataTable)_pmobject.ht_PMRPT[PMReportTable.PMR_Mstr];
            if (_dtMstr.Rows.Count == 0)
                _dtMstr.Rows.Add(_dtMstr.NewRow());
            try
            {
                _dtMstr.Rows[0][PMReportTable.PMR_MSTR_Project_Code] = prjcode;
                _dtMstr.Rows[0][PMReportTable.PMR_MSTR_Period] = this.period;
                _dtMstr.Rows[0][PMReportTable.PMR_MSTR_Period_From] = new DateTime(this.last_period_year, this.last_period_month, 1);
                _dtMstr.Rows[0][PMReportTable.PMR_MSTR_Period_to] = new DateTime(this.last_period_year, this.last_period_month, 1).AddMonths(1).AddDays(-1);
                _dtMstr.Rows[0][PMReportTable.PMR_MSTR_Create_Date] = DateTime.Now.ToString();
                _dtMstr.Rows[0][PMReportTable.PMR_MSTR_User_Sign1] = SessionInfo.UserId;
                _dtMstr.Rows[0][PMReportTable.PMR_MSTR_PCMS_DOCNUM] = ""; // Running Number for PM's Report
                _dtMstr.Rows[0][PMReportTable.PMR_MSTR_Document_Status] = PMReportTable.PMR_OPEN_STATUS;
            }
            catch (Exception ex)
            { PCCore.Common.HRLog.RecordException("ProjectInfo", ex); }
            // GET OTHER INFORMATION OF CURRENT PROJECT
            // GET PAYMENT INFORMATION
            _pmobject.ht_PMRPT[PMReportTable.PMR_Mstr] = _dtMstr;
            // Assign Project Code and PCMS_DocNum

            return true;

        }

        public Boolean CheckOpenReport()
        {
            string sql = string.Format("SELECT TOP 1 * FROM {0} WHERE {1} = {2} ORDER BY {3} DESC, {4} DESC", PMReportTable.PMR_Mstr, PMReportTable.PMR_MSTR_Project_Code, "'" + this.prjcode + "'" , PMReportTable.PMR_MSTR_Period_From, PMReportTable.PMR_COMM_ID);
            DataTable _dt;
            DateTime _lastDateTime;
            int period_month;
            int period_year;
            Boolean b_open = false;

            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                {
                    switch (_dt.Rows[0][PMReportTable.PMR_MSTR_Document_Status].ToString().Trim())
                    {
                        case PMReportTable.PMR_OPEN_STATUS:
                            b_open = true;
                            break;
                        case PMReportTable.PMR_CLOSE_STATUS:
                            b_open = false;
                            break;
                        case PMReportTable.PMR_CANCEL_STATUS:
                            b_open = false;

                            break;
                    }

                    if (!b_open)
                    {
                        b_open = false;
                        // ASSIGN NEW PERIOD
                        _lastDateTime = Convert.ToDateTime(_dt.Rows[0][PMReportTable.PMR_MSTR_Period_From]);
                        period_month = _lastDateTime.Month;
                        period_year = _lastDateTime.Year;
                        if (period_month == 12)
                        {
                            last_period_month = 1;
                            last_period_year = period_year + 1;

                        }
                        else
                        {
                            last_period_month = period_month + 1;
                            last_period_year = period_year;
                        }
                        last_period = PCCore.ComFunction2.LeftString(last_period_year.ToString(), 4, '0') + "-" + PCCore.ComFunction2.LeftString(last_period_month.ToString(), 2, '0');
                        last_period_from = new DateTime(last_period_year, last_period_month, 1);
                        last_period_to = last_period_from.AddMonths(1).AddDays(-1);

                    }



                }
                else
                {
                    b_open = false;
                    // get value from setting table
                    sql = string.Format("SELECT * FROM {0} WHERE {1} = '{2}'", "sy_systemsetting", "code", PMReportTable.PMR_START_DATE);

                    _dt = PCDb.Db.ExecQuery(sql);
                    if (_dt.Rows.Count > 0)
                    {
                        try
                        {
                            _lastDateTime = Convert.ToDateTime(_dt.Rows[0]["value"]);
                            last_period_month = _lastDateTime.Month;
                            last_period_year = _lastDateTime.Year;
                            last_period = PCCore.ComFunction2.LeftString(last_period_year.ToString(), 4, '0') + "-" + PCCore.ComFunction2.LeftString(last_period_month.ToString(), 2, '0');
                        }
                        catch (Exception ex)
                        {

                        }

                    }
                }

            }
            catch (Exception ex)
            {
            }
            return b_open;
        }

        Boolean PMReportInfo()
        {

            //Set PM's Report Info to DataTable in HashTable
            Hashtable _htProj = new Hashtable();
            if (_pmobject.ht_PMRPT.Count != PMReportTable.conTableCount)
            {
                _pmobject = new PMReportTable();
            }

            try
            {

                foreach (object o in _pmobject.ht_PMRPT.Keys)
                {
                    string _s = o.ToString();
                    PCCore.Common.HRLog.RecordLog("PMReport: " + o.ToString());
                    DataTable _dt = ExecuteDataTable(_s, PMReportTable.PMR_COMM_ID + "=" + this.periodid);

                    try
                    {
                        _htProj.Add(_s, _dt);
                    }
                    catch (Exception ex)
                    {

                    }

                }
                // Update Project Information into this session
                try
                {
                    ((DataTable)_htProj[PMReportTable.PMR_Mstr]).Rows[0][PMReportTable.PMR_MSTR_User_Sign2] = SessionInfo.UserId;
                    ((DataTable)_htProj[PMReportTable.PMR_Mstr]).Rows[0][PMReportTable.PMR_MSTR_Update_Date] = DateTime.Now.ToString();
                }
                catch (Exception ex)
                {

                }
                _pmobject.ht_PMRPT = _htProj;
            }
            catch (Exception ex)
            {
            }
            return true;

        }
        Boolean isNewRecord()
        {

            if (ExecuteHeader())
            {
                return false;
            }
            else
            {
                return true;
            }
        }

        Boolean Exists()
        {
            return true;
        }

        Boolean ReadSession()
        {
            return true;
        }

        Boolean ReadTable()
        {
            return true;
        }

        public Boolean Save()
        {
            decimal rptID;
            if (b_isNewRecord)
                rptID = PCDb.GetNextPMRptID();
            else
                rptID = Convert.ToInt32(this.periodid);


            Hashtable _htValue = PCCore.Common.XMLPMSession.Read();
            // Assign Docentry to Header Table
            ((Hashtable)((Hashtable)_htValue[PMReportTable.PMR_Mstr])["1"])[PMReportTable.PMR_COMM_ID] = rptID;

            foreach (object o in _htValue.Keys)
            {
                Hashtable _htTable = (Hashtable)_htValue[o.ToString()];

                foreach (object _o in _htTable.Keys)
                {
                    string _sql = "";
                    string _fieldset = "";
                    string _valueset = "";
                    Hashtable _htObject = (Hashtable)_htTable[_o.ToString()];
                    _htObject[PMReportTable.PMR_MSTR_Project_Code] = this.prjcode;
                    _htObject[PMReportTable.PMR_COMM_ID] = rptID;
                    _htObject[PMReportTable.PMR_MSTR_PCMS_DOCNUM] = "";

                    if (isRecordExists(o.ToString(), rptID, Convert.ToInt32(_o)))
                    {
                        foreach (object __o in _htObject.Keys)
                        {

                            _fieldset += string.Format("{0} = '{1}',", __o.ToString(), _htObject[__o.ToString()]);
                            PCCore.Common.HRLog.RecordLog("FieldSet: " + _fieldset);
                        }
                        _fieldset = _fieldset.Substring(0, _fieldset.Length - 1);
                        // Update Record
                        if (o.ToString() != PMReportTable.PMR_Mstr && o.ToString() != PMReportTable.PMR_PRJPARTICULAR)
                            _sql = string.Format("UPDATE {0} SET {1} WHERE {4} = {2} AND {5} ={3}", o.ToString(), _fieldset, rptID, _o.ToString(), PMReportTable.PMR_COMM_ID, PMReportTable.PMR_COMM_Line_Number);
                        else
                            _sql = string.Format("UPDATE {0} SET {1} WHERE {3}= {2}", o.ToString(), _fieldset, rptID, PMReportTable.PMR_COMM_ID);
                        PCCore.Common.HRLog.RecordLog("_sql: " + _sql);
                        try
                        {
                            int ret = PCDb.Db.ExecUpdate(_sql);
                        }
                        catch (Exception ex)
                        { PCCore.Common.HRLog.RecordException("Save: " + o.ToString(), ex); }

                    }
                    else
                    {
                        foreach (object __o in _htObject.Keys)
                        {
                            PCCore.Common.HRLog.RecordLog("Value: " + _htObject[__o.ToString()]);
                            _fieldset += string.Format("{0},", __o.ToString());
                            _valueset += string.Format("'{0}',", _htObject[__o.ToString()]);

                        }
                        _fieldset = _fieldset.Substring(0, _fieldset.Length - 1);
                        _valueset = _valueset.Substring(0, _valueset.Length - 1);
                        // Insert Record
                        _sql = string.Format("INSERT INTO {0} ({1}) values ( {2} )", o.ToString(), _fieldset, _valueset);
                        PCCore.Common.HRLog.RecordLog("_sql: " + _sql);
                        try
                        {
                            int ret = PCDb.Db.ExecUpdate(_sql);
                        }
                        catch (Exception ex)
                        { PCCore.Common.HRLog.RecordException("Save", ex); }
                    }
                }

            }
            this.periodid = rptID.ToString();
            return true;
        }

        /// <summary>
        ///  Copy PM's Report's Report from frontend to Backend
        /// </summary>
        /// <returns></returns>
        public Boolean Post()
        {
            if (_pmobject.ht_PMRPT.Count != PMReportTable.conTableCount)
                _pmobject = new PMReportTable();

            // Execute StorProc From FrontEnd Database to copy recrod to Backend
            foreach (object o in _pmobject.ht_PMRPT.Keys)
            {
                try
                {
                    DbParameter[] dbpar = new System.Data.Common.DbParameter[4];

                    dbpar[0] = new System.Data.SqlClient.SqlParameter("@tblName", "");
                    dbpar[1] = new System.Data.SqlClient.SqlParameter("@docentry", "");
                    dbpar[2] = new System.Data.SqlClient.SqlParameter("@B1server", "");
                    dbpar[3] = new System.Data.SqlClient.SqlParameter("@B1database", "");

                    PCDb.Db.ExecProcedureNonQuery("sp_PostPMPpt", dbpar);
                }
                catch (Exception ex)
                {
                    PCCore.Common.HRLog.RecordException("Post", ex);
                }
            }
            // Update Status to Close
            string sql = string.Format("UPDATE {0} SET {1} = {2} WHERE {3} = {4}", PMReportTable.PMR_Mstr, PMReportTable.PMR_MSTR_Document_Status, PMReportTable.PMR_CLOSE_STATUS, PMReportTable.PMR_COMM_ID, this.periodid);
            PCDb.Db.ExecUpdate(sql);
            return true;

        }

        Boolean isRecordExists(string datatable, decimal id, int linenum)
        {
            string sql;
            if (datatable != PMReportTable.PMR_Mstr && datatable != PMReportTable.PMR_PRJPARTICULAR)
                sql = string.Format("SELECT 1 FROM {0} WHERE DOCENTRY = {1} AND LINENUM = {2}", datatable, id, linenum);
            else
                sql = string.Format("SELECT 1 FROM {0} WHERE DOCENTRY = {1} ", datatable, id);
            DataTable _dt;
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("isRecordExists", ex);
                return false;
            }

        }

        Boolean ExecuteHeader()
        {
            Boolean found = false;
            DataTable _dt = ExecuteDataTable(PMReportTable.PMR_Mstr, string.Format(" {0} = {1} ", PMReportTable.PMR_COMM_ID, this.periodid));
            try
            {
                if (_dt != null)
                {
                    found = (_dt.Rows.Count > 0);
                }
                else
                {
                    found = false;
                }
            }
            catch (Exception ex)
            {
                found = false;
            }
            return found;

        }
        DataTable ExecuteDataTable(string datatable)
        {
            return ExecuteDataTable(datatable, "1 = 1");
        }
        DataTable ExecuteDataTable(string datatable, string other_filter)
        {
            return ExecuteDataTable(datatable, "*", true, other_filter);
        }
        DataTable ExecuteDataTable(string datatable, object field, Boolean isFrontEnd)
        {
            return ExecuteDataTable(datatable, "*", isFrontEnd, "1=1");
        }
        DataTable ExecuteDataTable(string datatable, object field, Boolean isFrontEnd, string other_filter)
        {
            DataTable _dt;
            string _sql = string.Format("SELECT {0} FROM {1} WHERE {2} {3} AND {4}", field, datatable, main_filter, this.prjcode, other_filter);
            PCCore.Common.HRLog.RecordLog("ExecuteDataTable: " + _sql);
            try
            {
                if (isFrontEnd)
                    _dt = PCDb.Db.ExecQuery(_sql);
                else
                    _dt = SAPDb.Db.ExecQuery(_sql);
                return _dt;

            }
            catch (Exception ex)
            {
                return null;
            }
        }


        public void SetProjectInfo(PCCore.Label txtPrjCode, PCCore.Label txtPrjName, PCCore.TextBox txtPeriodFrom, PCCore.TextBox txtPeriodTo)
        {
            txtPrjCode.Text = this.prjcode;
            txtPrjName.Text = this.prjname;
            txtPeriodFrom.Text = this.last_period_from.ToString();
            txtPeriodTo.Text = this.last_period_to.ToString();

            // ;

        }
        #region Session
        public static int NewLineNumber(Hashtable _dtControl)
        {
            int highestLineNum = 0;
            if (_dtControl != null)
            {
                try
                {
                    foreach (object o in _dtControl.Keys)
                    {
                        int _i = Convert.ToInt32(o);
                        if (_i > highestLineNum)
                            highestLineNum = _i;
                    }
                }
                catch (Exception ex)
                {
                    PCCore.Common.HRLog.RecordException("NewLineNum", ex);
                    throw ex;
                }
            }
            return highestLineNum + 1;
        }
        public Boolean SessionExists()
        {
            return PCCore.Common.XMLPMSession.Exists();
        }


        #endregion
        #region Initizial
        public Hashtable InitData(string dtControlName)
        {
            Hashtable _htcontrol = null;
            try
            {
                //PCCore.PCMS.PMReport _pmreport = new PCCore.PCMS.PMReport(this.projcode, id, period);
                // This Checking is most important for get information
                if (this.SessionExists())
                {

                    //objpmtable = _pmreport._PMReportTable;
                    Hashtable _ht = PCCore.Common.XMLPMSession.Read();
                    _htcontrol = (Hashtable)_ht[dtControlName];
                }
                else
                {
                    // set all value to session file first
                    Hashtable _ht = this._pmobject.toHashTale();
                    PCCore.Common.XMLPMSession.Write(_ht);

                    _htcontrol = (Hashtable)PCCore.Common.XMLPMSession.Read(dtControlName);
                    //objpmtable = _pmreport._pmobject;
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("InitPMReport", ex);
                throw ex;
            }
            return _htcontrol;

        }
        //private Hashtable initData(string htControlName,Hashtable _htcontrol)
        //{
        //    if (_htcontrol.Count == 0)
        //    {
        //        // No Data in Table, add blank record on it.
        //        switch (htControlName)
        //        {
        //            case PMReportTable.PMR_CostClaimsStatus:
        //                break;
        //            case PMReportTable.PMR_DiffReason:
        //                break;
        //            case PMReportTable.PMR_EnvIssues:
        //                break;
        //            case PMReportTable.PMR_EOTClaims:
        //                break;
        //            case PMReportTable.PMR_EXECSUMMARY:
        //                break;
        //            case PMReportTable.PMR_Images:
        //                break;

        //        }
        //    }
        //    else
        //        return _htcontrol;
        //}
        #endregion

        public static Boolean isStatusDraft(string id)
        {
            string sql = string.Format("SELECT Top 1 DocStatus FROM {0} WHERE id = {1}", Consts.Table_DocumentProperty, id);
            DataTable _dt;
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                    return (Convert.ToString(_dt.Rows[0][0]) == Consts.DOCUMENT_DRAFT);
                else
                    return false;
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("isStatusDraft", ex);
            }
            return false;
        }
        public static Boolean hasOutStandPMsReport(string projectcode)
        {
            string sql = string.Format("SELECT 1 FROM {0} WHERE TYPE = {1} AND PROJECTCODE = '{2}' AND DOCSTATUS = '{3}'",Consts.Table_DocumentProperty,4001,projectcode,Consts.DOCUMENT_DRAFT);
            PCCore.Common.HRLog.RecordLog(sql);
            DataTable _dt;
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("hasOutStandPMsReport", ex);
            }
            return false;
        }
        public static string NewAddValidate(string projectcode)
        {
            string errmsg = "";
            string sql = string.Format("SELECT Top 1 T0.DocNum,T0.DocRunningNum, isNull(T1.U_StartDate,'1900-01-01') as StartDate,T1.* FROM {0} T0 INNER JOIN PCMS_BE.{1}.DBO.OPRJ T1 ON (T1.PRJCODE = T0.PROJECTCODE) WHERE T0.PROJECTCODE = '{2}' ORDER BY T0.DocRunningNum DESC", "CPS_View_PMReportInfo", Config.SAPDatabase, projectcode);
            string sqlProjInfo = "SELECT * FROM {0}('{1}','{2}') ";
            PCCore.Common.HRLog.RecordLog(sql);
            DateTime dtStartDate;
            DateTime dtCurrStartDate;
            DateTime dtMaxAllowStartDate;
            DataTable _dt;
            DataTable _dtPrjInfo;
            int RunningNum = 0;
            int CurrentNum = 0;

            // Confirm MaxAllowStartDate
            dtMaxAllowStartDate = DateTime.Now.AddMonths(1);
            dtMaxAllowStartDate = new DateTime(dtMaxAllowStartDate.Year,dtMaxAllowStartDate.Month,1);
            try
            {
                PCCore.Common.HRLog.RecordLog("Backend Database: " + PCDb.Db.ConnectionString);
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                {
                    dtStartDate = Convert.ToDateTime(_dt.Rows[0]["StartDate"]);
                    RunningNum = Convert.ToInt32(_dt.Rows[0]["DocRunningNum"]);
                    CurrentNum = RunningNum + 1;
                    PCCore.Common.HRLog.RecordLog(string.Format(sqlProjInfo, "CPS_Function_ProjectInfo", projectcode, CurrentNum));
                    try
                    {
                        _dtPrjInfo = PCDb.Db.ExecQuery(string.Format(sqlProjInfo, "CPS_Function_ProjectInfo", projectcode, CurrentNum));
                        if (_dtPrjInfo.Rows.Count > 0)
                        {
                            dtCurrStartDate = Convert.ToDateTime(_dtPrjInfo.Rows[0]["FromDate"]);
                            if (dtMaxAllowStartDate < dtCurrStartDate)
                            {
                                errmsg = System.Web.HttpContext.GetGlobalResourceObject(Consts.ResourcesLabels, "PMDateError").ToString();
                            }

                        }
                        else
                        {
                            
                            errmsg = "System Error";
                        }
                    }
                    catch (Exception ex1)
                    {
                        PCCore.Common.HRLog.RecordLog("Find ProjectInfo ", ex1);
                    }

                }
                else
                {
                    return String.Empty;
                }
            }
                catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("NewAddPMsReportValidate", ex);
            }
            return errmsg;
        }
    }
    
}

    