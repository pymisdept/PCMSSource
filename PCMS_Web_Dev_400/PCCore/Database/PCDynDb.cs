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
using System.Web.SessionState;

namespace PCCore
{
    [Serializable]
    public class PCDynDb
    {
        public PCDynDb(Db newDb)
        {
            _db = newDb;
        }
        public PCDynDb(string sessionId)
        {
            _sessionId = sessionId;
            _db = SimpleCache.Get(sessionId) as Db;
            if (_db == null)
            {
                throw new ApplicationException(String.Format("Database not available for session: {0}", _sessionId));
            }
        }
        
        protected string _sessionId;
        protected Db _db;

        public Db Db
        {
            get { return _db; }            
        }

        //const int MUTEX_MAX_WAITTIME = 60000;
        //private static Mutex _mutexID = new Mutex(false, "ID");
        //private static Mutex _mutexEmpID = new Mutex(false, "EMPID");
        //private static Mutex _mutexLogID = new Mutex(false, "LOGID");
        //private static Mutex _mutexPreBachNo = new Mutex(false, "PREBATCHNO");
        //private static Mutex _mutexActBachNo = new Mutex(false, "PREBATCHNO");
        //private static Mutex _mutexSpecialPreBachNo = new Mutex(false, "SPECIALPREBATCHNO");
        //private static Mutex _mutexSpecialActBachNo = new Mutex(false, "SPECIALPREBATCHNO");

        public DbCommand GetCommand()
        {
            return _db.GetCommand();
        }
        public DataTable ExecQuery(string sql, DbCommand cm)
        {
            return _db.ExecQuery(sql, cm);
        }

        public DataTable ExecQuery(string sql)
        {
            return _db.ExecQuery(sql);
        }

        public DataTable ExecQuery(string sql, DbParameter[] p)
        {
            return _db.ExecQuery(sql, p);
        }

        public DataTable ExecQuery(string sql, DbConnection cn)
        {
            return _db.ExecQuery(sql, cn);
        }

        public DataTable ExecQuery(string sql, DbParameter[] p, DbConnection cn)
        {
            return _db.ExecQuery(sql, p, cn);
        }

        public decimal GetNextID()
        {
            return GetNextID("ID");
        }

        public decimal GetNextID(DbCommand command)
        {
            return GetNextID("ID", command);
        }

        public decimal GetNextEmpID()
        {
            return GetNextID("EMPID");
        }

        public decimal GetNextLogID()
        {
            return GetNextID("LOGID");
        }
        public decimal GetNextImgID()
        {
            return GetNextID("IMGID");
        }

        //public static decimal GetNextPreBatchNo()
        //{
        //    return GetNextID("PREBATCHNO");
        //}

        //public static decimal GetNextPreBatchNo()
        //{
        //    return GetNextID("ACTBATCHNO");
        //}

        #region GetNextID
        public decimal GetNextID(string idName)
        {            
            return  PCDb.GetNextID(idName, _db.GetCommand());
        }

        public decimal GetNextID(string idName, DbCommand command)
        {
            return PCDb.GetNextID(idName, command);
        }
        #endregion

        #region GetNextSeqNO

        public decimal GetNextSeqNO(string TableName, string SeqColumn, string GroupColumn, string GroupValue)
        {
            return PCDb.GetNextSeqNO(TableName, SeqColumn, GroupColumn, GroupValue,_db.GetCommand());
        }
        #endregion

    }//end of class
}
