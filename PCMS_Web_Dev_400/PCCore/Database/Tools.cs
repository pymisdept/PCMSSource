using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;
using System.Data;

namespace PCCore.Database
{
    public class Tools
    {
        public static Hashtable ReadRows(DataTable _dt, int index)
        {
            Hashtable row = new Hashtable();
            if (_dt.Columns.Count > 0 && _dt.Rows.Count > 0)
            {

                foreach (DataColumn _dc in _dt.Columns)
                {
                    row.Add(_dc.ColumnName.ToUpper(), _dt.Rows[index][_dc.ColumnName]);
                }
                return row;
            }
            else
                return null;
        }
        public static Hashtable CreateHashTable(DataTable _dt)
        {
            Hashtable row = new Hashtable();
            if (_dt.Columns.Count > 0)
            {
                foreach (DataColumn _dc in _dt.Columns)
                {
                    row.Add(_dc.ColumnName.ToUpper(), String.Empty);
                }
            }
            return row;
        }

        public static string GenerateInsertQuery(Hashtable row,string Tablename)
        {
            string sqlField = "";
            string sqlValue = "";
            
            foreach (Object o in row.Keys)
            {
                sqlField += o.ToString() + ",";
                sqlValue += "'" + Convert.ToString(row[o]) + "',";
            }
            sqlField = sqlField.Substring(0, sqlField.Length - 1);
            sqlValue = sqlValue.Substring(0, sqlValue.Length - 1);
            string sql = string.Format("INSERT INTO {0} ({1}) Values ({2})", Tablename,sqlField,sqlValue);
            PCCore.Common.HRLog.RecordLog("GenerateInsertQuery:" + sql);
            return sql;
        }
        public static string GenerateUpdateQuery(Hashtable row, string Tablename, string condition)
        {
            string sqlValue = "";
            foreach (Object o in row.Keys)
            {
                sqlValue += o.ToString() + " = '" + Convert.ToString(row[o]) + "',";

            }
            sqlValue = sqlValue.Substring(0, sqlValue.Length - 1);
            string sql = string.Format("UPDATE {0} SET {1} WHERE {2}",Tablename,sqlValue,condition);
            return sql;
        }
    
    }
}
