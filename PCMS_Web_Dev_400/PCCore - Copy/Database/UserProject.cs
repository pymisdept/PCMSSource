using System;
using System.Collections.Generic;
using System.Text;
using System.Collections;

namespace PCCore.Database
{
    public class UserProject
    {
        public static string tablename = "SC_UserProject";
        public static bool SaveByGroup(ArrayList _alProject, ArrayList _alUser, string groupid)
        {
            PCTable table = new PCTable(UserProject.tablename);
            table.AutoInsertStandardFields = false;
            table.BeginTransaction();

            PCCore.Common.HRLog.RecordLog("alUser Count: " + _alUser.Count);
            // Clear Group User Project
            UserProject.DeleteByGroup(groupid); 
            foreach (object o in _alUser.ToArray())
            {
                //UserProject.Delete(o.ToString(), "g");
                foreach (object _o in _alProject.ToArray())
                {
                    if (!UserProject.Exists(o.ToString(), _o.ToString()))
                    {
                        //string strSql = string.Format("Insert Into {0} (userid,AssignFrom,PrjCode) Values ('{1}','{2}','{3}')", UserProject.tablename, o.ToString(), "g", _o.ToString());
                        string strSql = string.Format("Insert Into {0} (userid,AssignFrom,PrjCode,GRPID) Values ('{1}','{2}','{3}',{4})", UserProject.tablename, o.ToString(), "g", _o.ToString(),groupid);

                        //Hashtable _ht = new Hashtable();
                        //_ht.Add("userid", o.ToString());
                        //_ht.Add("AssignFrom", "g");
                        //_ht.Add("PrjCode", _o.ToString());
                        try
                        {
                            //table.Insert(_ht);
                            PCDb.Db.ExecUpdate(strSql);
                        }
                        catch (Exception ex)
                        {
                            PCCore.Common.HRLog.RecordException("SavebyGroup UserProject", ex);
                            table.RollbackTransaction();
                            return false;
                        }
                    }
                }

            }
            table.CommitTransaction();

            return true;
        }
        public static bool SaveByUser(ArrayList _alProject,ArrayList _alGroup, string userid)
        {
            PCTable table = new PCTable(UserProject.tablename);
            table.AutoInsertStandardFields = false;
            table.BeginTransaction();
            
            //UserProject.Delete(userid, "u");
            
            //foreach (object o in _alProject.ToArray())
            //{
            //    if (!UserProject.Exists(userid, o.ToString()))
            //    {
            //        string strSql = string.Format("Insert Into {0} (userid,AssignFrom,PrjCode) Values ('{1}','{2}','{3}')", UserProject.tablename, userid, "u", o.ToString());
            //        //Hashtable _ht = new Hashtable();
            //        //_ht.Add("userid", userid);
            //        //_ht.Add("AssignFrom", "u");
            //        //_ht.Add("PrjCode", o.ToString());
            //        try
            //        {
            //            PCDb.Db.ExecUpdate(strSql);
            //            //table.Insert(_ht);
            //        }
            //        catch (Exception ex)
            //        {
            //            PCCore.Common.HRLog.RecordException("Save UserProject", ex);
            //            table.RollbackTransaction();
            //            return false;
            //        }
            //    }
            //}
            UserProject.Delete(userid, "g");
            // Get Group Project
            foreach (object o in _alGroup.ToArray())
            {
                ArrayList __alProject = UserProject.GroupProjectArray(o.ToString());
                PCCore.Common.HRLog.RecordLog(o.ToString() + ": Project Array: " + __alProject.Count);
                foreach (object _o in __alProject.ToArray())
                {
                    PCCore.Common.HRLog.RecordLog("Group Code: " + o.ToString() + ", + Group Project Code: " + _o.ToString());
                    PCCore.Database.User u;
                    if (!UserProject.Exists(userid,o.ToString()))
                    {
                        
                        string strSql = string.Format("Insert Into {0} (userid,AssignFrom,PrjCode,GRPID) Values ('{1}','{2}','{3}','{4}')", UserProject.tablename, userid, "g", _o.ToString(),o.ToString());
                        try
                        {
                            //table.Insert(_ht);
                            PCDb.Db.ExecUpdate(strSql);
                            u = new User(userid);
                            u.UpdateUserProject(_o.ToString());
                        }
                        catch (Exception ex)
                        {
                            PCCore.Common.HRLog.RecordException("Save UserProject", ex);
                            return false;
                        }
                    }

                }


            }
            PCCore.Common.HRLog.RecordLog("Commit Transaction");
            table.CommitTransaction();
            return true;
        }
        public static int DeleteByGroup(string groupid)
        {
            int ret = -1;
            string strSql = "";
            System.Data.DataTable _dt;
            strSql = string.Format("DELETE From {0} where grpid = {1}",UserProject.tablename,groupid);
            PCCore.Common.HRLog.RecordLog(strSql);
            try
            {
                ret = PCDb.Db.ExecUpdate(strSql);
                
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("DeleteByGroup", ex);
                return ret;
            }
            return ret;
            
        }

        public static int Delete(string userid, string type)
        {
            int ret = -1;
            string strSql = "";
            
            strSql = string.Format("DELETE FROM {0} where userid = {1} and AssignFrom = '{2}'",UserProject.tablename, userid, type);
            PCCore.Common.HRLog.RecordLog(strSql);

            try
            {

                ret = PCDb.Db.ExecUpdate(strSql);
            }
            catch (Exception ex)
            { PCCore.Common.HRLog.RecordException("Delete UserProject", ex); }
            return ret;
        }
        public static bool Exists(string userid, string prjcode)
        {
            System.Data.DataTable _dt;
            string strSql = string.Format("SELECT 1 FROM {0} where userid = {1} and prjcode = '{2}'", UserProject.tablename, userid, prjcode);
            try
            {
                _dt = PCDb.Db.ExecQuery(strSql);
                if (_dt.Rows.Count > 0)
                    return true;
                else
                    return false;
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("UserProject Exists", ex);
                return true;
            }
        }
        public static ArrayList UserProjectArray(string userid)
        {
            System.Data.DataTable _dt;
            
            ArrayList _alProject = new ArrayList();
            string[] ProjectCode;
            string strSql = string.Format("SELECT prjcode From {0} where userid = {1}  ", "SC_UserProject", userid);
            try
            {
                _dt = PCDb.Db.ExecQuery(strSql);

                if (_dt.Rows.Count > 0)
                {
                    
                    foreach (System.Data.DataRow _dr in _dt.Rows)
                    {
                        _alProject.Add(Convert.ToString(_dr[0]));
                    }
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("UserProjectArray", ex);
            }
            return _alProject;
        }
        public static ArrayList GroupProjectArray(string groupid)
        {
            System.Data.DataTable _dt;
            ArrayList _alProject = new ArrayList();
            string[] ProjectCode;
            string strSql = string.Format("SELECT isNull(ACCESSProjectID,'') From {0} where id = {1} ","SC_Group",groupid);
            try
            {
                _dt = PCDb.Db.ExecQuery(strSql);
                if (_dt != null)
                {
                    
                    if (_dt.Rows[0][0].ToString() != String.Empty)
                    {
                        PCCore.Common.HRLog.RecordLog("Project Code in AccessProjectID: " + _dt.Rows[0][0].ToString());
                        ProjectCode = Convert.ToString(_dt.Rows[0][0]).Split(Convert.ToChar(","));
                        foreach (string s in ProjectCode)
                        {
                            _alProject.Add(s);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                PCCore.Common.HRLog.RecordException("GroupProjectArray", ex);
            }
            return _alProject;
        }
        
    }
    
}
