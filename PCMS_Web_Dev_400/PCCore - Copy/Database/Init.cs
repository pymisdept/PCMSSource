using System;
using System.Collections.Generic;
using System.Text;
using System.Web;
using System.Web.UI.WebControls;
using System.Threading;
using System.Data;
using System.Data.Common;
using SimpleControls.SimpleDatabase;
using SimpleControls.Web;
using SimpleControls;

namespace PCCore.Database
{
    public class Init
    {
        #region Init
        /// <summary>
        /// 
        /// </summary>
        /// <param name="tablename"></param>
        /// <returns></returns>
        public static DataTable InitDataTable(string tablename)
        {

            return PCDb.Db.ExecQuery(string.Format("SELECT * FROM {0} WHERE {1}", tablename, " 1 = 2"));
        }
        #endregion




    }
}
