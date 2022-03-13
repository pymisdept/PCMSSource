using System;
using System.Data;
using System.Text;
using SimpleControls.SimpleDatabase;
using SimpleControls;
using System.Collections;

namespace PCCore
{
	public class ScInfo
	{
		protected string _loginName;
		protected string _funcName;
		protected string _funcCode;
		protected string _funcId;
		protected string _moduleId;
		protected string _moduleCode;
		protected string _moduleName;
        protected string _showstar;
		protected bool _isSuper;
        protected bool _isLogin;

		//the init value are very important
		protected bool _loginRequired = true;
		protected Sc.FunctionStates _fall = Sc.FunctionStates.Denied;
		protected Sc.FunctionStates _fnew = Sc.FunctionStates.Denied;
		protected Sc.FunctionStates _fedt = Sc.FunctionStates.Denied;
		protected Sc.FunctionStates _fqry = Sc.FunctionStates.Denied;
		protected Sc.FunctionStates _fdel = Sc.FunctionStates.Denied;
        protected Sc.PublishTypes _fpub = Sc.PublishTypes.PrivateUpdate;

		const string SQL_RIGHT = "select * from v_sc_rights where (fcode='{0}' or fcode='all') and (mcode='{1}' or mcode='all') and loginname='{2}'";
		//const string SQL_FUNCTION = "select * from v_sc_functions where fcode='{0}'";
        const string SQL_FUNCTION = "select * from v_sc_functions where fcode='{0}'";

		public ScInfo(string funcCode)
		{
			_funcCode = funcCode;
            _funcName = funcCode;
            _loginName = SessionInfo.LoginName;
            _isSuper = SessionInfo.IsSupervisor;
            _isLogin = SessionInfo.IsLogin;

            //SystemParameter sp = SystemParameter.Get(PCDb.Db);

            ////for compatible only, don't use this in future
            //object o = SimpleCache.Get(ComFunction2.SysSystemSetting);
            //if (o == null)
            //{
            //    SimpleCache.Insert(ComFunction2.SysSystemSetting, sp.Table, null, DateTime.Now.AddHours(2), SimpleCache.NoSlidingExpiration);
            //}

            //_showstar = sp.ShowRequiredStar ? "1" : "0";
            //_autostaffid = sp.AutoStaffId ? "1" : "0";
            //_defaultcurrency = sp.DefaultCurrency.ToString();
            //_sysCompany = sp.Company;

            //SessionInfo.FinancialYearDateBegin = sp.FinancialYearBeginDate;
            //SessionInfo.FinancialYearDateEnd = sp.FinancialYearEndDate;
            //SessionInfo.DefaultSysCompany = _sysCompany;
            //SessionInfo.NewStaffJoinDateDefalutNow = sp.NewStaffJoinDateDefaultNow == 1;
            //SessionInfo.DefaultCurrencyCode = sp.DefaultCurrencyCode;


			string sql = String.Format(SQL_FUNCTION, funcCode);
            PCCore.Common.HRLog.RecordLog("Permission Query in ScInfo: " + sql);
            DataTable dt = PCCore.PCDb.Db.ExecQuery(sql);
			if(dt==null || dt.Rows.Count!=1) return;

            DataRow dr = dt.Rows[0];
			_funcId = dr["ID"].ToString();
			_funcName = dr["FNAME"].ToString();
			_moduleId = dr["MID"].ToString();            
			_moduleCode = dr["MCODE"].ToString();
			_moduleName = dr["MNAME"].ToString();
            _fpub = Sc.ConvertToPublishType(dr["FPUB"]);

            _loginRequired = Sc.ConvertIntegerToBoolean(dr["FLOGIN"]);

            
                //因For Web 's check box 没有 Allow, Denine ,Note 三态的表示，所以就只 判断1和0 ; --add by jawance 2006-11-03 
                _fall = Sc.ConvertAvailableStateToFunctionState(dr["FALL"]);
                _fnew = Sc.ConvertAvailableStateToFunctionState(dr["FNEW"]);
                _fedt = Sc.ConvertAvailableStateToFunctionState(dr["FEDT"]);
                _fdel = Sc.ConvertAvailableStateToFunctionState(dr["FDEL"]);
                

            if (_loginRequired)
            {
                _fqry = Sc.ConvertAvailableStateToFunctionState(dr["FQRY"]);
            }
            else
            {
                _fqry = Sc.FunctionStates.Allowed;
            }

            
			dt = null;
			if(_isLogin) 
			{
				sql = String.Format(SQL_RIGHT, funcCode, _moduleCode, _loginName);
                PCCore.Common.HRLog.RecordLog("SQL about rights: " + sql);
                dt = PCCore.PCDb.Db.ExecQuery(sql);
				if(dt==null || dt.Rows.Count < 1)
					return;
			
				Sc.FunctionStates fs;
				for(int i=0; i<dt.Rows.Count; i++) 
				{
					fs = Sc.ConvertToFunctionState(dt.Rows[i]["FALL"]);
                    
                    
                    PCCore.Common.HRLog.RecordLog("fnew: ");
                    PCCore.Common.HRLog.RecordLog(_fnew);
					//if(fs > _fall) _fall = fs;
                    // new logic
                    _fall = fs;
					fs = Sc.ConvertToFunctionState(dt.Rows[i]["FNEW"]);
                    PCCore.Common.HRLog.RecordLog("fs new: ");
                    PCCore.Common.HRLog.RecordLog(fs);

					//if(fs > _fnew)
                    // new logic
                        _fnew = fs;
                    
					fs = Sc.ConvertToFunctionState(dt.Rows[i]["FEDT"]);
					//if(fs > _fedt) _fedt = fs;
                    // new logic
                    _fedt = fs;
					fs = Sc.ConvertToFunctionState(dt.Rows[i]["FDEL"]);
					//if(fs > _fdel) _fdel = fs;
                    // new logic
                    _fdel = fs;
					fs = Sc.ConvertToFunctionState(dt.Rows[i]["FQRY"]);
					//if(fs > _fqry) _fqry = fs;
                    // new logic
                    _fqry = fs;
                    /*
                    fs = Sc.ConvertToFunctionState(dt.Rows[i]["FALL"]);
                    if (fs = Sc.FunctionStates.Allowed)
                    {
                        _fnew = fs;
                        _fedt = fs;
                        
                    }
                     */
				}// end for
                
                PCCore.Common.HRLog.RecordLog("FNew: ");
                PCCore.Common.HRLog.RecordLog(_fnew);
			}
           
		}
      
        public bool IsSupervisor 
		{
			get { return _isSuper; }
		}
		public bool LoginRequired 
		{
			get { return _loginRequired; }
		}
		public Sc.FunctionStates FunctionStateAll 
		{
			get { return _fall; }
		}
		public Sc.FunctionStates FunctionStateEdit 
		{
			get { return _fedt; }
		}
		public Sc.FunctionStates FunctionStateNew
		{
			get { return _fnew; }
		}
		public Sc.FunctionStates FunctionStateDelete
		{
			get { return _fdel; }
		}
		public Sc.FunctionStates FunctionStateQuery 
		{
			get { return _fqry; }
		}
        public Sc.PublishTypes PublishType
        {
            set { _fpub = value; }
            get { return _fpub; }
        }
		public bool HasAccess 
		{
			get 
			{
				return CanDo(_fqry);
			}
		}
		public bool FullAccess 
		{
			get 
			{
				return CanDo(_fnew) && CanDo(_fedt) && CanDo(_fqry) && CanDo(_fdel);
			}
		}

        public bool CanDetail
        {
            get
            {
                return CanDo(_fedt);
            }
        }
        public bool CanBack
        {
            get
            {
                return true ;
            }
        }

		public bool CanNew 
		{
			get 
			{
				return CanDo(_fnew);
			}
		}
		public bool CanEdit
		{
			get 
			{
				return CanDo(_fedt);
			}
		}
		public bool CanQuery
		{
			get 
			{
				return CanDo(_fqry);
			}
		}
		public bool CanDelete
		{
			get 
			{
				return CanDo(_fdel);
			}
		}
		public bool CanSave
		{
			get 
			{
				return CanDo(_fnew) || CanDo(_fedt) || CanDo(_fdel);
			}
            //Karrson: Allow to assign CanSave to Security
            set
            {
                CanSave = value;
            }
		}
		private bool CanDo(Sc.FunctionStates fs) 
		{
            if (_isLogin)
            {
                return _isSuper || (_fall != Sc.FunctionStates.Denied && fs != Sc.FunctionStates.Denied &&
                    (_fall == Sc.FunctionStates.Allowed || fs == Sc.FunctionStates.Allowed));
            }
            else
            {
                return fs == Sc.FunctionStates.Allowed;
            }
		}

		public string ModuleId 
		{
			get { return _moduleId; }
		}
        
		public string ModuleName 
		{
			get { return _moduleName; }
		}
        public string ShowStar
        {
            get { return _showstar; }
        }
        
		public string FunctionId 
		{
			get { return _funcId; }
		}
		public string FunctionName 
		{
			get { return _funcName; }
		}
		public string FunctionCode 
		{
			get { return _funcCode; }
		}

        public string BuildScript()
        {
            StringBuilder sb = new StringBuilder();
            sb.AppendFormat("var ScCanNew={0};", (this.CanNew ? "true" : "false"));
            sb.AppendFormat("var ScCanEdit={0};", (this.CanEdit ? "true" : "false"));
            sb.AppendFormat("var ScCanDelete={0};", (this.CanDelete ? "true" : "false"));
            sb.AppendFormat("var ScCanSave={0};", (this.CanSave ? "true" : "false"));
            sb.AppendFormat("var ScLoginName='{0}';", this._loginName);
            sb.Append("</script>");
            return sb.ToString();
        }

	} //end of class
}
