using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using System.Data;
using SimpleControls;
using SimpleControls.SimpleDatabase;
using System.Web;

namespace PCCore
{
    public class ScLog
    {
        public enum Actions
        {
            Login,
            Logout,
            Timeout,
            Enter,
            Select,
            Insert,
            Update,
            Delete,
            Operation
        }

        #region modi by emily(2008-08-01)
        protected static string BuildInsertSql()
        {
            Db db = PCDb.Db;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("insert into {0} (id,loginname,action,logtime,ipaddress,hostname,osuser,tablename,funcid,funcname,projectcode) values (", "SC_LOG");
            sb.AppendFormat("{0},", db.GetParameterMarker("id"));
            sb.AppendFormat("{0},", db.GetParameterMarker("loginname"));
            sb.AppendFormat("{0},", db.GetParameterMarker("action"));
            sb.AppendFormat("{0},", db.GetParameterMarker("logtime"));
            sb.AppendFormat("{0},", db.GetParameterMarker("ipaddress"));
            sb.AppendFormat("{0},", db.GetParameterMarker("hostname"));
            sb.AppendFormat("{0},", db.GetParameterMarker("osuser"));
            sb.AppendFormat("{0},", db.GetParameterMarker("tablename"));
            sb.AppendFormat("{0},", db.GetParameterMarker("funcid"));
            sb.AppendFormat("{0},", db.GetParameterMarker("funcname"));
            sb.AppendFormat("{0})", db.GetParameterMarker("projectcode"));
            //sb.AppendFormat("{0})", db.GetParameterMarker("content"));
            return sb.ToString();
        }
        #endregion

        #region create by emily(2008-07-29)
        protected static string BuildInsertSqlForLogDetail()
        {
            Db db = PCDb.Db;
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("insert into {0} (id,logid,fieldname,oldvalue,newvalue,changed) values (", "SC_lOGDETAIL");
            sb.AppendFormat("{0},", db.GetParameterMarker("id"));
            sb.AppendFormat("{0},", db.GetParameterMarker("logid"));
            sb.AppendFormat("{0},", db.GetParameterMarker("fieldname"));
            sb.AppendFormat("{0},", db.GetParameterMarker("oldvalue"));
            sb.AppendFormat("{0},", db.GetParameterMarker("newvalue"));
            sb.AppendFormat("{0})", db.GetParameterMarker("changed"));
            return sb.ToString();
        }
        #endregion

        public static void Insert(Actions action)
        {
            Insert(action, -2, String.Empty, null, null, null);
        }
        public static void Insert(Actions action, int funcid, string funcName)
        {
            Insert(action, funcid, funcName, null, null, null);
        }
        // old Insert
        //public static void Insert(Actions action,int funcid, string funcName, PCTable table, Hashtable oldValues, Hashtable newValues)
        //{
        //    Db db = PCDb.Db;
        //    DbCommand cm;

        //    if (table != null && table.IsTransactionReady)
        //    {
        //        cm = table.InternalCommand;
        //    }
        //    else
        //    {
        //        cm = db.GetCommand();
        //    }

        //    cm.CommandText = BuildInsertSql();
        //    cm.CommandType = CommandType.Text;

        //    cm.Parameters.Add(db.GetParameter("id", PCDb.GetNextLogID()));
        //    cm.Parameters.Add(db.GetParameter("loginname", SessionInfo.LoginName));
        //    cm.Parameters.Add(db.GetParameter("action", action.ToString()));
        //    cm.Parameters.Add(db.GetParameter("logtime", DateTime.Now));
        //    cm.Parameters.Add(db.GetParameter("ipaddress", SessionInfo.RemoteAddr));
        //    cm.Parameters.Add(db.GetParameter("hostname", SessionInfo.RemoteHost));
        //    cm.Parameters.Add(db.GetParameter("osuser", SessionInfo.RemoteUser));
        //    cm.Parameters.Add(db.GetParameter("tablename", table == null ? String.Empty : table.TableName));
        //    cm.Parameters.Add(db.GetParameter("funcid", SimpleUtils.IfNull(funcid, -2)));
        //    cm.Parameters.Add(db.GetParameter("funcname", SimpleUtils.IfNull(funcName, String.Empty)));

        //    cm.Parameters.Add(db.GetParameter("content", BuildContent(action, table, oldValues, newValues)));

        //    cm.ExecuteNonQuery();

        //    // if data changed, remove cache. 2006-07-17
        //    if (action == ScLog.Actions.Insert || action == ScLog.Actions.Update || action == ScLog.Actions.Delete)
        //    {
        //        System.Web.Caching.Cache ca = HttpRuntime.Cache;
        //        ca.Remove(table.TableName);
        //    }

        //}

        #region create by emily(2008-07-31)
        public static void Insert(Actions action, int funcid, string funcName, PCTable table, Hashtable oldValues, Hashtable newValues)
        {
            Db db = PCDb.Db;
            DbCommand cm;

            if (table != null && table.IsTransactionReady)
            {
                cm = table.InternalCommand;
            }
            else
            {
                cm = db.GetCommand();
            }

            cm.CommandText = BuildInsertSql();
            cm.CommandType = CommandType.Text;
            decimal logid = PCDb.GetNextLogID();
            cm.Parameters.Add(db.GetParameter("id", logid));
            cm.Parameters.Add(db.GetParameter("loginname", SessionInfo.LoginName));
            cm.Parameters.Add(db.GetParameter("action", action.ToString()));
            cm.Parameters.Add(db.GetParameter("logtime", DateTime.Now));
            cm.Parameters.Add(db.GetParameter("ipaddress", SessionInfo.RemoteAddr));
            cm.Parameters.Add(db.GetParameter("tablename", table == null ? String.Empty : table.TableName));
            cm.Parameters.Add(db.GetParameter("funcid", SimpleUtils.IfNull(funcid, -2)));
            cm.Parameters.Add(db.GetParameter("funcname", SimpleUtils.IfNull(funcName, String.Empty)));
            cm.Parameters.Add(db.GetParameter("hostname", SessionInfo.RemoteHost));
            cm.Parameters.Add(db.GetParameter("osuser", SessionInfo.RemoteUser));
            cm.Parameters.Add(db.GetParameter("projectcode", newValues == null ? String.Empty : newValues["PROJECTCODE"]));
            cm.ExecuteNonQuery();
            if (action == ScLog.Actions.Insert || action == ScLog.Actions.Update || action == ScLog.Actions.Delete)
            {
                ConvertRowToString(logid, action, table, oldValues,newValues) ;
            }
          
        }
        #endregion

        public static void InsertTimeout(string loginName, string remoteAddr, string remoteHost, string remoteUser)
        {
            Db db = PCDb.Db;
            DbCommand cm = db.GetCommand();
            cm.CommandText = BuildInsertSql();
            cm.CommandType = CommandType.Text;

            cm.Parameters.Add(db.GetParameter("id", PCDb.GetNextLogID()));
            cm.Parameters.Add(db.GetParameter("loginname", loginName));
            cm.Parameters.Add(db.GetParameter("action", ScLog.Actions.Timeout.ToString()));
            cm.Parameters.Add(db.GetParameter("logtime", DateTime.Now));
            cm.Parameters.Add(db.GetParameter("ipaddress", remoteAddr));
            cm.Parameters.Add(db.GetParameter("hostname", remoteHost));
            cm.Parameters.Add(db.GetParameter("osuser", remoteUser));
            cm.Parameters.Add(db.GetParameter("tablename", String.Empty));
            cm.Parameters.Add(db.GetParameter("funcid", -2));
            cm.Parameters.Add(db.GetParameter("funcname", String.Empty));
            //cm.Parameters.Add(db.GetParameter("content", String.Empty));
            cm.ExecuteNonQuery();
        }

        public static void InsertOperation(Actions pAction, int pFuncid, string pFuncName, string pOperation)
        {
            Db db = PCDb.Db;
            DbCommand cm = db.GetCommand();
            cm.CommandText = BuildInsertSql();
            cm.CommandType = CommandType.Text;
            cm.Parameters.Add(db.GetParameter("id", PCDb.GetNextLogID()));
            cm.Parameters.Add(db.GetParameter("loginname", SessionInfo.LoginName));
            cm.Parameters.Add(db.GetParameter("action", pAction.ToString()));
            cm.Parameters.Add(db.GetParameter("logtime", DateTime.Now));
            cm.Parameters.Add(db.GetParameter("ipaddress", SessionInfo.RemoteAddr));
            cm.Parameters.Add(db.GetParameter("hostname", SessionInfo.RemoteHost));
            cm.Parameters.Add(db.GetParameter("osuser", SessionInfo.RemoteUser));
            cm.Parameters.Add(db.GetParameter("tablename", String.Empty));
            cm.Parameters.Add(db.GetParameter("funcid", SimpleUtils.IfNull(pFuncid, -2)));
            cm.Parameters.Add(db.GetParameter("funcname", SimpleUtils.IfNull(pFuncName, String.Empty)));
           // cm.Parameters.Add(db.GetParameter("content", pOperation));
            cm.ExecuteNonQuery();
        }

        #region modi by emily(2008-07-31)
        protected static void ConvertRowToString(decimal logid, Actions action, PCTable table, Hashtable cmpRow, Hashtable row)
        {
            StringBuilder sb = new StringBuilder();
            DbFieldCollection fc = table.Fields;
            Db db = PCDb.Db;
            DbCommand cm;
            decimal change = 0;
            for (int i = 0; i < fc.Count; i++)
            {
                string key = fc[i].FieldName;
                if (row != null )
                {   
                    if(!row.ContainsKey(key))
                    continue;
                }
                if (cmpRow != null)
                {
                    if (!cmpRow.ContainsKey(key))
                        continue;
                }
                object newval = null;
                object cmpval = null;
                if (cmpRow != null && cmpRow[key].ToString()!="")
                    cmpval = cmpRow[key];
                if (row != null && row[key].ToString()!="")
                    newval = row[key];
                if (cmpval != null && (cmpval.GetType().Name == "DateTime"))
                    cmpval = returnDateFormat(cmpval);
                if (newval != null && (newval.GetType().Name == "DateTime"))
                    newval = returnDateFormat(newval);

                # region get change's value
                if (newval != null && cmpval != null)
                {
                    if (fc[i].DataType == "System.Decimal")
                    {
                       if (Convert.ToDecimal(newval) == Convert.ToDecimal(cmpval))
                            change = 0;
                        else
                            change = 1;
                    }
                    else
                    {
                        if (newval.ToString() == cmpval.ToString())
                            change = 0;
                        else
                            change = 1;
                    }
                }

                #endregion

                #region get id's name
                object NewName = null,OldName=null;
                if ((newval != null && fc[i].DataType == "System.Decimal") || (newval != null && fc[i].DataType == "System.Int32"))
                {
                    //NewName = GetNameForID(key, newval, table);
                    if (NewName!= null)
                        newval = NewName;
                }
                if ((cmpval != null && fc[i].DataType == "System.Decimal") || (cmpval != null && fc[i].DataType == "System.Int32"))
                {
                    //OldName = GetNameForID(key,cmpval,table);
                    if (OldName != null)
                        cmpval = OldName;
                }
                #endregion

                if (table != null && table.IsTransactionReady)
                {
                    cm = table.InternalCommand;
                }
                else
                {
                    cm = db.GetCommand();
                }
                cm.CommandText = BuildInsertSqlForLogDetail();
                cm.CommandType = CommandType.Text;
                cm.Parameters.Add(db.GetParameter("id", PCDb.GetNextLogID()));
                cm.Parameters.Add(db.GetParameter("logid", logid));
                cm.Parameters.Add(db.GetParameter("fieldname", key));
                cm.Parameters.Add(db.GetParameter("changed",change));
              
                if (action == ScLog.Actions.Update || action == ScLog.Actions.Delete)
                {
                    cm.Parameters.Add(db.GetParameter("oldvalue", SimpleUtils.IfNull(cmpval, string.Empty)));
                }
                else
                    cm.Parameters.Add(db.GetParameter("oldvalue", String.Empty));
                if (action == ScLog.Actions.Insert || action == ScLog.Actions.Update)
                {
                    cm.Parameters.Add(db.GetParameter("newvalue", SimpleUtils.IfNull(newval, String.Empty)));
                }
                else
                {
                    cm.Parameters.Add(db.GetParameter("newvalue", String.Empty));
                }
                cm.ExecuteNonQuery();
            }
        }
        #endregion

        public static object GetNameForID(string Key,object  IdValue,PCTable table)
        {
            DataTable dt = null;
            string  idname = "", tablename = ""; 
            object val=null;
            try
            {
                dt= PCDb.Db.ExecQuery("select * from sy_TableRelation where idfield='" + Key + "'", table.InternalCommand);
                if (dt.Rows.Count == 1)
                {
                    tablename = dt.Rows[0]["tablename"].ToString();
                    idname = dt.Rows[0]["namefield"].ToString();
                    if(tablename!="" && idname!="")
                        val = PCDb.Db.ExecScalar("select " + idname + " as name  from " + tablename + " where id=" + Convert.ToDecimal(IdValue) + "", table.InternalCommand);
                }
                return val;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        protected static object returnDateFormat(object obj)
        {
            try
            {
                object returnvalue = obj;
                if (obj != null && obj.ToString().Trim() != "")
                {
                    returnvalue = Convert.ToDateTime(obj.ToString()).ToString(Consts.DateLongFormat);
                }
                return returnvalue;
            }
            catch
            {
                return obj;
            }
        }

        //protected static string BuildContent(Actions action, PCTable table, Hashtable oldValues, Hashtable newValues)
        //{
        //    if (table != null)
        //    {
        //        switch (action)
        //        {
        //            case Actions.Insert:
        //                ConvertRowToString(action, table, newValues, null);
        //                break;
        //            case Actions.Update:
        //                ConvertRowToString(action, table, newValues, oldValues);
        //                break;
        //            case Actions.Delete:
        //                ConvertRowToString(action, table, oldValues, null);
        //                break;
        //        }
        //    }
        //}

    }//end of class
}
