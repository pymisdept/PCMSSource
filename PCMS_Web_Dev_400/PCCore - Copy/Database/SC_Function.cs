using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace PCCore.Database
{
    
    public class SC_Function
    
    {
        string id = "";
        DataTable dt;
        DataTable dtRole;
        bool isProjectFunction;
        public SC_Function(string _id)
        {
            id = _id;
            initData();
        }

        void initData()
        {
            
            string sql = string.Format("SELECT * FROM {0} where {1} = {2}", Consts.Table_SCFunction, Consts.FieldID, id);
            try
            {
                dt = PCDb.Db.ExecQuery(sql);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("SC_Functon, GetFunction", ex);
            }
            
            string sqlRole = string.Format("SELECT T1.ProjectFunc FROM {0} T0 inner join {1} T1 on T1.id = T0.RoleID where {2} = {3}", Consts.Table_SCFunction,Consts.Table_SCRole, "T0.id", id);
            try
            {
                dtRole = PCDb.Db.ExecQuery(sqlRole);
                if (dtRole.Rows.Count > 0)
                {
                    try
                    {
                        isProjectFunction = (Convert.ToInt32(dtRole.Rows[0]["ProjectFunc"]) == 1);
                    }
                    catch (Exception ex2)
                    { isProjectFunction = false; }
                }

            }
            catch (Exception ex1)
            {
                PCCore.Common.HRLog.RecordException("SC_Function, GetRole", ex1);
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

        public bool ProjectFunction()
        {
            return isProjectFunction;
        }
        public string FunctionCode()
        {
            return getValue(Consts.FieldCode).ToString();
        }
        public string FunctionName()
        {
            return getValue(Consts.FieldName).ToString();
        }
    }
}
