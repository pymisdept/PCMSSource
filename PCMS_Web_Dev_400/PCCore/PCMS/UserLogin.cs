using System;
using System.Collections.Generic;
using System.Text;

namespace PCCore.PCMS
{
    public class UserLog
    {
        public static int Login(int userid)
        {
            int ret = 0;
            string sql = string.Format("INSERT INTO {0} (USERID,LOGIN,STATUS,SESSIONID) VALUES ({1},GETDATE(),'O','{2}')", "SC_UserLog", userid, System.Web.HttpContext.Current.Session.SessionID.ToString());
            PCCore.Common.HRLog.RecordLog(sql);
            try
            {
                ret = PCDb.Db.ExecUpdate(sql);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("UserLogin.Login", ex);
            }
            return ret;
        }

        public static int Logout(string sessionid)
        {
            PCCore.Common.HRLog.RecordLog("UserLog.Logout");
            int ret = 0;
            string sql = string.Format("UPDATE {0} SET LOGOUT=GETDATE(), STATUS = 'C' WHERE  SESSIONID = '{1}' AND LOGOUT IS NULL", "SC_USERLOG",  sessionid);
            try
            {
                ret = PCDb.Db.ExecUpdate(sql);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("UserLogin.Logout", ex);
            }
            return ret;
        }
        public static int Logout(int userid, string sessionid)
        {
            PCCore.Common.HRLog.RecordLog("UserLog.Logout");
            int ret = 0;
            string sql = string.Format("UPDATE {0} SET LOGOUT=GETDATE(), STATUS = 'C' WHERE USERID = {1} AND SESSIONID = '{2}' AND LOGOUT IS NULL","SC_USERLOG",userid,sessionid);
            PCCore.Common.HRLog.RecordLog(sql);
            try
            {
                ret = PCDb.Db.ExecUpdate(sql);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("UserLogin.Logout", ex);
            }
            return ret;
        }
        public static bool isDuplicateLogin(int userid)
        {
            bool ret = false;
            System.Data.DataTable dt;
            string sql = string.Format("SELECT 1 FROM {0} WHERE USERID = {1} AND LOGOUT IS NULL", "SC_USERLOG", userid);
            try
            {
                dt = PCDb.Db.ExecQuery(sql);
                if (dt.Rows.Count > 0)
                    ret = true;

            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("isDuplicateLogin", ex);
            }
            return ret;
        }

    }
}
