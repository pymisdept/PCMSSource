using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace PCCore.Database
{
    public class Payment
    {
        public Decimal TotalCertedAmount;
        public Decimal TotalAppAmount;
        public string prjcode;
        public Payment(string prjcode)
        {
            this.prjcode = prjcode;
        }

        public Boolean CurrentCertedAmount()
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}","*","v_CertedAmount","Project = '" + prjcode + "'");
            DataTable _dt = SAPDb.Db.ExecQuery(sql);
            try
            {
                if (_dt.Rows.Count > 0) 
                {
                    TotalCertedAmount = Convert.ToDecimal(_dt.Rows[0]["CertedAmt"]);

                }
                return true;
            } catch (Exception ex) 
            {
                TotalCertedAmount = 0;
                return false;
            }
        }

        public Boolean CurrentAppAmount()
        {
            string sql = string.Format("SELECT {0} FROM {1} WHERE {2}", "*", "v_AppAmount", "Project = '" + prjcode + "'");
            DataTable _dt = SAPDb.Db.ExecQuery(sql);
            try
            {
                if (_dt.Rows.Count > 0)
                {
                    TotalAppAmount = Convert.ToDecimal(_dt.Rows[0]["AppAmt"]);

                }
                return true;
            }
            catch (Exception ex)
            {
                TotalAppAmount = 0;
                return false;
            }
            
        }
        
    }
}
