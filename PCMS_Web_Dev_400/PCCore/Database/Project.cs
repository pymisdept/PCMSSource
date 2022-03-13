using System;
using System.Collections.Generic;
using System.Text;
using System.Data;

namespace PCCore.Database
{
    
    public class Project
    {
        string projcode;
        DataTable dtProject;
        
        
        public Project(string _projcode)
        {
            this.projcode = _projcode;
            setData();
        }

        void setData()
        {
            string _sql = string.Format("SELECT * FROM CPS_View_ProjectList WHERE {0} = '{1}'",Consts.FieldSearchProj,projcode);
            try
            {
                this.dtProject = SAPDb.Db.ExecQuery(_sql);
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("Project: SetData:", ex);
            }
        }

        public string ProjectName()
        {
            //return getValue("PrjName1").ToString();
            return getValue("PrjName").ToString();

        }

        public object getValue(string fieldname)
        {
            if (dtProject != null)
            {
                if (dtProject.Rows.Count > 0)
                    return dtProject.Rows[0][fieldname];
                else
                    return "";
            }
            else
            {
                return "";
            }
            
        }
        
    }
}
