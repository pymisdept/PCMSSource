using System;
using System.Web;
using System.Collections.Generic;
using System.Collections;
using System.Text;
using System.Data;
using System.Data.Common;
using SimpleControls;
using SimpleControls.SimpleDatabase;

namespace PCCore
{
    [Serializable]
    public class PCTable : DbTable
    {
        protected ScInfo _scInfo;

        public PCTable(string tableName, ScInfo scInfo)
            : base(PCDb.Db, tableName)
        {
            _scInfo = scInfo;
        }
        public PCTable(string tableName)
            : base(PCDb.Db, tableName)
        {

        }

        public DataRow GetRow(Hashtable keys)
        {
            return this.SelectRow(keys);
        }

        public DataRow GetRow(string id)
        {
            string where = String.Format("id={0}", id);
            DataTable dt = this.HRSelect(where);
            if (dt.Rows.Count < 1) return null;
            return dt.Rows[0];
        }

        bool _autoInsertStandardFields = true;
        public bool AutoInsertStandardFields
        {
            get { return _autoInsertStandardFields; }
            set { _autoInsertStandardFields = value; }
        }

        bool _enableLog = true;
        public bool EnableLog
        {
            get { return _enableLog; }
            set { _enableLog = value; }
        }

        public DataTable HRSelect()
        {
            return HRSelect("*", null, null);
        }

        public DataTable HRSelect(string where)
        {
            return HRSelect("*", where, null);
        }

        public DataTable HRSelect(string where, string order)
        {
            return HRSelect("*", where, order);
        }

        public DataTable HRSelect(string fields, string where, string order)
        {
            string sql = BuildSelectSql(fields, where, order);

            OnSelecting(sql);
            DbCommand cm = GetProperCommand();
            DataTable dt = PCDb.Db.ExecQuery(sql, cm);
            OnSelected(dt);

            return dt;
        }

        /// <summary>
        /// 根据Where 条件 获取DataTable .row Index =0 的记录 
        /// </summary>
        /// <param name="where"></param>
        /// <returns></returns>
        public DataRow HRSelectRow(string where)
        {
            DataRow dr = null;
            DataTable dt = this.Select(where);
            if (dt != null && dt.Rows.Count >0)
            {
                dr = dt.Rows[0];
            }
            return dr;
        }

        public override void Insert(object row)
        {
            Hashtable ht = row as Hashtable;
            if (ht != null)
            {
                if (_autoInsertStandardFields)
                {
                    AssignHashTableValue(ht, Consts.FieldCreated, DateTime.Now);
                    //Karrson: Change
                    //AssignHashTableValue(ht, Consts.FieldModified, DateTime.Now);
                    AssignHashTableValue(ht, Consts.FieldCreatedBy, SessionInfo.LoginName);
                    AssignHashTableValue(ht, Consts.FieldModifiedBy, SessionInfo.LoginName);
                }

                if (_enableLog)
                {
                    ScLog.Insert(ScLog.Actions.Insert, int.Parse(_scInfo.FunctionId), _scInfo.FunctionName, this, null, ht);
                }
            }
            
            base.Insert(row);
            
        }

        protected void CheckOwner(DataRow dr)
        {
            if (_scInfo == null) return;
            switch (_scInfo.PublishType)
            {
                case Sc.PublishTypes.Private:
                case Sc.PublishTypes.PrivateUpdate:
                    string createdBy = SimpleUtils.IfNull(dr[Consts.FieldCreatedBy], String.Empty);
                    if (createdBy != SessionInfo.LoginName)
                    {

                        throw new NotOwnerException("Accessdenied");
                        //throw NotOwnerExcption();

                    }
                    break;
            }
        }

        public override void Update(object row)
        {
            Hashtable ht = row as Hashtable;
            if (ht != null)
            {
                DataRow dr = GetRow(ht);
                CheckOwner(dr);

                AssignHashTableValue(ht, Consts.FieldModified, DateTime.Now);
                AssignHashTableValue(ht, Consts.FieldModifiedBy, SessionInfo.LoginName);

                if (_enableLog)
                {
                    ScLog.Insert(ScLog.Actions.Update, int.Parse(_scInfo.FunctionId), _scInfo.FunctionName, this, GetHashtable(dr), ht);
                }
            }
            base.Update(row);
        }

        private void AssignHashTableValue(Hashtable pHt, string pKey, object pValue)
        {
            if (pHt.ContainsKey(pKey))
            {
                pHt[pKey] = pValue;
            }
            else
            {
                pHt.Add(pKey, pValue);
            }
        }

        public override void Delete(object row)
        {
            Hashtable ht = row as Hashtable;
            if (ht != null)
            {
                DataRow dr = GetRow(ht);
                if (dr == null) return;
                CheckOwner(dr);

                ScLog.Insert(ScLog.Actions.Delete, int.Parse(_scInfo.FunctionId), _scInfo.FunctionName, this, GetHashtable(dr), null);
            }
            base.Delete(row);
        }

        public override void Delete(string where)
        {
            DataTable dt = this.HRSelect(where);
            if (dt == null || dt.Rows.Count < 1) return;

            if (this.EnableLog)
            {
                DataRowCollection drc = dt.Rows;
                for (int i = 0; i < drc.Count; i++)
                {
                    ScLog.Insert(ScLog.Actions.Delete, int.Parse(_scInfo.FunctionId), _scInfo.FunctionName, this, GetHashtable(drc[i]), null);
                }
            }

            base.Delete(where);
        }

        #region Exceptions
        public class NotOwnerException : ApplicationException
        {
            //string msg = HttpContext.GetGlobalResourceObject(Consts.ResourcesMessages, "AccesDenied").ToString();
            const string msg = "Access denied. You need to be record's owner to perform this operation.";
            public NotOwnerException(string msg)
                : base(msg)
            {

            }
            public NotOwnerException(string msg, Exception inner)
                : base(msg, inner)
            {

            }
        }

        //protected Exception NotOwnerExcption()
        //{

        //    return new Exception(HttpContext.GetGlobalResourceObject(Consts.ResourcesMessages, "Accessdenied").ToString());
        //}
        #endregion Exceptions
    }//end of class
}
