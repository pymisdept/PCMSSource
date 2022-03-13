using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Threading;
using System.Data;
using System.Data.Common;
using SimpleControls.SimpleDatabase;
using SimpleControls.Web;
using SimpleControls;

namespace PCCore
{

    public class SAPDb
    {
        public static Db Db
        {
            get
            {
                
                    if (HttpContext.Current == null || HttpContext.Current.Session == null)
                        throw new ApplicationException("Sesson is lost or in new thread.");

                    return GetDb(HttpContext.Current.Session.SessionID);
                

            }
        }

        public static Db GetDb(string sessionId)
        {
            //object o = SimpleCache.Get(sessionId);
            //if (o != null) return o as Db;
            
            PCCore.Common.HRLog.RecordLog(Config.SAPConnectionString);
            Db db = new Db(Config.DefaultProviderName, Config.SAPConnectionString);
            //SimpleCache.Insert(sessionId, db, null,
            //    SimpleCache.NoAbsoluteExpiration, new TimeSpan(0, 60, 0),
            //     System.Web.Caching.CacheItemPriority.NotRemovable, null);
            
            return db;
        }

        

        //public static Db SessionDB
        //{
        //    get { return _db; }
        //    set { IsSessionDB = true; _db = value; }

        //}


        const int MUTEX_MAX_WAITTIME = 60000;
        private static Mutex _mutexID = new Mutex(false, "ID");
        private static Mutex _mutexEmpID = new Mutex(false, "EMPID");
        private static Mutex _mutexLogID = new Mutex(false, "LOGID");

        public static decimal GetNextID()
        {
            return GetNextID("ID");
        }

        public static decimal GetNextID(DbCommand command)
        {
            return GetNextID("ID", command);
        }

        public static decimal GetNextEmpID()
        {
            return GetNextID("EMPID");
        }

        public static decimal GetNextLogID()
        {
            return GetNextID("LOGID");
        }
        public static decimal GetNextImgID()
        {
            return GetNextID("IMGID");
        }

        //public static decimal GetNextPreBatchNo()
        //{
        //    return GetNextID("PREBATCHNO");
        //}

        //public static decimal GetNextPreBatchNo()
        //{
        //    return GetNextID("ACTBATCHNO");
        //}

        #region GetNextID
        public static decimal GetNextID(string idName)
        {
            return GetNextID(idName, null);
        }

        public static decimal GetNextID(string idName, DbCommand command)
        {
            Mutex mutex;
            DbCommand cm;

            switch (idName)
            {
                case "ID":
                    mutex = _mutexID;
                    break;
                case "EMPID":
                    mutex = _mutexEmpID;
                    break;
                case "LOGID":
                    mutex = _mutexLogID;
                    break;                
                default:
                    mutex = _mutexID;
                    break;
            }

            if (mutex.WaitOne(MUTEX_MAX_WAITTIME, false))
            {
                try
                {
                    //考虑到事务没有提交而导致锁表，特使用新开连接进行。2007-02-01, 需要考虑attdance , paroll两个模块的工作方式
                    if (command == null)
                        cm = Db.GetCommand();
                    else
                        cm = command;

                    cm.CommandType = CommandType.Text;
                    cm.CommandText = String.Format("select max(idval) from {0} where idname='{1}'", Consts.TableID, idName);
                    object o = cm.ExecuteScalar();
                    cm.CommandText = String.Format("update {0} set idval = idval + 1 where idname='{1}'", Consts.TableID, idName);
                    cm.ExecuteNonQuery();
                    return Convert.ToDecimal(o);
                }
                catch (Exception ex)
                {
                    throw ex;
                }
                finally
                {
                    mutex.ReleaseMutex();
                }
            }
            else
            {
                throw new ApplicationException("Unable to get free ID");
            }
        }
        #endregion

        #region GetNextSeqNO

        public static decimal GetNextSeqNO(string TableName, string SeqColumn, string GroupColumn, string GroupValue)
        {
            decimal _n;
            try
            {
                DbCommand cm = Db.GetCommand();
                cm.CommandType = CommandType.Text;
                cm.CommandText = String.Format("select max({0}) from {1} where {2}='{3}'", SeqColumn, TableName, GroupColumn, GroupValue);
                object o = cm.ExecuteScalar();

                _n = Convert.ToDecimal(SimpleControls.SimpleUtils.IfNull(o, 0));
                if (_n >= 1) _n = _n + 1;
                if (_n == 0) _n = 1;

                //cm.CommandText = String.Format("update {0} set idval = idval + 1 where idname='{1}'", Consts.TableID, idName);
                //cm.ExecuteNonQuery(); 
                return _n;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        public static decimal GetNextSeqNO(string TableName, string SeqColumn, string GroupColumn, string GroupValue, DbCommand command)
        {
            decimal _n;
            try
            {
                DbCommand cm = command;
                cm.CommandType = CommandType.Text;
                cm.CommandText = String.Format("select max({0}) from {1} where {2}='{3}'", SeqColumn, TableName, GroupColumn, GroupValue);
                object o = cm.ExecuteScalar();

                _n = Convert.ToDecimal(SimpleControls.SimpleUtils.IfNull(o, 0));
                if (_n >= 1) _n = _n + 1;
                if (_n == 0) _n = 1;

                //cm.CommandText = String.Format("update {0} set idval = idval + 1 where idname='{1}'", Consts.TableID, idName);
                //cm.ExecuteNonQuery(); 
                return _n;
            }
            catch (Exception ex)
            {
                throw ex;
            }

        }
        #endregion

        #region DropDownList
        /// <summary>
        /// 最原始的版本
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="tableName"></param>
        /// <param name="valueField"></param>
        /// <param name="textField"></param>
        /// <param name="options"></param>
        /// <param name="selectedValue"></param>
        public static void InitDropDownList(DropDownList ddl, string tableName, string valueField, string textField, int options, string selectedValue)
        {
            tableName = tableName.ToLower();
            //Todo: 如果是货币的话FontSize恒定为8号体. 2008-01-16
            //if (tableName.ToUpper() == Consts.TableBsCurrency)
            //{
            //    ddl.Font.Size = FontUnit.Parse("8");
            //}

            bool vf = false;
            bool tf = false;
            DataTable dt = new DataTable();
            dt = null;
            string displayvalue = String.Empty;
            object o = null;

            ddl.Items.Clear();
            bool noTextField = (textField == null || textField == valueField);

            if (o != null)
            {
                dt = o as DataTable;
            }
            if (dt != null)
            {
                for (int m = 0; m < dt.Columns.Count; m++)
                {
                    if (dt.Columns[m].ColumnName == textField)
                        tf = true;
                    if (dt.Columns[m].ColumnName == valueField)
                        vf = true;
                }
            }

            if (dt == null || !(tf && vf))
            {



                string sql;
                //sql = String.Format("select displayfield from v_sc_dropdownsetting where tablename='{0}'", tableName);

                //dt = Db.ExecQuery(sql);
                //if (dt != null && dt.Rows.Count == 1)
                //{
                //    displayvalue = dt.Rows[0][0].ToString();
                //}
                //else
                //{
                //    displayvalue = String.Empty;
                //}
                
                if (displayvalue != String.Empty)
                {
                    sql = String.Format("select {0},{1} from {2} order by {3}", valueField, displayvalue, tableName, displayvalue);
                }
                else
                {
                    if (noTextField)
                    {
                        sql = String.Format("select {0},{1} from {2} order by {3}", valueField, "Name", tableName, "Name");
                    }
                    else
                    {
                        sql = String.Format("select {0},{1} from {2} order by {3}", valueField, textField, tableName, textField);
                    }
                }

                dt = Db.ExecQuery(sql);
                SimpleCache.Insert(tableName, dt, null, DateTime.Now.AddHours(1), SimpleCache.NoSlidingExpiration);
            }

            if (dt == null) return;

            ddl.DataSource = dt;
            ddl.DataMember = dt.TableName;
            if (noTextField)
                ddl.DataTextField = valueField;
            else
                ddl.DataTextField = textField;

            if (displayvalue != String.Empty)
                ddl.DataTextField = displayvalue;

            ddl.DataValueField = valueField;

            ddl.DataBind();

            if ((options & Consts.DropDownOptionNone) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownNone, Consts.DropDownNoneValue);
                ddl.Items.Insert(0, li);
            }
            if ((options & Consts.DropDownOptionAll) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownAll, Consts.DropDownAllValue);
                ddl.Items.Insert(0, li);
            }

            if (selectedValue != null)
            {
                System.Web.UI.WebControls.ListItem li = ddl.Items.FindByValue(selectedValue);
                if (li != null)
                {
                    ddl.SelectedIndex = ddl.Items.IndexOf(li);
                }
            }
            else
            {
                if (ddl.Items.Count > 0)
                    ddl.SelectedIndex = 0;
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="tableName"></param>
        /// <param name="valueField"></param>
        /// <param name="textField"></param>
        /// <param name="options"></param>
        /// <param name="selectedValue"></param>
        /// <param name="where"></param>
        /// <param name="getResource"></param>
        public static void InitDropDownList(DropDownList ddl, string tableName, string valueField, string textField, int options, string selectedValue, string where, bool getResource)
        {
            if (String.IsNullOrEmpty(where))
            {
                where = "where 1=1 ";
            }
            else
            {
                where = " where 1=1 and " + where;
            }
            DataTable dt = new DataTable();
            dt = null;
            string displayvalue = String.Empty;

            ddl.Items.Clear();
            bool noTextField = (textField == null || textField == valueField);

            ////if (o != null)
            //{
            //    dt = o as DataTable;
            //}
            if (dt == null)
            {



                string sql;
                //sql = String.Format("select displayfield from v_sc_dropdownsetting where tablename='{0}'", tableName);

                //dt = Db.ExecQuery(sql);
                //if (dt != null && dt.Rows.Count == 1)
                //{
                //    displayvalue = dt.Rows[0][0].ToString();
                //}
                //else
                //{
                //    displayvalue = String.Empty;
                //}
                if (displayvalue != String.Empty)
                {
                    sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, displayvalue, tableName, displayvalue, where);
                }
                else
                {
                    if (noTextField)
                    {
                        sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, "Name", tableName, "Name", where);
                    }
                    else
                    {
                        sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, textField, tableName, textField, where);
                    }
                }


                dt = Db.ExecQuery(sql);
                //SimpleCache.Insert(tableName, dt, null, DateTime.Now.AddHours(1), SimpleCache.NoSlidingExpiration);
            }

            if (dt == null) return;
            if (getResource)
            {
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    dt.Rows[k][1] = PCCore.ComFunction2.GetGlobalResourceObject_ByStringID(PCCore.Consts.ResourcesLabels, dt.Rows[k][1].ToString(), true);
                }
            }
            ddl.DataSource = dt;
            ddl.DataMember = dt.TableName;

            if (noTextField)
                ddl.DataTextField = valueField;
            else
                ddl.DataTextField = textField;
            if (displayvalue != String.Empty)
                ddl.DataTextField = displayvalue;

            ddl.DataValueField = valueField;

            ddl.DataBind();

            if ((options & Consts.DropDownOptionNone) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownNone, Consts.DropDownNoneValue);
                ddl.Items.Insert(0, li);
            }
            if ((options & Consts.DropDownOptionAll) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownAll, Consts.DropDownAllValue);
                ddl.Items.Insert(0, li);
            }

            if (selectedValue != null)
            {
                System.Web.UI.WebControls.ListItem li = ddl.Items.FindByValue(selectedValue);
                if (li != null)
                {
                    ddl.SelectedIndex = ddl.Items.IndexOf(li);
                }
            }
            else
            {
                if (ddl.Items.Count > 0)
                    ddl.SelectedIndex = 0;
            }

        }
        public static void InitDropDownList(DropDownList ddl, string tableName, string valueField, string textField, int options, string selectedValue, bool getResource)
        {
            tableName = tableName.ToLower();
            bool vf = false;
            bool tf = false;
            DataTable dt = new DataTable();
            dt = null;
            string displayvalue = String.Empty;
            object o = null;

            ddl.Items.Clear();
            bool noTextField = (textField == null || textField == valueField);

            if (o != null)
            {
                dt = o as DataTable;
            }
            if (dt != null)
            {
                for (int m = 0; m < dt.Columns.Count; m++)
                {
                    if (dt.Columns[m].ColumnName == textField)
                        tf = true;
                    if (dt.Columns[m].ColumnName == valueField)
                        vf = true;
                }
            }

            if (dt == null || !(tf && vf))
            {
                string sql;
                //sql = String.Format("select displayfield from v_sc_dropdownsetting where tablename='{0}'", tableName);

                //dt = Db.ExecQuery(sql);
                //if (dt != null && dt.Rows.Count == 1)
                //{
                //    displayvalue = dt.Rows[0][0].ToString();
                //}
                //else
                //{
                //    displayvalue = String.Empty;
                //}
                if (displayvalue != String.Empty)
                {
                    sql = String.Format("select {0},{1} from {2} order by id", valueField, displayvalue, tableName, displayvalue);
                }
                else
                {
                    if (noTextField)
                    {
                        sql = String.Format("select {0},{1} from {2} order by id", valueField, "Name", tableName, "Name");
                    }
                    else
                    {
                        sql = String.Format("select {0},{1} from {2} order by id", valueField, textField, tableName, textField);
                    }
                }

                dt = Db.ExecQuery(sql);
                SimpleCache.Insert(tableName, dt, null, DateTime.Now.AddHours(1), SimpleCache.NoSlidingExpiration);
            }

            if (dt == null) return;

            if (getResource)
            {
                for (int k = 0; k < dt.Rows.Count; k++)
                {
                    dt.Rows[k][1] = PCCore.ComFunction2.GetGlobalResourceObject_ByStringID(PCCore.Consts.ResourcesLabels, dt.Rows[k][1].ToString(),true);
                }
            }
            ddl.DataSource = dt;
            ddl.DataMember = dt.TableName;
            if (noTextField)
                ddl.DataTextField = valueField;
            else
                ddl.DataTextField = textField;

            if (displayvalue != String.Empty)
                ddl.DataTextField = displayvalue;

            ddl.DataValueField = valueField;

            ddl.DataBind();

            if ((options & Consts.DropDownOptionNone) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownNone, Consts.DropDownNoneValue);
                ddl.Items.Insert(0, li);
            }
            if ((options & Consts.DropDownOptionAll) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownAll, Consts.DropDownAllValue);
                ddl.Items.Insert(0, li);
            }

            if (selectedValue != null)
            {
                System.Web.UI.WebControls.ListItem li = ddl.Items.FindByValue(selectedValue);
                if (li != null)
                {
                    ddl.SelectedIndex = ddl.Items.IndexOf(li);
                }
            }
            else
            {
                if (ddl.Items.Count > 0)
                    ddl.SelectedIndex = 0;
            }

        }

        public static void InitDropDownList(DropDownList ddl, string tableName, string valueField, string textField, int options, string selectedValue, string where, string orderby)
        {
            if (String.IsNullOrEmpty(where))
            {
                where = "where 1=1 ";
            }
            else
            {
                where = " where 1=1 and " + where;
            }
            DataTable dt = new DataTable();
            dt = null;
            string displayvalue = String.Empty;

            ddl.Items.Clear();
            bool noTextField = (textField == null || textField == valueField);

            ////if (o != null)
            //{
            //    dt = o as DataTable;
            //}
            if (dt == null)
            {



                string sql;
                //sql = String.Format("select displayfield from v_sc_dropdownsetting where tablename='{0}'", tableName);

                //dt = Db.ExecQuery(sql);
                //if (dt != null && dt.Rows.Count == 1)
                //{
                //    displayvalue = dt.Rows[0][0].ToString();
                //}
                //else
                //{
                //    displayvalue = String.Empty;
                //}
                if (!String.IsNullOrEmpty(orderby))
                {
                    if (displayvalue != String.Empty)
                    {
                        sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, displayvalue, tableName, orderby, where);
                    }
                    else
                    {
                        if (noTextField)
                        {
                            sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, "Name", tableName, orderby, where);
                        }
                        else
                        {
                            sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, textField, tableName, orderby, where);
                        }
                    }
                }
                else
                {
                    if (displayvalue != String.Empty)
                    {
                        sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, displayvalue, tableName, orderby, where);
                    }
                    else
                    {
                        if (noTextField)
                        {
                            sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, "Name", tableName, displayvalue, where);
                        }
                        else
                        {
                            sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, textField, tableName, textField, where);
                        }
                    }
                }


                dt = Db.ExecQuery(sql);
                //SimpleCache.Insert(tableName, dt, null, DateTime.Now.AddHours(1), SimpleCache.NoSlidingExpiration);
            }

            if (dt == null) return;

            ddl.DataSource = dt;
            ddl.DataMember = dt.TableName;
            if (noTextField)
                ddl.DataTextField = valueField;
            else
                ddl.DataTextField = textField;

            if (displayvalue != String.Empty)
                ddl.DataTextField = displayvalue;

            ddl.DataValueField = valueField;

            ddl.DataBind();

            if ((options & Consts.DropDownOptionNone) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownNone, Consts.DropDownNoneValue);
                ddl.Items.Insert(0, li);
            }
            if ((options & Consts.DropDownOptionAll) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownAll, Consts.DropDownAllValue);
                ddl.Items.Insert(0, li);
            }

            if (selectedValue != null)
            {
                System.Web.UI.WebControls.ListItem li = ddl.Items.FindByValue(selectedValue);
                if (li != null)
                {
                    ddl.SelectedIndex = ddl.Items.IndexOf(li);
                }
            }
            else
            {
                if (ddl.Items.Count > 0)
                    ddl.SelectedIndex = 0;
            }

        }

        /// <summary>
        /// 根据pTextOrValue ，确定以什么的方式来查找确定定位的地方。
        /// 1.Value
        /// 2.Text
        /// </summary>
        /// <param name="ddl"></param>
        /// <param name="tableName"></param>
        /// <param name="valueField"></param>
        /// <param name="textField"></param>
        /// <param name="options"></param>
        /// <param name="pSelected"></param>
        /// <param name="pTextOrValue"></param>
        public static void InitDropDownList(DropDownList ddl, string tableName, string valueField, string textField, int options, string pSelected, int pTextOrValue)
        {
            bool vf = false;
            bool tf = false;
            DataTable dt = new DataTable();
            dt = null;
            string displayvalue = String.Empty;
            object o = SimpleCache.Get(tableName);

            ddl.Items.Clear();
            bool noTextField = (textField == null || textField == valueField);

            if (o != null)
            {
                dt = o as DataTable;
            }
            if (dt != null)
            {
                for (int m = 0; m < dt.Columns.Count; m++)
                {
                    if (dt.Columns[m].ColumnName == textField)
                        tf = true;
                    if (dt.Columns[m].ColumnName == valueField)
                        vf = true;
                }
            }

            if (dt == null || !(tf && vf))
            {



                string sql;
                //sql = String.Format("select displayfield from v_sc_dropdownsetting where tablename='{0}'", tableName);

                //dt = Db.ExecQuery(sql);
                //if (dt != null && dt.Rows.Count == 1)
                //{
                //    displayvalue = dt.Rows[0][0].ToString();
                //}
                //else
                //{
                //    displayvalue = String.Empty;
                //}
                if (displayvalue != String.Empty)
                {
                    sql = String.Format("select {0},{1} from {2} order by {3}", valueField, displayvalue, tableName, displayvalue);
                }
                else
                {
                    if (noTextField)
                    {
                        sql = String.Format("select {0},{1} from {2} order by {3}", valueField, "Name", tableName, "Name");
                    }
                    else
                    {
                        sql = String.Format("select {0},{1} from {2} order by {3}", valueField, textField, tableName, textField);
                    }
                }

                dt = Db.ExecQuery(sql);
                SimpleCache.Insert(tableName, dt, null, DateTime.Now.AddHours(1), SimpleCache.NoSlidingExpiration);
            }

            if (dt == null) return;

            ddl.DataSource = dt;
            ddl.DataMember = dt.TableName;
            if (noTextField)
                ddl.DataTextField = valueField;
            else
                ddl.DataTextField = textField;

            if (displayvalue != String.Empty)
                ddl.DataTextField = displayvalue;

            ddl.DataValueField = valueField;

            ddl.DataBind();

            if ((options & Consts.DropDownOptionNone) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownNone, Consts.DropDownNoneValue);
                ddl.Items.Insert(0, li);
            }
            if ((options & Consts.DropDownOptionAll) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownAll, Consts.DropDownAllValue);
                ddl.Items.Insert(0, li);
            }



            if (ddl.Items.Count > 0)
                ddl.SelectedIndex = 0;

            switch (pTextOrValue)
            {
                case 2:
                    if (pSelected != null)
                    {
                        System.Web.UI.WebControls.ListItem li = ddl.Items.FindByText(pSelected);
                        if (li != null)
                        {
                            ddl.SelectedIndex = ddl.Items.IndexOf(li);
                        }
                    }
                    break;
                case 1:
                    if (pSelected != null)
                    {
                        System.Web.UI.WebControls.ListItem li = ddl.Items.FindByValue(pSelected);
                        if (li != null)
                        {
                            ddl.SelectedIndex = ddl.Items.IndexOf(li);
                        }
                    }
                    break;
            }
        }

        /// <summary>
        /// 根据OrderByColumn,进行排序
        /// </summary>
        public static void InitDropDownList_ByID(DropDownList ddl, string tableName, string valueField, string textField, int options, string selectedValue, string OrderByColumn)
        {
            bool vf = false;
            bool tf = false;
            DataTable dt = new DataTable();
            dt = null;
            string displayvalue = String.Empty;
            object o = SimpleCache.Get(tableName);

            ddl.Items.Clear();
            bool noTextField = (textField == null || textField == valueField);

            if (o != null)
            {
                dt = o as DataTable;
            }
            if (dt != null)
            {
                for (int m = 0; m < dt.Columns.Count; m++)
                {
                    if (dt.Columns[m].ColumnName == textField)
                        tf = true;
                    if (dt.Columns[m].ColumnName == valueField)
                        vf = true;
                }
            }

            if (dt == null || !(tf && vf))
            {



                string sql;
                //sql = String.Format("select displayfield from v_sc_dropdownsetting where tablename='{0}'", tableName);

                //dt = Db.ExecQuery(sql);
                //if (dt != null && dt.Rows.Count == 1)
                //{
                //    displayvalue = dt.Rows[0][0].ToString();
                //}
                //else
                //{
                //    displayvalue = String.Empty;
                //}
                if (displayvalue != String.Empty)
                {
                    sql = String.Format("select {0},{1} from {2} order by {3}", valueField, displayvalue, tableName, OrderByColumn);
                }
                else
                {
                    if (noTextField)
                    {
                        //sql = String.Format("select {0},{1} from {2} order by {3}", valueField, "Name", tableName, OrderByColumn);
                        sql = String.Format("select {0},{1} from {2} order by {3}", valueField, valueField + " as Name ", tableName, OrderByColumn);
                    }
                    else
                    {
                        sql = String.Format("select {0},{1} from {2} order by {3}", valueField, textField, tableName, OrderByColumn);
                    }
                }

                dt = Db.ExecQuery(sql);
                SimpleCache.Insert(tableName, dt, null, DateTime.Now.AddHours(1), SimpleCache.NoSlidingExpiration);
            }

            if (dt == null) return;

            //1.用数据源邦定
            ddl.DataSource = dt;
            ddl.DataMember = dt.TableName;

            if (noTextField)
                ddl.DataTextField = valueField;
            else
                ddl.DataTextField = textField;
            if (displayvalue != String.Empty)
                ddl.DataTextField = displayvalue;

            ddl.DataValueField = valueField;

            ddl.DataBind();

            if ((options & Consts.DropDownOptionNone) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownNone, Consts.DropDownNoneValue);
                ddl.Items.Insert(0, li);
            }
            if ((options & Consts.DropDownOptionAll) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownAll, Consts.DropDownAllValue);
                ddl.Items.Insert(0, li);
            }

            //2.支持用每行的插入
            //for (int i = 1; i <= dt.Rows.Count; i++)
            //{
            //    System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(dt.Rows[i - 1][1].ToString(), dt.Rows[i - 1][0].ToString());
            //    ddl.Items.Insert(i, li);
            //}


            if (selectedValue != null)
            {
                System.Web.UI.WebControls.ListItem li = ddl.Items.FindByValue(selectedValue);
                if (li != null)
                {
                    ddl.SelectedIndex = ddl.Items.IndexOf(li);
                }
            }
            else
            {
                if (ddl.Items.Count > 0)
                    ddl.SelectedIndex = 0;
            }

        }

        public static void InitDropDownList(DropDownList ddl, string tableName, string valueField, string textField, int options, string selectedValue, string where)
        {
            if (String.IsNullOrEmpty(where))
            {
                where = "where 1=1 ";
            }
            else
            {
                where = " where 1=1 and " + where;
            }
            DataTable dt = new DataTable();
            dt = null;
            string displayvalue = String.Empty;

            ddl.Items.Clear();
            bool noTextField = (textField == null || textField == valueField);
            

            ////if (o != null)
            //{
            //    dt = o as DataTable;
            //}
            if (dt == null)
            {



                string sql;
                //sql = String.Format("select displayfield from v_sc_dropdownsetting where tablename='{0}'", tableName);

                //dt = Db.ExecQuery(sql);
                //if (dt != null && dt.Rows.Count == 1)
                //{
                //    displayvalue = dt.Rows[0][0].ToString();
                //}
                //else
                //{
                //    displayvalue = String.Empty;
                //}
                if (displayvalue != String.Empty)
                {
                    sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, displayvalue, tableName, displayvalue, where);
                }
                else
                {
                    if (noTextField)
                    {
                        sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, valueField + " as Name ", tableName, "Name", where);
                    }
                    else
                    {
                        sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, textField, tableName, textField, where);
                    }
                }
                PCCore.Common.HRLog.RecordLog(sql);

                dt = Db.ExecQuery(sql);
                //SimpleCache.Insert(tableName, dt, null, DateTime.Now.AddHours(1), SimpleCache.NoSlidingExpiration);
            }

            if (dt == null) return;

            ddl.DataSource = dt;
            ddl.DataMember = dt.TableName;

            if (noTextField)
                ddl.DataTextField = valueField;
            else
                ddl.DataTextField = textField;
            if (displayvalue != String.Empty)
                ddl.DataTextField = displayvalue;

            ddl.DataValueField = valueField;

            ddl.DataBind();

            if ((options & Consts.DropDownOptionNone) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownNone, Consts.DropDownNoneValue);
                ddl.Items.Insert(0, li);
            }
            if ((options & Consts.DropDownOptionAll) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownAll, Consts.DropDownAllValue);
                ddl.Items.Insert(0, li);
            }

            if (selectedValue != null)
            {
                System.Web.UI.WebControls.ListItem li = ddl.Items.FindByValue(selectedValue);
                if (li != null)
                {
                    ddl.SelectedIndex = ddl.Items.IndexOf(li);
                }
            }
            else
            {
                if (ddl.Items.Count > 0)
                    ddl.SelectedIndex = 0;
            }

        }
        public static void InitAddDocProjectList(DropDownList ddl, string tableName, string valueField, string textField, int options, string selectedValue, string where)
        {
            if (String.IsNullOrEmpty(where))
            {
                where = "where 1=1 ";
            }
            else
            {
                where = " where 1=1 and " + where;
            }
            DataTable dt = new DataTable();
            dt = null;
            string displayvalue = String.Empty;

            ddl.Items.Clear();
            bool noTextField = (textField == null || textField == valueField);


            ////if (o != null)
            //{
            //    dt = o as DataTable;
            //}
            if (dt == null)
            {



                string sql;
                //sql = String.Format("select displayfield from v_sc_dropdownsetting where tablename='{0}'", tableName);

                //dt = Db.ExecQuery(sql);
                //if (dt != null && dt.Rows.Count == 1)
                //{
                //    displayvalue = dt.Rows[0][0].ToString();
                //}
                //else
                //{
                //    displayvalue = String.Empty;
                //}
                //if (!SessionInfo.IsSupervisor)
                    //where += string.Format(" and EXISTS(SELECT 1 FROM PCMS_FE.{0}.DBO.SC_RIGHT WHERE FUNCID = {1} AND ID IN ({2}) AND PROJCODE = {3}.PRJCODE AND (ISNULL(FALL,0) = 1 OR ISNULL(FNEW,0) = 1) )",SessionInfo.Database,SessionInfo.CurrentFunctionID,PCCore.Database.SCGroupUser.getALLIDbyUser(SessionInfo.UserId),tableName);
                if (displayvalue != String.Empty)
                {
                    if (SessionInfo.IsSupervisor)
                        //sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, displayvalue, tableName, displayvalue, where);
                        sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, displayvalue, tableName, displayvalue, String.Empty);
                    else
                        sql = String.Format("select {0},{1} from {2} inner join PCMS_FE.{8}.dbo.{5} sr on (sr.projcode = {0} and sr.userid = {6} and sr.fid = {7}) {4} order by {3}", valueField, displayvalue, tableName, displayvalue, where, "CPS_View_AddPermission", SessionInfo.UserId, SessionInfo.CurrentFunctionID, SessionInfo.Database);
                }
                else
                {
                    if (noTextField)
                    {
                        if (SessionInfo.IsSupervisor)
                            //sql = String.Format("select {0},{1} from {2}  {4} order by {3}", valueField, valueField + " as Name ", tableName, "Name", where);
                            sql = String.Format("select {0},{1} from {2}  {4} order by {3}", valueField, valueField + " as Name ", tableName, "Name", String.Empty);
                        else
                            sql = String.Format("select {0},{1} from {2} inner join PCMS_FE.{8}.dbo.{5} sr on (sr.projcode = {0} and sr.userid = {6} and sr.fid = {7}) {4} order by {3}", valueField, valueField + " as Name ", tableName, "Name", where, "CPS_View_AddPermission", SessionInfo.UserId, SessionInfo.CurrentFunctionID, SessionInfo.Database);
                    }
                    else
                    {
                        if (SessionInfo.IsSupervisor)
                            //sql = String.Format("select {0},{1} from {2}  {4} order by {3}", valueField, textField, tableName, textField, where, "CPS_View_AddPermission", SessionInfo.UserId, SessionInfo.CurrentFunctionID, SessionInfo.Database);
                            sql = String.Format("select {0},{1} from {2}  {4} order by {3}", valueField, textField, tableName, textField, String.Empty, "CPS_View_AddPermission", SessionInfo.UserId, SessionInfo.CurrentFunctionID, SessionInfo.Database);
                        else
                            sql = String.Format("select {0},{1} from {2} inner join PCMS_FE.{8}.dbo.{5} sr on (sr.projcode = {0} and sr.userid = {6} and sr.fid = {7})  {4} order by {3}", valueField, textField, tableName, textField, where, "CPS_View_AddPermission", SessionInfo.UserId, SessionInfo.CurrentFunctionID, SessionInfo.Database);
                    }
                }
                PCCore.Common.HRLog.RecordLog(sql);

                dt = Db.ExecQuery(sql);
                //SimpleCache.Insert(tableName, dt, null, DateTime.Now.AddHours(1), SimpleCache.NoSlidingExpiration);
            }

            if (dt == null) return;

            ddl.DataSource = dt;
            ddl.DataMember = dt.TableName;

            if (noTextField)
                ddl.DataTextField = valueField;
            else
                ddl.DataTextField = textField;
            if (displayvalue != String.Empty)
                ddl.DataTextField = displayvalue;

            ddl.DataValueField = valueField;

            ddl.DataBind();

            if ((options & Consts.DropDownOptionNone) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownNone, Consts.DropDownNoneValue);
                ddl.Items.Insert(0, li);
            }
            if ((options & Consts.DropDownOptionAll) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownAll, Consts.DropDownAllValue);
                ddl.Items.Insert(0, li);
            }

            if (selectedValue != null)
            {
                System.Web.UI.WebControls.ListItem li = ddl.Items.FindByValue(selectedValue);
                if (li != null)
                {
                    ddl.SelectedIndex = ddl.Items.IndexOf(li);
                }
            }
            else
            {
                if (ddl.Items.Count > 0)
                    ddl.SelectedIndex = 0;
            }

        }

        public static void InitQueryDocProjectList(DropDownList ddl, string tableName, string valueField, string textField, int options, string selectedValue, string where)
        {
            if (String.IsNullOrEmpty(where))
            {
                where = "where 1=1 ";
            }
            else
            {
                where = " where 1=1 and " + where;
            }
            DataTable dt = new DataTable();
            dt = null;
            string displayvalue = String.Empty;

            ddl.Items.Clear();
            bool noTextField = (textField == null || textField == valueField);


            ////if (o != null)
            //{
            //    dt = o as DataTable;
            //}
            if (dt == null)
            {



                string sql;
                //sql = String.Format("select displayfield from v_sc_dropdownsetting where tablename='{0}'", tableName);

                //dt = Db.ExecQuery(sql);
                //if (dt != null && dt.Rows.Count == 1)
                //{
                //    displayvalue = dt.Rows[0][0].ToString();
                //}
                //else
                //{
                //    displayvalue = String.Empty;
                //}
                //if (!SessionInfo.IsSupervisor)
                //where += string.Format(" and EXISTS(SELECT 1 FROM PCMS_FE.{0}.DBO.SC_RIGHT WHERE FUNCID = {1} AND ID IN ({2}) AND PROJCODE = {3}.PRJCODE AND (ISNULL(FALL,0) = 1 OR ISNULL(FNEW,0) = 1) )",SessionInfo.Database,SessionInfo.CurrentFunctionID,PCCore.Database.SCGroupUser.getALLIDbyUser(SessionInfo.UserId),tableName);
                if (displayvalue != String.Empty)
                {
                    if (SessionInfo.IsSupervisor)
                        //sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, displayvalue, tableName, displayvalue, where);
                        sql = String.Format("select {0},{1} from {2} {4} order by {3}", valueField, displayvalue, tableName, displayvalue, String.Empty);
                        
                    else
                        sql = String.Format("select {0},{1} from {2} inner join PCMS_FE.{8}.dbo.{5} sr on (sr.projcode = {0} and sr.userid = {6} and sr.fid = {7}) {4} order by {3}", valueField, displayvalue, tableName, displayvalue, where, "CPS_View_QueryPermission", SessionInfo.UserId, SessionInfo.CurrentFunctionID, SessionInfo.Database);
                }
                else
                {
                    if (noTextField)
                    {
                        if (SessionInfo.IsSupervisor)
                            //sql = String.Format("select {0},{1} from {2}  {4} order by {3}", valueField, valueField + " as Name ", tableName, "Name", where);
                            sql = String.Format("select {0},{1} from {2}  {4} order by {3}", valueField, valueField + " as Name ", tableName, "Name",String.Empty);
                        else
                            sql = String.Format("select {0},{1} from {2} inner join PCMS_FE.{8}.dbo.{5} sr on (sr.projcode = {0} and sr.userid = {6} and sr.fid = {7}) {4} order by {3}", valueField, valueField + " as Name ", tableName, "Name", where, "CPS_View_QueryPermission", SessionInfo.UserId, SessionInfo.CurrentFunctionID, SessionInfo.Database);
                    }
                    else
                    {
                        if (SessionInfo.IsSupervisor)
                            //sql = String.Format("select {0},{1} from {2}  {4} order by {3}", valueField, textField, tableName, textField, where, "CPS_View_AddPermission", SessionInfo.UserId, SessionInfo.CurrentFunctionID, SessionInfo.Database);
                            sql = String.Format("select {0},{1} from {2}  {4} order by {3}", valueField, textField, tableName, textField, String.Empty, "CPS_View_AddPermission", SessionInfo.UserId, SessionInfo.CurrentFunctionID, SessionInfo.Database);
                        else
                            sql = String.Format("select {0},{1} from {2} inner join PCMS_FE.{8}.dbo.{5} sr on (sr.projcode = {0} and sr.userid = {6} and sr.fid = {7})  {4} order by {3}", valueField, textField, tableName, textField, where, "CPS_View_QueryPermission", SessionInfo.UserId, SessionInfo.CurrentFunctionID, SessionInfo.Database);
                    }
                }
                PCCore.Common.HRLog.RecordLog(sql);

                dt = Db.ExecQuery(sql);
                //SimpleCache.Insert(tableName, dt, null, DateTime.Now.AddHours(1), SimpleCache.NoSlidingExpiration);
            }

            if (dt == null) return;

            ddl.DataSource = dt;
            ddl.DataMember = dt.TableName;

            if (noTextField)
                ddl.DataTextField = valueField;
            else
                ddl.DataTextField = textField;
            if (displayvalue != String.Empty)
                ddl.DataTextField = displayvalue;

            ddl.DataValueField = valueField;

            ddl.DataBind();

            if ((options & Consts.DropDownOptionNone) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownNone, Consts.DropDownNoneValue);
                ddl.Items.Insert(0, li);
            }
            if ((options & Consts.DropDownOptionAll) > 0)
            {
                System.Web.UI.WebControls.ListItem li = new System.Web.UI.WebControls.ListItem(Consts.DropDownAll, Consts.DropDownAllValue);
                ddl.Items.Insert(0, li);
            }

            if (selectedValue != null)
            {
                System.Web.UI.WebControls.ListItem li = ddl.Items.FindByValue(selectedValue);
                if (li != null)
                {
                    ddl.SelectedIndex = ddl.Items.IndexOf(li);
                }
            }
            else
            {
                if (ddl.Items.Count > 0)
                    ddl.SelectedIndex = 0;
            }

        }

        public static void NewInitDropDownList(SimpleSelect ddl, string tableName, string valueField, string textField, int options, string selectedValue)
        {

            bool noTextField = (textField == null || textField == valueField);

            string sql;
            if (noTextField)
            {
                sql = String.Format("select {0} from {1} order by {2}", valueField, tableName, valueField);
            }
            else
            {
                sql = String.Format("select {0},{1} from {2} order by {3}", valueField, textField, tableName, textField);
            }

            DataTable dt = Db.ExecQuery(sql);
            if (dt == null) return;
            DataRow dr;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                dr = dt.Rows[i];
                ddl.AddItem(dr[textField.ToString()].ToString(), dr[valueField.ToString()].ToString());
            }
        }
        #endregion DropDownList     


        #region InitDropDownListOperator
        public static void InitDropDownListOperator(DropDownList ddl)
        {
            ddl.Items.Clear();
            ListItem li = new ListItem();

            li = new ListItem("+", "+");
            ddl.Items.Add(li);
            li = new ListItem("-", "-");
            ddl.Items.Add(li);
            li = new ListItem("*", "*");
            ddl.Items.Add(li);
            li = new ListItem("/", "/");
            ddl.Items.Add(li);
            li = new ListItem("(", "(");
            ddl.Items.Add(li);
            li = new ListItem(")", ")");

            ddl.Items.Add(li);



        }
        #endregion InitDropDownListOperator   

    }//end of class


}
