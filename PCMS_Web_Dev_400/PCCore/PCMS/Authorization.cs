using System;
using System.Collections.Generic;
using System.Text;
using System.Data;
using System.Collections;

namespace PCCore.PCMS
{

    public class Authorization
    {
        
        //public Authorization(string _funcCode,string userid,)
        public static Boolean CanAccess(string _funcCode)
        {
        //    ScInfo _scInfo = new ScInfo(_funcCode);
        //    return _scInfo.HasAccess;
            return true;
        }
        //_scInfo = new ScInfo(_funcCode);

        public static bool PageActionPermission(string functionid, string ProjCode, string ActionField)
        {
            DataTable _dtUser;
            DataTable _dtGroup;
            DataTable _dtPerm;
            string _tblName = "";
            //bool AllowAccess = false;

            //string strSql = string.Format("SELECT 1 FROM {0} WHERE funcid = {1} and id in ({2})",Consts.TableScRight,functionid,PCCore.Database.SCGroupUser.getALLIDbyUser(SessionInfo.UserId));
            // User Right
            if (SessionInfo.IsSupervisor == false)
            {
                switch (ActionField)
                {
                    case "FNEW":
                        _tblName = "CPS_View_AddPermission";
                        break;
                    case "FEDT":
                        _tblName = "CPS_View_EditPermission";
                        break;
                    case "FDEL":
                        _tblName = "CPS_View_DeletePermission";
                        break;
                    case "FQRY":
                        _tblName = "CPS_View_QueryPermission";
                        break;
                    case "FAPR":
                        _tblName = "CPS_View_ApprovePermission";
                        break;
                }


                //string strSql = string.Format("SELECT id,isNull(FALL,0) as FALL, isNull(FQRY,0) as FQRY, isNull(FNEW,0) as FNEW,isNull(FEDT,0) as FEDT,isNull(FDEL,0) as FDEL FROM {0} WHERE sctype = 'u' and funcid = {1} and id in ({2})", Consts.TableScRight, functionid, SessionInfo.UserId);
                string strSql = string.Format("SELECT 1 From {0} where fid = {1} and userid = {2} and projcode = '{3}'", _tblName, functionid, SessionInfo.UserId, ProjCode);
                //strSql += " and (isNull(FALL,0) = 1 or isNull(FQRY,0) = 1)";
                PCCore.Common.HRLog.RecordLog(strSql);
                try
                {
                    _dtPerm = PCDb.Db.ExecQuery(strSql);
                    if (_dtPerm.Rows.Count > 0)
                        return true;
                    else
                        return SessionInfo.IsSupervisor;
                }
                catch (Exception ex)
                {
                    PCCore.Common.HRLog.RecordException("PageAction", ex);
                    return false;
                }

            }
            return SessionInfo.IsSupervisor;
            //    try
            //    {
            //        _dtUser = PCDb.Db.ExecQuery(strSql);

            //        if (_dtUser.Rows.Count > 0)
            //        {
            //            // User Rights found, Check Allow or Deny, piority is FALL
            //            if (Convert.ToInt32(_dtUser.Rows[0]["FALL"]) == 1) // Allow
            //            {
            //                return true;
            //            }
            //            if (Convert.ToInt32(_dtUser.Rows[0]["FALL"]) == 2) // Deny
            //            {
            //                return false;

            //            }
            //            if (Convert.ToInt32(_dtUser.Rows[0][ActionField]) == 1) // Allow
            //            {
            //                return true;
            //            }
            //            if (Convert.ToInt32(_dtUser.Rows[0][ActionField]) == 2) // Deny
            //            {
            //                return false;

            //            }

            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        PCCore.Common.HRLog.RecordException("PageAllowAccess,USer", ex);
            //    }

            //    string strSqlGroup = string.Format("SELECT 1 FROM {0} WHERE sctype = 'g' and funcid = {1} and id in ({2})", Consts.TableScRight, functionid, PCCore.Database.SCGroupUser.getALLIDbyUser(SessionInfo.UserId));
            //    strSqlGroup += string.Format(" and (isNull(FALL,0) = 1 or (isNull(FALL,0) = 0 and isNull({0},0) = 1))",ActionField);

            //    try
            //    {
            //        _dtGroup = PCDb.Db.ExecQuery(strSqlGroup);
            //        if (_dtGroup.Rows.Count > 0)
            //            return true;
            //    }
            //    catch (Exception ex)
            //    {
            //        PCCore.Common.HRLog.RecordException("PageAllowAccess,Group", ex);
            //    }
            //}
            //return SessionInfo.IsSupervisor;
        }

        public static bool PageActionPermission(string functionid, string ActionField)
        {
            DataTable _dtUser;
            DataTable _dtGroup;
            DataTable _dtPerm;
            string _tblName = "";
            //bool AllowAccess = false;

            //string strSql = string.Format("SELECT 1 FROM {0} WHERE funcid = {1} and id in ({2})",Consts.TableScRight,functionid,PCCore.Database.SCGroupUser.getALLIDbyUser(SessionInfo.UserId));
            // User Right
            if (SessionInfo.IsSupervisor == false)
            {
                switch (ActionField)
                {
                    case "FNEW":
                        _tblName = "CPS_View_AddPermission";
                        break;
                    case "FEDT":
                        _tblName = "CPS_View_EditPermission";
                        break;
                    case "FDEL":
                        _tblName = "CPS_View_DeletePermission";
                        break;
                    case "FQRY":
                        _tblName = "CPS_View_QueryPermission";
                        break;
                    case "FAPR":
                        _tblName = "CPS_View_ApprovePermission";
                        break;
                }


                //string strSql = string.Format("SELECT id,isNull(FALL,0) as FALL, isNull(FQRY,0) as FQRY, isNull(FNEW,0) as FNEW,isNull(FEDT,0) as FEDT,isNull(FDEL,0) as FDEL FROM {0} WHERE sctype = 'u' and funcid = {1} and id in ({2})", Consts.TableScRight, functionid, SessionInfo.UserId);
                string strSql = string.Format("SELECT 1 From {0} where fid = {1} and userid = {2}", _tblName, functionid, SessionInfo.UserId);
                //strSql += " and (isNull(FALL,0) = 1 or isNull(FQRY,0) = 1)";
                PCCore.Common.HRLog.RecordLog(strSql);
                try
                {
                    _dtPerm = PCDb.Db.ExecQuery(strSql);
                    if (_dtPerm.Rows.Count > 0)
                        return true;
                    else
                        return SessionInfo.IsSupervisor;
                }
                catch (Exception ex)
                {
                    PCCore.Common.HRLog.RecordException("PageAction", ex);
                    return false;
                }

            }
            return SessionInfo.IsSupervisor;
            //    try
            //    {
            //        _dtUser = PCDb.Db.ExecQuery(strSql);

            //        if (_dtUser.Rows.Count > 0)
            //        {
            //            // User Rights found, Check Allow or Deny, piority is FALL
            //            if (Convert.ToInt32(_dtUser.Rows[0]["FALL"]) == 1) // Allow
            //            {
            //                return true;
            //            }
            //            if (Convert.ToInt32(_dtUser.Rows[0]["FALL"]) == 2) // Deny
            //            {
            //                return false;

            //            }
            //            if (Convert.ToInt32(_dtUser.Rows[0][ActionField]) == 1) // Allow
            //            {
            //                return true;
            //            }
            //            if (Convert.ToInt32(_dtUser.Rows[0][ActionField]) == 2) // Deny
            //            {
            //                return false;

            //            }

            //        }

            //    }
            //    catch (Exception ex)
            //    {
            //        PCCore.Common.HRLog.RecordException("PageAllowAccess,USer", ex);
            //    }

            //    string strSqlGroup = string.Format("SELECT 1 FROM {0} WHERE sctype = 'g' and funcid = {1} and id in ({2})", Consts.TableScRight, functionid, PCCore.Database.SCGroupUser.getALLIDbyUser(SessionInfo.UserId));
            //    strSqlGroup += string.Format(" and (isNull(FALL,0) = 1 or (isNull(FALL,0) = 0 and isNull({0},0) = 1))",ActionField);

            //    try
            //    {
            //        _dtGroup = PCDb.Db.ExecQuery(strSqlGroup);
            //        if (_dtGroup.Rows.Count > 0)
            //            return true;
            //    }
            //    catch (Exception ex)
            //    {
            //        PCCore.Common.HRLog.RecordException("PageAllowAccess,Group", ex);
            //    }
            //}
            //return SessionInfo.IsSupervisor;
        }

        public static bool PageAllowAccess(string functionid)
        {
            return PCCore.PCMS.Authorization.PageActionPermission(functionid, "FQRY");
            
        }
        public static bool PageAllowAdd(string functionid)
        {
            return PCCore.PCMS.Authorization.PageActionPermission(functionid, "FNEW");
            
        }

        public static bool AllowAdd(string prjcode, string functionid)
        {
            return PCCore.PCMS.Authorization.PageActionPermission(functionid,prjcode, "FNEW");
            
        }

        public static bool AllowEdit(string prjcode, string functionid)
        {
            return PCCore.PCMS.Authorization.PageActionPermission(functionid, prjcode, "FEDT");
            
        }
        
        public static bool AllowDelete(string prjcode, string functionid)
        {
            return PCCore.PCMS.Authorization.PageActionPermission(functionid, prjcode, "FDEL");
            
        }

        public static bool AllowQuery(string prjcode, string functionid)
        {
            return PCCore.PCMS.Authorization.PageActionPermission(functionid, prjcode, "FQRY");
            
        }
        public static bool AllowApprove(string prjcode, string functionid)
        {
            return PCCore.PCMS.Authorization.PageActionPermission(functionid, prjcode, "FAPR");
        }

    }
    

    public class FunctionRole
    {
        bool _FAll;
        bool _FQRY;
        bool _FNEW;
        bool _FEDT;
        bool _FDEL;
        bool _FAPR;
        string functionid = "";


        public FunctionRole(string _functionid)
        {
            this.functionid = _functionid;
            getRole(_functionid);
        }

        void getRole(string _functionid)
        {
            string strSql = string.Format("SELECT isNull(T1.FALL,0) as FALL,isNull(T1.FQRY,0) as FQRY,isNull(T1.FNEW,0) as FNEW,isNull(T1.FEDT,0) as FEDT,isNull(T1.FDEL,0) as FDEL, isNull(T1.FAPR,0) as FAPR FROM {0} T0 LEFT JOIN {1} T1 ON T1.ID = T0.ROLEID WHERE T0.ID = {2}", Consts.TableScFunction, Consts.TableScRole, functionid);
            PCCore.Common.HRLog.RecordLog("GetRole: " + strSql);
            DataTable _dtRole;
            DataRow _drRole;
            try
            {
                _dtRole = PCDb.Db.ExecQuery(strSql);
                if (_dtRole.Rows.Count > 0)
                {
                    _drRole = _dtRole.Rows[0];
                    _FAll = (Convert.ToInt32(_drRole["FALL"]) == 1);
                    _FQRY = (Convert.ToInt32(_drRole["FQRY"]) == 1);
                    _FNEW = (Convert.ToInt32(_drRole["FNEW"]) == 1);
                    _FEDT = (Convert.ToInt32(_drRole["FEDT"]) == 1);
                    _FDEL = (Convert.ToInt32(_drRole["FDEL"]) == 1);
                    _FAPR = (Convert.ToInt32(_drRole["FAPR"]) == 1);
                }
                else
                {
                    _FAll = false;
                    _FQRY = false;
                    _FNEW = false;
                    _FEDT = false;
                    _FDEL = false;
                    _FAPR = false;
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("getRole", ex);
            }
        }
        public bool FullRights()
        {
            return _FAll;
        }
        public bool AllowQuery()
        {
            return _FQRY;
        }
        public bool AllowAdd()
        {
            return _FNEW;
        }
        public bool AllowEdit()
        {
            return _FEDT;
        }
        public bool AllowDelete()
        {
            return _FDEL;
        }
        public bool AllowApprove()
        {
            return _FAPR;
        }

    }
}
