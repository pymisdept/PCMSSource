using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

namespace PCCore.Database
{
    public class SCRight
    {
        public static DataRow Find(string userid, string functionid, string projcode, string type)
        {
            DataTable _dt;
            string strSql = string.Format("SELECT * FROM {0} WHERE FUNCID = {1} AND SCTYPE = '{2}' AND ID = {3} AND PROJCODE = '{4}'",Consts.TableScRight,functionid,type,userid,projcode);
            PCCore.Common.HRLog.RecordLog(strSql);
            try
            {
                _dt = PCDb.Db.ExecQuery(strSql);
                if (_dt.Rows.Count > 0)
                {
                    return _dt.Rows[0];
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Find SCRight", ex);
                return null;
            }
        }
    }
}
