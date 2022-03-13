using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace PCCore.Database
{
    
    public class DocumentProperty
    
    {
        string id = "";
        DataTable dt;
        
        public DocumentProperty(string _id)
        {
            id = _id;
            initData();
        }

        void initData()
        {
            string sql = string.Format("SELECT * FROM {0} where {1} = {2}", Consts.Table_DocumentProperty, Consts.FieldID, id);
            try
            {
                dt = PCDb.Db.ExecQuery(sql);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("DocumentProperty", ex);
            }
        }

        public object getValue(string _fieldname)
        {
            if (dt != null && dt.Rows.Count > 0)
            {
                return dt.Rows[0][_fieldname];
            } else {
                return "";
            }
        }

        public string FunctionCode()
        {
            return getValue("Type").ToString();
        }

        public static string FindID(string field, string value)
        {
            string id = "";
            DataTable _dt;
            string sql = string.Format("SELECT TOP 1 ID FROM {0} WHERE {1} = '{2}'",Consts.Table_DocumentProperty,field,value);
            try
            {
                _dt = PCDb.Db.ExecQuery(sql);
                if (_dt.Rows.Count > 0)
                {
                    id = Convert.ToString(_dt.Rows[0][0]);
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("DocuemntProperty,FindID", ex);
            }
            return id;
        }
    }
}
