using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

namespace PCCore.Database
{
    public class User
    {
        string userid = "";
        Hashtable htRow;
        public User(string _userid)
        {
            userid = _userid;
            getInformation();
        }

        public void UpdateUserProject(string prjcode)
        {
            
            string sql = string.Format("UPDATE {0} SET ACCESSPROJECTID = Replace(ISNULL(ACCESSPROJECTID,''),',{1}','') + ',{1}' WHERE ID = {2}", "SC_User", prjcode, userid);
            try
            {
                PCDb.Db.ExecUpdate(sql);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("User: UpdateUserProject", ex);
            }
        }
        private void getInformation()
        {
            DataTable _dt;
            
            string sql = string.Format("SELECT * FROM {0} WHERE id = {1}","SC_User",userid);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                {
                    htRow = PCCore.Database.Tools.ReadRows(_dt,0);
                }
            } catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("User.GetInformation",ex);
            }

        }
        public static bool isExists(string loginname)
        {
            return isExists(loginname, null);
        }

        public static bool isExists(string loginname, string id)
        {
            DataTable _dt;
            string sql = "";
            if (id == null)
            {
                // Add Mode
                sql = string.Format("SELECT 1 FROM {0} where loginname = '{1}'", "SC_USER", loginname);

            }
            else
            {
                // Edit Mode
                sql = string.Format("SELECT 1 FROM {0} where loginname = '{1}' and id <> {2}", "SC_USER", loginname, id);


            }
            PCCore.Common.HRLog.RecordLog(sql);
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
                PCCore.Common.HRLog.RecordException("UserExists", ex);
            }
            return true;
        }

        public Object getValue(string fieldname)
        {
            if (htRow != null)
            {
                return htRow[fieldname.ToUpper()];
            }else
            {
                return null;
            }
        }
        public bool isSupervisor()
        {
            PCCore.Common.HRLog.RecordLog("isSupervisor: " + getValue("Supervisor"));
            if (getValue("SUPERVISOR") != null)
            {
                return (Convert.ToString(getValue("Supervisor")) == "1");
            } else 
            {
                return false;
            }
            
        }

        public static string getLoginName(string id)
        {
            string ret = string.Empty;
            string sql = string.Format("SELECT LOGINNAME FROM {0} WHERE ID = {1}", "SC_User", id);
            DataTable _dt;
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                {
                    ret = Convert.ToString(_dt.Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("getLoginName", ex);
            }
            return ret;
        }

        
        
    }
}
