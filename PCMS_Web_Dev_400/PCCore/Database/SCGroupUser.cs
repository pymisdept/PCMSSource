using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace PCCore.Database
{
    public class SCGroupUser
    {
        public static string tablename = "SC_GroupUser";

        public static ArrayList FindUser(string groupid)
        {
            ArrayList _al = new ArrayList();
            System.Data.DataTable _dt;
            string strSql = string.Format("SELECT userid FROM {0} where groupid = {1} ",SCGroupUser.tablename,groupid);
            try
            {
                _dt = PCDb.Db.ExecQuery(strSql);
                if (_dt != null)
                {
                    foreach (System.Data.DataRow _dr in _dt.Rows)
                    {
                        _al.Add(_dr[0].ToString());
                    }
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("FindUser", ex);
            }
            return _al;

        }
        public static string getALLIDbyUser(string userid)
        {
            string id = userid;
            System.Data.DataTable _dt;
            string strSql = string.Format("SELECT GROUPID FROM {0} WHERE USERID = {1}", Consts.TableScGroupUser, userid);
            try
            {

                _dt = PCDb.Db.ExecQuery(strSql);
                if (_dt.Rows.Count > 0)
                {
                    foreach (System.Data.DataRow _dr in _dt.Rows)
                    {
                        id += "," + _dr[0].ToString();
                    }
                    
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("getAllIDByUser", ex);
            }
            return id;
        }
    }
}
