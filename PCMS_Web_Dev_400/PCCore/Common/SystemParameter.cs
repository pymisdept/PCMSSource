using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Data;
using SimpleControls.SimpleDatabase;
using SimpleControls;

namespace PCCore
{
    public class SystemParameter
    {
        #region static members
        private static Dictionary<string, SystemParameter> _dbParameters = new Dictionary<string, SystemParameter>();

        public static SystemParameter Get(Db db)
        {
            if (!_dbParameters.ContainsKey(db.ConnectionString))
            {
                _dbParameters.Add(db.ConnectionString, new SystemParameter(db));
            }
            return _dbParameters[db.ConnectionString];
        }

        public static void Clear(Db db)
        {
            if (_dbParameters.ContainsKey(db.ConnectionString))
            {
                _dbParameters.Remove(db.ConnectionString);
            }
        }
        #endregion static members

        #region basic
        private DataTable _dt;
        

        /// <summary>
        /// Please use Get(),New this use for Testing 
        /// </summary>
        /// <param name="db"></param>
        private SystemParameter(Db db)
        {
            _dt = db.ExecQuery("select id,code,value from sy_systemsetting");
        }
        internal DataTable Table
        {
            get { return _dt; }
        }

        private string GetValue(int id)
        {
            DataView dv = new DataView(_dt, String.Format("id={0}", id), null, DataViewRowState.CurrentRows);
            if (dv.Count > 0)
            {
                return SimpleUtils.IfNull(dv[0]["value"], String.Empty);
            }
            return String.Empty;
        }

        private string GetValue(string code)
        {
            DataView dv = new DataView(_dt, String.Format("code='{0}'", code), null, DataViewRowState.CurrentRows);
            if (dv.Count > 0)
            {
                return SimpleUtils.IfNull(dv[0]["value"], String.Empty);
            }
            return String.Empty;
        }

        public string GetString(int id, string defaultValue)
        {
            string s = GetValue(id);
            if (String.IsNullOrEmpty(s))
                return defaultValue;
            return s;
        }
        public int GetInt(int id, int defaultValue)
        {
            string s = GetValue(id);
            int i;
            if (int.TryParse(s, out i))
                return i;
            return defaultValue;
        }

        public decimal GetDecimal(int id, decimal defaultValue)
        {
            string s = GetValue(id);
            decimal d;
            if (decimal.TryParse(s, out d))
                return d;
            return defaultValue;
        }

        public bool GetBool(int id, bool defaultValue)
        {
            string s = GetValue(id);
            if (s.Equals("1")) return true;
            if (s.Equals("0")) return false;
            bool b;
            if (bool.TryParse(s, out b))
                return b;
            return defaultValue;
        }
        #endregion basic

        #region Parameters


        public int MaxLoginTimes
        {
            get { return GetInt(301, 5); }
        }

        public int MinPasswordLength
        {
            get { return GetInt(302, 1); }
        }


        #endregion Parameters

    }//end of class
}
