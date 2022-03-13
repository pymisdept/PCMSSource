using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace PCCore.PCMS
{
    public class ProjectReport
    {
        string projcode = "";
        DateTime d;
        
        public ProjectReport(string _projcode, DateTime _d)
        {
            projcode = _projcode;
            d = _d;
        }

        public int CancelOldPostedPeriod(string docnum)
        {
            int ret = 0;
            string sql = string.Format("UPDATE DOCUMENTPROPERTY SET DOCSTATUS = '{0}' WHERE DOCNUM = '{1}'",Consts.DOCUMENT_CANCELED,projcode,docnum);

            try
            {
                ret = PCDb.Db.ExecUpdate(sql);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("CancelOldPostedPeriod", ex);
            }
            return ret;
        }
        public bool isPosted()
        {
            DataTable _dt;
            string sql = string.Format("SELECT * FROM {0} WHERE PrjCode = '{1}' AND DOCDATE = '{2}' AND DOCSTATUS = '{3}'","CPSMPC_RPTH",projcode,Convert.ToString(d),Consts.DOCUMENT_POSTED);
            try
            {
                _dt = SAPDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                    return true;
                else
                    return false;

            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("ProjectReport isPosted", ex);
                
            }
            return false;
        }
    }
}
